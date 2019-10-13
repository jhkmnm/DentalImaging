using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormMedicalHistory : Form
    {
        List<ImgInfo> Tooth = new List<ImgInfo>();
        string dirName = $"{User.DBPath}/{User.CurrentPatient.Number}";

        public FormMedicalHistory()
        {
            InitializeComponent();
            dgvCatalog.AutoGenerateColumns = false;
            InitCataLog();
        }

        private void InitCataLog()
        {
            var files = Directory.GetFiles(dirName, "*.ini");
            List<int> fileIndex = new List<int>();
            foreach (var file in files)
            {
                var str = File.ReadAllText(file);
                var img = str.ToObject<ImgInfo>();
                //img.Img = Help.Base64Util.GetImageFromBase64(img.Base64);
                Tooth.Add(img);
            }
            label2.Text = $"/ {Tooth.Count()}";
            imgInfoBindingSource.DataSource = Tooth;
        }

        public List<ImgInfo> SelectedTooth
        {
            get {
                var result = new List<ImgInfo>();
                foreach(DataGridViewRow row in dgvCatalog.SelectedRows)
                {
                    result.Add((ImgInfo)row.DataBoundItem);
                }
                return result;
            }
        }

        public ImgInfo CataLogCurrent
        {
            get { return (ImgInfo)imgInfoBindingSource.Current; }
        }

        private void dgvCatalog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            picCatalog.Image = CataLogCurrent.Img;
            lblCount.Text = dgvCatalog.SelectedRows.Count.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
