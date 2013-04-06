using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//using merusaia.Nakatsuka.Tetsuhiro.Experiment; // using使用前に，必ずプロジェクト右クリック→「参照の追加」→「プロジェクト」を追加すること！

using Yanesdk.Timer; // やねうらお様のゲームライブラリSDK
using Yanesdk.Draw;

namespace PublicDomain
{
    /// <summary>
    /// このゲーム画面を表示（管理）しているWindows専用のFormクラス．
    /// 今は、メインモード選択画面と戦闘画面とシナリオ画面を兼用しています。
    /// 
    /// 　　　　　※描画処理をWindows以外の移植をしやすくるすために，描画処理はFormを極力触らないようにしたいんだけど…，
    /// やっぱりFormならではの便利な機能も多いし、出力系はここに結構な処理を書いちゃってると言う現状。。
    /// できるだけメソッド化して後で移植しやすいようにしてください。
    /// </summary>
    public partial class FGameBattleForm1 : Form
    {
        // デバッグ時、ここを変更すると継続できないので、増えてきたら他のクラス（Var・変数.setVar***など）に移行しよう
        public bool p_isShowEnemyHP・敵のＨＰを表示するか = false;
        public bool p_isShowEnemyPara・敵のパラメータを表示するか = true;
        public bool p_isShowEnemyCommand・敵のダイスコマンドを表示するか = true;

        /// <summary>
        /// ゲームの様々なクラスを総合的に扱う、ゲーム受け渡しデータです。
        /// </summary>
        private CGameManager・ゲーム管理者 game;
        public CGameManager・ゲーム管理者 getP_GameData・ゲーム受け渡しデータ()
        {
            return game;
        }
        /// <summary>
        /// CGameData・ゲーム受け渡しデータのインスタンスgを生成したか、つまりゲームを初期化したかどうかです。
        /// 一度trueにしてからは、内部でも変更しないでください。
        /// </summary>
        private bool p_isCGameDataIlinalized・ゲーム初期化処理を実行したか = false;
        /// <summary>
        /// ゲームを初期化する処理です。メモリを食うnewが多いので、ゲーム終了まで一度きりしか呼びださないでください。
        /// </summary>
        public void _startDiceBattleGame・ダイスバトルゲーム()
        {
            // 【ゲーム初期化処理】（ゲーム終了までは一度きり！）
            // ●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●
            // (1)ゲームデータgの初期化
            game = new CGameManager・ゲーム管理者();
            // (2)ゲームデータgへ格納する，ゲーム画面の設定
            p_メッセージボックス = new CWinMainTextBox・メインテキストボックス(game, this, mainRichTextBox・メインメッセージボックス, mainSelectBox・選択肢リストボックス, mainInputGroup・入力メッセージグループボックス, richTextInputLabel1, textInput1, richTextInputLabel2, textInput2, richTextInputLabel3, textInput3);
            // gameWindow・ゲーム画面().createNewWindow・画面初期化(800, 600, p_usedForm・使用フォーム);
            p_ゲーム画面 = new CGameWindow・ゲーム画面(game, this.Width, this.Height, this, p_メッセージボックス);
            // (3)ゲームデータgに，ゲーム画面を登録
            game.setP_gameWindow・ゲーム画面(p_ゲーム画面);
            // 初期化終わり（スレッドなどを開始）
            p_isCGameDataIlinalized・ゲーム初期化処理を実行したか = true;

            // (4)テストゲーム画面、その他のフォームの呼び出し
            //p_scinarioCreateForm = new FScinarioCreateForm(game);
            //p_scinarioCreateForm.Show();
            //p_drawForm = new FDrawForm();
            //p_drawForm.Show();
            p_testBalanceForm = new FTestBalanceForm(game);
            p_testBalanceForm.Show();
            Program・実行ファイル管理者.p_log = new CLog(this.p_testBalanceForm.richtextBattle・戦闘);
            p_testBalanceForm.Hide();


            // テストゲームを呼び出し
            CTestGame・テストゲーム _testgame = new CTestGame・テストゲーム(game, this);
            // これより下の処理は、テストゲームのコンストラクタが終了してからでないと実行されないので注意。
            bool _isTempTestEnd = true;
            // ●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●●
        }
        /// <summary>
        /// ゲームバランスを調整するフォームです。
        /// </summary>
        public FTestBalanceForm p_testBalanceForm;
        /// <summary>
        /// シナリオをゲーム中に作成・編集するフォームです。
        /// </summary>
        public FScinarioCreateForm p_scinarioCreateForm;
        /// <summary>
        /// 画面描画するフォームです。
        /// </summary>
        public FDrawForm p_drawForm;


        CGameWindow・ゲーム画面 p_ゲーム画面; // WindowsFormを含む
        CWinMainTextBox・メインテキストボックス p_メッセージボックス;

        // Windowフォームのコントロールの名前と，識別名との対応表

        // キャラ１（主人公）のパラメータ
        GroupBox c1_group;
        RichTextBox[] c1_name; // 主人公[0]だけでなく、パーティキャラ[1～3]含まれる。
        PictureBox c1_image;
        Button c1_HP; int c1_HPMax_Width;
        Button c1_SP; int c1_SPMax_Width;
        Button c1_AP; int c1_APMax_Width;
        RichTextBox c1_serihu;
        ListBox c1_dice;
        RichTextBox c1_cost;
        RichTextBox c1_LV;
        RichTextBox c1_paraP;
        RichTextBox c1_paraM ;
        // キャラ１b～１Ｎ。パーティの名前や主要パラメータ（ＨＰなど）
        //RichTextBox c1b_name;
        //RichTextBox c1c_name;
        //RichTextBox c1d_name;
        //Button c1b_HP;
        //Button c1c_HP;
        //Button c1d_HP;

        // キャラ２（敵リーダー）のパラメータ
        GroupBox c2_group;
        RichTextBox[] c2_name; // リーダー[0]だけでなく、パーティキャラ[1～3]含まれる。
        PictureBox c2_image;
        Button c2_HP; int c2_HPMax_Width;
        Button c2_SP; int c2_SPMax_Width;
        Button c2_AP; int c2_APMax_Width;
        RichTextBox c2_serihu;
        ListBox c2_dice;
        RichTextBox c2_cost;
        RichTextBox c2_LV;
        RichTextBox c2_paraP;
        RichTextBox c2_paraM;
        // キャラ１b～１Ｎ（敵パーティ）。パーティの名前や主要パラメータ（ＨＰなど）
        //RichTextBox c2b_name;
        //RichTextBox c2c_name;
        //RichTextBox c2d_name;
        //Button c2b_HP;
        //Button c2c_HP;
        //Button c2d_HP;

