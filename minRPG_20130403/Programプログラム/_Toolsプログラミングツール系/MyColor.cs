using System;
using System.Collections.Generic;
using System.Text;
// Colorクラスに依存している
using System.Drawing;

using PublicDomain;

namespace PublicDomain
{
    /// <summary>
    /// よく使う色や、汎用的に使いたい色を登録できる、カラーパレットの列挙体です。
    /// 
    /// 新しく要素を追加した場合、プログラム中やstaticで取得したければ、同時にデフォルト値をgetやgetEColorで設定してください。
    /// 逆に、ゲーム中や、他のクラスだけで取得したければ、setEColor(int型)だけで登録してください。その場合はここに書く必要はありません。
    /// </summary>
    public enum EColor・カラーパレット
    {
        // 予め登録されている色（基本的に外部で変更されない）
        c01_赤,
        c02_橙,
        c03_黄,
        c04_緑,
        c05_青,
        c06_紫,

        c07_赤橙,
        c08_黄橙,
        c09_黄緑,
        c10_青緑,
        c11_青紫,
        c12_赤紫,

        c13_白,
        c14_黒,
        c15_銀,
        c16_金,
        c17_透明,
        c18_虹色,

        clast_staticに取れる色の数,



        // 後で登録できる色（外部で変更される可能性がある色）
        a00_透明色,
        a01_背景色,
        a02_文字フォント色,
        a03_文字背景色,
        a11_システム色Ａ,
        a12_システム色Ｂ,
        a13_システム色Ｃ,


        b20_デフォルトキャラ色,
        b21_味方キャラ側色,
        b22_敵キャラ側色,
        b23_中立キャラ側色,


        e00_エディット無し,
        e01_エディットＡ,
        e02_エディットＢ,
        e03_エディットＣ,
        e04_エディットＤ,
        e05_エディットＥ,
        // こんな風に適当に追加してください。


        // プログラム中やstaticでも参照したければ、同時にデフォルト値をstaticメソッドのgetやgetEColorで設定してください。
        // ゲーム中や、他のクラスだけで参照したければ、setEColor(int型)だけで登録してください。その場合はここに書く必要はありません。

        Count, // これで要素数が取れる
        
    }

    /// <summary>
    /// よく使う色や、汎用的に使いたい色を登録できる、カラーパレットEColor列挙体を使って色クラスColorを取得するクラスです。
    /// </summary>
    public class MyColor
    {
        /// <summary>
        /// 汎用的に使いたい色をsetで登録したColorを格納するリストです。
        /// </summary>
        private List<Color> p_setColors;
        /// <summary>
        /// 汎用的に使いたい色をsetで登録したColorのＩＤ（EColorのint型）を格納するリストです。p_setColorsと一緒に追加・編集します。
        /// </summary>
        private List<int> p_setColorIDs;


