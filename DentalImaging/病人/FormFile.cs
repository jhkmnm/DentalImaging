using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DentalImaging.Model;
using System.IO;

namespace DentalImaging
{
    public partial class FormFile : Form
    {
        public FormFile()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "浏览文件夹";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                textBox1.Text = dialog.SelectedPath;
            }
        }

        string dirName = User.DBPath;//$"Patient";

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show(this, "存档路径无效", "提示");
                return;
            }
            listDirectory(dirName);
            if(imgInfos.Count() == 0)
            {
                MessageBox.Show("存档完成:没有图像存档");
                return;
            }

            var dir = "图片存档";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(textBox1.Text + "\\" + dir);
            }
            foreach (var img in imgInfos)
            {
                if(!Directory.Exists(textBox1.Text + "\\" + dir + "\\" + img.Key))
                {
                    Directory.CreateDirectory(textBox1.Text + "\\" + dir + "\\" + img.Key);
                }
                foreach(var i in img.Value)
                {
                    i.Img.Save($"{textBox1.Text + "\\" + dir + "\\" + img.Key}\\{i.FileName}.png");
                    i.IsSaveToFile = true;
                    File.WriteAllText($"{dirName}\\{img.Key}\\{i.FileName}.ini", i.ToJson());
                }
            }
            MessageBox.Show("存档完成");
        }

        Dictionary<string, List<ImgInfo>> imgInfos = new Dictionary<string, List<ImgInfo>>();
        /// <summary>
        /// 列出path路径对应的文件夹中的子文件夹和文件
        /// 然后再递归列出子文件夹内的文件和文件夹
        /// </summary>
        /// <param name="path">需要列出内容的文件夹的路径</param>
        /// <param name="leval">当前递归层级，用于控制输出前导空格的数量</param>
        private void listDirectory(string path)
        {
            DirectoryInfo theFolder = new DirectoryInfo(path);

            List<ImgInfo> infos = null;
            //遍历文件
            foreach (var file in Directory.GetFiles(path, "*.ini"))
            {
                if (file.Contains("us.ini"))
                    continue;

                if(infos == null)
                    infos = new List<ImgInfo>();

                var str = File.ReadAllText(file);
                var img = str.ToObject<ImgInfo>();
                if(!img.IsSaveToFile)
                {
                    //img.Img = Help.Base64Util.GetImageFromBase64(img.Base64);
                    infos.Add(img);
                }
            }
            if(infos != null && infos.Count > 0)
            {
                imgInfos.Add(theFolder.Name, infos);
            }

            //遍历文件夹
            foreach (DirectoryInfo NextFolder in theFolder.GetDirectories())
            {
                listDirectory(NextFolder.FullName);
            }
        }
    }
}
