using DentalImaging.Help;
using DentalImaging.Model;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DentalImaging.NewForm
{
    public partial class FormHistory : XtraForm
    {
        List<Patient> Patients = new List<Patient>();

        public FormHistory()
        {
            InitializeComponent();
            LoadData();
            BandData();
            
            gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            //gridView1.OptionsView.ColumnAutoWidth = false;

            
            //gridView2.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            gridView2.OptionsView.ColumnAutoWidth = true;
        }

        private void LoadData()
        {
            Patients = User.Patients;            
            Patients.ForEach(f =>
            {
                if(f.Historys != null) f.Historys.Clear();
                var patPath = $"{User.DBPath}\\{f.FName + f.BirthDay}";
                if(Directory.Exists(patPath))
                {
                    var dirs = Directory.GetDirectories(patPath);
                    foreach (var dir in dirs)
                    {
                        var dirInfo = new DirectoryInfo(dir);
                        var str = File.ReadAllText($"{dir}\\{dirInfo.Name}.ini");
                        CaseHistory history = str.ToObject<CaseHistory>();
                        history.imgInfos = new List<ImgInfo>();

                        var files = Directory.GetFiles(dir, "*.ini");
                        
                        foreach (var file in files)
                        {
                            str = File.ReadAllText(file);
                            FileInfo finfo = new FileInfo(file);

                            if (finfo.Name != $"{dirInfo.Name}.ini")
                            {
                                var imgInfo = str.ToObject<ImgInfo>();
                                history.imgInfos.Add(imgInfo);
                            }
                        }
                        if (f.Historys == null)
                            f.Historys = new List<CaseHistory>();
                        f.Historys.Add(history);
                    }
                }                
            });
        }

        private void BandData()
        {
            string tableColumns = "序号,诊断编号,姓名,出生日期,社保号,病情描述";
            DataTable dt = CreateTable(tableColumns);
            DataRow dr = null;
            foreach (var info in Patients.OrderByDescending(o => o.Number))
            {
                if(info.Historys != null)
                {

                    foreach (var h in info.Historys)
                    {
                        if(dt.Select($"{GetText("诊断编号")}='{h.HistoryNo}'").Count() == 0)
                        {
                            dr = dt.NewRow();                            
                            dr[GetText("序号")] = info.Number;
                            dr[GetText("诊断编号")] = h.HistoryNo;
                            dr[GetText("姓名")] = info.FName;
                            dr[GetText("出生日期")] = info.BirthDay;
                            dr[GetText("社保号")] = info.SECU;
                            dr[GetText("病情描述")] = h.Yijian;
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    dr = dt.NewRow();
                    dr[GetText("序号")] = info.Number;
                    dr[GetText("诊断编号")] = "";
                    dr[GetText("姓名")] = info.FName;
                    dr[GetText("出生日期")] = info.BirthDay;
                    dr[GetText("社保号")] = info.SECU;
                    dr[GetText("病情描述")] = "";
                    dt.Rows.Add(dr);
                }
            }
            
            this.gridControl1.DataSource = dt.DefaultView;
            gridView1.BestFitColumns();
            gridView1.Columns.Last().Width = 240;
        }

        private void BandImgInfo(int number, string HistoryNo)
        {
            this.gridControl2.DataSource = null;
            var patient= Patients.FirstOrDefault(s => s.Number == number);
            if(patient != null)
            {
                if (patient.Historys == null)
                    return;
                var history = patient.Historys.FirstOrDefault(w => w.HistoryNo == HistoryNo);
                if(history != null)
                {
                    string tableColumns = "时间,图片路径";
                    DataTable dt = CreateTable(tableColumns);
                    DataRow dr;
                    foreach (var i in history.imgInfos.OrderByDescending(o => o.Date))
                    {
                        dr = dt.NewRow();
                        dr[GetText("时间")] = i.Date;
                        if(i.InfoType == 1)
                        {
                            dr[GetText("图片路径")] = i.VideoPath;
                        }
                        else
                            dr[GetText("图片路径")] = $"{User.DBPath}\\{patient.FName + patient.BirthDay}\\{HistoryNo}\\{i.FileName}.bmp";
                        dt.Rows.Add(dr);
                    }
                    this.gridControl2.DataSource = dt.DefaultView;
                    gridView2.BestFitColumns();
                }
            }
        }

        private DataTable CreateTable(string columns)
        {
            var cols = columns.Split(',');
            DataTable dt = new DataTable();
            foreach(var col in cols)
            {
                dt.Columns.Add(new DataColumn(LanguageHelp.GetTextLanguage(col), typeof(string)));
            }
            return dt;
        }

        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            SelectedRow();
        }

        private void SelectedRow()
        {
            var rows = gridView1.GetSelectedRows();
            var v = (DataRowView)gridView1.GetRow(rows[0]);
            txtID.Text = v.Row.ItemArray[0].ToString();
            textName.Text = v.Row.ItemArray[2].ToString();
            txtBirthDay.Text = v.Row.ItemArray[3].ToString();
            txtNo.Text = v.Row.ItemArray[4].ToString();
            txtYijiang.Text = v.Row.ItemArray[5].ToString();
            txtHistoryNo.Text = v.Row.ItemArray[1].ToString();

            BandImgInfo(Convert.ToInt32(txtID.Text), v.Row.ItemArray[1].ToString());
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var p1 = User.Patients.FirstOrDefault(f => f.Number.ToString() == txtID.Text);
            var h1 = p1.Historys.FirstOrDefault(f => f.HistoryNo == txtHistoryNo.Text);
            h1.Yijian = txtYijiang.Text;
            p1.BirthDay = txtBirthDay.Text;
            p1.Name = textName.Text;
            p1.SECU = txtNo.Text;

            var rows = gridView1.GetSelectedRows();
            var v = (DataRowView)gridView1.GetRow(rows[0]);
            v.Row.ItemArray[2] = textName.Text;
            v.Row.ItemArray[3] = txtBirthDay.Text;
            v.Row.ItemArray[4] = txtNo.Text;
            v.Row.ItemArray[5] = txtYijiang.Text;
            
            File.WriteAllText($"{User.DBPath}/us.ini", User.Patients.ToJson());
            File.WriteAllText($"{User.DBPath}/{p1.FName + p1.BirthDay}/{h1.HistoryNo}/{h1.HistoryNo}.ini", h1.ToJson());
            User.ReadPatients();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CommHelp.ShowYesNoAndWarning("所有图像都将被删除") == DialogResult.Yes)
            {
                var Number = txtID.Text;
                if (Directory.Exists(User.DBPath + "//" + Number))
                {
                    Directory.Delete(User.DBPath + "//" + Number, true);
                }
                var index = User.Patients.FindIndex(f => f.Number.ToString() == Number);
                User.Patients.RemoveAt(index);
                if (User.CurrentPatient.Number.ToString() == Number)
                    User.CurrentPatient = null;

                File.WriteAllText($"{User.DBPath}/us.ini", User.Patients.ToJson());

                var count = gridView1.RowCount;
                for(int i = 0;i < count;)
                {
                    DataRow dr = gridView1.GetDataRow(i);
                    if (dr != null)
                    {
                        if (dr[GetText("序号")].ToString() == Number)
                        {
                            gridView1.DeleteRow(i);
                        }
                        else
                            i++;
                    }
                    else
                        i++;                    
                }
            }
        }

        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            ShowImg();
        }

        private void ShowImg()
        {
            var rows = gridView2.GetSelectedRows();
            var v = (DataRowView)gridView2.GetRow(rows[0]);
            var path = v.Row.ItemArray[1].ToString(); //.Replace(".ini", ".bmp");
            System.Diagnostics.Process.Start(path);
        }

        private void FormHistory_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }

        private string GetText(string text)
        {
            return LanguageHelp.GetTextLanguage(text);
        }
    }
}
