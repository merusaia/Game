using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PublicDomain
{
    /// <summary>
    /// ■■■CFpsManager・フレーム管理者は、高速さが要求されるため、不必要な処理をばっさり削除して、スマートにしました。
    /// ■■■このdraft_は、クラス内で論理時間や描画時間などを産出しようとして、ややこしい計算が合わなくなっていきずまっちゃいましたが、念のためここに置いておきます。
    /// 
    /// ゲームの待ち時間やfps、遅延などを管理するフレーム管理者クラスです。yaneSDKのFpsTimerを編集しています。
    /// （Windows非依存。Windows依存のタイマー処理はFormに書いてください）
    /// 
    /// ゲームのメインループを管理するメソッド（またはスレッド）に、
    /// 
    /// p_fpsManager = new draft_CFpsManager・フレーム管理者(50); // 例えば、fpsを50に設定
    /// while (true){
    ///     
    ///     論理処理();
    ///     
    ///     // 論理処理も含めて、最大1フレーム分だけ待ち、遅延が大きければ描画処理をスキップ
    ///     p_fpsManager.updateFrame・論理処理後のフレーム更新処理と描画処理スキップ判定();
    ///     int _論理処理にかかった時間 = getLogicMSec();
    ///		if(isDelay_SkipDraw・遅延により描画処理をスキップするべきか() == true){ 
    ///		    描画処理();
    ///		}
    ///     p_fpsManager.waitRestFrame・遅延を考慮して最大1フレーム分だけ待つ();
    ///     int _描画処理にかかった時間 = getDrawMSec();
    /// }
    /// と書けば、1秒間に設定されたfpsの回数だけ、論理処理を行いつつ、
    /// 遅延に強い（フレームの安定した）描画処理が実現できます。
    /// 
    /// 
    /// （※fps(Frame Per Second)とは、1秒間に実行されるフレーム数のこと。
    /// 例えば、fps=50なら、1フレーム=1000/50=20ミリ秒になります）
    /// 
    /// </summary>
    public class draft_CFpsManager・フレーム管理者
    {
        /// <summary>
        /// このゲームタイマーが開始された時間（getTime・現在時刻()で取得したミリ秒）です。初期値は-1です。
        /// </summary>
        public long p_startTime・開始時刻 = -1;
        /// <summary>
        /// ゲームの経過時間（ミリ秒）を高精度に取得します。具体的には、このゲームタイマーが開始された時間（コンストラクタが呼ばれた時間）から、最後にフレームが更新された時刻を返します。
        /// </summary>
        /// <returns></returns>
        public long getPassedMSec・ゲーム経過時間を高精度にミリ秒で取得(){
            return (p_startTime・開始時刻!=-1)? (p_lastUpdateTime・最後にupdateFrameを呼び出した時刻 - p_startTime・開始時刻) : 0 ;
        }
        public long p_lastUpdateTime・最後にupdateFrameを呼び出した時刻 = 0;
        public long p_lastWaitTime・最後にwaitRestFrameを呼び出した時刻 = 0;
        /// <summary>
        /// 経過時間を高精度に測るクラスです。外部からも自由に使ってもらってＯＫです。ただし使う前に.Reset()するのを忘れずに。
        /// </summary>
        public System.Diagnostics.Stopwatch p_stopwatch・ストップウォッチ;
        // 今は使ってない
        /// <summary>
        /// 経過時間を高精度に測る内部のクラスです。内部限定で、getNowTimeMSecメソッドで使います。
        /// </summary>
        private System.Diagnostics.Stopwatch _stopwatchInner・内部腕時計;
        ///// <summary>
        ///// true実際より後のフレームにして実際の待ち時間をより長くしたいか、false実際より前のフレーム数分だけ待って短くしたいか
        ///// </summary>
        //public bool p_isWait1Frame_longer = false;

        /// <summary>
        /// コンストラクタです。引数にfps（１秒間に何回、処理を実行する必要があるか。ゲームでは通常50fpsほど、0にすると処理停止）を設定してください。
        /// あとでsetFPSでも変更できます。
        /// </summary>
        public draft_CFpsManager・フレーム管理者(int _fps_FramePerSecond)
		{
			// yaneSDKからの変更点。timerによる現在時刻取得は他のクラスに依存せず、メソッドgetNoTime()で取得。以下は元の依存してたコード。timer = new GameTimer();
            p_stopwatch・ストップウォッチ = new System.Diagnostics.Stopwatch();
            _stopwatchInner・内部腕時計 = new System.Diagnostics.Stopwatch();
            _stopwatchInner・内部腕時計.Start(); // ここから時間を開始

			p_fps = _fps_FramePerSecond;
            setFPS・ゲームの最大フレームレートを変更(_fps_FramePerSecond);
            p_startTime・開始時刻 = getNowTimeMSec();
			p_continualFrameSkipCount = 0;

			Reset();
		}

        /// <summary>
        /// ※定期スレッドにより、このメソッドを１フレーム毎に呼び出してください。
        /// 前回呼び出された時刻と論理処理間を考慮して、スキップするべきかどうかを調べます。
        /// （内部でWaitFrameを呼び出しています）。
        /// 
        /// なお、このメソッドの返り値は、
        /// isDelay_SkipDraw・遅延により描画処理をスキップするべきか()と同値です。 
        /// 
        /// </summary>
        public bool updateFrame・論理処理後のフレーム更新処理と描画処理スキップ判定()
        {
            // 遅延を考慮して、描画をスキップするべきかを調べる。
            WaitFrame();

            // 最後にupdateFrameを呼び出した時刻を更新
            p_lastUpdateTime・最後にupdateFrameを呼び出した時刻 = getNowTimeMSec();

            return isDelay_SkipDraw・遅延により描画処理をスキップするべきか();
        }
        /// <summary>
        /// 現在のフレームが、描画処理をスキップした方がいいくらい遅延しているかを判定します。
        /// 　このメソッドは、定期的にupdateFrame・フレーム更新処理()が呼び出されている時だけ機能します。
        /// </summary>
        /// <returns></returns>
        public bool isDelay_SkipDraw・遅延により描画処理をスキップするべきか()
        {
            return ToBeSkip;
        }
        /// <summary>
        /// 論理処理、描画処理の時間を考慮して、0msec～最大1フレーム分のミリ秒だけ待ち、
        /// このフレームで遅延したミリ秒を（余った場合は実際に待ったミリ秒をマイナスの値で）返します。
        /// ※フレーム毎のループ処理の最後に呼び出してください。
        /// </summary>
        public int waitRestFrame・遅延を考慮して最大1フレーム分だけ待つ()
        {

            // 余った時間
            int _restMSec = getRestFrameMSec();
            // 遅延時間 = -余った時間
            int _delayMSec = -_restMSec;

            if (_restMSec < 0)
            {
                // 遅延しているので待たない
                // (a)０秒待つ処理
                // ■一回はDoEventsしないと画面が起き変わらないので、とりあえず待つ。
                //これいらない？なんか他の処理をまったら、getRestFrameMSecがループしてる。MyTools.wait_ByApplicationDoEvents();
                if (_delayMSec > 0)
                {
                    // 遅れは1フレーム以内である（→なんとか正常。つまり論理処理が1フレーム以上～2フレーム内である。）
                }
                else  if (_delayMSec <= p_fpsWaitMSec)
                {
                    // 遅れは1フレーム以内である（→警告。論理処理＋描画処理が1フレーム以上かかっている。）
                }
                else
                {
                    // 遅れは1フレーム以上である（→警告。論理処理＋描画処理が2フレーム以上かかっている。）
                }
            }else // if(_restMSec >= 0)
            {
                // (a)実際に待つ場合の処理
                if (_restMSec > p_fpsWaitMSec) _restMSec = p_fpsWaitMSec; // 1フレーム以上は待たない

                // ■最大1フレームだけ時間待つ処理。フォームを使った画面だとThread.Sleepだと画面が完全に止まってしまう。フォーム画面だと下記じゃないと無理っぽい。以下は元のコード。Thread.Sleep(_restMSec);	// SDL_Delayの置き換え
                MyTools.wait_ByApplicationDoEvents(_restMSec);
            }
            // waitRestFrameを最後に呼び出した時刻を更新
            p_lastWaitTime・最後にwaitRestFrameを呼び出した時刻 = getNowTimeMSec();

            // CPUの忙しさを調べるために、余り時間を記録（マイナスでもＯＫとする）
            p_aElapseTime[p_drawCount & 31] = _restMSec;


            return _delayMSec;
        }
        // これはWaitFrameを同じことをやっているから、要らなかった
        #region 以下、WaitFrameの別バージョンの実装の草案メモ
        ///// <summary>
        ///// 指定した絶対時間（ミリ秒）だけ待ちます。ゲームのフレームレートと同期、かつ前フレーム描画後の処理時間を考慮します。
        ///// 
        ///// ※正確な時間というよりも、一番近いフレーム数分だけ待つので、実際指定した時間よりも若干待ち時間が短くなることの方が多いです。
        ///// 　例えば、10msec待つとしても、FPSが50の場合、前回描画後の処理時間+10msec >= 20msecでなければ、待つ必要がないと判断します。
        ///// 　もちろん、その後もう一度同じ待ち時間が来て、10msecが2回続いたら、前回描画後の処理時間+10+10msec は 20msec より大きくなるので、その時は待ちます。
        ///// （p_isWait1Frame_longerがデフォルトfalseの場合。待ち時間を実際より長くしたい場合は、trueにしてください。）
        ///// </summary>
        //public void waitMSec・絶対時間ミリ秒間だけ待つ(int _絶対ミリ秒数)
        //{

        //    // (A)以下、前の処理時間を考慮せずに待つ場合
        //    //p_stopwatch・ストップウォッチ.Reset();
        //    //p_stopwatch・ストップウォッチ.Start();
        //    //while (p_stopwatch・ストップウォッチ.ElapsedMilliseconds < _絶対ミリ秒数)
        //    //{
        //    //    updateFrame・論理処理後のフレーム更新処理と描画処理スキップ判定();
        //    //}
        //    //p_stopwatch・ストップウォッチ.Stop();
        //    //p_stopwatch・ストップウォッチ.Reset();


        //    // (B)ゲームフレートと同期、かつ前のフレームの処理時間を考慮
        //    // ・・・waitFrame()で既にこれと似た処理を作ってくれてるから、作る必要なかったかもしれない。。
        //    int _1fpsMSec = (int)(1000.0f/p_fps);

        //    // 前回描画後の処理時間（前のフレーム描画から、どれくらいたったかを計算）
        //    int _processMSec = Math.Abs(getTime・現在時刻() - p_lastUpdateTime・最後にupdateFrameを呼び出した時刻);
        //    // (i)その処理時間が待ち時間より長い場合（遅延状態：最悪。遅延してるパターン）
        //    if(_processMSec >= _絶対ミリ秒数){
        //        //System.Console.WriteLine("CGameTime._restMSec: ●遅延状態…"+_processMSec+"MSecかかる処理がありました。待ちません。");
        //        // (i)待たない
        //        // なにも入れないでテストしてみた。じゃあ見た目ピジー状態になった。一フレームは待つ処理を入れておこう
        //        updateFrame・論理処理後のフレーム更新処理と描画処理スキップ判定();
        //    }
        //    else{
        //        // その処理時間が待ち時間より短い

        //        // (ii)その処理時間が1フレームより長い場合（警告状態：1フレームで処理しきれてないのパターンの検出）
        //        if (_processMSec >= _1fpsMSec){
        //            //System.Console.WriteLine("CGameTime._restMSec: 警告状態…"+_processMSec+"MSecかかる処理がありました。1フレームで処理しきれてません。");
        //        }

        //        // (ii-1)その処理時間+待ち時間が、1フレームより長い場合（正常状態：待ちます）
        //        if (_processMSec + _絶対ミリ秒数 >= _1fpsMSec)
        //        {
        //            // ■絶対ミリ秒間だけ待つ処理
        //            _stopwatchInner・内部腕時計.Reset();
        //            _stopwatchInner・内部腕時計.Start();
        //            while (_stopwatchInner・内部腕時計.ElapsedMilliseconds < _絶対ミリ秒数)
        //            {
        //                updateFrame・論理処理後のフレーム更新処理と描画処理スキップ判定();
        //                // [Q]なんかどこからもさわってないのにp_stopwatch・ストップウォッチ.IsRunning = falseになることがあるみたい
        //                if (_stopwatchInner・内部腕時計.IsRunning == false) break;
        //            }
        //            _stopwatchInner・内部腕時計.Stop();
        //            _stopwatchInner・内部腕時計.Reset();
        //        }
        //        else
        //        {
        //            // (ii-1)その処理時間+待ち時間が、1フレームより短い場合（スキップ状態：待ちません）
        //            //System.Console.WriteLine("CGameTime._restMSec: スキップ状態…"+(_processMSec+_絶対ミリ秒数)+"msecの処理は1フレーム以内なので待ちません。");
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// 現在の時間を高速に取得します。
        /// </summary>
        /// <returns></returns>
        public long getNowTimeMSec()
        {
            return _stopwatchInner・内部腕時計.ElapsedMilliseconds;// Environment.TickCount; // DateTime.Now.Ticks;やStopwatchよりこれが処理速度が速いらしい。参考:http://d.hatena.ne.jp/saiya_moebius/20100819/1282201466#20100819f1
        }

        public int getFPS・ゲームの最大フレームレートを取得(){
            return (int)p_fps;
        }
        public void setFPS・ゲームの最大フレームレートを変更(int _newFPS)
        {
            Fps = _newFPS;
            p_fps = _newFPS;
        }
        /// <summary>
        /// 現在の実測値FPS（最大フレームレートでなくて、実際に測定した値。基本はこれを画面に表示してどれくらい重くなっているかを調べることが出来る)を取得します。
        /// </summary>
        /// <returns></returns>
        public int getRealFPS・現在のFPS実測値を取得() { return RealFpsInt; }
        /// <summary>
        /// 文字列「FPS:描画**/論理○○（←○○は最大FPSと同値）　CPUの忙しさ:**％　描画スキップF数:**」を返します。
        /// 現在のFPS実測値、WaitFrameでSleepした時間から算出されたCPUの忙しさと、描画スキップされたフレーム数の実測値を示した情報を示した文字列を取得します。
        /// 
        /// 　　個別に取りたい場合は、getRealFPS、CpuPower、SkipFrameをそれぞれ参照してください。
        /// </summary>
        /// <returns></returns>
        public string getRealFPSInfo・現在のFPSやCPU負荷等を示す情報を取得()
        {
            string _info = "FPS:描画" + getRealFPS・現在のFPS実測値を取得() + "/論理" + getFPS・ゲームの最大フレームレートを取得();
            _info += "　CPUの忙しさ:" + (int)CpuPower + "％";
            _info += "　描画スキップF数:" + (int)SkipFrame;
            return _info;
        }



        // 以下、yaneSDKのFpsTimerの記述。一部、名前やWaitFrameの内部処理など、merusaiaが変更。

		/// <summary>
        /// FPS(ディフォルトで0（停止）にしているが、コンストラクタでは必ず指定しなければならないとしている)
		/// </summary>
		private float	p_fps = 0;
		/// <summary>
        /// 1000/FPS; // 指定FPSに基づくウェイト時間 [ms]単位
		/// </summary>
		private int p_fpsWaitMSec;					
		/// <summary>
        /// 前回の描画時刻。最後に描画された時間（getTime・現在時刻()で取得したミリ秒）です。画面が固定されるフレーム単位のゲーム時間の計測などに使います。
        /// </summary>
		private long p_lastDrawTime;

		/// <summary>
        /// FPS測定用の描画時間計算用
		/// </summary>
		private long[]	p_aDrawTime = new long[32];
		/// <summary>
        /// CPU Power測定用
		/// </summary>
		private float[]	p_aElapseTime = new float[32];

		/// <summary>
        /// 描画した回数。WaitFrameを呼び出された回数とは厳密には違い、描画がスキップされた場合p_drawCountの方が少ない可能性がある。
		/// </summary>
		private int p_drawCount;
		/// <summary>
        /// 次のフレームはスキップするのか？
		/// </summary>
		private bool	p_bFrameSkip;
		/// <summary>
        /// フレームスキップカウンタ
		/// </summary>
		private int	p_frameSkipCount;
		/// <summary>
        /// 計測中のフレームスキップカウンタ
		/// </summary>
		private int	p_frameSkipCountNow;

		/// <summary>
		/// フレームスキップ10回に1回は強制描画する。そのためのカウンタ。
		/// </summary>
		private int p_continualFrameSkipCount;

        //private Timer timer;

        // 以下、やねうらお様によるFpsTimerクラスの説明。思考錯誤も乗っててわかりやすい。
        // CGameTimer・ゲームタイマーは、FpsTimerを改良して名前をfpsManagerに変えたもの。
        // （単独で定期スレッドを実行しているわけではないので、名前はタイマーより「管理者」の方が適切だと判断）

        /// <summary>
        /// 時間を効率的に待つためのタイマー。
        /// </summary>
        /// <remarks>
        /// フレームレート（一秒間の描画回数）を指定FPS（Frames Par Second）に
        /// 調整する時などに使う。
        /// 
        /// while (true){
        ///		gametimer.waitFrame();
        ///		if(!gametimer.toBeSkip()) Draw();
        /// }
        /// と書けば、1秒間にsetFPSで設定された回数だけDraw関数が呼び出される
        /// </remarks>
        /**
            // Game用のthreadを作らずにcallbackで書く場合
            // threadを作っても良いのだが、生成スレッドと異なると
            // FormのControlにアクセスできなくなってしまうので、あまり得策ではない。
            public Form1()
            {
                InitializeComponent();

                this.timer1.Interval = 3; // 3msごとにcallback
                this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
                this.timer1.Start();
                timer.setFps(60);
            }

            System.Windows.Forms.Timer interval_timer = new System.Windows.Forms.Timer();
            draft_CFpsManager・フレーム管理者 timer = new draft_CFpsManager・フレーム管理者();

            private void timer1_Tick(object sender, EventArgs _e)
            {
                if (!timer.toBeRendered())
                    return;

                myRender();
            }
         */
        /*
         * 　以下、threadを作って描画する例

            void MyThread()
            {
                while ( isValid )
                {
                    if ( p_fps.ToBeRendered )
                    {
                        if ( !p_fps.ToBeSkip )
                        {
                            window.Screen.Select();
                            window.Screen.Clear();
                            window.Screen.Blt(txt , x++ % 300 , 0);
                            fpslayer.OnDraw(window.Screen , 100 , 40);
                            window.Screen.Update();
                        //	UserControl1 u = this.Controls[0] as UserControl1;
                        //	u.textBox1.Text = "ABC"; // これダメ。threadが違うのでアクセスできない
                        }
                    }
                    else
                        Thread.Sleep(1);
		
            //　あるいは、
	 
                    p_fps.WaitFrame();
                    {
                        if (! p_fps.ToBeSkip )
                        {
                            window.Screen.Select();
                            window.Screen.Clear();
                            window.Screen.Blt(txt , x++ % 300 , 0);
                            fpslayer.OnDraw(window.Screen , 100 , 40);
                            window.Screen.Update();
                        }
                    }
                }
        */
        /// <summary>
        /// FPS値の設定（イニシャライズを兼ねる）と取得。
        /// ディフォルトでは50fps。0にするとnon-wait mode(FPS = ∞)
        /// </summary>
        /// <param name="p_fps"></param>
        public float Fps
        {
            set
            {
                p_lastDrawTime = getNowTimeMSec(); // 前回描画時間は、ここで設定
                p_bFrameSkip = false;
                p_frameSkipCount = 0;
                p_frameSkipCountNow = 0;
                p_drawCount = 0;

                this.p_fps = value;
                if (value == 0)
                {	// non-wait mode
                    return;
                }
                // １フレームごとに何ms待つ必要があるのか？[ms]
                p_fpsWaitMSec = (int)(1000 / p_fps);

            }
            get
            {
                return p_fps;
            }
        }

        /// <summary>
        /// FPSの取得（測定値）
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 1秒間に何回WaitFrameを呼び出すかを、
        /// 前回32回の呼び出し時間の平均から算出する。
        /// </remarks>
        public float RealFps
        {
            get
            {
                if (p_drawCount < 16) return 0; // まだ16フレーム計測していない
                if (p_drawCount < 32)
                {
                    float t = p_aDrawTime[(p_drawCount - 1)]	// 前回時間
                          - p_aDrawTime[(p_drawCount - 16)];	// 15回前の時間
                    if (t == 0)
                    {
                        return 0;	//	測定不能
                    }
                    return (1000 * 15.0f) / t;
                    // 平均から算出して値を返す（端数は四捨五入する）
                }
                else
                {
                    float t = p_aDrawTime[(p_drawCount - 1) & 31]	 // 前回時間
                          - p_aDrawTime[(p_drawCount) & 31];	 // 31回前の時間
                    if (t == 0)
                    {
                        return 0;	//	測定不能
                    }
                    return (1000 * 31.0f) / t;
                }
            }
        }

        /// <summary>
        /// FPSの取得(測定値)
        /// </summary>
        /// <remarks>
        /// getRealFpsの戻り値はfloatなので、こちらは、小数点以下を四捨五入して返すメソッド。
        /// </remarks>
        /// <returns></returns>
        private int RealFpsInt { get { return (int)(RealFps + 0.5); } }
        


        /// <summary>
        ///  CPU稼動率の取得（測定値）
        /// </summary>
        /// <remarks>
        ///	ＣＰＵの稼働率に合わせて、0～100の間の値が返る。
        ///	ただしこれは、WaitFrameでSleepした時間から算出されたものであって、
        /// あくまで参考値である。
        /// 最新の16フレーム間で余ったCPU時間から計測する。16フレーム経過していない場合は100が返る。
        /// ただし、ここで言うフレームとは、waitFrameの呼び出しごとに１フレームと計算。
        /// </remarks>
        /// <returns></returns>
        public float CpuPower
        {
            get
            {
                if (p_drawCount < 16) return 100; // まだ16フレーム計測していない

                float t = 0;
                for (int i = 0; i < 16; i++)
                    t += p_aElapseTime[i]; // ここ16フレーム内でFPSした時間
                // return 1-_nowTime/(1000*16/m_dwFPS)[%] ; // FPSノルマから算出して値を返す

                float w = 100 - (t * p_fps / 160);
                if (w < 0) w = 0;
                return w;
            }
        }

        /// <summary>
        /// スキップされたフレーム数を取得
        /// </summary>
        /// <remarks>
        ///		setFpsされた値までの描画に、ToBeSkipがtrueになっていた
        ///		フレーム数を返す。ただし、ここで言うフレーム数とは、
        ///		waitFrameの呼び出しごとに１フレームと計算。
        /// </remarks>
        /// <returns></returns>
        public float SkipFrame
        {
            get { return p_frameSkipCount; }

        }


        /// <summary>
        /// resetする。
        /// </summary>
        /// <remarks>
        /// インスタンス生成後、リソースを読み込み、そのあとゲームを
        /// 開始した場合、本来描画すべき時間から経過しているため、このタイマーは
        /// 処理落ちしていると判断して、描画をskipしてしまう。
        /// 
        /// そこで、ゲームがはじまる直前にResetを行ない、このタイマーが処理落ちしている
        /// と判定しないようにする必要がある。
        /// </remarks>
        public void Reset()
        {
            p_lastDrawTime = getNowTimeMSec();
        }


        /// <summary>
        /// 今回のフレームの論理処理が、描画処理をスキップした方がいい位、遅延しているかを調べる。
        /// 返り値にこのフレームでの余り時間を（遅延している場合はマイナスの値を）ミリ秒で返す。
        /// 
        /// 　■merusaiaが変更。現在はこのメソッドはprivate。外部では、このメソッドを呼び出す他のpublicメソッドを使ってください。
        /// 　このメソッドは実際には待たないので、ほんとはCheckFrameという名前の方が適切だが、
        /// 　YaneSDKとの変更点を明確にしたいため、なるべく元の名前を入れるようにしている。
        /// 
        /// 
        /// // ※このメソッドは、描画処理の時間を考慮できていない（描画処理は一瞬（0ミリ秒）で終わると解釈して待ち時間を計算している）。
        /// なので、このメソッドでは待たず、ラッパーしたupdateFrameメソッドで描画スキップを確認し、
        /// 描画処理を行った後、描画処理の時間を考慮した待ち処理をwaitRestFrameメソッドを呼び出すようにしている。
        /// </summary>
        /// <remarks>
        ///	メインループのなかでは、描画処理を行なったあと、
        ///	waitRestFrameメソッドを呼び出せば、setFPSで設定した
        ///	フレームレートに自動的に調整される。
        /// 
        /// 使い方は、toBeSkipメンバも参照すること。
        /// </remarks>
        private int WaitFrame()
        {
            int _restMSec = 0; // 返り値のこのフレームで余っているミリ秒。範囲は-p_fpsWaitMSec～p_fpsWaitMSec; // 最大は１フレームミリ秒。
            long _nowTime = getNowTimeMSec(); // 現在時刻

            // fpsが0だったらフレーム停止中。
            if (p_fps == 0)
            {
                p_aElapseTime[p_drawCount & 31] = 0;
                p_lastDrawTime = _nowTime;
                p_aDrawTime[p_drawCount & 31] = p_lastDrawTime;  // Drawした時間を記録することでFPSを算出する手助けにする
                p_bFrameSkip = false;
                p_fpsWaitMSec = 1000000; // とりあえず大きな値にしておく。
                _restMSec = (int)p_fpsWaitMSec;
                return _restMSec; // 遅延していないがフレーム停止中。
            }

            // １フレームごとに何ms待つ必要があるのか、を一応毎回更新
            p_fpsWaitMSec = (int)(1000 / p_fps); // 0割しないようにね
            // とりあえず初期値として、全部余っているとする。
            _restMSec = p_fpsWaitMSec;

            //	スキップレートカウンタをリセットするか
            if (p_fps != 0 && ((p_drawCount % (int)p_fps) == 0))
            {
                p_frameSkipCount = p_frameSkipCountNow;
                p_frameSkipCountNow = 0;
            }

            // ちゃんと１フレームミリ秒justで待った後、一瞬（0ミリ秒）で描画したとして、描画完了時刻を計算
            // (a)こうすると、実際に描画時刻が更新されてしまう。推定したいだけなのでは？
            //p_lastDrawTime += p_fpsWaitMSec;
            //float _restMSec = _nowTime - p_lastDrawTime;
            // (b)描画完了時刻を前もって推定する
            long _drawFinishTime = p_lastDrawTime + p_fpsWaitMSec;
            // 前もって計算した描画完了時刻は、現在の時刻からどの程度離れているかを計算する
            //    _restMSec = 遅延してなかったらこっちの方が少ないはず　- (普通はこっちの方が多い）
            float delay = _nowTime - _drawFinishTime;               //こうすると、実際に描画時刻が更新されてしまう。推定したいだけなのでは？ float _restMSec = _nowTime - p_lastDrawTime;
            // 実際の余り時間を計算して代入する。（余っている時間 = -1*遅れている時間）
            _restMSec = (int)-delay; // このフレームで余っているミリ秒
            if (delay < 0)
            {
                // 時間が余っている（→正常。つまり論理処理が0～1フレーム以内である。）
                p_bFrameSkip = false;

                // (b)ここでは待たない。描画処理を行った後に呼び出す、waitRestFrameで待つ。
                
                // (a)実際に待つ場合の処理
                //if (_restMSec > p_fpsWaitMSec) _restMSec = (int)p_fpsWaitMSec; // 1フレーム以上は待たない（merusaiaが念のため修正）
                // ■最大1フレームだけ時間待つ処理。フォームを使った画面だとThread.Sleepだと画面が完全に止まってしまう。フォーム画面だと下記じゃないと無理っぽい。以下は元のコード。Thread.Sleep(_restMSec);	// SDL_Delayの置き換え
                //MyTools.wait_ByApplicationDoEvents(_restMSec);
            }
            else if (delay <= p_fpsWaitMSec * 1)
            {
                // 遅れは1フレーム以内である（→なんとか正常。つまり論理処理が1フレーム以上～2フレーム内である。）
                p_bFrameSkip = false;
            }
            else // if ( _restMSec > fpsWait * 1 ) 
            {
                // 遅れは1フレーム以上である（→警告。論理処理が2フレーム以上かかっている。つまり描画処理をスキップした方がいい）

                // 1フレーム分は間違いなく時間が
                // 足りてないのでフレームスキップしたほうがいい
                p_bFrameSkip = true;

                //	しかし、一方にまったく描画無しだとフリーズ（暴走）しているのかと思われると嫌（癪）なので、
                //  4フレに1回は強制的に描画する。
                if (++p_continualFrameSkipCount == 4)
                {
                    p_bFrameSkip = false;
                    p_continualFrameSkipCount = 0;
                    p_lastDrawTime = _nowTime; // 今回描画するのでタイマをもとに更新する。
                }
            }

            if (p_bFrameSkip)
            {
                // 描画をスキップする場合
                p_frameSkipCountNow++;
                // こうすると、すっ気ぷした時でさえ描画時間が更新されてしまう。p_lastDrawTime += p_fpsWaitMSec; // 描画時間を進める
            }
            else
            {
                // 描画をスキップしない場合
                p_aDrawTime[p_drawCount & 31] = p_lastDrawTime;  // Drawした時間を記録することでFPSを算出する手助けにする
                if (++p_drawCount == 64)
                    p_drawCount = 32;
                // 32に戻すことによって、0～31なら、まだ32フレームの描画が終わっていないため、
                // FPSの算出が出来ないことを知ることが出来る。

                // CPUの忙しさを調べるために、余り時間を記録（マイナスでもＯＫとする）
                //ここでやらずwaitRestFrameでやるp_aElapseTime[p_drawCount & 31] = _restMSec;
            }
            // 余り時間を返す（遅延しているならマイナスの値で返す）
            return _restMSec;
        }
        /// <summary>
        /// 今回のフレームで、現在どれ位フレームの時間が余っているかを（遅延している場合はマイナスの値を）ミリ秒で返す。
        /// 基本的に、waitRestFrameメソッドが呼び出します。
        /// </summary>
        private int getRestFrameMSec()
        {
            int _restMSec = 0; // 返り値のこのフレームで余っているミリ秒。範囲は、-遅延時間～p_fpsWaitMSec; // 最大は１フレームミリ秒。
            long _nowTime = getNowTimeMSec(); // 現在時刻
            // ■時間の流れでは、updateFrame（WaitFrameを内部呼び出し） →　描画処理　→　waitRestFrame(このメソッドを内部呼び出し　→　今　） →　次のフレームのupdateFrame
            // ●最後にupdateFrameを呼び出した時刻から、現在の時刻で、実際の描画処理にかかった時間を計算
            long _NowdrawMSec = _nowTime - p_lastUpdateTime・最後にupdateFrameを呼び出した時刻;
            // ●最後にwaitFrameを呼び出した時刻から、最後にupdateFrameを呼び出した時刻、実際の論理処理にかかった時間を計算 // またp_lastWaitFrameTimeは更新してないからいける
            long _NowlogicMSec = p_lastUpdateTime・最後にupdateFrameを呼び出した時刻 - p_lastWaitTime・最後にwaitRestFrameを呼び出した時刻;

            // ●余った時間を取得
            //  余った時間 =  (１フレームのミリ秒) - かかった時間
            _restMSec = p_fpsWaitMSec - (int)(_NowlogicMSec + _NowdrawMSec);
            // どっちかが更新されていない場合、0
            if (p_lastUpdateTime・最後にupdateFrameを呼び出した時刻 == 0 || p_lastWaitTime・最後にwaitRestFrameを呼び出した時刻 == 0)
            {
                _restMSec = 0;
            }
            if (_restMSec > 0)
            {
                // 時間が余っている
                if (_restMSec > p_fpsWaitMSec) _restMSec = p_fpsWaitMSec; // 変な大きな時間が入っていることがあるので、その時は修正
                //int a = 0;
            }
            //else if (-_restMSec <= p_fpsWaitMSec)
            //{
            //    // 遅れは1フレーム以内である（→警告。つまり論理処理＋描画処理が1フレーム以上～2フレーム内である。）
            //    int a = _restMSec;
            //}
            //else // if ( -_restMSec > fpsWait * 1 ) 
            //{
            //    // 遅れは1フレーム以上である（→やばい遅延。論理処理＋描画処理が2フレーム以上かかっている。）
            //    int a = _restMSec;

            //}
            return _restMSec;
        }
        /// <summary>
        /// 今回のフレームで、描画処理にかかった時間を取得します。描画処理が終わった後、かつwaitRestFrameが終わった後に呼び出してください。
        /// </summary>
        public int getDrawMSec・今回のフレームで描画処理にかかった時間を取得()
        {
            // 最後にupdateFrameを呼び出した時刻から、waitRestFrameを呼び出した時刻までで、実際に描画処理にかかった時間を計算
            // ■時間の流れでは、updateFrame（WaitFrameを内部呼び出し） →　描画処理　→　waitRestFrame(） →　今　→　次のフレームのupdateFrame
            int _drawMSec = (int)(p_lastWaitTime・最後にwaitRestFrameを呼び出した時刻 - p_lastUpdateTime・最後にupdateFrameを呼び出した時刻);
            // どちらかが１回も更新されていない場合は0
            if (p_lastUpdateTime・最後にupdateFrameを呼び出した時刻 == 0 || p_lastWaitTime・最後にwaitRestFrameを呼び出した時刻 == 0)
            {
                _drawMSec = 0;
            }
            if (_drawMSec < 0)
            {
                // 何かがおかしい。p_lastFrameTimeが更新されていない可能性がある
                int a = 0;
            }
            //else if (_drawMSec <= p_fpsWaitMSec)
            //{
            //    // 描画処理は１フレーム以内に終了している。
            //}
            //else
            //{
            //    // 描画処理に１フレーム以上かかっている
            //}
            return _drawMSec;
        }
        /// <summary>
        /// 今回のフレームで、論理処理にかかった時間を取得します。論理処理がが終わった後、かつupdateFrameが終わった後から、waitRestFrameを呼び出す前までに呼び出してください。
        /// </summary>
        public int getLogicMSec・今回のフレームで論理処理にかかった時間を取得()
        {
            // 今回のフレームの開始時刻（前回のwaitRestFrameを呼び出した時刻）から、updateFrameを呼び出した時刻までで、実際の論理処理にかかった時間を計算
            // ■時間の流れでは、前回のwaitRestFrame() → 論理処理 → updateFrame（WaitFrameを内部呼び出し） →　描画処理　→　waitRestFrame(このメソッドを内部呼び出し　→　今　） →　次のフレームのupdateFrame
            int _LogicMSec = (int)(p_lastUpdateTime・最後にupdateFrameを呼び出した時刻 - p_lastWaitTime・最後にwaitRestFrameを呼び出した時刻);
            // どちらかが１回も更新されていない場合は0
            if (p_lastUpdateTime・最後にupdateFrameを呼び出した時刻 == 0 || p_lastWaitTime・最後にwaitRestFrameを呼び出した時刻 == 0)
            {
                _LogicMSec = 0;
            }
            if (_LogicMSec < 0)
            {
                // 何かがおかしい。p_lastFrameTimeが更新されていない可能性がある
                int a = 0;
            }
            //else if (_LogicMSec <= p_fpsWaitMSec)
            //{
            //    // 論理処理は１フレーム以内に終了している。
            //}
            //else
            //{
            //    // 論理処理に１フレーム以上かかっている
            //}
            return _LogicMSec;
        }

        /// <summary>
        /// 描画処理が可能かどうかを調べる。
        /// 
        /// C#でフォームを書いていると、数msごとにevent callbackをかけて、
        /// そのcallback先のハンドラで、一定のFPSで描画を行ないたいことがある。
        /// このメソッドは、それを実現する。
        /// </summary>
        /// <remarks>
        /// 
        /// 必ず以下のように書くべし！
        ///		timer = new System.Windows.Forms.Timer();
        ///		timer.Interval = 1;
        ///		timer.Tick += delegate { OnCallback(); };
        ///		timer.Start();
        ///
        ///
        ///		// OnDrawのCallback用タイマ
        ///		private global::System.Windows.Forms.Timer timer;
        ///
        ///		public void OnCallback()
        ///		{
        ///			if ( !gameContext.FPSTimer.ToBeRendered )
        ///				return;
        ///
        ///			// フレームスキップ処理
        ///			OnMove(); // 論理的な移動 
        ///			if ( gameContext.FPSTimer.ToBeSkip ){
        ///				return; 
        ///			}
        ///			OnDraw(); // 画面描画
        ///		}
        /// </remarks>
        /// <returns></returns>
        public bool ToBeRendered
        {
            get
            {
                // 以下のソースはwaitFrameからの改変

                // これある日突然マイナスになるので本当はまずいんだが、
                // そのときは一瞬コマ落ちするだけなのでいいか…。
                long t = getNowTimeMSec(); // 現在時刻
                if (t - p_lastDrawTime < p_fpsWaitMSec)
                {
                    // 時間あまっちょるのでまだ描画しない。
                    return false;
                }

                try
                {
                    // justで描画したとして計算する
                    p_lastDrawTime += p_fpsWaitMSec;

                    // かなり厳粛かつ正確かつ効率良く時間待ちをするはず。
                    if (p_fps == 0)
                    {
                        p_aElapseTime[p_drawCount & 31] = 0;
                        p_bFrameSkip = false;
                        return true; // Non-wait mode
                    }

                    float delay = t - p_lastDrawTime;
                    if (delay <= p_fpsWaitMSec)
                    {
                        p_bFrameSkip = false;
                        p_aElapseTime[p_drawCount & 31] = p_fpsWaitMSec - delay;
                    }
                    else if (delay < p_fpsWaitMSec * 2)
                    {	// 時間足りてないので、時間消費する必要なし
                        p_bFrameSkip = false;
                        p_aElapseTime[p_drawCount & 31] = 0;
                    }
                    else
                    {	// 1フレーム分、時間足りてないのでフレームスキップしたほうがいいのだが
                        p_bFrameSkip = true;
                        p_aElapseTime[p_drawCount & 31] = 0;

                        //	まったく描画無しだと暴走しているのかと思われると癪なので、
                        //  4フレに1回は強制的に描画する。
                        if (++p_continualFrameSkipCount == 4)
                        {
                            p_bFrameSkip = false;
                            p_continualFrameSkipCount = 0;

                            p_lastDrawTime = t; // どうしようもないので時間を現在に
                        }
                    }

                    if (p_bFrameSkip)
                    {
                        p_frameSkipCountNow++;
                    }
                    return true;
                }
                finally
                {
                    if (!p_bFrameSkip)
                    {
                        //	スキップレートカウンタ
                        if (p_fps != 0 && ((p_drawCount % (int)p_fps) == 0))
                        {
                            p_frameSkipCount = p_frameSkipCountNow;
                            p_frameSkipCountNow = 0;
                        }

                        // p_fps == 0は描画扱いなので描画時刻を記録する必要がある。
                        p_aDrawTime[p_drawCount & 31] = p_lastDrawTime;  // Drawした時間を記録することでFPSを算出する手助けにする
                        if (++p_drawCount == 64)
                            p_drawCount = 32;
                        // 32に戻すことによって、0～31なら、まだ32フレームの描画が終わっていないため、
                        // FPSの算出が出来ないことを知ることが出来る。
                    }
                }
            }
        }

        /// <summary>
        /// スキップすべきかを示すフラグを返す。
        /// </summary>
        /// <remarks>
        /// while (true){
        ///		gametimer.waitFrame();
        ///		if(!gametimer.toBeSkip) Draw();
        /// }
        /// と書けば、1秒間にsetFPSで設定された回数だけDraw関数が呼び出される
        /// </remarks>
        /// <returns></returns>
        public bool ToBeSkip
        {
            get { return p_bFrameSkip; }
        }


    }
}
    
