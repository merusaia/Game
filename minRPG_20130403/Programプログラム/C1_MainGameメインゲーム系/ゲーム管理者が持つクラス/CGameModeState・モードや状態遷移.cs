using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// ゲームの大雑把なモードを列挙したものです。使いたいものだけ使って、全部使わなくてもＯＫです。
    /// </summary>
    public enum EMode・ゲームモード{
        m0_Title・タイトル画面,
        m1_MainMenu・メインメニュー,
        m2_Scinario・シナリオモード,
        m3_Battle・戦闘モード,
        m4_Field・フィールドモード,
        mo_Others・その他,
    }
    /// <summary>
    /// ゲームの詳細な状態遷移や各モードで遷移する状態遷移を列挙したものです。使いたいものだけ使って、全部使わなくてもＯＫです。
    /// </summary>
    public enum EState・状態遷移{
        s0_None・不明,
        s1_InputWaiting・入力受付中,
        s2_NoInputWaiting・待ち時間中,
        s2_Loading・ロード中,
        s3_Saving・セーブ中,
        s4_Porse・ポーズ中,
        s5_Skip・スキップ中,
        so_Others・その他,

        t1_タイトル画面で待ち時間中,
        t2_タイトル画面で入力受付中,
        t3_タイトル画面でロード中,
        t4_メインメニューに画面遷移中,

        // ...
    }

    /// <summary>
    /// 大雑把なゲームモードや、画面遷移などの詳細な状態遷移などを持つクラスです。
    /// </summary>
    public class CGameModeState・モードや状態遷移
    {
        private EMode・ゲームモード p_Mode・ゲームモード = EMode・ゲームモード.m0_Title・タイトル画面;
        public EMode・ゲームモード getP_Mode・ゲームモードを取得() { return p_Mode・ゲームモード; }
        public void setP_Mode・ゲームモードを設定(EMode・ゲームモード _mode) { p_Mode・ゲームモード = _mode; }
        
        private EState・状態遷移 p_State・モード毎の状態遷移 = EState・状態遷移.s0_None・不明;
        public EState・状態遷移 getP_State・状態遷移の取得() { return p_State・モード毎の状態遷移; }
        public void setP_State・状態遷移の設定(EState・状態遷移 _state) { p_State・モード毎の状態遷移 = _state; }
    }
}
