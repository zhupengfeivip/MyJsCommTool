using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyJsCommTool
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();



            // Start the browser after initialize global component
            InitializeChromium();
        }


        public ChromiumWebBrowser chromeBrowser;

        //初始化浏览器并启动
        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.Locale = "zh-CN";
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CachePath = Directory.GetCurrentDirectory() + @"\cache";
            settings.LogSeverity = LogSeverity.Info;
            //settings.LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            settings.LogFile = Path.Combine(Directory.GetCurrentDirectory() + @"\log", "log.txt");
            
            // Initialize cef with the provided settings
            Cef.Initialize(settings);

            string url = Application.StartupPath + "\\myHtml\\test2.html";
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(url);
            // Add it to the form and fill it to the form window.
            this.panel1.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            //chromeBrowser.RegisterAsyncJsObject("googleBrower", new ScriptCallbackManager(), new CefSharp.BindingOptions { CamelCaseJavascriptNames = false });
            //chromeBrowser.RegisterJsObject("googleBrower", new ScriptCallbackManager(), true, BindingOptions.DefaultBinder);   //带false 可以识别大写字母开头的函数或变量
            // this.mychrome.RegisterJsObject("JsObj", new CallbackObjectForJs());  //不带 false  不能识别大写字母开头的函数或变量

            //For async object registration (equivalent to the old RegisterAsyncJsObject)
            chromeBrowser.JavascriptObjectRepository.Register("serialPortHelper", new SerialPortHelper(), true, BindingOptions.DefaultBinder);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //窗体关闭时，记得停止浏览器
            Cef.Shutdown();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            TextBoxTraceListener tbtl = new TextBoxTraceListener(this.textBox1);
            Debug.Listeners.Add(tbtl);
            Trace.Listeners.Add(tbtl);
            
            Debug.WriteLine("Testing Testing 123");
        }


    }
}
