using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 戦闘パラメータを計算するクラスです．
    /// </summary>
    public class CBattleParaCalc・戦闘パラ計算機
    {
        // 身体パラメータによる影響
        static double s_kougekirちからが攻撃力に比例する定数 = 2.0;
        static double s_gard2ちから対ガード率比例定数 = 0.5;

        static double s_hp持久力が最大HPに比例する定数 = 5.0;

        static double s_koudoug行動力対行動ゲージ比例定数 = 1.0;
        static double s_syubi行動力が守備力に比例する定数 = 0.5;

        static double s_kai2素早さが回避率に比例する定数 = 0.5;
        static double s_koudouk素早さ対行動回復量比例定数 = 0.2;
        static double s_kougekis素早さ対攻撃速度比例定数 = 0.2;

        static double s_sp精神力が最大SPに比例する定数 = 3.0;

        static double s_mahour賢さが魔法力に比例する定数 = 2.0;
        static double s_mahoub1賢さが魔法防御力に比例する定数 = 0.5;
        static double s_mahous賢さが魔法速度に比例する定数 = 0.2;

        // 精神パラメータによる影響
        static double s_mei器用さが命中率に比例する定数 = 1.0;
        static double s_kai1器用さが回避率に比例する定数 = 0.2;
        static double s_gard1器用さ対ガード率比例定数 = 0.2;

        static double s_mahoub2適応力が魔法防御力に比例する定数 = 1.0;

        /// <summary>
        /// ■■■[Parameter]基本・精神パラメータ＋補正から，ESPara・戦闘パラメータ（攻撃力など）を計算します．
        /// ※ゲームバランスのための能力計算を司る重要なメソッドです．
        /// </summary>
        private static void Calc・計算(CChara・キャラ _c)
        {
            // 今のところの戦闘21パラメータ：

            // LV，k経験値,HP,SP,行動ゲージ,テンションはここでは計算しない

            // HP
            double _hp = _c.Para(EPara.a2_持久力) * s_hp持久力が最大HPに比例する定数;
            _c.setParaValue(EPara.s03b_最大HP, _hp);

            // SP
            double _sp = _c.Para(EPara.a5_精神力) * s_sp精神力が最大SPに比例する定数;
            _c.setParaValue(EPara.s04b_最大SP, _sp);

            double _attack = _c.Para(EPara.a1_ちから) * s_kougekirちからが攻撃力に比例する定数;
            _c.setParaValue(EPara.s07_攻撃力, _attack);

            double _magic = _c.Para(EPara.a6_賢さ) * s_mahour賢さが魔法力に比例する定数;
            _c.setParaValue(EPara.s08_魔法力, _magic);

            double _difence = _c.Para(EPara.a3_行動力) * s_syubi行動力が守備力に比例する定数;
            _c.setParaValue(EPara.s09_守備力, _difence);

            double _antiMagic = _c.Para(EPara.a6_賢さ) * s_mahoub1賢さが魔法防御力に比例する定数 + _c.Para(EPara.b4_適応力) * s_mahoub2適応力が魔法防御力に比例する定数;
            _c.setParaValue(EPara.s10_魔法防御, _antiMagic);

            double _hit = _c.Para(EPara.b1_器用さ) * s_mei器用さが命中率に比例する定数;
            _c.setParaValue(EPara.s12_クリティカル率, _hit);

            double _dex = _c.Para(EPara.b1_器用さ) * s_kai1器用さが回避率に比例する定数 + _c.Para(EPara.a4_素早さ) * s_kai2素早さが回避率に比例する定数;
            _c.setParaValue(EPara.s11_回避率, _dex);

            double _gard = _c.Para(EPara.b1_器用さ) * s_gard1器用さ対ガード率比例定数 + _c.Para(EPara.a1_ちから) * s_gard2ちから対ガード率比例定数;
            _c.setParaValue(EPara.s13b_ガード率, _gard);
            
            // 行動ゲージ
            double _actionGage = _c.Para(EPara.a3_行動力) * s_koudoug行動力対行動ゲージ比例定数;
            _c.setParaValue(EPara.s20b_最大AP, _actionGage);

            double _actionRecovery = _c.Para(EPara.a4_素早さ) * s_koudouk素早さ対行動回復量比例定数;
            _c.setParaValue(EPara.s20c_AP回復量, _actionRecovery);
            
            // テンション
            double _tention = 100.0; // 未補正
            _c.setParaValue(EPara.s25b_最大テンション, _tention);

            double _attackSpeed = _c.Para(EPara.a4_素早さ) * s_kougekis素早さ対攻撃速度比例定数;
            _c.setParaValue(EPara.s10_速度, _attack);

            double _magicSpeed = _c.Para(EPara.a6_賢さ) * s_mahous賢さが魔法速度に比例する定数;
            _c.setParaValue(EPara.s20_魔法速度, _magicSpeed);

            double _attackGard = 20.0; // 未補正
            _c.setParaValue(EPara.s21_つば迫り合い率, _attackGard);
        }
    }
}
