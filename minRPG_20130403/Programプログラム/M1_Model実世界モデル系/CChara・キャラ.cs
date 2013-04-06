using System;
using System.Collections.Generic;
using System.Text;
//using Yanesdk;
using System.Drawing;
using PublicDomain;

namespace PublicDomain
{
    #region 日本語クラス（名前を短くしたクラス）
    // CChara・キャラは元々名前が短いので、日本語クラス使用をやめた
    ///// <summary>
    ///// プレイヤーや敵キャラ，町の人やイベントキャラ，ペットなどを含めた，一人のキャラが持つ情報を格納するクラスです．
    ///// ※CCharacter・キャラクラスの名前を省略する目的のためだけに作られる日本語名クラスのため，特別な理由がない限り，新しいプロパティ・メソッドを追加しないことが望ましいです．
    ///// </summary>
    //public class Cキャラ : CChara・キャラ
    //{

    //}
    #region メモ [ToDo]名前を省略したクラスでの呼び出し・日本語化はかなりややこしい
    // [ToDo]名前を省略したクラスでの呼び出し・日本語化はかなりややこしい．ベースのクラスを持つラッパークラスを作ってやってみた（CChoiceResult・選択結果クラスと選択結果ラッパークラス）．が，使い方が分散され複雑になるよりは，一つのクラスだけでやった方がいいかも．省略形クラスは名前を変えるだけで，とりあえず無しの方向で．
        
    // ※日本語クラスの作り方
        // (_onelineString)英語で作ったクラス内のプロパティ・メソッドを自由に使いたいなら，継承してもいい（※カプセル化が汚くなると思うので非推奨）
            //public class CAnswer・回答 : CAnswerManager{
        // (b)選択肢をGChoiceのラッパークラスにするなら，クラスを持つという形が一般的
            //CAnswerManager p_usedClass = new CAnswerManager();
    /*
    /// <summary>
    /// プレイヤーや敵キャラ，町の人やイベントキャラ，ペットなどを含めた，全てのキャラのベースクラスのラッパークラスです．
    /// </summary>
    public class Cキャラ//[ToDo]※継承した使い方は，何か新しいメソッドを作ると煩雑になる？ : CChara・キャラ
    {
        // [ToDo]継承するより，こうした方がいい？
        CChara・キャラ p_usedClass = new CChara・キャラ();
        public CParas・パラメータ一覧 Paras・パラメータ一括処理()
        {
            return p_usedClass.p_parameters・パラメータ一覧;
        }

    }
     * */
    #endregion
    #endregion

    /// <summary>
    /// キャラのパラメータの変更の仕方（代入・増減・乗除など、setParaメソッドによる値の変更の仕方）を定義した列挙体です。
    /// </summary>
    public enum ESet{
        /// <summary>
        /// 新しい値を代入します．（newValue = _default・代入値）
        /// </summary>
        _default・代入値,
        /// <summary>
        /// 現在の値を増減します．（ newValue = value + add・増減値)
        /// </summary>
        add・増減値,
        /// <summary>
        /// 現在の値をパーセンテージで掛け算します．（ newValue = value * _rate・倍率)
        /// </summary>
        _rate・倍率,
    }

