using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /// <summary>
    /// ※現在はできればCVars・変数一覧クラスと統合したいため、bool型専用のスイッチ一覧クラスは使っていません。
    /// それぞれの条件や結果のスイッチIDを特定するときに参照するenumです．
    /// </summary>
    public enum ESwitch・スイッチ
    {
        user0＿ゲーム未体験,
        user11＿初心者,
        user12＿中級者,
        user13＿上級者,
        user14＿熟練者,
        game0＿ゲーム中断中,

        // 他にも追加
    }

    /// <summary>
    /// ※現在はできればCVars・変数一覧クラスと統合したいため、bool型専用のスイッチ一覧クラスは使っていません。
    ///
    /// ゲーム中のシナリオ分岐の条件や結果の格納などに使う，
    /// ある文字列をキーにしてOn/Offだけの切り替えができるスイッチです．
    /// 基本的に、ESwitch列挙体でスイッチ名を指定します。
    /// </summary>
    public class CSwitchs・スイッチ一覧
    {
        /// <summary>
        /// 各スイッチIDのOn/Offを格納するスイッチ
        /// </summary>
        public Dictionary<string,bool> p_switchDictionary = new Dictionary<string,bool>();

        /// <summary>
        /// 指定したスイッチ名をON(true)にします．既にONだった場合はtrueを返します．
        /// </summary>
        /// <param name="_switchName"></param>
        /// <returns></returns>
        public bool On(string _switchName){
            bool _isAllreadyOn = false;
            bool _OningSwitch = p_switchDictionary[_switchName];
            if (_OningSwitch == true)
            {
                _isAllreadyOn = true;
            }
            else
            {
                p_switchDictionary[_switchName] = true;
            }
            return _isAllreadyOn;
        }
        /// <summary>
        /// 指定したスイッチ名をOFF(false)にします．既にOFFだった場合はtrueを返します．
        /// </summary>
        /// <param name="_switchName"></param>
        /// <returns></returns>
        public bool Off(string _switchName){
            bool _isAllreadyOff = false;
            bool _OffingSwitch = p_switchDictionary[_switchName];
            if (_OffingSwitch == false)
            {
                _isAllreadyOff = true;
            }
            else
            {
                p_switchDictionary[_switchName] = false;
            }
            return _isAllreadyOff;
        }

        /// <summary>
        /// コンストラクタで，全てのスイッチの初期化をします（falseにします）．
        /// </summary>
        public CSwitchs・スイッチ一覧(){
            // EnumをキーにしたDictionaryの値の初期化
            bool _defaultValue = false;
            //ESwitch・スイッチ _enum = new ESwitch・スイッチ();

            List<int> _typeArray = MyTools.getEnumValueList<ESwitch・スイッチ>();
            List<string> _nameArray = MyTools.getEnumNameList<ESwitch・スイッチ>();
            foreach(int _oneSwitchID in _typeArray){
                p_switchDictionary.Add(_nameArray[_oneSwitchID], _defaultValue);
            }
        }
        /// <summary>
        /// コンストラクタで，全てのスイッチデータを上書きロードします（ファイル読み込み）．
        /// </summary>
        public CSwitchs・スイッチ一覧(List<string> _CVSLines_loadedSwitchData)
        {
            Dictionary<string, bool> _LoadedSwitch = new Dictionary<string, bool>();
            string _addedKey;
            bool _addedValue;
            foreach(string _loadedOneSwitch in _CVSLines_loadedSwitchData){
                // "行説明タイトルラベル,キー,値"の一行文字列から，
                // index1（キー）とindex2（値）を格納
                _addedKey = MyTools.getItem_FromCVS(_loadedOneSwitch, 1);
                _addedValue = MyTools.getBool(MyTools.getItem_FromCVS(_loadedOneSwitch, 2));
                _LoadedSwitch.Add(_addedKey, _addedValue);
            }
            // 現在のスイッチデータに代入
            p_switchDictionary.Clear();
            p_switchDictionary = _LoadedSwitch;
        }
    }
}
