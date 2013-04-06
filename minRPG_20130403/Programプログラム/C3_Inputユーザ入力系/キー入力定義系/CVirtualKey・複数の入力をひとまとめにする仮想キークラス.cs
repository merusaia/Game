using System;
using System.Collections.Generic;

namespace PublicDomain
// 以下、ほとんどはYaneSDKのソースを引用。やねうらお様に感謝しますm(_ _)m

{
	/// <summary>
	/// VirtualKey。仮想キー管理者。ゲームなどで使う抽象的なボタンの追加・削除・状態（押し離しIsPullなど）を管理するクラス。
	/// </summary>
	/// <remarks>
	/// <Para>
	/// 統合キー入力クラス。派生クラスを利用して、
	/// それらをひとまとめにして管理できる。
	/// </Para>
	/// <Para>
	/// たとえば、キーボードの↑キーと、テンキーの８キー、
	/// ジョイスティックの↑入力を、一つの仮想キーとして登録することによって、
	/// それらのどれか一つが入力されているかを、関数をひとつ呼び出すだけで
	/// 判定できるようになる。
	/// </Para>
	/// <Para>
	/// 実際にKey1,CKey2・キー入力クラス,CKey3・キー入力クラス,Key4は、
	/// このクラスの応用事例なので、そちらも参照すること。
	/// </Para>
	/// <Para>
	/// 全体的な流れは、キーデバイスの登録→仮想キーの設定としておいて、
	/// InputしたのちisPress/isPushで判定します。
	/// </Para>
	/// </remarks>
	public class CVirtualKey・複数の入力をひとまとめにする仮想キー管理者 : IKeyInput・入力インタフェースに必要な機能定義クラス {
        /// <summary>
        /// 出来れば、コンストラクタの引数で、登録する仮想キーの最大数（定義するボタンの種類）を入力してください。
        /// デフォルトでは最大88個を定義できるようにしてあります。
        /// </summary>
        public CVirtualKey・複数の入力をひとまとめにする仮想キー管理者()
            :this(VIRTUAL_KEY_MAX)
        {
        }

        /// <summary>
        /// コンストラクタの引数で、登録する仮想キーの最大数（定義するボタンの種類）を入力してください。
        /// </summary>
        public CVirtualKey・複数の入力をひとまとめにする仮想キー管理者(int _VirtualKeyNum_Max)
        {
            // 値が不正なら、初期値を使う
            if (_VirtualKeyNum_Max < 0)
            {
                _VirtualKeyNum_Max = VIRTUAL_KEY_MAX;
            }
            else if (_VirtualKeyNum_Max > VIRTUAL_KEY_MAX)
            {
                _VirtualKeyNum_Max = VIRTUAL_KEY_MAX;
            }
            else
            {
                // 値が不正でなければ、代入
                setVirtualKeyNum_Max(_VirtualKeyNum_Max);
            }

            // デバイスの初期化
            ClearDevice();
            // 仮想キーリストの初期化
            ClearVirtualKeyList();
        }
		/// <summary>
		/// 仮想キーの最大個数。コンストラクタで指定できるが、もし不正な値を入力された場合も考えて、最大88鍵まで（＾＾；。　
        /// （１フレームあたりかなり高速で管理しないといけない場合が多いので、できるだけ少なめに。）
		/// </summary>
		private static int VIRTUAL_KEY_MAX = 88;
        /// <summary>
        /// 仮想コードの定義数（ボタンの種類数）を取得します。
        /// </summary>
        /// <returns></returns>
        public int getVirtualKeyNum() { return p_virtualKeylist.Length; }
        // privateにして、外部から動的には変更できないようにしておく
        private void setVirtualKeyNum_Max(int _max)
        {
            VIRTUAL_KEY_MAX = _max;
        }

        /// <summary>
        /// このDisposeは実際は何もしない。
        /// </summary>
        public void Dispose() { }


        /// <summary>
        /// 仮想キー番号vKeyに対応付けられた、全てのデバイスのkeycodeナンバーをまとめて、リストで取得する。
        /// 
        /// （※デバイス毎にキーコードkeycode_の定義が異なる場合、このメソッドは使えないので注意してください。）
        /// 
        /// vkeyは、仮想キー番号。これは0(入力無し)～VIRTUAL_KEY_MAX
        /// (コンストラクタで指定した値)番まで登録可能。
        /// キーデバイスnDeviceNoは、GetDeviceで指定するナンバーと同じもの。
        /// 返り値keycodeは、そのキーデバイスのkeycodeナンバー（例：　EKeyCodeのint型）。
        /// </summary>
        /// <param name="_vKey"></param>
        /// <returns></returns>
        public List<int> getKeyCodeList_ByVirtualKeyNo(int _vKey)
        {
            List<int> _keycodes = new List<int>();
            // これはvKey=0～VIRTUAL_KEY_MAX全部のを取る時
            //foreach (List<CVirtualKeyInfo・デバイス番号とキーコード情報> _oneDeviceVKeylist in p_virtualKeylist)
            //{
            //      foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 _info in _oneDeviceVKeylist)
            //      {
            //          ...

            // 指定したvKeyのキーコードだけでいい時
            List<CVirtualKeyInfo・デバイス番号とキーコード情報> _oneDeviceVKeylist = null;
            if(_vKey <= p_virtualKeylist.Length - 1)
            {
                _oneDeviceVKeylist = p_virtualKeylist[_vKey];
            }
            if(_oneDeviceVKeylist != null){
                foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 _info in _oneDeviceVKeylist)
                {
                    // キーコードが無入力（_NONE）じゃない。
                    if (_info.keycode != 0) // != EKeyCode._NONE)
                    {
                        // デバイスに関係なく、キーコードを取得（デバイス毎にキーコードkeycode_の定義が異なる場合はこれが使えないので注意してください）。
                        // 既に追加されてないキーコードだったら
                        if (_keycodes.Contains(_info.keycode) == false)
                        {
                            // 追加
                            _keycodes.Add(_info.keycode);
                        }
                    }
                }
            }
            return _keycodes;
        }
        /// <summary>
        /// 全てのデバイス共通に定義されたkeycodeナンバー（例：　EKeyCodeのint型）に対応付けられた、
        /// 仮想キー番号vkeyを取得する。
        /// 
        /// （※デバイス毎にキーコードkeycode_の定義が異なる場合、このメソッドは使えないので注意してください。）
        /// 
        /// vkeyは、仮想キー番号。これは0(入力無し)～VIRTUAL_KEY_MAX
        /// (コンストラクタで指定した値)番まで登録可能。
        /// キーデバイスnDeviceNoは、GetDeviceで指定するナンバーと同じもの。
        /// 返り値keycodeは、そのキーデバイスのkeycodeナンバー（例：　EKeyCodeのint型）。
        /// </summary>
        public int getVirtualKeyNo_ByKeyCode(int _EKeyCode_int)
        {
            int _vkey = 0; // 存在しない人は0(入力無し)
            List<CVirtualKeyInfo・デバイス番号とキーコード情報> _oneDeviceVKeylist;
            for(int i=0; i<p_virtualKeylist.Length; i++)
            {
                _oneDeviceVKeylist = p_virtualKeylist[i];
                foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 _info in _oneDeviceVKeylist)
                {
                    if (_info.keycode == _EKeyCode_int)
                    {
                        // vkeyは_oneDeviceVKeylistのインデックス
                        _vkey = i;
                    }
                }
            }
            return _vkey;
        }

		/// <summary>
		/// デバイスのクリア。
		/// </summary>
		public void	ClearDevice()
		{
			p_aDevice.Clear(); 
		}

		/// <summary>
		/// キーデバイスの登録。
		/// </summary>
		/// <param name="device"></param>
		/// <remarks>
		/// キーデバイスとは、KeyInputBaseの派生クラスのインスタンス。
		/// 具体的にはKeyInput,JoyStick,MidiInputのインスタンスが
		/// 挙げられる。入力したいキーデバイスをまず登録する。
		/// そしてInputを呼び出すことによって、それらのGetStateが呼び出される。
		/// </remarks>
		public void	AddDevice(IKeyInput・入力インタフェースに必要な機能定義クラス device)
		{	///	デバイスの登録
	 		p_aDevice.Add(device);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="device"></param>
		public void RemoveDevice(IKeyInput・入力インタフェースに必要な機能定義クラス device)
		{	///	デバイスの削除
			p_aDevice.RemoveAll(delegate(IKeyInput・入力インタフェースに必要な機能定義クラス b) { return b == device; });
		}

		/// <summary>
		/// ｎ番目に登録したデバイスの取得。
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		/// <remarks>
		/// この関数を使えばｎ番目（０から始まる）のaddDeviceしたデバイスを
		/// 取得できる。）
		/// </remarks>
		public IKeyInput・入力インタフェースに必要な機能定義クラス GetDevice(int n)
		{
		 	return p_aDevice[n] as IKeyInput・入力インタフェースに必要な機能定義クラス;
		}

		//	仮想キーの追加・削除
		/// <summary>
		/// 仮想キーのリセット。
		/// </summary>
		public void ClearVirtualKeyList()
		{
            // 仮想キーリストを初期化
            p_virtualKeylist = new List<CVirtualKeyInfo・デバイス番号とキーコード情報>[VIRTUAL_KEY_MAX + 1];
            for (int i = 0; i < p_virtualKeylist.Length; ++i)
            {
                p_virtualKeylist[i] = new List<CVirtualKeyInfo・デバイス番号とキーコード情報>();
            }
            // 0番は入力無しと定義し、1～VIRTUAL_KEY_MAX番までを定義可能とする。
            int _KEYCODE_NONE = 0; // =EKeyCode._NONE;
            AddVirtualKey(0, 0, _KEYCODE_NONE);
		}

		/// <summary>
		/// 仮想キーの追加。
        /// 
        /// vkeyは、仮想キー番号。これは0(入力無し)～VIRTUAL_KEY_MAX
        /// (コンストラクタで指定した値)番まで登録可能。
        /// キーデバイスnDeviceNoは、GetDeviceで指定するナンバーと同じもの。
        /// keycodeは、そのキーデバイスのkeycodeナンバー（例：　EKeyCodeのint型）。
		/// </summary>
		/// <param name="vkey"></param>
		/// <param name="nDeviceNo"></param>
		/// <param name="_keycode"></param>
		public void AddVirtualKey(int vkey,int nDeviceNo,int keycode) {
			CVirtualKeyInfo・デバイス番号とキーコード情報 k = new CVirtualKeyInfo・デバイス番号とキーコード情報(nDeviceNo,keycode);
            if (p_virtualKeylist[vkey].Contains(k) == false) // 既に含まれていなければ（merusaia追記）
            {
                p_virtualKeylist[vkey].Add(k);
            }
		}

		/// <summary>
		/// 仮想キーの削除。
		/// </summary>
		/// <param name="vkey"></param>
		/// <param name="nDeviceNo"></param>
		/// <param name="_keycode"></param>
		/// <remarks>
		/// vkeyは、仮想キー番号。これは0(入力無し)～VIRTUAL_KEY_MAX
		/// (コンストラクタで指定した値)番まで登録可能。
		/// キーデバイスnDeviceNoは、GetDeviceで指定するナンバーと同じもの。
		/// keycodeは、そのキーデバイスのkeycodeナンバー（例：　EKeyCodeのint型）。
		/// </remarks>
		public void RemoveVirtualKey(int vkey,int nDeviceNo,int keycode) {
			p_virtualKeylist[vkey].RemoveAll(delegate(CVirtualKeyInfo・デバイス番号とキーコード情報 k){
				return  (k.deviceNo == nDeviceNo && k.keycode == keycode);
			});
		}

		//	----	overriden from KeyInputBase	 ------

		/// <summary>
        /// 押し初めにTrue。前回のupdateのときに押されていなくて、今回のupdateで押されたか。
        /// 
        /// ボタン ：-------___________________----
        /// isPush : 000000010000000000000000000000　離し押した瞬間か　→　押し初めか
        /// 
        ///     /// ■（一般的なコンシューマゲームのボタン押し判定はisPull（押し離した瞬間true）、
        /// シューティングなど連射対応型はPress（押してる間true）だと、思います。）
        /// 詳細な違いはIKeyInputクラスを参照してください。
        /// </summary>
		/// <param name="vKey"></param>
		/// <returns></returns>
		public bool IsPush(int vKey)
		{
			foreach(CVirtualKeyInfo・デバイス番号とキーコード情報 k in p_virtualKeylist[vKey]){
				IKeyInput・入力インタフェースに必要な機能定義クラス kb = p_aDevice[k.deviceNo];
				if (kb.IsPush(k.keycode)) return true;
			}
			return false;
		}
        /// <summary>
        /// どれか一つでもボタンが押し初めされたらTrue。
        /// 
        /// 押し初めにTrue。前回のupdateのときに押されていなくて、今回のupdateで押されたか。
        /// 
        /// ボタン ：-------___________________----
        /// isPush : 000000010000000000000000000000　離し押した瞬間か　→　押し初めか
        /// 
        ///     /// ■（一般的なコンシューマゲームのボタン押し判定はisPull（押し離した瞬間true）、
        /// シューティングなど連射対応型はPress（押してる間true）だと、思います。）
        /// 詳細な違いはIKeyInputクラスを参照してください。
        /// </summary>
        /// <param name="vKey"></param>
        /// <returns></returns>
        public bool IsPush()
        {
            for (int vKey = 0; vKey < ButtonNum; vKey++)
            {
                foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 k in p_virtualKeylist[vKey])
                {
                    IKeyInput・入力インタフェースに必要な機能定義クラス kb = p_aDevice[k.deviceNo];
                    if (kb.IsPush(k.keycode)) return true;
                }
            }
            return false;
        }




		/// <summary>
        /// 押してる間はTrue。現在押されているか(状態はupdate関数を呼び出さないと更新されない)
        /// 
        /// ボタン ：-------___________________----
        /// isPress: 000000011111111111111111110000　押してる間はTrue　→　押し中か
        /// </summary>
		/// <param name="vKey"></param>
		/// <returns></returns>
		public bool IsPress(int vKey)
		{
			foreach(CVirtualKeyInfo・デバイス番号とキーコード情報 k in p_virtualKeylist[vKey]){
				IKeyInput・入力インタフェースに必要な機能定義クラス kb = p_aDevice[k.deviceNo];
				if (kb.IsPress(k.keycode)) return true;
			}
			return false;
		}

        /// <summary>
        /// どれか一つでもボタンが押してる間はTrue。
        /// 
        /// 押してる間はTrue。現在押されているか(状態はupdate関数を呼び出さないと更新されない)
        /// 
        /// ボタン ：-------___________________----
        /// isPress: 000000011111111111111111110000　押してる間はTrue　→　押し中か
        /// </summary>
        /// <param name="vKey"></param>
        /// <returns></returns>
        public bool IsPress()
        {
            for (int vKey = 0; vKey < ButtonNum; vKey++)
            {
                foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 k in p_virtualKeylist[vKey])
                {
                    IKeyInput・入力インタフェースに必要な機能定義クラス kb = p_aDevice[k.deviceNo];
                    if (kb.IsPress(k.keycode)) return true;
                }
            }
            return false;
        }



		/// <summary>
        /// 押し離した瞬間にTrue。前回のupdateのときに押されていて、今回のupdateで押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isPull : 000000000000000000000000001000　押し離した瞬間か　→　押し離したか
        /// </summary>
		/// <remarks>
		/// ひとつのdeviceでもPullであればtrue
		/// </remarks>
		/// <returns></returns>
		public bool IsPull(int vKey)
		{
			foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 k in p_virtualKeylist[vKey])
			{
				IKeyInput・入力インタフェースに必要な機能定義クラス kb = p_aDevice[k.deviceNo];
				if (kb.IsPull(k.keycode)) return true;
			}
			return false;
		}
        /// <summary>
        /// どれか一つでもボタンが押し離されたらTrue。
        /// 
        /// 押し離した瞬間にTrue。前回のupdateのときに押されていて、今回のupdateで押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isPull : 000000000000000000000000001000　押し離した瞬間か　→　押し離したか
        /// </summary>
        /// <remarks>
        /// ひとつのdeviceでもPullであればtrue
        /// </remarks>
        /// <returns></returns>
        public bool IsPull()
        {
            for (int vKey = 0; vKey < ButtonNum; vKey++)
            {
                foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 k in p_virtualKeylist[vKey])
                {
                    IKeyInput・入力インタフェースに必要な機能定義クラス kb = p_aDevice[k.deviceNo];
                    if (kb.IsPull(k.keycode)) return true;
                }
            }
            return false;
        }




		/// <summary>
		/// 前回のupdateのときに押されていなくて、今回のupdateでも押されていない。
		/// </summary>
		/// <remarks>
		/// ひとつのdeviceでもFreeであればtrue
		/// </remarks>
		/// <returns></returns>
		public bool IsFree(int vKey)
		{
			foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 k in p_virtualKeylist[vKey])
			{
				IKeyInput・入力インタフェースに必要な機能定義クラス kb = p_aDevice[k.deviceNo];
				if (kb.IsFree(k.keycode)) return true;
			}
			return false;
		}
        /// <summary>
        /// 一つでもボタンが離されていたらtrue。全てのボタンが押されていたらfalse（複数デバイスを使えばできるかもしれないが、登録ボタン数にもよるが厳しいだろう）
        /// 
        /// 前回のupdateのときに押されていなくて、今回のupdateでも押されていない。
        /// </summary>
        /// <remarks>
        /// ひとつのdeviceでもFreeであればtrue
        /// </remarks>
        /// <returns></returns>
        public bool IsFree()
        {
            for (int vKey = 0; vKey < ButtonNum; vKey++)
            {
                foreach (CVirtualKeyInfo・デバイス番号とキーコード情報 k in p_virtualKeylist[vKey])
                {
                    {
                        IKeyInput・入力インタフェースに必要な機能定義クラス kb = p_aDevice[k.deviceNo];
                        if (kb.IsFree(k.keycode)) return true;
                    }
                }
            }
            return false;
        }
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string DeviceName { get { return "VirtualKeyInput"; } }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int ButtonNum { get { return p_virtualKeylist.Length; } }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IntPtr Info { get { return IntPtr.Zero; } }

		/// <summary>
		/// 登録しておいたすべてのデバイスのupdateを呼び出す。
		/// </summary>
		public void Update() { foreach(IKeyInput・入力インタフェースに必要な機能定義クラス e in p_aDevice) e.Update(); }


		/// <summary>
        /// １つの仮想キー番号に割り当てられた仮想キー情報（キーデバイス番号deviceNo_と、キーコードkeycodeナンバーをそれぞれint型で持つ）を管理するクラスです。
		/// </summary>
		protected class CVirtualKeyInfo・デバイス番号とキーコード情報 {
			/// <summary>
            /// この仮想キー番号に割り当てられたキーデバイス番号deviceNo
			/// </summary>
			public int		deviceNo;
			/// <summary>
            /// この仮想キー番号に割り当てられたでデバイスのキーコードkeycodeナンバー
			/// </summary>
			public int		keycode;
			/// <summary>
            /// 引数に、この仮想キー番号に割り当てられたキーデバイス番号deviceNo_と、デバイスのキーコードkeycodeナンバーをそれぞれint型で入力してください。
			/// </summary>
			/// <param name="_deviceNo"></param>
			/// <param name="_keycode"></param>
			public CVirtualKeyInfo・デバイス番号とキーコード情報(int _deviceNo,int _keycode)
            {
                deviceNo = _deviceNo;
                keycode = _keycode;
            }
		}
		/// <summary>
		/// 入力リスト。
		/// </summary>
        protected List<CVirtualKeyInfo・デバイス番号とキーコード情報>[] p_virtualKeylist;
            //定義はコンストラクタで行う= new List<CVirtualKeyInfo・デバイス番号とキーコード情報>[VIRTUAL_KEY_MAX];
		/// <summary>
		/// 入力キーデバイスリスト。
		/// </summary>
		protected List<IKeyInput・入力インタフェースに必要な機能定義クラス> p_aDevice
            = new List<IKeyInput・入力インタフェースに必要な機能定義クラス>();
	}
}
