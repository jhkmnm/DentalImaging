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
using DentalImaging.Help;

namespace DentalImaging
{
    public partial class FormImg : Form
    {
        ImgInfo img;
        Font font = new Font(new FontFamily("宋体"), 24, FontStyle.Regular);
        bool isShow;

        public ImgInfo Img {
            get { return img; }
        }

        public FormImg(ImgInfo img)
        {
            InitializeComponent();
            this.img = img;
            this.Text = img.Date.ToString("yyyy-MM-dd");
            pictureBox1.Image = img.Img;
            //this.Width = img.Img.Width;
            //this.Height = img.Img.Height;
        }

        private void tsmSave_Click(object sender, EventArgs e)
        {
            var form = new FormSaveImg(img);
            form.ShowDialog();
        }

        private void FormImg_Activated(object sender, EventArgs e)
        {
            User.CurrentImg = this;
        }

        public void DrawImg(bool showTitle)
        {
            isShow = showTitle;
            pictureBox1.Image = (Image)img.Img.Clone();
            if(showTitle)
                pictureBox1.Invalidate();
        }

        public void RotateImage(int angle)
        {
            ImageClass imgClass = new ImageClass();
            Img.Img = (Bitmap)imgClass.GetRotateImage(Img.Img, angle);
            pictureBox1.Width = Img.Img.Width;
            pictureBox1.Height = Img.Img.Height;
            pictureBox1.Image = Img.Img;
            pictureBox1.Invalidate();
        }

        private void Draw(Graphics g, DrawControl draw)
        {
            switch (draw.DrawType)
            {
                case DrawType.Rectangle:
                    g.DrawRectangle(draw.Pen, draw.Rect);
                    break;
                case DrawType.FillRectangle:
                    g.DrawRectangle(draw.Pen, draw.Rect);
                    g.FillRectangle(new SolidBrush(draw.FillColor), draw.Rect);
                    break;
                case DrawType.Line:
                    g.DrawLine(draw.Pen, draw.Start, draw.End);
                    break;
                case DrawType.LineCap:
                    var pen = (Pen)draw.Pen.Clone();
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    g.DrawLine(pen, draw.Start, draw.End);
                    break;
                case DrawType.Ellipse:
                    g.DrawEllipse(draw.Pen, draw.Rect);
                    break;
                case DrawType.FillEllips:
                    g.DrawEllipse(draw.Pen, draw.Rect);
                    g.FillEllipse(new SolidBrush(draw.FillColor), draw.Rect);
                    break;
                case DrawType.LeftText:
                    g.DrawString(draw.Text, font, new SolidBrush(draw.Pen.Color), new PointF(draw.Start.X, draw.Start.Y));
                    break;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (img.ImgEditInfo != null && img.ImgEditInfo.Count > 0 && isShow)
            {
                img.ImgEditInfo.ForEach(f => Draw(Graphics.FromImage(pictureBox1.Image), f));
                pictureBox1.Refresh();
            }
        }

        public void Mirror()
        {
            Bitmap curBitmap = (Bitmap)pictureBox1.Image;

            if (curBitmap != null)
            {
                //位图矩形  
                Rectangle rect = new Rectangle(0, 0, curBitmap.Width, curBitmap.Height);
                //以可读写的方式锁定全部位图像素  
                System.Drawing.Imaging.BitmapData bmpData = curBitmap.LockBits(rect,
                    System.Drawing.Imaging.ImageLockMode.ReadWrite,
                    curBitmap.PixelFormat);
                //得到首地址  
                IntPtr ptr = bmpData.Scan0;
                //24为bmp位图字节数  
                int bytes = curBitmap.Width * curBitmap.Height * 3;
                //定义位图数组  
                byte[] grayValues = new byte[bytes];
                //复制被锁定的位图像素值到该数组内  
                System.Runtime.InteropServices.Marshal.Copy(ptr, grayValues, 0, bytes);
                //水平中轴  
                int halfWidth = (curBitmap.Width * 3) / 2;
                byte temp;
                //水平镜像处理  
                for (int i = 0; i < curBitmap.Height; i++)
                {
                    for (int j = 0; j < halfWidth; j++)
                    {
                        //以水平中轴线为对称轴，两边像素值交换  
                        temp = grayValues[i * curBitmap.Width * 3 + j];
                        grayValues[i * curBitmap.Width * 3 + j] = grayValues[(i + 1) * curBitmap.Width * 3 - 1 - j];
                        grayValues[(i + 1) * curBitmap.Width * 3 - 1 - j] = temp;
                    }
                }
                System.Runtime.InteropServices.Marshal.Copy(grayValues, 0, ptr, bytes);
                curBitmap.UnlockBits(bmpData);

                pictureBox1.Invalidate();

                string dirName = $"{User.DBPath}\\{User.CurrentPatient.Number}";
                img.Img = (Bitmap)pictureBox1.Image;
                File.WriteAllText($"{dirName}/{img.FileName}.ini", img.ToJson());
            }
        }

        private void tsmPrint_Click(object sender, EventArgs e)
        {
            var form = new FormSaveImg(img, true);
            form.ShowDialog();
        }
    }
}
