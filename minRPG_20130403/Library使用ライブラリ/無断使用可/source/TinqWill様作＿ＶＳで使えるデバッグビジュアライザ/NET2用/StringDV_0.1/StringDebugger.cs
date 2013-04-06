using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StringVisualizer
{
    public partial class StringDebugger : Form
    {
        public StringDebugger()
        {
            InitializeComponent();
        }

        //[閉じる]   
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //[変更を適用して閉じる]   
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        //[右端で折り返す]   
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.WordWrap = checkBox1.Checked;
        }

        //テキストボックスのテキストを外部に公開する   
        public string DebuggerText
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }   
    }
}
