using System;
using System.Collections.Generic;
using System.Text;

namespace MMFILE
{
    public class CheckFileResult
    {
        public readonly bool bError;//以降解析不可の場合にtrue
        public readonly string formatType;//AVI ,WAVE,...
        public readonly long FileSize;
        public readonly long TotalSize;//RIFFの場合は、RIFFに続く長さ情報の値
        public readonly string msg;

        public CheckFileResult(bool bError, string formatType, long fileSize, long totalSize, string msg)
        {
            this.bError = bError;
            this.formatType = formatType;
            this.FileSize = fileSize;
            this.TotalSize = totalSize;
            this.msg = msg;
        }
    }
}
