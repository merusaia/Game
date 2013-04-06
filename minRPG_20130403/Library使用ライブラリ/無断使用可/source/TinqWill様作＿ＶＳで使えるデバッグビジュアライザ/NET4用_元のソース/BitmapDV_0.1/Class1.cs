using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Drawing;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(BitmapDebugger.ImageDebugger),
typeof(VisualizerObjectSource),
Target = typeof(Bitmap),
Description = "Bitmapビジュアライザ")]
namespace BitmapDebugger
{
    public class ImageDebugger : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            ImageView iv = new ImageView();
            iv.SetBitmap(objectProvider.GetObject() as Bitmap);
            if (windowService.ShowDialog(iv) == System.Windows.Forms.DialogResult.OK)
            {
                objectProvider.ReplaceObject(iv.GetBitmap());
            }
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new 
                VisualizerDevelopmentHost(objectToVisualize, typeof(ImageDebugger));
            visualizerHost.ShowVisualizer();
        }
    }
}
