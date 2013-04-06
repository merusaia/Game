using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// キャラの魔法・技・必殺技など，全ての特技が属するクラスです．
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CSkill・特技
    {
        bool p_isUseOnBattle・戦闘中に使用可能 = true;
        bool p_isUseOnField・フィールドで使用可能 = true;
        bool p_isUseOnEvent・イベントで使用可能 = true; // いる？
    }
}
