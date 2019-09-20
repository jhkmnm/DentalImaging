using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormFirst : Form
    {
        string dirName = User.DBPath;//"Patient";

        public FormFirst()
        {
            InitializeComponent();
            this.Text = System.Environment.UserName;

            User.DBPath = ConfigurationManager.AppSettings["DBPath"];
            var allOrTop = ConfigurationManager.AppSettings["AllOrTop"];
            var top = ConfigurationManager.AppSettings["Top"];
            int pagecount = 200;
            int.TryParse(top, out pagecount);
            User.IsAll = allOrTop == "All";
            User.PageCount = pagecount;
            if (string.IsNullOrEmpty(User.DBPath))
            {
                User.DBPath = "Patient";
            }
            dirName = User.DBPath;
            ReadPatients();
            if (User.Patients == null)
            {
                User.Patients = new List<Patient>();
            }
        }

        private void AddNew()
        {
            txtNum.Text = string.Empty;
            txtName1.Text = string.Empty;
            txtName2.Text = string.Empty;
            txtAdress1.Text = string.Empty;
            txtAdress2.Text = string.Empty;
            txtAdress3.Text = string.Empty;
            txtSheBao.Text = string.Empty;
            txtTel1.Text = string.Empty;
            txtTel2.Text = string.Empty;
            txtType.Text = string.Empty;
            txtYijian.Text = string.Empty;
            barBtnInsertImg.Enabled = true;
            barBtnRemoveImg.Enabled = true;

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            pictureBox1.Enabled = true;
            panel1.Visible = true;
            txtType.Text = "新建";
        }

        private void Find()
        {
            var f = new FormSearch();
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtNum.Text = User.CurrentPatient.Number;
                txtName1.Text = User.CurrentPatient.FName;
                txtName2.Text = User.CurrentPatient.Name;
                dateTimePicker1.Text = User.CurrentPatient.BirthDay;
                txtSheBao.Text = User.CurrentPatient.SECU;
                txtAdress1.Text = User.CurrentPatient.Address1;
                txtAdress2.Text = User.CurrentPatient.Address2;
                txtAdress3.Text = User.CurrentPatient.Address3;
                txtTel1.Text = User.CurrentPatient.Tel1;
                txtTel2.Text = User.CurrentPatient.Tel2;
                txtYijian.Text = User.CurrentPatient.Yijian;
                pictureBox1.ImageLocation = User.CurrentPatient.Photo;

                groupBox1.Enabled = false;
                groupBox2.Enabled = false;
                groupBox3.Enabled = false;
                pictureBox1.Enabled = false;
                panel1.Visible = false;

                if (f.Type == "修改")
                {
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                    groupBox3.Enabled = true;
                    pictureBox1.Enabled = true;
                    panel1.Visible = true;
                    txtType.Text = "修改";
                }
            }
        }

        private void Edit()
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            pictureBox1.Enabled = true;
            panel1.Visible = true;
            txtType.Text = "修改";
            barBtnInsertImg.Enabled = true;
            barBtnRemoveImg.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Cancel()
        {
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            pictureBox1.Enabled = false;
            panel1.Visible = false;

            if (User.CurrentPatient != null)
            {
                txtNum.Text = User.CurrentPatient.Number;
                txtName1.Text = User.CurrentPatient.FName;
                txtName2.Text = User.CurrentPatient.Name;
                dateTimePicker1.Text = User.CurrentPatient.BirthDay;
                txtSheBao.Text = User.CurrentPatient.SECU;
                txtAdress1.Text = User.CurrentPatient.Address1;
                txtAdress2.Text = User.CurrentPatient.Address2;
                txtAdress3.Text = User.CurrentPatient.Address3;
                txtTel1.Text = User.CurrentPatient.Tel1;
                txtTel2.Text = User.CurrentPatient.Tel2;
                txtYijian.Text = User.CurrentPatient.Yijian;
            }
            else
            {
                txtNum.Text = "";
                txtName1.Text = "";
                txtName2.Text = "";
                dateTimePicker1.Text = "";
                txtSheBao.Text = "";
                txtAdress2.Text = "";
                txtAdress3.Text = "";
                txtAdress1.Text = "";
                txtTel1.Text = "";
                txtTel2.Text = "";
                txtYijian.Text = "";
                barBtnInsertImg.Enabled = false;
                barBtnRemoveImg.Enabled = false;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            panel1.Visible = true;
            pictureBox1.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName1.Text) || string.IsNullOrEmpty(txtName2.Text))
            {
                MessageBox.Show("必须输入姓名");
                return;
            }
            else
            {
                var v = new Patient { FName = txtName1.Text, Name = txtName2.Text, Address1 = txtAdress1.Text, Address2 = txtAdress2.Text, Address3 = txtAdress3.Text, BirthDay = dateTimePicker1.Text, SECU = txtSheBao.Text, Tel1 = txtTel1.Text, Tel2 = txtTel2.Text, Yijian = txtYijian.Text, Photo = pictureBox1.ImageLocation};
                if (txtType.Text == "新建")
                {
                    v.Number = CreateNum();
                    v.Photo = SaveImg(v.Number);
                    User.Patients.Add(v);
                }
                else
                {
                    v.Number = User.CurrentPatient.Number;
                    v.Photo = SaveImg(v.Number);
                    User.CurrentPatient = v;
                    var index = User.Patients.FindIndex(f => f.Number == v.Number);
                    User.Patients[index] = v;
                }

                SavePatients();
            }
        }

        private string CreateNum()
        {
            var py = PinYinConverterHelp.GetTotalPingYin(txtName1.Text);
            string f = "";
            if(py.FirstPingYin.Count == 0)
            {
                f = txtName1.Text[0].ToString();
            }
            else
            {
                f = py.FirstPingYin[0];
            }
            var patients = User.Patients.Where(w => w.Name.StartsWith(f)).OrderByDescending(o => o.Name).ToList();
            var index = 1;
            if(patients.Any())
            {
                index = Convert.ToInt32(patients[0].Number.Remove(0,1).TrimStart('0')) + 1;
            }

            return $"{f[0]}{index.ToString().PadLeft(5, '0')}";
        }
                
        private void SavePatients()
        {            
            if(!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllText($"{dirName}/us.ini", User.Patients.ToJson());
        }

        private void ReadPatients()
        {
            try
            {
                var str = File.ReadAllText($"{dirName}/us.ini");
                User.Patients = str.ToObject<List<Patient>>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string SaveImg(string num)
        {
            if (pictureBox1.Image == null)
                return "";            
            string imgDir = "img";
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            if (!Directory.Exists($"{dirName}/{ imgDir}"))
            {
                Directory.CreateDirectory($"{dirName}/{ imgDir}");
            }
            var imgPath = $"{dirName}/{imgDir}/{num}.jpg";

            Bitmap bmp = new Bitmap(pictureBox1.Image);
            bmp.Save(imgPath, pictureBox1.Image.RawFormat);
            //bmp.Save(imgPath);
            return imgPath;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            InsertImg();
        }

        private void InsertImg()
        {
            Regex reg = new Regex("(.jpg|.jpeg|.gif|.png|.bmp)$");
            int size = 1024 * 1024;
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.Filter = "JPG|*.jpg|JPGE|*.jpeg|GIF|*.gif|PNG|*.png|BMP|*.bmp";
                file.CheckFileExists = true;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    var match = reg.Match(file.SafeFileName);
                    if (!match.Success)
                    {
                        MessageBox.Show("请选择图片文件");
                    }
                    var f = new FileInfo(file.FileName);
                    if (f.Length > size)
                    {
                        MessageBox.Show("文件大于必须小于1M");
                    }
                    pictureBox1.ImageLocation = file.FileName;
                }
            }
        }

        private void RemoveImg()
        {
            pictureBox1.Image = null;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void bbtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddNew();
        }

        private void bbtnFind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Find();
        }

        private void bbtnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Edit();
        }

        private void bbtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barDockControlTop_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void barBtnInsertImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InsertImg();
        }

        private void barBtnRemoveImg_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RemoveImg();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var form = new FormFile();
            form.ShowDialog();
        }

        private void bbtnP_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new frmPatient().Show();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FormParameterB().Show();
        }

        private void barToggleSwitchItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bar1.Visible = !bar1.Visible;
        }

        private void bbtnDel2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("所有图像都将被删除", "警告", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Directory.Delete(dirName + "//" + User.CurrentPatient.Number, true);
                User.Patients.Remove(User.CurrentPatient);
                User.CurrentPatient = null;
                SavePatients();
                Cancel();
            }
        }
    }
}
