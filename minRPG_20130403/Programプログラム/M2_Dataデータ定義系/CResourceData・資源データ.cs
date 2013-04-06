using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// データベースcsvファイルに書いてある，1行分の資源データを格納するクラスです．
    /// 
    /// ======例)資源データの例==================================================
    ///  【ID, 参照名,      ファイル名, オプション1, オプション2, オプション3, ...】
    ///		1,  村人A, 村人A顔グラ.jpg,        本名,          力,      防御力, ...
    /// =========================================================================
    /// </summary>
    public class CResourceData・資源データ
    {
        public int p0_id・ID = 0;
        public string p1_name・参照名 = "";
        public string p2_fileName・ファイル名 = "";
        /// <summary>
        /// 資源データの0列～最後の列を格納している変数です。
        /// </summary>
        public List<string> p3to_option・列リスト = new List<string>();

        /// <summary>
        /// コンストラクタで，この資源データのID（いちおうの管理番号），参照名（検索名），資源を示すメインのファイル名，その他のオプション項目であるオプション文字列リストを定義します．
        /// 
        /// ======例)資源データの例==================================================
        ///  【         ID,     参照名,   ファイル名, ...】
        ///  【オプション0, オプション1, オプション2, オプション3, オプション4, オプション5, ...】
        ///		         1,       村人A, 村人A顔.jpg,        本名,          力,      防御力, ...
        /// =========================================================================
        /// </summary>
        /// <param name="_資源のID"></param>
        /// <param name="_資源の参照名"></param>
        /// <param name="_資源のファイル名"></param>
        /// <param name="_オプション文字列リスト"></param>
        public CResourceData・資源データ(int _資源のID, string _資源の参照名, string _資源のファイル名, string[] _オプション文字列リスト)
        {
            this.p0_id・ID = _資源のID;
            this.p2_fileName・ファイル名 = _資源のファイル名;
            this.p1_name・参照名 = _資源の参照名;
            this.p3to_option・列リスト.AddRange(_オプション文字列リスト); // [Warning]リストの代入はAddRange!，return以外では，ローカル変数は=じゃ渡せないので注意！
        }
        /// <summary>
        /// コンストラクタで，この資源データのID（いちおうの管理番号），参照名（検索名），資源を示すメインのファイル名，その他のオプション項目であるオプション文字列リストを定義します．
        /// 
        /// ======例)資源データの例==================================================
        ///  【         ID,     参照名,   ファイル名, ...】
        ///  【オプション0, オプション1, オプション2, オプション3, オプション4, オプション5, ...】
        ///		         1,       村人A, 村人A顔.jpg,        本名,          力,      防御力, ...
        /// =========================================================================
        /// </summary>
        /// <param name="_資源のID"></param>
        /// <param name="_資源の参照名"></param>
        /// <param name="_資源のファイル名"></param>
        /// <param name="_オプション文字列リスト"></param>
        public CResourceData・資源データ(int _資源のID, string _資源の参照名, string _資源のファイル名, List<string> _オプション文字列リスト)
        {
            this.p0_id・ID = _資源のID;
            this.p2_fileName・ファイル名 = _資源のファイル名;
            this.p1_name・参照名 = _資源の参照名;
            this.p3to_option・列リスト.AddRange(_オプション文字列リスト); // [Warning]リストの代入はAddRange!，return以外では，ローカル変数は=じゃ渡せないので注意！
        }
        /// <summary>
        /// コンストラクタで，この資源データのID（いちおうの管理番号），参照名（検索名），キャラの画像を示す画像ファイル名，その他のキャラクタを定義して生成します．重たい処理になるので、あまり頻繁に呼び出さない方がいいかもです。
        /// </summary>
        /// <param name="_資源のID"></param>
        /// <param name="_キャラ名"></param>
        /// <param name="_資源のファイル名"></param>
        /// <param name="_chara"></param>
        public CResourceData・資源データ(int _資源のID, string _キャラ名, string _キャラの画像ファイル名, CChara・キャラ _c){
            this.p0_id・ID = _資源のID;
            this.p1_name・参照名 = _キャラ名;
            this.p2_fileName・ファイル名 = _キャラの画像ファイル名;

            // ★こういう個所がもう一個存在します。「オプション最大インデックス」で検索してもう一つも修正してね。
            // 名前置き換え
            List<string> _オプション = this.p3to_option・列リスト;
            _オプション.Clear();

            int _開始列 = 0;
            _オプション.Add(p0_id・ID.ToString());
            _オプション.Add(p1_name・参照名);
            _オプション.Add(p2_fileName・ファイル名);
            _オプション.Add(_c.Var(EVar.本名));
            _オプション.Add(_c.Var(EVar.ふりがな)); // ふりがな
            _オプション.Add(_c.syougo称号()); // 称号
            _オプション.Add(_c.Var(EVar.性別)); // 性別
            _オプション.Add(""); // 空白

            _開始列 = 7;
            //double _a = _chara.Para(EPara.LV1c01_赤);
            _オプション.Add(_c.Para(EPara.LV1c01_赤).ToString());
            _オプション.Add(_c.Para(EPara.LV1c03_橙).ToString());
            _オプション.Add(_c.Para(EPara.LV1c05_黄).ToString());
            _オプション.Add(_c.Para(EPara.LV1c07_緑).ToString());
            _オプション.Add(_c.Para(EPara.LV1c09_青).ToString());
            _オプション.Add(_c.Para(EPara.LV1c11_紫).ToString());
            _オプション.Add(_c.Para(EPara.LV1c_基本6色総合値).ToString());
            
            _開始列 = 7 + 6 + 1;
            _オプション.Add(_c.Para(EPara.LV1c02_赤橙).ToString());
            _オプション.Add(_c.Para(EPara.LV1c04_黄橙).ToString());
            _オプション.Add(_c.Para(EPara.LV1c06_黄緑).ToString());
            _オプション.Add(_c.Para(EPara.LV1c08_青緑).ToString());
            _オプション.Add(_c.Para(EPara.LV1c10_青紫).ToString());
            _オプション.Add(_c.Para(EPara.LV1c12_赤紫).ToString());
            _オプション.Add(_c.Para(EPara.LV1c_中間6色総合値).ToString());

            _開始列 = 7 + 6 + 1 + 6 + 1;
            _オプション.Add(_c.Para(EPara.LV1c13_白).ToString());
            _オプション.Add(_c.Para(EPara.LV1c14_黒).ToString());
            _オプション.Add(_c.Para(EPara.LV1c15_銀).ToString());
            _オプション.Add(_c.Para(EPara.LV1c16_金).ToString());
            _オプション.Add(_c.Para(EPara.LV1c17_透明).ToString());
            _オプション.Add(_c.Para(EPara.LV1c18_虹色).ToString());
            _オプション.Add(_c.Para(EPara.LV1c_装飾6色総合値).ToString());

            _開始列 = 7 + 6 + 1 + 6 + 1 + 6 + 1;
            _オプション.Add(_c.Var(EVar.血液型).ToString());
            _オプション.Add(_c.Var(EVar.生年月日).ToString());

            _開始列 = 7 + 6 + 1 + 6 + 1 + 6 + 1 + 2;
            _オプション.Add(_c.Var(EVar.年齢).ToString());
            _オプション.Add(_c.Var(EVar.身長).ToString());
            _オプション.Add(_c.Var(EVar.体重).ToString());
            _オプション.Add(_c.Var(EVar.メイン武器).ToString());
            _オプション.Add(_c.Var(EVar.通り名).ToString());
            _オプション.Add(_c.Var(EVar.登場セリフ).ToString());
            _オプション.Add(_c.Var(EVar.個性を司る象徴語).ToString());
            _オプション.Add(_c.Var(EVar.光と闇の転換プロセス).ToString());
            _オプション.Add(_c.Var(EVar.ひっさつ).ToString());
            _オプション.Add(_c.Var(EVar.ひっさつ効果).ToString());
            _オプション.Add(_c.Var(EVar.声優さん).ToString());

            // レベルセーブ
            _開始列 = 7 + 6 + 1 + 6 + 1 + 6 + 1 + 2 + 11 + 1;
            _オプション.Add("");
            _オプション.Add(_c.Para(EPara.LV).ToString());
            // ここと、の配列をいっしょにしましょう

            // データベースの項目が足りない時にエラーにならないように、補完
            double _オプション最大インデックス = 7 + 6 + 1 + 6 + 1 + 6 + 1 + 2 + 11 + 1 + 1;
            if (_オプション.Count - 1 != _オプション最大インデックス)
            {
                Program・実行ファイル管理者.printlnLog(ELogType.l5_エラーダイアログ表示, "すみません、プログラムの間違いで、ちゃんとセーブされていないかもしれません・・・。\n【エラー】：　キャラクタデータベースの項目数（" + _オプション.Count + "個）が、正確な項目数（" + (_オプション最大インデックス + 1) + "個）に合っていません。\n");
            }
        }
        /// <summary>
        /// 資源データを「,」で区切ったCSV形式の文字列を返します
        /// </summary>
        /// <returns></returns>
        public string getCSVLineString()
        {
            List<string> _csvList = new List<string>();
            _csvList.AddRange(p3to_option・列リスト);
            string _line = MyTools.getCSVLineString(_csvList);
            return _line;
        }
        /// <summary>
        /// 一度getCharaData・キャラデータに変換して取得()メソッドで作った元のキャラデータです。
        /// メモリ節約の場合は、常にnullになります。
        /// </summary>
        private CChara・キャラ p_charaData = null;
        private bool p_isLowMemorryMode・メモリ節約モード = true;
        /// <summary>
        /// ■キャラクタデータを格納し，簡単に呼び出せるようにしたキャラクラスに変換して返します．２回目以降は高速に呼び出せるようにしています。
        /// </summary>
        /// <returns></returns>
        public CChara・キャラ getCharaData・キャラデータに変換して取得()
        {
            CChara・キャラ _c = new CChara・キャラ();
            if (p_charaData == null)
            {
                // 初めてキャラデータを作成

                // 置き換え
                List<string> _オプション = this.p3to_option・列リスト;
                // データベースの項目が足りない時にエラーにならないように、補完。
                // ★こういう個所がもう一個存在します。「オプション最大インデックス」で検索してもう一つも修正してね。
                double _オプション最大インデックス = 7 + 6 + 1 + 6 + 1 + 6 + 1 + 2 + 11 + 1 + 1;
                if (_オプション.Count - 1 < _オプション最大インデックス)
                {
                    while (_オプション.Count - 1 < _オプション最大インデックス)
                    {
                        _オプション.Add("");
                    }
                }

                int _開始列 = 0;
                _c.setVar・変数を変更(EVar.id, _オプション[_開始列 + 0]);
                _c.setVar・変数を変更(EVar.名前, this.p1_name・参照名); // _オプション[_開始列 + 1]
                _c.setName・名前を一時的に変更(this.p1_name・参照名); // _オプション[_開始列 + 2]
                //_chara.setFaceImage・顔画像を変更(this.p_fileName・ファイル名);]
                _c.setVar・変数を変更(EVar.本名, _オプション[_開始列 + 3]); // 1?
                _c.setVar・変数を変更(EVar.ふりがな, _オプション[_開始列 + 4]);
                _c.setVar・変数を変更(EVar.称号, _オプション[_開始列 + 5]); // [MEMO]今は称号は複数含む。「、」も含む
                //_chara.setVar・変数を変更(EVar.称号, MyTools.getStringItem(_オプション[_開始列 + 5], "、", 1)); // [MEMO]今は一つ目だけを表示している
                _c.setVar・変数を変更(EVar.性別, _オプション[_開始列 + 6]);

                _開始列 = 7;
                // 色パラメータを設定。setIroParas***()メソッドを呼び出した方がいいかも？
                List<double> _iroPara18s = new List<double>(new double[]{
                    MyTools.parseDouble(_オプション[_開始列 + 1]),
                    MyTools.parseDouble(_オプション[_開始列 + 2]),
                    MyTools.parseDouble(_オプション[_開始列 + 3]),
                    MyTools.parseDouble(_オプション[_開始列 + 4]),
                    MyTools.parseDouble(_オプション[_開始列 + 5]),
                    MyTools.parseDouble(_オプション[_開始列 + 6]),
                    MyTools.parseDouble(_オプション[_開始列 + 7]),
                    MyTools.parseDouble(_オプション[_開始列 + 8]),
                    MyTools.parseDouble(_オプション[_開始列 + 9]),
                    MyTools.parseDouble(_オプション[_開始列 + 10]),
                    MyTools.parseDouble(_オプション[_開始列 + 11]),
                    MyTools.parseDouble(_オプション[_開始列 + 12]),
                    MyTools.parseDouble(_オプション[_開始列 + 13]),
                    MyTools.parseDouble(_オプション[_開始列 + 14]),
                    MyTools.parseDouble(_オプション[_開始列 + 15]),
                    MyTools.parseDouble(_オプション[_開始列 + 16]),
                    MyTools.parseDouble(_オプション[_開始列 + 17]),
                    MyTools.parseDouble(_オプション[_開始列 + 18])
                });
                _c.Paras・パラメータ一括処理().setIroParas色パラメータを代入(_iroPara18s);
                //_chara.setParaValue(EPara.c01_赤, MyTools.parseDouble(_オプション[_開始列 + 1]));
                //_chara.setParaValue(EPara.c02_橙, MyTools.parseDouble(_オプション[_開始列 + 2]));
                //_chara.setParaValue(EPara.c03_黄, MyTools.parseDouble(_オプション[_開始列 + 3]));
                //_chara.setParaValue(EPara.c04_緑, MyTools.parseDouble(_オプション[_開始列 + 4]));
                //_chara.setParaValue(EPara.c05_青, MyTools.parseDouble(_オプション[_開始列 + 5]));
                //_chara.setParaValue(EPara.c06_紫, MyTools.parseDouble(_オプション[_開始列 + 6]));

                //_開始列 = 7 + 6 + 1;
                //_chara.setParaValue(EPara.c07_赤橙, MyTools.parseDouble(_オプション[_開始列 + 1]));
                //_chara.setParaValue(EPara.c08_黄橙, MyTools.parseDouble(_オプション[_開始列 + 2]));
                //_chara.setParaValue(EPara.c09_黄緑, MyTools.parseDouble(_オプション[_開始列 + 3]));
                //_chara.setParaValue(EPara.c10_青緑, MyTools.parseDouble(_オプション[_開始列 + 4]));
                //_chara.setParaValue(EPara.c11_青紫, MyTools.parseDouble(_オプション[_開始列 + 5]));
                //_chara.setParaValue(EPara.c12_赤紫, MyTools.parseDouble(_オプション[_開始列 + 6]));

                //_開始列 = 7 + 6 + 1 + 6 + 1;
                //_chara.setParaValue(EPara.c13_白, MyTools.parseDouble(_オプション[_開始列 + 1]));
                //_chara.setParaValue(EPara.c14_黒, MyTools.parseDouble(_オプション[_開始列 + 2]));
                //_chara.setParaValue(EPara.c15_銀, MyTools.parseDouble(_オプション[_開始列 + 3]));
                //_chara.setParaValue(EPara.c16_金, MyTools.parseDouble(_オプション[_開始列 + 4]));
                //_chara.setParaValue(EPara.c17_透明, MyTools.parseDouble(_オプション[_開始列 + 5]));
                //_chara.setParaValue(EPara.c18_虹色, MyTools.parseDouble(_オプション[_開始列 + 6]));

                _開始列 = 7 + 6 + 1 + 6 + 1 + 6 + 1;
                _c.setVar・変数を変更(EVar.血液型, _オプション[_開始列 + 1]);
                //_chara.setVar・変数を変更(EVar.生年月日, _オプション[_開始列 + 2]);

                _開始列 = 7 + 6 + 1 + 6 + 1 + 6 + 1 + 2;
                _c.setVar・変数を変更(EVar.年齢, _オプション[_開始列 + 1]);
                _c.setVar・変数を変更(EVar.身長, _オプション[_開始列 + 2]);
                _c.setVar・変数を変更(EVar.体重, _オプション[_開始列 + 3]);
                _c.setVar・変数を変更(EVar.メイン武器, _オプション[_開始列 + 4]);
                _c.setVar・変数を変更(EVar.通り名, _オプション[_開始列 + 5]);
                _c.setVar・変数を変更(EVar.登場セリフ, _オプション[_開始列 + 6]);
                _c.setVar・変数を変更(EVar.個性を司る象徴語, _オプション[_開始列 + 7]);
                _c.setVar・変数を変更(EVar.光と闇の転換プロセス, _オプション[_開始列 + 8]);
                _c.setVar・変数を変更(EVar.ひっさつ, _オプション[_開始列 + 9]);
                _c.setVar・変数を変更(EVar.ひっさつ効果, _オプション[_開始列 + 10]);
                _c.setVar・変数を変更(EVar.声優さん, _オプション[_開始列 + 11]);

                // レベル
                _開始列 = 7 + 6 + 1 + 6 + 1 + 6 + 1 + 2 + 11 + 1;
                _c.setParaValue(EPara.LV, Math.Max(1.0, MyTools.parseDouble(_オプション[_開始列 + 1])));

                // キャラデータに格納（２回目はこれを返すだけでいいようにする）、でもコピーしないと参照先を変更されるから、コピーを渡す
                if (p_isLowMemorryMode・メモリ節約モード == false)
                {
                    p_charaData = _c;
                }
            }
            else
            {
                _c = p_charaData;
            }

            return MyTools.DeepCopy<CChara・キャラ>(_c);
        }
    }
}
