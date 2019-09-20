using DentalImaging.Help;
using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormRotate : Form
    {
        ImgInfo img;
        public FormRotate(ImgInfo img)
        {
            InitializeComponent();
            this.img = img;
            pictureBox1.Image = img.Img;
            var graphics = pictureBox1.CreateGraphics();
            graphics.DrawRectangle(new Pen(Color.White), new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            label1.Text = $"{pictureBox1.Width}:{pictureBox1.Height}";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var angle = int.Parse(txtAngle.Text);
            ShowRotate(angle);
        }

        /// <summary>
        /// 获取旋转后的图像并显示出来
        /// </summary>
        /// <param name="Angle"></param>
        private void ShowRotate(int Angle)
        {
            this.txtAngle.Text = Angle.ToString();
            var iTool = new ImageClass();
            Image srcImage = null;
            Image rotateImage = null;
            Graphics graphics = null;
            try
            {
                srcImage = (Image)img.Img.Clone();
                //调用方法获取旋转后的图像
                rotateImage = iTool.GetRotateImage(srcImage, Angle);
                graphics = pictureBox1.CreateGraphics();
                //pictureBox1.Image = rotateImage;
                graphics.Clear(Color.Gray);
                //将旋转后的图像显示到pictureBox1里
                graphics.DrawImage(rotateImage, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
                ////是否显示图像所占矩形区域
                graphics.DrawRectangle(new Pen(Color.White), new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
                SaveImg((Bitmap)rotateImage.Clone());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (srcImage != null)
                    srcImage.Dispose();
                if (rotateImage != null)
                    rotateImage.Dispose();
                if (graphics != null)
                    graphics.Dispose();
            }
        }

        string dirName = $"{User.DBPath}\\{User.CurrentPatient.Number}";
        private void SaveImg(Bitmap bitImg)
        {
            img.Img = bitImg;
            //img.Base64 = Base64Util.GetBase64FromImage(img.Img);
            if (!checkBox1.Checked)
            {
                img.FileName = User.GetFileName;
            }
            File.WriteAllText($"{dirName}/{img.FileName}.ini", img.ToJson());
        }

        //private string GetFileName()
        //{
        //    var files = Directory.GetFiles(dirName, "*.ini");
        //    List<int> fileIndex = new List<int>();
        //    foreach (var file in files)
        //    {
        //        fileIndex.Add(Convert.ToInt32(file.Replace(dirName + "\\", "").Replace(".ini", "").Replace("S", "")));
        //    }
        //    return fileIndex.Count == 0 ? "S1" : $"S{(fileIndex.Max() + 1).ToString()}";
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = img.Img;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
