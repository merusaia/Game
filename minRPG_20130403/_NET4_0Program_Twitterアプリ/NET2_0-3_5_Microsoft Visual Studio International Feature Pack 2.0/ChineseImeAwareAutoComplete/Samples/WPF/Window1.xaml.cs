using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.International.ImeAwareAutoComplete.Wpf;

namespace WpfSample
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        ChineseImeAwareAutoCompleteListener listener = null;

        public Window1()
        {
            InitializeComponent();
            listener = new ChineseImeAwareAutoCompleteListener(this.myUserControl1);
            InitListener();
        }
        
        private void InitListener()
        {

            listener.ContextManager.AddItem("微软");
            listener.ContextManager.AddItem("微软学生中心");
            listener.ContextManager.AddItem("微软大中华区");
            listener.ContextManager.AddItem("微软=Microsoft");
            listener.ContextManager.AddItem("北京");
            listener.ContextManager.AddItem("上海");

            listener.ContextManager.AddItem("正版");
            listener.ContextManager.AddItem("正版Windows");
            listener.ContextManager.AddItem("正版Windows软件非常好用");

            listener.ContextManager.AddItem("msn");
            listener.ContextManager.AddItem("Msn download");

            listener.ContextManager.AddItem("微軟");
            listener.ContextManager.AddItem("微軟學生中心");
            listener.ContextManager.AddItem("微軟大中華區");
            listener.ContextManager.AddItem("微軟Microsoft");

        }
    }
}
