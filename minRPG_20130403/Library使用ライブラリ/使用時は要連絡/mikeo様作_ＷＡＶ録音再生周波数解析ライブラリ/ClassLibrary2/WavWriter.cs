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
        /// コンストラクタ
        /// </summary>
        /// <param name="path">WAVファイルのパス</param>
        /// <param name="format">WAVフォーマット</param>
        public WavWriter(string path, MMSYSTEM.WAVEFORMATEX format)
            : this(new FileStream(path, FileMode.Create), format)
        {
            this.path = path;
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stream">出力ストリーム</param>
        /// <param name="format">WAVフォーマット</param>
        public WavWriter(Stream stream, MMSYSTEM.WAVEFORMATEX format)
        {
            this.stream = stream;
            this.format = format;
            bodyLength = 0;
            bw = new BinaryWriter(stream);
            WriteHeader();
        }
        /// <summary>
        /// WAVファイルを閉じる
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
        /// WAVファイルにヘッダを書き込む。MemoryStreamの場合に使用。
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
        /// Bodyの書き込み
        /// </summary>
        /// <param name="wave">出力するデータ</param>
        /// <param name="size">出力するバイト数</param>
        public void Write(byte[] wave, int size)
        {
            bw.Write(wave, 0, size);
            bodyLength += size;
        }
        /// <summary>
        /// Bodyの書き込み
        /// </summary>
        /// <param name="wave">出力するデータ</param>
        /// <param name="size">出力するサンプル数(16ビット単位)</param>
        public void Write(short[] wave, int size)
        {
            for (int i = 0; i < size; i++)
            {
                bw.Write(wave[i]);
            }
            bodyLength += (size * 2);
        }
        /// <summary>
        /// 巻き戻し
        /// </summary>
        public void Rewind()
        {
            bodyLength = 0;
            bw.BaseStream.SetLength(0);
            WriteHeader();
        }
        //ヘッダ部の書き込み
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