using System;
using System.Collections.Generic;

using System.Text;

// TwitterSample
using System.Diagnostics;
using Twitterizer;
using PublicDomain.Twitter_NET4_0;
using PublicDomain;
//using Twitterizer.Streaming; // Streamingはない？

// _MyClassLibraryプロジェクトを参照追加して。
//using _MyClassLibrary・マイクラスライブラリ;
//using _MyWebGame.プログラム;

namespace PublicDomain.Twitter_NET4_0
{
    /// <summary>
    /// ツイッター試作プログラムのmain関数を持つクラスです。
    /// ※本来ならはツイッタープログラムは別のプロジェクトにするべきなのですが、
    /// 複数のプロジェクト管理は面倒なため、現段階ではmain関数を変えることで変更しています。
    /// 
    /// ■このツイッター試作プログラムを実行したい場合は、
    // プロジェクト右クリック→「プロパティ」→「スタートアッププログラム」をこれに設定してください。
    /// </summary>
    public class ProgramTwitter・ツイッター試作プログラム実行管理者
    {
        public static bool p_isRunTwitterSample = true;
        /// <summary>
        /// 何でも実験してください。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //_CArtofLife・人生芸術 _AoL = new _CArtofLife・人生芸術();
            // このクラスは出力するだけ。まだみじっそう
            //while(true){
            //    printf(_Art.nextMessage());
            //}

            if (p_isRunTwitterSample == true)
            {

                MyTwitterTools.__HELP・このクラスの使い方のヘルプ();
            }
        }
    }
}
