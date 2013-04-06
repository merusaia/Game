namespace PublicDomain
{
    public partial class FDrawForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonChange = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxYMin = new System.Windows.Forms.TextBox();
            this.textBoxYMax = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxXMin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxXMax = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonXYMinMaxChange = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DodgerBlue;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 480);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(658, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 63);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonChange
            // 
            this.buttonChange.Location = new System.Drawing.Point(658, 74);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(122, 29);
            this.buttonChange.TabIndex = 2;
            this.buttonChange.Text = "モードの切り替え";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 495);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "マウスがおかれているグラフの座標";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(715, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(65, 62);
            this.button2.TabIndex = 4;
            this.button2.Text = "画面クリア（Undoも削除）";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 507);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "押されているキー情報";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 519);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(658, 146);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Y最小値";
            // 
            // textBoxYMin
            // 
            this.textBoxYMin.Location = new System.Drawing.Point(712, 143);
            this.textBoxYMin.Name = "textBoxYMin";
            this.textBoxYMin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxYMin.Size = new System.Drawing.Size(68, 19);
            this.textBoxYMin.TabIndex = 9;
            this.textBoxYMin.Text = "0";
            // 
            // textBoxYMax
            // 
            this.textBoxYMax.Location = new System.Drawing.Point(712, 159);
            this.textBoxYMax.Name = "textBoxYMax";
            this.textBoxYMax.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxYMax.Size = new System.Drawing.Size(68, 19);
            this.textBoxYMax.TabIndex = 11;
            this.textBoxYMax.Text = "1000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(658, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "Y最大値";
            // 
            // textBoxXMin
            // 
            this.textBoxXMin.Location = new System.Drawing.Point(712, 184);
            this.textBoxXMin.Name = "textBoxXMin";
            this.textBoxXMin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxXMin.Size = new System.Drawing.Size(68, 19);
            this.textBoxXMin.TabIndex = 13;
            this.textBoxXMin.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(658, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "X最小値";
            // 
            // textBoxXMax
            // 
            this.textBoxXMax.Location = new System.Drawing.Point(712, 199);
            this.textBoxXMax.Name = "textBoxXMax";
            this.textBoxXMax.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxXMax.Size = new System.Drawing.Size(68, 19);
            this.textBoxXMax.TabIndex = 15;
            this.textBoxXMax.Text = "1000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(658, 202);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "X最大値";
            // 
            // buttonXYMinMaxChange
            // 
            this.buttonXYMinMaxChange.Location = new System.Drawing.Point(660, 224);
            this.buttonXYMinMaxChange.Name = "buttonXYMinMaxChange";
            this.buttonXYMinMaxChange.Size = new System.Drawing.Size(122, 34);
            this.buttonXYMinMaxChange.TabIndex = 16;
            this.buttonXYMinMaxChange.Text = "X・Y軸を更新";
            this.buttonXYMinMaxChange.UseVisualStyleBackColor = true;
            this.buttonXYMinMaxChange.Click += new System.EventHandler(this.buttonXYMinMaxChange_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "機能一覧"});
            this.comboBox1.Location = new System.Drawing.Point(658, 97);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 18;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // FDrawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 563);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.buttonXYMinMaxChange);
            this.Controls.Add(this.textBoxXMax);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxXMin);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxYMax);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxYMin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FDrawForm";
            this.Text = "テスト描画";
            this.Load += new System.EventHandler(this.FDrawForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FDrawForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxYMin;
        private System.Windows.Forms.TextBox textBoxYMax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxXMin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxXMax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonXYMinMaxChange;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}