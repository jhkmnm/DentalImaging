using DentalImaging.Help;
using DentalImaging.Model;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DentalImaging.新界面
{
    public partial class FormImageEdit : XtraForm
    {
        private ImgInfo imgInfo;
        private CaseHistory currHistory;
        private string historyNo;
        private Point location = new Point(12, 538);
        private ActionType CurrAction = ActionType.None;
        private Bitmap oldImg;
        private ImgInfo copyInfo;
        string dirName = "";
        float rate = 1.7f;
        //全局热键
        //private RegisterHotKeyHelper hotKey2 = new RegisterHotKeyHelper();

        /// <summary>
        /// 设置Alt+S的显示/隐藏窗体全局热键
        /// </summary>
        //private void SetHotKey()
        //{
        //    try
        //    {
        //        hotKey2.Keys = Keys.Delete;
        //        hotKey2.WindowHandle = this.Handle;
        //        hotKey2.WParam = 10003;
        //        hotKey2.HotKey += new RegisterHotKeyHelper.HotKeyPass(hotKey2_HotKey);
        //        hotKey2.StarHotKey();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //void hotKey2_HotKey()
        //{
        //    DeleteCurrentImg();
        //}

        public ImgInfo Img
        {
            get { return imgInfo; }
        }

        public FormImageEdit(ImgInfo imgInfo, CaseHistory history)
        {
            InitializeComponent();
            currHistory = history;
            dirName = $"{User.DBPath}\\{User.CurrentPatient.FName + User.CurrentPatient.BirthDay}\\{currHistory.HistoryNo}";
            this.imgInfo = imgInfo;
            Image img = Image.FromFile(imgInfo.ImgPath);
            this.imgInfo.Img = (Bitmap)img;
            this.oldImg = (Bitmap)img.Clone();
            currHistory = history;
            ShowPanel();
            InitDDL();
            pictureBox1.Image = img;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(3, 153);
            this.Text = $"图片编辑-{imgInfo.FileName}";
        }

        #region 图片旋转
        private void btnRotate_Click(object sender, EventArgs e)
        {
            CurrAction = ActionType.Rotate;
            ShowPanel();
        }

        private void btnRotate180_Click(object sender, EventArgs e)
        {
            RotateImage(180);
            SaveImg(false);
        }

        private void btnRotate90_Click(object sender, EventArgs e)
        {
            RotateImage(90);
            SaveImg(false);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var angle = int.Parse(txtAngle.Text);
            RotateImage(angle);
            SaveImg(!chkRotate.Checked);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = oldImg;
            imgInfo.Img = (Bitmap)oldImg.Clone();
            SaveImg();
            if (copyInfo != null)
            {
                currHistory.imgInfos.RemoveAt(currHistory.imgInfos.Count - 1);
                var filePath = $"{dirName}//{copyInfo.FileName}.ini";
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CurrAction = ActionType.None;
            ShowPanel();
        }

        public void RotateImage(int angle)
        {
            ImageClass imgClass = new ImageClass();
            Img.Img = (Bitmap)imgClass.GetRotateImage(imgInfo.Img, angle);
            pictureBox1.Image = Img.Img;
            pictureBox1.Invalidate();
        }
        #endregion

        #region 镜像
        private void btnMirror_Click(object sender, EventArgs e)
        {
            CurrAction = ActionType.None;
            ShowPanel();
            Mirror();
        }
        public void Mirror()
        {
            Bitmap _bmp = (Bitmap)pictureBox1.Image;

            Rectangle rect = new Rectangle(0, 0, _bmp.Width, _bmp.Height);
            BitmapData bmpdata = _bmp.LockBits(rect, ImageLockMode.ReadWrite, _bmp.PixelFormat);
            IntPtr ptr = bmpdata.Scan0;
            int bytes = 0;
            if (_bmp.PixelFormat == PixelFormat.Format8bppIndexed)//判断是灰度色图像还是彩色图像，给相应的大小
            {
                bytes = _bmp.Width * _bmp.Height;
            }
            else if (_bmp.PixelFormat == PixelFormat.Format24bppRgb)
            {
                bytes = _bmp.Width * _bmp.Height * 3;
            }

            byte[] pixelValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixelValues, 0, bytes);//内存法，从内存中将像素复制到pixelValues数组

            int halfWidth = _bmp.Width / 2;
            int halfheigth = _bmp.Height / 2;
            byte temp;
            byte temp1;
            byte temp2;
            //灰度图像
            if (_bmp.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                for (int r = 0; r < _bmp.Height; r++)
                    for (int c = 0; c < halfWidth; c++)
                    {
                        temp = pixelValues[r * _bmp.Width + c];
                        pixelValues[r * _bmp.Width + c] = pixelValues[(r + 1) * _bmp.Width - c - 1];
                        pixelValues[(r + 1) * _bmp.Width - c - 1] = temp;
                    }
            }
            else if (_bmp.PixelFormat == PixelFormat.Format24bppRgb)//彩色图像
            {
                for (int r = 0; r < _bmp.Height; r++)
                    for (int c = 0; c < halfWidth; c++)
                    {
                        temp = pixelValues[0 + r * _bmp.Width * 3 + c * 3];
                        temp1 = pixelValues[1 + r * _bmp.Width * 3 + c * 3];
                        temp2 = pixelValues[2 + r * _bmp.Width * 3 + c * 3];
                        pixelValues[0 + r * _bmp.Width * 3 + c * 3] = pixelValues[0 + (r + 1) * _bmp.Width * 3 - (c + 1) * 3];
                        pixelValues[1 + r * _bmp.Width * 3 + c * 3] = pixelValues[1 + (r + 1) * _bmp.Width * 3 - (c + 1) * 3];
                        pixelValues[2 + r * _bmp.Width * 3 + c * 3] = pixelValues[2 + (r + 1) * _bmp.Width * 3 - (c + 1) * 3];
                        pixelValues[0 + (r + 1) * _bmp.Width * 3 - (c + 1) * 3] = temp;
                        pixelValues[1 + (r + 1) * _bmp.Width * 3 - (c + 1) * 3] = temp1;
                        pixelValues[2 + (r + 1) * _bmp.Width * 3 - (c + 1) * 3] = temp2;
                    }
            }

            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, ptr, bytes);
            _bmp.UnlockBits(bmpdata);

            pictureBox1.Image = _bmp;

            SaveImg();            
        }
        #endregion

        #region 编辑备注
        private void btnEditRemark_Click(object sender, EventArgs e)
        {
            CurrAction = ActionType.Remark;
            ShowPanel();
            CurrDrawType = DrawType.Line;
            CurrPen = new Pen(btnColor.BackColor, 1);

            pictureBox1.MouseDown += PictureBox1_MouseDown;
            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseUp += PictureBox1_MouseUp;
            pictureBox1.Paint += imageBox1_Paint;
            this.KeyPress += FormEditPicture_KeyPress;
        }

        Point start; //画框的起始点
        Point end;//画框的结束点<br>
        bool blnDraw;//判断是否绘制<br>
        Rectangle rect;
        DrawType CurrDrawType;
        Pen CurrPen;
        private List<DrawControl> Rectangles = new List<DrawControl>();
        List<string> strText = new List<string>();
        Font font = new Font(new FontFamily("宋体"), 24, FontStyle.Regular);
        private List<DrawControl> OldRectangles = new List<DrawControl>();
        int currentyIndex = 0;
        bool isDraw = true;

        private void InitDDL()
        {
            var lineSource = new List<TextValue>();
            lineSource.AddRange(new[] {
                new TextValue { Text = "直线", Value = $"{(int)DrawType.Line}" },
                new TextValue { Text = "箭头", Value = $"{(int)DrawType.LineCap}"},
                new TextValue { Text = "曲线", Value = $"{(int)DrawType.Curve}" } });
            ddlLine.DataSource = lineSource;
            ddlLine.DisplayMember = "Text";
            ddlLine.ValueMember = "Value";
            ddlLine.SelectedIndex = 0;

            var ellipseSource = new List<TextValue>();
            ellipseSource.AddRange(new[] {
                new TextValue { Text = "空心圆", Value = $"{(int)DrawType.Ellipse}" },
                new TextValue { Text = "实心圆", Value = $"{(int)DrawType.FillEllips}"}});
            ddlEllipse.DataSource = ellipseSource;
            ddlEllipse.DisplayMember = "Text";
            ddlEllipse.ValueMember = "Value";
            ddlEllipse.SelectedIndex = 0;

            var rectangleSource = new List<TextValue>();
            rectangleSource.AddRange(new[] {
                new TextValue { Text = "空心矩形", Value = $"{(int)DrawType.Rectangle}" },
                new TextValue { Text = "实心矩形", Value = $"{(int)DrawType.FillRectangle}"} });
            ddlRectangle.DataSource = rectangleSource;
            ddlRectangle.DisplayMember = "Text";
            ddlRectangle.ValueMember = "Value";
            ddlRectangle.SelectedIndex = 0;

            var textSource = new List<TextValue>();
            textSource.AddRange(new[] {
                new TextValue { Text = "左对齐", Value = $"{(int)DrawType.LeftText}" },
                new TextValue { Text = "右对齐", Value = $"{(int)DrawType.RightText}"}});
            ddlText.DataSource = textSource;
            ddlText.DisplayMember = "Text";
            ddlText.ValueMember = "Value";
            ddlText.SelectedIndex = 0;

            var penSizeSource = new List<TextValue>();
            penSizeSource.AddRange(new[] {
                new TextValue { Text = "1", Value = "1" },
                new TextValue { Text = "2", Value = "2"},
                new TextValue { Text = "3", Value = "3" },
                new TextValue { Text = "4", Value = "4" },
                new TextValue { Text = "5", Value = "5" }});
            ddlPenSize.DataSource = penSizeSource;
            ddlPenSize.DisplayMember = "Text";
            ddlPenSize.ValueMember = "Value";
            ddlPenSize.SelectedIndex = 0;
        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //pictureBox1.Invalidate();
            //Invalidate();
            start = e.Location;
            blnDraw = true;
            isDraw = true;
            if (CurrDrawType == DrawType.LeftText || CurrDrawType == DrawType.RightText)
            {
                var draw = new DrawControl { DrawType = CurrDrawType, Pen = (Pen)CurrPen.Clone(), PenColor = CurrPen.Color, PenSize = CurrPen.Width, FillColor = btnColor.BackColor };
                switch (CurrDrawType)
                {
                    case DrawType.LeftText:
                    case DrawType.RightText:
                        draw.Start = start;
                        textBox1.Focus();
                        break;
                }
                Rectangles.Add(draw);
                currentyIndex = Rectangles.Count - 1;
            }
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
            var draw = new DrawControl { DrawType = CurrDrawType, Pen = (Pen)CurrPen.Clone(), PenColor = CurrPen.Color, PenSize = CurrPen.Width, FillColor = btnColor.BackColor };
            switch (CurrDrawType)
            {
                case DrawType.Line:
                case DrawType.LineCap:
                case DrawType.Curve:
                    draw.Start = start;
                    draw.End = end;
                    break;
                case DrawType.LeftText:
                case DrawType.RightText:
                    return;
                default:
                    draw.Rect = rect;
                    break;
            }
            if(start != end)
                Rectangles.Add(draw);
            rect.Width = rect.Height = 0;
        }

        private void imageBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null && isDraw)
            {
                Draw(e.Graphics);
            }

            if (Rectangles != null && Rectangles.Count > 0)
            {
                Rectangles.ForEach(f => Draw(e.Graphics, f));
            }
        }

        private void Draw(Graphics g)
        {
            var pen = (Pen)CurrPen.Clone();
            switch (CurrDrawType)
            {
                case DrawType.Rectangle:
                    g.DrawRectangle(pen, rect);
                    break;
                case DrawType.FillRectangle:
                    g.DrawRectangle(pen, rect);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.FillRectangle(new SolidBrush(btnColor.BackColor), rect);
                    break;
                case DrawType.Line:
                    g.DrawLine(CurrPen, start, end);
                    break;
                case DrawType.LineCap:
                    AdjustableArrowCap aac = new AdjustableArrowCap(5,3);
                    pen.CustomEndCap = aac;
                    g.DrawLine(pen, start, end);
                    break;
                case DrawType.Ellipse:
                    g.DrawEllipse(pen, rect);
                    break;
                case DrawType.FillEllips:
                    g.DrawEllipse(pen, rect);
                    g.FillEllipse(new SolidBrush(btnColor.BackColor), rect);
                    break;
                case DrawType.LeftText:
                    if (currentyIndex < Rectangles.Count())
                        g.DrawString(Rectangles[currentyIndex].Text, font, new SolidBrush(pen.Color), new PointF(start.X, start.Y));
                    break;
                case DrawType.RightText:
                    StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
                    if (currentyIndex < Rectangles.Count())
                        g.DrawString(Rectangles[currentyIndex].Text, font, new SolidBrush(pen.Color), new PointF(start.X, start.Y), format);
                    break;
            }
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
                    AdjustableArrowCap aac = new AdjustableArrowCap(5, 3);
                    pen.CustomEndCap = aac;
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
                case DrawType.RightText:
                    StringFormat format = new StringFormat(StringFormatFlags.DirectionRightToLeft);
                    g.DrawString(draw.Text, font, new SolidBrush(draw.Pen.Color), new PointF(draw.Start.X, draw.Start.Y), format);
                    break;
            }
        }

        private string ReverseString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog ColorForm = new ColorDialog();
            if (ColorForm.ShowDialog() == DialogResult.OK)
            {
                Color GetColor = ColorForm.Color;
                btnColor.BackColor = GetColor;
                CurrPen.Color = GetColor;
            }
        }

        private void ddlPenSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrPen.Width = Convert.ToInt32(ddlPenSize.SelectedValue);
        }

        private void DrawTypeSelected(object sender, EventArgs e)
        {
            CurrDrawType = (DrawType)Convert.ToInt32(((System.Windows.Forms.ComboBox)sender).SelectedValue);
            label1.Text = ((TextValue)((System.Windows.Forms.ComboBox)sender).SelectedItem).Text;
        }

        private void DrawType_Click(object sender, EventArgs e)
        {
            var btn = (System.Windows.Forms.Button)sender;
            CurrDrawType = (DrawType)btn.TabIndex;
            label1.Text = btn.Tag.ToString();
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            switch(tabControl1.SelectedIndex)
            {
                case 0:
                    CurrDrawType = DrawType.Line;
                    label1.Text = "直线";
                    break;
                case 1:
                    CurrDrawType = DrawType.Rectangle;
                    label1.Text = "空心矩形";
                    break;
                case 2:
                    CurrDrawType = DrawType.Ellipse;
                    label1.Text = "空心圆";
                    break;
                case 3:
                    CurrDrawType = DrawType.LeftText;
                    label1.Text = "左对齐";
                    break;
            }
        }

        private void FormEditPicture_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine(e.KeyChar.ToString());
            if (e.KeyChar == 8)
            {
                var str = Rectangles[currentyIndex].Text;
                if (str.Length == 0) return;
                Rectangles[currentyIndex].Text = str.Remove(str.Length - 1);
            }
            else
            {
                Rectangles[currentyIndex].Text += e.KeyChar.ToString();
            }
            pictureBox1.Invalidate();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Rectangles.Count > 0)
            {
                OldRectangles.Add(Rectangles.Last());
                Rectangles.Remove(Rectangles.Last());
                currentyIndex = Rectangles.Count - 1;
                isDraw = false;
                pictureBox1.Invalidate();
            }
        }

        private void btnRecover_Click(object sender, EventArgs e)
        {
            if (OldRectangles.Count > 0)
            {
                Rectangles.Add(OldRectangles.Last());
                OldRectangles.Remove(OldRectangles.Last());
                currentyIndex = Rectangles.Count - 1;
                isDraw = false;
                pictureBox1.Invalidate();
            }
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            imgInfo.ImgEditInfo = Rectangles;
            SaveImg(false);
            this.Close();
            //CurrAction = ActionType.None;
            //ShowPanel();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            OldRectangles.Clear();
            Rectangles.Clear();
            pictureBox1.Invalidate();
        }
        #endregion

        #region 裁剪图像
        private void btnCutImg_Click(object sender, EventArgs e)
        {
            CurrAction = ActionType.Cut;
            ShowPanel();

            pictureBox1.MouseDown += PictureBox1_Cut_MouseDown;
            pictureBox1.MouseMove += PictureBox1_Cut_MouseMove;
            pictureBox1.MouseUp += PictureBox1_Cut_MouseUp;
            pictureBox1.Paint += imageBox1_Cut_Paint;
        }

        Point cut_start; //画框的起始点
        Point cut_end;//画框的结束点<br>
        bool cut_blnDraw;//判断是否绘制<br>
        bool cut_isDraw = true;
        Rectangle cut_rect;
        private void PictureBox1_Cut_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X > pictureBox1.Width || e.Y > pictureBox1.Height)
            {
                base.OnMouseDown(e);
                return;
            }
            this.Invoke(new Action(() => { label6.Text = e.X + ":" + e.Y; label5.Text = pictureBox1.Width + ":" + pictureBox1.Height; })); 
            cut_start = e.Location;
            pictureBox1.Invalidate();
            Invalidate();
            cut_blnDraw = true;
            cut_isDraw = true;
        }

        private void PictureBox1_Cut_MouseMove(object sender, MouseEventArgs e)
        {
            if (cut_blnDraw)
            {
                if (e.Button != MouseButtons.Left)//判断是否按下左键
                    return;
                if (e.X > pictureBox1.Width || e.Y > pictureBox1.Height)
                {
                    base.OnMouseDown(e);
                    return;
                }
                Point tempEndPoint = e.Location; //记录框的位置和大小
                cut_rect.Location = new Point(
                Math.Min(cut_start.X, tempEndPoint.X),
                Math.Min(cut_start.Y, tempEndPoint.Y));
                cut_rect.Size = new Size(
                    Math.Abs(cut_start.X - tempEndPoint.X),
                    Math.Abs(cut_start.Y - tempEndPoint.Y));
                pictureBox1.Invalidate();
                cut_end = e.Location;
            }
        }

        private void PictureBox1_Cut_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Clip = Rectangle.Empty;
            cut_blnDraw = false; //结束绘制
            cut_end = e.Location;
        }

        private void imageBox1_Cut_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null && cut_isDraw && cut_rect.Width > 0)
            {
                Cut_Draw(e.Graphics);
            }
        }

        private void Cut_Draw(Graphics g)
        {
            g.DrawRectangle(Pens.Cyan, cut_rect);
        }

        private void btnOK_Cut_Click(object sender, EventArgs e)
        {
            if (cut_rect != null && cut_rect.Width > 0 && cut_rect.Height > 0)
            {
                //cut_rect.Width = (int)(cut_rect.Width * rate);
                //cut_rect.Height = (int)(cut_rect.Height * rate);
                //cut_rect.X = (int)(cut_rect.X * rate);
                var image = MakeThumbnailImage(pictureBox1.Image, cut_rect.Width, cut_rect.Height, cut_rect.Width, cut_rect.Height, cut_rect.X, cut_rect.Y);
                if (image != null)
                {
                    pictureBox1.Image = image;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    cut_isDraw = false;
                    cut_rect.Width = 0;
                    pictureBox1.Invalidate();
                    SaveImg(!chkCut.Checked);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Image = (Image)oldImg.Clone();
            if (copyInfo != null)
            {
                currHistory.imgInfos.RemoveAt(currHistory.imgInfos.Count - 1);
                var filePath = $"{dirName}//{copyInfo.FileName}.ini";
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CurrAction = ActionType.None;
            ShowPanel();
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
            var widthMultiple = originalImage.Width / (pictureBox1.Width * 1.0);
            var heightMultiple = originalImage.Height / (pictureBox1.Height * 1.0);
            cropWidth = maxWidth = (int)(cropWidth * widthMultiple);
            cropHeight = maxHeight = (int)(cropHeight * heightMultiple);
            X = (int)(X * widthMultiple);
            Y = (int)(Y * heightMultiple);

            if (cropWidth == 0 || cropHeight == 0)
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

        #endregion

        #region 调整大小
        private void btnChangeSize_Click(object sender, EventArgs e)
        {
            CurrAction = ActionType.ChangeSize;
            ShowPanel();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = (Image)oldImg.Clone();
            if (copyInfo != null)
            {
                currHistory.imgInfos.RemoveAt(currHistory.imgInfos.Count - 1);
                var filePath = $"{dirName}//{copyInfo.FileName}.ini";
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CurrAction = ActionType.None;
            ShowPanel();
        }

        private void txtAngle_Enter(object sender, EventArgs e)
        {
            if (chkC.Checked)
            {
                int height;
                int.TryParse(txtChangeH.Text, out height);
                if (height > 0)
                {
                    txtChangeW.Text = (imgInfo.Img.Width * height / imgInfo.Img.Height).ToString();
                }
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (chkC.Checked)
            {
                int width;
                int.TryParse(txtChangeW.Text, out width);
                if (width > 0)
                {
                    txtChangeH.Text = (imgInfo.Img.Height * width / imgInfo.Img.Width).ToString();
                }
            }
        }

        private void btnOK_Change_Click(object sender, EventArgs e)
        {
            int width;
            int.TryParse(txtChangeW.Text, out width);
            int height;
            int.TryParse(txtChangeH.Text, out height);
            string mode = "HW";
            if (chkC.Checked)
            {
                height = imgInfo.Img.Height * width / imgInfo.Img.Width;
                mode = "W";
            }
            MakeThumbnail(width, height, mode);
            SaveImg(chkChange.Checked);
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
            //label1.Text = $"{bitmap.Width} X {bitmap.Height}";
        }
        #endregion

        private void ShowPanel()
        {
            pChangSize.Visible = false;
            pCut.Visible = false;
            pEditRemark.Visible = false;
            pRotate.Visible = false;
            switch (CurrAction)
            {
                case ActionType.Rotate:
                    pRotate.Location = location;
                    pRotate.Visible = true;
                    break;
                case ActionType.ChangeSize:
                    pChangSize.Location = location;
                    pChangSize.Visible = true;
                    break;
                case ActionType.Cut:
                    pCut.Location = location;
                    pCut.Visible = true;
                    break;
                case ActionType.Remark:
                    pEditRemark.Location = location;
                    pEditRemark.Visible = true;
                    break;
            }

            pictureBox1.MouseDown -= PictureBox1_MouseDown;
            pictureBox1.MouseMove -= PictureBox1_MouseMove;
            pictureBox1.MouseUp -= PictureBox1_MouseUp;
            pictureBox1.Paint -= imageBox1_Paint;
            this.KeyPress -= FormEditPicture_KeyPress;

            pictureBox1.MouseDown -= PictureBox1_Cut_MouseDown;
            pictureBox1.MouseMove -= PictureBox1_Cut_MouseMove;
            pictureBox1.MouseUp -= PictureBox1_Cut_MouseUp;
            pictureBox1.Paint -= imageBox1_Cut_Paint;
        }

        private void SaveImg(bool isCopy = true)
        {
            var newImg = (Bitmap)pictureBox1.Image;
            if (isCopy)
            {                
                var names = currHistory.imgInfos.Select(s => Convert.ToInt32(s.FileName.Replace("S", "")));
                var id = names.Max() + 1;
                var fileName = $"{id}";
                var imgPath = $"{dirName}\\{copyInfo.FileName}.bmp";
                copyInfo = new ImgInfo
                {
                    ImgPath = imgPath,//Base64Util.GetBase64FromImage(newImg),
                    Date = imgInfo.Date, FileName = fileName, ID = id, Img = newImg,
                    ImgEditInfo = imgInfo.ImgEditInfo, InfoType = imgInfo.InfoType, IsSaveToFile = false,
                    Note = imgInfo.Note, ToothIndex = imgInfo.ToothIndex, VideoPath = imgInfo.VideoPath
                };
                File.WriteAllText($"{dirName}\\{copyInfo.FileName}.ini", copyInfo.ToJson());
                copyInfo.Img.Save(imgPath);
                currHistory.imgInfos.Add(copyInfo);
            }
            else
            {
                try
                {
                    var imgPath = $"{dirName}\\{imgInfo.FileName}.bmp";
                    imgInfo.Img = newImg;
                    imgInfo.ImgPath = imgPath;// Base64Util.GetBase64FromImage(img.Img);
                    imgInfo.IsSaveToFile = false;
                    File.WriteAllText($"{dirName}\\{imgInfo.FileName}.ini", imgInfo.ToJson());
                    imgInfo.Img.Save(imgPath);
                    copyInfo = null;
                }
                catch(Exception ex)
                {

                }
            }                
        }

        enum ActionType
        {
            None,
            Rotate,
            Remark,
            Cut,
            ChangeSize
        }

        private void btnShowRemark_Click(object sender, EventArgs e)
        {
            var isShow = btnShowRemark.Text == "显示备注";
            btnShowRemark.Text = btnShowRemark.Text == "显示备注" ? "隐藏备注" : "显示备注";
            
            pictureBox1.Image = (Image)oldImg.Clone();
            if (isShow)
            {
                isDraw = false;
                Rectangles = imgInfo.ImgEditInfo;
                pictureBox1.Paint += imageBox1_Paint;
                pictureBox1.Invalidate();
            }
            else
            {
                pictureBox1.Paint -= imageBox1_Paint;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeleteCurrentImg();
        }

        private void DeleteCurrentImg()
        {
            if(File.Exists($"{dirName}\\{copyInfo.FileName}.ini"))
            {
                File.Delete($"{dirName}\\{copyInfo.FileName}.ini");
            }
            CommHelp.ShowTips("图片已经删除，请选择其它图片编辑");
            this.Close();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {

        }        
    }    
}
