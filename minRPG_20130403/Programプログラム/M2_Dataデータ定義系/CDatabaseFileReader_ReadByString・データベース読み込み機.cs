
using System.Collections.Generic;
using Yanesdk.System;
using Yanesdk.Ytl;
using System;

// 一部にYanesdkのソースを含む．やねうらお様に感謝いたします．

namespace PublicDomain
{

    /// <summary>
    /// 資源データ（サウンド，画像など）のID・名前・ファイル名がリスト化されているデータベースcsvファイルを読み込む専用のクラスです．CSVRoaderを使います．
    /// </summary>
    public class CDatabaseFileReader_ReadByString・データベース読み込み機
    {
        #region データベースの読み込み処理
        /// <summary>
        /// データベースcsvファイルの読み込みをします．
        /// 
        /// <Para>
        ///	・データベースcsvファイル仕様
        /// ==================================================================================
        ///		ID, 名前(実際に表示される名前), ファイル名（相対パス） , option1, option2, option3, ...
        /// ===================================================================================
        ///	    の繰り返し。option1,2,3...は省略可能。略した場合、""扱いになる。
        /// </Para>
        /// <Para>
        ///		以下はコメント
        ///
        ///		ここで指定されているoptionが、どのような意味を持つかは、
        ///		このCacheLoaderクラスを使用するクラス(派生クラス？)で定義するので，そこに依存する。
        /// </Para>
        /// <Para>
        ///		その他、細かい解析ルールは、 CSVReader クラスの内容に依存する。
        ///	</Para>
        /// 
        /// <remarks>
        /// ●●※optNumは現在は必要ない
        /// 以下，古いメモ
        /// ============================
        /// optNum : optionの書いてある限度位置(列)を指定する。
        /// 上の例では、「_onelineString.wav , 2」なので、optNum = 0(なし)と考えることが出来る。
        /// 「_onelineString,wav , opt1, opt2 , 2」のようにopt1,opt2を指定するならば、
        /// optNum = 2と考えることが出来る。
        /// 「_onelineString,wav , opt1, 2 , opt2」のようにopt1,opt2を指定するならば、
        /// optNum = 1と考えることが出来る。
        /// 
        /// </remarks>
        /// </summary>
        public YanesdkResult LoadDefFile・データベースcsvファイルの読み込み(string _csvfilename)
        // オプション名のラベルは別に使わないのであれば必須では無い public YanesdkResult LoadDefFile・データベースcsvファイルの読み込み(string _csvfilename, List<string> _オプション名リスト)
        {
            int _t1 = MyTools.getNowTime_fast();

            Release・読み込み内容をリセット();

            // 読み込んだCSVから、ファイル名と番号とを対応させ、
            // LinkedListに格納していく。
            // (a)YanesdkのCSVReaderを使う場合
            //YanesdkResult result = p_csvreader.Read(_csvfilename);
            //if (result != YanesdkResult.NoError)
            //    return result;
            //List<List<string>> _csvData = p_csvreader.CsvData;
            // (b)MyTools.ReadCSVFileを使う場合
            List<List<string>> _csvData = MyTools.ReadFile_ToCSV(_csvfilename);
            if (_csvData == null) return YanesdkResult.FileReadError;

            int _line = 0;

            int _rowMaxNum = 0;
            int _index = 0;
            int _id = 0;
            string _name = "";
            string _fileName = "";
            string[] _options; //リスト List<string> _options = new List<string>();
            // 1行ずつ解析
            foreach (List<string> _onelineString in _csvData)
            {
                // 一行目はラベルなので飛ばす
                if (_line == 0)
                {
                    _line++;
                    continue;
                }

                // 1個ずつ解析
                _index = 0;
                _id = 0;
                _name = "";
                _fileName = "";
                _options = new string[_onelineString.Count];//_options.Clear();
                foreach (string _item in _onelineString)
                {
                    // (i)配列、早さ優先 ※(ii)と比べても0.80倍くらいしか早くならなかった…
                    // 項目の格納
                    _options[_index] = _item;
                    // その他の情報の格納
                    if (_index == 0) // ID
                    {
                        if (int.TryParse(_item, out _id) == false)
                        {
                            _id = 0; // _id=0:数値に変換できない時のID
                        }
                    }
                    else if (_index == 1) { _name = _item; } // 参照名
                    else if (_index == 2) { _fileName = _item; } // ファイル名
                    _index++;
                    //// (ii)リスト、見やすさ優先
                    //switch (_index)
                    //{
                    //    case 0: // ID
                    //        if (int.TryParse(_item_includingWaitWord, out _userID) == false)
                    //        {
                    //            _userID = 0; // _id=0:数値に変換できない時のID
                    //        }
                    //        _options[_index] = _item_includingWaitWord; ;//_options.Add(_item_includingWaitWord);
                    //        break;
                    //    case 1: // 参照名
                    //        _name = _item_includingWaitWord;
                    //        _options.Add(_item_includingWaitWord);
                    //        break;
                    //    case 2: // ファイル名
                    //        _fileName0_FullPath = _item_includingWaitWord;
                    //        _options.Add(_item_includingWaitWord);
                    //        break;
                    //    default: // index=2以降はオプション
                    //        _options.Add(_item_includingWaitWord);
                    //        break;
                    //}
                    //_index++;
                }
                _rowMaxNum = Math.Max(_index, _rowMaxNum);

                // 資源データとして格納
//                try
//                {
                    if (_name != "" && p_data・資源データ.ContainsKey(_name)==false)
                    {
                        p_data・資源データ.Add(_name, new CResourceData・資源データ(_id, _name, _fileName, _options));
                    }
//                }
//                catch
//                {
//                    // 同一の値で定義されている行によるエラーなのだが..これをtrycatchするだけでかなり時間をロスする
//                    // この行スキップしたろか
//#if DEBUG
//                    //global::System.Console.WriteLine("CDatabaseFileReader_ReadByString・データベース読み込み機.loadDefFileでエラー行検出。　同じ参照名"+_name+"がデータベースに２個以上定義されている可能性があります。");
//#endif
//                }

                ++_line;
            }
            p_rowNum = _rowMaxNum;

            int _t2 = MyTools.getNowTime_fast();
            MyTools.ConsoleWriteLine("★「"+MyTools.getFileName_NotFullPath_LastFileOrDirectory(_csvfilename)+"」のデータベース読み込み完了。 "+((_t2-_t1)/1000.0)+"秒 かかっちゃいました。");

            return YanesdkResult.NoError;
        }

