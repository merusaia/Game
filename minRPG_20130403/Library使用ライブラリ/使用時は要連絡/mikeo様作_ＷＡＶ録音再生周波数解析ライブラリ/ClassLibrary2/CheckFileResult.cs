using System;
using System.Collections.Generic;
using System.Text;

namespace MMFILE
{
    public class CheckFileResult
    {
        public readonly bool bError;//ˆÈ~‰ğÍ•s‰Â‚Ìê‡‚Étrue
        public readonly string formatType;//AVI ,WAVE,...
        public readonly long FileSize;
        public readonly long TotalSize;//RIFF‚Ìê‡‚ÍARIFF‚É‘±‚­’·‚³î•ñ‚Ì’l
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
