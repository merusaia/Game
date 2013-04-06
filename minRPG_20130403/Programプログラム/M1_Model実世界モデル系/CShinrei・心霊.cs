using System;
using System.Collections.Generic;
using System.Text;


namespace PublicDomain
{
    /// <summary>
    /// 人・モノ・コト，あらゆる個体に憑依し，個体の無意識の能力や価値観に影響させる心霊（つまり，日本で言う神々や霊のこと）です．
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CShinrei・心霊
    {
        string p_name・心霊名 = "";
        CParas・パラメータ一覧 p_addPara・憑依追加パラメータ = new CParas・パラメータ一覧();
        List<CSkill・特技> p_addSkill・憑依追加特技 = new List<CSkill・特技>();
        List<CVoice・ボイス> p_addVoice・憑依追加ボイス = new List<CVoice・ボイス>();

        CPositiveNegative・光陰 p_horyDark = new CPositiveNegative・光陰();


        CShinreiPara・心霊パラメータ p_shinreiPara・心霊パラメータ = new CShinreiPara・心霊パラメータ();
        
       
    }
}
