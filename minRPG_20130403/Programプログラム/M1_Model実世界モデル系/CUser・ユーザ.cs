using System;
using System.Collections.Generic;
using System.Text;

namespace PublicDomain
{

    /// <summary>
    /// このゲームをプレイしている利用者（ユーザ）です．
    /// </summary>
    [Serializable()]// 以下の[]はクラスのディープコピー（中身のまるごとコピー）を作成する時に必要 http://d.hatena.ne.jp/tekk/20100131/1264913887
    public class ユーザ
    {
        private CUser・ユーザ p_usedClass = new CUser・ユーザ();
        // [Tips]動的プロパティを動的プロパティで初期化できないため，こういう記述はできない．（ただ出来たとしても，プロパティを２つのメモリに持つのはよくない設計だが）
        // public string 名前 = p_usedClass.getP_name;

        /// <summary>
        /// あなたの名前（称号）
        /// </summary>
        /// <returns></returns>
        public string name名前()
        {
            return p_usedClass.getP_name();
        }
        public void set名前登録(string _ユーザ名)
        {
            p_usedClass.setP_name(_ユーザ名);
        }


        /// <summary>
        /// ユーザ識別ID
        /// </summary>
        /// <returns></returns>
        public string id()
        {
            return p_usedClass.getP_id();
        }
        public void setid登録(string _id名)
        {
            p_usedClass.setP_id(_id名);
        }

        /// <summary>
        /// パスワード
        /// </summary>
        /// <returns></returns>
        public string passパスワード()
        {
            return p_usedClass.getP_password();
        }
        public void setパスワード登録(string _パスワード)
        {
            p_usedClass.setP_password(_パスワード);
        }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ユーザ(string _名前, string _ID, string _パスワード)
        {
            set名前登録(_名前);
            setid登録(_ID);
            setパスワード登録(_パスワード);
        }

    }


    /// <summary>
    /// このゲームをプレイしている利用者（ユーザ）です．
    /// </summary>
    public class CUser・ユーザ
    {
        /// <summary>
        /// ユーザ名
        /// </summary>
        public string p_name = "";
        public string getP_name()
        {
            return p_name;
        }
        public void setP_name(string _name)
        {
            p_name = _name;
        }

        /// <summary>
        /// _id
        /// </summary>
        public string p_id = "";
        public string getP_id()
        {
            return p_id;
        }
        public void setP_id(string _id)
        {
            p_id = _id;
        }

        /// <summary>
        /// パスワード
        /// </summary>
        public string p_password = "";
        public string getP_password()
        {
            return p_password;
        }
        public void setP_password(string _password)
        {
            p_password = _password;
        }
    }
}
