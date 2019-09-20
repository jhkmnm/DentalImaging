using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging.Format
{
    public partial class FormCompare : Form
    {
        private PictureBox _currpic;
        public PictureBox CurrPic { get { return _currpic; } }

        public FormCompare()
        {
            InitializeComponent();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox p = (PictureBox)sender;
            if(p.Image == null)
            {
                Pen pp = new Pen(Color.White);
                e.Graphics.DrawRectangle(pp, e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.X + e.ClipRectangle.Width - 1, e.ClipRectangle.Y + e.ClipRectangle.Height - 1);
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            _currpic.Invalidate();
            PictureBox p = (PictureBox)sender;
            _currpic = p;
            Pen pp = new Pen(Color.Yellow);
            var g = Graphics.FromImage(p.Image);
            g.DrawRectangle(pp, p.Location.X, p.Location.Y, p.Location.X + p.Width - 1, p.Location.Y + p.Height - 1);
        }
    }
}
