using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 簡単に乱数を生成できる，MyRandomManagerの子クラスです
    /// </summary>
    public class CMyRandomGenerator・ランダム生成者 : CMyRandomManager //ファイル保存用: CMyRandomManager
    {
        bool p_isPastDataSaved・過去の乱数を保存するか = false;

        // コンストラクタ
        public CMyRandomGenerator・ランダム生成者()
        {
            
        }
        /*
        // ファイル保存用
        public CMyRandomGenerator・ランダム生成者(bool _isPastDataSaved・過去の乱数を保存するか)
            //:base( not(_isPastDataSaved・過去の乱数を保存するか), "")
        {
            p_isPastDataSaved・過去の乱数を保存するか = _isPastDataSaved・過去の乱数を保存するか;
            if (p_isPastDataSaved・過去の乱数を保存するか == true)
            {
            }
        }*/
        /*
        public int getRandomNum_0_100・乱数生成()
        {
            //return (int)
        }*/

        /// <summary>
        /// 新しく最小値～最大値（これらの値も含む）までのランダムなInt型の値を生成して，保存します．
        /// </summary>
        /// <param name="_minValue"></param>
        /// <param name="_maxValue_equals_EnumIntMax"></param>
        public int getRandomNum・ランダム値を生成して保存(int _minValue, int _maxValue)
        {
            return base.random.Next(_minValue, _maxValue);
        }
        /// <summary>
        /// 最小値～最大値（これらの値も含む）までのランダムなDouble型の小数を生成して，保存します．
        /// </summary>
        /// <param name="_minVale"></param>
        /// <param name="_maxVale"></param>
        /// <returns></returns>
        public double getRandomNum・ランダム値を生成して保存(double _minValue, double _maxValue)
        {
            double _randomNum = base.random.NextDouble() * (_maxValue - _minValue) + _minValue;
            return _randomNum;
        }
    }
}
