using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;

namespace PublicDomain
{
    internal partial class ImageView : Form
    {
        public ImageView()
        {
            InitializeComponent();
        }

        Image BaseImage;
        public Image img;
        double x = 1.0;
        bool ChangeFlag
        {
            set { ToolUndo.Enabled = value; }
            get { return ToolUndo.Enabled; }
        }

        public void SetBitmap(Bitmap image)
        {
            img = image;
            BaseImage = img;
            toolStripLabel1.Text = string.Format("幅[{0}],高[{1}]", img.Width, img.Height);
        }

        public Bitmap GetBitmap()
        {
            return img as Bitmap;
        }

        private void ToolMinus_Click(object sender, EventArgs e)
        {
            x -= 0.25;
            toolStripComboBox1.Text = x * 100 + "%";
            pictureBox1.Size = new Size((int)(img.Width * x), (int)(img.Height * x));
            Refresh();
        }

        private void ToolZero_Click(object sender, EventArgs e)
        {
            x = 1.0;
            toolStripComboBox1.Text = x * 100 + "%";
            pictureBox1.Size = new Size((int)(img.Width * x), (int)(img.Height * x));
            Refresh();
        }

        private void ToolPlus_Click(object sender, EventArgs e)
        {
            x += 0.25;
            toolStripComboBox1.Text = x * 100 + "%";
            pictureBox1.Size = new Size((int)(img.Width * x), (int)(img.Height * x));
            Refresh();
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(img, new Rectangle(0, 0, (int)(img.Width * x), (int)(img.Height * x)));
        }

        private void ImageView_Load(object sender, EventArgs e)
        {

            pictureBox1.Size = new Size((int)(img.Width * x), (int)(img.Height * x));
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                x = int.Parse(toolStripComboBox1.Text.Replace("%", "")) / 100f;
                if (x <= 0)
                {
                    toolStripComboBox1.Text = x * 100 + "%";
                }
                    pictureBox1.Size = new Size((int)(img.Width * x), (int)(img.Height * x));
            }
            catch
            {
                toolStripComboBox1.Text = x * 100 + "%";
            }
            Refresh();
        }

        private void ToolSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "画像を保存";
                sfd.Filter = "PNG形式|*.png|JPEG形式|*.jpg|GIF形式|*.gif|ビットマップ|*.bmp";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ImageFormat imf = ImageFormat.Png;
                    switch (sfd.FilterIndex)
                    {
                        case 0: imf = ImageFormat.Png; break;
                        case 1: imf = ImageFormat.Jpeg; break;
                        case 2: imf = ImageFormat.Gif; break;
                        case 3: imf = ImageFormat.Bmp; break;
                    }
                    img.Save(sfd.FileName, imf);
                }
            }
        }

        private void ToolCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(img);
            MessageBox.Show("画像をクリップボードにコピーしました");
        }

        private void ToolOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog sfd = new OpenFileDialog())
            {
                sfd.Title = "画像を開く";
                sfd.Filter = "PNG形式|*.png|JPEG形式|*.jpg|GIF形式|*.gif|ビットマップ|*.bmp|すべての画像|*.png;*.jpg;*.gif;*.bmp|すべてのファイル|*.*";
                sfd.FilterIndex = 5;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    img = Image.FromFile(sfd.FileName);
                    ChangeFlag = true;
                    ToolZero.PerformClick();
                    toolStripLabel1.Text = string.Format("幅[{0}],高[{1}]", img.Width, img.Height);
                }
            }
        }



        private void ToolPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                img = Clipboard.GetImage();
                ChangeFlag = true;
                Refresh();
                toolStripLabel1.Text = string.Format("幅[{0}],高[{1}]", img.Width, img.Height);
            }
            else
            {
                MessageBox.Show("画像がクリップボードにありません。","Bitmapビジュアライザ",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void ToolUndo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("変更を破棄してもとの画像を読み込みますか?", "画像の破棄", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                img = BaseImage;
                ChangeFlag = false;
                Refresh();
                toolStripLabel1.Text = string.Format("幅[{0}],高[{1}]", img.Width, img.Height);
            }
        }

        private void ImageView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChangeFlag)
            {
                DialogResult dr = MessageBox.Show("画像が変更されています。変更を適用しますか?", "画像の変更", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                
                if(dr == DialogResult.Yes)
                {
                    DialogResult = DialogResult.OK;
                }
                else if (dr == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void ToolEdit_Click(object sender, EventArgs e)
        {
            //テンポラリファイル名を取得
            string file = Path.ChangeExtension(Path.GetTempFileName(), ".png");
            //画像を保存
            img.Save(file, ImageFormat.Png);

            //Processオブジェクトを作成する
            Process p = new Process();
            //起動するファイルを指定する
            p.StartInfo.FileName = file;
            //イベントハンドラがフォームを作成したスレッドで実行されるようにする
            p.SynchronizingObject = this;
            p.StartInfo.Verb = "edit";
            //イベントハンドラの追加
            p.Exited += (a, b) =>
            {
                try
                {
                    if (MessageBox.Show("編集ソフトで編集された画像を読み込みます。\nよろしいですか？", "BitmapDebugger", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        FileStream fs = new FileStream(file, FileMode.Open);    //ファイルを開く
                        img = new Bitmap(fs);   //読み込む
                        ChangeFlag = true;
                        ToolZero.PerformClick();
                        fs.Close();             //Streamを閉じる
                    }
                        File.Delete(file);      //ファイルを消去。
                }
                catch
                {
                    MessageBox.Show("一時ファイルが見つかりません。\n編集結果は破棄されました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            };
            //プロセスが終了したときに Exited イベントを発生させる
            p.EnableRaisingEvents = true;
            //起動する
            p.Start();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = panel1.BackColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = cd.Color;
            }
        }

    }
}
