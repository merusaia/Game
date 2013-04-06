using System;
using System.Collections.Generic;
using System.Text;

// Twitterizerライブラリを使っている
using Twitterizer; //.NET Frameworkを4.0以上にしないと通らない？// VS2010/VWD2010以降なら、NuGetを使って、Install-Package twitterizer -Version 2.4.2 を実行して。参考: http://nuget.org/packages/twitterizer
// Process.Start()メソッドのため
using System.Diagnostics; 

// MyToolsクラスを使っている
using PublicDomain;

// Windows.Forms依存にしたくない時はコメントアウト＋修正
using System.Windows.Forms;
using System.Drawing;
using System.Net;
using System.IO; 

namespace PublicDomain.Twitter_NET4_0
{
    /// <summary>
    /// （※このクラスだけ、ビルドに「.NET Framework4.0」以上が必要です。
    /// ■注意：×「.NET Framework4.0 Client」でもダメ！）
    /// 
    ///   Twitter関連処理を（日本語付きメソッドなどで）簡単に扱えるようにしたstaticメソッドを集めたクラスです。
    /// 私のTwitterクラス、みたいな感じで、自分流に改良してってください。
    /// 
    /// ※推奨事項。TwitterAPIや関連ライブラリがバージョンアップなどで変更になった場合、
    /// いちいちたくさんのクラスを書き変えるのは面倒です。
    /// TwitterAPIを使う処理はこのクラスだけにして、いざというときはここだけ変更すればいいようにしておきましょう。
    /// </summary>
    public class MyTwitterTools
    {

        /// <summary>
        /// Twitter関連処理を行うプログラムのトークン作成に必要なキーです。
        /// 
        /// 普通は、Twitte開発者向けページr https://dev.twitter.com/ で作成したConsumer keyなどをここに代入して使ってください。
        /// 
        /// （トークンとは、例えばbotを作る場合、定期的につぶやくロボット君を作成するために必要な部品（キーパーツ）のことと思えばわかりやすい？）
        /// </summary>
        private static string p_Consumer_key = "0mgjucDY67md6rAH4z9Q";      // 必ず必要。
        private static string p_Consumer_secret = "IL43y69hYXv8SyMuVO7amOWWFGbZOl3DNPhyHuUPs";   // 必ず必要。
        private static string p_Access_token = "102343470-HfETfq6bwdAmYagD8WIReNuRVgt2PP7BAAbqOdN3";      // twitterクライアントなどの場合は、ユーザにＰＩＮコードなどを入力させてから作成するから、空白でもよい。botの場合は必要。
        private static string p_Access_secret = "gH6IvRTxnNGXKIRyZ5dNg6SjLXSmzPwiaMEttJx5Sn0";     // twitterクライアントなどの場合は、ユーザにＰＩＮコードなどを入力させてから作成するから、空白でもよい。botの場合は必要。

        /// <summary>
        /// OAuthで識別したトークンです。つぶやき投稿など、いろんな時に使います。
        /// 
        /// （トークンとは、例えばbotを作る場合、定期的につぶやくロボット君を作成するために必要な部品（キーパーツ）のことと思えばわかりやすい？）
        /// 
        /// ※値は、基本的には、static変数であるp_Consumer_keyなどに自分のTwitterアプリの値を代入していれば、
        /// 勝手に_initメソッドが呼びだされて、トークンが作成されています。
        /// ただし、取得できなかった場合は、nullが入っているかもしれないので、一応nullじゃないかは確認した方がいいかも。
        /// </summary>
        private static OAuthTokens p_token = init・初期化(p_Consumer_key, p_Consumer_secret, p_Access_token, p_Access_secret);




        /// <summary>
        /// コンストラクタです。このクラスは、基本的にはstaticメソッドとして使うので、あまり使いません。
        /// 詳しくはHELP・このクラスの使い方のヘルプ()メソッドなどをみてください。
        /// </summary>
        public MyTwitterTools()
        {

        }


