using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// キャラの性格や能力作成時などにプレイする，5分～2時間までで終了するキャラの伝説シナリオです（パワプロでいう短期間サクセス？）．
    /// </summary>
    public class CYourOwnLegend・伝説シナリオ
    {
        CGameManager・ゲーム管理者 game;
        CChara・キャラ p_n新キャラ = new CChara・キャラ();
        string p_n新キャラ名 = "";

        /// <summary>
        /// シナリオを決める毎に変化する、シナリオパラメータとしても作用します。
        /// 実は、新キャラが最後に出合うキャラ（ラスボス）です。
        /// 最後にシナリオパラメータと一番近いゲストキャラか、オリジナルキャラと出合います。
        /// </summary>
        CChara・キャラ p_l伝説キャラ = new CChara・キャラ();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CYourOwnLegend・伝説シナリオ(CGameManager・ゲーム管理者 _g)
        {
            game = _g;
        }

        public void begin・開始(){
            // 名前
            bool _次へ進む = false;
            while (_次へ進む == false)
            {
                game.QI質問入力("これからあなたが人生を創る、キャラクタの名前を入力してください。", "新しいキャラの名前", "", 0);
                game.QC質問選択肢("「" + game.QA質問結果().ks回答文字列() + "」でよろしいですか？", 2, EChoiceSample・選択肢例.e2a＿はい＿いいえ);
                if (game.QA質問結果().isはい())
                {
                    _次へ進む = true;

                    //p_n新キャラ.setnama名前を変更(game.QA質問結果().ks回答文字列());
                    p_n新キャラ.setVar・変数を変更(EVar.名前, game.QA質問結果().ks回答文字列());
                    p_n新キャラ名 = p_n新キャラ.name名前();
                    //p_n新キャラ名 = p_n新キャラ.getP_Vars・変数一括処理().getVar・変数値(EVar.名前);

                }
            }

            // 性別
            _次へ進む = false;
            while (_次へ進む == false)
            {
                game.QC質問選択肢(p_n新キャラ名 + "の性別は？", 1, EChoiceSample・選択肢例.sex3＿男＿女＿不明);
                game.QC質問選択肢("「" + game.QA質問結果().ks回答文字列() + "」でよろしいですか？", 2, EChoiceSample・選択肢例.e2a＿はい＿いいえ);
                if (game.QA質問結果().isはい())
                {
                    _次へ進む = true;
                    p_n新キャラ.setVar・変数を変更(EVar.性別, game.QA質問結果().ks回答文字列());
                }
            }
            string _s性別 = (game.QA質問結果().ks回答文字列() == "女" ? "彼女" : "彼");

            // キャラの顔を創る（最初に質問をして推定する？）
            game.createCharaFace・キャラの顔を創る();

            game.mメッセージ_ボタン送り("さぁ、"+p_n新キャラ名+"の人生を、"+_s性別+"と一緒に歩んで行きましょう…");
            //game.gameWindow・ゲーム画面().changeDark・暗くする(3000); // 3秒間で暗くしていく

            シナリオ開始();
        }

        #region ●伝説シナリオでよく使うプロパティ・メソッド
        public double p_pointRateポイント補正値 = 1.0;
        public double point(int _ポイント_いろいろな補正がかかる前の基準値)
        {
            return _ポイント_いろいろな補正がかかる前の基準値 * p_pointRateポイント補正値;
        }
        #region 引数・返り値が異なるメソッド
        public double point(double _ポイント_いろいろな補正がかかる前の基準値)
        {
            return _ポイント_いろいろな補正がかかる前の基準値 * p_pointRateポイント補正値;
        }
        public int pointInt(double _ポイント_いろいろな補正がかかる前の基準値)
        {
            return (int)(_ポイント_いろいろな補正がかかる前の基準値 * p_pointRateポイント補正値);
        }
        public int pointInt(int _ポイント_いろいろな補正がかかる前の基準値)
        {
            return (int)(_ポイント_いろいろな補正がかかる前の基準値 * p_pointRateポイント補正値);
        }
        #endregion
        #endregion

        public void シナリオ開始()
        {
            シナリオ1ー旅立ちの前ー();

            game.mメッセージ_ボタン送り("※現段階では、伝説シナリオはここで終了です。\n中途半端で終わってしまった方、ごめんなさい。。。");
            
        }

        public void シナリオ1ー旅立ちの前ー(){
            game.mメッセージ_ボタン送り("ここは、" + p_n新キャラ名 + "がこの世に生まれた瞬間にいた、旅立ちの地。");
            game.mメッセージ_ボタン送り("あなたは、" + p_n新キャラ名 + "がこれから歩む道を、密かに、そして暖かく見守る存在。");
            game.waitウェイト(1000);
            game.mメッセージ_ボタン送り(p_n新キャラ名 + "は、眩ゆい光に包まれた、宇宙・生命の源とも思える場所にたたずんでいます。");

            // 全ての選択した文字列を羅列して表示していくと、そのキャラの個性が観えていいかも？　
            // シナリオ中に「第三の目がある＊＊＊よ」とか言ってみたり＾＾。

            // ユーザが入力した「その他」の選択肢を次回から使うには、やはりシナリオはテキスト化する必要がある
            // 基本は小説、キャラ名「＊＊＊」、【質問】、【分岐】、【効果音】などを駆使して、
            // 英語・半角を使うと直観性がなくなるので、全て全角で、日本語のシナリオ言語を、なんとか作れないか？

            game.QC質問選択肢("あなたは、" + p_n新キャラ名 + "から、どんな姿が目に浮かびますか？", 0, EChoiceSample・選択肢例.sugata1＿力強い＿心優しい＿たくましい＿頼もしい＿自由な＿おごそかな＿その他);
            switch(game.QANo質問回答番号()){
                case 1: p_l伝説キャラ.setPara(EPara.c01_赤, ESet.add・増減値, point(100)); break;
                case 2: p_l伝説キャラ.setPara(EPara.c02_橙, ESet.add・増減値, point(100)); break;
                case 3: p_l伝説キャラ.setPara(EPara.c03_黄, ESet.add・増減値, point(100)); break;
                case 4: p_l伝説キャラ.setPara(EPara.c04_緑, ESet.add・増減値, point(100)); break;
                case 5: p_l伝説キャラ.setPara(EPara.c05_青, ESet.add・増減値, point(100)); break;
                case 6: p_l伝説キャラ.setPara(EPara.c06_紫, ESet.add・増減値, point(100)); break;
                case 7: ; break; // 新しい選択肢を入力
                default:break;
            }

            game.QC質問選択肢("あなたは、" + p_n新キャラ名 + "は、どこから生まれたように感じますか？", 0, EChoiceSample・選択肢例.hikari1＿まばゆい光＿ひっそりした闇);

            game.QC質問選択肢(p_n新キャラ名 + "は、どの部分が印象的ですか？", 0, EChoiceSample・選択肢例.karada1＿髪＿目＿鼻＿口＿手＿胸＿足＿その他);



            game.QC質問選択肢(p_n新キャラ + "の、一番特徴的な部分は、どこですか？", 0, EChoiceSample・選択肢例.sugata1＿白く光り輝いている＿鋭い目＿長く綺麗な髪＿華奢な体＿第三の目がある＿角が生えている＿羽が生えている＿しっぽが生えている＿その他);

            game.QC質問選択肢(p_n新キャラ名 + "は、どんな姿でたたずんていますか？", 1, EChoiceSample・選択肢例.sugata3a＿堂々と＿不安そうに＿ボーっと);
            switch (game.QA質問結果().k回答番号())
            {
                case 1: 宿敵がやってくる(); break;
                case 2: 優しい風が吹く(); break;
                case 3: 世界が移り変わっていく(); break;
                default: game.ユーザが新しい選択肢を追加(); break;
            }
        }

        #region ●どんな姿でたたずんでいる・・・？
        public void 宿敵がやってくる()
        {
            game.mメッセージ_ボタン送り("どこからともなく、足音がする。\n"+p_n新キャラ名 + "がかつてライバル視していた、人生の宿敵だ。");
            game.QC質問選択肢(p_n新キャラ名 + "は、宿敵を目の前にして、どうする？", 1, EChoiceSample・選択肢例.syukuteki3＿戦う＿話し合う＿捨て台詞を残して去る);
            switch (game.QA質問結果().k回答番号())
            {
                case 1: 宿敵と戦闘(); break;
                case 2: 宿敵と会話(); break;
                case 3: 宇宙旅行(); break;
                default: game.ユーザが新しい選択肢を追加(); break;
            }
        }
        #region ●宿敵を目の前にして・・・？
        public void 宿敵と戦闘()
        {
            game.pSE(ESE・効果音.attack01b・敵攻撃_ブルルッ);
            game.mメッセージ_ボタン送り("宿敵は、" + p_n新キャラ名+"が構えた瞬間、突然襲ってきた。\n" + p_n新キャラ名 + "は・・・");
            game.QC質問選択肢(p_n新キャラ名 + "は・・・", 1, EChoiceSample・選択肢例.syukutekisen1_すかさず応戦＿受け止める＿不意を突く);
            switch (game.QA質問結果().k回答番号())
            {
                case 1: // すかさず応戦
                    game.pSE(ESE・効果音.attack01a・味方攻撃_ピリリッ);
                    game.waitウェイト(1000);
                    game.pSE(ESE・効果音.gard01・ガード1_ガキィーン);
                    game.waitウェイト(2000);
                    game.pSE(ESE・効果音.attack01b・敵攻撃_ブルルッ);
                    game.waitウェイト(1500);
                    game.pSE(ESE・効果音.gard02・ガード2_キン);
                    game.waitウェイト(500);
                    game.pSE(ESE・効果音.gard01・ガード1_ガキィーン);
                    game.waitウェイト(3000);

                    game.mメッセージ_ボタン送り("戦闘は続いた。\n二人は長い間戦い続けたが、一向に決着は着かない・・・。");

                    game.waitウェイト(2000);

                    break;
                case 2: // 受け止める
                    ; break;
                case 3: // 不意を突く
                    宇宙旅行(); break;
                default: game.ユーザが新しい選択肢を追加(); break;
            }

        }
        public void 宿敵と会話()
        {

        }
        public void 宇宙旅行()
        {
            game.mメッセージ_ボタン送り("気がつくと、辺りは宇宙の世界。無限に広がる宇宙空間と、無重力で浮遊している間隔が心地よく、時間をかければどこまでも泳いで行けそうだ。");
            game.QC質問選択肢(p_n新キャラ名 + "は、どこに行く？", 1, EChoiceSample・選択肢例.utyuuryokou3＿火星＿冥王星＿他の星に何か感じた);
            switch (game.QA質問結果().k回答番号())
            {
                case 1: 火星での出来事(); break;
                case 2: 冥王星での出来事(); break;
                case 3: 他の星をランダムに(); break;
                default: game.ユーザが新しい選択肢を追加(); break;
            }
        }
        #endregion



        public void 優しい風が吹く()
        {

        }
        public void 世界が移り変わっていく()
        {
        }
        #endregion


        #region 伝説シナリオ3
        public void 火星での出来事()
        {

        }
        public void 冥王星での出来事()
        {

        }
        public void 他の星をランダムに()
        {
            //game.乱数().
        }
        #endregion

    }
}
