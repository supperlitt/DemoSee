using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace WinMonitorAssist
{
    public class Tool
    {
        public static string ReadCommandLine(int pid)
        {
            using (ManagementObjectSearcher mos = new ManagementObjectSearcher(
        "SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + pid))
            {
                foreach (ManagementObject mo in mos.Get())
                {
                    return mo["CommandLine"].ToString();
                }
            }

            return string.Empty;
        }
    }
}
