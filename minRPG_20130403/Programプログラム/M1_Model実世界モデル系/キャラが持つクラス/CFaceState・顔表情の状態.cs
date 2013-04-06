using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /// <summary>
    /// 実際に表示されるキャラの顔画像や表情を決める，顔表情の状態です．
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CFaceState・顔表情の状態
    {
        EFaceEmotion・顔表情感情 p_faceEmotion・顔表情感情 = EFaceEmotion・顔表情感情.f0通常で;
        EFaceEyeAngle・目眉の角度 p_faceEyeAngle・目眉の角度 = EFaceEyeAngle・目眉の角度.e0通常で;
        EFaceMouseSize・口元のサイズ p_faceMouseSize・口元のサイズ = EFaceMouseSize・口元のサイズ.m0通常で;
        EFaceDecorate・顔の装飾 p_faceDecorate・顔の装飾 = EFaceDecorate・顔の装飾.o0無し;

        public enum EFaceEmotion・顔表情感情
        {
            f0通常で,
            f1喜んで,
            f2悲しんで,
            f3恐れて,
            f4怒って,
            f5驚いて,
            f6嫌な感じで,
            f7恥ずかしがって,
            f8目を閉じて,
            f9困って,
            f10笑って,
            f11泣いて,
            f12ボケて,
            f13その他,
        }
        public enum EFaceEyeAngle・目眉の角度
        {
            e0通常で = 0,
            e1小上がり = 10,
            e2中上がり = 20,
            e3大上がり = 30,
            e4小下がり = -10,
            e5中下がり = -20,
            e6大下がり = -30,
            e7上移動 = 100,
            e8下移動 = -100,
            e9柔らかく = 1000,

        }
        public enum EFaceMouseSize・口元のサイズ
        {
            m0通常で,
            m1イッと,
            m2ニコッと,
            m3小ヮッと,
            m4大ヮッと,
            m5小Ｏッと,
            m6大Ｏっと,
            m7小ムッと,
            m8大ムッと,
            m9ムグムグッと,
            m10小ガァッと,
            m11大ガァッと,
            m12その他,
        }
        public enum EFaceDecorate・顔の装飾
        {
            o0無し,
            o1陽気三本線,
            o2ノリノリ音符,
            o3ガーン三重線,
            o4タラーン汗,
            o5焦り汗,
            o6ムカつき,
            o7天然系太陽,
            o8シクシク涙,
            o9頬二本線,
            o10頬小赤み,
            o11頬大赤み,
            o12顔真っ赤,
            o13顔真っ青,
            o13顔真っ黒,
        }



        /*
        public enum 顔表情例{
            普通,
            微笑んで,
            嬉しそうに,
            爆笑,
            悲しんで,
            嫌がって,
            楽しんで,
            考え中で,
        */
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CFaceState・顔表情の状態(EFaceEmotion・顔表情感情 _faceEmotion)
        {
            p_faceEmotion・顔表情感情 = _faceEmotion;

        }
        public CFaceState・顔表情の状態(EFaceEmotion・顔表情感情 _faceEmotion, EFaceEyeAngle・目眉の角度 _eyeAngle, EFaceMouseSize・口元のサイズ _mouseSize, EFaceDecorate・顔の装飾 _faceDecorate)
        {

            p_faceEmotion・顔表情感情 = _faceEmotion;
            p_faceEyeAngle・目眉の角度 = _eyeAngle;
            p_faceMouseSize・口元のサイズ = _mouseSize;
            p_faceDecorate・顔の装飾 = _faceDecorate;

        }
    }
    

}
