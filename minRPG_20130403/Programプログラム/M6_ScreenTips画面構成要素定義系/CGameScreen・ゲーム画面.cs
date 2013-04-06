using System;
using System.Collections.Generic;
using System.Text;
using Yanesdk.Draw;
using System.Windows.Forms;
using System.Diagnostics;

namespace PublicDomain
{
    /// <summary>
	/// 全てのゲーム画面を管理しているクラスです。
    /// ※現在は、Windows依存で作られています．
	/// </summary>
    public class CGameWindow・ゲーム画面
    {
        /// <summary>
        /// 描画画面として表示するウィンドウです．画面全体に画像やテキストをはっつける描画処理には，極力このクラスを触った方が早い？
        /// ※現在は使っておらず、その代わりにFGameBattleForm1のp_usedFormを使っています。
        /// </summary>
        private Win32Window p_window;
        public Win32Window getP_window() { return p_window; }
        /// <summary>
        /// このゲーム画面を表示（管理）しているWindows専用のFormクラス．
        /// 今は、メインモード選択画面と戦闘画面とシナリオ画面を兼用しています。
        /// ※描画処理をWindows以外の移植をしやすくるすために，描画処理はFormを極力触らないようにしたいんだけど…，
        /// やっぱりFormならではの便利な機能も多いし、出力系はここに結構な処理を書いちゃってると言う現状。。
        /// できるだけメソッド化して後で移植しやすいようにしてください。
        /// </summary>
        private FGameBattleForm1 p_usedForm;
        public FGameBattleForm1 getP_usedFrom() { return p_usedForm; }
        /// <summary>
        /// ゲーム画面にフォーカスが当たっているか（Windowsの場合はフォーカス、それ以外はウィンドウが一番前にあって操作可能状態になっているか）を返します。
        /// ※デフォルトはtrueです。
        /// Windowsの場合は、フォーカスを失ったタイミングでsetFocused()を使ってfalseにしてください。
        /// </summary>
        private bool p_isFocused = true;
        /// <summary>
        /// ゲーム画面にフォーカスが当たっているか（Windowsの場合はフォーカス、それ以外はウィンドウが一番前にあって操作可能状態になっているか）を返します。
        /// </summary>
        public bool getisFocused・フォーカスが当たっているか() { return p_isFocused; }
        /// <summary>
        /// ゲーム画面にフォーカスが当たっているか（Windowsの場合はフォーカス、それ以外はウィンドウが一番前にあって操作可能状態になっているか）を返します。
        /// ※デフォルトはtrueです。
        /// Windowsの場合は、フォーカスを失ったタイミングでsetFocused()を使ってfalseにしてください。
        /// </summary>
        public void setisFoucused・フォーカスが当たっているかを変更(bool _isFocused)
        {
            p_isFocused = _isFocused;
        }
        /// <summary>
        /// ゲーム画面でイベントが起こったか（Windowsの場合はフォーム上でボタン押しやイベント発生）を返します。
        /// ※デフォルトはfalseです。
        /// trueになっても、CGameManager・ゲーム管理者.loopUpdateFrame()メソッドですぐにfalseになります。
        /// </summary>
        private bool p_isEventOccured = true;
        /// <summary>
        /// ゲーム画面でイベントが起こったか（Windowsの場合はフォーム上でボタン押しやイベント発生）を返します。
        /// ※デフォルトはfalseです。
        /// trueになっても、CGameManager・ゲーム管理者.loopUpdateFrame()メソッドですぐにfalseになります。
        /// 　　　　　　省エネモードのフレーム更新フラグに利用されています。
        /// </summary>
        public bool getisEventOccured・イベントが起こった＿省エネモードのフレーム更新に利用() { return p_isFocused; }
        /// <summary>
        /// ゲーム画面でイベントが起こったか（Windowsの場合はフォーム上でボタン押しやイベント発生）を設定します。
        /// ※デフォルトはfalseです。
        /// trueになっても、CGameManager・ゲーム管理者.loopUpdateFrame()メソッドですぐにfalseになります。
        /// 　　　　　　省エネモードのフレーム更新フラグに利用されています。
        /// </summary>
        public void setisEventOccured・イベントが起こったかを設定(bool _isEventOccured)
        {
            p_isEventOccured = _isEventOccured;
            // ユーザ無入力時間をリセット
            game.setUserNoInputTime・ユーザ無入力時刻を設定(true, 0);
        }
        

