using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace PublicDomain
{

    /// <summary>
    /// CLogDataのログを表示する表示タイプの種類です．表示する場所を指定することもできます。
    /// </summary>
    public enum ELogType
    {
        /// <summary>
        ///=0。デフォルトは、コンソールに出力します。
        /// </summary>
        l0_標準出力,
        /// <summary>
        /// メインメッセージにも表示します。
        /// </summary>
        l1_メインメッセージ,
        l2_入力操作,
        l3_イベント,
        /// <summary>
        /// 特に重要なデバッグの時に指定します。コンソールに出力します。
        /// </summary>
        l4_重要なデバッグ,
        /// <summary>
        /// メッセージダイアログを表示します。
        /// </summary>
        l5_エラーダイアログ表示,

        /// <summary>
        /// GUIのTextBox1に表示します。わかりやすいように、名前を変えてもOKです。（戦闘用、スイッチ変数用、感情用、など）
        /// </summary>
        lgui1_ログGUIText戦闘用,
        /// <summary>
        /// GUIのTextBox2に表示します。わかりやすいように、名前を変えてもOKです。（戦闘用、スイッチ変数用、感情用、など）
        /// </summary>
        lgui2_ログGUITextスイッチ用,
        /// <summary>
        /// GUIのTextBox3に表示します。わかりやすいように、名前を変えてもOKです。（戦闘用、スイッチ変数用、感情用、など）
        /// </summary>
        lgui3_ログGUITextCPU思考用,



        l999_その他のログテスト,
    }
    /// <summary>
    /// プログラム実行中のテストやデバッグなどに使用する，ログメッセージを一括して格納するクラスです．
    /// ※Window.Form依存です。
    /// </summary>   
    public class CLog
    {
        public static List<CLogData> p_logData = new List<CLogData>();
        public static int s_GUITextBoxMaxNum = 10;
        /// <summary>
        /// ELogType.lgui*_ログGUIText***　に該当するメッセージを表示する、RichTextBoxです。
        /// コンストラクタで、フォームのあるRichTextBoxを引数に指定しておくと、これらのメッセージを
        /// デバッグが呼び出されたタイミングに表示させることが出来ます。
        /// </summary>
        public static RichTextBox[] p_GUITextBox = new RichTextBox[s_GUITextBoxMaxNum];

        /// <summary>
        /// ログを格納するクラスを生成します。
        /// </summary>
        public CLog()
        {
            for (int i = 0; i <= s_GUITextBoxMaxNum - 1; i++)
            {
                p_GUITextBox[i] = new RichTextBox();
            }
        }
        /// <summary>
        /// ログを格納するクラスを生成します。引数にGUIのコントロールを入れると、テキストボックスにログを出力できます。
        /// </summary>
        /// <param name="_GUITextBox1"></param>
        /// <param name="_GUITextBox2"></param>
        /// <param name="_GUITextBox3"></param>
        public CLog(params RichTextBox[]  _GUITextBoxs)
        {
            for (int i = 0; i <= s_GUITextBoxMaxNum - 1; i++)
            {
                if(i <= _GUITextBoxs.Length - 1){
                    p_GUITextBox[i] = _GUITextBoxs[i];
                }else{
                    p_GUITextBox[i] = new RichTextBox();
                }
            }
        }

        /// <summary>
        /// ログを一行出力します．コンソールに表示され，ログとして格納されます．時刻やメソッド名も表示されます．
        /// </summary>
        public static long printlnLog(ELogType _ログの種類名, string _ログ文字列)
        {
            long _時刻 = MyTime.getNowTimeAndMSec_NumberOnly();

            // 一行printlnの場合だけ，メソッド名や時刻を表示
            string _呼び出し先クラス・メソッド名と行番号 = MyTools.getClassMethodName(3, false, true, false); // 3だとgame.デバッグ一行ばっかり
            DateTime _time = MyTime.getDateTime_ByNumberOnly(_時刻);
            string _メッセージ = _time.Hour+":"+_time.Minute+":"+_time.Second+":"+_time.Millisecond
                +"【" + _ログの種類名.ToString() + "】" 
                + _ログ文字列 + " (" + _呼び出し先クラス・メソッド名と行番号 + ")";
            //string _改行置き換えデータ = _記録したいデータ.ToString().Replace("\n", "<改行>");
            
            // コンソール表示もデフォルトではOFF Console.WriteLine(_メッセージ);
            
            // ログの種類によって表示する場所を変える
            if (_ログの種類名 == ELogType.l0_標準出力)
            {
                // コンソールも表示
                Console.WriteLine(_メッセージ);
            }
            else if (_ログの種類名 == ELogType.l4_重要なデバッグ)
            {
                // コンソールも表示
                Console.WriteLine(_メッセージ);
                // 重要なデバッグに関して何か処理をしたかったらここにかく
                // ダイアログを表示
                //MessageBox.Show(_メッセージ);
            }
            else if (_ログの種類名 == ELogType.l5_エラーダイアログ表示)
            {
                // コンソールも表示
                Console.WriteLine(_メッセージ);
                // ダイアログを表示
                MessageBox.Show(_メッセージ);
            }
            else if (_ログの種類名 == ELogType.lgui1_ログGUIText戦闘用)
            {
                if (0 <= p_GUITextBox.Length - 1)
                {
                    p_GUITextBox[0].AppendText(_ログ文字列 + "\n");
                    //p_GUITextBox[0].SendToBack();
                    p_GUITextBox[0].ScrollToCaret();

                }
            }

            return _時刻;
        }
        /// <summary>
        /// ログを出力します．コンソールに表示され，ログとして格納されます．
        /// </summary>
        public static long printLog(ELogType _ログの種類名, string _ログ文字列)
        {
            return printLog(_ログの種類名, (object)_ログ文字列);
        }
        /// <summary>
        /// ログを出力します．コンソールに表示され，ログとして格納されます．
        /// </summary>
        public static long printLog(ELogType _ログの種類名, object _記録したいデータ)
        {
            // 現在時刻を記録
            long _時刻 = MyTime.getNowTimeAndMSec_NumberOnly();

            // データの文字列を表示
            Console.Write(_記録したいデータ.ToString());

            // 最後と同じログの種類でかつStringだったら後ろにくっつける，違うログの種類だったら追加
            CLogData _lastLogData = MyTools.getListValue<CLogData>(p_logData, p_logData.Count - 1);
            if (_lastLogData != null && _lastLogData.data.GetType() == typeof(string) && _lastLogData.type == _ログの種類名.ToString())
            {
                _lastLogData.data = _lastLogData.data.ToString() + _記録したいデータ.ToString();
            }
            else
            {
                p_logData.Add(new CLogData(_ログの種類名, _記録したいデータ, _時刻));
            }

            return _時刻;
        }

        /// <summary>
        /// 一つのログデータです．
        /// </summary>
        public class CLogData
        {
            public long time;
            public string type;
            public object data;

            public CLogData(ELogType _ログの種類名, object _記録したいデータ, long _時刻_yyyyMMddHHmmssaaa_の数字17桁)
            {
                type = _ログの種類名.ToString();
                data = _記録したいデータ;
                time = _時刻_yyyyMMddHHmmssaaa_の数字17桁;
            }
        }
    }

}
