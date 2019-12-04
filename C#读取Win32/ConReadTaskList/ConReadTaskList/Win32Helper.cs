using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
using System.Threading;
using System.Drawing;

namespace ConReadTaskList
{
    public class Win32Helper
    {
        #region 热键操作
        //如果函数执行成功，返回值不为0。            
        //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <param name="fsModifiers"></param>
        /// <param name="vk">(byte)Keys.D</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd,                //要定义热键的窗口的句柄
            int id,                     //定义热键ID（不能与其它ID重复）           
            KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
            byte vk                     //定义热键的内容
            );

        /*
         使用方式：
         * RegisterHotKey(this.Handle,100,KeyModifiers.None,Keys.F10);
         */

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,                //要取消热键的窗口的句柄
            int id                      //要取消热键的ID
            );


        #endregion

        #region 查找窗体

        /// <summary>
        /// 遍历所有窗口对于的委托
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="lParam">上层传入的值</param>
        /// <returns></returns>
        public delegate bool EnumWindowsProc(int hWnd, int lParam);

        /// <summary>
        /// 遍历进程中所有窗口
        /// </summary>
        /// <param name="ewp"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsProc ewp, int lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(int hWnd, StringBuilder title, int size);

        /// <summary>
        /// 获取当前窗口线程对于的进程ID
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        /// <summary>
        /// 查找窗口
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 查找子窗口
        /// </summary>
        /// <param name="hwndParent"></param>
        /// <param name="hwndChildAfter"></param>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        /// <summary>
        /// 返回与指定窗口有特定关系的窗口句柄
        /// 在循环体中调用函数EnumChildWindow比调用GetWindow函数可靠。调用GetWindow函数实现该任务的应用程序可能会陷入死循环或退回一个已被销毁的窗口句柄。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="uCmd">参数类型：</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        #endregion

        #region 发送消息
        [DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hwnd, uint Msg, long wParam, long lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessageInt(IntPtr hWnd, int Msg, int wParm, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessageStrs(IntPtr hwnd, int wMsg, int wParam, StringBuilder lParam);
        #endregion

        #region 窗体位置

        /// <summary>
        /// 设置鼠标位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern int SetCursorPos(int x, int y);

        /// <summary>
        /// 获取窗口大小及位置
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        /// <summary>
        /// 3-最大化窗口，2-最小化窗口，1-正常大小窗口；
        /// </summary>
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        private static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// 设置目标窗体大小，位置
        /// </summary>
        /// <param name="hWnd">目标句柄</param>
        /// <param name="x">目标窗体新位置X轴坐标</param>
        /// <param name="y">目标窗体新位置Y轴坐标</param>
        /// <param name="nWidth">目标窗体新宽度</param>
        /// <param name="nHeight">目标窗体新高度</param>
        /// <param name="BRePaint">是否刷新窗体</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool BRePaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        /// <summary>
        /// 得到当前活动的窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern System.IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public extern static int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        #endregion

        #region 键鼠操作
        /// <summary>
        /// 鼠标移动控制
        /// </summary>
        /// <param name="dwFlags">控制鼠标类型</param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <param name="dwData">0</param>
        /// <param name="dwExtraInfo">0</param>
        [DllImport("user32")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        /// <summary>
        /// 键盘事件，按下
        /// </summary>
        /// <param name="bVk">(byte)Keys.D</param>
        /// <param name="bScan">0</param>
        /// <param name="dwFlags">这里是整数类型  0 为按下，2为释放</param>
        /// <param name="dwExtraInfo">这里是整数类型 一般情况下设成为 0</param>
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int MK_LBUTTON = 0x1;
        public const int WM_SYSKEYUP = 0X105;
        public const int WM_SYSKEYDOWN = 0X104;

        // 缓冲区变量
        private const uint PURGE_TXABORT = 0x0001; // Kill the pending/current writes to the comm port. 
        private const uint PURGE_RXABORT = 0x0002; // Kill the pending/current reads to the comm port. 
        private const uint PURGE_TXCLEAR = 0x0004; // Kill the transmit queue if there. 
        private const uint PURGE_RXCLEAR = 0x0008; // Kill the typeahead buffer if there. 

        #endregion

        #region Kernel32
        [DllImport("Kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);
        [DllImport("Kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);
        [DllImport("Kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);
        [DllImport("Kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [DllImport("Kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);
        [DllImport("Kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        #endregion

        #region 时间操作

        //设定，获取系统时间,SetSystemTime()默认设置的为UTC时间，比北京时间少了8个小时。  
        [DllImport("Kernel32.dll")]
        public static extern bool SetSystemTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern bool SetLocalTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern void GetSystemTime(ref SYSTEMTIME time);
        [DllImport("Kernel32.dll")]
        public static extern void GetLocalTime(ref SYSTEMTIME time);

        #endregion

        #region 具体操作

        /// <summary>
        /// 读取 SysListView32 中的数据到DataTable，列名从0-~
        /// </summary>
        /// <param name="hwndListView"></param>
        /// <returns></returns>
        public static DataTable GetListView(IntPtr hwndListView)
        {
            DataTable result = new DataTable();
            IntPtr headerPtr = (IntPtr)SendMessageInt(hwndListView, 0x101f, 0, 0);
            int columnCount = SendMessageInt(headerPtr, 0x1200, 0, 0);
            int rowCount = SendMessageInt(hwndListView, 0x1004, 0, 0);
            int pId;
            GetWindowThreadProcessId(hwndListView, out pId);
            IntPtr hProcess = OpenProcess((uint)0x38, false, (uint)pId);
            IntPtr lpBaseAddress = VirtualAllocEx(hProcess, IntPtr.Zero, 0x100, 0x3000, 4);
            try
            {
                for (int k = 0; k < columnCount; k++)
                {
                    result.Columns.Add(k.ToString());
                }
            }
            catch { }

            try
            {
                for (int i = 0; i < rowCount; i++)
                {
                    var dr = result.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        byte[] arr = new byte[0x100];
                        LVITEM[] lvitemArray = new LVITEM[1];
                        lvitemArray[0].mask = 1;
                        lvitemArray[0].iItem = i;
                        lvitemArray[0].iSubItem = j;
                        lvitemArray[0].cchTextMax = arr.Length;
                        lvitemArray[0].pszText = (IntPtr)(((int)lpBaseAddress) + Marshal.SizeOf(typeof(LVITEM)));
                        uint vNumberOfBytesRead = 0;
                        WriteProcessMemory(hProcess, lpBaseAddress, Marshal.UnsafeAddrOfPinnedArrayElement(lvitemArray, 0), Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                        SendMessageInt(hwndListView, 0x1005, i, lpBaseAddress.ToInt32());
                        ReadProcessMemory(hProcess, (IntPtr)(((int)lpBaseAddress) + Marshal.SizeOf(typeof(LVITEM))), Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0), arr.Length, ref vNumberOfBytesRead);
                        int count = 0;
                        for (int k = 0; k < arr.Length; k++)
                        {
                            if (arr[k].ToString() == "0")
                            {
                                count = k;
                                break;
                            }
                        }

                        string str2 = "";
                        if (count > 0)
                        {
                            str2 = Encoding.Default.GetString(arr, 0, count).Trim();
                        }

                        dr[j] = str2;
                    }

                    result.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                VirtualFreeEx(hProcess, lpBaseAddress, 0, 0x8000);
                CloseHandle(hProcess);
            }

            return result;
        }

        /// <summary>
        /// 单击左键
        /// </summary>
        /// <param name="rX"></param>
        /// <param name="rY"></param>
        public static void MouseClick(int rX, int rY)
        {
            SetCursorPos(rX, rY);
            // 使用组合构造一次单击
            mouse_event(Win32Message.MOUSEEVENTF_LEFTDOWN | Win32Message.MOUSEEVENTF_LEFTUP, rX, rY, 0, 0);
        }

        /// <summary>
        /// 单击左键
        /// </summary>
        /// <param name="rX"></param>
        /// <param name="rY"></param>
        public static void MouseClick(Point p)
        {
            SetCursorPos(p.X, p.Y);
            // 使用组合构造一次单击
            mouse_event(Win32Message.MOUSEEVENTF_LEFTDOWN | Win32Message.MOUSEEVENTF_LEFTUP, p.X, p.Y, 0, 0);
        }

        public static void KeyDown(byte key)
        {
            keybd_event((byte)key, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event((byte)key, 0, 2, 0);
        }

        /// <summary>
        /// 设置窗口在最前面
        /// </summary>
        /// <param name="handle"></param>
        public static void SetWindowFirst(IntPtr handle)
        {
            ShowWindow(handle, 1);
            Thread.Sleep(300);
            SetWindowPos(handle, -1, 0, 0, 0, 0, 1 | 2);
        }

        public static void SetSystemTime(DateTime dt)
        {
            SYSTEMTIME time = new SYSTEMTIME();
            time.FromDateTime(dt);
            SetSystemTime(ref time);
        }
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;

        public void FromDateTime(DateTime dateTime)
        {
            wYear = (ushort)dateTime.Year;
            wMonth = (ushort)dateTime.Month;
            wDayOfWeek = (ushort)dateTime.DayOfWeek;
            wDay = (ushort)dateTime.Day;
            wHour = (ushort)dateTime.Hour;
            wMinute = (ushort)dateTime.Minute;
            wSecond = (ushort)dateTime.Second;
            wMilliseconds = (ushort)dateTime.Millisecond;
        }

        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond);
        }
    }

    public class Win32Message
    {
        /// <summary>
        /// ListBox
        ///  选择一个字符串，并将其所在的条目滚动到视野内。当新的字符串被选定，
        ///  列表框的高亮显示将从原有的选中字符串移动到这个新的字符串上
        /// </summary>
        public static readonly int LB_SETCURSEL = 0x0186;

        /// <summary>
        /// Combobox
        ///  选择一个字符串，并将其所在的条目滚动到视野内。当新的字符串被选定，
        ///  列表框的高亮显示将从原有的选中字符串移动到这个新的字符串上
        /// </summary>
        public static readonly int CB_SETCURSEL = 0x014E;

        /// <summary>
        /// double click
        /// </summary>
        public static readonly int WM_NCLBUTTONDBLCLK = 0x00A3;

        // 控制鼠标类型
        public static readonly int MOUSEEVENTF_MOVE = 0x0001;//模拟鼠标移动
        public static readonly int MOUSEEVENTF_LEFTDOWN = 0x0002;//模拟鼠标左键按下
        public static readonly int MOUSEEVENTF_LEFTUP = 0x0004;//模拟鼠标左键抬起
        public static readonly int MOUSEEVENTF_ABSOLUTE = 0x8000;//鼠标绝对位置
        public static readonly int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        public static readonly int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        public static readonly int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下 
        public static readonly int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 
    }

    //定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
    [Flags()]
    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Ctrl = 2,
        Shift = 4,
        WindowsKey = 8
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left; //最左坐标
        public int Top; //最上坐标
        public int Right; //最右坐标
        public int Bottom; //最下坐标
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LVITEM
    {
        public int mask;
        public int iItem;
        public int iSubItem;
        public int state;
        public int stateMask;
        public IntPtr pszText;
        public int cchTextMax;
        public int iImage;
        public IntPtr lParam;
        public int iIndent;
        public int iGroupId;
        public int cColumns;
        public IntPtr puColumns;
    }
}