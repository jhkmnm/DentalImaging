using DentalImaging.Help;
using DentalImaging.Model;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DentalImaging.新界面
{
    public partial class FormPatientEdit : XtraForm
    {
        private ActionType currentAction;
        string dirName = User.DBPath;
        bool isSearch = false;       

        public FormPatientEdit()
        {
            InitializeComponent();
            SetControl(ActionType.None);
            InitData();
            InitDataGrid();
        }

        private void SetControl(ActionType type)
        {
            currentAction = type;
            txtID.Enabled = false;
            switch(type)
            {
                case ActionType.Add:
                case ActionType.Edit:
                    txtName.Enabled = true;
                    txtBirthDay.Enabled = true;
                    txtNo.Enabled = true;
                    txtLink.Enabled = true;
                    txtAdress.Enabled = true;
                    isSearch = false;
                    break;
                case ActionType.Search:
                    txtName.Enabled = true;
                    txtNo.Enabled = true;
                    isSearch = true;
                    break;
                default:
                    txtName.Enabled = false;
                    txtBirthDay.Enabled = false;
                    txtNo.Enabled = false;
                    txtLink.Enabled = false;
                    txtAdress.Enabled = false;
                    break;
            }

            switch(type)
            {
                case ActionType.Add:                
                case ActionType.Search:
                    txtName.Text = "";
                    txtBirthDay.Text = "";
                    txtNo.Text = "";
                    txtLink.Text = "";
                    txtAdress.Text = "";
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SetControl(ActionType.Add);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName.Text))
            {
                CommHelp.ShowTips("请选择要修改的账号");
                return;
            }
            SetControl(ActionType.Edit);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SetControl(ActionType.Search);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(currentAction == ActionType.Add || currentAction == ActionType.Edit)
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    CommHelp.ShowTips("必须输入姓名");
                    return;
                }
                else if(string.IsNullOrEmpty(txtBirthDay.Text))
                {
                    CommHelp.ShowTips("必须输入生日");
                    return;
                }
                else
                {
                    var v = new Patient { FName = txtName.Text, Name = "", Address1 = txtAdress.Text, Address2 = "", Address3 = "", BirthDay = txtBirthDay.Text, SECU = txtNo.Text, Tel1 = txtLink.Text, Tel2 = ""};
                    if (currentAction == ActionType.Add)
                    {
                        v.Number = CreateNum();
                        if (User.Patients == null)
                            User.Patients = new List<Patient>();
                        User.Patients.Add(v);
                    }
                    else
                    {
                        v.Number = User.CurrentPatient.Number;
                        if(v.FName != User.CurrentPatient.FName || v.BirthDay != User.CurrentPatient.BirthDay)
                        {
                            try
                            {
                                Directory.Move(User.PatientPath, $"{User.DBPath}\\{v.FName + v.BirthDay}");
                            }
                            catch(Exception ex)
                            {
                                CommHelp.ShowError($"修改信息失败，请检查{User.PatientPath}没有被占用");
                                return;
                            }
                        }
                        var index = User.Patients.FindIndex(f => f.Number == v.Number);
                        User.CurrentPatient = v;
                        User.Patients[index] = v;
                    }

                    SavePatients();
                    InitData();
                    SetControl(ActionType.None);
                }
            }            
        }

        private int CreateNum()
        {
            var num = 1;
            if(User.Patients != null && User.Patients.Count > 0)
            {
                num = User.Patients.Max(m => m.Number) + 1;
            }
            return num;
        }

        private void SavePatients()
        {
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllText($"{dirName}/us.ini", User.Patients.ToJson());
        }

        List<Patient> dataSource;

        private void InitData()
        {
            dataSource = User.Patients;
            if (!string.IsNullOrEmpty(txtName.Text) && isSearch)
            {
                dataSource = User.Patients.Where(w => w.FName.Contains(txtName.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(txtBirthDay.Text) && isSearch)
            {
                dataSource = User.Patients.Where(w => w.BirthDay.Contains(txtBirthDay.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(txtNo.Text) && isSearch)
            {
                dataSource = User.Patients.Where(w => w.SECU.Contains(txtNo.Text)).ToList();
            }
            BandData();
        }

        private void BandData()
        {
            string tableColumns = "序号,姓名,出生日期,社保号,联系方式,地址";
            DataTable dt = CreateTable(tableColumns);
            DataRow dr = null;
            if (dataSource != null)
            {
                foreach (var info in dataSource.OrderBy(o => o.Number))
                {
                    dr = dt.NewRow();
                    dr["序号"] = info.Number;
                    dr["姓名"] = info.FName;
                    dr["出生日期"] = info.BirthDay;
                    dr["社保号"] = info.SECU;
                    dr["联系方式"] = info.Tel1;
                    dr["地址"] = info.Address1;
                    dt.Rows.Add(dr);
                }
            }
            this.gridControl1.DataSource = dt.DefaultView;
            for(int i=0;i< dt.Columns.Count;i++)
            {
                gridView1.Columns[i].OptionsFilter.AllowAutoFilter = false;
                gridView1.Columns[i].OptionsFilter.AllowFilter = false;
                gridView1.Columns[i].OptionsFilter.ImmediateUpdateAutoFilter = false;
            }            
        }

        private DataTable CreateTable(string columns)
        {
            var cols = columns.Split(',');
            DataTable dt = new DataTable();
            foreach (var col in cols)
            {
                dt.Columns.Add(new DataColumn(col, typeof(string)));
            }
            return dt;
        }

        private void SelectedUser()
        {
            isSearch = false;
            var rows = gridView1.GetSelectedRows();
            var v = (DataRowView)gridView1.GetRow(rows[0]);
            txtID.Text = v[0].ToString();
            txtName.Text = v[1].ToString();
            txtBirthDay.Text = v[2].ToString();
            txtNo.Text = v[3].ToString();
            txtLink.Text = v[4].ToString();
            txtAdress.Text = v[5].ToString();
            User.CurrentPatient = new Patient { Number = Convert.ToInt32(txtID.Text), FName = txtName.Text, BirthDay = txtBirthDay.Text, SECU = txtNo.Text, Tel1 = txtLink.Text, Address1 = txtAdress.Text };

            User.ReadCurrentPatientHistorys();
            BindImg();
        }        

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            SelectedUser();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if(User.Patients != null && isSearch)
            {
                InitData();
            }
        }

        private void txtBirthDay_EditValueChanged(object sender, EventArgs e)
        {
            if (User.Patients != null && isSearch)
            {
                InitData();
            }
        }

        private void txtNo_TextChanged(object sender, EventArgs e)
        {
            if (User.Patients != null && isSearch)
            {
                InitData();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (User.CurrentPatient != null)
            {
                if (CommHelp.ShowYesNoAndWarning("所有图像都将被删除") == DialogResult.Yes)
                {
                    var Number = User.CurrentPatient.Number;
                    if (Directory.Exists(dirName + "//" + Number))
                    {
                        Directory.Delete(dirName + "//" + Number, true);
                    }
                    var index = User.Patients.FindIndex(f => f.Number == Number);
                    User.Patients.RemoveAt(index);
                    User.CurrentPatient = null;
                    SavePatients();
                    InitData();
                }
            }
            else
            {
                CommHelp.ShowTips("请选择病人！");
            }
        }

        enum ActionType
        {
            Add,
            Edit,
            Search,
            None
        }

        private void InitDataGrid()
        {
            this.winExplorerView1.Columns.Clear();
            this.winExplorerView1.Columns.Add(new GridColumn() { FieldName = "Index" });
            this.winExplorerView1.Columns.Add(new GridColumn() { FieldName = "Image" });
            this.winExplorerView1.Columns.Add(new GridColumn() { FieldName = "InfoType" });
            this.winExplorerView1.Columns.Add(new GridColumn() { FieldName = "HistoryNo" });
            this.winExplorerView1.Columns.Add(new GridColumn() { FieldName = "VideoPath" });

            this.winExplorerView1.ColumnSet.MediumImageColumn = this.winExplorerView1.Columns["Image"];
            this.winExplorerView1.ColumnSet.GroupColumn = this.winExplorerView1.Columns["HistoryNo"];
            this.winExplorerView1.OptionsView.ShowExpandCollapseButtons = true;
        }

        private void BindImg()
        {
            var homesTable = new DataTable("Homes");
            homesTable.Columns.Add("Index", typeof(int));
            homesTable.Columns.Add("Image", typeof(Image));
            homesTable.Columns.Add("InfoType", typeof(int));
            homesTable.Columns.Add("HistoryNo", typeof(string));
            homesTable.Columns.Add("VideoPath", typeof(string));
            gridControl2.DataMember = null;
            gridControl2.DataSource = null;
            if(User.CurrentPatient.Historys != null)
            {
                User.CurrentPatient.Historys.ForEach(h => {
                    if(h.imgInfos != null)
                    {
                        Image img;
                        foreach(var f in h.imgInfos.OrderBy(o => o.ID))
                        {
                            img = Image.FromFile(f.ImgPath);
                            var row = homesTable.NewRow();
                            row["Index"] = f.ID;
                            row["Image"] = new Bitmap(img, new Size(351, 234));
                            row["InfoType"] = f.InfoType;
                            row["HistoryNo"] = h.HistoryNo;
                            row["VideoPath"] = f.VideoPath;
                            homesTable.Rows.Add(row);
                            img.Dispose();
                        };
                    }
                });
            }

            if(homesTable.Rows.Count > 0)
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(homesTable);
                DataViewManager dvManager = new DataViewManager(ds);
                DataView dv = dvManager.CreateDataView(homesTable);
                gridControl2.DataSource = dv;
            }
        }

        public void SaveImgToHistory(List<ImgInfo> imgList)
        {
            var historyNo = DateTime.Now.ToString("yyyyMMdd");
            CaseHistory currentHistory = User.CurrentPatient.Historys.FirstOrDefault(f => f.HistoryNo == historyNo);
            if (currentHistory == null)
                currentHistory = new CaseHistory { HistoryNo = historyNo, imgInfos = new List<ImgInfo>() };

            var index = 0;
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
                currentHistory.imgInfos.Add(img);
            }

            User.SaveHistorys(currentHistory);
            User.ReadPatients();
            User.ReadCurrentPatientHistorys();
            BindImg();
        }

        private void winExplorerView1_ItemDoubleClick(object sender, DevExpress.XtraGrid.Views.WinExplorer.WinExplorerViewItemDoubleClickEventArgs e)
        {
            var v = (DataRowView)winExplorerView1.GetRow(e.ItemInfo.Row.RowHandle);
            var history = User.CurrentPatient.Historys.FirstOrDefault(f => f.HistoryNo == v["HistoryNo"].ToString());
            var img = history.imgInfos.FirstOrDefault(f => f.ID == Convert.ToInt32(v["Index"]));
            FormImageEdit form = new FormImageEdit(img, history);
            form.ShowDialog();
            User.ReadCurrentPatientHistorys();
            BindImg();
        }

        private void winExplorerView1_ItemClick(object sender, DevExpress.XtraGrid.Views.WinExplorer.WinExplorerViewItemClickEventArgs e)
        {
            if(e.MouseInfo.Button == MouseButtons.Left)
            {
                var v = (DataRowView)winExplorerView1.GetRow(e.ItemInfo.Row.RowHandle);
                var path = v.Row.ItemArray[4].ToString(); //.Replace(".ini", ".bmp");
                if (!string.IsNullOrEmpty(path))
                    System.Diagnostics.Process.Start(path);
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
            var history = User.CurrentPatient.Historys.FirstOrDefault(f => f.HistoryNo == v["HistoryNo"].ToString());
            var img = history.imgInfos.FirstOrDefault(f => f.ID == Convert.ToInt32(v["Index"]));

            history.imgInfos.Remove(img);
            if (history.HistoryNo != null)
            {
                var path = Path.Combine(User.DBPath, User.CurrentPatient.FName + User.CurrentPatient.BirthDay, history.HistoryNo, img.FileName);

                if (File.Exists(path + ".bmp"))
                {
                    File.Delete(path + ".bmp");
                }

                if (File.Exists(path + ".ini"))
                {
                    File.Delete(path + ".ini");
                }
                if (File.Exists(img.VideoPath))
                {
                    File.Delete(img.VideoPath);
                }
                User.SaveHistorys(history);
            }
            User.ReadCurrentPatientHistorys();
            BindImg();
        }
    }    
}
