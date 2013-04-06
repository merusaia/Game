using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// キャラの種類を定義した列挙体です。
    /// </summary>
    public enum ECharaType・キャラの種類
    {
        c00_全キャラ,
        c01_プレイヤーキャラ,
        c02_ゲストキャラ,
        c03_亡霊キャラ,
    }

    /// <summary>
    /// 新しいキャラ生成を司るクラスです．ダイスコマンドの生成もこのクラスがやります。
    /// 静的メソッドを集めたクラスなので、CCharaCreator・キャラ生成機.***() で使いたいstaticメソッドを呼び出します。
    /// </summary>
    public class CCharaCreator・キャラ生成機
    {
        public static bool s_ダイスバトル_攻撃マスの攻撃力計算時にランダム要素を入れるか = true;

        public static string p_gestCharaDatabaseFileName = Program・実行ファイル管理者.ROOTDIRECTORY・実行ファイルがあるフォルダパス + "\\データベース\\キャラクタデータベース.csv";
        public static string p_playerCharaDtabaseFileName = Program・実行ファイル管理者.ROOTDIRECTORY・実行ファイルがあるフォルダパス + "\\データベース\\キャラクタデータベース_プレイヤー.csv";

        private static CDatabaseFileReader_ReadByString・データベース読み込み機 p_guestReader = null;
        private static CDatabaseFileReader_ReadByString・データベース読み込み機 p_playerReader = null;
        public static int p_syogoIndex・称号の列 = 5;
        public static int p_iroParaSumIndex・基本色6総合値の列 = 14;
        public static int p_iroParaSumIndex・応用色6総合値の列 = 14 + 7;
        #region データベース読み込み
        /// <summary>
        /// キャラクタデータベースを読み込みます．重い処理なので，一回だけ呼び出してください．
        /// </summary>
        /// <param name="_filenname"></param>
        /// <returns></returns>
        private static CDatabaseFileReader_ReadByString・データベース読み込み機 getDataBaseReader・データベース読み込み機の取得(String _databaseFileName)
        {

            CDatabaseFileReader_ReadByString・データベース読み込み機 _reader = new CDatabaseFileReader_ReadByString・データベース読み込み機();
            if (_reader.LoadDefFile・データベースcsvファイルの読み込み(_databaseFileName) != Yanesdk.Ytl.YanesdkResult.NoError)
            {
                Program・実行ファイル管理者.printlnLog(ELogType.l5_エラーダイアログ表示, "キャラクタデータベースが見つかりません．テキストエディタやExcelで開いている場合は参照できません。パス：　" + _databaseFileName);
            }
            return _reader;
        }
        #endregion

        public static List<string> p_newSampleCharaNameList = new List<string>(new string[] { 
            "アホネン", "アンドフ", "イラーネ", "ウレイナス", "エイドリアーン", "オレオダス", 
            "ベリアン", "マユトゥオー", "雪加弾", "走馬燈", "我はネオヅ",
            "ゥザ夫", "だめ子", "ゅぅーれぃ～チャン","最強ダヲォー～","めけんこめちゃんこぬ","ありが父サン",
            "ふぁぃゃ～でせヴェル","ぇんとらんぜルモン^^；","御前の相手はコノをれだ！"});
        public static List<string> p_pastSampleCharaNameList = new List<string>();
        
        // キャラ生成に関する定数
        /// <summary>
        /// LV1のキャラの標準の基本６色パラ総合値です。
        /// ここを変更すると、最初に作成されるキャラのパラメータの初期値合計値が変化します。
        /// ※現状リリース版は100（デバッグテスト用は250）
        /// </summary>
        public static double s_LV1Para_Iro6Sum・キャラＬＶ１標準の基本６色パラ総合値 = 250.0;
        /// <summary>
        /// キャラＬＶ１標準作成時の基本６色パラ総合値のうち、ボーナスパラとして振り分ける割合です。
        /// 現状リリース版は0.2。
        /// </summary>
        public static double s_LV1Para_BonusSumRate・キャラＬＶ１標準作成時のボーナスパラとして振り分ける割合 = 0.2;
        /// <summary>
        /// キャラＬＶＵＰ時の基本６色パラ総合値の増加パラメータのうち、ボーナスパラとして振り分ける割合です。
        /// 現状リリース版は0.2。
        /// </summary>
        public static double s_LVUP_BonusSumRate・キャラＬＶ１標準作成時のボーナスパラとして振り分ける割合 = 0.2;


        // ダイスマスに関係する影響
        public static int s0_Masu最大マス数 = 6;

        public static double s1_attaDevN_1マスの基本攻撃力倍率 = 1.0;

        // 身体パラメータによる影響
        public static double s_kougekirちからが攻撃力に比例する定数 = 1.0;

        public static double s_hp持久力が最大HPに比例する定数 = 3.0;

        public static double s_bougyo行動力が守備力に比例する定数 = 1.0;

        public static double s_sokudo素早さが速度に比例する定数 = 1.0;
        public static double s_bougyoNum行動力が防御マス数に対数比例するlog基底数 = 2.0;//常に6マスにしたいなら2.0。//可変にしたいなら前は6.0; // 個 + 初期値;
        public static double s_bougyoNum0防御マス初期値 = 0.0;
        //static double s_bougyoNum素早さが防御マス数に二次比例するXの2乗係数 = 0;//-0.00000234171; // 個 + 初期値;
        //static double s_bougyoNum素早さが防御マス数に二次比例するXの1乗係数 = 0.007433282; // 個 + 初期値;
        //static double s_bougyoNum0防御マス初期値 = 0.896076941; //0.896076941;
        public static double s_kaihiNum素早さが回避マス数に対数比例するlog基底数 = 8.0; // 個 + 初期値;
        public static double s_kaihi0回避マス初期値 = 0.0;
        // 【ソルバーでやった結果】
        // y = -2.34171E-06*x^2 + 0.007433282*x + 0.896076941　の二次関数で近似

        // 【手計算】
        //static double s_bougyoNum素早さが防御マス数に二次対比例する係数 = 5.0; // 個 + 初期値;
        //static double s_bougyoNum0防御マス初期値 = 1.0;
        //static double s_kaihiNum素早さが回避マス数に対数比例するlog基底数 = 10.0; // 個 + 初期値;
        //static double s_kaihi0回避マス初期値 = 1.0;
        // log_10(100)=2,log_10(1000)=3のa=10を基準にして，a=8？？
        // 【その他，近似の材料】
        // f(0)=0, f(50)=1, f(100)=2, f(200)=3, f(1000) = 6にしたいから，
        // ・log_a(100)=2, a^2=100，a=100^(1/2)= pow(100, 1/2) = 10
        // ・log_a(200)=3, a^3=200，a=pow(100, 1/3) = 4.6415
        // ・log_a(1000)=6, a^6=1000,a=1000^(1/6)= pow(1000, 1/6) = 3.162277660168379

        public static double s_kaihi精神力が回避率に比例0_020 = 0.020; // ％ + 初期値％;
        //static double s_kaihi精神力が回避率に比例0_010 = 1.0025; //1.0034 // ％ + 初期値％;
        public static double s_kai0回避率初期値 = 5.0; // 0.0
        public static double s_kai回避率MAX90 = 90.0;

        public static double s_mahou賢さが魔法力に比例 = 0.5;
        public static double s_kuri賢さがクリティカル率に比例0_015 = 0.015; //％ + 初期値％;
        public static double s_kur0クリティカル率初期値 = 10.0;
        public static double s_kuriクリティカル率MAX70 = 70.0;

        public static double s_mei器用さが命中率に比例0_010 = 0.010;
        public static double s_mei0命中率初期値 = 100.0;
        public static double s_mei命中率MAX90 = 90.0;

        public static double s_gurd行動力がガード率に比例0_005 = 0.005;
        public static double s_gurd器用さがガード率に比例0_001 = 0.001;
        public static double s_gurd忍耐力がガード率に比例0_003 = 0.003;
        public static double s_gurdガード率MAX50 = 50.0;

        public static double s_konzyo忍耐力が根性に比例0_099 = 0.099;
        public static double s_konzyo根性発動率MAX99 = 99.0;

        public static double s_over忍耐力が根性オーバーダメージに比例 = 1.0;

        public static double s_sp精神力がSPに比例 = 1.0;
        public static double s_sp健康力がSPに比例 = 1.0;
        public static double s_sp適応力がSPに比例 = 1.0;
        public static double s_sp集中力がSPに比例 = 1.0;

        public static double s_auto健康力が自然回復発動率に比例0_030 = 0.030;
        public static double s_auto自然回復発動率MAX90 = 90.0;
        public static double s_auto健康力が自然回復量に比例 = 0.3;

        public static double s_taiou適応力が対応率に比例0_010 = 0.010;
        public static double s_taiou対応率MAX70 = 70.0;

        public static double s_syutyu集中力が集中率に比例0_020 = 0.020;
        public static double s_syutyu集中力MAX90 = 90.0;

        static double s_senzyu思考力が戦術に比例0_10 = 0.10;

        /// <summary>
        /// 現在は攻撃力以外の防御力や回避率やクリティカル率のマス目増加時に適応。攻撃力は別の式を使っている。
        /// </summary>
        static double[] s_DiceNumAddBonusダイスマス分割数増加による合計値ボーナス率 = new double[] { 1.0, 1.4, 1.9, 2.5, 4.2, 6.0 }; //new List<double>(new double[] { 1.0, 1.4, 1.9, 2.5, 4.2, 6.0 }); // (b)固定0.5+0.1*マス目ずつ増加 //※(a)1.5倍ずつした時 { 1.0, 1.5, 2.25, 3.375, 5.0625, 7.59375 }
        /// <summary>
        /// 現在は攻撃力は初めから４マスあるので、４マス[3]=1.0。マス目を増やしても増加率は変わらず、マス目を減らすと徐々に増加するようにしている。
        /// </summary>
        static double[] s_AtackDiceNumAddBonus攻撃マス分割数増加による合計値ボーナス率 = new double[] { 2.0, 1.6, 1.2, 1.0, 1.0, 1.0 };
            
        #region パラメータの比例関係の考察
        // [PARA][Excel]【PARA】　近似関数の求め方は，ソルバーでやるのが一番早いよ！    
        // （詳しくは C:\MerusaiaDocuments\Documents\Game\●ゲーム創作活動\Excel数値解析表（ソルバーなど） のxlsファイルをみてね）

        // 【Parameter】　パラメータの比例方法の決め方
        //  ※参考 「人間の五感は対数変換している」 http://www.rd.mmtr.or.jp/~bunryu/5kanlog.shtml
        // クリティカル率のような，
        // x=50のときにy=5％，x=1000のときにy=30％，というような対数比例関係のある値を計算したい場合，
        // つまり，f(50)=5で，かつf(1000)=30となる，関数f(x)を近似する式はどう求める？？
        //
        //    ●案１：　指数関数 y=b^x で近似
        //    ・・・累乗数bの指数関数　y = b^xとおくと， 30=b^1000，という式になる．
        //    ・・・このbをどう求めるかというと，
        //　　・・・例えば，3の2乗が9であれば，乗数を逆数にして，9の(1/2)乗が3である，という法則から求める．
        //　　・・・つまり，30=b^1000から，b=30^(1/1000)とかける．
        //　　・・・これを計算すると，b=30^(1/1000)=pow(30,1/1000)=1.0034069880166463より，
        //　　・・・すなわち，y=1.0034069880166463^xで近似できる．確め算は，f(1000)=29.99999999999842なので，合ってる．
        //　　・・・ただし，もうひとつのxの時のyは，f(50)=1.185375816559305なので，=5には少し遠い．
        //　　・・・（ここで，≒1.003とするとダメ！　pow(1.003,1000)=19，pow(1.0034,1000)=29で，全く違う意味を持つ）
        //　　・・・＜以下，使わない＞
        // 　 ・・・y=b^x逆関数である，x=log_b(y)と式が等価であることから，
        //　　・・・log_b(y)=log_2(y)/log_2(b)=x，log_2(b)=log_2(y)/xで，log_2(b)を対数表／関数電卓を使って調べることになる．
        //　　・・・（log_a(b) = log_c(b) / log_c(a) （低cはなんでも同じ）より）
        //　　・・・f(30)=1000の場合，log_2(b)=log_2(1000)/30で，関数電卓からlog_2(1000)=ln2(1000)=0.6931471805599453より，
        //　　・・・0.6931471805599453/30 = 0.023104906018664842=log_2(b)であるbは，？？？
        //    ・・・b = log_1000(30) = log(30) / log(1000) = 0.4923737515732209 ≒ 0.50
        //　　・・・すなわち，y = x^(0.5) = √x で近似できる．f(50)=√50=7.07 なので，=5には少し近い．
        //　　・・・なお，f(50)=5を満たすbは，log(5)/log(50)=0.41..で，f(1000)=pow(1000,0.41)=16.98..なので，=30には遠い．
        //　　・・・＜以上，使わない＞
        //
        //    ●案２：　対数関数 y=log_a(x) で近似
        //　　・・・基底aの対数関数 y = log_a(x)とおくと，1000=log_a(30)という式になる．
        //    ・・・このaは，
        //    ・・・ 案１の逆で，逆関数である指数関数で，1000 = a^30，つまり，a = 1000^(1/30)で計算できる．
        //    ・・・（　　y=log_a(x)のとき，　　　xはaのy乗  => aはxの(1/y)乗なので，　　a=x^(1/y)より，）
        //　　・・・例：　y=log_2(1024)のとき，1024は2の10乗 => 2は1024の(1/10)乗なので，2=1024^(1/10)より，
        //　　・・・a=(1000)^(1/30) = pow(1000,1/30) = 1.2589254117941673 = 1.25，となる．
        //　　・・・（※ここで，≒1.2 とするとダメ！ log_1.2(x)=37で，log_1.25(x)=30で，全く違う意味を持つ）
        //　　・・・すなわち，y=log_1.25(x) で近似できる．
        //　　・・・ただし，もうひとつのxの時のyは，f(50)=log_1.25(50)=log(50)/log(1.25)=17.5..なので，=5には程遠い．
        //　　・・・なお，f(50)=5を満たすbは，pow(50,1/5)=2.18..で，
        //　　・・・すなわち，y=log_2.18(x)で近似できる．
        //　　・・・ただし，もうひとつのxの時のyは，ただし，f(1000)=log(1000)/log(2.18)=8.86..なので，=30には程遠い．
        //
        //    ■案１と案２の違い
        //　　・・・グラフを書いてみるとすぐわかるだろう．
        //　　・・・案１の指数関数 x^b      のグラフは，凹の右側（xが後ろに行くほど急，0<b<1のときは緩やか）のに対し，
        //　　・・・案２の対数関数 log_a(x) のグラフは，凸の右側（xが後ろに行くほど非常に緩やか，x=1のとき0）だ．
        //　　・・・すなわち，「値が小さい時は変化が薄くしたい（案１）」or「大きい時に変化を薄くしたい（案２）」
        //　　・・・かを判断基準として，使い分ければよい．
        //　　・・・例：　クリティカルの場合は，
        //　　・・・　　　小さい時は5～20％と一気に上がるが，大きい時は30％くらいに停滞させたいから，案２が適切
        //　　・・・つまり，まとめると
        //      ・・・
        //　　・・・○比例関数 = 「xが上がるのと同じようにyが変わる変数」は，【比例関数 y=ax or y=ax^2+bx,.. で近似】
        //　　・・・　　　　　　　・ちからと攻撃力，攻撃力とダメージ（DQは比例，FFは指数関数），持久力とHP？
        //　　・・・
        //　　・・・●逆関数   = 「xが上がるほどyは急激に下がり0に収束していく変数」は，【逆関数 y=a/x or y=a/x^2+b/xで近似】
        //　　・・・　　　　　　　・割る数が変化する除算，金持ちと貧乏の所持金，周波数と周期，割り算する値が変わる関数？
        //　　・・・
        //　　・・・☆指数関数 = 「xが上がるほどyもエスカレーションしていく変数」は，【対数関数 y=x^a で近似】
        //　　・・・　　　　　　　・LVとダメージやパラメータ（特にHP，素早さ？，精神力？，人間の五感？），消費コスト，
        //　　・・・　　　　　　　・ゲーム進行度（達成率，継続時間）とランキング(獲得ポイント)，経験値，お金(G)，，
        //　　・・・
        //　　・・・★対数関数 = 「xが上がるほどyは緩やかになる，収束する変数（例：確率）」は，【対数関数 y=log_a(x)で近似】
        //　　・・・　　　　　　　・普段発生するべき率（命中率，エラー率，認識率，再現率，もっともらしさ（尤度）
        //　　・・・　　　　　　　　　　　　　 　　　　 熟練率，習得率など）」や，
        //　　・・・　　　　　　  ・レアな事象発生確率（回避率，クリティカル率，突然変異確率など）
        //　　・・・それぞれの関数で近似すればよいだろう．
        //　　・・・まさか・・・(1-普段確率 = レア確率)にならないよね？　グラフを描いてみたらわかる？
        //
        //    ●案３：　２つのf(a)とf(b)を満たす，近似関数を，最小二乗法？多項式？正規分布？アトラクター重畳？で近似？
        //　　・・・f(50)=5ではなく，
        //　　・・・f(50)=1で，かつf(1000)=30を満たすある関数で近似するやり方も，賢い方法があるはず？？
        // 　例：　最小二乗法の場合，
        // 　　　　　　○比例関数　y=ax+b とおくため，
        // 　　　　　　近似直線からのズレΔy = (1/2) * Σ_n=1toN(|ax+b-y|^2) が最小になるa，bを求めるので，
        // 　　　　　　_y1=f(_x1)，_y2=f(_x2)，...，yn=f(xn)のN個のデータを通る近似直線を求める式は，
        //　　　　　　（・・ややこしい偏微分を用いた理論・・より，）
        //　　　　　　　正規方程式という，以下のaとbの連立1次方程式を解けばよいようだ．
        //　　　　　　　　(1) aΣ(x^2) + bΣ(x) = Σxy
        //　　　　　　　　(2) aΣ(x)   + bΣ(1) = Σy
        //              そしてこれのaとbの解は，wikipediaより，
        //                    (3) a = (nΣ(xy) - Σ(x)Σ(y))   / (nΣ(x^2)-(Σ(x))^2)
        //                    (4) b = (Σ(x^2)Σy - Σ(xy)Σx) / (nΣ(x^2)-(Σ(x))^2)
        //　　　　　　　　となるらしい．
        //
        // 　　　　　　とりあえず，N=2の例でやってみると，
        // 　　　　　　　　1=50a+b，30=1000a+bなので，
        //　　　　　　　　 （Δyでは，Δy = (1/2) * ( (50a+b-1)^2 + (1000a+b-1)^2 ) = ... で計算が面倒．．）
        //                  (1)と(2)に代入してみると，
        //　　　　　　　　(1) a*(2500+1000000) + b*(50+1000) = 50*1000+1*30
        //                (2) a*(1050)         + b*(2)       = 31
        // 　　　　　　　  => (1)整理，(2)の両辺に(1050/2)をかけて，てそれぞれ(1)'，(2)'とすると，
        //　　　　　　　　(1)'a*(102500) + b*(1050) = 50030
        //　　　　　　　　(2)'a*(551250) + b*(1050) = 16275
        // 　　　　　　　 ==============================================
        // 　　 　  (1)'-(2)' a*(451250)            = 33755
        //　　　　　　　　　　a = 33755/451250      = 0.07480332409972299 ≒ 0.074
        //    (2)にaを代入し，b = (31-(0.07480332409972299)*1025)/2 = -22.836703601108027   ≒ -22
        //    (1)'にaとbを代入して確かめると，
        //                  (0.07480332409972299)*(1002500)+(-22.836703601108027)*(1050)=51011.79362880887
        //     で，確かに近似した値になっている．
        //     よって，a=0.074，b=-22として，
        //     y=0.074x-22で近似すればよい．
        //    （確認） f(50) = 0.074*50-22 = -18.3，　f(1000)= 0.074*1000-22 = 52・・・て，あれ？全然ダメじゃん．
        // 　　この(1)(2)が間違ってるか，(1)'(2)'の導出過程で何か間違ってる？？
        // 
        //     ということで，もうひとつの(3)(4)でも同じ結果になるか試しておくと，203948
        //                   (3) a = (2*50030 - 1050*31) / (2*102500-(1050)^2) = 0.33101574911251885
        //                   (4) b = (102500*31 - 50030*1050) / (2*102500-(1050)^2) = -241.9930570537588
        //     なんか全然違う・・・，a=0.33，b=-240となった．
        //     （確認）f(50) = 0.33*50-240 = -223.5，　f(1000)= 0.33*1000-240 = 90・・・て，あれ？こっちもダメじゃん．
        //  　　あぁ～～，もう頭限界（AM3:30・・・）一旦止めます;_;．．．
        //
        //   ■wikipediaには，一応多項式関数での近似もできるようなことが書いてあるが・・・
        #endregion

        #region ＬＶＵＰ処理: LVUP***
        static double s_LV1UP毎の能力上昇率 = 1.0;
        /// <summary>
        /// ●キャラのレベルアップ処理（自動的に能力が上がる場合）をメソッド化したものです．
        /// キャラのパラ上昇値（基本6色パラメータ総合値の増加量）を返します。
        /// 引数にレベルアップ数（-1などでレベルダウン処理も可能）を指定します。
        /// レベルをセットする処理も兼ねるので、新しくLV1のキャラを創った時も必ず一度は(引数の _LVUP数=0 で)呼び出してください。
        /// 
        /// 
        /// 　　※なお、ＬＶＵＰエフェクト（文字や効果音）出力やボーナスポイントの振り分け処理は、このメソッドにはありません。
        /// 基本的には、game.LVUP・レベルアップ()やgame.userSetCharaPara・キャラのパラメータを調整()などを使ってください。
        /// 
        /// </summary>
        /// <param name="_chara"></param>
        /// <param name="_LVUP数"></param>
        /// <returns></returns>
        public static int LVUP・レベル設定(CChara・キャラ _chara, double _LVUP数)
        {
            // [MEMO][LV]拡張性のため、_LVUP数にはdouble型を使っているが、現段階ではLVは1.0毎にしか上がらないとする
            // (int)型に切り捨て
            _LVUP数 = (double)((int)_LVUP数);
            int _新LV = (int)(_chara.Para(EPara.LV) + _LVUP数);
            // LVをセット
            _chara.setParaValue(EPara.LV, _新LV);
            // [Memo]名前の語尾にＬＶを付けテストはもうやめた。
            // 現状テスト版_名前（_chara.name名前()だけ）の語尾にLVを追加，Var(EPara.名前)は名前のみ
            //●_chara.setName・名前を一時的に変更(_chara.Var(EVar.名前) + "LV" + _chara.Para(EPara.LV).ToString());

            // 返り値
            int _AddParaSum_Iro6・ＬＶＵＰ後のパラ上昇値 = 
                getLVUPAddPara_Iro6・キャラＬＶＵＰ時の基本6パラ上昇値を取得(_chara, _LVUP数);
           

            List<double> _6色パラ; // 基本６色、中間６色、を代入するための変数。装飾６色はＶＵＰで上昇しない。
            List<double> _6色上昇値;
            _6色パラ = _chara.Paras・パラメータ一括処理().getIro6基本６色パラメータ();
            _6色上昇値 = getAddingParaList・パラ上昇合計値から各色上昇リストを取得＿各色に比重比例(
                _AddParaSum_Iro6・ＬＶＵＰ後のパラ上昇値, _6色パラ);
            _chara.Paras・パラメータ一括処理().setIro6基本６色パラメータを変更(_6色パラ, ESet.add・増減値, _6色上昇値);
            _6色パラ = _chara.Paras・パラメータ一括処理().getIroMID6中間６色パラメータ();
            _6色上昇値 = getAddingParaList・パラ上昇合計値から各色上昇リストを取得＿各色に比重比例(
                _AddParaSum_Iro6・ＬＶＵＰ後のパラ上昇値, _6色パラ);
            _chara.Paras・パラメータ一括処理().setIroMID6中間６色パラメータを変更(_6色パラ, ESet.add・増減値, _6色上昇値);

            // もしLV1だったら、LV1時の各色パラと総合値の更新
            if (_新LV == 1)
            {
                setLV1Paras・LV1時の18色パラメータと総合値をセット(_chara);
            }
            // 総合値を更新
            setParas・現在のLVの18色パラメータと総合値をセット(_chara);

            // ダイスコマンドの作り直し（各種ダメージなどを更新）
            //（今はここではやらず、戦闘開始時だけやってる）createDiceCommand_FromParas・ダイスコマンドを自動生成(_chara);

            // [ブレークポイント]キャラのパラが代入されているかを確認
            Program・実行ファイル管理者.printlnLog(MyTools.getListValues_ToCSVLine("■" + _chara.name名前() + "LV" + _chara.Para(EPara.LV) + "の基本6色パラ", _chara.Paras・パラメータ一括処理().getIroParas基本色6パラメータint(), true));

            return _AddParaSum_Iro6・ＬＶＵＰ後のパラ上昇値;
        }
        /// <summary>
        /// ■ＬＶＵＰ時のパラメータ上昇値を決定しているメソッドです。
        /// キャラの現在のＬＶ＋指定した_LVUPNumになった時の、基本6色パラメータ総合値（赤～紫）の上昇値を返します。
        /// ※現段階では、ＬＶによらず、ＬＶ１時の6パラ総合値×比例定数 になっています。
        /// なお、キャラに成長曲線が設定されている場合は、LVによって比例定数が代わる場合があります。
        /// </summary>
        public static int getLVUPAddPara_Iro6・キャラＬＶＵＰ時の基本6パラ上昇値を取得(CChara・キャラ _chara, double _LVUPNum)
        {
            // ■パラメータ上昇値の決定
            // [TODO]レベルアップ処理のパラ上昇値は本当にこれでいいのか？
            // 現段階では、ＬＶ１時の6パラ総合値 × 1 になっています。
            //  つまり、LV○時のパラメータ = LV1時の6パラ総合値 × LV倍
            double _addParaSum・LVＵＰ時のパラ上昇値 =
                _chara.Para(EPara.LV1c_基本6色総合値) * s_LV1UP毎の能力上昇率;
            // 上昇値が変な値（以上、一応マイナスの値も-1000万まで考慮）の場合は、最大値を代入
            double _Max = 10000000;
            if (_addParaSum・LVＵＰ時のパラ上昇値 < -1 * _Max) _addParaSum・LVＵＰ時のパラ上昇値 = -1 * _Max;
            if (_addParaSum・LVＵＰ時のパラ上昇値 > _Max) _addParaSum・LVＵＰ時のパラ上昇値 = _Max;
            // 四捨五入した整数の値を返す。
            return MyTools.getSisyagonyuValue(_addParaSum・LVＵＰ時のパラ上昇値);
        }
        /// <summary>
        /// 第一引数に指定したパラ上昇値の合計値を、第二引数で指定した６色パラの色の重み（比重）に比例するように分割した、各色毎のパラメータ上昇値を返します。
        /// ※6色パラだけでなく、18色パラや、N個のパラメーター比例上昇にも使えます。
        /// </summary>
        private static List<double> getAddingParaList・パラ上昇合計値から各色上昇リストを取得＿各色に比重比例(int _パラ上昇値の合計値, List<double> _前の色パラリスト)
        {
            int _addParaSum_Iro6 = _パラ上昇値の合計値;
            int _paraNum = _前の色パラリスト.Count;
            List<double> _addingParaList・各色上昇値リスト = new List<double>(_paraNum);

            if (_addParaSum_Iro6 != 0.0)
            {
                int i = 0;
                double _色重み_6色における各色の占める割合 = 0;
                int _上昇値 = 0;
                int _残り上昇値 = _addParaSum_Iro6;
                double _前の6色パラ総合値 = MyTools.getSum(_前の色パラリスト);
                foreach (double _para in _前の色パラリスト)
                {
                    // パラメータの割合だけそれぞれ振り分け
                    _色重み_6色における各色の占める割合 = _para / (double)_前の6色パラ総合値;
                    _上昇値 = (int)(_色重み_6色における各色の占める割合 * _addParaSum_Iro6);
                    _残り上昇値 -= _上昇値;
                    _addingParaList・各色上昇値リスト.Add((double)_上昇値);
                    // 振り分けがおわるのに残っていたら，均等に振る
                    if (i == _paraNum - 1 && _残り上昇値 > 0)
                    {
                        for (int x = 0; x <= _paraNum - 1; x++)
                        {
                            // 残った分は，_上昇基本値 = _addParaSum_Iro6 / 6
                            _addingParaList・各色上昇値リスト[x] += (double)(_残り上昇値 / (_paraNum - 1));
                        }
                    }
                    i++;
                }
            }
            else
            {
                // 上昇値の合計値が0の場合、全て0のリストを作成して返す。
                for (int i = 0; i <= _paraNum - 1; i++)
                {
                    _addingParaList・各色上昇値リスト.Add(0.0);
                }
            }
            return _addingParaList・各色上昇値リスト;

            #region 他のLVUP方法のメモ
            // 現在は線形増加

            // 倍数的増加。以下だと総合値や攻撃パラメータまで同じように定数倍されてしまう．
            //Array _array = Enum.GetValues(typeof(EPara));
            //foreach (object _para in _array)
            //{
            //    _chara.setPara((EPara)_para, ESet._rate・倍率, _上昇倍);
            //}

            // 指数関数的増加。以下だと強いパラが強くなり過ぎる
            // double _上昇倍 = Math.Pow(1.2, _LVUP数);
            // Array _array = MyTools.getEnumArray(EIro18Para・色パラメータ.c01_赤);
            // foreach (object _para in _array)
            // {
            //     _chara.setPara((EIro18Para・色パラメータ)_para, ESet._rate・倍率, _上昇倍);
            // }
            #endregion
        }
        #endregion

        #region ＬＶＵＰメソッドが呼び出す、色パラメータの総合値のセットメソッド: setLV***
        /// <summary>
        /// // ■■LV1時の色パラメータとその総合値を保存する、setPara連続の面倒な処理をメソッド可したものです。
        /// ※このメソッドを呼び出す前に、setIroParas***で色パラメータをセットしてください。でないと、全てに0（か初期値）がセットされます。
        /// </summary>
        /// <param name="_chara"></param>
        private static void setLV1Paras・LV1時の18色パラメータと総合値をセット(CChara・キャラ _c)
        {
            List<double> _iro6・基本色６パラ = _c.Paras・パラメータ一括処理().getIro6基本６色パラメータ();
            List<double> _iroMID6・中間色６パラ = _c.Paras・パラメータ一括処理().getIroMID6中間６色パラメータ();
            List<double> _iroEXT6・装飾色６パラ = _c.Paras・パラメータ一括処理().getIroEXT6装飾６色パラメータ();
            setLV1Paras・LV1時の18色パラメータと総合値をセット(_c, _iro6・基本色６パラ, _iroMID6・中間色６パラ, _iroEXT6・装飾色６パラ);
        }
        /// <summary>
        /// // ■■LV1時の色パラメータとその総合値を保存する、setPara連続の面倒な処理をメソッド可したものです。
        /// ※引数に設定したいLV1時のパラメータを指定してください。nullを代入すると0（か初期値）が入りします。
        /// </summary>
        /// <param name="_chara"></param>
        private static void setLV1Paras・LV1時の18色パラメータと総合値をセット(CChara・キャラ _c, List<double> _iro6・基本色６パラ, List<double> _iroMID6・中間色６パラ, List<double> _iroEXT6・装飾色６パラ)
        {
            if (_iro6・基本色６パラ == null) { _iro6・基本色６パラ = _c.Paras・パラメータ一括処理().getIro6基本６色パラメータ(); }
            if (_iroMID6・中間色６パラ == null) { _iroMID6・中間色６パラ = _c.Paras・パラメータ一括処理().getIroMID6中間６色パラメータ(); }
            if (_iroEXT6・装飾色６パラ == null) { _iroEXT6・装飾色６パラ = _c.Paras・パラメータ一括処理().getIroEXT6装飾６色パラメータ(); }

            _c.setPara(EPara.LV1c01_赤, ESet._default・代入値, _iro6・基本色６パラ[0]);
            _c.setPara(EPara.LV1c03_橙, ESet._default・代入値, _iro6・基本色６パラ[1]);
            _c.setPara(EPara.LV1c05_黄, ESet._default・代入値, _iro6・基本色６パラ[2]);
            _c.setPara(EPara.LV1c07_緑, ESet._default・代入値, _iro6・基本色６パラ[3]);
            _c.setPara(EPara.LV1c09_青, ESet._default・代入値, _iro6・基本色６パラ[4]);
            _c.setPara(EPara.LV1c11_紫, ESet._default・代入値, _iro6・基本色６パラ[5]);
            _c.setPara(EPara.LV1c02_赤橙, ESet._default・代入値, _iroMID6・中間色６パラ[0]);
            _c.setPara(EPara.LV1c04_黄橙, ESet._default・代入値, _iroMID6・中間色６パラ[1]);
            _c.setPara(EPara.LV1c06_黄緑, ESet._default・代入値, _iroMID6・中間色６パラ[2]);
            _c.setPara(EPara.LV1c08_青緑, ESet._default・代入値, _iroMID6・中間色６パラ[3]);
            _c.setPara(EPara.LV1c10_青紫, ESet._default・代入値, _iroMID6・中間色６パラ[4]);
            _c.setPara(EPara.LV1c12_赤紫, ESet._default・代入値, _iroMID6・中間色６パラ[5]);
            _c.setPara(EPara.LV1c13_白, ESet._default・代入値, _iroEXT6・装飾色６パラ[0]);
            _c.setPara(EPara.LV1c14_黒, ESet._default・代入値, _iroEXT6・装飾色６パラ[1]);
            _c.setPara(EPara.LV1c15_銀, ESet._default・代入値, _iroEXT6・装飾色６パラ[2]);
            _c.setPara(EPara.LV1c16_金, ESet._default・代入値, _iroEXT6・装飾色６パラ[3]);
            _c.setPara(EPara.LV1c17_透明, ESet._default・代入値, _iroEXT6・装飾色６パラ[4]);
            _c.setPara(EPara.LV1c18_虹色, ESet._default・代入値, _iroEXT6・装飾色６パラ[5]);

            // LV1時の総合値代入処理
            double _基本6色パラ総合値 = MyTools.getSum(_iro6・基本色６パラ);
            _c.setPara(EPara.LV1c_基本6色総合値, ESet._default・代入値, _基本6色パラ総合値);
            double _中間6色パラ総合値 = MyTools.getSum(_iroMID6・中間色６パラ);
            _c.setPara(EPara.LV1c_中間6色総合値, ESet._default・代入値, _中間6色パラ総合値);
            double _装飾6色パラ総合値 = MyTools.getSum(_iroEXT6・装飾色６パラ);
            _c.setPara(EPara.LV1c_装飾6色総合値, ESet._default・代入値, _装飾6色パラ総合値);
            double _全色パラ総合値 = _基本6色パラ総合値 + _中間6色パラ総合値 + _装飾6色パラ総合値;

            _c.setPara(EPara.LV1c_色18色総合値, ESet._default・代入値, _全色パラ総合値);
        }
        /// <summary>
        /// // ■■現在のLVの18色パラメータとその総合値（基本色6パラ、中間色6パラ、装飾色6パラ）を更新します。
        /// ※パラメータをセットしたら、必ずこのメソッドを呼び出して総合値の合計値も更新してください。
        /// </summary>
        /// <param name="_chara"></param>
        private static void setParas・現在のLVの18色パラメータと総合値をセット(CChara・キャラ _c)
        {
            List<double> _iro6・基本色６パラ = _c.Paras・パラメータ一括処理().getIro6基本６色パラメータ();
            List<double> _iroMID6・中間色６パラ = _c.Paras・パラメータ一括処理().getIroMID6中間６色パラメータ();
            List<double> _iroEXT6・装飾色６パラ = _c.Paras・パラメータ一括処理().getIroEXT6装飾６色パラメータ();
            // ●●●色パラメータを代入
            //_chara.Paras・パラメータ一括処理().setIroParas基本色6パラメータを代入(_iro6・基本色６パラ);
            //_chara.Paras・パラメータ一括処理().setIroParas中間色6パラメータを代入(_iroMID6・中間色６パラ);
            //_chara.Paras・パラメータ一括処理().setIroParas装飾色6パラメータを代入(_iroEXT6・装飾色６パラ);
            // ●●●身体パラを基本色パラ，精神パラを中間色パラとして代入
            _c.Paras・パラメータ一括処理().setShintaiParas・身体パラメータをセット(_iro6・基本色６パラ);
            _c.Paras・パラメータ一括処理().setSeishinParas・精神パラメータをセット(_iroMID6・中間色６パラ);

            // 総合値代入処理
            double _基本6色パラ総合値 = MyTools.getSum(_iro6・基本色６パラ);
            _c.setPara(EPara._基本6色総合値, ESet._default・代入値, _基本6色パラ総合値);
            double _中間6色パラ総合値 = MyTools.getSum(_iroMID6・中間色６パラ);
            _c.setPara(EPara._中間6色総合値, ESet._default・代入値, _中間6色パラ総合値);
            double _装飾6色パラ総合値 = MyTools.getSum(_iroEXT6・装飾色６パラ);
            _c.setPara(EPara._装飾6色総合値, ESet._default・代入値, _装飾6色パラ総合値);
            double _全色パラ総合値 = _基本6色パラ総合値 + _中間6色パラ総合値 + _装飾6色パラ総合値;
            _c.setPara(EPara._18色総合値, ESet._default・代入値, _全色パラ総合値);
        }
        #endregion
        
        #region 以下、キャラのパラメータ更新処理: setCharaPara*** / getDividedAddPara***
        
        /// <summary>
        /// ■キャラの現在の色パラメータに、第二引数に指定した自動的に新しく振り分けるパラメータ（合計値）を、
        /// 第三引数に指定した分割方法で、それぞれの色パラメータを増加させます。
        /// 
        /// ※なお、パラメータ調整画面はこのクラスは管理しません。
        /// パラメータ調整画面を出したい場合は、game.userSetCharaPara・キャラのパラメータを調整()メソッドを呼び出してください。
        /// </summary>
        public static void setCharaPara・キャラのパラメータを調整(CChara・キャラ _chara, int _addParaNum・パラメータ振り分け値, EParaDividedType・パラ分割方法 _EDividedType・分割方法)
        {
            // 指定した分割方法（振り分け方法）で、増加パラメータリストを取得
            List<double> _addParaList = getDividedAddParaToParaLists・増加パラメータを指定した方法で色パラリストに分割
                (_chara, _addParaNum・パラメータ振り分け値, _EDividedType・分割方法);
            // キャラの色パラメータを、分割方法にリスト形式を合わせて、増加させる。
            switch (_EDividedType・分割方法)
            {
                // 基本6色の調整の時
                case EParaDividedType・パラ分割方法._default_EqualToIro6・基本６色パラに６等分割:
                    _chara.Paras・パラメータ一括処理().setIro6基本６色パラメータを変更(_chara.Paras・パラメータ一括処理().getIro6基本６色パラメータ(), ESet.add・増減値, _addParaList);
                    break;
                case EParaDividedType・パラ分割方法.RateToIro6・基本６色パラに比例分割:
                    goto case EParaDividedType・パラ分割方法._default_EqualToIro6・基本６色パラに６等分割;
                // 中間6色の調整の時
                case EParaDividedType・パラ分割方法.RateToIroMID6・中間６色パラに比例分割:
                    _chara.Paras・パラメータ一括処理().setIroMID6中間６色パラメータを変更(_chara.Paras・パラメータ一括処理().getIroMID6中間６色パラメータ(), ESet.add・増減値, _addParaList);
                    break;
                // 装飾6色の調整の時
                case EParaDividedType・パラ分割方法.RateToIroEXT6・装飾６色パラに比例分割:
                    _chara.Paras・パラメータ一括処理().setIroEXT6装飾６色パラメータを変更(_chara.Paras・パラメータ一括処理().getIroEXT6装飾６色パラメータ(), ESet.add・増減値, _addParaList);
                    break;
                // 12色の調整
                case EParaDividedType・パラ分割方法.EqualToIro12・１２等分割:
                    _chara.Paras・パラメータ一括処理().setIroParas色パラメータを変更(_chara.Paras・パラメータ一括処理().getIro12色パラメータ(), ESet.add・増減値, _addParaList);
                    break;
                case EParaDividedType・パラ分割方法.RateToIro12・１２色パラに比例分割:
                    goto case EParaDividedType・パラ分割方法.EqualToIro12・１２等分割;
                // 18色の調整
                case EParaDividedType・パラ分割方法.EqualToIro18・１８等分割:
                    _chara.Paras・パラメータ一括処理().setIroParas色パラメータを変更(_chara.Paras・パラメータ一括処理().getIro18色パラメータ(), ESet.add・増減値, _addParaList);
                    break;
                case EParaDividedType・パラ分割方法.RateToIro18・１８色パラに比例分割:
                    goto case EParaDividedType・パラ分割方法.EqualToIro18・１８等分割;
            }
            // 最後に色パラメータ総合値をセット。
            setParas・現在のLVの18色パラメータと総合値をセット(_chara);
        }
        /// <summary>
        /// 引数１のキャラに、引数２の増加させるパラメータ振り分け値を、引数３に指定した方法で分割した、
        /// 色パラメータの増加値をリスト化したものを返します。
        /// </summary>
        public static List<double> getDividedAddParaToParaLists・増加パラメータを指定した方法で色パラリストに分割(
            CChara・キャラ _chara, int _addParaNum・パラメータ振り分け値, EParaDividedType・パラ分割方法 _EDividedType・分割方法)
        {
            // 分割後のパラメータリスト
            List<double> _paraList = new List<double>();
            // 分割数
            int _dividedNum = 0;
            // リストに追加されるパラメータ値
            double _paraValue = 0;
            // リストに追加されるパラメータ値を計算する時に使う、6/12/18色パラメータにおける現在の色の比率
            double _paraRate = 0;
            switch (_EDividedType・分割方法)
            {
                case EParaDividedType・パラ分割方法._default_EqualToIro6・基本６色パラに６等分割:
                    _dividedNum = 6;
                    _paraValue = _addParaNum・パラメータ振り分け値 / _dividedNum;
                    for (int i = 0; i < _dividedNum; i++)
                    {
                        _paraList.Add(_paraValue);
                    }
                    break;
                case EParaDividedType・パラ分割方法.EqualToIro12・１２等分割:
                    _dividedNum = 12;
                    _paraValue = _addParaNum・パラメータ振り分け値 / _dividedNum;
                    for (int i = 0; i < _dividedNum; i++)
                    {
                        _paraList.Add(_paraValue);
                    }
                    break;
                case EParaDividedType・パラ分割方法.EqualToIro18・１８等分割:
                    _dividedNum = 18;
                    _paraValue = _addParaNum・パラメータ振り分け値 / _dividedNum;
                    for (int i = 0; i < _dividedNum; i++)
                    {
                        _paraList.Add(_paraValue);
                    }
                    break;
                case EParaDividedType・パラ分割方法.RateToIro6・基本６色パラに比例分割:
                    _dividedNum = 6;
                    for (int i = 0; i < _dividedNum; i++)
                    {
                        _paraRate = MyTools.getListValue<double>(_chara.Paras・パラメータ一括処理().getIro6基本６色パラメータ(), i)
                            / _chara.Para(EPara._基本6色総合値);
                        _paraValue = _addParaNum・パラメータ振り分け値 * _paraRate;
                        _paraList.Add(_paraValue);
                    }
                    break;
                case EParaDividedType・パラ分割方法.RateToIroMID6・中間６色パラに比例分割:
                    _dividedNum = 6;
                    for (int i = 0; i < _dividedNum; i++)
                    {
                        _paraRate = MyTools.getListValue<double>(_chara.Paras・パラメータ一括処理().getIroMID6中間６色パラメータ(), i)
                            / _chara.Para(EPara._中間6色総合値);
                        _paraValue = _addParaNum・パラメータ振り分け値 * _paraRate;
                        _paraList.Add(_paraValue);
                    }
                    break;
                case EParaDividedType・パラ分割方法.RateToIroEXT6・装飾６色パラに比例分割:
                    _dividedNum = 6;
                    for (int i = 0; i < _dividedNum; i++)
                    {
                        _paraRate = MyTools.getListValue<double>(_chara.Paras・パラメータ一括処理().getIroEXT6装飾６色パラメータ(), i)
                            / _chara.Para(EPara._装飾6色総合値);
                        _paraValue = _addParaNum・パラメータ振り分け値 * _paraRate;
                        _paraList.Add(_paraValue);
                    }
                    break;
                case EParaDividedType・パラ分割方法.RateToIro12・１２色パラに比例分割:
                    _dividedNum = 12;
                    for (int i = 0; i < _dividedNum; i++)
                    {
                        _paraRate = MyTools.getListValue<double>(_chara.Paras・パラメータ一括処理().getIro12色パラメータ(), i)
                            / (_chara.Para(EPara._基本6色総合値)+_chara.Para(EPara._中間6色総合値));
                        _paraValue = _addParaNum・パラメータ振り分け値 * _paraRate;
                        _paraList.Add(_paraValue);
                    }
                    break;
                case EParaDividedType・パラ分割方法.RateToIro18・１８色パラに比例分割:
                    _dividedNum = 18;
                    for (int i = 0; i < _dividedNum; i++)
                    {
                        _paraRate = MyTools.getListValue<double>(_chara.Paras・パラメータ一括処理().getIro18色パラメータ(), i)
                            / _chara.Para(EPara._18色総合値);
                        _paraValue = _addParaNum・パラメータ振り分け値 * _paraRate;
                        _paraList.Add(_paraValue);
                    }
                    break;
                default:
                    break;
            }
            return _paraList;
        }
        #endregion



        #region キャラ名リストの取得メソッド: getCharaNameList***
        /*
        /// <summary>
        /// ●キャラクタデータベースから，ゲストキャラ名のリストを取得します．
        /// </summary>
        public static List<string> getCharaNameList・キャラ名リストを取得()
        {
            List<string> _charaNameList = p_guestReader.GetNameList・参照名リストを取得();
            // 基本パラの項目がないキャラは消去
            for(int i=0; i<=_charaNameList.Count - 1; i++){
                if (p_guestReader.GetInfoLine・参照名に該当する一行の資源データを取得(_charaNameList[i]).p3to_option・列リスト[p_iroParaSumIndex・基本色6総合値の列] == "")
                {
                    _charaNameList.RemoveAt(i);
                    i--; // 削除した分だけずらす
                }
            }
            return _charaNameList;
        }
        */
        // 一回取得したら高速に取得できるように、もっとく。これで、各キャラタイプのキャラ数も.Countで取れるよ
        private static List<string> p_charaNameList_All・全種キャラ名のリスト;
        private static List<string> p_charaNameList_Guest・ゲストキャラ名リスト;
        private static List<string> p_charaNameList_Bourei・亡霊キャラ名リスト;
        private static List<string> p_charaNameList_Player・プレイヤーキャラ名リスト;
        /// <summary>
        /// ●キャラクタデータベースから，プレイヤーキャラ名のリスト（項目名はユニークな参照名、つまり名前だけ）を取得します。
        /// </summary>
        /// <returns></returns>
        public static List<string> getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類 _キャラタイプ)
        {
            return getCharaNameList・キャラ名リストを取得(_キャラタイプ, false, false, -1);
        }
        /// <summary>
        /// ●キャラクタデータベースから，プレイヤーキャラ名のリストを、指定したインデックスのパラメータと共に取得します。
        /// </summary>
        /// <returns></returns>
        public static List<string> getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類 _キャラタイプ, bool _isランダムソート, bool _isShowEnblem・称号表示, params int[] _リストに表示するパラメータのインデックス_必要ない場合はマイナス1だけ引数にする)
        {
            List<string> _charaNameList = null;

            int[] _paras = _リストに表示するパラメータのインデックス_必要ない場合はマイナス1だけ引数にする;
            bool _isFirst初めてリストを作成 = false;

            // パラメータを指定された時、リスト編集が必要
            bool _isListTextEdit・デフォルトと違うリスト編集が必要 = false;
            if(_paras == null) _isListTextEdit・デフォルトと違うリスト編集が必要 = false;
            if(_paras != null && _paras[0] != -1) _isListTextEdit・デフォルトと違うリスト編集が必要 = true;
            CDatabaseFileReader_ReadByString・データベース読み込み機 _fileReader = null;

            if (_キャラタイプ == ECharaType・キャラの種類.c01_プレイヤーキャラ || _キャラタイプ == ECharaType・キャラの種類.c00_全キャラ)
            {
                if (p_playerReader == null)
                {
                    p_playerReader = getDataBaseReader・データベース読み込み機の取得(p_playerCharaDtabaseFileName);
                }
                _fileReader = p_playerReader;
                if (p_charaNameList_Player・プレイヤーキャラ名リスト == null)
                {
                    _isFirst初めてリストを作成 = true;
                    p_charaNameList_Player・プレイヤーキャラ名リスト = _fileReader.GetNameList・参照名リストを取得();
                }
                _charaNameList = p_charaNameList_Player・プレイヤーキャラ名リスト;
            }
            if (_キャラタイプ == ECharaType・キャラの種類.c02_ゲストキャラ || _キャラタイプ == ECharaType・キャラの種類.c00_全キャラ)
            {
                if (p_guestReader == null)
                {
                    p_guestReader = getDataBaseReader・データベース読み込み機の取得(p_gestCharaDatabaseFileName);
                }
                _fileReader = p_guestReader;
                if (p_charaNameList_Guest・ゲストキャラ名リスト == null)
                {
                    _isFirst初めてリストを作成 = true;
                    p_charaNameList_Guest・ゲストキャラ名リスト = _fileReader.GetNameList・参照名リストを取得();
                }
                _charaNameList = p_charaNameList_Guest・ゲストキャラ名リスト;
            }
            if (_キャラタイプ == ECharaType・キャラの種類.c03_亡霊キャラ || _キャラタイプ == ECharaType・キャラの種類.c00_全キャラ)
            {
                if (p_charaNameList_Bourei・亡霊キャラ名リスト == null)
                {
                    _isFirst初めてリストを作成 = true;
                    p_charaNameList_Bourei・亡霊キャラ名リスト = p_newSampleCharaNameList;
                }
                _charaNameList = p_charaNameList_Bourei・亡霊キャラ名リスト;
            }
            if (_キャラタイプ == ECharaType・キャラの種類.c00_全キャラ)
            {
                if (p_charaNameList_All・全種キャラ名のリスト == null)
                {
                    _isFirst初めてリストを作成 = true;
                    p_charaNameList_All・全種キャラ名のリスト = new List<string>();
                    p_charaNameList_All・全種キャラ名のリスト.AddRange(p_charaNameList_Bourei・亡霊キャラ名リスト);
                    p_charaNameList_All・全種キャラ名のリスト.AddRange(p_charaNameList_Player・プレイヤーキャラ名リスト);
                    p_charaNameList_All・全種キャラ名のリスト.AddRange(p_charaNameList_Guest・ゲストキャラ名リスト);
                }
                _charaNameList = p_charaNameList_All・全種キャラ名のリスト;
            }

            if (_isFirst初めてリストを作成 == true)
            {
                // 初めてリストを作成する場合は、ここでリストに編集を加える。
                if (_キャラタイプ != ECharaType・キャラの種類.c03_亡霊キャラ)
                {
                    CResourceData・資源データ _resource = null;
                    // ■基本パラの項目がないキャラの扱い（パラメータが0や記述がないキャラリストは消去）
                    for (int i = 0; i <= _charaNameList.Count - 1; i++)
                    {
                        // リストのキャラ名を一つずつ調べる
                        _resource = _fileReader.GetInfoLine・参照名に該当する一行の資源データを取得(_charaNameList[i]);
                        // データが見つかったら
                        if (_resource != null && _resource.p3to_option・列リスト != null)
                        {
                            // パラメータの記述がなかったら、削除
                            if (_resource.p3to_option・列リスト.Count < 5 || _resource.p3to_option・列リスト[p_iroParaSumIndex・基本色6総合値の列] == "0" || _resource.p3to_option・列リスト[p_iroParaSumIndex・基本色6総合値の列] == "")
                            {
                                _charaNameList.RemoveAt(i);
                                i--; // 削除した分だけずらす
                            }
                            
                        }
                    }
                }
            }
            if (_isListTextEdit・デフォルトと違うリスト編集が必要 == true)
            {
                //※リスト編集した時は_charaNameListと違うリストをコピーして編集しないと、もとのp_charaNameList_**が変更されてしまう
                List<string> _tempcharaNameList = MyTools.getCopyedList<string>(_charaNameList);

                // ●キャラ名リストに付加情報として、称号や、パラメータインデックスを追加する場合の編集。
                CResourceData・資源データ _resource = null;
                for (int i = 0; i <= _tempcharaNameList.Count - 1; i++)
                {

                    // リストのキャラ名を一つずつ調べる
                    _resource = _fileReader.GetInfoLine・参照名に該当する一行の資源データを取得(_charaNameList[i]);
                    // データが見つかったら
                    if (_resource != null && _resource.p3to_option・列リスト != null)
                    {
                        // 綺麗にそろわないがご勘弁を↓
                        _tempcharaNameList[i] = MyTools.getStringFormat(_charaNameList[i], 10, true);
                        _tempcharaNameList[i] += "："; // 選択肢チェックの時、必ず"："か":"がいる。getStringItemでとるから

                        // 称号を追加
                        if (_isShowEnblem・称号表示 == true)
                        {
                            string _syougo = MyTools.getListValue(_resource.p3to_option・列リスト, CCharaCreator・キャラ生成機.p_syogoIndex・称号の列);
                            _syougo = MyTools.getStringItem(_syougo, "、", 1); // [MEMO]今は称号は一つ目だけを表示している
                            _syougo = MyTools.getStringFormat("【" + _syougo + "】", 7, true);
                            _tempcharaNameList[i] += _syougo;
                        }

                        // パラメータインデックスを追加
                        string _paraString = "";
                        _tempcharaNameList[i] += "（";
                        _tempcharaNameList[i] += MyTools.getStringFormat(MyTools.getListValue(_resource.p3to_option・列リスト, _paras[0]), 5, false);
                        for (int j = 1; j <= _paras.Length - 1; j++)
                        {
                            {
                                _paraString = MyTools.getStringFormat(MyTools.getListValue(_resource.p3to_option・列リスト, _paras[j]), 5, false); ;
                            }
                            _tempcharaNameList[i] += "," + _paraString;
                        }
                        _tempcharaNameList[i] += "）";
                    }
                }
                // temp kara charaName he
                _charaNameList = _tempcharaNameList;
            }
            // 表示順番をランダムにする
            if (_isランダムソート == true)
            {
                MyTools.sortList(_charaNameList, MyTools.ESortType.ランダム);
            }
            return _charaNameList;
        }
        #endregion

        public static void savePlayerCharaData・キャラをプレイヤーデータベースに追加(CChara・キャラ _CChara・キャラ, string _顔画像ファイル名＿無い場合は空白でＯＫ)
        {
            int _資源のID = _CChara・キャラ.Var_Int(EVar.id);// 100001;
            string _newNameキャラの登録名 = _CChara・キャラ.Var(EVar.名前);
            List<string> _allCharaNameList = getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c00_全キャラ);
            CResourceData・資源データ _newData = new CResourceData・資源データ(_資源のID, _CChara・キャラ.Var(EVar.名前), _顔画像ファイル名＿無い場合は空白でＯＫ, _CChara・キャラ);
            if (p_playerReader == null)
            {
                p_playerReader = getDataBaseReader・データベース読み込み機の取得(p_playerCharaDtabaseFileName);
            }
            p_playerReader.saveData(p_playerCharaDtabaseFileName, _newData);
        }

        #region ■キャラを取得するメソッド：getChara***

        /// <summary>
        /// 引数のキャラ名を（データベースに登録されている）そのままのレベルで取得します．見つからなければnullを返します。
        /// </summary>
        /// <param name="_追加キャラ名"></param>
        /// <returns></returns>
        public static CChara・キャラ getChara・キャラを取得(string _追加キャラ名)
        {
            return getChara・キャラを取得(_追加キャラ名, -101);  // 元のLVにする
        }
        /// <summary>
        /// ■全てのgetChara***メソッドが共通して呼び出すメソッドです。
        /// 引数のキャラ名を指定LVで取得します．見つからなければnullを返します。
        /// 引数の_LVが-101の場合、データベースに記録されていた元のLV、あるいはLV1で取得します
        /// </summary>
        /// <param name="_追加キャラ名"></param>
        /// <returns></returns>
        public static CChara・キャラ getChara・キャラを取得(string _nameキャラ名, double _LV)
        {
            // データベースから情報を取得してnewでインスタンス化
            CChara・キャラ _c = null;
            CResourceData・資源データ _resourceData = null;

            if (p_guestReader == null)
            {
                p_guestReader = getDataBaseReader・データベース読み込み機の取得(p_gestCharaDatabaseFileName);
            }
            _resourceData = p_guestReader.GetInfoLine・参照名に該当する一行の資源データを取得(_nameキャラ名);
            if (_resourceData == null)
            {
                // ゲストでなかったら、プレイヤーキャラで
                if (p_playerReader == null)
                {
                    p_playerReader = getDataBaseReader・データベース読み込み機の取得(p_playerCharaDtabaseFileName);
                }
                _resourceData = p_playerReader.GetInfoLine・参照名に該当する一行の資源データを取得(_nameキャラ名);
            }
            if (_resourceData == null)
            {
                // それでもなかったら、亡霊キャラで
                if (p_charaNameList_Bourei・亡霊キャラ名リスト != null)
                {
                    if (p_charaNameList_Bourei・亡霊キャラ名リスト.Contains(_nameキャラ名) == true)
                    {
                        // 亡霊キャラはパラメータが無いので、ここで作っちゃう
                        _c = getNormalSampleChara・標準サンプルキャラ自動生成(true, _LV);
                    }
                }
            }
            if (_c == null && _resourceData == null)
            {
                _c = null;
                Program・実行ファイル管理者.printlnLog(ELogType.l5_エラーダイアログ表示, "該当するキャラ名のデータが見つかりません。キャラ名："+_nameキャラ名);
            }
            else if (_resourceData != null)
            {
                _c = _resourceData.getCharaData・キャラデータに変換して取得();
            }


            if(_c != null)
            {
                if (_LV == -101)
                {
                    _LV = _c.Para(EPara.LV); // 元のLVにする
                }
                else if (_LV < 0)
                {
                    _LV = 1; // 引数の値が不正だったら、LV1
                }

                // LVUP処理
                _c.setPara(EPara.LV, _LV);
                LVUP・レベル設定(_c, Math.Max(_LV - 1, 0));

                // [ブレークポイント]キャラのパラが代入されているかを確認
                Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, MyTools.getListValues_ToCSVLine("■" + _c.name名前() + "の色パラ: ", _c.Paras・パラメータ一括処理().getIro18色パラメータ(), true));
                double _test = _c.Para(EPara.a1_ちから);
            }
            return _c;
        }

        /// <summary>
        /// 引数のキャラ名を指定した総合パラメータ値の指定倍数範囲（0.5-1.2など）のLVにしたキャラを取得します．
        /// </summary>
        /// <param name="_キャラ名"></param>
        /// <param name="_総合パラメータ_基本6色パラメータ"></param>
        /// <param name="_最低倍数"></param>
        /// <param name="_最高倍数"></param>
        /// <returns></returns>
        public static CChara・キャラ getChara・パラ総合値がほどほどに似たキャラを取得(ECharaType・キャラの種類 _キャラタイプ, double _基本力6パラメータ総合値, double _最低倍数, double _最高倍数)
        {
            double _比較パラ総合値 = _基本力6パラメータ総合値;

            string _ゲストキャラ名 = "";
            CChara・キャラ _キャラ = null;
            double _パラ総合値 = 0.0;
            List<string> _ゲストキャラ名リスト = getCharaNameList・キャラ名リストを取得(_キャラタイプ);
            if (_ゲストキャラ名リスト.Count == 0)
            {
                Program・実行ファイル管理者.printlnLog(ELogType.l5_エラーダイアログ表示, "キャラクタデータベースが見つかりません．\nしょうがないないので，LVランダムな亡霊キャラで我慢してください．");
                int _randomLVUPNum = MyTools.getRandomNum(1, 99);
                return getNormalSampleChara・標準サンプルキャラ自動生成(true, _randomLVUPNum);
            }

            // キャラのパラメータ総和が，与えられたパラメータ総和の指定倍数範囲になるキャラを生成
            int _doNum = 1; // [WARNING][MEMO]これ以上やってできなければ，変な結果が出る
            while (_doNum <= 100)
            {
                _ゲストキャラ名 = getGestCharaName・パラ総合値が0じゃないゲストキャラをランダム取得();
                if (p_guestReader == null)
                {
                    p_guestReader = getDataBaseReader・データベース読み込み機の取得(p_gestCharaDatabaseFileName);
                }
                _パラ総合値 = MyTools.parseDouble(p_guestReader.GetInfoWord・参照名に該当する一語のデータを取得(_ゲストキャラ名, p_iroParaSumIndex・基本色6総合値の列));

                // (1)最低～最高のうまいバランスで見つかったら，LV調整せず決定
                if (_パラ総合値 <= _最高倍数 * _比較パラ総合値 && _パラ総合値 >= _最低倍数 * _比較パラ総合値)
                {
                    _キャラ = getChara・キャラを取得(_ゲストキャラ名, 1);
                    break;
                }
                // (2)LV1で最高を超えていたら，キャラ選択からやり直し
                else if (_パラ総合値 > _最高倍数 * _比較パラ総合値)
                {
                    _ゲストキャラ名 = "";
                    _doNum++;
                    continue;
                }
                // (3)LV1で最低より下なら，LVを調整
                else if (_パラ総合値 < _最低倍数 * _比較パラ総合値)
                {
                    _キャラ = getCharaFromCharaName_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変えて取得(_ゲストキャラ名, _比較パラ総合値, _最低倍数, _最高倍数);
                    break;
                }
            }
            if (_doNum > 100 && _ゲストキャラ名 == "")
            {
                Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "※100以上キャラを探しても，適切なキャラが見つからなかったのでランダムにします．");
                int _randomLVUPNum = MyTools.getRandomNum(1, 99);
                _ゲストキャラ名 = getGestCharaName・パラ総合値が0じゃないゲストキャラをランダム取得();
                _キャラ = getChara・キャラを取得(_ゲストキャラ名, _randomLVUPNum);
            }
            return _キャラ;
        }
        /// <summary>
        /// ■引数のキャラ名を指定した総合パラメータ値の指定倍数範囲（0.5-1.2など）のLVにしたキャラを取得します．
        /// </summary>
        public static CChara・キャラ getCharaFromCharaName_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変えて取得(string _キャラ名, double _基本力6パラメータ総合値, double _最低倍数, double _最高倍数)
        {
            CChara・キャラ _キャラ = getChara・キャラを取得(_キャラ名, 1);

            _キャラ = getChara_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変更して取得(_キャラ, _基本力6パラメータ総合値, _最低倍数, _最高倍数);
            return _キャラ;
        }

        public static CChara・キャラ getChara_LVUP_UntilSimilarPara・パラがほどほどに似たキャラにレベルを変更して取得(CChara・キャラ _キャラ, double _基本力6パラメータ総合値, double _最低倍数, double _最高倍数)
        {
            double _パラ総合値 = 0.0;

            // キャラの総合値が，比較パラと同等以上になるまでレベル調整
            int _loopMaxNum = 100;
            // 変な値の処理
            _基本力6パラメータ総合値 = MyTools.adjustValue_From_Min_To_Max(_基本力6パラメータ総合値, 0, 100000000);
            _最低倍数 = MyTools.adjustValue_From_Min_To_Max(_最低倍数, 0, 1000.0);
            _最高倍数 = MyTools.adjustValue_From_Min_To_Max(_最高倍数, 0, 1000.0);

            // ランダムで，レベルアップしていくか，レベルダウンしていくかを決める
            int _random = MyTools.getRandomNum(1, 4);
            if (_random <= 3)
            {
                // (3.a)LVを1～上げていく
                int _キャラLVUP毎の上昇値 = (int)(_キャラ.Para(EPara.LV1c_基本6色総合値) * s_LV1UP毎の能力上昇率);
                if (_キャラLVUP毎の上昇値 <= 0) _キャラLVUP毎の上昇値 = 1; // 0割りやマイナス割りを防ぐ処理
                int _randomLVUPNum = Math.Max(0, ((int)_基本力6パラメータ総合値 / _キャラLVUP毎の上昇値) - 10);
                LVUP・レベル設定(_キャラ, _randomLVUPNum);
                _パラ総合値 = _キャラ.Para(EPara._基本6色総合値);
                int i = 0;
                while (i<_loopMaxNum && _パラ総合値 < _最低倍数 * _基本力6パラメータ総合値)
                {
                    _randomLVUPNum = MyTools.getRandomNum(1, 3);
                    LVUP・レベル設定(_キャラ, _randomLVUPNum);
                    _パラ総合値 = _キャラ.Para(EPara._基本6色総合値);
                    i++;
                }
            }
            else
            {
                // (3.b)LVを99～下げていく，ただしLVマイナスは威厳がなくなるので，LV3まで
                _パラ総合値 = _キャラ.Para(EPara._基本6色総合値);
                int i = 0;
                while (i<_loopMaxNum && _パラ総合値 < _基本力6パラメータ総合値)
                {
                    LVUP・レベル設定(_キャラ, 30); // 99以上もあり
                    _パラ総合値 = _キャラ.Para(EPara._基本6色総合値);
                    i++;
                }
                while (i < _loopMaxNum && _パラ総合値 > _最高倍数 * _基本力6パラメータ総合値 && _キャラ.Para(EPara.LV) >= 3)
                {
                    int _randomLVUPNum = MyTools.getRandomNum(1, 3);
                    LVUP・レベル設定(_キャラ, -1 * _randomLVUPNum);
                    _パラ総合値 = _キャラ.Para(EPara._基本6色総合値);
                    i++;
                }
            }
            return _キャラ;
        }
        #endregion


        public static string getGestCharaName・パラ総合値が0じゃないゲストキャラをランダム取得()
        {
            string _ゲストキャラ名 = "";
            double _パラ総合値 = 0.0;
            while (_ゲストキャラ名 == "" || _パラ総合値 <= 0)
            {
                _ゲストキャラ名 = MyTools.getRandomString(getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c02_ゲストキャラ));
                if (p_guestReader == null)
                {
                    p_guestReader = getDataBaseReader・データベース読み込み機の取得(p_gestCharaDatabaseFileName);
                }
                _パラ総合値 = MyTools.parseDouble(p_guestReader.GetInfoWord・参照名に該当する一語のデータを取得(_ゲストキャラ名, p_iroParaSumIndex・基本色6総合値の列));
            }
            return _ゲストキャラ名;
        }

        #region 新しく個性あるパラメータを持った標準サンプルキャラ作成
        /// <summary>
        /// ■新しく個性あるパラメータを持ったサンプルキャラを作成します．
        /// </summary>
        /// <returns></returns>
        public static CChara・キャラ getNormalSampleChara・標準サンプルキャラ自動生成(bool _TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, double _LV_Average, bool _isLVRandom, double _6パラ総合値, double _strongRate・強さ率＿基本は１で標準＿２倍だと２倍の強さ)
        {
            // CChara・キャラ _newChara = new CChara・キャラ(); //
            // テスト用
            // キャラのレベルを決定(LVも変動)
            double _randomLVAdd = 0.0;
            if (_isLVRandom == true)
            {
                _randomLVAdd = MyTools.getRandomNum(0, 4) - 2; // 5通り，LV変動値: -2～+2
            }
            double _LV = Math.Max(_LV_Average + _randomLVAdd, 1);

            CChara・キャラ _newChara = getNormalSampleChara・標準サンプルキャラ自動生成(_TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, _LV, _6パラ総合値, _strongRate・強さ率＿基本は１で標準＿２倍だと２倍の強さ, false);
            return _newChara;
        }
        /// <summary>
        /// 指定したLVのパラメータを自動的に振り分けたキャラを作ります．
        /// </summary>
        /// <param name="_LV_Average"></param>
        /// <returns></returns>
        public static CChara・キャラ getNormalSampleChara・標準サンプルキャラ自動生成(bool _TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, double _LV)
        {
            return getNormalSampleChara・標準サンプルキャラ自動生成(_TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, _LV, s_LV1Para_Iro6Sum・キャラＬＶ１標準の基本６色パラ総合値, 1.0, false);
        }
        /// <summary>
        /// 指定したLVのパラメータを自動的に振り分けたキャラを作ります．
        /// </summary>
        /// <param name="_LV_Average"></param>
        /// <returns></returns>
        public static CChara・キャラ getNormalSampleChara・標準サンプルキャラ自動生成(bool _TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, double _LV, bool _isAllAveragePara・全てのパラメータを均等に振り分けた超標準キャラを作るか)
        {
            return getNormalSampleChara・標準サンプルキャラ自動生成(_TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, _LV, s_LV1Para_Iro6Sum・キャラＬＶ１標準の基本６色パラ総合値, 1.0, _isAllAveragePara・全てのパラメータを均等に振り分けた超標準キャラを作るか);
        }
        /// <summary>
        /// 指定したLVの、パラメータを自動的に振り分けたキャラを作ります．
        /// </summary>
        /// <param name="_LV_Average"></param>
        /// <returns></returns>
        public static CChara・キャラ getNormalSampleChara・標準サンプルキャラ自動生成(bool _TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, double _LV, double _strongRate・強さ率＿基本は１で標準＿２倍だと２倍の強さ)
        {
            return getNormalSampleChara・標準サンプルキャラ自動生成(_TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, _LV, s_LV1Para_Iro6Sum・キャラＬＶ１標準の基本６色パラ総合値, _strongRate・強さ率＿基本は１で標準＿２倍だと２倍の強さ, false);
        }
        /// <summary>
        /// ■指定したLVの、パラメータを自動的に振り分けたキャラを作ります．
        /// </summary>
        /// <param name="_LV_Average"></param>
        /// <returns></returns>
        public static CChara・キャラ getNormalSampleChara・標準サンプルキャラ自動生成(bool _TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか, 
            double _LV, double _6パラ総合値, double _strongRate・強さ率＿基本は１で標準＿２倍だと２倍の強さ, bool _isAllAveragePara・全てのパラメータを均等に振り分けた超標準キャラを作るか)
        {

            CChara・キャラ _c = new CChara・キャラ();
            string _sampleCharaName = "";
            int _playerCharaNum = getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c01_プレイヤーキャラ).Count;
            int _sampleCharaID = 10000 + _playerCharaNum + 1; // デフォルト

            if (_TrueIsCreateNew_FalseIsUsePast・亡霊キャラを新規作成するか == true)
            {
                // サンプルの名前からどれかを選択
                _sampleCharaName = MyTools.getRandomString(p_newSampleCharaNameList);
                _sampleCharaID = 90000 + p_pastSampleCharaNameList.Count + 1;
                // サンプルキャラ（亡霊キャラ）生成数をプラス
                p_pastSampleCharaNameList.Add(_sampleCharaName);
                // 選択して削除，同じ名前のキャラは作らない
                p_newSampleCharaNameList.Remove(_sampleCharaName);
            }
            else
            {
                // 過去に作成したサンプルキャラ（亡霊キャラ）から名前を取得
                _sampleCharaName = MyTools.getRandomString(p_pastSampleCharaNameList);
                _sampleCharaID = 90000 + p_pastSampleCharaNameList.IndexOf(_sampleCharaName);
            }
            _c.setVar・変数を変更(EVar.名前, _sampleCharaName);
            _c.setName・名前を一時的に変更(_c.Var(EVar.名前));
            _c.setId・IDを変更(_sampleCharaID);

            // ●●●色パラを作成！（※現段階では、基本色，中間色でそれぞれ，250+LV*25を5ずつ振り分け）
            // 基本６色パラを初期化（まだすべて0のはず）
            List<double> _iro6・基本色６パラ = _c.Paras・パラメータ一括処理().getIro6基本６色パラメータ();
            // 中間６色パラを初期化（まだすべて0のはず）
            List<double> _iroMID6・中間色６パラ = _c.Paras・パラメータ一括処理().getIroMID6中間６色パラメータ();
            // 装飾６色パラは今は全部1
            List<double> _iroEXT6・装飾色６パラ = new List<double>(new double[] { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0 }); //_chara.Paras・パラメータ一括処理().getIroEXT6装飾６色パラメータ();

            #region 色パラをランダムで作成
            // ●基本６色パラ
            // サンプルパラメータ振り分けポイントの計算
            double _sampleParaPointTotal = _6パラ総合値 * _strongRate・強さ率＿基本は１で標準＿２倍だと２倍の強さ;
            double _ADDNUM_PER = 5.0;
            int _addIndex = 0;
            // 各パラメータに振り分ける
            if (_isAllAveragePara・全てのパラメータを均等に振り分けた超標準キャラを作るか == false)
            {
                // ランダムで振り分ける
                while (_sampleParaPointTotal > 0)
                {
                    _addIndex = MyTools.getRandomNum(0, _iro6・基本色６パラ.Count - 1);
                    _iro6・基本色６パラ[_addIndex] += _ADDNUM_PER;
                    _sampleParaPointTotal -= _ADDNUM_PER;
                }
            }
            else
            {
                // 全てのパラメータを均等に振り分ける
                for (int i = 0; i < _iro6・基本色６パラ.Count; i++)
                {
                    _iro6・基本色６パラ[_addIndex] += _ADDNUM_PER / _iro6・基本色６パラ.Count;
                }
            }

            // ●中間６色パラ
            // サンプルパラメータ振り分けポイントの計算
            _sampleParaPointTotal = _6パラ総合値 * _strongRate・強さ率＿基本は１で標準＿２倍だと２倍の強さ;
            // 各パラメータに振り分ける
            if (_isAllAveragePara・全てのパラメータを均等に振り分けた超標準キャラを作るか == false)
            {
                // ランダムで振り分ける
                while (_sampleParaPointTotal > 0)
                {
                    _addIndex = MyTools.getRandomNum(0, _iroMID6・中間色６パラ.Count - 1);
                    _iroMID6・中間色６パラ[_addIndex] += _ADDNUM_PER;
                    _sampleParaPointTotal -= _ADDNUM_PER;
                }
            }
            else
            {
                // 全てのパラメータを均等に振り分ける
                for (int i = 0; i < _iroMID6・中間色６パラ.Count; i++)
                {
                    _iroMID6・中間色６パラ[_addIndex] += _ADDNUM_PER / _iroMID6・中間色６パラ.Count;
                }
            }


            // [TODO]戦闘バランス調整がしやすいように，身体パラの「ちからの＊＊＊」というキャラ名＊＊＊の強いパラメータがわかるように名前を加える？
            // [テスト]キャラの名前は，サンプルとして，一番大きいパラメータの名前
            // int _biggestParaIndex = MyTools.getIndex_Biggest(_chara.Paras・パラメータ一括処理().getShintaiParas・身体パラメータ());
            // string _biggestParaName = MyTools.getEnumKeyName_InDetail(EPara.a1_ちから);
            // ※[テスト]身体パラメータ総合値を名前に追加
            //int _shintaiParaSum = (int)(MyTools.getSum(_chara.Paras・パラメータ一括処理().getShintaiParas・身体パラメータ()));
            //_chara.setName・名前を一時的に変更(_chara.name名前() + "" + _shintaiParaSum);
            #endregion

            // ●●●色パラメータを代入
            _c.Paras・パラメータ一括処理().setIroParas基本色6パラメータを代入(_iro6・基本色６パラ);
            _c.Paras・パラメータ一括処理().setIroParas中間色6パラメータを代入(_iroMID6・中間色６パラ);
            _c.Paras・パラメータ一括処理().setIroParas装飾色6パラメータを代入(_iroEXT6・装飾色６パラ);
            // ●●●身体パラを基本色パラ，精神パラを中間色パラとして代入
            _c.Paras・パラメータ一括処理().setShintaiParas・身体パラメータをセット(_iro6・基本色６パラ);
            _c.Paras・パラメータ一括処理().setSeishinParas・精神パラメータをセット(_iroMID6・中間色６パラ);

            // LV処理
            _c.setPara(EPara.LV, ESet._default・代入値, 1);
            LVUP・レベル設定(_c, _LV - 1);

            // [ブレークポイント]キャラのパラが代入されているかを確認
            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, MyTools.getListValues_ToCSVLine("■" + _c.name名前() + "の色パラ: ", _c.Paras・パラメータ一括処理().getIro18色パラメータ(), true));
            double _test = _c.Para(EPara.a1_ちから);

            return _c;
        }
        #endregion

        #region ダイスコマンドの自動生成: createDiceCommand***
        /// <summary>
        /// ●キャラの色パラメータから，ダイスコマンドを自動生成します．
        /// </summary>
        /// <param name="_chara"></param>
        public static void createDiceCommand_FromParas・ダイスコマンドを自動生成(CChara・キャラ _c)
        {
            // 生成時間を計算1
            long _time1 = MyTime.getNowTimeAndMSec_NumberOnly();

            // このキャラのダイスコマンドを最初から作る（初めてダイスを生成）のか、それとも値を変更するのかを判別。
            bool _isFirst初めてダイスを生成 = true;
            int _コマンドID = 0; // このキャラのコマンドＩＤ（今まで何個のコマンドを作ったか）。実際の値は1以上
            List<int> _キャラダイス順のコマンドID = new List<int>();
            if (_c.getP_dice・所有ダイス().Count != 0)
            {
                _isFirst初めてダイスを生成 = false;
                // ■２回目以降はダイスをランダムソートせず、（ＬＶが上がっているため）値のみを上書きするようにする。
                // そのために、キャラダイス順のコマンドIDをロードして覚えておく。
                int _キャラダイス固有のコマンドID = 0;
                int _コマンドIDの最小値 = 0; // 最小値を格納しておいて、それ新しく作成するダイスコマンドの開始コマンドIDにする。
                CDiceCommand・ダイスコマンド _dice;
                for (int i = 0; i < s0_Masu最大マス数; i++ )
                {
                    _dice = MyTools.getListValue(_c.getP_dice・所有ダイス(), i);
                    if (_dice != null)
                    {
                        _キャラダイス固有のコマンドID = _dice.getp_id();
                        _キャラダイス順のコマンドID.Add(_キャラダイス固有のコマンドID);
                        // 最小値を格納
                        if (_キャラダイス固有のコマンドID < _コマンドIDの最小値)
                        {
                            _コマンドIDの最小値 = _キャラダイス固有のコマンドID;
                        }
                    }
                    else
                    {
                        _キャラダイス順のコマンドID.Add(0); // コマンドID=0はコマンド無しということ（エラー対策用）。
                    }
                }
                // コマンドIDの開始位置を、ロードしたコマンドIDの最小値に設定
                _コマンドID = _コマンドIDの最小値;
                // 値を更新するため、所有ダイスはクリアしておく
                //セットする時にちゃんとClearするから、ここでやらなくていい。_chara.getP_dice・所有ダイス().Clear();
            }

            // 現在は，キャラの基本6色パラメータから，ダイスを作成

            // [Memo]初期ダイスコマンドの基本的なルール
            // ・コマンド6個（内，4個が攻撃（クリティカル付き），1個が防御，1個がミス（回避））

            double _c01赤 = _c.Para(EPara.c01_赤);
            double _c03橙 = _c.Para(EPara.c02_橙);
            double _c05黄 = _c.Para(EPara.c03_黄);
            double _c07緑 = _c.Para(EPara.c04_緑);
            double _c09青 = _c.Para(EPara.c05_青);
            double _c11紫 = _c.Para(EPara.c06_紫);
            double _c02赤橙 = _c.Para(EPara.c07_赤橙);
            double _c04黄橙 = _c.Para(EPara.c08_黄橙);
            double _c06黄緑 = _c.Para(EPara.c09_黄緑);
            double _c08青緑 = _c.Para(EPara.c10_青緑);
            double _c10青紫 = _c.Para(EPara.c11_青紫);
            double _c12赤紫 = _c.Para(EPara.c12_赤紫);

            // 以下，ダイスパラ専用の内部パラメータ（一応，戦闘パラメータに代入される）
            // ■■■戦闘パラメータを決定
            double _01攻撃力 = _c01赤 * s_kougekirちからが攻撃力に比例する定数;
            double _02HP = _c03橙 * s_hp持久力が最大HPに比例する定数;
            double _03守備力 = _c05黄 * s_bougyo行動力が守備力に比例する定数;

            double _04速度 = _c07緑 * s_sokudo素早さが速度に比例する定数;
            double _05回避率 = _c09青 * s_kaihi精神力が回避率に比例0_020 + s_kai0回避率初期値;
            if (_05回避率 > s_kai回避率MAX90) { _05回避率 = s_kai回避率MAX90; }
            // double _05回避率 = Math.Pow(s_kaihi精神力が回避率に比例0_010, _c09青) + s_kai0回避率初期値;
            // if (_c09青 == 0.0) { _05回避率 = s_kai回避率初期値; }
            // if (_05回避率 > s_kai回避率MAX90) { _05回避率 = s_kai0回避率MAX90; }
            
            double _06クリティカル率 = _c11紫 * s_kuri賢さがクリティカル率に比例0_015 + s_kur0クリティカル率初期値;
            if (_06クリティカル率 > s_kuriクリティカル率MAX70) { _06クリティカル率 = s_kuriクリティカル率MAX70; }
            double _06b魔法力 = _c11紫 * s_mahou賢さが魔法力に比例;
            double _07命中率 = _c02赤橙 * s_mei器用さが命中率に比例0_010 + s_mei0命中率初期値;
            if (_07命中率 > s_mei命中率MAX90) { _07命中率 = s_mei命中率MAX90; }
            
            double _07bガード率 = _c05黄 * s_gurd行動力がガード率に比例0_005 + 
                _c02赤橙 * s_gurd器用さがガード率に比例0_001 + _c04黄橙 * s_gurd忍耐力がガード率に比例0_003;
            if(_07bガード率 > s_gurdガード率MAX50) { _07bガード率 = s_gurdガード率MAX50; }
            
            double _08根性発動率 = _c04黄橙 * s_konzyo忍耐力が根性に比例0_099;
            if (_08根性発動率 > s_konzyo根性発動率MAX99) { _08根性発動率 = s_konzyo根性発動率MAX99; }
            double _08b根性発動最大オーバーダメージ = _c04黄橙 * s_over忍耐力が根性オーバーダメージに比例;
            
            double _09SP = _c09青 * s_sp精神力がSPに比例 + _c06黄緑 * s_sp健康力がSPに比例 + _c10青紫 * s_sp適応力がSPに比例 + s_sp集中力がSPに比例;
            double _09自然回復発動率 = _c06黄緑 * s_auto健康力が自然回復発動率に比例0_030;
            if (_09自然回復発動率 > s_auto自然回復発動率MAX90) { _09自然回復発動率 = s_auto自然回復発動率MAX90; }
            double _09b自然回復量 = _c06黄緑 * s_auto健康力が自然回復量に比例;
            
            double _10対応率 = _c08青緑 * s_taiou適応力が対応率に比例0_010;
            if (_10対応率 > s_taiou対応率MAX70) { _10対応率 = s_taiou対応率MAX70; }
            
            double _11集中率 = _c10青紫 * s_syutyu集中力が集中率に比例0_020;
            if (_11集中率 > s_syutyu集中力MAX90) { _11集中率 = s_syutyu集中力MAX90; }
            
            double _12戦術 = _c12赤紫 * s_senzyu思考力が戦術に比例0_10;
            // double _運 = 相手とのLV差(); // 全ての確率を有利にする？

            // ■■■マス数の決定
            double _masu = 0.0;
            //double _a = s_bougyoNum素早さが防御マス数に二次比例するXの2乗係数;
            //double _R = s_bougyoNum素早さが防御マス数に二次比例するXの1乗係数;
            //double _masu = Math.Pow(_04速度, 2) * _a + _04速度 * _R + s_bougyoNum0防御マス初期値;
            _masu = MyTools.Log(s_bougyoNum行動力が防御マス数に対数比例するlog基底数, _03守備力)
                + s_bougyoNum0防御マス初期値;
            double _09防御マス数 = MyTools.adjustValue_From_Min_To_Max(_masu, 0.0, s0_Masu最大マス数);
            _masu = MyTools.Log(s_kaihiNum素早さが回避マス数に対数比例するlog基底数, _04速度)
                + s_kaihi0回避マス初期値;
            double _10回避マス数 = MyTools.adjustValue_From_Min_To_Max(_masu, 0.0, s0_Masu最大マス数);
            double _07攻撃マス数 = 4; //s0_Masu最大マス数 - (int)_09防御マス数 - (int)_10回避マス数;
            // 攻撃マス数は必ず1つ以上
            _07攻撃マス数 = MyTools.adjustValue_From_Min_To_Max(_07攻撃マス数, 1.0, s0_Masu最大マス数);


            // [TEST]Math.Logのとき，0のときもちゃんとNaNにならず計算できるか？　→　できないからMyTools.logを使う！
            #region テスト
            //double テスト紫 = 0;
            //_06クリティカル率 = Math.Log(s_kuri賢さがクリティカル率に比例0_007, テスト紫) + s_kur0クリティカル率初期値;
            // [TEST]テスト終わり
            #endregion

            // ○○○キャラの戦闘パラメータを代入！
            _c.setPara(EPara.s07_攻撃力, ESet._default・代入値, _01攻撃力);
            _c.setPara(EPara.s03_HP, ESet._default・代入値, _02HP);
            _c.setPara(EPara.s03b_最大HP, ESet._default・代入値, _02HP);
            _c.setPara(EPara.s09_守備力, ESet._default・代入値, _03守備力);
            _c.setPara(EPara.s10_速度, ESet._default・代入値, _04速度);
            _c.setPara(EPara.s11_回避率, ESet._default・代入値, _05回避率);
            _c.setPara(EPara.s12_クリティカル率, ESet._default・代入値, _06クリティカル率);
            _c.setPara(EPara.s08_魔法力, ESet._default・代入値, _06b魔法力);
            _c.setPara(EPara.s13_命中率, ESet._default・代入値, _07命中率);
            _c.setPara(EPara.s13b_ガード率, ESet._default・代入値, _07bガード率);
            _c.setPara(EPara.s14_根性発動率, ESet._default・代入値, _08根性発動率);
            _c.setPara(EPara.s14b_根性発動最大オーバーダメージ, ESet._default・代入値, _08b根性発動最大オーバーダメージ);
            _c.setPara(EPara.s04_SP, ESet._default・代入値, _09SP);
            _c.setPara(EPara.s04b_最大SP, ESet._default・代入値, _09SP);
            _c.setPara(EPara.s15_自然回復発動率, ESet._default・代入値, _09自然回復発動率);
            _c.setPara(EPara.s15b_自然回復量, ESet._default・代入値, _09b自然回復量);
            _c.setPara(EPara.s16_対応率, ESet._default・代入値, _10対応率);
            _c.setPara(EPara.s17_集中率, ESet._default・代入値, _11集中率);
            _c.setPara(EPara.s18_戦術, ESet._default・代入値, _12戦術);

            _c.setVar・変数を変更("攻撃マス", _07攻撃マス数.ToString());
            _c.setVar・変数を変更("防御マス", _09防御マス数.ToString());
            _c.setVar・変数を変更("回避マス", _10回避マス数.ToString());

            // テスト
            string _charaSentouParaInfo = "●" + _c.name名前() + "の能力 : " +
               "攻撃力" + MyTools.getStringNumber(_01攻撃力, true, false, 6, 0, 6) + " " +
               "HP" + MyTools.getStringNumber(_02HP, true, false, 6, 0, 6) + " " +
               "守備力" + MyTools.getStringNumber(_03守備力, true, false, 6, 0, 6) + " " +
               "素早さ" + MyTools.getStringNumber(_04速度, true, false, 6, 0, 6) + " " +
               "回避率" + MyTools.getStringNumber(_05回避率, true, false, 3, 0, 3) + "％" + " " +
               "クリ率" + MyTools.getStringNumber(_06クリティカル率, true, false, 3, 0, 3) + "％" + " " +
               "魔法力" + MyTools.getStringNumber(_06b魔法力, true, false, 6, 0, 6) + " " + "\n" +

               "命中率" + MyTools.getStringNumber(_07命中率, true, false, 3, 0, 3) + "％" + " " +
               "gurd率" + MyTools.getStringNumber(_07bガード率, true, false, 3, 0, 3) + "％" + " " +
               "根性率" + MyTools.getStringNumber(_08根性発動率, true, false, 3, 0, 3) + "％" + " " +
               "Overダ" + MyTools.getStringNumber(_08b根性発動最大オーバーダメージ, true, false, 6, 0, 6) + "％" + " " + "\n" + 

               "SP" + MyTools.getStringNumber(_09SP, true, false, 6, 0, 6) + "" + " " +
               "回復率" + MyTools.getStringNumber(_09自然回復発動率, true, false, 3, 0, 3) + "％" + " " +
               "回復量" + MyTools.getStringNumber(_09b自然回復量, true, false, 3, 0, 3) + "％" + " " +
               "対応率" + MyTools.getStringNumber(_10対応率, true, false, 3, 0, 3) + "％" + " " +
               "集中率" + MyTools.getStringNumber(_11集中率, true, false, 3, 0, 3) + "％" + " " +
               "戦術" + MyTools.getStringNumber(_12戦術, true, false, 6, 0, 6) + " " + "\n" + 

               "攻撃マス" + MyTools.getStringNumber(_07攻撃マス数, true, false, 2, 0, 2) + "個 " +
               "防御マス" + MyTools.getStringNumber(_09防御マス数, true, false, 2, 0, 2) + "個 " +
               "回避マス" + MyTools.getStringNumber(_10回避マス数, true, false, 2, 0, 2) + "個 ";
            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, _charaSentouParaInfo);
            int a = 1;


            // ●●●全てのマスを生成
            List<CDiceCommand・ダイスコマンド> _attackCommand = new List<CDiceCommand・ダイスコマンド>();
            List<CDiceCommand・ダイスコマンド> _diffenceCommand = new List<CDiceCommand・ダイスコマンド>();
            List<CDiceCommand・ダイスコマンド> _avoidCommand = new List<CDiceCommand・ダイスコマンド>();
            List<int> _cAD攻撃ダメージ = new List<int>(s0_Masu最大マス数);
            List<int> _cCPクリ率 = new List<int>(s0_Masu最大マス数);
            List<int> _cAP回避率 = new List<int>(s0_Masu最大マス数);
            List<int> _cDD防御ダメージ = new List<int>(s0_Masu最大マス数);
            int _biggestIndex;

            // （）防御軽減ダメージ数を作成
            int _diffenceNum = (int)_09防御マス数;
            if (_diffenceNum > 0)
            {
                _cDD防御ダメージ.AddRange(getDividePara・パラメータ合計値をN個に分割(_03守備力, 
                    s_DiceNumAddBonusダイスマス分割数増加による合計値ボーナス率, _diffenceNum, 2.0, 1.0, 0.9, 1.1, 0.8, 1.2));
                _biggestIndex = MyTools.getIndex_Biggest(_cDD防御ダメージ);
                // 防御Ｅ～Ｓ、などの名前を取得
                string _rank = "" + getParaRank・パラランクＥ＿Ｓ(_cDD防御ダメージ[_biggestIndex], 100.0); // ■ランクＥ基準値 
                // ●一番大きい防御を防御マス（防御専用マス）として生成
                _コマンドID++;
                List<string> _gurdRandomMessages = new List<string>(); // ここでこう書いてもいい。new List<string>() {"[キャラ名]は防御している。" , "[キャラ名]は身を守っている…", "[キャラ名]は"};
                _diffenceCommand.Add(new CDiceCommand・ダイスコマンド(_コマンドID, "防御"+_rank,
                    new CBattleAction・戦闘行動(_gurdRandomMessages, EBattleActionType・行動タイプ.t04_防御, EBattleActionObject・攻撃対象.t03_自分, 
                    0.0, 0.0, 0.0, _cDD防御ダメージ[_biggestIndex])));
                // 一番大きい防御をリストから削除
                _cDD防御ダメージ.RemoveAt(_biggestIndex);
            }

            // （）回避率を作成
            int _avoidNum = (int)_10回避マス数;
            if (_avoidNum > 0)
            {
                _cAP回避率.AddRange(getDividePara・パラメータ合計値をN個に分割(_05回避率, 
                    s_DiceNumAddBonusダイスマス分割数増加による合計値ボーナス率, _avoidNum, 1.0, 0.85, 0.8, 0.9, 0.75, 1.1));
                _biggestIndex = MyTools.getIndex_Biggest(_cAP回避率);
                string _rank = "" + getParaRank・パラランクＥ＿Ｓ(_cAP回避率[_biggestIndex], 0.5); // ■ランクＥ基準値
                // ●一番大きい回避をミスマス（回避専用マス）として生成
                _コマンドID++;
                List<string> _avoidRandomMessages = new List<string>(); // 無しでもデフォルト文字が出る。が、ここでこう書いてもいい。new List<string>() {"[キャラ名]は防御している。" , "[キャラ名]は身を守っている…", "[キャラ名]は"};
                _avoidCommand.Add(new CDiceCommand・ダイスコマンド(_コマンドID, "回避"+_rank, 
                    new CBattleAction・戦闘行動(_avoidRandomMessages, EBattleActionType・行動タイプ.t05_回避, EBattleActionObject・攻撃対象.t03_自分, 
                    0.0, 0.0, _cAP回避率[_biggestIndex], 0.0)));
                // 一番大きい回避をリストから削除
                _cAP回避率.RemoveAt(_biggestIndex);
            }

            // （）攻撃ダメージ数を作成
            // ■全キャラ統一で攻撃マス（回復マス含む）は4マスに固定し，残り2マスは「ミス（回避）」と「防御」？
            // 例：　赤=100のとき，攻撃力=400とし，ダメージは120,110,100,70の4つとする
            int _attackDiceNum = (int)_07攻撃マス数;
            int _attack = (int)(_01攻撃力 * s1_attaDevN_1マスの基本攻撃力倍率);
            double _ASum攻撃力合計値 = _attack * _attackDiceNum;
            double[] _攻撃力分割レート = new double[] { 1.2, 1.1, 1.0, 0.7, 0.5, 1.5 }; //旧ダイスバトル{1.0, 1.2, 1.1, 0.7, 0.6, 1.4}
            if (s_ダイスバトル_攻撃マスの攻撃力計算時にランダム要素を入れるか == true)
            {
                // ５％間隔で１０回、ランダムに値を変更する。（運要素で合計値が変わることあり）
                int _randomChangeNum = MyTools.getRandomNum(1, 10);
                int _randomChangeIndex;
                double _changeValue = 0.05;
                for (int i = 0; i <= _randomChangeNum; i++)
                {
                    _randomChangeIndex = MyTools.getRandomNum(0, _attackDiceNum - 1);
                    if (_攻撃力分割レート[_randomChangeIndex] > _changeValue)
                    {
                        _攻撃力分割レート[_randomChangeIndex] -= _changeValue;
                        _randomChangeIndex = MyTools.getRandomNum(0, _attackDiceNum - 1);
                        _攻撃力分割レート[_randomChangeIndex] += _changeValue;
                    }
                }
            }
            _cAD攻撃ダメージ.AddRange(
                    getDividePara・パラメータ合計値をN個に分割(
                    _ASum攻撃力合計値, s_AtackDiceNumAddBonus攻撃マス分割数増加による合計値ボーナス率,
                    _attackDiceNum, _攻撃力分割レート));
            //_cAD攻撃ダメージ.Add((int)(_01攻撃力 * s1_attaDevN_1マスの基本攻撃力倍率 * 1.0));
            //_cAD攻撃ダメージ.Add((int)(_01攻撃力 * s1_attaDevN_1マスの基本攻撃力倍率 * 1.2));
            //_cAD攻撃ダメージ.Add((int)(_01攻撃力 * s1_attaDevN_1マスの基本攻撃力倍率 * 1.1));
            //_cAD攻撃ダメージ.Add((int)(_01攻撃力 * s1_attaDevN_1マスの基本攻撃力倍率 * 0.7));
            
            // （）クリティカル率を作成
            //int _critilcalNum = _attackDiceNum;
            double _CSumクリ率合計値 = _06クリティカル率 * _attackDiceNum;
            _cCPクリ率.AddRange(
                getDividePara・パラメータ合計値をN個に分割(
                _CSumクリ率合計値, s_AtackDiceNumAddBonus攻撃マス分割数増加による合計値ボーナス率, _attackDiceNum, 
                1.0, 0.8, 0.9, 0.7, 0.95, 1.1));
            
            // ●残りの防御・回避を，攻撃マスに混ぜて生成
            for (int i = 0; i <= _attackDiceNum - 1; i++)
            {
                int _atkValue = MyTools.getListValue(_cAD攻撃ダメージ, i);
                string _rank = getParaRank・パラランクＥ＿Ｓ(_atkValue, 100.0); // ■ランクＥ基準値
                // 攻撃力の％によって、名前を付ける
                string _name = "通常攻撃";
                double _rate = (double)_atkValue / (double)_attack;
                if (_rate < 0.1) { _name = "挑発"; }
                else if (_rate < 0.2) { _name = "微弱攻撃"; }
                else if (_rate < 0.3) { _name = "微弱攻撃"; }
                else if (_rate < 0.4) { _name = "微弱攻撃"; }
                else if (_rate < 0.5) { _name = "弱攻撃"; }
                else if (_rate < 0.6) { _name = "弱攻撃"; }
                else if (_rate < 0.7) { _name = "弱攻撃"; }
                else if (_rate < 0.8) { _name = "弱攻撃"; }
                else if (_rate < 0.9) { _name = "弱攻撃"; }
                else if (_rate < 1.0) { _name = "弱攻撃"; }
                else if (_rate < 1.1) { _name = "中攻撃"; }
                else if (_rate < 1.2) { _name = "中攻撃"; }
                else if (_rate < 1.3) { _name = "強攻撃"; }
                else if (_rate < 1.4) { _name = "強攻撃"; }
                else if (_rate < 1.5) { _name = "強攻撃"; }
                else if (_rate < 1.7) { _name = "特攻撃"; }
                else if (_rate < 2.0) { _name = "特攻撃"; }
                else if (_rate < 3.0) { _name = "激攻撃"; }
                else if (_rate < 4.0) { _name = "激攻撃"; }
                else if (_rate < 5.0) { _name = "超攻撃"; }
                // [TODO]攻撃に含まれる防御・回避コマンドの順序をランダムソート？
                _コマンドID++;
                List<string> _attackRandomMessages = new List<string>(); // ここでこう書いてもいい。new List<string>() {"[キャラ名]は防御している。" , "[キャラ名]は身を守っている…", "[キャラ名]は"};
                _attackCommand.Add(new CDiceCommand・ダイスコマンド(_コマンドID, _name+_rank,
                    new CBattleAction・戦闘行動(_attackRandomMessages, EBattleActionType・行動タイプ.t01_ＨＰダメ, EBattleActionObject・攻撃対象.t01_敵単,
                    _atkValue,
                    MyTools.getListValue(_cCPクリ率, i),
                    MyTools.getListValue(_cAP回避率, 0), MyTools.getListValue(_cDD防御ダメージ, 0))));
                // 使った防御・回避コマンド[0]をリストから削除
                if (_cDD防御ダメージ.Count > 0)
                {
                    _cDD防御ダメージ.RemoveAt(0);
                }
                if (_cAP回避率.Count > 0)
                {
                    _cAP回避率.RemoveAt(0);
                }
            }



            // ●●●全てのマスを，ダイスコマンドとして追加
            List<CDiceCommand・ダイスコマンド> _d・ダイスコマンド = new List<CDiceCommand・ダイスコマンド>();
            // ●一番大きい防御マス・回避マスがあれば追加
            _d・ダイスコマンド.AddRange(_diffenceCommand);
            _d・ダイスコマンド.AddRange(_avoidCommand);
            // ●残りの防御・回避が含まれる，攻撃マスを追加
            for (int i = 0; i <= _attackDiceNum - 1; i++)
            {
                _d・ダイスコマンド.Add(_attackCommand[i]);
            }

            List<string> _diceText = new List<string>(_d・ダイスコマンド.Count);
            if (_isFirst初めてダイスを生成 == true)
            {
                // ●ランダムに並び替え
                foreach (CDiceCommand・ダイスコマンド _command in _d・ダイスコマンド)
                {
                    _diceText.Add(_command.getp_Text・詳細());
                }
                Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "ランダムソート前: " + MyTools.getListValues_ToLines(_c.name名前() + "のダイスコマンド(" + _d・ダイスコマンド.Count + "個)\n", _diceText, true));

                MyTools.sortList(_d・ダイスコマンド, MyTools.ESortType.ランダム);
            }
            else
            {
                // ●２回目以降はダイス順は変更なし
                int _diceNum = _d・ダイスコマンド.Count;
                // このキャラダイス順のコマンド名に並び替え
                List<CDiceCommand・ダイスコマンド> _d2・順番を初めて作った順に並び変えたダイスコマンド = new List<CDiceCommand・ダイスコマンド>();
                for (int i = 0; i <= _diceNum - 1; i++)
                {
                    for (int j = 0; j <= _diceNum - 1; j++)
                    {
                        int _２回目以降のコマンドID = _d・ダイスコマンド[j].getp_id();
                        if (_２回目以降のコマンドID == _キャラダイス順のコマンドID[i])
                        {
                            _d2・順番を初めて作った順に並び変えたダイスコマンド.Add(_d・ダイスコマンド[j]);
                        }
                    }
                }
                _d・ダイスコマンド = _d2・順番を初めて作った順に並び変えたダイスコマンド;
            }
            _diceText.Clear();
            foreach (CDiceCommand・ダイスコマンド _command in _d・ダイスコマンド)
            {
                _diceText.Add(_command.getp_Text・詳細());
            }
            //Program.printlnLog(ELogType.l4_重要なデバッグ, "ランダムソート後: " + MyTools.getListValues_ToLines(_chara.name名前() + "のダイスコマンド(" + _d.Count + "個)\n", _diceText, true));

            // 生成時間を計算2
            long _time2 = MyTime.getNowTimeAndMSec_NumberOnly();
            long _timespan1to2 = MyTime.getDifferenceMSec_ByNumberOnly(_time1, _time2);
            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "このキャラクターのダイス生成時間：　" + _timespan1to2 + "ミリ秒");

            // ●●●キャラに生成したダイスを代入！
            _c.setP_dice・所有ダイス(_d・ダイスコマンド);
        }
        #endregion
        #region パラメータ合計値をN個に分割する: dividePara***
        /// <summary>
        /// ダイスコマンドの「攻撃力」「防御力」「回避率」などのパラメータ合計値を，複数マスに分割して振り分けるときの処理です．分割値は四捨五入されます。
        /// 分割数が多いほど，パラメータ合計値が「パラメータ合計値 * (合計値ボーナス率)」だけ増加する、「合計値ボーナス率」にも対応しています．
        /// 
        /// 　※合計値ボーナス率を設けない場合は、nullを指定してください（合計ボーナス率=1.0）になります。
        /// </summary>
        /// <param name="_baseParaSum・パラメータ合計値">例：攻撃力合計値1000を5分割する場合：　1000</param>
        /// <param name="_paraSumBonusRates・分割数増加による合計値ボーナス率">例：攻撃力合計値1000を5分割する場合、+1分割毎に10％合計値ボーナス率（5分割で1.4倍）をつけるとしたら：　new double[]{1.0, 1.1, 1.2, 1.3, 1.4};</param>
        /// <param name="_N・分割数">例：攻撃力合計値1000を5分割する場合：　5</param>
        /// <param name="_devidedRate_等分割した基準値を１としたそれぞれの値の倍率を列挙_合計でNになるように調整">例：攻撃力合計値1000を5分割する場合、１つ強いの400を用意して後４つは同じにするとしたら：　400, 150, 150, 150, 150, 150</param>
        /// <returns></returns>
        public static List<int> getDividePara・パラメータ合計値をN個に分割(double _baseParaSum・パラメータ合計値, double[] _paraSumBonusRates・分割数増加による合計値ボーナス率, int _N・分割数, params double[] _devidedRate_等分割した基準値を１としたそれぞれの値の倍率を列挙_合計でNになるように調整)
        {
            // 分割後の各マス毎のパラメータ
            List<int> _dividedParas = new List<int>(_N・分割数);

            double _bonusRate = 1.0;
            if (_N・分割数 <= _paraSumBonusRates・分割数増加による合計値ボーナス率.Length - 1)
            {
                _bonusRate = _paraSumBonusRates・分割数増加による合計値ボーナス率[_N・分割数];
            }
            // パラメータ合計値にボーナス率を加算して、（振り分け）残りパラメータ合計値とする
            double _RestParaSum = _baseParaSum・パラメータ合計値 * (_bonusRate);
            int _RestN = _N・分割数;
            int _OnePara = 0;
            double _rate = 1.0;
            for (int i = 0; i <= _N・分割数 - 1; i++)
            {
                _rate = MyTools.getArrayValue(_devidedRate_等分割した基準値を１としたそれぞれの値の倍率を列挙_合計でNになるように調整, i);
                if (_rate == 0.0)
                {
                    _rate = 1.0;
                }
                // 残ったパラ合計の分割値をその倍率で振り分けるか，残ったパラ合計値をそのまま代入
                _OnePara = MyTools.getSisyagonyuValue(Math.Min((_RestParaSum / _RestN) * _rate, _RestParaSum));
                _dividedParas.Add(_OnePara);

                _RestParaSum -= _OnePara;
                if (_RestParaSum <= 0)
                {
                    // パラ合計値が無くなり次第，振り分け終了
                    break;
                }
                else if (i == _N・分割数 - 1 && _RestParaSum > 0)
                {
                    // もう振り分けが終了するのにパラ合計値が余ってしまっていた場合，最後の振り分けマスに加算
                    _dividedParas[_N・分割数 - 1] += (int)_RestParaSum;
                    break;
                }
                // パラ合計値の分割数を-1する
                _RestN--;
            }
            string _paras = MyTools.toStringCSV_Indexes(_dividedParas, true);
            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, _baseParaSum・パラメータ合計値 + " を {" + _paras + "}に分割");

            return _dividedParas;
        }
        #endregion
        #region パラメータのＥ＿Ｓのランク付け: getParaRank
        /// <summary>
        /// 引数２のパラメータの基準値をＥとして、引数１の値のランクをＥ～Ｓで判定して返します。（現在は、Ｅ1倍～Ｄ５倍～Ｃ１０～Ｂ５０～Ａ１００～Ｓ２５５倍）
        /// </summary>
        /// <param name="_paraRankEValue・パラＥ基準値"></param>
        /// <returns></returns>
        public static string getParaRank・パラランクＥ＿Ｓ(double _value, double _paraRankEValue・パラＥ基準値)
        {
            double _paraEValue = _paraRankEValue・パラＥ基準値;
            string _rank = "Ｅ";
            if (_value < 1.0 * _paraEValue)
            {
                _rank = "Ｅ";
            }
            else if (_value < 5.0 * _paraEValue)
            {
                _rank = "Ｄ";
            }
            else if (_value < 10.0 * _paraEValue)
            {
                _rank = "Ｃ";
            }
            else if (_value < 50.0 * _paraEValue)
            {
                _rank = "Ｂ";
            }
            else if (_value < 100.0 * _paraEValue)
            {
                _rank = "Ａ";
            }
            else if (_value < 255.0 * _paraEValue)
            {
                _rank = "Ｓ";
            }
            else if (_value < 1000.0 * _paraEValue)
            {
                _rank = "Ｚ";
            }
            else
            {
                _rank = "？";
            }
            return _rank;
        }
        #endregion




    }

}
