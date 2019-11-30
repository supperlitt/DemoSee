using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;

namespace WinTestSocketRaw
{
    public class HttpWatch
    {
        /// <summary>
        /// 通知事件
        /// </summary>
        public event Action<string> DoMsg;
        int rid;
        long pktcount;
        private Queue<byte[]> pktcache = new Queue<byte[]>();

        private static object lockHeaderCache = new object();
        private Queue<string> headerCache = new Queue<string>();

        private Dictionary<string, List<byte>> dic_senddata = new Dictionary<string, List<byte>>();
        private Dictionary<string, httpsession> dic_ack_httpsession = new Dictionary<string, httpsession>();

        private Dictionary<string, string> dic_ack_seq = new Dictionary<string, string>();
        private List<string> list_ack = new List<string>();

        private Socket mainSocket = null;

        private string ip = string.Empty;
        private string filtedomain = string.Empty;

        private bool bContinueCapturing = true;

        public HttpWatch(string ip)
        {
            this.ip = ip;
        }

        public void Start()
        {
            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            mainSocket.Bind(new IPEndPoint(IPAddress.Parse(this.ip), 0));
            mainSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

            byte[] IN = new byte[4] { 1, 0, 0, 0 };
            byte[] OUT = new byte[4];
            int SIO_RCVALL = unchecked((int)0x98000001);
            mainSocket.IOControl(SIO_RCVALL, IN, OUT);

            this.bContinueCapturing = true;
            Thread t = new Thread(new ThreadStart(Execute));
            t.IsBackground = true;
            t.Start();

            Thread t2 = new Thread(new ThreadStart(ParserCacheThread));
            t2.IsBackground = true;
            t2.Start();
        }

        private void Execute()
        {
            while (bContinueCapturing)
            {
                try
                {
                    // byteData.Initialize();
                    byte[] buf = new byte[4096];
                    int size = mainSocket.Receive(buf, 0, buf.Length, SocketFlags.None);

                    // TH:判断上层协议是否是TCP协议。。。这里是用IP的报文在处理数据
                    //   if (size <= 40) continue;
                    if (buf[9] != 0x06) continue;
                    int offset = 20;
                    int srcport = PkFunction.Get2Bytes(buf, ref offset, 0);
                    int desport = PkFunction.Get2Bytes(buf, ref offset, 0);

                    bool isok = false;
                    if (iswhiteport(desport))
                    {
                        isok = true;

                        // TH:10010 确认号字段才有效 这是一个连接请求或连接接受报文
                        //if (buf[33] == 0x18) qpscount++;
                    }
                    else if (iswhiteport(srcport))
                    {

                        isok = true;
                    }
                    if (!isok) continue;

                    byte[] t = new byte[size];
                    Array.Copy(buf, 0, t, 0, t.Length);

                    pktcache.Enqueue(t);
                }
                catch { }
            }
        }

