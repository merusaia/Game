using System;
using System.Collections.Generic;
using System.Text;
using Yanesdk.Sound;
using Yanesdk.System;
using Yanesdk.Ytl;

namespace PublicDomain
{
    // ※YaneSDKのソースコードを含みます．やねうらお様に深く感謝いたします．

    /// <summary>
    /// サウンドを曲名・効果音名から読み込むクラスです．サウンド再生は，このクラスを直接使うよりも，CSoundManagerサウンド管理者を使う方が高機能です．
    /// </summary>
    public class CAudioLoader_FromName・オーディオデータ読み込み機 : CCachedObjectLoader_ByString
    {
        #region ●使用例メモ
        /// <summary>
        /// CSoundPlayData・オーディオ再生用クラス の一括読み込みクラス。
        /// 
        ///	CSoundPlayData・オーディオ再生用クラス を cache したり、自動解放したりいろいろします。
        ///	詳しいことは、親クラスを見てください。
        ///
        ///	int	loadDefFile(char[] _fileName_NotFullPath_ファイル名_名前だけ);
        ///	で読み込むデータベースファイルはテキストファイルで、
        /// ==========================================================================================
        /// ID,  名前,	               ファイル名,	            再生tチャンク,  ディレクトリ名
        ///	1,	 Shoot the thought,    Shoot the thought.wav,   -1,             
        /// 2,   タラリラーン,         タラリラーン.mp3,        1,               
        ///	3,	 バシュッ,             バシュッ.wav,            0,              効果音\\
        /// ==========================================================================================
        ///	のように書く．
        /// 
        /// 以下，各列の説明．
        /// ・ID:               （とりあえず管理用．普通は名前で検索するので使用しないかも）
        /// ・名前：            （実際の検索や表示に使用する名前、参照名）
        /// ・ファイル名：      （実際のファイルの名前）
        ///	・再生チャンネル    （-1:ループするBGM用, 1:ループしないBGM用，2:VOICE用，省略:SE用）
        /// ・ディレクトリ名　　（データベースからファイル名に行くまでのディレクトリパス（あれば））
        ///	
        /// 再生チャンクは省略可能で，省略した場合場合 0 を意味し、
        /// その時点で再生していないチャンクを用いて再生する。
        /// 
        /// ※再生チャンクに「-1」を指定したサウンド（BGMなど）はループ再生される．
        /// ※「1」を指定したサウンドは，現在再生中のBGMを一時ストップさせて割り込み再生し，
        /// 　 終了後に元のBGMをフェードインしながら再開させる（レベルアップ効果音など）．
        /// ※「2」を指定したVOICEは，優先的にチャンク2-5の中から空きを使用して再生する．
        /// ※省略したSEは，優先的にチャンク6-8から空きを使用して再生する．
        /// 　（VOICE単独でボリューム変更可能なVOICE連続再生は4人，SEは3つまで，
        /// 　　ただしSE用のボリュームでよければ最大8つ同時再生が可能，SEも同じ）
        /// 　 なお，現段階ではSEと同様にどんなVOICEでも連続再生可能．
        /// 　（同じ人のボイスだけ連続再生不可にするなどを検討中）
        /// 
        /// </summary>
        /// <remarks>
        /// cacheするサウンドファイルのリソース上限を設定。
        /// (defaultでは64MB
        /// oggファイルであれ、内部では非圧縮の状態にして再生するので、
        /// そこでのサイズがリソースの値となる。3分程度のoggであれ、
        /// 44KHzで再生するならば40MB程度食うことは覚悟したほうがいいだろう。
        /// 
        /// そのへんを考慮に入れて適切な値に設定すべきである。
        /// </remarks>
        #endregion
        