        /// <summary>
        /// このクラスのエラー処理を共通して管理するメソッドです。
        /// 
        /// TwitterResponse.Result != Success　だったとき、一貫してこのメソッドを呼び出してください。
        /// そうすると、エラー処理の個所をひとつにまとめられます。
        /// </summary>
        /// <param name="_ShownMessage・表示したいメッセージ"></param>
        /// <param name="_details・詳細エラーメッセージ＿TwitterResponse_ErrorMessage"></param>
        /// <returns></returns>
        public static bool showMessageBoxError・エラー処理(string _ShownMessage・表示したいメッセージ, string _details・詳細エラーメッセージ＿TwitterResponse_ErrorMessage)
        {
            MessageBox.Show("【エラー】" + _ShownMessage・表示したいメッセージ + "\n\n詳細：" + _details・詳細エラーメッセージ＿TwitterResponse_ErrorMessage, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // 以下、エラーの内容によって、よくはまる事例をまとめてくれているサイト。感謝。　http://oresta.blog66.fc2.com/blog-entry-22.html

            // よくある、"Status is a duplicate."エラーは重複。Twitterでは同じ発言を繰り返し投稿できません。少なくとも10ツイートはあける必要があります。 http://www26.atwiki.jp/easybotter_wiki/pages/21.html 

            return true;
        }



        #region ユーザ情報などを辞書に登録する処理：　getTwitterUser()など、ほとんどのメソッドで共通して使われる処理なので、TwitterAPIアクセス回数を節約するために、少し工夫している
        /// <summary>
        /// （１時間に１５０アクセスまでという）TwitterAPIアクセス回数節約のために、一度読み込んだデータを辞書に格納して使うか。
        /// trueだと処理はほんの少し重たくなるかもしれないが節約。falseだと余計なことはせずAPIをじゃんじゃん使う。
        /// 
        /// ※現時点では、trueにしないとすぐ１時間１５０回をすぐ使いきっちゃうので、trueにしている。
        /// </summary>
        public static bool p_isUseDictionary・TwitterAPIアクセス回数削減のために辞書を使うか = true;
        /// <summary>
        /// 一度getTwitterUser・ユーザ情報を取得(＠からはじまるtwitterID)で読み込んだTwitterUserクラスのインスタンスを格納している辞書です。
        /// 
        /// （１時間に１５０アクセスまでという）TwitterAPI実行数節約のために、一度読み込んだユーザのデータを取得するときに使用します。
        /// </summary>
        private static Dictionary<string, TwitterUser> p_Users_KeyTwitterID・今まで取得したユーザの辞書 = new Dictionary<string, TwitterUser>();
        /// <summary>
        /// 一度getTwitterUser・ユーザ情報を取得(TwitterUser.Idのユーザ識別子)で読み込んだTwitterUserクラスのインスタンスを格納している辞書です。
        /// 
        /// （１時間に１５０アクセスまでという）TwitterAPI実行数節約のために、一度読み込んだユーザのデータを取得するときに使用します。
        /// </summary>
        private static Dictionary<decimal, TwitterUser> p_Users_KeyDecimalId・今まで取得したユーザの辞書 = new Dictionary<decimal, TwitterUser>();

        /// <summary>
        /// ツイッターＩＤ（＠で始まる英数字記号の文字列）からユーザ情報TwitterUser型のインスタンスを取得します。失敗した場合は、nullが入ります。
        /// 
        /// ※このクラス内では、twitterAPI実行数を節約するため、getUser・ユーザ情報を取得(..)の代わりに呼びだしています。
        /// </summary>
        /// <returns></returns>
        public static TwitterUser getUser・ユーザ情報を取得(string _twitterID)
        {
            if (p_isUseDictionary・TwitterAPIアクセス回数削減のために辞書を使うか == true)
            {
                // APIを呼びだす前に、既に辞書に格納されていないかを確認
                if (p_Users_KeyTwitterID・今まで取得したユーザの辞書.ContainsKey(_twitterID) == true)
                {
                    // これでAPI呼びだす回数を節約できる。
                    return p_Users_KeyTwitterID・今まで取得したユーザの辞書[_twitterID];
                }
            }
            // 普通にAPIを呼びだして取得
            TwitterUser _user = null;
            TwitterResponse<TwitterUser> _res = TwitterUser.Show(_twitterID); // これでも150回のTwitterAPIアクセス回数は消費するから注意して！
            if (_res.Result == RequestResult.Success)
            {
                _user = _res.ResponseObject;
            }
            else if (_res.Result == RequestResult.Unknown)
            {
                showMessageBoxError・エラー処理("TwitterID「" + _twitterID + "」のユーザが見つかりませんでした。", _res.ErrorMessage);
            }
            else
            {
                showMessageBoxError・エラー処理("ツイッターＩＤからユーザ情報（TwitterUser）取得時のエラーです、", _res.ErrorMessage);
            }
            // 一度取得したユーザ情報を辞書に登録（辞書を使う場合だけ）
            if (p_isUseDictionary・TwitterAPIアクセス回数削減のために辞書を使うか == true)
            {
                if (_user != null)
                {
                    // それぞれの辞書に、登録されていなければ、新しくユーザ情報を取得
                    // こちらは確実に登録されていない
                    p_Users_KeyTwitterID・今まで取得したユーザの辞書.Add(_twitterID, _user);
                    // こちらは登録されているかわからないので確認
                    decimal _decimalId = _user.Id;
                    if (p_Users_KeyDecimalId・今まで取得したユーザの辞書.ContainsKey(_decimalId) == false)
                    {
                        p_Users_KeyDecimalId・今まで取得したユーザの辞書.Add(_decimalId, _user);
                    }

                }
            }
            return _user;

        }
        /// <summary>
        /// TwitterUser.Idのユーザ識別子（Decimal型のユーザ識別子）からユーザ情報TwitterUser型のインスタンスを取得します。失敗した場合は、nullが入ります。
        /// 
        /// ※このクラス内では、twitterAPI実行数を節約するため、getUser・ユーザ情報を取得(..)の代わりに呼びだしています。
        /// </summary>
        /// <returns></returns>
        public static TwitterUser getUser・ユーザ情報を取得(decimal _TwitterUser_Id_decimal)
        {
            if (p_isUseDictionary・TwitterAPIアクセス回数削減のために辞書を使うか == true)
            {
                // APIを呼びだす前に、既に辞書に格納されていないかを確認
                if (p_Users_KeyDecimalId・今まで取得したユーザの辞書.ContainsKey(_TwitterUser_Id_decimal) == true)
                {
                    // これでAPI呼びだす回数を節約できる。
                    return p_Users_KeyDecimalId・今まで取得したユーザの辞書[_TwitterUser_Id_decimal];
                }
            }
            // 普通にAPIを呼びだして取得
            TwitterUser _user = null;
            TwitterResponse<TwitterUser> _res = TwitterUser.Show(_TwitterUser_Id_decimal);
            if (_res.Result == RequestResult.Success)
            {
                _user = _res.ResponseObject;
                return _user;
            }
            else if (_res.Result == RequestResult.Unknown)
            {
                showMessageBoxError・エラー処理("Decimal型のユーザ識別子「" + _TwitterUser_Id_decimal + "」のユーザが見つかりませんでした。", _res.ErrorMessage);
            }
            else
            {
                showMessageBoxError・エラー処理("ユーザ識別子からユーザ識別子取得時のエラーです、", _res.ErrorMessage);
            }
            // 一度取得したユーザ情報を辞書に登録（辞書を使う場合だけ）
            if (p_isUseDictionary・TwitterAPIアクセス回数削減のために辞書を使うか == true)
            {
                if (_user != null)
                {
                    // それぞれの辞書に、登録されていなければ、新しくユーザ情報を取得
                    // こちらは確実に登録されていない
                    p_Users_KeyDecimalId・今まで取得したユーザの辞書.Add(_TwitterUser_Id_decimal, _user);
                    // こちらは登録されているかわからないので確認
                    string _twitterID = _user.Name;
                    if (p_Users_KeyTwitterID・今まで取得したユーザの辞書.ContainsKey(_twitterID) == false)
                    {
                        p_Users_KeyTwitterID・今まで取得したユーザの辞書.Add(_twitterID, _user);
                    }

                }
            }
            return null;

        }
        #endregion



        #region ツイッターのよく使う処理：　サムネイル画像取得、つぶやく、検索、タイムライン取得、フォロワー取得など
        /// <summary>
        /// tweetの代わりにメッセージボックスで内容を表示します。テストなのにtwitterAPIに迷惑かけちゃ…心が痛むからね。本番ではちゃんとfalseにするのを忘れないでね。
        /// </summary>
        private static bool p_isTest・テスト用にツイッターで投稿する代わりにメッセージボックスに表示するか = true;
        /// <summary>
        /// 引数の文字列を新しくつぶやきます。エラーが起こるとfalseを返します。
        /// 
        ///  ・・・やり方は、ここなどを参考。特に非同期のつぶやき投稿方法。感謝
        ///  http://www.satsukifactory.net/twitter/twitter-client/twitterizer-asyncmemo/
        /// </summary>
        /// <param name="_newTweetText・つぶやくツイート内容＿140文字以内じゃないと切れちゃうよ"></param>
        /// <returns></returns>
        public static bool tweet・つぶやく(string _newTweetText・つぶやくツイート内容＿140文字以内じゃないと切れちゃうよ)
        {
            bool _isSuccess = false;
            string _tweet = _newTweetText・つぶやくツイート内容＿140文字以内じゃないと切れちゃうよ;

            //ステータスラベルにポスト中の旨を表示
            //toolStripStatusLabel1.Text = "Posting...";

            //ポストする際のオプション指定。ここではSSLを使用しないように設定。
            StatusUpdateOptions _option = new StatusUpdateOptions();
            _option.UseSSL = false;

            // 以下、同期処理
            if (p_isTest・テスト用にツイッターで投稿する代わりにメッセージボックスに表示するか == false)
            {
                TwitterResponse<TwitterStatus> _result = TwitterStatus.Update(p_token, _tweet, _option);
                if (_result.Result == RequestResult.Success)
                {
                    MessageBox.Show("つぶやき投稿に成功したよ\n内容："+_tweet, "確認", MessageBoxButtons.OK, MessageBoxIcon.None); //ダイアログを表示
                    _isSuccess = true;
                }
                else
                {
                    showMessageBoxError・エラー処理("つぶやき投稿に失敗したよ。", _result.ErrorMessage);
                    _isSuccess = false;
                    
                }
            }
            else
            {
                MessageBox.Show("つぶやき投稿をテストでここに表示してるよ。\n内容：" + _tweet + "\n\n（twitterAPIに負荷与えないように配慮するなんて、君は偉いね。尊敬するよ。）", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            #region 非同期処理の草案（ TwitterStatusAsyncが認識しない。新しいバージョンのtwitterizerでは認識しなくなった？）
            //IAsyncResult aresult = TwitterStatusAsync.Update(tokens, _tweet, option,new TimeSpan(0, 1, 0), res =>
            //{
            //    if (res.Result == RequestResult.Success)
            //    {
            //       BeginInvoke(new Action(() =>
            //       {
            //          toolStripStatusLabel1.Text = "PostingSuccess!";　//ラベルの表示を変更
            //         MessageBox.Show("投稿成功！", "確認", MessageBoxButtons.OK, MessageBoxIcon.None); //ダイアログを表示
            //        }));
            //     }
            //     else
            //     {
            //        BeginInvoke(new Action(() =>
            //        {
            //          toolStripStatusLabel1.Text = "Error!";
            //          MessageBox.Show("失敗したよ", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //         }));
            //      }
            //});
            #endregion

            return _isSuccess;
        }

        /// <summary>
        /// 自分のタイムラインを引数で指定した数だけ、取得します。
        /// 返り値_timelineはTwitterStatusCollectionクラスです。
        /// 配列やリストと同じように、_timeline[配列].***で指定した配列のツイートを参照できます。
        /// </summary>
        /// <param name="_timeLineCount・取得するツイート数"></param>
        /// <returns></returns>
        public static TwitterStatusCollection getTimeLine・タイムラインを取得(int _timeLineCount・取得するツイート数)
        {
            TwitterStatusCollection _timeline = null;
            TimelineOptions options = new TimelineOptions()
            {
                //ホームタイムラインの取得する行を5行に設定する
                Count = _timeLineCount・取得するツイート数
            };

            //ホームタイムライン（p_tokenを持つアカウントの、自分のタイムライン）を取得する
            TwitterResponse<TwitterStatusCollection> _res = TwitterTimeline.HomeTimeline(p_token, options);
            if (_res.Result == RequestResult.Success)
            {
                _timeline = _res.ResponseObject;
            }
            else
            {
                showMessageBoxError・エラー処理("タイムライン取得時のエラーです。", _res.ErrorMessage);
            }
            return _timeline;
        }

        /// <summary>
        /// 指定twitterIDのユーザが、フォローしているユーザ数（フォロー数）を取得します（高速）。見つからない場合は、-1を返します。
        /// </summary>
        /// <param name="_twitterID"></param>
        /// <returns></returns>
        public static int getFriendsNum・フォロー数を取得(string _twitterID)
        {
            int _followNum = -1; // 見つからない場合は-1
            TwitterUser _user = getUser・ユーザ情報を取得(_twitterID);
            if (_user != null)
            {
                long _num = _user.NumberOfFriends; // フレンド数Friendがフォロー数でいいらしい。一方的にフォローしてる場合も多いのに、ネーミングセンスはなんか変…まぁフォロワー数と間違えないようにするためだろうけどね。
                _followNum = (int)_num;
            }
            else
            {
                showMessageBoxError・エラー処理("フォロー数を取ろうとして、ユーザ情報が見つかりませんでした。", "（エラー情報無し）");
            }
            return _followNum;
        }
        /// <summary>
        /// フォロワー数を取得します（高速）。見つからない場合は、-1を返します。
        /// </summary>
        /// <param name="_twitterID"></param>
        /// <returns></returns>
        public static int getFollowersNum・フォロワー数を取得(string _twitterID)
        {
            int _followersNum = -1; // 見つからない場合は-1
            TwitterUser _user = getUser・ユーザ情報を取得(_twitterID);
            if (_user != null)
            {
                // long?型の扱い方
                // bool?型はbool型かnullを取る場合もあるってこと。nullじゃないかをちゃんと確認しようね。 http://msdn.microsoft.com/ja-jp/library/vstudio/bb384091.aspx
                long? _num = _user.NumberOfFollowers;
                if (_num != null)
                {
                    _followersNum = (int)_num;
                }
            }
            else
            {
                showMessageBoxError・エラー処理("フォロワー数を取ろうとして、ユーザ情報が見つかりませんでした。","（エラー情報無し）");
            }
            return _followersNum;
        }


        /// <summary>
        ///  指定したtwitterID（＠ではじまるやつ）のユーザの、
        ///  フォロワー（フォローされている人）、もしくはフォローしている人、
        ///  全員のユーザ識別子（TwitterUser.Idで示されるDecimal型の数値）のリストを取得します。
        ///  
        /// ※※twitterizerではこの方法でないとフォロワーをユーザ情報を取得できない（作者の調べた段階では）ため、このメソッドだけ例外的にユーザ識別子を返すようにしています。
        ///  
        /// ※フォロワーが5000人以上いるTwitterIDを指定した場合は、TwitterAPIへのアクセス回数が消費されます。使用の祭は気を付けてください。
        /// </summary>
        /// <param name="_TwitterUser_Id_decimal"></param>
        /// <returns></returns>
        public static List<Decimal> getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(string _twitterID, bool _isTrueFollower_isFalseFriendsFolloing・trueだとフォロワー＿falseだとフォローしている人を取得)
        {
            string _twitterId = _twitterID;
            List<Decimal> _followers_TwitterUserIDs = new List<Decimal>();

            // この処理だと、一回に5000フォロワーしか呼びだせない
            //TwitterResponse<UserIdCollection> _userResponce = TwitterFriendship.FollowersIds(p_token);
            //UserIdCollection _users = _userResponce.ResponseObject;

            // 参考： http://ameblo.jp/funlife-v-v/entry-11388989280.html
            //TwitterFriendship.FollowersIdsは一度に5000件しか取得できません。
            //5000件以上フォロワーがいる場合は困ってしまいます。
            // 
            //そこでUserIdsOptionsを利用してやります。
            // 
            //UserIdsOptions.Cursorに、UserIdCollection.NextCursorを指定して
            //再度TwitterFriendship.FollowersIdsを実行します。
            // 
            //こんな感じです。	
            Decimal _userId = getDecimalId_ByTwitterID(_twitterId);
            UsersIdsOptions _options = new UsersIdsOptions()
            {
                UserId = _userId
            };
            // 取れなくなるまで調べる
            while (true)
            {
                // とりあず5000個まで呼びだすだけだったら、以下でもいい。
                // TwitterResponse<UserIdCollection> _users =TwitterFriendship.FollowersIds(p_token, _options);
                // var _users =TwitterFriendship.FollowersIds(p_token, _options); // TwitterResponse<UserIdCollection>とvarは表記が違うだけで、内部的にはいっしょ？
                
                // ※TwitterFriendship.Followersだと100人ずつしか呼びだせないみたい。しかもループさせても不安定で、よく399位で止まる。さらにかなり時間がかかる。
                // ※なのでFollowersIdsを使用する。が、これも5000人ずつしか呼びだせないみたい。ループさせたら数は安定。ただしかなり時間がかかる。
                TwitterResponse<UserIdCollection> _res = null;
                if (_isTrueFollower_isFalseFriendsFolloing・trueだとフォロワー＿falseだとフォローしている人を取得 == true)
                {
                    // フォロワーを取得
                    _res = TwitterFriendship.FollowersIds(p_token, _options);
                }
                else
                {
                    // フレンド（フォローしている人）を取得
                    _res = TwitterFriendship.FriendsIds(p_token, _options);
                }
                // リストに追加
                if (_res.Result == RequestResult.Success)
                {
                    // 数を調べる
                    int _idsCount = _res.ResponseObject.Count;
                    for (int i = 0; i < _idsCount; i++)
                    {
                        // DecimalIdをリストに代入
                        _followers_TwitterUserIDs.Add(_res.ResponseObject[i]);
                    }
                    // まだあるか調べる
                    UserIdCollection obj = _res.ResponseObject;
                    // 先頭に戻るとCursorが0となる
                    if ((_options.Cursor = obj.NextCursor) == 0)
                    {
                        // おわった～
                        break;
                    }
                    // まだあるから、もっかい実行
                }
                else
                {
                    // もうないか、失敗したかやから、おわり
                    break;
                }
            }


            return _followers_TwitterUserIDs;

        }

        /// <summary>
        ///  フォロワー（フォローされている人）のtwitterID（＠ではじまるやつ）リストを取得します。
        ///  
        /// ※フォロワーが5000以上のtwitterIDの場合、結構時間とTwitterAPIアクセス回数を消費するので、注意してください。
        /// </summary>
        /// <param name="_TwitterUser_Id_decimal"></param>
        /// <returns></returns>
        public static List<string> getFollowerTwitterIDs・フォロワーのツイッターＩＤを全て取得＿フォロワー数が多い時は注意して(string _twitterID)
        {
            string _twitterId = _twitterID;
            List<string> _followerTwitterIDs = new List<string>();

            // 現状では、ユーザ識別子をリストで取得した後、一個一個を数値DecimalId→文字列twitterIDに変換してる。
            // こうすると、ユーザ辞書に登録されるので、次からは子のユーザは余計なTwitterAPIを呼びださずに出力できるはず？。
            List<Decimal> _userIds = getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(_twitterId, true);
            foreach (Decimal _id in _userIds)
            {
                _twitterId = getTwitterID_ByDecimalId(_id);
                _followerTwitterIDs.Add(_twitterId);
            }

            return _followerTwitterIDs;

            #region 草案メモ。TwitterFriendship.FollowersIdsはループを回しても正常に動かない。
            //Decimal _userID = getDecimalId_ByTwitterID(_twitterId);
            //FollowersOptions _options = new FollowersOptions()
            //{
            //    UserId = _userID
            //};
            //// 取れなくなるまで調べる（ものすごく不安定）
            //while(true){
            //    // とりあず5000個まで呼びだす
            //    // TwitterResponse<UserIdCollection> _users =TwitterFriendship.FollowersIds(p_token, _options);
            //    // var _users =TwitterFriendship.FollowersIds(p_token, _options);
            //    // TwitterResponse<UserIdCollection>とvarは表記が違うだけで、内部的にはいっしょ？
            //    // これ（TwitterFriendship.Followers）だと100人ずつしか呼びだせないみたい。しかもかなり時間がかかる。
            //    TwitterResponse<TwitterUserCollection> _res = TwitterFriendship.Followers(p_token, _options);
            //    // リストに追加
            //    if (_res.Result == RequestResult.Success)
            //    {
            //        int _idsCount = _res.ResponseObject.Count;
            //        for(int i=0; i<_idsCount; i++){
            //            _followerIDs.Add(_res.ResponseObject[i].Name);
            //        }
            //            // まだあるか調べる
            //            TwitterUserCollection obj = _res.ResponseObject;
            //            // 先頭に戻るとCursorが0となる
            //            if ((_options.Cursor = obj.NextCursor) == 0)
            //            {
            //                // おわった～
            //                break;
            //            }
            //            // まだあるから、もっかい実行
            //    }else{
            //        // もうないか、失敗したかやから、おわり
            //        break;
            //    }
            //}
            //return _folloerIDs; 
            #endregion
        }
        #region 草案 　　指定番目のフォロワー／フレンド（フォローしている人）を取得するメソッド。
        // 指定番目のフォロワー／フレンド（フォローしている人）を取得するメソッド。
        // ※一応作ったが、処理が重たいし、何回も呼びだされたら困る処理だし、呼びだしても指定した番目のユーザ情報が無い時は取得し直しになるので、やめておく。
        //   もっと改良されて使いやすくなったら復活するかも

        ///// <summary>
        ///// 指定したTwitterID（＠ではじまるやつ）のユーザを、古い方から数えて（初めから）何番目に自分がフォローしている人（フレンド）のTwitterIDを取得します。
        ///// 取得できない場合は、nullが返ります。
        ///// 
        ///// ※フォロワーが5000人以上いるTwitterIDを指定した場合は、TwitterAPIへのアクセス回数が消費されます。使用の祭は気を付けてください。
        ///// </summary>
        ///// <returns></returns>
        //public static TwitterUser getFriend・フレンドを取得＿フォロー数が多い時は注意して(string _twitterID, int _folowerNo・何番目のフォロワーかを取得するか)
        //{
        //    TwitterUser _follower = null;
        //    TwitterUser _user = getUser・ユーザ情報を取得(_twitterID);
        //    List<Decimal> _followerIds = getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(_twitterID, false);
        //    if (_followerIds != null)
        //    {
        //        _follower = getUser・ユーザ情報を取得(_followerIds[_folowerNo・何番目のフォロワーかを取得するか]);
        //    }
        //    return _follower;
        //}
        ///// <summary>
        ///// 指定したTwitterID（＠ではじまるやつ）のユーザを、古い方から数えて（初めから）何番目に自分をフォローしているフォロワーのTwitterIDを取得します。
        ///// 取得できない場合は、nullが返ります。
        ///// 
        ///// ※フォロワーが5000人以上いるTwitterIDを指定した場合は、TwitterAPIへのアクセス回数が消費されます。使用の祭は気を付けてください。
        ///// </summary>
        ///// <returns></returns>
        //public static TwitterUser getFollowerUser・フォロワーを取得＿フォロワー数が多い時は注意して(string _twitterID, int _folowerNo・何番目のフォロワーかを取得するか)
        //{
        //    TwitterUser _follower = null;
        //    TwitterUser _user = getUser・ユーザ情報を取得(_twitterID);
        //    List<Decimal> _followerIds = getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(_twitterID, true);
        //    if(_followerIds != null){
        //        _follower = getUser・ユーザ情報を取得(_followerIds[_folowerNo・何番目のフォロワーかを取得するか]);
        //    }
        //    return _follower;
        //}
        #endregion


        /// <summary>
        /// ロックしているユーザ識別子が調べます。
        /// 
        /// ※まだ動作確認してません。
        /// </summary>
        /// <param name="_TwitterUser_Id_decimal"></param>
        /// <returns></returns>
        public static bool isProtectedUser・ロックされているユーザ識別子か(string _twitterID)
        {
            TwitterUser _user = getUser・ユーザ情報を取得(_twitterID);
            return _user.IsProtected;
        }

        /// <summary>
        /// TwitterID（User.Name：＠で始まる英数字記号のユニークな文字列）からニックネーム（表示名：ScreenName）を取ってきます。
        /// 見つからなければ""を返します。
        /// </summary>
        /// <param name="_nickName_TwitterUser_ScreenName"></param>
        /// <returns></returns>
        public static string getNickName・ニックネームを取得(string _twitterID)
        {
            string _userNickName = "";
            TwitterUser _user = getUser・ユーザ情報を取得(_twitterID);
            if (_user != null)
            {
                _userNickName = _user.ScreenName;
            }
            else
            {
                showMessageBoxError・エラー処理("twitterID（＠***、TwitterUser.Name）からニックネーム（TwitterUser.ScreenName）取得時のエラーです。", "（エラーメッセージ無し）");
            }
            return _userNickName;
        }
        /// <summary>
        /// ニックネーム（表示名：ScreenName）からTwitterID（User.Name：＠で始まる英数字記号のユニークな文字列）を取ってきます。
        /// 見つからなければ、""を返します。
        /// </summary>
        /// <param name="_nickName_TwitterUser_ScreenName"></param>
        /// <returns></returns>
        public static string getTwitterID_ByScreenName・ニックネームからツイッターＩＤを取得(string _nickName_TwitterUser_ScreenName)
        {
            TwitterUser _user = getUser・ユーザ情報を取得(_nickName_TwitterUser_ScreenName);
            if (_user != null)
            {
                return _user.Name; // これがTwitterID
            }
            else
            {
                showMessageBoxError・エラー処理("「"+_nickName_TwitterUser_ScreenName+"」というニックネームのユーザは見つかりませんでした。", "（エラーメッセージ無し）");
                return "";
            }
            // return Decimal.ToInt32(_user.Id); // これはDecimal型（数値）のid。内部ではよく使うがユーザに意識させても頭がこんがらがると思うから使わない。キャストもややこしいし。
        }

        /// <summary>
        /// ユーザのサムネイル画像を取得します。
        /// 
        /// ※まだ動くか確認してません。
        /// </summary>
        /// <param name="_twitterID_アットマークで始まるやつ"></param>
        /// <returns></returns>
        public static Image getUserImage・ユーザのサムネイル画像を取得(string _twitterID_アットマークで始まるやつ)
        {
            Image _image = null;
            TwitterUser _user = getUser・ユーザ情報を取得(_twitterID_アットマークで始まるやつ);
            if (_user != null)
            {
                //string _uri = _user.ProfileImageLocation;
                //WebRequest _request = WebRequest.Create(_uri);
                //Stream _fileStream = _request.GetRequestStream();
                //p_graphBackImage = Image.FromStream(_fileStream);
                // ↑ほんとはこれでいいはずなんだけど、途中でnullになった時が怖いから一応エラー処理しとく
                string _uri = _user.ProfileImageLocation;
                if (_uri == "")
                {
                    showMessageBoxError・エラー処理("TwitterID「" + _twitterID_アットマークで始まるやつ + "」のサムネイル画像はありません（urlが空です）。", "（エラー情報無し）");
                }
                else
                {
                    WebRequest _request = WebRequest.Create(_uri);
                    if (_request == null)
                    {
                        showMessageBoxError・エラー処理("TwitterID「" + _twitterID_アットマークで始まるやつ + "」のサムネイル画像のリクエスト中にエラーが発生しました。（_requestがnullです）。", "（エラー情報無し）");
                    }
                    else
                    {
                        Stream _fileStream = _request.GetRequestStream();
                        if (_fileStream == null)
                        {
                            showMessageBoxError・エラー処理("TwitterID「" + _twitterID_アットマークで始まるやつ + "」のサムネイル画像のファイルストリームリクエスト中にエラーが発生しました。（_fileStreamがnullです）。", "（エラー情報無し）");
                        }
                        else
                        {
                            // やっと成功。
                            _image = Image.FromStream(_fileStream);
                        }
                    }
                }
            }
            else
            {
                showMessageBoxError・エラー処理("TwitterID「" + _twitterID_アットマークで始まるやつ + "」のユーザ情報が取得できませんでした。サムネイル画像取得時のエラーです。", "（エラー情報無し）");
            }
            return _image;
        }

        /// <summary>
        /// 引数の検索文字列を含むツイートの検索結果_resultsをTwitterSearchResultColloction型で返します。
        /// 失敗した場合はnullが返ります。
        /// 
        /// 返り値の使い方は、例えば_results[配列].Textと記載すれば、指定配列番目のツイートの内容を取得したりできます。
        /// </summary>
        /// <param name="_serchingText・検索文字列"></param>
        /// <returns></returns>
        public static TwitterSearchResultCollection serchTweet・ツイート検索(string _serchingText・検索文字列)
        {
            TwitterResponse<TwitterSearchResultCollection> res = TwitterSearch.Search(_serchingText・検索文字列);
            if (res.Result == RequestResult.Success)
            {
                TwitterSearchResultCollection results = res.ResponseObject;

                // 詳細に分けて結果を返したい場合
                //for (int i = 0; i < results.Count; i++)
                //{
                //    string msg = results[i].Text;
                //    msg += "\r\n";
                //    msg += "UserID : " + results[i].FromUserId + "\r\n";
                //    msg += "—–\r\n";
                //    MyTools.showMessage_ConsoleLine("とりあえず検索結果はリスト毎：" + msg);
                //}
                return results;
            }
            else
            {
                showMessageBoxError・エラー処理("「" + _serchingText・検索文字列 + "」検索中のエラーです。", res.ErrorMessage);
                return null;
            }
        }


        #endregion


        #region クラス内部でよく使うメソッド：　TwitterUser.Id⇔TwitterUser.Name（＠で始まるtwitterID）の変換、など
        /// <summary>
        /// このクラスではよく使われます。
        /// 名前がややこしいので、他のクラスからはあまり使わないことをおススメします。
        /// 
        /// 「＠twitterID」のtwitterID（TwitterUser.Name）から、twitterAPIでユーザを識別するのに使われるDecimal型のTwitterUser.Idを取得します。
        /// 見つからなかったら0を返します。
        /// </summary>
        /// <param name="_twitterID・"></param>
        /// <returns></returns>
        private static Decimal getDecimalId_ByTwitterID(string _twitterID)
        {
            Decimal _userId = 0; // 見つからなかったら0
            TwitterUser _user = getUser・ユーザ情報を取得(_twitterID);
            if(_user != null)
            {
                _userId = _user.Id;
            }
            else
            {
                showMessageBoxError・エラー処理("twitterID「"+_twitterID+"」のユーザ識別子(TwitterUser.Id)は見つかりませんでした。\nこのTwitterIDのユーザは存在しない可能性があります", "（エラーメッセージ無し）");
            }
            return _userId;
        }
        /// <summary>
        /// このクラスではよく使われます。
        /// 名前がややこしいので、他のクラスからはあまり使わないことをおススメします。
        /// 
        /// twitterAPIでユーザを識別するのに使われるDecimal型のTwitterUser.Idから、「＠twitterID」のtwitterID（TwitterUser.Name）を取得します。
        /// 見つからなかったら""を返します。
        /// </summary>
        /// <param name="_twitterID・"></param>
        /// <returns></returns>
        private static string getTwitterID_ByDecimalId(decimal _userId＿Decimal型の数値ユーザ識別子＿TwitterUser_Id)
        {
            string _twitterId = ""; // 見つからなかったら""
            TwitterUser _user = getUser・ユーザ情報を取得(_userId＿Decimal型の数値ユーザ識別子＿TwitterUser_Id);
            if(_user != null)
            {
                _twitterId = _user.Name;
            }
            else
            {
                showMessageBoxError・エラー処理("ユーザ識別子（TwitterUser.Id）「"+_userId＿Decimal型の数値ユーザ識別子＿TwitterUser_Id+"」のTwitterID（TwitterUser.Name）は見つかりませんでした。\nこの識別子のユーザは存在しない可能性があります。", "（エラーメッセージ無し）");
            }
            return _twitterId;
        }


        #endregion



        #region OAuth認証系・init初期化処理、など

        /// <summary>
        /// 初期化処理をしたか
        /// </summary>
        private static bool p_isInitialed = false;
        /// <summary>
        /// 初期化処理です。p_token取得のため、このクラスが使われる前に自動的に呼びだされるようにしています。
        /// 
        /// OAuth用トークンp_tokenを更新します。
        /// 
        /// 引数３・４のAccess token、Access secretのどちらかが""の場合は、別途ブラウザを開き、ユーザにPINコードを入力させて取得します。
        /// </summary>
        /// <param name="_Consumer_key">Twitte開発者向けページr https://dev.twitter.com/ で作成したConsumer key</param>
        /// <param name="_Consumer_secret">Twitte開発者向けページr https://dev.twitter.com/ で作成したConsumer secret</param>
        /// <param name="_Access_token">Acces token。ツイッターにアクセスした対象を識別するIDみたいなもの。botなどなら同じくTwitter開発者向けページで作成したものを使う。ユーザがつぶやくなら""にして、乱数などで新しく作成して作ってもいい</param>
        /// <param name="Access_secret">おなじく、Acces secret。Acces tokenと意味は同じ。</param>
        private static OAuthTokens init・初期化(string _Consumer_key, string _Consumer_secret, string _Access_token, string _Access_secret)
        {
            if (p_Consumer_key == "" || p_Consumer_secret == "")
            {
                MessageBox.Show("プログラム上で、Consumer keyか、Consumer secretが入力されていません。\nMyTwitterTools.p_Consumer keyと、p_Consumer secretに、\nTwitte開発者向けページ https://dev.twitter.com/ で作成したConsumer keyとConsumer secretを入力しているか、確認してください。\n終了します。\nご迷惑をおかけして、申し訳ありません。");
                Application.Exit();
                return null;
            }
            else if (_Consumer_key == "" || _Consumer_secret == "")
            {
                MessageBox.Show("プログラム上で、Consumer keyか、Consumer secretに\"\"を代入しようとしています。MyTwitterTools.init・初期化(...)メソッドの引数を確認してください。終了します。\nご迷惑をおかけして、申し訳ありません。");
                Application.Exit();
                return null;
            }
            if (_Access_token == "" || _Access_secret == "")
            {

                // アクセストークンを取得するために、ブラウザを立ち上げて認証ページを表示させる。
                OAuthTokenResponse req =
                        OAuthUtility.GetRequestToken(_Consumer_key, _Consumer_secret, "oob");
                // これをすると、ツイッターの認証画面が出て、トークン「（英数字数ケタ）」が表示される。
                System.Diagnostics.Process.Start(OAuthUtility.BuildAuthorizationUri(req.Token).ToString());

                // ユーザにキーを入力させる。
                string _pincode = MyTools.showInputBox("ブラウザに表示されている、PINコードを入力してください", "", "");
                OAuthTokenResponse res = null;
                // [Q]PINコードが間違ってた時の処理はかかなくてええのん？
                try
                {
                    // トークンを取ってくる
                    res = OAuthUtility.GetAccessToken(_Consumer_key, _Consumer_secret, req.Token, _pincode);
                }
                catch (Exception e)
                {
                    MessageBox.Show("PINコードが間違っているか、サーバにエラーが起きました。終了します。\nご迷惑をおかけして、申し訳ありません。\n詳細2:"+e.Message);
                    Application.Exit();
                    return null;
                }
                if (res == null)
                {
                    MessageBox.Show("PINコードが間違っているか、サーバにエラーが起きました。終了します。\nご迷惑をおかけして、申し訳ありません。");
                    Application.Exit();
                    return null;
                }
                else
                {
                    // 認証情報を OAuthTokens に格納する。
                    _Access_token = res.Token;
                    _Access_secret = res.TokenSecret;
                }
            }
            // トークンを作成する情報が整った。

            // トークン情報を更新
            p_Consumer_key = _Consumer_key;
            p_Consumer_secret = _Consumer_secret;
            p_Access_token = _Access_token;
            p_Access_secret = _Access_secret;
            //HTTP_OAuth_Consumer_Request
            // OAuthのトークンを新しく作成
            OAuthTokens _token = new OAuthTokens
            {
                ConsumerKey = p_Consumer_key,
                ConsumerSecret = p_Consumer_secret,
                AccessToken = p_Access_token,
                AccessTokenSecret = p_Access_secret
            };

            p_isInitialed = true;

            return _token;
        }

        /// <summary>
        /// ＰＩＮコードをユーザに入力させてツイッター認証し、トークンp_tokenを更新します。
        /// ほぼ http://www.satsukifactory.net/twitter/twitter-client/twitterizer-gettoken/ のコピペです。感謝。
        /// </summary>
        /// <returns></returns>
        public static bool makeUserDoTwitterRecognization・ＰＩＮコードをユーザに入力させてツイッター認証()
        {
            //コンシューマーキー・コンシューマーシークレットの設定
            //const string consumerkey = "Your Consumerkey";
            //const string consumersecret = "Your Consumersecret";

            //リクエストトークンの取得
            OAuthTokenResponse oatr = OAuthUtility.GetRequestToken(p_Consumer_key, p_Consumer_secret, "oob"); //第三引数はよくわかんないけど、oobらしい

            //認証画面へ誘導(ブラウザを開く)
            Uri uri = Twitterizer.OAuthUtility.BuildAuthorizationUri(oatr.Token);
            Process.Start(uri.ToString());

            //PINコード入力画面を表示
            //inputPin pin = new inputPin();
            //pin.ShowDialog();
            //PINコードで認証
            //string pincode = pin.textBox1.Text;
            
            // ↑はいちいちフォームを作成するのが面倒だから、VisualBasicのInputBoxをつかっちゃおう
            string pincode = MyTools.showInputBox("ブラウザに表示されている、PINコードを入力してください", "認証", "");

            OAuthTokenResponse res;
            // [Q]PINコードが間違ってた時の処理はかかなくてええのん？
            try
            {
                res = OAuthUtility.GetAccessToken(p_Consumer_key, p_Consumer_secret, oatr.Token, pincode);
            }
            catch(Exception e)
            {
                MessageBox.Show("PINコードが間違っているか、サーバにエラーが起きました。終了します。\nご迷惑をおかけして、申し訳ありません。");
                return false;
            }
            if (res == null)
            {
                MessageBox.Show("PINコードが間違っているか、サーバにエラーが起きました。終了します。\nご迷惑をおかけして、申し訳ありません。");
                return false;
            }else{
                // static変数へ格納
                p_Access_token = res.Token;
                p_Access_secret = res.TokenSecret;

                // OAuthのトークンを新しく作成
                OAuthTokens _token = new OAuthTokens();
                _token.ConsumerKey = p_Consumer_key;
                _token.ConsumerSecret = p_Consumer_secret;
                _token.AccessToken = p_Access_token;
                _token.AccessTokenSecret = p_Access_secret;
                p_token = _token;

                //認証が完了した旨を知らせる
                System.Windows.Forms.MessageBox.Show("認証完了！", "確認", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            }

            return true;
        }
        #endregion


        #region ●ヘルプメソッド（テストメソッドともいえる）


        #region ■Twitterizerの使い方　（きゃのさんの日記 http://cannotdebug.blog.fc2.com/blog-entry-8.html より抜粋）
        ///  ・・・３．基本構造
        /// Twitterizerにおけるほとんどの操作は、次のような形で行われるようだ。
        ///
        /// TwitterResponse hoge = [Some Class].[Some Method](OAuthToken o,[Values...]);
        /// var Data = hoge.ResponseObject;
        ///
        /// こうすることで、Dataに[Some Class]型のインスタンスか、[Some Class]型のコレクションが代入される。
        ///
        /// この方法で操作するクラスは、
        ///
        /// ・TwitterUser ...ユーザー操作
        /// ・TwitterStatus ...ツイート操作
        /// ・TwitterDirectMessage ...DM操作
        /// ・TwitterList ...リスト操作
        /// ・TwitterSearch ...検索操作
        /// ・TwitterFriendship ...フォロー・アンフォロー操作
        /// ・TwitterFavorite ...お気に入り操作
        /// ・TwitterTimeline ...タイムライン操作
        ///
        /// の8つである。
        ///
        /// 先頭の4つは、ほとんどの静的なメソッドの戻り値として自分自身の型のインスタンス（ツイートするメソッドならツイートしたツイートの情報が入ったインスタンス）、またはそのコレクションを返す。
        ///
        /// 後半の4つは、ほとんどの静的なメソッドの戻り値として（種類によって）先頭の2つのうちどちらかの型のインスタンス（TwitterFriendship型ならTwitterUser型のインスタンス）、またはそのコレクションを返す。
        ///
        /// 具体的には、
        ///
        /// ・タイムラインの取得
        ///
        /// TwitterResponse twitterResponse = TwitterTimeline.HomeTimeline(oau); //取得処理
        /// var TimeLine = twitterResponse.ResponseObject; //TwitterStatusCollection型のインスタンスTimeLineを生成
        /// foreach(TwitterStatus _nowTime in TimeLine) //TimeLineからTwitterStatus型のインスタンスtを抽出
        /// {
        /// Console.WriteLine(_nowTime.User.ScreenName + ": " + _nowTime.Text); //タイムラインのツイートを表示
        /// // _nowTime.UserはTwitterUser型のインスタンス
        /// }
        ///
        /// ・フォロワーの取得
        ///
        /// TwitterResponse twitterResponse = TwitterUser.Followers(oau); //取得処理
        /// var Followers = twitterResponse.ResponseObject; //TwitterUserCollection型のインスタンスFollowersを生成
        /// foreach(TwitterUser u in Followers) //FollowersからTwitterUser型のインスタンスuを抽出
        /// {
        /// Console.WriteLine(u.ScreenName); //フォロワーのScreenName（アカウント名）を表示
        /// }
        ///
        /// ・ユーザー表示
        ///
        /// TwitterResponse twitterResponse = getUser・ユーザ情報を取得(oau,"Alice_Canno"); //取得処理 第二引数は取得したいユーザーのScreenName（アカウント名）
        /// var User = twitterResponse.ResponseObject; //TwitterUser型のインスタンスUserを生成
        /// Console.WriteLine(User.Name); //Alice_Cannoの名前を表示
        ///
        ///という感じである。
        ///
        ///また、第二以降の引数にOptionalPropertiesから派生したクラス（たいてい[hoge]Optionsという名前の型）のインスタンスを渡すものもある。
        ///これは、インスタンスのプロパティにプロパティ名から察して適当な値を入れて渡してやればよいし
        /// 、一部または全部の値を空のままにしてもよい。
        /// メソッドによってはnullを渡しても大丈夫なようだ。
        #endregion

        /// <summary>
        /// このクラスのヘルプです。ここの中身を参照して、このクラスの使い方を参考にしてもらえれば幸いです。
        /// 
        /// </summary>
        public static void __HELP・このクラスの使い方のヘルプ()
        {
            // もし可能なら、簡単な使用例を描く。

            // initはクラス生成時に自動的に呼びだされるから、呼びだす必要はない。

            // twitterクライアントなどで、トークンを取らせたかったら一度やってみて。botなら基本要らない。
            //MyTwitterTools.makeUserDoTwitterRecognization・ＰＩＮコードをユーザに入力させてツイッター認証();

            // Twitterに実際につぶやかせたいときはfalse。テスト用につぶやかせたくないときはtrue
            MyTwitterTools.p_isTest・テスト用にツイッターで投稿する代わりにメッセージボックスに表示するか = true;

            // TwitterAPIは基本1時間に150アクセスしかできないから、下手にやらないようがいいみたい…。

            string _message = "";
            string _info = "";

            Stopwatch _stopwatch1 = new Stopwatch();
            _stopwatch1.Reset();
            _stopwatch1.Start();
            _message = "#Test マイクテスト中…ツイートテスト中３。これはC#のTwitterAPIを使って、つぶやいています。。フォロワーのみなさん、うるさかったらごめんね。そろそろテストアカウントに移動しますね…";
            tweet・つぶやく(_message);
            _stopwatch1.Stop();
            _info = _stopwatch1.ElapsedMilliseconds + "ミリ秒かかってる ＜＝ MyTwitterTools.tweet・つぶやく(_message)";
            System.Console.WriteLine(_info);
            
            string _twitterID = "merusaia";

            bool _isTestGetNickName = false;
            if (_isTestGetNickName == true)
            {
                string _screenName = MyTwitterTools.getNickName・ニックネームを取得(_twitterID);
            }

            bool _isTestGetFollowerNum・フォロワー数取得のテスト = false;
            if (_isTestGetFollowerNum・フォロワー数取得のテスト == true)
            {
                _stopwatch1.Reset();
                _stopwatch1.Start();
                int _followerCount = MyTwitterTools.getFollowersNum・フォロワー数を取得(_twitterID);
                // 時間かかるよ _followerCount = MyTwitterTools.getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(_twitterID, true).Count;
                _stopwatch1.Stop();
                _message = "#Test " + _twitterID + " のフォロワーは、twitterizerによると " + _followerCount + " 人いるようです。…合ってます？　間違ってたらごめんね。";
                MyTwitterTools.tweet・つぶやく(_message);
                _info = _stopwatch1.ElapsedMilliseconds + "ミリ秒かかってる ＜＝ MyTwitterTools.getFollowersNum・フォロワー数を取得(_twitterID)";
                System.Console.WriteLine(_info);

                _stopwatch1.Reset();
                _stopwatch1.Start();
                //int _followerCount = MyTwitterTools.getFollowersNum・フォロワー数を取得(_twitterID);
                _followerCount = MyTwitterTools.getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(_twitterID, true).Count;
                _stopwatch1.Stop();
                _info = _stopwatch1.ElapsedMilliseconds + "ミリ秒かかってる ＜＝ MyTwitterTools.getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(_TwitterUser_Id_decimal)";
                _message = "#Test " + _twitterID + " のフォロワーは、twitterizerによると " + _followerCount + " 人いるようです。…合ってます？　間違ってたらごめんね。\nこの処理に" + (_stopwatch1.ElapsedMilliseconds / 1000.0) + "秒かかりました。…遅いね。";
                MyTwitterTools.tweet・つぶやく(_message);
                System.Console.WriteLine(_info);

                _stopwatch1.Reset();
                _stopwatch1.Start();
                //int _followerCount = MyTwitterTools.getFollowersNum・フォロワー数を取得(_twitterID);
                _followerCount = MyTwitterTools.getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(_twitterID, true).Count;
                _stopwatch1.Stop();
                _info = _stopwatch1.ElapsedMilliseconds + "ミリ秒かかってる ＜＝ MyTwitterTools.getFollowerOrFriendDecimalIDs・フォロワーかフォローしている人のユーザ識別子を全て取得＿フォロワー数が多い時は注意して(_TwitterUser_Id_decimal)";
                _message = "#Test " + _twitterID + " のフォロワーは、twitterizerによると " + _followerCount + " 人いるようです。…合ってます？　間違ってたらごめんね。\nこの処理に" + (_stopwatch1.ElapsedMilliseconds / 1000.0) + "秒かかりました。…遅いね。";
                MyTwitterTools.tweet・つぶやく(_message);
                System.Console.WriteLine(_info);
            }


            bool _isTest150Over・API１５０回オーバーのテストをする = false;
            if (_isTest150Over・API１５０回オーバーのテストをする == true)
            {
                TwitterUser _user;
                // MyTools.getUserを使えば、オーバーしないはず。
                for (int i = 0; i < 151; i++) // p_isUseDictionaryをtrueにしてたら問題無く、ポケモンゲットだぜ！
                {
                    _user = MyTwitterTools.getUser・ユーザ情報を取得(_twitterID);
                    _info = i + "回目のMyTwitterTools.getUser・ユーザ情報を取得(_twitterID)";
                    System.Console.WriteLine(_info);
                }

                // TwitterAPIの制限（１時間に１５０回）の１回に、TwitterUser.Showが含まれているか調べた→含まれてた
                // Rate limit exceed.が出たら１時間、実験できないからね。実行する時は気を付けて。
                var _res = new TwitterResponse<TwitterUser>();
                for (int i = 0; i < 151; i++) // 問題ありあり、アクセス禁止ゲットだぜ！
                {
                    _res = TwitterUser.Show(_twitterID);
                    _info = i + "回目のTwitterUser.Show(_twitterID)";
                    System.Console.WriteLine(_info);
                    // どっかで止まる。
                }
            }

            bool _isTestSeikiBunpu・正規分布っぽい値の取得テスト = true;
            if (_isTestSeikiBunpu・正規分布っぽい値の取得テスト == true)
            {
                // 処理時間を図りましょう
                _stopwatch1.Reset();
                _stopwatch1.Start();
                // ストップウォッチもいいけど、これの方が簡単だよ
                int _time1 = MyTools.getNowTime_fast();
                MyTools.showMessage_ConsoleLine("\n　↓　腕時計。よーい、スタート");

                // 例：剣技のテストの点数を、適当な平均値・最小値・最大値を使って、やんわり正規分布っぽい形で出してみる
                string _testName = "剣技";
                int _tensu;
                int _personNum = 10000; // テスト参加数。
                int _ketasuMax = 5;     //表示桁数
                int _heikinti = 60; // 平均点
                double _bunsanMin = -5; // 標準正規分布なら-1
                double _bunsanMax = 5; // 標準正規分布なら1
                // 以下は、テストの結果を格納する時に使う
                List<int> _tensuList = new List<int>();
                int _MaxTensu = 0;
                int _MinTensu = 100;

                // テスト開始
                MyTools.showMessage_ConsoleLine("\n"+_testName+"のテスト（0～100点）を、" + _personNum + "人にやってもらったよ。\n問題は、平均" + _heikinti + "点位、分散は"+_bunsanMin+"～"+_bunsanMax+"にバラツクように作りました。");
                for (int i = 1; i <= _personNum; i++)
                {
                    _tensu = MyTools.getSeikiRandomNum_RealWorldRate(_heikinti, 0, 100, _bunsanMin, _bunsanMax);
                    //MyTools.showMessage_ConsoleLine(i+"人目の数学のテストの点数,"+_tensu); // これだけ×100回で400ミリ秒かかるよ
                    _tensuList.Add(_tensu);
                    //if (_tensu > _MaxTensu) _MaxTensu = _tensu; //こんなことしなくても
                    //if (_tensu < _MinTensu) _MinTensu = _tensu;
                }
                _stopwatch1.Stop();
                int _time2 = MyTools.getNowTime_fast();
                MyTools.showMessage_ConsoleLine("　↑　テストをするだけで、"+(_time2-_time1)+"ミリ秒かかったよ。腕時計ととの精度の違いはどうかな。Stopwatch:"+_stopwatch1.ElapsedMilliseconds+"msec.");



                _stopwatch1.Reset();
                _stopwatch1.Start();
                _time1 = MyTools.getNowTime_fast();
                //MyTools.showMessage_ConsoleLine("\n　↓　腕時計。よーい、スタート");

                // 最大値と最小値を取得する
                _MaxTensu = MyTools.getBiggestValue(_tensuList); // 要素数が多い時はちょっと時間かかるかもだけどね
                _MinTensu = MyTools.getSmallestValue(_tensuList);
                MyTools.showMessage_ConsoleLine("\n最大値:" + _MaxTensu + "点、最小値:" + _MinTensu + "点、");

                // ソートする
                // _tensuList.Sort();もいいけど元のリストの中身が変更されちゃうし、降順か昇順かわからなくなるからねぇ
                // このメソッドを使うと、元のリストの中身は変更されずに、新しいソート済みのリストを作れるよ。
                List<int> _sortedTensuList =  MyTools.getSortedList(_tensuList, MyTools.ESortType.値が大きい順＿降順);
                _stopwatch1.Stop();
                _time2 = MyTools.getNowTime_fast();
                //MyTools.showMessage_ConsoleLine("　↑　最大値と最小値を計算するだけで、"+(_time2-_time1)+"ミリ秒かかったよ。腕時計ととの精度の違いはどうかな。Stopwatch:"+_stopwatch1.ElapsedMilliseconds+"msec.");


                _stopwatch1.Reset();
                _stopwatch1.Start();
                _time1 = MyTools.getNowTime_fast();
                //MyTools.showMessage_ConsoleLine("\n　↓　腕時計。よーい、スタート");
                // 平均値と標準偏差と分散を出してみる
                double _average = MyTools.getAverage_InList(_tensuList);
                double _bunsan = MyTools.getBunsan_InList(_tensuList);
                double _hyouzyunHensa = MyTools.getHyouzyunHensa_InList(_tensuList);
                MyTools.showMessage_ConsoleLine("平均値:" + _average + "点、分散" + _bunsan + "、標準偏差" + _hyouzyunHensa + "点、です。"
                    +"トップは"+_MaxTensu+"点、ビリは"+_MinTensu+"点でした。\n"
                    +"正規分布に従えば、約６８％の人が "+MyTools.getSisyagonyuValue(_average)+"±"+MyTools.getSisyagonyuValue(_hyouzyunHensa)+"点のところにいるかも。");
                // 同じものをまとめて計算したい時はこれ。forループが一回なので一番早く計算できるよ
                MyTools.getAnalyzedValues_InList(_tensuList, out _MinTensu, out _MaxTensu, out _average, out _bunsan, out _hyouzyunHensa);
                //MyTools.showMessage_ConsoleLine("\n平均" + _average + "　分散" + _bunsan + "　標準偏差" + _hyouzyunHensa + "　です。標準正規分布は分散1ですよ。");
                _stopwatch1.Stop();
                _time2 = MyTools.getNowTime_fast();
                //MyTools.showMessage_ConsoleLine("　↑　ここまでで、" + (_time2 - _time1) + "ミリ秒かかったよ。腕時計ととの精度の違いはどうかな。Stopwatch:" + _stopwatch1.ElapsedMilliseconds + "msec.");



                _stopwatch1.Reset();
                _stopwatch1.Start();
                _time1 = MyTools.getNowTime_fast();
                MyTools.showMessage_ConsoleLine("\n　↓　腕時計。よーい、スタート");

                // ランク情報
                // これを使うと、一定の値の範囲をＥ～ＳＳＳＳランクとして定義することができるよ。
                // 参考：大学の合格判定は、Ｅ５％　Ｄ２０％　Ｃ５０％　Ｂ５０～７５％　Ａ９０％　らしい。

                MyTools.setERank_FtoS_ByMin(1,20,30,40,60,80,90,95,98,100);
                string _rankInfo = MyTools.getERankInfo();
                MyTools.showMessage_ConsoleLine(_rankInfo);
                // ランク情報を更新。これを使うと、一定の人数の範囲をＥ～ＳＳＳＳランクとして定義することができるよ。
                //MyTools.setERank_FtoS_ByInclusingRate(0.001, 8, 10, 15, 20, 30, 20, 1, 0.1, 0.01, 0.001);
                // この最小値が、一つの上の人数のやつとだいたい一緒の値になる。
                MyTools.setERank_FtoS_ByMin(0.001, 8, 17, 30, 50, 80, 99, 99.9, 99.99, 99.999);
                _rankInfo = MyTools.getERankInfo();
                MyTools.showMessage_ConsoleLine("※ランク情報を以下に変更します。"+_rankInfo);

                // ランキングしてみる。
                MyTools.showMessage_ConsoleLine("【ランキング結果】");
                // リストをコピー
                List<int> _uniqueTensuList = MyTools.getCopyedList(_sortedTensuList);
                // 値の重複をなくす
                MyTools.removeSameValue_InList(_uniqueTensuList);
                int _ranking_TopMax = 10; // 上位10位までを紹介
                int _rankingNo = 1; // ランキング数。

                // トップランキング
                MyTools.showMessage_ConsoleLine("【トップランキング】");
                for (int i = 0; i < _ranking_TopMax; i++)
                {
                    // もし生徒数が1名未満でも、エラーを起こさず値を取ってこれるように、getListValueを使う
                    int _Noi_tensu = MyTools.getListValue(_uniqueTensuList, i); // 既に降順ソートされているからi番目がi位
                    // 同じ値の要素数を取ってくる
                    int _Noi_persons = MyTools.getSameValueCount_InList<int>(_sortedTensuList, _Noi_tensu);
                    // 桁数が違っても表示をそろえたいので、getStringNumberを使う
                    MyTools.showMessage_ConsoleLine(MyTools.getStringNumber(_rankingNo, true, _ketasuMax, 0) + "位：" + MyTools.getStringNumber(_Noi_tensu, true, 3, 0) + "点（" + MyTools.getStringNumber(_Noi_persons, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_Noi_persons / (double)_personNum * 100, true, 3, 6) + "％）　→　" + MyTools.getERank_FtoS(_Noi_tensu) + "ランク");
                    _rankingNo += _Noi_persons; // ランキング数に既にランクインした上位人数を足していく
                }
                // トップランキングをして全ての点数を列挙していなければ
                if (_uniqueTensuList.Count > _ranking_TopMax)
                {
                    // 関係ないけど、中間値（平均値とはちょっとだけ違う）があるインデックスもこんな簡単に取ってこれるよ
                    int _ranking_midTop = MyTools.getIndex_Middle(_uniqueTensuList);
                    
                    // 任意の値に一番近いインデックスもこんな簡単に取ってこれるよ
                    int _ranking_averageTop = MyTools.getIndex_MostClosed(_uniqueTensuList, (int)_average);
                    // 平均値付近のランキングが欲しいので、半分上位にする
                    if (_ranking_averageTop - _ranking_TopMax / 2 > _ranking_TopMax)
                    {
                        _ranking_averageTop = _ranking_averageTop - _ranking_TopMax / 2; // トップランキングとかぶってなかったら、半分上位にする
                    }
                    else
                    {
                        _ranking_averageTop = _ranking_TopMax + 1; // トップランキングの続きから表示
                    }
                    // ランキングNoを更新
                    for(int i=_ranking_TopMax; i<_ranking_averageTop; i++){
                        // もし生徒数が1名未満でも、エラーを起こさず値を取ってこれるように、getListValueを使う
                        int _Noi_tensu = MyTools.getListValue(_uniqueTensuList, i);
                        // 同じ値の要素数（重複数）を取ってくる
                        int _Noi_persons = MyTools.getSameValueCount_InList<int>(_sortedTensuList, _Noi_tensu);
                        _rankingNo += _Noi_persons;
                    }

                    MyTools.showMessage_ConsoleLine("【平均付近ランキング】");
                    for (int i = _ranking_averageTop ; i < _ranking_averageTop + _ranking_TopMax; i++)
                    {
                        // もし生徒数が1名未満でも、エラーを起こさず値を取ってこれるように、getListValueを使う
                        int _Noi_tensu = MyTools.getListValue(_uniqueTensuList, i);
                        // 同じ値の要素数（重複数）を取ってくる
                        int _Noi_persons = MyTools.getSameValueCount_InList<int>(_sortedTensuList, _Noi_tensu);
                        // 桁数が違っても表示をそろえたいので、getStringNumberを使う
                        MyTools.showMessage_ConsoleLine(MyTools.getStringNumber(_rankingNo, true, _ketasuMax, 0) + "位：" + MyTools.getStringNumber(_Noi_tensu, true, 3, 0) + "点（" + MyTools.getStringNumber(_Noi_persons, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_Noi_persons / (double)_personNum * 100, true, 3, 6) + "％）　→　" + MyTools.getERank_FtoS(_Noi_tensu) + "ランク");
                        _rankingNo += _Noi_persons; // ランキング数に既にランクインした上位人数を足していく

                        // _ranking_TopMaxととかぶったら終わり
                        if (i <= _ranking_TopMax)
                        {
                            break;
                        }
                    }
                }
                // トップランキングをして全ての点数を列挙していなければ
                if (_uniqueTensuList.Count > _ranking_TopMax)
                {
                    // ランキングNoをワーストに更新
                    _rankingNo = _personNum;
                    MyTools.showMessage_ConsoleLine("【ワーストランキング】");
                    // ワーストランキングは逆順
                    for (int i = _uniqueTensuList.Count - 1; i > (_uniqueTensuList.Count - 1) - _ranking_TopMax; i--)
                    {
                        // もし生徒数が1名未満でも、エラーを起こさず値を取ってこれるように、getListValueを使う
                        int _Noi_tensu = MyTools.getListValue(_uniqueTensuList, i);
                        // 同じ値の要素数（重複数）を取ってくる
                        int _Noi_persons = MyTools.getSameValueCount_InList<int>(_sortedTensuList, _Noi_tensu);
                        // 桁数が違っても表示をそろえたいので、getStringNumberを使う
                        MyTools.showMessage_ConsoleLine(MyTools.getStringNumber(_rankingNo, true, _ketasuMax, 0) + "位：" + MyTools.getStringNumber(_Noi_tensu, true, 3, 0) + "点（" + MyTools.getStringNumber(_Noi_persons, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_Noi_persons / (double)_personNum * 100, true, 3, 6) + "％）　→　" + MyTools.getERank_FtoS(_Noi_tensu) + "ランク");
                        _rankingNo -= _Noi_persons; // ワーストランキング数に既にランクインした上位人数を除いていく

                        // _ranking_TopMaxととかぶったら終わり
                        if (i <= _ranking_TopMax)
                        {
                            break;
                        }
                    }
                }
                // 値が任意の範囲にある人数やパーセンテージを出してみよう。
                // その１
                int _range1 = 60;
                int _range2 = 70;
                // 値が任意の範囲にあるリストの要素数を簡単に取ってこれるよ
                int _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                // 桁数が違っても表示をそろえたいので、getStringNumberを使う
                MyTools.showMessage_ConsoleLine("※"+_range1 + "～" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです");
                // その２
                _range1 = 70; 
                _range2 = 80;
                _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                MyTools.showMessage_ConsoleLine("※" + _range1 + "～" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです");
                // その３
                _range1 = 80;
                _range2 = 90;
                _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                MyTools.showMessage_ConsoleLine("※" + _range1 + "～" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです");
                // その４
                _range1 = 90;
                _range2 = 100;
                _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                MyTools.showMessage_ConsoleLine("※" + _range1 + "-" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです");
                // 標準偏差を使って、正規分布の確率６８％に近いかを確認
                _range1 = MyTools.getSisyagonyuValue(_average - _hyouzyunHensa); // 四捨五入が簡単にできるよ
                _range2 = MyTools.getSisyagonyuValue(_average + _hyouzyunHensa);
                _rangeNum = MyTools.getValueCount_InList(_sortedTensuList, _range1, _range2);
                MyTools.showMessage_ConsoleLine("※" + _range1 + "～" + _range2 + "点の人は、" + MyTools.getStringNumber(_rangeNum, true, _ketasuMax, 0) + "人 / " + MyTools.getStringNumber(_personNum, true, _ketasuMax, 0) + "人 （" + MyTools.getStringNumber((double)_rangeNum / (double)_personNum * 100.0, true, 3, 6) + "％）　いるみたいです。正規分布に従えば、平均値"+_average+"±標準偏差"+MyTools.getSisyagonyuValue(_hyouzyunHensa)+"にいる範囲は約６８％のはず。近かったかな？");

                _stopwatch1.Stop();
                _time2 = MyTools.getNowTime_fast();
                MyTools.showMessage_ConsoleLine("　↑　ここまでで、" + (_time2 - _time1) + "ミリ秒かかったよ。腕時計ととの精度の違いはどうかな。Stopwatch:" + _stopwatch1.ElapsedMilliseconds + "msec.");
            }

            System.Windows.Forms.Application.Exit();

        }

        #endregion
    }
}
