using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /// <summary>
    /// 選択肢でよく使う例，サンプルです．識別ID＿選択文字列１＿選択文字列２＿．．．という名前を付ければ，「選択文字列１」「選択文字列２」の順番で羅列表示されます．
    /// </summary>
    public enum EChoiceSample・選択肢例
    {
        // 肯定／否定
        e1a＿はい,
        e1b＿次へ,
        e1c＿わかった,
        e1d＿OK,
        e1e＿了解,

        e2a＿はい＿いいえ,
        e2b＿そう思う＿そう思わない,
        e2c＿わかった＿ダメ,
        e2cc＿ＯＫ_ダメ,
        e2d＿A＿B,
        e2e＿いぃよ＿イヤ,
        e2f＿肯定する＿否定する,
        e2g＿議論を続ける＿拒否権を発動する,
        e2cc＿もちろん＿ちょっとまって,

        e3a＿はい＿いいえ＿どっちでも,
        e3b＿そう思う＿そう思わない＿どちらともいえない,
        e3c＿うん＿ううん＿わからない,
        e3d＿A＿B＿C,
        e3e＿助けに行く＿助けに行かない＿様子を見る,
        e3f＿肯定する＿否定する＿何も言わない,
        e2g＿肯定する＿否定する＿ふざけるな,
        e3h＿そりゃそうだ＿それは違う＿うーん,
        e3i＿そりゃそうだ＿それは違う＿ふざけるな,


        e4d＿A＿B＿C＿D,

        e5b＿かなりそう思う＿ある程度そう思う＿どちらともいえない＿あまりそう思わない＿全くそう思わない,
        e5c＿確かにその通りだ＿どちらかというと賛成だ＿どちらともいえない＿どちらかというと反対だ＿全く反対だ,
        e5d＿A＿B＿C＿D＿E,

        // 性別
        sex3＿男＿女＿不明,

        // 自動
        auto＿手動＿自動,

        // （伝説シナリオ）姿
        sugata3a＿堂々と＿不安そうに＿ボーっと,
        sugata1＿力強い＿心優しい＿たくましい＿頼もしい＿自由な＿おごそかな＿その他,
        karada1＿髪＿目＿鼻＿口＿手＿胸＿足＿その他,
        hikari1＿まばゆい光＿ひっそりした闇,
        sugata1＿白く光り輝いている＿鋭い目＿長く綺麗な髪＿華奢な体＿第三の目がある＿角が生えている＿羽が生えている＿しっぽが生えている＿その他,

        syukuteki3＿戦う＿話し合う＿捨て台詞を残して去る,
        syukutekisen1_すかさず応戦＿受け止める＿不意を突く,


        utyuuryokou3＿火星＿冥王星＿他の星に何か感じた,

        // データベースから取ってくる系
        charaAllゲストキャラ一覧,
        charaAllプレイヤーキャラ一覧,

        //
        charaCreate1_新しいキャラを作成する＿これまで創られたキャラをロードする＿やっぱりやめる,
        charaSelect1_プレイヤーキャラ＿ゲストキャラ,
        random_自分で選択＿ランダムで選択,

    }

    /// <summary>
    /// 一般的な選択肢の回答の例です．選択した文字列が，この項目の名前と全く同じときに使用します．ただし，ここに登録されていない回答の文字列も多く存在します．
    /// </summary>
    /// <remarks>（将来目指す実装）独自の選択肢の回答（やセリフ）についても，Google補完のように実行時・デザイン時に入力した過去のデータを履歴ファイルに溜めておき，インテリセンスやenumなどで参照できたら，スペル打ち間違いがなく最高である．</remarks>
    public enum EAnswerSample・回答例
    {
        /// <summary>
        /// ="_none・無回答"。まだ何も回答されていないことを意味する、無回答の時に格納する、デフォルトの回答文字列です。
        /// </summary>
        _none・無回答,
        /// <summary>
        /// ="cancel"。回答をキャンセルしたことを意味する、選択肢で戻るボタンを押したり、選択や入力をキャンセルしたりしたときに格納する回答文字列です。
        /// ※なお、なお、isYes("cancel")=falseなので、isNo("cancel")==true です。
        /// </summary>
        cancel,

        A,
        B,
        C,
        D,
        E,
        はい,
        次へ,
        わかった,
        ＯＫ,
        了解,
        うん,
        いいよ,

        ううん,
        どっちでも,
        いいえ,
        そう思う,
        そう思わない,
        ダメ,
        どちらともいえない,
        わからない,
        うーん,

        助けに行く,
        助けに行かない,
        様子を見る,
        肯定する,
        否定する,
        何も言わない,
        ふざけるな,
        そりゃそうだ,
        それは違う,

        かなりそう思う,
        ある程度そう思う,
        あまりそう思わない,
        全くそう思わない,
        確かにその通りだ,
        どちらかというと賛成だ,
        どちらかというと反対だ,
        全く反対だ,

    }


    /// <summary>
    /// 前回のgame.QC質問選択肢()メソッドで、ユーザが選択・入力した回答結果（選択肢や入力テキスト）を格納するクラスです．
    /// 
    /// 引数 (int _何回目の回答か)を指定することで、一度に複数の回答結果を格納する質問にも対応しています。
    /// 
    /// 使用例：　
    ///     game.QC質問選択肢("ユーザ名とパスワードを入力: ", 0≪制限時間無≫, "ユーザ名", "user1", "パスワード", "pass1");
    ///     string _userName = game.QA質問結果().k回答文字列(1); // ユーザ名
    ///     string _pass = game.QA質問結果().k回答文字列(2); // パスワード
    /// </summary>
    public class CAnswer・回答
    {        
        #region メモ
        // [MEMO]英語で作ったクラス内のプロパティ・メソッドを自由に使いたいなら，継承してもいい（※カプセル化が汚くなると思うので非推奨）
        //public class CAnswer・回答 : CAnswerManager{
        #endregion
        // (b)CQResult・質問結果クラスをCQResultManagerクラスのラッパークラスにするなら，クラスを持つという形が一般的でよい？
        /// <summary>
        /// 主に選択結果クラスの機能を書き込んである，ラッパークラスです．
        /// </summary>
        CAnswerManager p_usedClass;

        #region コンストラクタ
        /// <summary>
        /// 選択肢の選択結果の答えを1～複数個持つ，コンストラクタです．
        /// デフォルトの回答例は"_none・無回答"、回答番号は-1になります．
        /// </summary>
        public CAnswer・回答()
        {
            List<string> _param = new List<string>();
            _param.Add(EAnswerSample・回答例._none・無回答.ToString());
            List<int> _defaultSelecedNo = new List<int>();
            _defaultSelecedNo.Add(0);
            p_usedClass = new CAnswerManager(_param, _defaultSelecedNo);
        }
        /// <summary>
        /// 選択肢の選択結果の答えを一回の質問に1個持つ，コンストラクタです．
        /// 回答例と回答番号をそれぞれ与えてください．
        /// </summary>
        public CAnswer・回答(string _回答文字列, int _回答番号)
        {
            List<string> _param = new List<string>();
            _param.Add(_回答文字列);
            List<int> _defaultSelecedNo = new List<int>();
            // 無回答だったら、回答番号は0
            _defaultSelecedNo.Add(_回答番号);
            p_usedClass = new CAnswerManager(_param, _defaultSelecedNo);
        }
        /// <summary>
        /// 選択肢の選択結果の答えを一回の質問に1個～複数子持つ，コンストラクタです．
        /// 回答例を与えてください。回答番号は1,2,3...の連番になります。
        /// キャンセルの場合は、EAnswerSample・回答例.cancel を1個だけ与えてください。
        /// </summary>
        public CAnswer・回答(params EAnswerSample・回答例[] _デフォルト回答例を列挙)
        {
            List<string> _param = new List<string>();
            // string[] _param = new string[_デフォルト回答例を列挙.Length];
            List<int> _defaultSelecedNo = new List<int>();
            int i = 1;
            foreach (EAnswerSample・回答例 _item in _デフォルト回答例を列挙)
            {
                _param.Add(_item.ToString());
                // キャンセルだったら、回答番号は-1
                if (_item.ToString() == EAnswerSample・回答例.cancel.ToString())
                {
                    _defaultSelecedNo.Add(-1);
                }
                // 無回答だったら、回答番号は0
                else if (_item.ToString() == EAnswerSample・回答例._none・無回答.ToString())
                {
                    _defaultSelecedNo.Add(0);
                }
                // それ以外は連番
                else
                {
                    _defaultSelecedNo.Add(i);
                }
                i++;
            }
            p_usedClass = new CAnswerManager(_param, _defaultSelecedNo);
        }
        /// <summary>
        /// 選択肢の選択結果の答えを一回の質問に1個～複数子持つ，コンストラクタです．回答例と回答番号をそれぞれリストで与えてください．
        /// </summary>
        public CAnswer・回答(List<string> _回答文字列リスト, List<int> _回答番号リスト)
        {
            p_usedClass = new CAnswerManager(_回答文字列リスト, _回答番号リスト);
        }
        #endregion

        // CMainMessage・メインメッセージボックスのgetSelectやgetInputで，適時新しい入力結果を得ているので要らない．
        ///// <summary>
        ///// 指定した回答（回答番号と回答した文字列の両方）を変更します．
        ///// </summary>
        ///// <param name="_何番目の回答か"></param>
        ///// <param name="_answerNo・回答番号"></param>
        ///// <param name="_answer・回答した文字列"></param>
        //public void setA回答を変更(int _何番目の回答か, int _answerNo・回答番号, string _answer・回答した文字列)
        //{
        //    p_usedClass.setP_selectedString・選択文字列を変更(_何番目の回答か, _answer・回答した文字列);
        //    p_usedClass.setP_selectedNumber・回答番号を変更(_何番目の回答か, _answerNo・回答番号);
        //}
        /// <summary>
        /// 一度回答した回答（回答番号と回答した文字列の両方）を修正します．
        /// ※これは一度確定した選択肢を後になって、意図的に変更したい時だけ呼び出してください。
        /// 基本的に、回答番号や回答文字列は、コンストラクタを呼び出しているCMainMessage・メインメッセージボックスのgetSelectやgetInputで自動的に取得されています。
        /// </summary>
        /// <param name="_result・回答文字列"></param>
        public void setA回答を修正(int _answerIndex_0ToN・何番目の回答結果か_０が今回, int _answerNo・回答番号, string _answer・回答した文字列)
        {
            p_usedClass.setP_selectedString・回答文字列を変更(_answerIndex_0ToN・何番目の回答結果か_０が今回, _answer・回答した文字列);
            p_usedClass.setP_selectedNumber・回答番号を変更(_answerIndex_0ToN・何番目の回答結果か_０が今回, _answerNo・回答番号);
        }

        /// <summary>
        /// ●選択肢回答や入力を確定したかどうかを設定します．メッセージボックスなどで選択肢が確定された時にこのメソッドを呼び出してください．これがtrueで無い場合は，格納されている回答文字列や回答番号はあくまで「考え中」の暫定値です．
        /// </summary>
        /// <param name="_入力確定したか"></param>
        public void set確定(bool _入力確定したか)
        {
            p_usedClass.setIsEnter(_入力確定したか);
        }
        /// <summary>
        /// 入力結果が確定されたかを返します．これがtrueで無い場合は，格納されている回答文字列や回答番号はあくまで「考え中」の暫定値です．
        /// </summary>
        /// <returns></returns>
        public bool is確定()
        {
            return p_usedClass.isEnter();
        }

        /// <summary>
        /// 前回のメッセージで選択／入力した文字列を返します．
        /// </summary>
        /// <returns></returns>
        public string ks回答文字列()
        {
            return p_usedClass.getP_SelectedString・個別の回答文字列を取得(0);
        }
        /// <summary>
        /// 前回のメッセージで選択／入力した文字列を返します．
        /// ※回答文字列だけではタイプミスが考えられるため，回答番号も格納する．選択肢の結果をifやswitchで調べるときは，回答番号を使用することを推奨．ただし，回答番号だけじゃ誰かが読んでも意味がわからないので，選択肢毎の処理が離れている場合はできるだけ回答文字列を使用すること（もしくは回答番号を使って回答文字列をコメントで書くこと）を推奨．
        /// </summary>
        /// <returns></returns>
        public string ks回答文字列(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            return p_usedClass.getP_SelectedString・個別の回答文字列を取得(_answerIndex_0ToN・何番目の回答結果か_０が今回);
        }

        #region 回答番号の取得（回答文字列だけではタイプミスが考えられるため，回答番号も併せて格納する．）
        /// <summary>
        /// 前回のメッセージで回答番号（上から順番に1,2,3,...）を返します．_none・無回答（未選択／未入力／選択時間切れ）の場合は0，キャンセル（選択肢で戻るボタン／isNoの自動判定）の場合は-1を返します．
        /// ※回答文字列だけではタイプミスが考えられるため，回答番号も格納する．選択肢の結果をifやswitchで調べるときは，回答番号を使用することを推奨．ただし，回答番号だけじゃ誰かが読んでも意味がわからないので，選択肢毎の処理が離れている場合はできるだけ回答文字列を使用すること（もしくは回答番号を使って回答文字列をコメントで書くこと）を推奨．
        /// </summary>
        /// <returns></returns>
        public int k回答番号()
        {
            int _no = p_usedClass.getP_SelectedNo・回答番号を取得(0);
            return _no;
        }
        /// <summary>
        /// 前回のメッセージで回答番号（上から順番に1,2,3,...）を返します．_none・無回答（未選択／未入力／選択時間切れ）の場合は0，キャンセル（選択肢で戻るボタン／isNoの自動判定）の場合は-1を返します．
        /// ※回答文字列だけではタイプミスが考えられるため，回答番号も格納する．選択肢の結果をifやswitchで調べるときは，回答番号を使用することを推奨．ただし，回答番号だけじゃ誰かが読んでも意味がわからないので，選択肢毎の処理が離れている場合はできるだけ回答文字列を使用すること（もしくは回答番号を使って回答文字列をコメントで書くこと）を推奨．
        /// </summary>
        /// <returns></returns>
        public int kb前回の回答番号()
        {
            int _no = p_usedClass.getP_SelectedNo・回答番号を取得(1);
            return _no;
        }
        /// <summary>
        /// 前回のメッセージで回答番号（上から順番に1,2,3,...）を返します．_none・無回答（未選択／未入力／選択時間切れ）の場合は0，キャンセル（選択肢で戻るボタン／isNoの自動判定）の場合は-1を返します．
        /// ※回答文字列だけではタイプミスが考えられるため，回答番号も格納する．選択肢の結果をifやswitchで調べるときは，回答番号を使用することを推奨．ただし，回答番号だけじゃ誰かが読んでも意味がわからないので，選択肢毎の処理が離れている場合はできるだけ回答文字列を使用すること（もしくは回答番号を使って回答文字列をコメントで書くこと）を推奨．
        /// </summary>
        /// <returns></returns>
        public int k数回前の回答番号(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            int _no = p_usedClass.getP_SelectedNo・回答番号を取得(_answerIndex_0ToN・何番目の回答結果か_０が今回);
            return _no;
        }
        #endregion

        #region is回答、isはい、isいいえメソッド（できるだけ選択肢の文字列の入力ミスを防ぐため、「はい」や「いいえ」などの典型例は、isはい()やisいいえ()などのbool型で取得するメソッド）
        /// <summary>
        /// 回答が指定した文字列と等しい場合にtrueを返します．
        /// </summary>
        /// <returns></returns>
        public bool is回答(string _result・回答文字列)
        {
            bool _is = p_usedClass.isSelectedAnswerEquals(0, _result・回答文字列);
            return _is;
        }
        /// <summary>
        /// 回答が指定した回答例と等しい場合にtrueを返します．
        /// </summary>
        /// <returns></returns>
        public bool is回答(EAnswerSample・回答例 _EAnswerSample・回答例)
        {
            bool _is = p_usedClass.isSelectedAnswerEquals(0, _EAnswerSample・回答例.ToString());
            return _is;
        }
        /// <summary>
        /// 回答が指定した文字列と等しい場合にtrueを返します．
        /// </summary>
        public bool is回答(int _answerIndex_0ToN・何番目の回答結果か_０が今回, string _result・回答文字列)
        {
            bool _is =  p_usedClass.isSelectedAnswerEquals(_answerIndex_0ToN・何番目の回答結果か_０が今回, _result・回答文字列);
            return _is;
        }
        /// <summary>
        /// 回答が指定した回答例と等しい場合にtrueを返します．
        /// </summary>
        public bool is回答(int _answerIndex_0ToN・何番目の回答結果か_０が今回, EAnswerSample・回答例 _EAnswerSample・回答例)
        {
            bool _is =  p_usedClass.isSelectedAnswerEquals(_answerIndex_0ToN・何番目の回答結果か_０が今回, _EAnswerSample・回答例.ToString());
            return _is;
        }
        /// <summary>
        /// 回答が「Yes」に近い意味の回答例のどれかであった場合にtrueを返します．詳しくはMyTools.isYes()を参照してください。
        /// </summary>
        public bool isはい()
        {
            bool _is =  p_usedClass.isYes(0);
            return _is;
        }
        /// <summary>
        /// 回答が「Yes」に近い意味の回答例のどれかであった場合にtrueを返します．詳しくはMyTools.isYes()を参照してください。
        /// </summary>
        public bool isはい(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            bool _is =  p_usedClass.isYes(_answerIndex_0ToN・何番目の回答結果か_０が今回);
            return _is;
        }
        /// <summary>
        /// 回答が「Yes」の意味を持つ回答【以外】であった場合にtrueを返します．
        /// ※つまり、isYes()の裏返しです。
        /// なお、"cancel"などもtrueになるので、キャンセル処理を設ける場合は、この処理の条件分岐よりキャンセル処理を先に書いてください。
        /// 　　　詳しくはMyTools.isYes()の候補語（値がfalseになる）を参照してください。
        /// </summary>
        public bool isいいえ()
        {
            bool _is =  p_usedClass.isNo(0);
            return _is;
        }
        /// <summary>
        /// 回答が「Yes」の意味を持つ回答【以外】であった場合にtrueを返します．
        /// ※つまり、isYes()の裏返しです。
        /// なお、"cancel"などもtrueになるので、キャンセル処理を設ける場合は、この処理の条件分岐よりキャンセル処理を先に書いてください。
        /// 　　　詳しくはMyTools.isYes()の候補語（値がfalseになる）を参照してください。
        /// </summary>
        public bool isいいえ(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            bool _is =  p_usedClass.isNo(_answerIndex_0ToN・何番目の回答結果か_０が今回);
            return _is;
        }
        /// <summary>
        /// 回答をキャンセルした動作（戻るボタンを押した、あるいはそれと同等の意味を持つ選択肢を回答した）場合にtrueを返します．
        /// ※具体的には、回答番号が-1か、回答文字列が"Cancel","CANCEL","キャンセル"のどれかならtrueを返します。
        /// 
        /// 　　なお、キャンセル処理を設ける場合は、いいえ処理の条件分岐よりこのキャンセル処理を先に書いてください。
        /// </summary>
        public bool isキャンセル()
        {
            bool _is =  p_usedClass.isCancel(0);
            return _is;
        }
        /// <summary>
        /// 回答をキャンセルした動作（戻るボタンを押した、あるいはそれと同等の意味を持つ選択肢を回答した）場合にtrueを返します．
        /// ※具体的には、回答番号が-1か、回答文字列が"Cancel","CANCEL","キャンセル"のどれかならtrueを返します。
        /// 
        /// 　　なお、キャンセル処理を設ける場合は、いいえ処理の条件分岐よりこのキャンセル処理を先に書いてください。
        /// </summary>
        public bool isキャンセル(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            bool _is =  p_usedClass.isCancel(_answerIndex_0ToN・何番目の回答結果か_０が今回);
            return _is;
        }
        #endregion

    }

    // 以下は、機能を実現するために、CQResult・質問結果クラスが内部で利用しているクラス。
    // 「game.」を使う場合、基本直接触ることはない。
    #region 質問結果の取得・設定・更新を管理するクラス: CAnswerManager
    /// <summary>
    /// game.QC質問選択肢()メソッドの回答結果（選択肢や入力テキスト）を格納するクラスです．
    /// 
    /// game.is回答()などのメソッドで適時呼び出したい場合には、これに機能を付け加えたラッパークラス「CQResult・入力結果」を使ってください。
    /// </summary>
    public class CAnswerManager
    {
        /// <summary>
        /// 選択した回答の文字列
        /// </summary>
        List<string> p_selectedString・回答文字列 = new List<string>();

        /// <summary>
        /// ="_none・無回答"。まだ何も回答されていないことを意味する、無回答の時に格納する、デフォルトの回答文字列です。
        /// </summary>
        public static string s_none・無回答 = EAnswerSample・回答例._none・無回答.ToString();

        /// <summary>
        /// コンストラクタです．格納する回答文字列と回答番号をそれぞれリストで与えてください．１回の質問で複数の回答文字列（回答番号）を持てるように、リストにしています。
        /// 入力質問など，回答番号リストが無い場合はnullを入れてください．nullの場合、回答番号には0が入ります。
        /// </summary>
        public CAnswerManager(List<string> _selectedStringList, List<int> _selectedNoList)
        {
            if (_selectedStringList != null)
            {
                // 回答文字列を格納
                foreach (string _string in _selectedStringList)
                {
                    p_selectedString・回答文字列.Add(_string);
                }
            }
            else
            {
                // 回答文字列リストがnullの時は、"s_none・無回答"が入る
                p_selectedString・回答文字列.Add(s_none・無回答);
            }
            // 回答番号を格納
            if (_selectedNoList != null)
            {
                foreach (int _no in _selectedNoList)
                {
                    p_selectedNo・回答番号.Add(_no);
                }
            }
            else
            {
                // 入力質問など，回答番号リストがnullの場合は、回答番号には0が入る。
                p_selectedNo・回答番号.Add(0);
            }
        }


        public string getP_SelectedString・個別の回答文字列を取得(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            string _string = "";
            if (isNotOverIndex(_answerIndex_0ToN・何番目の回答結果か_０が今回) == true)
            {
                _string = p_selectedString・回答文字列[_answerIndex_0ToN・何番目の回答結果か_０が今回];
            }
            else
            {
                _string = EAnswerSample・回答例._none・無回答.ToString();
            }
            return _string;
        }
        public List<string> getP_SelectedString・回答文字列を取得()
        {
            return p_selectedString・回答文字列;
        }
        public void setP_selectedString・回答文字列を変更(int _answerIndex_0ToN・何番目の回答結果か_０が今回, string _selectedString)
        {
            // ●ここだけが回答文字列の変更点！
            // 改行を消去？
            //_selectedString = _selectedString.Replace("\r\n", "");
            // 「：」「:」の後ろを消去（キャラ名の時など、「キャラ名：称号やパラメータ」の称号やパラメータを消去」
            if (_selectedString.Contains(":") == true)
            {
                _selectedString = MyTools.getStringItem(_selectedString, ":", 1);
            }
            if (_selectedString.Contains("：") == true)
            {
                _selectedString = MyTools.getStringItem(_selectedString, "：", 1);
            }

            if (isNotOverIndex(_answerIndex_0ToN・何番目の回答結果か_０が今回)==true)
            {
                p_selectedString・回答文字列[_answerIndex_0ToN・何番目の回答結果か_０が今回] = _selectedString;
            }
        }
        /// <summary>
        /// リストの参照する配列数がオーバーしていないかを調べます
        /// </summary>
        /// <param name="_Resultindex"></param>
        /// <returns></returns>
        private bool isNotOverIndex(int _Resultindex)
        {
            bool _isNotOverIndex = (_Resultindex <= p_selectedString・回答文字列.Count - 1);
            return _isNotOverIndex;
        }

        
        /// <summary>
        /// 選択肢の中から回答した番号（1,2,3,...の順で，0は無回答）を格納します．配列は，[0]が一般的に使われる一番目の回答結果です．[1]以降は複数の選択肢を回答可能な時に格納されます．
        /// </summary>
        List<int> p_selectedNo・回答番号 = new List<int>();
        public int getP_SelectedNo・回答番号を取得(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            if (_answerIndex_0ToN・何番目の回答結果か_０が今回 <= p_selectedNo・回答番号.Count - 1)
            {
                return p_selectedNo・回答番号[_answerIndex_0ToN・何番目の回答結果か_０が今回];
            }
            else
            {
                return 0;
            }
        }
        public void setP_selectedNumber・回答番号を変更(int _answerIndex_0ToN・何番目の回答結果か_０が今回, int _answerNo・回答番号)
        {
            if (_answerIndex_0ToN・何番目の回答結果か_０が今回 <= p_selectedNo・回答番号.Count - 1)
            {
                p_selectedNo・回答番号[_answerIndex_0ToN・何番目の回答結果か_０が今回] = _answerNo・回答番号;
            }
        }

        // [Memo][Tips]テンプレート使用時の選択結果を調べるとき，プログラム上で自動的にそのテンプレート内の項目だけにすることを考えたが，enumをプログラム上で追加・削除できる技量がなかったため一旦明らめた．
        /// <summary>
        /// 選択した選択肢例のテンプレート．nullでなければ，この中の項目だけを参照します．テンプレートを使用していなければnullです．
        /// </summary>
        //test_選択肢例＿草案 p_usedSelectedTemplate = null;


        public bool p_isEnter = false;
        /// <summary>
        /// ●入力結果が確定しているかどうかを返します．これがtrueでないと，回答は先に進みません．
        /// </summary>
        /// <returns></returns>
        public bool isEnter()
        {
            return p_isEnter;
        }
        public void setIsEnter(bool _入力を確定したか)
        {
            p_isEnter = _入力を確定したか;
        }

        /// <summary>
        /// 回答が「Yes」の意味を持つ回答であった場合にtrueを返します．詳しくはMyTools.isYes()を参照してください。
        /// </summary>
        /// <returns></returns>
        public bool isYes(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            if (isNotOverIndex(_answerIndex_0ToN・何番目の回答結果か_０が今回) == true)
            {
                string _result = p_selectedString・回答文字列[_answerIndex_0ToN・何番目の回答結果か_０が今回];
                return MyTools.isYes(_result);
            }
            // 配列数がオーバーしてたらfalse（※isNo()はtrueになる）
            return false;
        }
        /// <summary>
        /// 回答が「Yes」の意味を持つ回答【以外】であった場合にtrueを返します．
        /// ※つまり、isYes()の裏返しです。
        /// なお、"cancel"などもtrueになるので、キャンセル処理を設ける場合は、この処理の条件分岐よりキャンセル処理を先に書いてください。
        /// 　　　詳しくはMyTools.isYes()の候補語（値がfalseになる）を参照してください。
        /// </summary>
        public bool isNo(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            return !isYes(_answerIndex_0ToN・何番目の回答結果か_０が今回); // isYesの裏返し
        }
        /// <summary>
        /// 回答がキャンセルされた、またはそれと同等の意味の回答例のどれかであった場合にtrueを返します．
        /// ※具体的には、回答番号が-1か、回答文字列が"Cancel","CANCEL","キャンセル"のどれかならtrueを返します。
        /// 
        /// 　　なお、キャンセル処理を設ける場合は、いいえ処理の条件分岐よりこのキャンセル処理を先に書いてください。
        /// </summary>
        public bool isCancel(int _answerIndex_0ToN・何番目の回答結果か_０が今回)
        {
            if (isNotOverIndex(_answerIndex_0ToN・何番目の回答結果か_０が今回) == true)
            {
                if (p_selectedNo・回答番号[_answerIndex_0ToN・何番目の回答結果か_０が今回] == -1)
                {
                    return true;
                }
                else
                {
                    string _result = p_selectedString・回答文字列[_answerIndex_0ToN・何番目の回答結果か_０が今回];
                    if (_result == s_Cancel・キャンセルを示す回答文字列 || _result == "Cancel" || _result == "CANCEL"
                        || _result == "キャンセル")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            // 配列数がオーバーしてたらtrue（回答が見つからなければ、キャンセル扱い）
            return true;
        }
        /// <summary>
        /// ="cancel"。回答がキャンセルされた時に格納する回答文字列です。
        /// なお、isYes("cancel")=falseなので、isNo("cancel")=true です。
        /// 
        /// 　        /// ※回答にキャンセルをセットするメソッドは、このクラスにはありません。詳しくはgetChancelAnswer・キャンセルを意味する回答結果を取得()メソッドを参照してください。

        /// </summary>
        public static string s_Cancel・キャンセルを示す回答文字列 = "cancel"; //EAnswerSample・回答例.cancel.ToString();

        /// <summary>
        /// 回答が指定した文字列であればtrueを返します．
        /// </summary>
        /// <param name="_selectedString"></param>
        /// <returns></returns>
        public bool isSelectedAnswerEquals(int _ResultString, string _selectedString)
        {
            if (isNotOverIndex(_ResultString) == true)
            {
                if (p_selectedString・回答文字列[_ResultString] == _selectedString)
                {
                    return true;
                }
            }
            return false;
        }
    }
    #endregion

}
