using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /*
    /// <summary>
    /// ゲーム・シナリオ作成者に，わかりやすい日本語メソッドによる呼び出し方法を提供するクラスです．必要に応じて新しいメソッドを加えてもＯＫです．
    /// </summary>
    public class ゲーム
    {
        private CBattle p_battle = new CBattle();
        private CSoundPlayData・オーディオ再生用クラス p_sound = new CSoundPlayData・オーディオ再生用クラス();

        /// <summary>
        /// 戦闘を開始します．参加するキャラを選択します（戦闘中も変更可能です．）します．// [TODO]CGameManager・ゲーム管理者 _globalDataはどこに引数を入れればよい？
        /// </summary>
        public void 戦闘開始(CGlobalData _ゲームデータ, CChara[] _p次戦闘の味方キャラたち, CChara[] _敵キャラたち, CChara[] _仲介キャラたち)
        {
            beginBattle(_ゲームデータ, _p次戦闘の味方キャラたち, _敵キャラたち, _仲介キャラたち);
        }
        /// <summary>
        /// サウンド（効果音・SE）を再生します．
        /// </summary>
        /// <param name="_fileName_NotFullPath_ファイル名_名前だけ"></param>
        /// <returns></returns>
        public int サウンド再生(SoundName _fileName_NotFullPath_ファイル名_名前だけ)
        {
            return p_sound.playSound(_fileName_NotFullPath_ファイル名_名前だけ);
        }

    }

    /// <summary>
    /// 戦闘を管理するクラスです．
    /// </summary>
    public class CBattle
    {
        private List<CChara> p_charaPlayers;
        private List<CChara> p_charaEmenys;
        private List<CChara> p_charaExras;
        public static int _charaPlayerYou_PlayerID = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_charaPlayer"></param>
        /// <param name="_charaEnemy"></param>
        /// <param name="_charaOther"></param>
        public CBattle()
        {
        }

        /// <summary>
        /// 戦闘を開始します．参加するキャラを選択します（戦闘中も変更可能です．）します．// [TODO]CGameManager・ゲーム管理者 _globalDataはどこに引数を入れればよい？
        /// </summary>
        public void beginBattle(CGlobalData _globalData, CChara[] _charaPlayers, CChara[] _charaEnemys, CChara[] _charaExtras)
        {
            p_charaPlayers = new List<CChara>(_charaPlayers);
            p_charaEmenys = new List<CChara>(_charaEnemys);
            p_charaExras = new List<CChara>(_charaExtras);

            drawBattleView();
            innitializeCharaStatas();

            // 戦闘を開始します．
            foreach (CChara _enemy in charaEmenys)
            {
                _globalData.drawMessage(true, _enemy.getName() + " があらわれた！\n");
            }

            // 誰が先に攻撃する？
            // 戦闘ターン処理


        }

        /// <summary>
        /// 戦闘画面を表示します．
        /// </summary>
        /// <returns></returns>
        public int drawBattleView()
        {
            return 0;
        }
        /// <summary>
        /// 全キャラの戦闘ステータスを初期化します．
        /// </summary>
        /// <returns></returns>
        public int innitializeCharaStatas()
        {
            return 0;
        }

    }
     */
}
