using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Yanesdk.Ytl;

namespace PublicDomain
{
    /// <summary>
    /// 誰もが気軽に自分のキャラを作って対戦して遊べるモード（テストゲーム）を管理するクラスです．ゲーム作りの第一歩となるサンプルです．
    /// 使い方は，呼び出し元クラスから，このクラスを生成して呼び出してください．
    /// CTestGame・テストゲーム _test = new CTestGame・テストゲーム();
    /// </summary>
    public class CTestGame・テストゲーム
    {
        /// <summary>
        /// ゲームが終わるまでfalseになっている、無限ループを管理している変数です。これをtrueにするとゲームが強制終了します。
        /// ただし、できればメソッド「_ゲーム終了時の処理()」で終了させてください。このメソッドがこの変数をtrueにするのを管理しています。
        /// </summary>
        private bool _isEnd・ゲームを終わる = false;

        /// <summary>
        /// ゲームの作成に必要な基本データや基本メソッドが全て呼び出せる，グローバル通信データです．
        /// </summary>
        CGameManager・ゲーム管理者 game;
        FGameBattleForm1 p_usedForm;

        // 以下、やっつけで必要だった変数。一応プライベートにしてるけど、別に触りたかったらpubicにしてもええで。

        /// <summary>
        /// 今回プレイヤーが作成した／ロードした主人公キャラ
        /// </summary>
        private CChara・キャラ _c主人公キャラ = null;

        private int _勝利時LVUP数 = 1;
        private int _敗北時LVUP数 = 3;
        private int _勝利同じステージ挑戦時時LVDOWN数 = 1;

        /// <summary>
        /// なんステージまでいったか（勝利回数とほぼいっしょ）
        /// </summary>
        private int _sステージ数 = 1;
        /// <summary>
        /// 難解リトライしたか（リベンジ回数といっしょ）
        /// </summary>
        private int _rリトライ数 = 0;

        /// <summary>
        /// タイマン（味方と敵の数を1に固定したい）時はtrue
        /// </summary>
        public bool p_isタイマン＿味方対敵キャラは１対１か = false;
        /// <summary>
        /// 味方パーティを２人以上にする時に追加。falseの場合はパーティを追加するかすら聞かれない。
        /// ※タイマンがtrueの時はパーティは戦闘から外れる。
        /// </summary>
        public bool p_is味方パーティを決めるか = true;
        /// <summary>
        /// 味方パーティの上限人数。敵は別。
        /// </summary>
        public int p_charaPlayerNum_MAX・味方パーティ上限人数 = 4;
        /// <summary>
        /// 敵パーティの上限人数。味方は別。
        /// </summary>
        public int p_charaEnemyNum_MAX・敵パーティ上限人数 = 5;
        /// <summary>
        /// 数を味方パーティの数を同じにしたければfalse
        /// </summary>
        public bool p_is敵キャラの強さや数を味方レベルで調整するか = true;
        public bool p_isBGMZero・ＢＧＭだけミュート = false; // ＢＧＭミュートにするか


