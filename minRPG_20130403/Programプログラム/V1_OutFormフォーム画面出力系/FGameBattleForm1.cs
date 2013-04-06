using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//using merusaia.Nakatsuka.Tetsuhiro.Experiment; // using�g�p�O�ɁC�K���v���W�F�N�g�E�N���b�N���u�Q�Ƃ̒ǉ��v���u�v���W�F�N�g�v��ǉ����邱�ƁI

using Yanesdk.Timer; // ��˂��炨�l�̃Q�[�����C�u����SDK
using Yanesdk.Draw;

namespace PublicDomain
{
    /// <summary>
    /// ���̃Q�[����ʂ�\���i�Ǘ��j���Ă���Windows��p��Form�N���X�D
    /// ���́A���C�����[�h�I����ʂƐ퓬��ʂƃV�i���I��ʂ����p���Ă��܂��B
    /// 
    /// �@�@�@�@�@���`�揈����Windows�ȊO�̈ڐA�����₷���邷���߂ɁC�`�揈����Form���ɗ͐G��Ȃ��悤�ɂ������񂾂��ǁc�C
    /// ����ς�Form�Ȃ�ł͂֗̕��ȋ@�\���������A�o�͌n�͂����Ɍ��\�ȏ���������������Ă�ƌ�������B�B
    /// �ł��邾�����\�b�h�����Č�ňڐA���₷���悤�ɂ��Ă��������B
    /// </summary>
    public partial class FGameBattleForm1 : Form
    {
        // �f�o�b�O���A������ύX����ƌp���ł��Ȃ��̂ŁA�����Ă����瑼�̃N���X�iVar�E�ϐ�.setVar***�Ȃǁj�Ɉڍs���悤
        public bool p_isShowEnemyHP�E�G�̂g�o��\�����邩 = false;
        public bool p_isShowEnemyPara�E�G�̃p�����[�^��\�����邩 = true;
        public bool p_isShowEnemyCommand�E�G�̃_�C�X�R�}���h��\�����邩 = true;

