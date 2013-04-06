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
        /// コンストラクタ
        /// </summary>
        /// <param name="path">WAVEファイル名</param>
        /// <param name="fmtChunk">fmtチャンク</param>
        /// <param name="dataChunk">dataチャンク</param>
        public WaveReader(string path, Chunk fmtChunk, Chunk dataChunk)
        {
            waveReader(path, fmtChunk, dataChunk);
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path">WAVファイルのパス</param>
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
                throw new Exception("このRIFFファイルは、fmtとdataのチャンクがそろっていません。");
            waveReader(path, chunks[i_fmt], chunks[i_data]);
        }
        public WaveReader(Stream stream)
        {
        }
        /// <summary>
        /// コンストラクタのサブルーチン
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fmtChunk"></param>
        /// <param name="dataChunk"></param>
        void waveReader(string path, Chunk fmtChunk, Chunk dataChunk)
        {
            this.path = path;
            wfx = WavUtils.ReadFormat(path, fmtChunk);
            if (wfx.wFormatTag != (short)MMSYSTEM.WAVE_FORMAT.PCM)
                throw new Exception("PCM形式ではない。");
            if ((wfx.wBitsPerSample != 8) && (wfx.wBitsPerSample != 16))
                throw new Exception("8ビット/サンプルでも16ビット/サンプルでもない。");

            DataStartPosition = dataChunk.offset;
            DataLength = (int)dataChunk.length;
            samples = DataLength / (wfx.nChannels * (wfx.wBitsPerSample / 8));
        }
        /// <summary>
        /// チャネル数を考慮した全サンプル数。
        /// </summary>
        public int Samples
        {
            get { return samples; }
        }
        /// <summary>
        /// WaveファイルのWAVEFORMATEX
        /// </summary>
        public MMSYSTEM.WAVEFORMATEX Format
        {
            get { return wfx; }
        }
        //-----------------------------------------------------------------------------
        ReadFile16 ff;
        /// <summary>
        /// WAVEファイルを開く
        /// </summary>
        public void Open()
        {
            ff = new ReadFile16();
            ff.OpenRead(path);
        }
        /// <summary>
        /// WAVEファイルを閉じる
        /// </summary>
        public void Close()
        {
            ff.Close();
        }
        /// <summary>
        /// sample_offsetが示すサンプルのファイル上の位置を返す
        /// </summary>
        /// <param name="sample_offset">位置を求めたいサンプルの順番</param>
        /// <returns>ファイル上のオフセット</returns>
        long Position(long sample_offset)
        {
            return DataStartPosition + 8 + sample_offset * wfx.nChannels * (wfx.wBitsPerSample / 8);
        }
        /// <summary>
        /// 読み取りバイト数を返す。ファイルサイズを考慮する。
        /// </summary>
        /// <param name="start">読み取り開始サンプルの順番</param>
        /// <param name="n">読み取りサンプル数</param>
        /// <returns>読み取りバイト数</returns>
        long Size(long start, int n)
        {
            int block = wfx.nChannels * (wfx.wBitsPerSample / 8);
            long size = n * block;
            if (start + size > (DataStartPosition + DataLength))
            {
                //要求がファイルのサイズを超える
                size = DataStartPosition + DataLength - start;
                long f = size / block; //ハンパを切り捨て
                size = f * block;
            }
            return size;
        }
        /// <summary>
        /// WAVEファイルの指定位置から指定サンプル(16ビットサンプル)数読み出す
        /// </summary>
        /// <param name="start">dataチャンク中のサンプル順位（チャネル数を考慮）</param>
        /// <param name="n">サンプル数（2チャンネルなら1で、4バイトが読み取られる）</param>
        /// <returns>読み取ったサンプル</returns>
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
        /// WAVEファイルの指定位置から指定バイト数読み出す
        /// </summary>
        /// <param name="start">dataチャンク中のバイト位置</param>
        /// <param name="bytes">読み取りバイト数</param>
        /// <returns>読み取ったデータ</returns>
        public byte[] ReadBytes(long start, int bytes)
        {
            ff.Seek(DataStartPosition + start);
            return ff.ReadBytes(bytes);
        }
   }
}
