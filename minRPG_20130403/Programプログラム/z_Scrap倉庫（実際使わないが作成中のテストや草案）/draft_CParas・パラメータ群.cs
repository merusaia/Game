using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    
    /// <summary>
    /// 各キャラが持つ，整数か小数点double型で表わされるパラメータ群です．
    /// 基本的にパラメータは全てdouble型なので，文字列string型やbool型が入る可能性のある変数は，CVar・変数というクラスかやCSwitch・スイッチを使って定義してください．
    /// 
    /// ※日本語クラスや日本語メソッドというものは，名前を省略する目的のためだけに作られる日本語名クラス・メソッドのため，特別な理由がない限り，クラスやメソッドの内容を変更しないことが望ましいです．
    /// </summary>
    /// <remarks>
    /// 「獲得経験値」「総敵討伐数」なども，このキャラ毎のパラメータとして考える．全キャラの値を出すときは，主人公のものを使うか，全キャラのものを足す．
    /// </remarks>
    public class draft_CParas・パラメータ群
    {
        /// <summary>
        /// 力，攻撃力，体力，感情値などのIDで検索可能な，パラメータ名と値．
        /// </summary>
        List<double> p_parameter・パラメータ一覧 = new List<double>();

        /// <summary>
        /// コンストラクタです．
        /// </summary>
        public draft_CParas・パラメータ群()
        {
            List<int> _enumArray = MyTools.getEnumValueList<EPara>();
            // 全てのパラメータをリスト追加して初期化
            foreach (int _oneEnum in _enumArray)
            {
                p_parameter・パラメータ一覧.Add(0.0);
            }
        }
        #region get/setアクセサ
        /// <summary>
        /// 指定のIDのパラメータを代入します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public void setPara・パラを変更(EPara _parameterID・パラメータID, double _newValue)
        {
            //double _oldValue = MyTools.getListValue(p_parameters・パラメータ一覧, (int)_parameterID・パラメータID);
            //if (_oldValue != 0.0)

            if (p_parameter・パラメータ一覧.Count > (int)_parameterID・パラメータID)// enumは0から
            {
                // そのパラメータが定義されていれば（サイズが確保されていれば），範囲内に調整して代入
                MyTools.adjustValue_From_Min_To_Max(_newValue, int.MinValue, int.MaxValue);
            }
        }

        /// <summary>
        /// 指定のIDのパラメータを取得します．
        /// </summary>
        /// <param name="_parameterID・パラメータID"></param>
        /// <returns></returns>
        public double getPara・パラ(EPara _parameterID)
        {
            return MyTools.getListValue(p_parameter・パラメータ一覧, (int)_parameterID);
        }
        #endregion

        /*/// <summary>
        /// ※このメソッドはパラメータは使っても良い？
        /// 現段階は，Calc・計算はprivateで外部からはアクセス不可
        /// </summary>
        private void calcBattlePara・戦闘パラメータを計算()
        {
            CBattleParaCalulator・戦闘パラメータ計算機.Calc・計算(p_parameters・パラメータ一覧);
        }*/


        #region パラメータ情報（※パラメータIDに変更がある場合は，ここを変更してください）
        public readonly int shintaiparano身体パラメータの個数 = 6;
        public readonly int seisinparano精神パラメータの個数 = 6;
        public readonly int sentouparano戦闘パラメータの個数 = 21;

        /// <summary>
        /// 身体6パラメータ：　ちから,感性,素早さ,器用さ,行動力,体力,精神力,を配列で取得します．
        /// </summary>
        /// <returns></returns>
        public List<double> getShintaiParas・身体パラメータ()
        {
            List<double> _ShintaiPara = new List<double>(new double[] { getPara・パラ(EPara.a1_ちから), getPara・パラ(EPara.a6_賢さ), getPara・パラ(EPara.a4_素早さ), getPara・パラ(EPara.b1_器用さ), getPara・パラ(EPara.a2_持久力), getPara・パラ(EPara.a5_精神力) });
            return _ShintaiPara;
        }
        /// <summary>
        /// 身体パラメータを代入します．
        /// </summary>
        /// <param name="_iro6・基本色６パラ"></param>
        public void setShintaiParas・身体パラメータをセット(List<double> _normalParas)
        {
            setPara・パラを変更(EPara.a1_ちから, _normalParas[0]);
            setPara・パラを変更(EPara.a6_賢さ, _normalParas[1]);
            setPara・パラを変更(EPara.a4_素早さ, _normalParas[2]);
            setPara・パラを変更(EPara.b1_器用さ, _normalParas[3]);
            setPara・パラを変更(EPara.a2_持久力, _normalParas[4]);
            setPara・パラを変更(EPara.a5_精神力, _normalParas[5]);
        }
        
        /// <summary>
        /// 精神パラメータ：　行動力，思考力，適応力，集中力，忍耐力，健康管理力，を配列で取得します．
        /// </summary>
        /// <returns></returns>
        public List<double> getSeishinParas・精神パラメータ()
        {
            List<double> _SeishinPara = new List<double>(new double[] { getPara・パラ(EPara.a3_行動力), getPara・パラ(EPara.b6_思考力), getPara・パラ(EPara.b4_適応力), getPara・パラ(EPara.b5_集中力), getPara・パラ(EPara.b2_忍耐力), getPara・パラ(EPara.b3_健康力) });
            return _SeishinPara;
        }
        /// <summary>
        /// 精神パラメータを代入します．
        /// </summary>
        /// <param name="_iro6・基本色６パラ"></param>
        public void setSeishinParas・精神パラメータをセット(List<double> _mindParas)
        {
            setPara・パラを変更(EPara.a3_行動力, _mindParas[0]);
            setPara・パラを変更(EPara.b6_思考力, _mindParas[1]);
            setPara・パラを変更(EPara.b4_適応力, _mindParas[2]);
            setPara・パラを変更(EPara.b5_集中力, _mindParas[3]);
            setPara・パラを変更(EPara.b2_忍耐力, _mindParas[4]);
            setPara・パラを変更(EPara.b3_健康力, _mindParas[5]);
        }

        /// <summary>
        /// 戦闘パラメータを配列で取得します．
        /// </summary>
        /// <returns></returns>
        public List<double> getSentouParas・戦闘パラメータ()
        {
            List<double> _BattlePara = new List<double>(new double[] { getPara・パラ(EPara.LV), getPara・パラ(EPara.LVExpSum_経験値), getPara・パラ(EPara.s03_HP), getPara・パラ(EPara.s04_SP), getPara・パラ(EPara.s03b_最大HP), getPara・パラ(EPara.s04b_最大SP), getPara・パラ(EPara.s07_攻撃力), getPara・パラ(EPara.s08_魔法力), getPara・パラ(EPara.s09_守備力), getPara・パラ(EPara.s10_魔法防御), getPara・パラ(EPara.s12_クリティカル率), getPara・パラ(EPara.s11_回避率), getPara・パラ(EPara.s13b_ガード率), getPara・パラ(EPara.s20_AP), getPara・パラ(EPara.s20b_最大AP), getPara・パラ(EPara.s20c_AP回復量), getPara・パラ(EPara.s25_テンション), getPara・パラ(EPara.s25b_最大テンション), getPara・パラ(EPara.s10_速度), getPara・パラ(EPara.s20_魔法速度), getPara・パラ(EPara.s21_つば迫り合い率) });
            return _BattlePara;
        }
        /// <summary>
        /// 戦闘パラメータを代入します．
        /// </summary>
        /// <param name="_iro6・基本色６パラ"></param>
        public void setSentouParas・戦闘パラメータをセット(List<double> _BattleParas)
        {
            setPara・パラを変更(EPara.LV, _BattleParas[0]);
            setPara・パラを変更(EPara.LVExpSum_経験値, _BattleParas[1]);
            setPara・パラを変更(EPara.s03_HP, _BattleParas[2]);
            setPara・パラを変更(EPara.s04_SP, _BattleParas[3]);
            setPara・パラを変更(EPara.s03b_最大HP, _BattleParas[4]);
            setPara・パラを変更(EPara.s04b_最大SP, _BattleParas[5]);
            setPara・パラを変更(EPara.s07_攻撃力, _BattleParas[6]);
            setPara・パラを変更(EPara.s08_魔法力, _BattleParas[7]);
            setPara・パラを変更(EPara.s09_守備力, _BattleParas[8]);
            setPara・パラを変更(EPara.s10_魔法防御, _BattleParas[9]);
            setPara・パラを変更(EPara.s12_クリティカル率, _BattleParas[10]);
            setPara・パラを変更(EPara.s11_回避率, _BattleParas[11]);
            setPara・パラを変更(EPara.s13b_ガード率, _BattleParas[12]);
            setPara・パラを変更(EPara.s20_AP, _BattleParas[13]);
            setPara・パラを変更(EPara.s20b_最大AP, _BattleParas[14]);
            setPara・パラを変更(EPara.s20c_AP回復量, _BattleParas[15]);
            setPara・パラを変更(EPara.s25_テンション, _BattleParas[16]);
            setPara・パラを変更(EPara.s25b_最大テンション, _BattleParas[17]);
            setPara・パラを変更(EPara.s10_速度, _BattleParas[18]);
            setPara・パラを変更(EPara.s20_魔法速度, _BattleParas[19]);
            setPara・パラを変更(EPara.s21_つば迫り合い率, _BattleParas[20]);
        }
        #endregion

    }



    /* ●○●○古いバージョンのEParaID（全パラメータIDを定義した列挙体）のコピーです．
     * 現行バージョンは，CParas・パラメータ群というクラスのEPara・全パラを使っています．
     * 
     * 
    /// <summary>
    /// 特定のパラメータを名前で参照するenumです．パラメータを取得する際はIDの数値を直接入れず，必ず，これを使用するか，日本語メソッド（ちから()など）を使用してください．
    /// </summary>
    /// <remarks>
    /// なお，ESPara・戦闘パラメータ，身体パラメータなどを，それぞれ個別のクラスに分けない理由は，
    /// ・今後パラメータ数を変更する時にクラス間の連携が複雑にならないようにするため
    /// ・一つのパラメータクラス（List string型）でシンプルに管理したい
    /// からです．
    /// </remarks>
    public enum EPara
    {

        // 戦闘21パラメータ：　LV, k経験値, HP, SP, 最大HP, 最大SP, 攻撃力, 魔法力, 守備力, 魔法防御, 命中率, 回避率,ガード率,行動ゲージ,最大行動ゲージ,行動回復量,テンション,最大テンション,攻撃速度,魔法速度,つば迫り合い率，
        #region ■■■ ESPara・戦闘パラメータ
        /// <summary>
        /// 基本6パラメータ＋能力補正で調整可能な，戦闘などによく使う具体的なパラメータ
        /// </summary>
        LV, LVExp_経験値, s03_HP, s04_SP, s03b_最大HP, s04b_最大SP, s07_攻撃力, s08_魔法力, s09_守備力, s10_魔法防御, s12_クリティカル率, s11_回避率,s13b_ガード率,
        /// <summary>
        /// 攻撃・魔法を連続行動できる行動力(ACT)の量と，1秒間（指定フレーム？）の回復量(ACT/s)です．
        /// </summary>
        s20_AP, s20b_最大AP, s16_行動回復量,
        /// <summary>
        /// （器量），戦闘の流れを左右する自己の雰囲気．ダメージ・クリティカルの増減率，回避・つば競り合い・援護・根性の確率の増減率に影響する．
        /// </summary>
        s25_テンション, s25b_最大テンション,
        /// <summary>
        /// 攻撃・魔法の速度を通常100％として，どの程度の速さを持つか(％)です．
        /// </summary>
        s10_速度, s20_魔法速度,
        /// <summary>
        /// 相手の攻撃を受け止めてつばぜり合いをする確率です．
        /// </summary>
        s21_つば迫り合い率,
        #endregion
        // ※戦闘パラメータとは，基本的に戦闘でよく使われる具体的なパラメータ群


        // 身体6パラメータ：　ちから,感性,素早さ,器用さ,行動力,持久力,精神力
        #region ○○○標準（身体）パラ
        /// <summary>
        /// 攻撃力，守備力（弱），つば競り合い勝利率に影響
        /// </summary>
        a1_ちから,
        /// <summary>
        /// 魔法攻撃力，魔法防御（弱），精神対立勝利率に影響
        /// </summary>
        a6_賢さ,
        /// <summary>
        /// 行動回復量，攻撃速度・魔法速度，回避率に影響
        /// </summary>
        a4_素早さ,
        /// <summary>
        /// 命中率，ダメージの安定度，回避率（弱），コンボや連携攻撃でのタイミングの取りやすさ？に影響
        /// </summary>
        b1_器用さ,
        /// <summary>
        /// （体力，丈夫さ）・・・最大HPに影響
        /// </summary>
        a2_持久力,
        /// <summary>
        /// （知力，想像力，器量）・・・最大SPに影響
        /// </summary>
        a5_精神力,
        #endregion

        // 精神6パラメータ：　行動力，思考力，適応力，集中力，忍耐力，健康管理力
        #region ●●●　応用（精神）パラ　（青年期になってから？）
        /// <summary>
        /// 行動ゲージ（＝＞攻撃回数）に影響
        /// </summary>
        a3_行動力,
        /// <summary>
        /// （理解力，論理思考）敵の同じ攻撃・魔法を防ぐ確率Up，精神対立時の思考スピード，AI戦闘の賢さに影響
        /// </summary>
        b6_思考力,
        /// <summary>
        /// （柔軟性，協調性），味方と連携のしやすさ，相手の精神世界への適応確率，魔法防御力（弱），に影響
        /// </summary>
        b4_適応力,
        /// <summary>
        /// 決め技などの大事な一撃でのダメージUP量に影響
        /// </summary>
        b5_集中力,
        /// <summary>
        /// （タフさ）状態異常耐性，瀕死時の物理防御力・魔法防御力・回避率・受け止め率UP量，戦闘不能復帰率（根性）に影響
        /// </summary>
        b2_忍耐力,
        /// <summary>
        /// 自然HP回復量，自然SP回復量，自然状態変化回復速度に影響
        /// </summary>
        b3_健康力,

        #endregion

        #region その他の能力パラメータ
        /// <summary>
        /// クリティカル率，他の様々な中立ランダム要素に影響
        /// </summary>
        //運,
        /// <summary>
        /// （楽観性）笑いに影響，面白いイベントを頻繁に起こす，周囲をなごませる発言を頻繁にする
        /// </summary>
        ユーモア,
        /// <summary>
        /// 補助特技・連携をされやすい，敵が，戦闘のアピール度Up？（戦闘イベント発生率），恋愛イベント？に影響
        /// </summary>
        魅力,
        /// <summary>
        /// （愛），ドドメを刺す／刺される確率Down，，友好度Up？，信頼イベント？に影響
        /// </summary>
        優しさ,
        /// <summary>
        /// （体の柔らかさ-固さ）体調，良い感情の持続時間？，HPやSPの減り具合が，大きいと全快時に減りにくく／少ないとピンチ時に減りにくくなる，状態変化耐性（弱）に影響
        /// </summary>
        身体柔軟性,
        /// <summary>
        /// （発想力），最大SPに影響，新しい技を覚える速度・確率？，に影響
        /// </summary>
        想像力,
        /// <summary>
        /// （感情移入度，）喜怒哀楽の感情の変化が激しい，感情による能力・行動補正増減Up，に影響
        /// </summary>
        情緒安定性,
        /// <summary>
        /// 最大テンション，潜在能力の解放，覚醒に影響
        /// </summary>
        自己覚醒度,

        /// <summary>
        /// 物理防御力，物理ガード率（受け身・反撃率），その他攻撃受身系イベント発動率に影響
        /// </summary>
        護身術,
        /// <summary>
        /// 魔法防御力，魔法ガード率（吸収・反射率）に影響，その他魔法攻撃受身系イベント発動率に影響
        /// </summary>
        魔制術,

        //他案，候補：力, 身の守り, HP, SP, MP, AP, 素早さ, 回避, 命中, 運, 感情, 光影
        #endregion

        // ■■■ その他のプロフィール，属性
        // 年齢，性別，出身地，得意なもの，好き・嫌いなどは特徴に入れる！

    }
     * */
    // 英語版
    /*public enum EPara
    {
        Attack, Difence, HP, SP, MP, AP, Speed, Dex, Hit, Luck, emotion, HolyAndDark

    }*/

}