        /// <summary>
        /// 指定したEColorの色を取得します。staticメソッドとして使いたい場合は、ここだけを使ってください。
        /// 指定したIdの色が無い場合Color.Whiteが返ります。
        /// なお、引数をint型で渡すと間違えやすいので、できれば引数をEColor型で渡すもう一つの同名メソッドを使ってください。
        /// </summary>
        /// <param name="_EColor"></param>
        /// <returns></returns>
        public static Color get_byId(int _EColorId)
        {
            //EColor・カラーパレット _EColor = MyTools.getEnumItem_FromIndexOrValue<EColor・カラーパレット>(_EColorId);
            // MyToolsに依存したくないので、自前で書く

            // Enum要素が格納しているインデックスもしくは値（int型）から、Enum型（独自の列挙型）の要素を取ってきます．全ての要素配列Arrayを生成するため、ちょっと手間がかかります。
            EColor・カラーパレット _EColor = EColor・カラーパレット.a00_透明色;
            foreach (EColor・カラーパレット _value in Enum.GetValues(typeof(EColor・カラーパレット)))
            {
                if ((int)_value == _EColorId)
                {
                    _EColor = _value;
                    break;
                }
                //メンバ名を取得する
                //string name = Enum.GetName(typeof(EColor・カラーパレット), _value);
                //メンバの値と名前を表示する
                //Console.WriteLine("{0} = {1}", name, (int)_value);
            }
            return get(_EColor);
        }
        /// <summary>
        /// 指定したEColorの色を取得します。staticメソッドとして使いたい場合は、ここだけを使ってください。
        /// </summary>
        /// <param name="_EColor"></param>
        /// <returns></returns>
        public static Color get(EColor・カラーパレット _EColor)
        {
            // デフォルトの場合は白が入る
            Color _c = Color.White;

            // 予め登録されている色（基本的に外部で変更されない）
            if (_EColor == EColor・カラーパレット.c01_赤) _c = Color.Red;
            else if (_EColor == EColor・カラーパレット.c02_橙) _c = Color.Orange;
            else if (_EColor == EColor・カラーパレット.c03_黄) _c = Color.Yellow;
            else if (_EColor == EColor・カラーパレット.c04_緑) _c = Color.Green;
            else if (_EColor == EColor・カラーパレット.c05_青) _c = Color.Blue;
            else if (_EColor == EColor・カラーパレット.c06_紫) _c = Color.Violet;
            else if (_EColor == EColor・カラーパレット.c07_赤橙) _c = Color.OrangeRed;
            else if (_EColor == EColor・カラーパレット.c08_黄橙) _c = Color.Orchid; // ?
            else if (_EColor == EColor・カラーパレット.c09_黄緑) _c = Color.YellowGreen;
            else if (_EColor == EColor・カラーパレット.c10_青緑) _c = Color.AliceBlue; //?
            else if (_EColor == EColor・カラーパレット.c11_青紫) _c = Color.BlueViolet;
            else if (_EColor == EColor・カラーパレット.c12_赤紫) _c = Color.MediumVioletRed;
            else if (_EColor == EColor・カラーパレット.c13_白) _c = Color.White;
            else if (_EColor == EColor・カラーパレット.c14_黒) _c = Color.Black;
            else if (_EColor == EColor・カラーパレット.c15_銀) _c = Color.Silver;
            else if (_EColor == EColor・カラーパレット.c16_金) _c = Color.Gold;
            else if (_EColor == EColor・カラーパレット.c17_透明) _c = Color.Transparent; //透明度だから完全透明とはちょっと違うけど適当に
            else if (_EColor == EColor・カラーパレット.c18_虹色) _c = getRainbowRandomColor・呼び出す毎に色が変わるランダムな虹色に近い色(); //ランダムに変化する虹色を
            else if (_EColor == EColor・カラーパレット.clast_staticに取れる色の数) _c = Color.White; //無いからデフォルトで



            // 後で登録できる色（外部で変更される可能性がある色。値はデフォルト値）
            
            else if (_EColor == EColor・カラーパレット.a00_透明色) _c = Color.Transparent;
            else if (_EColor == EColor・カラーパレット.a01_背景色) _c = Color.Blue;
            else if (_EColor == EColor・カラーパレット.a02_文字フォント色) _c = Color.White;
            else if (_EColor == EColor・カラーパレット.a03_文字背景色) _c = Color.DeepSkyBlue;
            else if (_EColor == EColor・カラーパレット.a11_システム色Ａ) _c = Color.Blue;
            else if (_EColor == EColor・カラーパレット.a12_システム色Ｂ) _c = Color.White;
            else if (_EColor == EColor・カラーパレット.a13_システム色Ｃ) _c = Color.White;

            else if (_EColor == EColor・カラーパレット.b20_デフォルトキャラ色) _c = Color.White;
            else if (_EColor == EColor・カラーパレット.b21_味方キャラ側色) _c = Color.DeepSkyBlue;
            else if (_EColor == EColor・カラーパレット.b22_敵キャラ側色) _c = Color.Blue;
            else if (_EColor == EColor・カラーパレット.b23_中立キャラ側色) _c = Color.White;

            //無しでもデフォルトが返るelse if (_EColor == EColor・カラーパレット.e00_エディット無し) _chara = Color.White;
            else if (_EColor == EColor・カラーパレット.e01_エディットＡ) _c = Color.White;


            return _c;
        }


