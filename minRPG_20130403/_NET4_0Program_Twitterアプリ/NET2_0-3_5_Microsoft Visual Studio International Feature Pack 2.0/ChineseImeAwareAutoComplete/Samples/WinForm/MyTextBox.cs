using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChineseAutoCompletionWinFormDemo
{
    public partial class MyTextBox : TextBox
    {
        public MyTextBox()
        {
            InitializeComponent();
        }

        public MyTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
