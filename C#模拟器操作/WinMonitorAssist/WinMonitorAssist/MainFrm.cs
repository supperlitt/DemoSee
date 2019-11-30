using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WinMonitorAssist
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.lstAccount.Columns.Add("索引", 50, HorizontalAlignment.Left);
            this.lstAccount.Columns.Add("账户", 300, HorizontalAlignment.Left);
            this.txtSourcePath.Text = DataConfig.ReadSourcePath();
            this.cmbDevice.DisplayMember = "Title";
            this.cmbDevice.ValueMember = "IPPort";
        }

        #region 文件

        private void btnMonitorToPC_Click(object sender, EventArgs e)
        {

        }

        private void btnPCToMonitor_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 设备

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.btnConnect.Enabled = false;
            Thread t = new Thread(ConnectDevice);
            t.IsBackground = true;
            t.Start();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            this.btnDisconnect.Enabled = false;
            Thread t = new Thread(DisConnectDevice);
            t.IsBackground = true;
            t.Start();
        }

        private void btnBatConnect_Click(object sender, EventArgs e)
        {

        }

        private void btnBatDisconnect_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region UI

        private void btnRefreshView_Click(object sender, EventArgs e)
        {
            this.btnRefreshView.Enabled = false;
            Thread t = new Thread(LoadView);
            t.IsBackground = true;
            t.Start();
        }

        private void chkAutoRefreshUI_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkAutoRefreshUI.Checked)
            {
                if (this.btnRefreshView.Enabled)
                {
                    this.btnRefreshView.Enabled = false;
                    Thread t = new Thread(LoadViewWhileTrue);
                    t.IsBackground = true;
                    t.Start();
                }
                else
                {
                    this.chkAutoRefreshUI.Checked = false;
                }
            }
        }

        private void btnMonitorClick_Click(object sender, EventArgs e)
        {
            if (this.cmbDevice.SelectedIndex >= 0)
            {
                AdbShell.ReadProcess(this.cmbDevice.SelectedValue.ToString(), "shell input tap " + this.txtInputX.Text + " " + this.txtInputY.Text);
            }
        }

        private void btnMonitorInput_Click(object sender, EventArgs e)
        {
            if (this.cmbDevice.SelectedIndex >= 0)
            {
                AdbShell.ReadProcess(this.cmbDevice.SelectedValue.ToString(), "shell input text \"" + this.txtInputContent.Text + "\"");
            }
        }

        private void btnMonitorSwip_Click(object sender, EventArgs e)
        {
            if (this.cmbDevice.SelectedIndex >= 0)
            {
                AdbShell.ReadProcess(this.cmbDevice.SelectedValue.ToString(), string.Format("shell input swipe {0} {1} {2} {3} {4}",
                    this.txtStartX.Text,
                    this.txtStartY.Text,
                    this.txtEndX.Text,
                    this.txtEndY.Text,
                    this.txtMiniSeconds.Text));
            }
        }
        #endregion

        #region 账户

        private void lstAccount_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListView lst = sender as ListView;
            if (e.Item.Checked)
            {
                foreach (ListViewItem item in lst.CheckedItems)
                {
                    if (item != e.Item)
                        item.Checked = false;
                }
            }
        }
        #endregion

        #region API
        #endregion

        #region 配置

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtSourcePath.Text = dialog.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataConfig.SaveSourcePath(this.txtSourcePath.Text);
        }
        #endregion

        #region 工具

        private void btnReadActivity_Click(object sender, EventArgs e)
        {
            if (this.cmbDevice.SelectedIndex >= 0)
            {
                string result = AdbShell.ReadProcess(this.cmbDevice.SelectedValue.ToString(), "shell dumpsys activity | grep \"mFocusedActivity\"");

                //   mFocusedActivity: ActivityRecord{530bfdb4 u0 com.estrongs.android.pop/.view.FileExplorerActivity t4}
                Regex activityRegex = new Regex(@"\s(?<package>[^/\s]+)/(?<activity>[^/\s]+)");
                string package = activityRegex.Match(result).Groups["package"].Value;
                string activity = activityRegex.Match(result).Groups["activity"].Value;

                this.txtActivity.Text = package + "/" + activity;
            }
        }
        #endregion

        #region 操作

        private void btnRefreshDevices_Click(object sender, EventArgs e)
        {
            this.btnRefreshDevices.Enabled = false;
            Thread t = new Thread(LoadDevices);
            t.IsBackground = true;
            t.Start();
        }

        private void btnRefreshApps_Click(object sender, EventArgs e)
        {
            this.btnRefreshApps.Enabled = false;
            Thread t = new Thread(LoadApps);
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region 视图

        private void btnMonitorTool_Click(object sender, EventArgs e)
        {
            if (this.cmbDevice.SelectedIndex < 0)
            {
                return;
            }

            string device = this.cmbDevice.SelectedValue.ToString();
            Button btn = sender as Button;
            if (btn.Text == "Home")
            {
                AdbShell.ReadProcess(device, "shell input keyevent 3");
            }
            else if (btn.Text == "Back")
            {
                AdbShell.ReadProcess(device, "shell input keyevent 4");
            }
            else if (btn.Text == "Menu")
            {
                AdbShell.ReadProcess(device, "shell input keyevent 82");
            }
            else if (btn.Text == "V+")
            {
                AdbShell.ReadProcess(device, "shell input keyevent 24");
            }
            else if (btn.Text == "V-")
            {
                AdbShell.ReadProcess(device, "shell input keyevent 25");
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.cmbDevice.SelectedIndex >= 0)
            {
                Regex sizeRegex = new Regex(@"(?<width>\d+)x(?<height>\d+)");
                string content = AdbShell.ReadProcess(this.cmbDevice.SelectedValue.ToString(), "shell wm size");
                int width = int.Parse(sizeRegex.Match(content).Groups["width"].Value);
                int height = int.Parse(sizeRegex.Match(content).Groups["height"].Value);

                // adb shell wm size
                this.txtClickPoint.Text = (e.Location.X * width / this.pictureBox1.Width) + "," + (e.Location.Y * height / this.pictureBox1.Height);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

        }
        #endregion

        #region 线程方法
        private void LoadDevices()
        {
            try
            {
                Regex ipport_regex = new Regex(@"(?<ipport>[^\t]+)");
                Regex regex = new Regex(@"(?<num>\d+)$");
                List<Monitor> monitorList = new List<Monitor>();
                Process[] ps = Process.GetProcesses();
                foreach (var p in ps)
                {
                    try
                    {
                        if (p.MainModule.ModuleName == "MEmu.exe")
                        {
                            // 逍遥模拟器
                            string name = p.MainWindowTitle;
                            // "D:\Program Files\Microvirt\MEmu\MEmu.exe" MEmu_3
                            string cmdline = Tool.ReadCommandLine(p.Id);
                            int port = 21503;
                            if (cmdline.EndsWith("MEmu"))
                            {
                                port = 21503;
                            }
                            else
                            {
                                string numStr = regex.Match(cmdline).Groups["num"].Value;
                                int num = 0;
                                int.TryParse(numStr, out num);

                                port = (21503 + num * 10);
                            }

                            monitorList.Add(new Monitor()
                            {
                                IPPort = "127.0.0.1:" + port,
                                Title = name
                            });
                        }
                        else if (p.MainModule.ModuleName == "Nox.exe")
                        {
                            // 夜神模拟器
                            string name = p.MainWindowTitle;
                            // "D:\Program Files\Microvirt\MEmu\MEmu.exe" MEmu_3
                            string cmdline = Tool.ReadCommandLine(p.Id);
                            int port = 62001;
                            if (cmdline.EndsWith("administrator"))
                            {
                                port = 62001;
                            }
                            else
                            {
                                string numStr = regex.Match(cmdline).Groups["num"].Value;
                                int num = 0;
                                int.TryParse(numStr, out num);
                                if (num == 0)
                                {
                                    port = 62001;
                                }
                                else
                                {
                                    port = (62024 + num);
                                }
                            }

                            monitorList.Add(new Monitor()
                            {
                                IPPort = "127.0.0.1:" + port,
                                Title = name
                            });
                        }
                    }
                    catch { }
                }

                if (monitorList.Count > 0)
                {
                    foreach (var item in monitorList)
                    {
                        AdbShell.Connect(item.IPPort);
                    }

                    string content = AdbShell.Devices();
                    string[] array = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in array)
                    {
                        if (item.EndsWith("\tdevice"))
                        {
                            string ipport = ipport_regex.Match(item.Trim()).Groups["ipport"].Value;
                            var monitor = monitorList.Find(p => p.IPPort == ipport);
                            if (monitor == null)
                            {
                                monitorList.Add(new Monitor() { Title = ipport, IPPort = ipport });
                            }
                        }
                    }

                    this.cmbDevice.DataSource = monitorList;
                }
            }
            catch
            {
            }
            finally
            {
                this.btnRefreshDevices.Enabled = true;
            }
        }

        private void LoadApps()
        {
            try
            {
                if (this.cmbDevice.SelectedIndex < 0)
                {
                    return;
                }

                var list = AdbShell.ReadPackageList(this.cmbDevice.SelectedValue.ToString());
                this.cmbApp.DataSource = list;
            }
            catch
            {
            }
            finally
            {
                this.btnRefreshApps.Enabled = true;
            }
        }

        private void ConnectDevice()
        {
            try
            {
                AdbShell.Connect(this.txtDevice.Text);
            }
            catch { }
            finally
            {
                this.btnConnect.Enabled = true;
            }
        }

        private void DisConnectDevice()
        {
            try
            {
                AdbShell.DisConnect(this.txtDevice.Text);
            }
            catch { }
            finally
            {
                this.btnDisconnect.Enabled = true;
            }
        }

        private void LoadView()
        {
            try
            {
                if (this.cmbDevice.SelectedIndex < 0)
                {
                    return;
                }

                string monitor_path = @"/sdcard/screenshot.png";
                string pc_path = @"D:\screenshot.png";
                string device = this.cmbDevice.SelectedValue.ToString();
                if (!File.Exists(pc_path))
                {
                    File.Create(pc_path);
                }

                string content = AdbShell.ReadProcess(device, "shell /system/bin/screencap -p " + monitor_path);
                content = AdbShell.ReadProcess(device, "pull " + monitor_path + " " + pc_path);
                Thread.Sleep(1000);
                if (!File.Exists(pc_path))
                {
                    Thread.Sleep(1000);
                }

                var bs = File.ReadAllBytes(pc_path);
                using (MemoryStream ms = new MemoryStream(bs))
                {
                    Image img = Image.FromStream(ms);
                    this.Invoke(new Action<PictureBox>(p => p.Image = img), this.pictureBox1);
                }
            }
            catch { }
            finally
            {
                this.btnRefreshView.Enabled = true;
            }
        }

        private void LoadViewWhileTrue()
        {
            if (this.cmbDevice.SelectedIndex < 0)
            {
                return;
            }

            string device = this.cmbDevice.SelectedValue.ToString();
            string monitor_path = @"/sdcard/screenshot.png";
            string pc_path = @"D:\screenshot.png";
            while (this.chkAutoRefreshUI.Checked)
            {
                try
                {
                    if (!File.Exists(pc_path))
                    {
                        File.Create(pc_path);
                    }

                    string content = AdbShell.ReadProcess(device, "shell /system/bin/screencap -p " + monitor_path);
                    content = AdbShell.ReadProcess(device, "pull " + monitor_path + " " + pc_path);
                    Thread.Sleep(1000);
                    if (!File.Exists(pc_path))
                    {
                        Thread.Sleep(1000);
                    }

                    var bs = File.ReadAllBytes(pc_path);
                    using (MemoryStream ms = new MemoryStream(bs))
                    {
                        Image img = Image.FromStream(ms);
                        this.Invoke(new Action<PictureBox>(p => p.Image = img), this.pictureBox1);
                    }
                }
                catch { }
                finally
                {
                    Thread.Sleep(int.Parse(this.txtRefreshMimiSeconds.Text));
                }
            }

            this.btnRefreshView.Enabled = true;
        }

        #endregion
    }
}
