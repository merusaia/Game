using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PublicDomain
{
    public partial class EnumForm : Form
    {
        Enum originData;
        long _nowData;
        long nowData {
            get { return _nowData; }
            set {
				//変更があればテキストボックスにセット
                TextNow10.Text = value.ToString();
                TextNow2.Text = "0b" +  Convert.ToString(value,2);
                TextNow16.Text = "0x" + Convert.ToString(value,16);
                _nowData = value; }
        }
        long[] EnumItems;
        bool isLoading = false;
        public void SetEnumObject(Enum value)
        {
            originData = value;
        }
        private void SetEnumObject()
        {
            listView1.Items.Clear();
			//ItemCheckイベント抑止
            isLoading = true;
			//Enumからlongに。
            nowData = Convert.ToInt64(originData);
			//型取得
            Type enumType = originData.GetType();
			//基本型
            Type baseType = Enum.GetUnderlyingType(enumType);
            textBox1.Text = enumType.FullName;
            textBox2.Text = baseType.Name;

			//要素取得
            Array ary = Enum.GetValues(enumType);
            EnumItems = new long[ary.Length];
            for (int i = 0; i < ary.Length; i++)
            {
				//long型の配列に変換
                EnumItems[i] = Convert.ToInt64(ary.GetValue(i));
				//リストビューの項目を作成
                listView1.Items.Add(ary.GetValue(i).ToString()).SubItems.AddRange(
                    new string[]
                    {
                        Convert.ChangeType(ary.GetValue(i),baseType).ToString(),
                        Convert.ToString(EnumItems[i],16),
                        Convert.ToString(EnumItems[i],2)
                    }
                    );
            }

			//Flags属性がついている場合
            if (Attribute.IsDefined(enumType, typeof(FlagsAttribute)))
            { 
                listView1.CheckBoxes = true;
                SetCheck(originData);
            }
            else //ついていない場合
            {
                listView1.SelectedIndices.Add( Array.IndexOf(EnumItems,Convert.ToInt64(originData) ) );
                listView1.SelectedIndexChanged += (a, b) =>
                    {
                        if (listView1.SelectedIndices.Count > 0)
                            nowData = EnumItems[listView1.SelectedIndices[0]];
                        
                    };
            }
            listView1.Select();
            isLoading = false;
        }

		/// <summary>
		/// ビジュアライザで変更されたデータを取得
		/// </summary>
		/// <returns>変更後の列挙体オブジェクト</returns>
        public object GetResult()
        {
			//現在のデータ(long)からEnumを作成
            return Enum.ToObject(  originData.GetType()  , nowData );
        }

        void SetCheck(object value)
        {
			isLoading = true;
            int count = 0;
			//Enumからlongに
            long val = Convert.ToInt64(value);
			//すべての値を列挙
            foreach (var x in Enum.GetValues(value.GetType()))
            {
                long v = Convert.ToInt64(x);
				//値が0の場合は0の場合のみチェック
				if(v == 0)
				{
					listView1.Items[count].Checked = (val == 0);
				} else { //フラグが立っていればチェック
					listView1.Items[count].Checked = (val & v) == v;
				}
                count++;
            }
			isLoading = false;
        }

        private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
			//プログラムからCheckを付けているときは行わない。(忘れると無限ループ)
            if (!isLoading)
            {
				//チェック状態に応じてフラグをON/OFF
                if (e.NewValue == CheckState.Checked)
                    nowData |= EnumItems[e.Index];
                else
                    nowData &= ~EnumItems[e.Index];
				//他のフラグに影響することのもあるので、すべてのチェックを再評価
                SetCheck(Enum.ToObject(originData.GetType(),nowData));
            }
        }

        public EnumForm()
        {
            InitializeComponent();
        }


        private void EnumForm_Load(object sender, EventArgs e)
        {
            SetEnumObject();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            SetEnumObject();
        }

    }
}