        // サウンドデータベース読み込み用
        private static string p_soundDatabaseFileName = Program・実行ファイル管理者.p_SoundDatabaseFileName_FullPath・サウンドデータベースファイルパス;
        private static int p_indexMax = 4;
        private static int p_index_soundChannelNo・チャンネル番号のオプション配列インデックス = 3;
        private static int p_index_soundFileNameFullPath・フルパスのオプション配列インデックス = 4;
        private static int p_index_secondFileNameStart・２つ目のファイル名の開始オプション配列インデックス = 5;
        private static int p_index_secondFileNameFullPath・２つ目のファイル名のオプション配列インデックス = 6;
        private static int p_index_otherFileNameStart_5・２つ目以上のファイル名の開始オプション配列インデックス = 5;
        private static int p_index_otherFileNameFullPath_AddIndex_1・２つ目以上のファイル名フルパスのオプション配列インデックス = 1;
        private static int p_index_NextFileName_AddIndex_2・次のファイル名へ進む時のオプション配列インデックス = 2;

        #region ■データベース読み込み
        public void loadDatabase・データベース初期化処理()
        {
            IsDefRelativePath = false; // 絶対パス
            // [MEMO]このLoadDefFileでは， CDatabaseFileReader_ReadByString・データベース読み込み機を包含し，
            //  　　 データースの読み込み，更にオブジェクトのファイル名を格納している．
            LoadDefFile・データベースcsvファイルの読み込みとオブジェクトファイル名群読み込み(p_soundDatabaseFileName);
        }

        // 以下はキャラクタデータベースのやり方．サウンドはちょっと違うことをしている．要整理
        /*
        private static CDatabaseFileReader_ReadByString・データベース読み込み機 p_guestReader = getDataBaseReader・データベース読み込み機の取得();
        /// <summary>
        /// サウンドデータベースを読み込みます．ほんの少し重い処理なので，一回だけ呼び出してください．
        /// </summary>
        /// <param name="_filenname"></param>
        /// <returns></returns>
        private static CDatabaseFileReader_ReadByString・データベース読み込み機 getDataBaseReader・データベース読み込み機の取得()
        {

            CDatabaseFileReader_ReadByString・データベース読み込み機 _reader = new CDatabaseFileReader_ReadByString・データベース読み込み機();
            if (_reader.LoadDefFile・データベースcsvファイルの読み込み(p_soundDatabaseFileName) != Yanesdk.Ytl.YanesdkResult.NoError)
            {
                Program.printlnLog(ELogType.l5_エラーダイアログ表示, "サウンドデータベースが見つかりません．パス：　" + p_soundDatabaseFileName);
            }
            return _reader;
        }
         */


        /*
        /// <summary>
        /// サウンドの名前とファイル場所を格納している，テキストファイルのデータベースです．
        /// </summary>
        List<string> p_soundDataBase・サウンドデータベース = new List<string>();
        /// <summary>
        /// サウンドファイルを記憶する，辞書です．
        /// </summary>
        Dictionary<EBGM・曲, CSoundPlayData・オーディオ再生用クラス> p_soundDictionary・サウンド辞書 = new Dictionary<EBGM・曲, CSoundPlayData・オーディオ再生用クラス>();

        public readonly string p_SoundDirectory・サウンドデータディレクトリパス暫定版 = "C:\\Users\\Merusaia\\Music\\HIDE_mp3\\データベース";
        // 製品版のパス: public readonly string p_SoundDirectory・サウンドディレクトリパス = Program.ROOTDIRECTORY・実行ファイルがあるフォルダパス+"\\データベース\\サウンドデータベース.csv";
         * */
        #endregion

        #region ctor・コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CAudioLoader_FromName・オーディオデータ読み込み機()
            :base()
        {
            loadDatabase・データベース初期化処理();
        }
        #endregion

        #region methods・メソッド

        #region Soundオブジェクト上書き（merusaiaが追加）
        public void setSound・サウンドを上書き(string _name・参照名, object _newObject)
        {
            SetCachedObjectHelper・キャッシュオブジェクトの上書き(_name・参照名, _newObject as ICachedObject);
        }
        #endregion

