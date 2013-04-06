using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Yanesdk.Input;

namespace PublicDomain
{
    #region EBattleMode・戦闘方式
    /// <summary>
    /// 戦闘方式を定義した列挙体です。
    /// </summary>
    public enum EBattleMode・戦闘方式{
        _default・ダイスバトルモード,
        e01_command1・通常ＲＧＭ風コマンドバトルモード,
        e02_realtime1・モンスターファーム風バトルモード,
    }
    #endregion
    #region 表示コマンドの種類: EShowCommandType
    /// <summary>
    /// キャラが持つコマンド（戦闘開始用コマンド、攻撃ダイス、防御ダイスなど）の種類を定義した列挙体です。
    /// </summary>
    public enum EShowCommandType・表示コマンド
    {
        _none・非表示,
        c01_First・戦闘開始用コマンド＿たたかうやにげる等,
        c02_Target・対象選択,
        c03a_DiceAtack・攻撃ダイス,
        c03b_DiceDiffence・防御ダイス,
        c03c_SlotOther・自由記述スロット,
        //c04_Skill・特技,//普通の特技は、攻撃や防御スロットに含める？
        /// <summary>
        /// 必殺技、ようすをみる、交渉、仲間に誘う（スカウト）、考える、叫ぶ、憑依、浄化、、ぬすむ、など特殊なコマンドを実装したい時
        /// </summary>
        c04_Skill・特技,
        c05_Item・アイテム,
        /// <summary>
        /// バーが出て、ユーザがタイミング良く押す
        /// </summary>
        ct1_TimingBar・タイミングバー,
        /// <summary>
        /// サークルが出て、ユーザがタイミング良く押す
        /// </summary>
        ct2_TimingBar・タイミングサークル,
    }
    #endregion
    #region コマンド受け渡しデータ: CCommandData
    /// <summary>
    /// 行動毎に生成される，行動関連のメソッド間受け渡しデータです．
    /// ※代入処理の管理しやすさ向上のため，値の代入には出来ればsetではなくaddを使ってください．
    /// </summary>
    public class CCommandData・コマンド受け渡しデータ
    {
        public string p0_コマンド名 = "";
        public List<List<string>> p1_特別メッセージ = new List<List<string>>();
        public List<EBattleActionType・行動タイプ> p2_行動タイプ = new List<EBattleActionType・行動タイプ>();
        /// <summary>
        /// 1マスのダイスコマンドで，それぞれのアクション回数毎に攻撃する対象（1人～全員のキャラIDを攻撃順に格納）です．
        /// （※全体攻撃を数回行うコマンドなどを実装するために実装）
        /// </summary>
        public List<List<int>> p3_行動対象キャラID群 = new List<List<int>>();
        //public EBattleActionObject・攻撃対象 t行動対象;
        public List<double> p4_ダメージ数 = new List<double>();
        public List<double> p5_クリティカル率 = new List<double>();
        public List<double> p6_回避率 = new List<double>();
        public List<double> p7_防御軽減ダメージ数 = new List<double>();
        //public double getP7_防御軽減ダメージ数(int _何回目のアクションか) { return MyTools.getListValue(p7_防御軽減ダメージ数, _何回目のアクションか); }

        public CCommandData・コマンド受け渡しデータ(string _コマンド名)
        {
            p0_コマンド名 = _コマンド名;
        }

        /// <summary>
        /// このコマンドに新しく効果を追加します．追加しないものには""かnew クラス名()を入れてください．特別メッセージはランダム性無しのバージョンです。
        /// </summary>
        public void add(string _特別メッセージ, EBattleActionType・行動タイプ _行動タイプ, List<int> _行動対象キャラ郡, double _ダメージ数, double _クリティカル率, double _回避率, double _防御軽減ダメージ数)
        {
            // 特別メッセージはランダム性無し
            List<string> _actionMessage = new List<string>();
            _actionMessage.Add(_特別メッセージ);
            add(_actionMessage, _行動タイプ, _行動対象キャラ郡, _ダメージ数, _クリティカル率, _回避率, _防御軽減ダメージ数);
        }
        /// <summary>
        /// このコマンドに新しく効果を追加します．追加しないものには""かnew クラス名()を入れてください．特別メッセージにランダム性を持たせるためにリストで渡すバージョンです。
        /// </summary>
        public void add(List<string> _特別メッセージ, EBattleActionType・行動タイプ _行動タイプ, List<int> _行動対象キャラ郡, double _ダメージ数, double _クリティカル率, double _回避率, double _防御軽減ダメージ数)
        {
            p1_特別メッセージ.Add(_特別メッセージ); 
            p2_行動タイプ.Add(_行動タイプ); 
            p3_行動対象キャラID群.Add(_行動対象キャラ郡); 
            p4_ダメージ数.Add(_ダメージ数); 
            p5_クリティカル率.Add(_クリティカル率);
            p6_回避率.Add(_回避率);
            p7_防御軽減ダメージ数.Add(_防御軽減ダメージ数);
        }
    }
    #endregion

    /// <summary>
    /// 様々なバージョン（ダイスバトル、コマンドバトル、タイミングバトル）の戦闘を管理する，戦闘クラスです．
    /// </summary>
    public class CBattle・戦闘 : CBattleBase・戦闘を管理する基底クラス
    {
        #region 戦闘のシステムパラメータ
        public static int s_evenJudgeTurn・判定に持ち込むターン数 = 15; // （攻撃力が低い時など戦闘が終わらないので）判定に持ち込むターン。
        // リアルタイムか、ダイスバトルか
        /// <summary>
        /// trueだと行動ゲージ付きの戦闘、falseだと素早さが早いキャラからコマンド選択戦闘（ダイスバトル）
        /// </summary>
        public static bool s_is戦闘システム_trueボタンタイミングリアルタイム戦闘_falseダイスバトルターン戦闘 = false; 
        // クリティカル
        public static bool s_isクリティカル時のプラス魔法力は防御無視 = true;
        // 復活ターンシステム
        public static bool s_isコマンド復活ターン数を設けるシステム_true同じコマンドが頻繁に出ないようにするか = true;
            #region 上記システムの派生システム
            public bool s_isコマンド復活ターンは常に減少させるシステム_true指定ターン使わなければ連続使用可能にするか_false全マス使いきらないと復活しないか = true;
        public bool s_isコマンド復活ターンは一斉復活システム___全マス使い切ったら一斉復活するか = true;
            public int s_commandNoUsedTurnNum・復活ターン数 = 4; // 2以下だとすぐ回復。3だと同じコマンドは１ターンだけ使えない。6だと「何もできない…」行動不能ターンが0。7だと1ターンだけ行動不能。
            #endregion

        // 状態不能の一定確率など
        public static double s_parrize1_NoActRate・マヒ行動不能確率 = 0.5;

        /// <summary>
        /// ダイスが回転する時間を、無入力でも待つ最大時間。この時間を超えたら自動的にダイスがストップする（保険用）
        /// </summary>
        public static int p_dicePlayerEndMSec・味方ダイスストップ最大待ちミリ秒 = 3000000; // 5分
        public static int p_diceEnemyEndMSec・敵ダイスストップミリ秒 = 300;
        public static int p_diceRolateMSec・ダイス回転ミリ秒 = 50; //20-50-100位が妥当か
        public static int p_UserCommandSelectCheckedMSec・コマンド選択を確認する単位ミリ秒 = 1000;
        public static int p_decidedDiceNum1・現在の味方のダイス番号 = -1;
        public static int p_decidedDiceNum2・現在の敵のダイス番号 = -1;

        #endregion
        
        /// <summary>
        /// 現在ダイスがストップしているか
        /// </summary>
        public bool p_isDiceStop・ダイスストップ = true;
        public bool p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か = true; // falseだと選べる（特技用やテスト用）、true=>回転したスロットをボタンを押して止める。
        public bool p_isRolateRandom・スロットはtrueランダム＿false降順 = false; // 降順の方がユーザが止めようと言う意欲がでるかも。


        private EBattleMode・戦闘方式 p_battleMode = EBattleMode・戦闘方式._default・ダイスバトルモード;
        public EBattleMode・戦闘方式 getp_battleMode・現在の戦闘モードを取得(){ return p_battleMode; }
        /// <summary>
        /// 現在の戦闘モードを変更します
        /// </summary>
        public EBattleMode・戦闘方式 setp_battleMode・戦闘モードを変更(EBattleMode・戦闘方式 _battleMode) { EBattleMode・戦闘方式 _before = p_battleMode; p_battleMode = _battleMode; return _before; }

        private EShowCommandType・表示コマンド p_nowShowCommand = EShowCommandType・表示コマンド._none・非表示;
        /// <summary>
        /// 現在の戦闘画面に表示されているコマンド（ユーザが選択可能なリスト）の種類を取得します。
        /// </summary>
        public EShowCommandType・表示コマンド getP_nowShowCommand() { return p_nowShowCommand; }
        /// <summary>
        /// 戦闘に表示するコマンド（ユーザが選択可能なリスト）を変更します。
        /// 
        /// 画面のコマンドリストを、引数２のキャラが持つ（デフォルトでは味方キャラ[0]）、引数１の形式のコマンドに変更します。
        /// ※キャラが持つコマンドを使う場合は、第３引数はnullでＯＫです。特定のコマンドを指定した場合は、第三引数に指定してください。キャラが持つコマンドよりも優先して表示されます。
        /// </summary>
        public EShowCommandType・表示コマンド showCommand・表示コマンドを変更(EShowCommandType・表示コマンド _shownCommand)
        {
            return showCommand・表示コマンドを変更(_shownCommand, MyTools.getListValue<CChara・キャラ>(p_charaPlayer・味方キャラ, 0), null);
        }
        /// <summary>
        /// 戦闘に表示するコマンド（ユーザが選択可能なリスト）を変更します。
        /// 
        /// 画面のコマンドリストを、引数２のキャラが持つ、引数１の形式のコマンドに変更します。
        /// ※キャラが持つコマンドを使う場合は、第３引数はnullでＯＫです。特定のコマンドを指定した場合は、第三引数に指定してください。キャラが持つコマンドよりも優先して表示されます。
        /// </summary>
        public EShowCommandType・表示コマンド showCommand・表示コマンドを変更(EShowCommandType・表示コマンド _shownCommand, CChara・キャラ _shownChara, List<string> _shownCommandList・キャラより優先される＿必要なければnullでＯＫ)
        {
            EShowCommandType・表示コマンド _before = game.getP_gameWindow・ゲーム画面().getP_usedFrom()._showBattleCommandList・コマンドリストを変更(_shownCommand, _shownChara, _shownCommandList・キャラより優先される＿必要なければnullでＯＫ);
            //これはスレッドで常時呼びだれているのでする必要は無い drawUpdate・戦闘描画更新処理();
            return _before;
        }

        /// <summary>
        ///  これらは，よく使うメソッドを簡単に呼び出すために作られたプロパティ群です．必ず行動中キャラが変わった時はsetActCharaで変更してください．でないと，値が更新されない場合があります．
        /// 
        /// （…やっぱり，ショートカットメソッド（kou行動中攻撃対象キャラid(){return p_行動中キャラ.konotaこのターンの攻撃対象id();}）などを作って適時生データに参照するほうが賢い？）
        /// 
        /// （●●いや…やはり使わないかもしれないショートカットメソッドをたくさん置くよりも，それだったらせっかく使いやすくしてるCChara・キャラクラスの日本語メソッドを直接呼ぼう！）
        /// </summary>
        public CChara・キャラ p_行動中キャラ;
        public string p_行動中キャラ名 = "";
        public int p_行動中キャラid = 0;
        /// <summary>
        ///  これらは，よく使うメソッドを簡単に呼び出すために作られたプロパティ群です．必ず行動中キャラが変わった時はsetActCharaで変更してください．でないと，値が更新されない場合があります．
        /// </summary>
        public void setActChara・現在行動中キャラを設定(CChara・キャラ _キャラ, int _キャラid)
        {
            p_行動中キャラ = _キャラ;
            p_行動中キャラ名 = _キャラ.name名前();
            p_行動中キャラid = _キャラid;
            //p_行動中キャラの攻撃対象キャラid = MyTools.parseInt(_chara.konotaこのターンの攻撃対象id());

        }
        #region ショートカットメソッド（今は使っていない．●●CChara・キャラクラスの日本語メソッドを使おう！）
        /*
        /// <summary>
        ///  行動中のキャラが狙っている敵一体です．コマンドの攻撃対象が全体攻撃であっても，一人だけを指します．
        /// </summary>
        public int get攻撃対象キャラid()
        {
            return p_行動中キャラ.konotaこのターンの攻撃対象id();
        }
        /// <summary>
        ///  行動中のキャラが回復しようとしている味方一体です．コマンドの回復対象が全体であっても，一人だけを指します．
        /// </summary>
        public int get回復対象キャラid()
        {
            return p_行動中キャラ.konotaこのターンの回復対象id();
        }
        /// <summary>
        ///  行動中のキャラが補助しようとしている味方一体です．コマンドの補助対象が全体であっても，一人だけを指します．
        /// </summary>
        public int get補助対象キャラid()
        {
            return p_行動中キャラ.konotaこのターンの補助対象id();
        }
         * */
        #endregion


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_g"></param>
        public CBattle・戦闘(CGameManager・ゲーム管理者 _g)
            : base(_g)
        {
        }

