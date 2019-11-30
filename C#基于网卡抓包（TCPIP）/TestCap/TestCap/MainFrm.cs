using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TestCap
{
    public partial class MainFrm : Form
    {
        private static object lockObj = new object();
        private string wlanIp = "";

        public MainFrm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.wlanIp = this.txtIp.Text;
            WinCapHelper.WinCapInstance.NotifyPacket += WinCapInstance_NotifyPacket;
            WinCapHelper.WinCapInstance.Listen(this.txtLanCard.Text);
        }

        private int index = 0;

        void WinCapInstance_NotifyPacket(string protocol, string sourceIP, string sourcePort, string targetIP, string targetPort, string msg, uint key, byte[] bytes)
        {
            Regex hostRegex = new Regex(@"Host:\s*([^\s\r\n]+)");
            lock (lockObj)
            {
                if (bytes == null)
                {
                    return;
                }

                string host = "";
                string content = Encoding.UTF8.GetString(bytes);
                host = hostRegex.Match(content).Groups[1].Value;
                if (!string.IsNullOrEmpty(host))
                {

                }
                if (sourceIP == this.wlanIp)
                {
                    this.Invoke(new Action<ListView>(p =>
                    {
                        p.BeginUpdate();
                        if (bytes != null)
                        {
                            SendDataCache.AddData(key, bytes);
                        }
                        else
                        {
                            return;
                        }

                        bool ok = false;
                        for (int i = 0; i < p.Items.Count; i++)
                        {
                            ListViewItem viewItem = p.Items[i];
                            string currentKey = viewItem.SubItems[6].Text;
                            if (currentKey == key.ToString())
                            {
                                // 叠加
                                string text = viewItem.Tag as string;
                                text += "\r\n=========================\r\n";
                                text += msg;
                                p.Items[i].Tag = text;
                                p.Items[i].SubItems[6].Text = bytes.Length.ToString();
                                if (!string.IsNullOrEmpty(host))
                                {
                                    p.Items[i].SubItems[7].Text = host;
                                }

                                ok = true;
                                break;
                            }
                        }

                        if (!ok)
                        {
                            ListViewItem item = new ListViewItem(index.ToString());
                            item.Tag = msg;
                            index++;
                            item.SubItems.AddRange(new string[] { protocol, sourceIP, sourcePort, targetIP, targetPort, 
                                key.ToString(), host, SendDataCache.GetData(key).Length.ToString() });

                            p.Items.Add(item);
                        }

                        p.EndUpdate();
                    }), this.listView1);
                }
                else
                {
                    this.Invoke(new Action<ListView>(p =>
                    {
                        p.BeginUpdate();
                        if (bytes != null)
                        {
                            ReceiveDataCache.AddData(key, bytes);
                        }
                        else
                        {
                            return;
                        }

                        bool ok = false;
                        for (int i = 0; i < p.Items.Count; i++)
                        {
                            ListViewItem viewItem = p.Items[i];
                            string currentKey = viewItem.SubItems[6].Text;
                            if (currentKey == key.ToString())
                            {
                                // 叠加
                                string text = viewItem.Tag as string;
                                text += "\r\n=========================\r\n";
                                text += msg;
                                p.Items[i].Tag = text;
                                p.Items[i].SubItems[6].Text = bytes.Length.ToString();
                                if (!string.IsNullOrEmpty(host))
                                {
                                    p.Items[i].SubItems[7].Text = host;
                                }

                                ok = true;
                                break;
                            }
                        }

                        if (!ok)
                        {
                            ListViewItem item = new ListViewItem(index.ToString());
                            item.Tag = msg;
                            index++;
                            item.SubItems.AddRange(new string[] { protocol, sourceIP, sourcePort, targetIP, targetPort, 
                                key.ToString(), host, ReceiveDataCache.GetData(key).Length.ToString() });

                            p.Items.Add(item);
                        }

                        p.EndUpdate();
                    }), this.listView2);
                }
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.listView1.FullRowSelect = true;
            this.listView1.Columns.Add("索引", 60, HorizontalAlignment.Left);
            this.listView1.Columns.Add("协议", 80, HorizontalAlignment.Left);
            this.listView1.Columns.Add("源IP", 110, HorizontalAlignment.Left);
            this.listView1.Columns.Add("源端口", 50, HorizontalAlignment.Left);
            this.listView1.Columns.Add("目标IP", 110, HorizontalAlignment.Left);
            this.listView1.Columns.Add("目标端口", 60, HorizontalAlignment.Left);
            this.listView1.Columns.Add("标志位", 110, HorizontalAlignment.Left);
            this.listView1.Columns.Add("Host", 170, HorizontalAlignment.Left);
            this.listView1.Columns.Add("DataSize", 60, HorizontalAlignment.Left);

            this.listView2.FullRowSelect = true;
            this.listView2.Columns.Add("索引", 60, HorizontalAlignment.Left);
            this.listView2.Columns.Add("协议", 80, HorizontalAlignment.Left);
            this.listView2.Columns.Add("源IP", 110, HorizontalAlignment.Left);
            this.listView2.Columns.Add("源端口", 50, HorizontalAlignment.Left);
            this.listView2.Columns.Add("目标IP", 110, HorizontalAlignment.Left);
            this.listView2.Columns.Add("目标端口", 60, HorizontalAlignment.Left);
            this.listView2.Columns.Add("标志位", 110, HorizontalAlignment.Left);
            this.listView2.Columns.Add("Host", 170, HorizontalAlignment.Left);
            this.listView2.Columns.Add("DataSize", 60, HorizontalAlignment.Left);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ListView).Name.Contains("1"))
            {
                if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
                {
                    var item = this.listView1.SelectedItems[0];
                    string msg = item.Tag as string;

                    this.txtResult.Text = msg;
                    SetSendMsg(uint.Parse(item.SubItems[6].Text));
                }
            }
            else
            {
                if (this.listView2.SelectedItems != null && this.listView2.SelectedItems.Count > 0)
                {
                    var item = this.listView2.SelectedItems[0];
                    string msg = item.Tag as string;

                    this.txtResult.Text = msg;
                    SetReceiveMsg(uint.Parse(item.SubItems[6].Text));
                }
            }
        }

        private void SetSendMsg(uint key)
        {
            byte[] bs = SendDataCache.GetData(key);
            if (bs != null)
            {
                Encoding encode = Encoding.Default;
                if (this.rbtnUnicode.Checked)
                {
                    encode = Encoding.Unicode;
                }
                else if (this.rbtnAscii.Checked)
                {
                    encode = Encoding.ASCII;
                }
                else if (this.rbtnDefault.Checked)
                {
                    encode = Encoding.Default;
                }
                else if (this.rbtnutf8.Checked)
                {
                    encode = Encoding.UTF8;
                }
                else if (this.rbtnHex.Checked)
                {
                    // Hex
                }

                if (this.chkGzip.Checked)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(bs))
                        {
                            using (GZipStream gs = new GZipStream(ms, CompressionMode.Decompress))
                            {

                                using (StreamReader sr = new StreamReader(gs, encode))
                                {
                                    this.txtEncode.Text = sr.ReadToEnd().Replace('\0', '.');
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtEncode.Text = ex.Message;
                    }
                }
                else
                {
                    this.txtEncode.Text = encode.GetString(bs).Replace('\0', '.');
                }
            }
        }

        private void SetReceiveMsg(uint key)
        {
            byte[] bs = ReceiveDataCache.GetData(key);
            if (bs != null)
            {
                Encoding encode = Encoding.Default;
                if (this.rbtnUnicode.Checked)
                {
                    encode = Encoding.Unicode;
                }
                else if (this.rbtnAscii.Checked)
                {
                    encode = Encoding.ASCII;
                }
                else if (this.rbtnDefault.Checked)
                {
                    encode = Encoding.Default;
                }
                else
                {
                    encode = Encoding.UTF8;
                }

                if (this.chkGzip.Checked)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(bs))
                        {
                            using (GZipStream gs = new GZipStream(ms, CompressionMode.Decompress))
                            {

                                using (StreamReader sr = new StreamReader(gs, encode))
                                {
                                    this.txtEncode.Text = sr.ReadToEnd().Replace('\0', '.');
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.txtEncode.Text = ex.Message;
                    }
                }
                else
                {
                    this.txtEncode.Text = encode.GetString(bs).Replace('\0', '.');
                }
            }
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WinCapHelper.WinCapInstance.StopAll();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            WinCapHelper.WinCapInstance.StopAll();
        }

        private void rbtn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                var item = this.listView1.SelectedItems[0];
                SetSendMsg(uint.Parse(item.SubItems[6].Text));
            }
            else if (this.listView2.SelectedItems != null && this.listView2.SelectedItems.Count > 0)
            {
                var item = this.listView2.SelectedItems[0];
                SetReceiveMsg(uint.Parse(item.SubItems[6].Text));
            }
        }
    }
}