        // 対応表の続きとして、メインのコントロール
        /// <summary>
        /// butMainが、基本的に「次へ（進む）」もしくは「何も進まない」の意味を示すボタンとする
        /// </summary>
        Button mainButton・次へボタン;
        RichTextBox mainRichTextBox・メインメッセージボックス;
        // メインテキストボックスはグループを持たないGroupBox mainTextBoxGroup;
        ListBox mainSelectBox・選択肢リストボックス;
        GroupBox mainInputGroup・入力メッセージグループボックス;


        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public FGameBattleForm1()
        {
            InitializeComponent();

            // コントロールのキーイベントを，フォームのキーイベントでは受け取れないようにします．
            this.KeyPreview = false; // （Form1_KeyDownイベントで全てのコントロールのものを統一するなら、）=true;

            // ピクチャボックスに画像をドラッグアンドドロップできるようにする 参考：http://pgchallenge.seesaa.net/article/65889017.html
            // AllowDropプロパティはインテリセンスにはないので注意！ http://www.atmarkit.co.jp/bbs/phpBB//viewtopic.php?topic=33272&forum=7&4）
            pictureBox1.AllowDrop = true;
            pictureBox2.AllowDrop = true;

            // フォームのコントロール名（適当）と意味の対応付け
            c1_name = new RichTextBox[4];
            c1_name[0] = this.richTextNameP1;
            c1_name[1] = this.richTextNameP2;
            c1_name[2] = this.richTextNameP3;
            c1_name[3] = this.richTextNameP4;
            c1_image = this.pictureBox1;
            c1_HP = this.button1; c1_HPMax_Width = this.button1.Width;
            c1_SP = this.button2; c1_SPMax_Width = this.button2.Width;
            c1_AP = this.button5; c1_APMax_Width = this.button5.Width;
            c1_serihu = this.richTextSerihu1;
            c1_dice = this.listDicePlayer1;
            c1_cost = this.richTextBox8;
            c1_LV = this.richTextBox5;
            c1_paraP = this.richTextParaShintai;
            c1_paraM = this.richTextParaSeisin;

            c2_name = new RichTextBox[4];
            c2_name[0] = this.richTextNameE1;
            c2_name[1] = this.richTextNameE2;
            c2_name[2] = this.richTextNameE3;
            c2_name[3] = this.richTextNameE4;
            c2_image = this.pictureBox2;
            c2_HP = this.button4; c2_HPMax_Width = this.button4.Width;
            c2_SP = this.button3; c2_SPMax_Width = this.button3.Width;
            c2_AP = this.button6; c2_APMax_Width = this.button6.Width;
            c2_serihu = this.richTextSerihu2;
            c2_dice = this.listDiceEnemy1;
            c2_cost = this.richTextBox18;
            c2_LV = this.richTextBox20;
            c2_paraP = this.richTextBox14;
            c2_paraM = this.richTextBox11;

            mainButton・次へボタン = this.butNext次へボタン;

            mainRichTextBox・メインメッセージボックス = this.richTextBox1;
            // 以下、デフォルトからの変更点。
            // スクロールバーは垂直のみ有効
            mainRichTextBox・メインメッセージボックス.ScrollBars = RichTextBoxScrollBars.Vertical;
            // メインテキストボックスのフォーカスを失った時に、選択を非表示にしないfalse（trueだと選択を非表示）
            //（スクロールには影響無し）mainRichTextBox・メインメッセージボックス.HideSelection = false;
            // タブを有効にする
            mainRichTextBox・メインメッセージボックス.AcceptsTab = true;
            // ドラッグ＆ドロップを有効にするか（シナリオファイルの貼りつけ、画像などをドラッグ＆ドロップすると反映されたりする機能を作るなら、Trueにする）
            //mainRichTextBox・メインメッセージボックス.EnableAutoDragDrop = true;
            
            mainSelectBox・選択肢リストボックス = this.listBox1SelectBox;
            mainInputGroup・入力メッセージグループボックス = this.groupInputBox;

            #region パラメータの説明のツールチップ
            int i = 1;
            ToolTip _toopTip = new ToolTip();
            _toopTip.SetToolTip(richIro1, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro2, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro3, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro4, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro5, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro6, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro7, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro8, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro9, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro10, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro11, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toopTip.SetToolTip(richIro12, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            #endregion




            // ■ここで，最初のテスト画面を決める
            _startDiceBattleGame・ダイスバトルゲーム();
            //startTestDraw・テスト画面描画();
        }
        public void _startTestDraw・テスト画面描画()
        {
            this.timer1.Enabled = true;
            this.groupBox1.Visible = false;
            this.groupBox2.Visible = false;
        }
        // 定数の定義
        // 曲終了時に次の曲を流したい場合(Windows.Forms限定)　http://bb-side.com/modules/side03/index.php?content_id=32
        // 曲終了時の処理
        // ※■■注意！：これを実装しただけで、Win32Exception、"ウィンドウのハンドルを作成中にエラーが発生しました。"が出た。とりあえずWindowsメッセージは実装しない方向で。
        ///// <summary>
        ///// Windows.FormのWindowsのウィンドウメッセージです。MCIで再生されていたサウンドの終了タイミング取得などに使われています。
        ///// </summary>
        ///// <param name="m"></param>
        //protected override void WndProc(ref System.Windows.Forms.Message m)
        //{

        //    if (m.Msg == MM_MCINOTIFY && (int)m.WParam == MCI_NOTIFY_SUCCESSFUL)
        //    {
        //        // 再生終了時の処理（*2）
        //        //（*2）MCI では、再生終了時に指定したウィンドウに NOTIFY メッセージが送られる。
        //        //        そのメッセージを利用して再生終了時の処理を実行することができる。
        //        //        また、次のファイルを再生する場合は、前のファイルをクローズすること。 

        //        // とりあえずクローズだけしておいて、MCIのリソースを空けるようにしておく。
        //        MySound_Windows.MCI_stopBGM(); // これでMySound_Windows.MCI_getPlayingBGMName_FullPath();も""になる。
        //    }
        //    //base.WndProc (ref m);
        //}
        // 定数の定義
        private static int MM_MCINOTIFY = 0x3B9;
        private static int MCI_NOTIFY_SUCCESSFUL = 1;

        /// <summary>
        /// 戦闘を始める前に呼び出す画面初期化メソッドです。敵味方リーダーキャラの戦闘ステータスを表示します。
        /// </summary>
        public void _showBattleInitial・戦闘画面初期化処理(CBattle・戦闘 _b)
        {
            // ●キャラ1の情報を更新
            if (_b.p_charaPlayer・味方キャラ.Count > 0)
            {
                CChara・キャラ _c1 = MyTools.getListValue(_b.p_charaPlayer・味方キャラ, _b.p_charaPlayer_Index・味方キャラ_主人公ID);
                _drawCharaPara・キャラの名前やＨＰやパラメータを表示(true, _c1, _b.p_charaPlayer_Index・味方キャラ_主人公ID, true, true);
            }

            // ●キャラ2の情報を更新
            if (_b.p_charaEnemy・敵キャラ.Count > 0)
            {
                CChara・キャラ _c2 = MyTools.getListValue(_b.p_charaEnemy・敵キャラ, 0); //[TODO]ここの0は暫定
                _drawCharaPara・キャラの名前やＨＰやパラメータを表示(false, _c2, _b.p_charaEnemy_Index・敵キャラ_リーダーID, true, true);
            }

            _drawGameForm・常時描画処理();
        }
        #region 以下草案メモ
        ///// <summary>
        ///// 画面にキャラのダイスバトルのステータスを表示します。
        ///// </summary>
        //public void showCharaDiceBattleStatus・キャラのダイスバトルステータスを初期化(bool _isShownLeft・味方側に表示するか＿falseなら敵側表示, CChara・キャラ _cキャラ)
        //{
        //    string _ダイスコマンド = "";
        //    List<object> _ダイスコマンド群 = new List<object>();

        //    // ●キャラ1の情報を更新
        //    if (_isShownLeft・味方側に表示するか＿falseなら敵側表示 == true)
        //    {
        //        CChara・キャラ _c1 = _cキャラ;
        //        string _serihu = _c1.Var(EVar.登場セリフ);
        //        if (_serihu == "")
        //        {
        //            c1_serihu.Text = "";
        //        }
        //        else
        //        {
        //            c1_serihu.Text = "「" + _serihu + "」";
        //        }
        //        //再描画しないようにする
        //        //c1_dice.BeginUpdate();
        //        for (int i = 0; i <= _c1.getP_dice・所有ダイス().Count - 1; i++)
        //        {
        //            if (_c1.getP_dice・所有ダイス()[i].p_isUseInBattle・戦闘で使用可能 == true)
        //            {
        //                _ダイスコマンド = "○" + _c1.getP_dice・所有ダイス()[i].getp_Text・詳細();
        //                _ダイスコマンド群.Add(_ダイスコマンド);
        //            }
        //        }
        //        c1_dice.Items.Clear();
        //        c1_dice.Items.AddRange(_ダイスコマンド群.ToArray());
        //        //再描画するようにする
        //        //c1_dice.EndUpdate();
        //        _ダイスコマンド群.Clear();
        //    }
        //    // ●キャラ2の情報を更新
        //    else
        //    {
        //        CChara・キャラ _c2 = _cキャラ;
        //        string _serihu2 = _c2.Var(EVar.登場セリフ);
        //        if (_serihu2 == "")
        //        {
        //            c2_serihu.Text = "";
        //        }
        //        else
        //        {
        //            c2_serihu.Text = "「" + _serihu2 + "」";
        //        }
        //        //再描画しないようにする
        //        //c2_dice.BeginUpdate();
        //        for (int i = 0; i <= _c2.getP_dice・所有ダイス().Count - 1; i++)
        //        {
        //            if (_c2.getP_dice・所有ダイス()[i].p_isUseInBattle・戦闘で使用可能 == true)
        //            {
        //                _ダイスコマンド = "○" + _c2.getP_dice・所有ダイス()[i].getp_Text・詳細();
        //                _ダイスコマンド群.Add(_ダイスコマンド);
        //            }
        //        }
        //        c2_dice.Items.Clear();
        //        c2_dice.Items.AddRange(_ダイスコマンド群.ToArray());
        //        //再描画するようにする
        //        //c2_dice.EndUpdate();
        //    }
        //    _drawFormControls・HPなどの細かいパラメータ描画更新処理(game.getP_Battle・戦闘());
        //}
        #endregion

        /// <summary>
        /// フォームの描画更新だけをします。HPなど細かいパラメータなどの更新はしません。
        /// ※.Refresh()は、外部のスレッドから触るとエラーになります。なのでprivateにしています。
        /// </summary>
        private void _drawGameForm・常時描画処理()
        {
            this.Refresh();
        }
        public void _drawCharaPara・キャラの名前やＨＰやパラメータを表示(bool _isShownLeft・味方側に表示するか＿falseなら敵側表示, CChara・キャラ _cキャラ, int _charaPartyID・パーティで何番目のキャラか, bool _isShowIroParaAndDice・ダイスパラメータも表示するか, bool _isClearOtherPatryCharaNameAndHP・他のパーティキャラの名前やＨＰラベルを初期化するか_falseだと残す)
        {
            CChara・キャラ _c = _cキャラ;
            // パーティの名前とＨＰラベルの更新
            if (_isClearOtherPatryCharaNameAndHP・他のパーティキャラの名前やＨＰラベルを初期化するか_falseだと残す == true)
            {
                // パーティの名前とＨＰラベルを初期化
                if (_isShownLeft・味方側に表示するか＿falseなら敵側表示 == true)
                {
                    for (int i = 0; i <= 3; i++) // [TODO]3は暫定値
                    {
                        setC_name・キャラの名前とＨＰラベルの更新(_isShownLeft・味方側に表示するか＿falseなら敵側表示, i, null, false);
                    }
                }
                else
                {
                }
            }
            // このキャラの名前とＨＰラベルの更新
            int _id = _charaPartyID・パーティで何番目のキャラか;
            if (_c == null)
            {
                setC_name・キャラの名前とＨＰラベルの更新(_isShownLeft・味方側に表示するか＿falseなら敵側表示, _id, _c, false);
            }
            else
            {
                setC_name・キャラの名前とＨＰラベルの更新(_isShownLeft・味方側に表示するか＿falseなら敵側表示, _id, _c, true);

                // リーダーだけ表示する項目
                if (_isShowIroParaAndDice・ダイスパラメータも表示するか == true)
                {
                    if (_isShownLeft・味方側に表示するか＿falseなら敵側表示 == true)
                    {
                        // ●味方キャラ側の表示
                        string _serihu = _c.Var(EVar.登場セリフ);
                        if (_serihu == "")
                        {
                            c1_serihu.Text = "";
                        }
                        else
                        {
                            c1_serihu.Text = "「" + _serihu + "」";
                        }

                        c1_image.Image = null;
                        c1_HP.Text = _c.para_Int(EPara.s03_HP).ToString(); c1_HP.Width = (int)(c1_HPMax_Width * (_c.para_Int(EPara.s03_HP) / Math.Max(_c.para_Int(EPara.s03b_最大HP), 0.01)));
                        c1_SP.Text = _c.para_Int(EPara.s04_SP).ToString(); c1_SP.Width = (int)(c1_SPMax_Width * (_c.para_Int(EPara.s04_SP) / Math.Max(_c.para_Int(EPara.s04b_最大SP), 0.01)));
                        c1_AP.Text = _c.para_Int(EPara.s20_AP).ToString(); c1_AP.Width = (int)(c1_APMax_Width * (_c.para_Int(EPara.s20_AP) / Math.Max(_c.para_Int(EPara.s20b_最大AP), 0.01)));
                        c1_cost.Text = "？";
                        c1_LV.Text = _c.para_Int(EPara.LV).ToString();
                        c1_paraP.Text = _c.para_Int(EPara.a1_ちから) + "\n" + _c.para_Int(EPara.a2_持久力) + "\n" + _c.para_Int(EPara.a3_行動力) + "\n" + _c.para_Int(EPara.a4_素早さ) + "\n" + _c.para_Int(EPara.a5_精神力) + "\n" + _c.para_Int(EPara.a6_賢さ);
                        c1_paraM.Text = _c.para_Int(EPara.b1_器用さ) + "\n" + _c.para_Int(EPara.b2_忍耐力) + "\n" + _c.para_Int(EPara.b3_健康力) + "\n" + _c.para_Int(EPara.b4_適応力) + "\n" + _c.para_Int(EPara.b5_集中力) + "\n" + _c.para_Int(EPara.b6_思考力);

                        // ダイスコマンドの更新
                        _drawCharaDiceCommand・キャラのダイスコマンドの更新処理(true, _c);
                    }
                    else
                    {
                        // ●敵キャラ側の表示
                        string _serihu = _c.Var(EVar.登場セリフ);
                        if (_serihu == "")
                        {
                            c2_serihu.Text = "";
                        }
                        else
                        {
                            c2_serihu.Text = "「" + _serihu + "」";
                        }

                        // リーダーだけ表示する項目
                        c2_image.Image = null;
                        c2_HP.Text = _c.para_Int(EPara.s03_HP).ToString(); c2_HP.Width = (int)(c2_HPMax_Width * (_c.para_Int(EPara.s03_HP) / Math.Max(_c.para_Int(EPara.s03b_最大HP), 0.01)));
                        c2_SP.Text = _c.para_Int(EPara.s04_SP).ToString(); c2_SP.Width = (int)(c2_SPMax_Width * (_c.para_Int(EPara.s04_SP) / Math.Max(_c.para_Int(EPara.s04b_最大SP), 0.01)));
                        c2_AP.Text = _c.para_Int(EPara.s20_AP).ToString(); c2_AP.Width = (int)(c2_APMax_Width * (_c.para_Int(EPara.s20_AP) / Math.Max(_c.para_Int(EPara.s20b_最大AP), 0.01)));
                        c2_cost.Text = "？";
                        c2_LV.Text = _c.para_Int(EPara.LV).ToString();
                        c2_paraP.Text = _c.para_Int(EPara.a1_ちから) + "\n" + _c.para_Int(EPara.a2_持久力) + "\n" + _c.para_Int(EPara.a3_行動力) + "\n" + _c.para_Int(EPara.a4_素早さ) + "\n" + _c.para_Int(EPara.a5_精神力) + "\n" + _c.para_Int(EPara.a6_賢さ);
                        c2_paraM.Text = _c.para_Int(EPara.b1_器用さ) + "\n" + _c.para_Int(EPara.b2_忍耐力) + "\n" + _c.para_Int(EPara.b3_健康力) + "\n" + _c.para_Int(EPara.b4_適応力) + "\n" + _c.para_Int(EPara.b5_集中力) + "\n" + _c.para_Int(EPara.b6_思考力);

                        // ダイスコマンドの更新
                        _drawCharaDiceCommand・キャラのダイスコマンドの更新処理(false, _c);

                        // 敵のHPなどを表示するか
                        p_isShowEnemyHP・敵のＨＰを表示するか = true; // ここで変更してもよい
                        if (p_isShowEnemyHP・敵のＨＰを表示するか == true) panel4.Visible = true; else panel4.Visible = false;
                        if (p_isShowEnemyPara・敵のパラメータを表示するか == true) panel3.Visible = true; else panel3.Visible = false;
                        if (p_isShowEnemyCommand・敵のダイスコマンドを表示するか == true)
                        {
                            listBox4.Visible = true; listDiceEnemy1.Visible = true;
                        }
                        else
                        {
                            listBox4.Visible = false; listDiceEnemy1.Visible = false;
                        }
                    }
                }
            }
        }

        private void _drawCharaDiceCommand・キャラのダイスコマンドの更新処理(bool _isShownLeft・味方側に表示するか＿falseなら敵側表示, CChara・キャラ _c)
        {
            string _ダイスコマンド = "";
            ListBox _listBox = null;
            if (_isShownLeft・味方側に表示するか＿falseなら敵側表示 == true)
            {
                _listBox = c1_dice;
            }
            else
            {
                _listBox = c2_dice;
            }
            //再描画しないようにする（チラつき防止）
            _listBox.BeginUpdate();
            // 所有ダイスとリストボックスの要素数が同じだったらそのまま値を変更するだけだが、そうでなかったら初期化する
            if (_c.getP_dice・所有ダイス().Count != _listBox.Items.Count)
            {
                _listBox.Items.Clear();
            }
            for (int i = 0; i <= _c.getP_dice・所有ダイス().Count - 1; i++)
            {
                CCommand・コマンド _ダイス = _c.getP_dice・所有ダイス()[i];
                if (_ダイス.p_isUseInBattle・戦闘で使用可能 == true)
                {
                    if (_ダイス.p_isNowUse・現在使用可能 == true)
                    {
                        // ダイスマスを暗くしたいが，そういうメソッドはListBoxにはない？（選択にする？）
                        //c1_dice.SetSelected(i, false);
                        // このテキストを変更
                        _ダイスコマンド = "○" + _c.getP_dice・所有ダイス()[i].getp_Text・詳細();
                    }
                    else
                    {
                        //c1_dice.SetSelected(i, true);
                        // このテキストを変更
                        _ダイスコマンド = "×" + _c.getP_dice・所有ダイス()[i].getp_Text・詳細();
                    }
                    if (i <= c1_dice.Items.Count - 1)
                    {
                        _listBox.Items[i] = _ダイスコマンド;
                    }
                    else
                    {
                        _listBox.Items.Add(_ダイスコマンド); // 新しくリストを増やす
                    }
                }
            }
            //再描画するようにする
            _listBox.EndUpdate();
        }
        public void _drawFormControls・HPなどの細かいパラメータ描画更新処理(CBattle・戦闘 _b)
        {
            if (_b.p_charaPlayer・味方キャラ.Count > 0)
            {
                CChara・キャラ _cb;
                // パーティの名前とＨＰラベルの更新
                for (int i = 0; i <= 3; i++)
                {
                    _cb = MyTools.getListValue(_b.p_charaPlayer・味方キャラ, i);
                    _drawCharaPara・キャラの名前やＨＰやパラメータを表示(true, _cb, i, false, false);
                }
                // リーダーだけ表示する項目
                int _readerIndex = _b.p_charaPlayer_Index・味方キャラ_主人公ID;
                CChara・キャラ _c1 = MyTools.getListValue(_b.p_charaPlayer・味方キャラ, _readerIndex);
                _drawCharaPara・キャラの名前やＨＰやパラメータを表示(true, _c1, _readerIndex, true, false);
            }

            if (_b.p_charaEnemy・敵キャラ.Count > 0)
            {
                CChara・キャラ _cb;
                // パーティの名前とＨＰラベルの更新
                for(int i=0; i<=3; i++){
                    _cb = MyTools.getListValue(_b.p_charaEnemy・敵キャラ, i);
                    _drawCharaPara・キャラの名前やＨＰやパラメータを表示(false, _cb, i, false, false);
                }
                // リーダーだけ表示する項目
                int _readerIndex = _b.p_charaEnemy_Index・敵キャラ_リーダーID;
                CChara・キャラ _c2 = MyTools.getListValue(_b.p_charaEnemy・敵キャラ, _readerIndex);
                _drawCharaPara・キャラの名前やＨＰやパラメータを表示(false, _c2, _readerIndex, true, false);

                // 敵のHPなどを表示するか
                p_isShowEnemyHP・敵のＨＰを表示するか = true; // ここで変更してもよい
                if (p_isShowEnemyHP・敵のＨＰを表示するか == true)panel4.Visible = true; else panel4.Visible = false;
                if (p_isShowEnemyPara・敵のパラメータを表示するか == true) panel3.Visible = true; else panel3.Visible = false;
                if (p_isShowEnemyCommand・敵のダイスコマンドを表示するか == true)
                {
                    listBox4.Visible = true; listDiceEnemy1.Visible = true;
                }
                else
                {
                    listBox4.Visible = false; listDiceEnemy1.Visible = false;
                }

            }

            // フォーカスをフォームに戻す
            this.Focus();
        }
        public void setC_name・キャラの名前とＨＰラベルの更新(bool _isEnemy・true敵キャラか＿false味方キャラか, int _partyNo_0To・パーティＩＤ, CChara・キャラ _c1b, bool _isVisible・表示するか)
        {
            // 表示するコントロールを特定
            RichTextBox _textBox = null;
            if (_isEnemy・true敵キャラか＿false味方キャラか == true)
            {
                _textBox = MyTools.getArrayValue<RichTextBox>(c1_name, _partyNo_0To・パーティＩＤ);
            }
            else
            {
                _textBox = MyTools.getArrayValue<RichTextBox>(c2_name, _partyNo_0To・パーティＩＤ);
            }
            if (_textBox != null)
            {
                if (_isVisible・表示するか == true)
                {
                    if (_c1b != null)
                    {
                        string _shownText = _c1b.name名前().Substring(0, Math.Min(4, _c1b.name名前().Length)) + "LV" + _c1b.Para(EPara.LV);
                        _textBox.Text = _shownText;
                        if (_isEnemy・true敵キャラか＿false味方キャラか == true || p_isShowEnemyHP・敵のＨＰを表示するか == true)
                        {
                            _textBox.Text += "\nHP:" + _c1b.para_Int(EPara.s03_HP) + "/" + _c1b.para_Int(EPara.s03b_最大HP);
                        }
                        _textBox.Visible = true;
                        // 試しにラベルで表示
                        labelNameP1.Visible = true;
                        labelNameP1.Text = _shownText;
                    }
                }
                else
                {
                    _textBox.Visible = false;
                    // 試しにラベルで表示
                    labelNameP1.Visible = false;
                }
            }
        }
        // フォームコントロールの位置情報の一時退避。0のものは後で計算する
        private int _battleMessageWidth・バトルモード時のテキストボックスの幅 = 0;
        private int _battleMessageHeight・バトルモード時のテキストボックスの高さ = 0;
        private int _battleMessageTop・バトルモード時のテキストボックスのＹ座標 = 0;
        private int _battleMessageLeft・バトルモード時のテキストボックスのＸ座標 = 0;
        private int _storyMessageWidth・ストーリーモード時のテキストボックスの幅 = 0;
        private int _storyMessageHeight・ストーリーモード時のテキストボックスの高さ = 0;
        private int _storyMessageTop・ストーリーモード時のテキストボックスのＹ座標 = 12;
        private int _storyMessageLeft・ストーリーモード時のテキストボックスのＸ座標 = 12 + 30; // + 30はメニューバーの高さ
        /// <summary>
        /// 画面をストーリーモードに変更します。
        /// </summary>
        public void _setStroyMode・ストーリーモードに画面変更(){
            saveMainMessageBoxSize・メッセージボックスのサイズを一時退避();
            // バトルモードのパラメータ画面を非表示にする
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            _storyMessageWidth・ストーリーモード時のテキストボックスの幅 = (int)(this.Width * 0.90);
            _storyMessageHeight・ストーリーモード時のテキストボックスの高さ = this.Height - 200; // -200は選択・入力ボックスの分
            mainRichTextBox・メインメッセージボックス.SetBounds(_storyMessageTop・ストーリーモード時のテキストボックスのＹ座標, _storyMessageLeft・ストーリーモード時のテキストボックスのＸ座標, _storyMessageWidth・ストーリーモード時のテキストボックスの幅, _storyMessageHeight・ストーリーモード時のテキストボックスの高さ);
        }
        /// <summary>
        /// 画面をダイスバトルモードに変更します。
        /// </summary>
        public void _setDiceBattleMode・ダイスバトルモードに画面変更()
        {
            saveMainMessageBoxSize・メッセージボックスのサイズを一時退避();
            // バトルモードのパラメータ画面を表示にする
            groupBox1.Visible = true;
            groupBox2.Visible = true;
            mainRichTextBox・メインメッセージボックス.SetBounds(_battleMessageLeft・バトルモード時のテキストボックスのＸ座標, _battleMessageTop・バトルモード時のテキストボックスのＹ座標, _battleMessageWidth・バトルモード時のテキストボックスの幅, _battleMessageHeight・バトルモード時のテキストボックスの高さ);
        }
        private void saveMainMessageBoxSize・メッセージボックスのサイズを一時退避()
        {
            if (groupBox1.Visible == true)
            {
                // バトルモードのものを退避
                _battleMessageWidth・バトルモード時のテキストボックスの幅 = mainRichTextBox・メインメッセージボックス.Width;
                _battleMessageHeight・バトルモード時のテキストボックスの高さ = mainRichTextBox・メインメッセージボックス.Height;
                _battleMessageTop・バトルモード時のテキストボックスのＹ座標 = mainRichTextBox・メインメッセージボックス.Top;
                _battleMessageLeft・バトルモード時のテキストボックスのＸ座標 = mainRichTextBox・メインメッセージボックス.Left;
            }
            else
            {
                // ストーリーモードのものを退避
                _storyMessageWidth・ストーリーモード時のテキストボックスの幅 = mainRichTextBox・メインメッセージボックス.Width;
                _storyMessageHeight・ストーリーモード時のテキストボックスの高さ = mainRichTextBox・メインメッセージボックス.Height;
                _storyMessageTop・ストーリーモード時のテキストボックスのＹ座標 = mainRichTextBox・メインメッセージボックス.Top;
                _storyMessageLeft・ストーリーモード時のテキストボックスのＸ座標 = mainRichTextBox・メインメッセージボックス.Left;
            }
        }
        /// <summary>
        /// 戦闘に表示するコマンド（ユーザが選択可能なリスト）を変更します。
        /// 
        /// 画面のコマンドリストを、引数２のキャラが持つ、引数１の形式のコマンドに変更します。
        /// ※キャラが持つコマンドを使う場合は、第３引数はnullでＯＫです。特定のコマンドを指定した場合は、第三引数に指定してください。キャラが持つコマンドよりも優先して表示されます。
        /// </summary>
        public EShowCommandType・表示コマンド _showBattleCommandList・コマンドリストを変更(EShowCommandType・表示コマンド _showCommandType, CChara・キャラ _shownChara, List<string> _shownCommand・キャラより優先される＿必要なければnullでＯＫ)
        {
            // コマンド名リストに変換して、これをリストボックスに表示
            List<string> _nameList = new List<string>();
 
            EShowCommandType・表示コマンド _before = p_nowShowCommandType・現在の表示コマンド;
            // 項目を変更するリストボックス
            ListBox _listBox = c1_dice;

            if (_shownCommand・キャラより優先される＿必要なければnullでＯＫ != null)
            {
                // 特定のコマンドを表示（現段階ではスタイルは変えてない）
                _nameList = _shownCommand・キャラより優先される＿必要なければnullでＯＫ;
            }
            else
            {
                // キャラが持つ固有コマンドを表示

                // 項目を取得するCCommand・コマンドクラスのリスト
                List<CCommand・コマンド> _commandList = null;

                // とりあえずリストボックスを表示
                _listBox.Visible = true;
                switch (_showCommandType)
                {
                    case EShowCommandType・表示コマンド._none・非表示:
                        _listBox.Visible = false;
                        break;
                    case EShowCommandType・表示コマンド.c01_First・戦闘開始用コマンド＿たたかうやにげる等:
                        // 戦闘開始用コマンドに変更
                        if (_shownChara != null)
                        {
                            _commandList = _shownChara.getp_battleCommand1・戦闘開始用コマンド();
                        }
                        break;
                    case EShowCommandType・表示コマンド.c02_Target・対象選択:
                        // 戦闘クラスから攻撃対象や補助対象を正しく認識してから、変更
                        //_shownChara.Var(EVar.
                        break;
                    case EShowCommandType・表示コマンド.c03a_DiceAtack・攻撃ダイス:
                        // ダイスコマンドに変更
                        _commandList = _shownChara.getP_dice_ToCommand・所有ダイスをコマンド型で取得();
                        break;
                    case EShowCommandType・表示コマンド.c03b_DiceDiffence・防御ダイス:
                        // 防御ダイス。未定。
                        //
                        break;
                    case EShowCommandType・表示コマンド.c03c_SlotOther・自由記述スロット:
                        // 自由記述スロット。未定。
                        //
                        break;
                    case EShowCommandType・表示コマンド.c04_Skill・特技:
                        // 特殊≒特技。まだつくってない
                        //
                        break;
                    case EShowCommandType・表示コマンド.c05_Item・アイテム:
                        // アイテム
                        //
                        break;
                    case EShowCommandType・表示コマンド.ct1_TimingBar・タイミングバー:
                        // タイミングバー。横方向に移動する棒をタイミング良く止める
                        //
                        break;
                    case EShowCommandType・表示コマンド.ct2_TimingBar・タイミングサークル:
                        // タイミングサークル。中心に集まる円をタイミング良く止める
                        //
                        break;
                }
                // コマンドリストのコマンド名を格納
                foreach (CCommand・コマンド _item in _commandList)
                {
                    // リストボックスに追加
                    _nameList.Add(_item.getp_name());
                }
            }
            _listBox.Items.Clear();
            _listBox.Items.AddRange(_nameList.ToArray()); // コピーしないとClear()するときに参照元も消えるので気を付けて
            return _before;
        }
        public EShowCommandType・表示コマンド p_nowShowCommandType・現在の表示コマンド = EShowCommandType・表示コマンド._none・非表示;
        /// <summary>
        /// ユーザに待ち時間であることを示すコントロールを表示します。引数に、待ち時間の目安となる時間をミリ秒単位で入れてください。
        /// 省略すると具体的なミリ秒は表示されません。
        /// 呼び出す前に、待ち時間中だったかを返します。
        /// </summary>
        /// <param name="_waitMSec"></param>
        public bool setWaitingView・画面に待ち時間を表示するかを設定(bool _TrueIsShown_FalseIsHide)
        {
            return setWaitingView・画面に待ち時間を表示するかを設定(_TrueIsShown_FalseIsHide, -1);
        }
        /// <summary>
        /// ユーザに待ち時間であることを示すコントロールを表示します。引数に、待ち時間の目安となる時間をミリ秒単位で入れてください。
        /// 省略すると具体的なミリ秒は表示されません。
        /// </summary>
        /// <param name="_waitMSec"></param>
        public bool setWaitingView・画面に待ち時間を表示するかを設定(bool _TrueIsShown_FalseIsHide, int _waitMSec)
        {
            bool _isBeforeShown = p_isWaitViewShown・待ち時間画面が表示されているか;
            if (_TrueIsShown_FalseIsHide == true)
            {
                // 表示
                if (p_isWaitViewShown・待ち時間画面が表示されているか == false)
                {
                    p_isWaitViewShown・待ち時間画面が表示されているか = true;
                    // 現段階では、次へボタンのスタイルを変更
                    // 前のものを退避
                    p_butNext_BeforeText = butNext次へボタン.Text;
                    p_butNext_BeforeColor = butNext次へボタン.BackColor;
                    // 変更
                    butNext次へボタン.BackColor = Color.Green;
                    if (_waitMSec != -1)//デバッグ中だけにする？ && Program・実行ファイル管理者.isDebug == true)
                    {
                        // 待ち時間を表示
                        butNext次へボタン.Text = "あと" + _waitMSec + "msec"+"";
                    }
                    else
                    {
                        butNext次へボタン.Text = "...";
                    }
                }
            }
            else
            {
                // 非表示
                if (p_isWaitViewShown・待ち時間画面が表示されているか == true)
                {
                    p_isWaitViewShown・待ち時間画面が表示されているか = false;
                    butNext次へボタン.BackColor = p_butNext_BeforeColor;
                    butNext次へボタン.Text = p_butNext_BeforeText;
                }
            }
            return _isBeforeShown;
        }
        string p_butNext_BeforeText;
        Color p_butNext_BeforeColor;
        /// <summary>
        /// 待ち時間画面が表示されているか
        /// </summary>
        bool p_isWaitViewShown・待ち時間画面が表示されているか = false;
        /// <summary>
        /// リストボックス１Ｐ側の選択したリスト配列（現時点では、正確にはダイスコマンドのＩＤでは無いので注意）を設定します
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_isSelected"></param>
        public void setC1_dice_Selected(int _index, bool _isSelected)
        {
            // _indexのリストが存在したら、セット
            if (c1_dice.Items.Count > 0 && _index <= c1_dice.Items.Count - 1)
            {
                c1_dice.SetSelected(_index, _isSelected);
            }
        }
        public void setC2_dice_Selected(int _index, bool _isSelected)
        {
            // _indexのリストが存在したら、セット
            if (c1_dice.Items.Count > 0 && _index <= c1_dice.Items.Count - 1)
            {
                c2_dice.SetSelected(_index, _isSelected);
            }
        }
        /// <summary>
        /// リストボックス１Ｐ側で選択したリスト配列（現時点では、正確にはダイスコマンドのＩＤでは無いので注意）を取得します。
        /// 配列は0～、行動を選択しなかった場合は-1が返されます。
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_isSelected"></param>
        public int getC1_dice_Selected()
        {
            return c1_dice.SelectedIndex;
        }
        /// <summary>
        /// リストボックス２Ｐ側で選択したリスト配列（現時点では、正確にはダイスコマンドのＩＤでは無いので注意）を取得します。
        /// 配列は0～、行動を選択しなかった場合は-1が返されます。
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_isSelected"></param>
        public int getC2_dice_Selected()
        {
            return c2_dice.SelectedIndex;
        }
        /// <summary>
        /// １Ｐ用ダイスフォームにフォーカスを合わせます。
        /// </summary>
        public void focusC1_dice()
        {
            c1_dice.Focus();
        }
        /// <summary>
        /// ２Ｐ用ダイスフォームにフォーカスを合わせます。
        /// </summary>
        public void focusC2_dice()
        {
            c2_dice.Focus();
        }
        /// <summary>
        /// 「次へ（進む）」ボタンにフォーカスをうつします。
        /// 「次へ（進む）」ボタンが非表示の場合は、フォームにフォーカスをうつします。
        /// </summary>
        public void focusMainControl・フォーカスをメインコントロールに移す()
        {
            if (mainButton・次へボタン.Visible == true)
            {
                // butMainが、基本的に「次へ（進む）」もしくは「何も進まない」の意味を示すボタンとする
                mainButton・次へボタン.Focus();
            }
            else
            {
                // 「次へ（進む）」ボタンが非表示の場合は、しかたがないのでフォームにフォーカスをうつす
                this.Focus();
            }
        }
        /// <summary>
        /// 戦闘用画面に使うコントロールを表示し、使わないコントロールを非表示します。
        /// </summary>
        private void showBattleControl()
        {
            groupBox1.Visible = true;
            groupBox2.Visible = true;
            picScinario.Visible = false;
        }
        /// <summary>
        /// シナリオ用画面に使うコントロールを表示し、使わないコントロールを非表示します。
        /// </summary>
        private void showScinarioControl()
        {
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            picScinario.Visible = true;
        }


        // ロード処理は特になし。基本はコンストラクタでやっている。
        private void GameTestForm1_Load(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// フォームの操作可能なコントロールを列挙した列挙体です。
        /// Windows非依存のＵＩコントロール作成や、ＵＩイベントをわかりやすく記述する時にも使います。
        /// </summary>
        public enum EControlType・操作コントロール
        {
            c0a_GoNext・次へ進むボタン,
            c0b_GoBack・前へ戻るボタン,

            c01_Form・フォーム,
            c02_MessageBox・メインメッセージボックス,
            c03_SelectBox・選択ボックス,
            c04_InputBox・入力ボックス１から３のどれか,
            c05_Label1・注釈１,
            c06_Label2・注釈２,

            c11_ListBoxDiceP1・ダイスコマンドリスト味方１,
            c12_ListBoxDiceE1・ダイスコマンドリスト敵１,
        }
        /// <summary>
        /// ※このフォーム中でイベントが起こった時は、必ずこのメソッドを呼び出してください。
        /// WindowsFormのボタンイベントとマウスイベントをリンクさせる処理です。※ボタン押し続けも対応します・
        /// 引数のキーイベントとコントロールから、ゲーム中の処理を判断し、
        /// 処理に応じたゲーム進行変数game.p_is***に代入します。
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(object _senderControl, EControlType・操作コントロール _EControlType, EInputButton・入力ボタン _mainPressedButton1・押しているボタン１, EInputButton・入力ボタン _samePushedButton2・同時に押されているボタン２, EInputButton・入力ボタン _samePushedButton3・同時に押されているボタン３)
        {
            //キーを押しっぱなしにしても、ここは実行されてる System.Console.WriteLine("ボタン押しっぱ取れてる？");

            // 操作コントロール毎に、ボタンを押した時の処理を記述
            switch (_EControlType)
            {
                case EControlType・操作コントロール.c0a_GoNext・次へ進むボタン:
                    // 入力画面でボタン操作した時の処理
                    // 決定ボタンで次へ進む、戻るボタンで前に戻る
                    if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b1_決定ボタン_A)
                    {
                        // 「次へ」ボタンをクリック（もしくは決定ボタンを押し続け）したときの処理は、決定ボタンを押した時と変わらない。
                        game.iA決定ボタンを一瞬だけ自動で押す();
                    }
                    else if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b2_戻るボタン_B)
                    {
                        // 「戻る」ボタンをクリック（もしくは戻るボタンを押し続け）したときの処理は、戻るボタンを押した時と変わらない。
                        game.iB戻るボタンを一瞬だけ自動で押す();
                    }
                    break;
                case EControlType・操作コントロール.c0b_GoBack・前へ戻るボタン:
                    // 入力画面でボタン操作した時の処理
                    // 決定ボタンと戻るボタン、どちらを押しても、戻る処理を行う
                    if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b1_決定ボタン_A ||
                        _mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b2_戻るボタン_B)
                    {
                        // 「戻る」ボタンをクリックしたときの処理は、戻るボタンを押した時と変わらない。
                        game.iB戻るボタンを一瞬だけ自動で押す();
                    }
                    break;

                case EControlType・操作コントロール.c03_SelectBox・選択ボックス:
                    // 選択肢でボタン操作した時の処理
                    if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b1_決定ボタン_A)
                    {
                        // 決定ボタンで入力待ち完了
                        game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
                        //今は使っていない。次に進むに統合。game.p_isEndSelectBox・選択ボックス完了フラグ = true;
                    }
                    else if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b2_戻るボタン_B)
                    {
                        // 戻るボタンでも入力待ち完了
                        game.p_isUserInput_Back・前に戻る入力フラグ = true; // 戻るボタンを押したことを他に通知
                        game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
                    }
                    break;

