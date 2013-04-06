using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// キャラの能力を管理するクラスです。
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CAbility・能力
    {
        /// <summary>
        /// 装備するものの名前．
        /// </summary>
        private string name・名前 = "";
        public void setName・名前設定(string _name・_名前)
        {
            name・名前 = _name・_名前;
        }
        public string getName・名前取得()
        {
            return name・名前;
        }

        /// <summary>
        /// 装備時の，HP回復量，攻撃力増加量，覚える技，感情変化値などの，パラメータ名と値．
        /// </summary>
        private Dictionary<string, float> parameter・増減パラメータ一覧 = new Dictionary<string, float>();
        public void updwonParameter・パラメータ増減(double _rate・パラメータ増減割合パーセント)
        {
            
        }

        /// <summary>
        /// 装備しているか．
        /// </summary>
        private bool equiped・装備した = false;
        public void setEquiped・装備する(bool _trueOrFalse・_装備するorはずす){
            equiped・装備した = _trueOrFalse・_装備するorはずす;
            if(_trueOrFalse・_装備するorはずす == true){
                // 効果をパラメータに追加
            }else{
                // 効果をパラメータに削減
            }
        }
        public bool isEquiped・装備している(){
            return equiped・装備した;
        }

        public void stopEffect・効果無効になる(){
            // 効果をパラメータに削減
        }

        /// <summary>
        /// 効果をパラメータに適応します．
        /// </summary>
        public void affectParameters・効果をパラメータに適応(float _rate・増減割合パーセント)
        {
            foreach(KeyValuePair<string,float> _para・パラ in parameter・増減パラメータ一覧){
                parameter・増減パラメータ一覧[_para・パラ.Key] *= _rate・増減割合パーセント;
            }
        }

    }
}
