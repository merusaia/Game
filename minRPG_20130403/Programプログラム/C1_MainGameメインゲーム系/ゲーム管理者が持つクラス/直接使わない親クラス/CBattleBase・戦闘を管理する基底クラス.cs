using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /*/// <summary>
    /// 戦闘を管理するクラスです．
    /// </summary>
    public class 戦闘 : CBattleBase・戦闘を管理する基底クラス
    {

    }*/

    /// <summary>
    /// 戦闘結果を格納する列挙体です．
    /// </summary>
    public enum 戦闘結果
    {
        未定,

        勝利,
        敗北,
        引き分け,
        逃走,
        和解,
        崩壊,
    }
    /// <summary>
    /// 現時点での戦闘のフェーズの種類です．フェーズの種類によって，ボタン入力や割り込み可能な処理などが異なります．
    /// </summary>
    public enum EBattleFase・戦闘フェーズ{
        f00_開始中,

        f01_会話中,
        f02_作戦中,
        f03_ターン進行中,
        f04_行動中,
        f05_ターン停止中,
        f06_ターン終了中,

        f07_イベント中,
        
        f10_戦闘終了中,

        f21_ポーズ中,
    }

    /// <summary>
    /// 戦闘を管理する汎用クラスです．ダイスバトル専用のクラスは、子クラスのCBattleDaisu・ダイス戦闘をみてください。
    /// </summary>
    public class CBattleBase・戦闘を管理する基底クラス
    {
        // 戦闘参加キャラ
        public List<CChara・キャラ> p_charaPlayer・味方キャラ = new List<CChara・キャラ>();
        public List<CChara・キャラ> p_charaEnemy・敵キャラ = new List<CChara・キャラ>();
        public List<CChara・キャラ> p_charaOther・その他キャラ = new List<CChara・キャラ>();
        public List<CChara・キャラ> p_charaAll・全キャラ = new List<CChara・キャラ>(); // 戦闘に参加している全キャラのインデックスを管理しています．
        public int p_charaPlayer_Index・味方キャラ_主人公ID = 0;
        public int p_charaEnemy_Index・敵キャラ_リーダーID = 0;

        protected CGameManager・ゲーム管理者 game;

        #region 戦闘バランスを決めるプロパティ
        public static double s_行動ゲージ回復量＿素早さ１００につき = 1.0;
        public static double s_SP回復量＿精神力１００につき = 2.0;
        public static double s_クリティカル攻撃力上昇倍率1_0 = 1.0;
        public static double s_クリティカル時に加算する魔法力倍率1_0 = 1.0;

        public static double s_会心率標準10_0 = 10.0;
        public static double s_会心攻撃力上昇倍率1_5 = 1.5;
        public static double s_奇跡率標準1_0 = 1.0;
        public static double s_奇跡攻撃力上昇倍率2_0 = 2.0;

        public static double s_回避大失敗率標準 = 10.0;
        #endregion

        #region 戦闘中の状態を管理するプロパティ
        protected EBattleFase・戦闘フェーズ p_nowFase・フェーズ;
        protected double p_time・経過時間;
        protected int p_turn・ターン;
        //CInputButton・ボタン入力定義 p_input・入力 = EInputButton・入力ボタン.無し;
        #region get/setアクセサ
        public EBattleFase・戦闘フェーズ getP_Fase・フェーズを取得()
        {
            return p_nowFase・フェーズ;
        }
        public void setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ _フェーズ)
        {
            p_nowFase・フェーズ = _フェーズ;
        }

        /// <summary>
        /// キャラ毎のプロパティは，基本はキャラの特徴に格納するが，攻撃対象ぐらいは全キャラまとめて扱われることも多いだろうから，一応整理しておいた方がいいだろう，という意味で作られたリスト変数（もしかしたらあまり使わないかもしれない）
        /// </summary>
        /*protected List<int> p_target・このターンの全キャラの攻撃対象 = new List<int>();
        public void setP_target・このターンのキャラの攻撃対象を設定(int _戦闘キャラID, int _攻撃対象キャラID)
        {
            if (_戦闘キャラID > p_target・このターンの全キャラの攻撃対象.Count - 1)
            {
                p_target・このターンの全キャラの攻撃対象[_戦闘キャラID] = _攻撃対象キャラID;
            }
        }*/
        /*public void setP_target・このターンの全キャラの攻撃対象を設定(int _キャラID, int _攻撃対象キャラID)
        {
            if (_キャラID > p_target・このターンの全キャラの攻撃対象.Count - 1)
            {
                p_target・このターンの全キャラの攻撃対象[_キャラID] = _攻撃対象キャラID;
            }
        }*/
        #endregion
        #endregion
        #region 戦闘結果を管理するプロパティ・メソッド
        private 戦闘結果 p_battleResult = 戦闘結果.未定;
        public void set戦闘結果(戦闘結果 _結果)
        {
            p_battleResult = _結果;
        }
        public 戦闘結果 get戦闘結果()
        {
            return p_battleResult;
        }
        public bool is勝利()
        {
            if (p_battleResult == 戦闘結果.勝利)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool is敗北()
        {
            if (p_battleResult == 戦闘結果.敗北)
            {
                return true;
            }else{
                return false;
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_charaPlayer"></param>
        /// <param name="_charaEnemy"></param>
        /// <param name="_charaOther"></param>
        public CBattleBase・戦闘を管理する基底クラス(CGameManager・ゲーム管理者 _g)
        {
            game = _g;
        }

        /// <summary>
        /// 新しい戦闘を開始するときの初期化処理です．
        /// </summary>
        protected void initBattleParameter()
        {
            p_charaPlayer・味方キャラ.Clear();
            p_charaEnemy・敵キャラ.Clear();
            p_charaOther・その他キャラ.Clear();
            p_charaAll・全キャラ.Clear();

            // 戦闘結果の管理パラメータ
            p_battleResult = 戦闘結果.未定;

            // 戦闘中の管理パラメータ
            p_nowFase・フェーズ = EBattleFase・戦闘フェーズ.f00_開始中;
            p_time・経過時間 = 0.0;
            p_turn・ターン = 1;
        }

        /// <summary>
        /// 戦闘を開始します．参加するキャラを選択します（戦闘中も変更可能です．）// [TODO]CGameManager・ゲーム管理者 _globalDataはどこに引数を入れればよい？
        /// </summary>
        virtual public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _g, List<CChara・キャラ> _charaPlayer, List<CChara・キャラ> _charaEnemy, List<CChara・キャラ> _charaOther)
        {
            // 戦闘パラメータの初期化
            initBattleParameter();

            game = _g;
            p_charaPlayer・味方キャラ = _charaPlayer;
            p_charaEnemy・敵キャラ = _charaEnemy;
            p_charaOther・その他キャラ = _charaOther;
            p_charaAll・全キャラ.AddRange(_charaPlayer);
            p_charaAll・全キャラ.AddRange(_charaEnemy);
            p_charaAll・全キャラ.AddRange(_charaOther);

            viewBattleWindow・戦闘画面表示();
            initializeChara・キャラ戦闘開始状態初期化();

            // 戦闘を開始します．
            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "■■■■■■■■■戦闘開始！■■■■■■■■■■");
            
            // 音楽を再生
            //
            game.mメッセージ_ボタン送り("");
            foreach (CChara・キャラ _enemy in p_charaEnemy・敵キャラ)
            {
                game.mメッセージ_自動送り("【c】"+_enemy.name名前() + " があらわれた！");
            }
            game.waitウェイト(1000);

            // メイン戦闘コマンド処理を実行
            doBattleCommand();

        }
        #region 引数が異なるメソッド
        /// 戦闘を開始します（仲介キャラ無し）．参加するキャラを選択します（戦闘中も変更可能です．）
        /// </summary>
        virtual public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, List<CChara・キャラ> _charaPlayer, List<CChara・キャラ> _charaEnemy)
        {
            List<CChara・キャラ> _noChara = new List<CChara・キャラ>();
            startBattle・戦闘開始(_gameData, _charaPlayer, _charaEnemy, _noChara);
        }

        /// <summary>
        /// 戦闘を開始します（味方キャラ一人）．参加するキャラを選択します（戦闘中も変更可能です．）
        /// </summary>
        virtual public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, CChara・キャラ _charaPlayer, List<CChara・キャラ> _charaEnemy)
        {
            List<CChara・キャラ> _onePayer = new List<CChara・キャラ>();
            _onePayer.Add(_charaPlayer);
            startBattle・戦闘開始(_gameData, _onePayer, _charaEnemy);
        }
        /// <summary>
        /// 戦闘を開始します（敵キャラ一人）．参加するキャラを選択します（戦闘中も変更可能です．）
        /// </summary>
        virtual public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, List<CChara・キャラ> _charaPlayer, CChara・キャラ _charaEnemy)
        {
            List<CChara・キャラ> _oneEmeny = new List<CChara・キャラ>();
            _oneEmeny.Add(_charaEnemy);
            startBattle・戦闘開始(_gameData, _charaPlayer, _oneEmeny);
        }
        /// <summary>
        /// 戦闘を開始します（味方キャラも敵キャラも一人）．参加するキャラを選択します（戦闘中も変更可能です．）
        /// </summary>
        virtual public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, CChara・キャラ _charaPlayer, CChara・キャラ _charaEnemy)
        {
            List<CChara・キャラ> _onePlayer = new List<CChara・キャラ>();
            _onePlayer.Add(_charaPlayer);
            List<CChara・キャラ> _oneEnemy = new List<CChara・キャラ>();
            _oneEnemy.Add(_charaEnemy);
            startBattle・戦闘開始(_gameData, _onePlayer, _oneEnemy);
        }
        #endregion

        /// <summary>
        /// 戦闘ターン処理です．
        /// </summary>
        private void doBattleCommand()
        {
            // 戦闘ターン処理
            while (p_nowFase・フェーズ != EBattleFase・戦闘フェーズ.f10_戦闘終了中)
            {
                Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "-------------------1:会話中---------------------------");
                setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f01_会話中);
                command1・戦闘会話();
                Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "-------------------2:作戦入力中-----------------------");
                setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f02_作戦中);
                command2・戦闘作戦入力();
                Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "/_/_/_/_/_/_/_/_/_/3:第 " + p_turn・ターン + " ターン開始_/_/_/_/_/_/_/_/_/_");
                setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f03_ターン進行中);
                command3・戦闘ターン開始();
                while (p_nowFase・フェーズ != EBattleFase・戦闘フェーズ.f06_ターン終了中)
                {
                    // ボタン操作，コマンド入力受付
                    // [TODO]ここは入力をつくる必要？
                    EBattleFase・戦闘フェーズ _nextFase = game.setNextFase_FromBattleActions・アクションから次のフェーズを設定();
                    // [Q]イベント駆動型だとここにこう書くのは変？//_nextFase = doInput・入力操作から次のフェーズを決定();
                    switch (_nextFase)
                    {
                        case EBattleFase・戦闘フェーズ.f03_ターン進行中:
                            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "----------------3:ターン進行中----------------");
                            setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f03_ターン進行中);
                            command4・戦闘ターン行動継続();
                            break;
                        case EBattleFase・戦闘フェーズ.f04_行動中:
                            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "★★★★★★★★4:行動中★★★★★★★★★");
                            setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f04_行動中);
                            command5・戦闘行動割り込み();
                            break;
                        case EBattleFase・戦闘フェーズ.f05_ターン停止中:
                            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "----------------5:ターン停止中----------------");
                            setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f05_ターン停止中);
                            break;
                        default:
                            break;
                    }
                    // 描画処理
                    game.draw・描画更新処理();
                    // 1モーメント（時間単位，1FPS？）待ち
                    game.waitFウェイトフレーム(1);
                }
                Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "/_/_/_/_/_/_/_/_/_/6:第 " + p_turn・ターン + " ターン終了_/_/_/_/_/_/_/_/_/_");
                setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f06_ターン終了中);
                command6・戦闘ターン終了();
                p_turn・ターン++;
            }
        }

        protected EBattleFase・戦闘フェーズ doInput・入力操作から次のフェーズを決定()
        {
            EBattleFase・戦闘フェーズ _nextFase = EBattleFase・戦闘フェーズ.f03_ターン進行中;
            // [YET]未実装
            /*

            CInputButton・ボタン入力定義 p_input・入力 = game.getInput・入力操作の();
            switch(p_input・入力){
                case EInputButton・入力ボタン.攻撃ボタン:
                    _nextFase = EBattleFase・戦闘フェーズ.f04_行動中;
                    break;
                default:
                    break;
            }
             * */
            return _nextFase;
        }

        protected int viewBattleWindow・戦闘画面表示()
        {
            return 0;
        }
        protected int initializeChara・キャラ戦闘開始状態初期化()
        {
            return 0;
        }

        #region メインの戦闘の流れ処理（comannd1～6）
        protected void command1・戦闘会話(){
            // [TODO]
        }
        protected void command2・戦闘作戦入力()
        {
            int _キャラID = 0;
            // （めいれいさせろの仲間がいる時）味方キャラの作戦を入力
            foreach (CChara・キャラ _キャラ in p_charaPlayer・味方キャラ)
            {
                string _作戦 = _キャラ.Var(EVar.作戦);
                // ■command2-1: 作戦決め
                if (_作戦 == CVarValue・変数値.sakusen01_めいれいさせてね.ToString())
                {
                    // [TODO]作戦は毎回でもいいが，あるボタンを押して任意のタイミングでやる方法も有りにする？
                    // 作戦の音（ピコーン）
                    Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "-----------------------" + _キャラ.name名前() + "の作戦----------------");
                    CVarValue・変数値 _このターンの作戦 = game.showBattleCommand2・戦闘コマンド画面２作戦を表示して入力待機(_キャラ);
                    _キャラ.setVar・変数を変更(EVar.このターンの作戦, _このターンの作戦);
                }
                else
                {
                    _キャラ.setVar・変数を変更(EVar.このターンの作戦, _キャラ.Var(EVar.作戦));
                }

                // ■command2-2: 攻撃対象を選択（攻撃対象を指定可能な作戦の場合）
                if (_作戦 == CVarValue・変数値.sakusen01_めいれいさせてね.ToString() || _作戦 == CVarValue・変数値.sakusen02_アイツを狙え.ToString() || _作戦 == CVarValue・変数値.sakusen21_絶対服従.ToString())
                {
                    int _攻撃対象キャラID = game.showBattleCommand2_2・戦闘コマンド画面２攻撃対象を表示して入力待機(_キャラ, p_charaAll・全キャラ);
                    p_charaAll・全キャラ[_キャラID].setVar・変数を変更(EVar.このターンの攻撃対象id, _攻撃対象キャラID.ToString());
                }

                _キャラID++;
            }
            
        }
        virtual protected void command3・戦闘ターン開始()
        {
            // このターンのパラメータの代入

            // CPU思考処理？
            //Program・プログラム.printlnLog(ELogType.l4_重要なデバッグ, "-----------------------CPU思考中-----------------------");
        }
        virtual protected void command4・戦闘ターン行動継続()
        {
            // 行動継続中は常に呼び出されるメソッド
            p_time・経過時間 += CGameManager・ゲーム管理者.s_FRAME1_MSEC・1フレームミリ秒;
            // 行動力・SPの回復
            c4_1・パラ自然回復(p_charaPlayer・味方キャラ);
            c4_1・パラ自然回復(p_charaEnemy・敵キャラ);
            c4_1・パラ自然回復(p_charaOther・その他キャラ);
            
        }
        virtual protected void command5・戦闘行動割り込み()
        {
            // [TODO]
        }
        virtual protected void command6・戦闘ターン終了()
        {

        }
        #endregion



        #region サブの流れ処理(c*_*)

        public void c4_1・パラ自然回復(List<CChara・キャラ> _キャラたち){
            foreach (CChara・キャラ _キャラ in _キャラたち)
            {
                // 行動力(AP)を回復
                _キャラ.setPara(EPara.s20_AP, ESet.add・増減値, (_キャラ.Para(EPara.a4_素早さ) / 100.0 * s_行動ゲージ回復量＿素早さ１００につき * _キャラ.paraAuto・パラ補正乗除() + _キャラ.paraAuto・パラ補正増減()));
                //[MEMO]これでもいい //_chara.setParaValue(EPara.s20_AP, _chara.ko行動ゲージ() + s_行動ゲージ回復量＿素早さ１００につき * (_chara.su素早さ() / 50.0)) +  * _chara.Para(EPara.パラ自然回復補正乗除).get() + _chara.paraAuto・パラ自然回復補正値());

                // SPを回復
                _キャラ.setPara(EPara.s04_SP, ESet.add・増減値, (_キャラ.Para(EPara.s04_SP) / 100.0) * s_SP回復量＿精神力１００につき * _キャラ.paraAuto・パラ補正乗除() + _キャラ.paraAuto・パラ補正増減());
            }
        }
        
        #endregion






    }
}
