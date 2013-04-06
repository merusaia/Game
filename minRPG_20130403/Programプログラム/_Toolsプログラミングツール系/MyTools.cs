// デバッグ時はコメントアウト
//#define Debug
// エラーデバッグ時のテスト時はコメントアウト
//#define Test

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices ;
using System.Diagnostics;
using System.Reflection;
using System.Collections; // ジェネリックを使うのに必要（List＜T＞など）
using System.Threading;   // スレッドを使うのに必要

// System.IOを使う主なメソッドはRead***とWrite***だけ。一応環境非依存なはずだが、念のため分けておく
using System.IO;

// System.Drawingを使う主なメソッドはgetImage**などのイメージ操作メソッドだけ。ASP.NETを使う場合はこれらを使わない方がいい。
using System.Drawing;   // ASP.NETなどでは使えないため、一応分けておく

// ※Windows.FormアプリやWIN32API用。Windows非依存にしたい場合はコメントアウトして、「□□□□」で検索して後半のコードをコメントアウト
using System.Windows.Forms;

// ※VisualBasic>NETの機能をＣ＃で使いたい時。使用するにはプロジェクトにMicrosoft.VisualBasicの参照の追加が必要です。プロジェクトを右クリック→「参照の追加」などで追加。
using Microsoft.VisualBasic;

// ※Windowsのショートカットファイルやショートカットフォルダを扱う際に必要なライブラリ。使用するにはプロジェクトにCOMの「Windows Script Host Object Model」（wshom.ocx）の参照の追加が必要です。プロジェクトを右クリック→「参照の追加」などで追加。
using IWshRuntimeLibrary;

using PublicDomain;

// namespace merusaia.NakatsukaTetsuhiro.Experiment
namespace PublicDomain
{
    /// <summary>
    /// ちょっとした時に使いたい，様々なあると便利な機能を実装したstaticメソッドを持つクラスです．
    /// 
    /// </summary>
    public class MyTools
    {
        // 一応、regionを使ってカテゴリ毎に分けていますが、完全ではありません。見にくいかったらすみません。
        // 見にくい場合は、（ＶｉｓｕａｌＳｔｕｄｉｏ２０１０限定かもですが）Ctrl+m+oなどで一度全てのアウトラインを閉じてから、好きなところに飛んでください。
        // 逆に、全て展開してざっと見したい場合は、ソースを右クリック→「アウトライン」→「すべてのアウトラインの切り替え」などでできます。

        
        // 【使い方の例】__HELPメソッド
        /// <summary>
        /// このクラスの使い方を示したメソッドです。詳しくは中身をみてみてください。
        /// </summary>
        public static void __HELP_ThisClassusingExamples・このクラスの使い方やテストコード()
        {
            MyTools.ConsoleWriteLine("\n■■■テスト（ヘルプ）: MyTools.__HELP***\n"
            + "この出力は、MyToolsクラスの主な機能を紹介した出力結果です。"
            //+ "\n※詳しくは、MyTools.__HELP_ThisClassUsingExamples・このクラスの使い方やテストコード()"
            + "\n※詳しくは、" + MyTools.getMethodName(0) // MyTools.__HELP_ThisClassUsingExamples・このクラスの使い方やテストコード
            //+ "\n※詳しくは、" + MyTools.getMethodName_Japanese() + ": " + MyTools.getMethodName_English() // このクラスの使い方やテストコード: MyTools.__HELP_ThisClassUsingExamples
            +"() の中身を見てください。");

            // 現在の日付（何月何日とか）を知りたい時は、このgetNowTime_***メソッドが便利だよ
            string _nowTime = MyTools.getNowTime_Japanese();
            MyTools.ConsoleWriteLine("\n今は、"+_nowTime+"です。");

            // 経過時間時間を高速かつ簡単に調べたい時。ストップウォッチもいいけど、これも便利だよ
            MyTools.ConsoleWriteLine("\n■時間を測る時、数ミリ秒単位は腕時計（StopWatchクラス）の方が精度はいいけど定義は面倒だしちょっと遅い。\n"
                +"十数ミリ秒単位でいいならSystem.Environmentを使った、\n     int _time1 = MyTools.getNowTIme_fast();\n"
                +"の方が高速だし、一行で呼び出せるから簡単だよ。");
            int _time1 = MyTools.getNowTime_fast();
            // 処理を待つ
            MyTools.ConsoleWriteLine("\n■処理待ちは、直接 Thread.Sleep()と書くのもいいけど、後で一括して待ち時間を変更したり、待ち処理を変更したい時に不便。できれば、\n"
                +"      MyTools.wait_ByThreadSleep()\nを使っておいた方がいいかも。"
                +"あと、Windowsフォームアプリケーションの場合は、これを使うと入力も止まっちゃうので、"
                +"Application.DoEvents()を使ったたり、できれば、\n"
                +"      MyTools.wait_ByApplicationDoEvents()を使っておいた方がいいかも。");
            MyTools.wait_ByThreadSleep(1000); // こっちだとユーザの入力や他の処理を実行できない。後で一括して変更できるように、メソッドにしておくといいよね。
            MyTools.ConsoleWriteLine("これらを使って、(MyTools.getNowTime_fast() - _time1) = " + (MyTools.getNowTime_fast() - _time1) + "ミリ秒待ったよ。");
            MyTools.wait_ByApplicationDoEvents(500); // こっちだと、ユーザの入力や他の処理を実行できるよ。
            MyTools.ConsoleWriteLine("これらを使って、(MyTools.getNowTime_fast() - _time1) = "+(MyTools.getNowTime_fast() - _time1) + "ミリ秒待ったよ。");


            // エラーなどがおきたとき、一括して処理をしたい場合、showMessageを使う
            string _message = "\n\n■コンソール出力の一括管理メソッド: MyTool.ConsoleWriteLine\n"
                + "…\nSystem.Console.WriteLine(\"該当するファイルが見つかりませんでした。ファイル名: ***.txt\");\n" +
                "…\nこのエラーメッセージ。頻繁に使うんだけど、"
                + "System.Console.WriteLine()で書いちゃうと、あとで消したい時とかMessegeBox.Show()とかファイル保存したい"
            + "時に、全部変更しないといけないのは面倒だなぁ…";
            Console.WriteLine(_message);
            _message = "そんなとき、このメソッドを使うと、後で一括して変更できるよ。\nMyTools.ConsoleWriteLine(_message)";
            // そんなとき、このメソッドを使うと便利だよ。
            MyTools.ConsoleWriteLine(_message);
            // （おまけ）メッセージボックスに表示したいものは、別のメソッドを使うとさらに便利だよ。
            //MyTools.MessageBoxShow(_message);


            // リスト操作
            MyTools.ConsoleWriteLine("\n\n■配列チェック不要のリスト内要素呼び出しメソッド: getListValue<T>\n例えば、List<string>のリスト配列の値を参照する時、変なインデックスを入れないしないかいちいちチェックするのが面倒な場合、\n  getListValue<string>(_list, _index)\nを使うとソースコードがスッキリするよ");
            // 例えば、_list.Count=2のリストを用意する。
            List<string> _list = new List<string>();
            // Add処理…
            _list.Add("文字１");
            _list.Add("文字２");
            // ...
            // (a)例えば、いきなり値が入ってない_watchList[5]を取ろうとしたら、エラーになっちゃうから、
            // 配列チェックのif文が要る
            //int _index = 5;
            //if (_index < _list.Count) // // この配列チェックよく忘れるし、if文の入れ子が邪魔。一個ならいいけど、他の処理と重なると読みにくくなる。
            //{
            //    // でも、配列チェックを怠ると…
            //    string _value = _list[_index]; // ここでArgumentOutOfRangeException:「インデックスが範囲を超えています。」のエラー
            //}
            // (b)そんなとき、このメソッドを使うと、一行でスッキリかけるよ。
            string _value = MyTools.getListValue(_list, 5); // _list.Count=2だけど、内部でちゃんと配列チェックをしているので、エラーは起こらない
            
            // 配列が存在しないものの値は、null（=default(string)）が入る。
            // エラー時に格納する文字列を変えたかったら、一行でこう書いてもOK。もしくは、getListValueを改良／呼び出す新しいメソッドを作る。
            if (_value == null) _value = "Error";
            MyTools.ConsoleWriteLine("配列が存在しないものは、値型は0、string型はnull、クラス型にもnull。が入るよ（正確には、default(T)で確かめて）。\nもし値チェックをしたい時は、if(getListValue<string>(_list, _index) == null) _value = \"Error\";みたいに使ってね。");
            // 参考。default(T)の値一覧。感謝。 http://d.hatena.ne.jp/gsf_zero1/20110221/p1


            bool _isTestString・文字列チェックテスト = true;
            #region 文字列のチェックテスト
            if (_isTestString・文字列チェックテスト == true)
            {
                // IsZenkakuやgetEstringCharTypeで、全角チェックや文字タイプのチェックが簡単にできるよ
                Console.WriteLine("MyTools.IsZenkaku('d'):偽のはず: = " + MyTools.IsZenkaku('d'));
                Console.WriteLine("MyTools.IsZenkaku('ｄ'):真のはず: = " + MyTools.IsZenkaku('ｄ'));
                Console.WriteLine("MyTools.isZenkakuChar_Including(\"全takldjfkla\n\"):真のはず　" + MyTools.IsZenkakuChar_Including("全takldjfkla\n"));
                Console.WriteLine("MyTools.isZenkakuChar_Including(\"takldjfkla\"):偽のはず　" + MyTools.IsZenkakuChar_Including("takldjfkla"));
                Console.WriteLine("MyTools.getEStringCharType(\"漢字打毛\") = " + MyTools.getEStringCharType("漢字打毛").ToString());
                Console.WriteLine("MyTools.getEStringCharType(\"ﾊﾝｶｸｶﾀｶﾅフクムカタカナ\") = " + MyTools.getEStringCharType("ﾊﾝｶｸｶﾀｶﾅフクムカタカナ").ToString());
                Console.WriteLine("MyTools.getEStringCharType(\"HankakuEisuuzi1.dfa*@\") = " + MyTools.getEStringCharType("HankakuEisuuzi1.dfa*@").ToString());
                // getParsedStringで、指定した文字タイプに出来る限り変換することができるよ
                Console.WriteLine("MyTools.getParsedString(\"ﾊﾝｶｸｶﾀｶﾅフクムカタカナ\", EStringCharType.t0d_JapaneseKatakanaZenkakuOnly＿全角カタカナのみ) = " + MyTools.getParsedString("ﾊﾝｶｸｶﾀｶﾅフクムカタカナ", EStringCharType.t0d_JapaneseKatakanaZenkakuOnly＿全角カタカナのみ));

                Console.WriteLine("\n\n■Enum型の名前を取得する便利なメソッド: getEnum***");
                EStringCharType _EnumItem = EStringCharType.t0c_Japanese1_日本語＿半角カタカナを含む;
                Console.WriteLine("_EnumItem = EStringCharType.t0c_Japanese1_日本語＿半角カタカナを含む; のとき、");
                // getEnumNameで、全部の名前が取れるよ
                string _EnumName = MyTools.getEnumName(_EnumItem);
                Console.WriteLine("MyTools.getEnumName(_EnumItem) = " + _EnumName);
                // getEnumKeyName_OnlyFirstIndexWordで、最初の"_"や"＿"や"・"までの単語だけが取れるよ
                Console.WriteLine("MyTools.getEnumKeyName_OnlyFirstIndexWord(_EnumItem) = " + MyTools.getEnumKeyName_OnlyFirstIndexWord(_EnumItem));
                // getEnumKeyName_OnlyJapaneseで、最初の"_"や"＿"や"・"以降からはじまる日本語１単語が取れるよ
                string _EnumJanapeseName = MyTools.getEnumKeyName_OnlyJapanese(_EnumItem);
                Console.WriteLine("MyTools.getEnumKeyName_OnlyJapanese(_EnumItem) = " + _EnumJanapeseName);
                // getEnumKeyName_OnlyEnglishで、最初の"_"や"＿"や"・"以降からはじまる英語１単語が取れるよ
                string _ENumEnglishName = MyTools.getEnumKeyName_OnlyEnglish(_EnumItem);
                Console.WriteLine("MyTools.getEnumKeyName_OnlyEnglish(_EnumItem) = " + _ENumEnglishName);
                // getEnumKeyName_OnlyLastIndexWordで、最後の"_"や"＿"や"・"以降からはじまる１単語だけが取れるよ
                Console.WriteLine("MyTools.getEnumKeyName_OnlyLastIndexWord(_EnumItem) = " + MyTools.getEnumKeyName_OnlyLastIndexWord(_EnumItem));

                string _fileName = "enter1・決定音＿ピローン.mp3";
                Console.WriteLine("string _filleName = enter1・決定音＿ピローン.mp3");
                // getWords_InStringで、全部の単語が取れるよ
                List<string> _words = MyTools.getWords_InString(_fileName, 0, 1, 0, false, "_", "＿", "・");
                Console.WriteLine("getWords_InString(_fileName, 0, 1, 0, false, \"_\", \"＿\", \"・\") = \n\t" + _words.ToString());
                // getWords_InStringで、ひらがなカタカナだけの単語が取れるよ
                _words = MyTools.getWords_InString(_fileName, 0, 1, EStringCharType.t0d_JapaneseHiraganaOrKatakanaZenkakuOnly＿全角ひらがなかカタカナのみ, false, "_", "＿", "・");
                Console.WriteLine("getWords_InString(_fileName, 0, 1, EStringCharType.t0d_JapaneseHiraganaOrKatakanaZenkakuOnly＿全角ひらがなかカタカナのみ, false, \"_\", \"＿\", \"・\") = \n\t" + _words.ToString());
                
            }
            #endregion


            bool _isTestSeikiBunpu・正規分布っぽい値の取得テスト = true;
            #region 正規分布っぽい値を取得する、剣技テスト
            if (_isTestSeikiBunpu・正規分布っぽい値の取得テスト == true)
            {
                MyTools.ConsoleWriteLine("■正規分布っぽい値を取得する、剣技のテスト: MyTools.***\n（※MyToolsクラス内のいろんな便利メソッドを使ってます。詳しくは「__HELP***」メソッド内のソースコードを見てみてください");
                Stopwatch _stopwatch1 = new Stopwatch();
                // 処理時間を図りましょう
                _stopwatch1.Reset();
                _stopwatch1.Start();
                //MyTools.ConsoleWriteLine("数ミリ秒単位はストップウォッチの方が精度いいけど、\nint _time1 = MyTools.getNowTIme_fast();\nの方が高速だし、一行で呼び出せるから簡単だよ。");
                _time1 = MyTools.getNowTime_fast();
                MyTools.ConsoleWriteLine("\n　↓　腕時計。よーい、スタート");

                // 例：剣技のテストの点数を、適当な平均値・最小値・最大値を使って、やんわり正規分布っぽい形で出してみる
                string _testName = "剣技";
                int _tensu;
                int _personNum = 10000; // テスト参加数。
                int _ketasuMax = 5;     //表示桁数
                int _heikinti = 60; // 平均点
                double _bunsanMin = -5; // 標準正規分布なら-1
                double _bunsanMax = 5; // 標準正規分布なら1
                // 以下は、テストの結果を格納する時に使う
                List<int> _tensuList = new List<int>();
                int _MaxTensu = 0;
                int _MinTensu = 100;

                // テスト開始
                MyTools.ConsoleWriteLine("\n" + _testName + "のテスト（0～100点）を、" + _personNum + "人にやってもらったよ。\n問題は、平均" + _heikinti + "点位、分散は" + _bunsanMin + "～" + _bunsanMax + "にバラツクように作りました。");
                for (int i = 1; i <= _personNum; i++)
                {
                    _tensu = MyTools.getSeikiRandomNum_RealWorldRate(_heikinti, 0, 100, _bunsanMin, _bunsanMax);
                    //MyTools.ConsoleWriteLine(i+"人目の数学のテストの点数,"+_tensu); // これだけ×100回で400ミリ秒かかるよ
                    _tensuList.Add(_tensu);
                    //if (_tensu > _MaxTensu) _MaxTensu = _tensu; //こんなことしなくても
                    //if (_tensu < _MinTensu) _MinTensu = _tensu;
                }
                _stopwatch1.Stop();
                int _time2 = MyTools.getNowTime_fast();
                MyTools.ConsoleWriteLine("　↑　テストをするだけで、(MyTools.getNowTime_fast() - _time1) = " + (_time2 - _time1) + "ミリ秒かかったよ。腕時計ととの精度の違いはどうかな。Stopwatch:" + _stopwatch1.ElapsedMilliseconds + "msec.");



                _stopwatch1.Reset();
                _stopwatch1.Start();
                _time1 = MyTools.getNowTime_fast();
                //MyTools.ConsoleWriteLine("\n　↓　腕時計。よーい、スタート");

                // 最大値と最小値を取得する
                _MaxTensu = MyTools.getBiggestValue(_tensuList); // 要素数が多い時はちょっと時間かかるかもだけどね
                _MinTensu = MyTools.getSmallestValue(_tensuList);
                MyTools.ConsoleWriteLine("\n最大値:" + _MaxTensu + "点、最小値:" + _MinTensu + "点、");

                // ソートする
                // _tensuList.Sort();もいいけど元のリストの中身が変更されちゃうし、降順か昇順かわからなくなるからねぇ
                // このメソッドを使うと、元のリストの中身は変更されずに、新しいソート済みのリストを作れるよ。
                List<int> _sortedTensuList = MyTools.getSortedList(_tensuList, MyTools.ESortType.値が大きい順＿降順);
                _stopwatch1.Stop();
                _time2 = MyTools.getNowTime_fast();
                //MyTools.ConsoleWriteLine("　↑　最大値と最小値を計算するだけで、"+(_time2-_time1)+"ミリ秒かかったよ。腕時計ととの精度の違いはどうかな。Stopwatch:"+_stopwatch1.ElapsedMilliseconds+"msec.");


                _stopwatch1.Reset();
                _stopwatch1.Start();
                _time1 = MyTools.getNowTime_fast();
                //MyTools.ConsoleWriteLine("\n　↓　腕時計。よーい、スタート");
                // 平均値と標準偏差と分散を出してみる
                double _average = MyTools.getAverage_InList(_tensuList);
                double _bunsan = MyTools.getBunsan_InList(_tensuList);
                double _hyouzyunHensa = MyTools.getHyouzyunHensa_InList(_tensuList);
                MyTools.ConsoleWriteLine("平均値:" + _average + "点、分散" + _bunsan + "、標準偏差" + _hyouzyunHensa + "点、です。"
                    + "トップは" + _MaxTensu + "点、ビリは" + _MinTensu + "点でした。\n"
                    + "正規分布に従えば、約６８％の人が " + MyTools.getSisyagonyuValue(_average) + "±" + MyTools.getSisyagonyuValue(_hyouzyunHensa) + "点のところにいるかも。");
                // 同じものをまとめて計算したい時はこれ。forループが一回なので一番早く計算できるよ
                MyTools.getAnalyzedValues_InList(_tensuList, out _MinTensu, out _MaxTensu, out _average, out _bunsan, out _hyouzyunHensa);
                //MyTools.ConsoleWriteLine("\n平均" + _average + "　分散" + _bunsan + "　標準偏差" + _hyouzyunHensa + "　です。標準正規分布は分散1ですよ。");
                _stopwatch1.Stop();
                _time2 = MyTools.getNowTime_fast();
                //MyTools.ConsoleWriteLine("　↑　ここまでで、" + (_time2 - _time1) + "ミリ秒かかったよ。腕時計ととの精度の違いはどうかな。Stopwatch:" + _stopwatch1.ElapsedMilliseconds + "msec.");



                _stopwatch1.Reset();
                _stopwatch1.Start();
                _time1 = MyTools.getNowTime_fast();
                MyTools.ConsoleWriteLine("\n　↓　腕時計。よーい、スタート");

                // ランク情報
                // これを使うと、一定の値の範囲をＥ～ＳＳＳＳランクとして定義することができるよ。
                // 参考：大学の合格判定は、Ｅ５％　Ｄ２０％　Ｃ５０％　Ｂ５０～７５％　Ａ９０％　らしい。

                MyTools.setERank_FtoS_ByMin(1, 20, 30, 40, 60, 80, 90, 95, 98, 100);
                string _rankInfo = MyTools.getERankInfo();
                MyTools.ConsoleWriteLine(_rankInfo);
                // ランク情報を更新。これを使うと、一定の人数の範囲をＥ～ＳＳＳＳランクとして定義することができるよ。
                //MyTools.setERank_FtoS_ByInclusingRate(0.001, 8, 10, 15, 20, 30, 20, 1, 0.1, 0.01, 0.001);
                // この最小値が、一つの上の人数のやつとだいたい一緒の値になる。
                MyTools.setERank_FtoS_ByMin(0.001, 8, 17, 30, 50, 80, 99, 99.9, 99.99, 99.999);
                _rankInfo = MyTools.getERankInfo();
                MyTools.ConsoleWriteLine("※ランク情報を以下に変更します。" + _rankInfo);

                // ランキングしてみる。
                MyTools.ConsoleWriteLine("【ランキング結果】");
                // リストをコピー
                List<int> _uniqueTensuList = MyTools.getCopyedList(_sortedTensuList);
                // 値の重複をなくす
                MyTools.removeSameValue_InList(_uniqueTensuList);
                int _ranking_TopMax = 10; // 上位10位までを紹介
                int _rankingNo = 1; // ランキング数。

                // トップランキング
                MyTools.ConsoleWriteLine("【トップランキング】");
                for (int i = 0; i < _ranking_TopMax; i++)
                {
                    // もし生徒数が1名未満でも、エラーを起こさず値を取ってこれるように、getListValueを使う
                    int _Noi_tensu = MyTools.getListValue(_uniqueTensuList, i); // 既に降順ソートされているからi番目がi位
                    // 同じ値の要素数を取ってくる
                    int _Noi_persons = MyTools.getSameValueCount_InList<int>(_sortedTensuList, _Noi_tensu);
                    // 桁数が違っても表示をそろえたいので、getStringNumberを使う
                    MyTools.ConsoleWriteLine(MyTools.getStringNumber(_rankingNo, true, _ketasuMax, 0) + "位：" + MyTools.getStringNumber(_Noi_tensu, true, 3, 0) + "点（" + MyTools.getStringNumber(_Noi_persons, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_Noi_persons / (double)_personNum * 100, true, 3, 6) + "％）　→　" + MyTools.getERank_FtoS(_Noi_tensu) + "ランク");
                    _rankingNo += _Noi_persons; // ランキング数に既にランクインした上位人数を足していく
                }
                // トップランキングをして全ての点数を列挙していなければ
                if (_uniqueTensuList.Count > _ranking_TopMax)
                {
                    // 関係ないけど、中間値（平均値とはちょっとだけ違う）があるインデックスもこんな簡単に取ってこれるよ
                    int _ranking_midTop = MyTools.getIndex_Middle(_uniqueTensuList);

                    // 任意の値に一番近いインデックスもこんな簡単に取ってこれるよ
                    int _ranking_averageTop = MyTools.getIndex_MostClosed(_uniqueTensuList, (int)_average);
                    // 平均値付近のランキングが欲しいので、半分上位にする
                    if (_ranking_averageTop - _ranking_TopMax / 2 > _ranking_TopMax)
                    {
                        _ranking_averageTop = _ranking_averageTop - _ranking_TopMax / 2; // トップランキングとかぶってなかったら、半分上位にする
                    }
                    else
                    {
                        _ranking_averageTop = _ranking_TopMax + 1; // トップランキングの続きから表示
                    }
                    // ランキングNoを更新
                    for (int i = _ranking_TopMax; i < _ranking_averageTop; i++)
                    {
                        // もし生徒数が1名未満でも、エラーを起こさず値を取ってこれるように、getListValueを使う
                        int _Noi_tensu = MyTools.getListValue(_uniqueTensuList, i);
                        // 同じ値の要素数（重複数）を取ってくる
                        int _Noi_persons = MyTools.getSameValueCount_InList<int>(_sortedTensuList, _Noi_tensu);
                        _rankingNo += _Noi_persons;
                    }

                    MyTools.ConsoleWriteLine("【平均付近ランキング】");
                    for (int i = _ranking_averageTop; i < _ranking_averageTop + _ranking_TopMax; i++)
                    {
                        // もし生徒数が1名未満でも、エラーを起こさず値を取ってこれるように、getListValueを使う
                        int _Noi_tensu = MyTools.getListValue(_uniqueTensuList, i);
                        // 同じ値の要素数（重複数）を取ってくる
                        int _Noi_persons = MyTools.getSameValueCount_InList<int>(_sortedTensuList, _Noi_tensu);
                        // 桁数が違っても表示をそろえたいので、getStringNumberを使う
                        MyTools.ConsoleWriteLine(MyTools.getStringNumber(_rankingNo, true, _ketasuMax, 0) + "位：" + MyTools.getStringNumber(_Noi_tensu, true, 3, 0) + "点（" + MyTools.getStringNumber(_Noi_persons, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_Noi_persons / (double)_personNum * 100, true, 3, 6) + "％）　→　" + MyTools.getERank_FtoS(_Noi_tensu) + "ランク");
                        _rankingNo += _Noi_persons; // ランキング数に既にランクインした上位人数を足していく

                        // _ranking_TopMaxととかぶったら終わり
                        if (i <= _ranking_TopMax)
                        {
                            break;
                        }
                    }
                }
                // トップランキングをして全ての点数を列挙していなければ
                if (_uniqueTensuList.Count > _ranking_TopMax)
                {
                    // ランキングNoをワーストに更新
                    _rankingNo = _personNum;
                    MyTools.ConsoleWriteLine("【ワーストランキング】");
                    // ワーストランキングは逆順
                    for (int i = _uniqueTensuList.Count - 1; i > (_uniqueTensuList.Count - 1) - _ranking_TopMax; i--)
                    {
                        // もし生徒数が1名未満でも、エラーを起こさず値を取ってこれるように、getListValueを使う
                        int _Noi_tensu = MyTools.getListValue(_uniqueTensuList, i);
                        // 同じ値の要素数（重複数）を取ってくる
                        int _Noi_persons = MyTools.getSameValueCount_InList<int>(_sortedTensuList, _Noi_tensu);
                        // 桁数が違っても表示をそろえたいので、getStringNumberを使う
                        MyTools.ConsoleWriteLine(MyTools.getStringNumber(_rankingNo, true, _ketasuMax, 0) + "位：" + MyTools.getStringNumber(_Noi_tensu, true, 3, 0) + "点（" + MyTools.getStringNumber(_Noi_persons, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_Noi_persons / (double)_personNum * 100, true, 3, 6) + "％）　→　" + MyTools.getERank_FtoS(_Noi_tensu) + "ランク");
                        _rankingNo -= _Noi_persons; // ワーストランキング数に既にランクインした上位人数を除いていく

                        // _ranking_TopMaxととかぶったら終わり
                        if (i <= _ranking_TopMax)
                        {
                            break;
                        }
                    }
                }
                // 値が任意の範囲にある人数やパーセンテージを出してみよう。
                // その１
                int _range1 = 60;
                int _range2 = 70;
                // 値が任意の範囲にあるリストの要素数を簡単に取ってこれるよ
                int _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                // 桁数が違っても表示をそろえたいので、getStringNumberを使う
                MyTools.ConsoleWriteLine("※" + _range1 + "～" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです");
                // その２
                _range1 = 70;
                _range2 = 80;
                _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                MyTools.ConsoleWriteLine("※" + _range1 + "～" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです");
                // その３
                _range1 = 80;
                _range2 = 90;
                _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                MyTools.ConsoleWriteLine("※" + _range1 + "～" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです");
                // その４
                _range1 = 90;
                _range2 = 100;
                _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                MyTools.ConsoleWriteLine("※" + _range1 + "-" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです");
                // 標準偏差を使って、正規分布の確率６８％に近いかを確認
                _range1 = MyTools.getSisyagonyuValue(_average - _hyouzyunHensa); // 四捨五入が簡単にできるよ
                _range2 = MyTools.getSisyagonyuValue(_average + _hyouzyunHensa);
                _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                MyTools.ConsoleWriteLine("※" + _range1 + "～" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです。正規分布に従えば、平均値" + _average + "±標準偏差" + MyTools.getSisyagonyuValue(_hyouzyunHensa) + "にいる範囲は約６８％のはず。近かったかな？");

                _stopwatch1.Stop();
                _time2 = MyTools.getNowTime_fast();
                MyTools.ConsoleWriteLine("　↑　ここまでで、" + (_time2 - _time1) + "ミリ秒かかったよ。腕時計ととの精度の違いはどうかな。Stopwatch:" + _stopwatch1.ElapsedMilliseconds + "msec.");
            }
            #endregion

            #region その他、テストの草案
            // 以下、まだ作りかけのメソッドのテスト

            // [TODO]Image.Tagをつかった画像埋め込みテスト。まだできてない。保存したらなんか変な画像が出てきた。残念。
            //bool _isTestDrawImage・画像の読み込みテスト = false;
            //if (_isTestDrawImage・画像の読み込みテスト == true)
            //{
            //    string _canvasFileFullPath = MyTools.getProjectDirectory() + "\\データベース\\グラフィック\\1cm黒方眼紙_縦１０マス横１０マス_背景透明.png";//これだとファイルのアクセス権限がデフォルトでは無いからエラーになる"C\\:test.jpg";
            //    // ファイルから画像を読み込むならこれを使う
            //    Image _canvasImage = MyTools.getImage(_canvasFileFullPath);
            //    // ファイルから画像が読み込めなかったり、メモリ解放されてるかどうか調べたいなら、これを使う
            //    if (MyTools.isErrorImage(_canvasImage) == true)
            //    {
            //        _canvasImage = MyTools.getImage(480, 600); // 新しく画像を生成する

            //        // こうやって画像にタグを埋め込めるよ
            //        // MyTools.addTag_Image(_drawnImage, "作者：著作権フリー,方眼紙");
            //        MyTools.addTag_Image(_canvasImage, "作者：著作権フリー");
            //        MyTools.addTag_Image(_canvasImage, "方眼紙");
            //        MyTools.addTag_Image(_canvasImage, "著作情報URL:http://houganshi.net/houganshi_solid.php");
            //    }

            //    // (i)埋め込んでない画像ファイルをロードする方法
            //    Image _drawnImage = MyTools.getImage(MyTools.getProjectDirectory() + "\\データベース\\グラフィック\\1cm黒方眼紙_縦１０マス横１０マス_背景透明.png");
            //    // (ii)埋め込んだリソースからファイルをロードする方法
            //    // [TIPS]プロジェクトへのリソースの埋め込み方。埋め込んだリソースの使い方。参考URL。感謝。http://dobon.net/vb/dotnet/programing/vsresource.html
            //    //Image _drawnImage = Properties.Resources._sample_PNG;

            //    // こうやって画像のタグを読みこめるよ
            //    string _tagString = MyTools.getTag_Image(_canvasImage);
            //    string _imageCreator = MyTools.getTag_Image(_canvasImage, "作者");
            //    ConsoleWriteLine("ファイル\n"+_canvasFileFullPath+"\nの作者は「"+_imageCreator+"」、全てのタグ情報は\n\""+_tagString+"\"\nです。");

            //    // こうやって画像がエラー画像じゃないか（ちゃんと読みこまれているか）確認できるよ
            //    if (MyTools.isErrorImage(_drawnImage) == false)
            //    {
            //        // game.Dispose()とか気にしないで、こんな風に画像を加工できるよ
            //        MyTools.drawImage(_canvasImage, 0, 0, _drawnImage, 0.5);
            //        // jpg画像として保存する
            //        _canvasImage.Save(MyTools.getFileLeftOfPeriodName(_canvasFileFullPath)+"_タグ埋め込み.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //        // C/:直下だと許可がないと書き込み不可になってる可能性もあってか、NotSupportedExceptionがでる。詳細指定されたパスのフォーマットはサポートされていません。
            //        // 使わなくなったイメージは最後にDisposeする（でも大量メモリじゃなかったら、参照する変数がなくなったら自動的にＧＣがやってくれるけどね。）
            //        _canvasImage.Dispose();
            //    }
            //    // 使わなくなったイメージは最後にDisposeする（でも大量メモリじゃなかったら、参照する変数がなくなったら自動的にＧＣがやってくれるけどね。）
            //    _drawnImage.Dispose();

            //}


            // [TODO] mp3の作曲者情報などを取得。まだできてない。中身が取れない。
            //bool _isTestMP3Info・mp3データのファイル情報取得テスト = true;
            //if (_isTestMP3Info・mp3データのファイル情報取得テスト== true)
            //{
            //    string _fileFUllPath = MyTools.getProjectDirectory()+"\\データベース\\"+"sample.mp3";
            //    MyTools.ConsoleWriteLine(MySound_Windows.getFileInfo(_fileFUllPath));
            //}
            #endregion
            // 他にもたくさんあります。詳しくは↓のstaticメソッド集を見てみてください。

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

        // 待つ処理
        #region 一定時間待つ処理 wait_***
        /// <summary>
        /// 10ミリ秒間、操作不可能な状態で待った後、trueを返します。厳密に指定時間停止します。（System.Threading.Thead.Sleepを使ってるので，他のスレッドは動きますが，このスレッドのWindowsメッセージは処理できません．）
        /// </summary>
        public static bool wait_ByThreadSleep()
        {
            return wait_ByThreadSleep(10);
        }
        /// <summary>
        /// 一定時間、操作不可能な状態で待った後、trueを返します。厳密に指定時間停止します。（System.Threading.Thead.Sleepを使ってるので，他のスレッドは動きますが，このスレッドのWindowsメッセージは処理できません．）
        /// </summary>
        /// <param name="_waitMSec"></param>
        /// <returns></returns>
        public static bool wait_ByThreadSleep(int _waitMSec)
        {
            Thread.Sleep(_waitMSec);

            return true;
        }
        #endregion
        #region 他のイベントが実行されるのを待つ: DoEvents/wait_ForOtherEvents
        /// <summary>
        /// 一定時間、操作可能な状態で待った後、trueを返します。Application.EnableVisualStyles();Application.DoEvents();を実行し、他のイベントが実行されるのを待ちます。（このスレッドのWindowsメッセージも処理できます）
        /// </summary>
        public static void wait_ByApplicationDoEvents(int _waitMSec)
        {
            doEvents_WaitForOtherEvents(_waitMSec);
        }
        /// <summary>
        /// 厳密ではありませんが最低10ミリ秒間、操作可能な状態で待った後、trueを返します。Application.EnableVisualStyles();Application.DoEvents();を実行し、他のイベントが実行されるのを待ちます。（このスレッドのWindowsメッセージも処理できます）
        /// </summary>
        public static void wait_ByApplicationDoEvents()
        {
            doEvents_WaitForOtherEvents(10);
        }
        /// <summary>
        /// Application.EnableVisualStyles();Application.DoEvents();を実行し、他のイベントが実行されるのを待ちます。
        /// </summary>
        public static void doEvents_WaitForOtherEvents()
        {
            // [Tips]複数フォーム・孫にフォーム？継承のフォームで新しいフォームを表示するときのエラーで，DoEventsの前にEnableVisualStylesを入れると解消できた
            Application.EnableVisualStyles(); 
            Application.DoEvents();
        }
        /// <summary>
        /// 指定時間、Application.EnableVisualStyles();Application.DoEvents();を実行します。
        /// </summary>
        public static void doEvents_WaitForOtherEvents(int _waitMSec)
        {
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Start();
            while (_stopwatch.ElapsedMilliseconds <= _waitMSec)
            {
                // (ダメ！)これを永遠と呼び出すとＣＰＵ利用率１００％ＭＡＸ！（アホの子のやること。）
                //doEvents_WaitForOtherEvents();
                // (これでいい)一回だけDoEvents()して、あとはSleepしてみる（負荷が一気に下がった…）
                // [Tips]複数フォーム・孫にフォーム？継承のフォームで新しいフォームを表示するときのエラーで，DoEventsの前にEnableVisualStylesを入れると解消できた
                Application.EnableVisualStyles();
                Application.DoEvents();
                Thread.Sleep(_waitMSec);
            }
        }
        #endregion

        // 時刻処理（ちょっとだけ。詳細はMyTimeクラスか標準のStopWatchクラスなどを使ってね）
        #region 現在の時間を取得getTime
        /// <summary>
        /// 現在の時間（システム起動後の経過ミリ秒）を高速に取得します。
        ///  　※getNowTime***と名のつくメソッドは、ある定義に従った経過時間や時刻を、ミリ秒単位のint型で取得します。現在の定義は、「システム起動後の経過ミリ秒、休止状態やスタンバイは含まれない（Environment.TickCount;）」を使っています。
        /// </summary>
        /// <returns></returns>
        public static int getNowTime_fast()
        {
            return Environment.TickCount; // DateTime.Now.Ticks;やStopwatchよりこれが一番処理速度が速いらしい（精度はstopwatchの方がいい）。参考:http://d.hatena.ne.jp/saiya_moebius/20100819/1282201466#20100819f1
        }
        /// <summary>
        /// 現在の時刻をミリ秒を含めて「yyyyMMddHHmmssaaa（aaaはミリ秒）」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public static long getNowTimeAndMSec_NumberOnly()
        {
            DateTime nowDateTime = DateTime.Now;
            string nowTime = nowDateTime.ToString("yyyyMMddHHmmss");  // 現在の時刻（秒まで）
            string msec = nowDateTime.Millisecond.ToString(); // Millisecond.ToString();
            // 3桁以下なら「0」を足す
            if (msec.Length != 3)
            {
                for (int i = 1; msec.Length < 3; i++)
                {
                    msec = "0" + msec;
                }
            }
            nowTime += msec;
            return long.Parse(nowTime);
        }
        /// <summary>
        /// 現在の時刻を「yyyy年MM月dd日 HH時mm分ss秒」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public static string getNowTime_Japanese()
        {
            string nowTime = DateTime.Now.ToString("yyyy年MM月dd日 HH時mm分ss秒");
            return nowTime;
        }
        #endregion

        // アプリケーション実行関連処理
        #region 実行プロジェクトのディレクトリパスを取得する: getProjectDirectory
        /// <summary>
        /// 実行プロジェクトのディレクトリパス（EXEファイルがあるところ，デバッグ時はDebugモードでは"プロジェクト名\\bin\\Debug"やReleaseモードでは"...\\bin\\Release"など）を取得します．なお、ディレクトリの区切り文字は'\\'で統一されています。（※@"\"や、'/'で繋げないでください。一括した検索がしにくいですし、エラーが起こりやすくなります）
        /// </summary>
        /// <returns></returns>
        public static string getProjectDirectory()
        {
            string _path = System.Environment.CurrentDirectory;
            return _path;
        }
        #endregion
        #region N回呼び出し元のメソッド名と行番号を取ってくる: getMethodName
        /// <summary>
        /// N回呼び出し元のメソッドの名前をカッコ無しで取って来ます。（例：　"呼び出し元メソッド:"+ getMethodName(1) → "実行中のメソッド:calledMethod"）
        /// </summary>
        public static string getMethodName(int _StackTraceBackNum・0が自分自身＿1が自分を呼び出している呼び出し元メソッド＿2以降でも呼び出し元をたどれます)
        {
            // getClassMethodNameメソッドへの呼び出し回数が増えるため、1足さないと整合性が取れない
            int _stackNum・0が自分自身 = 1 + _StackTraceBackNum・0が自分自身＿1が自分を呼び出している呼び出し元メソッド＿2以降でも呼び出し元をたどれます;
            return getClassMethodName(_stackNum・0が自分自身, true, false, false);
        }
        /// <summary>
        /// 自分自身のメソッドの名前をカッコ無しで取って来ます。（例：　"実行中のメソッド:"+ getMethodName() → "実行中のメソッド:thisMethod"）
        /// </summary>
        public static string getMethodName()
        {
            // getMethodName(int)メソッドへの呼び出し回数が増えるため、1足さないと整合性が取れない
            string _methodName = getMethodName(1);
            return _methodName;
        }
        /// <summary>
        /// 自分自身のメソッドの英語名だけをカッコ無しで取って来ます。（※メソッド名が「EnglishName()」もしくは「"EnglishName+"・"+日本語名()」になっている必要があります。そうでない場合はとりあえず "メソッド名" 全体を取得します。）
        /// </summary>
        public static string getMethodName_English()
        {
            // getMethodName(int)メソッドへの呼び出し回数が増えるため、1足さないと整合性が取れない
            string _methodName_English = MyTools.getWord_InString(getMethodName(1), 0, 1, EStringCharType.t0b_English_半角英数字＿記号を含む, false, "・");
            if (_methodName_English == "") _methodName_English = getMethodName(1);
            return _methodName_English;
        }
        /// <summary>
        /// 自分自身のメソッドの日本語名だけをカッコ無しで取って来ます。（※メソッド名が「"EnglishName"+"・"+"日本語名"()」になっている必要があります。そうでない場合はとりあえず "メソッド名" 全体を取得します。）
        /// </summary>
        public static string getMethodName_Japanese()
        {
            // getMethodName(int)メソッドへの呼び出し回数が増えるため、1足さないと整合性が取れない
            string _methodName_Japanese = MyTools.getWord_InString(getMethodName(1), 0, 2, EStringCharType.t0c_Japanese2_日本語＿半角カタカナを含まない, false, "・");
            if (_methodName_Japanese == "") _methodName_Japanese = getMethodName(1);
            return _methodName_Japanese;
        }
        /// <summary>
        /// N回呼び出し元の「クラス名（一階層）.メソッド名:行番号」を取ってきます。現実装では、行番号が0になる欠陥があるので、privateにしています。publicなものはgetMethodNameを使ってください。
        /// </summary>
        private static string getMethodName_OnClassNameAndLineNo(int _StackTraceBackNum・0が自分自身＿1が自分を呼び出している呼び出し元メソッド＿2以降でも呼び出し元をたどれます, bool _isClassName_FullPath・クラス名を階層表示するか)
        {
            // getClassMethodNameメソッドへの呼び出し回数が増えるため、1足さないと整合性が取れない
            int _stackNum・0が自分自身 = 1 + _StackTraceBackNum・0が自分自身＿1が自分を呼び出している呼び出し元メソッド＿2以降でも呼び出し元をたどれます;
            return getClassMethodName(_stackNum・0が自分自身, false, true, false);
        }
        /// <summary>
        /// 呼び出し元の「クラス名（一階層）.メソッド名:行番号」を4回さかのぼって、"4回目呼び出し元 → 3回目 → 2回目 → 1回目 → 自分自身のメソッド名:行番号"の文字列を取ってきます。
        /// </summary>
        /// <returns></returns>
        public static string getMethodStackString()
        {
            string _stackString = "";
            // getMethodName_OnClassNameAndLineNoメソッドへの呼び出し回数が増えるため、1足さないと整合性が取れない
            //5->4>3->2->1_stackString += MyTools.getMethodName_OnClassNameAndLineNo(5, false) + " → " + MyTools.getMethodName_OnClassNameAndLineNo(4, false) + " → " + MyTools.getMethodName_OnClassNameAndLineNo(3, false) + " → " + MyTools.getMethodName_OnClassNameAndLineNo(2, false) + "\n → " + MyTools.getMethodName_OnClassNameAndLineNo(1, false);
            //1 <- 2 <- 3 -< 4
            _stackString += MyTools.getMethodName(1) + " ← " + MyTools.getMethodName(2) + " ← " + MyTools.getMethodName(3) + " ← " + MyTools.getMethodName(4);
            return _stackString;
        }
        /// <summary>
        /// 呼び出し元の「クラス名（一階層）.メソッド名:行番号」をN回さかのぼって、"N回目 → ... → 3回目 → 2回目 → 1回目 → \n → 自分自身のメソッド名:行番号"の文字列を取ってきます。
        /// </summary>
        /// <returns></returns>
        public static string getMethodStackString(int _StackTraceBackNum・1が自分でgetMethodを呼び出しているメソッド＿2がそのメソッドの呼び出し元)
        {
            // getClassMethodNameメソッドへの呼び出し回数が増えるため、1足さないと整合性が取れない
            int _own・1が自分自身 = 1 + 1;
            int _stackNum・1が自分自身 = 1 + _StackTraceBackNum・1が自分でgetMethodを呼び出しているメソッド＿2がそのメソッドの呼び出し元;
            return getMethodStackString(_own・1が自分自身, _stackNum・1が自分自身);
        }
        /// <summary>
        /// 呼び出し元の「クラス名（一階層）.メソッド名:行番号」をN回さかのぼって、"N回目 → ... → 3回目 → 2回目 → 1回目 → \n → 自分自身のメソッド名:行番号"の文字列を取ってきます。
        /// </summary>
        /// <returns></returns>
        public static string getMethodStackString(int _StackTaceBackNum_begin・メソッド表示開始呼び出し元回数＿1が自分自身で以下略, int _StackTraceBackNum・1が自分でgetMethodを呼び出しているメソッド＿2がそのメソッドの呼び出し元)
        {
            string _stackString = "";
            // getMethodName_OnClassNameAndLineNoメソッドへの呼び出し回数が増えるため、1足さないと整合性が取れない
            int _begin = _StackTaceBackNum_begin・メソッド表示開始呼び出し元回数＿1が自分自身で以下略;
            int _N = _StackTraceBackNum・1が自分でgetMethodを呼び出しているメソッド＿2がそのメソッドの呼び出し元;
            //5->4>3->2->1
            //for (int i = _N; i >= 1+1; i--)
            //{
            //    _stackString += MyTools.getMethodName_OnClassNameAndLineNo(i, false) + " → ";
            //}
            //// 最後に自分自身のメソッド名を追加
            //_stackString += " → " + MyTools.getMethodName_OnClassNameAndLineNo(0 + 1, false);
            //1<-2<-3...<-N
            for (int i = _begin; i <= _N-1; i++)
            {
                _stackString += MyTools.getMethodName(i) + " ← ";
            }
            // 最後にN回目呼び出し元のメソッド名を追加
            _stackString += MyTools.getMethodName(_N);
            return _stackString;
        }
        /// <summary>
        /// N回呼び出し元の「クラス名（一階層）.メソッド名:行番号」もしくは「メソッド名:行番号」もしくは「メソッド名」を取ってきます
        /// </summary>
        public static string getClassMethodName(int _StackTraceBackNum・0が自分自身＿1が呼び出し元のメソッド＿2以降でも呼び出し元をたどれます, bool _isMethodOnly・メソッド名だけか, bool _isShowMethodLineNum・行番号を表示するか, bool _isClassName_FullPath・クラス名を階層表示するか)
        {
            int _backNum・1が自分自身 = 1 + _StackTraceBackNum・0が自分自身＿1が呼び出し元のメソッド＿2以降でも呼び出し元をたどれます;
            StackTrace st = new StackTrace(false);
            StackFrame sf = st.GetFrame(_backNum・1が自分自身); // 0がこの自身クラスで，1が呼び出し元

            // クラス名・階層クラス名
            string _className = "";
            if (_isMethodOnly・メソッド名だけか == false)
            {
                if (_isClassName_FullPath・クラス名を階層表示するか == false)
                {
                    _className = sf.GetMethod().ReflectedType.Name;
                }
                else
                {
                    _className = sf.GetMethod().ReflectedType.FullName;
                }
                _className += ".";
            }

            // メソッド名・行番号
            string _methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
            string _methodLineNumStr = "";
            if (_isShowMethodLineNum・行番号を表示するか == true)
            {
                _methodLineNumStr = ":" + sf.GetFileLineNumber().ToString();
            }
            string _classMethodID = _className + _methodName + _methodLineNumStr;
            return _classMethodID;
        }
        #region メモ
        // [Tools][メソッド名][呼び出し]呼び出しもとのクラス名とメソッド名を取ってくるには，StackTraceクラスのインスタンスstの，GetFrame(0:自分自身のメソッド)や(1:1つ前の呼び出し先)の，GetFrame(0or1).GetMethod().ReflectedType.FullNameやGetMethod().Nameを使う
        /*public string getCalledClassAndMethodName(){
        GetFrame(CalledMethodFrameNum)?? // 呼び出しもとの呼び出し元？
        }
        */
        /* http://www.atmarkit.co.jp/bbs/phpBB/viewtopic.php?topic=43758&forum=7&7
        //自メソッドの第一引数名を取得
        string paramName = (new System.Diagnostics.StackTrace()).GetFrame(0).GetMethod().GetParameters()[0].Name;


        //改良版
        string paramName = System.Reflection.MethodBase.GetCurrentMethod().GetParameters()[0].Name;
        */
        /*
        StackTrace st = new StackTrace(false);
        StackFrame sf = st.GetFrame(rM.CalledMethodFrameNum); // 0がこの自身クラス？，1が呼び出し元？
        string className = sf.GetMethod().ReflectedType.FullName;
        string methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
        string classMethodID = className + "." + methodName;
        //return classMethodID;

        return getRandomNum_readPastOrSave(randomNum, classMethodID);
        */
        #endregion
        
        #endregion

        // クラスインスタンスのディープコピー（Clone()メソッドなどに代表される、メモリの中身コピーする方法）
        #region クラスインスタンスのディープコピー: DeepCopy()
        /// <summary>
        /// クラスインスタンスのディープコピー（Clone()メソッドなどに代表される、新しくメモリを別々に持ったインスタンス）を返します。
        /// </summary>
        public static object DeepCopy(object _sourcedObject) // (this object _sourcedObject) と書いていたが、それだとエラーになる
        {
            // 参考。感謝。http://d.hatena.ne.jp/tekk/20100131/1264913887
            object result;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter b = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, _sourcedObject);
                mem.Position = 0;
                result = b.Deserialize(mem);
            }
            finally
            {
                mem.Close();
            }

            return result;

        }
        /// <summary>
        /// クラスインスタンスのディープコピー（Clone()メソッドなどに代表される、新しくメモリを別々に持ったインスタンス）を返します。
        /// </summary>
        public static T DeepCopy<T>(T _sourcedObject)
        {
            T result;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter b = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            MemoryStream mem = new MemoryStream();

            try
            {
                b.Serialize(mem, _sourcedObject);
                mem.Position = 0;
                result = (T)b.Deserialize(mem);
            }
            finally
            {
                mem.Close();
            }

            return result;
        }
        #endregion

        // 計算式・関数
        #region 切り捨て・四捨五入の計算: getKirisuteValue/getSisyagonyuValue
        /// <summary>
        /// 引数の小数部分を、四捨五入した整数を返します。
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int getSisyagonyuValue(double _value)
        {
            // 四捨五入のやり方：　http://dobon.net/vb/dotnet/programing/round.html
            int _sisyagonyuValue = 0;
            _sisyagonyuValue = (int)Math.Round(_value, MidpointRounding.AwayFromZero);
            return _sisyagonyuValue;
        }
        /// <summary>
        /// 引数の小数部分を小数(N+1)位を四捨五入して、小数第N位までにした小数を返します。
        /// </summary>
        public static double getSisyagonyuValue(double _value, int N_kirisuteNai_KetaSuu)
        {
            return Math.Round(_value, N_kirisuteNai_KetaSuu, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// 引数の小数部分を切り上げた整数を返します。
        /// ※「(int)_value+0.5」といっしょです。
        /// </summary>
        public static int getKiriageValue(double _value)
        {
            return (int)(_value + 0.5);
        }
        /// <summary>
        /// 引数の小数部分を切り捨てた整数を返します。
        /// ※「(int)_value」との違いは、マイナスの数だと絶対値の大きい値になることです。（例： (int)(-1.1)=-1だが、getKirisuteValue(-1.1)=-2.0）
        /// </summary>
        public static int getKirisuteValue(double _value)
        {
            int _kirisuteValue = (_value > 0) ? (int)_value : (int)Math.Ceiling(_value);
            return _kirisuteValue;
        }
        /// <summary>
        /// 引数の小数部分を小数(N+1)位以下を切り捨てて、小数第N位までにした小数を返します。
        /// </summary>
        public static double getKirisuteValue(double _value, int N_kirisuteNai_KetaSuu)
        {
            double _yuukouketasuuYouBaiSuu = System.Math.Pow(10, N_kirisuteNai_KetaSuu);
            // 切り捨てには、意見が分かれるらしい。　参考：　http://q.hatena.ne.jp/1186199731
            // 正の数だったらFloor、負の数だったらCeilingを使って切り捨てる
            // (int)_valueとの違いは、マイナスの数だと絶対値の大きい値になることです。（例： (int)(-1.1)=-1だが、getKirisuteValue(-1.1)=-2.0）
            double _kirisuteValue = _value > 0 ? System.Math.Floor(_value * _yuukouketasuuYouBaiSuu) / _yuukouketasuuYouBaiSuu :
                                                 System.Math.Ceiling(_value * _yuukouketasuuYouBaiSuu) / _yuukouketasuuYouBaiSuu;
            return _kirisuteValue;
        }
        #endregion
        #region 対数関数（Log）の計算: Log
        /// <summary>
        /// log_a(X)を求めて返します．aやXに不適切な値が入った場合は，デフォルト値である0.0を返します．
        /// （※Math.Log(a, X)では，「NaN」となり-214...という変な値になるので，とりあえずデフォルト値は0.0の方がいいということで作ったメソッド）．
        /// </summary>
        /// <param name="_log_a・基底数a"></param>
        /// <param name="_y・入力値_aのなんとか乗がy"></param>
        /// <returns></returns>
        public static double Log(double _log_a・基底数a, double _y・入力値_aのなんとか乗がy)
        {
            double _answer = 0.0;
            if (_log_a・基底数a == 0.0)
            {
                return _answer;
            }
            if (_y・入力値_aのなんとか乗がy == 0.0)
            {
                return _answer;
            }
            _answer = Math.Log(_y・入力値_aのなんとか乗がy, _log_a・基底数a);
            return _answer;
        }
        /// <summary>
        /// log_a(X)を求めて返します．aやXに不適切な値が入った場合は，デフォルト値である0.0を返します．
        /// （※Math.Log(a, X)では，「NaN」となり-214...という変な値になるので，とりあえずデフォルト値は0.0の方がいいということで作ったメソッド）．
        /// </summary>
        /// <param name="_log_a・基底数a"></param>
        /// <param name="_y・入力値_aのなんとか乗がy"></param>
        /// <returns></returns>
        public static double Log(int _log_a・基底数a, int _x・入力値_aのなんとか乗がx)
        {
            return Log((double)_log_a・基底数a, (double)_x・入力値_aのなんとか乗がx);
        }
        #endregion
        #region 数値を値別にＦ～Ｓの評価に判別して取得する getERank_FtoS
        /// <summary>
        /// ランクを示すenum型列挙体です。
        /// ToString()にすると、要素名（"F","E"など）が取れます。
        /// (int)型に変更すると、要素の値として、比較可能な強さ（None=0,F=1,E=2,D=3,C=4,B=5,A=6,S=7,SS=8,SSS=9,SSSS=10,COUNT=11）を取得できます。。
        /// getERank_FtoSなどで使います。
        /// 
        /// 　※それぞれの値が取る範囲を設定したい場合は、setERank()を使ってください。
        /// </summary>
        public enum ERank
        {
            /// <summary>
            /// (int)=0。クラスＦ未満か、何も設定されていません。
            /// </summary>
            None,
            /// <summary>
            /// (int)=1。クラスＦ以上Ｅ未満です。
            /// </summary>
            F,
            /// <summary>
            /// (int)=2。クラスＥ以上Ｄ未満です。
            /// </summary>
            E,
            /// <summary>
            /// (int)=3。クラスＤ以上Ｃ未満です。
            /// </summary>
            D,
            /// <summary>
            /// (int)=4。クラスＣ以上Ｂ未満です。
            /// </summary>
            C,
            /// <summary>
            /// (int)=5。クラスＢ以上Ａ未満です。
            /// </summary>
            B,
            /// <summary>
            /// (int)=6。クラスＡ以上Ｓ未満です。
            /// </summary>
            A,
            /// <summary>
            /// (int)=7。クラスＳ以上ＳＳ未満です。
            /// </summary>
            S,
            /// <summary>
            /// (int)=8。クラスＳＳ以上ＳＳＳ未満です。
            /// </summary>
            SS,
            /// <summary>
            /// (int)=9。クラスＳＳＳ以上ＳＳＳＳ未満です。
            /// </summary>
            SSS,
            /// <summary>
            /// (int)=10。クラスＳＳＳＳ以上です。測定できないクラスＯＶＥＲもここ。
            /// </summary>
            SSSS,
            /// <summary>
            /// (int)=このEnumの要素数です。
            /// </summary>
            COUNT // これで要素数が取れる。
        }
        /// <summary>
        /// ERank型の各ランクそれぞれが取る最小値を格納している配列です。ここで初期化せず、setERank()か、getERank()を使ってください。
        /// </summary>
        private static double[] p_ERank_EachValueMins;
        /// <summary>
        /// Ｆ～Ｓ…のクラス分けしたenum型の、各クラスと判定される、最小値を設定します。
        /// 
        /// 第１引数にそれぞれクラスＦ～Ｓ～ＳＳＳＳの最小値を１０個の配列[1]～[10]に順番に入れてください。[0]は無視しますので0などで結構です。
        /// これを一度も呼び出していない場合は、F1-SSSS100のデフォルト値{ 0,1,20,40,50,60,70,90,95,98,100 }が予め入っています。
        /// </summary>
        public static void setERank_FtoS_ByMin(double[] _minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS)
        {
            if (_minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS.Length >= 11)
            {
                p_ERank_EachValueMins = null;
                p_ERank_EachValueMins = _minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS;
            }
        }
        /// <summary>
        /// Ｆ～Ｓ…のクラス分けしたenum型の、各クラスと判定される、最小値を設定します。
        /// 
        /// 引数にそれぞれクラスＦ～Ｓ～ＳＳＳＳの最小値に入れてください。
        /// これを一度も呼び出していない場合は、F1-SSSS100のデフォルト値{ 0,1,20,40,50,60,70,90,95,98,100 }が予め入っています。
        /// </summary>
        public static void setERank_FtoS_ByMin(double _F_min, double _E_min, double _D_min, double _C_min, double _B_min, double _A_min, double _S_min, double _SS_min, double _SSS_min, double _SSSS_min)
        {
            setERank_FtoS_ByMin(new double[]{0, _F_min, _E_min, _D_min, _C_min, _B_min, _A_min, _S_min, _SS_min, _SSS_min, _SSSS_min });
        }
        /// <summary>
        /// Ｆ～Ｓ…のクラス分けしたenum型の、各クラスと判定される、含有率（がんゆうりつ）を設定します。
        /// 全部で１００％にしなくても、内部で合計１００％になるように自動で調整するので、整数でも％でも小数でも万単位でも、好きな単位で入れてください。
        /// 
        /// 引数にそれぞれクラスＮＯＮＥ（boolFalse_無）～Ｆ～Ｓ～ＳＳＳＳの含有率に入れてください。
        /// これを一度も呼び出していない場合は、F1-SSSS100のデフォルト値{1,19,20,10,10,10,20, 5, 3, 2 ,1}（　　以下の最小値から計算{ 0,1,20,40,50,60,70,90,95,98,100 }）が予め入っています。
        /// </summary>
        public static void setERank_FtoS_ByInclusingRate(double _None_rate, double _F_rate, double _E_rate, double _D_rate, double _C_rate, double _B_rate, double _A_rate, double _S_rate, double _SS_rate, double _SSS_rate, double _SSSS_rate)
        {
            p_ERank_EachValueMins = null;
            double _rateSum = _None_rate + _F_rate + _E_rate + _D_rate + _C_rate + _B_rate + _A_rate + _S_rate + _SS_rate + _SSS_rate + _SSSS_rate;
            double _p100 = 100.0;
            // それぞれの含まれる率から、値の取りうる範囲を０～１００％として、最小値を計算            
            double _F_min =             _None_rate / _rateSum * _p100;
            double _E_min = _F_min +    _F_rate / _rateSum * _p100;
            double _D_min = _E_min +    _E_rate / _rateSum * _p100;
            double _C_min = _D_min +    _D_rate / _rateSum * _p100;
            double _B_min = _C_min +    _C_rate / _rateSum * _p100;
            double _A_min = _B_min +    _B_rate / _rateSum * _p100;
            double _S_min = _A_min +    _A_rate / _rateSum * _p100;
            double _SS_min = _S_min +   _S_rate / _rateSum * _p100;
            double _SSS_min = _SS_min + _SS_rate / _rateSum * _p100;
            double _SSSS_min = _SSS_min + _SSS_rate / _rateSum * _p100;
            p_ERank_EachValueMins = new double[] { 0, _F_min, _E_min, _D_min, _C_min, _B_min, _A_min, _S_min, _SS_min, _SSS_min, _SSSS_min };
        }
        /// <summary>
        /// Ｆ～Ｓ…のクラス分けしたenum型の、各クラスと判定される、含有率（がんゆうりつ）を設定します。
        /// 全部で１００％にしなくても、内部で合計１００％になるように自動で調整するので、整数でも％でも小数でも万単位でも、好きな単位で入れてください。
        /// 
        /// 引数にそれぞれクラスＮＯＮＥ（boolFalse_無）～Ｆ～Ｓ～ＳＳＳＳの含有率に入れてください。
        /// これを一度も呼び出していない場合は、F1-SSSS100のデフォルト値{1,19,20,10,10,10,20, 5, 3, 2 ,1}（　　以下の最小値から計算{ 0,1,20,40,50,60,70,90,95,98,100 }）が予め入っています。
        /// </summary>
        public static void setERank_FtoS_ByInclusingRate(double[] _InclusingRate_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS)
        {
            double[] _rates = _InclusingRate_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS;
            if (_rates.Length >= 11)
            {
                setERank_FtoS_ByInclusingRate(_rates[0], _rates[1], _rates[2], _rates[3], _rates[4], _rates[5], _rates[6], _rates[7], _rates[8], _rates[9], _rates[10]);
            }
        }
        /// <summary>
        /// 引数に指定した値_valueを、setERankメソッドで設定したＦ～Ｓ…のクラス分けしたenumのERank型で返します。
        /// 設定してない場合は、F1-SSSS100のデフォルト値{ 0,1,20,40,50,60,70,90,95,98,100 }が使われます。
        ///         ToString()にすると、要素名（"F","E"など）が取れます。
        /// (int)型に変更すると、要素の値として、比較可能な強さ（None=0,F=1,E=2,D=3,C=4,B=5,A=6,S=7,SS=8,SSS=9,SSSS=10,COUNT=11）を取得できます。。
        /// </summary>
        /// <returns></returns>
        public static ERank getERank_FtoS(double _value)
        {
            if (p_ERank_EachValueMins == null)
            {
                // 初期化処理（setERankなどで設定しない場合、ここの値がデフォルト）
                //                                  None, F, E, D, C, B, A, S,SS,SSS,SSSS
                p_ERank_EachValueMins = new double[] { 0,1,20,40,50,60,70,90,95,98,100 };
            }
            ERank _rank = ERank.None;
            if (_value < p_ERank_EachValueMins[1]) return ERank.None;
            if (_value < p_ERank_EachValueMins[2]) return ERank.F;
            if (_value < p_ERank_EachValueMins[3]) return ERank.E;
            if (_value < p_ERank_EachValueMins[4]) return ERank.D;
            if (_value < p_ERank_EachValueMins[5]) return ERank.C;
            if (_value < p_ERank_EachValueMins[6]) return ERank.B;
            if (_value < p_ERank_EachValueMins[7]) return ERank.A;
            if (_value < p_ERank_EachValueMins[8]) return ERank.S;
            if (_value < p_ERank_EachValueMins[9]) return ERank.SS;
            if (_value < p_ERank_EachValueMins[10]) return ERank.SSS;
            if (_value >= p_ERank_EachValueMins[10]) return ERank.SSSS;

            return _rank;
        }
        /// <summary>
        /// 第１引数に指定した値_valueを、Ｆ～Ｓ…のクラス分けしたenum型のERank型で返します。第２引数にそれぞれクラスＦ～Ｓ～ＳＳＳＳの最小値を１０個の配列[1]～[10]に順番に入れてください。[0]は無視しますので0などで結構です。
        ///             ToString()にすると、要素名（"F","E"など）が取れます。
        /// (int)型に変更すると、要素の値として、比較可能な強さ（None=0,F=1,E=2,D=3,C=4,B=5,A=6,S=7,SS=8,SSS=9,SSSS=10,COUNT=11）を取得できます。。
        /// </summary>
        /// <returns></returns>
        public static ERank getERank_FtoS(double _value, double[] _minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS)
        {
            ERank _returnedRank = ERank.None;
            double[] _min = _minValues_inEachClass_Index0None_1F_2E_3D_4C_5B_6A_7S_8SS_9SSS_10SSSS;
            if (_min.Length >= (int)ERank.COUNT - 1)
            {
                _returnedRank = getERank_FtoS(_value, _min[1], _min[2], _min[3], _min[4], _min[5], _min[6], _min[7], _min[8], _min[9], _min[10]);
            }
            return _returnedRank;
        }
        /// <summary>
        /// 引数に指定した値_valueを、Ｆ～Ｓ…のクラス分けしたenum型のERank型で返します。引数にそれぞれの最小値を順番に入れてください。
        ///             ToString()にすると、要素名（"F","E"など）が取れます。
        /// (int)型に変更すると、要素の値として、比較可能な強さ（None=0,F=1,E=2,D=3,C=4,B=5,A=6,S=7,SS=8,SSS=9,SSSS=10,COUNT=11）を取得できます。。
        /// </summary>
        /// <returns></returns>
        public static ERank getERank_FtoS(double _value, double _F_min, double _E_min, double _D_min, double _C_min, double _B_min, double _A_min, double _S_min, double _SS_min, double _SSS_min, double _SSSS_min)
        {
            ERank _rank = ERank.None;
            if (_value < _F_min) return ERank.None;
            if (_value < _E_min) return ERank.F;
            if (_value < _D_min) return ERank.E;
            if (_value < _C_min) return ERank.D;
            if (_value < _B_min) return ERank.C;
            if (_value < _A_min) return ERank.B;
            if (_value < _S_min) return ERank.A;
            if (_value < _SS_min) return ERank.S;
            if (_value < _SSS_min) return ERank.SS;
            if (_value < _SSSS_min) return ERank.SSS;
            if (_value >= _SSSS_min) return ERank.SSSS;
            
            return _rank;
        }
        /// <summary>
        /// 現在設定されているERankの情報（それぞれのランクに判定される値の範囲）を示した文字列を返します。
        /// </summary>
        /// <returns></returns>
        public static string getERankInfo()
        {
            if (p_ERank_EachValueMins == null)
            {
                getERank_FtoS(0); // getを呼び出してnullのプロパティを初期化
            }
            string _info = "【ERank情報】\t＜全体に含まれる％＞（0を無、1～100を有効ランクに当てはめるので、合計101％）\n";
            int _lastindex = p_ERank_EachValueMins.Length-1;
            double _beforeMin = 0;
            double _max = p_ERank_EachValueMins[_lastindex];
            for (int i = 0; i < p_ERank_EachValueMins.Length; i++ )
            {
                double _min = p_ERank_EachValueMins[i];
                double _nextMin = 100;      // ～未満の値を計算するために必要な、次の配列の最小値
                string _mimanString = "";   // ～未満と表示するかどうか
                string _hukumuString = "";  // （ランク無）と表示するかどうか
                if (i == 0) // 最初
                {
                    _hukumuString = "（ランク無）";
                    if (_lastindex >= 1) // 配列数が1の時はエラーになるので、確認
                    {
                        _nextMin = p_ERank_EachValueMins[1];
                        _mimanString = _nextMin.ToString() + "未満";
                    }
                }
                else if (i == _lastindex) // 最後
                {
                    if (_min == 100)
                    {
                        _nextMin = 101; // 100％も１％に含める
                    }
                    else
                    {
                        _nextMin = 100;
                    }
                    _mimanString = "";
                }
                else // 最初と最後以外
                {
                    _nextMin = p_ERank_EachValueMins[i + 1];
                    _mimanString = _nextMin.ToString() + "未満";
                }
                _info += "Rank " + getERank_FtoS(_min).ToString() + ": ";
                // 値の範囲
                _info += _min + "以上" + _mimanString;
                // 全体に含まれる率
                _info += "\t\t\t＜" + ((_nextMin - _min) / _max) * 100.0 + "％" + _hukumuString + "＞" + "\n";
                _beforeMin = _min;
            }
            return _info;
        }
        #endregion

        // 乱数
        #region 乱数を発生させる : getRandomNum
        static Random p_random = new Random(); // 引数を無しで生成すると、毎回違う乱数を発生させるが、その乱数を管理したい場合があるのでstaticに生成。
        static Random p_random_SameRandomValue_PerGet = new Random(1); // seedに定数を与えると、毎回同じ乱数を発生させる。  
        /// <summary>
        /// 最小値以上～最大値【以下】（これらの値も含む）までのInt型の乱数を返します．
        /// 
        /// ※普通のRandom.Next(min, max)だと、最小値以上～最大値「未満」になるので、Random.Next(min, max+1)の「+1」をよく見落とすため、このクラスを使っています。
        /// </summary>
        /// <param name="_minVale"></param>
        /// <param name="_maxVale"></param>
        /// <returns></returns>
        public static int getRandomNum(int _minValue, int _maxValue)
        {
            // min < max か確認（どちらかがオーバーフローしている場合など）
            if (_minValue > _maxValue)
            {
                // maxがオーバーフローしていないか確認し、していたらInt32.Max-1にする
                if (_maxValue < 0) _maxValue = Int32.MaxValue - 1;
                // それでも min < max だったら、min = max にする
                if (_minValue > _maxValue) _minValue = _maxValue;
            }
            return p_random.Next(_minValue, _maxValue + 1);
        }
        // Random.Next(long型, long型)は存在しない。
        /// <summary>
        /// 最小値以上～最大値【以下】（これらの値も含む）までのlong型の乱数を返します．
        /// ※自作しているので、(_minValue - _maxValue)がInt32.Maxより大きい場合は不完全です（物理的に厳しいので、大きめの_maxValue-Int32.Max～_maxValueを返す）。
        /// 
        /// ※普通のRandom.Next(min, max)だと、最小値以上～最大値「未満」になるので、Random.Next(min, max+1)の「+1」をよく見落とすため、このクラスを使っています。
        /// </summary>
        /// <param name="_minVale"></param>
        /// <param name="_maxVale"></param>
        /// <returns></returns>
        public static long getRandomNum(long _minValue, long _maxValue)
        {
            long _randomNum = _maxValue;
            int _distance = (int)(_maxValue - _minValue);
            // ディスタンスがオーバーフローしてたら（正の数のはずなのに、負の数になってたら）、
            // 無理だから桁数を重視して返す。
            if (_distance < 0)
            {
                //(a)上の桁数を重視して、下の桁数を減らして、返す
                long _N = 1;
                while (_distance < 0)
                {
                    _N *= 10;
                    _distance = (int)(_maxValue / _N - _minValue / _N);
                }
                _randomNum = _minValue + p_random.Next(0, _distance) * _N;
                //(b)_maxValue-Int32.Max～_maxValueを返す
                //_randomNum = (_maxValue - p_random.Next(0, Math.Min(Int32.MaxValue, Math.Abs((int)_maxValue))));
            }
            else
            {
                _randomNum = _minValue + p_random.Next(0, _distance);
            }
            return _randomNum;
        }
        #region 草案　たぶん引数がdouble型のはともかく、引数がdouble型のは要らないと思う。後で割ればいいだけの話だから。精度もintの桁数で判断すればよい
        ///// <summary>
        ///// 最小値～最大値（これらの値も含む）までのDouble型の乱数を返します．
        ///// </summary>
        ///// <param name="_minVale"></param>
        ///// <param name="_maxVale"></param>
        ///// <returns></returns>
        //public static double getRandomNum(double _minValue, double _maxValue_equals_EnumIntMax)
        //{
        //    //できない！！　質問中：　http://dobon.net/vb/dotnet/programing/random.html
            
        //    // なんかintで(+1)で乱数出して，有効数字のけた数が上がったら1.0もOKにする，という感じ？
        //    int _randomNum_Check = getRandomNum((int)_minValue, (int)(_maxValue_equals_EnumIntMax+1));

        //    // _xが鍵？
        //    double _x = ?;
        //    double _random0_to_1_NOTInclude1 =  p_random.NextDouble();
        //    double _random0_to_1 = _random0_to_1_NOTInclude1 + _x;
        //    double _decidedDiceNum = _random0_to_1*(_maxValue_equals_EnumIntMax-_minValue) + _minValue;
        //    return _decidedDiceNum;
        //}
        #endregion
        /// <summary>
        /// 毎回同じ乱数を生成する，最小値以上～最大値【以下】（これらの値も含む）までのInt型の乱数を返します．
        /// 
        /// ※普通のRandom.Next(min, max)だと、最小値以上～最大値「未満」になるので、Random.Next(min, max+1)の「+1」をよく見落とすため、このクラスを使っています。
        /// </summary>
        /// <param name="_minVale"></param>
        /// <param name="_maxVale"></param>
        /// <returns></returns>
        public static int getRandomNum_SaveValue_PerGet(int _minValue, int _maxValue)
        {
            int _randomNum = p_random_SameRandomValue_PerGet.Next(_minValue, _maxValue+1);
            return _randomNum;
        }
        /// <summary>
        /// 暗号化に使用する様な厳密なランダムバイト配列を作成します。
        /// </summary>
        /// <param name="_byteNum"></param>
        /// <returns></returns>
        public static byte[] getRandomByte_HighSecurity(int _byteNum){
            // 参考URL: http://dobon.net/vb/dotnet/programing/random.html
            //[C#]
            //暗号化に使用する厳密なランダムバイトを作成する
            //100バイト長のバイト型配列を作成
            byte[] random = new byte[_byteNum];

            //RNGCryptoServiceProviderクラスのインスタンスを作成
            System.Security.Cryptography.RNGCryptoServiceProvider rng =
                new System.Security.Cryptography.RNGCryptoServiceProvider();
            //または、次のようにもできる
            //System.Security.Cryptography.RandomNumberGenerator rng =
            //    System.Security.Cryptography.RandomNumberGenerator.Create();


            //バイト配列に暗号化に使用する厳密な値のランダムシーケンスを設定
            rng.GetBytes(random);

            //バイト配列に暗号化に使用する厳密な0以外の値の
            //ランダムシーケンスを設定するには次のようにする
            //rng.GetNonZeroBytes(random);

            return random;
        }
        #endregion
        #region 標準正規分布に沿った最小値～平均～最大値までをランダムに取ってくる getSeikiRandomNum
        // int型
        /// <summary>
        /// 標準正規分布に沿った最小値～平均～最大値までをランダムに取ってきます。
        /// 
        /// 【参考】：　平均値である分散±0は40％弱（約２回に一回）、
        /// 分散±1は24％弱（約４回に一回）、
        /// 分散±2は5％強（約２０回に１回）、
        /// 分散±3は0.5％弱（約２００回に一回）、
        /// 分散±4は0.01％強（約１万回に１回）、
        /// 分散±5は0.0001％強（約１００万回に１回）、
        /// です。
        /// 
        /// 　　　　　　　　　【説明】正規分布とは、なだらかな山を描く確率分布のことです。
        /// 平均値を取る確率が４０％弱で、極端な値を取るものほど確率がなだらかに低くなっていくというもので、
        /// 天才の生まれる確率や実力の差の分布など、様々な自然法則や事象に適応できると考えられています。
        /// 標準正規分布とは、その中でも分散（ばらつき）が1、平均値が0のものをこう呼びます。
        /// </summary>
        public static int getSeikiRandomNum_RealWorldRate(int _average, int _min, int _max)
        {
            int _seikiRandomNum = MyTools.getSisyagonyuValue(getSeikiRandomNum_RealWorldRate((double)_average, (double)_min, (double)_max));
            return _seikiRandomNum;
        }
        /// <summary>
        /// 指定した範囲（_bunsanMin～_bunsanMax）の範囲内で、
        /// 正規分布に沿った最小値～平均～最大値までをランダムに取ってきます。
        /// 
        /// 【参考】：　平均値である分散±0は40％弱（約２回に一回）、
        /// 分散±1は24％弱（約４回に一回）、
        /// 分散±2は5％強（約２０回に１回）、
        /// 分散±3は0.5％弱（約２００回に一回）、
        /// 分散±4は0.01％強（約１万回に１回）、
        /// 分散±5は0.0001％強（約１００万回に１回）、
        /// です。
        /// </summary>
        public static int getSeikiRandomNum_RealWorldRate(double _average, int _min, int _max, double _bunsanMin, double _bunsanMax)
        {
            return MyTools.getSisyagonyuValue(getSeikiRandomNum_RealWorldRate(_average, (double)_min, (double)_max, _bunsanMin, _bunsanMax, Double.NaN));
        }
        /// <summary>
        /// 指定した分散値（_OneChoicedBunsan_Or_RandomChoiceIsDouble_NaN）位平均からバラツキのある、
        /// 正規分布に沿った最小値～平均～最大値までをランダムに取ってきます。
        /// 非凡なものが生まれる確率を操作して、意図して天才や凡人を生み出したい場合などに利用できます。
        /// 
        /// 【参考】：　平均値である分散±0は40％弱（約２回に一回）、
        /// 分散±1は24％弱（約４回に一回）、
        /// 分散±2は5％強（約２０回に１回）、
        /// 分散±3は0.5％弱（約２００回に一回）、
        /// 分散±4は0.01％強（約１万回に１回）、
        /// 分散±5は0.0001％強（約１００万回に１回）、
        /// です。
        /// </summary>
        public static int getSeikiRandomNum_RealWorldRate(double _average, int _min, int _max, double _OneChoicedBunsan_Or_RandomChoiceIsDouble_NaN)
        {
            return MyTools.getSisyagonyuValue(getSeikiRandomNum_RealWorldRate(_average, (double)_min, (double)_max, p_SEIKIBUNSAN_MIN, p_SEIKIBUNSAN_MAX, _OneChoicedBunsan_Or_RandomChoiceIsDouble_NaN));
        }
        // double型
        /// <summary>
        /// 標準正規分布に沿った最小値～平均～最大値までをランダムに取ってきます。
        /// 
        /// 【参考】：　平均値である分散±0は40％弱（約２回に一回）、
        /// 分散±1は24％弱（約４回に一回）、
        /// 分散±2は5％強（約２０回に１回）、
        /// 分散±3は0.5％弱（約２００回に一回）、
        /// 分散±4は0.01％強（約１万回に１回）、
        /// 分散±5は0.0001％強（約１００万回に１回）、
        /// です。
        /// </summary>
        public static double getSeikiRandomNum_RealWorldRate(double _average, double _min, double _max)
        {
            return MyTools.getSeikiRandomNum_RealWorldRate(_average, _min, _max, p_SEIKIBUNSAN_MIN, p_SEIKIBUNSAN_MAX, Double.NaN);
        }
        /// <summary>
        /// 指定した範囲（_bunsanMin～_bunsanMax）の範囲内で、平均からバラツキのある、
        /// 正規分布に沿った最小値～平均～最大値までをランダムに取ってきます。
        /// 
        /// ※最後の引数である分散値（_OneChoicedBunsan）を指定すると、
        /// 意図的にそれ位バラツキのあるキー値をワンチョイス（抜擢）できます。
        /// 非凡なものが生まれる確率を操作して、意図して天才や凡人を生み出したい場合などに利用できます。
        /// 指定せず運任せで取りたい場合は、Double.NaNを代入してください。
        /// 
        /// 【参考】：　平均値である分散±0は40％弱（約２回に一回）、
        /// 分散±1は24％弱（約４回に一回）、
        /// 分散±2は5％強（約２０回に１回）、
        /// 分散±3は0.5％弱（約２００回に一回）、
        /// 分散±4は0.01％強（約１万回に１回）、
        /// 分散±5は0.0001％強（約１００万回に１回）、
        /// です。
        /// 
        /// 　　　　　　　　　【説明】正規分布とは、なだらかな山を描く確率分布のことです。
        /// 平均値を取る確率が４０％弱で、極端な値を取るものほど確率がなだらかに低くなっていくというもので、
        /// 天才の生まれる確率や実力の差の分布など、様々な自然法則や事象に適応できると考えられています。
        /// 標準正規分布とは、その中でも分散（ばらつき）が1、平均値が0のものをこう呼びます。
        /// 
        /// 
        /// 　　※このメソッドでは、予め格納した標準正規分布の確率分布テーブルを使って算出しています。
        /// テーブルは、Excelでここを参考につくったよ　http://www.relief.jp/itnote/archives/003121.php
        /// </summary>
        public static double getSeikiRandomNum_RealWorldRate(double _average, double _min, double _max, double _bunsanMin, double _bunsanMax, double _OneChoicedBunsan_Or_RandomChoiceIsDouble_NaN)
        {
            double _value = 0;

            // 時間計測。ほぼ全部0msecだったからいらない
            //Stopwatch _w1 = new Stopwatch();
            //_w1.Start();
            //int _time1 = getNowTime_fast();

            // 【注意】newするstaticプロパティ・クラスの初期化は必ずメソッド内でnullでないか確かめてから！
            //          いくらstaticでも、staticメソッドの呼び出しの方が早い場合が多く、初期化されてない（null）ことが多々あるよ！

            // （初期化処理）確率参照用テーブルを作ってなかったら、作る
            if (p_seikiRate == null || p_seikiBunsan == null)
            {
                // 新しく作る
                // ■分散p_SEIKIBUNSAN_MIN～p_SEIKIBUNSAN_MAXまで。
                p_seikiRate = new double[] { 
// 配列の分散値は0.1刻みずつ。有効桁数は10桁。変えたければp_SEIKIBUNSAN_VALUEADDING_PER1INDEXを変えてね
0.0000014867 //-5
,0.0000024390 
,0.0000039613 
,0.0000063698 
,0.0000101409 
,0.0000159837 
,0.0000249425 
,0.0000385352 
,0.0000589431 
,0.0000892617 
,0.0001338302 //-4
,0.0001986555 
,0.0002919469 
,0.0004247803 
,0.0006119019 
,0.0008726827 
,0.0012322192 
,0.0017225689 
,0.0023840882 
,0.0032668191 
,0.0044318484 //-3
,0.0059525324 
,0.0079154516 
,0.0104209348 
,0.0135829692 
,0.0175283005 
,0.0223945303 
,0.0283270377 
,0.0354745928 
,0.0439835960 
,0.0539909665//-2 
,0.0656158148 
,0.0789501583 
,0.0940490774 
,0.1109208347 
,0.1295175957 
,0.1497274656 
,0.1713685920 
,0.1941860550 
,0.2178521770 
,0.2419707245//-1 
,0.2660852499 
,0.2896915528 
,0.3122539334 
,0.3332246029 
,0.3520653268 
,0.3682701403 
,0.3813878155 
,0.3910426940 
,0.3969525475 
,0.3989422804//0
,0.3969525475 
,0.3910426940 
,0.3813878155 
,0.3682701403 
,0.3520653268 
,0.3332246029 
,0.3122539334 
,0.2896915528 
,0.2660852499 
,0.2419707245 
,0.2178521770 
,0.1941860550 
,0.1713685920 
,0.1497274656 
,0.1295175957 
,0.1109208347 
,0.0940490774 
,0.0789501583 
,0.0656158148 
,0.0539909665 
,0.0439835960 
,0.0354745928 
,0.0283270377 
,0.0223945303 
,0.0175283005 
,0.0135829692 
,0.0104209348 
,0.0079154516 
,0.0059525324 
,0.0044318484 
,0.0032668191 
,0.0023840882 
,0.0017225689 
,0.0012322192 
,0.0008726827 
,0.0006119019 
,0.0004247803 
,0.0002919469 
,0.0001986555 
,0.0001338302 
,0.0000892617 
,0.0000589431 
,0.0000385352 
,0.0000249425 
,0.0000159837 
,0.0000101409 
,0.0000063698 
,0.0000039613 
,0.0000024390 
,0.0000014867 

 };
                // 新しく作る
                p_seikiBunsan = new double[p_seikiRate.Length];
            }
            if(p_seikiRate != null){            
                for (int i = 0; i < p_seikiRate.Length; i++)
                {
                    // ■分散値の代入
                    p_seikiBunsan[i] = p_SEIKIBUNSAN_MIN + (i * 0.1);
                }
                // 出来たの確認
                //ConsoleWriteLine(p_seikiBunsan.ToString());

            }


            // ■確率参照用テーブルをランダムで参照し、その確率より低かったら、その分散値に決める。
            // ランダムに決められる分散値
            double _choicedBunsan = Double.NaN; // 生まれた時の極端さ（値のバラツキ度合い）。まだ決まってない
            double _bureValue; // 値のバラツキ（ブレ幅）
            double _rareRate = 0.0; // 生まれた時のレア度
            int _birthNum = 0; // 生まれなおした回数
            int _rebirthNum_MAX = 100; // 生まれ直した回数の限界値（ループを止めるため）

            // 【注意】[Tips]判定はdouble.IsNaN(_value)で！　_value == Double.NaNではいつもtrueになるから気を付けて！
            if (double.IsNaN(_OneChoicedBunsan_Or_RandomChoiceIsDouble_NaN) == false)
            {
                // ランダムで決めず、ブンサンクン、君に決めた！、とワンチョイス（抜擢）する
                _choicedBunsan = _OneChoicedBunsan_Or_RandomChoiceIsDouble_NaN;
            }else{
                // ランダムで分散値を決める

                // 乱数を正規分布にしたがうように加工する

                // 分散値の範囲を設定
                // 指定された分散値の確率参照用テーブルだけ使用する
                // でたらめな値を入れてないか、引数をエラーチェック
                if (_max < _min) _max = _average;
                if (_max < _average) _max = _average;
                if (_min > _max) _min = _average;
                if (_min > _average) _min = _average;
                if (_bunsanMax < _bunsanMin) _bunsanMin = _bunsanMax; // どちらでもいいが、最小値の方がマイナスとか怖いので、最大値の方にあわせる
                // 分散の範囲をオーバーしないように、引数をエラーチェック
                if (_bunsanMin < p_SEIKIBUNSAN_MIN) _bunsanMin = p_SEIKIBUNSAN_MIN;
                if (_bunsanMax > p_SEIKIBUNSAN_MAX) _bunsanMax = p_SEIKIBUNSAN_MAX;

                // 分散値はソートされているので、範囲指定するだけ。一定値刻みもわかっているので、任意の値はそれぞれ１回で取り出せる。
                int _startIndex = (int)((_bunsanMin - p_SEIKIBUNSAN_MIN) / p_SEIKIBUNSAN_VALUEADDING_PER_1INDEX);
                int _endIndex = (p_seikiRate.Length - 1) - (int)((p_SEIKIBUNSAN_MAX - _bunsanMax) / p_SEIKIBUNSAN_VALUEADDING_PER_1INDEX);
                // （余談）関係ないけど、ソートのアルゴリズムの可視化。ここわかりやすいよ。 http://ufcpp.net/study/algorithm/sort.html

                // 分散値が決まるまで、ループ
                // 乱数の有効桁数を決定して、生起確率（レア度）をランダムに発生
                int _yukoutetasu = 8; // 小数第8桁、0.00000001、１億分の１まで取る。
                double _baiSuu = Math.Pow(10, _yukoutetasu);
                int _rateNum = 0;
                int _randomIndex = 0;
                while (double.IsNaN(_choicedBunsan) == true)
                {
                    _birthNum++;
                    // この子が生まれてきたときのレア度を生成
                    _rateNum = MyTools.getRandomNum(0, (int)(_baiSuu) - 1);
                    _rareRate = (double)_rateNum / _baiSuu;

                    // 範囲内のインデックスで、ランダムに一つ配列を選ぶ
                    _randomIndex = MyTools.getRandomNum(_startIndex, _endIndex);
                    // 生起確率が、配列の確率より小さかったら、チョイス（抜擢）。
                    if(_rareRate <= p_seikiRate[_randomIndex]){
                        _choicedBunsan = p_seikiBunsan[_randomIndex];
                        break;
                    }
                    else if (_birthNum > _rebirthNum_MAX)
                    {
                        // ちょっとアンタ生まれ直し過ぎやから、完全ランダムな分散値で我慢して（これすごいレアになりやすい）
                        _choicedBunsan = MyTools.getRandomNum( (int)(_bunsanMin*10), (int)(_bunsanMax*10) )/10.0;
                        break;
                    }
                    // もっかい生まれ直し
                }
            }
            // 分散値と引数（平均と最小値と最大値）から、
            // ■分散値から値のぶれ値（ブレ幅、平均値からどれくらい離れているか）を計算
            // (_choiceBunsan / Math.Abs(_bunsanMin)は0～1のはず。
            //  // _choicedBunsan / Math.Abs(p_SEIKIBUNSAN_MIN)*100.0 だと分散値5じゃないと100％が出ない
            double _bunsanPercent = _choicedBunsan/Math.Abs(_bunsanMin) * 100.0; // -100～100％。負の値もありうる
            double _percent = 50.0 + _bunsanPercent*(50.0/100.0); // 0～100の範囲にする
            _value = getHireiValue(_min, _average, _max, _percent);
  
            // ほぼ全部0msecだったからいらない
            //int _time2 = getNowTime_fast();
            //_w1.Stop();
            //Console.WriteLine((_time2-_time1)+"ミリ秒かかってるよ. StopWatch:"+_w1.ElapsedMilliseconds+"msec.");
            //これを標準出力するだけでかなり時間を食うので、できればコメントアウトしといてね
            //MyTools.ConsoleWriteLine("(…" + _birthNum + "回 生まれ変わって、次の子が生まれました。最後のレア度は" + _rareRate + "、分散は" + MyTools.getSisyagonyuValue(_ValuePercent_Min0ToAverage50ToMax100_MinuOrOver100PercentIsAlsoOK, 1) + ")\n");

            return _value;
        }
        /// <summary>
        /// 指定した基準となる最小値０％～最大値１００％となる値の、任意の％の値を比例関係を使って算出します。
        /// 
        /// </summary>
        /// <param name="_min">０％の時の基準となる最小値</param>
        /// <param name="_max">１００％の時の基準となる最大値</param>
        /// <param name="_ValuePercent_Min0ToAverage50ToMax100_MinuOrOver100PercentIsAlsoOK">任意の％。１００％以上や、－１０％とかでもＯＫ。ちゃんと値を計算します。</param>
        /// <returns></returns>
        private static double getHireiValue(double _min, double _max, double _ValuePercent_Min0ToAverage50ToMax100_MinuOrOver100PercentIsAlsoOK)
        {
            double _value_y = 0.0;
            // ％を確率に戻す。これがx
            double _x = _ValuePercent_Min0ToAverage50ToMax100_MinuOrOver100PercentIsAlsoOK / 100.0;
            // 直線y=ax+bで近似する場合、(0, _min)を通ることは確定なので、y=_value, x=_x, b=_min。よって、
            // _value = a*_x+_min。a=(_value-_min)/_x
            // このaが比例定数。普通は(100,_max)を通ることを利用して出す。ただし、xが0の時は出せないので注意
            double _a = 0.0;
            double _b = _min;
            if(_x==0){
                _value_y = _b;
            }else{
                _a = (_max-_min)/_x;
                _value_y = _a*_x+_min;
            }
            return _value_y;
        }
        /// <summary>
        /// 指定した基準となる最小値０％～中間値を５０％～最大値１００％となる値の、任意の％の値を比例関係を使って算出します。
        /// 
        /// </summary>
        /// <param name="_min">０％の時の基準となる最小値</param>
        /// <param name="_mid">５０％の時の基準となる中間値</param>
        /// <param name="_max">１００％の時の基準となる最大値</param>
        /// <param name="_ValuePercent_Min0ToAverage50ToMax100_MinuOrOver100PercentIsAlsoOK">任意の％。１００％以上や、－１０％とかでもＯＫ。ちゃんと値を計算します。</param>
        /// <returns></returns>
        private static double getHireiValue(double _min, double _mid, double _max, double _ValuePercent_Min0ToAverage50ToMax100_MinuOrOver100PercentIsAlsoOK)
        {
            // ％を確率に戻す。これがx
            double _x = _ValuePercent_Min0ToAverage50ToMax100_MinuOrOver100PercentIsAlsoOK / 100.0;
            // 直線y=ax+bで近似する場合、y=_value, x=_x、よって、
            // _value = a*_x+_R。a=(_value-_R)/_x
            // このaが比例定数。普通は２点(0, _min)と(1.0, _max)を通ることを利用して出すが、それじゃあ_midは使われないし、面白味に欠ける。
            // このaをどう求めるかがポイント。

            // 現在は(A')を使っている


            // (A)min～maxの1直線、y = ax+b で計算。（_midは考慮しない、一直線型）
            // 計算する値。これがy
            double _value_A = 0.0;
            double _a = 0.0;
            // 直線y=ax+bで近似する場合、(0, _min)を通ることは確定なので、y=_value, x=_x, b=_min。よって、
            // _value = a*_x+_min。a=(_value-_min)/_x
            // このaが比例定数。これを、_valueを２点(x,y)=(0,_min)と(1.0,_max)を通る直線として、代入して、aを求める。
            _a = getHireiTeisu_UsingSaisyoZizyouHou(0, _min, 1.0, _max);
            // 値を計算
            _value_A = _a * _x + _min;

            

            // (A')min～midの直線、mid～maxの直線で、％の範囲によって２つに区切って近似する（への字直線型）
            double _value_ADash = 0.0;
            double _xMiddle = 0.5;
            double _aDash = 0.0;
            double _bDash = 0.0;
            if (_x <= _xMiddle)
            {
                // min～midの直線で近似
                // 直線y=ax+bで近似する場合、(0, _min)(0.5, _mid)を通ることは確定なので、y=_value, x=_x, b=_min。よって、
                // a=(y-b)/x = (_value-_min)/xで、aが求まる。x==0の時の扱いに注意。
                _aDash = getHireiTeisu_UsingSaisyoZizyouHou(0, _min, 0.5, _mid);
                _bDash = _min;
            }
            else
            {
                // mid～maxの直線で近似
                // 直線y=ax+bで近似する場合、(0.5, _mid)(1.0, _min)を通ることは確定なので、
                // a,bのペアが求まる。
                _aDash = getHireiTeisu_UsingSaisyoZizyouHou(0.5, _mid, 1.0, _max, out _bDash);
            }
            // 値を計算
            _value_ADash = _aDash * _x + _bDash;



            // (B)前の草案、中間値（平均値）からのブレ幅を比例増加／減少で計算する方法（正規分布でやってた頃の奴。あんまりうまくいかない）
            double _value_B = 0.0;
            double _bureValue = 0.0;
            double _average = _mid;
            if (_x > 0)
            {
                if (_average == 0) _average = double.MinValue;
                // mid～最大値の値を線形補完（比例増加）
                _bureValue = _average * ((_max - _average) / _average) * _x;
            }
            else
            {
                if (_min == 0) _min = double.MinValue;
                // 最小値～平均の値を線形補完（比例増加） // _xは負の値
                _bureValue = _average * ((_average - _min) / _min) * _x;

                // 以下は間違い。でも分散マイナスが、一気に最大値を超えてつきぬける、面白い天才的な値になっている？
                // double _x = _choicedBunsan / Math.Abs(p_SEIKIBUNSAN_MIN)
                //                 _bureValue = -1 * _average * ((_average - _min) / _average) * _x;
            }
            _value_B = _bureValue + _average;


            // ■値の設定
            double _value = 0.0;
            _value = _value_A;
            _value = _value_B;
            _value = _value_ADash;

            return _value;
        }
        /// <summary>
        /// 引数の２点(_x1,_y1),(_x2,_y2)を通る直線y=ax+bの比例定数aを返します。bはoutで返す別のメソッドがあります。
        /// </summary>
        public static double getHireiTeisu_UsingSaisyoZizyouHou(double _x1, double _y1, double _x2, double _y2)
        {
            double _b = 0.0;
            return getHireiTeisu_UsingSaisyoZizyouHou(_x1, _y1, _x2, _y2, out _b);
        }
        /// <summary>
        /// 引数の２点(_x1,_y1),(_x2,_y2)を通る直線y=ax+bの比例定数aを返します。bはoutで返します。
        /// </summary>
        /// <param name="_R">切片が代入される変数を指定してください。なお、引数の前に必ずoutを付けてください。</param>
        /// <returns></returns>
        public static double getHireiTeisu_UsingSaisyoZizyouHou(double _x1, double _y1, double _x2, double _y2, out double _b)
        {
            double[] _x = new double[] { _x1, _x2 };
            double[] _y = new double[] { _y1, _y2 };
            return getHireiTeisu_UsingSaisyoZizyouHou(_x, _y, out _b);
        }
        /// <summary>
        /// 引数のn点(_x1,_y1),(_x2,_y2)...(xn, yn)を通る直線y=ax+bの比例定数aを返します。bはoutで返します。
        /// </summary>
        /// <param name="b">切片が代入される変数を指定してください。なお、引数の前に必ずoutを付けてください。</param>
        /// <returns></returns>
        public static double getHireiTeisu_UsingSaisyoZizyouHou(double[] _x, double[] _y, out double _b)
        {
            double _a = 0.0;
            _b = 0.0; //double _R = 0.0; 引数のoutで既に宣言されている
            double x = 0.0;
            double y = 0.0;
            double xx = 0.0;
            double xy = 0.0;
            // 最小二乗法の参考ソース。感謝。　http://d.hatena.ne.jp/rainlib/20090112/1231735459
            int i = 0;
            int num = Math.Min(_x.Length, _y.Length); // どちらか要素の少ない方に合わせる
            //係数計算
            for (i = 0; i < num; i++)
            {
                //必要な値を計算
                xx += _x[i]*_x[i];    //xx += w[i, 0] * w[i, 0];
                xy += _x[i] * _y[i];  //xy += w[i, 0] * w[i, 1];
                x += _x[i];          //x += w[i, 0];
                y += _y[i];          //y += w[i, 1];

                _a = ((num * xy) - (x * y)) / ((num * xx) - (x * x));
                _b = ((xx * y) - (xy * x)) / ((num * xx) - (x * x));
            }
            // aやbが計算できない場合は、NaNを返す場合もあるので、Double.IsNaN()などでチェックしてください。
            if (Double.IsNaN(_a) == true)
            {
                // 最初と最後の点(x0,y0)と(x1,y1)を通る直線から、傾きを出すのを試してみる。
                if (num >= 2)
                {
                    if (_x[num - 1] - _x[0] != 0)
                    {
                        _a = (_y[num - 1] - _y[0]) / _x[num - 1] - _x[0];
                    }
                }
                if (Double.IsNaN(_a) == true)
                {
                    // それでもダメな場合、仕方がないので、とりあえず0.0を代入
                    _a = 0.0;
                }
            }
            if (Double.IsNaN(_b) == true)
            {
                // x[0]==0に一番近い値のYを代入してみる。
                List<double> _xList = new List<double>(_x);
                int _XZEROClosedIndex = MyTools.getIndex_MostClosed(_xList, 0.0);
                if (_y.Length - 1 >= _XZEROClosedIndex)
                {
                    _b = _y[_XZEROClosedIndex];
                }
                if (Double.IsNaN(_b) == true)
                {
                    // それでもダメな場合、仕方がないので、とりあえず0.0を代入
                    _b = 0.0;
                }
            }

            return _a;
        }


        /// <summary>
        /// 予め格納しておく、正規分布の分散テーブルです。getSeiki...で使います。
        /// </summary>
        public static double[] p_seikiBunsan;
        /// <summary>
        /// 予め格納しておく、正規分布の確率分布テーブルです。getSeiki...で使います
        /// 
        /// ■分散p_SEIKIBUNSAN_MIN～p_SEIKIBUNSAN_MAXまで。
        /// 分散の最小値（マイナスの値）～分散0～分散の最大値まで、分散p_SEIKIBUNSAN_VALUEADDING_PER_1INDEX刻みずつ、
        /// 配列として、それぞれの値を取る確率を格納しています。
        /// </summary>
        public static double[] p_seikiRate;
        /// <summary>
        /// 予め格納しておく、正規分布の確率分散テーブルの分散の最小値です。getSeiki...で使います
        /// </summary>
        public static double p_SEIKIBUNSAN_MIN = -5;
        /// <summary>
        /// 予め格納しておく、正規分布の確率分布テーブルの分散の最大値です。getSeiki...で使います
        /// </summary>
        public static double p_SEIKIBUNSAN_MAX = 5;
        /// <summary>
        /// 予め格納しておく、正規分布の確率分布テーブルの分散の配列値刻み値です。getSeiki...で使います
        /// </summary>
        public static double p_SEIKIBUNSAN_VALUEADDING_PER_1INDEX = 0.1;
        #endregion
        #region 複数のリスト項目からどれかランダムに選ぶ : getRandomValue/getRandomString
        public static T getRandomValue<T>(List<T> _list)
        {
            if (_list == null){
                return default(T);
            }
            else if (_list.Count == 0)
            {
                return default(T);
            }
            else
            {
                int _randomNum = getRandomNum(0, _list.Count - 1);
                return _list[_randomNum];
            }
        }
        public static T getRandomValue<T>(params T[] _values)
        {
            int _randomNum = getRandomNum(0, Math.Max(0, _values.Length - 1));
            return MyTools.getArrayValue(_values, _randomNum);

        }
        /// <summary>
        /// 引数の文字列リストから，ランダムでどれか一つを選んで返します．
        /// </summary>
        /// <param name="_stringList"></param>
        public static string getRandomString(params string[] _stringParameters)
        {
            int _randomNum = getRandomNum(0, Math.Max(0, _stringParameters.Length-1));
            return MyTools.getArrayValue(_stringParameters, _randomNum);
        }
        /// <summary>
        /// 引数の文字列リストから，ランダムでどれか一つを選んで返します．
        /// </summary>
        /// <param name="_stringList"></param>
        public static string getRandomString(List<string> _stringList)
        {
            if (_stringList.Count == 0)
            {
                return "";
            }
            else
            {
                int _randomNum = getRandomNum(0, _stringList.Count - 1);
                return _stringList[_randomNum];
            }
        }
        #endregion

        // 変数関連処理
        #region bool型の値　←→　int型の値1/0や，string型の値"ON"/"OFF"などに変換する（true:1、false:0）
        /// <summary>
        /// bool型の値をint型の値1/0に変換します（true:1、false:0）。
        /// </summary>
        public static int getBoolValue(bool _bool)
        {
            if (_bool == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// bool型の値をstring型の値"■"か"□"に変換します．
        /// </summary>
        public static string getBoolCheckString(bool _bool){
            return _bool == true ? "■" : "□";
        }
        /// <summary>
        /// bool型の値をstring型の値"ON"か"OFF"に変換します．
        /// </summary>
        public static string getBoolONOFF(bool _bool)
        {
            return _bool == true ? "ON" : "OFF";
        }
        /// <summary>
        /// int型の値1/0をbool型の値に変換します（1:true、0:false）。※1以外はfalseになります．
        /// </summary>
        public static bool getBool(int _boolValue)
        {
            if (_boolValue == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ※getBool、getTureOrFalse、isYesは名前が違うだけで、全て同じです。trueと判定できそうなstring型の値"ON"/"OFF","1"/"0"などをbool型の値に変換します（1:true、0:false）。基本的に、漢字には対応していません。
        /// 具体的には、"1","１","TRUE","true","True","OK","Ok","ok","YES","Yes","yes","イエス","いえす","イエッサ","いえっさ","ハイ","はい","そう","ソウ","うん","ウン","いいよ","イイヨ","わかった","ワカッタ","boolTrue_有","あり","アリ","ON","On","on"はtrue，以外はfalseになります．単語内に「!」「！」「ー」「-」「～」「、」「。」「…」「.」「・」が含まれていても、それらを削除してこれらの単語と一致していればtrueを返します。なお例外として"う～ん","うーん"だけはfalseを返します。
        /// </summary>
        public static bool isYes(string _string)
        {
            return getBool(_string);
        }
        /// <summary>
        /// ※getBool、getTureOrFalse、isYesは名前が違うだけで、全て同じです。trueと判定できそうなstring型の値"ON"/"OFF","1"/"0"などをbool型の値に変換します（1:true、0:false）。基本的に、漢字には対応していません。
        /// 具体的には、"1","１","TRUE","true","True","OK","Ok","ok","YES","Yes","yes","イエス","いえす","イエッサ","いえっさ","ハイ","はい","そう","ソウ","うん","ウン","いいよ","イイヨ","わかった","ワカッタ","boolTrue_有","あり","アリ","ON","On","on"はtrue，以外はfalseになります．単語内に「!」「！」「ー」「-」「～」「、」「。」「…」「.」「・」が含まれていても、それらを削除してこれらの単語と一致していればtrueを返します。なお例外として"う～ん","うーん"だけはfalseを返します。
        /// </summary>
        public static bool getTrueOrFalse(string _string)
        {
            return getBool(_string);
        }
        /// <summary>
        /// ※getBool、getTureOrFalse、isYesは名前が違うだけで、全て同じです。trueと判定できそうなstring型の値"1","0"などをbool型の値に変換します（1:true、0:false）。基本的に、漢字には対応していません。
        /// 具体的には、"1","１","TRUE","true","True","OK","Ok","ok","YES","Yes","yes","イエス","いえす","イエッサ","いえっさ","ハイ","はい","そう","ソウ","うん","ウン","いいよ","イイヨ","わかった","ワカッタ","boolTrue_有","あり","アリ","ON","On","on"はtrue，以外はfalseになります．単語内に「!」「！」「ー」「-」「～」「、」「。」「…」「.」「・」が含まれていても、それらを削除してこれらの単語と一致していればtrueを返します。なお例外として"う～ん","うーん"だけはfalseを返します。
        /// </summary>
        public static bool getBool(string _string)
        {
            // 「う～ん」「うーん」だけは例外としてfalse
            if (_string == "う～ん" || _string == "うーん")
            {
                return false;
            }
            // よく語尾や中間に付いていると思われる、「!」「！」「ー」「-」「～」「…」「・」「.」を取り除いてから判定する
            _string = _string.Replace("！", "");
            _string = _string.Replace("!", "");
            _string = _string.Replace("-", "");
            _string = _string.Replace("ー", "");
            _string = _string.Replace("～", "");
            _string = _string.Replace("、", "");
            _string = _string.Replace("。", "");
            _string = _string.Replace("…", "");
            _string = _string.Replace("・", "");
            _string = _string.Replace(".", "");
            // _boolValueて文字列短い単語だし、別にこんだけやっても0.000***ミリ秒だから、処理速度気にしなくていいよ
            foreach(string _isYesString in s_getBool_isYesStrings)
            {
                if(_string == _isYesString){
                    // どれか一つでも一致していればtrue
                    return true;
                }
            }
            // どれにも当てはまらなかったらfalse
            return false;
        }
        /// <summary>
        /// getBool/isYesなどで使われる、Yesと判定される文字列を集めた配列を取って来ます。
        /// 具体的には、"1","１","TRUE","true","True","OK","Ok","ok","YES","Yes","yes","イエス","いえす","イエッサ","いえっさ","ハイ","はい","そう","ソウ","うん","ウン","いいよ","イイヨ","わかった","ワカッタ","boolTrue_有","あり","アリ","ON","On","on"はtrue，以外はfalseになります．単語内に「!」「！」「ー」「-」「～」「、」「。」「…」「.」「・」が含まれていても、それらを削除してこれらの単語と一致していればtrueを返します。なお例外として"う～ん","うーん"だけはfalseを返します。
        /// </summary>
        public static string[] getIsYesStrings() { return s_getBool_isYesStrings; }

        /// <summary>
        /// getBool/isYesなどで使われる、Yesと判定される文字列を集めた配列です。
        /// </summary>
        private static string[] s_getBool_isYesStrings = new string[] { "1", "１", "TRUE", "true", "True", "OK", "Ok", "ok", "YES", "Yes", "yes", "イエス", "いえす", "イエッサ", "いえっさ", "ハイ", "はい", "そう", "ソウ", "うん", "ウン", "いいよ", "イイヨ", "わかった", "ワカッタ", "boolTrue_有", "あり", "アリ", "ON", "On", "on" };
        #endregion

        // 配列関連処理
        #region 一番大きな／小さな／中央の値を取ってくる: getMaxValue/MinValue/MiddleValue
        // ■■以下、配列版

        /// <summary>
        /// getBiggestValueと一緒です。任意のリストの値の最大値を取ってきます．リストがnullの場合はint.MaxValueを返します．
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static int getMaxValue(int[] _values)
        {
            return getBiggestValue(_values);
        }
        /// <summary>
        /// 任意のリストの値の最大値を取ってきます．リストがnullの場合はint.MaxValueを返します．
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static int getBiggestValue(int[] _values)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _maxValue = int.MaxValue;
            if (_values != null && _values.Length > 0)
            {
                _maxValue = _values[0];
                foreach (int _value in _values)
                {
                    if (_value > _maxValue)
                    {
                        _maxValue = _value;
                    }
                }
            }
            return _maxValue;
        }
        /// <summary>
        /// getSmallestValueと一緒です。任意のリストの値の最小値を取ってきます．リストがnullの場合はint.MinValueを返します．
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static int getMinValue(int[] _values)
        {
            return getSmallestValue(_values);
        }
        /// <summary>
        /// getMinValueと一緒です。任意のリストの値の最小値を取ってきます．リストがnullの場合はint.MinValueを返します．
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static int getSmallestValue(int[] _values)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _smallestValue = int.MinValue;
            if (_values != null && _values.Length > 0)
            {
                _smallestValue = _values[0];
                foreach (int _value in _values)
                {
                    if (_value < _smallestValue)
                    {
                        _smallestValue = _value;
                    }
                }
            }
            return _smallestValue;
        }
        /// <summary>
        /// 任意のリストの値の中間値（平均ではなく，ソートした時の中間）を取ってきます．リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static int getMiddleValue(int[] _values)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _middleValue = 0;
            if (_values != null && _values.Length > 0)
            {
                _middleValue = _values[0];
                List<int> _sortedLists = MyTools.getSortedList(new List<int>(_values), ESortType.値が小さい順＿昇順); // 小さい順に並べる
                _middleValue = _values[(int)(_sortedLists.Count / 2) - 1]; //  （配列数が4つの場合は2つめ[1]，7つの場合は3つめ[2]を取る（奇数の場合は前の方を取る）
                _sortedLists.Clear(); // メモリ節約
            }
            return _middleValue;
        }

        // Double版

        /// <summary>
        /// getBiggestValueと一緒です。任意のリストの値の最大値を取ってきます．リストがnullの場合はint.MaxValueを返します．
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static double getMaxValue(double[] _values)
        {
            return getBiggestValue(_values);
        }
        /// <summary>
        /// 任意のリストの値の最大値を取ってきます．リストがnullの場合はdouble.MaxValueを返します．
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static double getBiggestValue(double[] _values)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            double _maxValue = double.MaxValue;
            if (_values != null && _values.Length > 0)
            {
                _maxValue = _values[0];
                foreach (double _value in _values)
                {
                    if (_value > _maxValue)
                    {
                        _maxValue = _value;
                    }
                }
            }
            return _maxValue;
        }
        /// <summary>
        /// getSmallestValueと一緒です。任意のリストの値の最小値を取ってきます．リストがnullの場合はint.MinValueを返します．
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static double getMinValue(double[] _values)
        {
            return getSmallestValue(_values);
        }
        /// <summary>
        /// 任意のリストの値の最小値を取ってきます．リストがnullの場合はdouble.MinValueを返します．
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static double getSmallestValue(double[] _values)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            double _smallestValue = double.MinValue;
            if (_values != null && _values.Length > 0)
            {
                _smallestValue = _values[0];
                foreach (double _value in _values)
                {
                    if (_value < _smallestValue)
                    {
                        _smallestValue = _value;
                    }
                }
            }
            return _smallestValue;
        }
        /// <summary>
        /// 任意のリストの値の中間値（平均ではなく，ソートした時の中間）を取ってきます．リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_values"></param>
        /// <returns></returns>
        public static double getMiddleValue(double[] _values)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            double _middleValue = 0;
            if (_values != null && _values.Length > 0)
            {
                _middleValue = _values[0];
                List<double> _sortedLists = MyTools.getSortedList(new List<double>(_values), ESortType.値が小さい順＿昇順); // 小さい順に並べる
                _middleValue = _values[(int)(_sortedLists.Count / 2) - 1]; //  （配列数が4つの場合は2つめ[1]，7つの場合は3つめ[2]を取る（奇数の場合は前の方を取る）
                _sortedLists.Clear(); // メモリ節約
            }
            return _middleValue;
        }


        // ■■以下、リスト版

        /// <summary>
        /// getBiggestValueと一緒です。任意のリストの値の最大値を取ってきます．リストがnullの場合はint.MaxValueを返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getMaxValue(List<int> _list)
        {
            return getBiggestValue(_list);
        }
        /// <summary>
        /// 任意のリストの値の最大値を取ってきます．リストがnullの場合はint.MaxValueを返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static  int getBiggestValue(List<int> _list)
            // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _maxValue = int.MaxValue;
            if (_list != null && _list.Count > 0)
            {
                _maxValue = _list[0];
                foreach (int _value in _list)
                {
                    if (_value > _maxValue)
                    {
                        _maxValue = _value;
                    }
                }
            }
            return _maxValue;
        }
        /// <summary>
        /// getSmallestValueと一緒です。任意のリストの値の最小値を取ってきます．リストがnullの場合はint.MinValueを返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static  int getMinValue(List<int> _list){
            return getSmallestValue(_list);
        }
        /// <summary>
        /// getMinValueと一緒です。任意のリストの値の最小値を取ってきます．リストがnullの場合はint.MinValueを返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static  int getSmallestValue(List<int> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _smallestValue = int.MinValue;
            if (_list != null && _list.Count > 0)
            {
                _smallestValue = _list[0];
                foreach (int _value in _list)
                {
                    if (_value < _smallestValue)
                    {
                        _smallestValue = _value;
                    }
                }
            }
            return _smallestValue;
        }
        /// <summary>
        /// 任意のリストの値の中間値（平均ではなく，ソートした時の中間）を取ってきます．リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getMiddleValue(List<int> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _middleValue = 0;
            if (_list != null && _list.Count > 0)
            {
                _middleValue = _list[0];
                List<int> _sortedLists = MyTools.getSortedList(_list, ESortType.値が小さい順＿昇順); // 小さい順に並べる
                _middleValue = _list[(int)(_sortedLists.Count / 2) - 1]; //  （配列数が4つの場合は2つめ[1]，7つの場合は3つめ[2]を取る（奇数の場合は前の方を取る）
                _sortedLists.Clear(); // メモリ節約
            }
            return _middleValue;
        }
        
        // Double版

        /// <summary>
        /// getBiggestValueと一緒です。任意のリストの値の最大値を取ってきます．リストがnullの場合はint.MaxValueを返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static double getMaxValue(List<double> _list)
        {
            return getBiggestValue(_list);
        }
        /// <summary>
        /// 任意のリストの値の最大値を取ってきます．リストがnullの場合はdouble.MaxValueを返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static double getBiggestValue(List<double> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            double _maxValue = double.MaxValue;
            if (_list != null && _list.Count > 0)
            {
                _maxValue = _list[0];
                foreach (double _value in _list)
                {
                    if (_value > _maxValue)
                    {
                        _maxValue = _value;
                    }
                }
            }
            return _maxValue;
        }
        /// <summary>
        /// getSmallestValueと一緒です。任意のリストの値の最小値を取ってきます．リストがnullの場合はint.MinValueを返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static double getMinValue(List<double> _list)
        {
            return getSmallestValue(_list);
        }
        /// <summary>
        /// 任意のリストの値の最小値を取ってきます．リストがnullの場合はdouble.MinValueを返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static double getSmallestValue(List<double> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            double _smallestValue = double.MinValue;
            if (_list != null && _list.Count > 0)
            {
                _smallestValue = _list[0];
                foreach (double _value in _list)
                {
                    if (_value < _smallestValue)
                    {
                        _smallestValue = _value;
                    }
                }
            }
            return _smallestValue;
        }
        /// <summary>
        /// 任意のリストの値の中間値（平均ではなく，ソートした時の中間）を取ってきます．リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static double getMiddleValue(List<double> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            double _middleValue = 0;
            if (_list != null && _list.Count > 0)
            {
                _middleValue = _list[0];
                List<double> _sortedLists = MyTools.getSortedList(_list, ESortType.値が小さい順＿昇順); // 小さい順に並べる
                _middleValue = _list[(int)(_sortedLists.Count / 2) - 1]; //  （配列数が4つの場合は2つめ[1]，7つの場合は3つめ[2]を取る（奇数の場合は前の方を取る）
                _sortedLists.Clear(); // メモリ節約
            }
            return _middleValue;
        }
        #endregion
        #region 一番大きな／小さな／中央の配列インデックスを取ってくる: getIndex_***
        // ■今はリスト型だけ実装。欲しかったら配列型もコピペして実装してもいいかも。

        /// <summary>
        /// 任意のリストの値の最大値の配列インデックスを返します．リストがnullの場合は0を返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_Biggest(List<int> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _maxIndex = 0;
            int _maxValue = int.MaxValue;
            if (_list != null && _list.Count > 0)
            {
                _maxValue = _list[0];
                int i = 0;
                foreach (int _value in _list)
                {
                    if (_value > _maxValue)
                    {
                        _maxValue = _value;
                        _maxIndex = i;
                    }
                    i++;
                }
            }
            return _maxIndex;
        }
        /// <summary>
        /// 任意のリストの値の最小値の配列インデックスを返します．リストがnullの場合は0を返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_Smallest(List<int> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _smallestIndex = 0;
            int _smallestValue = int.MinValue;
            if (_list != null && _list.Count > 0)
            {
                _smallestValue = _list[0];
                int i = 0;
                foreach (int _value in _list)
                {
                    if (_value < _smallestValue)
                    {
                        _smallestValue = _value;
                        _smallestIndex = i;
                    }
                    i++;
                }
            }
            return _smallestIndex;
        }
        /// <summary>
        /// 任意のリストの値の中間値（平均ではなく，ソートした時の中間）の配列インデックスを返します．リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_Middle(List<int> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _middleIndex = 0;
            int _middleValue = 0;
            if (_list != null && _list.Count > 0)
            {
                _middleValue = _list[0];
                List<int> _sortedLists = MyTools.getSortedList(_list, ESortType.値が小さい順＿昇順); // 小さい順に並べる
                _middleIndex = (int)(_list.Count / 2) - 1;
                _middleValue = _sortedLists[_middleIndex]; //  （配列数が4つの場合は2つめ[1]，7つの場合は3つめ[2]を取る（奇数の場合は前の方を取る）
            }
            // コピーのindexではなく、元のindexを取得
            _middleIndex = _list.IndexOf(_middleValue);
            return _middleIndex;
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値にN番目に近い配列インデックスを返します．二つ以上ある場合は小さい方の配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_NstClosed(List<int> _list, int _searchingValue, int _N_closedRankingNo)
        {
            return getIndex_NstClosed(_list, _searchingValue, _N_closedRankingNo, 0, _list.Count);
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値にN番目に近い配列インデックスを返します．開始配列を指定することができます。二つ以上ある場合は小さい方の配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_NstClosed(List<int> _list, int _searchingValue, int _N_closedRankingNo, int _startIndex)
        {
            return getIndex_NstClosed(_list, _searchingValue, _N_closedRankingNo, _startIndex, _list.Count - _startIndex);
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値にN番目に近い配列インデックスを返します．検索範囲（開始配列とそこからの検索配列数）を指定することができます。検索配列数は大きすぎる値を入れても問題ありません（自動的に最後の配列で止まります）。二つ以上ある場合は小さい方の配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_NstClosed(List<int> _list, int _searchingValue, int _N_closedRankingNo, int _startIndex, int _searchingIndexCount)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            _startIndex = Math.Max(0, _startIndex);
            int _endIndex = Math.Min(_list.Count - 1, _startIndex + _searchingIndexCount - 1); if (_endIndex < 0) _endIndex = 0;

            List<int> _closedDisance_Ranking1toN = new List<int>();

            int _index = 0;
            int _distance = 0;
            int _NstClosedDistance = Int32.MaxValue; ;
            if (_list != null && _list.Count > 0)
            {
                // ソートされたリストを使って、値の差が最小からN番目の配列を見つける。
                for (int i = _startIndex; i < _endIndex; i++)
                {
                    _distance = Math.Abs(_list[i] - _searchingValue);
                    // 差ランキングリストに空きがあったら、とりあえず入れる
                    if (_closedDisance_Ranking1toN.Count <= _N_closedRankingNo)
                    {
                        // 差ランキングリストに追加
                        _closedDisance_Ranking1toN.Add(_distance);
                    }
                    else
                    {// if (_closedDisance_Ranking1toN.Count > _N_closedRankingNo)
                        // 差ランキングリストがいっぱいだったら、現時点でのNst（N番目に差が小さい値）より小さければ、入れる
                        if (_distance < _NstClosedDistance)
                        {
                            // 差ランキングリストの要素数がNより大きいから、最初の（差が一番大きい）要素を削除
                            _closedDisance_Ranking1toN.RemoveAt(0);

                            // 差ランキングリストに追加して、降順ソート
                            _closedDisance_Ranking1toN.Add(_distance);
                            MyTools.sortList(_closedDisance_Ranking1toN, ESortType.値が大きい順＿降順);
                            // 最初の（一番差が大きい）値をNstとする
                            _NstClosedDistance = _closedDisance_Ranking1toN[0];
                            _index = i;
                        }
                    }
                }
            }
            return _index;
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値に最も近い配列インデックスを返します．検索する開始配列を指定することができます。二つ以上ある場合は最も小さい配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_MostClosed(List<int> _list, int _searchingValue, int _startIndex)
        {
            return getIndex_MostClosed(_list, _searchingValue, _startIndex, _list.Count - _startIndex);
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値に最も近い配列インデックスを返します．検索範囲（開始配列とそこからの検索配列数）を指定することができます。検索配列数は大きすぎる値を入れても問題ありません（自動的に最後の配列で止まります）。二つ以上ある場合は最も小さい配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_MostClosed(List<int> _list, int _searchingValue, int _startIndex, int _searchingIndexCount)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            _startIndex = Math.Max(0, _startIndex);
            int _endIndex = Math.Min(_list.Count - 1, _startIndex + _searchingIndexCount - 1); if (_endIndex < 0) _endIndex = 0;

            int _index = 0;
            int _distance = 0;
            int _mostClosedDistance = Int32.MaxValue;
            if (_list != null && _list.Count > 0)
            {
                //List<int> _sortedLists = MyTools.getSortedList(_list, ESortType.値が小さい順＿昇順); // 小さい順に並べる
                // 値の差が最小の配列を見つける
                // 値の差の最小のものを探すだけだから、ソート済み配列を使って配列を飛ばせばもっと早く検索できる。が、ソート済みの配列の作成にも時間もメモリもかかるので、どちらが早いかは分からない。今はそのまま１つずつずらしてる
                for (int i = _startIndex; i <= _endIndex; i++)
                {
                    _distance = Math.Abs(_list[i] - _searchingValue);
                    if (_distance < _mostClosedDistance)
                    {
                        _mostClosedDistance = _distance;
                        _index = i;
                    }
                }
            }
            return _index;

        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値に最も近い配列インデックスを返します．二つ以上ある場合は最も小さい配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_MostClosed(List<int> _list, int _searchingValue)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            return getIndex_MostClosed(_list, _searchingValue, 0);
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値に最も近い値を返します．リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        public static int getMostClosedValue(List<int> _list, int _searchingValue)
        {
            int _index = getIndex_MostClosed(_list, _searchingValue);
            if (_index == 0) return 0;
            return _list[_index];
        }


        // Double版

        /// <summary>
        /// 任意のリストの値の最大値の配列インデックスを返します．リストがnullの場合は0を返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_Biggest(List<double> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _maxIndex = 0;
            double _maxValue = double.MaxValue;
            if (_list != null && _list.Count > 0)
            {
                _maxValue = _list[0];
                int i = 0;
                foreach (double _value in _list)
                {
                    if (_value > _maxValue)
                    {
                        _maxValue = _value;
                        _maxIndex = i;
                    }
                    i++;
                }
            }
            return _maxIndex;
        }
        /// <summary>
        /// 任意のリストの値の最小値の配列インデックスを返します．リストがnullの場合は0を返します．
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_Smallest(List<double> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _smallestIndex = 0;
            double _smallestValue = double.MinValue;
            if (_list != null && _list.Count > 0)
            {
                _smallestValue = _list[0];
                int i = 0;
                foreach (double _value in _list)
                {
                    if (_value < _smallestValue)
                    {
                        _smallestValue = _value;
                        _smallestIndex = i;
                    }
                    i++;
                }
            }
            return _smallestIndex;
        }
        /// <summary>
        /// 任意のリストの値の中間値（平均ではなく，ソートした時の中間）の配列インデックスを返します．リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_Middle(List<double> _list)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            int _middleIndex = 0;
            double _middleValue = 0;
            if (_list != null && _list.Count > 0)
            {
                _middleValue = _list[0];
                List<double> _sortedLists = MyTools.getSortedList(_list, ESortType.値が小さい順＿昇順); // 小さい順に並べる
                _middleIndex = (int)(_list.Count / 2) - 1;
                _middleValue = _sortedLists[_middleIndex]; //  （配列数が4つの場合は2つめ[1]，7つの場合は3つめ[2]を取る（奇数の場合は前の方を取る）
            }
            // コピーのindexではなく、元のindexを取得
            _middleIndex = _list.IndexOf(_middleValue);
            return _middleIndex;
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値にN番目に近い配列インデックスを返します．二つ以上ある場合は小さい方の配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_NstClosed(List<double> _list, int _searchingValue, int _N_closedRankingNo)
        {
            return getIndex_NstClosed(_list, _searchingValue, _N_closedRankingNo, 0, _list.Count);
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値にN番目に近い配列インデックスを返します．開始配列を指定することができます。二つ以上ある場合は小さい方の配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_NstClosed(List<double> _list, int _searchingValue, int _N_closedRankingNo, int _startIndex)
        {
            return getIndex_NstClosed(_list, _searchingValue, _N_closedRankingNo, _startIndex, _list.Count - _startIndex);
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値にN番目に近い配列インデックスを返します．検索範囲（開始配列とそこからの検索配列数）を指定することができます。検索配列数は大きすぎる値を入れても問題ありません（自動的に最後の配列で止まります）。二つ以上ある場合は小さい方の配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_NstClosed(List<double> _list, int _searchingValue, int _N_closedRankingNo, int _startIndex, int _searchingIndexCount)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            _startIndex = Math.Max(0, _startIndex);
            int _endIndex = Math.Min(_list.Count - 1, _startIndex + _searchingIndexCount - 1); if (_endIndex < 0) _endIndex = 0;

            List<double> _closedDisance_Ranking1toN = new List<double>();

            int _index = 0;
            double _distance = 0;
            double _NstClosedDistance = double.MaxValue; ;
            if (_list != null && _list.Count > 0)
            {
                // ソートされたリストを使って、値の差が最小からN番目の配列を見つける。
                for (int i = _startIndex; i < _endIndex; i++)
                {
                    _distance = Math.Abs(_list[i] - _searchingValue);
                    // 差ランキングリストに空きがあったら、とりあえず入れる
                    if (_closedDisance_Ranking1toN.Count <= _N_closedRankingNo)
                    {
                        // 差ランキングリストに追加
                        _closedDisance_Ranking1toN.Add(_distance);
                    }
                    else
                    {// if (_closedDisance_Ranking1toN.Count > _N_closedRankingNo)
                        // 差ランキングリストがいっぱいだったら、現時点でのNst（N番目に差が小さい値）より小さければ、入れる
                        if (_distance < _NstClosedDistance)
                        {
                            // 差ランキングリストの要素数がNより大きいから、最初の（差が一番大きい）要素を削除
                            _closedDisance_Ranking1toN.RemoveAt(0);

                            // 差ランキングリストに追加して、降順ソート
                            _closedDisance_Ranking1toN.Add(_distance);
                            MyTools.sortList(_closedDisance_Ranking1toN, ESortType.値が大きい順＿降順);
                            // 最初の（一番差が大きい）値をNstとする
                            _NstClosedDistance = _closedDisance_Ranking1toN[0];
                            _index = i;
                        }
                    }
                }
            }
            return _index;
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値に最も近い配列インデックスを返します．二つ以上ある場合は最も小さい配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_MostClosed(List<double> _list, double _searchingValue, int _startIndex)
        {
            return getIndex_MostClosed(_list, _searchingValue, _startIndex, _list.Count - _startIndex);
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値に最も近い配列インデックスを返します．二つ以上ある場合は最も小さい配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_MostClosed(List<double> _list, double _searchingValue, int _startIndex, int _searchingIndexCount)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            _startIndex = Math.Max(0, _startIndex);
            int _endIndex = Math.Min(_list.Count - 1, _startIndex + _searchingIndexCount - 1);

            int _index = 0;
            double _distance = 0;
            double _mostClosedDistance = double.MaxValue; ;
            if (_list != null && _list.Count > 0)
            {
                //List<int> _sortedLists = MyTools.getSortedList(_list, ESortType.値が小さい順＿昇順); // 小さい順に並べる
                // 値の差が最小の配列を見つける
                // 値の差の最小のものを探すだけだから、ソート済み配列を使って配列を飛ばせばもっと早く検索できる。が、ソート済みの配列の作成にも時間もメモリもかかるので、どちらが早いかは分からない。今はそのまま１つずつずらしてる
                for (int i = 0; i < _endIndex; i++)
                {
                    _distance = Math.Abs(_list[i] - _searchingValue);
                    if (_distance < _mostClosedDistance)
                    {
                        _mostClosedDistance = _distance;
                        _index = i;
                    }
                }
            }
            return _index;

        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値に最も近い配列インデックスを返します．二つ以上ある場合は最も小さい配列を返します。リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static int getIndex_MostClosed(List<double> _list, double _searchingValue)
        // [ToDo]任意の値型で書ける？ where T : struct // Tは値型
        {
            return getIndex_MostClosed(_list, _searchingValue, 0);
        }
        /// <summary>
        /// 任意のリストの値の中で、指定した値に最も近い値を返します．リストがnullの場合は0を返します．リストの中身は変更されません。
        /// </summary>
        public static double getMostClosedValue(List<double> _list, double _searchingValue)
        {
            int _index = getIndex_MostClosed(_list, _searchingValue);
            if (_index == 0) return 0.0;
            return _list[_index];
        }
        #endregion

        #region 任意の型のリストの値をインデックス不正値((リスト数-1)以上やマイナス)をエラーにせずに返す: getListValue
        /// <summary>
        /// 任意の型のリストの値をエラーなしに返します．
        /// なお，リスト[インデックス]のデータを調べ，インデックスが不正値((リスト数-1)以上やマイナス)だった場合は，
        /// その型のデフォルト値（値型なら0，※【注意】string型ならnull，クラス型ならnull）を返します．
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_datalist"></param>
        /// <param name="_index"></param>
        /// <returns></returns>
        public static T getListValue<T>(List<T> _datalist, int _index)
        {
            T _value = default(T); // これだとstring型でもnullが入るみたい // 参考。default(T)の値一覧。感謝。 http://d.hatena.ne.jp/gsf_zero1/20110221/p1
            if (_index >= 0 && _index <= _datalist.Count - 1)
            {
                _value = _datalist[_index];
            }
            return _value;
        }
        /// <summary>
        /// 任意の型の配列の値をエラーなしに返します．なお，配列[インデックス]のデータを調べ，インデックスが不正値((リスト数-1)以上やマイナス)だった場合は，その型のデフォルト値（値型なら0や""，クラス型ならnull）を返します．
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_datalist"></param>
        /// <param name="_index"></param>
        /// <returns></returns>
        public static T getArrayValue<T>(T[] _dataArray, int _index)
        {
            T _value = default(T);
            if (_index >= 0 && _index <= _dataArray.Length - 1)
            {
                _value = _dataArray[_index];
            }
            return _value;
        }
        #endregion
        #region 任意の型のリストの値を行("\n")で区切った文字列を返す: getListValues_ToLines
        /// <summary>
        /// 任意の型のリストの値を行("\n")で区切った文字列を返します．第３引数で配列内の中身がない要素（0や""やnull）を含めるかを指定してください。なお，リスト[インデックス]のデータを調べ，インデックスが不正値((リスト数-1)以上やマイナス)だった場合は，その型のデフォルト値（値型なら0や""，クラス型ならnull）を返します．
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string getListValues_ToLines<T>(string _titleLabel_OrNull, List<T> _var, bool _isDefaultValue_Included)
        {
            string _dividedWord_Between_values = "\n"; // 区切り文字
            string _title = "";
            if (_titleLabel_OrNull != null)
            {
                _title = _titleLabel_OrNull + ",";
            }
            return _title + getListValues_ToString_DividedAnyWord<T>(_var, _isDefaultValue_Included, _dividedWord_Between_values);
        }
        #endregion
        #region 任意の型のリストの値を任意の区切り文字で区切った文字列を返す: getListValues_ToString_DividedAnyWord
        /// <summary>
        /// 任意の型のリストの値を任意の区切り文字(dividedWord)で区切った文字列を返します．第２引数で配列内の中身がない要素（0や""やnull）を含めるかを指定してください。なお，リスト[インデックス]のデータを調べ，インデックスが不正値((リスト数-1)以上やマイナス)だった場合は，その型のデフォルト値（値型なら0や""，クラス型ならnull）を返します．
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string getListValues_ToString_DividedAnyWord<T>(List<T> _var, bool _isDefaultValue_Included, string _dividedWord_Between_values)
        {
            string CSVdata = "";
            string _valueString = "";
            // デフォルトの値を格納
            T _default = default(T);
            string _defaultValue = "";
            if (_default != null)
            {
                _defaultValue = _default.ToString();
            }
            int i = 0;

            foreach (T _value in _var)
            {
                if (_value != null) // 念のため
                {
                    _valueString = _value.ToString(); // 値型だと値を示した文字列，２重配列（List）やクラス型だとクラス名が入る
                    // 
                    if (_isDefaultValue_Included == true || (_isDefaultValue_Included == false && _valueString != _defaultValue))
                    {
                        CSVdata += _valueString;
                        if (i != _var.Count - 1)
                        {
                            CSVdata += _dividedWord_Between_values;//","; //System.Environment.NewLine;
                        }
                    }
                }
                i++;
            }
            return CSVdata;
        }
        #endregion
        #region 任意の方のリストの、ある範囲（指定した最小値～最大値）の値の要素数を調べる: getValueCount
        /// <summary>
        /// 任意の型のリストの指定した値が含まれる要素数を返します．
        /// </summary>
        public static int getValueCount_InList(List<double> _list, double _min, double _max)
        {
            int _count = 0;
            foreach (double _value in _list)
            {
                if (_value >= _min && _value <= _max)
                {
                    _count++;
                }
            }
            return _count;
        }
        /// <summary>
        /// 任意の型のリストの指定した値が含まれる要素数を返します．
        /// </summary>
        public static int getValueCount_InList(List<int> _list, int _min, int _max)
        {
            int _count = 0;
            foreach (int _value in _list)
            {
                if (_value >= _min && _value <= _max)
                {
                    _count++;
                }
            }
            return _count;
        }
        #endregion
        #region 任意の型のリストのある値の要素数を調べる: getSameValueCount
        /// <summary>
        /// 任意の型のリストの指定した値が含まれる要素数を返します．
        /// </summary>
        public static int getSameValueCount_InList<T>(List<T> _list, T _checkedValue)
            where T : struct // Tは値型
        {
            //[Tips][T][ジェネリック]ジェネリックメソッドの作り方 // checkValueCount_InList<T>(List<T> _list, T _checkedInstance) where T : ***// Tの文字は任意，***はTの条件（値型：struct，参照型：class）
            // http://ufcpp.net/study/csharp/sp2_generics.html#method
            int _count = 0;
            foreach (T _value in _list)
            {
                if (_value.Equals(_checkedValue) == true)
                {
                    _count++;
                }
            }
            return _count;
        }
        /// <summary>
        /// 任意の型のリストの指定したインスタンスと同じインスタンス（_list[0]==_checkedInstanceとなる）が含まれる要素数を返します．
        /// </summary>
        public static int getSameClassInstanceCount_InList<T>(List<T> _list, T _checkedInstance)
            where T : class // Tはクラス型
        {
            int _count = 0;
            foreach (T _value in _list)
            {
                if (_value == _checkedInstance)
                {
                    _count++;
                }
            }
            return _count;
        }
        #endregion
        #region String型のリストで、先頭がある文字列で始まるインデックスを返す: getListIndex
        /// <summary>
        /// String型のリストで、先頭がある文字列で始まるインデックスを返します
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_SearchFirstString"></param>
        /// <returns></returns>
        public static int getListLine(List<string> _list, string _SearchFirstString)
        {
            for (int i = 0; i < _list.Count; i++ )
            {
                if (_list[i].Length >= _SearchFirstString.Length)
                {
                    if (_list[i].Substring(0, _SearchFirstString.Length) == _SearchFirstString)
                    {
                        return i;
                    }
                }
            }
            return -1; // 見つからなかったら-1
        }
        #endregion

        // リストの値型／文字列型の変数処理
        #region リストをソートする: sortList/getSortedList
        /// <summary>
        /// リストをソートするタイプ（値が小さい順＿昇順，値が大きい順＿降順，あいうえお順など）を選択するときに使う列挙体です．
        /// 
        /// 
        /// ※英語版と日本語版、どちらも使っていただけます。
        /// なお、これの列挙体を参照して機能を実装する場合は、goto case ESortType.*** を使って、どちらも実装してください。
        /// 
        /// </summary>
        public enum ESortType
        {
            /// <summary>
            /// 無し、と同じ意味です。英語版と日本語版、どちらも使っていただけます。
            /// 実装する場合は、goto caseどちらも実装してください。
            /// </summary>
            _default_none, // 無し
            ascendingOrder_syouZyun,//値が小さい順＿昇順,
            descendingOrder_kouZyun,// 値が大きい順＿降順,
            abcdOrder_aiueoZyun,//あいうえお順,
            lengthBiggerOrder_mozisuuOokiiZyun,//文字数が大きい順,
            lengthSmallerOrder_mozisuuTiisaiZyun,//文字数が小さい順,
            random,//ランダム

            // 日本語Version
            値が小さい順＿昇順,
            値が大きい順＿降順,
            あいうえお順,
            文字数が大きい順,
            文字数が小さい順,
            ランダム,
            無し,
        }
        /// <summary>
        /// 引数のlist1を昇順・降順・文字列順などにソートします．引数のlist1自体の中身がソートされることに気をつけてください．未ソートのリストを残したい場合は，getSortedListを使ってください．
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_values1"></param>
        /// <param name="_sortType"></param>
        /// <returns></returns>
        public static void sortList<T>(List<T> _list1, ESortType _sortType)
        {
            List<T> _list = _list1;
            List<string> _listString;
            // リストをソート
            switch (_sortType)
            {
                case ESortType.無し:
                    break;
                case ESortType.値が小さい順＿昇順:
                    _list.Sort(); //小さい順にソート
                    break;
                case ESortType.値が大きい順＿降順:
                    _list.Sort(); //小さい順にソート
                    _list.Reverse(); // 反転して，降順に
                    break;
                case ESortType.あいうえお順:
                    _list.Sort(); // 文字コード順なので昇順と一緒
                    break;
                case ESortType.文字数が小さい順:
                    _listString = _list as List<string>;
                    if (_listString != null)
                    {
                        // リストのデリゲートを使ったソートの仕方 http://dobon.net/vb/dotnet/programing/icomparer.html
                        _listString.Sort(delegate(string x, string y) { return x.Length.CompareTo(y.Length); }); // .NetFramework3.0だとこうも書ける (x, y) => x.Length - y.Length);
                    }
                    break;
                case ESortType.文字数が大きい順:
                    _listString = _list as List<string>;
                    if (_listString != null)
                    {
                        _listString.Sort(delegate(string x, string y) { return y.Length.CompareTo(x.Length); });
                    }
                    break;
                case ESortType.ランダム:
                    int _listNum = _list.Count; // リストを触るとcountが変更されるので前もって持っておく！
                    List<T> _listTemp = new List<T>(_listNum);
                    for (int i = 0; i <= _listNum - 1; i++)
                    {
                        int _randomNum = MyTools.getRandomNum(0, _list.Count - 1);
                        _listTemp.Add(_list[_randomNum]);
                        _list.RemoveAt(_randomNum);
                    }
                    // _listTempの中身を_listとする (_list = _listTempでも，_list = new List<T>(_listTemp) でも，このメソッドがおわると（returnにListを返すメソッドじゃないから）_listの内容は全ては消えるので注意！)
                    _list.AddRange(_listTemp);
                    break;

                // 英語Version
                // [Warning]switch文で同じcaseを実行することをわかりやすく示すために，switch文にだけ，goto文を使っています．goto文嫌いの方，ごめんなさい・・・．
                case ESortType._default_none:
                    goto case ESortType.無し;
                case ESortType.ascendingOrder_syouZyun:
                    goto case ESortType.値が小さい順＿昇順;
                case ESortType.descendingOrder_kouZyun:
                    goto case ESortType.値が大きい順＿降順;
                case ESortType.abcdOrder_aiueoZyun:
                    goto case ESortType.あいうえお順;
                case ESortType.lengthSmallerOrder_mozisuuTiisaiZyun:
                    goto case ESortType.文字数が小さい順;
                case ESortType.lengthBiggerOrder_mozisuuOokiiZyun:
                    goto case ESortType.文字数が大きい順;
                case ESortType.random:
                    goto case ESortType.ランダム;

                default:
                    break;
            }
        }
        /// <summary>
        /// 引数のlist1を昇順・降順・文字列順などにソートしたリストを，新規作成して返します．引数のlist1自体は変更されません．
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_values1"></param>
        /// <param name="_sortType"></param>
        /// <returns></returns>
        public static List<T> getSortedList<T>(List<T> _list1, ESortType _sortType)
        {
            List<T> _sortedList = new List<T>(_list1);
            MyTools.sortList(_sortedList, _sortType);
            return _sortedList;
        }
        #endregion
        #region リストのコピーを作成する： getCopyedList
        /// <summary>
        /// リストの中身をそのままコピーしたコピーリストを作成し、返します。
        /// （メモリ容量を新たに確保して作成するため、コピーリストを変更しても、元のリストの中身は変更されません。）
        /// </summary>
        public static List<T> getCopyedList<T>(List<T> _list1)
        {
            List<T> _newList = new List<T>(_list1); // みんな知ってるかもしれないが、よく忘れるので、一応作った。
            return _newList;
        }
        #endregion
        #region リストの重複値を削除する： getUniqued/removeSameValue_InList
        /// <summary>
        /// リストの重複値を削除します。つまり、中身が同じ値は1つだけ存在するようにします。
        /// ※リストの中身が変更されるので注意してください。
        /// </summary>
        public static List<T> getuniquedValueOnly1List<T>(List<T> _list1)
        {
            List<T> _copyedList = null;
            // 重複値の排除
            if (_list1.Count > 0)
            {
                _copyedList = new List<T>(_list1);
                foreach (T _value in _copyedList)
                {
                    if (_list1.Contains(_value) == false)
                    {
                        _list1.Add(_value);
                    }
                }
            }
            else
            {
                _copyedList = new List<T>(); // 空のリスト
            }
            // 最後にコピーの方にする
            return _copyedList;
        }

        /// <summary>
        /// リストの重複値を削除します。つまり、中身が同じ値は1つだけ存在するようにします。
        /// ※リストの中身が変更されるので注意してください。
        /// </summary>
        public static void removeSameValue_InList<T>(List<T> _list1)
        {
            // 重複値の排除
            if (_list1.Count > 0) {
                for (int i = 0; i < _list1.Count; i++)
                {
                    T _checkingValue = _list1[i];
                    for (int j = 0; j < _list1.Count; j++ )
                    {
                        T _value = _list1[j];
                        if (i != j){ // 自分自身以外をチェック
                            // 中身が一緒かどうか
                            if (_value.Equals(_checkingValue)) // 値型でもこれで==になっているかを確かめる→double型で確認済み。string型も確認済み。
                            {
                                _list1.RemoveAt(j);
                                j--; // 一個消されたから、1つ配列を戻す
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region リストの順番をランダムにして格納するaddListA_FromListB_IndexRandom
        /// <summary>
        ///  _listBの中身を_listAに，順番をランダムにコピーします．
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_listA"></param>
        /// <param name="_listB"></param>
        /// <param name="p_random・乱数"></param>
        public static void addListA_FromListB_IndexRandom<T>(List<T> _listA, List<T> _listB, Random _random)
        {
            int _randomNum;
            List<T> _isAdded = new List<T>(_listB);
            // _listBの中身を順番をランダムに_listAにコピー
            for (int i = 0; i < _listB.Count; i++)
            {
                _randomNum = _random.Next(0, _isAdded.Count - 1);
                _listA.Add(_isAdded[_randomNum]);
                _isAdded.RemoveAt(_randomNum);
            }
        }
        #endregion
        #region リスト内の値の総和・平均値・分散値・標準偏差などを求める getSum/Average/Bunsan/Hyozyunhensa
        /// <summary>
        /// リスト内の値の総和を返します．
        /// </summary>
        /// <param name="_valuelist"></param>
        /// <returns></returns>
        public static int getSum_InList(List<int> _valuelist)
        {
            // 合計
            int _sum = 0;
            foreach (int i in _valuelist)
            {
                _sum += i;
            }
            return _sum;
        }
        /// <summary>
        /// リスト内の値の総和を返します．
        /// </summary>
        /// <param name="_valuelist"></param>
        /// <returns></returns>
        public static double getSum(List<double> _valuelist)
        {
            // 合計
            double _sum = 0.0;
            foreach (double i in _valuelist)
            {
                _sum += i;
            }
            return _sum;
        }
        /// <summary>
        /// リスト内の値の平均値を返します．
        /// </summary>
        /// <param name="_valuelist"></param>
        /// <returns></returns>
        public static double getAverage_InList(List<int> _valuelist)
        {
            List<double> _doublelist = MyTools.parseDouble_Lists(_valuelist);
            return getAverage_InList(_doublelist);
        }
        /// <summary>
        /// リスト内の値の平均値を返します．
        /// </summary>
        /// <param name="_valuelist"></param>
        /// <returns></returns>
        public static double getAverage_InList(List<double> _valuelist)
        {
            double _number = (double)_valuelist.Count;
            // 合計
            double _sum = getSum(_valuelist);
            // 平均
            double _average = _sum / _number;
            return _average;
        }
        #region 草案：平均との差を取得する：getDistanceFromAverage こんなあたりまえのようなメソッド要らないか…
        ///// <summary>
        ///// リスト内の値の平均との差（平均よりどれくらい大きい／小さいか）を返します。
        ///// 
        ///// ※一度平均値を計算していれば、第３引数_averageに平均値を指定してください。計算量が短縮されます。
        ///// なければ、Double.MinValueを与えてください。
        ///// </summary>
        ///// <typeparam name="?"></typeparam>
        ///// <param name="?"></param>
        ///// <returns></returns>
        //public static double getDistanceFromAverage_InList(List<int> _valuelist, int _checkedInstance, double _average)
        //{
        //    // 平均値が代入されてなければ計算し直す
        //    if (_average == Double.MinValue) // ==0にすると、本当に平均が0のときに再計算されてしまう
        //    {
        //        _average = getAverage_InList(_valuelist);
        //    }
        //    // 平均との差を取得
        //    double _distance = _checkedInstance - _average;
        //    return _distance;
        //}
        ///// <summary>
        ///// リスト内の値の平均との差（平均よりどれくらい大きい／小さいか）を返します。
        ///// 
        ///// ※一度平均値を計算していれば、第３引数_averageに平均値を指定してください。計算量が短縮されます。
        ///// なければ、Double.MinValueを与えてください。
        ///// </summary>
        ///// <typeparam name="?"></typeparam>
        ///// <param name="?"></param>
        ///// <returns></returns>
        //public static double getDistanceFromAverage_InList(List<double> _valuelist, double _checkedInstance, double _average)
        //{
        //    // 平均値が代入されてなければ計算し直す
        //    if (_average == Double.MinValue) // ==0にすると、本当に平均が0のときに再計算されてしまう
        //    {
        //        _average = getAverage_InList(_valuelist);
        //    }
        //    // 平均との差を取得
        //    double _distance = _checkedInstance - _average;
        //    return _distance;
        //}
        #endregion
        /// <summary>
        /// リスト内の値の分散値（値のバラツキ度合い）を返します。
        /// </summary>
        /// <typeparam name="?"></typeparam>
        /// <param name="?"></param>
        /// <returns></returns>
        public static double getBunsan_InList(List<int> _valuelist){
            double _bunsan = 0.0;
            // 平均
            double _average = getAverage_InList(_valuelist);
            double _distance = 0.0;
            double _sum_distance2Zyou = 0.0;
            foreach (int _value in _valuelist)
            {
                // (_i1_shortPathName__StringBuilder_Capactiy)平均との差を取得
                _distance = _value - _average;
                // (i2)平均との差の二乗を足していく。
                _sum_distance2Zyou += _distance * _distance;
            }
            // (j)足した二乗和を、要素数で割ると、分散が計算できる。
            _bunsan = _sum_distance2Zyou / (double)_valuelist.Count;
            return _bunsan;
        }
        /// <summary>
        /// リスト内の値の分散値（値のバラツキ度合い）を返します。
        /// </summary>
        /// <typeparam name="?"></typeparam>
        /// <param name="?"></param>
        /// <returns></returns>
        public static double getBunsan_InList(List<double> _valuelist)
        {
            double _bunsan = 0.0;
            // 平均
            double _average = getAverage_InList(_valuelist);
            double _distance = 0.0;
            double _sum_distance2Zyou = 0.0;
            foreach (double _value in _valuelist)
            {
                // (_i1_shortPathName__StringBuilder_Capactiy)平均との差を取得
                _distance = _value - _average;
                // (i2)平均との差の二乗を足していく。
                _sum_distance2Zyou += _distance * _distance;
            }
            // (j)足した二乗和を、要素数で割ると、分散が計算できる。
            _bunsan = _sum_distance2Zyou / (double)_valuelist.Count;
            return _bunsan;
        }

        /// <summary>
        /// リスト内の値の標準偏差（値のバラツキ度合い、分散の√）を返します。
        /// </summary>
        public static double getHyouzyunHensa_InList(List<int> _valuelist)
        {
            double _hyouzyunHensa = 0.0;
            _hyouzyunHensa = Math.Sqrt(getBunsan_InList(_valuelist));
            return _hyouzyunHensa;
        }
        /// <summary>
        /// リスト内の値の標準偏差（値のバラツキ度合い、分散の√）を返します。
        /// </summary>
        public static double getHyouzyunHensa_InList(List<double> _valuelist)
        {
            double _hyouzyunHensa = 0.0;
            _hyouzyunHensa = Math.Sqrt(getBunsan_InList(_valuelist));
            return _hyouzyunHensa;
        }
        /// <summary>
        /// リスト内の値の平均値・分散値・標準偏差などを求めます．標準偏差を返します．
        /// </summary>
        /// <param name="_valuelist">リスト配列</param>
        /// <param name="_average">平均値</param>
        /// <param name="_disperison">分散</param>
        /// <param name="_deviation">標準偏差</param>
        /// <returns>標準偏差</returns>
        public static double getAnalyzedValues_InList(List<int> _valuelist, out int _max, out int _min, out double _average, out double _disperison, out double _deviation)
        {
            List<double> _doublelist = MyTools.parseDouble_Lists(_valuelist);
            // 出力用のdouble型変数（後でint型に変換して使う）
            double _maxDouble = Double.MinValue;
            double _minDouble = Double.MaxValue;
            double _hyouzyunhensa = getAnalyzedValues_InList(_doublelist, out _maxDouble, out _minDouble, out _average, out _disperison, out _deviation);
            _max = (int)_maxDouble;
            _min = (int)_minDouble;
            return _hyouzyunhensa;
        }
        /// <summary>
        /// リスト内の値の平均値・分散値・標準偏差などを求めます．標準偏差を返します．
        /// </summary>
        /// <param name="_valuelist">リスト配列</param>
        /// <param name="_average">平均値</param>
        /// <param name="_disperison">分散</param>
        /// <param name="_deviation">標準偏差</param>
        /// <returns>標準偏差</returns>
        public static double getAnalyzedValues_InList(List<double> _valuelist, out double _max, out double _min, out double _average, out double _disperison, out double _deviation)
        {
            double _number = (double)_valuelist.Count;
            // 平均
            _average = MyTools.getAverage_InList(_valuelist);
            // 最小値と最大値もついでに取得
            _max = Double.MinValue;
            _min = Double.MaxValue;

            // 分散（正規分布と仮定した約66％が居る値の範囲，同変数の共分散）
            double _squaredSum = 0.0;
            foreach (double i in _valuelist)
            {
                if (i > _max) _max = i;
                if (i < _min) _min = i;
                _squaredSum += i * i;
            }
            _disperison = ((1.0 / _number) * _squaredSum) - (_average * _average);

            // 標準偏差（分散が1と仮定した約66％が居る値の範囲）
            _deviation = Math.Sqrt(_disperison);

            return _deviation;
        }
        #endregion
        #region ２つのリスト内の値の共分散・相関係数などを求めます． getAnalyzedValues
        /// <summary>
        /// ２つのリスト内の値の共分散・相関係数などを求めます．相関係数を返します．
        /// </summary>
        /// <param name="_valuelist_base1">基底リスト配列（変化前）</param>
        /// <param name="_valuelist_sub2">サブリスト配列（変化後）</param>
        /// <param name="_covariance">共分散</param>
        /// <param name="_correlation_coefficient">相関係数</param>
        /// <returns>相関係数</returns>
        public static double getAnalyzedValues_In2Lists(List<int> _valuelist_base1, List<int> _valuelist_sub2, out double _covariance, out double _correlation_coefficient)
        {
            List<double> _doublelist_base1 = MyTools.parseDouble_Lists(_valuelist_base1);
            List<double> _doublelist_sub2 = MyTools.parseDouble_Lists(_valuelist_sub2);
            return getAnalyzedValues_In2Lists(_doublelist_base1, _doublelist_sub2, out _covariance, out _correlation_coefficient);
        }
        /// <summary>
        /// ２つのリスト内の値の共分散・相関係数などを求めます．相関係数を返します．
        /// </summary>
        /// <param name="_valuelist_base1">基底リスト配列（変化前）</param>
        /// <param name="_valuelist_sub2">サブリスト配列（変化後）</param>
        /// <param name="_covariance">共分散</param>
        /// <param name="_correlation_coefficient">相関係数</param>
        /// <returns>相関係数</returns>
        public static double getAnalyzedValues_In2Lists(List<double> _valuelist_base1, List<double> _valuelist_sub2, out double _covariance, out double _correlation_coefficient)
        {
            double number_base1 = (double)_valuelist_base1.Count;
            double number_sub2 = (double)_valuelist_sub2.Count;
            if (number_base1 != number_sub2)
            {
                Console.WriteLine("エラー： 共分散・相関係数は，２つのリストの配列数が等しくないと正しく計算できません．");
                // 一応base1の数で計算する． // return 0.0;
            }
            // 平均
            double _max_base1, _min_base1 ,_average_base1, _disperison_base1, _deviation_base1; // 平均，分散，標準偏差
            double _max_sub2, _min_sub2, _average_sub2, _disperison_sub2, _deviation_sub2;
            MyTools.getAnalyzedValues_InList(_valuelist_base1, out _max_base1, out _min_base1, out _average_base1, out _disperison_base1, out _deviation_base1);
            MyTools.getAnalyzedValues_InList(_valuelist_sub2, out _max_sub2, out _min_sub2, out _average_sub2, out _disperison_sub2, out _deviation_sub2);

            // 共分散
            double _delta_FromAverage_base1, _delta_FromAverage_sub2;
            double _sum_Delta1_Multiply_Delta2 = 0.0;
            for (int i = 0; i < _valuelist_base1.Count; i++)
            {
                _delta_FromAverage_base1 = _valuelist_base1[i] - _average_base1;
                _delta_FromAverage_sub2 = _valuelist_sub2[i] - _average_sub2;
                // それぞれの平均値との差をかけあわせたものの総和
                _sum_Delta1_Multiply_Delta2 += _delta_FromAverage_base1 * _delta_FromAverage_sub2;

            }
            // 最後にデータ数で割る
            _covariance = (1.0 / number_base1) * _sum_Delta1_Multiply_Delta2;

            // 相関係数（データ1に対して，データ2がどれほど比例関係にあるか(-1.0～+1.0)）
            _correlation_coefficient = _covariance / (_deviation_base1 * _deviation_sub2);

            return _correlation_coefficient;
        }
        #endregion
        #region ２つのリストの値が全て等しいかどうかを調べる。: isListEquals
        /// <summary>
        /// ２つのリストの要素数が等しく、かつそれぞれの値が全て等しければ（値の等価：_values1[i]==_values2[i]なら）true、
        /// それ以外ならfalseを返します。
        /// なお、_values1=lis2=null や、_values1.Count=_values2.Count=0 の場合も、falseを返します。
        /// 　　
        /// 　　※片方リストのnullチェックや配列外エラーチェックもちゃんとしていますので、どんなリストを引数に与えても大丈夫です。
        /// </summary>
        public static bool isListEquals<T>(List<T> _list1, List<T> _list2)
            where T:struct // Tは値型。値型はnullは取れない。（値型のような動作が可能だが、nullを取れるstring型は参照型なので、where T:structではたぶん含まれない？）
        {
            bool _isEquals = true;
            // 例外的な返り値
            if (_list1 == null && _list2 == null) return false;
            if (_list1 == null && _list2 != null) return false;
            if (_list1 != null && _list2 == null) return false;
            if (_list1 != null && _list2 != null)
            {
                // 要素数が違えばfalse
                if (_list1.Count != _list2.Count) return false;

                for (int i = 0; i < _list1.Count; i++)
                {
                    // Equalsメソッドは、where T:structの値型の比較に使うと、「値の等価」を調べます。
                    // 参照型の比較に使うと、通常は「参照の等価」を調べます。しかし、String型のように、クラスのEqualsメソッドがオーバーライドされていれば、参照型でも「値の等価」を調べます。
                    //  補足：静的メソッドのObject.Equals(Object, Object)メソッドは、2つのオブジェクトがどちらもNULLでなければ、一番目のパラメータに渡されたオブジェクトのEqualsメソッドを呼び出した結果を返します。
                    //      「参照の等価」を調べるためには、Object.ReferenceEqualsメソッドを使用することもできます。さらにC#では、Object型にキャストしてから==演算子で比較することでも、確実に参照の等価を調べることができます。

                    if (_list1[i].Equals(_list2[i]) == false) // Tが値型なら、_values1 == _values2 と同じ意味
                    {
                        _isEquals = false;
                    }
                }
            }
            return _isEquals;
        }
        /// <summary>
        /// ２つの配列の要素数が等しく、かつそれぞれの値が全て等しければ（値の等価：_values1[i]==_values2[i]なら）true、
        /// それ以外ならfalseを返します。
        /// なお、_values1=lis2=null や、_values1.Count=_values2.Count=0 の場合も、falseを返します。
        /// 　　
        /// 　　※片方リストのnullチェックや配列外エラーチェックもちゃんとしていますので、どんなリストを引数に与えても大丈夫です。
        /// </summary>
        public static bool isValuesEquals<T>(T[] _values1, T[] _values2)
            where T : struct // Tは値型。値型はnullは取れない。（値型のような動作が可能だが、nullを取れるstring型は参照型なので、where T:structではたぶん含まれない？）
        {
            bool _isEquals = true;
            // 例外的な返り値
            if (_values1 == null && _values2 == null) return false;
            if (_values1 == null && _values2 != null) return false;
            if (_values1 != null && _values2 == null) return false;
            if (_values1 != null && _values2 != null)
            {
                // 配列の長さが違えばfalse
                if (_values1.Length != _values2.Length) return false;

                for (int i = 0; i < _values1.Length; i++)
                {
                    // Equalsメソッドは、where T:structの値型の比較に使うと、「値の等価」を調べます。
                    // 参照型の比較に使うと、通常は「参照の等価」を調べます。しかし、String型のように、クラスのEqualsメソッドがオーバーライドされていれば、参照型でも「値の等価」を調べます。
                    //  補足：静的メソッドのObject.Equals(Object, Object)メソッドは、2つのオブジェクトがどちらもNULLでなければ、一番目のパラメータに渡されたオブジェクトのEqualsメソッドを呼び出した結果を返します。
                    //      「参照の等価」を調べるためには、Object.ReferenceEqualsメソッドを使用することもできます。さらにC#では、Object型にキャストしてから==演算子で比較することでも、確実に参照の等価を調べることができます。

                    if (_values1[i].Equals(_values2[i]) == false) // Tが値型なら、_values1 == _values2 と同じ意味
                    {
                        _isEquals = false;
                    }
                }
            }
            return _isEquals;
        }
        #endregion

        // enum型（独自の列挙型）
        #region Enum型の要素数を取ってくる: getEnumItemCount
        /// <summary>
        /// Enum型の要素数を取ってきます。具体的には、Enum.GetValues(typeof(T)).Length を返します。
        /// 
        /// 　※注意：なお、列挙体をint型で管理する場合は、このメソッドを使わないでください。
        /// なぜなら、各要素が「要素=値,」で値が割り当てられている場合、以下ではインデックスが範囲外になる可能性が高いからです。
        ///     【×】for(int i=0; i＜MyTools.getEnumItemCount＜列挙体名＞(); i++)
        /// また、以下の【△】方法ではエラーが出ませんが、範囲外のインデックスも調べるため、少し非効率です。
        /// 
        /// 　※結論：int型で管理する場合は【△】を使い、Enum型のまま管理する場合は【○】を使ってください。
        ///     【△】for(int i=0; i＜MyTools.getEnumIntMaxCount＜列挙体名＞(); i++)
        ///     【○】foreach(列挙体名 _enumItem in MyTools.getEnumItems＜列挙体名＞()))
        ///  なお、要素を編集したい場合は、
        ///     【○】List＜列挙体名＞ _enumList = MyTools.getEnumItemlist＜列挙体名＞();
        ///         for(in i=0; i＜_enumList.Count; i++)
        ///         {
        ///             列挙体名[i] = ...
        ///         } 
        /// とかけます。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static int getEnumItemCount<T>()
        {
            return Enum.GetValues(typeof(T)).Length;
        }
        /// <summary>
        /// Enum型の要素に割り当てられた最大値+1（＝インデックス最大値+1）を返します。
        /// 内部では、(Enum.GetValues(typeof(T))の最大値+1)　を取得しています。
        /// 「必ず要素数以上なことを保証できる値」を取得できるので、Enum型をint型で管理する時の最大値検出などに使えます。
        /// 
        ///     ※注意：Enum型の各要素が「要素=値,」で割り当てられている場合、要素数とは異なります。
        /// 正しい要素数を取得したい場合は、　getEnumItemCount＜T＞()　を使ってください。　
        ///
        /// 　  ※結論：int型で管理する場合は【△】を使い、Enum型のまま管理する場合は【○】を使ってください。
        ///     【△】for(int i=0; i＜MyTools.getEnumIntMaxCount＜列挙体名＞(); i++)
        ///     【○】foreach(列挙体名 _enumItem in MyTools.getEnumItems＜列挙体名＞()))
        ///  なお、要素を編集したい場合は、
        ///     【○】List＜列挙体名＞ _enumList = MyTools.getEnumItemlist＜列挙体名＞();
        ///         for(in i=0; i＜_enumList.Count; i++)
        ///         {
        ///             列挙体名[i] = ...
        ///         } 
        /// とかけます。
        ///  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static int getEnumIntMaxCount<T>()
        {
            // [Q]GetValuesをforeach文で回さずにint型に変換するやり方がわからない。ので、わざわざ回している
            // ほんとはこうしたい
            //return MyTools.getMaxValue(  ( Enum.GetValues( typeof(T) )  ) + 1;
            int[] _enumValues = MyTools.getEnumValues<T>();
            int _maxValue_equals_EnumIntMax = MyTools.getMaxValue(_enumValues);
            return _maxValue_equals_EnumIntMax + 1;
        }
        #endregion
        #region Enum型（独自の列挙体）の配列を取ってくる: getEnumItems / getEnumItemList
        /// <summary>
        /// Enum型（独自の列挙体）の全ての要素を格納した配列を取ってきます．
        /// 具体的には，「Enum.GetValues(typeof(T))」を使っています．
        /// 
        ///        ■使用例：　全ての要素を参照したい時、
        ///             foreach(列挙体名 _enumItem in MyTools.getEnumItems＜列挙体名＞()))
        ///          のようにかけるので、便利です。
        ///          
        ///        ※なお、全ての要素を追加・削除・編集したい時、
        ///             List＜列挙体名＞ _enumList = MyTools.getEnumItemList＜列挙体名＞()))
        ///          のリストを使った方が、便利です。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        /// <returns></returns>
        public static Array getEnumItems<T>()
        {
            Type _type = typeof(T);
            if (_type.IsEnum == false)
            {
                return null; // Enumじゃなかったら、nullを返す。
            }
            // Array型のままだと、値の取り方がobject型なので、(int)(_array.GetValue(_index))と書かないといけない。
            // 開発者が戸惑うといけないので、使い慣れた　(int)_array[_index]　で簡単に値に取れるように、
            // Enum[]型にキャストしておく。
            // ※また、こういう風にも使える。
            //      foreach(列挙体名 _item_includingWaitWord in _enumItems)      
            //      for(int i=0; i< _enumItems.Length; i++)    
            Array _array = Enum.GetValues(_type);
            //これ、コンパイルは通るけど実行エラーになる。独自列挙体E**[]型をEnum[]型にキャストできませんだって。
            //できない。残念。Enum[] _enumItems = (Enum[])Enum.GetValues(_name);

            return _array;
        }
        /// <summary>
        /// Enum型（独自の列挙体）の全ての要素を格納したリストを取ってきます．
        /// 具体的には，「new List＜Enum＞( (Enum[])Enum.GetValues(typeof(T)) )」を使っています．
        /// 
        ///        ■使用例：　全ての要素を追加・削除・編集したい時、
        ///             List＜列挙体名＞ _enumList = MyTools.getEnumItemList＜列挙体名＞()))
        ///          のように編集可能な状態に出来るので、便利です。
        ///
        ///          なお、全ての要素を参照したい場合は、
        ///             foreach(列挙体名 _enumItem in MyTools.getEnumItems＜列挙体名＞()))
        ///          と書いた方が、高速です。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        /// <returns></returns>
        public static List<T> getEnumItemList<T>()
        {
            Array _array = getEnumItems<T>();
            if (_array == null) return new List<T>(); // Count=0の空のリストを返す。
            T[] _enumItems = new T[_array.Length];
            List<T> _enumList = new List<T>(_enumItems);
            return _enumList;
        }
        ///// <summary>
        ///// Enum型（独自の列挙体）の全ての要素を格納した配列を取ってきます．
        ///// 具体的には，「Enum.GetValues(typeof(T))」を使っています．
        ///// 
        /////        ■使用例：　全ての要素を参照したい時、
        /////             foreach(列挙体名 _enumItem in MyTools.getEnumItems＜列挙体名＞()))
        /////          のようにかけるので、便利です。
        /////          
        /////        ※なお、全ての要素を追加・削除・編集したい時、
        /////             List＜列挙体名＞ _enumList = MyTools.getEnumItemList＜列挙体名＞()))
        /////          のリストを使った方が、便利です。
        ///// </summary>
        ///// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        ///// <returns></returns>
        //public static T[] getEnumItems<T>()
        //{
        //    Type _name = typeof(T);
        //    if (_name.IsEnum == false)
        //    {
        //        return null; // Enumじゃなかったら、nullを返す。
        //    }
        //    // Array型のままだと、値の取り方がobject型なので、(int)(_array.GetValue(_index))と書かないといけない。
        //    // 開発者が戸惑うといけないので、使い慣れた　(int)_array[_index]　で簡単に値に取れるように、
        //    // Enum[]型にキャストしておく。
        //    // ※また、こういう風にも使える。
        //    //      foreach(列挙体名 _item_includingWaitWord in _enumItems)      
        //    //      for(int i=0; i< _enumItems.Length; i++)    
        //    //Array _array = Enum.GetValues(_name);
        //    //これ、コンパイルは通るけど実行エラーになる。独自列挙体E**[]型をEnum[]型にキャストできませんだって。残念。
        //    //Enum[] _enumItems = (Enum[])Enum.GetValues(_name);
        //    // これならいける
        //    T[] _enumItems = (T[])Enum.GetValues(_name);

        //    return _enumItems;
        //}
        ///// <summary>
        ///// Enum型（独自の列挙体）の全ての要素を格納したリストを取ってきます．
        ///// 具体的には，「new List＜Enum＞( (Enum[])Enum.GetValues(typeof(T)) )」を使っています．
        ///// 
        /////        ■使用例：　全ての要素を追加・削除・編集したい時、
        /////             List＜列挙体名＞ _enumList = MyTools.getEnumItemList＜列挙体名＞()))
        /////          のように編集可能な状態に出来るので、便利です。
        /////
        /////          なお、全ての要素を参照したい場合は、
        /////             foreach(列挙体名 _enumItem in MyTools.getEnumItems＜列挙体名＞()))
        /////          と書いた方が、高速です。
        ///// </summary>
        ///// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        ///// <returns></returns>
        //public static List<Enum> getEnumItemList<T>()
        //{
        //    Enum[] _enums = getEnumArray<T>();
        //    if (_enums == null) return new List<Enum>(); // Count=0の空のリストを返す。
        //    List<Enum> _enumList = new List<Enum>(_enums);
        //    return _enumList;
        //}
        #endregion
        #region Enum型（独自の列挙型）の値(int型)をまとめて取ってくる: getEnumValues / getEnumValueList

        // ■1個用　→　現段階での結論：メソッドを呼び出さず、int型キャスト「(int)_enumItem」でＯＫ
        // 具体的な、(int)EStringCharType.e01*** だったら変換できるのに、なぜかEnum型にするとint型に変換できない
        // ので、とりあえずEnum型の値を取得するには、(int)_enumItemでやって。（Javaには変換できなさそうだけど…）
        ///// <summary>
        ///// Enum型（独自の列挙型）の値を取ってきます。具体的には、(int)_enumItem をしているだけです。
        ///// </summary>
        ///// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        //public static int getEnumValue<T>(T _enumItem)
        ////where Enum : ValueType, IComparable, IFormattable, IConvertible // Tは列挙型
        //{
        //    Type _name = typeof(T);
        //    if (_name.IsEnum == false)
        //    {
        //        return 0; // Enumじゃなかったら、0を返す。
        //    }
        //    return (int)_enumItem; // [Q]具体的な、(int)EStringCharType.e01*** だったら変換できるのに、なぜかEnum型にするとint型に変換できない
        //}

        // ■まとめて取得する用
        /// <summary>
        /// Enum型（独自の列挙型）の値（インデックス）を配列で取ってくる
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        public static int[] getEnumValues<T>()
        //where Enum : ValueType, IComparable, IFormattable, IConvertible // Tは列挙型
        {
            Type _type = typeof(T);
            if (_type.IsEnum == false)
            {
                return new int[0]; // Enumじゃなかったら、要素数=0の配列を返す。
            }
            int[] _values = (int[])(  Enum.GetValues(typeof(T))  );
            return _values;
        }
        /// <summary>
        /// Enum型（独自の列挙型）の値（インデックス）リストを取ってくる
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        /// <returns></returns>
        public static List<int> getEnumValueList<T>()
        //where Enum : ValueType, IComparable, IFormattable, IConvertible // Tは列挙型
        {
            Type _type = typeof(T);
            if (_type.IsEnum == false)
            {
                return new List<int>(); // Enumじゃなかったら、Count=0のリストを返す。
            }
            List<int> _values = new List<int>();
            Array _array = Enum.GetValues(typeof(T));
            foreach (object _value in _array)
            {
                _values.Add((int)_value);
            }
            _array.Initialize(); // リソースを解放…DisposeがないのでとりあえずInitialize()
            return _values;
        }
        /*
        /// <summary>
        /// Enum型（独自の列挙型）の値を取ってくる．Tを(int)型にキャストできない場合は-1が返る．
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        /// <returns></returns>
        public static int getEnumValue<T>(T _EnumType)
            //where T : ValueType, IComparable, IFormattable, IConvertible // Tは列挙型
        {
            Type _name = _EnumType.GetType();
            if (_name.IsEnum)
            {

                return getEnumValueList(_EnumType)[(int)_EnumType]; // Tではintキャストができない
            }
            else
            {
                return -1;
            }
        }
        */
        #endregion
        #region Enum型（独自の列挙型）の要素の名前を取ってくる: getEnumName
        /// <summary>
        /// Enum型（独自の列挙型）の要素の名前の配列を取ってきます。メソッド名の後ろに＜名前を取りたいEnum型＞を指定してください。
        ///        返り値はnullにはならないことを保証します。要素がなくとも、Enum型が存在しなくとも、要素数0の空の配列を返します。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        public static string[] getEnumNames<T>()
        {
            Type _type = typeof(T);
            if (_type.IsEnum == false)
            {
                return new string[0]; // Enumじゃなかったら、Length=0のリストを返す。
            }
            string[] _array = Enum.GetNames(typeof(T));
            if (_array == null) _array = new string[0]; // nullだったら、0個の配列にしてエラー回避
            return _array;
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素の名前リストを取ってきます。メソッド名の後ろに＜名前を取りたいEnum型＞を指定してください。
        /// 　　返り値はnullにはならないことを保証します。要素がなくとも、Enum型が存在しなくとも、要素数0の空のリストを返します。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        public static List<string> getEnumNameList<T>()
        {
            Type _type = typeof(T);
            if (_type.IsEnum == false)
            {
                return new List<string>(); // Enumじゃなかったら、Count=0のリストを返す。
            }
            List<string> _names = new List<string>();
            string[] _array = getEnumNames<T>();
            foreach (string _value in _array)
            {
                _names.Add(_value);
            }
            _array.Initialize(); // 念のため、メモリ解放
            return _names; // 返り値はnullにはなりません。要素がなくとも、Enum型が存在しなくとも、要素数0の空のリストを返します。
        }

        /// <summary>
        /// Enum型（独自の列挙型）の要素の名前リストを取ってきます。メソッド名の後ろに＜名前を取りたいEnum型＞を指定してください。引数は、リストに含めたくないEnum型（例えば.Countなど）を３つ指定してください。３つもない場合は、同じものを指定してもらってＯＫです。
        /// 　　返り値はnullにはならないことを保証します。要素がなくとも、Enum型が存在しなくとも、要素数0の空のリストを返します。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        public static List<string> getEnumNameList<T>(Enum _EnumItem1_NotIncludingInNameList, Enum _EnumItem2_NotIncludingInNameList, Enum _EnumItem3_NotIncludingInNameList)
        {
            Type _type = typeof(T);
            if (_type.IsEnum == false)
            {
                return new List<string>(); // Enumじゃなかったら、Count=0のリストを返す。
            }
            List<string> _names = getEnumNameList<T>();
            string _notIncludingEnumName = getEnumName(_EnumItem1_NotIncludingInNameList);
            _names.Remove(_notIncludingEnumName);
            _notIncludingEnumName = getEnumName(_EnumItem2_NotIncludingInNameList);
            _names.Remove(_notIncludingEnumName);
            _notIncludingEnumName = getEnumName(_EnumItem3_NotIncludingInNameList);
            _names.Remove(_notIncludingEnumName);
            return _names;
        }


        /// <summary>
        /// Enum型（独自の列挙型）の値が指定値の名前を取ってくる．Enum.GetName(typeof(T), _n_enumValue);だけなので。結構処理は早いはず。
        /// 同じ値が２つ以上ある場合は、どちらをとるかはわかりません（Ｃ＃では保証されていない）。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        public static string getEnumName<T>(int _n_enumValue)
        {
            Type _type = typeof(T);
            if (_type.IsEnum == false)
            {
                return ""; // Enumじゃなかったら、""を返す。
            }
            string _name = "";
            _name = Enum.GetName(typeof(T), _n_enumValue);
            return _name;
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素の名前を取ってくる。
        /// やってるのは、Enum型の要素.ToString()だけです。これで名前が取れるので、このメソッドを使わずとも試してみてください。
        /// 
        /// 
        /// 参考：　Ｃ＃でのEnum型のToString()の挙動：　http://www.atmarkit.co.jp/fdotnet/csharp_abc2/csabc2_016/cs2_016_04.html
        /// </summary>
        public static string getEnumName(Enum _enum_OneItem)
        {
            return _enum_OneItem.ToString();
        }
        ///// <summary>
        ///// Enum型（独自の列挙型）の要素の名前を取ってくる．foreachしているので、ちょっと遅いかも。Tを(int)型にキャストできない場合は""が返る．
        ///// </summary>
        ///// <param name="_typeof_EnumType"></param>
        ///// <returns></returns>
        //public static string getEnumName<T>(T _EnumItem)
        //{
        //    string _name = "";
        //    Type _name = typeof(T);
        //    if (_name.IsEnum == true)
        //    {
        //        // Tが(int)キャストできないから，わざわざobject型の_arrayで_EnumTypeと等しい要素を探してきている．
        //        int _index = -1;
        //        Array _array = Enum.GetValues(_name);
        //        foreach (object _value in _array)
        //        {
        //            T _t = (T)_value;
        //            if (_t.Equals(_EnumItem) == true)
        //            {
        //                _index = (int)_value;
        //                break;
        //            }
        //        }
        //        if (_index != -1 && _index <= _array.Length-1)
        //        {
        //            _name = getEnumNames<T>()[_index];
        //        }
        //    }

        //    return _name;
        //}
        ///// <summary>
        ///// Enum型（独自の列挙型）の要素の名前リストを取ってきます。メソッド名の後ろに＜名前を取りたいEnum型＞を指定してください。引数は、リストに含めたくないEnum型（例えば.Countなど）を指定してください。指定しなくともＯＫです。
        ///// 　　返り値はnullにはならないことを保証します。要素がなくとも、Enum型が存在しなくとも、要素数0の空のリストを返します。
        ///// </summary>
        ///// <param name="_typeof_EnumType"></param>
        ///// <returns></returns>
        //public static List<string> getEnumNameList<T>(T _EnumItem_NotIncludingInNameList)
        //{
        //    List<string> _names = getEnumNameList<T>();
        //    string _notIncludingEnumName = getEnumName<T>(_EnumItem_NotIncludingInNameList);
        //    _names.Remove(_notIncludingEnumName);
        //    return _names;
        //}
        #endregion
        #region Enum型（独自の列挙型）の要素名のキー（"_"や"＿"や"・"で区切られた部分）を取ってくる: getEnumKeyName_***
        /// <summary>
        /// Enum型（独自の列挙型）の要素のキーインデックスだけ（最初の"_"や"＿"や"・"より左の部分）を取ってきます。
        /// 
        /// 　■使用例：　_EnumItem = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        /// 　               getEnumKeyName_OnlyIndex(_EnumItem) = "e01"
        /// </summary>
        /// <typeparam name="T">Enum列挙体のクラス名を書く。（例: EKeyCode、EPara、ESE・効果音 など）</typeparam>
        /// <param name="_EnumItem">Enum列挙体の要素（例：EKeyCode.a、EPara.***、ESE・効果音.***など）</param>
        /// <returns></returns>
        public static string getEnumKeyName_OnlyFirstIndexWord(Enum _EnumItem)
        {
            return getEnumKeyName(_EnumItem, 0, 1, 
                EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・");
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素のキーインデックスだけ（最後の"_"や"＿"や"・"より右の部分）を取ってきます。
        /// 
        /// 　■使用例：　_EnumItem = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        /// 　               getEnumKeyName_OnlyIndex(_EnumItem) = "あたえるだめーじにえいきょう"
        /// </summary>
        /// <typeparam name="T">Enum列挙体のクラス名を書く。（例: EKeyCode、EPara、ESE・効果音 など）</typeparam>
        /// <param name="_EnumItem">Enum列挙体の要素（例：EKeyCode.a、EPara.***、ESE・効果音.***など）</param>
        /// <returns></returns>
        public static string getEnumKeyName_OnlyLastIndexWord(Enum _EnumItem)
        {
            return getEnumKeyName(_EnumItem, 9999, 1,
                EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・");
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素のキー英語名だけ（最初の"_"や"＿"や"・"より右の部分で、次に出現する英語の１単語）を取ってきます。
        /// 
        /// 　■使用例：　_EnumItem = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        /// 　               getEnumKeyName_OnlyEnglish(_EnumItem) = "ATK"
        /// </summary>
        /// <typeparam name="T">Enum列挙体のクラス名を書く。（例: EKeyCode、EPara、ESE・効果音 など）</typeparam>
        /// <param name="_EnumItem">Enum列挙体の要素（例：EKeyCode.a、EPara.***、ESE・効果音.***など）</param>
        /// <returns></returns>
        public static string getEnumKeyName_OnlyEnglish(Enum _EnumItem)
        {
            return getEnumKeyName(_EnumItem, 1, 1, 
                EStringCharType.t0b_English_半角英数字＿記号を含む, "_", "＿", "・");
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素のキー日本語名だけ（最初の"_"や"＿"や"・"より右の部分で、次に出現する日本語の１単語）を取ってきます。
        /// 
        /// 　■使用例：　_EnumItem = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        /// 　               getEnumKeyName_OnlyJapanese(_EnumItem) = "攻撃力"
        /// </summary>
        /// <typeparam name="T">Enum列挙体のクラス名を書く。（例: EKeyCode、EPara、ESE・効果音 など）</typeparam>
        /// <param name="_EnumItem">Enum列挙体の要素（例：EKeyCode.a、EPara.***、ESE・効果音.***など）</param>
        /// <returns></returns>
        public static string getEnumKeyName_OnlyJapanese(Enum _EnumItem)
        {
            return getEnumKeyName(_EnumItem, 2, 1,
                EStringCharType.t0d_Japanese3_全角日本語＿半角カタカナ英数字記号を含まない, "_", "＿", "・");
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素名のうち、独自に定義したキー名だけを取得します。
        ///   該当する区切り文字が１つも見つからなかった場合は，文字列_EnumItem.ToString() が返ります。
        ///   N番目の区切り文字が見つからなかった場合は，最後に見つかった区切り文字より１文字右側をキーの開始位置とします。
        /// 　N+M番目の区切り文字が見つからなかった場合は，N番目から右側全てを含んだ名前を返します。
        /// 　条件を満たす文字タイプの単語が見つからなかった場合は，""が返ります。
        /// 
        /// 　※このメソッドは、
        /// 　　「N番目の区切り文字 ～ N+M番目の区切り文字までの文字列を取ってくるか」、
        /// 　　「キー名の条件（全角カタカナあるいはひらがなで書かれてあるものだけを取得するなど）」、
        /// 　　「区切り文字（"_"や"＿"や"・"など）」
        /// 　　の設定など、取得するキー名を詳細に定義できます。
        /// 　　
        ///       
        ///  　 ■使用例：　_EnumItem = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        ///  　　　　"攻撃力" = getEnumKeyName(_EnumItem, 2, 1, EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・");
        ///  　　　　"ATK・攻撃力" = getEnumKeyName(_EnumItem, 1, 2, EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・");
        ///  　　　　"あたえるだめーじにえいきょう" = getEnumKeyName(_EnumItem, 999, 1, EStringCharType.t0d_JapaneseHiraganaOnly＿全角ひらがなのみ, "_", "＿", "・");
        /// </summary>
        /// <typeparam name="T">Enum列挙体のクラス名を書く。（例: EKeyCode、EPara、ESE・効果音 など）</typeparam>
        /// <param name="_EnumItem">Enum列挙体の要素（例：EKeyCode.a、EPara.***、ESE・効果音.***など）</param>
        /// <param name="_N_beginWordIndex">
        /// キー名開始インデックスN。取得するキー名が何個目の単語から始まるかを示す。
        /// （例：　「e01_ATK・攻撃力＿…」から "攻撃力" だけを取得したい場合、区切り文字が"_", "・", "＿"とすると、
        /// 左から"e01"で0個目→"ATK"で1個目→"攻撃力"で2個目と数えて、N=2）。
        /// 
        /// 　　　なお、最後の単語だけを取得したい場合、999など十分に大きな値を与えてもよい。
        /// </param>
        /// <param name="_M_tokenNum_includingAsKeyWord">
        /// 単語連結数M。取得するキー名が何個の単語を連結するか（連結時には区切り文字も含まれる）を示す。
        /// （例：　「e01_ATK・攻撃力＿…」から "攻撃力" だけを取得したい場合、
        /// 1単語なので、M=1）。
        /// 
        /// 　　　　なお、N個から右全てを取得したい場合、999など十分に大きい値でもよい。
        /// Mが1以上なら、文字タイプの条件を満たしている複数の単語が連結される。
        /// 条件を満たしてい単語は連結されない。
        /// </param>
        /// <param name="_keyWordStringCharType">
        /// ※指定なしなら0でもいい。取得するキー名が満たす文字タイプ。この条件を満たす単語だけがキーとなる。
        /// （例：　「ESE・効果音.e01_system01・決定音＿ピローン_A」から擬音語を示す "ピローン" だけを取得したい場合、
        ///         EStringCharType.t0d_Japanese3_HiraganaOnly＿全角ひらがなまたはカタカナのみ に指定するとよい。
        ///         
        /// 　　　　なお、条件を満たす単語が複数あった場合、Nで指定した開始インデックスから数えて、条件を満たすM個の単語を繋げた文字列を取得する。
        /// </param>
        /// <param name="_dividedStrings">
        /// 区切り文字の候補。名前に区切ってある"_", "＿", "・"などをコンマ区切りで指定する。
        /// これらのうちいずれの文字が出現しても、区切り文字は一個と数える。</param>
        /// <returns></returns>
        public static string getEnumKeyName(Enum _EnumItem, int _N_beginWordIndex, int _M_tokenNum_includingAsKeyWord, EStringCharType _keyWordStringCharType, params string[] _dividedStrings)
        //        public static string getEnumKeyName_InDetail<T>(Enum _EnumItem, bool _isTrue_UsingWordIndexNAndM_isFalse_UsingCharStringType, int _N_beginWordIndex, int _M_tokenNum_includingAsKeyWord, EStringCharType _keyWordStringCharType, params string[] _dividedStrings)
        {
            string _keyName = ""; // どんどん連結していくから、デフォルトは""
            string _enumString = _EnumItem.ToString();
            _keyName = getWord_InString(_enumString, _N_beginWordIndex, _M_tokenNum_includingAsKeyWord, _keyWordStringCharType, false, _dividedStrings);
            return _keyName;
        }

        // string型
        /// <summary>
        /// 文字列の最初の単語だけ（最初の"_"や"＿"や"・"より左の部分）を取ってきます。
        /// 
        /// 　■使用例：　_sting = "ATK・攻撃力";
        /// 　               getName_OnlyEnglish(_string) = "ATK"
        /// </summary>
        public static string getName_OnlyFirstIndexWord(string _string)
        {
            return getWord_InString(_string, 0, 1,
                EStringCharType.t0_None_未定義_何でもＯＫ, false, "_", "＿", "・");
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素のキーインデックスだけ（最後の"_"や"＿"や"・"より右の部分）を取ってきます。
        /// 
        /// 　■使用例：　_EnumItem = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        /// 　               getEnumKeyName_OnlyIndex(_EnumItem) = "あたえるだめーじにえいきょう"
        /// </summary>
        /// <typeparam name="T">Enum列挙体のクラス名を書く。（例: EKeyCode、EPara、ESE・効果音 など）</typeparam>
        /// <param name="_EnumItem">Enum列挙体の要素（例：EKeyCode.a、EPara.***、ESE・効果音.***など）</param>
        /// <returns></returns>
        public static string getName_OnlyLastIndexWord(string _string)
        {
            return getWord_InString(_string, 9999, 1,
                EStringCharType.t0_None_未定義_何でもＯＫ, false, "_", "＿", "・");
        }
        /// <summary>
        /// 文字列の英語名だけ（最初の"_"や"＿"や"・"より左の部分で、英語に変換可能な文字列）を取ってきます。
        /// 
        /// 　■使用例：　_sting = "ATK・攻撃力";
        /// 　               getName_OnlyEnglish(_string) = "ATK"
        /// </summary>
        public static string getName_OnlyEnglish(string _string)
        {
            return getWord_InString(_string, 0, 1,
                EStringCharType.t0b_English_半角英数字＿記号を含む, false, "_", "＿", "・");
        }
        /// <summary>
        /// 文字列の日本語名だけ（最初の"_"や"＿"や"・"より右の部分で、次に出現する日本語に変換可能な文字列）を取ってきます。
        /// 
        /// 　■使用例：　_sting = "ATK・攻撃力";
        /// 　               getName_OnlyJapanese(_string) = "攻撃力"
        /// </summary>
        public static string getName_OnlyJapanese(string _string)
        {
            return getWord_InString(_string, 1, 1,
                EStringCharType.t0d_Japanese3_全角日本語＿半角カタカナ英数字記号を含まない, false, "_", "＿", "・");
        }
        /// <summary>
        /// 引数の文字列_baseStringのうち、独自に定義したトークン（１つあるいは複数の単語を連結した文字列）だけを取得します。
        ///   該当する区切り文字が１つも見つからなかった場合は，文字列_baseString が返ります。
        ///   N番目の区切り文字が見つからなかった場合は，最後に見つかった区切り文字より１文字右側をキーの開始位置とします。
        /// 　N+M番目の区切り文字が見つからなかった場合は，N番目から右側全てを含んだ名前を返します。
        /// 　条件を満たす文字タイプの単語が見つからなかった場合は，""が返ります。
        /// 
        /// 　※このメソッドは、
        /// 　　「N番目の区切り文字 ～ N+M番目の区切り文字までの文字列を取ってくるか」、
        /// 　　「トークンの条件（全角カタカナあるいはひらがなで書かれてあるものだけを取得するなど）」、
        /// 　　「区切り文字（"_"や"＿"や"・"など）」
        /// 　　の設定など、取得するキー名を詳細に定義できます。
        /// 　　
        ///       
        ///  　 ■使用例：　_baseString = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        ///  　　　　"攻撃力" = getWord_InString(_baseString, 2, 1, EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・");
        ///  　　　　"ATK・攻撃力" = getWord_InString(_baseString, 1, 2, EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・");
        ///  　　　　"あたえるだめーじにえいきょう" = getWord_InString(_baseString, 999, 1, EStringCharType.t0d_JapaneseHiraganaOnly＿全角ひらがなのみ, "_", "＿", "・");
        /// </summary>
        /// <param name="_baseString">トークンが含まれる、元となる文字列</param>
        /// <param name="_N_beginWordIndex">
        /// トークン開始インデックスN。取得するトークンが何個目の単語から始まるかを示す。
        /// （例：　「e01_ATK・攻撃力＿…」から "攻撃力" だけを取得したい場合、区切り文字が"_", "・", "＿"とすると、
        /// 左から"e01"で0個目→"ATK"で1個目→"攻撃力"で2個目と数えて、N=2）。
        /// 
        /// 　　　なお、最後の単語だけを取得したい場合、999など十分に大きな値を与えてもよい。
        /// </param>
        /// <param name="_M_tokenNum_includingAsKeyWord">
        /// 単語連結数M。取得するトークンが何個の単語を連結するか（連結時には区切り文字も含まれる）を示す。
        /// （例：　「e01_ATK・攻撃力＿…」から "攻撃力" だけを取得したい場合、
        /// 1単語なので、M=1）。
        /// 
        /// 　　　　なお、N個から右全てを取得したい場合、999など十分に大きい値でもよい。
        /// Mが1以上なら、文字タイプの条件を満たしている複数の単語が連結される。
        /// 条件を満たしてい単語は連結されない。
        /// </param>
        /// <param name="_keyWordStringCharType">
        /// ※指定なしなら0でもいい。取得するトークンが満たす文字タイプ。この条件を満たす単語だけがトークンとなる。
        /// （例：　「ESE・効果音.e01_system01・決定音＿ピローン_A」から擬音語を示す "ピローン" だけを取得したい場合、
        ///         EStringCharType.t0d_Japanese3_HiraganaOnly＿全角ひらがなまたはカタカナのみ に指定するとよい。
        ///         
        /// 　　　　なお、条件を満たす単語が複数あった場合、Nで指定した開始インデックスから数えて、条件を満たすM個の単語を繋げた文字列を取得する。
        /// </param>
        /// <param name="_isAddingDividedChar_RightOfWord">
        /// 普通はfalse。取りだしたトークンの右側に区切り文字も一緒にくっつけたい場合はtrueにする
        /// （例：　trueの時、「e01_ATK・攻撃力＿…」から "攻撃力＿"を取得できる）
        /// </param>
        /// <param name="_dividedStrings">
        /// 区切り文字の候補。名前に区切ってある"_", "＿", "・"などをコンマ区切りで指定する。
        /// これらのうちいずれの文字が出現しても、区切り文字は一個と数える。</param>
        /// <returns></returns>
        public static string getWord_InString(string _baseString, int _N_beginWordIndex, int _M_tokenNum_includingAsKeyWord, EStringCharType _keyWordStringCharType, bool _isAddingDividedChar_RightOfWord, params string[] _dividedStrings)
        {
            int _indexStart = -1;
            return getWord_InString(_baseString, _N_beginWordIndex, _M_tokenNum_includingAsKeyWord, _keyWordStringCharType, _isAddingDividedChar_RightOfWord, out _indexStart, _dividedStrings);
        }
        /// <summary>
        /// 引数の文字列_baseStringのうち、独自に定義したトークン（１つあるいは複数の単語を連結した文字列）だけを取得します。
        ///   該当する区切り文字が１つも見つからなかった場合は，文字列_baseString が返ります。
        ///   N番目の区切り文字が見つからなかった場合は，最後に見つかった区切り文字より１文字右側をキーの開始位置とします。
        /// 　N+M番目の区切り文字が見つからなかった場合は，N番目から右側全てを含んだ名前を返します。
        /// 　条件を満たす文字タイプの単語が見つからなかった場合は，""が返ります。
        /// 
        /// 　※このメソッドは、
        /// 　　「N番目の区切り文字 ～ N+M番目の区切り文字までの文字列を取ってくるか」、
        /// 　　「トークンの条件（全角カタカナあるいはひらがなで書かれてあるものだけを取得するなど）」、
        /// 　　「区切り文字（"_"や"＿"や"・"など）」
        /// 　　の設定など、取得するキー名を詳細に定義できます。
        /// 　　
        ///       
        ///  　 ■使用例：　_baseString = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        ///  　　　　"攻撃力" = getWord_InString(_baseString, 2, 1, EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・");
        ///  　　　　"ATK・攻撃力" = getWord_InString(_baseString, 1, 2, EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・");
        ///  　　　　"あたえるだめーじにえいきょう" = getWord_InString(_baseString, 999, 1, EStringCharType.t0d_JapaneseHiraganaOnly＿全角ひらがなのみ, "_", "＿", "・");
        /// </summary>
        /// <param name="_baseString">トークンが含まれる、元となる文字列</param>
        /// <param name="_N_beginWordIndex">
        /// トークン開始インデックスN。取得するトークンが何個目の単語から始まるかを示す。
        /// （例：　「e01_ATK・攻撃力＿…」から "攻撃力" だけを取得したい場合、区切り文字が"_", "・", "＿"とすると、
        /// 左から"e01"で0個目→"ATK"で1個目→"攻撃力"で2個目と数えて、N=2）。
        /// 
        /// 　　　なお、最後の単語だけを取得したい場合、999など十分に大きな値を与えてもよい。
        /// </param>
        /// <param name="_M_tokenNum_includingAsKeyWord">
        /// 単語連結数M。取得するトークンが何個の単語を連結するか（連結時には区切り文字も含まれる）を示す。
        /// （例：　「e01_ATK・攻撃力＿…」から "攻撃力" だけを取得したい場合、
        /// 1単語なので、M=1）。
        /// 
        /// 　　　　なお、N個から右全てを取得したい場合、999など十分に大きい値でもよい。
        /// Mが1以上なら、文字タイプの条件を満たしている複数の単語が連結される。
        /// 条件を満たしてい単語は連結されない。
        /// </param>
        /// <param name="_keyWordStringCharType">
        /// ※指定なしなら0でもいい。取得するトークンが満たす文字タイプ。この条件を満たす単語だけがトークンとなる。
        /// （例：　「ESE・効果音.e01_system01・決定音＿ピローン_A」から擬音語を示す "ピローン" だけを取得したい場合、
        ///         EStringCharType.t0d_Japanese3_HiraganaOnly＿全角ひらがなまたはカタカナのみ に指定するとよい。
        ///         
        /// 　　　　なお、条件を満たす単語が複数あった場合、Nで指定した開始インデックスから数えて、条件を満たすM個の単語を繋げた文字列を取得する。
        /// </param>
        /// <param name="_isAddingDividedChar_RightOfWord">
        /// 普通はfalse。取りだしたトークンの右側に区切り文字も一緒にくっつけたい場合はtrueにする
        /// （例：　trueの時、「e01_ATK・攻撃力＿…」から "攻撃力＿"を取得できる）
        /// </param>
        /// <param name="_indexStart">
        /// 省略してもＯＫ。引数にout _indexStartをつけると、
        /// 取得された単語の開始インデックス（例：　_word = _baseString.SubString(■ココ！_indexStart), _word.Length） 
        /// を返します。区切り文字が見つからなかった場合でも、最初のインデックス0を返します。
        /// ※主にgetWords_***メソッドなどで、単語の重複の確認などに使います。
        /// </param>
        /// <param name="_dividedStrings">
        /// 区切り文字の候補。名前に区切ってある"_", "＿", "・"などをコンマ区切りで指定する。
        /// これらのうちいずれの文字が出現しても、区切り文字は一個と数える。</param>
        /// <returns></returns>
        public static string getWord_InString(string _baseString, int _N_beginWordIndex, int _M_tokenNum_includingAsKeyWord, EStringCharType _keyWordStringCharType, bool _isAddingDividedChar_RightOfWord, out int _indexStart, params string[] _dividedStrings)
        //        public static string getEnumKeyName_InDetail<T>(Enum _EnumItem, bool _isTrue_UsingWordIndexNAndM_isFalse_UsingCharStringType, int _N_beginWordIndex, int _M_tokenNum_includingAsKeyWord, EStringCharType _keyWordStringCharType, params string[] _dividedStrings)
        {
            string _word = ""; // どんどん連結していくから、デフォルトは""

            // N番目の区切り文字を探す
            _indexStart = -1;   // 最初が区切り文字だった時0と区別するため、あえて-1を入れてる
            int _indexEnd = -1;     // 最初が区切り文字だった時0と区別するため、あえて-1を入れてる
            int _NOTFOUND = 99999;
            int _indexOf_Min = _NOTFOUND;
            int _indexOf_others = _NOTFOUND;
            int _indexOf_BeforeMin = _NOTFOUND; // 最後に見つかった候補語を検出するため
            string _kouhoGo = ""; // 単語連結の区切り文字
            string _kouhoGo_Before = ""; // ※_indexOfBeforeMin+_kouhoGo_Before.Length-1する時に必要
            // N=0でも、最初の区切り文字だけは調べる
            for (int i = 0; i <= _N_beginWordIndex; i++)
            {
                _indexOf_Min = _NOTFOUND;
                _indexOf_others = _NOTFOUND;
                // 進んだところにもう文字列がなかったら、検索おしまい
                if (_indexStart + 1 < _baseString.Length - 1)
                {
                    // i番目の候補語_dev（例："・"や"_"や"＿"など）を探す。どれか一つでも見つかったら共通して1回とする。
                    // 例：「e1_attack1・攻撃音１＿ズシャン」の場合、
                    //      「e1_」で1回目、「…attack1・」で2回目、「…攻撃音１＿」で3回目の候補語が見つかる
                    foreach (string _dev in _dividedStrings)
                    {
                        // 前の候補語があった場所_indexStartから、候補語文字数だけ進んだところから検索
                        _indexOf_others = _baseString.IndexOf(_dev, _indexEnd + 1);
                        if (_indexOf_others == -1) { _indexOf_others = _NOTFOUND; }
                        // 候補語が見つかったインデックスの中から、より左だった方を採用（ただし，見つからない-1は_NOTFOURNDで十分大きな値となっている）
                        // 候補語が見つかったインデックスの中から、より左だった方を採用（ただし，見つからない-1は_NOTFOURNDで十分大きな値となっている）
                        if (_indexOf_Min > _indexOf_others)
                        {
                            _indexOf_Min = _indexOf_others;
                            _kouhoGo = _dev;
                        }
                    }
                }
                // i番目の候補語が一つも見つからなかった場合（i番目の区切り文字が見つからない）
                if (_indexOf_Min == _NOTFOUND)
                {
                    // 1番目の区切り文字が見つからなかった（_indexOf_BeforeMinに初期値_NOTFOUNDが入っている）
                    if (_indexOf_BeforeMin == _NOTFOUND)
                    {
                        // ■_indexStartを、out用の「キー名の開始位置」に変更
                        _indexStart = 0; // 見つからなかった。ただし返り値として返す時は単語の最初0
                        // 1つも区切り文字が見つからなかったら，エラーとして"要素名"にして、return;
                        //_word = "？" + _baseString;
                        _word = _baseString;
                        return _word; // ■ここの場合、以下の処理（最後の区切り文字を消去するかの処理）は実行しないで終了
                    }
                    else
                    {
                        // Nが9999など十分に大きい値の場合、N番目の区切り文字に到達するまでに見つからなくなった時点で、最後の単語を取得する仕様にしている。
                        // なので、最後に見つかった(N-1)番目区切り文字の「開始位置-1」（※ここだけ違うので注意）を_indexStartとして、break;
                        _indexStart = _indexOf_BeforeMin + (_kouhoGo_Before.Length - 1); //_indexOf_BeforeMin - 1; // ここで、_indexOf_BeforeMin=0の場合、_indexStart==-1の場合があるが、後で必ず+1されるので、0以上になり正常。
                        // _indexEndは、「最後の文字列」を入れる（※最後の区切り文字が存在しないため）
                        _indexEnd = _baseString.Length - 1;

                        int _start = _indexStart + 1;
                        int _length = _indexEnd - _indexStart; // テスト検証済み：区切り文字数も含むので、長さはこれで合ってる！
                        if (_start <= _baseString.Length - 1 && _length <= _baseString.Length)
                        {
                            _word = _baseString.Substring(_start, _length);// テスト済み：_lengthは0でもＯＫ
                        }
                        else
                        {
                            _word = "";
                        }
                        // ■_indexStartを、out用の「キー名の開始位置」に変更
                        _indexStart = _start;
                        return _word; // ■ここの場合、以下の処理（最後の区切り文字を消去するかの処理）は実行しないで終了
                    }
                }
                else
                {
                    //  i番目の候補語が一つは見つかった（i番目の区切り文字が見つかった）
                    // 最初に見つかった区切り文字か（_indexStartが-1から更新されていないか）
                    if (_indexStart == -1)
                    {
                        // 最初に見つかった区切り文字の終了インデックス（0番目終了インデックス）
                        _indexStart = _indexOf_Min + (_kouhoGo.Length - 1); // ここで、_indexStart==-1の可能性はなくなった                    }
                    }
                    else
                    {
                        // N番目ではなく、(N-1)番目に見つかった区切り文字の終了インデックスを取得（(N-1)番目終了インデックス）
                        _indexStart = _indexOf_BeforeMin + (_kouhoGo_Before.Length - 1);
                    }
                    // N番目に見つかった区切り文字の終了インデックスは_indexEndに格納（N番目終了インデックス）
                    _indexEnd = _indexOf_Min + (_kouhoGo.Length - 1);
                    // (i-1)番目の候補語の位置を更新（はじめは_NOTFOUND=99999）
                    _indexOf_BeforeMin = _indexOf_Min;
                    _kouhoGo_Before = _kouhoGo;
                    string _indexStartSubs = _baseString.Substring(_indexStart + 1); // -1の場合もあるので注意
                    string _indexEndSubs = _baseString.Substring(_indexEnd + 1);
                    string _indexBeforeSubs = _baseString.Substring(_indexOf_BeforeMin + 1); // -1の場合もあるので注意
                }
            }
            // ここまでで、現在_indexStartには、
            // (a)N=0の場合、最初に見つかった区切り文字の0番目終了インデックス、
            // (b)N>0の場合、(N-1)番目に見つかった区切り文字の終了インデックス、
            // (c)もしくは1つも見つからなかった(_NOTFOUND)、
            // のどれかが入っている。

            // 区切り文字が1つでも見つかったか
            if (_indexStart != _NOTFOUND)
            {
                // 値を_NOTFOUNDじゃないように範囲調整し、わかりやすいように改名
                int _wordStart・Ｎマイナス１個目終了 = _indexStart;// Math.Min(_indexStart, _baseString.Length - 1 - 1); // ここが-1-1なのは、あとでほとんどの場合+1するから
                int _wordEnd・Ｎ個目終了 = _indexEnd; //Math.Min(_indexEnd, _baseString.Length - 1 - 1);
                string _kugirimozi・Ｎ個目の区切り文字 = _kouhoGo;

                // 以下、テスト用の文字列（デバッグ確認用に便利）。Length-1を超えてもエラーにはならず""になる
                string _indexStartSubs = _baseString.Substring(_wordStart・Ｎマイナス１個目終了 + 1); // -1の場合もあるので注意
                string _indexEndSubs = _baseString.Substring(_wordEnd・Ｎ個目終了 + 1);

                // N番目に見つかった右のトークンから、_tokenNum=M個のトークンをまとめて、キー名とする。
                if (_N_beginWordIndex == 0 && _M_tokenNum_includingAsKeyWord == 1)
                {
                    // N=0、M=1の時、文字列の冒頭～1番目に見つかった区切り文字までを、キー名とするだけでいい。
                    _word = _baseString.Substring(0, Math.Min(_wordEnd・Ｎ個目終了 + 1, _baseString.Length));
                    // ■_indexStartを、out用の「キー名の開始位置」に変更
                    _indexStart = 0;
                }
                else if (_M_tokenNum_includingAsKeyWord == 1)
                {
                    // N>0, M=1の時、(N-1)番目に見つかった区切り文字の終了位置の右～N番目に見つかった区切り文字までを、キー名とするだけでいい。
                    int _start = _wordStart・Ｎマイナス１個目終了 + 1;
                    int _length = _wordEnd・Ｎ個目終了 - _wordStart・Ｎマイナス１個目終了; // テスト検証済み：区切り文字数も含むので、長さはこれで合ってる！
                    if(_start <= _baseString.Length - 1 && _length <= _baseString.Length)
                    {
                        _word = _baseString.Substring(_start, _length);// テスト済み：_lengthは0でもＯＫ
                    }else{
                        _word = "";
                    }
                    // ■_indexStartを、out用の「キー名の開始位置」に変更
                    _indexStart = _start;
                }
                else if (_M_tokenNum_includingAsKeyWord == 0 || _M_tokenNum_includingAsKeyWord >= 9999)
                {
                    // Mが0の時、もしくは十分大きい時は、
                    // N番目に見つかった候補語の右から、終点まで右部全部含める
                    _word = _baseString.Substring(_wordEnd・Ｎ個目終了 + 1);
                    // ■_indexStartを、out用の「キー名の開始位置」に変更
                    _indexStart = _wordEnd・Ｎ個目終了 + 1;

                }
                else
                {
                    // Mが1以上9999未満の時、_indexStartの右から「一時単語_tempWord+区切り文字_kouhoGo」の並びをM回検索し、
                    // それらM個を連結して、キーとする。
                    string _tempWord = "";
                    // N+i個目に見つかった区切り文字のインデックスは、_indexEndで管理する。
                    int _indexTempWordEnd = _wordEnd・Ｎ個目終了; // ここでは、もう既に_indexStartが0以上のことが保証されている
                    // ■_indexStartを、out用の「キー名の開始位置」に変更
                    _indexStart = _wordEnd・Ｎ個目終了 + 1;

                    for (int i = 1; i <= _M_tokenNum_includingAsKeyWord; i++)
                    {
                        _indexOf_Min = _NOTFOUND;
                        _indexOf_others = _NOTFOUND;
                        _kouhoGo = "";
                        // 進んだところにもう文字列がなかったら、検索おしまい
                        if (_indexTempWordEnd + 1 < _baseString.Length - 1)
                        {
                            // N+M番目の候補語_dev（例：" "や"_"や"＿"など）を探す
                            foreach (string _dev in _dividedStrings)
                            {
                                // 前に見つかった候補語から、候補語文字数だけ進んだところから検索
                                _indexOf_others = _baseString.IndexOf(_dev, _indexTempWordEnd + 1);
                                if (_indexOf_others == -1) { _indexOf_others = _NOTFOUND; }
                                // 候補語が見つかったインデックスの中から、より左だった方を採用（ただし，見つからない-1は_NOTFOURNDで十分大きな値となっている）
                                if (_indexOf_Min > _indexOf_others)
                                {
                                    _indexOf_Min = _indexOf_others;
                                    _kouhoGo = _dev;
                                }
                            }
                        }
                        // N+i番目の候補語が見つかったか
                        if (_indexOf_Min != _NOTFOUND)
                        {

                            // N+(i-1)番目～N+i番目にある単語を一時的に格納
                            int _start = _indexTempWordEnd + 1;
                            if (_baseString.Length > _start && _indexOf_Min - _start > 1)
                            {
                                _tempWord = _baseString.Substring(_start, _indexOf_Min - _start);
                            }
                            else
                            {
                                _tempWord = "";
                            }

                            // ここで、できるだけ文字タイプを満たすように、文字列を変換しておく（例：英語は全角も混じってる場合はできるだけ半角に変換し、日本語は半角英数字記号も全角にできるだけなおす）。
                            //※全角と半角の区別をつかなくしたら、文字タイプで識別している意味がないため、現実装ではこういうことはしない _tempWord = getParsedString(_tempWord, _keyWordStringCharType);

                            // 単語が文字タイプの条件を満たすか
                            if (isEStringCharType(_tempWord, _keyWordStringCharType) == false)
                            {
                                // 満たしていなかったら、この単語を飛ばす（キー名のインデックスを１つずらす）
                                _wordEnd・Ｎ個目終了 = _indexOf_Min + (_kouhoGo.Length - 1); // キー名の開始位置を１つずらす
                            }
                            else
                            {
                                // 【キー名の連結】(a-1)１単語ずつ作っていく場合
                                // 満たしていれば、キー名として連結
                                _word += _tempWord + _kouhoGo; // 候補語を連結の区切り文字として含める
                            }
                        }
                        else
                        {
                            // N+M番目の候補語が見つからなかったら，開始から終点まで
                            // N番目の区切り文字を「含めずに」、N番目の区切り文字より右から終点まで右部全部含める
                            _word = _baseString.Substring(Math.Min(_wordEnd・Ｎ個目終了 + 1, _baseString.Length - 1));
                            // 以下は、草案
                            // N番目の区切り文字を「含めて」、N番目の区切り文字「開始位置」から終点まで右部全部含める
                            //_word = _baseString.Substring(Math.Max(0, _wordEnd・Ｎ個目終了 - (_kugirimozi・Ｎ個目の区切り文字.Length - 1)));
                            break; // もう見つからない
                        }

                        // 次の候補語を探す
                        _indexTempWordEnd = _indexOf_Min + (_kouhoGo.Length - 1);
                    }
                    // キー名の連結完了！
                    // 以下、連結せずに、一気に取る場合の草案メモ
                    // 【キー名の連結】(b)一気にトークンを作る場合
                    //if (_indexTempWordEnd != _NOTFOUND)
                    //{
                    //    // 連結語などの途中の候補語は含めるが、開始１個手前・終点１個後の候補語は含めない
                    //    _item_includingWaitWord = _baseString.Substring(_indexStart + 1, _indexTempWordEnd - (_indexStart + 1));
                    //}
                }
                // とりあえず、これで_wordに「"キー名"+区切り文字」、が入った（「キー名」だけのやつはreturn _wordでもう抜けてる）
                
                // 最後に、_wordに付く区切り文字を消すか
                if (_isAddingDividedChar_RightOfWord == false)
                {
                    // 【キー名の連結】(a-2)１単語ずつ作っていく場合、最後の区切り文字を消す（例："ATK・攻撃力_" → "ATK・攻撃力"）
                    int _removeLastKouhoIndex = -1;
                    if (_kouhoGo != "")
                    {
                        _removeLastKouhoIndex = _word.LastIndexOf(_kouhoGo);
                    }
                    if (_removeLastKouhoIndex == -1) _removeLastKouhoIndex = _word.Length;
                    if (_removeLastKouhoIndex > -1)
                    {
                        _word = _word.Substring(0, Math.Max(_removeLastKouhoIndex, 0)); // 区切り文字だけの場合、""になるのもあり
                    }
                }
            }
            return _word;
        }
        /// <summary>
        /// 引数の文字列_baseStringのうち、独自に定義したトークン（１つあるいは複数の単語を連結した文字列）を全てリストで取得します。
        ///   該当する区切り文字が１つも見つからなかった場合は，[0]=_baseString の要素数1のリストが返ります。
        ///   N番目の区切り文字が見つからなかった場合は，最後に見つかった区切り文字より１文字右側をキーの開始位置とします。
        /// 　N+(i*M)番目の区切り文字が見つからなかった場合は，N番目から右側全てを含んだ名前を返します。
        /// 　条件を満たす文字タイプの単語が見つからなかった場合は，長さ0のリストが返ります。
        /// 
        /// 　※このメソッドは、
        /// 　　「N番目の区切り文字 ～ N+M番目の区切り文字までの文字列を取ってくるか」、
        /// 　　「トークンの条件（全角カタカナあるいはひらがなで書かれてあるものだけを取得するなど）」、
        /// 　　「区切り文字（"_"や"＿"や"・"など）」
        /// 　　の設定など、取得するキー名を詳細に定義できます。
        /// 　　
        ///       
        ///  　 ■使用例：　_baseString = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        ///  　　　　getWords_InString(_baseString, 0, 1, EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・")　   →   [0]="e01", [1]="ATK", [2]="攻撃力", [3]="あたえるだめーじのえいきょう"                    
        ///  　　　　getWord_InString(_baseString, 1, 2, EStringCharType.t0_None_未定義_何でもＯＫ, "_", "＿", "・")　    →   [0]="ATK・攻撃力", [1]="あたえるだめーじのえいきょう"
        /// </summary>
        /// <param name="_baseString">トークンが含まれる、元となる文字列</param>
        /// <param name="_N_beginWordIndex">
        /// トークン開始インデックスN。取得するトークンが何個目の単語から始まるかを示す。
        /// （例：　「e01_ATK・攻撃力＿…」から "攻撃力" 以降だけを取得したい場合、区切り文字が"_", "・", "＿"とすると、
        /// 左から"e01"で0個目→"ATK"で1個目→"攻撃力"で2個目と数えて、N=2）。
        /// 
        /// 　　　なお、最後の単語だけを取得したい場合、999など十分に大きな値を与えてもよい。
        /// </param>
        /// <param name="_M_tokenNum_includingAsKeyWord">
        /// 単語連結数M。取得するトークンが何個の単語を連結するか（連結時には区切り文字も含まれる）を示す。
        /// （例：　「e01_ATK・攻撃力＿あたえるだめーじのえいきょう」から "攻撃力＿あたえるだめーじのえいきょう" だけを取得したい場合、
        /// 2単語なので、M=2）。
        /// 
        /// 　　　　なお、N個から右全てを取得したい場合、999など十分に大きい値でもよい。
        /// Mが1以上なら、文字タイプの条件を満たしている複数の単語が連結される。
        /// 条件を満たしてい単語は連結されない。
        /// </param>
        /// <param name="_keyWordStringCharType">
        /// ※指定なしなら0でもいい。取得するトークンが満たす文字タイプ。この条件を満たす単語だけがトークンとなる。
        /// （例：　「ESE・効果音.e01_system01・決定音＿ピローン_A」から擬音語を示す "ピローン" だけを取得したい場合、
        ///         EStringCharType.t0d_Japanese3_HiraganaOnly＿全角ひらがなまたはカタカナのみ に指定するとよい。
        ///         
        /// 　　　　なお、条件を満たす単語が複数あった場合、Nで指定した開始インデックスから数えて、条件を満たすM個の単語を繋げた文字列を取得する。
        /// </param>
        /// <param name="_isAddingDividedChar_RightOfWord">
        /// 普通はfalse。取りだしたトークンの右側に区切り文字も一緒にくっつけたい場合はtrueにする
        /// （例：　trueの時、「e01_ATK・攻撃力＿…」から "攻撃力＿"を取得できる）
        /// </param>
        /// <param name="_dividedStrings">
        /// 区切り文字の候補。名前に区切ってある"_", "＿", "・"などをコンマ区切りで指定する。
        /// これらのうちいずれの文字が出現しても、区切り文字は一個と数える。</param>
        /// <returns></returns>
        public static List<string> getWords_InString(string _baseString, int _N_beginWordIndex, int _M_tokenNum_includingAsKeyWord, EStringCharType _keyWordStringCharType, bool _isAddingDividedChar_RightOfWord, params string[] _dividedStrings)
        {
            List<string> _words = new List<string>();
            int i = 0;
            string _nowWord = "";
            int _indexStart = 0; // 検出した単語の開始インデックス
            int _beforeIndexStart = -1; // iを移動させても最後は同じ単語が取得されるため、それの検出に使う。あと、初回は_indexStartとかぶってはいけないから-1
            while (i < 100)
            {
                // i番目の単語を取得
                _nowWord = getWord_InString(_baseString, _N_beginWordIndex + (i * _M_tokenNum_includingAsKeyWord), _M_tokenNum_includingAsKeyWord, _keyWordStringCharType, _isAddingDividedChar_RightOfWord, out _indexStart, _dividedStrings);
                // これじゃあ、同じ単語続きの時、勝手に終わっちゃう　→　×iを変えても最後は同じ単語が入ることを利用して、終了
                // これじゃあ、最後に同じ単語が実際に連続するときと、同じ開始インデックスの単語を取る時と区別がつかない　→　×とりあえず全部入れて、最後だけ同じ単語が入ってたら消す
                // ●""じゃなく、かつ開始インデックスが前のものと異なるなら、違う単語と認識して追加
                if (_nowWord != "" && _indexStart != _beforeIndexStart)
                {
                    // 単語を追加
                    _words.Add(_nowWord);
                }
                else if (_indexStart == _beforeIndexStart)
                {
                    // iを変更しても同じインデックスの単語が取られたら、終了の合図
                    break;
                }
                // 前のインデックスを記憶
                _beforeIndexStart = _indexStart;
                i++;
            }
            return _words;
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素名で，全てのトークン（"_"や"＿"や"・"で区切られた文字列）をリストで取得します．
        /// "_"や"＿"や"・"が一つもが無い場合は，要素の名前をそのまま[0]に取得します．
        /// 
        ///  　 ■使用例：　_EnumItem = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        ///  　      _words = getEnumKeyName_Words(_EnumItem);
        ///  　      _words[0] = "e01";
        ///  　      _words[1] = "ATK";
        ///  　      _words[2] = "攻撃力";
        ///  　      _words[3] = "あたえるだめーじにえいきょう";
        /// </summary>
        /// <typeparam name="T">Enum列挙体のクラス名を書く。（例: EKeyCode、EPara、ESE・効果音 など）</typeparam>
        /// <param name="_EnumItem">Enum列挙体の要素（例：EKeyCode.a、EPara.***、ESE・効果音.***など）</param>
        /// <returns></returns>
        public static List<string> getEnumKeyName_Words(Enum _EnumItem)
        {
            List<string> _words = new List<string>();
            string _enumString = MyTools.getEnumName(_EnumItem);
            int i = 0;
            string _nowWord = "";
            int _indexStart = 0; // 検出した単語の開始インデックス
            int _beforeIndexStart = -1; // iを移動させても最後は同じ単語が取得されるため、それの検出に使う。あと、初回は_indexStartとかぶってはいけないから-1
            while (i < 100)
            {
                // i番目の単語を取得
                _nowWord = getWord_InString(_enumString, i, 1, EStringCharType.t0_None_未定義_何でもＯＫ, false, out _indexStart , "_", "＿", "・");
                // ●""じゃなく、かつ開始インデックスが前のものと異なるなら、違う単語と認識して追加
                if (_nowWord == "" || _indexStart == _beforeIndexStart) break;
                // 単語を追加
                _words.Add(_nowWord);
                // 前のインデックスを記憶
                _beforeIndexStart = _indexStart;
                i++;
            }
            return _words;
        }
        /// <summary>
        /// Enum型（独自の列挙型）の要素名で，「e01」などの検索のために使われるインデックストークン（最初に出現する区切り文字"_"や"＿"や"・"より左の部分）を除いた，
        /// 全てのトークンをリストで取得します．
        /// "_"や"＿"や"・"が一つもが無い場合は，要素の名前をそのまま[0]に取得します．
        /// 
        ///  　 ■使用例：　_EnumItem = EPara.e01_ATK・攻撃力＿あたえるだめーじにえいきょう;
        ///  　      _words = getEnumKeyName_Words(_EnumItem);
        ///  　      _words[0] = "ATK";
        ///  　      _words[1] = "攻撃力";
        ///  　      _words[2] = "あたえるだめーじにえいきょう";
        /// </summary>
        /// <typeparam name="T">Enum列挙体のクラス名を書く。（例: EKeyCode、EPara、ESE・効果音 など）</typeparam>
        /// <param name="_EnumItem">Enum列挙体の要素（例：EKeyCode.a、EPara.***、ESE・効果音.***など）</param>
        /// <returns></returns>
        public static List<string> getEnumKeyName_Words_NotIncludingFirstIndex(Enum _EnumItem)
        {
            List<string> _words = new List<string>();
            string _enumString = MyTools.getEnumName(_EnumItem);
            int i = 1; // ここが0じゃないのが、NotIncludingFirstIndexの特徴
            string _nowWord = "";
            int _indexStart = 0; // 検出した単語の開始インデックス
            int _beforeIndexStart = -1; // iを移動させても最後は同じ単語が取得されるため、それの検出に使う。あと、初回は_indexStartとかぶってはいけないから-1
            while (i < 100)
            {
                // i番目の単語を取得
                _nowWord = getWord_InString(_enumString, i, 1, EStringCharType.t0_None_未定義_何でもＯＫ, false, out _indexStart, "_", "＿", "・");
                // ●""じゃなく、かつ開始インデックスが前のものと異なるなら、違う単語と認識して追加
                if (_nowWord == "" || _indexStart == _beforeIndexStart) break;
                // 単語を追加
                _words.Add(_nowWord);
                // 前のインデックスを記憶
                _beforeIndexStart = _indexStart;
                i++;
            }
            return _words;
        }

        /// <summary>
        /// 第一引数の文字列から、第二引数以降の候補語を検索し、そのどれかが最初に見つかったら、その単語を返します。
        /// 見つからない場合は、""を返します。
        /// </summary>
        public static string getWord_First(string _baseString, params string[] _searchingWords)
        {
            int _indexOf_Min = -1; // デフォルトでは見つかっていないことを示す-1         
            return getWord_First(_baseString, out _indexOf_Min, _searchingWords);
        }
        /// <summary>
        /// 第一引数の文字列から、第二引数以降の候補語を検索し、そのどれかが最初に見つかったら、その単語を返します。
        /// 見つからない場合は、""を返します。
        /// </summary>
        public static string getWord_First(string _baseString, out int _indexOf_Min, params string[] _searchingWords)
        {
            _indexOf_Min = -1; // デフォルトでは見つかっていないことを示す-1         
            string _word = ""; // もらったトークン
            if (_baseString == null) return _word;
            int _NOTFOUND = 9999; // 十分大きな値を代入して、最小のインデックスを見つける
            int _indexOf_others = _NOTFOUND;
            _indexOf_Min = _NOTFOUND;
            // N+M番目の候補語_dev（例：" "や"_"や"＿"など）を探す
            foreach (string _str in _searchingWords)
            {
                _indexOf_others = _baseString.IndexOf(_str);
                if (_indexOf_others == -1) { _indexOf_others = _NOTFOUND; }
                // 候補語が見つかったインデックスの中から、より左だった方を採用（ただし，見つからない-1は_NOTFOURNDで十分大きな値となっている）
                if (_indexOf_Min > _indexOf_others)
                {
                    _indexOf_Min = _indexOf_others;
                    _word = _str;
                }
            }
            return _word;
        }
        /// <summary>
        /// 第一引数の文字列の最後尾から、第二引数以降の候補語を検索し、そのどれかが最初に見つかったら、その候補語を返します。
        /// 見つからない場合は、""を返します。
        /// </summary>
        public static string getWord_Last(string _baseString, params string[] _searchingWords)
        {
            int _indexOf_Min = -1; // デフォルトでは見つかっていないことを示す-1         
            return getWord_Last(_baseString, out _indexOf_Min, _searchingWords);
        }
        /// <summary>
        /// 第一引数の文字列から、第二引数以降の検索候補語のいずれか一つが最初に見つかった時、そのインデックスとトークンを返します。
        /// </summary>
        public static string getWord_Last(string _baseString, out int _lastIndexOf, params string[] _searchingWords)
        {
            _lastIndexOf = -1; // デフォルトでは見つかっていないことを示す-1         
            string _word = ""; // もらったトークン
            if (_baseString == null) return _word;
            int _NOTFOUND = 9999; // 十分大きな値を代入して、最小のインデックスを見つける
            int _indexOf_others = _NOTFOUND;
            _lastIndexOf = _NOTFOUND;
            // N+M番目の候補語_dev（例：" "や"_"や"＿"など）を探す
            foreach (string _str in _searchingWords)
            {
                _indexOf_others = _baseString.LastIndexOf(_str);
                if (_indexOf_others == -1) { _indexOf_others = _NOTFOUND; }
                // 候補語が見つかったインデックスの中から、より左だった方を採用（ただし，見つからない-1は_NOTFOURNDで十分大きな値となっている）
                if (_lastIndexOf > _indexOf_others)
                {
                    _lastIndexOf = _indexOf_others;
                    _word = _str;
                }
            }
            return _word;
        }


        ///// <summary>
        ///// Enum型（独自の列挙型）の要素の名前で，全てのキートークンと定義した部分だけ（N番目の"_"や"＿"より右～N+1番目の"_"や"＿"より左の部分までの文字列）をリストで取得します．
        ///// "_"や"＿"が一つもが無い場合は，要素の名前をそのまま取得します．
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="_EnumType"></param>
        ///// <returns></returns>
        //public static List<string> getEnumKeyNames<T>()
        //{
        //    if (typeof(T).IsEnum == false)
        //    {
        //        return new List<string>();
        //    }
        //    else
        //    {
        //        List<string> _tokenNames = getEnumNameList<T>();
        //        T _EnumItem;
        //        string _keytoken;
        //        for (int i = 0; i < _tokenNames.Count; i++)
        //        {
        //            _EnumItem = getEnumItem_FromString<T>(_tokenNames[i]);
        //            _keytoken = getEnumKeyName_RightOfUnderBar_All<T>(_EnumItem);
        //            _tokenNames.Add(_keytoken.ToString());
        //        }
        //        return _tokenNames;
        //    }
        //}
        ///// <summary>
        ///// Enum型（独自の列挙型）の要素の名
        ///// 前のキーと定義した文字列（最初の"_"や"＿"に囲まれた，つまり最初に出てきた"_"や"＿"より右の部分～次に出てきた"_"や"＿"より左の部分）を取得します．
        ///// "_"や"＿"がない場合は，用損名前をそのまま取得します．
        ///// （例：　enumの要素の名前が，「e01＿攻撃量」のとき，"＿"より右側の「攻撃量」の文字列を取得）
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="_EnumType"></param>
        ///// <returns></returns>
        //public static string getEnumKeyName_InDetail<T>(Enum _EnumItem)
        //{
        //    string _enumName = getEnumName(_EnumItem);

        //    // 一番目の_や＿を探す
        //    int _lastIndexOf = _enumName.IndexOf("_");
        //    if (_lastIndexOf == -1) { _lastIndexOf += 1000; }
        //    int _indexOf_others = _enumName.IndexOf("＿");
        //    if (_indexOf_others == -1) { _indexOf_others += 1000; }

        //    // _や＿でより左だった方を採用（ただし，見つからない-1は999となっている）
        //    int _indexStart = Math.Min(_lastIndexOf, _indexOf_others);
        //    if (_indexStart == 999)
        //    {
        //        // _や＿が1つも見つからなかったら，そのまま
        //        //_item_includingWaitWord = _item_includingWaitWord;
        //    }
        //    else
        //    {
        //        // 二番目の_や＿まで．
        //        _lastIndexOf = _enumName.IndexOf("_", _indexStart + 1);
        //        if (_lastIndexOf == -1) { _lastIndexOf += 1000; }
        //        _indexOf_others = _enumName.IndexOf("＿", _indexStart + 1);
        //        if (_indexOf_others == -1) { _indexOf_others += 1000; }
        //        int _indexTempWordEnd = Math.Min(_lastIndexOf, _indexOf_others);

        //        if (_indexTempWordEnd != 999)
        //        {
        //            _enumName = _enumName.Substring(_indexStart + 1, _indexTempWordEnd - (_indexStart + 1)); //_や＿は含めない
        //        }
        //        else
        //        {
        //            // 二番目の_や__が見つからなかったら，終点まで
        //            _enumName = _enumName.Substring(_indexStart + 1);
        //        }
        //    }

        //    return _enumName;
        //}
        #endregion
        #region 要素の格納している値（int型）からEnum型（独自の列挙型）を取ってくる: getEnumItem_FromIndexOrValue
        /// <summary>
        /// Enum要素が格納している値（もしくは「要素名=値,」で割り当てていない場合はインデックスを意味する）から、
        /// Enum型（独自の列挙型）の要素を取ってきます．全ての要素配列Arrayを生成するため、ちょっと手間がかかります。
        /// 
        /// ※foreach等を使って高速にEnum全要素を扱いたい場合は、getEnumItems＜列挙体名＞を使ってください。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        public static T getEnumItem_FromIndexOrValue<T>(int _enumIndexOrValue_int_parsed)
        {
            T _enumItem = default(T); 
            if (typeof(T).IsEnum == false)
            {
                return _enumItem; // タイプTのデフォルト値（値型なら0、string型やクラス型ならnull）
            }
            // 全ての要素配列Arrayを生成するため、ちょっと手間がかかります。
            Array _typeValues = Enum.GetValues(typeof(T));
            int _typeNum = _typeValues.Length;
            for (int i = 0; i < _typeNum; i++)
            {
                if ((int)_typeValues.GetValue(i) == _enumIndexOrValue_int_parsed)
                {
                    _enumItem = (T)_typeValues.GetValue(i);
                    break;
                }

            }
            return _enumItem;
        }
        /// <summary>
        /// Enum要素が格納しているインデックスもしくは値（int型）から、Enum型（独自の列挙型）の要素を取ってきます．
        /// 値を調べる範囲を指定することで，高速化できます．
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        /// <returns></returns>
        public static T getEnumItem_FromIndexOrValue<T>(int _enumIndexOrValue_int_parsed, int _startInt, int _endInt)
        {
            T _type = default(T);
            Array _typeValues = Enum.GetValues(typeof(T));
            int _typeNum = Math.Min(_typeValues.Length, _endInt);
            for (int i = _startInt; i < _typeNum; i++)
            {
                if ((int)_typeValues.GetValue(i) == _enumIndexOrValue_int_parsed)
                {
                    _type = (T)_typeValues.GetValue(i);
                    break;
                }

            }
            return _type;
        }

        #endregion
        #region 要素名（string型）からEnum型（独自の列挙型）を取ってくる: getEnumItem_FromString
        /// <summary>
        /// 要素の名前（ToString()から取れる文字列）からEnum型（独自の列挙型）を取ってきます．見つからなかった場合はdefault(T)、つまり最初の要素を返します。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        public static T getEnumItem_FromString<T>(string _enumName)
        {
            T _type = default(T);
            Array _typeValues = Enum.GetValues(typeof(T));
            int _typeNum = _typeValues.Length;
            for (int i = 0; i < _typeNum; i++)
            {
                if (_typeValues.GetValue(i).ToString() == _enumName)
                {
                    _type = (T)_typeValues.GetValue(i);
                    break;
                }

            }
            return _type;
        }
        /// <summary>
        /// 要素のインデックス（int型）からEnum型（独自の列挙型）を取ってきます．インデックスの調べる範囲を指定することで，高速化できます．見つからなかった場合はdefault(T)、つまり最初の要素を返します。
        /// </summary>
        /// <typeparam name="T">列挙体名（例：　EStringCharType）</typeparam>
        public static T getEnumItem_FromString<T>(string _enumString, int _startIndex, int _endIndex)
        {
            T _type = default(T);
            Array _typeValues = Enum.GetValues(typeof(T));
            int _typeNum = Math.Min(_typeValues.Length, _endIndex);
            for (int i = _startIndex; i < _typeNum; i++)
            {
                if (_typeValues.GetValue(i).ToString() == _enumString)
                {
                    _type = (T)_typeValues.GetValue(i);
                    break;
                }

            }
            return _type;
        }

        #endregion


        // 文字列関連処理
        #region 全角か半角かのチェック IsHankaku / IsZenkaku
        /// <summary>
        /// 全て半角文字であればtrueを返します。　「!IsZenkakuChar_Including(str)」と同値です。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsHankaku(string str)
        {
            // Not(全角文字が少なくとも一つは入っている) → 全角が一つも入っていない → 全てが半角 で取得できなくもない
            //bool _isHankaku = !IsZenkakuChar_Including(str);

            // バイト数を取得
            int _textByte = Encoding.GetEncoding("Shift_JIS").GetByteCount(str);
            // バイト数が文字数に等しいか
            return _textByte == str.Length;
        }
        /// <summary>
        /// 半角文字であればtrueを返します。
        /// </summary>
        public static bool IsHankaku(char _char)
        {
            string _charString = _char.ToString();
            // バイト数を取得
            int _textByte = Encoding.GetEncoding("Shift_JIS").GetByteCount(_charString);
            // バイト数が1か
            return _textByte == 1;
        }
        /// <summary>
        /// 全角文字であればtrueを返します。
        /// </summary>
        public static bool IsZenkaku(char _char)
        {
            string _charString = _char.ToString();
            // バイト数を取得
            int _textByte = Encoding.GetEncoding("Shift_JIS").GetByteCount(_charString);
            // バイト数が2か
            bool _isZenkaku = (_textByte == 2);
            return _isZenkaku;
        }
        /// <summary>
        /// 引数の文字列に、１文字でも半角カタカナが入っていればtrueを返します。
        /// </summary>
        public static bool IsHankakuKatakana_Including(string _string)
        {
            if (_string == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }
            foreach (char c in _string)
            {
                if (IsKatakana_Hankaku(c) == true)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 引数の文字列に、１文字でも全角文字が入っていればtrueを返します。 「!IsHankaku(str)」と同値です。
        /// </summary>
        public static bool IsZenkakuChar_Including(string _string)
        {
            // バイト数を取得
            int _textByte = Encoding.GetEncoding("Shift_JIS").GetByteCount(_string);
            // 文字数×2がバイト数と等しくなければ、全角文字が入っている証拠
            bool _isZenkakuIncluding = (_string.Length != _textByte);
            return _isZenkakuIncluding;
        }
        // 全角チェックの参考 http://codezine.jp/article/detail/1083?p=2
        /// <summary>
        /// 引数の文字列が、全て全角文字であればtrueを返します。  ※「!IsHankaku(str)」とは同値では無いので注意してください（IsHankaku(str) == falseは、一つでも全角文字が入っていることしか検出していない）
        /// </summary>
        public static bool IsZenkaku(string str)
        {
            // 改行など、全角文字にも含まれる可能性がある半角文字の特殊記号を消去
            string _string = str.Replace("\r\n", "");
            _string = str.Replace("\n", "");
            _string = str.Replace("\t", "");
            _string = str.Replace("\r", "");
            // 文字数を取得
            int _textLength = _string.Length;
            // バイト数を取得
            int _textByte = Encoding.GetEncoding("Shift_JIS").GetByteCount(_string);
            // 文字数×2がバイト数と等しいか
            bool _isZenkaku = (_textLength * 2 == _textByte);
            return _isZenkaku;
        }
        #endregion

        #region １文字のchar型またはstring型の文字列の種類を調べる Is***(char c) / Is***(string str)
        /// <summary>
        /// 文字がアルファベットならtrueを返します。全角・半角かは問いません。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlfabet(char c)
        {
            return char.IsLetter(c);
        }
        /// <summary>
        /// 文字がアルファベットか数字ならtrueを返します。全角・半角かは問いません。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlfabetOrNumber(char c)
        {
            return char.IsLetterOrDigit(c);
        }
        /// <summary>
        /// 文字が全角アルファベットか全角数字ならtrueを返します。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlfabetOrNumber_Zenkaku(char c)
        {
            return ('Ａ' <= c && c <= 'Ｚ') || ('ａ' <= c && c <= 'ｚ')
                || ('０' <= c && c <= '９');
        }
        /// <summary>
        /// 文字が半角アルファベットか半角数字ならtrueを返します。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlfabetOrNumber_Hankaku(char c)
        {
            return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z')
                || ('0' <= c && c <= '9');
        }

        /// <summary>
        /// 指定した Unicode 文字が、英字の大文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabetUpper_Hankaku(char c)
        {
            //半角英字と全角英字の大文字の時はTrue
            return ('A' <= c && c <= 'Z');
        }
        /// <summary>
        /// 指定した Unicode 文字が、英字の小文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の小文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabetLower_Hankaku(char c)
        {
            //半角英字と全角英字の小文字の時はTrue
            return ('a' <= c && c <= 'z');
        }
        /// <summary>
        /// 指定した Unicode 文字が、英字の大文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabetUpper_Zenkaku(char c)
        {
            //半角英字と全角英字の大文字の時はTrue
            return ('Ａ' <= c && c <= 'Ｚ');
        }

        /// <summary>
        /// 指定した Unicode 文字が、英字の小文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の小文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabetLower_Zenkaku(char c)
        {
            //半角英字と全角英字の小文字の時はTrue
            return ('ａ' <= c && c <= 'ｚ');
        }
        /// <summary>
        /// 指定した Unicode 文字が、半角英字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabet_Hankaku(char c)
        {
            //半角英字と全角英字の大文字の時はTrue
            return ('A' <= c && c <= 'z');
        }
        /// <summary>
        /// 指定した Unicode 文字が、全角英字の大文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabet_Zenkaku(char c)
        {
            //半角英字と全角英字の大文字の時はTrue
            return ('Ａ' <= c && c <= 'ｚ');
        }

        /// <summary>
        /// 指定した Unicode 文字が、半角の 0 から 9 までの数字（アラビア数字のみ）かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が数字である場合は true。それ以外の場合は false。</returns>
        public static bool IsNumber_Hankaku(char c)
        {
            return '0' <= c && c <= '9';
        }
        /// <summary>
        /// 指定した Unicode 文字が、全角の ０ から ９ までの数字（アラビア数字のみ）かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が数字である場合は true。それ以外の場合は false。</returns>
        public static bool IsNumber_Zenkaku(char c)
        {
            return '０' <= c && c <= '９';
        }

        /// <summary>
        /// 指定した Unicode 文字が、ひらがなかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c がひらがなである場合は true。それ以外の場合は false。</returns>
        public static bool IsHiragana(char c)
        {
            //「ぁ」～「より」までと、「ー」「ダブルハイフン」をひらがなとする
            return ('\u3041' <= c && c <= '\u309F')
                || c == '\u30FC' || c == '\u30A0';
        }
        /// <summary>
        /// 指定した Unicode 文字が、ひらがなあるいは全角カタカナだけで構成されているかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c がひらがなである場合は true。それ以外の場合は false。</returns>
        public static bool IsHiraganaOrZenkakuKatakana(char c)
        {
            return IsHiragana(c) || IsKatakana_Zenkaku(c);
        }
        /// <summary>
        /// 指定した Unicode 文字が、全角カタカナかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が全角カタカナである場合は true。それ以外の場合は false。</returns>
        public static bool IsKatakana_Zenkaku(char c)
        {
            //「ダブルハイフン」から「コト」までと、カタカナフリガナ拡張と、
            //濁点と半濁点を全角カタカナとする
            //中点と長音記号も含む
            return ('\u30A0' <= c && c <= '\u30FF')
                || ('\u31F0' <= c && c <= '\u31FF')
                || ('\u3099' <= c && c <= '\u309C');
        }
        /// <summary>
        /// 指定した Unicode 文字が、半角カタカナかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が半角カタカナである場合は true。それ以外の場合は false。</returns>
        public static bool IsKatakana_Hankaku(char c)
        {
            //「･」から「ﾟ」までを半角カタカナとする
            return '\uFF65' <= c && c <= '\uFF9F';
        }
        /// <summary>
        /// 指定した Unicode 文字が、カタカナかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c がカタカナである場合は true。それ以外の場合は false。</returns>
        public static bool IsKatakana_ZenkakuOrHankaku(char c)
        {
            //「ダブルハイフン」から「コト」までと、カタカナフリガナ拡張と、
            //濁点と半濁点と、半角カタカナをカタカナとする
            //中点と長音記号も含む
            return ('\u30A0' <= c && c <= '\u30FF')
                || ('\u31F0' <= c && c <= '\u31FF')
                || ('\u3099' <= c && c <= '\u309C')
                || ('\uFF65' <= c && c <= '\uFF9F');
        }
        /// <summary>
        /// 指定した Unicode 文字が、漢字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が漢字である場合は true。それ以外の場合は false。</returns>
        public static bool IsKanji(char c)
        {
            //CJK統合漢字、CJK互換漢字、CJK統合漢字拡張Aの範囲にあるか調べる
            return ('\u4E00' <= c && c <= '\u9FCF')
                || ('\uF900' <= c && c <= '\uFAFF')
                || ('\u3400' <= c && c <= '\u4DBF');
        }


        // ■以下、string型

        /// <summary>
        /// 文字がアルファベットならtrueを返します。全角・半角かは問いません。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlfabet(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsAlfabet(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 文字がアルファベットか数字ならtrueを返します。全角・半角かは問いません。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlfabetOrNumber(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsAlfabetOrNumber(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 文字が全角アルファベットか全角数字ならtrueを返します。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlfabetOrNumber_Zenkaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsAlfabetOrNumber_Zenkaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 文字が半角アルファベットか半角数字ならtrueを返します。
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsAlfabetOrNumber_Hankaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsAlfabetOrNumber_Hankaku(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 指定した Unicode 文字が、英字の大文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabetUpper_Hankaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsAlfabetUpper_Hankaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、英字の小文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の小文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabetLower_Hankaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsAlfabetLower_Hankaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、英字の大文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabetUpper_Zenkaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsAlfabetUpper_Zenkaku(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 指定した Unicode 文字が、英字の小文字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の小文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabetLower_Zenkaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsAlfabetLower_Zenkaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、半角のアルファベットかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabet_Hankaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!(IsAlfabetLower_Hankaku(c) || IsAlfabetUpper_Hankaku(c)))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、全角のアルファベットかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が英字の大文字である場合は true。
        /// それ以外の場合は false。</returns>
        public static bool IsAlfabet_Zenkaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!(IsAlfabetLower_Zenkaku(c) || IsAlfabetUpper_Zenkaku(c)))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、半角の 0 から 9 までの数字（アラビア数字のみ）かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が数字である場合は true。それ以外の場合は false。</returns>
        public static bool IsNumber_Hankaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsNumber_Hankaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、全角の ０ から ９ までの数字（アラビア数字のみ）かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が数字である場合は true。それ以外の場合は false。</returns>
        public static bool IsNumber_Zenkaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsNumber_Zenkaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、ひらがなかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c がひらがなである場合は true。それ以外の場合は false。</returns>
        public static bool IsHiragana(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsHiragana(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、ひらがなあるいは全角カタカナだけで構成されているかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c がひらがなである場合は true。それ以外の場合は false。</returns>
        public static bool IsHiraganaOrZenkakuKatakana(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsHiraganaOrZenkakuKatakana(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、全角カタカナかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が全角カタカナである場合は true。それ以外の場合は false。</returns>
        public static bool IsKatakana_Zenkaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsKatakana_Zenkaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、半角カタカナかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が半角カタカナである場合は true。それ以外の場合は false。</returns>
        public static bool IsKatakana_Hankaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsKatakana_Hankaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、カタカナかどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c がカタカナである場合は true。それ以外の場合は false。</returns>
        public static bool IsKatakana_ZenkakuOrHankaku(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            foreach (char c in str)
            {
                if (!IsKatakana_ZenkakuOrHankaku(c))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、漢字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が漢字である場合は true。それ以外の場合は false。</returns>
        public static bool IsKanji(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            for (int index = 0; index < str.Length; index++)
            {
                if (str.Length <= index)
                {
                    throw new ArgumentException("index が str 内にない位置です。");
                }

                char c1 = str[index];
                if (char.IsHighSurrogate(c1))
                {
                    if (str.Length - 1 <= index)
                    {
                        return false;
                    }

                    char c2 = str[index + 1];
                    //CJK統合漢字拡張Bの範囲にあるか調べる
                    bool _isKanji_B = (('\uD840' <= c1 && c1 < '\uD869') && char.IsLowSurrogate(c2))
                        || (c1 == '\uD869' && ('\uDC00' <= c2 && c2 <= '\uDEDF'));
                    if (_isKanji_B == false)
                    {
                        return false;
                    }
                }
                else
                {
                    //CJK統合漢字、CJK互換漢字、CJK統合漢字拡張Aの範囲にあるか調べる
                    bool _isKanji_A = ('\u4E00' <= c1 && c1 <= '\u9FCF')
                        || ('\uF900' <= c1 && c1 <= '\uFAFF')
                        || ('\u3400' <= c1 && c1 <= '\u4DBF');
                    if (_isKanji_A == false)
                    {
                        return false;
                    }
                }
            }
            // ここまで来てfalseを返していなければ、全て_isKanji_AまたはBがtrueだったということ
            return true;
        }
        /// <summary>
        /// 指定した Unicode 文字が、漢字かどうかを示します。
        /// </summary>
        /// <param name="c">評価する Unicode 文字。</param>
        /// <returns>c が漢字である場合は true。それ以外の場合は false。</returns>
        public static bool IsKanji(string str, int index)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (str.Length <= index)
            {
                throw new ArgumentException("index が str 内にない位置です。");
            }

            char c1 = str[index];
            if (char.IsHighSurrogate(c1))
            {
                if (str.Length - 1 <= index)
                {
                    return false;
                }

                char c2 = str[index + 1];
                //CJK統合漢字拡張Bの範囲にあるか調べる
                return (('\uD840' <= c1 && c1 < '\uD869') && char.IsLowSurrogate(c2))
                    || (c1 == '\uD869' && ('\uDC00' <= c2 && c2 <= '\uDEDF'));
            }
            else
            {
                //CJK統合漢字、CJK互換漢字、CJK統合漢字拡張Aの範囲にあるか調べる
                return ('\u4E00' <= c1 && c1 <= '\u9FCF')
                    || ('\uF900' <= c1 && c1 <= '\uFAFF')
                    || ('\u3400' <= c1 && c1 <= '\u4DBF');
            }
        }



        /// <summary>
        /// 文字列が数値に変換できる文字列であればtrueを返します。
        /// 内部では、Int32.TryParse()を使って数値に変換できればtrue、変換できなければfalseを返しています。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string _string)
        {
            int _number = 0;
            if (Int32.TryParse(_string, out _number) == false)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 引数が整数であればtrue、それ以外であればfalseを返します。
        /// 内部では、TryParse()で変換出来て、かつその数字と整数に切り捨てた値との差が0ならtrueを返します。
        /// </summary>
        public static bool IsSeisu(string _string)
        {
            double _doubleNumber = 0.0;
            if (Double.TryParse(_string, out _doubleNumber) == false)
            {
                return false;
            }
            if (_doubleNumber == (int)_doubleNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 郵便番号の形式であればtrueを返します。「1234567」でも、「１２３－４５６７」でもtrueを返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsYubinBangou(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^\d\d\d[-－]{0,1}\d\d\d\d$"); // －と―ーは同じ文字として認識されるらしい。

            // これだと数字は全角でも、ハイフンが半角でないといけない
            //return System.Text.RegularExpressions.Regex.IsMatch(str, @"\d\d\d-\d\d\d\d");
            // （参考）こんなこともできる
            //TextBox1内の郵便番号っぽい文字列の"-"を削除して、【】で囲む
            //str = System.Text.RegularExpressions.Regex.Replace(str, @"(\d\d\d)-(\d\d\d\d)", "【$1$2】");
        }

        // 以下は小数を考慮していないから、あまり使えない。あと、正規表現は時間がかかるので、できればTryParseなどを使った方がいい
        // とりえあず正規表現の練習用に、残しておく。
        private static bool isSeisu_Hankaku(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, "^[+-]{0,1}[0-9]+$");
        }
        private static bool isSeisu_Hankaku_SignMinusOnly(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, "^[-]{0,1}[0-9]+$");
        }
        private static bool isSeisu_Hankaku_NoSign(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, "^[0-9]+$");
        }
        private static bool isSeisu_HankakuAndZenkaku(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, "^[+-＋－]{0,1}[0-9０-９]+$"); // 「ー１」でも「―１」でもtrue。－と―ーは同じ文字として認識されるらしい。
        }
        private static bool isSeisu_HankakuAndZenkaku_NoSign(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, "^[0-9０-９]+$"); // 「ー１」でも「―１」でもtrue。－と―ーは同じ文字として認識されるらしい。
        }
        // 正規表現を用いた文字列のひらがな／カタカナ／漢字だけかのチェックの参考: http://www.atmarkit.co.jp/fdotnet/dotnettips/054iskana/iskana.html
        private static bool isHiragana(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^\p{IsHiragana}+$");
        }
        private static bool isKatakana(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^\p{IsKatakana}+$");
        }
        private static bool isKanji(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, @"^\p{IsCJKUnifiedIdeographs}+$");
        }
        #endregion

        #region 正規表現を用いた文字列マッチング: isMatch
        /// <summary>
        /// 正規表現を使って、第１引数の文字列がある条件を満たした文字列かどうかを返します。
        /// 
        /// 例１．半角数字であるか確かめる：　isMatch(文字列, @"^[+-]{0,1}[0-9]+$")、または、isNumber_Hankaku(文字列)
        /// 
        /// その他：
        /// [Tips][Memo]【正規表現】の使い方。
        /// ※　^          は冒頭、$は末尾
        /// ※  .          は改行文字(\n)以外の任意の一文字。（ただし [] 内ではピリオド文字。）
        /// ※  .?         は任意の0文字または1文字
        /// ※　.*         は任意の0文字以上
        /// ※  [abc]*     はabcのいずれかが0文字以上。
        /// ※　[abc]{0,1} は0文字以上1文字以下の繰り返し
        /// ※  [0-9]+     は0-9の1文字以上。
        /// ※　\d         は数字1文字。全角・半角どちらでも使える。@"[0-9]"は半角のみ
        /// ※  \s         は空白文字。改行文字、タブ文字、半角/全角スペース文字など。[\f\n\r\t\v\x85\p{Z}]と同じ。
        /// ※　@          をつけているのは\のため。　例：　@"\d"とすれば、"\\d"と書かないでいいようにするため
        /// ※その他詳細は、C#で使える正規表現の参考： http://dobon.net/vb/dotnet/string/regex.html
        /// </summary>
        /// <param name="_parsedString"></param>
        /// <param name="_matchString_SeikiHyougen">正規表現の文字列@""</param>
        /// <returns></returns>
        public static bool isMatch(string _str, string _matchString_SeikiHyougen)
        {
            // "txt"などで始まる正規表現の文字列はエラーにならないので、そのまま
            // ""だったり、"*"で始まる正規表現の文字列はエラーになるが、よく間違えやすいので、".*"におきかえる。
            if (_matchString_SeikiHyougen == "" || _matchString_SeikiHyougen.Substring(0, 1) == "*")
            {
                _matchString_SeikiHyougen = ".*";
            }
            return System.Text.RegularExpressions.Regex.IsMatch(_str, _matchString_SeikiHyougen);
        }
        #endregion

        #region A～Bに囲まれた文字列の取得: getWords_BetweenAtoB
        /// <summary>
        /// 引数のA～Bに囲まれた（最も短い）文字列を返します。AやBが見つからない場合はnullを返します。
        /// なお、_isGetMostLongOneをtrueにすると、最も長い文字列を返します。
        /// 
        /// 例１：　getWord_BetweenAtoB("今日の献立は「ご飯」と「味噌汁」だった", "「", "」", false)　→    [0]="ご飯", [1]="味噌汁"
        /// 例２：　getWord_BetweenAtoB("今日の献立は「ご飯」と「味噌汁」だった", "「", "」", true)　 →    [0]="ご飯」と「味噌汁"
        /// </summary>
        public static string[] getWords_BetweenAtoB(string _str, string _A_startString, string _B_endString, bool _isGetMostLongOne)
        {
            string[] _words = null;
            int _tokenNum = 0;

            // A～Bで囲まれている文字列を全て格納
            System.Text.RegularExpressions.MatchCollection _mc;
            if (_isGetMostLongOne == false)
            {
                _mc = System.Text.RegularExpressions.Regex.Matches(_str, _A_startString + ".*?" + _B_endString);
            }
            else
            {
                _mc = System.Text.RegularExpressions.Regex.Matches(_str, _A_startString + ".*" + _B_endString);
            }
            // 1個でもあればstring[]型に文字列を格納
            if (_mc != null)
            {
                _tokenNum = _mc.Count;
                if (_tokenNum > 0)
                {
                    _words = new string[_tokenNum];
                    for (int i = 0; i < _tokenNum; i++)
                    {
                        _words[i] = _mc[i].Value;
                    }
                }
            }
            return _words;
        }
        #endregion

        #region 文字タイプの変換: parseString_***
        /// <summary>
        /// 引数の文字列のうち、半角・全角カタカナをひらがなに変換したものを返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string parseString_KatakanaToHiragana(string _str)
        {
            string _parsedString = _str;
            if (_str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            // 1文字ずつ、指定した文字タイプに含まれているかを調べる
            for (int i = 0; i < _str.Length; i++)
            {
                char _char = _str[i];
                string _charString = _char.ToString();
                // 指定した文字タイプに含まれているか
                if (IsKatakana_Hankaku(_charString) == true)
                {
                    // 全角カタカナに置き換え
                    _charString = parseString(_charString, VbStrConv.Narrow);
                }
                // 全角カタカナだったら、ひらがなに置き換え
                if (IsKatakana_Zenkaku(_charString) == true)
                {
                    // 全角カタカナに置き換え
                    _charString = parseString(_charString, VbStrConv.Hiragana);
                    // 変換失敗文字に置き換え
                    _parsedString = _str.Substring(0, i) + _charString + _str.Substring(Math.Min(i + 1, _str.Length - 1));
                }
            }
            return _parsedString;
        }
        /// <summary>
        /// 引数の文字列のうち、ひらがなを全角カタカナに変換したものを返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string parseString_HiraganaToZenkakuKatakana(string _str)
        {
            string _parsedString = _str;
            if (_str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            // 1文字ずつ、指定した文字タイプに含まれているかを調べる
            for (int i = 0; i < _str.Length; i++)
            {
                char _char = _str[i];
                string _charString = _char.ToString();
                // 指定した文字タイプに含まれているか
                if (IsHiragana(_char) == true)
                {
                    // 全角カタカナに置き換え
                    _charString = parseString(_charString, VbStrConv.Katakana);
                    // 変換失敗文字に置き換え
                    _parsedString = _str.Substring(0, i) + _charString + _str.Substring(Math.Min(i + 1, _str.Length - 1));
                }
            }
            return _parsedString;
        }
        /// <summary>
        /// 引数の文字列のうち、ひらがなと全角カタカナ　を半角カタカナに変換したものを返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string parseString_HiraganaOrZenkakuKatakanaToHankakuKatakana(string _str)
        {
            string _parsedString = _str;
            if (_str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            // 1文字ずつ、指定した文字タイプに含まれているかを調べる
            for (int i = 0; i < _str.Length; i++)
            {
                char _char = _str[i];
                string _charString = _char.ToString();
                // 指定した文字タイプに含まれているか
                if (IsHiragana(_char) == true)
                {
                    // 全角カタカナに置き換え
                    _charString = parseString(_charString, VbStrConv.Katakana);
                }
                // 全角カタカナだったら、半角カタカナに置き換え
                if (IsKatakana_Zenkaku(_charString) == true)
                {
                    // 半角カタカナに置き換え
                    _charString = parseString(_charString, VbStrConv.Narrow);
                    // 変換失敗文字に置き換え
                    _parsedString = _str.Substring(0, i) + _charString + _str.Substring(Math.Min(i + 1, _str.Length - 1));
                }
            }
            return _parsedString;
        }
        /// <summary>
        /// 引数の文字列のうち、全角カタカナだけ（ひらがなは含まない）を半角カタカナに変換したものを返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string parseString_ZenkakuKatakanaToHankaku(string _str)
        {
            string _parsedString = _str;
            if (_str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            // 1文字ずつ、指定した文字タイプに含まれているかを調べる
            for (int i = 0; i < _str.Length; i++)
            {
                char _char = _str[i];
                string _charString = _char.ToString();
                // 指定した文字タイプに含まれているか
                if (IsKatakana_Zenkaku(_char) == true)
                {
                    // 半角カタカナに置き換え
                    _charString = parseString(_charString, VbStrConv.Narrow);
                    // 変換失敗文字に置き換え
                    _parsedString = _str.Substring(0, i) + _charString + _str.Substring(Math.Min(i + 1, _str.Length - 1));
                }
            }
            return _parsedString;
        }
        /// <summary>
        /// 第一引数の文字列を、半角カタカナが存在しない文字列に変換して返します。
        /// 第二引数をTrueにすると全角カタカナに置き換え、Falseにすると他の文字に置き換えます。
        /// 第三引数は半角カタカナを他の文字に置き換える時に使用する１文字です。nullを指定した場合はデフォルトの変換失敗文字"?"に置き換え、""を指定した場合は半角カタカナだけ消せます）。
        /// </summary>
        public static string parseString_NotInclusing_HankakuKatakana(string _str, bool _TrueIsParseToZenkaku_FalseIsReplacingChar, string _ReplacingChar_string)
        {
            string _parsedString = _str;
            if (_str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            // 1文字ずつ、指定した文字タイプに含まれているかを調べる
            for (int i = 0; i < _str.Length; i++)
            {
                char _char = _str[i];
                // 指定した文字タイプに含まれているか
                if (IsKatakana_Hankaku(_char) == true)
                {
                    if (_TrueIsParseToZenkaku_FalseIsReplacingChar == true)
                    {
                        // 全角カタカナに置き換え
                        _ReplacingChar_string = parseString(_char.ToString(), VbStrConv.Wide);
                    }
                    else
                    {
                        // 含まれていない文字は、変換失敗文字に置き換え
                        // 変換失敗文字にnullが指定されたら、デフォルトのものを使う
                        if (_ReplacingChar_string == null)
                        {
                            _ReplacingChar_string = s_CanNotParsedReplacingChar_Default_Hankaku;
                        }
                    }
                    // 変換失敗文字に置き換え
                    // これができないので、Substringを使ってる
                    //_parsedString[i] = _CanNoParsedReplacingChar_string;
                    _parsedString = _str.Substring(0, i) + _ReplacingChar_string + _str.Substring(Math.Min(i + 1, _str.Length - 1));
                }
            }
            return _parsedString;
        }
        /// <summary>
        /// parseStringメソッドで使う、指定文字タイプに変換できなかった変換失敗文字（半角）です。
        /// </summary>
        private static string s_CanNotParsedReplacingChar_Default_Hankaku = "?";
        /// <summary>
        /// parseStringメソッドで使う、指定文字タイプに変換できなかった変換失敗文字（全角）です。
        /// </summary>
        private static string s_CanNotParsedReplacingChar_Default_Zenkaku = "？";
        /// <summary>
        /// 第一引数の文字列を、指定した文字タイプに含まれる文字だけ残して、あとは指定した変換失敗文字（デフォルトでは"?"や"？"）にして返します。
        /// ※このメソッドでは全角⇔半角の変換はできません。
        /// 機能が中途半端なので、private型にしています。
        /// 
        /// 　　なお、getParsedString()が、内部でこのメソッドを呼び出しています。
        /// 　　getParsedString()では、出来る限り半角⇔全角変換も考慮した、文字タイプの変換が可能です。
        /// </summary>
        private static string parseString(string _str, EStringCharType _stringCharType)
        {
            return parseString(_str, _stringCharType, null);
        }
        /// <summary>
        /// 第一引数の文字列を、指定した文字タイプに含まれる文字だけ残して、あとは指定した変換失敗文字（デフォルトでは"?"や"？"）にして返します。
        /// ※このメソッドでは全角⇔半角の変換はできません。
        /// 機能が中途半端なので、private型にしています。
        /// 
        /// 　　なお、getParsedString()が、内部でこのメソッドを呼び出しています。
        /// 　　getParsedString()では、出来る限り半角⇔全角変換も考慮した、文字タイプの変換が可能です。
        /// </summary>
        private static string parseString(string _str, EStringCharType _stringCharType, string _CanNoParsedReplacingChar_string)
        {
            string _parsedString = _str;
            if (_str == null)
            {
                throw new ArgumentNullException("str", "引数がnullです");
            }

            // 1文字ずつ、指定した文字タイプに含まれているかを調べる
            for (int i = 0; i < _str.Length; i++)
            {
                char _char = _str[i];
                // 指定した文字タイプに含まれているか
                if (isEStringCharType(_char.ToString(), _stringCharType) == false)
                {
                    // 含まれていない文字は、変換失敗文字に置き換え
                    // 変換失敗文字にnullが指定されたら、デフォルトのものを使う
                    if (_CanNoParsedReplacingChar_string == null)
                    {
                        if (IsHankaku(_char) == true)
                        {
                            _CanNoParsedReplacingChar_string = s_CanNotParsedReplacingChar_Default_Hankaku;
                        }
                        else
                        {
                            _CanNoParsedReplacingChar_string = s_CanNotParsedReplacingChar_Default_Zenkaku;
                        }
                    }
                    // 変換失敗文字に置き換え
                    // これができないので、Substringを使ってる
                    //_parsedString[i] = _CanNoParsedReplacingChar_string;
                    _parsedString = _str.Substring(0, i) + _CanNoParsedReplacingChar_string + _str.Substring(Math.Min(i + 1, _str.Length - 1));
                }
            }
            return _parsedString;
        }

        #endregion

        #region 文字タイプのチェック: getParsedString / getStringCharType / isEStringCharType
        /// <summary>
        /// 第一引数の文字列を、指定した文字タイプかどうかをチェックし、
        /// 違う場合はその文字タイプに変換した、チェック後の文字列を返します。（変換できない文字は空白もしくは文字化けする可能性があります）
        /// （※返還前の文字タイプを知りたければ、getStringCharType()を使ってください）
        /// </summary>
        public static string getParsedString(string _string, EStringCharType _stringCharType)
        {
            string _checkedString = _string;
            bool _isType = isEStringCharType(_string, _stringCharType);
            if (_isType == false)
            {
                switch (_stringCharType)
                {
                    case EStringCharType.t0_None_未定義_何でもＯＫ:
                        break;
                    case EStringCharType.t0a_Number1_Seisu_半角数字＿符号なしの正の整数:
                        _checkedString = parseString(_checkedString, VbStrConv.Narrow);
                        _checkedString = parseInt(_checkedString).ToString();
                        break;
                    case EStringCharType.t0a_Number2_Syosu_半角数字＿符号なしの正の小数:
                        _checkedString = parseString(_checkedString, VbStrConv.Narrow);
                        _checkedString = parseInt(_checkedString).ToString();
                        break;
                    case EStringCharType.t0a_Number3_Hankaku_半角数字＿実数でマイナスのみ符号あり:
                        _checkedString = parseString(_checkedString, VbStrConv.Narrow);
                        _checkedString = parseInt(_checkedString).ToString();
                        _checkedString = _checkedString.Replace("+", ""); // プラス符号を消す
                        break;
                    case EStringCharType.t0a_Number4_Zenkaku_全角数字＿実数でマイナスのみ符号あり:
                        _checkedString = parseString(_checkedString, VbStrConv.Wide);
                        _checkedString = parseInt(_checkedString).ToString();
                        _checkedString = _checkedString.Replace("＋", ""); // プラス符号を消す
                        break;
                    case EStringCharType.t0b_AlfabetOrNumber_半角英数字＿記号を含まない:
                        _checkedString = parseString(_checkedString, VbStrConv.Narrow);
                        _checkedString = parseString(_checkedString, EStringCharType.t0b_AlfabetOrNumber_半角英数字＿記号を含まない);
                        break;
                    case EStringCharType.t0b_English_半角英数字＿記号を含む:
                        _checkedString = parseString(_checkedString, VbStrConv.Narrow);
                        break;
                    case EStringCharType.t0c_Japanese2_日本語＿半角カタカナを含まない:
                        _checkedString = parseString_NotInclusing_HankakuKatakana(_checkedString, true, null);
                        break;
                    case EStringCharType.t0c_Japanese1_日本語＿半角カタカナを含む:
                        break; //goto case EStringCharType.t0_None_未定義_何でもＯＫ;
                    case EStringCharType.t0d_JapaneseHiraganaOnly＿全角ひらがなのみ:
                        _checkedString = parseString(_checkedString, VbStrConv.Wide);
                        _checkedString = parseString(_checkedString, EStringCharType.t0c_Japanese2_日本語＿半角カタカナを含まない);
                        _checkedString = parseString(_checkedString, VbStrConv.Hiragana);
                        break;
                    case EStringCharType.t0d_JapaneseKatakanaZenkakuOnly＿全角カタカナのみ:
                        _checkedString = parseString(_checkedString, VbStrConv.Wide);
                        _checkedString = parseString(_checkedString, EStringCharType.t0c_Japanese2_日本語＿半角カタカナを含まない);
                        _checkedString = parseString(_checkedString, VbStrConv.Katakana);
                        break;
                    case EStringCharType.t0d_JapaneseKatakanaHankakuOnly＿半角カタカナのみ:
                        _checkedString = parseString(_checkedString, EStringCharType.t0c_Japanese1_日本語＿半角カタカナを含む);
                        _checkedString = parseString(_checkedString, VbStrConv.Katakana);
                        _checkedString = parseString(_checkedString, VbStrConv.Narrow);
                        break;
                    case EStringCharType.t0d_JapaneseKanjiOnly＿漢字のみ:
                        _checkedString = parseString(_checkedString, VbStrConv.Wide);
                        _checkedString = parseString(_checkedString, EStringCharType.t0d_JapaneseKanjiOnly＿漢字のみ);
                        break;
                    default:
                        break;
                }
            }
            return _checkedString;
        }
        /// <summary>
        /// 第一引数の文字列が、指定した文字タイプになっているかをチェックします。
        /// </summary>
        public static bool isEStringCharType(string _string, EStringCharType _stringtype)
        {
            bool _isType = false;
            switch (_stringtype)
            {
                case EStringCharType.t0_None_未定義_何でもＯＫ:
                    _isType = true; break;
                case EStringCharType.t0a_Number1_Seisu_半角数字＿符号なしの正の整数:
                    _isType = (IsHankaku(_string) && IsSeisu(_string) && parseInt(_string) > 0 && _string.Contains("+") == false); break;
                case EStringCharType.t0a_Number2_Syosu_半角数字＿符号なしの正の小数:
                    _isType = (IsHankaku(_string) && IsSeisu(_string) == false && parseInt(_string) > 0 && _string.Contains("+") == false); break;
                case EStringCharType.t0a_Number3_Hankaku_半角数字＿実数でマイナスのみ符号あり:
                    _isType = (IsHankaku(_string) && IsNumber(_string) && _string.Contains("+") == false); break;
                case EStringCharType.t0a_Number4_Zenkaku_全角数字＿実数でマイナスのみ符号あり:
                    _isType = (IsZenkaku(_string) && IsNumber(_string) && _string.Contains("＋") == false); break;
                case EStringCharType.t0a_Number5_Others_その他の数字:
                    _isType = (IsNumber(_string)); break;
                case EStringCharType.t0b_AlfabetOrNumber_半角英数字＿記号を含まない:
                    _isType = (IsHankaku(_string) && IsAlfabetOrNumber(_string)); break;
                case EStringCharType.t0b_English_半角英数字＿記号を含む:
                    _isType = (IsHankaku(_string) && IsHankakuKatakana_Including(_string) == false); break;
                case EStringCharType.t0c_Japanese1_日本語＿半角カタカナを含む:
                    _isType = (IsHankaku(_string) == false && IsHankakuKatakana_Including(_string)); break;
                case EStringCharType.t0c_Japanese2_日本語＿半角カタカナを含まない:
                    _isType = (IsHankaku(_string) == false && IsHankakuKatakana_Including(_string) == false); break;
                case EStringCharType.t0d_Japanese3_全角日本語＿半角カタカナ英数字記号を含まない:
                    _isType = (IsZenkaku(_string) && IsHankakuKatakana_Including(_string) == false); break;
                case EStringCharType.t0d_JapaneseHiraganaOnly＿全角ひらがなのみ:
                    _isType = (IsZenkaku(_string) && IsHiragana(_string)); break;
                case EStringCharType.t0d_JapaneseKatakanaZenkakuOnly＿全角カタカナのみ:
                    _isType = (IsZenkaku(_string) && IsKatakana_Zenkaku(_string)); break;
                case EStringCharType.t0d_JapaneseKatakanaHankakuOnly＿半角カタカナのみ:
                    _isType = (IsHankaku(_string) && IsKatakana_Hankaku(_string)); break;
                case EStringCharType.t0d_JapaneseHiraganaOrKatakanaZenkakuOnly＿全角ひらがなかカタカナのみ:
                    _isType = (IsZenkaku(_string) && IsHiraganaOrZenkakuKatakana(_string)); break;
                case EStringCharType.t0d_JapaneseKanjiOnly＿漢字のみ:
                    _isType = (IsZenkaku(_string) && IsKanji(_string)); break;
                default:
                    _isType = true; break;
            }
            return _isType;
        }
        /// <summary>
        /// 文字列の文字タイプを返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_EStringCharType"></param>
        public static EStringCharType getEStringCharType(string _string)
        {
            EStringCharType _type;
            // 判定が高速で、かつよく使う処理から優先的に調べる
            if (IsNumber(_string) == true)
            {
                // 数字
                if (IsHankaku(_string) == true)
                {
                    // 半角
                    // マイナスの符号が入っているか
                    if (_string.Contains("-") == false)
                    {
                        // 正の数
                        if (IsSeisu(_string) == true)
                        {
                            // プラスの符号が入っているか
                            if (_string.Contains("+") == false)
                            {
                                // 整数
                                _type = EStringCharType.t0a_Number1_Seisu_半角数字＿符号なしの正の整数;
                            }
                            else
                            {
                                // "+"が入っている正の整数
                                _type = EStringCharType.t0a_Number5_Others_その他の数字;
                            }
                        }
                        else
                        {
                            // プラスの符号が入っているか
                            if (_string.Contains("+") == false)
                            {
                                // 小数
                                _type = EStringCharType.t0a_Number2_Syosu_半角数字＿符号なしの正の小数;
                            }
                            else
                            {
                                // "+"が入っている正の小数
                                _type = EStringCharType.t0a_Number5_Others_その他の数字;
                            }
                        }
                    }
                    else
                    {
                        // 負の数

                        // プラスの符号が入っているか
                        if (_string.Contains("+") == false)
                        {
                            _type = EStringCharType.t0a_Number3_Hankaku_半角数字＿実数でマイナスのみ符号あり;
                        }
                        else
                        {
                            // "+"が入っている半角数字
                            _type = EStringCharType.t0a_Number5_Others_その他の数字;
                        }
                    }
                }
                else
                {
                    if (IsZenkaku(_string) == true)
                    {
                        // 全角
                        // プラスの符号が入っているか
                        if (_string.Contains("＋") == false)
                        {
                            _type = EStringCharType.t0a_Number4_Zenkaku_全角数字＿実数でマイナスのみ符号あり;
                        }
                        else
                        {
                            // "＋"が入ってる全角数字
                            _type = EStringCharType.t0a_Number5_Others_その他の数字;
                        }
                    }
                    else
                    {
                        // 全角と半角が混合している数字
                        _type = EStringCharType.t0a_Number5_Others_その他の数字;
                    }
                }
            }
            else
            {
                // 数字以外
                if (IsZenkakuChar_Including(_string) == false)
                {
                    // 半角
                    if (IsAlfabet_Hankaku(_string) == true || IsNumber_Hankaku(_string) == true || IsAlfabetOrNumber_Hankaku(_string) == true)
                    {
                        _type = EStringCharType.t0b_AlfabetOrNumber_半角英数字＿記号を含まない;
                    }
                    else if (IsKatakana_Hankaku(_string) == true)
                    {
                        // 半角カタカナのみ
                        _type = EStringCharType.t0d_JapaneseKatakanaZenkakuOnly＿全角カタカナのみ;
                    }
                    else
                    {
                        // 記号を含む半角文字
                        _type = EStringCharType.t0b_English_半角英数字＿記号を含む;
                    }
                }
                else
                {
                    // 全角を一文字以上含む文字列（半角の存在はまだ調べていない）

                    // 全て全角かで分岐
                    if (IsZenkaku(_string) == true)
                    {
                        // 全角ひらがなあるいは全角カタカナだけで構成されているかで分岐
                        if (IsHiraganaOrZenkakuKatakana(_string) == true)
                        {
                            if (IsHiragana(_string) == true)
                            {
                                _type = EStringCharType.t0d_JapaneseHiraganaOnly＿全角ひらがなのみ;
                            }
                            else if (IsKatakana_Zenkaku(_string) == true)
                            {
                                _type = EStringCharType.t0d_JapaneseKatakanaZenkakuOnly＿全角カタカナのみ;
                            }
                            else
                            {
                                _type = EStringCharType.t0d_JapaneseHiraganaOrKatakanaZenkakuOnly＿全角ひらがなかカタカナのみ;
                            }
                        }
                        else if (isKanji(_string) == true)
                        {
                            _type = EStringCharType.t0d_JapaneseKanjiOnly＿漢字のみ;
                        }
                        else
                        {
                            // 半角文字を含まない日本語
                            _type = EStringCharType.t0d_Japanese3_全角日本語＿半角カタカナ英数字記号を含まない;
                        }
                    }
                    else
                    {
                        // 半角文字と全角文字が混じっている

                        // 半角カタカナを含むかどうかで分岐
                        if (IsHankakuKatakana_Including(_string) == true)
                        {
                            _type = EStringCharType.t0c_Japanese1_日本語＿半角カタカナを含む;
                        }
                        else
                        {
                            _type = EStringCharType.t0c_Japanese2_日本語＿半角カタカナを含まない;
                        }
                    }
                }
            }
            return _type;
        }
        /// <summary>
        /// 文字列のタイプを示す列挙体です。getStringCharType()などで使われます。
        /// </summary>
        public enum EStringCharType
        {
            t0_None_未定義_何でもＯＫ,
            t0a_Number1_Seisu_半角数字＿符号なしの正の整数,
            t0a_Number2_Syosu_半角数字＿符号なしの正の小数,
            t0a_Number3_Hankaku_半角数字＿実数でマイナスのみ符号あり,
            t0a_Number4_Zenkaku_全角数字＿実数でマイナスのみ符号あり,
            t0a_Number5_Others_その他の数字,
            //t0a_Number1_数字＿半角＿符号を含む,
            //t0a_Number2_数字＿半角＿符号を含まない,
            //t0a_Number3_数字＿全角＿符号を含む,
            //t0a_Number4_数字＿全角＿符号を含まない,
            //t0a_NumberOther_数字＿その他半角全角混合,
            t0b_AlfabetOrNumber_半角英数字＿記号を含まない,
            t0b_English_半角英数字＿記号を含む,
            t0c_Japanese1_日本語＿半角カタカナを含む,
            t0c_Japanese2_日本語＿半角カタカナを含まない,
            t0d_Japanese3_全角日本語＿半角カタカナ英数字記号を含まない,
            t0d_JapaneseHiraganaOnly＿全角ひらがなのみ,
            t0d_JapaneseKatakanaZenkakuOnly＿全角カタカナのみ,
            t0d_JapaneseKatakanaHankakuOnly＿半角カタカナのみ,
            t0d_JapaneseHiraganaOrKatakanaZenkakuOnly＿全角ひらがなかカタカナのみ,
            t0d_JapaneseKanjiOnly＿漢字のみ,

            // 以下、できるだけ多くの条件を整理したメモ

            //t1_Number_半角全角数字_符号を含む,
            //t1a_NumberHankaku_半角数字,
            //t1b_NumberZenkaku_全角数字,
            //t1z_NumberOther_その他の数字,

            //t2_Hankaku_半角_半角文字だけなら何でも,
            //t2a_HankakuEisu_半角英数字,
            //t2a1_Atoz_半角英大小文字,
            //t2a1a_AtoZ_半角英大文字,
            //t2a1b_aTOz_半角英小文字,
            //t2b_HankakuKatakana_半角カタカナ,
            //t2z_その他の半角文字,

            //t3_Zenkaku_全角_全角文字だけなら何でも,
            //t3a_ZenkakuHiragana_全角ひらがな,
            //t3b_ZenkakuKatakana_全角カタカナ,
            //t3c_ZenkakuKanji_全角漢字,
            //t3d_ZenkakuEisu_全角英数,
            //t3e_ZenkakuOther_その他の全角文字,

        }
        #endregion

        #region 文字列比較: getNowEqual/getEqual..._checkString
        /// <summary>
        /// ２つの文字列で異なっている箇所の文字列インデックスを返します。等しい場合は-1を返します。
        /// </summary>
        /// <param name="baseStr1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int getNotEqualIndex_checkString1ToString2(string str1, string str2)
        {
            int strlen_min = str2.Length;
            if (strlen_min > str1.Length)
            {
                strlen_min = str1.Length;
            }
            // 左から文字列が等しいインデックスを調べる
            int length;
            for (length = 1; length <= strlen_min; length++)
            {
                if (str1.Substring(0, length) != str2.Substring(0, length))
                {
                    break;
                }
                else if (length == strlen_min)
                {
                    if (str1.Length != str2.Length)
                    {
                        length++;
                        break;
                    }
                    else
                    {
                        length = 0; // 等しい
                        break;
                    }
                }
            }
            return length - 1;
        }
        /// <summary>
        /// ２つの文字列で同じインデックスで等しい文字数を返します。途中で等しい文字列がずれている場合は等しくないと判断します。
        /// </summary>
        /// <param name="baseStr1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int getEqualsIndexesNum_checkString1ToString2(string str1, string str2)
        {
            int equalLength = 0;

            if (str1 == str2)
            {
                equalLength = str1.Length;
            }
            else
            {
                int strlen_min = str1.Length;
                if (strlen_min > str1.Length)
                {
                    strlen_min = str1.Length;
                }
                // 文字が等しいインデックスの数を調べる
                int i;
                for (i = 0; i < strlen_min; i++)
                {
                    if (str1.Substring(i, 1) == str2.Substring(i, 1))
                    {
                        equalLength++;
                    }
                }
            }
            return equalLength;
        }
        /// <summary>
        /// ２つの文字列でブロック単位で等しい文字数を返します。baseStr1を間違いの無い見本として解析し、str2が途中で等しい文字列が指定M文字以下ずれている場合は、等しい文字列ブロックが指定N文字以上なら等しいと判断します。第５引数に、等しいブロック文字列を" - "で区切った文字列を返します。
        /// </summary>
        /// <param name="baseStr1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int getEqualsLength_checkString1ToString2(string baseStr1, string str2, int missLength_max_M, int equalStringBlock_length_min_N, bool isShowMissedMessage, out string equalsBlockString_conection_minus_, out string missedBlockString_conection_minus_)
        {
            int equalLength = 0;
            string missedMessage = ""; // 確認メッセージ（Debug時のみ表示）
            equalsBlockString_conection_minus_ = ""; // 等しい文字列（外部へ返す）
            missedBlockString_conection_minus_ = ""; // 等しい文字列（外部へ返す）

            // 等しい文字列ブロックがあるかを調べる
            int i, j, k;
            int distance = 0, distance_temp = 0; // ベーステキストを基準にした入力テキストのずれ（等しいブロックが何文字余計に右にシフトしてるか）
            int blockNum = 0;
            string equalStringBlock = ""; // 等しい文字列（ブロック連結）
            string nextBlock; // 等しいブロック
            string missedStringBlock = "";// 間違った文字列（ブロック連結）
            string missedBlock; // 間違ったブロック
            string finishedText = "";

            if (baseStr1 == str2)
            {
                equalLength = baseStr1.Length;
                equalsBlockString_conection_minus_ = baseStr1;
                missedBlockString_conection_minus_ = "";
            }
            else
            {
                // 小さい文字列にあわせる
                int strlen_min = str2.Length;
                if (strlen_min > baseStr1.Length)
                {
                    strlen_min = baseStr1.Length;
                }

                // はじめの文字から終わりまで、等しいブロックを検索する
                for (i = 0; i < strlen_min; i++)
                {
                    if (i + distance >= strlen_min)
                    {
                        if (strlen_min == str2.Length)
                        {
                            // 入力テキストは足りていない
                            missedStringBlock += " - " + "*";
                        }
                        else
                        {
                            // 入力テキストの末端には余分な文字がある
                            missedStringBlock += " - " + str2.Substring(strlen_min - 1, str2.Length - strlen_min);
                        }
                        // 文字検索終了
                        break;
                    }
                    if (baseStr1.Substring(i, 1) == str2.Substring(i + distance, 1))
                    {
                        if (blockNum == 0)
                        {
                            blockNum++;
                        }
                        // 等しい文字列に追加！
                        equalStringBlock += baseStr1.Substring(i, 1);
                    }
                    else
                    {
                        // 入力テキストi番目の文字が、ベーステキストとずれているかどうかを調べる
                        for (j = i + 1; j < strlen_min; j++)
                        {
                            missedBlock = "";
                            nextBlock = "";
                            if (j + distance >= strlen_min)
                            {
                                break;
                            }
                            if (baseStr1.Substring(j, 1) == str2.Substring(i + distance, 1))
                            {
                                // ベーステキストを(j-_index)文字読み飛ばしている可能性
                                distance_temp = (-1) * (j + distance - i); // 入力テキストの方が左シフトしている
                                missedBlock = "*"; // 「*」は足りてないことを示す

                                nextBlock = baseStr1.Substring(j, 1);

                                // 次の等しいブロック文字列を調べる
                                for (k = j + 1; k < strlen_min; k++)
                                {
                                    if (baseStr1.Substring(k, 1) == str2.Substring(i + distance + (k - j), 1))
                                    {
                                        nextBlock += baseStr1.Substring(k, 1);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else if (baseStr1.Substring(i, 1) == str2.Substring(j + distance, 1))
                            {
                                // ベーステキストより(j-_index)文字余計に書いている可能性
                                distance_temp = (j + distance - i); // 入力テキストの方が右シフトしている
                                missedBlock = str2.Substring(i + distance, j - i);
                                nextBlock = baseStr1.Substring(i, 1);

                                // 次の等しいブロック文字列を調べる
                                for (k = j + 1; k < strlen_min; k++)
                                {
                                    if (baseStr1.Substring(i + (k - j), 1) == str2.Substring(k + distance, 1))
                                    {
                                        nextBlock += baseStr1.Substring(k, 1);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else if ((j - i) > missLength_max_M)
                            {
                                break; // ずれの検査がMより大きければ、終了
                            }

                            // 次のブロック文字数がN以上ならば、等しい文字列として認める
                            if (nextBlock.Length >= equalStringBlock_length_min_N)
                            {
                                // 等しい文字列に追加！
                                equalStringBlock += " - " + nextBlock; // 連結部分を"-"で格納 // 別方法：　for+="-"をmissedLength回繰り返すのもありかも
                                blockNum++;
                                // 間違った文字列を追加
                                if (blockNum == 1 && equalStringBlock.Substring(0, 3) != " - ") // 始めから間違いがない場合、始めだけ" - "は要らない
                                { }
                                else
                                {
                                    missedStringBlock += " - ";
                                }
                                missedStringBlock += missedBlock;

                                #region 確認メッセージ
                                if (distance_temp < 0)
                                {
                                    if (i > 1)
                                    {
                                        finishedText = baseStr1.Substring(0, i);
                                    }
                                    missedMessage += "●" + (-1) * distance_temp + "文字読み飛ばし：　" + i + "文字目\"" + finishedText + "\"の次の\"" + baseStr1.Substring(i, Math.Min(Math.Min(5, baseStr1.Length - i), 1)) + "...\"までに" + (-1) * distance_temp + "文字を読み飛ばしています。\n";
                                }
                                else
                                {
                                    if (i > 1)
                                    {
                                        finishedText = baseStr1.Substring(0, i);
                                    }
                                    missedMessage += "○" + distance_temp + "文字の余計な入力：" + i + "文字目\"" + finishedText + "\"の次の\"" + baseStr1.Substring(i, Math.Min(Math.Min(5, baseStr1.Length - i), 1)) + "...\"までに余計な" + distance_temp + "文字があります。\n";
                                }
                                #endregion

#if Test
                                MessageBox.Show(missedMessage);
#endif

                                distance += distance_temp; // 今回までで、入力テキストは、ベーステキストよりdistanceだけ右シフトしていることがわかった
                                // 次の文字はブロック数だけ右へ（-1しているのはループでi++するため）
                                i += nextBlock.Length - 1;

                                // ずれの検査終了
                                break;
                            }
                            else
                            {
                                // 次のずらしへ
                            }

                        }
                        // ずれの検査終了

                    }
                    // 文字終了
                    // 次の文字へ(_index++)
                }
                // " - "の数だけ文字数を引く
                //×equalStringBlock.Length - 3 * blockNum; 
                string equalString = equalStringBlock.Replace(" - ", "");
                equalLength = equalString.Length;

                equalsBlockString_conection_minus_ = equalStringBlock;
                missedBlockString_conection_minus_ = missedStringBlock;
            }
            string debugMessage = "";
#if Debug
            int rate = 100;
            if (baseStr1 != "")
            {
                rate = (int)((double)equalLength / (double)baseStr1.Length * 100.0);
            }
            debugMessage = ("ベーステキスト：\"" + baseStr1 + "\"と比較して、\n等しい文字列の比率は （" + equalLength + "文字/全" + baseStr1.Length + "文字数）＝" + rate + "％で、\n等しい文字列のブロック単位 : " + equalStringBlock + "\n誤り文字列のブロック単位\n" + missedStringBlock + "\nです。");
#endif
            if (isShowMissedMessage == true)
            {
                MessageBox.Show(debugMessage + "\n【誤り箇所】：\n" + missedMessage);
            }
            return equalLength;
        }
        /// <summary>
        /// ２つの文字列でブロック単位で等しい文字数を返します。baseStr1を間違いの無い見本として解析し、str2が途中で等しい文字列が指定M文字以下ずれている場合は、等しい文字列ブロックが指定N文字以上なら等しいと判断します。
        /// </summary>
        /// <param name="baseStr1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int getEqualsLength_checkString1ToString2(string baseStr1, string str2, int missLength_max_M, int equalStringBlock_length_min_N)
        {
            string equalsString, missedString;
            return getEqualsLength_checkString1ToString2(baseStr1, str2, missLength_max_M, equalStringBlock_length_min_N, false, out equalsString, out missedString);
        }



        #endregion
        #region 文字列を1行M文字の形式での整形：getStringFormat
        public static string getStringFormat(string _baseText, int _フィールド文字数_何文字になるまで空白で埋めるか, bool _isReft・左揃えか)
        {
            string _stringfield = "";
            if (_isReft・左揃えか == true)
            {
                _stringfield += _baseText;
                while (_stringfield.Length < _フィールド文字数_何文字になるまで空白で埋めるか)
                {
                    _stringfield += " ";
                }
            }
            else
            {
                while (_stringfield.Length + _baseText.Length < _フィールド文字数_何文字になるまで空白で埋めるか)
                {
                    _stringfield += " ";
                }
                _stringfield += _baseText;
            }
            return _stringfield;
        }
        /// <summary>
        /// 文字列を1行M文字で，N文字毎に半角空白が入ったフォーマットに整形したものを取得します．
        /// （例：　N=2, M=6だと，
        /// ab cd ef [改行]
        /// gh ig kl [改行]
        /// ...）
        /// </summary>
        /// <param name="_baseText"></param>
        /// <param name="N_Words_dividedBrank_Per"></param>
        /// <param name="_newLine_Per_M_Words"></param>
        /// <returns></returns>
        public static string getStringFormat__By_Per_N_Brank_And_Per_M_Line(string _baseText, int N_一固まり何文字か, int M_一行何文字か)
        {
            string changedString = "";
            int _n = N_一固まり何文字か;
            int _m = M_一行何文字か;

            // 整形のための削除
            _baseText = _baseText.Replace(" ", "");
            _baseText = _baseText.Replace(System.Environment.NewLine, "");
            _baseText = _baseText.Replace("\n", "");
            _baseText = _baseText.Replace("\t", "");



            int _line_Num = _baseText.Length / _m;
            string _lineString;
            for (int i = 0; i <= _line_Num; i++)
            {
                // 一行取り出し
                _lineString = _baseText.Substring(_m * i, Math.Min(_m, _baseText.Substring(_m * i).Length));

                // 1行中，N文字毎に区切り文字を入れる
                int _brank_Num = _lineString.Length / _n;
                for (int j = 0; j <= _brank_Num; j++)
                {
                    // 追加する文字が0個でなければ
                    if (_n * j <= _lineString.Length - 1)
                    {
                        changedString += _lineString.Substring(_n * j, Math.Min(_n, _lineString.Substring(_n * j).Length));
                    }
                    // 行末以外に空白
                    if (j != _brank_Num)
                    {
                        changedString += " ";
                    }
                }

                // m文字毎に改行を入れる
                changedString += System.Environment.NewLine; //"\n"
            }
            return changedString;

        }
        #endregion
        #region 特定の文字列で区切られたN番目の文字を取ってくる: getStringItem
        public static string getStringItem(string _itemsString・アイテム列挙文字列, string _dividedStr・区切り文字, int _N_何番目のアイテムを取ってくるか)
        {
            string[] _items = _itemsString・アイテム列挙文字列.Split(_dividedStr・区切り文字.ToCharArray());
            string _item = MyTools.getArrayValue(_items, _N_何番目のアイテムを取ってくるか - 1);
            return _item;

        }
        #endregion
        #region 整数・小数を必要桁数だけ文字列にして取得するメソッド : getAboutValue/String
        /// <summary>
        /// 整数・小数を，最小値で切り捨てた値を取得します．最小値は，0.01や10000などを入れてください．
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_minValue_kirisute"></param>
        /// <returns></returns>
        public static double getAboutValue(double _value, double _minValue_kirisute)
        {
            // 最小値が0なら変換しない
            if (_minValue_kirisute == 0)
            {
                return _value;
            }
            else
            {
                double _changedValue = (int)(_value / _minValue_kirisute) * _minValue_kirisute;
                return _changedValue;
            }

        }
        /// <summary>
        /// /// <summary>
        /// 整数・小数を，最小値で切り捨てた値を取得します．最小値は，100や10000などを入れてください．
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_minValue_kirisute"></param>
        /// <returns></returns>
        public static int getAboutValue(int _value, int _minValue_kirisute)
        {
            return (int)getAboutValue((double)_value, (double)_minValue_kirisute);
        }

        /// <summary>
        /// 数・小数を，最小値で切り捨てた文字列を取得します．最小値は，0.01や10000などを入れてください．
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_minValue_shownString"></param>
        /// <returns></returns>
        public static string getAboutString(double _value, double _minValue_shownString)
        {
            return getAboutValue(_value, _minValue_shownString).ToString();
        }
        /// <summary>
        /// /// <summary>
        /// 整数・小数を，最小値で切り捨てた値を取得します．最小値は，100や10000などを入れてください．
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_minValue_kirisute"></param>
        /// <returns></returns>
        public string getAboutString(int _value, int _minValue_shownString)
        {
            return getAboutString((double)_value, (double)_minValue_shownString).ToString();
        }
        #endregion
        #region 整数・小数を必要桁数だけ、指定空白フィールドに右詰／左詰で表示するメソッド : getShownNumber／getStringNumber
        /// <summary>
        /// 整数・小数値の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。
        /// </summary>
        public static string getStringNumber(double _n_元の数値, bool _isR_右詰表示するか, int _l1_整数部で表示したい桁数, int _l2_小数部で表示したい桁数)
        {
            return getStringNumber(_n_元の数値, _isR_右詰表示するか, true, _l1_整数部で表示したい桁数, _l2_小数部で表示したい桁数);
        }
        /// <summary>
        /// 整数・小数値の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。
        /// </summary>
        public static string getStringNumber(double _n_元の数値, bool _isR_右詰表示するか, bool _is符号がプラスでも左端1文字空けるか, int _l1_整数部で表示したい桁数, int _l2_小数部で表示したい桁数)
        {
            int _length_余白 = _l1_整数部で表示したい桁数;
            if (_l2_小数部で表示したい桁数 != 0)
            {
                _length_余白 = _l1_整数部で表示したい桁数 + _l2_小数部で表示したい桁数 + 1; // 「.」の１文字だけ
            }
            if (_is符号がプラスでも左端1文字空けるか == true)
            {
                _length_余白++; // 「-」か+の空白「 」の分
            }
            return getShownNumber(_n_元の数値, _l1_整数部で表示したい桁数, _l2_小数部で表示したい桁数, _length_余白, _isR_右詰表示するか, _is符号がプラスでも左端1文字空けるか);
        }
        /// <summary>
        /// 整数・小数値の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。
        /// </summary>
        public static string getStringNumber(double _n_元の数値, bool _isR_右詰表示するか, int _l1_整数部で表示したい桁数, int _l2_小数部で表示したい桁数, int _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨)
        {
            // 元の数値が負の数じゃなかったら、符号のマスはいらない、とする
            if (_n_元の数値 >= 0)
            {
                return getShownNumber(_n_元の数値, _l1_整数部で表示したい桁数, _l2_小数部で表示したい桁数, _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨, _isR_右詰表示するか,
                    false);
            }
            else
            {
                return getShownNumber(_n_元の数値, _l1_整数部で表示したい桁数, _l2_小数部で表示したい桁数, _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨, _isR_右詰表示するか,
                    true);
            }
        }
        /// <summary>
        /// 小数値の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。　※符号がマイナスの場合でもプラスの場合とそろえたい場合、_is符号がプラスでも左端1文字空けるか=trueにしてください。
        /// </summary>
        public static string getStringNumber(double _n_元の数値, bool _isR_右詰表示するか, bool _is符号がプラスでも1文字空けるか, int _l1_整数部で表示したい桁数, int _l2_小数部で表示したい桁数, int _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨)
        {
            return getShownNumber(_n_元の数値, _l1_整数部で表示したい桁数, _l2_小数部で表示したい桁数, _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨, _isR_右詰表示するか, _is符号がプラスでも1文字空けるか);
        }
        /// <summary>
        /// 小数値の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。　※符号がマイナスの場合でもプラスの場合とそろえたい場合、_is符号がプラスでも左端1文字空けるか=trueにしてください。
        /// </summary>
        public static string getStringNumber(long _n_元の数値, bool _isR_右詰表示するか, bool _is符号がプラスでも1文字空けるか, int _l1_整数部で表示したい桁数, int _l2_小数部で表示したい桁数, int _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨)
        {
            return getShownNumber(_n_元の数値, _l1_整数部で表示したい桁数, _l2_小数部で表示したい桁数, _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨, _isR_右詰表示するか, _is符号がプラスでも1文字空けるか);
        }
        /// <summary>
        /// 小数値の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。　※符号がマイナスの場合でもプラスの場合とそろえたい場合、_is符号がプラスでも左端1文字空けるか=trueにしてください。
        /// </summary>
        public static string getStringNumber(int _n_元の数値, bool _isR_右詰表示するか, bool _is符号がプラスでも1文字空けるか, int _l1_整数部で表示したい桁数, int _l2_小数部で表示したい桁数, int _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨)
        {
            return getShownNumber(_n_元の数値, _l1_整数部で表示したい桁数, _l2_小数部で表示したい桁数, _length_表示フィールド文字数_符号やピリオドや余白を含めた値_l1プラスl2より２文字大きい数を推奨, _isR_右詰表示するか, _is符号がプラスでも1文字空けるか);
        }
        /// <summary>
        /// ※getStringNumberと機能は同じです。
        /// 小数値の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。
        /// ※このメソッドは、符号がプラスの場合は符号用左端空白なし（isSignSpaceNeeded=false)、符号がマイナスの場合は空白あり（true）になります。
        /// </summary>
        /// <param name="baseNumber">元の数値</param>
        /// <param name="shownNumber_Seisu_Length">表示したい桁数（整数部）</param>
        /// <param name="shownNumber_Syousu_Length">表示したい桁数（小数部）</param>
        /// <param name="spaceBlank_Length">数値を表示する余白フィールドの最大文字数（小数点の"."と、符号の"-"のスペースを含めて、2 + Seisu_Length + Syousu_Length以上の値を推奨）</param>
        /// <param name="isRightSide">右詰</param>
        /// <param name="isSignSpaceNeeded">符号がマイナスの場合でもプラスの場合と桁数をそろえたい場合、true</param>
        /// <returns></returns>
        public static string getShownNumber(double baseNumber, int shownNumber_Seisu_Length, int shownNumber_Syousu_Length, int spaceBlank_Length, bool isRightSide)
        {
            // 元の数値の符号がプラスだったら、符号のマスはいらない、とする
            if (baseNumber >= 0)
            {
                return getShownNumber(baseNumber, shownNumber_Seisu_Length, shownNumber_Syousu_Length, spaceBlank_Length, isRightSide,
                    false);
            }
            else
            {
                return getShownNumber(baseNumber, shownNumber_Seisu_Length, shownNumber_Syousu_Length, spaceBlank_Length, isRightSide,
                    true);
            }

        }
        /// <summary>
        /// ※getStringNumberと機能は同じです。
        /// 小数値を整数値として、任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。
        /// ※このメソッドは、符号がプラスの場合は符号用左端空白なし（isSignSpaceNeeded=false)、符号がマイナスの場合は空白あり（true）になります。
        /// </summary>
        /// <param name="baseNumber">元の数値</param>
        /// <param name="shownNumber_Seisu_Length">表示したい桁数（整数部）</param>
        /// <param name="shownNumber_Syousu_Length">表示したい桁数（小数部）</param>
        /// <param name="spaceBlank_Length">数値を表示する余白フィールドの最大文字数（小数点の"."と、符号の"-"のスペースを含めて、2 + Seisu_Length + Syousu_Length以上の値を推奨）</param>
        /// <param name="isRightSide">右詰</param>
        /// <param name="isSignSpaceNeeded">符号がマイナスの場合でもプラスの場合と桁数をそろえたい場合、true</param>
        /// <returns></returns>
        public static string getShownNumber(double baseNumber, int shownNumber_Seisu_Length, bool isRightSide)
        {
            // 元の数値の符号がプラスだったら、符号のマスはいらない、とする
            if (baseNumber >= 0)
            {
                return getShownNumber(baseNumber, shownNumber_Seisu_Length, 0, shownNumber_Seisu_Length, isRightSide,
                    false);
            }
            else
            {
                return getShownNumber(baseNumber, shownNumber_Seisu_Length, 0, shownNumber_Seisu_Length + 1, isRightSide,
                    true);
            }

        }
        /// <summary>
        /// ※getStringNumberと機能は同じです。
        /// 小数値の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。　※符号がマイナスの場合でもプラスの場合とそろえたい場合、isSignSpaceNeeded=trueにしてください。
        /// </summary>
        /// <param name="baseNumber">元の数値</param>
        /// <param name="shownNumber_Seisu_Length">表示したい桁数（整数部）</param>
        /// <param name="shownNumber_Syousu_Length">表示したい桁数（小数部）</param>
        /// <param name="spaceBlank_Length">数値を表示する余白フィールドの最大文字数（小数点の"."と、符号の"-"のスペースを含めて、2 + Seisu_Length + Syousu_Length以上の値を推奨）</param>
        /// <param name="isRightSide">右詰</param>
        /// <param name="isSignSpaceNeeded">符号がマイナスの場合でもプラスの場合と桁数をそろえたい場合、true</param>
        /// <returns></returns>
        public static string getShownNumber(double baseNumber, int shownNumber_Seisu_Length, int shownNumber_Syousu_Length, int spaceBlank_Length, bool isRightSide, bool isSignSpaceNeeded)
        {
            // 文字列
            string strShownNumber = "";

            // 1.左右そろえ
            int right = 1; // rightが1だと右詰，-1だと左詰
            if (isRightSide == false)
            {
                right = -1;
            }

            // 2.整数部分の作成と「.」
            int seisuNumber = (int)baseNumber;
            // 整数部分を繋げる
            strShownNumber = seisuNumber.ToString();
            // 小数部分がいるかどうか調べる
            string syousyuNumberString = "";
            if (shownNumber_Syousu_Length != 0)
            {
                // 小数部分が無くて、小数部分の表示が必要な場合は、「.」を後ろに追加
                strShownNumber += ".";
            }


            // 3.小数部分の作成（これが結構ややこしい…）
            // 「-0.***」の***の部分だけ取得（小数部分が無い場合は""）
            // 小数部分が多すぎると表示時に困るので、とりあえず切り捨て
            double kirisuteNum = Math.Pow(10, shownNumber_Syousu_Length); // 小数部分を有効数字桁数だけ整数にするために、かける値
            double kirisuteBaseNumber = (int)(baseNumber * kirisuteNum) / kirisuteNum;
            // 例：元の数値「99.00000123056...」を、seisu_Lengh=2、syousu_Length=9、で表示するとき
            // 小数部分が小さすぎると、例えばToString()が「0.000001230..」が「1.230....E-6」とかになる可能性があるから、
            // まず、syousyuNumberには「1234.000..」などの小数部分を整数にした値を格納する
            int syousyuNumber = (int)(Math.Pow(10, shownNumber_Syousu_Length) * (kirisuteBaseNumber - (double)seisuNumber));
            // 小数部分の、前の0を補完
            string forward_0 = "";
            if(syousyuNumber != 0){
                // 例：ただし、表示したい小数部分は「000001234」なので、
                // 「1234」の整数部分の前に、足りない「0」を前に追加する

                // 例：10^9=「1000000000」に「1234」を足した、「1000001234」という数値を作る
                int tempNumber = syousyuNumber + (int)Math.Pow(10, shownNumber_Syousu_Length);
                // 例：「1000001234」の「0」を、前から2桁目から順に、0じゃなくなるまで、前に追加
                string temp_zero = "";
                int i=0;
                while(tempNumber.ToString().Length >= 2 && forward_0.Length < shownNumber_Syousu_Length) // 二個目の条件は保険
                {
                    temp_zero = tempNumber.ToString().Substring(1, 1);
                    if (temp_zero == "0")
                    {
                        forward_0 += "0";
                    }
                    else
                    {
                        break;
                    }
                    // 左側を1桁減らす。例：「1000001234」→「100001234」→…→「101234」→「11234」で終了
                    tempNumber = tempNumber - 9 * (int)Math.Pow(10, shownNumber_Syousu_Length - 1 - i);
                    i++;
                }
                // 例：「000001234」の部分を追加
                syousyuNumberString = forward_0 + syousyuNumber.ToString();
            }
            // 小数部分をつなげる
            strShownNumber += syousyuNumberString;
            // 少数部分、後ろの0を補完
            if (shownNumber_Syousu_Length != 0)
            {
                // 「***.***」以降に足りない「0」後ろに追加
                for (int i = 0; i < shownNumber_Syousu_Length - syousyuNumberString.Length; i++) // +1は「.」の１文字
                {
                    strShownNumber += "0";
                }
            }
            // やっと小数部分の完了


            // 4.符号部分の表示
            // +符号部分を冒頭に追加（-とそろえるため）
            if (isSignSpaceNeeded == true && baseNumber >= 0)
            {
                strShownNumber = " " + strShownNumber;
            }


            // 5.入りきらない値の表示
            // 値の整数部分が，指定した桁数より大きい場合は"OVER"と表示
            if (strShownNumber.Length > spaceBlank_Length)
            {
                strShownNumber = "OVER";
            }

            // 6. 表示する文字列を決定！

            //String.Format{変数代入インデックス番号,文字列の幅(-は左揃え):書式指定子(省略は空白，D*は10進数の'0'が*個)}
            string shownString = String.Format("{0, " + right * spaceBlank_Length + "}", strShownNumber);
            return shownString;
        }
        // 整数版は処理を高速化したものを別で作る
        /// <summary>
        /// ※getStringNumberと機能は同じです。
        /// 整数の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。　※符号がマイナスの場合でもプラスの場合とそろえたい場合、isSignSpaceNeeded=trueにしてください。
        /// </summary>
        /// <param name="baseNumber">元の数値</param>
        /// <param name="shownNumber_Seisu_Length">表示したい桁数（整数部）</param>
        /// <param name="shownNumber_Syousu_Length">表示したい桁数（小数部）</param>
        /// <param name="spaceBlank_Length">数値を表示する余白フィールドの最大文字数（符号の"-"のスペースを含めて、1 + Seisu_Length以上の値を推奨）</param>
        /// <param name="isRightSide">右詰</param>
        /// <param name="isSignSpaceNeeded">符号がマイナスの場合でもプラスの場合と桁数をそろえたい場合、true</param>
        /// <returns></returns>
        public static string getShownNumber(long baseNumber, int shownNumber_Seisu_Length, int spaceBlank_Length, bool isRightSide, bool isSignSpaceNeeded)
        {
            int right = 1; // rightが1だと右詰，-1だと左詰
            if (isRightSide == false)
            {
                right = -1;
            }
            string strShownNumber = baseNumber.ToString();
            // 4.符号部分の表示
            // +符号部分を冒頭に追加（-とそろえるため）
            if (isSignSpaceNeeded == true && baseNumber >= 0)
            {
                strShownNumber = " " + strShownNumber;
                // 符号部分も抽出する文字数に含める
                shownNumber_Seisu_Length++;
            }
            else if (baseNumber < 0)
            {
                // 符号部分も抽出する文字数に含める
                shownNumber_Seisu_Length++;
            }
            // 値の文字列（符号＋桁数）が，指定した符号＋桁数より大きい場合は"OVER"と表示
            if (baseNumber.ToString().Length > shownNumber_Seisu_Length)
            {
                strShownNumber = "OVER";
            }
            //String.Format{変数代入インデックス番号,文字列の幅(-は左揃え):書式指定子(省略は空白，D*は10進数の'0'が*個)}
            string shownNumber = String.Format("{0, " + right * spaceBlank_Length + "}", strShownNumber.Substring(0, Math.Min(shownNumber_Seisu_Length, strShownNumber.Length)));
            return shownNumber;
        }
        /// <summary>
        /// ※getStringNumberと機能は同じです。
        /// 整数の任意の桁数部分だけを右詰／左詰フォーマットで文字列を取得します。　※符号がマイナスの場合でもプラスの場合とそろえたい場合、isSignSpaceNeeded=trueにしてください。
        /// </summary>
        /// <param name="baseNumber">元の数値</param>
        /// <param name="shownNumber_Seisu_Length">表示したい桁数（整数部）</param>
        /// <param name="shownNumber_Syousu_Length">表示したい桁数（小数部）</param>
        /// <param name="spaceBlank_Length">数値を表示する余白フィールドの最大文字数（符号の"-"のスペースを含めて、1 + Seisu_Length以上の値を推奨）</param>
        /// <param name="isRightSide">右詰</param>
        /// <param name="isSignSpaceNeeded">符号がマイナスの場合でもプラスの場合と桁数をそろえたい場合、true</param>
        /// <returns></returns>
        public static string getShownNumber(int baseNumber, int shownNumber_Seisu_Length, bool isRightSide, bool isSignSpaceNeeded)
        {
            return getShownNumber((long)baseNumber, shownNumber_Seisu_Length, shownNumber_Seisu_Length+1, isRightSide, isSignSpaceNeeded);
        }
        #endregion
        #region string型の文字列に指定文字が何個入っているかを返す: getStringCount/getStringCountChar
        /// <summary>
        /// string型の文字列に指定文字列が何個入っているかを返します。
        /// </summary>
        /// <param name="_baseString"></param>
        /// <param name="_searchString">指定文字列</param>
        /// <returns></returns>
        public static int getStringCount(string _baseString, string _searchString)
        {
            // 文字の出現回数をカウント。
            // Replaceメソッドを用いて、カウントする文字を消去した文字列と、元の文字列との長さの差により、高速に文字数のカウントが可能
            return (_baseString.Length - _baseString.Replace(_searchString, "").Length) / _searchString.Length;
        }
        /// <summary>
        /// string型の文字列に指定文字が何個入っているかを返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <param name="_char">指定文字（'a'や'\n'などの表記で指定）</param>
        /// <returns></returns>
        public static int getStringCountChar(string _string, char _char){
            // 文字の出現回数をカウント。
            // Replaceメソッドを用いて、カウントする文字を消去した文字列と、元の文字列との長さの差により、高速に文字数のカウントが可能
            return _string.Length - _string.Replace(_char.ToString(), "").Length;
        }
        #endregion
        #region 改行文字'\n'で区切られたstring型文字列が何行目かを返す: getLineNo
        /// <summary>
        /// 改行文字'\n'で区切られたstring型の文字列の最後尾が、何行目かを返します。
        /// （例： getLineNo("")=0、getLineNo("おはよう")=1、getLineNo("おはよう\n")=2、getLineNo("おはよう\nいい天気だね")=2、getLineNo("おはよう\nいい天気だね\n今日は何をして遊ぼうか？")=3）
        /// </summary>
        /// <param name="_stringLines_PerLineDivided_LineChar"></param>
        /// <returns></returns>
        public static int getLineNo(string _stringLines_PerLineDivided_LineChar)
        {
            return getLineNo(_stringLines_PerLineDivided_LineChar, _stringLines_PerLineDivided_LineChar.Length - 1);
        }
        /// <summary>
        /// 改行文字'\n'で行が区切られたstring型の文字列の中で、文字列[指定インデックス]が、何行目かにあたるかを返します。
        /// （例： getLineNo("")=0、getLineNo("おはよう")=1、getLineNo("おはよう\n")=2、getLineNo("おはよう\nいい天気だね")=2、getLineNo("おはよう\nいい天気だね\n今日は何をして遊ぼうか？")=3）
        /// </summary>
        /// <param name="_stringLines_PerLineDivided_LineChar"></param>
        /// <returns></returns>
        public static int getLineNo(string _stringLines_PerLineDivided_LineChar, int _index)
        {
            // エラーの時は0行目
            if(_stringLines_PerLineDivided_LineChar == null || _stringLines_PerLineDivided_LineChar == "") return 0;
            if (_index < 0 || _index > _stringLines_PerLineDivided_LineChar.Length - 1) return 0;

            // 文字列の場所によって、"1行目 \n 2行目 \n 3行目 \n 4行目…"。要するに、それまで出てきた'\n'の数+1行目
            // それまで出てきた文字列の取得
            string _nowString = _stringLines_PerLineDivided_LineChar.Substring(0, _index+1);

            char _LineChar = '\n';
            int _count = getStringCountChar(_nowString, _LineChar);
            int _line = _count + 1; // 改行文字が0個あったら1行目、改行文字が1個あったら2行目、…
            return _line;
        }
        #endregion
        #region 改行文字'\n'で区切られたstring型文字列の指定行目だけを取ってくる: getLineString
        /// <summary>
        /// 改行文字'\n'で区切られたstring型文字列の指定行目だけを取ってきます。エラーの場合は""を返します。
        /// （例： getLineString(***, 0)=""、getLineString("おはよう\nいい天気だね", 1)="おはよう"、getLineString("おはよう\nいい天気だね", 2)="いい天気だね"、getLineString("おはよう\nいい天気だね\nえ…曇りだって？", 3)="え…曇りだって？"）
        /// </summary>
        /// <param name="_stringLines_PerLineDivided_LineChar"></param>
        /// <param name="_lineNo_1ToN">行番号。1以上。0だと""を返します。</param>
        /// <returns></returns>
        public static string getLineString(string _stringLines_PerLineDivided_LineChar, int _lineNo_1ToN)
        {
            char _LineChar = '\n';

            string _lineString = "";
            // エラーの場合は""
            if (_stringLines_PerLineDivided_LineChar == null || _stringLines_PerLineDivided_LineChar == "") return _lineString;
            int _lineMax = MyTools.getLineNo(_stringLines_PerLineDivided_LineChar);
            if (_lineNo_1ToN < 1 || _lineNo_1ToN > _lineMax) return _lineString;

            // 文字列の場所によって、"1行目 \n 2行目 \n 3行目 \n 4行目…"。要するに、それまで出てきた'\n'の数+1行目
            // すなわち、(a)_lineNo行目が欲しい　→　(_lineNo - 1)番目に見つかった'\n'のインデックス+1 ～ _lineNo番目に見つかった'\n'のインデックス-1　の文字列
            // または、  (b)_lineNo行目が欲しい　→　文字列.Split('\n')で区切った items[_lineNo-1]
            // メモリを食わないのは(a)だろうけど。どっちが高速だろうね。
            // _lineNo番目に見つかった改行文字
            string[] _items = _stringLines_PerLineDivided_LineChar.Split(_LineChar);
            _lineString = _items[_lineNo_1ToN - 1];
            return _lineString;
        }
        #endregion
        #region  改行文字'\n'で区切られたstring型文字列の更新: getStringLines_Updated
        /// <summary>
        /// 改行文字'\n'で区切られたstring型文字列の、１行目を削除して、新しい行を追加した、更新後の文字列を返します。エラーの場合は_newLineStringだけを返します。
        /// 
        /// 　　※表示する行が固定されているメッセージボックス更新などに便利です。
        /// （例： getStringLines_Updated("おはよう\n今日は", "いい天気だね")="今日は\nいい天気だね"）
        /// </summary>
        /// <param name="_stringLines_PerLineDivided_LineChar"></param>
        /// <param name="_newLineString"></param>
        /// <returns></returns>
        public static string getStringLines_Updated(string _stringLines_PerLineDivided_LineChar, string _newLineString)
        {
            char _LineChar = '\n';
            // エラーの時は_newStringを返す
            string _updatedString = "";
            if(_stringLines_PerLineDivided_LineChar == null || _stringLines_PerLineDivided_LineChar == "") return _newLineString;
            int _line2Index = _stringLines_PerLineDivided_LineChar.IndexOf(_LineChar);
            if(_line2Index == -1) return _newLineString;

            // 2行目～最後尾に、改行文字＋新行_newLineStringを付け足した文字列を返す
            _updatedString = _stringLines_PerLineDivided_LineChar.Substring(_line2Index + 1) + "\n" + _newLineString;
            return _updatedString;
        }
        #endregion
        #region string型の文字列を（変換可能なものだけ）int/double型に変換して返す: parseInt/parseDouble
        /// <summary>
        /// string型の文字列を（変換可能なものだけ）int型の値に変換して返します。TryParseで変換不可能な場合（数値ではない文字列が含まれていた場合），0を返します．
        /// </summary>
        public static int parseInt(string _strValue)
        {
            int _list = 0;
            if (Int32.TryParse(_strValue, out _list) == false)
            {
                _list = 0;
            }
            return _list;
        }
        /// <summary>
        /// string型の文字列を（変換可能なものだけ）double型の値に変換して返します。TryParseで変換不可能な場合（数値ではない文字列が含まれていた場合），0.0を返します．
        /// </summary>
        public static double parseDouble(string _strValue)
        {
            double _list = 0;
            if (Double.TryParse(_strValue, out _list) == false)
            {
                _list = 0;
            }
            return _list;
        }
        /// <summary>
        /// string型のリストの値を（変換可能なものだけ）int型のリストに変換して返します。第２引数では変換不可能な文字を値「0」としてリストの要素に含めるかを設定します．
        /// </summary>
        /// <param name="valueLists"></param>
        /// <returns></returns>
        public static List<int> parseInt(List<string> _stringLists, bool _isIncluding_CanNotParsedReplacingLists)
        {
            List<int> _list = new List<int>(_stringLists.Count);
            int _temp;
            for (int i = 0; i < _stringLists.Count; i++)
            {
                if (Int32.TryParse(_stringLists[i], out _temp) == false)
                {
                    if (_isIncluding_CanNotParsedReplacingLists == true)
                    {
                        _list.Add(0);
                    }
                }
                else
                {
                    _list.Add(_temp);
                }
            }
            return _list;
        }
        /// <summary>
        /// string型の配列の値を（変換可能なものだけ）int型の配列に変換して返します。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <returns></returns>
        public static int[] parseInt(string[] _stringArray)
        {
            int[] _list = new int[_stringArray.Length];
            for (int i = 0; i < _stringArray.Length; i++)
            {
                _list[i] = parseInt(_stringArray[i]);
            }
            return _list;
        }
        /// <summary>
        /// string型のリストの値を（変換可能なものだけ）double型のリストに変換して返します。第２引数では変換不可能な文字を値「0.0」としてリストの要素に含めるかを設定します．
        /// </summary>
        /// <param name="valueLists"></param>
        /// <returns></returns>
        public static List<double> parseDouble(List<string> _stringLists, bool _isIncluding_NotParsedLists)
        {
            List<double> _list = new List<double>(_stringLists.Count);
            double _temp;
            for (int i = 0; i < _stringLists.Count; i++)
            {
                if (Double.TryParse(_stringLists[i], out _temp) == false)
                {
                    if (_isIncluding_NotParsedLists == true)
                    {
                        _list.Add(0.0);
                    }
                }
                else
                {
                    _list.Add(_temp);
                }
            }
            return _list;
        }
        /// <summary>
        /// string型の配列の値を（変換可能なものだけ）double型の配列に変換して返します。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <returns></returns>
        public static double[] parseDouble(string[] _stringArray)
        {
            double[] _list = new double[_stringArray.Length];
            for (int i = 0; i < _stringArray.Length; i++)
            {
                _list[i] = parseDouble(_stringArray[i]);
            }
            return _list;
        }
        #endregion
        #region int[]型配列／リストをdouble[]型配列／リストに変換するメソッド: parseDouble
        /// <summary>
        /// int[]型配列をdouble[]型配列に変換したものを、新しく生成して返します。※現段階では，元のint型配列を削除しません。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <returns></returns>
        public static double[] parseDouble(int[] valueLists)
        {
            double[] value_double = new double[valueLists.Length];
            for (int i = 0; i < valueLists.Length; i++)
            {
                value_double[i] = (double)valueLists[i];
            }
            return value_double;
        }
        /// <summary>
        /// int型リストをdouble型リストに変換したものを、新しく生成して返します。※現段階では，元のint型リストを削除しません。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <returns></returns>
        public static List<double> parseDouble_Lists(List<int> valueLists)
        {
            List<double> value_double = new List<double>(valueLists.Count);
            for (int i = 0; i < valueLists.Count; i++)
            {
                value_double.Add((double)valueLists[i]);
            }
            // valueLists.Clear(); // ※現段階では，元のint型リストを削除しない
            return value_double;
        }
        /// <summary>
        /// int[]型配列をdouble型リストに変換したものを、新しく生成して返します。※現段階では，元のint型配列を削除しません。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <returns></returns>
        public static List<double> parseDouble_Lists(int[] valueLists)
        {
            List<double> value_double = new List<double>(valueLists.Length);
            for (int i = 0; i < valueLists.Length; i++)
            {
                value_double.Add((double)valueLists[i]);
            }
            // valueLists.Clear(); // ※現段階では，元のint型リストを削除しない
            return value_double;
        }
        /// <summary>
        /// int型リストをdouble[]型に変換したものを、新しく生成して返します。※現段階では，元のint型リストを削除しません。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <returns></returns>
        public static double[] parseDouble(List<int> valueLists)
        {
            double[] value_double = new double[valueLists.Count];
            for (int i = 0; i < valueLists.Count; i++)
            {
                value_double[i] = (double)valueLists[i];
            }
            return value_double;
        }
        #endregion



        // CVS形式関連
        #region string型のCSV形式から，任意の列の値を取ってくる
        /// <summary>
        /// string型のCSV形式（一行目が「行説明タイトルラベル0,変数1のラベル,変数2のラベル,...,\n」で、二行目以降が「変数0,変数1,変数2,...,\n」）のカンマ区切りの文字列から，任意の列の値を取ってくる
        /// </summary>
        /// <param name="_CVSString"></param>
        /// <param name="_gotItemIndex"></param>
        /// <returns></returns>
        public static string getItem_FromCVS(string _CVSString, int _gotItemIndex)
        {
            string[] _items = _CVSString.Split();
            return MyTools.getListValue(new List<string>(_items), _gotItemIndex);
        }
        #endregion
        #region リスト・配列内の中身がある要素のカンマ区切りCSV・ライン行の文字列生成: toStringLines
        /// <summary>
        /// 引数の配列変数の文字列を改行で区切った文字列で返します．第２引数で配列内の中身がない要素（""）を含めるかを指定してください。
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string toStringLines_Indexes(List<string> var, bool isZeroValue_Included)
        {
            string toString = "";
            string index = "";
            for (int i = 0; i < var.Count; i++)
            {
                index = var[i];
                if (isZeroValue_Included == true || (isZeroValue_Included == false && index != ""))
                {
                    toString += index;
                    if (i != var.Count - 1)
                    {
                        toString += System.Environment.NewLine;
                    }
                }
            }
            return toString;
        }
        /// <summary>
        /// 引数の配列変数の値をカンマで区切った文字列で返します．第２引数で配列内の中身がない要素（""）を含めるかを指定してください。
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string toStringCSV_Indexes(List<string> var, bool isZeroValue_Included)
        {
            string toString = "";
            string index = "";
            for (int i = 0; i < var.Count; i++)
            {
                index = var[i];
                if (isZeroValue_Included == true || (isZeroValue_Included == false && index != ""))
                {
                    toString += index;
                    if (i != var.Count - 1)
                    {
                        toString += ",";
                    }
                }
            }
            return toString;
        }
        /// <summary>
        /// 引数の配列変数の値をカンマで区切った文字列で返します．第２引数で配列内の中身がない要素（0や""）を含めるかを指定してください。
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string toStringCSV_Indexes(List<double> var, bool isZeroValue_Included)
        {
            string toString = "";
            double index = 0.0;
            for (int i = 0; i < var.Count; i++)
            {
                index = var[i];
                if (isZeroValue_Included == true || (isZeroValue_Included == false && index != 0.0))
                {

                    toString += index.ToString();
                    if (i != var.Count - 1)
                    {
                        toString += ",";
                    }
                }
            }
            return toString;
        }
        /// <summary>
        /// 引数の配列変数の値をカンマで区切った文字列で返します．第２引数で配列内の中身がない要素（0や""）を含めるかを指定してください。
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string toStringCSV_Indexes(List<int> var, bool isZeroValue_Included)
        {
            string toString = "";
            int index = 0;
            for (int i = 0; i < var.Count; i++)
            {
                index = var[i];
                if (isZeroValue_Included == true || (isZeroValue_Included == false && index != 0))
                {
                    toString += index.ToString();
                    if (i != var.Count - 1)
                    {
                        toString += ",";
                    }
                }
            }
            return toString;
        }
        /// <summary>
        /// 引数の配列変数の値をカンマで区切った文字列で返します．第２引数で配列内の中身がない要素（0や""）を含めるかを指定してください。
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string toStringCSV_Indexes(double[] var, bool isZeroValue_Included)
        {
            string toString = "";
            double index = 0.0;
            for (int i = 0; i < var.Length; i++)
            {
                index = var[i];
                if (isZeroValue_Included == true || (isZeroValue_Included == false && index != 0.0))
                {
                    toString += index.ToString();
                    if (i != var.Length - 1)
                    {
                        toString += ",";
                    }
                }
            }
            return toString;
        }
        /// <summary>
        /// 引数の配列変数の値をカンマで区切った文字列で返します．第２引数で配列内の中身がない要素（0や""）を含めるかを指定してください。
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string toStringCSV_Indexes(int[] var, bool isZeroValue_Included)
        {
            string toString = "";
            int index = 0;
            for (int i = 0; i < var.Length; i++)
            {
                index = var[i];
                if (isZeroValue_Included == true || (isZeroValue_Included == false && index != 0))
                {
                    toString += index.ToString();
                    if (i != var.Length - 1)
                    {
                        toString += ",";
                    }
                }
            }
            return toString;
        }
        /// <summary>
        /// 引数の配列変数の値をカンマで区切った文字列で返します．第２引数で配列内の中身がない要素（0や""）を含めるかを指定してください。
        /// </summary>
        /// <param name="_var"></param>
        /// <param name="_isDefaultValue_Included"></param>
        /// <returns></returns>
        public static string toStringCSV_Indexes(string[] var, bool isZeroValue_Included)
        {
            string toString = "";
            string index = "";
            for (int i = 0; i < var.Length; i++)
            {
                index = var[i];
                if (isZeroValue_Included == true || (isZeroValue_Included == false && index != ""))
                {
                    toString += index;
                    if (i != var.Length - 1)
                    {
                        toString += ",";
                    }
                }
            }
            return toString;
        }
        #endregion
        #region 任意の型のリストの値をCVS形式にして返す: getListValues_ToCSV
        /// <summary>
        /// 引数のリストの項目を","で区切ったCSVファイルを返します。
        /// </summary>
        /// <param name="_items"></param>
        /// <returns></returns>
        public static string getCSVLineString<T>(List<T> _items)
        {
            return getListValues_ToString_DividedAnyWord(_items, true, ",");
        }
        /// <summary>
        /// 引数のリストの項目を","で区切ったCSVファイルを返します。
        /// </summary>
        /// <param name="_items"></param>
        /// <returns></returns>
        public static string getCSVLineString<T>(params T[] _items)
        {
            List<T> _list = new List<T>(_items);
            return getCSVLineString(_list);
        }
        /// <summary>
        /// 任意の型のリストの値をstring型のCSV形式（一行目が「行説明タイトルラベル0,変数1のラベル,変数2のラベル,...,\n」で、二行目以降が「変数0,変数1,変数2,...,\n」）のカンマ区切りの文字列を，インデックス0を行説明行説明タイトルラベルとして参照渡しで格納し，インデックス1以降の値を（変換可能なものだけ）にして返します．第２引数で配列内の中身がない要素（0や""やnull）を含めるかを指定してください。なお，リスト[インデックス]のデータを調べ，インデックスが不正値((リスト数-1)以上やマイナス)だった場合は，その型のデフォルト値（値型なら0や""，クラス型ならnull）を返します．
        /// </summary>
        public static string getListValues_ToCSVLine<T>(string _titleLabel_OrNull, List<T> _var, bool _isDefaultValue_Included)
        {
            string _dividedWord_Between_values = ","; // 区切り文字
            string _title = "";
            if (_titleLabel_OrNull != null)
            {
                _title = _titleLabel_OrNull + ",";
            }
            return _title + getListValues_ToString_DividedAnyWord<T>(_var, _isDefaultValue_Included, _dividedWord_Between_values);
        }
        #endregion
        #region string型のCSV形式の文字列を（変換可能なものだけ）int/double型のリストに変換して返す: getListValues_FromCSVLine
        /// <summary>
        /// string型のCSV形式（「変数0,変数1,変数2,...,\n」）のカンマ区切り文字列）の値を（変換可能なものだけ）int型のリストに変換して返します。
        /// </summary>
        public static List<int> getListValues_FromCSVLine(string _CSVLineString)
        {
            string _firstLabel = "";
            return getListValues_FromCSVLine(_CSVLineString, true, out _firstLabel);
        }
        /// <summary>
        /// string型のCSV形式（一行目が「行説明タイトルラベル0,変数1のラベル,変数2のラベル,...,\n」で、二行目以降が「変数0,変数1,変数2,...,\n」）のカンマ区切りの文字列を，一行目（インデックス0）を行説明行説明タイトルラベルとして参照渡しで格納し，二行目（インデックス1）以降の値を（変換可能なものだけ）int型のリストに変換して返します。第２引数では変換不可能な文字を値「0」としてリストの要素に含めるかを設定します．第３引数では一行目の最初のラベル（例では「行説明タイトルラベル0」）を格納します。
        /// </summary>
        public static List<int> getListValues_FromCSVLine(string _CSVLineString, bool _isIncluding_NotParsedLists, out string _titleLabel)
        {
            _titleLabel = default(string);
            string[] _words = _CSVLineString.Split(",".ToCharArray());
            if (_words != null)
            {
                if (_words.Length > 0)
                {
                    _titleLabel = _words[0];
                }
            }
            List<string> _list = new List<string>(_words);
            _list.Remove(_words[0]);
            return parseInt(_list, _isIncluding_NotParsedLists);
        }
        #endregion
        #region 2つのCSVデータの同じ値だけの省略:getReplaceCSV
        /// <summary>
        /// 2つのある区切り文字（例：","など）のあるCSVデータを比較し，csvData1とcsvData2の同項目で同じ値の部分を，指定した文字列(例えば"〃"など)に置き換えた，CSVData文字列を返します．
        /// </summary>
        /// <param name="_cvsCharacter_DividedData">","などのCSVデータ区切り文字</param>
        /// <param name="_cvsData1_Replaced">同値を置き換えるCSVデータ</param>
        /// <param name="_cvsData2_Compared">比較するCSVデータ</param>
        /// <param name="_replaceWord">置き換える文字列</param>
        /// <returns>置き換えたCSV形式の文字列</returns>
        public static string getReplaceCSV_SameValueToWordA(string _csvCharacter_DividedString, string _csvData1_Replaced, string _csvData2_Compared, string _replaceWordA)
        {
            // 各データを取り出す
            string[] _csvDatas1 = _csvData1_Replaced.Split(_csvCharacter_DividedString.ToCharArray());
            string[] _csvDatas2 = _csvData2_Compared.Split(_csvCharacter_DividedString.ToCharArray());

            for (int i = 0; i < _csvDatas1.Length; i++)
            {
                // Data2のデータ数の方が少なかったら終了
                if (i >= _csvDatas2.Length)
                {
                    break;
                }
                if (_csvDatas1[i] == _csvDatas2[i])
                {
                    // 同じ値を置き換え
                    _csvDatas1[i] = _replaceWordA;
                }
            }
            // CVS文字列に結合
            string replacedCVSData = toStringCSV_Indexes(_csvDatas1, true);
            return replacedCVSData;
        }
        #endregion
        


        #region 円、線の集合の取得、円・線・点の描画

        // 点の集合の取得
        /// <summary>
        /// 中心座標と半径から円周上の点を返します。
        /// </summary>
        /// <param name="middlePoint"></param>
        /// <param name="distance__Radius"></param>
        /// <returns></returns>
        public static List<Point> getCirclePoints(Point middlePoint, double distance__Radius)
        {
            // otherControlの位置から閾値距離範囲の円周の点を求める関数は、
            #region 円周の点の求め方 http://r2cedar.cocolog-nifty.com/blog/2008/02/c_543e.html
            /*円の方程式は　「ｘ＊ｘ＋ｙ＊ｙ＝ｒ＊ｒ」なので、
　            ｘ＝±√（ｒ・ｒ－ｙ・ｙ）
　            ｙ＝±√（ｒ・ｒ－ｘ・ｘ）

            でも、これをそのまま使うと、上と下の方でｙの密度が高くなって、逆に真中付近ではｙが拡散してしまうので、別の計算方法を考える必要がありそう。と、いうことで、

            △ＡＢＣで、Ａの角度をθ、Ｂの角度を９０°とする直角三角形を見ると、
            ｓｉｎθ＝（ＢＣ）÷（ＣＡ）、ｃｏｓθ＝（ＡＢ）÷（ＣＡ）
　　　　　　　　　　　　　            ↓
            ｓｉｎθ＝ｙ÷ｒ、ｃｏｓθ＝ｘ÷ｒ　⇒　ｙ＝ｒ・ｓｉｎθ、ｘ＝ｒ・ｃｏｓθ
            このθをπで表し、３６０°＝２π。
            これを細かく分割して、その時のｘとｙの値を上記の換算式で計算し、点を算出する。
            それぞれの点を始点と終点として線を書けば、円に類似した多角形が描画できることになります。
            ８０分割すれば、きれいな円として描けるようです。
            =======
            C# 点で円を描く
            きれいな円が描ける様になりました。ソースは下記のように記述しています。

            private void pictureBox1_Paint(object sender, PaintEventArgs _e)
            {
　　            Bitmap Bmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
　　            int x, y, r;

　　            x = 0; y = 0; r = 150;
　　            this.circle(ref Bmap, x, y, r, Color.Yellow);
　　            x = 50; y = 50; r = 100;
　　            this.circle(ref Bmap, x, y, r, Color.Red);
　　            x = -50; y = -50; r = 100;
　　            this.circle(ref Bmap, x, y, r, Color.Blue);
　　            x = -50; y = 50; r = 100;
　　            this.circle(ref Bmap, x, y, r, Color.Green);
　　            x = 50; y = -50; r = 100;
　　            this.circle(ref Bmap, x, y, r, Color.Brown);

　　            pictureBox1.Image = Bmap;
            }

            private void circle(ref Bitmap bm, int x, int y, int r, Color c)
            {
　　            double ix, iy, E;

　　            ix = r; iy = 0; E = 1.0 / r;

　　            do
　　            {
　　　　            ix = ix - E * iy;
　　　　            iy = E * ix + iy;
　　　　            this.pset(ref bm, (int)ix+x, (int)iy+y, c);
　　            } while (Math.Abs(ix - r) > .5 || Math.Abs(iy) > .5);
            }

            sinθ
            cosθ　のθを０に限りなく近づくくらい小さい値を設定して、
            sinθ≒ε　cosθ≒１　になるとすれば、
            ｘ｛ｎ＋１｝＝ｘ｛ｎ｝・cosθ－ｙ｛ｎ｝・sinθ
            ｙ｛ｎ＋１｝＝ｘ｛ｎ｝・sinθ＋ｙ｛ｎ｝・cosθ　は、下記のように表せます
　　　　　　            ↓
            ｘ｛ｎ＋１｝＝ｘ｛ｎ｝－ε・ｙ｛ｎ｝
            ｙ｛ｎ＋１｝＝ε・ｘ｛ｎ｝＋ｙ｛ｎ｝　　…ｎは移動前の点、ｎ＋１は移動後の点
            ただこれでは、ｙ方向に大きく動きすぎるため、補正をかけて、すこし小さくすることにします。
             ｙ｛ｎ＋１｝＝ε・ｘ｛ｎ＋１｝＋ｙ｛ｎ｝

            また、εの値は、円周２πｒ（ドット）を埋めるために必要な角度、
            ２π（ラジアン＝３６０°）÷２πｒ（円周）で、１／ｒを使用すると良いようです。

            ということで、めでたく円がきれいに描ける様になりました。よかった。よかった。
            */
            #endregion
            List<Point> points = new List<Point>();
            double r = distance__Radius;
            double ix, iy, E;

            ix = r;
            iy = 0;
            E = 1.0 / r;
            do
            {
                ix = ix - E * iy;
                iy = E * ix + iy;
                points.Add(new Point((int)(ix + (double)middlePoint.X), (int)(iy + (double)middlePoint.Y)));
            } while (Math.Abs(ix - r) > .5 || Math.Abs(iy) > .5);
            return points;
        }

        // 描画
        /// <summary>
        /// 中心座標と半径から円周上の点を返します。同時に描画します。
        /// </summary>
        /// <param name="middlePoint"></param>
        /// <param name="distance__Radius"></param>
        /// <param name="drawnImage"></param>
        /// <param name="drawnColor"></param>
        /// <returns></returns>
        public static List<Point> getCirclePoints(Point middlePoint, double distance__Radius, ref Image drawnImage, Color drawnColor)
        {
            List<Point> points = getCirclePoints(middlePoint, distance__Radius);
            drawPoints(points, drawnColor, ref drawnImage);
            return points;
        }
        /// <summary>
        /// 指定した２つの座標間を結ぶ線の点集合を返します。同時に描画します。
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="drawnImage"></param>
        /// <param name="drawnColor"></param>
        /// <returns></returns>
        public static List<Point> getLinePoints(Point point1, Point point2, ref Image drawnImage, Color drawnColor)
        {
            List<Point> points = getLinePoints(point1.X, point1.Y, point2.X, point2.Y);
            drawPoints(points, drawnColor, ref drawnImage);
            return points;
        }

        // 直線の点の取得
        /// <summary>
        /// 指定した２つの座標間を結ぶ線の点集合を返します。
        /// </summary>
        /// <param name="_x1"></param>
        /// <param name="_y1"></param>
        /// <param name="_x2"></param>
        /// <param name="_y2"></param>
        /// <param name="drawnImage1"></param>
        /// <param name="drawnColor"></param>
        /// <returns></returns>
        public static List<Point> getLinePoints(int _x1, int _y1, int _x2, int _y2)
        {
            #region 点によって線を描く方法 http://r2cedar.cocolog-nifty.com/blog/2008/02/c_2f55.html
            /*
            C# 点を打つことで線を引く
            ｘ方向に始点（ｘ１）～終点（ｘ２）まで移動（符号方向に１づつ）させるときにｙの値がどうなるかをかんがえてみることにしました。

            移動の回数はｘ２－ｘ１の絶対値で長さを求めて、移動方向に関しては、ｘ２がｘ１以上の場合は＋１、ｘ１がｘ２より大きい場合は－１とする。つまり、ｘ２－ｘ１が負の場合、－１。正（ゼロを含めて）の場合は＋１とする。

            ここで、画面上の座標は整数となるため、ｘ方向に１進むときのｙの値も整数になるように小数点第一位を四捨五入するようにしてみる。

            整数部をｙ、少数部をｅであらわすことにして、
            ｎ番目のｙｎの値は、ｙ＝ｙ｛ｎ｝＋ｅ｛ｎ｝
            ここで、ｙ｛１｝はかならず整数で始まるのでｅ｛１｝＝０・・・（１）
            ｅ｛ｎ｝＝ｅ｛ｎ－１｝＋⊿ｙ÷⊿ｘ・・・（２）
            （四捨五入するのひ判定を簡単にして）
            ｅ｛ｎ｝－０．５≧０のとき、ｙ｛ｎ｝＝ｙ｛ｎ－１｝＋１，
　　　　　　　　　　　　　　　            ｅ｛ｎ｝はｅ｛ｎ｝－１を代入。
            ｅ｛ｎ｝－０．５＜０のとき、ｙ｛ｎ｝＝ｙ｛ｎ－１｝

            ｅ｛ｎ｝－０．５は単に判定用に使っているので計算を簡単にする都合上２⊿ｘを掛けて、
            Ｅ｛ｎ｝＝２⊿ｘ（ｅ｛ｎ｝－０．５）として、上記の(1)(2)に当てはめると、
            Ｅ｛１｝＝２⊿ｘ（ｅ｛１｝－０．５）＝２⊿ｘ（０－０．５）＝－⊿ｘ
            Ｅ｛ｎ｝＝２⊿ｘ（ｅ｛ｎ－１｝＋⊿ｙ÷⊿ｘ－０．５）
　　　　            ＝２⊿ｘｅ｛ｎ－１｝＋２⊿ｙ－⊿ｘ
　　　　            ＝（２⊿ｘｅ｛ｎ－１｝－⊿ｘ）＋２⊿ｙ＝Ｅ｛ｎ－１｝＋２⊿ｙ

            （⇒Ｅ｛ｎ｝＝２⊿ｘ（ｅ｛ｎ｝－０．５）としたので、
　　            Ｅ｛ｎ－１｝＝２⊿ｘ（ｅ｛ｎ－１｝－０．５）＝２⊿ｘｅ｛ｎ－１｝－⊿ｘ
　　            ・・・ｎ番目が前者なのでｎ－１番目は後者。
　　　            よって、前半の括弧をＥ｛ｎ－１｝に置換え可能。）

            上記を整理してみると、
            ｘ｛ｎ｝＝ｘ｛１｝＋ｎ，ｙ＝ｙ｛１｝＋⊿ｙ÷⊿ｘ・ｎのときに、
            Ｅ｛１｝＝－⊿ｘ，Ｅ｛ｎ｝＝Ｅ｛ｎ－１｝＋２⊿ｙとすると、
            Ｅ｛ｎ｝≧０のとき、ｙ｛ｎ｝＝ｙ｛ｎ－１｝＋１，
　　　　　　　　　　            Ｅ｛ｎ｝はＥ｛ｎ｝－２⊿ｘを代入。・・・（３）
            Ｅ｛ｎ｝＜０のとき、ｙ｛ｎ｝＝ｙ｛ｎ－１｝
            【（３）Ｅはｅの２⊿ｘとしたので、－１は－２⊿ｘと置き換える】

            これまでのことはすべて、ｘを動かすことを考えていたが、
            ⊿ｙ÷⊿ｘ＞１の場合、
            ｘを１動かすとｙが１以上動いてしまい、線がつながらなくなるので、その場合は、ｙを動かし、ｘを求めるようにする。以下がそのソース。

            nasatlxForm.csの内容。
　　            public partial class Form1 : Form
　　            {
　　　　            public Form1()
　　　　            {
　　　　　　            InitializeComponent();
　　　　            }

　　　　            private void pictureBox1_Paint(object sender, PaintEventArgs _e)
　　　　            {
　　　　　　            Bitmap Bmap = new Bitmap(pictureBox1.Width, 
                                                                pictureBox1.Height);
　　　　　　            int _x1,_x2;
　　　　　　            int _y1,_y2;

　　　　　　            _x1 = -100; _x2 = 100; _y1 = -100; _y2 = 100;
　　　　　　            this.line(ref Bmap, _x1, _y1, _x2, _y2, Color.Blue);
　　　　　　            _x1 = -100; _x2 = 100; _y1 = 100; _y2 = -100;
　　　　　　            this.line(ref Bmap, _x1, _y1, _x2, _y2, Color.Red);
　　　　　　            _x1 = 50; _x2 = 50; _y1 = 0; _y2 = 0;
　　　　　　            this.line(ref Bmap, _x1, _y1, _x2, _y2, Color.Black);
　　　　　　            _x1 = -50; _x2 = 50; _y1 = -100; _y2 = 100;
　　　　　　            this.line(ref Bmap, _x1, _y1, _x2, _y2, Color.Cyan);
　　　　　　            _x1 = -100; _x2 = 100; _y1 = -50; _y2 = 50;
　　　　　　            this.line(ref Bmap, _x1, _y1, _x2, _y2, Color.Magenta);

　　　　　　            pictureBox1.Image = Bmap;
　　　　            }

　　　　            private void line(ref Bitmap bm, int _x1, int _y1, int _x2, int _y2, 
                                            Color c)
　　　　            {
　　　　　　            int dx, lenx, sx;
　　　　　　            int dy, leny, sy;
　　　　　　            int _index, ix, iy, E;

　　　　　　            dx = _x2 - _x1; dy = _y2 - _y1;
　　　　　　            lenx = Math.Abs(dx); leny = Math.Abs(dy);
　　　　　　            sx = Math.Sign(dx + 0.1); sy = Math.Sign(dy + 0.1);
　　　　　　            ix = _x1; iy = _y1;

　　　　　　            if (leny > lenx)
　　　　　　            {
　　　　　　　　            E = -leny;
　　　　　　　　            for (_index=0; _index <= leny; _index++)
　　　　　　　　            {
　　　　　　　　　　            this.pset(ref bm, ix, iy, c);
　　　　　　　　　　            iy += sy;
　　　　　　　　　　            E += 2 * lenx;
　　　　　　　　　　            if (E >= 0)
　　　　　　　　　　            {
　　　　　　　　　　　　            ix += sx;
　　　　　　　　　　　　            E -= 2 * leny;
　　　　　　　　　　            }
　　　　　　　　            }
　　　　　　            }
　　　　　　            else
　　　　　　            {
　　　　　　　　            E = -lenx;
　　　　　　　　            for (_index=0; _index <= lenx; _index++)
　　　　　　　　            {
　　　　　　　　　　            this.pset(ref bm, ix, iy, c);
　　　　　　　　　　            ix += sx;
　　　　　　　　　　            E += 2 * leny;
　　　　　　　　　　            if (E >= 0)
　　　　　　　　　　            {
　　　　　　　　　　　　            iy += sy;
　　　　　　　　　　　　            E -= 2 * lenx;
　　　　　　　　　　            }
　　　　　　　　            }
　　　　　　            }
　　　　            }
　　　　            private void pset(ref Bitmap bm,int x,int y,Color c)
　　　　            {
　　　　　　            int x0 = 320; int y0 = 200;
　　　　　　            int xx = x0 + x;
　　　　　　            int yy = y0 - y;
　　　　　　            bm.SetPixel(xx, yy, c);
　　　　            }
　　            }

            */
            #endregion
            List<Point> points = new List<Point>();
            int dx, lenx, sx;
            int dy, leny, sy;
            int i, ix, iy, E;

            dx = _x2 - _x1; dy = _y2 - _y1;
            lenx = Math.Abs(dx); leny = Math.Abs(dy);
            sx = Math.Sign(dx + 0.1); sy = Math.Sign(dy + 0.1);
            ix = _x1; iy = _y1;

            if (leny > lenx)
            {
                E = -leny;
                for (i = 0; i <= leny; i++)
                {
                    points.Add(new Point(ix, iy));
                    iy += sy;
                    E += 2 * lenx;
                    if (E >= 0)
                    {
                        ix += sx;
                        E -= 2 * leny;
                    }
                }
            }
            else
            {
                E = -lenx;
                for (i = 0; i <= lenx; i++)
                {
                    points.Add(new Point(ix, iy));
                    ix += sx;
                    E += 2 * leny;
                    if (E >= 0)
                    {
                        iy += sy;
                        E -= 2 * lenx;
                    }
                }
            }
            return points;
        }
        // 直線の描画
        /// <summary>
        /// 指定したピクチャボックスの座標に、引数の点集合を描きます。指定した座標はウィンドウ座標（左端が(0,0)でy軸が下に行くとプラス）です。
        /// </summary>
        /// <param name="points"></param>
        /// <param name="drawnImage"></param>
        public static void drawPoints(List<Point> points, Color drawnColor, ref Image drawnImage)
        {
            Bitmap bitmap = new Bitmap(drawnImage); //new Bitmap(drawnImage.Width, drawnImage.Height);
            for (int i = 0; i < points.Count; i++)
            {
                // ※[注意] PictureBoxからBitmapを随時作成して点を打つdrawPointメソッドは、多くの点集合になると数秒かかり、使い物にならない。なので、最初にBitmapを作成し、まとめてpsetで点を打つ
                // 指定した点を指定した色で、指定したPicturebox上に打ちます。
                //drawPoint(points[_index].X, points[_index].Y, drawnColor, bitmap);
                pset(ref bitmap, points[i].X, points[i].Y, drawnColor);
            }
            //drawnImage = bitmap;
        }
        /// <summary>
        ///  点を打ちます。
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="c"></param>
        private static void pset(ref Bitmap bm, int x, int y, Color c)
        {
            if (x > 0 && x < bm.Width && y > 0 && y < bm.Height)
            {
                bm.SetPixel(x, y, c);
            }
        }
        /// <summary>
        /// ２Ｄ座標系の指定指定する時の種類です。座標(0,0)がどこにあるか、y軸がどの方向にいけばプラスかを間違えない様、きちんと確認してから指定してください。
        /// 
        /// ※.NET Frameworkでは、通常はウィンドウ座標が使われています。他の座標系への変換はgetGraphPositionやparseGraphPositionなどを使えば簡単にできます。
        /// </summary>
        public enum EGraphPositionType
        {
            t0_WindowPosition_ウィンドウ座標＿左上端が００で＿y軸が下に行くとプラス,
            t1_Graph00isTopLeftPosition_グラフ座標＿左下端が００で＿y軸が上に行くとプラス,
            t2_Graph00isMiddlePosition_グラフ座標＿中心が００で＿y軸が上に行くとプラス＿xやyがマイナスの値を取る時に使う,
            //t3_Graph00isLeftMiddlePosition_グラフ座標＿左端中心が００で＿y軸が上に行くとプラス＿yだけがマイナスの値を取る時に使う,            
        }
        /// <summary>
        /// 指定したピクチャボックスの座標に、引数の点集合を描きます。座標系は引数のEGraphTypeで指定すると、自動的に座標を変換して点を打ちます。
        /// </summary>
        /// <param name="points"></param>
        /// <param name="drawnImage"></param>
        public static void drawPoints_Graph(List<Point> points, Color drawnColor, ref Image drawnImage, EGraphPositionType _graphPositionTye)
        {
            Bitmap bitmap = new Bitmap(drawnImage); //(drawnImage.Width, drawnImage.Height);
            for (int i = 0; i < points.Count; i++)
            {
                // ※[注意] PictureBoxからBitmapを随時作成して点を打つdrawPointメソッドは、多くの点集合になると数秒かかり、使い物にならない
                // 指定した点を指定した色で、指定したPicturebox上に打ちます。
                //drawPoint(points[_index].X, points[_index].Y, drawnColor, bitmap);
                pset_Graph(ref bitmap, points[i].X, points[i].Y, drawnColor, _graphPositionTye);
            }
            //drawnImage = bitmap;
        }
        /// <summary>
        ///  指定した座標系EGraphTypeで、点を打ちます。
        /// </summary>
        /// <param name="bm"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="c"></param>
        private static void pset_Graph(ref Bitmap bm, int x, int y, Color c, EGraphPositionType _graphPositionType)
        {
            getPoint_ToGraph(ref x, ref y, bm, _graphPositionType);
            bm.SetPixel(x, y, c);
        }


        /// <summary>
        /// 1点のウィンドウ座標（左端が(0,0)でy軸が下に行くとプラス）を， 指定した座標系EGraphTypeに変換して返します．refがついてるxとyの値も変更されます。
        /// </summary>
        /// <param name="point_Control"></param>
        public static Point getPoint_ToGraph(ref int x, ref int y, Image _image, EGraphPositionType _graphPositionType)
        {
            Point _point = getPoint_ToGraph(new Point(x, y), _image, _graphPositionType);
            x = _point.X;
            y = _point.Y;
            return _point;
        }
        /// <summary>
        /// 1点のウィンドウ座標（左端が(0,0)でy軸が下に行くとプラス）を， 指定した座標系EGraphTypeに変換して返します．
        /// </summary>
        /// <param name="point_Control"></param>
        public static Point getPoint_ToGraph(Point point_Window, Image _image, EGraphPositionType _graphPositionType)
        {
            // 指定した座標系が示す、ウィンドウ座標系での位置（とりあえず初期値はウィンドウ座標系）
            int x0 = 0;
            int y0 = 0;
            int xx = point_Window.X;
            int yy = point_Window.Y;
            // それぞれの座標系での位置に変更
            if (_graphPositionType == EGraphPositionType.t0_WindowPosition_ウィンドウ座標＿左上端が００で＿y軸が下に行くとプラス)
            {
                // 初期値でやったから何もしなくていい
            }
            else if (_graphPositionType == EGraphPositionType.t1_Graph00isTopLeftPosition_グラフ座標＿左下端が００で＿y軸が上に行くとプラス)
            {
                x0 = 0;
                y0 = _image.Height;
                xx = x0 + point_Window.X;
                yy = y0 - point_Window.Y;
                xx = MyTools.adjustValue_From_Min_To_Max(xx, x0, _image.Width);
                yy = MyTools.adjustValue_From_Min_To_Max(yy, 0, _image.Height);
            }
            else if (_graphPositionType == EGraphPositionType.t2_Graph00isMiddlePosition_グラフ座標＿中心が００で＿y軸が上に行くとプラス＿xやyがマイナスの値を取る時に使う)
            {
                x0 = _image.Width / 2;
                y0 = _image.Height / 2;
                xx = x0 + point_Window.X;
                yy = y0 - point_Window.Y;
                xx = MyTools.adjustValue_From_Min_To_Max(xx, -x0, x0);
                yy = MyTools.adjustValue_From_Min_To_Max(yy, -y0, y0);
            }
            //Point conversedPoint_Graph = new Point(xx, yy);
            point_Window.X = xx;
            point_Window.Y = yy;
            return point_Window;
        }
        /// <summary>
        /// 点集合のウィンドウ座標（左端が(0,0)でy軸が下に行くとプラス）を，コントロールのグラフ座標（中心が(0,0)でy軸が上に行くとプラス）に変換して返します．
        /// </summary>
        /// <param name="point_Control"></param>
        public static List<Point> getPoints_ToGraph(List<Point> points_Window, ref Image drawnImage, EGraphPositionType _graphPositionType)
        {
            //Bitmap bitmap = new Bitmap(drawnImage.Width, drawnImage.Height);
            for (int i = 0; i < points_Window.Count; i++ )
            {
                points_Window[i] = getPoint_ToGraph(points_Window[i], drawnImage, _graphPositionType);
            }
            //drawnImage1.Image = bitmap;
            return points_Window;
        }
        /// <summary>
        /// 点集合のウィンドウ座標（左端が(0,0)でy軸が下に行くとプラス）を，コントロールのグラフ座標（中心が(0,0)でy軸が上に行くとプラス）に変換して返します．
        /// 
        /// ref Ysは中身も変更されるので注意してください。
        /// </summary>
        /// <param name="point_Control"></param>
        public static List<Point> getPoints_ToGraph(ref int[] Ys, ref Image drawnImage, EGraphPositionType _graphPositionType)
        {
            //Bitmap bitmap = new Bitmap(drawnImage.Width, drawnImage.Height);
            List<Point> _points = new List<Point>();
            int x = 0;
            Point _p;
            for (int i = 0; i < Ys.Length; i++)
            {
                x = i;
                _p = getPoint_ToGraph(ref x, ref Ys[i], drawnImage, _graphPositionType);
                _points.Add(_p);
            }
            //drawnImage1.Image = bitmap;
            return _points;
        }


        /// <summary>
        /// 指定した点集合を結ぶ線を描画します。 引数の画像drawnImageに、指定した座標系EGraphTypeに変換して点を打ちます。
        /// </summary>
        public static void drawLines_Graph(List<Point> points, ref Image drawnImage, Color drawnColor, EGraphPositionType _graphPositionType)
        {
            //Bitmap bitmap = new Bitmap(drawnImage.Width, drawnImage.Height);
            Graphics g = Graphics.FromImage(drawnImage);
            // グラフ座標に変換
            points = getPoints_ToGraph(points, ref drawnImage, _graphPositionType);
            Pen pen = new Pen(drawnColor, 1);
            g.DrawLines(pen, points.ToArray());
            //drawnImage.Image = bitmap;

        }
        // 大量に呼び出すと動作が遅くなると思われるので禁止
        /*
        /// <summary>
        /// 指定した２つの座標間を結ぶ線を描画します。指定した座標は一般的なグラフ座標（中心が(0,0)でy軸が上に行くとプラス）で構いません。自動的に座標を変換して点を打ちます。
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="drawnImage1"></param>
        /// <param name="drawnColor"></param>
        /// <returns></returns>
        public static void drawLine_Graph(ref Bitmap bm, Point point1, Point point2, PictureBox drawnImage1, Color drawnColor)
        {
            conversePoint_ToGraph(point1);
            conversePoint_ToGraph(point2);
            Graphics game = drawnImage1.CreateGraphics();
            Pen pen = new Pen(drawnColor, 1);
            game.DrawLines(pen, points.ToArray());
        }
        */
        /// <summary>
        /// 指定した点集合を結ぶ線を描画します。 引数の画像drawnImageに、指定した座標系EGraphTypeに変換して点を打ちます。
        /// </summary>
        public static void drawLines_Graph(int[] graphYs, ref Image drawnImage, Color drawnColor, EGraphPositionType _graphPositionType)
        {
            //Bitmap bitmap = new Bitmap(drawnImage.Width, drawnImage.Height);
            Graphics g = Graphics.FromImage(drawnImage);
            // グラフ座標に変換
            List<Point> _points = getPoints_ToGraph(ref graphYs, ref drawnImage, _graphPositionType);
            Pen pen = new Pen(drawnColor, 1);
            g.DrawLines(pen, _points.ToArray());
            //drawnImage.Image = bitmap;

        }
        /// <summary>
        /// 指定した点集合を結ぶ線を描画します。 引数の画像drawnImageに、指定した座標系EGraphTypeに変換して点を打ちます。
        /// </summary>
        public static void drawLines_Graph(double[] graphYs, ref Image drawnImage, Color drawnColor, EGraphPositionType _graphPositionType)
        {
            // int[] double[]
            int[] _ints = new int[graphYs.Length];
            for (int i = 0; i < graphYs.Length; i++)
            {
                _ints[i] = (int)graphYs[i];
            }
            drawLines_Graph(_ints, ref drawnImage, drawnColor, _graphPositionType);
        }
        #endregion
        #region 範囲制限がある値（範囲値，コントロールの座標など）の位置調整: getAdjustValue/adjustValue
        /// <summary>
        /// adjustValueと一緒です。値が最小値～最大値になるようにして調整して、返します。　※返り値に代入しないと、値が変更されないので、気を付けてください。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int getAdjustValue(int value, int min, int max)
        {
            return adjustValue_From_Min_To_Max(value, min, max);
        }
        /// <summary>
        /// getAdjustValueと一緒です。値が最小値～最大値になるように調整して、返します。　※返り値に代入しないと、値が変更されないので、気を付けてください。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int adjustValue_From_Min_To_Max(int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
            return value;
        }
        /// <summary>
        /// adjustValueと一緒です。値が最小値～最大値になるようにして、返します。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double getAdjustValue(double value, int min, int max)
        {
            return adjustValue_From_Min_To_Max(value, min, max);
        }
        /// <summary>
        /// 値が最小値～最大値になるようにします。　※返り値に代入しないと、値は変更されないので、気を付けてください。
        /// </summary>
        /// <param name="valueLists"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double adjustValue_From_Min_To_Max(double value, double min, double max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }
            return value;
        }
        /// <summary>
        /// フォームのクライアント領域をはみ出さないよう、指定サイズ物体の座標の端の位置を(0, 0)から(フォームのクライアント領域.X, フォームのクライアント領域.Y)までに，調整した座標を返します。
        /// </summary>
        /// <param name="control_sizes"></param>
        /// <param name="controlShownedform"></param>
        public static Point adjustControlPosition(Point point, Size size, int x_min, int y_min, int x_max, int y_max)
        {

            // Leftの調整
            int left_min = point.X;
            int left_max = left_min + size.Width;
            if (left_min >= x_min)
            {
                if (left_max > x_max)
                {
                    point.X = x_max - size.Width;
                }
            }
            else
            {
                point.X = x_min;
            }

            // Topの調整
            int top_min = point.Y;
            int top_max = point.Y + size.Height;
            if (top_min >= y_min)
            {
                if (top_max > y_max)
                {
                    point.Y = y_max - size.Height;
                }
            }
            else
            {
                point.Y = y_min;
            }
            return point;

        }
        #endregion


        // ●グラフィクス系（画像、座標など）　※PointクラスやImageクラスに依存している可能性があります

        // 座標関連
        #region オブジェクトがオブジェクト内にあるかどうか: isObjectA_InsideOf
        /// <summary>
        /// オブジェクトA（位置とサイズを持つ四角形）がオブジェクトB内にあるかどうかを返します．
        /// </summary>
        /// <param name="_objectA_Position"></param>
        /// <param name="_objectA_Size"></param>
        /// <param name="_objectB_Position"></param>
        /// <param name="_objectB_Size"></param>
        /// <returns></returns>
        public static bool isObjectA_InsideOf_ObjectB(Point _objectA_Position, Size _objectA_Size, Point _objectB_Position, Size _objectB_Size)
        {
            bool _isInside = false;
            if (_objectA_Position.X >= _objectB_Position.X &&
                _objectA_Position.X + _objectA_Size.Width <= _objectB_Position.X + _objectB_Size.Width)
            {
                if (_objectA_Position.Y >= _objectB_Position.Y &&
                    _objectA_Position.Y + _objectA_Size.Height <= _objectB_Position.Y + _objectB_Size.Height)
                {
                    _isInside = true;
                }
            }
            return _isInside;
        }
        #endregion

        #region ２点が頂点である四角形を返すメソッド createRectangle
        /// <summary>
        /// ２点が頂点である四角形を返します
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <returns></returns>
        public static Rectangle createRectangle(Point point1, Point point2)
        {
            Rectangle rec = new Rectangle(0, 0, 0, 0);
            if (point1.X <= point2.X)
            {
                if (point1.Y <= point2.Y)
                {
                    rec = getRectangle(point1.X, point1.Y, point2.X, point2.Y);
                }
                else if (point1.Y > point2.Y)
                {
                    rec = getRectangle(point1.X, point2.Y, point2.X, point1.Y);
                }
            }
            else if (point1.X > point2.X)
            {
                if (point1.Y <= point2.Y)
                {
                    rec = getRectangle(point2.X, point1.Y, point1.X, point2.Y);
                }
                else if (point1.Y > point2.Y)
                {
                    rec = getRectangle(point2.X, point2.Y, point1.X, point1.Y);
                }
            }

            // 四角形が作れるなら、四角形を返す
            return rec;
        }

        /// <summary>
        /// 条件が(_x1, _y1) →右下→ (_x2, _y2)の引数の四角形を返します。
        /// </summary>
        /// <param name="_x1"></param>
        /// <param name="_y1"></param>
        /// <param name="_x2"></param>
        /// <param name="_y2"></param>
        private static Rectangle getRectangle(int x1, int y1, int x2, int y2)
        {
            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }
        #endregion

        #region ２点間の距離を計算する: getDistance
        /// <summary>
        /// ２点間の距離を計算します。（Point型）
        /// </summary>
        /// <param name="mouseDrag_DownPoint"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        internal static double getDistance(Point _point1, Point _point2)
        {
            return getDistance(_point1.X, _point1.Y, _point2.X, _point2.Y);
        }
        /// <summary>
        /// ２点間の距離を計算します。（int型）
        /// </summary>
        /// <param name="_x1"></param>
        /// <param name="_y1"></param>
        /// <param name="_x2"></param>
        /// <param name="_x2"></param>
        /// <returns></returns>
        internal static double getDistance(int x1, int y1, int x2, int y2)
        {
            double distance = Math.Sqrt(Math.Pow((double)(x1 - x2), 2) + Math.Pow((double)(y1 - y2), 2));
            return distance;
        }
        #endregion

        #region グラフデータ用の座標：左下端が（0,0）⇔マウス用の座標：左上が(0,0)　の変換: getGraphPointPosition/MouseCursorPosition

        /// <summary>
        /// マウス用の座標：左上が(0,0)　→　高さHeightのグラフデータ用の座標：左下端が（0,0）　の変換。※座標はプラス値のみです。
        /// </summary>
        public static Point getGraphPointPosition_ByMouseCursorPosition(int _x_MouseCursor, int _y_MouseCursor, int _xMin, int _xMax, int _yMin, int _yMax, int _ScreenWidth, int _ScreenHeight)
        {
            int _width_GraphPositon = Math.Abs(_xMin) + Math.Abs(_xMax);
            int _height_GraphPosition = Math.Abs(_yMin) + Math.Abs(_yMax);
            // 等倍の時はこれでいい
            int _x = Math.Max(0, _x_MouseCursor); _x = Math.Min(_width_GraphPositon, _x);
            int _y = Math.Max(0, _height_GraphPosition - _y_MouseCursor); _y = Math.Min(_height_GraphPosition, _y);
            // 等倍じゃない時
            if (_width_GraphPositon != _ScreenWidth && _ScreenWidth !=0 && _ScreenHeight !=0)
            {
                _x = Math.Max(0, MyTools.getSisyagonyuValue((double)_x_MouseCursor * ((double)_width_GraphPositon / (double)_ScreenWidth)));
                _x = Math.Min(_width_GraphPositon, _x);
                int _val = MyTools.getSisyagonyuValue((double)_y_MouseCursor * ((double)_height_GraphPosition / (double)_ScreenHeight));
                _y = Math.Max(0, _height_GraphPosition - MyTools.getSisyagonyuValue((double)_y_MouseCursor * ((double)_height_GraphPosition / (double)_ScreenHeight))); 
                _y = Math.Min(_height_GraphPosition, _y);
            }
            Point _point = new Point(_x, _y);
            return _point;
        }
        /// <summary>
        /// マウス用の座標：左上が(0,0)　→　高さHeightのグラフデータ用の座標：左下端が（0,0）　の変換。※座標はプラス値のみです。
        /// </summary>
        public static Point getGraphPointPosition_ByMouseCursorPosition(Point _point_MouseCursor, int _xMin, int _xMax, int _yMin, int _yMax, int _ScreenWidth, int _ScreenHeight)
        {
            return getGraphPointPosition_ByMouseCursorPosition(_point_MouseCursor.X, _point_MouseCursor.Y, _xMin, _xMax, _yMin, _yMax, _ScreenWidth, _ScreenHeight);
        }


        /// <summary>
        /// 高さHeightのグラフデータ用の座標：左下端が（0,0）　→　マウス用の座標：左上が(0,0)　の変換。※座標はプラス値のみです。
        /// </summary>
        public static Point getMouseCursorPosition_ByGraphPointPosition(int _x_GraphPoint, int _y_GraphPoint, int _xMin, int _xMax, int _yMin, int _yMax, int _ScreenWidth, int _ScreenHeight)
        {
            int _width_GraphPositon = Math.Abs(_xMin) + Math.Abs(_xMax);
            int _height_GraphPosition = Math.Abs(_yMin) + Math.Abs(_yMax);
            // 等倍の時はこれでいい
            int _x = Math.Max(0, _x_GraphPoint); _x = Math.Min(_ScreenWidth, _x);
            int _y = Math.Max(0, _ScreenHeight - _y_GraphPoint); _y = Math.Min(_ScreenHeight, _y);
            // 等倍じゃない時
            if (_width_GraphPositon != _ScreenWidth && _width_GraphPositon != 0 && _height_GraphPosition != 0)
            {
                _x = Math.Max(0, MyTools.getSisyagonyuValue((double)_x_GraphPoint * ((double)_ScreenWidth / (double)_width_GraphPositon))); 
                _x = Math.Min(_ScreenWidth, _x);
                _y = Math.Max(0, _ScreenHeight - MyTools.getSisyagonyuValue((double)_y_GraphPoint * ((double)_ScreenHeight / (double)_height_GraphPosition))); 
                _y = Math.Min(_ScreenHeight, _y);
            }
            Point _point = new Point(_x, _y);
            return _point;
        }
        /// <summary>
        /// グラフデータ用の座標：左下端が（0,0）　→　マウス用の座標：左上が(0,0)　の変換。※座標はプラス値のみです。
        /// </summary>
        public static Point getMouseCursorPosition_ByGraphPointPosition(Point _point_GraphPosint, int _xMin, int _xMax, int _yMin, int _yMax, int _ScreenWidth, int _ScreenHeight)
        {
            return getMouseCursorPosition_ByGraphPointPosition(_point_GraphPosint.X, _point_GraphPosint.Y, _xMin, _xMax, _yMin, _yMax, _ScreenWidth, _ScreenHeight);
        }


        #endregion

        // 画像イメージ関連
        #region ★画像ファイル読み込みの基本　画像ファイルをロックせずに読み込む方法: getImage
        
        /// <summary>
        /// 指定した画像ファイルをロックせずに、System.Drawing.Imageを作成します。ファイルが存在しないなど、エラーが起こった場合はgetErrorImage()で取得されるイメージを返します。
        /// 　　
        /// （※通常のImage.FromFileなどで画像ファイルをロードした場合、エクスプローラでそのファイルがロックされ、
        /// 　削除・名前変更・上書き・移動などができなくなります。
        /// 　ですので、Imageをロードする時は、このメソッドを使うことをおススメします。）
        /// 　詳細。ここを参考にしました。感謝。：　http://dobon.net/vb/dotnet/graphics/drawpicture2.html
        /// 　
        /// 
        ///             // 【注意】Imageクラスは、ラスター画像を扱うBitmapクラス（bmp,jpeg,gif,pngなど）と、ベクトル画像を扱うMetafileクラス（wmf,emfなど）を、抽象的に扱えるクラスです。
        /// Bitmapクラスは、.bmpだけでなく、jpeg、gifなども扱えます。
        /// Metafileクラスは、ラスター画像とは違って各ピクセルのデータというものは持ちません。描画処理そのものを記録します。例えば、「(0,0) から (100, 100) まで幅 3 の赤色のペンで線を引く」等といった描画処理をデータで記録します。
        /// よって、Imageクラスの画素を細かくいじりたい場合は、Imageクラスがどちらのクラスに属しているかを知る必要があります。
        /// </summary>
        /// <param name="_filename_FullPath">作成元のファイルのフルパス</param>
        /// <returns>作成したImageクラスのインスタンス。</returns>
        public static System.Drawing.Image getImage(string _filename_FullPath)
        {
            System.Drawing.Image _image = null;
            try
            {
                System.IO.FileStream _fs = new System.IO.FileStream(
                    _filename_FullPath,
                    System.IO.FileMode.Open,
                    System.IO.FileAccess.Read);
                _image = System.Drawing.Image.FromStream(_fs);
                _fs.Close();
            }
            catch (Exception e)
            {
                // エラーメッセージ出力は1つにつき数十ミリ秒時間を無駄にするので、なるべく節約してね。
                ConsoleWriteLine(MyTools.getMethodName_OnClassNameAndLineNo(2, false) + _filename_FullPath + "の画像読み込みに失敗しました。nullを返します。詳細" + e.Message);

                // 失敗した場合は、エラー画像を貼っておく。
                // 予め用意されたエラー画像を表示
                _image = getErrorImage();
            }
            return _image;
        }
        #endregion

        #region プログラム上でImageを作成する: getImage

        /// <summary>
        /// 新しいBitmap画像（System.Drawing.Imageクラスの子クラス）を生成します。
        /// 
        /// 生成後、画像の加工方法は、このメソッド内にコメントアウトして載せてありますので、参考にしてください。
        /// なお、使わなくなったら、GraphicsとImage/BitmapをDispose()するのを忘れないでください。メモリ不足の原因になります。
        /// 
        /// ※基本的に、Imageオブジェクトを動的に作成する手順は、次のようになります。
        ///
        ///         1. Bitmapオブジェクトを作成する。
        ///         2. Graphics.FromImageメソッドでGraphicsオブジェクトを作成する。
        ///         3. Graphicsのメソッドを使って、図形などを描画する。
        ///         4. GraphicsをDisposeメソッドで解放する。
        ///         
        ///  作り方の参考。感謝。 http://dobon.net/vb/dotnet/graphics/createimage.html
        /// </summary>
        /// <returns></returns>
        public static Bitmap getImage_NewBitmap(int _width, int _height)
        {
            //グラフィックファイルを読み込んでImageオブジェクトを作成する
            Bitmap img = new Bitmap(_width, _height);
            ////ImageオブジェクトのGraphicsオブジェクトを作成する
            //Graphics game = Graphics.FromImage(p_graphBackImage);

            ////全体を黒で塗りつぶす
            //game.FillRectangle(Brushes.Black, game.VisibleClipBounds);
            ////黄色い扇形を描画する
            //game.DrawPie(Pens.Yellow, 60, 10, 80, 80, 30, 300);
            ////文字列("DOBON.NET")を左上に描画する
            //Font fnt = new Font("Arial", 12);
            //game.DrawString("DOBON.NET", fnt, Brushes.Black, 0, 0);
            //fnt.Dispose();
            ////作成した画像を保存する
            //p_graphBackImage.Save(@"C:\test\new1.bmp");

            ////リソースを解放する
            //game.Dispose();
            ////グラフィックを使わなくなった時、これを忘れずにね！p_graphBackImage.Dispose();

            return img;
        }
        /// <summary>
        /// 新しいBitmap画像（System.Drawing.Imageクラスの子クラス）を生成して、Imageクラスとして返します。
        /// 
        /// 【注意】以下、ImageクラスとGrpahicsクラスの説明・・・
        /// ※BitmapはImageの子クラスなので、Imageと同様に使用できます。（ベクトル画像を生成したいときは、別のメソッドを使ってください）
        /// 
        /// 生成後、Graphics.FromImage()でGraphicsを使って、画像を加工します。
        /// 詳細は、このメソッド内にコメントアウトして載せてありますので、参考にしてください。
        /// なお、使わなくなったら、GraphicsとImage/BitmapをDispose()するのを忘れないでください。ガベージコレクションがよきにはからってくれる可能性がありますが、大量に放置されると、メモリ不足の原因になります。
        /// 
        /// ※基本的に、Imageオブジェクトを動的に作成する手順は、次のようになります。
        ///
        ///         1. Bitmapオブジェクトを作成する。
        ///         2. Graphics.FromImageメソッドでGraphicsオブジェクトを作成する。
        ///         3. Graphicsのメソッドを使って、図形などを描画する。
        ///         4. GraphicsをDisposeメソッドで解放する。
        ///         
        ///  作り方の参考。感謝。 http://dobon.net/vb/dotnet/graphics/createimage.html
        /// </summary>
        /// <returns></returns>
        public static Image getImage(int _width, int _height)
        {
            // 新しいラスタ画像を生成。（ベクトル画像を生成したいときは、別のメソッドを使ってください）
            //※BitmapはImageの子クラスなので、Imageと同様に使用できます。キャストは不要。
            return getImage_NewBitmap(_width, _height);
        }
        #endregion

        #region Imageのコピーや一部コピー: getImage/getCopyedImage/getCopyedBitmapImage
        /// <summary>
        /// 指定したbaseImageと同じイメージをコピーして返します。元のイメージは変更されません。
        /// 
        /// （※何も考えず「image = _baseImage」とやると、参照先の名前が２つになるだけです。_imageを変更したりDispose()したりすると、
        /// 元の_baseImageも変更されたり参照できなくなったりする（型 'System.ArgumentException' の例外をスローする）ので、
        /// このではそういうことが起こらないよう、コピーを返しています。）
        /// </summary>
        public static Image getImage(Image _baseImage)
        {
            // Imageクラスのコンストラクタはコピーは無く、コピーをするメソッドColone()メソッドは.NET CompactFrameworkには存在しないらしいし、Bitmapのコンストラクタを使っている
            return getCopyedImage(_baseImage);
        }
        /// <summary>
        /// 指定したsourceImageをコピーしたイメージを返します。
        /// </summary>
        public static Image getCopyedImage(Image _sourceImage)
        {
            // Imageクラスのコンストラクタはコピーは無く、コピーをするメソッドColone()メソッドは.NET CompactFrameworkには存在しないらしいし、Bitmapのコンストラクタを使っている
            Bitmap _bitmap = new Bitmap(_sourceImage); //_sourceImage.Clone();
            return _bitmap; // BitmapはImageの子クラスなので、別にキャストしなくても大丈夫。
        }
        /// <summary>
        /// 指定したsourceBitmapをコピーしたイメージを返します。※BitmapはImageの子クラスなので、Imageと同様に使用できます。キャストは不要。
        /// </summary>
        public static Image getCopyedImage(Bitmap _sourceBitmapImage)
        {
            // Imageクラスのコンストラクタはコピーは無く、コピーをするメソッドColone()メソッドは.NET CompactFrameworkには存在しないらしいし、なんか使い方間違えると怖いので、Bitmapのコンストラクタを使っている
            Bitmap _bitmap = new Bitmap(_sourceBitmapImage); //_sourceImage.Clone();
            return _bitmap; // BitmapはImageの子クラスなので、別にキャストしなくても大丈夫。
        }
        /// <summary>
        /// 指定したsourceImageの一部分(x0, y0)～(x0+width, y0+height)をコピーしたイメージを返します。
        /// </summary>
        public static Image getCopyedImage(Image _sourceImage, int _x0, int _y0, int _width, int _height)
        {
            Image _image = getCopyedImage(_sourceImage);
            Graphics g = Graphics.FromImage(_image);
            g.DrawImage(_image, _x0, _y0, _width, _height);
            g.Dispose();
            return _image;
        }
        /// <summary>
        /// 指定したsourceBitmapの一部分(x0, y0)～(x0+width, y0+height)をコピーしたイメージを返します。※BitmapはImageの子クラスなので、Imageと同様に使用できます。キャストは不要。
        /// </summary>
        public static Image getCopyedImage(Bitmap _sourceBitmapImage, int _x0, int _y0, int _width, int _height)
        {
            // ソースの汎用性を高めるため、Imageで加工するようにする
            Image _image = getCopyedImage(_sourceBitmapImage);
            return getCopyedImage(_image);
        }
        /// <summary>
        /// 指定したsourceBitmapをコピーしたイメージを返します。※BitmapはImageの子クラスなので、Imageと同様に使用できます。キャストは不要。
        /// </summary>
        public static Bitmap getCopyedBitmapImage(Image _sourceBitmap)
        {
            return new Bitmap(_sourceBitmap);
        }
        /// <summary>
        /// 指定したsourceBitmapをコピーしたイメージを返します。※BitmapはImageの子クラスなので、Imageと同様に使用できます。キャストは不要。
        /// </summary>
        public static Bitmap getCopyedBitmapImage(Bitmap _sourceBitmap)
        {
            return new Bitmap(_sourceBitmap);
        }
        /// <summary>
        /// 指定したsourceBitmapの一部分(x0, y0)～(x0+width, y0+height)をコピーしたイメージを返します。※BitmapはImageの子クラスなので、Imageと同様に使用できます。キャストは不要。
        /// </summary>
        public static Bitmap getCopyedBitmapImage(Image _sourceBitmap, int _x0, int _y0, int _width, int _height)
        {
            Image _image = getCopyedImage(_sourceBitmap);
            return getCopyedBitmapImage(_image, _x0, _y0, _width, _height);
        }
        /// <summary>
        /// 指定したsourceBitmapの一部分(x0, y0)～(x0+width, y0+height)をコピーしたイメージを返します。※BitmapはImageの子クラスなので、Imageと同様に使用できます。キャストは不要。
        /// </summary>
        public static Bitmap getCopyedBitmapImage(Bitmap _sourceBitmap, int _x0, int  _y0, int _width, int _height)
        {
            Image _image = getCopyedImage(_sourceBitmap);
            return getCopyedBitmapImage(_image, _x0, _y0, _width, _height);
        }
        ///// <summary>
        ///// Copies a _copyedRange_Rectangle of a bitmap.
        ///// </summary>
        //public static Bitmap getCopyedBitmap(Bitmap _sourceBitmap, Rectangle _copyedRange_Rectangle)
        //{
        //    Bitmap bmp = new Bitmap(_copyedRange_Rectangle.Width, _copyedRange_Rectangle.Height);
        //    Graphics game = Graphics.FromImage(bmp);
        //    game.DrawImage(_sourceBitmap,0,0,_copyedRange_Rectangle,GraphicsUnit.Pixel);
        //    game.Dispose();
        //    return bmp;
        //}


        #endregion

        #region Imageのサイズ変更や拡大・縮小・サムネイル画像取得: getImage_Resized/resizeImage  / getImage_ResizedCopy/getImage  /getImage_Samune
        /// <summary>
        /// Imageを指定倍した画像を取得します。元の画像は消去（メモリ解放）されるので、注意してください。
        /// 参考URL。感謝。 http://dobon.net/vb/dotnet/graphics/drawimage.html#scaling
        /// </summary>
        public static Image getImage_Resized(Image _image, double _sizeRate)
        {
            return getImage_Resized(_image, _sizeRate, _sizeRate);
        }
        /// <summary>
        /// Imageを指定倍した画像を取得します。元の画像は消去（メモリ解放）されるので、注意してください。
        /// 参考URL。感謝。 http://dobon.net/vb/dotnet/graphics/drawimage.html#scaling
        /// </summary>
        public static Image getImage_Resized(Image _image, double _xRate, double _yRate)
        {
            int _width = (int)((double)_image.Width*_xRate);
            int _height = (int)((double)_image.Height*_yRate);
            return getImage_Resized(_image, _width, _height);
        }
        /// <summary>
        /// Imageをリサイズした画像を取得します。元の画像は消去（メモリ解放）されるので、注意してください。
        /// 参考URL。感謝。 http://dobon.net/vb/dotnet/graphics/drawimage.html#scaling
        /// </summary>
        public static Image getImage_Resized(Image _image, int _width_Resized, int _height_Resized)
        {
            //描画先とするImageオブジェクトを作成する
            Image _resizedImage = getImage(_width_Resized, _height_Resized);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(_resizedImage);

            //グラフィックのサイズを指定倍にしてcanvasに描画する
            g.DrawImage(_image, 0, 0, _width_Resized, _height_Resized);
            //Imageオブジェクトのリソースを解放する
            _image.Dispose();

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
            return _resizedImage;
        }
        /// <summary>
        /// getCopyedImage_Resizedと一緒です。Imageをリサイズした画像を新しく作成して返します。元の画像はそのまま残ります。
        /// 参考URL。感謝。 http://dobon.net/vb/dotnet/graphics/drawimage.html#scaling
        /// </summary>
        public static Image getImage_ResizedCopy(Image _image, int _width_Resized, int _height_Resized)
        {
            return getCopyedImage_Resized(_image, _width_Resized, _height_Resized);
        }
        /// <summary>
        /// getImage_ResizedCopyと一緒です。Imageをリサイズした画像を新しく作成して返します。元の画像はそのまま残ります。
        /// 参考URL。感謝。 http://dobon.net/vb/dotnet/graphics/drawimage.html#scaling
        /// </summary>
        public static Image getCopyedImage_Resized(Image _image, int _width_Resized, int _height_Resized)
        {
            //描画先とするImageオブジェクトを作成する
            Image _resizedImage = getImage(_width_Resized, _height_Resized);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(_resizedImage);

            //グラフィックのサイズを指定倍にしてcanvasに描画する
            g.DrawImage(_image, 0, 0, _width_Resized, _height_Resized);
            //Imageオブジェクトのリソースを解放する
            //消去しない_image.Dispose();

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
            return _resizedImage;
        }


        /// <summary>
        /// Imageのサムネイル画像を高速に取得します。デフォルトは120×120。これが一番早いそうです。
        /// 参考URL。感謝。 http://dobon.net/vb/dotnet/graphics/thumbnail.html
        /// </summary>
        public static Image getImage_Samune(Image _image)
        {
            return _image.GetThumbnailImage(120, 120, null, IntPtr.Zero);
        }
        #endregion

        #region Imageを一部描画した画像
        /// <summary>
        /// 引数のイメージの一部(_x0, _y1)～(_x0+_width, _y0+_height)を描画した画像を新しく生成して返します。引数のイメージは変更されません。
        /// </summary>
        public static Image getImage_DrawPart(Image _image, int _x0, int _y0, int _width, int _height)
        {
            //描画先とするImageオブジェクトを作成する
            Image _drawnImage = getImage(_width, _height);
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(_drawnImage);

            //グラフィックのサイズを指定倍にしてcanvasに描画する
            g.DrawImage(_image, _x0, _y0, _width, _height);
            //Imageオブジェクトのリソースを解放する
            //_image.Dispose();

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();
            return _drawnImage;
        }
        /// <summary>
        /// 引数のイメージの一部(_x0, _y1)～(_x0+_width, _y0+_height)を描画した画像を新しく生成して返します。引数のイメージは変更されません。
        /// </summary>
        public static Image getImage_DrawPart(Image _image, int _x0, int _y0, double _xyRate)
        {
            return getImage_DrawPart(_image, _x0, _y0,  (int)((double)(_image.Width)*_xyRate), (int)((double)(_image.Height)*_xyRate));
        }
        /// <summary>
        /// 引数のイメージの一部(_x0, _y1)～(_x0+_width, _y0+_height)を描画した画像を新しく生成して返します。引数のイメージは変更されません。
        /// </summary>
        public static Image getImage_DrawPart(Image _image, int _x0, int _y0, double _xRate, double _yRate)
        {
            return getImage_DrawPart(_image, _x0, _y0, (int)((double)(_image.Width) * _xRate), (int)((double)(_image.Height) * _yRate));
        }
        #endregion

        #region ★Imageの基本描画メソッド： drawImage
        /// <summary>
        /// 第一引数のcanvasイメージの一部領域（_xStart, _yStart）～（_xStart+_width, _yStart+height）に、第二引数の画像の一部(_x0, _y1)～(_x0+_width, _y0+_height)を描画します。
        /// 
        /// ※画像描画のメインルーチン。参考URL。感謝。http://dobon.net/vb/dotnet/graphics/drawimage.html
        /// </summary>
        public static void drawImage(Image _canvasImage, int _xStart_canvasDrawn, int _yStart_canvasPoint, Image _drawnImage, int _x0, int _y0, int _width, int _height, System.Drawing.Drawing2D.InterpolationMode _quarityMode)
        {
            //ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(_canvasImage);

            // 参考 http://dobon.net/vb/dotnet/graphics/interpolationmode.html
            //補間方法の指定
            g.InterpolationMode = _quarityMode;

            //グラフィックのサイズを指定倍にしてcanvasに描画する
            g.DrawImage(_drawnImage, _x0, _y0, _width, _height);
            //Imageオブジェクトのリソースを解放する
            //_image.Dispose();

            //Graphicsオブジェクトのリソースを解放する
            g.Dispose();

            // return _canvasImage;
        }
        /// <summary>
        /// 第一引数のcanvasイメージの一部領域（_xStart, _yStart）～（_xStart+_width, _yStart+height）に、第二引数の画像の一部(_x0, _y1)～(_x0+_width, _y0+_height)を描画します。
        /// 
        /// ※デフォルトのスケーリングを変更したいなら、このメソッドを変更してください。
        /// </summary>
        public static void drawImage(Image _canvasImage, int _xStart_canvasDrawn, int _yStart_canvasPoint, Image _drawnImage, int _x0, int _y0, int _width, int _height)
        {
            drawImage(_canvasImage, _xStart_canvasDrawn, _yStart_canvasPoint, _drawnImage, _x0, _y0, _width, _height, System.Drawing.Drawing2D.InterpolationMode.Bicubic);
        }
        /// <summary>
        /// 第一引数のcanvasイメージの一部領域（_xStart, _yStart）～（_xStart+_width, _yStart+height）に、第二引数の画像を描画します。
        /// </summary>
        public static void drawImage(Image _canvasImage, int _xStart_canvasDrawn, int _yStart_canvasPoint, Image _drawnImage)
        {
            drawImage(_canvasImage, _xStart_canvasDrawn, _yStart_canvasPoint, _drawnImage, 0, 0, _drawnImage.Width, _drawnImage.Height);
        }
        /// <summary>
        /// 第一引数のcanvasイメージの一部領域（_xStart, _yStart）～（_xStart+_width, _yStart+height）に、第二引数の画像を指定倍率で描画します。
        /// </summary>
        public static void drawImage(Image _canvasImage, int _xStart_canvasDrawn, int _yStart_canvasPoint, Image _drawnImage, double _xyRate)
        {
            drawImage(_canvasImage, _xStart_canvasDrawn, _yStart_canvasPoint, _drawnImage, 0, 0, (int)((double)(_drawnImage.Width)*_xyRate), (int)((double)(_drawnImage.Height)*_xyRate));
        }
        /// <summary>
        /// 第一引数のcanvasイメージの一部領域（_xStart, _yStart）～（_xStart+_width, _yStart+height）に、第二引数の画像を指定倍率で描画します。
        /// </summary>
        public static void drawImage(Image _canvasImage, int _xStart_canvasDrawn, int _yStart_canvasPoint, Image _drawnImage, double _xRate, double _yRate)
        {
            drawImage(_canvasImage, _xStart_canvasDrawn, _yStart_canvasPoint, _drawnImage, 0, 0, (int)((double)(_drawnImage.Width) * _xRate), (int)((double)(_drawnImage.Height) * _yRate));
        }



        #endregion

        #region ファイルのタグの付け方: addTag_Image / getTag_Image / getPropertyItem0_Sample
        /// <summary>
        /// 画像Imageに指定したタグが含まれている場合はtrue、含まれてない場合はfalseを返します。
        /// </summary>
        public static bool isTagAdded_Image(Image _image, string _tag)
        {
            bool _isAdded = false;
            string _tagAll = getTag_Image(_image);
            _isAdded = _tagAll.Contains(_tag);
            return _isAdded;
        }
        /// <summary>
        /// 画像Imageに指定した作者の名前（ユーザＩＤ）が含まれている場合はtrue、含まれてない場合はfalseを返します。
        /// </summary>
        public static bool isTagAuthor_Image(Image _image, string _TagAuthorName_UserID)
        {
            bool _isAdded = false;
            string _tagAll = getTag_Image(_image);
            _isAdded = _tagAll.Contains(_TagAuthorName_UserID);
            return _isAdded;
        }
        /// <summary>
        /// 画像Imageに指定したタグ名の値を取得します。存在しない場合は""を返します。
        /// なお、特定のタグが含まれていることを確認する時は、isTagAdded_Image("値")、またはisTagAdded_Image("タグ名")を使ってください。
        /// </summary>
        public static string getTag_Image(Image _image, string _tagName)
        {
            string _tagValue = "";
            string _tagAll = getTag_Image(_image);
            if (_tagAll.Contains(_tagName) == true)
            {
                // タグ名を探す
                int _index = _tagAll.LastIndexOf(_tagName + ":"); // 一応、最新の（最後に追加された）値を優先
                if (_index == -1) _index = _tagAll.LastIndexOf(_tagName + "："); // 全角コロンにも対応
                if (_index == -1) return _tagValue; // なんでかわからないけど見つからない
                // 「タグ名:」や「タグ名：」から後の文字列から、終点を探す
                int _endIndex = _tagAll.Substring(_index).IndexOf(",");
                if (_endIndex == -1) _endIndex = _index + (_tagName.Length + 1) + 1;// コロンが無いなんておかしいが、エラーを防ぐため、「タグ名:」から1文字としておく
                if (_endIndex == 0) { _tagValue = ""; }
                else
                {
                    _tagValue = _tagAll.Substring(_index, _endIndex);
                }
                // 「タグ名：値」から「値」だけを取得
                int _valueStartIndex = _tagValue.IndexOf(":");
                if (_valueStartIndex == -1) _valueStartIndex = _tagValue.IndexOf("：");
                if (_valueStartIndex == -1) return _tagValue; // なんかおかしいけどコロンみつからんから全部返しとけ
                _tagValue = _tagValue.Substring(_valueStartIndex+1);
            }
            return _tagValue;
        }
        /// <summary>
        /// 指定したイメージに、作者のタグが含まれていれば取得します。含まれていなければ、""を返します。
        /// </summary>
        public static string getTagAuthor_Image(Image _image)
        {
            return getTag_Image(_image, "作者");
        }
        /// <summary>
        /// 画像Imageに付加されているタグ（addTag_Imageで付加した、画像に埋め込まれている文字列）を全て取得します。存在しない場合は""を返します。
        /// なお、特定のタグが含まれていることを確認する時は、isTagAdded_Image("値")、またはisTagAdded_Image("タグ名")を使ってください。
        /// </summary>
        public static string getTag_Image(Image _image)
        {
            string _tag = "";
            // imageがnullだったら、終了。
            if (_image == null) return _tag;

            // PropertyItemsを格納していないイメージだったら、""。
            if (_image.Tag == null)
            {
                return _tag;
            }
            else
            {
                // Tagオブジェクトの文字列を取得する
                object _tagObject = _image.Tag;
                //文字列に変換する
                string _tagString = _tagObject.ToString();
                _tag = _tagString;
            }
            return _tag;
        }
        /// <summary>
        /// 画像Imageにタグを追加します。全く同じものが含まれていれば、何もしません。
        /// 　　　※タグ_addTagStringには、「値（String型）」、もしくは名前を付けて「TagName:Value（どちらもString型）」Or「タグ名：値」（名前や値・コロンは大文字でもOK）で指定してください。
        /// タグは、addする毎に自動的に「,」で区切られて保存されます。
        /// また、特定のタグを取得する時は、getTag_Image("タグ名")、もしくは値が存在することを確認するメソッドであるisTagAdded_Image("値")、またはisTagAdded_Image("タグ名")を使ってください。
        /// </summary>
        public static void addTag_Image(Image _image, string _addTagString)
        {
            // imageがnullだったら、終了。
            if (_image == null) return;

            string _defaultTag = "タグ情報:C#プログラム上で独自に作成したタグが付加されています。"; // 最初に付けるタグタイトル
            // Tagを格納していないイメージだったら、新しく適当なものを格納させる。
            if (_image.Tag == null)
            {
                string _tagString = _defaultTag + ",";
                // オブジェクトのコピーを渡す（(object)_tagStringよりは安心かな、と）
                object _newTagObj = _tagString.Clone();
                _image.Tag = _newTagObj;
            }
            object _tagObject = _image.Tag;
            if (_tagObject == null)
            {
                // 上記で追加しているはずなのに、タグが付加されていないエラー。ブレークポイント
                int _error = 1;
            }
            else
            {
                // 全く同じタグが含まれていなければ、追加
                if (isTagAdded_Image(_image, _addTagString) == false)
                {
                    //文字列に変換する
                    string _tagString = _tagObject.ToString();
                    //値を変更する
                    _tagString = _tagString + _addTagString + ",";
                    // オブジェクトのコピーを渡す（(object)_tagStringよりは安心かな、と）
                    _tagObject = _tagString.Clone();
                    //格納する
                    _image.Tag = _tagObject;
                }
            }
        }
        /// <summary>
        /// 画像に指定した作者の名前（ユーザＩＤ）をタグとして付加します。複数人付加することができます。
        /// </summary>
        public static void addTagAuthor_Image(Image _image, string _TagAutor_UserID)
        {
            addTag_Image(_image, "作者:"+_TagAutor_UserID);
        }
        #region 草案: PropertyItemを使って実装しようとしたけど、コンストラクタないし。これを持つ画像ファイルが見つからなったので、無理だった。
        ///// <summary>
        ///// 画像Imageに指定したタグが含まれている場合はtrue、含まれてない場合はfalseを返します。
        ///// </summary>
        //public static bool isTagAdded_Image(Image _image, string _tag)
        //{
        //    bool _isAdded = false;
        //    string _tagAll = getTag_Image(_image);
        //    _isAdded = _tagAll.Contains(_tag);
        //    return _isAdded;
        //}
        ///// <summary>
        ///// 画像Imageに指定したタグ名の値を取得します。存在しない場合は""を返します。
        ///// なお、特定のタグが含まれていることを確認する時は、isTagAdded_Image("値")、またはisTagAdded_Image("タグ名")を使ってください。
        ///// </summary>
        //public static string getTag_Image(Image _image, string _tagName)
        //{
        //    string _tagValue = "";
        //    string _tagAll = getTag_Image(_image);
        //    if (_tagAll.Contains(_tagName) == true)
        //    {
        //        // タグ名を探す
        //        int _index = _tagAll.LastIndexOf(_tagAll+":"); // 一応、最新の（最後に追加された）値を優先
        //        if(_index == -1) _index = _tagAll.LastIndexOf(_tagAll+"："); // 全角コロンにも対応
        //        if(_index == -1) return _tagValue; // なんでかわからないけど見つからない
        //        // 「タグ名:」や「タグ名：」から後の文字列から、終点を探す
        //        int _endIndex = _tagAll.Substring(_index).IndexOf(",");
        //        if(_endIndex == -1) _endIndex = _index + (_tagName.Length + 1) + 1;// コロンが無いなんておかしいが、エラーを防ぐため、「タグ名:」から1文字としておく
        //        _tagValue = _tagAll.Substring(_index, _endIndex - _index);
        //        // 「タグ名：値」から「値」だけを取得
        //        int _valueStartIndex = _tagValue.IndexOf(":");
        //        if(_valueStartIndex == -1) _valueStartIndex = _tagValue.IndexOf("：");
        //        if(_valueStartIndex == -1) return _tagValue; // なんかおかしいけどコロンみつからんから全部返しとけ
        //        _tagValue = _tagValue.Substring(_valueStartIndex);
        //    }
        //    return _tagValue;
        //}
        ///// <summary>
        ///// 画像Imageに付加されているタグ（addTag_Imageで付加した、画像に埋め込まれている文字列）を全て取得します。存在しない場合は""を返します。
        ///// なお、特定のタグが含まれていることを確認する時は、isTagAdded_Image("値")、またはisTagAdded_Image("タグ名")を使ってください。
        ///// </summary>
        //public static string getTag_Image(Image _image)
        //{
        //    // 独自に作成したタグを格納するExif情報のIDとType
        //    int _piID = 0x9286; // UserComment(0x9286)を上書き
        //    short _piType = 7; // _none_未定義（日本語など、String型なら何でも格納できるようにするため）

        //    string _tag = "";
        //    // imageがnullだったら、終了。
        //    if (_image == null) return _tag;
                        
        //    // PropertyItemsを格納していないイメージだったら、""。
        //    if (_image.PropertyItems == null || _image.PropertyItems.Length == 0)
        //    {
        //        return _tag;
        //    }else{
        //        //とにかくPropertyItemオブジェクトの文字列を取得する
        //        System.Drawing.Imaging.PropertyItem _pi = getTagPropertyItem_FromExifInfoID_Image(_image, _piID, _piType);
        //        if (_pi != null)
        //        {
        //            //文字列に変換する
        //            string _tagString = System.Text.Encoding.ASCII.GetString(_pi.Value);
        //            _tagString = _tagString.Trim(new char[] { '\0' }); // 最後の「\0」を消しておく。
        //            _tag = _tagString;
        //        }
        //    }
        //    return _tag;
        //}
        ///// <summary>
        ///// 画像Imageにタグを追加します。
        ///// タグ_addTagStringには、「値（String型）」、もしくは名前を付けて「TagName:Value（どちらもString型）」Or「タグ名：値」（名前や値・コロンは大文字でもOK）で指定してください。
        ///// タグは、addする毎に自動的に「,」で区切られて保存されます。
        ///// また、特定のタグを取得する時は、getTag_Image("タグ名")、もしくは値が存在することを確認するメソッドであるisTagAdded_Image("値")、またはisTagAdded_Image("タグ名")を使ってください。
        ///// </summary>
        //public static void addTag_Image(Image _image, string _addTagString)
        //{
        //    // 参考URL。感謝。　http://dobon.net/vb/dotnet/graphics/getexifinfo.html
        //    // imageがnullだったら、終了。
        //    if (_image == null) return;
            
        //    // 独自に作成したタグを格納するExif情報のIDとType
        //    int _piID = 0x9286; // UserComment(0x9286)を上書き
        //    short _piType = 7; // _none_未定義（日本語など、String型なら何でも格納できるようにするため）
        //    string _defaultTag = "タグ情報:C#プログラム上で独自に作成したタグが付加されています。"; // 最初に付けるタグタイトル
        //    // PropertyItemsを格納していないイメージだったら、新しく適当なものを格納させる。
        //    if (_image.PropertyItems == null || _image.PropertyItems.Length == 0)
        //    {
        //        System.Drawing.Imaging.PropertyItem _newPi = getPropertyItem0_Sample();//これができないから既存のファイルからわざわざ取ってくるしかないnew System.Drawing.Imaging.PropertyItem();
        //        //IdやType等を変更する
        //        _newPi.Id = _piID; // UserComment(0x9286)を上書き
        //        _newPi.Type = _piType;
        //        //タグ情報の前に'\0'を8バイト入れる
        //        string _tagTitle = new string('\0', 8) + _defaultTag + ",";
        //        // Shift-JISに文字コード変換
        //        _newPi.Value = System.Text.Encoding.GetEncoding("shift_jis").GetBytes(_tagTitle);
        //        _newPi.Len = _newPi.Value.Length;
        //        //格納する
        //        _image.SetPropertyItem(_newPi);
        //    }
        //    //とにかくPropertyItemオブジェクトの文字列を取得する
        //    System.Drawing.Imaging.PropertyItem _pi = getTagPropertyItem_FromExifInfoID_Image(_image, _piID, _piType);
        //    if (_pi == null)
        //    {
        //        // 上記で追加しているはずなのに、タグが付加されていないエラー。ブレークポイント
        //        int _error = 1;
        //    }
        //    else
        //    {
        //        //文字列に変換する
        //        string _tagString = System.Text.Encoding.ASCII.GetString(_pi.Value);
        //        _tagString = _tagString.Trim(new char[] { '\0' }); // 最後の「\0」を消しておく。
        //        //値を変更する
        //        _tagString = _tagString + _addTagString + ",";
        //        //タグ情報の前に'\0'を8バイト入れる
        //        _tagString = new string('\0', 8) + _tagString;
        //        // Shift-JISに文字コード変換
        //        _pi.Value = System.Text.Encoding.GetEncoding("shift_jis").GetBytes(_tagString);
        //        _pi.Len = _pi.Value.Length;
        //        //格納する
        //        _image.SetPropertyItem(_pi);
        //    }
        //}
        //private static Image p_getImage_PropertyItem0_SampleImage;
        ///// <summary>
        ///// リソースに組み込まれた画像ファイルから、PropertyItemのサンプルを取って来ます。
        ///// </summary>
        ///// <returns></returns>
        //private static System.Drawing.Imaging.PropertyItem getPropertyItem0_Sample()
        //{
        //    if (p_getImage_PropertyItem0_SampleImage == null)
        //    {
        //        p_getImage_PropertyItem0_SampleImage =
        //            getImage(MyTools.getProjectDirectory() + "\\データベース\\グラフィック\\CIMG6144.jpg");
        //    }
        //    // これを実行するには、ソリューションエクスプローラーから
        //    // プロジェクトの階層→Resources.resxを開き、「リソースの追加」でなんでもいいのでPNGファイルを追加して、
        //    // その名前を「_sample_PNG」にしてください。
        //    //Image _sampleImage = Properties.Resources._sample_PNG;
        //    Image _sampleImage = p_getImage_PropertyItem0_SampleImage;
        //    System.Drawing.Imaging.PropertyItem _pi0 = null;
        //    try
        //    {
        //        _pi0 = _sampleImage.PropertyItems[0];//_sampleImage.GetPropertyItem(0); // カメラで撮った画像.jpgでもExternalExceptionになる。もうだめだ。
        //    }
        //    catch (Exception _e)
        //    {
        //        ConsoleWriteLine("PropertyItem[0]を取得できないImageオブジェクトです。アプリケーションを終了します。詳細"+_e.Message);
        //        Application.Exit();
        //    }

        //    // プログラム上で作ったらホントにないのか、テスト。→ほんとになった。
        //    //Image _newImage = getImage(1, 1);
        //    //System.Drawing.Imaging.PropertyItem _pi0nasi = _sampleImage.GetPropertyItem(0); // AargumentExceptionになる

        //    return _pi0;
        //}
        #endregion
        #region 撮影時間・ユーザコメントなどのExif情報の取得。これは存在しない場合はnullを返すだけなので使える
        /// <summary>
        /// 画像ファイルに含まれるExif情報のIDから、様々な情報を取得します。該当するExif情報が存在しない場合はnullを返します。（例：ユーザーコメント:0x9286（Type=7）、写真の撮影時間: 0x9003（Type=2）、）
        /// その他のIDはここを参照してください http://dobon.net/vb/dotnet/graphics/getexifinfo.html
        /// </summary>
        public static System.Drawing.Imaging.PropertyItem getTagPropertyItem_FromExifInfoID_Image(Image _image, int _ExifInfo_ID, int _ExifInfo_Type)
        {
            System.Drawing.Imaging.PropertyItem _pi = null;
            string _info = "";
            foreach (System.Drawing.Imaging.PropertyItem item in _image.PropertyItems)
            {
                //Exif情報から撮影時間を取得する
                if (item.Id == _ExifInfo_ID && item.Type == _ExifInfo_ID)
                {
                    //文字列に変換する
                    string val = System.Text.Encoding.ASCII.GetString(item.Value);
                    val = val.Trim(new char[] { '\0' }); // 最後の「\0」を消しておく。
                    _info = val;

                    // 以下、例えば撮影時間を取得したら、撮影時間に基づいてファイルの作成日時を変更しなおしたりできる。
                    //DateTimeに変換
                    //DateTime dt = DateTime.ParseExact(val, "yyyy:MM:dd HH:mm:ss", null);
                    //ファイルの作成日時を変更
                    //System.IO.File.SetCreationTime(imgFile, dt);

                    break;
                }
            }
            //return _FPSInfo;
            return _pi;
        }
        #endregion
        #endregion

        #region イメージが使用不可かどうか、のエラー画像の取得・設定・確認: getErrorImage / setErrorImage / isErrorImage
        /// <summary>
        /// エラー画像に付加されるタグの文字列
        /// </summary>
        private static string p_getImage_ErrorImage_TagString = "エラー画像";
        /// <summary>
        /// エラー画像（ファイル読み込みエラーなどで画像が正しく取得できなかった時に表示される画像）。staticプロパティはstaticメソッド内で初期化しないとnullが入ったままなので、getErrorImageで初期化
        /// </summary>
        private static Image p_getImage_ErrorImage;
        /// <summary>
        /// エラー画像（ファイル読み込みエラーなどで画像が正しく取得できなかった時に表示される画像）を取得します。
        /// この画像を変更したい場合は、setErrorImageで設定してください。
        /// </summary>
        /// <returns></returns>
        public static Image getErrorImage()
        {
            if (p_getImage_ErrorImage == null)
            {
                // デフォルトのエラーイメージ「×」印を作成
                Image _errorImage = getImage(120, 120);
                addTag_Image(_errorImage, "エラー画像"); // エラー画像用のタグを付加（isErrorImageなどで使う）
                Graphics _g = Graphics.FromImage(_errorImage);
                int _lineWidth = Math.Max(1, (int)((double)(_errorImage.Width) * 0.1)); // 線の太さは10分の1、1以上
                int _yohaku = _lineWidth; // 余白も10分の1
                Pen _pen = new Pen(Color.Red, _lineWidth);
                _g.DrawLine(_pen, _yohaku, _yohaku, _errorImage.Width - _yohaku, _errorImage.Height - _yohaku);
                _g.DrawLine(_pen, _errorImage.Width - _yohaku, _yohaku, _yohaku, _errorImage.Height - _yohaku);
                // グラフィクスのメモリ解放を忘れずに
                _g.Dispose();
                // 代入
                setErrorImage(_errorImage);
            }
            return p_getImage_ErrorImage;
        }
        /// <summary>
        /// エラー画像（ファイル読み込みエラーなどで画像が正しく取得できなかった時に表示される画像）を設定します。
        /// デフォルトは、「×」印です（getErrorImageを参照）。
        /// </summary>
        public static void setErrorImage(Image _errorImage)
        {
            p_getImage_ErrorImage = _errorImage;
        }
        /// 引数のImageが使用不可能なエラー画像か、nullか、もしくはDispose()を呼び出された後か（既にメモリ解放されていて使えない状態になっているか）を返します。
        /// 
        /// ※Image.Dispose()したオブジェクトは、Image == nullで調べてもtrueになってしまうので、
        /// 調べる際、このメソッドを呼び出す癖を付けて、Flags = 'p_pictureNowImage.Flags' は、型 'System.ArgumentException' の例外を発生させないようにしています。
        /// 
        /// Imageオブジェクトを扱う際は、できるだけこのメソッド==falseを通してから処理を行うなどして、気を付けてください。
        public static bool isErrorImage(Image _image)
        {
            // nullかdisposeされていたらtrue
            if (isNullOrDisposedImage(_image) == true)
            {
                return true;
            }
            // タグにエラー画像であることを示す文字列が含まれていたらtrue
            if (isTagAdded_Image(_image, p_getImage_ErrorImage_TagString) == true)
            {
                return true;
            }
            else
            {
                // それ以外はfalse
                return false;
            }
        }
        #region イメージのメモリが解放されたかを調べる isNullOrDisposedImage
        /// <summary>
        /// 引数のImageがnullか、もしくはDispose()を呼び出された後か（既にメモリ解放されていて使えない状態になっているか）を返します。
        /// 
        /// ※Image.Dispose()したオブジェクトは、Image == nullで調べてもtrueになってしまうので、
        /// 調べる際、このメソッドを呼び出す癖を付けて、Flags = 'p_pictureNowImage.Flags' は、型 'System.ArgumentException' の例外を発生させないようにしています。
        /// 
        /// Imageオブジェクトを扱う際は、できるだけこのメソッド==falseを通してから処理を行うなどして、気を付けてください。
        /// </summary>
        /// <param name="_image"></param>
        /// <returns></returns>
        private static bool isNullOrDisposedImage(Image _image)
        {
            bool _isDisposed = false;
            if (_image != null)
            {
                if (_image.PixelFormat == System.Drawing.Imaging.PixelFormat.Undefined) { _isDisposed = true; }
            }
            else
            {
                _isDisposed = true;
                string _message = "私は " + MyTools.getMethodName_OnClassNameAndLineNo(2, false) + "行です。: 私を呼び出した" + MyTools.getMethodName_OnClassNameAndLineNo(2, false) + "行で、nullまたはDispose()されたImageオブジェクトが使われたよ。呼び出し元の" + MyTools.getMethodName_OnClassNameAndLineNo(3, false) + "行 や" + MyTools.getMethodName_OnClassNameAndLineNo(4, false) + "行 あたりも参照してみてね。";
                ConsoleWriteLine(_message);
            }
            return _isDisposed;
        }
        #endregion
        #endregion


        #region 反転画像を作成: getNegativeImage
        // プログラム。感謝。http://dobon.net/vb/dotnet/graphics/drawnegativeimage.html#lockbits
        //using System.Drawing;
        //using System.Drawing.Imaging;

        /// <summary>
        /// 指定された画像を反転画像（ネガティブイメージ）に変換して返します。
        /// </summary>
        /// <param name="p_graphBackImage">変換する画像</param>
        public static Bitmap getNegativeImage(Bitmap _image)
        {
            //1ピクセルあたりのバイト数を取得する
            int pixelSize = 3;
            if (_image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
            {
                pixelSize = 3;
            }
            else if (_image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb ||
                _image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb ||
                _image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppRgb)
            {
                pixelSize = 4;
            }
            else
            {
                throw new ArgumentException(
                    "1ピクセルあたり24または32ビットの形式のイメージのみ有効です。",
                    "p_graphBackImage");
            }

            //Bitmapをロックする
            System.Drawing.Imaging.BitmapData bmpDate = _image.LockBits(
                new System.Drawing.Rectangle(0, 0, _image.Width, _image.Height),
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                _image.PixelFormat);

            //ピクセルデータをバイト型配列で取得する
            IntPtr ptr = bmpDate.Scan0;
            byte[] pixels = new byte[bmpDate.Stride * _image.Height];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);

            //すべてのピクセルの色を反転させる
            for (int y = 0; y < bmpDate.Height; y++)
            {
                for (int x = 0; x < bmpDate.Width; x++)
                {
                    //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                    int pos = y * bmpDate.Stride + x * pixelSize;
                    //青、緑、赤の色を変更する
                    pixels[pos] = (byte)(255 - pixels[pos]);
                    pixels[pos + 1] = (byte)(255 - pixels[pos + 1]);
                    pixels[pos + 2] = (byte)(255 - pixels[pos + 2]);
                }
            }

            //ピクセルデータを元に戻す
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);

            //アンセーフコードを使うと、以下のようにもできる
            //unsafe
            //{
            //    byte* pixelPtr = (byte*)bmpDate.Scan0;
            //    for (int y = 0; y < bmpDate.Height; y++)
            //    {
            //        for (int x = 0; x < bmpDate.Width; x++)
            //        {
            //            //ピクセルデータでのピクセル(x,y)の開始位置を計算する
            //            int pos = y * bmpDate.Stride + x * pixelSize;
            //            //青、緑、赤の色を変更する
            //            pixelPtr[pos] = (byte)(255 - pixelPtr[pos]);
            //            pixelPtr[pos + 1] = (byte)(255 - pixelPtr[pos + 1]);
            //            pixelPtr[pos + 2] = (byte)(255 - pixelPtr[pos + 2]);
            //        }
            //    }
            //}

            //ロックを解除する
            _image.UnlockBits(bmpDate);
            return _image;
        }
        #endregion


        #region ファイルから読み込んだ画像を，透明色有効にする
        /// <summary>
        /// ファイルから読み込んだ画像を，白色（PhotoShopなどの透明色）を透明色有効にして，Imageクラスを返します．
        /// </summary>
        /// <param name="_imageFilename_FullPath"></param>
        /// <returns></returns>
        public static Image getTransImage(string _imageFilename_FullPath)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile(@_imageFilename_FullPath);
            bmp.MakeTransparent(); // [透明度][png][gif]透明度設定を加工した画像もMakeTransparentしないと駄目！
            // bmp.Save(@"C:\Resources\VS用透明色加工済み_"+_fileName_NotFullPath_ファイル名_名前だけ);
            return bmp;
        }
        /// <summary>
        /// ファイルから読み込んだ画像を，指定した色を透明色有効にして，Imageクラスを返します．
        /// </summary>
        /// <param name="_imageFilename_FullPath"></param>
        /// <returns></returns>
        public static Image getTransImage(string _imageFilename_FullPath, Color _transColor)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile(@_imageFilename_FullPath);
            bmp.MakeTransparent(_transColor); // [透明度][png][gif]透明度設定を加工した画像もMakeTransparentしないと駄目！
            // bmp.Save(@"C:\Resources\VS用透明色加工済み_"+_fileName_NotFullPath_ファイル名_名前だけ);
            return bmp;
        }
        /// <summary>
        /// 指定したイメージ(Image)の画像を，指定した色を透明色有効にして，Imageクラスを返します．
        /// </summary>
        /// <param name="_imageFilename_FullPath"></param>
        /// <returns></returns>
        public static Image getTransImage(Image _image, Color _transColor)
        {
            Bitmap bmp = (Bitmap)_image;
            bmp.MakeTransparent(_transColor); // [透明度][png][gif]透明度設定を加工した画像もMakeTransparentしないと駄目！
            // bmp.Save(@"C:\Resources\VS用透明色加工済み_"+_fileName_NotFullPath_ファイル名_名前だけ);
            return bmp;
        }
        #endregion

        #region イメージの透明色を透明にする: setImageTransColor
        /// <summary>
        /// イメージの透明色を指定して透明表示にします．
        /// </summary>
        /// <param name="image_includingTransColor"></param>
        /// <param name="transColorPlaceType_None0_TheLeftUpPixel1_While2_Green3_Blue4_TheRightUpPixel5"></param>
        public static void setImageTransColor(Image image_includingTransColor, Color _transColor)
        {
            setImageTransColor(new Bitmap(image_includingTransColor), _transColor);
        }
        /// <summary>
        /// イメージの透明色を指定して透明表示にします．
        /// </summary>
        /// <param name="image_includingTransColor"></param>
        /// <param name="transColorPlaceType_None0_TheLeftUpPixel1_While2_Green3_Blue4_TheRightUpPixel5"></param>
        public static void setImageTransColor(Bitmap image_includingTransColor, Color _transColor)
        {
            image_includingTransColor.MakeTransparent(_transColor);//(bmp.GetPixel(0,0))だと左上端の色 //(this.BackColor)だとフォームの背景色 // 引数なしだと白 // [透明度][png][gif]透明度設定を加工した画像もMakeTransparentしないと駄目！
            // bmp.Save(@"C:\Resources\VS用透明色加工済み_"+_fileName_NotFullPath_ファイル名_名前だけ);
        }
        /// <summary>
        /// イメージの透明色を指定して透明表示にします．
        /// </summary>
        /// <param name="image_includingTransColor"></param>
        /// <param name="transColorPlaceType_None0_TheLeftUpPixel1_While2_Green3_Blue4_TheRightUpPixel5"></param>
        public static void setImageTransColor(Image image_includingTransColor, int transColorPlaceType_None0_TheLeftUpPixel1_While2_Green3_Blue4_TheRightUpPixel5)
        {
            setImageTransColor(new Bitmap(image_includingTransColor), transColorPlaceType_None0_TheLeftUpPixel1_While2_Green3_Blue4_TheRightUpPixel5);
        }
        /// <summary>
        /// イメージの透明色を指定して透明表示にします．
        /// </summary>
        /// <param name="image_includingTransColor"></param>
        /// <param name="transColorPlaceType_None0_TheLeftUpPixel1_While2_Green3_Blue4_TheRightUpPixel5"></param>
        public static void setImageTransColor(Bitmap image_includingTransColor, int transColorPlaceType_None0_TheLeftUpPixel1_While2_Green3_Blue4_TheRightUpPixel5)
        {
            Color _transColor;
            switch (transColorPlaceType_None0_TheLeftUpPixel1_While2_Green3_Blue4_TheRightUpPixel5)
            {
                case 0:
                    return; // 透明色を指定しない
                case 1:
                    _transColor = image_includingTransColor.GetPixel(0, 0);
                    break;
                case 2:
                    _transColor = Color.White;
                    break;
                case 3:
                    _transColor = Color.Green;
                    break;
                case 4:
                    _transColor = Color.Blue;
                    break;
                case 5:
                    _transColor = image_includingTransColor.GetPixel(image_includingTransColor.Width - 1, 0);
                    break;
                default:
                    _transColor = image_includingTransColor.GetPixel(0, 0);
                    break;
            }
            image_includingTransColor.MakeTransparent(_transColor);//(bmp.GetPixel(0,0))だと左上端の色 //(this.BackColor)だとフォームの背景色 // 引数なしだと白 // [透明度][png][gif]透明度設定を加工した画像もMakeTransparentしないと駄目！
            // bmp.Save(@"C:\Resources\VS用透明色加工済み_"+_fileName_NotFullPath_ファイル名_名前だけ);

        }
        #endregion

        #region イメージに透明度を指定して表示
        /// <summary>
        /// 第一引数のグラフィック描画領域に，第二引数のイメージを，第三引数の不透明度（0.0：透明～1.0：不透明が）で表示します．
        /// </summary>
        /// <param name="game"></param>
        /// <param name="p_graphBackImage"></param>
        /// <param name="alpha"></param>
        public static void drawImage_Trans(Graphics g, Image img, float alpha)
        {
            //背景（ダブルバッファ）を用意する
            Bitmap back = new Bitmap(img.Width, img.Height);
            //backのGraphicsオブジェクトを取得
            Graphics bg = Graphics.FromImage(back);
            //白で塗りつぶす
            bg.Clear(Color.White);

            //ColorMatrixオブジェクトの作成
            System.Drawing.Imaging.ColorMatrix cm =
                new System.Drawing.Imaging.ColorMatrix();
            //ColorMatrixの行列の値を変更して、アルファ値がalphaに変更されるようにする
            cm.Matrix00 = 1;
            cm.Matrix11 = 1;
            cm.Matrix22 = 1;
            cm.Matrix33 = alpha;
            cm.Matrix44 = 1;

            //ImageAttributesオブジェクトの作成
            System.Drawing.Imaging.ImageAttributes ia =
                new System.Drawing.Imaging.ImageAttributes();
            //ColorMatrixを設定する
            ia.SetColorMatrix(cm);

            //ImageAttributesを使用して背景に描画
            bg.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height),
                0, 0, img.Width, img.Height, GraphicsUnit.Pixel, ia);
            //合成された画像を表示
            g.DrawImage(back, 0, 0);

            //リソースを開放する
            bg.Dispose();
            back.Dispose();
        }
        #endregion

        // 画面キャプチャ，アクティブウィンドウ取得系
        #region 画面キャプチャ（スクリーン画面全体，ウィンドウのみ）
        /// <summary>
        /// アクティブなウィンドウのスクリーンショットを取ったビットマップ画像を返します。
        /// </summary>
        /// <returns>アクティブなウィンドウの画像</returns>
        public static Bitmap getScreenCapture_ActiveWindow()
        {
            //アクティブなウィンドウのデバイスコンテキストを取得
            IntPtr hWnd = GetForegroundWindow();
            IntPtr winDC = GetWindowDC(hWnd);
            //ウィンドウの大きさを取得
            RECT winRect = new RECT();
            GetWindowRect(hWnd, ref winRect);
            //Bitmapの作成 // right, bottom
            Bitmap bmp = new Bitmap(winRect.height - winRect.left,
                winRect.width - winRect.top);
            //Graphicsの作成
            Graphics g = Graphics.FromImage(bmp);
            //Graphicsのデバイスコンテキストを取得
            IntPtr hDC = g.GetHdc();
            //Bitmapに画像をコピーする
            BitBlt(hDC, 0, 0, bmp.Width, bmp.Height,
                winDC, 0, 0, SRCCOPY);
            //解放
            g.ReleaseHdc(hDC);
            g.Dispose();
            ReleaseDC(hWnd, winDC);

            return bmp;
        }
        /// <summary>
        /// 指定座標のスクリーンショットを取ったビットマップ画像を返します。
        /// </summary>
        /// <param name="p_usedForm"></param>
        /// <returns></returns>
        public static Bitmap getScreenCapture(Point startPoint, Point endPoint)
        {
            // [Note][CopyFromScreen]:.NETのGraphics.CopyFromScreenメソッドを使って，簡単に任意の座標のスクリーンショットが取れる
            // http://lassy-tech.blogspot.com/
            Size size = new Size(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
            if (size.Width <= 0 || size.Height <= 0)
            {
                Console.WriteLine("getScreenCapture:開始座標と終点座標が，それぞれ左上と右下になっていません．画面キャプチャできません．サイズ(1,1)の初期化ビットマップを返します．");
                return new Bitmap(1, 1);
            }
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(startPoint, new Point(0, 0), bmp.Size);
            }
            //this.pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            //this.pictureBox1.Image = bmp;
            return bmp;
        }

        #endregion


        // ■ファイル系文字列処理
        // これらを使った新しいクラスをMyToolsと独立させたかったら、この部分だけをコピペして使ってください。
        // 　ここでは、ファイルの存在確認・読み込み・書き込み・検索などを、引数に文字列を指定するだけで処理を完結できる、
        //      staticメソッドをまとめています。
        // 　また、よくありがちなファイルパス文字列のケアレスミスのチェック→エラー処理→コンソール出力も、
        //   isExist()メソッドを読みだすだけで済ませることができます。
        // ※なお、Fileクラスのオブジェクトを作成する方法は、staticメソッドでは使いにくいので、MyFileIOクラスを参照してください。
        /// <summary>
        /// System.Environment.NewLine; を使った、環境非依存の改行コードを指します。
        /// 基本的は"\r\n"が入っているので、ConsoleWriteLineメソッドやstring型でよく指定する改行文字"\n"とは異なります。
        /// 基本的に、ファイルから取得した改行の認識などに使ってください。
        /// </summary>
        public static string _n = System.Environment.NewLine;
        /// <summary>
        /// ="utf-8"。●このクラス内のメソッドを使う時に使う、デフォルトの文字エンコードをです。 
        /// </summary>
        public static EEncodingType p_EncodingType_DEFAULT = EEncodingType.e01_UTF8・ネット上のデファクトスタンダード;
        #region よく使う文字列エンコード名の取得: getEncodingTypeName
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
        /// <summary>
        /// ファイルの文字コード（UTF-8、Shift-JISなどのエンコーディング形式）を取得します。
        /// ファイルが見つからない場合、ファイル新規作成時の推奨文字コードSystem.Text.Encoding.UTF8を返します。
        /// 
        /// 　　※ なお、ReadFile()メソッドは、別に内部でこの機能を実装していて、
        /// ファイル内のテキストの文字コードを検出してから、読み込みをしています。
        /// </summary>
        public static System.Text.Encoding ReadFileEncoding(string _readFileName_FullPath){
            System.Text.Encoding _encoding = System.Text.Encoding.UTF8;
            // とりあえずbyte[]型で、どんなエンコードのファイルでも取る
            byte[] _bytes = MyTools.ReadFile_AllBytes(_readFileName_FullPath);
            if (_bytes != null)
            {
                _encoding = MyTools.GetEncoding(_bytes);
            }
            // データ量大きいかもしれないbyte[]配列はもう使わないから、GCに早くメモリを解放してもらうよう、一応nullを与えておこうかな（どうせすぐメソッド終了するし、効果ないかもだけど）
            _bytes = null;
            return _encoding;
        }
        /// <summary>
        /// 書き込み用のファイルの文字コード（UTF-8、Shift-JISなどのエンコーディング形式）を取得します。
        /// ファイルが見つからない場合、ファイル新規作成時の推奨文字コードSystem.Text.Encoding.UTF8を返します。
        /// 
        /// 　　※ なお、WriteFile()メソッドは、自動的に内部でこのメソッドを呼び出して、
        /// ファイル内のテキストの文字コードを検出してから、書き込みをしています。
        /// </summary>
        public static System.Text.Encoding WriteFileEncoding(string _writeFileName_FullPath)
        {
            // まず、ファイルの文字コードを自動的に取得し、その文字コードで書きこむ。
            // ファイルが（存在しない場合はデフォルトの文字コードで書きこむ）
            Encoding _writeEncoding;
            // 文字コードを取得
            Encoding _encoding = ReadFileEncoding(_writeFileName_FullPath);
            if (_encoding == System.Text.Encoding.UTF8 || _encoding == System.Text.Encoding.ASCII)
            {
                // UTF-8を優先的に使う
                _writeEncoding = _encoding;
            }
            else if (_encoding == System.Text.Encoding.GetEncoding(MyTools.getEncodingTypeName(MyTools.EEncodingType.e02_SHIFTJIS・ウィンドウズのテキストファイルのデフォルト)))
            {
                ConsoleWriteLine("ReadFile: 文字コードがShift-JISのファイル「" + MyTools.getFileName_NotFullPath_LastFileOrDirectory(_writeFileName_FullPath) + "」をそのままの文字コードで書き込みました。\n■UTF-8ではないので、ファイル読み込み時は文字コード指定に注意してください！　ただし、プログラム上のstring型はUnicodeとして認識します。");
                // Shift-JIS
                _writeEncoding = _encoding;
            }
            else
            {
                // それ以外
                ConsoleWriteLine("ReadFile: 文字コードがUTF-8、Shift-JIS以外のファイル「" + MyTools.getFileName_NotFullPath_LastFileOrDirectory(_writeFileName_FullPath) + "」をUTF-8に変換して書き込みました。\n■元のファイルの文字コードがUTF-8ではなかったので、追加書き込み以外の場合、プログラム上のstring型が文字化けするかもしれません。");
                // (a)とりあえず無理やりにでもUTF-8に変換しておく
                _writeEncoding = Encoding.UTF8;
                // (b)とりあえずUTF-16で変換して返す。
                // ([MEMO]なんかウィンドウズや.NETではUTF-16を単に「Unicode」と呼んでいることが多い。ネットのスタンダードはUTF-8なのに。紛らわしいからやめて欲しい)
                //_writeEncoding = Encoding.Unicode;
            }
            return _writeEncoding;
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
                Encoding _encoding = MyTools.GetEncoding(_bytes);
                if (_encoding == System.Text.Encoding.UTF8 || _encoding == System.Text.Encoding.ASCII)
                {
                    // UTF-8を優先的に使う
                    _readString = MyTools.getString_ByBytes(MyTools.EEncodingType.e01_UTF8・ネット上のデファクトスタンダード, _bytes);
                }
                else if (_encoding == System.Text.Encoding.GetEncoding(MyTools.getEncodingTypeName(MyTools.EEncodingType.e02_SHIFTJIS・ウィンドウズのテキストファイルのデフォルト)))
                {
                    ConsoleWriteLine("ReadFile: 文字コードがShift-JISのファイル"+MyTools.getFileName_NotFullPath_LastFileOrDirectory(_readFileName_FullPath)+"を読み込みました。\n■UTF-8ではないので、ファイル書き込み時は文字コード指定に注意してください！　ただし、プログラム上のstring型はUnicodeとして認識します。");
                    // Shift-JIS
                    _readString = MyTools.getString_ByBytes(MyTools.EEncodingType.e02_SHIFTJIS・ウィンドウズのテキストファイルのデフォルト, _bytes);
                }
                else
                {
                    // それ以外
                    ConsoleWriteLine("ReadFile: 文字コードがUTF-8、Shift-JIS以外のファイル" + MyTools.getFileName_NotFullPath_LastFileOrDirectory(_readFileName_FullPath) + "を読み込みました。\n■UTF-8に判定されないので、プログラム上のstring型が文字化けしているかもしれません。とりあえず、一番可能性が高そうなUnicode(UTF-16)に変換しておきます。");
                    // (a)とりあえず無理やりにでもUTF-8に変換しておく
                    //_readString = System.Text.Encoding.UTF8.GetString(_bytes);
                    // (b)とりあえずUTF-16で変換して返す。
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
            // (a)これだとなぜか間に改行１行が余計に入る string[] readDataLines = readData.Split(System.Environment.NewLine.ToCharArray());
            // (b)改行を\nに統一するとうまくいく
            string dividedString = "\n";
            char[] dividedChars = dividedString.ToCharArray();
            readData = readData.Replace(System.Environment.NewLine, dividedString);
            string[] readDataLines = readData.Split(dividedChars); // (_n.ToCharArray()); // [ToDo]環境に寄らず，ちゃんと一行ずつ区切れてる？
            List<string> readLists = new List<string>(readDataLines);
            return readLists;
        }
        /// <summary>
        /// 引数ファイルの「,」区切りのテキストを読み込み、要素をリスト化したものを、さらに一行ごとにリストにしたものに格納して返します。
        /// </summary>
        /// <param name="readFile_FullPath"></param>
        /// <returns></returns>
        public static List<List<string>> ReadFile_ToCSV(string readFile_FullPath)
        {
            List<List<string>> readItemsList = new List<List<string>>();
            List<string> readLists = ReadFile_ToLists(readFile_FullPath);
            List<string> items = new List<string>();
            char[] _dividedString = ",".ToCharArray();
            foreach (string line in readLists)
            {
                items = new List<string>(line.Split(_dividedString));
                readItemsList.Add(items);
            }
            return readItemsList;
        }


        // ■エンコードを指定可能なstring型（今は使っていない）
        /// <summary>
        /// 引数ファイルのテキストを読み込み、文字列を返します。
        /// 基本のReadはReadFile_AllBytesを使ってエンコードを自動判別しているので、あまり使っていない。
        /// ただ、基本的にファイルの文字コードがわかっている場合は、この処理の方がメモリ消費も少ないし、早い。
        /// </summary>
        /// <param name="readFileName"></param>
        /// <returns></returns>
        //private static string ReadFile_EncodingTypeDefined(string readFile_FullPath, string _encodingTypeName)
        //{
        //    return ReadFile_EncodingTypeDefined(readFile_FullPath, System.Text.Encoding.GetEncoding(_encodingTypeName)); // shift_jis")など
        //}
        /// <summary>
        /// 引数ファイルのテキストを読み込み、文字列を返します。
        /// 基本のReadはReadFile_AllBytesを使ってエンコードを自動判別しているので、あまり使っていない。
        /// ただ、基本的にファイルの文字コードがわかっている場合は、この処理の方がメモリ消費も少ないし、早い。
        /// </summary>
        /// <param name="readFileName"></param>
        /// <returns></returns>
        private static string ReadFile_EncodingTypeDefined(string readFile_FullPath, System.Text.Encoding _encoding)
        {
            string readText = "";
            try
            {
                //読み込みファイル名指定（IntPtr型は @+string型で表現可能）
                FileStream fs = new FileStream(readFile_FullPath, FileMode.Open);
                //読み込みストリーム生成
                StreamReader reader = new StreamReader(fs, _encoding);

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
        /// 引数ファイル名にテキストを保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile(string _writeFileName_FullPath, string _writeText, FileMode _fileMode, System.Text.Encoding _encoding)
        {
            bool isOk = true;

            try
            {
                //保存ファイル名指定（Stream型は @+string型）
                FileStream fs = new FileStream(_writeFileName_FullPath, _fileMode); // Createは上書き。CreateNewは上書き禁止
                //書き込みストリーム生成
                StreamWriter writer = new StreamWriter(fs, _encoding); // _encodingのデフォルトは


                // 書き込みテキストの指定
                writer.WriteLine(_writeText);

                //ファイルへ書き込み
                writer.Flush();

                //ファイルのロックを解除
                writer.Close();

                // デバッグ表示用
                string shownWriteText = _writeText.Substring(0, Math.Min(10, _writeText.Length));
                Console.WriteLine(_writeFileName_FullPath + "に，\n" + shownWriteText + "\nを書き込みました．");
            }
            catch (IOException e)
            {
                isOk = false;
                MessageBox.Show("ファイル書き込みエラーです．ファイル名" + _writeFileName_FullPath + "\n" + e.ToString());

            }
            finally
            {
            }
            return isOk;
        }
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        public static bool WriteFile(string _writeFileName_FullPath, List<string> writeText, FileMode _fileMode, Encoding _encoding)
        {
            string data = "";
            for (int i = 0; i < writeText.Count; i++)
            {
                data += writeText[i] + _n;
            }
            return WriteFile(_writeFileName_FullPath, data, _fileMode, _encoding);
        }
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        public static bool WriteFile(string _writeFileName_FullPath, string writeText, Encoding _encoding)
        {
            return WriteFile(_writeFileName_FullPath, writeText, FileMode.Create, _encoding);
        }
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        public static bool WriteFile(string _writeFileName_FullPath, List<string> writeText, Encoding _encoding)
        {
            return WriteFile(_writeFileName_FullPath, writeText, FileMode.Create, _encoding);
        }
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        public static bool WriteFile(string _writeFileName_FullPath, string writeText, FileMode _fileMode)
        {
            return WriteFile(_writeFileName_FullPath, writeText, _fileMode, WriteFileEncoding(_writeFileName_FullPath));
        }
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        public static bool WriteFile(string _writeFileName_FullPath, List<string> writeText, FileMode _fileMode)
        {
            return WriteFile(_writeFileName_FullPath, writeText, _fileMode, WriteFileEncoding(_writeFileName_FullPath));
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。
        /// </summary>
        public static bool WriteFile(string writeFile_FullPath, string writeText)
        {
            return WriteFile(writeFile_FullPath, writeText, FileMode.Create, WriteFileEncoding(writeFile_FullPath));
        }
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile(string writeFile_FullPath, List<string> writeText)
        {
            return WriteFile(writeFile_FullPath, writeText, FileMode.Create, WriteFileEncoding(writeFile_FullPath));
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile_NoOverWrite(string writeFile_FullPath, string writeText, Encoding _encoding)
        {
            bool isOk = true;
            try
            {
                WriteFile(writeFile_FullPath, writeText, FileMode.CreateNew);

                //保存ファイル名指定（Stream型は @+string型）
                FileStream fs = new FileStream(writeFile_FullPath, FileMode.CreateNew); // CreateNewは上書き禁止
                //書き込みストリーム生成
                StreamWriter writer = new StreamWriter(fs, _encoding); // shift_jis")


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
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        public static bool WriteFile_NoOverWrite(string writeFile_FullPath, List<string> writeText, Encoding _encoding)
        {
            string data = "";
            for (int i = 0; i < writeText.Count; i++)
            {
                data += writeText[i] + _n;
            }
            return WriteFile_NoOverWrite(writeFile_FullPath, data, _encoding);
        }
        /// <summary>
        /// 引数ファイル名にテキスト（リストごとに改行）を保存します。
        /// </summary>
        public static bool WriteFile_NoOverWrite(string writeFile_FullPath, List<string> writeText)
        {
            return WriteFile_NoOverWrite(writeFile_FullPath, writeText);
        }
        /// <summary>
        /// 引数ファイル名にテキストを保存します。
        /// </summary>
        /// <param name="_writeFileName_FullPath"></param>
        /// <param name="_writeText"></param>
        /// <returns></returns>
        public static bool WriteFile_NoOverWrite(string writeFile_FullPath, string writeText)
        {
            return WriteFile_NoOverWrite(writeFile_FullPath, writeText);
        }
        /// <summary>
        /// 引数ファイルにテキストを追加保存します。
        /// </summary>
        public static bool WriteFile_Append(string writeFileName_FullPath, string writeText, Encoding _encoding)
        {
            bool isOk = true;
            try
            {
                // Appendなので，処理中にファイルが使えなくなる危険を防ぐため，念のためファイルをコピー
                System.IO.File.Copy(writeFileName_FullPath, writeFileName_FullPath + "copy", true);
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
                StreamWriter writer = new StreamWriter(fs, _encoding); // shift_jis")

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
        /// <summary>
        /// 引数ファイルにテキストを追加保存します。
        /// </summary>
        public static bool WriteFile_Append(string writeFileName_FullPath, List<string> writeText, Encoding _encoding)
        {
            string data = "";
            for (int i = 0; i < writeText.Count; i++)
            {
                data += writeText[i] + _n;
            }
            return WriteFile_Append(writeFileName_FullPath, data, _encoding);
        }
        /// <summary>
        /// 引数ファイルにテキストを追加保存します。
        /// </summary>
        public static bool WriteFile_Append(string writeFileName_FullPath, string writeText)
        {
            return WriteFile_Append(writeFileName_FullPath, writeText, WriteFileEncoding(writeFileName_FullPath));
        }
        /// <summary>
        /// 引数ファイルにテキストを追加保存します。
        /// </summary>
        public static bool WriteFile_Append(string writeFileName_FullPath, List<string> writeText)
        {
            return WriteFile_Append(writeFileName_FullPath, writeText, WriteFileEncoding(writeFileName_FullPath));
        }
        #endregion
        // ファイル名の取得系
        #region ファイルの拡張子無しの名前を取得: getFileLeftOfPeriodName
        /// <summary>
        /// ファイルの拡張子無しの名前（aaa.docの「aaa」）を取得します．
        /// ファイル名に「.」が存在しない場合は、ファイル名をそのまま返します。
        /// </summary>
        /// <param name="fileName"></param>
        public static string getFileLeftOfPeriodName(string fileName)
        {
            string fileName_NoDott = fileName;
            int dottIndex = fileName.LastIndexOf(".");
            if (dottIndex != -1)
            {
                fileName_NoDott = fileName.Substring(0, dottIndex);
            }
            return fileName_NoDott;
        }
        #endregion
        #region ファイルの拡張子を取得: getFileRightOfPeriodName
        /// <summary>
        /// ファイルの拡張子（aaa.docの「doc」）を小文字にして取得します．
        /// </summary>
        /// <param name="fileName"></param>
        public static string getFileRightOfPeriodName(string fileName)
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
        public static string getFileName_NotFullPath_LastFileOrDirectory(string fullPath)
        {
            string fullPath_Right = "";
            if (fullPath == null || fullPath == "") return fullPath_Right;
            // "/"での記述はこのクラスでは推奨していないが、ちゃんとチェックはする
            int lastDirectoryBeginIndex = fullPath.Substring(0, fullPath.Length - 1).LastIndexOf("/");
            if (lastDirectoryBeginIndex == -1)
            {
                lastDirectoryBeginIndex = fullPath.Substring(0, fullPath.Length - 1).LastIndexOf("\\");
            }
            fullPath_Right = fullPath.Substring(lastDirectoryBeginIndex + 1, fullPath.Length - lastDirectoryBeginIndex - 1);
            return fullPath_Right;
        }
        #endregion
        #region ディレクトリ名の取得: getDirectoryName
        /// <summary>
        /// 引数のファイルの絶対パス（例：C:\\aaa\\bbb\\sample.txt）から，一つ親であるディレクトリ名（例：C:\\aaa\\bbb、またはbbbだけ）を取ってきます．（いわゆる「..\\」の取得。（ただし"\\"は含まないので後で含めてください））
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
                fullPath_Right = getFileName_NotFullPath_LastFileOrDirectory(n_th_Path);
            }
            return fullPath_Right;
        }
        #endregion
        // ファイルの検索系
        #region ディレクトリ内の全てのファイル名を取得: getFileNames_FromDirectoryName
        /// <summary>
        /// ディレクトリ（フォルダ）内に存在する全てのファイル名（フルパス）一覧を取得します。
        /// サブディレクトリ（サブフォルダ）やサブディレクトリ内のファイルも含みます。
        /// ショートカット（ショートカットファイル／フォルダ）も含みます。
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">ディレクトリのフルパス（"C:\\...\\フォルダ"など）</param>
        public static List<string> getFileNames_FromDirectoryName(string _directoryName_FullPath)
        {
            return getFileNames_FromDirectoryName(_directoryName_FullPath, true, true, true, "*");
        }
        #endregion
        #region ディレクトリ内に存在する特定拡張子のファイル名一覧を取得: getFileNames_FromDirectoryName
        /// <summary>
        /// ディレクトリ（フォルダ）内に存在する特定拡張子のファイル名一覧を取得します。
        /// サブディレクトリ（サブフォルダ）やサブディレクトリ内のファイルも含むかどうかを指定できます。
        /// ショートカット（ショートカットファイル／フォルダ）を含めるかも設定できます。
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">ディレクトリのフルパス（"C:\\...\\フォルダ"など）</param>
        /// <param name="_isIncludingSubDirectoriesFiles">サブディレクトリ内のファイルを含めるかどうか（trueにすると、"C:\\...\\フォルダ\\サブフォルダA\\sample.txt"なども取得する）</param>
        /// <param name="_isIncludingShortcutDirectories">ショートカット（ショートカットファイル／フォルダ）を含めるかどうか（例えば検索文字列が"*"で、これをtrueにすると、"C:\\...\\フォルダ\\sample.txt - ショートカット.lnk"のリンク先である"C:\\...\\リンク先フォルダ\\sample.txt"なども取得できる）</param>
        /// <param name="_isFullPath_InEachFileName">ファイル名をフルパスにするかどうか（falseにすると、"sample.txt"などを取得する）</param>
        /// <param name="_fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ">"txt"や"jpg"や"mp3"など。全ての場合は"*"。今の実装では""でも"**"でも"***"でも"*"と同じ意味</param>
        /// <returns></returns>
        public static List<string> getFileNames_FromDirectoryName(string _directoryName_FullPath, bool _isIncludingSubDirectoriesFiles, bool _isIncludingShortcutDirectories, bool _isFullPath_InEachFileName, string _fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ)
        {
            string[] _拡張子たち = new string[1];
            _拡張子たち[0] = _fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ;
            return getFileNames_FromDirectoryName(_directoryName_FullPath, _isIncludingSubDirectoriesFiles, _isIncludingShortcutDirectories, _isFullPath_InEachFileName, _拡張子たち);
        }
        /// <summary>
        /// ディレクトリ（フォルダ）内に存在する特定拡張子のファイル名一覧を取得します。
        /// サブディレクトリ（サブフォルダ）やサブディレクトリ内のファイルも含むかどうか、フルパスで取得するかどうかを指定できます。
        /// ショートカット（ショートカットファイル／フォルダ）を含めるかも設定できます。
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">ディレクトリのフルパス（"C:\\...\\フォルダ"など）</param>
        /// <param name="_isIncludingSubDirectoriesFiles">サブディレクトリ内のファイルを含めるかどうか（trueにすると、"C:\\...\\フォルダ\\サブフォルダA\\sample.txt"なども取得する）</param>
        /// <param name="_isIncludingShortcutDirectories">ショートカット（ショートカットファイル／フォルダ）を含めるかどうか（例えば検索文字列が"*"で、これをtrueにすると、"C:\\...\\フォルダ\\sample.txt - ショートカット.lnk"のリンク先である"C:\\...\\リンク先フォルダ\\sample.txt"なども取得できる）</param>
        /// <param name="_isFullPath_InEachFileName">ファイル名をフルパスにするかどうか（falseにすると、"sample.txt"などを取得する）</param>
        /// <param name="_fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ">"txt"や"jpg"や"mp3"など。全ての場合は"*"。今の実装では""でも"**"でも"***"でも"*"と同じ意味</param>
        /// <returns></returns>
        public static List<string> getFileNames_FromDirectoryName(string _directoryName_FullPath, bool _isIncludingSubDirectoriesFiles, bool _isIncludingShortcutDirectories, bool _isFullPath_InEachFileName, params string[] _fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ)
        {
            List<string> _fileNameLists = new List<string>();

            // 拡張子のチェックは、大文字と小文字が混ざると確認できるか不安なので、小文字で統一した自前のメソッドで判定する
            string[] _拡張子たち = _fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ;
            string _拡張子０番目 = MyTools.getArrayValue(_fileType・拡張子小文字１から３ケタ＿全ての場合は無しやアスタリスクでＯＫ, 0);
            if (_拡張子０番目 == "" || _拡張子０番目 == "***" || _拡張子０番目 == "**" || _拡張子０番目 == "*")
            {
                // 一旦全て"*"を取得する
                // 全て格納
                _fileNameLists = getFileNames_FromSearchingDirectory(_directoryName_FullPath, 
                    _isIncludingSubDirectoriesFiles, true, "*", _isFullPath_InEachFileName);

            }
            else
            {
                // 指定した拡張子のファイルだけ格納
                foreach (string _拡張子 in _拡張子たち)
                {
                    _fileNameLists.AddRange(getFileNames_FromSearchingDirectory(_directoryName_FullPath,
                    _isIncludingSubDirectoriesFiles, true, "*." + _拡張子, _isFullPath_InEachFileName));
                }
            }
            return _fileNameLists;
            // 参考。感謝。
            // http://www.atmarkit.co.jp/fdotnet/dotnettips/053allfiles/allfiles.html
            //string[] files = Directory.GetFiles("c:\\");
            //string[] dirs = Directory.GetDirectories("c:\\");
            //string[] both = Directory.GetFileSystemEntries("c:\\");
        }
        #endregion
        #region ディレクトリ内のファイル検索: getFileNames_FromSearching*** / getFileName_FullPath・ファイル名を取得
        /// <summary>
        /// ディレクトリ内に存在する、検索文字列が含まれる（拡張子も含めた）ファイル名（フォルダ名も含む）一覧を取得します。見つからなかったら、Length=0の配列を返します。
        /// サブフォルダ（サブディレクトリ）内のファイルを含めるかを指定できます（trueにした場合、サブフォルダ自体の名前も検索対象に含まれます）。
        /// ショートカット（ショートカットファイル／フォルダ）を含めるかも設定できます。
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">検索フォルダ（ディレクトリ）のフルパス（"C:\\...\\フォルダ"など）</param>
        /// <param name="_isIncludingSubDirectoriesFiles">サブフォルダ（サブディレクトリ）内のファイルを含めるかどうか（例えば検索文字列が"*"で、これをtrueにすると、"C:\\...\\フォルダ\\サブフォルダA"や、"C:\\...\\フォルダ\\サブフォルダA\\sample.txt"なども取得する）</param>
        /// <param name="_isIncludingShortcutDirectories">ショートカット（ショートカットファイル／フォルダ）を含めるかどうか（例えば検索文字列が"*"で、これをtrueにすると、"C:\\...\\フォルダ\\sample.txt - ショートカット.lnk"のリンク先である"C:\\...\\リンク先フォルダ\\sample.txt"なども取得できる）</param>
        /// <param name="_searchString">検索文字列。全て"*"や拡張子だけを調べる"*.txt"などにも対応しています。""にすると一つも該当しなくなります。</param>
        /// <param name="_isFullPath_InEachFileName">ファイル名をフルパスにするかどうか（falseにすると、"sample.txt"などを取得する）</param>
        /// <returns></returns>
        public static List<string> getFileNames_FromSearchingDirectory(string _directoryName_FullPath, bool _isIncludingAllSubDirectoriesFiles, bool _isIncludingShortcutDirectories, string _searchString, bool _isFullPath_InEachFileName)
        {
            List<string> _fileNamesAll = new List<string>();
            string[] _fileNames1 = getFileNames_FromSearchingDirectory(_directoryName_FullPath, _isIncludingAllSubDirectoriesFiles, _searchString, _isFullPath_InEachFileName);
            _fileNamesAll.AddRange(_fileNames1);
            // ショートカットフォルダ（***.lnk）を含む場合
            if (_isIncludingShortcutDirectories == true)
            {
                // 普通の検索ではひっかからない、ショートカット（***.lnk）を取得
                string[] _shortcutAlias_FullPaths = getFileNames_FromSearchingDirectory(_directoryName_FullPath,
                    _isIncludingAllSubDirectoriesFiles, ".lnk", true);
                if (_shortcutAlias_FullPaths.Length > 0)
                {
                    // 検索語の"*"を削除して、string.Containで調べられるようにする
                    string _word = _searchString.Replace("*", "");
                    // ショートカットのリンク先（フルパス）配列を取得
                    List<string> _fileName2_Shortcut = new List<string>(); //new string[_shortcutAlias_FullPaths.Length];
                    int n = 0;
                    foreach (string _alias in _shortcutAlias_FullPaths)
                    {
                        // 実際に存在したら、リンク先（フルパス）を取得
                        string _fullPath = getShortcutLinkFileName_FullPath(_alias);
                        if (isExist(_fullPath) == true)
                        {
                            // 検索文字列（*を抜いたもの）が含まれていれば、これも検索結果に加える
                            if (_fullPath.Contains(_word) == true)
                            {
                                _fileName2_Shortcut.Add(_fullPath);
                                n++;
                            }
                        }
                        // リンク先（フルパス）がフォルダ（ディレクトリ）だったら、
                        // そのフォルダ内のファイル（フルパス）も全て検索
                        if (isFileName(_fullPath) == false)
                        {
                            // ※ショートカットのショートカットは、再帰的呼び出しで無限ループになる可能性があるので、しない
                            string[] _files = getFileNames_FromSearchingDirectory(_fullPath, true, _searchString, true);//これだと全てのファイルを取得するので結構時間がロスするgetFileNames_FromDirectoryName(_fullPath);
                            _fileName2_Shortcut.AddRange(_files);
                            n += _files.Length;
                        }
                    }
                    // ファイルパス整形
                    for (int i = 0; i < _fileName2_Shortcut.Count; i++)
                    {
                        // 整形
                        _fileName2_Shortcut[i] = getCheckedFilePath(_fileName2_Shortcut[i]);
                        // 名前だけにするかどうか
                        if (_isFullPath_InEachFileName == false)
                        {
                            _fileName2_Shortcut[i] = getFileName_NotFullPath_LastFileOrDirectory(_fileName2_Shortcut[i]);
                        }
                    }
                    // ショートカット以外のものとドッキング
                    _fileNamesAll.AddRange(_fileName2_Shortcut);
                }
            }
            return _fileNamesAll;
        }
        /// <summary>
        /// ディレクトリ内に存在する、検索文字列が含まれる（拡張子も含めた）ファイル名（フォルダ名も含む）一覧を取得します。見つからなかったら、Length=0の配列を返します。
        /// ※ショートカット（ファイル／フォルダは問わない）は含まれません。
        /// サブフォルダ（サブディレクトリ）内のファイルを含めるかを指定できます（trueにした場合、サブフォルダ自体の名前も検索対象に含まれます）。
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath">検索フォルダ（ディレクトリ）のフルパス（"C:\\...\\フォルダ"など）</param>
        /// <param name="_isIncludingSubDirectoriesFiles">サブフォルダ（サブディレクトリ）内のファイルを含めるかどうか（例えば検索文字列が"*"で、これをtrueにすると、"C:\\...\\フォルダ\\サブフォルダA"や、"C:\\...\\フォルダ\\サブフォルダA\\sample.txt"なども取得する）</param>
        /// <param name="_searchString">検索文字列。全て"*"や拡張子だけを調べる"*.txt"などにも対応しています。""にすると一つも該当しなくなります。</param>
        /// <param name="_isFullPath_InEachFileName">ファイル名をフルパスにするかどうか（falseにすると、"sample.txt"などを取得する）</param>
        /// <returns></returns>
        private static string[] getFileNames_FromSearchingDirectory(string _directoryName_FullPath, bool _isIncludingAllSubDirectoriesFiles, string _searchString, bool _isFullPath_InEachFileName)
        {
            string[] _fileNames = new string[0]; // 初期値はLength=0の配列
            try
            {
                //_searchStringの表記は"*.txt"などワイルドカードが入らないとちゃんと検索できないので、
                // もし"txt"など、"*"が入っていなかったら、"*"+"txt"+"*"にする
                if (_searchString.Contains("*") == false) _searchString = "*" + _searchString + "*";
                //基本は全部取得してから絞る？でもそれじゃあ重いよ絶対・・・だからやめとく。_searchString = "*";
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
            }catch(Exception e){
                ConsoleWriteLine("getFileNames_FromSearchingDirectory: 下記ディレクトリ内に「"+_searchString+"」が含まれるファイルは見つかりませんでした。"+_directoryName_FullPath);
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
            // ファイルパス整形
            for (int i = 0; i < _fileNames.Length; i++)
            {
                // 整形
                _fileNames[i] = getCheckedFilePath(_fileNames[i]);
                // 名前だけにするかどうか
                if (_isFullPath_InEachFileName == false)
                {
                    _fileNames[i] = getFileName_NotFullPath_LastFileOrDirectory(_fileNames[i]);
                }
            }
            // ファイルが見つからなかった場合のエラー
            if (_fileNames.Length == 0)
            {
                //ConsoleWriteLine("以下のディレクトリ内には「"+_searchString+"」を含むファイルは見つかりませんでした。 "+_directoryName_FullPath);
            }
            return _fileNames;
        }
        /// <summary>
        /// 引数のフルパスでないファイル名やディレクトリ（"sampleFile.txt"や"sampleDirectory"など）が、フルパスを指定したディレクトリ内にが実際に存在するかを調べ、存在していれば最初に見つかった該当ファイルのフルパスを返します。存在しなかったら""を返します。
        /// 　なお、内部ではgetCheckedFilePath()を使用してケアレスミスを修正してから確認します。
        /// 
        /// 　　※２つ以上見つかった場合を判定したい場合は、getFileNames_FromSearchingDirectoryを使用してください。
        /// </summary>
        public static string getFileName_FullPath_InSeachingDirectory(string _fileName_NotFullPath, string _searchingDirectoryName_FullPath)
        {
            string _FullPath = "";
            string _name = getCheckedFilePath(_fileName_NotFullPath);
            List<string> _fileNames = getFileNames_FromSearchingDirectory(_searchingDirectoryName_FullPath, true, true, _name, true);
            if (_fileNames.Count > 0)
            {
                _FullPath = _fileNames[0];
                // フルパスの整形も兼ねて、一応確認
                if (isExist(_FullPath) == false)
                {
                    int a = 0; // なにかがおかしい。
                }
            }
            return _FullPath;
        }
        #endregion
        #region 引数ファイル群内のファイル検索: getFileNames_FromFiles
        /// <summary>
        /// 引数１のファイル群（フルパスでもフルパスでなくても可、ディレクトリ（フォルダ）を含んでもよい）から、
        /// 引数２の検索語を含むファイルまたはディレクトリ（フォルダ）だけを取得します。
        /// ※string.Contains(_searchString）を調べているだけなので、ワイルドカード等は使えません。
        /// 
        /// </summary>
        /// <param name="_serchfileLists_FullPath"></param>
        /// <param name="_searchString"></param>
        /// <returns></returns>
        public static List<string> getFileNames_FromFileList(List<string> _serchfileLists_FullPath, string _searchString)
        {
            List<string> _fileNames = new List<string>();
            foreach (string _file in _serchfileLists_FullPath)
            {
                // (a)string.Contains(_searchString）を調べているだけなので、ワイルドカード等は使えないバージョン
                if (_file.Contains(_searchString))
                // (b)".*[.]?.*"などの正規表現を使えるバージョン
                //if(isMatch(_file, _searchString))
                // (c)"***.mp3"などのファイルでよくある検索語を使えるバージョンはまだ作ってない
                //if(isFileSearch(_file, _searchString))
                {
                    _fileNames.Add(_file);
                }
            }
            return _fileNames;
        }
        #endregion
        // ファイルの存在確認
        #region ■ファイルやディレクトリが実際に存在するか: isExist
        /// <summary>
        /// 引数にフルパスを指定したファイルやディレクトリが実際に存在するかを返します。なお、内部ではgetCheckedFilePath()を使用してケアレスミスを修正してから確認します。
        ///        　※ファイルの扱いでこの辺のケアレスミスが多いので、心配な場合は、このメソッドを呼び出して、予想外のエラーの少ないファイル名・ディレクトリ名を扱うよう心がけてください。
        /// 
        /// 　　　　参考：このメソッドは、System.IO.File.ExistsとSystem.IO.Directory.Existsの両方を使用して、実際に存在するかを判定しています。
        /// 　       ディレクトリ（フォルダ）の存在を調べるには、Directory.Existsメソッドを使います。
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
            if (_fileName_FullPath == "") return _isExist;
            if (isFileName(_fileName_FullPath) == true)
            {
                _isExist = System.IO.File.Exists(_fileName_FullPath);
            }
            else
            {
                _isExist = System.IO.Directory.Exists(_fileName_FullPath);
                // ショートカットファイルの場合、こちらで判定する
                if (_isExist == false && System.IO.File.Exists(_fileName_FullPath))
                {
                    _isExist = true;
                }
            }
            return _isExist;
        }
        #endregion
        #region フルパスがファイルかディレクトリか調べる: isFileName
        /// <summary>
        /// 引数のファイル名（フルパスでもフルパスでなくともどちらでもいい）であればtrueを返します（"."を含んでいるかを見ているだけで、実際に存在するかはチェックしていません）。
        /// "***.lnk"のショートカット（ファイル／ディレクトリかは問わない）や、"."が含まれないディレクトリ（フォルダ）の場合はfalseを返します。
        /// なお、内部ではgetCheckedFilePath()を使用してケアレスミスを修正してから確認します。
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        /// <returns></returns>
        public static bool isFileName(string _fileName_FullPath_or_NotFullPath)
        {
            _fileName_FullPath_or_NotFullPath = getCheckedFilePath(_fileName_FullPath_or_NotFullPath);
            string _name_OnlyRightest = getFileName_NotFullPath_LastFileOrDirectory(_fileName_FullPath_or_NotFullPath);
            bool _isFileName = _name_OnlyRightest.Contains(".");
            return _isFileName;
        }
        #endregion
        #region ショートカットのリンク先（フルパス）を取得: getShortcutFileName_FullPath
        /// <summary>
        /// 引数のショートカットのフルパス（例：　"C:\\aaa\\sample.exe - ショートカット.lnk"など）から、
        /// リンク先のフルパス（例： "C:\\bbb\\sample.exe"）を返します。
        /// 引数のファイルが存在しない場合は、""が返ります。
        /// </summary>
        public static string getShortcutLinkFileName_FullPath(string _shortcutAlias_FullPath)
        {
            _shortcutAlias_FullPath = getCheckedFilePath(_shortcutAlias_FullPath);
            if (isExist(_shortcutAlias_FullPath) == false) return "";

            // ショートカットのリンク先先（フルパス）の取得 http://detail.chiebukuro.yahoo.co.jp/qa/question_detail/q1344089703
            IWshShell_Class _shell = new IWshShell_Class();
            IWshShortcut_Class _shortcut = (IWshShortcut_Class)_shell.CreateShortcut(_shortcutAlias_FullPath);
            string _fileName_FullPath = _shortcut.TargetPath;
            //_shorcutAlias_FullPathと一緒 _shortcut.FullName;
            return _fileName_FullPath;
        }
        #endregion
        #region 新しいショートカットの作成: createShortcutFile
        /// <summary>
        /// 引数１のショートカットのフルパス（例：　"C:\\aaa\\sample.exe - ショートカット.lnk"など）から、
        /// 引数２のリンク先のフルパス（例： "C:\\bbb\\sample.exe"）を実行可能なショートカットファイル（※ショートカットフォルダは無理）を作成します。
        /// 引数２のファイルが存在しない場合は、falseが返ります。
        /// （※.NETではショートカットフォルダを作成することは難しいみたいです）
        /// </summary>
        public static bool createShortcutFile(string _shortcutAlias_FullPath, string _shortcutLinkFileName_FullPath)
        {
            bool _isSuccess = true;
            _shortcutAlias_FullPath = getCheckedFilePath(_shortcutAlias_FullPath);
            // リンク先が存在しない場合は、作成不可能
            _shortcutLinkFileName_FullPath = getCheckedFilePath(_shortcutLinkFileName_FullPath);
            if (isExist(_shortcutLinkFileName_FullPath) == false) return false;

            // http://dobon.net/vb/dotnet/file/createshortcut.html
            //WshShellを作成
            IWshRuntimeLibrary.WshShellClass shell = new IWshRuntimeLibrary.WshShellClass();
            //ショートカットのパスを指定して、WshShortcutを作成
            IWshRuntimeLibrary.IWshShortcut shortcut =
                (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(_shortcutAlias_FullPath);
            //リンク先
            shortcut.TargetPath = _shortcutLinkFileName_FullPath;
            //コマンドパラメータ 「リンク先」の後ろに付く
            shortcut.Arguments = "";// "/a /b /c";
            //作業フォルダ
            shortcut.WorkingDirectory = ""; // Application.StartupPath;
            //ショートカットキー（ホットキー）
            shortcut.Hotkey = "";// "Ctrl+Alt+Shift+F12";
            //実行時の大きさ 1が通常、3が最大化、7が最小化
            shortcut.WindowStyle = 1;
            //コメント
            shortcut.Description = "";// "テストのアプリケーション";
            //アイコンのパス 自分のEXEファイルのインデックス0のアイコン
            shortcut.IconLocation = "";// Application.ExecutablePath + ",0";


            //ショートカットを作成
            shortcut.Save();

            //後始末
            System.Runtime.InteropServices.Marshal.ReleaseComObject(shortcut);
            return _isSuccess;
        }
        #endregion
        #region ファイルやディレクトリのパスが正しいかどうかチェックして返す: getCheckedFilePath
        /// <summary>
        /// 引数のフルパスのファイルやディレクトリのパスが正しいかどうかチェックし、正しいパスを返します。
        /// 具体的には、ディレクトリの区切りが区切りが"/"なら"\\"に変換し、パスの最後に"\\"が入っていれば削除します。
        /// また、ファイルやディレクトリの存在を確認し、存在しない場合はメッセージボックスや標準出力に出力します。
        ///  　　　　※ファイルの扱いでこの辺のケアレスミスが多いので、心配な場合は、isExist()やこのメソッドを呼び出して、予想外のエラーの少ないファイル名・ディレクトリ名を扱うよう心がけてください。
        /// </summary>
        /// <param name="_fullPath"></param>
        /// <returns></returns>
        public static string getCheckedFilePath_AndShowErrorMessage(string _fullPath)
        {
            _fullPath = getCheckedFilePath(_fullPath);
            // ファイルの存在をチェック
            if (isExist(_fullPath) == false)
            {
                MessageBox.Show("getCheckedFilePath: ■エラー: ファイル／ディレクトリ名「" + getFileName_NotFullPath_LastFileOrDirectory(_fullPath) + "」\nフルパス\"" + _fullPath + "\"\nは存在しません。\nプログラムを続けますか？", "ファイルが見つからないエラー", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                ConsoleWriteLine("getCheckedFilePath: ■エラー: ファイル\"" + _fullPath + "\"は存在しません。");
            }
            return _fullPath;
        }
        /// <summary>
        /// ファイルやディレクトリのパスが正しいかどうかチェックし、正しいパスを返します。
        /// 具体的には、ディレクトリの区切りが区切りが"/"なら"\\"に変換し、パスの最後に"\\"が入っていれば削除します。
        /// 　　　　※ファイルの扱いでこの辺のケアレスミスが多いので、心配な場合は、isExist()やこのメソッドを呼び出して、予想外のエラーの少ないファイル名・ディレクトリ名を扱うよう心がけてください。
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

        #region ファイルコピー・移動・削除・名前の変更: copyFile/moveFile/deleteFile/renameFile
        /// <summary>
        /// 指定した元ファイルAを、コピー先ファイルBとしてコピーします。
        /// Bに指定したパスが同じファイルが既に存在していた場合、別の名前B+"(2)として"保存されます。
        /// Aのファイルが存在してなかった場合、falseを返します。
        /// Aが存在していて、かつBが正しくコピーされた場合、trueを返します。
        /// </summary>
        public static bool copyFile(string _A_baseFileName_FullPath, string _B_copyedNewFileName_FullPath)
        {
            if (isExist(_A_baseFileName_FullPath) == false)
            {
                return false;
            }
            else
            {
                try
                {
                    // すでにあるときは、例外IOExceptionがスローされる
                    System.IO.File.Copy(_A_baseFileName_FullPath, _B_copyedNewFileName_FullPath, false);
                }
                catch (Exception e)
                {
                    // Bに指定したパスが同じファイルが既に存在していた場合、別の名前B+"(2)として"保存されます。
                    System.IO.File.Copy(_A_baseFileName_FullPath, _B_copyedNewFileName_FullPath + "(2)", true);
                }
                return true;
            }
        }
        /// <summary>
        /// 指定した元ファイルAを、コピー先ファイルBとして移動します。
        /// Bに指定したパスが同じファイルが既に存在していた場合、別の名前B+"(2)として"保存されます。
        /// Aのファイルが存在してなかった場合、falseを返します。
        /// Aが存在していて、かつBに正しく移動した場合、trueを返します。
        /// </summary>
        public static bool moveFile(string _A_baseFileName_FullPath, string _B_movedNewFileName_FullPath)
        {
            if (isExist(_A_baseFileName_FullPath) == false)
            {
                return false;
            }
            else
            {
                try
                {
                    // すでにあるときは、例外IOExceptionがスローされる
                    System.IO.File.Move(_A_baseFileName_FullPath, _B_movedNewFileName_FullPath);
                }
                catch (Exception e)
                {
                    // Bに指定したパスが同じファイルが既に存在していた場合、別の名前B+"(2)として"保存されます。
                    System.IO.File.Move(_A_baseFileName_FullPath, _B_movedNewFileName_FullPath + "(2)");
                }
                return true;
            }
        }
        /// <summary>
        /// 指定したファイルを削除します。指定したファイルが存在しない場合は、falseを返します。
        /// 指定したファイルが存在しなくても例外はスローされません。
        /// </summary>
        public static bool deleteFile(string _fileName_FullPath)
        {
            if (isExist(_fileName_FullPath) == false)
            {
                return false;
            }
            else
            {
                System.IO.File.Delete(_fileName_FullPath);
                return true;
            }
        }
        /// <summary>
        /// 指定したファイルの名前を変更します。
        /// 第二引数にはフルパスでないファイル名（"新しいファイル名.txt"など）を指定してください。
        /// （※フルパスを使いたい場合は、moveFile()を使ってください。内部ではmoveFile()を使っています。）
        /// 指定したファイルが存在しない場合は、falseを返します。
        /// 指定したファイルが存在しなくても例外はスローされません。
        /// </summary>
        public static bool renameFile(string _fileName_FullPath, string _newfileName_NotFullPath)
        {
            if (isExist(_fileName_FullPath) == false)
            {
                return false;
            }
            else
            {
                string _directoryName = MyTools.getDirectoryName(_fileName_FullPath, true);
                moveFile(_fileName_FullPath, _directoryName + "\\" + _newfileName_NotFullPath);
                return true;
            }
        }
        #endregion


        // ■スレッド系（using System.Threadingが必要です）
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
        public static Thread threadSubMethod(ThreadDelegate _スレッドで並行作業するメソッド名)
        {
            Thread _thread = new Thread(new ThreadStart(_スレッドで並行作業するメソッド名));
            _thread.Start();
            return _thread;
        }
        /// <summary>
        /// 別スレッドとして非同期に処理したいメソッドを抽象化するデリゲートです。
        /// 引数はvoidにしか対応していません。引数ありのメソッドはThreadDelegateRefを使ってください。
        /// </summary>
        public delegate void ThreadDelegate();
        ///// <summary>
        ///// 新しくスレッドを立てて，引数のメソッドを並行して処理します．
        ///// 生成したスレッドを返します．
        ///// ・スレッドが終了するまで何も処理しない場合は，_thread.join()　を使ってください．
        ///// （・できれば，メソッドが終了したら，スレッドは _thread.Abort()　で破棄してください．）
        ///// 
        ///// ※マルチスレッドは，呼び出し過ぎ，lock(Object){}でロック処理をする，スレッドセーフなメソッドだけを引数にするなど，十分に注意してください．
        ///// http://ufcpp.net/study/csharp/sp_thread.html
        ///// </summary>
        ///// <param name="threadFadeOut・フェードアウトまでの処理"></param>
        //public static void threadSubMethod(ThreadDelegateRef _スレッドで並行作業するメソッド名, params object[] _refObjects)
        //{
        //    // 非同期メソッド開始
        //    IAsyncResult _ansyncResult = _スレッドで並行作業するメソッド名.BeginInvoke(_refObjects, null, null);
        //    // 非同期メソッドが終了するまで待つ
        //    _スレッドで並行作業するメソッド名.EndInvoke(_ansyncResult);
        //}
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
        public static void threadSubMethodRef(ThreadDelegateRef _スレッドで並行作業するメソッド名)
        {
            object[] _refObjects = _スレッドで並行作業するメソッド名.Method.GetParameters();
            // 非同期メソッド開始
            IAsyncResult _ansyncResult = _スレッドで並行作業するメソッド名.BeginInvoke(_refObjects, null, null);
            // 非同期メソッドが終了するまで待つ
            _スレッドで並行作業するメソッド名.EndInvoke(_ansyncResult);
        }
        /// <summary>
        /// 引数を入れる時のデリゲート。引数はオブジェクト型で渡してください。引数はvoidにしか対応していません。
        /// </summary>
        /// <param name="_refObjects"></param>
        public delegate void ThreadDelegateRef(params object[] _refObjects);
        #endregion





        // ■サウンド系
        // 他の様々な方法で再生したい場合は、MySoundクラスを参照してください。

        #region 簡易なサウンド再生のラッパーメソッド
        /// <summary>
        /// mp3/wav/midファイルをＢＧＭや効果音として（非同期で）再生します．
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static bool playSound(string _fileName_FullPath, bool _isRepeat)
        {
            return MySound_Windows.playSound(_fileName_FullPath, _isRepeat);
        }
        /// <summary>
        /// フォルダにあるmp3/wma/wavファイルをランダムで再生します。再生したサウンドファイルのフルパスを返します。
        /// </summary>
        /// <param name="_searchingDirectoryName_FullPath"></param>
        public static string playSound_MP3s_FromDirectory(string _directoryName_FullPath)
        {
            return MySound_Windows.playBGM_MP3s_FromDirectory(_directoryName_FullPath);
        }
        /// <summary>
        /// サウンドボリュームを調整します。
        /// </summary>
        /// <param name="_volume_0to1000"></param>
        public static int setVolume_Master(int _volume_0to1000)
        {
            return MySound_Windows.setVolume_Master(_volume_0to1000);
        }
        /// <summary>
        /// 再生中のmp3/wav/midファイルを停止します．
        /// </summary>
        /// <param name="_fileName_FullPath_or_NotFullPath"></param>
        public static void stopSound()
        {
            MySound_Windows.stopSound();
        }
        #endregion

        // □□□□　↑　以上は、できるだけ環境非依存のメソッドを書いてください。もちろん.NET Framework依存はですが。
        
        // System.Windowsが入るメソッドは、できるだけこれより下に書いてください。





        // □□□□　↓　以下は、環境依存のメソッドです。

        // Windows非依存、dll非依存にしたい場合は、まるごと削除orコメントアウトしてもかまいません。



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
            return parseString(_string, Microsoft.VisualBasic.VbStrConv.Wide);
        }
        /// <summary>
        /// 指定した文字列を半角に変換して返します。変換できない文字は、元の文字列のまま返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string getHankakuString(string _string)
        {
            return parseString(_string, Microsoft.VisualBasic.VbStrConv.Narrow);
        }
        /// <summary>
        /// 指定した文字列をひらがなに変換して返します。変換できない文字は、元の文字列のまま返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string getHiraganaString(string _string)
        {
            return parseString(_string, Microsoft.VisualBasic.VbStrConv.Hiragana);
        }
        /// <summary>
        /// 指定した文字列をカタカナに変換して返します。変換できない文字は、元の文字列のまま返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static string getKatakanaString(string _string)
        {
            return parseString(_string, Microsoft.VisualBasic.VbStrConv.Katakana);
        }
        /// <summary>
        /// プライベートメソッドです。Microsoft.VisualBasicに依存しているため、内部だけで使用します。
        /// 引数の文字列を指定した形式にに変換して返します。エラーが起きた場合は、もとの文字列を変換せずに返します。
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        private static string parseString(string _string, Microsoft.VisualBasic.VbStrConv _converingType)
        {
            string _returnedString = _string;
            try
            {
                _returnedString = Microsoft.VisualBasic.Strings.StrConv(_string, _converingType, 0x411);
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

            #region MessageBoxの呼び出し: MessageBoxShow
        /// <summary>
        /// メッセージボックスに文字列を表示します。選択肢は標準の「Yes」と「Cancel」で、「Yes」を選んだらtrue、「Cancel」を選んだらfalseを返します。
        /// </summary>
        /// <param name="_message"></param>
        /// <returns></returns>
        public static bool MessageBoxShow(string _message)
        {
            bool _isYes = true;
            DialogResult _result = System.Windows.Forms.MessageBox.Show(_message, "", System.Windows.Forms.MessageBoxButtons.OKCancel);
            if(_result == DialogResult.Yes) _isYes = true;
            if(_result == DialogResult.Cancel) _isYes = false;
            return _isYes;
        }
            #endregion

            #region マザーボードのBeep音: playBeepOnBoard
        /// <summary>
        /// マザーボードのBeep音を指定した周波数で鳴らします。
        /// ※周波数が設定でき、音量がミュート状態でもなるので、デバッグ時などに便利です。
        /// 内部では、Console.Beep(周波数, 音の継続時間ミリ秒)　を使っています。
        /// </summary>
        /// <param name="_BeepFrequency_37To32767"></param>
        /// <param name="_BeepMSec"></param>
        public static void playBeep(int _BeepFrequency_37To32767, int _BeepMSec)
        {
            Console.Beep(_BeepFrequency_37To32767, _BeepMSec);
        }
        /// マザーボードのBeep音を指定した音階で鳴らします。
        /// ※音階が設定でき、音量がミュート状態でもなるので、試しのメロディやデバッグ時などに便利です。
        /// 内部では、Console.Beep(周波数, 音の継続時間ミリ秒)　を使っています。
        /// </summary>
        /// <param name="_BeepFrequency_37To32767"></param>
        /// <param name="_BeepMSec"></param>
        public static void playBeep(EMusicalScale _EOnkai_440Hz_Is_s3A_LA, int _BeepMSec)
        {
            Console.Beep((int)_EOnkai_440Hz_Is_s3A_LA, _BeepMSec);
        }
        //
        /// <summary>
        /// 音階と周波数の対応表です。 (int)好きな音階の要素 とするだけで、周波数が取れます。
        /// なお、人間が聞こえる周波数は、20Hz-20000Hz程度とのことです。
        /// MP3は128kbps(12800Hz)か192bps(19200Hz)以上がよく切り捨てられますから、つまりそういうことですね。
        /// なお、Console.Beep()は37Hz以上じゃないと鳴りませんので、ご了承を。
        /// 
        /// 　※スケールの周波数は倍音なので、1上がる毎に２倍すれば計算できます
        /// が、小数点を取ってないせいもあり、±4Hz 位のズレが出るので、
        /// 音程が気になる場合はちゃんとスケールも各要素で指定してください。
        /// （例： (int)s3C__ド=262Hz ≒ s2C__ド=131Hz*2 ≒ s1C__ド=65Hz*4 ≒ s0C__ド=33Hz*8 
        /// 
        /// 
        /// 参考URL: 小数点精度 http://www.yk.rim.or.jp/~kamide/music/notes.html
        ///         整数精度（半音無し）と次元との対応表（独自解釈） http://merusaia.higoyomi.com/p_Game.html
        /// </summary>
        public enum EMusicalScale
        {
            _none_無音 = 0,
            s0C__ド = 33,
            s0Cs_ドSharp = 35,
            s0D__レ = 37,
            s0Ds_レSharp = 39,
            s0E__ミ = 41,
            s0F__ファ = 44,
            s0Fs_ファSharp = 46,
            s0G__ソ = 49,
            s0Gs_ソSharp = 52,
            s0A__ラ = 55,
            s0As_ラSharp = 58,
            s0B__シ = 62,

            s1C__ド = 65,
            s1Cs_ドSharp = 69,
            s1D__レ = 73,
            s1Ds_レSharp = 78,
            s1E__ミ = 82,
            s1F__ファ = 87,
            s1Fs_ファSharp = 92,
            s1G__ソ = 98,
            s1Gs_ソSharp = 104,
            s1A__ラ = 110,
            s1As_ラSharp = 117,
            s1B__シ = 123,

            s2C__ド = 131,
            s2Cs_ドSharp = 139,
            s2D__レ = 147,
            s2Ds_レSharp = 156,
            s2E__ミ = 165,
            s2F__ファ = 175,
            s2Fs_ファSharp = 185,
            s2G__ソ = 196,
            s2Gs_ソSharp = 208,
            s2A__ラ = 220,
            s2As_ラSharp = 233,
            s2B__シ = 247,

            s3C__ド = 262,
            s3Cs_ドSharp = 277,
            s3D__レ = 294,
            s3Ds_レSharp = 311,
            s3E__ミ = 330,
            s3F__ファ = 349,
            s3Fs_ファSharp = 370,
            s3G__ソ = 392,
            s3Gs_ソSharp = 415,
            s3A__ラ = 440,
            s3As_ラSharp = 466,
            s3B__シ = 494,

            s4C__ド = 523,
            s4Cs_ドSharp = 554,
            s4D__レ = 587,
            s4Ds_レSharp = 622,
            s4E__ミ = 659,
            s4F__ファ = 698,
            s4Fs_ファSharp = 740,
            s4G__ソ = 784,
            s4Gs_ソSharp = 830,
            s4A__ラ = 880,
            s4As_ラSharp = 932,
            s4B__シ = 988,

            s5C__ド = 1047,
            s5Cs_ドSharp = 1109,
            s5D__レ = 1175,
            s5Ds_レSharp = 1245,
            s5E__ミ = 1319,
            s5F__ファ = 1397,
            s5Fs_ファSharp = 1480,
            s5G__ソ = 1568,
            s5Gs_ソSharp = 1661,
            s5A__ラ = 1760,
            s5As_ラSharp = 1865,
            s5B__シ = 1976,

            s6C__ド = 2093,
            s6Cs_ドSharp = 2217,
            s6D__レ = 2349,
            s6Ds_レSharp = 2489,
            s6E__ミ = 2637,
            s6F__ファ = 2794,
            s6Fs_ファSharp = 2960,
            s6G__ソ = 3136,
            s6Gs_ソSharp = 3322,
            s6A__ラ = 3520,
            s6As_ラSharp = 3729,
            s6B__シ = 3951,

            s7C__ド = 4186,
            s7Cs_ドSharp = 4435,
            s7D__レ = 4699,
            s7Ds_レSharp = 4978,
            s7E__ミ = 5274,
            s7F__ファ = 5588,
            s7Fs_ファSharp = 5920,
            s7G__ソ = 6272,
            s7Gs_ソSharp = 6645,
            s7A__ラ = 7040,
            s7As_ラSharp = 7459,
            s7B__シ = 7902,

            s8C__ド = 8372,
            s8Cs_ドSharp = 8870,
            s8D__レ = 9397,
            s8Ds_レSharp = 9956,
            s8E__ミ = 10548,
            s8F__ファ = 11175,
            s8Fs_ファSharp = 11840,
            s8G__ソ = 12544,
            s8Gs_ソSharp = 13290,
            s8A__ラ = 14080,
            s8As_ラSharp = 14917,
            s8B__シ = 150804,

            s9C__ド = 16744,
            s9Cs_ドSharp = 17740,
            s9D__レ = 18795,
            s9Ds_レSharp = 19912,
            s9E__ミ = 21096,
            s9F__ファ = 22351,
            s9Fs_ファSharp = 23680,
            s9G__ソ = 25088,
            s9Gs_ソSharp = 26580,
            s9A__ラ = 28160,
            s9As_ラSharp = 29834,
            s9B__シ = 31609,

        }
        /// <summary>
        /// マザーボードのBeep音をある周波数で鳴らします。
        /// ※周波数が設定でき、音量がミュート状態でもなるので、デバッグ時などに便利です。
        /// 内部では、Console.Beep(周波数, 音の継続時間ミリ秒)　を使っています。
        /// </summary>
        /// <param name="_BeepFrequency_37To32767"></param>
        /// <param name="_BeepMSec"></param>
        public static void playBeepOnBoard(int _BeepFrequency_37To32767, int _BeepMSec)
        {
            Console.Beep(_BeepFrequency_37To32767, _BeepMSec);
        }
            #endregion

            #region Windowsシステムサウンドの再生: playSystemSound
        /// <summary>
        /// 予めWindowsに設定されているシステムサウンドを再生します。
        /// </summary>
        /// <param name="_systemSounTye_0None無し_1Asterisk情報_2Beep一般の警告_3Exclamation警告_4Handエラー_5Question問い合わせ"></param>
        public static void playSystemSound(int _systemSounTye_0None無し_1Asterisk情報_2Beep一般の警告_3Exclamation警告_4Handエラー_5Question問い合わせ)
        {
            int i = _systemSounTye_0None無し_1Asterisk情報_2Beep一般の警告_3Exclamation警告_4Handエラー_5Question問い合わせ;
            if (i == 1) { System.Media.SystemSounds.Asterisk.Play(); }
            else if (i == 2) { System.Media.SystemSounds.Beep.Play(); }
            else if (i == 3) { System.Media.SystemSounds.Exclamation.Play(); }
            else if (i == 4) { System.Media.SystemSounds.Hand.Play(); }
            else if (i == 5) { System.Media.SystemSounds.Question.Play(); }
        }
            #endregion

            #region フォームを表示してフォーカスを設定（最前面or最背面に表示）: showFormAndSetFocus
        /// <summary>
        /// フォームを表示してフォーカスを設定（最前面or最背面に表示）します。
        /// 既に表示されていたらfalseを返します。引数のフォームがnullでもfalseを返します。
        /// </summary>
        /// <param name="_shownNewForm">起動する新しいフォーム</param>
        /// <returns></returns>
        public static bool showFormAndSetFocus(Form _shownNewForm, bool _setIsFocused)
        {
            bool _isFirstShown = false;
            if (_shownNewForm == null) return false;
            // 既にフォーカスが当たっていたら、false
            if (_shownNewForm.Focused == true) _isFirstShown = false;

            // 1.フォームを表示
            _isFirstShown = true;
            _shownNewForm.Show();
            if (_setIsFocused == true)
            {
                // 2.最前面にする
                //(a)普通はこれでいけるはずだが、だめp_FParameter・パラメータ調整フォーム.Focus();
                //(b)これでもだめp_FParameter・パラメータ調整フォーム.BringToFront();
                //(c)これでもだめなことが多い_shownNewForm.Activate();
                //(d)ＷＩＮＡＰＩのSetForegroundWindow()をつかってだめ
                //System.IntPtr _handleWindow = getWindowHandle(_shownNewForm.Text);
                //SetForegroundWindow(_handleWindow);
                //(e)スレッドを無理やり変えるＷＩＮＡＰＩをつかってもダメ
                //setFormActivate(_shownNewForm);
                //(f)つまり、これを呼び出したメインのフォームが、メインスレッドなどで常に.Focus()や.Active()や.BringToFront()を呼び出していると、
                //   サブのフォームにはずっと回って来ない。
                //   こういう場合の最終解決案は、「常に最前面に表示する」これしかない。
                setFormShowing_AllwaysTopMost(_shownNewForm, true);
                //_shownNewForm.TopMost = true;
            }
            return _isFirstShown;
        }
            #endregion
                #region Formを「常に最前面に表示」する: setFormShowing_AlwaysTop
        /// <summary>
        /// Formを「常に最前面に表示するか」を設定します。前回の状態（true/false）を返します。
        /// </summary>
        public static bool setFormShowing_AllwaysTopMost(Form _form, bool _isShowingTopMost)
        {
            bool _isBefore = _form.TopMost;
            _form.TopMost = _isShowingTopMost;
            return _isBefore;
        }
                #endregion
                #region Formを「必ず」最前面に移動し、その Form にフォーカスを移動する: setFormActive
        //// 古い方法なので、あまり好まれないと思う。非推奨。
        //[DllImport("user32.dll")]
        //extern static int GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId) ;
        //[DllImport("user32.dll")]
        //extern static bool AttachThreadInput(int idAttach, int idAttachTo, bool fAttach) ;

        ///// <summary>
        ///// 古い方法なので、あまり好まれないと思う。非推奨。
        /////// Form を「必ず」最前面に移動し、その Form にフォーカスを移動する
        /////
        ///// this.Activate() だけでは、うまく Form が前面に移動しない場合がある。
        ///// これは既に前面にある「アプリ」が、これを拒否するからである(windows の仕様)。
        ///// 必ず前面に移動させるには、下記のようにスレッドのアタッチが必要である。
        ///// </summary>
        //public static void setFormActivate(Form _showedForm)
        //{
        //    // Thread のアタッチ
        //    int fore_thread = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero) ;
        //    int this_thread = System.Threading.Thread.CurrentThread.ManagedThreadId; // .NET2.0の仕様で変更。
        //    AttachThreadInput(this_thread, fore_thread, true) ;

        //    // this をアクティブ
        //    _showedForm.Activate() ;

        //    // Thread のデタッチ
        //    AttachThreadInput(this_thread, fore_thread, false) ;
        //}
        #endregion

            #region フォームを起動してそのフォームが閉じられるまで（同期的に）待つ: showForm_AndWaitClosingForm / wait_ForClosingForm
        /// <summary>
        /// フォームを起動してそのフォームが閉じられるまで（同期的に）待ちます。待ち終わったらtrueを返します。
        /// </summary>
        /// <param name="_shownNewForm">起動する新しいフォーム</param>
        /// <returns></returns>
        public static bool showForm_AndWaitClosingForm(Form _shownNewForm)
        {
            return wait_ForClosingForm(_shownNewForm);
        }
        /// <summary>
        /// フォームを起動してそのフォームが閉じられるまで（同期的に）待ちます。待ち終わったらtrueを返します。
        /// </summary>
        /// <param name="_shownNewForm">起動する新しいフォーム</param>
        /// <returns></returns>
        public static bool wait_ForClosingForm(Form _shownNewForm)
        {
            // フォームを非同期に呼び出す
            _shownNewForm.Show();
            _shownNewForm.Activate();
            _shownNewForm.Focus();
            // フォームが終了するまで待つ
            while (_shownNewForm.IsDisposed == false)
            {
                MyTools.wait_ByApplicationDoEvents(100);
            }
            return true;
        }
            #endregion

            #region イメージをフェードイン／アウトして描画
        /// <summary>
        /// イメージのフェードイン／アウトの0％～100％の分割数を示します．たとえば10だと，10％刻みでフェードイン／アウトします．
        /// </summary>
        public static int fadeAnimationNum = 10;
        /// <summary>
        /// 第一引数のグラフィック描画領域に，第二引数のイメージを，第三引数の指定秒間でフェードインして表示します．
        /// </summary>
        /// <param name="game"></param>
        /// <param name="p_graphBackImage"></param>
        /// <param name="fadeInMSec"></param>
        public static void drawImage_FadeIn(Graphics g, Image img, int fadeInMSec)
        {
            //グラフィックを読み込む
            //Image p_graphBackImage = Image.FromFile(@"C:\サンプル.jpg");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // フェードイン
            for (int i = 1; i <= fadeAnimationNum; i++)
            {
                // 半透明で画像を描画
                drawImage_Trans(g, img, (float)i * (1.0f / (float)fadeAnimationNum));
                // 待ち時間
                while (stopwatch.ElapsedMilliseconds <= i * (fadeInMSec / fadeAnimationNum))
                {
                    doEvents_WaitForOtherEvents();
                }
            }
        }
        /// <summary>
        /// 第一引数のグラフィック描画領域に，第二引数のイメージを，第三引数の指定秒間でフェードインして表示します．
        /// </summary>
        /// <param name="game"></param>
        /// <param name="p_graphBackImage"></param>
        /// <param name="fadeInMSec"></param>
        public static void drawImage_FadeOut(Graphics g, Image img, int fadeInMSec)
        {
            //グラフィックを読み込む
            //Image p_graphBackImage = Image.FromFile(@"C:\サンプル.jpg");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // フェードイン
            for (int i = fadeAnimationNum - 1; i >= 0; i--)
            {
                // 半透明で画像を描画
                drawImage_Trans(g, img, (float)i * (1.0f / (float)fadeAnimationNum));
                // 待ち時間
                while (stopwatch.ElapsedMilliseconds <= i * (fadeInMSec / fadeAnimationNum))
                {
                    doEvents_WaitForOtherEvents();
                }
            }
        }
            #endregion

            #region アクティブなウィンドウを取得
        /// <summary>
        /// 起動中のウィンドウの中で，アクティブなウィンドウを取得します。
        /// </summary>
        /// <returns></returns>
        public static IntPtr getActiveWindowHandle()
        {
            IntPtr activeWindowHandle = GetForegroundWindow();
            return activeWindowHandle;
        }
            #endregion

            #region ウィンドウタイトルのウィンドウハンドル（IntPtr型）を取得: getWindowHandle
        /// <summary>
        /// ウィンドウタイトルのウィンドウハンドル（IntPtr型）を取得します。
        /// </summary>
        /// <param name="_aplicationWindowTitle_OrFormText">アプリケーションの場合は上部のタイトルバーに表示されている文字列、フォームアプリケーションの場合はForm.Textのこと。</param>
        /// <returns></returns>
        public static System.IntPtr getWindowHandle(string _aplicationWindowTitle_OrFormText)
        {
            string _windowTitle = _aplicationWindowTitle_OrFormText;
            System.IntPtr _handleWindow = FindWindow(null, _windowTitle);
            return _handleWindow;
        }
            #endregion
            
            #region 指定したウィンドウタイトルのウィンドウをアクティブにする: setWindowToActive
        /// <summary>
        /// 指定したウィンドウをアクティブにする
        /// </summary>
        /// <param name="winTitle">
        /// アクティブにするウィンドウのタイトル</param>
        public static void setWindowToActivate(string winTitle)
        {
            IntPtr hWnd = FindWindow(null, winTitle);
            if (hWnd != IntPtr.Zero)
            {
                SetForegroundWindow(hWnd);
            }
        }
            #endregion

            #region 指定したウィンドウハンドルやコントロールからのメッセージを制御する: sendMessage_***
        /// <summary>
        /// 指定したウィンドウハンドルやコントロールからのメッセージを制御します。
        /// </summary>
        /// <param name="_Handle_ControlHandle_Or_WindowHandle">制御するウィンドウハンドル。コントロール.Handleなど</param>
        public static int sendMessage(IntPtr _Handle_ControlHandle_Or_WindowHandle, uint _MessageID_WM_Const, long _wParam, long _lParam)
        {
            int _result = SendMessage(_Handle_ControlHandle_Or_WindowHandle, _MessageID_WM_Const, _wParam, _lParam);
            return _result;
        }
        /// <summary>
        /// 指定したウィンドウハンドルやコントロールからの表示メッセージを有効／無効にします。
        /// </summary>
        /// <param name="_Handle_ControlHandle_Or_WindowHandle">制御するウィンドウハンドル。コントロール.Handleなど</param>
        /// <param name="_MessageID_WM_Const"></param>
        /// <param name="_isSendMessage_TrueOrFlase"></param>
        /// <returns></returns>
        public static int sendMessage_DRAW(IntPtr _Handle_ControlHandle_Or_WindowHandle, bool _isSendMessage_TrueOrFlase)
        {
			// ウィンドウの再描画を無効にする
            long _wParam = 1;
            if (_isSendMessage_TrueOrFlase == false) _wParam = 0;
			int _result = SendMessage(_Handle_ControlHandle_Or_WindowHandle, WM_SETREDRAW, _wParam, 0);
            return _result;
        }
            #endregion


            #region フォームのちらつき防止やNowLoading実装のための偽装フォーム設定 : setFormNowLoading_DamyPictureBox
        /// <summary>
        /// ※フォームのちらつき防止やNowLoading実装のために、偽装フォームを作成して、
        /// フォームを一時的に操作不能にしたい場合などに利用します。
        /// 
        /// 　　引数２がtrueのとき、「フタ画像」を持つコントロール（フォーム画面全体を上から覆うPictureBox）を作成して、
        /// フォームに上にかぶせるように表示します。返り値はその参照を返します。引数３で、NowLoadingのアニメーションを表示するかを設定します。
        /// 
        /// 
        /// 　　なお、引数２がfalseのとき、第三引数に指定されたコントロールの「フタ画像」を取ります（メモリを解放します）。引数３は使用しません。
        /// </summary>
        public static PictureBox setFormNowLoading_DamyPictureBox(Form _form, bool _isSetDamyImage, bool _isShowNowLoadingAnimation)
        {
            Image _image = null;
            return setFormNowLoading_DamyPictureBox(_form, _isSetDamyImage, _isShowNowLoadingAnimation, out _image);
        }
        /// <summary>
        /// ※フォームのちらつき防止やNowLoading実装のために、偽装フォームを作成して、
        /// フォームを一時的に操作不能にしたい場合などに利用します。
        /// 
        /// 　　引数２がtrueのとき、「フタ画像」を持つコントロール（フォーム画面全体を上から覆うPictureBox）を作成して、
        /// フォームに上にかぶせるように表示します。返り値はその参照を返します。引数３で、NowLoadingのアニメーションを表示するかを設定します。
        /// 
        /// 
        /// 　　なお、引数２がfalseのとき、第三引数に指定されたコントロールの「フタ画像」を取ります（メモリを解放します）。引数３は使用しません。
        /// 　　
        /// 　引数４には、最後に撮影されたフタ画像（これはメモリを解放しても消えません）を返します。
        /// </summary>
        public static PictureBox setFormNowLoading_DamyPictureBox(Form _form, bool _isSetDamyImage, bool _isShowNowLoadingAnimation, out Image _screenShotImage)
        {
            // outで返すイメージは、createDamyPictureBox内で変更される場合もあるが、staticに持っているものを使う
            _screenShotImage = p_screenshotImage;

            if (_isSetDamyImage == true)
            {
                // 偽装フォーム（フタPictureBox）を作成してフォームの上にかぶせる
                p_screenshotPictureBox = createDamyPictureBox(_form, _isShowNowLoadingAnimation);
            }
            else
            {
                // 偽装フォーム（フタPicitureBox）を廃棄して本来のフォームを表示
                if (p_screenshotPictureBox != null)
                {
                    p_screenshotPictureBox.Dispose();
                }
                p_screenshotPictureBox = null;
            }
            return p_screenshotPictureBox;
        }
                #region 上記メソッドの実装方法
        /// <summary>
        /// 最後に作られたフタ画像（フォームのスクリーンショット）
        /// </summary>
        private static Bitmap p_screenshotImage = null;
        /// <summary>
        /// フタ画像を持つコントロール、DamyPictureBox
        /// </summary>
        private static PictureBox p_screenshotPictureBox = null;
        /// <summary>
        /// フタ画像と一緒に表示される、NowLoadingのアニメーションの描画回数を管理する変数。
        /// </summary>
        private static int p_animcount = 0;
        /// <summary>
        /// 「フタ画像」を持つコントロール（フォーム画面全体を上から覆うPictureBox）を作成して、
        /// フォームに上にかぶせるように表示します。返り値はその参照を返します。
        /// 引数２で、NowLoadingのアニメーションを表示するかを設定します。
        /// </summary>
        /// <param name="_form"></param>
        /// <returns></returns>
        public static PictureBox createDamyPictureBox(Form _form, bool _isShowNowLoadingAnimation)
        {
            // 前回のメモリを破棄
            if (p_screenshotPictureBox != null)
            {
                p_screenshotPictureBox.Dispose();
            }
            // 「フタ」を作成
            p_screenshotPictureBox = new PictureBox();
            // フタの表示画像は、フォームのスクリーンショット
            p_screenshotPictureBox.BackgroundImage = screenshotWindow(_form);
            // フタの表示位置(x, y, width, height)は、フォームと同じ。
            p_screenshotPictureBox.Bounds = _form.ClientRectangle;
            if (_isShowNowLoadingAnimation == true)
            {
                // フタpictureBoxの描画イベントに、取ったスクリーンショット画像とNowLodingアニメを表示するイベントを追加
                p_screenshotPictureBox.Paint += new PaintEventHandler(DamyPictureBox_Paint);
            }
            else
            {
                // 既にイベントが追加されていた場合、削除
                p_screenshotPictureBox.Paint -= new PaintEventHandler(DamyPictureBox_Paint);
            }

            // フォームやコントロールに追加
            _form.Controls.Add(p_screenshotPictureBox);
            // フタを最前面に表示
            p_screenshotPictureBox.BringToFront();

            return p_screenshotPictureBox;
        }

        /// <summary>
        ///  クライアント領域（フォームやコントロール）のイメージをキャプチャして、その画像を返します。
        ///  
        /// 参考ＵＲＬ。感謝。http://codezine.jp/article/detail/3407?p=2#dl
        /// </summary>
        public static Bitmap screenshotWindow(Control _form_OrControl)
        {
            Control _control = _form_OrControl;
            // 有効状態であるクライアント領域のイメージをキャプチャ

            // ※Windows XPではPrintWindowの第３引数に
            // 　PW_CLIENTONLYを指定しても正しくキャプチャされないため、
            // 　引数0x0でウィンドウ全体を取得した後で、
            // 　クライアント領域部分をコピーするようにしている
            Bitmap winimg = new Bitmap(_control.Width, _control.Height);
            Graphics g = Graphics.FromImage(winimg);
            IntPtr hDC = g.GetHdc();
            PrintWindow(_control.Handle, hDC, 0x0);
            g.ReleaseHdc(hDC);
            g.Dispose();

            // クライアント領域の相対位置を取得
            Rectangle r1 = _control.Bounds; r1.X = r1.Y = 0;
            Rectangle r2 = _control.ClientRectangle;
            Rectangle r3 = new Rectangle(
                (r1.Width - r2.Width) / 2,
                (r1.Height - r2.Height - SystemInformation.CaptionHeight) / 2 + SystemInformation.CaptionHeight,
                r2.Width, r2.Height);

            // staticプロパティに格納
            // 前回のメモリを破棄
            if (p_screenshotImage != null)
            {
                p_screenshotImage.Dispose();
            }
            // フォームのクライアント部分をコピーする
            p_screenshotImage = new Bitmap(_control.ClientSize.Width, _control.ClientSize.Height);
            g = Graphics.FromImage(p_screenshotImage);
            g.DrawImage(winimg, _control.ClientRectangle, r3, GraphicsUnit.Pixel);
            g.Dispose();
            winimg.Dispose();

            // return p_screedshotImageで渡すと、元のp_screenshotImageがDispose()されると困るので、ここではコピーを返す。
            Bitmap _bitmap = new Bitmap(p_screenshotImage);
            return _bitmap;
        }
        /// <summary>
        /// フタ画像を持つコントロールDamyPictureBoxの描画イベントです。
        /// 描画回数によって、NowLoadingのアニメーションを表示します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void DamyPictureBox_Paint(object sender, PaintEventArgs e)
        {
            // ８個の「点」を円周上に描画する
            p_animcount++;
            if (p_animcount == 8) p_animcount = 0;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 背景色（透明度）
            int _alpha = (int)(255.0*0.0); // 0～255。0にするとスクリーンショットと変化なし。
            SolidBrush bgbrush = new SolidBrush(Color.FromArgb(_alpha, Color.White));
            g.FillRectangle(bgbrush, p_screenshotPictureBox.ClientRectangle);
            bgbrush.Dispose();

            SolidBrush brush = new SolidBrush(Color.SkyBlue);

            float x = (float)(p_screenshotPictureBox.Width - 32) / 2.0f;
            float y = (float)(p_screenshotPictureBox.Height - 32) / 2.0f;
            for (int i = 0; i < 8; i++)
            {
                // だんだん濃く、大きく
                float r = 3.0f * ((float)i * (8.0f / 70.0f) + 0.2f);

                System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
                m.Translate(32 / 3, 0, System.Drawing.Drawing2D.MatrixOrder.Append);
                // animcount描画メソッド呼び出し回数0-7に応じて、表示する角度を変更　→　アニメーション
                m.Rotate(45 * (i + p_animcount), System.Drawing.Drawing2D.MatrixOrder.Append);
                m.Translate(32 / 2 + x, 32 / 2 + y, System.Drawing.Drawing2D.MatrixOrder.Append);

                g.Transform = m;
                g.FillEllipse(brush, -r, -r, r * 2, r * 2);
            }
            // メモリの解放
            g.ResetTransform();
            brush.Dispose();
        }
        #endregion
        #endregion


        // その他のコントロール関連
        #region Windows.Formコントロール関連
        // ■リッチテキストボックスの色塗り
        /// <summary>
        /// ２つのリッチテキストボックスの異なる文字列をブロック単位で、前からColorDrawnNum個のみ色を付けます。返り値は等しい文字数を返します。baseStr1を間違いの無い見本として解析し、str2が途中で等しい文字列が指定M文字以下ずれている場合は、等しい文字列ブロックが指定N文字以上なら等しいと判断します。
        /// </summary>
        /// <param name="baseStr1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int checkEqualsTextBlock_BaseString1ToRichTextBox2_getEqualsLength(int MissedBlock_ColorDrawnNum, String baseString1, RichTextBox richTextBox2, int missLength_max_M, int equalStringBlock_length_min_N, bool isShowMissedMessage, Color missedTextBlock_color)
        {
            string equalsString, missedString;

            int equalsLength = getEqualsLength_checkString1ToString2(baseString1, richTextBox2.Text, missLength_max_M, equalStringBlock_length_min_N, isShowMissedMessage, out equalsString, out missedString);

            // 正解ブロックを取り出す
            string[] equalsBlock = equalsString.Split(" - ".ToCharArray());
            string[] missedBlock = missedString.Split(" - ".ToCharArray());

            // リッチテキストボックスの全てのテキストを、一旦デフォルト色で塗りつぶす
            richTextBox2.SelectAll();
            richTextBox2.SelectionColor = richTextBox2.ForeColor;
            richTextBox2.Select();

            int startIndex = 0;
            string searchedText = richTextBox2.Text;
            //int lossIndex=0, lossLength = 0; // 局所的な足りないブロック

            for (int i = 0; i < MissedBlock_ColorDrawnNum && i < missedBlock.Length; i++)
            {
                // 間違った文字列ブロック箇所に指定色を塗る
                if (missedBlock[i] != "*" && missedBlock[i] != "")
                {
                    startIndex = searchedText.IndexOf(missedBlock[i]);
                    richTextBox2.Select(startIndex, missedBlock[i].Length);
                    richTextBox2.SelectionColor = missedTextBlock_color;
                    // 次の位置へ
                    searchedText = searchedText.Substring(startIndex + missedBlock[i].Length);
                }
                else
                {
                    if (i == 0)
                    {
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = searchedText.IndexOf(equalsBlock[i - 1]) + equalsBlock[i].Length;
                    }
                    //searchedText.Substring(equalsBlock[_index], equalsBlock[_index].Length);
                    //int lossLength = searchedText.IndexOf(equalsBlock[_index+1]) - mouse_i;
                    //searchedText.Substring(mouse_i, lossLengh);
                    richTextBox2.Select();
                    richTextBox2.SelectedText = "*";
                    richTextBox2.SelectionStart = startIndex; // 開始点に"*"を挿入
                    richTextBox2.SelectionLength = 1;
                    richTextBox2.SelectionColor = missedTextBlock_color;
                    // 次の位置へ
                    searchedText = searchedText.Substring(startIndex);

                }
            }
            // リッチテキストの選択解除？
            //baseString1.

            return equalsLength;
        }
        /// <summary>
        /// ２つのリッチテキストボックスの異なる文字列をブロック単位で、全て色を付けます。返り値は等しい文字数を返します。baseStr1を間違いの無い見本として解析し、str2が途中で等しい文字列が指定M文字以下ずれている場合は、等しい文字列ブロックが指定N文字以上なら等しいと判断します。
        /// </summary>
        /// <param name="baseStr1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int checkEqualsTextBlock_BaseString1ToRicjTextBox2_getEqualsLength(string baseString, RichTextBox richTextBox2, int missLength_max_M, int equalStringBlock_length_min_N, bool isShowMissedMessage, Color missedTextBlock_color)
        {
            return checkEqualsTextBlock_BaseString1ToRichTextBox2_getEqualsLength(1000, baseString, richTextBox2, missLength_max_M, equalStringBlock_length_min_N, isShowMissedMessage, missedTextBlock_color);
        }

        
        // ■コントロールのサイズ調整
        /// <summary>
        /// 指定位置をはみ出さないよう、コントロールの端の位置を(x_min, y_min)から(x_max, y_max)までに調整します。
        /// </summary>
        /// <param name="control_sizes"></param>
        /// <param name="x_min"></param>
        /// <param name="y_min"></param>
        /// <param name="x_max"></param>
        /// <param name="y_max"></param>
        public static void adjustControlPosition(Control control, int x_min, int y_min, int x_max, int y_max)
        {
            Point point = adjustControlPosition(new Point(control.Left, control.Top), control.Size, x_min, y_min, x_max, y_max);
        }

        /// <summary>
        /// 指定位置をはみ出さないよう、コントロールの端の位置を(x_min, y_min)から(x_max, y_max)までに調整します。
        /// </summary>
        /// <param name="control_sizes"></param>
        /// <param name="x_min"></param>
        /// <param name="y_min"></param>
        /// <param name="x_max"></param>
        /// <param name="y_max"></param>
        public static void adjustControlPosition(Control control, Point point_min, Point point_max)
        {
            adjustControlPosition(control, point_min.X, point_min.Y, point_max.X, point_max.Y);
        }
        /// <summary>
        /// フォームのクライアント領域をはみ出さないよう、コントロールの端の位置を(0, 0)から(フォームのクライアント領域.X, フォームのクライアント領域.Y)までに調整します。
        /// </summary>
        /// <param name="control_sizes"></param>
        /// <param name="controlShownedform"></param>
        public static void adjustControlPosition(Control control, Form controlShownedform)
        {
            adjustControlPosition(control, 0, 0, getFormScreenSize(controlShownedform).Width, getFormScreenSize(controlShownedform).Height);
        }
        /// <summary>
        /// フォームのクライアント領域をはみ出さないよう、指定サイズ物体の座標の端の位置を(0, 0)から(フォームのクライアント領域.X, フォームのクライアント領域.Y)までに，調整した座標を返します。
        /// </summary>
        /// <param name="control_sizes"></param>
        /// <param name="controlShownedform"></param>
        public static Point adjustControlPosition(Point point, Size size, Form controlShownedform)
        {
            return adjustControlPosition(point, size, 0, 0, getFormScreenSize(controlShownedform).Width, getFormScreenSize(controlShownedform).Height);
        }

        #region オブジェクトや座標（四角形コントロール，点）がオブジェクト内にあるかどうか
        /// <summary>
        /// ある点A（位置）がコントロールB内にあるかどうかを返します．
        /// </summary>
        /// <param name="_objectA_Position"></param>
        /// <param name="_objectB"></param>
        /// <returns></returns>
        public static bool isObjectA_InsideOf_ObjectB(Point _pointA_Position, Control _contorolB)
        {
            return isObjectA_InsideOf_ObjectB(_pointA_Position, new Size(0, 0), _contorolB.Location, _contorolB.Size);
        }
        /// <summary>
        /// あるコントロールAがコントロールB内にあるかどうかを返します．
        /// </summary>
        /// <param name="_objectA_Position"></param>
        /// <param name="_objectB"></param>
        /// <returns></returns>
        public static bool isObjectA_InsideOf_ObjectB(Control _controlA, Control _contorlB)
        {
            return isObjectA_InsideOf_ObjectB(_controlA.Location, _controlA.Size, _contorlB.Location, _contorlB.Size);
        }
            #endregion

        #region （未実装）コントロールの背景を透過する（フォームの背景色（TransparentKey）だけでなく，他のコントロールも見える）
        // ※メソッドだけでやるのは難しい，現時点ではTransLabelユーザコントロール，もしくはMyTransLabelクラスで実装
        // http://youryella.wankuma.com/Library/Extensions/Label/Transparent.aspx
        //private void UpdateRegion()
        //{
        //    // コントロールの ClientSize と同じ大きさの Bitmap クラスを生成します。
        //    Bitmap foregroundBitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);

        //    // 文字列などの背景以外の部分を描画します。
        //    using (Graphics game = Graphics.FromImage(foregroundBitmap))
        //    {
        //        this.DrawForeground(game);
        //    }

        //    int w = foregroundBitmap.Width;
        //    int h = foregroundBitmap.Height;

        //    Rectangle rect = new Rectangle(0, 0, w, h);
        //    Region region = new Region(rect);

        //    // できた Bitmap クラスからピクセルの色情報を取得します。
        //    BitmapData bd = foregroundBitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        //    int stride = bd.Stride;
        //    int bytes = stride * h;
        //    byte[] bgraValues = new byte[bytes];
        //    Marshal.Copy(bd.Scan0, bgraValues, 0, bytes);
        //    foregroundBitmap.UnlockBits(bd);
        //    foregroundBitmap.Dispose();

        //    // 描画された部分だけの領域を作成します。
        //    int line;
        //    for (int y = 0; y < h; y++)
        //    {
        //        line = stride * y;
        //        for (int x = 0; x < w; x++)
        //        {
        //            // アルファ値が 0 は背景
        //            if (bgraValues[line + x * 4 + 3] == 0)
        //            {
        //                region.Exclude(new Rectangle(x, y, 1, 1));
        //            }
        //        }
        //    }

        //    // Region に描画された領域を設定します。
        //    this.Region = region;
        //}

        //private void DrawForeground(Graphics game)
        //{
        //    using (SolidBrush sb = new SolidBrush(this.ForeColor))
        //    {
        //        Rectangle r = new Rectangle(
        //            this.Padding.Left,
        //            this.Padding.Top,
        //            this.ClientRectangle.Width - this.Padding.Left - this.Padding.Right,
        //            this.ClientRectangle.Height - this.Padding.Top - this.Padding.Bottom
        //        );

        //        game.DrawString(this.Text, this.Font, sb, r);
        //    }
        //}
        #endregion
        #region （未実装）全てのコントロールを画像にして背景透明化
        //public static void setTransColor(params Control[] _controls)
        //{
        //    foreach (Control _con in _controls)
        //    {
        //        Graphics game = _con.CreateGraphics();
        //        //
        //    }
        //}
        #endregion

        #region 任意の単語を接続文字付きでコントロールに追加するメソッド（例：　「選択リスト: A1+B2」を表示）
        /// <summary>
        /// 指定されたフォームのコントロールの表示文字列（Textプロパティがあるもの）に，任意の単語を接続文字付き（例： +や,など）を付けて追加します．接続文字や，左側のデフォルト文字列（ラベル）も指定可能です．
        /// </summary>
        /// <param name="shownControl">Textプロパティがあるコントロール</param>
        /// <param name="shownText">任意の文字列（例：　B2）</param>
        /// <param name="connectingChar">"文字列を繋げる接続文字（例 + ）"</param>
        /// <param name="DefaultLeftString">左側に表示するラベル（例：　「選択リスト: A1+B2」の「選択リスト:」）</param>
        /// <returns></returns>
        public static void addWord_PerConnectinChar_OnFormControl(System.Windows.Forms.Control shownControl, string shownWord, string connectingChar, string DefaultLeftString)
        {
            // 指定されたコンポーネントのテキストを変更
            if (shownControl.Text.Equals(DefaultLeftString) == true)
            {
                shownControl.Text += shownWord;
            }
            else
            {
                shownControl.Text += connectingChar + shownWord;
            }
        }

        /// <summary>
        /// 指定されたフォームのコントロール（Textプロパティがあるもの）に，テキスト文字列を表示します．
        /// </summary>
        /// <param name="shownControl"></param>
        /// <param name="shownText"></param>
        /// <returns></returns>
        public static void addWord_PerConnectinChar_OnFormControl(System.Windows.Forms.Control shownControl, string shownText, string DefaultLeftString)
        {
            // 指定されたコンポーネントのテキストを変更
            shownControl.Text = DefaultLeftString + shownText;
        }
        #endregion
            #endregion

        // ■スクリーンショット
        #region スクリーンショット getScreenCapture***/screenCapture***
        /// <summary>
        /// 画面全体のスクリーンショットを取ったビットマップ画像を返します。
        /// </summary>
        /// <param name="p_usedForm"></param>
        /// <returns></returns>
        public static Bitmap getScreenCapture_FullScreen()
        {
            // 画面全体をキャプチャする ( to Bitmap )
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                            Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bmp);

            IntPtr src_hWnd = IntPtr.Zero;             // = Desktop 
            IntPtr srcHDC = GetDC(src_hWnd);  // 画面全体の HDC を取得
            IntPtr dstHDC = g.GetHdc();              // bmp     の HDC を取得 //Graphicsのデバイスコンテキストを取得

            BitBlt(                               // キャプチャの実行
                dstHDC, 0, 0, bmp.Width, bmp.Height,    //   コピー先 (= bmp)
                srcHDC, 0, 0,                           //   コピー元 (= 画面全体)
                SRCCOPY);                        //   動作 = コピー
            // 解放
            g.ReleaseHdc(dstHDC);
            ReleaseDC(src_hWnd, srcHDC);
            g.Dispose();

            return bmp;
        }
        
        /// <summary>
        /// 画面全体のスクリーンショットを取り、クリップボードに格納します。引数には、現在アクティブなウィンドウフォームを指定してください。
        /// </summary>
        /// <param name="p_usedForm"></param>
        /// <returns></returns>
        public static void screenCapture_ToClipBoard_FullScreen(Form usedForm)
        {
            // http://homepage3.nifty.com/midori_no_bike/CS/_valueString.html?userIO.265

            // 画面全体をキャプチャする ( to クリップボード )
            string name = "Program Manager";       //「画面全体」のウィンドウ名
            IntPtr hWnd = FindWindow(null, name);  //「画面全体」の hWnd 取得
            SetForegroundWindow(hWnd);             //「画面全体」をアクティブにする

            SendKeys.SendWait("%{PRTSC}");         // アクティブウィンドウをキャプチャ

            usedForm.Activate();                       // アクティブウィンドウを元に戻す

            // Note: 単純な SendKeys.SendWait("{PRTSC}") だけではダメ！
            //       理由は、キーコード "{PRTSC}" は "%{PRTSC}" に解釈されてしまうから。
        }
        /// <summary>
        /// アクティブなウィンドウのスクリーンショットを取り、クリップボードに格納します。
        /// </summary>
        /// <param name="p_usedForm"></param>
        /// <returns></returns>
        public static void screenCapture_ToClipBoard_FullScreen()
        {
            // アクティブなウィンドウをキャプチャする ( to クリップボード )
            SendKeys.SendWait("%{PRTSC}");  // "Alt + PrintScreen" キーコードをアクティブウィンドウへ送信する
        }
        #endregion
        #region マウスを隠す／表示する／位置を変更する
        /// <summary>
        /// 現在のマウスカーソルの表示／非表示を格納します．//[Cursor][表示][Show][Hide]:マウスカーソルが非表示かどうかを調べたいが，Cursorクラスには表示／非表示を格納する属性が見つからないため，自作している．
        /// </summary>
        public static bool isMouseCursorShow = true;

        /// <summary>
        /// マウスカーソルを非表示にします。アイコンが見えないだけで，入力は可能です。なお返り値は，マウスカーソルが表示の場合，非表示にしてtrueを返します。既に非表示の場合は何もせず，falseを返します．
        /// </summary>
        public static bool hideMouseCursor()
        {
            // [Tips][Cursor][Hide][Show]　HideとShowのメソッドは一対一にしなければならない！（何度も片方だけをしていると，一度もう一方をしても元に戻らない）
            if (isMouseCursorShow == true)
            {
                Cursor.Hide();
                isMouseCursorShow = false;
                return true;
            }
            return false;
        }
        /// <summary>
        /// マウスカーソルが非表示の場合，表示してtrueを返します。既に表示されている場合は何もせず，falseを返します．
        /// </summary>
        public static bool showMouseCursor()
        {
            if (isMouseCursorShow == false)
            {
                Cursor.Show();
                isMouseCursorShow = true;
                return true;
            }
            return false;
        }


        /// <summary>
        /// マウスカーソルを指定位置（グローバル）に移動し，trueを返します．マウスカーソルが非表示の場合は移動せず，falseを返します．
        /// </summary>
        /// <param name="movedPoint_Groval"></param>
        /// <returns></returns>
        public static bool changeMouseCursorPosition(Point movedPoint_Groval)
        {
            if (isMouseCursorShow == true)
            {
                Cursor.Position = movedPoint_Groval;
                return true;
            }
            return false;
        }
        public static bool changeMouseCursorPosition_FromFormPosition(Point movedPoint_FromFormPosition, Form usedForm)
        {
            if (isMouseCursorShow == true)
            {
                Point positionFromForm = MyTools.getFormClientPosition_InFullScreen(usedForm);
                positionFromForm.Offset(movedPoint_FromFormPosition);
                Cursor.Position = positionFromForm;
                return true;
            }
            return false;
        }
        /// <summary>
        /// マウスカーソルを砂時計に変更して，trueを返します．非表示の場合は何もせず，falseを返します．
        /// </summary>
        /// <returns></returns>
        public static bool changeMouseCursor_WaitCursor()
        {
            if (isMouseCursorShow == true)
            {
                Cursor.Current = Cursors.WaitCursor;
                return true;
            }
            return false;
        }
        /// <summary>
        /// マウスカーソルをデフォルト（矢印）に変更して，trueを返します．非表示の場合は何もせず，falseを返します．
        /// </summary>
        /// <returns></returns>
        public static bool changeMouseCursor_Default()
        {
            if (isMouseCursorShow == true)
            {
                Cursor.Current = Cursors.Default;
                return true;
            }
            return false;
        }
        // マウスカーソルを指定アイコンに変更して，trueを返します．非表示の場合は何もせず，falseを返します．
        // これはそんな簡単に出来ない．このアプリケーションForm上のDefalutやWaitをアイコンに変更するのが妥当のよう．
        // http://forums.microsoft.com/MSDN-JA/ShowPost.aspx?PostID=3207435&SiteID=7
        // 以下の方法でできる？？
        /// <summary>
        /// マウスカーソルを指定アイコンに変更して，trueを返します．非表示の場合は何もせず，falseを返します．
        /// </summary>
        /// <param name="_iconFileFullPath"></param>
        public static bool changeMouseCursor_Icon(string _iconFileFullPath)
        {
            if (isMouseCursorShow == true)
            {
                // Iconオブジェクトを作成
                Icon ico = new Icon(@_iconFileFullPath);

                // IconオブジェクトのハンドルからCursorオブジェクトを作成
                Cursor cur = new Cursor(ico.Handle);

                Cursor.Current = cur;
                return true;
            }
            return false;
        }
        #endregion


        // 自動入力系
        #region マウス自動入力: SendInput_Mouse_***
        #region 必要なプロパティ・DLL
        [DllImport("user32.dll")]
        private extern static uint SendInput(
            uint nInputs,   // INPUT 構造体の数(イベント数)
            INPUT[] pInputs,   // INPUT 構造体
            int cbSize     // INPUT 構造体のサイズ
            );

        [StructLayout(LayoutKind.Sequential)]  // アンマネージ DLL 対応用 struct 記述宣言
        public struct INPUT
        {
            public int type;  // 0 = INPUT_MOUSE(デフォルト), 1 = INPUT_KEYBOARD
            public MOUSEINPUT mouse;
            // Note: struct の場合、デフォルト(パラメータなしの)コンストラクタは、
            //       言語側で定義済みで、フィールドを 0 に初期化する。
        }

        [StructLayout(LayoutKind.Sequential)]  // アンマネージ DLL 対応用 struct 記述宣言
        public struct MOUSEINPUT
        {
            public int x;
            public int y;
            public int mouseData;  // amount of wheel movement
            public int eventFlag; // dwFlags
            public int time;  // time stamp for the event
            public IntPtr dwExtraInfo;
            // Note: struct の場合、デフォルト(パラメータなしの)コンストラクタは、
            //       言語側で定義済みで、フィールドを 0 に初期化する。
        }

        //  マウスイベント, dwFlags
        public const int MOUSEEVENTF_MOVED = 0x0001;
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;  // 左ボタン Down
        public const int MOUSEEVENTF_LEFTUP = 0x0004;  // 左ボタン Up
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;  // 右ボタン Down
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;  // 右ボタン Up
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;  // 中ボタン Down
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;  // 中ボタン Up
        public const int MOUSEEVENTF_WHEEL = 0x0080;
        public const int MOUSEEVENTF_XDOWN = 0x0100;
        public const int MOUSEEVENTF_XUP = 0x0200;
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        public const int screen_length = 0x10000;  // for MOUSEEVENTF_ABSOLUTE (この値は固定)

        // Note: マウスカーソルの移動方法は、Cursor.Position と SendInput() の2通りあるが、
        //       ドラッグ操作中の「マウスカーソルの移動」は、途中で割り込みが入らないよう
        //       SendInput() で行う方が安全である。

        // Note: MOUSEEVENTF_ABSOLUTE での座標指定は、特殊な座標単位系なので注意せよ。
        //       画面左上のコーナーが (0, 0)、画面右下のコーナーが (65535, 65535)である。

        // Note: No MOUSEEVENTF_ABSOLUTE での座標指定は、相対座標系になるが、単位が必ず
        //       しも 1px ではないので注意せよ。
        //       各 PC で設定された mouse speed と acceleration level に依存する。

        // Note: SendInput()パラメータの詳細は、MSDN『 MOUSEINPUT Structure 』を参照せよ。
        #endregion
        /// <summary>
        /// INPUT配列で定義されたマウスイベントを自動入力します．終了するまで自動入力時に割り込みはできません．
        /// </summary>
        /// <param name="_mouseInput"></param>
        public static void SendInput_Mouse_AllWaitForFinished(INPUT[] _mouseInput)
        {
            // http://homepage3.nifty.com/midori_no_bike/CS/_valueString.html?userIO.268
            //マウスの自動操作

            // マウスカーソルを、指定した位置へ移動したり、クリックしたりする

            // "user32.dll" の SendInput() を使い、マウスイベントを生成するので、
            // 他のアプリケーションのウィンドウ・オブジェクトをクリックすることも可能
            if (_mouseInput != null)
            {
                if (_mouseInput.Length > 0)
                {
                    SendInput((uint)_mouseInput.Length, _mouseInput, Marshal.SizeOf(_mouseInput[0]));
                }
            }
        }
        /// <summary>
        /// マウス自動入力中に，キーボードが押されたかどうかを示します．
        /// </summary>
        private static bool SendInput_Mouse_isKeyboardDown = false;
        /// <summary>
        /// INPUT配列で定義されたマウスイベントを自動入力します．自動入力を中止したい場合は，何かキーを押してください．
        /// </summary>
        /// <param name="_mouseInput"></param>
        public static void SendInput_Mouse(INPUT[] _mouseInput, Form _form1)
        {
            // http://homepage3.nifty.com/midori_no_bike/CS/_valueString.html?userIO.268
            //マウスの自動操作

            // マウスカーソルを、指定した位置へ移動したり、クリックしたりする

            // "user32.dll" の SendInput() を使い、マウスイベントを生成するので、
            // 他のアプリケーションのウィンドウ・オブジェクトをクリックすることも可能
            if (_mouseInput != null)
            {
                if (_mouseInput.Length > 0)
                {
                    int i = 0;
                    // KeyDownイベントに（デリゲートで）追加
                    _form1.KeyDown += new System.Windows.Forms.KeyEventHandler(keyboardDown);
                    SendInput_Mouse_isKeyboardDown = false;
                    //System.Windows.Input.Keyboard;
                    //Keyboard nowkeyboard = new Keyboard();
                    //Keyboard.IsToggled
                    //KeyEventArgs _e = ;
                    bool _isStop = false;
                    int _size = Marshal.SizeOf(_mouseInput[0]);
                    while (_isStop == false && i < _mouseInput.Length)
                    {
                        INPUT[] _input = new INPUT[1];
                        _input[0] = _mouseInput[i];
                        SendInput(1, _input, _size);
                        i++;
                        // キーボードキーが入力されたら中止
                        if (SendInput_Mouse_isKeyboardDown == true)
                        {
                            _isStop = true;
                        }
                    }
                }
            }
        }
        private static void keyboardDown(object sender, KeyEventArgs e)
        {
            if (e != null)
            {
                SendInput_Mouse_isKeyboardDown = true;
            }
        }
        private void SendInput_Mouse_test_SendInput()
        {
            // マウスカーソルの移動と、ドラッグ操作の例

            // マウスカーソルの移動 (絶対座標 200, 300 へ移動)
            Cursor.Position = new Point(200, 300);

            // ドラッグ操作の準備 (struct 配列の宣言)
            INPUT[] input = new INPUT[3];  // 計3イベントを格納

            // ドラッグ操作の準備 (第1イベントの定義 = 左ボタン Down)
            input[0].mouse.eventFlag = MOUSEEVENTF_LEFTDOWN;

            // ドラッグ操作の準備 (第2イベントの定義 = 絶対座標へ移動)
            input[1].mouse.x = screen_length / 2;  // X 座標 = 画面 1/2 (中央)
            input[1].mouse.y = screen_length / 2;  // Y 座標 = 画面 1/2 (中央)
            input[1].mouse.eventFlag = MOUSEEVENTF_MOVED | MOUSEEVENTF_ABSOLUTE;

            // ドラッグ操作の準備 (第3イベントの定義 = 左ボタン Up)
            input[2].mouse.eventFlag = MOUSEEVENTF_LEFTUP;

            // ドラッグ操作の実行 (計3イベントの一括生成)
            SendInput(3, input, Marshal.SizeOf(input[0]));
        }
        #endregion
        #region キーボード自動入力: SendInput_Keyboad
        /// <summary>
        /// 引数の文字列のキーを自動入力します．Ctrlなどの特殊キーは以下を参照してください．
        /// // Note: キーコード仕様 (詳細は MSDN の SendKeys クラス を参照)
        ///           A          ⇒  "A"
        ///           A B C      ⇒  "ABC"
        ///           Shift A    ⇒  "+A"
        ///           Ctrl  A    ⇒  "^A"
        ///           Alt   A    ⇒  "%A"
        ///           A を 10回  ⇒  "{A 10}"
        ///           F1         ⇒  "{F1}"
        ///           BackSpace  ⇒  "{BS}"
        ///           Del        ⇒  "{DEL}"
        ///           ↓         ⇒  "{DOWN}"
        ///           ↑         ⇒  "{UP}"
        ///           →         ⇒  "{RIGHT}"
        ///           ←         ⇒  "{LEFT}"
        ///           End        ⇒  "{RStick6_END}"
        ///           Enter      ⇒  "{ENTER}"
        ///           Esc        ⇒  "{ESC}"
        ///           Help       ⇒  "{HELP}"
        ///           Home       ⇒  "{RStick4_HOME}"
        ///           Insert     ⇒  "{INS}"
        ///           PageDown   ⇒  "{PGDN}"
        ///           PageUp     ⇒  "{PGUP}"
        ///           PrintScreen⇒  "{PRTSC}"  ⇒ スクリーンショットは"%{PRTSC}"　注) 下記 Note 参照。
        ///           Tab        ⇒  "{TAB}"
        ///           特別な記号として扱われる + ^ % ~ ( ) { } [ ] も、{ と } で囲む。
        /// </summary>
        /// <param name="_keyLists"></param>
        public static void SendInput_Keyboard(string _keyLists)
        {
            // http://homepage3.nifty.com/midori_no_bike/CS/_valueString.html?userIO.268
            SendKeys.Send(_keyLists);  // 送信し、メッセージが処理されるまで待機
            //SendKeys.SendWait(_keyLists) ;  // 送信し、メッセージが処理されるまで待機
            #region 参考メモ
            // Note: Windows ショートカットキー の機能も併用すれば、たいへん便利である。
            //       たとえば、SendKeys.SendWait("%{PRTSC}") だけで、「アクティブなウィンドウ
            //       のスナップショットをクリップボードに保存」することができる。

            // Note: キーコード "{PRTSC}" は、"%{PRTSC}" に解釈されてしまうようだ！
            //       よって、「画面全体のスナップショットをクリップボードに保存」したい場合、
            //       デスクトップをアクティブにしてから、SendKeys.SendWait("%{PRTSC}") せよ。
            //       具体例は、画面全体をキャプチャする(toクリップボード) を参照。

            // Note:「メッセージが処理されるまで待機」が不要な場合は、SendKeys.Send() 。

            // Note: この SendKeys.SendWait() メソッドは、アクティブなアプリケーションに対して
            //       しか、送信できない。
            //       C# には、他のアプリを 強制的にアクティブにする マネージ メソッド は存在し
            //       ないので、その場合は、アンマネージ メソッド を利用する必要がある。具体的
            //       には、他のアプリをアクティブにする を参照せよ。
            #endregion
        }
        #endregion

        #region ファイルダイアログを表示してロード・セーブ: loadFile_Dialog_***
        // ●読み込み
        /// <summary>
        /// ファイルダイアログを表示して、ファイルを読み込みます。
        /// </summary>
        /// <returns></returns>
        public static List<string> loadFile_Dialog_ToLists(string _fileType_拡張子)
        {
            List<string> _readString = new List<string>();
            OpenFileDialog ofd = openFileDialog("ファイルの読み込み", _fileType_拡張子, false);
            if (ofd != null)
            {
                _readString = ReadFile_ToLists(ofd.FileName);
            }
            return _readString;
        }
        /// <summary>
        /// ファイルダイアログを表示して、ファイルを読み込みます。
        /// </summary>
        /// <returns></returns>
        public static string loadFile_Dialog(string _fileType_拡張子)
        {
            string _readString = "";
            OpenFileDialog ofd = openFileDialog("ファイルの読み込み", _fileType_拡張子, false);
            if (ofd != null)
            {
                _readString = ReadFile(ofd.FileName);
            }
            return _readString;
        }
        /// <summary>
        /// ファイルダイアログを表示して、複数のファイルを読み込んでマージした文字列を返します。
        /// </summary>
        /// <returns></returns>
        public static string loadFiles_Dialog(string _fileType_拡張子)
        {
            string _readString = "";
            OpenFileDialog ofd = openFileDialog("ファイルの読み込み", _fileType_拡張子, true);
            if (ofd != null)
            {
                foreach (string _fileName in ofd.FileNames)
                {
                    _readString += ReadFile(_fileName);
                }
            }
            return _readString;
        }
        public static string p_loadFileDialog_LastDirectory = MyTools.getProjectDirectory();
        /// <summary>
        /// ファイルダイアログを表示して返します。「OK」以外をクリックしたときは、nullを返します。
        /// 拡張子は、"txt"などを入れることにより表示するファイルの種類を限定できます。空白""だと全てのファイルが表示されます。
        /// nullでない場合は、返したインスタンス.FileNameなどに選択したファイルを格納します。
        /// </summary>
        /// <param name="_title・ダイアログのタイトル"></param>
        /// <param name="_fileType・拡張子"></param>
        /// <param name="_CanSelectMultiFie・複数選択可能か"></param>
        /// <returns></returns>
        public static OpenFileDialog openFileDialog(string _title・ダイアログのタイトル, string _fileType・拡張子, bool _CanSelectMultiFie・複数選択可能か)
        {
            // 参考URL　http://dobon.net/vb/dotnet/form/openfiledialog.html
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();


            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            // ofd.FileName = "default.html";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリが表示される
            ofd.InitialDirectory = p_loadFileDialog_LastDirectory; //MyTools.getProjectDirectory(); // @"C:\";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            if (_fileType・拡張子 != "" && _fileType・拡張子 != "***")
            {
                string _type = _fileType・拡張子;
                ofd.Filter =
                    _type + "ファイル(*." + _type + ")|*." + _type + "|すべてのファイル(*.*)|*.*";
                //[ファイルの種類]ではじめに
                //「拡張子名ファイル」が選択されているようにする
                ofd.FilterIndex = 1;
            }
            else
            {
                ofd.FilterIndex = 1;
            }
            //タイトルを設定する
            ofd.Title = _title・ダイアログのタイトル;
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            // ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            // ofd.CheckPathExists = true;

            // 複数のファイルを選択できるようにする
            ofd.Multiselect = _CanSelectMultiFie・複数選択可能か;

            // 注意：ファイルダイアログで、プログラムから開くで何か開こうとしたら固まる場合がある
            //（特にwavファイルをWMPで再生しようとしたら、固まる場合がある。例外も取れない。原因は不明）
            try
            {
                DialogResult _result = ofd.ShowDialog();
                // 「OK」以外をクリックしたら、nullにする
                if (_result != DialogResult.OK)
                {
                    ofd = null;
                }
                else
                {
                    p_loadFileDialog_LastDirectory = MyTools.getDirectoryName(ofd.FileName, true);
                }
            }
            catch (Exception _e)
            {
                // 例外が発生したら、とりあえずダイアログを閉じる
                ofd = null;
            }

            return ofd;
        }

        // ●草案：　ストリーミング処理
        public static int p_loadFiles_Dialog_StreamBufferSize = 1000;
        public static char[] p_loadFiles_Dialog_StreamBuffer・ストリームバッファ;
        private static System.IO.Stream p_loadFiles_stream;
        private static System.IO.StreamReader p_loadFiles_streamReader;
        private static Thread p_loadFiles_streamReaderThread;
        /// <summary>
        /// ●草案：　読みこんだファイルを、ストリーミング処理します。
        /// </summary>
        /// <param name="_fileType_拡張子"></param>
        /// <returns></returns>
        public static System.IO.Stream loadFiles_Dialog_Stream(string _fileType_拡張子, int _streamSize・ストリーミングバッファ文字数)
        {
            p_loadFiles_Dialog_StreamBufferSize = _streamSize・ストリーミングバッファ文字数;
            p_loadFiles_Dialog_StreamBuffer・ストリームバッファ = new char[p_loadFiles_Dialog_StreamBufferSize];
            OpenFileDialog ofd = openFileDialog("ファイルの読み込み", _fileType_拡張子, false);
            p_loadFiles_stream = ofd.OpenFile();
            if (p_loadFiles_stream != null)
            {
                //内容を読み込み、表示する
                p_loadFiles_streamReader =
                    new System.IO.StreamReader(p_loadFiles_stream);
                //string _readString = sr.ReadToEnd(); // Readで一文字ずつでも、ReadToLineで一行ずつもＯＫ
                p_loadFiles_streamReaderThread = new Thread(new ThreadStart(loadFiles_Dialog_StreamThread));
            }
            return p_loadFiles_stream;
        }
        public static void loadFiles_Dialog_StreamThread()
        {
            while (p_loadFiles_streamReader.EndOfStream == false)
            {
                p_loadFiles_streamReader.Read(p_loadFiles_Dialog_StreamBuffer・ストリームバッファ, 0, p_loadFiles_Dialog_StreamBufferSize);
            }
            //閉じる
            p_loadFiles_streamReader.Close();
            p_loadFiles_stream.Close();
        }

        // ●書き込み
        public static string p_saveFileDialog_LastDirectory = MyTools.getProjectDirectory();
        /// <summary>
        /// ファイルダイアログを表示して、引数の文字列をファイルに書き込みます。
        /// </summary>
        /// <returns></returns>
        public static bool saveFile_Dialog(string _fileData, string _fileType_拡張子)
        {
            SaveFileDialog sfd = saveFileDialog("ファイルの保存", _fileType_拡張子);
            if (sfd != null)
            {
                WriteFile(sfd.FileName, _fileData);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// ファイルダイアログを表示して返します。「OK」以外をクリックしたときは、nullを返します。
        /// nullでない場合は、返したインスタンス.FileNameなどに選択したファイルを格納します。
        /// </summary>
        /// <param name="_title・ダイアログのタイトル"></param>
        /// <param name="_fileType・拡張子"></param>
        /// <returns></returns>
        public static SaveFileDialog saveFileDialog(string _title・ダイアログのタイトル, string _fileType・拡張子)
        {
            // 参考URL　http://msdn.microsoft.com/ja-jp/library/sfezx97z(VS.85).aspx
            //SaveFileDialogクラスのインスタンスを作成
            SaveFileDialog sfd = new SaveFileDialog();


            //はじめのファイル名を指定する
            //はじめに「ファイル名」で表示される文字列を指定する
            // ofd.FileName = "default.html";
            //はじめに表示されるフォルダを指定する
            //指定しない（空の文字列）の時は、現在のディレクトリ（普通はマイドキュメント？）が表示される
            sfd.InitialDirectory = p_saveFileDialog_LastDirectory; // @"C:\";
            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            if (_fileType・拡張子 != "***")
            {
                string _type = _fileType・拡張子;
                sfd.Filter =
                    _type + "ファイル(*." + _type + ")|*." + _type + "|すべてのファイル(*.*)|*.*";
                //[ファイルの種類]ではじめに
                //「拡張子名ファイル」が選択されているようにする
                sfd.FilterIndex = 1;
            }
            else
            {
                sfd.FilterIndex = 1;
            }
            //タイトルを設定する
            sfd.Title = _title・ダイアログのタイトル;
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            sfd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            // ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            // ofd.CheckPathExists = true;

            // 「OK」以外をクリックしたら、nullにする
            DialogResult _result = sfd.ShowDialog();
            if (_result != DialogResult.OK)
            {
                sfd = null;
            }
            else
            {
                p_saveFileDialog_LastDirectory = MyTools.getDirectoryName(sfd.FileName, true);
            }
            return sfd;
        }
        #endregion
        // ●フォーム系（System.Windows依存）

        // ウィンドウの透明化処理
        #region フォームの輪郭を指定画像にして，その他を透明（下のウィンドウを操作可能）にする: setFormImage_AndBackgroundTrans
        /// <summary>
        /// 第一引数のフォームの輪郭を，第二引数のイメージにし，その他をイメージの右上端ビットの色で透過（下のウィンドウを操作できる）します．
        /// </summary>
        /// <param name="_imageForm"></param>
        /// <param name="_formBackGroundImage"></param>
        public static void setFormImage_AndSetBackgroundTrans(Form _imageForm, Image _formBackGroundImage)
        {
            setFormImage_AndSetBackgroundTrans(_imageForm, new Bitmap(_formBackGroundImage));
        }
        /// <summary>
        /// 第一引数のフォームの輪郭を，第二引数のイメージにし，その他をイメージの右上端ビットの色で透過（下のウィンドウを操作できる）します．
        /// </summary>
        /// <param name="_imageForm"></param>
        /// <param name="_formBackGroundImage"></param>
        public static void setFormImage_AndSetBackgroundTrans(Form _imageForm, Bitmap _formBackGroundImage)
        {
            //フォームの境界線をなくす
            _imageForm.FormBorderStyle = FormBorderStyle.None;
            //大きさを適当に変更
            _imageForm.Size = new Size(100, 100);
            //透明にする色
            Color transColor = _formBackGroundImage.GetPixel(_formBackGroundImage.Width - 1, 0);
            //グラフィックを透明にする
            _formBackGroundImage.MakeTransparent(transColor);
            //背景画像を指定する
            _imageForm.BackgroundImage = _formBackGroundImage;
            //背景色を指定する
            _imageForm.BackColor = transColor;
            //透明色を指定する
            _imageForm.TransparencyKey = transColor;
        }
        #endregion
        
        
        
        // 画面サイズや位置
        #region フォームのスクリーンサイズや位置取得: getMouseCursorPositon/ getFormClientPosion / etc
        /// <summary>
        /// フォームの任意のコントロールを(0,0)とした、現在のマウス位置を相対位置で取ってきます。
        /// </summary>
        /// <param name="form1"></param>
        /// <param name="_control"></param>
        /// <returns></returns>
        public static Point getMouseCursorPosition_ByControl(Form form1, Control _control)
        {
            Point _p1 = getFormScreenPoint_ByMouseCursorPoint(form1);

            Point _point = new Point(Math.Max(0 ,_p1.X-_control.Left), Math.Max(0 ,_p1.Y-_control.Top));
            return _point;
        }
        /// <summary>
        /// フォームのタイトルバーや枠幅を除いた、フォームのスクリーンサイズ（表示領域＝クライアントサイズ）を取得します。
        /// </summary>
        /// <param name="nasatlxForm"></param>
        /// <returns></returns>
        public static Size getFormScreenSize(Form form1)
        {
            Size size = form1.ClientSize;
            return size;
        }
        /// <summary>
        /// フォームのタイトルバーや枠幅を除いた、画面上からのフォームのスクリーン開始座標（表示開始位置＝クライアント表示・描画位置）を取得します。
        /// </summary>
        /// <param name="nasatlxForm"></param>
        /// <returns></returns>
        public static Point getFormClientPosition_InForm(Form form1)
        {
            Point point = new Point((form1.Size.Width - form1.ClientSize.Width) / 2, (form1.Size.Height - form1.ClientSize.Height) - (form1.Size.Width - form1.ClientSize.Width) / 2);
            return point;
        }
        /// <summary>
        /// フォームのタイトルバーや枠幅を除いた、画面上からのフォームのスクリーン開始座標（表示開始位置＝クライアント表示・描画位置）を取得します。
        /// </summary>
        /// <param name="nasatlxForm"></param>
        /// <returns></returns>
        public static Point getFormClientPosition_InFullScreen(Form form1)
        {
            // 枠線のバーの幅
            int _barX = (form1.Size.Width - form1.ClientSize.Width) / 2;
            Point point = new Point(form1.Left + _barX, form1.Top + (form1.Size.Height - form1.ClientSize.Height) - _barX);
            return point;
        }
        /// <summary>
        /// 現在のマウス座標から，対応フォーム座標に変換します（対応するフォームのスクリーン開始座標を(0,0)とした相対位置を取ってきます）。
        /// </summary>
        /// <param name="cursorPosition_now"></param>
        /// <param name="nasatlxForm"></param>
        /// <returns></returns>
        public static Point getFormScreenPoint_ByMouseCursorPoint(Point cursorPosition_now, Form form1)
        {
            // これだと何故かぴったりじゃない
            //int left = cursorPosition_now.X - nasatlxForm.Left - (nasatlxForm.Size.Width - nasatlxForm.ClientSize.Width)/2;
            //int top = cursorPosition_now.Y - nasatlxForm.Top - (nasatlxForm.Size.Height - nasatlxForm.ClientSize.Height) + (nasatlxForm.Size.Width - nasatlxForm.ClientSize.Width) / 2;
            // これだとぴったり
            int left = cursorPosition_now.X - getFormClientPosition_InFullScreen(form1).X;
            int top = cursorPosition_now.Y - getFormClientPosition_InFullScreen(form1).Y;
            Point screenPoint = new Point(left, top);
            return screenPoint;
        }
        /// <summary>
        /// 現在のマウスの位置（Cursor.Positionを用います）に対応するフォームのスクリーン開始座標を(0,0)とした相対位置を取ってきます。
        /// </summary>
        /// <param name="nasatlxForm"></param>
        /// <returns></returns>
        public static Point getFormScreenPoint_ByMouseCursorPoint(Form form1)
        {
            return getFormScreenPoint_ByMouseCursorPoint(Cursor.Position, form1);
        }
        /// <summary>
        /// フォーム座標から，絶対座標に変換します（対応するフォームのスクリーン開始座標を(0,0)とした相対位置を取ってきます）。
        /// </summary>
        /// <returns></returns>
        public static Point getMousePosition_Absolute_ByWindowForm(Point _windowFormPosition, Form _form1)
        {
            // これだと何故かぴったりじゃない
            //int left = cursorPosition_now.X - nasatlxForm.Left - (nasatlxForm.Size.Width - nasatlxForm.ClientSize.Width)/2;
            //int top = cursorPosition_now.Y - nasatlxForm.Top - (nasatlxForm.Size.Height - nasatlxForm.ClientSize.Height) + (nasatlxForm.Size.Width - nasatlxForm.ClientSize.Width) / 2;
            // これだとぴったり
            int left = _windowFormPosition.X + getFormClientPosition_InFullScreen(_form1).X;
            int top = _windowFormPosition.Y + getFormClientPosition_InFullScreen(_form1).Y;
            Point _absolutePosition = new Point(left, top);
            return _absolutePosition;
        }
        /// <summary>
        /// 絶対座標から，指定したWindowフォーム座標に変換します（対応するフォームのスクリーン開始座標を(0,0)とした相対位置を取ってきます）。
        /// </summary>
        /// <returns></returns>
        public static Point getMousePosition_WindowForm_ByAbsolute(Point _absolutePosition, Form _form1)
        {
            //int left = cursorPosition_now.X - nasatlxForm.Left - (nasatlxForm.Size.Width - nasatlxForm.ClientSize.Width)/2;
            //int top = cursorPosition_now.Y - nasatlxForm.Top - (nasatlxForm.Size.Height - nasatlxForm.ClientSize.Height) + (nasatlxForm.Size.Width - nasatlxForm.ClientSize.Width) / 2;
            // これだと何故かぴったりじゃない
            int left = _absolutePosition.X - getFormClientPosition_InFullScreen(_form1).X;
            int top = _absolutePosition.Y - getFormClientPosition_InFullScreen(_form1).Y;
            Point _windowFormPoint = new Point(left, top);
            return _windowFormPoint;
        }

        #endregion
        #region ディプレイ画面のスクリーンサイズを取得: getScreenSize
        public static Size getScreenSize()
        {
            return Screen.PrimaryScreen.Bounds.Size;
        }
        #endregion
        #region フルスクリーンモードに設定: setFullScreenMode
        //http://www.atmarkit.co.jp/fdotnet/dotnettips/199fullscreen/fullscreen.html
        public static Form setFullScreenMode_form1;
        /// <summary>
        /// フルスクリーン・モードかどうかのフラグ
        /// </summary>
        private static bool setFullScreenMode_isScreenMode;
        /// <summary>
        /// フルスクリーン表示前のウィンドウの状態を保存する
        /// </summary>
        private static FormWindowState setFullScreenMode_prevFormState;
        /// <summary>
        /// 通常表示時のフォームの境界線スタイルを保存する
        /// </summary>
        private static FormBorderStyle setFullScreenMode_prevFormStyle;
        /// <summary>
        /// 通常表示時のウィンドウのサイズを保存する
        /// </summary>
        private static Size setFullScreenMode_prevFormSize;
        /// <summary>
        /// 指定したフォームをフルスクリーンにします。このメソッドはstaticプロパティを使って通常表示時のウィンドウの状態を保存するため，フルスクリーンにできるフォームは一つだけにしてください。
        /// </summary>
        /// <param name="_form1"></param>
        /// <param name="_isFullScreen_or_NormalMode"></param>
        public static void setFullScreenMode(Form _form1, bool _isFullScreen_or_NormalMode)
        {
            setFullScreenMode_form1 = _form1;
            if (setFullScreenMode_isScreenMode == false)
            {
                if (_isFullScreen_or_NormalMode == true)
                {
                    // ＜フルスクリーン表示への切り替え処理＞

                    // ウィンドウの状態を保存する
                    setFullScreenMode_prevFormState = _form1.WindowState;
                    // 境界線スタイルを保存する
                    setFullScreenMode_prevFormStyle = _form1.FormBorderStyle;

                    // 0. 「最大化表示」→「フルスクリーン表示」では
                    // タスク・バーが消えないので、いったん「通常表示」を行う
                    if (_form1.WindowState == FormWindowState.Maximized)
                    {
                        _form1.WindowState = FormWindowState.Normal;
                    }

                    // フォームのサイズを保存する
                    setFullScreenMode_prevFormSize = _form1.ClientSize;

                    // 1. フォームの境界線スタイルを「none」にする
                    _form1.FormBorderStyle = FormBorderStyle.None;
                    // 2. フォームのウィンドウ状態を「最大化」する
                    _form1.WindowState = FormWindowState.Maximized;

                    // フルスクリーン表示をONにする
                    setFullScreenMode_isScreenMode = true;
                }
            }
            else
            {
                if (_isFullScreen_or_NormalMode == false)
                {
                    // ＜通常表示／最大化表示への切り替え処理＞

                    // フォームのウィンドウのサイズを元に戻す
                    _form1.ClientSize = setFullScreenMode_prevFormSize;

                    // 0. 最大化表示に戻す場合にはいったん通常表示を行う
                    // （フルスクリーン表示の処理とのバランスと取るため）
                    if (setFullScreenMode_prevFormState == FormWindowState.Maximized)
                    {
                        _form1.WindowState = FormWindowState.Normal;
                    }

                    // 1. フォームの境界線スタイルを元に戻す
                    _form1.FormBorderStyle = setFullScreenMode_prevFormStyle;

                    // 2. フォームのウィンドウ状態を元に戻す
                    _form1.WindowState = setFullScreenMode_prevFormState;

                    // フルスクリーン表示をOFFにする
                    setFullScreenMode_isScreenMode = false;
                }
            }
        }
        /// <summary>
        /// 以前setFullScreenModeされたフォームがフルスクリーンかどうかを返します。
        /// </summary>
        /// <param name="nasatlxForm"></param>
        /// <returns></returns>
        public static bool isFullScreenMode()
        {
            bool _isFullScreen = false;
            if (setFullScreenMode_form1.WindowState == FormWindowState.Maximized)
            {
                _isFullScreen = true;
            }
            return _isFullScreen;
        }
        #endregion
        // コントロール
        #region ランダムに配置するコントロールの位置取得: getControlPosition_***
        /// <summary>
        /// フォーム上に指定サイズのがオブジェクトが置けるランダムな座標を，(x,y)のPointクラス型で返します．
        /// </summary>
        public static Point getControlPosition_CanShowOnForm_Random(Form controlShownForm, Size control_sizes, Random random)
        {
            int x = random.Next(getFormScreenSize(controlShownForm).Width - control_sizes.Width);
            int y = random.Next(getFormScreenSize(controlShownForm).Height - control_sizes.Height);
            Point randomPoint = new Point(x, y);
            return randomPoint;
        }
        /// <summary>
        /// フォーム上に指定サイズのがオブジェクトが置けるランダムな座標を，(x,y)のPointクラス型で返します．
        /// </summary>
        public static Point getControlPosition_CanShowOnForm_Random(Form controlShownForm, int control_width, int control_height, Random random)
        {
            return getControlPosition_CanShowOnForm_Random(controlShownForm, new Size(control_width, control_height), random);
        }
        /// <summary>
        /// フォーム上に指定サイズのがオブジェクトが置けるランダムな座標を，(x,y)のPointクラス型で返します．
        /// </summary>
        /// <param name="controlShownForm"></param>
        /// <param name="control_sizes"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static Point getControlPosition_ConstDistance_Random(Form controlShownForm, Size shownControl_sizes, Point otherControl_CenterPosition_FromDistance, double Distance_From_shownControl_To_othercontrol, Random random, int _thisRandom_seed___Randomize_minus1)
        {
            List<Point> circlePoints = getCirclePoints(otherControl_CenterPosition_FromDistance, Distance_From_shownControl_To_othercontrol);
            // ※ここのrandomは，成功したもののみMyRandomManagerで保存している
            Random _thisRandom;
            if (_thisRandom_seed___Randomize_minus1 == -1)
            {
                _thisRandom = new Random();
            }
            else
            {
                _thisRandom = new Random(_thisRandom_seed___Randomize_minus1);
            }
            int randomNum = _thisRandom.Next(0, circlePoints.Count);
            Point point = circlePoints[randomNum];
            int maxWidth = (getFormScreenSize(controlShownForm).Width - shownControl_sizes.Width);
            int maxHeight = (getFormScreenSize(controlShownForm).Height - shownControl_sizes.Height);
            int loopNum = 0;
            int loopMax = circlePoints.Count;
            while (!(point.X >= 0 && point.X < maxWidth && point.Y >= 0 && point.Y < maxHeight) && loopNum < loopMax)
            {
                randomNum = _thisRandom.Next(0, circlePoints.Count);
                point = circlePoints[randomNum];
                loopNum++;
            }
            // 円周上でウィンドウ表示領域内にある点だけ保存
            //merusaia.Nakatsuka.Tetsuhiro.Experiment.CMyRandomManager.MyRandom myRandom = random as merusaia.Nakatsuka.Tetsuhiro.Experiment.CMyRandomManager.MyRandom;
            //CMyRandomGenerator・ランダム生成者.MyRandom myRandom = random as CMyRandomGenerator・ランダム生成者.MyRandom;
            Random myRandom = new Random();
            if (myRandom != null)
            {
                // 保存するのは、MyRandom型でないとできないよ
                //myRandom.saveRandomValue(randomNum);
            }
            // 条件にマッチせずループを抜けた場合も，フォーム内に収める
            if (loopNum == loopMax)
            {
                point = adjustControlPosition(point, shownControl_sizes, controlShownForm);
            }
            return point;

        }
        #endregion
        // テキストボックス関連
        #region テキストボックスの末尾を表示: showTextBox_*** / showRichTextBox
        // 参考。感謝。http://dobon.net/vb/dotnet/control/tbscrolltolast.html
        /// <summary>
        /// テキストボックスの末尾を表示します。
        ///         /// （これをしなくても。TextBox1.Text += _str の代わりに、
        /// richTextBox1.ApplendText(_str)で追加すれば、自動的にスクロールされる場合があります。
        /// 　これで事足りる場合は、このメソッドを呼び出さないでください。このメソッドはフォーカスについて面倒くさいことをしています。）
        /// （※RichTextBoxとTextBoxは親子関係にありません。継承できないので注意してください。）
        /// </summary>
        /// <param name="TextBox1"></param>
        public static void showTextBox_EndLine(TextBox TextBox1)
        {
            TextBox1.SelectionStart = Math.Max(TextBox1.Text.Length-2, 0);   //カレット位置を末尾に移動
            //TextBoxは要らないらしい。TextBox1.Focus();                                   //テキストボックスにフォーカスを移動
            TextBox1.ScrollToCaret();                           //カレット位置までスクロール
        }
        /// <summary>
        /// テキストボックスの任意の行数目の場所を表示します。
        /// （※RichTextBoxとTextBoxは親子関係にありません。継承できないので注意してください。）
        /// </summary>
        /// <param name="TextBox1"></param>
        public static void showTextBox_FromLine(TextBox TextBox1, int _lineNum)
        {
            int _index = TextBox1.GetFirstCharIndexFromLine(_lineNum);
            if (_index == -1) _index = Math.Max(TextBox1.Text.Length - 2, 0);
            TextBox1.SelectionStart = _index;                   //カレット位置を末尾に移動
            //TextBoxは要らないらしい。TextBox1.Focus();                                   //テキストボックスにフォーカスを移動
            TextBox1.ScrollToCaret();                           //カレット位置までスクロール
        }
        /// <summary>
        /// リッチテキストボックスの任意の行数目の場所を表示します。
        ///  （※RichTextBoxとTextBoxは親子関係にありません。継承できないので注意してください。）
        /// </summary>
        /// <param name="TextBox1"></param>
        public static void showRichTextBox_FromLine(RichTextBox _richTextBox1, int _lineNum)
        {
            int _index = _richTextBox1.GetFirstCharIndexFromLine(_lineNum);
            if (_index == -1) _index = Math.Max(_richTextBox1.Text.Length - 2, 0);
            _richTextBox1.SelectionStart = _index;                   //カレット位置を末尾に移動
            _richTextBox1.Focus();                                   //リッチテキストボックスにフォーカスを移動
            _richTextBox1.ScrollToCaret();                           //カレット位置までスクロール
        }
        /// <summary>
        /// リッチテキストボックスの末尾を表示します。
        /// （※RichTextBoxとTextBoxは親子関係にありません。継承できないので注意してください。）
        /// </summary>
        /// <param name="_richTextBox1"></param>
        public static void showRichTextBox_EndLine(RichTextBox _richTextBox1)
        {
            _richTextBox1.SelectionStart = Math.Max(_richTextBox1.Text.Length, 0); //カレット位置を末尾に移動
            _richTextBox1.Focus();                                      //リッチテキストボックスにフォーカスを移動
            _richTextBox1.ScrollToCaret();                              //カレット位置までスクロール
        }
        /// <summary>
        /// showRichTextBox_EndLine_UnshowCursorメソッドで、最後にスクロールした時間です。
        /// これを格納してチェックしないと、高速で呼び出した際にスクロール時がガタガタとちらつくようです。
        /// 二つ以上のリッチテキストボックスで交互にこのメソッドを呼び出す場合には対応していませんので、使う際は注意してください。
        /// </summary>
        public static int p_showRichTextBox_EndLine_UnshowCursor__beforeScrollTime = 0;
        /// <summary>
        /// showRichTextBox_EndLine_UnshowCursorメソッドで、最後にスクロールした行数です。
        /// これを格納してチェックしないと、高速で呼び出した際にスクロール時がガタガタとちらつくようです。
        /// 二つ以上のリッチテキストボックスで交互にこのメソッドを呼び出す場合には対応していませんので、使う際は注意してください。
        /// </summary>
        public static int p_showRichTextBox_EndLine_UnshowCursor__beforeLineNo = 0;
        /// <summary>
        /// リッチテキストボックスの末尾を表示します。テキスト選択カーソル「|」を映しません。
        /// （※RichTextBoxとTextBoxは親子関係にありません。継承できないので注意してください。）
        /// 二つ以上のリッチテキストボックスで交互にこのメソッドを呼び出す場合には対応していませんので、使う際は注意してください。
        /// </summary>
        /// <param name="_richTextBox1"></param>
        public static void showRichTextBox_EndLine_UnshowCursor(RichTextBox _richTextBox1, Form _usedForm)
        {
            string _text = _richTextBox1.Text.Substring(0, Math.Max(_richTextBox1.Text.Length, 0));

            // 以下、メモ
            // 最後に改行だけが入っている場合があるので、それを除いてスクロールする。・・いや、最後の改行はいれてもいいか
            //string _text = _richTextBox1.Text.Substring(0, Math.Max(_richTextBox1.Text.Length - 1, 0)); 
            //Point _lastCharPosition = _richTextBox1.GetPositionFromCharIndex(_lastCharIndex);
            //int _nowLine = _richTextBox1.GetLineFromCharIndex(_richTextBox1.SelectionStart); // SelectionStartはフォーカスを失うとすぐ0になるから使えない
            
            // 最後にスクロールバーが表示されて1秒たっていなければ、何もしない。
            // （短期間で呼び出した際に、連続的にフォーカス⇔非フォーカスする負荷を避けるため）
            int _passedMSec = getNowTime_fast() - p_showRichTextBox_EndLine_UnshowCursor__beforeScrollTime;

            if (_passedMSec < 1000)
            {
                //ConsoleWriteLine(getMethodName() + ": 連続呼び出し禁止中 （" + _passedMSec+"/1000ミリ秒以内）");
            }
            else if (_passedMSec > 1000 * 3)
            {
                //ConsoleWriteLine(getMethodName() + ": 過去のスクロール行数をリセット: リセット前 "+ p_showRichTextBox_EndLine_UnshowCursor__beforeLineNo +"（" + _passedMSec + "/3000ミリ秒以内）");
                // かなり時間がたっていたら時間が立ち過ぎていたら、違うスクロールとし
                // スクロール用に過去の計算した行数をリセット
                p_showRichTextBox_EndLine_UnshowCursor__beforeLineNo = 0;
            }
            
            if (_passedMSec > 1000)
            {
                // 1秒以上時間がたっていて、3秒以内に再度呼び出されていたら、同じ画面のスクロールとする

                // 現在のテキストボックスの行数を取得（この時点では"\n"の数は入っていない）
                _richTextBox1.SelectionStart = _text.Length;                //カレット位置を末尾に移動
                int _nowIndex = _richTextBox1.GetFirstCharIndexOfCurrentLine();
                int _nowLine = _richTextBox1.GetLineFromCharIndex(_nowIndex);
                // 実際の最後尾行と、SelectionStartで取得する改行行が合ってない。だからチラつく？
                // ■Textの中に入ってる、"\n"がGetFirstCharIndexOfCurrentLine()では取れてないから、スクロールがちらつく。
                int _LineCharNum = MyTools.getStringCountChar(_text, '\n');
                // "\n"の数だけ行数を足す
                _nowLine += _LineCharNum;

                // スクロール行が下方向でないとスクロールしないようにして、高速で呼び出した時の画面ののちらつきを抑える
                if (_nowLine > p_showRichTextBox_EndLine_UnshowCursor__beforeLineNo)
                {
                    //ConsoleWriteLine(getMethodName() + ": テキスト行数+" + _nowLine + " > 前回の行数" + p_showRichTextBox_EndLine_UnshowCursor__beforeLineNo + "より、スクロール最後尾に移動");
                    // スクロールした過去の時間を更新
                    p_showRichTextBox_EndLine_UnshowCursor__beforeScrollTime = getNowTime_fast();

                    // スクロール最大行数を保存
                    p_showRichTextBox_EndLine_UnshowCursor__beforeLineNo = _nowLine;
                    _richTextBox1.SelectionStart = _text.Length;                //カレット位置を末尾に移動
                    _richTextBox1.Focus();                                      //リッチテキストボックスにフォーカスを移動
                    _richTextBox1.ScrollToCaret();                              //カレット位置までスクロール
                    // このテキストボックスではない、フォームの他のコントロールにフォーカスを移す
                    // 以下、草案メモ
                    // [MEMO]注意：　フォームにフォーカスを映そうとしても、コントロールでは無いので効かない。（フォームの特定のコントロールにフォーカスを移せば効く）
                    // (a)フォームを使ったフォーカス移動
                    _usedForm.ActiveControl.SelectNextControl(_richTextBox1, true, true, false, true);
                    // TabStopを無視して移動したのに、移動先のコントロールのTabStopプロパティがfalseのものしかない場合，
                    // 仕方がないのでフォームにフォーカスを戻す
                    if (_usedForm.ActiveControl.TabStop == false)
                    {
                        _usedForm.Focus();
                        _usedForm.Select();
                    }
                    // (b)これの代わりに、フォームの特定のメインコントロール（TabStop=trueのもの）にフォーカスを移せば効く
                    // label1.Focus();
                    // (c)これはダメ。このコントロール内にある他のコントロールを呼び出している_richTextBox1.SelectNextControl(_richTextBox1, true, true, false, true);
                }
                else
                {
                    //ConsoleWriteLine(getMethodName() + ": テキスト行数 " + _nowLine + " <= 前回の行数 " + p_showRichTextBox_EndLine_UnshowCursor__beforeLineNo + "より、スクロールせずにスルー");
                }
            }


        }
        #endregion
        // ピクチャボックス関連
        #region ピクチャボックスにドラッグアンドドロップした画像を表示する方法: showPictureBoxImage_ByDragAndDrop_Part1/Part2
        /// <summary>
        /// ピクチャボックスにドラッグアンドドロップした画像を表示するのに必要な処理をまとめたものです。
        /// (1)Part0（フォームのコンストラクタなどでAllowDropプロパティのtrue）と、
        /// (2)Part1（DragEnterイベントが呼び出すメソッド）と、
        /// (3)Part2（DragDropイベントが呼び出すメソッド）
        /// の３つをちゃんとやって、初めて機能します。
        /// </summary>
        public static void showPictureBoxImage_ByDragAndDrop_Part1(object _sender_picturebox, DragEventArgs _e)
        {
            // ドラッグアンドドロップ可能なファイルであれば、_e.Dataにコピーする。
            if (_e.Data.GetDataPresent(DataFormats.FileDrop))
                _e.Effect = DragDropEffects.Copy;

            ///             // 【Part0】まず、コンストラクタやForm_Loadイベントなどで、
            ///             ピクチャボックスに画像をドラッグアンドドロップできるようにする
            ///    【注意】AllowDropプロパティはインテリセンスにはないので注意！ http://www.atmarkit.co.jp/bbs/phpBB//viewtopic.php?topic=33272&forum=7&4）
            ///    以下を設定する。
            ///         pictureBox1.AllowDrop = true
            ///         
            /// まとめ参考。感謝。http://pgchallenge.seesaa.net/article/65889017.html
        }
        /// <summary>
        /// ※Part2は、画像のフルパスを返します。
        /// 
        /// ピクチャボックスにドラッグアンドドロップした画像を表示するのに必要な処理をまとめたものです。
        /// (1)Part0（フォームのコンストラクタなどでAllowDropプロパティのtrue）と、
        /// (2)Part1（DragEnterイベントが呼び出すメソッド）と、
        /// (3)Part2（DragDropイベントが呼び出すメソッド）
        /// の３つをちゃんとやって、初めて機能します。
        /// 
        /// </summary>
        public static string showPictureBoxImage_ByDragAndDrop_Part2(object _sender_picturebox, DragEventArgs e, out Image _image, bool _isResize_trueIsShowAll_falseIsShowPartWithFullSize)
        {

            // ドラッグアンドドロップ可能なファイルをe.Data.GetDataで取得し、画像として表示する。
            string _filename = ((string[])e.Data.GetData
           (DataFormats.FileDrop))[0]; // 一つ目の画像
            _image = null;
            _image = new Bitmap(_filename);
            PictureBox _picturebox = (PictureBox)_sender_picturebox;
            if (_isResize_trueIsShowAll_falseIsShowPartWithFullSize == false)
            {
                // そのままのサイズを表示（一部しか表示されない可能性がある）
                _picturebox.Image = _image;
            }
            else
            {
                // リサイズして全部を表示
                _picturebox.Image = MyTools.getImage_Resized(_image, _picturebox.Width, _picturebox.Height);
            }
            return _filename;

            ///             // 【Part0】まず、コンストラクタやForm_Loadイベントなどで、
            ///             ピクチャボックスに画像をドラッグアンドドロップできるようにする
            ///    【注意】AllowDropプロパティはインテリセンスにはないので注意！ http://www.atmarkit.co.jp/bbs/phpBB//viewtopic.php?topic=33272&forum=7&4）
            ///    以下を設定する。
            ///         pictureBox1.AllowDrop = true
            ///         
            /// まとめ参考。感謝。http://pgchallenge.seesaa.net/article/65889017.html
        }
        #endregion

        #endregion

        // ■WindowsAPIの呼び出し
        #region 宣言しているWin32API（メソッドが使う可能性のあるWinAPIです。Windows非依存のメソッドと分けて、個々に書いてます。）
            #region ダブルクリック閾値 Get/SetDoubleClickTime
        /// <summary>
        /// ダブルクリック閾値（何秒以内に２回クリックしたらダブルクリックと判定するか）の設定値を取得するＡＰＩです。
        ///</summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        extern static int GetDoubleClickTime();
        /// <summary>
        /// ダブルクリック閾値（何秒以内に２回クリックしたらダブルクリックと判定するか）の設定値を取得します。ＡＰＩを使ってます。
        /// </summary>
        /// <returns></returns>
        public static int getDoubleClickTime() { return GetDoubleClickTime(); }
        /// <summary>
        /// ダブルクリックの閾値を設定するＡＰＩです。
        /// 
        ///         /// Note:        // ダブルクリックの設定時間を ms 単位で取得する (Windowsでは、150, 300,・・・1650ms の11段階)
        /// 
        /// ダブルクリックした時のマウスイベントの順序は、下記のとおり。
        ///         MouseDown → MouseClick       → MouseUp →
        ///         MouseDown → MouseDoubleClick → MouseUp .
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll")]
        extern static int SetDoubleClickTime(int uInterval_default500);
        /// <summary>
        /// ダブルクリック閾値（何秒以内に２回クリックしたらダブルクリックと判定するか）を設定します。ＡＰＩを使ってます。
        /// </summary>
        /// <param name="_doubleClickTime_NanMSecInaini"></param>
        public static void setDoubleClickTime(int _doubleClickTime_NanMSecInaini) { SetDoubleClickTime(_doubleClickTime_NanMSecInaini); }
            #endregion

        #region 画面全体をキャプチャする ( to クリップボード ) 。　他、宣言しているWIn32API一覧

        /// <summary>
        /// ウィンドウメッセージ（WM_***）を制御するAPIです。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32")]
		private static extern int SendMessage(IntPtr hWnd, uint Msg, long wParam, long lParam);
		public const uint WM_SETREDRAW = 0x000B;

        /// <summary>
        /// 他のウィンドウを探すAPIです．
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// あるウィンドウを強制的にアクティブにするAPIです．
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        extern static bool SetForegroundWindow(IntPtr hWnd);

        // (a)画面全体をキャプチャする
        /// <summary>
        /// ウィンドウハンドルやコントロールののスクリーンショットを取得して、hdcBltにイメージの保存先のハンドルを返します。
        /// </summary>
        /// <param name="hwnd">イメージを取得する対象ウィンドウ(HWND)</param>
        /// <param name="hdcBlt">イメージの保存先(HDC)</param>
        /// <param name="nFlags">オプションフラッグ：</param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern bool PrintWindow(
            IntPtr hwnd,	// 
            IntPtr hdcBlt,	// 
            uint nFlags		// 
            // 
            );

        // (b)画面全体をキャプチャする ( to Bitmap )
        /// <summary>
        /// スクリーンの一部または全体のデバイスコンテキストDCのハンドルを取得するAPIです． 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        /// <summary>
        /// スクリーンの一部または全体のデバイスコンテキストDCのハンドルを破棄するAPIです． 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hDc"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        /// <summary>
        /// スクリーンの一部または全体のデバイスコンテキストDCから取得した画像をビットマップに変換するAPIです．
        /// </summary>
        /// <param name="hdcDst"></param>
        /// <param name="xDst"></param>
        /// <param name="yDst"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hdcSrc"></param>
        /// <param name="xSrc"></param>
        /// <param name="ySrc"></param>
        /// <param name="rasterOp"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(
            // コピー先画像
            IntPtr hdcDst,
            int xDst,
            int yDst,
            int width,
            int height,
            // コピー元画像
            IntPtr hdcSrc,
            int xSrc,
            int ySrc,
            // 動作(SRCCOPYなど)
            int rasterOperation
            );
        // 動作（コピー）
        public const int SRCCOPY = 0xcc0020; //13369376;

        // 指定座標をキャプチャする（ to Bitmap）
        /// <summary>
        /// 現在ユーザーが作業しているウィンドのハンドルを返します。
        /// </summary>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern IntPtr GetForegroundWindow();
        /// <summary>
        /// 指定したウィンドウハンドルのデバイスコンテキストを返します。GetDC関数と違い、ウィンドウの非クライアント領域も取得可能です。
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        /// <summary>
        /// 指定されたウィンドウ左上端と右下端の座標をスクリーン座標で取得します。第一引数には、座標を取得したいウィンドウのハンドルを指定します。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        /// <summary>
        /// Win32API互換のための，四角形構造体です．
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int height; // right
            public int width; // bottom
        }
        #endregion
        #endregion


    }
}
