using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyJsCommTool
{
    public partial class FrmCreateFile : Form
    {
        public FrmCreateFile()
        {
            InitializeComponent();
        }

        private void FrmCreateFile_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\highlighting\\" + tbxFileName.Text;
                if(File.Exists(path) == false)
                {
                    MessageBox.Show(this,"文件已存在，请更换文件名。");
                    return;
                }

                File.WriteAllText(path,"");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
