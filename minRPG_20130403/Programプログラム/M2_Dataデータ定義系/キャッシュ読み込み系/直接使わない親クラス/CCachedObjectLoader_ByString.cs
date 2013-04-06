using System;
using System.Collections.Generic;
using System.Text;

using Yanesdk.Ytl;
using Yanesdk.System;

// 一部Yanesdkのソースコードを含む．やねうらお様に感謝いたします．

namespace PublicDomain
{
    /// <summary>
    /// CachedObjectを生成するためのFactoryのためのdelegate
    /// </summary>
    /// <returns></returns>
    public delegate ICachedObject CachedObjectFactory();

    /// <summary>
    /// まとまった外部データ（画像，音楽ファイルなど）の名前と格納場所がリストになった
    /// csvファイルをロードし，
    /// それぞれ名前（string型）IDを振って管理する，データロードクラスの基底クラスです．
    /// 
    /// 読み込んだデータは，「ID＿名前」で参照が可能になります(読み込み番号で参照するCachedObjectLoaderとは異なる)．
    /// ※ファイルをロードするときは，重複ロードを避けるため，Smart***Loaderを使うとメモリ節約に繋がります．
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CCachedObjectLoader_ByString : IDisposable
    /*
    where
        T : ICachedObject

     */
    {
        /// <summary>
        /// CachedObjectを生成するためのFactoryのためのdelegate
        /// を設定する機能
        /// </summary>
        /// <returns></returns>
        public CachedObjectFactory Factory
        {
            get { return factory_; }
            set { factory_ = value; }
        }
        protected CachedObjectFactory factory_;
        // 大文字小文字違いだけのものはCLSに準拠しない(´ω`)


        /// <summary>
        /// 格納しているオブジェクト
        /// </summary>
        protected Dictionary<string, ICachedObject> dict・ロード済みオブジェクト = new Dictionary<string, ICachedObject>();


        /// <summary>
        /// 定義ファイルをロードします．
        /// 
        /// 定義ファイルに書いてあるファイル名が実行ファイルからの相対pathなのか、それとも
        /// 定義ファイルの存在するフォルダからの相対pathなのかどうかは、
        /// IsDefRelativePathオプションに従う。
        /// </summary>
        public YanesdkResult LoadDefFile・データベースcsvファイルの読み込みとオブジェクトファイル名群読み込み(string _csvfileName)
        {
            Release();

            basePath = FileSys.GetDirName(_csvfileName);
            List<string> _optionNameList = new List<string>(); // メソッドの引数合わせ，ここでは何もしない
            YanesdkResult result = reader.LoadDefFile・データベースcsvファイルの読み込み(_csvfileName);
            // YanesdkResult result = p_csvreader.LoadDefFile・データベースcsvファイルの読み込み(_csvfilename, OptNum・オプションの数);
            if (result == YanesdkResult.NoError)
            {
                Dictionary<string, CResourceData・資源データ>.KeyCollection
                    keys = reader.Data・資源データ.Keys;
                foreach (string _name・参照名 in keys)
                {
                    // ファイル名は定義ファイル相対pathならそのようにする
                    if (IsDefRelativePath)
                    {
                        reader.Data・資源データ[_name・参照名].p1_name・参照名 =
                            FileSys.MakeFullName(basePath, reader.Data・資源データ[_name・参照名].p1_name・参照名);
                    }

                    // 読み込んだ資源データをキャッシュオブジェクトとして生成し、dict・ロード済みオブジェクトに追加する
                    ICachedObject obj = OnDefFileLoaded(reader.Data・資源データ[_name・参照名]);
                    dict・ロード済みオブジェクト.Add(_name・参照名, obj);
                    //if (_name・参照名 == ESE・効果音._system01・決定音_ピリンッ.ToString())
                    //{
                    //    // デバッグ
                    //    int a = 0;
                    //}

                }
            }
            return result;
        }

        /// <summary>
        /// IsDefRelativePath = true（相対パス）をしたのちに
        /// 設定ファイルを読み込む。
        /// </summary>
        /// <remarks>
        /// 最後のRはRelativeのR
        /// </remarks>
        /// <param name="_fileName_NotFullPath_ファイル名_名前だけ"></param>
        /// <returns></returns>
        public YanesdkResult LoadDefFileR・相対パス読み込み(string filename)
        {
            IsDefRelativePath = true;
            return LoadDefFile・データベースcsvファイルの読み込みとオブジェクトファイル名群読み込み(filename);
        }

        /// <summary>
        /// 定義ファイルが読み込まれたときに実行する。
        /// 派生クラス側でoverrideして使うと良い。
        /// </summary>
        /// <returns></returns>
        protected virtual ICachedObject OnDefFileLoaded(CResourceData・資源データ info)
        {
            return null;
        }

        /// <summary>
        /// 定義ファイルからの相対pathなのか．デフォルトはfalse（絶対パス）です．
        /// </summary>
        public bool IsDefRelativePath
        {
            get { return isDefRelativePath; }
            set { isDefRelativePath = value; }
        }
        private bool isDefRelativePath = false;

        #region OptNumは現在は必要ない
        /*
        /// <summary>
        /// p_csvreader.LoadDefFileでロードするデータベースの中で，
        /// 指定されるオプションの数（ChachedObjectLoaderでいうOptNum）．
        /// つまり，ロードに使用する「名前」と「ファイル名」以外に，管理したい列の数ことです．
        /// 
        /// ***Loaderを作って継承するときは，それぞれ指定する必要があります．
        /// 
        /// ※ これは継承必須
        /// </summary>
        protected virtual int OptNum・オプションの数
        {
            get { return 0; }
        }
         * */
        #endregion

        /// IsDefRelativePathがtrueの場合は、最後に定義ファイルを
        /// LoadDefFile・データベースcsvファイルの読み込み/LoadDefFileRで読み込んだフォルダになる。
        /// 
        /// Loadするときに何も考えずに BasePath + _fileName_NotFullPath_ファイル名_名前だけ　すれば良い。
        /// </summary>
        public string BasePath
        {
            get
            {
                if (IsDefRelativePath)
                    return basePath;
                else
                    return "";
            }
        }
        private string basePath;

        /// <summary>
        /// 定義ファイルの読み込みは、CDatabaseFileReaderに任せておけば良い。
        /// </summary>
        protected CDatabaseFileReader_ReadByString・データベース読み込み機 reader = new CDatabaseFileReader_ReadByString・データベース読み込み機();

        /// <summary>
        /// このクラスで使うコードページを指定する。
        /// 一度設定すると再度設定するまで有効。
        /// ディフォルトでは、Shift_JIS。
        /// BOM付きのutf-16でも読み込める。
        /// </summary>
        public global::System.Text.Encoding CodePage
        {
            get { return reader.CodePage; }
            set { reader.CodePage = value; }
        }

        /*
        protected void LoadHelper(S s, int _name・参照名)
        {
            if (!s.Loaded)
            {
                // 読み込まれてないのん？なんで？解放してしもたん？
                CDatabaseFileReader_ReadByString・データベース読み込み機.CResourceData・資源データ soundItemInfo = p_csvreader.GetInfoLine・参照名に該当する一行の資源データを取得(_name・参照名);
                if (soundItemInfo != null)
                {
                    s.Load・ロード(basePath + soundItemInfo._filename);
                    cache.ChangeCachedObjSize(_name・参照名, s.BufferSize);
                }
            }
        }
         */


        /// <summary>
        /// ●(merusaiaが追加)
        /// 参照名のCacheしているオブジェクトを新しく追加（上書き）するのを手伝う。
        /// 
        /// オブジェクトを取得したときには、Reconstructが自動的に呼び出される。
        /// </summary>
        /// <param name="_name・参照名"></param>
        /// <returns></returns>
        protected void SetCachedObjectHelper・キャッシュオブジェクトの上書き(string _name・参照名, ICachedObject _cachedObject)
        {
            // 既に含まれていたら、過去のものを消す
            if (dict・ロード済みオブジェクト.ContainsKey(_name・参照名))
                dict・ロード済みオブジェクト.Remove(_name・参照名);
            // 新しく追加
            dict・ロード済みオブジェクト.Add(_name・参照名, _cachedObject);
        }
        /// <summary>
        /// 参照名のCacheしているオブジェクトの取得を手伝う。
        /// 
        /// 指定された番号のものがなければnullが戻る。
        /// オブジェクトを取得したときには、Reconstructが自動的に呼び出される。
        /// </summary>
        /// <param name="_name・参照名"></param>
        /// <returns></returns>
        protected ICachedObject GetCachedObjectHelper・キャッシュオブジェクトの取得(string _name・参照名)
        {
            if (!dict・ロード済みオブジェクト.ContainsKey(_name・参照名))
                return null;
            ICachedObject _chachedOjbject = dict・ロード済みオブジェクト[_name・参照名]; // 読み込み時にオブジェクト自体は生成されていると仮定できる
            
            // ●(b)Lostするかどうかに関係なく，再構築
            // _chachedOjbject.Reconstruct();

            // ●(a)ここがLostするかどうかが，YaneSDKで曲が再生できるかどうか！！？　なのだが，不規則にLostしなくなる…．せっかくtは取れているので、とりあえず今はコメントアウトする。
            //if (_chachedOjbject.IsLost) 
            //{ 
            //    _chachedOjbject.Reconstruct(); // 念のためオブジェクトの再構築を行なっておく。
            //}
            
            // アクセスされたものとしてマーカーをつける？
            // ここでつけても各methodがつけないと意味がないか…。
            // _nowTime.CCacheSystem.OnAccess(_nowTime);

            return _chachedOjbject;
        }

        /// <summary>
        /// 確保しているオブジェクト(not resource)をすべて破棄
        /// </summary>
        public void Release()
        {
            Dictionary<string, ICachedObject>.ValueCollection
                values = dict・ロード済みオブジェクト.Values;
            foreach (ICachedObject obj in values)
                obj.Destruct();

            dict・ロード済みオブジェクト.Clear();
        }

        /// <summary>
        /// 確保しているオブジェクトをすべて破棄して終了する。
        /// </summary>
        public void Dispose()
        {
            Release();
        }
    }

}

