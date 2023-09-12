using System;
using System.Collections.Generic;
using System.Text;
using SocketGameProtocol;
using Google.Protobuf;
using System.Linq;

namespace SocketGameServer.Tool
{
    class Message
    {
        private byte[] buffer = new byte[1024];

        private int startindex;

        public byte[] Buffer
        {
            get
            {
                return buffer;
            }
        }

        public int StartIndex
        {
            get
            {
                return startindex;
            }
        }

        public int Remsize
        {
            get
            {
                return buffer.Length - startindex;
            }
        }

        //处理分包粘包的关键代码
        public void ReadBuffer(int len,Action<MainPack> HandleRequest)
        {
            startindex += len;
            
            while(true)
            {
                if (startindex <= 4) return;
                int count = BitConverter.ToInt32(buffer, 0);
                if (startindex>=(count+4))//当一个数据包头中所有数据已经传输到达，再进行解析。
                {
                    MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, 4, count);
                    HandleRequest(pack);
                    //将当前缓冲区后面的数据拷贝复制到开头，进行消息解析。
                    Array.Copy(buffer, count + 4, buffer, 0, startindex - count - 4);

                    startindex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }

        public static byte[] PackData(MainPack pack)
        {
            //将包转换为字节数组
            byte[] data = pack.ToByteArray();
            //将包体长度转换为字节数组
            byte[] head = BitConverter.GetBytes(data.Length);
            //将包头与包体进行拼接
            return head.Concat(data).ToArray();
        }

        public static byte[] PackDataUDP(MainPack pack)
        {
            return pack.ToByteArray();
        }
    }
}
