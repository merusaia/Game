// デバッグ時はコメントアウト
//#define Debug
// エラーデバッグ時のテスト時はコメントアウト
//#define Test

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using System.Runtime.InteropServices; // 以下の様な、MCIを定義する時に使う[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]

// ※VisualBasic>NETの機能をＣ＃で使いたい時。使用するにはプロジェクトにMicrosoft.VisualBasicの参照の追加が必要です。プロジェクトを右クリック→「参照の追加」などで追加。
using Microsoft.VisualBasic;

// ※MP3ファイルのアーティスト情報などを取得するためのライブラリ（既定ではC:\WINDOWS\System32に格納されている）参照の追加が必要。http://www.atmarkit.co.jp/fdotnet/dotnettips/591mp3tags/mp3tags.html
using Shell32;

using PublicDomain;

// namespace merusaia.NakatsukaTetsuhiro.Experiment
namespace PublicDomain
{
    /// <summary>
    /// サウンド（オーディオ）関係で、ちょっとした時に使いたい便利な機能を実装したメソッドを持つクラスです．
    /// 
    /// ※Windows依存です。
    /// 
    /// </summary>
    public class MySound_Windows
    {
        /// <summary>
        /// playSound()でサウンド再生・録音にMCI（MediaControlInterface）を使用するかどうか。falseにするとMCIを使用しないので、playSound()中にMCI関連エラーが原因で音が鳴らないことはなくなります。
        /// </summary>
        public static bool p_isUseMCI = true;
        /// <summary>
        /// playSound()でサウンド再生・録音にWMP（WindowsMediaPlayer）を使用するかどうか。falseにするとWMPを使用しないので、playSound()中にWMP関連エラーが原因で音が鳴らないことはなくなります。
        /// </summary>
        public static bool p_isUseWMP = false;


        
        // 【使い方の例】__HELPメソッド
        /// <summary>
        /// このクラスの使い方を示したメソッドです。詳しくは中身をみてみてください。
        /// </summary>
        public static void __HELP_ThisClassusingExamples・このクラスの使い方やテストコード()
        {
            string _directory = @"C:\MerusaiaDocuments\Documents\Visual Studio 2010\new_PertnerOfMind_2010\minRPG\bin\Debug\データベース";
            string _mp3_SE1 = "_sample.mp3";
            string _mp3_SE2 = "ぴん＿短めで綺麗でぽんとした選択音.mp3";
            string _wav_SE1 = "倒1.wav";
            string _wav_SE2 = "決定.wav";
            string _mp3_BGM1 = "Boss_Battle.mp3";
            string _mp3_BGM2 = "GSR.mp3";
            string _mp3_BGM3 = "が～ん！もう終わりかよ．．．.mp3";
            string _wma_BGM4 = "OAM's Blues.wma";
            string _wma_BGM5 = "Love Comes.wma";
            string _mid_BGM6 = "Girls sword Rock7_GM音源に直さずsw6か8の形式をそのままMIDI形式に変換したファイル_ＰＣ環境によっては何も聞こえないかも.mid";

            #region 事前テスト

            bool _isTestBaseWMPPlay・WMPで音が鳴るかどうかだけのテスト = true;
            if (_isTestBaseWMPPlay・WMPで音が鳴るかどうかだけのテスト == true)
            {
                if (p_isUseWMP == true)
                {
                    if (MySound_Windows.isExist(_directory) == false)
                        ConsoleWriteLine("以下のディレクトリは存在しないか、アクセス不正です。_directory に代入したフルパスが間違っていないか、確認してください。_directory = " + _directory);
                    string _file = _directory + "\\" + _mp3_BGM1;
                    WMPLib.WindowsMediaPlayer _wmp1 = new WMPLib.WindowsMediaPlayer();
                    _wmp1.settings.volume = 1000; // ボリューム設定
                    _wmp1.URL = _file;
                    _wmp1.controls.play();
                    ConsoleWriteLine("WMP（WindowsMediaPlayer）で音、なってます？（５秒間停止中）" +
                        "\n鳴って無かったら、WMP_系のメソッドは使いものならないですね。" +
                        "\n鳴らなかったら、p_isUseWMP = false にしとくと意図しないエラーが減りますよ。只今の値:" + p_isUseWMP + "\n\n");
                    Thread.Sleep(5000);
                    _wmp1.settings.volume = 400; // ボリューム設定
                    Thread.Sleep(1000);
                    _wmp1.settings.volume = 300; // ボリューム設定
                    Thread.Sleep(1000);
                    _wmp1.settings.volume = 200; // ボリューム設定
                    Thread.Sleep(1000);
                    _wmp1.settings.volume = 100; // ボリューム設定
                    Thread.Sleep(1000);
                    _wmp1.settings.volume = 1000; // ボリューム設定
                    MySound_Windows.WMP_stopSound();
                }
                else
                {
                    ConsoleWriteLine("WMP（WindowsMediaPlayer）は使用しない設定になっています（p_isUseWMP == false）。");
                }
            }
            bool _isTestBaseMCIPlay・MCIで音が鳴るかどうかだけのテスト = true;
            if (_isTestBaseMCIPlay・MCIで音が鳴るかどうかだけのテスト == true)
            {
                if (p_isUseMCI == true)
                {
                    string _file = _directory + "\\" + _wma_BGM4;
                    MCI_setVolume_BGM(1000);
                    MCI_playBGM(_file, false);
                    ConsoleWriteLine("MCI（MediaControlInterface）で音、なってます？（５秒間停止中）" +
                        "\n鳴って無かったら、MCI_系のメソッドは使いものならないですね。" +
                        "\n鳴らなかったら、p_isUseMCI = false にしとくと意図しないエラーが減りますよ。只今の値:" + p_isUseMCI + "\n\n");
                    Thread.Sleep(5000);
                    _file = _directory + "\\" + _mid_BGM6;
                    MCI_setVolume_BGM(1000);
                    MCI_playBGM(_file, false);
                    ConsoleWriteLine("MCI（MediaControlInterface）でmidを再生しました。音、なってます？（５秒間停止中）" +
                        "\n鳴らなかったら、そのＰＣ環境では鳴らないmid音源かもしれません。");
                    Thread.Sleep(5000);
                    MCI_setVolume_BGM(400); // ボリューム設定
                    Thread.Sleep(1000);
                    MCI_setVolume_BGM(300); // ボリューム設定
                    Thread.Sleep(1000);
                    MCI_setVolume_BGM(200); // ボリューム設定
                    Thread.Sleep(1000);
                    MCI_setVolume_BGM(100); // ボリューム設定
                    Thread.Sleep(1000);
                    MCI_setVolume_BGM(1000); // ボリューム設定
                    MCI_stopBGM();
                }
                else
                {
                    ConsoleWriteLine("MCI（MediaControlInferface）は使用しない設定になっています（p_isUseMCI == false）。");
                }
            }

            bool _isTestOnly2・複数同時再生テスト = true;
            if (_isTestOnly2・複数同時再生テスト == true)
            {
                // 2つ同時再生ができるかどうか
                MySound_Windows.playSound(_directory + "\\" + _wma_BGM4, false);
                Thread.Sleep(3000);
                MySound_Windows.playSound(_directory + "\\" + _wma_BGM5, false);

                ConsoleWriteLine("playSound()メソッドで、２つのＢＧＭを再生しようとしました。１個目は切れたかな？（５秒間停止中）\n\n");
                // 同じファイルを再生しようとしたらどうなるか
                //MySound_Windows.MCI_playBGM(_directory + "\\" + _mp3_BGM1, false);
                //Thread.Sleep(3000);
                //MySound_Windows.WMP_playSound(_directory + "\\" + _mp3_BGM1, false);

                Thread.Sleep(5000);
            }
            #endregion

            bool _isTestAll = true;
            if (_isTestAll == true)
            {
                // ボリュームを取得
                ConsoleWriteLine("現在のマスターボリュームの初期値: getVolume_Master = "+ MySound_Windows.getVolume_Master() + "\n");
                Thread.Sleep(3000);
                // ボリュームを変更
                MySound_Windows.setVolume_Master(500);
                ConsoleWriteLine("マスターボリュームはこうやって変更できます: setVolume_Master(" + MySound_Windows.getVolume_Master() + ")\n");
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(3000);

                #region メインテスト

                // BGMを鳴らしながら
                playSound(_directory + "\\" + _mp3_BGM2, true);
                // 一時停止
                ConsoleWriteLine("BGMを再生しました。　現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(1000);
                ConsoleWriteLine("ボリュームを徐々に増加(500 -> 1000)");
                // ボリュームを徐々に増加
                for (int i = 0; i < 250; i++)
                {
                    setVolume_Master(500 + i * 2);
                    Thread.Sleep(10);
                }
                // 効果音を鳴らす
                playSound(_directory + "\\" + _wav_SE1, false);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(1000);
                // 効果音を鳴らす
                playSound(_directory + "\\" + _wav_SE2, false);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(1000);
                // 全て停止
                stopSound();

                // 違うBGMを鳴らしながら
                playSound(_directory + "\\" + _mp3_BGM1, true);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(3000);
                // 効果音を鳴らす
                playSound(_directory + "\\" + _wav_SE1, false);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(100);
                // 効果音を鳴らす
                playSound(_directory + "\\" + _wav_SE2, false);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(10);
                // 効果音を鳴らす
                playSound(_directory + "\\" + _wav_SE2, false);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(10);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(5000);

                // 同じBGMを鳴らしながら
                playSound(_directory + "\\" + _mp3_BGM1, true);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(3000);
                // mp3の効果音を鳴らす
                playSound(_directory + "\\" + _mp3_SE1, false);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(1000);
                // 効果音を鳴らす
                playSound(_directory + "\\" + _mp3_SE2, false);
                // 一時停止
                Thread.Sleep(1000);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(3000);

                // 違うBGMを鳴らしながら
                playSound(_directory + "\\" + _mp3_BGM3, true);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(10);
                // ボリュームを徐々に増加
                ConsoleWriteLine("\nボリュームを徐々に増加(0 -> 1000)");
                for (int i = 0; i < 500; i++)
                {
                    setVolume_Master(i * 2);
                    Thread.Sleep(10);
                }
                // 効果音を鳴らす
                playSound(_directory + "\\" + _wav_SE2, false);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(1000);
                // 同じ効果音を鳴らす
                ConsoleWriteLine("同じ効果音を鳴らす");
                playSound(_directory + "\\" + _wav_SE2, false);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                Thread.Sleep(1000);
                // 一時停止
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                // ボリュームを徐々に減少
                ConsoleWriteLine("\nボリュームを徐々に減少(1000 -> 0)");
                for (int i = 0; i < 10; i++)
                {
                    setVolume_Master(1000 - i * 100);
                    Thread.Sleep(100);
                    // 絶えず効果音を鳴らす
                    playSound(_directory + "\\" + _wav_SE2, false);
                }
                #endregion

                // 全て停止
                // ボリュームを元に戻す
                ConsoleWriteLine("\nボリュームを最後にデフォルトに設定(500)");
                setVolume_Master(500);
                if (stopSound() == true)
                {
                    ConsoleWriteLine("停止する前に、最後に何か再生されてたみたいだよ。");
                }
                ConsoleWriteLine("現在のサウンド同時再生数（理論値）: " + getPlayingSoundFileNum());
                ConsoleWriteLine("おしまい。");
            }
            // 他にもたくさんあります。詳しくは↓のメソッド集を見てみてください。

        }

        // 出力処理
        /// <summary>
        /// デバッグ用のコンソール出力を表示するかどうかです。デフォルトではtrueです。falseにしたい場合はこの変数を直接変更してください。
        /// </summary>
        public static bool p_showMessage_isDebug = true;
        // エラーなどを出力するメソッド
        /// <summary>
        /// コンソールに文字列を表示します。
        /// エラーメッセージなどを一括して管理するためのメソッドです。このクラスの他のメソッドも使用しています。
        /// 様々な標準出力の処理内容を変更したい場合は、このメソッドの中身を変更すると、一括して変更で来て便利かもしれません。
        /// 
        /// ※例えば、１回Console.WriteLien()するだけでも数ミリ～十数ミリ秒の時間が無駄になるみたいなので、
        /// エラーメッセージの出力はなるべくテストだけにして、本番では表示しないようにするフラグ（p_isDebug_WriteConsole）が使えるよ。
        /// </summary>
        /// <param name="_message"></param>
        public static void ConsoleWriteLine(string _message)
        {
            if (p_showMessage_isDebug == true)
            {
                Console.WriteLine(_message);
            }
        }
        /// <summary>
        /// コンソールに文字列を表示します。
        /// エラーメッセージなどを一括して管理するため、このクラスの他のメソッドも使用しています。
        /// 様々な標準出力の処理内容を変更したい場合は、このメソッドの中身を変更してください。
        /// </summary>
        /// <param name="_message"></param>
        public static void printErrorMessage(string _message)
        {
            ConsoleWriteLine(_message);
        }



        // ■mp3ファイル関連
        #region mp3ファイルのタグの付け方: addTag / getTage　（まだ未実装）
        ///// <summary>
        ///// MP3ファイルに指定したタグが含まれている場合はtrue、含まれてない場合はfalseを返します。
        ///// </summary>
        //public static bool isTagAdded_MP3(string _mp3FileFullPath, string _tag)
        //{
        //    bool _isAdded = false;
        //    string _tagAll = getTag_MP3(_mp3FileFullPath);
        //    _isAdded = _tagAll.Contains(_tag);
        //    return _isAdded;
        //}
        ///// <summary>
        ///// MP3ファイルに指定した作者の名前（ユーザＩＤ）が含まれている場合はtrue、含まれてない場合はfalseを返します。
        ///// </summary>
        //public static bool isTagAuthor_MP3(string _mp3FileFullPath, string _TagAuthorName_UserID)
        //{
        //    bool _isAdded = false;
        //    string _tagAll = getTag_MP3(_mp3FileFullPath);
        //    _isAdded = _tagAll.Contains(_TagAuthorName_UserID);
        //    return _isAdded;
        //}
        ///// <summary>
        ///// MP3ファイルに指定したタグ名の値を取得します。存在しない場合は""を返します。
        ///// </summary>
        //public static string getTag_MP3(string _mp3FileFullPath, string _tagName)
        //{
        //    string _tagValue = "";
        //    string _tagAll = getTag_MP3(_mp3FileFullPath);
        //    if (_tagAll.Contains(_tagName) == true)
        //    {
        //        // タグ名を探す
        //        int _index = _tagAll.LastIndexOf(_tagName + ":"); // 一応、最新の（最後に追加された）値を優先
        //        if (_index == -1) _index = _tagAll.LastIndexOf(_tagName + "："); // 全角コロンにも対応
        //        if (_index == -1) return _tagValue; // なんでかわからないけど見つからない
        //        // 「タグ名:」や「タグ名：」から後の文字列から、終点を探す
        //        int _endIndex = _tagAll.Substring(_index).IndexOf(",");
        //        if (_endIndex == -1) _endIndex = _index + (_tagName.Length + 1) + 1;// コロンが無いなんておかしいが、エラーを防ぐため、「タグ名:」から1文字としておく
        //        _tagValue = _tagAll.Substring(_index, _endIndex - _index);
        //        // 「タグ名：値」から「値」だけを取得
        //        int _valueStartIndex = _tagValue.IndexOf(":");
        //        if (_valueStartIndex == -1) _valueStartIndex = _tagValue.IndexOf("：");
        //        if (_valueStartIndex == -1) return _tagValue; // なんかおかしいけどコロンみつからんから全部返しとけ
        //        _tagValue = _tagValue.Substring(_valueStartIndex);
        //    }
        //    return _tagValue;
        //}
        ///// <summary>
        ///// 指定したイメージに、作者のタグが含まれていれば取得します。含まれていなければ、""を返します。
        ///// </summary>
        //public static string getTagAuthor_MP3(string _mp3FileFullPath)
        //{
        //    return getTag_MP3(_mp3FileFullPath, "作者");
        //}
        ///// <summary>
        ///// MP3ファイルに付加されているタグ（addTag_MP3で付加した、mp3ファイルに埋め込まれている文字列）を全て取得します。存在しない場合は""を返します。
        ///// </summary>
        //public static string getTag_MP3(string _mp3FileFullPath)
        //{
        //    string _tag = "";
        //    ////System.IO.FileStream _image = new System.IO.FileStream(_fileFullPath, System.IO.FileMode.Open);
        //    //// imageがnullだったら、終了。
        //    //if (_image == null) return _tag;

        //    //// PropertyItemsを格納していないイメージだったら、""。
        //    //if (_image.Tag == null)
        //    //{
        //    //    return _tag;
        //    //}
        //    //else
        //    //{
        //    //    // Tagオブジェクトの文字列を取得する
        //    //    object _tagObject = _image.Tag;
        //    //    //文字列に変換する
        //    //    string _tagString = _tagObject.ToString();
        //    //    _tag = _tagString;
        //    //}
        //    return _tag;
        //}
        ///// <summary>
        ///// MP3ファイルにタグを追加します。
        ///// タグ_addTagStringには、「値（String型）」、もしくは名前を付けて「TagName:Value（どちらもString型）」Or「タグ名：値」（名前や値・コロンは大文字でもOK）で指定してください。
        ///// タグは、addする毎に自動的に「,」で区切られて保存されます。
        ///// また、特定のタグを取得する時は、getTag_MP3("タグ名")、もしくは値が存在することを確認するメソッドであるisTagAdded_Image("値")、またはisTagAdded_MP3("タグ名")を使ってください。
        ///// </summary>
        //public static void addTag_MP3(string _mp3File_FullPath, string _addTagString)
        //{
        //    //// imageがnullだったら、終了。
        //    //if (_mp3File_FullPath == "") return;

        //    //string _defaultTag = "タグ情報:C#プログラム上で独自に作成したタグが付加されています。"; // 最初に付けるタグタイトル
        //    //// Tagを格納していないイメージだったら、新しく適当なものを格納させる。
        //    //if (_mp3File_FullPath.Tag == null)
        //    //{
        //    //    string _tagString = _defaultTag + ",";
        //    //    // オブジェクトのコピーを渡す（(object)_tagStringよりは安心かな、と）
        //    //    object _newTagObj = _tagString.Clone();
        //    //    _mp3File_FullPath.Tag = _newTagObj;
        //    //}
        //    //object _tagObject = _mp3File_FullPath.Tag;
        //    //if (_tagObject == null)
        //    //{
        //    //    // 上記で追加しているはずなのに、タグが付加されていないエラー。ブレークポイント
        //    //    int _error = 1;
        //    //}
        //    //else
        //    //{
        //    //    //文字列に変換する
        //    //    string _tagString = _tagObject.ToString();
        //    //    //値を変更する
        //    //    _tagString = _tagString + _addTagString + ",";
        //    //    // オブジェクトのコピーを渡す（(object)_tagStringよりは安心かな、と）
        //    //    _tagObject = _tagString.Clone();
        //    //    //格納する
        //    //    _mp3File_FullPath.Tag = _tagObject;
        //    //}
        //}
        ///// <summary>
        ///// MP3ファイルに指定した作者の名前（ユーザＩＤ）をタグとして付加します。複数人付加することができます。
        ///// </summary>
        //public static void addTagAuthor_MP3(string _mp3File_FullPath, string _TagAutor_UserID)
        //{
        //    addTag_MP3(_mp3File_FullPath, "作者:" + _TagAutor_UserID);
        //}
        #endregion


        // □□□□　↑　以上は、できるだけ環境非依存のメソッドを書いてください。もちろん.NET Framework依存はですが。
        
        // System.Windowsが入るメソッドは、できるだけこれより下に書いてください。





        // □□□□　↓　以下は、環境依存のメソッドです。

        // Windows非依存、dll非依存にしたい場合は、まるごと削除orコメントアウトしてもかまいません。




        // ■音楽系 MCI依存。
        // ※WaveIO2やClassLibrarySub3を作った人のネームスペースのSampleRecorder、SamplePlayerに依存しているので注意して！
        #region サウンド再生・録音処理。　System標準のSoundPlayerクラス、MCI（MediaControlInterface）、WMP（WindowsMediaPlayer）、PlaySoundというWindows32APIを試しましたが、できたものとできてないものがあります。playSound()とstopSound()など、一応出来てるやつを総動員して使えるメソッドにしたやつもあります。

        /// <summary>
        /// mp3/wav/midファイルをＢＧＭや効果音として（非同期で）再生します．
        /// 理論的に再生チャネルが空いていて、再生出来るようだったらtrue（これがtrueでも実際音が鳴らない原因不明なエラーは多々ありますが）、
        /// ファイルが見つからないなどのエラーやチャネルが空いていなくて再生不可能な場合はfalseを返します。
        /// 
        /// 既に一度再生中にもう一度このメソッドが呼ばれた場合は、複数の方法を使ってなるべくたくさんの音を同時に鳴らせるようにします。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static bool playSound(string _fileName_FullPath)
        {
            return playSound(_fileName_FullPath, false);
        }
        /// <summary>
        /// 全てのサウンドを停止します。停止する前に、音が鳴っていたか（何かのファイルが再生中だったか）を返します。
        /// </summary>
        /// <returns></returns>
        public static bool stopSound()
        {
            bool _isPlaying = false;
            if (SoundPlayer_canPlay() == false)
            {
                // SoundPlayerは再生が終了したかを確認できないため、_isPlayingはあてにならないのでパスしとく
                p_soundPlayer.Stop();
            }
            if (p_isUseMCI == true && MCI_canPlay() == false)
            {
                _isPlaying = true;
                MCI_stopBGM();
            }
            if (p_isUseWMP == true && WMP_canPlay() == false)
            {
                _isPlaying = true;
                WMP_stopSound();
            }
            updatePlayingSoundFileNum();
            return _isPlaying;
        }
        /// <summary>
        /// マスターボリュームを0-1000の範囲で調整します。前のボリュームを返します。setVolume_ByWindowsAPI(_volume_0To1000)をそのまま使ってます。
        /// </summary>
        public static int setVolume_Master(int _volume_0To1000)
        {
            return setVolume_ByWindowsAPI(_volume_0To1000);
        }
        /// <summary>
        /// マスターボリュームを0-1000の範囲で調整します。getVolume_ByWindowsAPI()をそのまま使ってます。
        /// </summary>
        public static int getVolume_Master()
        {
            return getVolume_ByWindowsAPI();
        }


            #region ■■■難しいことは気にせずサウンド同時再生ハイブリット方式:playSound　（System.Media.SoundPlayerクラスでwav、それが使えなかったらMCIやWMPでwav/wma/midファイルを再生する方法）
        // 参考。感謝。 http://www.geocities.co.jp/NatureLand/2023/reference/Multimedia/sound01.html#jmp03        

        /// <summary>
        /// mp3/wav/midファイルをＢＧＭや効果音として（非同期で）再生します．
        /// 理論的に再生チャネルが空いていて、再生出来るようだったらtrue（これがtrueでも実際音が鳴らない原因不明なエラーは多々ありますが）、
        /// ファイルが見つからないなどのエラーやチャネルが空いていなくて再生不可能な場合はfalseを返します。
        /// 
        /// 既に一度再生中にもう一度このメソッドが呼ばれた場合は、複数の方法を使ってなるべくたくさんの音を同時に鳴らせるようにします。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static bool playSound(string _fileName_FullPath, bool _isRepeat)
        {
            bool _canPlay = true;

            //再生するファイル名
            string fileName = _fileName_FullPath;
            // ファイルが存在しなければ終了
            if (isExist(_fileName_FullPath) == false)
            {
                ConsoleWriteLine("playSound: ※エラー：　下記の音楽ファイルは存在しないか、アクセス不許可です。 音楽ファイル名 :" + fileName);
                _canPlay = false;
                return _canPlay;
            }

            string _format = MyTools.getFileRightOfPeriodName(fileName);
            if (_format == "")
            {
                ConsoleWriteLine("playSound: ※エラー：　下記の音楽ファイルの名前に拡張子が付いていません。 音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(fileName));
                _canPlay = false;
            }
            else
            {
                if (_format == "wav")
                {
                    // WAVの再生は、まずSoundPlayerからが優先
                    if (checkPlayMethod_isOkByUsingSoundPlayer_InWaveFile(_fileName_FullPath) == true)
                    {
                        // 【WAV1】SoundPlayerが空いていれば、まず優先してwavを再生
                        if (_isRepeat == false)
                        {
                            p_soundPlayer.Play(); // 非同期再生。ただしこの方法では２個以上の同時再生はできない
                        }
                        else
                        {
                            p_soundPlayer.PlayLooping();
                        }
                        ConsoleWriteLine("SoundPlayer：　WAVファイルを再生しました。 音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(fileName));
                        // 再生が終了したらnullに戻すスレッドを非同期で生成
                        //スレッドは面倒だから今はcheckIsnull・・・で上書きするかをチェックしている。threadSubMethod(checkIsStoped_AndNullP_SoundPlayer);
                    }
                    else
                    {
                        _canPlay = false;
                        // SoundPlayerが空いてなかったら、他の方法でWAVを再生
                        if (p_isUseMCI == true && MCI_canPlay() == true)
                        {
                            ConsoleWriteLine("playSound：　SoundPlayerクラスが空いてないので、MCIでWAVファイルの再生を試みます。音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(fileName));
                            // 【WAV2】MCIで再生を試みる
                            if (MySound_Windows.MCI_playSE(_fileName_FullPath, _isRepeat) != -1)
                            {
                                _canPlay = true;
                            }// MCI_playBGMは１個再生用なので使わない。
                        }
                        if (_canPlay == false && p_isUseWMP == true && WMP_canPlay() == true)
                        {
                            ConsoleWriteLine("playSound：　SoundPlayerクラスが空いてないので、WMPでWAVファイルの再生を試みます。音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(fileName));
                            // 【WAV3】WMPで再生を試みる
                            _canPlay = MySound_Windows.WMP_playSound(_fileName_FullPath, _isRepeat);
                        }
                    }
                }
                else if (_format == "mp3" || _format == "mid" || _format == "wma") // WAV以外
                {
                    _canPlay = false;
                    // WAV以外のサウンドを再生
                    if (_canPlay == false && p_isUseMCI == true && MCI_canPlay() == true)
                    {
                        // 【MP3の1】【midの1】【wmaの1】MCIで再生を試みる
                        if (MySound_Windows.MCI_playSE(_fileName_FullPath, _isRepeat) != -1)
                        {
                            _canPlay = true;
                        }// MCI_playBGMは１個再生用なので使わない。
                    }
                    if (_canPlay == false && p_isUseWMP == true && WMP_canPlay() == true)
                    {
                        ConsoleWriteLine("playSound：　MCIクラスが空いてないので、WMPでサウンドファイルの再生を試みます。音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(fileName));
                        // 【MP3の2】【midの2】【wmaの2】WMPで再生を試みる
                        _canPlay = MySound_Windows.WMP_playSound(_fileName_FullPath, _isRepeat);
                    }
                    if (_canPlay == false)
                    {
                        ConsoleWriteLine("playSound: ※警告：　下記の音楽ファイルは、エラーか、再生チャンネルが空いていなかったため、再生できませんでした。 音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(fileName));
                    }
                }
                else
                {
                    ConsoleWriteLine("playSound: ※エラー：　下記の音楽ファイル名の拡張子に、この音楽再生メソッドが対応していません。 音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(fileName));
                    _canPlay = false;
                }
            }
            // （理論的に）同時再生サウンド数を計算する
            updatePlayingSoundFileNum();
            return _canPlay;
        }
        private static System.Media.SoundPlayer p_soundPlayer;
        /// <summary>
        /// 現在鳴らしている音楽ファイルの数。updatePlayingSoundFileNumメソッドで管理します。
        /// </summary>
        private static int p_playSound_playingSoundFileNuml = 0;
        public static int getPlayingSoundFileNum() { return p_playSound_playingSoundFileNuml; }
        /// <summary>
        /// MCI(MediaControlInterface)で再生中のサウンドがチャネルが空いていれば、trueを返します。（ポーズは後で継続再生される可能性があるため、falseを返す）。
        /// </summary>
        /// <returns></returns>
        public static bool MCI_canPlay()
        {
            bool _isEmpty = true;
            // (i)上書き再生を許可する場合（MCI_PlayBGM()やMCI_PlaySE()メソッド内で判定する場合、もしくは他の再生チャンネル数が少なくmp3専用に使えない場合）。
            _isEmpty = true;
            // (ii)上書き再生を防止する場合（まだ一度も再生されていないか、既に再生終了しているときだけtrueを返します。（ポーズは後に再生される可能性があるため、falseを返す）
            //if (MCI_isEndBGM() == true) _isEmpty = false;
            return _isEmpty;
        }
        /// <summary>
        /// （※現実装では必ずtrueを返します）　WMP(WindowMediaPlayer)で再生中のサウンドがチャネルが空いていれば、trueを返します。（ポーズは後に再生される可能性があるため、falseを返す）。
        /// </summary>
        /// <returns></returns>
        public static bool WMP_canPlay()
        {
            bool _isEmpty = true;
            // (i)上書き再生を許可する場合（再生チャンネル数が少ないので、現時点ではこっち）。
            _isEmpty = true;
            // (ii)上書き再生を防止する場合（まだ一度も再生されていないか、既に再生終了しているときだけtrueを返します。（ポーズは後に再生される可能性があるため、falseを返す）
            //int _soundLengthMSec = WMP_getSoundLengthMSec();
            //if (_soundLengthMSec != 0 && _soundLengthMSec <= WMP_getSoundPlayingMSec()) _isEmply = false;
            return _isEmpty;
        }
        /// <summary>
        /// （※現実装では必ずtrueを返します） p_playingSoundFileNum更新などに使います。SoundPlayerクラスのチャネルが空いていれば、trueを返します。
        /// </summary>
        /// <returns></returns>
        public static bool SoundPlayer_canPlay()
        {
            bool _isEmpty = true;
            // (i)上書き再生を許可する場合（再生チャンネル数が少ないので、現時点ではこっち）。
            _isEmpty = true;
            // (ii)上書き再生を防止する場合（まだ一度も再生されていないか、既に再生終了していれば）trueを返します。（ポーズは後に再生される可能性があるため、falseを返す）
            //if (p_soundPlayer != null) _isEmpty = false;
            return _isEmpty;
        }

        /// <summary>
        /// SoundPlayerクラスのチャネルが空いていて、新しいサウンドを再生可能かを示します。
        /// 内部では、SoulPlayerクラスでＷａｖを再生していいかを判定し、よければtrueを返します。
        /// （trueの場合、このままp_soundPlayer.Play()をするだけでＯＫです。
        /// 　falseの場合、他の方法でＷａｖファイルを再生してください）
        /// </summary>
        private static bool checkPlayMethod_isOkByUsingSoundPlayer_InWaveFile(string _fileName_FullPath)
        {
            // SoundoPlayerクラスを使用するか
            bool _isUseSoundPlayer = true;
            if (p_soundPlayer == null)
            {
                p_soundPlayer = new System.Media.SoundPlayer(_fileName_FullPath);
                // このコンストラクタを呼び出すと、LoadやSetLocationなどの呼び出しは要らないはず
            }
            else if (p_soundPlayer.SoundLocation == _fileName_FullPath)
            {
                // ■同じファイルをもう一度再生しようとしている
                // 前のファイルの再生が終わっているか
                // if (SoundPlayer_isStopped() == true) // これを確かめる方法がわからないから、現段階ではずっとfalseにする
                if (false)
                {
                    // もう前のファイルの再生が終わっているなら、そのまま素通りでもっかい再生させやればよろし
                }
                else
                {
                    // 前のファイルを上書きして再生。もし前のファイルが再生中だったら、その時ブチっと切りたくないから、他の方法に任せるやり方を考える
                    p_soundPlayer.Stop();
                }
            }
            else// if (p_soundPlayer.SoundLocation != _fileName0_FullPath)
            {
                // ■既に他のファイルが再生中（現時点終了タイミング時を検出していないので、終了している可能性もある）

                if (SoundPlayer_canPlay() == true)
                {
                    // 空いていると判定して、上書きして再生
                    p_soundPlayer.Dispose();
                    p_soundPlayer = new System.Media.SoundPlayer(_fileName_FullPath);
                    // このコンストラクタを呼び出すと、LoadやSetLocationなどの呼び出しは要らないはず
                }
                else if ((p_isUseMCI == true && MCI_canPlay() == true) || (p_isUseWMP == true && WMP_canPlay() == true))
                {
                    // MCIまたはWMPのチャネルが空いているので、SoudPlayerチャネルは使わない
                    _isUseSoundPlayer = false;
                }
                else
                {
                    // MCIにもWMPにもチャネルに空きがないので、SoundPlayerチャネルを上書きして再生
                    p_soundPlayer.Dispose();
                    p_soundPlayer = new System.Media.SoundPlayer(_fileName_FullPath);
                    // このコンストラクタを呼び出すと、LoadやSetLocationなどの呼び出しは要らないはず
                }
            }
            return _isUseSoundPlayer;
        }
        /// <summary>
        /// （理論的に）同時再生サウンド数を計算し、更新します。
        /// </summary>
        private static void updatePlayingSoundFileNum()
        {
            p_playSound_playingSoundFileNuml = 0;
            if (SoundPlayer_canPlay() == false) p_playSound_playingSoundFileNuml++;
            if (p_isUseMCI == true && MCI_isEndBGM() == true) p_playSound_playingSoundFileNuml++;
            if (p_isUseWMP == true && WMP_isPlaying() == true) p_playSound_playingSoundFileNuml++;
        }
        #region 草案、音楽再生終了を待つメソッド。いまは処理を重くしないため、再生前に調べるようにしている
        ///// <summary>
        ///// 一定時間ごとにSoundPlayerクラスで現在再生されている音が終わった／停止されたかをチェックして、終わっていればメモリを開放してnullにします。
        ///// </summary>
        ///// <returns></returns>
        //private static bool checkIsStoped_AndNullP_SoundPlayer()
        //{
        //    bool _isStoped = false;
        //    if (p_soundPlayer != null)
        //    {
        //        while(_isStoped == false){
        //            updatePlayingSoundFileNum(); // 現在のサウンド同時再生数の更新
        //            if (p_soundPlayer.isStoped() == true)// これを取得する方法が解からない)
        //            {
        //                _isStoped = true;
        //            }
        //            Thread.Sleep(1000); // 指定ミリ秒後、再チェック
        //        }
        //        p_soundPlayer.Dispose();
        //        p_soundPlayer = null;
        //    }
        //}
        #endregion

        /// <summary>
        /// SoundPlayerクラスを使って、wavファイルを再生します．理論的に再生できそうだったらtrue（これがtrueでも再生できない原因不明なエラーは多々ありますが）、理論的に再生不可能な場合はfalseを返します。
        /// </summary>
        public static bool SoundPlayer_playSound(string _fileName_FullPath, bool _isRepeat)
        {
            bool _canPlay = true;
            // ファイルが存在しなければ終了
            if (isExist(_fileName_FullPath) == false)
            {
                ConsoleWriteLine("WMP_playSound: ※エラー：　下記の音楽ファイルは存在しないか、アクセス不許可です。 音楽ファイル名 :" + _fileName_FullPath);
                _canPlay = false;
                return _canPlay;
            }

            if (p_soundPlayer == null)
            {
                p_soundPlayer = new System.Media.SoundPlayer(_fileName_FullPath);
                // このコンストラクタを呼び出すと、LoadやSetLocationなどの呼び出しは要らないはず
            }
            else if (p_soundPlayer.SoundLocation == _fileName_FullPath)
            {
                // ■同じファイルをもう一度再生しようとしている
                // 前のファイルの再生が終わっているか
                // if (SoundPlayer_isStopped() == true) // これを確かめる方法がわからないから、現段階ではずっとfalseにする
                if (false)
                {
                    // もう前のファイルの再生が終わっているなら、そのまま素通りでもっかい再生させやればよろし
                }
                else
                {
                    // 前のファイルを上書きして再生。もし前のファイルが再生中だったら、その時ブチっと切りたくないから、他の方法に任せるやり方を考える
                    p_soundPlayer.Stop();
                }
            }
            else if (p_soundPlayer.SoundLocation != _fileName_FullPath)
            {
                // ■既に他のファイルが再生中（現時点では再生してから何も処理していないので、終了している場合も含む）の場合、
                // 違うファイルを再生しようとする場合、上書きする条件
                if (true)
                { // SoundPlayerでは２つ以上のwaveファイルを一度に再生できない。再生時間も取れないので、必ず上書き
                    p_soundPlayer.Dispose();
                    p_soundPlayer = new System.Media.SoundPlayer(_fileName_FullPath);
                    // このコンストラクタを呼び出すと、LoadやSetLocationなどの呼び出しは要らないはず
                }
            }
            // ロードするまで待つ（要る？）
            //while(p_soundPlayer.IsLoadCompleted)
            //{
            //    MyTools.wait_ByApplicationDoEvents();
            //}
            // サウンド再生
            if (_isRepeat == true)
            {
                p_soundPlayer.PlayLooping();
            }
            else
            {
                p_soundPlayer.Play();
            }
            return _canPlay;
        }
        /// <summary>
        /// SoundPlayerクラスが現在持っているwavファイルを再生します（一時停止の再開resume機能も備えます）．現在持っているサウンドファイルがない場合はfalseを返します。
        /// </summary>
        public static bool SoundPlayer_playSound(bool _isRepeat){
            bool _isHavingSound = false;
            if (p_soundPlayer != null)
            {
                _isHavingSound = true;
                if (_isRepeat == true)
                {
                    p_soundPlayer.PlayLooping();
                }
                else
                {
                    p_soundPlayer.Play();
                }
            }
            return _isHavingSound;
        }
        /// <summary>
        /// SoundPlayerで再生されているサウンドファイルを停止します。
        /// </summary>
        /// <returns></returns>
        public static int SoundPlayer_stopSound(){
            int _isSuccess = 0;
            // 再生時間がわからないから判断しようがないbool _isPlaying = false;
            if (p_soundPlayer != null)
            {
                p_soundPlayer.Stop();
            }
            return _isSuccess;
        }
        // SoundPlayerクラスでは、独自に音量・取得設定が出来ない
        //public static int SoundPlayer_getVolume()
        //{
        //    getVolume_Master();
        //}
        // SoundPlayerクラスでは、曲の長さも取得できないので、独自に再生しているかが取得出来ない
        //public static int SoundPlayer_isPlaying()
        //{
        //}
        /// <summary>
        /// SoundPlayerクラスが持っているサウンドファイル名のフルパスを取得します。無い場合は""を返します。
        /// </summary>
        /// <returns></returns>
        public static string SoundPlayer_getFileNameFullPath()
        {
            string _fileNameFullPath = "";
            if (p_soundPlayer != null)
            {
                _fileNameFullPath = p_soundPlayer.SoundLocation;
            }
            return _fileNameFullPath;
        }

            #region ■WMP(Windows Media Player)を使って、コントロールをフォームに配置せずに再生する方法
        // 参考。感謝。http://dobon.net/vb/dotnet/programing/playmidifile.html#wmp
        // ユーザーインターフェイスが必要なければ、フォームに配置する必要もありません。
        // フォームに配置しない場合は、次のようなコードを書きます。
        // なお、wmp.dllとWMPLib.dllを参照に追加する必要があります
        // （フォームにWindows Media Playerを一度配置すれば、これらは自動的に追加されます）。
        private static WMPLib.WindowsMediaPlayer p_WindowsMediaPlayer;
        /// <summary>
        /// WindowsMediaPlayerクラスを使って、mp3/wav/mid/wmaファイルを再生します．理論的に再生できそうだったらtrue（これがtrueでも再生できない原因不明なエラーは多々ありますが）、理論的に再生不可能な場合はfalseを返します。
        /// 
        ///         // ※なお、メルサイアのVAIOノートＰＣではWMPを使った方法だとなぜか音が鳴りません…ライブラリもいるのに、WMP_系、作った意味無しorz
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        /// <param name="_isRepeated"></param>
        /// <returns></returns>
        public static bool WMP_playSound(string _fileName_FullPath, bool _isRepeated)
        {
            bool _canPlay = true;
            // ファイルが存在しなければ終了
            if (isExist(_fileName_FullPath) == false)
            {
                ConsoleWriteLine("WMP_playSound: ※エラー：　下記の音楽ファイルは存在しないか、アクセス不許可です。 音楽ファイル名 :" + _fileName_FullPath);
                _canPlay = false;
                return _canPlay;
            }

            // nullだったら新規作成
            if(p_WindowsMediaPlayer == null){
                p_WindowsMediaPlayer = new WMPLib.WindowsMediaPlayer();
            }
            if(p_WindowsMediaPlayer != null){
                try{
                    // ■既に再生されていてもURLを変更して上書き（上書き再生モード）
                    //オーディオファイルを指定する（自動的に再生される）
                    string _shortFileName = MySound_Windows.MCI_getShortPathName_ByWindowsAPI(_fileName_FullPath);
                    p_WindowsMediaPlayer.URL = _shortFileName;
                    // ショートネームの方が失敗が少ない？
                    //p_WindowsMediaPlayer.URL = _fileName_FullPath_or_NotFullPath;
                    // リピートの設定
                    WMP_setRepeat(_isRepeated);
                    //再生する
                    p_WindowsMediaPlayer.controls.play();
                    ConsoleWriteLine("WMP_playSound: : 次のファイルの再生しました（理論上は成功した…と思います）。 ファイル名:" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath));

                }catch(Exception e){
                    // 再生できなかった時のエラー処理の取り方がチープ。。
                    ConsoleWriteLine("WMP_playSound: ※エラー: 次のファイル再生時に例外が検出されました。 ファイル名:"+getFileName_TheMostRightFileOrDirectory(_fileName_FullPath));
                    _canPlay = false;
                }
            }
            return _canPlay;
        }
        /// <summary>
        /// WMPで現在サウンドを再生しているかを返します。
        /// </summary>
        /// <returns></returns>
        public static bool WMP_isPlaying()
        {
            bool _isPlaying = false;
            if (WMP_getPlayState() == 3)
            {
                _isPlaying = true;
            }
            return _isPlaying;
        }
        /// <summary>
        /// WMPで現在再生しているサウンドを一時停止します。WMP_playSound()で継続再生します。
        /// </summary>
        public static bool WMP_pauseSound()
        {
            bool _isPlaying = WMP_isPlaying();
            // nullじゃなかったら
            if (p_WindowsMediaPlayer != null)
            {
                //再生しているオーディオを停止する
                p_WindowsMediaPlayer.controls.pause();
            }
            return _isPlaying;
        }
        /// <summary>
        /// WMPで現在再生しているサウンドを全て停止します。
        /// </summary>
        public static bool WMP_stopSound(){
            bool _isPlaying = false;
            // nullじゃなかったら
            if (p_WindowsMediaPlayer != null)
            {
                //再生しているオーディオを停止する
                p_WindowsMediaPlayer.controls.stop();
            }
            return _isPlaying;
        }
        /// <summary>
        /// 状態を示すint型の値playStateを返します。　　　　　　　返り値playStateの意味 ：
        /// 
        /// 値 	状態 	説明
        /// -----------------------------
        /// 0 	Undefined 	Windows Media Player の状態が定義されません。
        /// 1 	Stopped 	現在のメディア クリップの再生が停止されています。
        /// 2 	Paused 	現在のメディア クリップの再生が一時停止されています。メディアを一時停止した場合は、再生が同じ位置から再開されます。
        /// 3 	Playing 	現在のメディア クリップは再生中です。
        /// 4 	ScanForward 	現在のメディア クリップは早送り中です。
        /// 5 	ScanReverse 	現在のメディア クリップは巻き戻し中です。
        /// 6 	Buffering 	現在のメディア クリップはサーバーからの追加情報を取得中です。
        /// 7 	Waiting 	接続は確立されましたが、サーバーがビットを送信していません。セッションの開始を待機中です。
        /// 8 	MediaEnded 	メディアの再生が完了し、最後の位置にあります。
        /// 9 	Transitioning 	新しいメディアを準備中です。
        /// 10 	Ready 	再生を開始する準備ができています。
        /// </summary>
        /// <returns></returns>
        public static int WMP_getPlayState()
        {
            int _playState = 0;
            if (p_WindowsMediaPlayer != null)
            {
                _playState = (int)(p_WindowsMediaPlayer.playState);
            }
            return _playState;
        }
        /// <summary>
        /// プレイリストにサウンドを追加します。プレイリストのサウンドファイル数を返します。
        /// </summary>
        public static int WMP_addPlayList(string _fileNameFullPath)
        {
            int _playListNum = 0;
            if (p_WindowsMediaPlayer != null)
            {
                if (p_WindowsMediaPlayer.currentPlaylist == null)
                {
                    // なければ新しくプレイリストを作成
                    p_WindowsMediaPlayer.newPlaylist("WMP_playlist1", p_WindowsMediaPlayer.currentMedia.sourceURL);
                }
                p_WindowsMediaPlayer.currentPlaylist.appendItem(p_WindowsMediaPlayer.newMedia(_fileNameFullPath));
                _playListNum = p_WindowsMediaPlayer.currentPlaylist.count;
            }
            return _playListNum;            
        }
        /// <summary>
        /// 次のプレイリストがある場合、次のファイルを再生します。次のプレイリストがあるかどうかを返します。
        /// </summary>
        public static bool WMP_playNext_PlayList()
        {
            bool _isNextPlayListExist = false;
            int _plyState = WMP_getPlayState();
            // 対象となるメディアがある場合のみ末尾へ移動できる
            if (_plyState > 0)
            {
                _isNextPlayListExist = true;
                p_WindowsMediaPlayer.controls.currentPosition = p_WindowsMediaPlayer.currentMedia.duration;
            }
            return _isNextPlayListExist;
        }
        /// <summary>
        /// 現在のファイルを最初から再生し直します。りスタートする前の再生時間を返します。
        /// </summary>
        public static int WMP_playAtFirst_PlayList()
        {
            int _lastPlayingMSec = 0;
            if (p_WindowsMediaPlayer != null)
            {
                _lastPlayingMSec = WMP_getSoundPlayingMSec();
                p_WindowsMediaPlayer.controls.currentPosition = 0;
            }
            return _lastPlayingMSec;
        }
        /// <summary>
        /// 現在のファイルが再生された再生時間をミリ秒で返します。
        /// </summary>
        public static int WMP_getSoundPlayingMSec()
        {
            int _MSec = 0;
            if(p_WindowsMediaPlayer != null){
                _MSec = (int)(p_WindowsMediaPlayer.controls.currentPosition * 1000);
            }
            return _MSec;
        }
        /// <summary>
        /// 現在のファイルの全部の再生時間をミリ秒で返します。
        /// </summary>
        public static int WMP_getSoundLengthMSec()
        {
            int _MSec = 0;
            if (p_WindowsMediaPlayer != null)
            {
                _MSec = (int)(p_WindowsMediaPlayer.controls.currentItem.duration * 1000);
            }
            return _MSec;
        }
        /// <summary>
        /// WMPの現在の音量を(0-1000)で返します。（マスターボリュームgetSound_Masterとはまた違うもので、別々に設定できます。例えば、WMPだけ小さくしたりできます。）
        /// </summary>
        public static int WMP_getVolume()
        {
            int _volume = 0;
            if (p_WindowsMediaPlayer != null)
            {
                // OSの音量とは違うので、コントロールを複数同時に使ってそれぞれのボリュームコントロールが可能。http://www.gan.st/gan/blog/index.php?itemid=1406
                int _volume_0To100 = p_WindowsMediaPlayer.settings.volume;
                _volume = _volume_0To100 * 10;
            }
            return _volume;
        }
        /// <summary>
        /// WMPの再生音量を(0-1000)で設定します。以前設定されていた音量を返します。（マスターボリュームgetSound_Masterとはまた違うもので、別々に設定できます。例えば、WMPだけ小さくしたりできます。）
        /// </summary>
        public static int WMP_setVolume(int _volume_0To1000)
        {
            int _beforevolume = 0;
            int _volume_0To100 = _volume_0To1000 / 10;
            if (p_WindowsMediaPlayer != null)
            {
                // OSの音量とは違うので、コントロールを複数同時に使ってそれぞれのボリュームコントロールが可能。http://www.gan.st/gan/blog/index.php?itemid=1406
                int _beforevolume_0To100 = p_WindowsMediaPlayer.settings.volume;
                _beforevolume = _beforevolume_0To100 * 10;
                p_WindowsMediaPlayer.settings.volume = _volume_0To100;
            }
            return _beforevolume;
        }
        // その他、リピート、倍速再生、早送りなど http://www.cosmosoft.org/Geeklog/article.php?story=20120201153203616
        /// <summary>
        /// WindowsMediaPlayerクラスで現在再生されているサウンドの、リピートON/OFFを設定します。以前設定されていたリピートのON/OFFを返します。
        /// </summary>
        public static bool WMP_setRepeat(bool _isRepeatPlay)
        {
            bool _beforeRepeatMode = false;
            if (p_WindowsMediaPlayer != null)
            {
                _beforeRepeatMode = p_WindowsMediaPlayer.settings.getMode("loop");
                p_WindowsMediaPlayer.settings.setMode("loop", _isRepeatPlay);
            }
            return _beforeRepeatMode;
        }
        /// <summary>
        /// 現在の再生スピードを0.5～8.0で取得します。
        /// </summary>
        public static double WMP_getPlaySpeed()
        {
            double _Speed_Slow0_5_To_Normal1_To_Fast8 = 1.0;
            if (p_WindowsMediaPlayer != null)
            {
                _Speed_Slow0_5_To_Normal1_To_Fast8 = p_WindowsMediaPlayer.settings.rate;
            }
            return _Speed_Slow0_5_To_Normal1_To_Fast8;
        }
        /// <summary>
        /// 再生スピードを0.5～8.0で設定します。以前設定されていた再生スピードを返します。
        /// </summary>
        public static double WMP_setPlaySpeed(double _Speed_Slow0_5_To_Normal1_To_Fast8)
        {
            double _beforeSpeed = 1.0;
            if (p_WindowsMediaPlayer != null)
            {
                _beforeSpeed = p_WindowsMediaPlayer.settings.rate;
                _Speed_Slow0_5_To_Normal1_To_Fast8 = Math.Max(0.5, _Speed_Slow0_5_To_Normal1_To_Fast8);
                _Speed_Slow0_5_To_Normal1_To_Fast8 = Math.Min(8.0, _Speed_Slow0_5_To_Normal1_To_Fast8);
                p_WindowsMediaPlayer.settings.rate = _Speed_Slow0_5_To_Normal1_To_Fast8;
            }
            return _beforeSpeed;
        }
        /// <summary>
        /// 巻き戻しをします。
        /// </summary>
        public void WMP_playFastReverse()
        {
            if (p_WindowsMediaPlayer != null)
            {
                p_WindowsMediaPlayer.controls.fastReverse();
            }
        }
        /// <summary>
        /// 早送りをします。
        /// </summary>
        public void WMP_playFastForward()
        {
            if (p_WindowsMediaPlayer != null)
            {
                p_WindowsMediaPlayer.controls.fastForward();
            }
        }
            #endregion

            #region ■mikeo410様のSampleRecorderなどをつかったマイク録音とサウンド再生（フォーム非表示でも使えるはず）
        // mikeo410様に感謝。参考ＵＲＬ: http://mikeo410.lv9.org/lumadcms/~PCMDLLWaveIO2DLL
        private static SampleRecorder.Form1 p_sampleRecorder;
        private static SamplePlayer.Form1 p_samplePlayer;
        /// <summary>
        /// マイク録音を開始します。既に録音中の場合はfalseを返します。
        /// </summary>
        /// <returns></returns>
        public static bool recordMic_Start()
        {
            if (p_sampleRecorder == null)
            {
                p_sampleRecorder = new SampleRecorder.Form1();
                // 見せないp_sampleRecorder.Show(); // フォームを見せる
            }
            if (p_sampleRecorder.p_isRecording == true)
            {
                // 録音中
                return false;
            }
            else
            {
                if (p_sampleRecorder.startRecord() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// マイク録音を停止します。マイク録音中の場合だけ、停止して、録音したサウンドファイルのフルパスを返します。
        /// 録音していない場合は、""が返ります。
        /// </summary>
        /// <returns></returns>
        public static string recordMic_Stop()
        {
            if (p_sampleRecorder != null)
            {
                if (p_sampleRecorder.p_isRecording == true)
                {
                    p_lastRecordedMicSound_FileName_FullPath = p_sampleRecorder.stopRecord();
                    return p_lastRecordedMicSound_FileName_FullPath;
                }
            }
            return "";
        }
        /// <summary>
        /// 最後にrecordMic_Start()～recordMic_Stop()で記録したマイク録音音声のサウンドファイルのフルパス。
        /// </summary>
        public static string p_lastRecordedMicSound_FileName_FullPath = "";
        /// <summary>
        /// 最後に記録したマイク録音音声を再生します。再生できない場合はfalseを返します。
        /// </summary>
        /// <returns></returns>
        public static bool play_LastRecordedMicSound()
        {
            return SoundPlayer_playSound(p_lastRecordedMicSound_FileName_FullPath, false);
            // これだとうまく動かないことが多い。
            //if (p_sampleRecorder != null)
            //{
            //    return p_sampleRecorder.playSound_LastRecordedd();
            //}else{
            //    return false; 
            //}
        }
            #endregion

            #region ■MCI（Media Control Inferface）を使った方法
        // 数か月の思考錯誤の結果、MCIだけでＢＧＭを再生しながら複数の効果音を同時再生できるようになった♪
        // WindowsVistaの環境（メルサイアのＶＡＩＯノートＰＣで確認）では、同時再生は３２個まで動作確認済み。エイリアス名を分割するだけでＯＫだったみたい。
        // 
        //■MCIの欠点
        //
        //MCI はメモリにあるWAVE等を操作する事ができません。
        //リソースからの操作もできません。
        //ファイルだけです。
        //従って、リソースデータを操作するには、一時ファイルに書き出す必要があります。
        //●●●重要！●●●また、オープンしたスレッド以外からは操作できません。
        //      例えば、ゲームメインスレッドでopenしたファイルを、
        //              Windowsフォームのボタンイベントでcloseしたりpauseしたりはできません。
        //              →　どちらもゲームメインスレッドでやった方がいいです。
        //
        // Note: Windows 98 などの古い OS の場合、MCISendString でファイルを open する時、
        // 「長いファイル名」では稀に失敗することがある(失敗条件不明)。
        // その場合は「短いファイル名」を使って open せよ。⇒ 短いファイル名の取得 http://homepage3.nifty.com/midori_no_bike/CS/sample.html
        #region MCIでファイルをオープンする時に長いファイル名だと失敗することがあるので、その時に対処法として使う、短いファイルの取得API
        /// <summary>
        /// 長いファイル名(パス付き)を、短いファイル名(8.3形式)に変換するAPIです。
        /// </summary>
        /// <param name="_s1_fileNameFullPath">ファイルのパス</param>
        /// <param name="_s2_shortPathName_StringBuilder"></param>
        /// <param name="_i1_shortPathName__StringBuilder_Capactiy"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        extern static int GetShortPathName(string _s1_fileNameFullPath, StringBuilder _s2_shortPathName_StringBuilder, int _i1_shortPathName__StringBuilder_Capactiy);
        // 返り値 = 出力された文字数(失敗した場合、0 が返る)
        // _s1_fileNameFullPath     = Long  Name
        // _s2_shortPathName_StringBuilder     = Short Name
        // _i1_shortPathName__StringBuilder_Capactiy     = Short Name Buffer Length
        //
        //string        long_name  = @"C:\BIN\ASCII コード表\chara_table.cpp" ;  // 例
        //StringBuilder short_name = new StringBuilder(260) ;
        //int           len = GetShortPathName(long_name, short_name, short_name.Capacity) ;
        // 結果)
        // short_name ← @"C:\BIN\ASCII~1\CHARA_~1.CPP"
        // len        ← 27
        //
        // Note: ディレクトリ名も短い形式に変換される。
        // Note: "kernel32.dll" は、Windows に標準でインストールされている。
        /// <summary>
        /// MCIでファイルをオープンする時に失敗することがあるので、その時に対処法として使う短いファイル名を取得します。
        /// 
        /// Windows 98 などの古い OS の場合、MCISendString でファイルを open する時、
        /// 「長いファイル名」では稀に失敗することがある(失敗条件不明)。
        /// その場合は「短いファイル名」を使って open せよ。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        /// <returns></returns>
        public static string MCI_getShortPathName_ByWindowsAPI(string _fileNameFullPath)
        {
            StringBuilder _shortPathStringBuilder = new StringBuilder(260);
            GetShortPathName(_fileNameFullPath, _shortPathStringBuilder, _shortPathStringBuilder.Capacity);
            string _shortPathName = _shortPathStringBuilder.ToString();
            return _shortPathName;
        }
        #endregion
        // MCIの高度な使い方。感謝。 http://pcdn.int21.co.jp/pcdn/vb/noriolib/vbmag/9802/mci/
        // MCIでmp3/wav/midファイルを再生する方法。参考URL。感謝。 http://dobon.net/vb/dotnet/programing/playmidifile.html
        /// <summary>
        /// MCIのコマンドを実行するAPIです。
        /// ※詳しい使い方が解からない場合は、これを直接呼び出すよりも、他のMCI_***のメソッドを使うと便利です。
        /// </summary>
        /// <param name="command">コマンド文字列。例："play エイリアス名 from 0"</param>
        /// <param name="buffer">MCIから値を取得する時のバッファ。必要ない場合はnullでいい。</param>
        /// <param name="bufferSize">バッファのサイズ。必要ない場合は0でいい。</param>
        /// <param name="hwndCallback">曲再生終了などをコールバックするウィンドウハンドル。必要ない場合はIntPtr.Zeroでいい。</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("winmm.dll",
        CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int mciSendString(string command,
        System.Text.StringBuilder buffer, int bufferSize, IntPtr hwndCallback);
        /// <summary>
        /// コマンド／エラー出力機能の有無を容易に変更可能な、mciSendString()のラッパーメソッドです。
        /// 現在は、そのままmciSendString()を呼び出しています。
        /// </summary>
        public static int MCISendString(string _command, System.Text.StringBuilder _buffer, int _bufferSize, IntPtr _hWindowHandleCallback)
        {
            // 一斉デバッグしない時
            int _return = mciSendString(_command, _buffer, _bufferSize, _hWindowHandleCallback);
            return _return;
            // 一斉デバッグする時
            //return DEBUGShown_MCISendString(_command, _buffer, _bufferSize, _hWindowHandleCallback);
        }
        /// <summary>
        /// ※デバッグ用に使ってください。
        /// コンソール出力は意外と処理時間がかかるので、エラーの原因が根本解決した場合は、
        /// 普通のDEBUG_MCISendString()メソッドを使うことをおススメします。
        /// 
        /// コマンド／エラー出力機能を備えた、MCISendString()のラッパーメソッドです。
        /// 実行に成功したらコマンドをそのままコンソール出力して0、
        /// 失敗したら（解かる範囲内で）失敗原因を出力してから-1を返します。
        /// </summary>
        public static int DEBUGShown_MCISendString(string _command, System.Text.StringBuilder _buffer, int _bufferSize, IntPtr _hWindowHandleCallback)
        {
            int _return = mciSendString(_command, _buffer, _bufferSize, _hWindowHandleCallback);
            bool _isTestDebug = false;
            if (_isTestDebug == true)
            {
                int _StackTraceNum = 12;
                if (_return == p_MCISendStringReturn_SUCCESS)//(_return == 0)
                {
                    // 成功
                    ConsoleWriteLine("MCISendString: 成功(0): " + _command + "@" + MyTools.getMethodStackString(_StackTraceNum));
                }
                else if (_return == p_MCISendStringReturn_MCIERR_INVALID_DEVICE_NAME) // ==263
                {
                    // 失敗。デバイス名が見つからないエラー（別スレッドで処理しようとした場合が多い）
                    ConsoleWriteLine("MCISendString: 失敗(" + _return + "): " + _command + ": 原因はデバイス名が不明。※まだopenされていないか、openしたスレッドと別スレッドで触ろうとしている可能性が高いです。@" + MyTools.getMethodStackString(_StackTraceNum));
                }
                else if (_return == p_MCISendStringReturn_MCIERR_CANNOT_LOAD_DRIVER) // ==266
                {
                    // 失敗。ドライバがロードできないエラー（よくわからない）
                    ConsoleWriteLine("MCISendString: 失敗(" + _return + "): " + _command + ": 原因はドライバがロード不可。すみません。上記など、エラー番号をネット検索して調べてみてください。@" + MyTools.getMethodStackString(_StackTraceNum));
                }
                else
                {
                    // 失敗。理由はよくわからない。
                    // この辺から理由を推測して。
                    // MSNの引数と例外名。 http://msdn.microsoft.com/ja-jp/library/aa228215%28VS.60%29.aspx
                    // 代表的なエラーメッセージの日本語。 http://source.winehq.org/transl/resource.php?lang=011%3A00&resfile=dlls%2Fwinmm&type=6&_id=17&compare=
                    ConsoleWriteLine("MCISendString: 失敗(" + _return + "): " + _command + ": 原因は分かりません。すみません。上記など、エラー番号をネット検索して調べてみてください。http://msdn.microsoft.com/ja-jp/library/aa228215%28VS.60%29.aspx" + MyTools.getMethodStackString(5));
                }
            }
            return _return;
        }
        /// <summary>
        /// =0。MCISendString()メソッドが正常に成功した時に返す値です。
        /// </summary>
        public static int p_MCISendStringReturn_SUCCESS = 0;
        /// <summary>
        /// =263。MCISendString()メソッドが失敗し、その原因がデバイスが見つからなかった場合（別スレッドで処理しようとした時に起こりやすい）に返す値です。
        /// ※openしたスレッドと別スレッドで触ろうとするとこのエラーになるみたい。
        ///     例: 
        /// 1.スレッドAでDEBUG_MCISendStringで"open ファイルパス alias my_sound"を送信。
        /// 2.スレッドAでDEBUG_MCISendStringで"play my_sound"を送信。←ここまでは動く
        /// 3.スレッドBでDEBUG_MCISendStringで"status my_sound volume"を送信。
        /// 4.戻り値が入っているであろうバッファをParseする。←例外発生
        /// 
        /// 要するにMCIのエイリアス名は単一スレッドしか触れない？
        /// 参考情報。感謝。http://d.hatena.ne.jp/ZerOx4C/20101010/1286691996#c
        /// </summary>
        private static int p_MCISendStringReturn_MCIERR_INVALID_DEVICE_NAME = 263;
        /// <summary>
        /// =263。MCISendString()メソッドが失敗し、その原因がドライバがロードできなかった場合に返す値です。
        /// </summary>
        private static int p_MCISendStringReturn_MCIERR_CANNOT_LOAD_DRIVER = 266;
        //private static string p_MCI_NowOpened_soundFileAliasName;// = "my_bgm"; // エイリアス、なんでもいい。だけど、staticプロパティはちゃんとstaticメソッドで初期化しよう
        //public static void MCI_setSoundFileAliasName(){
        //    p_MCI_NowOpened_soundFileAliasName = "my_bgm";
        //}
        /// <summary>
        /// 今MCIでＢＧＭ再生中のサウンドファイルのフルパス。未再生の場合は""。MCI_stopBGM()で""に初期化される。
        /// ストップ中かどうかはこれだけじゃ厳密には確認できないので、ちゃんとMCI_getSoundMode()を呼び出して確認して。
        /// </summary>
        private static string p_MCI_nowPlayingBGMName_FullPath = "";
        /// <summary>
        /// 現在再生されているＢＧＭの音量（新しくＢＧＭをロードしても引き継ぎます）。
        /// MCI_setVolume_BGM()をすることで更新されます。初期値で未定義の場合は-1が入っています。
        /// </summary>
        private static int p_MCI_VolumeBGM = -1;
        /// <summary>
        /// 最後にＢＧＭ再生されたサウンドがループ再生する必要があるか（MCI_playBGMだけではループ再生不可なので、外部のメインスレッドなどがこれを感知してする用）
        /// </summary>
        private static bool p_MCI_isBGMLoop = false;
        /// <summary>
        /// 最後にＢＧＭ再生されたサウンドがループ再生する必要があるか（外部のメインスレッドなどに頼む用）
        /// </summary>
        public static bool MCI_isBGMLoop() { return p_MCI_isBGMLoop; }

        // 以下、効果音（ＳＥ）用
        /// <summary>
        /// 今MCIで再生中の効果音ファイルのフルパス。未再生の場合は""。MCI_playSE()で更新され、MCI_stopSE()で""に初期化される。
        /// ストップ中かどうかはこれだけじゃ厳密には確認できないので、ちゃんとMCI_getSoundMode_SE()を呼び出して確認して。
        /// </summary>
        private static string p_MCI_nowPlayingSEName_FullPath = "";
        /// <summary>
        /// （プログラム上の理論的に）同時再生可能なＳＥの最大チャンネル数+1です。（+1はint[]やbool[]の配列初期化に使うため）
        /// ※現在は多めに32チャンネル用意していますが、複数の効果音を何個まで同時に鳴らせるかは環境により異なります。
        /// </summary>
        public static int p_MCI_SEChannel_MAXplus1 = 32+1;
        /// <summary>
        /// 効果音ＳＥ[チャンネル番号1～p_MCI_SEChannel_MAXplus1-1]の音量です（新しくＳＥをロードしても引き継ぎます）。
        /// [0]は全チャンネルの音量に掛け算する、SEマスタ音量です。
        /// MCI_setVolume_SE()をすることで更新されます。初期値で未定義の場合は-1が入っています。
        /// </summary>
        private static int[] p_MCI_VolumeSE;
        /// <summary>
        /// 効果音ＳＥ[チャンネル番号1～p_MCI_SEChannel_MAXplus1-1]が現在再生されているかを示します。（再生時間による停止を検知していないので、stopSE()するまではtrueのままです）
        /// [0]は必ずfalseです。
        /// チャンネル番号は、ＳＥのエイリアス番号No（"my_se"+No）にも対応しています。
        /// MCI_stopSE()で全てfalseに初期化され、MCI_stopSE(p_MCI_nowSEAliasNo)で変更されます。
        /// </summary>
        private static bool[] p_MCI_isSEChannelNowUsing;
        /// <summary>
        /// 効果音ＳＥ[チャンネル番号1～p_MCI_SEChannel_MAXplus1-1]がループ再生かを示します。
        /// [0]は必ずfalseです。
        /// チャンネル番号は、ＳＥのエイリアス番号No（"my_se"+No）にも対応しています。
        /// MCI_stopSE()で全てfalseに初期化され、MCI_stopSE(p_MCI_nowSEAliasNo)で変更されます。
        /// </summary>
        private static bool[] p_MCI_isSELoop;
        /// <summary>
        /// 効果音ＳＥ[チャンネル番号1～p_MCI_SEChannel_MAXplus1-1]がループ再生する必要があるか（外部のメインスレッドなどに頼む用）
        /// </summary>
        public static bool MCI_isSELoop(int _Channel) { if (p_MCI_isSELoop != null) { return p_MCI_isSELoop[_Channel]; } else { p_MCI_isSELoop = new bool[p_MCI_SEChannel_MAXplus1]; return p_MCI_isSELoop[_Channel]; } }
        /// <summary>
        /// 最後に上書きしたＳＥチャンネル番号を格納する変数です。
        /// 全てのチャンネルが埋まった時、この「最後に上書きしたＳＥチャンネル番号+1」を上書きします。
        /// </summary>
        private static int p_MCI_SEChannelLastOverride = 1;

        /// <summary>
        /// 効果音再生用の指定チャンネルが使用中かどうかを返します。
        /// 引数に範囲外に値を入れたり、指定チャンネルがstopSE(_ChannelNo)されて解放されている場合や、まだ一度も使用されていない場合はfalseを返します。
        /// </summary>
        public static bool MCI_isSEUsing(int _ChannelNo){
            bool _isUsing = true;
            // チャンネルがnullだったら、初期化
            if (p_MCI_isSEChannelNowUsing == null)
            {
                p_MCI_isSEChannelNowUsing = new bool[p_MCI_SEChannel_MAXplus1]; // boolの初期値はfalse
            }
            _isUsing = MyTools.getArrayValue<bool>(p_MCI_isSEChannelNowUsing, _ChannelNo); // boolの初期値はfalse
            return _isUsing;
        }
        /// <summary>
        /// MediaControlInterfaceを使って、mp3/wav/mid/wmaファイルを効果音（ＳＥ）として再生します．
        /// MCI_plyaBGM()とはチャネル番号は干渉（競合）せず、ＢＧＭと同時に効果音を鳴らせます。
        /// SEのチャンネル番号は、再生中でないチャンネルを自動的に探して再生し、いっぱいになった時は「最後に上書きしたＳＥチャンネル番号+1」を上書きします。
        /// ※ただし、複数の効果音を何個まで同時に鳴らせるかは環境により異なります。
        /// 引数２で、ループの有無を設定できます。
        /// 
        /// 　　　　　　　　　　
        /// 理論的に再生できたら再生チャンネル番号を返し（これが送られても再生できない原因不明なエラーは多々ありますが）、
        /// 理論的に再生不可能な場合は-1を返します。
        /// </summary>
        public static int MCI_playSE(string _fileName_FullPath, bool _isRepeat)
        {
            return MCI_playSE(_fileName_FullPath, _isRepeat, false, 0);
        }
        /// <summary>
        /// MediaControlInterfaceを使って、mp3/wav/mid/wmaファイルを効果音（ＳＥ）として再生します．
        /// MCI_plyaBGM()とはチャネル番号は干渉（競合）せず、ＢＧＭと同時に効果音を鳴らせます。
        /// 引数２で、ループの有無を設定できます。
        /// 引数３で、他のチャンネルのSEを全て停止するかを設定できます。
        /// 引数４で、SEのチャンネル番号を設定でき、同じチャンネル番号を使うと上書き再生します。
        /// ※ただし、複数の効果音を何個まで同時に鳴らせるかは環境により異なります。
        /// 
        /// 　　　　　　　　　　同じチャンネルの同じファイルの場合は、最初から再生しなおします．
        /// 理論的に再生できたら再生チャンネル番号を返し（これが送られても再生できない原因不明なエラーは多々ありますが）、
        /// 理論的に再生不可能な場合は-1を返します。
        /// </summary>
        public static int MCI_playSE(string _fileName_FullPath, bool _isRepeat, bool _isStopOtherSEs, int _ChannelNo)
        {
            // 右に書いてあるのは嘘かも。もしこのメソッドだけで複数再生したかったら、ちゃんとstaticでないメソッド作って要検証。        /// （スレッドにして、2回呼び出すと、同時に2ファイルだけ鳴らせるようです。しかし、3ファイル目を鳴らすと、2番目のファイルが上書きされます。また、同じファイルを呼び出しても初期化されず、1番目のファイルの再生が継続されます。）

            // チャンネル0は、空いているチャンネルを自動取得
            if (_ChannelNo == 0)
            {
                // チャンネルがnullだったら、初期化
                if (p_MCI_isSEChannelNowUsing == null)
                {
                    p_MCI_isSEChannelNowUsing = new bool[p_MCI_SEChannel_MAXplus1]; // boolの初期値はfalse
                }
                _ChannelNo = 0; // メインはチャンネル1～だから、まず+1すること。
                bool _isUsing = true;
                while (_isUsing == true)
                {
                    _ChannelNo++; // メインはチャンネル1～
                    _isUsing = p_MCI_isSEChannelNowUsing[_ChannelNo];
                    // チャンネルが全て埋まっている場合は、「最後に上書きしたＳＥチャンネル番号+1」を上書きする
                    if (_ChannelNo >= p_MCI_SEChannel_MAXplus1 - 1)
                    {
                        _ChannelNo = p_MCI_SEChannelLastOverride + 1;
                        p_MCI_isSEChannelNowUsing[_ChannelNo] = false;
                        // 上書きしたチャンネルは、空いているとする
                        _isUsing = false;
                        // 最後に上書きしたＳＥチャンネル番号を更新（1つずらす）
                        p_MCI_SEChannelLastOverride++;
                        if (p_MCI_SEChannelLastOverride >= p_MCI_SEChannel_MAXplus1 - 1) p_MCI_SEChannelLastOverride = 1;
                        break;
                    }
                }
                // 効果音同時再生数の目安値を出力（デバッグ用）
                // 前から数えてるし、効果音の停止を判定していないので、あくまで目安。ConsoleWriteLine("MCI_playSE: 効果音 同時再生数:"+_ChannelNo);
                // チャンネルを使用中にする（以下の処理でやっている）
                //p_MCI_isSEChannelNowUsing[_ChannelNo] = true;
            }
            int _playingChannelNo = -1;
            bool _canPlay = true;
            if (isExist(_fileName_FullPath) == false)
            {
                ConsoleWriteLine("MCI_playSE: ※エラー：　下記の効果音ファイルは存在しないか、アクセス不許可です。 ファイル名 :" + _fileName_FullPath);
                _canPlay = false;
                return _playingChannelNo;
            }

            //再生するファイル名
            string fileName = _fileName_FullPath;
            string _format = getFileDottName(fileName);
            // ショートネームでやった方が失敗が少ない？
            fileName = MySound_Windows.MCI_getShortPathName_ByWindowsAPI(_fileName_FullPath);
            if (_format == "")
            {
                ConsoleWriteLine("MCI_playSE: ※エラー：　下記の効果音ファイルの名前に拡張子が付いていません。 ファイル名 :" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath));
                _canPlay = false;
                return _playingChannelNo;
            }
            else if (_format == "mp3" || _format == "wav" || _format == "mid" || _format == "wma")
            {

                // ●（とりあえず）SEの場合は、ファイルオープンにエラー覚悟で、複数のファイルを同時に開く。
                // 効果音は、my_bgmというエイリアス名は使わず、my_se*（*は半角数字）というエイリアス名を使う。
                if (_isStopOtherSEs == true)
                {
                    // 他の効果音をクローズ
                    ConsoleWriteLine("MCI_playSE: close：　他の効果音「" + getFileName_TheMostRightFileOrDirectory(p_MCI_nowPlayingSEName_FullPath) + "」などをcloseして、新しいファイル「" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath) + "」を開きます。 : " + _fileName_FullPath);
                    MCI_stopSE();
                }

                string command;
                string _aliasName = "my_se" + _ChannelNo;

                //ファイルを開く
                command = "open \"" + fileName + "\" alias "+ _aliasName;
                // ※mp3の場合，command（コマンド）はこのほうがよい？
                if (_format == "mp3" || _format == "MP3")
                {
                    command = "open \"" + fileName + "\" type mpegvideo alias " + _aliasName;
                }
                if (MCISendString(command, null, 0, IntPtr.Zero) != 0)
                {
                    // ここで失敗したら、チャンネルが空いていないのかもしれない（詳しくは不明）。
                    // ■上書き
                    // 前のファイルが再生中であっても、新しいファイルに上書きして再生
                    // ファイルをクローズ（stop→close）して、もう一度openをトライ
                    ConsoleWriteLine("MCI_playSE: 上書きopen：　前の効果音「" + getFileName_TheMostRightFileOrDirectory(p_MCI_nowPlayingSEName_FullPath) + "」をcloseして、新しいファイル「" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath) + "」を開きます。 : " + _fileName_FullPath);
                    // クローズ
                    MCI_stopSE(_ChannelNo);
                    // オープン
                    if (DEBUGShown_MCISendString(command, null, 0, IntPtr.Zero) != 0)
                    {
                        ConsoleWriteLine("MCI_playSE: 上書きopen失敗：　※エラー：　新しい効果音ファイルをopenできませんでした（原因はよくわかりません）。 新しいファイル: " + _fileName_FullPath);
                        _canPlay = false;
                    }
                }
                // チャンネルを使用中にする
                if (p_MCI_isSEChannelNowUsing == null)
                {
                    p_MCI_isSEChannelNowUsing = new bool[p_MCI_SEChannel_MAXplus1]; // boolの初期値はfalse
                }
                p_MCI_isSEChannelNowUsing[_ChannelNo] = true;
                // ループを設定する
                if (p_MCI_isSELoop == null)
                {
                    p_MCI_isSELoop = new bool[p_MCI_SEChannel_MAXplus1]; // boolの初期値はfalse
                }
                p_MCI_isSELoop[_ChannelNo] = _isRepeat;
                // ■前のボリュームの適応（これを呼び出さないと、毎回最大音量で流れてしまう）
                if (p_MCI_VolumeSE == null)
                {
                    MCI_setVolume_SEMaster(1000);
                }
                int _beforeVolume = MCI_setVolume_SE(p_MCI_VolumeSE[_ChannelNo], _ChannelNo);
                // 最初から再生
                command = "play "+_aliasName+" from 0"; //command = "play エイリアス名"; だと、stopしてもfrom0をつけないと途中から再生されてしまう。
                //再生する
                if (MCISendString(command, null, 0, IntPtr.Zero) == 0)
                {
                    ConsoleWriteLine("MCI_playSE: サウンド再生成功: 下記の効果音ファイルを再生。（理論的には成功したはずです） : " + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath));
                    p_MCI_nowPlayingSEName_FullPath = _fileName_FullPath;
                }
                else
                {
                    ConsoleWriteLine("MCI_playSE: ※エラー：　効果音再生に失敗しました（原因はよくわかりません）。ファイル名 : " + getFileName_TheMostRightFileOrDirectory(fileName));
                    _canPlay = false;
                }
            }
            else
            {
                ConsoleWriteLine("MCI_playSE: ※エラー：　下記の効果音ファイルの名前に拡張子が、MCIに対応していません。ファイル名 :" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath));
                _canPlay = false;
            }
            // 理論的に再生出来たら、-1ではなく、再生チャンネル番号を返す。
            if (_canPlay == true)
            {
                _playingChannelNo = _ChannelNo;
            }
            return _playingChannelNo;
        }
        /// <summary>
        /// 再生中の効果音ファイルを全て停止します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_stopSE()
        {
            int _stopResult = -1;
            int _No;
            for (_No = 1; _No <= p_MCI_SEChannel_MAXplus1 - 1; _No++)
            {
                _stopResult = MCI_stopSE(_No);
            }
            return _stopResult;
        }
        /// <summary>
        /// 再生中の効果音ファイルを全て停止します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_stopSE(int _ChannelNo)
        {
            int _stopResult = -1;
            int _closeResult = -1;
            string command;
            string _aliasName = "my_se" + _ChannelNo;
            // 再生しているサウンドの音量を覚えておく（閉じるとボリュームもリセットされてしまうため）
            if (p_MCI_VolumeSE == null)
            {
                MCI_setVolume_SEMaster(1000);
            }
            p_MCI_VolumeSE[_ChannelNo] = MCI_getVolume_SE(_ChannelNo);
            //再生しているサウンドを停止する
            command = "stop " + _aliasName;
            _stopResult = MCISendString(command, null, 0, IntPtr.Zero);
            //openしたファイルを閉じ、リソースを解放する（これで次、新しいファイルがopenできる）
            command = "close " + _aliasName;
            _closeResult = DEBUGShown_MCISendString(command, null, 0, IntPtr.Zero);

            // プロパティの初期化
            p_MCI_nowPlayingSEName_FullPath = "";
            // チャンネルを空ける
            if (p_MCI_isSEChannelNowUsing == null)
            {
                p_MCI_isSEChannelNowUsing = new bool[p_MCI_SEChannel_MAXplus1]; // boolの初期値はfalse
            }
            p_MCI_isSEChannelNowUsing[_ChannelNo] = false;

            if (_closeResult != 0)
            {
                _closeResult = -1; // 他のエラーが出ても、失敗は-1とする
            }
            return _closeResult; // ファイルが正常に閉じるまでを成功とする
        }
        /// <summary>
        /// 再生中の効果音ファイルを全て一時停止します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_pauseSE()
        {
            int _stopResult = -1;
            int _No;
            for (_No = 1; _No <= p_MCI_SEChannel_MAXplus1; _No++)
            {
                _stopResult = MCI_pauseSE(_No);
            }
            return _stopResult;
        }
        /// <summary>
        /// 再生中のＳＥを一時停止します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_pauseSE(int _ChannelNo)
        {
            string command;
            //再生しているサウンドを一時停止する
            string _aliasName = "my_se" + _ChannelNo;
            command = "pause "+_aliasName;
            int _result = MCISendString(command, null, 0, IntPtr.Zero);
            if (_result == 0)
            {
                ConsoleWriteLine("MCI_pauseSE: チャンネル"+_ChannelNo+" を一時停止（pause）中。");
            }
            else
            {
                ConsoleWriteLine("MCI_pauseSE: チャンネル"+_ChannelNo+" 失敗(" + _result + ")");
                _result = -1;
            }
            return _result;
        }
        /// <summary>
        /// 再生中の効果音ファイルを全て一時停止から再開します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_resumeSE()
        {
            int _stopResult = -1;
            int _No;
            for (_No = 1; _No <= p_MCI_SEChannel_MAXplus1; _No++)
            {
                _stopResult = MCI_resumeSE(_No);
            }
            return _stopResult;
        }
        /// <summary>
        /// 一時停止中のＳＥファイルを一時停止から再開(resume)します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_resumeSE(int _ChannelNo)
        {
            string command;
            string _aliasName = "my_se" + _ChannelNo;
            command = "resume "+_aliasName;
            int _result = MCISendString(command, null, 0, IntPtr.Zero);
            if (_result == 0)
            {
                ConsoleWriteLine("MCI_resumeSE: チャンネル" + _ChannelNo + " を再開（resume）。");
            }
            else
            {
                ConsoleWriteLine("MCI_resumeSE: チャンネル" + _ChannelNo + " 失敗(" + _result + ")");
                _result = -1;
            }
            return _result;
        }
        /// <summary>
        /// MCIで現在再生中の効果音[チャンネル番号]があれば、引数２の経過時間ミリ秒の場所に移動（seek）します。
        /// 引数２に0を入れると最初から再生し、十分大きな値を入れると最後の位置（再生終了）に移動します。
        /// 成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        public static int MCI_seekSE(int _ChannelNo, int _PassedMSec)
        {
            string command;
            string _aliasName = "my_se" + _ChannelNo;
            string _position = "start"; // 移動位置には0以上の値の他、startやendも指定できる。
            _position = _PassedMSec.ToString();
            //再生位置を移動する
            command = "seek "+_aliasName+" to " + _position;
            if (DEBUGShown_MCISendString(command, null, 0, IntPtr.Zero) == 0)
            {
                return 0;
            }
            else
            {
                // 失敗したらも、play試してみる？
                command = "play " + _aliasName + " from "+_position; //command = "play エイリアス名"; だと、stopしてもfrom0をつけないと途中から再生されてしまう。
                //再生する
                if (DEBUGShown_MCISendString(command, null, 0, IntPtr.Zero) == 0)
                {
                    return 0;
                }
                else
                {
                    ConsoleWriteLine("MCI_seek: 「seek"+_aliasName+" to "+_position+"」でも「play"+_aliasName+" from "+_position+"」でも再生位置移動できませんでした。");
                }
                // 失敗したら、位置をendにする？
                //command = "seek my_bgm to end";
                //MCISendString(command, null, 0, IntPtr.Zero);
                return -1;
            }
        }
        /// <summary>
        /// MCIで現在効果音[チャンネル番号]が再生終了していればtrueを返します。
        /// 再生中や、ポーズ中の場合は再開される可能性があるため、falseを返します。
        /// </summary>
        /// <returns></returns>
        public static bool MCI_isEndSE(int _ChannelNo)
        {
            bool _isPlaying = false;
            if (MCI_getSoundMode_SE(_ChannelNo) == "stopped")
            {
                _isPlaying = true;
            }
            return _isPlaying;
        }
        /// <summary>
        /// 現在のサウンドのモード（再生／停止／一時停止中か）を取得
        /// 
        /// ※返り値: 再生中          = "playing"
        ///           停止中/再生終了 = "stopped"
        ///           一時停止中      = "paused"
        ///           失敗            = ""
        /// </summary>
        /// <returns></returns>
        public static string MCI_getSoundMode_SE(int _ChannelNo)
        {
            string _aliasName = "my_se" + _ChannelNo;
            StringBuilder sb = new StringBuilder(32);  // MCISendString() の Return String 格納用
            DEBUGShown_MCISendString("status "+_aliasName+" mode", sb, sb.Capacity, IntPtr.Zero);
            string _result = sb.ToString();
            return _result;
        }
        /// <summary>
        /// 効果音[チャンネル番号]のファイルの経過時間を msec単位 で返す
        /// </summary>
        /// <returns></returns>
        public static int MCI_getSEPassedMSec(int _ChannelNo)
        {
            string _aliasName = "my_se"+_ChannelNo;

            StringBuilder sb = new StringBuilder(32);  // MCISendString() の Return String 格納用
            DEBUGShown_MCISendString("status "+_aliasName+" position", sb, sb.Capacity, IntPtr.Zero);
            int _PassedMSec = int.Parse(sb.ToString());
            return _PassedMSec;
        }
        /// <summary>
        /// 効果音[チャンネル番号]のファイルの曲の長さを msec単位 で返す
        /// </summary>
        /// <returns></returns>
        public int MCI_getSELengthMSec(int _ChannelNo)
        {
            string _aliasName = "my_se"+_ChannelNo;
            StringBuilder sb = new StringBuilder(32);  // MCISendString() の Return String 格納用
            DEBUGShown_MCISendString("status "+_aliasName+" length", sb, sb.Capacity, IntPtr.Zero);
            int _LengthMSec = int.Parse(sb.ToString());
            return _LengthMSec;
        }
        /// <summary>
        /// MCIのSEマスターボリューム（p_MCI_VolumeSE[0]）を取得します。取得できなかった場合は初期値1000を返します。
        /// ※なお、実際に再生されるSEの音量は、「getVolume_Master() * MCI_getVolume_SEMaster() *MCI_getVolume_SE(各チャンネル番号)」 です。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        public static int MCI_getVolume_SEMaster()
        {
            if (p_MCI_VolumeSE == null)
            {
                MCI_setVolume_SEMaster(1000);
            }
            return p_MCI_VolumeSE[0];
        }
        /// <summary>
        /// MCIの指定チャンネルの効果音ボリュームを取得します。
        /// 取得できなかった場合は_ChannelNo=0なら1000、それ以外なら中間値500を返します。
        /// ※なお、実際に再生されるSEの音量は、「getVolume_Master() * MCI_getVolume_SEMaster() * 各チャンネルの音量」 です。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        public static int MCI_getVolume_SE(int _ChannelNo)
        {
            // 失敗したとき、0を返すと鳴らない場合があるので、最大値500を返すことにしている
            int _volume_0to_1000 = 500; // 取れなかった場合
            // ※本当によく失敗するので、できれば前回setValumeで設定した値を使った方がいい。
            // もし前回setVolumeで設定された値が一度でもあれば（-1でなければ）、これを代入
            if (p_MCI_VolumeSE != null)
            {
                _volume_0to_1000 = p_MCI_VolumeSE[_ChannelNo];
            }
            return _volume_0to_1000;

            // 以下、MCIを使って実際の音量を取る方法だが、本当によく失敗するので、現在は使っていない。
            ////string command = "";
            //string _aliasName = "my_se"+_ChannelNo;

            //// 一度もopenされてなくて、my_bgmがまだエイリアス定義されてないので、その場合は0を返す
            //StringBuilder _buff = new StringBuilder(4);
            //int _buffSize = _buff.Capacity;
            //int _result = MCISendString("status "+_aliasName+" volume", _buff, _buffSize, IntPtr.Zero);
            //if (_result == 0) // _resut = 261～8は、my_bgmが定義されていない場合（一度もファイルが呼び出されていない場合）に返る値？
            //{
            //    // ==0だと成功
            //    //bool _isSuccess = true;
            //    _volume_0to_1000 = MyTools.parseInt(_buff.ToString());
            //    // ※なお、実際に再生されるSEの音量は、「getVolume_Master() * MCI_getVolume_SEMaster() * 各チャンネルの音量」 です。
            //    _volume_0to_1000 = _volume_0to_1000 / MCI_getVolume_SEMaster();
            //}
            //else
            //{
            //    // ※本当によく失敗するので、できれば前回setValumeで設定した値を使った方がいい。
            //    // もし前回setVolumeで設定された値が一度でもあれば（-1でなければ）、これを代入
            //    if (p_MCI_VolumeSE != null)
            //    {
            //        _volume_0to_1000 = p_MCI_VolumeSE[_ChannelNo];
            //    }
            //}
            //return _volume_0to_1000;
        }
        /// <summary>
        /// MCIのSEマスターボリューム（p_MCI_VolumeSE[0]）を設定します。以前設定されていたボリュームを返します。
        /// 設定されていない場合は1000を返します。
        /// ※なお、実際に再生されるSEの音量は、「getVolume_Master() * MCI_getVolume_SEMaster() * 各チャンネルの音量」 です。
        /// 
        ///             // MCIの音量調節については、WMA、MP3ファイルでは動作しましたが、WAVE、MIDIファイルでは動作しませんでした。原因はまだよくわかっていません。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        public static int MCI_setVolume_SEMaster(int _volume_0to1000)
        {
            return MCI_setVolume_SE(_volume_0to1000, 0);
        }
        /// <summary>
        /// MCIのサウンドボリュームを調整します。以前設定されていたボリュームを返します。
        /// 設定されていない場合は_ChannelNo=0なら1000、それ以外なら中間値500を返します。
        /// ※なお、実際に再生されるSEの音量は、「getVolume_Master() * MCI_getVolume_SEMaster() * 各チャンネルの音量」 です。
        /// 
        ///             // MCIの音量調節については、WMA、MP3ファイルでは動作しましたが、WAVE、MIDIファイルでは動作しませんでした。原因はまだよくわかっていません。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        public static int MCI_setVolume_SE(int _volume_0to1000, int _ChannelNo)
        {
            if (p_MCI_VolumeSE == null)
            {
                p_MCI_VolumeSE = new int[p_MCI_SEChannel_MAXplus1];
                p_MCI_VolumeSE[0] = 1000; // SEマスターボリュームの初期値は1000
                for (int i = 1; i < p_MCI_VolumeSE.Length; i++)
                {
                    // その他のチャンネルの初期値は中間値500。
                    p_MCI_VolumeSE[i] = 500;
                }
            }
            // 今回setVolumeした値を、プロパティの更新
            p_MCI_VolumeSE[_ChannelNo] = _volume_0to1000;

            int _beforeVolume = MCI_getVolume_SE(_ChannelNo);
            //string command = "";
            string _aliasName = "my_se"+_ChannelNo;

            if (_volume_0to1000 < 0) _volume_0to1000 = 0;
            if (_volume_0to1000 > 1000) _volume_0to1000 = 1000;
            // ※なお、実際に再生されるSEの音量は、「getVolume_Master() * MCI_getVolume_SEMaster() * 各チャンネルの音量」 です。
            int _MCIVolume = MCI_getVolume_SEMaster() * _volume_0to1000; // getVolume_Master()はここでかけなくてもデバイス側でされる
            StringBuilder _buff = new StringBuilder(4);
            _buff.Append(_MCIVolume);
            int _buffSize = _buff.Capacity;
            // http://bbs.wankuma.com/index.cgi?mode=al2&namber=11021&KLOG=24
            // http://www.bekkoame.ne.jp/i/mr.manri/MCI/
            // 音量調節については、WMA、MP3形式のファイルでは動作しましたが、WAVE、MIDI形式のファイルでは動作しませんでした。現在、研究中です。
            int _result = MCISendString("setaudio "+_aliasName+" volume to " + _volume_0to1000, null, 0, IntPtr.Zero);
            if (_result == 0) // _resut = 261は、my_bgmが定義されていない場合（一度もファイルが呼び出されていない場合）に返る値？
            {
                // 成功
                //bool _isSuccess = true;
            }
            // (メモ)右だけ、左だけ調節したい場合
            //MCISendString("setaudio my_bgm right volume to 100", null, 0, IntPtr.Zero);


            return _beforeVolume;
        }

        // 以下、BGM専用
        /// <summary>
        /// MediaControlInterfaceを使って、mp3/wav/mid/wmaファイルをＢＧＭ（音楽）として再生します．
        /// 前に再生されていたＢＧＭは停止します。
        /// 同じファイルの場合は最初から再生しなおします．
        /// 理論的に再生できたらtrue（これがtrueでも再生できない原因不明なエラーは多々ありますが）、
        /// 理論的に再生不可能な場合はfalseを返します。
        /// （※このメソッドでは、一時停止したものを再開できません。再開したい場合は、MCI_resumeBGM()メソッドを使ってください）
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static bool MCI_playBGM(string _fileName_FullPath, bool _isRepeat)
        {
            // ●テスト
            p_MCI_nowPlayingBGMName_FullPath = _fileName_FullPath;

            bool _canPlay = true;
            // ファイルが存在しなければ終了
            if (isExist(_fileName_FullPath) == false)
            {
                ConsoleWriteLine("MCI_playBGM: ※エラー：　下記の音楽ファイルは存在しないか、アクセス不許可です。 音楽ファイル名 :" + _fileName_FullPath);
                _canPlay = false;
                return _canPlay;
            }

            //再生するファイル名
            string fileName = _fileName_FullPath;
            string _format = getFileDottName(fileName);
            // ショートネームでやった方が失敗が少ない？
            fileName = MySound_Windows.MCI_getShortPathName_ByWindowsAPI(_fileName_FullPath);
            if (_format == "")
            {
                ConsoleWriteLine("MCI_playBGM: ※エラー：　下記の音楽ファイルの名前に拡張子が付いていません。 音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath));
                _canPlay = false;
            }
            else if (_format == "mp3" || _format == "wav" || _format == "mid" || _format == "wma")
            {

                string command;

                // ●（とりあえず）ファイルオープンにエラーがなくても、複数のファイルを同時開いてしまったら、
                // my_bgmが上書きされて古いファイルのクローズが出来なくなって困るから、とりあえずクローズ
                // 音楽（ＢＧＭ）は、my_bgmというエイリアス名を使う
                MCI_stopBGM();

                //ファイルを開く
                command = "open \"" + fileName + "\" alias my_bgm";
                // ※mp3の場合，command（コマンド）はこのほうがよい？
                if (_format == "mp3" || _format == "MP3")
                {
                    command = "open \"" + fileName + "\" type mpegvideo alias my_bgm";
                }
                //ConsoleWriteLine("MCI_playBGM: ロードトライ: " + command);
                int _result = MCISendString(command, null, 0, IntPtr.Zero);
                if (_result == 0)
                {
                    ConsoleWriteLine("MCI_playBGM: ロード成功(0): "+command);
                }
                else
                {
                    ConsoleWriteLine("MCI_playBGM: ロード失敗("+_result+"): " + command);

                    // オープンに失敗した理由を究明
                    // 基本は、チャネルが空いてない（既に前のmy_bgmのファイルがopenされてcloseされていない）ので、続けてopenできないことが多い。
                    //      それを段階的に対処。

                    // 1.既に現在再生中のファイルがなければ、もう一度closeして再openする
                    //      現在再生しているファイルがあるかどうかをチェック（ポーズ中のものはfalseになる）
                    string _beforeBGMSoundMode = MCI_getSoundMode_BGM();
                    if (_beforeBGMSoundMode != "playing")
                    {
                        // 再生中ではないということは、ポーズ中、ストップ中のどちらかである。
                        // ループ再生がＯＦＦで、再生が終了している（ストップ中）かどうかをチェック
                        if (_beforeBGMSoundMode == "stopped")
                        {
                            // 上書きopen:チャンネルファイルをクローズ（stop→close）して、もう一度openをトライ
                            ConsoleWriteLine("MCI_playBGM: 前のファイル「" + getFileName_TheMostRightFileOrDirectory(p_MCI_nowPlayingBGMName_FullPath) + "」は再生終了しているので、新しいファイル「" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath) + "」を開きます。 : " + _fileName_FullPath);
                            // クローズ
                            MCI_stopBGM();
                            // オープン
                            if (DEBUGShown_MCISendString(command, null, 0, IntPtr.Zero) != 0)
                            {
                                // ここで失敗したら、原因はよくわからない
                                ConsoleWriteLine("MCI_playBGM: open失敗：　※エラー：　新しいファイルをopenできませんでした（原因はよくわかりません）。 新しいファイル: " + _fileName_FullPath);
                                _canPlay = false;
                                return _canPlay;
                            }
                        }
                        else
                        {
                            // 再生中のファイルがないのに、チャンネルが空いてないので、ポーズ中の可能性が高い。
                            if (_beforeBGMSoundMode == "paused")
                            {
                                // ポーズを使うということは、この曲はResumeでまた途中から再生したいという意味があるので、
                                // できれば消したくない
                                ConsoleWriteLine("MCI_playBGM: pause中 ※警告：　新しい音楽ファイル" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath) + "を再生しませんでした。（前のファイルがポーズ中のため、Resumeするまでは待つ仕様になっています）。 前のファイル: " + getFileName_TheMostRightFileOrDirectory(p_MCI_nowPlayingBGMName_FullPath));
                                _canPlay = false;
                                return _canPlay;
                            }
                            else
                            {
                                // 再生中でも、ストップ中でも、ポーズ中でもない。原因がよくわからない。

                                // モードを取得できていないエラー。この時点でもうお手上げか…。
                                // MCI_getSoundMode_BGM()==""で、ちゃんと機能していない可能性が高い。
                                // openしたスレッドと別のスレッドで触ろうとしたのかも。

                                // とりあえず、チャンネルファイルをクローズ（stop→close）して、もう一度openをトライ
                                ConsoleWriteLine("MCI_playBGM: 上書きopen: openに失敗した原因は不明ですが、一度前のファイル「" + getFileName_TheMostRightFileOrDirectory(p_MCI_nowPlayingBGMName_FullPath) + "」を閉じて、新しいファイル「" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath) + "」を開くをトライします。 : " + _fileName_FullPath);
                                // クローズ
                                MCI_stopBGM();
                                // オープン
                                if (DEBUGShown_MCISendString(command, null, 0, IntPtr.Zero) != 0)
                                {
                                    // ここで失敗したら、原因はよくわからない
                                    ConsoleWriteLine("MCI_playBGM: 上書きopenエラー：　新しいファイルをopen失敗。原因はよくわかりません。※openしたスレッドと別のスレッドで触ろうとしたのかもしれません。SoundMode=\"" + _beforeBGMSoundMode + "\"、新しいファイル: " + _fileName_FullPath);
                                    _canPlay = false;
                                    return _canPlay;
                                }
                            }
                        }
                    }
                    // 2.現在再生中のファイルがあるなら、まず既に同じファイルを開いてる可能性を考える。
                    //      現在再生しているファイルと、同じファイルかどうか調べる
                    else
                    {
                        if (p_MCI_nowPlayingBGMName_FullPath == _fileName_FullPath)
                        {
                            // 同じなら、再生しているサウンドを停止して、同じサウンドをもう一回最初から再生
                            MCISendString("stop my_bgm", null, 0, IntPtr.Zero);
                            // openはしなくていい
                        }
                        // 3.同じでないなら、既に違うファイルを開いていて、それが再生中である可能性が高い。
                        else
                        {
                            // ■上書き（他のチャンネルが空いていれば他のチャンネルで任せたいが、MCIのＢＧＭは１チャンネルしかないという前提で書いているので、仕方がない）

                            // 前のファイルが再生中であっても、新しいファイルに上書きして再生
                            // ファイルをクローズ（stop→close）して、もう一度openをトライ
                            ConsoleWriteLine("MCI_playBGM: 上書きopen：　前のファイル「" + getFileName_TheMostRightFileOrDirectory(p_MCI_nowPlayingBGMName_FullPath) + "」をcloseして、新しいファイル「" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath) + "」を開きます。 : " + _fileName_FullPath);
                            // クローズ
                            MCI_stopBGM();
                            // オープン
                            if (DEBUGShown_MCISendString(command, null, 0, IntPtr.Zero) != 0)
                            {
                                ConsoleWriteLine("MCI_playBGM: 上書きopen失敗：　※エラー：　新しいファイルをopenできませんでした（原因はよくわかりません）。 新しいファイル: " + _fileName_FullPath);
                                _canPlay = false;
                                return _canPlay;
                            }
                        }
                    }// エラーの原因究明、終わり。
                }
                // ファイルオープン、終わり

                // ■前のボリュームの適応（これを呼び出さないと、毎回最大音量で流れてしまう）
                if (p_MCI_VolumeBGM == -1)
                {
                    p_MCI_VolumeBGM = 500; // 未定義なら、中間値500を割り当てる
                }
                int _beforeVolume = MCI_setVolume_BGM(p_MCI_VolumeBGM);
                // 最初から再生
                command = "play my_bgm from 0"; //command = "play my_bgm"; だと、stopしてもfrom0をつけないと途中から再生されてしまう。
                // リピートの設定
                if (_isRepeat == true)
                {
                    // ループフラグを立てて、メインスレッドでどこかで終了を感知したら再度再生しないといけないみたい…
                    p_MCI_isBGMLoop = true;
                    //command = "play my_bgm from 0 repeat"; // repeatをつけてもダメらしい…スレッドかウィンドウメッセージで取らないとループ再生は難しい…
                }
                else
                {
                    p_MCI_isBGMLoop = false;
                }
                //再生する
                if (MCISendString(command, null, 0, IntPtr.Zero) == 0)
                {
                    ConsoleWriteLine("MCI_playBGM: サウンド再生成功: 下記の音楽ファイルを再生。（理論的には成功したはずです） : " + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath));
                    p_MCI_nowPlayingBGMName_FullPath = _fileName_FullPath;
                }
                else
                {
                    ConsoleWriteLine("MCI_playBGM: ※エラー：　音楽再生に失敗しました（原因はよくわかりません）。: 下記の音楽ファイル : " + getFileName_TheMostRightFileOrDirectory(fileName));
                }

            }
            else
            {
                ConsoleWriteLine("MCI_playBGM: ※エラー：　下記の音楽ファイルの名前に拡張子が、MCIに対応していません。 音楽ファイル名 :" + getFileName_TheMostRightFileOrDirectory(_fileName_FullPath));
                _canPlay = false;
            }
            return _canPlay;
        }
        /// <summary>
        /// 再生中の音楽ファイルを停止します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_stopBGM()
        {
            int _stopResult = -1;
            int _closeResult = -1;
            string command;
            // 再生しているサウンドの音量を覚えておく（閉じるとボリュームもリセットされてしまうため）
            p_MCI_VolumeBGM = MCI_getVolume_BGM();
            //再生しているサウンドを停止する
            command = "stop my_bgm";
            _stopResult = MCISendString(command, null, 0, IntPtr.Zero);
            //openしたファイルを閉じ、リソースを解放する（これで次、新しいファイルをopenする場合、上書きしなくて済む）
            command = "close my_bgm";
            _closeResult = DEBUGShown_MCISendString(command, null, 0, IntPtr.Zero);
            //p_MCI_nowPlayingBGMName_FullPath = "";
            if (_closeResult != 0)
            {
                _closeResult = -1; // 失敗した場合の戻り値は-1
            }
            return _closeResult; // ファイルが正常に閉じるまでを成功とする
        }
        /// <summary>
        /// MCIで現在ＢＧＭが再生終了していればtrueを返します。再生中や、ポーズ中の場合は再開される可能性があるため、falseを返します。
        /// </summary>
        /// <returns></returns>
        public static bool MCI_isEndBGM()
        {
            bool _isPlaying = false;
            if (MCI_getSoundMode_BGM() == "stopped")
            {
                _isPlaying = true;
            }
            return _isPlaying;
        }
        /// <summary>
        /// MCIで現在再生中のＢＧＭサウンドファイルがあれば、そのフルパスを返します。なければ、""が返ります。
        /// ※MCIコマンドを使わずp_MCI_nowPlayingBGMName_FullPathの値を取得するだけので、高速です。
        /// </summary>
        /// <returns></returns>
        public static string MCI_getPlayingBGMName_FullPath(){
            return p_MCI_nowPlayingBGMName_FullPath;
        }
        /// <summary>
        /// MCIで現在再生中のＢＧＭファイルがあれば、引数の経過時間ミリ秒の場所に移動（seek）します。
        /// 引数に0を入れると最初から再生し、十分大きな値を入れると最後の位置（再生終了）に移動します。
        /// 成功した場合は0、失敗した場合（現在再生中のＢＧＭがない場合も）は-1が返ります。
        /// </summary>
        public static int MCI_seekBGM(int _PassedMSec)
        {
            string command;
            string _aliasName = "my_bgm";
            string _position = "start"; // 移動位置には0以上の値の他、startやendも指定できる。
            _position = _PassedMSec.ToString();
            //再生位置を移動する
            command = "seek " + _aliasName + " to " + _position;
            int _result = MCISendString(command, null, 0, IntPtr.Zero);
            if (_result == 0)
            {
                return 0;
            }
            else
            {
                if (_result != p_MCISendStringReturn_MCIERR_INVALID_DEVICE_NAME && _result != p_MCISendStringReturn_MCIERR_CANNOT_LOAD_DRIVER)
                {
                    // 失敗したらも、play試してみる？
                    command = "play " + _aliasName + " from " + _position; //command = "play エイリアス名"; だと、stopしてもfrom0をつけないと途中から再生されてしまう。
                    //再生する
                    if (MCISendString(command, null, 0, IntPtr.Zero) == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        //ConsoleWriteLine("MCI_seekBGM: 「seek" + _aliasName + " to " + _position + "」でも「play" + _aliasName + " from " + _position + "」でも再生位置移動できませんでした。");
                    }
                }
                // 失敗したら、位置をendにする？
                //command = "seek my_bgm to end";
                //MCISendString(command, null, 0, IntPtr.Zero);
                return -1;
            }
        }
        /// <summary>
        /// 再生中のＢＧＭを一時停止します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_pauseBGM()
        {
            string command;
            //再生しているサウンドを一時停止する
            command = "pause my_bgm";
            int _result = MCISendString(command, null, 0, IntPtr.Zero);
            if (_result == 0)
            {
                ConsoleWriteLine("MCI_pauseBGM: 現在再生中のＢＧＭ「" + getFileName_TheMostRightFileOrDirectory(p_MCI_nowPlayingBGMName_FullPath) + "」を一時停止（pause）中。");
            }
            else
            {
                ConsoleWriteLine("MCI_pauseBGM: 失敗("+_result+")");
                _result = -1;
            }
            return _result;
        }
        /// <summary>
        /// 一時停止中のＢＧＭファイルを再開(resume)します．成功した場合は0、失敗した場合は-1が返ります。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static int MCI_resumeBGM()
        {
            string command;
            command = "resume my_bgm";
            int _result = MCISendString(command, null, 0, IntPtr.Zero);
            if (_result == 0)
            {
                ConsoleWriteLine("MCI_resumeBGM: 現在停止中のＢＧＭ「" + getFileName_TheMostRightFileOrDirectory(p_MCI_nowPlayingBGMName_FullPath) + "」を再開（resume）。");
            }
            else
            {
                ConsoleWriteLine("MCI_resumeBGM: 失敗(" + _result + ")");
                _result = -1;
            }
            return _result;
        }
        /// <summary>
        /// 現在のサウンドのモード（再生／停止／一時停止中か）を取得
        /// 
        /// ※返り値: 再生中          = "playing"
        ///           停止中/再生終了 = "stopped"
        ///           一時停止中      = "paused"
        ///           失敗した場合    = ""
        /// </summary>
        /// <returns></returns>
        public static string MCI_getSoundMode_BGM()
        {
            StringBuilder sb = new StringBuilder(32);  // MCISendString() の Return String 格納用
            int _ref = MCISendString("status my_bgm mode", sb, sb.Capacity, IntPtr.Zero);
            string _soundMode = sb.ToString();
            //if(_ref != 0) _soundMode = ""; // 自動的にこの値が入っている
            return _soundMode;
        }
        /// <summary>
        /// 再生中のファイルの経過時間を msec単位 で返す
        /// </summary>
        /// <returns></returns>
        public static int MCI_getBGMPassedMSec()
        {
            StringBuilder sb = new StringBuilder(32);  // MCISendString() の Return String 格納用
            MCISendString("status my_bgm position", sb, sb.Capacity, IntPtr.Zero);
            int _PassedMSec = int.Parse(sb.ToString());
            return _PassedMSec;
        }
        /// <summary>
        /// open 中のファイルの曲の長さを msec単位 で返す
        /// </summary>
        /// <returns></returns>
        public int MCI_getBGMLengthMSec()
        {
            StringBuilder sb = new StringBuilder(32);  // MCISendString() の Return String 格納用
            MCISendString("status my_bgm length", sb, sb.Capacity, IntPtr.Zero);
            int _LengthMSec = int.Parse(sb.ToString());
            return _LengthMSec;
        }
        /// <summary>
        /// MCIのサウンドボリュームを取得します。取得できなかった場合は中間値500を返します。
        /// ※MCIコマンドを使わずp_MCI_VolumeBGMの値を取得するだけので、高速です。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        public static int MCI_getVolume_BGM()
        {
            int _volume_0to_1000 = 500; // 取れなかった場合
            // ※本当によく失敗するので、できれば前回setValumeで設定した値を使った方がいい。
            // もし前回setVolumeで設定された値が一度でもあれば（-1でなければ）、これを代入
            if (p_MCI_VolumeBGM != -1)
            {
                _volume_0to_1000 = p_MCI_VolumeBGM;
            }
            return _volume_0to_1000;

            //// 音量取得だけにＭＣＩを使うのは勿体ないし、１フレーム20ミリ秒毎に連続して取ることが多いので、使わない。
            //// 失敗したとき、0を返すと鳴らない場合があるので、最大値500を返すことにしている
            //int _volume_0to_1000 = 500; // 取れなかった場合
            //string command = "status my_bgm volume";
            //// 一度もopenされてなくて、my_bgmがまだエイリアス定義されてないので、その場合は0を返す
            //StringBuilder _buff = new StringBuilder(4);
            //int _buffSize = _buff.Capacity;
            //int _result = MCISendString(command, _buff, _buffSize, IntPtr.Zero);
            //if (_result == 0) // _resut = 261～8は、my_bgmが定義されていない場合（一度もファイルが呼び出されていない場合）に返る値？
            //{
            //    // ==0だと成功
            //    //bool _isSuccess = true;
            //    _volume_0to_1000 = MyTools.parseInt(_buff.ToString());
            //}
            //else
            //{
            //    // ※本当によく失敗するので、できれば前回setValumeで設定した値を使った方がいい。
            //    // もし前回setVolumeで設定された値が一度でもあれば（-1でなければ）、これを代入
            //    if (p_MCI_VolumeBGM != -1)
            //    {
            //        _volume_0to_1000 = p_MCI_VolumeBGM;
            //    }
            //}
            return _volume_0to_1000;
        }
        /// <summary>
        /// MCIのサウンドボリュームを調整します。以前設定されていたボリュームを返します。
        /// 設定されていない場合は0を返します。
        /// 
        ///             // MCIの音量調節については、WMA、MP3ファイルでは動作しましたが、WAVE、MIDIファイルでは動作しませんでした。原因はまだよくわかっていません。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        public static int MCI_setVolume_BGM(int _volume_0to1000)
        {
            int _beforeVolume = MCI_getVolume_BGM();
            if(_volume_0to1000 < 0) _volume_0to1000 = 0;
            if(_volume_0to1000 > 1000) _volume_0to1000 = 1000;
            StringBuilder _buff = new StringBuilder(4);
            _buff.Append(_volume_0to1000);
            int _buffSize = _buff.Capacity;
            // http://bbs.wankuma.com/index.cgi?mode=al2&namber=11021&KLOG=24
            // http://www.bekkoame.ne.jp/i/mr.manri/MCI/
            string command = "setaudio my_bgm volume to " + _volume_0to1000;
            // 音量調節については、WMA、MP3形式のファイルでは動作しましたが、WAVE、MIDI形式のファイルでは動作しませんでした。現在、研究中です。
            int _result = MCISendString(command, null, 0, IntPtr.Zero);
            if (_result == 0) // _resut = 261は、my_bgmが定義されていない場合（一度もファイルが呼び出されていない場合）に返る値？
            {
                // 成功
                //bool _isSuccess = true;
            }
            // (メモ)右だけ、左だけ調節したい場合
            //MCISendString("setaudio my_bgm right volume to 100", null, 0, IntPtr.Zero);

            // 前回setVolumeした値を、現在再生中のサウンドの音量として格納
            p_MCI_VolumeBGM = _volume_0to1000;

            return _beforeVolume;
        }
        /// <summary>
        /// フォルダにあるmp3ファイルをランダムで再生します。再生したサウンドファイルのフルパスを返します。
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath"></param>
        public static string playBGM_MP3s_FromDirectory(string _directoryName_FullPath)
        {
            List<string> _fileNames = MyTools.getFileNames_FromDirectoryName(_directoryName_FullPath, false, false, true, "mp3");// "mp3", "wav", "wma");
            string _returnFileName = "";
            if (_fileNames.Count > 0)
            {
                int _randomNum = MyTools.getRandomNum(0, _fileNames.Count - 1);
                _returnFileName = _fileNames[_randomNum];
                MCI_stopBGM();
                MCI_playBGM(_fileNames[_randomNum], true);
            }
            return _returnFileName;
        }
        // 以下、mciSendCommandを使った方法の草案。結局、なぜかエラーでできなかった・・・。
        #region 草案、mciSendCommandを使ってMP3・WAV・WMAファイルを再生するMCIを使いこなすメソッド playSound_MCI
        //public enum ESoundMCI
        //{
        //    ファイルオープン,
        //    ファイルクローズ,
        //    再生,
        //    停止,
        //    最初に巻き戻し,
        //    一時停止,
        //    一時停止解除,
        //}
        //// 草案、出来なかった。なんかうまく動かない。
        //public static void playSound_MCI(ESoundMCI _サウンドMCI機能)
        //{
        //    switch (_サウンドMCI機能)
        //    {
        //        case ESoundMCI.ファイルオープン:		//オープン
        //            mop.lpstrDeviceType = "WaveAudio";
        //            mop.lpstrElementName = "Windows XP Startup.wav";
        //            mciSendCommand(NULL, MCI.MCI_OPEN, MCI.MCI_OPEN_TYPE | MCI.MCI_OPEN_ELEMENT, mop);
        //            break;
        //        case ESoundMCI.ファイルクローズ:	//クローズ
        //            mciSendCommand(mop.wDeviceID, MCI.MCI_CLOSE, 0, 0);
        //            //PostQuitMessage(0);
        //            break;
        //        case ESoundMCI.再生: //再生
        //            mciSendCommand(mop.wDeviceID, MCI.MCI_PLAY, 0, 0);
        //            break;
        //        case ESoundMCI.停止:	//停止
        //            mciSendCommand(mop.wDeviceID, MCI.MCI_STOP, 0, 0);
        //            break;
        //        case ESoundMCI.最初に巻き戻し:	//巻き戻し
        //            mciSendCommand(mop.wDeviceID, MCI.MCI_SEEK, MCI.MCI_SEEK_TO_START, 0);
        //            break;
        //        case ESoundMCI.一時停止:	//一時停止
        //            mciSendCommand(mop.wDeviceID, MCI.MCI_PAUSE, 0, 0);
        //            break;
        //        case ESoundMCI.一時停止解除:	//一時停止解除
        //            mciSendCommand(mop.wDeviceID, MCI.MCI_RESUME, 0, 0);
        //            break;
        //    }
        //    //return 0;

        //    //元のメモ
            
        //        case WM_CREATE:		//オープン
        //            mop.lpstrDeviceType = "WaveAudio";
        //            mop.lpstrElementName = "Windows XP Startup.wav";
        //            mciSendCommand(NULL, MCI.MCI_OPEN, MCI.MCI_OPEN_TYPE | MCI.MCI_OPEN_ELEMENT, (DWORD) & mop);
        //            return 0;
        //        case WM_DESTROY:	//クローズ
        //            mciSendCommand(mop.wDeviceID, MCI.MCI_CLOSE, 0, 0);
        //            PostQuitMessage(0);
        //            return 0;
        //        case WM_KEYDOWN:
        //            switch (wParam)
        //            {
        //                case 'A':	//再生
        //                    mciSendCommand(mop.wDeviceID, MCI.MCI_PLAY, 0, 0);
        //                    return 0;
        //                case 'S':	//停止
        //                    mciSendCommand(mop.wDeviceID, MCI.MCI_STOP, 0, 0);
        //                    return 0;
        //                case 'D':	//巻き戻し
        //                    mciSendCommand(mop.wDeviceID, MCI.MCI_SEEK, MCI.MCI_SEEK_TO_START, 0);
        //                    return 0;
        //                case 'F':	//一時停止
        //                    mciSendCommand(mop.wDeviceID, MCI.MCI_PAUSE, 0, 0);
        //                    return 0;
        //                case 'G':	//一時停止解除
        //                    mciSendCommand(mop.wDeviceID, MCI.MCI_RESUME, 0, 0);
        //                    return 0;
        //            }
        //            return 0;

        //    }
        //    //
        #endregion
        #region 草案、MCIでマイク録音する、できなかった。。
        /// <summary>
        /// 録音ファイル名です。
        /// </summary>
        public static string p_MCI_recordedFileName_FullPath = 
            Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス + "MCI_recordMIC_sample1.wav";//"C:/Temp/sampleRecord.wav"; // 初期値は添付ファイル
        /// <summary>
        /// マイクから録音を開始して、trueを返します。デフォルトの録音ファイル名： p_MCI_recordedFileName_FullPath
        /// 既に録音ファイルが存在していたら、上書きします。
        /// 何らかの原因で録音を開始できなかったら、falseを返します。
        /// </summary>
        public static bool MCI_recordMic_Start()
        {
            return MCI_recordMic_Start(p_MCI_recordedFileName_FullPath);
        }
        /// <summary>
        /// マイクから録音を開始して、trueを返します。デフォルトの録音ファイル名： p_MCI_recordedFileName_FullPath
        /// 既に録音ファイルが存在していたら、上書きします。
        /// 何らかの原因で録音を開始できなかったら、falseを返します。
        /// </summary>
        public static bool MCI_recordMic_Start(string _recordedFileName_FullPath)
        {
            p_MCI_recordedFileName_FullPath = _recordedFileName_FullPath;
            string set = "channels 2 samplespersec 44100 alignment 2 bitspersample 16";
            if(DEBUGShown_MCISendString("open new type waveaudio alias REC", null, 0, IntPtr.Zero) == 0){
                mciSendString("set REC " + set, null, 0, IntPtr.Zero);
                mciSendString("record REC", null, 0, IntPtr.Zero);
                return true;
            }
            return false;
        }
        /// <summary>
        /// マイクから録音を開始していたら、ストップして、保存した録音ファイル名を返します。デフォルトの録音ファイル名： p_MCI_recordedFileName_FullPath
        /// 既に録音ファイルが存在していたら、上書きします。
        /// </summary>
        public static string MCI_recordMic_Stop()
        {
            DEBUGShown_MCISendString("stop REC", null, 0, IntPtr.Zero);
            mciSendString("save REC " + p_MCI_recordedFileName_FullPath, null, 0, IntPtr.Zero);
            mciSendString("close REC", null, 0, IntPtr.Zero);
            return p_MCI_recordedFileName_FullPath;
        }
        //public static bool recordMic_Start_MCI()
        //{
        //    mop = new MCI.MCI_OPEN_PARMS();
        //    mciSendCommand(0, MCI.MCI_OPEN, MCI.MCI_OPEN_TYPE | MCI.MCI_OPEN_ELEMENT, mop.lpstrAlias);

        //    if (mciSendCommand(mop.wDeviceID, MCI.MCI_RECORD, 0, IntPtr.Zero) != 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public static bool recordMic_Stop_MCI()
        //{
        //    // 適当。コマンドをCLOSEにしただけ。動作確認していない。。
        //    if (mciSendCommand(mop.wDeviceID, MCI.MCI_CLOSE, 0, IntPtr.Zero) != 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //private static MCI.MCI_OPEN_PARMS mop;
        #endregion
        // 草案終わり。      
        // このプログラムソースの参考　http://wisdom.sakura.ne.jp/system/winapi/media/mm8.html 
        // 他に詳しいサイト　http://www13.plala.or.jp/kymats/study/MULTIMEDIA/mciCommand_play.html
        #region mciSendCommandのAPI宣言方法
        //[DllImport("winmm.dll")]//http://msdn.microsoft.com/ja-jp/library/cc410493.aspx
        //private extern static uint mciSendCommand(
        //    uint IDDevice, // デバイス識別子
        //    uint uMsg, // コマンドメッセージ
        //    uint fdwCommand, // フラグ
        //    uint dwParam // パラメータを保持している構造体
        //);

        //[StructLayout(LayoutKind.Sequential)]
        //private struct MCI_RECORD_PARMS
        //{
        //    public uint dwCallback;
        //    public uint dwFrom;
        //    public uint dwTo;
        //}
        //private static MCI_RECORD_PARMS mrp;


        //[StructLayout(LayoutKind.Sequential)]//http://wisdom.sakura.ne.jp/system/winapi/media/mm3.html
        //private struct MCI_OPEN_PARMS
        //{
        //    public uint dwCallback;
        //    public uint wDeviceID;
        //    public string lpstrDeviceType; // 必ず指定？　"WaveAudio"(wav)、"MPEGVideo"(mp3,wma)など。以下のレジストリの中の値の入れる？　HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\MCI Extensions 
        //    public string lpstrElementName; // 必ず指定？
        //    public string lpstrAlias;
        //}



        // http://www.dotnetspark.com/kb/kb/kb/2082-create-sound-recorder-c-and-c-sharp.aspx
        // Functions
        [DllImport("winmm.dll", CharSet = CharSet.Ansi,
        BestFitMapping = true, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern uint mciSendCommand(
            uint mciId,
            uint uMsg,
            uint dwParam1,
            IntPtr dwParam2);
        // こうとも書ける
        //uint IDDevice, // デバイス識別子
        //uint uMsg, // コマンドメッセージ
        //uint fdwCommand, // フラグ
        //uint dwParam // パラメータを保持している構造体

        [DllImport("winmm.dll", CharSet = CharSet.Ansi,
        BestFitMapping = true, ThrowOnUnmappableChar = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool mciGetErrorString(
            uint mcierr,
            [MarshalAs(UnmanagedType.LPStr)]
            System.Text.StringBuilder pszText,
            uint cchText);
        #endregion
        #region mciSendCommandの定数一覧
        internal static class MCI
        {
            // Constants

            public const string WaveAudio = "waveaudio";

            public const uint MM_MCINOTIFY = 0x3B9;

            public const uint MCI_NOTIFY_SUCCESSFUL = 0x0001;
            public const uint MCI_NOTIFY_SUPERSEDED = 0x0002;
            public const uint MCI_NOTIFY_ABORTED = 0x0004;
            public const uint MCI_NOTIFY_FAILURE = 0x0008;

            public const uint MCI_OPEN = 0x0803;
            public const uint MCI_CLOSE = 0x0804;
            public const uint MCI_PLAY = 0x0806;
            public const uint MCI_SEEK = 0x0807;
            public const uint MCI_STOP = 0x0808;
            public const uint MCI_PAUSE = 0x0809;
            public const uint MCI_RECORD = 0x080F;
            public const uint MCI_RESUME = 0x0855;
            public const uint MCI_SAVE = 0x0813;
            public const uint MCI_LOAD = 0x0850;
            public const uint MCI_STATUS = 0x0814;

            public const uint MCI_SAVE_FILE = 0x00000100;
            public const uint MCI_OPEN_ELEMENT = 0x00000200;
            public const uint MCI_OPEN_TYPE = 0x00002000;
            public const uint MCI_LOAD_FILE = 0x00000100;
            public const uint MCI_STATUS_POSITION = 0x00000002;
            public const uint MCI_STATUS_LENGTH = 0x00000001;
            public const uint MCI_STATUS_ITEM = 0x00000100;

            public const uint MCI_NOTIFY = 0x00000001;
            public const uint MCI_WAIT = 0x00000002;
            public const uint MCI_FROM = 0x00000004;
            public const uint MCI_TO = 0x00000008;

            // Structures

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct MCI_OPEN_PARMS
            {
                public IntPtr dwCallback;
                public uint wDeviceID;
                public IntPtr lpstrDeviceType;
                public IntPtr lpstrElementName;
                public IntPtr lpstrAlias;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct MCI_RECORD_PARMS
            {
                public IntPtr dwCallback;
                public uint dwFrom;
                public uint dwTo;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct MCI_PLAY_PARMS
            {
                public IntPtr dwCallback;
                public uint dwFrom;
                public uint dwTo;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct MCI_GENERIC_PARMS
            {
                public IntPtr dwCallback;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct MCI_SEEK_PARMS
            {
                public IntPtr dwCallback;
                public uint dwTo;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct MCI_SAVE_PARMS
            {
                public IntPtr dwCallback;
                public IntPtr lpfilename;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            public struct MCI_STATUS_PARMS
            {
                public IntPtr dwCallback;
                public uint dwReturn;
                public uint dwItem;
                public uint dwTrack;
            } ;
        }
        #endregion
        
            #endregion // MCI終わり

        #endregion // サウンド同時再生ハイブリット方式終わり

            #region ■■■BGM再生と効果音再生をわけたメソッド: playBGM / playSE
        public static bool playBGM(string _BGM_FileNameFullPath, bool _isRepeat)
        {
            // BGMは一つだけなので、他のBGMファイルを切る
            // これはMCI_playBGM()内でやってるMCI_stopBGM();
            return MCI_playBGM(_BGM_FileNameFullPath, _isRepeat);
        }
        public static int stopBGM()
        {
            return MCI_stopBGM();
        }
        public static int pauseBGM()
        {
            return MCI_pauseBGM();
        }
        public static int resumeBGM()
        {
            return MCI_resumeBGM();
        }
        public static int getVolumeBGM()
        {
            return MCI_getVolume_BGM();
        }
        public static int setVolumeBGM(int _volume_0to1000)
        {
            return MCI_setVolume_BGM(_volume_0to1000);
        }
        public static string getBGMFileNameFullPath_NowPlaying()
        {
            return MCI_getPlayingBGMName_FullPath();
        }
        public static bool isBGMPlaying()
        {
            return MCI_isEndBGM();
        }
        /// <summary>
        /// MediaControlInterfaceを使って、mp3/wav/mid/wmaファイルを効果音（ＳＥ）として再生します．
        /// MCI_plyaBGM()とはチャネル番号は干渉（競合）せず、ＢＧＭと同時に効果音を鳴らせます。
        /// 引数２で、ループの有無を設定できます。
        /// 引数３で、他のチャンネルのSEを全て停止するかを設定できます。
        /// 引数４で、SEのチャンネル番号を設定でき、同じチャンネル番号を使うと上書き再生します。
        /// ※ただし、複数の効果音を何個まで同時に鳴らせるかは環境により異なります。
        /// 
        /// 　　　　　　　　　　同じチャンネルの同じファイルの場合は、最初から再生しなおします．
        /// 理論的に再生できたら再生チャンネル番号を返し（これが送られても再生できない原因不明なエラーは多々ありますが）、
        /// 理論的に再生不可能な場合は-1を返します。
        /// </summary>
        public static int playSE(string _SE_FileNameFullPath, bool _isRepeat, int _ChannelNo, bool _isStopOtherSEs)
        {
            // (a)MCIだけを使うバージョン
            return MCI_playSE(_SE_FileNameFullPath, _isRepeat, _isStopOtherSEs, _ChannelNo);
        }
        /// <summary>
        /// MediaControlInterfaceを使って、mp3/wav/mid/wmaファイルを効果音（ＳＥ）として再生します．
        /// MCI_plyaBGM()とはチャネル番号は干渉（競合）せず、ＢＧＭと同時に効果音を鳴らせます。
        /// SEのチャンネル番号は、再生中でないチャンネルを自動的に探して再生し、いっぱいになった時は「最後に上書きしたＳＥチャンネル番号+1」を上書きします。
        /// ※ただし、複数の効果音を何個まで同時に鳴らせるかは環境により異なります。
        /// 引数２で、ループの有無を設定できます。
        /// 
        /// 　　　　　　　　　　
        /// 理論的に再生できたら再生チャンネル番号を返し（これが送られても再生できない原因不明なエラーは多々ありますが）、
        /// 理論的に再生不可能な場合は-1を返します。
        /// </summary>
        public static int playSE(string _SE_FileNameFullPath, bool _isRepeat)
        {
            // (a)MCIだけを使うバージョン
            return MCI_playSE(_SE_FileNameFullPath, _isRepeat);

            // (b)MCIはBGM専用として、MCIを使わないバージョン
            //if (SoundPlayer_canPlay() == true)
            //{
            //    return SoundPlayer_playSound(_SE_FileNameFullPath, false);
            //}
            //else if (p_isUseWMP == true && WMP_canPlay() == true)
            //{
            //    return WMP_playSound(_SE_FileNameFullPath, false);
            //}
            //ConsoleWriteLine("playSE: チャンネル不足により再生不可。効果音ファイル名:"+_SE_FileNameFullPath);
        }
        public static int stopSE()
        {
            return MCI_stopSE();
        }
        public static int pauseSE()
        {
            return MCI_pauseSE();
        }
        public static int resumeSE()
        {
            return MCI_resumeSE();
        }
        public static int getVolumeSE()
        {
            return MCI_getVolume_SE(0); //getVolume_Master(); // マスターしか取れない
        }
        public static int setVolumeSE(int _volume_0to1000)
        {
            return MCI_setVolume_SE(_volume_0to1000, 0); //setVolume_Master(_volume_0to1000); // マスターを上書きしてしまう
        }
        //public static string getSEFileNameFullPath_NowPlaying()
        //{
        //    return SoundPlayer_getFileNameFullPath();
        //}
        // SoundPlayerクラスでは曲の長さを取得しないので、取れない。
        //public static bool isSEPlaying()
        //{
        //    return SoundPlayer_isPlaying();
        //}
            #endregion

            #region ■Win32API waveOutGetVolume/waveOutSetVolume でWAVファイルの再生音量設定・取得するAPI宣言方法
        //★音量設定
        [DllImport("winmm.dll")]
        extern static int waveOutSetVolume(int uDeviceID, int dwVolume);
        //★音量取得
        [DllImport("winmm.dll")]
        extern static int waveOutGetVolume(int uDeviceID, ref int lpdwVolume);
        /// <summary>
        /// waveOutGetVolomeのAPIを使って、WAVEファイルの現在の再生音量を（0-1000）で取得します。
        /// </summary>
        private static int getVolume_ByWindowsAPI()
        {
            int _volume_0To65535 = 0;
            waveOutGetVolume(0, ref _volume_0To65535);
            int _volume = (int)((double)_volume_0To65535 * (1000.0 / 65535.0));
            return _volume;
        }
        /// <summary>
        /// waveOutSetVolomeのAPIを使って、WAVEファイルの現在の再生音量を(0-1000)で設定します。
        /// </summary>
        private static int setVolume_ByWindowsAPI(int _volume_0To1000)
        {
            int _beforeVolume = getVolume_ByWindowsAPI();
            int _volume = (int)((double)_volume_0To1000 * (65535.0 / 1000.0));
            _volume = Math.Max(0, _volume);
            _volume = Math.Min(_volume, 65535);
            waveOutSetVolume(0, _volume);
            return _beforeVolume;
        }
        #endregion



    #endregion // サウンド系終わり




        // ■ファイル系文字列処理
        // これらを使った新しいクラスをMyToolsと独立させたかったら、この部分だけをコピペして使ってください。
        // ※標準のFileクラスのオブジェクトを作成する方法は、staticメソッドでは使いにくいですし、ファイルアクセスも必要になります。
        //　 この部分は、staticメソッド以外を使う必要があるので、MyFileIOクラスを参照してください。
        // 　ここでは、ファイルの変換を文字列だけで処理できる内容はそれで完結させた方がよいものや、staticメソッドだけで実現できるディレクトリ内のファイルチェックなどだけを、まとめてみました。
        // 　また、よくありがちなファイルパス文字列のケアレスミスのチェック→エラー処理も、できるだけここで済ませてしまえば、あとで見落としにくくなるかもしれません。
        #region ファイルの拡張子無しの名前を取得: getFileLeftOfPeriodName
        /// <summary>
        /// ファイルの拡張子無しの名前（aaa.docの「aaa」）を取得します．
        /// </summary>
        /// <param name="fileName"></param>
        public static string getFileName_NoDott(string fileName)
        {
            string fileName_NoDott = "";
            int dottIndex = fileName.LastIndexOf(".");
            fileName_NoDott = fileName.Substring(0, dottIndex);
            return fileName_NoDott;
        }
        #endregion
        #region ファイルの拡張子を取得: getFileRightOfPeriodName
        /// <summary>
        /// ファイルの拡張子（aaa.docの「doc」）を小文字にして取得します．
        /// </summary>
        /// <param name="fileName"></param>
        public static string getFileDottName(string fileName)
        {
            string fileDottName = "";
            if (fileName == null)
            {
                return "";
            }
            int dottIndex = fileName.LastIndexOf(".");
            if (dottIndex == -1)
            {
                fileDottName = ""; // 「.」が存在しないときは、""を返す。
            }
            else
            {
                fileDottName = fileName.Substring(dottIndex + 1, fileName.Length - (dottIndex + 1));
            }
            // 小文字にする
            fileDottName = fileDottName.ToLower();
            return fileDottName;
        }
        #endregion
        #region ファイルの一番右端のディレクトリまたはファイル名の文字列（"aaa\\bbb\\ccc"でのccc）の取得: getFileName_***
        /// <summary>
        /// 引数の絶対パスから，一番右端のディレクトリまたはファイル名の文字列（"aaa\\bbb\\ccc"でのccc）を取ってきます．
        /// </summary>
        /// <param name="fineName_FullPath"></param>
        /// <returns></returns>
        public static string getFileName_TheMostRightFileOrDirectory(string fullPath)
        {
            string fullPath_Right = "";
            if (fullPath != "")
            {
                // "/"での記述はこのクラスでは推奨していないが、ちゃんとチェックはする
                int lastDirectoryBeginIndex = fullPath.Substring(0, fullPath.Length - 1).LastIndexOf("/");
                if (lastDirectoryBeginIndex == -1)
                {
                    lastDirectoryBeginIndex = fullPath.Substring(0, fullPath.Length - 1).LastIndexOf("\\");
                }
                fullPath_Right = fullPath.Substring(lastDirectoryBeginIndex + 1, fullPath.Length - lastDirectoryBeginIndex - 1);
            }
            return fullPath_Right;
        }
        #endregion
        #region ディレクトリ名の取得: getDirectoryName
        /// <summary>
        /// 引数のファイルの絶対パスから，一つ親であるディレクトリ名を取ってきます．（いわゆる「..\\」の取得。（ただし"\\"は含まないので後で含めてください））
        /// </summary>
        /// <param name="fileName_FullPath">ディレクトリ内にあるサンプルファイルのフルパス（"C:\\...\\SampleDirectory\\sample.txt"など）</param>
        /// <param name="isIncluding_FullPath">ディレクトリにフルパスを含めるかどうか（trueなら"C:\\...\\SampleDirectory"、falseなら"SampleDirectory"を取得）</param>
        /// <returns></returns>
        public static string getDirectoryName(string fileName_FullPath, bool isIncluding_FullPath)
        {
            return getDirectoryName_The_Nth_Right(fileName_FullPath, 1, isIncluding_FullPath);
        }
        #endregion
        #region n階層目のディレクトリ名またはファイル名を取得: getDirectoryName_Tha_Nth_Right
        /// <summary>
        /// 引数の絶対パスから，右端から数えてn階層目のディレクトリ名またはファイル名を取ってきます
        /// （例えば、n_parentNum=1,isIncluding_FullPath=trueの場合，"C:\\aaa\\bbb\\ccc"では，「C:\\aaa\\bbb」を取得します。 また例えば、n_parentNum=0のとき「ccc」を取得します。）
        /// </summary>
        /// <param name="fileName_FullPath">ディレクトリ内にあるサンプルファイルのフルパス（"C:\\...\\SampleDirectory\\sample.txt"など）</param>
        /// <param name="n_parentNum">n階層目のディレクトリ名を取得する、n</param>
        /// <param name="isIncluding_FullPath">ディレクトリにフルパスを含めるかどうか（trueなら"C:\\...\\SampleDirectory"、falseなら"SampleDirectory"を取得）</param>
        /// <returns></returns>
        public static string getDirectoryName_The_Nth_Right(string fileName_FullPath, int n_parentNum, bool isIncluding_FullPath)
        {
            string n_th_Path = fileName_FullPath;
            // 一番右にある"/"を探す
            int DirectoryBeginIndex = n_th_Path.LastIndexOf("/");
            if (DirectoryBeginIndex == -1)
            {
                // "\\"の可能性の方が優先（だから後でやる）
                DirectoryBeginIndex = n_th_Path.LastIndexOf("\\");
            }
            // nの回数だけ、上の階層に移動する
            for (int i = 0; i < n_parentNum; i++)
            {
                DirectoryBeginIndex = n_th_Path.LastIndexOf("/");
                if (DirectoryBeginIndex == -1)
                {
                    DirectoryBeginIndex = n_th_Path.LastIndexOf("\\");
                }
                // 一個上の階層へ
                n_th_Path = n_th_Path.Substring(0, DirectoryBeginIndex);
            }
            // n階層目のディレクトリを、絶対パスで取得
            string fullPath_Right = n_th_Path;

            // 絶対パスでない場合、左側を削除
            if (isIncluding_FullPath == false)
            {
                fullPath_Right = getFileName_TheMostRightFileOrDirectory(n_th_Path);
            }
            return fullPath_Right;
        }
        #endregion
        #region ディレクトリ内の全てのファイル名を取得: getFileNames_FromDirectoryName
        /// <summary>
        /// ディレクトリ内に存在する特定拡張子のファイル名一覧を取得します。（ディレクトリは含みません。）
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">ディレクトリのフルパス（"C:\\...\\フォルダ"など）</param>
        public static List<string> getFileNames_FromDirectoryName(string _directoryName_FullPath)
        {
            return getFileNames_FromDirectoryName(_directoryName_FullPath, true, true, "*");
        }
        #endregion
        #region ディレクトリ内に存在する特定拡張子のファイル名一覧を取得
        /// <summary>
        /// ディレクトリ内に存在する特定拡張子のファイル名一覧を取得します。（ディレクトリは含みません。）
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">ディレクトリのフルパス（"C:\\...\\フォルダ"など）</param>
        /// <param name="_isIncludingSubDirectoriesFiles">サブディレクトリ内のファイルを含めるかどうか（trueにすると、"C:\\...\\フォルダ\\サブフォルダA\\sample.txt"なども取得する）</param>
        /// <param name="_isFullPath_InEachFileName">ファイル名をフルパスにするかどうか（falseにすると、"sample.txt"などを取得する）</param>
        /// <param name="_fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ">"txt"や"jpg"や"mp3"など。全ての場合は"*"。今の実装では""でも"**"でも"***"でも"*"と同じ意味</param>
        /// <returns></returns>
        public static List<string> getFileNames_FromDirectoryName(string _directoryName_FullPath, bool _isIncludingSubDirectoriesFiles, bool _isFullPath_InEachFileName, string _fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ)
        {
            string[] _拡張子たち = new string[1];
            _拡張子たち[0] = _fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ;
            return getFileNames_FromDirectoryName(_directoryName_FullPath, _isIncludingSubDirectoriesFiles, _isFullPath_InEachFileName, _拡張子たち);
        }
        /// <summary>
        /// ディレクトリ内に存在する特定拡張子のファイル名一覧を取得します。（ディレクトリは含みません。）
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">ディレクトリのフルパス（"C:\\...\\フォルダ"など）</param>
        /// <param name="_isIncludingSubDirectoriesFiles">サブディレクトリ内のファイルを含めるかどうか（trueにすると、"C:\\...\\フォルダ\\サブフォルダA\\sample.txt"なども取得する）</param>
        /// <param name="_isFullPath_InEachFileName">ファイル名をフルパスにするかどうか（falseにすると、"sample.txt"などを取得する）</param>
        /// <param name="_fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ">"txt"や"jpg"や"mp3"など。全ての場合は"*"。今の実装では""でも"**"でも"***"でも"*"と同じ意味</param>
        /// <returns></returns>
        public static List<string> getFileNames_FromDirectoryName(string _directoryName_FullPath, bool _isIncludingSubDirectoriesFiles, bool _isFullPath_InEachFileName, params string[] _fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ)
        {
            string[] _fileNames = getFileNames_FromSearchingDirectory(_directoryName_FullPath, _isIncludingSubDirectoriesFiles, "*", _isFullPath_InEachFileName);
            List<string> _fileNameLists = null;

            // 拡張子のチェックは、大文字と小文字が混ざると確認できるか不安なので、小文字で統一した自前のメソッドで判定する
            string[] _拡張子たち = _fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ;
            string _拡張子０番目 = MyTools.getArrayValue(_fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ, 0);
            if (_拡張子０番目 == "" || _拡張子０番目 == "***" || _拡張子０番目 == "**" || _拡張子０番目 == "*")
            {
                // 全て格納
                _fileNameLists = new List<string>(_fileNames);
            }
            else
            {
                // 指定した拡張子のファイルだけ格納
                _fileNameLists = new List<string>(_fileNames.Length);
                foreach (string _fName in _fileNames)
                {
                    foreach (string _拡張子 in _拡張子たち)
                    {
                        if (MyTools.getFileRightOfPeriodName(_fName) == _拡張子)
                        {
                            _fileNameLists.Add(_fName);
                            break;
                        }
                    }
                }
            }
            return _fileNameLists;
            // 参考。感謝。
            // http://www.atmarkit.co.jp/fdotnet/dotnettips/053allfiles/allfiles.html
            //string[] files = Directory.GetFiles("c:\\");
            //string[] dirs = Directory.GetDirectories("c:\\");
            //string[] both = Directory.GetFileSystemEntries("c:\\");
        }
        /// <summary>
        /// ディレクトリ内に存在する、検索文字列が含まれる（拡張子も含めた）ファイル名一覧を取得します。サブディレクトリ内のファイルを含めるかを指定できます。（サブディレクトリはファイル名一覧には含みません。）
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">ディレクトリのフルパス（"C:\\...\\フォルダ"など）</param>
        /// <param name="_isIncludingSubDirectoriesFiles">サブディレクトリ内のファイルを含めるかどうか（trueにすると、"C:\\...\\フォルダ\\サブフォルダA\\sample.txt"なども取得する）</param>
        /// <param name="_searchString">検索文字列。全て"*"や拡張子だけを調べる"*.txt"などにも対応しています。""にすると一つも該当しなくなります。</param>
        /// <param name="_isFullPath_InEachFileName">ファイル名をフルパスにするかどうか（falseにすると、"sample.txt"などを取得する）</param>
        /// <returns></returns>
        private static string[] getFileNames_FromSearchingDirectory(string _directoryName_FullPath, bool _isIncludingAllSubDirectoriesFiles, string _searchString, bool _isFullPath_InEachFileName)
        {
            string[] _fileNames;
            //基本は全部取得してから絞る？_searchString = "*";
            if (_isIncludingAllSubDirectoriesFiles == true)
            {
                // サブディレクトリ内のファイルも含む
                _fileNames = System.IO.Directory.GetFiles(_directoryName_FullPath, _searchString, System.IO.SearchOption.AllDirectories);
            }
            else
            {
                // トップディレクトリ内のファイルのみ
                _fileNames = System.IO.Directory.GetFiles(_directoryName_FullPath, _searchString, System.IO.SearchOption.TopDirectoryOnly);
            }
            // GetFilesはフルパスを取得する
            //// フルパスにする
            //// パスの区切り文字「/」や「\\」が入っているかを調べ、区切り文字をどちらにするか決める
            //char _path1 = '/';
            //if(_searchingDirectoryName_FullPath.Contains("\\") == true) _path1 = '\\';
            //// 最後に区切り文字が付いてなかったら、つける
            //if(_searchingDirectoryName_FullPath[_searchingDirectoryName_FullPath.Length-1] != _path1) _searchingDirectoryName_FullPath = _searchingDirectoryName_FullPath + _path1;
            //for (int i=0; i<_fileNames.Length; i++)
            //{
            //    _fileNames[i] = _searchingDirectoryName_FullPath + _fileNames[i];
            //}
            // 名前だけにするかどうか
            if (_isFullPath_InEachFileName == false)
            {
                for (int i = 0; i < _fileNames.Length; i++)
                {
                    _fileNames[i] = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_fileNames[i]);
                }
            }
            return _fileNames;
        }
        #endregion
        #region ファイルやディレクトリが実際に存在するか: isExist
        /// <summary>
        /// 引数にフルパスを指定したファイルやディレクトリが実際に存在するかを返します。
        ///        　※ファイルの扱いでこの辺のケアレスミスが多いので、心配な場合は、isExt()やこのメソッドを呼び出して、予想外のエラーの少ないファイル名・ディレクトリ名を扱うよう心がけてください。
        /// 
        /// 　　　　参考：ディレクトリ（フォルダの）存在を調べるには、System.IO.Directory.Existsメソッドを使います。
        /// 　File.Existsメソッドで、ディレクトリの存在を調べることはできません。
        ///   File.Existsメソッドに存在するディレクトリを指定しても結果はFalseになります。
        /// 　また、パス名として無効な文字列を指定しても、Falseです。
        /// 　さらには、たとえファイルが存在しても、ファイルを読み取るのに十分なアクセス許可を持たない場合も、Falseです。
        /// 　参考。感謝。http://dobon.net/vb/dotnet/file/fileexists.html
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        /// <returns></returns>
        public static bool isExist(string _fileName_FullPath)
        {
            bool _isExist = false;
            if (isFile(_fileName_FullPath) == true)
            {
                _isExist = System.IO.File.Exists(_fileName_FullPath);
            }
            else
            {
                _isExist = System.IO.Directory.Exists(_fileName_FullPath);
            }
            return _isExist;
        }
        #endregion
        #region フルパスがファイルかディレクトリか調べる: isFileName
        /// <summary>
        /// 引数のフルパスの最後がファイルであればtrueを返します（"."を含んでいるかを見ているだけで、実際に存在するかはチェックしていません）。ディレクトリの場合はfalseを返します。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        /// <returns></returns>
        public static bool isFile(string _fileName_FullPath)
        {
            _fileName_FullPath = getCheckedFilePath(_fileName_FullPath);
            string _name_OnlyRightest = getFileName_TheMostRightFileOrDirectory(_fileName_FullPath);
            bool _isFile = _name_OnlyRightest.Contains(".");
            return _isFile;
        }
        #endregion
        #region ■ファイルやディレクトリのパスが正しいかどうかチェックして返す: getCheckedFilePath
        /// <summary>
        /// 引数のフルパスのファイルやディレクトリのパスが正しいかどうかチェックし、正しいパスを返します。
        /// 具体的には、ディレクトリの区切りが区切りが"/"なら"\\"に変換し、パスの最後に"\\"が入っていれば削除します。
        /// また、ファイルやディレクトリの存在を確認し、存在しない場合はメッセージボックスや標準出力に出力します。
        ///  　　　　※ファイルの扱いでこの辺のケアレスミスが多いので、心配な場合は、isExt()やこのメソッドを呼び出して、予想外のエラーの少ないファイル名・ディレクトリ名を扱うよう心がけてください。
        /// </summary>
        /// <param name="_fullPath"></param>
        /// <returns></returns>
        public static string getCheckedFilePath_AndShowErrorMessage(string _fullPath)
        {
            _fullPath = getCheckedFilePath(_fullPath);
            // ファイルの存在をチェック
            if (isExist(_fullPath) == false)
            {
                //MessageBox.Show("getCheckedFilePath: ■エラー: ファイル／ディレクトリ名「" + getFileName_NotFullPath_LastFileOrDirectory(_fullPath) + "」\nフルパス\"" + _fullPath + "\"\nは存在しません。\nプログラムを続けますか？", "ファイルが見つからないエラー", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                ConsoleWriteLine("getCheckedFilePath: ■エラー: ファイル\"" + _fullPath + "\"は存在しません。");
            }
            return _fullPath;
        }
        /// <summary>
        /// ファイルやディレクトリのパスが正しいかどうかチェックし、正しいパスを返します。
        /// 具体的には、ディレクトリの区切りが区切りが"/"なら"\\"に変換し、パスの最後に"\\"が入っていれば削除します。
        /// 　　　　※ファイルの扱いでこの辺のケアレスミスが多いので、心配な場合は、isExt()やこのメソッドを呼び出して、予想外のエラーの少ないファイル名・ディレクトリ名を扱うよう心がけてください。
        /// </summary>
        /// <param name="_fullPath"></param>
        /// <returns></returns>
        public static string getCheckedFilePath(string _fullPath)
        {
            // 予め、区切り文字が"/"なら"\\"に変換しておく
            if (_fullPath.EndsWith("/") == true)
            {
                _fullPath = _fullPath.Replace('/', '\\');
            }
            // フルパスの右端に"\\"が付いていたら取る。
            if (_fullPath.EndsWith("\\") == true)
            {
                _fullPath = _fullPath.Substring(0, _fullPath.Length - 1);
            }
            // まだ"\\"が付いていたら取る
            if (_fullPath.EndsWith("\\") == true)
            {
                _fullPath = _fullPath.Substring(0, _fullPath.Length - 1);
            }

            // 右端の部分だけ取り出す
            //string _rightestName = getFileName_NotFullPath_LastFileOrDirectory(fullPath);
            //if (isFileName(_rightestName) == true)
            //{
            //    // ファイルの場合は、何もしない
            //    // 拡張子は「.TXT」や「.txt」とどちらを指定しても、ＯＳ側がちゃんと判断してくれるみたい
            //}else
            //{
            //    // ディレクトリの場合も、何もしない
            //}
            return _fullPath;
        }
        #endregion


        // ■スレッド系
        #region 新しいスレッドを立てて並列処理する場合の簡易メソッド
        /// <summary>
        /// 新しくスレッドを立てて，引数のメソッドを並行して処理します．
        /// 生成したスレッドを返します．
        /// ・スレッドが終了するまで何も処理しない場合は，_thread.join()　を使ってください．
        /// （・できれば，メソッドが終了したら，スレッドは _thread.Abort()　で破棄してください．）
        /// 
        /// ※マルチスレッドは，呼び出し過ぎ，lock(Object){}でロック処理をする，スレッドセーフなメソッドだけを引数にするなど，十分に注意してください．
        /// http://ufcpp.net/study/csharp/sp_thread.html
        /// </summary>
        /// <param name="threadFadeOut・フェードアウトまでの処理"></param>
        public static Thread threadSubMethod(ThreadStart _スレッドで並行作業するメソッド名)
        {
            Thread _thread = new Thread(new ThreadStart(_スレッドで並行作業するメソッド名));
            _thread.Start();
            return _thread;
        }
        #endregion



        // ■VisualBasicの機能
        #region VisualBasic.NETの機能を使う（プロジェクトにMicrosoft.VisualBasicの参照の追加が必要です（プロジェクト右クリック→「参照の追加」））
        // この辺を参考に http://www.atmarkit.co.jp/fdotnet/csharptips/013vb/013vb_01.html

        #region 入力ダイアログボックス showInputBox
        /// <summary>
        /// VisualBasic風の入力ダイアログボックスを表示し、入力した文字列を返します。キャンセルを押された場合は、文字列""を返します。
        /// </summary>
        /// <param name="_message・本文"></param>
        /// <param name="_title・入力ボックスのタイトル＿なしでもＯＫ"></param>
        /// <param name="_defaultInputedText・デフォルトテキスト＿なしでもＯＫ"></param>
        /// <returns></returns>
        public static string showInputBox(string _message・本文, string _title・入力ボックスのタイトル＿なしでもＯＫ, string _defaultInputedText・デフォルトテキスト＿なしでもＯＫ)
        {
            string _inputedText = "";
            _inputedText = Microsoft.VisualBasic.Interaction.InputBox(_message・本文, _title・入力ボックスのタイトル＿なしでもＯＫ,
            _defaultInputedText・デフォルトテキスト＿なしでもＯＫ, -1, -1); // 第３と第４は画面の位置。-1は省略された時と一緒の値。
            // キャンセルされると""を返すのも、ちゃんとこの関数の仕様に入ってる。
            return _inputedText;
        }
        #endregion
        #region 文字列変換： getZenkakuString/getHankaku
        // 大文字・小文字変換（String.ToUpper()/ToLower()）以外は、Ｃ＃標準では用意されていないらしい。
        // この辺を参考に http://dobon.net/vb/dotnet/string/strconv.html

        /// <summary>
        /// 指定した文字列を全角に変換して返します。変換できない文字は、元の文字列のまま返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string getZenkakuString(string _string)
        {
            return getConvertedString(_string, Microsoft.VisualBasic.VbStrConv.Wide);
        }
        /// <summary>
        /// 指定した文字列を半角に変換して返します。変換できない文字は、元の文字列のまま返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string getHankakuString(string _string)
        {
            return getConvertedString(_string, Microsoft.VisualBasic.VbStrConv.Narrow);
        }
        /// <summary>
        /// 指定した文字列をひらがなに変換して返します。変換できない文字は、元の文字列のまま返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string getHiraganaString(string _string)
        {
            return getConvertedString(_string, Microsoft.VisualBasic.VbStrConv.Hiragana);
        }
        /// <summary>
        /// 指定した文字列をカタカナに変換して返します。変換できない文字は、元の文字列のまま返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string getKatakanaString(string _string)
        {
            return getConvertedString(_string, Microsoft.VisualBasic.VbStrConv.Katakana);
        }
        /// <summary>
        /// プライベートメソッドです。Microsoft.VisualBasicに依存しているため、内部だけで使用します。
        /// 引数の文字列を指定した形式にに変換して返します。エラーが起きた場合は、もとの文字列を変換せずに返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        private static string getConvertedString(string _string, Microsoft.VisualBasic.VbStrConv _converingType)
        {
            string _returnedString = _string;
            try
            {
                _returnedString = Microsoft.VisualBasic.Strings.StrConv(_string, _converingType, 0);
            }
            catch (Exception e)
            {
            }
            return _returnedString;
        }
        #endregion

        #endregion




        // ■Windowsアプリ系
        #region ※以下はwindowsアプリケーション用のメソッドです。using System.Windows.*;のインポートが無ければ機能しません。がんばって修正すれば他のアプリでも使えるようになるかもしれません…WIN32APIは無理かもですが。

        //// 定数の定義
        //// 曲終了時に次の曲を流したい場合(Windows.Forms限定)　http://bb-side.com/modules/side03/index.php?content_id=32
        //// 曲終了時の処理
        ///// <summary>
        ///// Windows.FormのWindowsのウィンドウメッセージです。MCIで再生されていたサウンドの終了タイミング取得などに使われています。
        ///// </summary>
        ///// <param name="m"></param>
        //protected override void WndProc(ref System.Windows.Forms.Message m)
        //{
        //    if (m.Msg == MM_MCINOTIFY && (int)m.WParam == MCI_NOTIFY_SUCCESSFUL)
        //    {
        //        // 再生終了時の処理（*2）
        //        //（*2）MCI では、再生終了時に指定したウィンドウに NOTIFY メッセージが送られる。
        //        //        そのメッセージを利用して再生終了時の処理を実行することができる。
        //        //        また、次のファイルを再生する場合は、前のファイルをクローズすること。 

        //        // とりあえずクローズだけしておく
        //        MySound_Windows.MCI_stopBGM();
        //        MySound_Windows.p_MCI_nowPlayingBGMName_FullPath = "";
        //    }
        //    //base.WndProc (ref m);
        //}
        //// 定数の定義
        //private static int MM_MCINOTIFY = 0x3B9;
        //private static int MCI_NOTIFY_SUCCESSFUL = 1;


        #endregion

        // ■WindowsAPIの呼び出し
        #region 宣言しているWin32API（メソッドが使う可能性のあるWinAPIです。Windows非依存のメソッドと分けて、個々に書いてます。）

        #region （using Shell32に依存）MP3ファイルのアーティスト情報などを取得: getFileInfo
        ///// <summary>
        ///// MP3ファイルの情報の一覧を文字列型で取得します。
        ///// </summary>
        ///// <param name="_fileFullPath"></param>
        ///// <returns></returns>
        //public static string getFileInfo(string _fileFullPath)
        //{
        //    string dir = MyTools.getDirectoryName(_fileFullPath, true); // MP3ファイルのあるディレクトリ
        //    string file = _fileFullPath;

        //    // VS2010になると以下の文はエラー。２つともShellCalss→Shellにするとうまくいく。参考。感謝。http://tinqwill.blog59.fc2.com/blog-entry-83.html
        //    Shell shell = new Shell();
        //    Folder f = shell.NameSpace(dir);
        //    FolderItem item = f.ParseName(file);
        //    string _FPSInfo = "ファイル"+MyTools.getFileName_NotFullPath_LastFileOrDirectory(_fileFullPath)
        //        +"\n（フルパス："+_fileFullPath+"）\nの情報は、\n";
        //    for (int i = 0; i < 300; i++)
        //    {
        //        _FPSInfo += i+": "+f.GetDetailsOf(item, i)+":"+f.i+"\n"; //★ないようがでないよう・・・[TODO]
        //    }
        //    //MyTools.ConsoleWriteLine(_FPSInfo);
        //    // 以下、たぶんXP/Vistaでの番号。
        //    //Console.WriteLine(f.GetDetailsOf(item, 9)); // アーティスト
        //    //Console.WriteLine(f.GetDetailsOf(item, 10)); // タイトル
        //    //Console.WriteLine(f.GetDetailsOf(item, 12)); // ジャンル
        //    //Console.WriteLine(f.GetDetailsOf(item, 14)); // コメント
        //    //Console.WriteLine(f.GetDetailsOf(item, 17)); // アルバムのタイトル
        //    //Console.WriteLine(f.GetDetailsOf(item, 18)); // 年
        //    //Console.WriteLine(f.GetDetailsOf(item, 19)); // トラック番号

        //    // Folder.GetDetailsOfの定義 参考。感謝。 http://dzone.sakura.ne.jp/blog/2009/06/vbnet-getditailsof.html
        //    // このような項目が取得できることになります。
        //    //    10.タイトル
        //    //    16.アーティスト
        //    //    17.アルバムのタイトル
        //    //    18.年
        //    //    19.トラック番号
        //    //    21.長さ　（※曲の長さらしいです）
        //    // ※Windows7ではXPと値が異なります。(XPとVistaとは一緒？)
        //    //    14.アルバム（※アルバムのタイトル）
        //    //    15.年
        //    //    21.タイトル（※トラックのタイトル）
        //    //    26.トラック番号
        //    //    27.長さ（※曲の長さ）
        //    //    13.参加アーティスト
        //    //プロパティは300近くあるようです。
        //    return _FPSInfo;
        //}
        #endregion



        #endregion


    }
}
