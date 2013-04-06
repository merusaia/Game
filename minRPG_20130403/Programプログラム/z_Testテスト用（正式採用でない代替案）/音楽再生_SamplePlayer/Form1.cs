using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using LIB;
using RIFF;

namespace SamplePlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 再生ファイル名です。
        /// </summary>
        public string p_playSoundFileName_FullPath = "";

        const string PAUSE = "PAUSE";   //ポーズボタンのキートップ表示文字列
        const string RESUME = "RESUME"; //ポーズボタンのキートップ表示文字列
        public int VOLUME_MAX = 1000; // 16;//トラックバーの刻みになる


        WaveReader wr;
        PCMPlayer player;

        bool p_isPlay_orPause = false;//再生中（一時停止中もtrue）

        bool bStopCalled = false;//ストップを呼び出した
        bool bVolumeInitialized = false;//一回目だけ、音量を読み出す


        private void Form1_Load(object sender, EventArgs e)
        {
            initialize();
        }
        /// <summary>
        /// 初期化処理です。
        /// </summary>
        private void initialize()
        {
            this.Text = App.AppTitleBar();
            //ボタン類の状態設定

            IdleState();
            //音量トラックバーの設定

            trackBar1.Minimum = 0;
            trackBar1.Maximum = VOLUME_MAX;
            trackBar1.Value = 0;
            //インターバルタイマ設定

            timer1.Interval = 200;
            timer1.Enabled = false;
        }
        //------------------------------------------------------------------
        //状況に応じた、ボタンなどの状態を設定

        void IdleState()
        {
            textBox1.Enabled = true;
            buttonFileDlg.Enabled = true;
            buttonStart.Enabled = true;
            buttonPause.Enabled = false;
            buttonStop.Enabled = false;
            buttonPause.Text = PAUSE;
            toolStripStatusLabelPos.Text = "";
            toolStripStatusLabelSize.Text = "";
            toolStripStatusLabelTime.Text = "";
        }
        void Pause_or_ResumeState()
        {
            if (buttonPause.Text == PAUSE)
            {

                buttonPause.Text = RESUME;
            }
            else
            {
                buttonPause.Text = PAUSE;
            }
        }
        void PlayingState()
        {
            p_isPlay_orPause = true;
            bStopCalled = false;
            buttonPause.Text = PAUSE;
            textBox1.Enabled = false;
            buttonFileDlg.Enabled = false;
            buttonStart.Enabled = false;
            buttonPause.Enabled = true;
            buttonStop.Enabled = true;
        }

        //------------------------------------------------------------------
        long pos = 0;
        byte[] ReadPCM(int bytes)
        {
            if (!p_isPlay_orPause) return null;
            byte[] data = wr.ReadBytes(pos, bytes);
            if ((data == null) || (data.Length <= 0))
            {
                p_isPlay_orPause = false;//EOF
                timer1.Enabled = true;
            }
            pos += data.Length;
            toolStripStatusLabelPos.Text = pos.ToString("#,##0");
            return data;
        }
        void OpenPCM(Chunk fmt, Chunk data)
        {
            wr = new WaveReader(p_playSoundFileName_FullPath, fmt, data);
            player = new PCMPlayer(this.Handle, wr.Format);
            wr.Open();
            toolStripStatusLabelSize.Text = data.length.ToString("#,##0") + " bytes";
            toolStripStatusLabelTime.Text = ((int)(data.length / wr.Format.nAvgBytesPerSec)).ToString("#,##0") + " 秒";
            pos = 0;
            //音量トラックバーの設定

            if (bVolumeInitialized)
            {
                int vol = FromTrackBarVlue(trackBar1.Value);
                player.SetVolume(vol, vol);
            }
            else
            {
                PCMPlayer.VOLUME vol = player.GetVolume();
                trackBar1.Value = (int)vol.left * VOLUME_MAX / 65535;
                bVolumeInitialized = true;
            }
        }
        void ClosePCM()
        {
            wr.Close();
            player.Close();
        }
        //------------------------------------------------------------------
        //トラックバーの値から、0-65535の範囲に。

        int FromTrackBarVlue(int trackBarVlue)
        {
            return trackBarVlue * (65535 / VOLUME_MAX);
        }
        //------------------------------------------------------------------
        protected override void WndProc(ref Message m)
        {
            switch ((uint)m.Msg)
            {
                case WaveOut.MM_WOM_DONE:
                    if (bStopCalled) return;
                    PcmOutBuffer ob = PcmOutBuffer.GetPcmOutBufferFromHeader(m.LParam);
                    if (p_isPlay_orPause)
                    {
                        ob.Write(ReadPCM(ob.Size));
                    }
                    else
                    {
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
        //------------------------------------------------------------------
        //タイマを使って、全バッファがなり終わるのを待ち合わせる
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (player.CheckDone())
            {
                timer1.Enabled = false;
                ClosePCM();
                IdleState();
            }
        }
        //------------------------------------------------------------------
        //ファイル選択ボタン
        private void buttonFileDlg_Click(object sender, EventArgs e)
        {
            selectWavFile();
        }
        /// <summary>
        /// ファイルダイアログからwaveファイルを選択させます。選択したファイルは、p_playSoundFileName_FullPathに格納されます。
        /// </summary>
        public void selectWavFile()
        {
            openFileDialog1.Filter = "WAVE files (*.wav)|*.wav";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result != DialogResult.OK) return;
            p_playSoundFileName_FullPath = openFileDialog1.FileName;
            textBox1.Text = p_playSoundFileName_FullPath;
        }
        //STARTボタン
        private void buttonStart_Click(object sender, EventArgs e)
        {
            playSound();
        }
        /// <summary>
        /// 音楽ファイルを再生します。再生できた場合だけtrueを返します。
        /// </summary>
        public bool playSound()
        {
            return playSound(p_playSoundFileName_FullPath);
        }
        /// <summary>
        /// 音楽ファイルを再生します。再生できた場合だけtrueを返します。
        /// </summary>
        public bool playSound(string _playSoundFileName_FullPath)
        {
            if (wr == null)
            {
                initialize();
            }
            p_playSoundFileName_FullPath = _playSoundFileName_FullPath;
            if (p_playSoundFileName_FullPath.Length <= 0)
            {
                MessageBox.Show("ファイル名を入力してください。");
                return false;
            }
            MMFILE.CheckFileResult result = GetChunks.CheckRIFF(p_playSoundFileName_FullPath);
            if (result.bError)
            {
                MessageBox.Show(result.msg);
                return false;
            }
            if (result.formatType != "WAVE")
            {
                MessageBox.Show("このファイルのRIFFファイルは、WAV形式ではありません。\n"+p_playSoundFileName_FullPath+"\n" + result.formatType);
                return false;
            }
            long progress = 0;
            bool bDone = false;
            Chunk[] chunks = GetChunks.Chunks(p_playSoundFileName_FullPath, 4096, ref progress, ref bDone);
            int i_fmt = -1;
            int i_data = -1;
            for (int i = 0; i < chunks.Length; i++)
            {
                if ((i_fmt <= 0) && (chunks[i].label == "fmt ")) i_fmt = i;
                if ((i_data <= 0) && (chunks[i].label == "data")) i_data = i;
            }
            if ((i_fmt < 0) || (i_data < 0))
            {
                MessageBox.Show("このファイルのRIFFファイルは、fmtとdataのチャンクがそろっていません。" + result.formatType);
                return false;
            }
            OpenPCM(chunks[i_fmt], chunks[i_data]);
            PlayingState();
            player.Start();
            return true;
        }
        //STOPボタン
        private void buttonStop_Click(object sender, EventArgs e)
        {
            stopSound();
        }
        /// <summary>
        /// サウンドを停止します。サウンドが再生されていた時だけ、停止してtrueを返します。
        /// </summary>
        public bool stopSound()
        {
            if (p_isPlay_orPause)
            {
                p_isPlay_orPause = false;
                bStopCalled = true;
                player.Stop();//直ちに中止され、呼び出すとその後、WOM_DONEは発生しない。

                ClosePCM();
                IdleState();
                return true;
            }
            else
            {
                return false;
            }
        }
        //PAUSE/RESUMEボタン
        private void buttonPause_Click(object sender, EventArgs e)
        {
            pause_or_resumeSound();
        }
        /// <summary>
        /// サウンドを一時停止もしくは再開します。一時停止の場合はfalse、再開の場合はtrueを返します。サウンドが再生されていない場合はfalseを返します。
        /// </summary>
        public bool pause_or_resumeSound()
        {
            if (p_isPlay_orPause)
            {
                if (buttonPause.Text == PAUSE)
                {
                    pauseSound();
                    return false;
                }
                else
                {
                    player.Resume();
                    Pause_or_ResumeState();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// サウンドを一時停止します。一時停止の場合だけtrueを返します。
        /// </summary>
        public bool pauseSound()
        {
            if (p_isPlay_orPause)
            {
                if (buttonPause.Text == PAUSE)
                {
                    player.Pause();
                    Pause_or_ResumeState();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// サウンドを再開します。一時停止の場合だけ、再開してtrueを返します。
        /// </summary>
        public bool resumeSound()
        {
            if (p_isPlay_orPause)
            {
                if (buttonPause.Text == RESUME)
                {
                    player.Resume();
                    Pause_or_ResumeState();
                    return true;
                }
            }
            return false;
        }

        //ボリュームスライドバー操作

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            setVolume(trackBar1.Value);
        }
        /// <summary>
        /// サウンドのボリュームを調整します。
        /// </summary>
        private void setVolume(int _volume_0toVOLUMEMAX)
        {
            if (p_isPlay_orPause)
            {
                int vol = FromTrackBarVlue(_volume_0toVOLUMEMAX);
                player.SetVolume(vol, vol);
            }
        }
        //Formが閉じられる
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(p_isPlay_orPause)
                player.Stop();
        }
        //------------------------------------------------------------------
    }
}
