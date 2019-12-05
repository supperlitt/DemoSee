using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace WinWMI
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            SelectQuery query = new SelectQuery("Select * from Win32_Process");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            try
            {
                foreach (ManagementObject disk in searcher.Get())
                {
                    int pid = int.Parse(disk["ProcessId"].ToString());
                    string command = disk["CommandLine"] == null ? "" : disk["CommandLine"].ToString();
                    dic.Add(pid, command);
                }
            }
            catch
            {
            }

            List<PInfo> pList = new List<PInfo>();
            Process[] ps = Process.GetProcesses();
            foreach (var p in ps)
            {
                try
                {
                    pList.Add(new PInfo() { name = p.ProcessName, path = p.MainModule.FileName, commandline = dic.ContainsKey(p.Id) ? dic[p.Id] : "" });
                }
                catch { }
            }

            this.lstSite.Items.Clear();
            foreach (var p in pList)
            {
                ListViewItem item = new ListViewItem(p.name);
                item.SubItems.AddRange(new string[] { p.path, p.commandline });
                this.lstSite.Items.Add(item);
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.lstSite.FullRowSelect = true;
            this.lstSite.Columns.Add("名称", 130, HorizontalAlignment.Left);
            this.lstSite.Columns.Add("路径", 130, HorizontalAlignment.Left);
            this.lstSite.Columns.Add("参数", 130, HorizontalAlignment.Left);
        }
    }

    public class PInfo
    {
        public string name { get; set; }

        public string path { get; set; }

        public string commandline { get; set; }
    }
}
