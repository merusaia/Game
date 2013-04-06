using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace RIFF
{
    public class Chunk
    {
        public readonly bool bListType;
        //ファイル上のオフセット
        public readonly long offset;
        //Chunkの長さ
        public readonly long length;
        //Chunkののラベル。
        public readonly string label;   //LISTのときは、フォーマット文字列

        public Chunk(long offset, long length, string label, string format)
        {
            this.offset = offset;
            this.length = length;
            if (label == "LIST")
            {
                bListType = true;
                this.label = format;
            }
            else
            {
                bListType = false;
                this.label = label;
            }
        }
    }
}
