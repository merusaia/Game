using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /// <summary>
    /// キャラのパラメータリスト（パラリスト、List＜double＞型で表現される）は、様々な表現を取る時があります。
    /// それをメソッドなどで明示的に指定したり、わかりやすく判別したりする場合に使う、
    /// パラメータリストのタイプを示す列挙体です。
    /// なお、列挙体名の数字は、引数などに使うパラリストの要素数（_paraList.Count）と等しいことを保証させてください）
    /// </summary>
    public enum EParaListType
    {
        /// <summary>
        /// =0。デフォルト値。EPara列挙体に列挙されている全てのパラメータリストであることを示します。
        /// </summary>
        _default_all・全てのEParaパラ,
        /// <summary>
        /// [0]赤～[5]紫の基本６色パラメータリスト
        /// </summary>
        Iro6・基本６色パラ,
        /// <summary>
        /// [0]赤橙～[5]赤紫の中間６色パラメータリスト
        /// </summary>
        IroMID6・中間６色パラ,
        /// <summary>
        /// [0]白～[6]虹色の装飾６色パラメータリスト
        /// </summary>
        IroEXT6・装飾６色パラ,
        /// <summary>
        /// [0]赤～[5]紫、[6]赤橙～[11]赤紫の、基本６色に中間６色を足した、１２色パラメータリスト
        /// </summary>
        Iro12・１２色パラ,
        /// <summary>
        /// [0]赤～[5]紫、[6]赤橙～[11]赤紫、[12]白～虹[17]の、基本６色＋中間６色＋装飾６色を足した、１８色パラメータリスト
        /// </summary>
        Iro18・１８色パラ,
        /// <summary>
        /// [0]？～[Count-1]？（詳しくは_paraList.Count確認して）の戦闘で使うパラメータを集めた、戦闘パラメータリスト
        /// </summary>
        Battle・戦闘パラ,
        //現在は、基本６パラ[0-5]＝身体６パラ[0-5]、で値的に全く等しい。こんがらがるので必要ない。Sin6・身体６パラ,
        //現在は、中間６パラ[0-5]＝精神６パラ[0-5]、で値的に全く等しい。こんがらがるので必要ない。Sei6・精神６パラ,
    }
    /// <summary>
    /// キャラのパラメータ合計値（int型もしくはdouble型）を分割して、
    /// パラメータリスト（List＜double＞型）にする時の、分割方法を指定する列挙体です。
    /// </summary>
    public enum EParaDividedType・パラ分割方法
    {
        _default_EqualToIro6・基本６色パラに６等分割,
        EqualToIro12・１２等分割,
        EqualToIro18・１８等分割,
        RateToIro6・基本６色パラに比例分割,
        RateToIroMID6・中間６色パラに比例分割,
        RateToIroEXT6・装飾６色パラに比例分割,
        RateToIro12・１２色パラに比例分割,
        RateToIro18・１８色パラに比例分割,
    }

    /// <summary>
    /// 特定のパラメータを名前で参照するenumです。このCParas・パラメータ群.csファイルに定義されています。
    /// 
    /// 特定のパラメータをアクセス（取得／代入）する際は、IDの数値を直接入れず、CChara・キャラクラスのオブジェクトで、
    /// ●getPara(EPara.パラメータ名)/setParaValue(EPara.パラメータ名, 新しい値)
    /// などを使ってください（game.以外で呼び出すときに推奨）。
    /// なお、getParaでいちいちパラメータ名を指定するのが面倒な場合は、
    /// 日本語メソッド（tikaちから()など）を使用してください（game.の時に推奨）。
    /// 
    /// ※新しいパラメータ（値はdouble型のみ）を作りたい場合は、このEPara・全パラに列挙するだけで、上記メソッドでアクセスできます。
    /// 　なお、文字列string型が入る可能性のあるパラメータは、EVar・変数という列挙体に定義してください。
    /// 
    /// 　
    /// 
    /// 新しいパラメータを作る場合は、ここEPara列挙体に追加してください。
    /// 
    ///  （注意）：
    ///   EParaに列挙せずに、ゲーム中に即興的にいきなり「setPara("○○○への一時的好感度", 初期値)」などといった、
    ///   新しくパラメータを作る方法は、パラメータではサポートしません。
    /// 　理由は高速化のため、キャラのパラメータはp_parametersのList＜double＞で管理しているからです。
    /// 　  
    ///　 ※キャラ毎に新しい変数を作りたい場合は、パラメータではなく、変数を使ってください。
    ///　 つまり、「setVar("新しい変数名", 初期値)」で新しい変数を適時追加してください。
    /// 　理由は拡張性を高めるため、p_varsは、Dictionary＜Key=string＞で管理しているからです。
    /// 　
    /// つまり、「setVar("新しい変数名", 初期値)」はOKですので、
    /// 　即興変数を使いたい人は、setFeaやsetVarでどんどん自由に作ってください。
    /// 
    ///   なお、bool型だけが入るパラメータは、ESwitch・スイッチという列挙体に使って定義してください。
    /// 
    /// </summary>
    /// <remarks>
    /// なお、ESPara・戦闘パラメータ、身体パラメータなどを、それぞれ個別のクラスに分けない理由は、
    /// ・今後パラメータ数を変更する時にクラス間の連携が複雑にならないようにするため
    /// ・全てのパラメータを、一つのパラメータ群（CPara・パラメータ型のList）にシンプルに管理したい
    /// からです。
    /// </remarks>
    public enum EPara
    {
        // 色パラメータ18色：　現段階では全てのパラメータを創る基本となっている、キャラを構成するパラメータ
        // 基本6色パラメータ: 赤,   橙,   黄,   緑,   青,   紫
        // 中間6色パラメータ: 赤橙, 黄橙, 黄緑, 青緑, 青紫, 赤紫
        // 装飾6色パラメータ: 白,   黒,   銀,   金,   透明, 虹色
        //              基本的に、 （身体能力）基本6色で身体パラメータ、
        //                         （精神能力）中間6色で精神パラメータ、
        //                         （戦闘能力）基本6+中間6色=色彩12色で、戦闘パラメータ、
        //                              を計算している。
        //              装飾6色は、（明度）白黒で光闇パラメータ（ポジティブ～ネガティブなどの気分）の変化、
        //                         （鮮度）金銀で調子パラメータ（金で金運や特技効果や会心率などの絶好調～絶不調、銀でアイテム運や道具効果や武具クリティカル率などの絶好調～絶不調）の変化、
        //                         （透明度）透明で浸透パラメータ（状態異常率や神霊や神々の憑依率など）の変化、
        //                         （多彩度）虹色で身体・精神・戦闘パラメータの動的変化や入れ替え・掛け合わせ、
        //                              などに使っている。
        // 色の選定には例えば以下を参考。http://handywebdesign.net/2012/07/12colors-give-you-the-impression/　http://www.tagindex.com/color/color_wheel.html
        #region 必須★★★ c**_*** 色パラメータと○色合計値
        c01_赤,
        c02_橙,
        c03_黄,
        c04_緑,
        c05_青,
        c06_紫,

        c07_赤橙,
        c08_黄橙,
        c09_黄緑,
        c10_青緑,
        c11_青紫,
        c12_赤紫,

        c13_白,
        c14_黒,
        c15_銀,
        c16_金,
        c17_透明,
        c18_虹色,

        _18色総合値,
        _基本6色総合値,
        _中間6色総合値,
        _装飾6色総合値,
        #endregion
        // LV1時の色パラメータ
        #region 必須★★★ LV1c**_*** LV1時の色パラメータ
        LV1c01_赤,
        LV1c02_赤橙,
        LV1c03_橙,
        LV1c04_黄橙,
        LV1c05_黄,
        LV1c06_黄緑,
        LV1c07_緑,
        LV1c08_青緑,
        LV1c09_青,
        LV1c10_青紫,
        LV1c11_紫,
        LV1c12_赤紫,

        LV1c13_白,
        LV1c14_黒,
        LV1c15_銀,
        LV1c16_金,
        LV1c17_透明,
        LV1c18_虹色,

        LV1c_色18色総合値,
        LV1c_基本6色総合値,
        LV1c_中間6色総合値,
        LV1c_装飾6色総合値,

        #endregion
        // LVと経験値とスキルレベル
        #region 必須★★★ LV*** LVと経験とスキルレベル
        /// <summary>
        /// キャラクタの基本的な戦闘パラメータを決めるLVです。
        /// </summary>
        LV,
        /// <summary>
        /// キャラクタが今まで取得した経験値の合計値です。
        /// </summary>
        LVExpSum_経験値,
        /// <summary>
        /// キャラクタの次のレベルアップまでに必要な経験値（Next）です。
        /// ※この値は、setPara(EPara.LVExp_経験値)をした時に、自動的に更新されます。
        /// 総和ではありません。総和を出したい場合は、Para(EPara.LVExp_経験値) + Para(EPara.LVNext_次のレベルまでに必要な経験値) を計算してください。
        /// </summary>
        LVNext_次のレベルまでに必要な経験値,
        /// <summary>
        /// キャラクタの次のLVUP時などにユーザが手動で振り分け可能な、まだ振り分けていないボーナスパラメータ値を保存しています。
        /// （基本的に、色パラメータに割り振られるために使います。）
        /// </summary>
        LVParaBonus_未振り分けボーナスパラ,
        /// <summary>
        /// キャラクタが今まで獲得したスキルポイントの合計値です。
        /// </summary>
        LVSkilPointSum_スキルポイント,
        /// <summary>
        /// キャラクタの次のLVUP時などにユーザが手動で振り分け可能な、まだ振り分けていないスキルポイント値を保存しています。
        /// （基本的に、スキルレベルを上げるために使います。）
        /// </summary>
        LVSkilBonus_未振り分けスキルポイント,
        #endregion

        // 以下、色パラメータにより計算されている、サブのパラメータ。最悪、定義されていない場合でも再計算可能。

        // 身体6パラメータ：　ちから,感性,素早さ,器用さ,行動力,持久力,精神力
        #region サブ a*_*** 標準（身体）パラ
        /// <summary>
        /// 【赤】ちから：　攻撃力、つば競り合い勝利率に影響
        /// </summary>
        a1_ちから,
        /// <summary>
        /// 【橙】持久力：　HP、（体力、丈夫さ）・・・最大HPに影響
        /// </summary>
        a2_持久力,
        /// <summary>
        /// 【黄】行動力：　防御力、行動ゲージ（＝＞攻撃回数）に影響
        /// </summary>
        a3_行動力,
        /// <summary>
        /// 【緑】素早さ：　行動順番、行動回復量、攻撃速度・魔法速度、回避率（一定の確率でダメージ0、0.25、1、1.25倍）に影響
        /// </summary>
        a4_素早さ,
        /// <summary>
        /// 【青】精神力：　回避率、（知力、想像力、器量）・・・最大SP、魔法防御力に影響
        /// </summary>
        a5_精神力,
        /// <summary>
        /// 【紫】感性：　クリティカル率、魔法攻撃力、魔法防御（弱）、精神対立勝利率に影響
        /// </summary>
        a6_賢さ,
        #endregion
        // 精神6パラメータ：　行動力、思考力、適応力、集中力、忍耐力、健康管理力
        #region サブ　b*_*** 応用（精神）パラ　（青年期になってから？）
        /// <summary>
        /// 【赤橙、朱】器用さ：　命中率、ダメージの安定度（ダメージ乱数を1～10回振って最大値を取る）、回避率（弱）、コンボや連携攻撃でのタイミングの取りやすさ（？）に影響
        /// </summary>
        b1_器用さ,
        /// <summary>
        /// 【山吹】忍耐力：　（タフさ）状態異常耐性、瀕死時の物理防御力・魔法防御力・回避率・受け止め率UP量、戦闘不能復帰率（根性）に影響
        /// </summary>
        b2_忍耐力,
        /// <summary>
        /// 【黄緑】健康力：　包容できる許容量、自然HP回復量、自然状態変化回復速度に影響
        /// </summary>
        b3_健康力,
        /// <summary>
        /// 【青緑、エメラルド】適応力：　（柔軟性、協調性）、味方と連携のしやすさ、相手の精神世界への適応確率、魔法防御力（弱）、に影響
        /// </summary>
        b4_適応力,
        /// <summary>
        /// 【青紫】集中力：　決め技などの大事な一撃でのダメージUP+安定度（乱数をプラスにしやすくする＝バランス破壊率を増加させて、さらに1～10回振ったときの最大値を取る）量に影響
        /// </summary>
        b5_集中力,
        /// <summary>
        /// 【赤紫】思考力：　善悪を見分ける力、（理解力、論理思考）敵の同じ攻撃・魔法を防ぐ確率Up、精神対立時の思考スピード、AI戦闘の賢さに影響
        /// </summary>
        b6_思考力,

        #endregion

        // 戦闘パラメータ：　戦闘で使うパラメータ
        // よく使うのは以下の19個？: HP, SP, 最大HP, 最大SP, 攻撃力, 魔法力, 守備力, 魔法防御, 命中率, 回避率,ガード率,行動ゲージ,最大行動ゲージ,行動回復量,テンション,最大テンション,攻撃速度,魔法速度,つば迫り合い率、
        // （適時追加するので、ここの値を信用せずに、ちゃんと全部取って。）
        #region サブ s**_*** ESPara・戦闘パラメータ、ＬＶや経験値など
        // 基本6パラメータ＋能力補正で調整可能な、戦闘などによく使う具体的なパラメータ

        // [TODO]sは番号がバラバラで、s01とs02はまだ未設定。保留。とりあえずちゃんと決まってから、番号を振り直す。

        /// <summary>
        /// 0になると気絶する、ヒットポイントです。（ただし、忍耐力が高いと根性で立ち上がったり、SPや感情・心霊・光闇パラメータが高いと覚醒したりする？）
        /// </summary>
        s03_HP,
        /// <summary>
        /// ダイス戦闘の場合、HPやパラメータを適時回復・増減するために消費する残りスピリットポイントです。また、技や魔法を使うことで増減し、最大値（+）や最小値（-）になると光や闇に還る、スピリットポイントです。（ただし、HPや感情・心霊・光闇パラメータが高いと覚醒したりする？）
        /// </summary>
        /// </summary>
        s03b_最大HP,
        /// <summary>
        /// 最大スピリットポイントです。（ただし、HPや感情・心霊・光闇パラメータが高いと覚醒したりする？）
        s04_SP,
        /// <summary>
        /// 最大スピリットポイントです。（ただし、忍耐力が高いと根性で立ち上がったり、SPや感情・心霊・光闇パラメータが高いと覚醒したりする？）
        /// </summary>
        s04b_最大SP,
        /// <summary>
        /// 斬撃・突撃・打撃など、物理的な攻撃の強さに影響する力です。
        /// </summary>
        s07_攻撃力,
        /// <summary>
        /// 黒魔撃・白魔撃・精神攻撃など、精神的な攻撃の強さに影響する力です。
        /// </summary>
        s08_魔法力,
        /// <summary>
        /// 斬撃・突撃・打撃など、物理的な攻撃を受けた時のダメージを軽減する、身の守りです。
        /// </summary>
        s09_守備力,
        /// <summary>
        /// 戦闘の行動の速さ、攻撃・防御・回避の速度（ダイス戦闘の場合は回避マス目）に影響する値です。通常100％として、どの程度の速さを持つか(％)です。
        /// </summary>
        s10_速度,
        /// <summary>
        /// 敵の攻撃を回避する確率（受けるダメージが0, 0.25, 1, 1.25倍のどれか）、敵の連続攻撃やつば競り合いから逃れる確率に影響する、回避率です。
        /// </summary>
        s11_回避率,
        /// <summary>
        /// 攻撃のクリティカルヒット率（クリーンヒットして大ダメージ＋敵を少しひるませる（ACTゲージ減少）。ただしクリティカルは、防御無視の会心／痛恨・奇跡／悲劇の一撃とは異なる）、クリティカル率です。
        /// </summary>
        s12_クリティカル率,
        /// <summary>
        /// 敵の回避を軽減する確率、敵の回避を失敗させやすくする確率です。
        /// </summary>
        s13_命中率,
        /// <summary>
        /// 敵の攻撃を軽減／無効化する確率（受けるダメージが0、1/2、1/4）、敵の連続攻撃を止める確率、つば競り合いで敵の攻撃を弾く確率に影響する、ガード率です。
        /// </summary>
        s13b_ガード率,
        /// <summary>
        /// HPが0になっても、オーバーダメージが一定以下の時に根性で復活する確率です。
        /// </summary>
        s14_根性発動率,
        /// <summary>
        /// 根性発動確率が0にならないオーバーダメージ、復活した時のHPの多さです。
        /// </summary>
        s14b_根性発動最大オーバーダメージ,
        /// <summary>
        /// SPを消費して、傷ついたHPを回復する確率です。
        /// </summary>
        s15_自然回復発動率,
        /// <summary>
        /// SPを消費して、傷ついたHPを回復する一回の回復量です。
        /// </summary>
        s15b_自然回復量,
        /// <summary>
        /// SPを消費して、相手の攻撃などに応じて、適切な行動を臨機応変に取る確率・その強さ（相手が攻撃の時、追加して防御を固められるなど）です。
        /// </summary>
        s16_対応率,
        /// <summary>
        /// トドメ・大ダメージを与える大事な一撃での命中率・クリティカル率・会心率が上がる確率です。
        /// </summary>
        s17_集中率,
        /// <summary>
        /// 相手の防御など（状況・状態）に応じて、適切な行動を臨機応変に取る確率・その強さ（相手が防御の時、攻撃せずに防御／力を溜めるを選ぶ確率が増えるなど）です。
        /// </summary>
        s18_戦術,


        // ●以下、戦闘用（今のところ、ダイス戦闘では使っていない？。詳しくはすべての参照を検索で調べてみて）
        /// <summary>
        /// 回避時（受けるダメージが0,1/4だった場合）や防御時（受けるダメージが0.5倍）のカウンター攻撃をする確率、敵の連続攻撃を止めてカウンター連続攻撃を繰り出す確率です。
        /// </summary>
        // ※反撃するかどうかはユーザがボタンを押すかどうか／CPUが残り行動ゲージから判断して決めるので、現在は反撃率は使ってない。　s14_反撃率,
        /// <summary>
        /// 連続攻撃したり、連続防御したりできる行動力(AP、ACTゲージ)の量です。
        /// </summary>
        s20_AP,
        /// <summary>
        /// 連続攻撃したり、連続防御したりできる行動力(AP、ACTゲージ)の最大値です。
        /// </summary>
        s20b_最大AP,
        /// <summary>
        /// 行動ゲージの1秒間（指定フレーム？）の回復量(ACT/s)です。
        /// </summary>
        s20c_AP回復量,
        /// <summary>
        /// （やる気）テンション。戦闘のやる気で、戦闘の流れを左右する自己の雰囲気。ダメージ・クリティカルの増減率、回避・つば競り合い・援護・根性の確率の増減率に影響します。
        /// </summary>
        s25_テンション,
        /// <summary>
        /// テンションの最大値です。ただし、ある条件ではテンションは最大テンションを超えることがあります。最大テンションを超えると、他者の能力を支配する（戦闘を牛耳る）「覚醒」となりやすいです。
        /// </summary>
        s25b_最大テンション,
        /// <summary>
        /// 魔法の速度を通常100％として、どの程度の速さを持つか(％)です。
        /// </summary>
        s20_魔法速度,
        /// <summary>
        /// 黒魔撃・白魔撃・精神攻撃など、精神的な攻撃を受けた時のダメージを軽減する、魔法防御力です。
        /// </summary>
        s10_魔法防御,
        /// <summary>
        /// 相手の攻撃を受け止めてつばぜり合いをする確率です。
        /// </summary>
        s21_つば迫り合い率,
        /// <summary>
        /// ヒットダメージの安定性です。
        /// </summary>
        s22_ダメージバランス,
        /// <summary>
        /// HPを適時回復する残り回復量です。
        /// </summary>
        s23_体力,






        #endregion
        // ※戦闘パラメータとは、基本的に戦闘でよく使われる具体的なパラメータ群

        #region サブ （無印）　その他の能力パラメータ
        /// <summary>
        /// クリティカル率、他の様々な中立ランダム要素に影響
        /// </summary>
        //運,
        /// <summary>
        /// （楽観性）笑いに影響、面白いイベントを頻繁に起こす、周囲をなごませる発言を頻繁にする
        /// </summary>
        ユーモア,
        /// <summary>
        /// 補助特技・連携をされやすい、敵が、戦闘のアピール度Up？（戦闘イベント発生率）、恋愛イベント？に影響
        /// </summary>
        魅力,
        /// <summary>
        /// （愛）、ドドメを刺す／刺される確率Down、、友好度Up？、信頼イベント？に影響
        /// </summary>
        優しさ,
        /// <summary>
        /// （体の柔らかさ-固さ）体調、良い感情の持続時間？、HPやSPの減り具合が、大きいと全快時に減りにくく／少ないとピンチ時に減りにくくなる、状態変化耐性（弱）に影響
        /// </summary>
        身体柔軟性,
        /// <summary>
        /// （発想力）、最大SPに影響、新しい技を覚える速度・確率？、に影響
        /// </summary>
        想像力,
        /// <summary>
        /// （感情移入度、）喜怒哀楽の感情の変化が激しい、感情による能力・行動補正増減Up、に影響
        /// </summary>
        情緒安定性,
        /// <summary>
        /// 最大テンション、潜在能力の解放、覚醒に影響
        /// </summary>
        自己覚醒度,

        /// <summary>
        /// 物理防御力、物理ガード率（受け身・反撃率）、その他攻撃受身系イベント発動率に影響
        /// </summary>
        護身術,
        /// <summary>
        /// 魔法防御力、魔法ガード率（吸収・反射率）に影響、その他魔法攻撃受身系イベント発動率に影響
        /// </summary>
        魔制術,

        //他案、候補：力, 身の守り, HP, SP, MP, AP, 素早さ, 回避, 命中, 運, 感情, 光影
        #endregion

        #region サブ （無印）　戦闘パラメータ補正値
        パラ自然回復補正増減,
        パラ自然回復補正乗除,


        // 戦闘ダメージ関連

        物理攻撃補正増減,
        物理攻撃補正乗除,
        物理威力,
        守備補正増減,
        守備補正乗除,
        物理ダメージ軽減率,

        物理攻撃強化率,// = 50,
        物理防御強化率,// = 25,

        ダメージバランス率,// = 20; // ダメージ安定率（％）
        ダメージ集中回数,//振りなおし回数 = 5.0;

        エスカレーション指数係数,// = 1.0; 



        #endregion

        #region ■■■ その他、「数値で表現した方が都合がよい」パラメータがあれば、以下に自由に追加してください。
        #endregion
        // ※ただし、プロフィール、属性、年齢、性別、出身地、身長、体重、血液型、生年月日、得意なもの、好き・嫌い
        // など、文字列で格納した方がわかりやすいものは、変数EVarに追加すること！
        戦闘不能ターン数,
        マヒターン数,
        金縛りターン数,

    }
    #region 以下、草案メモ。
    // （メモ）EParaのよく使う部分だけを抜粋・分割した。子列挙体。どちらを使うかややこしくなるので、今は使ってません。
    
    ///// <summary>
    ///// EParaの中の、一部のパラメータ群だけを扱いたいときに使う列挙体です。内容と順番はEParaのものと同じとしてください。
    ///// </summary>
    //public enum EIro18Para・色パラメータ
    //{
    //    #region ★★★色パラメータ
    //    c01_赤,
    //    c02_赤橙,
    //    c03_橙,
    //    c04_黄橙,
    //    c05_黄,
    //    c06_黄緑,
    //    c07_緑,
    //    c08_青緑,
    //    c09_青,
    //    c10_青紫,
    //    c11_紫,
    //    c12_赤紫,

    //    c13_白,
    //    c14_黒,
    //    c15_銀,
    //    c16_金,
    //    c17_透明,
    //    c18_虹色,

    //    //c01_赤,
    //    //c02_赤橙,
    //    //c03_橙,
    //    //c04_黄橙,
    //    //c05_黄,
    //    //c06_黄緑,
    //    //c07_緑,
    //    //c08_青緑,
    //    //c09_青,
    //    //c10_青紫,
    //    //c11_紫,
    //    //c12_赤紫,

    //    //c13_白,
    //    //c14_黒,
    //    //c15_銀,
    //    //c16_金,
    //    //c17_透明,
    //    //c18_虹色,
    //    #endregion
    //}
    //    /// <summary>
    ///// EParaの中の、一部のパラメータ群だけを扱いたいときに使う列挙体です。内容と順番はEParaのものと同じとしてください。
    ///// </summary>
    //public enum ESPara・戦闘パラメータ
    //{
    //    #region ■■■ ESPara・戦闘パラメータ
    //    // 基本6パラメータ＋能力補正で調整可能な、戦闘などによく使う具体的なパラメータ

    //    /// <summary>
    //    /// キャラクタの基本的な戦闘パラメータを決めるLVです。
    //    /// </summary>
    //    s01_LV,
    //    /// <summary>
    //    /// キャラクタのLVに関係する経験値です。
    //    /// </summary>
    //    s02_経験値,
    //    /// <summary>
    //    /// 0になると気絶する、ヒットポイントです。（ただし、忍耐力が高いと根性で立ち上がったり、SPや感情・心霊・光闇パラメータが高いと覚醒したりする？）
    //    /// </summary>
    //    s03_HP,
    //    /// <summary>
    //    /// 技や魔法を使うことで増減し、最大値（+）や最小値（-）になると光や闇に還る、スピリットポイントです。（ただし、HPや感情・心霊・光闇パラメータが高いと覚醒したりする？）
    //    /// </summary>
    //    s04_SP,
    //    /// <summary>
    //    /// 最大ヒットポイントです。（ただし、忍耐力が高いと根性で立ち上がったり、SPや感情・心霊・光闇パラメータが高いと覚醒したりする？）
    //    /// </summary>
    //    s05_最大HP,
    //    /// <summary>
    //    /// 最大スピリットポイントです。（ただし、HPや感情・心霊・光闇パラメータが高いと覚醒したりする？）
    //    /// </summary>
    //    s06_最大SP,
    //    /// <summary>
    //    /// 斬撃・突撃・打撃など、物理的な攻撃の強さに影響する力です。
    //    /// </summary>
    //    s07_攻撃力,
    //    /// <summary>
    //    /// 黒魔撃・白魔撃・精神攻撃など、精神的な攻撃の強さに影響する力です。
    //    /// </summary>
    //    s08_魔法力,
    //    /// <summary>
    //    /// 斬撃・突撃・打撃など、物理的な攻撃を受けた時のダメージを軽減する、身の守りです。
    //    /// </summary>
    //    s09_守備力,
    //    /// <summary>
    //    /// 黒魔撃・白魔撃・精神攻撃など、精神的な攻撃を受けた時のダメージを軽減する、魔法防御力です。
    //    /// </summary>
    //    s10_魔法防御,
    //    /// <summary>
    //    /// ヒットダメージの安定性です。
    //    /// </summary>
    //    //s11_ダメージバランス,
    //    /// <summary>
    //    /// 攻撃のクリティカルヒット率（クリーンヒットして大ダメージ＋敵を少しひるませる（ACTゲージ減少）。ただしクリティカルは、防御無視の奇跡・偶然・会心の一撃とは異なる）、クリティカル率です。
    //    /// </summary>
    //    s11_クリティカル率,
    //    /// <summary>
    //    /// 敵の攻撃を回避する確率（受けるダメージが0, 0.25, 1, 1.25倍のどれか）、敵の連続攻撃やつば競り合いから逃れる確率に影響する、回避率です。
    //    /// </summary>
    //    s12_回避率,
    //    /// <summary>
    //    /// 敵の攻撃を無効化する確率（受けるダメージが0）、敵の連続攻撃を止める確率、つば競り合いで敵の攻撃を弾く確率に影響する、ガード率です。
    //    /// </summary>
    //    /// <returns></returns>
    //    s13_ガード率,
    //    /// <summary>
    //    /// 攻撃・魔法を連続行動できる行動力(ACT)の量と、1秒間（指定フレーム？）の回復量(ACT/s)です。
    //    /// </summary>
    //    /// <summary>
    //    /// 回避時（受けるダメージが0,0 .25だった場合）や防御時（受けるダメージが0.5倍）のカウンター攻撃率、敵の連続攻撃からカウンター連続攻撃を繰り出す確率です。
    //    /// </summary>
    //    // ※反撃するかどうかはユーザがボタンを押すかどうか／CPUが残り行動ゲージから判断して決めるので、反撃率は要らない・・。　s14_反撃率,
    //    /// <summary>
    //    /// 連続攻撃したり、連続防御したりできる行動力(ACTゲージ)の量です。
    //    /// </summary>
    //    s14_行動ゲージ,
    //    /// <summary>
    //    /// 連続攻撃したり、連続防御したりできる行動力(ACTゲージ)の最大値です。
    //    /// </summary>
    //    s15_最大行動ゲージ,
    //    /// <summary>
    //    /// 行動ゲージの1秒間（指定フレーム？）の回復量(ACT/s)です。
    //    /// </summary>
    //    s16_行動回復量,
    //    /// <summary>
    //    /// （やる気）戦闘のやる気で、戦闘の流れを左右する自己の雰囲気。ダメージ・クリティカルの増減率、回避・つば競り合い・援護・根性の確率の増減率に影響する、テンションです。
    //    /// </summary>
    //    s17_テンション,
    //    /// <summary>
    //    /// テンションの最大値です。ただし、ある条件ではテンションは最大テンションを超えることがあります。最大テンションを超えると、他者の能力を支配する（戦闘を牛耳る）「覚醒」となりやすいです。
    //    /// </summary>
    //    s18_最大テンション,
    //    /// <summary>
    //    /// 攻撃・魔法の速度を通常100％として、どの程度の速さを持つか(％)です。
    //    /// </summary>
    //    s19_攻撃速度,
    //    /// <summary>
    //    /// 魔法の速度を通常100％として、どの程度の速さを持つか(％)です。
    //    /// </summary>
    //    s20_魔法速度,
    //    /// <summary>
    //    /// 相手の攻撃を受け止めてつばぜり合いをする確率です。
    //    /// </summary>
    //    s21_つば迫り合い率,

    //    #endregion
    //}
    #endregion

    /// <summary>
    /// 各キャラが持つdouble型で表現されるパラメータ（力，体力，攻撃力，基本6色パラ総合値など）の，全ての名前と値を管理するクラスです．
    /// 管理するパラメータ一覧は，列挙体EParaで指定します．
    /// 
    /// ここでは、特定のパラメータ群（色・身体・精神・戦闘パラメータ）だけは、一斉取得や個別の日本語メソッド化などをして簡単参照できるよう実装されています。
    /// ただし、パラメータ名や仕様変更時にメソッドの変更に手間がかかるので、日本語メソッドは本当によく使うパラメータだけ定義してください。
    /// 
    /// ※日本語クラスや日本語メソッドというものは、名前を省略する目的のためだけに作られる日本語名クラス・メソッドのため、特別な理由がない限り、クラスやメソッドの内容を変更しないことが望ましいです。
    /// </summary>
    /// <remarks>
    /// 「獲得経験値」「総敵討伐数」なども、このキャラ毎のパラメータとして考える。全キャラの値を出すときは、主人公のものを使うか、全キャラのものを足す。
    /// </remarks>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CParas・パラメータ一覧
    {

        #region ■■■パラメータ群の情報（※パラメータIDに変更がある場合は、ここを変更してください）
        public readonly static int s_iro色パラメータの個数 = 18;
        public readonly static int s_sin身体パラメータの個数 = 6;
        public readonly static int s_sei精神パラメータの個数 = 6;
        public readonly static int s_sen戦闘パラメータの個数 = 21;
        public static List<string> s_paraExplanパラメータの説明ファイル;
        #region パラメータの説明テキストの取得
        public static void readParaExplanText・パラメータの説明テキストを読み込み()
        {
            if (s_paraExplanパラメータの説明ファイル == null)
            {
                // パラメータ説明のファイル読み込み
                s_paraExplanパラメータの説明ファイル = MyTools.ReadFile_ToLists(Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス +
                    "パラメータの説明.txt");
            }
        }
        public static string getParaExplan・12色パラメータの説明テキストを取得()
        {
            readParaExplanText・パラメータの説明テキストを読み込み();
            String _説明テキスト = MyTools.getListValues_ToLines("", s_paraExplanパラメータの説明ファイル, true);
            return _説明テキスト;
        }
        public static string getParaExplan・各パラメータの説明テキストを取得(int _iroParaNo_赤1_紫6_赤紫_青紫12)
        {
            return getParaExplan・各パラメータの説明テキストを取得(EPara.c01_赤 + (_iroParaNo_赤1_紫6_赤紫_青紫12 - 1));
        }
        public static string getParaExplan・各パラメータの説明テキストを取得(EPara _パラメータID)
        {
            readParaExplanText・パラメータの説明テキストを読み込み();

            String _説明テキスト = "";
            int _line = 1;
            // 身体・精神パラメータの説明行の始まりを取得
            int _lineSin身体 = MyTools.getListLine(s_paraExplanパラメータの説明ファイル, "【赤】");
            int _lineSei精神 = MyTools.getListLine(s_paraExplanパラメータの説明ファイル, "【赤橙】");

            // 行の始まりから、指定色番号の説明を取得
            int _iroParaNo = ((int)_パラメータID - (int)EPara.c01_赤) + 1;
            if(_iroParaNo <= s_sin身体パラメータの個数){
                _line = _lineSin身体 + (_iroParaNo-1);
            }else{
                _line = _lineSei精神 + (_iroParaNo - 1 - s_sin身体パラメータの個数);
            }
            _説明テキスト = MyTools.getListValue(s_paraExplanパラメータの説明ファイル, _line);
            return _説明テキスト;
        }
        #endregion
        // 以下、パラメータ一括取得／セットに便利なget/setアクセサ
        // 各色パラメータの一括取得／セットメソッド===========================================

        #region 色パラメータの一括取得／セット：　getIroParas／setIroparas

        // 一括getアクセサ
        // ●double版
        /// <summary>
        /// 全ての色18パラメータ：　赤、赤橙、橙、黄橙、黄、黄緑、緑、青緑、青、青紫、紫、赤紫、　白、黒、金、銀、透明、虹色、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<double> getIro18色パラメータ()
        {
            List<double> _paras = new List<double>(new double[] { 
                getParaValue・パラの値取得(EPara.c01_赤), 
                getParaValue・パラの値取得(EPara.c02_橙), 
                getParaValue・パラの値取得(EPara.c03_黄), 
                getParaValue・パラの値取得(EPara.c04_緑), 
                getParaValue・パラの値取得(EPara.c05_青), 
                getParaValue・パラの値取得(EPara.c06_紫), 
                getParaValue・パラの値取得(EPara.c07_赤橙), 
                getParaValue・パラの値取得(EPara.c08_黄橙), 
                getParaValue・パラの値取得(EPara.c09_黄緑),
                getParaValue・パラの値取得(EPara.c10_青緑), 
                getParaValue・パラの値取得(EPara.c11_青紫), 
                getParaValue・パラの値取得(EPara.c12_赤紫),
                getParaValue・パラの値取得(EPara.c13_白), 
                getParaValue・パラの値取得(EPara.c14_黒), 
                getParaValue・パラの値取得(EPara.c15_銀), 
                getParaValue・パラの値取得(EPara.c16_金), 
                getParaValue・パラの値取得(EPara.c17_透明), 
                getParaValue・パラの値取得(EPara.c18_虹色)});
            return _paras;
        }
        /// <summary>
        /// 循環する色12パラメータ：　赤、赤橙、橙、黄橙、黄、黄緑、緑、青緑、青、青紫、紫、赤紫、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<double> getIro12色パラメータ()
        {
            List<double> _paras = new List<double>(new double[] { 
                getParaValue・パラの値取得(EPara.c01_赤), 
                getParaValue・パラの値取得(EPara.c02_橙), 
                getParaValue・パラの値取得(EPara.c03_黄), 
                getParaValue・パラの値取得(EPara.c04_緑), 
                getParaValue・パラの値取得(EPara.c05_青), 
                getParaValue・パラの値取得(EPara.c06_紫), 
                getParaValue・パラの値取得(EPara.c07_赤橙), 
                getParaValue・パラの値取得(EPara.c08_黄橙), 
                getParaValue・パラの値取得(EPara.c09_黄緑),
                getParaValue・パラの値取得(EPara.c10_青緑), 
                getParaValue・パラの値取得(EPara.c11_青紫), 
                getParaValue・パラの値取得(EPara.c12_赤紫),
                //getParaValue・パラの値取得(EPara.c01_赤), 
                //getParaValue・パラの値取得(EPara.c07_赤橙), 
                //getParaValue・パラの値取得(EPara.c02_橙), 
                //getParaValue・パラの値取得(EPara.c08_黄橙), 
                //getParaValue・パラの値取得(EPara.c03_黄), 
                //getParaValue・パラの値取得(EPara.c09_黄緑),
                //getParaValue・パラの値取得(EPara.c04_緑), 
                //getParaValue・パラの値取得(EPara.c10_青緑), 
                //getParaValue・パラの値取得(EPara.c05_青), 
                //getParaValue・パラの値取得(EPara.c11_青紫), 
                //getParaValue・パラの値取得(EPara.c06_紫), 
                //getParaValue・パラの値取得(EPara.c12_赤紫)
            });
            return _paras;
        }
        /// <summary>
        /// 基本色6パラメータ：　赤、橙、黄、緑、青、紫、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<double> getIro6基本６色パラメータ()
        {
            List<double> _paras = new List<double>(new double[] { 
                getParaValue・パラの値取得(EPara.c01_赤), 
                getParaValue・パラの値取得(EPara.c02_橙), 
                getParaValue・パラの値取得(EPara.c03_黄), 
                getParaValue・パラの値取得(EPara.c04_緑), 
                getParaValue・パラの値取得(EPara.c05_青), 
                getParaValue・パラの値取得(EPara.c06_紫)});
            return _paras;
        }
        /// <summary>
        /// 中間色6パラメータ：　赤橙、黄橙、黄緑、青緑、青紫、赤紫、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<double> getIroMID6中間６色パラメータ()
        {
            List<double> _paras = new List<double>(new double[] { 
                getParaValue・パラの値取得(EPara.c07_赤橙), 
                getParaValue・パラの値取得(EPara.c08_黄橙), 
                getParaValue・パラの値取得(EPara.c09_黄緑),
                getParaValue・パラの値取得(EPara.c10_青緑), 
                getParaValue・パラの値取得(EPara.c11_青紫), 
                getParaValue・パラの値取得(EPara.c12_赤紫)});
            return _paras;
        }
        /// <summary>
        /// 装飾色6パラメータ：　白、黒、金、銀、透明、虹色、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<double> getIroEXT6装飾６色パラメータ()
        {
            List<double> _paras = new List<double>(new double[] { 
                getParaValue・パラの値取得(EPara.c13_白), 
                getParaValue・パラの値取得(EPara.c14_黒), 
                getParaValue・パラの値取得(EPara.c15_銀), 
                getParaValue・パラの値取得(EPara.c16_金), 
                getParaValue・パラの値取得(EPara.c17_透明), 
                getParaValue・パラの値取得(EPara.c18_虹色)});
            return _paras;
        }


        // ●int版
        /// <summary>
        /// 全ての色18パラメータ：　赤、赤橙、橙、黄橙、黄、黄緑、緑、青緑、青、青紫、紫、赤紫、　白、黒、金、銀、透明、虹色、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<int> getIroParas色18パラメータint()
        {
            List<int> _paras = new List<int>(new int[] { 
                (int)getParaValue・パラの値取得(EPara.c01_赤), 
                (int)getParaValue・パラの値取得(EPara.c02_橙), 
                (int)getParaValue・パラの値取得(EPara.c03_黄), 
                (int)getParaValue・パラの値取得(EPara.c04_緑), 
                (int)getParaValue・パラの値取得(EPara.c05_青), 
                (int)getParaValue・パラの値取得(EPara.c06_紫), 
                (int)getParaValue・パラの値取得(EPara.c07_赤橙), 
                (int)getParaValue・パラの値取得(EPara.c08_黄橙), 
                (int)getParaValue・パラの値取得(EPara.c09_黄緑),
                (int)getParaValue・パラの値取得(EPara.c10_青緑), 
                (int)getParaValue・パラの値取得(EPara.c11_青紫), 
                (int)getParaValue・パラの値取得(EPara.c12_赤紫),
                (int)getParaValue・パラの値取得(EPara.c13_白), 
                (int)getParaValue・パラの値取得(EPara.c14_黒), 
                (int)getParaValue・パラの値取得(EPara.c15_銀), 
                (int)getParaValue・パラの値取得(EPara.c16_金), 
                (int)getParaValue・パラの値取得(EPara.c17_透明), 
                (int)getParaValue・パラの値取得(EPara.c18_虹色)});
            return _paras;
        }
        /// <summary>
        /// 循環する色12パラメータ：　赤、赤橙、橙、黄橙、黄、黄緑、緑、青緑、青、青紫、紫、赤紫、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<int> getIroParas色12パラメータint()
        {
            List<int> _paras = new List<int>(new int[] { 
                (int)getParaValue・パラの値取得(EPara.c01_赤), 
                (int)getParaValue・パラの値取得(EPara.c02_橙), 
                (int)getParaValue・パラの値取得(EPara.c03_黄), 
                (int)getParaValue・パラの値取得(EPara.c04_緑), 
                (int)getParaValue・パラの値取得(EPara.c05_青), 
                (int)getParaValue・パラの値取得(EPara.c06_紫), 
                (int)getParaValue・パラの値取得(EPara.c07_赤橙), 
                (int)getParaValue・パラの値取得(EPara.c08_黄橙), 
                (int)getParaValue・パラの値取得(EPara.c09_黄緑),
                (int)getParaValue・パラの値取得(EPara.c10_青緑), 
                (int)getParaValue・パラの値取得(EPara.c11_青紫), 
                (int)getParaValue・パラの値取得(EPara.c12_赤紫),
            });
            return _paras;
        }
        /// <summary>
        /// 基本色6パラメータ：　赤、橙、黄、緑、青、紫、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<int> getIroParas基本色6パラメータint()
        {
            List<int> _paras = new List<int>(new int[] { 
                (int)getParaValue・パラの値取得(EPara.c01_赤), 
                (int)getParaValue・パラの値取得(EPara.c02_橙), 
                (int)getParaValue・パラの値取得(EPara.c03_黄), 
                (int)getParaValue・パラの値取得(EPara.c04_緑), 
                (int)getParaValue・パラの値取得(EPara.c05_青), 
                (int)getParaValue・パラの値取得(EPara.c06_紫)});
            return _paras;
        }
        /// <summary>
        /// 中間色6パラメータ：　赤橙、黄橙、黄緑、青緑、青紫、赤紫、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<int> getIroParas中間色6パラメータint()
        {
            List<int> _paras = new List<int>(new int[] { 
                (int)getParaValue・パラの値取得(EPara.c07_赤橙), 
                (int)getParaValue・パラの値取得(EPara.c08_黄橙), 
                (int)getParaValue・パラの値取得(EPara.c09_黄緑),
                (int)getParaValue・パラの値取得(EPara.c10_青緑), 
                (int)getParaValue・パラの値取得(EPara.c11_青紫), 
                (int)getParaValue・パラの値取得(EPara.c12_赤紫)});
            return _paras;
        }
        /// <summary>
        /// 装飾色6パラメータ：　白、黒、金、銀、透明、虹色、　を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<int> getIroParas装飾色6パラメータint()
        {
            List<int> _paras = new List<int>(new int[] { 
                (int)getParaValue・パラの値取得(EPara.c13_白), 
                (int)getParaValue・パラの値取得(EPara.c14_黒), 
                (int)getParaValue・パラの値取得(EPara.c15_銀), 
                (int)getParaValue・パラの値取得(EPara.c16_金), 
                (int)getParaValue・パラの値取得(EPara.c17_透明), 
                (int)getParaValue・パラの値取得(EPara.c18_虹色)});
            return _paras;
        }


        // setアクセサ
        // ●double版のみ
        /// <summary>
        /// 色パラメータを変更します。18色の時は18配列、12色の時は12配列を用意してください。6配列は複数考えられるためエラーになります。
        /// </summary>
        /// <param name="_iro6・基本色６パラ"></param>
        public void setIroParas色パラメータを変更(List<double> _iroParas・変更前の色パラメータリスト, ESet _変更方法, double _代入値or増減値or倍率)
        {
            double _para = 0.0;
            if (_変更方法 == ESet._default・代入値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] = _代入値or増減値or倍率;
                }
                setIroParas色パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] += _代入値or増減値or倍率;
                }
                setIroParas色パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] *= _代入値or増減値or倍率;
                }
                setIroParas色パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
        }
        /// <summary>
        /// 基本色6パラメータを変更します。
        /// </summary>
        public void setIro6基本６色パラメータを変更(List<double> _iroParas・変更前の色パラメータリスト, ESet _変更方法, double _代入値or増減値or倍率)
        {
            double _para = 0.0;
            if (_変更方法 == ESet._default・代入値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] = _代入値or増減値or倍率;
                }
                setIroParas基本色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] += _代入値or増減値or倍率;
                }
                setIroParas基本色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] *= _代入値or増減値or倍率;
                }
                setIroParas基本色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
        }
        /// <summary>
        /// 中間色6パラメータを変更します。
        /// </summary>
        public void setIroMID6中間６色パラメータを変更(List<double> _iroParas・変更前の色パラメータリスト, ESet _変更方法, double _代入値or増減値or倍率)
        {
            double _para = 0.0;
            if (_変更方法 == ESet._default・代入値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] = _代入値or増減値or倍率;
                }
                setIroParas中間色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] += _代入値or増減値or倍率;
                }
                setIroParas中間色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] *= _代入値or増減値or倍率;
                }
                setIroParas中間色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
        }
        /// <summary>
        /// 装飾色6パラメータを変更します。
        /// </summary>
        public void setIroEXT6装飾６色パラメータを変更(List<double> _iroParas・変更前の色パラメータリスト, ESet _変更方法, double _代入値or増減値or倍率)
        {
            double _para = 0.0;
            if (_変更方法 == ESet._default・代入値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] = _代入値or増減値or倍率;
                }
                setIroParas装飾色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] += _代入値or増減値or倍率;
                }
                setIroParas装飾色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] *= _代入値or増減値or倍率;
                }
                setIroParas装飾色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
        }
        /// <summary>
        /// 色パラメータを変更します。18色の時は18配列、12色の時は12配列を用意してください。6配列は複数考えられるためエラーになります。
        /// </summary>
        /// <param name="_iro6・基本色６パラ"></param>
        public void setIroParas色パラメータを変更(List<double> _iroParas・変更前の色パラメータリスト, ESet _変更方法, List<double> _代入値or増減値or倍率)
        {
            double _para = 0.0;
            if (_変更方法 == ESet._default・代入値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] = _代入値or増減値or倍率[i];
                }
                setIroParas色パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] += _代入値or増減値or倍率[i];
                }
                setIroParas色パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] *= _代入値or増減値or倍率[i];
                }
                setIroParas色パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
        }
        /// <summary>
        /// 基本色6パラメータを変更します。
        /// </summary>
        public void setIro6基本６色パラメータを変更(List<double> _iroParas・変更前の色パラメータリスト, ESet _変更方法, List<double> _代入値or増減値or倍率)
        {
            double _para = 0.0;
            if (_変更方法 == ESet._default・代入値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] = _代入値or増減値or倍率[i];
                }
                setIroParas基本色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] += _代入値or増減値or倍率[i];
                }
                setIroParas基本色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] *= _代入値or増減値or倍率[i];
                }
                setIroParas基本色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
        }
        /// <summary>
        /// 中間色6パラメータを変更します。
        /// </summary>
        public void setIroMID6中間６色パラメータを変更(List<double> _iroParas・変更前の色パラメータリスト, ESet _変更方法, List<double> _代入値or増減値or倍率)
        {
            double _para = 0.0;
            if (_変更方法 == ESet._default・代入値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] = _代入値or増減値or倍率[i];
                }
                setIroParas中間色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] += _代入値or増減値or倍率[i];
                }
                setIroParas中間色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] *= _代入値or増減値or倍率[i];
                }
                setIroParas中間色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
        }
        /// <summary>
        /// 装飾色6パラメータを変更します。
        /// </summary>
        public void setIroEXT6装飾６色パラメータを変更(List<double> _iroParas・変更前の色パラメータリスト, ESet _変更方法, List<double> _代入値or増減値or倍率)
        {
            double _para = 0.0;
            if (_変更方法 == ESet._default・代入値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] = _代入値or増減値or倍率[i];
                }
                setIroParas装飾色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] += _代入値or増減値or倍率[i];
                }
                setIroParas装飾色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                for (int i = 0; i <= _iroParas・変更前の色パラメータリスト.Count - 1; i++)
                {
                    _iroParas・変更前の色パラメータリスト[i] *= _代入値or増減値or倍率[i];
                }
                setIroParas装飾色6パラメータを代入(_iroParas・変更前の色パラメータリスト);
            }
        }
        
        /// <summary>
        /// 色パラメータを代入します。18色の時は18配列、12色の時は12配列を用意してください。6配列は複数考えられるためエラーになります。
        /// </summary>
        public void setIroParas色パラメータを代入(List<double> _iroParas・変更前の色パラメータリスト)
        {
            if (_iroParas・変更前の色パラメータリスト.Count < 12)
            {
                Program・実行ファイル管理者.error("色パラメータの代入個数が足りないため、セットされませんでした。: " + _iroParas・変更前の色パラメータリスト.Count + " < " + 12 + "or" + 18);
            }
            else
            {
                int _i = 0;
                setPara・パラを変更(EPara.c01_赤, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c02_橙, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c03_黄, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c04_緑, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c05_青, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c06_紫, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c07_赤橙, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c08_黄橙, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c09_黄緑, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c10_青緑, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c11_青紫, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c12_赤紫, _iroParas・変更前の色パラメータリスト[_i++]);

                if (_iroParas・変更前の色パラメータリスト.Count >= s_iro色パラメータの個数)
                {
                    setPara・パラを変更(EPara.c13_白, _iroParas・変更前の色パラメータリスト[_i++]);
                    setPara・パラを変更(EPara.c14_黒, _iroParas・変更前の色パラメータリスト[_i++]);
                    setPara・パラを変更(EPara.c15_銀, _iroParas・変更前の色パラメータリスト[_i++]);
                    setPara・パラを変更(EPara.c16_金, _iroParas・変更前の色パラメータリスト[_i++]);
                    setPara・パラを変更(EPara.c17_透明, _iroParas・変更前の色パラメータリスト[_i++]);
                    setPara・パラを変更(EPara.c18_虹色, _iroParas・変更前の色パラメータリスト[_i++]);
                }
            }
        }
        /// <summary>
        /// 基本色6パラメータを代入します。
        /// </summary>
        public void setIroParas基本色6パラメータを代入(List<double> _iroParas・変更前の色パラメータリスト)
        {
            if (_iroParas・変更前の色パラメータリスト.Count < 6)
            {
                Program・実行ファイル管理者.error("基本色パラメータの代入個数が足りないため、セットされませんでした。: " + _iroParas・変更前の色パラメータリスト.Count + " < " + 6);
            }
            else
            {
                int _i = 0;
                setPara・パラを変更(EPara.c01_赤, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c02_橙, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c03_黄, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c04_緑, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c05_青, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c06_紫, _iroParas・変更前の色パラメータリスト[_i++]);
            }
        }
        /// <summary>
        /// 中間色6パラメータを代入します。
        /// </summary>
        public void setIroParas中間色6パラメータを代入(List<double> _iroParas・変更前の色パラメータリスト)
        {
            if (_iroParas・変更前の色パラメータリスト.Count < 6)
            {
                Program・実行ファイル管理者.error("中間色パラメータの代入個数が足りないため、セットされませんでした。: " + _iroParas・変更前の色パラメータリスト.Count + " < " + 6);
            }
            else
            {
                int _i = 0;
                setPara・パラを変更(EPara.c07_赤橙, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c08_黄橙, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c09_黄緑, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c10_青緑, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c11_青紫, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c12_赤紫, _iroParas・変更前の色パラメータリスト[_i++]);
            }
        }
        /// <summary>
        /// 装飾色6パラメータを代入します。
        /// </summary>
        public void setIroParas装飾色6パラメータを代入(List<double> _iroParas・変更前の色パラメータリスト)
        {
            if (_iroParas・変更前の色パラメータリスト.Count < 6)
            {
                Program・実行ファイル管理者.error("装飾色パラメータの代入個数が足りないため、セットされませんでした。: " + _iroParas・変更前の色パラメータリスト.Count + " < " + 6);
            }
            else
            {
                int _i = 0;
                setPara・パラを変更(EPara.c13_白, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c14_黒, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c15_銀, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c16_金, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c17_透明, _iroParas・変更前の色パラメータリスト[_i++]);
                setPara・パラを変更(EPara.c18_虹色, _iroParas・変更前の色パラメータリスト[_i++]);
            }
        }
        #endregion

        #region 身体パラメータの一括取得／セット：　getShintaiParas/setShintaiParas
        /// <summary>
        /// 身体6パラメータ：　ちから、持久力、行動力、素早さ、精神力、賢さ、を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<double> getShintaiParas・身体パラメータ()
        {
            List<double> _ShintaiPara = new List<double>(new double[] { 
                getParaValue・パラの値取得(EPara.a1_ちから), 
                getParaValue・パラの値取得(EPara.a2_持久力), 
                getParaValue・パラの値取得(EPara.a3_行動力), 
                getParaValue・パラの値取得(EPara.a4_素早さ), 
                getParaValue・パラの値取得(EPara.a5_精神力),
                getParaValue・パラの値取得(EPara.a6_賢さ) });
            return _ShintaiPara;
        }
        /// <summary>
        /// 身体パラメータを代入します。
        /// </summary>
        /// <param name="_iro6・基本色６パラ"></param>
        public void setShintaiParas・身体パラメータをセット(List<double> _phisicalParas)
        {
            if (_phisicalParas.Count < s_sin身体パラメータの個数)
            {
                Program・実行ファイル管理者.error("身体パラメータの代入個数が足りないため、セットされませんでした。: " + _phisicalParas.Count + " < " + s_sin身体パラメータの個数);
            }
            else
            {
                int _i = 0;
                setPara・パラを変更(EPara.a1_ちから, _phisicalParas[_i++]);
                setPara・パラを変更(EPara.a2_持久力, _phisicalParas[_i++]);
                setPara・パラを変更(EPara.a3_行動力, _phisicalParas[_i++]);
                setPara・パラを変更(EPara.a4_素早さ, _phisicalParas[_i++]);
                setPara・パラを変更(EPara.a5_精神力, _phisicalParas[_i++]);
                setPara・パラを変更(EPara.a6_賢さ, _phisicalParas[_i++]);
            }
        }
        #endregion

        #region 精神パラメータの一括取得／セット：　getSeishinParas/setSeishinParas
        /// <summary>
        /// 精神パラメータ：　器用さ、忍耐力、健康力、適応力、集中力、思考力、を配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<double> getSeishinParas・精神パラメータ()
        {
            List<double> _SeishinPara = new List<double>(new double[] { 
                getParaValue・パラの値取得(EPara.b1_器用さ), 
                getParaValue・パラの値取得(EPara.b2_忍耐力), 
                getParaValue・パラの値取得(EPara.b3_健康力),
                getParaValue・パラの値取得(EPara.b4_適応力), 
                getParaValue・パラの値取得(EPara.b5_集中力), 
                getParaValue・パラの値取得(EPara.b6_思考力) });
            return _SeishinPara;
        }
        /// <summary>
        /// 精神パラメータを代入します。
        /// </summary>
        /// <param name="_iro6・基本色６パラ"></param>
        public void setSeishinParas・精神パラメータをセット(List<double> _mindParas)
        {
            if (_mindParas.Count < s_sei精神パラメータの個数)
            {
                Program・実行ファイル管理者.error("精神パラメータの代入個数が足りないため、セットされませんでした。: " + _mindParas.Count + " < " + s_sei精神パラメータの個数);
            }
            else
            {
                int _i = 0;
                setPara・パラを変更(EPara.b1_器用さ, _mindParas[_i++]);
                setPara・パラを変更(EPara.b2_忍耐力, _mindParas[_i++]);
                setPara・パラを変更(EPara.b3_健康力, _mindParas[_i++]);
                setPara・パラを変更(EPara.b4_適応力, _mindParas[_i++]);
                setPara・パラを変更(EPara.b5_集中力, _mindParas[_i++]);
                setPara・パラを変更(EPara.b6_思考力, _mindParas[_i++]);
            }
        }
        #endregion

        #region 戦闘パラメータの一括取得／セット: getSentouParas／setSentouParas
        /*
        /// <summary>
        /// 戦闘パラメータを配列で取得します。
        /// </summary>
        /// <returns></returns>
        public List<double> getSentouParas・戦闘パラメータ()
        {
            List<double> _BattlePara = new List<double>(new double[] {
                getParaValue・パラの値取得(EPara.LV), 
                getParaValue・パラの値取得(EPara.LVExp_経験値), 
                getParaValue・パラの値取得(EPara.s03_HP), 
                getParaValue・パラの値取得(EPara.s04_SP), 
                getParaValue・パラの値取得(EPara.s03b_最大HP), 
                getParaValue・パラの値取得(EPara.s04b_最大SP), 
                getParaValue・パラの値取得(EPara.s07_攻撃力), 
                getParaValue・パラの値取得(EPara.s08_魔法力), 
                getParaValue・パラの値取得(EPara.s09_守備力), 
                getParaValue・パラの値取得(EPara.s10_魔法防御), 
                getParaValue・パラの値取得(EPara.s12_クリティカル率), 
                getParaValue・パラの値取得(EPara.s11_回避率), 
                getParaValue・パラの値取得(EPara.s13b_ガード率), 
                //getParaValue・パラの値取得(EPara.s14_反撃率), 
                getParaValue・パラの値取得(EPara.s20_AP), 
                getParaValue・パラの値取得(EPara.s20b_最大AP), 
                getParaValue・パラの値取得(EPara.s16_行動回復量), 
                getParaValue・パラの値取得(EPara.s25_テンション), 
                getParaValue・パラの値取得(EPara.s25b_最大テンション), 
                getParaValue・パラの値取得(EPara.s10_速度), 
                getParaValue・パラの値取得(EPara.s20_魔法速度), 
                getParaValue・パラの値取得(EPara.s21_つば迫り合い率)
            });
            return _BattlePara;
        }
        /// <summary>
        /// 戦闘パラメータを代入します。
        /// </summary>
        /// <param name="_iro6・基本色６パラ"></param>
        public void setSentouParas・戦闘パラメータをセット(List<double> _BattleParas)
        {
            if (_BattleParas.Count < s_sen戦闘パラメータの個数)
            {
                Program.error("戦闘パラメータの代入個数が足りないため、セットされませんでした。: " + _BattleParas.Count + " < " + s_sen戦闘パラメータの個数);
            }
            else
            {
                int _birthNum = 0;
                setPara・パラを変更(EPara.LV, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.LVExp_経験値, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s03_HP, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s04_SP, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s03b_最大HP, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s04b_最大SP, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s07_攻撃力, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s08_魔法力, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s09_守備力, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s10_魔法防御, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s12_クリティカル率, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s11_回避率, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s13b_ガード率, _BattleParas[_birthNum++]);
                //setParaValue(EPara.s14_反撃率, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s20_AP, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s20b_最大AP, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s16_行動回復量, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s25_テンション, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s25b_最大テンション, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s10_速度, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s20_魔法速度, _BattleParas[_birthNum++]);
                setPara・パラを変更(EPara.s21_つば迫り合い率, _BattleParas[_birthNum++]);
            }
        }
         * */
        #endregion
        
        // 各色パラメータの一括取得／セットメソッド===========================================

        #endregion

        /// <summary>
        /// 力、攻撃力、体力、感情値などのIDで検索可能な、パラメータ名と値。
        /// </summary>
        List<CPara・パラメータ> p_parameter・パラメータ一覧 = new List<CPara・パラメータ>();

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public CParas・パラメータ一覧()
        {
            List<int> _enumArray = MyTools.getEnumValueList<EPara>();
            // 全てのパラメータをリスト追加して初期化
            foreach (int _oneEnum in _enumArray)
            {
                p_parameter・パラメータ一覧.Add(new CPara・パラメータ());
            }
        }
        #region get/setアクセサ
        /// <summary>
        /// ※非推奨です。できればキャラクラスのsetParaを使ってください。指定のIDのパラメータを代入します。
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public void setPara・パラを変更(EPara _parameterID・パラメータID, double _newValue)
        {
            //double _oldValue = MyTools.getListValue(p_parameters・パラメータ一覧, (int)_parameterID・パラメータID);
            //if (_oldValue != 0.0)

            if ((int)_parameterID・パラメータID <= p_parameter・パラメータ一覧.Count - 1)// enumは0から
            {
                // そのパラメータが定義されていれば（サイズが確保されていれば）、代入
                // パラメータの値の代入！
                p_parameter・パラメータ一覧[(int)_parameterID・パラメータID].set(_newValue);
            }
        }

        /// <summary>
        /// ※非推奨です。できればキャラクラスのparaを使ってください。指定のIDのパラメータを取得します。
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public CPara・パラメータ getPara・パラ(EPara _parameterID)
        {
            return MyTools.getListValue(p_parameter・パラメータ一覧, (int)_parameterID);
        }
        /// <summary>
        /// 指定のIDのパラメータを取得します。リストに格納されていないパラメータでも、デフォルト値（初期値）が定義されているパラメータは、デフォルト値を取得します。デフォルト値が未定義のパラメータは、0を取得します。
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public double getParaValue・パラの値取得(EPara _parameterID)
        {
            int _paraID = (int)_parameterID;
            double _paraValue = MyTools.getListValue<CPara・パラメータ>(p_parameter・パラメータ一覧, _paraID).get();
            // 定義されていなかったら、デフォルト値の代入
            if (_paraValue == 0)
            {
                _paraValue = CParasDefault・パラ初期値群.get(_parameterID);
            }
            return _paraValue;
        }
        #endregion

        /*/// <summary>
        /// ※このメソッドはパラメータは使っても良い？
        /// 現段階は、Calc・計算はprivateで外部からはアクセス不可
        /// </summary>
        private void calcBattlePara・戦闘パラメータを計算()
        {
            CBattleParaCalulator・戦闘パラメータ計算機.Calc・計算(p_parameters・パラメータ一覧);
        }*/

    }

}

