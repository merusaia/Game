using System;
using Yanesdk.Input;

namespace PublicDomain
{
    // 以下、ほとんどはYaneSDKのソースを引用。やねうらお様に感謝しますm(_ _)m


	/// <summary>
	/// キーデバイス登録済み、仮想キー設定済みのクラスです。
	/// </summary>
	/// <remarks>
	/// <Para>
	/// ＫｅｙＢｏａｒｄ　＋　ＪｏｙＳｔｉｃｋ判定。
	/// 上下左右＋２ボタンのゲーム用。
	/// </Para>
	/// <Para>
	/// ボタン番号
	/// ０：ＥＳＣキー
	/// １：テンキー８，↑キー，ジョイスティック↑
	/// ２：テンキー２，↓キー，ジョイスティック↓
	/// ３：テンキー４，←キー，ジョイスティック←
	/// ４：テンキー２，→キー，ジョイスティック→
	/// ５：スペースキー，ジョイスティック　ボタン１
	/// ６：テンキーEnter,リターンキー，左シフト，右シフト。ジョイスティック　ボタン２
	/// </Para>
	/// </remarks>
	/// <example>
	/// 使用例)
	/// <code>
	///
	///		CKey1・キー入力クラス _keycode = new CKey1・キー入力クラス();
	///
	///		while(true){
	///
	///			_keycode.Update();
	///
	///			string s = null;
	///			keyinput.Update();
    ///			for (int i = 0; i 小なり 7; ++i)
	///			{
	///				if (keyinput.IsPress(i))
	///					s+=i.ToString();
	///			}
	///			Console.WriteLine(s);
	///		}
	///	}
 	/// </code>
	/// </example>
	public class CKey1・キー入力クラス : CVirtualKey・複数の入力をひとまとめにする仮想キー管理者 {
		/// <summary>
		/// 
		/// </summary>
		public CKey1・キー入力クラス() {
			keyboard = new CMouseAndKeyBoardKeyInput・キー入力定義();
			joystick = new CJoyStick・ジョイスティック入力定義(0);

			AddDevice(keyboard);
			AddDevice(joystick);

			//	0	:	Escape
			AddVirtualKey(0,0,(int)EKeyCode.ESCAPE);

			//	1	:	Up
			AddVirtualKey(1,0,(int)EKeyCode.LStick8_TenKeyPad8);
			AddVirtualKey(1,0,(int)EKeyCode.UP);
			AddVirtualKey(1,1,0);

			//	2	:	Down
			AddVirtualKey(2,0,(int)EKeyCode.LStick2_TenKeyPad2);
			AddVirtualKey(2,0,(int)EKeyCode.DOWN);
			AddVirtualKey(2,1,1);

			//	3	:	Left
			AddVirtualKey(3,0,(int)EKeyCode.LStick4_TenKeyPad4);
			AddVirtualKey(3,0,(int)EKeyCode.LEFT);
			AddVirtualKey(3,1,2);

			//	4	:	Right
			AddVirtualKey(4,0,(int)EKeyCode.LStick6_TenKeyPad6);
			AddVirtualKey(4,0,(int)EKeyCode.RIGHT);
			AddVirtualKey(4,1,3);

			//	5	:	Space
			AddVirtualKey(5,0,(int)EKeyCode.SPACE);
			AddVirtualKey(5,1,4);

			//	6	:	Return
			AddVirtualKey(6,0,(int)EKeyCode.ENTER);
			AddVirtualKey(6,0,(int)EKeyCode.KeyCode271_TenKeyPad_KP_ENTER);
			AddVirtualKey(6,0,(int)EKeyCode.LSHIFT);
			AddVirtualKey(6,0,(int)EKeyCode.RSHIFT);
			AddVirtualKey(6,1,5);
		}

		/// <summary>
		/// 
		/// </summary>
		private CMouseAndKeyBoardKeyInput・キー入力定義 keyboard;
		/// <summary>
		/// 
		/// </summary>
		private CJoyStick・ジョイスティック入力定義 joystick;
	}

