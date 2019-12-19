using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Tangh.Infrastructure
{
    public class HotKey
    {
        //如果函数执行成功，返回值不为0。            
        //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd,                //要定义热键的窗口的句柄
            int id,                     //定义热键ID（不能与其它ID重复）           
            KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
            Keys vk                     //定义热键的内容
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
    }
}

/*
 //// 使用该类，需要对Window窗体的 WndProc方法进行重写。
 /// 监视Windows消息
        /// 重载WndProc方法，用于实现热键响应
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Shift+S
                            if (btnStart.Enabled)
                            {
                                btnStart_Click(this, new EventArgs());
                            }
                            break;
                        case 101:    //按下的是Ctrl+B
                            //此处填写快捷键响应代码
                            if (btnStop.Enabled)
                            {
                                btnStop_Click(this, new EventArgs());
                            }
                            break;
                    }
                    break;
            }

            base.WndProc(ref m);
        }
 */