        /// <summary>
        /// 今まで登録した色を取得します。
        /// </summary>
        /// <param name="_EColor"></param>
        /// <returns></returns>
        public Color getEColor(EColor・カラーパレット _EColor)
        {
            return getColor((int)_EColor);
        }
        /// <summary>
        /// 今まで登録した色を取得します。EColorに列挙されていないIdも取得可能です。
        /// </summary>
        /// <param name="_EColor"></param>
        /// <returns></returns>
        public Color getColor(int _EColorId)
        {
            // 見つからなかった場合や、デフォルトは白
            Color _c = Color.White;
            // 一応、まずはstaticに取る（後で変更・登録されたものがあれば、上書きされる）
            //if ((int)_setEColor <= (int)EColor・カラーパレット.clast_staticに取れる色の数)
            _c = get_byId(_EColorId);

            // 後で変更・登録されたものがあれば、それを追加
            if (p_setColorIDs.Contains(_EColorId))
            {
                // 2つのリストのインデックスは同じとして扱っているので、該当の配列のColorクラスを取ってくる
                int _id = p_setColorIDs.IndexOf(_EColorId);
                _c = p_setColors[_id];
            }
            return _c;
        }
        /// <summary>
        /// 新しく色を登録します。EColorに列挙されていないIdも登録可能です。
        /// </summary>
        /// <param name="_setEColor"></param>
        /// <param name="_Color"></param>
        public void setNewColor(int _setEColorId, Color _Color)
        {
            if (_Color != null)
            {
                p_setColors.Add(_Color);
                p_setColorIDs.Add(_setEColorId);
            }
        }
        /// <summary>
        /// 新しく色を登録します。
        /// </summary>
        /// <param name="_setEColor"></param>
        /// <param name="_Color"></param>
        public void setNewEColor(EColor・カラーパレット _setEColor, Color _Color)
        {
            setNewColor((int)_setEColor, _Color);
        }

        #region ○銀色・金色関連メソッド
        /// <summary>
        /// 引数の色を、○銀色のように色の明度を変えてランダムに取って来ます。
        /// （黄色だと金色、青色だと青銀色のように。何度も呼び出して色を変更すると、光っているように見えるかも？）
        /// </summary>
        public Color getBrightingRandomColor・呼び出すごとに光り出すランダムに銀色っぽく輝く色(EColor・カラーパレット _EColor)
        {
            return getBrightingRandomColor・呼び出すごとに光り出すランダムに銀色っぽく輝く色(getEColor(_EColor));
        }
        /// <summary>
        /// 引数の色を、○銀色のように色の明度を変えてランダムに取って来ます。
        /// （黄色だと金色、青色だと青銀色のように。何度も呼び出して色を変更すると、光っているように見えるかも？）
        /// </summary>
        public static Color getBrightingRandomColor・呼び出すごとに光り出すランダムに銀色っぽく輝く色(Color _color)
        {
            Random _random = new Random();
            int _max = 10;
            int _randomNum = _random.Next(0, _max); // 0-9の乱数
            HslColor _colorB = HslColor.FromRgb(_color);
            // 輝度Lだけを変更して光っているように見せる
            _colorB.L = _colorB.L+(float)(_randomNum/10f);

            // Color型に戻す
            Color _returnColor = HslColor.ToRgb(_colorB);
            return _returnColor;
        }
        #endregion


        #region 虹色作成関連メソッド
        /// <summary>
        /// 虹色のように鮮やかな色をランダムに取って来ます。
        /// （何度も呼び出して色を変更すると、虹色に輝いているように見えるかも？）
        /// </summary>
        /// <returns></returns>
        public static Color getRainbowRandomColor・呼び出す毎に色が変わるランダムな虹色に近い色()
        {
            Random _random = new Random();
            int _colorNum = 256;
            int _randomNum = _random.Next(0, _colorNum); // 0-255の乱数
            return getRainbowLike256Colors()[_randomNum];
        }

