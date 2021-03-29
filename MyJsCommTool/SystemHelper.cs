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
    class SystemHelper
    {
        public string buildText(string data)
        {
            string newData = data;
            //去掉空格
            newData = newData.Replace(" ", "");
            do
            {
                int index = newData.IndexOf("[");
                if (index < 0) break;

                int index2 = newData.IndexOf("]", index);
                string cmdText = newData.Substring(index, index2 - index + 1);
                Debug.WriteLine(cmdText);

                string cmdParamText = cmdText.TrimStart('[').TrimEnd(']');
                string[] cmdParas = cmdParamText.Split(',');
                switch (cmdParas[0])
                {
                    case "当前时间":
                        {
                            //第二个参数为时间格式
                            string dateFormat = cmdParas[1];
                            //将参数部分替换为要求的格式
                            newData = newData.Replace(cmdText, DateTime.Now.ToString(dateFormat));
                        }
                        break;
                    case "总加和":
                        {
                            //第二个总加和从第几个字节开始，第三个参数为结束
                            //注意这里的位数是两位计算的，一个字节是两位
                            int startIndex = int.Parse(cmdParas[1]);
                            int stopIndex = int.Parse(cmdParas[2]);
                            byte[] calcBytes = newData.Substring(startIndex, stopIndex).ToBytes();
                            byte sum = calcBytes.GetVerifyFramesSum();
                            //将参数部分替换为要求的格式
                            newData = newData.Replace(cmdText, sum.ToString("X2"));
                        }
                        break;
                    case "随机数字":
                        {

                        }
                        break;
                    default:
                        break;
                }

            } while (true);

            return newData;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void log(string message)
        {
            Debug.WriteLine(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void sleep(int millsecond)
        {
            System.Threading.Thread.Sleep(millsecond);
        }






    }
}
