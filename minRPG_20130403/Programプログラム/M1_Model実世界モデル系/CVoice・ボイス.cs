using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{
    /// <summary>
    /// １キャラにしゃべらせることのできるセリフを抽象的に定義するところです．
    /// ここに定義したボイスを、
    ///     game.pVo(EVoice・ボイス _ボイスの種類, CChara・キャラ _chara)
    /// で、実際にしゃべらせることができます。
    /// 
    ///     ※現段階ではリスト一覧作成の効率化を測るため、
    ///     一部右側の効果音の日本語が変な個所がありますが、ESE・効果音からコピペしているだけなので気にしないでください
    ///     （だって実際のセリフはキャラ毎に違うんだし、ここで日本語打ってもしょうがないので。）
    ///     
    /// 
    /// 基本的にはセリフをstring型で管理しますが，
    /// 性別によるセリフ分け，敬語・タメ口？のセリフ分け，などが拡張可能になっています．
    /// </summary>
    public enum EVoice・ボイス
    {

        b0_gree1・戦闘開始あいさつ1_はじめまして,

        // 以下、ESE・効果音からとりあえずコピペ。だってセリフはキャラ毎に違うんだし、ここで日本語打ってもしょうがないので。
        system01・決定音_ピロリンッ=10000,
        system03・確定音_ピロピロリーンッ,
        system02・選択音_ピッ,
        system04・戻り音_ピロンッ,

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
    /// 様々なキャラにしゃべらせる応用可能なセリフのクラスです．
    /// 基本的にはセリフ（キャラがしゃべる言葉）はEvoice・ボイス列挙体で管理します。
    /// 
    /// 　なお、実際にしゃべる文字列は、get()メソッドを呼び出すことによりstring型で取得が可能です。
    /// 　
    /// 同じEVoice・ボイス列挙体のセリフでも、実際にしゃべる音声は、キャラの性別などにより異なるように設計できます。
    /// 性別によるセリフ分け，敬語・タメ口？のセリフ分け，などが拡張可能にする予定です。
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class CVoice・ボイス
    {
        EVoice・ボイス p_Voice・セリフ;

        /// <summary>
        /// コンストラクタ．引数にセリフを設定します．
        /// </summary>
        public CVoice・ボイス(string _m_Voice)
        {

        }

        public EVoice・ボイス getP_Voice・セリフ()
        {
            return p_Voice・セリフ;
        }
        /// <summary>
        /// キャラが実際にしゃべる文字列を返します。（音が出ない時はこの文字列をふきだしなどに表示してもＯＫ）
        /// </summary>
        /// <param name="_EVoice"></param>
        /// <returns></returns>
        public static string get(EVoice・ボイス _EVoice, CChara・キャラ _chara)
        {
            string _voiceString = MyTools.getEnumKeyName_OnlyLastIndexWord(_EVoice);
            return _voiceString;
        }

    }
}
