using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace FTestDebug
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string str = "Hello!";
            Image _image = new Bitmap(100, 100);
            // 普通にデバッガビジュアライザを使う時は、インスタンスにnullでない値を入れてから、ブレークポイントで止める
            _image = new Bitmap(100, 100); // ここをブレークポイント
            // ↑ _imageにマウスカーソルを合わせて、虫眼鏡アイコンを押す。

            // なお、TestShowVisualizerを呼び出すと、実行プログラムとして、実際にフォームがちゃんと表示されるかを確認できる
            BitmapDebugger.ImageDebugger.TestShowVisualizer(_image);
            // しかし、普通にデバッガビジュアライザを使う場合は、参照の追加やこういうことをしなくていい。
            // dllを以下のフォルダにいれるだけ

            //            デバッガービジュアライザ
            // 以下のような、VisualStudioがインストールされているパス（もしくはマイドキュメント）の
            // Visualizersフォルダに***.DLLをコピーすることで利用可能。例１と例２のどちらでもいい
            // 
            //   例１：インストール パス\Microsoft Visual Studio 10.0\Common7\Packages\Debugger\Visualizers
            //   例２：マイドキュメント\Visual Studio 2010\Visualizers
            // 
            // ただし、dllが参照の追加をしている.NETのMicrosoft.VisualStudio.DebuggerVisualizerライブラリは同じ名前でもバージョンが別なので、
            //  VisualStudioのバージョン（2010→10.0、2009→9.0）によって、別々に作らなければならないらしい。
            // これで動かない場合は、それぞれのdllを作成するソースで、参照の追加をそれぞれ作り変えたものを使用する
            // （間違えないように、名前も変えておくとよい）
            //
            //   2010用：　VS2010DebuggerVisualizer_Bitmap.dll
            //   2008用：　VS2008DebuggerVisualizer_Bitmap.dll

            Application.Run(new Form1());
        }
    }
}
