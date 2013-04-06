using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
        /// <summary>
        /// ゲーム本体。
        /// ゲームなタスクの生成や、BGMの処理を行う。
        /// </summary>
    public class CDiceBattleTask・ダイスバトルタスク : CTaskBase・タスクベース, IDisposable
    {

        private CSoundPlayData・オーディオ再生用クラス bgm = new CSoundPlayData・オーディオ再生用クラス(-1);
        private bool p_isInitialized = false;

        /// <summary>
        /// 初期化。
        /// </summary>
        public CDiceBattleTask・ダイスバトルタスク()
        {
            // テスト
            bgm.Load・ロード("森の奥の神殿.mp3", -1, "");
            //bgm.Load("data/abyss.ogg", -1);
            //bgm.Loop = -1;
            bgm.Play();
        }

        /// <summary>
        /// 後始末。
        /// </summary>
        public void Dispose()
        {
            bgm.Dispose();
        }

        /// <summary>
        /// 1フレーム毎に逐次実行される処理（いわゆるマルチスレッド処理の代わりに、メインスレッドがラウンドロビンをしてくれるタスク処理？）です。
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override int Task(object o)
        {
            /*
             *                 CGameManager・ゲーム管理者 _g = (GameInfo)o;
            // 初期化
            if (!p_isInitialized)
            {
                // コンフィグファイルが読み込めてないならどうしようもないので終わってしまう。
                if (!_g.Config.IsReady)
                {
                    // エラーメッセージくらいは出す方がいいかも…。

                    _g.SceneController.ReturnScene();
                    return 1;
                }

                Task.PlayerTask playerTask = new Task.PlayerTask(_g);
                _g.TaskController.AddTask(playerTask, (int)Tasks.Player);
                _g.TaskController.AddTask(new Task.DrawBgTask(), (int)Tasks.DrawBg);
                _g.TaskController.AddTask(new Task.DrawPlayerTask(playerTask), (int)Tasks.DrawPlayer);

                p_isInitialized = true;
            }

            // Playerが死んでたら、シーンコントローラに終わりを告げる。
            Task.PlayerTask player = _g.TaskController.GetTask((int)Tasks.Player) as Task.PlayerTask;
            if (player == null || player.State == NQPNet.Task.PlayerTask.States.GameOver)
            {
                // 追加したタスクは一応削除しておく。
                _g.TaskController.KillTask((int)Tasks.DrawPlayer);
                _g.TaskController.KillTask((int)Tasks.DrawBg);
                _g.TaskController.KillTask((int)Tasks.Player);

                _g.SceneController.ReturnScene();
                return 1;
            }
            */
            return 0;

        }
    }
}
