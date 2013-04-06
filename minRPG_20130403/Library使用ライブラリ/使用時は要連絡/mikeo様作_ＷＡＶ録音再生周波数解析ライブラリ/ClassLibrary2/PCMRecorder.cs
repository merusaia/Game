using System;
using System.Text;

using System.Windows.Forms;
using System.Runtime.InteropServices;
using MMSYSTEM;

namespace LIB
{
    public class PCMRecorder
    {
        Form form;
        IPCMRecorder ipr;
        IPCMRecorder2 ipr2;

        IntPtr hWI = IntPtr.Zero;
        WAVEFORMATEX wfx;
        WaveIn.CALLBACK inCallback;

        //ドライバとの間のバッファ
        readonly int NumberOfBuffers = 10;
        readonly int BufferSize = 8192;//16ビット、モノラル、44100Hzで93ms弱。
        PcmInBuffer[] inBuffer = null;

        bool bDone = false;

        /// <summary>
        /// コンストラクタ（byte[]型）
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ipr"></param>
        public PCMRecorder(Form form, IPCMRecorder ipr)
        {
            this.form = form;
            this.ipr = ipr;
            this.ipr2 = null;
        }
        /// <summary>
        /// コンストラクタ（Int16[]型）
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ipr2"></param>
        public PCMRecorder(Form form, IPCMRecorder2 ipr2)
        {
            this.form = form;
            this.ipr = null;
            this.ipr2 = ipr2;
        }
        /// <summary>
        /// コンストラクタ（Int16[]型）
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ipr2"></param>
        /// <param name="bufSize"></param>
        /// <param name="numberOfBuf"></param>
        public PCMRecorder(Form form, IPCMRecorder2 ipr2, int bufSize, int numberOfBuf)
        {
            this.form = form;
            this.ipr = null;
            this.ipr2 = ipr2;
            this.BufferSize = bufSize;
            this.NumberOfBuffers = numberOfBuf;
        }
        public void Close()
        {
            if (inBuffer != null)
            {
                if (!bDone) Stop();
                int err = WaveIn.waveInStop(hWI);
                for (int i = 0; i < inBuffer.Length; i++)
                {
                    inBuffer[i].Free();
                }
            }
        }
        public void Start(int ch, int hz)
        {
            //WaveInのオープン
            inCallback = new WaveIn.CALLBACK(callback);
            wfx = WinMMHelper.WAVEFORMATEX_PCM(ch, hz, 16);//16ビット、モノラル
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
            Close();
            bDone = false;
            inBuffer = new PcmInBuffer[NumberOfBuffers];
            for (int i = 0; i < NumberOfBuffers; i++)
            {
                inBuffer[i] = new PcmInBuffer(hWI, BufferSize);
                inBuffer[i].AddBuffer();
            }
            err = WaveIn.waveInStart(hWI);
            if (err != WaveIn.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveIn.waveInGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception("音声入力インタフェースをスタートできませんでした。\n" + str.ToString());
            }
        }
        public void Stop()
        {
            bDone = true;
            int err = WaveIn.waveInStop(hWI);
            if (err != WaveIn.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveIn.waveInGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception("音声入力インタフェースを停止できませんでした。\n" + str.ToString());
            }
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
            if (!bDone)
            {
                if (uMsg == WaveIn.MM_WIM_DATA)
                {
                    try
                    {
                        GCHandle h = (GCHandle)wavhdr.dwUser;
                        PcmInBuffer ib = (PcmInBuffer)h.Target;
                        if (ipr != null)
                            form.Invoke((MethodInvoker)delegate() { ipr.PCMData(ib.GetByteData()); });
                        if (ipr2 != null)
                            form.Invoke((MethodInvoker)delegate() { ipr2.PCMData(ib.GetShortData()); });
                        ib.AddBuffer();
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
