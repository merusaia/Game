using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Runtime.InteropServices;
using LIB;
using MMSYSTEM;

using HWAVEIN = System.IntPtr;	// Handler on Input Wave

namespace AudioFC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const int SamplingRate = 8000;         //Hz 16000Hzだと8000Hzが検出上限
        const int FFTSize = 16384;              //FFTするサンプル数。2の整数乗に限る。
                                                //16000Hzだと4.1秒に相当
        const int SamplingSize = SamplingRate/2;  //この数のサンプルをFFTSizeにゼロ詰めしてFFT
        const double Threshold = 10000;         //有音と見なす下限(根拠ない)

        InPCM inPCM;

        private void Form1_Load(object sender, EventArgs e)
        {
            //ウインドウのタイトルバー作成
            this.Text = App.AppTitleBar();
            //表示の初期化
            label1.Text = "";
            toolStripProgressBar1.Maximum = 100;
            //Wave入力の準備
            inPCM = new InPCM(SamplingRate, SamplingSize);
            timer1.Interval = 490;//ms
            timer1.Enabled = true;
        }
        //--------------------------------------------------------------------------------------
        int Volume(short[] pcm)
        {
            double sum = 0;
            foreach (short s in pcm)
                sum += Math.Abs((double)s);
            int v = (int)((sum / pcm.Length) * 100 / 23768);
            return v > 100 ? 100 : v;
        }
        //--------------------------------------------------------------------------------------
        //インターバル・タイマ
        private void timer1_Tick(object sender, EventArgs e)
        {
            while (true)
            {
                int qc = inPCM.GetQueueCount();
                toolStripStatusLabel1.Text = "Queue:" + qc.ToString();
                if (qc <= 0) break;//データーがない
                short[] pcm = inPCM.Read();
                short[] fftdata = new short[FFTSize];
                Array.Copy(pcm, fftdata, pcm.Length);
                Complex[] fft_result = FFT.fft(fftdata);
                double max = double.MinValue;
                int pos = -1;
                for (int i = 0; i < fft_result.Length; i++)
                {
                    double v = (double)(fft_result[i] * fft_result[i].Conj()).real;
                    if (v > max)
                    {
                        max = v;
                        pos = i;
                    }
                }
                //周波数の表示
                double hz = 0;
                if (max > Threshold)
                    hz = pos * ((double)(SamplingRate / 2) / (double)fft_result.Length);
                label1.Text = hz.ToString("0.0");
                //ボリュームの表示
                toolStripProgressBar1.Value = Volume(pcm);
            }
        }
        //--------------------------------------------------------------------------------------
        //Windowが閉じられる
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            if (inPCM != null) inPCM.Close();
        }
    }
}
