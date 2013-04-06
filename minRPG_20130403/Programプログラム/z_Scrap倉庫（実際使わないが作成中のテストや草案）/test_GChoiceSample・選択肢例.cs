using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /// <summary>
    /// ※現在はenumを使うことにしています．
    /// 選択肢の典型例（はい，いいえ）や，独自の選択項目を作るときに使用するラッパークラスです．（結果は，GChoice選択肢クラスに格納します．）
    /// </summary>
    public class test_選択肢例＿草案
    {
        test_GChoiceSample・選択肢例＿草案 p_usedClass = new test_GChoiceSample・選択肢例＿草案();
        

    }

    /// <summary>
    /// ※現在はenumを使うことにしています．選択肢の典型例（はい，いいえ）や，独自の選択項目を作るときに使用するクラスです．（結果は，GChoice選択肢クラスに格納します．）
    /// </summary>
    public class test_GChoiceSample・選択肢例＿草案
    {


        int p_selectedSampleID・選択肢例の番号 = 0;
        //[Tips]readonlyは宣言部でしか宣言できないが，constは宣言部に加えてコンストラクタで代入可能．つまり，一度しか初期化しない定数（かつ宣言部で初期化できるもの）はreadonlyが早い．ただし，C++プログラマはconstに慣れている．
        public const int はい = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public test_GChoiceSample・選択肢例＿草案()
        {

        }
    }
}
