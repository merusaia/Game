using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    public enum ENoize
    {
        /// <summary>
        /// 影響率 * (値の0～100％)の，大きめのランダムノイズがのります．
        /// </summary>
        nr_大きめ乱数ノイズ,
        /// <summary>
        /// 影響率 * (値の0～50％)の，雑音のような，一様なノイズがのります．
        /// </summary>
        nw_ホワイトノイズ,
        /// <summary>
        /// 影響率 * (値の50～200％)の，いきなりブチっと切れる音のような，突発的なノイズがのります．
        /// </summary>
        nb_バーストノイズ,
        /// <summary>
        /// 影響率 * ？ のノイズがのります．
        /// </summary>
        np_ピンクノイズ,
    }

    /// <summary>
    /// アトラクター選択の出力に必要となるプロパティやメソッドを定義したクラスです．
    /// </summary>
    public class CAtractorOut・ΔX
    {
        public CAtractorOut・ΔX()
        {
        }
        public CAtractorOut・ΔX(CAtractorIn・X _X)
        {
            setDelta(_X);
        }
        public CAtractorOut・ΔX(CAtractorIn・X _X2, CAtractorIn・X _X1)
        {
            calcDelta(_X2, _X1);
        }

        private void setDelta(CAtractorIn・X _X)
        {
            // [TODO] this.要素 = X.要素;
        }
        private void calcDelta(CAtractorIn・X _X2, CAtractorIn・X _X1)
        {
            // [TODO] this.要素 = (X2.要素 - X1.要素);
        }

        /// <summary>
        /// 全ての要素を，掛け算します．
        /// ※この要素の中身を変更するので，何も返しません．
        /// </summary>
        /// <param name="_multiRate"></param>
        /// <returns></returns>
        virtual public void multiply(double _multiRate)
        {
            // ここに，全ての要素を掛け算する処理を書いてください．
            
            //return this;
        }

        virtual public void noize(ENoize _tノイズの種類, double _rノイズ影響率)
        {
            switch (_tノイズの種類)
            {
                case ENoize.nr_大きめ乱数ノイズ:
                    break;
                case ENoize.nw_ホワイトノイズ:
                    break;
                case ENoize.nb_バーストノイズ:
                    break;
                case ENoize.np_ピンクノイズ:
                    break;
                default:
                    break;
            }
        }


    }
}
