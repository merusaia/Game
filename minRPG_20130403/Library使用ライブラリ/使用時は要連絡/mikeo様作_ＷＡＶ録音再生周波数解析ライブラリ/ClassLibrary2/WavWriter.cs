//
//FileWrite.cs
//(C)mikeo_410@hotmail.com

using System;
using System.IO;


namespace RIFF
{
    public class WavWriter
    {
        private Stream stream;
        private BinaryWriter bw;

        private string path = null;
        private MMSYSTEM.WAVEFORMATEX format;

        private int bodyLength;
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="path">WAV�t�@�C���̃p�X</param>
        /// <param name="format">WAV�t�H�[�}�b�g</param>
        public WavWriter(string path, MMSYSTEM.WAVEFORMATEX format)
            : this(new FileStream(path, FileMode.Create), format)
        {
            this.path = path;
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="stream">�o�̓X�g���[��</param>
        /// <param name="format">WAV�t�H�[�}�b�g</param>
        public WavWriter(Stream stream, MMSYSTEM.WAVEFORMATEX format)
        {
            this.stream = stream;
            this.format = format;
            bodyLength = 0;
            bw = new BinaryWriter(stream);
            WriteHeader();
        }
        /// <summary>
        /// WAV�t�@�C�������
        /// </summary>
        public void Close()
        {
            if (stream != null)
            {
                if (!bWriteEnd)
                {
                    bw.Seek(0, SeekOrigin.Begin);
                    WriteHeader();
                }
                stream.Close();
                bw.Close();
                stream = null;
            }
        }
        bool bWriteEnd = false;
        /// <summary>
        /// WAV�t�@�C���Ƀw�b�_���������ށBMemoryStream�̏ꍇ�Ɏg�p�B
        /// </summary>
        public void WriteEnd()
        {
            if (stream != null)
            {
                bWriteEnd = true;
                bw.Seek(0, SeekOrigin.Begin);
                WriteHeader();
                bw.Seek(0, SeekOrigin.Begin);
            }
        }
        /// <summary>
        /// Body�̏�������
        /// </summary>
        /// <param name="wave">�o�͂���f�[�^</param>
        /// <param name="size">�o�͂���o�C�g��</param>
        public void Write(byte[] wave, int size)
        {
            bw.Write(wave, 0, size);
            bodyLength += size;
        }
        /// <summary>
        /// Body�̏�������
        /// </summary>
        /// <param name="wave">�o�͂���f�[�^</param>
        /// <param name="size">�o�͂���T���v����(16�r�b�g�P��)</param>
        public void Write(short[] wave, int size)
        {
            for (int i = 0; i < size; i++)
            {
                bw.Write(wave[i]);
            }
            bodyLength += (size * 2);
        }
        /// <summary>
        /// �����߂�
        /// </summary>
        public void Rewind()
        {
            bodyLength = 0;
            bw.BaseStream.SetLength(0);
            WriteHeader();
        }
        //�w�b�_���̏�������
        private void WriteHeader()
        {
            bw.Write(new char[] { 'R', 'I', 'F', 'F' });
            bw.Write((int)bodyLength + 44 - 8);
            bw.Write(new char[] { 'W', 'A', 'V', 'E' });
            bw.Write(new char[] { 'f', 'm', 't', ' ' });
            bw.Write((int)16);
            bw.Write((short)format.wFormatTag);
            bw.Write((short)format.nChannels);
            bw.Write((int)format.nSamplesPerSec);
            bw.Write((int)format.nAvgBytesPerSec);
            bw.Write((short)format.nBlockAlign);
            bw.Write((short)format.wBitsPerSample);

            bw.Write(new char[] { 'd', 'a', 't', 'a' });
            bw.Write((int)bodyLength);
        }
    }
}