        // 以下、虹色のようなカラーパレット作成関連メソッド http://www.atmarkit.co.jp/bbs/phpBB/viewtopic.php?topic=35337&forum=7
        /// <summary>
        /// 虹色のようなカラーパレットを返します。
        /// [詳細]
        /// 赤→橙→黄→緑→青と徐々に変化する256個のカラーパレット（Colorクラスの配列）を返します。
        /// </summary>
        /// <returns></returns>
        public static Color[] getRainbowLike256Colors()
        {
            Color[] _ans = new Color[256];
            // 青→緑
            for (int i = 0; i < 64; i++)
            {
                int _green = i * 4;
                _ans[i] = Color.FromArgb(255, 0, _green, 255 - _green);
            }
            // 緑→黄
            for (int i = 0; i < 64; i++)
            {
                int _red = i * 4;
                _ans[i + 64] = Color.FromArgb(255, _red, 255, 0);

            }
            // 黄→赤
            for (int i = 0; i < 128; i++)
            {
                int _green = 255 - i * 2;
                _ans[i + 128] = Color.FromArgb(255, 255, _green, 0);
            }

            return _ans;
        }
        #endregion


        //参考。RGBをHSV(HSB)、HSL(HLS)、HSIに変換、復元する http://dobon.net/vb/dotnet/graphics/hsv.html
        // 各色の扱い方の違いは、この画像がわかりやすい http://www.google.co.jp/imgres?um=1&hl=ja&client=firefox-a&sa=N&tbo=d&rls=org.mozilla:ja:official&biw=959&bih=528&tbm=isch&tbnid=jpHq0WJtRDhDFM:&imgrefurl=http://umesyrup.blog85.fc2.com/blog-entry-118.html&docid=xGmaW2nfYIdWaM&imgurl=http://blog-imgs-30-origin.fc2.com/u/m/_e/umesyrup/20100115104427bb1.jpg&w=1024&h=1024&ei=MB_dUOLJHYTQkgXtlYCgAg&zoom=1&iact=hc&vpx=4&vpy=116&dur=43&hovh=225&hovw=225&tx=133&ty=104&sig=101998425504022082226&page=1&tbnh=148&tbnw=148&start=0&ndsp=16&ved=1t:429,r:0,s:0,i:85
        #region HSV（HSB）（色相・彩度・明度）で直観的に色を扱う。
        /// <summary>
        /// HSV (HSB) カラーを表す。色相 (Hue)、彩度 (Saturation)、明度 (Value, Brightness)の三種類で定義。
        /// </summary>
        public class HsvColor
        {
            private float _h;
            /// <summary>
            /// 色相 (Hue)。値は0以上360未満
            /// </summary>
            public float H
            {
                get { return this._h; }
                set
                {
                    if (value < 0f || 360f <= value)
                    {
                        throw new ArgumentException(
                            "hueは0以上360未満の値です。", "hue");
                    }
                    else
                    {
                        this._h = value;
                    }
                }
            }

            private float _s;
            /// <summary>
            /// 彩度 (Saturation)。値は0以上1未満
            /// </summary>
            public float S
            {
                get { return this._s; }
                set
                {
                    if (value < 0f || 1f <= value)
                    {
                        throw new ArgumentException(
                            "saturationは0以上1以下の値です。", "saturation");
                    }
                    else
                    {
                        this._s = value;
                    }
                }
            }

            private float _v;
            /// <summary>
            /// 明度 (Value, Brightness)。値は0以上1未満
            /// </summary>
            public float V
            {
                get { return this._v; }
                set
                {
                    if (value < 0f || 1f <= value)
                    {
                        throw new ArgumentException(
                            "valueは0以上1以下の値です。", "value");
                    }
                    else
                    {
                        this._v = value;
                    }
                }
            }
            // merusaiaがコンストラクタをprivateからpublicに変更。
            public HsvColor(float hue, float saturation, float brightness)
            {
                if (hue < 0f || 360f <= hue)
                {
                    throw new ArgumentException(
                        "hueは0以上360未満の値です。", "hue");
                }
                if (saturation < 0f || 1f < saturation)
                {
                    throw new ArgumentException(
                        "saturationは0以上1以下の値です。", "saturation");
                }
                if (brightness < 0f || 1f < brightness)
                {
                    throw new ArgumentException(
                        "brightnessは0以上1以下の値です。", "brightness");
                }

                this._h = hue;
                this._s = saturation;
                this._v = brightness;
            }

