using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Tangh.Controls
{
    public class TimeStatusStrip : StatusStrip
    {
        private ToolStripStatusLabel lblTime = new ToolStripStatusLabel();

        private Thread updateThread = null;

        public TimeStatusStrip()
            : base()
        {
            this.lblTime.Size = new System.Drawing.Size(0, 17);
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTime});
        }

        public void Start()
        {
            if (updateThread == null)
            {
                updateThread = new Thread(new ThreadStart(OnlineTest));
                updateThread.IsBackground = true;
                updateThread.Start();
            }
        }

        public void Stop()
        {
            try
            {
                if (updateThread != null)
                {
                    updateThread.Abort();
                }
            }
            catch { }

            try
            {
                updateThread = null;
            }
            catch { }
        }

        private void OnlineTest()
        {
            DateTime startRunTime = DateTime.Now;
            while (true)
            {
                try
                {
                    TimeSpan time = DateTime.Now - startRunTime;
                    SetTime(string.Format("已经运行：{0}天{1}小时{2}分钟{3}秒", time.Days, time.Hours, time.Minutes, time.Seconds));
                }
                catch { }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void SetTime(string msg)
        {
            this.Invoke(new Action<ToolStripStatusLabel>(p =>
            {
                p.Text = msg;
            }), this.lblTime);
        }
    }
}