        #region Soundオブジェクト取得
        /// <summary>
        ///	指定されたファイル名のオブジェクトを構築して返す
        /// </summary>
        /// 暗黙のうちにファイル名は構築され、loadされる。
        ///	定義ファイル上に存在しないファイル名を指定した場合はnullが返る
        ///	ファイルの読み込みに失敗した場合は、nullは返らない。
        ///	定義ファイル上に存在しないファイル名のものを再生することは
        ///	考慮しなくてもいいと思われる．．。
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <returns></returns>
        public CSoundPlayData・オーディオ再生用クラス getSound・オーディオ再生用クラスを取得(string _name・参照名)
        {
            return GetCachedObjectHelper・キャッシュオブジェクトの取得(_name・参照名) as CSoundPlayData・オーディオ再生用クラス;
        }

        /// <summary>
        /// SoundHelperメソッドに渡す用のdelegate
        /// </summary>
        /// <param name="sound"></param>
        /// <returns></returns>
        public delegate YanesdkResult SoundCallbackDelegate(CSoundPlayData・オーディオ再生用クラス sound);

        /// <summary>
        /// サウンド再生で用いると便利かも知れないヘルパメソッド。
        /// ほとんど全てのサウンド再生・停止・編集メソッドがこれを呼び出しているので、共通した処理をここに書ける。
        /// 
        /// 詳しいことは、この実装と、このメソッドの呼び出し元を見て。
        /// </summary>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <param name="dg"></param>
        /// <returns></returns>
        public YanesdkResult SoundHelper(string _name・参照名, SoundCallbackDelegate dg)
        {
            if (!EnableSound・BGMとSEとVOICE再生が全てONかどうか)
                return YanesdkResult.NoError; // そのまま帰ればok

            CSoundPlayData・オーディオ再生用クラス s = getSound・オーディオ再生用クラスを取得(_name・参照名);
            if (s == null)
                return YanesdkResult.PreconditionError; // 存在しない
            else
                return dg(s); // これは後処理，再生しなくても呼び出される。
                              // このデリゲートの中身は、SoundHelperを呼び出すメソッドによって違う。再生かもしれないし、停止かもしれない
        }
        #endregion

        #region 通常の再生関連
        /// <summary>
        ///	直接指定されたサウンド名のサウンドを再生させる
        /// </summary>
        /// <remarks>
        /// 失敗時はYanesdkResult.no_error以外が返る。
        ///	(定義ファイル上に存在しない番号を指定した場合や
        ///	ファイルの読み込みに失敗した場合もYanesdkResult.no_error以外が返る)
        /// </remarks>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <returns></returns>
        public YanesdkResult Play(string _name・参照名)
        {
            return SoundHelper(_name・参照名, delegate(CSoundPlayData・オーディオ再生用クラス s) { return s.Play(); });
        }

        /// <summary>
        /// FadeInつきのplay。FadeInのスピードは[ms]単位。
        /// </summary>
        /// <remarks>
        /// 失敗時はYanesdkResult.no_error以外が返る。
        ///	(定義ファイル上に存在しない番号を指定した場合や
        ///	ファイルの読み込みに失敗した場合もYanesdkResult.no_error以外が返る)
        /// </remarks>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public YanesdkResult PlayFade(string _name・参照名, int speed)
        {
            return SoundHelper(_name・参照名, delegate(CSoundPlayData・オーディオ再生用クラス s) { return s.PlayFade(speed); });
        }

        /// <summary>
        /// 直接指定された番号のサウンドを停止させる
        /// </summary>
        /// <remarks>
        /// 失敗時はYanesdkResult.no_error以外が返る。
        ///	(定義ファイル上に存在しない番号を指定した場合や
        ///	ファイルの読み込みに失敗した場合もYanesdkResult.no_error以外が返る)
        /// </remarks>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <returns></returns>
        public YanesdkResult Stop(string _name・参照名)
        {
            return SoundHelper(_name・参照名, delegate(CSoundPlayData・オーディオ再生用クラス s) { return s.Stop(); });
        }

