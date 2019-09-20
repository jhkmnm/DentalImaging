using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormParameterB : Form
    {
        public FormParameterB()
        {
            InitializeComponent();
            LoadConfig();
        }

        private void btnImgSavePath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                folder.Description = "浏览文件夹";
                if(folder.ShowDialog() == DialogResult.OK)
                {
                    txtImgSavePath.Text = folder.SelectedPath;
                    //Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    //configuration.AppSettings.Settings["ImgSavePath"].Value = txtImgSavePath.Text;
                    //configuration.Save();
                }
            }
        }

        private void LoadConfig()
        {
            txtImgSavePath.Text = ConfigurationManager.AppSettings["DBPath"];
            var allOrTop = ConfigurationManager.AppSettings["AllOrTop"];
            var top = ConfigurationManager.AppSettings["Top"];
            textBox1.Text = top;
            if (allOrTop == "All")
            {
                rbtnAll.Checked = true;
                textBox1.Enabled = false;
            }
            else
            {
                rbtnTop.Checked = true;
                textBox1.Enabled = true;
            }
        }

        private void rbtnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnTop.Checked)
                textBox1.Enabled = true;
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnAll.Checked)
                textBox1.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["DBPath"].Value = txtImgSavePath.Text;
            configuration.AppSettings.Settings["AllOrTop"].Value = rbtnAll.Checked ? "All" : "Top";
            string page = textBox1.Text == "" ? "200" : textBox1.Text;
            int ipage = 200;
            configuration.AppSettings.Settings["Top"].Value = page;
            User.IsAll = rbtnAll.Checked;
            int.TryParse(page, out ipage);
            User.PageCount = ipage;
            configuration.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}
