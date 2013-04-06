using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PublicDomain
{
    public partial class Test_UndoTextBox : RichTextBox // UsrControl
    {
        /// <summary>
        /// Undoが可能な立地テキストボックスです。
        /// </summary>
        public Test_UndoTextBox()
        {
            InitializeComponent();
        }
        public List<string> p_Texts_TimeLine = new List<string>();

        private void Test_UndoTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
