using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PublicDomain;

namespace PublicDomain
{
    /// <summary>
    /// 戦闘ダメージのテスト計算機です．
    /// 
    /// 実際のダメージは，これを[1～限界ダメージld]の範囲に調整したもの．なお，マイナスの時は0か1（どちらを取るかはランダムで５０％）。
    ///
    /// ●みんつくのオリジナルダメージ計算式：
    /// 物理ダメージDamage = (int)(((A*B*C*0.50)^_e - (D*E*0.25)^_e) * (1-F)*2.0 +  R) + 0.5)　・・・①
    ///
    ///                    = 整数値( ( (物理攻撃力*威力率*補正値 * 物理攻撃強化定数0.50)のエスカレーション係数e乗
    ///                                   - (相手の守備力*補正値 * 物理防御強化定数0.25)のエスカレーション係数e乗)
    ///                        *(1-相手の物理ダメージ軽減率)*物理ダメージ強化定数2.0 + ダメージ乱数幅R)
    ///                       + 四捨五入0.5 )
    ///
    /// 
    ///・物理ダメージDamage： 実際に敵に与えるダメージ数．　（限界ダメージは，シナリオが進む／キャラ固有の隠し能力，光・闇覚醒で一時的に開放）
    ///（エスカレーション例：　3　→　150　→　999　→　9999　→　99999　→　1230056＜＜→　8億123万？＞＞）
    ///
    ///・物理攻撃力A：　　　　基本は，ちから（赤色パラメータ）から計算されたキャラ攻撃力＋武器攻撃力．　能力修正を考慮すると，(キャラ攻撃力*キャラ攻撃力影響率 + 武器攻撃力)*能力乗除値+能力増減値．
    ///（エスカレーション例：　ちから25+武器10　→　ちから100+武器200　→　ちから250+武器750　→　キャラ600+武器1500　→　キャラ1000+武器5000）
    ///
    ///・威力率B（威力　   ：　技などの威力．　威力100だと等倍（シンプルな通常攻撃），威力200だと２倍．（0～500という値表記だが，内部では威力率として0～5.0の％として計算） 　
    ///（エスカレーション例：　100　→　150　→　220　→　300　→　500）
    ///
    ///・攻撃補正値C ：       攻撃力ＵＰ効果による増減または倍率の補正値．
    ///　　　　　　　　　　　　実際の式は C = 乗除値M+増減値N/(物理攻撃力A*威力B)
    ///                   　　　　　　（N，Mはそれぞれ(A*B)を＋M，N倍する、という意味）
    ///
    ///・相手の守備力D：　　　基本は，みのまもり（黄色パラメータ）から計算されたキャラ守備力＋防具守備力．
    /// 
    ///・守備補正値E：　　　　守備力ＵＰ効果による増減または倍率の補正値．
    /// 
    ///・相手の物理ダメージ軽減率F：　ダメージ軽減率．例えば，防御の場合は0.5で，受けるダメージが半分になる．回避にミスった時は２倍にしたりも出来る。
    /// 
    ///・エスカレーション係数e：　通常は1.00～1.2。攻撃力や守備力の絶対値が大きいほど、
    ///                           同じ（攻撃力－守備力）の差でも，ダメージが指数関数的に跳ね上がらせることができる係数．
    ///                         （例えば、現実世界では0.5，ファンタジー世界では1.0，異空間では1.5？，力の世界では2.0，エスカレーションを見せたい裏面や宇宙空間なら3.0？とかにするとものすごい違いケタ数に．．※つまり，差が大きいほどダメージが大きくなるので，攻撃力が強いものが圧倒的に有利になる（力の世界）．）
    ///（エスカレーション例：　攻撃力=100, 守備力=50とき：　_e=0.5なら、((100*0.5)^0.5 - (50*0.25)^0.5))*2 = 7
    ///                                                 →　_e=1.0なら、((100*0.5)^1.0 - (50*0.25)^1.0))*2  = 75
    ///                                                 →　_e=1.2なら、((100*0.5)^1.2 - (50*0.25)^1.2))*2 = 177
    ///                                                 →　_e=1.5なら、((100*0.5)^1.5 - (50*0.25)^1.5))*2 = 618
    ///                                                 →　_e=2.0なら、((100*0.5)^2.0 - (50*0.25)^2.0))*2 = 4687
    ///
    ///・ダメージ幅乱数R：    最小ダメージ～最大ダメージまでの幅を決める乱数。
    ///                     　 実際の式は、 R = ±(①-r)*(バランス率b) 。
    ///                     　　乱数がいい目を出やすくするキャラとしにくいキャラの差別化を測りたいなら，器用さ・集中力などから計算された再計算数d回振りなおしたときのmaxを取るという方法もある（詳しくは振り直し回数を参照）。　　
    ///                   （バランス（破壊）率bは値のバラツキ．普通5％程度，安定度の悪い武器なら20～50％。覚醒時などあり得ない場合は100～1000％の値を取り，物理ダメージを±(b/100)倍する）
    ///
    /// 
    ///  ※ちなみに，ドラクエ５では， 物理攻撃強化定数は0.50，物理防御強化定数は0.25，
    ///                               乱数Rは，－ダメージ定数*(1/32)～＋ダメージ定数*(1/32) くらいがちょうどいいみたい．
    /// </summary>
    public partial class FDamageCalculator : Form
    {
        public static string p_PlayerName = "勇者";
        public static double p_fa・物理攻撃力 = 100;
        public static double p_fac1・物理補正パーセント = 100;
        public static double p_fac2・物理補正値 = 0;
        public static double p_fp・威力 = 100;
        public static string p_fskillName・技名 = "攻撃";
        public static double p_c・クリティカル率 = 10;
        public static double p_c・クリティカル威力パーセント = 150;
        public static double p_c・会心率 = 5;
        public static double p_c・会心威力パーセント = 200;
        public static double p_c・奇跡まぐれ率 = 1;
        public static double p_c・奇跡まぐれ威力パーセント = 300;