        /// <summary>
        /// 解放処理。読み込んでいた定義ファイルの内容をリセットする。
        /// </summary>
        public void Release・読み込み内容をリセット()
        {
            p_data・資源データ.Clear();
        }/// <summary>
        /// 解放処理。読み込んでいた定義ファイルの内容をリセットする。
        /// </summary>
        public void Clear・読み込み内容をリセット＿Releaseと全く同じ()
        {
            Release・読み込み内容をリセット();
        }
        #endregion

        /// <summary>
        /// 各行の参照名（行データを示す名前）のリストです．
        /// </summary>
        private List<string> p_nameList・参照名 = null;//p_data・資源データ_.Keys
        /// <summary>
        /// ●定義ファイルから読み込んだデータの，全ての参照名リスト（インデックス1の値）を返します．
        /// 
        /// 例)定義ファイルが
        ///		1, 曲a, 戦闘開始.wav
        ///		2, 曲b, フィールド.ogg
        ///		3, 曲c, タイトル画面.mp3
        /// と書いてあって、これをGetNameListで読み込んだあとならば、
        /// {曲a,曲b,曲c} が返る。
        /// </summary>
        /// <param name="_keycode"></param>
        /// <returns></returns>
        public List<string> GetNameList・参照名リストを取得()
        {
            List<string> _nameLists;
            if (p_nameList・参照名 == null)
            {
                p_nameList・参照名 = new List<string>(); //p_data・資源データ_.Keys;
                foreach (KeyValuePair<string, CResourceData・資源データ> _data in p_data・資源データ)
                {
                    p_nameList・参照名.Add(_data.Value.p1_name・参照名);
                }
            }
            _nameLists = new List<string>();
            _nameLists.AddRange(p_nameList・参照名);
            return _nameLists;
            // return p_nameList・参照名; // プロパティのそのまま代入は危険！　呼び出し元で値を変更されてしまう！
        }

