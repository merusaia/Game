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
    /// �퓬���Ǘ�����N���X�ł��D
    /// </summary>
    public class �e�X�gGBattle�E�퓬
    {
        private List<CChara�E�L����> p_charaPlayer�E�����L����;

        private List<CChara�E�L����> p_charaEnemy�E�G�L����;
        private List<CChara�E�L����> p_charaOther�E���̑��L����;
        public static int p_charaPlayer_Index�E�����L����_��l��ID = 0;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="_charaPlayer"></param>
        /// <param name="_charaEnemy"></param>
        /// <param name="_charaOther"></param>
        public �e�X�gGBattle�E�퓬()
        {
        }

        /// <summary>
        /// �퓬���J�n���܂��D�Q������L������I�����܂��i�퓬�����ύX�\�ł��D�j���܂��D// [TODO]CGameManager�E�Q�[���Ǘ��� _globalData�͂ǂ��Ɉ���������΂悢�H
        /// </summary>
        public void startBattle�E�퓬�J�n(CGameManager�E�Q�[���Ǘ��� _gameData, CChara�E�L����[] _charaPlayer, CChara�E�L����[] _charaEnemy, CChara�E�L����[] _charaOther)
        {
            p_charaPlayer�E�����L���� = new List<CChara�E�L����>(_charaPlayer);
            p_charaEnemy�E�G�L���� = new List<CChara�E�L����>(_charaEnemy);
            p_charaOther�E���̑��L���� = new List<CChara�E�L����>(_charaOther);

            viewBattleWindow�E�퓬��ʕ\��();
            initializeChara�E�L�����퓬�J�n��ԏ�����();

            // �퓬���J�n���܂��D
            foreach (CChara�E�L���� _enemy in p_charaEnemy�E�G�L����)
            {
                _gameData.m���b�Z�[�W_�{�^������(_enemy.name���O() + " �������ꂽ�I\n");
            }

            // �N����ɍU������H
            // �퓬�^�[������


        }

        private int viewBattleWindow�E�퓬��ʕ\��()
        {
            return 0;
        }
        private int initializeChara�E�L�����퓬�J�n��ԏ�����()
        {
            return 0;
        }



        

    }
}
