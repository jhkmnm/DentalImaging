using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormEditPicture : Form
    {
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
        ImgInfo img;

        public FormEditPicture(ImgInfo img)
        {
            InitializeComponent();
            InitDDL();
            this.img = img;
            pictureBox1.Image = img.Img;
            CurrDrawType = DrawType.Line;
            CurrPen = new Pen(btnColor.BackColor, 1);
        }

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
            start = e.Location;
            pictureBox1.Invalidate();
            Invalidate();
            blnDraw = true;
            isDraw = true;
            if (CurrDrawType == DrawType.LeftText || CurrDrawType == DrawType.RightText)
            {
                var draw = new DrawControl { DrawType = CurrDrawType, Pen = (Pen)CurrPen.Clone(), FillColor = btnColor.BackColor };
                switch (CurrDrawType)
                {
                    case DrawType.LeftText:
                    case DrawType.RightText:
                        draw.Start = start;
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
            var draw = new DrawControl { DrawType = CurrDrawType, Pen = (Pen)CurrPen.Clone(), FillColor = btnColor.BackColor };
            switch(CurrDrawType)
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
            Rectangles.Add(draw);
            rect.Width = rect.Height = 0;
        }

        private void imageBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null && isDraw)
            {
                //if (rect != null && rect.Width > 0 && rect.Height > 0)
                //{
                Draw(e.Graphics);
                //    //e.Graphics.DrawRectangle(new Pen(Color.Red, 3), rect);//重新绘制颜色为红色
                //    //e.Graphics.FillRectangle(new SolidBrush(Color.Green), rect);
                //}
            }

            if (Rectangles.Count > 0)
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
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
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
                    if(currentyIndex < Rectangles.Count())
                        g.DrawString(Rectangles[currentyIndex].Text, font, new SolidBrush(pen.Color), new PointF(start.X, start.Y));
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

        private void btnColor_Click(object sender, EventArgs e)
        {
            ColorDialog ColorForm = new ColorDialog();
            if (ColorForm.ShowDialog() == DialogResult.OK)
            {
                Color GetColor = ColorForm.Color;
                //GetColor就是用户选择的颜色，接下来就可以使用该颜色了
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
            CurrDrawType = (DrawType)Convert.ToInt32(((ComboBox)sender).SelectedValue);
            label1.Text = ((TextValue)((ComboBox)sender).SelectedItem).Text;
        }

        private void FormEditPicture_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 8)
            {
                var str = Rectangles[currentyIndex].Text;
                if (str.Length == 0) return;
                Rectangles[currentyIndex].Text = str.Remove(str.Length - 1);
            }
            else
                Rectangles[currentyIndex].Text += e.KeyChar.ToString();
            pictureBox1.Invalidate();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(Rectangles.Count > 0)
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
            img.ImgEditInfo = Rectangles;
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            OldRectangles.Clear();
            Rectangles.Clear();
            pictureBox1.Invalidate();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {

        }
    }
}