        /// <summary>
        /// 各行の値のリストを，列毎に格納したリストです．
        /// </summary>
        private List<string>[] p_rowList・列値リスト = null;
        private int p_rowNum = 0;
        /// <summary>
        /// ●指定列の全ての行のリストを取得します．
        /// 
        /// 例)定義ファイルが
        ///index0, 1, 2 
        /// ============================
        ///		1, 曲a, 戦闘開始.wav
        ///		2, 曲b, フィールド.ogg
        ///		3, 曲c, タイトル画面.mp3
        /// と書いてあって、これをGetRowList(2)で読み込んだあとならば、
        /// {戦闘開始.wav, フィールド.ogg, タイトル画面.mp3} が返る。
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        public List<string> GetRowList・列インデックスに該当する列値リストを取得(int _index)
        {
            // 初期化
            if(p_rowList・列値リスト == null){
                if(p_rowNum != 0){
                    p_rowList・列値リスト = new List<string>[p_rowNum];
                }else{
                    p_rowList・列値リスト = new List<string>[100];
                }
            }
            List<string> _optionList;
            // 指定列の列値リストを過去に作っていなかったら，作る
            if (MyTools.getArrayValue<List<string>>(p_rowList・列値リスト, _index) == null)
            {
                _optionList = new List<string>();
                if (_index == 0)
                {
                    foreach (KeyValuePair<string, CResourceData・資源データ> _data in p_data・資源データ)
                    {
                        _optionList.Add(_data.Value.p0_id・ID.ToString());
                    }
                }
                else if (_index == 1)
                {
                    _optionList = GetNameList・参照名リストを取得();
                }
                else if (_index == 2)
                {
                    foreach (KeyValuePair<string, CResourceData・資源データ> _data in p_data・資源データ)
                    {
                        _optionList.Add(_data.Value.p2_fileName・ファイル名);
                    }
                }
                else if (_index > 2)
                {
                    foreach (KeyValuePair<string, CResourceData・資源データ> _data in p_data・資源データ)
                    {
                        _optionList.Add(MyTools.getListValue(_data.Value.p3to_option・列リスト, _index));
                    }
                }
                else
                {
                }
                // 一度作った列値リストはすぐに呼び出せるように，格納
                p_rowList・列値リスト[_index] = _optionList;
            }else{
                // 過去に作った列値を呼び出す
                _optionList = p_rowList・列値リスト[_index];
            }
            return _optionList;
            
        }

        /// <summary>
        /// 定義ファイルから読み込んだデータのなかから、
        /// この名前に対応するファイル名を返す
        /// 
        /// 例)定義ファイルが
        ///		1, 曲a, 戦闘開始.wav
        ///		2, 曲b, フィールド.ogg
        ///		3, 曲c, タイトル画面.mp3
        /// と書いてあって、これをreadDefFileで読み込んだあとならば、
        /// GetFileName("曲a")とすれば"戦闘開始.wav"が返る。
        /// 定義ファイル上に該当行が見つからない場合は、getはnullを返す。
        /// </summary>
        /// <param name="_keycode"></param>
        /// <returns></returns>
        public string GetFileName・参照名に該当するファイル名を取得(string key)
        {
            if (!p_data・資源データ.ContainsKey(key))
                return null;
            return p_data・資源データ[key].p1_name・参照名;
        }

