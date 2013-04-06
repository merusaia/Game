using System;
using System.Diagnostics;
using Sdl;
using Yanesdk.Ytl;

// 一部にYaneSDKのソースを含む．やねうらお様に深く感謝いたします．

namespace PublicDomain
{

    /// <summary>
    /// CSoundPlayData・オーディオ再生用クラス クラスのための再生品質設定クラス。
    /// </summary>
    /// <remarks>
    /// <code>
    /// int audio_rate; // = 44100;
    /// ushort audio_format; // = AUDIO_S16;
    /// int audio_channels; //	= 2;
    /// int audio_buffers; // = 4096;
    /// </code>
    /// 初期状態では上記のようになっている。
    /// 
    /// あえて変更したければsingletonオブジェクト通じて変更すること。
    /// </remarks>
    public class CAudioConfig
    {

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        public CAudioConfig()
        {
            audioRate = 44100;
            audioFormat = (int)SDL.AUDIO_S16;
            audioChannels = 2;
            audioBuffers = 4096;

            Update();
        }
        #endregion

        #region properties
        /// <summary>
        /// 再生周波数(default:44100)。
        /// 値を変更したなら、Updateメソッドを呼び出すべし。
        /// </summary>
        public int AudioRate
        {
            get { return audioRate; }
            set { audioRate = value; dirty = true; }
        }
        private int audioRate;

        /// <summary>
        /// 再生ビット数(default:AUDIO_S16)。
        /// </summary>
        /// <remarks>
        /// このAUDIO_S16というのは、SDL_audio で定義されている。
        /// 値を変更したなら、Updateメソッドを呼び出すべし。
        /// </remarks>
        public int AudioFormat
        {
            get { return audioFormat; }
            set { audioFormat = value; dirty = true; }
        }
        private int audioFormat;

        /// <summary>
        /// 再生チャンネル数(default:2)。
        /// 値を変更したなら、Updateメソッドを呼び出すべし。
        /// </summary>
        public int AudioChannels
        {
            get { return audioChannels; }
            set { audioChannels = value; dirty = true; }
        }
        private int audioChannels;

        /// <summary>
        /// 再生バッファ長[bytes](default:4096)。
        /// 値を変更したなら、Updateメソッドを呼び出すべし。
        /// </summary>
        public int AudioBuffers
        {
            get { return audioBuffers; }
            set { audioBuffers = value; dirty = true; }
        }
        private int audioBuffers;

        /// <summary>
        /// 汚れフラグ。このフラグが立っていれば
        /// オーディオデバイスを再初期化する必要がある。
        /// </summary>
        public bool Dirty
        {
            get { return dirty; }
            set { dirty = value; }
        }
        private bool dirty = true;
        #endregion

        #region methods
        /// <summary>
        /// Audioデバイスを初期化する(明示的に呼び出す必要はないが、
        /// 他にAudioデバイスを必要とするモジュールがあるならばそこから呼び出すべし)
        /// 
        /// また、このクラスのフィールドを変更したときも呼び出すべし。
        /// </summary>
        /// <returns></returns>
        public YanesdkResult Update()
        {
            if (dirty)
            {
                // 処理をするのでフラグをおろす
                dirty = false;

                // 前回Openしていればいったん閉じる。
                if (isOpened)
                {
                    isOpened = false;
                    SDL.Mix_CloseAudio();
                }

                if (SDL.Mix_OpenAudio(audioRate, (ushort)audioFormat,
                    audioChannels, audioBuffers) < 0)
                {
                    // sound deviceが無いのかbufferがあふれたのかは不明なので
                    //	サウンドを使えなくすることはない
                    return YanesdkResult.PreconditionError;

                }

                // Openに成功したのでOpenフラグを立てておく。
                isOpened = true;

                //	どうせ高目で設定しても、その通りの能力を
                //	デバイスに要求するわけではないので．．
                //	  Mix_QuerySpec(&audio_rate, &audio_format, &audio_channels);
                //	↑最終的な結果は、これで取得できる
            }
            return YanesdkResult.NoError;
        }

        /// <summary>
        /// ミキサーの後始末
        /// </summary>
        public void Close()
        {
            if (isOpened)
            {
                isOpened = false;
                SDL.Mix_CloseAudio();
                // 次のUpdate()で再度初期化する
                dirty = true;
            }
        }

        #endregion

        #region private
        /// <summary>
        /// サウンドデバイスをopenしたのか。
        /// </summary>
        private bool isOpened = false;
        #endregion
    }

    /// <summary>
    /// サウンド（プログラム上の表記はオーディオ:Audio）再生用クラス。
    /// </summary>
    /// <remarks>
    /// サウンド再生の考えかた
    ///	１．music(bgm) + chuck(se)×8　の9個を同時にミキシングして出力出来る
    /// 
    ///	２．次のmusicが再生されると前のmusicは自動的に停止する
    /// 
    ///	３．seを再生するときには1～8のchuck(チャンク)ナンバーを指定できる
    ///	同じchuckナンバーのseを再生すると、前回再生していたものは
    ///	自動的に停止する
    /// 
    ///	４．musicもchunkも、どちらもwav,riff,ogg...etcを再生する能力を持つ
    /// 
    ///	５．midi再生に関しては、musicチャンネルのみ。
    ///	つまり、musicとchunkに関しては、５．６．の違いを除けば同等の機能を持つ。
    /// 
    ///	６．チャンクは、0を指定しておけば、1～8の再生していないチャンクを
    ///	自動的に探す。
    /// 
    ///	７．bgmとして、musicチャンネルを用いない場合(midiか、途中から再生
    ///	させるわけでもない限りは用いる必要はないと思われる)、
    ///	bgmのクロスフェードなども出来る。
    /// 
    /// また、UnmanagedResourceManager.Instance.SoundCache.LimitSizeで示される値まで
    /// 自動的にcacheする仕組みもそなわっている。そのため明示的にDisposeを呼び出す必要はない。
    /// </remarks>
    /// <example>
    /// 使用例)
    /// <code>
    /// CSoundPlayData・オーディオ再生用クラス _filename = new CSoundPlayData・オーディオ再生用クラス();
    /// _filename.load("1.ogg",-1);
    /// _filename.setLoop(-1); // endless
    /// _filename.play();
    /// 
    /// CSoundPlayData・オーディオ再生用クラス _s2_shortPathName_StringBuilder = new CSoundPlayData・オーディオ再生用クラス();
    /// _s2_shortPathName_StringBuilder.load("extend.wav",1);
    /// _s2_shortPathName_StringBuilder.play();
    /// </code>
    /// </example>
    public class CSoundPlayData・オーディオ再生用クラス : CCachedObject・一時記憶メモリシステム, ILoader, IDisposable
    {
        #region ctor & Dispose
        /// <summary>
        /// static class constructorはlazy instantiationが保証されているので
        /// ここで SDL_mixerが間接的に読み込むdllを事前に読み込む。
        /// </summary>

