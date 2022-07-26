using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Winb2b10086cn
{
    public class IISManager
    {
        public Action<string> NotifyMsg = null;

        public string GetIISVersion()
        {
            DirectoryEntry getEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");

            return getEntity.Properties["MajorIISVersionNumber"].Value.ToString();
        }

        /// <summary>
        /// 判断程序池是否存在
        /// </summary>
        /// <param name="AppPoolName">程序池名称</param>
        /// <returns>true存在 false不存在</returns>
        private bool IsAppPoolName(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 删除指定程序池
        /// </summary>
        /// <param name="AppPoolName">程序池名称</param>
        /// <returns>true删除成功 false删除失败</returns>
        private bool DeleteAppPool(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    try
                    {
                        getdir.DeleteTree();
                        result = true;
                    }
                    catch
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public void Test()
        {
            string AppPoolName = "LamAppPool";
            if (!IsAppPoolName(AppPoolName))
            {
                DirectoryEntry newpool;
                DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
                newpool.CommitChanges();
            }

            // 修改应用程序的配置(包含托管模式及其NET运行版本)
            ServerManager sm = new ServerManager();
            sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion = "v4.0";
            sm.ApplicationPools[AppPoolName].ManagedPipelineMode = ManagedPipelineMode.Classic; //托管模式Integrated为集成 Classic为经典
            sm.CommitChanges();
            this.ShowMsg(AppPoolName + "程序池托管管道模式：" + sm.ApplicationPools[AppPoolName].ManagedPipelineMode.ToString() + "运行的NET版本为:" + sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion);

        }

        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="FileAdd">文件夹路径</param>
        public void SetFileRole(string dir, string role = "Everyone")
        {
            dir = dir.Trim('\\');
            DirectorySecurity fSec = new DirectorySecurity();

            fSec.AddAccessRule(new FileSystemAccessRule(role, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            System.IO.Directory.SetAccessControl(dir, fSec);
        }

        /// <summary>
            /// 创建网站
            /// </summary>
            /// <param name="siteInfo"></param>
        public void CreateNewWebSite(string newSiteNum, NewWebSiteInfo siteInfo)
        {
            DirectoryEntry rootEntry = GetDirectoryEntry(entPath);
            DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteNum, "IIsWebServer");
            newSiteEntry.CommitChanges();
            newSiteEntry.Properties["ServerBindings"].Value = siteInfo.BindString;
            newSiteEntry.Properties["ServerComment"].Value = siteInfo.CommentOfWebSite;
            newSiteEntry.CommitChanges();
            DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
            vdEntry.CommitChanges();
            string ChangWebPath = siteInfo.WebPath.Trim().Remove(siteInfo.WebPath.Trim().LastIndexOf('\\'), 1);
            vdEntry.Properties["Path"].Value = ChangWebPath;

            vdEntry.Invoke("AppCreate", true);//创建应用程序
            vdEntry.Properties["AccessRead"][0] = true; //设置读取权限
            vdEntry.Properties["AccessWrite"][0] = true;
            vdEntry.Properties["AccessScript"][0] = true;//执行权限
            vdEntry.Properties["AccessExecute"][0] = false;
            vdEntry.Properties["DefaultDoc"][0] = "Login.aspx";//设置默认文档
            vdEntry.Properties["AppFriendlyName"][0] = newSiteNum;// "LabManager"; //应用程序名称           
            vdEntry.Properties["AuthFlags"][0] = 1;// 0表示不允许匿名访问,1表示就可以3为基本身份验证，7为windows继承身份验证
            vdEntry.CommitChanges();
            //操作增加MIME
            //IISOle.MimeMapClass NewMime = new IISOle.MimeMapClass();
            //NewMime.Extension = ".xaml"; NewMime.MimeType = "application/xaml+xml";
            //IISOle.MimeMapClass TwoMime = new IISOle.MimeMapClass();
            //TwoMime.Extension = ".xap"; TwoMime.MimeType = "application/x-silverlight-app";
            //rootEntry.Properties["MimeMap"].Add(NewMime);
            //rootEntry.Properties["MimeMap"].Add(TwoMime);
            //rootEntry.CommitChanges();
            #region 针对IIS7
            DirectoryEntry getEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
            int Version = int.Parse(getEntity.Properties["MajorIISVersionNumber"].Value.ToString());
            this.ShowMsg("得到IIS版本 " + Version);
            if (Version > 6)
            {
                #region 创建应用程序池
                string AppPoolName = newSiteNum;// "LabManager";
                if (!IsAppPoolName(AppPoolName))
                {
                    DirectoryEntry newpool;
                    DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                    newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
                    newpool.CommitChanges();
                    this.ShowMsg("创建应用程序池");
                }
                #endregion

                #region 修改应用程序的配置(包含托管模式及其NET运行版本)
                ServerManager sm = new ServerManager();
                sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion = "v4.0";
                sm.ApplicationPools[AppPoolName].ManagedPipelineMode = ManagedPipelineMode.Classic; //托管模式Integrated为集成 Classic为经典
                sm.CommitChanges();
                this.ShowMsg("修改站点配置 v4.0 和经典模式");
                #endregion

                vdEntry.Properties["AppPoolId"].Value = AppPoolName;
                vdEntry.CommitChanges();
            }
            #endregion

            //启动aspnet_regiis.exe程序 
            //string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
            //ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            ////处理目录路径 
            //string path = vdEntry.Path.ToUpper();
            //int index = path.IndexOf("W3SVC");
            //path = path.Remove(0, index);
            ////启动ASPnet_iis.exe程序,刷新脚本映射 
            //startInfo.Arguments = "-s " + path;
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //startInfo.UseShellExecute = false;
            //startInfo.CreateNoWindow = true;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.RedirectStandardError = true;
            //Process process = new Process();
            //process.StartInfo = startInfo;
            //process.Start();
            //process.WaitForExit();
            //string errors = process.StandardError.ReadToEnd();
            //if (errors != string.Empty)
            //{
            //    throw new Exception(errors);
            //}
        }

        string entPath = String.Format("IIS://{0}/w3svc", "localhost");
        public DirectoryEntry GetDirectoryEntry(string entPath)
        {
            DirectoryEntry ent = new DirectoryEntry(entPath);
            return ent;
        }
        public class NewWebSiteInfo
        {
            private string hostIP;   // 主机IP
            private string portNum;   // 网站端口号
            private string descOfWebSite; // 网站表示。一般为网站的网站名。例如"www.dns.com.cn"
            private string commentOfWebSite;// 网站注释。一般也为网站的网站名。
            private string webPath;   // 网站的主目录。例如"e:\ mp"
            public NewWebSiteInfo(string hostIP, string portNum, string descOfWebSite, string commentOfWebSite, string webPath)
            {
                this.hostIP = hostIP;
                this.portNum = portNum;
                this.descOfWebSite = descOfWebSite;
                this.commentOfWebSite = commentOfWebSite;
                this.webPath = webPath;
            }
            public string BindString
            {
                get
                {
                    return String.Format("{0}:{1}:{2}", hostIP, portNum, descOfWebSite); //网站标识（IP,端口，主机头值）
                }
            }
            public string PortNum
            {
                get
                {
                    return portNum;
                }
            }
            public string CommentOfWebSite
            {
                get
                {
                    return commentOfWebSite;
                }
            }
            public string WebPath
            {
                get
                {
                    return webPath;
                }
            }
        }

        private void ShowMsg(string msg)
        {
            if (NotifyMsg != null)
            {
                NotifyMsg(msg);
            }
        }
    }
}
