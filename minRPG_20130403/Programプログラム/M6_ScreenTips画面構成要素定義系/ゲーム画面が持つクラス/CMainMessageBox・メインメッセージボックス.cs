using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms; // これを使う限りWindows環境のみ
using Yanesdk.Timer; // やねうらお様のゲームライブラリ
using Yanesdk.Draw;
using System.Diagnostics;
using PublicDomain;

namespace PublicDomain
{

    /// <summary>
    /// 主なシステム文・セリフやテキスト情報を表示するメッセージボックスです．今のところ，RichTextBoxなどを使っているため，Windows依存になっています．
    /// </summary>
    public class CWinMainTextBox・メインテキストボックス
    {
        Form p_usedForm;
        CGameManager・ゲーム管理者 game;

        //この辺は実際に文字にしておいた方が、ソースは見やすいかも。変更する時は置換でやるか
        //public static string s_改行コード = "\n";
        //public static string s_タブコード = "\t";
        //public static string s_新規ページコード = "【p】";
        //public static string s_待機文字コード = "【w】";
        //public static string s_一文字消す文字コード = "【b】";

        /// <summary>
        /// メッセージで表示する時（これらの文字が表示するした後）に一定時間止まる（待機する）１文字（…、・、。、！など）を列挙しています。
        /// </summary>
        public string[] p_waitWords・表示時に待機する文字たち = new string[] { 
            //"…",
            //"・",  
            //".",
            //"！",
            //"．",
            //"。",
            //"？",
            //"※",
            //"「",
            //"」",
            //"、",
            //"，",
            "\n", // 標準の改行コード
            "\t", // 標準のタブコード
            "【c】", // 以前のメッセージを白紙にする、独自の消去コード
            "【p】", // メッセージのボタン送りに使用する、独自の改ページコード。ボタンを押した後にメッセージボックスを先頭に移動する
            "【b】", // 手前の一文字を消す、独自のバックスペースコード
            "【w】", // 一定時間待機させる、独自の待機コード
        };
        /// <summary>
        /// p_waitWords・表示時に待機する文字たちをキーとした、それぞれの待機時間の倍率を格納した辞書です。
        /// </summary>
        public Dictionary<string, double> p_waitWordLength・待機文字毎に設定された待機時間の倍率 = new Dictionary<string, double>();
        #region その他の草案メモ
        // [Tips][List][初期化]List<string>型の初期化方法
        // string List<string> s_waitWords・表示時に待機する文字たち = new List<string>(new string[] { "……", "・・・" });      
        ///// <summary>
        ///// メッセージで表示する時（これらの文字が表示するした後）に一定時間止まる（待機する）１文字（…、・、。、！など）を列挙しています。
        ///// 同じ文字を複数列挙すると、更に待機時間が長くなります（文字数で比例増加）。
        ///// 確認する場合は１文字目だけ見るので、複数の文字列の待機には対応していません。
        ///// </summary>
        //public static string s_waitWordCounts・表示時に待機する文字たちの文字数 =
        //    "…………" +  // 4倍
        //    "・・・" +// 3倍  
        //    "..." +
        //    "！！！" +
        //    "．．" +// 2倍
        //    "。。" +
        //    "？？" +
        //    "※※" +
        //    "「「" +
        //    "」」" +
        //    "、" +
        //    "，" + // 1倍
        //    "\n" +
        //    "【p】"+
        //    "【w】";
        #endregion
        /// <summary>
        /// メッセージで表示する時（これらの文字が表示した後）に一定時間止まる（待機する）単位ミリ秒です。
        /// </summary>
        public static int s_waitCharMSec_Par1Char・表示時に待機するミリ秒単位
            = CGameManager・ゲーム管理者.s_waitMSecForMessage1CharShown・メッセージを１文字ずつ表示する時の単位ミリ秒 * 5;
        /// <summary>
        /// テキストをコピペ可能な文字列として表示するか(true)，コピペ不可な画像として表示するか(false)です．
        /// </summary>
        private static bool p_isShowText_ToString・表示する文字列はコピペ可能か = true;


        #region Windows標準のFontの文字描画に必要なプロパティ
        /// <summary>
        /// WindowsOS依存の，メインメッセージを表示するテキストボックスです．
        /// </summary>
        CMessageBox・メッセージボックス p_messageBox;
        /// <summary>
        /// ユーザが選択可能な入力ボックスです．
        /// </summary>
        CSelectBox・選択ボックス p_selectBox;
        /// <summary>
        /// ユーザが入力可能な入力ボックスを一括管理するグループボックスです．
        /// </summary>
        CInputGroupBox・入力グループ p_inputGroupBox;

        private int p_inputBoxNum・入力ボックス数 = 0;
        public int getP_inputBoxNum() { return p_inputBoxNum・入力ボックス数; }
        private int p_selectBoxNum・選択ボックス数 = 0;
        public int getP_selectBoxNum() { return p_selectBoxNum・選択ボックス数; }

        #region メッセージボックスクラス
        public class CMessageBox・メッセージボックス
        {
            public RichTextBox p_richTextBox;
            /// <summary>
            /// 引数の親クラスであるRichTextBoxの機能をすべて持った，子クラスを生成します．
            /// </summary>
            public CMessageBox・メッセージボックス(RichTextBox _baseClass_richTextBox)
                : base()
            {
                p_richTextBox = _baseClass_richTextBox;
               
            }
            public void show・表示()
            {
                this.p_richTextBox.Show();
            }
            public void hide・非表示()
            {
                this.p_richTextBox.Hide();
            }
        }
        public CMessageBox・メッセージボックス getP_messageBox・メッセージボックス()
        {
            return p_messageBox;
        }
        #endregion
        #region 選択ボックスクラス
        /// <summary>
        /// 選択ボックスを取得します．
        /// </summary>
        /// <returns></returns>
        public CSelectBox・選択ボックス getP_selectBox()
        {
            return p_selectBox;
        }
        public class CSelectBox・選択ボックス
        {
            public ListBox p_listBox;
            /// <summary>
            /// 引数の親クラスの機能をすべて持った，子クラスを生成します．
            /// </summary>
            public CSelectBox・選択ボックス(ListBox _baseClass_ListBox)
                : base()
            {
                p_listBox = _baseClass_ListBox;
            }
            public void show・表示()
            {
                p_listBox.Show();
            }
            public void hide・非表示()
            {
                p_listBox.Hide();
            }
            public int getResult・ただ一つ選択したリストの回答番号を取得()
            {
                int _index = p_listBox.SelectedIndex;
                if (_index != -1)
                {
                    return (_index + 1);
                }
                else
                {
                    return 0;
                }
            }
            public string getResult・ただ一つ選択したリストの文字列を取得()
            {
                object _item = p_listBox.SelectedItem;
                if (_item != null)
                {
                    return _item.ToString();
                }
                else
                {
                    return EAnswerSample・回答例._none・無回答.ToString();
                }
            }
        }
        #endregion
        #region 入力ボックスグループクラス
        /// <summary>
        /// 入力ボックスとラベルを一括して表示/非表示するグループボックスを取得します．
        /// </summary>
        /// <returns></returns>
        public CInputGroupBox・入力グループ getP_InputGroupBox()
        {
            return p_inputGroupBox;
        }
        /// <summary>
        /// ユーザが入力可能な入力ボックスとラベルを一括して表示/非表示するグループボックスです．
        /// </summary>
        public class CInputGroupBox・入力グループ
        {
            public GroupBox p_groupBox;
            List<CInputBox・入力ボックス> p_inputBox = new List<CInputBox・入力ボックス>();
            List<CInputLabel・入力ラベル> p_inputLabel = new List<CInputLabel・入力ラベル>();

