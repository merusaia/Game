// �ǂ��炩����R�����g�A�E�g
// ������ID�ƂȂ郁�\�b�h���𖼑O��ԁ`�����N���X�`�����N���X:���\�b�h���̑S�Ă����C�R�����g����Β����N���X:���\�b�h���̂�
//#define MethodName_Absolute

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


//namespace merusaia.Nakatsuka.Tetsuhiro.Experiment
namespace PublicDomain
{
    /// <summary>
    /// �ߋ��̗������Q�Ɖ\�ȃ����_�������N���X�ł��DNext���\�b�h�ɂ�闐���ł́A���̗������͂��߂Ăł���ΐ������ꂽ�������t�@�C���ɕۑ��A�łȂ���Ήߋ��ɐ����������̂Ɠ����������Ăяo���܂��B
    /// </summary>
    public class CMyRandomManager
    {
        int CalledMethodFrameNum = 2; // Sample���\�b�h���CMyRandom�̌Ăяo�����̊O�����\�b�h�������邽�߂̃X�^�b�N�ԍ�

        /// <summary>
        /// �����������L�^���邽�߂Ɏ��C�����̃����_�������N���X�ł��D
        /// </summary>
        protected MyRandom random;
        /// <summary>
        /// �����Ǘ��N���X����MyRandom�N���X������Ă��܂��D
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
        /// ���̃N���X���ߋ��Ɏg�p�����������g�����ǂ�����w���Ă��܂��D
        /// </summary>
        private bool isUsedPastRandomNum = true;
        public void setIsUsedPastRandomNum(bool _isUsedPastRandomNums_FromFile)
        {
            isUsedPastRandomNum = _isUsedPastRandomNums_FromFile;
        }
        /// <summary>
        /// �����𐶐�����u�N���X:���\�b�h:���������񐔁v���L�[�Ƃ��āC�ߋ��ɐ������ꂽ�������i�[���܂��D
        /// </summary>
        private Dictionary<string, double> pastRandomNum = new Dictionary<string, double>();
        /// <summary>
        /// �����𐶐�����u�N���X:���\�b�h�v���L�[�Ƃ��āC�ߋ��Ƀ��\�b�h�����s���ꂽ�i���������ŗ������������ꂽ�j�񐔂��i�[���܂��D
        /// </summary>
        private Dictionary<string, int> pastMethodRandomGeneratedNum = new Dictionary<string, int>();

        public MyFileIO myfileio;
        string writeFilename = "";

        /// <summary>
        /// �����𖳂��ɂ���ƁC���������p�^�[�������񃉃��_����Next()�ȂǂŔ��������闐�������N���X�𐶐����Ċi�[���܂��D�܂��CesetMethodNum�����s����ƁC�N���X���\�b�h�̗��������񐔂ɉ����ĉߋ��Ɠ���������ǂݍ��݂܂��D
        /// </summary>
        public CMyRandomManager()
            :this(false, 0)
        {
        }
        /// <summary>
        /// �����ŁC���������N���X�̐����p�^�[�����C�N�����Ƃɓ�����(true)�^�قȂ邩(false)���w�肵�܂��DNext()�ȂǂŔ��������������i�[���܂��D�܂��CesetMethodNum�����s����ƁC�N���X���\�b�h�̗��������񐔂ɉ����ĉߋ��Ɠ���������ǂݍ��݂܂��D
        /// </summary>
        public CMyRandomManager(bool isRandom_Const, int randomSeed)
        {
            if (isRandom_Const == true)
            {
                random = new MyRandom(randomSeed, this); // �������萔���Ɩ��񓯂��������� // new MyRandom(this); // �����������ƁC�����ɂ���ă����_���l�����߂�Seed��������
            }
            else
            {
                random = new MyRandom(this);
            }
            // �ۑ��f�B���N�g�����쐬
            writeFilename = MyTime.getNowTime_Japanese() + ".txt";
            myfileio = new MyFileIO(MyTools.getProjectDirectory() + "/RandomNumList", false, false);//
        }
        /// <summary>
        /// �����́C�����N���X���\�b�h�����s����v���O�����́C�ߋ��̗�����ۑ������t�@�C�����ł��D
        /// </summary>
        /// <param name="pastRandomNumList_Filename"></param>
        public CMyRandomManager(string pastRandomNumList_Filename)
            :this()
        {

            string readString = myfileio.readFile_simple(pastRandomNumList_Filename);
            string[] lines = readString.Split(MyTools._n.ToCharArray());
            string[] words = new string[2];
            // �ߋ��̔����������t�@�C������ǂݍ���Őݒ�
            foreach (string line in lines)
            {
                words = line.Split(",".ToCharArray());
                pastRandomNum.Add(words[0], Int32.Parse(words[1]));
            }
        }
        /// <summary>
        /// �f�X�g���N�^�ŁC�i�[���������������t�@�C���ɕۑ����܂��D
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
        /// �V���������_���Ȓl�𐶐����āC�ۑ����܂��D
        /// </summary>
        /// <param name="_minValue"></param>
        /// <param name="_maxValue_equals_EnumIntMax"></param>
        public int getRandomNum(int _minValue, int _maxValue)
        {
            return this.random.Next(_minValue, _maxValue);
        }
        