        public static string p_EnemyName = "魔王";
        public static double p_fg・相手の守備力 = 50;
        public static double p_fgc1・相手の守備補正パーセント = 100;
        public static double p_fgc2・相手の守備補正値 = 0;
        public static double p_fr・相手の物理ダメージ軽減パーセント = 10;

        public static double p_faw・物理攻撃強化パーセント = 50; // ＤＱは50
        public static double p_fgw・物理防御強化パーセント = 25; // ＤＱは25
        public static double p_fdw・ダメージ強化パーセント = 200; // ＤＱは50

        public static double p_e・エスカレーション指数係数 = 1.0; // これを変更すると、攻撃力が上がるたびにダメージが一気に上がる

        public static bool p_isRandom・ランダムダメージ = true; // これをfalseにすると固定ダメージ
        public static double p_b・ダメージバランスパーセント = 20; // ダメージ安定率（％）。増減率のパーセント表示。
        public static double p_d・ダメージバラツキ振りなおし回数 = 10.0; // 一回の攻撃で何回攻撃するか（ダメージのバラツキを調べる回数）
        public static bool p_isMinDamegeRandom・ダメージが少ない時も増減値を変動させるか = true;
        public static bool p_isRandomRateSeiki_増減確率分布＿trueは正規分布_falseは等確立分布 = false;

        /// <summary>
        /// 最後に計算した物理ダメージを格納しています。
        /// </summary>
        public static long p_lastDamage・受けたダメージ = 0;
        /// <summary>
        /// 最後に計算した物理ダメージ中間値を格納しています。
        /// </summary>
        public static double p_lastDamageMID・物理ダメージ中間値 = 0.0;
        /// <summary>
        /// 最後に計算した合計物理ダメージを格納しています。
        /// </summary>
        public static double p_lastDamageSUM・受けたダメージ総数 = 0.0;
        /// <summary>
        /// 最後に計算した物理ダメージ計算式の詳細を格納しています。
        /// </summary>
        public static string p_lastDamageCalcResult・ダメージ計算結果 = "";

