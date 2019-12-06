using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;
using WebSocketSharp.Net;
using System.Threading;

namespace Win2To1
{
    public partial class MainFrm : Form
    {
        private WebSocketServer server = null;
        private WebSocketSharp.WebSocket client = null;

        public MainFrm()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {

        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            server = new WebSocketServer("ws://127.0.0.1:9800");
            server.AddWebSocketService<Laputa>("/Laputa");
            server.Start();

            this.ShowMsg(this.txtServer, "启动服务：ws://127.0.0.1:9800/Laputa");
        }

        private void ShowMsg(TextBox txt, string msg)
        {
            this.Invoke(new Action<TextBox>(p =>
            {
                if (p.TextLength > 20000)
                {
                    p.Clear();
                }

                p.AppendText(msg + "\r\n");
            }), txt);
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            // ws://127.0.0.1:9800/Laputa
            client = new WebSocketSharp.WebSocket("ws://127.0.0.1:9800/Laputa");
            client.OnClose += socket_OnClose;
            client.OnOpen += socket_OnOpen;
            client.OnMessage += socket_OnMessage;
            client.OnError += socket_OnError;
            client.Connect();

            this.ShowMsg(this.txtClient, "连接到服务端：ws://127.0.0.1:9800/Laputa");

            Thread t = new Thread(SendExecute);
            t.IsBackground = true;
            t.Start();
        }

        private void SendExecute()
        {
            int index = 0;
            while (true)
            {
                try
                {
                    client.Send(Encoding.UTF8.GetBytes("test" + index));
                    index++;
                }
                catch { }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        void socket_OnError(object sender, WebSocketSharp.ErrorEventArgs e)
        {
            this.ShowMsg(this.txtClient, "OnError:" + e.Message);
        }

        void socket_OnMessage(object sender, WebSocketSharp.MessageEventArgs e)
        {
            this.ShowMsg(this.txtClient, "OnMessage:" + e.Data);
        }

        void socket_OnOpen(object sender, EventArgs e)
        {
            this.ShowMsg(this.txtClient, "OnOpen");
        }

        void socket_OnClose(object sender, WebSocketSharp.CloseEventArgs e)
        {
            this.ShowMsg(this.txtClient, "OnClose:" + e.Reason);
        }
    }

    public class Laputa : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            // 得到cookie判断一下

            base.OnOpen();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = e.Data == "BALUS"
                      ? "I've been balused already..."
                      : "I'm not available now.";

            Send(msg);
        }
    }
}
