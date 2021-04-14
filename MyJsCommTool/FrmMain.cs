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

        /// <summary>
        /// 
        /// </summary>
        public ChromiumWebBrowser chromeBrowser;

        /// <summary>
        /// html文件列表
        /// </summary>
        public List<string> pathList = new List<string>();

        /// <summary>
        /// 当前打开的html文件路径
        /// </summary>
        public string curFilePath = "";

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
            chromeBrowser.JavascriptObjectRepository.Register("systemHelper", new SystemHelper(), true, BindingOptions.DefaultBinder);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                //获取安装目录下myhtml所有的html文件列表
                DirectoryInfo folder = new DirectoryInfo(Application.StartupPath + "\\myHtml\\");

                foreach (FileInfo file in folder.GetFiles("*.html"))
                {
                    Debug.WriteLine(file.Name);
                    tscbxJsList.Items.Add(file.Name);   //在下拉框中加入文件名
                    pathList.Add(file.FullName);    //在文件列表中加入全路径，方便加载打开
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
                textEditorControl1.Font = new Font("Courier New", 9);

                this.toolStrip1.Visible = false;//暂时没按钮，先隐藏
                //tsbtnOpen.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message,"出错啦",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //窗体关闭时，记得停止浏览器
                Cef.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            curFilePath = pathList[tscbxJsList.SelectedIndex];
            chromeBrowser.Load(curFilePath);

            textEditorControl1.Text = File.ReadAllText(curFilePath);

            this.Text = $"调试工具[{curFilePath}]";
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
                if (string.IsNullOrWhiteSpace(curFilePath))
                {
                    Debug.WriteLine($"请先打开文件再保存");
                    return;
                }
                File.WriteAllText(curFilePath, textEditorControl1.Text);

                Debug.WriteLine($"[{curFilePath}]已保存");

                tsbtnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                chromeBrowser.Load(curFilePath);

                Debug.WriteLine($"[{curFilePath}]已刷新");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(this, ex.Message, "出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbtnOpenFromFile_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "网页|*.html|所有文件|*.*";
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.FilterIndex = 1;
                if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
                {
                    MessageBox.Show(this, "操作取消。");
                    return;
                }

                FileInfo fileInfo = new FileInfo(this.openFileDialog1.FileName);
                if (fileInfo.Exists == false)
                {
                    MessageBox.Show(this, "文件不存在");
                    return;
                }

                int index = tscbxJsList.Items.Add(fileInfo.Name);   //在下拉框中加入文件名
                pathList.Add(fileInfo.FullName);    //在文件列表中加入全路径，方便加载打开
                tscbxJsList.SelectedIndex = index;

                curFilePath = fileInfo.FullName;
                chromeBrowser.Load(curFilePath);

                textEditorControl1.Text = File.ReadAllText(curFilePath);

                this.Text = $"调试工具[{curFilePath}]";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbtnSetFont_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = textEditorControl1.Font;
            fontDialog1.ShowColor = false;
            fontDialog1.ShowDialog();//此方法用于弹出字体对话框

            textEditorControl1.Font = fontDialog1.Font;
        }

        private void FrmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12)
            {
                tsmiOpenDevTools.PerformClick();

            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                tsbtnSave.PerformClick();
            }
        }
    }
}
