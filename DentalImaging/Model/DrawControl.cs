using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DentalImaging.Model
{
    public enum DrawType
    {
        /// <summary>
        /// 空心矩形
        /// </summary>
        Rectangle,
        /// <summary>
        /// 实心矩形
        /// </summary>
        FillRectangle,
        /// <summary>
        /// 直线
        /// </summary>
        Line,
        /// <summary>
        /// 箭头
        /// </summary>
        LineCap,
        /// <summary>
        /// 曲线
        /// </summary>
        Curve,
        /// <summary>
        /// 空心圆
        /// </summary>
        Ellipse,
        /// <summary>
        /// 实心圆
        /// </summary>
        FillEllips,
        /// <summary>
        /// 左对齐文字
        /// </summary>
        LeftText,
        /// <summary>
        /// 右对齐文字
        /// </summary>
        RightText
    }

    public class DrawControl
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Rectangle Rect { get; set; }
        public Point Start { get; set; }
        public Point End { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public DrawType DrawType { get; set; }
        /// <summary>
        /// 填充颜色
        /// </summary>
        public Color FillColor { get; set; }
        private Pen _pen;
        /// <summary>
        /// 画笔
        /// </summary>
        [JsonIgnore]
        public Pen Pen
        {
            get
            {
                if (_pen == null)
                    _pen = new Pen(PenColor, PenSize);
                return _pen;
            }
            set
            {
                _pen = value;
            }
        }
        public Color PenColor { get; set; }
        public float PenSize { get; set; }
        public string Text { get; set; }
    }

    public class TextValue
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
