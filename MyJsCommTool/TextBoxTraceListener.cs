﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyJsCommTool
{
    class TextBoxTraceListener : TraceListener
    {
        private TextBox tBox;

        public TextBoxTraceListener(TextBox box)
        {
            this.tBox = box;
        }

        public override void Write(string msg)
        {
            //allows tBox to be updated from different thread
            tBox.Parent.Invoke(new MethodInvoker(delegate ()
            {
                tBox.AppendText(msg);
            }));
        }

        public override void WriteLine(string msg)
        {
            Write(msg + "\r\n");
        }
    }
}