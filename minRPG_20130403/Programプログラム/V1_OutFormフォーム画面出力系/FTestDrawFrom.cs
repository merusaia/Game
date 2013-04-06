using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;


using System.Windows.Forms;

namespace PublicDomain
{
    public partial class FDrawForm : Form
    {
        public FDrawForm()
        {
            InitializeComponent();
        }




        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        // お絵かきコード。ここから抜粋 http://social.msdn.microsoft.com/Forums/ja-JP/csharpexpressja/thread/9825e388-63b2-4a8b-a74f-d272b329545d/

        private bool p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = true;
        private bool p_isTrueDrawingSassa_FalseDrawFree・Trueサッサ描き専用か＿False普通のお絵かきか = false;

        private bool _mouseDown;
        private int _drawStartMousePosX;
        private int _drawStartMousePosY;
        private int _drawStartPosX;
        private int _drawStartPosY;

        // グラフのXY軸の最小値・最大値
        private int p_graphYMin;
        private int p_graphYMax;
        private int p_graphXMin;
        private int p_graphXMax;
        // グラフスクリーンの幅・高さ
        private int p_graphScreenWidth;
        private int p_graphScreenHeight;


        /// <summary>
        /// Undoに使うXdataとYdataの履歴
        /// </summary>
        private List<List<int>> _HistoryXdata_UsingUndo;
        /// <summary>
        /// Undoに使うXdataとYdataの履歴
        /// </summary>
        private List<List<int>> _HistoryYdata_UsingUndo;
        private int _HistoryNowIndex = 0;
        /// <summary>
        /// ドロー中に検出したグラフX座標の配列
        /// </summary>
        private List<int> _drawingUpdatedPosXdata;
        /// <summary>
        /// ドロー中に検出したグラフY座標の配列
        /// </summary>
        private List<int> _drawingUndatedPosYdata;

        /// <summary>
        /// 飛び飛びなグラフの縦軸Xの配列
        /// </summary>
        private List<int> _Xdata;
        /// <summary>
        /// 飛び飛びなグラフの縦軸Yの配列
        /// </summary>
        private List<int> _Ydata;
        //private int _XYdataNum = 0; // Xdata.CountとYdata.Countといっしょ。２つはいつも同時に追加・削除しているからいらない
        /// <summary>
        /// Imageに描画し終えたXdataの配列（フリーハンドお絵かき／サッサ描きのときのみ有効。サッサ描きのときは要素がか変なので削除された分だけ減らされる可能性がある。グラフ描画のときは使わない）
        /// </summary>
        private int _lastDrawnXdataIndex = 0;

        /// <summary>
        /// ドラッグを離した時（やデータロード時）に一気に作成される、詰まってるグラフの縦軸Xの配列
        /// </summary>
        private List<int> _XAlldata;
        /// <summary>
        /// ドラッグを離した時（やデータロード時）に一気に作成される、詰まってるグラフの縦軸Yの配列
        /// </summary>
        private List<int> _YAlldata;

        private Rectangle _lastRect;

        /// <summary>
        /// pictureBox1に描画される、イメージ。（reDrawで画面に更新されます）
        /// </summary>
        private Image p_pictureNowImage;
        /// <summary>
        /// 過去に描画されたイメージの履歴（Undo等に使用）
        /// </summary>
        private List<Image> _HistoryImage;
        /// <summary>
        /// グラフの背景イメージ。方眼紙
        /// </summary>
        private Image p_graphBackImage;

        /// <summary>
        /// button1をクリックしたときのモードや機能の列挙体。モードはbuttonCangeを押すなどして変更します。
        /// </summary>
        private enum ETestButtonMode
        {
            無し,
            フリーハンドお絵描きモード,
            フリーハンドサッサ描きモード,
            円描画モード,
            四角形描画モード,
            線描画モード,
            グラフのフリーハンド描画モード,
            グラフの点データ読み込み,
            グラフの出力,
            Count,
        }
        private int p_buttonMode; // モード番号
        public bool p_isTestGraphMode・グラフをフリーハンドで描いて値を出力するモード = true;
        //public bool p_isTestGraphLoad・グラフ読み込みモード = false;
        //public bool p_isTestGraphSave・グラフ出力モード = false;


        private Point parseToGraphPoint・カーソル座標からグラフ座標への変換(int _x, int _y)
        {
            return MyTools.getGraphPointPosition_ByMouseCursorPosition(_x, _y, p_graphXMin, p_graphXMax, p_graphYMin, p_graphYMax, p_graphScreenWidth, p_graphScreenHeight);
        }
        private Point parseToGraphPoint・カーソル座標からグラフ座標への変換(Point _position)
        {
            return MyTools.getGraphPointPosition_ByMouseCursorPosition(_position, p_graphXMin, p_graphXMax, p_graphYMin, p_graphYMax, p_graphScreenWidth, p_graphScreenHeight);
        }

        private Point parseToCursorPoint・グラフ座標からカーソル座標への変換(int _x, int _y)
        {
            return MyTools.getMouseCursorPosition_ByGraphPointPosition(_x, _y, p_graphXMin, p_graphXMax, p_graphYMin, p_graphYMax, p_graphScreenWidth, p_graphScreenHeight);
        }
        private Point parseToCursorPoint・グラフ座標からカーソル座標への変換(Point _position)
        {
            return MyTools.getMouseCursorPosition_ByGraphPointPosition(_position, p_graphXMin, p_graphXMax, p_graphYMin, p_graphYMax, p_graphScreenWidth, p_graphScreenHeight);
        }

        private void showPosition・座標をラベルに表示()
        {
            // 現在のマウス座標を取得
            Point _posiMouse1 = MyTools.getMouseCursorPosition_ByControl(this, pictureBox1);
            //int _x1 = _posiMouse1.X; //_e.X;
            //int _y1 = _posiMouse1.Y; //_e.Y;
            // カーソル用の座標：左上が(0,0)　→　グラフデータ用の座標：左下端が（0,0）　の変換
            Point _posiGraph1 = parseToGraphPoint・カーソル座標からグラフ座標への変換(_posiMouse1);
            int _x1 = _posiGraph1.X;
            int _y1 = _posiGraph1.Y;
            // 座標を表示
            label1.Text = "マウス座標(" + MyTools.getStringNumber(_posiMouse1.X, true, 4, 0) + ", " + MyTools.getStringNumber(_posiMouse1.Y, true, 4, 0) + ")"
                + "　　グラフ座標(" + MyTools.getStringNumber(_x1, true, 6, 0) + ", " + MyTools.getStringNumber(_y1, true, 6, 0) + ")"
                + "　　Undo可能な残り履歴数:" + _HistoryNowIndex;
        }


