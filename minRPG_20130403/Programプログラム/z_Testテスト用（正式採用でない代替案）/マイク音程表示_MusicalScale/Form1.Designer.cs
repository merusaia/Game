namespace MusicalScale
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
            this.checkBoxFile = new System.Windows.Forms.CheckBox();
            this.buttonScale = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonJust = new System.Windows.Forms.RadioButton();
            this.radioButtonEqual = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxC2 = new System.Windows.Forms.CheckBox();
            this.checkBoxC = new System.Windows.Forms.CheckBox();
            this.checkBoxH = new System.Windows.Forms.CheckBox();
            this.checkBoxD = new System.Windows.Forms.CheckBox();
            this.checkBoxA = new System.Windows.Forms.CheckBox();
            this.checkBoxE = new System.Windows.Forms.CheckBox();
            this.checkBoxG = new System.Windows.Forms.CheckBox();
            this.checkBoxF = new System.Windows.Forms.CheckBox();
            this.ButtonChord = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxFile
            // 
            this.checkBoxFile.AutoSize = true;
            this.checkBoxFile.Location = new System.Drawing.Point(323, 14);
            this.checkBoxFile.Name = "checkBoxFile";
            this.checkBoxFile.Size = new System.Drawing.Size(91, 16);
            this.checkBoxFile.TabIndex = 17;
            this.checkBoxFile.Text = "ファイルに保存";
            this.checkBoxFile.UseVisualStyleBackColor = true;
            // 
            // buttonScale
            // 
            this.buttonScale.Location = new System.Drawing.Point(4, 68);
            this.buttonScale.Name = "buttonScale";
            this.buttonScale.Size = new System.Drawing.Size(108, 23);
            this.buttonScale.TabIndex = 15;
            this.buttonScale.Text = "ドレミファソラシド";
            this.buttonScale.UseVisualStyleBackColor = true;
            this.buttonScale.Click += new System.EventHandler(this.buttonScale_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonJust);
            this.groupBox1.Controls.Add(this.radioButtonEqual);
            this.groupBox1.Location = new System.Drawing.Point(4, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(108, 61);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // radioButtonJust
            // 
            this.radioButtonJust.AutoSize = true;
            this.radioButtonJust.Location = new System.Drawing.Point(9, 39);
            this.radioButtonJust.Name = "radioButtonJust";
            this.radioButtonJust.Size = new System.Drawing.Size(59, 16);
            this.radioButtonJust.TabIndex = 15;
            this.radioButtonJust.TabStop = true;
            this.radioButtonJust.Text = "純正律";
            this.radioButtonJust.UseVisualStyleBackColor = true;
            // 
            // radioButtonEqual
            // 
            this.radioButtonEqual.AutoSize = true;
            this.radioButtonEqual.Location = new System.Drawing.Point(9, 18);
            this.radioButtonEqual.Name = "radioButtonEqual";
            this.radioButtonEqual.Size = new System.Drawing.Size(59, 16);
            this.radioButtonEqual.TabIndex = 14;
            this.radioButtonEqual.TabStop = true;
            this.radioButtonEqual.Text = "平均律";
            this.radioButtonEqual.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxC2);
            this.groupBox2.Controls.Add(this.checkBoxC);
            this.groupBox2.Controls.Add(this.checkBoxH);
            this.groupBox2.Controls.Add(this.checkBoxD);
            this.groupBox2.Controls.Add(this.checkBoxA);
            this.groupBox2.Controls.Add(this.checkBoxE);
            this.groupBox2.Controls.Add(this.checkBoxG);
            this.groupBox2.Controls.Add(this.checkBoxF);
            this.groupBox2.Controls.Add(this.ButtonChord);
            this.groupBox2.Location = new System.Drawing.Point(118, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(199, 100);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // checkBoxC2
            // 
            this.checkBoxC2.AutoSize = true;
            this.checkBoxC2.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxC2.Location = new System.Drawing.Point(175, 13);
            this.checkBoxC2.Name = "checkBoxC2";
            this.checkBoxC2.Size = new System.Drawing.Size(18, 30);
            this.checkBoxC2.TabIndex = 11;
            this.checkBoxC2.Text = "ド";
            this.checkBoxC2.UseVisualStyleBackColor = true;
            // 
            // checkBoxC
            // 
            this.checkBoxC.AutoSize = true;
            this.checkBoxC.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxC.Location = new System.Drawing.Point(6, 13);
            this.checkBoxC.Name = "checkBoxC";
            this.checkBoxC.Size = new System.Drawing.Size(18, 30);
            this.checkBoxC.TabIndex = 4;
            this.checkBoxC.Text = "ド";
            this.checkBoxC.UseVisualStyleBackColor = true;
            // 
            // checkBoxH
            // 
            this.checkBoxH.AutoSize = true;
            this.checkBoxH.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxH.Location = new System.Drawing.Point(150, 13);
            this.checkBoxH.Name = "checkBoxH";
            this.checkBoxH.Size = new System.Drawing.Size(19, 30);
            this.checkBoxH.TabIndex = 10;
            this.checkBoxH.Text = "シ";
            this.checkBoxH.UseVisualStyleBackColor = true;
            // 
            // checkBoxD
            // 
            this.checkBoxD.AutoSize = true;
            this.checkBoxD.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxD.Location = new System.Drawing.Point(30, 13);
            this.checkBoxD.Name = "checkBoxD";
            this.checkBoxD.Size = new System.Drawing.Size(18, 30);
            this.checkBoxD.TabIndex = 5;
            this.checkBoxD.Text = "レ";
            this.checkBoxD.UseVisualStyleBackColor = true;
            // 
            // checkBoxA
            // 
            this.checkBoxA.AutoSize = true;
            this.checkBoxA.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxA.Location = new System.Drawing.Point(126, 13);
            this.checkBoxA.Name = "checkBoxA";
            this.checkBoxA.Size = new System.Drawing.Size(17, 30);
            this.checkBoxA.TabIndex = 9;
            this.checkBoxA.Text = "ラ";
            this.checkBoxA.UseVisualStyleBackColor = true;
            // 
            // checkBoxE
            // 
            this.checkBoxE.AutoSize = true;
            this.checkBoxE.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxE.Location = new System.Drawing.Point(54, 13);
            this.checkBoxE.Name = "checkBoxE";
            this.checkBoxE.Size = new System.Drawing.Size(16, 30);
            this.checkBoxE.TabIndex = 6;
            this.checkBoxE.Text = "ミ";
            this.checkBoxE.UseVisualStyleBackColor = true;
            // 
            // checkBoxG
            // 
            this.checkBoxG.AutoSize = true;
            this.checkBoxG.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxG.Location = new System.Drawing.Point(102, 13);
            this.checkBoxG.Name = "checkBoxG";
            this.checkBoxG.Size = new System.Drawing.Size(18, 30);
            this.checkBoxG.TabIndex = 8;
            this.checkBoxG.Text = "ソ";
            this.checkBoxG.UseVisualStyleBackColor = true;
            // 
            // checkBoxF
            // 
            this.checkBoxF.AutoSize = true;
            this.checkBoxF.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxF.Location = new System.Drawing.Point(78, 13);
            this.checkBoxF.Name = "checkBoxF";
            this.checkBoxF.Size = new System.Drawing.Size(24, 30);
            this.checkBoxF.TabIndex = 7;
            this.checkBoxF.Text = "ファ";
            this.checkBoxF.UseVisualStyleBackColor = true;
            // 
            // ButtonChord
            // 
            this.ButtonChord.Location = new System.Drawing.Point(54, 67);
            this.ButtonChord.Name = "ButtonChord";
            this.ButtonChord.Size = new System.Drawing.Size(75, 23);
            this.ButtonChord.TabIndex = 12;
            this.ButtonChord.Text = "和音";
            this.ButtonChord.UseVisualStyleBackColor = true;
            this.ButtonChord.Click += new System.EventHandler(this.ButtonChord_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 108);
            this.Controls.Add(this.checkBoxFile);
            this.Controls.Add(this.buttonScale);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxFile;
        private System.Windows.Forms.Button buttonScale;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonJust;
        private System.Windows.Forms.RadioButton radioButtonEqual;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxC2;
        private System.Windows.Forms.CheckBox checkBoxC;
        private System.Windows.Forms.CheckBox checkBoxH;
        private System.Windows.Forms.CheckBox checkBoxD;
        private System.Windows.Forms.CheckBox checkBoxA;
        private System.Windows.Forms.CheckBox checkBoxE;
        private System.Windows.Forms.CheckBox checkBoxG;
        private System.Windows.Forms.CheckBox checkBoxF;
        private System.Windows.Forms.Button ButtonChord;
    }
}

