using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// キャラのIDと特定の一つのパラメータだけを格納する、必要最小限のクラスです．
    /// 使用例としては，戦闘参加キャラの素早さを比較したり，
    /// 特定のパラメータの値が誰のものかを検索したりするときに使うと便利です．
    /// </summary>
    public class CCPInfo・キャラパラ情報
    {
        /// <summary>
        /// 特定のパラメータの値です。何のパラメータを示すかは、外部で定義・取得してください。
        /// </summary>
        public double para = 0.0;
        /// <summary>
        /// キャラのIDです。何のIDを示すかは、外部で定義・取得してください。
        /// </summary>
        public int id = 0;

        /// <summary>
        /// コンストラクタです．特定の一つのパラメータと，そのキャラのIDを定義してださい．
        /// </summary>
        public CCPInfo・キャラパラ情報(double _para・このキャラの特定のパラメータの値, int _id・このキャラのid)
        {
            para = _para・このキャラの特定のパラメータの値;
            id = _id・このキャラのid;
        }
    }
}
