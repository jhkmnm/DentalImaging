using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DentalImaging.Model;
using System.Linq;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System.IO;
using WareHouseMis.UI;
using DentalImaging.Help;

namespace DentalImaging.新界面
{
    public partial class FrmItemDetail : XtraForm
    {
        CaseHistory currentHistory;

        public FrmItemDetail()
        {
            InitializeComponent();
            InitGrid();
        }

        public void InitData()
        {
            var NOs = new List<CListItem>();
            if (User.CurrentPatient != null)
            {
                lblName.Text = User.CurrentPatient.FName;
                lblBirthDay.Text = User.CurrentPatient.BirthDay;
                lblNo.Text = User.CurrentPatient.SECU;
                User.ReadCurrentPatientHistorys();
                if (User.CurrentPatient.Historys != null)
                {
                    NOs = User.CurrentPatient.Historys.Select(s => new CListItem(s.HistoryNo)).ToList();
                }
                NOs.Insert(0, new CListItem("新建"));
                BindComboBoxItems(txtUsagePos, NOs, "新建");
                txtUsagePos.SelectedIndex = 0;
                txtRemark.Text = string.Empty;
                currentHistory = null;
                gridControl1.DataSource = null;
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 绑定下拉列表控件为指定的数据字典列表
        /// </summary>
        /// <param name="combo">下拉列表控件</param>
        /// <param name="itemList">数据字典列表</param>
        /// <param name="defaultValue">控件默认值</param>
        private void BindComboBoxItems(ComboBoxEdit combo, List<CListItem> itemList, string defaultValue)
        {
            combo.Properties.BeginUpdate();//可以加快
            combo.Properties.Items.Clear();
            combo.Properties.Items.AddRange(itemList);

            if (!string.IsNullOrEmpty(defaultValue))
            {
                //combo.SetComboBoxItem(defaultValue);
            }

            combo.Properties.EndUpdate();//可以加快
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int index = -1;
            if (txtUsagePos.Text == "新建")
            {
                if(currentHistory == null)
                    currentHistory = new CaseHistory();
                currentHistory.HistoryNo = DateTime.Now.ToString("yyyyMMddHHmmss");
                var item = new CListItem(currentHistory.HistoryNo);
                index = txtUsagePos.Properties.Items.Add(item);
            }
            currentHistory.Yijian = txtRemark.Text;
            User.SaveHistorys(currentHistory);
            if(index >= 0)
                txtUsagePos.SelectedIndex = index;
            User.ReadPatients();
        }

        private void txtUsagePos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(txtUsagePos.Text == "新建")
            {
                currentHistory = new CaseHistory();
                gridControl1.DataSource = null;
                txtRemark.Text = string.Empty;
            }
            else
            {
                currentHistory = User.CurrentPatient.Historys.FirstOrDefault(s => s.HistoryNo == txtUsagePos.Text);
                txtRemark.Text = currentHistory.Yijian;
                BindImg();
            }
        }

        private void FrmItemDetail_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void InitGrid()
        {
            this.winExplorerView1.Columns.Clear();            
            this.winExplorerView1.Columns.Add(new GridColumn() { FieldName = "Image" });
            this.winExplorerView1.Columns.Add(new GridColumn() { FieldName = "IsSelect" });
            this.winExplorerView1.ColumnSet.MediumImageColumn = this.winExplorerView1.Columns["Image"];
            this.winExplorerView1.ColumnSet.CheckBoxColumn = this.winExplorerView1.Columns["IsSelect"];            
        }

        private void btnPai_Click(object sender, EventArgs e)
        {
            var form = new FormPicture();
            if (form.ShowDialog() == DialogResult.OK)
            {
                AddImg(form.imgList);
                BindImg();
            }
        }

        private void AddImg(List<ImgInfo> imgList)
        {
            if (currentHistory == null)
                currentHistory = new CaseHistory { imgInfos = new List<ImgInfo>() };

            var index = 1;
            if (currentHistory.imgInfos != null && currentHistory.imgInfos.Count > 0)
            {
                index = currentHistory.imgInfos.Max(m => m.ID) + 1;
            }
            else
                currentHistory.imgInfos = new List<ImgInfo>();
            foreach (var img in imgList)
            {
                img.ID = index++;
                img.FileName = img.ID.ToString();
                //img.Base64 = Help.Base64Util.GetBase64FromImage(img.Img);
                currentHistory.imgInfos.Add(img);
            }
        }

        private void BindImg()
        {
            var homesTable = new DataTable("Homes");
            homesTable.Columns.Add("Index", typeof(int));
            homesTable.Columns.Add("Image", typeof(Image));
            homesTable.Columns.Add("IsSelect", typeof(bool));
            gridControl1.DataMember = null;
            gridControl1.DataSource = null;
            if (currentHistory.imgInfos != null)
            {
                currentHistory.imgInfos.ForEach(f => {
                    //if (f.InfoType == 0)
                    //{
                        var row = homesTable.NewRow();
                        row["Index"] = f.ID;
                        row["Image"] = new Bitmap(f.Img, new Size(351, 234));
                        row["IsSelect"] = 0;
                        homesTable.Rows.Add(row);
                    //}
                });
                DataSet ds = new DataSet();
                ds.Tables.Add(homesTable);
                DataViewManager dvManager = new DataViewManager(ds);
                DataView dv = dvManager.CreateDataView(homesTable);
                gridControl1.DataSource = dv;
            }            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (btnPrint.Text == "选择打印图片")
            {
                this.winExplorerView1.OptionsView.ShowCheckBoxes = true;
                btnPrint.Text = "取消打印图片";
            }
            else
            {
                this.winExplorerView1.OptionsView.ShowCheckBoxes = false;
                btnPrint.Text = "选择打印图片";
            }
        }

        private void btnImgEdit_Click(object sender, EventArgs e)
        {
            var rows = winExplorerView1.GetSelectedRows();
            if(rows.Count() == 0)
            {
                CommHelp.ShowWarning("请选择要编辑的图片");
                return;
            }
            var v = (DataRowView)winExplorerView1.GetRow(rows[0]);
            var img = currentHistory.imgInfos.FirstOrDefault(f => f.ID == Convert.ToInt32(v["Index"]));
            FormImageEdit form = new FormImageEdit(img, currentHistory);
            form.ShowDialog();
            User.ReadCurrentPatientHistorys();
            BindImg();
        }

        private void btnImportImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.Filter = "BMP Bitmap(*.bmp)|*.bmp|CUR Cursor(*.cur)|*.cur|GIF (*.gif)|*.gif|ICO Icon(*.ico)|*.ico|JPG JPEG Image(*.jpg;*.jpeg;*jpe)|*.jpg;*.jpeg;*jpe|PNG Portable NetworkGraphics(*.png)|*.png";
                if (file.ShowDialog() == DialogResult.OK)
                {
                    var img = (Bitmap)Image.FromFile(file.FileName, false);
                    AddImg(new List<ImgInfo> { new ImgInfo { Date = DateTime.Now, Img = img } });
                    BindImg();                   
                }
            }
        }

