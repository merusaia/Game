// どちらか一つをコメントアウト
// 乱数のIDとなるメソッド名を名前空間～所属クラス～直属クラス:メソッド名の全てを取る，コメントすれば直属クラス:メソッド名のみ
//#define MethodName_Absolute

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


//namespace merusaia.Nakatsuka.Tetsuhiro.Experiment
namespace PublicDomain
{
    /// <summary>
    /// 過去の乱数を参照可能なランダム生成クラスです．Nextメソッドによる乱数では、その乱数がはじめてであれば生成された乱数をファイルに保存、でなければ過去に生成したものと同じ乱数を呼び出します。
    /// </summary>
    public class CMyRandomManager
    {
        int CalledMethodFrameNum = 2; // Sampleメソッドが，MyRandomの呼び出し元の外部メソッドを見つけるためのスタック番号

        /// <summary>
        /// 発生乱数を記録するために持つ，内部のランダム生成クラスです．
        /// </summary>
        protected MyRandom random;
        /// <summary>
        /// 乱数管理クラスからMyRandomクラスを取ってきます．
        /// </summary>
        /// <returns></returns>
        public MyRandom getRandom()
        {
            return random;
        }
        public void setMyRandom(MyRandom _newRandom)
        {
            random = _newRandom;
        }
        /// <summary>
        /// このクラスが過去に使用した乱数を使うかどうかを背ってします．
        /// </summary>
        private bool isUsedPastRandomNum = true;
        public void setIsUsedPastRandomNum(bool _isUsedPastRandomNums_FromFile)
        {
            isUsedPastRandomNum = _isUsedPastRandomNums_FromFile;
        }
        /// <summary>
        /// 乱数を生成する「クラス:メソッド:乱数発生回数」をキーとして，過去に生成された乱数を格納します．
        /// </summary>
        private Dictionary<string, double> pastRandomNum = new Dictionary<string, double>();
        /// <summary>
        /// 乱数を生成する「クラス:メソッド」をキーとして，過去にメソッドが実行された（同じ処理で乱数が生成された）回数を格納します．
        /// </summary>
        private Dictionary<string, int> pastMethodRandomGeneratedNum = new Dictionary<string, int>();

        public MyFileIO myfileio;
        string writeFilename = "";

        /// <summary>
        /// 引数を無しにすると，乱数生成パターンが毎回ランダムなNext()などで発生させる乱数発生クラスを生成して格納します．また，esetMethodNumを実行すると，クラスメソッドの乱数発生回数に応じて過去と同じ乱数を読み込みます．
        /// </summary>
        public CMyRandomManager()
            :this(false, 0)
        {
        }
        /// <summary>
        /// 引数で，乱数発生クラスの生成パターンが，起動ごとに同じか(true)／異なるか(false)を指定します．Next()などで発生した乱数を格納します．また，esetMethodNumを実行すると，クラスメソッドの乱数発生回数に応じて過去と同じ乱数を読み込みます．
        /// </summary>
        public CMyRandomManager(bool isRandom_Const, int randomSeed)
        {
            if (isRandom_Const == true)
            {
                random = new MyRandom(randomSeed, this); // 引数が定数だと毎回同じ乱数発生 // new MyRandom(this); // 引数無しだと，時刻によってランダム値を決めるSeedを初期化
            }
            else
            {
                random = new MyRandom(this);
            }
            // 保存ディレクトリを作成
            writeFilename = MyTime.getNowTime_Japanese() + ".txt";
            myfileio = new MyFileIO(MyTools.getProjectDirectory() + "/RandomNumList", false, false);//
        }
        /// <summary>
        /// 引数は，同じクラスメソッドを実行するプログラムの，過去の乱数を保存したファイル名です．
        /// </summary>
        /// <param name="pastRandomNumList_Filename"></param>
        public CMyRandomManager(string pastRandomNumList_Filename)
            :this()
        {

            string readString = myfileio.readFile_simple(pastRandomNumList_Filename);
            string[] lines = readString.Split(MyTools._n.ToCharArray());
            string[] words = new string[2];
            // 過去の発生乱数をファイルから読み込んで設定
            foreach (string line in lines)
            {
                words = line.Split(",".ToCharArray());
                pastRandomNum.Add(words[0], Int32.Parse(words[1]));
            }
        }
        /// <summary>
        /// デストラクタで，格納した発生乱数をファイルに保存します．
        /// </summary>
        ~CMyRandomManager()
        {
            List<string> data = new List<string>();
            foreach(KeyValuePair<string,double> _randomnum in pastRandomNum){
                data.Add(_randomnum.Key+","+pastRandomNum[_randomnum.Key].ToString());
            }
            myfileio.writeFile_simple(writeFilename, data);
        }
        /// <summary>
        /// 新しくランダムな値を生成して，保存します．
        /// </summary>
        /// <param name="_minValue"></param>
        /// <param name="_maxValue_equals_EnumIntMax"></param>
        public int getRandomNum(int _minValue, int _maxValue)
        {
            return this.random.Next(_minValue, _maxValue);
        }
        

