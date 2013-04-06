using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /// <summary>
    /// 特定の変数を列挙型で参照するEVarです．
    /// CVars・変数一覧のインスタンスを持つクラス（CChara・キャラクラスなど）から、
    /// ●「クラス.Var(EVar.変数名)」
    /// を使うか，日本語メソッド（name名前()など）を使用してください．
    /// 
    /// 　なお，CVars・変数一覧に格納されている変数には、
    /// 　EVarに宣言されていない変数も格納できます（デバッグなどで扱う一時的な変数など）．
    /// それを参照するには、直接static変数
    /// ●「Vars・変数一覧.s_***」
    /// を参照してください。
    /// 
    /// 　　※CParas・パラメータ一覧から特定のパラメータを参照する列挙体EParaと異なる点は、
    ///     　　・string型やobject型を格納可能な点
    ///     　　・適時setVar()メソッドを使うことで、新しい変数を追加／削除可能な点
    /// です。
    /// 従って、スイッチ（ON・OFF）やパラメータなどのdouble型で表現可能な変数は、メモリを使ってください。
    /// </summary>
    public enum EVar
    {
        /// <summary>
        /// キャラクタのユニークなidです．
        /// </summary>
        id,
        /// <summary>
        /// キャラの名前です。
        /// </summary>
        名前,
        ニックネーム,
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
        //称号,
        メイン武器,

        作戦,

        // 戦闘一時格納変数用。数値パラメータもあるけど、文字列型と一緒くたにして扱いたいからここではEVarとして定義している。
        /// <summary>
        /// 味方キャラだと"1"，敵キャラだと"0"です．
        /// </summary>
        この戦闘では味方キャラ,
        この戦闘では敵キャラ,
        このターンの作戦,
        このターンの攻撃対象id,
        このターンの攻撃対象キャラ,
        このターンの攻撃対象キャラ名,
        このターンの回復対象id,
        このターンの補助対象id,
        このターンの物理防御ダメージ軽減数,
        このターンの回避率,


        血液型,
        生年月日,
        年齢,
        身長,
        体重,
        個性を司る象徴語,
        //特に強い光の次元要素,
        //特に強い闇の次元要素,
        光と闇の転換プロセス,
        ひっさつ,
        ひっさつ効果,

        声優さん,

        服のサイズ,
        バスト,
        ウエスト,
        ヒップ,
        靴のサイズ,

        趣味,
        好きなもの,
        嫌いなもの,

        恋人の存在,

        // 後で整理
        このターンは攻撃対象にされない,
        サムネ画像,
        ファイルフルパス_サムネ画像,
    }

    /// <summary>
    /// EVarで定義される変数値（string型）によく使われるテンプレートを集めたクラスです．
    /// ただし，あくまで値の打ち間違いを防ぐためのテンプレートです．
    /// 変数名によってはこの中に存在しない値が存在します．なお，実際に潜在パラメータとして表示される文字列は，変数名ではなく，stringの値です．
    /// また，変数値の比較の際は，実際の値をint/double型に変換したりして比較します．
    /// </summary>
    public class CVarValue・変数値
    {
        /// <summary>
        /// =""。空の文字列。あらゆる変数が取る初期値。setVar("新しい変数名")で作った時、初期値が未定義の場合、この空の文字列""が入る。
        /// </summary>
        public static readonly string _none_未定義 = "";
        /// <summary>
        /// =null。できれば使わないでください。（念のため、こう書いておけば、誰もあえて使ったりしないよね…）
        /// プログラム上で、if(getVar(EVar.変数名) != null)なんてエラーチェック文は書きたくないです。
        /// 原則変数にnullを割り当てたりせずに、""を使ってください。
        /// </summary>
        public static readonly string _null_原則変数にnullを割り当てないでください = null;
        /// <summary>
        /// ="NO"。bool値のtrue/falseで管理するスイッチ、オプションのON/OFF、キャラ毎のフラグなど、
        /// 値を２パターン（0/1）、あるいは３パターン（0/1/未定義）、もしくは４パターン（0/1/両方/未定義）
        /// しか値を取らない変数は、キャスト出来る数値と混同しないよう、これを使うといいかも。
        /// </summary>
        public static readonly string _NO = "NO";
        /// <summary>
        /// ="YES"。bool値のtrue/falseで管理するスイッチ、オプションのON/OFF、キャラ毎のフラグなど、
        /// 値を２パターン（0/1）、あるいは３パターン（0/1/未定義）、もしくは４パターン（0/1/両方/未定義）
        /// しか値を取らない変数は、キャスト出来る数値と混同しないよう、これを使うといいかも。
        /// </summary>
        public static readonly string _YES = "YES";
        /// <summary>
        /// ="BOTH"。両方を取るという意味。ゲームオプションのON/OFF、キャラ毎のフラグなど、
        /// 値を２パターン（0/1）、あるいは３パターン（0/1/未定義）、もしくは４パターン（0/1/両方/未定義）
        /// しか値を取らない変数は、キャスト出来る数値と混同しないよう、これを使うといいかも。
        /// </summary>
        public static readonly string _BOTH = "BOTH";

        // 以下、数値はメソッドで検証するため、こういうのは要らない
        ///// <summary>
        ///// ="0"。半角の0。全角数字を用意したい場合は、MyTools.getZenkakuString()を使うといい。
        ///// </summary>
        //public static readonly string _0 = "0";
        //public static readonly string _1 = "1";
        //public static readonly string _2 = "2";
        //public static readonly string _3 = "3";


        // 以下、サイズ（服のサイズ、敵のサイズなど）
        public static readonly string size_SS = "SS";
        public static readonly string size_S = "S";
        public static readonly string size_M = "M";
        public static readonly string size_L = "L";
        public static readonly string size_LL = "LL";
        public static readonly string size_XL = "XL";

        // 以下、ランク（キャラの強さの総合評価、キャラ毎のミニゲームのうまさ、など）
        public static readonly string rank_Gminus = "G-";
        public static readonly string rank_G = "G";
        public static readonly string rank_Gplus = "G+";
        public static readonly string rank_Fminus = "F-";
        public static readonly string rank_F = "F";
        public static readonly string rank_Fplus = "F+";
        public static readonly string rank_Eminus = "F-";
        public static readonly string rank_E = "E";
        public static readonly string rank_Eplus = "E+";
        public static readonly string rank_Dminus = "D-";
        public static readonly string rank_D = "D";
        public static readonly string rank_Dplus = "D+";
        public static readonly string rank_Cminus = "C-";
        public static readonly string rank_C = "C";
        public static readonly string rank_Cplus = "C+";
        public static readonly string rank_Bminus = "B-";
        public static readonly string rank_B = "B";
        public static readonly string rank_Bplus = "B+";
        public static readonly string rank_Aminus = "A-";
        public static readonly string rank_A = "A";
        public static readonly string rank_Aplus = "A+";
        public static readonly string rank_AA = "AA";
        public static readonly string rank_AAA = "AAA";
        public static readonly string rank_Sminus = "S-";
        public static readonly string rank_S = "S";
        public static readonly string rank_Splus = "S+";
        public static readonly string rank_SS = "SS";
        public static readonly string rank_SSS = "SSS";
        public static readonly string rank_SSSS = "SSSS";
        public static readonly string rank_Z = "Z";

        // 以下、血液型
        /// <summary>
        /// ="AA"。血液型。AAとAOを同値に扱いたい場合は、できれば"AA"と"AO"どっちも判定に入れて欲しい
        /// </summary>
        public static readonly string blood_AA = "AA";
        public static readonly string blood_BB = "BB";
        public static readonly string blood_O = "O";
        public static readonly string blood_AO = "AO";
        public static readonly string blood_BO = "BO";
        public static readonly string blood_AB = "AB";
        /// <summary>
        /// ="-"
        /// </summary>
        public static readonly string blood_Unknown_minus = "-";
        /// <summary>
        /// ="No"
        /// </summary>
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



    ///// <summary>
    ///// キャラやシステムを表す様々な変数や備考・特記事項などを，自由に追加・削除可能な変数群クラスです．
    ///// ※名前を省略する目的のためだけに作られる日本語名クラスのため，特別な理由がない限り，新しいプロパティ・メソッドを追加しないことが望ましいです．
    ///// </summary>
    //public class 変数一覧 : CVars・変数一覧{

    //}

    /// <summary>
    /// 全てのクラスで参照可能なstatic変数を持っている、変数一覧クラスです。
    /// staticでない値を取得する方法は、EPara列挙体を参照してください。
    /// 
    /// 【staticなクラスとしての使い方】：
    /// ゲームで一括管理したい変数は、わかりやすい名前を付けてここのstatic変数に追加すると、
    /// ●「CVars・変数一覧.s_***」
    /// で、どのクラスからでも簡単に参照できます。
    /// 
    /// 【staticでない、インスタンスとしての使い方】：
    /// 様々な型の変数を、Dictionaly＜string, Char・変数クラス＞で、名前や型に自由度を持たせたまま、一括管理できます。
    /// （List＜Char・変数クラス＞にしなかったのは、ゲーム上で新しい変数を自由に追加／削除したかったからです。）
    /// 個々の変数は，基本的にEVar.変数名などの列挙体で指定します．
    /// 例えば、CChara・キャラクラスでは，パラメータ（double型）以外の，
    /// キャラの名前，ニックネーム，その他プロフィールなどを，
    /// 原則string型（必要に応じてobject型）で表現される変数を一括管理するクラスとして使っています．
    /// 
    ///  【共通の特徴】：
    ///  このクラスに定義された変数は、自由に変数名や型を決められるので、
    ///  キャラ毎の一時的なフラグやストーリー進行度、ゲーム全体の設定変数などにフレキシブルに利用できます。
    ///  また、プログラム上で一時的な変数を作りたい場合、
    ///  setVar()／removeVar()メソッドなどで自由に作成／削除可能なように設計されています．    
    /// 
    /// 　　※なお、キャラのパラメータなど、数値（double型）で高速に参照する必要がある変数は、
    /// 　　　EPara列挙体に追加して、CParas・パラメータ一覧クラスを使ってください。
    /// 
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CVars・変数一覧
    {
        /// <summary>
        /// この変数一覧を利用している、ゲーム管理者クラスです。
        /// ほぼゲームに必要な機能を全て呼び出せるので、このインスタンスを持てば、
        /// 非常に強力なパワーを引き出します（もうメソッドの引数の渡し方に迷う必要はありません！…たぶん）。
        /// 
        /// 　　　※初期値はnullですが、ゲーム開始後はちゃんと入っています。
        /// （ゲーム開始時は必ずコンストラクタnew CVars・変数一覧(_game)が
        /// 一度は呼び出されるようにしているので、基本的にnull処理をしなくてもいいようになってます。）
        /// </summary>
        public static CGameManager・ゲーム管理者 game = null;
        public CVars・変数一覧(CGameManager・ゲーム管理者 _game)
        {
            game = _game;
        }
        public CVars・変数一覧()
        {
        }

        /// <summary>
        /// キャラ固有の名前，プロフィール，今の気分，その他キャラやシステムを表す様々な変数や備考・特記事項などを編集する，変数名と値（文字列）．
        /// </summary>
        Dictionary<string, CVar・変数> p_var・変数一覧 = new Dictionary<string, CVar・変数>();
        #region get/setアクセサ
        // 変数をDictionary<string, CVar・変数>で管理するとき用

        /// <summary>
        /// 引数で指定した変数名の変数値とオブジェクトを追加または変更します．
        /// 返り値は，上書きされる前の以前設定されていたオブジェクトの値（無い場合はnull）です．
        /// </summary>
        public object setVar・変数クラスを変更(EVar _varID, string _varValue, object _varOjbect)
        {
            string _varName = _varID.ToString();
            object _beforeVar = CVar・変数.s_none・変数値未定義;
            if (p_var・変数一覧.ContainsKey(_varName) == true)
            {
                
                _beforeVar = getVar・変数値(_varName);
                // 既にその変数名が定義されている場合は変更
                p_var・変数一覧[_varName].setVar・変数クラスを設定_クラス型(_varValue, _varOjbect);
            }
            else
            {
                // 無い場合は新しく追加
                p_var・変数一覧.Add(_varName, new CVar・変数(_varValue, _varOjbect));
            }
            return _beforeVar;
        }
        /// <summary>
        /// 引数で指定した変数名の変数値を追加または変更します．
        /// 返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数値を変更(EVar _varID, string _varValue)
        {
            return setVar・変数値を変更(_varID.ToString(), _varValue);
        }

        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．
        /// 返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        /// <param name="_変数名"></param>
        /// <param name="_varValue"></param>
        /// <returns></returns>
        public string setVar・変数値を変更(string _varName, string _varValue)
        {
            string _beforeVar = CVar・変数.s_none・変数値未定義;
            if (p_var・変数一覧.ContainsKey(_varName) == true)
            {
                _beforeVar = getVar・変数値(_varName);
                // 既にその変数名が定義されている場合は値だけ変更
                p_var・変数一覧[_varName].set(_varValue);
            }
            else
            {
                // 無い場合は新しく追加
                p_var・変数一覧.Add(_varName, new CVar・変数(_varValue));
            }
            return _beforeVar;
        }


        /// <summary>
        /// 引数で指定した変数名の変数クラスを取得します．未定義の場合は，nullを返します．
        /// </summary>
        /// <param name="_varName"></param>
        /// <returns></returns>
        public CVar・変数 getVar・変数クラス(EVar _varID)
        {
            return getVar・変数クラス(_varID.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の変数クラスを取得します．未定義の場合は，nullを返します．
        /// </summary>
        /// <param name="_varName"></param>
        /// <returns></returns>
        public CVar・変数 getVar・変数クラス(string _varName)
        {
            if (p_var・変数一覧.ContainsKey(_varName) == true)
            {
                return p_var・変数一覧[_varName];
            }
            else
            {
                // 無い場合はnullで統一
                return null;
            }
        }
        /// <summary>
        /// 引数で指定した変数名の値を取得します．該当する変数が存在しない場合は""が返ります。
        /// </summary>
        /// <param name="_変数名"></param>
        public string getVar・変数値(EVar _varID)
        {
            return getVar・変数値(_varID.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の変数値を取得します．該当する変数が存在しない場合は""が返ります。
        /// </summary>
        /// <param name="_変数名"></param>
        public string getVar・変数値(string _varName)
        {
            if (p_var・変数一覧.ContainsKey(_varName) == true)
            {
                return p_var・変数一覧[_varName].get();
            }
            else
            {
                // 無い場合は""で統一
                return CVar・変数.s_none・変数値未定義;
            }
        }
        #endregion

        /// <summary>
        /// 引数で指定した変数名の定義を削除します．
        /// 基本的に、メモリを節約するためだけ（もう使わなくなった一時変数削除など）に使います。
        /// ※EVarに定義されている変数を削除すると、その値は""に初期化されますが、あまりやる必要はないと思います。
        /// </summary>
        /// <param name="_変数名"></param>
        /// <returns></returns>
        public void removeVar・変数を削除(string _varName)
        {
            p_var・変数一覧.Remove(_varName);
        }

    }



}