        /// <summary>
        /// 描画モード毎の機能を実行します。
        /// </summary>
        private void doButtonMode・描画機能を実行()
        {
            if (p_buttonMode == (int)ETestButtonMode.グラフのフリーハンド描画モード)
            {
                drawImage・XYdataに従ってフリーハンド画像を描画();
            }
            else if (p_buttonMode == (int)ETestButtonMode.グラフの出力)
            {
                string _message = "";
                List<int> _data = _YAlldata; //_Ydata;
                for (int i = 0; i < _data.Count; i++)
                {
                    //_message += _Xdata[i] + "," + _Ydata[i] + ","; 
                    _message += _data[i] + ",";
                }
                // 最後の「,」を消す
                _message = _message.Substring(0, Math.Max(0, _message.Length - 1));
                MyTools.showInputBox("下記のテキストがグラフの高さYを羅列したグラフのデータです。コピーしてint[]型の配列やファイルに保存して使ってください。", "グラフデータの出力", _message);
            }
            else if (p_buttonMode == (int)ETestButtonMode.グラフの点データ読み込み)
            {
                string _inputString = MyTools.showInputBox("以下の入力ボックスにX=0,1,2..,の時の高さYを「,」区切りで羅列したグラフのデータを入力してください。ＯＫを押すとグラフに出力します", "グラフデータの読み込み", "（例：1,5,10,50,100,...）");
                string _graphName = "";
                List<int> _YloadData = MyTools.getListValues_FromCSVLine("グラフデータ,\n" + _inputString, false, out _graphName);
                List<int> _XloadData = new List<int>();
                for (int i = 0; i < _YloadData.Count; i++)
                {
                    _XloadData.Add(i);
                }
                // これだと２種類以上のグラフを読みこんだら、一方が消えちゃう。とりあえず読み込んだグラフを修正するには、これでいい
                clearXYData・フリーハンドデータをクリア＿Ｕｎｄｏには残る();
                _Xdata = _XloadData;
                _Ydata = _YloadData;
                drawImage・XYdataに従ってフリーハンド画像を描画();
                
                // お絵かきモードで描画
                showGraph・引数のグラフをお絵かきモードで描画(_YloadData.ToArray(), MyColor.getRainbowRandomColor・呼び出す毎に色が変わるランダムな虹色に近い色());
            }
            else if (p_buttonMode == (int)ETestButtonMode.フリーハンドお絵描きモード)
            {
                button1.Enabled = false;
                drawImage・XYdataに従ってフリーハンド画像を描画(10);
                button1.Enabled = true;
            }
            else if (p_buttonMode == (int)ETestButtonMode.フリーハンドサッサ描きモード)
            {
                button1.Enabled = false;
                drawImage・XYdataに従ってフリーハンド画像を描画(10);
                button1.Enabled = true;
            }
        }
        /// <summary>
        ///  引数のグラフを、指定した色で、お絵かきモードで描画します（複数回呼び出してもグラフは残ります）。描画成功したらtrue、描画できないエラーが起こった際はfalseを返します。
        /// </summary>
        private bool showGraph・引数のグラフをお絵かきモードで描画(double[] _graphYs, Color _graphColor)
        {
            int[] _ints = new int[_graphYs.Length];
            for (int i = 0; i < _graphYs.Length; i++)
            {
                _ints[i] = (int)_graphYs[i];
            }
            return showGraph・引数のグラフをお絵かきモードで描画(_ints, _graphColor);
        }
        /// <summary>
        /// 引数のグラフを、指定した色で、お絵かきモードで描画します（複数回呼び出してもグラフは残ります）。描画成功したらtrue、描画できないエラーが起こった際はfalseを返します。
        /// </summary>
        private bool showGraph・引数のグラフをお絵かきモードで描画(int[] _graphYs, Color _graphColor)
        {
            bool _isSuccess = false;
            if (_graphYs == null || _graphYs.Length < 1 || _graphColor == null) return _isSuccess;
            _isSuccess = true;
            if (MyTools.isErrorImage(p_pictureNowImage) == true)
            {
                clearScreenImage・画面だけをクリア＿データは消えない();
            }
            MyTools.drawLines_Graph(_graphYs, ref p_pictureNowImage, _graphColor, 
                MyTools.EGraphPositionType.t1_Graph00isTopLeftPosition_グラフ座標＿左下端が００で＿y軸が上に行くとプラス);
            reDraw・描画領域を再描画();
            return _isSuccess;
        }

        /// <summary>
        /// 描画モードを変更します。
        /// </summary>
        private void changeButtonMode・描画モードを変更(int _buttonMode)
        {
            p_isTestGraphMode・グラフをフリーハンドで描いて値を出力するモード = false;
            if (_buttonMode == (int)ETestButtonMode.グラフのフリーハンド描画モード)
            {
                p_isTestGraphMode・グラフをフリーハンドで描いて値を出力するモード = true;
                p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = true;
            }
            else if (_buttonMode == (int)ETestButtonMode.グラフの出力)
            {
                p_isTestGraphMode・グラフをフリーハンドで描いて値を出力するモード = true;
                p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = true;
            }
            else if (_buttonMode == (int)ETestButtonMode.グラフの点データ読み込み)
            {
                p_isTestGraphMode・グラフをフリーハンドで描いて値を出力するモード = true;
                p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = true;
            }
            else if (_buttonMode == (int)ETestButtonMode.フリーハンドお絵描きモード)
            {
                p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = false;
                p_isTrueDrawingSassa_FalseDrawFree・Trueサッサ描き専用か＿False普通のお絵かきか = false;
            }
            else if (_buttonMode == (int)ETestButtonMode.フリーハンドサッサ描きモード)
            {
                p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = false;
                p_isTrueDrawingSassa_FalseDrawFree・Trueサッサ描き専用か＿False普通のお絵かきか = true;
            }
            else if (_buttonMode == (int)ETestButtonMode.円描画モード)
            {
                p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = false;
            }
            else if (_buttonMode == (int)ETestButtonMode.四角形描画モード)
            {
                p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = false;
            }
            else if (_buttonMode == (int)ETestButtonMode.線描画モード)
            {
                p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか = false;
            }



            // button1のラベルを変更
            button1.Text = MyTools.getEnumName<ETestButtonMode>(_buttonMode);
        }


        #region 画面／データクリア系
        private void clearXYData・フリーハンドデータをクリア＿Ｕｎｄｏも残らない()
        {
            clearHistory・Ｕｎｄｏ用履歴データの削除();

            // データクリア
            _Xdata.Clear();
            _Ydata.Clear();


            clearScreenImage・画面だけをクリア＿データは消えない();
        }
        private void clearXYData・フリーハンドデータをクリア＿Ｕｎｄｏには残る()
        {
            addHistory・Ｕｎｄｏ用履歴データの追加();

            // データクリア
            _Xdata.Clear();
            _Ydata.Clear();


            clearScreenImage・画面だけをクリア＿データは消えない();
        }
        private void clearScreenImage・画面だけをクリア＿データは消えない()
        {
            if (p_graphBackImage == null)
            {
                string _fileFullPath = MyTools.getProjectDirectory() + "\\データベース\\グラフィック\\" + "1cm黒方眼紙_縦１０マス横１０マス_背景透明.png";
                //これだと画像がロックされてしまう。pictureBox1.Load(_fileFullPath);
                // ロックされずに、画像をロード
                if (MyTools.isExist(_fileFullPath) == true)
                {
                    p_graphBackImage = MyTools.getImage(_fileFullPath);
                    // エラー画像じゃなかったら
                    if (MyTools.isErrorImage(p_graphBackImage) == false)
                    {
                        if (p_graphScreenWidth == 0) changeGraphXYMinMax・グラフのＸＹ軸を変更();
                        // サイズをグラフX軸に合わせる。
                        double _sizeXRate = (double)p_graphScreenWidth / (double)p_graphBackImage.Width;
                        int _drawStartX = 0; int _drawStartY = 0; //// 左上からだとグラフの左下のy=0付近の軸が切れてしまうので、下端が必ず入るように、上部を切り取ってもらう _drawStartY = p_graphBackImage.Height - (int)((double)p_graphBackImage.Height * _sizeXRate);
                        // サイズをグラフY軸もX軸と別々に合わせる。
                        double _sizeYRate = (double)p_graphScreenHeight / (double)p_graphBackImage.Height;
                        p_graphBackImage = MyTools.getImage_DrawPart(p_graphBackImage, _drawStartX, _drawStartY, _sizeXRate, _sizeYRate);
                    }
                }
                else
                {
                    MessageBox.Show("ファイル\n" + _fileFullPath + "\nが見つかりませんでした。イメージをロードできません。");
                    p_graphBackImage = MyTools.getImage(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);
                }
                p_pictureNowImage = MyTools.getCopyedImage(p_graphBackImage);
                // pictureBox1.Imageに代入
                pictureBox1.Image = MyTools.getCopyedImage(p_graphBackImage);
                // または
                //pictureBox1.Image =
                //    new Bitmap(pictureBox1.ClientSize.Width, pictureBox1.ClientSize.Height);

            }

            if (pictureBox1.Image == null)
            {
            }
            else
            {
                // Imageを白紙（方眼紙だけがある状態）に戻す
                using (p_pictureNowImage) // マルチスレッド対策用。多少効果あり？
                {
                    if (p_pictureNowImage != null)
                    {
                        // [ToDo][Tips][Memo]これはしちゃダメ。なぜかMainメソッドのFDrawにさかのぼってまでのエラーになる・・。
                        //p_pictureNowImage.Dispose();
                        //p_pictureNowImage = null;
                    }

                    p_pictureNowImage = MyTools.getCopyedImage(p_graphBackImage);
                }
                reDraw・描画領域を再描画();
            }
        }
        #endregion

