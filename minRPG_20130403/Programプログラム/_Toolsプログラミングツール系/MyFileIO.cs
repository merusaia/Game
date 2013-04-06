// デバッグ時にコメントアウト
//#define Debug

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;// Note:VisualBasic.NETの機能を使うInteractionモジュールを使うため．※必ず．「プロジェクト」→「参照の追加」→「Microsoft Visual Basic.NET (Ver1.1以前はRuntime)」のdllを追加する
using System.Threading; 

// [ToDo]: ディレクトリパスを絶対値指定する、staticな読み書きメソッドを作る

namespace PublicDomain
{
    /// <summary>
    /// ファイルの読み書き処理の雛形を持つクラスです．
    /// </summary>
    public class MyFileIO
    {
        // 定数
        // 改行を示す変数を簡単に！
        // Note: string型はconstにできない，文字定数はどうやって宣言する？ → staticにする。クラスで一つだけ存在可能で、編集可能。インスタンスを持ってもメモリ上に複製されない。
        /// <summary>
        /// 改行コードを指します。System.Environment.NewLine;
        /// </summary>
        public static string _n = System.Environment.NewLine;

        // 文字コードの説明（これでいちいち指定すると変更箇所が紛らわしいので、実際に使う時はp_EncodingType_DEFAULTか、getEncodingTypeName()メソッドで取るといいかも）
        /// <summary>
        /// VisualStudio2005以降のソースコードのデフォルトの文字エンコードです。「utf-8」
        /// </summary>
        private static string ENCODING_DEFAULT_VisualStudioSouce = "utf-8";
        /// <summary>
        /// Windowsのデフォルトの文字エンコードです。「Shift-JIS」
        /// </summary>
        private static string ENCODING_SHIFT_JIS = "Shift-JIS";
        /// <summary>
        /// インターネット上のデファクトスタンダードの文字エンコードです。「utf-8」
        /// </summary>
        private static string ENCODING_UNICODE_UTF_8 = "utf-8"; // BOMをつけるかつけないかで変わる？（VS2005からUTF-8シグネチャ付き（BOM））
        private static string ENCODING_UNICODE_UTF_16 = "utf-16"; // 細かくはBOM（Byte Over Mark）をつける場合、UTF16BE, UTF16LEがある
        private static string ENCODING_EUC_JP = "Euc-JP"; // 細かくはBOM（Byte Over Mark）をつける場合、UTF16BE, UTF16LEがある

        // ファイルの文字列エンコード、クラス内のメソッドが使うデフォルトのエンコードの定義（●現在は「UTF-8」をデフォルトにしている）
        #region ■よく使う文字列エンコード名の取得、デフォルトのエンコード名の定義: getEncodingTypeName
        /// <summary>
        /// ="utf-8"。このクラス内のメソッドを使う時に使う、デフォルトの文字エンコードです。 
        /// </summary>
        public static EEncodingType p_EncodingType_DEFAULT = EEncodingType.e01_UTF8・ネット上のデファクトスタンダード;
        /// <summary>
        /// ="utf-8"。このクラス内のメソッドを使う時に使う、デフォルトの文字エンコードです。 
        /// </summary>
        private static string ENCODING_DEFAULT = getEncodingTypeName(p_EncodingType_DEFAULT); // = "utf-8"
        // こういう書き方もできるが、念のためデファクトスタンダードのUTF-8に統一しておく。=System.Text.Encoding.Default
        /// <summary>
        /// よく使う文字コードを指定したものです。
        /// </summary>
        public enum EEncodingType
        {
            e00_unknown・不明,
            e01_UTF8・ネット上のデファクトスタンダード,
            e02_SHIFTJIS・ウィンドウズのテキストファイルのデフォルト,
            e03_UTF16・ウィンドウズの一部内部で使われているエンコードだがあまり使わないで,
            e04_others・その他のエンコード,
            // 以下、メモ
            m01_VisualStudioSource・ＶＳ2005以降のソースコードのデフォルトはＵＴＦ８,
        }
        /// <summary>
        /// 引数の列挙体に指定した文字コードの参照名_encodingTypeNameをstring型で取得します。
        /// 
        /// （例：　System.Text.Encoding.GetEncoding(MyTools.getEncodingTypeName(EEncodingType.***))）
        /// </summary>
        public static string getEncodingTypeName(EEncodingType _EEncodingType)
        {
            string _encodingTypeName = "unknown";
            switch (_EEncodingType)
            {
                case EEncodingType.e00_unknown・不明:
                    _encodingTypeName = "unknown"; break;
                case EEncodingType.e01_UTF8・ネット上のデファクトスタンダード:
                    _encodingTypeName = "utf-8"; break;
                case EEncodingType.e02_SHIFTJIS・ウィンドウズのテキストファイルのデフォルト:
                    _encodingTypeName = "Shift-JIS"; break;
                case EEncodingType.e03_UTF16・ウィンドウズの一部内部で使われているエンコードだがあまり使わないで:
                    _encodingTypeName = "utf-16"; break;
                case EEncodingType.m01_VisualStudioSource・ＶＳ2005以降のソースコードのデフォルトはＵＴＦ８:
                    _encodingTypeName = "utf-8"; break;
                default:
                    _encodingTypeName = "unknown"; break;
            }
            return _encodingTypeName;
        }
        #endregion
        #region 文字列⇔バイトデータの変換: getString_ByBytes / getBytes_ByString
        // 先に、文字列⇔バイトデータの変換
        /// <summary>
        /// 引数のバイトデータを、文字列に変換します。
        /// 
        /// ※主に、各種ファイルやWebから取得したバイトコードの文字コード判別に使います。
        /// なお、プログラムソース中のstring型/byte[]型に格納されている文字列"***"の文字コードは、
        /// 基本的にUnicode型（おそらくUTF-8）で統一されています。
        /// </summary>
        public static string getString_ByBytes(byte[] _bytes)
        {
            return getString_ByBytes(EEncodingType.e01_UTF8・ネット上のデファクトスタンダード, _bytes);
        }
        /// <summary>
        /// 引数のバイトデータを、文字列に変換します。
        /// 
        /// ※主に、各種ファイルやWebから取得したバイトコードの文字コード判別に使います。
        /// なお、プログラムソース中のstring型/byte[]型に格納されている文字列"***"の文字コードは、
        /// 基本的にUnicode型（おそらくUTF-8）で統一されています。
        /// </summary>
        public static string getString_ByBytes(EEncodingType _EEncodingType, byte[] _bytes)
        {
            string _string = "";
            string _encodingName = getEncodingTypeName(_EEncodingType);
            if (_encodingName != "unknown")
            {
                _string = System.Text.Encoding.GetEncoding(_encodingName).GetString(_bytes);
            }
            return _string;
        }
        /// <summary>
        /// 引数の文字列を、バイトデータに変換します。
        /// </summary>
        public static byte[] getBytes_ByString(string _string)
        {
            return getBytes_ByString(EEncodingType.e01_UTF8・ネット上のデファクトスタンダード, _string);
        }
        /// <summary>
        /// 引数の文字列を、バイトデータに変換します。
        /// 
        /// ※主に、各種ファイルやWebから取得したバイトコードの文字コード判別に使います。
        /// なお、プログラムソース中のstring型/byte[]型に格納されている文字列"***"の文字コードは、
        /// 基本的にUnicode型（おそらくUTF-8）で統一されています。
        /// </summary>
        public static byte[] getBytes_ByString(EEncodingType _EEncodingType, string _string)
        {
            byte[] _bytes = null;
            string _encodingName = getEncodingTypeName(_EEncodingType);
            if (_encodingName != "unknown")
            {
                _bytes = System.Text.Encoding.GetEncoding(_encodingName).GetBytes(_string);
            }
            return _bytes;
        }
        #endregion
        #region 文字コードの判別: getEncoding
        // ソース引用。感謝。　http://dobon.net/vb/dotnet/string/detectcode.html
        /// <summary>
        /// 引数のバイトデータの文字コードを判別して、System.Text.Enocding型で返します。
        /// 
        /// ※主に、各種ファイルやWebから取得したバイトコードの文字コード判別に使います。
        /// なお、プログラムソース中のstring型/byte[]型に格納されている文字列"***"の文字コードは、
        /// 基本的にUnicode型（おそらくUTF-8）で統一されているため、特に気にする必要はありません。
        /// </summary>
        /// <remarks>
        /// Jcode.pmのgetcodeメソッドを移植したものです。
        /// Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
        /// Jcode.pmのCopyright: Copyright 1999-2005 Dan Kogai
        /// </remarks>
        /// <param name="bytes">文字コードを調べるデータ</param>
        /// <returns>適当と思われるEncodingオブジェクト。
        /// 判断できなかった時はnull。</returns>
        public static System.Text.Encoding GetEncoding(byte[] bytes)
        {
            const byte bEscape = 0x1B;
            const byte bAt = 0x40;
            const byte bDollar = 0x24;
            const byte bAnd = 0x26;
            const byte bOpen = 0x28;    //'('
            const byte bB = 0x42;
            const byte bD = 0x44;
            const byte bJ = 0x4A;
            const byte bI = 0x49;

            int len = bytes.Length;
            byte b1, b2, b3, b4;

            //Encode::is_utf8 は無視

            bool isBinary = false;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 <= 0x06 || b1 == 0x7F || b1 == 0xFF)
                {
                    //'binary'
                    isBinary = true;
                    if (b1 == 0x00 && i < len - 1 && bytes[i + 1] <= 0x7F)
                    {
                        //smells like raw unicode
                        return System.Text.Encoding.Unicode;
                    }
                }
            }
            if (isBinary)
            {
                return null;
            }

