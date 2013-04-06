using System;
using System.Text;

using System.Runtime.InteropServices;
using MMSYSTEM;

namespace LIB
{
    public class PcmInBuffer
    {
        GCHandle hWavehdr;
        GCHandle hData;
        IntPtr hWI;
        int size;
        WAVEHDR wavehdr;
        byte[] data;
        //コンストラクタ
        public PcmInBuffer(IntPtr hWI, int size)
        {
            this.hWI = hWI;
            this.size = size;
            //WAVEHDRを格納する領域を割り当てる
            wavehdr = new WAVEHDR();
            hWavehdr = GCHandle.Alloc(wavehdr, GCHandleType.Pinned);
            //実際にデータを格納するバッファを割り当てる
            data = new byte[size];
            hData = GCHandle.Alloc(data, GCHandleType.Pinned);
            //wavehdrにバッファ情報をセット
            wavehdr.dwUser = (IntPtr)GCHandle.Alloc(this);
            wavehdr.lpData = hData.AddrOfPinnedObject();
            wavehdr.dwBufferLength = (uint)size;
            wavehdr.dwFlags = 0;
            //ヘッダ準備要求。ドライバには、wavehdrのアドレスのみ渡る。
            //コールバック時には、wavehdr.dwUser.Targetからクラスオブジェクトにアクセスできる。
            int err = WaveIn.waveInPrepareHeader(hWI, wavehdr, (uint)Marshal.SizeOf(wavehdr));
            if (err != WaveIn.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveIn.waveInGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception(str.ToString());
            }
        }
        //解放
        public void Free()
        {
            hWavehdr.Free();
            hData.Free();
        }
        //バッファ登録
        public void AddBuffer()
        {
            int err;
            err = WaveIn.waveInAddBuffer(hWI, wavehdr, (uint)Marshal.SizeOf(wavehdr));
            if (err != WaveIn.MMSYSERR_NOERROR)
            {
                StringBuilder str = new StringBuilder(256);
                WaveIn.waveInGetErrorText(err, str, (uint)str.Capacity);
                throw new Exception(str.ToString());
            }
        }
        public override string ToString()
        {
            return wavehdr.dwBytesRecorded.ToString() + "/" + wavehdr.dwBufferLength.ToString();
        }
        //WAVEデータをshort[]で取得
        public short[] GetShortData()
        {
            int size = (int)(wavehdr.dwBytesRecorded / 2);
            short[] data = new short[size];
            Marshal.Copy(wavehdr.lpData, data, 0, size);
            return data;
        }
        //WAVEデータをbyte[]で取得
        public byte[] GetByteData()
        {
            int size = (int)wavehdr.dwBytesRecorded;
            byte[] data = new byte[size];
            Marshal.Copy(wavehdr.lpData, data, 0, size);
            return data;
        }
    }
}
