using System;
using System.Collections.Generic;
using System.Text;
//using Yanesdk;
using System.Drawing;
using PublicDomain;

namespace PublicDomain
{
    /*
    /// <summary>
    /// プレイヤーや敵キャラ，町の人やイベントキャラ，ペットなどを含めた，全てのキャラのベースクラスです．
    /// ※GCharacter・キャラクタークラスの名前を省略する目的のためだけに作られる日本語名クラスのため，特別な理由がない限り，新しいプロパティ・メソッドを追加しないことが望ましいです．
    /// </summary>
    public class Cキャラ : CChara・キャラ
    {

    }
     * */
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


    /// <summary>
    /// プレイヤーや敵キャラ，町の人やイベントキャラ，ペットなどを含めた，全てのキャラのベースクラスです．
    /// </summary>
    public class draft_CChara・キャラ
    {
        /// <summary>
        /// 力，体力，感情値などのint型値の，パラメータ名と値．
        /// </summary>
        draft_CParas・パラメータ群 p_parameter・パラメータ一覧 = new draft_CParas・パラメータ群();
        /// <summary>
        /// キャラクタのパラメータクラスを取得します．
        /// </summary>
        /// <returns></returns>
        public draft_CParas・パラメータ群 getP_Paras()
        {
            return p_parameter・パラメータ一覧;
        }
        /// <summary>
        /// キャラクタのパラメータクラスを変更します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <param name="_value・変更後の値"></param>
        public void setP_Parameter(draft_CParas・パラメータ群 _parameter)
        {
            p_parameter・パラメータ一覧 = _parameter;
        }
        /// <summary>
        /// 指定のIDのパラメータを代入します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public void setPara・パラを変更(EPara _parameterID・パラメータID, double _newValue・変更後の値)
        {
            getP_Paras().setPara・パラを変更(_parameterID・パラメータID, _newValue・変更後の値);
        }
        /// <summary>
        /// 指定のIDのパラメータを取得します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public double para・パラ(EPara _parameterID)
        {
            return getP_Paras().getPara・パラ(_parameterID);
        }
        
        // 【廃止】パラメータクラスを簡単にを呼び出す(get)日本語メソッド
        // ※※※わざわざ新しいパラメータに日本語メソッド（クラス，値ともに）作るのは面倒くさいし，
        //       はじめの英語が汚いし，
        //       それならむしろ，Para(パラ名)／setParaValue(パラ名, ...)でやる方がリストが綺麗で賢いので，
        //       パラメータの日本語メソッドは廃止！
        // 
        // public CPara・パラメータ c01_赤()
        // { return Paras・パラメータ一括処理().getPara・パラ(EPara.c01_赤); }
        // public double c01_赤()
        // { return Para(EPara.c01_赤)}