        /// <summary>
        ///	FadeOutつきのstop。FadeOutのスピードは[ms]単位。
        /// </summary>
        /// <remarks>
        /// 失敗時はYanesdkResult.no_error以外が返る。
        ///	(定義ファイル上に存在しない番号を指定した場合や
        ///	ファイルの読み込みに失敗した場合もYanesdkResult.no_error以外が返る)
        /// </remarks>
        public YanesdkResult StopFade(string _name・参照名, int speed)
        {
            return SoundHelper(_name・参照名, delegate(CSoundPlayData・オーディオ再生用クラス s) { return s.StopFade(speed); });
        }
        #endregion

        #region SE再生関連

        /// <summary>
        ///	SE再生のguard時間（一定のフレーム数が経過するまでは、
        ///	同じSEの再生はplaySEを呼び出しても行なわなくするフレーム数）．デフォルトは5フレームです．
        /// </summary>
        /// <remarks>
        ///	ガードタイム。多過ぎて固まるような連続SE再生を防止するため，
        /// SE再生後、一定のフレーム数が経過するまでは、
        ///	そのSEの再生はplaySEを呼び出しても行なわない。
        ///	(ディフォルトでは5フレーム。)
        ///	UpdateSE()を呼び出したときに1フレーム経過したものとする。
        /// </remarks>
        public int GuardTime
        {
            get { return guardTime_; }
            set { guardTime_ = value; }
        }
        private int guardTime_ = 5;

        /// <summary>
        /// SE再生で1フレームが経過したことを記録するメソッド。メインのスレッドで，必ず毎フレームにつき1回呼び出すこと！
        /// </summary>
        /// <remarks>
        ///	SE再生後、一定のフレーム数が経過するまでは、
        ///	そのSEの再生はplaySEを呼び出しても行なわない。
        ///	これをguard timeと呼ぶ。これは、 SetGuardTime で設定する。
        ///	また、毎フレーム updateSE を呼び出す。
        ///	(これを呼び出さないとフレームが経過したとみなされない)
        /// </remarks>
        public void UpdateSE()
        {
            string[] keys = new string[guardTimes.Keys.Count];
            guardTimes.Keys.CopyTo(keys, 0);
            foreach (string key in keys)
            {
                int rest = guardTimes[key];
                if (0 < rest)
                    guardTimes[key] = rest - 1;
            }
        }

        /// <summary>
        /// SE再生用のplay関数。
        /// </summary>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <returns></returns>
        /// <remarks>
        /// <Para>
        /// SEの再生は例えばシューティングで発弾時に行なわれたりして、
        /// 1フレーム間に何十回と同じSE再生を呼び出してしまうことがある。
        /// そのときに、毎回SEの再生をサウンドミキサに要求していたのでは
        /// 確実にコマ落ちしてしまう。そこで、一定時間（gurdTimeと呼ぶ）は
        /// 同じ番号のSE再生要求は無視する必要がある。それが、playSEの機能である。
        /// </Para>
        /// <Para>
        /// playSEで再生させたものをstopさせるときは
        /// stop/stopFade関数を用いれば良い。
        /// </Para>
        /// <Para>
        /// この関数を用いる場合は、 updateSE を毎フレーム呼び出さないと
        /// いけないことに注意。
        /// </Para>
        /// </remarks>
        public YanesdkResult PlaySE(string _name・参照名)
        {
            return SoundHelper(_name・参照名, delegate(CSoundPlayData・オーディオ再生用クラス s)
            {
                if (IsInGuardTime(_name・参照名))
                    return YanesdkResult.NoError;

                return s.Play();
            });
        }

        /// <summary>
        /// 	FadeInつきのplaySE。FadeInのスピードは[ms]単位。
        /// 
        /// 　　これもguard time中だと利かないので注意すること。
        /// </summary>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public YanesdkResult PlaySEFade(string _name・参照名, int speed)
        {
            return SoundHelper(_name・参照名, delegate(CSoundPlayData・オーディオ再生用クラス s)
            {
                if (IsInGuardTime(_name・参照名))
                    return YanesdkResult.NoError;

                return s.PlayFade(speed);
            });
        }

