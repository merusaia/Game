using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// ユーザが入力した操作を，抽象的なボタン名などで表現する場合の、入力操作を抽象化した列挙型です．
    /// 
    /// ここに定義した種類だけ、ゲーム中のボタン（ヴァーチャルキー）があります。
    /// 
    ///     /// 現段階では、merusaiaが勝手に以下のような対応表を作っちゃってるので、変更したい人は、
    ///         EInputButton・入力ボタン  や   コンストラクタ で好きなように変更／追加して使ってください。
    /// 
    /// 
    /// 【対応表】以下、マウスやキーボードを入力処理を、ゲーム中のボタン表記に対応させた表
    ///      ＜ポリシー＞：キーの意味を考えたボタン名と、キーボードの矢印キーの周りで操作しやすいボタン配置を意識してみた。
    /// （[座標]とは、座標が特定される操作。マウスやスマホである場所を指定してから、操作することを示す。）
    /// 
    /// ●スマホ（草案）    マウスと          キーボード                  （ゲームパッド対応表）    【ボタン名】        ：戦闘中／シナリオ中での機能の対応表。　
    /// ＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝
    /// [座標]タップ        [座標]左クリック    ↑・↓・→・←  キー       （十字キー）             【十字キー】        ：移動？／移動
    ///
    /// タップ              左クリック          Enter/Z         キー       （○＝Ａボタン）         【決定ボタン】      ：攻撃／決定
    /// フリック←？        右クリック          BackSpace/X     キー       （×＝Ｂボタン）         【戻るボタン】    ：回避／キャンセル
    ///（２指押し・ダブルタッチ？）
    /// 画面押しっぱなし？  中クリック          Ctrl/C          キー       （□＝Ｙボタン）         【コントロールボタン】：防御／便利ボタン・整頓
    ///（フリック↓？）
    /// ダブルタップ？      中ダブルクリック    Shift/V         キー       （△＝Ｘボタン）         【シフトボタン】    ：技／メニュー・ジャンプ
    ///（フリック↑？）
    /// ロングタップ        左長押し            Enter長押し     キー       （○／Ａ長押し）         【決定ボタン長押し】：必殺技？／高速モード・メッセージスキップなど
    ///
    ///                 
    /// 左へスライド？      ホイール上？        Tab/A           キー       （Ｌ）                   【タブボタン】      ：リスト選択（行動・技・アイテムなど）／ページ移動
    /// 右へスライド？      ホイール下？        CapsLock/S      キー       （Ｒ）                   【キャプスロックボタン】：リスト選択（行動・技・アイテムなど）／ページ移動
    /// 上へスライド？      ホイールボタン？    1（ぬ）/3（あ） キー       （Ｌ２）                 【オプション１ボタン】：ショートカットキー／好きな行動・技・作戦などを割り当る
    /// 下へスライド？      ホイール連続押し？  2（ふ）/4（う） キー       （Ｒ２）                 【オプション２ボタン】：ショートカットキー／好きな行動・技・作戦などを割り当る
    ///
    /// メニューボタン      割り当てなし？      Space           キー       （START）                【スペースボタン】  ：   ポーズ（ヘルプ・操作方法など）
    /// メニュー長押し      割り当てなし？      Alt             キー       （SELCT）                【アルトボタン】    ：   オプション（設定方法など様々なカスタマイズ機能・テストプレイ時に多用）
    ///
    /// トリプルタップ      左トリプルクリック  Enterトリプル押 キー       （Ｒ３）                 【トリプルクリック】：超必殺技？・イライラ検知・ごめんなさいヘルプ・バランス調整など
    /// 
    /// // 参考：　スマホを操作する時の基礎用語 http://spgirl.jp/smartphone/2011100211-kiso_yougo
    /// </summary>
    public enum EInputButton・入力ボタン
    {
        /// <summary>
        /// =0。デフォルト値です。無入力、もしくはニュートラルな状態（十字キーもボタンも何も押していない状態）を示します。
        /// </summary>
        _none_無入力,

        // シナリオ進行中・移動中
        a1_右,
        a2_下,
        a3_左,
        a4_上,

        // 操作案1
        b1_決定ボタン_A,
        b2_戻るボタン_B,
        b3_コントロールボタン_Y,
        b4_シフトボタン_X,
        b5_タブボタン_L,
        b6_キャプスロックボタン_R,

        b7_オプション１ボタン_L2,
        b8_オプション２ボタン_R2,

        b9_スペースボタン_START,
        b10_アルトボタン_SELECT,

        // 操作案0
        //A_決定ボタン,
        //B_キャンセルボタン,
        //Y_便利ボタン,
        //X_メニューボタン,
        //L_スクロールボタン1,
        //R_スクロールボタン2,


        // 戦闘中
        //A_攻撃ボタン,
        //B_回避ボタン,
        //X_必殺技ボタン,
        //Y_技ボタン,
        //L_作戦ボタン1,
        //R_作戦ボタン2,

        // 戦闘防御中
        //A2_反撃ボタン,
        //B2_受け身ボタン,
        

    }

    /// <summary>
    /// ユーザが入力した複数の入力機器による入力操作（キー入力やクリック），
    /// 「決定ボタン」や「バックボタン」など，抽象的なボタンで表現可能にするクラスです。
    /// 
    /// EInputButton・入力ボタンenum列挙体に、それらのボタンの定義があり，必要に応じていつでも追加できます。
    /// </summary>
    public class CInputButton・ボタン入力定義
    {

        /// <summary>
        /// ボタン（仮想キー）管理者。EInput列挙体で定義したゲーム中のボタンが、それぞれなんのキーコードに対応しているかを格納し、それの押し判定を一括して管理するクラスです。
        /// </summary>
        public CVirtualKey・複数の入力をひとまとめにする仮想キー管理者 p_buttonManager・ボタン管理者 = new CVirtualKey・複数の入力をひとまとめにする仮想キー管理者();
        //CVirtualKeyクラスに依存したくない場合、こんな風に自前にする？public List<List<EKeyCode>> p_ButtonEqualKeyCodes・ボタンに対応するキー群 = new List<List<EKeyCode>>();

        /// <summary>
        /// ゲーム中のボタンに対応するキーを（追加されていなければ）追加します。
        /// キーボードキーに加えて、マウスボタンの処理もこのメソッドで登録できます。
        /// キーコンフィグなどで使います。
        /// </summary>
        /// <param name="_e・ボタン"></param>
        /// <param name="_keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード"></param>
        public void setP_ButtonKeys・指定ボタンに対応するキーを設定(EInputButton・入力ボタン _e・ボタン, EKeyCode _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード)
        {
            int _keycode = (int)_keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード;
            if (p_deviceNo_MouseAndKeyBoardKey != s_deviceNo_NONE・デバイス未登録)
            {
                int _deviceNo = p_deviceNo_MouseAndKeyBoardKey;
                p_buttonManager・ボタン管理者.AddVirtualKey((int)_e・ボタン, _deviceNo, _keycode);
            }
        }
        // ※このメソッドはWindows.Form依存になってしまうため、現在のところ実装していない。
        // もし実装する場合でも、できるならマウスボタンとキーボードキーを一緒に管理できる、setP_ButtonKeys・指定ボタンに対応するキーを設定(EInput, EKeyCode)を使ってください。
        ///// <summary>
        ///// マウスボタンを、ゲーム中のボタンに対応するキーを（追加されていなければ）追加します。
        ///// キーコンフィグなどで使います。
        ///// </summary>
        ///// <param name="_e・ボタン"></param>
        ///// <param name="_keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード"></param>
        //public void setP_ButtonKeys・指定ボタンに対応するマウスボタンを設定(EInputButton・入力ボタン _e・ボタン, EMouseButton _mousebutton)
        //{
        //    if (p_deviceNo_mouse != s_deviceNo_NONE・デバイス未登録)
        //    {
        //        int _deviceNo = p_deviceNo_mouse;
        //        p_buttonManager・ボタン管理者.AddVirtualKey((int)_e・ボタン, _deviceNo, (int)_mousebutton);
        //    }
        //}
        // いまさらジョイスティックなんているんだろうか…それよりはゲームパッドに対応した方が…（まだ未対応）。
        /// <summary>
        /// ゲーム中のボタンに対応するキーを（追加されていなければ）追加します。キーコンフィグなどで使います。
        /// </summary>
        /// <param name="_e・ボタン"></param>
        /// <param name="_keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード"></param>
        public void setP_ButtonKeys・指定ボタンに対応するジョイスティックボタンを設定(EInputButton・入力ボタン _e・ボタン, int _buttonNo)
        {
            if (p_deviceNo_joystick != s_deviceNo_NONE・デバイス未登録)
            {
                int _deviceNo = p_deviceNo_joystick;
                p_buttonManager・ボタン管理者.AddVirtualKey((int)_e・ボタン, _deviceNo, (int)_buttonNo);
            }
        }


        /// <summary>
        /// 登録しているデバイスの総数です。追加するたびに+1されます。
        /// </summary>
        public int p_deviceNum = 0;
        public static int s_deviceNo_NONE・デバイス未登録 = -1; // -1はデバイス未登録
        public int p_deviceNo_mouse = s_deviceNo_NONE・デバイス未登録; // マウスの登録デバイス番号。初期値はコンストラクタを参照。
        public int p_deviceNo_MouseAndKeyBoardKey = s_deviceNo_NONE・デバイス未登録;
        public int p_deviceNo_joystick = s_deviceNo_NONE・デバイス未登録;
        // public int p_deviceNo_smartphone; // 未実装

        /// <summary>
        /// コンストラクタです。
        /// 
        /// 引数1には必ず、定期的にUpdate()を呼びだす、CMouseAndKeyBoardKeyInputクラスのインスタンスを渡してください。
        /// 
        /// なお、その他のCMouseInputクラス、CJoyStickクラスなどの入力機器は、もし認識したければでいいので、インスタンスを引数に渡してください。なければnullでもＯＫです。
        /// 引数をnullにしたり、指定しないと、その入力は無効となります。
        /// 
        /// ■EInput列挙体で定義したゲーム中のボタンが、それぞれマウスやキーボードなどのキーを押した操作に対応しているかを変更したい場合、ここを変更してください。
        /// </summary>
        public CInputButton・ボタン入力定義(CMouseAndKeyBoardKeyInput・キー入力定義 _KeyInput, CMouseInput・マウス入力定義 _mouseInput, CJoyStick・ジョイスティック入力定義 _joystick)
        {
            // ゲーム中のボタン（ヴァーチャルキー）を、それぞれの入力デバイス（マウス・キーボードなど）で一括して管理できるよう、
            // 入力デバイスを登録します。
            // ●登録デバイス番号
            // 基本は、マウスキーボードキークラスが0
            // マウスが1
            if (_KeyInput != null)
            {
                p_deviceNo_MouseAndKeyBoardKey = p_deviceNum;
                p_deviceNum++;
                p_buttonManager・ボタン管理者.AddDevice(_KeyInput);
            }
            if (_mouseInput != null)
            {
                p_deviceNo_mouse = p_deviceNum;
                p_deviceNum++;
                p_buttonManager・ボタン管理者.AddDevice(_mouseInput);
            }
            // ジョイスティックが2
            if (_joystick != null)
            {
                p_deviceNo_joystick = p_deviceNum;
                p_deviceNum++;
                p_buttonManager・ボタン管理者.AddDevice(_joystick);
            }

            if (_KeyInput != null)
            {
                for (int i = 0; i < MyTools.getEnumIntMaxCount<EInputButton・入力ボタン>(); i++)
                {
                    // i番目の要素を取ってくる
                    EInputButton・入力ボタン _e = MyTools.getEnumItem_FromIndexOrValue<EInputButton・入力ボタン>(i);
                    // _eが取れてるか確認
                    //int a = 0; // 取れてる。ＯＫ。
                    
                    if (i == (int)(EInputButton・入力ボタン.a1_右))
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.RIGHT);
                    }
                    else if (i == (int)(EInputButton・入力ボタン.a2_下))
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.DOWN);
                    }
                    else if (i == (int)(EInputButton・入力ボタン.a3_左))
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.LEFT);
                    }
                    else if (i == (int)(EInputButton・入力ボタン.a4_上))
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.UP);
                    }
                    if (i == (int)(EInputButton・入力ボタン.b1_決定ボタン_A)) // 現在は、Ａボタン = 左クリック,Enter,z
                    {
                        // これはまずい？　だって、マウスの位置に限らず、決定キーをどこでも押したら確定になっちゃうから。選択肢などで注意が必要。スマホも一緒。
                        // 決定キーにそれほど重要性を持たせないなら問題ないけど、座標を指定して何かを確定する処理は、決定ボタンじゃない方がいいのかも。
                        //      現段階では、リスト決定とかは選択後に再度クリック　（選択したリストが未定義（-1）でなければ決定）にしてるけど、
                        //      決定キーで誤動作が多いようなら、対策を検討しよう。
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.MOUSE_LEFT);

                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.ENTER);//setP_ButtonKeys・指定ボタンに対応するキーを設定(_eエスカレーション指数, EKeyCode.ENTER); // Enterと一緒
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.z);
                    }
                    else if (i == (int)(EInputButton・入力ボタン.b2_戻るボタン_B))// 現在は、Ｂボタン = 右クリック,BackSpace,x
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.MOUSE_RIGHT); // （ただしVAIOノートＰＣタッチパッドだと、パッド左上タップは右クリックとして認識しないので注意）

                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.BACKSPACE);
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.x);
                    }
                    else if (i == (int)(EInputButton・入力ボタン.b3_コントロールボタン_Y)) // 現在は、Ｙボタン = 右ダブルクリック,Ctrl,c
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.MOUSE_RIGHT_DOUBLECLICK);

                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.LCTRL);
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.RCTRL);
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.c);
                    }
                    else if (i == (int)(EInputButton・入力ボタン.b4_シフトボタン_X)) // 現在は、Ｘボタン = 左ダブルクリック,Shift,v
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.MOUSE_LEFT_DOUBLECLICK);

                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.LSHIFT);
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.RSHIFT);
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.v);
                    }
                    // Start,Select
                    else if (i == (int)EInputButton・入力ボタン.b9_スペースボタン_START) // 現在は、Space、左トリプルクリック。SPACEボタンを決定ボタンＡにしない理由は、SPACEで操作方法ヘルプを表示したいから
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.MOUSE_LEFT_TRIPLECLICK);

                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.SPACE);
                    }
                    else if (i == (int)EInputButton・入力ボタン.b10_アルトボタン_SELECT) // 現在は、AL、右トリプルクリック、（まだ未対応）×左長押し。T
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.MOUSE_RIGHT_TRIPLECLICK);

                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.ALT);
                    }
                    // LR,L2R2
                    else if (i == (int)(EInputButton・入力ボタン.b5_タブボタン_L))
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.TAB);
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.a);
                    }
                    else if (i == (int)(EInputButton・入力ボタン.b6_キャプスロックボタン_R))
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.CAPSLOCK);
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.s);
                    }
                    else if (i == (int)EInputButton・入力ボタン.b7_オプション１ボタン_L2)
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.KEY1);
                    }
                    else if (i == (int)EInputButton・入力ボタン.b8_オプション２ボタン_R2)
                    {
                        setP_ButtonKeys・指定ボタンに対応するキーを設定(_e, EKeyCode.KEY2);
                    }
                }
            }
            


        }
        /// <summary>
        /// 指定したボタンが押されている状態かを返します。
        /// </summary>
        /// <param name="_eInput・入力ボタン"></param>
        /// <returns></returns>
        public bool isPressing・ボタンを押し中か(int _入力ボタン番号_EInput・入力操作のintキャスト型)
        {
            return p_buttonManager・ボタン管理者.IsPress(_入力ボタン番号_EInput・入力操作のintキャスト型);
        }
        /// <summary>
        /// 指定したボタンが押されている状態かを返します。
        /// </summary>
        /// <param name="_eInput・入力ボタン"></param>
        /// <returns></returns>
        public bool isPressing・ボタンを押し中か(EInputButton・入力ボタン _eInput・入力ボタン)
        {
            return p_buttonManager・ボタン管理者.IsPress((int)_eInput・入力ボタン);
        }
        /// <summary>
        /// 指定したボタンが押し離した瞬間（１フレームのみ）かを返します。
        /// </summary>
        /// <param name="_eInput・入力ボタン"></param>
        /// <returns></returns>
        public bool isPulled・ボタンを押し離した瞬間か(int _入力ボタン番号_EInput・入力操作のintキャスト型)
        {
            return p_buttonManager・ボタン管理者.IsPull(_入力ボタン番号_EInput・入力操作のintキャスト型);
        }
        /// <summary>
        /// 指定したボタンが押し離した瞬間（１フレームのみ）かを返します。
        /// </summary>
        /// <param name="_eInput・入力ボタン"></param>
        /// <returns></returns>
        public bool isPulled・ボタンを押し離した瞬間か(EInputButton・入力ボタン _eInput・入力ボタン)
        {
            return p_buttonManager・ボタン管理者.IsPull((int)_eInput・入力ボタン);
        }


        /// <summary>
        /// 特定のボタンを次の１フレームで自動入力（setPress()で一瞬だけ押す操作をエミュレート）します。
        /// 成功すればtrue、ユーザが既にキーを押していたり、デバイスなどが見つからなかったりで失敗すればfalseを返します。
        /// 具体的には、今回のフレームで押されて、次のフレームで離される（IsPull()==true）ように、
        /// 入力ボタンに対応している全てのEKeyCodeをエミュレートします。
        /// 
        /// ※なお、自動入力よりユーザの入力を優先させるため、
        /// ユーザが指定したキーを押していたら、エミュレートは失敗してfalseを返します。
        /// </summary>
        /// <param name="_eInput・入力ボタン"></param>
        /// <returns></returns>
        public bool autoInputButton・ボタン自動入力(EInputButton・入力ボタン _eInput・入力ボタン)
        {
            // 現段階の実装では、入力エミュレートメソッドのsetPressを実装しているクラスは１つしかないため、
            // とりあえずマウスキーボード入力機器で自動入力したことにしている。
            CMouseAndKeyBoardKeyInput・キー入力定義 _autoInputDevice
                = (CMouseAndKeyBoardKeyInput・キー入力定義)p_buttonManager・ボタン管理者.GetDevice(p_deviceNo_MouseAndKeyBoardKey);
            if (_autoInputDevice == null)
            {
                return false;
            }
            List<int> _intKeycodes = getEKeyCodeList_Int・入力ボタンに登録された対応キーコードをint型で取得(_eInput・入力ボタン);
            foreach (int _keycode in _intKeycodes)
            {
                if (_autoInputDevice.setPress(true, _keycode, true) == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// EInput列挙体に対応したキーを取得します。代表的なキーは引数[0]にされますが、複数キーが登録されている場合は、引数[0]、引数[1]、引数[2]、引数[3]...に格納されてます。
        /// </summary>
        /// <param name="_eInput・入力ボタン"></param>
        /// <returns></returns>
        public List<EKeyCode> getEKeyCodeList・入力ボタンに登録された対応キーコードを取得(EInputButton・入力ボタン _eInput・入力ボタン)
        {
            List<int> _intKeyCodeList = getEKeyCodeList_Int・入力ボタンに登録された対応キーコードをint型で取得(_eInput・入力ボタン);
            List<EKeyCode> _EKeyCodeList = new List<EKeyCode>();
            EKeyCode _ENumItem;
            foreach(int _keycode in _intKeyCodeList){
                // int型をEKeyCode型に変換して追加
                _ENumItem = MyTools.getEnumItem_FromIndexOrValue<EKeyCode>(_keycode);
                _EKeyCodeList.Add(_ENumItem);
            }
            return _EKeyCodeList;
        }
        /// <summary>
        /// EInput列挙体に対応したキーを取得します。代表的なキーは引数[0]にされますが、複数キーが登録されている場合は、引数[0]、引数[1]、引数[2]、引数[3]...に格納されてます。
        /// </summary>
        /// <param name="_eInput・入力ボタン"></param>
        /// <returns></returns>
        public List<int> getEKeyCodeList_Int・入力ボタンに登録された対応キーコードをint型で取得(EInputButton・入力ボタン _eInput・入力ボタン)
        {
            List<int> _intKeyCodeList = p_buttonManager・ボタン管理者.getKeyCodeList_ByVirtualKeyNo((int)_eInput・入力ボタン);
            return _intKeyCodeList;
        }
        //    List<EKeyCode> _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード = new List<EKeyCode>();
        //    EInputButton・入力ボタン _eエスカレーション指数 = _eInput・入力ボタン;

        //    if (_eエスカレーション指数 == EInputButton・入力ボタン.a1_右)
        //    {
        //        p_button・ボタン一覧.
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.RIGHT);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.a2_下)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.DOWN);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.a3_左)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.LEFT);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.a4_上)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.UP);
        //    }
        //    if (_eエスカレーション指数 == EInputButton・入力ボタン.b1_決定ボタン_A)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.ENTER);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.z);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b2_戻るボタン_B)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.BACKSPACE);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.x);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b3_コントロールボタン_Y)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.LCTRL);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.RCTRL);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.c);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b4_シフトボタン_X)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.LSHIFT);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.RSHIFT);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.v);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b5_タブボタン_L)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.TAB);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.a);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b6_キャプスロックボタン_R)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.CAPSLOCK);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.s);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b7_オプション１ボタン_L2)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.KEY1);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b8_オプション２ボタン_R2)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.KEY2);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b9_スペースボタン_START)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.SPACE);
        //    }
        //    else if (_eエスカレーション指数 == EInputButton・入力ボタン.b10_アルトボタン_SELECT)
        //    {
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.KeyCode308_LALT);
        //        _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード.Add(EKeyCode.RALT);
        //    }


        //    return _keycode・マウスボタンとキーボードキーを一括して扱えるEKeyCode型のキーコード;
        //}

        /// <summary>
        /// （※Windows専用）Window.Forms.Keysのキーに対応した、ゲームで認識可能なキーEKeyCodeを返します。
        /// 主要キーのみしか対応していません。
        /// また、ダブルクリックなどには対応していません。
        /// 
        /// 【ゲームで認識可能な主要キー】矢印キー, Enter, Backspace, Shift, Ctrl, Tab, CapsLock, Space, Alt, Delete, Esc, KEY1, KEY2, ..., KEY9, KEY0, a, b, ..., zまで (記号はとりあえず無し)
        /// </summary>
        public static EKeyCode getEKeyCode_FromWindowsFormsKeys(System.Windows.Forms.Keys _keys)
        {
            return CMouseAndKeyBoardKeyInput・キー入力定義.getEKeyCode_FromWindowsFormsKeys(_keys);
        }

        /// <summary>
        /// ゲームで認識可能なキーEKeyCodeに対応した、ゲーム中で使うボタンを返します。
        /// </summary>
        public EInputButton・入力ボタン getInputButton(EKeyCode _Ekeycode)
        {
            EInputButton・入力ボタン _input;// = EInputButton・入力ボタン._none_無入力;
            // KeysとEKeyCodeとの対応
            int _input_int = p_buttonManager・ボタン管理者.getVirtualKeyNo_ByKeyCode((int)_Ekeycode);
            // 対応するボタンが無かった場合は、勝手に無入力が入る
            _input = MyTools.getEnumItem_FromIndexOrValue<EInputButton・入力ボタン>(_input_int);
            return _input;
        }
        /// <summary>
        /// （※Windows専用）Window.Forms.Keysのキーに対応した、ゲーム中で使うボタンを返します。
        ///         /// 【ゲームで認識可能な主要キー】矢印キー, Enter, Backspace, Shift, Ctrl, Tab, CapsLock, Space, Alt, Delete, Esc, KEY1, KEY2, ..., KEY9, KEY0, a, b, ..., zまで (記号はとりあえず無し)
        /// </summary>
        public EInputButton・入力ボタン getInputButton(System.Windows.Forms.Keys _keys)
        {
            // KeysとEKeyCodeとの対応
            EKeyCode _keycode = getEKeyCode_FromWindowsFormsKeys(_keys);
            // Keysとボタンの対応
            EInputButton・入力ボタン _input = getInputButton(_keycode);
            return _input;
        }

    }

    
}
