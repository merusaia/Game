
using System;
using System.Text;

using System.Runtime.InteropServices;

//----------------------------------------------------------------------
//MMReg.hより
//----------------------------------------------------------------------
namespace MMSYSTEM
{
    #region Constant
    //wFormatTagの値。圧縮方式等。
    public enum WAVE_FORMAT
    {
        UNKNOWN = 0x0000, /* Microsoft Corporation */
        PCM = 0x0001,
        ADPCM = 0x0002, /* Microsoft Corporation */
        IEEE_FLOAT = 0x0003, /* Microsoft Corporation */
        VSELP = 0x0004, /* Compaq Computer Corp. */
        IBM_CVSD = 0x0005, /* IBM Corporation */
        ALAW = 0x0006, /* Microsoft Corporation */
        MULAW = 0x0007, /* Microsoft Corporation */
        DTS = 0x0008, /* Microsoft Corporation */
        DRM = 0x0009, /* Microsoft Corporation */
        OKI_ADPCM = 0x0010, /* OKI */
        DVI_ADPCM = 0x0011, /* Intel Corporation */
        IMA_ADPCM = DVI_ADPCM, /*  Intel Corporation */
        MEDIASPACE_ADPCM = 0x0012, /* Videologic */
        SIERRA_ADPCM = 0x0013, /* Sierra Semiconductor Corp */
        G723_ADPCM = 0x0014, /* Antex Electronics Corporation */
        DIGISTD = 0x0015, /* DSP Solutions, Inc. */
        DIGIFIX = 0x0016, /* DSP Solutions, Inc. */
        DIALOGIC_OKI_ADPCM = 0x0017, /* Dialogic Corporation */
        MEDIAVISION_ADPCM = 0x0018, /* Media Vision, Inc. */
        CU_CODEC = 0x0019, /* Hewlett-Packard Company */
        YAMAHA_ADPCM = 0x0020, /* Yamaha Corporation of America */
        SONARC = 0x0021, /* Speech Compression */
        DSPGROUP_TRUESPEECH = 0x0022, /* DSP Group, Inc */
        ECHOSC1 = 0x0023, /* Echo Speech Corporation */
        AUDIOFILE_AF36 = 0x0024, /* Virtual Music, Inc. */
        APTX = 0x0025, /* Audio Processing Technology */
        AUDIOFILE_AF10 = 0x0026, /* Virtual Music, Inc. */
        PROSODY_1612 = 0x0027, /* Aculab plc */
        LRC = 0x0028, /* Merging Technologies S.A. */
        DOLBY_AC2 = 0x0030, /* Dolby Laboratories */
        GSM610 = 0x0031, /* Microsoft Corporation */
        MSNAUDIO = 0x0032, /* Microsoft Corporation */
        ANTEX_ADPCME = 0x0033, /* Antex Electronics Corporation */
        CONTROL_RES_VQLPC = 0x0034, /* Control Resources Limited */
        DIGIREAL = 0x0035, /* DSP Solutions, Inc. */
        DIGIADPCM = 0x0036, /* DSP Solutions, Inc. */
        CONTROL_RES_CR10 = 0x0037, /* Control Resources Limited */
        NMS_VBXADPCM = 0x0038, /* Natural MicroSystems */
        CS_IMAADPCM = 0x0039, /* Crystal Semiconductor IMA ADPCM */
        ECHOSC3 = 0x003A, /* Echo Speech Corporation */
        ROCKWELL_ADPCM = 0x003B, /* Rockwell International */
        ROCKWELL_DIGITALK = 0x003C, /* Rockwell International */
        XEBEC = 0x003D, /* Xebec Multimedia Solutions Limited */
        G721_ADPCM = 0x0040, /* Antex Electronics Corporation */
        G728_CELP = 0x0041, /* Antex Electronics Corporation */
        MSG723 = 0x0042, /* Microsoft Corporation */
        MPEG = 0x0050, /* Microsoft Corporation */
        RT24 = 0x0052, /* InSoft, Inc. */
        PAC = 0x0053, /* InSoft, Inc. */
        MPEGLAYER3 = 0x0055, /* ISO/MPEG Layer3 Format Tag */
        LUCENT_G723 = 0x0059, /* Lucent Technologies */
        CIRRUS = 0x0060, /* Cirrus Logic */
        ESPCM = 0x0061, /* ESS Technology */
        VOXWARE = 0x0062, /* Voxware Inc */
        CANOPUS_ATRAC = 0x0063, /* Canopus, co., Ltd. */
        G726_ADPCM = 0x0064, /* APICOM */
        G722_ADPCM = 0x0065, /* APICOM */
        DSAT_DISPLAY = 0x0067, /* Microsoft Corporation */
        VOXWARE_BYTE_ALIGNED = 0x0069, /* Voxware Inc */
        VOXWARE_AC8 = 0x0070, /* Voxware Inc */
        VOXWARE_AC10 = 0x0071, /* Voxware Inc */
        VOXWARE_AC16 = 0x0072, /* Voxware Inc */
        VOXWARE_AC20 = 0x0073, /* Voxware Inc */
        VOXWARE_RT24 = 0x0074, /* Voxware Inc */
        VOXWARE_RT29 = 0x0075, /* Voxware Inc */
        VOXWARE_RT29HW = 0x0076, /* Voxware Inc */
        VOXWARE_VR12 = 0x0077, /* Voxware Inc */
        VOXWARE_VR18 = 0x0078, /* Voxware Inc */
        VOXWARE_TQ40 = 0x0079, /* Voxware Inc */
        SOFTSOUND = 0x0080, /* Softsound, Ltd. */
        VOXWARE_TQ60 = 0x0081, /* Voxware Inc */
        MSRT24 = 0x0082, /* Microsoft Corporation */
        G729A = 0x0083, /* AT&T Labs, Inc. */
        MVI_MVI2 = 0x0084, /* Motion Pixels */
        DF_G726 = 0x0085, /* DataFusion Systems (Pty) (Ltd) */
        DF_GSM610 = 0x0086, /* DataFusion Systems (Pty) (Ltd) */
        ISIAUDIO = 0x0088, /* Iterated Systems, Inc. */
        ONLIVE = 0x0089, /* OnLive! Technologies, Inc. */
        SBC24 = 0x0091, /* Siemens Business Communications Sys */
        DOLBY_AC3_SPDIF = 0x0092, /* Sonic Foundry */
        MEDIASONIC_G723 = 0x0093, /* MediaSonic */
        PROSODY_8KBPS = 0x0094, /* Aculab plc */
        ZYXEL_ADPCM = 0x0097, /* ZyXEL Communications, Inc. */
        PHILIPS_LPCBB = 0x0098, /* Philips Speech Processing */
        PACKED = 0x0099, /* Studer Professional Audio AG */
        MALDEN_PHONYTALK = 0x00A0, /* Malden Electronics Ltd. */
        RHETOREX_ADPCM = 0x0100, /* Rhetorex Inc. */
        IRAT = 0x0101, /* BeCubed Software Inc. */
        VIVO_G723 = 0x0111, /* Vivo Software */
        VIVO_SIREN = 0x0112, /* Vivo Software */
        DIGITAL_G723 = 0x0123, /* Digital Equipment Corporation */
        SANYO_LD_ADPCM = 0x0125, /* Sanyo Electric Co., Ltd. */
        SIPROLAB_ACEPLNET = 0x0130, /* Sipro Lab Telecom Inc. */
        SIPROLAB_ACELP4800 = 0x0131, /* Sipro Lab Telecom Inc. */
        SIPROLAB_ACELP8V3 = 0x0132, /* Sipro Lab Telecom Inc. */
        SIPROLAB_G729 = 0x0133, /* Sipro Lab Telecom Inc. */
        SIPROLAB_G729A = 0x0134, /* Sipro Lab Telecom Inc. */
        SIPROLAB_KELVIN = 0x0135, /* Sipro Lab Telecom Inc. */
        G726ADPCM = 0x0140, /* Dictaphone Corporation */
        QUALCOMM_PUREVOICE = 0x0150, /* Qualcomm, Inc. */
        QUALCOMM_HALFRATE = 0x0151, /* Qualcomm, Inc. */
        TUBGSM = 0x0155, /* Ring Zero Systems, Inc. */
        MSAUDIO1 = 0x0160, /* Microsoft Corporation */
        UNISYS_NAP_ADPCM = 0x0170, /* Unisys Corp. */
        UNISYS_NAP_ULAW = 0x0171, /* Unisys Corp. */
        UNISYS_NAP_ALAW = 0x0172, /* Unisys Corp. */
        UNISYS_NAP_16K = 0x0173, /* Unisys Corp. */
        CREATIVE_ADPCM = 0x0200, /* Creative Labs, Inc */
        CREATIVE_FASTSPEECH8 = 0x0202, /* Creative Labs, Inc */
        CREATIVE_FASTSPEECH10 = 0x0203, /* Creative Labs, Inc */
        UHER_ADPCM = 0x0210, /* UHER informatic GmbH */
        QUARTERDECK = 0x0220, /* Quarterdeck Corporation */
        ILINK_VC = 0x0230, /* I-link Worldwide */
        RAW_SPORT = 0x0240, /* Aureal Semiconductor */
        ESST_AC3 = 0x0241, /* ESS Technology, Inc. */
        IPI_HSX = 0x0250, /* Interactive Products, Inc. */
        IPI_RPELP = 0x0251, /* Interactive Products, Inc. */
        CS2 = 0x0260, /* Consistent Software */
        SONY_SCX = 0x0270, /* Sony Corp. */
        FM_TOWNS_SND = 0x0300, /* Fujitsu Corp. */
        BTV_DIGITAL = 0x0400, /* Brooktree Corporation */
        QDESIGN_MUSIC = 0x0450, /* QDesign Corporation */
        VME_VMPCM = 0x0680, /* AT&T Labs, Inc. */
        TPC = 0x0681, /* AT&T Labs, Inc. */
        OLIGSM = 0x1000, /* Ing C. Olivetti & C., S.p.A. */
        OLIADPCM = 0x1001, /* Ing C. Olivetti & C., S.p.A. */
        OLICELP = 0x1002, /* Ing C. Olivetti & C., S.p.A. */
        OLISBC = 0x1003, /* Ing C. Olivetti & C., S.p.A. */
        OLIOPR = 0x1004, /* Ing C. Olivetti & C., S.p.A. */
        LH_CODEC = 0x1100, /* Lernout & Hauspie */
        NORRIS = 0x1400, /* Norris Communications, Inc. */
        SOUNDSPACE_MUSICOMPRESS = 0x1500, /* AT&T Labs, Inc. */
        DVM = 0x2000  /* FAST Multimedia AG */
    };
	//WAVEINCAPS,WAVEOUTCAPSのdwFormatsの値
    public class CapsFormats
    {
        public const int WAVE_FORMAT_1M08 = 0x00000001; /* 11.025 kHz, Mono,   8-bit  */
        public const int WAVE_FORMAT_1S08 = 0x00000002; /* 11.025 kHz, Stereo, 8-bit  */
        public const int WAVE_FORMAT_1M16 = 0x00000004; /* 11.025 kHz, Mono,   16-bit */
        public const int WAVE_FORMAT_1S16 = 0x00000008; /* 11.025 kHz, Stereo, 16-bit */
        public const int WAVE_FORMAT_2M08 = 0x00000010; /* 22.05  kHz, Mono,   8-bit  */
        public const int WAVE_FORMAT_2S08 = 0x00000020; /* 22.05  kHz, Stereo, 8-bit  */
        public const int WAVE_FORMAT_2M16 = 0x00000040; /* 22.05  kHz, Mono,   16-bit */
        public const int WAVE_FORMAT_2S16 = 0x00000080; /* 22.05  kHz, Stereo, 16-bit */
        public const int WAVE_FORMAT_4M08 = 0x00000100; /* 44.1   kHz, Mono,   8-bit  */
        public const int WAVE_FORMAT_4S08 = 0x00000200; /* 44.1   kHz, Stereo, 8-bit  */
        public const int WAVE_FORMAT_4M16 = 0x00000400; /* 44.1   kHz, Mono,   16-bit */
        public const int WAVE_FORMAT_4S16 = 0x00000800; /* 44.1   kHz, Stereo, 16-bit */