        /// <summary>
        /// 物理ダメージを計算して返します。
        /// </summary>
        public static double getFhysicalDamage(double _攻撃力, double _相手の守備力, bool _isクリティカル, bool _is会心, bool _is奇跡orまぐれ)
        {
            return getFhysicalDamage(_攻撃力, _相手の守備力, p_fdw・ダメージ強化パーセント/100.0, p_e・エスカレーション指数係数, _isクリティカル, _is会心, _is奇跡orまぐれ, p_isRandom・ランダムダメージ, p_isMinDamegeRandom・ダメージが少ない時も増減値を変動させるか, p_b・ダメージバランスパーセント/100.0, p_isRandomRateSeiki_増減確率分布＿trueは正規分布_falseは等確立分布);
        }
        /// <summary>
        /// 物理ダメージを計算して返します。
        /// </summary>
        public static double getFhysicalDamage(double _攻撃力, double _相手の守備力, double _ダメージ倍率＿基本は１, double _増減率＿基本は０＿０５, bool _isクリティカル, bool _is会心, bool _is奇跡orまぐれ)
        {
            return getFhysicalDamage(_攻撃力, _相手の守備力, _ダメージ倍率＿基本は１, p_e・エスカレーション指数係数, _isクリティカル, _is会心, _is奇跡orまぐれ, p_isRandom・ランダムダメージ, p_isMinDamegeRandom・ダメージが少ない時も増減値を変動させるか, _増減率＿基本は０＿０５, p_isRandomRateSeiki_増減確率分布＿trueは正規分布_falseは等確立分布);
        }
        /// <summary>
        /// 物理ダメージを計算して返します。
        /// </summary>
        public static double getFhysicalDamage(double _攻撃力, double _相手の守備力, double _ダメージ倍率＿基本は１, double _e・エスカレーション指数係数＿基本は１＿００から１＿１０位, bool _isクリティカル, bool _is会心, bool _is奇跡orまぐれ, bool _isランダムダメージtrue増減＿false固定, bool _ダメージが少ない時も増減値を変動させるか＿trueはさせる＿falseは固定値になる, double _増減率＿基本は０＿０５, bool _増減確率分布＿trueは正規分布_falseは等確立分布)
        {
            return getFhysicalDamage(_攻撃力, p_fp・威力/100.0, _相手の守備力, p_fgw・物理防御強化パーセント/100.0, _ダメージ倍率＿基本は１, _e・エスカレーション指数係数＿基本は１＿００から１＿１０位, _isクリティカル, _is会心, _is奇跡orまぐれ, _isランダムダメージtrue増減＿false固定, _ダメージが少ない時も増減値を変動させるか＿trueはさせる＿falseは固定値になる, _増減率＿基本は０＿０５, _増減確率分布＿trueは正規分布_falseは等確立分布);
        }
        /// <summary>
        /// 物理ダメージを計算して返します。
        /// ※補正値は別に_攻撃力や_守備力に含んでもいいです。ちゃんと計算式の文字列に表示したいわけじゃなければ。
        /// </summary>
        public static double getFhysicalDamage(double _攻撃力, double _威力率, double _相手の守備力, double _相手の物理ダメージ軽減率, double _ダメージ倍率＿基本は１, double _e・エスカレーション指数係数＿基本は１＿００から１＿１０位, bool _isクリティカル, bool _is会心, bool _is奇跡orまぐれ, bool _isランダムダメージtrue増減＿false固定, bool _ダメージが少ない時も増減値を変動させるか＿trueはさせる＿falseは固定値になる, double _増減率＿基本は０＿０５, bool _増減確率分布＿trueは正規分布_falseは等確立分布)
        {
            // 補正値は別に_攻撃力や_守備力に含んでもいいです。ちゃんと計算式の文字列に表示したいわけじゃなければ。
            p_fa・物理攻撃力 = _攻撃力;
            p_fp・威力 = _威力率 * 100; // パーセント表記にする
            p_fg・相手の守備力 = _相手の守備力;
            p_fr・相手の物理ダメージ軽減パーセント = _相手の物理ダメージ軽減率 * 100; // パーセント表記にする
            p_fdw・ダメージ強化パーセント = _ダメージ倍率＿基本は１ * 200; // １で200とする
            p_e・エスカレーション指数係数 = _e・エスカレーション指数係数＿基本は１＿００から１＿１０位;

            p_isRandom・ランダムダメージ = _isランダムダメージtrue増減＿false固定;
            p_b・ダメージバランスパーセント = _増減率＿基本は０＿０５ * 100; // パーセント表記にする
            p_isMinDamegeRandom・ダメージが少ない時も増減値を変動させるか = _ダメージが少ない時も増減値を変動させるか＿trueはさせる＿falseは固定値になる;
            p_isRandomRateSeiki_増減確率分布＿trueは正規分布_falseは等確立分布 = _増減確率分布＿trueは正規分布_falseは等確立分布;

            return getFhysicalDamage(_isクリティカル, _is会心, _is奇跡orまぐれ);
        }
        /// <summary>
        /// 物理ダメージを計算して返します。引数でクリティカルの有無を指定できます。
        /// </summary>
        public static double getFhysicalDamage(bool _isクリティカル無し＿true通常攻撃のみ＿falseだと会心や奇跡まぐれもあり)
        {
            double _クリティカル率 = p_c・クリティカル率 / 100.0; // 通常は１０％～５０％位
            bool _isクリティカル = false;
            //double _クリティカル乗除定数 = p_c・クリティカル威力パーセント / 100.0; // 通常は1.5
            double _会心率 = p_c・会心率 / 100.0; // 通常は３％～５％位
            bool _is会心 = false;
            //double _会心乗除定数 = p_c・会心威力パーセント / 100.0; // 通常は2.0 
            double _奇跡まぐれ率 = p_c・奇跡まぐれ率 / 100.0; // 通常は１％位
            bool _is奇跡まぐれ = false;
            //double _奇跡まぐれ乗除定数 = p_c・奇跡まぐれ威力パーセント / 100.0; // 通常は3.0 
            
            // クリティカル・会心・奇跡まぐれ判定は、それぞれ別々に判定（３連続同時になる場合もある）
            if (_isクリティカル無し＿true通常攻撃のみ＿falseだと会心や奇跡まぐれもあり == false)
            {
                // クリティカル判定
                double _rCritical = 0.01 * (double)MyTools.getRandomNum(0, 100);
                if (_rCritical <= _クリティカル率)
                {
                    _isクリティカル = true;
                }
                // 会心判定
                double _rKaishin = 0.01 * (double)MyTools.getRandomNum(0, 100);
                if (_rKaishin <= _会心率)
                {
                    _is会心 = true;
                }
                // 奇跡・まぐれ判定
                double _rKisekiOrMagure = 0.01 * (double)MyTools.getRandomNum(0, 100);
                if (_rKisekiOrMagure <= _奇跡まぐれ率)
                {
                    _is奇跡まぐれ = true;
                }
            }
            return getFhysicalDamage(_isクリティカル, _is会心, _is奇跡まぐれ);
        }
        /// <summary>
        /// 物理ダメージを計算して返します。引数に指定したisクリティカルかどうか…などに従ったクリティカル判定でダメージを計算します。
        /// </summary>
        public static double getFhysicalDamage(bool _isクリティカル, bool _is会心, bool _is奇跡orまぐれ)
        {
            double _ダメージ中間値 = 0.0; // 計算されたダメージの中間値。受けるダメージにはこれに乱数が加わる
            // intの最大値は約21億（9ケタまで）。doubleはケタ数なので最大値は308ケタまでなので、doubleを採用。
            // 受けるダメージが、設定された最大ダメージを超えた場合は、最大ダメージが適応される
            double _受けるダメージ = 0;
            double _最小ダメージ = 0; // 最小ダメージ。後に受けるダメージランダムの最大ダメージの代入にも使われる。
            double _最大ダメージ = 999999999; // 最大ダメージ。後に受けるダメージランダムの最大ダメージの代入にも使われる。
            string _damageCalcResult = ""; // デバッグ時、ダメージ計算式の詳細な代入結果を格納した文字列

            double _攻撃力 = p_fa・物理攻撃力;
            double _攻撃補正率 = p_fac1・物理補正パーセント / 100.0;
            double _攻撃補正値 = p_fac2・物理補正値;
            double _威力率 = p_fp・威力 / 100.0;
            //double _総合攻撃力 = (p_fa・物理攻撃力 * _威力率) * _攻撃補正率 + _攻撃補正値;

            double _守備力 = p_fg・相手の守備力;
            double _守備補正率 = p_fgc1・相手の守備補正パーセント / 100.0;
            double _守備補正値 = p_fgc2・相手の守備補正値;
            //double _総合守備力 = p_fg・相手の守備力 * _守備補正率 + _守備補正値;
            double _物理ダメージ軽減率 = p_fr・相手の物理ダメージ軽減パーセント / 100.0;

            double _物理攻撃強化率 = p_faw・物理攻撃強化パーセント / 100.0; // 通常は0.50
            double _物理防御強化率 = p_fgw・物理防御強化パーセント / 100.0; // 通常は0.25
            double _物理ダメージ強化率 = p_fdw・ダメージ強化パーセント / 100.0; // 通常は2.00

            bool _ランダムダメージか = p_isRandom・ランダムダメージ;
            double _R増減率＿基本は０＿０５ = p_b・ダメージバランスパーセント / 100.0;
            bool _ダメージが少ない時も増減値を変動させるか＿trueはさせる＿falseは固定値になる = p_isMinDamegeRandom・ダメージが少ない時も増減値を変動させるか;
            bool _増減確率分布＿trueは正規分布_falseは等確立分布 = p_isRandomRateSeiki_増減確率分布＿trueは正規分布_falseは等確立分布;
            double _eエスカレーション指数 = p_e・エスカレーション指数係数;

            //double _クリティカル率 = p_c・クリティカル率 / 100.0; // 通常は１０％～５０％位
            double _クリティカル乗除定数 = p_c・クリティカル威力パーセント / 100.0; // 通常は1.5
            //double _会心率 = p_c・会心率 / 100.0; // 通常は３％～５％位
            double _会心乗除定数 = p_c・会心威力パーセント / 100.0; // 通常は2.0 
            //double _奇跡まぐれ率 = p_c・奇跡まぐれ率 / 100.0; // 通常は１％位
            double _奇跡まぐれ乗除定数 = p_c・奇跡まぐれ威力パーセント / 100.0; // 通常は3.0             
            // クリティカル・会心・奇跡まぐれ判定は、それぞれ別々に判定（３連続同時になる場合もある）
            if (_isクリティカル)
            {
                _威力率 *= _クリティカル乗除定数;
            }

            if (_is奇跡orまぐれ)
            {
                _威力率 *= _奇跡まぐれ乗除定数;
            }

            if (_is会心)
            {
                _威力率 *= _会心乗除定数;

                // 会心の一撃時の（防御無視ダメージ）判定
                _ダメージ中間値 =
                    (Math.Pow((_攻撃力 * _威力率 * _攻撃補正率 + _攻撃補正値)  *  _物理攻撃強化率, _eエスカレーション指数)
                    - 0.0)
                    * _物理ダメージ強化率;
            }
            else
            {
                // 通常攻撃時（クリティカルや奇跡orまぐれ時も含む）のダメージ判定
                _ダメージ中間値 =
                    (Math.Pow((_攻撃力 * _威力率  * _攻撃補正率 + _攻撃補正値)  *  _物理攻撃強化率, _eエスカレーション指数)
                   - Math.Pow((_守備力            * _守備補正率 + _守備補正値)  *  _物理防御強化率, _eエスカレーション指数))
                    * _物理ダメージ強化率 * (1 - _物理ダメージ軽減率);
                if (Program・実行ファイル管理者.isDebug == true)
                {
                    // デバッグ用に、ダメージ計算式の詳細を格納
                    _damageCalcResult
                        = "※ダメージ計算式\n"
                    + "=  {　(攻撃力" + (long)_攻撃力 + "×威力率" + MyTools.getShownNumber(_威力率, 1, 2, 4, false) + "×攻撃補正率" + MyTools.getShownNumber(_攻撃補正率, 1, 2, 4, false) + "＋攻撃補正値" + (int)_攻撃補正値 + ")  ×  物理攻撃強化率" + MyTools.getShownNumber(_物理攻撃強化率, 1, 2, 4, false) + "  }のe" + MyTools.getShownNumber(_eエスカレーション指数, 1, 2, 4, false) + "乗\n"
                    + "  -{  (守備力" + (long)_守備力 + "×防御率" + MyTools.getShownNumber(1.0, 1, 2, 4, false) + "×守備補正率" + MyTools.getShownNumber(_守備補正率, 1, 2, 4, false) + "＋守備補正値" + (int)_守備補正値 + ")  ×  物理防御強化率" + MyTools.getShownNumber(_物理防御強化率, 1, 2, 4, false) + "  }のe" + MyTools.getShownNumber(_eエスカレーション指数, 1, 2, 4, false) + "乗  ×  {  物理ダメージ強化率" + MyTools.getShownNumber(_物理ダメージ強化率, 1, 2, 4, false) + "×(1－物理ダメージ軽減率" + MyTools.getShownNumber(_物理ダメージ軽減率, 1, 2, 4, false) + ")  }\n"
                    + "\n"
                    + "= {" + (_攻撃力 * _威力率 * _攻撃補正率 + _攻撃補正値) * _物理攻撃強化率 + "}のe" + MyTools.getShownNumber(_eエスカレーション指数, 1, 2, 4, false) + "乗－{" + (_守備力 * _守備補正率 + _守備補正値) * _物理防御強化率 + "}のe" + MyTools.getShownNumber(_eエスカレーション指数, 1, 2, 4, false) + "乗  ×  {  物理ダメージ強化率" + MyTools.getShownNumber(_物理ダメージ強化率, 1, 2, 4, false) + "×(1－物理ダメージ軽減率" + MyTools.getShownNumber(_物理ダメージ軽減率, 1, 2, 4, false) + ")  }\n"
                    + "= {" + (long)Math.Pow((_攻撃力 * _威力率 * _攻撃補正率 + _攻撃補正値) * _物理攻撃強化率, _eエスカレーション指数) + "}－{" + (long)Math.Pow((_守備力 * _守備補正率 + _守備補正値) * _物理防御強化率, _eエスカレーション指数) + "}  ×  {  物理ダメージ強化率" + MyTools.getShownNumber(_物理ダメージ強化率, 1, 2, 4, false) + "×(1－物理ダメージ軽減率" + MyTools.getShownNumber(_物理ダメージ軽減率, 1, 2, 4, false) + ")  }\n"
                    + "= {" + (long)(Math.Pow((_攻撃力 * _威力率 * _攻撃補正率 + _攻撃補正値) * _物理攻撃強化率, _eエスカレーション指数) - Math.Pow((_守備力 * _守備補正率 + _守備補正値) * _物理防御強化率, _eエスカレーション指数)) + "}  ×  {" + MyTools.getShownNumber(_物理ダメージ強化率 * (1 - _物理ダメージ軽減率), 1, 2, 4, false) + "}\n"
                    +"";
                    //MyTools.ConsoleWriteLine(_damageCalcResult);
                }
            }
            if (_ランダムダメージか == true)
            {
                _受けるダメージ = CDamageCalc・ダメージ計算機.getRandomDamage(_ダメージ中間値, _R増減率＿基本は０＿０５, _ダメージが少ない時も増減値を変動させるか＿trueはさせる＿falseは固定値になる, _増減確率分布＿trueは正規分布_falseは等確立分布,
                    out _最小ダメージ, out _最大ダメージ);
                // デバッグ用に、ランダムダメージの幅を格納
                _damageCalcResult += "= "+(long)_ダメージ中間値 + "ダメージ中間値　　　( [" + (long)_最小ダメージ + "～" + (long)_最大ダメージ + "]の内、" + (long)_受けるダメージ + "を採用)";
            }
            else
            {
                _受けるダメージ = _ダメージ中間値;
            }
            // 計算式の詳細を格納
            p_lastDamageCalcResult・ダメージ計算結果 = _damageCalcResult;
            p_lastDamage・受けたダメージ = (long)_受けるダメージ;
            p_lastDamageMID・物理ダメージ中間値 = _ダメージ中間値;
            return _受けるダメージ;
        }
        
