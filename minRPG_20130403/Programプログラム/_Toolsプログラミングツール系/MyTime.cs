// デバッグ中はコメントアウト
//#define Debug

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading; //Stopwatchクラス

namespace PublicDomain
{
    /// <summary>
    ///  年月日付を含めた時刻や時間（特に，NumberOnly型と呼ぶ時刻（long型）　⇔　時刻を表すDateTime型　⇔　秒・ミリ秒などの時間間隔（int型）　の相互変換）に関する処理をする雛形を持つクラスです．
    /// </summary>
    public class MyTime
    {

        protected DateTime nowDateTime; // 今の時間をDateTime型で格納 // timeから変換して一時的に生成
        protected DateTime startTime; // コンストラクタ起動時の時間

        /// <summary>
        /// 保存した時間リスト
        /// </summary>
        protected Dictionary<int, long> savedTimeName = new Dictionary<int, long>(SAVEDTIMENAMENUM_MAX);
        public const int SAVEDTIMENAMENUM_MAX = 1000;

        protected DateTime beforeCheckedTime; // 以前checkTimeを押された時からの経過時間
        protected int checkNum1 = 0; // checkTimeでチェックした回数1
        protected Stopwatch stopwatch1 = new Stopwatch(); // 時間を計るためのウトップウォッチ1

        protected int checkNum2 = 0; // checkTimeでチェックした回数
        protected int checkNum2_instance = 0; // checkTimeの指定秒間あたりのチェックした回数
        protected Stopwatch stopwatch2 = new Stopwatch(); // 時間を計るためのウトップウォッチ2

        /// <summary>
        /// コンストラクタ（アプリケーション起動時にMyTimeインスタンスを生成すると，経過後の時間を計測します．）
        /// </summary>
        public MyTime()
        {
            // 開始時間
            startTime = DateTime.Now;
            // チェックタイムの初期値は開始時間
            beforeCheckedTime = DateTime.Now;
            checkTime();
        }

        #region MyTimeインスタンスが生成されてからの経過時間・時刻の取得: getStartTime_**
        /// <summary>
        /// MyTimeインスタンスが生成されてからの経過時間（始めにMyTimeインスタンスを生成していればアプリケーション起動時間）を計算します．
        /// </summary>
        /// <returns></returns>
        public TimeSpan getTimeSpan_FromStartTime()
        {
            TimeSpan ts = new TimeSpan();
            string timeSpan = "";
            if (this.startTime != null)
            {
                ts = DateTime.Now.Subtract(this.startTime);
                // デバッグ用
                timeSpan =
                "経過時間 = " + ts.Days + "日" +
                                ts.Hours + "時間" +
                                ts.Minutes + "分" +
                                ts.Seconds + "秒";
            }
            return ts;
        }

        /// <summary>
        /// MyTimeインスタンスが生成された時刻からの経過時間（ミリ秒）を返します．
        /// </summary>
        /// <returns></returns>
        public long getPassedMSec_FromStartTime()
        {
            DateTime nowTime = DateTime.Now;
            TimeSpan ts = nowTime - startTime;
            return (long)(ts.TotalMilliseconds);
        }

        /// <summary>
        /// MyTimeインスタンスが生成された時刻からの経過時間（秒）を返します．
        /// </summary>
        /// <returns></returns>
        public long getPassedSec_FromStartTime()
        {
            DateTime nowTime = DateTime.Now;
            TimeSpan ts = nowTime - startTime;
            return (long)(ts.TotalSeconds);
        }

        /// <summary>
        /// MyTimeインスタンスが生成された時刻を，「yyyyMMddHHmmss」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public long getStartTime_NumberOnly()
        {
            string nowTime = startTime.ToString("yyyyMMddHHmmss");  // 現在の時刻（秒まで）
            return long.Parse(nowTime);
        }

