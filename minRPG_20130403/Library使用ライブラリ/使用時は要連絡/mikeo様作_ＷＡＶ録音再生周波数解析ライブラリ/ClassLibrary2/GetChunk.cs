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
                return new MMFILE.CheckFileResult(true, "", fileSize, 0, "ファイルが小さ過ぎる。");
            }
            string label = fr.GetLabel(0);  //labelには"RIFF(0x52494646)"と入るはず。
            //ファイルの先頭でRIFFファイルか確認。
            if (label.Substring(0, 4) != "RIFF")
            {
                fr.Close();
                return new MMFILE.CheckFileResult(true, "", fileSize, 0, "ファイルがRIFFで始まっていない。");
            }
            //RIFFサイズ取得
            long riffSize = fr.GetLength();
            string formatType = fr.GetLabel();
            fr.Close();

            if (riffSize != (fileSize - 8))
                return new MMFILE.CheckFileResult(false, formatType, fileSize, riffSize, "RIFFに続く長さがファイルサイズと矛盾する。");
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
        /// RIFF形式ファイルのチャンクを読み出す
        /// </summary>
        /// <param name="path">RIFF形式ファイル</param>
        /// <param name="limit">ファイルの先頭からlimitバイトの範囲でチャンクを探す</param>
        /// <param name="progress">進捗をセットする変数</param>
        /// <param name="bDone">打ち切りを指定するための変数</param>
        /// <returns>チャンク</returns>
        public static Chunk[] Chunks(Stream stream, long limit, ref long progress, ref bool bDone)
        {
            List<Chunk> chunks=new List<Chunk>();
            ListProc(new FileReader(stream), ref chunks, 12, limit, ref progress, ref bDone);//RIFFnnnnAVI_ の次から
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
                if (bDone) return false;//アプリケーションからのキャンセル要求

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
                pos += (size + 8);//長さの値分 + 名前と長さ分
                if ((pos & 1) == 1) pos++;//偶数バウンダリ
                Interlocked.Exchange(ref progress, pos);
            }
            return pos == limit;
        }
        /// <summary>
        /// ファイルの4バイト目から4バイト（RIFFに続く4バイト）の値に相当。
        /// ただし、この値は不正な場合があるので、検出した最後のChunkから計算する。
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