        /// <summary>
        ///	guardタイムのリセット
        /// </summary>
        /// <remarks>
        /// 連続してクリックしたときにクリック音を鳴らさないと
        ///	いけないことがある。SE連続再生を禁止したくないシーンにおいて、
        ///	このメソッドを毎フレームごとor1回呼び出すことにより、
        ///	すべてのサウンドのguardtimeをリセットすることができる
        /// </remarks>
        public void ResetGuardTime()
        {
            foreach (string key in guardTimes.Keys)
                guardTimes[key] = 0;
        }

        /// <summary>
        /// ガードタイムのチェック用
        /// </summary>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <returns></returns>
        private bool IsInGuardTime(string _name・参照名)
        {
            if (!guardTimes.ContainsKey(_name・参照名))
                guardTimes.Add(_name・参照名, 0);

            int guardTime = guardTimes[_name・参照名];

            // ガード時間中なので再生できナーだった(エラー扱いではない)
            if (0 < guardTime) return true;

            //　ガード時間の更新
            guardTimes[_name・参照名] = this.guardTime_;
            return false;
        }

        /// <summary>
        /// それぞれのサウンド名を一度に連続再生してコマ落ちしないために管理するガードタイム
        /// </summary>
        private Dictionary<string, int> guardTimes = new Dictionary<string, int>();

        #endregion

        #region BGM再生関連

        /// <summary>
        ///	BGMの再生用play関数．再生中のBGMは現在のBGM1つだけ保存しています．
        /// </summary>
        /// <remarks>
        ///	メニュー等で、サウンド再生とSEの再生のon/offを切り替えたとする。
        ///	この切り替え自体はenableSound関数を使えば簡単に実現できるが、
        ///	そのときに、(SEはともかく)BGMは鳴らないとおかしい、と言われることが
        ///	ある。そのときにBGMを鳴らすためには、再生要求のあったBGMに関しては
        ///	保持しておき、それを再生してやる必要がある(と考えられる)
        ///
        ///	よって、BGM再生に特化した関数が必要なのである。
        /// </remarks>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <returns></returns>
        public YanesdkResult PlayBGM(string _name・参照名)
        {
            bgmName・曲名 = _name・参照名;
            return Play(_name・参照名);
        }

        /// <summary>
        /// FadeInつきの playBGM 。FadeInのスピードは[ms]単位。
        /// </summary>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public YanesdkResult PlayBGMFade(string _name・参照名, int speed)
        {
            bgmName・曲名 = _name・参照名;
            return PlayFade(_name・参照名, speed);
        }

        /// <summary>
        /// BGMの再生用のstop関数
        /// </summary>
        /// <returns></returns>
        public YanesdkResult StopBGM()
        {
            if (bgmName・曲名 == "")
            {
                return YanesdkResult.NoError; // 再生中じゃなさげ
            }
            else
            {
                string _name = bgmName・曲名;
                // 停止しても，直後に再生していた曲名は覚えておく．// bgmName・曲名 = "";
                return Stop(_name);
            }
        }

        /// <summary>
        /// FadeOutつきの stopBGM 。FadeInのスピードは[ms]単位。
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public YanesdkResult StopBGMFade(int speed)
        {
            if (bgmName・曲名 == "")
            {
                return YanesdkResult.NoError; // 再生中じゃなさげ
            }
            else
            {
                string _name = bgmName・曲名;
                // 停止しても，直後に再生していた曲名は覚えておく．// bgmName・曲名 = "";
                return StopFade(_name, speed);
            }
        }

        /// <summary>
        ///  BGMのクロスフェード用関数．現在再生中のBGMをフェードアウトさせつつ、
        /// 新しいBGMをフェードインして再生するための関数。
        /// fade in/outは同じスピードで行なわれる。
        ///	スピードは[ms]単位。fade inするBGMとfade outするBGMの再生chunkが
        ///	異なることが前提。
        /// </summary>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public YanesdkResult PlayBGMCrossFade(string _name・参照名, int speed)
        {
            StopBGMFade(speed);
            return PlayBGMFade(_name・参照名, speed);
        }

