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
using PublicDomain;

namespace SampleRecorder
{
    public partial class Form1 : Form, IPCMRecorder
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 録音ファイル名です。
        /// </summary>
        public string p_recordedFileName_FullPath = Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス + "sampleRecord1.wav";//"C:/Temp/sampleRecord.wav"; // 初期値は添付ファイル

        PCMRecorder pcmRecorder;
        SamplePlayer.Form1 pcmPlayerForm;

        WavWriter ww;
        public bool p_isRecording;

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
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            IdleState();
            pcmRecorder = new PCMRecorder(this, (IPCMRecorder)this);
            p_isRecording = false;
        }
        //ボタン類の状態設定

        void RecordingState()
        {
            p_isRecording = true;
            StartButton.Enabled = false;
            StopButton.Enabled = true;
            textBox1.Enabled = false;
            FileDlgButton.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
        }
        void IdleState()
        {
            p_isRecording = false;
            StartButton.Enabled = true;
            StopButton.Enabled = false;
            textBox1.Enabled = true;
            FileDlgButton.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
        }
        #region IPCMRecorder メンバ

        //PCMデータをファイルに書き込む(WinMMのコールバック関数から)
        public void PCMData(byte[] pcm)
        {
            if (p_isRecording)
            {
                ww.Write(pcm,pcm.Length);
            }
        }
        #endregion
        //開始ボタン
        private void StartButton_Click(object sender, EventArgs e)
        {
            startRecord();
        }
        /// <summary>
        /// マイク録音を開始します。開始した時だけtrueを返します。
        /// </summary>
        public bool startRecord()
        {
            return startRecord(p_recordedFileName_FullPath, false);
        }
        /// <summary>
        /// マイク録音を開始します。開始した時だけtrueを返します。
        /// </summary>
        public bool startRecord(bool _isCheckOverRide)
        {
            return startRecord(p_recordedFileName_FullPath, _isCheckOverRide);
        }
        /// <summary>
        /// マイク録音を開始します。開始した時だけtrueを返します。
        /// </summary>
        public bool startRecord(string _recordedFileName_FullPath, bool _isCheckOverRide)
        {
            if (p_recordedFileName_FullPath.Length <= 0)
            {
                MessageBox.Show("出力ファイル名を設定してください。");
                return false;
            }

            // 録音ファイルの設定
            p_recordedFileName_FullPath = _recordedFileName_FullPath;
            int hz = 44100;
            if (radioButton48.Checked) hz = 48000;
            int ch = 2;
            if (radioButton_mono.Checked) ch = 1;
            try
            {
                if (File.Exists(_recordedFileName_FullPath))
                {
                    if (_isCheckOverRide == true)
                    {
                        if (MessageBox.Show("ファイルを上書きしますか？", this.Text, MessageBoxButtons.YesNo) != DialogResult.Yes)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(MyTools.getDirectoryName(_recordedFileName_FullPath, true));
                }
                MMSYSTEM.WAVEFORMATEX wfx = WinMMHelper.WAVEFORMATEX_PCM(ch, hz, 16);
                ww = new WavWriter(_recordedFileName_FullPath, wfx);
            }
            catch(Exception _e)
            {
                MessageBox.Show("指定された出力ファイルを開けません。\n"+_recordedFileName_FullPath+"というファイルを作れません\n"+_e.ToString());
                return false;
            }
            if (pcmRecorder == null)
            {
                initialize();
            }
            RecordingState();
            pcmRecorder.Start(ch, hz);
            return true;
        }
        //停止ボタン
        private void StopButton_Click(object sender, EventArgs e)
        {
            stopRecord();
        }
        /// <summary>
        /// マイク録音を停止します。録音したファイルのフルパスを返します。
        /// </summary>
        /// <returns></returns>
        public string stopRecord()
        {
            if (pcmRecorder == null)
            {
                initialize();
            }
            pcmRecorder.Stop();
            ww.Close();
            IdleState();
            return p_recordedFileName_FullPath;
            //return true;
        }
        //ファイル選択ダイアログ
        private void FileDlg_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "WAVE files (*.wav)|*.wav";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                p_recordedFileName_FullPath = saveFileDialog1.FileName;
                if (Path.GetExtension(p_recordedFileName_FullPath) == "")
                    p_recordedFileName_FullPath += ".wav";
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            playSound_LastRecordedd();
        }
        /// <summary>
        /// 最後に録音したファイルの再生 // うまく動かないことが多い
        /// </summary>
        public bool playSound_LastRecordedd()
        {
            if (pcmPlayerForm == null)
            {
                pcmPlayerForm = new SamplePlayer.Form1();
            }
            // うまく動かないことが多い
            return pcmPlayerForm.playSound(p_recordedFileName_FullPath);
        }
    }
}