    /// <summary>
    /// プレイヤーや敵キャラ，町の人やイベントキャラ，ペットなどを含めた，一人のキャラが持つ情報を格納するクラスです．
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CChara・キャラ
    {
        // ※キャラクラスのプロパティは全てprivate（クラス外部から保護）にして、参照する時はget/setメソッドを使って。

        /// <summary>
        /// キャラ固有のidです．
        /// </summary>
        int p_id = 0;
        public void setId・IDを変更(int _キャラID)
        {            
            p_id = _キャラID;
        }

        // ●●●パラメータ
        /// <summary>
        /// 力，体力，攻撃力，基本6色パラメータ総合値など，double型で表現されるパラメータ名と値を格納したクラス．
        /// </summary>
        CParas・パラメータ一覧 p_parameters・パラメータ一覧 = new CParas・パラメータ一覧();
        #region get/setアクセサ
        /// <summary>
        /// getP_Paras()の名前省略メソッドです。キャラクタのパラメータクラスを取得します．
        /// ※色・身体・精神・戦闘パラなどを一括して取得／セットしたいときに使用します．
        /// </summary>
        /// <returns></returns>
        public CParas・パラメータ一覧 Paras・パラメータ一括処理()
        {
            return getP_parameters();
        }
        /// <summary>
        /// Paras・パラメータ一括処理()といっしょです。できればparas()を使ってください。キャラクタのパラメータクラスを取得します．
        /// ※色・身体・精神・戦闘パラなどを一括して取得／セットしたいときに使用します．
        /// </summary>
        /// <returns></returns>
        private CParas・パラメータ一覧 getP_parameters()
        {
            return p_parameters・パラメータ一覧;
        }
        /// <summary>
        /// キャラクタのパラメータクラスを変更します．
        /// ※キャラのパラメータをそっくり入れ替えたい場合に使用します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <param name="_value・変更後の値"></param>
        public void setP_parameters(CParas・パラメータ一覧 _parameters)
        {
            p_parameters・パラメータ一覧 = _parameters;
        }
        /// <summary>
        /// 指定のパラメータの値を代入します．代入以外の操作を行いたい場合は、他の同名メソッドを使ってください．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public void setPara(EPara _parameterID・パラメータID, double _代入値)
        {
            setPara(_parameterID・パラメータID, ESet._default・代入値, _代入値);
        }
        /// <summary>
        /// ●指定のパラメータの値を変更します．絶対値の（代入）と相対値（増減・乗除）の両方が使えます．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public void setPara(EPara _parameterID・パラメータID, ESet _変更方法, double _代入値or増減値or倍率)
        {
            double _newValue・変更後の値 = Para(_parameterID・パラメータID);
            if (_変更方法 == ESet._default・代入値)
            {
                _newValue・変更後の値 = _代入値or増減値or倍率;
            }
            else if (_変更方法 == ESet.add・増減値)
            {
                _newValue・変更後の値 += _代入値or増減値or倍率;
            }
            else if (_変更方法 == ESet._rate・倍率)
            {
                _newValue・変更後の値 *= _代入値or増減値or倍率;
            }
            Paras・パラメータ一括処理().setPara・パラを変更(_parameterID・パラメータID, _newValue・変更後の値);
        }
        #region 上記メソッドの派生形
        /// 指定のパラメータに新しい値を（絶対値で）代入します．
        /// ※非推奨です．大量にセットする場合以外は，相対値で変更も可能な「setPara」を使ってください．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public void setParaValue(EPara _parameterID・パラメータID, double _newValue・変更後の値)
        {
            Paras・パラメータ一括処理().setPara・パラを変更(_parameterID・パラメータID, _newValue・変更後の値);
        }
        /// <summary>
        /// 指定のパラメータに新しい値を（絶対値で）代入します．
        /// ※非推奨です．大量にセットする場合以外は，相対値で変更も可能な「setPara」を使ってください．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public void setParaValue(EPara _parameterID・パラメータID, int _newValue・変更後の値)
        {
            setParaValue(_parameterID・パラメータID, (double)_newValue・変更後の値);
        }
      #region 各色パラメータの指定セットを作ったが，直接は極力呼び出さない（一括セットは，Paras・パラメータ一括処理().の色パラ群のセットを使う！）
        ///// <summary>
        ///// 指定の色パラメータの値を変更します．絶対値の（代入）と相対値（増減・乗除）の両方が使えます．
        ///// </summary>
        ///// <param name="_iro18Parameter"></param>
        ///// <param name="_変更方法"></param>
        ///// <param name="_代入値or増減値or倍率"></param>
        //public void setPara(EIro18Para・色パラメータ _iro18Parameter, ESet _変更方法, double _代入値or増減値or倍率)
        //{
        //    EPara _parameterID = MyTools.getEnumItem_FromString<EPara>(_iro18Parameter.ToString(), 0, 17);
        //    setPara(_parameterID, _変更方法, _代入値or増減値or倍率);
        //}
        ///// <summary>
        ///// 指定の戦闘パラメータの値を変更します．絶対値の（代入）と相対値（増減・乗除）の両方が使えます．
        ///// </summary>
        ///// <param name="_iro18Parameter"></param>
        ///// <param name="_変更方法"></param>
        ///// <param name="_代入値or増減値or倍率"></param>
        //public void setPara(ESPara・戦闘パラメータ _parameter, ESet _変更方法, double _代入値or増減値or倍率)
        //{
        //    EPara _parameterID = MyTools.getEnumItem_FromString<EPara>(_parameter.ToString(), 36, 80);
        //    setPara(_parameterID, _変更方法, _代入値or増減値or倍率);
        //}
        #endregion
        #endregion

        /// <summary>
        /// ●指定のパラメータの値を返します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public double Para(EPara _parameterID・パラメータID)
        {
            return Paras・パラメータ一括処理().getParaValue・パラの値取得(_parameterID・パラメータID);
        }
        #region 上記メソッドの派生形
        /// <summary>
        /// ※非推奨です．原則，パラメータの取得／変更は，Para(パラ名)／setParaValue(パラ名, ***)を使ってください．
        /// ただし，日本語メソッドが用意されているものは，a01_ちから()／a01_ちからを変更()を使ってください．
        /// 
        /// 指定したIDのパラメータの値を返します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public double getPara・パラ(EPara _parameterID)
        {
            return Para(_parameterID);
        }
        /// <summary>
        /// 指定のパラメータの値をint型で返します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public int para_Int(EPara _parameterID・パラメータID)
        {
            return (int)Para(_parameterID・パラメータID);
        }
        /// <summary>
        /*
        /// <summary>
        /// ※非推奨です．原則，パラメータの取得／変更は，Para(パラ名)／setParaValue(パラ名, ***)を使ってください．
        /// ただし，日本語メソッドが用意されているものは，a01_ちから()を使ってもいいです．
        /// 
        /// 指定したIDのパラメータクラスを返します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public CPara・パラメータ getPara(EPara _parameterID)
        {
            return Paras・パラメータ一括処理().getPara・パラ(_parameterID);
        }
         * */

        // 【廃止】パラメータクラスや値を簡単にを呼び出す(get)日本語メソッド
        // ※※※わざわざ新しいパラメータに日本語メソッド（クラス，値ともに）作るのは面倒くさいし，
        //       はじめの英語が汚いし，
        //       それならむしろ，Para(パラ名)／setParaValue(パラ名, ...)でやる方がリストが綺麗で賢いので，
        //       パラメータの日本語メソッドは廃止！
        // 
        // public CPara・パラメータ c01_赤()
        // { return Paras・パラメータ一括処理().getPara・パラ(EPara.c01_赤); }
        // public double c01_赤()
        // { return Para(EPara.c01_赤)}
        #endregion
        #endregion

        // ●●●変数群
        /// <summary>
        /// 名前，称号，感情状態など，string型で表現される変数名と値を格納したクラス．
        /// </summary>
        CVars・変数一覧 p_Vars・変数一覧 = new CVars・変数一覧();
        #region get/setアクセサ
        /// <summary>
        /// キャラクタの変数一覧を取得します．
        /// ※変数の取得はVar()で行うため、基本的にはあまり使われませんが、全ての変数を一括して処理したい時に使います。
        /// </summary>
        /// <returns></returns>
        public CVars・変数一覧 getP_Vars・変数一括処理()
        {
            return p_Vars・変数一覧;
        }
        /// <summary>
        /// キャラクタの特徴クラスを変更します．
        /// ※変数の代入はVar()で行うため、基本的にはあまり使われませんが、全ての変数を一括して置き換えたい時に使います。
        /// </summary>
        /// <param name="_varName・変更する変数名"></param>
        /// <param name="_value・変更後の値"></param>
        public void setP_Vars・変数一覧を全て置き換え(CVars・変数一覧 _vars)
        {
            p_Vars・変数一覧 = _vars;
        }
        // 特徴をDictionary<string, CVar・変数>で管理するとき用
        /// <summary>
        /// ●引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数を変更(EVar _EVar・変更する変数ID, string _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_EVar・変更する変数ID, _value・変更後の値);
        }
        /// <summary>
        /// ●引数で指定した変数名の値（string型とオブジェクト型の両方）を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名のオブジェクト（無い場合はnull）です．
        /// </summary>
        public object setVar・変数を変更(EVar _EVar・変更する変数ID, object _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数クラスを変更(_EVar・変更する変数ID, _value・変更後の値.ToString(), _value・変更後の値);
        }
        #region 上記と機能同一メソッド
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        /// <param name="_変数名"></param>
        /// <param name="_varValue"></param>
        /// <returns></returns>
        public string setVar・変数を変更(EVar _EVar・変更する変数ID, CVarValue・変数値 _varValue・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_EVar・変更する変数ID, _varValue・変更後の値.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        /// <param name="_変数名"></param>
        /// <param name="_varValue"></param>
        /// <returns></returns>
        public string setVar・変数を変更(string _varName・変更する変数名, string _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_varName・変更する変数名, _value・変更後の値);
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        /// <param name="_変数名"></param>
        /// <param name="_varValue"></param>
        /// <returns></returns>
        public string setVar・変数を変更(string _varName・変更する変数名, CVarValue・変数値 _varValue・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_varName・変更する変数名, _varName・変更する変数名.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数を変更(EVar _EVar・変更する変数ID, int _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_EVar・変更する変数ID, _value・変更後の値.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数を変更(string _varName・変更する変数名, int _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_varName・変更する変数名, _value・変更後の値.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数を変更(EVar _EVar・変更する変数ID, double _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_EVar・変更する変数ID, _value・変更後の値.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数を変更(string _varName・変更する変数名, double _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_varName・変更する変数名, _value・変更後の値.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数を変更(EVar _EVar・変更する変数ID, bool _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_EVar・変更する変数ID, MyTools.getBoolValue(_value・変更後の値).ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数を変更(string _varName・変更する変数名, bool _value・変更後の値)
        {
            return getP_Vars・変数一括処理().setVar・変数値を変更(_varName・変更する変数名, MyTools.getBoolValue(_value・変更後の値).ToString());
        }
        #endregion
        /// <summary>
        /// ●引数で指定した変数名の値を取得します．
        /// </summary>
        /// <param name="_変数名"></param>
        public string Var(EVar _EVar・変数名ID)
        {
            return getP_Vars・変数一括処理().getVar・変数値(_EVar・変数名ID);
        }
        /// <summary>
        /// 引数で指定した変数名の値をDouble型で取得します．
        /// </summary>
        /// <param name="_EVar・変数名ID"></param>
        /// <returns></returns>
        public double Var_Double(EVar _EVar・変数名ID)
        {
            return MyTools.parseDouble(Var(_EVar・変数名ID));
        }
        /// <summary>
        /// 引数で指定した変数名の値をInt型で取得します．
        /// </summary>
        /// <param name="_EVar・変数名ID"></param>
        /// <returns></returns>
        public int Var_Int(EVar _EVar・変数名ID)
        {
            return MyTools.parseInt(Var(_EVar・変数名ID));
        }
        /// <summary>
        /// 引数で指定した変数名の値をBool型（1,0）で取得します．
        /// </summary>
        /// <param name="_EVar・変数名ID"></param>
        /// <returns></returns>
        public bool Var_Bool(EVar _EVar・変数名ID)
        {
            return MyTools.getBool(Var(_EVar・変数名ID));
        }
        #region 上記メソッドの他バージョン
        /// <summary>
        /// ※できれば，特徴リストEFeaに定義されている特徴は，IDで呼び出す同一メソッド「Var(EVar.特徴量ID名)」を使ってください．引数で指定した変数名の値を取得します．
        /// </summary>
        /// <param name="_変数名"></param>
        private string Var(string _変数名)
        {
            return getP_Vars・変数一括処理().getVar・変数値(_変数名);
        }
        /// <summary>
        /// ※できれば同一メソッド「Var」を使ってください．引数で指定した変数名の値を取得します．
        /// </summary>
        /// <param name="_変数名"></param>
        private string getVar・変数を取得(EVar _EVar・変数名ID)
        {
            return Var(_EVar・変数名ID);
        }
        #endregion
        /// <summary>
        /// ●引数で指定した変数名の値が等しいかを返します．
        /// 変数値がよく使う値の場合は，できるだけ，CVarValue・変数値の列挙体を参照してください．
        /// </summary>
        /// <param name="_変数名"></param>
        public bool isVar(string _変数名, string CVarValue・変数値_orValueStringこの文字列と等しいか)
        {
            bool _isEqual = false;
            if (Var(_変数名) == CVarValue・変数値_orValueStringこの文字列と等しいか)
            {
                _isEqual = true;
            }
            return _isEqual;
        }
        /// <summary>
        /// ●引数で指定した変数名の値が"YES"かどうかを返します．"NO"の場合は、isVar(EVar.***)==falseを使ってください。
        /// 変数名はEVar列挙体で、変数値がbool値などを取る場合は，できるだけ，CVarValue・変数値の列挙体を参照してください．
        /// </summary>
        /// <param name="_変数名"></param>
        public bool isVarYES(EVar _変数名)
        {
            bool _isEqual = false;
            if (Var(_変数名) == CVarValue・変数値._YES)
            {
                _isEqual = true;
            }
            return _isEqual;
        }
        /// <summary>
        /// ●引数で指定した変数名の値が"YES"かどうかを返します．"NO"の場合は、isVar(EVar.***)==falseを使ってください。
        /// 変数名はできるだけEVar列挙体を使ってください。また、変数値がbool値などを取る場合は，できるだけ，CVarValue・変数値の列挙体を参照してください．
        /// </summary>
        /// <param name="_変数名"></param>
        public bool isVarYES(string _変数名)
        {
            bool _isEqual = false;
            if (Var(_変数名) == CVarValue・変数値._YES)
            {
                _isEqual = true;
            }
            return _isEqual;
        }
        #region 上記メソッドの他バーション
        // もしenum型で作るなら、こうする
        ///// <summary>
        ///// 引数で指定した変数名の値が等しいかを返します．
        ///// 変数値がよく使う値の場合は，できるだけ，CVarValue・変数値の列挙体を参照してください．
        ///// </summary>
        ///// <param name="_変数名"></param>
        //public bool isVar(EVar _EVar・変数名ID, EVarValue・変数値 _EVarValue・変数値と等しいか)
        //{
        //    return isVar(_EVar・変数名ID, EVarValue・変数値と等しいか.);
        //}
        /// <summary>
        /// 引数で指定した変数名の値が等しいかを返します．
        /// 変数値がよく使う値の場合は，できるだけ，CVarValue・変数値の列挙体を参照してください．
        /// </summary>
        /// <param name="_変数名"></param>
        public bool isVar(EVar _EVar・変数名ID, int _value・この値と等しいか)
        {
            bool _isEqual = false;
            if (MyTools.parseInt(Var(_EVar・変数名ID)) == _value・この値と等しいか)
            {
                _isEqual = true;
            }
            return _isEqual;
        }
        /// <summary>
        /// 引数で指定した変数名の値が等しいかを返します．
        /// 変数値がよく使う値の場合は，できるだけ，CVarValue・変数値の列挙体を参照してください．
        /// </summary>
        /// <param name="_変数名"></param>
        public bool isVar(EVar _EVar・変数名ID, double _value・この値と等しいか)
        {
            bool _isEqual = false;
            if (MyTools.parseDouble(Var(_EVar・変数名ID)) == _value・この値と等しいか)
            {
                _isEqual = true;
            }
            return _isEqual;
        }
        /// <summary>
        /// 引数で指定した変数名の値が等しいかを返します．
        /// 変数値がよく使う値の場合は，できるだけ，CVarValue・変数値の列挙体を参照してください．
        /// </summary>
        /// <param name="_変数名"></param>
        public bool isVar(string _変数名, int _value・この値と等しいか)
        {
            bool _isEqual = false;
            if (MyTools.parseInt(Var(_変数名)) == _value・この値と等しいか)
            {
                _isEqual = true;
            }
            return _isEqual;
        }
        /// <summary>
        /// 引数で指定した変数名の値が等しいかを返します．
        /// 変数値がよく使う値の場合は，できるだけ，CVarValue・変数値の列挙体を参照してください．
        /// </summary>
        /// <param name="_変数名"></param>
        public bool isVar(string _変数名, double _value・この値と等しいか)
        {
            bool _isEqual = false;
            if (MyTools.parseDouble(Var(_変数名)) == _value・この値と等しいか)
            {
                _isEqual = true;
            }
            return _isEqual;
        }
        #endregion
        #endregion
        #region 名前などの特徴を直接呼び出せるようにする日本語メソッド（[Tips]パラメータの日本語メソッドは廃止したが，特徴は，よく使う特徴（名前など）だけ作ることにする．でも，もう最低限以外は作らない！　[Tips]プロパティを作る方法は．別の記憶領域への格納と更新が必要になるのでダメ！）．

        /// <summary>
        /// キャラ名（よく参照される参照名，ユーザなら名前）です．name名前()メソッドで取得します。
        /// 厳密にはVar(EVar.名前)で取得する名前とはメモリが異なりますが、（一時変更時以外）基本的に同じ文字列を格納しています。： getP_Vars・変数一括処理().getVar・変数値(EVar.名前); }
        /// </summary>
        string p_name = "";
        public void setName・名前を一時的に変更(string _キャラ名)
        {
            p_name = _キャラ名;
            setVar・変数を変更(EVar.名前, _キャラ名);
        }
        public string name名前()
        { return p_name; } // 厳密にはこっちと異なるが、（一時変更時以外）基本的に同じ文字列を格納している： getP_Vars・変数一括処理().getVar・変数値(EVar.名前); }
        public string syougo称号()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.称号); }
        public string sei性別()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.性別); }
        /*
        public string nenre年齢()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.年齢); }
        public string ketueki血液型()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.血液型); }
        public string tait体調()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.体調); }
        public string kanzyo感情()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.感情); }
        public string kibu気分()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.今の気分); }
        public string serihu登場セリフ()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.登場セリフ); }
        public string syougo称号()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.通り名); }

        public string konotaこのターンの作戦()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.このターンの作戦); }
        public string konotaこのターンの攻撃対象id()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.このターンの攻撃対象id); }
        public CChara・キャラ konotaこのターンの攻撃対象キャラ()
        { return (CChara・キャラ)(getP_Vars・変数一括処理().getVar・変数クラス(EVar.このターンの攻撃対象キャラ).getObject()); }
        public string konotaこのターンの攻撃対象キャラ名()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.このターンの攻撃対象キャラ名); }
        public string konotaこのターンの回復対象id()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.このターンの回復対象id); }
        public string konotaこのターンの補助対象id()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.このターンの補助対象id); }
        public string konotaこのターンの防御力()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.このターンの物理防御ダメージ軽減数); }
        public string konotaこのターンの回避率()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.このターンの回避率); }


        public string sint身長()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.身長); }
        public string taiz体重()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.体重); }
        public string huku服のサイズ()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.服のサイズ); }
        public string basuバスト()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.バスト); }
        public string uestウエスト()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.ウエスト); }
        public string hipヒップ()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.ヒップ); }
        public string tait靴のサイズ()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.靴のサイズ); }

        public string syumi趣味()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.趣味); }
        public string sukina好きなもの()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.好きなもの); }
        public string kirai嫌いなもの()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.嫌いなもの); }
        public string koibi恋人の存在()
        { return getP_Vars・変数一括処理().getVar・変数値(EVar.恋人の存在); }
        */
        #endregion


        /// <summary>
        /// 習得済みのアビリティ（能力）の一覧リストです．アビリティ毎に装着のOn・Offを切り替えます（装着アビリティのリストも兼ねます）．
        /// </summary>
        List<CAbility・能力> p_ability・習得アビリティ = new List<CAbility・能力>();
        /// <summary>
        /// 所有アイテムの一覧リストです．アイテム毎に装備のOn／Offを切り替えます（装備アイテムのリストも兼ねます）．
        /// </summary>        
        List<CItem・アイテム> p_item・所有アイテム = new List<CItem・アイテム>();
        /// <summary>
        /// 閃き済みの技の一覧リストです．
        /// </summary>
        List<CSkill・特技> p_skill・閃きスキル = new List<CSkill・特技>();

        // 以下、コマンド
        List<CDiceCommand・ダイスコマンド> p_dice・所有ダイス = null;
        List<CCommand・コマンド> p_battleCommand1・戦闘開始用コマンド = null;
        #region get/setアクセサ
        /// <summary>
        /// キャラが所有しているダイス戦闘用コマンドです．
        /// 呼び出し時にnullの場合は自動的に設定されているので、nullになることはありません。
        /// </summary>
        public List<CDiceCommand・ダイスコマンド> getP_dice・所有ダイス()
        {
            // 存在しない場合は、自動で作る
            if (p_dice・所有ダイス == null)
            {
                p_dice・所有ダイス = new List<CDiceCommand・ダイスコマンド>();
                CCharaCreator・キャラ生成機.createDiceCommand_FromParas・ダイスコマンドを自動生成(this);
            }
            return p_dice・所有ダイス;
        }
        public List<CCommand・コマンド> getP_dice_ToCommand・所有ダイスをコマンド型で取得()
        {
            List<CDiceCommand・ダイスコマンド> _dices = getP_dice・所有ダイス();
            List<CCommand・コマンド> _commands = new List<CCommand・コマンド>();
            foreach (CDiceCommand・ダイスコマンド _item in _dices)
            {
                _commands.Add((CCommand・コマンド)_item);
            }
            return _commands;
        }
        public void setP_dice・所有ダイス(List<CDiceCommand・ダイスコマンド> _ダイスコマンド)
        {
            //[MEMO]Listリストのset（代入）は、=だけだと参照元が消えるとメモリが消えてしまう。できるだけAddRangeでやって！
            // (a)これだと参照元のやつが消えると消えてしまう。AddRangeでやった方が余計なエラーが無くて済む。
            //p_dice・所有ダイス = _ダイスコマンド;
            // (b)AddRangeを使って、コピーを保存する。その前にリストを初期化するのも忘れずに。
            p_dice・所有ダイス.Clear();
            p_dice・所有ダイス.AddRange(_ダイスコマンド);
        }
        /// <summary>
        /// キャラが所有している戦闘開始用コマンド（たたかう、にげる等）です．
        /// 初期値は「たたかう」、「ダイスをふる」、「にげる」などが生成されているので、nullになることはありません。
        /// </summary>
        public List<CCommand・コマンド> getp_battleCommand1・戦闘開始用コマンド() {
            // 存在しない場合は、自動で作る
            if (p_battleCommand1・戦闘開始用コマンド == null)
            {
                p_battleCommand1・戦闘開始用コマンド = new List<CCommand・コマンド>();
                p_battleCommand1・戦闘開始用コマンド.Add(new CCommand・コマンド(CCommand・コマンド.s_CommandName_Fight・たたかう));
                p_battleCommand1・戦闘開始用コマンド.Add(new CCommand・コマンド(CCommand・コマンド.s_CommandName_Dice・ダイスをふる));
                p_battleCommand1・戦闘開始用コマンド.Add(new CCommand・コマンド(CCommand・コマンド.s_CommandName_Escape・にげる));
            }
            return p_battleCommand1・戦闘開始用コマンド;
        }
        #endregion

        // 以下、草案
        ///// <summary>
        ///// 普通・喜び・必殺などの顔画像や立ち画像の，種類名と画像．
        ///// </summary>
        //Dictionary<string, Image> p_image・画像一覧 = new Dictionary<string, Image>();
        ///// <summary>
        ///// 今の顔表情の状態
        ///// </summary>
        //CFaceState・顔表情の状態 p_nowFaceState・今の顔表情の状態 = new CFaceState・顔表情の状態(CFaceState・顔表情の状態.EFaceEmotion・顔表情感情.f0通常で);
        ////GFaceType・顔表情 nowFaceType・顔表情 = new GFaceType・顔表情();

        //CPositiveNegative・光陰 p_nowHeryDark・聖闇 = new CPositiveNegative・光陰();

        ///// <summary>
        ///// 今の感情（気分）．
        ///// </summary>
        //CEmotion・感情 p_nowEmotion・今の感情 = new CEmotion・感情();
        //CCondition・状態変化 p_nowCondition・今の状態変化 = new CCondition・状態変化();

        #region その他，プログラムのための，便利なパラメータショートカットメソッド　（まだ未整理）
        /// <summary>
        /// キャラのパラメータ自動回復を掛け算で補正する値を取得します．
        /// </summary>
        /// <returns></returns>
        public double paraAuto・パラ補正乗除()
        {
            return getPara・パラ(EPara.パラ自然回復補正乗除);
        }
        /// <summary>
        /// キャラのパラメータ自動回復を足し算で補正する値を取得します．
        /// </summary>
        /// <returns></returns>
        public double paraAuto・パラ補正増減()
        {
            return getPara・パラ(EPara.パラ自然回復補正増減);
        }
        #endregion



        #region コンストラクタ
        public CChara・キャラ()
        {

        }
        #endregion



        #region キャラの通常行動をすぐに呼び出し可能な動詞メソッド（草案）

        ///// <summary>
        ///// ダイアログに引数(1)のテキストを表示します．※まだ未完成！
        ///// </summary>
        ///// <param name="_text・_話すテキスト"></param>
        ///// <returns></returns>
        //public void speak・話す(string _text・_話すテキスト)
        //{
        //    // ある自作？ダイアログにテキストを表示
        //}
        
        ///// <summary>
        ///// キャラの標準画像がスクリーンの引数(1)の位置に出現します．※まだ未完成！
        ///// </summary>
        ///// <param name="_shownPosition_Parcent・_出現位置_左0から右1への割合">0.0：左端-画像サイズ～1.0:右端-画像サイズ</param>
        ///// <returns></returns>
        //public void show・スクリーンに出現(float _shownPosition_Parcent・_出現位置_左0から右1への度合いパーセント)
        //{
        //    // ある自作？ダイアログの上に画像を表示
        //}
        ////public bool show・スクリーンに出現(ScreenPosition _shownPosition・_出現位置, int _MSec・表示ミリ秒, bool _isFade・フェード有無)
        ////[ToDo]:画面を横スライドするか，速度，フェードインアウトなどをいじれるようにしても良い
        ///// <summary>
        ///// キャラの標準画像がスクリーンの引数(1)の位置に出現します．※まだ未完成！
        ///// </summary>
        ///// <param name="_text・_話すテキスト"></param>
        ///// <returns></returns>
        //public void hide・スクリーンから消える()
        //{

        //}

        ///// <summary>
        ///// キャラの顔表情を，喜び・悲しみ・恐れ・怒り・嫌悪・笑・恥・涙などに変更します．※まだ未完成！
        ///// </summary>
        ///// <param name="_emotion・_顔表情の感情"></param>
        ///// <returns></returns>
        //public void changeFace・顔表情変える(CFaceState・顔表情の状態 _state・_状態)
        //{

        //}
        ///// <summary>
        ///// キャラの顔表情を，顔，目，口，その他をそれぞれ変更します．※まだ未完成！
        ///// </summary>
        ///// <param name="_face・_顔"></param>
        ///// <param name="_eye・_目"></param>
        ///// <param name="_mouse・_口"></param>
        ///// <param name="_other・_その他"></param>
        ///// <returns></returns>
        ////public bool changeFaceInDetail・顔表情を詳細に変える(GFaceType・顔タイプ _face・_顔, GEyeType・目タイプ _eye・_目, GMouseType・口タイプ _mouse・_口, GFaceOtherType・顔その他タイプ _other・_その他)
        ////{
        ////}

        ///// <summary>
        ///// ※まだ未完成！
        ///// </summary>
        //public void becomeTranse・透明になる()
        //{
            
        //}
        ///// <summary>
        ///// ※まだ未完成！
        ///// </summary>
        ///// <param name="_brightColor・_光る色"></param>
        ///// <param name="_colorParsent・_光る度合いパーセント"></param>
        //public void becomeBright・光る(Color _brightColor・_光る色, float _colorParsent・_光る度合いパーセント)
        //{

        //}
        ///// <summary>
        ///// ※まだ未完成！
        ///// </summary>
        //public void becomeOrdinal・画像効果を元に戻す()
        //{

        //}




        #endregion
    }
}