        /// <summary>
        /// ���s���̃N���X���\�b�h�̗��������񐔂�0�Ƃ��Ĉ����܂��DisUsedPastRandomNum��true�̏ꍇ�C������́C�ߋ��̓����u�N���X���\�b�h�F���������񐔁v�ɉ����Ĕ��������������g�p���܂��D
        /// </summary>
        public void resetAllMethodRandomGeneratedNum()
        {
            
            //Enumerator pastMethodRandomGeneratedNum.Keys.GetEnumerator();
            foreach(KeyValuePair<string,int> _randomGeneratednum in pastMethodRandomGeneratedNum){
                pastMethodRandomGeneratedNum[_randomGeneratednum.Key] = 0;
            }
        }

        /// <summary>
        /// �����_���l��ێ�����C�����N���X
        /// </summary>
        public class MyRandom : Random
        {
            /// <summary>
            /// ���������������i�[���Ă��闐���Ǘ��N���X
            /// </summary>
            public CMyRandomManager rM;
            /// <summary>
            /// �N�����Ƃɖ��񃉃��_���̐����p�^�[�����ς�闐�������N���X���쐬���܂��i=new Random()�Ɠ������������j
            /// </summary>
            /// <param name="_usedMyRandomManager"></param>
            public MyRandom(CMyRandomManager _usedMyRandomManager)
                : base()
            {
                rM = _usedMyRandomManager;
            }
            /// <summary>
            /// ���܂��������_���̐����p�^�[���������������N���X���C�����p�^�[���̎�ނł���seed��ݒ肵�Đ������܂��i=new Random(int seed)�Ɠ������������j
            /// </summary>
            /// <param name="_seed"></param>
            /// <param name="_usedMyRandomManager"></param>
            public MyRandom(int _seed, CMyRandomManager _usedMyRandomManager)
                : base(_seed)
            {
                rM = _usedMyRandomManager;
            }
            /// <summary>
            /// 1�`100��100�ʂ�̗�����Ԃ��܂��D���������������C���������L�[�Ƃ��ĕۑ����܂��D
            /// </summary>
            /// <param name="randomNum_name_AndRange">���̂��߂̗������C���{��Ő������Ă��������D</param>
            /// <returns></returns>
            public int NextParsentage�E�P����P�O�O�̗�������(string randomNum_Name_Explanation)
            {
                // [Memo][Random][Sample]:Random�N���X��Next���\�b�h�́C�����Ɋ֌W�Ȃ�Sample���\�b�h���Ăяo���̂ŁC���ʂ̏�����Sample���\�b�h�ɏ����I
                //���ꂾ��Sample()�ł̌Ăяo������NextParsentage()�ɂȂ��Ă��܂��Ddouble randomNum = (double)(base.Next(1, 100)) / 100.0; // �e�N���X��Sample���\�b�h�����C�C���X�^���X�̌^��MyRandom�Ȃ̂ŁC���ʓI�ɂ��̃N���X��Sample���\�b�h���Ăяo��
                int randomNum = (int)(this.Sample() * 100.0)+1; // �e�N���X��Sample���\�b�h�����C�C���X�^���X�̌^��MyRandom�Ȃ̂ŁC���ʓI�ɂ��̃N���X��Sample���\�b�h���Ăяo��
                if (randomNum == 101)
                {
                    randomNum = 100;
                }
                return randomNum;
            }
            /// <summary>
            /// 0.0001�`1.0000�i0.01�`100���j��10000�ʂ�̗�����Ԃ��܂��D���������������C���������L�[�Ƃ��ĕۑ����܂��D
            /// </summary>
            /// <param name="randomNum_name_AndRange">���̂��߂̗������C���{��Ő������Ă��������D</param>
            /// <returns></returns>
            public double NextParsentage�E�P���ʂ�̊m���p�[�Z���g�𔭐�(string randomNum_Name_Explanation)
            {
                // [Memo][Random][Sample]:Random�N���X��Next���\�b�h�́C�����Ɋ֌W�Ȃ�Sample���\�b�h���Ăяo���̂ŁC���ʂ̏�����Sample���\�b�h�ɏ����I
                //���ꂾ��Sample()�ł̌Ăяo������NextParsentage()�ɂȂ��Ă��܂��Ddouble randomNum = (double)(base.Next(1, 100)) / 100.0; // �e�N���X��Sample���\�b�h�����C�C���X�^���X�̌^��MyRandom�Ȃ̂ŁC���ʓI�ɂ��̃N���X��Sample���\�b�h���Ăяo��
                double randomNum = (double)(int)(this.Sample()*10000)/10000.0; // �e�N���X��Sample���\�b�h�����C�C���X�^���X�̌^��MyRandom�Ȃ̂ŁC���ʓI�ɂ��̃N���X��Sample���\�b�h���Ăяo��
                return randomNum;

                // [Tools][���\�b�h��][�Ăяo��]�Ăяo�����Ƃ̃N���X���ƃ��\�b�h��������Ă���ɂ́CStackTrace�N���X�̃C���X�^���Xst�́CGetFrame(0:�������g�̃��\�b�h)��(1:1�O�̌Ăяo����)�́CGetFrame(0or1).GetMethod().ReflectedType.FullName��GetMethod().Name���g��
                /*public string getCalledClassAndMethodName(){
                GetFrame(CalledMethodFrameNum)?? // �Ăяo�����Ƃ̌Ăяo�����H
                }
                */
                /* http://www.atmarkit.co.jp/bbs/phpBB/viewtopic.php?topic=43758&forum=7&7
                //�����\�b�h�̑����������擾
                string paramName = (new System.Diagnostics.StackTrace()).GetFrame(0).GetMethod().GetParameters()[0].Name;


                //���ǔ�
                string paramName = System.Reflection.MethodBase.GetCurrentMethod().GetParameters()[0].Name;
                */
                /*
                StackTrace st = new StackTrace(false);
                StackFrame sf = st.GetFrame(rM.CalledMethodFrameNum); // 0�����̎��g�N���X�H�C1���Ăяo�����H
                string className = sf.GetMethod().ReflectedType.FullName;
                string methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
                string classMethodID = className + "." + methodName;
                //return classMethodID;

                return getRandomNum_readPastOrSave(randomNum, classMethodID);
                */

            }
            #region Next���\�b�h���C�v�Z�����������i�[�ł���悤�I�[�o�[���C�h����Next�֘A���\�b�h�iSample�j
            
            // [Tip][Random]Next���\�b�h��Sample���\�b�h���Ăяo���̂ŁCSample���\�b�h�����������i����΂悢�I
            /// <summary>
            /// ���̗��������̃��\�b�h�ŏ��߂Ăł����0.0�`1.0�̗����𔭐������ĕԂ��C�ߋ��Ɏ��s�������\�b�h�ł���Η��������񐔂Ɠ����������Ăяo���ĕԂ��܂��D
            /// </summary>
            /// <returns></returns>
            protected override double Sample()
            {

                double randomNum = base.Sample();

                
                StackTrace st = new StackTrace(false);
                StackFrame sf = st.GetFrame(rM.CalledMethodFrameNum); // 0�����̎��g�N���X�H�C1���Ăяo�����i�����ł�this.Next()��NextDouble�Ȃǁj,2���X�ɌĂяo�����̊O���̃��\�b�h
                string _className = sf.GetMethod().ReflectedType.Name;
#if MethodName_Absolute
                    sf.GetMethod().ReflectedType.FullName;
#endif
                string _methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
                string _classMethodID = _className + "." + _methodName;
                //return classMethodID;

                // �V����������ۑ����邩�C�������͉ߋ��̓������������񐔂̗������Ăяo���ĕԂ��܂��D
                return getRandomNum_Save_Or_ReadPastValue(randomNum, _classMethodID);

            }
            #endregion

            /// <summary>
            /// �u�N���X�F���\�b�h�F���������񐔁v�ɂ����āC�͂��߂Ăł���ΐ������ꂽ������ۑ����ĕԂ��A�łȂ���Ήߋ��ɐ����������̂Ɠ����������Ăяo���ĕԂ��܂��D
            /// </summary>
            /// <param name="randomNum"></param>
            /// <param name="classMethodID"></param>
            /// <returns></returns>
            private double getRandomNum_Save_Or_ReadPastValue(double _randomNum, string _classMethodID)
            {
                //try(){
                // �ߋ��̗����g�p�������Ă��邩�A���Ă��Ȃ���Εۑ��̂�
                if (rM.isUsedPastRandomNum == true)
                {
                    // ���������������\�b�h���n�߂Ăł���΃��\�b�h��ǉ��A�܂����������񐔂ɉ�����������ۑ��D
                    if (rM.pastMethodRandomGeneratedNum.ContainsKey(_classMethodID) == true)
                    {
                        // ���������񐔂𑝉�
                        rM.pastMethodRandomGeneratedNum[_classMethodID]++;

                        // �ߋ��ɁC�������������񐔂ŗ���������������
                        if (rM.pastRandomNum.ContainsKey(_classMethodID + "." + rM.pastMethodRandomGeneratedNum[_classMethodID]) == true)
                        {
                            // �����\�b�h�Ɨ��������񐔂����ɋL�^����Ă���΁C�ߋ��ɐ����������̂Ɠ����������Ăяo��
                            // b�ߋ��̗������Ăяo���ĕԂ�
                            return getPastRandomValue(_classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                        }
                        else
                        {
                            // a������ۑ�
                            saveRandomValue(_randomNum, _classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                            return _randomNum;
                        }
                    }
                    else
                    {
                        // �N���X���\�b�h�̗��������񐔂�1�ɏ�����
                        rM.pastMethodRandomGeneratedNum.Add(_classMethodID, 1);
                        // a������ۑ�
                        saveRandomValue(_randomNum, _classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                        return _randomNum;
                    }
                }
                else
                {
                    // ���N���X�̗��������񐔂������ɗ����ۑ��̂�

                    // �N���X���\�b�h�̗��������񐔂�ۑ�
                    if (rM.pastMethodRandomGeneratedNum.ContainsKey(_classMethodID) == true)
                    {
                        // �N���X���\�b�h�̗��������񐔂𑝉�
                        rM.pastMethodRandomGeneratedNum[_classMethodID]++;
                    }
                    else
                    {
                        // �N���X���\�b�h�̗��������񐔂�1�ɏ�����
                        rM.pastMethodRandomGeneratedNum.Add(_classMethodID, 1);
                    }
                    // a������ۑ�
                    saveRandomValue(_randomNum, _classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                        
                    return _randomNum;
                }

            }

            /// <summary>
            /// �w�肵������randomNum���C�u�N���X�F���\�b�h�F���������񐔁v���L�[�Ƃ��ĕۑ����܂��D
            /// </summary>
            /// <param name="_decidedDiceNum"></param>
            /// <param name="_classMethodID"></param>
            /// <param name="_methodRandomGeneratedNum"></param>
            private void saveRandomValue(double _randomNum, string _classMethodID, int _methodRandomGeneratedNum)
            {
                rM.pastRandomNum.Add(_classMethodID + "." + _methodRandomGeneratedNum, _randomNum);
            }
            /// <summary>
            /// �w�肵������randomNum���C�Ăяo�������\�b�h�́u�N���X�F���\�b�h�F���������񐔁v���L�[�Ƃ��ĕۑ����܂��D���������񐔂��������܂��D
            /// </summary>
            /// <param name="_classMethodID"></param>
            /// <param name="_methodRandomGeneratedNum"></param>
            /// <param name="_decidedDiceNum"></param>
            public void saveRandomValue(double _randomNum)
            {
                StackTrace st = new StackTrace(false);
                StackFrame sf = st.GetFrame(1); // 0�����̎��g�N���X�ŁC1���Ăяo����
                string _className = sf.GetMethod().ReflectedType.Name;
#if MethodName_Absolute
                    sf.GetMethod().ReflectedType.FullName;
#endif
                string _methodName = sf.GetMethod().Name; //System.Reflection.MethodBase.GetCurrentMethod().Name;
                string _classMethodID = _className + "." + _methodName;
                //return classMethodID;

                // �N���X���\�b�h�̗��������񐔂�ۑ�
                // �N���X���\�b�h���o�^����Ă��邩
                if (rM.pastMethodRandomGeneratedNum.ContainsKey(_classMethodID) == true)
                {
                    // �N���X���\�b�h�̗��������񐔂𑝉�
                    rM.pastMethodRandomGeneratedNum[_classMethodID]++;
                }
                else
                {
                    // �N���X���\�b�h�̗��������񐔂�1�ɏ�����
                    rM.pastMethodRandomGeneratedNum.Add(_classMethodID, 1);
                }
                // a������ۑ�
                saveRandomValue(_randomNum, _classMethodID, rM.pastMethodRandomGeneratedNum[_classMethodID]);
                        
            }
            /// <summary>
            /// �ߋ��ɕۑ����ꂽ�����́u�N���X�F���\�b�h�F���������񐔁v�̗����̒l��Ԃ��܂��D
            /// </summary>
            /// <param name="_classMethodID"></param>
            /// <param name="_methodRandomGeneratedNum"></param>
            /// <returns></returns>
            private double getPastRandomValue(string _classMethodID, int _methodRandomGeneratedNum)
            {
                return rM.pastRandomNum[_classMethodID + "." + _methodRandomGeneratedNum];
            }
            /// <summary>
            /// �ߋ��ɕۑ����ꂽ�u�N���X�F���\�b�h�F���������񐔁v�̗����̒l���C�Ăяo�������\�b�h�̎w�肵�����������񐔂̂��̂��Ăяo���ĕԂ��܂��D���݂��Ȃ��ꍇ�́C-1��Ԃ��܂��D
            /// </summary>
            /// <param name="_classMethodID"></param>
            /// <param name="_methodRandomGeneratedNum"></param>
            /// <returns></returns>
            public double getPastRandomValue(int _methodRandomGeneratedNum)
            {
                StackTrace st = new StackTrace(false);
                StackFrame sf = st.GetFrame(1); // 0�����̎��g�N���X�ŁC1���Ăяo����
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
                    // Sample�ƈႢ���������񐔂͑������Ȃ�

                    // �ߋ��ɁC�������������񐔂ŗ���������������
                    if (rM.pastRandomNum.ContainsKey(_classMethodID + "." + _methodRandomGeneratedNum) == true)
                    {
                        // b�ߋ��̗������Ăяo���ĕԂ�
                        _pastRandomValue = getPastRandomValue(_classMethodID, _methodRandomGeneratedNum);
                    }
                    else
                    {
                        // �����ꍇ�C-1
                        _pastRandomValue = -1;
                    }
                }

                return _pastRandomValue;
            }
        }
        
    }
}
