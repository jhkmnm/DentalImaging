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
    public partial class FormEditControl : Form
    {
        List<int> ToothIndex = new List<int>();
        string dirName = $"{User.DBPath}/{User.CurrentPatient.Number}";
        private static FormEditControl form;

        public static FormEditControl GetForm()
        {
            if(form == null)
            {
                form = new FormEditControl();
            }
            return form;
        }

        public FormEditControl()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User.CurrentImg.DrawImg(button1.Text == "显示备注");
            button1.Text = button1.Text == "显示备注" ? "隐藏备注" : "显示备注";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new FormEditPicture(User.CurrentImg.Img);
            form.ShowDialog();
        }

        private void btnRotate180_Click(object sender, EventArgs e)
        {
            User.CurrentImg.RotateImage(180);
        }

        private void btnRotate90_Click(object sender, EventArgs e)
        {
            User.CurrentImg.RotateImage(90);
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            User.CurrentImg.Mirror();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new FormRotate(User.CurrentImg.Img);
            form.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var form = new FormChangeSize(User.CurrentImg.Img);
            form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var form = new FormCutImg(User.CurrentImg.Img);
            form.ShowDialog();
        }
    }
}