        private void ParserCacheThread()
        {
            while (bContinueCapturing)
            {
                while (pktcache.Count > 0)
                {
                    byte[] t = pktcache.Dequeue();
                    if (t != null)
                    {
                        ParcePkt_Cache(t, t.Length);
                    }
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void ParcePkt_Cache(byte[] byteData, int nReceived)
        {
            IPHeader ipHeader = new IPHeader(byteData, nReceived);
            switch (ipHeader.ProtocolType)
            {
                case Protocol.TCP:
                    TCPHeader tcpHeader = new TCPHeader(ipHeader.Data, ipHeader.MessageLength);//Length of the data field          
                    int headlen = ipHeader.HeaderLength + tcpHeader.HeaderLength;
                    if (iswhiteport(tcpHeader.DestinationPort))
                    {
                        pktcount++;
                        if (headlen >= nReceived) return;
                        if (tcpHeader.Flags == 0x18)
                        {
                            BuildPacket(true, tcpHeader.AcknowledgementNumber, tcpHeader.SequenceNumber, byteData, headlen, nReceived);
                        }
                        else if (tcpHeader.Flags == 0x10)
                        {
                            byte[] dataArray = new byte[nReceived - headlen];
                            Array.Copy(byteData, headlen, dataArray, 0, dataArray.Length);
                            if (dic_senddata.ContainsKey(tcpHeader.AcknowledgementNumber))
                            {
                                dic_senddata[tcpHeader.AcknowledgementNumber].AddRange(dataArray);
                            }
                            else
                            {
                                List<byte> listtmp = new List<byte>();
                                listtmp.AddRange(dataArray);
                                dic_senddata.Add(tcpHeader.AcknowledgementNumber, listtmp);
                            }
                        }
                    }
                    else if (iswhiteport(tcpHeader.SourcePort))
                    {
                        pktcount++;
                        if (headlen >= nReceived) return;
                        BuildPacket(false, tcpHeader.AcknowledgementNumber, tcpHeader.SequenceNumber, byteData, headlen, nReceived);
                    }

                    break;
            }
        }

        private void BuildPacket(bool isout, string ack, string seq, byte[] PacketData, int start, int reclen)
        {
            try
            {
                if (reclen <= start) return;
                byte[] dataArray = new byte[reclen - start];
                Array.Copy(PacketData, start, dataArray, 0, dataArray.Length);
                if (isout)//如果是请求包
                {
                    httpsession osesion = new httpsession();
                    osesion.id = rid;
                    osesion.senddtime = DateTime.Now;
                    osesion.ack = ack;
                    if (dic_senddata.ContainsKey(ack))
                    {
                        osesion.sendraw = dic_senddata[ack];
                    }
                    else
                    {
                        osesion.sendraw = new List<byte>();
                    }

                    osesion.sendraw.AddRange(dataArray);

                    string http_str = System.Text.Encoding.ASCII.GetString(osesion.sendraw.ToArray());
                    //lock (lockHeaderCache)
                    //{
                    //    headerCache.Enqueue(http_str);
                    //}

                    string host = Gethost(http_str);
                    if (filtedomain == "" || host.IndexOf(filtedomain) >= 0)
                    {
                        int fltag = http_str.IndexOf("\r\n");
                        if (fltag > 0)
                        {
                            string fline = http_str.Substring(0, fltag);
                            int fblacktag = fline.IndexOf(" ");
                            if (fblacktag > 0)
                            {
                                osesion.method = fline.Substring(0, fline.IndexOf(" "));
                                int urllen = fline.LastIndexOf(" ") - fblacktag - 1;
                                if (urllen > 0)
                                    osesion.url = String.Format("http://{0}{1}", host, fline.Substring(fblacktag + 1, urllen));
                            }
                        }
                        if (!this.dic_ack_httpsession.ContainsKey(osesion.ack))
                        {
                            this.dic_ack_httpsession.Add(osesion.ack, osesion);
                            this.list_ack.Add(ack);
                        }
                        rid++;
                    }

                    if (osesion.url != null)
                    {
                        if (DoMsg != null)
                        {
                            DoMsg(osesion.url);
                        }
                    }
                }
                else//如果是返回数据包
                {
                    if (dic_ack_httpsession.ContainsKey(seq))//如果第一次匹配
                    {
                        //    log(ack + ":" + seq + " 第一次返回匹配，添加映射");
                        httpsession osession = dic_ack_httpsession[seq];
                        if (osession.responseraw == null) osession.responseraw = new List<byte>();
                        osession.responseraw.AddRange(dataArray);
                        osession.responoversetime = DateTime.Now;

                        string headb = System.Text.Encoding.ASCII.GetString(dataArray);
                        lock (lockHeaderCache)
                        {
                            headerCache.Enqueue(osession.url ?? string.Empty);
                        }

                        if (osession.url != null)
                        {
                            if (DoMsg != null)
                            {
                                DoMsg(osession.url);
                            }
                        }

                        int flinetag = headb.IndexOf("\r\n");
                        if (flinetag > 0)
                        {
                            headb = headb.Substring(0, flinetag);
                            string[] p3 = headb.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (p3.Length >= 2)
                            {
                                osession.statucode = int.Parse(p3[1]);
                            }
                        }
                        dic_ack_httpsession[seq] = osession;
                        if (!dic_ack_seq.ContainsKey(ack))
                        {
                            dic_ack_seq.Add(ack, seq);
                        }
                    }
                    else
                    {
                        // 后面的数据包
                        if (dic_ack_seq.ContainsKey(ack))
                        {
                            httpsession osession = dic_ack_httpsession[dic_ack_seq[ack]];
                            osession.responseraw.AddRange(dataArray);
                            dic_ack_httpsession[dic_ack_seq[ack]] = osession;
                        }
                        else
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool iswhiteport(int port)
        {
            int[] ports = new int[] { 80, 8080 };
            for (int i = 0; i < ports.Length; i++)
            {
                if (port == ports[i])
                {
                    return true;
                }
            }

            return false;
        }

        Regex rhost = new Regex(@"\bhost:.(\S*)", RegexOptions.IgnoreCase);
        private string Gethost(string http)
        {
            Match m = rhost.Match(http);
            return m.Groups[1].Value;
        }

        public void ResetCheck()
        {
            lock (lockHeaderCache)
            {
                headerCache.Clear();
            }
        }

        public bool CheckExist(string key)
        {
            lock (lockHeaderCache)
            {
                foreach (var item in headerCache)
                {
                    if (item.Contains(key))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }

    [Serializable()]
    public struct httpsession
    {
        public string url;
        public string method;
        public DateTime senddtime;
        public DateTime responoversetime;
        public int id;
        public string ack;
        public List<byte> sendraw;
        public List<byte> responseraw;
        public int statucode;
    }

    public enum Protocol
    {
        TCP = 6,
        UDP = 17,
        Unknown = -1
    };
}
