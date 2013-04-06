using System;
using System.Collections.Generic;

using System.Threading;
using System.IO;

namespace RIFF
{
    public class GetChunks
    {
        public static MMFILE.CheckFileResult CheckRIFF(string path)
        {
            FileReader fr;
            try
            {
                fr = new FileReader(path);
            }
            catch (Exception ex)
            {
                return new MMFILE.CheckFileResult(true, "", 0, 0, ex.Message);
            }
            long fileSize = fr.FileSize;
            if (fileSize < 12)
            {
                fr.Close();
                return new MMFILE.CheckFileResult(true, "", fileSize, 0, "�t�@�C���������߂���B");
            }
            string label = fr.GetLabel(0);  //label�ɂ�"RIFF(0x52494646)"�Ɠ���͂��B
            //�t�@�C���̐擪��RIFF�t�@�C�����m�F�B
            if (label.Substring(0, 4) != "RIFF")
            {
                fr.Close();
                return new MMFILE.CheckFileResult(true, "", fileSize, 0, "�t�@�C����RIFF�Ŏn�܂��Ă��Ȃ��B");
            }
            //RIFF�T�C�Y�擾
            long riffSize = fr.GetLength();
            string formatType = fr.GetLabel();
            fr.Close();

            if (riffSize != (fileSize - 8))
                return new MMFILE.CheckFileResult(false, formatType, fileSize, riffSize, "RIFF�ɑ����������t�@�C���T�C�Y�Ɩ�������B");
            return new MMFILE.CheckFileResult(false, formatType, fileSize, riffSize, null);
        }
        public static byte[] Chunk(string fileName, Chunk chunk)
        {
            FileReader fr = new FileReader(fileName);
            byte[] data = fr.GetBytes(chunk.offset, 256);
            fr.Close();
            return data;
        }
        /// <summary>
        /// RIFF�`���t�@�C���̃`�����N��ǂݏo��
        /// </summary>
        /// <param name="path">RIFF�`���t�@�C��</param>
        /// <param name="limit">�t�@�C���̐擪����limit�o�C�g�͈̔͂Ń`�����N��T��</param>
        /// <param name="progress">�i�����Z�b�g����ϐ�</param>
        /// <param name="bDone">�ł��؂���w�肷�邽�߂̕ϐ�</param>
        /// <returns>�`�����N</returns>
        public static Chunk[] Chunks(Stream stream, long limit, ref long progress, ref bool bDone)
        {
            List<Chunk> chunks=new List<Chunk>();
            ListProc(new FileReader(stream), ref chunks, 12, limit, ref progress, ref bDone);//RIFFnnnnAVI_ �̎�����
            //stream.Close();
            return chunks.ToArray();
        }
        public static Chunk[] Chunks(string path, long limit, ref long progress, ref bool bDone)
        {
            return Chunks(File.OpenRead(path), limit, ref progress, ref bDone);
        }
        static bool ListProc(FileReader fr, ref List<Chunk> chunks, long pos, long limit, ref long progress, ref bool bDone)
        {
            while (pos < limit)
            {
                if (bDone) return false;//�A�v���P�[�V��������̃L�����Z���v��

                string label = fr.GetLabel(pos);
                long size = fr.GetLength();
                if (label == "LIST")
                {
                    string formtype = fr.GetLabel();
                    Chunk chunk = new Chunk(pos, size, label, formtype);
                    chunks.Add(chunk);
                    ListProc(fr, ref chunks, pos + 12, pos + size + 8, ref progress, ref bDone);
                }
                else
                {
                    Chunk chunk = new Chunk(pos, size, label, null);
                    chunks.Add(chunk);
                }
                pos += (size + 8);//�����̒l�� + ���O�ƒ�����
                if ((pos & 1) == 1) pos++;//�����o�E���_��
                Interlocked.Exchange(ref progress, pos);
            }
            return pos == limit;
        }
        /// <summary>
        /// �t�@�C����4�o�C�g�ڂ���4�o�C�g�iRIFF�ɑ���4�o�C�g�j�̒l�ɑ����B
        /// �������A���̒l�͕s���ȏꍇ������̂ŁA���o�����Ō��Chunk����v�Z����B
        /// </summary>
        /// <param name="chunks"></param>
        /// <returns></returns>
        public static long RiffLehgth(ref Chunk[] chunks)
        {
            if (chunks.Length < 1) return 0;
            Chunk chunk = chunks[chunks.Length - 1];
            return chunk.offset + chunk.length + 8;
        }
    }
}