        /// <summary>
        /// テキストを表示するメインメッセージボックスです．
        /// 出来るだけゲーム画面とは独立して管理できるように設計されています。
        /// </summary>
        private CWinMainTextBox・メインテキストボックス p_messageBox;
        public CWinMainTextBox・メインテキストボックス getP_messageBox() { return p_messageBox; }

        /// <summary>
        /// 現在起動しているゲームの情報を全クラスでやりとりする，ゲームデータです．
        /// </summary>
        private CGameManager・ゲーム管理者 game;

		/// <summary>
		/// Windows専用ゲーム画面の初期化処理です．
		/// </summary>
		/// <param name="width">ウィンドウの描画領域の横幅</param>
		/// <param name="height">ウィンドウの描画領域の縦幅</param>
        public CGameWindow・ゲーム画面(CGameManager・ゲーム管理者 _ゲームデータg, int _幅, int _高さ, FGameBattleForm1 _usedForm・使用フォーム, CWinMainTextBox・メインテキストボックス _メッセージボックス)
		{
            this.game = _ゲームデータg;
            p_usedForm = _usedForm・使用フォーム;
			p_usedForm.ClientSize = new System.Drawing.Size(_幅, _高さ);
            p_messageBox = _メッセージボックス;

            // ウィンドウの初期化
			p_window = new Win32Window(p_usedForm.Handle);
			//p_usedForm.timer1.Enabled = true; // タイマー開始。

            // フォームの表示
            p_usedForm.Show();
		}


		/// <summary>
		/// 後始末
		/// </summary>
		private void GameWindow_FormClosed(object sender, FormClosedEventArgs e)
		{
			//usedForm1.timer1.Enabled = false; // 念のためタイマー止めておく
			//p_window.Dispose();
		}

        /*
		/// <summary>
		/// サイズが変更されたらScreenの方も直しておく。
		/// </summary>
		/// <remarks>
		/// 今回はサイズ変更は禁止してるので、要らないといえば要らない。
		/// </remarks>
		protected override void OnSizeChanged(EventArgs _e)
		{
			base.OnSizeChanged(_e);
			if (p_window != null)
			{
				p_window.p_Screen.UpdateView(ClientSize.Width, ClientSize.Height);
			}
		}

		/// <summary>
		/// このメソッドをoverrideして、backgroundへの描画を
		/// 潰しておかないと、画面がちらつく。
		/// </summary>
		/// <remarks>
		/// …というコードがYanesdkのサンプルなどにあるのだが、
		/// 別に無くてもちらつかないような？
		/// </remarks>
		protected override void OnPaintBackground(PaintEventArgs _e)
		{
 			 //base.OnPaintBackground(_e);
		}
         * */

        ///// <summary>
        ///// タイマー処理（ゲームのメインタスク処理・描画処理）をロックするかどうか
        ///// </summary>
        //bool _timerLock = false;
        ///// <summary>
        ///// <summary>
        ///// タイマーによって定期的に呼ばれるメソッド。
        ///// </summary>
        //public void doTimerEvent(object sender, EventArgs _e)
        //{
        //    if (_timerLock) return;
        //    _timerLock = true;
        //    // SDLの一部の関数の呼び出しや、
        //    // MessageBox、Form.ShowDialog()などによって
        //    // メッセージループが回されてしまうと、
        //    // 予想外の再帰的な呼び出しが起きてしまう可能性があるので、
        //    // ロックしておく。
        //    // ※ ちなみに、特別この関数を別のスレッドから呼び出したりしない限りは
        //    // Formのスレッドから呼び出されるはずなので、マルチスレッド対策はいらないはず。
        //    try
        //    {
        //        if (game.p_fpsManager・フレーム管理者.ToBeRendered)
        //        {
        //            if (game.TaskController.End)
        //            {
        //                Close();
        //            }
        //            else
        //            {
        //                // 溜めていたタスクを次々と処理していく
        //                game.TaskController.CallTask(game);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        _timerLock = false;
        //    }
        //}

		/// <summary>
		/// 描画先の取得。
		/// </summary>
		public Screen2DGl Screen
		{
			get { return p_window.Screen; }
        }


        #region 自前の画面処理メソッド
      

        #endregion


        public string getNowMainMessageText()
        {
            return getP_messageBox().getNowMessage・現在のメッセージを取得();
        }
    }
}
