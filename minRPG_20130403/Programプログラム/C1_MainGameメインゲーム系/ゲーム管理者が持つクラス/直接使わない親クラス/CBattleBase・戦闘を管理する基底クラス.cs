using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /*/// <summary>
    /// �퓬���Ǘ�����N���X�ł��D
    /// </summary>
    public class �퓬 : CBattleBase�E�퓬���Ǘ�������N���X
    {

    }*/

    /// <summary>
    /// �퓬���ʂ��i�[����񋓑̂ł��D
    /// </summary>
    public enum �퓬����
    {
        ����,

        ����,
        �s�k,
        ��������,
        ����,
        �a��,
        ����,
    }
    /// <summary>
    /// �����_�ł̐퓬�̃t�F�[�Y�̎�ނł��D�t�F�[�Y�̎�ނɂ���āC�{�^�����͂⊄�荞�݉\�ȏ����Ȃǂ��قȂ�܂��D
    /// </summary>
    public enum EBattleFase�E�퓬�t�F�[�Y{
        f00_�J�n��,

        f01_��b��,
        f02_��풆,
        f03_�^�[���i�s��,
        f04_�s����,
        f05_�^�[����~��,
        f06_�^�[���I����,

        f07_�C�x���g��,
        
        f10_�퓬�I����,

        f21_�|�[�Y��,
    }

    /// <summary>
    /// �퓬���Ǘ�����ėp�N���X�ł��D�_�C�X�o�g����p�̃N���X�́A�q�N���X��CBattleDaisu�E�_�C�X�퓬���݂Ă��������B
    /// </summary>
    public class CBattleBase�E�퓬���Ǘ�������N���X
    {
        // �퓬�Q���L����
        public List<CChara�E�L����> p_charaPlayer�E�����L���� = new List<CChara�E�L����>();
        public List<CChara�E�L����> p_charaEnemy�E�G�L���� = new List<CChara�E�L����>();
        public List<CChara�E�L����> p_charaOther�E���̑��L���� = new List<CChara�E�L����>();
        public List<CChara�E�L����> p_charaAll�E�S�L���� = new List<CChara�E�L����>(); // �퓬�ɎQ�����Ă���S�L�����̃C���f�b�N�X���Ǘ����Ă��܂��D
        public int p_charaPlayer_Index�E�����L����_��l��ID = 0;
        public int p_charaEnemy_Index�E�G�L����_���[�_�[ID = 0;

        protected CGameManager�E�Q�[���Ǘ��� game;

        #region �퓬�o�����X�����߂�v���p�e�B
        public static double s_�s���Q�[�W�񕜗ʁQ�f�����P�O�O�ɂ� = 1.0;
        public static double s_SP�񕜗ʁQ���_�͂P�O�O�ɂ� = 2.0;
        public static double s_�N���e�B�J���U���͏㏸�{��1_0 = 1.0;
        public static double s_�N���e�B�J�����ɉ��Z���閂�@�͔{��1_0 = 1.0;

        public static double s_��S���W��10_0 = 10.0;
        public static double s_��S�U���͏㏸�{��1_5 = 1.5;
        public static double s_��՗��W��1_0 = 1.0;
        public static double s_��ՍU���͏㏸�{��2_0 = 2.0;

        public static double s_���厸�s���W�� = 10.0;
        #endregion

        #region �퓬���̏�Ԃ��Ǘ�����v���p�e�B
        protected EBattleFase�E�퓬�t�F�[�Y p_nowFase�E�t�F�[�Y;
        protected double p_time�E�o�ߎ���;
        protected int p_turn�E�^�[��;
        //CInputButton�E�{�^�����͒�` p_input�E���� = EInputButton�E���̓{�^��.����;
        #region get/set�A�N�Z�T
        public EBattleFase�E�퓬�t�F�[�Y getP_Fase�E�t�F�[�Y���擾()
        {
            return p_nowFase�E�t�F�[�Y;
        }
        public void setP_Fase�E�t�F�[�Y��ݒ�(EBattleFase�E�퓬�t�F�[�Y _�t�F�[�Y)
        {
            p_nowFase�E�t�F�[�Y = _�t�F�[�Y;
        }

        /// <summary>
        /// �L�������̃v���p�e�B�́C��{�̓L�����̓����Ɋi�[���邪�C�U���Ώۂ��炢�͑S�L�����܂Ƃ߂Ĉ����邱�Ƃ��������낤����C�ꉞ�������Ă����������������낤�C�Ƃ����Ӗ��ō��ꂽ���X�g�ϐ��i�����������炠�܂�g��Ȃ���������Ȃ��j
        /// </summary>
        /*protected List<int> p_target�E���̃^�[���̑S�L�����̍U���Ώ� = new List<int>();
        public void setP_target�E���̃^�[���̃L�����̍U���Ώۂ�ݒ�(int _�퓬�L����ID, int _�U���ΏۃL����ID)
        {
            if (_�퓬�L����ID > p_target�E���̃^�[���̑S�L�����̍U���Ώ�.Count - 1)
            {
                p_target�E���̃^�[���̑S�L�����̍U���Ώ�[_�퓬�L����ID] = _�U���ΏۃL����ID;
            }
        }*/
        /*public void setP_target�E���̃^�[���̑S�L�����̍U���Ώۂ�ݒ�(int _�L����ID, int _�U���ΏۃL����ID)
        {
            if (_�L����ID > p_target�E���̃^�[���̑S�L�����̍U���Ώ�.Count - 1)
            {
                p_target�E���̃^�[���̑S�L�����̍U���Ώ�[_�L����ID] = _�U���ΏۃL����ID;
            }
        }*/
        #endregion
        #endregion
        #region �퓬���ʂ��Ǘ�����v���p�e�B�E���\�b�h
        private �퓬���� p_battleResult = �퓬����.����;
        public void set�퓬����(�퓬���� _����)
        {
            p_battleResult = _����;
        }
        public �퓬���� get�퓬����()
        {
            return p_battleResult;
        }
        public bool is����()
        {
            if (p_battleResult == �퓬����.����)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool is�s�k()
        {
            if (p_battleResult == �퓬����.�s�k)
            {
                return true;
            }else{
                return false;
            }
        }
        #endregion

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="_charaPlayer"></param>
        /// <param name="_charaEnemy"></param>
        /// <param name="_charaOther"></param>
        public CBattleBase�E�퓬���Ǘ�������N���X(CGameManager�E�Q�[���Ǘ��� _g)
        {
            game = _g;
        }

        /// <summary>
        /// �V�����퓬���J�n����Ƃ��̏����������ł��D
        /// </summary>
        protected void initBattleParameter()
        {
            p_charaPlayer�E�����L����.Clear();
            p_charaEnemy�E�G�L����.Clear();
            p_charaOther�E���̑��L����.Clear();
            p_charaAll�E�S�L����.Clear();

            // �퓬���ʂ̊Ǘ��p�����[�^
            p_battleResult = �퓬����.����;

            // �퓬���̊Ǘ��p�����[�^
            p_nowFase�E�t�F�[�Y = EBattleFase�E�퓬�t�F�[�Y.f00_�J�n��;
            p_time�E�o�ߎ��� = 0.0;
            p_turn�E�^�[�� = 1;
        }

        /// <summary>
        /// �퓬���J�n���܂��D�Q������L������I�����܂��i�퓬�����ύX�\�ł��D�j// [TODO]CGameManager�E�Q�[���Ǘ��� _globalData�͂ǂ��Ɉ���������΂悢�H
        /// </summary>
        virtual public void startBattle�E�퓬�J�n(CGameManager�E�Q�[���Ǘ��� _g, List<CChara�E�L����> _charaPlayer, List<CChara�E�L����> _charaEnemy, List<CChara�E�L����> _charaOther)
        {
            // �퓬�p�����[�^�̏�����
            initBattleParameter();

            game = _g;
            p_charaPlayer�E�����L���� = _charaPlayer;
            p_charaEnemy�E�G�L���� = _charaEnemy;
            p_charaOther�E���̑��L���� = _charaOther;
            p_charaAll�E�S�L����.AddRange(_charaPlayer);
            p_charaAll�E�S�L����.AddRange(_charaEnemy);
            p_charaAll�E�S�L����.AddRange(_charaOther);

            viewBattleWindow�E�퓬��ʕ\��();
            initializeChara�E�L�����퓬�J�n��ԏ�����();

            // �퓬���J�n���܂��D
            Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "�������������������퓬�J�n�I��������������������");
            
            // ���y���Đ�
            //
            game.m���b�Z�[�W_�{�^������("");
            foreach (CChara�E�L���� _enemy in p_charaEnemy�E�G�L����)
            {
                game.m���b�Z�[�W_��������("�yc�z"+_enemy.name���O() + " �������ꂽ�I");
            }
            game.wait�E�F�C�g(1000);

            // ���C���퓬�R�}���h���������s
            doBattleCommand();

        }
        #region �������قȂ郁�\�b�h
        /// �퓬���J�n���܂��i����L���������j�D�Q������L������I�����܂��i�퓬�����ύX�\�ł��D�j
        /// </summary>
        virtual public void startBattle�E�퓬�J�n(CGameManager�E�Q�[���Ǘ��� _gameData, List<CChara�E�L����> _charaPlayer, List<CChara�E�L����> _charaEnemy)
        {
            List<CChara�E�L����> _noChara = new List<CChara�E�L����>();
            startBattle�E�퓬�J�n(_gameData, _charaPlayer, _charaEnemy, _noChara);
        }

        /// <summary>
        /// �퓬���J�n���܂��i�����L������l�j�D�Q������L������I�����܂��i�퓬�����ύX�\�ł��D�j
        /// </summary>
        virtual public void startBattle�E�퓬�J�n(CGameManager�E�Q�[���Ǘ��� _gameData, CChara�E�L���� _charaPlayer, List<CChara�E�L����> _charaEnemy)
        {
            List<CChara�E�L����> _onePayer = new List<CChara�E�L����>();
            _onePayer.Add(_charaPlayer);
            startBattle�E�퓬�J�n(_gameData, _onePayer, _charaEnemy);
        }
        /// <summary>
        /// �퓬���J�n���܂��i�G�L������l�j�D�Q������L������I�����܂��i�퓬�����ύX�\�ł��D�j
        /// </summary>
        virtual public void startBattle�E�퓬�J�n(CGameManager�E�Q�[���Ǘ��� _gameData, List<CChara�E�L����> _charaPlayer, CChara�E�L���� _charaEnemy)
        {
            List<CChara�E�L����> _oneEmeny = new List<CChara�E�L����>();
            _oneEmeny.Add(_charaEnemy);
            startBattle�E�퓬�J�n(_gameData, _charaPlayer, _oneEmeny);
        }
        /// <summary>
        /// �퓬���J�n���܂��i�����L�������G�L��������l�j�D�Q������L������I�����܂��i�퓬�����ύX�\�ł��D�j
        /// </summary>
        virtual public void startBattle�E�퓬�J�n(CGameManager�E�Q�[���Ǘ��� _gameData, CChara�E�L���� _charaPlayer, CChara�E�L���� _charaEnemy)
        {
            List<CChara�E�L����> _onePlayer = new List<CChara�E�L����>();
            _onePlayer.Add(_charaPlayer);
            List<CChara�E�L����> _oneEnemy = new List<CChara�E�L����>();
            _oneEnemy.Add(_charaEnemy);
            startBattle�E�퓬�J�n(_gameData, _onePlayer, _oneEnemy);
        }
        #endregion

        /// <summary>
        /// �퓬�^�[�������ł��D
        /// </summary>
        private void doBattleCommand()
        {
            // �퓬�^�[������
            while (p_nowFase�E�t�F�[�Y != EBattleFase�E�퓬�t�F�[�Y.f10_�퓬�I����)
            {
                Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "-------------------1:��b��---------------------------");
                setP_Fase�E�t�F�[�Y��ݒ�(EBattleFase�E�퓬�t�F�[�Y.f01_��b��);
                command1�E�퓬��b();
                Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "-------------------2:�����͒�-----------------------");
                setP_Fase�E�t�F�[�Y��ݒ�(EBattleFase�E�퓬�t�F�[�Y.f02_��풆);
                command2�E�퓬������();
                Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "/_/_/_/_/_/_/_/_/_/3:�� " + p_turn�E�^�[�� + " �^�[���J�n_/_/_/_/_/_/_/_/_/_");
                setP_Fase�E�t�F�[�Y��ݒ�(EBattleFase�E�퓬�t�F�[�Y.f03_�^�[���i�s��);
                command3�E�퓬�^�[���J�n();
                while (p_nowFase�E�t�F�[�Y != EBattleFase�E�퓬�t�F�[�Y.f06_�^�[���I����)
                {
                    // �{�^������C�R�}���h���͎�t
                    // [TODO]�����͓��͂�����K�v�H
                    EBattleFase�E�퓬�t�F�[�Y _nextFase = game.setNextFase_FromBattleActions�E�A�N�V�������玟�̃t�F�[�Y��ݒ�();
                    // [Q]�C�x���g�쓮�^���Ƃ����ɂ��������͕̂ρH//_nextFase = doInput�E���͑��삩�玟�̃t�F�[�Y������();
                    switch (_nextFase)
                    {
                        case EBattleFase�E�퓬�t�F�[�Y.f03_�^�[���i�s��:
                            Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "----------------3:�^�[���i�s��----------------");
                            setP_Fase�E�t�F�[�Y��ݒ�(EBattleFase�E�퓬�t�F�[�Y.f03_�^�[���i�s��);
                            command4�E�퓬�^�[���s���p��();
                            break;
                        case EBattleFase�E�퓬�t�F�[�Y.f04_�s����:
                            Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "����������������4:�s����������������������");
                            setP_Fase�E�t�F�[�Y��ݒ�(EBattleFase�E�퓬�t�F�[�Y.f04_�s����);
                            command5�E�퓬�s�����荞��();
                            break;
                        case EBattleFase�E�퓬�t�F�[�Y.f05_�^�[����~��:
                            Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "----------------5:�^�[����~��----------------");
                            setP_Fase�E�t�F�[�Y��ݒ�(EBattleFase�E�퓬�t�F�[�Y.f05_�^�[����~��);
                            break;
                        default:
                            break;
                    }
                    // �`�揈��
                    game.draw�E�`��X�V����();
                    // 1���[�����g�i���ԒP�ʁC1FPS�H�j�҂�
                    game.waitF�E�F�C�g�t���[��(1);
                }
                Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "/_/_/_/_/_/_/_/_/_/6:�� " + p_turn�E�^�[�� + " �^�[���I��_/_/_/_/_/_/_/_/_/_");
                setP_Fase�E�t�F�[�Y��ݒ�(EBattleFase�E�퓬�t�F�[�Y.f06_�^�[���I����);
                command6�E�퓬�^�[���I��();
                p_turn�E�^�[��++;
            }
        }

        protected EBattleFase�E�퓬�t�F�[�Y doInput�E���͑��삩�玟�̃t�F�[�Y������()
        {
            EBattleFase�E�퓬�t�F�[�Y _nextFase = EBattleFase�E�퓬�t�F�[�Y.f03_�^�[���i�s��;
            // [YET]������
            /*

            CInputButton�E�{�^�����͒�` p_input�E���� = game.getInput�E���͑����();
            switch(p_input�E����){
                case EInputButton�E���̓{�^��.�U���{�^��:
                    _nextFase = EBattleFase�E�퓬�t�F�[�Y.f04_�s����;
                    break;
                default:
                    break;
            }
             * */
            return _nextFase;
        }

        protected int viewBattleWindow�E�퓬��ʕ\��()
        {
            return 0;
        }
        protected int initializeChara�E�L�����퓬�J�n��ԏ�����()
        {
            return 0;
        }

        #region ���C���̐퓬�̗��ꏈ���icomannd1�`6�j
        protected void command1�E�퓬��b(){
            // [TODO]
        }
        protected void command2�E�퓬������()
        {
            int _�L����ID = 0;
            // �i�߂��ꂢ������̒��Ԃ����鎞�j�����L�����̍������
            foreach (CChara�E�L���� _�L���� in p_charaPlayer�E�����L����)
            {
                string _��� = _�L����.Var(EVar.���);
                // ��command2-1: ��팈��
                if (_��� == CVarValue�E�ϐ��l.sakusen01_�߂��ꂢ�����Ă�.ToString())
                {
                    // [TODO]���͖���ł��������C����{�^���������ĔC�ӂ̃^�C�~���O�ł����@���L��ɂ���H
                    // ���̉��i�s�R�[���j
                    Program�E���s�t�@�C���Ǘ���.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "-----------------------" + _�L����.name���O() + "�̍��----------------");
                    CVarValue�E�ϐ��l _���̃^�[���̍�� = game.showBattleCommand2�E�퓬�R�}���h��ʂQ����\�����ē��͑ҋ@(_�L����);
                    _�L����.setVar�E�ϐ���ύX(EVar.���̃^�[���̍��, _���̃^�[���̍��);
                }
                else
                {
                    _�L����.setVar�E�ϐ���ύX(EVar.���̃^�[���̍��, _�L����.Var(EVar.���));
                }

                // ��command2-2: �U���Ώۂ�I���i�U���Ώۂ��w��\�ȍ��̏ꍇ�j
                if (_��� == CVarValue�E�ϐ��l.sakusen01_�߂��ꂢ�����Ă�.ToString() || _��� == CVarValue�E�ϐ��l.sakusen02_�A�C�c��_��.ToString() || _��� == CVarValue�E�ϐ��l.sakusen21_��Ε��].ToString())
                {
                    int _�U���ΏۃL����ID = game.showBattleCommand2_2�E�퓬�R�}���h��ʂQ�U���Ώۂ�\�����ē��͑ҋ@(_�L����, p_charaAll�E�S�L����);
                    p_charaAll�E�S�L����[_�L����ID].setVar�E�ϐ���ύX(EVar.���̃^�[���̍U���Ώ�id, _�U���ΏۃL����ID.ToString());
                }

                _�L����ID++;
            }
            
        }
        virtual protected void command3�E�퓬�^�[���J�n()
        {
            // ���̃^�[���̃p�����[�^�̑��

            // CPU�v�l�����H
            //Program�E�v���O����.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, "-----------------------CPU�v�l��-----------------------");
        }
        virtual protected void command4�E�퓬�^�[���s���p��()
        {
            // �s���p�����͏�ɌĂяo����郁�\�b�h
            p_time�E�o�ߎ��� += CGameManager�E�Q�[���Ǘ���.s_FRAME1_MSEC�E1�t���[���~���b;
            // �s���́ESP�̉�
            c4_1�E�p�����R��(p_charaPlayer�E�����L����);
            c4_1�E�p�����R��(p_charaEnemy�E�G�L����);
            c4_1�E�p�����R��(p_charaOther�E���̑��L����);
            
        }
        virtual protected void command5�E�퓬�s�����荞��()
        {
            // [TODO]
        }
        virtual protected void command6�E�퓬�^�[���I��()
        {

        }
        #endregion



        #region �T�u�̗��ꏈ��(c*_*)

        public void c4_1�E�p�����R��(List<CChara�E�L����> _�L��������){
            foreach (CChara�E�L���� _�L���� in _�L��������)
            {
                // �s����(AP)����
                _�L����.setPara(EPara.s20_AP, ESet.add�E�����l, (_�L����.Para(EPara.a4_�f����) / 100.0 * s_�s���Q�[�W�񕜗ʁQ�f�����P�O�O�ɂ� * _�L����.paraAuto�E�p���␳�揜() + _�L����.paraAuto�E�p���␳����()));
                //[MEMO]����ł����� //_chara.setParaValue(EPara.s20_AP, _chara.ko�s���Q�[�W() + s_�s���Q�[�W�񕜗ʁQ�f�����P�O�O�ɂ� * (_chara.su�f����() / 50.0)) +  * _chara.Para(EPara.�p�����R�񕜕␳�揜).get() + _chara.paraAuto�E�p�����R�񕜕␳�l());

                // SP����
                _�L����.setPara(EPara.s04_SP, ESet.add�E�����l, (_�L����.Para(EPara.s04_SP) / 100.0) * s_SP�񕜗ʁQ���_�͂P�O�O�ɂ� * _�L����.paraAuto�E�p���␳�揜() + _�L����.paraAuto�E�p���␳����());
            }
        }
        
        #endregion






    }
}
