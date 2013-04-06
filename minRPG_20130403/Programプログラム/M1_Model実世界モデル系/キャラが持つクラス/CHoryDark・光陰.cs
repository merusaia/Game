using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// キャラクタの現在のポジティブとネガティブ，光と影，聖と闇，陰と陽，太陽と月，（結果で言うと成功と失敗）の創出バランス度合をを司る属性です．
    /// ※名前を省略する目的のためだけに作られる日本語名クラスのため，特別な理由がない限り，新しいプロパティ・メソッドを追加しないことが望ましいです．
    /// </summary>
    //[Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    //public class 光陰 : CPositiveNegative・光陰
    //{
    //}

    /// <summary>
    /// キャラクタの現在のポジティブとネガティブ，光と影，聖と闇，陰と陽，太陽と月，（結果で言うと成功と失敗）の創出バランス度合をを司る属性です．
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CPositiveNegative・光陰
    {
        // [Tips]externをつけないと，.NET3.0以降のget/setアクセサの省略は付けられない？　初期化はこの行ではできない？
        // extern private int p_horyDark・聖闇{get; set;}

    }
}
