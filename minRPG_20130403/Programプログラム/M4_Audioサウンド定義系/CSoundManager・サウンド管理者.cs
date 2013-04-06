using System;
using System.Collections.Generic;
using System.Text;

using Yanesdk.Sound;
using Yanesdk.Ytl; // YanesdkResult

namespace PublicDomain
{

    /// <summary>
    /// 再生可能な曲をすぐに呼び出せるようにしておく曲リストです．
    /// もちろん、game.pBGM()ではここにない曲も直接ファイル名フルパスを与えて再生することもできますが、
    /// ここに置いておくと複数の曲をランダムに再生出来たり、ファイルパスの間違いを減らしたりできるので、あとあと便利かもしれません。
    /// 
    /// 
    /// 　追加するときは，拡張子無しのファイル名（例：「***.mp3」の***の部分だけ）を追加すると，
    /// 　自動的に「データベース」内のサブフォルダを探してファイルのフルパスを取得して再生できます。
    /// 　
    /// 　※それ以外の抽象的な要素を追加した際は，あわせて「サウンドデータベースcsv」ファイルの方も更新してください．
    /// 　でないと_enumItem.ToString()+".mp3"や".wav"や".wma"などで取ったファイルが見つからない場合、音が鳴りません（テスト段階では，デフォルトの音（こうして伝説がはじまった）が鳴るようにしているかもしれません）。
    /// </summary>
    public enum EBGM・曲
    {
        /// <summary>
        /// =0。最初の要素。無音。ＢＧＭを再生しません。（ミュートとは内部的に異なります。ミュートとして使わないでください。ミュートはgame.setBGM_Mute()）
        /// </summary>
        __none・無し,
        /// <summary>
        /// ファイルの読み込みに失敗したとき、何も音が鳴らないとエラー検出に困るので、代わりに鳴らす音です。ファイル名はサウンドデータベース.csvで定義します。  //これはやめた　＿後の再生ファイル名をMyTools.getEnumName_LastIndex() + ".wav"で認識して再生します
        /// </summary>
        _fileNotFound・ファイル読み込み失敗した時に代わりに鳴らす音,
        /// <summary>
        /// EMusic・曲に未定義の曲。game.pBGM(生ファイルのフルパス)などで再生している場合は、この状態に当てはまる。
        /// </summary>
        _unknown・未定義曲,

        // 基本的にはこういう抽象的な記述にして、「サウンドデータベース.csv」複数の曲を書いて、後でそれぞれの再生確率やお気に入り曲などを管理できるようにしたい
        e01_battle1・通常戦闘曲１,
        e02_battle2・通常戦闘曲２,
        e03_boss1・ボス戦闘曲１,
        e04_zako1・ザコ戦闘曲１,

        // 現状はこんな感じで書いてる。ただこれだと１抽象タイプについて、１曲しか書けない（＿で連結すれば書けるが長い）
        //      要素名からファイル名を取るならMyTools.getEnumKeyName_LastIndexToken()+"mp3"で一見いけそうだが、
        //          ファイル名に直接"_"が入っていたり（Shoot_the_thoughtsはそう）、全角全角空白や、二曲以上連結させる時はややこしくなる。
        //      よって、現在はenumからファイル名を取るのは一応試してはいるが推奨していない。代わりに、
        //          「サウンドデータベース.csv」に、これらの参照名とファイル名の対応表を格納している。
        //          そうすれば、曲をゲーム中に変更出来たり、ユーザが後から変更出来るようになる。詳しくは「サウンドデータｂ－エス.csv」を参照。
        battle01・通常戦闘＿Shoot_the_thoughts,
        battle02・通常戦闘＿Wind,
        battleBoss01・ボス戦＿破壊神,
        battleZako01・ザコ戦＿応戦,
        battleBoss03・ボス戦＿Girls_Sword_Rock,
        battleBoss02・ボス戦＿強き者に挑む,
        battleBoss05・ボス戦＿規則からの一脱,
        win01・勝利ファンファーレ＿タラッラララッター,
        win02・勝利ファンファーレ＿やったぜーやったぜー,
        lose01・全滅＿がーん_もう終わりかよ,
        dangyon01・ダンジョン＿露光る木漏れ日,
        dangyon02・ダンジョン＿神殿,
        town01・村＿やさしめの村,
        sports01・ヘルプ＿一からはじめよう,
        god01・神聖＿こうして伝説が始まった,
        // 一応保険的な機能として、拡張し無しのファイル名（***.mp3の）をそのまま書いても、ファイルさえ見つかれば再生できるようにしてある。
        // ただし、ファイル名に空白や記号が入っている場合は不可能な場合があるので、列挙する際は気をつけること。
        キソクからの一脱_新,
        GSR_新,
        //バンド1＿バンドタイトル1,


        // こういう記述もあり。
        random・どれか一つ_ランダム,
        // あとはユーザの好みのプレイリスト
        favorite01・選曲＿お気に入り曲1,
        favorite02・選曲＿お気に入り曲2,
        favorite03・選曲＿お気に入り曲,
        favorite04・選曲＿お気に入り曲4,
        favorite05・選曲＿お気に入り曲5,
        // ユーザが作成／ゲーム中に作曲したオリジナル曲とか
        original01・挿入曲＿オリジナル1,
        original02・挿入曲＿オリジナル2,
        original03・挿入曲＿オリジナル3,
        original04・挿入曲＿オリジナル4,
        original05・挿入曲＿オリジナル5,
        その他,