        /// <summary>
        /// MyTimeインスタンスが生成された時刻を，「yyyyMMddHHmmssaaa(aはミリ秒)」のlong型フォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public long getStartTimeAndMSec_NumberOnly()
        {
            string nowTime = startTime.ToString("yyyyMMddHHmmss");  // 現在の時刻（秒まで）
            string msec = startTime.Millisecond.ToString(); // Millisecond.ToString();
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
        #endregion

        /// ここからは，staticメソッドです．

        /// <summary>
        /// コンソールに文字列を表示します。他のメソッドもこれを呼び出します。
        /// </summary>
        /// <param name="_message"></param>
        public static void showMessage_ConsoleLine(string _message)
        {
            Console.WriteLine(_message);
        }

        #region 一定時間停止（入力操作不可能／操作可能）: wait_***


        /// <summary>
        /// 一定時間、操作不可能な状態で待った後、trueを返します。厳密に指定時間停止します。（System.Threading.Thead.Sleepを使ってるので，他のスレッドは動きますが，このスレッドのWindowsメッセージは処理できません．）
        /// </summary>
        /// <param name="_waitMSec"></param>
        /// <returns></returns>
        public static bool wait_Stoped(int _waitMSec)
        {
            Thread.Sleep(_waitMSec);
            return true;
        }

        /// <summary>
        /// 一定時間、操作可能な状態で待った後、trueを返します。他のメッセージが終了するまでは遅延が発生することがあります。（DoEventsを使っているので，他のWindowsメッセージを処理できます）
        /// </summary>
        /// <param name="_waitMsec"></param>
        /// <returns></returns>
        public static bool wait_Movable(int _waitMSec)
        {
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Start();
            while (_stopwatch.ElapsedMilliseconds < _waitMSec)
            {
                // 他のWindowsメッセージの処理を行う
                MyTools.doEvents_WaitForOtherEvents();

            }
            return true;
        }

        /// <summary>
        /// 一定時間、コントロールに残り時間を表示しながら、操作可能な状態で待った後、trueを返します。他のメッセージが終了するまで遅延が発生することがあります。（DoEventsで他のWindowsメッセージを処理できます）
        /// </summary>
        /// <param name="_waitMSec"></param>
        /// <param name="shownControl"></param>
        /// <returns></returns>
        public static bool wait_Movable_ShowingRestTime(int _waitMSec, Control shownControl)
        {
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Start();
            while (_stopwatch.ElapsedMilliseconds < _waitMSec)
            {
                shownControl.Text = "制限時間: " + (double)((int)(((double)_waitMSec - (double)_stopwatch.ElapsedMilliseconds) / 100.0)) * 100.0 / 1000.0 + " 秒";
                // 他のWindowsメッセージの処理を行う
                MyTools.doEvents_WaitForOtherEvents();

            }
            return true;
        }
        #endregion

        /// <summary>
        /// 現在の時間（コンピュータ起動後の経過ミリ秒）を高速に取得します。
        /// </summary>
        /// <returns></returns>
        public static int getTime_fast()
        {
            return Environment.TickCount; // DateTime.Now.Ticks;やStopwatchよりこれが一番処理速度が速いらしい（精度はstopwatchの方がいい）。参考:http://d.hatena.ne.jp/saiya_moebius/20100819/1282201466#20100819f1
        }

        #region 現在の時刻を取得：　getNowTime***

        /// <summary>
        /// 0001 年 1 月 1 日午前 12:00 から経過したミリ秒を返します（Java仕様？）。
        /// </summary>
        /// <returns></returns>
        public static long getNowMSec()
        {

            return DateTime.Now.Ticks / (Stopwatch.Frequency / 1000);
            
            
            // ここの情報＋以下の情報 http://dobon.net/vb/dotnet/system/stopwatch.html

            // 現在の0001 年 1 月 1 日午前 12:00 から経過した 100 ナノ秒間隔の数を取得
            //long tick = DateTime.Now.Ticks;
            //DateTime d = new DateTime(tick);
            //Console.WriteLine(d.ToString());
            // 処理結果 2004/07/06 00:02:38

        }
        /// <summary>
        /// 現在の時刻を「yyyyMMddHHmmss」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public static long getNowTimeAndSec_NumberOnly()
        {
            string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");  // 現在の時刻を表示
            return long.Parse(nowTime);
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
        /// コンピュータ起動後の経過時間を計算し，「dd日HH時mm分ss秒」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public static string getTimeSpanNowFromComputerRestart()
        {

            int tc = Environment.TickCount;              // ミリ秒単位で経過時間を取得

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, tc);  // 値を TimeSpan 型へ変換する

            string timeSpan =
                "経過秒数 = " + (tc / 1000) + "秒\r\n" +
                "経過日時 = " + ts.Days + "日" +
                                ts.Hours + "時間" +
                                ts.Minutes + "分" +
                                ts.Seconds + "秒";
            return timeSpan;
        }

        /// <summary>
        /// 現在の時刻を「yyyy年MM月dd日 HH:mm:ss」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public static string getNowTime_Simple()
        {
            string nowTime = DateTime.Now.ToString("F");  // 現在の時刻を表示

            return nowTime;

            // Note: Form のスタイルについては、Form を画像の形にする を参照。

            // Note: この処理を System.Windows.Forms.Timer のイベントにさせた場合，
            //       間隔は、システムに対して
            //       重い負荷がかかった場合、設定時間より(時には１割以上)長くなる！
            // Note: タイマの停止は、Stop() を使う。

        }

        /// <summary>
        /// 現在の時刻を「yyyyMMdd_HHmmss」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public static string getNowTime_Number()
        {
            string nowTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");  // 現在の時刻を表示
            return nowTime;
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

        // 未実装
        /*public static double getTimeSpan_FromAtoB(string pastTime, string nowTime)
        {
            double timeSpan = ;
            return timeSpan;
        }*/

        #region DateTime型⇔long型の変換： getTime/DateTime
        /// <summary>
        /// 引数のDateTimeのをミリ秒を含めて「yyyyMMddHHmmss」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public static long getTimeAndSec_NumberOnly_ByDateTime(DateTime dt)
        {
            string time = dt.ToString("yyyyMMddHHmmss");  // 現在の時刻（秒まで）
            return long.Parse(time);
        }

        /// <summary>
        /// 現在の時刻をミリ秒を含めて「yyyyMMddHHmmssaaa（aaaはミリ秒）」のフォーマットで返します．
        /// </summary>
        /// <returns></returns>
        public static long getTimeAndMSec_NumberOnly_ByDateTIme(DateTime dt)
        {
            string time = dt.ToString("yyyyMMddHHmmss");  // 現在の時刻（秒まで）
            string msec = dt.Millisecond.ToString(); // Millisecond.ToString();
            // 3桁以下なら「0」を足す
            if (msec.Length != 3)
            {
                for (int i = 1; msec.Length < 3; i++)
                {
                    msec = "0" + msec;
                }
            }
            time += msec;
            return long.Parse(time);
        }

        /// <summary>
        /// 引数の「yyyyMMddHHmmss」もしくは「yyyyMMddHHmmssaaa」の数値から，DateTime型に変換して返します．
        /// </summary>
        /// <param name="time_NumberOnly"></param>
        /// <returns></returns>
        public static DateTime getDateTime_ByNumberOnly(long time_NumberOnly)
        {
            string time = time_NumberOnly.ToString();

            DateTime dt;
            int years = int.Parse(time.Substring(0, 4));
            int months = int.Parse(time.Substring(4, 2));
            int days = int.Parse(time.Substring(6, 2));
            int hours = int.Parse(time.Substring(8, 2));
            int minutes = int.Parse(time.Substring(10, 2));
            int seconds = int.Parse(time.Substring(12, 2));
            // ミリ秒を管理しているのは15～17桁
            if (time_NumberOnly.ToString().Length > 14)
            {
                int milliseconds = int.Parse(time.Substring(14, 3));
                dt = new DateTime(years, months, days, hours, minutes, seconds, milliseconds);
            }
            else
            {
                dt = new DateTime(years, months, days, hours, minutes, seconds);
            }

            return dt;

        }
        #endregion

        #region NumberOnly(long)型の時刻の加算・減算：addTime/reduceTime_NumberOnly
        /// <summary>
        /// 第１引数のNumberOnly型（「yyyyMMddHHmmss」もしくは「yyyyMMddHHmmssaaa」）の時刻に，第２引数の秒間だけ経過したNumberOnly型の時刻を返します．
        /// </summary>
        /// <param name="_baseTime_NumberOnly"></param>
        /// <param name="_addSec"></param>
        /// <returns></returns>
        public static long addTime_NumberOnly(long _baseTime_NumberOnly, double _addSec)
        {
            DateTime _dt = getDateTime_ByNumberOnly(_baseTime_NumberOnly);
            _dt.AddSeconds(_addSec);
            long _addedTime = getTimeAndMSec_NumberOnly_ByDateTIme(_dt);
            return _addedTime;
        }
        /// <summary>
        /// 第１引数のNumberOnly型（「yyyyMMddHHmmss」もしくは「yyyyMMddHHmmssaaa」）の時刻に，第２引数の秒間だけ巻き戻したNumberOnly型の時刻を返します．
        /// </summary>
        /// <param name="_baseTime_NumberOnly"></param>
        /// <param name="_addSec"></param>
        /// <returns></returns>
        public static long reduceTime_NumberOnly(long _baseTime_NumberOnly, double _reducedSec)
        {
            DateTime _dt = getDateTime_ByNumberOnly(_baseTime_NumberOnly);
            _dt.AddSeconds((-1)*_reducedSec);
            long _reducedTime = getTimeAndMSec_NumberOnly_ByDateTIme(_dt);
            return _reducedTime;
        }

        /// <summary>
        /// 第１引数のNumberOnly型（「yyyyMMddHHmmss」もしくは「yyyyMMddHHmmssaaa」）の時刻に，第２引数のTimeSpan時間だけ経過したNumberOnly型の時刻を返します．
        /// </summary>
        /// <param name="_baseTime_NumberOnly"></param>
        /// <param name="_addSec"></param>
        /// <returns></returns>
        public static long addTime_NumberOnly(long _baseTime_NumberOnly, TimeSpan _addTimeSpan)
        {
            DateTime _dt = getDateTime_ByNumberOnly(_baseTime_NumberOnly);
            _dt.Add(_addTimeSpan);
            long _addedTime = getTimeAndMSec_NumberOnly_ByDateTIme(_dt);
            return _addedTime;
        }
        /// <summary>
        /// 第１引数のNumberOnly型（「yyyyMMddHHmmss」もしくは「yyyyMMddHHmmssaaa」）の時刻に，第２引数のTimeSpan時間を巻き戻したNumberOnly型の時刻を返します．
        /// </summary>
        /// <param name="_baseTime_NumberOnly"></param>
        /// <param name="_addSec"></param>
        /// <returns></returns>
        public static long reduceTime_NumberOnly(long _baseTime_NumberOnly, TimeSpan _reduceTimeSpan)
        {
            DateTime _dt = getDateTime_ByNumberOnly(_baseTime_NumberOnly);
            _dt.Subtract(_reduceTimeSpan);
            long _reducedTime = getTimeAndMSec_NumberOnly_ByDateTIme(_dt);
            return _reducedTime;
        }
        #endregion

        #region 経過時間の計算: getDifference***
        /// <summary>
        /// 第1引数の時刻から第2引数の時刻までの経過時間を格納するTimeSpan型をを返します．なお，引数の時刻は引数の「yyyyMMddHHmmss」もしくは「yyyyMMddHHmmssaaa」の数値です．
        /// </summary>
        /// <returns></returns>
        public static TimeSpan getDifferenceTimeSpan_ByNumberOnly(long beginTime, long endTime)
        {

            DateTime dt_b = getDateTime_ByNumberOnly(beginTime);
            DateTime dt_e = getDateTime_ByNumberOnly(endTime);
            /*if (dt_e == null || dt_b == null)
            {
                // エラーは、0秒を返す？
                return new TimeSpan(); // TimeSpan(0);
            }
             * */
            TimeSpan ts = dt_e - dt_b;
            return ts;

        }

        /// <summary>
        /// 第1引数の時刻から，第2引数までに経過したミリ秒間を返します．なお，引数の時刻は引数の「yyyyMMddHHmmss」もしくは「yyyyMMddHHmmssaaa」の数値です．
        /// </summary>
        /// <returns></returns>
        public static long getDifferenceMSec_ByNumberOnly(long beginTime, long endTime)
        {
            int error = -1;
            if (beginTime.ToString().Length < 14 || endTime.ToString().Length < 14)
            {
                // [エラー]数が足りない
#if Debug
                showMessage_ConsoleLine("時刻の桁数が14桁より少ないです。");
#endif
                return error;
            }

            TimeSpan ts = getDifferenceTimeSpan_ByNumberOnly(beginTime, endTime);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 第1引数の時刻から，第2引数までに経過した秒間を返します．なお，引数の時刻は引数の「yyyyMMddHHmmss」もしくは「yyyyMMddHHmmssaaa」の数値です．
        /// </summary>
        /// <returns></returns>
        public static long getDifferenceSec_ByNumberOnly(long beginTime, long endTime)
        {
            int error = -1;
            if (beginTime.ToString().Length < 14 || endTime.ToString().Length < 14)
            {
                // [エラー]数が足りない
                showMessage_ConsoleLine("時刻の桁数が14桁より少ないです。");
                return error;
            }


            TimeSpan ts = getDifferenceTimeSpan_ByNumberOnly(beginTime, endTime);
            return (long)ts.TotalSeconds;
        }

        /*
        // [未完成][時刻計算]TimeSpanを使わずに計算しようとしたが，難しかったので中断
        /// <summary>
        /// 第1引数の時刻から，第2引数までに経過した秒間を，long型14桁"yyyymmddhhmmss"の値のみ（0は省略）で返します．エラーの場合，-1を返します．
        /// </summary>
        /// <returns></returns>
        public static long getDifference_WithSec_ByNumberOnly(long beginTime, long endTime)
        {
            int error = -1;
            if (beginTime.ToString().Length < 14 || endTime.ToString().Length < 14)
            {
                // [エラー]数が足りない
                return error;
            }

            int _index; // iはbeginとendで，前から数えて値が異なる桁数（例：　2000***と2001***なら，_index=5）
            for (_index = 1; _index <= 14; _index++)
            {
                if (beginTime.ToString().Substring(_index-1,1).Equals(endTime.ToString().Substring(_index-1, 1)) == false)
                {
                    break;
                }
            }
            // それぞれの差分を計算
            // [未実装]逆じゃないと繰り下がりが計算できない！　2単位以上（秒→時間）の繰り下がりも考慮すべき！
            int year_d = 0;
            if (_index <= 4)
            {
                int year_b = int.Parse(beginTime.ToString().Substring(0, 4));
                int year_e = int.Parse(endTime.ToString().Substring(0, 4));
                year_d = year_b - year_e;
            }
            int month_d = 0;
            if (_index <= 6)
            {
                int month_b = int.Parse(beginTime.ToString().Substring(4, 2));
                int month_e = int.Parse(endTime.ToString().Substring(4, 2));
                month_d = month_b - month_e;
            }
            int day_d = 0;
            if (_index <= 8)
            {
                int day_b = int.Parse(beginTime.ToString().Substring(6, 2));
                int day_e = int.Parse(endTime.ToString().Substring(6, 2));
                day_d = day_b - day_e;
            }
            int hour_d = 0;
            if (_index <= 10)
            {
                int hour_b = int.Parse(beginTime.ToString().Substring(8, 2));
                int hour_e = int.Parse(endTime.ToString().Substring(8, 2));
                hour_d = hour_b - hour_e;
            }
            int minute_d = 0;
            if (_index <= 12)
            {
                int minute_b = int.Parse(beginTime.ToString().Substring(10, 2));
                int minute_e = int.Parse(endTime.ToString().Substring(10, 2));
                minute_d = minute_b - minute_e;
            }
            int sec_d = 0;
            if (_index <= 14)
            {
                int sec_b = int.Parse(beginTime.ToString().Substring(12, 2));
                int sec_e = int.Parse(endTime.ToString().Substring(12, 2));
                sec_d = sec_b - sec_e;
            }
            
            long distance = year_d*10000000000 + month_d*100000000 + day_d*1000000 + hour_d*10000 + minute_d*100 + + sec_d;
            return distance;

        }

        /// <summary>
        /// 第1引数の時刻から，第2引数までに経過したミリ秒間を，long型17桁"yyyymmddhhmmssaaa"の値のみ（0は省略）で返します．
        /// </summary>
        /// <returns></returns>
        public static long getDifference_WithMSec_ByNumberOnly(long beginTime, long endTime)
        {
            int error = -1;
            if (beginTime.ToString().Length < 17 || endTime.ToString().Length < 17)
            {
                // [エラー]数が足りない
                return error;
            }

            int _index; // iはbeginとendで，前から数えて値が異なる桁数（例：　2000***と2001***なら，_index=5）
            for (_index = 15; _index <= 17; _index++)
            {
                if (beginTime.ToString().Substring(_index - 1, 1).Equals(endTime.ToString().Substring(_index - 1, 1)) == false)
                {
                    break;
                }
            }
            int msec_b = int.Parse(beginTime.ToString().Substring(14, 3));
            int msec_d = 0;

            // ミリ秒の差分を計算
            if (_index <= 17)
            {
                int msec_b = int.Parse(beginTime.ToString().Substring(14, 3));
                int msec_e = int.Parse(endTime.ToString().Substring(14, 3));
                if (msec_b >= msec_e)
                {
                    msec_d = msec_b - msec_e;
                }
                else
                {
                    msec_d = 1000 + msec_b - msec_e;
                    if (sec_b != 0)
                    {
                        sec_b--;
                    }
                    else
                    {
                        sec_b = 59;
                        minute_be--;
                    }
                }
            }

            long TimeuntilSec_d = getDifference_WithSec_ByNumberOnly(beginTime, endTime);
            
            

            long distance = (long)1000*(year_d * 10000000000 + month_d * 100000000 + day_d * 1000000 + hour_d * 10000 + minute_d * 100 + +sec_d)+msec_d;
            return distance;

        }
        */
        #endregion

        #region 時刻のある部分だけを取得：getSecOnlyなど
        /// <summary>
        /// 引数の時刻の秒の部分だけを返します．
        /// </summary>
        /// <param name="time_NumberOnly"></param>
        /// <returns></returns>
        public static int getSecOnly_NumberOnly(long _time_NumberOnly)
        {
            // "yyyyMMddHHmmss"型のnowData
            string time_NumberOnly = _time_NumberOnly.ToString();
            int sec = int.Parse(time_NumberOnly.Substring(12, 2));
            Console.WriteLine(sec + "秒");
            return sec;
        }
        #endregion

        #region 時刻の単位の変換：transToMinutesなど
        /// <summary>
        /// 引数の秒間を分間に変換して，小数点第1位までの値を返す
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static double transToMinutes_BySec(long sec)
        {
            return transToMinutes_ByMSec(sec * 1000);
        }
        /// <summary>
        /// 引数のミリ秒間を分間に変換して，小数点第1位までの値を返す
        /// </summary>
        /// <param name="msec"></param>
        /// <returns></returns>
        public static double transToMinutes_ByMSec(long msec)
        {
            // string nowTime = ;
            TimeSpan ts = TimeSpan.FromSeconds(msec);
            return ts.TotalSeconds;
        }
        #endregion

        #region 出力用の変換：getShownTime/get***_ToString
        /// <summary>
        /// 引数の秒間を分間に変換して，第二引数に指定した小数点第n位までの値を右詰め文字列として返す
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static string getMinutes_BySec_ToString(long sec, int shownDouble_n)
        {
            return getMinutes_ByMSec_ToString(sec * 1000, shownDouble_n);

        }

        /// <summary>
        /// 引数の秒間を分間に変換して，第二引数に指定した小数点第n位までの値を右詰め文字列として返す
        /// </summary>
        /// <param name="sec"></param>
        /// <returns></returns>
        public static string getMinutes_ByMSec_ToString(long msec, int shownDouble_n)
        {
            return getMinutes_ByMSec_ToString(msec, shownDouble_n);

        }

        /// <summary>
        /// 引数の小数点で表された時間単位を，第二引数に指定したnケタまでの値を、nケタのフィールド上に右詰め／左詰め文字列として返します。nケタを超えた場合、「OVER」と表示されます。
        /// </summary>
        /// <param name="msec"></param>
        /// <returns></returns>
        public static string getShownTime(long showntime_MINUATESorSECorMSEC, int shownMaxLength_n, bool _isRightSide)
        {
            // 第一引数の整数n桁までを右詰にして表示    
            string time = MyTools.getShownNumber(showntime_MINUATESorSECorMSEC, shownMaxLength_n, shownMaxLength_n, _isRightSide, true);
            return time;

        }
        #endregion

        #region 前にこのメソッドを呼び出してからの経過時間の秒数を計算する: checkTime
        /// <summary>
        /// 前にこのメソッドを呼び出してからの経過時間の秒数を返します．
        /// </summary>
        /// <returns>呼び出し間の経過秒数</returns>
        public double checkTime()
        {
            if (stopwatch2.IsRunning == false)
            {
                stopwatch1.Start();
                return 0.0;
            }
            else
            {
                checkNum1++;
                stopwatch1.Stop();
                double sec = (double)(stopwatch1.ElapsedMilliseconds) / 1000.0;
                Console.WriteLine(checkNum1 + ":" + sec + "秒経過"); // 出力例：0.998466888760429
                stopwatch1.Reset();
                stopwatch1.Start();
                return sec;
            }
        }
        /// <summary>
        /// このメソッドを呼び出してからの経過時間が引数の秒数以上であればタイマーを止め，trueを返し，再びタイマーを稼動します．
        /// </summary>
        /// <returns>呼び出し間の経過秒数</returns>
        public bool isOver_OnCheckTime(int stopedMSec)
        {
            if (stopwatch2.IsRunning == false)
            {
                stopwatch2.Start();
                return false;
            }
            else
            {
                checkNum2++;
                checkNum2_instance++;
                int msec = (int)stopwatch2.ElapsedMilliseconds;
                if (msec > stopedMSec)
                {
#if Debug
                    Console.WriteLine(checkNum2 + ": " + checkNum2_instance + "回チェック " + msec + "秒経過"); // 出力例：0.998466888760429
#endif
                    stopwatch2.Reset();
                    checkNum2_instance = 0;
                    stopwatch2.Start();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion


        #region 時刻の記録：　saveNowTime/getSavedTime
        /// <summary>
        /// 現在の時刻を、後で参照できる時間ＩＤをキーにしてメモリに格納します。同じキーの時は上書きします。
        /// </summary>
        /// <param name="timeName_ID">この時間を参照するときに使う時間ＩＤ</param>
        public void saveNowTime(int timeName_ID)
        {
            savedTimeName.Remove(timeName_ID);
            savedTimeName.Add(timeName_ID, getNowTimeAndMSec_NumberOnly());
        }
        /// <summary>
        /// あらかじめ保存された時間ＩＤの時刻を返します。
        /// </summary>
        /// <param name="timeName_ID"></param>
        /// <returns></returns>
        public long getSavedTime(int timeName_ID)
        {
            if (savedTimeName.ContainsKey(timeName_ID) == true)
            {
                return savedTimeName[timeName_ID];
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// あらかじめ保存された時間ＩＤの時刻を返します。
        /// </summary>
        /// <param name="timeName_ID"></param>
        /// <returns></returns>
        public int getSavedTime_ToInt(int timeName_ID)
        {
            return (int)savedTimeName[timeName_ID];
        }


        /// <summary>
        /// あらかじめ保存された時間ＩＤからの経過時間（ミリ秒）を返します。
        /// </summary>
        /// <param name="timeName_ID"></param>
        /// <returns></returns>
        public long getPassedMSec_FromSavedTime(int timeName_ID)
        {
            try
            {
                long beginTime = savedTimeName[timeName_ID];
                long endTime = getNowTimeAndMSec_NumberOnly();
                return getDifferenceMSec_ByNumberOnly(beginTime, endTime);
            }
            catch (Exception e)
            {
#if Debug
                showMessage_ConsoleLine("MyTime::getPassedMSec()のエラー：　まだ作成されていない、時間セーブIDが呼ばれました。");
#endif
                return -1;
            }

        }
        public int getPassedMSec_FromSavedTime_ToInt(int timeName_ID)
        {
            return (int)getPassedMSec_FromSavedTime(timeName_ID);
        }
        /// <summary>
        /// あらかじめ保存された時間ＩＤからの経過時間（秒）を返します。
        /// </summary>
        /// <param name="timeName_ID"></param>
        /// <returns></returns>
        public long getPassedSec_FromSavedTime(int timeName_ID)
        {
            return getPassedMSec_FromSavedTime(timeName_ID) / 1000;
        }
        public int getPassedSec_FromSavedTime_ToInt(int timeName_ID)
        {
            return (int)getPassedMSec_FromSavedTime(timeName_ID) / 1000;
        }
        #endregion

        #region 時間平均値の計算
        //public void calcAvarageTime_ByList<int, long, double>(List<int, long, double> _timeList, int dividedNum)
        /// <summary>
        /// 引数の値を格納したリストの平均値を算出して返します
        /// </summary>
        /// <param name="_timeList"></param>
        /// <param name="dividedNum"></param>
        /// <returns></returns>
        public double calcAvarageTime_ByList(List<int> _timeList)
        {
            int sum = 0;
            for (int i = 0; i < _timeList.Count; i++)
            {
                sum += _timeList[i];
            }
            double average = (double)sum / _timeList.Count;
            return average;
        }

        /// <summary>
        /// あらかじめ保存された、ある開始用の時間ＩＤからある終了用の時間ＩＤまでの経過時間（ミリ秒）をnで割った，平均値を返します。
        /// </summary>
        /// <param name="begin_timeName_ID"></param>
        /// <param name="end_timeName_ID"></param>
        /// <param name="dividedNum"></param>
        /// <returns></returns>
        public long calcAvarageMSec_FromSavedTime(int begin_timeName_ID, int end_timeName_ID, int dividedNum)
        {
            int error = -1;
            if (dividedNum == 0)
            {
                return error;
            }
            double differenceMSec = (double)getDifferenceMSec_ByNumberOnly(savedTimeName[begin_timeName_ID], savedTimeName[end_timeName_ID]);
            return (long)(differenceMSec / (double)dividedNum);
        }
        public int calcAvarageMSec_FromSavedTime_ToInt(int begin_timeName_ID, int end_timeName_ID, int dividedNum)
        {
            return (int)calcAvarageMSec_FromSavedTime(begin_timeName_ID, end_timeName_ID, dividedNum);
        }
        /// <summary>
        /// あらかじめ保存された、ある開始用の時間ＩＤからある終了用の時間ＩＤまでの経過時間（秒）をnで割った，平均値を返します。
        /// </summary>
        /// <param name="begin_timeName_ID"></param>
        /// <param name="end_timeName_ID"></param>
        /// <param name="dividedNum"></param>
        /// <returns></returns>
        public long calcAvarageSec_FromSavedTime(int begin_timeName_ID, int end_timeName_ID, int dividedNum)
        {
            return calcAvarageMSec_FromSavedTime(begin_timeName_ID, end_timeName_ID, dividedNum) / 1000;
        }
        public int calcAvarageSec_FromSavedTime_ToInt(int begin_timeName_ID, int end_timeName_ID, int dividedNum)
        {
            return (int)calcAvarageSec_FromSavedTime(begin_timeName_ID, end_timeName_ID, dividedNum);
        }
        #endregion

        #region ■Windowsアプリ用。FormクラスやControlクラスがインポートされてないと使えません。


        ///// <summary>
        ///// 一定時間、操作可能な状態で待った後、trueを返します。他のメッセージが終了するまでは遅延が発生することがあります。（DoEventsで他のWindowsメッセージを処理できます）
        ///// ※現在、WindowsFormアプリのものはできますが、Webアプリなどのものは出来ていません。
        ///// </summary>
        ///// <param name="_waitMsec"></param>
        ///// <returns></returns>
        //public static bool wait_Movable(int _waitMSec)
        //{
        //    Stopwatch _stopwatch = new Stopwatch();
        //    _stopwatch.Start();
        //    while (_stopwatch.ElapsedMilliseconds < _waitMSec)
        //    {
        //        // 他のWindowsメッセージの処理を行う
        //        doEvents_WaitForOtherEvents();
        //    }
        //    return true;
        //}
        //public static void doEvents_WaitForOtherEvents()
        //{
        //    // [Tips]複数フォーム・孫にフォーム？継承のフォームで新しいフォームを表示するときのエラーで，DoEventsの前にEnableVisualStylesを入れると解消できた
        //    Application.EnableVisualStyles();
        //    Application.DoEvents();
        //}

        ///// <summary>
        ///// 一定時間、コントロールに残り時間を表示しながら、操作可能な状態で待った後、trueを返します。他のメッセージが終了するまで遅延が発生することがあります。（DoEventsで他のWindowsメッセージを処理できます）
        ///// </summary>
        ///// <param name="_waitMSec"></param>
        ///// <param name="shownControl"></param>
        ///// <returns></returns>
        //public static bool wait_Movable_ShowingRestTime(int _waitMSec, Control shownControl)
        //{
        //    Stopwatch _stopwatch = new Stopwatch();
        //    _stopwatch.Start();
        //    while (_stopwatch.ElapsedMilliseconds < _waitMSec)
        //    {
        //        shownControl.Text = "制限時間: " + (double)((int)(((double)_waitMSec - (double)_stopwatch.ElapsedMilliseconds) / 100.0)) * 100.0 / 1000.0 + " 秒";
        //        // 他のWindowsメッセージの処理を行う
        //        MyTools.doEvents_WaitForOtherEvents();
        //    }
        //    return true;
        //}
        #endregion


        #region テスト用
        private static void test_getActualMsecOn1sec()
        {
            // ストップウォッチの使い方の勉強
            Stopwatch sw = new Stopwatch();

            sw.Start();
            System.Threading.Thread.Sleep(1000);
            sw.Stop();

            // ミリ秒単位で出力
            long millisec = sw.ElapsedMilliseconds;
            Console.WriteLine(millisec); // 出力例：998

            // TimeSpan構造体で書式付き表示
            TimeSpan ts = sw.Elapsed;
            Console.WriteLine(ts); // 出力例：00:00:00.9984668

            // 高分解能なタイマが利用可能か
            Console.WriteLine(Stopwatch.IsHighResolution); // 出力例：True

            // タイマ刻み回数
            long ticks = sw.ElapsedTicks;
            Console.WriteLine(ticks); // 出力例：2988141812

            // タイマの1秒あたりの刻み回数
            Console.WriteLine(Stopwatch.Frequency); // 出力例：2992730000

            // より詳細な秒数
            double sec = (double)sw.ElapsedTicks
                          / (double)Stopwatch.Frequency;
            Console.WriteLine(sec); // 出力例：0.998466888760429
        }
        #endregion
    }

}
