using System;
using System.IO;

namespace RIFF
{
    public class WaveReader
    {
        string path;
        MMSYSTEM.WAVEFORMATEX wfx;
        long DataStartPosition;
        int DataLength;
        int samples;
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="path">WAVE�t�@�C����</param>
        /// <param name="fmtChunk">fmt�`�����N</param>
        /// <param name="dataChunk">data�`�����N</param>
        public WaveReader(string path, Chunk fmtChunk, Chunk dataChunk)
        {
            waveReader(path, fmtChunk, dataChunk);
        }
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="path">WAV�t�@�C���̃p�X</param>
        public WaveReader(string path)
        {
            long prog = 0;
            bool done = false;
            Chunk[] chunks = GetChunks.Chunks(path, 4096, ref prog, ref done);
            int i_fmt = -1;
            int i_data = -1;
            for (int i = 0; i < chunks.Length; i++)
            {
                if ((i_fmt <= 0) && (chunks[i].label == "fmt ")) i_fmt = i;
                if ((i_data <= 0) && (chunks[i].label == "data")) i_data = i;
            }
            if ((i_fmt < 0) || (i_data < 0))
                throw new Exception("����RIFF�t�@�C���́Afmt��data�̃`�����N��������Ă��܂���B");
            waveReader(path, chunks[i_fmt], chunks[i_data]);
        }
        public WaveReader(Stream stream)
        {
        }
        /// <summary>
        /// �R���X�g���N�^�̃T�u���[�`��
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fmtChunk"></param>
        /// <param name="dataChunk"></param>
        void waveReader(string path, Chunk fmtChunk, Chunk dataChunk)
        {
            this.path = path;
            wfx = WavUtils.ReadFormat(path, fmtChunk);
            if (wfx.wFormatTag != (short)MMSYSTEM.WAVE_FORMAT.PCM)
                throw new Exception("PCM�`���ł͂Ȃ��B");
            if ((wfx.wBitsPerSample != 8) && (wfx.wBitsPerSample != 16))
                throw new Exception("8�r�b�g/�T���v���ł�16�r�b�g/�T���v���ł��Ȃ��B");

            DataStartPosition = dataChunk.offset;
            DataLength = (int)dataChunk.length;
            samples = DataLength / (wfx.nChannels * (wfx.wBitsPerSample / 8));
        }
        /// <summary>
        /// �`���l�������l�������S�T���v�����B
        /// </summary>
        public int Samples
        {
            get { return samples; }
        }
        /// <summary>
        /// Wave�t�@�C����WAVEFORMATEX
        /// </summary>
        public MMSYSTEM.WAVEFORMATEX Format
        {
            get { return wfx; }
        }
        //-----------------------------------------------------------------------------
        ReadFile16 ff;
        /// <summary>
        /// WAVE�t�@�C�����J��
        /// </summary>
        public void Open()
        {
            ff = new ReadFile16();
            ff.OpenRead(path);
        }
        /// <summary>
        /// WAVE�t�@�C�������
        /// </summary>
        public void Close()
        {
            ff.Close();
        }
        /// <summary>
        /// sample_offset�������T���v���̃t�@�C����̈ʒu��Ԃ�
        /// </summary>
        /// <param name="sample_offset">�ʒu�����߂����T���v���̏���</param>
        /// <returns>�t�@�C����̃I�t�Z�b�g</returns>
        long Position(long sample_offset)
        {
            return DataStartPosition + 8 + sample_offset * wfx.nChannels * (wfx.wBitsPerSample / 8);
        }
        /// <summary>
        /// �ǂݎ��o�C�g����Ԃ��B�t�@�C���T�C�Y���l������B
        /// </summary>
        /// <param name="start">�ǂݎ��J�n�T���v���̏���</param>
        /// <param name="n">�ǂݎ��T���v����</param>
        /// <returns>�ǂݎ��o�C�g��</returns>
        long Size(long start, int n)
        {
            int block = wfx.nChannels * (wfx.wBitsPerSample / 8);
            long size = n * block;
            if (start + size > (DataStartPosition + DataLength))
            {
                //�v�����t�@�C���̃T�C�Y�𒴂���
                size = DataStartPosition + DataLength - start;
                long f = size / block; //�n���p��؂�̂�
                size = f * block;
            }
            return size;
        }
        /// <summary>
        /// WAVE�t�@�C���̎w��ʒu����w��T���v��(16�r�b�g�T���v��)���ǂݏo��
        /// </summary>
        /// <param name="start">data�`�����N���̃T���v�����ʁi�`���l�������l���j</param>
        /// <param name="n">�T���v�����i2�`�����l���Ȃ�1�ŁA4�o�C�g���ǂݎ����j</param>
        /// <returns>�ǂݎ�����T���v��</returns>
        public Int16[] ReadSamples(long start, int n)
        {
            long size = Size(start, n);
            ff.Seek(Position(start));
            if (wfx.wBitsPerSample == 8)
            {
                byte[] b = ff.ReadBytes((int)size);
                Int16[] i16 = new Int16[size];
                for (int i = 0; i < size; i++)
                {
                    i16[i] = (Int16)(((int)b[i] - 128) * 256);
                }
                return i16;
            }
            else
            {
                return ff.ReadInt16((int)(size / 2));
            }
        }
        /// <summary>
        /// WAVE�t�@�C���̎w��ʒu����w��o�C�g���ǂݏo��
        /// </summary>
        /// <param name="start">data�`�����N���̃o�C�g�ʒu</param>
        /// <param name="bytes">�ǂݎ��o�C�g��</param>
        /// <returns>�ǂݎ�����f�[�^</returns>
        public byte[] ReadBytes(long start, int bytes)
        {
            ff.Seek(DataStartPosition + start);
            return ff.ReadBytes(bytes);
        }
   }
}