	/// <summary>
	/// ＫｅｙＢｏａｒｄ　＋　ＪｏｙＳｔｉｃｋ判定。
	/// →　Key1も参照のこと。
	/// </summary>
	/// <remarks>
	/// <Para>こちらは、上下左右＋６ボタンのゲーム用。
	/// </Para>
	/// <Para>
	/// ボタン配置：
	/// ０：ＥＳＣキー,ジョイスティック　ボタン７，８，９
	/// １：テンキー８，↑キー，ジョイスティック↑
	/// ２：テンキー２，↓キー，ジョイスティック↓
	/// ３：テンキー４，←キー，ジョイスティック←
	/// ４：テンキー２，→キー，ジョイスティック→
	/// ５：スペースキー，Ｚキー，ジョイスティック　ボタン１
	/// ６：テンキーEnter,リターンキー，Ｘキー，ジョイスティック　ボタン２
	/// ７：Ｃキー，ジョイスティック　ボタン３
	/// ８：Ａキー，ジョイスティック　ボタン４
	/// ９：Ｓキー，ジョイスティック　ボタン５
	/// １０：Ｄキー，ジョイスティック　ボタン６
	/// </Para>
	/// </remarks>
	public class CKey2・キー入力クラス : CVirtualKey・複数の入力をひとまとめにする仮想キー管理者 {
		/// <summary>
		/// 
		/// </summary>
		public CKey2・キー入力クラス() {
			keyboard = new CMouseAndKeyBoardKeyInput・キー入力定義();
			joystick = new CJoyStick・ジョイスティック入力定義(0);

			AddDevice(keyboard);
			AddDevice(joystick);

			//	0	:	Escape
			AddVirtualKey(0,0,(int)EKeyCode.ESCAPE);
			AddVirtualKey(0,1,10);
			AddVirtualKey(0,1,11);
			AddVirtualKey(0,1,12);

			//	1	:	Up
			AddVirtualKey(1,0,(int)EKeyCode.LStick8_TenKeyPad8);
			AddVirtualKey(1,0,(int)EKeyCode.UP);
			AddVirtualKey(1,1,0);

			//	2	:	Down
			AddVirtualKey(2,0,(int)EKeyCode.LStick2_TenKeyPad2);
			AddVirtualKey(2,0,(int)EKeyCode.DOWN);
			AddVirtualKey(2,1,1);

            //	3	:	Left
			AddVirtualKey(3,0,(int)EKeyCode.LStick4_TenKeyPad4);
			AddVirtualKey(3,0,(int)EKeyCode.LEFT);
			AddVirtualKey(3,1,2);

			//	4	:	Right
			AddVirtualKey(4,0,(int)EKeyCode.RStick6_TenKeyPad_PLUS);
			AddVirtualKey(4,0,(int)EKeyCode.RIGHT);
			AddVirtualKey(4,1,3);

			//	5	:	Space
			AddVirtualKey(5,0,(int)EKeyCode.SPACE);
			AddVirtualKey(5,0,(int)EKeyCode.z);
			AddVirtualKey(5,1,4);

			//	6	:	Return
			AddVirtualKey(6,0,(int)EKeyCode.ENTER);
			AddVirtualKey(6,0,(int)EKeyCode.KeyCode271_TenKeyPad_KP_ENTER);
			AddVirtualKey(6,0,(int)EKeyCode.LSHIFT);
			AddVirtualKey(6,0,(int)EKeyCode.RSHIFT);
			AddVirtualKey(6,0,(int)EKeyCode.x);
			AddVirtualKey(6,1,5);

			//	7	:	Button C
			AddVirtualKey(7,0,(int)EKeyCode.c);
			AddVirtualKey(7,1,6);

			//	8	:	Button A
			AddVirtualKey(8,0,(int)EKeyCode.a);
			AddVirtualKey(8,1,7);

			//	9	:	Button S
			AddVirtualKey(9,0,(int)EKeyCode.s);
			AddVirtualKey(9,1,8);

			//	10	:	Button D
			AddVirtualKey(10,0,(int)EKeyCode.d);
			AddVirtualKey(10,1,9);

		}

		/// <summary>
		/// 
		/// </summary>
		private CMouseAndKeyBoardKeyInput・キー入力定義 keyboard;
		/// <summary>
		/// 
		/// </summary>
		private CJoyStick・ジョイスティック入力定義 joystick;
	}