        /*
        #region 色パラメータの日本語メソッド 
        public CPara・パラメータ c02_赤橙()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c02_赤橙); }
        public CPara・パラメータ c03_橙()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c03_橙); }
        public CPara・パラメータ c04_黄橙()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c04_黄橙); }
        public CPara・パラメータ c05_黄()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c05_黄); }
        public CPara・パラメータ c06_黄緑()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c06_黄緑); }
        public CPara・パラメータ c07_緑()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c07_緑); }
        public CPara・パラメータ c08_青緑()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c08_青緑); }
        public CPara・パラメータ c09_青()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c09_青); }
        public CPara・パラメータ c10_青紫()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c10_青紫); }
        public CPara・パラメータ c11_紫()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c11_紫); }
        public CPara・パラメータ c12_赤紫()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c12_赤紫); }
        public CPara・パラメータ c13_白()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c13_白); }
        public CPara・パラメータ c14_黒()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c14_黒); }
        public CPara・パラメータ c15_銀()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c15_銀); }
        public CPara・パラメータ c16_金()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c16_金); }
        public CPara・パラメータ c17_透明()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c17_透明); }
        public CPara・パラメータ c18_虹色()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.c18_虹色); }
        #endregion

        #region ShintaiPara: ちから，素早さなどの身体パラメータクラスを呼び出せる日本語メソッド（[Tips]プロパティはまた記憶領域への格納と更新が必要になるのでダメ！）．
        //    public double ちから = 0.0;
        //    ちから = MyTools.getListValue(p_parameters・パラメータ一覧, (int)EPara.ちから);
        //    感性 = MyTools.getListValue(p_parameters・パラメータ一覧, (int)EPara.感性);
        
        public CPara・パラメータ a01_ちから()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.a1_ちから); }
        public CPara・パラメータ a02_持久力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.a2_持久力); }
        public CPara・パラメータ a03_行動力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.a3_行動力); }
        public CPara・パラメータ a04_素早さ()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.a4_素早さ); }
        public CPara・パラメータ a05_精神力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.a5_精神力); }
        public CPara・パラメータ a06_賢さ()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.a6_賢さ); }
        #endregion

        #region SeishinPara：　器用さ，思考力などの精神パラメータの日本語メソッド
        public CPara・パラメータ b01_器用さ()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.b1_器用さ); }
        public CPara・パラメータ b02_忍耐力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.b2_忍耐力); }
        public CPara・パラメータ b03_健康管理力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.b3_健康力); }
        public CPara・パラメータ b04_適応力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.b4_適応力); }
        public CPara・パラメータ b05_集中力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.b5_集中力); }
        public CPara・パラメータ b06_思考力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.b6_思考力); }
        #endregion

        #region BattlePara：　HPなどの戦闘パラメータの日本語メソッド
        public CPara・パラメータ LV()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.LV); }
        public CPara・パラメータ LVExp_経験値()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.LVExp_経験値); }
        public CPara・パラメータ s03_HP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s03_HP); }
        public CPara・パラメータ s04_SP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s04_SP); }
        public CPara・パラメータ s03b_最大HP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s03b_最大HP); }
        public CPara・パラメータ s04b_最大SP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s04b_最大SP); }
        public CPara・パラメータ s07_攻撃力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s07_攻撃力); }
        public CPara・パラメータ s08_魔法力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s08_魔法力); }
        public CPara・パラメータ s09_守備力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s09_守備力); }
        public CPara・パラメータ s10_魔法防御()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s10_魔法防御); }
        public CPara・パラメータ s13_命中率()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s12_クリティカル率); }
        public CPara・パラメータ s11_回避率()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s11_回避率); }
        public CPara・パラメータ s13b_ガード率()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s13b_ガード率); }
        //      public CPara・パラメータ s14_反撃率()
        //      { return Paras・パラメータ一括処理().getPara・パラ(EPara.s14_反撃率); }
        public CPara・パラメータ s20_AP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s20_AP); }
        public CPara・パラメータ s20b_最大AP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s20b_最大AP); }
        public CPara・パラメータ s16_行動回復量()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s16_行動回復量); }
        public CPara・パラメータ s25_テンション()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s25_テンション); }
        public CPara・パラメータ s25b_最大テンション()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s25b_最大テンション); }
        public CPara・パラメータ s10_速度()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s10_速度); }
        public CPara・パラメータ s20_魔法速度()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s20_魔法速度); }
        public CPara・パラメータ s21_つば迫り合い率()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s21_つば迫り合い率); }

        #endregion

        // ○パラメータの実際の値を簡単に取得（get）する日本語メソッド・・・あくまで草案．
        #region ShintaiPara: ちから，体力などの身体パラメータを直接呼び出せるようにする日本語メソッド（[Tips]プロパティはまた記憶領域への格納と更新が必要になるのでダメ！）．

        public double tiちから()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.a1_ちから); }
        public double zi持久力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.a2_持久力); }
        public double ko行動力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.a3_行動力); }
        public double su素早さ()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.a4_素早さ); }
        public double se精神力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.a5_精神力); }
        public double ka賢さ()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.a6_賢さ); }
        #endregion

        #region SeishinPara：　行動力，思考力などの精神パラメータの日本語メソッド
        public double ki器用さ()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.b1_器用さ); }
        public double ni忍耐力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.b2_忍耐力); }
        public double ke健康力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.b3_健康力); }
        public double te適応力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.b4_適応力); }
        public double sy集中力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.b5_集中力); }
        public double si思考力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.b6_思考力); }
        #endregion
     #region BattlePara：　HPなどの戦闘パラメータの日本語メソッド（プログラミングの効率化のためだけにつかうとしても，コードが汚くなるし，網羅性でミスがあるかも！　パラメータの日本語メソッド廃止！）
        /// <summary>
        /// キャラクタの基本的な戦闘パラメータを決めるLV．
        /// </summary>
        /// <returns></returns>
        public double LV()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.LV); }
        /// <summary>
        /// キャラクタのLVに関係する経験値
        /// </summary>
        /// <returns></returns>
        public double ke経験値()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.LVExp_経験値); }
        /// <summary>
        /// 0になると気絶する，ヒットポイント．
        /// </summary>
        /// <returns></returns>
        public double HP()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s03_HP); }
        /// <summary>
        /// 0になると，スピリットポイント（もしくはSeishinポイント）です．
        /// </summary>
        /// <returns></returns>
        public double SP()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s04_SP); }
        public double hp最大HP()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s03b_最大HP); }
        public double sp最大SP()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s04b_最大SP); }
        /// <summary>
        /// 物理的な攻撃の強さに影響する力です．
        /// </summary>
        /// <returns></returns>
        public double ko攻撃力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s07_攻撃力); }
        /// <summary>
        /// 精神的な魔法の強さに影響する力です．
        /// </summary>
        /// <returns></returns>
        public double ma魔法力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s08_魔法力); }
        /// <summary>
        /// 物理的な攻撃を受けた時のダメージを軽減する，身の守りです．
        /// </summary>
        /// <returns></returns>
        public double sy守備力()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s09_守備力); }
        /// <summary>
        /// 精神的な魔法を受けた時のダメージを軽減する，精神的強さです．
        /// </summary>
        /// <returns></returns>
        public double ma魔法防御()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s10_魔法防御); }
        /// <summary>
        /// 攻撃が当たる確率，ヒットダメージの安定性，敵の回避時の致命的ヒット率に影響
        /// </summary>
        /// <returns></returns>
        public double me命中率()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s12_クリティカル率); }
        /// <summary>
        /// 敵の攻撃をミスさせる確率，避けた時のダメージ軽減率（不安定），敵の連続攻撃から逃れる確率，つば競り合い時の回避率に影響
        /// </summary>
        /// <returns></returns>
        public double ka回避率()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s11_回避率); }
        /// <summary>
        /// 敵の攻撃を無効化する確率，ガード時のダメージ軽減率（安定），敵の連続攻撃から逃れる確率，つば競り合い時の回避率に影響
        /// </summary>
        /// <returns></returns>
        public double gaガード率()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s13b_ガード率); }
        /// <summary>
        /// 攻撃・魔法を連続行動できる行動力(ACT)の量です．
        /// </summary>
        public double ko行動ゲージ()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s20_AP); }
        /// <summary>
        /// 攻撃・魔法を連続行動できる行動力(ACT)の量の最大値です．
        /// </summary>
        public double ko最大行動ゲージ()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s20b_最大AP); }
        /// <summary>
        /// 行動ゲージの1秒間（指定フレーム？）の回復量(ACT/s)です．
        /// </summary>
        public double ko行動回復量()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s16_行動回復量); }
        /// <summary>
        /// （器量），戦闘の流れを左右する自己の雰囲気．ダメージ・クリティカルの増減率，回避・つば競り合い・援護・根性の確率の増減率に影響する．
        /// </summary>
        public double teテンション()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s25_テンション); }
        /// <summary>
        /// 最大テンションを超えると，他者の能力を支配する（戦闘を牛耳る）「覚醒」となりやすい．
        /// </summary>
        /// <returns></returns>
        public double te最大テンション()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s25b_最大テンション); }
        /// <summary>
        /// 攻撃の速度を通常100％として，どの程度の速さを持つか(％)です．
        /// </summary>
        public double ko攻撃速度()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s10_速度); }
        /// <summary>
        /// 魔法の速度を通常100％として，どの程度の速さを持つか(％)です．
        /// </summary>
        /// <returns></returns>
        public double ma魔法速度()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s20_魔法速度); }
        /// <summary>
        /// 相手の攻撃を受け止めてつばぜり合いをする確率です．
        /// </summary>
        public double tuつば迫り合い率()
        { return Paras・パラメータ一括処理().getParaValue・パラの値取得(EPara.s21_つば迫り合い率); }
        #endregion
         * */
        
