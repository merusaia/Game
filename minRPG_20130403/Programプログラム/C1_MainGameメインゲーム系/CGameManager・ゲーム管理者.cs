using System;
using System.Collections.Generic;
using System.Text;

using Yanesdk;
using Yanesdk.Timer;
using Yanesdk.Input;
using Yanesdk.Math;
using Yanesdk.Draw;
using Yanesdk.System;
using System.Windows.Forms;
using System.Threading;

namespace PublicDomain
{
    
    #region ゲーム内で共通して使う列挙体
    /// <summary>
    /// 「速さ度合い」を定性的に評価・管理する速度情報（表示速度など，実際のミリ秒は別の共通部分で実装したい場合などに使う）の列挙体です．
    /// 
    /// （注意：なんか0や0.0(double型）でもはじめの要素が選択されるようです。enum型のはじめの要素はできるだけデフォルトで！）
    /// </summary>
    public enum ESPeed
    {
        s00_デフォルト_待ち時間なし,
        s01_超遅い＿標準で５秒,
        s02_非常に遅い＿標準で３秒,
        s03_遅い＿標準で２秒,
        s04_やや遅い＿標準で１３００ミリ秒,
        s05_普通＿標準で１秒,
        s06_やや早い＿標準で８００ミリ秒,
        s07_早い＿標準で６００ミリ秒,
        s08_非常に速い＿標準で３００ミリ秒,
        s09_超早い＿標準で１００ミリ秒,
    }
    /// <summary>
    /// リストをソートするタイプ（値が小さい順＿昇順，値が大きい順＿降順，あいうえお順など）を選択するときに使う列挙体です．
    /// </summary>
    public enum ESortType・並び替え順
    {
        昇順,
        降順,
        あいうえお順,
        文字数が大きい順,
        文字数が小さい順,
        無,
    }
    #endregion

    // 以下、ゲーム再起動に必要な処理

    #region 以下、参考メモ。ゲーム開始処理の書き方（例：ゲームを実行したいフォームのコンストラクタに、以下の様な処理を書く）

    //public CGameManager・ゲーム管理者 game;
    //public bool p_isCGameDataIlinalized・ゲーム初期化処理を実行したか = false;
    //public Form1(){
    //    InitializeComponent();

    //    // コントロールのキーイベントを，フォームのキーイベントでも受け取れるようにします．
    //    this.KeyPreview = true;

    //    // フォームのコントロール名と意味の対応付け
    //    //...

    //    // 【ゲーム初期化処理】（ゲーム終了までは一度きり！）
    //    // ●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●
    //    // (1)ゲームデータgの初期化
    //    game = new CGameManager・ゲーム管理者();
    //    // (2)ゲームデータgへ格納する，ゲーム画面の設定
    //    p_メッセージボックス = new CWinMainTextBox・メインテキストボックス(game, this, richTextBox1, listBox1, mainInputGroup・入力メッセージグループボックス, richTextInputLabel1, textInput1, richTextInputLabel2, textInput2, richTextInputLabel3, textInput3);
    //    // gameWindow・ゲーム画面().createNewWindow・画面初期化(800, 600, p_usedForm・使用フォーム);
    //    p_ゲーム画面 = new CGameWindow・ゲーム画面(game, this.Width, this.Height, this, p_メッセージボックス);

    //    // (3)ゲームデータgに，ゲーム画面を登録
    //    game.setP_gameWindow・ゲーム画面(p_ゲーム画面);
    //    // 初期化終わり（スレッドなどを開始）
    //    p_isCGameDataIlinalized・ゲーム初期化処理を実行したか = true;

    //    // (4)テストゲーム画面、その他のフォームの呼び出し
    //    p_testBalanceForm = new FTestBalanceForm(game);
    //    p_testBalanceForm.Show();
    //    Program・プログラム.p_log = new CLog(this.p_testBalanceForm.richtextBattle・戦闘);
    //    CTestGame・テストゲーム _testgame = new CTestGame・テストゲーム(game, this);
    //    // ●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●

    //    // 最後に、ゲーム開始処理を呼び出す
    //    game.Start・ゲーム開始処理();
    //}
    #endregion
    /// <summary>
    /// ゲーム内のデータで、多くのクラスが互いにやり取りするときに使われる共通データのクラスです。
    /// 他のクラスでは、インスタンス名としてよく「game.」として使います。
    /// 
    ///             
    /// </summary>
    public class CGameManager・ゲーム管理者
    {
        #region ■■ゲームオプションの静的プロパティ（頭文字にs_を付けて、いつでも変更可能なように統一。p_の動的プロパティとは、変更頻度が違う。）
        // 定数
        // [Tips]プロパティのreadonlyとstaticの違いは、staticは変更可能，readonlyは変更不可だが早い？。あとstaticプロパティはインスタンス名.ではアクセスできない。とりあえずstaticでやっている
        /// <summary>
        /// ゲームがデバッグ中の時はtrueにします。falseにすると、出来る限り不必要な処理を省いて高速化します。
        /// </summary>
        public static bool s_isDebugMode・デバッグモード = true;


        /// <summary>
        /// ゲームの入力更新処理をストップします。trueにすると、ユーザの入力操作を受け付けません。ゲーム内部の論理処理や描画処理は正常に実行されます。
        /// </summary>
        public static bool s_isStopInput・入力更新をストップ = false;
        /// <summary>
        /// ゲームの論理処理をストップするかです。trueにすると、他のサブタスクを全て止め、ゲームの入力操作受付とその処理だけに特化します。よほどのことがない限り、trueにしないでください。
        /// </summary>
        public static bool s_isStopLogic・入力更新以外の論理処理をストップ = false;
        /// <summary>
        /// ゲームの描画更新をストップするかです。trueにすると、ゲーム画面が止まります。ゲームのpauseポーズ処理の時などに使います。trueでも、内部の論理処理や、ユーザからの入力は受け付けます。
        /// </summary>
        public static bool s_isStopDraw・描画更新をストップ = false;
        /// <summary>
        /// BGMがミュートされていないかを示します。現実装では、これがfalseの場合、ボリューム0にするのではなく、BGMを再生するメソッドを呼び出すことすらしませんので注意してください。
        /// </summary>
        public static bool s_optionBGM_ON・ＢＧＭを再生する状態か = true;
        /// <summary>
        /// 効果音がミュートされていないかを示します。現実装では、これがfalseの場合、ボリューム0にするのではなく、効果音を再生するメソッドを呼び出すことすらしませんので注意してください。
        /// </summary>
        public static bool s_optionSE_ON・効果音を再生する状態か = true;
        /// <summary>
        /// 曲を再生する時に指定したミリ秒以上再生しないと、次の曲の再生を拒否するようにする時に使う変数です。
        /// </summary>
        //private static double s_MusicMinTimePlayingMSec・曲最低持続秒 = 10.0;
        /// <summary>
        /// =1万秒。待ち時間メソッド内のエラー対処。指定秒以上は待たない
        /// </summary>
        public static int s_waitMSec_MAX = 1000000;
        /// <summary>
        /// メッセージボックスの選択肢や入力の回答を数ミリ秒毎にチェックする、時間単位ミリ秒です。（決定ボタン送りも含まれます）
        /// 
        /// 　　※なお、p_isEcoMode・省エネモードがtrueの場合、この待ち時間はユーザの無入力時間において動的に変化します。
        /// </summary>
        public static int s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒 = 200;
        /// <summary>
        /// メッセージボックスの選択肢や入力の回答を数ミリ秒毎にチェックする、時間単位ミリ秒の最小値（初期値）です。（決定ボタン送りも含まれます）
        /// </summary>
        public static int s_waitMSecForUserSelectOrInput_MIN20 = 20;
            #region エコモード時の定数
        /// <summary>
        /// エコモード時の、メッセージボックスの選択肢や入力の回答を数ミリ秒毎にチェックする、時間単位ミリ秒の最大値です。（決定ボタン送りも含まれます）
        /// </summary>
        public static int s_waitMSecForUserSelectOrInput_EcoModeStartPassedMSec5000 = 5000;
        /// <summary>
        /// エコモード時の、メッセージボックスの選択肢や入力の回答を数ミリ秒毎にチェックする、時間単位ミリ秒の最大値です。（決定ボタン送りも含まれます）
        /// </summary>
        public static int s_waitMSecForUserSelectOrInput_MAX1000 = 1000;
            #endregion
        public static int s_waitMSecForMessage1CharShown・メッセージを１文字ずつ表示する時の単位ミリ秒 = 0; // 0なら瞬速。やるなら、20-30位が適切か。WindowsFormの場合、少なくし過ぎるとちらつきが酷くなるので気を付けて。
        public static int s_waitMSecForMessage1WaitChar・メッセージに句読点や点々や遅延コードが入っていた時に待つ単位ミリ秒 = 100;
        
        private static int s_UserLastInputedTime・ユーザの最後に入力した時刻_マウス移動は含まれない = 0;
        /// <summary>
        /// ユーザが最後に入力した時刻（MyTools.getNowTime()で取得したミリ秒単位の時刻）です。
        /// 何か入力があれば現在時刻にリセットされます。
        /// ※なお、ゲームボタンやキー押し離しやクリックは入力は含まれますが、マウス移動だけは入力含まれません。
        /// </summary>
        public int getUserLastInputTime・ユーザが最後に入力した時刻を取得() { return s_UserLastInputedTime・ユーザの最後に入力した時刻_マウス移動は含まれない; }
        private static int s_UserNoInputedMSec・ユーザ無入力ミリ秒_何か入力があれば０にリセット_マウス移動は含まれない = 0;
        /// <summary>
        /// ユーザの無入力時間ミリ秒です。何か入力があれば０にリセットされます。
        /// ※なお、ゲームボタンやキー押し離しやクリックは入力は含まれますが、マウス移動だけは入力含まれません。
        /// </summary>
        public int getUserNoInputMSec・ユーザ無入力ミリ秒を取得() { return s_UserLastInputedTime・ユーザの最後に入力した時刻_マウス移動は含まれない; }
        /// <summary>
        /// ユーザの無入力時間ミリ秒です。何か入力があれば０にリセットされます。
        /// game.setisEventOccured()メソッドやgame.updateFrame()メソッドなどから呼ばれます。
        /// </summary>
        public void setUserNoInputTime・ユーザ無入力時刻を設定(bool _isReset・リセットするか, int _addMsec・増加値＿リセット時は初期値)
        {
            if (_isReset・リセットするか == true)
            {
                s_UserLastInputedTime・ユーザの最後に入力した時刻_マウス移動は含まれない = MyTools.getNowTime_fast();
                s_UserNoInputedMSec・ユーザ無入力ミリ秒_何か入力があれば０にリセット_マウス移動は含まれない = 0;
            }
            s_UserNoInputedMSec・ユーザ無入力ミリ秒_何か入力があれば０にリセット_マウス移動は含まれない += _addMsec・増加値＿リセット時は初期値;
        }

        /// <summary>
        /// メッセージ描画速度や処理の待ち時間（フレームレートも短縮する）などの、ゲーム全体の速度を調整する定数です。1.0が標準倍率ですが、今はテスト用で0.5。（テスト・デバッグでは、「S（Speedy）」キーと「D（Delay）」キー長押しで調整できます。）。setアクセサはないのでこの値をそのまま変更してください。 0にすると理論的な待ち時間は0になります。
        /// </summary>
        public static double s_gameSpeed・ゲームの速さ倍率 = 0.5;
        /// <summary>
        /// ゲーム描画の速さ倍率を調整するときに少しずつ加算する値です。
        /// </summary>
        public static double s_ゲーム描画速度増減倍率 = 0.01;
        public static bool s_決定ボタン自動押し = false;
        public static int s_決定ボタン自動押しミリ秒 = 1000;
        /// <summary>
        /// メッセージを自動送り表示するとき、速さが普通のときのフレーム数です。
        /// </summary>
        public static int s_メッセージ表示フレーム数 = 15;
        /// <summary>
        /// このゲームのFPS（フレームレート、１秒間に何回描画処理を更新するか）です。あくまで最大であり、重い時は実測値は下がります。更新する場合はsetP_FPSを使用してください。
        /// </summary>
        public static int s_FPS・ゲームの最大フレームレート = 30; // 初期値はここを変えるのでもいいよ。50フレームだったらかなり遅延激しく+5msec/20msec、30フレームだったら遅延-10msec/33msec位で大丈夫みたい。
        public static int getFPS・ゲームの最大フレームレートを取得() { return s_FPS・ゲームの最大フレームレート; }

            #region その他のget/setアクセサ
        public static int s_FRAME1_MSEC・1フレームミリ秒 = (int)(1000.0 / (double)s_FPS・ゲームの最大フレームレート);
        //public static double s_FRAME1_SEC・1フレーム秒 = (1.0 / (double)s_FPS・ゲームの最大フレームレート);
        /// <summary>
        /// 1秒間に画面が更新される回数を変更します．値を変更するときは，必ずこのsetアクセサを使ってください．変更できればtrueを返します．少ないほど処理が軽くなりまが，滑らかでは無くなります．
        /// </summary>
        public bool setFPS・フレーム数(int _新フレーム数)
        {
            if (_新フレーム数 > 0 && _新フレーム数 < 61)
            {
                s_FRAME1_MSEC・1フレームミリ秒 = (int)(1000.0 / (double)_新フレーム数);
                //s_FRAME1_SEC・1フレーム秒 = (1.0 / (double)_新フレーム数);

                p_fpsManager・フレーム管理者.setFPS・ゲームの最大フレームレートを変更(_新フレーム数);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ESpeedで定義された表示速度の、実際のミリ秒を取得します。
        /// （参考： 50fps、s_gameSpeed・ゲームの速さ倍率 = 1.0、ESPeed.s04_普通で１秒（1000MSec）です。）
        /// </summary>
        /// <param name="_表示速度"></param>
        /// <returns></returns>
        public static int getMSec(ESPeed _表示速度)
        {
            double _速度倍 = 0.0;
            switch (_表示速度)
            {
                case ESPeed.s00_デフォルト_待ち時間なし:
                    _速度倍 = 0.0; break;
                case ESPeed.s09_超早い＿標準で１００ミリ秒:
                    _速度倍 = 1.00; break;
                case ESPeed.s08_非常に速い＿標準で３００ミリ秒:
                    _速度倍 = 3.00; break;
                case ESPeed.s07_早い＿標準で６００ミリ秒:
                    _速度倍 = 1.50; break;
                case ESPeed.s06_やや早い＿標準で８００ミリ秒:
                    _速度倍 = 1.25; break;
                case ESPeed.s05_普通＿標準で１秒:
                    _速度倍 = 1.00; break;
                case ESPeed.s04_やや遅い＿標準で１３００ミリ秒:
                    _速度倍 = 0.75; break;
                case ESPeed.s03_遅い＿標準で２秒:
                    _速度倍 = 0.50; break;
                case ESPeed.s02_非常に遅い＿標準で３秒:
                    _速度倍 = 0.33; break;
                case ESPeed.s01_超遅い＿標準で５秒:
                    _速度倍 = 0.20; break;
                default:
                    _速度倍 = 0.0; break;
            }
            int _BaseMSec = (int)(s_FRAME1_MSEC・1フレームミリ秒 * s_メッセージ表示フレーム数);
            int _MSec = (int)((double)_BaseMSec * s_gameSpeed・ゲームの速さ倍率 * (1.0 / _速度倍));
            _MSec = MyTools.getAdjustValue(_MSec, 0, 1000000); // 1000秒以上は待たない
            return _MSec;
        }
        /// <summary>
        /// ESpeedで定義された表示速度の、実際のフレーム数を取得します。
        /// （参考： 50fps、s_gameSpeed・ゲームの速さ倍率 = 1.0、ESPeed.s04_普通で、50フレームです。）
        /// </summary>
        public static int getFrameNum(ESPeed _表示速度)
        {
            return getMSec(_表示速度) / s_FRAME1_MSEC・1フレームミリ秒;
        }
            #endregion
            #region ゲームオプション系取得メソッドの草案（getアクセさの代わりにもっと使いやすくする？）
        //public static double getP0_ゲーム描画時間倍率() { return s_gameSpeed・ゲームの速さ倍率; }
            #endregion
        
        #endregion

        // ＝＝＝＝ ■■静的でない動的プロパティ・データ（各分野系の実現に必要なプロパティやそのset/getアクセサ）
        
        /// <summary>
        /// 他のクラスのコピペでメソッドを新しく作ることが多いため、
        /// 自分自身を意味するキーワードを一応作って、コンストラクタでgame=this;と指定しておく
        /// （新しくインスタンスは生成しないので、何もつけない（this.）のと game.は全くいっしょの意味）。
        /// </summary>
        private CGameManager・ゲーム管理者 game;

        // ■■■以下、動的プロパティ（値型）。game.p_is***で統一。s_***とは変更頻度や呼び出し方（CGameManager・ゲーム管理者.s_***）が違う。
        /// <summary>
        /// ゲームがポーズ中か（ゲーム進行と画面を一時停止している状態）
        /// </summary>
        public bool p_isPause・ポーズモード = false;
        /// <summary>
        /// ゲームがクリエーションモード中か（ユーザがゲームの中身を編集している状態）
        /// </summary>
        public bool p_isCreation・クリエーションモード = false;
        /// <summary>
        /// ゲームがスキップ中か（シナリオモードの時はメッセージスキップ中、戦闘モードの時は高速戦闘スキップ中）
        /// </summary>
        public bool p_isSkip・スキップモード = false;
        /// <summary>
        /// ゲームが自動送り中か（シナリオモードの時はメッセージ自動送り中、戦闘モードの時は自動戦闘中）
        /// ※s_決定ボタン自動押しとは若干違う意味です。これがfalseでも、s_決定ボタン自動押しがtrueかどうかはわかりません。
        /// </summary>
        public bool p_isAutoPlay・自動モード = false;
        /// <summary>
        /// ゲームが省エネモード（ＣＰＵ低負荷モード）か。ＰＣデバッグ時ではよくtrueにするが、一定フレームレートを保ちたい戦闘やタイミング入力時ではfalseにしないと厳しいかも。
        /// 
        /// ※falseの場合：　一定時間毎にupdateFrame()メソッドが呼ばれ、50or60フレームを出来るだけ守ろうとする。
        /// 　　　　　　　　通常のゲームの仕様だが、高負荷。ＣＰＵファンがうるさい。
        /// ※trueの場合：　ユーザが何かイベントを起こす（キーやボタンを押す）までupdateFrame()メソッドが呼ばれないし、
        /// 　　　　　　　　入力待ちウェイトミリ秒もユーザの入力時間が無ければ徐々に遅くなる仕様になる。
        /// </summary>
        public bool p_isEcoMode・省エネモード = true;
        /// <summary>
        /// ゲームの難易度です。
        /// </summary>
        private EGameLV・難易度 p_gameLV・難易度 = EGameLV・難易度._LVNone_Cosmos・ユーザのプレイ状況に合わせて自動調節;
        public EGameLV・難易度 getP_gameLV・難易度() { return p_gameLV・難易度; }
        /// <summary>
        /// ゲーム難易度を変更します。以前設定されていた難易度を返します。
        /// </summary>
        public EGameLV・難易度 setP_gameLV・難易度を変更(EGameLV・難易度 _gameLV・新しいゲーム難易度){EGameLV・難易度 _before = p_gameLV・難易度; p_gameLV・難易度 = _gameLV・新しいゲーム難易度; return _before; }
        // ■■■動的プロパティ（値型）、終わり

        // 以下、基本的にコンストラクタで新しいクラスを生成する、動的クラスインスタンス
            #region ■■■テストゲーム実行用のテスト系データ
        /// <summary>
        /// テストゲームの実行などを管理する、テストゲームクラスです。現段階では、ゲームはここから始まります。
        /// </summary>
        private CTestGame・テストゲーム p_testGame;
        public CTestGame・テストゲーム getP_testGame() { return p_testGame; }
        public void setP_testGame(CTestGame・テストゲーム _testGame) { p_testGame = _testGame; }
            #endregion

            #region ■■■ゲーム画面系・タスク系データ
        private CFpsManager・フレーム管理者 p_fpsManager・フレーム管理者;
        public CFpsManager・フレーム管理者 getP_fpsManager・フレーム管理者() { return p_fpsManager・フレーム管理者; }
        public CGameModeState・モードや状態遷移 p_modeState・ゲームのモードや状態遷移の管理者;
        public CTaskController・タスク管理者 p_taskController・タスク管理者;
        public CSceneController・シーン管理者<EScenes・シーン> p_sceneController・ゲームシーン管理者;
        /// <summary>
        /// メインのゲーム画面の抽象的なインスタンスである，Windows環境の場合のWin32Windowであるウィンドウです．他のＯＳには互換がありません．
        /// </summary>
        private CGameWindow・ゲーム画面 p_gameWindow・ゲーム画面;
        #region OS汎用ゲーム画面
        /// <summary>
        /// （※現在は使用を見合わせ中）メインのゲーム画面となる，ＯＳ汎用のウィンドウです．テキストボックスによる文字表示ができるかは不明です．
        /// </summary>
        /// <remarks>
        /// Linux環境の場合はGameWindowの代わりにこっちを使う。
        /// Windows版でもこれで構わないのだが、
        /// (手と栗鼠もこれなので？）、折角なのでWindowsでは
        /// Win32Windowを使ってみる。
        /// </remarks>
        private SDLWindow2DGl p_SDLWindow・ＯＳ汎用描画画面;
        #endregion
        public CGameWindow・ゲーム画面 getP_gameWindow・ゲーム画面() { return p_gameWindow・ゲーム画面; }
        public void setP_gameWindow・ゲーム画面(CGameWindow・ゲーム画面 _gameWindow_OrForm){
            p_gameWindow・ゲーム画面 = _gameWindow_OrForm;
        }
        /*
        /// <summary>
        /// メインのゲーム画面の具体的なフォームです．WindowsFromを直接扱いたいときに使用します．
        /// （将来はゲーム画面と統合予定）
        /// </summary>
        private FGameBattleForm1・バトル画面 p_gameForm・ゲーム画面フォーム;
        public FGameBattleForm1・バトル画面 getP_gameForm・ゲーム画面フォーム() { return p_gameForm・ゲーム画面フォーム; }
        public void setP_gameForm・ゲーム画面フォーム(FGameBattleForm1・バトル画面 _gameForm)
        {
            p_gameForm・ゲーム画面フォーム = _gameForm;
        }
        */
        #endregion

            #region ■■■ユーザの入力系のデータ、と入力更新スレッド（使う場合）
        /// <summary>
        /// ゲーム中の選択肢を格納するクラスです。
        /// </summary>
        private CAnswer・回答 p_selectedResult・選択結果 = new CAnswer・回答();
        public CAnswer・回答 getP_selectedResult・選択結果()
        {
            return p_selectedResult・選択結果;
        }
        /// <summary>
        /// ゲーム中の仮想ボタン（決定ボタン、バックボタンなど）であらゆる入力機器の操作を同じ様に管理したい場合に使うクラスです。
        /// 
        /// メソッドsetButtonKeysにより、マウスやキーボードなどの指定キーをオプションで簡単に変更することができます。
        /// 
        /// ■ボタンを押したかの操作は、iボタンを押したか()などを呼びだしてください。
        /// </summary>
        private CInputButton・ボタン入力定義 p_inputButton・ボタン入力;
        public CInputButton・ボタン入力定義 getP_InputButton() { return p_inputButton・ボタン入力; }
        // 現在では外部からsetしないようにしている
        //public void setP_gameInput(CInputButton・ボタン入力定義 _CInput・総合入力クラス) { p_inputButton・ボタン入力 = _CInput・総合入力クラス; }
        /// <summary>
        /// 入力したマウス操作を管理するクラスです．
        /// </summary>
        private CMouseInput・マウス入力定義 p_mouseInput・マウス入力;
        public CMouseInput・マウス入力定義 getP_mouseInput()
        {
            return p_mouseInput・マウス入力;
        }
        // 現在では外部からsetしないようにしている
        //public void setP_mouseInput(CMouseInput・マウス入力定義 _マウス入力)
        //{
        //    p_mouseInput・マウス入力 = _マウス入力;
        //}
        /// <summary>
        /// 入力したキーを管理するクラスです．
        /// </summary>
        private CMouseAndKeyBoardKeyInput・キー入力定義 p_mouseKeyBoardInput・キー入力;
        // private readonly CKey2・キー入力クラス p_key・入力キー; // こっちの方が早いが、ボタンが数個に限定される
        public CMouseAndKeyBoardKeyInput・キー入力定義 getP_keyboardInput()
        {
            return p_mouseKeyBoardInput・キー入力;
        }
        // 現在は外部からsetしないようにしている
        //public void setP_keyboardInput(CMouseAndKeyBoardKeyInput・キー入力定義 _キーボード入力)
        //{
        //    p_mouseKeyBoardInput・キー入力 = _キーボード入力;
        //}
        #endregion

            #region ■■■ユーザやＣＰＵや難易度
        private ユーザ p_user・ユーザ;
        public ユーザ user・ユーザ()
        {
            return p_user・ユーザ;
        }
        private CCPU・ＣＰＵ管理者 p_cpu・ＣＰＵ管理者;
        public CCPU・ＣＰＵ管理者 cpu・ＣＰＵ管理者() { return p_cpu・ＣＰＵ管理者; }
            #endregion


            #region ■■■プログラム専用データ通信系データ
        /// <summary>
        /// 画像や音声データを格納されている資源データのルートパス。末尾に"\\"を含むので、そのまま + _fileNameでいい。
        /// </summary>
        //public readonly string p_RootPath・ルートディレクトリ = Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス;
        #endregion

            #region ■■■グラフィクス系データ

        //public readonly TaskController p_TaskController;
        //public readonly SceneController<EScenes・シーン> p_SceneController;

        /// <summary>
        ///  高速に乱数を管理するクラスです
        /// </summary>
        //private readonly Rand p_Rand;
        //public readonly Config p_Config;


        private Screen2DGl p_Screen
        {
            get
            {
                return Platform.IsWindows ?
                    p_gameWindow・ゲーム画面.Screen
                :
                    p_SDLWindow・ＯＳ汎用描画画面.Screen
                ;
            }
        }

        #endregion

            #region ■■■サウンド系データ
        private CSoundManager・サウンド管理者 p_sound・サウンド管理者;
        public CSoundManager・サウンド管理者 getP_Sound・サウンド管理者() { return p_sound・サウンド管理者; }
        #endregion

        // クラスインスタンス、終わり
        
        // ＝＝＝＝ ■■静的でないデータ。終わり。
        


        // コンストラクタ／デストラクタ（CGameData・ゲーム受け渡しデータクラス生成時／破棄時に呼び出されるメソッド）
        #region ■■コンストラクタ・デストラクタ
        /// <summary>
        /// あらゆるクラスで使う共通データである，ゲームデータgを初期化します．
        /// 　　※ゲーム画面だけはここでは初期できないので，これを呼び出してから，
        /// 　　　呼び出し側ですぐgame.setP_CGameWindowでセットしてください．
        /// </summary>
        public CGameManager・ゲーム管理者()
        {
            game = this;

            // ●ゲーム内でよく使うクラス初期化
            //setP_gameWindow・ゲーム画面(_ゲーム画面);
            //setP_gameForm・ゲーム画面フォーム(_ゲーム画面.getP_usedFrom());
            //p_battle・戦闘 = new CBattleBase・戦闘を管理する基底クラス(this);
            p_diceBattle・ダイス戦闘 = new CBattle・戦闘(this);


            //TaskController = new TaskController();
            //SceneController = new SceneController<EScenes・シーン>();
            //SceneController.TaskFactory = new CSceneFactory・シーン生成機();

            // ■ユーザやＣＰＵ
            p_user・ユーザ = new ユーザ("ゲスト", "guest", "guest");
            p_cpu・ＣＰＵ管理者 = new CCPU・ＣＰＵ管理者();

            // ■ユーザの入力
            // ゲームで認識可能なボタンとして総合入力（とマウスとキーボード入力機器）
            // http://yanesdkdotnet.sourceforge.jp/wiki/index.php?Yanesdk.Input
            // Windows環境の場合は、Window.FormやWindow.Controlをコンストラクタで渡すと、そのFormやControlにフォーカスがあるときのみ入力をとれる。
            // また、Windows環境の場合は何もコンストラクタで指定しなければ、GetAsyncKeyStateでスキャンするのでフォーカスがなくとも入力をスキャンすることが出来る。
            p_mouseInput・マウス入力 = new CMouseInput・マウス入力定義();
            p_mouseKeyBoardInput・キー入力 = new CMouseAndKeyBoardKeyInput・キー入力定義();
            p_inputButton・ボタン入力 = new CInputButton・ボタン入力定義(p_mouseKeyBoardInput・キー入力, p_mouseInput・マウス入力, null);
            // p_inputKey = new CKey2・キー入力クラス();
            //p_Rand = new Rand();
            //p_Rand.Randomize(); // 毎回生成する乱数を変える
            //Config = new Config();

            // ■タスク（ゲームの複数スレッド・並列実行にあたる処理）
            //以下はupdateFrameで初期化するp_fpsManager・フレーム管理者 = new CFpsManager・フレーム管理者(s_FPS・ゲームの最大フレームレート);
            p_modeState・ゲームのモードや状態遷移の管理者 = new CGameModeState・モードや状態遷移();
            p_taskController・タスク管理者 = new CTaskController・タスク管理者();
            p_sceneController・ゲームシーン管理者 = new CSceneController・シーン管理者<EScenes・シーン>();
            p_sceneController・ゲームシーン管理者.TaskFactory = new CGameScenes・ゲームシーン.CSceneFactory・シーン生成機();

            // タスクの追加
            //p_taskController・タスク管理者.AddTask(new Task.SystemTask(), (int)Tasks.System);
            //p_taskController・タスク管理者.AddTask(new Task.SystemTask(), (int)ETasks・タスク一覧.入力更新);
            //p_taskController・タスク管理者.AddTask(SceneController, (int)Tasks.SceneControler);
            //p_taskController・タスク管理者.AddTask(new Task.DrawStertTask(), (int)Tasks.DrawStert);
            //p_taskController・タスク管理者.AddTask(new Task.DrawEndTask(), (int)Tasks.DrawEnd);

            // 最初のシーンを指定。
            p_sceneController・ゲームシーン管理者.JumpScene(EScenes・シーン.s01_ダイスバトル);

            // サウンド
            p_sound・サウンド管理者 = new CSoundManager・サウンド管理者(this);

            // タスクの追加
            //TaskController.AddTask(new Task.SystemTask(), (int)Tasks.System);
            //TaskController.AddTask(SceneController, (int)Tasks.SceneControler);
            //TaskController.AddTask(new Task.CDrawStartTask(), (int)Tasks.DrawStert);
            //TaskController.AddTask(new Task.CDrawEndTask(), (int)Tasks.DrawEnd);

            // 最初のシーンを指定。
            //SceneController.JumpScene(EScenes・シーン.Game);

            // ウィンドウの作成
            if (Platform.IsWindows)
            {
                //GameWindow = new GameWindow(640, 480, this);
            }
            else
            {
                //SDLWindow = new SDLWindow2DGl();
                //SDLWindow.SetCaption("NQP.NET");
                //SDLWindow.SetVideoMode(640, 480, 0);
            }

            // ゲーム内で使うパラメータの初期化
            p_AudioFileList_FullPath・ゲーム内で認識済みの全オーディオファイルリスト = 
                getAudioFileList・ゲーム中で再生可能なオーディオファイルリストを取得(true, true, true, true);

            // ■■ゲームの開始処理（スレッド開始など）
            Start・ゲーム開始処理();
        }

        /// <summary>
        /// ゲーム終了時の後始末（ここでちゃんとメモリ開放やスレッド停止などをしないと、正常に終了してくれない）
        /// </summary>
        public void DisposeGamaData・ゲーム受け渡しデータの破棄()
        {
            if (p_SDLWindow・ＯＳ汎用描画画面 != null) p_SDLWindow・ＯＳ汎用描画画面.Dispose();
            if (p_gameWindow・ゲーム画面 != null) p_gameWindow・ゲーム画面.getP_window().Dispose();

            if (p_mouseInput・マウス入力 != null) p_mouseInput・マウス入力.Dispose();
            if (p_mouseKeyBoardInput・キー入力 != null) p_mouseKeyBoardInput・キー入力.Dispose();
            //p_key・入力キー.Dispose();

            stopThread_LoopUpdateFrame();

            //TaskController.Dispose();

            SDLFrame.Quit();
        }
        #endregion



        // ＝＝＝＝ ■■ゲームのメイン処理　以下、ゲームの開始・メインルーチン・終了・時間を司るメソッド
        #region ■■ゲーム開始・終了処理
        /// <summary>
        /// ゲームを初期化する処理です。コンストラクタが呼び出します。
        /// </summary>
        public void Start・ゲーム開始処理()
        {
            // ゲーム開始時の処理。クラスの初期化はコンストラクタにほとんど書いてるので、ここではスレッドの開始などのみ。
            // ■スレッドの開始
            startThread();

        }
        /// <summary>
        /// ゲーム終了時には、必ずこのメソッドを呼び出してください。
        /// </summary>
        public void End・ゲーム終了処理()
        {
            stopThread_LoopUpdateFrame();
            // テストゲームのゲーム終了処理を呼び出す
            if (p_testGame != null)
            {
                p_testGame._ゲーム終了時の処理();
            }
            this.DisposeGamaData・ゲーム受け渡しデータの破棄();
            Program・実行ファイル管理者.End();
        }
        #endregion

        #region ■■スレッド開始・終了処理
        /// <summary>
        /// フレームを更新するスレッドです。
        /// </summary>
        private Thread p_thread_loopUpdateFrame;
        /// <summary>
        /// 必要に応じでフレーム更新スレッドをスタートします。Start・ゲーム開始処理()が呼び出します
        /// </summary>
        public void startThread()
        {
            // ■入力を定期的にUpdateするスレッドを開始
            // (a)独自スレッドを使う場合。（これをしないと、キー入力は更新されない）
            startThread_LoopUpdateFrame();
            // (b)別のスレッドでthread_updateInput常時スレッドが呼び出す入力更新処理()を呼び出す場合、
            //タイマーでやったら失敗した…よくわからない。// ここでは何もしなくていい。ただし、Formのタイマーなどで、以下の様に書く

            // (c)定期スレッドは、タスクにAddTaskすることで実現できるらしい。ただ、今はよくわからないので、定期的に呼び出している。
        }
            #region 入力更新処理のスレッド化（独自スレッドを作って実装する場合。Windows.Formでイベント発生をコントロールする場合は、このメソッドで呼び出し頻度を調節してください））
        bool p_isRun_thread_loopUpdateFrame = false;
        public void startThread_LoopUpdateFrame()
        {
            p_thread_loopUpdateFrame = new Thread(loopUpdateFrame);
            p_isRun_thread_loopUpdateFrame = true;
            p_thread_loopUpdateFrame.Start();

        }
        public void stopThread_LoopUpdateFrame()
        {
            p_isRun_thread_loopUpdateFrame = false;
            if (p_thread_loopUpdateFrame != null)
            {
                p_thread_loopUpdateFrame.Abort();
            }
            p_thread_loopUpdateFrame = null;
        }
        public void loopUpdateFrame()
        {
            while (p_isRun_thread_loopUpdateFrame == true)
            {
                // (a)スレッドが止まるまで無限ループ
                //updateFrame・一定時間毎に呼び出す入力_論理_描画_フレーム更新処理();

                // (b)省エネモード（Windows.Form依存）
                // 省エネモードの場合、Formからイベント通知をもらった時だけフレーム更新処理を呼び出す
                if(p_isEcoMode・省エネモード == false){
                    
                    updateFrame・一定時間毎に呼び出す入力_論理_描画_フレーム更新処理();
                    // 待ち時間も上記メソッド内に含まれる

                }else{
                    if (p_gameWindow・ゲーム画面 != null &&
                        p_gameWindow・ゲーム画面.getisEventOccured・イベントが起こった＿省エネモードのフレーム更新に利用() == true)
                    {
                        // イベント通知をもらった時だけフレーム更新
                        updateFrame・一定時間毎に呼び出す入力_論理_描画_フレーム更新処理();
                        // 待ち時間も上記メソッド内に含まれる
                        // イベント通知のリセット
                        p_gameWindow・ゲーム画面.setisEventOccured・イベントが起こったかを設定(false);
                    }
                    else
                    {
                        // 次にフレーム更新を確かめる時間まで待つ（無入力時間によって増減）
                        // スレッドをお休み
                        Thread.Sleep(s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒);
                        //これだとちょっと重いかもwウェイト(s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒, false);
                        //デバッグテスト。ちゃんと増加してた。
                        //MyTools.ConsoleWriteLine("※省エネモード：次のupdateFrame()を呼び出すまで " + s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒 + "ミリ秒待つ");
                    }
                }
            }
        }
            #endregion
        #endregion
        #region ■■ゲームのメインスレッド／ループ処理（updateFrameメソッドと、一定時間毎の論理処理・描画処理・待ち処理）
        // updateFrameで更新されるプロパティたち
        private int p_FramePassedMSecゲーム経過時間ミリ秒_フレーム単位 = 0;
        private int p_FramePassedMSec経過ミリ秒_フレーム単位_一秒間でリセット = 0;
        private int p_FrameNum経過フレーム数_一秒間でリセット = 0;
        private int p_FrameLogicMSecSum論理処理合計ミリ秒_一秒間でリセット = 0;
        private int p_FrameDrawMSecSum描画処理合計ミリ秒_一秒間でリセット = 0;
        private int p_FrameWaitRestMSecSumフレーム余り待ち時間合計ミリ秒_マイナスなら遅延_一秒間でリセット = 0;
        private int p_FrameAllMSecSumフレーム全時間合計ミリ秒_一秒間でリセット = 0;
        private bool p_randomBGM = false;
        private string p_updateFrameInfoフレーム更新出力情報 = "";
        /// <summary>
        /// updateFrameメソッドの返り値である、最新のフレーム更新に関する出力情報を返します。
        /// </summary>
        public string getUpdateFrameInfoフレーム更新出力情報を取得(){ return p_updateFrameInfoフレーム更新出力情報; }
        /// <summary>
        /// ゲームのメインループです。フレーム毎の論理処理や描画処理などを一括して行います。
        /// 返り値は、フレーム毎の処理時間や遅延情報などをまとめたフレーム関連出力情報をstring型で返します（現実装では、１秒に一回だけで出力に、それ以外は""を返します）。
        /// 　　※ゲームを開始時にstartThread_LoopUpdateFrame()メソッドを呼び出すか、他の常時スレッドで１フレーム毎にこのメソッドを定期的に呼び出してください。
        /// ■マウス・キーボード・ジョイパッド入力、ゲームボタンやその他入力操作を更新します。
        /// ■定期的に実行する論理処理を行い、描画処理も行います。
        /// ■
        /// </summary>
        public string updateFrame・一定時間毎に呼び出す入力_論理_描画_フレーム更新処理()
        {
            string _返り値_フレーム関連出力情報 = "";
            //      ■■■1.フレーム開始処理
            if (p_fpsManager・フレーム管理者 == null)
            {
                //  ■■1.1フレーム管理者を初期化
                p_fpsManager・フレーム管理者 = new CFpsManager・フレーム管理者(s_FPS・ゲームの最大フレームレート);

                // ゲーム開始時間を初期化
                //CFrameManeger・フレーム管理者が内部で定義しているからいらないp_gameBeginTimeゲーム開始時間 = getTime・現在時刻();
                // ゲームロード時間を初期化
                p_gameLoadTime最後のゲームデータをロード終了した時間 = getGameBeginTimeゲーム開始時間を取得();
                // ストップウォッチをとりあえずりリスタート
                StopWatch_Restart();
                // 以下はなくても第一フレームが描画されないだけかな？
                getP_fpsManager・フレーム管理者().Reset();
            }
            //これをすると毎フレーム開始毎にストップウォッチが0になり、ここで出す計算時間がこのメソッド以外の処理を考慮しなくなる。それでもいいならりリスタートして。
            //StopWatch_Restart();
            // [Test]フレーム管理者クラス内部の時間の整合性を確かめるテスト
            long _t1フレーム開始時間 = getNowTime_InUpdateFrameメインループ用の高精度なゲーム時間取得();
            
            

            //      ■■■2.フレーム論理処理
            // ○○ここに1フレーム毎に実行したい処理・メソッド群をおく
            //      ■■2.1入力更新（これを行わないとキーボードやマウスの入力が取得できない！）
            if (s_isStopInput・入力更新をストップ == false)
            {
                getP_keyboardInput().Update();
                getP_mouseInput().Update();
            }
            //      ■■2.2入力処理以外の論理処理
            if (s_isStopLogic・入力更新以外の論理処理をストップ == false)
            {
                //  ■■2.2.1. 効果音再生時間更新のフレーム処理
                updateSound・フレーム毎のＢＧＭやＳＥのループ再生やサウンド再生時間更新処理();
                //  ■■2.2.*. ボタン入力音（共通のものだけ）やスピード調節など、テスト入力処理
                // ※ＣＰＵ・メモリ節約のため、効果音再生系・画面処理はフォーカスが当たっている時のみ処理する
                if (game.isFocus・ゲーム画面にフォーカスが当たっているか() == true)
                {
                    #region 　以下、キー入力によるいろいろなテスト。ボタン入力音（共通のものだけ）、a+1、a+2による自動決定送り、s、dによるスピード調整など、

                    bool _isButtonBeepAndSpeedControlTest・ボタン入力音＿スピード調整など = true;
                    if (_isButtonBeepAndSpeedControlTest・ボタン入力音＿スピード調整など == true)
                    {
                        // ※CKeyCede.Key1は、数字の「1（「！」や「ぬ」といっしょにあるキー。テンキーの方の１ボタンも動作するかは未確認）」ボタンです。

                        // ■ボタン入力音（共通のものだけ）
                        if (isSE_Mute・効果音がミュート状態か() == false && isFocus・ゲーム画面にフォーカスが当たっているか() == true)
                        {
                            if (ibボタンを押したか_連射非対応(EInputButton・入力ボタン.a1_右) || ibボタンを押したか_連射非対応(EInputButton・入力ボタン.a2_下) || ibボタンを押したか_連射非対応(EInputButton・入力ボタン.a3_左) || ibボタンを押したか_連射非対応(EInputButton・入力ボタン.a4_上))
                            {
                                // 十字キー押した時の選択音
                                pSE(ESE・効果音._system02・選択音_ピッ);
                            }
                            if (ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b1_決定ボタン_A))
                            {
                                // 決定音
                                pSE(ESE・効果音._system01・決定音_ピリンッ);
                            }
                            if (ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b2_戻るボタン_B))
                            {
                                // 戻り音
                                pSE(ESE・効果音._system04・戻り音_フォョッ);
                            }
                            if (ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b3_コントロールボタン_Y))
                            {
                                // コントロール音
                                pSE(ESE・効果音._system06・制御音_ファンッ);
                            }
                            if (ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b4_シフトボタン_X))
                            {
                                // シフト音
                                pSE(ESE・効果音._system05・シフト音_フィンッ);
                            }
                            if (ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b9_スペースボタン_START))
                            {
                                // スペース音
                                pSE(ESE・効果音._system07・ポーズ音_ピリンッ);
                            }
                            if (ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b10_アルトボタン_SELECT))
                            {
                                // アルト音
                                pSE(ESE・効果音._system08・ヘルプ音_ピロリロリーンッ);
                            }
                        }

