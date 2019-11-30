using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinTestSocketRaw
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            HttpWatch watch = new HttpWatch(this.txtIP.Text);
            watch.DoMsg += watch_DoMsg;
            watch.Start();
        }

        private void watch_DoMsg(string msg)
        {
            this.Invoke(new Action<TextBox>(p =>
            {
                if (p.TextLength > 30000)
                {
                    p.Clear();
                }

                p.AppendText(msg + "\r\n");
            }), this.txtResult);
        }
    }
}
