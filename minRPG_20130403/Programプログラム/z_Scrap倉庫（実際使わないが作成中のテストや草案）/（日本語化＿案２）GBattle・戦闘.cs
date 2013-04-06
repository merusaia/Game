using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /*/// <summary>
    /// 戦闘を管理するクラスです．
    /// </summary>
    public class 戦闘 : CBattleBase・戦闘を管理する基底クラス
    {

    }*/

    /// <summary>
    /// 戦闘を管理するクラスです．
    /// </summary>
    public class テストGBattle・戦闘
    {
        private List<CChara・キャラ> p_charaPlayer・味方キャラ;

        private List<CChara・キャラ> p_charaEnemy・敵キャラ;
        private List<CChara・キャラ> p_charaOther・その他キャラ;
        public static int p_charaPlayer_Index・味方キャラ_主人公ID = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_charaPlayer"></param>
        /// <param name="_charaEnemy"></param>
        /// <param name="_charaOther"></param>
        public テストGBattle・戦闘()
        {
        }

        /// <summary>
        /// 戦闘を開始します．参加するキャラを選択します（戦闘中も変更可能です．）します．// [TODO]CGameManager・ゲーム管理者 _globalDataはどこに引数を入れればよい？
        /// </summary>
        public void startBattle・戦闘開始(CGameManager・ゲーム管理者 _gameData, CChara・キャラ[] _charaPlayer, CChara・キャラ[] _charaEnemy, CChara・キャラ[] _charaOther)
        {
            p_charaPlayer・味方キャラ = new List<CChara・キャラ>(_charaPlayer);
            p_charaEnemy・敵キャラ = new List<CChara・キャラ>(_charaEnemy);
            p_charaOther・その他キャラ = new List<CChara・キャラ>(_charaOther);

            viewBattleWindow・戦闘画面表示();
            initializeChara・キャラ戦闘開始状態初期化();

            // 戦闘を開始します．
            foreach (CChara・キャラ _enemy in p_charaEnemy・敵キャラ)
            {
                _gameData.mメッセージ_ボタン送り(_enemy.name名前() + " があらわれた！\n");
            }

            // 誰が先に攻撃する？
            // 戦闘ターン処理


        }

        private int viewBattleWindow・戦闘画面表示()
        {
            return 0;
        }
        private int initializeChara・キャラ戦闘開始状態初期化()
        {
            return 0;
        }



        

    }
}
