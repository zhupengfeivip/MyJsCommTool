using System;
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
        private RichTextBox tBox;

        public TextBoxTraceListener(RichTextBox box)
        {
            this.tBox = box;
        }

        public override void Write(string msg)
        {
            //allows tBox to be updated from different thread
            tBox.Parent.Invoke(new MethodInvoker(delegate ()
            {
                tBox.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}    {msg}");
                tBox.SelectionStart = tBox.Text.Length;
                tBox.ScrollToCaret();
            }));
        }

        public override void WriteLine(string msg)
        {
            Write(msg + "\r\n");
        }
    }
}
