using DentalImaging.Help;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging.拍摄
{
    public partial class FormImgControl : Form
    {
        public ImgInfo info { get; set; }
        public FormImgControl()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void FormImgControl_Load(object sender, EventArgs e)
        {
            //var parent = this.MdiParent;
            //this.Location = new Point(parent.Width - this.Width, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Resetting();
        }

        public void Resetting()
        {
            trackBar1.Value = 0;
            trackBar2.Value = 0;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            info.Img = ImageClass.KiLighten(info.Img, trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            info.Img = ImageClass.KiContrast(info.Img, trackBar1.Value);
        }
    }
}
