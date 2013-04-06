using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// あるアトラクターに向かって様々なルールで解を求める，個々のアトラクター選択（妖精さん）
    /// を統括する，アトラクター重畳の管理クラス（以下，世界樹と呼ぶ）です．
    /// 　　世界樹に必要なプロパティやメソッドを定義しているクラスです．
    /// 　　アトラクター重畳を１つのクラスで実装したい場合に参考となる，テストクラスとも言えます．
    /// </summary>
    public class CAtractorManager・世界樹
    {
        /// <summary>
        /// 管理するアトラクター選択です．
        /// 　　世界樹が管理している妖精さんです．
        /// 　　それぞれの妖精さんがする仕事を，世界樹はデリゲートによって与えられます（C#のみ）．
        /// </summary>
        private List<CAtractorSelection・妖精さん> _selections・妖精さんたち;

        private C関数近似データX p_X;
        private C関数近似データX p_Xmae;
        private C関数近似変化量ΔX p_ΔX;

        /// <summary>
        /// コンストラクタ（Program.cから呼び出される，mainメソッドの代わり）
        /// </summary>
        public CAtractorManager・世界樹()
        {
            // １．初期化（妖精さんたちのグループと，お仕事内容の準備）
            _selections・妖精さんたち = new List<CAtractorSelection・妖精さん>();
            p_X = new C関数近似データX();
            p_ΔX = new C関数近似変化量ΔX();

            // ２．アトラクター選択の追加（妖精さんの召喚）
            _selections・妖精さんたち.Add(new CAtractorSelection・妖精さん(Dスマートな妖精さんの仕事));
            _selections・妖精さんたち.Add(new CAtractorSelection・妖精さん(D頑固な妖精さんの仕事));

        }
        /// <summary>
        /// 今良く使われる最小二乗法で働く，スマートな妖精さんの仕事です．
        /// </summary>
        /// <param name="_x"></param>
        /// <returns></returns>
        private CAtractorOut・ΔX Dスマートな妖精さんの仕事(CAtractorIn・X _x)
        {
            //ΔX = f(x)*activity + noize,  f(x) = ...x...
            CAtractorOut・ΔX _deltaX = new CAtractorOut・ΔX(_x);
            // [TODO]
            return _deltaX;
        }
        /// <summary>
        /// 堅実に誤差がShikiiti以下にしようと働く，頑固な妖精さんの仕事です．
        /// </summary>
        /// <param name="_x"></param>
        /// <returns></returns>
        private CAtractorOut・ΔX D頑固な妖精さんの仕事(CAtractorIn・X _x)
        {
            CAtractorOut・ΔX _deltaX = new CAtractorOut・ΔX(_x);
            int _shikiiti = 10;
            return _deltaX;
        }
    }
    /// <summary>
    /// N個の座標がわかっているとき，特定の関数に近似する処理のために必要なデータです．
    /// </summary>
    public class C関数近似データX : CAtractorIn・X
    {
    }
    /// <summary>
    /// C関数近似データXの変化量を示したデータです．
    /// （※C関数近似データXを直接扱う方が実装が容易ですが，
    /// 　　各妖精さんが出した変化量の履歴記録，更新順序やデッドロックなどの防止を考えて，
    /// 　　別々に実装しています）
    /// </summary>
    public class C関数近似変化量ΔX : CAtractorOut・ΔX
    {
    }
}
