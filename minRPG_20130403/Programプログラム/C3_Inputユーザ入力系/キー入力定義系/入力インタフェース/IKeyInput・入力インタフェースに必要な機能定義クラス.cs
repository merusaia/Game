using System;

namespace PublicDomain
// 以下、ほとんどはYaneSDKのソースを引用。やねうらお様に感謝しますm(_ _)m

{
	/// <summary>
	/// 入力用の基底クラス。抽象クラス（インタフェース）なので直接は使用しない。継承（抽象メソッドを実装）して使う
    /// 
    /// 【参考】isFree/isPress/isPushの定義と意味。
    /// 
    /// ボタン ：-------___________________----
    ///                ↑isPushは一回だけ  ↑isPullは一回だけ1
    /// 状態　 : isFree  isPressはずっと1    isFreはisPullの時だけ0で、以降ずっと1
    /// isFree : 111111100000000000000000000111  離してる間はTrue　→　離し中か
    /// isPress: 000000011111111111111111110000　押してる間はTrue　→　押し中か
    /// isPush : 000000010000000000000000000000　離し押した瞬間か　→　押し初めか
    /// isPull : 000000000000000000000000001000　押し離した瞬間か　→　押し離したか
	/// </summary>
	/// <remarks>
	/// 軸入力もボタンとして扱う。
	/// 	↑:0　↓:1　←:2　→:3
	/// 	1つ目のボタン:4  2つ目のボタン:5  3つ目のボタン:6..
	/// </remarks>
	public interface IKeyInput・入力インタフェースに必要な機能定義クラス : IDisposable {
		
		/// <summary>
		/// 押してる間はTrue。現在押されているか(状態はupdate関数を呼び出さないと更新されない)
        /// 
        /// ボタン ：-------___________________----
        /// isPress: 000000011111111111111111110000　押してる間はTrue　→　押し中か
		/// </summary>
		/// <param name="nButtonNo">
		/// 軸入力もボタンとして扱う。
		/// 	↑:0　↓:1　←:2　→:3
		/// 	1つ目のボタン:4  2つ目のボタン:5  3つ目のボタン:6..
		/// </param>
		/// <returns></returns>
		bool IsPress(int nButtonNo);

		/// <summary>
		/// 押し初めにTrue。前回のupdateのときに押されていなくて、今回のupdateで押されたか。
        /// 
        /// ボタン ：-------___________________----
        /// isPush : 000000010000000000000000000000　離し押した瞬間か　→　押し初めか
        /// </summary>
		/// <param name="nButtonNo">
		/// 軸入力もボタンとして扱う。
		/// 	↑:0　↓:1　←:2　→:3
		/// 	1つ目のボタン:4  2つ目のボタン:5  3つ目のボタン:6..
		/// </param>
		/// <returns></returns>
		bool IsPush(int nButtonNo);

		/// <summary>
		/// 押し離した瞬間にTrue。前回のupdateのときに押されていて、今回のupdateで押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isPull : 000000000000000000000000001000　押し離した瞬間か　→　押し離したか
		/// </summary>
		/// <param name="nButtonNo">
		/// 軸入力もボタンとして扱う。
		/// 	↑:0　↓:1　←:2　→:3
		/// 	1つ目のボタン:4  2つ目のボタン:5  3つ目のボタン:6..
		/// </param>
		/// <returns></returns>
		bool IsPull(int nButtonNo);

		/// <summary>
		/// 離してる間はTrue。前回のupdateのときに押されていなくて、今回のupdateでも押されていない。
        /// 
        /// ボタン ：-------___________________----
        /// isFree : 111111100000000000000000001111  離してる時はTrue　→　離し中か
		/// </summary>
		/// <param name="nButtonNo">
		/// 軸入力もボタンとして扱う。
		/// 	↑:0　↓:1　←:2　→:3
		/// 	1つ目のボタン:4  2つ目のボタン:5  3つ目のボタン:6..
		/// </param>
		/// <returns></returns>
		bool IsFree(int nButtonNo);

		/// <summary>
		/// 状態を更新する。このメソッドを呼び出さないとisPress,isPushで
		/// 返ってくる値は更新されないので注意。
		/// </summary>
		void Update();

		/// <summary>
		/// デバイス名の取得。
		/// </summary>
		/// <returns></returns>
		string DeviceName { get; } 

		/// <summary>
		/// デバイスのボタンの数。
		/// </summary>
		/// <returns></returns>
		int ButtonNum { get; }

		/// <summary>
		/// デバイスの固有情報を返す。
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// CJoyStickならば、SDL_Joystick*
		/// (SDLのCJoyStickのAPIを直接呼びたいときに)
		/// KeyInputNullDeviceならばならば、null
		/// </remarks>
		IntPtr Info { get; }

		/// <summary>
		/// このデバイスが不要になったときに呼び出す
		/// </summary>
		new void Dispose();
	}

	/// <summary>
	/// 入力系のnull device。
	/// すべてのメソッドは、false/null/0などを返す
	/// </summary>
	public class KeyInputNullDevice : IKeyInput・入力インタフェースに必要な機能定義クラス {
		
		/// <summary>
		/// 未登録のデバイスなので、falseを返します。
		/// </summary>
		/// <param name="nButtonNo"></param>
		/// <returns></returns>
		public bool IsPush(int nButtonNo) { return false; }

		/// <summary>
        /// 未登録のデバイスなので、falseを返します。
		/// </summary>
		/// <param name="nButtonNo"></param>
		/// <returns></returns>
		public bool IsPress(int nButtonNo) { return false; }

		/// <summary>
        /// 未登録のデバイスなので、falseを返します。
		/// </summary>
		/// <param name="nButtonNo"></param>
		/// <returns></returns>
		public bool IsPull(int nButtonNo) { return false; }

		/// <summary>
        /// 未登録のデバイスなので、falseを返します。
		/// </summary>
		/// <param name="nButtonNo"></param>
		/// <returns></returns>
		public bool IsFree(int nButtonNo) { return false; }

		/// <summary>
        /// 未登録のデバイスなので、何もしません
		/// </summary>
		public void Update() { }
		
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string DeviceName { get { return "KeyInputNullDevice"; }  }
		
		/// <summary>
        /// 未登録のデバイスなので、0を返します。
		/// </summary>
		/// <returns></returns>
		public int ButtonNum { get {return 0;} }

		/// <summary>
        /// 未登録のデバイスなので、IntPtr.Zeroを返します。
		/// </summary>
		/// <returns></returns>
		public IntPtr Info { get { return IntPtr.Zero;} }

		/// <summary>
        /// 未登録のデバイスなので、何もしません。
		/// </summary>
		public void Dispose()
		{
		}
	}
}
