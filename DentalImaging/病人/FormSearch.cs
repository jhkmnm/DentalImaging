using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormSearch : Form
    {
        bool isPage = false;
        int pageIndex = 1;
        int pageCount = 200;

        public FormSearch()
        {
            InitializeComponent();
            txtFocus = textBox1;
            isPage = !User.IsAll;
            pageCount = User.PageCount;
            InitData();
            InitButton();
        }

        List<Patient> dataSource;
        private void InitData()
        {
            dataSource = User.Patients;
            if(!string.IsNullOrEmpty(textBox1.Text))
            {
                dataSource = User.Patients.Where(w => w.FName.Contains(textBox1.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                dataSource = User.Patients.Where(w => w.Name.Contains(textBox2.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                dataSource = User.Patients.Where(w => w.SECU.Contains(textBox3.Text)).ToList();
            }
            pageIndex = 1;
            BandData();
        }

        private void BandData()
        {
            if(isPage)
            {
                panel2.Visible = true;
                var pageTotal = Math.Ceiling(dataSource.Count / (pageCount * 1.0));
                lblPage.Text = $"{pageIndex}/{pageTotal}";
                if(pageTotal == 1)
                {
                    btnFirst.Enabled = false;
                    btnA.Enabled = false;
                    btnEnd.Enabled = false;
                    btnB.Enabled = false;
                }
                else
                {
                    if (pageIndex == 1)
                    {
                        btnFirst.Enabled = false;
                        btnA.Enabled = false;
                        btnEnd.Enabled = true;
                        btnB.Enabled = true;
                    }

                    if (pageIndex == pageTotal)
                    {
                        btnFirst.Enabled = true;
                        btnA.Enabled = true;
                        btnEnd.Enabled = false;
                        btnB.Enabled = false;
                    }
                }

                List<Patient> source = new List<Patient>();
                var start = (pageIndex == 1 ? 0 : (pageIndex - 1) * pageCount);
                for (int i = 0; i < pageCount; i++)
                {
                    if (start + i >= dataSource.Count)
                        break;
                    source.Add(dataSource[start + i]);
                }

                patientBindingSource.DataSource = source;
                dataGridView1.Refresh();
            }
            else
            {
                panel2.Visible = false;
                patientBindingSource.DataSource = dataSource;
                dataGridView1.Refresh();
            }            
        }

        public string Type { get; set; }

        TextBox txtFocus;

        string[] strKey = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "'", "-", " ", "<-"};
        private void InitButton()
        {
            panel1.Controls.Clear();
            int xstart = 34, ystart = 18;
            int xstep = 33, ystep = 29;
            for (int i = 0; i < 40; i++)
            {
                int x = xstart;
                int y = ystart;
                if (i > 0)
                {
                    x = xstart + (xstep * (i % 10));
                    y = ystart + (ystep * (i / 10));
                }

                Button btn = new Button();
                btn.Location = new System.Drawing.Point(x, y);
                btn.Name = $"btnS{strKey[i]}";
                btn.Size = new System.Drawing.Size(27, 23);
                btn.Text = strKey[i];
                btn.UseVisualStyleBackColor = true;
                btn.Click += Btn_Click;
                panel1.Controls.Add(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if(txtFocus != null)
            {
                var btn = (Button)sender;
                if(btn.Text == "<-")
                {
                    txtFocus.Text = txtFocus.Text.Length > 0 ? txtFocus.Text.Remove(txtFocus.Text.Length - 1, 1) : "";
                }
                else
                {
                    txtFocus.Text += btn.Text;
                }
            }
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            txtFocus = (TextBox)sender;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(User.Patients != null)
            {
                InitData();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (User.Patients != null)
            {
                InitData();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (User.Patients != null)
            {
                InitData();
            }
        }

        private Patient SelectedPatient
        {
            get {
                var rows = dataGridView1.SelectedRows;
                if (rows.Count == 0)
                {
                    return (Patient)patientBindingSource[0];
                }
                else
                {
                    return (Patient)patientBindingSource.Current;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            User.CurrentPatient = SelectedPatient;
            Type = "查看";
            DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            User.CurrentPatient = SelectedPatient;
            Type = "修改";
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            DialogResult = DialogResult.No;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            User.CurrentPatient = (Patient)patientBindingSource.Current;
            new frmPatient().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User.CurrentPatient = SelectedPatient;
            new frmPatient().Show();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            pageIndex = 1;
            BandData();
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            pageIndex -= 1;
            BandData();
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            pageIndex += 1;
            BandData();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            pageIndex = (int)Math.Ceiling(dataSource.Count / (pageCount * 1.0));
            BandData();
        }
    }
}
