using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows.Forms;
using System.Drawing;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(ColorDebugger.ColorVisualizer),
typeof(VisualizerObjectSource),
Target = typeof(System.Drawing.Color),
Description = "Colorビジュアライザ")]   

namespace ColorDebugger
{
    public class ColorVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
using (ColorDialog cd = new ColorDialog())
{
    cd.Color = (Color)objectProvider.GetObject();
    if (windowService.ShowDialog(cd) == DialogResult.OK)
    {
        objectProvider.ReplaceObject(cd.Color);
    }
}
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(ColorVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
