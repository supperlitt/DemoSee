using System.Net;
using System.Text;
using System;
using System.IO;

namespace WinTestSocketRaw
{
    public class TCPHeader
    {
        //TCP header fields
        private ushort usSourcePort;              //Sixteen bits for the source port number
        private ushort usDestinationPort;         //Sixteen bits for the destination port number
        private uint   uiSequenceNumber=555;          //Thirty two bits for the sequence number
        private uint   uiAcknowledgementNumber=555;   //Thirty two bits for the acknowledgement number
        private ushort usDataOffsetAndFlags=555;      //Sixteen bits for flags and data offset
        private ushort usWindow=555;                  //Sixteen bits for the window size
        private short  sChecksum=555;                 //Sixteen bits for the checksum
                                                    //(checksum can be negative so taken as short)
        private ushort usUrgentPointer;           //Sixteen bits for the urgent pointer
        //End TCP header fields

        private byte   byHeaderLength;            //Header length

        private byte[] byTCPData = new byte[4096];//Data carried by the TCP packet
       
        public TCPHeader(byte [] byBuffer, int nReceived)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
                BinaryReader binaryReader = new BinaryReader(memoryStream);
           
                usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16 ());

                usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16 ());

                uiSequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

                uiAcknowledgementNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

                usDataOffsetAndFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                usWindow = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                sChecksum = (short)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                usUrgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                byHeaderLength = (byte)(usDataOffsetAndFlags >> 12);
                byHeaderLength *= 4;

            }
            catch (Exception ex)
            {
            //    MessageBox.Show(ex.Message, "HttpWatch " + (nReceived), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public ushort SourcePort
        {
            get
            {
                return usSourcePort;
            }
        }

        public ushort DestinationPort
        {
            get
            {
                return usDestinationPort;
            }
        }

        public string SequenceNumber
        {
            get
            {
                return uiSequenceNumber.ToString();
            }
        }

        public string AcknowledgementNumber
        {
            get
            {
                //If the ACK flag is set then only we have a valid value in
                //the acknowlegement field, so check for it beore returning 
                //anything
                if ((usDataOffsetAndFlags & 0x10) != 0)
                {
                    return uiAcknowledgementNumber.ToString();
                }
                else
                    return "";
            }
        }

        public int HeaderLength
        {
            get
            {
                return byHeaderLength;
            }
        }

        public string WindowSize
        {
            get
            {
                return usWindow.ToString();
            }
        }

        public string UrgentPointer
        {
            get
            {
                //If the URG flag is set then only we have a valid value in
                //the urgent pointer field, so check for it beore returning 
                //anything
                if ((usDataOffsetAndFlags & 0x20) != 0)
                {
                    return usUrgentPointer.ToString();
                }
                else
                    return "";
            }
        }

        public int Flags
        {
            get
            {
             
                return usDataOffsetAndFlags&0x3F;
 
            }
        }

        public string Checksum
        {
            get
            {
                //Return the checksum in hexadecimal format
                return string.Format("0x{0:x2}", sChecksum);
            }
        }

      
    }
}