        /// <summary>
        /// コンストラクタです．ゲーム画面などを初期化したグローバルタスク受け渡しゲームデータを引数に入れてください．
        /// </summary>
        public CTestGame・テストゲーム(CGameManager・ゲーム管理者 _g, FGameBattleForm1 _usedGameForm)
        {
            game = _g;
            game.setP_testGame(this);
            p_usedForm = _usedGameForm;

            //string _tuyosa = "";
            ////　デッバガビジュアライザのテスト
            //System.Drawing.Color _color = new System.Drawing.Color();
            //System.Drawing.Image _image = MyTools.getImage_NewBitmap(100, 100);
            //Enum _enum = new EBattleActionObject・攻撃対象();
            //Enum _enum2 = EBattleActionObject・攻撃対象.t01_敵単;
            //EBattleActionObject・攻撃対象 _enum3 = EBattleActionObject・攻撃対象.t02_敵全;
            //_tuyosa = "変更された？";

            //game.mメッセージ_ボタン送り("ゲームスタート！"); // 画面に表示されない・・・ゲーム画面のp_messageBoxがちゃんと代入されていないのか・・・．
            //String _messageText = game.getP_gameWindow・ゲーム画面().getNowMainMessageText();
            //MessageBox.Show("p_messageBoxのテキスト: " + _messageText);

            永久テスト戦闘開始();

        }
        private void 永久テスト戦闘開始()
        {
            // ■1.永久ループ、ゲームが終わるまで繰り返し実行
            while (_isEnd・ゲームを終わる == false)
            {
                
            LABELモードセレクト:
                game.setBGM_Mute・ＢＧＭをミュート状態にする(p_isBGMZero・ＢＧＭだけミュート);
                game.pBGM_setVolume(500); // テストなのと、はじめはびっくりさせるといけないから音量少量で
                game.pSE_setVolume(200); // テストなのと、はじめはびっくりさせるといけないから音量少量で
                // モードセレクト画面（ずっと流してて、前と同じだったらランダムで曲が変更されるかも…くわしくはpBGMにて。）
                game.pBGM(EBGM・曲.god01・神聖＿こうして伝説が始まった);

                game.QC質問選択肢("【c】皆で創るＲＰＧにようこそ！！！！【b】【b】【b】【b】。【w】\nダイスバトルで遊びますか？\nそれとも、ストーリーモードをテストプレイしますか？", 1, "ダイスバトルで遊ぶ", "ストーリーモードで遊ぶ", "ゲームを終わる");
                if (game.QAIsCannel質問結果＿キャンセル())
                {
                    // キャンセルの場合は何もしない。モードセレクトにループする。以下のラベルを設定した時といっしょ。
                    goto LABELモードセレクト;
                }
                else if (game.QA質問結果().k回答番号() == 1)
                {
                    // ダイスバトルで遊ぶ
                    #region ●●●（現在のテストゲームのメイン）ダイスバトルで遊ぶ
                    game.getP_gameWindow・ゲーム画面().getP_usedFrom()._setDiceBattleMode・ダイスバトルモードに画面変更();

                    game.mメッセージ_自動送り("【c】ここは世界中のキャラクターが集う、ダイスバトル空間です。");
                    string _name = "新しいキャラ";
                    string _tuyosa = "";
                    bool _is主人公決めた = false;
                    bool _is主人公を新規作成した = false;
                    int _c主人公作成回数 = 0;
                    // ■■2-1.永久ループ、主人公が決まるまでループ、やめるを選んだら_is主人公決めた=falseでループ抜ける
                    while (_is主人公決めた == false)
                    {
                        #region 主人公キャラの作成
                    LABEL主人公キャラの作成:
                        _is主人公を新規作成した = false;
                        game.QC質問選択肢("ダイスバトルで遊ぶ、主人公キャラを決めてください。\n新しくキャラを作成しますか？\nそれとも、これまで創られたキャラをロードしますか？", 2, EChoiceSample・選択肢例.charaCreate1_新しいキャラを作成する＿これまで創られたキャラをロードする＿やっぱりやめる);

                        if (game.QA質問結果().k回答番号() == -1 || game.QA質問結果().k回答番号() == 3)
                        {
                            // キャンセルorテスト戦闘をやめる
                            goto LABELモードセレクト;
                        }
                        else if (game.QA質問結果().k回答番号() == 2)
                        {
                            // ■これまでのキャラをロード
                            game.mメッセージ_自動送り("（キャラクターデータベースの読み込み中…）", ESPeed.s09_超早い＿標準で１００ミリ秒);
                            _c主人公キャラ = game.userSelectCharactor・選択キャラを取得();
                        }
                        else if (game.QA質問結果().k回答番号() == 1)
                        {
                            // ■新しくキャラを創造

                            // 二回目以降（これでいいですか？→いいえを押した場合）は修正
                            if (_is主人公を新規作成した == true)
                            {
                                // キャラ名入力を飛ばす
                            }
                            else
                            {
                                _c主人公作成回数 = 0;
                                game.mメッセージボックスを初期化();
                                //game.waitウェイト(1.0);
                                game.QI質問入力("【c】キャラの名前を入れてください。\nまた，作成者を認識するためのユーザIDとパスワード（どちらも半角英数字）を入れてください。\n※ユーザID・パスワードは空白でも結構です",
                                    0, _name, "", "UserID", "（空白でも可）", "PASS", "（空白でも可）");
                                // 【ゲーム初期化処理】（ゲームを新しく始めるまではほぼ一度きり！，複数ユーザ（２P同時操作とか）を作らない限りは．）
                                if (game.QA質問結果().k回答番号() == -1)
                                {
                                    // キャンセル
                                    goto LABEL主人公キャラの作成;
                                }
                                _name = game.QA質問結果().ks回答文字列(0);
                                string _userID = game.QA質問結果().ks回答文字列(1);
                                string _pass = game.QA質問結果().ks回答文字列(2);

                                game.createNewUser・新規ユーザ作成(_name, _userID, _pass);

                                game.mメッセージボックスを初期化();
                                game.mメッセージ_自動送り("はじめまして、" + _name + "さん。ここは初心者の広場です。\n" +
                                    "まず、あなたの名前で、新しいキャラクターを創造しましょう。");

                                //game.m選択肢付メッセージ("キャラのパラメータは、自分で振り分けしたいですか？\nそれとも、おまかせにしますか？\n", EChoiceSample・選択肢例.auto＿手動＿自動, 1, 0);
                                game.QC質問選択肢("パラメータを自分で振り分けしますか（手動生成）？　それとも、自動生成しますか？\n", 1, EChoiceSample・選択肢例.auto＿手動＿自動);
                                game.mメッセージボックスを初期化();

                                // パラメータを自分で調整するか？
                                if (game.QA質問結果().k回答番号() == 1)
                                {
                                    // 手動作成
                                    _c主人公キャラ = game.userCreateNewChara・新しいキャラを作成(_name, 1, 250.0, 0.2);
                                }
                                else
                                {
                                    // 自動生成
                                    game.mメッセージ_自動送り("（只今キャラのパラメータを自動創造中…）", ESPeed.s03_遅い＿標準で２秒);
                                    _c主人公キャラ = game.userCreateNewChara・新しいキャラを作成(_name, 1, 250.0, 0.0);
                                }
                                if (_c主人公キャラ != null)
                                {
                                    _is主人公を新規作成した = true;
                                }
                            }
                        }
                        else
                        {
                            // 不正な入力
                            goto LABEL主人公キャラの作成;
                        }

                        if (_c主人公キャラ != null)
                        {
                            // 主人公のパラメータを調整した場合は、パラメータを修正するまで小ループ
                            bool _isパラメータ修正完了 = false;
                            while (_isパラメータ修正完了 == false)
                            {

                                if (_is主人公を新規作成した == false)
                                {
                                    // 主人公キャラをロードした場合は飛ばす
                                }
                                else
                                {
                                    if (_c主人公作成回数 > 0)
                                    {
                                        // 主人公のパラメータ調整後の二回目以降（これでいいですか？→いいえを押した場合）はパラメータ修正
                                        game.userSetCharaPara・キャラのパラメータを調整(_c主人公キャラ, 0, 0.2);
                                    }
                                }
                                game.showCharaDiceBattleStatus・キャラのダイスバトルステータスを表示(true, _c主人公キャラ, 0, true, true);
                                if(game.QC質問選択肢("このキャラでゲームを始めますか？", 
                                    1, EChoiceSample・選択肢例.e2a＿はい＿いいえ).isはい()){
                                    // 小ループ、大ループを抜ける
                                    _isパラメータ修正完了 = true;
                                    _is主人公決めた = true;
                                }
                                else
                                {
                                    if (_is主人公を新規作成した == true)
                                    {
                                        _c主人公作成回数++;
                                        // 小ループしたまま、パラメータ振り分け画面に戻る
                                    }
                                    else
                                    {
                                        // 主人公キャラを再選択する
                                        goto LABEL主人公キャラの作成;
                                    }
                                }
                            }
                            // 主人公を決めるまで、大ループに戻る
                        }
                        #endregion
                    }

                    // 以下、主人公キャラを決め終わって、はじまり
                    List<CChara・キャラ> _party仲間キャラたち_保存版 = new List<CChara・キャラ>();
                    // 以下は変わる
                    List<CChara・キャラ> _p次戦闘の味方キャラたち = new List<CChara・キャラ>();
                    List<CChara・キャラ> _e次戦闘の敵キャラたち = new List<CChara・キャラ>();
                    string _rライバルキャラ名 = "";
                    string _rライバルキャラの性別 = "男";
                    int _rライバルキャラの現在LV = 80;
                    CChara・キャラ _e敵キャラリーダー = game.getChara・キャラを取得("カノン", _rライバルキャラの現在LV); // デフォルト
                    _p次戦闘の味方キャラたち.Add(_c主人公キャラ);

                    game.QC質問選択肢("どちらのモードで遊びますか？ ", 1, "打倒ライバルモード", "トレーニングモード");
                    if (game.QANo質問回答番号() == 1)
                    {
                        #region 打倒ライバルモード
                    LABELライバル名決定処理:
                        game.QI質問入力(_c主人公キャラ.name名前() + "の宿敵（ライバル）の名前を決めてください。", 0, "ライバルの名前", "魔王");
                        _rライバルキャラ名 = game.QAStr質問回答文字列();
                        if (_rライバルキャラ名 == "")
                        {
                            // ""なら隠しボス
                            _rライバルキャラ名 = "カノン";
                            _rライバルキャラの性別 = "不明";
                            //_rライバルキャラの現在LV = 80;
                        }
                        // ライバルを作成
                        _rライバルキャラの現在LV = 10;
                        _e敵キャラリーダー = game.userCreateNewChara・新しいキャラを作成(_rライバルキャラ名, _rライバルキャラの現在LV, _c主人公キャラ.Para(EPara._基本6色総合値), 0);
                        // キャンセル処理（名前がユニークでなかった時など）
                        if (_e敵キャラリーダー == null) goto LABELライバル名決定処理;
                        game.QC質問選択肢("【c】宿敵の名前は、「" + _rライバルキャラ名 + "」でよろしいですか？", 1, EChoiceSample・選択肢例.e2a＿はい＿いいえ);
                        if (game.QA質問結果().isいいえ())
                        {
                            goto LABELライバル名決定処理;
                        }
                        else
                        {
                            game.QC質問選択肢(_rライバルキャラ名 + "の性別は？", 1, EChoiceSample・選択肢例.sex3＿男＿女＿不明);
                            // キャンセル処理
                            if (game.QA質問結果().isキャンセル()) goto LABELライバル名決定処理;
                            _rライバルキャラの性別 = game.QAStr質問回答文字列();
                        }
                        // 宿敵と勝負して負ける設定。
                        // ボス戦闘曲
                        game.pBGM(EBGM・曲.battleBoss01・ボス戦＿破壊神);
                        game.sダイス戦闘(_c主人公キャラ, _e敵キャラリーダー);
                        if (game.s戦闘に勝利() == true)
                        {
                            // 勝利曲
                            game.pBGM(EBGM・曲.win02・勝利ファンファーレ＿やったぜーやったぜー);
                            game.mメッセージ単語_末尾改行なし_ボタン送り("【c】ななな…なんと！　\n" + _c主人公キャラ.name名前() + "はライバル" + _rライバルキャラ名 + "に初戦で勝ってしまった！\n");
                            game.mメッセージ単語_末尾改行なし_自動送り("ラ【w】イ【w】バ【w】ル【w】【w】　討【w】伐【w】達【w】成【w】☆\n\n 【【w】C【w】o【w】n【w】g【w】r【w】a【w】t【w】u【w】l【w】a【w】t【w】i【w】o【w】n【w】s【w】【w】!【w】 \n\n", ESPeed.s02_非常に遅い＿標準で３秒);
                            _isEnd・ゲームを終わる = true;
                        }
                        else
                        {
                            // 敗北曲
                            game.pBGM(EBGM・曲.lose01・全滅＿がーん_もう終わりかよ);

                            if(_rライバルキャラの性別 == "男"){
                                game.mメッセージ_一行ずつボタン送り("【c】"+_rライバルキャラ名+"「ふっ…" + _c主人公キャラ.name名前() + "よ。\nお前は、まだ我と戦うには早すぎたようだな。\n我を倒したければ、もっと強くなることだ。\n出直してくるがいい。");
                            }else if(_rライバルキャラの性別 == "女"){
                                game.mメッセージ_一行ずつボタン送り("【c】"+_rライバルキャラ名+"「ふふっ、" + _c主人公キャラ.name名前() + "。\n貴方は、まだまだこんなものじゃないはずよ。\n私と対等になりたいなら、もっと強くなることね。\nそれじゃ、またあとでね。");
                            }else{
                                game.mメッセージ_一行ずつボタン送り("【c】"+_rライバルキャラ名+"「" + _c主人公キャラ.name名前() + "よ。\nそなたは所詮、この程度の存在なのか？\n我を消滅させたくば、もっと己の魂を磨くことだ。\nそなたの活躍を、祈っている。");
                            }
                            game.mメッセージ_一行ずつボタン送り("【c】" + _c主人公キャラ.name名前() + "は、宿敵 " + _rライバルキャラ名 + "を倒すため、\nまずは己を鍛えようと、修練場に向かったのであった…");
                        }
                        #endregion // 打倒ライバルモード、終わり
                    }
                    if (_isEnd・ゲームを終わる == false)
                    {
                        // 単にテストプレイ用のメッセージ
                        game.mメッセージ_ボタン送り("【c】さぁ、新しく作成したあなたのキャラクター\n「" + _c主人公キャラ.Var(EVar.名前) + "」で、様々なキャラと戦ってみましょう。");
                    }
                    

                    bool _isReFリベンジ = false;
                    bool _isEC敵キャラ変更 = true;
                    bool _isPartyFirstAddedパーティを既に決定した = false;

                    // ■■無限ループ 2-2.１キャラのダイスバトル戦闘繰り返しのループ
                    while (_isEnd・ゲームを終わる == false)
                    {
                        if (_is主人公決めた == false)
                        {
                            break; // やっぱりやめる、のときなど、一旦ダイスバトルをやめる
                        }

                        #region 味方キャラ追加処理
                        // とりあえず主人公キャラ一人を味方パーティにする

                        _p次戦闘の味方キャラたち.Clear();
                        _p次戦闘の味方キャラたち.Add(_c主人公キャラ);
                        if (p_is味方パーティを決めるか == true)
                        {
                            // 一緒に戦う味方キャラを追加
                            if (game.QC質問選択肢("パーティキャラ（仲間）と一緒に戦いますか？",
                                2, EChoiceSample・選択肢例.e2a＿はい＿いいえ).isはい())
                            // ※選択肢が２択の場合はこうかける。しかし、キャンセルの場合もfalseになるので気を付けて。
                            //   キャンセルを先に書く場合はここをisキャンセル()にして、後でgame.QA質問結果＿はい()を書く
                            {
                                // 初めての場合は、パーティを決める
                                if (_isPartyFirstAddedパーティを既に決定した == true)
                                {
                                    // 既に決めたパーティを追加するだけ
                                    _p次戦闘の味方キャラたち.AddRange(_party仲間キャラたち_保存版);
                                }
                                else
                                {
                                    _isPartyFirstAddedパーティを既に決定した = true;
                                LABEL追加味方キャラ選択:
                                    game.mメッセージ_自動送り("【c】一緒に戦う仲間を選んでください．");
                                    CChara・キャラ _c追加味方キャラ = game.userSelectCharactor・選択キャラを取得();
                                    if (_c追加味方キャラ == null)
                                    {
                                        // まだ味方キャラを追加するかを聞く
                                    }
                                    else
                                    {
                                        // キャラのステータスを表示
                                        game.showCharaDiceBattleStatus・キャラのダイスバトルステータスを表示(true, _c追加味方キャラ,
                                            _party仲間キャラたち_保存版.Count + 1, true, false);
                                        if (game.QC質問選択肢("このキャラをパーティに追加しますか？",
                                            1, EChoiceSample・選択肢例.e2a＿はい＿いいえ).isはい())
                                        {
                                            // 味方キャラを追加
                                            _party仲間キャラたち_保存版.Add(_c追加味方キャラ);
                                            _p次戦闘の味方キャラたち.Add(_c追加味方キャラ);
                                        }
                                    }
                                    if (_party仲間キャラたち_保存版.Count < p_charaPlayerNum_MAX・味方パーティ上限人数)
                                    {
                                    // まだ味方キャラを追加するかを聞く
                                    LABEL味方キャラを追加するか確認:
                                        if (game.QC質問選択肢("【c】まだ仲間をパーティに追加しますか？",
                                            1, EChoiceSample・選択肢例.e2a＿はい＿いいえ).isキャンセル())
                                        //   キャンセルを先に書く場合はここをisキャンセル()にして、後でgame.QA質問結果＿はい()を書く
                                        {
                                            // キャンセルの場合、入力を効かないようにする（入力し直し）
                                            goto LABEL味方キャラを追加するか確認;
                                        }
                                        else if (game.QAIsYes質問結果＿はい())
                                        {
                                            goto LABEL追加味方キャラ選択;
                                        }
                                    }
                                }
                            }
                            else if (game.QAIsNo質問結果＿いいえ())
                            {
                                // 味方キャラを追加せずに戦闘へ
                            }
                        }
                        #endregion

                        #region 敵キャラ選択処理
                        // 1～100の乱数
                        int _randomNum = MyTools.getRandomNum(1, 100);
                        // 敵キャラのレベルやパラメータの調整
                        double _e敵キャラのレベル調整値 = 0;
                        double _最低比較倍率 = 0.9; // 敵リーダーのパラメータ倍率
                        double _最高比較倍率 = 1.3;
                        double _ザコ最低比較倍率 = 0.5; // ザコ敵のパラメータ倍率
                        double _ザコ最高比較倍率 = 1.1;
                        string _charaName = "";
                        // 発生させる敵キャラの数は、基本、パーティの数といっしょ
                        int _eNum出現させる敵キャラ数 = _p次戦闘の味方キャラたち.Count; //game.getRandom・ランダム値を生成(1, _p次戦闘の味方キャラたち.Count);
                        if (_isEC敵キャラ変更 == true)
                        {
                            double _cLV主人公レベル = _c主人公キャラ.Para(EPara.LV);
                            _e次戦闘の敵キャラたち.Clear();
                            // レベルが3より少なければ弱くし、敵キャラ数を1にする（初回負けを避けるため）
                            if (_cLV主人公レベル < 3)
                            {
                                _e敵キャラのレベル調整値 = -3;
                                _eNum出現させる敵キャラ数 = 1;
                            }
                            // ■敵キャラの強さや数を味方レベルの強さ調整をするかどうか
                            if (p_is敵キャラの強さや数を味方レベルで調整するか == true)
                            {
                                // 敵キャラの数は、ランダムで、１体～パーティの数+1
                                _eNum出現させる敵キャラ数 = game.getRandom・ランダム値を生成(1, _p次戦闘の味方キャラたち.Count);

                                // レベルが10より少なければ弱くし、敵キャラ数を2までにする（序盤負けを避けるため）
                                if (_cLV主人公レベル > 3 && _cLV主人公レベル < 10)
                                {
                                    _e敵キャラのレベル調整値 = -2;
                                    _eNum出現させる敵キャラ数 = Math.Min(2, _eNum出現させる敵キャラ数);
                                }
                                // レベルが20より少なければ弱くし、敵キャラ数を2までにする（序盤負けを避けるため）
                                else if (_cLV主人公レベル < 20)
                                {
                                    _e敵キャラのレベル調整値 = -1;
                                    _eNum出現させる敵キャラ数 = Math.Min(2, _eNum出現させる敵キャラ数);
                                }
                                // レベルが30より少なければ、敵キャラ数を3までにする（中盤でイーブン）
                                else if (_cLV主人公レベル < 30)
                                {
                                    _e敵キャラのレベル調整値 = 0;
                                    _eNum出現させる敵キャラ数 = Math.Min(3, _eNum出現させる敵キャラ数);
                                }
                                // レベルが40より少なければ、敵キャラを強くし、敵キャラ数を3までにする（少し歯ごたえ）
                                else if (_cLV主人公レベル < 40)
                                {
                                    _e敵キャラのレベル調整値 = 1;
                                    _eNum出現させる敵キャラ数 = Math.Min(3, _eNum出現させる敵キャラ数);
                                }
                                // レベルが50より少なければ、敵キャラを強くし、敵キャラ数を4までにする（まずまずの歯ごたえ）
                                else if (_cLV主人公レベル < 50)
                                {
                                    _e敵キャラのレベル調整値 = 2;
                                    _eNum出現させる敵キャラ数 = Math.Min(4, _eNum出現させる敵キャラ数);
                                }
                                // レベルが50より多ければ、敵キャラを強くし、敵キャラ数を4までにする（かなりの歯ごたえ）
                                else if (_cLV主人公レベル >= 50)
                                {
                                    _e敵キャラのレベル調整値 = 3;
                                    _eNum出現させる敵キャラ数 = Math.Min(4, _eNum出現させる敵キャラ数);
                                }
                            }
                            // 敵キャラを生成する時の、味方キャラのパラメータ参考値（チームの合計にする？でもそれだと強い味方が居ると強い敵が出てきて主人公がなおざりになるか）
                            double _p6色パラメータ合計値参考値 = _c主人公キャラ.Para(EPara._基本6色総合値);
                            double _pLV参考値 = _c主人公キャラ.Para(EPara.LV) + _e敵キャラのレベル調整値;

                            game.QC質問選択肢("【c】戦うキャラは、これまでクリエーターが創られたゲストキャラがいいですか？\n" +
                                "それとも、トレーニング用キャラがいいですか？\nもしくは、他のプレイヤーが創ったキャラとも戦えます。", 1, "適当に、一番いいやつを頼む。", "トレーニング用キャラ（レベル上げに最適こちらからが無難かも^^）", "ゲストキャラ（某有名キャラたち？…強い者～弱いものまで^^;）", "プレイヤーキャラ（他のプレイヤーさんが創ったキャラ^-^");
                            //int a = game.QA質問結果().k回答番号();
                            if (game.QA質問結果().k回答番号() == 1)
                            {
                                // 適当に、一番いいやつを頼む。
                                int _ランダム回答番号 = game.getRandom・ランダム値を生成(2, 4);
                                // 適当に、キャラを選んじまおう
                                //[TODO]未来の回答を修正できればええんじゃないか？
                                // 適当に、回答番号をランダムにしてまうぜ！
                                //game.QA質問結果().setA回答を修正(0, _ランダム回答番号, _ランダム回答番号.ToString());
                                // _
                                if (_ランダム回答番号 == 2)
                                {
                                    _e次戦闘の敵キャラたち = game.createEnemys・亡霊キャラたちを自動生成(_eNum出現させる敵キャラ数, _pLV参考値);
                                    // 一定確率で、リーダーだけ、LVを同じにせず、パラメータをほどほどに似させる（亡霊キャラなので、あまり変化は無い）
                                    if (_c主人公キャラ.Para(EPara.LV) > 5 && _randomNum <= 80)
                                    {
                                        _e敵キャラリーダー = _e次戦闘の敵キャラたち[0];
                                        _e敵キャラリーダー = CCharaCreator・キャラ生成機.getChara_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変更して取得(_e敵キャラリーダー, _p6色パラメータ合計値参考値, _ザコ最低比較倍率, _ザコ最高比較倍率);
                                        _e次戦闘の敵キャラたち[0] = _e敵キャラリーダー;
                                        // 少しだけ強いリーダーが現れた時のメッセージ
                                        game.mメッセージ_自動送り("なにやら強者の気配がする…", ESPeed.s03_遅い＿標準で２秒);
                                    }
                                }
                                else if (_ランダム回答番号 == 3)
                                {
                                    _e次戦闘の敵キャラたち = game.createEnemys・総合パラメータがほどほどに似ているゲストキャラたちを自動生成(_eNum出現させる敵キャラ数, _p6色パラメータ合計値参考値, _ザコ最低比較倍率, _ザコ最高比較倍率);
                                }
                                else if (_ランダム回答番号 == 4)
                                {
                                    _e次戦闘の敵キャラたち = game.createEnemys・総合パラメータがほどほどに似ているプレイヤーキャラたちを自動生成(_eNum出現させる敵キャラ数, _p6色パラメータ合計値参考値, _ザコ最低比較倍率, _ザコ最高比較倍率);
                                }
                                // 適当に、一番いいやつを頼む、だと、ザコだから、そんなに強すぎるやつは出てこないだろ、たぶん。
                            }
                            // 敵キャラの種類2～4
                            if (game.QA質問結果().k回答番号() == 2)
                            {
                                // 亡霊キャラ（トレーニング用キャラ）
                                _e次戦闘の敵キャラたち = game.createEnemys・亡霊キャラたちを自動生成(_eNum出現させる敵キャラ数, _pLV参考値);
                                // 一定確率で、リーダーだけ、LVを同じにせず、パラメータをほどほどに似させる（亡霊キャラなので、あまり変化は無い）
                                if (_c主人公キャラ.Para(EPara.LV) > 5 && _randomNum <= 80)
                                {
                                    _e敵キャラリーダー = _e次戦闘の敵キャラたち[0];
                                    _e敵キャラリーダー = CCharaCreator・キャラ生成機.getChara_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変更して取得(_e敵キャラリーダー, _p6色パラメータ合計値参考値, _最低比較倍率, _最高比較倍率);
                                    _e次戦闘の敵キャラたち[0] = _e敵キャラリーダー;
                                    // 少しだけ強いリーダーが現れた時のメッセージ
                                    game.mメッセージ_自動送り("なにやら強者の気配がする…", ESPeed.s03_遅い＿標準で２秒);
                                }
                            }
                            else if (game.QA質問結果().k回答番号() == 3)
                            {
                                // ゲストキャラ
                                game.QC質問選択肢("【c】ゲストキャラは，あなたの能力に合わせてランダムに選択しますか？\n" +
                                    "それとも、自分で選びたいですか？", 1, "ランダムでお願い", "自分で選ぶ");
                                if (game.QA質問結果().k回答番号() == 1)
                                {
                                    _e次戦闘の敵キャラたち = game.createEnemys・総合パラメータがほどほどに似ているゲストキャラたちを自動生成(_eNum出現させる敵キャラ数, _p6色パラメータ合計値参考値);
                                }
                                else if (game.QA質問結果().k回答番号() == 2)
                                {
                                    game.QC質問選択肢("【c】戦うゲストキャラを選択してください。",
                                        1, EChoiceSample・選択肢例.charaAllゲストキャラ一覧);
                                    // ゲストキャラはたまに少し強くなる
                                    double _addLV = game.getRandom・ランダム値を生成(0, 3);
                                    _charaName = MyTools.getStringItem(game.QAStr質問回答文字列(), " ", 1);
                                    string _a = _charaName;
                                    // _charaNameが、グライドを選択した時「よくしんおう（漢字）」しか取得できてない
                                    _e敵キャラリーダー = game.getChara・キャラを取得(_charaName, _pLV参考値 + _addLV);
                                    // エラー対策
                                    if (_e敵キャラリーダー == null)
                                    {
                                        game.mメッセージ_自動送り("※キャラ取得にエラーが発生したため、ランダムでキャラを取得します。\n（せっかく選んでくれたのに、ごめんなさい…）", ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                                        _e敵キャラリーダー = CCharaCreator・キャラ生成機.getChara・パラ総合値がほどほどに似たキャラを取得(ECharaType・キャラの種類.c02_ゲストキャラ, _p6色パラメータ合計値参考値, _最低比較倍率, _最高比較倍率);
                                    }
                                    // 一定確率で、LVを同じにせず、パラメータをほどほどに似させる
                                    if (_randomNum <= 80)
                                    {
                                        _e敵キャラリーダー = CCharaCreator・キャラ生成機.getCharaFromCharaName_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変えて取得(
                                            _e敵キャラリーダー.Var(EVar.名前), _p6色パラメータ合計値参考値, _最低比較倍率, _最高比較倍率);
                                    }
                                    else
                                    {
                                        // 似ていなければ、ゲストキャラの基本LV1パラはかなり強いことがあるので危険。
                                        game.mメッセージ_自動送り("なにやら危険な香りがする…", ESPeed.s03_遅い＿標準で２秒);
                                    }
                                    _e次戦闘の敵キャラたち.Add(_e敵キャラリーダー);
                                    // 残りはザコのゲストキャラを自動で作成
                                    for (int i = 1; i <= _eNum出現させる敵キャラ数 - 1; i++)
                                    {
                                        CChara・キャラ _eザコ = game.createEnemy・ゲストキャラを自動生成(
                                            _p6色パラメータ合計値参考値, _ザコ最低比較倍率, _ザコ最高比較倍率);
                                        _e次戦闘の敵キャラたち.Add(_eザコ);
                                    }
                                }
                            }
                            else if (game.QA質問結果().k回答番号() == 4)
                            {
                                // プレイヤーキャラ
                                game.QC質問選択肢("【c】プレイヤーキャラは，ランダムに選択しますか？\n" +
                                    "それとも、自分で選びたいですか？", 1, "ランダムでお願い", "自分で選ぶ");
                                if (game.QA質問結果().k回答番号() == 1)
                                {
                                    string _プレイヤーキャラ名 = MyTools.getRandomString(game.getAllPlayerCharaName・全プレイヤーキャラ名を取得());
                                    _e次戦闘の敵キャラたち.Add(game.getChara・キャラを取得(_プレイヤーキャラ名, _pLV参考値));
                                }
                                else if (game.QA質問結果().k回答番号() == 2)
                                {
                                    game.QC質問選択肢("【c】戦うプレイヤーキャラを選択してください。",
                                        1, EChoiceSample・選択肢例.charaAllプレイヤーキャラ一覧);
                                    _charaName = MyTools.getStringItem(game.QAStr質問回答文字列(), " ", 1);
                                    //_charaName = game.QAStr質問回答文字列();
                                    _e敵キャラリーダー = game.getChara・キャラを取得(_charaName, _pLV参考値);
                                    // 一定確率で、LVを同じにせず、パラメータをほどほどに似させる
                                    if (_randomNum <= 100)
                                    {
                                        _e敵キャラリーダー = CCharaCreator・キャラ生成機.getCharaFromCharaName_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変えて取得(_e敵キャラリーダー.Var(EVar.名前), 
                                            _c主人公キャラ.Para(EPara._基本6色総合値), _最低比較倍率, _最高比較倍率);
                                    }
                                    _e次戦闘の敵キャラたち.Add(_e敵キャラリーダー);
                                    // 残りはザコを自動で作成
                                    for (int i = 1; i <= _eNum出現させる敵キャラ数 - 1; i++)
                                    {
                                        CChara・キャラ _eザコ = game.createEnemy・総合パラメータがほどほどに似ているキャラを全てのキャラから自動生成(
                                            _p6色パラメータ合計値参考値, _ザコ最低比較倍率, _ザコ最高比較倍率);
                                        _e次戦闘の敵キャラたち.Add(_eザコ);
                                    }
                                }
                            }
                        }
                        else
                        {
                            _isEC敵キャラ変更 = true;
                        }
                        #endregion
                        if (_isReFリベンジ == false)
                        {
                            _tuyosa = "【c】■■ Stage " + _sステージ数 + " ： ";
                            game.mメッセージ_自動送り(_tuyosa +
                                MyTools.getStringFormat(_c主人公キャラ.Var(EVar.名前) + " LV" + _c主人公キャラ.Para(EPara.LV), 10, false) + " VS " +
                                MyTools.getStringFormat(_e次戦闘の敵キャラたち[0].Var(EVar.名前) + " LV" + _e次戦闘の敵キャラたち[0].Para(EPara.LV), 10, false)
                                + " ■■"
                                , ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                        }
                        else
                        {
                            _tuyosa = "【c】■■ Stage " + _sステージ数 + "リベンジ ： ";
                            game.mメッセージ_自動送り(_tuyosa +
                                MyTools.getStringFormat(_c主人公キャラ.Var(EVar.名前) + "LV" + _c主人公キャラ.Para(EPara.LV), 10, false) + " VS " +
                                MyTools.getStringFormat(_e次戦闘の敵キャラたち[0].Var(EVar.名前) + "LV" + _e次戦闘の敵キャラたち[0].Para(EPara.LV), 10, false) +
                                " ■■\n（ゲーム進行速度を「s」「d」キーで調節できます。　※「s」キーで速度UP、「d」キーで速度DOWN）"
                                , ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                            _isReFリベンジ = false;
                        }
                        int _tabLength = _tuyosa.Substring(0).Length - 1;
                        _tuyosa = MyTools.getStringFormat("★ " + "基本的な強さ: ", _tabLength, true) +
                            MyTools.getStringFormat(MyTools.getStringNumber(_c主人公キャラ.Para(EPara._基本6色総合値), true, 8, 0), 10, false) + " VS " +
                            MyTools.getStringFormat(MyTools.getStringNumber(_e次戦闘の敵キャラたち[0].Para(EPara._基本6色総合値), false, 8, 0), 10, false);

                        game.mメッセージ_自動送り(_tuyosa, ESPeed.s06_やや早い＿標準で８００ミリ秒);
                        _tuyosa = MyTools.getStringFormat("★ " + "応用的な強さ: ", _tabLength, true) +
                            MyTools.getStringFormat(MyTools.getStringNumber(_c主人公キャラ.Para(EPara._中間6色総合値), true, 8, 0), 10, false) + " VS " +
                            MyTools.getStringFormat(MyTools.getStringNumber(_e次戦闘の敵キャラたち[0].Para(EPara._中間6色総合値), false, 8, 0), 10, false);
                        game.mメッセージ_自動送り(_tuyosa, ESPeed.s04_やや遅い＿標準で１３００ミリ秒);
                        // 描画処理
                        game.getP_Battle・戦闘().drawUpdate・戦闘描画更新処理();

                        _e敵キャラリーダー = MyTools.getListValue(_e次戦闘の敵キャラたち, 0);

                        // 敵キャラのレベルで，音楽を決める
                        EBGM・曲 _b戦闘曲 = EBGM・曲.battle01・通常戦闘＿Shoot_the_thoughts;
                        int _e敵の6パラ合計値 = _e敵キャラリーダー.para_Int(EPara._基本6色総合値);
                        int _c主人公キャラの6パラ合計値 = _c主人公キャラ.para_Int(EPara._基本6色総合値);
                        if (_sステージ数 == 1)
                        {
                            // はじめの曲
                            _b戦闘曲 = EBGM・曲.battle01・通常戦闘＿Shoot_the_thoughts;
                        }
                        // あとは敵の強さ毎に変わる
                        else if (_e敵の6パラ合計値 <= 2000)
                        {
                            // 普通位の敵ならこの音楽で
                            _b戦闘曲 = MyTools.getRandomValue<EBGM・曲>(
                                EBGM・曲.battle01・通常戦闘＿Shoot_the_thoughts,
                                EBGM・曲.battle02・通常戦闘＿Wind,
                                EBGM・曲.battleBoss02・ボス戦＿強き者に挑む);
                        }
                        else if (_e敵の6パラ合計値 <= 4000)
                        {
                            // Ａランク位の敵ならこの音楽で
                            _b戦闘曲 = EBGM・曲.GSR_新;
                        }
                        else if (_e敵の6パラ合計値 <= 100000)
                        {
                            // Ｓランク位の敵
                            _b戦闘曲 = EBGM・曲.battleBoss01・ボス戦＿破壊神;
                        }
                        // あとは主人公に比べて弱すぎたり強すぎたりしたら特定の音楽を鳴らす（テスト）
                        if (_e敵の6パラ合計値 <= _c主人公キャラの6パラ合計値 * 0.9)
                        {
                            // 0.9倍以下。ザコ敵用
                            _b戦闘曲 = EBGM・曲.e04_zako1・ザコ戦闘曲１;
                        }
                        else if (_e敵の6パラ合計値 > _c主人公キャラの6パラ合計値 * 1.2 && _e敵の6パラ合計値 <= _c主人公キャラの6パラ合計値 * 1.5)
                        {
                            // 1.2～1.5倍。ツヨ敵（中ボス）用
                            _b戦闘曲 = EBGM・曲.e03_boss1・ボス戦闘曲１;
                        }
                        
                        // ランダムで音楽再生
                        //int _ran = MyTools.getRandomNum(1, 100);
                        //if (_ran < 40) // 40%の確率で
                        //{
                        //    if (_b戦闘曲 == EBGM・曲.battle01・通常戦闘＿Shoot_the_thoughts)
                        //    {
                        //        // 音楽バグなので止める
                        //        MyTools.stopSound();
                        //    }
                        //    _b戦闘曲 = game.getRandomMusic(); 
                        //}

                        // ●音楽を再生
                        game.pBGM(_b戦闘曲);
                        //if (game.p_sound・サウンド管理者.getNowPlayingMusic・現在再生中の曲を取得() != _b戦闘曲)
                        //{
                        //    game.pBGM(_b戦闘曲);
                        //}


                        // ●戦闘開始！
                        game.sダイス戦闘(_p次戦闘の味方キャラたち, _e次戦闘の敵キャラたち);
                        // ●戦闘終了
                        if (game.s戦闘に勝利())
                        {
                            // デバッグテスト
                            if (_b戦闘曲 == EBGM・曲.battle01・通常戦闘＿Shoot_the_thoughts)
                            {
                                // 音楽バグなので止める
                                game.stopBGM・ＢＧＭを一時停止();
                            }

                            game.pBGM(EBGM・曲.win01・勝利ファンファーレ＿タラッラララッター);
                            game.mメッセージ_自動送り("【c】勝利！", ESPeed.s03_遅い＿標準で２秒);
                            game.LVUP・レベルアップ(_p次戦闘の味方キャラたち, _勝利時LVUP数);
                            double _e経験値 = 0;
                            if (_e敵キャラリーダー != null)
                            {
                                _e経験値 = _e敵キャラリーダー.Para(EPara._18色総合値) / 10.0;
                            }
                            game.mメッセージ_自動送り(_c主人公キャラ.Var(EVar.名前) + "は、" + (int)(_e経験値) + "の経験値を得た。", ESPeed.s03_遅い＿標準で２秒);
                            double _randomNum2 = game.getRandom・ランダム値を生成(0, 10);
                            string _message = _c主人公キャラ.Var(EVar.名前) + "はLVUP！";
                            for (int i = 1; i <= _勝利時LVUP数 + _randomNum2 % 3; i++)
                            {
                                game.mメッセージ_自動送り(_message, ESPeed.s05_普通＿標準で１秒);
                            }

                            game.QC質問選択肢("次のステージに進みますか？",
                                1, "イエッス！", "ちょっとまったぁ～");
                            //何故かだめif (game.QA質問結果().is回答(EAnswerSample・回答例.ＯＫ) == true)
                            if (game.QA質問結果().k回答番号() == 1)
                            {
                                _次のステージへ進む(_c主人公キャラ, ref _tuyosa, _p次戦闘の味方キャラたち, ref _sステージ数);
                            }
                            else
                            {
                                game.pBGM(EBGM・曲.god01・神聖＿こうして伝説が始まった);
                                game.QC質問選択肢("このステージに再挑戦しますか？\nそれとも、ゲームを終了しますか？",
                                    1, "このまま次のステージに進む", "このキャラに再チャレンジ", "もうやめる");
                                if (game.QA質問結果().k回答番号() == 1)
                                {
                                    _次のステージへ進む(_c主人公キャラ, ref _tuyosa, _p次戦闘の味方キャラたち, ref _sステージ数);
                                }
                                else if (game.QA質問結果().k回答番号() == 2)
                                {
                                    game.mメッセージ_ボタン送り("では、このステージで、もう一度同じキャラとの対戦です。");
                                    _isEC敵キャラ変更 = false;
                                }
                                else
                                {
                                    // ダイスバトルをやめる
                                    break;
                                }
                            }
                        }
                        else if (game.s戦闘に敗北())
                        {
                            // 味方パーティの戦闘不能を解除
                            foreach (CChara・キャラ _chara in _p次戦闘の味方キャラたち)
                            {
                                _chara.setPara(EPara.戦闘不能ターン数, 0);
                            }

                            game.pBGM(EBGM・曲.lose01・全滅＿がーん_もう終わりかよ);
                            game.QC質問選択肢("【c】負けてしまいました…残念。。。\n" +
                                "リベンジしますか？\nそれとも、諦めて他のキャラと戦うますか？",
                                1, "リベンジ！", "諦めて他のキャラと戦う", "もうやめる");
                            if (game.QA質問結果().k回答番号() == 1)
                            {
                                _isEC敵キャラ変更 = false;
                                _rリトライ数++;
                                game.LVUP・レベルアップ(_p次戦闘の味方キャラたち, _敗北時LVUP数);
                                _tuyosa = _c主人公キャラ.Var(EVar.名前) + "はLVUP！";
                                for (int i = 1; i <= _敗北時LVUP数; i++)
                                {
                                    game.mメッセージ_自動送り(_tuyosa, ESPeed.s06_やや早い＿標準で８００ミリ秒);
                                }
                            }
                            else if (game.QA質問結果().k回答番号() == 2)
                            {
                                if (_c主人公キャラ.Para(EPara.LV) >= _勝利同じステージ挑戦時時LVDOWN数 + 1)
                                {
                                    game.LVUP・レベルアップ(_p次戦闘の味方キャラたち, (-1) * _勝利同じステージ挑戦時時LVDOWN数);
                                    for (int i = 1; i <= _勝利同じステージ挑戦時時LVDOWN数; i++)
                                    {
                                    }
                                }
                                _tuyosa = _c主人公キャラ.Var(EVar.名前) + "は一度初心にかえるため、あえてLVを " + _勝利同じステージ挑戦時時LVDOWN数 + " 下げた…";
                                game.mメッセージ_自動送り(_tuyosa, ESPeed.s02_非常に遅い＿標準で３秒);
                            }
                            else
                            {
                                game.mメッセージ_自動送り("お気を悪くされてしまっていたら、申し訳ありません。", ESPeed.s02_非常に遅い＿標準で３秒);
                                game.mメッセージ_自動送り("\nゲームはストレスを解消するためにものなのに…。\n開発者の力不足で、ごめんなさい。。。", ESPeed.s03_遅い＿標準で２秒);
                                game.mメッセージ_自動送り("\n今後、このようなことがないよう、精一杯改善に努めていきたいと思います。", ESPeed.s02_非常に遅い＿標準で３秒);
                                // ダイスバトルをやめる
                                break;
                            }
                        }
                        // 一回のダイスバトルが終了

                    }
                    //endwhile １キャラのダイスバトル繰り返し終了
                    if (_c主人公キャラ != null)
                    {
                        game.QC質問選択肢("【c】お疲れさまでした。\nこのキャラクターを次回からプレイヤーキャラに登録しますか？\n（LVも保存されます）", 1, EChoiceSample・選択肢例.e2a＿はい＿いいえ);
                        if (game.QA質問結果().isはい())
                        {
                            // ■このプレイヤーデータを保存！
                            game.savePlayerCharaData・キャラをプレイヤーデータベースに追加(_c主人公キャラ);
                        }
                    }

                    // 主人公キャラのリセット
                    _c主人公キャラ = null;
                    #endregion
                }
                else if (game.QA質問結果().k回答番号() == 2)
                {
                    // ■ストーリーモードをテストプレイする
                    string _シナリオファイル名 = Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス + "シナリオ1.txt";
                    string _シナリオ１全文 = MyTools.ReadFile(_シナリオファイル名);
                    game.sシナリオ進行開始(_シナリオ１全文);
                    // シナリオを終わる

                }
                else if(game.QA質問結果().k回答番号() == 3)
                {
                    // 確認
                    if(game.QC質問選択肢("ゲームを終了して本当によろしいですか？",
                        1, EChoiceSample・選択肢例.e2a＿はい＿いいえ).isはい())
                    {
                        // ゲームをやめる
                        _isEnd・ゲームを終わる = true;
                        break;
                    }
                    // キャンセルはいいえと同じ。はじめのモード選択画面にループする
                }

            }

            // ゲーム終了処理
            game.End・ゲーム終了処理();
        }

        private void _次のステージへ進む(CChara・キャラ _c新キャラ, ref string _tuyosa, List<CChara・キャラ> _p味方キャラたち, ref int _sステージ数)
        {
            _sステージ数++;
        }

        /// <summary>
        /// game.Endゲーム終了処理()から実行されます。このメソッドを直接呼び出す必要はありません。
        /// プレイヤーデータの保存や、最後のメッセージなどを表示します。
        /// </summary>
        public void _ゲーム終了時の処理()
        {

            if (_isEnd・ゲームを終わる == false)
            {
                // 主人公キャラの保存。↑でもして=nullにしているが、念のため・・・
                if (_c主人公キャラ != null)
                {
                    game.QC質問選択肢("【c】お疲れさまでした。\nこのキャラクターを次回からプレイヤーキャラに登録しますか？\n（LVも保存されます）", 1, EChoiceSample・選択肢例.e2a＿はい＿いいえ);
                    if (game.QA質問結果().isはい())
                    {
                        // ■このプレイヤーデータを保存！
                        game.savePlayerCharaData・キャラをプレイヤーデータベースに追加(_c主人公キャラ);
                    }
                }

                // ■終了処理
                string _も = "";
                if (((_sステージ数-1) + _rリトライ数) >= 10) _も = "も";
                if((_sステージ数-1)>0){
                    game.mメッセージ_自動送り("ここまで、" + ((_sステージ数-1) + _rリトライ数) + " 回" + _も +
                        "プレイしてもらい、ありがとうございました！\n", ESPeed.s02_非常に遅い＿標準で３秒);
                    game.mメッセージ_自動送り("作者に意見・要望・提案がある方は、ツイッターアカウント「@merusaia」まで、\nお気軽にご連絡ください。", ESPeed.s02_非常に遅い＿標準で３秒);
                    game.mメッセージ_自動送り("またのご参加、お待ちしてます♪\n", ESPeed.s03_遅い＿標準で２秒);
                }
            }
            // この変数を変えるのは一応ここだけが原則
            _isEnd・ゲームを終わる = true;
        }



        // PollEvent() をLuaから呼び出すためのクラス
        //http://blog.livedoor.jp/artos/archives/50555864.html
        public class MySDLFrame
        {
          public Yanesdk.Ytl.YanesdkResult PollEvent()
          {
            return Yanesdk.Draw.SDLFrame.PollEvent();
          }
        }
        MySDLFrame p_sdlframe;
        Yanesdk.Draw.SDLWindow2DGl p_window; // 今は創って無い = game.getP_gameWindow・ゲーム画面().getP_window();
        Yanesdk.Timer.FpsTimer p_timer;
        Yanesdk.Draw.Font p_font;
        Yanesdk.Draw.GlTexture p_texture;
        bool p_isQuit = false;
        public void TestTerop・テロップ表示()
        {
            p_sdlframe = new MySDLFrame();

            p_window = new Yanesdk.Draw.SDLWindow2DGl();

            // 初期化 --------------------------------------------------------------
            //Yanesdk.Timer YT = luanet.Yanesdk.Timer;
            //Yanesdk.Draw YD = luanet.Yanesdk.Draw;

            //Quit = nil  //終了フラグ初期化

            //-- ウィンドウ設定
            // このウィンドウにはこれらのプロパティがない
            // p_window.SetVideoMode(640, 480, 0);
            // p_window.SetCaption("文字列を描画するテスト");

            // タイマー初期化
            p_timer = new Yanesdk.Timer.FpsTimer();
            p_timer.Fps = 60; // 初期FPS 60

            // フォント初期化
            p_font = new Yanesdk.Draw.Font(); // フォント生成
            p_font.Load("msgothic.ttc", 48, 0); // ＭＳ ゴシック、48pt
            p_font.SetColor(255, 255, 255); // 色は白

            
            // [ERROR] ***.Select()～***.Updatte()で囲まないとtextureは生成できない？
            // テクスチャ初期化
            p_texture = new Yanesdk.Draw.GlTexture(); // テクスチャ生成
            p_texture.SetSurface(p_font.DrawBlendedUnicode("Hello, world!")); // 文字列を貼り付け



            // ループ開始 ---------------------------------------------------------
            while ((p_sdlframe.PollEvent() == YanesdkResult.NoError) && (p_isQuit == false))
            {
                p_window.Screen.Select(); // 画面選択

                p_window.Screen.Clear(); // 画面クリア
                p_window.Screen.BlendSrcAlpha(); // αブレンド（透過）を有効にする

                // 描画
                p_window.Screen.Blt(p_texture, 100, 100);

                p_window.Screen.Update(); // 画面の更新

                p_timer.WaitFrame(); // フレーム調節

            }

        }


    }
}
