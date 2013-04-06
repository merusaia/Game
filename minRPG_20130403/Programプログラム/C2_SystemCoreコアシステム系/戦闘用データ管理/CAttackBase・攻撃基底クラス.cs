using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    public enum EAttackResult・攻撃結果
    {
        命中,
        ミス,
        会心の一撃,
        一部回避,
        受け止め,
        跳ね返し,
        その他,
    }

    /// <summary>
    /// 剣・打撃・飛び道具（銃・ボール）・魔法・氣などの様々な攻撃の種類の元となる親クラスです．
    /// </summary>
    public class CAttackBase・攻撃基底クラス
    {
        virtual public EAttackResult・攻撃結果 攻撃(){
            return EAttackResult・攻撃結果.その他;
        }
    }
}
