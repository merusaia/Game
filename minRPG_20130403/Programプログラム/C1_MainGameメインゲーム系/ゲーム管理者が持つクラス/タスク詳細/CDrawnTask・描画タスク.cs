using System;
using System.Collections.Generic;
using System.Text;

using Yanesdk.System;
using System.Drawing;
using Yanesdk.Draw;

namespace PublicDomain
{
    
    #region 描画タスクのクラス定義
    /// <summary>
    /// 全ての描画タスクのベースクラス。
    /// </summary>
    public abstract class CDrawTaskBase : CTaskBase・タスクベース
    {
	    public override int Task(object o)
	    {
		    CGameManager・ゲーム管理者 _g = (CGameManager・ゲーム管理者)o;
		    if (!_g.getP_fpsManager・フレーム管理者().ToBeSkip)
		    {
			    return Task(_g);
		    }
		    return 0;
	    }
	    abstract protected int Task(CGameManager・ゲーム管理者 _g);
    }

    /// <summary>
    /// 描画開始タスク
    /// </summary>
    public class CDrawStartTask : CDrawTaskBase
    {
	    protected override int Task(CGameManager・ゲーム管理者 _g)
	    {
		    _g.getP_gameWindow・ゲーム画面().Screen.Select();
		    return 0;
	    }
    }

    /// <summary>
    /// 描画終了タスク
    /// </summary>
    public class CDrawEndTask : CDrawTaskBase
    {
	    protected override int Task(CGameManager・ゲーム管理者 _g)
	    {
		    _g.getP_gameWindow・ゲーム画面().Screen.Update();
            /*
#if DEBUG
		    // ついでにDrawStartTask ～ DrawEndTaskまでの間のがちゃんとDrawTaskBaseを継承してるかチェック。
		    foreach (TaskController.Task _nowTime in _g.TaskController.TaskList)
		    {
			    if ((int)Tasks.DrawStert <= _nowTime.Priority && _nowTime.Priority <= (int)Tasks.DrawEnd)
			    {
				    System.Diagnostics.Debug.Assert(_nowTime.ITaskBase is CDrawTaskBase);
			    }
		    }
#endif
             * */
		    return 0;
	    }
    }
    #endregion

    /// <summary>
    /// サンプル画像を描画するタスクです。
    /// 任意の画像を描画するタスクを作りたい場合は、このクラスを継承してTask(CGameData _g)をオーバーライドしてください。
    /// </summary>
    public class CDrawnTask : CDrawTaskBase
    {
		private GlTexture p_image・描画する画像 = null;
        protected override int Task(CGameManager・ゲーム管理者 _g)
		{
			if (p_image・描画する画像 == null)
			{
				p_image・描画する画像 = new GlTexture();
				p_image・描画する画像.Load("data/back_img.png");
			}
			_g.getP_gameWindow・ゲーム画面().Screen.Blt(p_image・描画する画像, 0, 0);

			return 0;
		}

		public void Dispose()
		{
			p_image・描画する画像.Dispose();
		}

    }




        //以下、自分で作ったがよくわからない・・＾＾；。


        /*
        /// <summary>
    /// タスクとして処理されると同時に，処理後に画面に描画されるタスクです．1タスクにつき1つのイメージを持ちます．
    /// </summary> 
        
        /// <summary>
        /// 画像集合(アニメーション対応)
        /// </summary>
        public List<Image> p_images = new List<Image>();
        /// <summary>
        /// 現在の画像(この画像が描画対象となります)
        /// </summary>
        public Image p_image;
        /// <summary>
        /// アニメーション画像インデックス(現在どのアニメーションを差しているか)
        /// </summary>
        public int p_animeIndex = 0;
        /// <summary>
        /// アニメーション画像待ち時間(何フレーム後にアニメーションを切り替えるか)
        /// </summary>
        public int p_animeWait = 0;
        public int p_workAnimeWait = 0;


        #region 基底クラス（インタフェース）の実装

        ///	<summary>呼び出されるべきタスク。</summary>
        /// <remarks>
        ///	派生クラス側で、これをオーバーライドして使う。
        ///	この引数には、 TaskController.CallTask 呼び出し時に
        ///	渡したパラメータが入る。
        ///
        ///	非0を返せば、このタスクは消される。
        /// </remarks>
        public override int Task(Object o)
        {
            bool _isDeleted = true;


            if (_isDeleted == true)
            {
                return 1; // このタスクは消される
            }
            else
            {
                return 0;
            }
        }

        // // これは、ToStringをオーバーライドすればいいので、消す
        /////	<summary>デバッグ用にタスク名を返す関数。</summary>
        ///// <remarks>
        /////	必要ならばオーバーライドして使うと良い。
        ///// </remarks>
        //public virtual string getTaskName() { return "タスク名 _filename Task"; }
        //
        
        /// <summary>
        /// タスク間で情報を渡したいときに用いる。
        /// </summary>
        /// <remarks>
        /// TaskControllerからは、taskのpriorityを指定すればそのTaskが取得できるので
        /// そのあと、このメソッドを使うと良い。
        ///	</remarks>
        /// <returns></returns>
        public override Object TaskInfo
        {
            get
            {
                return null; // 未実装
            }
        }

        /// <summary>
        /// デバッグ用にこのタスクの情報を返す。必要になれば、オーバーライドして使うと良い。
        /// </summary>
        public override string TaskDebugInfo
        {
            get { return "CDrawnTaskBase : タスク名 ExtraInfo"; }  // 未実装
        }

        #endregion
         * */
}