                        // ■ゲームスピード調節（ゲーム速度倍率の調整s,d）
                        if (ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.s) == true)
                        {
                            DEBUGデバッグ一行出力("ゲーム速度UP: " + (1.0 / s_gameSpeed・ゲームの速さ倍率) + " 倍");
                            s_gameSpeed・ゲームの速さ倍率 -= s_ゲーム描画速度増減倍率;
                            if (s_gameSpeed・ゲームの速さ倍率 < 0.01)
                            {
                                s_gameSpeed・ゲームの速さ倍率 = 0.01;
                            }
                        }
                        if (ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.d) == true)
                        {
                            DEBUGデバッグ一行出力("ゲーム速度DOWN: " + (1.0 / s_gameSpeed・ゲームの速さ倍率) + " 倍");
                            s_gameSpeed・ゲームの速さ倍率 += s_ゲーム描画速度増減倍率;
                            if (s_gameSpeed・ゲームの速さ倍率 > 10.0)
                            {
                                s_gameSpeed・ゲームの速さ倍率 = 10.0;
                            }
                        }

                        // ■決定ボタン自動押しa+1,a+2
                        if (s_決定ボタン自動押し == true)
                        {
                            if (getUserNoInputMSec・ユーザ無入力ミリ秒を取得() > s_決定ボタン自動押しミリ秒)
                            {
                                iA決定ボタンを一瞬だけ自動で押す();
                                setUserNoInputTime・ユーザ無入力時刻を設定(true, -1 * s_FRAME1_MSEC・1フレームミリ秒);
                            }
                        }
                        //ATは発生しないif (i指定キーが押されているか(KeyCode.a) == true && i指定キーが押されているか(KeyCode.ATMark) == true)
                        //LALTも発生しないif (i指定キーが押されているか(KeyCode.KeyCode308_LALT) == true)
                        if (ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.a) == true && ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.KEY1) == true)
                        {
                            if (s_決定ボタン自動押し == false)
                            {
                                p_isAutoPlay・自動モード = true;
                                s_決定ボタン自動押し = true;
                                DEBUGデバッグ一行出力("※決定ボタン自動押しON");
                            }
                        } if (ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.a) == true && ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.KEY2) == true)
                        {
                            if (s_決定ボタン自動押し == true)
                            {
                                p_isAutoPlay・自動モード = false;
                                s_決定ボタン自動押し = false;
                                DEBUGデバッグ一行出力("※決定ボタン自動押しOFF");
                            }
                        }

                        // ■サウンドボリューム調整 j,k
                        // COMMA,PERIODもダメ。つまり、CKeyboardInputが最小限のキーしか対応していないから。
                        if (ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.j))
                        {
                            p_BGMVolume_0to1000 -= 10;
                            p_BGMVolume_0to1000 = MyTools.adjustValue_From_Min_To_Max(p_BGMVolume_0to1000, 0, 1000);
                            pBGM_setVolume(p_BGMVolume_0to1000);
                            DEBUGデバッグ一行出力("サウンド音量: " + p_BGMVolume_0to1000 + "");
                        }
                        if (ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.k))
                        {
                            p_BGMVolume_0to1000 += 10;
                            p_BGMVolume_0to1000 = MyTools.adjustValue_From_Min_To_Max(p_BGMVolume_0to1000, 0, 1000);
                            pBGM_setVolume(p_BGMVolume_0to1000);
                            DEBUGデバッグ一行出力("サウンド音量: " + p_BGMVolume_0to1000 + "");
                        }

                        // ■サウンドランダム再生,1+r
                        // KeyCode.ASTERISK「*」もKeyCode.ATMark「@？」も効かない
                        if (ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.r) == true && ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.KEY1) == true)
                        {
                            p_randomBGM = true;
                            //p_nowBGMPassedMSec・現在の曲再生ミリ秒 = s_MusicMinTimePlayingMSec・曲最低持続秒 + 1;

                            // [MEMO]別スレッドでstopSound/playSoundを入れても、違うチャンネルで同時再生されるだけ。メインスレッドでやらないと・・・。
                            //game.stopBGM・ＢＧＭを一時停止();
                            pBGM(getRnadomMusic・ランダムに曲を選曲());
                            //pSE(getRnadomMusic・ランダムに曲を選曲(), 5.0, 10.0, 5.0);
                        }



                    }
                    #endregion
                }
            }
            // ○○論理処理、終わり
            // [Test]フレーム管理者クラス内部の時間の整合性を確かめるテスト
            long _t2論理終了時間 = getNowTime_InUpdateFrameメインループ用の高精度なゲーム時間取得();
            


            //      ■■■3.フレーム描画更新処理
            getP_fpsManager・フレーム管理者().updateFrame・論理処理後のフレーム更新処理と描画処理スキップ判定();
            //      ■■3.1描画スキップ判定処理
            if (getP_fpsManager・フレーム管理者().isDelay_SkipDraw・遅延により描画処理をスキップするべきか() == false)
            {
                // ※ＣＰＵ・メモリ節約のため、効果音再生系・画面処理はフォーカスが当たっている時のみ処理する
                if (game.isFocus・ゲーム画面にフォーカスが当たっているか() == true)
                {
                    // ポーズ中など画面停止中じゃなかったら、描画を更新
                    if (s_isStopDraw・描画更新をストップ == false)
                    {
                //  ■■3.2描画処理
                        // ○○ここに画面を更新したい処理・メソッド群をおく
                        draw・描画更新処理();
                        // ○○描画処理、終わり
                    }
                }
            }
            // [Test]フレーム管理者クラス内部の時間の整合性を確かめるテスト
            long _t3描画終了時間 = getNowTime_InUpdateFrameメインループ用の高精度なゲーム時間取得();

            //      ■■■4.フレーム待ち処理（余った時間だけ待つ処理、遅延していたら待たない）
            int _tR余った時間ミリ秒 = s_FRAME1_MSEC・1フレームミリ秒 - (int)(_t3描画終了時間 - _t1フレーム開始時間);
            //          ●●●実際にフレーム終了まで待つ処理
            if (_tR余った時間ミリ秒 > 0)
            {
                MyTools.wait_ByApplicationDoEvents(_tR余った時間ミリ秒);// Thread.Sleep(_tR余った時間ミリ秒); // MyTools.wait_ByApplicationDoEvents(_tR余った時間ミリ秒)にしたらDoEventsのところでSystem.StackOverflowExceptionになってフリーズする。たぶんtimer1_Trickがいっぱい呼び出されているのではなかろうか。。やはりFormアプリのタイマーイベント内でのDoEventsは怖い。
            }
            // （苦し紛れのエラー対処）WindowsFormの画面更新のため、一度はやっておく
            // これどこにいれてもやったらフリーズするApplication.DoEvents();

            // [Test]フレーム管理者クラス内部の時間の整合性を確かめるテスト
            long _t4フレーム待ち終了時間 = getNowTime_InUpdateFrameメインループ用の高精度なゲーム時間取得();
            

            //      ■■■5.フレーム終了処理
            //      ■■5.1経過時間更新処理
            // フレーム数の更新
            p_FrameNum経過フレーム数_一秒間でリセット++;
            // 一秒間毎にリセットされる経過時間（0.0～1.0）
            int _前回の経過ミリ秒 = p_FramePassedMSecゲーム経過時間ミリ秒_フレーム単位;
            p_FramePassedMSec経過ミリ秒_フレーム単位_一秒間でリセット += (int)CFpsManager・フレーム管理者.getPassedMSec・ゲーム経過時間ミリ秒を高精度に取得() -_前回の経過ミリ秒;
            // ゲーム経過時間を更新（数十時間まで格納可能）
            p_FramePassedMSecゲーム経過時間ミリ秒_フレーム単位 = (int)CFpsManager・フレーム管理者.getPassedMSec・ゲーム経過時間ミリ秒を高精度に取得();
            // 無入力時間の更新
            if (ibボタンを押し中か_連射対応() == true || getP_mouseInput().IsPress(EMouseButton.Left) == true)
            {
                setUserNoInputTime・ユーザ無入力時刻を設定(true, 0);
            }
            else
            {
                setUserNoInputTime・ユーザ無入力時刻を設定(false, s_FRAME1_MSEC・1フレームミリ秒);
            }
            // 論理処理、描画処理にかかった時間を加算
            p_FrameLogicMSecSum論理処理合計ミリ秒_一秒間でリセット += (int)(_t2論理終了時間 - _t1フレーム開始時間);//_logicMSec論理処理にかかったミリ秒;
            p_FrameDrawMSecSum描画処理合計ミリ秒_一秒間でリセット += (int)(_t3描画終了時間 - _t2論理終了時間);// _drawMSec描画処理にかかったミリ秒;
            p_FrameWaitRestMSecSumフレーム余り待ち時間合計ミリ秒_マイナスなら遅延_一秒間でリセット += (int)(s_FRAME1_MSEC・1フレームミリ秒 - (_t3描画終了時間 - _t1フレーム開始時間)); // -_遅延ミリ秒;
            p_FrameAllMSecSumフレーム全時間合計ミリ秒_一秒間でリセット += (int)(_t4フレーム待ち終了時間 - _t1フレーム開始時間); // _今回の１フレーム実測ミリ秒;
            //
            //      ■■5.2一秒毎の処理
            if (p_FramePassedMSec経過ミリ秒_フレーム単位_一秒間でリセット >= s_FRAME1_MSEC・1フレームミリ秒*s_FPS・ゲームの最大フレームレート) // 厳密には1000では無い
            {
                // ○○ここに一秒間毎に実行したい処理をかく

                if (s_isDebugMode・デバッグモード == true)
                {
                    string _１秒毎の情報 = "■■■" + (MyTools.getSisyagonyuValue((double)p_FramePassedMSecゲーム経過時間ミリ秒_フレーム単位 / 1000.0)) + "秒経過→";
                    int _fps = getFPS・ゲームの最大フレームレートを取得();
                    int _1fmsec = s_FRAME1_MSEC・1フレームミリ秒;
                    // 論理処理、描画処理にかかった時間の平均値を計算し、1秒に一回だけ表示（１フレーム毎に表示すると重くなるから）
                    _１秒毎の情報 += "" + MyTools.getShownNumber(p_FramePassedMSec経過ミリ秒_フレーム単位_一秒間でリセット, 4, true) + "msecで" + p_FrameNum経過フレーム数_一秒間でリセット + "F(フレーム)実行／";
                    //_１秒毎の情報 += "1F";
                    //_１秒毎の情報 += +_1fmsec + "msec≒";
                    _１秒毎の情報 += "実測平均" + (p_FrameAllMSecSumフレーム全時間合計ミリ秒_一秒間でリセット / _fps) + "msec（";
                    _１秒毎の情報 += "余暇" + (p_FrameWaitRestMSecSumフレーム余り待ち時間合計ミリ秒_マイナスなら遅延_一秒間でリセット) + "+"; // / _fps) + "+";
                    _１秒毎の情報 += "描画" + (p_FrameDrawMSecSum描画処理合計ミリ秒_一秒間でリセット)+"+"; // / _fps) + "+";
                    _１秒毎の情報 += "論理" + (p_FrameLogicMSecSum論理処理合計ミリ秒_一秒間でリセット)+"）"; // / _fps) + "）";
                    // 一秒間でリセットする
                    p_FrameNum経過フレーム数_一秒間でリセット = 0;
                    p_FramePassedMSec経過ミリ秒_フレーム単位_一秒間でリセット = 0;
                    p_FrameLogicMSecSum論理処理合計ミリ秒_一秒間でリセット = 0;
                    p_FrameDrawMSecSum描画処理合計ミリ秒_一秒間でリセット = 0;
                    p_FrameWaitRestMSecSumフレーム余り待ち時間合計ミリ秒_マイナスなら遅延_一秒間でリセット = 0;
                    p_FrameAllMSecSumフレーム全時間合計ミリ秒_一秒間でリセット = 0;
                    // 現在の実測フレームレートを表示
                    string _FPSInfo = getP_fpsManager・フレーム管理者().getRealFPSInfo・現在のFPSやCPU負荷等を示す情報を取得();
                    _１秒毎の情報 += _FPSInfo;
                    _１秒毎の情報 += "　最後の1フレーム開始～終了時間:" + _t1フレーム開始時間 + "～" + _t4フレーム待ち終了時間;

                    // ●１秒に一回だけ、返り値を出力
                    _返り値_フレーム関連出力情報 = _１秒毎の情報;
                    // 標準出力に表示するか
                    //DEBUGデバッグ一行出力(_１秒毎の情報);
                    // フォームラベルに表示…は、フォームのTimerに任せる
                    //getP_gameWindow・ゲーム画面().getP_usedFrom().label..はプライベートだしフォームスレッドでしか参照不可なのでここでは参照しません。.Text =  p_updateFrameInfoフレーム更新出力情報;
                }
                // ○○一秒間毎の処理、終わり
            }

            // ●返り値
            p_updateFrameInfoフレーム更新出力情報 = _返り値_フレーム関連出力情報;
            return _返り値_フレーム関連出力情報;
        }
        /// <summary>
        /// 一定時間毎にの画面を更新する、描画処理です。updateFrameメソッドが定期的に呼び出します。
        /// </summary>
        public void draw・描画更新処理()
        {
            // [TODO]まだ何もしてません。最大で10ミリ秒待つだけです。
            MyTools.wait_ByApplicationDoEvents();
        }
        #endregion

        #region ■■ゲーム時間に関する定義と、時間を扱うメソッド
        /// <summary>
        /// 現在の時間をある定義に従ったint型で取得します。現在の定義は、「システム起動後の経過ミリ秒、休止状態やスタンバイは含まれない」で、CFpsManager・フレーム管理者.getNowTimeMSec・現在のゲーム時間を高精度に取得()メソッドを使っています。
        /// </summary>
        /// <returns></returns>
        public long getTime・現在時刻()
        {
            return CFpsManager・フレーム管理者.getNowTimeMSec・現在のゲーム時間を高精度に取得();
        }
        /// <summary>
        /// 現在の時刻をある定義に従ったstring型で取得します。現在の定義は、「yyyy年MM月dd日 HH時mm分ss秒」のフォーマットを使っています。
        /// </summary>
        /// <returns></returns>
        public string getNowDate現在時刻を示す文字列()
        {
            return MyTools.getNowTime_Japanese();
        }
        /// <summary>
        /// ゲーム開始時間です。「getTime・現在時刻()」メソッドで定義された時間のlong型で返します。まだ開始していない場合は0が返ります。
        /// </summary>
        public long getGameBeginTimeゲーム開始時間を取得() { return CFpsManager・フレーム管理者.getStartTime・ゲーム開始時刻を取得(); }
        /// <summary>
        /// updateFrameスレッド更新毎に更新される、ゲーム開始時間からの経過時間ミリ秒を取得します。まだ開始していない場合は0が返ります。
        /// ※1ミリ秒単位でリアルタイムで更新される経過時間には、getGamePassedMSecを使ってください。止めたり再稼働したりしたかったら、StopWatchでもいいです。
        /// </summary>
        public int getGamePassedMSecByFrameゲーム経過時間ミリ秒をフレーム更新単位で取得() { return (CFpsManager・フレーム管理者.getStartTime・ゲーム開始時刻を取得() == -1) ? 0 : p_FramePassedMSecゲーム経過時間ミリ秒_フレーム単位; }
        /// <summary>
        /// 1ミリ秒単位でリアルタイムで更新される、ゲーム開始時間からの経過時間ミリ秒を取得します。まだ開始していない場合は0が返ります。
        /// ※フレーム計算時間など、フレーム単位で同じにしたい場合は、getGamePassedMSecByFrameを使ってください。止めたり再稼働したりしたかったら、StopWatchでもいいです。
        /// </summary>        
        public int getGamePassedMSecByRealゲーム経過時間ミリ秒を高精度に取得＿int型は２９日間まで()
        {
            return (int)(getTime・現在時刻() - getGameBeginTimeゲーム開始時間を取得());
        }
        /// <summary>
        /// 1ミリ秒単位でリアルタイムで更新される、ゲーム開始時間からの経過時間ミリ秒を取得します。まだ開始していない場合は0が返ります。
        /// ※フレーム計算時間など、フレーム単位で同じにしたい場合は、getGamePassedMSecByFrameを使ってください。止めたり再稼働したりしたかったら、StopWatchでもいいです。
        /// </summary>        
        public long getGamePassedMSecゲーム経過時間ミリ秒を高精度に取得＿long型は何億年間まで()
        {
            return getTime・現在時刻() - getGameBeginTimeゲーム開始時間を取得();
        }
        private long getNowTime_InUpdateFrameメインループ用の高精度なゲーム時間取得()
        {
            return CFpsManager・フレーム管理者.getNowTimeMSec・現在のゲーム時間を高精度に取得();
            // 代わりにこれにしたらフレーム開始時のRestartを忘れないで
            //return StopWatch_GetMsec前のRestartからの経過時間ミリ秒＿int型は２９日間まで();
        }
        /// <summary>
        /// 最後にロードしたゲームデータを開始した時間です。「getTime・現在時刻()」メソッドで定義された時間のlong型で格納しています。初期値はゲーム開始時間が入ります。
        /// </summary>
        private long p_gameLoadTime最後のゲームデータをロード終了した時間 = 0;
        /// <summary>
        /// ゲームをロードしてからのゲームプレイ時間です。「getTime・現在時刻()」メソッドで定義された時間のlong型で返します。まだ開始していない場合は0が返ります。
        /// </summary>
        public long getGamePassedTimeゲームプレイ時間を取得() { return (p_gameLoadTime最後のゲームデータをロード終了した時間 == 0) ? 0 : getTime・現在時刻() - p_gameLoadTime最後のゲームデータをロード終了した時間; }

        // 以下、ゲーム内で自由に使えるストップウォッチ
        public void StopWatch_Restart()
        {
            getP_fpsManager・フレーム管理者().p_stopwatch・ストップウォッチ.Reset();
            getP_fpsManager・フレーム管理者().p_stopwatch・ストップウォッチ.Start();
        }
        public void StopWatch_Pause()
        {
            getP_fpsManager・フレーム管理者().p_stopwatch・ストップウォッチ.Stop();
        }
        public void StopWatch_Continue()
        {
            getP_fpsManager・フレーム管理者().p_stopwatch・ストップウォッチ.Start();
        }
        public int StopWatch_GetMsec前のRestartからの経過時間ミリ秒＿int型は２９日間まで()
        {
            return (int)getP_fpsManager・フレーム管理者().p_stopwatch・ストップウォッチ.ElapsedMilliseconds;
        }
        public long StopWatch_GetMsec前のRestartからの経過時間ミリ秒＿long型は数億年間まで()
        {
            return getP_fpsManager・フレーム管理者().p_stopwatch・ストップウォッチ.ElapsedMilliseconds;
        }
        #endregion

        // ＝＝＝＝ ■■ゲームのメイン処理、終わり。



        // ＝＝＝＝ ■■ゲームの制作用メソッド
        // 以下■■■で囲まれているのは、ゲーム開発時によく使う、基本メソッド（●●●はカテゴリ毎にまとめきれていないメソッド
        // （様々なクラスが持つメソッドを、game.***ですぐ使えるように、ラップしてチンして調理（ラッパー化）したもの。
        // 　具体的なゲームの制作やシナリオなどを作成をする時には、なるべくここだけ触ればいいようにして、
        //   より直観的に効率よく、楽しくゲーム制作ができる環境を整えよう♪）
        //　　・・・・ただ、整理には膨大な時間がかかるし、限界がある。とりあえずでほっとかれてるメソッドも多い。
        //            詳しくはgame.***のインテリセンスをみてください。
        #region ■■■debugデバッグ系、デバッグ出力を一括管理したい時に便利
        /// <summary>
        /// （デバッグモードなら）デバッグを標準出力に出力します。デバッグモードならtrueを返します。
        /// （　if (s_isDebugMode・デバッグモード == true) の判定処理は内部でしているので、本番モードの場合はスキップされ、何もしません。）
        /// </summary>
        public bool DEBUGデバッグ一行出力(string _デバッグ文字列)
        {
            return DEBUGデバッグ一行出力(ELogType.l0_標準出力, _デバッグ文字列);
        }
        /// <summary>
        /// （デバッグモードなら）デバッグを標準出力に出力します。デバッグモードならtrueを返します。
        /// （　if (s_isDebugMode・デバッグモード == true) の判定処理は内部でしているので、本番モードの場合はスキップされ、何もしません。）
        /// </summary>
        public bool DEBUGデバッグ一行出力(ELogType _ログの種類＿デフォルトはl4_デバッグ, string _デバッグ文字列){
            bool _isDebugMode = s_isDebugMode・デバッグモード;
            if (s_isDebugMode・デバッグモード == true)
            {
                Program・実行ファイル管理者.printlnLog(_ログの種類＿デフォルトはl4_デバッグ, _デバッグ文字列);
            }
            return _isDebugMode;
        }
        // ログ表示はProgramクラスのstaticメソッドでprintLog
        #endregion

        // 1.基本処理
        #region ■■■isフラグ系（画面フォーカスが当たっているか、ＢＧＭがミュートかなど）
        /// <summary>
        /// ゲーム画面にフォーカスが当たっているか（Windowsの場合はフォーカス、それ以外はウィンドウが一番前にあって操作可能状態になっているか）を返します。
        /// </summary>
        /// <returns></returns>
        public bool isFocus・ゲーム画面にフォーカスが当たっているか()
        {
            bool _isFocus = true;
            // ※とりあえず今はWindows依存です。
            if (getP_gameWindow・ゲーム画面() != null)
            {
                if(getP_gameWindow・ゲーム画面().getisFocused・フォーカスが当たっているか() == false) _isFocus = false;
            }
            return _isFocus;
        }
        #endregion

        #region ■■■pサウンド系（pSEやpBGMなど、play関連、set音量調整なども含まれる）
        // 一時的な再生曲の管理情報

        /// <summary>
        /// 現在ＢＧＭを一時停止しているかを示します。変更する時はstopBGM()メソッドやpBGM()メソッドを使ってください。
        /// </summary>
        bool p_isPauseBGM・ＢＧＭ一時停止中 = false;
        /// <summary>
        /// 現在ＳＥを一時停止しているかを示します。変更する時はstopSE()メソッドやpSE()メソッドを使ってください。
        /// </summary>
        bool p_isPauseSE・ＳＥ一時停止中 = false;

        /// <summary>
        /// BGM再生用に使っている、BGM音楽のボリュームです。初期値は500です。変更する時はpBGM_setVolume()メソッドを使ってください。
        /// </summary>
        private int p_BGMVolume_0to1000 = 500;

        /// <summary>
        /// 現在の再生曲の前に再生されていた、前の再生曲のファイル名（フルパス）を格納してます。
        /// 　　フィールド曲→戦闘曲→同じフィールド曲など、前の再生曲をフレキシブルに変えたい時に、
        /// p_nowMusic = p_beforeMusicにして使うといいかもです。
        /// </summary>
        public string p_beforeBGM・前の再生曲フルパス = ""; // EBGM・曲.__none・無し;
        int p_beforeBGMPassedMSec・前の曲再生ミリ秒 = 0;
        /// <summary>
        /// 前の再生曲が無限ループ再生されていたかを示します。これをtrueにすると、updateSoundメソッドで自動的に無限ループします。
        /// </summary>
        bool p_beforeBGMIsLoop・前の再生曲はループ再生していたか = false;

        /// <summary>
        /// 現在の再生曲のファイル名（フルパス）を格納してます。
        /// これを変更すると、updateSoundメソッドで自動的にＢＧＭが変更されます。
        /// </summary>
        public string p_nowBGM・現在の再生曲フルパス = ""; // EBGM・曲.__none・無し;
        int p_nowBGMPassedMSec・現在の曲再生ミリ秒 = 0;
        /// <summary>
        /// 現在の再生曲を無限ループ再生するかを示します。これをtrueにすると、updateSoundメソッドで自動的に無限ループします。
        /// </summary>
        bool p_nowBGMIsLoop・現在の再生曲はループ再生中か = false;


        // 以下、効果音
        /// <summary>
        /// このゲームで一度に同時SE再生可能なチャンネル数の最大値+1です。初期値は32+1です。変更する時はプロパティの宣言部分の代入値を変更してください。
        /// </summary>
        private static int p_SEChannel_MAX = 32+1; // ●これを変更したらp_SEVolume_0to1000のプロパティの配列数も変更してね。
        /// <summary>
        /// 効果音の各チャンネル毎のボリュームです。[0]は各チャンネル共通のマスターボリュームで初期値は1000、その他の初期値は中間値500が入っています。
        /// </summary>
        private int[] p_SEVolume_0to1000
            = new int[32+1] { 1000,     500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500,     500,500 };
        /// <summary>
        /// このフレームで再生効果音を追加したか、です。
        /// これをtrueにすると、updateSoundメソッドで自動的にＳＥが再生され、次のフレームまでにすぐfalseに戻ります。
        /// </summary>
        bool[] p_nowSEAdd・このフレームで再生効果音を追加した = new bool[p_SEChannel_MAX]; // boolの初期値は全てfalse
        /// <summary>
        /// 各チャンネルの効果音を無限ループ再生するかを示します。これをtrueにすると、updateSoundメソッドで自動的に無限ループします。
        /// </summary>
        bool[] p_nowSEIsLoop・各チャンネルの効果音はループ再生中か = new bool[p_SEChannel_MAX]; // boolの初期値は全てfalse
        /// <summary>
        /// 現在再生されている効果音のファイル名（フルパス）のリストです。
        /// 　　　　p_nowSEAdd・このフレームで再生効果音を追加した、と配列を同期しています。
        /// 上記変数がtrueになったフレームで、updateSoundメソッドで自動的にtrueになった配列の効果音ファイルが格納され、再生終了後に""が入ります。
        /// </summary>
        public string[] p_nowSEs・現在の再生効果音フルパス一覧 = new string[p_SEChannel_MAX];
        /// <summary>
        /// 　現在再生されている効果音のチャンネル番号[1～p_SEChannel_MAX-1]にはtrueが入っています。
        /// [0]はチャンネル自動設定時に一瞬（１フレーム）だけtrueになり、その後すぐにfalseになります。
        /// 　　　　p_nowSEAdd・このフレームで再生効果音を追加した、と配列を同期しています。
        /// ※この値は基本的に読み取り専用です。updateSoundメソッドで自動的に値が変更されます。
        /// 上記変数がtrueになったフレームで、updateSoundメソッドで自動的に特定チャンネルがtrueになり、再生終了後にfalseになります。
        /// </summary>
        public bool[] p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧 = new bool[p_SEChannel_MAX]; // boolの初期値はfalse
        /// <summary>
        /// p_nowSEAdd・このフレームで再生効果音を追加した、の効果音再生と同時に、他の効果音を全て停止するかを指定します。
        /// </summary>
        bool p_nowSEIsStopOtherSEs・このフレームの再生効果音以外の効果音を停止するか = false;
        /// <summary>
        /// SE再生用に使っている、今のSE効果音の各チャンネル毎に設定可能なボリューム配列です。
        /// [0]はマスターボリュームで、初期値は1000です。他の初期値は全て500です。変更する時はpSE_setVolume()メソッドを使ってください。



        // 現在、サウンド再生方法は、3つの実装方法をテストしている。
        TestSoundPlayType・サウンド再生実装方法 p_TestSoundPlayType・サウンド実装方法 = TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用; // ■□ここで変更！
        enum TestSoundPlayType・サウンド再生実装方法
        {
            a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用,
            b_OS依存のサウンド再生方法＿ＭＣＩ使用,
            c_一時テスト用＿ＹａｎｅＳＤＫ使用
        }
        Yanesdk.Sound.SoundLoader p_soundTemp;         // ■□(c)一時テスト用 



        // 以下、ＢＧＭ再生関連
        /// <summary>
        /// stopBGM・ＢＧＭを一時停止で止まっていた曲を再開して、trueを返します。
        /// 一時停止していなかった場合はfalseを返します。
        /// </summary>
        public bool pBGM()
        {
            bool _is一時停止していたか = false;
            if (p_isPauseBGM・ＢＧＭ一時停止中 == true)
            {
                p_isPauseBGM・ＢＧＭ一時停止中 = false;
                _is一時停止していたか = true;
            }
            return _is一時停止していたか;
        }
        /// <summary>
        /// 曲を再生します。
        /// </summary>
        public bool pBGM(string _fileName_FullPath・ファイル名のフルパス)
        {
            return pBGM(_fileName_FullPath・ファイル名のフルパス, true);
        }
        /// <summary>
        /// 曲を再生します。
        /// </summary>
        public bool pBGM(string _fileName_FullPath・ファイル名のフルパス, bool _isRepeat・ループ再生するか)
        {
            bool _isSoundChanged = true;

            // 現在は、どの方法も何もしていない。
            //switch (p_TestSoundPlayType・サウンド実装方法)
            //{
            //    case TestSoundPlayType・サウンド再生実装方法.a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用:
            //        // ■□(a)動いて欲しいOS非依存版（名前参照に改良版）・・・でも成功したと思ったらブツブツ鳴る・・・。
            //        //生ファイルは扱っていない
            //        //_isSoundChanged = p_sound・サウンド管理者.playBGM_SDL・曲を再生(_EMusic・曲);
            //        break;
            //    case TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用:
            //        // ■□(b)Windows依存のmp3の再生方法
            //        //string _fileName_FullPath = _fileName_FullPath・ファイル名のフルパス;
            //        // (D)現在は、ファイルをオープンするスレッドとループ再生するスレッドを統一するため、
            //        //    ここでは再生予定曲を変更するだけ。実際はupdateSoundメソッドでMCIをオープン／再生するようにしている。
                    
            //        // (A)これでやってしまうと、複数のBGMを同時再生してしまう
            //        //MyTools.playSound(_fileName0_FullPath, true);
            //        // (B)mp3/wav/wma/oggでも、とりあえずＢＧＭはMCIのチャンネルで再生
            //        //_isSoundChanged = MySound_Windows.playBGM(_fileName_FullPath, _isRepeat);
            //        // (C)ファイルの拡張子によって再生方法を分ける場合
            //        //string _kakutyousi = MyTools.getFileRightOfPeriodName(_fileName_FullPath);
            //        //if (_kakutyousi == "mp3")
            //        //{
            //        //    _isSoundChanged = MySound_Windows.playBGM(_fileName_FullPath, _isRepeat);
            //        //}
            //        //else if (_kakutyousi == "wav")
            //        //{
            //        //    _isSoundChanged = MySound_Windows.SoundPlayer_playSound(_fileName_FullPath, _isRepeat);
            //        //}
            //        //else
            //        //{
            //        //    // mp3とwav以外 wma,ogg,
            //        //    _isSoundChanged = MySound_Windows.playSound(_fileName_FullPath, _isRepeat);
            //        //}
            //        break;
            //    case TestSoundPlayType・サウンド再生実装方法.c_一時テスト用＿ＹａｎｅＳＤＫ使用:
            //        // ■□(c)一時テスト用、YaneSDKの元祖（番号参照）に従う・・でもやっぱり時々オブジェクト取得失敗する・・・。
            //        //生ファイルは扱っていない
            //        //if (p_soundTemp == null)
            //        //{
            //        //    p_soundTemp = new Yanesdk.Sound.SoundLoader();
            //        //    p_soundTemp.LoadDefFile(Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス + "\\サウンドデータベース番号参照.csv");
            //        //}
            //        //p_soundTemp.PlayBGM((int)_EMusic・曲);
            //        break;
            //    default:
            //        break;
            //}
            pBGM_addBGM・ＢＧＭの変更＿内部専用メソッド(_fileName_FullPath・ファイル名のフルパス, _isRepeat・ループ再生するか);
            return _isSoundChanged;
        }
        private void pBGM_addBGM・ＢＧＭの変更＿内部専用メソッド(string _fileName_FullPath・ファイル名のフルパス, bool _isRepeat・ループ再生するか)
        {
            // ●テスト（ほんとはメインスレッドでやるつもりだけど、なぜかＢＧＭだけメインスレッドでならないから、ここで鳴らしてみる）
            // ＢＧＭ再生
            MySound_Windows.playBGM(_fileName_FullPath・ファイル名のフルパス, _isRepeat・ループ再生するか);
            //MySound_Windows.playBGM(p_nowBGM・現在の再生曲フルパス, p_nowBGMIsLoop・現在の再生曲はループ再生中か);

            p_nowBGM・現在の再生曲フルパス = _fileName_FullPath・ファイル名のフルパス;
            p_nowBGMIsLoop・現在の再生曲はループ再生中か = _isRepeat・ループ再生するか;
            // 前の再生曲と経過時間を保存
            p_beforeBGM・前の再生曲フルパス = p_nowBGM・現在の再生曲フルパス;
            p_beforeBGMPassedMSec・前の曲再生ミリ秒 = p_nowBGMPassedMSec・現在の曲再生ミリ秒;
            p_beforeBGMIsLoop・前の再生曲はループ再生していたか = p_nowBGMIsLoop・現在の再生曲はループ再生中か;
            // 現在の再生曲を保存
            p_nowBGM・現在の再生曲フルパス = _fileName_FullPath・ファイル名のフルパス; //EBGM・曲._unknown・未定義曲;
            p_nowBGMPassedMSec・現在の曲再生ミリ秒 = 0;
        }
        /// <summary>
        /// 曲を再生します。
        /// </summary>
        /// <param name="_EMusic・曲"></param>
        /// <returns></returns>
        public bool pBGM(EBGM・曲 _EMusic・曲)
        {
            bool _isSoundChanged = true;
            
            // サウンドＯＦＦだとfalseで、再生しない。
            if (s_optionBGM_ON・ＢＧＭを再生する状態か == false)
            {
                _isSoundChanged = false;
                return _isSoundChanged;
            }
            
            if (_EMusic・曲 == EBGM・曲.__none・無し)
            {
                // 無音
                pBGM_addBGM・ＢＧＭの変更＿内部専用メソッド("", false);
            }
            else
            {
                string _fileName・フルパス = getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_EMusic・曲);
                bool _isRepeat = true; // デフォルトはtrue

                // 今は必ず再生
                if(false)
                // 指定秒以内だったら曲を変更しない（無音・勝利・敗北の曲は例外）
                // ※でも、今は必ず勝利・敗北が入るので、そんなことしても無駄。
                //if (p_nowBGM・現在の再生曲フルパス != EBGM・曲.none・無し＿OFF &&
                //    p_nowBGM・現在の再生曲フルパス != EBGM・曲.win01・勝利ファンファーレ＿タラッラララッター &&
                //    p_nowBGM・現在の再生曲フルパス != EBGM・曲.lose01・全滅＿がーん_もう終わりかよ && 
                //    p_nowBGMPassedMSec・現在の曲再生ミリ秒 < s_MusicMinTimePlayingMSec・曲最低持続秒)
                {
                    _isSoundChanged = false;
                    DEBUGデバッグ一行出力(ELogType.l4_重要なデバッグ, "♪曲継続：　現在の再生曲（" +
                        MyTools.getFileName_NotFullPath_LastFileOrDirectory(p_nowBGM・現在の再生曲フルパス) + "）の再生秒が短すぎる（" +
                        p_nowBGMPassedMSec・現在の曲再生ミリ秒 + "秒）なので、" +
                    "新しい曲（" + getBGMLabel・曲名を取得(_EMusic・曲) + "）は再生させませんでした。");
                }
                else
                {
                    // 勝利・敗北の曲は無条件で流す（連勝・連敗だと、必ず前の曲とおんなじになるため）
                    if (_EMusic・曲 == EBGM・曲.win01・勝利ファンファーレ＿タラッラララッター ||
                        _EMusic・曲 == EBGM・曲.lose01・全滅＿がーん_もう終わりかよ)
                    {
                        // 鳴らす
                    }
                    else
                    {
                        // ＜テスト＞戦闘の曲が、現在の曲や、前の曲（勝利・敗北前の曲以外）と一緒だったら、
                        // ランダムで違う曲を鳴らす
                        while (_fileName・フルパス == p_nowBGM・現在の再生曲フルパス
                            || _fileName・フルパス == p_beforeBGM・前の再生曲フルパス)
                        {
                            _EMusic・曲 = getRnadomMusic・ランダムに曲を選曲();
                            _fileName・フルパス = getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_EMusic・曲);
                            DEBUGデバッグ一行出力(ELogType.l4_重要なデバッグ, "♪曲をランダム選曲：　" +
                                getBGMLabel・曲名を取得(_EMusic・曲) +
                                "　＜理由＞現在の再生曲（" + MyTools.getFileName_NotFullPath_LastFileOrDirectory(p_nowBGM・現在の再生曲フルパス) +
                                "か前の再生曲（" + MyTools.getFileName_NotFullPath_LastFileOrDirectory(p_beforeBGM・前の再生曲フルパス) +
                                "）と同じだったため。");
                        }
                        // サウンドファイルが存在しなかったら、代わりに鳴らす音に変更
                        if (MyTools.isExist(p_sound・サウンド管理者.getFileName_FullPath・ファイル名を取得(_EMusic・曲)) == false)
                        {
                            _EMusic・曲 = EBGM・曲._fileNotFound・ファイル読み込み失敗した時に代わりに鳴らす音;                            
                            // ランダム再生
                            //pBGMRandom_FromDirectory();
                            //return true;
                        }
                    }


                    switch (p_TestSoundPlayType・サウンド実装方法)
                    {
                        case TestSoundPlayType・サウンド再生実装方法.a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用:
                            // ■□(a)動いて欲しいOS非依存版（名前参照に改良版）・・・でも成功したと思ったらブツブツ鳴る・・・。
                            _isSoundChanged = p_sound・サウンド管理者.playBGM_SDL・曲を再生(_EMusic・曲);
                            break;
                        case TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用:
                            // ■□(b)Windows依存のmp3の再生方法
                            //string _kakutyousi = MyTools.getFileRightOfPeriodName(_fileName・フルパス);
                            _isRepeat = p_sound・サウンド管理者.getIsRepeat・繰り返し情報を取得(_EMusic・曲);
                            // ※直接以下を呼び出すと、ループ再生がスレッドで管理できなくなる
                            //_isSoundChanged = MySound_Windows.MCI_playBGM(_fileName・フルパス, _isRepeat);
                            break;
                        case TestSoundPlayType・サウンド再生実装方法.c_一時テスト用＿ＹａｎｅＳＤＫ使用:
                            // ■□(c)一時テスト用、YaneSDKの元祖（番号参照）に従う・・でもやっぱり時々オブジェクト取得失敗する・・・。
                            if (p_soundTemp == null)
                            {
                                p_soundTemp = new Yanesdk.Sound.SoundLoader();
                                p_soundTemp.LoadDefFile(Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス + "\\サウンドデータベース番号参照.csv");
                            }
                            p_soundTemp.PlayBGM((int)_EMusic・曲);
                            break;
                        default:
                            break;
                    }
                    // 生ファイル版のメソッドを呼び出す
                    pBGM(_fileName・フルパス, _isRepeat);
                }
            }
            return _isSoundChanged;
        }
        /// <summary>
        /// 基本的に、updateFrameメソッドが呼び出します。
        /// ＢＧＭのループ再生確認や、効果音連続再生の負荷を軽減したりする処理です。
        /// </summary>
        public void updateSound・フレーム毎のＢＧＭやＳＥのループ再生やサウンド再生時間更新処理()
        {
            // サウンド管理者のUpdateを呼び出す。
            getP_Sound・サウンド管理者().Update・サウンド再生時間を更新();
            // サウンドの継続秒をプラス
            p_nowBGMPassedMSec・現在の曲再生ミリ秒 += s_FRAME1_MSEC・1フレームミリ秒;

            bool _isTestBGMやSE再生にメインスレッドを使うかtrue＿適時イベントで取得するかfalse = false;
            if (_isTestBGMやSE再生にメインスレッドを使うかtrue＿適時イベントで取得するかfalse == false)
            {
                return;
            }
            //  _isTestBGMやSE再生にメインスレッドを使うかがtrueのとき、ＢＧＭループ確認（MCIを使った方法だけ必要）
            switch (p_TestSoundPlayType・サウンド実装方法)
            {
                case TestSoundPlayType・サウンド再生実装方法.a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用:
                    break;
                case TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用:
                    // 1.0.BGM音量に変更があったら表示
                    int _volume = MySound_Windows.MCI_getVolume_BGM();
                    if (p_BGMVolume_0to1000 != _volume)
                    {
                        MySound_Windows.MCI_setVolume_BGM(p_BGMVolume_0to1000);
                        DEBUGデバッグ一行出力("BGM音量変更：" + MySound_Windows.MCI_getVolume_BGM());
                    }
                    // 1.1.ＢＧＭ再生
                    // ＢＧＭに変更があったか
                    string _nowPlayingBGM = MySound_Windows.MCI_getPlayingBGMName_FullPath();
                    if (p_nowBGM・現在の再生曲フルパス != _nowPlayingBGM)
                    {
                        // ＢＧＭ再生
                        MySound_Windows.playBGM(p_nowBGM・現在の再生曲フルパス, p_nowBGMIsLoop・現在の再生曲はループ再生中か);
                    }
                    // 1.2.ＢＧＭ再生終了確認
                    // ループＢＧＭか
                    if (//getP_Sound・サウンド管理者().getIsRepeat・繰り返し情報を取得(p_nowBGM・現在の再生曲フルパス) == true &&
                        MySound_Windows.MCI_isBGMLoop() == true)
                    {
                        // 再生が終了（もしくはサウンドデータベースに記述された指定時間を超えたら）、もう一度再生
                        if (MySound_Windows.MCI_isEndBGM() == true)
                        //if(p_nowBGMPassedMSec・現在の曲再生ミリ秒 >= getP_Sound・サウンド管理者().getLoopEndMSec・ループ折り返し時間(p_nowBGM・現在の再生曲フルパス))
                        {
                            int _loopStartPassedMSec = 0; // とりあえず最初から再生
                            MySound_Windows.MCI_seekBGM(_loopStartPassedMSec);
                            //string _FullPath = getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(p_nowBGM・現在の再生曲フルパス);
                            // MySound_Windows.MCI_playBGM(_FullPath, true, p_LoopStartMSec・ループ再生開始時間);
                        }
                    }
                    else
                    {
                        // これをしたら最後に再生したＢＧＭファイル名は""になって取れなくなるがいいのか？
                        // ループ無しのＢＧＭは、再生が終了したらクローズしてチャンネルを空ける
                        //if (MySound_Windows.MCI_isEndBGM() == true)
                        //{
                        //    MySound_Windows.MCI_stopBGM();
                        //}
                    }


                    for (int _ChannelNo = 0; _ChannelNo <= Math.Min(p_SEChannel_MAX-1, MySound_Windows.p_MCI_SEChannel_MAXplus1-1); _ChannelNo++)
                    {
                        // 2.0.SE音量に変更があったら表示
                        _volume = MySound_Windows.MCI_getVolume_SE(_ChannelNo);
                        if (p_SEVolume_0to1000[_ChannelNo] != _volume)
                        {
                            MySound_Windows.MCI_setVolume_SE(p_SEVolume_0to1000[_ChannelNo], _ChannelNo);
                            DEBUGデバッグ一行出力("BGM音量変更：" + MySound_Windows.MCI_getVolume_SE(_ChannelNo));
                        }
                        // 1.1ＳＥ再生
                        if (_ChannelNo == 0) // 0はチャンネル自動設定。チャンネル0は再生用には割り当てていない
                        {
                            if (p_nowSEAdd・このフレームで再生効果音を追加した[0] == true)
                            {
                                // チャンネルを自動設定して、最後に追加されたＳＥファイルを再生
                                int _autoChannelNo・自動取得チャンネル番号 = 
                                    MySound_Windows.MCI_playSE(p_nowSEs・現在の再生効果音フルパス一覧[0],
                                    p_nowSEIsLoop・各チャンネルの効果音はループ再生中か[0]);
                                if (_autoChannelNo・自動取得チャンネル番号 != -1) // 理論上再生不可なら再生しない
                                {
                                    // 自動設定したチャンネル番号を使用中にし、再生中のＳＥファイルフルパスを格納
                                    p_nowSEs・現在の再生効果音フルパス一覧[_autoChannelNo・自動取得チャンネル番号] = p_nowSEs・現在の再生効果音フルパス一覧[0];
                                    p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[_autoChannelNo・自動取得チャンネル番号] = true;
                                }
                                // チャンネル0を初期化
                                p_nowSEs・現在の再生効果音フルパス一覧[0] = "";
                                p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[0] = false;
                                // 追加したフラグを初期化
                                p_nowSEAdd・このフレームで再生効果音を追加した[0] = false;
                                p_nowSEIsStopOtherSEs・このフレームの再生効果音以外の効果音を停止するか = false;
                            }
                        }else{
                            // 指定チャンネルのＳＥに変更があったか
                            if (p_nowSEAdd・このフレームで再生効果音を追加した[_ChannelNo] == true)
                            {
                                // 最後に追加されたＳＥファイルを再生
                                MySound_Windows.playSE(p_nowSEs・現在の再生効果音フルパス一覧[_ChannelNo],
                                    p_nowSEIsLoop・各チャンネルの効果音はループ再生中か[_ChannelNo], 
                                    _ChannelNo, 
                                    p_nowSEIsStopOtherSEs・このフレームの再生効果音以外の効果音を停止するか);
                                // チャンネル番号を使用中にする
                                p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[_ChannelNo] = true;
                                // 追加したフラグを初期化
                                p_nowSEAdd・このフレームで再生効果音を追加した[_ChannelNo] = false;
                                p_nowSEIsStopOtherSEs・このフレームの再生効果音以外の効果音を停止するか = false;
                            }
                            // そのチャンネルが使用されていれば、チェック
                            if (p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[_ChannelNo] == true)
                            {
                                // 2.1.ＳＥループ確認
                                if (MySound_Windows.MCI_isSELoop(_ChannelNo) == true)
                                {
                                    // 再生中か
                                    if (MySound_Windows.MCI_isEndSE(_ChannelNo) == true)
                                    {
                                        // ループ有りのＳＥは、最初から再生
                                        MySound_Windows.MCI_seekSE(_ChannelNo, 0);
                                    }
                                }
                                else
                                {
                                    // 再生中か
                                    if (MySound_Windows.MCI_isEndSE(_ChannelNo) == true)
                                    {
                                        // 1.3.ＳＥの削除
                                        // ループ無しのＳＥは、停止してチャンネルを空ける
                                        MySound_Windows.MCI_stopSE(_ChannelNo);
                                        p_nowSEs・現在の再生効果音フルパス一覧[_ChannelNo] = "";
                                        p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[_ChannelNo] = false;
                                        p_nowSEIsLoop・各チャンネルの効果音はループ再生中か[_ChannelNo] = false;

                                    }

                                }
                            }
                        }
                    }
                        break;
                default:
                    break;
            }
        }
        /// <summary>
        /// データベースのフォルダ内（ディレクトリー内）から、ランダムで音楽を再生します。再生したサウンドファイルのフルパスを返します。
        /// ミュート中の場合は、"ＢＧＭミュート中です"と返します。
        /// </summary>
        public string pBGMRandom_FromDirectory()
        {
            if (s_optionBGM_ON・ＢＧＭを再生する状態か == false) return "ＢＧＭミュート中です";
            return MyTools.playSound_MP3s_FromDirectory(Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス);
        }
        /// <summary>
        /// このゲーム内で再生するWAVE・mp3・wma全部のマスタ音量を変更します。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        public void setVolume_master(int _volume_0to1000)
        {
            // これだとWAVE・mp3・wma全部の音量を変えるようになってしまう。
            MyTools.setVolume_Master(_volume_0to1000);
        }
        /// <summary>
        /// 曲（BGM）のボリュームを調整します。効果音のボリュームは変更されません。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        /// <returns></returns>
        public void pBGM_setVolume(int _volume_0to1000)
        {
            switch(p_TestSoundPlayType・サウンド実装方法)
            {
                case TestSoundPlayType・サウンド再生実装方法.a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用:
                    // ■□(a)動いて欲しいOS非依存版（名前参照に改良版）・・・でも成功したと思ったらブツブツ鳴る・・・。
                    getP_Sound・サウンド管理者().setBGMVolume_SDL・ＢＧＭ音量変更(_volume_0to1000);
                    break;
                case TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用:
                    // ■□(b)Windows依存のmp3の再生方法・・・時々オブジェクト取得失敗して鳴らなくなる・・・。
                    // 今まで一度もBGMを再生していないか
                    if (p_beforeBGM・前の再生曲フルパス == "" && p_nowBGM・現在の再生曲フルパス == "")
                    {
                        // ※注意：少なくともpBGM()でBGMを１曲以上再生してから、このメソッドを呼び出さないと、効果がない
                        // my_soundエイリアスが生成されないと、MCIのボリューム設定は効かない
                        game.pBGM(EBGM・曲._fileNotFound・ファイル読み込み失敗した時に代わりに鳴らす音);
                        // 音量設定
                        p_BGMVolume_0to1000 = _volume_0to1000; //これやると、一度もロードしてない場合0が返る MySound_Windows.MCI_getVolume_BGM();
                        // 今は、ループ再生を管理するupdateSoundメソッドが呼び出しているから、ここではp_BGMVolume_0to1000を変更するだけでいい。
                        // ※以下を直接やっても、MCI_playBGMメソッドを呼び出すスレッドと異なるとエラーになる。
                        //MySound_Windows.MCI_setVolume_BGM(_volume_0to1000);
                        // すぐ止める
                        game.stopBGM・ＢＧＭを一時停止();
                    }
                    else
                    {
                        // 音量設定
                        p_BGMVolume_0to1000 = _volume_0to1000;
                    }
                    break;
                case TestSoundPlayType・サウンド再生実装方法.c_一時テスト用＿ＹａｎｅＳＤＫ使用:
                    // ■□(c)一時テスト用、YaneSDKの元祖（番号参照）に従う・・でもやっぱり時々オブジェクト取得失敗する・・・。
                    CSoundPlayData・オーディオ再生用クラス.ChunkManager.SetVolume(0, _volume_0to1000);
                    break;
                default:
                    break;
            }
        }
        public int pBGM_getVolume() { return p_BGMVolume_0to1000; }
        /// <summary>
        /// BGMを一時停止します。pBGM()で再開できます。
        /// </summary>
        /// <returns></returns>
        public void stopBGM・ＢＧＭを一時停止()
        {
            p_isPauseBGM・ＢＧＭ一時停止中 = true;
                    
            switch (p_TestSoundPlayType・サウンド実装方法)
            {
                case TestSoundPlayType・サウンド再生実装方法.a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用:
                    // ■□(a)動いて欲しいOS非依存版（名前参照に改良版）・・・でも成功したと思ったらブツブツ鳴る・・・。
                    p_sound・サウンド管理者.stopBGM_SDL・曲を停止();
                    break;
                case TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用:
                    // ■□(b)Windows依存のmp3の再生方法
                    // なにもしない
                    // MCIは、ループ再生を管理するupdateSoundメソッドを呼び出すメインスレッドが呼び出しているから、ここではプロパティを変更するだけ。
                    // MySound_Windows.stopBGM();
                    break;
                case TestSoundPlayType・サウンド再生実装方法.c_一時テスト用＿ＹａｎｅＳＤＫ使用:
                    // ■□(c)一時テスト用、YaneSDKの元祖（番号参照）に従う・・でもやっぱり時々オブジェクト取得失敗する・・・。
                    if (p_soundTemp != null)
                    {
                        p_soundTemp.StopBGM();
                    }
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// BGMがミュート中かを設定します。現実装では、これがfalseにすると、ボリューム0にするのではなく、BGMを再生するメソッドすら呼び出しませんので注意してください。
        /// </summary>
        /// <param name="_isMute"></param>
        public void setBGM_Mute・ＢＧＭをミュート状態にする(bool _isMute)
        {
            if (_isMute == true){
                s_optionBGM_ON・ＢＧＭを再生する状態か = false;
                // デバッグモードの時は確認
                //if (s_isDebugMode・デバッグモード == true)
                //{
                //    //これをやると画面が固まる。Application.DoEventsとMessageBoxは相性が悪いのかもしれないMyTools.MessageBoxShow("ＢＧＭがミュート状態になっています。\n（s_optionBGM_ON・ＢＧＭを再生する状態か == false）");
                //}
            }
            else
            {
                s_optionBGM_ON・ＢＧＭを再生する状態か = true;
            }
        }
        /// <summary>
        /// BGMがミュート中かを返します。現実装では、これをfalseにすると、ボリューム0にするのではなく、BGMを再生するメソッドすら呼び出しませんので注意してください。
        /// </summary>
        /// <returns></returns>
        public bool isBGM_Mute・ＢＧＭがミュート状態か() { return s_optionBGM_ON・ＢＧＭを再生する状態か; }

        // 以下、効果音
        /// <summary>
        /// 一時停止してした効果音を再開してtrueを返します。一時停止していない場合は、falseを返します。
        /// </summary>
        public bool pSE()
        {
            if (p_isPauseSE・ＳＥ一時停止中 == true)
            {
                p_isPauseSE・ＳＥ一時停止中 = false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 引数のサウンドファイルを効果音として再生します。再生に成功したかを返します。
        /// </summary>
        public bool pSE(string _fileName_FullPath)
        {
            return pSE(_fileName_FullPath, false);
        }
        /// <summary>
        /// 引数のサウンドファイルを効果音として再生します。引数にループ再生するかどうかを示します。再生に成功したかを返します。
        /// </summary>
        public bool pSE(string _fileName_FullPath, bool _isRepeat)
        {

            bool _isSEChanged = true;
            //string _kakutyousi = MyTools.getFileRightOfPeriodName(_fileName_FullPath);
            // MCIは、ループ再生を管理するupdateSoundメソッドを呼び出すメインスレッドが呼び出しているから、ここではプロパティを変更するだけ。
            pSE_AddSE・再生効果音の追加＿内部専用メソッド(_fileName_FullPath, _isRepeat, 0, false); // チャンネル0はチャンネル自動取得
            //_isSEChanged = MySound_Windows.playSE(_fileName_FullPath, _isRepeat);
            return _isSEChanged;
        }
        /// <summary>
        /// 引数のサウンドファイルを効果音として再生します。引数にループ再生するかどうかを示します。
        /// 引数３にチャンネル番号を指定します。引数４にこの効果音再生時に他の全ての効果音を停止するかを示します。
        /// 再生に成功したかを返します。
        /// </summary>
        public bool pSE(string _fileName_FullPath, bool _isRepeat, int _ChannelNo, bool _isStopOtherSEs)
        {

            bool _isSEChanged = true;
            //string _kakutyousi = MyTools.getFileRightOfPeriodName(_fileName_FullPath);
            // MCIは、ループ再生を管理するupdateSoundメソッドを呼び出すメインスレッドが呼び出しているから、ここではプロパティを変更するだけ。
            pSE_AddSE・再生効果音の追加＿内部専用メソッド(_fileName_FullPath, _isRepeat, _ChannelNo, _isStopOtherSEs); // チャンネル0はチャンネル自動取得
            //_isSEChanged = MySound_Windows.playSE(_fileName_FullPath, _isRepeat);
            return _isSEChanged;
        }

        private void pSE_AddSE・再生効果音の追加＿内部専用メソッド(string _fileName_FullPath, bool _isRepeat, int _ChannelNo, bool _isStopOtherSEs)
        {
            p_nowSEs・現在の再生効果音フルパス一覧[_ChannelNo] = _fileName_FullPath;
            p_nowSEAdd・このフレームで再生効果音を追加した[_ChannelNo] = true;
            p_nowSEIsLoop・各チャンネルの効果音はループ再生中か[_ChannelNo] = _isRepeat;
            p_nowSEIsStopOtherSEs・このフレームの再生効果音以外の効果音を停止するか = _isStopOtherSEs;

            // ●テスト（ほんとはメインスレッドでやりたいが、どうしてもできないのでここでやる）
            // 1.1.ＳＥ再生
            if (_ChannelNo == 0) // 0はチャンネル自動設定。チャンネル0は再生用には割り当てていない
            {
                if (p_nowSEAdd・このフレームで再生効果音を追加した[0] == true)
                {
                    // チャンネルを自動設定して、最後に追加されたＳＥファイルを再生
                    int _autoChannelNo・自動取得チャンネル番号 =
                        MySound_Windows.MCI_playSE(p_nowSEs・現在の再生効果音フルパス一覧[0],
                        p_nowSEIsLoop・各チャンネルの効果音はループ再生中か[0]);
                    if (_autoChannelNo・自動取得チャンネル番号 != -1) // 理論上再生不可なら再生しない
                    {
                        // 自動設定したチャンネル番号を使用中にし、再生中のＳＥファイルフルパスを格納
                        p_nowSEs・現在の再生効果音フルパス一覧[_autoChannelNo・自動取得チャンネル番号] = p_nowSEs・現在の再生効果音フルパス一覧[0];
                        p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[_autoChannelNo・自動取得チャンネル番号] = true;
                    }
                    // チャンネル0を初期化
                    p_nowSEs・現在の再生効果音フルパス一覧[0] = "";
                    p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[0] = false;
                    // 追加したフラグを初期化
                    p_nowSEAdd・このフレームで再生効果音を追加した[0] = false;
                    p_nowSEIsStopOtherSEs・このフレームの再生効果音以外の効果音を停止するか = false;
                }
            }
            else
            {
                // 指定チャンネルのＳＥに変更があったか
                if (p_nowSEAdd・このフレームで再生効果音を追加した[_ChannelNo] == true)
                {
                    // 最後に追加されたＳＥファイルを再生
                    MySound_Windows.playSE(p_nowSEs・現在の再生効果音フルパス一覧[_ChannelNo],
                        p_nowSEIsLoop・各チャンネルの効果音はループ再生中か[_ChannelNo],
                        _ChannelNo,
                        p_nowSEIsStopOtherSEs・このフレームの再生効果音以外の効果音を停止するか);
                    // チャンネル番号を使用中にする
                    p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[_ChannelNo] = true;
                    // 追加したフラグを初期化
                    p_nowSEAdd・このフレームで再生効果音を追加した[_ChannelNo] = false;
                    p_nowSEIsStopOtherSEs・このフレームの再生効果音以外の効果音を停止するか = false;
                }
            }
        }
        private void pSE_removeSE・再生効果音の削除＿内部専用メソッド(int _ChannelNo)
        {
            // 1.3.ＳＥの削除
            // ループ無しのＳＥは、停止してチャンネルを空ける
            MySound_Windows.MCI_stopSE(_ChannelNo);
            p_nowSEs・現在の再生効果音フルパス一覧[_ChannelNo] = "";
            p_nowSEChannelsUsing・現在の効果音チャンネル使用状況一覧[_ChannelNo] = false;
            p_nowSEIsLoop・各チャンネルの効果音はループ再生中か[_ChannelNo] = false;
        }

        /// <summary>
        /// 効果音を再生します。
        /// </summary>
        /// <param name="_ESE・効果音"></param>
        /// <returns></returns>
        public bool pSE(ESE・効果音 _ESE・効果音)
        {
            return pSE(_ESE・効果音, 0, false);
        }
        /// <summary>
        /// 効果音を再生します。
        /// </summary>
        /// <param name="_ESE・効果音"></param>
        /// <returns></returns>
        public bool pSE(ESE・効果音 _ESE・効果音, int _ChannelNo, bool _isStopOtherSEs)
        {
            bool _isSEChanged = false;

            string _fileName_FullPath = p_sound・サウンド管理者.getFileName_FullPath・ファイル名を取得(_ESE・効果音);
            //string _kakutyousi = MyTools.getFileRightOfPeriodName(_fileName_FullPath);
            bool _isRepeat = p_sound・サウンド管理者.getIsRepeat・繰り返し情報を取得(_ESE・効果音);

            switch (p_TestSoundPlayType・サウンド実装方法)
            {
                case TestSoundPlayType・サウンド再生実装方法.a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用:
                    _isSEChanged = p_sound・サウンド管理者.playSE_SDL・効果音を再生(_ESE・効果音);
                    break;
                case TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用:
                    break;
                default:
                    break;
            }
            // 生ファイル版のメソッドを呼び出す
            _isSEChanged = pSE(_fileName_FullPath, _isRepeat, _ChannelNo, _isStopOtherSEs);
            return _isSEChanged;
        }
        /// <summary>
        /// 曲を効果音として再生します。　(a)動いて欲しいOS非依存版（ＳＤＬ）を使っているので、現在は正常動作しない場合が多いです。
        /// </summary>
        /// <param name="_EMusic・曲"></param>
        /// <param name="_フェードイン秒"></param>
        /// <param name="_通常音量再生秒"></param>
        /// <param name="_フェードアウト秒"></param>
        /// <returns></returns>
        public bool pSE(EBGM・曲 _EMusic・曲, double _フェードイン秒, double _通常音量再生秒, double _フェードアウト秒)
        {
            // ■□(a)動いて欲しいOS非依存版
            return p_sound・サウンド管理者.playSE_SDL・効果音を再生(_EMusic・曲, _フェードイン秒, _通常音量再生秒, _フェードアウト秒);
        }
        #region サウンド・効果音系の別の記述方法
        public void se効果音(ESE・効果音 _ESE・効果音)
        {
            pSE(_ESE・効果音);
        }
        public void se効果音(EBGM・曲 _EMusic・曲, double _フェードイン秒, double _通常音量再生秒, double _フェードアウト秒)
        {
            pSE(_EMusic・曲, _フェードイン秒, _通常音量再生秒, _フェードアウト秒);
        }
        #endregion
        /// <summary>
        /// SE全てを一時停止（ポーズ）します。
        /// </summary>
        /// <returns></returns>
        public void stopSE・効果音を一時停止()
        {
            switch (p_TestSoundPlayType・サウンド実装方法)
            {
                case TestSoundPlayType・サウンド再生実装方法.a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用:
                    // ■□(a)動いて欲しいOS非依存版（名前参照に改良版）・・・でも成功したと思ったらブツブツ鳴る・・・。
                    p_sound・サウンド管理者.stopBGM_SDL・曲を停止();
                    break;
                case TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用:
                    // ■□(b)Windows依存のmp3の再生方法
                    p_isPauseSE・ＳＥ一時停止中 = true;
                    // MCIは、ループ再生を管理するupdateSoundメソッドを呼び出すメインスレッドが呼び出しているから、ここではプロパティを変更するだけ。
                    // MySound_Windows.stopBGM();
                    break;
                case TestSoundPlayType・サウンド再生実装方法.c_一時テスト用＿ＹａｎｅＳＤＫ使用:
                    // ■□(c)一時テスト用、YaneSDKの元祖（番号参照）に従う・・でもやっぱり時々オブジェクト取得失敗する・・・。
                    if (p_soundTemp != null)
                    {
                        // Stopの引数がよくわからない…チャンネル？0-7？
                        p_soundTemp.Stop(0);
                    }
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 効果音（SE）のチャンネル共通のマスターボリュームを調整します。ＢＧＭのボリュームは変更されません。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        /// <returns></returns>
        public void pSE_setVolume(int _volume_0to1000)
        {
            pSE_setVolume(_volume_0to1000, 0);
        }
        /// <summary>
        /// 効果音（SE）のボリュームを調整します。ＢＧＭのボリュームは変更されません。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        /// <returns></returns>
        public void pSE_setVolume(int _volume_0to1000, int _SEChannelNo)
        {
            switch (p_TestSoundPlayType・サウンド実装方法)
            {
                case TestSoundPlayType・サウンド再生実装方法.a_動いて欲しいがなぜか動かないOS非依存改良版＿ＳＤＬ使用:
                    // ■□(a)動いて欲しいOS非依存版（名前参照に改良版）・・・でも成功したと思ったらブツブツ鳴る・・・。
                    getP_Sound・サウンド管理者().setSEVolume_SDL・ＳＥ音量変更(_volume_0to1000);
                    break;
                case TestSoundPlayType・サウンド再生実装方法.b_OS依存のサウンド再生方法＿ＭＣＩ使用:
                    // ■□(b)Windows依存のmp3の再生方法
                    // MCIは、ループ再生を管理するupdateSoundメソッドを呼び出すメインスレッドが呼び出しているから、ここではプロパティを変更するだけ。
                    //MySound_Windows.setVolumeSE(_volume_0to1000);
                    if (_SEChannelNo >= 0 && _SEChannelNo <= p_SEVolume_0to1000.Length - 1)
                    {
                        p_SEVolume_0to1000[_SEChannelNo] = _volume_0to1000; //これやると、一度もロードしてない場合0が返る MySound_Windows.getVolumeSE();
                    }
                    break;
                case TestSoundPlayType・サウンド再生実装方法.c_一時テスト用＿ＹａｎｅＳＤＫ使用:
                    // ■□(c)一時テスト用、YaneSDKの元祖（番号参照）に従う・・でもやっぱり時々オブジェクト取得失敗する・・・。
                    for (int _chunkNo_SE = 1; _chunkNo_SE <= 5; _chunkNo_SE++)
                    {
                        CSoundPlayData・オーディオ再生用クラス.ChunkManager.SetVolume(_chunkNo_SE, _volume_0to1000);
                    }
                    break;
                default:
                    break;
            }
        }
        public int pSE_getVolume() { return MyTools.getArrayValue<int>(p_SEVolume_0to1000, 0); }
        public int pSE_getVolume(int _SEChannelNo) { return MyTools.getArrayValue<int>(p_SEVolume_0to1000, _SEChannelNo); }
        /// <summary>
        /// 効果音がミュート中かを設定します。現実装では、これがfalseにすると、ボリューム0にするのではなく、効果音を再生するメソッドすら呼び出しませんので注意してください。
        /// </summary>
        /// <param name="_isMute"></param>
        public void setSE_Mute・効果音をミュート状態にする(bool _isMute)
        {
            if (_isMute == true)
            {
                s_optionSE_ON・効果音を再生する状態か = false;
            }
            else
            {
                s_optionSE_ON・効果音を再生する状態か = true;
            }
        }
        /// <summary>
        /// BGMがミュート中かを返します。現実装では、これがfalseにすると、ボリューム0にするのではなく、効果音を再生するメソッドすら呼び出しませんので注意してください。
        /// </summary>
        /// <returns></returns>
        public bool isSE_Mute・効果音がミュート状態か() { return !s_optionSE_ON・効果音を再生する状態か; }


        // 以下、ＢＧＭやＳＥのファイル名や表示名の取得関連
        // (a)列挙体で記述された場合
        /// <summary>
        /// 曲に割り当てられたファイル名をフルパスで取得します。
        /// 　　　一つじゃない場合は、代表的なファイル名１つのみを取得します。複数ファイルを取得したい場合は、p_sound・サウンド管理者の別のメソッドを使ってください。
        /// </summary>
        public string getBGMFileName・曲のフルパスを取得(EBGM・曲 _EBGM・曲)
        {
            return p_sound・サウンド管理者.getFileName_FullPath・ファイル名を取得(_EBGM・曲);
        }
        /// <summary>
        /// 曲に割り当てられたファイル名をフルパスで取得します。
        /// 　　　一つじゃない場合は、代表的なファイル名１つのみを取得します。複数ファイルを取得したい場合は、p_sound・サウンド管理者の別のメソッドを使ってください。
        /// </summary>
        public string getSEFileName・曲のフルパスを取得(ESE・効果音 _ESE・効果音)
        {
            return p_sound・サウンド管理者.getFileName_FullPath・ファイル名を取得(_ESE・効果音);
        }
        /// <summary>
        /// 曲の表示名（例：○○○.mp3の"○○○"など）を取得します。
        ///         /// 曲の情報を視覚的に認識可能な、ラベルを取得します。
        /// 基本的には曲ファイル名の拡張子を抜いた文字列「曲名」や、効果音ファイル名の"_"や"＿"や"・"で区切られた最後「ピコーン」などの擬音語が格納されるようになっています。
        /// </summary>
        public string getBGMLabel・曲名を取得(EBGM・曲 _EMusic・曲)
        {
            return p_sound・サウンド管理者.getLabel・ラベルを取得(_EMusic・曲);
        }
        /// <summary>
        /// 効果音の表示名（例："ピコーン"など）を取得します。
        ///         /// 効果音の情報を視覚的に認識可能な、ラベルを取得します。
        /// 基本的には曲ファイル名の拡張子を抜いた文字列「曲名」や、効果音ファイル名の"_"や"＿"や"・"で区切られた最後「ピコーン」などの擬音語が格納されるようになっています。
        /// </summary>
        public string getSELabel・ＳＥ擬音語を取得(ESE・効果音 _ESE・効果音)
        {
            return p_sound・サウンド管理者.getLabel・ラベルを取得(_ESE・効果音);
        }
        // (b)フルパスで記述された場合
        /// <summary>
        /// 引数を曲とした場合の表示名（例：○○○.mp3の"○○○"など）を取得します。
        ///         /// 曲の情報を視覚的に認識可能な、ラベルを取得します。
        /// 基本的には曲ファイル名の拡張子を抜いた文字列「曲名」や、効果音ファイル名の"_"や"＿"や"・"で区切られた最後「ピコーン」などの擬音語が格納されるようになっています。
        /// </summary>
        public string getBGMLabel・曲名を取得(string _BGMFileName_FullPath・曲のフルパス)
        {
            return p_sound・サウンド管理者.getLabelBGM・ラベルを取得(_BGMFileName_FullPath・曲のフルパス);
        }
        /// <summary>
        /// 引数を効果音とした場合の表示名（例：sample＿ピコーン.wavの"ピコーン"など）を取得します。
        ///         /// 効果音の情報を視覚的に認識可能な、ラベルを取得します。
        /// 基本的には曲ファイル名の拡張子を抜いた文字列「曲名」や、効果音ファイル名の"_"や"＿"や"・"で区切られた最後「ピコーン」などの擬音語が格納されるようになっています。
        /// </summary>
        public string getSELabel・ＳＥ擬音語を取得(string _SEFileName_FullPath・効果音のフルパス)
        {
            return p_sound・サウンド管理者.getLabelSE・ラベルを取得(_SEFileName_FullPath・効果音のフルパス);
        }

        // 以下、オーディオファイルを扱うプロパティやメソッド群
        /// <summary>
        /// 再生可能なオーディオファイル名（「データベース」ディレクトリ下の全てのwav、mp3、wmaファイル）のリストを作成
        /// </summary>
        List<string> p_AudioFileList_FullPath・ゲーム内で認識済みの全オーディオファイルリスト;
        /// <summary>
        /// 再生可能な曲ファイル名（ＢＧＭディレクトリ下の全てのmp3ファイル）のリストを作成
        /// </summary>
        List<string> p_BGMFileList_FullPath・曲ファイルリスト;
        /// <summary>
        /// 再生可能な曲ファイル名（効果音ディレクトリ下の全てのmp3ファイル）のリストを作成
        /// </summary>
        List<string> p_SEFileList_FullPath・曲を含まない効果音ファイルリスト;
        /// <summary>
        /// ゲーム中に再生可能なオーディオファイル名一覧（フルパス（「C:\\aaa\\bbb\\***.mp3」など）またはフルパスじゃない（「***.mp3」などだけ）をリストで取得します。
        /// 引数により、音楽ファイルだけ、効果音ファイルだけ、を指定できます。
        /// ゲーム開始時に一度呼び出し、情報は格納されているので、高速に取得できます。
        /// 
        /// ※_isUpdate・データベースを更新=trueにすると、前回更新より後に「データベース」フォルダ／ディレクトリに追加されたオーディオファイルを認識できるようになります。
        /// </summary>
        public List<string> getAudioFileList・ゲーム中で再生可能なオーディオファイルリストを取得(bool _isFullPath・フルパスか, bool _isAddBGM・ＢＧＭを含めるか, bool _isAddSE・効果音を含めるか, bool _isUpdate・データベースを更新するか)
        {
            if (p_AudioFileList_FullPath・ゲーム内で認識済みの全オーディオファイルリスト == null
                || _isUpdate・データベースを更新するか == true)
            {
                long _time1 = getTime・現在時刻();
                
                // 再生可能なファイル名（「データベース」ディレクトリ下の全てのオーディオファイル名）のリストを作成
                p_AudioFileList_FullPath・ゲーム内で認識済みの全オーディオファイルリスト = MyTools.getFileNames_FromDirectoryName(
                    Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス, true, true, true, "wav", "mp3", "wma"); //, "ogg");
                // 再生可能なファイル名（「データベース」ディレクトリ下の全てのオーディオファイル名）のリストを作成
                p_BGMFileList_FullPath・曲ファイルリスト = MyTools.getFileNames_FromDirectoryName(
                    Program・実行ファイル管理者.p_BGMDirectory_FullPath・曲フォルダパス, true, true, true, "mp3", "wma", "wav"); //, "ogg");
                p_SEFileList_FullPath・曲を含まない効果音ファイルリスト = MyTools.getFileNames_FromDirectoryName(
                    Program・実行ファイル管理者.p_SEDirectory_FullPath・効果音ファルダパス, true, true, true, "wav", "mp3", "wma"); //, "ogg");

                DEBUGデバッグ一行出力("getAudioFileList: 「データベース」内のオーディオファイルリスト更新に "+ (getTime・現在時刻() - _time1) + "ミリ秒かかったよ。");
            }
            List<string> _list;
            if (_isAddBGM・ＢＧＭを含めるか == true && _isAddSE・効果音を含めるか == true)
            {
                _list = p_AudioFileList_FullPath・ゲーム内で認識済みの全オーディオファイルリスト;
            }
            else if (_isAddBGM・ＢＧＭを含めるか == true && _isAddSE・効果音を含めるか == false)
            {
                _list = p_BGMFileList_FullPath・曲ファイルリスト;
            }
            else if (_isAddBGM・ＢＧＭを含めるか == false && _isAddSE・効果音を含めるか == true)
            {
                // (a)一応曲も全部含める
                _list = p_AudioFileList_FullPath・ゲーム内で認識済みの全オーディオファイルリスト;
                // (b)効果音のみ
                //_list = p_SEFileList_FullPath・曲を含まない効果音ファイルリスト;
            }
            else
            {
                _list = new List<string>(); // 要素数0の空のリスト
            }
            if (_isFullPath・フルパスか == false)
            {
                for(int i=0; i<_list.Count; i++){
                    _list[i] = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_list[i]);
                }
            }
            return _list;
        }

        // サウンドのリストを作成
        /// <summary>
        /// 引数の参照名（EMusic・曲やESE・効果音やファイル名）から、
        /// そのオーディオファイル名のフルパスを取得します。
        /// </summary>
        public string getAudioFileName_FullPath・オーディオファイル名を取得(string _audioDataName・参照名)
        {
            return getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_audioDataName・参照名);
        }
        /// <summary>
        /// 引数の参照名（EMusic・曲やESE・効果音やファイル名）から、
        /// そのオーディオファイル名のフルパスを取得します。
        /// </summary>
        public string getAudioFileName_FullPath・オーディオファイル名を取得(ESE・効果音 _ESE・効果音)
        {
            return getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_ESE・効果音);
        }
        /// <summary>
        /// 引数の参照名（EMusic・曲やESE・効果音やファイル名）から、
        /// そのオーディオファイル名のフルパスを取得します。
        /// </summary>
        public string getAudioFileName_FullPath・オーディオファイル名を取得(EBGM・曲 _EMusic・曲)
        {
            return getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_EMusic・曲);
        }

        /// <summary>
        /// ゲーム中で定義されているEMusic・曲の抽象的な名前リストを取得します。
        /// </summary>
        public List<string> getBGMNameList・ＢＧＭの抽象名リストを取得()
        {
            // ゲーム中で定義されている（EMusic、ESEの）オーディオ名リストを作成
            List<string> _MusicNameList = new List<string>();
            string _name = "";
            foreach (EBGM・曲 _eMusic in MyTools.getEnumItems<EBGM・曲>())
            {
                _name = game.getP_Sound・サウンド管理者().getAudioDataName・参照名を取得(_eMusic);
                _MusicNameList.Add(_name);
            }
            return _MusicNameList;
        }
        /// <summary>
        /// ゲーム中で定義されているESE・効果音の抽象的な名前リストを取得します。
        /// </summary>
        public List<string> getSENameList・効果音の抽象名リストを取得()
        {
            // ゲーム中で定義されている（EMusic、ESEの）オーディオ名リストを作成
            List<string> _SENameList = new List<string>();
            string _name = "";
            foreach (ESE・効果音 _eSE in MyTools.getEnumItems<ESE・効果音>())
            {
                _name = game.getP_Sound・サウンド管理者().getAudioDataName・参照名を取得(_eSE);
                _SENameList.Add(_name);
            }
            return _SENameList;
        }

        /// <summary>
        /// 曲のファイル名（フルパス）をランダムに選曲します。
        /// 引数で、データベース内にあるmp3ファイルを含めるかどうかを設定できます。
        /// </summary>
        public string getRandomBGM_FullPath・曲ファイル名をランダム取得(bool _isIncludingRowMP3・生mp3ファイルも含めるか)
        {
            string _fileName_FullPath = "";
            List<string> _list;
            if (_isIncludingRowMP3・生mp3ファイルも含めるか == true)
            {
                _list = getAudioFileList・ゲーム中で再生可能なオーディオファイルリストを取得(true, true, false, false);
            }
            else
            {
                _list = new List<string>(MyTools.getEnumNames<EBGM・曲>());
            }
            while (MyTools.isExist(_fileName_FullPath) == true)
            {
                _fileName_FullPath = game.getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(
                    MyTools.getRandomString(_list)  );
            }
            return _fileName_FullPath;
        }
        /// <summary>
        /// 効果音のファイル名（フルパス）をランダムに選曲します。
        /// 引数で、データベース内にあるファイルを含めるかどうかを設定できます。
        /// </summary>
        public string getRandomSE_FullPath・効果音ファイル名をランダム取得(bool _isIncludingRowAudioFile・生ファイルも含めるか)
        {
            string _fileName_FullPath = "";
            List<string> _list;
            if (_isIncludingRowAudioFile・生ファイルも含めるか == true)
            {
                _list = getAudioFileList・ゲーム中で再生可能なオーディオファイルリストを取得(true, false, true, false);
            }
            else
            {
                _list = new List<string>(MyTools.getEnumNames<ESE・効果音>());
            }
            while (MyTools.isExist(_fileName_FullPath) == true)
            {
                _fileName_FullPath = game.getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(
                    MyTools.getRandomString(_list));
            }
            return _fileName_FullPath;
        }
        /// <summary>
        /// （テスト）メソッド内に指定した曲をランダムに選曲して返します。
        /// </summary>
        /// <returns></returns>
        public EBGM・曲 getRnadomMusic・ランダムに曲を選曲()
        {
            if (s_optionBGM_ON・ＢＧＭを再生する状態か == false) return EBGM・曲.__none・無し;

            // ランダムに再生したい曲をここに追加してください（テスト）
            List<EBGM・曲> _randomMusic = new List<EBGM・曲>();
            _randomMusic.Add(EBGM・曲.battle01・通常戦闘＿Shoot_the_thoughts);
            //_randomMusic.Add(EBGM・曲.battle02・通常戦闘＿Wind);
            _randomMusic.Add(EBGM・曲.battleBoss01・ボス戦＿破壊神);
            //_randomMusic.Add(EBGM・曲.battleBoss02・ボス戦＿神聖な瞬間);
            _randomMusic.Add(EBGM・曲.battleBoss05・ボス戦＿規則からの一脱);
            //_randomMusic.Add(EBGM・曲.dangyon01・ダンジョン＿露光る木漏れ日);
            //_randomMusic.Add(EBGM・曲.dangyon02・ダンジョン＿神殿);
            //_randomMusic.Add(EBGM・曲.god01・神聖＿こうして伝説が始まった);
            //_randomMusic.Add(EBGM・曲.sports01・ヘルプ＿一からはじめよう);
            //_randomMusic.Add(EBGM・曲.town01・村＿やさしめの村);

            //_randomMusic.Add(EBGM・曲.battleBoss02・ボス戦＿神聖な瞬間);
            _randomMusic.Add(EBGM・曲.battleBoss02・ボス戦＿強き者に挑む);
            _randomMusic.Add(EBGM・曲.battleBoss03・ボス戦＿Girls_Sword_Rock);
            _randomMusic.Add(EBGM・曲.battle02・通常戦闘＿Wind);

            int _randomIndex = MyTools.getRandomNum(1, _randomMusic.Count - 1);
            return _randomMusic[_randomIndex];
        }



        #endregion

        #region ■■■wウェイト系
        /// <summary>
        /// 指定ミリ秒間，ゲーム描画処理を停止します。いわゆるポーズ状態、画面が固まった状態にします。ただし、バックグラウンドの論理処理や、ユーザからの入力は受け付けます。
        /// ※p_ゲーム描画時間調整倍率により待ち時間が変わります。
        /// ●他のw画面停止メソッドも、最終的にこのメソッドを呼び出します。
        /// </summary>
        /// <param name=""></param>
        public void w画面停止絶対時間(double _秒数)
        {
            int _waitMSec・実際に待つミリ秒数 = (int)(_秒数 * s_gameSpeed・ゲームの速さ倍率 * 1000.0);
            _waitMSec・実際に待つミリ秒数 = MyTools.getAdjustValue(_waitMSec・実際に待つミリ秒数, 0, CGameManager・ゲーム管理者.s_waitMSec_MAX); // 指定秒以上は待たない
            //Console.WriteLine("game.w画面停止 :" + _waitMSec・実際に待つミリ秒数 + " ミリ秒");
            // 画面を停止させるには、draw・描画更新処理を呼び出さないようにするだけ。
            s_isStopDraw・描画更新をストップ = true;
            wウェイト処理待ち絶対時間(_waitMSec・実際に待つミリ秒数, false);
            s_isStopDraw・描画更新をストップ = false;
        }
            #region 他のw画面停止メソッド
        /// <summary>
        /// 指定秒間，ゲーム処理を停止します。入力を受け付けません。
        /// ※p_ゲーム描画時間調整倍率により待ち時間が変わります。
        /// </summary>
        /// <param name=""></param>
        public void w画面停止(int _ミリ秒数)
        {
            w画面停止絶対時間((double)_ミリ秒数 * s_gameSpeed・ゲームの速さ倍率 / 1000.0);
        }
        /// <summary>
        /// 指定ミリ秒間，ゲーム処理を停止します。入力を受け付けません。
        /// ※p_ゲーム描画時間調整倍率により待ち時間が変わります。
        /// </summary>
        /// <param name=""></param>
        public void w画面停止(ESPeed _待ち時間)
        {
            int _ミリ秒数 = getMSec(_待ち時間);
            w画面停止絶対時間(_ミリ秒数);
        }
        /// <summary>
        /// 指定フレーム，ゲーム処理を停止します。入力を受け付けません。
        /// ※p_ゲーム描画時間調整倍率により待ち時間が変わります。
        /// </summary>
        /// <param name=""></param>
        public void w画面停止フレーム(int _停止フレーム数)
        {
            w画面停止(s_FRAME1_MSEC・1フレームミリ秒);
        }
            #endregion
        /// <summary>
        /// 指定秒間，他の処理が終わるのを待ちします。入力を受け付けます。
        /// p_ゲーム描画時間調整倍率やフレームレートなど、ゲーム速度が代わっても、絶対にこの時間待ちます。
        /// ●他のwウェイト処理待ちメソッドも、最終的にこのメソッドを呼び出します。
        /// </summary>
        /// <param name=""></param>
        public void wウェイト処理待ち絶対時間(int _絶対ミリ秒数, bool _フォームに待ち時間を表示するか)
        {
            // この時間だけ待つ（指定秒以上は待たない）
            _絶対ミリ秒数 = MyTools.getAdjustValue(_絶対ミリ秒数, 0, CGameManager・ゲーム管理者.s_waitMSec_MAX);

            if (_フォームに待ち時間を表示するか == true)
            {
                // 待ち時間中はをテストして、次へボタンに「あと○ミリ秒」ラベルを表示            
                if (_絶対ミリ秒数 > 100 &&
                    getP_gameWindow・ゲーム画面() != null && getP_gameWindow・ゲーム画面().getP_usedFrom() != null)//&&
                //p_isWaitingUserInput_NextOrBack・入力待ちフラグ == false)
                {
                    // 100ミリ秒以上で、入力待ちでない場合は、待ち時間を表示する
                    getP_gameWindow・ゲーム画面().getP_usedFrom().setWaitingView・画面に待ち時間を表示するかを設定(true, _絶対ミリ秒数);
                }
            }
            
            // ※描画の更新は別スレッドのupdateFrame()メソッドに任せるから、ここでは待つだけで、他は何もしなくていい
            // ■■■実際に待つ処理（ここで待ってる間に、フォームでイベントが起こったり、updateFrame()メソッドは呼ばれることは普通にある。）
            MyTools.wait_ByApplicationDoEvents(_絶対ミリ秒数);

            if (_フォームに待ち時間を表示するか == true)
            {
                if (_絶対ミリ秒数 > 100 &&
                    getP_gameWindow・ゲーム画面() != null && getP_gameWindow・ゲーム画面().getP_usedFrom() != null)//&&
                //p_isWaitingUserInput_NextOrBack・入力待ちフラグ == false)
                {
                    // 入力待ちでない場合は、待ち時間を表示する
                    getP_gameWindow・ゲーム画面().getP_usedFrom().setWaitingView・画面に待ち時間を表示するかを設定(false, _絶対ミリ秒数);
                }
            }
            
            // 以下、待っている間に何かテストしたい場合は書く。例えば音楽変えるとか、タイミングゲームとか…？
            // 待ち時間中はをテストして、「NowLoading」ラベルを表示？

            // [TEST] 曲ランダム再生と、ボリューム調整
            //if (p_BGMVolume_0to1000 != p_BGMVolume_0to1000_seted)
            //{
            //    pBGM_setVolume(p_BGMVolume_0to1000);
            //}
            // ここに書いても二重になりました・・・＾＾；。
            //if (p_randomBGM == true && p_nowBGMPassedMSec・現在の曲再生ミリ秒 >= s_MusicMinTimePlayingMSec・曲最低持続秒)
            //{
            //    pBGM(getRnadomMusic・ランダムに曲を選曲());
            //    p_randomBGM = false;
            //}
        }
        /// <summary>
        /// 指定ミリ秒間，他の処理が終わるのを待ちします。入力を受け付けます。
        /// ※p_ゲーム描画時間調整倍率により、内部で待ち時間が変わります。
        /// （新しい入力処理も受け付けます，描画処理はそのまま行います）．
        /// </summary>
        /// <param name=""></param>
        public void waitウェイト(int _ミリ秒数)
        {
            waitウェイト(_ミリ秒数, true);
        }
        /// <summary>
        /// 指定ミリ秒間，他の処理が終わるのを待ちします。入力を受け付けます。
        /// ※p_ゲーム描画時間調整倍率により、内部で待ち時間が変わります。
        /// （新しい入力処理も受け付けます，描画処理はそのまま行います）．
        /// </summary>
        /// <param name=""></param>
        public void waitウェイト(int _ミリ秒数, bool _フォームに待ち時間を表示するか)
        {
            int _絶対ミリ秒 = (int)(_ミリ秒数 * s_gameSpeed・ゲームの速さ倍率);

            if (p_isEcoMode・省エネモード == true &&
                _ミリ秒数 == CGameManager・ゲーム管理者.s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒)
            {
                // 省エネモード時は、ユーザ入力回答待ち時間を、ユーザの無入力時間に応じて動的に変化させる（省エネ用）
                // updateFrame()メソッドが動いていないことが前提なので、最後に入力した時刻だけを信用
                int _無入力時間 = Math.Abs(MyTools.getNowTime_fast() - getUserLastInputTime・ユーザが最後に入力した時刻を取得()); // Absは念のため
                if (_無入力時間 < _絶対ミリ秒)
                {
                    // まだ待ってない（一回もwウェイトが呼ばれていない）ので、ユーザ入力回答待ち時間を初期化
                    s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒 = s_waitMSecForUserSelectOrInput_MIN20;
                }
                else // _無入力時間 >= _絶対ミリ秒
                {
                    if (_無入力時間 > s_waitMSecForUserSelectOrInput_EcoModeStartPassedMSec5000)
                    {
                        // 省エネモードにより、無入力時間に応じて、ユーザ入力回答待ち時間を調整し、ループ負荷を徐々に少なくする
                        s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒 += Math.Min(Math.Max(_無入力時間 / 2, 20), 500); // += 無入力時間 / 整数 することで、待つ時間が長ければ長いほど、次に待つ時間も長くなる
                        // ただし、最大値以上にはならない （ユーザがボタンを押しても、数秒間も反応なかったら嫌だもんね）
                        if (s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒 > s_waitMSecForUserSelectOrInput_MAX1000)
                        {
                            s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒 = s_waitMSecForUserSelectOrInput_MAX1000;
                        }
                    }
                }
                // 増減した結果を、代入
                _ミリ秒数 = s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒;
                // 絶対ミリ秒を再計算
                _絶対ミリ秒 = (int)(_ミリ秒数 * s_gameSpeed・ゲームの速さ倍率);
                //デバッグテスト。ちゃんと増加してた。
                //MyTools.ConsoleWriteLine("※省エネモード：ユーザ入力回答待ち時間を "+s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒+"ミリ秒に変更");

            }
            
            wウェイト処理待ち絶対時間(_絶対ミリ秒, _フォームに待ち時間を表示するか);
        }
        // 引数の型間違えたら1000倍待つとかえらいことになるし、ミリ秒で
        ///// <summary>
        ///// 指定秒間，他の処理が終わるのを待ちします。入力を受け付けます。
        ///// ※p_ゲーム描画時間調整倍率により待ち時間が変わります。（新しい入力処理も受け付けます，描画処理はそのまま行います）．
        ///// </summary>
        ///// <param name=""></param>
        //public void waitウェイト(double _秒数)
        //{
        //    int _絶対ミリ秒数 = (int)(_秒数 * s_gameSpeed・ゲームの速さ倍率 * 1000.0);

        //    wウェイト処理待ち絶対時間(_絶対ミリ秒数);
        //}
        /// <summary>
        /// 指定フレーム数、他の処理が終わるのを待ちます。入力を受け付けます。
        /// p_ゲーム描画時間調整倍率により待ち時間が変わります。
        /// </summary>
        public void waitFウェイトフレーム(int _waitFrameNum・待つフレーム数)
        {
            waitウェイト(_waitFrameNum・待つフレーム数*s_FRAME1_MSEC・1フレームミリ秒);
        }
        #endregion

        #region ■■■i入力判定系、ibでゲーム共通のボタン処理、ikでキーボードのキーやマウスボタンを直接みにいく
        /// <summary>
        /// ●ゲームの指定ボタンが押された状態かを返します。
        /// 押しっぱなしにしていてもtrueを返します。連投
        /// 
        /// マウスとキーボードを同じ処理として扱いたい時に使う入力判定処理です。（例：　左クリック＝Enter/Zと同じ処理）
        /// ボタンの変更はCInput・入力操作.setButtonKeysで行ってください。
        ///  
        /// ※キー押しっぱなしをボタン連打に対応したい（1フレーム毎にtrueを連続して返したい）操作の場合は、このメソッドを使ってください。
        /// なお、押し離した瞬間だけを検知したい操作の場合は、「ibボタンを押したか」を使ってください。
        /// なお、キー個別に扱いたい場合は、「i指定キーが押されているか」などを使ってください。
        /// </summary>
        /// <param name="_Ckeycode・入力キー"></param>
        /// <returns></returns>
        public bool ibボタンを押し中か_連射対応(EInputButton・入力ボタン _inputButton・入力ボタン)
        {
            return p_inputButton・ボタン入力.isPressing・ボタンを押し中か(_inputButton・入力ボタン);
        }
        /// <summary>
        /// ●ゲームの指定ボタンが押し離された瞬間か（前回フレーム更新時に押されていて、今回離された）かを返します。押しっぱなしにしているとfalseを返します。
        /// 
        /// マウスとキーボードを同じ処理として扱いたい時に使う入力判定処理です。（例：　左クリック＝Enter/Zと同じ処理）
        /// ボタンの変更はCInput・入力操作.setButtonKeysで行ってください。
        ///  
        /// </summary>
        /// <param name="_Ckeycode・入力キー"></param>
        /// <returns></returns>
        public bool ibボタンを押したか_連射非対応(EInputButton・入力ボタン _inputButton・入力ボタン)
        {
            return p_inputButton・ボタン入力.isPulled・ボタンを押し離した瞬間か(_inputButton・入力ボタン);
        }
        #region 引数の異なるメソッド
        /// <summary>
        /// どれか一つでもボタンを押し続けていたらTrueを返します。
        /// </summary>
        /// <returns></returns>
        public bool ibボタンを押し中か_連射対応()
        {
            bool _is押し中か = false;
            for (int i = 0; i < MyTools.getEnumIntMaxCount<EInputButton・入力ボタン>(); i++)
            {
                if (p_inputButton・ボタン入力.isPressing・ボタンを押し中か(i) == true)
                {
                    _is押し中か = true;
                    break;
                }
            }
            return _is押し中か;
        }
        /// <summary>
        ///  ●ゲームの指定ボタンが押し離された瞬間か（前回フレーム更新時に押されていて、今回離された）かを返します。押しっぱなしにしているとfalseを返します。
        /// 
        /// どれか一つでもボタンを押したらTrueを返します。
        /// </summary>
        /// <param name="_inputButton・入力ボタン"></param>
        /// <returns></returns>
        public bool ibボタンを押したか_連射非対応()
        {
            bool _is押したか = false;
            for (int i = 0; i < MyTools.getEnumIntMaxCount<EInputButton・入力ボタン>(); i++)
            {
                if (p_inputButton・ボタン入力.isPulled・ボタンを押し離した瞬間か(i) == true)
                {
                    _is押したか = true;
                    break;
                }
            }
            return _is押したか;
        }
        #endregion
        #region 同時押しメソッド
        /// <summary>
        ///  ●ゲームの２つの指定ボタンが同時押したか（瞬間的な同時押し、もしくはどちらか一方が押されたまま、もう一方のボタンが押し離された瞬間か（前回フレーム更新時に押されていて、今回離された））を返します。押しっぱなしにしているとfalseを返します。
        ///   
        /// ※キー押しっぱなしをボタン連打に対応したい（1フレーム毎にtrueを連続して返したい）操作の場合は、このメソッドを使ってください。
        /// なお、押し離した瞬間だけを検知したい操作の場合は、「ibボタンを押したか」を使ってください。
        /// なお、キー個別に扱いたい場合は、「i指定キーが押されているか」などを使ってください。
        /// </summary>
        /// <param name="_Ckeycode・入力キー"></param>
        /// <returns></returns>
        public bool ibボタンを同時押したか_連射非対応(EInputButton・入力ボタン _inputButton・入力ボタン1, EInputButton・入力ボタン _inputButton・入力ボタン2)
        {
            bool _is同時押し = false;
            // どちらか一方を押しながら、どちらか一方を押した時も対応
            if ((ibボタンを押したか_連射非対応(_inputButton・入力ボタン1) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン2) == true)
            || (ibボタンを押し中か_連射対応(_inputButton・入力ボタン1) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン2) == true)
            || (ibボタンを押したか_連射非対応(_inputButton・入力ボタン1) == true && ibボタンを押し中か_連射対応(_inputButton・入力ボタン2) == true) )
            {
                _is同時押し = true;
            }
            return _is同時押し;
        }
        /// <summary>
        ///  ●ゲームの３つの指定ボタンが同時押したか（瞬間的な同時押し、もしくはどれか２つが押されたまま、残る１つが押し離された瞬間か（前回フレーム更新時に押されていて、今回離された））を返します。押しっぱなしにしているとfalseを返します。
        ///   
        /// ※キー押しっぱなしをボタン連打に対応したい（1フレーム毎にtrueを連続して返したい）操作の場合は、このメソッドを使ってください。
        /// なお、押し離した瞬間だけを検知したい操作の場合は、「ibボタンを押したか」を使ってください。
        /// なお、キー個別に扱いたい場合は、「i指定キーが押されているか」などを使ってください。
        /// </summary>
        /// <param name="_Ckeycode・入力キー"></param>
        /// <returns></returns>
        public bool ibボタンを同時押したか_連射非対応(EInputButton・入力ボタン _inputButton・入力ボタン1, EInputButton・入力ボタン _inputButton・入力ボタン2, EInputButton・入力ボタン _inputButton・入力ボタン3)
        {
            bool _is同時押し = false;
            // どちらか２つを押しながら、残る１つを押した時も対応（必ずどれか一つは非対応にしないと押し続けONになってしまうので注意）
                // 1が連射非対応の時
            if ((ibボタンを押したか_連射非対応(_inputButton・入力ボタン1) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン2) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン3) == true)
            || (ibボタンを押したか_連射非対応(_inputButton・入力ボタン1) == true && ibボタンを押し中か_連射対応(_inputButton・入力ボタン2) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン3) == true)
            || (ibボタンを押したか_連射非対応(_inputButton・入力ボタン1) == true && ibボタンを押し中か_連射対応(_inputButton・入力ボタン2) == true && ibボタンを押し中か_連射対応(_inputButton・入力ボタン3) == true)
                // 1が連射対応の時
            || (ibボタンを押し中か_連射対応(_inputButton・入力ボタン1) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン2) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン3) == true)
            || (ibボタンを押し中か_連射対応(_inputButton・入力ボタン1) == true && ibボタンを押し中か_連射対応(_inputButton・入力ボタン2) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン3) == true)
            || (ibボタンを押し中か_連射対応(_inputButton・入力ボタン1) == true && ibボタンを押したか_連射非対応(_inputButton・入力ボタン2) == true && ibボタンを押し中か_連射対応(_inputButton・入力ボタン3) == true))
            {
                _is同時押し = true;
            }
            return _is同時押し;
        }
        #endregion
        #region 長押しメソッドの作り方
        // キーの長押しメソッドは、EKeyCode.***LONGPRESSを追加してね。
        // ボタンやEInput・入力操作に新しい長押しボタンを定義して、それにEKeyCode.***LONGPRESSを追加してね。
        /// <summary>
        /// ●ゲームの指定ボタンが長押しされたか瞬間か（ずっと押されていて、今回のフレームで一定閾値を超えたか）かを返します。１回だけ検出し、その後は押しっぱなしにしていてもfalseを返します。
        /// 
        /// マウスとキーボードを同じ処理として扱いたい時に使う入力判定処理です。（例：　左クリック＝Enter/Zと同じ処理）
        /// ボタンの変更はCInput・入力操作.setButtonKeysで行ってください。
        ///  
        /// </summary>
        /// <param name="_Ckeycode・入力キー"></param>
        /// <returns></returns>
        //public bool ibボタンを長押ししたか_連射非対応(EInputButton・入力ボタン _inputButton・入力ボタン)
        //{
        //    if(check長押し対応ボタンかどうか調べる()==true)
        //    return p_inputButton・ボタン入力.isPulled・ボタンを押し離した瞬間か(_inputButton・入力ボタン);
        //}
        #endregion
        #region 指定キーの入力判定メソッド
        /// <summary>
        /// ※できれば、ib指定ボタンが押し状態か()、などを使ってください（マウスとキーボードを共通に扱いたいため）
        /// 
        /// キーボードの指定キーが押された状態かを返します。押しっぱなしにしていても1フレーム毎にtrueを返します。
        /// キー押しっぱなしをボタン連打に対応したい操作の場合は、このメソッドを使ってください。
        /// なお、押し離した瞬間だけを検知したい場合は、別のメソッドを使ってください。
        /// </summary>
        /// <param name="_Ckeycode・入力キー"></param>
        /// <returns></returns>
        public bool ik指定キーを押し中か_押しっぱ連射対応(EKeyCode _Ckeycode・入力キー)
        {
            return getP_keyboardInput().IsPress(_Ckeycode・入力キー);
        }
        // 下記は、上記メソッド==falseで検出できる？
        ///// <summary>
        ///// ※できれば、ib指定ボタンが押し状態か()、などを使ってください（マウスとキーボードを共通に扱いたいため）
        /////
        ///// キーボードの指定キーが離されているか（前回更新時に押されていなくて、今回もそのまま）かを返します。押しっぱなしにしているとfalseを返します。
        ///// 押し離した瞬間を検知したい場合は、別のメソッドを使ってください。
        ///// </summary>
        ///// <param name="_Ckeycode・入力キー"></param>
        ///// <returns></returns>
        //public bool ik指定キーが離されているか(EKeyCode _Ckeycode・入力キー)
        //{
        //    return getP_keyboardInput().IsFree(_Ckeycode・入力キー);
        //}
        /// <summary>
        /// ※できれば、ib指定ボタンが押し状態か()、などを使ってください（マウスとキーボードを共通に扱いたいため）
        /// 
        /// キーボードの指定キーが押し離した瞬間か（前回更新時に押していて、今回離された）かを返します。押しっぱなしにしているとfalseを返します。
        /// 
        /// キー押しっぱなしをボタン連打に対応「したくない」操作の場合は、このメソッドを使ってください。
        /// </summary>
        /// <param name="_Ckeycode・入力キー"></param>
        /// <returns></returns>
        public bool ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode _Ckeycode・入力キー)
        {
            return getP_keyboardInput().IsPull(_Ckeycode・入力キー);
        }
        /// <summary>
        /// ※できれば、ib指定ボタンが押し状態か()、などを使ってください（マウスとキーボードを共通に扱いたいため）
        /// 
        /// キーボードの指定キーが押し離した瞬間か（前回更新時に押していて、今回離された）かを返します。押しっぱなしにしているとfalseを返します。
        /// 
        /// キー押しっぱなしをボタン連打に対応「したくない」操作の場合は、このメソッドを使ってください。
        /// </summary>
        /// <param name="_Ckeycode・入力キー"></param>
        /// <returns></returns>
        public bool ik指定キーが押し離した瞬間か_押しっぱ連射非対応()
        {
            return getP_keyboardInput().IsPull();
        }
        // ２キー同時押し（３キー以上は今は認識させていない）
        /// <summary>
        /// ※できれば、ib指定ボタンが押し状態か()、などを使ってください（マウスとキーボードを共通に扱いたいため）
        ///   
        /// キーボードの指定キーが押し離した瞬間か（前回更新時に押していて、今回離された）かを返します。押しっぱなしにしているとfalseを返します。
        /// 
        /// キー押しっぱなしをボタン連打に対応「したくない」操作の場合は、このメソッドを使ってください。
        /// </summary>
        /// <param name="_Ckeycode・入力キー1"></param>
        /// <returns></returns>
        public bool ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode _Ckeycode・入力キー1, EKeyCode _Ckeycode・入力キー2)
        {
            bool _is同時押し = false;
            // どちらか一方を押しながら、どちらか一方を押した時も対応
            if ((ik指定キーが押し離した瞬間か_押しっぱ連射非対応(_Ckeycode・入力キー1) == true && ik指定キーを押し中か_押しっぱ連射対応(_Ckeycode・入力キー2) == true)
            || (ik指定キーが押し離した瞬間か_押しっぱ連射非対応(_Ckeycode・入力キー1) == true && ik指定キーを押し中か_押しっぱ連射対応(_Ckeycode・入力キー2) == true)
            || (ik指定キーが押し離した瞬間か_押しっぱ連射非対応(_Ckeycode・入力キー1) == true && ik指定キーを押し中か_押しっぱ連射対応(_Ckeycode・入力キー2) == true))
            {
                _is同時押し = true;
            }
            return _is同時押し;
        }
        #endregion

        #endregion

        #region ■■■wI任意ボタン／wA決定ボタン入力待ち系、メッセージ送りなどに便利
        /// <summary>
        /// ●ユーザの入力待ちを確認する共通処理です。ユーザの入力待ちであればfalse、ユーザの入力を完了したらtrueを返します。
        /// 具体的には、ゲーム画面にフォーカスが当たっていて、かつp_isEndUserInput_GoNextOrBack・入力待ち完了フラグがtrueであれば、trueを返します。
        /// </summary>
        public bool isEndUserInput・ユーザの入力が完了したか()
        {
            bool _isEndUserInput = false;
            // (a)ゲーム画面がアクティブで、かつ入力待ち完了フラグがtrueだったら、入力完了
            //if (getP_gameWindow・ゲーム画面() != null && getP_gameWindow・ゲーム画面().getP_usedFrom() != null && 
            //    Form.ActiveForm == getP_gameWindow・ゲーム画面().getP_usedFrom() && // フォームがアクティブかどうか
            //    p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ == true)
            //{
            // (b)画面アクティブ確認は面倒だから、これで済ませてしまうバージョン。
            if (p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ == true)
            {
                _isEndUserInput = true;
            }
            return _isEndUserInput;
        }
        /// <summary>
        /// ●ユーザの入力待ちを開始／終了する時に行う共通処理です。isEndUserInput・ユーザの入力が完了したか()メソッドの前後に呼び出してください。
        /// 具体的には、game.p_isWaitingUserInput_NextOrBack・入力待ちフラグ、p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ、p_isGoBack・前に戻る入力フラグを初期化します。
        /// 
        /// ※isEndUserInput・ユーザの入力が完了したか()メソッド以外の、全てのgame.W***入力待ち()メソッドがこのメソッドを呼び出します。それ以外でも、外部から入力待ちをする時も、このメソッドを呼び出してください。
        /// </summary>
        public void WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(bool _is入力待ち開始ならＴｒｕｅ＿終了ならＦａｌｓｅ)
        {
            if (_is入力待ち開始ならＴｒｕｅ＿終了ならＦａｌｓｅ == true)
            {
                p_isWaitingUserInput_NextOrBack・入力待ちフラグ = true;
            }
            else
            {
                p_isWaitingUserInput_NextOrBack・入力待ちフラグ = false;
            }
            p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = false;
            p_isUserInput_Back・前に戻る入力フラグ = false;
        }
        // ■■入力待ち
        /// <summary>
        /// game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true になるまで待ちます。
        /// （※コマンド入力、画面遷移など、上記フラグを操作するあらゆる入力完了待ちに使用できます。）
        /// 　　　　引数で入力受付最小時間を設定できます。引数を無しにすると、デフォルトでs_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒が入ります。
        /// </summary>
        public void wIa特定入力完了待ち()
        {
            wIa特定入力完了待ち(s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒);
        }
        /// <summary>
        /// game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true になるまで待ちます。
        /// （※コマンド入力、画面遷移など、上記フラグを操作するあらゆる入力完了待ちに使用できます。）
        /// 　　　　引数で入力受付最小時間を設定できます。引数を無しにすると、デフォルトでs_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒が入ります。
        /// </summary>
        public void wIa特定入力完了待ち(int _waitMSec)
        {
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
            while (game.isEndUserInput・ユーザの入力が完了したか() == false)
            {
                game.waitウェイト(_waitMSec);
            }
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);
        }
        /// <summary>
        /// 任意のボタン（ゲームで認識しているあらゆるボタン、キーボードのあらゆるキー、マウスのあらゆるボタン、EnterやSpace，マウスクリックなど）が押されるまで待ちます．
        /// ただし，メインメッセージボックスにフォーカスが当たっている時は，クリックしても次に進みません。
        /// （※通常のメッセージ送りなどに使用します。）
        /// 　　　　引数で入力受付最小時間を設定できます。引数を無しにすると、デフォルトでs_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒が入ります。
        /// </summary>
        public void wIn任意ボタン入力待ち()
        {
            wIn任意ボタン入力待ち(s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒);
        }
        /// <summary>
        /// 任意のボタン（ゲームで認識しているあらゆるボタン、キーボードのあらゆるキー、マウスのあらゆるボタン、EnterやSpace，マウスクリックなど）が押されるまで待ちます．
        /// ただし，メインメッセージボックスにフォーカスが当たっている時は，クリックしても次に進みません。
        /// （※通常のメッセージ送りなどに使用します。）
        /// 　　　　引数で入力受付最小時間を設定できます。引数を無しにすると、デフォルトでs_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒が入ります。
        /// </summary>
        public void wIn任意ボタン入力待ち(int _waitMSec)
        {
            WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
            // p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ=trueが最優先
            // （Windowsフォーム用の「次に進む」ボタンが押されるまで、他のボタンを押しても無効とする）
            while (isEndUserInput・ユーザの入力が完了したか() == false ||
                (
                ibボタンを押したか_連射非対応() == false
                && ik指定キーが押し離した瞬間か_押しっぱ連射非対応() == false
                && p_mouseInput・マウス入力.IsPush(EMouseButton.Left) == false
                && Program・実行ファイル管理者.isEnd == false
                )
            )
            {
                waitウェイト(_waitMSec);
            }
            WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);
        }
        /// <summary>
        /// ゲームで認識可能な指定ボタンが押されるまで待ちます．
        /// 　　　　引数２で入力受付最小時間を設定できます。引数を無しにすると、デフォルトでs_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒が入ります。
        /// </summary>
        public void wIb指定ボタン入力待ち(EInputButton・入力ボタン _次へ進む指定ボタン)
        {
            wIb指定ボタン入力待ち(_次へ進む指定ボタン, s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒);
        }
        /// <summary>
        /// ゲームで認識可能な指定ボタンが押されるまで待ちます．
        /// 　　　　引数２で入力受付最小時間を設定できます。引数を無しにすると、デフォルトでs_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒が入ります。
        /// </summary>
        public void wIb指定ボタン入力待ち(EInputButton・入力ボタン _次へ進む指定ボタン, int _waitMSec)
        {
            WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
            while (ibボタンを押したか_連射非対応(_次へ進む指定ボタン) == false
                //&& p_mouseInput・マウス入力.IsPush(EMouseButton.Left) == false
                //&& p_mouseKeyBoardInput・キー入力.IsPress(EKeyCode.ENTER) == false
                //&& p_mouseKeyBoardInput・キー入力.IsPress(EKeyCode.z) == false
                && Program・実行ファイル管理者.isEnd == false)
            {
                waitウェイト(_waitMSec);
            }
            WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);
        }
        /// <summary>
        /// 決定ボタン（デフォルトでは、決定ボタンAの他に、Enterキー，zキー、左マウスクリックが対応されている）が押されるまで待ちます．
        /// 　　　　引数で入力受付最小時間を設定できます。引数を無しにすると、デフォルトでs_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒が入ります。
        /// </summary>
        public void wA決定ボタン入力待ち()
        {
            wA決定ボタン入力待ち(s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒);
        }
        /// <summary>
        /// 決定ボタン（デフォルトでは、決定ボタンAの他に、Enterキー，zキー、左マウスクリックが対応されている）が押されるまで待ちます．
        /// 　　　　引数で入力受付最小時間を設定できます。引数を無しにすると、デフォルトでs_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒が入ります。
        /// </summary>
        public void wA決定ボタン入力待ち(int _waitMSec)
        {
            WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
            // p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ=trueが最優先
            // （Windowsフォーム用の「次に進む」ボタンが押されるまで、他のボタンを押しても無効とする）
            while (isEndUserInput・ユーザの入力が完了したか() == false ||
                (
                ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b1_決定ボタン_A) == false
                && p_mouseInput・マウス入力.IsPush(EMouseButton.Left) == false
                && p_mouseKeyBoardInput・キー入力.IsPress(EKeyCode.ENTER) == false
                && p_mouseKeyBoardInput・キー入力.IsPress(EKeyCode.z) == false
                && Program・実行ファイル管理者.isEnd == false
                )
            )
            {
                waitウェイト(_waitMSec);
            }
            WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);
        }

        // ■■特定ボタンを自動押しする処理
        /// <summary>
        /// ユーザの代わりに、決定ボタン自動押しします。
        /// 決定ボタンを一瞬だけ自動で押して、これまでの処理を決定しつつ、次の処理に行きたい時に呼び出す処理です。
        /// （よくわからない場合は呼びださなくて可）
        /// </summary>
        public bool iA決定ボタンを一瞬だけ自動で押す()
        {
            // 実際に決定ボタンを押すエミュレート処理
            bool _isSuccess = getP_InputButton().autoInputButton・ボタン自動入力(EInputButton・入力ボタン.b1_決定ボタン_A);
            if (_isSuccess == true)
            {
                // 決定ボタンで必要な処理
                p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
            }
            return _isSuccess;
        }
        /// <summary>
        /// ユーザの代わりに、戻るボタン自動押しをします。
        /// 戻るボタンを一瞬だけ自動で押して、前の処理に戻りたい時に呼び出す処理です。
        /// （よくわからない場合は呼びださなくて可）
        /// </summary>
        public bool iB戻るボタンを一瞬だけ自動で押す()
        {
            // 実際に戻るボタンを押すエミュレート処理
            bool _isSuccess = getP_InputButton().autoInputButton・ボタン自動入力(EInputButton・入力ボタン.b2_戻るボタン_B);
            if (_isSuccess == true)
            {
                //戻るボタンで必要な処理
                p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
                p_isUserInput_Back・前に戻る入力フラグ = true;
            }
            return _isSuccess;
        }
        /// <summary>
        /// ユーザの代わりに、指定ボタン自動押しをします。
        /// 任意のボタンをエミュレートしたい時に呼び出す処理です。
        /// （よくわからない場合は呼びださなくて可）
        /// </summary>
        public bool iN指定ボタンを一瞬だけ自動で押す(EInputButton・入力ボタン _autoInputButton)
        {
            if (_autoInputButton == EInputButton・入力ボタン.b1_決定ボタン_A)
            {
                // 決定ボタンの場合はそちらを使う
                return iA決定ボタンを一瞬だけ自動で押す();
            }
            else if (_autoInputButton == EInputButton・入力ボタン.b2_戻るボタン_B)
            {
                // 戻るボタンの場合はそちらを使う
                return iB戻るボタンを一瞬だけ自動で押す();
            }
            else
            {
                // 実際に指定ボタンを押すエミュレート処理
                bool _isSuccess = getP_InputButton().autoInputButton・ボタン自動入力(_autoInputButton);
                if (_isSuccess == true)
                {
                    //指定ボタンで必要な処理
                }
                return _isSuccess;
            }
        }
        /// <summary>
        /// ユーザの入力待ち状態の時にtrueにして、入力が終了したらfalseになるように設計された、入力待ちフラグです。
        /// ※変更する時は、必ず、WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化()メソッドを使ってください。
        /// 
        /// 画面に入力待ちであることを示すアニメーションを表示したり、(falseの時)入力待ち以外の待ち時間を表示したりする時に使っています。
        /// 詳しくは、すべての参照を検索してください。
        /// </summary>
        public bool p_isWaitingUserInput_NextOrBack・入力待ちフラグ = false;
        /// <summary>
        /// ユーザの入力待ち後、ユーザの入力が完了した（「次に進む」／「前に戻る」処理を意味するキーやボタンを押した、あるいはそれらと同等を意味する画面上のコントロールの処理を完了／キャンセルした）時にtrueになり、その後すぐにfalseに戻るフラグです。
        /// ※原則として、戻る処理（キャンセル処理を含む）をした場合も、trueになります。
        /// 
        /// 入力完了時、ユーザが入力した処理が戻る処理（キャンセル処理を含む）の場合は、
        /// このフラグとあわせて、p_isBack・前に戻る入力フラグをtrueにしてください。
        /// 
        /// 詳しくはすべての参照を検索してください。
        /// </summary>
        public bool p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = false;
        /// <summary>
        /// ユーザの入力が、「前に戻る」処理（キャンセル処理を含む）を意味する場合にtrueになり、
        /// 「次に進む」処理ならfalseになるフラグです。
        /// なお、このフラグをtrueやfalseにするときは、p_isGoNextOrBack・入力完了フラグもあわせてtrueにしてください。
        ///
        /// 詳しくはすべての参照を検索してください。
        /// </summary>
        public bool p_isUserInput_Back・前に戻る入力フラグ = false;
        #region その他草案
        // 以下、草案
        // 下記のようにフラグをそれぞれの処理で作っていたら、コントロールの分だけ作らないといけないから、
        // 上記のisGoNextとisGoBackの２つで済ませられるよう、メソッド内で完結させる。
        ///// <summary>
        ///// 入力ボックスを表示する前にfalseに更新され、入力ボックスを完了（確定あるいはキャンセル）した時だけtrueになり、その後すぐfalseに戻るフラグです。詳しくはすべての参照を検索してください。
        ///// </summary>
        //public bool p_isEndInputBox・入力ボックス完了フラグ = false;
        ///// <summary>
        ///// 選択肢を表示する前にfalseに更新され、選択肢を完了（確定あるいはキャンセル）した時だけtrueになり、その後すぐfalseに戻るフラグです。
        ///// </summary>
        //public bool p_isEndSelectBox・選択ボックス完了フラグ = false;
        ///// <summary>
        ///// 他のフォームを表示する前にfalseに更新され、他のフォーム画面で処理を完了した時だけtrueになり、その後すぐfalseに戻るフラグです。詳しくはすべての参照を検索してください。
        ///// </summary>
        //public bool p_isOtherWindowEnded・他の画面での処理を完了して元の画面に戻った入力フラグ = false;

        // 以下、草案
        // これをやると、いろんな勘違いが生まれやすい（このメソッドを呼び出すだけで次へ進む処理や前に戻る処理が自動的に出来ると思いこむ）ので実装しない。
        ///// <summary>
        ///// ユーザの入力待ちを完了し、次に進みます。
        ///// </summary>
        //public void GoNext次に進む()
        //{
        //    p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
        //}
        ///// <summary>
        ///// ユーザの入力待ちを完了し、次に進みます。
        ///// </summary>
        //public void GoBack前に戻る()
        //{
        //    p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
        //}

        ///// <summary>
        ///// 入力決定される（入力ボックスに入力して決定する）まで待ちます．
        ///// </summary>
        //public void i入力待ち()
        //{
        //    n入力待ち();
        //}
        ///// <summary>
        ///// 入力決定される（入力ボックスに入力して決定する）まで待ちます．
        ///// </summary>
        //public void e入力待ち()
        //{
        //    // [TODO]とりあえず・・・
        //    //string _defaultAnswer = getP_gameWindow・ゲーム画面().getDefaultInputText();
        //    while (true &&
        //    p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ == false && p_mouseInput・マウス入力.IsPush(MouseInput.Button.Left) == false && p_mouseKeyBoardInput・キー入力.IsPress(KeyCode.ENTER) == false && p_mouseKeyBoardInput・キー入力.IsPress(KeyCode.SPACE) == false && Program.isEnd == false)
        //    {
        //        //getP_gameWindow・ゲーム画面().getP_usedFrom().Refresh();
        //        waitウェイト(s_FRAME1_MSEC・1フレームミリ秒);
        //    }
        //    p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = false;
        //    //while(p_)
        //}
        #endregion

        #endregion
        

        // 2.メッセージ処理
        #region ■■■mメッセージ系
        /// <summary>
        /// メッセージを表示します。全てのメッセージ描画メソッドはこのメソッドを呼び出します。
        /// </summary>
        private void メッセージを表示(string _文字列_改行はエンマークn, bool _true一文字ずつ表示_false一挙に表示)
        {
            // ■■全てのメッセージ描画はこのメソッドを呼び出す
            getP_gameWindow・ゲーム画面().getP_messageBox().showMessage・メッセージを表示(_文字列_改行はエンマークn, _true一文字ずつ表示_false一挙に表示);
        }
        /// <summary>
        /// メッセージウィンドウを初期化します。（"【c】"を入力します）
        /// </summary>
        public void mメッセージボックスを初期化()
        {
            メッセージを表示("【c】", false);
        }
        /// <summary>
        /// （※主に標準のメッセージ表示に使います．）メッセージの全てを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ_ボタン送り(string _一行文字列_途中改行はエンマークn)
        {
            mメッセージ単語_末尾改行なし_ボタン送り(_一行文字列_途中改行はエンマークn + "\n");
        }
        /// <summary>
        /// （※主にシナリオのメッセージ表示に使います．）メッセージを画面に表示し、一行ずつ任意のボタンが入力されるまで待ちます．※最後に改行が自動的に入ります．改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ_一行ずつボタン送り(string _文字列_改行はエンマークn)
        {
            int _行数 = MyTools.getLineNo(_文字列_改行はエンマークn);
            for (int i = 1; i <= _行数; i++)
            {
                string _i行目 = MyTools.getLineString(_文字列_改行はエンマークn, i);
                メッセージを表示(_i行目 + "\n", true);
                wIn任意ボタン入力待ち();
            }
            // 最後に次のメッセージに行くまで待つ
            wIn任意ボタン入力待ち();
        }
        /// <summary>
        /// （※主に戦闘メッセージなどに使います．）メッセージを画面に表示し、任意のボタンが入力されるまで待ちます．改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ単語_末尾改行なし_ボタン送り(string _文字列_改行はエンマークn)
        {
            メッセージを表示(_文字列_改行はエンマークn, true);
            wIn任意ボタン入力待ち();
        }
        /// <summary>
        /// （※主に戦闘メッセージなどに使います．）メッセージを画面に表示します．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ単語_末尾改行なし_自動送り(string _一行文字列_途中改行はエンマークn, int _自動メッセージ送りミリ秒)
        {
            メッセージを表示(_一行文字列_途中改行はエンマークn, true);
            waitウェイト(_自動メッセージ送りミリ秒);
        }
        public void mメッセージ単語_末尾改行なし_自動送り(string _一行文字列_途中改行はエンマークn, ESPeed _表示速度)
        {
            mメッセージ単語_末尾改行なし_自動送り(_一行文字列_途中改行はエンマークn,
                getMSec(_表示速度));
        }
        /// <summary>
        /// （※主に戦闘メッセージなどに使います．）一行メッセージを画面に一挙に（瞬時に）表示します．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ単語_末尾改行なし_瞬時に表示(string _一行文字列_途中改行はエンマークn, int _自動メッセージ送りミリ秒)
        {
            メッセージを表示(_一行文字列_途中改行はエンマークn, true);
            waitウェイト(_自動メッセージ送りミリ秒);
        }
        /// <summary>
        /// （※主に戦闘メッセージなどに使います．）一行メッセージを画面に一挙に（瞬時に）表示します．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ単語_末尾改行なし_瞬時に表示(string _一行文字列_途中改行はエンマークn, ESPeed _表示速度)
        {
            mメッセージ単語_末尾改行なし_瞬時に表示(_一行文字列_途中改行はエンマークn,
                getMSec(_表示速度));
        }
        /// <summary>
        /// （※ミリ秒を厳密に指定したい時だけ使ってください。）メッセージを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ_自動送り(string _一行文字列_途中改行はエンマークn, int _自動メッセージ送りミリ秒)
        {
            メッセージを表示(_一行文字列_途中改行はエンマークn + "\n", true);
            waitウェイト(_自動メッセージ送りミリ秒);
        }
        /// <summary>
        /// （※主に通常のメッセージ自動送りに使います。）メッセージを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ_自動送り(string _一行文字列_途中改行はエンマークn, ESPeed _表示速度) // [Q]なんか引数のdouble型もESpeedとして認識されるらしい。バグ？あぶない！
        {
            mメッセージ単語_末尾改行なし_自動送り(_一行文字列_途中改行はエンマークn + "\n", _表示速度);
        }
        /// <summary>
        /// （※主にオートモード時のメッセージ自動送りに使います）メッセージを画面に表示し、標準ゲームスピードで６００ミリ秒待ちます．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ_自動送り(string _一行文字列_途中改行はエンマークn)
        {
            mメッセージ_自動送り(_一行文字列_途中改行はエンマークn, ESPeed.s07_早い＿標準で６００ミリ秒);
        }
        /// <summary>
        /// （※主にスキップモード時のメッセージスキップに使います）メッセージを画面に瞬時に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        public void mメッセージ_瞬時に表示(string _一行文字列_途中改行はエンマークn)
        {
            mメッセージ_自動送り(_一行文字列_途中改行はエンマークn, ESPeed.s00_デフォルト_待ち時間なし);
        }
        #endregion

        #region ■■■s質問系（m選択肢付メッセージや、m入力メッセージを簡潔に使いやすくしたもの。）
        // この実装は初心者に優しくない。
        // game.QC質問選択肢(game.mメッセージ単語_末尾改行なし_ボタン送り(...))とも、game.mメッセージ単語_末尾改行なし_ボタン送り()とも書けてしまうので、デフォルトの書き方がこんがらがるし、よろしくない。
        ///// <summary>
        ///// ユーザに質問をします．
        ///// </summary>
        ///// <param name="_選択結果を返すメソッド＿mメッセージなどを呼び出す"></param>
        //public void QC質問選択肢(CAnswer・回答 _選択結果を返すメソッド＿mメッセージなどを呼び出す)
        //{
        //    p_selectedResult・選択結果 = _選択結果を返すメソッド＿mメッセージなどを呼び出す;
        //}
        /// <summary>
        /// ユーザに質問をします．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// 結果を判定するには、
        /// (a)返り値ですぐ判定するする方法と、
        /// (b)QA質問結果()メソッドやQANo質問回答番号()メソッドやQAStr質問回答番号()メソッドを呼び出す方法があります。
        /// 
        /// 　　※わざわざ入力結果クラスを宣言したく場合や、少し処理が離れている場は、
        /// 　　　(b)を使うと、便利です。
        /// </summary>
        public CAnswer・回答 QC質問選択肢(string _一行メッセージ文字列, int _デフォルト回答番号_1toN, EChoiceSample・選択肢例 _選択肢例)
        {
            List<int> _デフォルト回答番号たち = new List<int>();
            _デフォルト回答番号たち.Add(_デフォルト回答番号_1toN);
            return QC質問選択肢(_一行メッセージ文字列, _デフォルト回答番号たち, _選択肢例);
        }
        /// <summary>
        /// ユーザに質問をします．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// 結果を判定するには、
        /// (a)返り値ですぐ判定するする方法と、
        /// (b)QA質問結果()メソッドやQANo質問回答番号()メソッドやQAStr質問回答番号()メソッドを呼び出す方法があります。
        /// 
        /// 　　※わざわざ入力結果クラスを宣言したく場合や、少し処理が離れている場は、
        /// 　　　(b)を使うと、便利です。
        /// </summary>
        public CAnswer・回答 QC質問選択肢(string _一行メッセージ文字列, int _デフォルト回答番号_1toN, List<string> _選択肢リスト)
        {
            List<int> _デフォルト回答番号たち = new List<int>();
            _デフォルト回答番号たち.Add(_デフォルト回答番号_1toN);
            return QC質問選択肢(_一行メッセージ文字列, _デフォルト回答番号たち, _選択肢リスト);
        }
        /// <summary>
        /// ユーザに質問をします．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// 結果を判定するには、
        /// (a)返り値ですぐ判定するする方法と、
        /// (b)QA質問結果()メソッドやQANo質問回答番号()メソッドやQAStr質問回答番号()メソッドを呼び出す方法があります。
        /// 
        /// 　　※わざわざ入力結果クラスを宣言したく場合や、少し処理が離れている場は、
        /// 　　　(b)を使うと、便利です。
        /// </summary>
        public CAnswer・回答 QC質問選択肢(string _一行メッセージ文字列, int _デフォルト回答番号_1toN, params string[] _選択肢を列挙)
        {
            List<int> _デフォルト回答番号たち = new List<int>();
            _デフォルト回答番号たち.Add(_デフォルト回答番号_1toN);
            List<string> _選択肢リスト = new List<string>(_選択肢を列挙);
            return QC質問選択肢(_一行メッセージ文字列, _デフォルト回答番号たち, _選択肢リスト);
        }
        /// <summary>
        /// ユーザに質問をします．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// 結果を判定するには、
        /// (a)返り値ですぐ判定するする方法と、
        /// (b)QA質問結果()メソッドやQANo質問回答番号()メソッドやQAStr質問回答番号()メソッドを呼び出す方法があります。
        /// 
        /// 　　※わざわざ入力結果クラスを宣言したく場合や、少し処理が離れている場は、
        /// 　　　(b)を使うと、便利です。
        /// </summary>
        public CAnswer・回答 QC質問選択肢(string _一行メッセージ文字列, List<int> _デフォルト回答番号たち_複数選択可能な時の回答1toN, EChoiceSample・選択肢例 _選択肢例)
        {
            return m選択肢付メッセージ(_一行メッセージ文字列, _選択肢例, _デフォルト回答番号たち_複数選択可能な時の回答1toN, 0);
        }
        /// <summary>
        /// ユーザに質問をします．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// 結果を判定するには、
        /// (a)返り値ですぐ判定するする方法と、
        /// (b)QA質問結果()メソッドやQANo質問回答番号()メソッドやQAStr質問回答番号()メソッドを呼び出す方法があります。
        /// 
        /// 　　※わざわざ入力結果クラスを宣言したく場合や、少し処理が離れている場は、
        /// 　　　(b)を使うと、便利です。
        /// </summary>
        public CAnswer・回答 QC質問選択肢(string _一行メッセージ文字列, List<int> _デフォルト回答番号たち_複数選択可能な時の回答1toN, List<string> _選択肢例)
        {
            return m選択肢付メッセージ(_一行メッセージ文字列, _選択肢例, _デフォルト回答番号たち_複数選択可能な時の回答1toN, 0);
        }
        /// <summary>
        /// ユーザに入力を受け付ける質問をします．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，改行以外で一定時間待ちたい場合は"【w】"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// 結果を判定するには、
        /// (a)返り値ですぐ判定するする方法と、
        /// (b)QA質問結果()メソッドやQANo質問回答番号()メソッドやQAStr質問回答番号()メソッドを呼び出す方法があります。
        /// 
        /// 　　※わざわざ入力結果クラスを宣言したく場合や、少し処理が離れている場は、
        /// 　　　(b)を使うと、便利です。
        /// </summary>
        public CAnswer・回答 QI質問入力(string _複数の入力を受け付けるメッセージ用の一行文字列, double _制限時間＿秒_永久は0, params string[] _入力欄毎のラベル名とデフォルト値_それぞれ2個ずつ列挙)
        {
            return m複数入力メッセージ(_複数の入力を受け付けるメッセージ用の一行文字列, _制限時間＿秒_永久は0, _入力欄毎のラベル名とデフォルト値_それぞれ2個ずつ列挙);
        }
        /// <summary>
        /// ユーザに入力を受け付ける質問をします．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// 結果を判定するには、
        /// (a)返り値ですぐ判定するする方法と、
        /// (b)QA質問結果()メソッドやQANo質問回答番号()メソッドやQAStr質問回答番号()メソッドを呼び出す方法があります。
        /// 
        /// 　　※わざわざ入力結果クラスを宣言したく場合や、少し処理が離れている場は、
        /// 　　　(b)を使うと、便利です。
        /// </summary>
        public CAnswer・回答 QI質問入力(string _入力メッセージを受け付ける一行文字列, string _入力フォームの左に付くラベル名_入力文字列の意味を端的な単語で説明_空白でもＯＫ, string _デフォルトの入力文字列, double _制限時間＿秒_永久は0)
        {
            return QI質問入力(_入力メッセージを受け付ける一行文字列, _制限時間＿秒_永久は0, _入力フォームの左に付くラベル名_入力文字列の意味を端的な単語で説明_空白でもＯＫ, _デフォルトの入力文字列);
        }

        // ■■■sitsu質問結果の取得
        /// <summary>
        /// 前回の質問結果を返します．　
        /// ※返り値は，
        /// 「.is回答()」「.isキャンセル()」「.isはい()」「.isいいえ()」で，典型例とマッチしているか確認したり，
        /// 「.k回答番号()」で，番号で条件分したり，
        /// 「.ks回答文字列()」で，文字列化したりして使用してください．）
        /// </summary>
        /// <returns></returns>
        public CAnswer・回答 QA質問結果()
        {
            return getP_selectedResult・選択結果();
        }
        /// <summary>
        /// 前回の質問で回答した文字列（選択肢）を返します．
        /// </summary>
        /// <returns></returns>
        public String QAStr質問回答文字列()
        {
            return getP_selectedResult・選択結果().ks回答文字列();
        }
        /// <summary>
        /// 前回の質問で回答した番号（何番目の選択肢か）を返します．
        /// </summary>
        /// <returns></returns>
        public int QANo質問回答番号()
        {
            return getP_selectedResult・選択結果().k回答番号();
        }
        /// <summary>
        /// 回答が「Yes」に近い意味の回答例のどれかであった場合にtrueを返します．詳しくはMyTools.isYes()を参照してください。
        /// </summary>
        public bool QAIsYes質問結果＿はい()
        {
            return getP_selectedResult・選択結果().isはい();
        }
        /// <summary>
        /// 回答が「Yes」の意味を持つ回答【以外】であった場合にtrueを返します．
        /// ※つまり、isYes()の裏返しです。
        /// なお、"cancel"などもtrueになるので、キャンセル処理を設ける場合は、この処理の条件分岐よりキャンセル処理を先に書いてください。
        /// 　　　詳しくはMyTools.isYes()の候補語（値がfalseになる）を参照してください。
        /// </summary>
        public bool QAIsNo質問結果＿いいえ()
        {
            return getP_selectedResult・選択結果().isいいえ();
        }
        /// <summary>
        /// 回答をキャンセルした動作（戻るボタンを押した、あるいはそれと同等の意味を持つ選択肢を回答した）場合にtrueを返します．
        /// ※具体的には、回答番号が-1か、回答文字列が"Cancel","CANCEL","キャンセル"のどれかならtrueを返します。
        /// 
        /// 　　なお、キャンセル処理を設ける場合は、いいえ処理の条件分岐よりこのキャンセル処理を先に書いてください。
        /// </summary>
        public bool QAIsCannel質問結果＿キャンセル()
        {
            return getP_selectedResult・選択結果().isキャンセル();
        }


        // ＝＝＝＝＝＝＝選択肢付き・入力メッセージ（現在はprivateにして、publicバージョンはsitsu質問メソッドに統合）
        // ■■■選択肢付きメッセージ
        /// <summary>
        /// 選択肢付きのメッセージを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        /// <param name="_複数の入力を受け付けるメッセージ用の一行文字列"></param>
        /// <param name="_選択肢リスト"></param>
        /// <returns></returns>
        private CAnswer・回答 m選択肢付メッセージ(string _一行文字列, List<string> _選択肢, List<int> _デフォルト回答番号たち1toN_複数選択する時, double _制限時間秒_永久は0)
        {
            mメッセージ_自動送り(_一行文字列, ESPeed.s00_デフォルト_待ち時間なし);
            CAnswer・回答 _selectedChoice = getP_gameWindow・ゲーム画面().getP_messageBox().selectSelection・選択肢を出し選択するまで待機(_選択肢, _デフォルト回答番号たち1toN_複数選択する時, (int)(_制限時間秒_永久は0 * 1000));
            p_selectedResult・選択結果 = _selectedChoice;
            return _selectedChoice;
        }
        /// <summary>
        /// 選択肢付きのメッセージを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        /// <param name="_複数の入力を受け付けるメッセージ用の一行文字列"></param>
        /// <param name="_選択肢リスト"></param>
        /// <returns></returns>
        private CAnswer・回答 m選択肢付メッセージ(string _一行文字列, EChoiceSample・選択肢例 _選択肢, List<int> _デフォルト回答番号たち1toN_複数選択する時, double _制限時間秒_永久は0)
        {
            mメッセージ_自動送り(_一行文字列, ESPeed.s00_デフォルト_待ち時間なし);
            CAnswer・回答 _selectedChoice = getP_gameWindow・ゲーム画面().getP_messageBox().selectSelection・選択肢を出し選択するまで待機(_選択肢, _デフォルト回答番号たち1toN_複数選択する時, (int)(_制限時間秒_永久は0 * 1000));
            p_selectedResult・選択結果 = _selectedChoice;
            return _selectedChoice;
        }
        /// <summary>
        /// 選択肢付きのメッセージを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        /// <param name="_複数の入力を受け付けるメッセージ用の一行文字列"></param>
        /// <param name="_選択肢リスト"></param>
        /// <returns></returns>
        private CAnswer・回答 m選択肢付メッセージ(string _一行文字列, EChoiceSample・選択肢例 _選択肢, int _デフォルト回答番号1toN, double _制限時間＿秒_永久は0)
        {
            List<int> _デフォルト回答番号たち = new List<int>();
            _デフォルト回答番号たち.Add(_デフォルト回答番号1toN);
            return m選択肢付メッセージ(_一行文字列, _選択肢, _デフォルト回答番号たち, _制限時間＿秒_永久は0);
        }
        /// <summary>
        /// 選択肢付きのメッセージを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        /// <param name="_複数の入力を受け付けるメッセージ用の一行文字列"></param>
        /// <param name="_選択肢リスト"></param>
        /// <returns></returns>
        private CAnswer・回答 m選択肢付メッセージ(string _一行文字列, List<string> _選択肢, int _デフォルト回答番号1toN, double _制限時間＿秒_永久は0)
        {
            List<int> _デフォルト回答番号たち = new List<int>();
            _デフォルト回答番号たち.Add(_デフォルト回答番号1toN);
            return m選択肢付メッセージ(_一行文字列, _選択肢, _デフォルト回答番号たち, _制限時間＿秒_永久は0);
        }

        // ■■■入力メッセージ
        /// <summary>
        /// 複数の入力フォーム付きのメッセージを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        /// <param name="_複数の入力を受け付けるメッセージ用の一行文字列"></param>
        /// <param name="_選択肢リスト"></param>
        /// <returns></returns>
        private CAnswer・回答 m複数入力メッセージ(string _一行文字列, double _制限時間＿秒_永久は0, params string[] _入力欄毎のラベル名とデフォルト値_それぞれ2個ずつ列挙)
        {
            mメッセージ_瞬時に表示(_一行文字列);
            CAnswer・回答 _inputedResult = getP_gameWindow・ゲーム画面().getP_messageBox().showInput・複数欄入力画面表示((int)(_制限時間＿秒_永久は0 * 1000), _入力欄毎のラベル名とデフォルト値_それぞれ2個ずつ列挙);
            p_selectedResult・選択結果 = _inputedResult;
            return _inputedResult;
        }
        /// <summary>
        /// 入力フォーム付きのメッセージを画面に表示します．※最後に改行が自動的に入ります．途中で改行を入れるには"\n"，今までのメッセージを消して新しく表示したい場合は"【c】"を入力してください．
        /// </summary>
        /// <param name="_複数の入力を受け付けるメッセージ用の一行文字列"></param>
        /// <param name="_選択肢リスト"></param>
        /// <returns></returns>
        private CAnswer・回答 m入力メッセージ(string _一行文字列, string _入力フォームの左に付くラベル名_入力文字列の意味を端的な単語で説明_空白でもＯＫ, string _デフォルトの入力文字列, double _制限時間＿秒_永久は0)
        {
            return m複数入力メッセージ(_一行文字列, _制限時間＿秒_永久は0, _入力フォームの左に付くラベル名_入力文字列の意味を端的な単語で説明_空白でもＯＫ, _デフォルトの入力文字列);
        }
        /// <summary>
        // [Memo]こういう方法もある
        //public void m選択肢付メッセージ(string _文字列・表示文字列, EChoiceSample・選択肢例 _選択肢リスト, out CAnswer・回答 _selectedItem)
        //{
        //    m選択肢付メッセージ(_文字列・表示文字列);
        //    画面.selectSelection・選択肢を出し選択するまで待機(_選択肢リスト);
        //    _selectedItem = 処理.waitForSelect・選択確定まで待機();
        //}
        // ＝＝＝＝＝＝＝＝＝＝＝＝＝＝選択・入力メッセージ、終わり

        #endregion


        // 3.シナリオ処理
        #region ■■■sシナリオ操作系

        public bool p_isScinarioRun・シナリオ実行中 = false;
        public bool p_isScinarioStop・シナリオ実行が一時中断中 = false;
        public bool p_isScinarioShowPerLine・シナリオをtrue一行ずつ一挙表示するか＿false一文字ずつ表示するか = false;
        string p_scinarioAll・現在進行中のシナリオ台本 = "";
        string p_scinarioRest・残りの台本 = "";
        string p_scinarioNextM・改行までの文章 = "";
        int p_scinarioLineNum・シナリオ実行済み行数 = 0;
        int p_NextLineCharIndex_次の改行位置 = 0;
        int p_LINECHARContinuedNum・改行連続数 = 0;
        public bool p_isScinarioEnd・シナリオがＥＮＤ = false;
        /// <summary>
        /// 引数のシナリオ台本（改行\\n付きの一括テキスト）を進行開始します。
        /// </summary>
        /// <param name="_scinarioText・シナリオ台本"></param>
        public void sシナリオ進行開始(string _scinarioText・シナリオ台本＿改行コード付きの一括テキスト)
        {
            // シナリオの二重起動を防止するための処理（[TODO]これだと不十分かもしれない）
            if (p_isScinarioRun・シナリオ実行中 == true)
            {
                // すでにシナリオが始まっている場合、決定ボタンを自動で押して、古いシナリオを進めます。
                // （新しくシナリオを実行しません）
                p_isScinarioStop・シナリオ実行が一時中断中 = true;
                iA決定ボタンを一瞬だけ自動で押す();
                p_isScinarioStop・シナリオ実行が一時中断中 = false;
            }
            else
            {
                showScinarioMode・ゲーム画面をシナリオモードに移行();

                p_scinarioAll・現在進行中のシナリオ台本 = _scinarioText・シナリオ台本＿改行コード付きの一括テキスト;
                // 古い残りの台本が残っていても、新しい台本に上書き
                p_scinarioRest・残りの台本 = _scinarioText・シナリオ台本＿改行コード付きの一括テキスト;

                // シナリオが動いていない場合、新しくシナリオを実行します。
                p_isScinarioRun・シナリオ実行中 = true;

                sシナリオ進行中の処理(); // ■※このメソッドの中に無限ループあり

                // シナリオが終わったら、実行中フラグをなくす
                p_isScinarioRun・シナリオ実行中 = false;
            }
        }
        /// <summary>
        /// シナリオを一行ずつ進行する時の処理です。
        /// 割り込みが入った場合を考えて、表示する個所を覚える処理などもここに書いてください。
        /// 
        /// ※このメソッドの中に無限ループあり
        /// </summary>
        private void sシナリオ進行中の処理()
        {
            // ■■シナリオ実行無限ループ。シナリオが途中中断かＥＮＤになるまで繰り返し実行
            while (p_isScinarioStop・シナリオ実行が一時中断中 == false)
            {
                // シナリオ行数を１足す
                p_scinarioLineNum・シナリオ実行済み行数++;

                // 残りの台本から、次の改行位置を探す。
                string _LINECHAR = System.Environment.NewLine; // テキストファイルの改行はWindowsでも"\r\n"だよ。"\n"じゃないので気を付けて！
                p_NextLineCharIndex_次の改行位置 = p_scinarioRest・残りの台本.IndexOf(_LINECHAR);
                if (p_NextLineCharIndex_次の改行位置 > 0)
                {
                    // 内容が改行だけではなかった場合、まず、改行までの文章を先に読み込んでおく
                    // 最初～次の改行位置までの文字列を取りだす
                    p_scinarioNextM・改行までの文章 = p_scinarioRest・残りの台本.Substring(0, 
                        p_NextLineCharIndex_次の改行位置 + _LINECHAR.Length);
                    // 改行を取り除く
                    p_scinarioNextM・改行までの文章 = p_scinarioNextM・改行までの文章.Replace(_LINECHAR, "");
                    string _一行シナリオ = p_scinarioNextM・改行までの文章;

                    // 1.一行の文字列を全文チェック

                    if (_一行シナリオ == null)
                    {
                        // nullであることはありえない。だって、Replaceができないもの。でも念のため終了。
                        p_isScinarioEnd・シナリオがＥＮＤ = true;
                    }
                    else if (_一行シナリオ == "")
                    {
                        // 何も中身がなければ、ただの改行なので自動送りとする
                        // （たぶんここだと_index==0なので、ここはは通らないはず。だけど念のためかいておく）
                        // 次の一行が改行だけの場合、改行とする
                        mメッセージ_自動送り("", ESPeed.s08_非常に速い＿標準で３００ミリ秒);
                        p_scinarioRest・残りの台本 = p_scinarioRest・残りの台本.Substring(_LINECHAR.Length);
                        // 連続した改行の数をカウントし、２つ以上なら改ページ
                        p_LINECHARContinuedNum・改行連続数++;
                        if (p_LINECHARContinuedNum・改行連続数 > 2)
                        {
                            // 改ページ
                            mメッセージボックスを初期化();
                            // 改行連続数を初期化
                            p_LINECHARContinuedNum・改行連続数 = 0;
                        }
                        DEBUGデバッグ一行出力(ELogType.l5_エラーダイアログ表示, "■■一行シナリオが空白だったよ。なんかプログラム変じゃない？");
                    }
                    else if (_一行シナリオ == "ＥＮＤ" && _一行シナリオ == "RStick6_END")
                    {
                        // 物語が終わってしまったら、終了。
                        p_isScinarioEnd・シナリオがＥＮＤ = true;
                        // [TODO]エンディング処理
                        DEBUGデバッグ一行出力(ELogType.l5_エラーダイアログ表示, "■■エンディングです。お疲れさまでした！");
                    }
                    else
                    {
                        // 2.一行の文字列の最初の一文字目をチェック

                        string _一文字目 = _一行シナリオ.Substring(0, 1);
                        // ”オプション、つまり「（」などで始まる文章だったら
                        if (_一文字目 == "＞" || _一文字目 == ">" || _一文字目 == "・" || _一文字目 == "/" || _一文字目 == "／"
                            || _一文字目 == "*" || _一文字目 == "＊" || _一文字目 == "！" || _一文字目 == "!" || _一文字目 == "％" || _一文字目 == "%" || _一文字目 == "￥" || _一文字目 == "\\"
                            )
                        {

                            // ※コメント例：　「＞・／＊￥％！」はコメント
                            //         　　　　その他コメントにする無効頭文字　（●選択後の分岐ラベル、【配役】、・Ａ：○○役、）など。


                        }
                        else if (_一文字目 == "？" || _一文字目 == "?")
                        {
                            // 選択肢例：　「？」は選択肢にする？、でも普通の
                            //    ？？？「貴様、私が誰だか知らないのか？」
                            // という言葉があるので却下。

                        }
                        else if(_一文字目 == "（" || _一文字目 == "(")
                        {
                            // 選択肢例：　「？」は選択肢にする？、（選択肢）、（選択　…）、（効果音）
                            //      （ここは読まなくていい）
                            // て感じで、冒頭からカッコが入ってる奴は、いまのところ飛ばしている。
                            // [TODO]選択肢・効果音処理。ひとまず、スルー
                        }
                        else if(_一文字目 == "●" || _一文字目 == "★" || _一文字目 == "■" || _一文字目 == "◆"
                            || _一文字目 == "○" || _一文字目 == "☆" || _一文字目 == "□" || _一文字目 == "◇" || _一文字目 == "◎")
                        {
                            // これも、いまのところ飛ばしている。                            
                        }
                        else if(_一文字目 == "【")
                        {
                            // これも、いまのところ飛ばしている。
                        }
                        else if (_一文字目 == "→" || _一文字目 == "⇒" || _一文字目 == "＠")
                        {
                            // [TODO]ジャンプ処理にする？。ひとまず、スルー

                        }
                        else if (_一文字目 == "＝" || _一文字目 == "=")
                        {
                            // 改ページ処理。
                            //      =========
                            // や
                            //      ＝＝＝＝＝＝＝＝＝
                            // で区切られた個所と認識し、ボタン送り後にメッセージボックスを初期化。
                            mメッセージ_瞬時に表示(_一行シナリオ);
                            // 決定ボタンで次のページへ
                            wIn任意ボタン入力待ち();
                            mメッセージボックスを初期化();
                        }
                        else
                        {
                            // 最初の一文字のチェックで、どれでもない場合

                            // ■■■文章を一行ずつ表示
                            // 一行ずつ一挙表示か、一文字ずつか
                            if (p_isScinarioShowPerLine・シナリオをtrue一行ずつ一挙表示するか＿false一文字ずつ表示するか == true)
                            {
                                mメッセージ_瞬時に表示(_一行シナリオ);
                                // 決定ボタンで次の一行へ
                                wIn任意ボタン入力待ち();
                            }
                            else
                            {
                                mメッセージ単語_末尾改行なし_ボタン送り(_一行シナリオ + "\n");
                            }


                        }
                    }
                    // 一行の処理が終了
                    //DEBUGデバッグ一行出力("■シナリオ " + p_scinarioLineNum・シナリオ実行済み行数 + " 行目（"+_一行シナリオ.Substring(0, Math.Min(_一行シナリオ.Length, 10))+"…）実行終了");

                    // 残りの台本を更新
                    p_scinarioRest・残りの台本 = p_scinarioRest・残りの台本.Substring(p_NextLineCharIndex_次の改行位置 + _LINECHAR.Length);
                    // 改行連続数を初期化
                    p_LINECHARContinuedNum・改行連続数 = 0;
                }
                else if (p_NextLineCharIndex_次の改行位置 == 0)
                {
                    //DEBUGデバッグ一行出力("■シナリオ " + p_scinarioLineNum・シナリオ実行済み行数 + " 行目は改行。改行処理実行中");
                    // 次の一行が改行だけの場合、改行とする
                    mメッセージ_自動送り("", ESPeed.s08_非常に速い＿標準で３００ミリ秒);
                    p_scinarioRest・残りの台本 = p_scinarioRest・残りの台本.Substring(_LINECHAR.Length);
                    // 連続した改行の数をカウントし、２つ以上なら改ページ
                    p_LINECHARContinuedNum・改行連続数++;
                    if (p_LINECHARContinuedNum・改行連続数 > 2)
                    {
                        // 改ページ
                        mメッセージボックスを初期化();
                        // 改行連続数を初期化
                        p_LINECHARContinuedNum・改行連続数 = 0;
                    }
                }
                else// if (_次の改行位置 <= -1)
                {
                    DEBUGデバッグ一行出力("■シナリオ " + p_scinarioLineNum・シナリオ実行済み行数 + " 行目でこれ以上改行は見つからず、終了。");
                    // もう改行が見つからなかった場合、終わり
                    p_isScinarioEnd・シナリオがＥＮＤ = true;
                    break;
                }
            }
        }
        #endregion
        #region ■■■show***: ゲーム画面関連
        /// <summary>
        /// ゲーム画面をダイスバトルモードに移行します（バトルはいきなり始まりません）。
        /// なお、sダイス戦闘()メソッドでも、このメソッドが最初に呼ばれます。
        /// </summary>
        public void showDiceBattleMode・ゲーム画面をダイスバトルモードに移行()
        {
            getP_gameWindow・ゲーム画面().getP_usedFrom()._setDiceBattleMode・ダイスバトルモードに画面変更();
            // タスクやシーンの管理
            p_modeState・ゲームのモードや状態遷移の管理者.setP_Mode・ゲームモードを設定(EMode・ゲームモード.m3_Battle・戦闘モード);
            //p_sceneController・ゲームシーン管理者...
        }
        /// <summary>
        /// ゲーム画面をシナリオモードに移行します（シナリオは始まりません）。
        /// なお、sシナリオ進行開始()メソッドでも、このメソッドが最初に呼ばれます。
        /// </summary>
        public void showScinarioMode・ゲーム画面をシナリオモードに移行()
        {
            // ゲーム画面をシナリオモードに変更
            getP_gameWindow・ゲーム画面().getP_usedFrom()._setStroyMode・ストーリーモードに画面変更();
            // タスクやシーンの管理
            p_modeState・ゲームのモードや状態遷移の管理者.setP_Mode・ゲームモードを設定(EMode・ゲームモード.m2_Scinario・シナリオモード);
            //p_sceneController・ゲームシーン管理者...
        }

        /// <summary>
        /// 画面にキャラのダイスバトルのステータス（名前やＬＶやあいさつセリフと、戦闘に関する色パラメータとダイスコマンド）を表示します。
        /// </summary>
        public void showCharaDiceBattleStatus・キャラのダイスバトルステータスを表示(bool _isShownLeft・味方側に表示するか＿falseなら敵側表示, CChara・キャラ _cキャラ, int _charaPartyID・パーティで何番目のキャラか, bool _isShowIroParaAndDice・ダイスパラメータも表示するか, bool _isClearOtherPatryCharaNameAndHP・他のパーティキャラの名前やＨＰラベルを初期化するか_falseだと残す)
        {
            // キャラのダイス戦闘画面の描画処理
            if (_cキャラ.getP_dice・所有ダイス() == null || _cキャラ.getP_dice・所有ダイス().Count == 0)
            {
                // 目で見てわかりやすいように、仮のダイスコマンドを作成
                CCharaCreator・キャラ生成機.createDiceCommand_FromParas・ダイスコマンドを自動生成(_cキャラ);
            }
            // 画面にステータスを表示
            game.getP_gameWindow・ゲーム画面().getP_usedFrom()._drawCharaPara・キャラの名前やＨＰやパラメータを表示(
                    _isShownLeft・味方側に表示するか＿falseなら敵側表示, _cキャラ, _charaPartyID・パーティで何番目のキャラか,
                    _isShowIroParaAndDice・ダイスパラメータも表示するか, _isClearOtherPatryCharaNameAndHP・他のパーティキャラの名前やＨＰラベルを初期化するか_falseだと残す);

            // メッセージボックスにも表示
            string _tuyosa = "★" + _cキャラ.name名前() + "の基本的な強さ: " +
                _cキャラ.Para(EPara._基本6色総合値) + "、応用的な強さ: " +
                _cキャラ.Para(EPara._中間6色総合値) + "";
            game.mメッセージ_瞬時に表示(_tuyosa);
        }
        public void showBattleForm・バランス調整画面を表示()
        {
            FTestBalanceForm _form = game.getP_FBalance・バランス調整フォーム();
            if (_form == null)
            {
                _form = new FTestBalanceForm(game);
            }
            _form.Show();
        }

        public void showBattleDamageCalcForm・ダメージ計算機画面を表示()
        {
            FDamageCalculator _form = game.getP_FDamage・ダメージ調整フォーム();//game.getP_FBalance・バランス調整フォーム();
            if (_form == null)
            {
                _form = new FDamageCalculator(game);
            }
            _form.Show();
        }
        #endregion
        // 乱数取得
        #region ■■■getRandom乱数系
        /// <summary>
        /// ランダム生成クラス
        /// </summary>
        private CMyRandomGenerator・ランダム生成者 p_random・乱数 = new CMyRandomGenerator・ランダム生成者();
        public CMyRandomGenerator・ランダム生成者 getP_random・乱数()
        {
            return p_random・乱数;
        }
        /// <summary>
        /// 最小値～最大値を含むランダムな整数を生成します．　例えば，最小値:1，最大値:5だと，1,2,3,4,5の内のどれかになります．
        /// </summary>
        /// <param name="_最小値"></param>
        /// <param name="_最大値"></param>
        /// <returns></returns>
        public int getRandom・ランダム値を生成(int _最小値, int _最大値)
        {
            int _randomNum = p_random・乱数.getRandomNum・ランダム値を生成して保存(_最小値, _最大値);
            return _randomNum;
        }
        #endregion
        

        // 4.戦闘などのイベント処理
        #region ■■■s戦闘系
        ///// <summary>
        ///// 戦闘クラス
        ///// </summary>
        //private CBattleBase・戦闘を管理する基底クラス p_battle・戦闘;
        //public CBattleBase・戦闘を管理する基底クラス getP_battle・戦闘()
        //{
        //    return p_battle・戦闘;
        //}
        //public void s戦闘(CChara・キャラ _味方キャラ, CChara・キャラ _敵キャラ)
        //{
        //    p_battle・戦闘.startBattle・戦闘開始(this, _味方キャラ, _敵キャラ);
        //}
        //public void s戦闘(List<CChara・キャラ> _味方キャラ, CChara・キャラ _敵キャラ)
        //{
        //    p_battle・戦闘.startBattle・戦闘開始(this, _味方キャラ, _敵キャラ);
        //}
        //public void s戦闘(CChara・キャラ _味方キャラ, List<CChara・キャラ> _敵キャラ)
        //{
        //    p_battle・戦闘.startBattle・戦闘開始(this, _味方キャラ, _敵キャラ);
        //}
        //public void s戦闘(List<CChara・キャラ> _味方キャラ, List<CChara・キャラ> _敵キャラ)
        //{
        //    p_battle・戦闘.startBattle・戦闘開始(this, _味方キャラ, _敵キャラ);
        //}
        //public bool s戦闘に勝利()
        //{
        //    return p_battle・戦闘.is勝利();
        //}
        //public bool s戦闘に敗北()
        //{
        //    return p_battle・戦闘.is敗北();
        //}
        /// <summary>
        /// ダイス戦闘クラス
        /// </summary>
        private CBattle・戦闘 p_diceBattle・ダイス戦闘;
        public CBattle・戦闘 getP_Battle・戦闘()
        {
            return p_diceBattle・ダイス戦闘;
        }
        public void sダイス戦闘(CChara・キャラ _味方キャラ, CChara・キャラ _敵キャラ)
        {
            p_diceBattle・ダイス戦闘.startBattle・戦闘開始(this, _味方キャラ, _敵キャラ);
        }
        public void sダイス戦闘(List<CChara・キャラ> _味方キャラ, CChara・キャラ _敵キャラ)
        {
            p_diceBattle・ダイス戦闘.startBattle・戦闘開始(this, _味方キャラ, _敵キャラ);
        }
        public void sダイス戦闘(CChara・キャラ _味方キャラ, List<CChara・キャラ> _敵キャラ)
        {
            p_diceBattle・ダイス戦闘.startBattle・戦闘開始(this, _味方キャラ, _敵キャラ);
        }
        public void sダイス戦闘(List<CChara・キャラ> _味方キャラ, List<CChara・キャラ> _敵キャラ)
        {
            p_diceBattle・ダイス戦闘.startBattle・戦闘開始(this, _味方キャラ, _敵キャラ);
        }
        public bool s戦闘に勝利()
        {
            return p_diceBattle・ダイス戦闘.is勝利();
        }
        public bool s戦闘に敗北()
        {
            return p_diceBattle・ダイス戦闘.is敗北();
        }
        #endregion
        #region ●●●setNextFase/showBattleCommandなど、戦闘その他系
        internal EBattleFase・戦闘フェーズ setNextFase_FromBattleActions・アクションから次のフェーズを設定()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal CVarValue・変数値 showBattleCommand2・戦闘コマンド画面２作戦を表示して入力待機()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal CVarValue・変数値 showBattleCommand2・戦闘コマンド画面２作戦を表示して入力待機(CChara・キャラ _キャラ)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal int showBattleCommand2_2・戦闘コマンド画面２攻撃対象を表示して入力待機(CChara・キャラ _キャラ, List<CChara・キャラ> p_charaAll・全キャラ)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
        // イベント操作系
        #region ■■■lレベル操作系
        /// <summary>
        /// 指定キャラのレベルアップ処理をします．LVUP数でレベルアップ回数を指定します．（-1などでレベルダウン処理も可能です）．
        /// レベルアップにより増加したキャラのパラ上昇値（基本６色パラメータ総合値の増加量）を返します。
        /// 
        /// 　なお、レベルが上げられない場合は0を返します．
        /// </summary>
        /// <returns></returns>
        public int LVUP・レベルアップ(CChara・キャラ _キャラ, double _LVUP数)
        {
            return CCharaCreator・キャラ生成機.LVUP・レベル設定(_キャラ, _LVUP数);
        }
        /// <summary>
        /// 指定キャラ全てのレベルアップ処理をします．LVUP数でレベルアップ回数を指定します．（-1などでレベルダウン処理も可能です）．
        /// 誰か一人のレベルが上げられなかった（レベルが上限でカンストした場合など）場合はfalseを返します．
        /// </summary>
        /// <returns></returns>
        public bool LVUP・レベルアップ(List<CChara・キャラ> _キャラたち, double _LVUP数)
        {
            bool _isLVUP = true;
            foreach (CChara・キャラ _キャラ in _キャラたち)
            {
                if (CCharaCreator・キャラ生成機.LVUP・レベル設定(_キャラ, _LVUP数) == 0)
                {
                    _isLVUP = false;
                }
            }
            return _isLVUP;
        }
        #endregion


        // 5.オプションなど設定処理
        #region ■■■pポーズ系
        /// <summary>
        /// 指定キーが押されるまで、ゲーム画面を一時停止します。操作方法やヘルプなどもあわせて表示すると親切かもしれません。
        /// </summary>
        /// <param name="_trueポーズ＿falseポーズ解除"></param>
        /// <returns></returns>
        public bool pポーズ(bool _trueポーズ＿falseポーズ解除)
        {
            bool _is呼び出し前はポーズだったか = false;
            if (s_isStopDraw・描画更新をストップ == false)
            {
                s_isStopDraw・描画更新をストップ = true;

                // 指定キーが押されるまで画面停止
                int _WaitMSec_1Loop = 1000; // ポーズ中はなるべく負担をかけないように、あと再開時に急に始まるよりはゆっくり始まる方がコマ送りしやすいかも。
                // 画面を暗くするだけじゃなくて、操作方法やヘルプなどを表示する方が親切かも。
                // また、スクリーンショットを取るために、ボタン一つで画面を元に戻す処理があった方がいい。
                MyTools.ConsoleWriteLine("■■■pポーズ中■■■　（「SPACE」キー か スタートボタンで解除）。画面に操作方法・ヘルプも表示");
                while (ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b9_スペースボタン_START)
                    || ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.SPACE))
                {
                    w画面停止(_WaitMSec_1Loop);
                }
            }
            else
            {
                _is呼び出し前はポーズだったか = true;
                s_isStopDraw・描画更新をストップ = false;
            }
            return _is呼び出し前はポーズだったか;
        }
        #endregion
        

        // 6.ファイル処理
        #region ■■■loadロード系

        #endregion
        #region ■■■saveセーブ系
        public void savePlayerCharaData・キャラをプレイヤーデータベースに追加(CChara・キャラ _c)
        {
            CCharaCreator・キャラ生成機.savePlayerCharaData・キャラをプレイヤーデータベースに追加(_c, "");
        }
        #endregion

        // その他、ユーザの操作系
        #region ■■■U***, user***: ユーザに何かをさせる処理系（キャラ選択、リスト選択など）
        /// <summary>
        /// ●ユーザにキャラを選択させ、全キャラから選択したキャラを取得します。LVがある場合は、LV1でスタートするか、そのままのLVでスタートするか、決めます。
        /// </summary>
        /// <param name="_LV_キャラのレベル"></param>
        /// <returns></returns>
        public CChara・キャラ userSelectCharactor・選択キャラを取得()
        {
            return userSelectChara・選択キャラを指定LVで取得(-101);
        }
        /// <summary>
        /// ●ユーザにキャラを選択させ、全キャラから選択したキャラを、指定LVで取得します。
        /// </summary>
        /// <param name="_LV_キャラのレベル"></param>
        /// <returns></returns>
        public CChara・キャラ userSelectChara・選択キャラを指定LVで取得(int _LV)
        {
            CChara・キャラ _選択キャラ;
            string _charaName = "";
            bool _isキャラが存在した = false;
            while (_isキャラが存在した == false)
            {
            Labelキャラの種類選択:
                game.QC質問選択肢("キャラの種類を選択してください。", 1, EChoiceSample・選択肢例.charaSelect1_プレイヤーキャラ＿ゲストキャラ);
                if (game.QA質問結果().k回答番号() == 1)
                {
                Labelプレイヤーキャラ選択:
                    // プレイヤーキャラ
                    game.QC質問選択肢("【c】プレイヤーキャラは，自分で選びますか？\n" +
                        "それとも、ランダムで選択しますか？", 1, EChoiceSample・選択肢例.random_自分で選択＿ランダムで選択);
                    _charaName = MyTools.getRandomString(game.getAllPlayerCharaName・全プレイヤーキャラ名を取得());
                    // 戻るボタンが押されたらひとつ前に戻る
                    //以下と一緒 if (game.ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b2_戻るボタン_B) == true)
                    if(game.QA質問結果().isキャンセル() == true)
                    {
                        goto Labelキャラの種類選択; // goto文はreturnやcontinueといっしょで、後の処理は実行しない。気にしなくていい
                    }
                    if (game.QA質問結果().k回答番号() == 1)
                    {
                        game.QC質問選択肢("【c】プレイヤーキャラを選択してください。", 1, EChoiceSample・選択肢例.charaAllプレイヤーキャラ一覧);
                        // 戻るボタンが押されたらひとつ前に戻る
                        if (game.ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b2_戻るボタン_B) == true)
                        {
                            goto Labelプレイヤーキャラ選択;
                        }
                        // 称号やパラメータなどを消去
                        _charaName = MyTools.getStringItem(game.QAStr質問回答文字列(), " ", 1);
                    }
                    // 既にランダムのキャラ名は取得済み
                }
                else if (game.QA質問結果().k回答番号() == 2)
                {
                Labelゲストキャラ選択:
                    // ゲストキャラ
                    game.QC質問選択肢("【c】ゲストキャラは，自分で選びますか？\n" +
                        "それとも、ランダムで選択しますか？", 1, EChoiceSample・選択肢例.random_自分で選択＿ランダムで選択);
                    _charaName = MyTools.getRandomString(game.getAllGuestCharaName・全ゲストキャラ名を取得());
                    // 戻るボタンが押されたらひとつ前に戻る
                    if (game.ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b2_戻るボタン_B) == true)
                    {
                        goto Labelキャラの種類選択;
                    }
                    if (game.QA質問結果().k回答番号() == 1)
                    {
                        game.QC質問選択肢("【c】ゲストキャラを選択してください。", 1, EChoiceSample・選択肢例.charaAllゲストキャラ一覧);
                        // 戻るボタンが押されたらひとつ前に戻る
                        if (game.ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b2_戻るボタン_B) == true)
                        {
                            goto Labelゲストキャラ選択;
                        }
                        // 称号やパラメータなどを消去
                        _charaName = MyTools.getStringItem(game.QAStr質問回答文字列(), " ", 1);
                    }
                    // 既にランダムのキャラ名は取得済み
                }
                // キャラの存在を確認
                if (game.isCharaキャラが存在するか(_charaName) == true)
                {
                    _isキャラが存在した = true;
                }
                else
                {
                    game.QC質問選択肢("ごめんなさい。そのキャラ名「" + _charaName + "」はデータベース上に存在しないようです。\nもう一度選び直してください。\nもしくは、ランダムで選択することもできます。", 2, EChoiceSample・選択肢例.random_自分で選択＿ランダムで選択);
                    if (game.QA質問結果().k回答番号() == 2)
                    {
                        // ランダムでキャラを決定
                        _charaName = MyTools.getRandomString(game.getAllCharaNameList・全キャラ名リストを取得＿亡霊キャラを含まない());
                    }
                    // もう一度自分で選び直し
                }
            }
            // ランダムか自分で、_charaNameを取得し終わった
            if (_LV == -101)
            {
                _選択キャラ = CCharaCreator・キャラ生成機.getChara・キャラを取得(_charaName);
                if (_選択キャラ.Para(EPara.LV) != 1)
                {
                    game.QC質問選択肢("【c】キャラのLVは1からにしますか？\nそれとも、強くてニューゲームにしますか？", 2, "LV1からスタート", "強くてニューゲーム");
                    if (game.QA質問結果().k回答番号() == 1)
                    {
                        _選択キャラ = game.getChara・キャラを取得(_charaName, 1);
                    }
                }
            }
            else
            {
                // 選択したキャラ名のキャラ取得
                _選択キャラ = game.getChara・キャラを取得(_charaName, _LV);
            }
            return _選択キャラ;
        }
        /// <summary>
        /// 指定キャラ名のキャラが存在するかを返します。
        /// </summary>
        /// <param name="_charaName"></param>
        /// <returns></returns>
        public bool isCharaキャラが存在するか(string _charaName){
            if (CCharaCreator・キャラ生成機.getChara・キャラを取得(_charaName) == null) return false; else return true;
        }

        /// <summary>
        /// ●ユーザに、新しいキャラを作成させて、そのキャラを返します。_bonusRateが0.0以外の場合、ユーザが手動でキャラを作成する画面に移行します。0.0の場合、画面は出ずに自動生成されたキャラを返します。
        /// ※なお、ユーザのキャラ名がキャンセルされた場合に、nullが返ることがあります（戻るボタンを連打したり、キャラ名がかぶっていた場合に改名をキャンセルした場合など）。
        /// </summary>
        /// <returns></returns>
        public CChara・キャラ userCreateNewChara・新しいキャラを作成(string _charaName, double _LV_キャラのレベル, double _LV1Iro6paraSum・ＬＶ１時の6パラ総合値, double _bonusRate_N_0to1・ボーナスパラ手動振り分け率＿０だと自動振り分け＿０以外だ後で手動振り分けする画面が出る)
        {
            string _newNameキャラの登録名 = _charaName;
            List<string> _allCharaNameList = getAllCharaNameList・全キャラ名リストを取得＿亡霊キャラを含む();
            bool _isUniqueキャラ登録名がかぶっていない = false;
            while (_isUniqueキャラ登録名がかぶっていない == false)
            {
                if (_allCharaNameList.Contains(_newNameキャラの登録名) == true)
                {
                    string _aideaName候補名 = "新" + _newNameキャラの登録名;
                    while (_allCharaNameList.Contains(_aideaName候補名) == true)
                    {
                        _aideaName候補名 = MyTools.getRandomString(
                            _newNameキャラの登録名 + "＠"+p_user・ユーザ.name名前(),
                            "超人" + _newNameキャラの登録名,
                            "S" + _newNameキャラの登録名,
                            _newNameキャラの登録名 + "X",
                            _newNameキャラの登録名 + "Y",
                            _newNameキャラの登録名 + "Z",
                            _newNameキャラの登録名 + MyTools.getRandomNum(0, 9));
                    }
                    game.QI質問入力("【c】キャラ名「" + _newNameキャラの登録名 + "」は既に使われています。別のユニークな名前で登録をお願いします。", 0, "ユニークなキャラ名",
                        _aideaName候補名);
                    _newNameキャラの登録名 = game.QA質問結果().ks回答文字列();
                    //_newNameキャラの登録名 = MyTools.showInputBox("キャラ名「" + _newNameキャラの登録名 + "」は既に使われています。別の名前にしてください。", "既に使われているキャラ名です。", _newNameキャラの登録名 + "改");
                    // キャンセルの場合
                    //if (_newNameキャラの登録名 == "")
                    if(game.QA質問結果().isキャンセル() == true)
                    {
                        game.QC質問選択肢("【c】キャラの改名をキャンセルすると、新しくこのキャラを登録できません。\n本当にキャンセルしますか？", 2, EChoiceSample・選択肢例.e2a＿はい＿いいえ);
                        // キャンセルでもいいえを選んだことにする
                        if (game.QA質問結果().isキャンセル() || game.QA質問結果().isいいえ())
                        {
                            // もう一度改名をやりなおす
                            _newNameキャラの登録名 = _charaName;
                        }
                        else // isはい
                        {
                            // 新しいキャラ登録をキャンセル。やめちゃう。
                            _isUniqueキャラ登録名がかぶっていない = true;
                            return null; // nullを返す
                        }
                    }
                }
                else
                {
                    _isUniqueキャラ登録名がかぶっていない = true;
                }
            }

            CChara・キャラ _新キャラ;
            if (_bonusRate_N_0to1・ボーナスパラ手動振り分け率＿０だと自動振り分け＿０以外だ後で手動振り分けする画面が出る == 0.0)
            {
                // 自動生成します．
                _新キャラ = CCharaCreator・キャラ生成機.getNormalSampleChara・標準サンプルキャラ自動生成(false,
                    _LV_キャラのレベル, false, _LV1Iro6paraSum・ＬＶ１時の6パラ総合値, 1.0);
                _新キャラ.setVar・変数を変更(EVar.名前, _charaName);
            }
            else
            {
                // 手動生成します。
                _新キャラ = CCharaCreator・キャラ生成機.getNormalSampleChara・標準サンプルキャラ自動生成(false,
                    _LV_キャラのレベル, false, _LV1Iro6paraSum・ＬＶ１時の6パラ総合値, 1.0 - _bonusRate_N_0to1・ボーナスパラ手動振り分け率＿０だと自動振り分け＿０以外だ後で手動振り分けする画面が出る);
                _新キャラ.setVar・変数を変更(EVar.名前, _charaName);

                // ボーナスは、身体6+精神6合わせて
                int _ボーナスパラ値 = (int)(CCharaCreator・キャラ生成機.s_LV1Para_Iro6Sum・キャラＬＶ１標準の基本６色パラ総合値 * CCharaCreator・キャラ生成機.s_LV1Para_BonusSumRate・キャラＬＶ１標準作成時のボーナスパラとして振り分ける割合);
                showFPara・パラメータ調整フォームを表示(_新キャラ, _ボーナスパラ値);

                // (a)パラメータ調整フォームを終了するイベントを、p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ で管理（パラメータ調整フォーム上でtrueにする）
                // フラグの初期化
                game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
                game.wIa特定入力完了待ち();
                // (b)パラメータ調整フォームを終了するイベントを、p_otherWindowEnded・他の画面での処理を完了して元の画面に戻ったかで管理（パラメータ調整フォーム上でtrueにする）
                //今は使ってない。GoNextと統合。p_isOtherWindowEnded・他の画面での処理を完了して元の画面に戻った入力フラグ = false;
                //while (p_isOtherWindowEnded・他の画面での処理を完了して元の画面に戻った入力フラグ == false)

                // 終了時のフラグの初期化
                game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);
                // パラメータ調整フォームから新しい作成したキャラデータを取得
                _新キャラ = p_FParameter・パラメータ調整フォーム.getCharaData();
                // HideはFParaForm上のボタンでやる
                //_パラメータ調整フォーム.Hide();


            }
            // 名前を変更
            _新キャラ.setVar・変数を変更(EVar.名前, _charaName);
            _新キャラ.setName・名前を一時的に変更(_charaName);
            // 名前が変わったので，念のため、もう一度LVUP処理をしておく
            //いらない？LVUP・レベルアップ(_新キャラ, _LV_キャラのレベル - 1);
            return _新キャラ;

        }
        /// <summary>
        /// レベルアップ以外で、ユーザにキャラのパラメータ調整させたい（画面を出したい）時に呼び出すメソッドです。
        /// ※LVUP・レベルアップ()も、キャラの基本能力を上げた後、このメソッドを呼び出します。
        /// なお、キャラのＬＶＵＰ時の増加パラメータを調べたい時は、これではなく、LVUP・レベルアップ()を使ってください。
        /// 
        /// 引数_bonusRate_N_0to1・ボーナスパラ手動振り分け率が0だと自動振り分け、
        /// 0以外だとユーザがボーナスパラメータを手動で振り分けする画面が出でます。
        /// 
        /// 返り値は、パラメータが少しでも変更されていればtrue、変更されていなければfalseを返します
        /// （ただし、余った手動振り分けで余ったパラメータはキャラ_chara.Para(EPara.LVBonus_未振り分けボーナスパラ)に保存されます。）。
        /// 
        /// </summary>
        public bool userSetCharaPara・キャラのパラメータを調整(CChara・キャラ _chara・パラメータ調整キャラ, double _LVUPNum_キャラのＬＶＵＰ数＿ＬＶＵＰ時に使う＿再調整の場合は０, double _bonusRate_N_0to1・ボーナスパラ手動振り分け率)
        {

            // 引数の変数名が長いので短縮
            CChara・キャラ _chara = _chara・パラメータ調整キャラ;
            double _LVUPNum = _LVUPNum_キャラのＬＶＵＰ数＿ＬＶＵＰ時に使う＿再調整の場合は０;
            double _bonusRate = _bonusRate_N_0to1・ボーナスパラ手動振り分け率;

            bool _isChange・少しでもパラメータを変更したか = false;
            // 調整前のキャラの６パラメータを一時退避し、変更したかを判定できるようにしておく
            List<double> _beforeIro6Para = _chara.Paras・パラメータ一括処理().getIro6基本６色パラメータ();

            
            // LVUP時の増加パラメータを取得
            int _AddPara_Iro6ParaSum = 0;
            if (_LVUPNum != 0)
            {
                _AddPara_Iro6ParaSum = CCharaCreator・キャラ生成機.getLVUPAddPara_Iro6・キャラＬＶＵＰ時の基本6パラ上昇値を取得(_chara, _LVUPNum);
            }
            // 全部自動振り分けするか
            if (_bonusRate == 0.0)
            {
                // 全部自動振り分け
                CCharaCreator・キャラ生成機.setCharaPara・キャラのパラメータを調整(_chara, 
                    _AddPara_Iro6ParaSum, EParaDividedType・パラ分割方法.RateToIro6・基本６色パラに比例分割);
            }
            else
            {
                // 一部自動振り分け
                int _AutoAddPara = (int)(_AddPara_Iro6ParaSum * _bonusRate);
                CCharaCreator・キャラ生成機.setCharaPara・キャラのパラメータを調整(_chara, 
                    _AutoAddPara, EParaDividedType・パラ分割方法.RateToIro6・基本６色パラに比例分割);
                // 手動生成します。

                // ボーナスは、身体6+精神6合わせて
                int _bonusAddPara・ボーナスパラ値 = _AddPara_Iro6ParaSum - _AutoAddPara; ;
                showFPara・パラメータ調整フォームを表示(_chara, _bonusAddPara・ボーナスパラ値);

                // (a)パラメータ調整フォームを終了するイベントを、p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ で管理（パラメータ調整フォーム上でtrueにする）
                // フラグの初期化
                game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
                game.wIa特定入力完了待ち();                // (b)パラメータ調整フォームを終了するイベントを、p_otherWindowEnded・他の画面での処理を完了して元の画面に戻ったかで管理（パラメータ調整フォーム上でtrueにする）
                //今は使ってない。GoNextと統合。p_isOtherWindowEnded・他の画面での処理を完了して元の画面に戻った入力フラグ = false;
                //while (p_isOtherWindowEnded・他の画面での処理を完了して元の画面に戻った入力フラグ == false)

                // 終了時のフラグの初期化
                game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);
                // パラメータ調整フォームから新しい作成したキャラデータを取得
                _chara = p_FParameter・パラメータ調整フォーム.getCharaData();
                // HideはFParaForm上のボタンでやる
                //_パラメータ調整フォーム.Hide();

            }

            // 調整後のキャラの６パラメータを取得
            List<double> _afterIro6Para = _chara.Paras・パラメータ一括処理().getIro6基本６色パラメータ();
            // before～afterで、全てのパラメータに少しでも変化があれば、true
            if (MyTools.isListEquals(_beforeIro6Para, _afterIro6Para) == false)
            {
                _isChange・少しでもパラメータを変更したか = true;
            }
            return _isChange・少しでもパラメータを変更したか;

        }
        #endregion

        // ＝＝＝＝その他応用系のメソッド（●●●はカテゴリ毎にまとめきれていないメソッド、get系も多い）
        
        // パラメータ名やデフォルト値を取得系
        #region ■■■getParaパラ取得系
        /// <summary>
        /// パラメータのデフォルト値（初期値）を取得します．初期値がないパラメータは0を取得します．
        /// </summary>
        /// <param name="_parameterID"></param>
        public double getParaDefault・パラ初期値を取得(EPara _parameterID)
        {
            return CParasDefault・パラ初期値群.get(_parameterID);
        }
        /// <summary>
        /// パラメータの名前を取得します．※パラメータの名前は適時変更される可能性があるので，出力文字列には直接"HP"などとは書かず，出来るだけこのメソッドを使ってください．
        /// </summary>
        /// <param name="_parameterID"></param>
        /// <returns></returns>
        public string getParaName(EPara _parameterID)
        {
            return MyTools.getEnumName(_parameterID);
        }
        #endregion
        // キャラ取得系
        #region ●●●getCharaキャラ取得系
        /// <summary>
        /// 引数のキャラ名を（データベースに登録されている）そのままのレベルで取得します．見つからなければnullを返します。
        /// </summary>
        public CChara・キャラ getChara・キャラを取得(string _charaName)
        {
            return CCharaCreator・キャラ生成機.getChara・キャラを取得(_charaName);
        }
        /// <summary>
        /// 引数のキャラ名を指定レベルで取得します．見つからなければnullを返します。
        /// </summary>
        public CChara・キャラ getChara・キャラを取得(string _charaName, double _LV)
        {
            return CCharaCreator・キャラ生成機.getChara・キャラを取得(_charaName, _LV);
        }
        /// <summary>
        /// キャラクタデータベースから，特定キャラ名のリスト（項目名はユニークな参照名、つまり名前だけ）を取得します。
        /// </summary>
        public List<string> getAllBoureiCharaName・全亡霊キャラ名を取得()
        {
            return CCharaCreator・キャラ生成機.getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c03_亡霊キャラ);
        }
        /// <summary>
        /// キャラクタデータベースから，特定キャラ名のリスト（項目名はユニークな参照名、つまり名前だけ）を取得します。
        /// </summary>
        public List<string> getAllGuestCharaName・全ゲストキャラ名を取得()
        {
            return CCharaCreator・キャラ生成機.getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c02_ゲストキャラ);
        }
        /// <summary>
        /// キャラクタデータベースから，特定キャラ名のリスト（項目名はユニークな参照名、つまり名前だけ）を取得します。
        /// </summary>
        public List<string> getAllPlayerCharaName・全プレイヤーキャラ名を取得()
        {
            return CCharaCreator・キャラ生成機.getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c01_プレイヤーキャラ);
        }
        public CChara・キャラ createEnemy・ザコキャラを全種キャラから自動生成＿ボス級キャラは除く(double _基本6色パラメータ, double _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, double _最高倍数＿ザコなら０点８割から１点３までくらいが標準か)
        {
            CChara・キャラ _cキャラ = null;
            // ボス級キャラがでなくなるまで繰り返し
            int i = 0;
            while (_cキャラ == null)
            {
                _cキャラ = createEnemy・総合パラメータがほどほどに似ているキャラを全てのキャラから自動生成(_基本6色パラメータ, _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, _最高倍数＿ザコなら０点８割から１点３までくらいが標準か);
                // 基本値が標準値を超えていれば、やりなおし
                if (_cキャラ.Para(EPara.LV1c_基本6色総合値) > getNormalChara・全て平均値の標準キャラを取得(1.0).Para(EPara.LV1c_基本6色総合値))
                {
                    _cキャラ = null;
                    i++;
                    continue;
                }
                if (i > 100) { break; } // 100回もループしてたら、ボスでも我慢してね
            }
            return _cキャラ;
        }
        public CChara・キャラ p_normalChara・標準キャラ;
        public CChara・キャラ getNormalChara・全て平均値の標準キャラを取得(double _LV)
        {
            if (p_normalChara・標準キャラ == null)
            {
                p_normalChara・標準キャラ = CCharaCreator・キャラ生成機.getNormalSampleChara・標準サンプルキャラ自動生成(true, 1);
            }
            getChara・キャラを取得(p_normalChara・標準キャラ.name名前(), _LV);
            return p_normalChara・標準キャラ;
        }
        #endregion
        // よく使うリスト取得系
        #region ●●●get***: よく使うリスト取得系メソッド（キャラ名リスト、キャラのパラメータのリスト取得、リスト操作など）
        // 全キャラリスト名を持ったリスト
        public List<string> p_allCharaNames_IncludingBoreiChara;
        public List<string> p_allCharaNames_NotIncludingBoreiChara;
        public List<string> getAllCharaNameList・全キャラ名リストを取得＿亡霊キャラを含む()
        {
            if (p_allCharaNames_IncludingBoreiChara == null)
            {
                List<string> _guests = getAllGuestCharaName・全ゲストキャラ名を取得();
                List<string> _players = getAllPlayerCharaName・全プレイヤーキャラ名を取得();
                List<string> _allCharaNames = new List<string>();
                _allCharaNames.AddRange(_guests);
                _allCharaNames.AddRange(_players);
                p_allCharaNames_IncludingBoreiChara = _allCharaNames;
            }
            return p_allCharaNames_IncludingBoreiChara;
        }
        public List<string> getAllCharaNameList・全キャラ名リストを取得＿亡霊キャラを含まない()
        {
            if (p_allCharaNames_NotIncludingBoreiChara == null)
            {
                List<string> _guests = getAllGuestCharaName・全ゲストキャラ名を取得();
                List<string> _players = getAllPlayerCharaName・全プレイヤーキャラ名を取得();
                List<string> _boureis = getAllBoureiCharaName・全亡霊キャラ名を取得();
                List<string> _allCharaNames = new List<string>();
                _allCharaNames.AddRange(_guests);
                _allCharaNames.AddRange(_players);
                _allCharaNames.AddRange(_boureis);
                p_allCharaNames_NotIncludingBoreiChara = _allCharaNames;
            }
            return p_allCharaNames_NotIncludingBoreiChara;
        }
        /// <summary>
        /// キャラのID毎に特定のパラメータの数値を格納したキャラパラ情報をリストを取得します。（例えば、「キャラAの攻撃力:200、キャラBの攻撃力:100」という情報を表示したり内部比較したりしたいときに使います）
        /// </summary>
        public List<CCPInfo・キャラパラ情報> getCharaParaList・キャラパラリストを取得(List<CChara・キャラ> _キャラたち, EPara _パラID, ESortType・並び替え順 _並び替え順)
        {
            List<CCPInfo・キャラパラ情報> _キャラパラリスト = new List<CCPInfo・キャラパラ情報>();
            int _キャラパラリストid = 0;
            double _パラ値 = 0.0;
            foreach (CChara・キャラ _chara in _キャラたち)
            {
                _パラ値 = _chara.Para(_パラID);
                _キャラパラリスト.Add(new CCPInfo・キャラパラ情報(_パラ値, _キャラパラリストid));
                _キャラパラリストid++;
            }
            // リストをソート
            switch (_並び替え順)
            {
                case ESortType・並び替え順.無:
                    break;
                case ESortType・並び替え順.昇順:
                    //_キャラパラリスト.Sort(delegate(CCPInfo・キャラパラ情報 x, CCPInfo・キャラパラ情報 y) { return (x.Para < y.Para); }); //小さい順にソート
                    _キャラパラリスト.Sort(昇順比較); // 小さい順にソート
                    break;
                case ESortType・並び替え順.降順:
                    //_キャラパラリスト.Sort(delegate(CCPInfo・キャラパラ情報 x, CCPInfo・キャラパラ情報 y) { return (x.Para < y.Para); }); //小さい順にソート
                    //_キャラパラリスト.Reverse(); // 反転して，降順に
                    _キャラパラリスト.Sort(降順比較); // 大きい順にソート
                    break;
                case ESortType・並び替え順.あいうえお順:
                    _キャラパラリスト.Sort(); // 文字コード順なので昇順と一緒
                    break;
                case ESortType・並び替え順.文字数が小さい順:
                    //_キャラパラリスト.Sort(delegate(string x, string y) { return (x.Length < y.Length); });  // .NetFramework3.0だとこうも書ける (x, y) => x.Length - y.Length);
                    break;
                case ESortType・並び替え順.文字数が大きい順:
                    //_キャラパラリスト.Sort(delegate(string x, string y) { return (x.Length > y.Length); });
                    break;
                default:
                    break;
            }
            return _キャラパラリスト;
        }
        // 小さい順にソート
        private int 昇順比較(CCPInfo・キャラパラ情報 x, CCPInfo・キャラパラ情報 y) { return (int)(x.para - y.para); }
        // 大きい順にソート
        private int 降順比較(CCPInfo・キャラパラ情報 x, CCPInfo・キャラパラ情報 y) { return (int)(y.para - x.para); }

        internal List<int> getParaSortedCharaIDList・パラメータ順のキャラIDリストを取得(List<CChara・キャラ> _識別するキャラID順に格納されたキャラたち, EPara _パラID, ESortType・並び替え順 _並び替え順)
        {
            List<int> _パラ順のキャラidリスト = new List<int>();

            List<CCPInfo・キャラパラ情報> _キャラパラリスト = getCharaParaList・キャラパラリストを取得(_識別するキャラID順に格納されたキャラたち, _パラID, _並び替え順);
            for (int i = 0; i <= _キャラパラリスト.Count - 1; i++)
            {
                _パラ順のキャラidリスト.Add(_キャラパラリスト[i].id); // キャラパラリストのidをパラ順に格納
            }
            return _パラ順のキャラidリスト;
        }
        #endregion

        // キャラ新規作成系
        #region ●●●create新規作成系
        public void createNewUser・新規ユーザ作成(string _あなたの名前_ニックネーム, string _ユーザID, string _パスワード)
        {
            p_user・ユーザ = new ユーザ(_あなたの名前_ニックネーム, _ユーザID, _パスワード);
        }
        #region その他、敵キャラの自動作成系
        public CChara・キャラ createEnemy・総合パラメータがほどほどに似ているキャラを全てのキャラから自動生成(double _基本6色パラメータ, double _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, double _最高倍数＿０から１テン２とか３とか)
        {
            CChara・キャラ _cキャラ;
            int _キャラ数 = getAllCharaNameList・全キャラ名リストを取得＿亡霊キャラを含む().Count;
            int _randomNum = getRandom・ランダム値を生成(0, _キャラ数 - 1);
            string _キャラ名 = getAllCharaNameList・全キャラ名リストを取得＿亡霊キャラを含む()[_キャラ数];
            _cキャラ = CCharaCreator・キャラ生成機.getChara・キャラを取得(_キャラ名);
            // 基本6パラが近くなるまでLVUP/Down
            _cキャラ = CCharaCreator・キャラ生成機.getCharaFromCharaName_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変えて取得(
                _キャラ名, _基本6色パラメータ, _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, _最高倍数＿０から１テン２とか３とか);

            return _cキャラ;
        }
        public List<CChara・キャラ> createEnemys・総合パラメータがほどほどに似ているプレイヤーキャラたちを自動生成(int _EnemyNum敵キャラ数, double _基本6色パラメータ, double _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, double _最高倍数＿０から１テン２とか３とか)
        {
            int _敵キャラ数 = _EnemyNum敵キャラ数; //getRandom・ランダム値を生成(1, Math.Max(1, _EnemyNum敵キャラ数));
            List<CChara・キャラ> _敵キャラたち = new List<CChara・キャラ>(_敵キャラ数);

            CChara・キャラ _敵キャラ;
            for (int i = 0; i < _敵キャラ数; i++)
            {
                _敵キャラ = createPlayer・プレイヤーキャラを自動生成(_基本6色パラメータ, _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, _最高倍数＿０から１テン２とか３とか);
                _敵キャラたち.Add(_敵キャラ);
            }
            return _敵キャラたち;
        }
        public CChara・キャラ createPlayer・プレイヤーキャラを自動生成(double _基本6色パラメータ, double _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, double _最高倍数＿０から２とか３とか)
        {
            return CCharaCreator・キャラ生成機.getChara・パラ総合値がほどほどに似たキャラを取得(ECharaType・キャラの種類.c01_プレイヤーキャラ, _基本6色パラメータ, _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, _最高倍数＿０から２とか３とか);
        }
        public List<CChara・キャラ> createEnemys・総合パラメータがほどほどに似ているゲストキャラたちを自動生成(int _EnemyNum敵キャラ数, double _基本6色パラメータ)
        {
            return createEnemys・総合パラメータがほどほどに似ているゲストキャラたちを自動生成(_EnemyNum敵キャラ数, _基本6色パラメータ, 0.8, 1.2);
        }
        public List<CChara・キャラ> createEnemys・総合パラメータがほどほどに似ているゲストキャラたちを自動生成(int _EnemyNum敵キャラ数, double _基本6色パラメータ, double _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, double _最高倍数＿０から１テン２とか３とか)
        {
            int _敵キャラ数 = _EnemyNum敵キャラ数; //getRandom・ランダム値を生成(1, Math.Max(1, _EnemyNum敵キャラ数));
            List<CChara・キャラ> _敵キャラたち = new List<CChara・キャラ>(_敵キャラ数);

            CChara・キャラ _敵キャラ;
            for (int i = 0; i < _敵キャラ数; i++)
            {
                _敵キャラ = createEnemy・ゲストキャラを自動生成(_基本6色パラメータ, _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, _最高倍数＿０から１テン２とか３とか);
                _敵キャラたち.Add(_敵キャラ);
            }
            return _敵キャラたち;
        }
        public List<CChara・キャラ> createEnemys・ゲストキャラたちを自動生成(int _EnemyNum敵キャラ数, double _LV_Average)
        {
            List<CChara・キャラ> _敵キャラたち = new List<CChara・キャラ>(_EnemyNum敵キャラ数);
            List<string> _ゲストキャラ名リスト = CCharaCreator・キャラ生成機.getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c02_ゲストキャラ);
            string _ゲストキャラ名 = MyTools.getRandomString(_ゲストキャラ名リスト);
            for (int i = 0; i < _EnemyNum敵キャラ数; i++)
            {
                _ゲストキャラ名 = MyTools.getRandomString(_ゲストキャラ名リスト);
                _敵キャラたち.Add(getChara・キャラを取得(_ゲストキャラ名, _LV_Average));
            }
            return _敵キャラたち;
        }
        public CChara・キャラ createEnemy・ゲストキャラを自動生成(double _基本6色パラメータ, double _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, double _最高倍数＿０から２とか３とか)
        {
            return CCharaCreator・キャラ生成機.getChara・パラ総合値がほどほどに似たキャラを取得(ECharaType・キャラの種類.c02_ゲストキャラ, _基本6色パラメータ, _最低倍数＿基本パラの何倍以上の敵キャラが出てくるか＿０から１まで, _最高倍数＿０から２とか３とか);
        }
        public List<CChara・キャラ> createEnemys・亡霊キャラたちを自動生成(int _EnemyNum敵キャラ数, double _LV_Average)
        {
            int _敵キャラ数 = _EnemyNum敵キャラ数; //getRandom・ランダム値を生成(1, Math.Max(1,_EnemyNum敵キャラ数));
            List<CChara・キャラ> _敵キャラたち = new List<CChara・キャラ>(_敵キャラ数);
            for (int i = 0; i < _敵キャラ数; i++)
            {
                _敵キャラたち.Add(createEnemy・亡霊キャラを自動生成(_LV_Average));
            }
            return _敵キャラたち;
        }
        public CChara・キャラ createEnemy・亡霊キャラを自動生成(double _LV_Average)
        {
            return CCharaCreator・キャラ生成機.getNormalSampleChara・標準サンプルキャラ自動生成(true, _LV_Average, true, CCharaCreator・キャラ生成機.s_LV1Para_Iro6Sum・キャラＬＶ１標準の基本６色パラ総合値, 1.0);
        }

        #endregion


        #endregion

        internal void createCharaFace・キャラの顔を創る()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        internal void ユーザが新しい選択肢を追加()
        {
            throw new Exception("The method or operation is not implemented.");
        }


        // ＝＝＝＝＝＝＝＝ ●●●描画エンジン依存グラフィクス系（Imageクラスなど、描画エンジンに依存する記述が含まれるもの）

        // ＝＝＝＝＝＝＝＝ ●●●描画エンジン依存グラフィクス系、終わり



        // ＝＝＝＝その他応用系メソッド、終わり。

        public bool setBattleAuto・自動戦闘モードにする()
        {
            bool _is既にオートモードになっている = p_isAutoPlay・自動モード;
            game.p_isAutoPlay・自動モード = true;
            game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か = true;
            CGameManager・ゲーム管理者.s_決定ボタン自動押し = true;
            return _is既にオートモードになっている;
        }
        public bool setBattleSlot・スロット戦闘モードにする(bool _isSlotRolate・スロットを回転させるか, bool _isRolateRandom・スロットはtrueランダム＿false降順)
        {
            bool _is既にそのモードになっている = p_isAutoPlay・自動モード == false && CGameManager・ゲーム管理者.s_決定ボタン自動押し == false && game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か == true &&  game.getP_Battle・戦闘().getp_battleMode・現在の戦闘モードを取得() == EBattleMode・戦闘方式._default・ダイスバトルモード;
            game.p_isAutoPlay・自動モード = false;
            CGameManager・ゲーム管理者.s_決定ボタン自動押し = false;
            game.getP_Battle・戦闘().setp_battleMode・戦闘モードを変更(EBattleMode・戦闘方式._default・ダイスバトルモード);
            game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か = _isSlotRolate・スロットを回転させるか;
            game.getP_Battle・戦闘().p_isRolateRandom・スロットはtrueランダム＿false降順 = _isRolateRandom・スロットはtrueランダム＿false降順;
            return _is既にそのモードになっている;
        }
        public bool setBattleCommandSelect・コマンド選択戦闘モードにする()
        {
            bool _is既にそのモードになっている = p_isAutoPlay・自動モード == false && CGameManager・ゲーム管理者.s_決定ボタン自動押し == false && game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か == false && game.getP_Battle・戦闘().getp_battleMode・現在の戦闘モードを取得() == EBattleMode・戦闘方式.e01_command1・通常ＲＧＭ風コマンドバトルモード;
            game.p_isAutoPlay・自動モード = false;
            CGameManager・ゲーム管理者.s_決定ボタン自動押し = false;
            game.getP_Battle・戦闘().setp_battleMode・戦闘モードを変更(EBattleMode・戦闘方式.e01_command1・通常ＲＧＭ風コマンドバトルモード);
            game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か = false;
            //攻撃中とかに変えられるとまずいので、ここですぐに変えなくてもいい。game.getP_Battle・戦闘().showCommand・表示コマンドを変更(EShowCommandType・表示コマンド.c01_First・戦闘開始用コマンド＿たたかうやにげる等);
            return _is既にそのモードになっている;
        }
        public bool setBattleRealTime・間合い取り戦闘モードにする()
        {
            bool _is既にそのモードになっている = p_isAutoPlay・自動モード == false && CGameManager・ゲーム管理者.s_決定ボタン自動押し == false && game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か == false && game.getP_Battle・戦闘().getp_battleMode・現在の戦闘モードを取得() == EBattleMode・戦闘方式.e02_realtime1・モンスターファーム風バトルモード;

            game.p_isAutoPlay・自動モード = false;
            game.getP_Battle・戦闘().setp_battleMode・戦闘モードを変更(EBattleMode・戦闘方式.e02_realtime1・モンスターファーム風バトルモード);
            CGameManager・ゲーム管理者.s_決定ボタン自動押し = false;
            game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か = false;
            return _is既にそのモードになっている;
        }

        // ＝＝＝＝ ■■ゲームの制作用メソッド、終わり。





        // ＝＝＝＝※Windowsフォーム依存メソッド（後で一括して変更できるように、非依存メソッドとはできるだけ分けて）

        private FDamageCalculator p_FDamege・ダメージ調整フォーム = null;
        public FDamageCalculator getP_FDamage・ダメージ調整フォーム() { return p_FDamege・ダメージ調整フォーム; }
        public void setP_FDamage・ダメージ調整フォーム(FDamageCalculator _form) { p_FDamege・ダメージ調整フォーム = _form; }

        private FTestBalanceForm p_FBalance・バランス調整フォーム = null;
        public FTestBalanceForm getP_FBalance・バランス調整フォーム() { return p_FBalance・バランス調整フォーム; }
        public void setP_FBalance・バランス調整フォーム(FTestBalanceForm _form) { p_FBalance・バランス調整フォーム = _form; }

        #region ■■■パラメータ調整画面系（FParameter）
        private FParameter p_FParameter・パラメータ調整フォーム;
        public FParameter getP_FParameter・パラメータ調整フォーム() { return p_FParameter・パラメータ調整フォーム; }
        public void setP_FParameter・パラメータ調整フォーム(FParameter _form) { p_FParameter・パラメータ調整フォーム = _form; }
        /// <summary>
        /// パラメータ調整フォームを表示します。_キャラはパラメータを表示したいキャラです。_ボーナスパラ値は振り分け可能な値です。必要なければ0を記入してください。
        /// </summary>
        /// <param name="_新キャラ"></param>
        /// <param name="_bonusAddPara・ボーナスパラ値"></param>
        /// <returns></returns>
        private void showFPara・パラメータ調整フォームを表示(CChara・キャラ _キャラ, int _ボーナスパラ値)
        {
            if (p_FParameter・パラメータ調整フォーム == null)
            {
                p_FParameter・パラメータ調整フォーム = new FParameter(this, _キャラ.Paras・パラメータ一括処理().getIro12色パラメータ(), _ボーナスパラ値, _キャラ);
            }
            else
            {
                p_FParameter・パラメータ調整フォーム.setParameter(_キャラ.Paras・パラメータ一括処理().getIro12色パラメータ(), _ボーナスパラ値, _キャラ);
            }
            p_FParameter・パラメータ調整フォーム.Show();
            // 最前面にする
            //(a)普通はこれでいけるはずだが、だめp_FParameter・パラメータ調整フォーム.Focus();
            //(b)これでもだめp_FParameter・パラメータ調整フォーム.BringToFront();
            //(c)これで無理やりやるしかないのか
            MyTools.showFormAndSetFocus(p_FParameter・パラメータ調整フォーム, true);
            
            mメッセージ_自動送り("キャラの各種項目を選択し、ボーナスパラメータを振り分けてください。\n（※既に振り分けられたパラメータの調整も可能です）");
        }
        private void hideFPara・パラメータ調整フォームを非表示(CChara・キャラ _キャラ, int _ボーナスパラ値)
        {
            p_FParameter・パラメータ調整フォーム.Hide();
        }
        #endregion

        // ＝＝＝＝Windowsフォーム依存メソッド、終わり。






        // 以下、自動的に作成されたスタブや、適時更新していってる最新メソッド

        #region ■■■eエフェクト系

        ////[MEMO]※enumで管理するよりも，引数付きのメソッドの方が詳細にパラメータ調整できていいと思う。以下、enumで管理する草案
        ///// <summary>
        ///// すぐ呼び出して画面に表示できる，エフェクトの一覧です，
        ///// </summary>
        //public enum EEfect・エフェクト
        //{
        //    a01_通常攻撃時,
        //    a02_通常攻撃クリティカル時,
        //    d01_ダメージ時,
        //    e01_戦闘不能時,
        //}
        public void ea味方通常攻撃エフェクト(int _攻撃キャラid, int _攻撃対象キャラid)
        {
            se効果音(ESE・効果音.attack01a・味方攻撃_ピリリッ);
            waitウェイト(1);
        }
        public void ea味方通常攻撃クリティカルエフェクト(int _攻撃キャラid, int _攻撃対象キャラid)
        {
            se効果音(ESE・効果音.attack02a・味方クリティカル_ジャキィーン);
            waitウェイト(500);
        }
        public void ea敵通常攻撃エフェクト(int _攻撃キャラid, int _攻撃対象キャラid)
        {
            se効果音(ESE・効果音.attack01b・敵攻撃_ブルルッ);
            waitウェイト(1);
        }
        public void ea敵通常攻撃クリティカルエフェクト(int _攻撃キャラid, int _攻撃対象キャラid)
        {
            se効果音(ESE・効果音.attack02a・味方クリティカル_ジャキィーン);
            waitウェイト(500);
        }
        public void ea攻撃会心エフェクト(int _攻撃キャラid, int _攻撃対象キャラid)
        {
            se効果音(ESE・効果音.attack03a・会心の一撃_シュンシュンシュンッ);
            waitウェイト(700);
        }
        public void ea攻撃痛恨エフェクト(int _攻撃キャラid, int _攻撃対象キャラid)
        {
            se効果音(ESE・効果音.attack03b・痛恨の一撃_ジュンジュンジュンッ);
            waitウェイト(700);
        }
        public void ea攻撃悲劇エフェクト(int _攻撃キャラid, int _攻撃対象キャラid)
        {
            se効果音(ESE・効果音.attack04b・悲劇の一撃_シュゥーーンドバドバドバァーーーン);
            waitウェイト(900);
        }
        public void ea攻撃奇跡エフェクト(int _攻撃キャラid, int _攻撃対象キャラid)
        {
            se効果音(ESE・効果音.attack04a・奇跡の一撃_キラリンッードバッシャーーーン);
            waitウェイト(900);
        }


        public void edダメージエフェクト(int _キャラid, double _ダメージ数)
        {
            se効果音(ESE・効果音.damade01・ダメージ_ブワッ);
            waitウェイト(1);
        }
        internal void ec回復エフェクト(int _キャラid, double _回復ポイント数)
        {
            se効果音(ESE・効果音.heal01・回復1_ホワン);
            waitウェイト(500);
        }
        public void ed戦闘不能エフェクト(int _キャラid)
        {
            se効果音(ESE・効果音.dameyo01・戦闘不能_バタンッ);
            waitウェイト(1000);
        }
        public void ea回避エフェクト(int _キャラid)
        {
            se効果音(ESE・効果音.avoid01・回避1_シャッ);
            waitウェイト(300);
        }
        public void eb防御エフェクト(int _キャラid)
        {
            se効果音(ESE・効果音.guardPre01・防御準備1_チャッ);
            waitウェイト(200);
        }
        public void ea回避開始エフェクト(int _キャラid)
        {
            waitウェイト(100);
        }
        public void eb防御開始エフェクト(int _キャラid)
        {
            waitウェイト(300);
        }

        #endregion


    }
    

}
