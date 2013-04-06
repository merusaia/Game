using System;
using System.Collections.Generic;
using System.Text;
// 以上、Visual Web Developerで新規クラス追加したときのusing4個

using PublicDomain;

namespace PublicDomain
{
    /// <summary>
    /// ゲームランクを示すenum型列挙体です。
    /// ToString()にすると、要素名（"F","E"など）が取れます。
    /// (int)型に変更すると、要素の値として、比較可能な強さ（None=0,F=1,E=2,D=3,C=4,B=5,A=6,S=7,SS=8,SSS=9,SSSS=10,COUNT=11）を取得できます。。
    /// getEGameRank・ゲームランク_FtoSなどで使います。
    /// 
    /// 　※それぞれの値が取る範囲を設定したい場合は、setEGameRank・ゲームランク()を使ってください。
    /// </summary>
    public enum ERank・ランク
    {
        /// <summary>
        /// (int)=0。クラスＦ未満か、何も設定されていません。
        /// </summary>
        無し,
        /// <summary>
        /// (int)=1。クラスＦ以上Ｅ未満です。
        /// </summary>
        Ｆ,
        /// <summary>
        /// (int)=2。クラスＥ以上Ｄ未満です。
        /// </summary>
        Ｅ,
        /// <summary>
        /// (int)=3。クラスＤ以上Ｃ未満です。
        /// </summary>
        Ｄ,
        /// <summary>
        /// (int)=4。クラスＣ以上Ｂ未満です。
        /// </summary>
        Ｃ,
        /// <summary>
        /// (int)=5。クラスＢ以上Ａ未満です。
        /// </summary>
        Ｂ,
        /// <summary>
        /// (int)=6。クラスＡ以上Ｓ未満です。
        /// </summary>
        Ａ,
        /// <summary>
        /// (int)=7。クラスＳ以上ＳＳ未満です。
        /// </summary>
        Ｓ,
        /// <summary>
        /// (int)=8。クラスＳＳ以上ＳＳＳ未満です。
        /// </summary>
        ＳＳ,
        /// <summary>
        /// (int)=9。クラスＳＳＳ以上ＳＳＳＳ未満です。
        /// </summary>
        ＳＳＳ,
        /// <summary>
        /// (int)=10。クラスＳＳＳＳ以上です。測定できないクラスＯＶＥＲもここ。
        /// </summary>
        ＳＳＳＳ,
        /// <summary>
        /// (int)=このEnumの要素数です。
        /// </summary>
        _COUNT // これで要素数が取れる。
    }


