using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ToText
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        // 点击“转换”事件
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string img_Path = txt_imgpath.Text.Trim();  // 图片地址
            if (String.IsNullOrEmpty(img_Path))
            {
                MessageBox.Show("请先输入图片地址！");
                return;
            }
            try
            {
                MODI.Document doc = new MODI.Document();
                doc.Create(img_Path);
                MODI.Image image;
                MODI.Layout layout;
                doc.OCR(GetLanuageType(cmb_languagetype.SelectedValue.ToString()), true, true);  // 识别文字类型
                for (int i = 0; i < doc.Images.Count; i++)
                {
                    image = (MODI.Image)doc.Images[i];
                    layout = image.Layout;
                    sb.Append(layout.Text);
                }
                txt_result.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                txt_result.Text = "转换失败！详情：" + ex.Message;
            }
        }

        // 浏览事件
        private void button2_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            txt_imgpath.Text = openFileDialog1.FileName;
        }

        // 窗体加载事件
        private void Form2_Load(object sender, EventArgs e)
        {
            List<ListItem> list = new List<ListItem>();
            ListItem item = new ListItem();

            // 数值对应 以MODI.MiLANGUAGES 枚举类为准
            list.Add(new ListItem("简体中文", "2052"));
            list.Add(new ListItem("繁体中文", "1028"));
            list.Add(new ListItem("英语", "9"));
            list.Add(new ListItem("捷克语", "5"));
            list.Add(new ListItem("丹麦语", "6"));
            list.Add(new ListItem("德语", "7"));
            list.Add(new ListItem("希腊语", "8"));
            list.Add(new ListItem("西班牙语", "10"));
            list.Add(new ListItem("芬兰语", "11"));
            list.Add(new ListItem("法语", "12"));
            list.Add(new ListItem("匈牙利语", "14"));
            list.Add(new ListItem("意大利语", "16"));
            list.Add(new ListItem("日语", "17"));
            list.Add(new ListItem("韩语", "18"));
            list.Add(new ListItem("荷兰语", "19"));
            list.Add(new ListItem("挪威语", "20"));
            list.Add(new ListItem("波兰语", "21"));
            list.Add(new ListItem("葡萄牙语", "22"));
            list.Add(new ListItem("俄语", "25"));
            list.Add(new ListItem("瑞典语", "29"));
            list.Add(new ListItem("土耳其语", "31"));
            cmb_languagetype.DataSource = list;
            cmb_languagetype.ValueMember = "_sValue";
            cmb_languagetype.DisplayMember = "_sText";
        }


        private MODI.MiLANGUAGES GetLanuageType(string sValue)
        {
            switch (sValue)
            {
                case "2052":
                    return MODI.MiLANGUAGES.miLANG_CHINESE_SIMPLIFIED;
                case "5":
                    return MODI.MiLANGUAGES.miLANG_CZECH;
                case "6":
                    return MODI.MiLANGUAGES.miLANG_DANISH;
                case "7":
                    return MODI.MiLANGUAGES.miLANG_GERMAN;
                case "8":
                    return MODI.MiLANGUAGES.miLANG_GREEK;
                case "9":
                    return MODI.MiLANGUAGES.miLANG_ENGLISH;
                case "10":
                    return MODI.MiLANGUAGES.miLANG_SPANISH;
                case "11":
                    return MODI.MiLANGUAGES.miLANG_FINNISH;
                case "12":
                    return MODI.MiLANGUAGES.miLANG_FRENCH;
                case "14":
                    return MODI.MiLANGUAGES.miLANG_HUNGARIAN;
                case "16":
                    return MODI.MiLANGUAGES.miLANG_ITALIAN;
                case "17":
                    return MODI.MiLANGUAGES.miLANG_JAPANESE;
                case "18":
                    return MODI.MiLANGUAGES.miLANG_KOREAN;
                case "19":
                    return MODI.MiLANGUAGES.miLANG_DUTCH;
                case "20":
                    return MODI.MiLANGUAGES.miLANG_NORWEGIAN;
                case "21":
                    return MODI.MiLANGUAGES.miLANG_POLISH;
                case "22":
                    return MODI.MiLANGUAGES.miLANG_PORTUGUESE;
                case "25":
                    return MODI.MiLANGUAGES.miLANG_RUSSIAN;
                case "29":
                    return MODI.MiLANGUAGES.miLANG_SWEDISH;
                case "31":
                    return MODI.MiLANGUAGES.miLANG_TURKISH;
                case "1028":
                    return MODI.MiLANGUAGES.miLANG_CHINESE_TRADITIONAL;
                default:
                    return MODI.MiLANGUAGES.miLANG_ENGLISH;
            }
        }

    }
}
