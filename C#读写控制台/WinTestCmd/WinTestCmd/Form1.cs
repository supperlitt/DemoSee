using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinTestCmd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            ProcessTool.ShowMsg += ProcessTool_ShowMsg;
        }

        private void btnShell_Click(object sender, EventArgs e)
        {
            this.btnShell.Enabled = false;
            if (this.chkAsyc.Checked)
            {
                Thread t = new Thread(() =>
                {
                    ProcessTool.ProecssCmd("ping", "www.baidu.com");
                    this.btnShell.Enabled = true;
                });
                t.IsBackground = true;
                t.Start();
            }
            else
            {
                Thread t = new Thread(() =>
                {
                    this.txtResult.Text = ProcessTool.ProcessCmd_2("ping", "www.baidu.com");
                    this.btnShell.Enabled = true;
                });
                t.IsBackground = true;
                t.Start();
            }
        }

        private void ProcessTool_ShowMsg(string obj)
        {
            this.Invoke(new Action<TextBox>(p =>
            {
                if (p.TextLength > 30000)
                {
                    p.Clear();
                }

                p.AppendText(obj + "\r\n");
            }), this.txtResult);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