        /* SDL_Initializerで行なうように変更
        static CSoundPlayData・オーディオ再生用クラス()
        {
            // Sound関連のDLLを必要ならば読み込んでおくべ。

            DllManager d = DllManager.Instance;
            string current = DllManager.DLL_CURRENT;
            d.LoadLibrary(current, DLL_OGG);
            d.LoadLibrary(current, DLL_VORBIS);
            d.LoadLibrary(current, DLL_VORBISFILE);
            d.LoadLibrary(current, DLL_SMPEG);
        }
        */

        /// <summary>
        /// コンストラクタでは、Audioデバイスの初期化も行なう。
        /// </summary>
        public CSoundPlayData・オーディオ再生用クラス(int _LoopNum・ループ回数＿ループ無しは０＿無限ループならマイナス１)
        {
            This = this; // mix-in用
            this.Loop = _LoopNum・ループ回数＿ループ無しは０＿無限ループならマイナス１;
            // Audioデバイスの初期化。
            // CAudioConfig.Update();
            // ↑これは、initがコンストラクタで行なう

            CacheSystem = CUnmanagedResourceManager・資源管理者.Instance.SoundMemory;
            CacheSystem.Add(this);
        }

        /// <summary>
        /// このメソッドを呼び出したあとは再度Loadできない。
        /// Loadしたデータを解放したいだけならばReleaseを呼び出すこと。
        /// </summary>
        public void Dispose()
        {
            Release();

            init.Dispose();
            CacheSystem.Remove(this);
        }
        #endregion

        #region ILoaderの実装

        /// <summary>
        /// Load・ロード(_fileName0_FullPath,0)と等価。
        /// デフォルトはループ無し再生。
        /// ILoader interfaceが要求するので辻褄合わせに用意してある。
        /// </summary>
        /// <param name="_fileName0_FullPath"></param>
        /// <returns></returns>
        public YanesdkResult Load(string _filename)
        {
            return Load・ロード(_filename, 0, ""); 
        }

        /// <summary>
        /// サウンドファイルを読み込む
        /// </summary>
        /// <param name="_fileName_NotFullPath_ファイル名_名前だけ">ファイル名「sample.mp3」など</param>
        /// <param name="_channelNo・チャンネル番号">読み込むチャンネル
        /// -1 : BGMチャンネル          : 0のchunkを更新してループ再生．midiもOKのBGM専用．
        /// 1  : ループ無しBGMチャンネル: 5のchunkを使用し，0のchunkを一時停止して割り込む
        ///                                （例：レベルアップ時，ファンファーレなど）
        /// 　　　　　　　　　　　　　　　 （※なお，ボリュームはSEの方に含まれる）
        /// 2  : VOICEチャンネル        : 1～4のchunkのうち再生していないチャンネルを昇順に検索して再生，無かったら5～8のchunkも探す．
        /// 0  : SEチャンネル(省略時も0): 1～8のchunkのうち再生していないチャンネルを降順に検索して再生
        /// 
        /// 3～8 : 指定したchunkに読み込む
        /// </param>
        /// <returns>読み込みエラーならばYanesdkResult.no_error以外</returns>
        public YanesdkResult Load・ロード(string _fileName_NotFullPath_ファイル名_名前だけ, int _channelNo・チャンネル番号, string _fileNameFullPath_ファイルの存在を確認したフルパス_存在しない場合はNone)
        {
            Release();

            //string _fileName0_FullPath = Program・プログラム.p_SoundDatabaseFileName_FullPath・サウンドデータベースファイルパス + _fileNameFullPath_ファイルの存在を確認したフルパス_存在しない場合はNone + _fileName_NotFullPath_ファイル名_名前だけ;
            string _fileName_FullPath = _fileNameFullPath_ファイルの存在を確認したフルパス_存在しない場合はNone;
            YanesdkResult result;
            if (_channelNo・チャンネル番号 == -1) // BGM
            {
                result = LoadMusic(_fileName_FullPath); // chunk0
            }
            else if (_channelNo・チャンネル番号 == 1) // ループ無しBGM
            {
                result = LoadChunk(_fileName_FullPath, 5); // chunk5
            }
            else if (_channelNo・チャンネル番号 == 2) // VOICE
            {
                result = LoadChunk(_fileName_FullPath, Math.Max(1, CSoundPlayData・オーディオ再生用クラス.ChunkManager.GetEmptyChunk())); // chunk1～8の空き
            }
            else if (1 <= _channelNo・チャンネル番号 && _channelNo・チャンネル番号 <= 8){
                result = LoadChunk(_fileName_FullPath, _channelNo・チャンネル番号);
            }
            else if (_channelNo・チャンネル番号 == 0) // 効果音
            {
                result = LoadChunk(_fileName_FullPath, CSoundPlayData・オーディオ再生用クラス.ChunkManager.GetEmptyChunk_Reverse()); // chunk8～1の空き
            }
            else {
                Program・実行ファイル管理者.printlnLog(ELogType.l5_エラーダイアログ表示, "チャンネル番号が不正です．-1～8までのはずです． ChannelNo=" + _channelNo・チャンネル番号);
                result = YanesdkResult.InvalidParameter; //	errorですよ、と。
            }	

            // サウンドファイルを読み込む
            if (result == YanesdkResult.NoError)
            {
                loaded = true;
                this.fileName = _fileName_FullPath;

                // もし、ローダー以外からファイルを単に読み込んだだけならば、Reconstructableにしておく。
                if (constructInfo・オブジェクト再構築データ == null)
                {
                    // 再構築する
                    // （※よくわからないが、サウンドデータベース.csvを読みこんだ段階では作ったデータが残っていない場合、ここでもう一回作るってこと？
                    // とりあえず再構築データは１ファイル（参照名はフルパスじゃないファイル名の拡張子無し版にしておこう）
                    // 参照名は、拡張子を抜いたバージョン
                    string _audioDataName・参照名 = MyTools.getFileLeftOfPeriodName(MyTools.getFileName_NotFullPath_LastFileOrDirectory(_fileName_FullPath));
                    constructInfo・オブジェクト再構築データ = 
                        new CSoundConstructAdaptor・オーディオデータ定義クラス(
                            0, _audioDataName・参照名, _fileName_FullPath, _channelNo・チャンネル番号, _fileNameFullPath_ファイルの存在を確認したフルパス_存在しない場合はNone);
                }

                // リソースサイズが変更になったことをcache systemに通知する
                // Releaseのときに0なのは通知しているので通知するのは正常終了時のみでok.
                CacheSystem.OnResourceChanged(this);
            }

            return result;
        }
        // これを呼び出しているのは，下の方にある，baseクラスのOnReconstructというメソッド．

        /// <summary>
        /// ファイルの読み込みが完了しているかを返す
        /// </summary>
        /// <returns></returns>
        public bool Loaded { get { return loaded; } }
        private bool loaded = false;

        /// <summary>
        /// loadで読み込んだサウンドを解放する
        /// </summary>
        public void Release()
        {
            if (music != IntPtr.Zero)
            {
                Stop();
                SDL.Mix_FreeMusic(music);
                music = IntPtr.Zero;
            }
            if (chunk != IntPtr.Zero)
            {
                Stop();
                SDL.Mix_FreeChunk(chunk);
                chunk = IntPtr.Zero;
            }
            tmpFile = null;
            loaded = false;
            fileName = null;

            // リソースサイズが変更になったことをcache systemに通知する
            CacheSystem.OnResourceChanged(this);

            constructInfo・オブジェクト再構築データ = null;
        }

