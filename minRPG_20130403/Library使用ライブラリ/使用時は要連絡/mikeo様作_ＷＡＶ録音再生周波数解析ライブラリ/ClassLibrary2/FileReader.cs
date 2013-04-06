using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace RIFF
{
    class FileReader
    {
        //FileStream fs = null;
        BinaryReader br = null;
        public FileReader(Stream stream)
        {
            br = new BinaryReader(stream);
        }
        public FileReader(string path)
            :this(File.OpenRead(path))
        {
        }
        public void Close()
        {
            if (br != null) br.Close();
            br = null;
        }
        public long Position
        {
            get { return br.BaseStream.Position; }
            set { br.BaseStream.Position = value; }
        }
        public long FileSize
        {
            get{return br.BaseStream.Length;}
        }
        //-----------------------------------------------------------
        //byte[]‚Ì“Ç‚Ýž‚Ý
        public byte[] GetBytes(long offset, int length)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            return br.ReadBytes(length);
        }
        //-----------------------------------------------------------
        //ƒ‰ƒxƒ‹•”•ª‚Ì“Ç‚Ýž‚Ý
        char byteChar(byte b)
        {
            if ((b >= 0x20) && (b <= 0x7e)) return Convert.ToChar(b);
            return '.';
        }
        string byteStr(byte[] b4)
        {
            char[] c4 = new char[4];
            c4[0] = byteChar(b4[0]);
            c4[1] = byteChar(b4[1]);
            c4[2] = byteChar(b4[2]);
            c4[3] = byteChar(b4[3]);
            return new string(c4);
        }
        public string GetLabel()
        {
            byte[] b4 = br.ReadBytes(4);
            if (b4.Length != 4)
            {
            }
            string label = byteStr(b4); 
            return label;
        }
        public string GetLabel(long offset)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            return GetLabel();
        }
        //-----------------------------------------------------------
        //’·‚³‚Ì“Ç‚Ýž‚Ý
        public long GetLength()
        {
            return GetDWord();
        }
        public long GetLength(long offset)
        {
            return GetDWord(offset);
        }
        //-----------------------------------------------------------
        //DWORD‚Ì“Ç‚Ýž‚Ý
        public long GetDWord()
        {
            return br.ReadUInt32();
        }
        public long GetDWord(long offset)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            return GetDWord();
        }
        //-----------------------------------------------------------
        //WORD‚Ì“Ç‚Ýž‚Ý
        public int GetWord()
        {
            return br.ReadUInt16();
        }
        public int GetWord(long offset)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            return GetWord();
        }
    }
    /*
    class Dump
    {
        public static void Write(byte[] buf, int size)
        {
            string str;
            if (buf.Length < size) size = buf.Length;
            str = String.Format("=== {0} bytes ===", size);
            Debug.WriteLine(str);
            int i = 0;
            while (true)
            {
                if (i > size) break;
                str = String.Format(" {0,2:x}", buf[i]);
                Debug.Write(str);
                if ((i % 16) == 15) Debug.WriteLine("");
                i++;
            }
            Debug.WriteLine("");
        }
        public static void Write(byte[] buf)
        {
            Write(buf, buf.Length);
        }
    }*/
}
