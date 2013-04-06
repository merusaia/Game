using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// 特定のパラメータID（EPara.パラメータ名）の初期値を格納しているクラスです。
    /// </summary>
    public class CParasDefault・パラ初期値群
    {
        // プロパティはメモリを消費するから，現段階ではstaticメソッドでやっている．List<EPara> p_defaultValue = new List<EPara>();
        /*
         ※こんな感じでやった方が見やすい？
                    set(EPara.物理攻撃強化定数, 50.0);
                    set(EPara.物理防御強化定数, 25.0);
                    set(EPara.ダメージバランス率, 20);
                    set(EPara.ダメージ集中回数, 1.0);
                    set(EPara.エスカレーション指数係数, 1.0); 
         */
        public static double get(EPara _parameterID)
        {
            double _デフォルト値 = 0.0;
            switch (_parameterID)
            {
                case EPara.物理攻撃強化率:
                    _デフォルト値 = 50.0;
                    break;
                case EPara.物理防御強化率:
                    _デフォルト値 = 25.0;
                    break;
                case EPara.ダメージバランス率:
                    _デフォルト値 = 20.0;
                    break;
                case EPara.ダメージ集中回数:
                    _デフォルト値 = 1.0;
                    break;
                case EPara.エスカレーション指数係数:
                    _デフォルト値 = 1.0;
                    break;
                default:
                    break;
            }
            return _デフォルト値;
        }
    }
}
