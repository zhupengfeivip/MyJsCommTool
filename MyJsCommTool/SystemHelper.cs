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
        /// <summary>
        /// 
        /// </summary>
        public const char functionLeftChar = '(';

        /// <summary>
        /// 
        /// </summary>
        public const char functionRightChar = ')';

        /// <summary>
        /// 
        /// </summary>
        public const char commentLeftChar = '[';

        /// <summary>
        /// 
        /// </summary>
        public const char commentRightChar = ']';

        public string buildText(string data)
        {
            string newData = data;
            //去掉空格
            newData = newData.Replace(" ", "");
            //去掉注释
            newData = clearAllComment(newData);

            do
            {
                int index = newData.IndexOf(functionLeftChar);
                if (index < 0) break;

                int index2 = newData.IndexOf(functionRightChar, index);
                string cmdText = newData.Substring(index, index2 - index + 1);
                Debug.WriteLine(cmdText);

                string cmdParamText = cmdText.TrimStart(functionLeftChar).TrimEnd(functionRightChar);
                string[] cmdParas = cmdParamText.Split(',');
                switch (cmdParas[0])
                {
                    case "当前时间":
                        {
                            //参数格式 [当前时间,yyyyMMdd]
                            //第二个参数为时间格式
                            string dateFormat = cmdParas[1];
                            //将参数部分替换为要求的格式
                            newData = newData.Replace(cmdText, DateTime.Now.ToString(dateFormat));
                        }
                        break;
                    case "总加和":
                        {
                            //参数格式 [总加和,2,8]第二个总加和从第几个字节开始，第三个参数为结束
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
                            //参数格式 [随机数字,0,100]
                            //第二个参数表示最小值
                            int minValue = int.Parse(cmdParas[1]);
                            //第三个参数表示最大值
                            int maxValue = int.Parse(cmdParas[2]);
                            //第四个参数表示输出几位，就是几个字符，一般为双数
                            int charCoount = int.Parse(cmdParas[3]);
                            //第五个参数，输出格式1为16进制，默认；2为BCD
                            int outType = 1;
                            if(cmdParas.Length >= 5)
                                outType = int.Parse(cmdParas[4]);
                            Random rd = new Random();
                            int rdValue = rd.Next(minValue, maxValue);
                            string str;
                            if (outType == 1)
                                str = rdValue.ToString("X2");
                            else
                                str = rdValue.ToString();

                            if (str.Length < charCoount)
                                str = str.PadLeft(charCoount, '0');

                            //将参数部分替换为要求的格式
                            newData = newData.Replace(cmdText, str);
                        }
                        break;
                    default:
                        break;
                }

            } while (true);

            return newData;
        }

        public string clearAllComment(string data)
        {
            string newData = data;
            //去掉空格
            newData = newData.Replace(" ", "");
            do
            {
                int index = newData.IndexOf(commentLeftChar);
                if (index < 0) break;

                int index2 = newData.IndexOf(commentRightChar, index);
                string cmdText = newData.Substring(index, index2 - index + 1);
                Debug.WriteLine(cmdText);
                //将注释部分替换为空
                newData = newData.Replace(cmdText, "");
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
