using System;

using System.IO;
using System.Runtime.InteropServices;

namespace RIFF
{
    public class WavUtils
    {
        /// <summary>
        /// WavファイルからWAVEFORMATEXを読み出す
        /// </summary>
        /// <param name="path">Wavファイルのパス</param>
        /// <param name="fmtChunk">フォーマットチャンク（ファイル中の位置を示す）</param>
        /// <returns>WAVEFORMATEX</returns>
        public static MMSYSTEM.WAVEFORMATEX ReadFormat(string path, Chunk fmtChunk)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[Marshal.SizeOf(typeof(MMSYSTEM.WAVEFORMATEX))];
            fs.Seek(fmtChunk.offset + 8,SeekOrigin.Begin);
            fs.Read(buf,0,buf.Length);
            fs.Close();
            IntPtr p = Marshal.AllocHGlobal(buf.Length);
            Marshal.Copy(buf, 0, p, buf.Length);
            MMSYSTEM.WAVEFORMATEX wfx=new MMSYSTEM.WAVEFORMATEX();
            Marshal.PtrToStructure(p, wfx);
            Marshal.FreeHGlobal(p);
            return wfx;
        }
    }
}