        /// <summary>
        /// pictureBox1.Imageに、p_pictureNowImageを描画して、コントロールを表示領域を更新します。
        /// </summary>
        public void reDraw・描画領域を再描画(){
            using (pictureBox1.Image) // マルチスレッド対策？
            {

                if (MyTools.isErrorImage(p_pictureNowImage) == false)
                {
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                    }
                    pictureBox1.Image = MyTools.getCopyedImage(p_pictureNowImage);
                }
            }
            // すぐでなくてもいいので、再描画（でも今回の描画ではImageを変更してるから、あまり効果は無い？？）
            pictureBox1.Invalidate();
            //pictureBox1.Update();

            //参考:Refresh、Update、Invalidateメソッドの違い http://dobon.net/vb/dotnet/control/refreshupdateinvalidate.html
            // 実際のこれらのメソッドの使い分けとしては、
            // コントロールを再描画したいがすぐである必要がないときはInvalidateメソッドを使い、
            // 今すぐ再描画する必要があるときはRefreshメソッドを使うということになるでしょう。


        }
        /// <summary>
        /// 今まで保存した飛び飛びのXY座標_Xdata、_Ydataを使って、x=0,1,2,...,graph_Widthと1ずつ詰まった_XAlldataとYAlldataを作成し、pictureBox1.Imageにグラフを一気に描画します。
        /// </summary>
        private void drawImage・XYdataに従ってフリーハンド画像を描画()
        {
            drawImage・XYdataに従ってフリーハンド画像を描画(0);
        }
        /// <summary>
        /// 今まで保存した飛び飛びのXY座標_Xdata、_Ydataを使って、x=0,1,2,...,graph_Widthと1ずつ詰まった_XAlldataとYAlldataを作成し、pictureBox1.Imageにグラフを一気に描画します。
        /// </summary>
        private void drawImage・XYdataに従ってフリーハンド画像を描画(int _waitingMsec_perDrawLine)
        {
            int _time1 = MyTools.getNowTime_fast();

            // 一回消したものを消すかどうか
            if (p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか == true)
            {
                // フリーハンドで線を描く（一回描いたものを消して、再描画する）
                clearScreenImage・画面だけをクリア＿データは消えない();
            }

            // 描画処理（Image に保持される、この間にpicturBox1.Imageを変更するコードをここにいれちゃダメ。gが機能しなくなるからダメ！）
            //using (Var game = Graphics.FromImage(pictureBox1.Image))
            using (Graphics g = Graphics.FromImage(p_pictureNowImage))
            {
                if (p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか == true)
                {

                    // フリーハンドで線を描く（一回描いたものを消して、再描画する）
                    //このメソッド内にpicture1.Imageを変更するコードがかいてあるからここにいれちゃダメ！clearScreenImage・画面だけをクリア＿データは消えない();

                    // 詰まったXY座標を初期化
                    _XAlldata.Clear();
                    _YAlldata.Clear();

                    // グラフのX座標に従って1ずつずらして描画
                    // グラフの最初の点(x1,y1)=(0,y_0)を定義
                    int _x1 = p_graphXMin;
                    int _y1 = MyTools.getListValue(_Ydata, 0); // データがなければ、Yの最初の要素
                    _y1 = MyTools.getAdjustValue(_y1, p_graphYMin, p_graphYMax); // 値の範囲調整
                    // 最初だけ、始点も追加
                    _XAlldata.Add(_x1);
                    _YAlldata.Add(_y1);

                    // 始点(x0,y0)-終点(x1,y1)を通る点を作成
                    int _x0;
                    int _y0;
                    int _startIndex = 0; // _Xdataと_Ydataで検索が終わった共通のインデックス
                    int _searchingIndexBlockNum = 5; // 周辺5ブロックを調べる // ここが処理速度のボトルネック…じゃないみたい？
                    // (x0, y0)～(graphWidth,ygraphWidth)までの全プロット点を作成
                    for (int x = p_graphXMin + 1; x < p_graphXMax; x++)
                    {
                        // 始点(x0,y0)は、前の終点
                        _x0 = _x1;
                        _y0 = _y1;
                        // 終点が(x1,y1)
                        _x1 = x;
                        // Yは、データがあれば、代入
                        // _nowIndex1 = MyTools.getIndex_MostClosed(_Xdata, x, _startIndex, _searchingIndexBlockNum); // ここが処理速度のボトルネック…じゃないみたい？
                        int _nowIndex1 = _Xdata.IndexOf(x, _startIndex); // これをするなら_startndex++をコメントアウトして
                        if (_nowIndex1 != -1)
                        {
                            _y1 = MyTools.getListValue(_Ydata, _nowIndex1);
                        }
                        else
                        {
                            
                            // ここ、ほとんど実行されてない。てか、上をIndexOfにして、実行されたら、パルスみたいになって精度ダメダメ。。。

                            // データがなければ、Yは検索し終わった最後の値
                            //_y1 = MyTools.getListValue(_Ydata, _startIndex);
                            // ↑よりも、↓こっちの方が精度が良いはず
                            // データが含まれていなければ、Yを補完して格納
                            // Yの値は、ドラック中のＸ座標に含まれる最も近い２点を通る、比例直線を使う
                            int _updateX = _x1;
                            int _index1 = MyTools.getIndex_MostClosed(_Xdata, _updateX, _startIndex, _searchingIndexBlockNum); // ここが処理速度のボトルネック…じゃないみたい？
                            int _index2 = MyTools.getIndex_NstClosed(_Xdata, _updateX, 2, _startIndex, _searchingIndexBlockNum); // ここが処理速度のボトルネック…じゃないみたい？
                            int x1 = MyTools.getListValue(_drawingUpdatedPosXdata, _index1); int y1 = MyTools.getListValue(_drawingUndatedPosYdata, _index1);
                            int x2 = MyTools.getListValue(_drawingUpdatedPosXdata, _index2); int y2 = MyTools.getListValue(_drawingUndatedPosYdata, _index2);
                            double _b;
                            double _hireiTeisu = MyTools.getHireiTeisu_UsingSaisyoZizyouHou(x1, y1, x2, y2, out _b);
                            int _hirei_y = MyTools.getSisyagonyuValue(_hireiTeisu * _updateX + _b);
                            _y1 = MyTools.getAdjustValue(_hirei_y, p_graphYMin, p_graphYMax);

                        }
                        // 区切り座標がまじってたら無視（フリーハンドお絵かき・サッサ描きとの整合性のため）
                        if (_y1 == -1) continue;
                        // 値の範囲調整
                        _x1 = MyTools.getAdjustValue(_x1, p_graphXMin, p_graphXMax);
                        _y1 = MyTools.getAdjustValue(_y1, p_graphYMin, p_graphYMax);

                        // サーチを開始する配列を移動。
                        if (_nowIndex1 != -1)
                        {
                            // サーチして見つかった後なので、配列数を超えたりはしない。でも0のことはあるので、念のため0以上にする
                            _startIndex = Math.Max(0, _nowIndex1);
                        }


                        // 線として描画
                        bool _isDrawLine = true;
                        // x0が0の時、初めの描画でない限り、描画しない
                        //if (_x0 == 0)
                        //{
                        //    if (_x1 != MyTools.getListValue(_Xdata, 0) && _x1 != MyTools.getListValue(_Xdata, 1))
                        //    {
                        //        _isDrawLine = false;
                        //    }
                        //}
                        //// x1が0のとき、描画しない
                        //if (_x1 == 0) _isDrawLine = false;
                        // _Xdata範囲外の、フリーハンドで保存されていない(xmid,ymid)の点は、直線に補完して描く
                        // xmidは、点(x0,x0)と点(x1,y1)を通る直線なので、実際には点を描かず、飛ばすだけでよい
                        if (_isDrawLine == true)
                        {
                            // グラフデータ用の座標：左下端が（0,0）⇔マウス用の座標：左上が(0,0)　の変換
                            Point _p0 = parseToCursorPoint・グラフ座標からカーソル座標への変換(_x0, _y0);
                            Point _p1 = parseToCursorPoint・グラフ座標からカーソル座標への変換(_x1, _y1);

                            // ペンの設定
                            float _penSize = 1.0f;
                            Pen _pen = new Pen(Color.Red, _penSize);

                            // 線を描画
                            g.DrawLine(_pen, _p0.X, _p0.Y, _p1.X, _p1.Y);
                        }


                        int _x = 640; int _y = 4167;
                        //Point _graphPoint = parseToGraphPoint・カーソル座標からグラフ座標への変換(_x, _y);
                        //Point _mousePoint = parseToCursorPoint・グラフ座標からカーソル座標への変換(_graphPoint.X, _graphPoint.Y);
                        if (_x1 == _x || _y1 == _y)
                        {
                            // なんか違う
                            int a = 0;
                        }

                        // 詰まったXY座標を追加
                        // 今の終点
                        _XAlldata.Add(_x1);
                        _YAlldata.Add(_y1);

                        // 待ち時間
                        MyTools.wait_ByApplicationDoEvents(_waitingMsec_perDrawLine);
                    }
                }
                else
                {
                    // フリーハンドで絵を描く（一回描いたものを消さない）
                    // 最後に描画し終わったインデックスの、次のインデックスから描画
                    for (int i = _lastDrawnXdataIndex + 1; i < _Xdata.Count; i++)
                    {
                        // 終点
                        int _x1 = MyTools.getListValue(_Xdata, i);
                        int _y1 = MyTools.getListValue(_Ydata, i);
                        // 終点に線の区切り座標(-1,-1)が入っていたら、この線はつなげない。
                        if (_x1 == -1 && _y1 == -1) continue; // 区切り座標がまじってたら無視
                        // グラフデータ用の座標：左下端が（0,0）⇔マウス用の座標：左上が(0,0)　の変換
                        Point _p1 = parseToCursorPoint・グラフ座標からカーソル座標への変換(_x1, _y1);

                        // 始点
                        int _x0 = MyTools.getListValue(_Xdata, i - 1);
                        int _y0 = MyTools.getListValue(_Ydata, i - 1);
                        // もし始点が存在しなかったら、終点といっしょ
                        if (_x0 == 0 && _y0 == 0)
                        {
                            _x0 = _x1;
                            _y0 = _y1;
                        }
                        if (_x0 == -1 && _y0 == -1) continue; // 区切り座標がまじってたら無視
                        // グラフデータ用の座標：左下端が（0,0）⇔マウス用の座標：左上が(0,0)　の変換
                        Point _p0 = parseToCursorPoint・グラフ座標からカーソル座標への変換(_x0, _y0);

                        float _penSize = 1.0f;
                        Pen _pen = new Pen(Color.Black, _penSize);

                        // 線を描画
                        g.DrawLine(_pen, _p0.X, _p0.Y, _p1.X, _p1.Y);

                    }
                    // 既に描画したXdataの配列を更新
                    _lastDrawnXdataIndex = _Xdata.Count - 1;
                }

            }
            // ピクチャボックスを再描画
            reDraw・描画領域を再描画();

            int _time2 = MyTools.getNowTime_fast();
            MyTools.ConsoleWriteLine(MyTools.getMethodName(2)+": 描画に "+(_time2-_time1)+"ミリ秒かかってるよ。");
        }
        private void changeGraphXYMinMax・グラフのＸＹ軸を変更()
        {
            p_graphYMin = Int32.Parse(textBoxYMin.Text);
            p_graphYMax = Int32.Parse(textBoxYMax.Text);
            p_graphXMin = Int32.Parse(textBoxXMin.Text);
            p_graphXMax = Int32.Parse(textBoxXMax.Text);

            p_graphScreenWidth = pictureBox1.Width;
            p_graphScreenHeight = pictureBox1.Height;
        }

