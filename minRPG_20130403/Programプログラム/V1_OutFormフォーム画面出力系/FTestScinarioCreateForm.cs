using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PublicDomain
{
    public partial class FScinarioCreateForm : Form
    {
        public static CGameManager・ゲーム管理者 game;
        public FScinarioCreateForm(CGameManager・ゲーム管理者 _g)
        {
            game = _g;
            InitializeComponent();
        }

        private void butロード_Click(object sender, EventArgs e)
        {
            string _text = "";
            _text = MyTools.loadFile_Dialog("txt");
            richTextScinario.Text = _text;
        }

        private void butセーブ_Click(object sender, EventArgs e)
        {
            string _savedText = richTextScinario.Text;
            MyTools.saveFile_Dialog(_savedText, "txt");
        }

        private int p_selectedScinarioIndex・選択した台本の位置インデックス = 1;
        private void but選択した行からテスト実行_Click(object sender, EventArgs e)
        {
            string _testScinario = richTextScinario.Text.Substring(p_selectedScinarioIndex・選択した台本の位置インデックス);
            game.sシナリオ進行開始(_testScinario);
        }

        private void richTextScinario_SelectionChanged(object sender, EventArgs e)
        {
            RichTextBox _richtext1 = (RichTextBox)sender;
            p_selectedScinarioIndex・選択した台本の位置インデックス = _richtext1.SelectionStart;
        }
    }
}
