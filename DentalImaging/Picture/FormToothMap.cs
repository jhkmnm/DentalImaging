using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormToothMap : Form
    {
        List<int> ToothIndex = new List<int>();
        string dirName = $"{User.DBPath}/{User.CurrentPatient.Number}";

        public FormToothMap()
        {
            InitializeComponent();
            this.ControlBox = false;            
            Init();
        }

        private void FormToothMap_Load(object sender, EventArgs e)
        {
            var parent = this.MdiParent;
            this.Location = new Point(parent.Width - this.Width, 0);
        }

        private void Tooth()
        {
            if(!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            var files = Directory.GetFiles(dirName, "*.ini");
            List<int> fileIndex = new List<int>();
            foreach (var file in files)
            {
                var str = File.ReadAllText(file);
                var img = str.ToObject<ImgInfo>();
                if(img.ToothIndex != null)
                {
                    foreach (var index in img.ToothIndex)
                    {
                        if (!ToothIndex.Contains(index))
                            ToothIndex.Add(index);
                    }
                }                
            }
        }

        private void Init()
        {
            var dirPath = $"{System.Environment.CurrentDirectory}\\Image";
            var xstart = 101;
            var ystart = 8;
            var w = 17;
            var h = 43;
            var rowCount = 16;
            Tooth();

            for (int i = 0; i <= 32; i++)
            {
                int x = xstart;
                int y = ystart;
                if(i > 0)
                {
                    x = xstart + (w * (i % rowCount));
                    y = ystart + (h * (i / rowCount));
                }
                var pic = new PictureBox();
                pic.Name = $"pic_{i+1}";
                pic.Tag = i + 1;
                pic.ImageLocation = ToothIndex.Contains((i + 1)) ? $"{dirPath}\\{i + 1}-2.bmp" : $"{dirPath}\\{i + 1}-1.bmp";
                pic.Location = new System.Drawing.Point(x, y);
                pic.Size = new System.Drawing.Size(18, 44);
                pic.TabIndex = i;
                pic.TabStop = false;
                pic.MouseEnter += Pic_MouseEnter;
                pic.Click += Pic_Click;
                this.Controls.Add(pic);
            }
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            var pic = (PictureBox)sender;
            var form = new FormToothImg(Convert.ToInt32(pic.Tag));
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        private void Pic_MouseEnter(object sender, EventArgs e)
        {
            label1.Text = ((PictureBox)sender).Tag.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new FormMedicalHistory();
            if(form.ShowDialog() == DialogResult.OK)
            {
                foreach(var img in form.SelectedTooth)
                {
                    var imgForm = new FormImg(img);
                    imgForm.MdiParent = this.MdiParent;
                    imgForm.Show();                    
                }
                var conForm = FormEditControl.GetForm();
                conForm.MdiParent = this.MdiParent;
                conForm.Show();
            }
        }
    }
}