        #endregion

        #endregion

        #region properties

        /// <summary>
        ///	サウンドが再生中かを調べる
        /// </summary>
        ///	ただし、再生中か調べるために、ファイルを読み込まれていなければ
        ///	ファイルを読み込むので注意。
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <returns></returns>
        public bool IsPlaying(string _name・参照名)
        {
            if (!EnableSound・BGMとSEとVOICE再生が全てONかどうか) return false; // そのまま帰ればok
            CSoundPlayData・オーディオ再生用クラス s = getSound・オーディオ再生用クラスを取得(_name・参照名);
            if (s == null)
                return false;
            return s.IsPlaying();
        }

        /// <summary>
        /// サウンド（BGM，SE，VOICEを含む）の有効/無効の切り替え。初期状態ではtrue(有効)。
        /// </summary>
        /// <param name="bEnable"></param>
        /// <remarks>
        /// <Para>
        /// 初期状態ではtrue(有効)。
        /// これを無効にして、そのあと有効に戻したとき、playBGMで再生指定が
        /// されているBGMがあれば、有効に戻した瞬間にその番号のBGMを
        /// 再生する。
        /// </Para>
        /// <Para>
        /// 無効状態においては、play/playFade/playSE/playBGMの呼び出しすべてに
        /// おいて何も再生されないことが保証される。(ただし、このクラスを通じて
        /// 再生するときのみの話で、Soundクラスを直接getで取得して再生するならば
        /// この限りではない)
        /// </Para>
        /// <Para>
        /// また、有効状態から無効状態へ切り替えた瞬間、musicとすべてのchunkでの
        /// 再生を停止させる。
        /// </Para>
        /// </remarks>
        public bool EnableSound・BGMとSEとVOICE再生が全てONかどうか
        {
            set
            {
                if (value == isSoundEnable) return; // 同じならば変更する必要なし
                if (value)
                {
                    // ●サウンド再生ON
                    isSoundEnable = true;
                    // 状態を先に変更しておかないと、playBGMが無効になったままだ

                    //	無効状態から有効化されたならば、
                    //	BGM再生要求がヒストリーにあればそれを再生する
                    if (bgmName・曲名 != "")
                    {
                        PlayBGM(bgmName・曲名);
                    }
                }
                else
                {
                    // ●サウンド再生OFF
                    //	無効化するならサウンドすべて停止させなくては..
                    if (bgmName・曲名 != "")
                    {
                        //	再生中のものを停止させていることを確認して
                        //	再生中のBGMでなければ、BGMは再生されてないと把握し
                        //	次にenableSound(true)とされたときも再開させる必要はない
                        if (IsPlaying(bgmName・曲名) == false)
                        {
                            bgmName・曲名 = "";
                        }
                    }
                    CSoundPlayData・オーディオ再生用クラス.StopAll();
                    isSoundEnable = false;
                    //	このあとで無効化しなくては、bSoundEnableがfalseの状態だと
                    //	stop等が利かない可能性がある
                }
            }
            get { return isSoundEnable; }
        }

        #endregion

        #region private
        /// <summary>
        /// 現在再生しているBGMの名前(""ならば再生してない)
        /// </summary>
        protected string bgmName・曲名 = "";

        /// <summary>
        /// サウンド（BGM，SE，VOICEを含む）再生がONかどうか(デフォルトはtrue)
        /// </summary>
        protected bool isSoundEnable = true;
        #endregion

