using System;
using System.Collections.Generic;
using System.Text;
using Yanesdk.Ytl;

// 一部にyanesdkのソースを含む．やねうらお様に感謝いたします．

namespace PublicDomain
{
    /// <summary>
    /// mp3やbmpなど，ファイルを参照名で自動的に読み込むクラスです．
    /// ※CDatabaseFileReader_ReadByString・データベース読み込み機とは，別の機能を持つクラスです．要整理．
    /// 
    /// ※スマートクラスは，CCachedObjectLoader_ByStringの親玉。
    /// つまり，ファイルをロードするときに，このクラスのLoadDefFileで呼び出せば，
    /// ***Loader_ByStringが同じファイルを重複ロードしていたとしても，
    /// 必ず1つだけのロードで済むようにしてくれる．（結果的に，メモリを節約可能）
    /// （デザインパターンでいうと，CachedObjectLoaderのfactoryクラス）
    /// 
    /// T : CachedObjectLoader派生クラスを渡す
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class CCachedObjectSmartLoader_ByString・オブジェクト読み込み機<T> :
        IDisposable
            where
        T : CCachedObjectLoader_ByString, new()
    {
        /// <summary>
        /// 定義ファイルの読み込み。
        /// 
        /// ここで生成したCachedObjectLoaderのDisposeを呼び出さないこと。
        /// (同時に同じ定義ファイルをLoadDefFileしているCachedObjectLoaderのインスタンスと
        /// 同一インスタンスなので、そちら側で困ったことになる)
        /// </summary>
        /// <param name="_defFile・オブジェクトファイルパス_フルパス"></param>
        /// <returns></returns>
        public T LoadDefFile・データベースcsvファイルの読み込み(string _defFile・オブジェクトファイルパス_フルパス)
        {
            // 念のため、一意な名前にしておく。
            // ex. "./def.txt"と"def.txt"が別のファイル名扱いされると
            // Loaderが別になってしまうので。
            string defFileName = Yanesdk.System.FileSys.MakeFullName("", _defFile・オブジェクトファイルパス_フルパス);

            if (maps・ファイル名とローダーの辞書.ContainsKey(defFileName))
                return maps・ファイル名とローダーの辞書[defFileName];

            T loader = new T();
            loader.Factory = this.Factory;

            OnLoadDefFile(loader);

            loader.IsDefRelativePath = this.isDefRelativePath;
            loader.LoadDefFile・データベースcsvファイルの読み込みとオブジェクトファイル名群読み込み(_defFile・オブジェクトファイルパス_フルパス); // このresultがどうなってようがそれは知らん

            maps・ファイル名とローダーの辞書.Add(defFileName, loader);
            return loader;
        }

        /// <summary>
        /// LoadDefFileのIsDefRelativePathの設定をかねる版。
        /// IsDefRelativePathにはこの値は反映されない。
        /// </summary>
        /// <param name="_fileName_NotFullPath_ファイル名_名前だけ"></param>
        /// <param name="isDefRelativePath"></param>
        /// <returns></returns>
        public T LoadDefFile・データベースcsvファイルの読み込み(string filename, bool isDefRelativePath)
        {
            bool b = this.isDefRelativePath;

            this.isDefRelativePath = isDefRelativePath;
            T loader = LoadDefFile・データベースcsvファイルの読み込み(filename);

            this.isDefRelativePath = b;

            return loader;
        }

        /// <summary>
        /// LoadDefFileでT型をnewした直後に何らかの処理を入れたいときは、
        /// これをoverrideして。
        /// </summary>
        /// <param name="loader"></param>
        protected virtual void OnLoadDefFile(T loader) { }

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
        /// LoadDefFileで定義ファイルの置いてあったフォルダからの相対pathで
        /// ファイルを読み込むのか
        /// 
        /// これがfalseの場合、実行ファイルからの相対path。
        /// (default = false)
        /// 
        /// これを設定しておけば、LoadDefFileでは T(ほにゃららLoader)の
        /// IsDefRelativePathを、ここで設定した値にしたものが返る。
        /// </summary>
        public bool IsDefRelativePath
        {
            get { return isDefRelativePath; }
            set { isDefRelativePath = value; }
        }
        private bool isDefRelativePath;

        /// <summary>
        /// 定義file名から実体ファイルへのmap．これによって重複ロードを防ぎます．
        /// </summary>
        protected Dictionary<string, T> maps・ファイル名とローダーの辞書 = new Dictionary<string, T>();

        /// <summary>
        /// こいつから生成したものはすべて破棄
        /// こいつが生成したloaderすべてのDispose()も呼び出す。
        /// 
        /// ゲームの規模が大きい場合、シーン間の移動などに際してすべての画像を解放して良いとされる
        /// 状況ならば、そのタイミングでこのDisposeを呼び出したほうが良いだろう。
        /// </summary>
        public void Dispose()
        {
            foreach (CCachedObjectLoader_ByString loader in maps・ファイル名とローダーの辞書.Values)
                loader.Dispose();
        }
    }
}
