using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Drawing;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(PublicDomain.DebuggerVisualizer_Bitmap),
typeof(VisualizerObjectSource),
Target = typeof(Bitmap),
Description = "新Bitmapビジュアライザ")]
namespace PublicDomain
{
    public class DebuggerVisualizer_Bitmap : DialogDebuggerVisualizer
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

        /// <summary>
        /// テスト。            // なお、TestShowVisualizerを呼び出すと、実行プログラムとして、実際にフォームがちゃんと表示されるかを確認できる
        /// </summary>
        /// <param name="objectToVisualize"></param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new
                VisualizerDevelopmentHost(objectToVisualize, typeof(DebuggerVisualizer_Bitmap));
            visualizerHost.ShowVisualizer();
        }

    }
}
