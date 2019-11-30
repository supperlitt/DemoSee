using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Tangh.Controls
{
    public class OnlineLabel : Label
    {
        private Thread onlineThread = null;

        private int second = 60;
        private string onlineText = "已连接";
        private Color onlineColor = Color.Green;
        private string outlineText = "已断开";
        private Color outlineColor = Color.Red;


        public OnlineLabel()
            : base()
        {
        }

        public void SetOnlineText(string text)
        {
            this.onlineText = text;
        }

        public void SetOutlineText(string text)
        {
            this.outlineText = text;
        }

        public void SetOnlineColor(Color c)
        {
            this.onlineColor = c;
        }

        public void SetOutlineColor(Color c)
        {
            this.outlineColor = c;
        }

        public void SetInterval(int second = 60)
        {
            this.second = second;
        }

        public void Start()
        {
            if (onlineThread == null)
            {
                onlineThread = new Thread(new ThreadStart(Execute));
                onlineThread.IsBackground = true;
                onlineThread.Start();
            }
        }

        public void Stop()
        {
            try
            {
                if (onlineThread != null)
                {
                    onlineThread.Abort();
                }
            }
            catch { }

            try
            {
                onlineThread = null;
            }
            catch { }
        }

        private void Execute()
        {
            while (true)
            {
                bool isonline = false;
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    client.Connect(new IPEndPoint(Dns.GetHostByName("www.baidu.com").AddressList[0], 80));

                    isonline = true;
                }
                catch
                {
                    isonline = false;
                }
                finally
                {
                    try
                    {
                        if (client != null)
                        {
                            client.Close();
                        }
                    }
                    catch { }

                    this.Invoke(new Action<Label>(p =>
                    {
                        if (isonline)
                        {
                            this.Text = onlineText;
                            this.ForeColor = onlineColor;
                        }
                        else
                        {
                            this.Text = outlineText;
                            this.ForeColor = outlineColor;
                        }
                    }), this);

                    Thread.Sleep(this.second * 1000);
                }
            }
        }
    }
}
