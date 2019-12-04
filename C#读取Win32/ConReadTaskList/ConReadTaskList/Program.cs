using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConReadTaskList
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exist = false;
            Process[] ps = Process.GetProcesses();
            foreach (var p in ps)
            {
                try
                {
                    if (p.MainWindowTitle == "Windows 任务管理器")
                    {
                        Console.WriteLine("找到了进程");
                        exist = true;
                        IntPtr main = p.MainWindowHandle;

                        Console.WriteLine("main " + main);
                        IntPtr tab = Win32Helper.FindWindowEx(main, IntPtr.Zero, null, "Processes"); // Processes Applications

                        Console.WriteLine("tab " + tab);
                        IntPtr tabItem = Win32Helper.FindWindowEx(tab, IntPtr.Zero, "SysListView32", null);

                        Console.WriteLine("tabItem " + tabItem);
                        var dt = Win32Helper.GetListView(tabItem);
                        foreach (DataRow item in dt.Rows)
                        {
                            List<string> rowList = new List<string>();
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                rowList.Add(item[i].ToString());
                            }

                            Console.WriteLine(string.Join("\t", rowList.ToArray()));
                        }
                    }
                }
                catch (Exception e)
                {

                }

            }

            if (!exist)
            {
                Console.WriteLine("找不到进程");
            }

            Console.ReadLine();


        }
    }
}
