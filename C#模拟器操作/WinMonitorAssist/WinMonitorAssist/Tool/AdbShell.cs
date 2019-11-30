using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WinMonitorAssist
{
    public class AdbShell
    {
        public static string ReadProcess(string clientname, string args)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb.exe");

                string fileCmd = string.IsNullOrEmpty(clientname) ? "" : (" -s " + clientname + " ");
                process.StartInfo.Arguments = (args == null ? string.Empty : (fileCmd + args));
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                // process.WaitForExit();
                var result = process.StandardOutput.ReadToEnd();

                return result;
            }
        }

        public static string Connect(string clientname)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb.exe");

                string fileCmd = " connect " + clientname;
                process.StartInfo.Arguments = fileCmd;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                // process.WaitForExit();
                var result = process.StandardOutput.ReadToEnd();

                return result;
            }
        }

        public static string DisConnect(string clientname)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb.exe");

                string fileCmd = " disconnect " + clientname;
                process.StartInfo.Arguments = fileCmd;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                // process.WaitForExit();
                var result = process.StandardOutput.ReadToEnd();

                return result;
            }
        }

        public static string Devices()
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "adb.exe");

                string fileCmd = " devices";
                process.StartInfo.Arguments = fileCmd;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                // process.WaitForExit();
                var result = process.StandardOutput.ReadToEnd();

                return result;
            }
        }

        public static List<string> ReadPackageList(string clientname)
        {
            // adb shell pm list package
            string content = ReadProcess(clientname, "shell pm list package");
            string[] array = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();
            foreach (var str in array)
            {
                if (str.StartsWith("package:"))
                {
                    result.Add(str.Substring("package:".Length));
                }
            }

            return result;
        }
    }
}
