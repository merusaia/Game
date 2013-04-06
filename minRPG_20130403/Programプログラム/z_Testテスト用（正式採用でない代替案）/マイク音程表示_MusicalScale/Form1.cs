using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using LIB;
using MMSYSTEM;
using RIFF;

namespace MusicalScale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------------
        const int SAMPLING_RATE = 22050;
        const int SAMPLE_PER_BITS = 16;

        Timer timer1 = new Timer();
        PCMPlayer player;
        bool bIdle;
        //-----------------------------------------------------------------------
        Int16[] pcm;
        int pcm_pos;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = App.AppTitleBar();
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Enabled = false;
            timer1.Interval = 200;
            IdleState();
        }
        protected override void WndProc(ref Message m)
        {
            switch ((uint)m.Msg)
            {
                case WaveOut.MM_WOM_DONE:
                    PcmOutBuffer ob = PcmOutBuffer.GetPcmOutBufferFromHeader(m.LParam);
                    if (pcm_pos < pcm.Length)
                    {
                        int len = ob.Size / 2;
                        if ((pcm.Length - pcm_pos) < len) len = pcm.Length - pcm_pos;
                        ob.Write(pcm, pcm_pos, len);
                        pcm_pos += len;
                    }
                    else
                    {
                        timer1.Enabled = true;
                        ob.SetDone();
                    }
                    break;
                case WaveIn.MM_WIM_OPEN:
                case WaveIn.MM_WIM_CLOSE:
                case WaveIn.MM_WIM_DATA:
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        //-----------------------------------------------------------------------
        //タイマ

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (player.CheckDone())
            {
                timer1.Enabled = false;
                IdleState();
                player.Close();
            }
        }
        //-----------------------------------------------------------------------
        //状況でボタン類の状態を変える

        void IdleState()
        {
            bIdle = true;
            buttonScale.Enabled = true;
            ButtonChord.Enabled = true;
            checkBoxFile.Enabled = true;
            radioButtonEqual.Enabled = true;
            radioButtonJust.Enabled = true;
            checkBoxC.Enabled = true;
            checkBoxD.Enabled = true;
            checkBoxE.Enabled = true;
            checkBoxF.Enabled = true;
            checkBoxG.Enabled = true;
            checkBoxA.Enabled = true;
            checkBoxH.Enabled = true;
            checkBoxC2.Enabled = true;
        }
        void PlayingState()
        {
            bIdle = false;
            buttonScale.Enabled = false;
            ButtonChord.Enabled = false;
            checkBoxFile.Enabled = false;
            radioButtonEqual.Enabled = false;
            radioButtonJust.Enabled = false;
            checkBoxC.Enabled = false;
            checkBoxD.Enabled = false;
            checkBoxE.Enabled = false;
            checkBoxF.Enabled = false;
            checkBoxG.Enabled = false;
            checkBoxA.Enabled = false;
            checkBoxH.Enabled = false;
            checkBoxC2.Enabled = false;
        }
        //-----------------------------------------------------------------------
        //音程の選択状態を配列に
        bool[] GetSelectedKey()
        {
            bool[] selected = new bool[8];
            int i = 0;
            selected[i++] = checkBoxC.Checked;
            selected[i++] = checkBoxD.Checked;
            selected[i++] = checkBoxE.Checked;
            selected[i++] = checkBoxF.Checked;
            selected[i++] = checkBoxG.Checked;
            selected[i++] = checkBoxA.Checked;
            selected[i++] = checkBoxH.Checked;
            selected[i++] = checkBoxC2.Checked;
            return selected;
        }
        //-----------------------------------------------------------------------
        //Waveファイル出力

        void FileWrite(WAVEFORMATEX wfx)
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "WAVE files (*.wav)|*.wav";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fname = dlg.FileName;
                if (Path.GetExtension(fname) == "") fname += ".wav";
                WavWriter ww = new WavWriter(fname, wfx);
                ww.Write(pcm, pcm.Length);
                ww.Close();
            }
        }
        //-----------------------------------------------------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!bIdle)
                e.Cancel = true;
        }
        //音階を鳴らす
        private void buttonScale_Click(object sender, EventArgs e)
        {
            if (radioButtonEqual.Checked)
                pcm = Wave.EqualScale(SAMPLING_RATE);
            else
                pcm = Wave.JustScale(SAMPLING_RATE);
            pcm_pos = 0;

            WAVEFORMATEX wfx = WinMMHelper.WAVEFORMATEX_PCM(1, SAMPLING_RATE, SAMPLE_PER_BITS);
            if (checkBoxFile.Checked)
            {
                FileWrite(wfx);
            }
            else
            {
                player = new PCMPlayer(this.Handle, wfx);
                player.Start();
                PlayingState();
            }
        }
        //和音ボタン
        private void ButtonChord_Click(object sender, EventArgs e)
        {
            bool[] selected = GetSelectedKey();
            bool notEmpty = false;
            for (int i = 0; i < selected.Length; i++) notEmpty |= selected[i];
            if (!notEmpty) return;

            if (radioButtonEqual.Checked)
                pcm = Wave.EqualChord(SAMPLING_RATE, selected);
            else
                pcm = Wave.JustChord(SAMPLING_RATE, selected);
            pcm_pos = 0;

            WAVEFORMATEX wfx = WinMMHelper.WAVEFORMATEX_PCM(1, SAMPLING_RATE, SAMPLE_PER_BITS);
            if (checkBoxFile.Checked)
            {
                FileWrite(wfx);
            }
            else
            {
                player = new PCMPlayer(this.Handle, wfx);
                player.Start();
                PlayingState();
            }
        }
    }
}
