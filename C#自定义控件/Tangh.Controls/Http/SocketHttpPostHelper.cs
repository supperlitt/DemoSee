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
    public class SocketHttpPostHelper
    {
        private IPAddress ip = IPAddress.Any;
        private int port = 8123;
        private int count = 0;
        private Socket server = null;

        public string DefaultReturn = string.Empty;

        public event Func<string, string, string> PostHandler = null;

        public SocketHttpPostHelper()
        {
        }

        public SocketHttpPostHelper(IPAddress ip, int port)
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

                    // 判断是否需要接受POSTData的数据
                    Regex sizeRegex = new Regex(@"Content-Length:\s+(?<size>\d+)");
                    int size = int.Parse(sizeRegex.Match(content).Groups["size"].Value);
                    string[] array = content.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
                    int nextAllcount = 0;
                    while ((array[1].Length + nextAllcount) < size)
                    {
                        int nextcount = client.Receive(buffer);
                        allbuffer.AddRange(buffer.Take(nextcount));

                        nextAllcount += nextcount;
                    }

                    content = Encoding.UTF8.GetString(allbuffer.ToArray());

                    string headStr = @"HTTP/1.1 200 OK
Content-Type: text/html
Connection: close
Content-Encoding: utf-8
Content-Length: {0}

{1}";

                    Regex postRegex = new Regex(@"POST\s+/(?<action>\w+)[\s\S]+?\r?\n\r?\n(?<postdata>.{0,})");

                    // 解析POST数据
                    string action = postRegex.Match(content).Groups["action"].Value;
                    string postdata = postRegex.Match(content).Groups["postdata"].Value;
                    if (PostHandler != null)
                    {
                        try
                        {
                            string result = PostHandler(action, postdata);
                            string data = string.Format(headStr, Encoding.UTF8.GetBytes(result).Length, result);
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
                    client.Dispose();
                }
                catch { }
            }
        }
    }
}
