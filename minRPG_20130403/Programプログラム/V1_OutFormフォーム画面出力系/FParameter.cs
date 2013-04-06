using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PublicDomain
{
    public partial class FParameter : Form
    {
        public bool p_isShowIroPara6Only・true色パラメータ6色モード＿false12色モード = true;

        CGameManager・ゲーム管理者 game;
        CChara・キャラ p_c;
        List<double> p_iroPara12 = new List<double>();
        /// <summary>
        /// パラメータ振り分けボタンを一回押す前のパラメータ（ボーナスパラメータ計算のためなどに使う）
        /// </summary>
        List<double> p_beforeIroPara12 = new List<double>();
        /// <summary>
        /// 現在のボーナスパラメータ値
        /// </summary>
        int p_bonusParaボーナスパラ = 0;
        /// <summary>
        /// 割り振られた、振り分ける前のボーナスパラメータ値
        /// </summary>
        int p_bonusParaInit = 0;

        /// <summary>
        /// コンストラクタです。ゲームデータと、調整する色パラメータの初期値を入れてください。このコンストラクタのキャラはnullにすると名前が表示されないだけです。
        /// </summary>
        /// <param name="_g"></param>
        /// <param name="_iroPara12"></param>
        public FParameter(CGameManager・ゲーム管理者 _g, List<double> _iroPara12, int _bonusParaボーナスパラ, CChara・キャラ _cキャラ_or_null)
        {
            game = _g;
            game.setP_FParameter・パラメータ調整フォーム(this);
            // フォームコントロールの初期化（これをしてからじゃないとコントロールがnullになる、たぶん）
            InitializeComponent();
            // パラメータをセット
            setParameter(_iroPara12, _bonusParaボーナスパラ, _cキャラ_or_null);
        }
        /// <summary>
        /// コンストラクタです。ゲームデータと、調整する色パラメータのキャラと、ボーナスパラメータを指定してください。
        /// </summary>
        /// <param name="_g"></param>
        /// <param name="_iroPara12"></param>
        public FParameter(CGameManager・ゲーム管理者 _g, CChara・キャラ _cキャラ, int _bonusParaボーナスパラ)
            : this(_g, _cキャラ.Paras・パラメータ一括処理().getIro12色パラメータ(), _bonusParaボーナスパラ, _cキャラ) // 上のコンストラクタを実行
        {
        }
        /// <summary>
        /// フォームの更新です。設定された１２色パラメータ、ボーナスパラメータ、キャラを画面の初期値として設定し、画面を更新します。
        /// </summary>
        /// <param name="_iroPara12"></param>
        /// <param name="_bonusParaボーナスパラ"></param>
        /// <param name="_cキャラ"></param>
        public void setParameter(List<double> _iroPara12, int _bonusParaボーナスパラ, CChara・キャラ _cキャラ_or_null)
        {
            // 6色モードか、12色モードか
            if (p_isShowIroPara6Only・true色パラメータ6色モード＿false12色モード == true)
            {
                groupBox1.Visible = true;
                groupBox2.Visible = false;
                // [TODO]6色パラメータバーでも付ける？でもバトル用も作るなら、画像の方がいいかな。
            }
            else
            {
                groupBox1.Visible = true;
                groupBox2.Visible = true;
            }

            p_c = _cキャラ_or_null;
            p_iroPara12.Clear();
            p_iroPara12.AddRange(_iroPara12);
            p_beforeIroPara12.Clear();
            p_beforeIroPara12.AddRange(_iroPara12);
            p_bonusParaInit = _bonusParaボーナスパラ; // 初期値
            p_bonusParaボーナスパラ = _bonusParaボーナスパラ;

            // キャラ情報
            if (_cキャラ_or_null != null)
            {
                textBox1.Text = _cキャラ_or_null.Var(EVar.名前);
            }
            else
            {
                textBox1.Text = "";
            }
            // 初期色パラを代入
            richText1BonusPara.Text = p_bonusParaボーナスパラ.ToString();
            int i = -1;
            numericUpDown1.Value = (int)_iroPara12[++i];
            numericUpDown2.Value = (int)_iroPara12[++i];
            numericUpDown3.Value = (int)_iroPara12[++i];
            numericUpDown4.Value = (int)_iroPara12[++i];
            numericUpDown5.Value = (int)_iroPara12[++i];
            numericUpDown6.Value = (int)_iroPara12[++i];
            numericUpDown7.Value = (int)_iroPara12[++i];
            numericUpDown8.Value = (int)_iroPara12[++i];
            numericUpDown9.Value = (int)_iroPara12[++i];
            numericUpDown10.Value = (int)_iroPara12[++i];
            numericUpDown11.Value = (int)_iroPara12[++i];
            numericUpDown12.Value = (int)_iroPara12[++i];

            ToolTip _toolTip = new ToolTip();
            i = 1;
            _toolTip.SetToolTip(richIro1, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro2, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro3, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro4, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro5, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro6, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro7, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro8, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro9, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro10, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro11, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(richIro12, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));

            _toolTip.SetToolTip(numericUpDown1, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown2, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown3, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown4, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown5, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown6, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown7, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown8, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown9, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown10, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown11, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
            _toolTip.SetToolTip(numericUpDown12, CParas・パラメータ一覧.getParaExplan・各パラメータの説明テキストを取得(i++));
        }

        /// <summary>
        /// フォームに入力した値（名前・性別・武器・セリフ・色パラなど）を更新した、キャラを返します。
        /// </summary>
        /// <returns></returns>
        public CChara・キャラ getCharaData()
        {
            p_c.Paras・パラメータ一括処理().setIroParas色パラメータを代入(p_iroPara12);
            //List<double> _iroPara18 = new List<double>(12);
            //_iroPara18.AddRange(p_iroPara12);
            // 6個消す
            //for (int i = 1; i <= 6; i++)
            //{
            //    _iroPara18.RemoveAt(_iroPara12.Count - 1);
            //}

            p_c.setVar・変数を変更(EVar.名前, textBox1.Text);
            p_c.setVar・変数を変更(EVar.性別, comboBoxSeibetu.Text);
            p_c.setVar・変数を変更(EVar.メイン武器, comboMainWeapon.Text);
            p_c.setVar・変数を変更(EVar.登場セリフ, comboSerihu.Text);

            // 初期パラメータが変わったので、もうＬＶＵＰ処理をしてパラメータや総合値をセット
            game.LVUP・レベルアップ(p_c, 0);
            //ここでLV1のセットをやったら、このフォームがＬＶＵP時に使えなくなるCCharaCreator・キャラ生成機.setLV1Paras・LV1時の18色パラメータと総合値をセット(p_c);
            //CCharaCreator・キャラ生成機.setParas・現在のLVの18色パラメータと総合値をセット(p_c);

            // ●振り分けポイントが余っている場合、保存するのを忘れずに
            double _RestBonusPara = MyTools.parseDouble(richText1BonusPara.Text);
            p_c.setPara(EPara.LVParaBonus_未振り分けボーナスパラ, _RestBonusPara);

            return p_c;
        }


        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 決定ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (p_bonusParaボーナスパラ < 0)
            {
                MessageBox.Show("これで決定してあげたいのですが・・残念ながら・・、\nボーナス値が" + (int)(-1 * p_bonusParaボーナスパラ) + "ポイントオーバーしちゃってます(＞_＜)。\n他のキャラと優劣付け過ぎず、個性化をはかりたいので・・・。\n振り分けパラメータを0以上にしてもらえるないでしょうか？\nすみません、お願いしますm(_ _)m。");
            }
            else
            {
                game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true; // p_isOtherWindowEnded・他の画面での処理を完了して元の画面に戻った入力フラグ = true;
            }
        }
        /// <summary>
        /// 現在の画面の振り分け後のパラメータから、ボーナスパラメータを再計算し、画面を更新します。
        /// </summary>
        public void setBonusPara(){

            // 前回との差分から、ボーナスパラを更新
            p_iroPara12.Clear();

            // コントロールの値を代入
            p_iroPara12.Add(Double.Parse(numericUpDown1.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown7.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown2.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown8.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown3.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown9.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown4.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown10.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown5.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown11.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown6.Value.ToString()));
            p_iroPara12.Add(Double.Parse(numericUpDown12.Value.ToString()));


            double _delta = MyTools.getSum(p_iroPara12) - MyTools.getSum(p_beforeIroPara12);
            p_bonusParaボーナスパラ -= (int)_delta;
            // 合計値を表示
            int _sum = (int)(p_iroPara12[0] + p_iroPara12[2] + p_iroPara12[4] + p_iroPara12[6] + p_iroPara12[8] + p_iroPara12[10]);
            richTextShintaiSum.Text = _sum.ToString();
            _sum = (int)(p_iroPara12[1] + p_iroPara12[3] + p_iroPara12[5] + p_iroPara12[7] + p_iroPara12[9] + p_iroPara12[11]);
            richTextSeishinSum.Text = _sum.ToString();
            // ボーナス残りを表示
            richText1BonusPara.Text = p_bonusParaボーナスパラ.ToString();

            // 前回の色パラを保存
            p_beforeIroPara12.Clear();
            p_beforeIroPara12.AddRange(p_iroPara12);
        }

        // 数値調整ボタン

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            setBonusPara();
        }

        /// <summary>
        /// パラ自動生成のリセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            p_bonusParaボーナスパラ = p_bonusParaInit;

            // 例：身体パラ全250ポイントのうち、200までランダムで作成しておいて、あと100はボーナスパラメータとする
            p_c = CCharaCreator・キャラ生成機.getNormalSampleChara・標準サンプルキャラ自動生成(false,
                1, CCharaCreator・キャラ生成機.s_LV1Para_Iro6Sum・キャラＬＶ１標準の基本６色パラ総合値 - p_bonusParaボーナスパラ, 1.0, false);
            List<double> _iroPara12 = p_c.Paras・パラメータ一括処理().getIro12色パラメータ();
            p_iroPara12.Clear();
            p_beforeIroPara12.Clear();
            p_iroPara12.AddRange(_iroPara12);
            p_beforeIroPara12.AddRange(_iroPara12);

            // 初期色パラを代入
            richText1BonusPara.Text = p_bonusParaボーナスパラ.ToString();
            int i = -1;
            numericUpDown1.Value = (int)_iroPara12[++i];
            numericUpDown2.Value = (int)_iroPara12[++i];
            numericUpDown3.Value = (int)_iroPara12[++i];
            numericUpDown4.Value = (int)_iroPara12[++i];
            numericUpDown5.Value = (int)_iroPara12[++i];
            numericUpDown6.Value = (int)_iroPara12[++i];
            numericUpDown7.Value = (int)_iroPara12[++i];
            numericUpDown8.Value = (int)_iroPara12[++i];
            numericUpDown9.Value = (int)_iroPara12[++i];
            numericUpDown10.Value = (int)_iroPara12[++i];
            numericUpDown11.Value = (int)_iroPara12[++i];
            numericUpDown12.Value = (int)_iroPara12[++i];
        }

        private void comboMainWeapon_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 「-」で始まるリストは、去勢的に「剣」にする
            if (comboMainWeapon.Text.Substring(0, 1) == "-")
            {
                comboMainWeapon.Text = "剣";
            }
        }

        private void butShownParameterExplainText_Click(object sender, EventArgs e)
        {
            MessageBox.Show(CParas・パラメータ一覧.getParaExplan・12色パラメータの説明テキストを取得());
        }




    }
}