            /// <summary>
            /// 引数の親クラスの機能をすべて持った，子クラスを生成します．入力ボックスを1つ作ります．
            /// </summary>
            public CInputGroupBox・入力グループ(GroupBox _baseClass_GroupBox, CInputLabel・入力ラベル _inputLabel1, CInputBox・入力ボックス _inputBox1)
                : this(_baseClass_GroupBox, _inputLabel1, _inputBox1, null, null, null, null)
            {
            }
            /// <summary>
            /// 引数の親クラスの機能をすべて持った，子クラスを生成します．入力ボックスを2つだけ作りたいときは，3つ目にはnullを入れてください．これ以上入力ボックスを追加したい場合は，addInputBoxメソッドを呼び出してください．
            /// </summary>
            public CInputGroupBox・入力グループ(GroupBox _baseClass_GroupBox, CInputLabel・入力ラベル _inputLabel1, CInputBox・入力ボックス _inputBox1, CInputLabel・入力ラベル _inputLabel2, CInputBox・入力ボックス _inputBox2, CInputLabel・入力ラベル _inputLabel3, CInputBox・入力ボックス _inputBox3)
                : base()
            {
                p_groupBox = _baseClass_GroupBox;
                if (_inputBox1 != null)
                {
                    addInputBox(_inputLabel1, _inputBox1);
                }
                if (_inputBox2 != null)
                {
                    addInputBox(_inputLabel2, _inputBox2);
                }
                if (_inputBox3 != null)
                {
                    addInputBox(_inputLabel3, _inputBox3);
                }
            }
            public void addInputBox(CInputLabel・入力ラベル _inputLabel, CInputBox・入力ボックス _inputBox)
            {
                if (_inputLabel != null)
                {
                    p_inputLabel.Add(_inputLabel);
                }
                else
                {
                    p_inputLabel.Add(new CInputLabel・入力ラベル(new RichTextBox()));
                }
                p_inputBox.Add(_inputBox);
            }
            public void setFocus(int _inputBoxIndex)
            {
                if (_inputBoxIndex <= p_inputBox.Count - 1)
                {
                    p_inputBox[_inputBoxIndex].p_textBox.Focus();
                }
            }
            /* なるべく外部に触らせない
             * public CInputBox・入力ボックス getInputBox(int _inputBoxIndex)
            {
                if (_inputBoxIndex <= p_inputBox.Count - 1)
                {
                    return p_inputBox[_inputBoxIndex];
                }
                else
                {
                    return null;
                }
            }*/

            public void show・表示(int _入力欄の数_全て非表示は0_全て表示は100)
            {
                // 表示する入力ボックス
                int i = 0;
                for (i = 0; i <= _入力欄の数_全て非表示は0_全て表示は100 - 1; i++)
                {
                    if (i <= p_inputBox.Count - 1)
                    {
                        p_inputLabel[i].show・表示();
                        p_inputBox[i].show・表示();
                    }
                    else
                    {
                        break;
                    }
                }
                i--; // ここまでのiが表示された入力ボックスのインデックス
                // _index+1からのインデックスを非表示にする
                for (int j = i + 1; j <= p_inputBox.Count - 1; j++)
                {
                    p_inputLabel[j].hide・非表示();
                    p_inputBox[j].hide・非表示();
                }
                this.p_groupBox.Show();

            }
            public void hide・非表示()
            {
                p_groupBox.Hide();
            }
            public void setInputDefault(int _inputBoxIndex, string _DefaultString)
            {
                if (_inputBoxIndex <= p_inputBox.Count - 1)
                {
                    p_inputBox[_inputBoxIndex].p_textBox.Text = _DefaultString;
                }
            }
            public void setLabel(int _inputBoxIndex, string _labelName)
            {
                if (_inputBoxIndex <= p_inputLabel.Count - 1)
                {
                    p_inputLabel[_inputBoxIndex].p_richTextBox.Text = _labelName;
                }
            }
            public string getLabel(int _inputBoxIndex)
            {
                if (_inputBoxIndex <= p_inputLabel.Count - 1)
                {
                    return p_inputLabel[_inputBoxIndex].p_richTextBox.Text;
                }
                else
                {
                    return "";
                }
            }
            public string getResult(int _inputBoxIndex)
            {
                if (_inputBoxIndex <= p_inputLabel.Count - 1)
                {
                    return p_inputBox[_inputBoxIndex].p_textBox.Text;
                }
                else
                {
                    return "";
                }
            }
            public string getResult(string _labelName)
            {
                string _inputResultString = "";
                int i = 0;
                foreach (CInputLabel・入力ラベル _label in p_inputLabel)
                {
                    if (_label.p_richTextBox.Text == _labelName)
                    {
                        _inputResultString = p_inputBox[i].p_textBox.Text;
                        break;
                    }
                    i++;
                }
                return _inputResultString;
            }
        }
        /// <summary>
        /// ユーザが入力可能な入力ボックスです．
        /// </summary>
        public class CInputBox・入力ボックス
        {
            public TextBox p_textBox;
            /// <summary>
            /// 引数の親クラスの機能をすべて持った，子クラスを生成します．
            /// </summary>
            public CInputBox・入力ボックス(TextBox _baseClass_TextBox)
                : base()
            {
                p_textBox = _baseClass_TextBox;
            }
            public void show・表示()
            {
                p_textBox.Show();
            }
            public void hide・非表示()
            {
                p_textBox.Hide();
            }
        }
        /// <summary>
        /// ユーザが入力可能な入力欄の説明をするラベルです．
        /// </summary>
        public class CInputLabel・入力ラベル
        {
            public RichTextBox p_richTextBox;
            /// <summary>
            /// 引数の親クラスの機能をすべて持った，子クラスを生成します．
            /// </summary>
            public CInputLabel・入力ラベル(RichTextBox _baseClass_RichTextBox)
                : base()
            {
                p_richTextBox = _baseClass_RichTextBox;
            }
            public void show・表示()
            {
                p_richTextBox.Show();
            }
            public void hide・非表示()
            {
                p_richTextBox.Hide();
            }
        }
        #endregion

        /// <summary>
        /// メッセージ・選択・入力ボックスを全て表示します．
        /// </summary>
        public void show・表示()
        {
            p_messageBox.show・表示();
            getP_selectBox().show・表示();
            getP_InputGroupBox().p_groupBox.Show();
        }
        /// <summary>
        /// メッセージ・選択・入力ボックスを全て非表示にします．
        /// </summary>
        public void hide・非表示()
        {
            p_messageBox.hide・非表示();
            getP_selectBox().hide・非表示();
            getP_InputGroupBox().hide・非表示();
        }
        public void hideSelect・選択ボックスを非表示()
        {
            getP_selectBox().hide・非表示();
        }
        public void hideInput・入力ボックスを非表示()
        {
            getP_InputGroupBox().hide・非表示();
        }

