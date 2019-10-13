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
    public partial class FormChangeSize : Form
    {
        ImgInfo img;
        public FormChangeSize(ImgInfo img)
        {
            InitializeComponent();
            this.img = img;
            pictureBox1.Image = img.Img;
            var graphics = pictureBox1.CreateGraphics();
            graphics.DrawRectangle(new Pen(Color.White), new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            label1.Text = $"{img.Img.Width} X {img.Img.Height}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = img.Img;
        }

        string dirName = $"{User.DBPath}\\{User.CurrentPatient.Number}";
        private void SaveImg()
        {
            img.Img = (Bitmap)pictureBox1.Image;
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtAngle_Enter(object sender, EventArgs e)
        {
            if(checkBox2.Checked)
            {
                int height;
                int.TryParse(textBox1.Text, out height);
                if(height > 0)
                {
                    txtAngle.Text = (img.Img.Width * height / img.Img.Height).ToString();
                }
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                int width;
                int.TryParse(txtAngle.Text, out width);
                if (width > 0)
                {
                    textBox1.Text = (img.Img.Height * width / img.Img.Width).ToString();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int width;
            int.TryParse(txtAngle.Text, out width);
            int height;
            int.TryParse(textBox1.Text, out height);
            string mode = "HW";
            if(checkBox2.Checked)
            {
                height = img.Img.Height * width / img.Img.Width;
                mode = "W";
            }
            MakeThumbnail(width, height, mode);
            SaveImg();
        }

        public void MakeThumbnail(int width, int height, string mode)
        {
            int towidth = width;
            int toheight = height;
            var originalImage = pictureBox1.Image;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            

            switch (mode)
            {
                case "HW":  //指定高宽缩放（可能变形）
                    break;
                case "W":   //指定宽，高按比例                      
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H":   //指定高，宽按比例  
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut": //指定高宽裁减（不变形）                  
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法  
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充  
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight), new System.Drawing.Rectangle(x, y, ow, oh), System.Drawing.GraphicsUnit.Pixel);
            pictureBox1.Image = bitmap;
            label1.Text = $"{bitmap.Width} X {bitmap.Height}";
        }
    }
}