        /// <summary>
        /// ファイルを読み込んでいる場合、読み込んでいるファイル名を返す
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }
        private string fileName;

        #endregion

        #region methods
        /// <summary>
        /// loadで読み込んだサウンドを再生する
        /// </summary>
        /// <returns>
        /// 再生エラーならばYanesdkResult.no_error以外が返る
        /// </returns>
        public YanesdkResult Play()
        {
            // cache systemにこのSoundを使用したことを通知する
            CacheSystem.OnAccess(this);
            isPlayingLast = true;

            if (NoSound) return YanesdkResult.NoError;
            Stop(); // 停止させて、sound managerの再生チャンネルをクリアしなければ
            if (music != IntPtr.Zero) return PlayMusic();
            if (chunk != IntPtr.Zero) return PlayChunk();
            return YanesdkResult.PreconditionError; // Sound読み込んでないぽ
        }

        /// <summary>
        /// フェードイン付きのplay。speedはfade inに必要な時間[ms]
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        public YanesdkResult PlayFade(int speed)
        {
            CacheSystem.OnAccess(this);
            isPlayingLast = true;

            if (NoSound) return YanesdkResult.NoError;
            Stop(); // 停止させて、sound managerの再生チャンネルをクリアしなければ
            if (music != IntPtr.Zero) return PlayMusicFade(speed);
            if (chunk != IntPtr.Zero) return PlayChunkFade(speed);
            return YanesdkResult.PreconditionError; // Sound読み込んでないぽ
        }

        /// <summary>
        ///	play中のサウンドを停止させる
        /// </summary>
        ///	読み込んでいるサウンドデータ自体を解放するには release を
        ///	呼び出すこと。こちらは、あくまで停止させるだけ。次にplayが
        ///	呼び出されれば、再度、先頭から再生させることが出来る。
        /// <returns></returns>
        public YanesdkResult Stop()
        {
            CacheSystem.OnAccess(this);
            isPlayingLast = false;

            //	stopは、channelごとに管理されているので、
            //	自分が再生しているchannelなのかどうかを
            //	このクラスが把握している必要がある
            if (NoSound) return YanesdkResult.NoError;
            return ChunkManager.Stop(this);
        }

        /// <summary>
        /// 一時停止を行なう。
        /// 再生中にこのメソッドを呼び出した場合、IsPlayingはtrueを返す。
        /// その後、バッファを破棄するならばStopを必ず呼び出すこと。
        /// そうしないと、いつまでもこのchunkはIsPlayingがtrueを返すので
        /// Playでchunkをおまかせにしている場合、空きchunkがないと判断されることになる。
        /// </summary>
        /// <returns></returns>
        public YanesdkResult Pause()
        {
            CacheSystem.OnAccess(this);
            isPlayingLast = false;

            if (NoSound) return YanesdkResult.NoError;
            return ChunkManager.Pause(this);
        }

        /// <summary>
        /// Pauseで停止させていたならば、
        /// それを前回停止させていた再生ポジションから再開する。
        /// </summary>
        /// <returns></returns>
        public YanesdkResult Resume()
        {
            CacheSystem.OnAccess(this);
            isPlayingLast = true;

            if (NoSound) return YanesdkResult.NoError;
            return ChunkManager.Resume(this);
        }

        /// <summary>
        ///	musicチャンネルを(徐々に)フェードアウトさせる
        /// </summary>
        /// <remarks>
        /// speedはfadeoutスピード[ms]
        /// </remarks>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static YanesdkResult FadeMusic(int speed)
        {
            if (NoSound) return YanesdkResult.PreconditionError;
            ChunkManager.music = null;
            return SDL.Mix_FadeOutMusic(speed) == 0 ?
                YanesdkResult.NoError : YanesdkResult.SdlError;
        }

        /// <summary>
        /// 0～7のチャンネルを(徐々に)フェードアウトさせる
        /// </summary>
        /// <remarks>
        /// speedはfadeoutスピード[ms]
        /// </remarks>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static YanesdkResult FadeChunk(int speed)
        {
            if (NoSound) return YanesdkResult.PreconditionError;
            for (int i = 0; i < 8; ++i)
            {
                ChunkManager.SEchunk_0To7[i] = null;
                SDL.Mix_FadeOutChannel(i, speed);
            }
            return YanesdkResult.NoError;
        }

        /// <summary>
        ///	すべてのchunk(除くmusicチャンネル)の再生を停止させる
        /// </summary>
        /// <remarks>
        ///	このメソッド呼び出し中に他のスレッドから
        ///	サウンド関係をいじることは考慮に入れていない
        /// </remarks>
        public static void StopAllChunk()
        {
            ChunkManager.StopAllChunk();
        }

        /// <summary>
        ///	musicチャンネルの再生を停止させる
        /// </summary>
        /// <remarks>
        ///	このメソッド呼び出し中に他のスレッドから
        ///	サウンド関係をいじることは考慮に入れていない
        /// </remarks>
        /// <returns></returns>
        public static YanesdkResult StopMusic()
        {
            return ChunkManager.StopMusic();
        }

        /// <summary>
        ///	musicチャンネルと、すべてのchunkの再生を停止させる
        /// </summary>
        /// <remarks>
        /// このメソッド呼び出し中に他のスレッドから
        ///	サウンド関係をいじることは考慮に入れていない
        /// </remarks>
        /// <returns></returns>
        public static YanesdkResult StopAll()
        {
            StopAllChunk();
            return StopMusic();
        }