        /// <summary>
        /// �Q�[���̗l�X�ȃN���X�𑍍��I�Ɉ����A�Q�[���󂯓n���f�[�^�ł��B
        /// </summary>
        private CGameManager�E�Q�[���Ǘ��� game;
        public CGameManager�E�Q�[���Ǘ��� getP_GameData�E�Q�[���󂯓n���f�[�^()
        {
            return game;
        }
        /// <summary>
        /// CGameData�E�Q�[���󂯓n���f�[�^�̃C���X�^���Xg�𐶐��������A�܂�Q�[�����������������ǂ����ł��B
        /// ��xtrue�ɂ��Ă���́A�����ł��ύX���Ȃ��ł��������B
        /// </summary>
        private bool p_isCGameDataIlinalized�E�Q�[�����������������s������ = false;
        /// <summary>
        /// �Q�[�������������鏈���ł��B��������H��new�������̂ŁA�Q�[���I���܂ň�x���肵���Ăт����Ȃ��ł��������B
        /// </summary>
        public void _startDiceBattleGame�E�_�C�X�o�g���Q�[��()
        {
            // �y�Q�[�������������z�i�Q�[���I���܂ł͈�x����I�j
            // ��������������������������������������������������������������������������
            // (1)�Q�[���f�[�^g�̏�����
            game = new CGameManager�E�Q�[���Ǘ���();
            // (2)�Q�[���f�[�^g�֊i�[����C�Q�[����ʂ̐ݒ�
            p_���b�Z�[�W�{�b�N�X = new CWinMainTextBox�E���C���e�L�X�g�{�b�N�X(game, this, mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X, mainSelectBox�E�I�������X�g�{�b�N�X, mainInputGroup�E���̓��b�Z�[�W�O���[�v�{�b�N�X, richTextInputLabel1, textInput1, richTextInputLabel2, textInput2, richTextInputLabel3, textInput3);
            // gameWindow�E�Q�[�����().createNewWindow�E��ʏ�����(800, 600, p_usedForm�E�g�p�t�H�[��);
            p_�Q�[����� = new CGameWindow�E�Q�[�����(game, this.Width, this.Height, this, p_���b�Z�[�W�{�b�N�X);
            // (3)�Q�[���f�[�^g�ɁC�Q�[����ʂ�o�^
            game.setP_gameWindow�E�Q�[�����(p_�Q�[�����);
            // �������I���i�X���b�h�Ȃǂ��J�n�j
            p_isCGameDataIlinalized�E�Q�[�����������������s������ = true;

            // (4)�e�X�g�Q�[����ʁA���̑��̃t�H�[���̌Ăяo��
            //p_scinarioCreateForm = new FScinarioCreateForm(game);
            //p_scinarioCreateForm.Show();
            //p_drawForm = new FDrawForm();
            //p_drawForm.Show();
            p_testBalanceForm = new FTestBalanceForm(game);
            p_testBalanceForm.Show();
            Program�E���s�t�@�C���Ǘ���.p_log = new CLog(this.p_testBalanceForm.richtextBattle�E�퓬);
            p_testBalanceForm.Hide();


            // �e�X�g�Q�[�����Ăяo��
            CTestGame�E�e�X�g�Q�[�� _testgame = new CTestGame�E�e�X�g�Q�[��(game, this);
            // �����艺�̏����́A�e�X�g�Q�[���̃R���X�g���N�^���I�����Ă���łȂ��Ǝ��s����Ȃ��̂Œ��ӁB
            bool _isTempTestEnd = true;
            // ��������������������������������������������������������������������������
        }
        /// <summary>
        /// �Q�[���o�����X�𒲐�����t�H�[���ł��B
        /// </summary>
        public FTestBalanceForm p_testBalanceForm;
        /// <summary>
        /// �V�i���I���Q�[�����ɍ쐬�E�ҏW����t�H�[���ł��B
        /// </summary>
        public FScinarioCreateForm p_scinarioCreateForm;
        /// <summary>
        /// ��ʕ`�悷��t�H�[���ł��B
        /// </summary>
        public FDrawForm p_drawForm;


        CGameWindow�E�Q�[����� p_�Q�[�����; // WindowsForm���܂�
        CWinMainTextBox�E���C���e�L�X�g�{�b�N�X p_���b�Z�[�W�{�b�N�X;

        // Window�t�H�[���̃R���g���[���̖��O�ƁC���ʖ��Ƃ̑Ή��\

        // �L�����P�i��l���j�̃p�����[�^
        GroupBox c1_group;
        RichTextBox[] c1_name; // ��l��[0]�����łȂ��A�p�[�e�B�L����[1�`3]�܂܂��B
        PictureBox c1_image;
        Button c1_HP; int c1_HPMax_Width;
        Button c1_SP; int c1_SPMax_Width;
        Button c1_AP; int c1_APMax_Width;
        RichTextBox c1_serihu;
        ListBox c1_dice;
        RichTextBox c1_cost;
        RichTextBox c1_LV;
        RichTextBox c1_paraP;
        RichTextBox c1_paraM ;
        // �L�����Pb�`�P�m�B�p�[�e�B�̖��O���v�p�����[�^�i�g�o�Ȃǁj
        //RichTextBox c1b_name;
        //RichTextBox c1c_name;
        //RichTextBox c1d_name;
        //Button c1b_HP;
        //Button c1c_HP;
        //Button c1d_HP;

        // �L�����Q�i�G���[�_�[�j�̃p�����[�^
        GroupBox c2_group;
        RichTextBox[] c2_name; // ���[�_�[[0]�����łȂ��A�p�[�e�B�L����[1�`3]�܂܂��B
        PictureBox c2_image;
        Button c2_HP; int c2_HPMax_Width;
        Button c2_SP; int c2_SPMax_Width;
        Button c2_AP; int c2_APMax_Width;
        RichTextBox c2_serihu;
        ListBox c2_dice;
        RichTextBox c2_cost;
        RichTextBox c2_LV;
        RichTextBox c2_paraP;
        RichTextBox c2_paraM;
        // �L�����Pb�`�P�m�i�G�p�[�e�B�j�B�p�[�e�B�̖��O���v�p�����[�^�i�g�o�Ȃǁj
        //RichTextBox c2b_name;
        //RichTextBox c2c_name;
        //RichTextBox c2d_name;
        //Button c2b_HP;
        //Button c2c_HP;
        //Button c2d_HP;

        // �Ή��\�̑����Ƃ��āA���C���̃R���g���[��
        /// <summary>
        /// butMain���A��{�I�Ɂu���ցi�i�ށj�v�������́u�����i�܂Ȃ��v�̈Ӗ��������{�^���Ƃ���
        /// </summary>
        Button mainButton�E���փ{�^��;
        RichTextBox mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X;
        // ���C���e�L�X�g�{�b�N�X�̓O���[�v�������Ȃ�GroupBox mainTextBoxGroup;
        ListBox mainSelectBox�E�I�������X�g�{�b�N�X;
        GroupBox mainInputGroup�E���̓��b�Z�[�W�O���[�v�{�b�N�X;


        /// <summary>
        /// �R���X�g���N�^�ł��B
        /// </summary>
        public FGameBattleForm1()
        {
            InitializeComponent();

            // �R���g���[���̃L�[�C�x���g���C�t�H�[���̃L�[�C�x���g�ł͎󂯎��Ȃ��悤�ɂ��܂��D
            this.KeyPreview = false; // �iForm1_KeyDown�C�x���g�őS�ẴR���g���[���̂��̂𓝈ꂷ��Ȃ�A�j=true;

            // �s�N�`���{�b�N�X�ɉ摜���h���b�O�A���h�h���b�v�ł���悤�ɂ��� �Q�l�Fhttp://pgchallenge.seesaa.net/article/65889017.html
            // AllowDrop�v���p�e�B�̓C���e���Z���X�ɂ͂Ȃ��̂Œ��ӁI http://www.atmarkit.co.jp/bbs/phpBB//viewtopic.php?topic=33272&forum=7&4�j
            pictureBox1.AllowDrop = true;
            pictureBox2.AllowDrop = true;

            // �t�H�[���̃R���g���[�����i�K���j�ƈӖ��̑Ή��t��
            c1_name = new RichTextBox[4];
            c1_name[0] = this.richTextNameP1;
            c1_name[1] = this.richTextNameP2;
            c1_name[2] = this.richTextNameP3;
            c1_name[3] = this.richTextNameP4;
            c1_image = this.pictureBox1;
            c1_HP = this.button1; c1_HPMax_Width = this.button1.Width;
            c1_SP = this.button2; c1_SPMax_Width = this.button2.Width;
            c1_AP = this.button5; c1_APMax_Width = this.button5.Width;
            c1_serihu = this.richTextSerihu1;
            c1_dice = this.listDicePlayer1;
            c1_cost = this.richTextBox8;
            c1_LV = this.richTextBox5;
            c1_paraP = this.richTextParaShintai;
            c1_paraM = this.richTextParaSeisin;

            c2_name = new RichTextBox[4];
            c2_name[0] = this.richTextNameE1;
            c2_name[1] = this.richTextNameE2;
            c2_name[2] = this.richTextNameE3;
            c2_name[3] = this.richTextNameE4;
            c2_image = this.pictureBox2;
            c2_HP = this.button4; c2_HPMax_Width = this.button4.Width;
            c2_SP = this.button3; c2_SPMax_Width = this.button3.Width;
            c2_AP = this.button6; c2_APMax_Width = this.button6.Width;
            c2_serihu = this.richTextSerihu2;
            c2_dice = this.listDiceEnemy1;
            c2_cost = this.richTextBox18;
            c2_LV = this.richTextBox20;
            c2_paraP = this.richTextBox14;
            c2_paraM = this.richTextBox11;

            mainButton�E���փ{�^�� = this.butNext���փ{�^��;

            mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X = this.richTextBox1;
            // �ȉ��A�f�t�H���g����̕ύX�_�B
            // �X�N���[���o�[�͐����̂ݗL��
            mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.ScrollBars = RichTextBoxScrollBars.Vertical;
            // ���C���e�L�X�g�{�b�N�X�̃t�H�[�J�X�����������ɁA�I�����\���ɂ��Ȃ�false�itrue���ƑI�����\���j
            //�i�X�N���[���ɂ͉e�������jmainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.HideSelection = false;
            // �^�u��L���ɂ���
            mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.AcceptsTab = true;
            // �h���b�O���h���b�v��L���ɂ��邩�i�V�i���I�t�@�C���̓\����A�摜�Ȃǂ��h���b�O���h���b�v����Ɣ��f���ꂽ�肷��@�\�����Ȃ�ATrue�ɂ���j
            //mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.EnableAutoDragDrop = true;
            
            mainSelectBox�E�I�������X�g�{�b�N�X = this.listBox1SelectBox;
            mainInputGroup�E���̓��b�Z�[�W�O���[�v�{�b�N�X = this.groupInputBox;

            #region �p�����[�^�̐����̃c�[���`�b�v
            int i = 1;
            ToolTip _toopTip = new ToolTip();
            _toopTip.SetToolTip(richIro1, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro2, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro3, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro4, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro5, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro6, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro7, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro8, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro9, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro10, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro11, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            _toopTip.SetToolTip(richIro12, CParas�E�p�����[�^�ꗗ.getParaExplan�E�e�p�����[�^�̐����e�L�X�g���擾(i++));
            #endregion




            // �������ŁC�ŏ��̃e�X�g��ʂ����߂�
            _startDiceBattleGame�E�_�C�X�o�g���Q�[��();
            //startTestDraw�E�e�X�g��ʕ`��();
        }
        public void _startTestDraw�E�e�X�g��ʕ`��()
        {
            this.timer1.Enabled = true;
            this.groupBox1.Visible = false;
            this.groupBox2.Visible = false;
        }
        // �萔�̒�`
        // �ȏI�����Ɏ��̋Ȃ𗬂������ꍇ(Windows.Forms����)�@http://bb-side.com/modules/side03/index.php?content_id=32
        // �ȏI�����̏���
        // ���������ӁI�F������������������ŁAWin32Exception�A"�E�B���h�E�̃n���h�����쐬���ɃG���[���������܂����B"���o���B�Ƃ肠����Windows���b�Z�[�W�͎������Ȃ������ŁB
        ///// <summary>
        ///// Windows.Form��Windows�̃E�B���h�E���b�Z�[�W�ł��BMCI�ōĐ�����Ă����T�E���h�̏I���^�C�~���O�擾�ȂǂɎg���Ă��܂��B
        ///// </summary>
        ///// <param name="m"></param>
        //protected override void WndProc(ref System.Windows.Forms.Message m)
        //{

        //    if (m.Msg == MM_MCINOTIFY && (int)m.WParam == MCI_NOTIFY_SUCCESSFUL)
        //    {
        //        // �Đ��I�����̏����i*2�j
        //        //�i*2�jMCI �ł́A�Đ��I�����Ɏw�肵���E�B���h�E�� NOTIFY ���b�Z�[�W��������B
        //        //        ���̃��b�Z�[�W�𗘗p���čĐ��I�����̏��������s���邱�Ƃ��ł���B
        //        //        �܂��A���̃t�@�C�����Đ�����ꍇ�́A�O�̃t�@�C�����N���[�Y���邱�ƁB 

        //        // �Ƃ肠�����N���[�Y�������Ă����āAMCI�̃��\�[�X���󂯂�悤�ɂ��Ă����B
        //        MySound_Windows.MCI_stopBGM(); // �����MySound_Windows.MCI_getPlayingBGMName_FullPath();��""�ɂȂ�B
        //    }
        //    //base.WndProc (ref m);
        //}
        // �萔�̒�`
        private static int MM_MCINOTIFY = 0x3B9;
        private static int MCI_NOTIFY_SUCCESSFUL = 1;

        /// <summary>
        /// �퓬���n�߂�O�ɌĂяo����ʏ��������\�b�h�ł��B�G�������[�_�[�L�����̐퓬�X�e�[�^�X��\�����܂��B
        /// </summary>
        public void _showBattleInitial�E�퓬��ʏ���������(CBattle�E�퓬 _b)
        {
            // ���L����1�̏����X�V
            if (_b.p_charaPlayer�E�����L����.Count > 0)
            {
                CChara�E�L���� _c1 = MyTools.getListValue(_b.p_charaPlayer�E�����L����, _b.p_charaPlayer_Index�E�����L����_��l��ID);
                _drawCharaPara�E�L�����̖��O��g�o��p�����[�^��\��(true, _c1, _b.p_charaPlayer_Index�E�����L����_��l��ID, true, true);
            }

            // ���L����2�̏����X�V
            if (_b.p_charaEnemy�E�G�L����.Count > 0)
            {
                CChara�E�L���� _c2 = MyTools.getListValue(_b.p_charaEnemy�E�G�L����, 0); //[TODO]������0�͎b��
                _drawCharaPara�E�L�����̖��O��g�o��p�����[�^��\��(false, _c2, _b.p_charaEnemy_Index�E�G�L����_���[�_�[ID, true, true);
            }

            _drawGameForm�E�펞�`�揈��();
        }
        #region �ȉ����ă���
        ///// <summary>
        ///// ��ʂɃL�����̃_�C�X�o�g���̃X�e�[�^�X��\�����܂��B
        ///// </summary>
        //public void showCharaDiceBattleStatus�E�L�����̃_�C�X�o�g���X�e�[�^�X��������(bool _isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\��, CChara�E�L���� _c�L����)
        //{
        //    string _�_�C�X�R�}���h = "";
        //    List<object> _�_�C�X�R�}���h�Q = new List<object>();

        //    // ���L����1�̏����X�V
        //    if (_isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\�� == true)
        //    {
        //        CChara�E�L���� _c1 = _c�L����;
        //        string _serihu = _c1.Var(EVar.�o��Z���t);
        //        if (_serihu == "")
        //        {
        //            c1_serihu.Text = "";
        //        }
        //        else
        //        {
        //            c1_serihu.Text = "�u" + _serihu + "�v";
        //        }
        //        //�ĕ`�悵�Ȃ��悤�ɂ���
        //        //c1_dice.BeginUpdate();
        //        for (int i = 0; i <= _c1.getP_dice�E���L�_�C�X().Count - 1; i++)
        //        {
        //            if (_c1.getP_dice�E���L�_�C�X()[i].p_isUseInBattle�E�퓬�Ŏg�p�\ == true)
        //            {
        //                _�_�C�X�R�}���h = "��" + _c1.getP_dice�E���L�_�C�X()[i].getp_Text�E�ڍ�();
        //                _�_�C�X�R�}���h�Q.Add(_�_�C�X�R�}���h);
        //            }
        //        }
        //        c1_dice.Items.Clear();
        //        c1_dice.Items.AddRange(_�_�C�X�R�}���h�Q.ToArray());
        //        //�ĕ`�悷��悤�ɂ���
        //        //c1_dice.EndUpdate();
        //        _�_�C�X�R�}���h�Q.Clear();
        //    }
        //    // ���L����2�̏����X�V
        //    else
        //    {
        //        CChara�E�L���� _c2 = _c�L����;
        //        string _serihu2 = _c2.Var(EVar.�o��Z���t);
        //        if (_serihu2 == "")
        //        {
        //            c2_serihu.Text = "";
        //        }
        //        else
        //        {
        //            c2_serihu.Text = "�u" + _serihu2 + "�v";
        //        }
        //        //�ĕ`�悵�Ȃ��悤�ɂ���
        //        //c2_dice.BeginUpdate();
        //        for (int i = 0; i <= _c2.getP_dice�E���L�_�C�X().Count - 1; i++)
        //        {
        //            if (_c2.getP_dice�E���L�_�C�X()[i].p_isUseInBattle�E�퓬�Ŏg�p�\ == true)
        //            {
        //                _�_�C�X�R�}���h = "��" + _c2.getP_dice�E���L�_�C�X()[i].getp_Text�E�ڍ�();
        //                _�_�C�X�R�}���h�Q.Add(_�_�C�X�R�}���h);
        //            }
        //        }
        //        c2_dice.Items.Clear();
        //        c2_dice.Items.AddRange(_�_�C�X�R�}���h�Q.ToArray());
        //        //�ĕ`�悷��悤�ɂ���
        //        //c2_dice.EndUpdate();
        //    }
        //    _drawFormControls�EHP�Ȃǂׂ̍����p�����[�^�`��X�V����(game.getP_Battle�E�퓬());
        //}
        #endregion

        /// <summary>
        /// �t�H�[���̕`��X�V���������܂��BHP�ȂǍׂ����p�����[�^�Ȃǂ̍X�V�͂��܂���B
        /// ��.Refresh()�́A�O���̃X���b�h����G��ƃG���[�ɂȂ�܂��B�Ȃ̂�private�ɂ��Ă��܂��B
        /// </summary>
        private void _drawGameForm�E�펞�`�揈��()
        {
            this.Refresh();
        }
        public void _drawCharaPara�E�L�����̖��O��g�o��p�����[�^��\��(bool _isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\��, CChara�E�L���� _c�L����, int _charaPartyID�E�p�[�e�B�ŉ��Ԗڂ̃L������, bool _isShowIroParaAndDice�E�_�C�X�p�����[�^���\�����邩, bool _isClearOtherPatryCharaNameAndHP�E���̃p�[�e�B�L�����̖��O��g�o���x�������������邩_false���Ǝc��)
        {
            CChara�E�L���� _c = _c�L����;
            // �p�[�e�B�̖��O�Ƃg�o���x���̍X�V
            if (_isClearOtherPatryCharaNameAndHP�E���̃p�[�e�B�L�����̖��O��g�o���x�������������邩_false���Ǝc�� == true)
            {
                // �p�[�e�B�̖��O�Ƃg�o���x����������
                if (_isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\�� == true)
                {
                    for (int i = 0; i <= 3; i++) // [TODO]3�͎b��l
                    {
                        setC_name�E�L�����̖��O�Ƃg�o���x���̍X�V(_isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\��, i, null, false);
                    }
                }
                else
                {
                }
            }
            // ���̃L�����̖��O�Ƃg�o���x���̍X�V
            int _id = _charaPartyID�E�p�[�e�B�ŉ��Ԗڂ̃L������;
            if (_c == null)
            {
                setC_name�E�L�����̖��O�Ƃg�o���x���̍X�V(_isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\��, _id, _c, false);
            }
            else
            {
                setC_name�E�L�����̖��O�Ƃg�o���x���̍X�V(_isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\��, _id, _c, true);

                // ���[�_�[�����\�����鍀��
                if (_isShowIroParaAndDice�E�_�C�X�p�����[�^���\�����邩 == true)
                {
                    if (_isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\�� == true)
                    {
                        // �������L�������̕\��
                        string _serihu = _c.Var(EVar.�o��Z���t);
                        if (_serihu == "")
                        {
                            c1_serihu.Text = "";
                        }
                        else
                        {
                            c1_serihu.Text = "�u" + _serihu + "�v";
                        }

                        c1_image.Image = null;
                        c1_HP.Text = _c.para_Int(EPara.s03_HP).ToString(); c1_HP.Width = (int)(c1_HPMax_Width * (_c.para_Int(EPara.s03_HP) / Math.Max(_c.para_Int(EPara.s03b_�ő�HP), 0.01)));
                        c1_SP.Text = _c.para_Int(EPara.s04_SP).ToString(); c1_SP.Width = (int)(c1_SPMax_Width * (_c.para_Int(EPara.s04_SP) / Math.Max(_c.para_Int(EPara.s04b_�ő�SP), 0.01)));
                        c1_AP.Text = _c.para_Int(EPara.s20_AP).ToString(); c1_AP.Width = (int)(c1_APMax_Width * (_c.para_Int(EPara.s20_AP) / Math.Max(_c.para_Int(EPara.s20b_�ő�AP), 0.01)));
                        c1_cost.Text = "�H";
                        c1_LV.Text = _c.para_Int(EPara.LV).ToString();
                        c1_paraP.Text = _c.para_Int(EPara.a1_������) + "\n" + _c.para_Int(EPara.a2_���v��) + "\n" + _c.para_Int(EPara.a3_�s����) + "\n" + _c.para_Int(EPara.a4_�f����) + "\n" + _c.para_Int(EPara.a5_���_��) + "\n" + _c.para_Int(EPara.a6_����);
                        c1_paraM.Text = _c.para_Int(EPara.b1_��p��) + "\n" + _c.para_Int(EPara.b2_�E�ϗ�) + "\n" + _c.para_Int(EPara.b3_���N��) + "\n" + _c.para_Int(EPara.b4_�K����) + "\n" + _c.para_Int(EPara.b5_�W����) + "\n" + _c.para_Int(EPara.b6_�v�l��);

                        // �_�C�X�R�}���h�̍X�V
                        _drawCharaDiceCommand�E�L�����̃_�C�X�R�}���h�̍X�V����(true, _c);
                    }
                    else
                    {
                        // ���G�L�������̕\��
                        string _serihu = _c.Var(EVar.�o��Z���t);
                        if (_serihu == "")
                        {
                            c2_serihu.Text = "";
                        }
                        else
                        {
                            c2_serihu.Text = "�u" + _serihu + "�v";
                        }

                        // ���[�_�[�����\�����鍀��
                        c2_image.Image = null;
                        c2_HP.Text = _c.para_Int(EPara.s03_HP).ToString(); c2_HP.Width = (int)(c2_HPMax_Width * (_c.para_Int(EPara.s03_HP) / Math.Max(_c.para_Int(EPara.s03b_�ő�HP), 0.01)));
                        c2_SP.Text = _c.para_Int(EPara.s04_SP).ToString(); c2_SP.Width = (int)(c2_SPMax_Width * (_c.para_Int(EPara.s04_SP) / Math.Max(_c.para_Int(EPara.s04b_�ő�SP), 0.01)));
                        c2_AP.Text = _c.para_Int(EPara.s20_AP).ToString(); c2_AP.Width = (int)(c2_APMax_Width * (_c.para_Int(EPara.s20_AP) / Math.Max(_c.para_Int(EPara.s20b_�ő�AP), 0.01)));
                        c2_cost.Text = "�H";
                        c2_LV.Text = _c.para_Int(EPara.LV).ToString();
                        c2_paraP.Text = _c.para_Int(EPara.a1_������) + "\n" + _c.para_Int(EPara.a2_���v��) + "\n" + _c.para_Int(EPara.a3_�s����) + "\n" + _c.para_Int(EPara.a4_�f����) + "\n" + _c.para_Int(EPara.a5_���_��) + "\n" + _c.para_Int(EPara.a6_����);
                        c2_paraM.Text = _c.para_Int(EPara.b1_��p��) + "\n" + _c.para_Int(EPara.b2_�E�ϗ�) + "\n" + _c.para_Int(EPara.b3_���N��) + "\n" + _c.para_Int(EPara.b4_�K����) + "\n" + _c.para_Int(EPara.b5_�W����) + "\n" + _c.para_Int(EPara.b6_�v�l��);

                        // �_�C�X�R�}���h�̍X�V
                        _drawCharaDiceCommand�E�L�����̃_�C�X�R�}���h�̍X�V����(false, _c);

                        // �G��HP�Ȃǂ�\�����邩
                        p_isShowEnemyHP�E�G�̂g�o��\�����邩 = true; // �����ŕύX���Ă��悢
                        if (p_isShowEnemyHP�E�G�̂g�o��\�����邩 == true) panel4.Visible = true; else panel4.Visible = false;
                        if (p_isShowEnemyPara�E�G�̃p�����[�^��\�����邩 == true) panel3.Visible = true; else panel3.Visible = false;
                        if (p_isShowEnemyCommand�E�G�̃_�C�X�R�}���h��\�����邩 == true)
                        {
                            listBox4.Visible = true; listDiceEnemy1.Visible = true;
                        }
                        else
                        {
                            listBox4.Visible = false; listDiceEnemy1.Visible = false;
                        }
                    }
                }
            }
        }

        private void _drawCharaDiceCommand�E�L�����̃_�C�X�R�}���h�̍X�V����(bool _isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\��, CChara�E�L���� _c)
        {
            string _�_�C�X�R�}���h = "";
            ListBox _listBox = null;
            if (_isShownLeft�E�������ɕ\�����邩�Qfalse�Ȃ�G���\�� == true)
            {
                _listBox = c1_dice;
            }
            else
            {
                _listBox = c2_dice;
            }
            //�ĕ`�悵�Ȃ��悤�ɂ���i�`�����h�~�j
            _listBox.BeginUpdate();
            // ���L�_�C�X�ƃ��X�g�{�b�N�X�̗v�f���������������炻�̂܂ܒl��ύX���邾�������A�����łȂ������珉��������
            if (_c.getP_dice�E���L�_�C�X().Count != _listBox.Items.Count)
            {
                _listBox.Items.Clear();
            }
            for (int i = 0; i <= _c.getP_dice�E���L�_�C�X().Count - 1; i++)
            {
                CCommand�E�R�}���h _�_�C�X = _c.getP_dice�E���L�_�C�X()[i];
                if (_�_�C�X.p_isUseInBattle�E�퓬�Ŏg�p�\ == true)
                {
                    if (_�_�C�X.p_isNowUse�E���ݎg�p�\ == true)
                    {
                        // �_�C�X�}�X���Â����������C�����������\�b�h��ListBox�ɂ͂Ȃ��H�i�I���ɂ���H�j
                        //c1_dice.SetSelected(i, false);
                        // ���̃e�L�X�g��ύX
                        _�_�C�X�R�}���h = "��" + _c.getP_dice�E���L�_�C�X()[i].getp_Text�E�ڍ�();
                    }
                    else
                    {
                        //c1_dice.SetSelected(i, true);
                        // ���̃e�L�X�g��ύX
                        _�_�C�X�R�}���h = "�~" + _c.getP_dice�E���L�_�C�X()[i].getp_Text�E�ڍ�();
                    }
                    if (i <= c1_dice.Items.Count - 1)
                    {
                        _listBox.Items[i] = _�_�C�X�R�}���h;
                    }
                    else
                    {
                        _listBox.Items.Add(_�_�C�X�R�}���h); // �V�������X�g�𑝂₷
                    }
                }
            }
            //�ĕ`�悷��悤�ɂ���
            _listBox.EndUpdate();
        }
        public void _drawFormControls�EHP�Ȃǂׂ̍����p�����[�^�`��X�V����(CBattle�E�퓬 _b)
        {
            if (_b.p_charaPlayer�E�����L����.Count > 0)
            {
                CChara�E�L���� _cb;
                // �p�[�e�B�̖��O�Ƃg�o���x���̍X�V
                for (int i = 0; i <= 3; i++)
                {
                    _cb = MyTools.getListValue(_b.p_charaPlayer�E�����L����, i);
                    _drawCharaPara�E�L�����̖��O��g�o��p�����[�^��\��(true, _cb, i, false, false);
                }
                // ���[�_�[�����\�����鍀��
                int _readerIndex = _b.p_charaPlayer_Index�E�����L����_��l��ID;
                CChara�E�L���� _c1 = MyTools.getListValue(_b.p_charaPlayer�E�����L����, _readerIndex);
                _drawCharaPara�E�L�����̖��O��g�o��p�����[�^��\��(true, _c1, _readerIndex, true, false);
            }

            if (_b.p_charaEnemy�E�G�L����.Count > 0)
            {
                CChara�E�L���� _cb;
                // �p�[�e�B�̖��O�Ƃg�o���x���̍X�V
                for(int i=0; i<=3; i++){
                    _cb = MyTools.getListValue(_b.p_charaEnemy�E�G�L����, i);
                    _drawCharaPara�E�L�����̖��O��g�o��p�����[�^��\��(false, _cb, i, false, false);
                }
                // ���[�_�[�����\�����鍀��
                int _readerIndex = _b.p_charaEnemy_Index�E�G�L����_���[�_�[ID;
                CChara�E�L���� _c2 = MyTools.getListValue(_b.p_charaEnemy�E�G�L����, _readerIndex);
                _drawCharaPara�E�L�����̖��O��g�o��p�����[�^��\��(false, _c2, _readerIndex, true, false);

                // �G��HP�Ȃǂ�\�����邩
                p_isShowEnemyHP�E�G�̂g�o��\�����邩 = true; // �����ŕύX���Ă��悢
                if (p_isShowEnemyHP�E�G�̂g�o��\�����邩 == true)panel4.Visible = true; else panel4.Visible = false;
                if (p_isShowEnemyPara�E�G�̃p�����[�^��\�����邩 == true) panel3.Visible = true; else panel3.Visible = false;
                if (p_isShowEnemyCommand�E�G�̃_�C�X�R�}���h��\�����邩 == true)
                {
                    listBox4.Visible = true; listDiceEnemy1.Visible = true;
                }
                else
                {
                    listBox4.Visible = false; listDiceEnemy1.Visible = false;
                }

            }

            // �t�H�[�J�X���t�H�[���ɖ߂�
            this.Focus();
        }
        public void setC_name�E�L�����̖��O�Ƃg�o���x���̍X�V(bool _isEnemy�Etrue�G�L�������Qfalse�����L������, int _partyNo_0To�E�p�[�e�B�h�c, CChara�E�L���� _c1b, bool _isVisible�E�\�����邩)
        {
            // �\������R���g���[�������
            RichTextBox _textBox = null;
            if (_isEnemy�Etrue�G�L�������Qfalse�����L������ == true)
            {
                _textBox = MyTools.getArrayValue<RichTextBox>(c1_name, _partyNo_0To�E�p�[�e�B�h�c);
            }
            else
            {
                _textBox = MyTools.getArrayValue<RichTextBox>(c2_name, _partyNo_0To�E�p�[�e�B�h�c);
            }
            if (_textBox != null)
            {
                if (_isVisible�E�\�����邩 == true)
                {
                    if (_c1b != null)
                    {
                        string _shownText = _c1b.name���O().Substring(0, Math.Min(4, _c1b.name���O().Length)) + "LV" + _c1b.Para(EPara.LV);
                        _textBox.Text = _shownText;
                        if (_isEnemy�Etrue�G�L�������Qfalse�����L������ == true || p_isShowEnemyHP�E�G�̂g�o��\�����邩 == true)
                        {
                            _textBox.Text += "\nHP:" + _c1b.para_Int(EPara.s03_HP) + "/" + _c1b.para_Int(EPara.s03b_�ő�HP);
                        }
                        _textBox.Visible = true;
                        // �����Ƀ��x���ŕ\��
                        labelNameP1.Visible = true;
                        labelNameP1.Text = _shownText;
                    }
                }
                else
                {
                    _textBox.Visible = false;
                    // �����Ƀ��x���ŕ\��
                    labelNameP1.Visible = false;
                }
            }
        }
        // �t�H�[���R���g���[���̈ʒu���̈ꎞ�ޔ��B0�̂��̂͌�Ōv�Z����
        private int _battleMessageWidth�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̕� = 0;
        private int _battleMessageHeight�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̍��� = 0;
        private int _battleMessageTop�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̂x���W = 0;
        private int _battleMessageLeft�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̂w���W = 0;
        private int _storyMessageWidth�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̕� = 0;
        private int _storyMessageHeight�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̍��� = 0;
        private int _storyMessageTop�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̂x���W = 12;
        private int _storyMessageLeft�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̂w���W = 12 + 30; // + 30�̓��j���[�o�[�̍���
        /// <summary>
        /// ��ʂ��X�g�[���[���[�h�ɕύX���܂��B
        /// </summary>
        public void _setStroyMode�E�X�g�[���[���[�h�ɉ�ʕύX(){
            saveMainMessageBoxSize�E���b�Z�[�W�{�b�N�X�̃T�C�Y���ꎞ�ޔ�();
            // �o�g�����[�h�̃p�����[�^��ʂ��\���ɂ���
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            _storyMessageWidth�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̕� = (int)(this.Width * 0.90);
            _storyMessageHeight�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̍��� = this.Height - 200; // -200�͑I���E���̓{�b�N�X�̕�
            mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.SetBounds(_storyMessageTop�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̂x���W, _storyMessageLeft�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̂w���W, _storyMessageWidth�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̕�, _storyMessageHeight�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̍���);
        }
        /// <summary>
        /// ��ʂ��_�C�X�o�g�����[�h�ɕύX���܂��B
        /// </summary>
        public void _setDiceBattleMode�E�_�C�X�o�g�����[�h�ɉ�ʕύX()
        {
            saveMainMessageBoxSize�E���b�Z�[�W�{�b�N�X�̃T�C�Y���ꎞ�ޔ�();
            // �o�g�����[�h�̃p�����[�^��ʂ�\���ɂ���
            groupBox1.Visible = true;
            groupBox2.Visible = true;
            mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.SetBounds(_battleMessageLeft�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̂w���W, _battleMessageTop�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̂x���W, _battleMessageWidth�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̕�, _battleMessageHeight�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̍���);
        }
        private void saveMainMessageBoxSize�E���b�Z�[�W�{�b�N�X�̃T�C�Y���ꎞ�ޔ�()
        {
            if (groupBox1.Visible == true)
            {
                // �o�g�����[�h�̂��̂�ޔ�
                _battleMessageWidth�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̕� = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Width;
                _battleMessageHeight�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̍��� = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Height;
                _battleMessageTop�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̂x���W = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Top;
                _battleMessageLeft�E�o�g�����[�h���̃e�L�X�g�{�b�N�X�̂w���W = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Left;
            }
            else
            {
                // �X�g�[���[���[�h�̂��̂�ޔ�
                _storyMessageWidth�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̕� = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Width;
                _storyMessageHeight�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̍��� = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Height;
                _storyMessageTop�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̂x���W = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Top;
                _storyMessageLeft�E�X�g�[���[���[�h���̃e�L�X�g�{�b�N�X�̂w���W = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Left;
            }
        }
        /// <summary>
        /// �퓬�ɕ\������R�}���h�i���[�U���I���\�ȃ��X�g�j��ύX���܂��B
        /// 
        /// ��ʂ̃R�}���h���X�g���A�����Q�̃L���������A�����P�̌`���̃R�}���h�ɕύX���܂��B
        /// ���L���������R�}���h���g���ꍇ�́A��R������null�łn�j�ł��B����̃R�}���h���w�肵���ꍇ�́A��O�����Ɏw�肵�Ă��������B�L���������R�}���h�����D�悵�ĕ\������܂��B
        /// </summary>
        public EShowCommandType�E�\���R�}���h _showBattleCommandList�E�R�}���h���X�g��ύX(EShowCommandType�E�\���R�}���h _showCommandType, CChara�E�L���� _shownChara, List<string> _shownCommand�E�L�������D�悳���Q�K�v�Ȃ����null�łn�j)
        {
            // �R�}���h�����X�g�ɕϊ����āA��������X�g�{�b�N�X�ɕ\��
            List<string> _nameList = new List<string>();
 
            EShowCommandType�E�\���R�}���h _before = p_nowShowCommandType�E���݂̕\���R�}���h;
            // ���ڂ�ύX���郊�X�g�{�b�N�X
            ListBox _listBox = c1_dice;

            if (_shownCommand�E�L�������D�悳���Q�K�v�Ȃ����null�łn�j != null)
            {
                // ����̃R�}���h��\���i���i�K�ł̓X�^�C���͕ς��ĂȂ��j
                _nameList = _shownCommand�E�L�������D�悳���Q�K�v�Ȃ����null�łn�j;
            }
            else
            {
                // �L���������ŗL�R�}���h��\��

                // ���ڂ��擾����CCommand�E�R�}���h�N���X�̃��X�g
                List<CCommand�E�R�}���h> _commandList = null;

                // �Ƃ肠�������X�g�{�b�N�X��\��
                _listBox.Visible = true;
                switch (_showCommandType)
                {
                    case EShowCommandType�E�\���R�}���h._none�E��\��:
                        _listBox.Visible = false;
                        break;
                    case EShowCommandType�E�\���R�}���h.c01_First�E�퓬�J�n�p�R�}���h�Q����������ɂ��铙:
                        // �퓬�J�n�p�R�}���h�ɕύX
                        if (_shownChara != null)
                        {
                            _commandList = _shownChara.getp_battleCommand1�E�퓬�J�n�p�R�}���h();
                        }
                        break;
                    case EShowCommandType�E�\���R�}���h.c02_Target�E�ΏۑI��:
                        // �퓬�N���X����U���Ώۂ�⏕�Ώۂ𐳂����F�����Ă���A�ύX
                        //_shownChara.Var(EVar.
                        break;
                    case EShowCommandType�E�\���R�}���h.c03a_DiceAtack�E�U���_�C�X:
                        // �_�C�X�R�}���h�ɕύX
                        _commandList = _shownChara.getP_dice_ToCommand�E���L�_�C�X���R�}���h�^�Ŏ擾();
                        break;
                    case EShowCommandType�E�\���R�}���h.c03b_DiceDiffence�E�h��_�C�X:
                        // �h��_�C�X�B����B
                        //
                        break;
                    case EShowCommandType�E�\���R�}���h.c03c_SlotOther�E���R�L�q�X���b�g:
                        // ���R�L�q�X���b�g�B����B
                        //
                        break;
                    case EShowCommandType�E�\���R�}���h.c04_Skill�E���Z:
                        // ��������Z�B�܂������ĂȂ�
                        //
                        break;
                    case EShowCommandType�E�\���R�}���h.c05_Item�E�A�C�e��:
                        // �A�C�e��
                        //
                        break;
                    case EShowCommandType�E�\���R�}���h.ct1_TimingBar�E�^�C�~���O�o�[:
                        // �^�C�~���O�o�[�B�������Ɉړ�����_���^�C�~���O�ǂ��~�߂�
                        //
                        break;
                    case EShowCommandType�E�\���R�}���h.ct2_TimingBar�E�^�C�~���O�T�[�N��:
                        // �^�C�~���O�T�[�N���B���S�ɏW�܂�~���^�C�~���O�ǂ��~�߂�
                        //
                        break;
                }
                // �R�}���h���X�g�̃R�}���h�����i�[
                foreach (CCommand�E�R�}���h _item in _commandList)
                {
                    // ���X�g�{�b�N�X�ɒǉ�
                    _nameList.Add(_item.getp_name());
                }
            }
            _listBox.Items.Clear();
            _listBox.Items.AddRange(_nameList.ToArray()); // �R�s�[���Ȃ���Clear()����Ƃ��ɎQ�ƌ���������̂ŋC��t����
            return _before;
        }
        public EShowCommandType�E�\���R�}���h p_nowShowCommandType�E���݂̕\���R�}���h = EShowCommandType�E�\���R�}���h._none�E��\��;
        /// <summary>
        /// ���[�U�ɑ҂����Ԃł��邱�Ƃ������R���g���[����\�����܂��B�����ɁA�҂����Ԃ̖ڈ��ƂȂ鎞�Ԃ��~���b�P�ʂœ���Ă��������B
        /// �ȗ�����Ƌ�̓I�ȃ~���b�͕\������܂���B
        /// �Ăяo���O�ɁA�҂����Ԓ�����������Ԃ��܂��B
        /// </summary>
        /// <param name="_waitMSec"></param>
        public bool setWaitingView�E��ʂɑ҂����Ԃ�\�����邩��ݒ�(bool _TrueIsShown_FalseIsHide)
        {
            return setWaitingView�E��ʂɑ҂����Ԃ�\�����邩��ݒ�(_TrueIsShown_FalseIsHide, -1);
        }
        /// <summary>
        /// ���[�U�ɑ҂����Ԃł��邱�Ƃ������R���g���[����\�����܂��B�����ɁA�҂����Ԃ̖ڈ��ƂȂ鎞�Ԃ��~���b�P�ʂœ���Ă��������B
        /// �ȗ�����Ƌ�̓I�ȃ~���b�͕\������܂���B
        /// </summary>
        /// <param name="_waitMSec"></param>
        public bool setWaitingView�E��ʂɑ҂����Ԃ�\�����邩��ݒ�(bool _TrueIsShown_FalseIsHide, int _waitMSec)
        {
            bool _isBeforeShown = p_isWaitViewShown�E�҂����ԉ�ʂ��\������Ă��邩;
            if (_TrueIsShown_FalseIsHide == true)
            {
                // �\��
                if (p_isWaitViewShown�E�҂����ԉ�ʂ��\������Ă��邩 == false)
                {
                    p_isWaitViewShown�E�҂����ԉ�ʂ��\������Ă��邩 = true;
                    // ���i�K�ł́A���փ{�^���̃X�^�C����ύX
                    // �O�̂��̂�ޔ�
                    p_butNext_BeforeText = butNext���փ{�^��.Text;
                    p_butNext_BeforeColor = butNext���փ{�^��.BackColor;
                    // �ύX
                    butNext���փ{�^��.BackColor = Color.Green;
                    if (_waitMSec != -1)//�f�o�b�O�������ɂ���H && Program�E���s�t�@�C���Ǘ���.isDebug == true)
                    {
                        // �҂����Ԃ�\��
                        butNext���փ{�^��.Text = "����" + _waitMSec + "msec"+"";
                    }
                    else
                    {
                        butNext���փ{�^��.Text = "...";
                    }
                }
            }
            else
            {
                // ��\��
                if (p_isWaitViewShown�E�҂����ԉ�ʂ��\������Ă��邩 == true)
                {
                    p_isWaitViewShown�E�҂����ԉ�ʂ��\������Ă��邩 = false;
                    butNext���փ{�^��.BackColor = p_butNext_BeforeColor;
                    butNext���փ{�^��.Text = p_butNext_BeforeText;
                }
            }
            return _isBeforeShown;
        }
        string p_butNext_BeforeText;
        Color p_butNext_BeforeColor;
        /// <summary>
        /// �҂����ԉ�ʂ��\������Ă��邩
        /// </summary>
        bool p_isWaitViewShown�E�҂����ԉ�ʂ��\������Ă��邩 = false;
        /// <summary>
        /// ���X�g�{�b�N�X�P�o���̑I���������X�g�z��i�����_�ł́A���m�ɂ̓_�C�X�R�}���h�̂h�c�ł͖����̂Œ��Ӂj��ݒ肵�܂�
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_isSelected"></param>
        public void setC1_dice_Selected(int _index, bool _isSelected)
        {
            // _index�̃��X�g�����݂�����A�Z�b�g
            if (c1_dice.Items.Count > 0 && _index <= c1_dice.Items.Count - 1)
            {
                c1_dice.SetSelected(_index, _isSelected);
            }
        }
        public void setC2_dice_Selected(int _index, bool _isSelected)
        {
            // _index�̃��X�g�����݂�����A�Z�b�g
            if (c1_dice.Items.Count > 0 && _index <= c1_dice.Items.Count - 1)
            {
                c2_dice.SetSelected(_index, _isSelected);
            }
        }
        /// <summary>
        /// ���X�g�{�b�N�X�P�o���őI���������X�g�z��i�����_�ł́A���m�ɂ̓_�C�X�R�}���h�̂h�c�ł͖����̂Œ��Ӂj���擾���܂��B
        /// �z���0�`�A�s����I�����Ȃ������ꍇ��-1���Ԃ���܂��B
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_isSelected"></param>
        public int getC1_dice_Selected()
        {
            return c1_dice.SelectedIndex;
        }
        /// <summary>
        /// ���X�g�{�b�N�X�Q�o���őI���������X�g�z��i�����_�ł́A���m�ɂ̓_�C�X�R�}���h�̂h�c�ł͖����̂Œ��Ӂj���擾���܂��B
        /// �z���0�`�A�s����I�����Ȃ������ꍇ��-1���Ԃ���܂��B
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_isSelected"></param>
        public int getC2_dice_Selected()
        {
            return c2_dice.SelectedIndex;
        }
        /// <summary>
        /// �P�o�p�_�C�X�t�H�[���Ƀt�H�[�J�X�����킹�܂��B
        /// </summary>
        public void focusC1_dice()
        {
            c1_dice.Focus();
        }
        /// <summary>
        /// �Q�o�p�_�C�X�t�H�[���Ƀt�H�[�J�X�����킹�܂��B
        /// </summary>
        public void focusC2_dice()
        {
            c2_dice.Focus();
        }
        /// <summary>
        /// �u���ցi�i�ށj�v�{�^���Ƀt�H�[�J�X�������܂��B
        /// �u���ցi�i�ށj�v�{�^������\���̏ꍇ�́A�t�H�[���Ƀt�H�[�J�X�������܂��B
        /// </summary>
        public void focusMainControl�E�t�H�[�J�X�����C���R���g���[���Ɉڂ�()
        {
            if (mainButton�E���փ{�^��.Visible == true)
            {
                // butMain���A��{�I�Ɂu���ցi�i�ށj�v�������́u�����i�܂Ȃ��v�̈Ӗ��������{�^���Ƃ���
                mainButton�E���փ{�^��.Focus();
            }
            else
            {
                // �u���ցi�i�ށj�v�{�^������\���̏ꍇ�́A���������Ȃ��̂Ńt�H�[���Ƀt�H�[�J�X������
                this.Focus();
            }
        }
        /// <summary>
        /// �퓬�p��ʂɎg���R���g���[����\�����A�g��Ȃ��R���g���[�����\�����܂��B
        /// </summary>
        private void showBattleControl()
        {
            groupBox1.Visible = true;
            groupBox2.Visible = true;
            picScinario.Visible = false;
        }
        /// <summary>
        /// �V�i���I�p��ʂɎg���R���g���[����\�����A�g��Ȃ��R���g���[�����\�����܂��B
        /// </summary>
        private void showScinarioControl()
        {
            groupBox1.Visible = false;
            groupBox2.Visible = false;
            picScinario.Visible = true;
        }


        // ���[�h�����͓��ɂȂ��B��{�̓R���X�g���N�^�ł���Ă���B
        private void GameTestForm1_Load(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// �t�H�[���̑���\�ȃR���g���[����񋓂����񋓑̂ł��B
        /// Windows��ˑ��̂t�h�R���g���[���쐬��A�t�h�C�x���g���킩��₷���L�q���鎞�ɂ��g���܂��B
        /// </summary>
        public enum EControlType�E����R���g���[��
        {
            c0a_GoNext�E���֐i�ރ{�^��,
            c0b_GoBack�E�O�֖߂�{�^��,

            c01_Form�E�t�H�[��,
            c02_MessageBox�E���C�����b�Z�[�W�{�b�N�X,
            c03_SelectBox�E�I���{�b�N�X,
            c04_InputBox�E���̓{�b�N�X�P����R�̂ǂꂩ,
            c05_Label1�E���߂P,
            c06_Label2�E���߂Q,

            c11_ListBoxDiceP1�E�_�C�X�R�}���h���X�g�����P,
            c12_ListBoxDiceE1�E�_�C�X�R�}���h���X�g�G�P,
        }
        /// <summary>
        /// �����̃t�H�[�����ŃC�x���g���N���������́A�K�����̃��\�b�h���Ăяo���Ă��������B
        /// WindowsForm�̃{�^���C�x���g�ƃ}�E�X�C�x���g�������N�����鏈���ł��B���{�^�������������Ή����܂��E
        /// �����̃L�[�C�x���g�ƃR���g���[������A�Q�[�����̏����𔻒f���A
        /// �����ɉ������Q�[���i�s�ϐ�game.p_is***�ɑ�����܂��B
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(object _senderControl, EControlType�E����R���g���[�� _EControlType, EInputButton�E���̓{�^�� _mainPressedButton1�E�����Ă���{�^���P, EInputButton�E���̓{�^�� _samePushedButton2�E�����ɉ�����Ă���{�^���Q, EInputButton�E���̓{�^�� _samePushedButton3�E�����ɉ�����Ă���{�^���R)
        {
            //�L�[���������ςȂ��ɂ��Ă��A�����͎��s����Ă� System.Console.WriteLine("�{�^���������ώ��Ă�H");

            // ����R���g���[�����ɁA�{�^�������������̏������L�q
            switch (_EControlType)
            {
                case EControlType�E����R���g���[��.c0a_GoNext�E���֐i�ރ{�^��:
                    // ���͉�ʂŃ{�^�����삵�����̏���
                    // ����{�^���Ŏ��֐i�ށA�߂�{�^���őO�ɖ߂�
                    if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b1_����{�^��_A)
                    {
                        // �u���ցv�{�^�����N���b�N�i�������͌���{�^�������������j�����Ƃ��̏����́A����{�^�������������ƕς��Ȃ��B
                        game.iA����{�^������u���������ŉ���();
                    }
                    else if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b2_�߂�{�^��_B)
                    {
                        // �u�߂�v�{�^�����N���b�N�i�������͖߂�{�^�������������j�����Ƃ��̏����́A�߂�{�^�������������ƕς��Ȃ��B
                        game.iB�߂�{�^������u���������ŉ���();
                    }
                    break;
                case EControlType�E����R���g���[��.c0b_GoBack�E�O�֖߂�{�^��:
                    // ���͉�ʂŃ{�^�����삵�����̏���
                    // ����{�^���Ɩ߂�{�^���A�ǂ���������Ă��A�߂鏈�����s��
                    if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b1_����{�^��_A ||
                        _mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b2_�߂�{�^��_B)
                    {
                        // �u�߂�v�{�^�����N���b�N�����Ƃ��̏����́A�߂�{�^�������������ƕς��Ȃ��B
                        game.iB�߂�{�^������u���������ŉ���();
                    }
                    break;

                case EControlType�E����R���g���[��.c03_SelectBox�E�I���{�b�N�X:
                    // �I�����Ń{�^�����삵�����̏���
                    if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b1_����{�^��_A)
                    {
                        // ����{�^���œ��͑҂�����
                        game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O = true;
                        //���͎g���Ă��Ȃ��B���ɐi�ނɓ����Bgame.p_isEndSelectBox�E�I���{�b�N�X�����t���O = true;
                    }
                    else if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b2_�߂�{�^��_B)
                    {
                        // �߂�{�^���ł����͑҂�����
                        game.p_isUserInput_Back�E�O�ɖ߂���̓t���O = true; // �߂�{�^�������������Ƃ𑼂ɒʒm
                        game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O = true;
                    }
                    break;

                case EControlType�E����R���g���[��.c02_MessageBox�E���C�����b�Z�[�W�{�b�N�X:
                    // ���C�����b�Z�[�W�{�b�N�X�i��ɃQ�[�����̃e�L�X�g���\�������ꏊ�j�Ń{�^�����삵�����̏���

                    // (a)�N���G�[�V�������[�h���֌W�Ȃ��A�t�H�[���i��ʂɃt�H�[�J�X���������Ă��鎞�j�Ɠ����Ƃ���
                    //goto case EControlType�E����R���g���[��.c01_Form�E�t�H�[��;

                    // (b)�N���G�[�V�����Ƃ�������Ȃ����ł킯��
                    // �N���G�[�V�������[�h����
                    if (game.p_isCreation�E�N���G�[�V�������[�h == true)
                    {
                        // ���C���e�L�X�g�{�b�N�X���̃e�L�X�g�Ƀt�H�[�J�X���������Ă�����A
                        // �V�i���I�̃e�L�X�g�ҏW���Ƃ��āA���ɐi�܂Ȃ��B�ifalse�̎������i�߂�j
                        if (mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Focused == true)
                        {
                            // �t�H�[�J�X���������Ă���ꍇ�A����{�^���Ńt�H�[�J�X���u���ցv�{�^���ւ���
                            // �������Ō���{�^���������ɂ���ƃe�L�X�g�I�����ł��Ȃ��Ȃ�̂ŁA�}�E�X���N���b�N�͊O�������������B
                            // if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b1_����{�^��_A)
                            // ����͓��͍X�V���ł��ĂȂ��̂��A�������ςȂ��ɂ��Ȃ��Ǝg���Ȃ�
                            if (game.ik�w��L�[����������_�������ϘA�ˑΉ�(EKeyCode.ENTER) 
                                || game.ik�w��L�[����������_�������ϘA�ˑΉ�(EKeyCode.z))
                            {
                                focusMainControl�E�t�H�[�J�X�����C���R���g���[���Ɉڂ�();
                            }
                            // ���̃L�[�ł��r�[�v�����Ȃ�̂������
                            //focusMainControl�E�t�H�[�J�X�����C���R���g���[���Ɉڂ�();

                        }
                        else
                        {
                            // ���C���e�L�X�g�{�b�N�X���̃e�L�X�g�Ƀt�H�[�J�X���������Ă��Ȃ����A����{�^����������
                            // ���̃��\�b�h���Ă΂ꂽ��A�V�i���I�̃e�L�X�g�t�@�C���C������������B
                            if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b1_����{�^��_A)
                            {
                                // [TODO]�N���G�[�V�������[�h���́A�V�i���I�̃e�L�X�g�t�@�C���C������
                            }
                        }
                    }
                    else
                    {
                        // �N���G�[�V�������[�h�łȂ���΁A�t�H�[���i��ʂɃt�H�[�J�X���������Ă��鎞�j�Ɠ���
                        goto case EControlType�E����R���g���[��.c01_Form�E�t�H�[��;
                    }
                    break;
                case EControlType�E����R���g���[��.c11_ListBoxDiceP1�E�_�C�X�R�}���h���X�g�����P:
                    // �_�C�X�R�}���h���X�g�����P
                    if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b1_����{�^��_A ||
                        _mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b2_�߂�{�^��_B)
                    {
                        // �_�C�X������
                        game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O = true;
                    }
                    break;

                case EControlType�E����R���g���[��.c01_Form�E�t�H�[��:
                    // �t�H�[���Ƀt�H�[�J�X���������Ă��鎞�i���̓��ʂȃR���g���[���Ƀt�H�[�J�X���������Ă��Ȃ����j�̏���

                    // �����ꂪ�ʏ�̃{�^�����菈��
                    if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b1_����{�^��_A ||
                        _mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b2_�߂�{�^��_B ||
                        _mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.a4_�� ||
                        _mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.a2_�� ||
                        _mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.a3_�� ||
                        _mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.a1_�E)
                    {
                        // �\���L�[��`����{�^���E�a�߂�{�^�����������玟�ɐi��
                        game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O = true;
                    }
                    else if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b4_�V�t�g�{�^��_X)
                    {
                        // X�V�t�g�{�^���Ŏ������[�h�H�i���b�Z�[�W��������A�܂��͎����퓬�j
                        if (game.p_isAutoPlay�E�������[�h == false) { game.p_isAutoPlay�E�������[�h = true; } else { game.p_isAutoPlay�E�������[�h = false; }
                    }
                    else if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b3_�R���g���[���{�^��_Y)
                    {
                        // Y�R���g���[���{�^���ň�ԍŏ��̒P��̃w���vor�t�q�k�����N���u���E�U�ŊJ���H�i�������j
                    }
                    else if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b5_�^�u�{�^��_L)
                    {
                        // L�^�u�{�^���Ń��b�Z�[�W����\���H�i�������j
                    }
                    else if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b6_�L���v�X���b�N�{�^��_R)
                    {
                        // R�L���v�X���b�N�{�^���ŃX�L�b�v���[�h�H�i���b�Z�[�W�X�L�b�v�A�܂��͍����X�L�b�v�퓬�j
                        if (game.p_isSkip�E�X�L�b�v���[�h == false) { game.p_isSkip�E�X�L�b�v���[�h = true; } else { game.p_isSkip�E�X�L�b�v���[�h = false; }
                    }
                    else if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b9_�X�y�[�X�{�^��_START)
                    {
                        // START�X�y�[�X�{�^���Ń|�[�Y�؂�ւ�
                        if (game.p_isPause�E�|�[�Y���[�h == false) { game.p_isPause�E�|�[�Y���[�h = true; } else { game.p_isPause�E�|�[�Y���[�h = false; }
                    }
                    else if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b10_�A���g�{�^��_SELECT)
                    {
                        // SELECT�A���g�{�^���͊g���{�^���A�����ȃ{�^���Ƒg�ݍ��킹�ăV���[�g�J�b�g�i�������j
                        if (_samePushedButton2�E�����ɉ�����Ă���{�^���Q == EInputButton�E���̓{�^��.b1_����{�^��_A)
                        {
                            // ��FSELECT+A�Ńf�o�b�O�^�e�L�X�g�ҏW�^�N���G�[�V�������[�h�؂�ւ�
                            if (game.p_isCreation�E�N���G�[�V�������[�h == false) { game.p_isCreation�E�N���G�[�V�������[�h = true; } else { game.p_isCreation�E�N���G�[�V�������[�h = false; }
                        }
                        else if (_samePushedButton2�E�����ɉ�����Ă���{�^���Q == EInputButton�E���̓{�^��.b9_�X�y�[�X�{�^��_START)
                        {
                            // ��FSELECT+START�ŃX�N���[���V���b�g
                            // ����炷
                            game.pSE(ESE�E���ʉ�._system09�E�X�N���[���V���b�g��_�J�V���b);
                            Bitmap _screenShotImage = MyTools.getScreenCapture_ActiveWindow();
                            string _fileName = MyTools.getNowTime_Japanese() + ".png";
                            _screenShotImage.Save(Program�E���s�t�@�C���Ǘ���.p_DatabaseDirectory_FullPath�E�f�[�^�x�[�X�t�H���_�p�X
                                + "�X�N���[���V���b�g\\" + _fileName);
                        }
                    }
                    break;

                default:
                    // ���̑��̃R���g���[���i���̓{�b�N�X�P�`�R��A���x���Ȃǁj�̏ꍇ

                    // Enter�������ƁATab�C���f�b�N�X�����ɑ傫���i�������͍ŏ��́j�A
                    // ���̃R���g���[���Ƀt�H�[�J�X������
                    if (_mainPressedButton1�E�����Ă���{�^���P == EInputButton�E���̓{�^��.b1_����{�^��_A)
                    {
                        SelectNextControl((Control)_senderControl, true, true, true, true);
                        // �t�H�[�J�X��̃R���g���[��
                        Control _nextControl = this.ActiveControl;
                        if (_nextControl.TabStop == false)
                        {
                            // �t�H�[�J�X��������ׂ��ł͂Ȃ��R���g���[���iTabSop==false�j�̂Ƃ��A���C���{�^���ɃR���g���[��������
                            focusMainControl�E�t�H�[�J�X�����C���R���g���[���Ɉڂ�();
                        }
                        // �ȉ��̗l�Ȃ��ƁA�킴�킴���Ȃ��Ă����B
                        //if (textInput2.Visible == true)
                        //{
                        //    textInput2.Focus();
                        //}else...
                    }
                    break;
            }
            // �������ȃG�l���[�h�̎��́A���̏����������Ȃ���updateFrame���\�b�h���Ă΂�Ȃ�
            if (game.getP_gameWindow�E�Q�[�����() != null)
            {
                game.getP_gameWindow�E�Q�[�����().setisEventOccured�E�C�x���g���N����������ݒ�(true);
            }
        }
        #region �����̈قȂ铯�����\�b�h
        /// <summary>
        /// �����̃t�H�[�����ŃC�x���g���N���������́A�K�����̃��\�b�h���Ăяo���Ă��������B
        /// WindowsForm�̃{�^���C�x���g�ƃ}�E�X�C�x���g�������N�����鏈���ł��B
        /// �����̃L�[�C�x���g�ƃR���g���[������A�Q�[�����̏����𔻒f���A
        /// �����ɉ������Q�[���i�s�ϐ�game.p_is***�ɑ�����܂��B
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(EControlType�E����R���g���[�� _EControlType, System.Windows.Forms.Keys _keys)
        {
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(null, _EControlType, _keys);
        }
        /// <summary>
        /// �����̃t�H�[�����ŃC�x���g���N���������́A�K�����̃��\�b�h���Ăяo���Ă��������B
        /// WindowsForm�̃{�^���C�x���g�ƃ}�E�X�C�x���g�������N�����鏈���ł��B
        /// �����̃L�[�C�x���g�ƃR���g���[������A�Q�[�����̏����𔻒f���A
        /// �����ɉ������Q�[���i�s�ϐ�game.p_is***�ɑ�����܂��B
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(object _senderControl, EControlType�E����R���g���[�� _EControlType, System.Windows.Forms.Keys _keys)
        {
            // ���̃{�^���͖�����
            EInputButton�E���̓{�^�� _input1 = game.getP_InputButton().getInputButton(_keys);
            EInputButton�E���̓{�^�� _input2 = EInputButton�E���̓{�^��._none_������;
            EInputButton�E���̓{�^�� _input3 = EInputButton�E���̓{�^��._none_������;
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(_senderControl, _EControlType, _input1, _input2, _input3);
        }
        /// <summary>
        /// �����̃t�H�[�����ŃC�x���g���N���������́A�K�����̃��\�b�h���Ăяo���Ă��������B
        /// WindowsForm�̃{�^���C�x���g�ƃ}�E�X�C�x���g�������N�����鏈���ł��B
        /// �����̃L�[�C�x���g�ƃR���g���[������A�Q�[�����̏����𔻒f���A
        /// �����ɉ������Q�[���i�s�ϐ�game.p_is***�ɑ�����܂��B
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(EControlType�E����R���g���[�� _EControlType, EInputButton�E���̓{�^�� _input1)
        {
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(null, _EControlType, _input1);
        }
        /// <summary>
        /// �����̃t�H�[�����ŃC�x���g���N���������́A�K�����̃��\�b�h���Ăяo���Ă��������B
        /// WindowsForm�̃{�^���C�x���g�ƃ}�E�X�C�x���g�������N�����鏈���ł��B
        /// �����̃L�[�C�x���g�ƃR���g���[������A�Q�[�����̏����𔻒f���A
        /// �����ɉ������Q�[���i�s�ϐ�game.p_is***�ɑ�����܂��B
        /// </summary>
        public void _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(object _senderContorl, EControlType�E����R���g���[�� _EControlType, EInputButton�E���̓{�^�� _input1)
        {
            // ���̃{�^���͖�����
            EInputButton�E���̓{�^�� _input2 = EInputButton�E���̓{�^��._none_������;
            EInputButton�E���̓{�^�� _input3 = EInputButton�E���̓{�^��._none_������;
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(_senderContorl, _EControlType, _input1, _input2, _input3);
        }
        #endregion




        #region �������t�H�[����Timer �^�C�}�[�C�x���g�F�@����I�Ș_��������`�揈�����Ǘ�����N���X�⃁�\�b�h�Ȃ�

        // ����timer1�̃^�C�}�[�ŊǗ�
        /// <summary>
        /// Timer1�̍X�V�Ԋu�ł��B�f�U�C�i�Őݒ肵�Ă��A�����炪�D�悳��܂��B
        /// </summary>
        public int p_timer1_interval�E�^�C�}�[�̍X�V�~���b = 20; // fps50��������Ainterval=1000/50=20
        /// <summary>
        /// �Ӑ}�I��timer1���~�߂�������false�ɂ��Ă��������B
        /// </summary>
        public bool p_isTimer1Run = false; // �����������Ńt�H�[���^�C�}�[�������������Ȃ��������܂�B
        /// <summary>
        /// �����ꂽ�{�^�����R���\�[�����ɕ\������e�X�g���̎���true�ɂȂ�܂��Bbut���낢��e�X�g�A�{�^���Őݒ肵�܂��B
        /// </summary>
        public bool p_isTimerTestButton�E�������{�^�������x���ɕ\������e�X�g�� = false;
        public int p_timer1_startTime = 0;
        //public int p_timer1_FrameNo1ToFPSMAX = 1;���̃t���[�����\���������ꍇ�����Ă���������
        /// <summary>
        /// �������^�C�}�[�ɂ���Ē���I�ɌĂ΂�郁�\�b�h�B
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // �Q�[���������O�͎��s���Ȃ�
            if (p_isCGameDataIlinalized�E�Q�[�����������������s������ == false)
            {
                return;
            }
            // �O�̂��߁Atimer�̍X�V�Ԋu���X�V����H�i���̃��\�b�h�ŕύX�����\�����������߁j
            //p_timer1_interval�E�^�C�}�[�̍X�V�~���b = CGameManager�E�Q�[���Ǘ���.s_FRAME1_MSEC�E1�t���[���~���b;
            //timer1.Interval = p_timer1_interval�E�^�C�}�[�̍X�V�~���b;

            if (p_isTimer1Run == false){
                timer1.Enabled = false;
                return;
            }else{
                // �Q�[���v���C���Ԃ��v�Z���邽�߁A�^�C�}�[���X�^�[�g�������Ԃ��i�[
                if (p_timer1_startTime == 0) p_timer1_startTime = MyTools.getNowTime_fast();
                // 1��̏����̂����鎞�Ԃ��v��
                int _t1 = MyTools.getNowTime_fast();

                // ���Q�[���̃t���[���X�V����
                string _frameInfo�E�o�͏�� = game.getUpdateFrameInfo�t���[���X�V�o�͏����擾();
                
                //(b)�Q�[�����C���X���b�h�����̃��\�b�h�̏ꍇ string _frameInfo�E�o�͏�� = game.updateFrame�t���[�����ɌĂяo�����͘_���`��Ȃǃt���[���X�V����();
                // �����ł���Ȃ񂿂傭����������炠����ŁB�S��g.updateFrame�ɂ܂����Ƃ�������v���g.getP_fpsManager�E�t���[���Ǘ���().updateFrame�E�_��������̃t���[���X�V�����ƕ`�揈���X�L�b�v����();
                // �o�͏���
                if (_frameInfo�E�o�͏�� != "")
                {
                    // �R���\�[���͂���ւ�BMyTools.ConsoleWriteLine(_frameInfo�E�o�͏��);
                    // Label1�ɂ��ő�3�s�܂łŒǉ�
                    string _labFrameInfoText = labInfo2.Text;
                    if (MyTools.getLineNo(_labFrameInfoText) < 3)
                    {
                        labInfo2.Text += "\n"+_frameInfo�E�o�͏��;
                    }
                    else
                    {
                        labInfo2.Text = MyTools.getStringLines_Updated(_labFrameInfoText, _frameInfo�E�o�͏��);
                    }
                }

                // �����낢��e�X�g
                if (p_isTimerTestButton�E�������{�^�������x���ɕ\������e�X�g�� == true)
                {
                    // �{�^���̕\�����}�E�X�e�X�g���ɂ���
                    butTest���낢��e�X�g.Text = "�{�^���e�X�g���c";
                    butTest���낢��e�X�g.BackColor = Color.Green;

                    string _buttonInfo = "";
                    // �`�F�b�N�������{�^����L�[�{�[�h�L�[��}�E�X�{�^���������ɂ����Ă�
                    #region �{�^���e�X�g

                    // �S�Ẵ{�^�����`�F�b�N
                    bool _isTestShowAllButtons = false;
                    if(_isTestShowAllButtons == true){
                        foreach (EInputButton�E���̓{�^�� _key in Enum.GetValues(typeof(EInputButton�E���̓{�^��)))
                        {
                            if (game.ib�{�^������������_�A�˔�Ή�(_key) == true)
                            {
                                _buttonInfo += ("\n��"+_key.ToString() + " �{�^���������ꂽ��: game.ib�{�^������������_�A�˔�Ή�(" + _key.ToString() + ")==true");
                            }
                        }
                    }
                    // �S�ẴL�[�i�}�E�X�{�^���A�_�u���N���b�N�A���������܂ށj���`�F�b�N
                    bool _isTestShowAllEKeyCodes = false; // ������true�ɂ���Ə����d���Ȃ�i20�~���b���ɖ�500���[�v�Ō��\�t�@�����邳���Ȃ�j�̂Œ��ӁB
                    if (_isTestShowAllEKeyCodes == true)
                    {
                        foreach (EKeyCode _key in Enum.GetValues(typeof(EKeyCode)))
                        {
                            // �f�o�b�O�p
                            //if (_key == EKeyCode.MOUSE_LEFT_DOUBLECLICK)
                            //{
                            //    int a = 0;
                            //}
                            if (game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(_key) == true)
                            {

                                _buttonInfo += ("\n" + _key.ToString() + " �L�[�������ꂽ��: game.i�w��L�[����������_�A�˔�Ή�(" + _key.ToString() + ")==true");
                            }
                        }
                    }
                    #region �ꕔ�̎w��L�[�����m�F����ꍇ
                    bool _isTestOnlyPartKeys = true;
                    if (_isTestOnlyPartKeys == true)
                    {
                        // �}�E�X�N���b�N
                        // �������ł��ł��邯�ǁA�}�E�X�����ˑ��̎���������A���܂�g��Ȃ���// if (game.getP_mouseInput().IsPush(EMouseButton.Left) == true) 
                        // �ʏ�͂��������g���Ă�
                        // if(game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_LEFT_CLICK) == true)
                        //{
                        //    _buttonInfo += ("\n"+"�}�E�X���N���b�N�������ꂽ��");
                        //}
                        // �t�H�[�����̈ꎞ�I�ȃ}�E�X�N���b�N�i�e�X�g�Ŏg���Ă邪�Afalse�̏����������܂������Ȃ��Ƃ�������̂ŁA�X���b�h�Ō��m���Ă���j
                        //if (game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O == true)
                        //{
                        //    //_buttonInfo += ("\n"+"�}�E�X���N���b�N�������ꂽ�� : game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O");
                        //    game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O = false; // �}�E�X�N���b�N���Z�b�g
                        //}

                        // �L�[�P��
                        //if (game.ik�w��L�[����������_�������ϘA�ˑΉ�(EKeyCode.ENTER) == true)
                        //{
                        //    _buttonInfo += ("\n" + "ENTER�������ꂽ��: game.ik�w��L�[����������_�������ϘA�ˑΉ�(EKeyCode.ENTER");
                        //}

                        // �{�^���P��
                        //if (game.ib�{�^������������_�A�˔�Ή�(EInputButton�E���̓{�^��.b1_����{�^��_A) == true)
                        //{
                        //    _buttonInfo += ("\n" + "����{�^���iA�j�i�A�˔�Ή��j�������ꂽ��: game.ib�{�^������������_�A�˔�Ή�(EInputButton�E���̓{�^��.b1_����{�^��_A");
                        //}
                        // ���L�͏�L�ƈꏏ
                        //if (game.getP_InputButton().isPulled�E�{�^���������������u�Ԃ�(EInputButton�E���̓{�^��.b1_����{�^��_A) == true)
                        // �A�ˑΉ��͉������ςȂ��ŘA�˂ɂȂ�
                        //if (game.ib�{�^������������_�A�ˑΉ�(EInputButton�E���̓{�^��.b1_����{�^��_A) == true)
                        //{
                        //    _buttonInfo += ("\n" + "����{�^���iA�j�y�A�ˑΉ��z�������ꂽ��: game.ib�{�^������������_�A�ˑΉ�(EInputButton�E���̓{�^��.b1_����{�^��_A");
                        //}
                        //if (game.ib�{�^������������_�A�ˑΉ�(EInputButton�E���̓{�^��.b2_�߂�{�^��_B) == true)
                        //{
                        //    _buttonInfo += ("\n" + "�o�b�N�{�^���iB�j�y�A�ˑΉ��z�������ꂽ��: game.ib�{�^������������_�A�ˑΉ�(EInputButton�E���̓{�^��.b2_�߂�{�^��_B");
                        //}
                        // �_�u���N���b�N�p
                        if (game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_LEFT_DOUBLECLICK) == true)
                        {
                            _buttonInfo += ("\n" + "�}�E�X���_�u���N���b�N������: game.i�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_LEFT_DOUBLECLICK) == true");
                        }
                        if (game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_LEFT_TRIPLECLICK) == true)
                        {
                            _buttonInfo += ("\n" + "�}�E�X���g���v���N���b�N������: game.i�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_LEFT_TRIPLECLICK) == true");
                        }
                        if (game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_RIGHT_DOUBLECLICK) == true)
                        {
                            _buttonInfo += ("\n" + "�E�_�u���N���b�N������: game.i�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_RIGHT_DOUBLECLICK) == true");
                        }
                        if (game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_RIGHT_TRIPLECLICK) == true)
                        {
                            _buttonInfo += ("\n" + "�E�g���v���N���b�N������: game.i�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_LEFT_TRIPLECLICK) == true");
                        }
                        if (game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_LEFT_PRESSLONG) == true)
                        {
                            _buttonInfo += ("\n" + "�}�E�X����������������: game.i�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.MOUSE_LEFT_PRESSLONG) == true");
                        }
                    }
                    #endregion
                    // ���������̃`�F�b�N
                    bool _isTestDoubleKeysPress = true;
                    if (_isTestDoubleKeysPress == true)
                    {
                        if (game.ib�{�^���𓯎���������_�A�˔�Ή�(EInputButton�E���̓{�^��.b1_����{�^��_A, EInputButton�E���̓{�^��.b2_�߂�{�^��_B) == true)
                        {
                            _buttonInfo += ("\n" + "A+B�{�^���i����{�^���{�o�b�N�{�^���j�������������ꂽ��: game.ib�{�^���𓯎���������_�A�˔�Ή�(EInputButton�E���̓{�^��.b1_����{�^��_A, EInputButton�E���̓{�^��.b2_�߂�{�^��_B) == true");
                        }
                        if (game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.LCTRL_DOUBLECLICK, EKeyCode.s) == true)
                        {
                            _buttonInfo += ("\n" + "��Ctrl+S�L�[�������������ꂽ��: game.ik�w��L�[�������������u�Ԃ�_�������ϘA�˔�Ή�(EKeyCode.LCTRL_DOUBLECLICK, EKeyCode.s) == true");
                        }
                    }

                    // �Ō�ɂ��̃X���b�h�̌v�Z���Ԃ��v�Z���A�W���o�͂⃉�x���ɕ\���B
                    string _passedMSecString = " Form�X���b�h�v�Z����: " + (MyTools.getNowTime_fast() - _t1) + "�~���b";
                    //�ŏ��̉��s���폜
                    if (_buttonInfo.Length > 0) _buttonInfo = _buttonInfo.Substring(1);
                    // �����X�V����Ă���΁A�{�^������\��
                    if (_buttonInfo.Length > 0)
                    {
                        // �Ō�̍s�����\��
                        labInfo1.Text = "���̓{�^�����:" + MyTools.getLineString(_buttonInfo, MyTools.getLineNo(_buttonInfo)) + _passedMSecString;
                    }
                    // �W���o�͂ɕ\������̂̏����͂ǂ�����H
                    if (_buttonInfo.Length > 0) MyTools.ConsoleWriteLine(_buttonInfo + " " + _passedMSecString);
                    #endregion // �{�^���e�X�g�I��
                }

                #region �ȉ��A�e�X�g���ă���
                // �ȉ��A�`��e�X�g

                //p_�Q�[�����.doTimerEvent(sender, _e);

                //Win32Window2DGl _�`��G���W�� = new Win32Window2DGl();
                //_�`��G���W��.InitByHWnd(this.Handle); // ���̃t�H�[�����G���W���Ƃ��ď�����
                //Screen2DGl _�X�N���[�� = _�`��G���W��.Screen;
                //GlTexture _�摜 = new GlTexture();

                //// ���u�����h�i���������j�̐ݒ�
                //_�X�N���[��.Blend = true;
                //_�X�N���[��.Select();
                //{
                //    // �`��摜�̓ǂݍ���
                //    string _�摜�t�@�C���� = Program�E�v���O����.p_ImageDataDirectory�E�摜�t�H���_�p�X + "mxp159.png"; //_ResourceDirectory + "isya02.gif";
                //    _�摜.Load(_�摜�t�@�C����);


                //    // �X�N���[���ɑ΂��鏈���͂����ɏ���

                //    for (int i = 0; i < 1000; i++)
                //    {
                //        _�X�N���[��.Clear(); // �ĕ`��
                //        _�X�N���[��.Blt(_�摜, i, 0); // �摜��(0i,0)�ɓ��{���ŕ\��
                //        p_fpsTimer.WaitFrame();
                //        Program�E�v���O����.printlnLog(ELogType.l4_�d�v�ȃf�o�b�O, " " + i + ": �`�悵�Ă܂��D");
                //    }
                //    // �X�N���[���ɑ΂��鏈���͂����܂�
                //}
                //_�X�N���[��.Update();
                ////_�X�N���[��.Unselect();


                //// 2.�e�L�X�g�`��e�X�g

                //string _message = "_onelineString�C�C�Ca�E�E�Eaa�B_onelineString\n�����͂����������������B";
                //int _�\���������b�Z�[�W�� = 0;

                //while (_�\���������b�Z�[�W�� == _message.Length)
                //{
                //    // �t���[������Ă���H
                //    if (p_fpsTimer.ToBeRendered)
                //    {
                //        // ���b�Z�[�W��1�����\��
                //        game.m���b�Z�[�W_�P�̖������s�Ȃ�_�{�^������(_message);
                //        _�\���������b�Z�[�W��++;

                //        p_fpsTimer.WaitFrame(); // �t���[���I���܂ő҂�
                //    }
                //}
                #endregion
            }



        }
            #region �ȉ��A���̃^�C�}�[�̎������@�̑��ă���
        // FpsTimer���͎g���ĂȂ��B
        /// <summary>
        /// fps���Ǘ�����N���X�ł��B
        /// </summary>
        //FpsTimer p_fpsTimer = new FpsTimer();

        // �^�C�}�[�X���b�h�����Timer�N���X�B���͎g���ĂȂ��B
        /// <summary>
        /// ��莞�Ԃ��Ƃɉ��������������������Ɏg���A
        /// �i�e�N���X�����R�ɒ�����s������ǉ����邱�Ƃ̂ł���jOnCallback�p�^�C�}�ł��B
        /// 
        /// �Ƃ��郁�\�b�h�������s�����ɂ��������́A
        /// 
        ///                 p_OnCallbackTimer.Tick += delegate { ���\�b�h��(); };
        /// �Ƃ����āA���̃��\�b�h���ň�莞�Ԃ��Ƃɏ���������悤�ɂ���ƕ֗��ł��B
        ///
        /// 
        /// �V�����^�C�}�[����肽���ꍇ�́A�ȉ������񂱂��ɂ��Ă��������B
        ///        timer = new System.Windows.Forms.Timer();
        ///
        ///        timer.Interval = 1;
        ///        timer.Tick += delegate { OnCallback(); };
        ///        timer.Start();
        ///        
        /// 		public void OnCallback()
        ///         {
        ///             if ( FpsTimer.ToBeRendered )
        ///                 return;
        ///
        ///             // �t���[���X�L�b�v����
        ///             OnMove(); // �_���I�Ȉړ� 
        ///             if ( gameContext.FPSTimer.ToBeSkip ){
        ///                 return; 
        ///             }
        ///             OnDraw(); // ��ʕ`��
        ///         }
        /// </summary>
        //private System.Windows.Forms.Timer p_OnCallbackTimer;
        #endregion

        #endregion


        // �ȉ��A�t�H�[���ƃt�H�[���R���g���[���̃C�x���g�B
        // �����Ɏ��̏�����������Windows�t�H�[���ˑ��̕����������邽�߁A���܂菑���Ȃ��ł��������B

        private void butNext���փ{�^��_Click(object sender, EventArgs e)
        {
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                EControlType�E����R���g���[��.c0a_GoNext�E���֐i�ރ{�^��, EInputButton�E���̓{�^��.b1_����{�^��_A);
            // �u���ցv�{�^�����N���b�N�����Ƃ��̏����́A����{�^�������������ƕς��Ȃ��B
        }

        private void butBack�߂�{�^��_Click(object sender, EventArgs e)
        {
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                EControlType�E����R���g���[��.c0b_GoBack�E�O�֖߂�{�^��, EInputButton�E���̓{�^��.b1_����{�^��_A);
            // �u�߂�v�{�^�����N���b�N�����Ƃ��̏����́A�߂�{�^�������������ƕς��Ȃ��B
        }

        #region �t�H�[���ƃt�H�[���R���g���[���̃C�x���g

        private void FGameTestForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // �E�B���h�E������Ƃ��C�A�v���P�[�V�������I�����܂��D
            // �������Q�[���I���̌�n���I
            game.End�E�Q�[���I������();
        }
        // �t�H�[���Ƀt�H�[�J�X�����������̏���
        private void FGameBattleForm1_Enter(object sender, EventArgs e)
        {
            // �悭�킩��Ȃ����A�����̓u���[�N�|�C���g���w�肵�Ă��������s����Ă��Ȃ��B�t�H�[���t�H�[�J�X�C�x���g�͎��Ȃ��H

            // �t�H�[���Ƀt�H�[�J�X�������������i�t�H�[�����N���b�N�����Ƃ��j�A
            // �^����Ƀt�H�[�J�X�𓖂Ă�R���g���[��
            focusMainControl�E�t�H�[�J�X�����C���R���g���[���Ɉڂ�();
        }
        // �t�H�[�����N���b�N�����Ƃ��̏���
        private void FGameTestForm1_Click(object sender, EventArgs e)
        {
            // ���C���e�L�X�g�{�b�N�X���̃e�L�X�g�Ƀt�H�[�J�X���������Ă�����A�e�L�X�g�ҏW���̂��߁A���ɐi�܂Ȃ��B�ifalse�̎������i�߂�j
            if (mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Focused == false)
            {
                // ���C���e�L�X�g�{�b�N�X�Ƀt�H�[�J�X���������Ă��Ȃ����A����{�^������
                _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(
                    EControlType�E����R���g���[��.c01_Form�E�t�H�[��, EInputButton�E���̓{�^��.b1_����{�^��_A);
            }
            else
            {
                // �t�H�[�����N���b�N�����Ƃ��A���C���e�L�X�g�{�b�N�X�Ƀt�H�[�J�X�𓖂����Ă��Ȃ��Ȃ�΁A
                // �^����Ƀt�H�[�J�X�𓖂Ă�R���g���[���i�����C���R���g���[���j
                focusMainControl�E�t�H�[�J�X�����C���R���g���[���Ɉڂ�();
            }
        }

        /// <summary>
        /// �t�H�[����̑S�ẴR���g���[�����܂߂āi���j�CEnter�L�[���������Ƃ��̏����ł��E
        /// �i�������C���́Cthis.KeyPreview = true�̂Ƃ��B
        /// this.KeyPreview = false�̂Ƃ��́A���̃R���g���[���Ƀt�H�[�J�X���������Ă��Ȃ����������s�����j
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void FGameTestForm1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                // Enter�Ńt�H�[�J�X�����̃R���g���[���Ɉړ�������B

                Control _activeControl = this.ActiveControl; // = _senderControl;�Ɠ���
                bool isFocusMoveCancel = false; // �t�H�[�J�X�ړ����L�����Z�����邩�ǂ���
                // Enter�ŉ��s�Ȃǂ����镡���s���͉\�ȃe�L�X�g�{�b�N�X�R���g���[���̏ꍇ�́C�t�H�[�J�X�ړ����Ȃ�
                if (_activeControl is TextBox)
                {
                    if (((TextBox)_activeControl).Multiline == true)
                    {
                        isFocusMoveCancel = true;
                    }
                }

                if (isFocusMoveCancel == false)
                {
                    // �t�H�[�J�X�̈ړ������i�V�t�g�������Ƌt�j
                    bool isfocusMoveForward = e.Modifiers != Keys.Shift;
                    //this.ProcessTabKey(isfocusMoveForward);
                    this.SelectNextControl(this.ActiveControl, isfocusMoveForward, true, true, true);
                    // TabStop�𖳎����Ĉړ������̂ɁA�ړ���̃R���g���[����TabStop�v���p�e�B��false�̂��̂����Ȃ��ꍇ�C
                    // �d�����Ȃ��̂Ńt�H�[���Ƀt�H�[�J�X��߂�
                    if (this.ActiveControl.TabStop == false)
                    {
                        this.Focus();
                    }
                }
            }
            // ���C���e�L�X�g�{�b�N�X���̃e�L�X�g�Ƀt�H�[�J�X���������Ă�����A�e�L�X�g�ҏW���̂��߁A���ɐi�܂Ȃ��B�ifalse�̎������i�߂�j
            if (mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Focused == false)
            {
                // ��(a)����{�^�������C������������Ȃ��ƁC���͊m�肪�ł��܂���D�K���������Ă��������I
                //if (_e.KeyCode == Keys.Enter || _e.KeyCode == Keys.Space || _e.KeyCode == Keys.Z)
                //{
                //    game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O = true;
                //    game.p_isEndSelectBox�E�I���{�b�N�X�����t���O = true;
                //}
                // ��(b)����{�^�������C��L�̑���ɂ���ōs��
                _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(
                    EControlType�E����R���g���[��.c01_Form�E�t�H�[��, e.KeyCode);
            }
        }

        // ���C�����b�Z�[�W�{�b�N�XrichTextBox1�i=mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X�j���N���b�N�����Ƃ��̏���
        private void richTextBox1_Click(object sender, EventArgs e)
        {
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                EControlType�E����R���g���[��.c02_MessageBox�E���C�����b�Z�[�W�{�b�N�X,
                EInputButton�E���̓{�^��.b1_����{�^��_A); // �����A�V���v���Ɍ���{�^���ł����̂��H�t�q�k�N���b�N���́H
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // http://dobon.net/vb/dotnet/control/tbsuppressbeep.html
            // ���C�����b�Z�[�W�{�b�N�X�ŃL�[���͂����Ƃ��AEnter��Escape�L�[�ȂǂŃr�[�v������Ȃ��悤�ɂ���
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape
                || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right
                || e.KeyCode == Keys.Z || e.KeyCode == Keys.Space || e.KeyCode == Keys.Back || e.KeyCode == Keys.X)

            {
                e.Handled = true; // ����Ńr�[�v������Ȃ��悤�ɂł���
            }
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                EControlType�E����R���g���[��.c02_MessageBox�E���C�����b�Z�[�W�{�b�N�X, e.KeyCode);
        }

        /// <summary>
        /// ���C�����b�Z�[�W�{�b�N�X�Ƀt�H�[�J�X�������������̏����ł��D
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            // ��������ƃe�L�X�g�̑I����R�s�y���ł��Ȃ��B�B
            //// ���C�����b�Z�[�W�{�b�N�X�Ƀt�H�[�J�X��������Ȃ��悤�ɂ��܂��D�t�H�[�����̂��̂��Ƃ��ƌ��ʂ��Ȃ������悤�ł��B
            //if (game.getP_gameWindow�E�Q�[�����().getP_usedFrom() != null)
            //{
            //    game.getP_gameWindow�E�Q�[�����().getP_usedFrom().focusC1_dice();
            //}
        }
        /// <summary>
        /// ���C�����b�Z�[�W�{�b�N�X����t�H�[�J�X���Ȃ��Ȃ������̏����ł��B
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            //// 1.�A���t�H�[�J�X�̑O�ɑI���s���Ō�ɂ��Ȃ��ƁA�ŏ��̍s�ɖ߂��Ă��܂��B
            //MyTools.showRichTextBox_EndLine_UnshowCursor(mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X, this);
            //// ���C�����b�Z�[�W�{�b�N�X�Ƀt�H�[�J�X��������Ȃ��悤�ɂ��܂��D�t�H�[�����̂��̂��Ƃ��ƌ��ʂ��Ȃ������悤�ł��B
            //if (game.getP_gameWindow�E�Q�[�����().getP_usedFrom() != null)
            //{
            //    focusMainControl�E�t�H�[�J�X�����C���R���g���[���Ɉڂ�();
            //}
            //���ꂶ�Ⴀ�M�U�M�U�ɂȂ��Ėڂ�����mainRichTextBox�E���C���e�L�X�g�{�b�N�X.SelectionStart = mainRichTextBox�E���C�����b�Z�[�W�{�b�N�X.Text.Length; // �Ō�ɂ���B

        }


        /// <summary>
        /// �u���͊m��v�{�^���N���b�N���B
        /// �����̏����́u���ցv�{�^���Ɠ��������̂ŁA���͎g���Ă��Ȃ��B
        /// </summary>
        private void buttonInput�m��{�^��_Click(object sender, EventArgs e)
        {
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(
                EControlType�E����R���g���[��.c0a_GoNext�E���֐i�ރ{�^��,
                EInputButton�E���̓{�^��.b1_����{�^��_A);
        }



        // ���I���{�b�N�X�ł���listBox1�́A_Click��_KeyDown�C�x���g�͕K���������C�x���g�ǉ����āB
        /// <summary>
        /// �I���{�b�N�X�ōŌ�ɑI�������C���f�b�N�X�������܂��B���I���̏ꍇ��-1�������Ă��܂��B
        /// </summary>
        public int p_SelectBox_beforeSelectedIndex�E���݂̑I���� = -1;
        /// <summary>
        /// �I�����N���b�N��
        /// </summary>
        private void listBox1_Click(object sender, EventArgs e)
        {
            // ��(a)���I���ς݂̍��ڂ��N���b�NorEnter/Space/Z����������I���m��
            // �ł��ĂȂ�

            // ��(b)�������ڂ�2��N���b�N������I���m��
            int _nowSelectedIndex = mainSelectBox�E�I�������X�g�{�b�N�X.SelectedIndex;
            // ������{�^�������C������������Ȃ��ƁC���͊m�肪�ł��܂���D�K���������Ă��������I
            if (_nowSelectedIndex == p_SelectBox_beforeSelectedIndex�E���݂̑I����)
            {
                // ��(b)����{�^������
                _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(
                    EControlType�E����R���g���[��.c03_SelectBox�E�I���{�b�N�X, EInputButton�E���̓{�^��.b1_����{�^��_A);
            }
            p_SelectBox_beforeSelectedIndex�E���݂̑I���� = _nowSelectedIndex;
        }
        /// <summary>
        /// �I�����ŃL�[����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // ������{�^�������C������������Ȃ��ƁC���͊m�肪�ł��܂���D�K���������Ă��������I
            //if (_e.KeyCode == Keys.Enter || _e.KeyCode == Keys.Space || _e.KeyCode == Keys.Z)
            //{
            // _e.BackSpace�̎�������̂ŁA�L�[�����̂܂ܓn��
                _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(
                    EControlType�E����R���g���[��.c03_SelectBox�E�I���{�b�N�X, e.KeyCode);
            //}
        }
        //private void listBox1_KeyPress(object sender, KeyPressEventArgs _e)
        //{
        //    if (_e.KeyChar.ToString() == Keys.Enter.ToString() || _e.KeyChar.ToString() == Keys.Space.ToString())
        //    {
        //        // ���Ԃ񂱂�C���s����ĂȂ��E�E�EKeyPressEventArgs.KeyChar��e.KeyCode�͓������Ȃ��DKeyDown�ɂ�����������D
        //        //game.p_isEndSelectBox�E�I���{�b�N�X�����t���O = true;
        //    }
        //}
        /// <summary>
        /// �I�����̑I�����ڂ��ς������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion

        // �����������������������ȉ��A���̃R���g���[���̃C�x���g

        private void picScinario_Click(object sender, EventArgs e)
        {

        }

        private void butTest���낢��e�X�g_Click(object sender, EventArgs e)
        {
            test���낢��e�X�g();
        }
        /// <summary>
        /// �u���낢��e�X�g�v�{�^�������������̏���
        /// </summary>
        public void test���낢��e�X�g()
        {
            bool _isTestButton�{�^���e�X�g = true;
            if (_isTestButton�{�^���e�X�g == true)
            {
                if (p_isTimerTestButton�E�������{�^�������x���ɕ\������e�X�g�� == false)
                {
                    // �^�C�}�[�X���b�h�Ƀe�X�g�J�n��`����
                    p_isTimerTestButton�E�������{�^�������x���ɕ\������e�X�g�� = true;

                    //�����ł�邱�Ƃ���Ȃ���������game.p_isEndUserInput_GoNextOrBack�E���͑҂������t���O = false; // �}�E�X�N���b�N���Z�b�g
                    butTest���낢��e�X�g.Text = "�{�^���e�X�g���c";
                    butTest���낢��e�X�g.BackColor = Color.Green;
                    //butTest���낢��e�X�g.Enabled = false; // �A���N���b�N��h��
                    // �҂��Ȃ��ƁA���̃e�X�g�{�^�����N���b�N�����Ƃ��̏������󂯕t���Ă����I�������Ⴄ�̂ŁA��莞�ԑ҂B
                    game.wait�E�F�C�g(500);

                    //MyTools.ConsoleWriteLine("\n���� SPACE�L�[�������܂ŁA�{�^���e�X�g�J�n");
                    //labInfo1.Text = ("���� SPACE�L�[�������܂ŁA�{�^���e�X�g�J�n");
                    //// ���[�U��Enter��Space���A�}�E�X���N���b�N���������܂ő҂�
                    //int _passedMSec = 0;
                    //int _diceRolateMSec = 1000;
                    //while (game.ik�w��L�[����������_�������ϘA�ˑΉ�(EKeyCode.SPACE) == false && Program�E�v���O����.isEnd == false)
                    //{
                    //    game.wait�E�F�C�g(_diceRolateMSec);
                    //    _passedMSec += _diceRolateMSec;
                    //}
                    //MyTools.ConsoleWriteLine("\n���� SPACE�L�[�������ꂽ�̂ŁA�{�^���e�X�g�I��\n");
                    //labInfo1.Text = ("���� SPACE�L�[�������ꂽ�̂ŁA�{�^���e�X�g�I��");
                    //butTest���낢��e�X�g.BackColor = Color.Gray;
                    //butTest���낢��e�X�g.Enabled = true; // �A���N���b�N��h��
                    //butTest���낢��e�X�g.Text = "���낢��e�X�g";
                    //// �e�X�g�I�����^�C�}�[�X���b�h�ɓ`����
                    //p_isTimerTestButton�E�������{�^�������x���ɕ\������e�X�g�� = false;
                }
                else
                {
                    butTest���낢��e�X�g.BackColor = Color.Gray;
                    //butTest���낢��e�X�g.Enabled = true; // �A���N���b�N��h��
                    butTest���낢��e�X�g.Text = "���낢��e�X�g";
                    // �e�X�g�I�����^�C�}�[�X���b�h�ɓ`����
                    p_isTimerTestButton�E�������{�^�������x���ɕ\������e�X�g�� = false;
                }
            }
        }
        // �������������������������������̃R���g���[���̃C�x���g�A�I���


        // �����������������������������ȉ��A���j���[�̃C�x���g

        private void �I��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.End�E�Q�[���I������();
        }

        private void ���߂���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _startDiceBattleGame�E�_�C�X�o�g���Q�[��();
        }

        /*
        private void BGM�Đ�ToolStripMenuItem_Click(object sender, EventArgs _e)
        {
            if (game.s_optionBGM_ON�E�a�f�l���Đ������Ԃ� == true)
            {
                BGM�Đ�ToolStripMenuItem.Checked = false;
                game.s_optionBGM_ON�E�a�f�l���Đ������Ԃ� = false;
                MyTools.stopSound();
            }
            else
            {
                BGM�Đ�ToolStripMenuItem.Checked = true;
                game.s_optionBGM_ON�E�a�f�l���Đ������Ԃ� = true;
                MyTools.stopSound();
                game.pBGM(game.p_nowBGM�E���݂̍Đ��ȃt���p�X);
            }
        }
         */


        private void ���yToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CGameManager�E�Q�[���Ǘ���.s_optionBGM_ON�E�a�f�l���Đ������Ԃ� == true)
            {
                ���yToolStripMenuItem.Checked = false;
                CGameManager�E�Q�[���Ǘ���.s_optionBGM_ON�E�a�f�l���Đ������Ԃ� = false;
                game.pBGM();
            }
            else
            {
                ���yToolStripMenuItem.Checked = true;
                CGameManager�E�Q�[���Ǘ���.s_optionBGM_ON�E�a�f�l���Đ������Ԃ� = true;
                game.stopBGM�E�a�f�l���ꎞ��~();
                // �t�H���_�ɂ���mp3�t�@�C���������_���Đ�
                game.pBGMRandom_FromDirectory();
                //game.pBGM(game.p_nowBGM�E���݂̍Đ��ȃt���p�X);
            }
        }


        public bool p_isMicRecord = false;
        public int p_MicRecord_StartMSec = 0;
        private void but�{�C�X�^��_Click(object sender, EventArgs e)
        {

            if (p_isMicRecord == false)
            {
                if (MySound_Windows.MCI_recordMic_Start() == true) //MySound_Windows.recordMic_Start() == true)
                {
                    game.m���b�Z�[�W_�u���ɕ\��("�^���J�n...");
                    p_isMicRecord = true;
                    p_MicRecord_StartMSec = MyTools.getNowTime_fast();
                }
                else
                {
                    game.m���b�Z�[�W_�u���ɕ\��("�^�����s�i�����ɘ^�����ł��j�B");
                    p_isMicRecord = false;
                }
            }
            else
            {
                string _filename = MySound_Windows.MCI_recordMic_Stop();//MySound_Windows.recordMic_Stop();
                int _recordingMSec = MyTools.getNowTime_fast() - p_MicRecord_StartMSec;
                game.m���b�Z�[�W_�u���ɕ\��("�^���I���i�����ɘ^�������T�E���h" + _recordingMSec + "�~���b���Đ����j");
                p_isMicRecord = false;

                // �Đ��I���܂ő��̑���𖳌��ɂ���
                MyTools.setFormNowLoading_DamyPictureBox(this, true, true);
                // �����ɍĐ�
                MySound_Windows.playSE(_filename, false);
                //MySound_Windows.play_LastRecordedMicSound();
                // �Đ��I����A�t�@�C�����N���[�Y���Ȃ��Ə㏑���ł��Ȃ��̂ŁA�Đ����Ԃ����҂��āA���ꂩ��N���[�Y����
                game.wait�E�F�C�g(_recordingMSec);
                MySound_Windows.stopSE();
                game.m���b�Z�[�W_�u���ɕ\��("�^�������T�E���h�̍Đ��I��");
                // �Đ��I���܂ő��̑���𖳌��ɂ���
                MyTools.setFormNowLoading_DamyPictureBox(this, false, true);
            }
        }

        private void �I�v�V����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void �e�X�g�p�����Đ���ʂ�\��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SamplePlayer.Form1 _���y�Đ���ʃt�H�[�� = new SamplePlayer.Form1();
            _���y�Đ���ʃt�H�[��.Show();
        }

        private void �T�E���h�ݒ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // �T�E���h�R���t�B�O�t�H�[����\��
            FSoundConfig _soundConfigForm = new FSoundConfig(game);
            _soundConfigForm.Show();
        }

        private void FGameBattleForm1_Activated(object sender, EventArgs e)
        {
            if (game.getP_gameWindow�E�Q�[�����() != null)
            {
                game.getP_gameWindow�E�Q�[�����().setisFoucused�E�t�H�[�J�X���������Ă��邩��ύX(true);
            }
        }

        private void FGameBattleForm1_Deactivate(object sender, EventArgs e)
        {
            if (game.getP_gameWindow�E�Q�[�����() != null)
            {
                game.getP_gameWindow�E�Q�[�����().setisFoucused�E�t�H�[�J�X���������Ă��邩��ύX(false);
            }
        }

        private void textInput1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // ���̓{�b�N�X�P��Enter�L�[����������A���̓��̓{�b�N�X�ֈړ��iTab�L�[�݂����ȑ���̎������j
                _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                    EControlType�E����R���g���[��.c04_InputBox�E���̓{�b�N�X�P����R�̂ǂꂩ, EInputButton�E���̓{�^��.b1_����{�^��_A);
            }
        }

        private void textInput2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // ���̓{�b�N�X�Q��Enter�L�[����������A���̓��̓{�b�N�X�ֈړ��iTab�L�[�݂����ȑ���̎������j
                _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                    EControlType�E����R���g���[��.c04_InputBox�E���̓{�b�N�X�P����R�̂ǂꂩ, EInputButton�E���̓{�^��.b1_����{�^��_A);
            }
        }

        private void textInput3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // ���̓{�b�N�X�R��Enter�L�[����������A���̓��̓{�b�N�X�ֈړ��iTab�L�[�݂����ȑ���̎������j
                _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                    EControlType�E����R���g���[��.c04_InputBox�E���̓{�b�N�X�P����R�̂ǂꂩ, EInputButton�E���̓{�^��.b1_����{�^��_A);
            }
        }

        private void butNext���փ{�^��_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter�����łȂ��A�������ςȂ��ɑΉ�
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                EControlType�E����R���g���[��.c0a_GoNext�E���֐i�ރ{�^��, e.KeyCode);
        }

        private void butBack�߂�{�^��_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter�����łȂ��A�������ςȂ��ɑΉ�
            _checkNextEvent_ByFormControl_UserInput�E���[�U�̓��̓C�x���g����(sender,
                EControlType�E����R���g���[��.c0b_GoBack�E�O�֖߂�{�^��, e.KeyCode);
        }

        private void �V�X�e���N���G�[�V�����ݒ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool _switch = game.getP_Battle�E�퓬().p_is�����□���̃_�C�X�R�}���h��true�X���b�g��]��_false�X�g�b�v�����Ď��R�I����;
            // �X�C�b�`��؂�ւ�
            _switch = !_switch;
            // ���
            game.getP_Battle�E�퓬().p_is�����□���̃_�C�X�R�}���h��true�X���b�g��]��_false�X�g�b�v�����Ď��R�I���� = _switch;
        }

        private void �퓬�����Q�����퓬�����X�L�b�v�pToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setBattleAuto�E�����퓬���[�h�ɂ���();
        }

        private void �퓬�����Q�X���b�g�`��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setBattleSlot�E�X���b�g�퓬���[�h�ɂ���(false, false);
        }

        private void �퓬�����Q�X���b�g�������Ō���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setBattleSlot�E�X���b�g�퓬���[�h�ɂ���(true, false);
        }
        
        private void �퓬�����Q�R�}���h�I�����ʏ�q�o�f��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setBattleCommandSelect�E�R�}���h�I��퓬���[�h�ɂ���();
        }

        private void �퓬�����Q�ԍ��������������X�^�[�t�@�[����ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        // �ȉ��A�Q�[����Փx
        private void cosmos���[�U�ɍ��킹�Ď�������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx._LVNone_Cosmos�E���[�U�̃v���C�󋵂ɍ��킹�Ď�������);
        }

        private void eden�y��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx.LV00_Eden�E�܂��Ɋy��);
        }

        private void ��Փx�V��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx.LV01_VeryEasy�E�ƂĂ��₳����);
        }

        private void ��Փx�D����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx.LV02_Easy�E�₳����);
        }

        private void ��Փx�W��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx.LV03_Normal�E����);
        }

        private void ��Փx���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx.LV04_Hard�E�ނ�������);
        }

        private void ��Փx����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx.LV05_VeryHard�E�ƂĂ��ނ�������);
        }

        private void ��Փx�_�X�N���XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx.LV06_God�E�_�X�N���X);
        }

        private void ��Փx�F������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.setP_gameLV�E��Փx��ύX(EGameLV�E��Փx.LV10_Kaos�E�܂��ɃJ�I�X);
        }

        private void �e�X�g�p�_���[�W�v�Z�@�ݒ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.showBattleDamageCalcForm�E�_���[�W�v�Z�@��ʂ�\��();
        }

        private void �e�X�g�p�o�����X����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.showBattleForm�E�o�����X������ʂ�\��();
        }

        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            MyTools.showPictureBoxImage_ByDragAndDrop_Part1(sender, e);
        }
        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            Image _image = null;
            string _filename_FullPath = MyTools.showPictureBoxImage_ByDragAndDrop_Part2(sender, e, out _image, true);
            // �L�����̉摜������΁A�L�����摜�Ƃ��ĕۑ�
            if (_image != null)
            {
                CChara�E�L���� _playerChara = MyTools.getListValue<CChara�E�L����>(game.getP_Battle�E�퓬().p_charaPlayer�E�����L����, game.getP_Battle�E�퓬().p_charaPlayer_Index�E�����L����_��l��ID);
                if (_playerChara != null)
                {
                    setCharaImage�E�L�����̃T���l�C���[�W��ݒ�(_image, _filename_FullPath, _playerChara);
                }
            }
        }

        public static void setCharaImage�E�L�����̃T���l�C���[�W��ݒ�(Image _image, string _filename_FullPath, CChara�E�L���� _chara)
        {
            _chara.setVar�E�ϐ���ύX(EVar.�T���l�摜, _image);
            _chara.setVar�E�ϐ���ύX(EVar.�t�@�C���t���p�X_�T���l�摜, _filename_FullPath);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MyTools.MessageBoxShow("�t�@�C�����h���b�O�A���h�h���b�v���āA�D���ȉ摜�t�@�C����ݒ�ł��܂��B");
        }


        private void pictureBox2_DragEnter(object sender, DragEventArgs e)
        {
            MyTools.showPictureBoxImage_ByDragAndDrop_Part1(sender, e);
        }
        private void pictureBox2_DragDrop(object sender, DragEventArgs e)
        {
            Image _image = null;
            string _filename_FullPath = MyTools.showPictureBoxImage_ByDragAndDrop_Part2(sender, e, out _image, true);
            // �L�����̉摜������΁A�L�����摜�Ƃ��ĕۑ�
            // �i�G�L�����̕��͕ۑ����邩�͂܂��l���Ă��Ȃ��j
            if (_image != null)
            {
                CChara�E�L���� _enemyChara = MyTools.getListValue<CChara�E�L����>(game.getP_Battle�E�퓬().p_charaEnemy�E�G�L����, game.getP_Battle�E�퓬().p_charaEnemy_Index�E�G�L����_���[�_�[ID);
                if (_enemyChara != null) _enemyChara.setVar�E�ϐ���ύX(EVar.�T���l�摜, _image);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MyTools.MessageBoxShow("�t�@�C�����h���b�O�A���h�h���b�v���āA�D���ȉ摜�t�@�C����ݒ�ł��܂��B");
        }






        // �������������������������������j���[�̃C�x���g�A�I���


        // AxWindowsMediaPlayer1��ǉ�������A�ςȃG���[���o��̂ō폜�B
        ///// <summary>
        ///// AxWindowsMediaPlayer�R���g���[����ǉ��i�j http://dobon.net/vb/dotnet/programing/playmidifile.html#wmp
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="_e"></param>
        //private void axWindowsMediaPlayer1_Enter(object sender, EventArgs _e)
        //{

        //}
        //public void playSound_ByWindowsMediaPlayer1(string _fileName_FullPath_or_NotFullPath)
        //{
        //    //URL�v���p�e�B���w�肳�ꂽ�玩���I�ɍĐ������悤�ɂ���
        //    axWindowsMediaPlayer1.settings.autoStart = true;
        //    //�I�[�f�B�I�t�@�C�����w�肷��i�����I�ɍĐ������j
        //    axWindowsMediaPlayer1.URL = _fileName_FullPath_or_NotFullPath;

        //    //autoStart��false�̂Ƃ��́A���̂悤�ɂ��čĐ�����
        //    //axWindowsMediaPlayer1.Ctlcontrols.play();
        //}




        /* �ȉ��C�Ȃ��Ă����i�e�N���X���O���[�v�ł��CForm�ɃL�[�C�x���g����ԁI�j
        /// <summary>
        /// �����L�����̃_�C�X�R�}���h�ŃL�[���́@
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="_e"></param>
        private void listBox2_KeyDown(object sender, KeyEventArgs _e)
        {
            //this.FGameTestForm1_KeyDown(sender, _e);
        }
         * */





        

 
                   
    }
}