                case EControlType・操作コントロール.c02_MessageBox・メインメッセージボックス:
                    // メインメッセージボックス（主にゲーム中のテキストが表示される場所）でボタン操作した時の処理

                    // (a)クリエーションモードも関係なく、フォーム（画面にフォーカスが当たっている時）と同じとする
                    //goto case EControlType・操作コントロール.c01_Form・フォーム;

                    // (b)クリエーションとそうじゃない時でわける
                    // クリエーションモード時か
                    if (game.p_isCreation・クリエーションモード == true)
                    {
                        // メインテキストボックス中のテキストにフォーカスが当たっていたら、
                        // シナリオのテキスト編集中として、次に進まない。（falseの時だけ進める）
                        if (mainRichTextBox・メインメッセージボックス.Focused == true)
                        {
                            // フォーカスが当たっている場合、決定ボタンでフォーカスを「次へ」ボタンへうつす
                            // ※ここで決定ボタンを条件にするとテキスト選択ができなくなるので、マウス左クリックは外した方がいい。
                            // if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b1_決定ボタン_A)
                            // これは入力更新ができてないのか、押しっぱなしにしないと使えない
                            if (game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.ENTER) 
                                || game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.z))
                            {
                                focusMainControl・フォーカスをメインコントロールに移す();
                            }
                            // 他のキーでもビープ音をなるのを避ける
                            //focusMainControl・フォーカスをメインコントロールに移す();

                        }
                        else
                        {
                            // メインテキストボックス中のテキストにフォーカスが当たっていない時、決定ボタンを押して
                            // このメソッドが呼ばれたら、シナリオのテキストファイル修正を完了する。
                            if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b1_決定ボタン_A)
                            {
                                // [TODO]クリエーションモード時は、シナリオのテキストファイル修正処理
                            }
                        }
                    }
                    else
                    {
                        // クリエーションモードでなければ、フォーム（画面にフォーカスが当たっている時）と同じ
                        goto case EControlType・操作コントロール.c01_Form・フォーム;
                    }
                    break;
                case EControlType・操作コントロール.c11_ListBoxDiceP1・ダイスコマンドリスト味方１:
                    // ダイスコマンドリスト味方１
                    if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b1_決定ボタン_A ||
                        _mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b2_戻るボタン_B)
                    {
                        // ダイスを決定
                        game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
                    }
                    break;

                case EControlType・操作コントロール.c01_Form・フォーム:
                    // フォームにフォーカスが当たっている時（他の特別なコントロールにフォーカスが当たっていない時）の処理

                    // ●これが通常のボタン送り処理
                    if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b1_決定ボタン_A ||
                        _mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b2_戻るボタン_B ||
                        _mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.a4_上 ||
                        _mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.a2_下 ||
                        _mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.a3_左 ||
                        _mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.a1_右)
                    {
                        // 十字キーやＡ決定ボタン・Ｂ戻るボタンを押したら次に進む
                        game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
                    }
                    else if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b4_シフトボタン_X)
                    {
                        // Xシフトボタンで自動モード？（メッセージ自動送り、または自動戦闘）
                        if (game.p_isAutoPlay・自動モード == false) { game.p_isAutoPlay・自動モード = true; } else { game.p_isAutoPlay・自動モード = false; }
                    }
                    else if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b3_コントロールボタン_Y)
                    {
                        // Yコントロールボタンで一番最初の単語のヘルプorＵＲＬリンクをブラウザで開く？（未実装）
                    }
                    else if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b5_タブボタン_L)
                    {
                        // Lタブボタンでメッセージ履歴表示？（未実装）
                    }
                    else if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b6_キャプスロックボタン_R)
                    {
                        // Rキャプスロックボタンでスキップモード？（メッセージスキップ、または高速スキップ戦闘）
                        if (game.p_isSkip・スキップモード == false) { game.p_isSkip・スキップモード = true; } else { game.p_isSkip・スキップモード = false; }
                    }
                    else if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b9_スペースボタン_START)
                    {
                        // STARTスペースボタンでポーズ切り替え
                        if (game.p_isPause・ポーズモード == false) { game.p_isPause・ポーズモード = true; } else { game.p_isPause・ポーズモード = false; }
                    }
                    else if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b10_アルトボタン_SELECT)
                    {
                        // SELECTアルトボタンは拡張ボタン、いろんなボタンと組み合わせてショートカット（未実装）
                        if (_samePushedButton2・同時に押されているボタン２ == EInputButton・入力ボタン.b1_決定ボタン_A)
                        {
                            // 例：SELECT+Aでデバッグ／テキスト編集／クリエーションモード切り替え
                            if (game.p_isCreation・クリエーションモード == false) { game.p_isCreation・クリエーションモード = true; } else { game.p_isCreation・クリエーションモード = false; }
                        }
                        else if (_samePushedButton2・同時に押されているボタン２ == EInputButton・入力ボタン.b9_スペースボタン_START)
                        {
                            // 例：SELECT+STARTでスクリーンショット
                            // 音を鳴らす
                            game.pSE(ESE・効果音._system09・スクリーンショット音_カシャッ);
                            Bitmap _screenShotImage = MyTools.getScreenCapture_ActiveWindow();
                            string _fileName = MyTools.getNowTime_Japanese() + ".png";
                            _screenShotImage.Save(Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス
                                + "スクリーンショット\\" + _fileName);
                        }
                    }
                    break;

                default:
                    // その他のコントロール（入力ボックス１～３や、ラベルなど）の場合

                    // Enterを押すと、Tabインデックスが次に大きい（もしくは最初の）、
                    // 次のコントロールにフォーカスをうつす
                    if (_mainPressedButton1・押しているボタン１ == EInputButton・入力ボタン.b1_決定ボタン_A)
                    {
                        SelectNextControl((Control)_senderControl, true, true, true, true);
                        // フォーカス後のコントロール
                        Control _nextControl = this.ActiveControl;
                        if (_nextControl.TabStop == false)
                        {
                            // フォーカスが当たるべきではないコントロール（TabSop==false）のとき、メインボタンにコントロールをうつす
                            focusMainControl・フォーカスをメインコントロールに移す();
                        }
                        // 以下の様なこと、わざわざしなくていい。
                        //if (textInput2.Visible == true)
                        //{
                        //    textInput2.Focus();
                        //}else...
                    }
                    break;
            }
            // ●●●省エネモードの時は、この処理をかかないとupdateFrameメソッドが呼ばれない
            if (game.getP_gameWindow・ゲーム画面() != null)
            {
                game.getP_gameWindow・ゲーム画面().setisEventOccured・イベントが起こったかを設定(true);
            }
        }
        #region 引数の異なる同名メソッド
        /// <summary>
        /// ※このフォーム中でイベントが起こった時は、必ずこのメソッドを呼び出してください。
        /// WindowsFormのボタンイベントとマウスイベントをリンクさせる処理です。
        /// 引数のキーイベントとコントロールから、ゲーム中の処理を判断し、
        /// 処理に応じたゲーム進行変数game.p_is***に代入します。
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(EControlType・操作コントロール _EControlType, System.Windows.Forms.Keys _keys)
        {
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(null, _EControlType, _keys);
        }
        /// <summary>
        /// ※このフォーム中でイベントが起こった時は、必ずこのメソッドを呼び出してください。
        /// WindowsFormのボタンイベントとマウスイベントをリンクさせる処理です。
        /// 引数のキーイベントとコントロールから、ゲーム中の処理を判断し、
        /// 処理に応じたゲーム進行変数game.p_is***に代入します。
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(object _senderControl, EControlType・操作コントロール _EControlType, System.Windows.Forms.Keys _keys)
        {
            // 他のボタンは無入力
            EInputButton・入力ボタン _input1 = game.getP_InputButton().getInputButton(_keys);
            EInputButton・入力ボタン _input2 = EInputButton・入力ボタン._none_無入力;
            EInputButton・入力ボタン _input3 = EInputButton・入力ボタン._none_無入力;
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(_senderControl, _EControlType, _input1, _input2, _input3);
        }
        /// <summary>
        /// ※このフォーム中でイベントが起こった時は、必ずこのメソッドを呼び出してください。
        /// WindowsFormのボタンイベントとマウスイベントをリンクさせる処理です。
        /// 引数のキーイベントとコントロールから、ゲーム中の処理を判断し、
        /// 処理に応じたゲーム進行変数game.p_is***に代入します。
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(EControlType・操作コントロール _EControlType, EInputButton・入力ボタン _input1)
        {
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(null, _EControlType, _input1);
        }
        /// <summary>
        /// ※このフォーム中でイベントが起こった時は、必ずこのメソッドを呼び出してください。
        /// WindowsFormのボタンイベントとマウスイベントをリンクさせる処理です。
        /// 引数のキーイベントとコントロールから、ゲーム中の処理を判断し、
        /// 処理に応じたゲーム進行変数game.p_is***に代入します。
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(object _senderContorl, EControlType・操作コントロール _EControlType, EInputButton・入力ボタン _input1)
        {
            // 他のボタンは無入力
            EInputButton・入力ボタン _input2 = EInputButton・入力ボタン._none_無入力;
            EInputButton・入力ボタン _input3 = EInputButton・入力ボタン._none_無入力;
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(_senderContorl, _EControlType, _input1, _input2, _input3);
        }
        #endregion




        #region ■■■フォームのTimer タイマーイベント：　定期的な論理処理や描画処理を管理するクラスやメソッドなど

        // 今はtimer1のタイマーで管理
        /// <summary>
        /// Timer1の更新間隔です。デザイナで設定しても、こちらが優先されます。
        /// </summary>
        public int p_timer1_interval・タイマーの更新ミリ秒 = 20; // fps50だったら、interval=1000/50=20
        /// <summary>
        /// 意図的にtimer1を止めたい時はfalseにしてください。
        /// </summary>
        public bool p_isTimer1Run = false; // ■■■ここでフォームタイマーが動くか動かないかが決まる。
        /// <summary>
        /// 押されたボタンをコンソール中に表示するテスト中の時にtrueになります。butいろいろテスト、ボタンで設定します。
        /// </summary>
        public bool p_isTimerTestButton・押したボタンをラベルに表示するテスト中 = false;
        public int p_timer1_startTime = 0;
        //public int p_timer1_FrameNo1ToFPSMAX = 1;今のフレーム数表示したい場合つかってもいいかも
        /// <summary>
        /// ■■■タイマーによって定期的に呼ばれるメソッド。
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // ゲーム初期化前は実行しない
            if (p_isCGameDataIlinalized・ゲーム初期化処理を実行したか == false)
            {
                return;
            }
            // 念のため、timerの更新間隔を更新する？（他のメソッドで変更される可能性が高いため）
            //p_timer1_interval・タイマーの更新ミリ秒 = CGameManager・ゲーム管理者.s_FRAME1_MSEC・1フレームミリ秒;
            //timer1.Interval = p_timer1_interval・タイマーの更新ミリ秒;

            if (p_isTimer1Run == false){
                timer1.Enabled = false;
                return;
            }else{
                // ゲームプレイ時間を計算するため、タイマーがスタートした時間を格納
                if (p_timer1_startTime == 0) p_timer1_startTime = MyTools.getNowTime_fast();
                // 1回の処理のかかる時間を計測
                int _t1 = MyTools.getNowTime_fast();

                // ■ゲームのフレーム更新処理
                string _frameInfo・出力情報 = game.getUpdateFrameInfoフレーム更新出力情報を取得();
                
                //(b)ゲームメインスレッドがこのメソッドの場合 string _frameInfo・出力情報 = game.updateFrameフレーム毎に呼び出す入力論理描画などフレーム更新処理();
                // ここでこんなんちょくせつさわったらあかんで。全部g.updateFrameにまかせとったら大丈夫やでg.getP_fpsManager・フレーム管理者().updateFrame・論理処理後のフレーム更新処理と描画処理スキップ判定();
                // 出力処理
                if (_frameInfo・出力情報 != "")
                {
                    // コンソールはいらへん。MyTools.ConsoleWriteLine(_frameInfo・出力情報);
                    // Label1にも最大3行までで追加
                    string _labFrameInfoText = labInfo2.Text;
                    if (MyTools.getLineNo(_labFrameInfoText) < 3)
                    {
                        labInfo2.Text += "\n"+_frameInfo・出力情報;
                    }
                    else
                    {
                        labInfo2.Text = MyTools.getStringLines_Updated(_labFrameInfoText, _frameInfo・出力情報);
                    }
                }

                // ●いろいろテスト
                if (p_isTimerTestButton・押したボタンをラベルに表示するテスト中 == true)
                {
                    // ボタンの表示をマウステスト中にする
                    butTestいろいろテスト.Text = "ボタンテスト中…";
                    butTestいろいろテスト.BackColor = Color.Green;

                    string _buttonInfo = "";
                    // チェックしたいボタンやキーボードキーやマウスボタンをここにかいてね
                    #region ボタンテスト

                    // 全てのボタンをチェック
                    bool _isTestShowAllButtons = false;
                    if(_isTestShowAllButtons == true){
                        foreach (EInputButton・入力ボタン _key in Enum.GetValues(typeof(EInputButton・入力ボタン)))
                        {
                            if (game.ibボタンを押したか_連射非対応(_key) == true)
                            {
                                _buttonInfo += ("\n●"+_key.ToString() + " ボタンが押されたよ: game.ibボタンを押したか_連射非対応(" + _key.ToString() + ")==true");
                            }
                        }
                    }
                    // 全てのキー（マウスボタン、ダブルクリック、長押しも含む）をチェック
                    bool _isTestShowAllEKeyCodes = false; // ■これtrueにすると処理重くなる（20ミリ秒毎に約500ループで結構ファンうるさくなる）ので注意。
                    if (_isTestShowAllEKeyCodes == true)
                    {
                        foreach (EKeyCode _key in Enum.GetValues(typeof(EKeyCode)))
                        {
                            // デバッグ用
                            //if (_key == EKeyCode.MOUSE_LEFT_DOUBLECLICK)
                            //{
                            //    int a = 0;
                            //}
                            if (game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(_key) == true)
                            {

                                _buttonInfo += ("\n" + _key.ToString() + " キーが押されたよ: game.i指定キーを押したか_連射非対応(" + _key.ToString() + ")==true");
                            }
                        }
                    }
                    #region 一部の指定キーだけ確認する場合
                    bool _isTestOnlyPartKeys = true;
                    if (_isTestOnlyPartKeys == true)
                    {
                        // マウスクリック
                        // こっちでもできるけど、マウスだけ依存の実装だから、あまり使わないで// if (game.getP_mouseInput().IsPush(EMouseButton.Left) == true) 
                        // 通常はこっちを使ってね
                        // if(game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_LEFT_CLICK) == true)
                        //{
                        //    _buttonInfo += ("\n"+"マウス左クリックが押されたよ");
                        //}
                        // フォーム中の一時的なマウスクリック（テストで使ってるが、falseの初期化がうまくいかないときがあるので、スレッドで検知している）
                        //if (game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ == true)
                        //{
                        //    //_buttonInfo += ("\n"+"マウス左クリックが押されたよ : game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ");
                        //    game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = false; // マウスクリックリセット
                        //}

                        // キー単位
                        //if (game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.ENTER) == true)
                        //{
                        //    _buttonInfo += ("\n" + "ENTERが押されたよ: game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.ENTER");
                        //}

                        // ボタン単位
                        //if (game.ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b1_決定ボタン_A) == true)
                        //{
                        //    _buttonInfo += ("\n" + "決定ボタン（A）（連射非対応）が押されたよ: game.ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b1_決定ボタン_A");
                        //}
                        // 下記は上記と一緒
                        //if (game.getP_InputButton().isPulled・ボタンを押し離した瞬間か(EInputButton・入力ボタン.b1_決定ボタン_A) == true)
                        // 連射対応は押しっぱなしで連射になる
                        //if (game.ibボタンを押し中か_連射対応(EInputButton・入力ボタン.b1_決定ボタン_A) == true)
                        //{
                        //    _buttonInfo += ("\n" + "決定ボタン（A）【連射対応】が押されたよ: game.ibボタンを押し中か_連射対応(EInputButton・入力ボタン.b1_決定ボタン_A");
                        //}
                        //if (game.ibボタンを押し中か_連射対応(EInputButton・入力ボタン.b2_戻るボタン_B) == true)
                        //{
                        //    _buttonInfo += ("\n" + "バックボタン（B）【連射対応】が押されたよ: game.ibボタンを押し中か_連射対応(EInputButton・入力ボタン.b2_戻るボタン_B");
                        //}
                        // ダブルクリック用
                        if (game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_LEFT_DOUBLECLICK) == true)
                        {
                            _buttonInfo += ("\n" + "マウス左ダブルクリックしたよ: game.i指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_LEFT_DOUBLECLICK) == true");
                        }
                        if (game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_LEFT_TRIPLECLICK) == true)
                        {
                            _buttonInfo += ("\n" + "マウス左トリプルクリックしたよ: game.i指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_LEFT_TRIPLECLICK) == true");
                        }
                        if (game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_RIGHT_DOUBLECLICK) == true)
                        {
                            _buttonInfo += ("\n" + "右ダブルクリックしたよ: game.i指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_RIGHT_DOUBLECLICK) == true");
                        }
                        if (game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_RIGHT_TRIPLECLICK) == true)
                        {
                            _buttonInfo += ("\n" + "右トリプルクリックしたよ: game.i指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_LEFT_TRIPLECLICK) == true");
                        }
                        if (game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_LEFT_PRESSLONG) == true)
                        {
                            _buttonInfo += ("\n" + "マウス左長押しをしたよ: game.i指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.MOUSE_LEFT_PRESSLONG) == true");
                        }
                    }
                    #endregion
                    // 同時押しのチェック
                    bool _isTestDoubleKeysPress = true;
                    if (_isTestDoubleKeysPress == true)
                    {
                        if (game.ibボタンを同時押したか_連射非対応(EInputButton・入力ボタン.b1_決定ボタン_A, EInputButton・入力ボタン.b2_戻るボタン_B) == true)
                        {
                            _buttonInfo += ("\n" + "A+Bボタン（決定ボタン＋バックボタン）が同時押しされたよ: game.ibボタンを同時押したか_連射非対応(EInputButton・入力ボタン.b1_決定ボタン_A, EInputButton・入力ボタン.b2_戻るボタン_B) == true");
                        }
                        if (game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.LCTRL_DOUBLECLICK, EKeyCode.s) == true)
                        {
                            _buttonInfo += ("\n" + "左Ctrl+Sキーが同時押しされたよ: game.ik指定キーが押し離した瞬間か_押しっぱ連射非対応(EKeyCode.LCTRL_DOUBLECLICK, EKeyCode.s) == true");
                        }
                    }

                    // 最後にこのスレッドの計算時間を計算し、標準出力やラベルに表示。
                    string _passedMSecString = " Formスレッド計算時間: " + (MyTools.getNowTime_fast() - _t1) + "ミリ秒";
                    //最初の改行を削除
                    if (_buttonInfo.Length > 0) _buttonInfo = _buttonInfo.Substring(1);
                    // 何か更新されていれば、ボタン情報を表示
                    if (_buttonInfo.Length > 0)
                    {
                        // 最後の行だけ表示
                        labInfo1.Text = "入力ボタン情報:" + MyTools.getLineString(_buttonInfo, MyTools.getLineNo(_buttonInfo)) + _passedMSecString;
                    }
                    // 標準出力に表示するのの条件はどうする？
                    if (_buttonInfo.Length > 0) MyTools.ConsoleWriteLine(_buttonInfo + " " + _passedMSecString);
                    #endregion // ボタンテスト終了
                }

                #region 以下、テスト草案メモ
                // 以下、描画テスト

                //p_ゲーム画面.doTimerEvent(sender, _e);

                //Win32Window2DGl _描画エンジン = new Win32Window2DGl();
                //_描画エンジン.InitByHWnd(this.Handle); // このフォームをエンジンとして初期化
                //Screen2DGl _スクリーン = _描画エンジン.Screen;
                //GlTexture _画像 = new GlTexture();

                //// αブレンド（透明処理）の設定
                //_スクリーン.Blend = true;
                //_スクリーン.Select();
                //{
                //    // 描画画像の読み込み
                //    string _画像ファイル名 = Program・プログラム.p_ImageDataDirectory・画像フォルダパス + "mxp159.png"; //_ResourceDirectory + "isya02.gif";
                //    _画像.Load(_画像ファイル名);


                //    // スクリーンに対する処理はここに書く

                //    for (int i = 0; i < 1000; i++)
                //    {
                //        _スクリーン.Clear(); // 再描画
                //        _スクリーン.Blt(_画像, i, 0); // 画像を(0i,0)に等倍率で表示
                //        p_fpsTimer.WaitFrame();
                //        Program・プログラム.printlnLog(ELogType.l4_重要なデバッグ, " " + i + ": 描画してます．");
                //    }
                //    // スクリーンに対する処理はここまで
                //}
                //_スクリーン.Update();
                ////_スクリーン.Unselect();


                //// 2.テキスト描画テスト

                //string _message = "_onelineString，，，a・・・aa。_onelineString\n今日はすがすがしい朝だ。";
                //int _表示したメッセージ数 = 0;

                //while (_表示したメッセージ数 == _message.Length)
                //{
                //    // フレームされてたら？
                //    if (p_fpsTimer.ToBeRendered)
                //    {
                //        // メッセージを1文字表示
                //        game.mメッセージ_単体末尾改行なし_ボタン送り(_message);
                //        _表示したメッセージ数++;

                //        p_fpsTimer.WaitFrame(); // フレーム終了まで待つ
                //    }
                //}
                #endregion
            }



        }
            #region 以下、他のタイマーの実装方法の草案メモ
        // FpsTimer今は使ってない。
        /// <summary>
        /// fpsを管理するクラスです。
        /// </summary>
        //FpsTimer p_fpsTimer = new FpsTimer();

        // タイマースレッドを作るTimerクラス。今は使ってない。
        /// <summary>
        /// 一定時間ごとに何か処理をさせたい時に使う、
        /// （各クラスが自由に定期実行処理を追加することのできる）OnCallback用タイマです。
        /// 
        /// とあるメソッドを定期実行処理にしたい時は、
        /// 
        ///                 p_OnCallbackTimer.Tick += delegate { メソッド名(); };
        /// とかいて、そのメソッド内で一定時間ごとに処理をするようにすると便利です。
        ///
        /// 
        /// 新しくタイマーを作りたい場合は、以下をさんこうにしてください。
        ///        timer = new System.Windows.Forms.Timer();
        ///
        ///        timer.Interval = 1;
        ///        timer.Tick += delegate { OnCallback(); };
        ///        timer.Start();
        ///        
        /// 		public void OnCallback()
        ///         {
        ///             if ( FpsTimer.ToBeRendered )
        ///                 return;
        ///
        ///             // フレームスキップ処理
        ///             OnMove(); // 論理的な移動 
        ///             if ( gameContext.FPSTimer.ToBeSkip ){
        ///                 return; 
        ///             }
        ///             OnDraw(); // 画面描画
        ///         }
        /// </summary>
        //private System.Windows.Forms.Timer p_OnCallbackTimer;
        #endregion

        #endregion


        // 以下、フォームとフォームコントロールのイベント。
        // ここに次の処理を書くとWindowsフォーム依存の部分が増えるため、あまり書かないでください。

        private void butNext次へボタン_Click(object sender, EventArgs e)
        {
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                EControlType・操作コントロール.c0a_GoNext・次へ進むボタン, EInputButton・入力ボタン.b1_決定ボタン_A);
            // 「次へ」ボタンをクリックしたときの処理は、決定ボタンを押した時と変わらない。
        }

        private void butBack戻るボタン_Click(object sender, EventArgs e)
        {
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                EControlType・操作コントロール.c0b_GoBack・前へ戻るボタン, EInputButton・入力ボタン.b1_決定ボタン_A);
            // 「戻る」ボタンをクリックしたときの処理は、戻るボタンを押した時と変わらない。
        }

        #region フォームとフォームコントロールのイベント

        private void FGameTestForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // ウィンドウを閉じたとき，アプリケーションを終了します．
            // ■■■ゲーム終了の後始末！
            game.End・ゲーム終了処理();
        }
        // フォームにフォーカスが当たっ時の処理
        private void FGameBattleForm1_Enter(object sender, EventArgs e)
        {
            // よくわからないが、ここはブレークポイントを指定しても一回も実行されていない。フォームフォーカスイベントは取れない？

            // フォームにフォーカスが当たった時（フォームをクリックしたとき）、
            // 真っ先にフォーカスを当てるコントロール
            focusMainControl・フォーカスをメインコントロールに移す();
        }
        // フォームをクリックしたときの処理
        private void FGameTestForm1_Click(object sender, EventArgs e)
        {
            // メインテキストボックス中のテキストにフォーカスが当たっていたら、テキスト編集中のため、次に進まない。（falseの時だけ進める）
            if (mainRichTextBox・メインメッセージボックス.Focused == false)
            {
                // メインテキストボックスにフォーカスが当たっていない時、決定ボタン処理
                _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(
                    EControlType・操作コントロール.c01_Form・フォーム, EInputButton・入力ボタン.b1_決定ボタン_A);
            }
            else
            {
                // フォームをクリックしたとき、メインテキストボックスにフォーカスを当たっていないならば、
                // 真っ先にフォーカスを当てるコントロール（＝メインコントロール）
                focusMainControl・フォーカスをメインコントロールに移す();
            }
        }

        /// <summary>
        /// フォーム上の全てのコントロールを含めて（※），Enterキーを押したときの処理です・
        /// （ただし，※は，this.KeyPreview = trueのとき。
        /// this.KeyPreview = falseのときは、他のコントロールにフォーカスが当たっていない時だけ実行される）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void FGameTestForm1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                // Enterでフォーカスを次のコントロールに移動させる。

                Control _activeControl = this.ActiveControl; // = _senderControl;と同じ
                bool isFocusMoveCancel = false; // フォーカス移動をキャンセルするかどうか
                // Enterで改行などをする複数行入力可能なテキストボックスコントロールの場合は，フォーカス移動しない
                if (_activeControl is TextBox)
                {
                    if (((TextBox)_activeControl).Multiline == true)
                    {
                        isFocusMoveCancel = true;
                    }
                }

                if (isFocusMoveCancel == false)
                {
                    // フォーカスの移動方向（シフトを押すと逆）
                    bool isfocusMoveForward = e.Modifiers != Keys.Shift;
                    //this.ProcessTabKey(isfocusMoveForward);
                    this.SelectNextControl(this.ActiveControl, isfocusMoveForward, true, true, true);
                    // TabStopを無視して移動したのに、移動先のコントロールのTabStopプロパティがfalseのものしかない場合，
                    // 仕方がないのでフォームにフォーカスを戻す
                    if (this.ActiveControl.TabStop == false)
                    {
                        this.Focus();
                    }
                }
            }
            // メインテキストボックス中のテキストにフォーカスが当たっていたら、テキスト編集中のため、次に進まない。（falseの時だけ進める）
            if (mainRichTextBox・メインメッセージボックス.Focused == false)
            {
                // ●(a)決定ボタン処理，これを実装しないと，入力確定ができません．必ず実装してください！
                //if (_e.KeyCode == Keys.Enter || _e.KeyCode == Keys.Space || _e.KeyCode == Keys.Z)
                //{
                //    game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true;
                //    game.p_isEndSelectBox・選択ボックス完了フラグ = true;
                //}
                // ●(b)決定ボタン処理，上記の代わりにこれで行く
                _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(
                    EControlType・操作コントロール.c01_Form・フォーム, e.KeyCode);
            }
        }

        // メインメッセージボックスrichTextBox1（=mainRichTextBox・メインメッセージボックス）をクリックしたときの処理
        private void richTextBox1_Click(object sender, EventArgs e)
        {
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                EControlType・操作コントロール.c02_MessageBox・メインメッセージボックス,
                EInputButton・入力ボタン.b1_決定ボタン_A); // ここ、シンプルに決定ボタンでいいのか？ＵＲＬクリック時は？
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // http://dobon.net/vb/dotnet/control/tbsuppressbeep.html
            // メインメッセージボックスでキー入力したとき、EnterやEscapeキーなどでビープ音が鳴らないようにする
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape
                || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Z || e.KeyCode == Keys.Space || e.KeyCode == Keys.Back || e.KeyCode == Keys.X)

            {
                e.Handled = true; // これでビープ音が鳴らないようにできる
            }
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                EControlType・操作コントロール.c02_MessageBox・メインメッセージボックス, e.KeyCode);
        }

        /// <summary>
        /// メインメッセージボックスにフォーカスが当たった時の処理です．
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            // これをやるとテキストの選択やコピペができない。。
            //// メインメッセージボックスにフォーカスが当たらないようにします．フォームそのものだとだと効果がなかったようです。
            //if (game.getP_gameWindow・ゲーム画面().getP_usedFrom() != null)
            //{
            //    game.getP_gameWindow・ゲーム画面().getP_usedFrom().focusC1_dice();
            //}
        }
        /// <summary>
        /// メインメッセージボックスからフォーカスがなくなった時の処理です。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            //// 1.アンフォーカスの前に選択行を最後にしないと、最初の行に戻ってしまう。
            //MyTools.showRichTextBox_EndLine_UnshowCursor(mainRichTextBox・メインメッセージボックス, this);
            //// メインメッセージボックスにフォーカスが当たらないようにします．フォームそのものだとだと効果がなかったようです。
            //if (game.getP_gameWindow・ゲーム画面().getP_usedFrom() != null)
            //{
            //    focusMainControl・フォーカスをメインコントロールに移す();
            //}
            //これじゃあギザギザになって目が酔うmainRichTextBox・メインテキストボックス.SelectionStart = mainRichTextBox・メインメッセージボックス.Text.Length; // 最後にする。

        }


        /// <summary>
        /// 「入力確定」ボタンクリック時。
        /// ※この処理は「次へ」ボタンと統合したので、今は使っていない。
        /// </summary>
        private void buttonInput確定ボタン_Click(object sender, EventArgs e)
        {
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(
                EControlType・操作コントロール.c0a_GoNext・次へ進むボタン,
                EInputButton・入力ボタン.b1_決定ボタン_A);
        }



        // ■選択ボックスであるlistBox1は、_Clickと_KeyDownイベントは必ず実装＆イベント追加して。
        /// <summary>
        /// 選択ボックスで最後に選択したインデックスを示します。未選択の場合は-1が入っています。
        /// </summary>
        public int p_SelectBox_beforeSelectedIndex・現在の選択肢 = -1;
        /// <summary>
        /// 選択肢クリック時
        /// </summary>
        private void listBox1_Click(object sender, EventArgs e)
        {
            // ○(a)今選択済みの項目をクリックorEnter/Space/Zを押したら選択確定
            // できてない

            // △(b)同じ項目を2回クリックしたら選択確定
            int _nowSelectedIndex = mainSelectBox・選択肢リストボックス.SelectedIndex;
            // ●決定ボタン処理，これを実装しないと，入力確定ができません．必ず実装してください！
            if (_nowSelectedIndex == p_SelectBox_beforeSelectedIndex・現在の選択肢)
            {
                // ●(b)決定ボタン処理
                _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(
                    EControlType・操作コントロール.c03_SelectBox・選択ボックス, EInputButton・入力ボタン.b1_決定ボタン_A);
            }
            p_SelectBox_beforeSelectedIndex・現在の選択肢 = _nowSelectedIndex;
        }
        /// <summary>
        /// 選択肢でキー入力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // ●決定ボタン処理，これを実装しないと，入力確定ができません．必ず実装してください！
            //if (_e.KeyCode == Keys.Enter || _e.KeyCode == Keys.Space || _e.KeyCode == Keys.Z)
            //{
            // _e.BackSpaceの時もあるので、キーをそのまま渡す
                _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(
                    EControlType・操作コントロール.c03_SelectBox・選択ボックス, e.KeyCode);
            //}
        }
        //private void listBox1_KeyPress(object sender, KeyPressEventArgs _e)
        //{
        //    if (_e.KeyChar.ToString() == Keys.Enter.ToString() || _e.KeyChar.ToString() == Keys.Space.ToString())
        //    {
        //        // たぶんこれ，実行されてない・・・KeyPressEventArgs.KeyCharとe.KeyCodeは等しくない．KeyDownにした方が無難．
        //        //game.p_isEndSelectBox・選択ボックス完了フラグ = true;
        //    }
        //}
        /// <summary>
        /// 選択肢の選択項目が変わった時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        // ＝＝＝＝＝＝＝＝＝＝＝以下、他のコントロールのイベント

        private void picScinario_Click(object sender, EventArgs e)
        {

        }

        private void butTestいろいろテスト_Click(object sender, EventArgs e)
        {
            testいろいろテスト();
        }
        /// <summary>
        /// 「いろいろテスト」ボタンを押した時の処理
        /// </summary>
        public void testいろいろテスト()
        {
            bool _isTestButtonボタンテスト = true;
            if (_isTestButtonボタンテスト == true)
            {
                if (p_isTimerTestButton・押したボタンをラベルに表示するテスト中 == false)
                {
                    // タイマースレッドにテスト開始を伝える
                    p_isTimerTestButton・押したボタンをラベルに表示するテスト中 = true;

                    //ここでやることじゃないきがするgame.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = false; // マウスクリックリセット
                    butTestいろいろテスト.Text = "ボタンテスト中…";
                    butTestいろいろテスト.BackColor = Color.Green;
                    //butTestいろいろテスト.Enabled = false; // 連続クリックを防ぐ
                    // 待たないと、このテストボタンをクリックしたときの処理を受け付けてすぐ終了しちゃうので、一定時間待つ。
                    game.waitウェイト(500);

                    //MyTools.ConsoleWriteLine("\n■↓ SPACEキーを押すまで、ボタンテスト開始");
                    //labInfo1.Text = ("■↓ SPACEキーを押すまで、ボタンテスト開始");
                    //// ユーザがEnterかSpaceか、マウスがクリックが押されるまで待つ
                    //int _passedMSec = 0;
                    //int _diceRolateMSec = 1000;
                    //while (game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.SPACE) == false && Program・プログラム.isEnd == false)
                    //{
                    //    game.waitウェイト(_diceRolateMSec);
                    //    _passedMSec += _diceRolateMSec;
                    //}
                    //MyTools.ConsoleWriteLine("\n■↑ SPACEキーが押されたので、ボタンテスト終了\n");
                    //labInfo1.Text = ("■↑ SPACEキーが押されたので、ボタンテスト終了");
                    //butTestいろいろテスト.BackColor = Color.Gray;
                    //butTestいろいろテスト.Enabled = true; // 連続クリックを防ぐ
                    //butTestいろいろテスト.Text = "いろいろテスト";
                    //// テスト終了をタイマースレッドに伝える
                    //p_isTimerTestButton・押したボタンをラベルに表示するテスト中 = false;
                }
                else
                {
                    butTestいろいろテスト.BackColor = Color.Gray;
                    //butTestいろいろテスト.Enabled = true; // 連続クリックを防ぐ
                    butTestいろいろテスト.Text = "いろいろテスト";
                    // テスト終了をタイマースレッドに伝える
                    p_isTimerTestButton・押したボタンをラベルに表示するテスト中 = false;
                }
            }
        }
        // ＝＝＝＝＝＝＝＝＝＝＝＝＝＝他のコントロールのイベント、終わり


        // ＝＝＝＝＝＝＝＝＝＝＝＝＝＝以下、メニューのイベント

        private void 終了ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.End・ゲーム終了処理();
        }

        private void 初めからToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _startDiceBattleGame・ダイスバトルゲーム();
        }

        /*
        private void BGM再生ToolStripMenuItem_Click(object sender, EventArgs _e)
        {
            if (game.s_optionBGM_ON・ＢＧＭを再生する状態か == true)
            {
                BGM再生ToolStripMenuItem.Checked = false;
                game.s_optionBGM_ON・ＢＧＭを再生する状態か = false;
                MyTools.stopSound();
            }
            else
            {
                BGM再生ToolStripMenuItem.Checked = true;
                game.s_optionBGM_ON・ＢＧＭを再生する状態か = true;
                MyTools.stopSound();
                game.pBGM(game.p_nowBGM・現在の再生曲フルパス);
            }
        }
         */


        private void 音楽ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CGameManager・ゲーム管理者.s_optionBGM_ON・ＢＧＭを再生する状態か == true)
            {
                音楽ToolStripMenuItem.Checked = false;
                CGameManager・ゲーム管理者.s_optionBGM_ON・ＢＧＭを再生する状態か = false;
                game.pBGM();
            }
            else
            {
                音楽ToolStripMenuItem.Checked = true;
                CGameManager・ゲーム管理者.s_optionBGM_ON・ＢＧＭを再生する状態か = true;
                game.stopBGM・ＢＧＭを一時停止();
                // フォルダにあるmp3ファイルをランダム再生
                game.pBGMRandom_FromDirectory();
                //game.pBGM(game.p_nowBGM・現在の再生曲フルパス);
            }
        }


        public bool p_isMicRecord = false;
        public int p_MicRecord_StartMSec = 0;
        private void butボイス録音_Click(object sender, EventArgs e)
        {

            if (p_isMicRecord == false)
            {
                if (MySound_Windows.MCI_recordMic_Start() == true) //MySound_Windows.recordMic_Start() == true)
                {
                    game.mメッセージ_瞬時に表示("録音開始...");
                    p_isMicRecord = true;
                    p_MicRecord_StartMSec = MyTools.getNowTime_fast();
                }
                else
                {
                    game.mメッセージ_瞬時に表示("録音失敗（※既に録音中です）。");
                    p_isMicRecord = false;
                }
            }
            else
            {
                string _filename = MySound_Windows.MCI_recordMic_Stop();//MySound_Windows.recordMic_Stop();
                int _recordingMSec = MyTools.getNowTime_fast() - p_MicRecord_StartMSec;
                game.mメッセージ_瞬時に表示("録音終了（すぐに録音したサウンド" + _recordingMSec + "ミリ秒を再生中）");
                p_isMicRecord = false;

                // 再生終了まで他の操作を無効にする
                MyTools.setFormNowLoading_DamyPictureBox(this, true, true);
                // すぐに再生
                MySound_Windows.playSE(_filename, false);
                //MySound_Windows.play_LastRecordedMicSound();
                // 再生終了後、ファイルをクローズしないと上書きできないので、再生時間だけ待って、それからクローズする
                game.waitウェイト(_recordingMSec);
                MySound_Windows.stopSE();
                game.mメッセージ_瞬時に表示("録音したサウンドの再生終了");
                // 再生終了まで他の操作を無効にする
                MyTools.setFormNowLoading_DamyPictureBox(this, false, true);
            }
        }

        private void オプションToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void テスト用音声再生画面を表示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SamplePlayer.Form1 _音楽再生画面フォーム = new SamplePlayer.Form1();
            _音楽再生画面フォーム.Show();
        }

        private void サウンド設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // サウンドコンフィグフォームを表示
            FSoundConfig _soundConfigForm = new FSoundConfig(game);
            _soundConfigForm.Show();
        }

        private void FGameBattleForm1_Activated(object sender, EventArgs e)
        {
            if (game.getP_gameWindow・ゲーム画面() != null)
            {
                game.getP_gameWindow・ゲーム画面().setisFoucused・フォーカスが当たっているかを変更(true);
            }
        }

        private void FGameBattleForm1_Deactivate(object sender, EventArgs e)
        {
            if (game.getP_gameWindow・ゲーム画面() != null)
            {
                game.getP_gameWindow・ゲーム画面().setisFoucused・フォーカスが当たっているかを変更(false);
            }
        }

        private void textInput1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 入力ボックス１でEnterキーを押したら、次の入力ボックスへ移動（Tabキーみたいな操作の自動化）
                _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                    EControlType・操作コントロール.c04_InputBox・入力ボックス１から３のどれか, EInputButton・入力ボタン.b1_決定ボタン_A);
            }
        }

        private void textInput2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 入力ボックス２でEnterキーを押したら、次の入力ボックスへ移動（Tabキーみたいな操作の自動化）
                _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                    EControlType・操作コントロール.c04_InputBox・入力ボックス１から３のどれか, EInputButton・入力ボタン.b1_決定ボタン_A);
            }
        }

        private void textInput3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // 入力ボックス３でEnterキーを押したら、次の入力ボックスへ移動（Tabキーみたいな操作の自動化）
                _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                    EControlType・操作コントロール.c04_InputBox・入力ボックス１から３のどれか, EInputButton・入力ボタン.b1_決定ボタン_A);
            }
        }

        private void butNext次へボタン_KeyDown(object sender, KeyEventArgs e)
        {
            // Enterだけでなく、押しっぱなしに対応
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                EControlType・操作コントロール.c0a_GoNext・次へ進むボタン, e.KeyCode);
        }

        private void butBack戻るボタン_KeyDown(object sender, KeyEventArgs e)
        {
            // Enterだけでなく、押しっぱなしに対応
            _checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理(sender,
                EControlType・操作コントロール.c0b_GoBack・前へ戻るボタン, e.KeyCode);
        }

        private void システムクリエーション設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool _switch = game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か;
            // スイッチを切り替え
            _switch = !_switch;
            // 代入
            game.getP_Battle・戦闘().p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か = _switch;
        }

        private void 戦闘方式＿自動戦闘高速スキップ用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setBattleAuto・自動戦闘モードにする();
        }

        private void 戦闘方式＿スロット形式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setBattleSlot・スロット戦闘モードにする(false, false);
        }

        private void 戦闘方式＿スロットを自分で決定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setBattleSlot・スロット戦闘モードにする(true, false);
        }
        
        private void 戦闘方式＿コマンド選択式通常ＲＰＧ風ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setBattleCommandSelect・コマンド選択戦闘モードにする();
        }

        private void 戦闘方式＿間合い取り方式モンスターファーム風ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        // 以下、ゲーム難易度
        private void cosmosユーザに合わせて自動調節ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度._LVNone_Cosmos・ユーザのプレイ状況に合わせて自動調節);
        }

        private void eden楽園ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度.LV00_Eden・まさに楽園);
        }

        private void 難易度天国ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度.LV01_VeryEasy・とてもやさしい);
        }

        private void 難易度優しいToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度.LV02_Easy・やさしい);
        }

        private void 難易度標準ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度.LV03_Normal・普通);
        }

        private void 難易度難しいToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度.LV04_Hard・むずかしい);
        }

        private void 難易度試練ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度.LV05_VeryHard・とてもむずかしい);
        }

        private void 難易度神々クラスToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度.LV06_God・神々クラス);
        }

        private void 難易度宇宙時代ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV・難易度を変更(EGameLV・難易度.LV10_Kaos・まさにカオス);
        }

        private void テスト用ダメージ計算機設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.showBattleDamageCalcForm・ダメージ計算機画面を表示();
        }

        private void テスト用バランス調整ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.showBattleForm・バランス調整画面を表示();
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            MyTools.showPictureBoxImage_ByDragAndDrop_Part1(sender, e);
        }
        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            Image _image = null;
            string _filename_FullPath = MyTools.showPictureBoxImage_ByDragAndDrop_Part2(sender, e, out _image, true);
            // キャラの画像があれば、キャラ画像として保存
            if (_image != null)
            {
                CChara・キャラ _playerChara = MyTools.getListValue<CChara・キャラ>(game.getP_Battle・戦闘().p_charaPlayer・味方キャラ, game.getP_Battle・戦闘().p_charaPlayer_Index・味方キャラ_主人公ID);
                if (_playerChara != null)
                {
                    setCharaImage・キャラのサムネイメージを設定(_image, _filename_FullPath, _playerChara);
                }
            }
        }

        public static void setCharaImage・キャラのサムネイメージを設定(Image _image, string _filename_FullPath, CChara・キャラ _chara)
        {
            _chara.setVar・変数を変更(EVar.サムネ画像, _image);
            _chara.setVar・変数を変更(EVar.ファイルフルパス_サムネ画像, _filename_FullPath);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MyTools.MessageBoxShow("ファイルをドラッグアンドドロップして、好きな画像ファイルを設定できます。");
        }


        private void pictureBox2_DragEnter(object sender, DragEventArgs e)
        {
            MyTools.showPictureBoxImage_ByDragAndDrop_Part1(sender, e);
        }
        private void pictureBox2_DragDrop(object sender, DragEventArgs e)
        {
            Image _image = null;
            string _filename_FullPath = MyTools.showPictureBoxImage_ByDragAndDrop_Part2(sender, e, out _image, true);
            // キャラの画像があれば、キャラ画像として保存
            // （敵キャラの方は保存するかはまだ考えていない）
            if (_image != null)
            {
                CChara・キャラ _enemyChara = MyTools.getListValue<CChara・キャラ>(game.getP_Battle・戦闘().p_charaEnemy・敵キャラ, game.getP_Battle・戦闘().p_charaEnemy_Index・敵キャラ_リーダーID);
                if (_enemyChara != null) _enemyChara.setVar・変数を変更(EVar.サムネ画像, _image);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MyTools.MessageBoxShow("ファイルをドラッグアンドドロップして、好きな画像ファイルを設定できます。");
        }






        // ＝＝＝＝＝＝＝＝＝＝＝＝＝＝メニューのイベント、終わり


        // AxWindowsMediaPlayer1を追加したら、変なエラーが出るので削除。
        ///// <summary>
        ///// AxWindowsMediaPlayerコントロールを追加（） http://dobon.net/vb/dotnet/programing/playmidifile.html#wmp
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="_e"></param>
        //private void axWindowsMediaPlayer1_Enter(object sender, EventArgs _e)
        //{

        //}
        //public void playSound_ByWindowsMediaPlayer1(string _fileName_FullPath_or_NotFullPath)
        //{
        //    //URLプロパティが指定されたら自動的に再生されるようにする
        //    axWindowsMediaPlayer1.settings.autoStart = true;
        //    //オーディオファイルを指定する（自動的に再生される）
        //    axWindowsMediaPlayer1.URL = _fileName_FullPath_or_NotFullPath;

        //    //autoStartがfalseのときは、次のようにして再生する
        //    //axWindowsMediaPlayer1.Ctlcontrols.play();
        //}




        /* 以下，なくていい（親クラスがグループでも，Formにキーイベントが飛ぶ！）
        /// <summary>
        /// 味方キャラのダイスコマンドでキー入力　
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void listBox2_KeyDown(object sender, KeyEventArgs _e)
        {
            //this.FGameTestForm1_KeyDown(sender, _e);
        }
         * */





        

 
                   
    }
}