        #region Undo/Redoの実現方法
        private bool p_isUndoUsingImage・メモリ肥大化するがUndo高速実現に画像を使うか = true;
        private bool p_isUndoUsingXYdata・Undo実現にXYdata履歴を使うか = false;

        private void addHistory・Ｕｎｄｏ用履歴データの追加()
        {
            bool _isAddedData = false;
            if (p_isUndoUsingImage・メモリ肥大化するがUndo高速実現に画像を使うか == true)
            {
                if (_HistoryNowIndex < _HistoryImage.Count - 1)
                {
                    // 最新の編集より前に格納されている履歴を消す（これまでのRedoを消す）
                    for (int i = _HistoryNowIndex + 1; i < _HistoryImage.Count; i++)
                    {
                        _HistoryImage.RemoveAt(i);
                    }
                }
                // 更新される前の画像を格納
                if (MyTools.isErrorImage(p_pictureNowImage) == false)
                {
                    _HistoryImage.Add(MyTools.getCopyedImage(p_pictureNowImage)); // [Tips]★Imageはcopyを渡さないと参照した側がDispose()すると元のsizeなどが全部'System.ArgumentException' の例外をスローしました。になる。
                    _isAddedData = true;
                }
            }else if (p_isUndoUsingXYdata・Undo実現にXYdata履歴を使うか == true)
            {
                if (_HistoryNowIndex < _HistoryXdata_UsingUndo.Count - 1)
                {
                    // 最新の編集より前に格納されている履歴を消す（これまでのRedoを消す）
                    for (int i = _HistoryNowIndex + 1; i < _HistoryXdata_UsingUndo.Count; i++)
                    {
                        _HistoryXdata_UsingUndo.RemoveAt(i);
                    }
                }
                // 更新される前の座標Ｘ,Yの格納（Undo用）
                _HistoryXdata_UsingUndo.Add(MyTools.getCopyedList(_Xdata));
                _HistoryYdata_UsingUndo.Add(MyTools.getCopyedList(_Ydata));
                _isAddedData = true;
            }
            // どっちかの要素が増えたらプラス
            if(_isAddedData == true)
            {
                _HistoryNowIndex++;
            }
        }
        private void clearHistory・Ｕｎｄｏ用履歴データの削除()
        {
            _HistoryXdata_UsingUndo.Clear();
            _HistoryYdata_UsingUndo.Clear();
            _HistoryImage.Clear();
            _HistoryNowIndex=0;
        }
        private bool undo・画像をひとつ前に戻す()
        {
            bool _isCanUndo = false;
            // Ctrl+ZはUndo
            if (_HistoryNowIndex > 0)
            {
                if (p_isUndoUsingImage・メモリ肥大化するがUndo高速実現に画像を使うか == true)
                {
                    if (_HistoryNowIndex - 1 <= _HistoryImage.Count - 1)
                    {
                        if (_HistoryNowIndex == _HistoryImage.Count)
                        {
                            // 更新される前の座標Ｘ,Yを最後のインデックスに最新一個だけ格納（Redo用）
                            if (MyTools.isErrorImage(p_pictureNowImage) == false)
                            {
                                _HistoryImage.Add(MyTools.getCopyedImage(p_pictureNowImage)); // [Tips]★Imageはcopyを渡さないと参照した側がDispose()すると元のsizeなどが全部'System.ArgumentException' の例外をスローしました。になる。
                            }
                        }
                        // ただ、履歴数はここでは増やさない

                        // 一個前
                        p_pictureNowImage = _HistoryImage[_HistoryNowIndex - 1];
                        reDraw・描画領域を再描画();

                        _isCanUndo = true;
                    }
                }
                else if (p_isUndoUsingXYdata・Undo実現にXYdata履歴を使うか == true)
                {
                    if (_HistoryNowIndex - 1 <= _HistoryXdata_UsingUndo.Count - 1)
                    {
                        if (_HistoryNowIndex == _HistoryXdata_UsingUndo.Count)
                        {
                            // 更新される前の座標Ｘ,Yを最後のインデックスに最新一個だけ格納（Redo用）
                            _HistoryXdata_UsingUndo.Add(MyTools.getCopyedList(_Xdata));
                            _HistoryYdata_UsingUndo.Add(MyTools.getCopyedList(_Ydata));
                        }
                        // ただ、履歴数はここでは増やさない
                        
                        // 一個前
                        _Xdata = _HistoryXdata_UsingUndo[_HistoryNowIndex - 1];
                        _Ydata = _HistoryYdata_UsingUndo[_HistoryNowIndex - 1];
                        drawImage・XYdataに従ってフリーハンド画像を描画();

                        _isCanUndo = true;
                    }
                }
                if (_isCanUndo == true)
                {
                    // Undoしたから一個前にする
                    _HistoryNowIndex--;
                    if (_HistoryNowIndex < 0) _HistoryNowIndex = 0;
                }
            }
            showPosition・座標をラベルに表示();
            return _isCanUndo;
        }
        private bool redo・画像を一度戻ったひとつ前の状態に進ませる()
        {
            bool _isCanRedo = false;
            // Redo
            if (_HistoryNowIndex > -1)
            {
                if (p_isUndoUsingImage・メモリ肥大化するがUndo高速実現に画像を使うか == true)
                {
                    if (_HistoryNowIndex + 1 <= _HistoryImage.Count - 1)
                    {
                        // たぶん、RedoはＵｎｄｏが一回以上されることが条件になってるから、追加は要らない

                        // 一個後
                        p_pictureNowImage = _HistoryImage[_HistoryNowIndex + 1];
                        reDraw・描画領域を再描画();

                        _isCanRedo = true;
                    }
                }else if (p_isUndoUsingXYdata・Undo実現にXYdata履歴を使うか == true)
                {
                    if (_HistoryNowIndex + 1 <= _HistoryXdata_UsingUndo.Count - 1)
                    {
                        // たぶん、RedoはＵｎｄｏが一回以上されることが条件になってるから、追加は要らない

                        // 一個後
                        _Xdata = _HistoryXdata_UsingUndo[_HistoryNowIndex + 1];
                        _Ydata = _HistoryYdata_UsingUndo[_HistoryNowIndex + 1];
                        drawImage・XYdataに従ってフリーハンド画像を描画();

                        _isCanRedo = true;
                    }
                }
                if (_isCanRedo == true)
                {
                    // Redoしたから、一個後にする
                    _HistoryNowIndex++;
                    //if (_HistoryNowIndex > _HistoryXdata_UsingUndo.Count - 1)
                    //{
                    //    _HistoryNowIndex = _HistoryXdata_UsingUndo.Count; // 一個超えてるけど、これでいい
                    //}
                    //if (_HistoryNowIndex > _HistoryImage.Count - 1)
                    //{
                    //    _HistoryNowIndex = _HistoryImage.Count; // 一個超えてるけど、これでいい
                    //}
                }

            }
            showPosition・座標をラベルに表示();
            return _isCanRedo;
        }
        #endregion