        /// <summary>
        /// 物理ダメージやクリティカル結果などを計算して返します。クリティカル判定などは内部で計算します。
        /// </summary>
        /// <returns></returns>
        public static CAttackResult・戦闘結果 getAttackResult_FhysicalDamage()
        {
            CAttackResult・戦闘結果 _result = new CAttackResult・戦闘結果();
            double _受けるダメージ = 0;

            double _クリティカル率 = p_c・クリティカル率 / 100.0; // 通常は１０％～５０％位
            double _クリティカル乗除定数 = p_c・クリティカル威力パーセント / 100.0; // 通常は1.5
            double _会心乗除定数 = p_c・会心威力パーセント / 100.0; // 通常は2.0 
            double _会心率 = p_c・会心率 / 100.0; // 通常は３％～５％位
            double _奇跡まぐれ乗除定数 = p_c・奇跡まぐれ威力パーセント / 100.0; // 通常は3.0 
            double _奇跡まぐれ率 = p_c・奇跡まぐれ率 / 100.0; // 通常は１％位

            // クリティカル判定
            double _rCritical = 0.01 * (double)MyTools.getRandomNum(1, 100);
            if (_rCritical <= _クリティカル率)
            {
                _result.isCritical = true;
            }
            // 会心判定
            double _rKaishin = 0.01 * (double)MyTools.getRandomNum(1, 100);
            if (_rKaishin <= _会心率)
            {
                _result.isKaishin = true;
            }
            // 奇跡・まぐれ判定
            double _rKisekiOrMagure = 0.01 * (double)MyTools.getRandomNum(1, 100);
            if (_rKisekiOrMagure <= _奇跡まぐれ率)
            {
                _result.isKisekiOrMagure = true;
            }

            // 受けるダメージを計算
            _受けるダメージ = getFhysicalDamage(_result.isCritical, _result.isKaishin, _result.isKisekiOrMagure);
            _result.damage = _受けるダメージ;
            //Console.WriteLine("物理ダメージ : " + _受けるダメージ + " (= ダメージ定数 " + p_lastDamageMID・物理ダメージ中間値 + " + 乱数 " + _増減率);

            return _result;
        }