	/// <summary>
	/// CKey1・キー入力クラス から、ジョイスティックサポートを取り除いたもの。
	/// </summary>
	public class CKey3・キー入力クラス : CVirtualKey・複数の入力をひとまとめにする仮想キー管理者 {
		/// <summary>
		/// 
		/// </summary>
		public CKey3・キー入力クラス() {
			keyboard = new CMouseAndKeyBoardKeyInput・キー入力定義();

			AddDevice(keyboard);

			//	0	:	Escape
			AddVirtualKey(0,0,(int)EKeyCode.ESCAPE);

			//	1	:	Up
			AddVirtualKey(1,0,(int)EKeyCode.LStick8_TenKeyPad8);
			AddVirtualKey(1,0,(int)EKeyCode.UP);

			//	2	:	Down
			AddVirtualKey(2,0,(int)EKeyCode.LStick2_TenKeyPad2);
			AddVirtualKey(2,0,(int)EKeyCode.DOWN);

			//	3	:	Left
			AddVirtualKey(3,0,(int)EKeyCode.LStick4_TenKeyPad4);
			AddVirtualKey(3,0,(int)EKeyCode.LEFT);

			//	4	:	Right
			AddVirtualKey(4,0,(int)EKeyCode.LStick6_TenKeyPad6);
			AddVirtualKey(4,0,(int)EKeyCode.RIGHT);

			//	5	:	Space
			AddVirtualKey(5,0,(int)EKeyCode.SPACE);

			//	6	:	Return
			AddVirtualKey(6,0,(int)EKeyCode.ENTER);
			AddVirtualKey(6,0,(int)EKeyCode.KeyCode271_TenKeyPad_KP_ENTER);
			AddVirtualKey(6,0,(int)EKeyCode.LSHIFT);
			AddVirtualKey(6,0,(int)EKeyCode.RSHIFT);
		}

		/// <summary>
		/// 
		/// </summary>
		public CMouseAndKeyBoardKeyInput・キー入力定義 keyboard;
	}

	/// <summary>
	/// CKey2・キー入力クラス から、ジョイスティックサポートを取り除いたもの。
	/// </summary>
	public class CKey4・キー入力クラス : CVirtualKey・複数の入力をひとまとめにする仮想キー管理者 {
		/// <summary>
		/// 
		/// </summary>
		public CKey4・キー入力クラス() {
			keyboard = new CMouseAndKeyBoardKeyInput・キー入力定義();

			AddDevice(keyboard);

			//	0	:	Escape
			AddVirtualKey(0,0,(int)EKeyCode.ESCAPE);

			//	1	:	Up
			AddVirtualKey(1,0,(int)EKeyCode.LStick8_TenKeyPad8);
			AddVirtualKey(1,0,(int)EKeyCode.UP);

			//	2	:	Down
			AddVirtualKey(2,0,(int)EKeyCode.LStick2_TenKeyPad2);
			AddVirtualKey(2,0,(int)EKeyCode.DOWN);

            //	3	:	Left
			AddVirtualKey(3,0,(int)EKeyCode.LStick4_TenKeyPad4);
			AddVirtualKey(3,0,(int)EKeyCode.LEFT);

			//	4	:	Right
			AddVirtualKey(4,0,(int)EKeyCode.LStick6_TenKeyPad6);
			AddVirtualKey(4,0,(int)EKeyCode.RIGHT);

			//	5	:	Space
			AddVirtualKey(5,0,(int)EKeyCode.SPACE);
			AddVirtualKey(5,0,(int)EKeyCode.z);

			//	6	:	Return
			AddVirtualKey(6,0,(int)EKeyCode.ENTER);
			AddVirtualKey(6,0,(int)EKeyCode.KeyCode271_TenKeyPad_KP_ENTER);
			AddVirtualKey(6,0,(int)EKeyCode.LSHIFT);
			AddVirtualKey(6,0,(int)EKeyCode.RSHIFT);
			AddVirtualKey(6,0,(int)EKeyCode.x);

			//	7	:	Button C
			AddVirtualKey(7,0,(int)EKeyCode.c);

			//	8	:	Button A
			AddVirtualKey(8,0,(int)EKeyCode.a);

			//	9	:	Button S
			AddVirtualKey(9,0,(int)EKeyCode.s);

			//	10	:	Button D
			AddVirtualKey(10,0,(int)EKeyCode.d);
		}

		/// <summary>
		/// 
		/// </summary>
		private CMouseAndKeyBoardKeyInput・キー入力定義 keyboard;
	}
}
