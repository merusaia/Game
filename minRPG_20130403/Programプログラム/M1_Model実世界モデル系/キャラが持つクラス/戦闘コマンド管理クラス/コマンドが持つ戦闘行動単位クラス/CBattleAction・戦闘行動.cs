using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    #region 戦闘で使う列挙体 EBattleActionType・行動タイプ、EBattleActionObject・攻撃対象、EAddEffect・追加効果など
    // EnumにFlag属性（複数のフラグのチェックボタンとして機能する）を付ける　http://www.atmarkit.co.jp/fdotnet/dotnettips/1052enumflags/enumflags.html
    /// <summary>
    /// その戦闘行動のタイプ（攻撃系か，回復系か，パラメータ増減系か，それ以外の補助系か，防御系かなど）を定義します．
    /// </summary>
    [Flags] // 複数の要素のオン／オフを格納可能にする
    public enum EBattleActionType・行動タイプ
    {
        /// <summary>
        /// ＨＰ（ヒットポイント、生命力）にダメージ
        /// </summary>
        t01_ＨＰダメ,
        t01b_ＨＰ回復,
        /// <summary>
        /// ＳＰ（スピリチュアルポイント、精神）にダメージ
        /// </summary>
        t02_精神ダメ,
        t02b_精神回復,
        /// <summary>
        /// 必ずミスする行動（ミスの効果音が鳴る）時に割り当てる処理
        /// </summary>
        t03_ミス,
        /// <summary>
        /// 防御行動に割り当てる処理
        /// </summary>
        t04_防御,
        /// <summary>
        /// 回避行動に割り当てる処理
        /// </summary>
        t05_回避,

        /// <summary>
        /// キャラの全パラメータ増加・減少・指定倍（乗算や徐算）、回復量の増減／乗除以外の、補助効果のある行動に割り当てる処理
        /// </summary>
        t11_補助その他,
        t11a_全パラ増,
        t11b_全パラ減,
        t11b_全パラ倍,
        t12a_回復増減,
        t12b_回復乗除,

        /// <summary>
        /// その他、判別が不明な行動に割り当てる処理
        /// </summary>
        t07_その他,



    }

    /// <summary>
    /// その戦闘行動を起こす対象（敵単体か，敵全員か，自分自身か，ランダム無差別か，）などを定義します．
    /// </summary>
    public enum EBattleActionObject・攻撃対象
    {
        t01_敵単,
        t02_敵全,
        t03_自分,
        t04_味単,
        t05_味全,
        /// <summary>
        /// 敵味方問わず、無差別にランダム
        /// </summary>
        t06_不明,
        /// <summary>
        /// 敵ランダム
        /// </summary>
        t06b_敵ラ,
        /// <summary>
        /// 味方ランダム
        /// </summary>
        t06c_味ラ,
        t07_他,
    }

    /// <summary>
    /// その戦闘行動を起こした時の追加効果です。
    /// </summary>
    [Flags] // 複数の要素のオン／オフを格納可能にする
    public enum EAddEffect・追加効果
    {
        _none・無し,
        addAllStatus・全状態異常付加,
        addPoison・毒,
        addPoison2・猛毒,
        addParalysis・マヒ,
        addParalysis2・金縛,
        addConfusion・混乱,
        addSeal・封印,
        addDebility・衰弱,
        addCurse・呪い,
        addFast・倍足,
        addSlow・鈍足,
        cureAllStatus・全状態異常解除,
        curePoison・毒解除,
        curePoison2・猛毒解除,
        cureParalysis・マヒ解除,
        //...
        paraUPAttack05・攻撃力半減,
        paraUPAttack06・攻撃力０点６倍,
        paraUPAttack07・攻撃力０点７倍,
        paraUPAttack08・攻撃力０点８倍,
        paraUPAttack09・攻撃力０点９倍,
        paraUPAttack10・攻撃力補正解除,
        paraUPAttack11・攻撃力１点１倍,
        paraUPAttack12・攻撃力１点２倍,
        paraUPAttack13・攻撃力１点３倍,
        paraUPAttack14・攻撃力１点４倍,
        paraUPAttack15・攻撃力１点５倍,
        paraUPAttack20・攻撃力２倍,

    }

    /// <summary>
    /// ゲームで認識可能な特技一覧を指定可能な、列挙体です。
    /// </summary>
    public enum ESkill・特技
    {
        _none・無し,
        // 以下、テスト
        attack1_弱攻撃,
        attack2_中攻撃,
        attack3_強攻撃,
        heal1_弱回復,
        heal2_中回復,
        heal3_回復,
    }
    #endregion

    #region CBattleAction・戦闘行動　クラス
    /// <summary>
    /// 与えるダメージや攻撃対象などの情報を管理する、戦闘行動（アクション）の定義に必要なクラス。
    /// 
    /// １マスのダイスコマンドは，連続攻撃などを可能にするため，１～複数の戦闘行動（アクション）で構成される．
    /// 以下，ダイスコマンドを構成する，１つの戦闘行動（アクション）のプロパティ
    /// 
    /// </summary>
    public class CBattleAction・戦闘行動
    {
        // 以下，複数の戦闘行動で１つのダイスコマンドを構成する，１つの戦闘行動（アクション）のプロパティ

        string p0_アクションテキスト = "";
        /// <summary>
        /// この行動の説明が書かれた，アクションテキストを取得します．
        /// </summary>
        /// <returns></returns>
        public string actionText() { return p0_アクションテキスト; }

        /// <summary>
        /// コマンド名を行動した時にある条件で（確率や、高速戦闘などで省略されない限り）表示される、特別メッセージです。
        /// １マスのコマンドでのアクション回数毎に表示されるメッセージをそれぞれ個別にランダムに変えられるよう、リストのリストにしています。
        /// なお、addでリストを指定した場合は、リストの中からランダムで一つが表示されます。
        /// 
        /// 　【未記入例】：Count=0だと、「[キャラ名]の[コマンド名]！」と表示されますが、それ以外のメッセージを表示したい時に使用します。
        /// 　
        /// 　【記入例】：例えば、"[キャラ名]はしずかに[コマンド名]を始めた…"だと、「○○○はしずかに瞑想を始めた…」にしたり。
        /// "何者かの鋭い一閃が相手を襲う！"といった、キャラ名やコマンド名を表示しないことも可能です。
        /// </summary>
        List<string> p1_特別メッセージ = new List<string>();
        public List<string> getP1_特別メッセージ() { return p1_特別メッセージ; }
        EBattleActionType・行動タイプ p2_行動タイプ = EBattleActionType・行動タイプ.t01_ＨＰダメ;
        public EBattleActionType・行動タイプ getP2_行動タイプ() { return p2_行動タイプ; }
        EBattleActionObject・攻撃対象 p3_行動対象 = EBattleActionObject・攻撃対象.t01_敵単;
        public EBattleActionObject・攻撃対象 getP3_行動対象() { return p3_行動対象; }
        double p4_ダメージ数 = 0.0;
        public double getP4_ダメージ数() { return p4_ダメージ数; }
        double p4b_精神ダメージ数 = 0.0;
        public double getP4b_精神ダメージ数() { return p4b_精神ダメージ数; }
        double p5_クリティカル率 = 0.0;
        public double getP5_クリティカル率() { return p5_クリティカル率; }
        double p5a_命中率 = 0.0;
        public double get5a_命中率() { return p5a_命中率; }
        double p6_回避率 = 0.0;
        public double getP6_回避率() { return p6_回避率; }
        double p7_防御軽減ダメージ数 = 0.0;
        public double getP7_防御軽減ダメージ数() { return p7_防御軽減ダメージ数; }
        ERank・ランク p1c_タイミング難易度 = 0;
        public ERank・ランク getp1c_タイミング難易度() { return p1c_タイミング難易度; }
        /// <summary>
        /// リストを指定すると、ランダムに１つ選びます。
        /// </summary>
        List<ESE・効果音> p1d_効果音 = new List<ESE・効果音>();
        public List<ESE・効果音> getp1d_効果音() { return p1d_効果音; }
        int p1b_待ち時間ミリ秒 = 0;
        public int getp1b_待ち時間ミリ秒() { return p1b_待ち時間ミリ秒; }
        /// <summary>
        /// 毒、マヒ、属性ダメージ、などの追加効果（特殊効果）です。リストで複数指定できます。
        /// </summary>
        List<EAddEffect・追加効果> p4c_追加効果 = new List<EAddEffect・追加効果>();
        public List<EAddEffect・追加効果> getP4c_追加効果() { return p4c_追加効果; }
        /// <summary>
        /// このアクションが参照する特技です。ない場合はnullを指定してください。
        /// 参照特技を指定すると、p4～ダメージ数の初期値は1.0になり、参照特技の指定倍数で調整することが出来ます（例：ダメージ数=2.0を入れると、参照特技の2倍）
        /// </summary>
        ESkill・特技 p3b_参照特技 = ESkill・特技._none・無し;
        public ESkill・特技 getp3b_特技効果() { return p3b_参照特技; }

        /// <summary>
        /// ダイスコマンドを1～複数で構成する，ダイスアクション（1回の行動単位）を作成します．
        /// </summary>
        /// <param name="_特別メッセージ">特別メッセージ：　※攻撃や防御などは「～の攻撃！」などは自動的に表示されますので，デフォルト""のままでOKです．
        /// 特に，回復・必殺などで，
        /// ・「【自キャラ】の回復！」などの後に，『【自キャラ】は深く集中して気を送り込んだ…．\nなんと，【行動対象キャラ】のHPがみるみる回復していく！！』
        /// ・「【自キャラ】の必殺！」後，「【自キャラの】」『【自キャラ】は大きく息を吸い込み，灼熱の炎を吐きだした！！』などの技毎にユニークなメッセージを表示したい場合に入力してください．
        /// 
        /// なお，行動中のキャラは【自キャラ】，指定キャラは【行動対象キャラ】（一人のみ可）と書くと，自動的に名前が表示されます．</param>
        /// <param name="_行動タイプ">行動タイプ：　このアクションが「攻撃」なのか，「回復」なのかなどを指します．</param>
        /// <param name="_行動対象">行動対象：　このアクションの対象が「敵単体」なのか，「味方全員」なのかなどを指します．</param>
        /// <param name="_ダメージ数">ダメージ数：　このアクションの強さを表す数値です．ダメージ数には，回復量にも，ダメージ（攻撃力）増減値なども意味します．</param>
        /// <param name="_クリ率">クリティカル率(0～100％)：　このアクションが何％の確率でクリティカルヒット（1.5倍くらい？）するかを指します．</param>
        /// <param name="回避率">回避率(0～100％)：　このアクションが次の敵の攻撃を何％の確率で避けるか（ダメージを0にする？，もしくはちょっと食らう？）を指します．</param>
        /// <param name="防御軽減ダメージ数">防御軽減ダメージ数：　このアクションが次の敵の攻撃のダメージをいくら軽減するのかを指します．</param>
        public CBattleAction・戦闘行動(List<string> _特別メッセージ, EBattleActionType・行動タイプ _行動タイプ, EBattleActionObject・攻撃対象 _行動対象, double _ダメージ数, double _クリティカル率, double _回避率, double _防御軽減ダメージ数)
        {
            p1_特別メッセージ = _特別メッセージ;
            p2_行動タイプ = _行動タイプ;
            p3_行動対象 = _行動対象;
            p4_ダメージ数 = _ダメージ数;
            p5_クリティカル率 = _クリティカル率;
            p6_回避率 = _回避率;
            p7_防御軽減ダメージ数 = _防御軽減ダメージ数;

            // 自動的にアクションテキストを作成
            createActionText・アクションテキストを設定();

        }
        /// <summary>
        /// １つのダイスアクションを説明するアクションテキスト（例：「攻撃・敵単体  50（防御  20 回避10％ クリ 5％）」）などを文字列で作成します．
        /// </summary>
        /// <returns></returns>
        private void createActionText・アクションテキストを設定(){
            string _actionText = "";
            //string s_tab = "\_nowTime"; // タブ文字列

            // (1)行動タイプ
            _actionText += ""; // MyTools.getEnumKeyName_InDetail(getP2_行動タイプ()) +"・";
            // (2)行動対象
            _actionText += MyTools.getEnumKeyName_OnlyJapanese(getP3_行動対象());
            // (3)ダメージ数
            _actionText += MyTools.getStringNumber(getP4_ダメージ数(), true, 5, 0, 5);
            // (4)命中
            _actionText += " ";
            _actionText += "命" + MyTools.getStringNumber(get5a_命中率(), true, 2, 0, 2) + "％";
            // (5)防御
            _actionText += "（";
            _actionText += "防" + MyTools.getStringNumber(getP7_防御軽減ダメージ数(), true, 5, 0, 5);
            // (6)回避
            _actionText += " ";
            _actionText += "回" + MyTools.getStringNumber(getP6_回避率(), true, 2, 0, 2) + "％";
            // (7)クリ率
            _actionText += " ";
            _actionText += "ク" + MyTools.getStringNumber(getP5_クリティカル率(), true, 2, 0, 2) + "％";
            _actionText += "）";

            p0_アクションテキスト = _actionText;
        }

    }
    #endregion

}
