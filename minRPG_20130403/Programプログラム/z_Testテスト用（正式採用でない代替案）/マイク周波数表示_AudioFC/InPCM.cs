using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;
using System.Runtime.InteropServices;
using LIB;
using MMSYSTEM;

namespace LIB
{
	class InPCM
	{
        IntPtr hWI = IntPtr.Zero;
        WAVEFORMATEX wfx;
        WaveIn.CALLBACK inCallback;
        //ドライバからの受信をキューイング
        Queue<Int16[]> Q;
        //ドライバとの間のバッファ
        const int NumberOfBuffers = 10;
        int BufferSize;
        PcmInBuffer[] inBuffer;

        public InPCM(int samplingRate, int sample_size)
        {
            BufferSize = sample_size * 2;//バイト数にして保存
            //WaveInのオープン
            inCallback = new WaveIn.CALLBACK(callback);
            wfx = WinMMHelper.WAVEFORMATEX_PCM(1, samplingRate, 16);//16ビット、モノラル
            int err = WaveIn.waveInOpen(out hWI, WaveIn.WAVE_MAPPER, wfx,
                                                 inCallback, (uint)0,
                                                 WaveIn.CALLBACK_FUNCTION);
            if (err != WaveIn.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveIn.waveInGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception("音声入力インタフェースをオープンできませんでした。\n" + str.ToString());
            }
            //バッファの準備
            Q = new Queue<Int16[]>();
            inBuffer = new PcmInBuffer[NumberOfBuffers];
            for (int i = 0; i < NumberOfBuffers; i++)
            {
                inBuffer[i] = new PcmInBuffer(hWI, BufferSize);
                inBuffer[i].AddBuffer();
            }
            //入力開始
            err = WaveIn.waveInStart(hWI);
            if (err != WaveIn.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveIn.waveInGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception("音声入力インタフェースをスタートできませんでした。\n" + str.ToString());
            }
        }
        public void Close()
        {
            if (inBuffer != null)
            {
                int err = WaveIn.waveInStop(hWI);
                for (int i = 0; i < inBuffer.Length; i++)
                {
                    inBuffer[i].Free();
                }
            }
        }
        public void Pause()
        {
            WaveIn.waveInStop(hWI);
            Q.Clear();
        }
        public void Resume()
        {
            Q.Clear();
            WaveIn.waveInStart(hWI);
        }
        public int GetQueueCount()
        {
            return Q.Count;
        }
        public Int16[] Read()
        {
            return Q.Dequeue();
        }
        /// <summary>
        /// WinMM.dllからのコールバック
        /// </summary>
        /// <param name="hdrvr"></param>
        /// <param name="uMsg"></param>
        /// <param name="dwUser"></param>
        /// <param name="wavhdr"></param>
        /// <param name="dwParam2"></param>
        private void callback(IntPtr hdrvr, uint uMsg, uint dwUser,
                                WAVEHDR wavhdr, uint dwParam2)
        {
            if (uMsg == WaveIn.MM_WIM_DATA)
            {
                //必ずバッファフルで通知されるようだ
                GCHandle h = (GCHandle)wavhdr.dwUser;
                PcmInBuffer ib = (PcmInBuffer)h.Target;
                Q.Enqueue(ib.GetShortData());
                ib.AddBuffer();
            }
        }
    }
}