        #region overridden from base class(CCachedObjectLoader_ByString)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="soundItemInfo"></param>
        /// <returns></returns>
        protected override ICachedObject OnDefFileLoaded(CResourceData・資源データ info)
        {
            //	if (Factory == null)
            //		return null; //  YanesdkResult.PreconditionError; // factoryが設定されていない

            // soundItemInfo._filename, soundItemInfo.opt1 を渡したい

            // default値は""扱い。
            if (info.p3to_option・列リスト.Count - 1 < p_indexMax)
            {
                while (info.p3to_option・列リスト.Count - 1 < p_indexMax)
                {
                    info.p3to_option・列リスト.Add("");
                }
            }

            // 内部で相当チェックしてるからたぶん要らない
            //try
            //{
            string _fileName0_NotFullPath = info.p2_fileName・ファイル名;
            int _chanelNo = MyTools.parseInt(info.p3to_option・列リスト[p_index_soundChannelNo・チャンネル番号のオプション配列インデックス]);
            string _fileName0_FullPath = info.p3to_option・列リスト[p_index_soundFileNameFullPath・フルパスのオプション配列インデックス];
                
            // １行に二つ以上のファイル名（ファイル名１～）が書かれている場合、それらのファイル名を追加
            int _index = p_index_otherFileNameStart_5・２つ目以上のファイル名の開始オプション配列インデックス;
            int _fulPathIndex = p_index_otherFileNameFullPath_AddIndex_1・２つ目以上のファイル名フルパスのオプション配列インデックス;
            int _addIndex = p_index_NextFileName_AddIndex_2・次のファイル名へ進む時のオプション配列インデックス;
            // リストに、（フルパスでない）ファイル名１、フルパス１、ファイル名２、フルパス２、…という順に格納する。
            List<string> _fileLists・ファイル名１からＮまでとフルパス = new List<string>();
            string _fileName2_simple = MyTools.getListValue(info.p3to_option・列リスト, _index);
            string _fileName2_NullPath = "";
            while (_fileName2_simple != null)
            {
                // getListValueは要素がなければnullが返る
                _fileName2_simple = MyTools.getListValue(info.p3to_option・列リスト, _index);
                _fileName2_NullPath = MyTools.getListValue(info.p3to_option・列リスト, _index + _fulPathIndex);
                _fileLists・ファイル名１からＮまでとフルパス.Add(_fileName2_simple);
                _fileLists・ファイル名１からＮまでとフルパス.Add(_fileName2_NullPath);
                _index += _addIndex;
            }

            // オーディオ再生用クラスとしてICachedObjectを生成
            int _LoopNum = 0; // デフォルトでは0（1回のみ再生）
            if (_chanelNo == -1) _LoopNum = -1; // ループ再生
            ICachedObject obj = new CSoundPlayData・オーディオ再生用クラス(_LoopNum); // Factory();
            // ICachedObjectの中に、オーディオデータの定義（ファイル名０が存在しているかはコンストラクタ内で検証）
            obj.Construct(new CSoundConstructAdaptor・オーディオデータ定義クラス(
                    info.p0_id・ID, info.p1_name・参照名, _fileName0_NotFullPath, _chanelNo, _fileName0_FullPath,
                     _fileLists・ファイル名１からＮまでとフルパス.ToArray())
                     );

            //}
            //catch (Exception _e)
            //{
            //    Program・プログラム.printlnLog(ELogType.l5_エラーダイアログ表示, "※サウンドデータベース読み込みエラー。サウンドデータベース.csvに何らかの入力ミスがあります．\n「,」を「.」と間違えて使ったりしていませんか？ : " + _e.StackTrace);
            //}
            return obj;
        }

        #region OptNumは現在はいらない
        /* 
        /// <summary>
        /// オプションとしてchunk no（サウンドのチャンク番号）を指定するのでこの値は1になる。
        /// </summary>
        protected override int OptNum・オプションの数
        {
            get { return 1; }
        }
         * */
        #endregion
        #endregion
    }

    /// <summary>
    /// SoundLoaderのSmartLoader版
    /// </summary>
    /// <remarks>
    /// SmartSoundLoaderを用いるときは、
    /// SoundLoaderが勝手にSoundを解放するとSmartSoundLoaderが困るので
    /// SoundLoader.Disposeは呼び出さないこと。
    /// </remarks>
    public class CSmartSoundLoader : CCachedObjectSmartLoader_ByString・オブジェクト読み込み機<CAudioLoader_FromName・オーディオデータ読み込み機> { }


}