        //public FDamageTest()
        //{
        //    InitializeComponent();
        //}
        CGameManager・ゲーム管理者 game = null;
        public FDamageCalculator(CGameManager・ゲーム管理者 _game)
        {
            game = _game;
            game.setP_FDamage・ダメージ調整フォーム(this);
            InitializeComponent();
        }




        public void attack()
        {
            for (int i = 1; i <= p_d・ダメージバラツキ振りなおし回数; i++)
            {
                CAttackResult・戦闘結果 _a = getAttackResult_FhysicalDamage();
                p_lastDamage・受けたダメージ = (long)_a.damage;
                string _クリティカル有無 = "";
                if (_a.isCritical == true)
                {
                    _クリティカル有無 = "クリティカルヒット！　";
                }
                string _会心有無 = "";
                if (_a.isKaishin == true)
                {
                    _会心有無 = "会心の一撃！！　";
                }
                string _奇跡有無 = "";
                if (_a.isKisekiOrMagure == true)
                {
                    _奇跡有無 = "奇跡の一撃！！！　";
                }

                // ダメージ定数を表示
                if (i == 1)
                {
                    richTextBox1.Text = p_lastDamageCalcResult・ダメージ計算結果;
                    richTextBox1.Text += "\n*ダメージ定数（一回目のダメージ中間値）: "+(long)p_lastDamageMID・物理ダメージ中間値+"\n";
                }
                // 攻撃回数毎のダメージを表示
                richTextBox1.Text += i + "回目: " + p_PlayerName + "の" + p_fskillName・技名 + "！　"
                    + _クリティカル有無 + _会心有無 + _奇跡有無
                    + p_EnemyName + "に " + p_lastDamage・受けたダメージ + " のダメージ！\n";
                p_lastDamageSUM・受けたダメージ総数 += p_lastDamage・受けたダメージ;
                p_lastDamage・受けたダメージ = 0;

                // 合計ダメージを表示
                if (i == p_d・ダメージバラツキ振りなおし回数)
                {
                    richTextBox1.Text += "*"+i+"回の合計ダメージ: " + (long)p_lastDamageSUM・受けたダメージ総数 + "\n";
                    p_lastDamageSUM・受けたダメージ総数 = 0;
                }
            }
        }