        public const int WAVE_FORMAT_44M08 = 0x00000100; /* 44.1   kHz, Mono,   8-bit  */
        public const int WAVE_FORMAT_44S08 = 0x00000200; /* 44.1   kHz, Stereo, 8-bit  */
        public const int WAVE_FORMAT_44M16 = 0x00000400; /* 44.1   kHz, Mono,   16-bit */
        public const int WAVE_FORMAT_44S16 = 0x00000800; /* 44.1   kHz, Stereo, 16-bit */
        public const int WAVE_FORMAT_48M08 = 0x00001000; /* 48     kHz, Mono,   8-bit  */
        public const int WAVE_FORMAT_48S08 = 0x00002000; /* 48     kHz, Stereo, 8-bit  */
        public const int WAVE_FORMAT_48M16 = 0x00004000; /* 48     kHz, Mono,   16-bit */
        public const int WAVE_FORMAT_48S16 = 0x00008000; /* 48     kHz, Stereo, 16-bit */
        public const int WAVE_FORMAT_96M08 = 0x00010000; /* 96     kHz, Mono,   8-bit  */
        public const int WAVE_FORMAT_96S08 = 0x00020000; /* 96     kHz, Stereo, 8-bit  */
        public const int WAVE_FORMAT_96M16 = 0x00040000; /* 96     kHz, Mono,   16-bit */
        public const int WAVE_FORMAT_96S16 = 0x00080000; /* 96     kHz, Stereo, 16-bit */
    };
    //WAVEOUTCAPSのdwSupportの値
    public class CapsSuport
    {
        public const int WAVECAPS_PITCH = 0x0001;/* supports pitch control */
        public const int WAVECAPS_PLAYBACKRATE = 0x0002;/* supports playback rate control */
        public const int WAVECAPS_VOLUME = 0x0004;/* supports volume control */
        public const int WAVECAPS_LRVOLUME = 0x0008;/* separate left-right volume control */
        public const int WAVECAPS_SYNC = 0x0010;
        public const int WAVECAPS_SAMPLEACCURATE = 0x0020;
    };
    #endregion
    #region Structure
    //Wave形式Audioデータ構造
    [StructLayout(LayoutKind.Sequential, Pack=2)]
    public class WAVEFORMATEX    //public struct WAVEFORMATEX
    {
        public UInt16 wFormatTag;
        public UInt16 nChannels;
        public UInt32 nSamplesPerSec;
        public UInt32 nAvgBytesPerSec;
        public UInt16 nBlockAlign;
        public UInt16 wBitsPerSample;
        public UInt16 cbSize;
    }
    //Wave形式Audioデータ構造(MP3)
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class WAVEFORMATEX_MP3
    {
        public UInt16 wFormatTag;
        public UInt16 nChannels;
        public UInt32 nSamplesPerSec;
        public UInt32 nAvgBytesPerSec;
        public UInt16 nBlockAlign;
        public UInt16 wBitsPerSample;
        public UInt16 cbSize;
        public UInt16 wID;
        public UInt32 fdwFlags;
        public UInt16 nBlockSize;
        public UInt16 nFramesPerBlock;
        public UInt16 nCodecDelay;

        public const int WFX_EXTRA_BYTES = 12;
        public const int ID_MPEG = 1;
        public const int FLAG_PADDING_OFF = 0x00000002;
    }
   	//Wave形式ヘッダ構造体
	[StructLayout(LayoutKind.Sequential)]
    public class WAVEHDR
	{
		public IntPtr	lpData;
		public uint		dwBufferLength;
		public uint		dwBytesRecorded;
		public IntPtr	dwUser;
		public uint		dwFlags;
		public uint		dwLoops;
		public IntPtr	lpNext;
		public uint		reserved;
	}
    //Wave In デバイス定義構造体
    [StructLayout(LayoutKind.Sequential)]
	public struct WAVEINCAPS
    {
        public short	wMid;
        public short	wPid;
        public uint		vDriverVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string	szPname;
        public uint     dwFormats;
        public short	wChannels;
        public short	wReserved1;
    }
	[StructLayout(LayoutKind.Sequential)]
	public struct WAVEOUTCAPS
    {
        public short	wMid;
        public short	wPid;
        public uint		vDriverVersion;
		[MarshalAs(UnmanagedType.ByValTStr,SizeConst=32)]
        public string	szPname;
        public uint     dwFormats;
        public short	wChannels;
        public short	wReserved1;
	    public uint		dwSupport;
    }
    #endregion
}
