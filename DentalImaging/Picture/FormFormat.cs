using DentalImaging.Help;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DentalImaging.Picture
{
    public partial class FormFormat : Form
    {
        public FormFormat()
        {
            InitializeComponent();
            lbxFormat.SelectedIndex = 0;
        }

        public int SelectedIndex {
            get { return lbxFormat.SelectedIndex; }
        }

        private void InitPanel(int index)
        {
            panel1.Controls.Clear();
            if(index == 1)
            {
                var width = (panel1.Width / 2) - 8;
                var height = panel1.Height - 3;

                Panel p1 = new Panel();
                p1.ForeColor = Color.White;
                p1.Location = new Point(3,3);
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Name = "p1";
                p1.Size = new Size(width, height);

                Panel p2 = new Panel();
                p2.ForeColor = Color.White;
                p2.BorderStyle = BorderStyle.FixedSingle;
                p2.Location = new Point(3 + width + 2, 3);
                p2.Name = "p2";
                p2.Size = new Size(width, height);

                panel1.Controls.Add(p1);
                panel1.Controls.Add(p2);
            }
            else if(index == 2)
            {
                var width = (panel1.Width / 2) - 8;
                var height = (panel1.Height / 2) - 8;

                Panel p1 = new Panel();
                p1.ForeColor = Color.White;
                p1.Location = new Point(3, 3);
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Name = "p1";
                p1.Size = new Size(width, height);

                Panel p2 = new Panel();
                p2.ForeColor = Color.White;
                p2.BorderStyle = BorderStyle.FixedSingle;
                p2.Location = new Point(3 + width + 2, 3);
                p2.Name = "p2";
                p2.Size = new Size(width, height);

                Panel p3 = new Panel();
                p3.ForeColor = Color.White;
                p3.BorderStyle = BorderStyle.FixedSingle;
                p3.Location = new Point(3, 3 + height + 2);
                p3.Name = "p3";
                p3.Size = new Size(width, height);

                Panel p4 = new Panel();
                p4.ForeColor = Color.White;
                p4.BorderStyle = BorderStyle.FixedSingle;
                p4.Location = new Point(3 + width + 2, 3 + height + 2);
                p4.Name = "p4";
                p4.Size = new Size(width, height);

                panel1.Controls.Add(p1);
                panel1.Controls.Add(p2);
                panel1.Controls.Add(p3);
                panel1.Controls.Add(p4);
            }
            else if (index == 3)
            {
                var width = (panel1.Width / 4) - 12;
                var height = (panel1.Height / 2) - 8;

                Panel p1 = new Panel();
                p1.ForeColor = Color.White;
                p1.Location = new Point(3, 3);
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Name = "p1";
                p1.Size = new Size(width, height);

                Panel p2 = new Panel();
                p2.ForeColor = Color.White;
                p2.Location = new Point(3 + width + 2, 3);
                p2.BorderStyle = BorderStyle.FixedSingle;
                p2.Name = "p2";
                p2.Size = new Size(width, height);

                Panel p3 = new Panel();
                p3.ForeColor = Color.White;
                p3.Location = new Point(3 + width * 2 + 4, 3);
                p3.BorderStyle = BorderStyle.FixedSingle;
                p3.Name = "p3";
                p3.Size = new Size(width, height);

                Panel p4 = new Panel();
                p4.ForeColor = Color.White;
                p4.Location = new Point(3 + width * 3 + 6, 3);
                p4.BorderStyle = BorderStyle.FixedSingle;
                p4.Name = "p4";
                p4.Size = new Size(width, height);

                Panel p5 = new Panel();
                p5.ForeColor = Color.White;
                p5.Location = new Point(3, 3 + height + 2);
                p5.BorderStyle = BorderStyle.FixedSingle;
                p5.Name = "p5";
                p5.Size = new Size(width, height);

                Panel p6 = new Panel();
                p6.ForeColor = Color.White;
                p6.Location = new Point(3 + width + 2, 3 + height + 2);
                p6.BorderStyle = BorderStyle.FixedSingle;
                p6.Name = "p6";
                p6.Size = new Size(width, height);

                Panel p7 = new Panel();
                p7.ForeColor = Color.White;
                p7.Location = new Point(3 + width * 2 + 4, 3 + height + 2);
                p7.BorderStyle = BorderStyle.FixedSingle;
                p7.Name = "p7";
                p7.Size = new Size(width, height);

                Panel p8 = new Panel();
                p8.ForeColor = Color.White;
                p8.Location = new Point(3 + width * 3 + 6, 3 + height + 2);
                p8.BorderStyle = BorderStyle.FixedSingle;
                p8.Name = "p6";
                p8.Size = new Size(width, height);

                panel1.Controls.Add(p1);
                panel1.Controls.Add(p2);
                panel1.Controls.Add(p3);
                panel1.Controls.Add(p4);
                panel1.Controls.Add(p5);
                panel1.Controls.Add(p6);
                panel1.Controls.Add(p7);
                panel1.Controls.Add(p8);
            }
        }

        private void lbxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {            
            InitPanel(lbxFormat.SelectedIndex + 1);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void FormFormat_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }
    }
}