        // ●●●●● SDLを使うか（緊急停止）
        bool p_isUsedSDL・SDLを使うか口口口OS非依存サウンドを信用口口口 = true; // falseならMyTools.playSoundをやっつけで使う。
        /// <summary>
        /// ファイル(wav,ogg,mid)の読み込み。
        /// ここで読み込んだものは、bgmとして再生される
        /// </summary>
        /// <param name="_fileName0_FullPath"></param>
        /// <returns></returns>
        private YanesdkResult LoadMusic(string filename)
        {
            if (NoSound) return YanesdkResult.PreconditionError;

            tmpFile = Yanesdk.System.FileSys.GetTmpFile(filename);
            string f = tmpFile.FileName;

            YanesdkResult result;
            if (f != null)
            {
                // ついに，実在するファイルを使ってロードできたよ！
                if (p_isUsedSDL・SDLを使うか口口口OS非依存サウンドを信用口口口 == true)
                {
                    music = SDL.Mix_LoadMUS(f); // 実際にやってみたら・・・何なん！，この「ブツッブツッブツ」ってのは！！？　音楽は？？
                }
                else
                {
                    // ※OS非依存のSDLがちゃんと再生しないから，やっつけでとりあえずWindowsのみのWin32APIで緊急対処

                    // とりあえずやっつけでplaySoundを呼び出す
                    //MyTools.stopSound();
                    MyTools.playSound(f, true);
                    string _kakutyousi = MyTools.getFileRightOfPeriodName(f);
                    MySound_Windows.playBGM(f, true);
                    //MyTools.playSound(_fileName0_FullPath, true);
                }

                // Debug.Fail(SDL.Mix_GetError());
                // ここでmusic == 0が返ってくるのは明らかにおかしい。
                // smpeg.dllがないのに mp3を再生しようとしただとか？

                if (music == IntPtr.Zero)
                    result = YanesdkResult.HappenSomeError;
                else
                    result = YanesdkResult.NoError;
            }
            else
            {
                result = YanesdkResult.FileNotFound; // file not found
            }

            //		music = Mix_LoadMUS_RW(rwops,1);
            //	この関数なんでないかなーヽ(´Д`)ノ

            //	↑については↓と教えていただきました。

            //	この関数は SDL_mixer をコンパイルするときに
            //	"USE_RWOPS" プリプロセッサ定義を追加すると使えるようです。
            //	http://ilaliart.sourceforge.jp/tips/mix_rwops.html

            // 最新のSDL_mixerならばあるらしい。

            //  ここで、注意して欲しいのは、Mix_LoadMUS_RW関数でMix_Musicオブジェクトを生成しても
            //  すぐにSDL_RWclose関数でSDL_RWopsオブジェクトを開放していないことである。
            //  Mix_Musicオブジェクトはストリーミング再生されるため、常にファイルを開いた状態でなければならない。
            //  そのため、SDL_RWcloseでロード後にすぐにファイルを閉じてしまった場合、Mix_PlayMusic関数で再生に失敗してしまう。
            //  そのため、再生に用いるSDL_RWopsオブジェクトは再生停止後に破棄する必要がある。
            /*
                SDL_RWopsH rwops = FileSys.ReadRW(_fileName0_FullPath);
                if (rwops.Handle != IntPtr.Zero)
                {
                    music = SDL.Mix_LoadMUS_RW(rwops.Handle);
                    // ↑このときrwopsをどこかで解放する必要あり

                }
                else
                {
                    return YanesdkResult.FileNotFound;	// file not found
                }

                if (music == IntPtr.Zero) {
                    return YanesdkResult.SdlError;
                }
             */
            // MacOSやmp3再生だと対応してないっぽいので、やっぱりこれ使うのやめ

            if (result == YanesdkResult.NoError)
            {
                this.fileName = filename;
                CacheSystem.OnResourceChanged(this);
            }

            return result;
        }

        /// <summary>
        /// ファイル(ogg,wav)の読み込み。
        /// </summary>
        /// <param name="_filename"></param>
        /// <returns></returns>
        /// <remarks>
        /// 空きチャンネルを自動的に使用する。
        /// 使用するチャンクナンバーはお任せバージョン。
        /// </remarks>
        private YanesdkResult LoadChunk(string name)
        {
            return LoadChunk(name, 0);
        }

        /// <summary>
        /// ファイル(ogg,wav)の読み込み。
        /// </summary>
        /// <param name="_filename"></param>
        /// <param name="_channelNo・チャンネル番号">
        /// 読み込むチャンクを指定。_channelNo・チャンネル番号=0なら自動で空きを探す 1～8</param>
        /// <returns></returns>
        private YanesdkResult LoadChunk(string filename, int _channelNo・チャンネル番号)
        {
            // ついに，実在するファイルを使ってロードできたよ！
            // ※OS非依存のSDLがちゃんと再生しないから，やっつけでとりあえずWindowsのみのWin32APIで緊急対処
            if (p_isUsedSDL・SDLを使うか口口口OS非依存サウンドを信用口口口 == true)
            {
                if (NoSound) return YanesdkResult.PreconditionError;
                SDL_RWopsH rwops = Yanesdk.System.FileSys.ReadRW(filename);
                if (rwops.Handle != IntPtr.Zero)
                {
                    chunk = SDL.Mix_LoadWAV_RW(rwops.Handle, 1);
                }
                else
                {
                    return YanesdkResult.FileNotFound;	// file not found
                }

                if (chunk == IntPtr.Zero)
                {
                    return YanesdkResult.FileReadError;	// 読み込みに失敗してやんの
                }
                chunkChannel = _channelNo・チャンネル番号;

                CacheSystem.OnResourceChanged(this);
            }
            else
            {
                // とりあえずやっつけでplaySound
                //MyTools.stopSound();
                MyTools.playSound(filename, false);
            }


            return YanesdkResult.NoError;
        }

        /// <summary>
        /// loadMusicで読み込んだBGMを再生させる
        /// </summary>
        /// <returns></returns>
        private YanesdkResult PlayMusic()
        {
            CacheSystem.OnAccess(this);
            isPlayingLast = true;

            if (NoSound) return YanesdkResult.PreconditionError;
            ChunkManager.music = this; // チャンネルの占拠を明示

            // volumeの設定(これは再生時に設定する)
            ChunkManager.SetVolume(0, Volume);

            if(SDL.Mix_PlayMusic(music, loopflag) == 0){
                return YanesdkResult.NoError;
            }else{
                return YanesdkResult.SdlError;
            }
        }

        /// <summary>
        /// loadMusicで読み込んだBGMをfadeさせる
        /// </summary>
        private YanesdkResult PlayMusicFade(int speed)
        {
            CacheSystem.OnAccess(this);
            isPlayingLast = true;

            if (NoSound) return YanesdkResult.PreconditionError;
            ChunkManager.music = this; // チャンネルの占拠を明示

            // volumeの設定(これは再生時に設定する)
            ChunkManager.SetVolume(0, Volume);

            return SDL.Mix_FadeInMusic(music, loopflag, speed) == 0 ?
                YanesdkResult.NoError : YanesdkResult.SdlError;
        }

        /// <summary>
        /// loadChunkで読み込んだサウンドを再生させる
        /// </summary>
        private YanesdkResult PlayChunk()
        {
            CacheSystem.OnAccess(this);
            isPlayingLast = true;

            if (NoSound) return YanesdkResult.PreconditionError;

            int ch =
                (chunkChannel == 0) ?
                // おまかせchunkでの再生ならば空きチャンクを探す
                    ch = ChunkManager.GetEmptyChunk()
                :
                    ch = chunkChannel - 1;

            // チャンネルの占拠を明示
            ChunkManager.SEchunk_0To7[ch] = this;
            //	↑このチャンクが使用中であることはここで予約が入る

            // volumeの設定(これは再生時に設定する)
            ChunkManager.SetVolume(ch + 1, Volume);

            return SDL.Mix_PlayChannel(ch, chunk, loopflag) == ch ?
                YanesdkResult.NoError : YanesdkResult.SdlError;
        }

        /// <summary>
        /// loadChunkで読み込んだサウンドをfadeさせる
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        private YanesdkResult PlayChunkFade(int speed)
        {
            CacheSystem.OnAccess(this);
            isPlayingLast = true;

            if (NoSound) return YanesdkResult.PreconditionError;
            int ch =
                (chunkChannel == 0) ?
                // おまかせchunkでの再生ならば空きチャンクを探す
                    ch = ChunkManager.GetEmptyChunk()
                :
                    ch = chunkChannel - 1;

            // チャンネルの占拠を明示
            ChunkManager.SEchunk_0To7[ch] = this;
            //	↑このチャンクが使用中であることはここで予約が入る

            // volumeの設定(これは再生時に設定する)
            ChunkManager.SetVolume(ch + 1, Volume);

            return SDL.Mix_FadeInChannel(ch, chunk, loopflag, speed) == ch ?
                YanesdkResult.NoError : YanesdkResult.SdlError;
        }