        /// <summary>
        /// 実行中のクラスメソッドの乱数発生回数を0として扱います．isUsedPastRandomNumがtrueの場合，次からは，過去の同じ「クラスメソッド：乱数発生回数」に応じて発生した乱数を使用します．
        /// </summary>
        public void resetAllMethodRandomGeneratedNum()
        {
            
            //Enumerator pastMethodRandomGeneratedNum.Keys.GetEnumerator();
            foreach(KeyValuePair<string,int> _randomGeneratednum in pastMethodRandomGeneratedNum){
                pastMethodRandomGeneratedNum[_randomGeneratednum.Key] = 0;
            }
        }

        /// <summary>
        /// ランダム値を保持する，内部クラス
        /// </summary>
        public class MyRandom : Random
        {
            /// <summary>
            /// 発生した乱数を格納している乱数管理クラス
            /// </summary>
            public CMyRandomManager rM;
            /// <summary>
            /// 起動ごとに毎回ランダムの生成パターンが変わる乱数発生クラスを作成します（=new Random()と同じ乱数生成）
            /// </summary>
            /// <param name="_usedMyRandomManager"></param>
            public MyRandom(CMyRandomManager _usedMyRandomManager)
                : base()
            {
                rM = _usedMyRandomManager;
            }
            /// <summary>
            /// 決まったランダムの生成パターンを持つ乱数発生クラスを，生成パターンの種類であるseedを設定して生成します（=new Random(int seed)と同じ乱数生成）
            /// </summary>
            /// <param name="_seed"></param>
            /// <param name="_usedMyRandomManager"></param>
            public MyRandom(int _seed, CMyRandomManager _usedMyRandomManager)
                : base(_seed)
            {
                rM = _usedMyRandomManager;
            }
            /// <summary>
            /// 1～100の100通りの乱数を返します．発生した乱数を，説明文をキーとして保存します．
            /// </summary>
            /// <param name="randomNum_name_AndRange">何のための乱数か，日本語で説明してください．</param>
            /// <returns></returns>
            public int NextParsentage・１から１００の乱数発生(string randomNum_Name_Explanation)
            {
                // [Memo][Random][Sample]:RandomクラスのNextメソッドは，引数に関係なくSampleメソッドを呼び出すので，共通の処理はSampleメソッドに書く！
                //これだとSample()での呼び出し元がNextParsentage()になってしまう．double randomNum = (double)(base.Next(1, 100)) / 100.0; // 親クラスのSampleメソッドだが，インスタンスの型がMyRandomなので，結果的にこのクラスのSampleメソッドを呼び出す
                int randomNum = (int)(this.Sample() * 100.0)+1; // 親クラスのSampleメソッドだが，インスタンスの型がMyRandomなので，結果的にこのクラスのSampleメソッドを呼び出す
                if (randomNum == 101)
                {
                    randomNum = 100;
                }
                return randomNum;
            }
            /// <summary>
            /// 0.0001～1.0000（0.01～100％）の10000通りの乱数を返します．発生した乱数を，説明文をキーとして保存します．
            /// </summary>
            /// <param name="randomNum_name_AndRange">何のための乱数か，日本語で説明してください．</param>
            /// <returns></returns>
            public double NextParsentage・１万通りの確率パーセントを発生(string randomNum_Name_Explanation)
            {
                // [Memo][Random][Sample]:RandomクラスのNextメソッドは，引数に関係なくSampleメソッドを呼び出すので，共通の処理はSampleメソッドに書く！
                //これだとSample()での呼び出し元がNextParsentage()になってしまう．double randomNum = (double)(base.Next(1, 100)) / 100.0; // 親クラスのSampleメソッドだが，インスタンスの型がMyRandomなので，結果的にこのクラスのSampleメソッドを呼び出す
                double randomNum = (double)(int)(this.Sample()*10000)/10000.0; // 親クラスのSampleメソッドだが，インスタンスの型がMyRandomなので，結果的にこのクラスのSampleメソッドを呼び出す
                return randomNum;

                // [Tools][メソッド名][呼び出し]呼び出しもとのクラス名とメソッド名を取ってくるには，StackTraceクラスのインスタンスstの，GetFrame(0:自分自身のメソッド)や(1:1つ前の呼び出し先)の，GetFrame(0or1).GetMethod().ReflectedType.FullNameやGetMethod().Nameを使う
                /*public string getCalledClassAndMethodName(){
                GetFrame(CalledMethodFrameNum)?? // 呼び出しもとの呼び出し元？
                }
                */
                /* http://www.atmarkit.co.jp/bbs/phpBB/viewtopic.php?topic=43758&forum=7&7
                //自メソッドの第一引数名を取得
                string paramName = (new System.Diagnostics.StackTrace()).GetFrame(0).GetMethod().GetParameters()[0].Name;


                //改良版
                string paramName = System.Reflection.MethodBase.GetCurrentMethod().GetParameters()[0].Name;
                */
                /*
                StackTrace st = new StackTrace(false);
                StackFrame sf = st.GetFrame(rM.CalledMethodFrameNum); // 0がこの自身クラス？，1が呼び出し元？
                string className = sf.GetMethod().ReflectedType.FullName;
                string methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
                string classMethodID = className + "." + methodName;
                //return classMethodID;

                return getRandomNum_readPastOrSave(randomNum, classMethodID);
                */

            }
            #region Nextメソッドを，計算した乱数を格納できるようオーバーライドしたNext関連メソッド（Sample）
            
