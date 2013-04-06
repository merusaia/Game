using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 喜び，悲しみ，恐れ，怒り，驚き，嫌悪などの感情です．
    /// ※名前を省略する目的のためだけに作られる日本語名クラスのため，特別な理由がない限り，新しいプロパティ・メソッドを追加しないことが望ましいです．
    /// </summary>
    //public class 感情 : CEmotion・感情
    //{
    //}

    /// <summary>
    /// 喜び，悲しみ，恐れ，怒り，驚き，嫌悪などの感情です．
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CEmotion・感情
    {
        int 喜び = 0; //{get;set;} // .NET 3.0仕様ができない？
        int 哀しみ = 0;
        int 恐れ = 0;
        int 怒り = 0;
        int 驚き = 0;
        int 嫌悪 = 0;
    }
}
