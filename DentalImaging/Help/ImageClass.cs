using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace DentalImaging.Help
{
    public class ImageClass
    {
        private static readonly double sizePerPx = 0.18;

        public ImageClass()
        { }

        #region 缩略图  
        /// <summary>  
        /// 生成缩略图  
        /// </summary>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>  
        /// <param name="width">缩略图宽度</param>  
        /// <param name="height">缩略图高度</param>  
        /// <param name="mode">生成缩略图的方式</param>      
        public static Image MakeThumbnail(Image originalImage, int width, int height, string mode)
        {
            int towidth = width;
            int toheight = height;

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

            originalImage.Dispose();
            bitmap.Dispose();
            g.Dispose();
            return originalImage;
        }
        #endregion

        #region 图片水印  
        /// <summary>  
        /// 图片水印处理方法  
        /// </summary>  
        /// <param name="path">需要加载水印的图片路径（绝对路径）</param>  
        /// <param name="waterpath">水印图片（绝对路径）</param>  
        /// <param name="location">水印位置（传送正确的代码）</param>  
        public static string ImageWatermark(string path, string waterpath, string location)
        {
            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Image waterimg = Image.FromFile(waterpath);
                Graphics g = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, waterimg);
                g.DrawImage(waterimg, new Rectangle(int.Parse(loca[0].ToString()), int.Parse(loca[1].ToString()), waterimg.Width, waterimg.Height));
                waterimg.Dispose();
                g.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;
        }

        /// <summary>  
        /// 图片水印位置处理方法  
        /// </summary>  
        /// <param name="location">水印位置</param>  
        /// <param name="img">需要添加水印的图片</param>  
        /// <param name="waterimg">水印图片</param>  
        private static ArrayList GetLocation(string location, Image img, Image waterimg)
        {
            ArrayList loca = new ArrayList();
            int x = 0;
            int y = 0;

            if (location == "LT")
            {
                x = 10;
                y = 10;
            }
            else if (location == "T")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else if (location == "RT")
            {
                x = img.Width - waterimg.Width;
                y = 10;
            }
            else if (location == "LC")
            {
                x = 10;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - waterimg.Width;
                y = img.Height / 2 - waterimg.Height / 2;
            }
            else if (location == "LB")
            {
                x = 10;
                y = img.Height - waterimg.Height;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - waterimg.Width / 2;
                y = img.Height - waterimg.Height;
            }
            else
            {
                x = img.Width - waterimg.Width;
                y = img.Height - waterimg.Height;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;
        }
        #endregion

        #region 文字水印  
        /// <summary>  
        /// 文字水印处理方法  
        /// </summary>  
        /// <param name="path">图片路径（绝对路径）</param>  
        /// <param name="size">字体大小</param>  
        /// <param name="letter">水印文字</param>  
        /// <param name="color">颜色</param>  
        /// <param name="location">水印位置</param>  
        public static string LetterWatermark(string path, int size, string letter, Color color, string location)
        {
            #region  

            string kz_name = Path.GetExtension(path);
            if (kz_name == ".jpg" || kz_name == ".bmp" || kz_name == ".jpeg")
            {
                DateTime time = DateTime.Now;
                string filename = "" + time.Year.ToString() + time.Month.ToString() + time.Day.ToString() + time.Hour.ToString() + time.Minute.ToString() + time.Second.ToString() + time.Millisecond.ToString();
                Image img = Bitmap.FromFile(path);
                Graphics gs = Graphics.FromImage(img);
                ArrayList loca = GetLocation(location, img, size, letter.Length);
                Font font = new Font("宋体", size);
                Brush br = new SolidBrush(color);
                gs.DrawString(letter, font, br, float.Parse(loca[0].ToString()), float.Parse(loca[1].ToString()));
                gs.Dispose();
                string newpath = Path.GetDirectoryName(path) + filename + kz_name;
                img.Save(newpath);
                img.Dispose();
                File.Copy(newpath, path, true);
                if (File.Exists(newpath))
                {
                    File.Delete(newpath);
                }
            }
            return path;

            #endregion
        }

        /// <summary>  
        /// 文字水印位置的方法  
        /// </summary>  
        /// <param name="location">位置代码</param>  
        /// <param name="img">图片对象</param>  
        /// <param name="width">宽(当水印类型为文字时,传过来的就是字体的大小)</param>  
        /// <param name="height">高(当水印类型为文字时,传过来的就是字符的长度)</param>  
        private static ArrayList GetLocation(string location, Image img, int width, int height)
        {
            #region  

            ArrayList loca = new ArrayList();  //定义数组存储位置  
            float x = 10;
            float y = 10;

            if (location == "LT")
            {
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "T")
            {
                x = img.Width / 2 - (width * height) / 2;
                loca.Add(x);
                loca.Add(y);
            }
            else if (location == "RT")
            {
                x = img.Width - width * height;
            }
            else if (location == "LC")
            {
                y = img.Height / 2;
            }
            else if (location == "C")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height / 2;
            }
            else if (location == "RC")
            {
                x = img.Width - height;
                y = img.Height / 2;
            }
            else if (location == "LB")
            {
                y = img.Height - width - 5;
            }
            else if (location == "B")
            {
                x = img.Width / 2 - (width * height) / 2;
                y = img.Height - width - 5;
            }
            else
            {
                x = img.Width - width * height;
                y = img.Height - width - 5;
            }
            loca.Add(x);
            loca.Add(y);
            return loca;

            #endregion
        }
        #endregion

        #region 调整光暗  
        /// <summary>  
        /// 调整光暗  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        /// <param name="val">增加或减少的光暗值</param>  
        public Bitmap LDPic(Bitmap mybm, int width, int height, int val)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录经过处理后的图片对象  
            int x, y, resultR, resultG, resultB;//x、y是循环次数，后面三个是记录红绿蓝三个值的  
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值  
                    resultR = pixel.R + val;//检查红色值会不会超出[0, 255]  
                    resultG = pixel.G + val;//检查绿色值会不会超出[0, 255]  
                    resultB = pixel.B + val;//检查蓝色值会不会超出[0, 255]  
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//绘图  
                }
            }
            return bm;
        }
        #endregion

        /// <summary>
        /// 图像明暗调整
        /// </summary>
        /// <param name="b">原始图</param>
        /// <param name="degree">亮度[-255, 255]</param>
        /// <returns></returns>
        public static Bitmap KiLighten(Bitmap b, int degree)
        {
            if (b == null)
            {
                return null;
            }

            if (degree < -255) degree = -255;
            if (degree > 255) degree = 255;

            try
            {

                int width = b.Width;
                int height = b.Height;

                int pix = 0;

                BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                unsafe
                {
                    byte* p = (byte*)data.Scan0;
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // 处理指定位置像素的亮度
                            for (int i = 0; i < 3; i++)
                            {
                                pix = p[i] + degree;

                                if (degree < 0) p[i] = (byte)Math.Max(0, pix);
                                if (degree > 0) p[i] = (byte)Math.Min(255, pix);

                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }

                b.UnlockBits(data);

                return b;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 图像对比度调整
        /// </summary>
        /// <param name="b">原始图</param>
        /// <param name="degree">对比度[-100, 100]</param>
        /// <returns></returns>
        public static Bitmap KiContrast(Bitmap b, int degree)
        {
            if (b == null)
            {
                return null;
            }

            if (degree < -100) degree = -100;
            if (degree > 100) degree = 100;

            try
            {

                double pixel = 0;
                double contrast = (100.0 + degree) / 100.0;
                contrast *= contrast;
                int width = b.Width;
                int height = b.Height;
                BitmapData data = b.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                unsafe
                {
                    byte* p = (byte*)data.Scan0;
                    int offset = data.Stride - width * 3;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            // 处理指定位置像素的对比度
                            for (int i = 0; i < 3; i++)
                            {
                                pixel = ((p[i] / 255.0 - 0.5) * contrast + 0.5) * 255;
                                if (pixel < 0) pixel = 0;
                                if (pixel > 255) pixel = 255;
                                p[i] = (byte)pixel;
                            } // i
                            p += 3;
                        } // x
                        p += offset;
                    } // y
                }
                b.UnlockBits(data);
                return b;
            }
            catch
            {
                return null;
            }
        }

        #region 反色处理  
        /// <summary>  
        /// 反色处理  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        public Bitmap RePic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录处理后的图片的对象  
            int x, y, resultR, resultG, resultB;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值  
                    resultR = 255 - pixel.R;//反红  
                    resultG = 255 - pixel.G;//反绿  
                    resultB = 255 - pixel.B;//反蓝  
                    bm.SetPixel(x, y, Color.FromArgb(resultR, resultG, resultB));//绘图  
                }
            }
            return bm;
        }
        #endregion

        #region 浮雕处理  
        /// <summary>  
        /// 浮雕处理  
        /// </summary>  
        /// <param name="oldBitmap">原始图片</param>  
        /// <param name="Width">原始图片的长度</param>  
        /// <param name="Height">原始图片的高度</param>  
        public Bitmap FD(Bitmap oldBitmap, int Width, int Height)
        {
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color color1, color2;
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    color1 = oldBitmap.GetPixel(x, y);
                    color2 = oldBitmap.GetPixel(x + 1, y + 1);
                    r = Math.Abs(color1.R - color2.R + 128);
                    g = Math.Abs(color1.G - color2.G + 128);
                    b = Math.Abs(color1.B - color2.B + 128);
                    if (r > 255) r = 255;
                    if (r < 0) r = 0;
                    if (g > 255) g = 255;
                    if (g < 0) g = 0;
                    if (b > 255) b = 255;
                    if (b < 0) b = 0;
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            return newBitmap;
        }
        #endregion

        #region 拉伸图片  
        /// <summary>  
        /// 拉伸图片  
        /// </summary>  
        /// <param name="bmp">原始图片</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        public static Bitmap ResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap bap = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(bap);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(bap, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bap.Width, bap.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return bap;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 滤色处理  
        /// <summary>  
        /// 滤色处理  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        public Bitmap FilPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);//初始化一个记录滤色效果的图片对象  
            int x, y;
            Color pixel;

            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值  
                    bm.SetPixel(x, y, Color.FromArgb(0, pixel.G, pixel.B));//绘图  
                }
            }
            return bm;
        }
        #endregion

        #region 左右翻转  
        /// <summary>  
        /// 左右翻转  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        public Bitmap RevPicLR(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z; //x,y是循环次数,z是用来记录像素点的x坐标的变化的  
            Color pixel;
            for (y = height - 1; y >= 0; y--)
            {
                for (x = width - 1, z = 0; x >= 0; x--)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值  
                    bm.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图  
                }
            }
            return bm;
        }
        #endregion

        /// <summary>
        /// 获取原图像绕中心任意角度旋转后的图像
        /// </summary>
        /// <param name="rawImg"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Image GetRotateImage(Image srcImage, int angle)
        {
            angle = angle % 360;
            //原图的宽和高
            int srcWidth = srcImage.Width;
            int srcHeight = srcImage.Height;
            //图像旋转之后所占区域宽和高
            Rectangle rotateRec = GetRotateRectangle(srcWidth, srcHeight, angle);
            int rotateWidth = rotateRec.Width;
            int rotateHeight = rotateRec.Height;
            //目标位图
            Bitmap destImage = null;
            Graphics graphics = null;
            try
            {
                //定义画布，宽高为图像旋转后的宽高
                destImage = new Bitmap(rotateWidth, rotateHeight);
                //graphics根据destImage创建，因此其原点此时在destImage左上角
                graphics = Graphics.FromImage(destImage);
                //要让graphics围绕某矩形中心点旋转N度，分三步
                //第一步，将graphics坐标原点移到矩形中心点,假设其中点坐标（x,y）
                //第二步，graphics旋转相应的角度(沿当前原点)
                //第三步，移回（-x,-y）
                //获取画布中心点
                Point centerPoint = new Point(rotateWidth / 2, rotateHeight / 2);
                //将graphics坐标原点移到中心点
                graphics.TranslateTransform(centerPoint.X, centerPoint.Y);
                //graphics旋转相应的角度(绕当前原点)
                graphics.RotateTransform(angle);
                //恢复graphics在水平和垂直方向的平移(沿当前原点)
                graphics.TranslateTransform(-centerPoint.X, -centerPoint.Y);
                //此时已经完成了graphics的旋转

                //计算:如果要将源图像画到画布上且中心与画布中心重合，需要的偏移量
                Point Offset = new Point((rotateWidth - srcWidth) / 2, (rotateHeight - srcHeight) / 2);
                //将源图片画到rect里（rotateRec的中心）
                graphics.DrawImage(srcImage, new Rectangle(Offset.X, Offset.Y, srcWidth, srcHeight));
                //重至绘图的所有变换
                graphics.ResetTransform();
                graphics.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (graphics != null)
                    graphics.Dispose();
            }
            return destImage;
        }

        /// <summary>
        /// 计算矩形绕中心任意角度旋转后所占区域矩形宽高
        /// </summary>
        /// <param name="width">原矩形的宽</param>
        /// <param name="height">原矩形高</param>
        /// <param name="angle">顺时针旋转角度</param>
        /// <returns></returns>
        public Rectangle GetRotateRectangle(int width, int height, float angle)
        {
            double radian = angle * Math.PI / 180;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //只需要考虑到第四象限和第三象限的情况取大值(中间用绝对值就可以包括第一和第二象限)
            int newWidth = (int)(Math.Max(Math.Abs(width * cos - height * sin), Math.Abs(width * cos + height * sin)));
            int newHeight = (int)(Math.Max(Math.Abs(width * sin - height * cos), Math.Abs(width * sin + height * cos)));
            return new Rectangle(0, 0, newWidth, newHeight);
        }

        #region 上下翻转  
        /// <summary>  
        /// 上下翻转  
        /// </summary>  
        /// <param name="mybm">原始图片</param>  
        /// <param name="width">原始图片的长度</param>  
        /// <param name="height">原始图片的高度</param>  
        public Bitmap RevPicUD(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, z;
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = height - 1, z = 0; y >= 0; y--)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前像素的值  
                    bm.SetPixel(x, z++, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图  
                }
            }
            return bm;
        }
        #endregion

        #region 压缩图片  
        /// <summary>  
        /// 压缩到指定尺寸  
        /// </summary>  
        /// <param name="oldfile">原文件</param>  
        /// <param name="newfile">新文件</param>  
        public bool Compress(string oldfile, string newfile)
        {
            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(oldfile);
                System.Drawing.Imaging.ImageFormat thisFormat = img.RawFormat;
                Size newSize = new Size(100, 125);
                Bitmap outBmp = new Bitmap(newSize.Width, newSize.Height);
                Graphics g = Graphics.FromImage(outBmp);
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
                g.Dispose();
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICI = null;
                for (int x = 0; x < arrayICI.Length; x++)
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICI = arrayICI[x]; //设置JPEG编码  
                        break;
                    }
                img.Dispose();
                if (jpegICI != null) outBmp.Save(newfile, System.Drawing.Imaging.ImageFormat.Jpeg);
                outBmp.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 图片灰度化  
        public Color Gray(Color c)
        {
            int rgb = Convert.ToInt32((double)(((0.3 * c.R) + (0.59 * c.G)) + (0.11 * c.B)));
            return Color.FromArgb(rgb, rgb, rgb);
        }
        #endregion

        #region 转换为黑白图片  
        /// <summary>  
        /// 转换为黑白图片  
        /// </summary>  
        /// <param name="mybt">要进行处理的图片</param>  
        /// <param name="width">图片的长度</param>  
        /// <param name="height">图片的高度</param>  
        public Bitmap BWPic(Bitmap mybm, int width, int height)
        {
            Bitmap bm = new Bitmap(width, height);
            int x, y, result; //x,y是循环次数，result是记录处理后的像素值  
            Color pixel;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    pixel = mybm.GetPixel(x, y);//获取当前坐标的像素值  
                    result = (pixel.R + pixel.G + pixel.B) / 3;//取红绿蓝三色的平均值  
                    bm.SetPixel(x, y, Color.FromArgb(result, result, result));
                }
            }
            return bm;
        }
        #endregion

        #region 获取图片中的各帧  
        /// <summary>  
        /// 获取图片中的各帧  
        /// </summary>  
        /// <param name="pPath">图片路径</param>  
        /// <param name="pSavePath">保存路径</param>  
        public void GetFrames(string pPath, string pSavedPath)
        {
            Image gif = Image.FromFile(pPath);
            FrameDimension fd = new FrameDimension(gif.FrameDimensionsList[0]);
            int count = gif.GetFrameCount(fd); //获取帧数(gif图片可能包含多帧，其它格式图片一般仅一帧)  
            for (int i = 0; i < count; i++)    //以Jpeg格式保存各帧  
            {
                gif.SelectActiveFrame(fd, i);
                gif.Save(pSavedPath + "\\frame_" + i + ".jpg", ImageFormat.Jpeg);
            }
        }
        #endregion

        #region 生成高质量缩略图、优化图片      
        /// <summary>
        /// 生成高质量缩略图（固定宽高），不一定保持原宽高比
        /// </summary>
        /// <param name="destPath">目标保存路径</param>
        /// <param name="srcPath">源文件路径</param>
        /// <param name="width">生成缩略图的宽度，设置为0，则与源图比处理</param>
        /// <param name="height">生成缩略图的高度，设置为0，则与源图等比例处理</param>
        /// <param name="quality">1~100整数,无效值则取默认值95</param>
        /// <param name="mimeType">如 image/jpeg</param>    
        public Image GetThumbnailImage(Image srcImg, int destWidth, int destHeight, int quality, out string error, string mimeType = "image/jpeg")
        {
            error = string.Empty;
            //宽高不能小于0
            if (destWidth < 0 || destHeight < 0)
            {
                error = "目标宽高不能小于0";
                return null;
            }
            //宽高不能同时为0
            if (destWidth == 0 && destHeight == 0)
            {
                error = "目标宽高不能同时为0";
                return null;
            }
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            try
            {
                //获取源图像
                srcImage = srcImg;
                //计算高宽比例
                float d = (float)srcImage.Height / srcImage.Width;
                //如果输入的宽为0，则按高度等比缩放
                if (destWidth == 0)
                {
                    destWidth = Convert.ToInt32(destHeight / d);
                }
                //如果输入的高为0，则按宽度等比缩放
                if (destHeight == 0)
                {
                    destHeight = Convert.ToInt32(destWidth * d);
                }
                //定义画布
                destImage = new Bitmap(destWidth, destHeight);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);                
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (srcImage != null)
                    srcImage.Dispose();
                if (destImage != null)
                    destImage.Dispose();
                if (graphics != null)
                    graphics.Dispose();
            }
            return destImage;
        }
        /// <summary>
        /// 对图片进行压缩优化（限制宽高），始终保持原宽高比
        /// </summary>
        /// <param name="destPath">目标保存路径</param>
        /// <param name="srcPath">源文件路径</param>
        /// <param name="max_Width">压缩后的图片宽度不大于这值，如果为0，表示不限制宽度</param>
        /// <param name="max_Height">压缩后的图片高度不大于这值，如果为0，表示不限制高度</param>
        /// <param name="quality">1~100整数,无效值则取默认值95</param>
        /// <param name="mimeType">如 image/jpeg</param>
        public bool GetCompressImage(string destPath, string srcPath, int maxWidth, int maxHeight, int quality, out string error, string mimeType = "image/jpeg")
        {
            bool retVal = false;
            error = string.Empty;
            //宽高不能小于0
            if (maxWidth < 0 || maxHeight < 0)
            {
                error = "目标宽高不能小于0";
                return retVal;
            }
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            try
            {
                //获取源图像
                srcImage = Image.FromFile(srcPath, false);
                FileInfo fileInfo = new FileInfo(srcPath);
                //目标宽度
                var destWidth = srcImage.Width;
                //目标高度
                var destHeight = srcImage.Height;
                //如果输入的最大宽为0，则不限制宽度
                //如果不为0，且原图宽度大于该值，则附值为最大宽度
                if (maxWidth != 0 && destWidth > maxWidth)
                {
                    destWidth = maxWidth;
                }
                //如果输入的最大宽为0，则不限制宽度
                //如果不为0，且原图高度大于该值，则附值为最大高度
                if (maxHeight != 0 && destHeight > maxHeight)
                {
                    destHeight = maxHeight;
                }
                float srcD = (float)srcImage.Height / srcImage.Width;
                float destD = (float)destHeight / destWidth;
                //目的高宽比 大于 原高宽比 即目的高偏大,因此按照原比例计算目的高度  
                if (destD > srcD)
                {
                    destHeight = Convert.ToInt32(destWidth * srcD);
                }
                else if (destD < srcD) //目的高宽比 小于 原高宽比 即目的宽偏大,因此按照原比例计算目的宽度  
                {
                    destWidth = Convert.ToInt32(destHeight / srcD);
                }
                //如果维持原宽高，则判断是否需要优化
                if (destWidth == srcImage.Width && destHeight == srcImage.Height &&
                    fileInfo.Length < destWidth * destHeight * sizePerPx)
                {
                    error = "图片不需要压缩优化";
                    return retVal;
                }
                //定义画布
                destImage = new Bitmap(destWidth, destHeight);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //如果是覆盖则先释放源资源
                if (destPath == srcPath)
                {
                    srcImage.Dispose();
                }
                //保存到文件，同时进一步控制质量
                //SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;

            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (srcImage != null)
                    srcImage.Dispose();
                if (destImage != null)
                    destImage.Dispose();
                if (graphics != null)
                    graphics.Dispose();
            }
            return retVal;
        }
        /// <summary>
        /// 对图片进行压缩优化，始终保持原宽高比，限制长边长度，常用场景：相片
        /// </summary>
        /// <param name="destPath">目标保存路径</param>
        /// <param name="srcPath">源文件路径</param>
        /// <param name="max_Length">压缩后的图片边（宽或者高）长变不大于这值，为0表示不限制</param>  
        /// <param name="quality">1~100整数,无效值，则取默认值95</param>
        /// <param name="mimeType">如 image/jpeg</param>
        public bool GetCompressImage(string destPath, string srcPath, int maxLength, int quality, out string error, string mimeType = "image/jpeg")
        {
            bool retVal = false;
            error = string.Empty;
            //最大边长不能小于0
            if (maxLength < 0)
            {
                error = "最大边长不能小于0";
                return retVal;
            }
            Image srcImage = null;
            Image destImage = null;
            Graphics graphics = null;
            try
            {
                //获取源图像
                srcImage = Image.FromFile(srcPath, false);
                FileInfo fileInfo = new FileInfo(srcPath);
                //目标宽度
                var destWidth = srcImage.Width;
                //目标高度
                var destHeight = srcImage.Height;
                //如果限制
                if (maxLength > 0)
                {
                    //原高宽比
                    float srcD = (float)srcImage.Height / srcImage.Width;
                    //如果宽>高，且大于 限制
                    if (destWidth > destHeight && destWidth > maxLength)
                    {
                        destWidth = maxLength;
                        destHeight = Convert.ToInt32(destWidth * srcD);
                    }
                    else
                    {
                        if (destHeight > maxLength)
                        {
                            destHeight = maxLength;
                            destWidth = Convert.ToInt32(destHeight / srcD);
                        }
                    }
                }
                //如果维持原宽高，则判断是否需要优化
                if (destWidth == srcImage.Width && destHeight == srcImage.Height &&
                    fileInfo.Length < destWidth * destHeight * sizePerPx)
                {
                    error = "图片不需要压缩优化";
                    return retVal;
                }
                //定义画布
                destImage = new Bitmap(destWidth, destHeight);
                //获取高清Graphics
                graphics = GetGraphics(destImage);
                //将源图像画到画布上，注意最后一个参数GraphicsUnit.Pixel
                graphics.DrawImage(srcImage, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                //如果是覆盖则先释放源资源
                if (destPath == srcPath)
                {
                    srcImage.Dispose();
                }
                //保存到文件，同时进一步控制质量
                //SaveImage2File(destPath, destImage, quality, mimeType);
                retVal = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (srcImage != null)
                    srcImage.Dispose();
                if (destImage != null)
                    destImage.Dispose();
                if (graphics != null)
                    graphics.Dispose();
            }
            return retVal;
        }

        /// <summary>
        /// 获取高清的Graphics
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public Graphics GetGraphics(Image img)
        {
            var g = Graphics.FromImage(img);
            //设置质量
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            //InterpolationMode不能使用High或者HighQualityBicubic,如果是灰色或者部分浅色的图像是会在边缘处出一白色透明的线
            //用HighQualityBilinear却会使图片比其他两种模式模糊（需要肉眼仔细对比才可以看出）
            g.InterpolationMode = InterpolationMode.Default;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            return g;
        }
        #endregion
    }
}
