namespace WinFormSample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imeAwareAutoCompleteTextBox1 = new Microsoft.International.ImeAwareAutoComplete.WinForm.ImeAwareAutoCompleteTextBox();
            this.myTextBox1 = new ChineseAutoCompletionWinFormDemo.MyTextBox(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ImeAutoCompletionTextBox :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "MyTextBox :";
            // 
            // imeAwareAutoCompleteTextBox1
            // 
            this.imeAwareAutoCompleteTextBox1.Location = new System.Drawing.Point(238, 85);
            this.imeAwareAutoCompleteTextBox1.Name = "imeAwareAutoCompleteTextBox1";
            this.imeAwareAutoCompleteTextBox1.Size = new System.Drawing.Size(100, 20);
            this.imeAwareAutoCompleteTextBox1.TabIndex = 0;
            // 
            // myTextBox1
            // 
            this.myTextBox1.Location = new System.Drawing.Point(238, 191);
            this.myTextBox1.Name = "myTextBox1";
            this.myTextBox1.Size = new System.Drawing.Size(100, 20);
            this.myTextBox1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 358);
            this.Controls.Add(this.myTextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.imeAwareAutoCompleteTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.International.ImeAwareAutoComplete.WinForm.ImeAwareAutoCompleteTextBox imeAwareAutoCompleteTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ChineseAutoCompletionWinFormDemo.MyTextBox myTextBox1;
    }
}

