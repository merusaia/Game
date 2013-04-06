namespace SampleRecorder
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナ変数です。

        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。

        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード


        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を

        /// コード エディタで変更しないでください。

        /// </summary>
        private void InitializeComponent()
        {
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.FileDlgButton = new System.Windows.Forms.Button();
            this.radioButton44 = new System.Windows.Forms.RadioButton();
            this.radioButton48 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_stereo = new System.Windows.Forms.RadioButton();
            this.radioButton_mono = new System.Windows.Forms.RadioButton();
            this.PlayButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(5, 98);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(141, 23);
            this.StartButton.TabIndex = 0;
            this.StartButton.Text = "録音開始";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(180, 97);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(141, 23);
            this.StopButton.TabIndex = 1;
            this.StopButton.Text = "停止";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 149);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(326, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(134, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(134, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(271, 19);
            this.textBox1.TabIndex = 3;
            // 
            // FileDlgButton
            // 
            this.FileDlgButton.Location = new System.Drawing.Point(287, 10);
            this.FileDlgButton.Name = "FileDlgButton";
            this.FileDlgButton.Size = new System.Drawing.Size(28, 23);
            this.FileDlgButton.TabIndex = 4;
            this.FileDlgButton.Text = "...";
            this.FileDlgButton.UseVisualStyleBackColor = true;
            this.FileDlgButton.Click += new System.EventHandler(this.FileDlg_Click);
            // 
            // radioButton44
            // 
            this.radioButton44.AutoSize = true;
            this.radioButton44.Checked = true;
            this.radioButton44.Location = new System.Drawing.Point(7, 14);
            this.radioButton44.Name = "radioButton44";
            this.radioButton44.Size = new System.Drawing.Size(66, 16);
            this.radioButton44.TabIndex = 6;
            this.radioButton44.TabStop = true;
            this.radioButton44.Text = "44100Hz";
            this.radioButton44.UseVisualStyleBackColor = true;
            // 
            // radioButton48
            // 
            this.radioButton48.AutoSize = true;
            this.radioButton48.Location = new System.Drawing.Point(7, 36);
            this.radioButton48.Name = "radioButton48";
            this.radioButton48.Size = new System.Drawing.Size(66, 16);
            this.radioButton48.TabIndex = 7;
            this.radioButton48.Text = "48000Hz";
            this.radioButton48.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton48);
            this.groupBox1.Controls.Add(this.radioButton44);
            this.groupBox1.Location = new System.Drawing.Point(5, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 57);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_stereo);
            this.groupBox2.Controls.Add(this.radioButton_mono);
            this.groupBox2.Location = new System.Drawing.Point(202, 34);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(119, 57);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            // 
            // radioButton_stereo
            // 
            this.radioButton_stereo.AutoSize = true;
            this.radioButton_stereo.Location = new System.Drawing.Point(7, 36);
            this.radioButton_stereo.Name = "radioButton_stereo";
            this.radioButton_stereo.Size = new System.Drawing.Size(59, 16);
            this.radioButton_stereo.TabIndex = 7;
            this.radioButton_stereo.Text = "ステレオ";
            this.radioButton_stereo.UseVisualStyleBackColor = true;
            // 
            // radioButton_mono
            // 
            this.radioButton_mono.AutoSize = true;
            this.radioButton_mono.Checked = true;
            this.radioButton_mono.Location = new System.Drawing.Point(7, 14);
            this.radioButton_mono.Name = "radioButton_mono";
            this.radioButton_mono.Size = new System.Drawing.Size(58, 16);
            this.radioButton_mono.TabIndex = 6;
            this.radioButton_mono.TabStop = true;
            this.radioButton_mono.Text = "モノラル";
            this.radioButton_mono.UseVisualStyleBackColor = true;
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(90, 123);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(141, 23);
            this.PlayButton.TabIndex = 10;
            this.PlayButton.Text = "再生";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 171);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.FileDlgButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button FileDlgButton;
        private System.Windows.Forms.RadioButton radioButton44;
        private System.Windows.Forms.RadioButton radioButton48;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_stereo;
        private System.Windows.Forms.RadioButton radioButton_mono;
        private System.Windows.Forms.Button PlayButton;
    }
}

