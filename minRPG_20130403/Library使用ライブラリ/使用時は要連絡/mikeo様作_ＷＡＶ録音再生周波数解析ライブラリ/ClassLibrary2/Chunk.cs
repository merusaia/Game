using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace RIFF
{
    public class Chunk
    {
        public readonly bool bListType;
        //�t�@�C����̃I�t�Z�b�g
        public readonly long offset;
        //Chunk�̒���
        public readonly long length;
        //Chunk�̂̃��x���B
        public readonly string label;   //LIST�̂Ƃ��́A�t�H�[�}�b�g������

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
