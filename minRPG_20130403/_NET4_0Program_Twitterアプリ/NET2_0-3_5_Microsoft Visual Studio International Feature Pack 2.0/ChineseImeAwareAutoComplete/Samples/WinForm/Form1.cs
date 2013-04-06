using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.International.ImeAwareAutoComplete.WinForm;

namespace WinFormSample
{
    public partial class Form1 : Form
    {
        ChineseImeAwareAutoCompleteListener listener = null;

        public Form1()
        {
            InitializeComponent();
            listener = new ChineseImeAwareAutoCompleteListener(this.myTextBox1);
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
