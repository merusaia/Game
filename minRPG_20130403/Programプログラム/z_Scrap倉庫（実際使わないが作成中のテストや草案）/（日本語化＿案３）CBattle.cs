using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /*
    /// <summary>
    /// �Q�[���E�V�i���I�쐬�҂ɁC�킩��₷�����{�ꃁ�\�b�h�ɂ��Ăяo�����@��񋟂���N���X�ł��D�K�v�ɉ����ĐV�������\�b�h�������Ă��n�j�ł��D
    /// </summary>
    public class �Q�[��
    {
        private CBattle p_battle = new CBattle();
        private CSoundPlayData�E�I�[�f�B�I�Đ��p�N���X p_sound = new CSoundPlayData�E�I�[�f�B�I�Đ��p�N���X();

        /// <summary>
        /// �퓬���J�n���܂��D�Q������L������I�����܂��i�퓬�����ύX�\�ł��D�j���܂��D// [TODO]CGameManager�E�Q�[���Ǘ��� _globalData�͂ǂ��Ɉ���������΂悢�H
        /// </summary>
        public void �퓬�J�n(CGlobalData _�Q�[���f�[�^, CChara[] _p���퓬�̖����L��������, CChara[] _�G�L��������, CChara[] _����L��������)
        {
            beginBattle(_�Q�[���f�[�^, _p���퓬�̖����L��������, _�G�L��������, _����L��������);
        }
        /// <summary>
        /// �T�E���h�i���ʉ��ESE�j���Đ����܂��D
        /// </summary>
        /// <param name="_fileName_NotFullPath_�t�@�C����_���O����"></param>
        /// <returns></returns>
        public int �T�E���h�Đ�(SoundName _fileName_NotFullPath_�t�@�C����_���O����)
        {
            return p_sound.playSound(_fileName_NotFullPath_�t�@�C����_���O����);
        }

    }

    /// <summary>
    /// �퓬���Ǘ�����N���X�ł��D
    /// </summary>
    public class CBattle
    {
        private List<CChara> p_charaPlayers;
        private List<CChara> p_charaEmenys;
        private List<CChara> p_charaExras;
        public static int _charaPlayerYou_PlayerID = 0;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="_charaPlayer"></param>
        /// <param name="_charaEnemy"></param>
        /// <param name="_charaOther"></param>
        public CBattle()
        {
        }

        /// <summary>
        /// �퓬���J�n���܂��D�Q������L������I�����܂��i�퓬�����ύX�\�ł��D�j���܂��D// [TODO]CGameManager�E�Q�[���Ǘ��� _globalData�͂ǂ��Ɉ���������΂悢�H
        /// </summary>
        public void beginBattle(CGlobalData _globalData, CChara[] _charaPlayers, CChara[] _charaEnemys, CChara[] _charaExtras)
        {
            p_charaPlayers = new List<CChara>(_charaPlayers);
            p_charaEmenys = new List<CChara>(_charaEnemys);
            p_charaExras = new List<CChara>(_charaExtras);

            drawBattleView();
            innitializeCharaStatas();

            // �퓬���J�n���܂��D
            foreach (CChara _enemy in charaEmenys)
            {
                _globalData.drawMessage(true, _enemy.getName() + " �������ꂽ�I\n");
            }

            // �N����ɍU������H
            // �퓬�^�[������


        }

        /// <summary>
        /// �퓬��ʂ�\�����܂��D
        /// </summary>
        /// <returns></returns>
        public int drawBattleView()
        {
            return 0;
        }
        /// <summary>
        /// �S�L�����̐퓬�X�e�[�^�X�����������܂��D
        /// </summary>
        /// <returns></returns>
        public int innitializeCharaStatas()
        {
            return 0;
        }

    }
     */
}
