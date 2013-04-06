using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    public class CTestStrategyPara・戦略パラメータ
    {
        private int p0_頭の回転速度;
        public int P0_頭の回転速度
        {
            get { return p0_頭の回転速度; }
            set { p0_頭の回転速度 = value; }
        }

        private int p1_知識力;
        public int P1_知識力
        {
            get { return p1_知識力; }
            set { p1_知識力 = value; }
        }

        private int p2_知恵;
        public int P2_知恵
        {
            get { return p2_知恵; }
            set { p2_知恵 = value; }
        }

        public CTestStrategyPara・戦略パラメータ(int _p0_頭の回転速度, int _p1_知識力, int _p2_知恵)
        {
            P0_頭の回転速度 = _p0_頭の回転速度;
            P1_知識力 = _p1_知識力;
            p2_知恵 = _p2_知恵;
        }
        /*
        即決型,
        塾考型,
        標準型,*/
    }
    public class CTestStrategyManager・CPU思考戦略管理者
    {
        // 戦略クラスを持つ
        CTestStrategy・CPU思考戦略 p_CPU思考;
        // 戦略タイプを持つ
        private CTestStrategyPara・戦略パラメータ p_戦略パラメータ;// = CTestStrategyPara・戦略パラメータ.標準型;
        public CTestStrategyPara・戦略パラメータ P0_戦略パラメータ
        {
            get { return p_戦略パラメータ; }
            set { p_戦略パラメータ = value; }
        }

        public static void main()
        {
            CTestStrategyManager・CPU思考戦略管理者 _this = new CTestStrategyManager・CPU思考戦略管理者();
            _this.setStrategyType・戦略タイプをセット(new CTestStrategyPara・戦略パラメータ(0,1,1));
            _this.p_CPU思考.考える();
            _this.setStrategyType・戦略タイプをセット(new CTestStrategyPara・戦略パラメータ(1,1,1));
            _this.p_CPU思考.考える();

        }
        public void setStrategyType・戦略タイプをセット(CTestStrategyPara・戦略パラメータ _StrategyPara)
        {
            P0_戦略パラメータ = _StrategyPara;
            CTestStrategyPara・戦略パラメータ _p = _StrategyPara;
            if (_p.P0_頭の回転速度 >= 1 && _p.P1_知識力 >= 0 && _p.P2_知恵 >= 0) 
            {
                p_CPU思考 = new CTestStrategy1・戦略即決();
            }
            else if (_p.P0_頭の回転速度 < 1 && _p.P1_知識力 >= 1 && _p.P2_知恵 >= 1)
            {
                p_CPU思考 = new CTestStrategy2・戦略塾考();
            }
            else
            {
                p_CPU思考 = new CTestStrategy・CPU思考戦略();
            }


            // switchでenumとクラス名を対応づけるのは冗長！
            /*
            switch(P0_戦略パラメータ){
                case CTestStrategyPara・戦略パラメータ.標準型:
                    p_CPU思考 = new CTestStrategy・CPU思考戦略();
                    break;
                case CTestStrategyPara・戦略パラメータ.塾考型:
                    p_CPU思考 = new CTestStrategy2・戦略塾考();
                    break;
                case CTestStrategyPara・戦略パラメータ.即決型:
                    p_CPU思考 = new CTestStrategy1・戦略即決();
                    break;
                default:
                    p_CPU思考 = new CTestStrategy・CPU思考戦略();
            }
             */
        }
    }

    // 「考える」を実行するCPU思考戦略クラス
    public class CTestStrategy・CPU思考戦略
    {

        virtual public void 考える()
        {
            Console.WriteLine("普通に考えると，勝てるかなぁ．");
        }
    }
    public class CTestStrategy1・戦略即決 : CTestStrategy・CPU思考戦略
    {
        override public void 考える()
        {
            Console.WriteLine("とりあえず戦ってみたらいいじゃん！");
        }
    }
    public class CTestStrategy2・戦略塾考 : CTestStrategy・CPU思考戦略
    {
        override public void 考える()
        {
            Console.WriteLine("う～ん…，もうちょっと情報を調べてから戦うか決めた方が．");
        }
    }
}
