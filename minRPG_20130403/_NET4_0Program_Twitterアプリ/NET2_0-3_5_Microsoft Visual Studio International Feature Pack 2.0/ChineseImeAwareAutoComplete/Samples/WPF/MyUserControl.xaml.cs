using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.International.ImeAwareAutoComplete.Wpf;

namespace WpfSample
{
    public partial class MyUserControl : UserControl, IChineseAutoCompletionControl
	{
		public MyUserControl()
		{
			this.InitializeComponent();

            this.myTextBox.TextChanged += new TextChangedEventHandler(myTextBox_TextChanged);
			// Insert code required on object creation below this point.
		}

        void myTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextChanged(e);
        }

        protected void OnTextChanged(TextChangedEventArgs e)
        {
            if (TextChanged!=null)
            {
                TextChanged(this, e);
            }
        }

        #region IChineseAutoCompletionControl Members

        public string Text
        {
            get
            {
                return this.myTextBox.Text;
            }
            set
            {
                this.myTextBox.Text = value;
            }
        }

        public int CaretIndex
        {
            get
            {
                return this.myTextBox.CaretIndex;
            }
            set
            {
                this.myTextBox.CaretIndex = value;
            }
        }

        public Control Element
        {
            get { return this.myTextBox; }
        }

        public event TextChangedEventHandler TextChanged;

        #endregion
    }
}