        // 以下、フォームコントロールイベント



        private void FDrawForm_Load(object sender, EventArgs e)
        {


            // キーイベントはコントロールと一緒にする
            this.KeyPreview = true;

            _Xdata = new List<int>();
            _Ydata = new List<int>();
            _XAlldata = new List<int>();
            _YAlldata = new List<int>();
            _drawingUpdatedPosXdata = new List<int>();
            _drawingUndatedPosYdata = new List<int>();
            _HistoryXdata_UsingUndo = new List<List<int>>();
            _HistoryYdata_UsingUndo = new List<List<int>>();
            _HistoryImage = new List<Image>();


            // button1の機能初期化
            p_buttonMode = 1; // デフォルトは1にしとく
            changeButtonMode・描画モードを変更(p_buttonMode);
            // comboBox1の初期化 // モードを司る
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(MyTools.getEnumNames<ETestButtonMode>());
            comboBox1.Items.RemoveAt(0); // ETestButtonModeの最初の要素「無し」を削除
            comboBox1.Items.RemoveAt(comboBox1.Items.Count - 1); // ETestButtonModeの最後の要素「.Count」を削除
            int _index = comboBox1.Items.IndexOf(ETestButtonMode.グラフのフリーハンド描画モード.ToString());
            if (_index != -1) comboBox1.Items.RemoveAt(_index); // ETestButtonModeのいらない要素を削除
            _index = comboBox1.Items.IndexOf(ETestButtonMode.フリーハンドお絵描きモード.ToString());
            if (_index != -1) comboBox1.Items.RemoveAt(_index); // ETestButtonModeのいらない要素を削除
            _index = comboBox1.Items.IndexOf(ETestButtonMode.フリーハンドサッサ描きモード.ToString());
            if (_index != -1) comboBox1.Items.RemoveAt(_index); // ETestButtonModeのいらない要素を削除
            // labelの初期化
            label2.Text = "" + "　（※「Ctrl+Z」でグラフをひとつ前に戻す（Ｕｎｄｏ）、「Ctrl+Y」でＲｎｄｏ）が出来ます）";

            // 座標変換テスト
            //int _x = 640; int _y = 480;
            //Point _graphPoint = parseToGraphPoint・カーソル座標からグラフ座標への変換(_x, _y);
            //Point _mousePoint = parseToCursorPoint・グラフ座標からカーソル座標への変換(_graphPoint.X, _graphPoint.Y);
            //if (_x != _mousePoint.X || _y != _mousePoint.Y)
            //{
            //    // なんか違う
            //    int a = 0;
            //}

            // 画像の初期化
            changeGraphXYMinMax・グラフのＸＹ軸を変更();
            clearScreenImage・画面だけをクリア＿データは消えない();

        }



        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // モードに関係なく描画する
            if (true) //p_isTestGraphMode・グラフをフリーハンドで描いて値を出力するモード == true)
            {
                _mouseDown = true;
                // 現在のマウス座標を取得
                Point _position1 = MyTools.getMouseCursorPosition_ByControl(this, pictureBox1);
                _drawStartMousePosX = _position1.X;// _e.X;
                _drawStartMousePosY = _position1.Y;// _e.Y;
                // カーソル用の座標：左上が(0,0)　→　グラフデータ用の座標：左下端が（0,0）　の変換
                Point _posiGraph1 = parseToGraphPoint・カーソル座標からグラフ座標への変換(_position1);
                
                // ドラッグの最初のポイントを追加
                _drawStartPosX = _posiGraph1.X;
                _drawStartPosY = _posiGraph1.Y;
                _drawingUpdatedPosXdata.Clear();
                _drawingUpdatedPosXdata.Add(_drawStartPosX);
                _drawingUndatedPosYdata.Clear();
                _drawingUndatedPosYdata.Add(_drawStartPosY);                

                _lastRect = Rectangle.Empty;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // モードに関係なく描画する
            if (true) //p_isTestGraphMode・グラフをフリーハンドで描いて値を出力するモード == true)
            {
                // 現在のマウス座標を取得
                Point _posiMouse1 = MyTools.getMouseCursorPosition_ByControl(this, pictureBox1);
                //int _x1 = _posiMouse1.X; //_e.X;
                //int _y1 = _posiMouse1.Y; //_e.Y;
                // カーソル用の座標：左上が(0,0)　→　グラフデータ用の座標：左下端が（0,0）　の変換
                Point _posiGraph1 = parseToGraphPoint・カーソル座標からグラフ座標への変換(_posiMouse1);
                int _x1 = _posiGraph1.X;
                int _y1 = _posiGraph1.Y;
                // 座標を表示
                label1.Text = "マウス座標(" + MyTools.getStringNumber(_posiMouse1.X, true, 4, 0) + ", " + MyTools.getStringNumber(_posiMouse1.Y, true, 4, 0) + ")"
                    + "　　グラフ座標(" + MyTools.getStringNumber(_x1, true, 6, 0) + ", " + MyTools.getStringNumber(_y1, true, 6, 0) + ")"
                    +"　　Undo可能な残り履歴数:" + _HistoryNowIndex;

                // マウスボタンが押されていなかったら、何もしない
                if (!_mouseDown) return;



                // ■↓以下、ドラッグ時の処理


                // ピクチャボックスを再描画
                if (p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか == false)
                {
                    // フリーハンドお絵かき・サッサ描きのときは再描画。グラフの時は要らない
                    pictureBox1.Invalidate();
                    pictureBox1.Update();

                }
                // 円・四角形・線を描く時、描画を更新する領域だけを初期化
                if (!_lastRect.IsEmpty)
                {
                    pictureBox1.Invalidate(_lastRect);
                    pictureBox1.Update();
                }

                // 描画処理（一時的）
                using (var g = pictureBox1.CreateGraphics()) // [Tips]★game=Graphics.FromImage(pictureBox1.Image)とは違うので、すぐ消える。game = pictureBox1.CreateGraphics()はpricuteBox1.Invalidate()で初期化される
                {
                    if (p_buttonMode == (int)ETestButtonMode.円描画モード)
                    {
                        // 描画を更新する領域を設定
                        _lastRect = new Rectangle(_drawStartMousePosX, _drawStartMousePosY, _posiMouse1.X - _drawStartMousePosX, _posiMouse1.Y - _drawStartMousePosY);
                        // 円を描く
                        g.DrawEllipse(Pens.Black, _lastRect);
                        // Pen の幅の分、広げる
                        _lastRect.Inflate(_posiMouse1.X > _drawStartMousePosX ? 1 : -1, _posiMouse1.Y > _drawStartMousePosY ? 1 : -1);
                    }
                    else if (p_buttonMode == (int)ETestButtonMode.四角形描画モード)
                    {
                        // 描画を更新する領域を設定
                        _lastRect = new Rectangle(_drawStartMousePosX, _drawStartMousePosY, _posiMouse1.X - _drawStartMousePosX, _posiMouse1.Y - _drawStartMousePosY);
                        // 四角形を描く
                        g.DrawRectangle(Pens.Black, _lastRect);
                        // Pen の幅の分、広げる
                        _lastRect.Inflate(_posiMouse1.X > _drawStartMousePosX ? 1 : -1, _posiMouse1.Y > _drawStartMousePosY ? 1 : -1);
                    }
                    else if (p_buttonMode == (int)ETestButtonMode.線描画モード)
                    {
                        // 円を描く時、描画を更新する領域を設定
                        _lastRect = new Rectangle(_drawStartMousePosX, _drawStartMousePosY, _posiMouse1.X - _drawStartMousePosX, _posiMouse1.Y - _drawStartMousePosY);
                        // 線を描く
                        g.DrawLine(Pens.Black, _drawStartMousePosX, _drawStartMousePosY, _posiMouse1.X, _posiMouse1.Y);
                    }
                    else
                    {




                        // フリーハンドの線を描画

                        // 一個前のデータ座標を、あれば取得。なければ現在の座標と一緒（_x0=_x1,_y0=_y1）
                        int _x0 = MyTools.getListValue(_drawingUpdatedPosXdata, _drawingUpdatedPosXdata.Count - 1); if (_x0 == 0) _x0 = _x1;
                        int _y0 = MyTools.getListValue(_drawingUndatedPosYdata, _drawingUndatedPosYdata.Count - 1); if (_y0 == 0) _y0 = _y1;

                        // ペンの設定、描く
                        float _penSize = 3.0f;
                        Pen _pen = new Pen(Color.Orange, _penSize);
                        // グラフデータ用の座標：左下端が（0,0）　→　カーソル用の座標：左上が(0,0)　の変換
                        Point _posi0 = parseToCursorPoint・グラフ座標からカーソル座標への変換(_x0, _y0);
                        Point _posi1 = parseToCursorPoint・グラフ座標からカーソル座標への変換(_x1, _y1);
                        g.DrawLine(_pen, _posi0.X, _posi0.Y, _posi1.X, _posi1.Y);


                        // グラフ専用フリーハンドでは、同じＸの値は最新のもの一つだけにする。
                        if (p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか == true)
                        {

                            // ドラッグ中は、例えば上にドラッグしても(50,100)→(50,101)→(50,104)...など、同じＸの値がよく入る 
                            // ドラッグ中に認識したＸの中に、同じＸの値があった場合、上書き。
                            if (_drawingUpdatedPosXdata.Contains(_posiGraph1.X))
                            {
                                int _updateIndex = _drawingUpdatedPosXdata.IndexOf(_posiGraph1.X);
                                _drawingUpdatedPosXdata[_updateIndex] = _posiGraph1.X;
                                // Yの値もいっしょのインデックスを書き変える
                                _drawingUndatedPosYdata[_updateIndex] = _posiGraph1.Y;
                            }
                            else
                            {
                                // 同じＸの値がなければ、ドラッグ中のＸの範囲に新しくデータを追加
                                _drawingUpdatedPosXdata.Add(_posiGraph1.X);
                                _drawingUndatedPosYdata.Add(_posiGraph1.Y);
                            }
                        }
                        else
                        {
                            // ドラッグ中のＸの範囲に新しくデータを追加
                            _drawingUpdatedPosXdata.Add(_posiGraph1.X);
                            _drawingUndatedPosYdata.Add(_posiGraph1.Y);
                        }

                    }
                    // 各描画モードの処理終わり
                }
                // using終わり
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // モードに関係なく描画する
            if (true) //p_isTestGraphMode・グラフをフリーハンドで描いて値を出力するモード == true)
            {

                if (!_mouseDown) return;
                _mouseDown = false;

                // 更新される前の座標Ｘ,Yの格納（Undo用）
                addHistory・Ｕｎｄｏ用履歴データの追加();

                // マウス座標を取得
                Point _positionMouse = MyTools.getMouseCursorPosition_ByControl(this, pictureBox1);
                // グラフ用座標に変換
                Point _positionGraph = parseToGraphPoint・カーソル座標からグラフ座標への変換(_positionMouse);




                // グラフ専用フリーハンドでは、同じＸの値は最新のもの一つだけにする。
                if (p_isTrueDrawingGraph_FalseDrawFreeHandOekaki・Trueグラフ専用に調整されたフリーハンドか＿Falseお絵かき専用の自由なフリーハンドか == true)
                {
                    // 更新された座標Ｘの範囲を、前から1ずつ確認して格納
                    int _xstart = MyTools.getSmallestValue(_drawingUpdatedPosXdata);
                    int _xend = MyTools.getBiggestValue(_drawingUpdatedPosXdata);
                    // ■■下記一行にすると、Xdataの数が膨大に多く重くなるが、穴を埋めるため、曲線の上書きが正常に動作する。
                    for (int i = _xstart; i <= _xend; i++)
                    // ■上記一行のforの代わりに以下の二行のforeachの以下を使うと、Xは1ずつではなく、飛ばすのも許して格納することになる
                    // 　　　こうすると描画する直線は滑らかになるし、処理速度は上がる。が、穴が大きくなるので、Xの数が大きいと上書きするとギザギザになるから、使いものにならない。
                    // List<int> _sortedDrawingUpdatePosXdata = MyTools.getSortedList(_drawingUpdatedPosXdata, MyTools.ESortType.値が小さい順＿昇順);
                    // foreach (int i in _sortedDrawingUpdatePosXdata)
                    {
                        if (i == -1) continue; // 区切り座標がまじってたら無視（グラフモードとお絵かきモードの整合性を高めるため）

                        // 最新のXとYの値の確定
                        int _updateX = i;
                        int _updateY = -1; // まだ決まってない
                        if (_drawingUpdatedPosXdata.Contains(_updateX) == true)
                        {
                            // 含まれていれば、最新の（後ろから検索して見つかった）Yをそのまま格納 ※■ここ重要。LastIndexOfにしないと、前に描いた値になってしまう。
                            int _updateIndex = _drawingUpdatedPosXdata.LastIndexOf(_updateX);
                            // 値を整形（マイナスは無しで）
                            _updateY = _drawingUndatedPosYdata[_updateIndex];

                        }
                        else
                        {
                            // 含まれていなければ、Yを補完して格納
                            // Yの値は、ドラック中のＸ座標に含まれる最も近い２点を通る、比例直線を使う                            
                            int _index1 = MyTools.getIndex_MostClosed(_drawingUpdatedPosXdata, _updateX);
                            int _index2 = MyTools.getIndex_NstClosed(_drawingUpdatedPosXdata, _updateX, 2);
                            int _x1 = MyTools.getListValue(_drawingUpdatedPosXdata, _index1); int _y1 = MyTools.getListValue(_drawingUndatedPosYdata, _index1);
                            int _x2 = MyTools.getListValue(_drawingUpdatedPosXdata, _index2); int _y2 = MyTools.getListValue(_drawingUndatedPosYdata, _index2);
                            double _b;
                            double _hireiTeisu = MyTools.getHireiTeisu_UsingSaisyoZizyouHou(_x1, _y1, _x2, _y2, out _b);
                            int _hirei_y = MyTools.getSisyagonyuValue(_hireiTeisu * _updateX + _b);
                            // 値を整形（マイナスは無しで）
                            _hirei_y = MyTools.getAdjustValue(_hirei_y, p_graphYMin, p_graphYMax);
                            _updateY = _hirei_y;
                        }

                        // なんか違う値が入った時のテスト用
                        int _x = 640; int _y = -1;//480;
                        //Point _graphPoint = parseToGraphPoint・カーソル座標からグラフ座標への変換(_x, _y);
                        //Point _mousePoint = parseToCursorPoint・グラフ座標からカーソル座標への変換(_graphPoint.X, _graphPoint.Y);
                        if (_updateY == _y)
                        {
                            // なんか違う
                            int a = 0;
                        }


                        // 最新のXとYを格納。
                        if (_Xdata.Contains(_updateX))
                        {
                            // ×同じXがあった場合は上書き // ■■上書きちがう。だって配列はうわがきしたら、かならずさいしんのものがうしろにあるとはかぎらんじゃん。ちゃんと後ろに最新が来るように、消して追加しないと。
                            // ○同じXが合った場合は消して新しく追加
                            int _updateIndex = _Xdata.IndexOf(_updateX);
                            _Xdata.RemoveAt(_updateIndex);
                            _Ydata.RemoveAt(_updateIndex);
                            _Xdata.Add(_updateX);
                            _Ydata.Add(_updateY);
                            
                        }
                        else
                        {
                            // 新しくデータを追加
                            _Xdata.Add(_updateX);
                            _Ydata.Add(_updateY);
                        }
                    }

                    // へんなきせきをえがいたばあいのたいしょがまだのような・・・

                }
                else
                {
                    // グラフ以外の描画モードの時

                    if (p_buttonMode == (int)ETestButtonMode.円描画モード)
                    {
                        Graphics g = Graphics.FromImage(p_pictureNowImage);
                        //  円を描く
                        g.DrawEllipse(Pens.Black, _drawStartMousePosX, _drawStartMousePosY, _positionMouse.X - _drawStartMousePosX, _positionMouse.Y - _drawStartMousePosY);
                        g.Dispose();
                    }
                    else if (p_buttonMode == (int)ETestButtonMode.四角形描画モード)
                    {
                        Graphics g = Graphics.FromImage(p_pictureNowImage);
                        // 四角形を描く
                        g.DrawRectangle(Pens.Black, _drawStartMousePosX, _drawStartMousePosY, _positionMouse.X - _drawStartMousePosX, _positionMouse.Y - _drawStartMousePosY);
                        g.Dispose();
                    }
                    else if (p_buttonMode == (int)ETestButtonMode.線描画モード)
                    {
                        Graphics g = Graphics.FromImage(p_pictureNowImage);
                        // 線を描く
                        g.DrawLine(Pens.Black, _drawStartMousePosX, _drawStartMousePosY, _positionMouse.X, _positionMouse.Y);
                        g.Dispose();
                    }
                    else
                    {

                        // フリーハンドお絵かき・サッサ描きの場合は、そのまま新しくデータを追加し、最後に区切り座標を追加
                        int _updateX;
                        int _updateY;
                        for (int i = 0; i < _drawingUpdatedPosXdata.Count; i++)
                        {
                            // 最新のXとYの値の確定
                            _updateX = _drawingUpdatedPosXdata[i];
                            _updateY = _drawingUndatedPosYdata[i];
                            // 新しくデータを追加
                            _Xdata.Add(_updateX);
                            _Ydata.Add(_updateY);
                        }
                        // 最後に、線の区切り座標(-1,-1)を追加
                        _updateX = -1;
                        _updateY = -1;
                        _Xdata.Add(_updateX);
                        _Ydata.Add(_updateY);
                    }
                }

                drawImage・XYdataに従ってフリーハンド画像を描画();
            }
        }



        
        // 以下、フォームコントロールイベント

        private void button1_Click(object sender, EventArgs e)
        {
            doButtonMode・描画機能を実行();
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            // 次のモードへ変更（機能は実行しない）
            p_buttonMode++;
            string _modeString = MyTools.getEnumName<ETestButtonMode>(p_buttonMode);
            // 「モード」と記述のあるものが出るまで、モード番号を++
            while(_modeString.Contains("モード") == false){
                p_buttonMode++;
                // インデックスが超えたら、初めに戻す
                if (p_buttonMode > (int)ETestButtonMode.Count-1) p_buttonMode = 0;
                _modeString = MyTools.getEnumName<ETestButtonMode>(p_buttonMode);
            }
            changeButtonMode・描画モードを変更(p_buttonMode);

            // モード毎に、コンボボックスの機能リストを更新するか
        }
        // 上記のボタンの代わりに、リストボックスでも実現した方が選択が早いし使いやすい（しかしリストは動的に出てこない）
        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs _e)
        //{
        //    // 選んだモードへ変更
        //    ListBox _listBox = (ListBox)sender;
        //    p_buttonMode = _listBox.SelectedIndex; // 0:「無し」はリストに入っているとする
        //    if (p_buttonMode < 0) p_buttonMode = 1; // 1:「グラフ」にしとく
        //    if (p_buttonMode >= (int)ETestButtonMode.Count) p_buttonMode = 1; // 1:「グラフ」に戻る
        //    button1.Text = MyTools.getEnumName<ETestButtonMode>(p_buttonMode);
        //    changeButtonMode・描画モードを変更();
        //}
        // コンボボックスの方が、縦サイズが狭くても右の▼ボタンを押すとリストを出して選べるし、いざとなったら自分で書けるからいい（リストボックスはリストが出てこない）
        // ただし、コンボボックスは要素を選ぶのに計二回クリックしないといけない。
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 選んだ機能を実行
            ComboBox _control = (ComboBox)sender;

