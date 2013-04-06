using System;
using System.Runtime.InteropServices;

namespace YomiganaFrameworkSample
{
    internal class YomiHelper : IDisposable
    {
        private static IFEInterface ifeInterface = null;
        private static int hResult_Yomi;
        private static object objForLock = new object();

        public static string GetYomiByIME(string text, int yomiMode)
        {
            IntPtr ppResult_Yomi = IntPtr.Zero;
            string yomi = string.Empty;

            lock (objForLock)
            {
                Guid clsid = new Guid();
                int hr = NativeMethods.CLSIDFromString("MSIME.Japan", ref clsid);
                if (hr != NativeMethods.S_OK)
                    throw Marshal.GetExceptionForHR(hr);
                Object temp;

                Guid iid = new Guid("21164102-C24A-11d1-851A-00C04FCC6B14");//IFELanguage2

             
                hr = NativeMethods.CoCreateInstance(ref clsid, IntPtr.Zero, NativeMethods.CLSCST, ref iid, out temp);
                if (hr != NativeMethods.S_OK)
                {
                    return text;
                }

                ifeInterface = (IFEInterface)temp;

                ifeInterface.Open();

                hResult_Yomi = 0;
                try
                {
                    hResult_Yomi = ifeInterface.GetJMorphResult(NativeMethods.FELANG_REQ_REV,
                                                          yomiMode,
                                                          text == null ? 0 : text.Length,
                                                          text,
                                                          IntPtr.Zero,
                                                          out ppResult_Yomi);
                    if (hResult_Yomi == NativeMethods.S_OK)
                    {
                        yomi = MORRSLTPtrToString(ppResult_Yomi);
                    }
                }
                finally
                {
                    if (ppResult_Yomi != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(ppResult_Yomi);
                    }
                }

                ifeInterface.Close();
            }

            return yomi;
        }


        public void Dispose()
        {

        }

        // Get the pwchOutput of a MORRSLT PTR and return a string
        private static string MORRSLTPtrToString(IntPtr pMorrslt)
        {
            NativeMethods.MORRSLT morrslt = (NativeMethods.MORRSLT)Marshal.PtrToStructure(pMorrslt, typeof(NativeMethods.MORRSLT));
            return Marshal.PtrToStringUni(morrslt.pwchOutput, morrslt.cchOutput);
        }
    }

}