            //not Japanese
            bool notJapanese = true;
            for (int i = 0; i < len; i++)
            {
                b1 = bytes[i];
                if (b1 == bEscape || 0x80 <= b1)
                {
                    notJapanese = false;
                    break;
                }
            }
            if (notJapanese)
            {
                return System.Text.Encoding.ASCII;
            }

            for (int i = 0; i < len - 2; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                b3 = bytes[i + 2];

                if (b1 == bEscape)
                {
                    if (b2 == bDollar && b3 == bAt)
                    {
                        //JIS_0208 1978
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bDollar && b3 == bB)
                    {
                        //JIS_0208 1983
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && (b3 == bB || b3 == bJ))
                    {
                        //JIS_ASC
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    else if (b2 == bOpen && b3 == bI)
                    {
                        //JIS_KANA
                        //JIS
                        return System.Text.Encoding.GetEncoding(50220);
                    }
                    if (i < len - 3)
                    {
                        b4 = bytes[i + 3];
                        if (b2 == bDollar && b3 == bOpen && b4 == bD)
                        {
                            //JIS_0212
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                        if (i < len - 5 &&
                            b2 == bAnd && b3 == bAt && b4 == bEscape &&
                            bytes[i + 4] == bDollar && bytes[i + 5] == bB)
                        {
                            //JIS_0208 1990
                            //JIS
                            return System.Text.Encoding.GetEncoding(50220);
                        }
                    }
                }
            }

            //should be euc|sjis|utf8
            //use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
            int sjis = 0;
            int euc = 0;
            int utf8 = 0;
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0x81 <= b1 && b1 <= 0x9F) || (0xE0 <= b1 && b1 <= 0xFC)) &&
                    ((0x40 <= b2 && b2 <= 0x7E) || (0x80 <= b2 && b2 <= 0xFC)))
                {
                    //SJIS_C
                    sjis += 2;
                    i++;
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if (((0xA1 <= b1 && b1 <= 0xFE) && (0xA1 <= b2 && b2 <= 0xFE)) ||
                    (b1 == 0x8E && (0xA1 <= b2 && b2 <= 0xDF)))
                {
                    //EUC_C
                    //EUC_KANA
                    euc += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if (b1 == 0x8F && (0xA1 <= b2 && b2 <= 0xFE) &&
                        (0xA1 <= b3 && b3 <= 0xFE))
                    {
                        //EUC_0212
                        euc += 3;
                        i += 2;
                    }
                }
            }
            for (int i = 0; i < len - 1; i++)
            {
                b1 = bytes[i];
                b2 = bytes[i + 1];
                if ((0xC0 <= b1 && b1 <= 0xDF) && (0x80 <= b2 && b2 <= 0xBF))
                {
                    //UTF8
                    utf8 += 2;
                    i++;
                }
                else if (i < len - 2)
                {
                    b3 = bytes[i + 2];
                    if ((0xE0 <= b1 && b1 <= 0xEF) && (0x80 <= b2 && b2 <= 0xBF) &&
                        (0x80 <= b3 && b3 <= 0xBF))
                    {
                        //UTF8
                        utf8 += 3;
                        i += 2;
                    }
                }
            }
            //M. Takahashi's suggestion
            //utf8 += utf8 / 2;

            System.Diagnostics.Debug.WriteLine(
                string.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8));
            if (euc > sjis && euc > utf8)
            {
                //EUC
                return System.Text.Encoding.GetEncoding(51932);
            }
            else if (sjis > euc && sjis > utf8)
            {
                //SJIS
                return System.Text.Encoding.GetEncoding(932);
            }
            else if (utf8 > euc && utf8 > sjis)
            {
                //UTF8
                return System.Text.Encoding.UTF8;
            }

            return null;
        }
        #endregion

        // ファイルの書き込み・読み込み
        #region シンプルなファイル読み書きメソッド（※■ここだけSystem.IO依存）
        // ■バイトデータ型（テキストファイル以外はこれが基本）
        /// <summary>
        /// バイトデータをファイルに書き込みます。返り値は、「ファイルが存在したか」を返します。
        /// ファイルが存在しない場合、ファイルを新規作成して書き込みます。ファイルが既に存在している場合は、上書きします。
        /// 例外が発生しても確実にファイルを閉じてくれます。
        /// </summary>
        public static bool WriteFile_AllBytes(string _fileName_FullPath, byte[] _bytes)
        {
            bool _isExist = false;
            if (isExist(_fileName_FullPath) == true)
            {
                //バイト配列をファイルにすべて書き込む
                System.IO.File.WriteAllBytes(_fileName_FullPath, _bytes);
            }
            else
            {
                _isExist = false;
                //バイト配列をファイルにすべて書き込む
                System.IO.File.WriteAllBytes(_fileName_FullPath, _bytes);
            }
            return _isExist;
        }
        /// <summary>
        /// ファイルのバイトデータを読み込みます。ファイルが存在しない場合、nullを返します。
        /// 例外が発生しても確実にファイルを閉じてくれます。
        /// </summary>
        public static byte[] ReadFile_AllBytes(string _fileName_FullPath)
        {
            byte[] _bytes = null;
            if (isExist(_fileName_FullPath) == true)
            {
                //ファイルの内容をバイト配列にすべて読み込む
                _bytes = System.IO.File.ReadAllBytes(_fileName_FullPath);
            }
            return _bytes;
        }


        // ■string型（テキストファイルはこれを使う。内部で、バイトデータ型を呼び出しているものもある）
        /// <summary>
        /// 引数ファイルのテキストを（文字コードを自動変換して）読み込み、文字列を返します。
        /// ファイルが存在しない場合、""を返します。
        /// </summary>
        public static string ReadFile(string _readFileName_FullPath)
        {
            string _readString = "";
            // とりあえずbyte[]型で、どんなエンコードのファイルでも取る
            byte[] _bytes = ReadFile_AllBytes(_readFileName_FullPath);
            if (_bytes != null)
            {
                // 文字コードを取得
                if (MyTools.GetEncoding(_bytes) == System.Text.Encoding.UTF8 || MyTools.GetEncoding(_bytes) == System.Text.Encoding.ASCII)
                {
                    // UTF-8を優先的に使う
                    _readString = MyTools.getString_ByBytes(MyTools.EEncodingType.e01_UTF8・ネット上のデファクトスタンダード, _bytes);
                }
                else if (MyTools.GetEncoding(_bytes) == System.Text.Encoding.GetEncoding(MyTools.getEncodingTypeName(MyTools.EEncodingType.e02_SHIFTJIS・ウィンドウズのテキストファイルのデフォルト)))
                {
                    // Shift-JIS
                    _readString = MyTools.getString_ByBytes(MyTools.EEncodingType.e02_SHIFTJIS・ウィンドウズのテキストファイルのデフォルト, _bytes);
                }
                else
                {
                    ConsoleWriteLine("ReadFile: 文字コードがUTF-8、Shift-JIS以外のファイルを読み込みました。とりあえず、UTF-16に変換しておきます。");
                    // それ以外はよくわからないから、とりあえずUTF-16で変換して返す。
                    // ([MEMO]なんかウィンドウズや.NETではUTF-16を単に「Unicode」と呼んでいることが多い。ネットのスタンダードはUTF-8なのに。紛らわしいからやめて欲しい)
                    _readString = System.Text.Encoding.Unicode.GetString(_bytes);
                }
            }
            return _readString;
        }
        /// <summary>
        /// 引数ファイルのテキストを読み込み、文字列を一行ごとにリストに格納して返します。
        /// </summary>
        /// <param name="readFileName"></param>
        /// <returns></returns>
        public static List<string> ReadFile_ToLists(string readFile_FullPath)
        {
            string readData = ReadFile(readFile_FullPath);
            string[] readDataLines = readData.Split(System.Environment.NewLine.ToCharArray()); // (_n.ToCharArray()); // [ToDo]環境に寄らず，ちゃんと一行ずつ区切れてる？
            List<string> readLists = new List<string>(readDataLines);
            return readLists;
        }


        // ■エンコードを指定可能なstring型（今は使っていない）
        /// <summary>
        /// 引数ファイルのテキストを読み込み、文字列を返します。
        /// 基本のReadはReadFile_AllBytesを使ってエンコードを自動判別しているので、あまり使っていない。
        /// ただ、基本的にファイルの文字コードがわかっている場合は、この処理の方がメモリ消費も少ないし、早い。
        /// </summary>
        /// <param name="readFileName"></param>
        /// <returns></returns>
        private static string ReadFile_EncodingTypeDefined(string readFile_FullPath, string _encodingTypeName)
        {
            string readText = "";
            try
            {
                //読み込みファイル名指定（IntPtr型は @+string型で表現可能）
                FileStream fs = new FileStream(readFile_FullPath, FileMode.Open);
                //読み込みストリーム生成
                StreamReader reader = new StreamReader(fs, System.Text.Encoding.GetEncoding(_encodingTypeName));  // shift_jis")

                //全て読み込み
                readText = reader.ReadToEnd();


                //ファイルのロックを解除
                reader.Close();
                reader.Dispose();
                fs.Dispose();
                // デバッグ表示用
                string shownReadText = readText.Substring(0, Math.Min(10, readText.Length));
                Console.WriteLine(readFile_FullPath + "から，\n" + shownReadText + "\nを読み込みました．");
            }
            catch (IOException e)
            {
                MessageBox.Show("ファイル読み込みエラーです．\n" + e.ToString());
            }
            finally
            {
            }
            return readText;
        }




        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile(string writeFile_FullPath, List<string> writeText)
        {
            return WriteFile(writeFile_FullPath, writeText, ENCODING_DEFAULT);
        }
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile(string writeFile_FullPath, List<string> writeText, string _encodingTypeName)
        {
            string data = "";
            for (int i = 0; i < writeText.Count; i++)
            {
                data += writeText[i] + _n;
            }
            return WriteFile(writeFile_FullPath, data, _encodingTypeName);
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile(string writeFile_FullPath, string writeText)
        {
            return WriteFile(writeFile_FullPath, writeText, ENCODING_DEFAULT);
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile(string writeFile_FullPath, string writeText, string _encodingTypeName)
        {
            bool isOk = true;
            try
            {
                //保存ファイル名指定（Stream型は @+string型）
                FileStream fs = new FileStream(writeFile_FullPath, FileMode.Create); // CreateNewは上書き禁止
                //書き込みストリーム生成
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding(_encodingTypeName)); // shift_jis")


                // 書き込みテキストの指定
                writer.WriteLine(writeText);

                //ファイルへ書き込み
                writer.Flush();

                //ファイルのロックを解除
                writer.Close();

                // デバッグ表示用
                string shownWriteText = writeText.Substring(0, Math.Min(10, writeText.Length));
                Console.WriteLine(writeFile_FullPath + "に，\n" + shownWriteText + "\nを書き込みました．");
            }
            catch (IOException e)
            {
                isOk = false;
                MessageBox.Show("ファイル書き込みエラーです．ファイル名" + writeFile_FullPath + "\n" + e.ToString());

            }
            finally
            {
            }
            return isOk;
        }
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        public static bool WriteFile_NoOverWrite(string writeFile_FullPath, List<string> writeText)
        {
            return WriteFile_NoOverWrite(writeFile_FullPath, writeText, ENCODING_DEFAULT);
        }
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile_NoOverWrite(string writeFile_FullPath, List<string> writeText, string _encodingTypeName)
        {
            string data = "";
            for (int i = 0; i < writeText.Count; i++)
            {
                data += writeText[i] + _n;
            }
            return WriteFile(writeFile_FullPath, data, _encodingTypeName);
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile_NoOverWrite(string writeFile_FullPath, string writeText)
        {
            return WriteFile_NoOverWrite(writeFile_FullPath, writeText, ENCODING_DEFAULT);
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile_NoOverWrite(string writeFile_FullPath, string writeText, string _encodingTypeName)
        {
            bool isOk = true;
            try
            {
                //保存ファイル名指定（Stream型は @+string型）
                FileStream fs = new FileStream(writeFile_FullPath, FileMode.CreateNew); // CreateNewは上書き禁止
                //書き込みストリーム生成
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding(_encodingTypeName)); // shift_jis")


                // 書き込みテキストの指定
                writer.WriteLine(writeText);

                //ファイルへ書き込み
                writer.Flush();

                //ファイルのロックを解除
                writer.Close();

                // デバッグ表示用
                string shownWriteText = writeText.Substring(0, Math.Min(10, writeText.Length));
                Console.WriteLine(writeFile_FullPath + "に，\n" + shownWriteText + "\nを書き込みました．");
            }
            catch (IOException e)
            {
                isOk = false;
                if (writeFile_FullPath.Contains("(2)") == false)
                {
                    MessageBox.Show("ファイル書き込みエラーです．同じ名前のファイルに上書きしようとしているかもしれません．\n念のため，異なるファイル名" + writeFile_FullPath + "(2)" + "に保存します．\n" + e.ToString());
                    // もう一度だけ名前を変えてセーブ
                    return WriteFile(writeFile_FullPath + "(2)", writeText);
                }
                else
                {
                    MessageBox.Show("ファイル書き込みエラーです．異なるファイル名" + writeFile_FullPath + "(2)" + "で保存できませんでした．\nディレクトリ名が間違っているかもしれません．" + e.ToString());
                }
            }
            finally
            {
            }
            return isOk;
        }
        /// <summary>
        /// 引数ファイルにテキストを追加保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile_Append(string writeFileName_FullPath, string writeText)
        {
            return WriteFile_Append(writeFileName_FullPath, writeText, ENCODING_DEFAULT);
        }
        /// <summary>
        /// 引数ファイルにテキストを追加保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile_Append(string writeFileName_FullPath, string writeText, string _encodingTypeName)
        {
            bool isOk = true;
            try
            {
                // Appendなので，処理中にファイルが使えなくなる危険を防ぐため，念のためファイルをコピー
                File.Copy(writeFileName_FullPath, writeFileName_FullPath + "copy", true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Append:コピー: ファイル名「" + writeFileName_FullPath + "」が見つからなかったようです．");
            }
            finally { }
            try
            {


                //保存ファイル名指定（Stream型は @+string型）
                FileStream fs = new FileStream(writeFileName_FullPath, FileMode.Append); // Appendは追加書き込み
                //書き込みストリーム生成
                StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding(_encodingTypeName)); // shift_jis")

                // 書き込みテキストの指定
                writer.WriteLine(writeText);

                //ファイルへ書き込み
                writer.Flush();

                //ファイルのロックを解除
                writer.Close();

                // デバッグ表示用
                string shownWriteText = writeText.Substring(0, Math.Min(10, writeText.Length));
                Console.WriteLine(writeFileName_FullPath + "に，\n" + shownWriteText + "\nを書き込みました．");

                // 消去しなくても同じ名前は上書きされる
                // Appendで，念のため作成したファイルを消去
                //File.Delete(_writeFileName_FullPath + "copy");
            }
            catch (IOException e)
            {
                isOk = false;
                MessageBox.Show("Append:ファイル書き込みエラーです．ファイル名" + writeFileName_FullPath + "\n" + e.ToString());

            }
            finally
            {
            }
            return isOk;
        }
        #endregion

        #region 過去のもの
        /*
        public static bool WriteFile(string _writeFileName_FullPath, string _writeText)
        {
            bool isOk = true;
            try
            {
                    //保存ファイル名指定（Stream型は @+string型）
                    FileStream fs = new FileStream(_writeFileName_FullPath, FileMode.CreateNew); // CreateNewは上書き禁止
                    //書き込みストリーム生成
                    StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding(ENCODING_DEFAULT)); // shift_jis")

                    // 書き込みテキストの指定
                    writer.WriteLine(_writeText);

                    //ファイルへ書き込み
                    writer.Flush();

                    //ファイルのロックを解除
                    writer.Close();

                    // デバッグ表示用
                    string shownWriteText = _writeText.Substring(0, Math.Min(10, _writeText.Length));
                    Console.WriteLine(_writeFileName_FullPath + "に，" + _n + shownWriteText + "を書き込みました．");
            }
            catch (IOException _e)
            {
                isOk = false;
                MessageBox.Show("ファイル書き込みエラーです．同じ名前のファイルに上書きしようとしているかもしれません．\n" + _e.ToString());
            }
            finally
            {
            }
            return isOk;
        }
        */
        #endregion


        /// <summary>
        /// ルートディレクトリの絶対パス
        /// </summary>
        protected string rootDirectory = ""; // ルートディレクトリの絶対パス
        protected string newDirectoryName = ""; // 新しく作成したディレクトリの名前（未作成時は""）（ルートディレクトリの最後のディレクトリ）
        /// <summary>
        /// セーブ用のファイルを作ってセーブするならtrue
        /// </summary>
        protected bool canSave = true; // ファイルを保存するかどうか

        protected bool isOpen = false; // 一行書き込み中のファイルを開いているかどうか
        protected string writeLine_FileName = ""; // 一行書き込み中のファイル名
        protected FileStream writeLine_fs;
        protected StreamWriter writeLine_writer;
        protected string readLine_FileName = ""; // 一行読み取り中のファイル名
        protected FileStream readLine_fs;
        protected StreamReader readLine_reader;

        /// <summary>
        /// マルチスレッド対応ファイル書き込みのスレッド
        /// </summary>
        protected Thread thread_WriteLine;
        protected string thread_WriteLine_FileName = "";
        protected List<string> thread_WriteLine_WriteText = new List<string>();
        protected int thread_WriteLine_WrritenLineNum = 0;
        protected bool thread_WriteLine_isEnd = false;
        protected bool thread_WriteLine_isOpen = false;
        protected FileStream thread_WriteLine_fs;
        protected StreamWriter thread_WriteLine_writer;
        /// <summary>
        /// マルチスレッド対応ファイル読み込みのスレッド
        /// </summary>
        protected Thread thread_ReadLine;
        protected string thread_ReadLine_FileName = "";
        protected List<string> thread_ReadLine_ReadText = new List<string>();
        protected bool thread_ReadLine_isEnd = false;
        protected bool thread_ReadLine_isOpen = false;
        protected FileStream thread_ReadLine_fs;
        protected StreamReader thread_ReadLine_reader;

        protected MyTime mytime; // コンストラクタ呼び出しからの経過時間などを扱うクラス

        /// <summary>
        /// コンストラクタでは，第1引数で保存先ルートディレクトリを設定します（"C:\\root"など、最後の「\\」は要らない、存在しない場合は新規作成する）。第２引数では、ディレクトリの中に保存先ファイルを格納する新しいディレクトリを作成するかを指定します(false:直接格納)。第３引数では、その新しいディレクトリの名前をコンストラクタ起動時にユーザが指定するかを指定します(false:日付になる)。第１引数を指定しない場合はデフォルト"C:\\MyFileIO"になります．
        /// </summary>
        /// <param name="_rootDerectoryPath"></param>
        public MyFileIO(string rootDerectoryPath, string encodingName, bool isCreateNewDirectory, bool newDirectoryIsNamed_ByUser)
        :this(rootDerectoryPath, isCreateNewDirectory, newDirectoryIsNamed_ByUser)
        {
            ENCODING_DEFAULT = encodingName;
        }
        /// <summary>
        /// コンストラクタでは，第1引数で保存先ルートディレクトリを設定します（"C:\\root"など、最後の「\\」は要らない、存在しない場合は新規作成する）。第２引数では、ディレクトリの中に保存先ファイルを分けて格納する新しいディレクトリを作成するかを指定します(false:直接格納)。第３引数では、その新しいディレクトリの名前をコンストラクタ起動時にユーザが指定するかを指定します(false:日付になる)。第１引数を指定しない場合はデフォルト"C:\\MyFileIO"になります．
        /// </summary>
        /// <param name="_rootDerectoryPath"></param>
        public MyFileIO(string _rootDerectoryPath, bool _isCreateNewDirectory, bool _isNewDirectoryNamed_ByUser)
        {
            if(_rootDerectoryPath.Equals("")==true){
                _rootDerectoryPath = "C:";
            }
            rootDirectory = _rootDerectoryPath;

            // ルートディレクトリが無ければ作成
            if (Directory.Exists(rootDirectory) == false)
            {
                // ルートディレクトリの作成
                Directory.CreateDirectory(rootDirectory);
            }

            // 新しいディレクトリを作成するか
            if (_isCreateNewDirectory == true)
            {
                // デフォルトの新しいディレクトリの名前
                newDirectoryName = DateTime.Now.ToString("yyyy年MM月dd日_HH時mm分ss秒");

                
                // ユーザがデータファイルを格納するディレクトリの名前を入力するか
                if (_isNewDirectoryNamed_ByUser == true)
                {
                    // Note: C#にはInputBoxクラスが無いが，C#/VB.NETは標準.NET Frameworkに含まれるので，以下のようVB.NETランタイムを参照設定するだけで，InteractionモジュールでVB.NETのコンポーネントを使用できる．
                    // Note: .NET TIPS VB.NET固有の関数をC#で使用するには？ http://www.atmarkit.co.jp/fdotnet/dotnettips/254vbfunc/vbfunc.html
                    // Memo: VisualBasic.NETの機能を使うInteractionモジュールを使うため．※必ず．「プロジェクト」→「参照の追加」→「.NET」タブの「Microsoft Visual Basic.NET (Ver1.1以前はRuntime)」のdllを追加する

                    // Note: VB.NETはメソッドの引数を省略可能だが，VC#では省略できないので，規定値（デフォルト）を入力する必要がある
                    newDirectoryName = Microsoft.VisualBasic.Interaction.InputBox("このプログラムで保存されるデータを格納するディレクトリの名前を付けてください．" + _n + "なお，作成元のルートディレクトリは，" + this.rootDirectory + "\\　です．", "保存先ディレクトリの名前入力", newDirectoryName, -1, -1); // 最後の引数-1,-1は省略(600, 350)

                }
                if (newDirectoryName.Equals("") == false)
                {
                    // 新しいディレクトリの作成
                    Directory.CreateDirectory(rootDirectory + "\\" + newDirectoryName);
                    // ルートディレクトリの変更
                    rootDirectory = rootDirectory + "\\" + newDirectoryName;
                }
                else
                {
                    // 全てのファイル書き込み操作を無効にする
                    canSave = false;
                    MessageBox.Show("データは保存されません．");
                    // Application.Exit(); // Note: Application.Exit()では，Formクラス名_Closing／FormClosingメソッドは呼ばれない！！
                    //ToDo: 呼ばれ元クラスの実行を停止する？
                }

                // このコンストラクタが呼び出されてから（アプリ起動してから）の時間を計る
                mytime = new MyTime();
            }
        }
        /// <summary>
        ///  引数無しのコンストラクタでは、保存先ルートディレクトリは"C:/MyFileIO_test"になります。　
        /// </summary>
        public MyFileIO()
            : this(MyTools.getProjectDirectory() + "\\MyFileIO_test", false, false)
        {

        }
        /// <summary>
        ///  このルートディレクトリ上で読み込みしているファイルを全て閉じる
        /// </summary>
        public void writeLine_close()
        {
            //ファイルのロックを解除
            writeLine_writer.Close();
            writeLine_writer.Dispose();
            writeLine_fs.Close();
            writeLine_fs.Dispose();
            writeLine_FileName = "";
            isOpen = false;
        }


        // 出力処理
        /// <summary>
        /// デバッグ用のコンソール出力を表示するかどうかです。デフォルトではtrueです。falseにしたい場合はこの変数を直接変更してください。
        /// </summary>
        public static bool p_isDebug_WriteConsole = true;
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
            if (p_isDebug_WriteConsole == true)
            {
                Console.WriteLine(_message);
            }
        }
        
        #region シンプルなファイル読み書き機能メソッド（pravateメソッド。string型 ※エンコードはデフォルト）
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public bool writeFile_simple(string writeFileName, List<string> writeText)
        {
            string data = "";
            foreach(string _value in writeText)
            {
                data += _value + _n;
            }
            return writeFile_simple(writeFileName, data);
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public bool writeFile_simple(string writeFileName, string writeText)
        {
            if (canSave == true)
            {
                return WriteFile(rootDirectory + "\\" + writeFileName, writeText);
            }
            else
            {
                return true;
            }
        }
        #region 過去のもの
        /*
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public bool writeFile_list(string _writeFileName_FullPath, List<string> _writeText)
        {
            bool isOk = true;
            try
            {
                if (canSave == true)
                {
                    //保存ファイル名指定（Stream型は @+string型）
                    FileStream fs = new FileStream(rootDirectory + "\\" + _writeFileName_FullPath, FileMode.CreateNew); // CreateNewは上書き禁止
                    //書き込みストリーム生成
                    StreamWriter writer = new StreamWriter(fs, System.Text.Encoding.GetEncoding(ENCODING_DEFAULT)); // shift_jis")

                    // 書き込みテキストの指定
                    foreach (string _writeText in _writeText)
                    {
                        writer.WriteLine(_writeText);
                    }

                    //ファイルへ書き込み
                    writer.Flush();

                    //ファイルのロックを解除
                    writer.Close();

                    // デバッグ表示用
                    //string shownWriteText = _writeText.Substring(0, Math.Min(10, _writeText.Length));
                    //Console.WriteLine(rootDirectory + "\\" + _writeFileName_FullPath + "に，" + _n + shownWriteText + "を書き込みました．");
                }
            }
            catch (IOException _e)
            {
                isOk = false;
                if (_writeFileName_FullPath.Contains("(2)") == false)
                {
                    MessageBox.Show("ファイル書き込みエラーです．同じ名前のファイルに上書きしようとしているかもしれません．\n念のため，異なるファイル名" + _writeFileName_FullPath + "(2)" + "に保存します．\n" + _e.ToString());
                    // もう一度だけ名前を変えてセーブ
                    return writeFile_simple(_writeFileName_FullPath + "(2)", _writeText);
                }
                else
                {
                    MessageBox.Show("ファイル書き込みエラーです．異なるファイル名" + _writeFileName_FullPath + "(2)" + "で保存できませんでした．\nディレクトリ名が間違っているかもしれません．" + _e.ToString());
                }
            }
            finally
            {
            }
            return isOk;
        }
         * */
        #endregion
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public bool writeFile_NoOverwrite(string writeFileName, List<string> writeText)
        {
            string data = "";
            foreach (string _value in writeText)
            {
                data += _value + _n;
            }
            return writeFile_simple(writeFileName, data);
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public bool writeFile_NoOverwrite(string writeFileName, string writeText)
        {
            if (canSave == true)
            {
                return WriteFile(rootDirectory + "\\" + writeFileName, writeText);
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 引数ファイルにテキストリストを改行付きでを追加保存します。ファイルが存在しない場合は新規作成します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public bool writeFile_Append(string writeFileName, List<string> writeText)
        {
            string data = "";
            foreach (string _value in writeText)
            {
                data += _value + _n;
            }
            return writeFile_Append(writeFileName, data);
        }
        /// <summary>
        /// 引数ファイルにテキストを追加保存します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public bool writeFile_Append(string writeFileName, string writeText)
        {
            if (canSave == true)
            {
                return WriteFile_Append(rootDirectory + "\\" + writeFileName, writeText);
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 引数ファイルのテキストを読み込み、文字列を一行ごとにリストに格納して返します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="readFileName"></param>
        /// <returns></returns>
        public List<string> readFile_ToLists(string readFileName)
        {
            string readData = readFile_simple(readFileName);
            string[] readDataLines = readData.Split(_n.ToCharArray()); //_n.ToCharArray()); // [ToDo]環境に寄らず，ちゃんと一行ずつ区切れてる？
            List<string> readLists = new List<string>(readDataLines);
            return readLists;
        }
        /// <summary>
        /// 引数ファイルのテキストを読み込み、文字列を返します。ディレクトリパスはコンストラクタで指定済みなので必要ありません。
        /// </summary>
        /// <param name="readFileName"></param>
        /// <returns></returns>
        public string readFile_simple(string readFileName)
        {
            return ReadFile_EncodingTypeDefined(rootDirectory + "\\" + readFileName, ENCODING_DEFAULT);
        }
        #endregion

        #region [チェック][保存]:ファイルの保存を消すかチェックするメソッド
        /// <summary>
        /// ファイルの保存を消すかチェックするダイアログを表示して処理するメソッド
        /// </summary>
        public bool checkReallySaved(bool _isSkipMessage_CheckReallySaveOrNot)
        {
            bool _reallySaved = false;
            if (canSave == true)
            {
                if (_isSkipMessage_CheckReallySaveOrNot == false)
                {
                    if (MessageBox.Show(
                      "今回のデータを保存していいですか？" + _n + "（「いいえ」を選んだ場合、ディレクトリごとゴミ箱に入れられます．）", "確認",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question
                        ) == DialogResult.No)
                    {
                        canSave = false;
                    }
                    else
                    {
                        _reallySaved = true;
                    }
                }
                else
                {
                    _reallySaved = true;
                }
                
            }
            return renameSavedDirectory_Or_delete(_reallySaved);
        }
        /// <summary>
        /// ファイルの保存するかどうかの処理をするメソッド
        /// </summary>
        public bool renameSavedDirectory_Or_delete(bool _isSaved)
        {


            if (_isSaved == true)
            {
                // ディレクトリの名前の末尾に保存時間（秒間）を追記する
                long passedTime = mytime.getPassedSec_FromStartTime();
                string savedSec = MyTime.getShownTime(passedTime, 4, true);
                renameDirectory(this.rootDirectory, this.newDirectoryName, this.newDirectoryName + "(" + savedSec + "秒間)");
                setRootDirectory(this.rootDirectory + "(" + savedSec + "秒間)");
                // （ここではtrueを返す以外何もせず後で）データを保存する
                return true;
            }
            else
            {
                // セーブ用のファイルを作っているなら
                if (canSave == true)
                {
                    //[ToDo]ちゃんとゴミ箱に入らない？完全に消えてしまう？ので，現時点では名前変更して残す
                    // ディレクトリをゴミ箱に入れる
                    //trashoutDirectory(this.rootDirectory);

                    // ディレクトリの名前の頭に「（Test）」と追記する
                    string _test = "(Test)";
                    renameDirectory(this.rootDirectory, this.newDirectoryName, _test + this.newDirectoryName);
                    setRootDirectory(this.rootDirectory.Replace(this.newDirectoryName, _test + this.newDirectoryName));
                    // ディレクトリの名前の末尾に保存時間（秒間）を追記する
                    long passedTime = mytime.getPassedSec_FromStartTime();
                    string savedSec = MyTime.getShownTime(passedTime, 4, true);
                    renameDirectory(this.rootDirectory, this.newDirectoryName, this.newDirectoryName + "(" + savedSec + "秒間)");
                    setRootDirectory(this.rootDirectory + "(" + savedSec + "秒間)");
                }
            }
            
            return false;
        }
        #endregion

        #region [削除]:[名前変更]:[移動]: ファイルの情報を変更するメソッド

        /// <summary>
        /// ディレクトリ（引数はフルパス）を削除します．ゴミ箱では無く，完全に削除されます．
        /// </summary>
        /// <param name="fineName_FullPath"></param>
        public static void deleteDirectory(string fullPath)
        {
            Directory.Delete(fullPath);
        }
        /// <summary>
        /// ディレクトリ（引数はフルパス）をゴミ箱に入れます．
        /// </summary>
        /// <param name="fineName_FullPath"></param>
        public static void trashoutDirectory(string fullPath)
        {
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(fullPath, Microsoft.VisualBasic.FileIO.DeleteDirectoryOption.DeleteAllContents);
        }


        /// <summary>
        /// ディレクトリの名前の変更，第１引数であるディレクトリのフルパス（ディレクトリ名は含んでも含まなくてもよい）の第２引数のディレクトリ名を，第３引数の新しい名前に変える
        /// </summary>
        /// <param name="fineName_FullPath"></param>
        /// <param name="oldDirectoryName"></param>
        /// <param name="newDirectoryName"></param>
        /// <returns></returns>
        public static void renameDirectory(string fullPath, string oldDirectoryName, string newDirectoryName)
        {
            //Note: フォルダの内容の移動（名前の変更）
            // "C:\\TEST"の内容を"C:\\TEST1\\A"へ移動する
            // 移動先フォルダは存在してなく、移動元と同じドライブにあること
            // 移動先フォルダは移動元フォルダのサブフォルダではダメ
            // "C:\\TEST1"フォルダが存在していなければダメ
            // 属性も受け継がれる
            //Directory.Move("C:\\TEST", "C:\\TEST1\\A");

            // ファイルパスチェック
            fullPath = getCheckedFilePath(fullPath);

            // フルパスにディレクトリ名が含まれていない場合は追加
            int lastDirectoryBeginIndex = fullPath.Substring(0, fullPath.Length - 1).LastIndexOf("/");
            string fullPath_Right = fullPath.Substring(lastDirectoryBeginIndex + 1, fullPath.Length - lastDirectoryBeginIndex - 1);
            if (fullPath_Right.Equals(oldDirectoryName + "\\") == false)
            {
                fullPath += oldDirectoryName;
            }
            string fullPath_before = fullPath;

            if (oldDirectoryName.Equals("") == false)
            {
                fullPath = fullPath.Substring(0, lastDirectoryBeginIndex+1) + newDirectoryName; 
                // 効果なし？？ //fineName_FullPath.Replace(oldDirectoryName, newDirectoryName);
                // 変更する名前が元と同じだとエラーが出る
                if (fullPath.Equals(fullPath_before) == false)
                {
                    // ファイルの名前を更新
                    Directory.Move(fullPath_before, fullPath);
                }
            }
        }



        /// <summary>
        /// ディレクトリの移動
        /// </summary>
        /// <param name="oldFullPath"></param>
        /// <param name="newFullPath"></param>
        /// <returns></returns>
        public static void moveDirectory(string oldFullPath, string newFullPath)
        {
            Directory.Move(oldFullPath, newFullPath);
        }
        /* http://dobon.net/vb/dotnet/file/directorycreate.html　より
        //フォルダの作成
        //"C:\TEST"フォルダが存在しなくても"C:\TEST\SUBTEST"が作成される
        System.IO.Directory.CreateDirectory(@"C:\TEST\SUBTEST");

        //フォルダの内容の移動（名前の変更）
        //"C:\TEST"の内容を"C:\TEST1\A"へ移動する
        //移動先フォルダは存在してなく、移動元と同じドライブにあること
        //移動先フォルダは移動元フォルダのサブフォルダではダメ
        //"C:\TEST1"フォルダが存在していなければダメ
        //属性も受け継がれる
        System.IO.Directory.Move(@"C:\TEST", @"C:\TEST1\A");

        //フォルダの削除する
        //"C:\TEST"を根こそぎ（サブフォルダ、ファイルも）削除する
        //"C:\TEST"以下に読み取り専用ファイルがあるとエラーが出る
        System.IO.Directory.Delete(@"C:\TEST", true);


        //作成日時の取得（DateTime値で返される）
        Console.WriteLine(System.IO.Directory.GetCreationTime(@"C:\TEST"));

        //更新日時の取得（DateTime値で返される）
        Console.WriteLine(System.IO.Directory.GetLastWriteTime(@"C:\TEST"));

        //アクセス日時の取得（DateTime値で返される）
        Console.WriteLine(System.IO.Directory.GetLastAccessTime(@"C:\TEST"));


        //カレントディレクトリの取得
        Console.WriteLine(System.IO.Directory.GetCurrentDirectory());

        //このコンピュータの理論ドライブ名をすべて取得
        Console.WriteLine(System.IO.Directory.GetLogicalDrives());

        //ルートディレクトリの取得（下の例では"C:\"）
        //"C:\TEST"が存在している必要はない
        Console.WriteLine(System.IO.Directory.GetDirectoryRoot(@"C:\TEST"));

        //親ディレクトリの取得（下の例では"C:\TEST"）
        //"C:\TEST\A"が存在している必要はない
        Console.WriteLine(System.IO.Directory.GetParent(@"C:\TEST\A"));


        //カレントディレクトリの設定
        System.IO.Directory.SetCurrentDirectory(@"C:\TEST");

        //作成日時の設定（現在の時間にする）
        System.IO.Directory.SetCreationTime(@"C:\TEST", DateTime.Now);

        //更新日時の設定
        System.IO.Directory.SetLastWriteTime(@"C:\TEST", DateTime.Now);

        //アクセス日時の設定
        System.IO.Directory.SetLastAccessTime(@"C:\TEST", DateTime.Now);
        */
        #endregion


        #region [読み書き]:同じファイルに、一行ごとに何度も、読み込み、書き込みするメソッド
        /// <summary>
        /// 引数ファイルに指定文字列一行を書き込みます。同じファイルならば、このメソッドを何回も実行することで、ファイル開け閉め無しに一行ごとの書き込みが可能です。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="writeTextLine"></param>
        /// <returns></returns>
        public bool writeLine_OnFile(string writeFileName, string writeTextLine)
        {
            bool isOk = true;
            if (canSave == true)
            {
                try
                {
                    if (writeLine_FileName.Equals("") == true)
                    {
                        writeLine_FileName = writeFileName;
                        //保存ファイル名指定（Stream型は @+string型）
                        writeLine_fs = new FileStream(rootDirectory + "/" + writeFileName, FileMode.Append); // Appendは追加書き込み
                        //書き込みストリーム生成
                        writeLine_writer = new StreamWriter(writeLine_fs, System.Text.Encoding.GetEncoding("utf-8")); // shift_jis")
                        isOpen = true;
                    }

                    // 書き込みテキストの指定
                    writeLine_writer.Write(writeTextLine);
                    writeLine_writer.Write(writeLine_writer.NewLine);
                    //ファイルへ書き込み
                    writeLine_writer.Flush();

                    // writeLineのclose処理はこのクラスのcloseメソッドで行う

                    // デバッグ表示用
                    //Console.WriteLine(rootDerectory + "/" + _writeFileName_FullPath + "に一行，\n" + writeTextLine + "を書き込みました．");
                }
                catch (IOException e)
                {
                    isOk = false;
                    MessageBox.Show("ファイル書き込みエラーです．\n" + e.ToString());
                    this.writeLine_close();
                }
                finally
                {
                }
            }
            return isOk;
        }

        /// <summary>
        /// 引数ファイルの文字列一行を読み込んで返します。同じファイルならば、このメソッドを何回も実行することで、ファイル開け閉め無しに一行ごとの読み込みが可能です。
        /// </summary>
        /// <param name="readFileName"></param>
        /// <returns></returns>
        public string readLine_OnFile(string readFileName)
        {
            string readText = "";
            try
            {
                if (readLine_FileName.Equals("") == true)
                {
                    //読み込みファイル名指定（IntPtr型は @+string型で表現可能）
                    readLine_fs = new FileStream(rootDirectory + "/" + readFileName, FileMode.Open);
                    //読み込みストリーム生成
                    readLine_reader = new StreamReader(readLine_fs, System.Text.Encoding.GetEncoding("utf-8"));  // shift_jis")
                }

                //一行読み込み
                readText = readLine_reader.ReadLine();

                // ファイルの終わりまで行ったら終了
                if (readLine_reader.EndOfStream == true)
                {
                    readLine_FileName = "";
                    //ファイルのロックを解除
                    readLine_reader.Close();
                    readLine_reader.Dispose();
                }
                
                // デバッグ表示用
                //Console.WriteLine(rootDerectory + "/" + readFileName + "から一行，\n" + readText + "を読み込みました．");
            }
            catch (IOException e)
            {
                MessageBox.Show("ファイル読み込みエラーです．\n" + e.ToString());
                readLine_FileName = "";
                //ファイルのロックを解除
                readLine_reader.Close();
                readLine_reader.Dispose();
            }
            finally
            {
            }
            return readText;
        }
        #endregion

        #region [読み書き][スレッド]:マルチスレッドに対応した、実行直後の遅延が少ない、同じファイルに、一行ごとに何度も、読み込み、書き込みするメソッド
        /// <summary>
        /// マルチスレッドに対応した、実行直後の遅延が少ない、指定ファイルに指定文字列を書き込むメソッドです。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        public bool write_OnFile_Thread(string writeFileName, List<string> writeText)
        {
            if (canSave == true)
            {
                thread_WriteLine_FileName = writeFileName;
                thread_WriteLine_WriteText = writeText;
                thread_WriteLine_WrritenLineNum = 0;
                thread_WriteLine_isEnd = false;
                thread_WriteLine_isOpen = false;
                thread_WriteLine = new Thread(new ThreadStart(this.writeLine_OnFile_Thread));
#if Debug
                MessageBox.Show("書き込みスレッド開始！");
#endif
                thread_WriteLine.Start();
                while (thread_WriteLine_isEnd == false)
                {
                    Application.DoEvents();
                }
                thread_WriteLine.Abort();
                thread_WriteLine = null;
#if Debug
                MessageBox.Show("書き込みスレッド終了！");
#endif
            }
            return true;
        }

        /// <summary>
        /// マルチスレッドに対応した、実行直後の遅延が少ない、指定ファイルの内容文字列を返すメソッドです。
        /// </summary>
        /// <param name="reaFileName"></param>
        public List<string> read_OnFile_Thread(string readFileName)
        {
            thread_ReadLine_FileName = readFileName;
            //thread_ReadLine_ReadText = new List<string>();
            thread_ReadLine_isEnd = false;
            thread_ReadLine_isOpen = false;
            thread_ReadLine = new Thread(new ThreadStart(this.readLine_OnFile_Thread));
            thread_ReadLine.Start();
            while (thread_ReadLine_isEnd == false)
            {
                Application.DoEvents();
            }
            thread_ReadLine.Abort();
            thread_ReadLine = null;
            // スレッド終了時の処理はここに書けばよい？
            //MessageBox.Show("読み込みスレッド終了！");
            return thread_ReadLine_ReadText;
        }

        // 後、上記メソッドのスレッド
        protected void writeLine_OnFile_Thread()
        {
            bool isOk = false; // 終了時にtrue
            while (isOk == false)
            {

                try
                {
#if Debug
                    if (thread_WriteLine_FileName.Equals("taskdata.csv") == true)
                    {
                        Program.ApplicationError();
                    }
#endif
                    if (thread_WriteLine_isOpen == false)
                    {
                        //保存ファイル名指定（Stream型は @+string型）
                        thread_WriteLine_fs = new FileStream(rootDirectory + "/" + thread_WriteLine_FileName, FileMode.Append); // Appendは追加書き込み、ファイルがなければ新規書き込み
                        //書き込みストリーム生成
                        thread_WriteLine_writer = new StreamWriter(thread_WriteLine_fs, System.Text.Encoding.GetEncoding(ENCODING_DEFAULT)); // shift_jis")
                        thread_WriteLine_isOpen = true;
                    }

                    if (thread_WriteLine_WrritenLineNum < thread_WriteLine_WriteText.Count)
                    {
                        // 書き込みテキストの指定
                        string writeTextLine = thread_WriteLine_WriteText[thread_WriteLine_WrritenLineNum];
                        thread_WriteLine_writer.Write(writeTextLine);
                        thread_WriteLine_writer.Write(thread_WriteLine_writer.NewLine);
                        thread_WriteLine_WrritenLineNum++;
                        //ファイルへ書き込み
                        thread_WriteLine_writer.Flush();
                    }
                    else
                    {
                        //ファイルのロックを解除
                        thread_WriteLine_writer.Close();
                        thread_WriteLine_writer.Dispose();
                        thread_WriteLine_fs.Close();
                        thread_WriteLine_fs.Dispose();
                        thread_WriteLine_isOpen = false;
                        isOk = true;
                    }

                    // デバッグ表示用
                    //Console.WriteLine(rootDerectory + "/" + thread_WriteLine_FileName + "に一行，\n" + writeTextLine + "を書き込みました．");
                }
                catch (IOException e)
                {
                    MessageBox.Show("ファイル書き込みエラーです．" +_n + e.Message + _n + e.ToString() + _n + e.StackTrace);
                    //ファイルのロックを解除
                    thread_WriteLine_writer.Close();
                    thread_WriteLine_writer.Dispose();
                    thread_WriteLine_fs.Close();
                    thread_WriteLine_fs.Dispose();
                    thread_WriteLine_isOpen = false;
                    isOk = true;
                }
                finally
                {
                }
            }
            thread_WriteLine_isEnd = true;
        }

        protected void readLine_OnFile_Thread()
        {
            bool isOk = false; // 終了時にtrue
            while (isOk == false)
            {
                string _readTextLine = "";
                try
                {
                    if (thread_ReadLine_isOpen == false)
                    {
                        //読み込みファイル名指定（IntPtr型は @+string型で表現可能）
                        thread_ReadLine_fs = new FileStream(rootDirectory + "/" + thread_ReadLine_FileName, FileMode.Open);
                        //読み込みストリーム生成
                        thread_ReadLine_reader = new StreamReader(thread_ReadLine_fs, System.Text.Encoding.GetEncoding(ENCODING_DEFAULT));  // shift_jis")
                        thread_ReadLine_isOpen = true;
                    }

                    //一行読み込み
                    _readTextLine = thread_ReadLine_reader.ReadLine();
                    // ※一行を読み込みテキストリストに追加
                    thread_ReadLine_ReadText.Add(_readTextLine);

                    // ファイルの終わりまで行ったら終了
                    if (thread_ReadLine_reader.EndOfStream == true)
                    {
                        //ファイルのロックを解除
                        thread_ReadLine_reader.Close();
                        thread_ReadLine_reader.Dispose();
                        thread_ReadLine_fs.Close();
                        thread_ReadLine_fs.Dispose();
                        thread_ReadLine_isOpen = false;
                        isOk = true;
                    }

                    // デバッグ表示用
                    //Console.WriteLine(rootDerectory + "/" + thread_ReadLine_FileName + "から一行，\n" + thread_ReadLine_ReadTextLine + "を読み込みました．");
                }
                catch (IOException e)
                {
                    MessageBox.Show("ファイル読み込みエラーです．" +_n+e.Message+_n+ e.ToString() +_n+ e.StackTrace);
                    //ファイルのロックを解除
                    thread_ReadLine_reader.Close();
                    thread_ReadLine_reader.Dispose();
                    thread_ReadLine_fs.Close();
                    thread_ReadLine_fs.Dispose();
                    thread_ReadLine_isOpen = false;
                    isOk = true;
                }
                finally
                {
                }
            }
            thread_ReadLine_isEnd = true;
        }
        #endregion




        // ■ファイル系（最新版は、MyToolsのものをコピーしてね）
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
            // "/"での記述はこのクラスでは推奨していないが、ちゃんとチェックはする
            int lastDirectoryBeginIndex = fullPath.Substring(0, fullPath.Length - 1).LastIndexOf("/");
            if (lastDirectoryBeginIndex == -1)
            {
                lastDirectoryBeginIndex = fullPath.Substring(0, fullPath.Length - 1).LastIndexOf("\\");
            }
            string fullPath_Right = fullPath.Substring(lastDirectoryBeginIndex + 1, fullPath.Length - lastDirectoryBeginIndex - 1);
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
                    _fileNames[i] = getFileName_TheMostRightFileOrDirectory(_fileNames[i]);
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
                MessageBox.Show("getCheckedFilePath: ■エラー: ファイル／ディレクトリ名「" + getFileName_TheMostRightFileOrDirectory(_fullPath) + "」\nフルパス\"" + _fullPath + "\"\nは存在しません。\nプログラムを続けますか？", "ファイルが見つからないエラー", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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


        #region get/setアクセサ
        // 一行書き込みのファイルが開いているかどうか
        public bool isOpened()
        {
            return isOpen;
        }
        /// <summary>
        /// データをセーブするかどうかを返します．
        /// </summary>
        /// <returns></returns>
        public bool getCanSave()
        {
            return canSave;
        }

        public string getRootDirectory()
        {
            return this.rootDirectory;
        }
        public void setRootDirectory(string _newRootDirectory_Path)
        {
            rootDirectory = _newRootDirectory_Path;
            // 新しいディレクトリの変更
            this.newDirectoryName = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_newRootDirectory_Path);
        }

        public string getNewDirectoryName()
        {
            return this.newDirectoryName;
        }

        #endregion

    }
}