    /// <summary>
    /// ゲームランクを示すenum型列挙体EGameRankを管理するstaticメソッドを集めた、クラスです。
    /// getEGameRank・ゲームランク_FtoSなどで使います。
    /// 
    /// 　※EGameRankそれぞれの値が取る範囲を設定したい場合は、setEGameRank・ゲームランク()を使ってください。
    /// </summary>
    public class CRank・ゲームランク
    {
        /// <summary>
        /// EGameRank・ゲームランク型の各ランクそれぞれが取る最小値を格納している配列です。ここで初期化せず、setEGameRank・ゲームランク()か、getEGameRank・ゲームランク()を使ってください。
        /// </summary>
        private static double[] p_EGameRank・ゲームランク_EachValueMins;
        /// <summary>
        /// Ｆ～Ｓ…のクラス分けしたenum型の、各クラスと判定される、最小値を設定します。
        /// 
        /// 第１引数にそれぞれクラスＦ～Ｓ～ＳＳＳＳの最小値を１０個の配列[1]～[10]に順番に入れてください。[0]は無視しますので0などで結構です。
        /// これを一度も呼び出していない場合は、F1-SSSS100のデフォルト値{ 0,1,20,40,50,60,70,90,95,98,100 }が予め入っています。
        /// </summary>
        public static void setEGameRank・ゲームランク_FtoS_ByMin(double[] _minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS)
        {
            if (_minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS.Length >= 11)
            {
                p_EGameRank・ゲームランク_EachValueMins = null;
                p_EGameRank・ゲームランク_EachValueMins = _minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS;
            }
        }
        /// <summary>
        /// Ｆ～Ｓ…のクラス分けしたenum型の、各クラスと判定される、最小値を設定します。
        /// 
        /// 引数にそれぞれクラスＦ～Ｓ～ＳＳＳＳの最小値に入れてください。
        /// これを一度も呼び出していない場合は、F1-SSSS100のデフォルト値{ 0,1,20,40,50,60,70,90,95,98,100 }が予め入っています。
        /// </summary>
        public static void setEGameRank・ゲームランク_FtoS_ByMin(double _F_min, double _E_min, double _D_min, double _C_min, double _B_min, double _A_min, double _S_min, double _SS_min, double _SSS_min, double _SSSS_min)
        {
            setEGameRank・ゲームランク_FtoS_ByMin(new double[] { _F_min, _E_min, _D_min, _C_min, _B_min, _A_min, _S_min, _SS_min, _SSS_min, _SSSS_min });
        }
        /// <summary>
        /// Ｆ～Ｓ…のクラス分けしたenum型の、各クラスと判定される、含有率（がんゆうりつ）を設定します。
        /// 全部で１００％にしなくても、内部で合計１００％になるように自動で調整するので、整数でも％でも小数でも万単位でも、好きな単位で入れてください。
        /// 
        /// 引数にそれぞれクラスＮＯＮＥ（boolFalse_無）～Ｆ～Ｓ～ＳＳＳＳの含有率に入れてください。
        /// これを一度も呼び出していない場合は、F1-SSSS100のデフォルト値{1,19,20,10,10,10,20, 5, 3, 2 ,1}（　　以下の最小値から計算{ 0,1,20,40,50,60,70,90,95,98,100 }）が予め入っています。
        /// </summary>
        public static void setEGameRank・ゲームランク_FtoS_ByInclusingRate(double _None_rate, double _F_rate, double _E_rate, double _D_rate, double _C_rate, double _B_rate, double _A_rate, double _S_rate, double _SS_rate, double _SSS_rate, double _SSSS_rate)
        {
            p_EGameRank・ゲームランク_EachValueMins = null;
            double _rateSum = _None_rate + _F_rate + _E_rate + _D_rate + _C_rate + _B_rate + _A_rate + _S_rate + _SS_rate + _SSS_rate + _SSSS_rate;
            double _p100 = 100.0;
            // それぞれの含まれる率から、値の取りうる範囲を０～１００％として、最小値を計算            
            double _F_min = _None_rate / _rateSum * _p100;
            double _E_min = _F_min + _F_rate / _rateSum * _p100;
            double _D_min = _E_min + _E_rate / _rateSum * _p100;
            double _C_min = _D_min + _D_rate / _rateSum * _p100;
            double _B_min = _C_min + _C_rate / _rateSum * _p100;
            double _A_min = _B_min + _B_rate / _rateSum * _p100;
            double _S_min = _A_min + _A_rate / _rateSum * _p100;
            double _SS_min = _S_min + _S_rate / _rateSum * _p100;
            double _SSS_min = _SS_min + _SS_rate / _rateSum * _p100;
            double _SSSS_min = _SSS_min + _SSS_rate / _rateSum * _p100;
            p_EGameRank・ゲームランク_EachValueMins = new double[] { 0, _F_min, _E_min, _D_min, _C_min, _B_min, _A_min, _S_min, _SS_min, _SSS_min, _SSSS_min };
        }
        /// <summary>
        /// Ｆ～Ｓ…のクラス分けしたenum型の、各クラスと判定される、含有率（がんゆうりつ）を設定します。
        /// 全部で１００％にしなくても、内部で合計１００％になるように自動で調整するので、整数でも％でも小数でも万単位でも、好きな単位で入れてください。
        /// 
        /// 引数にそれぞれクラスＮＯＮＥ（boolFalse_無）～Ｆ～Ｓ～ＳＳＳＳの含有率に入れてください。
        /// これを一度も呼び出していない場合は、F1-SSSS100のデフォルト値{1,19,20,10,10,10,20, 5, 3, 2 ,1}（　　以下の最小値から計算{ 0,1,20,40,50,60,70,90,95,98,100 }）が予め入っています。
        /// </summary>
        public static void setEGameRank・ゲームランク_FtoS_ByInclusingRate(double[] _InclusingRate_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS)
        {
            double[] _rates = _InclusingRate_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS;
            if (_rates.Length >= 11)
            {
                setEGameRank・ゲームランク_FtoS_ByInclusingRate(_rates[0], _rates[1], _rates[2], _rates[3], _rates[4], _rates[5], _rates[6], _rates[7], _rates[8], _rates[9], _rates[10]);
            }
        }
        /// <summary>
        /// 引数に指定した値_valueを、setEGameRank・ゲームランクメソッドで設定したＦ～Ｓ…のクラス分けしたenumのEGameRank・ゲームランク型で返します。
        /// 設定してない場合は、F1-SSSS100のデフォルト値{ 0,1,20,40,50,60,70,90,95,98,100 }が使われます。
        ///         ToString()にすると、要素名（"F","E"など）が取れます。
        /// (int)型に変更すると、要素の値として、比較可能な強さ（None=0,F=1,E=2,D=3,C=4,B=5,A=6,S=7,SS=8,SSS=9,SSSS=10,COUNT=11）を取得できます。。
        /// </summary>
        /// <returns></returns>
        public static ERank・ランク getEGameRank・ゲームランク_FtoS(double _value)
        {
            if (p_EGameRank・ゲームランク_EachValueMins == null)
            {
                // ■初期化処理（setEGameRank・ゲームランクなどで設定しない場合、ここの値がデフォルト）
                //                                                      None, F, E,  D,  C,  B,  A,  S, SS, SSS, SSSS
                p_EGameRank・ゲームランク_EachValueMins = new double[] { 0, 1, 20, 40, 50, 60, 70, 90, 95, 98, 100 };
                // 参考：大学の合格判定は、Ｅ５％　Ｄ２０％　Ｃ５０％　Ｂ５０～７５％　Ａ９０％　らしい。

            }
            ERank・ランク _rank = ERank・ランク.無し;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[1]) return ERank・ランク.無し;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[2]) return ERank・ランク.Ｆ;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[3]) return ERank・ランク.Ｅ;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[4]) return ERank・ランク.Ｄ;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[5]) return ERank・ランク.Ｃ;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[6]) return ERank・ランク.Ｂ;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[7]) return ERank・ランク.Ａ;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[8]) return ERank・ランク.Ｓ;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[9]) return ERank・ランク.ＳＳ;
            if (_value < p_EGameRank・ゲームランク_EachValueMins[10]) return ERank・ランク.ＳＳＳ;
            if (_value >= p_EGameRank・ゲームランク_EachValueMins[10]) return ERank・ランク.ＳＳＳＳ;

            return _rank;
        }
        /// <summary>
        /// 第１引数に指定した値_valueを、Ｆ～Ｓ…のクラス分けしたenum型のEGameRank・ゲームランク型で返します。第２引数にそれぞれクラスＦ～Ｓ～ＳＳＳＳの最小値を１０個の配列[1]～[10]に順番に入れてください。[0]は無視しますので0などで結構です。
        ///             ToString()にすると、要素名（"F","E"など）が取れます。
        /// (int)型に変更すると、要素の値として、比較可能な強さ（None=0,F=1,E=2,D=3,C=4,B=5,A=6,S=7,SS=8,SSS=9,SSSS=10,COUNT=11）を取得できます。。
        /// </summary>
        /// <returns></returns>
        public static ERank・ランク getEGameRank・ゲームランク_FtoS(double _value, double[] _minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS)
        {
            ERank・ランク _returnedRank = ERank・ランク.無し;
            double[] _min = _minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS;
            if (_min.Length >= (int)ERank・ランク._COUNT - 1)
            {
                _returnedRank = getEGameRank・ゲームランク_FtoS(_value, _min[1], _min[2], _min[3], _min[4], _min[5], _min[6], _min[7], _min[8], _min[9], _min[10]);
            }
            return _returnedRank;
        }
        /// <summary>
        /// 引数に指定した値_valueを、Ｆ～Ｓ…のクラス分けしたenum型のEGameRank・ゲームランク型で返します。引数にそれぞれの最小値を順番に入れてください。
        ///             ToString()にすると、要素名（"F","E"など）が取れます。
        /// (int)型に変更すると、要素の値として、比較可能な強さ（None=0,F=1,E=2,D=3,C=4,B=5,A=6,S=7,SS=8,SSS=9,SSSS=10,COUNT=11）を取得できます。。
        /// </summary>
        /// <returns></returns>
        public static ERank・ランク getEGameRank・ゲームランク_FtoS(double _value, double _F_min, double _E_min, double _D_min, double _C_min, double _B_min, double _A_min, double _S_min, double _SS_min, double _SSS_min, double _SSSS_min)
        {
            ERank・ランク _rank = ERank・ランク.無し;
            if (_value < _F_min) return ERank・ランク.無し;
            if (_value < _E_min) return ERank・ランク.Ｆ;
            if (_value < _D_min) return ERank・ランク.Ｅ;
            if (_value < _C_min) return ERank・ランク.Ｄ;
            if (_value < _B_min) return ERank・ランク.Ｃ;
            if (_value < _A_min) return ERank・ランク.Ｂ;
            if (_value < _S_min) return ERank・ランク.Ａ;
            if (_value < _SS_min) return ERank・ランク.Ｓ;
            if (_value < _SSS_min) return ERank・ランク.ＳＳ;
            if (_value < _SSSS_min) return ERank・ランク.ＳＳＳ;
            if (_value >= _SSSS_min) return ERank・ランク.ＳＳＳＳ;

            return _rank;
        }
        /// <summary>
        /// 現在設定されているEGameRank・ゲームランクの情報（それぞれのランクに判定される値の範囲）を示した文字列を返します。
        /// </summary>
        /// <returns></returns>
        public static string getEGameRank・ゲームランクInfo()
        {
            if (p_EGameRank・ゲームランク_EachValueMins == null)
            {
                getEGameRank・ゲームランク_FtoS(0); // getを呼び出してnullのプロパティを初期化
            }
            string _info = "【EGameRank・ゲームランク情報】\t＜全体に含まれる％＞（0を無、1～100を有効ランクに当てはめるので、合計101％）\n";
            int _lastindex = p_EGameRank・ゲームランク_EachValueMins.Length - 1;
            double _beforeMin = 0;
            double _max = p_EGameRank・ゲームランク_EachValueMins[_lastindex];
            for (int i = 0; i < p_EGameRank・ゲームランク_EachValueMins.Length; i++)
            {
                double _min = p_EGameRank・ゲームランク_EachValueMins[i];
                double _nextMin = 100;      // ～未満の値を計算するために必要な、次の配列の最小値
                string _mimanString = "";   // ～未満と表示するかどうか
                string _hukumuString = "";  // （ランク無）と表示するかどうか
                if (i == 0) // 最初
                {
                    _hukumuString = "（ランク無）";
                    if (_lastindex >= 1) // 配列数が1の時はエラーになるので、確認
                    {
                        _nextMin = p_EGameRank・ゲームランク_EachValueMins[1];
                        _mimanString = _nextMin.ToString() + "未満";
                    }
                }
                else if (i == _lastindex) // 最後
                {
                    if (_min == 100)
                    {
                        _nextMin = 101; // 100％も１％に含める
                    }
                    else
                    {
                        _nextMin = 100;
                    }
                    _mimanString = "";
                }
                else // 最初と最後以外
                {
                    _nextMin = p_EGameRank・ゲームランク_EachValueMins[i + 1];
                    _mimanString = _nextMin.ToString() + "未満";
                }
                _info += "Rank " + getEGameRank・ゲームランク_FtoS(_min).ToString() + ": ";
                // 値の範囲
                _info += _min + "以上" + _mimanString;
                // 全体に含まれる率
                _info += "\t\t\t＜" + ((_nextMin - _min) / _max) * 100.0 + "％" + _hukumuString + "＞" + "\n";
                _beforeMin = _min;
            }
            return _info;
        }




    }
}
