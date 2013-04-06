using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(StringVisualizer.StringVisualizer),
typeof(VisualizerObjectSource),
Target = typeof(string),
Description = "テキスト ビジュアライザEx")]   
namespace StringVisualizer
{

    /// <summary>
    /// String のビジュアライザです。  
    /// </summary>
    public class StringVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            //フォームを作成   
            StringDebugger sd = new StringDebugger();
            //テキストボックスにセット   
            sd.DebuggerText = objectProvider.GetObject().ToString();
            //ダイアログとして表示。[変更を適用して閉じる]が押されるとDialogResult.Yesになる   
            if (windowService.ShowDialog(sd) == DialogResult.Yes)
            {
                //文字を差し替える   
                objectProvider.ReplaceObject(sd.DebuggerText);
            }
        }   
        /// <summary>
        /// デバッガの外部にホストすることにより、ビジュアライザをテストします。
        /// </summary>
        /// <param name="objectToVisualize">ビジュアライザに表示するオブジェクトです。</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(StringVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
