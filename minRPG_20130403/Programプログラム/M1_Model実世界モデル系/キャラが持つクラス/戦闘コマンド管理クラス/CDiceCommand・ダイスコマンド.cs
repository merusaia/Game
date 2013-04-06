using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// ダイス戦闘に使われる，ダイスを振って戦うときの１つのダイスコマンドです．
    /// 全キャラ間でユニークな識別子は、コマンドＩＤです。コマンド名は重複しても問題ありません。
    /// １人のキャラが多くのダイスコマンドを所有する場合は，どのコマンドを戦闘で使うかをON/OFFすることができます．
    /// 
    /// なお、１つのダイスコマンドには、複数のアクション（CBattleAction・戦闘行動クラス）を定義できます。
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CDiceCommand・ダイスコマンド : CCommand・コマンド
    {
        // プロパティの詳細は基底クラスを参照

        public List<CBattleAction・戦闘行動> p_actionList・ダイスアクション群 = new List<CBattleAction・戦闘行動>();

        // [MEMO]基底クラスのコンストラクタを明示的に呼び出す http://ufcpp.net/study/csharp/oo_inherit.html
        // 派生クラスのインスタンスを生成する際、 自動的に基底クラスのコンストラクタも呼び出されます。
        // しかし、この際、呼び出されるコンストラクタは「引数なしのコンストラクタ」になります。
        // 基底クラスの引数つきのコンストラクタを呼び出すためには、 以下のように自分でコードを書いて明示的に基底クラスのコンストラクタを呼び出す必要があります。 
        // public クラス名 : base(引数)
        //{
        //}
        /// <summary>
        /// コンストラクタです．このコマンドの，コマンド名（技名）と，それぞれのダイスコマンドの詳細パラメータ（行動タイプ，ダメージ数など）を設定できます．
        /// 詳しくは、すべての参照を検索して見てみてください。
        /// </summary>
        public CDiceCommand・ダイスコマンド(int _コマンドID, string _コマンド名, params CBattleAction・戦闘行動[] _ダイスアクションを羅列)
            :base(_コマンドID, _コマンド名, _ダイスアクションを羅列)
        {
            string _diceActionsText;
            _diceActionsText = _コマンド名 + s_tab;
            foreach (CBattleAction・戦闘行動 _action in _ダイスアクションを羅列)
            {
                p_actionList・ダイスアクション群.Add(_action);
                _diceActionsText += _action.actionText();
            }
            // (a)ダイステキストを自動的に作成
            //createDiceCommandText();
            // (b)現段階はコマンド名＋アクションテキストをタブで並べただけ
            setp_Text・詳細(_diceActionsText);

        }
        /*
        /// <summary>
        /// このダイスコマンドの説明である，ダイステキストを作成します．
        /// </summary>
        /// <returns></returns>
        private void createDiceCommandText()
        {
            string _diceCommandText = "";

            _diceCommandText = _コマンド名 + s_tab;

            
            actionText += getP3_行動対象().ToString() + "に" + s_tab;
            if (getP2_行動タイプ() == EBattleActionType・行動タイプ.t03_ミス)
            {
                _actionText = "";
            }
            else
            {
                _actionText = MyTools.getEnumKeyName_InDetail<EActionObject・行動対象>(_行動対象1) + MyTools.getAboutString(_ダメージ数1, 0);
                if (_行動タイプ1 == EBattleActionType・行動タイプ.t01_ダメージ)
                {
                    _actionText += "ダメージ";
                }
                else if (_行動タイプ1 == EBattleActionType・行動タイプ.t01b_ＨＰ回復)
                {
                    _actionText += "回復";
                }else if (_行動タイプ1 == EBattleActionType・行動タイプ.)
            }
            

            p_Text・詳細 = _diceCommandText;
        }
        */
    }

}
