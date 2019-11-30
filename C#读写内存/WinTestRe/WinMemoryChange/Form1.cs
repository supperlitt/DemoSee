using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WinMemoryChange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //0xFD24F3
        //0xFD1000
        private string processName = "MFCTest"; //

        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr startAddress = IntPtr.Zero;
            int pid = ApiHelper.GetPidByProcessName(processName, ref startAddress);
            if (pid == 0)
            {
                MessageBox.Show("哥们启用之前该运行吧！");
                return;
            }

            this.txtPId.Text = pid.ToString();
            this.txtBaseAddr.Text = startAddress.ToInt32().ToString("X2");
            int baseAddress = startAddress.ToInt32() + 0x1000;
            this.txtCodeAddr.Text = (startAddress.ToInt32() + 0x1000).ToString("X2");
            int value = ReadMemoryValue(baseAddress);             // 读取基址(该地址不会改变)
            int address = baseAddress + 0x14F3;                   // 获取2级地址，，忘记哪里来的了。。
            this.txtKeyAddr.Text = address.ToString("X2");
            value = ReadMemoryValue(address);
            bool result = ApiHelper.WriteMemoryValue(address, processName, new int[] { 144 + 144 * 256 }, 2);

            MessageBox.Show(result ? "成功" : "失败");
        }

        //读取制定内存中的值
        public int ReadMemoryValue(int baseAdd)
        {
            return ApiHelper.ReadMemoryValue(baseAdd, processName);
        }

        //将值写入指定内存中
        public bool WriteMemory(int baseAdd, int[] value)
        {
            return ApiHelper.WriteMemoryValue(baseAdd, processName, value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IntPtr ptr = IntPtr.Zero;
            int pid = ApiHelper.GetPidByProcessName(processName, ref ptr);

            this.txtBaseAddr.Text = pid.ToString();
            this.txtCodeAddr.Text = (ptr.ToInt32() + 0x1000).ToString();
        }
    }
}
