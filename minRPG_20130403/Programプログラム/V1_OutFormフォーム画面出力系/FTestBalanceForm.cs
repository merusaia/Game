using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PublicDomain
{
    public partial class FTestBalanceForm : Form
    {
        CGameManager・ゲーム管理者 game;
        // publicに、クラス外部から呼び出せるために作成
        public RichTextBox richtextBattle・戦闘;
        public FTestBalanceForm(CGameManager・ゲーム管理者 _g)
        {
            InitializeComponent();
            game = _g;
            game.setP_FBalance・バランス調整フォーム(this);

            richtextBattle・戦闘 = textBattle1;
            richTextBox1.Text = CCharaCreator・キャラ生成機.s_kaihi精神力が回避率に比例0_020.ToString();
            richTextBox2.Text = CCharaCreator・キャラ生成機.s_kai回避率MAX90.ToString();
            richTextBox3.Text = CCharaCreator・キャラ生成機.s_syutyu集中力が集中率に比例0_020.ToString();
            richTextBox4.Text = CCharaCreator・キャラ生成機.s_kuri賢さがクリティカル率に比例0_015.ToString();
            richTextBox5.Text = CCharaCreator・キャラ生成機.s_kaihiNum素早さが回避マス数に対数比例するlog基底数.ToString();
            richTextBox6.Text = CCharaCreator・キャラ生成機.s_bougyoNum行動力が防御マス数に対数比例するlog基底数.ToString();
        }

        private void FTestBalanceForm_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            CCharaCreator・キャラ生成機.s_kaihi精神力が回避率に比例0_020 = MyTools.parseDouble(richTextBox1.Text);
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            CCharaCreator・キャラ生成機.s_kai回避率MAX90 = MyTools.parseDouble(richTextBox2.Text);
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            CCharaCreator・キャラ生成機.s_syutyu集中力が集中率に比例0_020 = MyTools.parseDouble(richTextBox3.Text);
        }


        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            CCharaCreator・キャラ生成機.s_kuri賢さがクリティカル率に比例0_015 = MyTools.parseDouble(richTextBox4.Text);
        }

        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {
            CCharaCreator・キャラ生成機.s_kaihiNum素早さが回避マス数に対数比例するlog基底数 = MyTools.parseDouble(richTextBox5.Text);
        }

        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {
            CCharaCreator・キャラ生成機.s_bougyoNum行動力が防御マス数に対数比例するlog基底数 = MyTools.parseDouble(richTextBox6.Text);
        }


    }
}