            int _defaultButtonMode = 1; // 1にしとく
            // 選んだ要素名からモードを取得。
            string _selectedItemString = _control.SelectedItem.ToString();
            ETestButtonMode _EbuttonMode = MyTools.getEnumItem_FromString<ETestButtonMode>(_selectedItemString);
            //ETestButtonMode _defaultT = default(ETestButtonMode); // default(T)てどんなものかてすと =最初の要素「.無し」でした。
            //p_buttonMode = (int)_defaultT; // テスト　=0でした。
            p_buttonMode = (int)_EbuttonMode;
            // 変な値だった場合は変更
            if (_control.Items.Contains(_control.Text) == false) p_buttonMode = _defaultButtonMode;
            if (p_buttonMode < 0) p_buttonMode = _defaultButtonMode;
            if (p_buttonMode >= (int)ETestButtonMode.Count) p_buttonMode = _defaultButtonMode;
            changeButtonMode・描画モードを変更(p_buttonMode);

            // 実行もしちゃえ
            doButtonMode・描画機能を実行();
        }


        
        private void button2_Click(object sender, EventArgs e)
        {
            // データ毎クリア
            //残すとどんどん重くなるclearXYData・フリーハンドデータをクリア＿Ｕｎｄｏには残る();
            clearXYData・フリーハンドデータをクリア＿Ｕｎｄｏも残らない();
        }

        private void FDrawForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // PressはなんかKeyCodeが使えない
            if (e.KeyChar == 'z' || e.KeyChar == 'Z') // z　か　SHIFT+Z　か　CapsLock中のz　だったら、とかしかできない
            {
            }
        }

        private void FDrawForm_KeyDown(object sender, KeyEventArgs e)
        {
            // 押しているキー情報の表示
            string stat = "";
            if (e.Control) stat += "Ctrl ";
            if (e.Alt) stat += "Alt ";
            label2.Text = stat + e.KeyCode.ToString() + "　（※「Ctrl+Z」でグラフをひとつ前に戻す（Ｕｎｄｏ）、「Ctrl+Y」でＲｎｄｏ）が出来ます）";

            if (e.Control == true)
            {
                if (e.KeyCode == Keys.Z)
                {
                    undo・画像をひとつ前に戻す();
                }
                if (e.KeyCode == Keys.Y)
                {
                    redo・画像を一度戻ったひとつ前の状態に進ませる();
                }
            }
        }



        private void buttonXYMinMaxChange_Click(object sender, EventArgs e)
        {
            changeGraphXYMinMax・グラフのＸＹ軸を変更();
            drawImage・XYdataに従ってフリーハンド画像を描画();
            
        }
        public void _HELP_TestCode・このクラスの使い方サンプル()
        {
            //描画テスト
            List<Point> points = new List<Point>();
            points.Add(new Point(100, 100));
            points.Add(new Point(500, 300));
            MyTools.drawLines_Graph(points, ref p_pictureNowImage, Color.Orange, MyTools.EGraphPositionType.t0_WindowPosition_ウィンドウ座標＿左上端が００で＿y軸が下に行くとプラス);
            reDraw・描画領域を再描画();

            // 自分で作ったグラフの値をすぐ出したい時に
            double[] _myGraph = new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int _x = 5;
            double _y = MyGraph.getY(_myGraph, _x);
            Console.WriteLine("グラフ： x=" + _x + "の時のyの値は、" + _y + "です。");

            // 予め定義されたグラフの値をすぐ出したい時に
            double[] _graph1_x1To1000_y0To1000 = MyGraph.getYs(EGraph・グラフの種類.freehandA_成長曲線まったりいこうぜ＿直線波揺れ型);
            double[] _graph2_x1To1000_y0To1000 = MyGraph.getYs(EGraph・グラフの種類.freehandB_成長曲線ジグザグいこうぜ＿階段型);
            double[] _graph3_x0To1000_y0To1000 = MyGraph.getYs(EGraph・グラフの種類.freehandC_成長曲線早いとこいこうぜ＿円弧型);//, 0, 100, 0, 100);
            double[] _graph4_x0To1000_y0To1000 = MyGraph.getYs(EGraph・グラフの種類.freehandD_成長曲線遅咲きでいこうぜ＿指数関数増加型);//, 0, 100, 0, 100);
            double _y_x30_in_graph1 = MyGraph.getY(_graph1_x1To1000_y0To1000, 30);
            _x = 500;
            _y = MyGraph.getY(_graph1_x1To1000_y0To1000, _x);
            Console.WriteLine("グラフ： x=" + _x + "の時のyの値は、" + _y + "です。");
            _x = 500;
            _y = MyGraph.getY(_graph2_x1To1000_y0To1000, _x);
            Console.WriteLine("グラフ： x=" + _x + "の時のyの値は、" + _y + "です。");

            showGraph・引数のグラフをお絵かきモードで描画(_graph1_x1To1000_y0To1000, System.Drawing.Color.Green);
            // これでもできる
            MyTools.drawLines_Graph(_graph2_x1To1000_y0To1000, ref p_pictureNowImage, MyColor.getRainbowRandomColor・呼び出す毎に色が変わるランダムな虹色に近い色(), MyTools.EGraphPositionType.t0_WindowPosition_ウィンドウ座標＿左上端が００で＿y軸が下に行くとプラス);
            MyTools.drawLines_Graph(_graph1_x1To1000_y0To1000, ref p_pictureNowImage, Color.Yellow, MyTools.EGraphPositionType.t0_WindowPosition_ウィンドウ座標＿左上端が００で＿y軸が下に行くとプラス);            
            reDraw・描画領域を再描画();
        }


        #region 以下、草案やメモ


        // http://bbs.wankuma.com/index.cgi?mode=al2&namber=43595&KLOG=74
        //> ３ボタン同時押しのイベントは判断可能でしょうか？
        //できますよー
        //private void Form1_KeyDown(object sender, KeyEventArgs _e)
        //{
        //    string stat = "";
        //    if (_e.Control) stat += "CTRL ";
        //    if (_e.Alt) stat += "ALT ";
        //    label1.Text = stat + _e.KeyCode.ToString();
        //}



        // http://nonki.yoka-yoka.jp/e3293.html
        //アプリ起動時にSHIFTキーが押されているかどうかを
        //検知する方法を調べておりましたら、Control.ModifierKeysが
        //使える事がわかりました。
        //（ちゃんとした方法があるかも知れませんが、まぁこれで代用できました。）

        //以下を、Form_Load辺りに埋め込めばＯＫ。
        //if((Control.ModifierKeys & Keys.Shift) == Keys.Shift){
        //   //ここにShiftキーが押されている時の処理をいれる。
        //}

        //※Control.ModifierKeys は、いろんなところで使えるみたい。

        //しかし、探すのに苦労しました。
        #endregion




    }
}
