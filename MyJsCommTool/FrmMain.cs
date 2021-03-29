using CefSharp;
using CefSharp.WinForms;
using ICSharpCode.TextEditor.Document;
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

        public List<string> pathList = new List<string>();

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

            //string url = Application.StartupPath + "\\myHtml\\tcpClient1.html";
            // Create a browser component
            //chromeBrowser = new ChromiumWebBrowser(pathList[tscbxJsList.SelectedIndex]);
            chromeBrowser = new ChromiumWebBrowser();
            // Add it to the form and fill it to the form window.
            this.panel1.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;

            //chromeBrowser.RegisterAsyncJsObject("googleBrower", new ScriptCallbackManager(), new CefSharp.BindingOptions { CamelCaseJavascriptNames = false });
            //chromeBrowser.RegisterJsObject("googleBrower", new ScriptCallbackManager(), true, BindingOptions.DefaultBinder);   //带false 可以识别大写字母开头的函数或变量
            // this.mychrome.RegisterJsObject("JsObj", new CallbackObjectForJs());  //不带 false  不能识别大写字母开头的函数或变量

            //For async object registration (equivalent to the old RegisterAsyncJsObject)
            chromeBrowser.JavascriptObjectRepository.Register("serialPortHelper", new SerialPortHelper(), true, BindingOptions.DefaultBinder);
            chromeBrowser.JavascriptObjectRepository.Register("tcpClientHelper", new TcpClientHelper(), true, BindingOptions.DefaultBinder);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo folder = new DirectoryInfo(Application.StartupPath + "\\myHtml\\");

                foreach (FileInfo file in folder.GetFiles("*.html"))
                {
                    Debug.WriteLine(file.Name);
                    tscbxJsList.Items.Add(file.Name);
                    pathList.Add(file.FullName);
                }

                if (tscbxJsList.Items.Count > 0)
                    tscbxJsList.SelectedIndex = 0;

                //自定义代码高亮
                string path = Application.StartupPath + "\\highlighting";
                FileSyntaxModeProvider fsmp;
                fsmp = new FileSyntaxModeProvider(path);
                HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmp);

                textEditorControl1.ShowEOLMarkers = false;
                textEditorControl1.ShowHRuler = false;
                textEditorControl1.ShowInvalidLines = false;
                textEditorControl1.ShowMatchingBracket = true;
                textEditorControl1.ShowSpaces = false;
                textEditorControl1.ShowTabs = false;
                textEditorControl1.ShowVRuler = false;
                textEditorControl1.AllowCaretBeyondEOL = false;
                //textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy(Application.StartupPath + "//highlighting//JavaScript.xshd");
                textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("HTML");
                textEditorControl1.Encoding = Encoding.GetEncoding("GB2312");


                this.toolStrip1.Visible = false;//暂时没按钮，先隐藏
                //tsbtnOpen.PerformClick();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //窗体关闭时，记得停止浏览器
            Cef.Shutdown();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            TextBoxTraceListener tbtl = new TextBoxTraceListener(this.rtbxLog);
            Debug.Listeners.Add(tbtl);
            //Trace.Listeners.Add(tbtl);

            //Debug.WriteLine("Testing Testing 123");
        }

        private void tsbtnOpen_Click(object sender, EventArgs e)
        {
            string fileName = pathList[tscbxJsList.SelectedIndex];
            chromeBrowser.Load(fileName);

            textEditorControl1.Text = File.ReadAllText(fileName);
        }

        private void tsmiOpenDevTools_Click(object sender, EventArgs e)
        {
            //CefWindowInfo windowInfo;
            var host = chromeBrowser.GetBrowserHost();

            var windowInfo = new WindowInfo();
            windowInfo.SetAsPopup(host.GetWindowHandle(), "DevTools");
            //windowInfo.SetAsPopup(this.panel2.Handle, "DevTools");
            //windowInfo.SetAsChild(this.panel2.Handle);
            chromeBrowser.ShowDevTools(windowInfo); // Opens Chrome Developer tools window

        }

        private void tsbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = pathList[tscbxJsList.SelectedIndex];

                File.WriteAllText(fileName, textEditorControl1.Text);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void tsbtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = pathList[tscbxJsList.SelectedIndex];
                chromeBrowser.Load(fileName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void tsbtnCreateFile_Click(object sender, EventArgs e)
        {
            try
            {
                FrmCreateFile frm = new FrmCreateFile();
                frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
