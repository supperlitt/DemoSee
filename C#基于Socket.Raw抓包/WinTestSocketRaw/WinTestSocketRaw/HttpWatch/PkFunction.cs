using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;

namespace WinTestSocketRaw
{
    /// <summary>
    /// 数据包处理函数 修正set4bytes
    /// </summary>
    public class PkFunction
    {
        public const int NORMAL = 0;
        public const int VALUE = 1;

        /// <summary>
        /// 转换字符串到包数组
        /// </summary>
        /// <param name="datastr"></param>
        /// <returns></returns>
        public static byte[] StrToByte(string datastr)
        {
            if (datastr.Length % 2 == 1) datastr += "00";
            byte[] ret = new byte[datastr.Length / 2];
            for (int i = 0; i < datastr.Length; i += 2)
            {
                if (i < datastr.Length) ret[i / 2] = (byte)(System.Uri.FromHex(datastr[i]) * 16 + System.Uri.FromHex(datastr[i + 1]));
            }
            return ret;
        }

        public static bool Islocalip(string ip)//是否内网IP 
        {
            if (ip.Substring(0, ip.IndexOf(".")) == "10" || ip.Substring(0, ip.IndexOf(".")) == "192" || ip.Substring(0, ip.IndexOf(".")) == "172")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetIpAddress(string SrvName)
        {
            IPHostEntry ipEntry = new IPHostEntry();
            IPAddress Ip = new IPAddress(0);

            if (SrvName.Trim() == "")
            {
                //ipEntry = Dns.Resolve( Dns.GetHostName() );
                ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            }
            else
            {
                ipEntry = Dns.GetHostEntry(SrvName);
            }

            Ip = ipEntry.AddressList[0];

            return Ip.ToString();

        }

        public static string GetHostName(string IpAddr)
        {
            IPHostEntry iphEntry = new IPHostEntry();

            if (IpAddr.Trim() == "")
            {
                return Dns.GetHostName();
            }
            else
            {
                //iphEntry = Dns.GetHostByAddress(IpAddr );
                iphEntry = Dns.GetHostEntry(IpAddr);
            }

            return iphEntry.HostName;

        }

        public static ushort Get2Bytes(byte[] ptr, ref int Index, int Type)
        {
            ushort u = 0;

            if (Type == NORMAL)
            {
                u = (ushort)ptr[Index++];
                u *= 256;
                u += (ushort)ptr[Index++];
            }
            else if (Type == VALUE)
            {
                u = (ushort)ptr[++Index];
                u *= 256; Index--;
                u += (ushort)ptr[Index++]; Index++;
            }

            return u;
        }

        public static void KillProcess(string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            try
            {
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                {
                    if (!thisproc.CloseMainWindow())
                    {
                        thisproc.Kill();
                    }
                }
            }
            catch
            {
            }
        }

        public static bool CheckProcess(string processName)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            try
            {
                int i = 0;
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                {
                    i++;

                }
                if (i > 1) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void Set2Bytes(ref byte[] ptr, ref int Index, ushort NewValue, int Type)
        {

            if (Type == NORMAL)
            {
                ptr[Index] = (byte)(NewValue >> 8);
                ptr[Index + 1] = (byte)NewValue;
            }
            else if (Type == VALUE)
            {
                ptr[Index + 0] = (byte)NewValue;
                ptr[Index + 1] = (byte)(NewValue >> 8);
                Index += 2;
            }

        }

        public static uint Get3Bytes(byte[] ptr, ref int Index, int Type)
        {
            uint ui = 0;

            if (Type == NORMAL)
            {
                ui = ((uint)ptr[Index++]) << 16;
                ui += ((uint)ptr[Index++]) << 8;
                ui += (uint)ptr[Index++];
            }

            return ui;
        }

        public static uint Get4Bytes(byte[] ptr, ref int Index, int Type)
        {
            uint ui = 0;

            if (Type == NORMAL)
            {
                ui = ((uint)ptr[Index++]) << 24;
                ui += ((uint)ptr[Index++]) << 16;
                ui += ((uint)ptr[Index++]) << 8;
                ui += (uint)ptr[Index++];
            }
            else if (Type == VALUE)
            {
                ui = ((uint)ptr[Index + 3]) << 24;
                ui += ((uint)ptr[Index + 2]) << 16;
                ui += ((uint)ptr[Index + 1]) << 8;
                ui += (uint)ptr[Index]; Index += 4;
            }

            return ui;
        }

        public static void Set4Bytes(ref byte[] ptr, ref int Index, uint NewValue, int Type)
        {

            if (Type == NORMAL)
            {
                ptr[Index] = (byte)(NewValue >> 24);
                ptr[Index + 1] = (byte)(NewValue >> 16);
                ptr[Index + 2] = (byte)(NewValue >> 8);
                ptr[Index + 3] = (byte)NewValue;
            }
            else if (Type == VALUE)
            {
                ptr[Index] = (byte)NewValue;
                ptr[Index + 1] = (byte)(NewValue >> 8);
                ptr[Index + 2] = (byte)(NewValue >> 16);
                ptr[Index + 3] = (byte)(NewValue >> 24);
            }
            Index += 4;
        }

        public static ulong Get8Bytes(byte[] ptr, ref int Index, int Type)
        {
            ulong ui = 0;

            if (Type == NORMAL)
            {
                ui = ((uint)ptr[Index++]) << 56;
                ui += ((uint)ptr[Index++]) << 48;
                ui += ((uint)ptr[Index++]) << 40;
                ui += ((uint)ptr[Index++]) << 32;
                ui += ((uint)ptr[Index++]) << 24;
                ui += ((uint)ptr[Index++]) << 16;
                ui += ((uint)ptr[Index++]) << 8;
                ui += (uint)ptr[Index++];
            }
            else if (Type == VALUE)
            {
                ui = ((uint)ptr[Index + 7]) << 56;
                ui += ((uint)ptr[Index + 6]) << 48;
                ui += ((uint)ptr[Index + 5]) << 40;
                ui += ((uint)ptr[Index + 4]) << 32;
                ui += ((uint)ptr[Index + 3]) << 24;
                ui += ((uint)ptr[Index + 2]) << 16;
                ui += ((uint)ptr[Index + 1]) << 8;
                ui += (uint)ptr[Index]; Index += 8;
            }

            return ui;
        }

        public static void Set8Bytes(ref byte[] ptr, ref int Index, ulong NewValue, int Type)
        {
            if (Type == NORMAL)
            {
                ptr[Index] = (byte)(NewValue >> 56);
                ptr[Index + 1] = (byte)(NewValue >> 48);
                ptr[Index + 2] = (byte)(NewValue >> 40);
                ptr[Index + 3] = (byte)(NewValue >> 32);
                ptr[Index + 4] = (byte)(NewValue >> 24);
                ptr[Index + 5] = (byte)(NewValue >> 16);
                ptr[Index + 6] = (byte)(NewValue >> 8);
                ptr[Index + 7] = (byte)NewValue;
            }
            else if (Type == VALUE)
            {
                ptr[Index + 7] = (byte)(NewValue >> 56);
                ptr[Index + 6] = (byte)(NewValue >> 48);
                ptr[Index + 5] = (byte)(NewValue >> 40);
                ptr[Index + 4] = (byte)(NewValue >> 32);
                ptr[Index + 3] = (byte)(NewValue >> 24);
                ptr[Index + 2] = (byte)(NewValue >> 16);
                ptr[Index + 1] = (byte)(NewValue >> 8);
                ptr[Index] = (byte)NewValue;
                Index += 8;
            }

        }

        public static string GetIpAddress(byte[] ptr, ref int Index)
        {
            string str = "";

            str += ptr[Index++].ToString() + ".";
            str += ptr[Index++].ToString() + ".";
            str += ptr[Index++].ToString() + ".";
            str += ptr[Index++].ToString();

            return str;
        }

        public static string GetIpAddress(byte[] ptr, ref int Index, int Length)
        {
            string str = "";
            int i = 0;

            for (i = 0; i < Length - 1; i++)
                str += ptr[Index++].ToString() + ".";

            str += ptr[Index++].ToString();

            return str;
        }

        public static short Swap(short s)
        {
            short b1 = 0, b2 = 0;
            short rs;

            b1 = (short)((s >> 8) & 0x00ff);
            b2 = (short)((s & 0x00ff) << 8);
            rs = (short)(b1 + b2);

            return rs;
        }

        public static ushort Swap(ushort us)
        {
            ushort b1 = 0, b2 = 0;
            ushort rus;

            b1 = (ushort)((us >> 8) & 0x00ff);
            b2 = (ushort)((us & 0x00ff) << 8);
            rus = (ushort)(b1 + b2);

            return rus;
        }

        public static int Swap(int i)
        {
            int b1 = 0, b2 = 0, b3 = 0, b4 = 0;
            int ri;

            b1 = (i >> 24) & 0x000000ff;
            b2 = ((i >> 16) & 0x000000ff) << 8;
            b3 = ((i >> 8) & 0x000000ff) << 16;
            b4 = (i & 0x000000ff) << 24;

            ri = b1 + b2 + b3 + b4;

            return ri;
        }

        public static uint Swap(uint ui)
        {
            uint b1 = 0, b2 = 0, b3 = 0, b4 = 0;
            uint rui;

            b1 = (ui >> 24) & 0x000000ff;
            b2 = ((ui >> 16) & 0x000000ff) << 8;
            b3 = ((ui >> 8) & 0x000000ff) << 16;
            b4 = (ui & 0x000000ff) << 24;

            rui = b1 + b2 + b3 + b4;

            return rui;
        }

        public static long Swap(long l)
        {
            long b1 = 0, b2 = 0, b3 = 0, b4 = 0;
            long b5 = 0, b6 = 0, b7 = 0, b8 = 0;
            long rl;

            b1 = ((l >> 56) & 0x00000000000000ff);
            b2 = ((l >> 48) & 0x00000000000000ff) << 8;
            b3 = ((l >> 40) & 0x00000000000000ff) << 16;
            b4 = ((l >> 32) & 0x00000000000000ff) << 24;
            b5 = ((l >> 24) & 0x00000000000000ff) << 32;
            b6 = ((l >> 16) & 0x00000000000000ff) << 40;
            b7 = ((l >> 8) & 0x00000000000000ff) << 48;
            b8 = (l & 0x00000000000000ff) << 56;

            rl = b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8;

            return rl;
        }

        public static ulong Swap(ulong ul)
        {
            ulong b1 = 0, b2 = 0, b3 = 0, b4 = 0;
            ulong b5 = 0, b6 = 0, b7 = 0, b8 = 0;
            ulong url;

            b1 = ((ul >> 56) & 0x00000000000000ff);
            b2 = ((ul >> 48) & 0x00000000000000ff) << 8;
            b3 = ((ul >> 40) & 0x00000000000000ff) << 16;
            b4 = ((ul >> 32) & 0x00000000000000ff) << 24;
            b5 = ((ul >> 24) & 0x00000000000000ff) << 32;
            b6 = ((ul >> 16) & 0x00000000000000ff) << 40;
            b7 = ((ul >> 8) & 0x00000000000000ff) << 48;
            b8 = (ul & 0x00000000000000ff) << 56;

            url = b1 + b2 + b3 + b4 + b5 + b6 + b7 + b8;

            return url;
        }

        /// <summary>
        /// FF:FF:FF
        /// </summary>
        /// <param name="PacketData"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static string GetMACAddress(byte[] PacketData, ref int Index)
        {
            string Tmp = "";
            int i = 0;

            for (i = 0; i < 5; i++)//前5组HEX
            {
                Tmp += PacketData[Index++].ToString("x02") + ":";
            }

            Tmp += PacketData[Index++].ToString("x02");//最后一组HEX
            // MessageBox.Show(Tmp.ToUpper());
            return Tmp.ToUpper();

        }
        /// <summary>
        /// 没有冒号格式 FFFFF
        /// </summary>
        /// <param name="PacketData"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static string GetMACAddress2(byte[] PacketData, ref int Index)
        {
            string Tmp = "";
            int i = 0;

            for (i = 0; i < 5; i++)//前5组HEX
            {
                Tmp += PacketData[Index++].ToString("x02");
            }

            Tmp += PacketData[Index++].ToString("x02");//最后一组HEX
            // MessageBox.Show(Tmp.ToUpper());
            return Tmp.ToUpper();

        }
        public static void Setipaddres(ref byte[] PacketDate, string ip, ref int Index)//设置IP
        {
            if (ip != null)
            {

                byte[] _ipaddr = IPAddress.Parse(ip).GetAddressBytes();
                Array.Copy(_ipaddr, 0, PacketDate, Index, 4);
                Index += _ipaddr.Length;
            }
        }
    }
}
