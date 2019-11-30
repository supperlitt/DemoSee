using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WinTestCmd
{
    public class ProcessTool
    {
        public static event Action<string> ShowMsg;

        public static string ProecssCmd(string filename, string args = "")
        {
            Process process = new Process();
            if (filename.Contains(":"))
            {
                process.StartInfo.WorkingDirectory = filename.Substring(0, filename.LastIndexOf("\\") + 1);
            }

            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("gbk");

            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();

            return null;
        }

        public static string ProcessCmd_2(string filename, string args = "")
        {
            using (Process process = new Process
            {
                StartInfo = { FileName = filename, Arguments = args, UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardInput = true }
            })
            {
                process.Start();
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine("exit");
                string str = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();

                return str;
            }
        }

        private static void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (ShowMsg != null)
            {
                ShowMsg(e.Data);
            }
        }
    }
}
