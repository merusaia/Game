using System;
using System.Text;

using MMSYSTEM;

namespace LIB
{
    public class PCMPlayer
    {
        public struct VOLUME
        {
            public ushort left;
            public ushort right;
        }

        //バッファの大きさ。1秒分を3面確保。
        readonly int bufferSize;  //バイト数
        readonly int numberOfBuffer = 3;
        PcmOutBuffer[] outBuffer;

        IntPtr hWO = IntPtr.Zero;//WinMMの出力用ハンドル
  
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hWnd">Formのハンドル</param>
        /// <param name="format">PCM形式を示すWAVEFORMATEX</param>
        public PCMPlayer(IntPtr hWnd, WAVEFORMATEX format)
        {
            //1秒分のバッファサイズ計算
            bufferSize = (int)(format.wBitsPerSample / 8
                             * format.nChannels
                             * format.nSamplesPerSec);
            //WimMM Open()
            int err = WaveOut.waveOutOpen(out hWO, WaveOut.WAVE_MAPPER, format,
                                            hWnd, (uint)0,
                                            WaveOut.CALLBACK_WINDOW);
            if (err != WaveOut.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveOut.waveOutGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception(str.ToString());
            }
            outBuffer = new PcmOutBuffer[numberOfBuffer];
            for (int i = 0; i < outBuffer.Length; i++)
            {
                outBuffer[i] = new PcmOutBuffer(hWO, bufferSize);
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hWnd">Formのハンドル</param>
        /// <param name="format">PCM形式を示すWAVEFORMATEX</param>
        /// <param name="bufSize">バッファのサイズ</param>
        /// <param name="numberOfBuffer">バッファ数</param>
        public PCMPlayer(IntPtr hWnd, WAVEFORMATEX format, int bufSize, int numberOfBuffer)
        {
            this.bufferSize = bufSize;
            this.numberOfBuffer = numberOfBuffer;
            int err = WaveOut.waveOutOpen(out hWO, WaveOut.WAVE_MAPPER, format,
                                            hWnd, (uint)0,
                                            WaveOut.CALLBACK_WINDOW);
            if (err != WaveOut.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveOut.waveOutGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception(str.ToString());
            }
            outBuffer = new PcmOutBuffer[numberOfBuffer];
            for (int i = 0; i < outBuffer.Length; i++)
            {
                outBuffer[i] = new PcmOutBuffer(hWO, bufferSize);
            }
        }
        /// <summary>
        /// バッファを開放します。
        /// </summary>
        public void Close()
        {
            for (int i = 0; i < outBuffer.Length; i++)
            {
                outBuffer[i].Free();
            }
        }
        /// <summary>
        /// 全バッファが空いたらtrueを返す
        /// </summary>
        /// <returns>全バッファが空いたらtrue</returns>
        public bool CheckDone()
        {
            for (int i = 0; i < outBuffer.Length; i++)
            {
                if (!outBuffer[i].Done) return false;
            }
            return true;//すべてのバッファが停止した
        }
        //音量取得
        public VOLUME GetVolume()
        {
            uint vol;
            int err = WaveOut.waveOutGetVolume(hWO, out vol);
            if (err != WaveOut.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveOut.waveOutGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception(str.ToString());
            }
            VOLUME V;
            V.left = (ushort)(vol & 0xffff);
            V.right = (ushort)(vol >> 16);
            return V;
        }
        //音量設定
        public void SetVolume(int left, int right)
        {
            uint vol = (((uint)right) << 16) | (uint)left;
            int err = WaveOut.waveOutSetVolume(hWO, (uint)vol);
            if (err != WaveOut.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveOut.waveOutGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception(str.ToString());
            }
        }
        /// <summary>
        /// 再生を開始します
        /// </summary>
        public void Start()
        {
            byte[] pcm = new byte[0];
            for (int i = 0; i < outBuffer.Length; i++)
            {
                outBuffer[i].Write(pcm);
            }
        }
        /// <summary>
        /// 再生を中止します
        /// </summary>
        public void Stop()
        {
            WaveOut.waveOutReset(hWO);
        }
        /// <summary>
        /// 再生を一時停止します
        /// </summary>
        public void Pause()
        {
            WaveOut.waveOutPause(hWO);
        }
        /// <summary>
        /// 一時停止を解除します
        /// </summary>
        public void Resume()
        {
            WaveOut.waveOutRestart(hWO);
        }
    }
}
