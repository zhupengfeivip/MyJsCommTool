using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bonn.Helper;
using System.Diagnostics;

namespace MyJsCommTool
{
    class TcpClientHelper
    {
        TcpClient client = new TcpClient();
        public BinaryReader br;
        public BinaryWriter bw;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="port"></param>
        public void conn(string serverIp, int port)
        {
            // 连接到服务器
            client.Connect(serverIp, port);

            Debug.WriteLine($"conn：{serverIp}:{port}");
        }

        /// <summary>
        /// 
        /// </summary>
        public void close()
        {
            if (client != null)
            {
                client.Close();
                client = null;
            }

            Debug.WriteLine($"close");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="type">发送的数据类型 1为HEX,2为string</param>
        public void sendData(string strData, int type = 1)
        {
            Debug.WriteLine($"sendData：{strData}");

            NetworkStream clientStream = client.GetStream();
            bw = new BinaryWriter(clientStream);

            //去掉所有的空格，再转换为字节
            byte[] data = strData.Replace(" ","").HexStringToByte();        
            if (type == 1)
                bw.Write(data);
            else
                bw.Write(strData.Replace(" ", ""));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string recvData(int type = 1)
        {
            System.Threading.Thread.Sleep(200);
            if (client.Available <= 0) return "";

            NetworkStream clientStream = client.GetStream();
            br = new BinaryReader(clientStream);

            byte[] buff = new byte[2048];
            int length = client.Client.Receive(buff);
            byte[] recvBuff = new byte[length];
            Buffer.BlockCopy(buff, 0, recvBuff, 0, recvBuff.Length);
            string receive;
            if (type == 1)
                receive = recvBuff.ByteToHexString();
            else
                receive = recvBuff.ToStringByAscii();
            Debug.WriteLine($"recvData：{receive}");

            return receive;
        }
    }
}
