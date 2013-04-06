namespace PublicDomain
{
    partial class ImageView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.ToolMinus = new System.Windows.Forms.ToolStripButton();
            this.ToolZero = new System.Windows.Forms.ToolStripButton();
            this.ToolPlus = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.ToolUndo = new System.Windows.Forms.ToolStripButton();
            this.ToolSave = new System.Windows.Forms.ToolStripButton();
            this.ToolOpen = new System.Windows.Forms.ToolStripButton();
            this.ToolCopy = new System.Windows.Forms.ToolStripButton();
            this.ToolPaste = new System.Windows.Forms.ToolStripButton();
            this.ToolEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.ToolMinus,
            this.ToolZero,
            this.ToolPlus,
            this.toolStripComboBox1,
            this.ToolUndo,
            this.ToolSave,
            this.ToolOpen,
            this.ToolCopy,
            this.ToolPaste,
            this.ToolEdit,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(408, 26);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(95, 23);
            this.toolStripLabel1.Text = "W[000],H[000]";
            // 
            // ToolMinus
            // 
            this.ToolMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolMinus.Image = ((System.Drawing.Image)(resources.GetObject("ToolMinus.Image")));
            this.ToolMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolMinus.Name = "ToolMinus";
            this.ToolMinus.Size = new System.Drawing.Size(23, 23);
            this.ToolMinus.Text = "縮小(&-)";
            this.ToolMinus.Click += new System.EventHandler(this.ToolMinus_Click);
            // 
            // ToolZero
            // 
            this.ToolZero.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolZero.Image = ((System.Drawing.Image)(resources.GetObject("ToolZero.Image")));
            this.ToolZero.ImageTransparentColor = System.Drawing.Color.White;
            this.ToolZero.Name = "ToolZero";
            this.ToolZero.Size = new System.Drawing.Size(23, 23);
            this.ToolZero.Text = "元の倍率(&0)";
            this.ToolZero.Click += new System.EventHandler(this.ToolZero_Click);
            // 
            // ToolPlus
            // 
            this.ToolPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolPlus.Image = ((System.Drawing.Image)(resources.GetObject("ToolPlus.Image")));
            this.ToolPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolPlus.Name = "ToolPlus";
            this.ToolPlus.Size = new System.Drawing.Size(23, 23);
            this.ToolPlus.Text = "拡大(&+)";
            this.ToolPlus.Click += new System.EventHandler(this.ToolPlus_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "10%",
            "25%",
            "50%",
            "75%",
            "90%",
            "100%",
            "110%",
            "125%",
            "150%",
            "175%",
            "200%",
            "250%",
            "300%"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(75, 26);
            this.toolStripComboBox1.Text = "100%";
            this.toolStripComboBox1.TextChanged += new System.EventHandler(this.toolStripComboBox1_TextChanged);
            // 
            // ToolUndo
            // 
            this.ToolUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolUndo.Enabled = false;
            this.ToolUndo.Image = ((System.Drawing.Image)(resources.GetObject("ToolUndo.Image")));
            this.ToolUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolUndo.Name = "ToolUndo";
            this.ToolUndo.Size = new System.Drawing.Size(23, 23);
            this.ToolUndo.Text = "変更を戻す";
            this.ToolUndo.Click += new System.EventHandler(this.ToolUndo_Click);
            // 
            // ToolSave
            // 
            this.ToolSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolSave.Image = ((System.Drawing.Image)(resources.GetObject("ToolSave.Image")));
            this.ToolSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolSave.Name = "ToolSave";
            this.ToolSave.Size = new System.Drawing.Size(23, 23);
            this.ToolSave.Text = "保存(&S)";
            this.ToolSave.Click += new System.EventHandler(this.ToolSave_Click);
            // 
            // ToolOpen
            // 
            this.ToolOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolOpen.Image = ((System.Drawing.Image)(resources.GetObject("ToolOpen.Image")));
            this.ToolOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolOpen.Name = "ToolOpen";
            this.ToolOpen.Size = new System.Drawing.Size(23, 23);
            this.ToolOpen.Text = "画像を開く";
            this.ToolOpen.Click += new System.EventHandler(this.ToolOpen_Click);
            // 
            // ToolCopy
            // 
            this.ToolCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolCopy.Image = ((System.Drawing.Image)(resources.GetObject("ToolCopy.Image")));
            this.ToolCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolCopy.Name = "ToolCopy";
            this.ToolCopy.Size = new System.Drawing.Size(23, 23);
            this.ToolCopy.Text = "画像をコピー(&C)";
            this.ToolCopy.Click += new System.EventHandler(this.ToolCopy_Click);
            // 
            // ToolPaste
            // 
            this.ToolPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolPaste.Image = ((System.Drawing.Image)(resources.GetObject("ToolPaste.Image")));
            this.ToolPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolPaste.Name = "ToolPaste";
            this.ToolPaste.Size = new System.Drawing.Size(23, 23);
            this.ToolPaste.Text = "画像を貼り付け";
            this.ToolPaste.Click += new System.EventHandler(this.ToolPaste_Click);
            // 
            // ToolEdit
            // 
            this.ToolEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolEdit.Image = ((System.Drawing.Image)(resources.GetObject("ToolEdit.Image")));
            this.ToolEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolEdit.Name = "ToolEdit";
            this.ToolEdit.Size = new System.Drawing.Size(23, 23);
            this.ToolEdit.Text = "画像編集ソフトを開いて編集";
            this.ToolEdit.Click += new System.EventHandler(this.ToolEdit_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(48, 22);
            this.toolStripButton1.Text = "背景色";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(388, 220);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 228);
            this.panel1.TabIndex = 2;
            // 
            // ImageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(408, 254);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Bitmapビジュアライザ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageView_FormClosing);
            this.Load += new System.EventHandler(this.ImageView_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton ToolMinus;
        private System.Windows.Forms.ToolStripButton ToolPlus;
        private System.Windows.Forms.ToolStripButton ToolZero;
        private System.Windows.Forms.ToolStripButton ToolSave;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton ToolCopy;
        private System.Windows.Forms.ToolStripButton ToolOpen;
        private System.Windows.Forms.ToolStripButton ToolPaste;
        private System.Windows.Forms.ToolStripButton ToolUndo;
        private System.Windows.Forms.ToolStripButton ToolEdit;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}