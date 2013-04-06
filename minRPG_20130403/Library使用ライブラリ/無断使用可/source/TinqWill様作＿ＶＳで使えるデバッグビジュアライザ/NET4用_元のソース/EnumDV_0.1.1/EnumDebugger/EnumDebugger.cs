using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(EnumDebugger.EnumDebugger),
typeof(VisualizerObjectSource),
Target = typeof(Enum),
Description = "EnumDebugger")]
namespace EnumDebugger
{
        /// <summary>
        /// 列挙体のデバッグをサポートします
        /// </summary>
        public class EnumDebugger : DialogDebuggerVisualizer
        {
            protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
            {
                EnumForm ev = new EnumForm();
                ev.SetEnumObject((Enum)objectProvider.GetObject());
                if (windowService.ShowDialog(ev) == DialogResult.OK)
                {
                    objectProvider.ReplaceObject( ev.GetResult() );
                }
            }
            public static void TestShowVisualizer(object objectToVisualize)
            {
                VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(EnumDebugger));
                visualizerHost.ShowVisualizer();
            }
        }
}