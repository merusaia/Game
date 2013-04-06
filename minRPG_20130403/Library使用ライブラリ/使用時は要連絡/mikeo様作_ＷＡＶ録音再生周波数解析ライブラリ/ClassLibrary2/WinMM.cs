
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace LIB
{
    //Audioデバイスからの入力（WINMM.DLL呼び出し）
	public  class WaveIn
	{
        public const uint WAVE_MAPPER = (unchecked((uint)-1));
        public const int MMSYSERR_NOERROR = 0;
		public const int MM_WIM_OPEN  = 0x3BE;
		public const int MM_WIM_CLOSE = 0x3BF;
		public const int MM_WIM_DATA  = 0x3C0;
        public const int CALLBACK_FUNCTION = 0x00030000;

		public delegate void CALLBACK(IntPtr hwi,uint uMsg,uint dwUser,
        						MMSYSTEM.WAVEHDR hdr,uint dwParam2);
        //DLL関数
		[DllImport("winmm.dll")]
        public static extern int waveInGetErrorText(int err,
        								StringBuilder text,
                                        uint uSize);
		[DllImport("winmm.dll")]
		public static extern int waveInGetNumDevs();
		[DllImport("winmm.dll")]
        public static extern int waveInGetDevCaps(int index,
                                        ref MMSYSTEM.WAVEINCAPS pwic, uint cbwic);
		[DllImport("winmm.dll")]
		public static extern int waveInAddBuffer(IntPtr hwi,
                                    MMSYSTEM.WAVEHDR pwh, uint cbwh);
		[DllImport("winmm.dll")]
		public static extern int waveInClose(IntPtr hwi);
		[DllImport("winmm.dll")]
		public static extern int waveInOpen(out IntPtr phwi,uint uDeviceID,
                                    MMSYSTEM.WAVEFORMATEX lpFormat, CALLBACK callback,
                                    uint dwInstance, uint dwOpen);
		[DllImport("winmm.dll")]
		public static extern int waveInPrepareHeader(IntPtr hwi,
                                    MMSYSTEM.WAVEHDR pwh, uint uSize);
		[DllImport("winmm.dll")]
		public static extern int waveInUnprepareHeader(IntPtr hwi,
                                    MMSYSTEM.WAVEHDR pwh, uint uSize);
		[DllImport("winmm.dll")]
		public static extern int waveInReset(IntPtr hwi);
		[DllImport("winmm.dll")]
		public static extern int waveInStart(IntPtr hwi);
		[DllImport("winmm.dll")]
		public static extern int waveInStop(IntPtr hwi);
	}

    //Audioデバイスへ出力（WINMM.DLL呼び出し）
	public  class WaveOut
	{
        public const uint WAVE_MAPPER = (unchecked((uint)-1));
        public const uint MMSYSERR_NOERROR = 0;
        public const uint MM_WOM_DONE = 0x3BD;
        public const int CALLBACK_WINDOW = 0x00010000;
        public const int CALLBACK_FUNCTION = 0x00030000;
        public delegate void CALLBACK(IntPtr hwi, uint uMsg, uint dwUser,
                                IntPtr hdr, uint dwParam2);

		[DllImport("winmm.dll")]
        public static extern int waveOutGetErrorText(int err,
        								StringBuilder text,
                                        uint uSize);
		[DllImport("winmm.dll")]
		public static extern int waveOutGetNumDevs();
		[DllImport("winmm.dll")]
        public static extern int waveOutGetDevCaps(int index,
                                        ref MMSYSTEM.WAVEOUTCAPS pwoc, uint cbwoc);
		[DllImport("winmm.dll")]
		public static extern int waveOutPrepareHeader(IntPtr hwo,
                                        MMSYSTEM.WAVEHDR pwo, uint uSize);
		[DllImport("winmm.dll")]
		public static extern int waveOutUnprepareHeader(IntPtr hwo,
                                        MMSYSTEM.WAVEHDR pwo, uint uSize);
		[DllImport("winmm.dll")]
		public static extern int waveOutWrite(IntPtr hwo,
                                        MMSYSTEM.WAVEHDR pwo, uint uSize);
        [DllImport("winmm.dll")]
        public static extern int waveOutOpen(out IntPtr hwo, uint uDeviceID,
                                        MMSYSTEM.WAVEFORMATEX lpFormat,
                                        IntPtr hWnd,
                                        uint dwInstance, uint dwOpen);
        [DllImport("winmm.dll")]
        public static extern int waveOutOpen(out IntPtr hwo, uint uDeviceID,
                                        MMSYSTEM.WAVEFORMATEX lpFormat,
                                        CALLBACK callback,
                                        uint dwInstance, uint dwOpen);
        [DllImport("winmm.dll")]
		public static extern int waveOutReset(IntPtr hwo);
		[DllImport("winmm.dll")]
		public static extern int waveOutClose(IntPtr hwo);
		[DllImport("winmm.dll")]
		public static extern int waveOutPause(IntPtr hwo);
		[DllImport("winmm.dll")]
		public static extern int waveOutRestart(IntPtr hwo);
		[DllImport("winmm.dll")]
		public static extern int waveOutGetPosition(IntPtr hwo,
        								out uint lpInfo,uint uSize);
		[DllImport("winmm.dll")]
		public static extern int waveOutSetVolume(IntPtr hwo,uint dwVolume);
		[DllImport("winmm.dll")]
		public static extern int waveOutGetVolume(IntPtr hwo,out uint dwVolume);
    }
}
