using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PublicDomain;
using System.Threading;

namespace PublicDomain
{
    /// <summary>
    /// アプリケーションのMain関数を持つ、プロジェクト実行時に最初に呼ばれるクラスです。
    /// プロジェクトで共通して使えるプロパティ（ディレクトリの場所、ログ・エラー出力メソッド）を持っています。
    /// 
    /// 単体テストなど、どのクラスからプログラムを始めるかを変更したいときは、このクラスの中を編集してください。
    /// 
    /// ■このプログラムをプロジェクトビルド・実行時に、最初に動かない場合は、
    /// プロジェクト右クリック→「プロパティ」→「スタートアッププログラム」をこれに設定してください。
    /// </summary>
    static class Program・実行ファイル管理者
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            bool _isTestOnly1Class・単体テスト = false; // ゲームを開始する時はfalseにする
            if (_isTestOnly1Class・単体テスト == true)
            {
                // 各クラスの単体テストするときは、ここにテストしたいクラスの__HELP**()メソッドや_test***()メソッドを書く
                MyTools.__HELP_ThisClassusingExamples・このクラスの使い方やテストコード();
                //MySound_Windows.__HELP_ThisClassusingExamples・このクラスの使い方やテストコード();
            }
            else
            {

                // ●●●このプロジェクト実行時に開始するフォームを一個だけ決める！
                // 同時に表示したいフォームなそのメインフォーム内に記述して。
                // （FGameBattleForm1なら、_startDiceBattleGame・ダイスバトルゲーム()内に、バランステスト用、シナリオ用フォームの生成を記述）

                p_gameTestform1 = new FGameBattleForm1();
                // ↑これ実行したら、ここから↓の行、上のフォールを終了するまで実行されてへんで＾＾；。なんかフォーム系は一個newしたらそれ終了するまで次のフォーム実行できひんらしいわ。

                //Application.Run(new SamplePlayer.Form1()); // ＷＡＶ再生のテストフォーム
                //Application.Run(new SampleRecorder.Form1()); // マイク録音のテストフォーム
                //Application.Run(new Form1()); // テストフォーム
                //Application.Run(new FDamageTest());// テストダメージ計算機
                //Application.Run(new FDrawForm()); // フリーハンドグラフフォームのテスト用
                //MyTools.__HELP_ThisClassusingExamples・このクラスの使い方やテストコード(); // MyToolsのテスト用
            }
        }
        /// <summary>
        /// プログラムが最初に実行するフォームのインスタンス
        /// </summary>
        public static FGameBattleForm1 p_gameTestform1 = null;

        /// <summary>
        /// ルートディレクトリ（実行ファイルがある場所，絶対パス）
        /// </summary>
        public static string ROOTDIRECTORY・実行ファイルがあるフォルダパス = MyTools.getProjectDirectory();
        /// <summary>
        /// 「データベース」ディレクトリ（絶対パス，最後に\\を含む）
        /// </summary>
        public static string p_DatabaseDirectory_FullPath・データベースフォルダパス = Program・実行ファイル管理者.ROOTDIRECTORY・実行ファイルがあるフォルダパス + "\\データベース\\";
        /// <summary>
        /// サウンド読み込み用（サウンドデータベース.csvファイルがある場所，絶対パス，最後に\\を含む）。
        /// ※音楽ファイルはこれ + "ＢＧＭ"、効果音ファイルはこれ + "効果音"だが、名前が変わるかもしれないので、できるだけ p_BGMDirectory***、p_SEDirectoryを使って
        /// </summary>
        public static string p_SoundDatabaseFileName_FullPath・サウンドデータベースファイルパス = Program・実行ファイル管理者.ROOTDIRECTORY・実行ファイルがあるフォルダパス + "\\データベース\\サウンドデータベース.csv";
        /// <summary>
        /// ＢＧＭファイル置き場（音楽ファイルがある場所，絶対パス，最後に\\を含む）　※「ＢＧＭ」は全角なので注意。フォルダの名前変更時はここも変えて。
        /// </summary>
        public static string p_BGMDirectory_FullPath・曲フォルダパス = Program・実行ファイル管理者.ROOTDIRECTORY・実行ファイルがあるフォルダパス + "\\データベース\\ＢＧＭ\\";
        /// <summary>
        /// 効果音ファイル置き場（音楽ファイルがある場所，絶対パス，最後に\\を含む）　※フォルダの名前変更時はここも変えて。
        /// </summary>
        public static string p_SEDirectory_FullPath・効果音ファルダパス = Program・実行ファイル管理者.ROOTDIRECTORY・実行ファイルがあるフォルダパス + "\\データベース\\効果音\\";

        /// <summary>
        /// 画像読み込み用（.pngなどの画像ファイルがある場所，絶対パス，最後に\\を含む）
        /// // これを変更する時は覚悟を持って（事前にセーブして）。なんかマニフェストファイルに埋め込まれた画像のところまで変えないといけなくなるし、変えたら変えたでエラーが起こる。
        /// </summary>
        public static string p_ImageDataDirectory・画像フォルダパス = Program・実行ファイル管理者.ROOTDIRECTORY・実行ファイルがあるフォルダパス + "\\データベース\\グラフィック\\";
        

        /// <summary>
        /// このプログラムが終了しているかどうかを全クラスから参照できるようにするための変数です．
        /// </summary>
        public static bool isEnd = false;
        /// <summary>
        /// このプログラムを終了するときの共通処理です。Application.Exit();を呼び出したいメソッドから呼び出してください。
        /// </summary>
        public static void End()
        {
            isEnd = true;
            Application.Exit();
            Thread.CurrentThread.Abort();
        }
        /// <summary>
        /// 今の実行状態がデバッグ用か（コンソールにいろいろ表示・確認をするか）を示します．
        /// </summary>
        public static bool isDebug = true;
        /// <summary>
        /// 警告するためのメッセージボックスを出して，ユーザや開発者に注意を即したいときに呼び出してください．
        /// </summary>
        /// <param name="_warningMessage"></param>
        public static void warning(string _warningMessage)
        {
            if (isDebug == true)
            {
                String _calledMethodName = MyTools.getMethodName(2);
                MessageBox.Show(_warningMessage, "Warning On : "+_calledMethodName);
            }
        }
        /// <summary>
        /// エラーが起きたことをメッセージボックスを出して，ユーザや開発者に注意を即したいときに呼び出してください． 
        /// </summary>
        /// <param name="_ShownMessage・表示したいメッセージ"></param>
        public static void error(string _errorMessage)
        {
            if (isDebug == true)
            {
                String _calledMethodName = MyTools.getMethodName(2);
                MessageBox.Show(_errorMessage, "Error ON : " + _calledMethodName);
            }
        }

        #region ログ表示系
        public static CLog p_log;
        // ■■■ログ表示
        /*
        /// <summary>
        /// ログを出力します．コンソールに表示され，ゲームのログとして格納されます．
        /// </summary>
        public static void printLog(String _ログメッセージ)
        {
            System.Console.WriteLine(_ログメッセージ);
            p_logMessage・ログ += _ログメッセージ;
        }
        /// <summary>
        /// ログを出力します．コンソールに表示され，ゲームのログとして格納されます．
        /// </summary>
        public static void printlnLog(String _一行ログメッセージ)
        {
            System.Console.WriteLine(_一行ログメッセージ);
            p_logMessage・ログ += _一行ログメッセージ + "\n";
        }
        */
        /// <summary>
        /// ログを出力します．標準出力（コンソール）に表示され，ゲームのログとして格納されます．メソッド名も表示されます．
        /// </summary>
        public static void printlnLog(string _ログ文字列)
        {
            CLog.printlnLog(ELogType.l0_標準出力, _ログ文字列);
        }
        /// <summary>
        /// ログを出力します．ログの種類で指定した場所に表示され，ゲームのログとして格納されます．メソッド名も表示されます．
        /// </summary>
        public static void printlnLog(ELogType _ログの種類, string _ログ文字列)
        {
            CLog.printlnLog(_ログの種類, _ログ文字列);
        }
        /// <summary>
        /// ログを出力します．コンソールに表示され，ゲームのログとして格納されます．メソッド名も表示されます．
        /// </summary>
        public static void printLog(ELogType _ログの種類, string _ログ文字列)
        {
            CLog.printLog(_ログの種類, _ログ文字列);
        }
        public static void printLog(ELogType _ログの種類, object _記録したいデータ)
        {
            CLog.printLog(_ログの種類, _記録したいデータ);
        }
        #endregion

    }
}