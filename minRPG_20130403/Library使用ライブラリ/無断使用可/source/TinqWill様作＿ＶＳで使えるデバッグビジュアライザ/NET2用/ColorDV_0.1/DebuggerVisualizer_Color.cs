using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Windows.Forms;
using System.Drawing;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(PublicDomain.ColorDebugger),
typeof(VisualizerObjectSource),
Target = typeof(System.Drawing.Color),
Description = "Colorビジュアライザ")]   

namespace PublicDomain
{
    public class ColorDebugger : DialogDebuggerVisualizer
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
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(ColorDebugger));
            visualizerHost.ShowVisualizer();
        }
    }
}