        /// <summary>
        /// 戦闘を開始します．参加するキャラを選択します（戦闘中も変更可能です．）// [TODO]CGameManager・ゲーム管理者 _globalDataはどこに引数を入れればよい？
        /// </summary>
        override public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _g, List<CChara・キャラ> _charaPlayer, List<CChara・キャラ> _charaEnemy, List<CChara・キャラ> _charaOther)
        {
            
            //List<CChara・キャラ> _yobi_charaPlayer = new List<CChara・キャラ>();
            //_yobi_charaPlayer.AddRange(_charaPlayer);
            //p_charaPlayer・味方キャラ.Clear();

            // ダイス戦闘独自の戦闘システムを創るので，親メソッドを呼ばない！　base.startBattle・戦闘開始(_g, _charaPlayer, _charaEnemy, _charaOther);
            // 戦闘パラメータの初期化（親メソッド）
            initBattleParameter();

            game = _g;

            // [Question]何故か，p_charaPlayer・味方キャラ.Clear();，とすると，引数の_charaPlayerまで消えてしまうので，初期化前に退避．
            // [WARNING]p_charaPlayer・味方キャラ = _charaPlayer;　だと参照渡しなので，戦闘でp_chara.Clear()とすると，元の引数の_charaPlayerまで消えてしまうので注意！
            p_charaPlayer・味方キャラ.AddRange(_charaPlayer);
            p_charaEnemy・敵キャラ.AddRange(_charaEnemy);
            p_charaOther・その他キャラ.AddRange(_charaOther);
            p_charaAll・全キャラ.AddRange(_charaPlayer);
            p_charaAll・全キャラ.AddRange(_charaEnemy);
            p_charaAll・全キャラ.AddRange(_charaOther);

            // ●●●ダイスコマンドを作成！
            foreach (CChara・キャラ _c in p_charaAll・全キャラ)
            {
                CCharaCreator・キャラ生成機.createDiceCommand_FromParas・ダイスコマンドを自動生成(_c);
                // キャラの一時パラを消去
                _c.setVar・変数を変更(EVar.このターンの回避率, 0.0);
                _c.setVar・変数を変更(EVar.このターンの物理防御ダメージ軽減数, 0.0);
            }


            viewBattleWindow・戦闘画面表示();
            initializeChara・キャラ戦闘開始状態初期化();

            drawInitial・戦闘画面初期化処理();
            drawUpdate・戦闘描画更新処理();

            // 戦闘を開始します．
            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "■■■■■■■■■ダイス戦闘開始！■■■■■■■■■■");
            
            // 音楽を再生
            //
            game.mメッセージ_自動送り("");
            foreach (CChara・キャラ _enemy in p_charaEnemy・敵キャラ)
            {
                game.mメッセージ_自動送り("【c】"+_enemy.name名前() + "があらわれた！");
                _enemy.setVar・変数を変更(EVar.この戦闘では敵キャラ, CVarValue・変数値._YES);
                _enemy.setVar・変数を変更(EVar.この戦闘では味方キャラ, CVarValue・変数値._NO);
            }
            foreach(CChara・キャラ _player in p_charaPlayer・味方キャラ){
                _player.setVar・変数を変更(EVar.この戦闘では敵キャラ, CVarValue・変数値._NO);
                _player.setVar・変数を変更(EVar.この戦闘では味方キャラ, CVarValue・変数値._YES);
            }

            game.waitウェイト(1000);

