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
    public partial class FormParameter : Form
    {
        public FormParameter()
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
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    configuration.AppSettings.Settings["ImgSavePath"].Value = txtImgSavePath.Text;
                    configuration.Save();
                }
            }
        }

        private void LoadConfig()
        {
            txtImgSavePath.Text = ConfigurationManager.AppSettings["ImgSavePath"];
        }
    }
}