        /// <summary>
        /// ●定義ファイルから読みこんだ，指定した参照名の（一行）資源データを取得する．無い場合はnullが返る．
        /// 
        /// 例)定義ファイルが
        ///		1, 曲a, 戦闘開始.wav
        ///		2, 曲b, フィールド.ogg
        ///		3, 曲c, タイトル画面.mp3
        /// と書いてあって、これをreadDefFileで読み込んだあとならば、
        /// GetFileName("曲a")とすれば 資源データ {1, 曲a, 戦闘開始.wav} が返る。
        /// </summary>
        /// <remarks>
        /// opt1,opt2が設定ファイル上で省略されていた場合には、
        /// int.MinValueが返る。
        /// </remarks>
        /// <param name="_参照名"></param>
        /// <returns></returns>
        public CResourceData・資源データ GetInfoLine・参照名に該当する一行の資源データを取得(string _参照名)
        {
            if (!p_data・資源データ.ContainsKey(_参照名))
                return null;
            return p_data・資源データ[_参照名];
        }
        /// <summary>
        /// ●定義ファイルから読みこんだ，指定した参照名で，かつ指定列の（一語）資源データを取得する．無い場合は、""が返る．
        /// 
        /// 例)定義ファイルが
        /// index0, 1, 2 
        /// ============================
        ///		1, 曲a, 戦闘開始.wav
        ///		2, 曲b, フィールド.ogg
        ///		3, 曲c, タイトル画面.mp3
        /// と書いてあって、これをreadDefFileで読み込んだあとならば、
        /// GetFileName("曲a", 2)とすれば {戦闘開始.wav} が返る。
        /// </summary>
        /// <remarks>
        /// opt1,opt2が設定ファイル上で省略されていた場合には、
        /// int.MinValueが返る。
        /// </remarks>
        /// <param name="_keycode"></param>
        /// <returns></returns>
        public string GetInfoWord・参照名に該当する一語のデータを取得(string _参照名, int _列index)
        {
            string _word = "";
            if (!p_data・資源データ.ContainsKey(_参照名))
                return _word;

            CResourceData・資源データ _line = p_data・資源データ[_参照名];
            if (_列index == 0)
                {
                    _word = _line.p0_id・ID.ToString();
                }
                else if (_列index == 1)
                {
                    _word = _line.p1_name・参照名;
                }
                else if (_列index == 2)
                {
                    _word = _line.p2_fileName・ファイル名;
                }
                else if (_列index > 2)
                {
                    _word = MyTools.getListValue(_line.p3to_option・列リスト, _列index);
                }
            return _word;
        }


        /// <summary>
        /// 読み込んだ資源データのプロパティ．
        /// ※外部クラスからプロパティで呼べるようにするためだけに作られたプロパティです．
        /// 　中身は別のプロパティが持つため，外部クラスからの変更はできません．
        /// 
        /// キー：　参照名(string形式)  : "ID＿名前"（例：ID=12, 名前="主人公横顔画像"だと，「ID＿主人公横画像」）
        /// 
        /// 例)定義ファイルが，
        ///index0, 1, 2, 3, 4 
        /// ======================
        ///		1, 曲a, 戦闘開始.wav    ,0 , メルサイア
        ///		2, 曲b, フィールド.ogg  ,-1, SPLECT
        ///		3, 曲c, タイトル画面.mp3,5 , 匿名
        /// ======================
        /// の場合，一行目は・・・，
        /// ====
        /// ID=option[0]=12，
        /// 参照名=option[1]=曲a，
        /// ファイル名=option[2]=戦闘開始.wav，
        /// option[3]=0
        /// option[4]=メルサイア
        /// ====
        /// となる．
        /// </summary>
        public Dictionary<string, CResourceData・資源データ> Data・資源データ
        {
            get { return p_data・資源データ; }
        }
        /// <summary>
        /// ●●●読み込んだ資源データの中身．
        /// ※内部でしか触ってはダメです．クラス内部ではこっちを変更してください．
        /// 
        /// キー：　参照名(string形式)  : "ID＿名前"
        /// （例：ID=12, 名前="主人公横顔画像"だと，「ID＿主人公横画像」）
        /// </summary>
        private Dictionary<string, CResourceData・資源データ> p_data・資源データ = new Dictionary<string, CResourceData・資源データ>();

        /// <summary>
        /// このクラスで使うコードページを指定する。
        /// 一度設定すると再度設定するまで有効。
        /// ディフォルトでは、Shift_JIS。
        /// BOM付きのutf-16でも読み込める。
        /// </summary>
        public global::System.Text.Encoding CodePage
        {
            get { return p_csvreader.CodePage; }
            set { p_csvreader.CodePage = value; }
        }
        private CSVReader p_csvreader = new CSVReader();


        /// <summary>
        /// 引数１の○○データベース.csvファイルに、該当の資源データを上書き保存します。
        /// </summary>
        /// <param name="_csvFileName"></param>
        /// <param name="_newData"></param>
        public void saveData(string _csvFileName, CResourceData・資源データ _newData)
        {
           MyTools.WriteFile_Append(_csvFileName, _newData.getCSVLineString());
        }
    }

}