        public void Print()
        {
            List<DataRowView> rows = new List<DataRowView>();
            for (int i = 0; i < winExplorerView1.RowCount; i++)
            {
                var view = (DataRowView)winExplorerView1.GetRow(i);
                if (Convert.ToBoolean(view["IsSelect"]))
                {
                    rows.Add(view);
                }
            }

            if (rows.Count() > 4)
            {
                CommHelp.ShowWarning("一次只能打印4张图片");
                return;
            }
            if(rows.Count() == 0)
            {
                CommHelp.ShowWarning("请选择要打印的图片");
                return;
            }
            ReportSource source = new ReportSource();
            source.Name = User.CurrentPatient.FName;
            source.Birthday = User.CurrentPatient.BirthDay;
            source.No = User.CurrentPatient.SECU;
            source.Yijian = currentHistory.Yijian;
                        
            source.Img1 = Help.Base64Util.GetBase64FromImage((Bitmap)rows[0]["Image"]);
            if(rows.Count() > 1)
            {                
                source.Img2 = Help.Base64Util.GetBase64FromImage((Bitmap)rows[1]["Image"]);
            }
            if(rows.Count() > 2)
            {                
                source.Img3 = Help.Base64Util.GetBase64FromImage((Bitmap)rows[2]["Image"]);
            }
            if(rows.Count() > 3)
            {
                source.Img4 = Help.Base64Util.GetBase64FromImage((Bitmap)rows[3]["Image"]);
            }

            ReportViewerDialog dlg = new ReportViewerDialog();
            dlg.DataSourceDict = new List<ReportSource>() { source };
            dlg.ReportFilePath = "Report/Report.rdlc";
            dlg.ShowDialog();
        }

        private void FrmItemDetail_Activated(object sender, EventArgs e)
        {
            if(User.CurrentPatient == null || User.Patients.Count == 0)
            {
                var form = new FormPatientEdit();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    InitData();
                }
                else
                    this.Close();
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteCurrentImg();
        }

        private void DeleteCurrentImg()
        {
            var rows = winExplorerView1.GetSelectedRows();
            if (rows.Count() == 0)
            {
                CommHelp.ShowWarning("请选择要删除的图片");
                return;
            }
            var v = (DataRowView)winExplorerView1.GetRow(rows[0]);
            var img = currentHistory.imgInfos.FirstOrDefault(f => f.ID == Convert.ToInt32(v["Index"]));

            currentHistory.imgInfos.Remove(img);
            if (currentHistory.HistoryNo != null)
            {
                var path = Path.Combine(User.DBPath, User.CurrentPatient.FName + User.CurrentPatient.BirthDay, currentHistory.HistoryNo, img.FileName);

                if (File.Exists(path + ".bmp"))
                {
                    File.Delete(path + ".bmp");
                }

                if (File.Exists(path + ".ini"))
                {
                    File.Delete(path + ".ini");
                }
                User.SaveHistorys(currentHistory);
            }
            winExplorerView1.DeleteSelectedRows();
        }
    }
}
