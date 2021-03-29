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
    /// <summary>
    /// 
    /// </summary>
    class SerialPortHelper
    {
        public System.IO.Ports.SerialPort com = new System.IO.Ports.SerialPort();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <param name="parity"></param>
        public void init(string portName = "COM1", int baudRate = 57600, int dataBits = 8, int stopBits = 1, int parity = 0)
        {
            com.PortName = portName;  //端口名称，默认COM1
            com.BaudRate = baudRate;
            com.DataBits = dataBits;
            com.StopBits = (System.IO.Ports.StopBits)stopBits;
            com.Parity = (System.IO.Ports.Parity)parity;

            Debug.WriteLine($"init data port:{portName} baudRate:{baudRate} dataBits:{dataBits} stopBits:{stopBits} parity:{parity}");
        }

        /// <summary>
        /// 
        /// </summary>
        public void open()
        {
            try
            {
                com.Open();
                Debug.WriteLine($"Open串口成功");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Open串口时发生异常," + ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void close()
        {
            try
            {
                com.Close();
                Debug.WriteLine($"Close串口成功");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Close串口时发生异常," + ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string recvData(int type = 1)
        {
            //Debug.WriteLine($"recvData successfully.");

            Thread.Sleep(50);  //（毫秒）等待一定时间，确保数据的完整性 int len        
            int len = com.BytesToRead;
            if (len == 0) return "";

            byte[] buff = new byte[len];
            com.Read(buff, 0, len);

            string receive;
            if (type == 1)
                receive = buff.ByteToHexString();
            else
                receive = buff.ToStringByAscii();

            Debug.WriteLine($"recvData：{receive}");

            return receive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type">发送的数据类型 1为HEX,2为string</param>
        public void sendData(string data, int type = 1)
        {
            if (com.IsOpen == false)
            {
                Debug.WriteLine($"串口未打开");
                return;
            }

            byte[] sendBytes = data.Replace(" ", "").ToBytes();
            if (type == 1)
                com.Write(sendBytes, 0, sendBytes.Length);
            else
                com.Write(data.Replace(" ", ""));

            Debug.WriteLine($"send_data {data}");
        }


    }
}