        // ゲスト名曲集（版権ゲームの曲は、ダウンロード版では消さないといけないかなぁ…＞＜。。）
    }

    /// <summary>
    /// 再生可能な全ての効果音です．
    /// もちろん、game.pSE()ではここにない曲も直接ファイル名フルパスを与えて再生することもできますが、
    /// ここに置いておくと複数の効果音をランダムに再生出来たり、ファイルパスの間違いを減らしたりできるので、あとあと便利かもしれません。
    /// 
    /// 
    /// 　追加するときは，拡張子無しのファイル名（例：「***.wav」の***の部分だけ）を追加すると，
    /// 　自動的に「データベース」内のサブフォルダを探してファイルのフルパスを取得して再生できます。
    /// 　
    /// 　※それ以外の抽象的な要素を追加した際は，あわせて「サウンドデータベースcsv」ファイルの方も更新してください．
    /// 　でないと_enumItem.ToString()+".mp3"や".wav"や".wma"などで取ったファイルが見つからない場合、音が鳴りません。
    /// </summary>
    public enum ESE・効果音
    {
        /// <summary>
        /// =0。最初の要素。無音。効果音を再生しません。（ミュートとは内部的に異なります。ミュートとして使わないでください。ミュートはgame.setSE_Mute()）
        /// </summary>
        __none・無し,
        /// <summary>
        /// ファイルの読み込みに失敗したとき、何も音が鳴らないとエラー検出に困るので、代わりに鳴らす音です。ファイル名はサウンドデータベース.csvで定義します。  //これはやめた　＿後の再生ファイル名をMyTools.getEnumName_LastIndex() + ".wav"で認識して再生します
        /// </summary>
        _fileNotFound・ファイル読み込み失敗した時に代わりに鳴らす音,

        _system01・決定音_ピリンッ,
        _system02・選択音_ピッ,
        _system03・確定音_ピロリーンッ,
        _system04・戻り音_フォョッ,
        _system05・シフト音_フィンッ,
        _system06・制御音_ファンッ,
        _system07・ポーズ音_ピリンッ,
        _system08・ヘルプ音_ピロリロリーンッ,
        _system09・スクリーンショット音_カシャッ,

        attack01a・味方攻撃_ピリリッ,
        attack01b・敵攻撃_ブルルッ,
        attack02a・味方クリティカル_ジャキィーン,
        attack02b・相手クリティカル_グヴァッ,
        attack03a・会心の一撃_シュンシュンシュンッ,
        attack04a・奇跡の一撃_キラリンッードバッシャーーーン,
        attack03b・痛恨の一撃_ジュンジュンジュンッ,
        attack04b・悲劇の一撃_シュゥーーンドバドバドバァーーーン,

        gard01・ガード1_ガキィーン,
        gard02・ガード2_キン,
        gard03・ガード3_カーン,

        avoid01・回避1_シャッ,
        avoid01・回避2_シュパッ,
        avoid01・回避3_パシャーン,
        avoid01・回避4_シュキンッ,


        damade01・ダメージ_ブワッ,
        damege02・大ダメージ_ブルルワッ,
        damege03・特大ダメージ_ティラリーン,

        gun01・銃1_チユーン,

        dice01・ダイス回転音_コロ,
        dice02・ダイス停止音_ピタッ,

        heal01・回復1_ホワン,
        dameyo01・戦闘不能_バタンッ,
        guardPre01・防御準備1_チャッ,
    }

    /// <summary>
    /// ゲーム中の全てのサウンド（BGMや歌曲を含む音楽、効果音、ボイスデータの総称であるオーディオファイルを管理するクラス。
    /// プログラム上の表記はAudio（オーディオ）ですが、わかりやすいように日本語ではサウンドという名前も使います。
    /// 
    /// ※SDLを使用した再生を行う場合は、Update・サウンド再生時間を更新()メソッドを、必ず１フレーム毎に呼び出してください。
    /// </summary>
    public class CSoundManager・サウンド管理者
    {
        #region メモ
        // 現段階の実装は，再生するファイルを直接持たず，サウンドデータベース.csvから適時読みだす。その方がファイルパスの間違いなどが減る。
        // どうしても欲しい場合は、フルパスを与えるバージョンも作ってもＯＫ
        #endregion

        CGameManager・ゲーム管理者 game;

        /// <summary>
        /// サウンドファイルの再生をする読み込み機です．
        /// ※ただし，メモリ削減のため，この読み込み機は2つ以上のサウンドデータの記憶メモリは持ちません．
        /// 　サウンドデータベースを使って読み込み，サウンドデータのＢＧＭは1つ（現在再生中の曲）しか記憶せず，効果音は記憶しません．
        /// </summary>
        CAudioLoader_FromName・オーディオデータ読み込み機 p_audioLoader・オーディオデータ読み込み機 = new CAudioLoader_FromName・オーディオデータ読み込み機();

