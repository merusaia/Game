using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 現在のキャラとＮ次元世界との影響度を表す，心霊パラメータです．この心霊パラメータが特定の心霊に近くなれば憑依します．なお，それぞれの次元パラメータ(例えば5次元)が100になると次の次元(6次元)が増加し，0になると前の次元(4次元)が増加します．
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CShinreiPara・心霊パラメータ
    {

        // 0次元～11次元まで
        List<double> p_ｚigen・次元 = new List<double>(12);
        public static int p_zigenSize・次元の最大サイズ = 11;
        /*
        　【次元数（人が抱く気持ちを表現する漢字１文字）：次元の名前：　象徴する言葉（複数可）　＜到達者・墜落者＞　（状態を表現する漢字一文字）　個-全，分-合，開-閉，動-静，流れ】
         *
　　・10(然) ：自然：　（光）創造や（闇）破壊を，秩序や混沌により統括　＜大自然＞　　　　　
　　・9（神）：神  ： 創造-破壊  :存在感，威圧感，畏怖，美，エネルギー　＜コスモス・カオス＞　
　　・8（愛）：博愛：　慈悲心: 人類愛，優しさ-厳しさ，倫理，絶望　 　 ＜マザー・魔王＞　　　　　
　　・7（救）：恋愛：　貢献心: 正義-邪悪，主張，（救済），（普及）　　＜勇者・ドラゴン（竜王）＞
　　・6（誠）：仕事：　向上心: 変化，努力，効率　　　 　　　　　　　　＜玄人（天使）・天狗＞　　 
　　・5（和）：平和：　安心: 感謝，安定　 　　　　　 　　　　　　　　 ＜村人・罪人（無気力族）＞
　　・4（想）：妄想：  想像，幽霊　　　　　　　　　　　　　　　       ＜精霊（妖精）・悪魔＞
　　・3（物）：物質：　現実　　　　　　　　　　　 　　　　　　　　　　＜死・物質化（自爆霊）＞　 
　　・2（場）：空間，近い-遠い　　　　　　　　　　　　　　　　　　　  ＜広・狭＞　　　　　　　
　　・1（時）：流れ，早い-遅い　　　  　　　　　　　　　　　　　　　  ＜未来・過去＞　　　　　
　　・0:（boolFalse_無）:　　　　　　　　　　　　　　　　　　　　　　　　　 　　＜boolFalse_無＞　　　　　   　　　
        */

        public CShinreiPara・心霊パラメータ()
        {
            for (int i = 0; i < p_zigenSize・次元の最大サイズ; i++)
            {
                p_ｚigen・次元.Add(0);
            }
        }
        public CShinreiPara・心霊パラメータ(int _無0, int _時1, int _場2, int _物3, int _想4, int _和5, int _誠6, int _救7, int _愛8, int _神9, int _然10, int _宇11)
        {
            int[] _zigen = new int[]{_無0, _時1, _場2, _物3, _想4, _和5, _誠6, _救7, _愛8, _神9,  _然10,  _宇11};
            for (int i = 0; i < p_zigenSize・次元の最大サイズ; i++)
            {
                p_ｚigen・次元.Add(_zigen[i]);
            }
        }

        public List<double> getP_zigen・次元を取得()
        {
            return p_ｚigen・次元;
        }
        private double z次元(int _次元数)
        {
            return MyTools.getListValue(p_ｚigen・次元, _次元数);
        }
        public double z0無()
        {
            return z次元(0);
        }
        public double z1時()
        {
            return z次元(1);
        }
        public double z2場()
        {
            return z次元(2);
        }
        public double z3物()
        {
            return z次元(3);
        }
        public double z4想()
        {
            return z次元(4);
        }
        public double z5和()
        {
            return z次元(5);
        }
        public double z6誠()
        {
            return z次元(6);
        }
        public double z7救()
        {
            return z次元(7);
        }
        public double z8愛()
        {
            return z次元(8);
        }
        public double z9神()
        {
            return z次元(9);
        }
        public double z10然()
        {
            return z次元(10);
        }
        public double z11宇()
        {
            return z次元(11);
        }
    }
}
