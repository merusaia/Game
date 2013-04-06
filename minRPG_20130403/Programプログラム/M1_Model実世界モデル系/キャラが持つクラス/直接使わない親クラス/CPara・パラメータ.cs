using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 各パラメータの値をdouble型で管理しているクラスです．remarksの案2の場合の実装です．パラメータ名はCParameters・パラメータ群クラスが管理するので格納しないでください．
    /// </summary>
    /// <remarks>
    /// こう書けるようにする！！？つまり，各パラメータをdoubleではなく，doubleを継承したクラスにする？
    /// でも，キャストがめんどうになるからやっぱり違う？
    /// それでもget/setアクセサを使うんだから全然面倒ではない！見やすいコードのほうが優先！
    /// ==================================
    /// (案1: こっちが今の原則．しかし，パラメータを追加するごとに修正する手間がかかる．)
    /// double _attack = _chara.攻撃力();
    /// _chara.calc行動ゲージ(+50);
    /// ==================================
    /// (案2 : こっちの方がものすごく汎用性が高い．しかし，パラメータの複雑な計算式には.get()がとっても邪魔．)
    /// double _attack = _chara.攻撃力().get();
    /// _chara.行動ゲージ().set(+50);
    /// ===================================
    /// </remarks>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CPara・パラメータ
    {
        double p_paraValue・パラメータ値 = 0.0;
        
        
        /// <summary>
        /// パラメータの値を取得します．
        /// </summary>
        /// <returns></returns>
        public double get()
        {
            return p_paraValue・パラメータ値;
        }
        /// <summary>
        /// パラメータの値を設定します．
        /// </summary>
        /// <param name="_newValue"></param>
        /// <returns></returns>
        public void set(double _newValue)
        {
            setP_paraValue・値の範囲チェックして代入(_newValue);
        }
        /// <summary>
        /// パラメータの値を増減します．
        /// </summary>
        /// <param name="_増減値"></param>
        public void 増減(double _増減値)
        {
            double _value = p_paraValue・パラメータ値 + _増減値;
            setP_paraValue・値の範囲チェックして代入(_value);
        }
        /// <summary>
        /// パラメータの値を増減します．
        /// </summary>
        /// <param name="_増減値"></param>
        public void 増減(int _増減値)
        {
            増減((double)_増減値);
        }
        /// <summary>
        /// パラメータの値を＊＊＊％します．※引数は％の数値（例：　0％～100％，負の値もOK）です．
        /// </summary>
        /// <param name="_乗除パーセント"></param>
        public void 乗除(int _乗除パーセント)
        {
            乗除((double)_乗除パーセント / 100.0);
        }
        /// <summary>
        /// パラメータの値を実数倍します．※引数は倍数（例：　0.0～1.0，負の値もOK）です．
        /// </summary>
        /// <param name="_乗除パーセント"></param>
        public void 乗除(double _倍率)
        {
            double _value = p_paraValue・パラメータ値 * _倍率;
            setP_paraValue・値の範囲チェックして代入(_value);
        }

        /// <summary>
        /// ※パラメータの値を変更するときは，必ずこのメソッドが呼び出されます．
        /// </summary>
        /// <param name="_新しいパラメータ値"></param>
        private void setP_paraValue・値の範囲チェックして代入(double _新しいパラメータ値)
        {
            if (_新しいパラメータ値 > s_MAX)
            {
                p_paraValue・パラメータ値 = s_MAX;
            }
            else if (_新しいパラメータ値 < s_MIN)
            {
                p_paraValue・パラメータ値 = s_MIN;
            }
            //else
            //{
            //    // [MEMO][TODO]代入値の桁数は，元が整数なら切り捨て，小数なら3～6位まで格納にする？
            //}
            p_paraValue・パラメータ値 = _新しいパラメータ値;

        }
        /// <summary>
        /// パラメータの理論的限界の最大値です．
        /// </summary>
        public static double s_MAX = double.MaxValue; //整数999999999999と，0.999999999999に分ける;？
        /// <summary>
        /// パラメータの理論的限界の最小値です．
        /// </summary>
        public static double s_MIN = double.MinValue;
    }
}