            // [Tip][Random]NextメソッドがSampleメソッドを呼び出すので，Sampleメソッドだけが乱数格すればよい！
            /// <summary>
            /// その乱数がこのメソッドで初めてであれば0.0～1.0の乱数を発生させて返し，過去に実行したメソッドであれば乱数発生回数と同じ乱数を呼び出して返します．
            /// </summary>
            /// <returns></returns>
            protected override double Sample()
            {

                double randomNum = base.Sample();

                
                StackTrace st = new StackTrace(false);
                StackFrame sf = st.GetFrame(rM.CalledMethodFrameNum); // 0がこの自身クラス？，1が呼び出し元（ここではthis.Next()やNextDoubleなど）,2が更に呼び出し元の外部のメソッド
                string _className = sf.GetMethod().ReflectedType.Name;
#if MethodName_Absolute
                    sf.GetMethod().ReflectedType.FullName;
#endif
                string _methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
                string _classMethodID = _className + "." + _methodName;
                //return classMethodID;

                // 新しく乱数を保存するか，もしくは過去の同じ乱数発生回数の乱数を呼び出して返します．
                return getRandomNum_Save_Or_ReadPastValue(randomNum, _classMethodID);

            }
            #endregion

            /// <summary>
            /// 「クラス：メソッド：乱数発生回数」において，はじめてであれば生成された乱数を保存して返し、でなければ過去に生成したものと同じ乱数を呼び出して返します．
            /// </summary>
            /// <param name="randomNum"></param>
            /// <param name="classMethodID"></param>
            /// <returns></returns>
            private double getRandomNum_Save_Or_ReadPastValue(double _randomNum, string _classMethodID)
            {
                //try(){
                // 過去の乱数使用を許可しているか、していなければ保存のみ
                if (rM.isUsedPastRandomNum == true)
                {
                    // ※発生させたメソッドが始めてであればメソッドを追加、また乱数発生回数に応じた乱数を保存．
                    if (rM.pastMethodRandomGeneratedNum.ContainsKey(_classMethodID) == true)
                    {
                        // 乱数発生回数を増加
                        rM.pastMethodRandomGeneratedNum[_classMethodID]++;

                        // 過去に，同じ乱数発生回数で乱数発生させたか
                        if (rM.pastRandomNum.ContainsKey(_classMethodID + "." + rM.pastMethodRandomGeneratedNum[_classMethodID]) == true)
                        {
                            // ※メソッドと乱数発生回数が既に記録されていれば，過去に生成したものと同じ乱数を呼び出し
                            // b過去の乱数を呼び出して返す
                            return getPastRandomValue(_classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                        }
                        else
                        {
                            // a乱数を保存
                            saveRandomValue(_randomNum, _classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                            return _randomNum;
                        }
                    }
                    else
                    {
                        // クラスメソッドの乱数発生回数を1に初期化
                        rM.pastMethodRandomGeneratedNum.Add(_classMethodID, 1);
                        // a乱数を保存
                        saveRandomValue(_randomNum, _classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                        return _randomNum;
                    }
                }
                else
                {
                    // ※クラスの乱数発生回数を見ずに乱数保存のみ

                    // クラスメソッドの乱数発生回数を保存
                    if (rM.pastMethodRandomGeneratedNum.ContainsKey(_classMethodID) == true)
                    {
                        // クラスメソッドの乱数発生回数を増加
                        rM.pastMethodRandomGeneratedNum[_classMethodID]++;
                    }
                    else
                    {
                        // クラスメソッドの乱数発生回数を1に初期化
                        rM.pastMethodRandomGeneratedNum.Add(_classMethodID, 1);
                    }
                    // a乱数を保存
                    saveRandomValue(_randomNum, _classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                        
                    return _randomNum;
                }

            }

            /// <summary>
            /// 指定した乱数randomNumを，「クラス：メソッド：乱数発生回数」をキーとして保存します．
            /// </summary>
            /// <param name="_decidedDiceNum"></param>
            /// <param name="_classMethodID"></param>
            /// <param name="_methodRandomGeneratedNum"></param>
            private void saveRandomValue(double _randomNum, string _classMethodID, int _methodRandomGeneratedNum)
            {
                rM.pastRandomNum.Add(_classMethodID + "." + _methodRandomGeneratedNum, _randomNum);
            }
            /// <summary>
            /// 指定した乱数randomNumを，呼び出し元メソッドの「クラス：メソッド：乱数発生回数」をキーとして保存します．乱数発生回数も増加します．
            /// </summary>
            /// <param name="_classMethodID"></param>
            /// <param name="_methodRandomGeneratedNum"></param>
            /// <param name="_decidedDiceNum"></param>
            public void saveRandomValue(double _randomNum)
            {
                StackTrace st = new StackTrace(false);
                StackFrame sf = st.GetFrame(1); // 0がこの自身クラスで，1が呼び出し元
                string _className = sf.GetMethod().ReflectedType.Name;
#if MethodName_Absolute
                    sf.GetMethod().ReflectedType.FullName;
#endif
                string _methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
                string _classMethodID = _className + "." + _methodName;
                //return classMethodID;

                // クラスメソッドの乱数発生回数を保存
                // クラスメソッドが登録されているか
                if (rM.pastMethodRandomGeneratedNum.ContainsKey(_classMethodID) == true)
                {
                    // クラスメソッドの乱数発生回数を増加
                    rM.pastMethodRandomGeneratedNum[_classMethodID]++;
                }
                else
                {
                    // クラスメソッドの乱数発生回数を1に初期化
                    rM.pastMethodRandomGeneratedNum.Add(_classMethodID, 1);
                }
                // a乱数を保存
                saveRandomValue(_randomNum, _classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                        
            }
            /// <summary>
            /// 過去に保存された引数の「クラス：メソッド：乱数発生回数」の乱数の値を返します．
            /// </summary>
            /// <param name="_classMethodID"></param>
            /// <param name="_methodRandomGeneratedNum"></param>
            /// <returns></returns>
            private double getPastRandomValue(string _classMethodID, int _methodRandomGeneratedNum)
            {
                return rM.pastRandomNum[_classMethodID + "." + _methodRandomGeneratedNum];
            }
            /// <summary>
            /// 過去に保存された「クラス：メソッド：乱数発生回数」の乱数の値を，呼び出し元メソッドの指定した乱数発生回数のものを呼び出して返します．存在しない場合は，-1を返します．
            /// </summary>
            /// <param name="_classMethodID"></param>
            /// <param name="_methodRandomGeneratedNum"></param>
            /// <returns></returns>
            public double getPastRandomValue(int _methodRandomGeneratedNum)
            {
                StackTrace st = new StackTrace(false);
                StackFrame sf = st.GetFrame(1); // 0がこの自身クラスで，1が呼び出し元
                string _className = sf.GetMethod().ReflectedType.Name;
#if MethodName_Absolute
                    sf.GetMethod().ReflectedType.FullName;
#endif
                string _methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
                string _classMethodID = _className + "." + _methodName;
                //return classMethodID;

                double _pastRandomValue = 0;
                if (rM.pastMethodRandomGeneratedNum.ContainsKey(_classMethodID) == true)
                {
                    // Sampleと違い乱数発生回数は増加しない

                    // 過去に，同じ乱数発生回数で乱数発生させたか
                    if (rM.pastRandomNum.ContainsKey(_classMethodID + "." + _methodRandomGeneratedNum) == true)
                    {
                        // b過去の乱数を呼び出して返す
                        _pastRandomValue = getPastRandomValue(_classMethodID, _methodRandomGeneratedNum);
                    }
                    else
                    {
                        // 無い場合，-1
                        _pastRandomValue = -1;
                    }
                }

                return _pastRandomValue;
            }
        }
        
    }
}