            /// <summary>
            /// 指定したColorからHsvColorを作成する
            /// </summary>
            /// <param name="rgb">Color</param>
            /// <returns>HsvColor</returns>
            public static HsvColor FromRgb(Color rgb)
            {
                float r = (float)rgb.R / 255f;
                float g = (float)rgb.G / 255f;
                float b = (float)rgb.B / 255f;

                float max = Math.Max(r, Math.Max(g, b));
                float min = Math.Min(r, Math.Min(g, b));

                float brightness = max;

                float hue, saturation;
                if (max == min)
                {
                    //undefined
                    hue = 0f;
                    saturation = 0f;
                }
                else
                {
                    float c = max - min;

                    if (max == r)
                    {
                        hue = (g - b) / c;
                    }
                    else if (max == g)
                    {
                        hue = (b - r) / c + 2f;
                    }
                    else
                    {
                        hue = (r - g) / c + 4f;
                    }
                    hue *= 60f;
                    if (hue < 0f)
                    {
                        hue += 360f;
                    }

                    saturation = c / max;
                }

                return new HsvColor(hue, saturation, brightness);
            }

            /// <summary>
            /// 指定したHsvColorからColorを作成する
            /// </summary>
            /// <param name="hsv">HsvColor</param>
            /// <returns>Color</returns>
            public static Color ToRgb(HsvColor hsv)
            {
                float v = hsv.V;
                float s = hsv.S;

                float r, g, b;
                if (s == 0)
                {
                    r = v;
                    g = v;
                    b = v;
                }
                else
                {
                    float h = hsv.H / 60f;
                    int i = (int)Math.Floor(h);
                    float f = h - i;
                    float p = v * (1f - s);
                    float q;
                    if (i % 2 == 0)
                    {
                        //_nowTime
                        q = v * (1f - (1f - f) * s);
                    }
                    else
                    {
                        q = v * (1f - f * s);
                    }

                    switch (i)
                    {
                        case 0:
                            r = v;
                            g = q;
                            b = p;
                            break;
                        case 1:
                            r = q;
                            g = v;
                            b = p;
                            break;
                        case 2:
                            r = p;
                            g = v;
                            b = q;
                            break;
                        case 3:
                            r = p;
                            g = q;
                            b = v;
                            break;
                        case 4:
                            r = q;
                            g = p;
                            b = v;
                            break;
                        case 5:
                            r = v;
                            g = p;
                            b = q;
                            break;
                        default:
                            throw new ArgumentException(
                                "色相の値が不正です。", "hsv");
                    }
                }

                return Color.FromArgb(
                    (int)Math.Round(r * 255f),
                    (int)Math.Round(g * 255f),
                    (int)Math.Round(b * 255f));
            }
        }
        #endregion

        #region HSL(HLS)（色相・彩度・輝度で定義。Windowsペイント、PhotoShopなどで採用）
        /// <summary>
        /// HSL (HLS) カラーを表す。色を、色相 (Hue)、彩度 (Saturation)、輝度 (Lightness)の三種類で定義。
        /// </summary>
        public class HslColor
        {
            private float _h;
            /// <summary>
            /// 色相 (Hue)。値は0以上360未満
            /// </summary>
            public float H
            {
                get { return this._h; }
                set
                {
                    if (value < 0f || 360f <= value)
                    {
                        throw new ArgumentException(
                            "hueは0以上360未満の値です。", "hue");
                    }
                    else
                    {
                        this._h = value;
                    }
                }
            }

            private float _s;
            /// <summary>
            /// 彩度 (Saturation)。値は0以上1未満
            /// </summary>
            public float S
            {
                get { return this._s; }
                set
                {
                    if (value < 0f || 1f < value)
                    {
                        throw new ArgumentException(
                            "saturationは0以上1以下の値です。", "saturation");
                    }
                    else
                    {
                        this._s = value;
                    }
                }
            }