        private EBGM・曲 p_nowPlayingMusic・現在再生中の曲 = EBGM・曲.__none・無し;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CSoundManager・サウンド管理者(CGameManager・ゲーム管理者 _g)
        {
            game = _g;

            #region メモ
            /*これらは使ってない．
             * 
             * // サウンドデータベース（テキストのcsvデータ，サウンドファイルの曲・効果音名とファイル名が示されたもの）を読み込み
            //p_audioDataBase・サウンドデータベース = MyTools.ReadFile_ToLists(p_SoundDirectory・サウンドデータディレクトリパス暫定版 + "\\サウンドデータベース.csv");
             * 
            // 全てのサウンドファイルをロードしておく？
            CSoundPlayData・オーディオ再生用クラス _bgm = new CSoundPlayData・オーディオ再生用クラス();
            string _csvfilename = "";
            Array _enumArray = Enum.GetValues(EBGM・曲);
            foreach (int _enum in _enumArray)
            {
                while(_bgm.Loaded()==false){
                    MyTime.wait_Movable(10); // 待つ
                }
                p_audioDictionary・サウンド辞書.Add(_enum.ToString(), _bgm);
            }*/
            #endregion
        }

        /// <summary>
        /// サウンドの再生時間を更新する処理です。
        /// 
        /// ※BGM再生にMCIを使っている場合は、ＢＧＭをループさせる処理も含みます。
        /// 
        /// ※SDLを使用した再生を行う場合は、Update・サウンド再生時間を更新()メソッドを、必ず１フレーム毎に呼び出してください。
        /// 
        /// また、
        /// SE再生で1フレームが経過したことを記録するメソッド。メインのスレッドで，必ず毎フレームにつき1回呼び出すこと！
        ///	SE再生後、一定のフレーム数が経過するまでは、そのSEの再生はplaySEを呼び出しても行なわない処理（guard time）も含みます。
        /// </summary>
        public void Update・サウンド再生時間を更新()
        {
            // SE再生で1フレームの経過を記録
            p_audioLoader・オーディオデータ読み込み機.UpdateSE();

            // 今回のチャンク情報を格納
            // 全てのチャンネル0～8（0:BGM、1～5:SE、6～8？:ボイス）に割り当てられている、ファイル名(音量)を表示
            string _chunkInfo = "音再生情報 chunk番号:使用名(音量)→";
            for (int _chunkNo = 1; _chunkNo <= 8; _chunkNo++)
            {
                bool _isEmptyChunk = CSoundPlayData・オーディオ再生用クラス.ChunkManager.GetIsEmptyChunk(_chunkNo);
                if (_isEmptyChunk == false)
                {
                    CSoundPlayData・オーディオ再生用クラス _sound = CSoundPlayData・オーディオ再生用クラス.ChunkManager.GetSound_ByChunkNo(_chunkNo);
                    string _name = "";
                    if(_sound != null) _name = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_sound.FileName);
                    _chunkInfo += _chunkNo + ":"+ _name +"(" + CSoundPlayData・オーディオ再生用クラス.ChunkManager.GetVolume(_chunkNo) + "), ";
                }
                else
                {
                    _chunkInfo += _chunkNo + ":未使用:" + "(" + CSoundPlayData・オーディオ再生用クラス.ChunkManager.GetVolume(_chunkNo) + "), ";
                }
            }
            // 前回のチャンク情報と変化があれば
            bool _isChanged = false;
            if(p_chunkInfo != _chunkInfo) _isChanged = true;
            // ■テスト一時：　約一秒に一回表示
            // if (game.getGamePassedMSecByFrameゲーム経過時間ミリ秒をフレーム更新単位で取得() % 1000 < 10) _isChanged = true;
            if(_isChanged == true){
                // 表示
                game.DEBUGデバッグ一行出力(_chunkInfo);
                // 更新
                p_chunkInfo = _chunkInfo;
            }
        }
        /// <summary>
        /// 前回フレームまでの各chunkチャンネル情報。冒頭説明文の後に、「チャンネル番号:ファイル名／未使用(音量), 」が0～8の9つ繋げられた文章。　（※ファイル名はフルパスではない、「***.wav」など）
        /// </summary>
        public static string p_chunkInfo = "";