        /// <summary>
        /// 戦闘結果（ダメージやクリティカルの有無，回避など？）を格納するデータです．
        /// </summary>
        public class CAttackResult・戦闘結果{
            public double damage = 0.0;
            public bool isCritical = false;
            public bool isKaishin = false;
            public bool isKisekiOrMagure = false;
            public bool isAvoid = false;
            public void set(double _damage, bool _isCritical, bool _isKaishin, bool _isKisekiOrMagure, bool _isAvoid)
            {
                damage = _damage;
                isCritical = _isCritical;
                isKaishin = _isKaishin;
                isKisekiOrMagure = _isKisekiOrMagure;
                isAvoid = _isAvoid;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            p_fa・物理攻撃力 = MyTools.parseDouble(textBox1.Text);
            attack();
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            p_fac1・物理補正パーセント = MyTools.parseDouble(textBox3.Text);
            attack();
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            p_fac2・物理補正値 = MyTools.parseDouble(textBox4.Text);
            attack();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            p_fp・威力 = MyTools.parseDouble(textBox2.Text);
            attack();
        }
        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            p_faw・物理攻撃強化パーセント = MyTools.parseDouble(textBox15.Text);
            attack();
        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            p_b・ダメージバランスパーセント = MyTools.parseDouble(textBox13.Text);
            attack();
        }




        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            p_fg・相手の守備力 = MyTools.parseDouble(textBox8.Text);
            attack();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            p_fgc1・相手の守備補正パーセント = MyTools.parseDouble(textBox6.Text);
            attack();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            p_fgc2・相手の守備補正値 = MyTools.parseDouble(textBox5.Text);
            attack();
        }
        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            p_fgw・物理防御強化パーセント = MyTools.parseDouble(textBox14.Text);
            attack();
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            p_fr・相手の物理ダメージ軽減パーセント = MyTools.parseDouble(textBox7.Text);
            attack();
        }


        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            p_e・エスカレーション指数係数 = MyTools.parseDouble(textBox9.Text);
            attack();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            attack();
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            p_fskillName・技名 = textBox12.Text;
            attack();
        }




        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            p_PlayerName = textBox10.Text;
        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            p_EnemyName = textBox11.Text;
        }

        private void FDamageTest_Load(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            p_c・クリティカル率 = MyTools.parseDouble(textBox16.Text);
            attack();
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
            p_c・会心率 = MyTools.parseDouble(textBox16.Text);
            attack();
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
            p_c・クリティカル威力パーセント = MyTools.parseDouble(textBox18.Text);
            attack();
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            p_d・ダメージバラツキ振りなおし回数 = MyTools.parseDouble(textBox19.Text);
            attack();
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            p_c・会心威力パーセント = MyTools.parseDouble(textBox20.Text);
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            p_fdw・ダメージ強化パーセント = MyTools.parseDouble(textBox21.Text);
            attack();
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            if (textBox22.Text == "1")
            {
                p_isRandomRateSeiki_増減確率分布＿trueは正規分布_falseは等確立分布 = false;
            }
            else
            {
                p_isRandomRateSeiki_増減確率分布＿trueは正規分布_falseは等確立分布 = true;
            }
            attack();
        }




 
    }
}