            private float _l;
            /// <summary>
            /// 輝度 (Lightness)。値は0以上1未満
            /// </summary>
            public float L
            {
                get { return this._l; }
                set
                {
                    if (value < 0f || 1f < value)
                    {
                        throw new ArgumentException(
                            "lightnessは0以上1以下の値です。", "lightness");
                    }
                    else
                    {
                        this._l = value;
                    }
                }
            }
            // merusaiaがコンストラクタをprivateからpublicに変更。
            public HslColor(float hue, float saturation, float lightness)
            {
                if (hue < 0f || 360f <= hue)
                {
                    throw new ArgumentException(
                        "hueは0以上360未満の値です。", "hue");
                }
                if (saturation < 0f || 1f < saturation)
                {
                    throw new ArgumentException(
                        "saturationは0以上1以下の値です。", "saturation");
                }
                if (lightness < 0f || 1f < lightness)
                {
                    throw new ArgumentException(
                        "lightnessは0以上1以下の値です。", "lightness");
                }

                this._h = hue;
                this._s = saturation;
                this._l = lightness;
            }

            /// <summary>
            /// 指定したColorからHslColorを作成する
            /// </summary>
            /// <param name="rgb">Color</param>
            /// <returns>HslColor</returns>
            public static HslColor FromRgb(Color rgb)
            {
                float r = (float)rgb.R / 255f;
                float g = (float)rgb.G / 255f;
                float b = (float)rgb.B / 255f;

                float max = Math.Max(r, Math.Max(g, b));
                float min = Math.Min(r, Math.Min(g, b));

                float lightness = (max + min) / 2f;

                float hue, saturation;
                if (max == min)
                {
                    //undefined
                    hue = 0f;
                    saturation = 0f;
                }
                else
                {
                    float c = max - min;

                    if (max == r)
                    {
                        hue = (g - b) / c;
                    }
                    else if (max == g)
                    {
                        hue = (b - r) / c + 2f;
                    }
                    else
                    {
                        hue = (r - g) / c + 4f;
                    }
                    hue *= 60f;
                    if (hue < 0f)
                    {
                        hue += 360f;
                    }

                    //saturation = c / (1f - Math.Abs(2f * lightness - 1f));
                    if (lightness < 0.5f)
                    {
                        saturation = c / (max + min);
                    }
                    else
                    {
                        saturation = c / (2f - max - min);
                    }
                }

                return new HslColor(hue, saturation, lightness);
            }

            /// <summary>
            /// 指定したHslColorからColorを作成する
            /// </summary>
            /// <param name="hsl">HslColor</param>
            /// <returns>Color</returns>
            public static Color ToRgb(HslColor hsl)
            {
                float s = hsl.S;
                float l = hsl.L;

                float r1, g1, b1;
                if (s == 0)
                {
                    r1 = l;
                    g1 = l;
                    b1 = l;
                }
                else
                {
                    float h = hsl.H / 60f;
                    int i = (int)Math.Floor(h);
                    float f = h - i;
                    //float c = (1f - Math.Abs(2f * l - 1f)) * s;
                    float c;
                    if (l < 0.5f)
                    {
                        c = 2f * s * l;
                    }
                    else
                    {
                        c = 2f * s * (1f - l);
                    }
                    float m = l - c / 2f;
                    float p = c + m;
                    //float x = c * (1f - Math.Abs(h % 2f - 1f));
                    float q; // q = x + m
                    if (i % 2 == 0)
                    {
                        q = l + c * (f - 0.5f);
                    }
                    else
                    {
                        q = l - c * (f - 0.5f);
                    }

                    switch (i)
                    {
                        case 0:
                            r1 = p;
                            g1 = q;
                            b1 = m;
                            break;
                        case 1:
                            r1 = q;
                            g1 = p;
                            b1 = m;
                            break;
                        case 2:
                            r1 = m;
                            g1 = p;
                            b1 = q;
                            break;
                        case 3:
                            r1 = m;
                            g1 = q;
                            b1 = p;
                            break;
                        case 4:
                            r1 = q;
                            g1 = m;
                            b1 = p;
                            break;
                        case 5:
                            r1 = p;
                            g1 = m;
                            b1 = q;
                            break;
                        default:
                            throw new ArgumentException(
                                "色相の値が不正です。", "hsl");
                    }
                }

                return Color.FromArgb(
                    (int)Math.Round(r1 * 255f),
                    (int)Math.Round(g1 * 255f),
                    (int)Math.Round(b1 * 255f));
            }
        }
        #endregion
    }
}
