namespace PublicDomain
{
    partial class FScinarioCreateForm
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
            this.richTextScinario = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.but選択した行からテスト実行 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.butロード = new System.Windows.Forms.Button();
            this.butセーブ = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextScinario
            // 
            this.richTextScinario.AcceptsTab = true;
            this.richTextScinario.AutoWordSelection = true;
            this.richTextScinario.BulletIndent = 2;
            this.richTextScinario.EnableAutoDragDrop = true;
            this.richTextScinario.HideSelection = false;
            this.richTextScinario.Location = new System.Drawing.Point(12, 39);
            this.richTextScinario.Name = "richTextScinario";
            this.richTextScinario.Size = new System.Drawing.Size(397, 387);
            this.richTextScinario.TabIndex = 0;
            this.richTextScinario.Text = "";
            this.richTextScinario.SelectionChanged += new System.EventHandler(this.richTextScinario_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "台本テキスト（コピー＆ペーストして。ドラッグ＆ドロップは未対応）";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.but選択した行からテスト実行);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Location = new System.Drawing.Point(450, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 254);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "処理の追加（まだ作ってない）";
            // 
            // but選択した行からテスト実行
            // 
            this.but選択した行からテスト実行.Location = new System.Drawing.Point(11, 18);
            this.but選択した行からテスト実行.Name = "but選択した行からテスト実行";
            this.but選択した行からテスト実行.Size = new System.Drawing.Size(149, 23);
            this.but選択した行からテスト実行.TabIndex = 6;
            this.but選択した行からテスト実行.Text = "選択した行からテスト実行";
            this.but選択した行からテスト実行.UseVisualStyleBackColor = true;
            this.but選択した行からテスト実行.Click += new System.EventHandler(this.but選択した行からテスト実行_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(11, 82);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(60, 16);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "選択肢";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(11, 60);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "入力";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // butロード
            // 
            this.butロード.Location = new System.Drawing.Point(324, 12);
            this.butロード.Name = "butロード";
            this.butロード.Size = new System.Drawing.Size(48, 23);
            this.butロード.TabIndex = 4;
            this.butロード.Text = "ロード";
            this.butロード.UseVisualStyleBackColor = true;
            this.butロード.Click += new System.EventHandler(this.butロード_Click);
            // 
            // butセーブ
            // 
            this.butセーブ.Location = new System.Drawing.Point(378, 10);
            this.butセーブ.Name = "butセーブ";
            this.butセーブ.Size = new System.Drawing.Size(49, 23);
            this.butセーブ.TabIndex = 5;
            this.butセーブ.Text = "セーブ";
            this.butセーブ.UseVisualStyleBackColor = true;
            this.butセーブ.Click += new System.EventHandler(this.butセーブ_Click);
            // 
            // FScinarioCreateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 543);
            this.Controls.Add(this.butセーブ);
            this.Controls.Add(this.butロード);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextScinario);
            this.Name = "FScinarioCreateForm";
            this.Text = "FScinarioCreateForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextScinario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Button butロード;
        private System.Windows.Forms.Button butセーブ;
        private System.Windows.Forms.Button but選択した行からテスト実行;
    }
}