        #endregion

        #region properties
        /// <summary>
        /// volumeの設定。0.0～1.0までの間で。
        /// 1.0なら100%の意味。
        /// 
        /// master volumeのほうも変更できる。
        ///		出力volume = (master volumeの値)×(ここで設定されたvolumeの値)
        /// である。
        /// 
        /// ここで設定された値はLoad/Play等によっては変化しない。
        /// 再設定されるまで有効である。
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;

                // 再生中ならば、そのchunkのvolumeの再設定が必要だ。
                int ch = GetPlayingChunk();
                if (ch != -1) // 「再生なし」でなければ
                    ChunkManager.SetVolume(ch, volume);
            }
        }
        private float volume = 1.0f;

        /// <summary>
        /// マスターvolumeの設定
        /// すべてのSoundクラスに影響する。
        ///		出力volume = (master volumeの値)×(volumeの値)
        /// である。
        /// </summary>
        /// <param name="volume"></param>
        public static float MasterVolume
        {
            get { return ChunkManager.MasterVolume; }
            set { ChunkManager.MasterVolume = value; }
        }

        /// <summary>
        /// stopのフェード版
        /// </summary>
        /// <remarks>
        /// fadeoutのスピードを指定できる。speed の単位は[ms]。
        ///	その他はstopと同じ。
        ///	</remarks>
        /// <param name="speed"></param>
        /// <returns></returns>
        public YanesdkResult StopFade(int speed)
        {
            if (NoSound)
                return YanesdkResult.NoError;

            return ChunkManager.StopFade(this, speed);
        }

        /// <summary>
        /// 再生中かを調べる
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            // このメソッドはReconstructableから再三呼び出される可能性があるので、
            // 少しでも高速化しておきたい。
            // そのため、結果をcacheし、前回再生されていなくて、
            // そのあとPlayを呼び出していなければ即座にfalseを返す実装にする。

            if (!isPlayingLast)
                return false;

            bool isPlaying = (GetPlayingChunk() != -1);
            isPlayingLast = isPlaying;

            return isPlaying;
        }

        /// <summary>
        /// 前回IsPlayingが呼び出されたときのcache結果。
        /// 
        /// falseならば無条件でIsPlayingはfalse
        /// trueならば再生されている可能性があるのでそれを調べる。
        /// </summary>
        private bool isPlayingLast = false;

        /// <summary>
        /// 自分が再生中のchunkを返す。
        /// -1   : なし
        ///  0   : BGM SEchunk_0To7
        ///  1-4 : VOICE SEchunk_0To7
        ///  5-8 : SE SEchunk_0To7
        /// </summary>
        /// <returns></returns>
        public int GetPlayingChunk()
        {
            if (NoSound) return -1;
            if (ChunkManager.music == this && SDL.Mix_PlayingMusic() != 0) return 0;
            for (int i = 0; i < 8; ++i)
            {
                if (ChunkManager.SEchunk_0To7[i] == this && SDL.Mix_Playing(i) != 0) return i + 1;
            }
            return -1; // not found
        }

        /// <summary>
        /// ループ回数の設定/取得
        /// 
        /// ■値の意味  
        /// -1:endless
        /// 0 :1回のみ(default)
        /// 1 :2回
        /// 
        /// 以下、再生したい回数-1を指定する。
        /// </summary>
        /// <param name="loop">
        /// -1:endless
        /// 0 :1回のみ(default)
        /// 1 :2回
        /// 
        /// 以下、再生したい回数-1を指定する。
        /// </param>
        /// <remarks>
        /// ここで設定した値は一度設定すれば再度この関数で
        /// 変更しない限り変わらない
        /// </remarks>
        public int Loop
        {
            set { loopflag = value; }
            get { return loopflag; }
        }

        /// <summary>
        /// 音なしモードなのか
        /// </summary>
        public static bool NoSound
        {
            get
            {
                //	return CAudioConfig.noSound;
                using (AudioInit init = new AudioInit())
                {
                    return init.NoSound;
                }
            }
        }

        /// <summary>
        /// SoundConfigへのsingletonなインスタンス
        /// </summary>
        /// <returns></returns>
        public static CAudioConfig SoundConfig
        {
            //.NET 4.0のときはこれでビルド通った？それともただリファクタリングで変更箇所間違っただけ？get { return Singleton<SoundConfig>.Instance; }
            get { return Singleton<CAudioConfig>.Instance; }
        }

        /// <summary>
        /// SoundChunkManagerへのsingleton。
        /// 外部からChunkManagerをどうしてもいじりたいときにだけ使うべし。
        /// </summary>
        public static SoundChunkManager ChunkManager
        {
            get { return Singleton<SoundChunkManager>.Instance; }
        }

        #endregion

        #region overridden from base class
        /// <summary>
        ///	loadで読み込んでいるサウンドのために確保されているバッファ長を返す
        /// </summary>
        /// <remarks>
        /// 全体で使用している容量を計算したりするのに使える．．かな？
        /// </remarks>
        /// <returns></returns>
        public override long ResourceSize
        {
            get { return getBufferSize_(); }
        }

        private long getBufferSize_()
        {
            return 0;
        }
        // Yanesdkコードから変更:元はアンセーフコードだった
        /*
        unsafe private long getBufferSize_()
        {
            if (SEchunk_0To7 == IntPtr.Zero) return 0;
            SDL.Mix_Chunk* chunk_ = (SDL.Mix_Chunk*)SEchunk_0To7;
            if (chunk_->allocated != 0)
                return chunk_->alen;
            return 0;
        }*/

        /// <summary>
        /// サウンドファイルをMusic/SE/Voice毎にチャンネル番号を区別して読み込みます．
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override YanesdkResult OnReconstruct(object param)
        {
            /*
            CSoundConstructAdaptor・オーディオデータ定義クラス soundItemInfo = param as CSoundConstructAdaptor・オーディオデータ定義クラス;
            return Load・ロード(soundItemInfo.p_fileName・ファイル名, soundItemInfo.p_channelNo・再生チャンネル番号);
            */
            CSoundConstructAdaptor・オーディオデータ定義クラス soundItemInfo;
            try
            {
                soundItemInfo = param as CSoundConstructAdaptor・オーディオデータ定義クラス;
                if (soundItemInfo == null)
                {
                    // サウンドがが見つからない・・・しょ～がないから、デフォルトの曲を取得
                    soundItemInfo = getDefaultAudioData_fileNotFound・デフォルトのオーディオデータを取得();
                }
            }
            catch (Exception e)
            {
                // サウンドがが見つからない・・・しょ～がないから、デフォルトの曲を取得
                soundItemInfo = getDefaultAudioData_fileNotFound・デフォルトのオーディオデータを取得();
            }
            return Load・ロード(soundItemInfo.p_fileName・ファイル名, soundItemInfo.p_channelNo・再生チャンネル番号, soundItemInfo.p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある);

        }
        public static CSoundConstructAdaptor・オーディオデータ定義クラス getDefaultAudioData_fileNotFound・デフォルトのオーディオデータを取得()
        {
            if (s_defaultAudioData_fileNotFound・ファイル見つからない時のデフォルトのオーディオデータ == null)
            {
                s_defaultAudioData_fileNotFound・ファイル見つからない時のデフォルトのオーディオデータ = new CSoundConstructAdaptor・オーディオデータ定義クラス(
                    -1, MyTools.getEnumName(EBGM・曲._fileNotFound・ファイル読み込み失敗した時に代わりに鳴らす音), s_defaultAudioDataFileName_NotFullPath・ファイル見つからない時のデフォルトのオーディオデータファイル, 0, ""); // 再生チャンネルを-1以外にして繰り返しは避ける
            }
            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ,
                "※サウンドの再構築データがnullのエラーにより，指定した音が鳴りません．デフォルト「"+s_defaultAudioDataFileName_NotFullPath・ファイル見つからない時のデフォルトのオーディオデータファイル+"」で勘弁してください．すみません…");

            return s_defaultAudioData_fileNotFound・ファイル見つからない時のデフォルトのオーディオデータ;
        }
        private static CSoundConstructAdaptor・オーディオデータ定義クラス s_defaultAudioData_fileNotFound・ファイル見つからない時のデフォルトのオーディオデータ;
        private static string s_defaultAudioDataFileName_NotFullPath・ファイル見つからない時のデフォルトのオーディオデータファイル = "ピリンッ２.wav";

        /// <summary>
        /// 再生中に解放されると困るので何とか穏便に頼む。
        /// </summary>
        public override bool Reconstructable 
        {
            get
            {
                return constructInfo・オブジェクト再構築データ != null && !IsPlaying();
            }
        }
        //	IsPlayingの判定に時間がかかると嫌だなぁ…
        //	まあ、そんなに大量のSEが存在することはありえないか。

        #endregion

        #region private
        private IntPtr music; // SDLのなんぞ
        private IntPtr chunk; // SDLのなんぞ
        private int chunkChannel;	// 再生するときのchunkの使用番号 
        private int loopflag;		// 再生をループさせるのか
        private Yanesdk.System.FileSys.TmpFile tmpFile;	//	ファイルからしか再生できないものはテンポラリファイルを利用する
        #endregion

        #region internal classes

        /// <summary>
        /// SDL Audioの初期化用クラス
        /// </summary>
        internal class AudioInit : IDisposable
        {
            public AudioInit()
            {
                NoSound = init.Instance.Result < 0;
            }

            public void Dispose()
            {
                init.Dispose();
            }

            private RefSingleton<Yanesdk.Sdl.SDL_AUDIO_Initializer>
                init = new RefSingleton<Yanesdk.Sdl.SDL_AUDIO_Initializer>();

            /// <summary>
            /// サウンドデバイスはついていなければtrue。
            /// </summary>
            internal bool NoSound;
        }

        /// <summary>
        /// SDL Audioの初期化用
        /// </summary>
        private RefSingleton<AudioInit>
            init = new RefSingleton<AudioInit>();

        /// <summary>
        /// あるチャンクを再生しているインスタンスを把握しておく必要がある。
        /// CSoundPlayData・オーディオ再生用クラス.Manager経由でアクセスしてちょうだい。
        /// </summary>
        /// <remarks>
        /// CSoundPlayData・オーディオ再生用クラス.Manager経由でアクセスしてちょうだい。
        /// </remarks>
        public class SoundChunkManager : IDisposable
        {
            /// <summary>
            /// musicチャンネルで現在再生中のクラスが入っている。
            /// </summary>
            public CSoundPlayData・オーディオ再生用クラス music;		//	musicチャンネルを再生中のやつを入れておく
            /// <summary>
            /// 各chunk1～8チャンネルに格納されているクラスが入っている。※配列は0～7を指定
            /// </summary>
            public CSoundPlayData・オーディオ再生用クラス[] SEchunk_0To7;	//	chunkチャンネルを再生中のやつを入れておく
            /// <summary>
            /// 特定のchunkのサウンド再生用クラスを取得する。
            /// _channelNo・チャンネル番号 == 0(music chuck),1～8(通常のchunk)
            /// </summary>
            public CSoundPlayData・オーディオ再生用クラス GetSound_ByChunkNo(int ch)
            {
                if (ch < 0) ch = 0;
                if (ch > 8) ch = 8;
                if (ch == 0) { return music; } else { return SEchunk_0To7[ch - 1]; }
            }
            
            public SoundChunkManager()
            {
                SEchunk_0To7 = new CSoundPlayData・オーディオ再生用クラス[8];

                chunkVolumes = new float[9];
                for (int i = 0; i < 9; ++i)
                    chunkVolumes[i] = 1.0f;
            }

            public void Dispose()
            {
            }

            /// <summary>
            /// 指定したチャンクが空いているかを調べる
            /// </summary>
            /// <param name="_filename"></param>
            /// <returns></returns>
            public bool GetIsEmptyChunk(int ch_0to8)
            {
                if (NoSound || SEchunk_0To7 == null) return false;
                // １～８は、SEチャネルとしては1～7、SDL.Mix_Playing(SEチャネル)
                if (ch_0to8 > 0) ch_0to8--;
                if (SEchunk_0To7[ch_0to8] == null) return false;
                // チャンクが空いているか（8を入れるとまずい？）
                if (SDL.Mix_Playing(ch_0to8) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /// <summary>
            /// 現在再生していないチャンクを昇順で探す
            /// </summary>
            /// <param name="_filename"></param>
            /// <returns></returns>
            public int GetEmptyChunk()
            {
                if (NoSound || SEchunk_0To7 == null) return 0;
                for (int i = 0; i < 8; ++i)
                {
                    if (SDL.Mix_Playing(i) == 0)
                    {
                        //	SEchunk_0To7[_index] = s;
                        // ↑ここでやる必要ぜんぜんねーな
                        return i;
                    }
                }
                return 0; // 空きが無いので0番を潰すかぁ..(´Д`)
            }
            /// <summary>
            /// 現在再生していないチャンクを降順で探す
            /// </summary>
            /// <param name="_filename"></param>
            /// <returns></returns>
            public int GetEmptyChunk_Reverse()
            {
                if (NoSound || SEchunk_0To7 == null) return 0;
                for (int i = 7; i >= 0; i--)
                {
                    if (SDL.Mix_Playing(i) == 0)
                    {
                        //	SEchunk_0To7[_index] = s;
                        // ↑ここでやる必要ぜんぜんねーな
                        return i;
                    }
                }
                return 0; // 空きが無いので0番を潰すかぁ..(´Д`)
            }

            /// <summary>
            /// 現在再生しているチャンクを停止させる
            /// </summary>
            /// <param name="_filename"></param>
            /// <returns></returns>
            public YanesdkResult Stop(CSoundPlayData・オーディオ再生用クラス s)
            {
                if (s == music)
                {
                    //	SDL.Mix_PauseMusic(); // Mix_HaltMusic();
                    SDL.Mix_HaltMusic();
                    music = null;
                }
                if (SEchunk_0To7 == null)
                    return YanesdkResult.PreconditionError;
                for (int i = 0; i < 8; ++i)
                {
                    if (s == SEchunk_0To7[i])
                    {
                        //	SDL.Mix_Pause(_index); 
                        // ↑これだと IsPlayingでtrueが戻ってきてしまう。
                        SDL.Mix_HaltChannel(i);
                        // ↑なのでバッファを破棄する必要がある。
                        SEchunk_0To7[i] = null;
                    }
                }
                return YanesdkResult.NoError;
            }

            /// <summary>
            /// 現在再生しているチャンクを停止させる
            /// </summary>
            /// <param name="_filename"></param>
            /// <returns></returns>
            public YanesdkResult Pause(CSoundPlayData・オーディオ再生用クラス s)
            {
                if (s == music)
                {
                    SDL.Mix_PauseMusic(); // Mix_HaltMusic();
                    // バッファは破棄しないなりよ
                }
                if (SEchunk_0To7 == null)
                    return YanesdkResult.PreconditionError;
                for (int i = 0; i < 8; ++i)
                {
                    if (s == SEchunk_0To7[i])
                    {
                        SDL.Mix_Pause(i);
                        // バッファは破棄しないなりよ
                    }
                }
                return YanesdkResult.NoError;
            }

            /// <summary>
            /// 現在一時停止させているチャンクを再生させる
            /// 
            /// 一時停止させている間にVolumeが変更されている可能性があるので
            /// そのへんを考慮して再開。
            /// </summary>
            /// <param name="_filename"></param>
            /// <returns></returns>
            public YanesdkResult Resume(CSoundPlayData・オーディオ再生用クラス s)
            {
                if (s == music)
                {
                    SetVolume(0, s.Volume);
                    SDL.Mix_ResumeMusic();
                }
                if (SEchunk_0To7 == null)
                    return YanesdkResult.PreconditionError;
                for (int i = 0; i < 8; ++i)
                {
                    if (s == SEchunk_0To7[i])
                    {
                        SetVolume(i + 1, s.Volume);
                        SDL.Mix_Resume(i);
                        break;
                    }
                }
                return YanesdkResult.NoError;
            }

            /// <summary>
            /// 現在再生しているチャンクを停止させる
            /// </summary>
            /// <param name="_filename"></param>
            /// <param name="speed"></param>
            /// <returns></returns>
            public YanesdkResult StopFade(CSoundPlayData・オーディオ再生用クラス s, int speed)
            {
                int hr = 0;
                if (s == music)
                {
                    hr = SDL.Mix_FadeOutMusic(speed);
                    music = null;
                }
                if (SEchunk_0To7 == null)
                    return YanesdkResult.PreconditionError;
                for (int i = 0; i < 8; ++i)
                {
                    if (s == SEchunk_0To7[i])
                    {
                        hr = SDL.Mix_FadeOutChannel(i, speed);
                        SEchunk_0To7[i] = null;
                    }
                }
                return hr == 0 ? YanesdkResult.NoError : YanesdkResult.SdlError;
            }

            /// <summary>
            /// すべてのchunkの停止
            /// </summary>
            public void StopAllChunk()
            {
                if (SEchunk_0To7 == null) return;
                for (int i = 0; i < 8; ++i)
                {
                    if (SEchunk_0To7[i] != null) SEchunk_0To7[i].Stop();
                }
            }

            /// <summary>
            /// musicの停止
            /// </summary>
            /// <returns></returns>
            public YanesdkResult StopMusic()
            {
                if (music != null) return music.Stop();
                return YanesdkResult.NoError;
            }

            /// <summary>
            /// マスターvolumeの設定
            /// すべてのSoundクラスに影響する。
            ///		出力volume = (master volumeの値)×(volumeの値)
            /// である。
            /// </summary>
            /// <param name="volume"></param>
            public float MasterVolume
            {
                get { return masterVolume; }
                set
                {
                    float currentMasterVolume = masterVolume;
                    if (masterVolume != value)
                    {
                        masterVolume = value;

                        // 再生中のchunkに影響があるかも知れないので再設定すべき。

                        for (int i = 0; i < 9; ++i)
                            // 現在の値と異なるので設定しなおす
                            innerSetVolume(i, chunkVolumes[i]);

                        if (onMasterVolumeChanged != null)
                            onMasterVolumeChanged();
                    }
                }
            }
            private float masterVolume = 1.0f;

            /// <summary>
            /// マスターVolumeが変更されたときにCallbackされるdelegate
            /// </summary>
            public delegate void OnMasterVolumeChangedDelegate();

            /// <summary>
            /// マスターVolumeが変更されたときにCallbackされるdelegateを登録しておく。
            /// </summary>
            public OnMasterVolumeChangedDelegate OnMasterVolumeChanged
            {
                get { return onMasterVolumeChanged; }
                set { onMasterVolumeChanged = value; }
            }
            private OnMasterVolumeChangedDelegate onMasterVolumeChanged;

            /// <summary>
            /// 特定のchunkのvolumeを取得する
            /// 0.0f～1.0fの間の値で。1.0fは100%。0.0fは無音。
            /// 
            /// _channelNo・チャンネル番号 == 0(music chuck),1～8(通常のchunk)
            /// </summary>
            public float GetVolume(int ch)
            {
                float currentVolume = chunkVolumes[ch];
                return currentVolume;
            }
            /// <summary>
            /// 特定のchunkのvolumeを設定する
            /// 0.0f～1.0fの間の値で。1.0fは100%。0.0fは無音。
            /// 
            /// _channelNo・チャンネル番号 == 0(music chuck),1～8(通常のchunk)
            /// 現在設定されている値と同じならば再設定はしない。
            /// </summary>
            /// <param name="volume"></param>
            public void SetVolume(int ch, float volume)
            {
                if (ch < 0 || 8 < ch)
                    return;

                float currentVolume = chunkVolumes[ch];

                // 現在の音量値と異なるときのみ音量設定
                if (currentVolume != volume)
                {
                    // 現在の値と異なるので設定する
                    innerSetVolume(ch, volume);
                }
            }

            /// <summary>
            /// volume設定(内部的に用いる)
            /// </summary>
            /// <param name="_channelNo・チャンネル番号"></param>
            /// <param name="volume"></param>
            private void innerSetVolume(int ch, float volume)
            {
                // どの値にしたいのか
                float vol = volume * masterVolume;

                if (ch == 0)
                {
                    SDL.Mix_VolumeMusic((int)(vol * SDL.MIX_MAX_VOLUME));
                }
                else
                {
                    SDL.Mix_Volume(ch - 1, (int)(vol * SDL.MIX_MAX_VOLUME));
                }

                // 設定した値を記憶しておく。(master volume掛け算前)
                chunkVolumes[ch] = volume;

                Console.WriteLine("_channelNo・チャンネル番号 {0} = {1} Volume", ch, (int)(vol * SDL.MIX_MAX_VOLUME));
            }

            /// <summary>
            /// 各chunkのvolume値。
            /// indexは、
            ///  0   : music SEchunk_0To7
            ///  1-8 : sound SEchunk_0To7
            /// である。
            /// 
            /// 保存されている値は master volumeを掛け算する前の値。
            /// </summary>
            float[] chunkVolumes;

        }

        #endregion
    }

    /// <summary>
    /// Sound系のclassがICachedObject.Constructを実装するときに使うadaptorクラス、１つのサウンドファイル（オーディオデータ）が項目が定義されているクラスです。
    /// </summary>
    public class CSoundConstructAdaptor・オーディオデータ定義クラス
    {

        /// <summary>
        /// 新しいサウンドを定義します。
        /// とりあえず必須項目として、再生するオーディオデータのファイル名（「***.mp3」など拡張子を含むフルパスでないファイル名）だけ入れてくれればＯＫです。
        /// 　　※その他の項目は、後で必要に応じて後で変更できます。
        /// 
        /// 　　IDは、0にしても自動的に数値が割り振られます。
        /// 　　_audioDataName・参照名は、オーディオデータを参照する時に使用する名前で、空白の場合はファイル名と拡張子だけ（「***.mp3」など）になります。
        /// 　　再生チャンネル番号は、ループしたい場合は-1、それ以外はデフォルト0でＯＫです。
        /// 　　ファイル名フルパスは、""でもちゃんとデータベース内は探しに行き、ファイルが存在すればフルパスを自動的に格納します。
        /// 
        /// 　なお、参照名に「battle1・通常戦闘曲」などのEMusic・曲やESE・効果音で定義された要素抽象的な文字列（全角漢字や半角英数字記号OK）で定義する時、
        /// 　最初の代表的なファイル名以外にも、ランダムなど状況に応じて複数のファイルを再生したい場合は、
        /// _fileNameFullPathの後に、
        /// _fileName1, _fileNameFullPath1, _fileName2, _fileNameFullPath2、....と言う形式でファイル名を指定してください。
        /// </summary>
        /// <param name="_ID"></param>
        /// <param name="_audioDataName・参照名＿空白でもＯＫ＿空白だとファイルの名前と拡張子だけ"></param>
        /// <param name="_fileName0_NotFullPath・ファイルの名前と拡張子だけ＿必ず指定して"></param>
        /// <param name="_channelNo・再生チャンネル番号"></param>
        /// <param name="_fileNameFullPath0・ファイル名フルパス_デフォルトの場所にあれば通常は空白でもＯＫ"></param>
        /// <param name="_Nth_fileNameAndFullPaths・複数のファイル名を再生したい場合＿ファイル名とフルパスを２単語ずつで列挙"></param>
        public CSoundConstructAdaptor・オーディオデータ定義クラス(int _ID, string _audioDataName・参照名＿空白でもＯＫ＿空白だとファイルの名前と拡張子だけ, string _fileName0_NotFullPath・ファイルの名前と拡張子だけ＿必ず指定して, int _channelNo・再生チャンネル番号, string _fileNameFullPath0・ファイル名フルパス_デフォルトの場所にあれば通常は空白でもＯＫ, params string[] _Nth_fileNameAndFullPaths・複数のファイル名を再生したい場合＿ファイル名とフルパスを２単語ずつで列挙)
        {
            this.p_id・サウンドID = s_idAuto・サウンドID自動割り当て番号;
            s_idAuto・サウンドID自動割り当て番号++;
            //this.p_AudioDataName・参照名 = _ID.ToString();
            this.p_AudioDataName・参照名 = _audioDataName・参照名＿空白でもＯＫ＿空白だとファイルの名前と拡張子だけ;
            this.p_fileName・ファイル名 = _fileName0_NotFullPath・ファイルの名前と拡張子だけ＿必ず指定して;
            this.p_channelNo・再生チャンネル番号 = MyTools.adjustValue_From_Min_To_Max(_channelNo・再生チャンネル番号, -1, 8);
            this.p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある = _fileNameFullPath0・ファイル名フルパス_デフォルトの場所にあれば通常は空白でもＯＫ;

            // ■サウンドファイルが存在するかを確かめて、p_fileNameFullPath・フルパスをちゃんとファイルが存在する値に更新
            string _fileName = this.p_fileName・ファイル名;
            string _fullpath = this.p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある;
            string _None = "None"; // 見つからない時の値
            if (_fileName == "" || _fileName == "\"" || _fileName == ".mp3")
            {
                // ファイル名が空白なら仕方がないので、Noneを入れる
                _fileName = _None; // 見つからない時の値
            }
            else
            {
                // ""の場合はデフォルトのフルパスは、ＢＧＭフォルダと効果音フォルダをそれぞれ存在しないか調べ、
                // それでも見つからない場合は、データベースフォルダ以下のサブフォルダを全部を探す
                if (_fullpath == "")
                {
                    _fullpath = Program・実行ファイル管理者.p_BGMDirectory_FullPath・曲フォルダパス + _fileName;
                }
                // 念のため、ディレクトリのパスに修正チェック
                _fullpath = MyTools.getCheckedFilePath(_fullpath);
                if (MyTools.isExist(_fullpath) == true)
                {
                    // ＢＧＭフォルダに存在した
                    //_fullpath = Program・プログラム.p_SoundDatabaseFileName_FullPath・サウンドデータベースファイルパス + _fileName0_FullPath;
                }
                else if (MyTools.isExist(Program・実行ファイル管理者.p_SEDirectory_FullPath・効果音ファルダパス + _fileName) == true)
                {
                    // 効果音フォルダに存在した
                    _fullpath = Program・実行ファイル管理者.p_SEDirectory_FullPath・効果音ファルダパス + _fileName;
                }
                else
                {
                    // それでもなければ、"データベース\\"以下の全てのディレクトリ内を探す
                    _fullpath = MyTools.getFileName_FullPath_InSeachingDirectory(_fileName, 
                        Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス);
                    if (_fullpath != "")
                    {
                        // 整形
                        _fullpath = MyTools.getCheckedFilePath(_fullpath); // 検索でやってるからほんとは別に要らない
                    }
                    else
                    {
                        if (_fileName != "")
                        {
                            Program・実行ファイル管理者.printlnLog(ELogType.l4_重要なデバッグ, "※サウンド: " + _fileName + "は、データベース内には見つかりませんでした。" + Program・実行ファイル管理者.p_DatabaseDirectory_FullPath・データベースフォルダパス);
                        }
                        _fullpath = _None; // 見つからない時の値
                    }
                }
            }
            // ファイル名を更新
            p_fileName・ファイル名 = _fileName;
            // 参照名の更新
            if (_audioDataName・参照名＿空白でもＯＫ＿空白だとファイルの名前と拡張子だけ == "")
            {
                p_AudioDataName・参照名 = _fileName;
            }
            // 見つかった場合、ファイル名をディレクトリをフルパスで格納しておく
            p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある = _fullpath;


        }
        /// <summary>
        /// 新しく CSoundConstructAdaptor・オーディオデータ定義クラス のコンストラクタを呼び出す毎に＋１される、自動生成されるidです。初期値から始まります。
        /// </summary>
        public static int s_idAuto・サウンドID自動割り当て番号 = 90000;
        public int p_id・サウンドID;
        public string p_AudioDataName・参照名;
        public string p_fileName・ファイル名;
        public int p_channelNo・再生チャンネル番号;
        public string p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある;

    }



}