        // ●以下，もっと昔のパラメータ変更（set）まで作ってた・・・日本語メソッド
#region ShintaiPara: ちから，体力などの身体パラメータを直接呼び出せるようにする日本語メソッド（[Tips]プロパティはまた記憶領域への格納と更新が必要になるのでダメ！）．

        /// <summary>
        /// 攻撃力，守備力（弱），つば競り合い勝利率に影響
        /// </summary>
        public double tiちから()
        { return getP_Paras().getPara・パラ(EPara.a1_ちから); }
        public void addtiちから増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a1_ちから, tiちから() + (double)_増減値); }
        public void addtiちから増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a1_ちから, tiちから() + _増減値); }
        public void multiちから乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.a1_ちから, tiちから() * _乗除パーセント); }
        /// <summary>
        /// 魔法攻撃力，魔法防御（弱），精神対立勝利率に影響
        /// </summary>
        public double ka感性()
        { return getP_Paras().getPara・パラ(EPara.a6_賢さ); }
        public void addka感性増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a6_賢さ, ka感性() + (double)_増減値); }
        public void addka感性増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a6_賢さ, ka感性() + _増減値); }
        public void mulka感性乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.a6_賢さ, ka感性() * _乗除パーセント); }
        /// <summary>
        /// 行動回復量，攻撃速度・魔法速度，回避率に影響
        /// </summary>
        public double su素早さ()
        { return getP_Paras().getPara・パラ(EPara.a4_素早さ); }
        public void addsu素早さ増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a4_素早さ, su素早さ() + (double)_増減値); }
        public void addsu素早さ増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a4_素早さ, su素早さ() + _増減値); }
        public void mulsu素早さ乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.a4_素早さ, su素早さ() * _乗除パーセント); }
        /// <summary>
        /// 命中率，ダメージの安定度，回避率（弱），コンボや連携攻撃でのタイミングの取りやすさ（？）に影響
        /// </summary>
        public double ki器用さ()
        { return getP_Paras().getPara・パラ(EPara.b1_器用さ); }
        public void addki器用さ増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b1_器用さ, ki器用さ() + (double)_増減値); }
        public void addki器用さ増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b1_器用さ, ki器用さ() + _増減値); }
        public void mulki器用さ乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.b1_器用さ, ki器用さ() * _乗除パーセント); }
        /// <summary>
        /// （体力，丈夫さ）・・・最大HPに影響
        /// </summary>
        public double zi持久力()
        { return getP_Paras().getPara・パラ(EPara.a2_持久力); }
        public void addzi持久力増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a2_持久力, zi持久力() + (double)_増減値); }
        public void addzi持久力増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a2_持久力, zi持久力() + _増減値); }
        public void mulzi持久力乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.a2_持久力, zi持久力() * _乗除パーセント); }
        /// <summary>
        /// （知力，想像力，器量）・・・最大SPに影響
        /// </summary>
        public double se精神力()
        { return getP_Paras().getPara・パラ(EPara.a5_精神力); }
        public void addse精神力増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a5_精神力, se精神力() + (double)_増減値); }
        public void addse精神力増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a5_精神力, se精神力() + _増減値); }
        public void mulse精神力乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.a5_精神力, se精神力() * _乗除パーセント); }
        #endregion

        #region SeishinPara：　行動力，思考力などの精神パラメータの日本語メソッド
        /// <summary>
        /// 行動ゲージ（＝＞攻撃回数）に影響
        /// </summary>
        public double ko行動力()
        { return getP_Paras().getPara・パラ(EPara.a3_行動力); }
        public void addko行動力増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a3_行動力, ko行動力() + (double)_増減値); }
        public void addko行動力増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.a3_行動力, ko行動力() + _増減値); }
        public void mulko行動力乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.a3_行動力, ko行動力() * _乗除パーセント); }
        /// <summary>
        /// （理解力，論理思考）敵の同じ攻撃・魔法を防ぐ確率Up，精神対立時の思考スピード，AI戦闘の賢さに影響
        /// </summary>
        public double si思考力()
        { return getP_Paras().getPara・パラ(EPara.b6_思考力); }
        public void addsi思考力増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b6_思考力, si思考力() + (double)_増減値); }
        public void addsi思考力増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b6_思考力, si思考力() + _増減値); }
        public void mulsi思考力乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.b6_思考力, si思考力() * _乗除パーセント); }
        /// <summary>
        /// （柔軟性，協調性），味方と連携のしやすさ，相手の精神世界への適応確率，魔法防御力（弱），に影響
        /// </summary>
        public double te適応力()
        { return getP_Paras().getPara・パラ(EPara.b4_適応力); }
        public void addte適応力増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b4_適応力, te適応力() + (double)_増減値); }
        public void addte適応力増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b4_適応力, te適応力() + _増減値); }
        public void multe適応力乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.b4_適応力, te適応力() * _乗除パーセント); }
        /// <summary>
        /// 決め技などの大事な一撃でのダメージUP量に影響
        /// </summary>
        public double sy集中力()
        { return getP_Paras().getPara・パラ(EPara.b5_集中力); }
        public void addsy集中力増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b5_集中力, sy集中力() + (double)_増減値); }
        public void addsy集中力増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b5_集中力, sy集中力() + _増減値); }
        public void mulsy集中力乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.b5_集中力, sy集中力() * _乗除パーセント); }
        /// <summary>
        /// （タフさ）状態異常耐性，瀕死時の物理防御力・魔法防御力・回避率・受け止め率UP量，戦闘不能復帰率（根性）に影響
        /// </summary>
        public double ni忍耐力()
        { return getP_Paras().getPara・パラ(EPara.b2_忍耐力); }
        public void addni忍耐力増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b2_忍耐力, ni忍耐力() + (double)_増減値); }
        public void addni忍耐力増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b2_忍耐力, ni忍耐力() + _増減値); }
        public void mulni忍耐力乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.b2_忍耐力, ni忍耐力() * _乗除パーセント); }
        /// <summary>
        /// 自然HP回復量，自然SP回復量，自然状態変化回復速度に影響
        /// </summary>
        public double ke健康管理力()
        { return getP_Paras().getPara・パラ(EPara.b3_健康力); }
        public void addke健康管理力増減(int _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b3_健康力, ke健康管理力() + (double)_増減値); }
        public void addke健康管理力増減(double _増減値)
        { getP_Paras().setPara・パラを変更(EPara.b3_健康力, ke健康管理力() + _増減値); }
        public void muke健康管理力乗除(double _乗除パーセント)
        { getP_Paras().setPara・パラを変更(EPara.b3_健康力, ke健康管理力() * _乗除パーセント); }
        #endregion

        #region BattlePara：　HPなどの戦闘パラメータの日本語メソッド
        /*
        /// <summary>
        /// キャラクタの基本的な戦闘パラメータを決めるLV．
        /// </summary>
        /// <returns></returns>
        public double LV()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.LV); }
        public void addLV増減(int _増減値)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LV, LV() + (double)_増減値); }
        public void addLV増減(double _増減値)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LV, LV() + _増減値); }
        public void mulLV乗除(double _乗除パーセント)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LV, LV() * _乗除パーセント); }
        /// <summary>
        /// キャラクタのLVに関係する経験値
        /// </summary>
        /// <returns></returns>
        public double ke経験値()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.LVExp_経験値); }
        public void addke経験値増減(int _増減値)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LVExp_経験値, ke経験値() + (double)_増減値); }
        public void addke経験値増減(double _増減値)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LVExp_経験値, ke経験値() + _増減値); }
        public void mulke経験値乗除(double _乗除パーセント)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LVExp_経験値, ke経験値() * _乗除パーセント); }
        /// <summary>
        /// 0になると気絶する，ヒットポイント．
        /// </summary>
        /// <returns></returns>
        public double HP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s03_HP); }
        public void addHP(int _増減値)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LVExp_経験値, ke経験値() + (double)_増減値); }
        public void addke経験値(double _増減値)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LVExp_経験値, ke経験値() + _増減値); }
        public void mulke経験値(double _乗除パーセント)
        { Paras・パラメータ一括処理().setPara・パラを変更(EPara.LVExp_経験値, ke経験値() * _乗除パーセント); }
        /// <summary>
        /// 0になると，スピリットポイント（もしくはSeishinポイント）です．
        /// </summary>
        /// <returns></returns>
        public double SP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s04_SP); }
        public double hp最大HP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s03b_最大HP); }
        public double sp最大SP()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s04b_最大SP); }
        /// <summary>
        /// 物理的な攻撃の強さに影響する力です．
        /// </summary>
        /// <returns></returns>
        public double ko攻撃力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s07_攻撃力); }
        /// <summary>
        /// 精神的な魔法の強さに影響する力です．
        /// </summary>
        /// <returns></returns>
        public double ma魔法力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s08_魔法力); }
        /// <summary>
        /// 物理的な攻撃を受けた時のダメージを軽減する，身の守りです．
        /// </summary>
        /// <returns></returns>
        public double sy守備力()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s09_守備力); }
        /// <summary>
        /// 精神的な魔法を受けた時のダメージを軽減する，精神的強さです．
        /// </summary>
        /// <returns></returns>
        public double ma魔法防御()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s10_魔法防御); }
        /// <summary>
        /// 攻撃が当たる確率，ヒットダメージの安定性，敵の回避時の致命的ヒット率に影響
        /// </summary>
        /// <returns></returns>
        public double me命中率()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s13_命中率); }
        /// <summary>
        /// 敵の攻撃をミスさせる確率，避けた時のダメージ軽減率（不安定），敵の連続攻撃から逃れる確率，つば競り合い時の回避率に影響
        /// </summary>
        /// <returns></returns>
        public double ka回避率()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s11_回避率); }
        /// <summary>
        /// 敵の攻撃を無効化する確率，ガード時のダメージ軽減率（安定），敵の連続攻撃から逃れる確率，つば競り合い時の回避率に影響
        /// </summary>
        /// <returns></returns>
        public double gaガード率()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s13b_ガード率); }
        /// <summary>
        /// 攻撃・魔法を連続行動できる行動力(ACT)の量です．
        /// </summary>
        public double ko行動ゲージ()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s20_AP); }
        /// <summary>
        /// 行動ゲージの1秒間（指定フレーム？）の回復量(ACT/s)です．
        /// </summary>
        public double ko行動回復量()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s16_行動回復量); }
        /// <summary>
        /// （器量），戦闘の流れを左右する自己の雰囲気．ダメージ・クリティカルの増減率，回避・つば競り合い・援護・根性の確率の増減率に影響する．
        /// </summary>
        public double teテンション()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s25_テンション); }
        /// <summary>
        /// 最大テンションを超えると，他者の能力を支配する（戦闘を牛耳る）「覚醒」となりやすい．
        /// </summary>
        /// <returns></returns>
        public double te最大テンション()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s25b_最大テンション); }
        /// <summary>
        /// 攻撃の速度を通常100％として，どの程度の速さを持つか(％)です．
        /// </summary>
        public double ko攻撃速度()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s10_速度); }
        /// <summary>
        /// 魔法の速度を通常100％として，どの程度の速さを持つか(％)です．
        /// </summary>
        /// <returns></returns>
        public double ma魔法速度()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s20_魔法速度); }
        /// <summary>
        /// 相手の攻撃を受け止めてつばぜり合いをする確率です．
        /// </summary>
        public double tuつば迫り合い率()
        { return Paras・パラメータ一括処理().getPara・パラ(EPara.s21_つば迫り合い率); }
         * */
        #endregion
        

        /// <summary>
        /// 名前，称号，感情状態などのstring型値の，変数名と値（文字列）．
        /// </summary>
        CVars・変数一覧 p_var・特徴 = new CVars・変数一覧();
        /// <summary>
        /// キャラクタの特徴クラスを取得します．
        /// </summary>
        /// <returns></returns>
        public CVars・変数一覧 getP_var()
        {
            return p_var・特徴;
        }
        /// <summary>
        /// キャラクタの特徴クラスを変更します．
        /// </summary>
        /// <param name="_varName・変更する変数名"></param>
        /// <param name="_value・変更後の値"></param>
        public void setP_var(CVars・変数一覧 _var)
        {
            p_var・特徴 = _var;
        }
        #region 上記と機能同一メソッド
        // 特徴をDictionary<string, CVar・変数>で管理するとき用
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        public string setVar・変数を変更(EVar _EVar・変更する変数ID, string _value・変更後の値)
        {
            return getP_var().setVar・変数値を変更(_EVar・変更する変数ID, _value・変更後の値);
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        /// <param name="_変数名"></param>
        /// <param name="_varValue"></param>
        /// <returns></returns>
        public string setVar・変数を変更(EVar _EVar・変更する変数ID, CVarValue・変数値 _varValue・変更後の値)
        {
            return getP_var().setVar・変数値を変更(_EVar・変更する変数ID, _varValue・変更後の値.ToString());
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        /// <param name="_変数名"></param>
        /// <param name="_varValue"></param>
        /// <returns></returns>
        public string setVar・変数を変更(string _varName・変更する変数名, string _value・変更後の値)
        {
            return getP_var().setVar・変数値を変更(_varName・変更する変数名, _value・変更後の値);
        }
        /// <summary>
        /// 引数で指定した変数名の値を追加または変更します．返り値は，上書きされる前の以前設定されていた変数名の値（無い場合は""）です．
        /// </summary>
        /// <param name="_変数名"></param>
        /// <param name="_varValue"></param>
        /// <returns></returns>
        public string setVar・変数を変更(string _varName・変更する変数名, CVarValue・変数値 _varValue・変更後の値)
        {
            return getP_var().setVar・変数値を変更(_varName・変更する変数名, _varName・変更する変数名.ToString());
        }
        #endregion
        #region 名前，ニックネームなどの特徴量を直接呼び出せるようにする日本語メソッド（[Tips]プロパティを作る方法は．別の記憶領域への格納と更新が必要になるのでダメ！）．

        public string nam名前()
        { return getP_var().getVar・変数値(EVar.名前); }
        public string nickニックネーム()
        { return getP_var().getVar・変数値(EVar.称号); }
        public string sei性別()
        { return getP_var().getVar・変数値(EVar.性別); }
        public string nenre年齢()
        { return getP_var().getVar・変数値(EVar.年齢); }
        public string ketueki血液型()
        { return getP_var().getVar・変数値(EVar.血液型); }

        public string tait体調()
        { return getP_var().getVar・変数値(EVar.体調); }
        public string kanzyo感情()
        { return getP_var().getVar・変数値(EVar.感情); }
        public string kibu気分()
        { return getP_var().getVar・変数値(EVar.今の気分); }
        public string serihu登場セリフ()
        { return getP_var().getVar・変数値(EVar.登場セリフ); }
        //public string syougo称号()
        //{ return getP_var().getVar・変数値(EVar.称号); }

        public string konotaこのターンの作戦()
        { return getP_var().getVar・変数値(EVar.このターンの作戦); }

        public string sint身長()
        { return getP_var().getVar・変数値(EVar.身長); }
        public string taiz体重()
        { return getP_var().getVar・変数値(EVar.体重); }
        public string huku服のサイズ()
        { return getP_var().getVar・変数値(EVar.服のサイズ); }
        public string basuバスト()
        { return getP_var().getVar・変数値(EVar.バスト); }
        public string uestウエスト()
        { return getP_var().getVar・変数値(EVar.ウエスト); }
        public string hipヒップ()
        { return getP_var().getVar・変数値(EVar.ヒップ); }
        public string tait靴のサイズ()
        { return getP_var().getVar・変数値(EVar.靴のサイズ); }

        public string syumi趣味()
        { return getP_var().getVar・変数値(EVar.趣味); }
        public string sukina好きなもの()
        { return getP_var().getVar・変数値(EVar.好きなもの); }
        public string kirai嫌いなもの()
        { return getP_var().getVar・変数値(EVar.嫌いなもの); }
        public string koibi恋人の存在()
        { return getP_var().getVar・変数値(EVar.恋人の存在); }
        #endregion


        /// <summary>
        /// 普通・喜び・必殺などの顔画像や立ち画像の，種類名と画像．
        /// </summary>
        Dictionary<string, Image> p_image・画像一覧 = new Dictionary<string, Image>();
        /// <summary>
        /// 今の顔表情の状態
        /// </summary>
        CFaceState・顔表情の状態 p_nowFaceState・今の顔表情の状態 = new CFaceState・顔表情の状態(CFaceState・顔表情の状態.EFaceEmotion・顔表情感情.f0通常で);
        //GFaceType・顔表情 nowFaceType・顔表情 = new GFaceType・顔表情();

        CPositiveNegative・光陰 p_nowHeryDark・聖闇 = new CPositiveNegative・光陰();

        /// <summary>
        /// 今の感情（気分）．
        /// </summary>
        CEmotion・感情 p_nowEmotion・今の感情 = new CEmotion・感情();
        CCondition・状態変化 p_nowCondition・今の状態変化 = new CCondition・状態変化();

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


        #region コンストラクタ
        public draft_CChara・キャラ()
        {
        }
        #endregion

        #region ダイアログ内の通常行動

        /// <summary>
        /// ダイアログに引数(1)のテキストを表示します．
        /// </summary>
        /// <param name="_text・_話すテキスト"></param>
        /// <returns></returns>
        public void speak・話す(string _text・_話すテキスト)
        {
            // ある自作？ダイアログにテキストを表示
        }

        /// <summary>
        /// キャラの標準画像がスクリーンの引数(1)の位置に出現します．
        /// </summary>
        /// <param name="_shownPosition_Parcent・_出現位置_左0から右1への割合">0.0：左端-画像サイズ～1.0:右端-画像サイズ</param>
        /// <returns></returns>
        public void show・スクリーンに出現(float _shownPosition_Parcent・_出現位置_左0から右1への度合いパーセント)
        {
            // ある自作？ダイアログの上に画像を表示
        }
        //public bool show・スクリーンに出現(ScreenPosition _shownPosition・_出現位置, int _MSec・表示ミリ秒, bool _isFade・フェード有無)
        //[ToDo]:画面を横スライドするか，速度，フェードインアウトなどをいじれるようにしても良い
        /// <summary>
        /// キャラの標準画像がスクリーンの引数(1)の位置に出現します．
        /// </summary>
        /// <param name="_text・_話すテキスト"></param>
        /// <returns></returns>
        public void hide・スクリーンから消える()
        {

        }

        /// <summary>
        /// キャラの顔表情を，喜び・悲しみ・恐れ・怒り・嫌悪・笑・恥・涙などに変更します．
        /// </summary>
        /// <param name="_emotion・_顔表情の感情"></param>
        /// <returns></returns>
        public void changeFace・顔表情変える(CFaceState・顔表情の状態 _state・_状態)
        {

        }
        /// <summary>
        /// キャラの顔表情を，顔，目，口，その他をそれぞれ変更します．
        /// </summary>
        /// <param name="_face・_顔"></param>
        /// <param name="_eye・_目"></param>
        /// <param name="_mouse・_口"></param>
        /// <param name="_other・_その他"></param>
        /// <returns></returns>
        //public bool changeFaceInDetail・顔表情を詳細に変える(GFaceType・顔タイプ _face・_顔, GEyeType・目タイプ _eye・_目, GMouseType・口タイプ _mouse・_口, GFaceOtherType・顔その他タイプ _other・_その他)
        //{
        //}


        public void becomeTranse・透明になる()
        {

        }
        public void becomeBright・光る(Color _brightColor・_光る色, float _colorParsent・_光る度合いパーセント)
        {

        }
        public void becomeOrdinal・画像効果を元に戻す()
        {

        }




        #endregion
    }
}
