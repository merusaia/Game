using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 戦闘などに使われる，「たたかう」／「にげる」のコマンドリストや、
    /// 入れ子構造になった「たたかう」を選択→「こうげき」／「ぼうぎょ」／「アイテム」などを統括的に管理可能な、
    /// １つのコマンドを定義したクラスです．
    /// 使う時は基本的にList＜CCommand・コマンド＞で作成します。
    /// また、実際にコマンドの表示／非表示や、現在使用可能／不可能状況、復活するまでのターン数などを、いつでも変更することができます。
    /// 
    /// キャラ毎にユニークな識別子はコマンドIDです（コマンド名は重複あり）。
    /// １人のキャラが複数の種類のコマンド（戦闘コマンド、特技コマンドなど）を持つので、
    /// キャラクラスにはぞれぞれ別のリストを持たせてください。
    /// 
    /// ※なお、CBattleDice・ダイスコマンドは仕様が異なるため、別のクラスで定義してあります（継承もしていないので注意してください）。
    /// 
    /// なお、１つのコマンドには、複数のアクション（CBattleAction・戦闘行動クラス）を定義できます。
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CCommand・コマンド
    {
        /// <summary>
        /// これまで生成されたCCommand・コマンドクラスのインスタンス数を格納しています。
        /// ※コンストラクタが呼ばれるたびに+1されます。
        /// </summary>
        public static int s_idNum = 0;
        /// <summary>
        /// コマンドを区切る時に使う、タブを意味する文字列"\t"です。
        /// </summary>
        public static string s_tab = "\t"; // タブ文字列
        /// <summary>
        /// 戦闘用コマンドで戦うを意味するコマンド名（兼表示文字列）です。
        /// </summary>
        public static string s_CommandName_Fight・たたかう = "たたかう";
        /// <summary>
        /// 戦闘用コマンドでダイスコマンドを呼び出す時のコマンド名（兼表示文字列）です。
        /// </summary>
        public static string s_CommandName_Dice・ダイスをふる = "ダイスをふる";
        /// <summary>
        /// 戦闘用コマンドで逃げるを意味するコマンド名（兼表示文字列）です。
        /// </summary>
        public static string s_CommandName_Escape・にげる = "にげる";



        private int p_id・コマンドＩＤ = 0;
        /// <summary>
        /// この戦闘コマンドのＩＤです。実際に使われるのは1～。
        /// コンストラクタで、自動的にユニークな識別子が1以上で設定されます。原則、変更はしないでください。
        /// ※キャラ間でも重複しないＩＤなので、コマンドＩＤだけでキャラやコマンドを特定することができます。
        /// </summary>
        public int getp_id() { return p_id・コマンドＩＤ; }
        
        private string p_name・コマンド名 = "";
        /// <summary>
        /// （※よく使うものだけgetアクセサを作っています。）
        /// この戦闘コマンドの表示名（コマンド名）。例：攻撃１、攻撃Ｅ、回避Ｓ、ミス、など。
        /// コマンド名は、キャラ内に重複があっても問題ありません。
        /// </summary>
        public string getp_name() { return p_name・コマンド名; }
        /// <summary>
        /// （※よく使うものだけsetアクセサを作っています。）
        /// この戦闘コマンドの表示名（コマンド名）うぃ変更します。例：攻撃１、攻撃Ｅ、回避Ｓ、ミス、など。
        /// コマンド名は、キャラ内に重複があっても問題ありません。
        /// </summary>
        public void setp_name(string _commandName) { p_name・コマンド名 = _commandName; }

        private string p_Text・詳細 = "";
        /// <summary>
        /// （※よく使うものだけsetアクセサを作っています。）
        /// 戦闘コマンドリストに表示されるテキスト（コマンドの説明）を変更します。
        /// 初期値は、コマンド名がそのまま含まれている場合が多いです。
        /// </summary>
        public string setp_Text・詳細(string _shownText) { return p_Text・詳細 = _shownText; }
        /// <summary>
        /// （※よく使うものだけgetアクセサを作っています。）
        /// 戦闘コマンドリストに表示されるテキスト（コマンドの説明）。
        /// 初期値は、コマンド名がそのまま含まれている場合が多いです。
        /// </summary>
        public string getp_Text・詳細() { return p_Text・詳細; }

        /// <summary>
        /// このコマンドを選択した後に表示される、次のコマンド一覧です。初期値はnullです。
        /// 例えば、（例：「たたかう」を選択→「対象Ａ」／「対象Ｂ」…等）を設定できます．
        /// 
        /// 　※入れ子構造になるため、無限ループにならないよう気を付けてください。
        /// </summary>
        public List<CCommand・コマンド> p_nextCommandList・次のコマンド一覧 = null;
        /// <summary>
        /// このコマンドにおける、複数の行動を定義するアクションリストです。初期値はnullです。
        /// 例えば、ここに攻撃用のアクションを２つ追加することで、
        /// このコマンド選択時に二回攻撃をする処理が実現できます。
        /// </summary>
        public List<CBattleAction・戦闘行動> p_actionList・アクションリスト = null;

        /// <summary>
        /// trueなら，このコマンドを実際にこのターンに戦闘で使用できます．
        /// </summary>
        public bool p_isNowUse・現在使用可能 = true;
        /// <summary>
        /// trueなら，このコマンドを実際に戦闘で使用します．ただし，p_isNowUse・現在使用可能がfalseだと使えません．
        /// </summary>
        public bool p_isUseInBattle・戦闘で使用可能 = true;
        /// <summary>
        /// このコマンドを使用してから，再使用可能になるまでのターン数です．
        /// 具体的には，このコマンドを使用してからp_isNowUse・現在使用可能がfalsになり，指定ターン後にtrueになります．
        /// </summary>
        public int p_ReUseTurn・復活ターン数 = 0;


        #region コンストラクタ
        /// <summary>
        /// コンストラクタです．このコマンドの，コマンド名（デフォルトの表示名にもなります）を指定してください。
        /// p_id・コマンドＩＤは自動的に割り当てられます。
        /// 詳しくは、すべての参照を検索して見てみてください。
        /// </summary>
        public CCommand・コマンド(string _commandName・コマンド名)
        {
            // idを増加
            s_idNum++;
            p_id・コマンドＩＤ = s_idNum; // idは1～。クラスで自動的に割り当てられる。

            p_name・コマンド名 = _commandName・コマンド名;
            p_Text・詳細 = _commandName・コマンド名; // デフォルトはコマンド名。
        }
        /// <summary>
        /// コンストラクタです．このコマンドの，コマンド名（デフォルトの表示名にもなります）を指定してください。
        /// p_id・コマンドＩＤは自動的に割り当てられます。
        /// 詳しくは、すべての参照を検索して見てみてください。
        /// </summary>
        public CCommand・コマンド(int _commandID・コマンドID, string _commandName・コマンド名)
        {
            p_id・コマンドＩＤ = _commandID・コマンドID;

            p_name・コマンド名 = _commandName・コマンド名;
            p_Text・詳細 = _commandName・コマンド名; // デフォルトはコマンド名。
        }

        // その他のコンストラクタ
        /// <summary>
        /// コンストラクタです．このコマンドの，コマンド名（デフォルトの表示名にもなります）を必ず指定してください。
        /// また必要であれば、このコマンドを選択した後に表示される，次のコマンド一覧（例：「たたかう」を選択→「対象Ａ」／「対象Ｂ」…等）を設定できます．
        /// （詳しくは、コンストラクタのすべての参照を検索して見てみてください。）
        /// </summary>
        public CCommand・コマンド(string _コマンド名, params CCommand・コマンド[] _nextCommands・このコマンド選択後の次のコマンドを列挙)
            : this(_コマンド名) // 先に、他のコンストラクタを呼び出す
        {
            setNextCommands(_コマンド名, _nextCommands・このコマンド選択後の次のコマンドを列挙);

        }
        /// <summary>
        /// コンストラクタです．このコマンドの，コマンド名（デフォルトの表示名にもなります）を必ず指定してください。
        /// また必要であれば、このコマンドを選択した後に表示される，次のコマンド一覧（例：「たたかう」を選択→「対象Ａ」／「対象Ｂ」…等）を設定できます．
        /// （詳しくは、コンストラクタのすべての参照を検索して見てみてください。）
        /// </summary>
        public CCommand・コマンド(int _コマンドID, string _コマンド名, params CCommand・コマンド[] _nextCommands・このコマンド選択後の次のコマンドを列挙)
            : this(_コマンドID, _コマンド名) // 先に、他のコンストラクタを呼び出す
        {
            setNextCommands(_コマンド名, _nextCommands・このコマンド選択後の次のコマンドを列挙);
        }
        private void setNextCommands(string _コマンド名, CCommand・コマンド[] _nextCommands・このコマンド選択後の次のコマンドを列挙)
        {
            // コマンド一覧を作成
            if (p_nextCommandList・次のコマンド一覧 == null)
            {
                p_nextCommandList・次のコマンド一覧 = new List<CCommand・コマンド>();
            }
            // テキストを自動的に作成
            string _text = _コマンド名 + "→";
            foreach (CCommand・コマンド _item in _nextCommands・このコマンド選択後の次のコマンドを列挙)
            {
                p_nextCommandList・次のコマンド一覧.Add(_item);
                _text += s_tab + _item.getp_name();
            }
            // (a)テキストを自動的に作成
            //createCommandText();
            // (b)テキストはコマンド名＋テキストをタブで並べただけ
            p_Text・詳細 = _text;
        }
        /// <summary>
        /// コンストラクタです．このコマンドの，コマンド名（デフォルトの表示名にもなります）を必ず指定してください。
        /// また必要であれば、戦闘行動（行動タイプ，ダメージ数などの詳細パラメータ）を設定できます．
        /// （詳しくは、コンストラクタのすべての参照を検索して見てみてください。）
        /// </summary>
        public CCommand・コマンド(string _コマンド名, params CBattleAction・戦闘行動[] _CBattleActions・戦闘行動を羅列)
            :this(_コマンド名) // 先に、他のコンストラクタを呼び出す
        {
            setBattleActions(_コマンド名, _CBattleActions・戦闘行動を羅列);
        }
        /// <summary>
        /// コンストラクタです．このコマンドの，コマンド名（デフォルトの表示名にもなります）を必ず指定してください。
        /// また必要であれば、戦闘行動（行動タイプ，ダメージ数などの詳細パラメータ）を設定できます．
        /// （詳しくは、コンストラクタのすべての参照を検索して見てみてください。）
        /// </summary>
        public CCommand・コマンド(int _コマンドID, string _コマンド名, params CBattleAction・戦闘行動[] _CBattleActions・戦闘行動を羅列)
            : this(_コマンドID, _コマンド名) // 先に、他のコンストラクタを呼び出す
        {
            setBattleActions(_コマンド名, _CBattleActions・戦闘行動を羅列);
        }
        private void setBattleActions(string _コマンド名, CBattleAction・戦闘行動[] _CBattleActions・戦闘行動を羅列)
        {
            // テキストを自動的に作成
            string _diceActionsText;
            _diceActionsText = _コマンド名 + s_tab;
            // アクションリストを作成
            if (p_actionList・アクションリスト == null)
            {
                p_actionList・アクションリスト = new List<CBattleAction・戦闘行動>();
            }
            foreach (CBattleAction・戦闘行動 _action in _CBattleActions・戦闘行動を羅列)
            {
                p_actionList・アクションリスト.Add(_action);
                _diceActionsText += _action.actionText();
            }
            // (a)ダイステキストを自動的に作成
            //createDiceCommandText();
            // (b)現段階はコマンド名＋アクションテキストをタブで並べただけ
            p_Text・詳細 = _diceActionsText;
        }
        #endregion

        // ここでこういうメソッドを書くと、１コマンドのメモリが重たくなるし、
        // なによりモデル定義と出力処理との分離ができないので、他のクラスに任せる
        //public void next・次のコマンド()
        //{
        //    // 次のコマンド一覧を表示する処理
        //}
    }

}
