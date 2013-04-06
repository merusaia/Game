using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// あるアトラクター（以下，安定状態と呼ぶ）にするために，
    /// １つのアトラクター選択（以下，妖精さんと呼ぶ（笑））に必要となる
    /// プロパティやメソッドを定義したクラスです．
    /// </summary>
    public class CAtractorSelection・妖精さん
    {
        /// <summary>
        /// 活動しているか（ON/OFF）です．
        /// 　　この妖精さんが少しでも働いているか（true），お休み中か（false）を示します．
        /// </summary>
        private bool p0_isOn・活動しているか = true;
        public bool getP0_isOn() { return p0_isOn・活動しているか; }
        public void setP01_isOn(bool _isOn) { p0_isOn・活動しているか = _isOn; }

        /// <summary>
        /// 活動量（このアトラクター選択の重み係数）です．
        /// 　　この妖精さんがどれくらい働いてるかを示します．
        /// 　　標準は1.0で，範囲は0.0～5.0などです．
        /// 　　時には，あまのじゃくなって，逆方向-5.0～0.0になるのもアリです．
        /// </summary>
        private double p1_activityRate・活動率 = 1.0;
        public double getP1_Activity() { return p1_activityRate・活動率; }
        public void setP1_Activity(double _activityRate) { p1_activityRate・活動率 = _activityRate; }

        /// <summary>
        /// 環境ノイズに影響される率です．
        /// 　　この妖精さんがどれくらい周りの環境に振り回されているかを示します．
        /// 　　標準は1.0で，範囲は0.0～5.0などです．
        /// </summary>
        private double p2_envirronmentNoizeEffectedRate・環境振り回され率 = 1.0;
        public double getP2_envNoize() { return p2_envirronmentNoizeEffectedRate・環境振り回され率; }
        public void setP2_envNoice(double _newValueRate) { p2_envirronmentNoizeEffectedRate・環境振り回され率 = _newValueRate; }
        /// <summary>
        /// 自己ノイズに影響される率です．
        /// 　　この妖精さんがどれくらい自暴自棄になっているかを示します．
        /// 　　標準は1.0で，範囲は0.0～5.0などです．
        /// </summary>
        private double p3_ownNoizeEffectedRate・自暴自棄倍率 = 1.0;
        public double getP3_ownNoize() { return p3_ownNoizeEffectedRate・自暴自棄倍率; }
        public void setP3_ownNoice(double _newValueRate) { p3_ownNoizeEffectedRate・自暴自棄倍率 = _newValueRate; }

        /// <summary>
        /// 環境ノイズの種類です．
        /// </summary>
        private ENoize p4_envirronmentNoizeType・環境ノイズ = ENoize.nw_ホワイトノイズ;
        public ENoize getP4_envNoizeType() { return p4_envirronmentNoizeType・環境ノイズ; }
        public void setP4_envNoiceType(ENoize _ノイズの種類) { p4_envirronmentNoizeType・環境ノイズ = _ノイズの種類; }

        /// <summary>
        /// 自己ノイズの種類です．
        /// </summary>
        private ENoize p5_ownNoizeType・自己ノイズ = ENoize.nw_ホワイトノイズ;
        public ENoize getP5_ownNoizeType() { return p5_ownNoizeType・自己ノイズ; }
        public void setP5_ownNoiceType(ENoize _ノイズの種類) { p5_ownNoizeType・自己ノイズ = _ノイズの種類; }


        /// <summary>
        /// 関数 ΔX= f(X)の働き方を指定可能なデリゲート（好きなメソッドを代入できる述語みたいなもの）です．
        /// 　　入力Xを基に，この妖精さんが安定状態のためにいいと思っている，独自のルール
        ///  　 f(X) を働かせ，単位時間でのXの変化量ΔX（＝dX/dt）を出力します．
        /// </summary>
        /// <param name="_入力"></param>
        /// <returns></returns>
        public delegate CAtractorOut・ΔX functionEvents・働き方(CAtractorIn・X _x);
        private functionEvents・働き方 p_workEvent・働くスケジュール群;

        /// <summary>
        /// コンストラクタで，この妖精さんにせっせと働いてもらう働き方（入力はCAtractorIn，出力はCAtractorOut，の好きなメソッド）を与えます．
        /// </summary>
        /// <param name="_働くスケジュール"></param>
        public CAtractorSelection・妖精さん(functionEvents・働き方 _働くスケジュール)
        {
            p_workEvent・働くスケジュール群 += _働くスケジュール;
        }
        /// <summary>
        /// この妖精さんに，ちょっと悪いけどもう一個せっせと働いてもらう働き方（入力はCAtractorIn，出力はCAtractorOut，の好きなメソッド）を与えます．
        /// </summary>
        /// <param name="_働くスケジュール"></param>
        public void addFunction・働くスケジュールを追加(functionEvents・働き方 _働くスケジュール)
        {
            // 働き方を追加します．
            p_workEvent・働くスケジュール群 += _働くスケジュール;
        }

        /// <summary>
        /// ●アトラクター管理者（世界樹）が頻繁に呼び出す，アトラクター選択（妖精さん）唯一のメソッドです．
        /// 　　この妖精さんの単位時間当たりにせっせと働いた成果を受け取り，世界中の安定に反映させます．
        /// </summary>
        /// <returns></returns>
        public CAtractorOut・ΔX getArtactorOutResult・成果を受け取る(CAtractorIn・X _x)
        {
            CAtractorOut・ΔX _result = new CAtractorOut・ΔX();

            _result = function・働く(_x);

            _result.noize(getP4_envNoizeType(), getP2_envNoize());
            _result.noize(getP5_ownNoizeType(), getP3_ownNoize());

            _result.multiply(getP1_Activity());

            return _result;
        }

        /// <summary>
        /// 関数 ΔX= f(X)です．
        /// 　　入力Xを基に，この妖精さんが安定状態のためにいいと思っている，独自のルール
        ///  　 f(X) を働かせ，単位時間でのXの変化量ΔX（＝dX/dt）を出力します．
        /// </summary>
        /// <param name="_入力"></param>
        /// <returns></returns>
        virtual protected CAtractorOut・ΔX function・働く(CAtractorIn・X _x)
        {
            CAtractorOut・ΔX _fx = new CAtractorOut・ΔX();

            // dt/dx = f(x)・activity + noize

            // [TODO]ここに，f(x)を決める式を書いてください．

            return _fx;
        }



        //[MEMO][SHORTCUT][登場セリフ]複数行コメントはCtrl+K+C　→　解除Ctrl+K+U

        ///// <summary>
        ///// 関数 ΔX= f(X)です．
        ///// 　　入力Xを基に，この妖精さんが安定状態のためにいいと思っている，独自のルール
        /////  　 f(X) を働かせ，単位時間でのXの変化量ΔX（＝dX/dt）を出力します．
        ///// </summary>
        ///// <param name="_入力"></param>
        ///// <returns></returns>
        //virtual public CAtractorOut・ΔX function・働く(CAtractorIn・X _x)
        //{
        //    CAtractorOut・ΔX _normalDeltaX = new CAtractorOut・ΔX();

        //    // dt/dx = f(x)・activity + noize

        //    // ここに，_normalDeltXが受けるnoize（環境，自己の両方）の影響を加える処理を書いてください．

        //    CAtractorOut・ΔX _outDeltaX = _normalDeltaX.multiply(getP1_Activity());
        //    return _outDeltaX;
        //}
    }
}
