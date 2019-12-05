using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinIIS
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            // get directory service
            DirectoryEntry Services = new DirectoryEntry("IIS://localhost/W3SVC");
            IEnumerator ie = Services.Children.GetEnumerator();
            DirectoryEntry Server = null;

            List<site_info> list = new List<site_info>();
            // find iis website
            while (ie.MoveNext())
            {
                Server = (DirectoryEntry)ie.Current;
                if (Server.SchemaClassName == "IIsWebServer")
                {
                    string name = Server.Properties["ServerComment"][0].ToString();
                    string binding = Server.Properties["Serverbindings"][0].ToString();
                    string appPoolId = Server.Properties["AppPoolId"][0].ToString();
                    string path = "";
                    IEnumerator childIe = Server.Children.GetEnumerator();
                    while (childIe.MoveNext())
                    {
                        DirectoryEntry root = (DirectoryEntry)childIe.Current;
                        if (root.Name == "ROOT")
                        {
                            path = root.Properties["Path"][0].ToString();
                            break;
                        }
                    }

                    list.Add(new site_info() { name = name, binding = binding, apppool = appPoolId, path = path });
                }
            }

            this.lstSite.Items.Clear();
            foreach (var model in list)
            {
                ListViewItem item = new ListViewItem(model.name);
                item.SubItems.AddRange(new string[] { model.binding, model.path });
                this.lstSite.Items.Add(item);
            }
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.lstSite.FullRowSelect = true;
            this.lstSite.Columns.Add("名称", 130, HorizontalAlignment.Left);
            this.lstSite.Columns.Add("IP端口", 130, HorizontalAlignment.Left);
            this.lstSite.Columns.Add("路径", 200, HorizontalAlignment.Left);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CreateWebsite("test1234", 18001, @"D:\test", "test1234");
        }

        public static void CreateWebsite(string name, int port, string rootPath, string appPool)
        {
            string IIsWebServer = "IIsWebServer";

            // validate root path
            if (System.IO.Directory.Exists(rootPath) == false)
            {
                throw new Exception("找不到 " + rootPath);
            }

            // get directory service
            DirectoryEntry Services = new DirectoryEntry("IIS://localhost/W3SVC");

            // get server name (index)
            int index = 0;
            foreach (DirectoryEntry server in Services.Children)
            {
                if (server.SchemaClassName == "IIsWebServer")
                {
                    if (server.Properties["ServerComment"][0].ToString() == name)
                    {
                        throw new Exception("website:" + name + " already exists.");
                    }

                    if (Convert.ToInt32(server.Name) > index)
                    {
                        index = Convert.ToInt32(server.Name);
                    }
                }
            }
            index++; // new index created

            // create website
            DirectoryEntry Server = Services.Children.Add(index.ToString(), IIsWebServer);
            Server.Properties["ServerComment"].Clear();
            Server.Properties["ServerComment"].Add(name);
            Server.Properties["Serverbindings"].Clear();
            Server.Properties["Serverbindings"].Add(":" + port + ":");

            string IIsVirtualDir = "IIsWebVirtualDir";
            // create ROOT for website
            DirectoryEntry root = Server.Children.Add("ROOT", IIsVirtualDir);
            root.Properties["path"].Clear();
            root.Properties["path"].Add(rootPath);

            // create application
            if (string.IsNullOrEmpty(appPool))
            {
                root.Invoke("appCreate", 0);
            }
            else
            {
                // use application pool
                root.Invoke("appCreate3", 0, appPool, true);
            }

            root.Properties["AppFriendlyName"].Clear();
            root.Properties["AppIsolated"].Clear();
            root.Properties["AccessFlags"].Clear();
            root.Properties["FrontPageWeb"].Clear();
            root.Properties["AppFriendlyName"].Add(root.Name);
            root.Properties["AppIsolated"].Add(2);
            root.Properties["AccessFlags"].Add(513);
            root.Properties["FrontPageWeb"].Add(1);

            // commit changes
            root.CommitChanges();
            Server.CommitChanges();
        }

        private void btnReStart_Click(object sender, EventArgs e)
        {
            DirectoryEntry Services = new DirectoryEntry("IIS://localhost/W3SVC");
            IEnumerator ie = Services.Children.GetEnumerator();
            DirectoryEntry Server = null;

            List<site_info> list = new List<site_info>();
            // find iis website
            while (ie.MoveNext())
            {
                Server = (DirectoryEntry)ie.Current;
                if (Server.SchemaClassName == "IIsWebServer")
                {
                    string name = Server.Properties["ServerComment"][0].ToString();
                    if (name == "test1234")
                    {
                        Server.Invoke("Stop");
                        Server.Invoke("Start");
                        break;
                    }
                }
            }
        }
    }

    public class site_info
    {
        public string name { get; set; }

        public string binding { get; set; }

        public string apppool { get; set; }

        public string path { get; set; }
    }
}