            // 戦闘ターン処理
            doDiceBattleCommand();

        }
        #region 引数が異なるメソッド
        /// 戦闘を開始します（仲介キャラ無し）．参加するキャラを選択します（戦闘中も変更可能です．）
        /// </summary>
        override public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, List<CChara・キャラ> _charaPlayer, List<CChara・キャラ> _charaEnemy)
        {
            List<CChara・キャラ> _noChara = new List<CChara・キャラ>();
            startBattle・戦闘開始(_gameData, _charaPlayer, _charaEnemy, _noChara);
        }

        /// <summary>
        /// 戦闘を開始します（味方キャラ一人）．参加するキャラを選択します（戦闘中も変更可能です．）
        /// </summary>
        override public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, CChara・キャラ _charaPlayer, List<CChara・キャラ> _charaEnemy)
        {
            List<CChara・キャラ> _onePayer = new List<CChara・キャラ>();
            _onePayer.Add(_charaPlayer);
            startBattle・戦闘開始(_gameData, _onePayer, _charaEnemy);
        }
        /// <summary>
        /// 戦闘を開始します（敵キャラ一人）．参加するキャラを選択します（戦闘中も変更可能です．）
        /// </summary>
        override public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, List<CChara・キャラ> _charaPlayer, CChara・キャラ _charaEnemy)
        {
            List<CChara・キャラ> _oneEmeny = new List<CChara・キャラ>();
            _oneEmeny.Add(_charaEnemy);
            startBattle・戦闘開始(_gameData, _charaPlayer, _oneEmeny);
        }
        /// <summary>
        /// 戦闘を開始します（味方キャラも敵キャラも一人）．参加するキャラを選択します（戦闘中も変更可能です．）
        /// </summary>
        override public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, CChara・キャラ _charaPlayer, CChara・キャラ _charaEnemy)
        {
            List<CChara・キャラ> _onePlayer = new List<CChara・キャラ>();
            _onePlayer.Add(_charaPlayer);
            List<CChara・キャラ> _oneEnemy = new List<CChara・キャラ>();
            _oneEnemy.Add(_charaEnemy);
            startBattle・戦闘開始(_gameData, _onePlayer, _oneEnemy);
        }
        #endregion
        /// <summary>
        /// ●●ターン戦闘時のメインループです。
        /// </summary>
        private void doDiceBattleCommand()
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
                Program・実行ファイル管理者.printlnLog(ELogType.lgui1_ログGUIText戦闘用, "/_/_/_/_/_/_/_/_/_/3:第 " + p_turn・ターン + " ターン開始_/_/_/_/_/_/_/_/_/_");
                setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f03_ターン進行中);
                command3・戦闘ターン開始();
                while (p_nowFase・フェーズ != EBattleFase・戦闘フェーズ.f06_ターン終了中)
                {
                    if (s_is戦闘システム_trueボタンタイミングリアルタイム戦闘_falseダイスバトルターン戦闘 == true)
                    {
                        _戦闘メインスレッドb_ボタンを押してリアルタイム行動();
                    }
                    else
                    {
                        _戦闘用メインスレッドa_素早さの早いキャラから自動行動();
                    }
                }
                Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "/_/_/_/_/_/_/_/_/_/6:第 " + p_turn・ターン + " ターン終了_/_/_/_/_/_/_/_/_/_");
                setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f06_ターン終了中);
                command6・戦闘ターン終了();
                p_turn・ターン++;
            }
        }
        /// <summary>
        /// コマンド選択方式用の戦闘システムのターン管理スレッドです。
        /// </summary>
        private void _戦闘用メインスレッドa_素早さの早いキャラから自動行動()
        {
            // 素早さリストの作成
            List<int> _素早いキャラid順 = game.getParaSortedCharaIDList・パラメータ順のキャラIDリストを取得(p_charaAll・全キャラ, EPara.a4_素早さ, ESortType・並び替え順.降順);
            //MyTools.getSortList(p_charaAll・全キャラ, MyTools.SortType.AscendingOrder_syouzyun);
            // 素早いキャラから順番に行動
            while (_素早いキャラid順.Count > 0)
            {
                command4D・ダイス行動(_素早いキャラid順[0]);
                _素早いキャラid順.RemoveAt(0); // 終わったキャラはリストから消していく
            }
            // 描画処理
            drawUpdate・戦闘描画更新処理();
            // 1モーメント（時間単位，1FPS？）待ち
            game.waitFウェイトフレーム(1);

            // ■全キャラの行動が終わったらすぐにターン終了
            p_nowFase・フェーズ = EBattleFase・戦闘フェーズ.f06_ターン終了中;
        }
        /// <summary>
        /// リアルタイムバトル用の戦闘システムのターン管理スレッドです。
        /// </summary>
        private void _戦闘メインスレッドb_ボタンを押してリアルタイム行動()
        {
            // ボタン操作，コマンド入力受付
            EBattleFase・戦闘フェーズ _nextFase = EBattleFase・戦闘フェーズ.f03_ターン進行中;

            int _restAPターン残り = 100;
            while(_restAPターン残り > 0){

                _nextFase = setNextFase_FromDiceBattleActions・アクションから次のフェーズを設定();
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
                _restAPターン残り--;
                // 描画処理
                drawUpdate・戦闘描画更新処理();
                // 最大1フレーム待ち
                game.w画面停止フレーム(1);
            }

            // ■ターン終了
            p_nowFase・フェーズ = EBattleFase・戦闘フェーズ.f06_ターン終了中;
        }
        /// <summary>
        /// ■■リアルタイムバトル時に、瞬間的な入力を受け付けて、そのボタンの種類により次の行動を判定する処理です。
        /// </summary>
        /// <returns></returns>
        public EBattleFase・戦闘フェーズ setNextFase_FromDiceBattleActions・アクションから次のフェーズを設定()
        {
            EBattleFase・戦闘フェーズ _nextFase = EBattleFase・戦闘フェーズ.f03_ターン進行中;
            // 【暫定】入力キー（ゲームパッドボタン対応表）と行動。矢印キーの周りで
            // Enter/Z          キー    （○＝Ａボタン）：  攻撃／決定
            // Fn/BackSpace     キー    （×＝Ｂボタン）：  回避／キャンセル
            // Shift            キー    （△＝Ｘボタン）：  必殺／メニュー／ジャンプ
            // Ctrl             キー    （□＝Ｙボタン）：  防御／便利ボタン／整頓
            CMouseAndKeyBoardKeyInput・キー入力定義 _key = game.getP_keyboardInput();
            if (_key.IsPush(EKeyCode.z))
            {
                _nextFase = EBattleFase・戦闘フェーズ.f04_行動中;
            }
            return _nextFase;
        }
        /// <summary>
        /// ダイスバトル時の行動継続の処理は，ここに書いてください．
        /// </summary>
        override protected void command4・戦闘ターン行動継続()
        {
            // 親クラスのメソッドを実行
            base.command4・戦闘ターン行動継続();

            // 処理はここから
        }
        /// <summary>
        /// ダイスバトル時の行動割り込み時の処理は，ここに書いてください．
        /// </summary>
        override protected void command5・戦闘行動割り込み()
        {
            // 親クラスのメソッドを実行
            base.command5・戦闘行動割り込み();

            // 処理はここから
        }

        #region ■■■ターン開始・終了処理

        /// <summary>
        /// ●ターン開始時毎回更新する処理は，ここに書いてください．
        /// </summary>
        override protected void command3・戦闘ターン開始()
        {
            // 親クラスのメソッドを実行
            base.command3・戦闘ターン開始();

            // このターンのパラメータ初期値の代入
            int _全キャラでの攻撃対象キャラＩＤ = 0;
            int _healCharaID = 0;
            int _supportCharaID = 0;
            int _ownID = p_charaAll・全キャラ.IndexOf(p_行動中キャラ);
            int _相手パーティでの攻撃対象ID = -1; // メソッド内部で決める時は-1
            CChara・キャラ _攻撃対象キャラ = null;
            foreach(CChara・キャラ _キャラ in p_charaAll・全キャラ){
                // 0.テストで作戦を代入。
                if (_キャラ.isVarYES(EVar.この戦闘では味方キャラ) == true && _キャラ.Var(EVar.このターンの作戦) == CVarValue・変数値._none_未定義)
                {
                    //_キャラ.setVar・変数を変更(EVar.このターンの作戦, CVarValue・変数値.sakusen01_めいれいさせてね);
                    _キャラ.setVar・変数を変更(EVar.このターンの作戦, CVarValue・変数値.sakusen03_ぼちぼちがんばって);
                }

                // 1.これ以上戦う必要がなくなったら（基本は敵・味方どちらかが全滅したら、もしくは中断や和解）、
                //   その時点で他のキャラの行動はスキップして、戦闘終了
                if (isKOAllEnemys・敵全滅() || isKOAllPlayers・味方全滅())
                {
                    break; // 戦闘ターン開始を終了して、戦闘ターン終了へ
                }

                // 2.1.攻撃対象を決める
                _相手パーティでの攻撃対象ID = -1; // メソッド内部で決める時は-1
                if (_キャラ.isVarYES(EVar.この戦闘では味方キャラ) == true)
                {
                    _攻撃対象キャラ = setAttackTarget・攻撃対象を決定(_キャラ, false, p_charaEnemy・敵キャラ, ref _相手パーティでの攻撃対象ID);
                }
                else if (_キャラ.isVarYES(EVar.この戦闘では敵キャラ) == true)
                {
                    // (a)ちゃんと思考する場合
                    _攻撃対象キャラ = setAttackTarget・攻撃対象を決定(_キャラ, true, p_charaPlayer・味方キャラ, ref _相手パーティでの攻撃対象ID);
                    // (b)完全ランダムでやる場合（これだけだと、ＨＰ０以下のキャラも攻撃するので気を付けて）
                    //_相手パーティでの攻撃対象ID = game.getRandom・ランダム値を生成(0, p_charaPlayer・味方キャラ.Count - 1);
                    //_攻撃対象キャラ = MyTools.getListValue(p_charaPlayer・味方キャラ, _相手パーティでの攻撃対象ID);
                }
                else
                {
                    int _randomNum = MyTools.getRandomNum(0, p_charaAll・全キャラ.Count - 1);
                    _攻撃対象キャラ = p_charaAll・全キャラ[_randomNum];
                }
                // _全キャラでの攻撃対象キャラＩＤ は，全キャラでの攻撃対象キャラＩＤ
                _全キャラでの攻撃対象キャラＩＤ = p_charaAll・全キャラ.IndexOf(_攻撃対象キャラ);
                _キャラ.setVar・変数を変更(EVar.このターンの攻撃対象id, _全キャラでの攻撃対象キャラＩＤ.ToString());
                _キャラ.setVar・変数を変更(EVar.このターンの攻撃対象キャラ, p_charaAll・全キャラ[_全キャラでの攻撃対象キャラＩＤ]); //名前だけ格納する場合: p_charaAll・全キャラ[_全キャラでの攻撃対象キャラＩＤ].name名前());
                // 2.2.回復対象は今は自分
                _healCharaID = _ownID;
                _キャラ.setVar・変数を変更(EVar.このターンの回復対象id, _healCharaID.ToString());
                // 2.3.補助対象は今は自分
                _supportCharaID = _ownID;
                _キャラ.setVar・変数を変更(EVar.このターンの補助対象id, _supportCharaID.ToString());
                game.DEBUGデバッグ一行出力(ELogType.lgui1_ログGUIText戦闘用, _キャラ.name名前() + "のダイス復活ターン数 (");

                // 3.ダイスコマンド復活ターン数を減らす
                #region ダイスコマンド復活ターン処理
                int i = 0;
                foreach (CDiceCommand・ダイスコマンド _dice in _キャラ.getP_dice・所有ダイス())
                {
                    if (s_isコマンド復活ターンは常に減少させるシステム_true指定ターン使わなければ連続使用可能にするか_false全マス使いきらないと復活しないか == true)
                    {
                        _dice.p_ReUseTurn・復活ターン数--; // いつでも-1する
                    }
                    else
                    {
                        if (_dice.p_ReUseTurn・復活ターン数 > 0)
                        {
                            _dice.p_ReUseTurn・復活ターン数--; // 1以上なら-1するが、0になると-1しなくなる
                        }
                    }

                    // 復活ターン数が0以下になったら適時復活
                    if (_dice.p_ReUseTurn・復活ターン数 <= 0)
                    {
                        _dice.p_isNowUse・現在使用可能 = true; // 使用可能に
                    }
                    else
                    {
                        _dice.p_isNowUse・現在使用可能 = false; // 使用不可能に
                    }
                    

                    if (i != 0)
                    {
                        //game.DEBUGデバッグ一行出力(ELogType.lgui1_ログGUIText戦闘用, ", ");
                    }
                    //game.DEBUGデバッグ一行出力(ELogType.lgui1_ログGUIText戦闘用, _dice.getp_name()+":"+_dice.p_ReUseTurn・復活ターン数);
                    i++;
                }
                //game.DEBUGデバッグ一行出力(ELogType.lgui1_ログGUIText戦闘用, ")\n");
                #endregion
            }
            // ●画面描画
            drawUpdate・戦闘描画更新処理();
        }
        #region 攻撃対象決定処理
        private CChara・キャラ setAttackTarget・攻撃対象を決定(CChara・キャラ _c行動キャラ, bool _isAutoSelect・true自動決定か＿falseユーザが選択するか, List<CChara・キャラ> _攻撃対象キャラグループ, ref int _攻撃対象パーティＩＤ＿メソッド内で決めるならマイナス１＿外部から決めるなら直接代入も可能)
        {
            CChara・キャラ _攻撃対象キャラ = null;
            if (_isAutoSelect・true自動決定か＿falseユーザが選択するか == false)
            {
                // 一人しかいなければ、選択をスキップ
                if (_攻撃対象キャラグループ.Count != 1)
                {
                    List<string> _対象名リスト = new List<string>();
                    foreach (CChara・キャラ _chara in _攻撃対象キャラグループ)
                    {
                        // 戦闘不能だったら、攻撃選択から外す（隠れる（攻撃対象にされない）みたいな意味のキャラも外す）
                        // HPが0未満より戦闘不能かで判断した方がいいかも。HP0以下になっても（気絶であって戦闘不能ではない）も根性で蘇るキャラいるし、死んだ敵をオーバーキルをしまくる残忍な戦い方もあってもいい…か？
                        if (isKO・戦闘不能(_chara) == false && _chara.isVarYES(EVar.このターンは攻撃対象にされない) == false)
                        {
                            _対象名リスト.Add(_chara.name名前());
                        }
                    }
                    showCommand・表示コマンドを変更(EShowCommandType・表示コマンド.c02_Target・対象選択, _c行動キャラ, _対象名リスト);
                    //int _ミリ秒数 = p_UserCommandSelectCheckedMSec・コマンド選択を確認する単位ミリ秒;
                    // 待つ
                    game.wIa特定入力完了待ち();
                    int _selectedIndex・リスト配列 = -1;
                    if (_c行動キャラ.isVarYES(EVar.この戦闘では味方キャラ) == true)
                    {
                        _selectedIndex・リスト配列 = game.getP_gameWindow・ゲーム画面().getP_usedFrom().getC1_dice_Selected();
                    }
                    else
                    {
                        _selectedIndex・リスト配列 = game.getP_gameWindow・ゲーム画面().getP_usedFrom().getC2_dice_Selected();
                    }
                    _攻撃対象パーティＩＤ＿メソッド内で決めるならマイナス１＿外部から決めるなら直接代入も可能 = _selectedIndex・リスト配列;
                }
            }
            else
            {
                // 自動で決めるなら、ランダム
                // [TODO]生来的には、ランダムではなく、狙われやすさで評価した方がいいかも。
                if (_攻撃対象パーティＩＤ＿メソッド内で決めるならマイナス１＿外部から決めるなら直接代入も可能 == -1)
                {
                    _攻撃対象パーティＩＤ＿メソッド内で決めるならマイナス１＿外部から決めるなら直接代入も可能
                        = game.getRandom・ランダム値を生成(0, _攻撃対象キャラグループ.Count - 1);
                }
            }
            // 攻撃対象キャラの決定
            _攻撃対象キャラ = MyTools.getListValue(_攻撃対象キャラグループ, _攻撃対象パーティＩＤ＿メソッド内で決めるならマイナス１＿外部から決めるなら直接代入も可能);
            // 現状の問題は、nullになることがあること。どうする？
            if (_攻撃対象キャラ == null)
            {
                // 敵キャラグループの最初
                _攻撃対象キャラ = MyTools.getListValue<CChara・キャラ>(_攻撃対象キャラグループ, 0);
                // それでもnullなら、_攻撃対象キャラグループがnullか_攻撃対象キャラグループ.Count=0なんだから、もうnullを返すしかないでしょう。
            }
            return _攻撃対象キャラ;
        }
        /// <summary>
        /// キャラがこのターンに行動できるかを返します。
        /// マヒは特定％の確率、金縛りは１００％、戦闘不能な場合もtrueを返します。
        /// （厳密にはHPマイナスでも根性で蘇ることがあるので、HPが0未満だったらは含まれない）
        /// </summary>
        private bool isAct・行動不能(CChara・キャラ _chara)
        {
            // （厳密にはHPマイナスでも根性で蘇ることがあるので、HPが0になったらではない）
            bool _isKO = _chara.Para(EPara.戦闘不能ターン数) > 0 || 
                (_chara.Para(EPara.マヒターン数) > 0 && MyTools.getRandomNum(1, 100) <= s_parrize1_NoActRate・マヒ行動不能確率 ) || 
                _chara.Para(EPara.金縛りターン数) > 0;
            return _isKO;
        }
        /// <summary>
        /// _chara.Para(EPara.戦闘不能ターン数) > 0 を返します。
        /// （厳密にはHPマイナスでも根性で蘇ることがあるので、HPが0未満だったらは含まれない）
        /// </summary>
        private bool isKO・戦闘不能(CChara・キャラ _chara)
        {
            bool _isKO = _chara.Para(EPara.戦闘不能ターン数) > 0;
            //_isKO = (_chara.Para(EPara.s03_HP) <= 0);　// 後で修正
            return _isKO;
        }
        #endregion

        /// <summary>
        /// ●ターン終了時に毎回更新する処理は，ここに書いてください．
        /// </summary>
        override protected void command6・戦闘ターン終了()
        {
            base.command6・戦闘ターン終了();

            bool _is戦闘終了 = true;

            // (a)どちらかのキャラたちが全員戦闘不能になったら（厳密にはHPマイナスでも根性で蘇ることがあるので、HPが0になったらではない）
            // 戦闘終了
            bool _is味方全滅 = isKOAllPlayers・味方全滅();
            bool _is敵全滅 = isKOAllEnemys・敵全滅();
            if (_is味方全滅 == false && _is敵全滅 == true)
            {
                // 勝利☆
                set戦闘結果(戦闘結果.勝利);
            }
            else if (_is味方全滅 == true && _is敵全滅 == false)
            {
                // 敗北★
                set戦闘結果(戦闘結果.敗北);
            }
            else if (_is味方全滅 == true && _is敵全滅 == true)
            {
                // 引き分け！
                set戦闘結果(戦闘結果.引き分け);
            }
            else
            {
                // 戦闘続行。まだ戦闘を続ける
                _is戦闘終了 = false;
            }

            // (b)和解？
            // (c)逃走？

            // (d)判定処理。指定ターンで、判定に持ち込み（攻撃力低いと戦闘が終わらないため）
            if (p_turn・ターン >= s_evenJudgeTurn・判定に持ち込むターン数)
            {
                _is戦闘終了 = true;

                game.mメッセージ_自動送り(""+p_turn・ターン +"ターン経過したため、勝負は判定へ…",ESPeed.s01_超遅い＿標準で５秒);
                game.mメッセージ_自動送り("",ESPeed.s09_超早い＿標準で１００ミリ秒);
                game.mメッセージ_自動送り("―――――判定―――――", ESPeed.s09_超早い＿標準で１００ミリ秒);
                // (d-1)どっちのHPが多いか（残りＨＰ率）
                double _味方残りHP率 = getHPRate_charasAve・キャラたちの残りＨＰ率を取得(p_charaPlayer・味方キャラ);
                double _敵残りHP率 = getHPRate_charasAve・キャラたちの残りＨＰ率を取得(p_charaEnemy・敵キャラ);
                game.mメッセージ単語_末尾改行なし_瞬時に表示("残りＨＰ：　", ESPeed.s05_普通＿標準で１秒);
                game.mメッセージ単語_末尾改行なし_瞬時に表示(MyTools.getStringNumber(_味方残りHP率,true,3,0) + "％" ,ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                game.mメッセージ単語_末尾改行なし_瞬時に表示(_味方残りHP率 + "　ｖｓ　" ,ESPeed.s02_非常に遅い＿標準で３秒);
                game.mメッセージ単語_末尾改行なし_瞬時に表示(MyTools.getStringNumber(_敵残りHP率,true,3,0) + "％", ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                game.mメッセージ_自動送り("――――――――――――", ESPeed.s01_超遅い＿標準で５秒);
                if (_味方残りHP率 > _敵残りHP率)
                {
                    // 勝利☆
                    set戦闘結果(戦闘結果.勝利);
                }
                else if (_味方残りHP率 < _敵残りHP率)
                {
                    // 敗北★
                    set戦闘結果(戦闘結果.敗北);
                }
                else
                {
                    // 引き分け！
                    set戦闘結果(戦闘結果.引き分け);
                }
            }

            if (_is戦闘終了 == true)
            {
                // ●戦闘終了！
                setP_Fase・フェーズを設定(EBattleFase・戦闘フェーズ.f10_戦闘終了中);
            }
        }

        private bool isKOAllEnemys・敵全滅()
        {
            bool _is敵全滅 = true;
            foreach (CChara・キャラ _c in p_charaEnemy・敵キャラ)
            {
                if (isKO・戦闘不能(_c) == false)
                {
                    // 一人でも生き残っていたらfalse
                    _is敵全滅 = false;
                    break;
                }
            }
            return _is敵全滅;
        }
        private bool isKOAllPlayers・味方全滅()
        {
            bool _is味方全滅 = true;
            foreach (CChara・キャラ _c in p_charaPlayer・味方キャラ)
            {
                if (isKO・戦闘不能(_c) == false)
                {
                    // 一人でも生き残っていたらfalse
                    _is味方全滅 = false;
                    break;
                }
            }
            return _is味方全滅;
        }

        public double getHPRate_charasAve・キャラたちの残りＨＰ率を取得(List<CChara・キャラ> _charasList)
        {
            double _HP率総数 = 0.0;
            double _残りHP率 = 0.0;
            List<CChara・キャラ> _charas = _charasList;
            int _数 = 1; // 0割を防ぐため1にしてる
            for (int i = 0; i < _charas.Count; i++)
            {
                if (MyTools.getListValue(_charas, i) != null)
                {
                    _HP率総数 += _charas[i].Para(EPara.s03_HP) / _charas[i].Para(EPara.s03b_最大HP);
                    if (i > 0)
                    {
                        _数++; // 二人目以降なら＋１
                    }
                }
            }
            _残りHP率 = _HP率総数 / _数;
            return _残りHP率;
        }
        #endregion


        #region ■■■ダイス行動の処理　（ダイス回転エフェクトなどを含む）
        /// <summary>
        /// キャラから、味方キャラパーティＩＤか敵キャラパーティＩＤを取って来ます。
        /// </summary>
        public int getPartyID(bool _isPlayerChara_trueなら味方キャラ_falseなら敵キャラ, CChara・キャラ _キャラ)
        {
            int _partyID = -1;
            if (_isPlayerChara_trueなら味方キャラ_falseなら敵キャラ == true)
            {
                _partyID = p_charaPlayer・味方キャラ.IndexOf(_キャラ);
            }
            else
            {
                _partyID = p_charaEnemy・敵キャラ.IndexOf(_キャラ);
            }
            return _partyID;
        }
        /// <summary>
        /// 全キャラp_charaAllのＩＤから、味方キャラパーティＩＤか敵キャラパーティＩＤを取って来ます。
        /// </summary>
        public int getPartyID(bool _isPlayerChara_trueなら味方キャラ_falseなら敵キャラ, int _キャラid_全キャラp_charaAllのＩＤ)
        {
            CChara・キャラ _chara = MyTools.getListValue<CChara・キャラ>(p_charaAll・全キャラ, _キャラid_全キャラp_charaAllのＩＤ);
            return getPartyID(_isPlayerChara_trueなら味方キャラ_falseなら敵キャラ, _chara);
        }
        /// <summary>
        /// ダイスバトルでの、ターンごとの１キャラの基本行動を記述するメソッドです。
        /// 行動出来た場合はtrue、行動出来なかった場合はfalseを返します。
        /// </summary>
        /// <param name="_キャラid"></param>
        protected bool command4D・ダイス行動(int _キャラid)
        {
            CChara・キャラ _chara = p_charaAll・全キャラ[_キャラid];
            string _キャラ名 = _chara.name名前();
            bool _is味方キャラ = _chara.Var_Bool(EVar.この戦闘では味方キャラ);

            // 【グローバル変数set】p_行動中キャラを設定
            setActChara・現在行動中キャラを設定(_chara, _キャラid);

            // 戦闘不能ターン数（HPが0未満でも最後のあがきで行動する時はある）だったり，状態異常になっていたら行動できない
            bool _isAct・行動できるか = true;
            if (isAct・行動不能(_chara))
            {
                _isAct・行動できるか = false;
                return _isAct・行動できるか;
            }

            // ここから，キャラの行動開始！
            game.mメッセージ_自動送り("【c】――――第" + p_turn・ターン + "ターン――――", ESPeed.s00_デフォルト_待ち時間なし);

            // ■1.戦闘に使用するダイスコマンドの中から，どれか一つを選んで行動
            // 選択可能なダイス番号だけを取りだす
            List<CDiceCommand・ダイスコマンド> _戦闘使用可能ダイスコマンド = new List<CDiceCommand・ダイスコマンド>();
            List<int> _selectableDiceIndexs・使用可能ダイス = new List<int>();
            List<CDiceCommand・ダイスコマンド> _dices = _chara.getP_dice・所有ダイス();
            for (int i = 0; i <= _dices.Count - 1; i++)
            {
                if (_dices[i].p_isNowUse・現在使用可能 == true)
                {
                    _戦闘使用可能ダイスコマンド.Add(_dices[i]);
                    _selectableDiceIndexs・使用可能ダイス.Add(i);
                }
            }
            if (_戦闘使用可能ダイスコマンド.Count == 0)
            {
                // 行動するダイスがないので，何もできない．
                //"は何もできない…。", 
                string _randomString = MyTools.getRandomString("は構え直した…。", "は体勢を立て直している…。","は何もできないっ…","は様子をみている…");
                game.mメッセージ_自動送り("" + _キャラ名 + _randomString);
                if (s_isコマンド復活ターンは一斉復活システム___全マス使い切ったら一斉復活するか == true)
                {
                    foreach (CDiceCommand・ダイスコマンド _dice in _chara.getP_dice・所有ダイス())
                    {
                        // 全てのダイスマスを復活（復活ターン数がマイナスのものはそのまま、変更しないから、使っても復活しやすい）
                        _dice.p_isNowUse・現在使用可能 = true;
                        if (_dice.p_ReUseTurn・復活ターン数 > 0)
                        {
                            _dice.p_ReUseTurn・復活ターン数 = 0;
                        }
                    }

                }
            }
            else
            {
                //1.1.ダイスコマンド決定処理
                #region ダイスコマンド決定処理
                
                // ダイスコマンドを今回行動するキャラのものに変更（なんか二番目のパーティキャラのしか表示されない、へん）
                //game.showCharaDiceBattleStatus・キャラのダイスバトルステータスを表示(
                //    _is味方キャラ, _chara, getPartyID(_is味方キャラ, _キャラid), true, false);

                // ダイスコマンドにフォーカスを当てる
                game.getP_gameWindow・ゲーム画面().getP_usedFrom().focusC1_dice();
                int _decidedDiceNum = MyTools.getRandomValue(_selectableDiceIndexs・使用可能ダイス);
                // ●ダイスコマンドを自動回転してランダムに決定するか、ユーザに自由に選ばせるか
                if (p_is自分や味方のダイスコマンドはtrueスロット回転か_falseストップさせて自由選択か == true)
                {
                    // (a)自動回転してランダムに決定
                    string _バリエーション = MyTools.getRandomString("はどうする？", "、コマンド？", "の行動は…", "は何をする？", "のターン。");
                    game.mメッセージ_自動送り("" + _キャラ名 + _バリエーション +"　（Enter/Spaceで行動を決定）", ESPeed.s09_超早い＿標準で１００ミリ秒); //　の行動！
                    p_isDiceStop・ダイスストップ = false;
                    // [TODO]グラフィックで，ダイスが回転し，止まる処理
                    waitDiceRolating・ダイス自動回転からユーザがストップするまでの処理(_selectableDiceIndexs・使用可能ダイス);
                }
                else
                {
                    // (b)ユーザに自由にコマンドを選ばせる
                    if (_chara.Var_Bool(EVar.この戦闘では味方キャラ) == true)
                    {
                        // ユーザがダイスを決定するまで待つ
                        string _バリエーション = MyTools.getRandomString("はどうする？", "、コマンド？", "の行動は…", "は何をする？", "のターン。");
                        game.mメッセージ_自動送り("" + _キャラ名 + _バリエーション +"　（「十字キー＋Enter/Space」で行動を決定）", ESPeed.s09_超早い＿標準で１００ミリ秒); //　の行動！
                        bool _isSelectedユーザが使用可能なコマンドを選んだ = false;
                        while (_isSelectedユーザが使用可能なコマンドを選んだ == false)
                        {
                            waitUserSelectDice・ユーザが静止したダイスを自由に選択するまで待つ処理(_selectableDiceIndexs・使用可能ダイス);

                            // 使用可能なダイスではなかった場合、もう一度選び直し
                            int _selectedDiceIndex選んだコマンドの配列 = 
                                game.getP_gameWindow・ゲーム画面().getP_usedFrom().getC1_dice_Selected();
                            // _selectedDiceNum選んだダイスの配列は0～、行動を選択しなかった場合は-1
                            foreach (int _diceIndex in _selectableDiceIndexs・使用可能ダイス)
                            {
                                if (_selectedDiceIndex選んだコマンドの配列 == _diceIndex)
                                {
                                    _isSelectedユーザが使用可能なコマンドを選んだ = true;
                                    break;
                                }
                            }
                            if (_isSelectedユーザが使用可能なコマンドを選んだ == false)
                            {
                                if (_selectedDiceIndex選んだコマンドの配列 == -1)
                                {
                                    game.mメッセージ_自動送り("行動を選択していないぞ！　コマンド？", ESPeed.s09_超早い＿標準で１００ミリ秒);
                                }
                                else // まだ復活していないダイスコマンド
                                {
                                    game.mメッセージ_自動送り("その行動はまだ復活に時間がかかるようだ…（ターン経過で選択可）", ESPeed.s09_超早い＿標準で１００ミリ秒);
                                }
                            }
                        }
                    }
                    else
                    {
                        // 敵の場合はランダム決定
                        string _バリエーション = MyTools.getRandomString("の行動…","はどう出るか…","は何をしてくる…","は静かに構えつつ…","２，３歩前に出ながら…","は激しく前に出て…","は後ろに下がりながら…","は一歩下がりつつ…","はそっと動き…","の行動は…");
                        game.mメッセージ_自動送り("" + _キャラ名 + _バリエーション, ESPeed.s09_超早い＿標準で１００ミリ秒); //　の行動！
                        p_isDiceStop・ダイスストップ = false;
                        waitDiceRolating・ダイス自動回転からユーザがストップするまでの処理(_selectableDiceIndexs・使用可能ダイス);
                    }
                }

                // 変数p_decidedDiceNum1～2のダイス番号の決定
                if (_chara.Var_Bool(EVar.この戦闘では味方キャラ) == true)
                {
                    // 味方の場合：　決定ボタンを押した直後に止まっていたダイス番号
                    p_decidedDiceNum1・現在の味方のダイス番号 = 
                        game.getP_gameWindow・ゲーム画面().getP_usedFrom().getC1_dice_Selected();
                    if (p_decidedDiceNum1・現在の味方のダイス番号 == -1)
                    {
                        p_decidedDiceNum1・現在の味方のダイス番号 = MyTools.getRandomValue(_selectableDiceIndexs・使用可能ダイス);
                    }
                    _decidedDiceNum = p_decidedDiceNum1・現在の味方のダイス番号;
                }
                else
                {
                    // 敵の場合：　ランダムにダイス番号を決定
                    p_decidedDiceNum2・現在の敵のダイス番号 =
                        MyTools.getRandomValue(_selectableDiceIndexs・使用可能ダイス);
                        //game.getRandom・ランダム値を生成(0, Math.Max(0, _戦闘使用可能ダイスコマンド.Count - 1));
                    // 決定したダイスを選択
                    game.getP_gameWindow・ゲーム画面().getP_usedFrom()
                        .setC2_dice_Selected(p_decidedDiceNum2・現在の敵のダイス番号, true);
                    _decidedDiceNum = p_decidedDiceNum2・現在の敵のダイス番号;
                }
                p_isDiceStop・ダイスストップ = true;
                #endregion

                // ●_diceが，今から取る行動
                CDiceCommand・ダイスコマンド _diceC = p_行動中キャラ.getP_dice・所有ダイス()[_decidedDiceNum];
                Program・実行ファイル管理者.printlnLog(ELogType.lgui1_ログGUIText戦闘用, "★★★ " + _キャラ名 + " が " + _diceC.getp_Text・詳細() + "で 行動 ★★★");

                // ■このダイスコマンドを指定ターン使えないようにする（オプションで変更可）
                if (s_isコマンド復活ターン数を設けるシステム_true同じコマンドが頻繁に出ないようにするか == true)
                {
                    _diceC.p_isNowUse・現在使用可能 = false;
                    _diceC.p_ReUseTurn・復活ターン数 += s_commandNoUsedTurnNum・復活ターン数;
                    // ●描画更新
                    drawUpdate・戦闘描画更新処理();
                }

                game.mメッセージ_自動送り(_キャラ名 + "の" + _diceC.getp_name() + "！", ESPeed.s06_やや早い＿標準で８００ミリ秒);

                // ●●●行動毎関連データの作成
                CCommandData・コマンド受け渡しデータ _commandData = new CCommandData・コマンド受け渡しデータ(_diceC.getp_name());
                // このダイスコマンドを構成するアクションの数だけ，複数回のアクションを追加
                foreach(CBattleAction・戦闘行動 _action in _diceC.p_actionList・ダイスアクション群){
                    _commandData.add(_action.getP1_特別メッセージ(), _action.getP2_行動タイプ(),
                        setTargetCharas・行動対象キャラ群を決定(_action.getP2_行動タイプ(), _action.getP3_行動対象()),
                        _action.getP4_ダメージ数(), _action.getP5_クリティカル率(), _action.getP6_回避率(), _action.getP7_防御軽減ダメージ数());
                }
                // ●●行動ダイスコマンドの各アクションに従って処理！
                command4_2_Dice・ダイスのアクション毎の処理(_commandData);

                // 最後に「キャラ名の○○！」行動表示の待ち時間（メッセージを見やすくするため）
                game.waitウェイト(500);
            }
            return _isAct・行動できるか;
        }
        /// <summary>
        /// ダイス自動回転開始～ユーザが止めるまでの処理です。
        /// 敵の場合はすぐ止まる様に内部で分けて書いているので、敵の場合もこれを呼び出すだけでＯＫです。
        /// </summary>
        private void waitDiceRolating・ダイス自動回転からユーザがストップするまでの処理(List<int> _selectableDiceNum・使用可能ダイス)
        {
            // [MEMO]スレッドだと、Formのコントロールを操作できないので、以下は却下。
            //Thread _ダイス回転処理スレッド = MyTools.threadSubMethod(waitDiceRolating・ダイス自動回転からユーザがストップするまでの処理);

            bool _is味方 = MyTools.getBool(p_行動中キャラ.Var(EVar.この戦闘では味方キャラ));
            int _passedMSec = 0;
            int _diceRolateMSec = p_diceRolateMSec・ダイス回転ミリ秒;
            int _endMSec・敵のダイスストップミリ秒 = 
                (int)(p_diceEnemyEndMSec・敵ダイスストップミリ秒 * CGameManager・ゲーム管理者.s_ゲーム描画速度増減倍率);
            int _beforeDiceIndex・前回のダイスインデックス = -1;

            // フラグの初期化
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
            // ユーザの入力を待つ
            while (game.isEndUserInput・ユーザの入力が完了したか() == false
                && game.ibボタンを押したか_連射非対応() == false
                //&& game.getP_mouseInput().IsPush(EMouseButton.Left) == false
                //&& game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.ENTER) == false 
                //&& game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.SPACE) == false 
                && Program・実行ファイル管理者.isEnd == false)
            {
                _beforeDiceIndex・前回のダイスインデックス
                    = edダイス回転エフェクト(_is味方, _selectableDiceNum・使用可能ダイス, p_isRolateRandom・スロットはtrueランダム＿false降順,
                    _beforeDiceIndex・前回のダイスインデックス);
                game.waitウェイト(_diceRolateMSec);
                _passedMSec += _diceRolateMSec;

                if (_is味方 == false)
                {
                    // 敵の場合は、ユーザが決定ボタンを押さなくても、敵のストップミリ秒経過したら、すぐ止める
                    if (_passedMSec >= _endMSec・敵のダイスストップミリ秒)
                    {
                        break;
                    }
                }
                else
                {
                    // ストップミリ秒以上経過したら、止める
                    if (_passedMSec >= p_dicePlayerEndMSec・味方ダイスストップ最大待ちミリ秒)
                    {
                        break;
                    }
                }
            }
            // フラグの初期化
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);

            // スレッド実装用
            //while (p_isDiceStop・ダイスストップ == false)
            //{
            //    edダイス回転エフェクト(_is味方, _selectableDiceIndexs・使用可能ダイス);
            //    game.waitウェイト(0.1);
            //    game.se効果音(ESE・効果音.dice01・ダイス回転音_コロ);
            //}
        }
        /// <summary>
        ///  回転後のダイス番号を返します。
        ///  _beforeDiceNo・降順用に使う前回のダイス番号は、なければ-1を指定してください。
        /// </summary>
        private int edダイス回転エフェクト(bool _is味方, List<int> _selectableDiceNo, bool _isRolateRandom・スロット回転方式はtrueランダム＿false降順, int _beforeDiceNo・降順用に使う前回のダイス番号)
        {
            // 音を出さなくするとめちゃくちゃ早くなる…やっぱりMCIが重いのか。。。
            //game.se効果音(ESE・効果音.dice01・ダイス回転音_コロ);

            int _rolatedDiceNo = 0;
            if (_isRolateRandom・スロット回転方式はtrueランダム＿false降順 == true)
            {
                // ランダムの場合、選択可能なダイス番号からランダム
                _rolatedDiceNo = MyTools.getRandomValue(_selectableDiceNo);
            }
            else
            {
                // 降順の場合、次のダイス番号を設定(-1の場合も0になる)
                _rolatedDiceNo = _beforeDiceNo・降順用に使う前回のダイス番号 + 1;
                if (_rolatedDiceNo < 0) _rolatedDiceNo = 0; // マイナスはなし
                int _MaxDiceNo = MyTools.getMaxValue(_selectableDiceNo);
                while (_selectableDiceNo.Contains(_rolatedDiceNo) == false)
                {
                    _rolatedDiceNo++;
                    // 選択可能なダイス番号を超えたら、最初に戻す
                    if (_rolatedDiceNo > _MaxDiceNo) { _rolatedDiceNo = 0; }
                }
            }

            if (_is味方 == true)
            {
                game.getP_gameWindow・ゲーム画面().getP_usedFrom().setC1_dice_Selected(_rolatedDiceNo, true);
            }
            else
            {
                game.getP_gameWindow・ゲーム画面().getP_usedFrom().setC2_dice_Selected(_rolatedDiceNo, true);
            }
            return _rolatedDiceNo;
        }

        private void waitUserSelectDice・ユーザが静止したダイスを自由に選択するまで待つ処理(List<int> _selectableDiceNum・使用可能ダイス)
        {
            bool _is味方 = MyTools.getBool(p_行動中キャラ.Var(EVar.この戦闘では味方キャラ));
            //int _passedMSec = 0;
            int _diceSelectingMSec = p_UserCommandSelectCheckedMSec・コマンド選択を確認する単位ミリ秒;
            int _endMSec・敵のダイス回転処理時間 = (int)(p_diceEnemyEndMSec・敵ダイスストップミリ秒 * CGameManager・ゲーム管理者.s_ゲーム描画速度増減倍率);

            game.getP_gameWindow・ゲーム画面().getP_usedFrom().focusC1_dice(); // フォーカスをダイスに
            // フラグの初期化
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
            // ユーザの入力を待つ
            while (game.isEndUserInput・ユーザの入力が完了したか() == false
                && game.ibボタンを押したか_連射非対応(EInputButton・入力ボタン.b1_決定ボタン_A) == false
                //&& game.getP_mouseInput().IsPush(EMouseButton.Left) == false
                //&& game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.ENTER) == false 
                //&& game.ik指定キーを押し中か_押しっぱ連射対応(EKeyCode.SPACE) == false 
                && Program・実行ファイル管理者.isEnd == false)
            {
                game.waitウェイト(CGameManager・ゲーム管理者.s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒);
            }
        }


        protected List<int> setTargetCharas・行動対象キャラ群を決定(EBattleActionType・行動タイプ _行動タイプ, EBattleActionObject・攻撃対象 _行動対象)
        {

            // この行動の攻撃対象を決定
            List<int> _行動対象キャラ群 = new List<int>();
            int _randomNum = 0;
            switch (_行動対象)
            {
                case EBattleActionObject・攻撃対象.t01_敵単:
                    _行動対象キャラ群.Add(MyTools.parseInt(p_行動中キャラ.Var(EVar.このターンの攻撃対象id)));
                    break;
                case EBattleActionObject・攻撃対象.t04_味単:
                    if (_行動タイプ == EBattleActionType・行動タイプ.t01b_ＨＰ回復)
                    {
                        _行動対象キャラ群.Add(MyTools.parseInt(p_行動中キャラ.Var(EVar.このターンの回復対象id)));
                    }
                    else if (_行動タイプ == EBattleActionType・行動タイプ.t01_ＨＰダメ)
                    {
                        _行動対象キャラ群.Add(MyTools.parseInt(p_行動中キャラ.Var(EVar.このターンの攻撃対象id)));
                    }
                    break;
                case EBattleActionObject・攻撃対象.t03_自分:
                    _行動対象キャラ群.Add(p_行動中キャラid);
                    break;
                case EBattleActionObject・攻撃対象.t02_敵全:
                    foreach (CChara・キャラ _chara in p_charaEnemy・敵キャラ)
                    {
                        _行動対象キャラ群.Add(p_charaAll・全キャラ.IndexOf(_chara));

                        // テスト
                        string _test = "";
                        foreach (int _c in _行動対象キャラ群)
                        {
                            _test += _c + ",";
                        }
                        game.DEBUGデバッグ一行出力(ELogType.l4_重要なデバッグ, "敵キャラ全体のキャラIDが取得できているか確認: " + _test);

                    }
                    break;
                case EBattleActionObject・攻撃対象.t05_味全:
                    foreach (CChara・キャラ _chara in p_charaPlayer・味方キャラ)
                    {
                        _行動対象キャラ群.Add(p_charaAll・全キャラ.IndexOf(_chara));
                    }
                    break;
                case EBattleActionObject・攻撃対象.t06_不明:
                    _randomNum = game.getRandom・ランダム値を生成(0, p_charaAll・全キャラ.Count);
                    _行動対象キャラ群.Add(_randomNum);
                    break;
                case EBattleActionObject・攻撃対象.t06b_敵ラ:
                    _randomNum = game.getRandom・ランダム値を生成(0, p_charaAll・全キャラ.Count);
                    _行動対象キャラ群.Add(_randomNum);
                    break;
                case EBattleActionObject・攻撃対象.t06c_味ラ:
                    _randomNum = game.getRandom・ランダム値を生成(0, p_charaAll・全キャラ.Count);
                    _行動対象キャラ群.Add(_randomNum);
                    break;
                default:
                    break;
            }
            return _行動対象キャラ群;

        }

        protected void command4_2_Dice・ダイスのアクション毎の処理(CCommandData・コマンド受け渡しデータ _a)
        {
            int _何回目 = 0;
            // 1つの行動に含まれる，効果の回数だけ繰り返し
            for (_何回目 = 0; _何回目 < _a.p2_行動タイプ.Count; _何回目++)
            {
                EBattleActionType・行動タイプ _行動タイプ = MyTools.getListValue(_a.p2_行動タイプ, _何回目);


                // 攻撃か，回復か，などのタイプ分け
                switch (_行動タイプ)
                {
                    case EBattleActionType・行動タイプ.t01_ＨＰダメ:
                        act_attack・攻撃処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t01b_ＨＰ回復:
                        act_heal・回復処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t02_精神ダメ:
                        act_attack・攻撃処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t02b_精神回復:
                        act_heal・回復処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t03_ミス:
                        act_miss・ミス処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t04_防御:
                        act_gard・防御処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t05_回避:
                        act_avoid・回避処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t11_補助その他:
                        act_support・補助処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t07_その他:
                        act_other・その他処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t11a_全パラ増:
                        act_damageAdd・攻撃ダメージ増減乗除処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t11b_全パラ倍:
                        act_damageAdd・攻撃ダメージ増減乗除処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t12a_回復増減:
                        act_healAdd・回復量増減乗除処理(_a, _何回目);
                        break;
                    case EBattleActionType・行動タイプ.t12b_回復乗除:
                        act_healAdd・回復量増減乗除処理(_a, _何回目);
                        break;
                    default:
                        Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "command4_2D: ダイス戦闘行動が省略されました．ダイスの行動タイプに，予期しない値が入っているため．");
                        break;
                }
            }
        }

        protected void act_attack・攻撃処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            // 攻撃しているキャラが、味方か、敵か
            bool _is味方 = true;
            string _会心or痛恨 = "会心";
            string _奇跡or悲劇 = "奇跡";
            if (p_行動中キャラ.Var_Bool(EVar.この戦闘では味方キャラ) == false)
            {
                _is味方 = false;
                _会心or痛恨 = "痛恨";
                _奇跡or悲劇 = "悲劇";
            }

            // ■攻撃力計算
            double _攻撃力の元 = _a.p4_ダメージ数[_何回目];
            double _攻撃力補正値 = p_行動中キャラ.Para(EPara.物理攻撃補正増減) + _攻撃力の元 * p_行動中キャラ.Para(EPara.物理攻撃補正乗除);
            double _攻撃力通常 = _攻撃力の元 + _攻撃力補正値;
            double _攻撃力最終 = _攻撃力通常;
            // ■魔法力計算
            double _魔法力の元 = p_行動中キャラ.Para(EPara.s08_魔法力);
            double _魔法力最終 = _魔法力の元;


            foreach (int _被攻撃キャラid in _a.p3_行動対象キャラID群[_何回目])
            {
                CChara・キャラ _被攻撃キャラ = p_charaAll・全キャラ[_被攻撃キャラid];
                string _被攻撃キャラ名 = _被攻撃キャラ.name名前();

                // ■相手能力計算
                double _相手HP = _被攻撃キャラ.Para(EPara.s03_HP);
                double _相手防御力 = _被攻撃キャラ.Var_Double(EVar.このターンの物理防御ダメージ軽減数);
                double _相手を倒せる攻撃力 = _相手HP + _相手防御力;
                double _相手回避率 = _被攻撃キャラ.Var_Double(EVar.このターンの回避率);
                double _相手ガード率 = _被攻撃キャラ.Para(EPara.s13b_ガード率);
                double _相手根性率 = _被攻撃キャラ.Para(EPara.s14_根性発動率);
                double _相手根性オーバーダメージ = _被攻撃キャラ.Para(EPara.s14b_根性発動最大オーバーダメージ);

                #region ●～の攻撃！の後のランダムメッセージ（基本攻撃力から変化した時だけ，その強さによって変わる）
                double _基本攻撃力からの倍率 = _a.p4_ダメージ数[_何回目] / (p_行動中キャラ.Para(EPara.s07_攻撃力) * CCharaCreator・キャラ生成機.s1_attaDevN_1マスの基本攻撃力倍率);
                string _randomString = "";
                if (_基本攻撃力からの倍率 == 1.0)
                {
                    _randomString = "";
                }
                else if (_基本攻撃力からの倍率 <= 0.5)
                {
                    _randomString = "は思うように体が動かない！";
                }
                else if (_基本攻撃力からの倍率 <= 0.7)
                {
                    _randomString = "の攻撃は少し狙いが外れた！";
                }
                else if (_基本攻撃力からの倍率 <= 1.0)
                {
                    _randomString = "は慎重に攻めた！";
                }
                else if (_基本攻撃力からの倍率 <= 1.1)
                {
                    _randomString = "の狙いを定めた！"; // "の集中した一撃！";
                }
                else if (_基本攻撃力からの倍率 <= 1.2)
                {
                    _randomString = "の鋭い一撃！"; // "は鋭く斬りつけた！";
                }
                else if (_基本攻撃力からの倍率 <= 1.3)
                {
                    _randomString = "の凄まじい猛攻！";
                }
                else if (_基本攻撃力からの倍率 <= 1.4)
                {
                    _randomString = "の重くのしかかる一撃！";
                }
                else if (_基本攻撃力からの倍率 <= 1.5)
                {
                    _randomString = "の威力の高まった一撃！";
                }

                if (_基本攻撃力からの倍率 == 1.0)
                {
                    //やめると一行空白がなくなるg.mメッセージ単語_末尾改行なし_瞬時に表示(_randomString + "\n", ESPeed.s09_超早い＿標準で１００ミリ秒);
                }
                else
                {
                    game.mメッセージ単語_末尾改行なし_瞬時に表示(p_行動中キャラ名 + _randomString + "　", ESPeed.s06_やや早い＿標準で８００ミリ秒);
                }
                // 効果音 味方：ピリリッ，　敵：ブルルッ
                if (_is味方 == true)
                {
                    game.ea味方通常攻撃エフェクト(p_行動中キャラid, _被攻撃キャラid);
                }
                else
                {
                    game.ea敵通常攻撃エフェクト(p_行動中キャラid, _被攻撃キャラid);
                }
                #endregion

                #region // ●集中判定 // 集中すると、集中増減率によりクリティカル率などが大幅に増減
                double _集中率 = p_行動中キャラ.Para(EPara.s17_集中率);
                bool _is集中した = false;
                bool _isトドメに近い = false;
                string _hugouSyutyu = " > ";
                double _集中増減率 = 0.0;
                double _randomNumSyuu = MyTools.getRandomNum(1, 100);
                if (_randomNumSyuu <= _集中率)
                {
                    // 基本攻撃力より強い攻撃の時だけ、集中する
                    if (_基本攻撃力からの倍率 > 1.0)
                    {
                        // 集中増減率により、クリティカル率などが大幅に増減
                        _集中増減率 = _集中率 * (_基本攻撃力からの倍率 - 1.0);
                        _randomString = MyTools.getRandomString("は力を集中させた！！", "は全神経を集中させた！！", "は全身全霊を込めた！！", "はド派手に決めた！！");
                        game.mメッセージ単語_末尾改行なし_瞬時に表示("\n" + p_行動中キャラ名 + _randomString + "　", ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                        _hugouSyutyu = " <=";
                        _is集中した = true;

                        // 会心を出せば相手を倒せる場合、さらに集中する。
                        if (_相手を倒せる攻撃力 > _攻撃力通常 && _相手を倒せる攻撃力 <= _攻撃力通常 * s_会心攻撃力上昇倍率1_5)
                        {
                            _isトドメに近い = true;
                        }
                        if (_isトドメに近い == true)
                        {
                            _集中増減率 += p_行動中キャラ.Para(EPara.s17_集中率) * 0.2;
                        }
                    }
                    else
                    {
                        _hugouSyutyu = " 威力低:" + MyTools.getStringNumber(_基本攻撃力からの倍率, false, 2, 1) + " 集中力:";
                    }

                }
                #endregion

                // ■その他の確率計算
                double _クリ率 = Math.Min(90.0, _a.p5_クリティカル率[_何回目]);
                double _会心率 = Math.Min(90.0, s_会心率標準10_0);
                double _奇跡率 = Math.Min(50.0, s_奇跡率標準1_0);
                // 集中すると、クリティカル率などが大幅に増加
                double _クリ率増減率 = _集中増減率;
                double _会心率増減率 = _集中増減率 * 0.5;
                double _奇跡率増減率 = _集中増減率 * 0.1;

                double _命中率 = Math.Min(190.0, p_行動中キャラ.Para(EPara.s13_命中率) + _集中増減率 * 0.5);
                // 10％～190％、同じの時100％（命中190％＝回避90％）
                double _命中率マイナス相手回避10to190_同じ時100 = _命中率 - _相手回避率;

                #region // ●会心・痛恨の一撃判定
                bool _is会心した = false;
                string _hugouKaishin = " > ";
                double _randomNumKaishin = MyTools.getRandomNum(1, 100);
                if (_randomNumKaishin <= _会心率 + _会心率増減率)
                {
                    // 効果音：シュンシュンシュン！
                    if (_is味方 == true)
                    {
                        _randomString = "会心の一撃！！";
                        game.ea攻撃会心エフェクト(p_行動中キャラid, _被攻撃キャラid);
                    }
                    else
                    {
                        _randomString = "痛恨の一撃！！";
                        game.ea攻撃痛恨エフェクト(p_行動中キャラid, _被攻撃キャラid);
                    }
                    game.mメッセージ単語_末尾改行なし_瞬時に表示(_randomString + "　", ESPeed.s05_普通＿標準で１秒);
                    _攻撃力最終 *= s_会心攻撃力上昇倍率1_5;
                    _魔法力最終 *= s_会心攻撃力上昇倍率1_5;
                    _is会心した = true;
                    _hugouKaishin = " <=";
                }

                // ●クリティカル判定
                bool _isクリティカルした = false;
                string _hugouKuri = " > ";
                double _randomNumKuri = MyTools.getRandomNum(1, 100);
                if (_randomNumKuri <= _クリ率 + _クリ率増減率)
                {
                    game.mメッセージ単語_末尾改行なし_瞬時に表示("クリティカルヒット！"+ "", ESPeed.s05_普通＿標準で１秒);
                    _攻撃力最終 *= s_クリティカル攻撃力上昇倍率1_0;
                    _魔法力最終 *= s_クリティカル時に加算する魔法力倍率1_0;
                    _isクリティカルした = true;
                    _hugouKuri = " <=";
                    if (_is味方 == true)
                    {
                        game.ea味方通常攻撃クリティカルエフェクト(p_行動中キャラid, _被攻撃キャラid);
                    }
                    else
                    {
                        game.ea敵通常攻撃クリティカルエフェクト(p_行動中キャラid, _被攻撃キャラid);
                    }
                }

                // ●奇跡・悲劇の一撃判定
                bool _is奇跡した = false;
                string _hugouKiseki = " > ";
                double _randomNumKiseki = MyTools.getRandomNum(1, 100);
                if (_randomNumKiseki <= _奇跡率 + _奇跡率増減率)
                {
                    if (_is味方 == true)
                    {
                        _randomString = "なんとっ！　奇跡の一撃が発動！！！";
                        game.ea攻撃奇跡エフェクト(p_行動中キャラid, _被攻撃キャラid);
                    }
                    else
                    {
                        game.pSE(ESE・効果音.attack04b・悲劇の一撃_シュゥーーンドバドバドバァーーーン);
                        _randomString = "まずいっ！　悲劇の一撃が発動！！！";
                        game.ea攻撃悲劇エフェクト(p_行動中キャラid, _被攻撃キャラid);
                    }
                    game.mメッセージ単語_末尾改行なし_瞬時に表示("\n"+_randomString+"", ESPeed.s02_非常に遅い＿標準で３秒);
                    _攻撃力最終 *= s_奇跡攻撃力上昇倍率2_0;
                    _魔法力最終 *= s_奇跡攻撃力上昇倍率2_0;
                    _相手防御力 = 0;
                    _is会心した = true;
                    _hugouKiseki = " <=";
                }
                // ここで強制改行
                game.mメッセージ_自動送り("", ESPeed.s09_超早い＿標準で１００ミリ秒);
                #endregion

                // ■■■【ダメージ計算機】受けるダメージの計算
                // (a)攻撃力-防御力の超単純式
                //double _通常物理ダメージ = Math.Max(0, _攻撃力最終 - _相手防御力);
                // (b)いろいろなダメージ計算式を使えるクラスを呼び出す
                double _通常物理ダメージ = CDamageCalc・ダメージ計算機.getFhysicalDamage(_isクリティカルした, _is会心した, _is奇跡した);
                
                double _防御無視物理ダメージ = _攻撃力最終;
                double _魔法ダメージ = _魔法力最終; // 魔法防御力は？
                // 受けるダメージは、通常は物理ダメージ
                int _受けるダメージ = (int)_通常物理ダメージ;
                // クリティカルしない／した場合で、異なる受けるダメージを代入
                double _クリティカルダメージ =
                    _魔法ダメージ + Math.Max(0, _攻撃力最終 * s_クリティカル攻撃力上昇倍率1_0 - _相手防御力);
                if (_isクリティカルした == true)
                {
                    if (s_isクリティカル時のプラス魔法力は防御無視 == true)
                    {
                        // クリティカルの魔法力分は，防御による軽減は無効にする。
                        _受けるダメージ = (int)_クリティカルダメージ;
                    }
                    else
                    {
                        _受けるダメージ = (int)Math.Max(0.0, _攻撃力最終 + _魔法ダメージ - _相手防御力);
                    }
                }
                else
                {
                    _魔法ダメージ = 0;
                }


                // 戦闘ダメージ計算デバッグ
                Program・実行ファイル管理者.printlnLog(ELogType.lgui1_ログGUIText戦闘用,
                    "・攻撃ダメージ" + _受けるダメージ + " = (攻撃力最終" + _攻撃力最終 + " - 防御力" +
                    _相手防御力 + ") + 魔法力" + _魔法ダメージ + "\n" +
                    "【集中" + MyTools.getBoolCheckString(_is集中した) +
                      "(乱数" + (int)_randomNumSyuu + _hugouSyutyu + "" + (int)_集中率 + "％）】" + "\n" +
                    " 【クリ" + MyTools.getBoolCheckString(_isクリティカルした) +
                      " (乱数" + (int)_randomNumKuri + _hugouKuri + "" + (int)_クリ率 + " + " + (int)_クリ率増減率 + "％) 】" + "\n" +
                    " 【" + _会心or痛恨 + "" + MyTools.getBoolCheckString(_is会心した) +
                      " (乱数" + (int)_randomNumKaishin + _hugouKaishin + "" + (int)_会心率 + " + " + (int)_会心率増減率 + "％) 】" + "\n" +
                    " 【" + _奇跡or悲劇 + "" + MyTools.getBoolCheckString(_is奇跡した) +
                      " (乱数" + (int)_randomNumKiseki + _hugouKiseki + "" + (int)_奇跡率 + " + " + (int)_奇跡率増減率 + "％) 】"// + "\n"
                    );

                // ●回避判定
                bool _is回避した = false;
                string _hugouKaihi = " > ";
                double _randomNumKaihi = game.getRandom・ランダム値を生成(1, 100);
                //double _回避失敗度 = _randomNumKaihi;   // 0に近いほどうまくかわし、_相手回避率-1に近いほどギリギリでかわして、100に近いほど駄目だったことになる
                //double _回避余り率minus100to90 = _相手回避率 - _randomNumKaihi; // 回避しなかった時はマイナスになる
                //double _回避余り率_回避した0to90 = _回避余り率_回避した0to90;   // 高いほど余裕でかわしたことになる
                if (_randomNumKuri <= _相手回避率)
                {
                    game.ea回避エフェクト(_被攻撃キャラid);
                    _randomString = MyTools.getRandomString(
                        "は素早くかわした！", "は巧みに避けた！", "とっさにしゃがんだ！", "回避行動を取った！",
                        "は高くジャンプした！", "はすかざず横にステップを踏んだ！", "は瞬時に回避した！", "紙一重でかわした！");
                    game.mメッセージ単語_末尾改行なし_瞬時に表示(_被攻撃キャラ名 + _randomString + "　", ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                    _is回避した = true;
                    _hugouKaihi = " <=";

                    //_randomString = MyTools.getRandomString(
                    //"は優雅に微笑んでいる…。", "は華麗なステップを踏んでいる…。",
                    //"は運良くかわした！", "はなんなく回避した！",  "はなんとか避けた！", "は間一髪でかわした！");
                }

                // ■回避処理
                bool _is当たった = true;
                string _回避結果 = "回避無（ダ1倍）";
                string _hugouKaihi2 = " > ";
                double _回避失敗率 = s_回避大失敗率標準;
                double _randomNumKaihi2 = game.getRandom・ランダム値を生成(1, 100);
                double _回避ダメージ倍数 = 1.0;
                if (_is回避した == true)
                {
                    // ●命中判定
                    double _回避失敗率標準 = s_回避大失敗率標準;
                    // 命中-回避の(差/10)％分だけ、失敗率を広げたり狭めたりする
                    double _回避失敗率補正 = (_命中率マイナス相手回避10to190_同じ時100 - 100.0) / 10.0;
                    _回避失敗率 = _回避失敗率標準 + _回避失敗率補正;

                    if (_randomNumKaihi2 <= _回避失敗率)
                    {
                        _回避結果 = "大失敗（ダ2倍）";
                        _hugouKaihi2 = " <=";
                        _回避ダメージ倍数 = 2.0;
                        _randomString = MyTools.getRandomString(
                            "\nしかし、" + p_行動中キャラ名 + "はその裏をかいて、渾身の一撃を浴びせた！！",
                            // "\nしかし、" + _被攻撃キャラ名 + "は油断して、無防備な姿をさらしてしまった！！",
                            "\nしかし、その動きは、" + p_行動中キャラ名 + "に読まれていた！　渾身の一撃！！",
                            "\nしかし、" + p_行動中キャラ名 + "は" + _被攻撃キャラ名 + "が無防備になった瞬間を見逃さなかった！！");
                        game.mメッセージ_自動送り(_randomString, ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                    }
                    else if (_randomNumKaihi2 <= _回避失敗率 * 2.5)
                    {
                        _回避結果 = "失敗（ダ0.25倍）";
                        _hugouKaihi2 = " <=";
                        _回避ダメージ倍数 = 0.25;
                        _randomString = MyTools.getRandomString(
                            ""+_被攻撃キャラ名 + "は、ちょっとよろめいた。",
                            "" + p_行動中キャラ名 + "は続けて追い打ちをかけた！",
                            "" + p_行動中キャラ名 + "の攻撃は厳しく、避け切れなかった！",
                            "" + p_行動中キャラ名 + "の攻撃は思ったよりも鋭かった！",
                            _被攻撃キャラ名 + "は間一髪で避けた！");
                        game.mメッセージ_自動送り(_randomString, ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                    }
                    else if (_randomNumKaihi2 <= _回避失敗率 * 4.0)
                    {
                        _回避結果 = "かすり（ダ0.125倍）";
                        _hugouKaihi2 = " <=";
                        _回避ダメージ倍数 = 0.125;
                        _randomString = MyTools.getRandomString(
                            p_行動中キャラ名 + "の攻撃は、少しだけかすった。",
                            _被攻撃キャラ名 + "は、かすりキズを負った。",
                            _被攻撃キャラ名 + "は、なんとか回避した！",
                            _被攻撃キャラ名 + "は、一瞬ひるんだ！",
                            _被攻撃キャラ名 + "は、ギリギリのところでかわした！",
                            p_行動中キャラ名 + "の一撃は体をかすめた！",
                            p_行動中キャラ名 + "の攻撃は少しだけ当たった。");
                        game.mメッセージ_自動送り(_randomString, ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                    }
                    else
                    {
                        _is当たった = false;
                        _回避ダメージ倍数 = 0.0;
                        // 完全回避（当たらない、ノーダメージ）
                        _randomString = MyTools.getRandomString(
                        "は優雅に微笑んでいる…。", "、見事なフットワークだ！", "は華麗なステップを踏んでいる。",
                        "はつまらなそうな顔で" + p_行動中キャラ名 + "を見ている…。",
                        "は余裕綽綽だ！", "", "", "", "");
                        if (_randomString != "")
                        {
                            game.mメッセージ_自動送り("\n"+_被攻撃キャラ名 + _randomString, ESPeed.s03_遅い＿標準で２秒);
                        }
                    }
                }
                Program・実行ファイル管理者.printlnLog(ELogType.lgui1_ログGUIText戦闘用,
                    "【回避" + MyTools.getBoolCheckString(_is回避した) +
                    " (乱数" + (int)_randomNumKaihi + _hugouKaihi + "回避率" + (int)_相手回避率 + "％), " +
                    "" + _回避結果 + "】:" +
                    " (乱数" + (int)_randomNumKaihi2 + _hugouKaihi2 + "大失敗" + (int)_回避失敗率 + "％ or 失敗" +
                    (int)(_回避失敗率 * 2) + "％ or かすり" + (int)(_回避失敗率 * 3) + "％）");


                // ■ガード処理
                bool _isガードした = false;
                string _ガード結果 = "ガード無（ダ1倍）";
                string _hugouGurd = " > ";
                double _randomNumGurd = game.getRandom・ランダム値を生成(1, 100);
                double _ガードダメージ倍数 = 1.0;
                // 回避無のとき、または回避大失敗したときに発動
                if (_is当たった == true && (_回避ダメージ倍数 == 2.0 || _回避ダメージ倍数 == 1.0))
                {
                    if (_randomNumGurd <= _相手ガード率)
                    {
                        _isガードした = true;
                        _hugouGurd = " <=";
                        string _addString = "";
                        _randomString = MyTools.getRandomString(
                            "は防御を試みた！　",
                            "はガードの構えを取った！　",
                            "はすかさず身を守った！　",
                            "はとっさに防御した！　");
                        if (_is回避した == true)
                        {
                            // 大失敗時のガード
                            _addString = "なんとっ！　それでも体勢を立て直し、";
                        }
                        game.mメッセージ単語_末尾改行なし_瞬時に表示(_addString + _被攻撃キャラ名 + _randomString, ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                    }

                    if (_randomNumGurd <= _相手ガード率 * (1.0 / 5.0))
                    {

                        _ガードダメージ倍数 = 0.0;
                        _ガード結果 = "ガード完全（ダ0）";
                        if (_is回避した == false)
                        {
                            _randomString = MyTools.getRandomString(
                                p_行動中キャラ名 + "の攻撃を完全に防いだ！！",
                                "完全防御でノーダメージ！！",
                                "完璧な構えで攻撃を無効化した！",
                                p_行動中キャラ名 + "の攻撃をもろともせずに弾いた！！");
                            game.mメッセージ_自動送り(_randomString, ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                        }
                        else
                        {
                            // 回避大失敗したのに、完全ガードでノーダメージ
                            _randomString = MyTools.getRandomString(
                                "\nなんと！　その素晴らしい受け身で、ダメージゼロ！！",
                                "\nそんなバカな！　" + p_行動中キャラ名 + "の攻撃は全く効いていない！！",
                                "\n" + _被攻撃キャラ名 + "の惚れ惚れとする武器さばきで、ダメージ無し！！");
                            game.mメッセージ_自動送り(_randomString, ESPeed.s03_遅い＿標準で２秒);
                        }
                    }
                    else if(_randomNumGurd <= _相手ガード率)
                    {
                        _ガードダメージ倍数 = 0.5;
                        _ガード結果 = "ガード受止（ダ0.5倍）";
                        _randomString = MyTools.getRandomString(
                            p_行動中キャラ名 + "攻撃を軽減！",
                            p_行動中キャラ名 + "の攻撃を受け止めた！",
                            "なんとか防いだ！",
                            "ガード成功！",
                            "堅い守りでダメージ半減！");
                        game.mメッセージ_自動送り(_randomString, ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                    }
                }
                Program・実行ファイル管理者.printlnLog(ELogType.lgui1_ログGUIText戦闘用,
                    "【ガード" + MyTools.getBoolCheckString(_isガードした) +
                    "" + _ガード結果+ "】:" +
                    " (乱数" + (int)_randomNumGurd + _hugouGurd + "半減ガード率" + (int)_相手ガード率 + "％ or 完全ガード率" + (int)(_相手ガード率 * 2.0 / 5.0) + "％）");

                // ■非回避処理
                // 回避ダメージ倍数、ガードダメージ倍数を考慮
                _受けるダメージ = (int)((double)_受けるダメージ * _回避ダメージ倍数);
                _受けるダメージ = (int)((double)_受けるダメージ * _ガードダメージ倍数);
                // ●ダメージ後の処理
                double _痛くない率 = 0.01;
                double _痛かったぞ率 = 0.30;
                double _痛すぎる率 = 0.80;
                double _相手最大HP = _被攻撃キャラ.Para(EPara.s03b_最大HP);
                double _相手残りHP = _相手HP - _受けるダメージ;
                double _受けるダメージHP割合 = _受けるダメージ / _相手HP;
                double _受けるダメージ最大HP割合 = _受けるダメージ / _相手最大HP;
                double _相手HP割合 = _相手残りHP / _相手最大HP;
                string _情景描写 = "";
                if (_is当たった == true)
                {
                    if (_受けるダメージ <= _相手HP * _痛くない率)
                    {
                        _randomString = MyTools.getRandomString(
                            "は蚊に刺された程度のダメージ", "には痛くもかゆくもないちゃっチャッ♪",
                            "にはほとんど効いていない！",
                            "は大丈夫だった…。", "はダメージを感じない！", "は笑っている…。",
                            "は余裕の表情だ。"); //"は見事に受けとめた！"
                        game.mメッセージ_自動送り(_被攻撃キャラ名 + _randomString, ESPeed.s03_遅い＿標準で２秒);
                    }
                    else if (_受けるダメージ >= _相手HP * _痛すぎる率)
                    {
                        game.pSE(ESE・効果音.damege03・特大ダメージ_ティラリーン);

                        // 致命的なダメージを受けた時。一撃で仕留められたときなど、結構頻度は多い。
                        _randomString = MyTools.getRandomString(
                           "クリーンヒットォ～", "決まったぁ～", 
                           _被攻撃キャラ名 + "は大きく吹っ飛んだ！",
                           _被攻撃キャラ名 + "に瀕死のダメージ！", _被攻撃キャラ名+"、万事休す…",
                           _被攻撃キャラ名+"、これは痛い…", _被攻撃キャラ名+"、再起不能か？",
                           _被攻撃キャラ名+"、もう無理ぽ…", "これは決まってしまったか…"); // _被攻撃キャラ名 + "に大ダメージ！！"
                        game.mメッセージ_自動送り(_randomString, ESPeed.s02_非常に遅い＿標準で３秒);
                    }

                    // ■■■■ダメージ処理
                    if (_受けるダメージ > 0)
                    {
                        game.mメッセージ_自動送り(_被攻撃キャラ.name名前() + "に " +
                            _受けるダメージ + " のダメージ！", ESPeed.s03_遅い＿標準で２秒);
                        // ●ダメージエフェクト
                        game.edダメージエフェクト(_被攻撃キャラid, _受けるダメージ);
                        _被攻撃キャラ.setPara(EPara.s03_HP, ESet.add・増減値, -1.0 * _受けるダメージ);
                    }
                    // ■残りHP更新の描画処理
                    drawUpdate・戦闘描画更新処理();


                    // ■情景描写処理　強い攻撃を受けた場合は、受けたダメージと残りHPによって相手の情景描写が入る
                    if (_isクリティカルした == true || _is会心した == true || _is奇跡した == true || _is集中した == true)
                    {
                        #region 残りHP割合でセリフが変わる
                        if (_相手HP割合 >= 0.8)
                        {
                            _情景描写 = "は笑っている";
                        }
                        else if (_相手HP割合 >= 0.65)
                        {
                            if (_受けるダメージHP割合 < _痛かったぞ率)
                            {
                                _情景描写 = "は平然としている";
                            }
                            else
                            {
                                _情景描写 = "から余裕の表情が消えた！";
                            }
                        }
                        else if (_相手HP割合 >= 0.5)
                        {
                            if (_受けるダメージHP割合 < _痛かったぞ率)
                            {
                                _情景描写 = " はまだ大丈夫そうだ";
                            }
                            else
                            {
                                _情景描写 = "、してやられたり！";
                            }
                        }
                        else if (_相手HP割合 >= 0.4)
                        {
                            _情景描写 = "はなんとか持ちこたえている…";
                        }
                        else if (_相手HP割合 >= 0.3)
                        {
                            if (_受けるダメージHP割合 < _痛かったぞ率)
                            {
                                _情景描写 = "はよろめいている…";
                            }
                            else
                            {
                                _情景描写 = "には、かなり効いたようだ！";
                            }
                        }
                        else if (_相手HP割合 >= 0.2)
                        {
                            if (_受けるダメージHP割合 < _痛かったぞ率)
                            {
                                _情景描写 = "は苦しい表情を見せた";
                            }
                            else
                            {
                                _情景描写 = "はいきなり窮地に立たされた！";
                            }
                        }
                        else if (_相手HP割合 >= 0.1)
                        {
                            _情景描写 = "、大ピンチ！";
                        }
                        else if (_相手HP割合 >= 0.05)
                        {
                            _情景描写 = "は意識がモウロウとしている…！";
                        }
                        else if (_相手HP割合 > 0)
                        {
                            _情景描写 = "はもう少しで倒れそうだ！"; // _情景描写 = " は 助けを求めている・・・";
                        }
                        else
                        {
                            // 残りHPが0のとき（一番多い）
                            _情景描写 = MyTools.getRandomString(
                                "、万事休すか…！？", "は致命傷を受けた！！", "にスペシャルなダメージ！！",
                                "、これはまずいぞ！！", "、大丈夫か！？", "は悲鳴を上げた！！",
                                "、これは苦しそうだ！！", "は一瞬、自分の一生が走馬灯のように駆け巡った！！",
                                "、ここで終わりなのか！？", "、ここでジ・エンドか！？");
                        }
                        #endregion
                        game.mメッセージ_自動送り(_被攻撃キャラ名 + _情景描写, ESPeed.s02_非常に遅い＿標準で３秒);
                    }
                }

                if (_相手残りHP < 0.0)
                {
                    game.pSE(ESE・効果音.dameyo01・戦闘不能_バタンッ);
                    int _overDamege = -1 * (int)_相手残りHP;
                    // 戦闘不能フラグ（HPが0になっても行動不能にならない仕様のため、ちゃんと設定する必要がある）
                    // (a)オーバーダメージによらず、戦闘不能ターン数は1（シンプルバージョン）
                    //_被攻撃キャラ.setPara(EPara.戦闘不能ターン数, 1);
                    // (b)オーバーダメージによって、戦闘不能ターン数を設定（オリジナルバージョン）
                    double _戦闘不能ターン数 = 1.0 + ((double)_overDamege / _相手HP) * 5.0; // 例：1割Overだったら+1.5、2割Overだったら+2.0、5割Overだったら+3.5
                    _被攻撃キャラ.setPara(EPara.戦闘不能ターン数, ESet.add・増減値, _戦闘不能ターン数);
                    string _message = (_is味方 == false ? "は気絶してしまった…。" : "を気絶させた！");
                    if (_overDamege <= _相手最大HP * 0.3)
                    {
                        _message = (_is味方 == false ? "は倒れてしまった…。" : "を倒した！");
                    }
                    else if (_overDamege <= _相手最大HP * 0.5)
                    {
                        _message = (_is味方 == false ? "は致命傷を受けた…。" : "に致命傷を与えた！");
                    }
                    else if (_overDamege <= _相手最大HP * 0.7)
                    {
                        _message = (_is味方 == false ? "は動けなくなってしまった…。" : "を再起不能にさせた！");
                    }
                    else
                    {
                        _message = (_is味方 == false ? "の魂が抜けていった…。" : "の魂が上空に飛んで行った…。");
                    }
                    game.mメッセージ_自動送り("" + _被攻撃キャラ.name名前() + _message, ESPeed.s02_非常に遅い＿標準で３秒);
                }

                setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避
            } // 次の攻撃（２回目）へ
        }
        protected void act_heal・回復処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            // ●回復判定
            double _回復ポイント = _a.p4_ダメージ数[_何回目];

            foreach (int _id in _a.p3_行動対象キャラID群[_何回目])
            {
                game.ec回復エフェクト(_id, _回復ポイント);
                game.mメッセージ_自動送り(p_charaAll・全キャラ[_id].name名前() + "の" + 
                    game.getParaName(EPara.s03_HP) + "が " + _回復ポイント + " ポイント回復した！");
                p_行動中キャラ.setPara(EPara.s03_HP, ESet.add・増減値, _回復ポイント);
            }
            setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避
        }
        protected void act_miss・ミス処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            string _randomString = MyTools.getRandomString(
                "の攻撃は当たらなかった！", "は空振りした！",
                "の攻撃は失敗した！", "の攻撃はミスった！", "の攻撃は空を斬った！");
            game.mメッセージ_自動送り("ミス！　"+p_行動中キャラ名 + _randomString);
            setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避

        }
        protected void act_gard・防御処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            game.eb防御エフェクト(p_行動中キャラid);
            game.mメッセージ_自動送り(p_行動中キャラ名 + "は身を守っている・・・。");
            setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避

        }
        protected void act_avoid・回避処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            //game.eb回避開始エフェクト(p_行動中キャラid);
            game.mメッセージ_自動送り(p_行動中キャラ名 + "は相手の動きをうかがっている・・・。");
            setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避

        }
        protected void act_support・補助処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避

        }
        protected void act_other・その他処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避
        }
        protected void act_damageAdd・攻撃ダメージ増減乗除処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避

        }
        protected void act_healAdd・回復量増減乗除処理(CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            setFeas_Against_NextAttack(p_行動中キャラ, _a, _何回目); // 次の相手の攻撃に対する防御・回避

        }
        
        protected void setFeas_Against_NextAttack(CChara・キャラ _キャラ, CCommandData・コマンド受け渡しデータ _a, int _何回目)
        {
            // ●次の防御・回避処理
            _キャラ.setVar・変数を変更(EVar.このターンの物理防御ダメージ軽減数, _a.p7_防御軽減ダメージ数[_何回目]);
            _キャラ.setVar・変数を変更(EVar.このターンの回避率, _a.p6_回避率[_何回目]);
        }
        #endregion


        /// <summary>
        /// 戦闘が開始されるときの画面初期化処理です．
        /// </summary>
        public void drawInitial・戦闘画面初期化処理()
        {
            
            game.getP_gameWindow・ゲーム画面().getP_usedFrom()._showBattleInitial・戦闘画面初期化処理(this);
        }
        /// <summary>
        /// 戦闘が継続されているときに常に呼び出される，画面描画処理です．
        /// </summary>
        public void drawUpdate・戦闘描画更新処理()
        {
            // HPなどパラメータの更新
            game.getP_gameWindow・ゲーム画面().getP_usedFrom()._drawFormControls・HPなどの細かいパラメータ描画更新処理(this);

            // 全体の描画処理（とりあえず）
            game.draw・描画更新処理();
        }
    }
}
