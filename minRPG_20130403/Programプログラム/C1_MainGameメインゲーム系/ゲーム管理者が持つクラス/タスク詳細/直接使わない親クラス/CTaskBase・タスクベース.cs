using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// ゲームの最小実行単位であるタスクの親クラスです．
    /// </summary>
    public class CTaskBase・タスクベース : ITaskBase
    {
        // ●タスクの状態一覧のサンプルです。独自のタスクを作る場合は、このクラスを継承してTaskをオーバーライドしてください。
        /*
        EState p_state = EState.停止中;
        /// <summary>
        /// タスクの状態一覧をここに定義します。
        /// </summary>
        public virtual enum EState
        {
            停止中,
            カーソル移動,
            決定,
            キャンセル,
        }
         * */

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public CTaskBase・タスクベース()
        {
        }

        /// <summary>
        /// このタスクを実行します。
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override int Task(object o)
        {
            int _runedTaskState = 0;
            // ●このタスクを実行するサンプルです。独自のタスクを作る場合は、このクラスを継承してTaskをオーバーライドしてください。
            /*
            CGameManager・ゲーム管理者 _g = (CGameManager・ゲーム管理者)o;

            // (a)タスクが適時実行する処理をここに入力してください。
            _g.適時実行する処理();

            // (b)タスクが状態によって変わる場合の処理をここに入力してください。
            switch (p_state)
            {
                case EState.停止中: break;
                case EState.カーソル移動: 選択ボタン移動(_g); break;
                case EState.決定: 選択ボタンの画面へ(_g); break;
                case EState.キャンセル: 前の画面へ(_g); break;
                default: break;
            }
            _runedTaskState = (int)p_state;
             * */
            return _runedTaskState;
        }
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
                return null;
            }
        }


        /// <summary>
        /// デバッグ用にこのタスクの情報を返す。必要になれば、オーバーライドして使うと良い。
        /// </summary>
        public override string TaskDebugInfo
        {
            get { return "タスク名 ExtraInfo"; }
        }

    }

    
}
