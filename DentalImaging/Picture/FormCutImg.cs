using DentalImaging.Help;
using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormCutImg : Form
    {
        ImgInfo img;
        public FormCutImg(ImgInfo img)
        {
            InitializeComponent();
            this.img = img;
            pictureBox1.Image = img.Img;
            pictureBox2.Image = (Image)img.Img.Clone();
            var graphics = pictureBox1.CreateGraphics();
            graphics.DrawRectangle(new Pen(Color.White), new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));
            label1.Text = $"{img.Img.Width} X {img.Img.Height}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Image = (Image)pictureBox2.Image.Clone();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string dirName = $"{User.DBPath}\\{User.CurrentPatient.Number}";
        private void SaveImg()
        {
            img.Img = (Bitmap)pictureBox1.Image;
            img.Base64 = Base64Util.GetBase64FromImage(img.Img);
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

        Point start; //画框的起始点
        Point end;//画框的结束点<br>
        bool blnDraw;//判断是否绘制<br>
        bool isDraw = true;
        Rectangle rect;
        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            start = e.Location;
            pictureBox1.Invalidate();
            Invalidate();
            blnDraw = true;
            isDraw = true;
        }

        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnDraw)
            {
                if (e.Button != MouseButtons.Left)//判断是否按下左键
                    return;
                Point tempEndPoint = e.Location; //记录框的位置和大小
                rect.Location = new Point(
                Math.Min(start.X, tempEndPoint.X),
                Math.Min(start.Y, tempEndPoint.Y));
                rect.Size = new Size(
                    Math.Abs(start.X - tempEndPoint.X),
                    Math.Abs(start.Y - tempEndPoint.Y));
                pictureBox1.Invalidate();
                end = e.Location;
            }
        }

        private void PictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            blnDraw = false; //结束绘制
            end = e.Location;
        }

        private void imageBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null && isDraw && rect.Width > 0)
            {
                Draw(e.Graphics);
            }
        }

        private void Draw(Graphics g)
        {
            g.DrawRectangle(Pens.Cyan, rect);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rect != null && rect.Width > 0 && rect.Height > 0)
            {
                var image = MakeThumbnailImage(pictureBox1.Image, rect.Width, rect.Height, rect.Width, rect.Height, rect.X, rect.Y);
                if(image != null)
                {
                    pictureBox1.Image = image;
                    isDraw = false;
                    rect.Width = 0;
                    pictureBox1.Invalidate();
                    SaveImg();
                }                
            }                
        }

        /// <summary>
        /// 裁剪图片并保存
        /// </summary>
        /// <param name="Image">图片信息</param>
        /// <param name="maxWidth">缩略图宽度</param>
        /// <param name="maxHeight">缩略图高度</param>
        /// <param name="cropWidth">裁剪宽度</param>
        /// <param name="cropHeight">裁剪高度</param>
        /// <param name="X">X轴</param>
        /// <param name="Y">Y轴</param>
        public Bitmap MakeThumbnailImage(Image originalImage, int maxWidth, int maxHeight, int cropWidth, int cropHeight, int X, int Y)
        {
            var widthMultiple = originalImage.Width / pictureBox1.Width;
            var heightMultiple = originalImage.Height / pictureBox1.Height;
            cropWidth = maxWidth *= widthMultiple;
            cropHeight = maxHeight *= heightMultiple;
            X *= widthMultiple;
            Y *= heightMultiple;

            if(cropWidth == 0 || cropHeight == 0)
            {
                MessageBox.Show("裁剪的图片过小，请重新选择");
                return null;
            }

            Bitmap b = new Bitmap(cropWidth, cropHeight);
            try
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    //清空画布并以透明背景色填充
                    g.Clear(Color.Transparent);
                    //在指定位置并且按指定大小绘制原图片的指定部分
                    g.DrawImage(originalImage, new Rectangle(0, 0, cropWidth, cropHeight), X, Y, cropWidth, cropHeight, GraphicsUnit.Pixel);
                    Image displayImage = new Bitmap(b, maxWidth, maxHeight);
                    //displayImage.Save("E:\\cutimg.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    Bitmap bit = new Bitmap(b, maxWidth, maxHeight);
                    return bit;
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                b.Dispose();
            }
        }
    }
}
