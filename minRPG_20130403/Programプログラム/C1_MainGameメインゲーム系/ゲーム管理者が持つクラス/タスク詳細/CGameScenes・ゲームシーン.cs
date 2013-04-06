using System;
using System.Collections.Generic;
using System.Text;
using Yanesdk.System;

namespace PublicDomain
{
    /// <summary>
    /// ゲームシーンの種類（タイトル画面、シナリオ、戦闘、フィールドなどのゲーム中の画面の種類と捉えるとわかりやすい？）
    /// </summary>
    public enum EScenes・シーン
    {
        s01_ダイスバトル,
        s02_戦闘,

        // スタートメニューなど、ゲーム以外の状態を作るときはここに追加する
    }

    /// <summary>
    /// ゲームシーンの種類（タイトル画面、シナリオ、戦闘、フィールドなどのゲーム中の画面の種類と捉えるとわかりやすい？）の生成を管理するクラスです。
    /// </summary>
    public class CGameScenes・ゲームシーン
    {


        /// <summary>
        /// ゲームシーンを生成するファクトリクラスです。
        /// </summary>
        public class CSceneFactory・シーン生成機 : CTaskFactoryBase・タスク生成機ベース<EScenes・シーン>
        {
            /// <summary>
            /// ゲームシーンを生成します。
            /// </summary>
            public override CTaskBase・タスクベース CreateTask(EScenes・シーン name)
            {
                CTaskBase・タスクベース _selectedGameScene = new CTaskBase・タスクベース();
                // ここに、生成するゲームシーンを書いておけば、シーンコントローラで管理できる
                switch (name)
                {
                    case EScenes・シーン.s01_ダイスバトル:
                        _selectedGameScene = new CDiceBattleTask・ダイスバトルタスク();
                        break;
                }

                return _selectedGameScene;
            }
        }
    }
}