        #region 文字表示フォント
        /// <summary>
        /// メインメッセージボックスのフォントサイズです．適時変更可能です．
        /// </summary>
        float p_fontSizeShown・表示文字サイズ = 20.0f;
        public float getP_ShownFontSize()
        {
            return p_fontSizeShown・表示文字サイズ;
        }
        System.Drawing.Font p_shownFont・表示文字フォント = new System.Drawing.Font(EFont.GOSHIKKU_UI・ゴシック最短, 20.0f);
        #endregion
        #endregion

        #region yaneSDKの画像としての文字描画に必要なプロパティ

        GlTexture p_fontTexture = new GlTexture();
            Yanesdk.Draw.Font p_font = new Yanesdk.Draw.Font();
            Surface p_surface = new Surface();
       #endregion

        /// <summary>
        /// ●●●Windows依存のメインテキストボックスのコンストラクタです．
        /// </summary>
        /// <param name="_メッセージボックス"></param>
        /// <param name="_入力グループボックス"></param>
        public CWinMainTextBox・メインテキストボックス(CGameManager・ゲーム管理者 _g, Form _usedForm, CMessageBox・メッセージボックス _メッセージボックス, CSelectBox・選択ボックス _選択ボックス, CInputGroupBox・入力グループ _入力グループボックス)
        {
            game = _g;
            p_usedForm = _usedForm;
            p_messageBox = _メッセージボックス;
            p_selectBox = _選択ボックス;
            p_inputGroupBox = _入力グループボックス;

            // Dictionaryの初期化(.NET Framework 2.0じゃ、プロパティ定義行での一括初期化はできない。3.0から。)
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("【b】", 10.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("【w】", 5.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("…", 4.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("！",3.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("．",3.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("。",3.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("？",3.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("・", 2.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add(".", 2.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("，", 2.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add(",", 2.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("※", 2.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("「",1.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("」",1.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("、",1.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("【p】",1.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("\n",1.0);
            p_waitWordLength・待機文字毎に設定された待機時間の倍率.Add("\t", 1.0);

        }
        /// <summary>
        /// コンストラクタです．Windows依存の場合は，フォーム上に作成した，メインのメッセージを表示するRichTextBoxと，選択肢ボックスListBoxと，入力ボックスを一括管理するグループGroupBoxとそのラベル(RichTextBox)と入力ボックス(TextBox)を引数で渡してください．
        /// 
        /// ※ここで作成する入力ボックス（入力欄）は最大数であり，表示時に調整できます．1～2つだけ作りたい場合は，他の引数にnullを入れてください．4つ以上作りたい場合は，入力グループボックスのaddInputBoxを呼び出してください．
        /// </summary>
        /// <param name="_メッセージボックス"></param>
        /// <param name="_入力グループボックス"></param>
        public CWinMainTextBox・メインテキストボックス(CGameManager・ゲーム管理者 _g, Form _usedForm, RichTextBox _メッセージボックス, ListBox _選択ボックス, GroupBox _入力グループボックス, RichTextBox _入力ラベル1, TextBox _入力欄1, RichTextBox _入力ラベル2, TextBox _入力欄2, RichTextBox _入力ラベル3, TextBox _入力欄3)
            : this(_g, _usedForm, 
                new CMessageBox・メッセージボックス(_メッセージボックス), 
                new CSelectBox・選択ボックス(_選択ボックス), 
                new CInputGroupBox・入力グループ(_入力グループボックス, 
                new CInputLabel・入力ラベル(_入力ラベル1), new CInputBox・入力ボックス(_入力欄1), new CInputLabel・入力ラベル(_入力ラベル2), new CInputBox・入力ボックス(_入力欄2), new CInputLabel・入力ラベル(_入力ラベル3), new CInputBox・入力ボックス(_入力欄3))
            )
        {
        }

        #region ●●●●メインテキストを表示するメソッド
        /// <summary>
        /// 画面のメインメッセージボックスに指定したテキストを表示します．画面表示に成功した場合はtrue，何らかの理由で画面に表示できなかった場合はfalseを返します．
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public bool showMessage・メッセージを表示(string _message・表示文字列, bool _isShownPer1Char・true一文字ずつ表示するか_false一気に表示するか)
        {
            //if (p_messageBox.showMessage・メッセージ表示(_isNewLine・改行するか, _string) == 1)
            if (_isShownPer1Char・true一文字ずつ表示するか_false一気に表示するか == true
                && CGameManager・ゲーム管理者.s_waitMSecForMessage1CharShown・メッセージを１文字ずつ表示する時の単位ミリ秒 != 0)
            {
                if (_showTextB・メッセージを少しずつ表示(_message・表示文字列) == 1) { return true; } else { return false; }
            }
            else
            {
                if (_showTextA・メッセージ一挙表示(_message・表示文字列) == 1) { return true; } else { return false; }
            }
        }


        /// <summary>
        /// 画面のテキストボックスに引数の文字列を一挙に表示し，1を返します．
        /// </summary>
        /// <returns></returns>
        private int _showTextA・メッセージ一挙表示(string _string)
        {
            // ■特殊コードがあればそれを実際に表示する文字列から消し、その後特殊コードの機能を実行
            // 待機文字コードが含まれていたら，それまでを表示して一定時間待つ
            if (MyTools.getWord_First(_string, p_waitWords・表示時に待機する文字たち) != "")
            {
                // 待機が必要な文字ごとに区切る
                List<string> _words = MyTools.getWords_InString(_string, 0, 1, MyTools.EStringCharType.t0_None_未定義_何でもＯＫ,
                    true, p_waitWords・表示時に待機する文字たち);
                foreach (string _word in _words)
                {
                    if (_word == "") continue; // 空なら、次の単語へ

                    // _wordには、文字列の末尾に待機文字（「…」「！」、もしくは特殊な文字コードが含まれている。
                    // ただし、_wordsの中で最後_wordだけは含まれていない
                    string _waitWord = MyTools.getWord_Last(_word, p_waitWords・表示時に待機する文字たち);
                    string _shownWord = _word;
                    // 表示する文字から、改行以外の特殊な文字コードを消す
                    // 改行"\n"とタグ文字"\t"はテキストボックスが自動的に認識してくれるから、消さなくていい
                    if (_waitWord == "【c】" || _waitWord == "【p】" || _waitWord == "【b】" || _waitWord == "【w】")
                    {
                        _shownWord = _word.Replace(_waitWord, "");
                    }

                    // (A)テキストを実際に表示。
                    if (p_isShowText_ToString・表示する文字列はコピペ可能か == true)
                    {
                        _printText_ToTextBox(_shownWord);
                    }
                    else
                    {
                        _printText_ToImage(_shownWord);
                    }

                    // 表示後、待機文字の決められた時間だけ待機
                    int _waitWordMSec = getWaitWordMSec・文字の待機時間ミリ秒を計算(_waitWord);
                    game.waitウェイト(_waitWordMSec);

                    // 待機後、待機文字列に特殊な文字コードが含まれていたら、それらの機能を実行
                    if (_waitWord == "【c】")
                    {
                        // 初期化
                        clear・消去＿メッセージボックス内のテキストを初期化();
                    }
                    if (_waitWord == "【p】")
                    {
                        // 改ページ
                        newPage・改ページ＿ボタン待ち後に最後の行が見えなくなるまでスクロールして先頭へ(true);
                    }
                    else if (_waitWord == "【b】")
                    {
                        // 一文字消す
                        string _nowText = p_messageBox.p_richTextBox.Text;
                        p_messageBox.p_richTextBox.Text = _nowText.Substring(0, Math.Max(0, _nowText.Length - 1));
                        // 戻り文字は、消したことがわかるようにするため、消した後も待つ。ただ、消した後は待機コードにしましょうか
                        game.waitウェイト(getWaitWordMSec・文字の待機時間ミリ秒を計算("【w】"));
                    }
                    else if (_waitWord == "【w】")
                    {
                        // 待機コード。先ほど文字表示後に待ったからもう何もしなくていい
                    }
                }
            }
            else
            {
                // (A)特殊な文字コードが存在しないので、_stringをそのまま表示。
                if (p_isShowText_ToString・表示する文字列はコピペ可能か == true)
                {
                    _printText_ToTextBox(_string);
                }
                else
                {
                    _printText_ToImage(_string);
                }
            }

            // テキストボックスが非表示だったら、出現させる
            if (p_messageBox.p_richTextBox.Visible == false)
            {
                p_messageBox.p_richTextBox.Visible = true;
            }

            // (B)（デバッグ用に）コンソールとログに表示
            game.DEBUGデバッグ一行出力(ELogType.l1_メインメッセージ, _string);
            p_pastTextShownLength・クラス生成から今まで表示したテキストの文字数 += _string.Length;

            // (C) テキストボックスを最後の行にする，最後まで移動
            Refresh・最後の行が見えるようにスクロール();
            // (D)スクロールを他のコントロールに映す
            game.getP_gameWindow・ゲーム画面().getP_usedFrom().focusMainControl・フォーカスをメインコントロールに移す();
            return 1;
        }
        /// <summary>
        /// 画面のテキストボックスに引数の文字列を表示します．このメソッドが実行されてから，（同期的に）指定されたスピードで1文字ずつ表示し，全ての文字列を表示し終えたら1を返します．
        /// </summary>
        /// <returns></returns>
        private int _showTextB・メッセージを少しずつ表示(string _string)
        {
            // (A)テキストを表示

            // 1文字に一回じゃウェイト0でも時間がかかってしまうし、フレームレート的に酔うし頭痛に悪いので、
            // 5～20文字に一回だけ待つことにする
            int _onceShownLength・一度に表示する文字数 = 5;

            // 何かボタンを押したら一挙表示に切り替えるため、前回のテキストを保存しておく
            string _beforeShownText・前回のテキスト = p_messageBox.p_richTextBox.Text;
            int _shownMessageLength = 0;

            // テキストボックスが非表示だったら、出現させる
            if (p_messageBox.p_richTextBox.Visible == false)
            {
                p_messageBox.p_richTextBox.Visible = true;
            }

            // ■特殊コードがあれば、それが出現する前の文字表示後に、特殊コードの機能を実行
            // 特殊な操作が必要な単語ごとに区切る
            List<string> _words = MyTools.getWords_InString(_string, 0, 1, MyTools.EStringCharType.t0_None_未定義_何でもＯＫ,
                true, p_waitWords・表示時に待機する文字たち);
            int _nowWordIndex = 0;
            // _stringが全部表示されるまで、ループ
            while (_shownMessageLength < _string.Length)
            {
                // 何かボタンが押されたら、一挙表示
                if (game.ibボタンを押したか_連射非対応() == true)
                {
                    // 前回のテキストに戻す（このループで書いたメッセージを消す）
                    p_messageBox.p_richTextBox.Text = _beforeShownText・前回のテキスト;
                    Refresh・最後の行が見えるようにスクロール();
                    // 一挙に表示に切り替え
                    _showTextA・メッセージ一挙表示(_string);
                    // ループを抜ける
                    _shownMessageLength = _string.Length; //break;
                }
                else
                {
                    string _item = MyTools.getListValue<string>(_words, _nowWordIndex);
                    if (_item == null)
                    {
                        break; // 配列外（全部終わった終わった）なら、ループを抜ける
                    }
                    else if (_item == "")
                    {
                        _nowWordIndex++;
                        continue; // 空なら、次の単語へ
                    }
                    // _itemには、文字列の末尾に待機文字（「…」「！」、もしくは特殊な文字コードが含まれている。
                    // ただし、_wordsの中で最後_wordだけは含まれていない
                    string _waitWord = MyTools.getWord_Last(_item, p_waitWords・表示時に待機する文字たち);
                    string _word = _item;
                    // 表示する文字から、改行以外の特殊な文字コードを消す
                    // 改行"\n"とタグ文字"\t"はテキストボックスが自動的に認識してくれるから、消さなくていい
                    if (_waitWord == "【c】" || _waitWord == "【p】" || _waitWord == "【b】" || _waitWord == "【w】")
                    {
                        _word = _item.Replace(_waitWord, "");
                    }
                    // 特殊コードをとった文字列_shownWordを、少しずつ表示
                    int _shownWordNum = 0;
                    while (_shownWordNum < _word.Length)
                    {
                        // ========■メッセージを少しずつ表示=================
                        // 1文字に一回じゃウェイト0でも時間がかかってしまうし、フレームレート的に酔うし頭痛に悪いので、5～20文字に一回だけ待つ
                        int _length・表示数 = Math.Min(_onceShownLength・一度に表示する文字数, _word.Substring(_shownWordNum).Length);
                        string _shownText = _word.Substring(_shownWordNum, _length・表示数);
                        if (p_isShowText_ToString・表示する文字列はコピペ可能か == true)
                        {
                            _printText_ToTextBox(_shownText);
                        }
                        else
                        {
                            _printText_ToImage(_shownText);
                        }
                        wait_PerOnceShownText・少しずつ表示する時の待ち時間だけ待つ(_onceShownLength・一度に表示する文字数);
                        // 文字数を追加
                        _shownWordNum += _shownText.Length;
                        // ========■メッセージを少しずつ表示、終わり=================
                    }
                    // 表示後、待機文字の決められた時間だけ待機
                    int _waitWordMSec = getWaitWordMSec・文字の待機時間ミリ秒を計算(_waitWord);
                    game.waitウェイト(_waitWordMSec);

                    // 待機後、待機文字列に特殊な文字コードが含まれていたら、それらの機能を実行
                    if (_waitWord == "【c】")
                    {
                        // 初期化
                        clear・消去＿メッセージボックス内のテキストを初期化();
                    }
                    if (_waitWord == "【p】")
                    {
                        // 改ページ
                        newPage・改ページ＿ボタン待ち後に最後の行が見えなくなるまでスクロールして先頭へ(true);
                    }
                    else if (_waitWord == "【b】")
                    {
                        // 一文字消す
                        string _nowText = p_messageBox.p_richTextBox.Text;
                        p_messageBox.p_richTextBox.Text = _nowText.Substring(0, Math.Max(0, _nowText.Length - 1));
                        // 戻り文字は、消したことがわかるようにするため、消した後も待つ。ただ、消した後は待機コードにしましょうか
                        game.waitウェイト(getWaitWordMSec・文字の待機時間ミリ秒を計算("【w】"));
                    }
                    else if (_waitWord == "【w】")
                    {
                        // 待機コード。先ほど文字表示後に待ったからもう何もしなくていい
                    }
                    // 一通り特殊なコードの機能実行が終わってから、表示文字数を更新
                    _shownMessageLength += _item.Length;
                    // 次のワード
                    _nowWordIndex++;
                }
            }
            // (B)（デバッグ用に）コンソールとログに表示
            game.DEBUGデバッグ一行出力(ELogType.l1_メインメッセージ, _string);
            p_pastTextShownLength・クラス生成から今まで表示したテキストの文字数 += _string.Length;
            // (C) テキストボックスを最後の行にする，最後まで移動
            Refresh・最後の行が見えるようにスクロール();
            // (D)スクロールを他のコントロールに映す
            game.getP_gameWindow・ゲーム画面().getP_usedFrom().focusMainControl・フォーカスをメインコントロールに移す();
            return 1;
        }

        private void wait_PerOnceShownText・少しずつ表示する時の待ち時間だけ待つ(int _onceShownLength・一度に表示する文字数)
        {
            // 60フレーム以上のものをみせたらあかん。酔いと頭と脳に悪い（体験談）
            // もっと悪いのは、メッセージボックスがガタガタと変にスクロールする時に酔うみたい…
            // 要するに、こうフレームレート画面揺れと点滅がまずい。
            if (CGameManager・ゲーム管理者.s_waitMSecForMessage1CharShown・メッセージを１文字ずつ表示する時の単位ミリ秒 > 0)
            {
                int _waitMSec = _onceShownLength・一度に表示する文字数 * CGameManager・ゲーム管理者.s_waitMSecForMessage1CharShown・メッセージを１文字ずつ表示する時の単位ミリ秒;
                if (_waitMSec < 17) _waitMSec = 17; // 60フレーム以上のものをみせちゃいかん。酔いと頭と脳に悪い（体験談）
                game.waitウェイト(_waitMSec);
            }
        }
        public int p_pastTextShownLength・クラス生成から今まで表示したテキストの文字数 = 0;
        public string p_pastTextShown・今まで表示したテキスト = "";
        /// <summary>
        /// メイン画面にコピペ可能なテキストとして表示します．
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        private int _printText_ToTextBox(string _string)
        {
            // 特殊な文字コードが存在しないと保障された_stringを、そのまま表示。
            // (a)テキストを追加して、スクロールバーを最後の行に見えるようにする（ガタガタを防ぐため、下方向にしか動かないように工夫している）
            p_messageBox.p_richTextBox.Text += _string;
            // ここだけ、フォーカスをオンしたまま、最後の行まで移動（でないとガタガタスクロールが表示されて目に毒）
            MyTools.showRichTextBox_EndLine(p_messageBox.p_richTextBox);

            // (b)AppendTextを使うと、スクロールバーが自動的に最後の行が見えるようになるが、こうフレームでやるとガタガタするので、おススメできない
            //p_messageBox.p_richTextBox.AppendText(_string);
            //これをつけなくてもあとでフレーム毎につけるからいいApplication.DoEvents();


            return 1;
        }
        /// <summary>
        /// 引数の単語が待機文字だった場合、実際に待機するミリ秒を取得します。待機文字じゃない場合、0が返ります。
        /// </summary>
        private int getWaitWordMSec・文字の待機時間ミリ秒を計算(string _waitWord)
        {
            double _waitRate・待機倍数 = 0.0;
            if (p_waitWordLength・待機文字毎に設定された待機時間の倍率.ContainsKey(_waitWord) == true)
            {
                _waitRate・待機倍数 = p_waitWordLength・待機文字毎に設定された待機時間の倍率[_waitWord];
            }
            int _waitWordMSec = (int)(s_waitCharMSec_Par1Char・表示時に待機するミリ秒単位
                * CGameManager・ゲーム管理者.s_gameSpeed・ゲームの速さ倍率 * _waitRate・待機倍数);
            // 60フレーム以上のものをみせたらあかん。酔いと頭と脳に悪い（体験談）
            // もっと悪いのは、メッセージボックスがガタガタと変にスクロールする時に酔うみたい…
            // 要するに、こうフレームレート画面揺れと点滅がまずい。
            if (_waitWordMSec != 0 && _waitWordMSec < 17) _waitWordMSec = 17;

            return _waitWordMSec;
        }
        /// <summary>
        /// Windowsフォントに依存しない？，テキストをコピペ不可の画像として生成して表示します．
        /// ※まだ未完成です。
        /// </summary>
        /// <returns></returns>
        private int _printText_ToImage(string _string)
        {
            // [TODO]改行だった場合、行数を変えないと…。しかも右端で折り返す処理も書いてないよ…
            if (_string.Contains("\n") == true)
            {
            }

            // 前処理
            int _size = (int)p_fontSizeShown・表示文字サイズ;
            p_font.Load("msgothic.ttc", _size);
            //          msgothic.ttc : MSゴシック
            //			msmincho.ttc : MS明朝
            //m_font.Open(0, 30);
            p_font.SetColor(200, 0, 50);
            p_font.Style = 1;

            p_font.DrawBlendedUnicode(p_surface, _string); //drawSolid のα付きサーフェースを返すバージョン。 綺麗なかわりに、遅い。
            //m_font.DrawSolidUnicode(p_surface, _str); // 早いが，少し汚いかも

            p_fontTexture.SetSurface(p_surface);

            return 1;
        }
        /// <summary>
        /// メインメッセージボックスの最後の行が一番下に表示されるまでスクロールします。
        ///　※最後の行が先頭に来るまでスクロールしないので、注意してください。
        /// </summary>
        private void Refresh・最後の行が見えるようにスクロール()
        {
            // アンフォーカスするのを一旦あきらめたバーション
            //MyTools.showRichTextBox_EndLine(p_messageBox.p_richTextBox);
            // アンフォーカスしてできるだけテキスト編集カーソル「I」を出来るだけ消したバージョン
            MyTools.showRichTextBox_EndLine_UnshowCursor(p_messageBox.p_richTextBox, p_usedForm);
        }
        /// <summary>
        /// メインメッセージボックスの最後の行が見えなくなるまでスクロールします。
        /// </summary>
        private void RefreshPage・最後の行が見えなくなるまでスクロール()
        {
            // 3.メインメッセージボックスの行数を計算してその分だけスクロール（１～２行多い位の、だいたいでいい）
            int _lineNum_nowMessageBox・行数 = p_messageBox.p_richTextBox.Height / (int)(p_messageBox.p_richTextBox.Font.Size * 1.5);
            if (p_isShowText_ToString・表示する文字列はコピペ可能か == true)
            {
                // 文字をテキストで表示している場合

                // メッセージボックスの行数分、改行を追加
                for (int i = 0; i < _lineNum_nowMessageBox・行数; i++)
                {
                    _printText_ToTextBox("\n");
                    // デフォルトの改行時のウェイト時間、を使わない
                    //int _waitMSec = getWaitWordMSec・文字の待機時間ミリ秒を計算("\n");
                    // 改ページだけ特別、一行毎のスピードを遅くする（待機コードくらいでいいか）
                    int _waitMSec = getWaitWordMSec・文字の待機時間ミリ秒を計算("【w】");
                    game.waitウェイト(_waitMSec);
                }
            }
            else
            {
                // 文字を画像で描画している場合
                // とりあえず、消す
                // メッセージボックスの行数分、改行を追加して、消す
                for (int i = 0; i < _lineNum_nowMessageBox・行数; i++)
                {
                    _printText_ToImage("\n");
                    // デフォルトの改行時のウェイト時間、を使わない
                    //int _waitMSec = getWaitWordMSec・文字の待機時間ミリ秒を計算("\n");
                    // 改ページだけ特別、一行毎のスピードを遅くする（待機コードくらいでいいか）
                    int _waitMSec = getWaitWordMSec・文字の待機時間ミリ秒を計算("【w】");
                    game.waitウェイト(_waitMSec);
                }
            }
        }
        private bool clear・消去＿メッセージボックス内のテキストを初期化()
        {
            bool _isSucces = true;
            // 文字履歴はどこかに格納しないといけないでしょう。とりあえず、念のため、全部格納。
            p_pastTextShown・今まで表示したテキスト += p_messageBox.p_richTextBox.Text;

            // リッチテキストボックスはテキスト量が多いとメモリを食うようなので、頻繁に消すとメモリ節約になる。
            // できれば、スクロールで履歴は別の画面でやった方がいいだろう。
            p_messageBox.p_richTextBox.Text = "";

            return _isSucces;
        }
        /// <summary>
        /// 改ページ（ボタン待ち後、メインメッセージボックスの最後の行が見えなくなるまでスクロール）します。
        /// </summary>
        /// <returns></returns>
        private bool newPage・改ページ＿ボタン待ち後に最後の行が見えなくなるまでスクロールして先頭へ(bool _isClearPastText・改ページ後に過去のテキストを消すか)
        {
            game.DEBUGデバッグ一行出力("（【p】が含まれるメッセージ）: ユーザの任意ボタン入力待ち中…");
            bool _isSucces = true;
            // メッセージボックスの行数分、改行を追加し、最後の行が戦闘になるまでスクロールする

            // 1.まず、最後の行が見えるようになるまでスクロール
            Refresh・最後の行が見えるようにスクロール();

            // 2.ボタン待ち
            game.wIn任意ボタン入力待ち();

            // 3.メッセージボックスの最後の行が見えなくなるまでスクロール
            RefreshPage・最後の行が見えなくなるまでスクロール();

            // 4.その後、メッセージボックスを先頭に移動したら、必要に応じて過去のテキストを消す。
            if (_isClearPastText・改ページ後に過去のテキストを消すか == true)
            {
                clear・消去＿メッセージボックス内のテキストを初期化();
            }

            return _isSucces;
        }
        #endregion
        #region 以下、草案メモ
        ///// <summary>
        ///// 画面のテキストボックスに引数の文字列を表示します．このメソッドが実行されてから，（非同期に）指定されたスピードで1文字ずつ表示し，全ての文字列を表示し終えたら1を返します．
        ///// </summary>
        ///// <returns></returns>
        //private int _showTextB・メッセージを少しずつ表示(string _string)
        //{
        //    int _shownMessageLength = 0;
        //    FpsTimer _fpsTimer = new FpsTimer();
        //    // 改ページコードが含まれていたら，メインメッセージをリセット
        //    string s_新規ページコード = "【p】";
        //    if (_string.IndexOf(s_新規ページコード) == 0)
        //    {
        //        p_messageBox.p_richTextBox.Text = "";
        //        _string = _string.Replace(s_新規ページコード, "");
        //    }

        //    // (A)テキストボックスに表示
        //    if (p_messageBox.p_richTextBox.Visible == false)
        //    {
        //        p_messageBox.p_richTextBox.Visible = true;
        //    }
        //    while (_shownMessageLength < _string.Length)
        //    {
        //        // TODO: これより，描画処理は描画ルーチン（一定時間ごとに描画するタスク）登録をまとめて，一気に描画する処理を書いた方がいい

        //        // フレームされてたら？
        //        if (_fpsTimer.ToBeRendered)
        //        {
        //            // メッセージを1文字表示
        //            if (p_isShowText_ToString・表示する文字列はコピペ可能か == true)
        //            {
        //                _printText_ToTextBox(_string.Substring(_shownMessageLength, 1));
        //            }
        //            else
        //            {
        //                _printText_ToImage(_string.Substring(_shownMessageLength, 1));
        //            }
        //            // (B)（デバッグ用に）コンソールに表示
        //            game.DEBUGデバッグ一行出力(ELogType.l1_メインメッセージ, _string.Substring(_shownMessageLength, 1));
        //            _shownMessageLength++;
        //            _fpsTimer.WaitFrame(); // フレーム終了まで待つ
        //        }
        //    }
        //    return 1;
        //}
        #endregion


        #region ●●●選択肢を表示するメソッド
        /// <summary>
        /// 画面の選択ボックスに指定した選択肢例を表示します．画面表示に成功した場合はtrue，何らかの理由で画面に表示できなかった場合はfalseを返します．
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public CAnswer・回答 selectSelection・選択肢を出し選択するまで待機(EChoiceSample・選択肢例 _selectionStyle, List<int> _defaultSelectedNo, int _limitedTime_MSec)
        {
            List<string> _選択肢;
            // (A)特殊な選択肢の時は，ここで選択肢を作成
            if (_selectionStyle == EChoiceSample・選択肢例.charaAllゲストキャラ一覧)
            {
                game.mメッセージ_自動送り("（ゲストキャラデータベースを読み込み中…）", ESPeed.s09_超早い＿標準で１００ミリ秒);
                _選択肢 = CCharaCreator・キャラ生成機.getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c02_ゲストキャラ, true, true, CCharaCreator・キャラ生成機.p_iroParaSumIndex・基本色6総合値の列, CCharaCreator・キャラ生成機.p_iroParaSumIndex・応用色6総合値の列);
            }
            else if (_selectionStyle == EChoiceSample・選択肢例.charaAllプレイヤーキャラ一覧)
            {
                game.mメッセージ_自動送り("（プレイヤーキャラデータベースを読み込み中…）", ESPeed.s09_超早い＿標準で１００ミリ秒);
                _選択肢 = CCharaCreator・キャラ生成機.getCharaNameList・キャラ名リストを取得(ECharaType・キャラの種類.c01_プレイヤーキャラ, false, true, CCharaCreator・キャラ生成機.p_iroParaSumIndex・基本色6総合値の列, CCharaCreator・キャラ生成機.p_iroParaSumIndex・応用色6総合値の列);
            }
            else
            {
                // (B)選択肢例とデフォルト回答番号から，選択肢の回答文字列を抽出
                _選択肢 = MyTools.getEnumKeyName_Words_NotIncludingFirstIndex(_selectionStyle);
            }

            /*string[] _selections = _選択肢リスト.ToString().Split("＿".ToCharArray());
            // 識別ID以外の項目を選択肢リストに入れる
            List<string> _selectionList = new List<string>();
            int _index = 0;
            foreach (string _item_includingWaitWord in _selections)
            {
                if (_index != 0)
                {
                    _selectionList.Add(_item_includingWaitWord);
                }
            }*/
            return selectSelection・選択肢を出し選択するまで待機(_選択肢, _defaultSelectedNo, _limitedTime_MSec);
        }
        /// <summary>
        /// ●画面の選択ボックスに指定した選択肢例を表示します．画面表示に成功した場合はtrue，何らかの理由で画面に表示できなかった場合はfalseを返します．
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public CAnswer・回答 selectSelection・選択肢を出し選択するまで待機(List<string> _selectionString, List<int> _defaultSelectedNo, int _limitedTime_MSec)
        {
            /*List<string> _デフォルト選択肢 = new List<string>();
            foreach (int _no in _defaultSelectedNo)
            {
                _デフォルト選択肢.Add(MyTools.getListValue(_selectionString, _no - 1));
            }*/
            CAnswer・回答 _selectedResult;// = new CAnswer・回答(_デフォルト選択肢, _defaultSelectedNo);

            // 画面に表示して待つ
            canSelect・選択肢を表示可能に(_selectionString, _defaultSelectedNo); // [TODO]ここに前回の入力結果インスタンスを入れたら、過去の選択肢も保存できるかもね
            _selectedResult = waitForSelect・選択確定まで待機(_limitedTime_MSec);

            // 選択結果を取得
            _selectedResult.set確定(true);
            hideSelect・選択ボックスを非表示();

            return _selectedResult;
        }
            #region 選択肢の表示・操作に関するメソッド
        /// <summary>
        /// 選択肢を入力可能な状態にします．
        /// </summary>
        /// <returns></returns>
        private bool canSelect・選択肢を表示可能に(List<string> _selections, List<int> _defaultSelectedNo)
        {
            // 選択ボックスを表示
            showSelect・選択肢を表示(_selections);
            // デフォルト選択肢を選択状態にする
            foreach (int _selectedNo in _defaultSelectedNo)
            {
                selectItems・特定の項目を選択状態に(_selectedNo);
            }
            return true;
        }
        /// <summary>
        /// 選択完了まで待つメソッドです。
        ///      ※ユーザの入力を待つ処理も、
        ///          (i)ボタン入力待ち（game.wIb指定ボタン入力待ち(指定ボタン)など）で済む場合、と
        ///          (ii)画面入力待ちなので、単なるボタン入力待ちで済まない場合
        ///      がある。
        ///      例えば、選択肢の選択完了待ちは、(ii)の方に該当する。
        ///      選択完了待ちには、画面の操作コントロールと入力ボタンをイベントに、
        ///          (1)操作コントロール　    →   が選択ボックスで、
        ///          (2)入力ボタン            →   が決定ボタン（選択肢の確定）か戻るボタン（選択肢のキャンセルを示す）である
        ///      などを確認しなければならない。
        ///      この確認作業は、_checkNextEvent_ByFormControl_UserInput・ユーザの入力イベント処理()メソッドでやっている
        ///      ユーザが選択完了したら、上記メソッド内で game.p_isEndUserInput_GoNextOrBack・入力待ち完了フラグ = true になっている。
        ///      ユーザが選択をキャンセルしたら、上記メソッド内で p_isBackNext・前に戻る入力フラグ = trueになっている。 
        ///      なお、上記メソッドは これらのフラグをfalseにはしないので、
        ///      フラグの初期化は呼び出し元がやる必要がある。 
        /// </summary>
        /// <param name="_limitedTime_MSec_endless0"></param>
        /// <returns></returns>
        private CAnswer・回答 waitForSelect・選択確定まで待機(int _limitedTime_MSec_endless0)
        {
            // とりあえず、デフォルトの回答を取得
            CAnswer・回答 _selectedResult = new CAnswer・回答();

            // フラグを初期化
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
            int _spendTime = 0;
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Start();
            // 現在は、戻るボタン（キャンセル）が押されても、選択肢は完了という仕様。（_selectedResult.is戻る()が入る）
            while (game.isEndUserInput・ユーザの入力が完了したか() == false
                && (_limitedTime_MSec_endless0 == 0 ||  _spendTime <= _limitedTime_MSec_endless0))
            {
                // 随時考え中の入力結果を変更
                _selectedResult = getNowSelectedResult・現在の回答結果を取得();

                // 待つ
                game.waitウェイト(CGameManager・ゲーム管理者.s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒);
                _spendTime = (int)_stopwatch.ElapsedMilliseconds;
            }
            if (game.p_isUserInput_Back・前に戻る入力フラグ == true)
            {
                // キャンセルを格納
                _selectedResult = getChancelAnswer・キャンセルを意味する回答結果を取得();
            }
            // フラグを初期化
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);
            return _selectedResult;
        }
        private CAnswer・回答 getNowSelectedResult・現在の回答結果を取得()
        {
            ////メモリが勿体ないから変更時だけ変更？
            //string _現在の選択文字列 = p_selectBox.getResult・ただ一つ選択したリストの文字列を取得();
            //int _現在の選択番号 = p_selectBox.getResult・ただ一つ選択したリストの回答番号を取得();
            List<string> _選択文字列群 = new List<string>();
            List<int> _選択番号群 = new List<int>();
            // 今は選択肢はただ一つだけの回答
            _選択文字列群.Add(p_selectBox.getResult・ただ一つ選択したリストの文字列を取得());
            _選択番号群.Add(p_selectBox.getResult・ただ一つ選択したリストの回答番号を取得());
            CAnswer・回答 _結果 = new CAnswer・回答(_選択文字列群, _選択番号群);
            return _結果;
        }
        private CAnswer・回答 getChancelAnswer・キャンセルを意味する回答結果を取得()
        {
            return new CAnswer・回答(EAnswerSample・回答例.cancel);
        }
        /// <summary>
        /// 選択ボックスに選択肢を表示してフォーカスを当てます．選択リストの数を計算して返します．
        /// </summary>
        /// <param name="_デフォルト入力値"></param>
        private int showSelect・選択肢を表示(List<string> _選択肢一覧)
        {
            int _selectBoxNum = _選択肢一覧.Count;

            p_selectBox.p_listBox.Items.Clear();
            foreach (string _item in _選択肢一覧)
            {
                p_selectBox.p_listBox.Items.Add(_item); // 改行はいらなかった
            }
            //_listbox1.SetBounds(500, 100, _listbox1.Width, _listbox1.Height);
            p_selectBox.p_listBox.Visible = true; // 確認のため。あとでShow()でtrueになるので意味は無い
            p_selectBox.p_listBox.ClearSelected();
            p_selectBox.p_listBox.Select();
            p_selectBox.p_listBox.Show();
            p_selectBox.p_listBox.Focus();

            p_selectBoxNum・選択ボックス数 = _selectBoxNum;
            return _selectBoxNum;
        }
        /// <summary>
        /// 選択リストの特定の項目（1番目～N番目）を選択状態にします．前に選択状態だった項目の選択番号を返します．
        /// </summary>
        /// <param name="_selectedNo_1toN・選択項目番号"></param>
        /// <returns></returns>
        private int selectItems・特定の項目を選択状態に(int _selectedNo_1toN・選択項目番号){
            int _beforeSelectedNo = p_selectBox.p_listBox.SelectedIndex;
            if (_selectedNo_1toN・選択項目番号 > 0)
            {
                if (_selectedNo_1toN・選択項目番号 <= p_selectBox.p_listBox.Items.Count)
                {
                    p_selectBox.p_listBox.SelectedIndex = (_selectedNo_1toN・選択項目番号 - 1);
                }
            }
            return _beforeSelectedNo;
        }
            #endregion
        #endregion

        #region ●●●入力ボックスを表示するメソッド
        /// <summary>
        /// 画面の入力ボックス1つに指定したデフォルト文字列を表示し，ユーザに入力を促します．画面表示に成功した場合はtrue，何らかの理由で画面に表示できなかった場合はfalseを返します．
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public CAnswer・回答 showInput・入力画面表示(int _limitedTime_MSec, string _入力ラベル名, string _デフォルト入力値)
        {
            CAnswer・回答 _inputedAnswer;

            // 入力ボックスを表示
            showInput・入力欄を表示(_入力ラベル名, _デフォルト入力値);
            _inputedAnswer = waitForInput・入力確定まで待機(_limitedTime_MSec);

            _inputedAnswer.set確定(true);
            return _inputedAnswer;
        }
        /// <summary>
        /// 画面の複数に入力ボックスに指定したデフォルト文字列を表示し，ユーザに入力を促します．画面表示に成功した場合はtrue，何らかの理由で画面に表示できなかった場合はfalseを返します．
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public CAnswer・回答 showInput・複数欄入力画面表示(int _limitedTime_MSec, params string[] _入力欄毎のラベル名とデフォルト値_それぞれ2個ずつ列挙)
        {
            string[] _params = _入力欄毎のラベル名とデフォルト値_それぞれ2個ずつ列挙;
            //List<string> _inputedDefault = new List<string>();
            //for (int i = 1; i <= _params.Length - 1; i += 2)
            //{
            //    _inputedDefault.Add(_params[i]);
            //}
            CAnswer・回答 _inputedAnswer;
            // 入力ボックスを表示
            showInput・入力欄を表示(_params);
            // 入力完了まで待つ
            _inputedAnswer = waitForInput・入力確定まで待機(_limitedTime_MSec);

            // 入力結果を格納
            _inputedAnswer.set確定(true);
            hideInput・入力ボックスを非表示();

            return _inputedAnswer;
        }
        private CAnswer・回答 waitForInput・入力確定まで待機(int _limitedTime_MSec)
        {
            CAnswer・回答 _inputedAnswer = getNowUserInput・入力回答を取得();
            bool _isNoTime = false;
            if (_limitedTime_MSec == 0)
            {
                _isNoTime = true;
            }

            int _spendTime = 0;
            // フラグを初期化
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(true);
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Start();
            while (game.isEndUserInput・ユーザの入力が完了したか() == false)
            //while (game.getP_keyboardInput().IsPull(Yanesdk.Input.KeyCode.ENTER)==false)
            {
                // 随時考え中の入力結果を変更
                _inputedAnswer = getNowUserInput・入力回答を取得();

                // 待つ
                game.waitウェイト(CGameManager・ゲーム管理者.s_waitMSecForUserInput・ユーザ入力の回答待ち時間単位ミリ秒);
                _spendTime = (int)_stopwatch.ElapsedMilliseconds;
                if (_isNoTime == false)
                {
                    if (_spendTime > _limitedTime_MSec)
                    {
                        break;
                    }
                }
            }
            if (game.p_isUserInput_Back・前に戻る入力フラグ == true)
            {
                // キャンセルを格納
                _inputedAnswer = getChancelAnswer・キャンセルを意味する回答結果を取得();
            }
            // 入力フラグを初期化
            game.WaitUserInput_ClearFlagユーザの入力待ちフラグ初期化(false);
            return _inputedAnswer;
        }
        public CAnswer・回答 getNowUserInput・入力回答を取得()
        {
            string _現在の入力文字列;
            List<string> _回答文字列群 = new List<string>();
            List<int> _回答番号群 = new List<int>();
            for (int i = 0; i <= p_inputBoxNum・入力ボックス数 - 1; i++)
            {
                _現在の入力文字列 = p_inputGroupBox.getResult(i);
                _回答文字列群.Add(_現在の入力文字列);
                if (_現在の入力文字列 == "")
                {
                    _回答番号群.Add(0); // 入力ボックスは，入力したら1，未入力なら0
                }
                else
                {
                    _回答番号群.Add(1);
                }
            }
            CAnswer・回答 _結果 = new CAnswer・回答(_回答文字列群, _回答番号群);
            //Program.printlnLog(ELogType.l4_重要なデバッグ, "入力文字列{ "+MyTools.getListValues_ToCSVLine(null, _回答文字列群, true) + "}, 回答番号{" + MyTools.getListValues_ToCSVLine(null, _回答番号群, true) + "}");
            return _結果;
        }
        /// <summary>
        /// 入力ボックスに入力デフォルト値を表示してフォーカスを当てます．回答数（入力ボックスの数）を計算して返します．
        /// </summary>
        /// <param name="_デフォルト入力値"></param>
        private int showInput・入力欄を表示(params string[] _入力欄毎のラベル名とデフォルト値_それぞれ2個ずつ列挙)
        {
            string[] _param = _入力欄毎のラベル名とデフォルト値_それぞれ2個ずつ列挙;
            int _showInputBoxNum = _param.Length / 2;

            for (int i = 0; i <= _showInputBoxNum - 1; i++)
            {
                p_inputGroupBox.setLabel(i, MyTools.getArrayValue(_param, 2 * i));
                p_inputGroupBox.setInputDefault(i, MyTools.getArrayValue(_param, 2 * i + 1));
            }
            p_inputGroupBox.show・表示(_showInputBoxNum);
            p_inputGroupBox.setFocus(0);
            p_inputBoxNum・入力ボックス数 = _showInputBoxNum;
            return _showInputBoxNum;
        }
        #endregion

        #region ●●●メインメッセージボックスの情報を取得するメソッド
        /// <summary>
        /// 今のメインメッセージボックスのテキストを返します．
        /// </summary>
        /// <returns></returns>
        public string getNowMessage・現在のメッセージを取得()
        {
            return p_messageBox.p_richTextBox.Text;
        }
        #endregion

    }
}
