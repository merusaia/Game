using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PublicDomain
{
    public partial class FSoundConfig : Form
    {   
        // サウンドデータのリストを作成
        List<string> p_MusicNameList;
        List<string> p_SENameList;
        /// <summary>
        /// 再生可能なファイル名（「データベース」ディレクトリ下の全てのオーディオファイル名）のリストを作成
        /// </summary>
        List<string> p_audioFileList_NotFullPath;

        public CGameManager・ゲーム管理者 game;
        public FSoundConfig(CGameManager・ゲーム管理者 _game)
        {
            // コントロールの初期化
            InitializeComponent();

            game = _game;

            // 現在のボリュームを反映
            numericUpDown1ＢＧＭ.Value = game.pBGM_getVolume() / 10;
            numericUpDown2効果音.Value = game.pSE_getVolume() / 10;
            numericUpDown3ボイス.Value = 50;

            // コントロールの更新
            updateAudioDataControls・オーディオデータを扱うコントロールの更新();
        }
        private void updateAudioDataControls・オーディオデータを扱うコントロールの更新()
        {
            // ゲーム中で定義されている（EMusic、ESEの）オーディオ名リストを作成
            p_MusicNameList = game.getBGMNameList・ＢＧＭの抽象名リストを取得();
            p_SENameList = game.getSENameList・効果音の抽象名リストを取得();

            // コンボボックスを反映
            comboBox1ＢＧＭ名.Items.Clear();
            comboBox1ＢＧＭ名.Items.AddRange(p_MusicNameList.ToArray());
            comboBox3効果音名.Items.Clear();
            comboBox3効果音名.Items.AddRange(p_SENameList.ToArray());

            // 再生可能なファイル名（「データベース」ディレクトリ下の全てのオーディオファイル名）のリストを作成
            p_audioFileList_NotFullPath = game.getAudioFileList・ゲーム中で再生可能なオーディオファイルリストを取得(false, true, true, false);
            // コンボボックスを反映
            comboBox2ＢＧＭファイル名.Items.AddRange(
                game.getAudioFileList・ゲーム中で再生可能なオーディオファイルリストを取得(false, true, false, false).ToArray());
            comboBox4効果音ファイル名.Items.AddRange(
                game.getAudioFileList・ゲーム中で再生可能なオーディオファイルリストを取得(false, false, true, false).ToArray());
        }

        /// <summary>
        /// falseだと必要最小限の機能（音量だけ）を表示しています。trueだとフォームのサイズが大きくなっています。
        /// </summary>
        public bool p_isExtraOptionShow・拡張オプションを表示しているか = true;
        private void button1拡張オプション表示切り替え_Click(object sender, EventArgs e)
        {
            if (p_isExtraOptionShow・拡張オプションを表示しているか == true)
            {
                p_isExtraOptionShow・拡張オプションを表示しているか = false;
                this.SetBounds(this.Left, this.Top, 219, 170);
                button1拡張オプション表示切り替え.Text = "拡張オプションの表示";
            }
            else
            {
                p_isExtraOptionShow・拡張オプションを表示しているか = true;
                this.SetBounds(this.Left, this.Top, 533, 340);
                button1拡張オプション表示切り替え.Text = "拡張オプションの非表示";
            }
        }

        private void button2ＢＧＭテスト_Click(object sender, EventArgs e)
        {
            game.pBGM(game.getRandomBGM_FullPath・曲ファイル名をランダム取得(false));
            //game.pBGM(EBGM・曲.battleBoss01・ボス戦＿破壊神);
        }

        private void button2効果音テスト_Click(object sender, EventArgs e)
        {
            game.pSE(game.getRandomSE_FullPath・効果音ファイル名をランダム取得(false));
            //game.pSE(ESE・効果音.attack03a・会心の一撃_シュンシュンシュンッ);
        }

        private void button3ボイステスト_Click(object sender, EventArgs e)
        {
            // 未実装
        }

        private void numericUpDown1ＢＧＭ_ValueChanged(object sender, EventArgs e)
        {
            game.pBGM_setVolume(((int)numericUpDown1ＢＧＭ.Value * 10));
        }

        private void numericUpDown2効果音_ValueChanged(object sender, EventArgs e)
        {
            game.pSE_setVolume(((int)numericUpDown2効果音.Value * 10));
        }

        private void numericUpDown3ボイス_ValueChanged(object sender, EventArgs e)
        {
            // 未実装
        }



        /// <summary>
        /// ロード時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void FSoundConfig_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 終了時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void FSoundConfig_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose(true);
        }

        private void comboBox1ＢＧＭ名_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _audioDataName = comboBox1ＢＧＭ名.SelectedItem.ToString();
            EBGM・曲 _eMusic = MyTools.getEnumItem_FromString<EBGM・曲>(_audioDataName);
            if (_eMusic == EBGM・曲.__none・無し)
            {
                // 無しの場合は、ファイル名もなし
                comboBox2ＢＧＭファイル名.SelectedIndex = -1;
            }
            else
            {
                string _fileName_FullPath = game.getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_eMusic);
                string _name = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_fileName_FullPath);
                // ラベルに登録されていた、実際に再生するファイル名を更新
                int _selectedIndex = comboBox2ＢＧＭファイル名.Items.IndexOf(_name);
                comboBox2ＢＧＭファイル名.SelectedIndex = _selectedIndex;
                // ■実際に再生
                game.pBGM(_fileName_FullPath);
            }
        }
        private void comboBox2ＢＧＭファイル名_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ■選択したら実際に再生
            if (comboBox2ＢＧＭファイル名.SelectedItem != null)
            {
                string _fileName_NotFullPath = comboBox2ＢＧＭファイル名.SelectedItem.ToString();
                string _fileName_FullPath = game.getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_fileName_NotFullPath);
                game.pBGM(_fileName_FullPath);
            }
        }


        private void comboBox3効果音リスト_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _audioDataName = comboBox3効果音名.SelectedItem.ToString();
            ESE・効果音 _eSE = MyTools.getEnumItem_FromString<ESE・効果音>(_audioDataName);
            if (_eSE == ESE・効果音.__none・無し)
            {
                // 無しの場合は、ファイル名もなし
                comboBox4効果音ファイル名.SelectedIndex = -1;
            }
            else
            {
                string _fileName_FullPath = game.getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_eSE);
                string _name = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_fileName_FullPath);
                // ラベルに登録されていた、実際に再生するファイル名を更新
                int _selectedIndex = comboBox4効果音ファイル名.Items.IndexOf(_name);
                comboBox4効果音ファイル名.SelectedIndex = _selectedIndex;
                // ■実際に再生
                game.pSE(_fileName_FullPath);
            }
        }
        private void comboBox4効果音ファイル名_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4効果音ファイル名.SelectedItem != null)
            {
                // ■選択したら実際に再生
                string _fileName_NotFullPath = comboBox4効果音ファイル名.SelectedItem.ToString();
                string _fileName_FullPath = game.getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_fileName_NotFullPath);
                game.pSE(_fileName_FullPath);
            }
        }



        private void button5ＢＧＭエディット確定_Click(object sender, EventArgs e)
        {
            string _updateName = comboBox1ＢＧＭ名.SelectedItem.ToString();
            //EBGM・曲 _updateItem = MyTools.getEnumItem_FromString<EBGM・曲>(_updateName);

            string _newFileName_NotFullPath = comboBox2ＢＧＭファイル名.SelectedItem.ToString();
            //EBGM・曲 _newItem = MyTools.getEnumItem_FromString<EBGM・曲>(_newFileName_NotFullPath);
            //string _newFileName_FullPath = game.getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_newItem);
            // 新しいオーディオデータを作成
            CSoundConstructAdaptor・オーディオデータ定義クラス _newData = new CSoundConstructAdaptor・オーディオデータ定義クラス(
            CSoundConstructAdaptor・オーディオデータ定義クラス.s_idAuto・サウンドID自動割り当て番号,
            _updateName, _newFileName_NotFullPath, 0, "");
            CSoundPlayData・オーディオ再生用クラス _newSound = new CSoundPlayData・オーディオ再生用クラス(-1);
            _newSound.constructInfo・オブジェクト再構築データ = _newData;

            //_newData.p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある = _newFileName_FullPath;
            //_newData.p_fileName・ファイル名 = _newFileName_NotFullPath;
            //_newData.p_AudioDataName・参照名 = _newFileName_NotFullPath;
            //_newData.p_id・サウンドID = 0;
            //_newData.p_channelNo・再生チャンネル番号 = 0;


            // 古いオーディオデータを更新
            game.getP_Sound・サウンド管理者().setAudioData・該当参照名のオーディオデータを更新(_updateName, _newSound);
            // コントロールの更新
            updateAudioDataControls・オーディオデータを扱うコントロールの更新();
            // ファイルに書き出し
            updateSoundDatabaseFile・サウンドデータベースファイルの更新(_updateName, _newFileName_NotFullPath);
        }

        private void button6効果音エディット確定_Click(object sender, EventArgs e)
        {
            string _updateName = comboBox3効果音名.SelectedItem.ToString();
            //ESE・効果音 _updateItem = MyTools.getEnumItem_FromString<ESE・効果音>(_updateName);

            string _newFileName_NotFullPath = comboBox4効果音ファイル名.SelectedItem.ToString();
            //ESE・効果音 _newItem = MyTools.getEnumItem_FromString<ESE・効果音>(_newFileName_NotFullPath);
            //string _newFileName_FullPath = game.getP_Sound・サウンド管理者().getFileName_FullPath・ファイル名を取得(_newItem);
            // 新しいオーディオデータを作成
            CSoundConstructAdaptor・オーディオデータ定義クラス _newData = new CSoundConstructAdaptor・オーディオデータ定義クラス(
                CSoundConstructAdaptor・オーディオデータ定義クラス.s_idAuto・サウンドID自動割り当て番号,
                _updateName, _newFileName_NotFullPath, 0, "");
            CSoundPlayData・オーディオ再生用クラス _newSound = new CSoundPlayData・オーディオ再生用クラス(0);
            _newSound.constructInfo・オブジェクト再構築データ = _newData;

            // 古いオーディオデータを更新
            game.getP_Sound・サウンド管理者().setAudioData・該当参照名のオーディオデータを更新(_updateName, _newSound);
            //_newData.p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある = _newFileName_FullPath;
            //_newData.p_fileName・ファイル名 = _newFileName_NotFullPath;
            //_newData.p_AudioDataName・参照名 = _newFileName_NotFullPath;
            //_newData.p_id・サウンドID = 0;
            //_newData.p_channelNo・再生チャンネル番号 = 0;

            // ファイルに書き出し
            updateSoundDatabaseFile・サウンドデータベースファイルの更新(_updateName, _newFileName_NotFullPath);
            // コントロールの更新
            updateAudioDataControls・オーディオデータを扱うコントロールの更新();
            // 変更の効果音
            game.pSE(ESE・効果音._system03・確定音_ピロリーンッ);
        }
        /// <summary>
        /// カスタムサウンドの変更をサウンドデータベースファイルに書き込み（csvファイル更新）
        /// </summary>
        /// <param name="_updateName"></param>
        /// <param name="_newFileName_NotFullPath"></param>
        private void updateSoundDatabaseFile・サウンドデータベースファイルの更新(string _updateName, string _newFileName_NotFullPath)
        {
            // ファイル名を取得
            string _soundDatabaseFileName_FullPath = Program・実行ファイル管理者.p_SoundDatabaseFileName_FullPath・サウンドデータベースファイルパス;
            string _AllDataText = MyTools.ReadFile(_soundDatabaseFileName_FullPath);
            // 探す文字列は、",古い参照名,"
            string _searchWord = "," + _updateName + ",";
            // ",参照名,"が最初に含まれる開始インデックスを検索
            int _wordIndex = _AllDataText.IndexOf(_searchWord);
            // ",参照名,"の次のインデックス（最後の文字より一つ右）を取得
            int _startIndex = _wordIndex + _searchWord.Length;
            // ",参照名,"の次のインデックス（最後の文字より一つ右）から数えて、次の「,」を検索
            int _endIndex = _AllDataText.IndexOf(",", _startIndex);
            // ",参照名,"の次のインデックス　～　次の「,」より一つ左　までの単語（置換前文字列）を取得
            string _oldFileName_NotFullPath = _AllDataText.Substring(_startIndex, _endIndex - _startIndex);
            // （ファイル名の更新）置換前文字列を、置換後文字列に置換
            _AllDataText = _AllDataText.Replace(_oldFileName_NotFullPath, _newFileName_NotFullPath);
            // （再生チャンネルは更新しません）
            // ファイルを上書き
            MyTools.WriteFile(_soundDatabaseFileName_FullPath, _AllDataText);

            // 変更の効果音
            game.pSE(ESE・効果音._system03・確定音_ピロリーンッ);

        }

        private void button8データベースフォルダを開く_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(MyTools.getCheckedFilePath(Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス));
        }

        private void button7外部ファイルをＢＧＭとして追加_Click(object sender, EventArgs e)
        {
            OpenFileDialog _dialog = null;
            try
            {
                // 「OK」以外をクリックしたらnullが入る
                _dialog = MyTools.openFileDialog("外部のオーディオファイルを追加", "", true);
            }
            catch (Exception _e)
            {
                // 例外が発生したら、とりあえずダイアログを閉じる
            }
            // 選択したファイルをコピーして追加
            if (_dialog != null)
            {
                foreach (string _fileName in _dialog.FileNames)
                {
                    MyTools.copyFile(_fileName, 
                        Program・実行ファイル管理者.p_BGMDirectory_FullPath・曲フォルダパス
                        + MyTools.getFileName_NotFullPath_LastFileOrDirectory(_fileName));
                }
                // コントロールの更新
                updateAudioDataControls・オーディオデータを扱うコントロールの更新();
            }
        }

        private void button8外部ファイルを効果音として追加_Click(object sender, EventArgs e)
        {
            OpenFileDialog _dialog = null;
            try
            {
                // 「OK」以外をクリックしたらnullが入る
                _dialog = MyTools.openFileDialog("外部のオーディオファイルを追加", "", true);
            }
            catch (Exception _e)
            {
                // 例外が発生したら、とりあえずダイアログを閉じる
            }
            // 選択したファイルをコピーして追加
            if (_dialog != null)
            {
                foreach (string _fileName in _dialog.FileNames)
                {
                    MyTools.copyFile(_fileName, 
                        Program・実行ファイル管理者.p_SEDirectory_FullPath・効果音ファルダパス
                        + MyTools.getFileName_NotFullPath_LastFileOrDirectory(_fileName));
                }
                // コントロールの更新
                updateAudioDataControls・オーディオデータを扱うコントロールの更新();
            }

        }

    }
}
