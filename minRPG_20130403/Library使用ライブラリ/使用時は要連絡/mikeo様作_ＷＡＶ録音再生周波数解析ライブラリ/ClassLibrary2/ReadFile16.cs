using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace RIFF
{
    public class ReadFile16
    {
        [Flags]
        public enum FileAccess : uint
        {
            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000,
        }

        [Flags]
        public enum FileShare : uint
        {
            None = 0x00000000,
            Read = 0x00000001,
            Write = 0x00000002,
            Delete = 0x00000004,
        }

        public enum CreationDisposition : uint
        {
            New = 1,
            CreateAlways = 2,
            OpenExisting = 3,
            OpenAlways = 4,
            TruncateExisting = 5,
        }

        [Flags]
        public enum FileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }
        public enum MoveMethod : uint
        {
            FILE_BEGIN = 0,
            FILE_CURRENT = 1,
            FILE_END = 2
        }
   
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(
            string lpFileName,
            FileAccess dwDesiredAccess,
            FileShare dwShareMode,
            IntPtr lpSecurityAttributes,
            CreationDisposition dwCreationDisposition,
            FileAttributes dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);



        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ReadFile(IntPtr handle, IntPtr bytes, uint numBytesToRead,
            ref uint numBytesRead, IntPtr overlapped);

        [DllImport("kernel32.dll")]
        static extern uint SetFilePointer(IntPtr hFile, int lDistanceToMove,
           IntPtr lpDistanceToMoveHigh, MoveMethod dwMoveMethod);

        IntPtr hFile;

        public ReadFile16()
        {
        }
        public void OpenRead(string path)
        {
            //ファイルを開く
            hFile = CreateFile(path, FileAccess.GenericRead, FileShare.Read, IntPtr.Zero,
                               CreationDisposition.OpenExisting, FileAttributes.Normal, IntPtr.Zero);

            if (hFile.ToInt32() == -1)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }
        public void Close()
        {
            CloseHandle(hFile);
        }
        public void Seek(long pos)
        {
            int low;
            IntPtr pHigh;
            if (pos < 0x80000000)
            {
                low = (int)pos;
                pHigh = IntPtr.Zero;
            }
            else
            {
                low = (int)(pos & 0xffffffff);
                uint high=(uint)(pos>>32);
                pHigh = Marshal.AllocHGlobal(4);
                Marshal.WriteInt32(pHigh, (int)high);
            }
            uint result = SetFilePointer(hFile,low,pHigh,MoveMethod.FILE_BEGIN);
            if(result==0xffffffff)
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            if (pHigh != IntPtr.Zero) Marshal.FreeHGlobal(pHigh);
        }
        /// <summary>
        /// 16ビット単位の読み取り
        /// </summary>
        /// <param name="n">読み取りサンプル数</param>
        /// <returns>読み取ったデータ</returns>
        public Int16[] ReadInt16(int n)
        {
            uint size = (uint)n * 2;
            uint bytesRead = 0;
            IntPtr p = Marshal.AllocHGlobal((int)size);
            bool bResult = ReadFile(hFile, p, size, ref bytesRead, IntPtr.Zero);
            if (!bResult) bytesRead = 0;
            Int16[] buf = new Int16[bytesRead / 2];
            if (buf.Length > 0)
                Marshal.Copy(p, buf, 0, buf.Length);
            Marshal.FreeHGlobal(p);
            return buf;
        }
        /// <summary>
        /// バイト列の読み取り
        /// </summary>
        /// <param name="bytes">読み取りバイト数</param>
        /// <returns>読み取ったデータ</returns>
        public byte[] ReadBytes(int bytes)
        {
            uint bytesRead = 0;
            IntPtr p = Marshal.AllocHGlobal(bytes);
            bool bResult = ReadFile(hFile, p, (uint)bytes, ref bytesRead, IntPtr.Zero);
            if (!bResult) bytesRead = 0;
            byte[] buf = new byte[bytesRead];
            if (buf.Length > 0)
                Marshal.Copy(p, buf, 0, buf.Length);
            Marshal.FreeHGlobal(p);
            return buf;
        }
    }
}