        /// <summary>
        /// ＳＥのサウンド音量を変更します。変更前のサウンド音量を返します。
        /// </summary>
        public int setSEVolume_SDL・ＳＥ音量変更(int _volume_0To1000)
        {
            if (_volume_0To1000 < 0) _volume_0To1000 = 0;
            if (_volume_0To1000 > 1000) _volume_0To1000 = 1000;

            int _SE_mainChunkNo = 1;
            int _beforeVolume = (int)(1000 * CSoundPlayData・オーディオ再生用クラス.ChunkManager.GetVolume(_SE_mainChunkNo));
            // チャンネル1～5（SE）の音量を全て変更
            for (int _chunkNo = 1; _chunkNo <= 5; _chunkNo++)
            {
                CSoundPlayData・オーディオ再生用クラス.ChunkManager.SetVolume(_chunkNo, (float)_volume_0To1000 / 1000.0f);
            }
            return _beforeVolume;
        }
        /// <summary>
        /// ＢＧＭのサウンド音量を変更します。変更前のサウンド音量を返します。
        /// </summary>
        public int setBGMVolume_SDL・ＢＧＭ音量変更(int _volume_0To1000)
        {
            if (_volume_0To1000 < 0) _volume_0To1000 = 0;
            if (_volume_0To1000 > 1000) _volume_0To1000 = 1000;

            // (a)これだと新しくＢＧＭを再生したらまた戻っちゃう
            //string _musicName・参照名 = getAudioDataName・参照名を取得(p_nowPlayingMusic・現在再生中の曲);
            //return setVolume_SDL・音量変更(_musicName・参照名, _volume_0To1000);

            // (b)チャンネル0（BGM）の音量を変更
            int _chunkNo = 0;
            int _beforeVolume = (int)(1000 * CSoundPlayData・オーディオ再生用クラス.ChunkManager.GetVolume(_chunkNo));
            CSoundPlayData・オーディオ再生用クラス.ChunkManager.SetVolume(_chunkNo, (float)_volume_0To1000 / 1000.0f);
            return _beforeVolume;
        }
        /// <summary>
        /// 指定した参照名のサウンド音量を変更します。変更前のサウンド音量を返します。
        /// </summary>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ">getAudioDataName・参照名を取得(E***)で取得可能な、参照名</param>
        /// <param name="_volume_0To1000"></param>
        /// <returns></returns>
        public int setVolume_SDL・音量変更(string _name・参照名, int _volume_0To1000)
        {
            if (_volume_0To1000 < 0) _volume_0To1000 = 0;
            if (_volume_0To1000 > 1000) _volume_0To1000 = 1000;
            int _beforeVolume = (int)(1000 * p_audioLoader・オーディオデータ読み込み機.getSound・オーディオ再生用クラスを取得(_name・参照名).Volume);

            // (a)これだと新しくＢＧＭを再生したらまた戻っちゃう
            p_audioLoader・オーディオデータ読み込み機.getSound・オーディオ再生用クラスを取得(_name・参照名).Volume = _volume_0To1000;
            return _beforeVolume;
        }

