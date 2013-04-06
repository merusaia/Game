using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "Hello!";
            Image _image = new Bitmap();
            BitmapDebugger.ImageDebugger.TestShowVisualizer(_image);
        }
    }
}
