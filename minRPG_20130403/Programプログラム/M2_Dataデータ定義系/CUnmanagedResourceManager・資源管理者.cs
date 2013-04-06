using System;
using System.Collections.Generic;
using System.Text;
using Yanesdk.Ytl;

namespace PublicDomain
{

	/// <summary>
	/// Unmanaged Resourceの利用上限をここで設定する。
	/// ここで設定した以上にはUnmanaged Resourceを使わない。
	/// (値がぴったりとは限らないので過信はしないように。)
	/// </summary>
	public class CUnmanagedResourceManager・資源管理者
	{
		/// <summary>
		/// new禁止(Instance経由でアクセスすること)
		/// </summary>
		/// <remarks>
		/// こいつはprivateにしておきたいのだが、Singletonを返す都合でprivateにできない。
		/// </remarks>
        public CUnmanagedResourceManager・資源管理者()
		{
			this.VideoMemory.LimitSize = 32 * 1024 * 1024;
			this.SoundMemory.LimitSize = 64 * 1024 * 1024;
			this.UnmanagedMemory.LimitSize = 128 * 1024 * 1024;
		}

		/// <summary>
		/// singleton instance
		/// </summary>
        public static CUnmanagedResourceManager・資源管理者 Instance
		{
			get { return Singleton<CUnmanagedResourceManager・資源管理者>.Instance; }
		}

		/// <summary>
		/// ビデオメモリ用のcache system
		/// cache limit は defaultで32MB
		/// </summary>
		public CCacheSystem<ICachedObject> VideoMemory = new CCacheSystem<ICachedObject>();

		/// <summary>
		/// サウンド再生のためのcache system
		/// cache limit は defaultで64MB(これはメインメモリからさかれるので大き目にとっても
		/// スワップがおきるだけのことでビデオメモリのように劇的にパフォーマンスが落ちることは
		/// 考えにくい)
		/// </summary>
		public CCacheSystem<ICachedObject> SoundMemory = new CCacheSystem<ICachedObject>();

		/// <summary>
		/// unmanaged memoryのためのcache system
		/// Fontなどが利用するunmanaged memory。128MBに設定。
		/// </summary>
		public CCacheSystem<ICachedObject> UnmanagedMemory = new CCacheSystem<ICachedObject>();

	}
}
