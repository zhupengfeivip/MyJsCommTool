using Bonn.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace MyJsCommTool
{
    class ScriptCallbackManager
    {
        public System.IO.Ports.SerialPort com = new System.IO.Ports.SerialPort();

        /// <summary>
        /// 天平通讯串口
        /// </summary>
        public string PortName = "COM3";

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate = 57600;

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits = 8;

        /// <summary>
        /// 停止位
        /// </summary>
        public int StopBits = (int)System.IO.Ports.StopBits.One;

        /// <summary>
        /// 校验位
        /// </summary>
        public int Parity = (int)System.IO.Ports.Parity.None;

        /// <summary>
        /// 查找电脑信息
        /// </summary> IJavascriptCallback javascriptCallback
        /// <param name="javascriptCallback"></param>
        public string FindComputerInfo()
        {
            return JsonConvert.SerializeObject(new
            {
                cpu_id = "1",
                disk_id = "2",
                host_name = "联想",
                networkcard = "是的",
                serialNumber = "12233",
                manufacturer = "hello",
                product = "联想",
            });
        }

        public void init()
        {
            com.PortName = "COM2";  //端口名称，默认COM1
            com.BaudRate = BaudRate;
            com.DataBits = DataBits;
            com.StopBits = (System.IO.Ports.StopBits)StopBits;
            com.Parity = (System.IO.Ports.Parity)Parity;

            com.Open();
        }

        public string recvData()
        {
            Debug.WriteLine($"recvData successfully.");

            Thread.Sleep(50);  //（毫秒）等待一定时间，确保数据的完整性 int len        
            int len = com.BytesToRead;
            if (len == 0) return "";

            byte[] buff = new byte[len];
            com.Read(buff, 0, len);

            Debug.WriteLine($"recvData successfully. {buff.ByteToHexString()}");

            return buff.ByteToHexString();
        }

        public void send_data(string data)
        {
            byte[] sendBytes = data.Replace(" ", "").ToBytes();
            com.Write(sendBytes, 0, sendBytes.Length);

            Debug.WriteLine($"send_data {data}");
        }

        public int div(int dividend, int divisor)
        {
            Thread.Sleep(1000 * 2);
            Debug.WriteLine($"{DateTime.Now.ToString()} Object was bound successfully.");
            return dividend / divisor;
        }

    }
}