        /// <summary>
        /// 曲（BGM，歌など）を再生します．
        /// </summary>
        /// <param name="_EMusic・曲"></param>
        public bool playBGM_SDL・曲を再生(EBGM・曲 _EMusic・曲)
        {
            #region メモ
            // 今はSoundLoaderに番号を指定して，txtファイルから自動でファイルを再生する機能を使っているから，個別にファイルをロードしなくていい
            // CSoundPlayData・オーディオ再生用クラス _bgm = new CSoundPlayData・オーディオ再生用クラス();
            // bgm.Load・ロード(p_SoundDirectory・サウンドデータディレクトリパス暫定版 + "\\" + _EMusic・曲, -1); //mp3の場合は必ず第二引数に-1を付ける
            //if (_bgm.Loaded == true)
            //{
            #endregion
            string _musicName・参照名 = getAudioDataName・参照名を取得(_EMusic・曲);
            YanesdkResult _result = p_audioLoader・オーディオデータ読み込み機.PlayBGM(_musicName・参照名);
            if (_result == YanesdkResult.NoError)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 曲（ＢＧＭ・歌など）を停止します。
        /// </summary>
        /// <returns></returns>
        public bool stopBGM_SDL・曲を停止()
        {
            YanesdkResult _result = p_audioLoader・オーディオデータ読み込み機.StopBGM();
            if (_result == YanesdkResult.NoError)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 効果音を再生します．
        /// </summary>
        /// <param name="_ESE・効果音"></param>
        public bool playSE_SDL・効果音を再生(ESE・効果音 _ESE・効果音)
        {
            return playSE_SDL・効果音を再生(_ESE・効果音, 0.0, 0.0, 0.0);
        }
        /// <summary>
        /// 指定した効果音を停止します。
        /// </summary>
        /// <returns></returns>
        public bool stopSE_SDL・効果音を停止(ESE・効果音 _ESE・効果音)
        {
            string _seName・参照名 = getAudioDataName・参照名を取得(_ESE・効果音);
            YanesdkResult _result = p_audioLoader・オーディオデータ読み込み機.Stop(_seName・参照名);
            if (_result == YanesdkResult.NoError) { return true; } else { return false; }
        }
        /// <summary>
        /// 全ての効果音を停止します。
        /// </summary>
        /// <returns></returns>
        public bool stopSE_SDL・効果音を停止()
        {
            YanesdkResult _result = CSoundPlayData・オーディオ再生用クラス.StopAll();
            if (_result == YanesdkResult.NoError) { return true; } else { return false; }
        }
        /// <summary>
        /// 音楽を効果音として再生します．
        /// </summary>
        /// <param name="_ESE・効果音"></param>
        /// <returns></returns>
        public bool playSE_SDL・効果音を再生(EBGM・曲 _EMusic・曲, double _フェードイン秒, double _通常音量再生秒, double _フェードアウト秒)
        {
            string _参照サウンド名 = getAudioDataName・参照名を取得(_EMusic・曲);
            return playSE_SDL・効果音を再生(_参照サウンド名, _フェードイン秒, _通常音量再生秒, _フェードアウト秒);
        }
        /// <summary>
        /// 効果音をフェードイン，もしくはフェードアウトしながら再生します．
        /// </summary>
        /// <param name="_ESE・効果音"></param>
        public bool playSE_SDL・効果音を再生(ESE・効果音 _ESE・効果音, double _フェードイン秒, double _通常音量再生秒, double _フェードアウト秒)
        {
            string _参照サウンド名 = getAudioDataName・参照名を取得(_ESE・効果音);
            return playSE_SDL・効果音を再生(_参照サウンド名, _フェードイン秒, _通常音量再生秒, _フェードアウト秒);
        }
        /// <summary>
        /// 効果音をフェードイン，もしくはフェードアウトしながら再生します．
        /// </summary>
        /// <param name="_ESE・効果音"></param>
        public bool playSE_SDL・効果音を再生(string _参照サウンド名, double _フェードイン秒, double _通常音量再生秒, double _フェードアウト秒)
        {
            YanesdkResult _result;
            if (_フェードイン秒 == 0.0 && _通常音量再生秒 == 0.0 && _フェードアウト秒 == 0.0)
            {
                _result = p_audioLoader・オーディオデータ読み込み機.PlaySE(_参照サウンド名);
            }
            else
            {
                _result = p_audioLoader・オーディオデータ読み込み機.PlaySEFade(_参照サウンド名, (int)_フェードイン秒 * 1000);
            }
            // 通常音量再生秒だけ，待ってからフェードアウト
            // これはスレッドでやらないとできない。
            // (a)Updateメソッドで処理するやりかたを検討中
            // (b)マルチスレッドは使い過ぎはよくないし、呼び出し方がよくわからないのであきらめた・・・MyTools.threadSubMethodRef(threadFadeOut・フェードアウトまでの処理(_参照サウンド名, _通常音量再生秒, _通常音量再生秒));

            return _result == YanesdkResult.NoError ? true : false;
        }
        //private void threadFadeOut・フェードアウトまでの処理(string _参照サウンド名, double _通常音量再生秒, double _フェードアウト秒)
        //{
        //    game.waitウェイト((int)(_通常音量再生秒 * 1000));
        //    p_audioLoader・オーディオデータ読み込み機.StopFade(_参照サウンド名, (int)_フェードアウト秒 * 1000);
        //}

        /// <summary>
        /// 現在再生中の曲を取得します．
        /// </summary>
        /// <returns></returns>
        public EBGM・曲 getNowPlayingMusic・現在再生中の曲を取得(){
            return p_nowPlayingMusic・現在再生中の曲;
            /*Array _array = Enum.GetValues(typeof(EBGM・曲));
            foreach (int _name・参照名 in _array)
            {
                if ( == _name・参照名)
                {
                    return _name・参照名.ToString();
                }
            }*/
        }
        /// <summary>
        /// 現在再生中の曲名を取得します．
        /// </summary>
        /// <returns></returns>
        public string getNowPlayingMusicName・現在再生中の曲を取得()
        {
            return getLabel・ラベルを取得(p_nowPlayingMusic・現在再生中の曲);
        }



        #region AudioDataの各種情報取得: getAudioData他。ファイル名を取得:getFileName_FullPath、ラベル（曲名や効果音擬音語）の取得: getLabel、参照名の取得: getAudioDataName、繰り返しの取得: getIsRepeat
        /// <summary>
        /// 曲を示すEMusic列挙体から、ファイル名をフルパスで取ってきます。ファイルが存在しない場合は""を返します。
        /// </summary>
        /// <param name="_EMusic・曲"></param>
        /// <returns></returns>
        public CSoundConstructAdaptor・オーディオデータ定義クラス getAudioData・各種情報取得(EBGM・曲 _EMusic・曲)
        {
            return getAudioData・各種情報取得(getAudioDataName・参照名を取得(_EMusic・曲), true);
        }
        /// <summary>
        /// 曲を示すEMusic列挙体から、ファイル名をフルパスで取ってきます。ファイルが存在しない場合は""を返します。
        /// </summary>
        /// <param name="_EMusic・曲"></param>
        /// <returns></returns>
        public CSoundConstructAdaptor・オーディオデータ定義クラス getAudioData・各種情報取得(ESE・効果音 _ESE・効果音)
        {
            return getAudioData・各種情報取得(getAudioDataName・参照名を取得(_ESE・効果音), false);
        }
        /// <summary>
        /// 引数に指定した参照名（もしくはファイル名、フルパスでもフルパスでなくてもＯＫ）のオーディオデータ定義クラスを取得します。
        /// 該当する参照名のデータが存在しない場合は、サウンドデーターベースディレクトリ内部に参照名のファイルがないか探し、それでもなければデフォルトのオーディオデータ（音が鳴るもの）を返します。
        /// </summary>
        public CSoundConstructAdaptor・オーディオデータ定義クラス getAudioData・各種情報取得(string _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名)
        {
            return getAudioData・各種情報取得(_AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名, false);
        }
        /// <summary>
        /// 引数に指定した参照名（もしくはファイル名、フルパスでもフルパスでなくてもＯＫ）のオーディオデータ定義クラスを取得します。
        /// 該当する参照名のデータが存在しない場合は、サウンドデーターベースディレクトリ内部に参照名のファイルがないか探し、それでもなければデフォルトのオーディオデータ（音が鳴るもの）を返します。
        /// 
        /// 　※引数３にループ情報を付けておくと、新しくオーディオデータを作成する場合はループ情報が適応されます。
        /// </summary>
        public CSoundConstructAdaptor・オーディオデータ定義クラス getAudioData・各種情報取得(string _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名, bool _isLoop・ファイル名からのデータ新規作成時ループフラグ＿新しくＢＧＭなどを作成するならＴｒｕｅ)
        {
            // 参照名から、ファイルフルパス存在確認済みのオーディオデータを読み込む
            CSoundConstructAdaptor・オーディオデータ定義クラス _audioData = null;
            // 参照名は、EMusicの名前かMSEの名前か、ファイル名「***.mp3」とかしか入っていない（フルパスではない））
            string _name = _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名;
            // でもフルパスを指定されたときのため、一応これを入れておく
            _name = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_name);
            // 各種列挙体に存在している名前であれば、そのまま返す。
            CSoundPlayData・オーディオ再生用クラス _audio =
                p_audioLoader・オーディオデータ読み込み機.getSound・オーディオ再生用クラスを取得(_name);
            if (_audio != null)
            {
                // 正常。列挙体に存在していた。
                _audioData = (CSoundConstructAdaptor・オーディオデータ定義クラス)_audio.constructInfo・オブジェクト再構築データ;
            }
            if (_audioData == null)
            {
                // 列挙体に存在していない、ファイル名の可能性が高い。生でファイルを探す（サブディレクトリも含めて探す）
                _audioData = getAudioData_FromFile・オーディオデータをファイルから生成(
                    _name, _isLoop・ファイル名からのデータ新規作成時ループフラグ＿新しくＢＧＭなどを作成するならＴｒｕｅ);
                // それでもなかったら、フルパスを指定されたかもしれないパスでもう一度探す
                if(_audioData == null){
                    _audioData = getAudioData_FromFile・オーディオデータをファイルから生成(
                        _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名, _isLoop・ファイル名からのデータ新規作成時ループフラグ＿新しくＢＧＭなどを作成するならＴｒｕｅ);
                }
                // それでもなかったら、nullを返して明らめる。
                // 　→　列挙体に存在していない名前か、実在しないファイル名。
            }
            return _audioData;
        }
        /// <summary>
        /// 引数のフルパスじゃないファイル名（「***.wav」など）、もしくはフルパスのファイル名（「C:\\aaa\bbb\***.wav」など）が
        /// データベースディレクトリ内（サブディレクトリも含む）にあるか探し、
        /// あればそのオーディオデータ定義クラスをキャッシュに追加して返し、なければデフォルトのものを返します。
        /// </summary>
        /// <param name="_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ"></param>
        /// <param name="_isLoop・ファイル名からのデータ新規作成時ループフラグ＿新しくＢＧＭなどを作成するならＴｒｕｅ"></param>
        /// <returns></returns>
        private CSoundConstructAdaptor・オーディオデータ定義クラス getAudioData_FromFile・オーディオデータをファイルから生成(string _fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ, bool _isLoop・ループするか)
        {
            CSoundConstructAdaptor・オーディオデータ定義クラス _audioData;
            int _ChannelNo = 0;
            if (_isLoop・ループするか == true) _ChannelNo = -1;
            // フルパスじゃないファイル名　＝　参照名
            string _name・参照名 = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ);

            // まず、フルパスと仮定して、ファイルが存在するかどうか確かめる
            if (MyTools.isExist(_fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ) == true)
            {
                // フルパスなので、オーディオデータにフルパス情報を追加して作成
                _audioData = new CSoundConstructAdaptor・オーディオデータ定義クラス(CSoundConstructAdaptor・オーディオデータ定義クラス.s_idAuto・サウンドID自動割り当て番号,
                    "", _name・参照名, _ChannelNo, _fileName・ファイル名＿フルパスでもフルパスじゃなくてもＯＫ);
            }else{
                // フルパスじゃないので、オーディオデータにフルパス情報を追加しないで自動的に検索させて作成
                _audioData = new CSoundConstructAdaptor・オーディオデータ定義クラス(CSoundConstructAdaptor・オーディオデータ定義クラス.s_idAuto・サウンドID自動割り当て番号,
                    "", _name・参照名, _ChannelNo, "");
            }
            // 生ファイルが見つかったから、次からは探さなくていいようにキャッシュに追加
            CSoundPlayData・オーディオ再生用クラス _newSound = new CSoundPlayData・オーディオ再生用クラス(_ChannelNo);
            _newSound.constructInfo・オブジェクト再構築データ = _audioData;
            setAudioData・該当参照名のオーディオデータを更新(_name・参照名, _newSound);
            // ファイルが実在したか
            if (_audioData.p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある == "None")
            {
                // 生ファイルも見つからなかった場合。
                // ■（暫定テスト）音ならないと失敗したかわからないから、とりあえず、曲が取得できない時に流す曲を取得
                _audioData = CSoundPlayData・オーディオ再生用クラス.getDefaultAudioData_fileNotFound・デフォルトのオーディオデータを取得();
                //_audioData = new CSoundConstructAdaptor・オーディオデータ定義クラス(0, "Girls_Sword_Rock", "GSR.mp3", -1, "");
                // _audioData = (CSoundConstructAdaptor・オーディオデータ定義クラス)_audio.constructInfo・オブジェクト再構築データ;
            }
            return _audioData;
        }
        /// <summary>
        /// 引数１の参照名に指定されたオーディオデータを追加します。既に同じ参照名のデータがキャッシュに存在する時は上書きします。
        /// </summary>
        public void setAudioData・該当参照名のオーディオデータを更新(string _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名, CSoundPlayData・オーディオ再生用クラス _newData)
        {
            p_audioLoader・オーディオデータ読み込み機.setSound・サウンドを上書き(_AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名, _newData);
        }
        /// <summary>
        /// 実際のファイル名（フルパスではない）またはEMusic・曲やESE・効果音の要素名から、ファイル名をフルパスで取ってきます。ファイルが存在しない場合は"None"を返します。
        /// </summary>
        public string getFileName_FullPath・ファイル名を取得(string _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名)
        {
            string _fileName_FullPath = getAudioData・各種情報取得(_AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かフルパスじゃないファイル名).p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある;
            //これはファイルを読み込むときに確認しているのでいらないif(MyTools.isExist(_fileName0_FullPath)==false) _fileName0_FullPath = "";
            return _fileName_FullPath;
        }
        /// <summary>
        /// 曲を示すEMusic列挙体から、ファイル名をフルパスで取ってきます。ファイルが存在しない場合は""を返します。
        /// </summary>
        /// <param name="_EMusic・曲"></param>
        /// <returns></returns>
        public string getFileName_FullPath・ファイル名を取得(EBGM・曲 _EMusic・曲)
        {
            string _fileName_FullPath = getAudioData・各種情報取得(_EMusic・曲).p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある;
            //これはファイルを読み込むときに確認しているのでいらないif(MyTools.isExist(_fileName0_FullPath)==false) _fileName0_FullPath = "";
            return _fileName_FullPath;
        }
        /// <summary>
        /// 曲を示すESE列挙体から、ファイル名をフルパスで取ってきます。ファイルが存在しない場合は""を返します。
        /// </summary>
        public string getFileName_FullPath・ファイル名を取得(ESE・効果音 _ESE・効果音)
        {
            string _fileName_FullPath = getAudioData・各種情報取得(_ESE・効果音).p_fileNameFullPath・ファイルの存在を確かめたフルパス_不存在はNoneと書いてある;
            //これはファイルを読み込むときに確認しているのでいらないif(MyTools.isExist(_fileName0_FullPath)==false) _fileName0_FullPath = "";
            return _fileName_FullPath;
        }
        /// <summary>
        /// 実際のファイル名（フルパスでなくてもフルパスでもどちらでもいい）またはEMusic・曲やESE・効果音の要素名から、サウンド情報を視覚的に認識可能な、ラベルを取得します。
        /// 基本的には曲ファイル名の拡張子を抜いた文字列「曲名」や、効果音ファイル名の"_"や"＿"や"・"で区切られた最後「ピコーン」などの擬音語が格納されるようになっています。
        /// </summary>
        public string getLabelBGM・ラベルを取得(string _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かファイル名)
        {
            string _name = _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かファイル名;
            string _fileName_FullPath = MyTools.getFileLeftOfPeriodName(  MyTools.getFileName_NotFullPath_LastFileOrDirectory( getFileName_FullPath・ファイル名を取得(_name) )  );
            //これはファイルを読み込むときに確認しているのでいらないif(MyTools.isExist(_fileName0_FullPath)==false) _fileName0_FullPath = "";
            return _fileName_FullPath;
        }
        /// <summary>
        /// 実際のファイル名（フルパスでなくてもフルパスでもどちらでもいい）またはEMusic・曲やESE・効果音の要素名から、サウンド情報を視覚的に認識可能な、ラベルを取得します。
        /// 基本的には曲ファイル名の拡張子を抜いた文字列「曲名」や、効果音ファイル名の"_"や"＿"や"・"で区切られた最後「ピコーン」などの擬音語が格納されるようになっています。
        /// </summary>
        public string getLabelSE・ラベルを取得(string _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かファイル名)
        {
            string _name = _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かファイル名;
            string _fileName_FullPath = MyTools.getFileLeftOfPeriodName(MyTools.getName_OnlyLastIndexWord(  MyTools.getFileName_NotFullPath_LastFileOrDirectory( getFileName_FullPath・ファイル名を取得(_name) )  ));
            //これはファイルを読み込むときに確認しているのでいらないif(MyTools.isExist(_fileName0_FullPath)==false) _fileName0_FullPath = "";
            return _fileName_FullPath;
        }
        /// <summary>
        /// 曲の情報を視覚的に認識可能な、ラベルを取得します。
        /// 基本的には曲ファイル名の拡張子を抜いた文字列「曲名」や、効果音ファイル名の"_"や"＿"や"・"で区切られた最後「ピコーン」などの擬音語が格納されるようになっています。
        /// </summary>
        public string getLabel・ラベルを取得(EBGM・曲 _EMusic・曲)
        {
            string _MusicLabel・曲ラベル = MyTools.getFileLeftOfPeriodName(  MyTools.getFileName_NotFullPath_LastFileOrDirectory( getFileName_FullPath・ファイル名を取得(_EMusic・曲) )  );
            return _MusicLabel・曲ラベル;
        }
        /// <summary>
        /// 効果音の情報を視覚的に認識可能な、ラベルを取得します。
        /// 基本的には曲ファイル名の拡張子を抜いた文字列「曲名」や、効果音ファイル名の"_"や"＿"や"・"で区切られた最後「ピコーン」などの擬音語が格納されるようになっています。
        /// </summary>
        public string getLabel・ラベルを取得(ESE・効果音 _ESE・効果音)
        {
            string _SELabel・効果音ラベル = MyTools.getFileLeftOfPeriodName(MyTools.getName_OnlyLastIndexWord(    MyTools.getFileName_NotFullPath_LastFileOrDirectory( getFileName_FullPath・ファイル名を取得(_ESE・効果音) ) )  );
            return _SELabel・効果音ラベル;
        }
        /// <summary>
        /// ファイル名（フルパス）またはEMusic・曲やESE・効果音の要素名から、オーディオデータ情報をデータベースで検索可能な、参照名を取得します。
        /// 現実装では、「フルパスではないファイル名（***.wavなど）」または「要素名そのまま」が参照名として返ります。
        /// </summary>
        public string getAudioDataName・参照名を取得(string _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かファイル名)
        {
            string _name = _AudioDataName・参照名＿ＥＭｕｓｉｃ名かＭＳＥ名かファイル名;
            // 各種列挙体に存在している名前であれば、そのまま返す。
            if (MyTools.getEnumItem_FromString<EBGM・曲>(_name) != EBGM・曲.__none・無し
                || MyTools.getEnumItem_FromString<ESE・効果音>(_name) != ESE・効果音.__none・無し)
            {
                return _name;
            }
            // でなければ、ファイル名と判断して、フルパスでないファイル名を返す。
            string _AudioDataName・参照名 = MyTools.getFileName_NotFullPath_LastFileOrDirectory(_name);
            return _AudioDataName・参照名;
        }
        /// <summary>
        /// 曲の情報を曲データベースで検索可能な、参照名を取得します。現実装では、_enumItem.ToString()そのままです。
        /// </summary>
        public string getAudioDataName・参照名を取得(EBGM・曲 _EMusic・曲)
        {
            string _SEName・参照名 = MyTools.getEnumName(_EMusic・曲); // _enumItem.ToString()そのまま
            return _SEName・参照名;
        }
        /// <summary>
        /// 効果音の情報を効果音データベースで検索可能な、参照名を取得します。現実装では、_enumItem.ToString()そのままです。
        /// </summary>
        public string getAudioDataName・参照名を取得(ESE・効果音 _ESE・効果音)
        {
            string _MusicName・参照名 = MyTools.getEnumName(_ESE・効果音); // _enumItem.ToString()そのまま
            return _MusicName・参照名;
        }
        // リピート情報はstring型は作ってない（ファイル名毎にリピート情報を決めるのではなく、要素毎に決めるから）
        /// <summary>
        /// この曲は繰り返し（リピート）再生するかどうかを取得します。
        /// </summary>
        public bool getIsRepeat・繰り返し情報を取得(EBGM・曲 _EMusic・曲)
        {
            bool _isRepeat = false; // 参照名が見つからない場合、デフォルトはfalse
            CSoundPlayData・オーディオ再生用クラス _audio = 
                p_audioLoader・オーディオデータ読み込み機.getSound・オーディオ再生用クラスを取得(
                getAudioDataName・参照名を取得(_EMusic・曲));
            if (_audio != null)
            {
                // ループ情報を取得
                int _LoopNo_minus1 = _audio.Loop;
                if (_LoopNo_minus1 == -1) _isRepeat = true;
            }
            return _isRepeat;
        }
        /// <summary>
        /// この効果音は繰り返し（リピート）再生するかどうかを取得します。
        /// </summary>
        public bool getIsRepeat・繰り返し情報を取得(ESE・効果音 _ESE・効果音)
        {
            bool _isRepeat = true; // デフォルトはtrue
            CSoundPlayData・オーディオ再生用クラス _audio = p_audioLoader・オーディオデータ読み込み機.getSound・オーディオ再生用クラスを取得(getAudioDataName・参照名を取得(_ESE・効果音));
            if (_audio != null)
            {
                // ループ情報を取得
                int _LoopNo_minus1 = _audio.Loop;
                if (_LoopNo_minus1 != -1) _isRepeat = false;
            }
            return _isRepeat;
        }
        #endregion

    }
}
