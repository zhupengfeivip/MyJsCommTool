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
