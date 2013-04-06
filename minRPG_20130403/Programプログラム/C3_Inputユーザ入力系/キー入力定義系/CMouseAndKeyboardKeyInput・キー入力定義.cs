using System;
using System.Runtime.InteropServices;
using Sdl;
using System.Collections.Generic;

namespace PublicDomain
// 以下、ほとんどはYaneSDKのソースを引用。やねうらお様に感謝しますm(_ _)m

{
	/// <summary>
	/// キー入力定義クラス。
    /// 
    /// キーボードのキー入力に加えて、
    /// マウスボタンの入力（左右クリックだけでなく、長押し、ダブルクリックなどの時間も定義）も一括して管理できるようにしたクラスです。
    /// キーコードの参照は、EKeyCodeを使ってください。
    /// 
    /// なお、キーボードのキーのみを扱うクラスは、old_CKeyboardInputを参照してください。
    ///
    /// ■（一般的なコンシューマゲームのボタン押し判定はIsPull（押し離した瞬間true）、
    /// シューティングなど連射対応型はIsPress（押してる間true）だと、思います。）
    /// 
    /// 詳細な違いはIKeyInputクラスを参照してください。
    /// 
    /// ※isPress、IsPush、IsPull、IsFreeの意味は、それぞれのメソッドの説明を読んで、注意して使ってください。
    /// 
	/// </summary>
	/// <remarks>
	/// <Para>
	/// キーのスキャンコードはSDLのものと同等
	/// →SDL/SDL_keysym.dを参照のこと。
	/// →SDLのimportをしたくない場合、
	/// 	このヘッダのなかにあるEKeyCodeというenumを用いても良い。
	/// </Para>
	/// <Para>
	/// 関数仕様についてはIKeyInputインターフェースと同じなのでそちらを参照のこと。
	/// </Para>
	/// </remarks>
	public class CMouseAndKeyBoardKeyInput・キー入力定義 : IKeyInput・入力インタフェースに必要な機能定義クラス
    {

        #region エミュレート機能の実装: setPress/setPull/setPush/setFree
        /// <summary>
        /// 第二引数に指定したキーコード（_keycode）の押し離し情報（_isDown）を意図的に置き換えます。
        /// 第三引数をtrueにすると次のフレーム、falseにすると現在のフレームのキー押し離し情報を置き換えます。
        /// （※基本的に、現在のフレームの判定処理は終わっているので、次のフレームもあわせて設定しないと基本は認識されません。
        /// 　　よくわからない場合はこのメソッドは触らず、setPullを触ってください）。
        /// 
        /// （このメソッド以外にも、p_keyIsDown[][] = _isDownを代入している個所はたくさんありますが、現在ではメソッド呼び出し時間短縮のため、このメソッドに統一していません）
        /// 
        ///         /// ※このメソッドは、時系列的には特殊キー setPull((int)EKeyCode.***_DOUBLECLICK) などにも対応していますが、
        /// 元のキーコードの押し離し（***）まで変更していないので、使う場合は元のキーコードもあわせて
        ///  setPull((int)EKeyCode.***) も置き換えておいた方が無難です。
        /// </summary>
        /// <param name="_keycode"></param>
        /// <returns></returns>
        public bool setPress(bool _isDown, int _keycode, bool _TrueIsNextFrame_FalseIsNow)
        {
            bool _isSuccess = true;
            if (s_isPriorityFirstUserInput_ThanAutoInput・自動入力よりユーザ入力を優先 == true)
            {
                // エミュレートよりユーザの入力を優先させる場合、
                // ユーザがそのキーを押し続けていたら、エミュレートしない。
                if (p_keyIsDown[p_flip][_keycode] == true && p_keyIsDown[1 - p_flip][_keycode] == true)
                {
                    _isSuccess = false;
                }
            }

            if(_TrueIsNextFrame_FalseIsNow == true){
                // 現在の値のフレームの押し離し情報を置き換え
                p_keyIsDown[p_flip][_keycode] = _isDown;
            }else{
                // 次のフレームで押されるキーコードを格納しておいて、次のフレーム開始時に意図的に「押す」に置き換える
                // 自動押しされるキーはユーザが押してないことが前提（ユーザが押していたらずっと_isDownになる仕様で問題無い）
                // だから、_isDown==trueだけでいい
                if(_isDown == true){
                    if (p_NextToPressKeysNum < p_MultiPressedKeysNumMax・１フレーム最大キー認識数)
                    {
                        p_NextToPressKeys[p_NextToPressKeysNum] = _keycode;
                    }
                    p_NextToPressKeysNum++;
                }
            }
            if (_isSuccess == false)
            {
                return false; // エミュレート失敗
            }
            if (Program・実行ファイル管理者.isDebug == true)
            {
                // エミュレート成功
                string _KeyStateString = "押し";
                if (_isDown == false) _KeyStateString = "離し";
                string _FrameString = "今フレーム";
                if (_TrueIsNextFrame_FalseIsNow == true)
                {
                    _FrameString = "次フレーム";
                }
                MyTools.ConsoleWriteLine("setPress: " + MyTools.getEnumName<EKeyCode>(_keycode) + " キー「"+_KeyStateString+"」（"+_FrameString+"）を自動入力しました");
            }
            return _isSuccess;
        }
        /// <summary>
        /// ユーザ入力と自動入力の優先順位です。デフォルトではfalse（自動入力の方が優先されます）。
        /// </summary>
        public static bool s_isPriorityFirstUserInput_ThanAutoInput・自動入力よりユーザ入力を優先 = false;

        /// <summary>
        /// いわゆる入力エミュレートメソッドです。成功したらtrueを返します。
        /// 
        /// 具体的には、指定したキーコード（_keycode）を、次のフレームで押し離した瞬間を検知する
        /// （IsPull(_keycode)==trueになる）ように、キー押し離し情報を意図的に置き換えます。
        /// 
        /// ※このメソッドは、時系列的には特殊キー setPull((int)EKeyCode.***_DOUBLECLICK) などにも対応していますが、
        /// 元のキーコードの押し離し（***）まで変更していないので、使う場合は元のキーコードもあわせて
        ///  setPull((int)EKeyCode.***) も置き換えておいた方が無難です。
        /// </summary>
        /// <param name="_keycode"></param>
        /// <returns></returns>
        public bool setPull(int _keycode){
            bool _Down = true;
            bool _Up = false;
            bool _isSuccess =   setPress(_Down, _keycode, false);   // 現在のフレームで押され、
            _isSuccess =        setPress(_Up, _keycode, true);      // 次のフレームで離される、ように置き換え
            if (_isSuccess == false)
            {
                return false; // エミュレート失敗
            }
            if (Program・実行ファイル管理者.isDebug == true)
            {
                //MyTools.ConsoleWriteLine("setPull: " + MyTools.getEnumName<EKeyCode>(_keycode) + " キー押し離しを自動入力しました");
            }
            return true;
        }
        /// <summary>
        /// いわゆる入力エミュレートメソッドです。成功したらtrueを返します。
        /// 
        /// 具体的には、指定したキーコード（_keycode）を、次のフレームで離し押しした瞬間を検知する
        /// （IsPush(_keycode)==trueになる）ように、キー押し離し情報を意図的に置き換えます。
        /// 
        /// ※このメソッドは、時系列的には特殊キー setPull((int)EKeyCode.***_DOUBLECLICK) などにも対応していますが、
        /// 元のキーコードの押し離し（***）まで変更していないので、使う場合は元のキーコードもあわせて
        ///  setPull((int)EKeyCode.***) も置き換えておいた方が無難です。
        /// </summary>
        /// <param name="_keycode"></param>
        /// <returns></returns>
        public bool setPush(int _keycode)
        {
            bool _Down = true;
            bool _Up = false;
            bool _isSuccess =   setPress(_Up, _keycode, false);         // 現在のフレームで離され、
            _isSuccess =        setPress(_Down, _keycode, true);        // 次のフレームで押される、ように置き換え
            if (_isSuccess == false)
            {
                return false; // エミュレート失敗
            }
            if (Program・実行ファイル管理者.isDebug == true)
            {
                //MyTools.ConsoleWriteLine("setPush: " + MyTools.getEnumName<EKeyCode>(_keycode) + " キー離し押しを自動入力しました");
            }
            return true;
        }
        /// <summary>
        /// いわゆる入力エミュレートメソッドです。成功したらtrueを返します。
        /// 
        /// 具体的には、指定したキーコード（_keycode）を、次のフレームで離し続けている瞬間を検知する
        /// （IsFree(_keycode)==trueになる）ように、キー押し離し情報を意図的に置き換えます。
        /// 
        /// ※このメソッドは、時系列的には特殊キー setPull((int)EKeyCode.***_DOUBLECLICK) などにも対応していますが、
        /// 元のキーコードの押し離し（***）まで変更していないので、使う場合は元のキーコードもあわせて
        ///  setPull((int)EKeyCode.***) も置き換えておいた方が無難です。
        /// </summary>
        /// <param name="_keycode"></param>
        /// <returns></returns>
        public bool setFree(int _keycode)
        {
            bool _Up = false;
            bool _isSuccess =   setPress(_Up, _keycode, false);         // 現在のフレームで離され、
            _isSuccess =        setPress(_Up, _keycode, true);        // 次のフレームでも離される、ように置き換え
            if (_isSuccess == false)
            {
                return false; // エミュレート失敗
            }
            if (Program・実行ファイル管理者.isDebug == true)
            {
                //MyTools.ConsoleWriteLine("setFree: " + MyTools.getEnumName<EKeyCode>(_keycode) + " キー離し続けを自動入力しました");
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 押してる間はTrue。現在押されているか(状態はupdate関数を呼び出さないと更新されない)
        /// 
        /// ボタン ：-------___________________----
        /// isPress: 000000011111111111111111110000　押してる間はTrue　→　押し中か
        /// </summary>
		/// <param name="_keycode"></param>
		/// <returns></returns>
		public bool IsPress(int _keycode) {
			if (_keycode >= ButtonNum) return false;
			return (p_keyIsDown[p_flip][_keycode]==true);
		}

		/// <summary>
        /// 押してる間はTrue。現在押されているか(状態はupdate関数を呼び出さないと更新されない)
        /// 
        /// ボタン ：-------___________________----
        /// isPress: 000000011111111111111111110000　押してる間はTrue　→　押し中か
        /// 
        /// ※IsPress(int)のほうはcastするのが面倒なので
        /// こちらも用意。
		/// </summary>
		/// <param name="_keycode"></param>
		/// <returns></returns>
		public bool IsPress(EKeyCode key){
			return IsPress((int)key);
		}

		/// <summary>
		/// どれかひとつでも押されていれば trueが返る。
        /// 
        /// 押してる間はTrue。現在押されているか(状態はupdate関数を呼び出さないと更新されない)
        /// 
        /// ボタン ：-------___________________----
        /// isPress: 000000011111111111111111110000　押してる間はTrue　→　押し中か
		/// </summary>
		public bool IsPress()
		{
			for (int i = 0; i < ButtonNum; ++i) {
				if (IsPress(i))
					return true;
			}
			return false;
		}

		/// <summary>
        /// 押し離した瞬間にTrue。前回のupdateのときに押されていて、今回のupdateで押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isPull : 000000000000000000000000001000　押し離した瞬間か　→　押し離したか
        /// </summary>
		/// <returns></returns>
		public bool IsPull(int _keycode)
		{
			if (_keycode >= ButtonNum) return false;

            // デバッグテスト用（もうちゃんとできるようになったので用済みだが、一応残しておく）
            //if (Program・実行ファイル管理者.isDebug == true)
            //{
            //    if (_keycode == (int)EKeyCode.MOUSE_LEFT_DOUBLECLICK)
            //    {
            //        string _down = "凹"; string _up = "凸";
            //        string _before = (p_keyIsDown[1 - p_flip][_keycode] == true) ? _down : _up;
            //        string _now = (p_keyIsDown[p_flip][_keycode] == true) ? _down : _up;
            //        string _result = ((p_keyIsDown[1 - p_flip][_keycode] == true) && (p_keyIsDown[p_flip][_keycode] == false)) ? "押し離しTrue" : "押し離しFalse";
            //        string _methodName = MyTools.getMethodStackString(6);
            //        string _debugPushInfo = _methodName + "IsPull(" + EKeyCode.MOUSE_LEFT_DOUBLECLICK.ToString() + "): 前今回のキーフレーム " + _before + _now + " により、" + _result;
            //        MyTools.ConsoleWriteLine(_debugPushInfo);
            //    }
            //}
            //これじゃだめ。0^1=1, 1^1=1, で、ぜんぶ1になる。int a = p_flip ^ 1;
			return (p_keyIsDown[1 - p_flip][_keycode]) && (!p_keyIsDown[p_flip][_keycode]);
		}

		/// <summary>
        /// 押し離した瞬間にTrue。前回のupdateのときに押されていて、今回のupdateで押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isPull : 000000000000000000000000001000　押し離した瞬間か　→　押し離したか
        /// 
        /// 　　※IsPull(int)のほうはcastするのが面倒なので
		/// こちらを使っても良い。
		/// </summary>
		/// <param name="_keycode"></param>
		/// <returns></returns>
		public bool IsPull(EKeyCode key)
		{
			return IsPull((int)key);
		}
        /// <summary>
        /// どれか一つでもボタンを押し離した瞬間にTrueになる。
        /// 
        /// 押し離した瞬間にTrue。前回のupdateのときに押されていて、今回のupdateで押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isPull : 000000000000000000000000001000　押し離した瞬間か　→　押し離したか
        /// </summary>
        /// <returns></returns>
        public bool IsPull()
        {
            for (int i = 0; i < ButtonNum; ++i)
            {
                if (IsPull(i))
                    return true;
            }
            return false;
        }



        /// <summary>
        /// 押し初めにTrue。前回のupdateのときに押されていなくて、今回のupdateで押されたか。
        /// 
        /// ボタン ：-------___________________----
        /// isPush : 000000010000000000000000000000　離し押した瞬間か　→　押し初めか
        /// 
        ///      ■（一般的なコンシューマゲームのボタン押し判定はisPull（押し離した瞬間true）、
        /// シューティングなど連射対応型はPress（押してる間true）だと、思います。）
        /// 詳細な違いはIKeyInputクラスを参照してください。
        /// </summary>
        /// <returns></returns>
		public bool IsPush(int _keycode) {
			if (_keycode >= ButtonNum) return false;
			return (p_keyIsDown[1-p_flip][_keycode]==false)
				&& (p_keyIsDown[p_flip][_keycode]==true);
		}

        /// <summary>
        /// 押し初めにTrue。前回のupdateのときに押されていなくて、今回のupdateで押されたか。
        /// 
        /// ボタン ：-------___________________----
        /// isPush : 000000010000000000000000000000　離し押した瞬間か　→　押し初めか
        /// 
        ///      ■（一般的なコンシューマゲームのボタン押し判定はisPull（押し離した瞬間true）、
        /// シューティングなど連射対応型はPress（押してる間true）だと、思います。）
        /// 詳細な違いはIKeyInputクラスを参照してください。

        /// 
        /// 　　※IsPush(int)のほうはcastするのが面倒なので
		/// こちらも用意。
		/// </summary>
		/// <param name="_keycode"></param>
		/// <returns></returns>
		public bool IsPush(EKeyCode key)
		{
			return IsPush((int)key);
		}

		/// <summary>
        /// どれかひとつでも押し初めならば trueが返る。
        /// 
        /// 押し初めにTrue。前回のupdateのときに押されていなくて、今回のupdateで押されたか。
        /// 
        /// ボタン ：-------___________________----
        /// isPush : 000000010000000000000000000000　離し押した瞬間か　→　押し初めか
        /// 
        ///      ■（一般的なコンシューマゲームのボタン押し判定はisPull（押し離した瞬間true）、
        /// シューティングなど連射対応型はPress（押してる間true）だと、思います。）
        /// 詳細な違いはIKeyInputクラスを参照してください。
        /// </summary>
		/// <returns></returns>
		public bool IsPush() {
			for (int i = 0; i < ButtonNum; ++i) {
				if (IsPush(i))
					return true;
			}
			return false;
		}


		/// <summary>
        /// 離してる間はTrue。前回のupdateのときに押されていなくて、今回のupdateでも押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isFree : 111111100000000000000000000111  離してる時はTrue　→　離し中か
        /// </summary>
		/// <returns></returns>
		public bool IsFree(int _keycode)
		{
			if (_keycode >= ButtonNum) return false;
			return (!p_keyIsDown[p_flip ^ 1][_keycode]) && (!p_keyIsDown[p_flip][_keycode]);
		}

		/// <summary>
        /// 		/// 離してる間はTrue。前回のupdateのときに押されていなくて、今回のupdateでも押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isFree : 111111100000000000000000000111  離してる時はTrue　→　離し中か
        /// 
        /// 　　※IsFree(int)のほうはcastするのが面倒なので
		/// こちらを使っても良い。
		/// </summary>
		/// <param name="_keycode"></param>
		/// <returns></returns>
		public bool IsFree(EKeyCode key)
		{
			return IsFree((int)key);
		}


		/// <summary>
		/// 認識可能なボタンの総数を返します。
		/// </summary>
		/// <returns></returns>
		public int ButtonNum { get { return p_keyIsDown[0].Length; } }

		/// <summary>
		/// デバイスの名前（このクラスではマウスも取ってますが一応メインはキーボードなので"KeyBoard"）を返します。
		/// </summary>
		/// <returns></returns>
		public string DeviceName { get { return "KeyBoard"; } }

		/// <summary>
		/// IntPtr.Zeroを返します。
		/// </summary>
		/// <returns></returns>
		public IntPtr Info { get { return IntPtr.Zero; } }

		/// <summary>
		/// デバイスをopenしているわけではないのでcloseする必要がない。
		/// </summary>
		public void Dispose()
		{
		}

		/// <summary>
        /// 現在のキー情報（マウスボタンも含む）を取得します。
        /// このUpdate()メソッドを、１フレーム毎に定期的に呼び出すと、
        /// マウスやキーボードのイベントが、このクラスで認識可能なキーコードEKeyCodeで扱えます。
        /// 
        /// ※例：　IsPull(_EKeyCode)== true だと、そのボタンが押し離した瞬間だけtrue。IsPress(_EKeyCode)== true だと、そのボタンが押されている状態ならtrue。
		/// </summary>
		/// <remarks>
		/// <Para>
		/// SDLのSDL_PumpEventsを定期的に呼び出していることを前提とする。
		/// ここで得られるのは前回のSDL_PumpEventsを呼び出してからの
		/// 入力情報である。
		/// </Para>
		/// <Para>
		/// また、キー入力には、
		/// 	SDL_SetVideoMode(640,480,16,SDL_SWSURFACE);
		/// に相当する命令で、SDLのウィンドゥを初期化してある必要がある。
		/// (キー入力は、ウィンドゥに付帯するものなので)	
		/// 
		/// Windows上で動作させるときは、
		/// GetAsyncKeyStateを用いるのでフォーカスがどこにあろうとキー入力が出来る。
		/// もしこの仕様がまずければこのクラスは使うべきではない。
		/// 
		/// また、EnterキーはNum Padのほうと区別がつかない。
		/// Num PadのほうのEnterが押されてもKeyCode.RETURNが返ってくるので注意。
		/// あと、すべてのキーの情報は取得していない。(普段使わないキーをチェックしても
		/// 処理時間がもったいないため) サポートしているキーについては
		/// CheckKeyInWindowsメソッドの実装を見ること。これでまずければ自前で書いて。
		/// </Para>
		/// </remarks>
		public void Update() {
            // ■配列の前後を逆転させる。
            // （こうすることで、連続的に前回の状態と今回の状態を無駄なく格納していける。ただし、配列の前後関係は逆転する）
            // 前回の状態を[1-p_flip]、今回の状態を[p_flip]とすれば問題なく使える
			p_flip = 1 - p_flip;

            // ■1.環境に合わせた、キー押し離し状態の取得
			if (Yanesdk.System.Platform.IsLinux)
			{
                //  ■1.1.Linuxの場合、SDLから取ったキーコードをそのままマッピング
				IntPtr pkeys = SDL.SDL_GetKeyState(IntPtr.Zero);
				byte[] keys = new byte[ButtonNum];
				Marshal.Copy(pkeys, keys, 0, keys.Length);
				for (int i = 0; i < keys.Length; i++)
				{
					p_keyIsDown[p_flip][i] = (keys[i] != 0);
				}
			}
			else 
            {
                //  ■1.2.Windowsの場合、SDLじゃうまいこととれないからいろいろややこしいことしてる
				/*
				if (userControl != null || userForm != null)
				{
					// キー入力のイベントハンドラをhookしているので
					// 何もしなくとも入力されると思われ。
				}
				else
				{
					IntPtr pkeys = SDL.SDL_GetKeyState(IntPtr.Zero);
					byte[] keys = new byte[ButtonNum];
					Marshal.Copy(pkeys, keys, 0, keys.Length);
					for (int i = 0; i < keys.Length; i++)
					{
						p_keyIsDown[p_flip][i] = keys[i] != 0;
					}
				}
				 */
				// あかんこの方法ではうまいこといかへん。仕方ないからWindows32APIのGetAsycKeyStateを使うわ
				CheckKeyInWindows・このフレームでこのクラスで認識可能なキー押し離し状態を格納();

                // [TODO]…あれ？Macの場合は？YaneSDKに記述されてない？Windowsのやり方でいけるん？
                //if(Yanesdk.System.Platform.IsWindows)
                //{
                //}
                //else if(Yanesdk.System.Platform.IsMac)
                //{
                //}
            }
            // ■1.キー押し離し情報の取得、終わり
            
            // ■1.キーエミュレート処理
            if(p_NextToPressKeysNum > 0){
                // 次のフレームで押される予定だったキーを押す　（キー押し離し情報を意図的に「押す」に上書き）
                foreach(int _keycode in p_NextToPressKeys){
                    p_keyIsDown[p_flip][_keycode] = true;
                }
            }
            // 次に入力されるキーの初期化
            p_NextToPressKeysNum = 0;

            // ■2.特殊なキーの判定
            //      ■2.1ダブルトリプル連打判定
            checkDubbleTriplePress・ダブルトリプル連打判定();
            //      ■2.2長押し判定
            checkLongPressedButton・長押しボタンを判定();

            // ■3.最後に押し離したキー履歴に関する処理
            // フリップを更新
            // [0]初A[1]無し　→　[0]初A[1]次B　→　[0]次の次C[1]次B　→ [0]次の次C[1]次の次の次D ...
            p_lastPulledFlip = 1 - p_lastPulledFlip;
            // ■4.今回このフレームで、押し離したキー（マウスボタンも含む）が一つでもあれば、
            // 最後に押したキーを更新する。また、その時刻も格納する。
            if (p_isKeyPulled・今回のフレームで一つでもキーが押し離されたか == true)
            {
                // 最後に押したキーたちを更新
                p_nowPulledKeys.CopyTo(p_lastPulledKeys, 0);
                p_lastPulledKeysNum = p_nowPulledKeysNum;
                // 今回のフレームで押し離したキーを押し離した時間を更新
                int _nowPressedTime = MyTools.getNowTime_fast();
                p_lastKeyPulledTime[p_lastPulledFlip] = _nowPressedTime; // （この処理は絶対に2.1ダブルトリプル連打判定「後」にもってこないと、トリプル連打判定ができない）
            }

		}

		private void Init()
		{
			p_keyIsDown = new bool[2][];
			for (int i = 0; i < 2; ++i)
			{
				p_keyIsDown[i] = new bool[MyTools.getEnumIntMaxCount<EKeyCode>()];
			}
            p_lastKeyPulledTime = new int[2]; // 初期値は全部0でOK
            p_lastPulledKeys = new int[p_MultiPressedKeysNumMax・１フレーム最大キー認識数]; // 初期値は全部0（(int)EKeyCode._NONE）でOK
            p_nowPulledKeys = new int[p_MultiPressedKeysNumMax・１フレーム最大キー認識数]; // 初期値は全部0（(int)EKeyCode._NONE）でOK
            p_NextToPressKeys = new int[p_MultiPressedKeysNumMax・１フレーム最大キー認識数]; // 初期値は全部0（(int)EKeyCode._NONE）でOK
		}

		/// <summary>
		/// 
		/// </summary>
		public CMouseAndKeyBoardKeyInput・キー入力定義()
		{
			Init();
		}

	/*
		/// <summary>
		/// Windowsプラットフォームの場合は、UserControlを渡して、そのUserControlに
		/// 対するキー入力を取得できる
		/// </summary>
		/// <param name="control"></param>
		public CKeyboardInput(global::System.Windows.Forms.UserControl control)
		{
			Init();
			userControl = control;
			userControl.KeyDown+=
				new global::System.Windows.Forms.KeyEventHandler(KeyDown);
			userControl.KeyUp+=
				new global::System.Windows.Forms.KeyEventHandler(KeyUp);
		}
		private global::System.Windows.Forms.UserControl userControl = null;

		/// <summary>
		/// Windowsプラットフォームの場合は、Formを渡して、そのフォームに
		/// 対するキー入力を取得できる
		/// cf.FormUpdate
		/// </summary>
		public CKeyboardInput(global::System.Windows.Forms.Form form)
		{
			Init();
			userForm = form;

			// このformに付随するコントロールすべてのkey eventをhookする必要あり？
			userForm.KeyDown+=
				new global::System.Windows.Forms.KeyEventHandler(KeyDown);
			userForm.KeyUp +=
				new global::System.Windows.Forms.KeyEventHandler(KeyUp);
		}

		/// <summary>
		/// フォームからキーを入力しようとした場合、その下にControlが乗っていると
		/// そのControlに食われてしまう。そこで、そこに乗っているControlすべての
		/// KeyDown,KeyUpをhookしなければならない。
		/// 
		/// また、フォームにControlを加えたときも同様で、
		/// そこに乗っているControlの内容の
		/// </summary>
		public void FormUpdate()
		{

		}

		private global::System.Windows.Forms.Form userForm = null;

		// keyの押し下げ/押し上げをhookする。
		private void KeyDown(object sender, global::System.Windows.Forms.KeyEventArgs _e){
			OnKey(_e, true);
		}
		private void KeyUp(object sender, global::System.Windows.Forms.KeyEventArgs _e)
		{
			OnKey(_e, false);
		}

		private void OnKey(global::System.Windows.Forms.KeyEventArgs _e,bool press)
		{
			int f = 1-p_flip;

			// すべてのキーに対するmappingを行なわなくてはならない。(´ω`)
			switch (_e.KeyCode)
			{
				case Keys.A: p_keyIsDown[f][(int)KeyCode.a] = press; break;
				case Keys.B: p_keyIsDown[f][(int)KeyCode.b] = press; break;
				case Keys.C: p_keyIsDown[f][(int)KeyCode.c] = press; break;
			}
		}
	*/
		// あかん、この方法ではうまいこといかへん。
		// 仕方ない。全部のキーコードスキャンしよか..(´ω`)
        /// <summary>
        /// Windows32APIを使って、キーボードで押されたキーコードをスキャンする処理です。マウスのボタンも一部取得できます。
        /// </summary>
        /// <param name="nVirtKey"></param>
        /// <returns></returns>
		[DllImport("user32")]
		public static extern short GetAsyncKeyState(int _intWindowsVirtualKeyState);
        /// <summary>
        /// Windows32APIのGetAsyncKeyState(該当の仮想キーコード_intWindowsVirtualKeyState)を使って、現在の_EKeyCodeキーが押されているかに変換してp_keyPressed[][]の値を設定します。
        /// 返り値は、そのキーが押されているかを取得します。マウスのボタンも一部扱います。
        /// 
        /// ※このメソッドは、自前で作った特殊なキー（***_DOUBLEPRESS/TRIPLEPRESSや***_LONGPRESS）に対応していません。
        /// 特殊なキーはsetDoublePulledなどを使ってください。
        /// </summary>
        /// <returns></returns>
        private bool setKeyState・現在のキー押し離し状態を設定(int _EKeyCode_ToInt, int _intWindowsVirtualKeyState)
        {
            // 現在のキー押し離し状態を設定
            // 例：
            //  p_keyIsDown[p_flip][(int)EKeyCode.KEY0 + i] = (GetAsyncKeyState(0x30 + i) & 0x8000) != 0;
            // を、以下にするだけ
            // bool _isDown = setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.KEY0 + i, 0x30 + i);
            bool _isDown = (GetAsyncKeyState(_intWindowsVirtualKeyState) & 0x8000) != 0;
            p_keyIsDown[p_flip][_EKeyCode_ToInt] = _isDown;

            // 今回のフレームで押し離したキーを更新
            if (_isDown == false)
            {
                // 押し離しされたか（IsPull）をここで調べる
                if (IsPull(_EKeyCode_ToInt) == true)
                {
                    if (p_isKeyPulled・今回のフレームで一つでもキーが押し離されたか == false)
                    {
                        p_isKeyPulled・今回のフレームで一つでもキーが押し離されたか = true;
                    }
                    if (p_nowPulledKeysNum < p_MultiPressedKeysNumMax・１フレーム最大キー認識数)
                    {
                        // 今回のフレームで押し離したキーまたはマウスボタン（配列の一番若いもの）を更新。ただし、ダブルトリプルは含まない。
                        p_nowPulledKeys[p_nowPulledKeysNum] = (int)_EKeyCode_ToInt; // この処理は必ず2.1ダブルトリプル連打判定「前」に持ってくる
                        p_nowPulledKeysNum++;
                    }
                }
            }
            // デバッグ用
            //if (_EKeyCode_ToInt == (int)EKeyCode.MOUSE_LEFT_DOUBLECLICK)
            //{
            //    int a = 0; //何かがおかしい。ここにはダブル系は入って来ないはず
            //}
            return _isDown;
        }
        #region 以下、前バージョンのforループでやる場合の草案と注意（もう既にgetKeyStateでやっているから今は必要ない。）
        //public bool setLastPulledKey()
        //{
            //// ■1.今回このフレームで、押し離したキー（マウスボタンも含む）が一つでもあれば、
            //// そのKeyCodeを一つだけ（配列の小さいもの）を格納する。また、その時刻も格納する。
            //int _nowPressedTime = MyTools.getNowTime_fast();
            //int _nowPressedKey = (int)EKeyCode._NONE; // デフォルトは0
            //// ※ここ注意。前回のフレームでダブル押しやトリプル押しがtrueの場合、そのIsPull(***DoublePulledのKeyCode)がtrueになるから、永遠にtrueになってしまう。
            //// →対策として、前回のフレームでダブル押しがtrueだと、調べないようにしておくとＯＫ。でもトリプル押しのときは***DobulePulledはいらないけど***の押し離しは調べないといけないので注意！
            //// まだ2.1ダブルトリプル連打判定をやってないから、今回のフレームが前回のフレームになる
            ////↓の二行じゃあまだ不完全。なぜかというと、TrippleのときにDoubleが連続的に検出されてしまうから。
            //if (p_isDoublePulled・今回のフレームでダブル連打がされたか == false && p_isTripplePulled・今回のフレームでトリプル連打がされたか == false
            //    || p_isDoublePulled・今回のフレームでダブル連打がされたか == true && p_isTripplePulled・今回のフレームでトリプル連打がされたか == false)
            //{
            //    for (int i = 0; i < ButtonNum; i++)
            //    {
            //        if (IsPull(i) == true)
            //        {
            //            _nowPressedKey = i;
            //            break; // EKeyCodeが早い順に優先（マウスボタン系 < Shift系 < A系 なので、ほとんどの場合はかぶらないはず）
            //        }
            //    }
            //}
            // 今回のフレームで押し離したキーを更新
            //p_lastPulledKeys = _nowPressedKey; // この処理は2.1ダブルトリプル連打判定「前」に持ってくる
        //}
        #endregion
        /// <summary>
        /// 今回のフレームのCheckKeyInWindows()の中で、一つでもキーが押されたことを検知したらtrueを返します。p_lastPulledKeyを取得するために使います。
        /// </summary>
        public bool p_isKeyPulled・今回のフレームで一つでもキーが押し離されたか = false;
        /// <summary>
        /// 今回のフレームで押したキー（マウスボタンを含む）のキーコードEKeyCodeを、Int型で格納した配列です。
        /// 
        /// ※同フレームに２つ以上のキーが押された（完全同期の同時押しの）場合は、
        /// キーコードの値が小さい方が少ない配列に格納されています（マウス右より左、マウス左よりBキー、BキーよりAキーが優先）。
        /// ***_DOUBLE, ***TRIPLE, ***LONGPRESSのキーコードは格納しません。
        /// </summary>
        private int[] p_nowPulledKeys;
        /// <summary>
        /// 今回のフレームで押したキー（マウスボタンを含む）の同時押し数です。p_nowPulledKeyのキー格納数として使います。
        /// </summary>
        private int p_nowPulledKeysNum = 0;
        /// <summary>
        /// 最後に押したキー（マウスボタンを含む）のキーコードEKeyCodeを、Int型で格納した配列です。
        /// 
        /// ※同フレームに２つ以上のキーが押された（完全同期の同時押しの）場合は、
        /// キーコードの値が小さい方が少ない配列に格納されています（マウス右より左、マウス左よりBキー、BキーよりAキーが優先）。
        /// ***_DOUBLE, ***TRIPLE, ***LONGPRESSのキーコードは格納しません。
        /// </summary>
        private int[] p_lastPulledKeys;
        /// <summary>
        /// 最後に押したキー（マウスボタンを含む）の同時押し数です。p_lastPulledKeyのキー格納数として使います。
        /// </summary>
        private int p_lastPulledKeysNum = 0;
        /// <summary>
        /// 次のフレームで押す予定のキー（マウスボタンを含む）のキーコードEKeyCodeを、Int型で格納した配列です。
        /// キーエミュレータ機能として、setPressメソッドの実現に使います。
        /// 
        /// ※同フレームに２つ以上のキーが押された（完全同期の同時押しの）場合は、
        /// キーコードの値が小さい方が少ない配列に格納されています（マウス右より左、マウス左よりBキー、BキーよりAキーが優先）。
        /// ***_DOUBLE, ***TRIPLE, ***LONGPRESSのキーコードは格納しません。
        /// </summary>
        private int[] p_NextToPressKeys;
        /// <summary>
        /// 次のフレームで押す予定のキー（マウスボタンを含む）の同時押し数です。p_nextPulledKeyのキー格納数として使います。
        /// </summary>
        private int p_NextToPressKeysNum = 0;
        /// <summary>
        /// １フレーム毎のCheckKeyInWindows()の中で、0やマイナスの値を設定しても、一つだけ（p_lastPulledKeys[0]）は必ず格納します。
        /// </summary>
        private int p_MultiPressedKeysNumMax・１フレーム最大キー認識数 = 3;

        /// <summary>
        /// 前回の入力状態と今回の入力状態。p_keyIsDown[1-p_flip][(int)EKeyCode.キー]が前回のキー押し中か、p_keyIsDown[p_flip][...]が今回のキー押し中かを示す。
        /// ***_DOUBLE, ***TRIPLE, ***LONGPRESSのキーコードも格納します。
        /// </summary>
        private bool[][] p_keyIsDown;
        /// <summary>
        /// 前回の入力状態と今回の入力状態をflip（逆転）させて使う。p_flip={0,1}
        /// 
        /// p_keyIsDown[1-p_flip][(int)EKeyCode.キー]が前回のキー押し中か、p_keyIsDown[p_flip][...]が今回のキー押し中か、
        /// </summary>
        private int p_flip = 0;


        
        
        /// <summary>
        /// Update()メソッドが１フレーム毎に定期的に呼び出す、
        /// APIのGetAsycKeyStateのキーVirtualKeyStates.VK_***を使って、このクラスで認識可能なキーコードEKeyCodeに置き換える処理です。
        /// 
        /// 　　■※このクラスで認識可能なキーを増やしたい場合、ここを変更してください。
        /// </summary>
		private void CheckKeyInWindows・このフレームでこのクラスで認識可能なキー押し離し状態を格納()
		{
            // 現在押されているキーの初期化
            p_nowPulledKeysNum = 0;
            for (int i = 0; i < p_nowPulledKeys.Length; i++)
            {
                p_nowPulledKeys[i] = (int)EKeyCode._NONE;
            }

            // フレームのキー押し情報の初期化
            p_isKeyPulled・今回のフレームで一つでもキーが押し離されたか = false;
            // 一つ一つ、このクラスで認識可能なキーが押されたかを調べる
			for (int i = 0; i < 10; ++i)
			{
				//--  0x30～0x39  メインキーボード 0～9  
				//p_keyIsDown[p_flip][(int)EKeyCode.KEY0 + i]
				//	= (GetAsyncKeyState(0x30 + i) & 0x8000) != 0;
                setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.KEY0 + i, 0x30 + i);

				// -- NumPadの0～9
				// VK_NUMPAD0==0x60
				setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick1_TenKeyPad0 + i, 0x60 + i);
			}
			for (int i = 0; i < 26; ++i)
			{
				//--  0x41～0x5A  文字キー A から Z  
				setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.a + i, 0x41 + i);
			}
			// 全部のキーをサポートしても（処理が重くなるだけで）仕方がないので、あとは主要キーのみにする
            // MOUSE_***
            // UP,DOWN,RIGHT,LEFT,RStick6_TenKeyPad_PLUS,RStick9_TenKeyPad_MINUS,RStick8_TenKeyPad_MULTIPLY,RStick7_TenKeyPad_DIVIDE,
            // SPACE,ENTER,TAB,ALT,LSHIFT,RSHIFT,LCTRL,RCTRL,ESCAPE,
            // BACKSPACE,CAPSLOCK

			// カーソル関連とNumPadの+,-,*,/
			setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.UP, (int)WindowsVirtualKeyStates.VK_UP);
			setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.DOWN, (int)WindowsVirtualKeyStates.VK_DOWN);
			setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RIGHT, (int)WindowsVirtualKeyStates.VK_RIGHT);
			setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.LEFT, (int)WindowsVirtualKeyStates.VK_LEFT);
			setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick6_TenKeyPad_PLUS, (int)WindowsVirtualKeyStates.VK_ADD);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick7_TenKeyPad_DIVIDE, (int)WindowsVirtualKeyStates.VK_DIVIDE);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick8_TenKeyPad_MULTIPLY, (int)WindowsVirtualKeyStates.VK_MULTIPLY);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick9_TenKeyPad_MINUS, (int)WindowsVirtualKeyStates.VK_SUBTRACT);//VK_ADD～SUBTRACTはテンキーの+～-

            // あとはspace,return(enterといっしょ),tab,alt,shift,ctrl,escぐらいでいいか。
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.SPACE, (int)WindowsVirtualKeyStates.VK_SPACE);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.ENTER, (int)WindowsVirtualKeyStates.VK_RETURN);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.TAB, (int)WindowsVirtualKeyStates.VK_TAB);
            // merusaia修正。WindowsVirtualKeyStates.VK_LWINはウィンドウズキー。ALTはVirtualKeyStates.VK_MENUのはず。なのでLRを区別できないはず。
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.ALT, (int)WindowsVirtualKeyStates.VK_MENU);
            //setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.KeyCode308_LALT, (int)WindowsVirtualKeyStates.VK_LWIN);
            //setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RALT, (int)WindowsVirtualKeyStates.VK_RWIN);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.LSHIFT, (int)WindowsVirtualKeyStates.VK_LSHIFT);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RSHIFT, (int)WindowsVirtualKeyStates.VK_RSHIFT);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.LCTRL, (int)WindowsVirtualKeyStates.VK_LCONTROL);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RCTRL, (int)WindowsVirtualKeyStates.VK_RCONTROL);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.ESCAPE, (int)WindowsVirtualKeyStates.VK_ESCAPE);

            // merusaiaが追加（EKeyCode.RStick1_TenKeyPad0とRStick6-9はテンキーで既に登録されているから、それ以外のRStick2-5と、PSButtonキーを追加）
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick2_TenKeyPad_PERIOD, (int)WindowsVirtualKeyStates.VK_DECIMAL);//VK_DECIMALはテンキーのピリオド。VK_OEM_PERIODが普通のピリオド。
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick3_DELETE, (int)WindowsVirtualKeyStates.VK_DELETE);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick4_HOME, (int)WindowsVirtualKeyStates.VK_HOME);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick5_R3Button_PAGEDOWN, (int)WindowsVirtualKeyStates.VK_NEXT);//VK_NEXTはPgDnキー
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.PSButton_PAGEUP, (int)WindowsVirtualKeyStates.VK_PRIOR);//VK_PRIORはPgUpキー
            // merusaiaが追加。Backspace,Capslock,(DeleteはRStick3だからもう既に入れた),ESCもいれて
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.BACKSPACE, (int)WindowsVirtualKeyStates.VK_BACK);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.CAPSLOCK, (int)WindowsVirtualKeyStates.VK_OEM_ATTN);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.RStick3_DELETE, (int)WindowsVirtualKeyStates.VK_DELETE);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.ESCAPE, (int)WindowsVirtualKeyStates.VK_ESCAPE);
            // merusaiaが追加。Webアプリでいるかもだから、ブラウザ関係キーもいれて
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.BROWSER_BACK, (int)WindowsVirtualKeyStates.VK_BROWSER_BACK);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.BROWSER_FORWARD, (int)WindowsVirtualKeyStates.VK_BROWSER_FORWARD);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.BROWSER_REFRESH, (int)WindowsVirtualKeyStates.VK_BROWSER_REFRESH);

            // ●merusaiaが追加。せっかく取れるんだから、マウスボタンもいれて
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.MOUSE_LEFT, (int)WindowsVirtualKeyStates.VK_LBUTTON);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.MOUSE_RIGHT, (int)WindowsVirtualKeyStates.VK_RBUTTON);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.MOUSE_MIDDLE, (int)WindowsVirtualKeyStates.VK_MBUTTON);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.MOUSE_X1, (int)WindowsVirtualKeyStates.VK_XBUTTON1);
            setKeyState・現在のキー押し離し状態を設定((int)EKeyCode.MOUSE_X2, (int)WindowsVirtualKeyStates.VK_XBUTTON2);

            // ■ダブルトリプルイベントの初期化
            if (p_isCanReconizeDoubleTriplePull・ダブルトリプル連打を判定するか == true)
            {
                // マウスと一部キーだけ、EKeyCodeイベントを作っちゃったので一応登録しとこうか。
                //   …う～ん…キーボードキーと統一させるなら、
                //   …ちゃんと別のメソッドで判定した方がいいかもしれない。
                // 　　　…よし、p_keyDoublePressed[][]を作っちゃおうか。。
                //　 　　　…いや、そうするとTriple用も作るとまたメモリ増えるし、しかもダブル連打などをボタン管理者がIsPush()で管理できなくなる。
                // →　やっぱり、マウスや限られたキーだけダブル・トリプル連打を採用するようにしよう。

                // ■初期化は、変える配列は今回のフレームで、p_keyIsDown[p_flip][EKeyCode_toInt] = falseだよ。
                // ■気を付けて！押し離し(Pull=CLICK）をtrueにする、すなわちPull=trueをセットする時は、今回のフレームはそのままでＯＫ、変える配列は前回のフレームのp_keyIsDown[1-p_flip][EKeyCode_toInt] = trueだよ。
                bool _isDown = false;
                // ●ダブル連打対応キー一覧
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_LEFT_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_RIGHT_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_MIDDLE_DOUBLECLICK] = _isDown;
                // 　　　→　ダブル連打を認識したいEKeyCodeを増やしたら、
                //              ここに処理を増やしてね。（←この文を検索して全ての処理に追加。ダブル連打・トリプル連打・長押しでそれぞれ初期化と設定の２か所ずつ、計６か所を変更）
                // case (int)EKeyCode.連打採用キー: ...
                // MOUSE_***
                // UP,DOWN,RIGHT,LEFT,RStick6_TenKeyPad_PLUS,RStick9_TenKeyPad_MINUS,RStick8_TenKeyPad_MULTIPLY,RStick7_TenKeyPad_DIVIDE,
                // SPACE,ENTER,TAB,ALT,LSHIFT,RSHIFT,LCTRL,RCTRL,ESCAPE,
                // BACKSPACE,CAPSLOCK
                p_keyIsDown[p_flip][(int)EKeyCode.UP_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.DOWN_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.RIGHT_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LEFT_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.KEY1_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.KEY2_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.DELETE_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.PSButton_PgUp_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.SPACE_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ENTER_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.TAB_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ALT_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LSHIFT_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.RSHIFT_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LCTRL_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LCTRL_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ESCAPE_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.BACKSPACE_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.CAPSLOCK_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.z_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.x_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.c_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.v_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.a_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.s_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.q_DOUBLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.w_DOUBLECLICK] = _isDown;


                // ●トリプル連打対応キー一覧
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_LEFT_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_RIGHT_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_MIDDLE_TRIPLECLICK] = _isDown;
                // 　　　→　トリプル連打を認識したいEKeyCodeを増やしたら、
                //              ここに処理を増やしてね。（←この文を検索して全ての処理に追加。ダブル連打・トリプル連打・長押しでそれぞれ初期化と設定の２か所ずつ、計６か所を変更）
                // case (int)EKeyCode.連打採用キー: ...
                // MOUSE_***
                // UP,DOWN,RIGHT,LEFT,RStick6_TenKeyPad_PLUS,RStick9_TenKeyPad_MINUS,RStick8_TenKeyPad_MULTIPLY,RStick7_TenKeyPad_DIVIDE,
                // SPACE,ENTER,TAB,ALT,LSHIFT,RSHIFT,LCTRL,RCTRL,ESCAPE,
                // BACKSPACE,CAPSLOCK
                p_keyIsDown[p_flip][(int)EKeyCode.UP_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.DOWN_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.RIGHT_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LEFT_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.KEY1_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.KEY2_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.DELETE_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.PSButton_PgUp_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.SPACE_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ENTER_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.TAB_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ALT_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LSHIFT_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.RSHIFT_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LCTRL_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LCTRL_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ESCAPE_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.BACKSPACE_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.CAPSLOCK_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.z_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.x_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.c_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.v_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.a_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.s_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.q_TRIPLECLICK] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.w_TRIPLECLICK] = _isDown;

                // ●長押し対応キー一覧
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_LEFT_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_RIGHT_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.MOUSE_MIDDLE_PRESSLONG] = _isDown;
                // 　　　→　トリプル連打を認識したいEKeyCodeを増やしたら、
                //              ここに処理を増やしてね。（←この文を検索して全ての処理に追加。ダブル連打・トリプル連打・長押しでそれぞれ初期化と設定の２か所ずつ、計６か所を変更）
                // case (int)EKeyCode.連打採用キー: ...
                // MOUSE_***
                // UP,DOWN,RIGHT,LEFT,RStick6_TenKeyPad_PLUS,RStick9_TenKeyPad_MINUS,RStick8_TenKeyPad_MULTIPLY,RStick7_TenKeyPad_DIVIDE,
                // SPACE,ENTER,TAB,ALT,LSHIFT,RSHIFT,LCTRL,RCTRL,ESCAPE,
                // BACKSPACE,CAPSLOCK
                p_keyIsDown[p_flip][(int)EKeyCode.UP_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.DOWN_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.RIGHT_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LEFT_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.KEY1_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.KEY2_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.DELETE_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.PSButton_PgUp_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.SPACE_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ENTER_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.TAB_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ALT_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LSHIFT_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.RSHIFT_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LCTRL_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.LCTRL_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.ESCAPE_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.BACKSPACE_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.CAPSLOCK_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.z_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.x_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.c_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.v_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.a_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.s_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.q_PRESSLONG] = _isDown;
                p_keyIsDown[p_flip][(int)EKeyCode.w_PRESSLONG] = _isDown;

            }
            // ■1.キーコードの初期化、終わり

        }

        //■仮想キーコードの意味がわからないとき http://chokuto.ifdef.jp/urawaza/prm/virtual_key_code.html
        #region WindowsVirtualKeyStates -- Windowsの仮想キーコード表 
        private enum WindowsVirtualKeyStates : int
		{
			VK_LBUTTON = 0x01,
			VK_RBUTTON = 0x02,
			VK_CANCEL = 0x03,
			VK_MBUTTON = 0x04,
			//
			VK_XBUTTON1 = 0x05,
			VK_XBUTTON2 = 0x06,
			//
			VK_BACK = 0x08,
			VK_TAB = 0x09,
			//
			VK_CLEAR = 0x0C,
			VK_RETURN = 0x0D,
			//
			VK_SHIFT = 0x10,
			VK_CONTROL = 0x11,
			VK_MENU = 0x12,
			VK_PAUSE = 0x13,
			VK_CAPITAL = 0x14,
			//
			VK_KANA = 0x15,
			VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
			VK_HANGUL = 0x15,
			VK_JUNJA = 0x17,
			VK_FINAL = 0x18,
			VK_HANJA = 0x19,
			VK_KANJI = 0x19,
			//
			VK_ESCAPE = 0x1B,
			//
			VK_CONVERT = 0x1C,
			VK_NONCONVERT = 0x1D,
			VK_ACCEPT = 0x1E,
			VK_MODECHANGE = 0x1F,
			//
			VK_SPACE = 0x20,
			VK_PRIOR = 0x21,
			VK_NEXT = 0x22,
			VK_END = 0x23,
			VK_HOME = 0x24,
			VK_LEFT = 0x25,
			VK_UP = 0x26,
			VK_RIGHT = 0x27,
			VK_DOWN = 0x28,
			VK_SELECT = 0x29,
			VK_PRINT = 0x2A,
			VK_EXECUTE = 0x2B,
			VK_SNAPSHOT = 0x2C,
			VK_INSERT = 0x2D,
			VK_DELETE = 0x2E,
			VK_HELP = 0x2F,
			//
			VK_LWIN = 0x5B,
			VK_RWIN = 0x5C,
			VK_APPS = 0x5D,
			//
			VK_SLEEP = 0x5F,
			//
			VK_NUMPAD0 = 0x60,
			VK_NUMPAD1 = 0x61,
			VK_NUMPAD2 = 0x62,
			VK_NUMPAD3 = 0x63,
			VK_NUMPAD4 = 0x64,
			VK_NUMPAD5 = 0x65,
			VK_NUMPAD6 = 0x66,
			VK_NUMPAD7 = 0x67,
			VK_NUMPAD8 = 0x68,
			VK_NUMPAD9 = 0x69,
			VK_MULTIPLY = 0x6A,
			VK_ADD = 0x6B,
			VK_SEPARATOR = 0x6C,
			VK_SUBTRACT = 0x6D,
			VK_DECIMAL = 0x6E,
			VK_DIVIDE = 0x6F,
			VK_F1 = 0x70,
			VK_F2 = 0x71,
			VK_F3 = 0x72,
			VK_F4 = 0x73,
			VK_F5 = 0x74,
			VK_F6 = 0x75,
			VK_F7 = 0x76,
			VK_F8 = 0x77,
			VK_F9 = 0x78,
			VK_F10 = 0x79,
			VK_F11 = 0x7A,
			VK_F12 = 0x7B,
			VK_F13 = 0x7C,
			VK_F14 = 0x7D,
			VK_F15 = 0x7E,
			VK_F16 = 0x7F,
			VK_F17 = 0x80,
			VK_F18 = 0x81,
			VK_F19 = 0x82,
			VK_F20 = 0x83,
			VK_F21 = 0x84,
			VK_F22 = 0x85,
			VK_F23 = 0x86,
			VK_F24 = 0x87,
			//
			VK_NUMLOCK = 0x90,
			VK_SCROLL = 0x91,
			//
			VK_OEM_NEC_EQUAL = 0x92,   // '=' _keycode on numpad
			//
			VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' _keycode
			VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' _keycode
			VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' _keycode
			VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' _keycode
			VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' _keycode
			//
			VK_LSHIFT = 0xA0,
			VK_RSHIFT = 0xA1,
			VK_LCONTROL = 0xA2,
			VK_RCONTROL = 0xA3,
			VK_LMENU = 0xA4,
			VK_RMENU = 0xA5,
			//
			VK_BROWSER_BACK = 0xA6,
			VK_BROWSER_FORWARD = 0xA7,
			VK_BROWSER_REFRESH = 0xA8,
			VK_BROWSER_STOP = 0xA9,
			VK_BROWSER_SEARCH = 0xAA,
			VK_BROWSER_FAVORITES = 0xAB,
			VK_BROWSER_HOME = 0xAC,
			//
			VK_VOLUME_MUTE = 0xAD,
			VK_VOLUME_DOWN = 0xAE,
			VK_VOLUME_UP = 0xAF,
			VK_MEDIA_NEXT_TRACK = 0xB0,
			VK_MEDIA_PREV_TRACK = 0xB1,
			VK_MEDIA_STOP = 0xB2,
			VK_MEDIA_PLAY_PAUSE = 0xB3,
			VK_LAUNCH_MAIL = 0xB4,
			VK_LAUNCH_MEDIA_SELECT = 0xB5,
			VK_LAUNCH_APP1 = 0xB6,
			VK_LAUNCH_APP2 = 0xB7,
			//
			VK_OEM_1 = 0xBA,   // ';:' for US
			VK_OEM_PLUS = 0xBB,   // '+' any country
			VK_OEM_COMMA = 0xBC,   // ',' any country
			VK_OEM_MINUS = 0xBD,   // '-' any country
			VK_OEM_PERIOD = 0xBE,   // '.' any country
			VK_OEM_2 = 0xBF,   // '/?' for US
			VK_OEM_3 = 0xC0,   // '`~' for US
			//
			VK_OEM_4 = 0xDB,  //  '[{' for US
			VK_OEM_5 = 0xDC,  //  '\|' for US
			VK_OEM_6 = 0xDD,  //  ']}' for US
			VK_OEM_7 = 0xDE,  //  ''"' for US
			VK_OEM_8 = 0xDF,
			//
			VK_OEM_AX = 0xE1,  //  'AX' _keycode on Japanese AX kbd
			VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-_keycode kbd.
			VK_ICO_HELP = 0xE3,  //  Help _keycode on ICO
			VK_ICO_00 = 0xE4,  //  00 _keycode on ICO
			//
			VK_PROCESSKEY = 0xE5,
			//
			VK_ICO_CLEAR = 0xE6,
			//
			VK_PACKET = 0xE7,
			//
			VK_OEM_RESET = 0xE9,
			VK_OEM_JUMP = 0xEA,
			VK_OEM_PA1 = 0xEB,
			VK_OEM_PA2 = 0xEC,
			VK_OEM_PA3 = 0xED,
			VK_OEM_WSCTRL = 0xEE,
			VK_OEM_CUSEL = 0xEF,
			VK_OEM_ATTN = 0xF0,
			VK_OEM_FINISH = 0xF1,
			VK_OEM_COPY = 0xF2,
			VK_OEM_AUTO = 0xF3,
			VK_OEM_ENLW = 0xF4,
			VK_OEM_BACKTAB = 0xF5,
			//
			VK_ATTN = 0xF6,
			VK_CRSEL = 0xF7,
			VK_EXSEL = 0xF8,
			VK_EREOF = 0xF9,
			VK_PLAY = 0xFA,
			VK_ZOOM = 0xFB,
			VK_NONAME = 0xFC,
			VK_PA1 = 0xFD,
			VK_OEM_CLEAR = 0xFE
		}
		#endregion
        




        #region 同時押しの判定処理
        #endregion






        #region ダブルクリック（キーダブル連打）やトリプルクリック（三連打）判定処理
        /// <summary>
        /// ダブル連打・トリプルを判定するか。判定したくない場合はfalseにすると、少し高速化します。
        /// </summary>
        public bool p_isCanReconizeDoubleTriplePull・ダブルトリプル連打を判定するか = true;

        public bool p_isDoublePulled・今回のフレームでダブル連打がされたか = false;
        public bool p_islastKeyDoublePulled・最後に押されたキーでダブル連打がされたか = false;
        public bool p_isTripplePulled・今回のフレームでトリプル連打がされたか = false;
        /// <summary>
        /// 最後の押し離したキー（キーコードはp_lastPulledKey）の前回の入力状態と今回の入力状態。
        /// p_lastKeyPulledTime[1-p_lastPulledFlip]が前回のキー押し離し時間、p_lastKeyPulledTime[p_lastPulledFlip]が今回のキー押し離し時間（MyTools.getNowTime_fast()）。
        /// 
        ///   ※連続押し・二連打（ダブルクリック）、三連打（トリプルクリック）判定のために使う。***_DOUBLE, ***TRIPLE, ***LONGPRESSの時刻は格納しません。
        /// 
        /// </summary>
        private int[] p_lastKeyPulledTime;
        /// <summary>
        /// 値を反転させて使います。キーがpushされるたびに更新されます。 {0,1}
        /// </summary>
        private int p_lastPulledFlip = 0;

        /// <summary>
        /// キーを何ミリ秒以内に２回押し続けたらダブル押しと判定するか。最大時間の閾値です。
        /// </summary>
        private static int s_keyDoublePressMSec・キー連続押し閾値 = 700;//これで取ると200ミリ秒と異常な値になる…そんなん常人には無理MyTools.getDoubleClickTime();

        /// <summary>
        /// マウスボタンを何ミリ秒以内に２回押し続けたらダブルクリックと判定するか。最大時間の閾値です。
        /// </summary>
        private static int s_mouseDoubleClickLeftMSec・左マウスダブルクリック閾値 = s_keyDoublePressMSec・キー連続押し閾値;
        private static int s_mouseDoubleClickRightMSec・右マウスダブルクリック閾値 = s_keyDoublePressMSec・キー連続押し閾値;
        private static int s_mouseDoubleClickMiddleMSec・中マウスホイールダブルクリック閾値 = s_keyDoublePressMSec・キー連続押し閾値;
        //private static int s_mouseTripleClickLeftMSec・左マウストリプルクリック閾値 = s_mouseDoubleClickLeftMSec・左マウスダブルクリック閾値*2;
        //private static int s_mouseTripleClickRightMSec・右マウストリプルクリック閾値 = s_mouseDoubleClickMiddleMSec・中マウスホイールダブルクリック閾値*2;
        //private static int s_mouseTripleClickMiddleMSec・中マウスホイールトリプルクリック閾値 = s_mouseDoubleClickMiddleMSec・中マウスホイールダブルクリック閾値*2;

        /// <summary>
        /// このフレーム中に押し離したキーを元に、そのキーのダブル連打、トリプル連打を判定します。
        /// 
        /// なお、返り値には、このフレーム中にダブルトリプル連打されたキーがあるかどうかを返します。
        /// </summary>
        /// <returns></returns>
        private bool checkDubbleTriplePress・ダブルトリプル連打判定()
        {
            bool _isTestShowDebug = false; // デバッグテスト: ダブルトリプル連打の失敗・成功、押し間隔と閾値を出力。                            

            bool _isExitDoubleOrTriplePulledKey_NowFrame = false;
            if (p_isCanReconizeDoubleTriplePull・ダブルトリプル連打を判定するか == false)
            {
                // ダブルトリプル連打判定をしない場合は、このメソッドは何もせずすぐ抜ける。
                // ダブル連打を初期化しておく
                p_islastKeyDoublePulled・最後に押されたキーでダブル連打がされたか = false;
                return _isExitDoubleOrTriplePulledKey_NowFrame;
            }
            // 今回のフレームのダブル押し・トリプル押しの初期化
            p_isDoublePulled・今回のフレームでダブル連打がされたか = false;
            p_isTripplePulled・今回のフレームでトリプル連打がされたか = false;


            // （前処理による高速化）0.まず、最後に押したキーの時刻を取ってきて、そのタイムスパンが閾値以上なら、キーがなんであろうが、判定はfalseである。
            int _nowTime = MyTools.getNowTime_fast(); //これはまだ更新されてないから違うgetNT・今回のキー押し離し時刻();
            int _beforePressedTime = getBT・前回のキー押し離し時刻();
            int _t・閾値 = getDoublePulled_TimeSpanMaxMSec・ダブル連打の閾値を取得();
            int _timeSpan = Math.Abs(_nowTime - _beforePressedTime);
            if (_timeSpan > _t・閾値)
            {
                // そのタイムスパンが閾値以上なら、キーがなんであろうが、判定はfalseである。
                // ダブル連打を初期化しておく
                p_islastKeyDoublePulled・最後に押されたキーでダブル連打がされたか = false;
                return _isExitDoubleOrTriplePulledKey_NowFrame;
            }

            // 1.今回のフレームで押し離したキー（ここではマウスのボタンも含む）が見つかったかどうかを調べる。見つからなければダブル押しは判定できないのでfalse
            int[] _Keys_nowFrame = getNowKeys・今回押し離したキー配列を取得();
            if (_Keys_nowFrame[0] == (int)EKeyCode._NONE) return _isExitDoubleOrTriplePulledKey_NowFrame;
            // 1.1.前回のキーがない（今回のキーが初めて押し離したキーの）場合は、ダブル押しは判定できないのでfalse
            int[] _Keys_beforeFrame = getLastKeys・前回押し離したキー配列を取得();
            if (_Keys_beforeFrame[0] == (int)EKeyCode._NONE) return _isExitDoubleOrTriplePulledKey_NowFrame;
            // 1.2.今回押し離したキーと、前回押し離したキーに、同じキーのペアが存在するか調べる。
            bool _ExitsEqualKey_InBeforeAndFirst = false;
            int _nowKey = (int)EKeyCode._NONE; // これが判定するキー
            foreach (int _key in _Keys_nowFrame)
            {
                if (_key == (int)EKeyCode._NONE) continue; // NONEなら、次のキーへ
                foreach (int _lastKey in _Keys_beforeFrame)
                {
                    if (_key == _lastKey)
                    {
                        _ExitsEqualKey_InBeforeAndFirst = true;
                        _nowKey = _key; // これが判定するキー
                        // デバッグ用
                        if(_isTestShowDebug == true) MyTools.ConsoleWriteLine("【デバッグ】同じ " + MyTools.getEnumName<EKeyCode>(_nowKey) + " キーを押した");

                        break; // 一個でも見つかったら即ループを抜ける（複数キーの同時ダブルトリプル押しは判定しない）
                    }
                }
                if (_nowKey != (int)EKeyCode._NONE) break;
            }
            if (_ExitsEqualKey_InBeforeAndFirst == false)
            {
                // ペアが存在しないなら、判定はしない
                // 前の更新時間を消去（同じキーじゃないから）
                // ※消さないと、間に他のキーが入った時、そのキーとのタイムスパンを取ってしまうことになる。今のダブルトリプル判定は、間に別のキーだけが入ってたら、ダブルは無効とみなして判定しない）
                //p_lastKeyPulledTime[1 - p_lastPulledFlip] = 0;
                //p_lastKeyPulledTime[p_lastPulledFlip] = 0; // これもトリプル処理にしか使われないので消しておく（今回の時刻はこのメソッド後にちゃんと新しく格納される）

                // ダブル連打を初期化しておく
                p_islastKeyDoublePulled・最後に押されたキーでダブル連打がされたか = false;
                return _isExitDoubleOrTriplePulledKey_NowFrame;
            }
            else
            {
                // 2.ぺアが見つかれば、そのキーのダブル押しをタイムスパンと閾値で判定

                // この場でのp_lastKeyPulledTime更新時間と          ：nowTimeと ：フリップの状態（もちろん0/1は逆の時もある）
                // (a)同じキーのPullが1回目：　[前回]無し[今回]無し ：初Ａ　    ：フリップ=0　→　ダブル連打判定不可
                // (b)同じキーのPullが2回目：　[前回]初Ａ[今回]無し ：次Ｂ　    ：フリップ=1　→　（時刻(a)-(b)中にダブル連打がfalseの時）:（次Ｂ―初Ａ）＜閾値で、ダブル連打が判定可能
                // (c)同じキーのPullが3回目：　[前回]次Ｂ[今回]初Ａ ：三Ｃ　    ：フリップ=0　→　（時刻(a)-(c)中にダブル連打がfalseの時）:（三Ｃ―次Ｂ）＜閾値で、ダブル連打が判定可能、（trueの時）：（三Ｃ―初Ａ）＜閾値で、トリプル連打が判定可能
                // (d)同じキーのPullが4回目以上[前回]有り[今回]有り ：○Ｄ      ：フリップ=0/1→　2回の時と同様
                // 
                // ※配列は、[前回]=[1-フリップ]、[今回]=[フリップ]、で取得できる

                int _beforeTime = getBT・前回のキー押し離し時刻();
                // 前回の更新時間が記録されているか（同じキーのPushが2回目以上）
                if (_beforeTime != 0)
                {
                    // ■時刻(a)-(c)中にダブル連打がfalseの時
                    if (p_islastKeyDoublePulled・最後に押されたキーでダブル連打がされたか == false)
                    {
                        // falseだったら、ダブル連打を判定する

                        // タイムスパンを再計算
                        _timeSpan = Math.Abs(_nowTime - _beforeTime);
                        // (b)(c)(d)ダブル連打を判定
                        if (_timeSpan <= _t・閾値)
                        {
                            _isExitDoubleOrTriplePulledKey_NowFrame = true;
                            p_isDoublePulled・今回のフレームでダブル連打がされたか = true;
                            // ダブル連打をＯＮ状態にして、次からのフレームはトリプル連打に移行する
                            p_islastKeyDoublePulled・最後に押されたキーでダブル連打がされたか = true;
                            // 今回のフレームのダブル押しのフラグをON
                            setDoublePulled(_nowKey, true);
                            // これが呼び出されないキーも、既に初期化処理でfalseが初期値として代入されているので心配ない

                            // デバッグ用
                            if (_isTestShowDebug == true)
                            {
                                // ※マウスでもキーボードでも、普通の人はがんばっても毎秒5連打(200～300ミリ秒)位だよ。やっぱ高橋名人の16連打ってめっちゃすごい…
                                string _kigou = (_timeSpan <= _t・閾値) ? "<=" : ">";
                                MyTools.ConsoleWriteLine("【ダブル連打成功！デバッグ】同じ " + MyTools.getEnumName<EKeyCode>(_nowKey) + " キーを押した間隔ミリ秒: " + _timeSpan + " " + _kigou + " 閾値" + _t・閾値 + "");
                            }
                        }
                        else
                        {
                            // デバッグ用
                            if (_isTestShowDebug == true)
                            {
                                // ※マウスでもキーボードでも、普通の人はがんばっても毎秒5連打(200～300ミリ秒)位だよ。やっぱ高橋名人の16連打ってめっちゃすごい…
                                string _kigou = (_timeSpan <= _t・閾値) ? "<=" : ">";
                                MyTools.ConsoleWriteLine("【ダブル連打届かず…デバッグ】同じ " + MyTools.getEnumName<EKeyCode>(_nowKey) + " キーを押した間隔ミリ秒: " + _timeSpan + " " + _kigou + " 閾値" + _t・閾値 + "");
                            }
                        }
                    }
                    else
                    {
                        // ■時刻(a)-(c)中にダブル連打がtrueの時
                        // trueだったら、既にダブル連打をした後の処理として、今回はトリプル連打を判定する

                        // 今回（２つ前）の更新時間が記録されているか（同じキーのPushが3回目以上）
                        // （今回といっても、_nowTimeはまだ記録されてないので、２回前のが残ってる。）
                        int _beforeBeforeTime = getNT・今回のキー押し離し時刻();
                        if (_beforeBeforeTime != 0)
                        {
                            // トリプル連打を判定するときのタイムスパンは、ダブルの時とは異なる
                            _timeSpan = Math.Abs(_nowTime - _beforeBeforeTime);
                            // [Q]トリプルの閾値は一緒じゃないと、ダブルの次にトリプル判定が来てしまう。それでいいのか？
                            _t・閾値 = _t・閾値 * 1;
                            if (_timeSpan <= _t・閾値)
                            {
                                _isExitDoubleOrTriplePulledKey_NowFrame = true;
                                p_isTripplePulled・今回のフレームでトリプル連打がされたか = true;
                                // 今回のフレームのトリプル押しのフラグをON
                                setTriplePulled(_nowKey, true);
                                // これが呼び出されないキーも、既に初期化処理でfalseが初期値として代入されているので心配ない
                                // ダブル連打を初期化しておく
                                p_islastKeyDoublePulled・最後に押されたキーでダブル連打がされたか = false;

                                // デバッグ用
                                if (_isTestShowDebug == true)
                                {
                                    // ※マウスでもキーボードでも、普通の人はがんばっても毎秒5連打(200～300ミリ秒)位だよ。やっぱ高橋名人の16連打ってめっちゃすごい…
                                    string _kigou = (_timeSpan <= _t・閾値) ? "<=" : ">";
                                    MyTools.ConsoleWriteLine("【トリプル連打成功！デバッグ】同じ " + MyTools.getEnumName<EKeyCode>(_nowKey) + " キーを押した間隔ミリ秒: " + _timeSpan + " " + _kigou + " 閾値" + _t・閾値 + "");
                                }
                            }
                            else
                            {
                                // トリプルへの道は終わった…
                                // ダブル連打を初期化しておく
                                p_islastKeyDoublePulled・最後に押されたキーでダブル連打がされたか = false;

                                // デバッグ用
                                if (_isTestShowDebug == true)
                                {
                                    // ※マウスでもキーボードでも、普通の人はがんばっても毎秒5連打(200～300ミリ秒)位だよ。やっぱ高橋名人の16連打ってめっちゃすごい…
                                    string _kigou = (_timeSpan <= _t・閾値) ? "<=" : ">";
                                    MyTools.ConsoleWriteLine("【トリプル連打届かず…デバッグ】同じ " + MyTools.getEnumName<EKeyCode>(_nowKey) + " キーを押した間隔ミリ秒: " + _timeSpan + " " + _kigou + " 閾値" + _t・閾値 + "");
                                }
                            }
                        }
                        else
                        {
                            // (b)ダブル判定trueだけど、トリプルは無理
                        }

                    }
                }
                else
                {
                    //(a)前の押し離し時刻が0。まだシングル押し離し。
                }
            } // 判定がtrueでもfalseでも、チェックが終了
            return _isExitDoubleOrTriplePulledKey_NowFrame;
        }
        /// <summary>
        /// 今回のフレームで押し離したキーを、EKeyCode型の(int)でパースした値で取って来ます。
        /// 
        ///  ※同フレームに２つ以上のキーが押された（完全同期の同時押しの）場合は、
        /// キーコードの値が小さい方が優先されます。（マウス右より左、マウス左よりBキー、BキーよりAキーが優先）。
        /// </summary>
        /// <returns></returns>
        public int getNowKey・今回押し離したキー()
        {
            int _EKeyCode_ToInt = (int)EKeyCode._NONE;
            if (p_nowPulledKeys != null && p_nowPulledKeysNum > 0)
            {
                _EKeyCode_ToInt = p_nowPulledKeys[0];
            }
            return _EKeyCode_ToInt;
        }
        /// <summary>
        /// 最後に押し離したキーの配列を、EKeyCode型の(int)でパースした値で取って来ます。
        /// キーが無い場合は、[0]=(int)KeyCode.NONEである配列を返します。
        /// 
        ///  ※同フレームに２つ以上のキーが押された（完全同期の同時押しの）場合は、
        /// キーコードの値が小さい方が優先されます。（マウス右より左、マウス左よりBキー、BキーよりAキーが優先）。
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] getLastKeys・前回押し離したキー配列を取得()
        {
            return p_lastPulledKeys;
        }
        /// <summary>
        /// 今回のフレームで押し離したキーの配列を、EKeyCode型の(int)でパースした値で取って来ます。
        /// キーが無い場合は、[0]=(int)KeyCode.NONEである配列を返します。
        /// 
        ///  ※同フレームに２つ以上のキーが押された（完全同期の同時押しの）場合は、
        /// キーコードの値が小さい方が優先されます。（マウス右より左、マウス左よりBキー、BキーよりAキーが優先）。
        /// </summary>
        /// <returns></returns>
        public int[] getNowKeys・今回押し離したキー配列を取得()
        {
            return p_nowPulledKeys;
        }
        /// <summary>
        /// 今回のキー押し離しした時刻を、MyTools.getNowTime_fast()で取得したint型の時刻で取得します。
        /// </summary>
        /// <returns></returns>
        public int getNT・今回のキー押し離し時刻()
        {
            if (p_lastKeyPulledTime == null) { return 0; }
            return p_lastKeyPulledTime[p_lastPulledFlip];
        }
        /// <summary>
        /// 前回のキー押し離しした時刻を、MyTools.getNowTime_fast()で取得したint型の時刻で取得します。
        /// </summary>
        /// <returns></returns>
        public int getBT・前回のキー押し離し時刻()
        {
            if (p_lastKeyPulledTime == null) { return 0; }
            return p_lastKeyPulledTime[1 - p_lastPulledFlip];
        }

        /// <summary>
        /// 全てのボタン・キーのダブルクリック（ダブル連打）の閾値の最大値を取って来ます。ダブルクリックだけなら500ミリ秒、トリプルクリックも判定したいなら700ミリ秒位が妥当です。
        /// 
        /// ※マウスでもキーボードでも、普通の人はがんばっても毎秒5連打(200～300ミリ秒)位だよ。やっぱ高橋名人の16連打ってめっちゃすごい…
        /// </summary>
        /// <param name="_EKeyCode_toInt"></param>
        /// <returns></returns>
        public int getDoublePulled_TimeSpanMaxMSec・ダブル連打の閾値を取得()
        {
            int _timespan = Math.Max(s_mouseDoubleClickLeftMSec・左マウスダブルクリック閾値,
                s_mouseDoubleClickRightMSec・右マウスダブルクリック閾値);
            _timespan = Math.Max(_timespan, s_mouseDoubleClickMiddleMSec・中マウスホイールダブルクリック閾値);
            _timespan = Math.Max(_timespan, s_keyDoublePressMSec・キー連続押し閾値);
            return _timespan;
        }
        /// <summary>
        /// 指定EKeyCodeのダブルクリック（ダブル連打）の閾値を取って来ます。0や-1などEKeyCodeに指定されていない引数をつけると、デフォルト値を返します。
        /// ダブルクリックだけなら500ミリ秒、トリプルクリックも判定したいなら700ミリ秒位が妥当なので、デフォルトは700です。
        /// 
        /// ※マウスでもキーボードでも、普通の人はがんばっても毎秒5連打(200～300ミリ秒)位だよ。やっぱ高橋名人の16連打ってめっちゃすごい…
        /// </summary>
        /// <param name="_EKeyCode_toInt"></param>
        /// <returns></returns>
        public int getDoublePulled_TimeSpanMaxMSec・ダブル連打の閾値を取得(int _EKeyCode_toInt){
            int _timespan = s_keyDoublePressMSec・キー連続押し閾値; // デフォルト
            if (_EKeyCode_toInt == (int)EKeyCode.MOUSE_LEFT)
            {
                _timespan = s_mouseDoubleClickLeftMSec・左マウスダブルクリック閾値;
            }else if (_EKeyCode_toInt == (int)EKeyCode.MOUSE_RIGHT)
            {
                _timespan = s_mouseDoubleClickRightMSec・右マウスダブルクリック閾値;
            }else if (_EKeyCode_toInt == (int)EKeyCode.MOUSE_MIDDLE)
            {
                _timespan = s_mouseDoubleClickMiddleMSec・中マウスホイールダブルクリック閾値;
            }
            return _timespan;
        }

        #endregion






        #region 長押しボタンの判定処理
        /// <summary>
        /// 現在まで押し続けているキーたちのEKeyCodeを格納しているリスト。１つも無ければ.Count=0になっている。
        /// ***_DOUBLE, ***TRIPLE, ***LONGPRESSのキーコードは格納しません。
        /// </summary>
        private List<int> p_pressingKey = new List<int>();
        /// <summary>
        /// 現在まで押し続けているキーたちの押し継続時間。p_pressingKeyと配列番号は共通。
        /// ***_DOUBLE, ***TRIPLE, ***LONGPRESSの時刻は格納しません。
        /// </summary>
        private List<int> p_pressingKeyTime = new List<int>();
        /// <summary>
        /// 押し続けるキーを格納するリストの最大値です。ここを少なくすると多少処理速度は速くなるかも…です。
        /// </summary>
        private static int s_pressingKeyMaxNum・押し続けているキーの最大格納数 = 5;


        /// <summary>
        /// シングル押しっぱなしや連打押しとは別に、長押しボタン（キーを一定時間押しっぱなししたらメニューを開くなど、「長押し操作」として別々に処理したい場合に使う）を判定するか。判定したくない場合はfalseにすると、少し高速化します。
        /// </summary>
        public bool p_isCanReconizeLongPressedButton・長押しボタンを判定するか = false;
        // ↑■■ここがfalseだと長押し判定しないが高速化できる
        /// <summary>
        /// キーを何ミリ秒押し続けたら長押しと判定するか。最小時間の閾値です。
        /// </summary>
        private static int s_keyLongPressMSec・キー長押し閾値 = 1000;
        /// <summary>
        /// マウスボタンを何ミリ秒押し続けたら長押しと判定するか。最小時間の閾値です。
        /// </summary>
        private static int s_mouseLongPressLeftMSec・左マウスボタン長押し閾値 = 1000;
        private static int s_mouseLongPressRightMSec・右マウスボタン長押し閾値 = 1000;
        private static int s_mouseLongPressMiddleMSec・中マウスホイールボタン長押し閾値 = 1000;
        private static int s_1frameMSec・長押し判定に使う１フレームミリ秒 = 20;

        public bool p_isLongPulled・今回のフレームで長押しボタンが判定されたか = false;
        /// <summary>
        /// このフレーム中に押し続けているキーを探して、
        /// そのキーの長押しボタンを
        /// （キーが一定時間押されていたら、
        /// 　その瞬間フレームだけ「EKeyCOde.***Key_LONGPRESSED」という
        /// 　別のキーコードとして）
        /// 判定します。
        /// 
        /// なお、返り値には、このフレーム中に現在押し続けているキーが見つかったかを返します。
        /// </summary>
        private bool checkLongPressedButton・長押しボタンを判定()
        {
            if (p_isCanReconizeLongPressedButton・長押しボタンを判定するか == false)
            {
                return false; // 見つからなかったと判定
            }
            p_isLongPulled・今回のフレームで長押しボタンが判定されたか = false;

            int _nowTime = MyTools.getNowTime_fast();
            int _nowKey = (int)EKeyCode._NONE;

            // 1.このフレームの瞬間に、押し続けているキーがあるか探す
            for (int i = 0; i < ButtonNum; i++)
            {
                // 高速化するなら、長押し対応キーじゃなかったらスキップ
                //if(isCanBeLongPressdButton・長押し対応キーか(i) == false){
                // // スキップ
                //}else{
                if (IsPush(i) == true)
                {
                    if (p_pressingKey.Count <= s_pressingKeyMaxNum・押し続けているキーの最大格納数)
                    {
                        if (p_pressingKey.Contains(i) == false)
                        {
                            // 押し続けているキーに追加
                            p_pressingKey.Add(i);
                            p_pressingKeyTime.Add(_nowTime);
                            _nowKey = i;
                        }
                    }
                    //break; // 高速化するなら、1個見つけたら終了。EKeyCodeが早い順に優先（マウスボタン系 < Shift系 < A系 なので、ほとんどの場合はかぶらないはず）
                }
                else
                {
                    // 削除
                    p_pressingKey.Remove(i);
                }
                //}

            }

            // 2.見つかれば、そのキーの長押しを判定
            // もっと高速化できると思うが…
            if (p_pressingKey.Count > 0)
            {
                for (int i = 0; i < p_pressingKey.Count; i++)
                {
                    int _timeSpan = _nowTime - p_pressingKeyTime[i];
                    int _t・閾値 = getLongPressed_TimeSpanMaxMSec・キー長押しの閾値を取得(p_pressingKey[i]);
                    if (_timeSpan > _t・閾値 + s_1frameMSec・長押し判定に使う１フレームミリ秒 && _timeSpan <= _t・閾値)
                    {
                        // 長押しボタンＯＮ
                        p_isLongPulled・今回のフレームで長押しボタンが判定されたか = true;
                        //break; // 高速化するなら、1個見つけたら終了。でも、Pushタイミングが全く同時だとまずい。EKeyCodeが早い順に優先（マウスボタン系 < Shift系 < A系 なので、ほとんどの場合はかぶらないはず）
                    }
                    setLongPressed(p_pressingKey[i], p_isLongPulled・今回のフレームで長押しボタンが判定されたか);
                }
            }
            bool _isExitLongPressedKey_NowFrame = false;
            if (p_pressingKey.Count > 0) _isExitLongPressedKey_NowFrame = true;
            return _isExitLongPressedKey_NowFrame;

        }

        /// <summary>
        /// 指定EKeyCodeの長押し時間の閾値を取って来ます。
        /// </summary>
        /// <param name="_EKeyCode_toInt"></param>
        /// <returns></returns>
        public int getLongPressed_TimeSpanMaxMSec・キー長押しの閾値を取得(int _EKeyCode_toInt)
        {
            int _timespan = s_keyLongPressMSec・キー長押し閾値; // デフォルト
            if (_EKeyCode_toInt == (int)EKeyCode.MOUSE_LEFT)
            {
                _timespan = s_mouseLongPressLeftMSec・左マウスボタン長押し閾値;
            }
            else if (_EKeyCode_toInt == (int)EKeyCode.MOUSE_RIGHT)
            {
                _timespan = s_mouseLongPressRightMSec・右マウスボタン長押し閾値;
            }
            else if (_EKeyCode_toInt == (int)EKeyCode.MOUSE_MIDDLE)
            {
                _timespan = s_mouseLongPressMiddleMSec・中マウスホイールボタン長押し閾値;
            }
            return _timespan;
        }

        #endregion



        /// <summary>
        /// 指定(int)EKeyCode.キーのダブル連打をＯＮ／ＯＦＦします。
        /// </summary>
        /// <param name="_EKeyCode_toInt"></param>
        public void setDoublePulled(int _EKeyCode_toInt, bool _isDoublePulled)
        {
            // マウスと一部キーだけ、EKeyCodeイベントを作っちゃったので一応登録しとこうか。
            //   …う～ん…キーボードキーと統一させるなら、
            //   …ちゃんと別のメソッドで判定した方がいいかもしれない。
            // 　　　…よし、p_keyDoublePressed[][]を作っちゃおうか。。
            //　 　　　…いや、そうするとTriple用も作るとまたメモリ増えるし、しかもダブル連打などをボタン管理者がIsPush()で管理できなくなる。
            // →　やっぱり、マウスや限られたキーだけダブル・トリプル連打を採用するようにしよう。

            // Pull=falseをセットするなら、変える配列は今回のフレームのp_keyIsDown[p_flip][EKeyCode_toInt] = falseだよ。
            if (_isDoublePulled == false) return; // falseならデフォルトで毎回p_keyIsDown[p_flip][EKeyCode_toInt] = falseをやっているので、何もしなくていい
            // ■気を付けて！Pull=trueをセットするなら、今回のフレームはそのままでＯＫ、変える配列は前回のフレームのp_keyIsDown[1-p_flip][EKeyCode_toInt] = trueだよ。
            switch (getNowKey・今回押し離したキー())
            {
                case (int)EKeyCode.MOUSE_LEFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_LEFT_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.MOUSE_RIGHT: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_RIGHT_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.MOUSE_MIDDLE: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_MIDDLE_DOUBLECLICK] = _isDoublePulled; break;
                // 　　　→　ダブル連打を認識したいEKeyCodeを増やしたら、
                //              ここに処理を増やしてね。（←この文を検索して全ての処理に追加。ダブル連打・トリプル連打・長押しでそれぞれ初期化と設定の２か所ずつ、計６か所を変更）
                // case (int)EKeyCode.連打採用キー: ...
                // MOUSE_***
                // UP,DOWN,RIGHT,LEFT,RStick6_TenKeyPad_PLUS,RStick9_TenKeyPad_MINUS,RStick8_TenKeyPad_MULTIPLY,RStick7_TenKeyPad_DIVIDE,
                // SPACE,ENTER,TAB,ALT,LSHIFT,RSHIFT,LCTRL,RCTRL,ESCAPE,
                // BACKSPACE,CAPSLOCK

                case (int)EKeyCode.UP: p_keyIsDown[1 - p_flip][(int)EKeyCode.UP_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.DOWN: p_keyIsDown[1 - p_flip][(int)EKeyCode.DOWN_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.RIGHT: p_keyIsDown[1 - p_flip][(int)EKeyCode.RIGHT_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.LEFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.LEFT_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.KEY1: p_keyIsDown[1 - p_flip][(int)EKeyCode.KEY1_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.KEY2: p_keyIsDown[1 - p_flip][(int)EKeyCode.KEY2_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.RStick3_DELETE: p_keyIsDown[1 - p_flip][(int)EKeyCode.DELETE_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.PSButton_PAGEUP: p_keyIsDown[1 - p_flip][(int)EKeyCode.PSButton_PgUp_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.SPACE: p_keyIsDown[1 - p_flip][(int)EKeyCode.SPACE_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.ENTER: p_keyIsDown[1 - p_flip][(int)EKeyCode.ENTER_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.TAB: p_keyIsDown[1 - p_flip][(int)EKeyCode.TAB_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.ALT: p_keyIsDown[1 - p_flip][(int)EKeyCode.ALT_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.LSHIFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.LSHIFT_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.RSHIFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.RSHIFT_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.LCTRL: p_keyIsDown[1 - p_flip][(int)EKeyCode.LCTRL_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.RCTRL: p_keyIsDown[1 - p_flip][(int)EKeyCode.LCTRL_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.ESCAPE: p_keyIsDown[1 - p_flip][(int)EKeyCode.ESCAPE_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.BACKSPACE: p_keyIsDown[1 - p_flip][(int)EKeyCode.BACKSPACE_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.CAPSLOCK: p_keyIsDown[1 - p_flip][(int)EKeyCode.CAPSLOCK_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.z: p_keyIsDown[1 - p_flip][(int)EKeyCode.z_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.x: p_keyIsDown[1 - p_flip][(int)EKeyCode.x_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.c: p_keyIsDown[1 - p_flip][(int)EKeyCode.c_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.v: p_keyIsDown[1 - p_flip][(int)EKeyCode.v_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.a: p_keyIsDown[1 - p_flip][(int)EKeyCode.a_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.s: p_keyIsDown[1 - p_flip][(int)EKeyCode.s_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.q: p_keyIsDown[1 - p_flip][(int)EKeyCode.q_DOUBLECLICK] = _isDoublePulled; break;
                case (int)EKeyCode.w: p_keyIsDown[1 - p_flip][(int)EKeyCode.w_DOUBLECLICK] = _isDoublePulled; break;



                default: break; // これはないはず
            }
        }
        /// <summary>
        /// 指定(int)EKeyCode.キーのトリプル連打をＯＮ／ＯＦＦします。
        /// </summary>
        /// <param name="_EKeyCode_toInt"></param>
        public void setTriplePulled(int _EKeyCode_toInt, bool _isTriplePulled)
        {
            switch (getNowKey・今回押し離したキー())
            {
                case (int)EKeyCode.MOUSE_LEFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_LEFT_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.MOUSE_RIGHT: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_RIGHT_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.MOUSE_MIDDLE: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_MIDDLE_TRIPLECLICK] = _isTriplePulled; break;
                // 　　　→　トリプル連打を認識したいEKeyCodeを増やしたら、
                //              ここに処理を増やしてね。（←この文を検索して全ての処理に追加。ダブル連打・トリプル連打・長押しでそれぞれ初期化と設定の２か所ずつ、計６か所を変更）
                // case (int)EKeyCode.連打採用キー: ...            
                // MOUSE_***
                // UP,DOWN,RIGHT,LEFT,RStick6_TenKeyPad_PLUS,RStick9_TenKeyPad_MINUS,RStick8_TenKeyPad_MULTIPLY,RStick7_TenKeyPad_DIVIDE,
                // SPACE,ENTER,TAB,ALT,LSHIFT,RSHIFT,LCTRL,RCTRL,ESCAPE,
                // BACKSPACE,CAPSLOCK

                case (int)EKeyCode.UP: p_keyIsDown[1 - p_flip][(int)EKeyCode.UP_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.DOWN: p_keyIsDown[1 - p_flip][(int)EKeyCode.DOWN_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.RIGHT: p_keyIsDown[1 - p_flip][(int)EKeyCode.RIGHT_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.LEFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.LEFT_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.KEY1: p_keyIsDown[1 - p_flip][(int)EKeyCode.KEY1_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.KEY2: p_keyIsDown[1 - p_flip][(int)EKeyCode.KEY2_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.RStick3_DELETE: p_keyIsDown[1 - p_flip][(int)EKeyCode.DELETE_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.PSButton_PAGEUP: p_keyIsDown[1 - p_flip][(int)EKeyCode.PSButton_PgUp_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.SPACE: p_keyIsDown[1 - p_flip][(int)EKeyCode.SPACE_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.ENTER: p_keyIsDown[1 - p_flip][(int)EKeyCode.ENTER_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.TAB: p_keyIsDown[1 - p_flip][(int)EKeyCode.TAB_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.ALT: p_keyIsDown[1 - p_flip][(int)EKeyCode.ALT_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.LSHIFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.LSHIFT_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.RSHIFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.RSHIFT_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.LCTRL: p_keyIsDown[1 - p_flip][(int)EKeyCode.LCTRL_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.RCTRL: p_keyIsDown[1 - p_flip][(int)EKeyCode.LCTRL_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.ESCAPE: p_keyIsDown[1 - p_flip][(int)EKeyCode.ESCAPE_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.BACKSPACE: p_keyIsDown[1 - p_flip][(int)EKeyCode.BACKSPACE_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.CAPSLOCK: p_keyIsDown[1 - p_flip][(int)EKeyCode.CAPSLOCK_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.z: p_keyIsDown[1 - p_flip][(int)EKeyCode.z_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.x: p_keyIsDown[1 - p_flip][(int)EKeyCode.x_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.c: p_keyIsDown[1 - p_flip][(int)EKeyCode.c_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.v: p_keyIsDown[1 - p_flip][(int)EKeyCode.v_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.a: p_keyIsDown[1 - p_flip][(int)EKeyCode.a_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.s: p_keyIsDown[1 - p_flip][(int)EKeyCode.s_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.q: p_keyIsDown[1 - p_flip][(int)EKeyCode.q_TRIPLECLICK] = _isTriplePulled; break;
                case (int)EKeyCode.w: p_keyIsDown[1 - p_flip][(int)EKeyCode.w_TRIPLECLICK] = _isTriplePulled; break;


                default: break; // これはないはず
            }
        }

        /// <summary>
        /// 指定キーの長押しボタンのＯＮ／ＯＦＦを設定します。
        /// </summary>
        /// <param name="_EKeyCode_toInt"></param>
        private void setLongPressed(int _EKeyCode_toInt, bool _isLongPressed)
        {
            switch (_EKeyCode_toInt)
            {
                case (int)EKeyCode.MOUSE_LEFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_LEFT_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.MOUSE_RIGHT: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_RIGHT_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.MOUSE_MIDDLE: p_keyIsDown[1 - p_flip][(int)EKeyCode.MOUSE_MIDDLE_PRESSLONG] = _isLongPressed; break;
                // 　　　→　ダブル連打を認識したいEKeyCodeを増やしたら、
                //              ここに処理を増やしてね。（←この文を検索して全ての処理に追加。ダブル連打・トリプル連打・長押しでそれぞれ初期化と設定の２か所ずつ、計６か所を変更）
                // case (int)EKeyCode.連打採用キー: ...
                // MOUSE_***
                // UP,DOWN,RIGHT,LEFT,RStick6_TenKeyPad_PLUS,RStick9_TenKeyPad_MINUS,RStick8_TenKeyPad_MULTIPLY,RStick7_TenKeyPad_DIVIDE,
                // SPACE,ENTER,TAB,ALT,LSHIFT,RSHIFT,LCTRL,RCTRL,ESCAPE,
                // BACKSPACE,CAPSLOCK
                case (int)EKeyCode.UP: p_keyIsDown[1 - p_flip][(int)EKeyCode.UP_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.DOWN: p_keyIsDown[1 - p_flip][(int)EKeyCode.DOWN_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.RIGHT: p_keyIsDown[1 - p_flip][(int)EKeyCode.RIGHT_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.LEFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.LEFT_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.KEY1: p_keyIsDown[1 - p_flip][(int)EKeyCode.KEY1_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.KEY2: p_keyIsDown[1 - p_flip][(int)EKeyCode.KEY2_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.RStick3_DELETE: p_keyIsDown[1 - p_flip][(int)EKeyCode.DELETE_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.PSButton_PAGEUP: p_keyIsDown[1 - p_flip][(int)EKeyCode.PSButton_PgUp_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.SPACE: p_keyIsDown[1 - p_flip][(int)EKeyCode.SPACE_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.ENTER: p_keyIsDown[1 - p_flip][(int)EKeyCode.ENTER_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.TAB: p_keyIsDown[1 - p_flip][(int)EKeyCode.TAB_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.ALT: p_keyIsDown[1 - p_flip][(int)EKeyCode.ALT_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.LSHIFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.LSHIFT_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.RSHIFT: p_keyIsDown[1 - p_flip][(int)EKeyCode.RSHIFT_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.LCTRL: p_keyIsDown[1 - p_flip][(int)EKeyCode.LCTRL_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.RCTRL: p_keyIsDown[1 - p_flip][(int)EKeyCode.LCTRL_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.ESCAPE: p_keyIsDown[1 - p_flip][(int)EKeyCode.ESCAPE_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.BACKSPACE: p_keyIsDown[1 - p_flip][(int)EKeyCode.BACKSPACE_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.CAPSLOCK: p_keyIsDown[1 - p_flip][(int)EKeyCode.CAPSLOCK_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.z: p_keyIsDown[1 - p_flip][(int)EKeyCode.z_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.x: p_keyIsDown[1 - p_flip][(int)EKeyCode.x_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.c: p_keyIsDown[1 - p_flip][(int)EKeyCode.c_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.v: p_keyIsDown[1 - p_flip][(int)EKeyCode.v_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.a: p_keyIsDown[1 - p_flip][(int)EKeyCode.a_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.s: p_keyIsDown[1 - p_flip][(int)EKeyCode.s_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.q: p_keyIsDown[1 - p_flip][(int)EKeyCode.q_PRESSLONG] = _isLongPressed; break;
                case (int)EKeyCode.w: p_keyIsDown[1 - p_flip][(int)EKeyCode.w_PRESSLONG] = _isLongPressed; break;



                default: break; // これはないはず
            }

        }

        #region Windows.Forms.KeysとEKeyCodeとの対応メソッド: getEKeyCode_***
        /// <summary>
        /// （※Windows専用）Window.Forms.Keysのキーに対応した、EKeyCodeを返します。主要キーのみしか対応していません。
        /// また、ダブルクリックなどには対応していません。
        /// 
        /// 【ゲームで認識可能な主要キー】矢印キー, Enter, Backspace, Shift, Ctrl, Tab, CapsLock, Space, Alt, Delete, Esc, KEY1, KEY2, ..., KEY9, KEY0, a, b, ..., zまで (記号はとりあえず無し)
        /// </summary>
        public static EKeyCode getEKeyCode_FromWindowsFormsKeys(System.Windows.Forms.Keys _keys)
        {

            // 見つからない場合は、わからないとする
            EKeyCode _Ekeycode = EKeyCode._NONE;
            // 全部調べるのは大変なので、主要キーだけにする。
            if (_keys == System.Windows.Forms.Keys.Enter) { _Ekeycode = EKeyCode.ENTER; }
            else if (_keys == System.Windows.Forms.Keys.Back) { _Ekeycode = EKeyCode.BACKSPACE; }
            else if (_keys == System.Windows.Forms.Keys.LShiftKey) { _Ekeycode = EKeyCode.LSHIFT; }
            else if (_keys == System.Windows.Forms.Keys.RShiftKey) { _Ekeycode = EKeyCode.RSHIFT; }
            else if (_keys == System.Windows.Forms.Keys.LControlKey) { _Ekeycode = EKeyCode.LCTRL; }
            else if (_keys == System.Windows.Forms.Keys.RControlKey) { _Ekeycode = EKeyCode.RCTRL; }
            else if (_keys == System.Windows.Forms.Keys.Tab) { _Ekeycode = EKeyCode.TAB; }
            else if (_keys == System.Windows.Forms.Keys.CapsLock) { _Ekeycode = EKeyCode.CAPSLOCK; }
            else if (_keys == System.Windows.Forms.Keys.Space) { _Ekeycode = EKeyCode.SPACE; }
            else if (_keys == System.Windows.Forms.Keys.Alt) { _Ekeycode = EKeyCode.ALT; }
            else if (_keys == System.Windows.Forms.Keys.Delete) { _Ekeycode = EKeyCode.RStick3_DELETE; }
            else if (_keys == System.Windows.Forms.Keys.Escape) { _Ekeycode = EKeyCode.ESCAPE; }

            else if (_keys == System.Windows.Forms.Keys.D1) { _Ekeycode = EKeyCode.KEY1; }
            else if (_keys == System.Windows.Forms.Keys.D2) { _Ekeycode = EKeyCode.KEY2; }
            else if (_keys == System.Windows.Forms.Keys.D3) { _Ekeycode = EKeyCode.KEY3; }
            else if (_keys == System.Windows.Forms.Keys.D4) { _Ekeycode = EKeyCode.KEY4; }
            else if (_keys == System.Windows.Forms.Keys.D5) { _Ekeycode = EKeyCode.KEY5; }
            else if (_keys == System.Windows.Forms.Keys.D6) { _Ekeycode = EKeyCode.KEY6; }
            else if (_keys == System.Windows.Forms.Keys.D7) { _Ekeycode = EKeyCode.KEY7; }
            else if (_keys == System.Windows.Forms.Keys.D8) { _Ekeycode = EKeyCode.KEY8; }
            else if (_keys == System.Windows.Forms.Keys.D9) { _Ekeycode = EKeyCode.KEY9; }
            else if (_keys == System.Windows.Forms.Keys.D0) { _Ekeycode = EKeyCode.KEY0; }

            else if (_keys == System.Windows.Forms.Keys.A) { _Ekeycode = EKeyCode.a; }
            else if (_keys == System.Windows.Forms.Keys.B) { _Ekeycode = EKeyCode.b; }
            else if (_keys == System.Windows.Forms.Keys.C) { _Ekeycode = EKeyCode.c; }
            else if (_keys == System.Windows.Forms.Keys.D) { _Ekeycode = EKeyCode.d; }
            else if (_keys == System.Windows.Forms.Keys.E) { _Ekeycode = EKeyCode.e; }
            else if (_keys == System.Windows.Forms.Keys.F) { _Ekeycode = EKeyCode.f; }
            else if (_keys == System.Windows.Forms.Keys.G) { _Ekeycode = EKeyCode.g; }
            else if (_keys == System.Windows.Forms.Keys.F) { _Ekeycode = EKeyCode.h; }
            else if (_keys == System.Windows.Forms.Keys.I) { _Ekeycode = EKeyCode.i; }
            else if (_keys == System.Windows.Forms.Keys.J) { _Ekeycode = EKeyCode.j; }
            else if (_keys == System.Windows.Forms.Keys.K) { _Ekeycode = EKeyCode.k; }
            else if (_keys == System.Windows.Forms.Keys.L) { _Ekeycode = EKeyCode.l; }
            else if (_keys == System.Windows.Forms.Keys.M) { _Ekeycode = EKeyCode.m; }
            else if (_keys == System.Windows.Forms.Keys.N) { _Ekeycode = EKeyCode.n; }
            else if (_keys == System.Windows.Forms.Keys.O) { _Ekeycode = EKeyCode.o; }
            else if (_keys == System.Windows.Forms.Keys.P) { _Ekeycode = EKeyCode.p; }
            else if (_keys == System.Windows.Forms.Keys.Q) { _Ekeycode = EKeyCode.q; }
            else if (_keys == System.Windows.Forms.Keys.R) { _Ekeycode = EKeyCode.r; }
            else if (_keys == System.Windows.Forms.Keys.S) { _Ekeycode = EKeyCode.s; }
            else if (_keys == System.Windows.Forms.Keys.T) { _Ekeycode = EKeyCode.t; }
            else if (_keys == System.Windows.Forms.Keys.U) { _Ekeycode = EKeyCode.u; }
            else if (_keys == System.Windows.Forms.Keys.V) { _Ekeycode = EKeyCode.v; }
            else if (_keys == System.Windows.Forms.Keys.W) { _Ekeycode = EKeyCode.w; }
            else if (_keys == System.Windows.Forms.Keys.X) { _Ekeycode = EKeyCode.x; }
            else if (_keys == System.Windows.Forms.Keys.Y) { _Ekeycode = EKeyCode.y; }
            else if (_keys == System.Windows.Forms.Keys.Z) { _Ekeycode = EKeyCode.z; }
            return _Ekeycode;
        }
        #endregion
    }        

	/// <summary>
    /// ゲームで認識可能なキーコード。CMouseAndKeyboardKeyInputで用いるキースキャンコード。
    /// System.Windows.Forms.Keysと対応させるには、getEKeyCode_***()メソッドを参照。
    /// 
    /// ・・・ゲームに使うキーだけを一部マッピングしたもの。
    /// 元はSDL.SDLKeyに対応できるようにやねうらお様が作ったものから、要素数は変更しないまま、
    /// メルサイアがわかりやすいように名前を変更し、更に語尾に必要なキーだけ連打（***_DOUBLECLICKなど）や長押し（***_LONGPRESS）を追加したもの。
    /// 
    /// ・・・a～zの値は英数小文字のASCIIコード（SHIFT-JISといっしょ）を使っているが、それ以外は適当。
    /// 自分で追加しているものも多いので、値は参考にならない。
    /// 
    /// ・・・マウスのキーは、MOUSE_LEFTなどで識別する。マウスや一部のキーは、ダブルクリック***_DOUBLE***なども登録されている。
    /// 
    /// キーボードのキーは、Shiftや左右別にLShiftとRShiftで認識するので注意。
    /// あと、Altは今のところ左右区別はしていない。
    /// あと、Fnはソフト的に単独では認識しないらしい。
    /// 
    /// ※なお、ここに列挙されているキーコード全てが認識できてるわけではないので注意！
    /// 　今は、アルファベット、メインキーの数字、Enter/ALT/Tab/CapsLockなどの主要キーしか認識させていない。
    /// 　認識出来るキーを増やすには、CMouseAndKeyboardKeyInputクラスのCheckKeyInWindows()メソッドを変更してください。
	/// </summary>
	public enum EKeyCode : int {

        // ●●●UNKNOWN～LASTまでは、Linux版（Androidなど）ではSdl.SDL.SDLKeyからそのままコピペするから、
        //      順番と値は変更してはいけない！。ただし、名前は変更してもよい。
		/* The keyboard syms have been cleverly chosen to map to ASCII */
        /// <summary>
        /// 0。わからないばあい。_NONEは、_UNKNOWNと同値です。
        /// </summary>
        UNKNOWN = 0,
        /// <summary>
        /// 0。デフォルト値。わからないばあい。_NONE、UNKNOWNと同値です。
        /// </summary>
        FIRST = 0,
        /// <summary>
        /// =0。デフォルト値。無入力、もしくはわからないばあい。_NONEは、_UNKNOWNと同値です。
        /// </summary>
        _NONE = 0,
		/// <summary>
		/// BackSpaceキー。
		/// </summary>
		BACKSPACE		= 8,
		/// <summary>
		/// Tabキー。
		/// </summary>
		TAB		= 9,
		/// <summary>
		/// クリアーキー。ASCIIコードの書式送りキーコード。現在はあまり使われない。
		/// </summary>
		CLEAR		= 12,

        // ●●ENTERと表記したいので，名前をRETURN→ENTERに変更 // Merusaia
        /// <summary>
        /// Enterキー(=13)。RETURNキーともよく表記される。
        /// </summary>
        ENTER = 13,
        // ●●変更終わり
		/// <summary>
        /// Pauseキー。Breakキーと共にDeleteキーの近くにあるキー。Fnを押しながらじゃないと認識しないキーボードもある。
		/// </summary>
		PAUSE		= 19,
		/// <summary>
		/// ESCキー
		/// </summary>
		ESCAPE		= 27,
		/// <summary>
		/// スペースキー
		/// </summary>
		SPACE		= 32,
        /// <summary>
        /// !。びっくりマーク。Shiftを押しながらで判定。
        /// </summary>
        EXCLAIM		= 33,
        /// <summary>
        /// "。ダブルクォート（QUOTEDBL）。Shiftを押しながらで判定。
        /// </summary>
        QUOTEDBL		= 34,
        /// <summary>
        /// #。シャープ。ハッシュ（HASH）。Shiftを押しながらで判定。
        /// </summary>
        HASH		= 35,
        /// <summary>
        /// $。ドルマーク（DOLLAR）。Shiftを押しながらで判定。
        /// </summary>
        DOLLAR		= 36,
        /// <summary>
        /// &。アンパサンド（AMPERSAND）。Shiftを押しながらで判定。
        /// </summary>
        AMPERSAND		= 38,
        /// <summary>
        /// '。クォート（QUOTE）。Shiftを押しながらで判定。
        /// </summary>
        QUOTE		= 39,
        /// <summary>
        /// (。開始カッコ。Shiftを押しながらで判定。
        /// </summary>
        LEFTPAREN		= 40,
        /// <summary>
        /// )。閉じカッコ。Shiftを押しながらで判定。
        /// </summary>
        RIGHTPAREN		= 41,
        /// <summary>
        /// *。アスタリスク（ASTERISK）。Shiftを押しながらで判定。
        /// </summary>
        ASTERISK		= 42,
		/// <summary>
        /// +。プラス（PLUS）。Shiftを押さないで判定。
		/// </summary>
		PLUS		= 43,
		/// <summary>
        /// ,。コンマ（COMMA）。Shiftを押さないで判定。
		/// </summary>
		COMMA		= 44,
		/// <summary>
        /// -。マイナス（MINUS）。Shiftを押さないで判定。
		/// </summary>
		MINUS		= 45,
		/// <summary>
        /// .。ピリオド（PERIOD）。Shiftを押さないで判定。
		/// </summary>
		PERIOD		= 46,
		/// <summary>
        /// /。スラッシュ（SLASH）。Shiftを押さないで判定。
		/// </summary>
		SLASH		= 47,
		/// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに半角/全角キーの右の「ぬ」キーを押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。USBやデスクトップＰＣ等に用意されているテンキーとは別。
		/// </summary>
		KEY0			= 48,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY1 = 49,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY2 = 50,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY3 = 51,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY4 = 52,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY5 = 53,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY6 = 54,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY7 = 55,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY8 = 56,
        /// <summary>
        /// Key1～Key9～Key0はメインキーボードの数字キー、Shiftを押さずに押すとこのキーが判定される。「１」キー（「ぬ」や「！」と同じキー）～左列の「０」（「を」や「わ」と同じキー）。ＡＳＣＩＩコード用。デスクトップＰＣ等に用意されている、テンキーとは別。
        /// </summary>
        KEY9 = 57,
		/// <summary>
        /// :。コロン（COLON）。Shiftを押さないで判定。
		/// </summary>
		COLON		= 58,
		/// <summary>
        /// ;。セミコロン（SEMICOLON）。Shiftを押さないで判定。
		/// </summary>
		SEMICOLON		= 59,
        /// <summary>
        /// \<。小なり。レス（LESS）。Shiftを押しながらで判定。
        /// </summary>
        LESS = 60,
        /// <summary>
        /// =。イコール（EQUAL）。Shiftを押しながらで判定。
        /// </summary>
        EQUAL = 61,
        /// <summary>
        /// \>。大なり。グレイター（GREATER）。Shiftを押しながらで判定。
        /// </summary>
        GREATER = 62,
        /// <summary>
        /// ?。クオーテーション（QUESTION）。Shiftを押しながらで判定。
        /// </summary>
        QUESTION = 63,
		/// <summary>
        /// @。アットマーク（ATMark）。Shiftを押さないで判定。
		/// </summary>
		ATMark			= 64,
		/* 
		   Skip uppercase letters
		 */
		/// <summary>
        /// [。左角括弧。レフトブラッケット（LEFTBRACKET）。Shiftを押さないで判定。
		/// </summary>
		LEFTBRACKET	= 91,
		/// <summary>
        /// \。バックスラッシュ（BACKSLASH）。Shiftを押さないで判定。
		/// </summary>
		BACKSLASH		= 92,
		/// <summary>
        /// ]。右角括弧。ライトブラッケット（RIGHTBRACKET）。Shiftを押さないで判定。
		/// </summary>
		RIGHTBRACKET	= 93,
		/// <summary>
        /// ^。キャレット（CARET）。Shiftを押さないで判定。
		/// </summary>
		CARET		= 94,
        /// <summary>
        /// _。アンダーバー。アンダースコープ（UNDERSCORE）。Shiftを押しながらで判定。
		/// </summary>
		UNDERSCORE		= 95,
		/// <summary>
        /// `。バックくオート（BACKQUOTE）。Shift+「@」。Shiftを押しながらで判定。
		/// </summary>
		BACKQUOTE		= 96,
		/// <summary>
        /// aキー。大文字版は用意されていないので、Shiftを押しながらでも、押さなくてもこのキーが判定。以下、b-zも同じ。
		/// </summary>
		a			= 97,
		/// <summary>
		/// 
		/// </summary>
		b			= 98,
		/// <summary>
		/// 
		/// </summary>
		c			= 99,
		/// <summary>
		/// 
		/// </summary>
		d			= 100,
		/// <summary>
		/// 
		/// </summary>
		e			= 101,
		/// <summary>
		/// 
		/// </summary>
		f			= 102,
		/// <summary>
		/// 
		/// </summary>
		g			= 103,
		/// <summary>
		/// 
		/// </summary>
		h			= 104,
		/// <summary>
		/// 
		/// </summary>
		i			= 105,
		/// <summary>
		/// 
		/// </summary>
		j			= 106,
		/// <summary>
		/// 
		/// </summary>
		k			= 107,
		/// <summary>
		/// 
		/// </summary>
		l			= 108,
		/// <summary>
		/// 
		/// </summary>
		m			= 109,
		/// <summary>
		/// 
		/// </summary>
		n			= 110,
		/// <summary>
		/// 
		/// </summary>
		o			= 111,
		/// <summary>
		/// 
		/// </summary>
		p			= 112,
		/// <summary>
		/// 
		/// </summary>
		q			= 113,
		/// <summary>
		/// 
		/// </summary>
		r			= 114,
		/// <summary>
		/// 
		/// </summary>
		s			= 115,
		/// <summary>
		/// 
		/// </summary>
		t			= 116,
		/// <summary>
		/// 
		/// </summary>
		u			= 117,
		/// <summary>
		/// 
		/// </summary>
		v			= 118,
		/// <summary>
		/// 
		/// </summary>
		w			= 119,
		/// <summary>
		/// 
		/// </summary>
		x			= 120,
		/// <summary>
		/// 
		/// </summary>
		y			= 121,
		/// <summary>
		/// 
		/// </summary>
		z			= 122,
		/// <summary>
		/// =127。ASCIIコードの最後。Deleteキー。普通のキーボードではBackspaceキーの上の方にある。
        /// ※Deleteキーはゲームパッド/PS3/PSVitaの右アナログスティック３（右下倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
		RStick3_DELETE		= 127,
		// End of ASCII mapped keysyms
        // アスキーコードに値が対応した場所、終わり。

        // ●国際キーボードのキーコードを格納しても、特に使わないが、SDL互換性のために残さないといけない。
		// International keyboard syms
        /// <summary>
        /// International keyboard syms
        /// </summary>
        WORLD_0 = 160,		/* 0xA0 */
        /// <summary>
        /// 
        /// </summary>
        WORLD_1 = 161,
        /// <summary>
        /// 
        /// </summary>
        WORLD_2 = 162,
        /// <summary>
        /// 
        /// </summary>
        WORLD_3 = 163,
        /// <summary>
        /// 
        /// </summary>
        WORLD_4 = 164,
        /// <summary>
        /// 
        /// </summary>
        WORLD_5 = 165,
        /// <summary>
        /// 
        /// </summary>
        WORLD_6 = 166,
        /// <summary>
        /// 
        /// </summary>
        WORLD_7 = 167,
        /// <summary>
        /// 
        /// </summary>
        WORLD_8 = 168,
        /// <summary>
        /// 
        /// </summary>
        WORLD_9 = 169,
        /// <summary>
        /// 
        /// </summary>
        WORLD_10 = 170,
        /// <summary>
        /// 
        /// </summary>
        WORLD_11 = 171,
        /// <summary>
        /// 
        /// </summary>
        WORLD_12 = 172,
        /// <summary>
        /// 
        /// </summary>
        WORLD_13 = 173,
        /// <summary>
        /// 
        /// </summary>
        WORLD_14 = 174,
        /// <summary>
        /// 
        /// </summary>
        WORLD_15 = 175,
        /// <summary>
        /// 
        /// </summary>
        WORLD_16 = 176,
        /// <summary>
        /// 
        /// </summary>
        WORLD_17 = 177,
        /// <summary>
        /// 
        /// </summary>
        WORLD_18 = 178,
        /// <summary>
        /// 
        /// </summary>
        WORLD_19 = 179,
        /// <summary>
        /// 
        /// </summary>
        WORLD_20 = 180,
        /// <summary>
        /// 
        /// </summary>
        WORLD_21 = 181,
        /// <summary>
        /// 
        /// </summary>
        WORLD_22 = 182,
        /// <summary>
        /// 
        /// </summary>
        WORLD_23 = 183,
        /// <summary>
        /// 
        /// </summary>
        WORLD_24 = 184,
        /// <summary>
        /// 
        /// </summary>
        WORLD_25 = 185,
        /// <summary>
        /// 
        /// </summary>
        WORLD_26 = 186,
        /// <summary>
        /// 
        /// </summary>
        WORLD_27 = 187,
        /// <summary>
        /// 
        /// </summary>
        WORLD_28 = 188,
        /// <summary>
        /// 
        /// </summary>
        WORLD_29 = 189,
        /// <summary>
        /// 
        /// </summary>
        WORLD_30 = 190,
        /// <summary>
        /// 
        /// </summary>
        WORLD_31 = 191,
        /// <summary>
        /// 
        /// </summary>
        WORLD_32 = 192,
        /// <summary>
        /// 
        /// </summary>
        WORLD_33 = 193,
        /// <summary>
        /// 
        /// </summary>
        WORLD_34 = 194,
        /// <summary>
        /// 
        /// </summary>
        WORLD_35 = 195,
        /// <summary>
        /// 
        /// </summary>
        WORLD_36 = 196,
        /// <summary>
        /// 
        /// </summary>
        WORLD_37 = 197,
        /// <summary>
        /// 
        /// </summary>
        WORLD_38 = 198,
        /// <summary>
        /// 
        /// </summary>
        WORLD_39 = 199,
        /// <summary>
        /// 
        /// </summary>
        WORLD_40 = 200,
        /// <summary>
        /// 
        /// </summary>
        WORLD_41 = 201,
        /// <summary>
        /// 
        /// </summary>
        WORLD_42 = 202,
        /// <summary>
        /// 
        /// </summary>
        WORLD_43 = 203,
        /// <summary>
        /// 
        /// </summary>
        WORLD_44 = 204,
        /// <summary>
        /// 
        /// </summary>
        WORLD_45 = 205,
        /// <summary>
        /// 
        /// </summary>
        WORLD_46 = 206,
        /// <summary>
        /// 
        /// </summary>
        WORLD_47 = 207,
        /// <summary>
        /// 
        /// </summary>
        WORLD_48 = 208,
        /// <summary>
        /// 
        /// </summary>
        WORLD_49 = 209,
        /// <summary>
        /// 
        /// </summary>
        WORLD_50 = 210,
        /// <summary>
        /// 
        /// </summary>
        WORLD_51 = 211,
        /// <summary>
        /// 
        /// </summary>
        WORLD_52 = 212,
        /// <summary>
        /// 
        /// </summary>
        WORLD_53 = 213,
        /// <summary>
        /// 
        /// </summary>
        WORLD_54 = 214,
        /// <summary>
        /// 
        /// </summary>
        WORLD_55 = 215,
        /// <summary>
        /// 
        /// </summary>
        WORLD_56 = 216,
        /// <summary>
        /// 
        /// </summary>
        WORLD_57 = 217,
        /// <summary>
        /// 
        /// </summary>
        WORLD_58 = 218,
        /// <summary>
        /// 
        /// </summary>
        WORLD_59 = 219,
        /// <summary>
        /// 
        /// </summary>
        WORLD_60 = 220,
        /// <summary>
        /// 
        /// </summary>
        WORLD_61 = 221,
        /// <summary>
        /// 
        /// </summary>
        WORLD_62 = 222,
        /// <summary>
        /// 
        /// </summary>
        WORLD_63 = 223,
        /// <summary>
        /// 
        /// </summary>
        WORLD_64 = 224,
        /// <summary>
        /// 
        /// </summary>
        WORLD_65 = 225,
        /// <summary>
        /// 
        /// </summary>
        WORLD_66 = 226,
        /// <summary>
        /// 
        /// </summary>
        WORLD_67 = 227,
        /// <summary>
        /// 
        /// </summary>
        WORLD_68 = 228,
        /// <summary>
        /// 
        /// </summary>
        WORLD_69 = 229,
        /// <summary>
        /// 
        /// </summary>
        WORLD_70 = 230,
        /// <summary>
        /// 
        /// </summary>
        WORLD_71 = 231,
        /// <summary>
        /// 
        /// </summary>
        WORLD_72 = 232,
        /// <summary>
        /// 
        /// </summary>
        WORLD_73 = 233,
        /// <summary>
        /// 
        /// </summary>
        WORLD_74 = 234,
        /// <summary>
        /// 
        /// </summary>
        WORLD_75 = 235,
        /// <summary>
        /// 
        /// </summary>
        WORLD_76 = 236,
        /// <summary>
        /// 
        /// </summary>
        WORLD_77 = 237,
        /// <summary>
        /// 
        /// </summary>
        WORLD_78 = 238,
        /// <summary>
        /// 
        /// </summary>
        WORLD_79 = 239,
        /// <summary>
        /// 
        /// </summary>
        WORLD_80 = 240,
        /// <summary>
        /// 
        /// </summary>
        WORLD_81 = 241,
        /// <summary>
        /// 
        /// </summary>
        WORLD_82 = 242,
        /// <summary>
        /// 
        /// </summary>
        WORLD_83 = 243,
        /// <summary>
        /// 
        /// </summary>
        WORLD_84 = 244,
        /// <summary>
        /// 
        /// </summary>
        WORLD_85 = 245,
        /// <summary>
        /// 
        /// </summary>
        WORLD_86 = 246,
        /// <summary>
        /// 
        /// </summary>
        WORLD_87 = 247,
        /// <summary>
        /// 
        /// </summary>
        WORLD_88 = 248,
        /// <summary>
        /// 
        /// </summary>
        WORLD_89 = 249,
        /// <summary>
        /// 
        /// </summary>
        WORLD_90 = 250,
        /// <summary>
        /// 
        /// </summary>
        WORLD_91 = 251,
        /// <summary>
        /// 
        /// </summary>
        WORLD_92 = 252,
        /// <summary>
        /// 
        /// </summary>
        WORLD_93 = 253,
        /// <summary>
        /// 
        /// </summary>
        WORLD_94 = 254,
        /// <summary>
        /// 
        /// </summary>
        WORLD_95 = 255,		/* 0xFF */

        // Numeric keypad。テンキー
        // ●ゲームパッド/PS3/PSVitaの左右アナログスティックとの対応に、テンキーを使っている。
        //   ただしテンキーだけでは足りないので、PageUpなどのキーも使っている。
        /// <summary>
        /// テンキーの0。KeyPad0。QWERTYキーボード標準搭載の0キーはKEY0を参照。　
        /// ※テンキー0はゲームパッド/PS3/PSVitaの右アナログスティック１（左下倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        RStick1_TenKeyPad0 = 256,
        /// <summary>
        /// テンキーの1。QWERTYキーボード標準搭載の1キーはKEY1を参照。　
        /// ※テンキーはゲームパッド/PS3/PSVitaの左アナログスティック１（左下倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick1_TenKeyPad1 = 257,
        /// <summary>
        /// テンキーの2。QWERTYキーボード標準搭載の数字キーはKEY1～0を参照。　
        /// ※テンキーはゲームパッド/PS3/PSVitaの左アナログスティック２（左下倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick2_TenKeyPad2 = 258,
        /// <summary>
        /// テンキーの3。QWERTYキーボード標準搭載の数字キーはKEY1～0を参照。　
        /// ※テンキーはゲームパッド/PS3/PSVitaの左アナログスティック３（右下倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick3_TenKeyPad3 = 259,
        /// <summary>
        /// テンキーの4。QWERTYキーボード標準搭載の数字キーはKEY1～0を参照。　
        /// ※テンキーはゲームパッド/PS3/PSVitaの左アナログスティック４（左倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick4_TenKeyPad4 = 260,
        /// <summary>
        /// テンキーの5。QWERTYキーボード標準搭載の数字キーはKEY1～0を参照。　
        /// ※テンキー5はゲームパッド/ゲームパッド/PS3/PSVitaの左アナログスティックの５中ボタン（Ｌ３ボタン）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick5_L3Button_TenKeyPad5 = 261,
        /// <summary>
        /// テンキーの6。QWERTYキーボード標準搭載の数字キーはKEY1～0を参照。　
        /// ※テンキーはゲームパッド/PS3/PSVitaの左アナログスティック６（右倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick6_TenKeyPad6 = 262,
        /// <summary>
        /// テンキーの7。QWERTYキーボード標準搭載の数字キーはKEY1～0を参照。　
        /// ※テンキーはゲームパッド/PS3/PSVitaの左アナログスティック７（左上倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick7_TenKeyPad7 = 263,
        /// <summary>
        /// テンキーの8。QWERTYキーボード標準搭載の数字キーはKEY1～0を参照。　
        /// ※テンキーはゲームパッド/PS3/PSVitaの左アナログスティック８（上倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick8_TenKeyPad8 = 264,
        /// <summary>
        /// テンキーの9。QWERTYキーボード標準搭載の数字キーはKEY1～0を参照。　
        /// ※テンキーはゲームパッド/PS3/PSVitaの左アナログスティック９（右上倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        LStick9_TenKeyPad9 = 265,
        /// <summary>
        /// .。テンキーのピリオド（小数点）。
        /// ※テンキー.はゲームパッド/PS3/PSVitaの右アナログスティック２（下倒し）に対応させるといいかも。
        /// </summary>
        RStick2_TenKeyPad_PERIOD = 266,
        /// <summary>
        /// /。テンキーのスラッシュ（÷）。
        /// ※テンキー/はゲームパッド/PS3/PSVitaの右アナログスティック７（左上倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        RStick7_TenKeyPad_DIVIDE = 267,
        /// <summary>
        /// *。テンキーの掛け算（×）
        /// ※テンキー*はゲームパッド/PS3/PSVitaの右アナログスティック８（上倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        RStick8_TenKeyPad_MULTIPLY = 268,
        /// <summary>
        /// -。テンキーのマイナス（ー）。
        /// ※テンキー-はゲームパッド/PS3/PSVitaの右アナログスティック９（右上倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        RStick9_TenKeyPad_MINUS = 269,
        /// <summary>
        /// +。テンキーのプラス（＋）。
        /// ※テンキーEnterはゲームパッド/PS3/PSVitaの右アナログスティック６（右倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
        RStick6_TenKeyPad_PLUS = 270,
        /// <summary>
        /// Enter。テンキーのEnter。Windowsの仮想キーコードVK_***では普通のEnterキーと別には取れない。これを使わず、普通のEnterを使った方がいい。
        /// </summary>
        KeyCode271_TenKeyPad_KP_ENTER = 271,
        /// <summary>
        /// =。テンキーのイコール。普通、テンキーには、=キーは用意されていない。+-/*もしくはEnterキーがその機能を代用するので、そのどちらかを使った方がいいかも。=キーがあるのは、バーチャルキーボードかiPhone/Androidの電卓アプリ位。
        /// </summary>
        KeyCode272_TenKeyPad_KP_EQUALS = 272,


		// Arrows + Home/End pad
		/// <summary>
        /// ↑。上矢印
		/// </summary>
		UP			= 273,
		/// <summary>
		/// ↓。下矢印
		/// </summary>
		DOWN		= 274,
		/// <summary>
		/// →。右矢印。
		/// </summary>
		RIGHT		= 275,
		/// <summary>
		/// ←。左矢印。
		/// </summary>
		LEFT		= 276,
		/// <summary>
		/// Insertキー。挿入・上書きの切り替えキー。よくBackspaceキーの上の方にある。
		/// </summary>
		INSERT		= 277,
		/// <summary>
		/// ＰＣのキーボードによくあるHomeキー。ノートＰＣだとFnを押しながらとかかも。
        /// ※Homeキーはゲームパッド/PS3/PSVitaの右アナログスティック４（左倒し）に対応させるといいかも。十字キーはUPなどを参照。
		/// </summary>
		RStick4_HOME		= 278,
		/// <summary>
        /// ＰＣのキーボードによくあるEndキー。ノートＰＣだとFnを押しながらとかかも。
        /// ※Endキーはゲームパッド/PS3/PSVitaの右アナログスティック６（右倒し）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
		RStick6_END		= 279,
		/// <summary>
        /// ＰＣのキーボードによくあるPgUpキー。ノートＰＣだとFnを押しながらとかかも。
        /// ※PageUpキーはゲームパッド/PS3/PSVitaのＰＳボタン（ゲームの中断やゲーム画面とクロスメディアバーの切り替え）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
		PSButton_PAGEUP		= 280,
		/// <summary>
        /// ＰＣのキーボードによくあるPgDnキー。ノートＰＣだとFnを押しながらとかかも。
        /// ※PageDownキーはゲームパッド/PS3/PSVitaの右アナログスティック５中ボタン押し（Ｒ３ボタン）に対応させるといいかも。十字キーはUPなどを参照。
        /// </summary>
		RStick5_R3Button_PAGEDOWN		= 281,

		// Function keys
		/// <summary>
		/// F1キー。ファンクションキー
		/// </summary>
		F1			= 282,
		/// <summary>
		/// F2キー。ファンクションキー
		/// </summary>
		F2			= 283,
		/// <summary>
		/// 
		/// </summary>
		F3			= 284,
		/// <summary>
		/// 
		/// </summary>
		F4			= 285,
		/// <summary>
		/// 
		/// </summary>
		F5			= 286,
		/// <summary>
		/// 
		/// </summary>
		F6			= 287,
		/// <summary>
		/// 
		/// </summary>
		F7			= 288,
		/// <summary>
		/// 
		/// </summary>
		F8			= 289,
		/// <summary>
		/// 
		/// </summary>
		F9			= 290,
		/// <summary>
		/// 
		/// </summary>
		F10		= 291,
		/// <summary>
		/// 
		/// </summary>
		F11		= 292,
		/// <summary>
		/// 
		/// </summary>
		F12		= 293,
		/// <summary>
		/// 
		/// </summary>
		F13		= 294,
		/// <summary>
		/// 
		/// </summary>
		F14		= 295,
		/// <summary>
		/// 
		/// </summary>
		F15		= 296,

		// Key state modifier keys
		/// <summary>
		/// NumLockキー。数字限定入力切り替えキー。これを押すとキーボードを叩いても数字しか入力できなくなるので注意。もう一度押すと元に戻る。
		/// </summary>
		NUMLOCK		= 300,
		/// <summary>
		/// CapsLockキー。大文字小文字切り替えキー。これを押すとデフォルトで半角英字が大文字になる。もう一度押すと元に戻る。
		/// </summary>
		CAPSLOCK		= 301,
		/// <summary>
		/// ScrolLockキー。スクロールロックキー。現在はこれを採用しているソフトウェアは少なく、ほとんど使われていない。
		/// </summary>
		SCROLLOCK		= 302,
		/// <summary>
		/// 右のShiftキー。左のShiftキーとは別のキーとして区別可能にしている。
		/// </summary>
		RSHIFT		= 303,
		/// <summary>
		/// 左のShiftキー。右のShiftキーとは別のキーとして区別可能にしている。
		/// </summary>
		LSHIFT		= 304,
		/// <summary>
        /// 右のControlキー。左のControlキーとは別のキーとして区別可能にしている。
        /// </summary>
		RCTRL		= 305,
		/// <summary>
        /// 左のControlキー。左のControlキーとは別のキーとして区別可能にしている。
        /// </summary>
		LCTRL		= 306,
        /// <summary>
        /// Altキー。左のALTと右のALTとキーコードは同じ値。現在は諸事情により、左右の区別は付けてない。詳細はVirtualKeyStates.VK_MENUを検索して参照
        /// </summary>
        ALT = 307,
        /// <summary>
        /// ※Altの判定にはALTを使ってください。元はSDLのLALT（=308）。ALT(=307)とキーコードは異なります。SDL.Keys対応のために便宜上残しているだけです。
        /// 左のALTと右のALTとキーコードは同じ値。現在は諸事情により、左右の区別は付けてない。詳細はVirtualKeyStates.VK_MENUを検索して参照。
        /// </summary>
        KeyCode308_LALT = 308,
        /// <summary>
        /// 右メタキー（Meta key）。キーボードには◆や◇のような記号で印字されている。現在はほとんど使われていない。
        /// </summary>
        RMETA = 309,
        /// <summary>
        /// 右メタキー（Meta key）。キーボードには◆や◇のような記号で印字されている。現在はほとんど使われていない。
        /// </summary>
        LMETA = 310,
        /// <summary>
        /// Left "Windows" _keycode
        /// </summary>
        LSUPER = 311,
        /// <summary>
        /// Right "Windows" _keycode
        /// </summary>
        RSUPER = 312,
        /// <summary>
        /// "Alt Gr" _keycode
        /// </summary>
        MODE = 313,
        /// <summary>
        /// Multi-_keycode compose _keycode
        /// </summary>
        COMPOSE = 314,

        ///* Miscellaneous function keys */
        /// <summary>
        /// 
        /// </summary>
        HELP = 315,
        /// <summary>
        /// 
        /// </summary>
        PRINT = 316,
        /// <summary>
        /// 
        /// </summary>
        SYSREQ = 317,
        /// <summary>
        /// 
        /// </summary>
        BREAK = 318,
        /// <summary>
        /// 
        /// </summary>
        MENU = 319,
        /// <summary>
        /// Power Macintosh power _keycode
        /// </summary>
        POWER = 320,
        /// <summary>
        /// Some european keyboards
        /// </summary>
        EURO = 321,
        /// <summary>
        /// Atari keyboard has Undo
        /// </summary>
        UNDO = 322,
        
		/* Add any other keys here */
		/// <summary>
        /// =323。SDL.Keysに対応したキーの終了。
        /// その後、追加したマウスボタンなどは = 324以降
		/// </summary>
		LAST,


        // ●●●SDL.Keysに対応したキーの終了
        // 以上、やねうらお様が追加

        // 以下、merusaiaが追加

        // ブラウザボタン。VB_***にたまたまあったので、ブラウザアプリ用に対応できるよう、一応追加しておく。
        /// <summary>
        /// // ブラウザボタン。次へ進むボタン。VB_***にたまたまあったので、ブラウザアプリ用に対応できるよう、一応追加しておく。
        /// </summary>
        BROWSER_FORWARD = 324,
        /// <summary>
        /// // ブラウザボタン。戻るボタン。VB_***にたまたまあったので、ブラウザアプリ用に対応できるよう、一応追加しておく。
        /// </summary>
        BROWSER_BACK,
        /// <summary>
        /// // ブラウザボタン。更新ボタン。VB_***にたまたまあったので、ブラウザアプリ用に対応できるよう、一応追加しておく。
        /// </summary>
        BROWSER_REFRESH,


        // ●マウスボタン
        /// <summary>
        /// マウス左ボタン
        /// </summary>
        MOUSE_LEFT,
        /// <summary>
        /// マウス右ボタン
        /// </summary>
        MOUSE_RIGHT,
        /// <summary>
        /// マウス中ボタン。MOUSE_WHEELホイールボタンと同じ。
        /// </summary>
        MOUSE_MIDDLE,
        /// <summary>
        /// マウスホイールボタン。MOUSE_MIDDLEと同じ。
        /// </summary>
        MOUSE_WHEELBUTTON,
        /// <summary>
        /// マウスX1ボタン（WindowsXP～？）
        /// </summary>
        MOUSE_X1,
        /// <summary>
        /// マウスX2ボタン（WindowsXP～？）
        /// </summary>
        MOUSE_X2,

        /// <summary>
        /// マウス左クリック長押しボタン。閾値はs_mouseLongPressLeftを参照してください。
        /// </summary>
        MOUSE_LEFT_PRESSLONG,
        /// <summary>
        /// マウス右クリック長押しボタン。閾値はs_mouseLongPressRightを参照してください。
        /// </summary>
        MOUSE_RIGHT_PRESSLONG,
        /// <summary>
        /// マウス中（ホイールボタン）クリック長押しボタン。閾値はs_mouseLongPressMiddleを参照してください。
        /// </summary>
        MOUSE_MIDDLE_PRESSLONG,

        /// <summary>
        /// マウス左ダブルクリック。閾値はs_mouseDoubleClickLeftを参照してください。
        /// </summary>
        MOUSE_LEFT_DOUBLECLICK,
        /// <summary>
        /// マウス右ダブルクリック。閾値はs_mouseDoubleClickRightを参照してください。
        /// </summary>
        MOUSE_RIGHT_DOUBLECLICK,
        /// <summary>
        /// マウス中（ホイールボタン）ダブルクリック。閾値はs_mouseDoubleClickMiddleを参照してください。
        /// </summary>
        MOUSE_MIDDLE_DOUBLECLICK,

        /// <summary>
        /// マウス左トリプルクリック。閾値はs_mouseDoubleClickLeftを参照してください。
        /// </summary>
        MOUSE_LEFT_TRIPLECLICK,
        /// <summary>
        /// マウス右トリプルクリック。閾値はs_mouseDoubleClickRightを参照してください。
        /// </summary>
        MOUSE_RIGHT_TRIPLECLICK,
        /// <summary>
        /// マウス中（ホイールボタン）トリプルクリック。閾値はs_mouseTripleClickMiddleを参照してください。
        /// </summary>
        MOUSE_MIDDLE_TRIPLECLICK,




        // ■キーの長押しボタンや連打ボタンの追加（単なるキー押し続けるイベントとは違うイベントとして認識させたい場合だけ追加）

        //【ゲームで認識可能な主要キー】矢印キー4種類, Enter, Backspace, Shift, Ctrl, Tab, CapsLock, Space, Alt, Delete, Esc, KEY1, KEY2, ..., KEY9, KEY0, a, b, ..., zまで (記号はとりあえず無し)

        // 以下は、ゲームパッド/PS3/PSVita用のボタンに登録する予定のキー。
        // 基本は、→・↓・←・↑・○・×・□・△・Ｌ・Ｒ・ＳＴＡＲＴ・ＳＥＬＥＣＴ・Ｌ２・Ｒ２の合計１４ボタン。
        // ＰＳ系コントローラーの２連打・３連打・長押しの全対応を考えるなら、可能なら、
        //      応用として、ＰＳボタン・Ｌアナログスティック１～９（Ｌ３ボタン含む）・Ｒアナログスティック１～９（Ｒ３ボタン含む）、オプションボタン１（ＰＳ４搭載予定のシェアボタン）の合計１０ボタンも含めて
        //      基本１４＋応用２０　＝　計３４ボタンは備えておいた方がいいかも。


        // ●ＰＳ系コントローラーの↑、↓、←、→の長押しボタン。長押しダッシュ（？）などを認識可能にするために追加。
        /// <summary>
        ///  ↑の長押しボタン。長押しダッシュ（？）などを認識可能にするために追加。
        /// </summary>
        UP_PRESSLONG,
        /// <summary>
        ///  ↑の２連打ボタン。ダッシュなどを認識可能にするために追加。
        /// </summary>
        UP_DOUBLECLICK,
        /// <summary>
        ///  ↑の３連打ボタン。スペシャルダッシュ（？）などを認識可能にするために追加。
        /// </summary>
        UP_TRIPLECLICK,

        DOWN_PRESSLONG,
        DOWN_DOUBLECLICK,
        DOWN_TRIPLECLICK,

        LEFT_PRESSLONG,
        LEFT_DOUBLECLICK,
        LEFT_TRIPLECLICK,

        RIGHT_PRESSLONG,
        RIGHT_DOUBLECLICK,
        RIGHT_TRIPLECLICK,

        // ＰＳ系コントローラーの○×□△ＬＲボタンは、このあたりのボタンで当てはめるつもり。

        /// <summary>
        /// Enterの長押しボタン。閾値はs_keyLongPressを参照してください。
        /// </summary>        
        ENTER_PRESSLONG,
        /// <summary>
        /// Enterのダブルクリック（二連打）ボタン。閾値はs_keyDoubleClickRightを参照してください。
        /// </summary>
        ENTER_DOUBLECLICK,
        /// <summary>
        /// Enterのトリプルクリック（三連だ打）ボタン。閾値はs_keyDoubleClickRightを参照してください。
        /// </summary>
        ENTER_TRIPLECLICK,

        BACKSPACE_PRESSLONG,
        BACKSPACE_DOUBLECLICK,
        BACKSPACE_TRIPLECLICK,

        LSHIFT_PRESSLONG,
        LSHIFT_DOUBLECLICK,
        LSHIFT_TRIPLECLICK,

        RSHIFT_PRESSLONG,
        RSHIFT_DOUBLECLICK,
        RSHIFT_TRIPLECLICK,

        LCTRL_PRESSLONG,
        LCTRL_DOUBLECLICK,
        LCTRL_TRIPLECLICK,

        RCTRL_PRESSLONG,
        RCTRL_DOUBLECLICK,
        RCTRL_TRIPLECLICK,

        TAB_PRESSLONG,
        TAB_DOUBLECLICK,
        TAB_TRIPLECLICK,

        CAPSLOCK_PRESSLONG,
        CAPSLOCK_DOUBLECLICK,
        CAPSLOCK_TRIPLECLICK,

        // このあたりでもいい
        z_PRESSLONG,
        z_DOUBLECLICK,
        z_TRIPLECLICK,

        x_PRESSLONG,
        x_DOUBLECLICK,
        x_TRIPLECLICK,

        c_PRESSLONG,
        c_DOUBLECLICK,
        c_TRIPLECLICK,

        v_PRESSLONG,
        v_DOUBLECLICK,
        v_TRIPLECLICK,



        a_PRESSLONG,
        a_DOUBLECLICK,
        a_TRIPLECLICK,

        s_PRESSLONG,
        s_DOUBLECLICK,
        s_TRIPLECLICK,

        q_PRESSLONG,
        q_DOUBLECLICK,
        q_TRIPLECLICK,

        w_PRESSLONG,
        w_DOUBLECLICK,
        w_TRIPLECLICK,


        // ●ＰＳ系コントローラーのSTART,SELECTを予定している、SPACEとALT

        /// <summary>
        /// スペースの長押しボタン。閾値はs_keyLongPressを参照してください。
        /// </summary>
        SPACE_PRESSLONG,
        /// <summary>
        /// スペースのダブルクリック（二連打）ボタン。閾値はs_keyDoubleClickRightを参照してください。
        /// </summary>
        SPACE_DOUBLECLICK,
        /// <summary>
        /// スペースのトリプルクリック（三連打）ボタン。閾値はs_keyDoubleClickRightを参照してください。
        /// </summary>
        SPACE_TRIPLECLICK,

        ALT_PRESSLONG,
        ALT_DOUBLECLICK,
        ALT_TRIPLECLICK,

        // ●ＰＳ系コントローラーのL2,R2用を予定している、KEY1とKEY2
        KEY1_PRESSLONG,
        KEY1_DOUBLECLICK,
        KEY1_TRIPLECLICK,

        KEY2_PRESSLONG,
        KEY2_DOUBLECLICK,
        KEY2_TRIPLECLICK,

        // ●ＰＳ系コントローラーのＰＳボタン用を予定している、PgUp
        PSButton_PgUp_PRESSLONG,
        PSButton_PgUp_DOUBLECLICK,
        PSButton_PgUp_TRIPLECLICK,

        // この2キーは今のところ実用的な使い道はない。とりあえず新しく２連打・３連打・長押しイベントを作りたい保健用としておいておく。

        DELETE_PRESSLONG,
        DELETE_DOUBLECLICK,
        DELETE_TRIPLECLICK,

        ESCAPE_PRESSLONG,
        ESCAPE_DOUBLECLICK,
        ESCAPE_TRIPLECLICK,




		/// <summary>
        /// このEnum要素の総数（Count）代わりに…ならない。
        /// だってa=1000,b=50,c,d,..,z,countとかでやっても、countは27になっちゃうもん。
        /// 要素数はMyTools.getEnumIntMaxCount()で取れるから、それでちゃんと取って。
        /// そういうルールさえ守れば、とても使いやすいよ、列挙体。「Ｃ＃、みんなもつかおう、列挙たい」。
		/// </summary>
        //count・もし要素に=がなければ使えるけど、厳密には保証できないから、怖いのでちゃんとMyTools.getEnumIntMaxCount()で取って
	}
}
