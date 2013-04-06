using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /* 以下、古い変数値のメモ
     * 最新版はCVars・変数群に定義
/// <summary>
/// 特定の特徴を名前で参照するenumです．これらの特徴を取得する際はIDの数値を直接入れずに，日本語メソッド（名前()など）を使用してください．ただし，ここに宣言されていない変数名も多く存在します（一時的な特徴など）．
/// </summary>
public enum EVar
{
    名前,
    本名,
    ふりがな,
    称号,
    性別,

    状態変化,
    体調,
    //調子,
    感情,
    今の気分,
    登場セリフ,
    通り名,

    作戦,
    このターンの作戦,
    このターンの攻撃対象キャラ,
    このターンの攻撃対象キャラ名,

    メイン武器,
    通り名,
    キー台詞_幼少期,

    生年月日,
    血液型,
    年齢,
    身長,
    体重,
    個性を司る象徴語,
    特に強い光の次元要素,
    特に強い闇の次元要素,
    光と闇の転換プロセス,

    服のサイズ,
    バスト,
    ウエスト,
    ヒップ,
    靴のサイズ,

    趣味,
    好きなもの,
    嫌いなもの,

    恋人の存在,
}

/// <summary>
/// 変数値（文字列）によく使われるテンプレートを集めたクラスです．ただし，あくまで値の打ち間違いを防ぐためのテンプレートであり，変数名によってはこの中に存在しない値が多く存在します．なお，実際に潜在パラメータとして表示される文字列は，変数名ではなく，stringの値です．また，変数値の比較の際は，実際の値をint型に変換したりして比較します．
/// </summary>
public class CVarValue・変数値
{
    public static readonly string boolFalse_無 = "boolFalse_無";
    public static readonly string boolTrue_有 = "boolTrue_有";

    public static readonly string size_SS = "SS";
    public static readonly string size_S = "S";
    public static readonly string size_M = "M";
    public static readonly string size_L = "L";
    public static readonly string size_LL = "LL";
    public static readonly string size_XL = "XL";

    public static readonly string LV_S = "10";
    public static readonly string LV_AA = "9";
    public static readonly string LV_Aplus = "8";
    public static readonly string LV_A = "7";
    public static readonly string LV_B = "6";
    public static readonly string LV_C = "5";
    public static readonly string LV_D = "4";
    public static readonly string LV_E = "3";
    public static readonly string LV_F = "2";
    public static readonly string LV_G = "1";
    public static readonly string LV_SS = "11";
    public static readonly string LV_SSS = "12";
    public static readonly string LV_Z = "15";

    public static readonly string blood_AA = "AA";
    public static readonly string blood_BB = "BB";
    public static readonly string blood_O = "O";
    public static readonly string blood_AO = "AO";
    public static readonly string blood_BO = "BO";
    public static readonly string blood_AB = "AB";
    public static readonly string blood_Unknown = "unknown";
    public static readonly string blood_No = "No";

    public static readonly string sakusen01_めいれいさせてね = "めいれいさせてね";
    public static readonly string sakusen02_アイツを狙え = "アイツを狙え";
    public static readonly string sakusen03_ぼちぼちがんばって = "ぼちぼちがんばって";
    public static readonly string sakusen04_全力で頼むよ = "全力で頼むよ";
    public static readonly string sakusen05_自分のペースで = "自分のペースで";
    public static readonly string sakusen06_無理しないで = "無理しないで";
    public static readonly string sakusen07_みんなに合わせて = "みんなに合わせて";
    public static readonly string sakusen08_みんなを守って = "みんなを守って";
    public static readonly string sakusen09_楽しく遊ぼう = "楽しく遊ぼう";
    public static readonly string sakusen10_お好きにどうぞ = "お好きにどうぞ";
    public static readonly string sakusen21_絶対服従 = "絶対服従";
    public static readonly string sakusen22_死んでもいいよ = "死んでもいいよ";
    public static readonly string sakusen23_敵を殺しなさい = "敵を殺しなさい";
    public static readonly string sakusen24_皆殺しだ = "皆殺しだ";
    public static readonly string sakusen25_自分を磨け = "自分を磨け";
    public static readonly string sakusen26_魂を捧げよ = "魂を捧げよ";



    //public static readonly string 調子_絶好調 = "絶好調";


}
 * */




    ///// <summary>
    ///// 名前，称号，感情状態など，string型で表現される変数名と値を格納したクラス．
    ///// ※名前を省略する目的のためだけに作られる日本語名クラスのため，特別な理由がない限り，新しいプロパティ・メソッドを追加しないことが望ましいです．
    ///// </summary>
    //public class 変数 : CVar・変数{

    //}

    /// <summary>
    /// （※基本的に、CVars・変数一覧クラスが使います。詳しくはそちらを参照してください。）
    /// 様々な型の変数を一括して管理する、変数クラスです。
    /// 
    /// 　　　【使い方】：特定のクラスにCVar・変数クラスのインスタンスを持たせることで、
    /// 様々な型の変数を一括管理することが出来ます。
    /// 例えば、キャラにDictionaly＜string, CVar・変数＞ p_vars; を作ることで、キャラの名前，各種画像など，値がstring型（必要であればbool型やobject型も可能）で表現される変数名と値を一括して扱うことができます．
    /// （ただし、型を意識しない変数管理は、デバッグが煩雑になり、かつ処理速度も落ちるので、
    /// 　頻繁に使用する変数はちゃんと型を付けて管理した方が無難です。）
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CVar・変数
    {
        /// <summary>
        /// この特徴のメインの値を示す，文字列です．値型も文字列で格納します．代表的な値はEVarValue列挙体に定義されています．
        /// </summary>
        public string p_VarValue・変数値 = s_none・変数値未定義;
        /// <summary>
        /// この特徴が参照もしくは所有するobject型（どんなクラスでも格納可）のオブジェクトです．
        /// 文字列型以外の変数を格納する時に使います．
        /// （※注意）適切にキャストして使わないと，エラーになる恐れがあります．
        /// </summary>
        public object p_VarObject・変数オブジェクト = null;

        /// <summary>
        /// 変数値が定義されていないときに返す，特徴例のデフォルト値です．
        /// </summary>
        public static readonly string s_none・変数値未定義 = CVarValue・変数値._none_未定義;

        /// <summary>
        /// コンストラクタです．変数値""で、オブジェクトはnullの変数を作成します．
        /// </summary>
        public CVar・変数()
         : this(s_none・変数値未定義, null)
        {
        }
        /// <summary>
        /// コンストラクタです．変数値，オブジェクトは省略可能です．
        /// </summary>
        public CVar・変数　(string _value・変数値, object _object・所有もしくは参照オブジェクト)
        {
            p_VarValue・変数値 = _value・変数値;
            p_VarObject・変数オブジェクト = _object・所有もしくは参照オブジェクト;
        }
        /// <summary>
        /// コンストラクタです．変数値が引数で、オブジェクトはnullの変数を作成します．
        /// </summary>
        public CVar・変数(string _変数値)
            : this(_変数値, null) 
        {
        }


        #region get/setアクセサ
        
        /// <summary>
        /// 変数値を変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string set(string _varValue)
        {
            string _beforeFea = s_none・変数値未定義;
            _beforeFea = p_VarValue・変数値;

            // 変数値を変更
            p_VarValue・変数値 = _varValue;
            
            return _beforeFea;
        }
        /// <summary>
        /// 特徴クラスに新しいオブジェクトを設定します．返り値は，上書きされる前の以前設定されていた値（無い場合はnull）です．
        /// </summary>
        public object setVar・変数クラスを設定_クラス型<T>(string _varValue, T _varObject)
            where T : class // Tは参照型
        {
            object _beforeObject = p_VarObject・変数オブジェクト;
            set(_varValue);
            // setObject(_varObject);
            object _newObject = new object();
            p_VarObject・変数オブジェクト = _newObject;

            return _beforeObject;
        }
        /// <summary>
        /// 特徴クラスに新しいオブジェクトを設定します．返り値は，上書きされる前の以前設定されていた値（無い場合はnull）です．
        /// </summary>
        public object setVar・変数クラスを設定_値型<T>(string _varValue, T _varObject)
            where T : struct // Tは値型
        {
            object _beforeObject = p_VarObject・変数オブジェクト;
            set(_varValue);
            object _newObject = _varObject;
            p_VarObject・変数オブジェクト = _newObject;

            return _beforeObject;
        }

        /// <summary>
        /// この特徴の変数値を取得します．
        /// </summary>
        /// <param name="_変数名"></param>
        public string get()
        {
            return p_VarValue・変数値;
        }
        /// <summary>
        /// この特徴の変数値を取得します．（※注意）何も無い場合はnullが返るので，必ずnullだった場合の処理をしてください．
        /// </summary>
        public object getObject()
        {
            return p_VarObject・変数オブジェクト;
        }
        #endregion
    }

 

}
