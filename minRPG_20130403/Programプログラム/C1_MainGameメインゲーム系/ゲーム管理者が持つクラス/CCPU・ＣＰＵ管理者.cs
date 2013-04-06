using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /// <summary>
    /// ゲームの難易度を列挙したクラスです。CCPU・ＣＰＵ管理者クラスなどが使います。
    /// </summary>
    public enum EGameLV・難易度
    {
        _LVNone_Cosmos・ユーザのプレイ状況に合わせて自動調節,
        LV00_Eden・まさに楽園,
        LV01_VeryEasy・とてもやさしい,
        LV02_Easy・やさしい,
        LV03_Normal・普通,
        LV04_Hard・むずかしい,
        LV05_VeryHard・とてもむずかしい,
        LV06_God・神々クラス,
        LV10_Kaos・まさにカオス,
    }

    /// <summary>
    /// ゲームのＣＰＵ（ＣＰＵ思考や、味方キャラ・主人公の場合は命令思考などを含む）を管理するクラスです．
    /// ゲームの難易度は、よく使うため、game.p_gameLV・難易度に持たせています。
    /// ゲームの難易度は、EGameLV・難易度の列挙体で指定してください。
    /// </summary>
    public class CCPU・ＣＰＵ管理者
    {

        //CVars・変数一覧 _m_var = new CVars・変数一覧();
        CParas・パラメータ一覧 _m_para = new CParas・パラメータ一覧();

    }
}
