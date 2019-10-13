using DentalImaging.Help;
using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormToothMap2 : Form
    {
        private bool isEdit;
        private List<int> toothMap = new List<int>();
        public ImgInfo img;
        public FormToothMap2(bool isEdit, ImgInfo img)
        {
            InitializeComponent();
            this.isEdit = isEdit;
            this.img = img;
            Init();
            textBox1.Enabled = isEdit;
        }

        private void Init()
        {
            var dirPath = $"{System.Environment.CurrentDirectory}\\Image";
            var xstart = 14;
            var ystart = 211;
            var w = 17;
            var h = 43;
            var rowCount = 16;

            for (int i = 0; i <= 32; i++)
            {
                int x = xstart;
                int y = ystart;
                if (i > 0)
                {
                    x = xstart + (w * (i % rowCount));
                    y = ystart + (h * (i / rowCount));
                }
                var pic = new PictureBox();
                pic.Name = $"pic_{i + 1}";
                pic.Tag = "#" + (i + 1);
                pic.ImageLocation = $"{dirPath}\\{i + 1}-1.bmp";
                pic.Location = new System.Drawing.Point(x, y);
                pic.Size = new System.Drawing.Size(18, 44);
                pic.TabIndex = i + 1;
                pic.TabStop = false;
                pic.Click += Pic_Click;
                //pic.MouseEnter += Pic_MouseEnter;
                this.Controls.Add(pic);
            }
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;
            var str = pic.Tag.ToString().Replace("#", "");
            int index = 0;
            int.TryParse(str, out index);
            if (img.ToothIndex == null) img.ToothIndex = new List<int>();
            if(img.ToothIndex.Contains(index))
            {
                img.ToothIndex.Remove(index);
            }
            else
            {
                img.ToothIndex.Add(index);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isEdit)
            {
                img.Note = textBox1.Text;
                this.DialogResult = DialogResult.OK;
                SaveImg();
            }
            else
                this.DialogResult = DialogResult.No;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void SaveImg()
        {
            img.Date = DateTime.Now;
            img.FileName = User.GetFileName;
            img.Note = textBox1.Text;
            //img.Base64 = Base64Util.GetBase64FromImage(img.Img);            

            string dirName = $"{User.DBPath}\\{User.CurrentPatient.FName + User.CurrentPatient.BirthDay}";
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllText($"{dirName}/{img.FileName}.ini", img.ToJson());
            this.DialogResult = DialogResult.OK;
            var savePath = ConfigurationManager.AppSettings["ImgSavePath"];
            if (string.IsNullOrWhiteSpace(savePath))
            {
                img.Img.Save($"{dirName}/{img.FileName}.png");
            }
            else
            {
                img.Img.Save($"{savePath}/{img.FileName}.png");
            }
        }

        private void FormToothMap2_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }
    }
}
