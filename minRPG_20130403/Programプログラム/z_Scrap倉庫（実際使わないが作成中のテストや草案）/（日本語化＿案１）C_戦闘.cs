using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /*

    /// <summary>
    /// 戦闘を管理するクラスです．
    /// </summary>
    public class C_戦闘
    {
        private List<C_キャラ> p_味方キャラたち;

        private List<C_キャラ> p_敵キャラたち;
        private List<C_キャラ> p_仲介キャラたち;
        public static int p_味方キャラ_主人公ID = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_charaPlayer"></param>
        /// <param name="_charaEnemy"></param>
        /// <param name="_charaOther"></param>
        public C_戦闘()
        {
        }

        /// <summary>
        /// 戦闘を開始します．参加するキャラを選択します（戦闘中も変更可能です．）します．// [TODO]CGameManager・ゲーム管理者 _globalDataはどこに引数を入れればよい？
        /// </summary>
        public void begin_戦闘(C_グローバルタスク受け渡し通信データ _ゲームデータ, C_キャラ[] _p次戦闘の味方キャラたち, C_キャラ[] _敵キャラたち, C_キャラ[] _仲介キャラたち)
        {
            p_味方キャラたち = new List<C_キャラ>(_p次戦闘の味方キャラたち);
            p_敵キャラたち = new List<C_キャラ>(_敵キャラたち);
            p_仲介キャラたち = new List<C_キャラ>(_仲介キャラたち);

            draw_戦闘画面表示();
            do_キャラ戦闘開始状態初期化();

            // 戦闘を開始します．
            foreach (C_キャラ _chara in p_敵キャラたち)
            {
                _ゲームデータ.drawメッセージ表示(true, _chara.名前() + " があらわれた！\n");
            }

            // 誰が先に攻撃する？
            // 戦闘ターン処理


        }

        private int draw_戦闘画面表示()
        {
            return 0;
        }
        private int do_キャラ戦闘開始状態初期化()
        {
            return 0;
        }



        

    }
     * */
}
