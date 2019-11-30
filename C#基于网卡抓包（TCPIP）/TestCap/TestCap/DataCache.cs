using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCap
{
    public class SendDataCache
    {
        private static List<DataInfo> dataList = new List<DataInfo>();

        static SendDataCache()
        {
        }

        public static void AddData(uint id, byte[] data)
        {
            lock (dataList)
            {
                var item = dataList.Find(p => p.Id == id);
                if (item == null)
                {
                    var info = new DataInfo();
                    info.Id = id;
                    info.Data.AddRange(data);
                    dataList.Add(info);
                }
                else
                {
                    item.Data.AddRange(data);
                }
            }
        }

        public static byte[] GetData(uint id)
        {
            lock (dataList)
            {
                var item = dataList.Find(p => p.Id == id);
                if (item == null)
                {
                    return null;
                }
                else
                {
                    return item.Data.ToArray();
                }
            }
        }
    }

    public class ReceiveDataCache
    {
        private static List<DataInfo> dataList = new List<DataInfo>();

        static ReceiveDataCache()
        {
        }

        public static void AddData(uint id, byte[] data)
        {
            lock (dataList)
            {
                var item = dataList.Find(p => p.Id == id);
                if (item == null)
                {
                    var info = new DataInfo();
                    info.Id = id;
                    info.Data.AddRange(data);
                    dataList.Add(info);
                }
                else
                {
                    item.Data.AddRange(data);
                }
            }
        }

        public static byte[] GetData(uint id)
        {
            lock (dataList)
            {
                var item = dataList.Find(p => p.Id == id);
                if (item == null)
                {
                    return null;
                }
                else
                {
                    return item.Data.ToArray();
                }
            }
        }
    }

    public class DataInfo
    {
        public uint Id { get; set; }

        private List<byte> data = new List<byte>();

        public List<byte> Data
        {
            set { this.data = value; }
            get { return this.data; }
        }
    }
}
