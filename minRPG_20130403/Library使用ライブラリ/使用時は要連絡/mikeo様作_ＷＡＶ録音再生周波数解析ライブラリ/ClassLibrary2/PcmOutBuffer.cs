using System;
using System.Text;

using System.Runtime.InteropServices;
using MMSYSTEM;

namespace LIB
{
    public class PcmOutBuffer
    {
        GCHandle hWavehdr;
        GCHandle hData;
		WAVEHDR wavehdr;
        byte[]  data;
        IntPtr hWO;
        int size;   //data[]のバイト数

        bool bDone;//再生完了、停止操作で、trueをセット

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hWO"></param>
        /// <param name="size"></param>
        public PcmOutBuffer(IntPtr hWO, int size)
		{
        	this.hWO = hWO;
            this.size = size;

            bDone = false;

            data = new byte[size];
            hData = GCHandle.Alloc(data, GCHandleType.Pinned);
        	wavehdr = new WAVEHDR();
            wavehdr.dwUser = (IntPtr)GCHandle.Alloc(this);
            wavehdr.lpData = hData.AddrOfPinnedObject();
        	wavehdr.dwBufferLength = (uint)0;
            wavehdr.dwFlags = 0;
            hWavehdr = GCHandle.Alloc(wavehdr, GCHandleType.Pinned);

            int err = WaveOut.waveOutPrepareHeader(hWO, wavehdr,	(uint)Marshal.SizeOf(wavehdr));
            if( err!=WaveOut.MMSYSERR_NOERROR )
            {
            	StringBuilder str = new StringBuilder(256);
                WaveOut.waveOutGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception(str.ToString());
            }
		}
        //プロパティ
        public WAVEHDR Wavehdr
        {
            get { return wavehdr; }
        }
        public int Size
        {
            get { return size; }
        }
        public bool Done
        {
            get { return bDone; }
        }
        /// <summary>
        /// これ以上データをセットしないことを設定する。
        /// WinProcで、WOM_DONEを受け取ったが、データをセットしなかったことを示す。
        /// </summary>
        public void SetDone()
        {
            bDone = true;
        }
		//領域開放
        public void Free()
        {

            WaveOut.waveOutUnprepareHeader(hWO, wavehdr,
                						(uint)Marshal.SizeOf(wavehdr));
            hData.Free();
            hWavehdr.Free();
        }
        //------------------------------------------------------------------
        /// <summary>
        /// WAVEHDRのポインタからこのバッファを取得する。
        /// </summary>
        /// <param name="pWavhdr">WAVEHDRのポインタ</param>
        /// <returns>PcmOutBuffer</returns>
        public static PcmOutBuffer GetPcmOutBufferFromHeader(IntPtr pWavhdr)
        {
            WAVEHDR wavehdr = new WAVEHDR();
            Marshal.PtrToStructure(pWavhdr, wavehdr);
            GCHandle h = (GCHandle)wavehdr.dwUser;
            return (PcmOutBuffer)h.Target;
        }
        /// <summary>
        /// PCMデータの出力
        /// </summary>
        /// <param name="len">バイト数</param>
        void Write(int len)
        {
            wavehdr.dwBufferLength = (uint)(len);
            int err = WaveOut.waveOutWrite(hWO, wavehdr,
                                        (uint)Marshal.SizeOf(wavehdr));
            if (err != WaveOut.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveOut.waveOutGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception(str.ToString());
            }
        }
        /// <summary>
        /// PCMデータの出力（FormのWinProc()から呼び出す）
        /// </summary>
        /// <param name="pcm">出力データ(byte[])</param>
        public void Write(byte[] pcm)
        {
            Array.Copy(pcm, this.data, pcm.Length);
            Write(pcm.Length);
        }
        /// <summary>
        /// PCMデータの出力（FormのWinProc()から呼び出す）
        /// </summary>
        /// <param name="pcm">出力データ(byte[])</param>
        /// <param name="pos">pcmの開始位置</param>
        /// <param name="len">pcmの開始位置からのバイト数</param>
        public void Write(byte[] pcm, int pos, int len)
        {
            Array.Copy(pcm, pos, this.data, 0, len);
            Write(len);
        }
        /// <summary>
        /// PCMデータの出力（FormのWinProc()から呼び出す）
        /// </summary>
        /// <param name="pcm">出力データ(Int16[])</param>
        public void Write(Int16[] pcm)
        {
            int dest = 0;
            for (int i = 0; i < pcm.Length; i++)
            {
                this.data[dest++] = (byte)((UInt16)pcm[i]);
                this.data[dest++] = (byte)(((UInt16)pcm[i]) >> 8);
            }
            Write(dest);
        }
        /// <summary>
        /// PCMデータの出力（FormのWinProc()から呼び出す）
        /// </summary>
        /// <param name="pcm">出力データ(Int16[])</param>
        /// <param name="pos">pcmの開始位置</param>
        /// <param name="len">pcmの開始位置からのバイト数</param>
        public void Write(Int16[] pcm, int pos, int len)
        {
            int dest = 0;
            for (int i = 0; i < len; i++)
            {

                this.data[dest++] = (byte)((UInt16)pcm[pos + i]);
                this.data[dest++] = (byte)(((UInt16)pcm[pos + i]) >> 8);
            }
            Write(dest);
        }
    }
}
