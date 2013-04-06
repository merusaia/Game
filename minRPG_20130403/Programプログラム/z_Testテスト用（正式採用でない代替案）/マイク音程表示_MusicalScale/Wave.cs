using System;
using System.Collections.Generic;
using System.Text;

namespace MusicalScale
{
    class Wave
    {
        const double Maximize = 16384;
        const int ScaleTimeSec = 2;//音階を鳴らす時の各音の長さ(秒)
        /// <summary>
        /// 指定周波数の正弦波のPCMサンプルを作る
        /// </summary>
        /// <param name="Hz">周波数</param>
        /// <param name="samplingRate">サンプリングレート</param>
        /// <param name="sec">サンプリング時間</param>
        /// <returns></returns>
        public static double[] Sin(double Hz, int samplingRate, int sec)
        {
            double[] wave = new double[samplingRate * sec];
            double r = 0.0;
            double d = (2.0 * Math.PI * Hz) / samplingRate;
            for (int i = 0; i < wave.Length; i++)
            {
                wave[i]=Math.Sin(r);
                r += d;
            }
            return wave;
        }
        static readonly double[] equalScaleFact = new double[] { 261.63, 293.66, 329.63, 349.23, 392.0, 440.0, 493.88, 523.25 };
        /// <summary>
        /// 十二平均律の音階のサンプルを作る
        /// </summary>
        /// <param name="samplingRate">サンプリングレート</param>
        /// <returns>サンプル</returns>
        public static Int16[] EqualScale(int samplingRate)
        {
            Int16[] wave = new Int16[samplingRate * equalScaleFact.Length * ScaleTimeSec];
            int dest = 0;
            for (int i = 0; i < equalScaleFact.Length; i++)
            {
                double[] s = Sin(equalScaleFact[i], samplingRate, ScaleTimeSec);
                for (int j = 0; j < samplingRate * ScaleTimeSec; j++)
                {
                    wave[dest++] = (Int16)(s[j] * Maximize);
                }
            }
            return wave;
        }
        static readonly double[] justScaleFact = new double[] { 264.0, 297.0, 330.0, 352.0, 396.0, 440.0, 495.0, 528.0 };
        /// <summary>
        /// 純正律の音階のサンプルを作る
        /// </summary>
        /// <param name="samplingRate">サンプリングレート</param>
        /// <returns>サンプル</returns>
        public static Int16[] JustScale(int samplingRate)
        {
            Int16[] wave = new Int16[samplingRate * justScaleFact.Length * ScaleTimeSec];
            int dest = 0;
            for (int i = 0; i < justScaleFact.Length; i++)
            {
                double[] s = Sin(justScaleFact[i], samplingRate, ScaleTimeSec);
                for (int j = 0; j < samplingRate * ScaleTimeSec; j++)
                {
                    wave[dest++] = (Int16)(s[j] * Maximize);
                }
            }
            return wave;
        }
        const int CHORD_TIME = 5;//5秒間分の和音を作る
        /// <summary>
        /// 十二平均律の和音のサンプルを作る
        /// </summary>
        /// <param name="samplingRate">サンプリングレート</param>
        /// <param name="selected">8音程のどの音が選択されているかを示す配列</param>
        /// <returns>サンプル</returns>
        public static Int16[] EqualChord(int samplingRate, bool[] selected)
        {
            Int16[] wave = new Int16[samplingRate * CHORD_TIME];

            double[] w = new double[samplingRate * CHORD_TIME];
            for (int i = 0; i < samplingRate * CHORD_TIME; i++)
                w[i] = 0.0;//初期値ゼロをセット

            double N = 0.0;
            for (int i = 0; i < selected.Length; i++)
            {
                if (selected[i])
                {
                    N += 1.0;
                    double[] s = Sin(equalScaleFact[i], samplingRate, CHORD_TIME);
                    for (int j = 0; j < samplingRate * CHORD_TIME; j++)
                    {
                        w[j] += s[j];
                    }
                }
            }
            for (int i = 0; i < samplingRate * CHORD_TIME; i++)
            {
                wave[i] = (Int16)(w[i] / N * Maximize);
            }
            return wave;
        }
        /// <summary>
        /// 純正律の和音のサンプルを作る
        /// </summary>
        /// <param name="samplingRate">サンプリングレート</param>
        /// <param name="selected">8音程のどの音が選択されているかを示す配列</param>
        /// <returns>サンプル</returns>
        public static Int16[] JustChord(int samplingRate, bool[] selected)
        {
            Int16[] wave = new Int16[samplingRate * CHORD_TIME];

            double[] w = new double[samplingRate * CHORD_TIME];
            for (int i = 0; i < samplingRate * CHORD_TIME; i++)
                w[i] = 0.0;//初期値ゼロをセット

            double N = 0.0;
            for (int i = 0; i < selected.Length; i++)
            {
                if (selected[i])
                {
                    N += 1.0;
                    double[] s = Sin(justScaleFact[i], samplingRate, CHORD_TIME);
                    for (int j = 0; j < samplingRate * CHORD_TIME; j++)
                    {
                        w[j] += s[j];
                    }
                }
            }
            for (int i = 0; i < samplingRate * CHORD_TIME; i++)
            {
                wave[i] = (Int16)(w[i] / N * Maximize);
            }
            return wave;
        }
    }
}
