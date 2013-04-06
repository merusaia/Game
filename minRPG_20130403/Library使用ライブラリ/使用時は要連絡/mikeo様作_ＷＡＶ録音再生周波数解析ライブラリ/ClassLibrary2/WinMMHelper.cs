using System;
using System.Collections.Generic;

using System.Reflection;
using System.Runtime.InteropServices;

namespace LIB
{
    public class WinMMHelper
    {
        //PCMの詳細を定義
        //PCMには、符号有り無しやバイトオーダーによって多数の形式があるらしいが、Windowsで使われるものを定義。
        //wFormatTagとwBitsPerSampleから
        public enum PCM_FORMAT
        {
            IEEE_FLOAT, S8, S16LE, S24LE, S32LE
        };
        /// <summary>
        /// 単純なPCM用のWAVEFORMATEXを作成する
        /// </summary>
        /// <param name="nChannels">1:mono / 2:stereo</param>
        /// <param name="nSamplesPerSec">サンプリング周期(Hz)</param>
        /// <param name="wBitsPerSample">1サンプルのビット数(8,16bit)</param>
        /// <returns></returns>
        public static MMSYSTEM.WAVEFORMATEX
            WAVEFORMATEX_PCM(int nChannels, int nSamplesPerSec, int wBitsPerSample)
        {
            MMSYSTEM.WAVEFORMATEX format = new MMSYSTEM.WAVEFORMATEX();
            format.wFormatTag = (ushort)MMSYSTEM.WAVE_FORMAT.PCM;
            format.nChannels = (ushort)nChannels;
            format.nSamplesPerSec = (uint)nSamplesPerSec;
            format.wBitsPerSample = (ushort)wBitsPerSample;
            format.nBlockAlign = (ushort)(nChannels * wBitsPerSample / 8);
            format.nAvgBytesPerSec = (uint)(nSamplesPerSec * format.nBlockAlign);
            format.cbSize = 0;
            return format;
        }
        /// <summary>
        /// 単純なMP3用のWAVEFORMATEXを作成する
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="samplesPerSec"></param>
        /// <param name="bitRate"></param>
        /// <returns></returns>
        public static MMSYSTEM.WAVEFORMATEX_MP3 
            WAVEFORMATEX_MP3(int channels, int samplesPerSec, int bitRate)
        {
            MMSYSTEM.WAVEFORMATEX_MP3 format = new MMSYSTEM.WAVEFORMATEX_MP3();
            format.wFormatTag = (ushort)MMSYSTEM.WAVE_FORMAT.MPEGLAYER3;
            format.nChannels = (ushort)channels;
            format.nSamplesPerSec = (uint)samplesPerSec;
            format.nAvgBytesPerSec = (uint)(bitRate / 8);
            format.nBlockAlign = 1;
            format.wBitsPerSample = 0;
            format.cbSize = MMSYSTEM.WAVEFORMATEX_MP3.WFX_EXTRA_BYTES;
            format.wID = MMSYSTEM.WAVEFORMATEX_MP3.ID_MPEG;
            format.fdwFlags = MMSYSTEM.WAVEFORMATEX_MP3.FLAG_PADDING_OFF;
            format.nBlockSize = (ushort)(144 * bitRate / samplesPerSec);
            format.nFramesPerBlock = 1;
            format.nCodecDelay = 0;
            return format;
        }
        /// <summary>
        /// wFormatTagに対応したWAVE_FORMATの定義文字列を返す。
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatName(int format)
        {
            FieldInfo[] fis = typeof(MMSYSTEM.WAVE_FORMAT).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo fi in fis)
            {
                if ((int)fi.GetRawConstantValue() == format) return fi.Name;
            }
            return format.ToString("x");
        }
        /// <summary>
        /// WAVEFORMATEXとWAVEFORMATEX_MP3からWAVEFORMATEXを取り出す。
        /// </summary>
        /// <param name="pbFormat"></param>
        /// <returns></returns>
        public static MMSYSTEM.WAVEFORMATEX GetWaveFormatEx(object pbFormat)
        {
            MMSYSTEM.WAVEFORMATEX wfx = new MMSYSTEM.WAVEFORMATEX();
            string[] fieldNames = WaveFormatExFields();
            foreach (string name in fieldNames)
            {
                CopyFieldValue(wfx, name, pbFormat);
            }
            return wfx;
        }
        public static void CopyFieldValue(object dest, string name, object src)
        {
            FieldInfo fi_dest = dest.GetType().GetField(name);
            FieldInfo fi_src = src.GetType().GetField(name);
            if ((fi_dest == null) || (fi_src == null))
                throw new Exception("Undefined field name :" + name);
            fi_dest.SetValue(dest, fi_src.GetValue(src));
        }
        public static string[] WaveFormatExFields()
        {
            List<string> list = new List<string>();
            FieldInfo[] fis = typeof(MMSYSTEM.WAVEFORMATEX).GetFields();
            foreach (FieldInfo fi in fis)
            {
                if (!fi.IsLiteral)
                {
                    list.Add(fi.Name);
                }
            }
            return list.ToArray();
        }
        /// <summary>
        /// 入力デバイス数を返す
        /// </summary>
        /// <returns>入力デバイス数</returns>
        public static int GetNumInDevs()
        {
            return WaveIn.waveInGetNumDevs();
        }
        /// <summary>
        /// 入力デバイスの性能情報を返す
        /// </summary>
        /// <param name="index">デバイスを選択する順番</param>
        /// <returns>性能情報</returns>
        public static MMSYSTEM.WAVEINCAPS GetInDevCaps(int index)
        {
            MMSYSTEM.WAVEINCAPS pwic = new MMSYSTEM.WAVEINCAPS();//WAVEINCAPSは、struct
            int err = WaveIn.waveInGetDevCaps(index, ref pwic, (uint)Marshal.SizeOf(typeof(MMSYSTEM.WAVEINCAPS)));
            return pwic;
        }
        /// <summary>
        /// 出力デバイス数を返す
        /// </summary>
        /// <returns>出力デバイス数</returns>
        public static int GetNumOutDevs()
        {
            return WaveOut.waveOutGetNumDevs();
        }
        /// <summary>
        /// 出力デバイスの性能情報を返す
        /// </summary>
        /// <param name="index"></param>
        /// <returns>性能情報</returns>
        public static MMSYSTEM.WAVEOUTCAPS GetOutDevCaps(int index)
        {
            MMSYSTEM.WAVEOUTCAPS pwoc = new MMSYSTEM.WAVEOUTCAPS();//WAVEOUTCAPSは、struct
            int err = WaveOut.waveOutGetDevCaps(index, ref pwoc, (uint)Marshal.SizeOf(typeof(MMSYSTEM.WAVEOUTCAPS)));
            return pwoc;
        }
    }
}
