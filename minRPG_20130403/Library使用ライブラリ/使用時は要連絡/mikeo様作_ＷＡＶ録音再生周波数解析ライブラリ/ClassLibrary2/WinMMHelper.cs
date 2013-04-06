using System;
using System.Collections.Generic;

using System.Reflection;
using System.Runtime.InteropServices;

namespace LIB
{
    public class WinMMHelper
    {
        //PCM�̏ڍׂ��`
        //PCM�ɂ́A�����L�薳����o�C�g�I�[�_�[�ɂ���đ����̌`��������炵�����AWindows�Ŏg������̂��`�B
        //wFormatTag��wBitsPerSample����
        public enum PCM_FORMAT
        {
            IEEE_FLOAT, S8, S16LE, S24LE, S32LE
        };
        /// <summary>
        /// �P����PCM�p��WAVEFORMATEX���쐬����
        /// </summary>
        /// <param name="nChannels">1:mono / 2:stereo</param>
        /// <param name="nSamplesPerSec">�T���v�����O����(Hz)</param>
        /// <param name="wBitsPerSample">1�T���v���̃r�b�g��(8,16bit)</param>
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
        /// �P����MP3�p��WAVEFORMATEX���쐬����
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
        /// wFormatTag�ɑΉ�����WAVE_FORMAT�̒�`�������Ԃ��B
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
        /// WAVEFORMATEX��WAVEFORMATEX_MP3����WAVEFORMATEX�����o���B
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
        /// ���̓f�o�C�X����Ԃ�
        /// </summary>
        /// <returns>���̓f�o�C�X��</returns>
        public static int GetNumInDevs()
        {
            return WaveIn.waveInGetNumDevs();
        }
        /// <summary>
        /// ���̓f�o�C�X�̐��\����Ԃ�
        /// </summary>
        /// <param name="index">�f�o�C�X��I�����鏇��</param>
        /// <returns>���\���</returns>
        public static MMSYSTEM.WAVEINCAPS GetInDevCaps(int index)
        {
            MMSYSTEM.WAVEINCAPS pwic = new MMSYSTEM.WAVEINCAPS();//WAVEINCAPS�́Astruct
            int err = WaveIn.waveInGetDevCaps(index, ref pwic, (uint)Marshal.SizeOf(typeof(MMSYSTEM.WAVEINCAPS)));
            return pwic;
        }
        /// <summary>
        /// �o�̓f�o�C�X����Ԃ�
        /// </summary>
        /// <returns>�o�̓f�o�C�X��</returns>
        public static int GetNumOutDevs()
        {
            return WaveOut.waveOutGetNumDevs();
        }
        /// <summary>
        /// �o�̓f�o�C�X�̐��\����Ԃ�
        /// </summary>
        /// <param name="index"></param>
        /// <returns>���\���</returns>
        public static MMSYSTEM.WAVEOUTCAPS GetOutDevCaps(int index)
        {
            MMSYSTEM.WAVEOUTCAPS pwoc = new MMSYSTEM.WAVEOUTCAPS();//WAVEOUTCAPS�́Astruct
            int err = WaveOut.waveOutGetDevCaps(index, ref pwoc, (uint)Marshal.SizeOf(typeof(MMSYSTEM.WAVEOUTCAPS)));
            return pwoc;
        }
    }
}
