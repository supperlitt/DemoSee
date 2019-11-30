using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Tangh.Controls
{
    public class SocketHttpGetHelper
    {
        private IPAddress ip = IPAddress.Any;
        private int port = 8123;
        private int count = 0;
        private Socket server = null;

        public string DefaultReturn = string.Empty;

        public event Func<string, string, string> GetHandler = null;

        public SocketHttpGetHelper()
        {
        }

        public SocketHttpGetHelper(IPAddress ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public void StartListen(int count = 10)
        {
            this.count = count;
            Thread t = new Thread(new ThreadStart(ProcessThread));
            t.IsBackground = true;
            t.Start();
        }

        public void CloseSocket()
        {
            try
            {
                if (server != null)
                {
                    server.Close();
                }
            }
            catch { }
        }

        private void ProcessThread()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new System.Net.IPEndPoint(ip, port));
            server.Listen(count);
            while (true)
            {
                try
                {
                    Socket client = server.Accept();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ListenExecute), client);
                }
                catch { }
                finally
                {
                }
            }
        }

        private void ListenExecute(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                string ip = (client.RemoteEndPoint as System.Net.IPEndPoint).Address.ToString();
                List<byte> allbuffer = new List<byte>();
                byte[] buffer = new byte[8192];
                client.ReceiveBufferSize = 8192;
                int count = client.Receive(buffer);
                if (count > 0)
                {
                    allbuffer.AddRange(buffer.Take(count));
                    string content = Encoding.UTF8.GetString(buffer, 0, count);
                    content = Encoding.UTF8.GetString(allbuffer.ToArray());

                    // 解析 content
                    Regex actionRegex = new Regex(@"GET\s+/(?<action>\w+)\??(?<args>[^\s]{0,})");
                    string action = actionRegex.Match(content).Groups["action"].Value;
                    string args = actionRegex.Match(content).Groups["args"].Value;
                    string headStr = @"HTTP/1.1 200 OK
Content-Type: text/html
Connection: close
Content-Encoding: utf-8
Content-Length: {0}

{1}";

                    if (GetHandler != null && !string.IsNullOrEmpty(action))
                    {
                        try
                        {
                            string result = GetHandler(action, args);
                            int length = Encoding.UTF8.GetBytes(result).Length;
                            string data = string.Format(headStr, length, result);
                            client.Send(Encoding.UTF8.GetBytes(data), SocketFlags.None);
                        }
                        catch { }
                        finally
                        {
                        }
                    }
                    else
                    {
                        string data = string.Format(headStr, Encoding.UTF8.GetBytes(DefaultReturn).Length, DefaultReturn);
                        client.Send(Encoding.UTF8.GetBytes(data));
                    }
                }
            }
            catch { }
            finally
            {
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
                catch { }
            }
        }
    }
}
