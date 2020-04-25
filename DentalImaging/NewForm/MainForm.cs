using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.LookAndFeel;
using DentalImaging;
using DentalImaging.Model;
using System.Configuration;
using DentalImaging.Help;
using DentalImaging.NewForm;
using System.IO;

namespace WareHouseMis.UI
{
    public partial class MainForm : RibbonForm
    {
        #region 属性变量
        //private AppConfig config = new AppConfig();
        
        //全局热键
        //private RegisterHotKeyHelper hotKey2 = new RegisterHotKeyHelper();

        string dirName = User.DBPath;

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }
        /// <summary>
        /// Sets command status
        /// </summary>
        public string CommandStatus
        {
            get { return lblCommandStatus.Caption; }
            set { lblCommandStatus.Caption = value; }
        }
        /// <summary>
        /// Sets user status
        /// </summary>
        public string UserStatus
        {
            get { return lblCurrentUser.Caption; }
            set { lblCurrentUser.Caption = value; }
        }
        #endregion

        public MainForm()
        {
            InitializeComponent();

            User.DBPath = ConfigurationManager.AppSettings["DBPath"];//Environment.CurrentDirectory + "\\Patient";// 
            dirName = User.DBPath;
            User.ReadPatients();
            InitUserRelated();
            //var form = new FormPatientEdit();
            //if (form.ShowDialog() == DialogResult.OK)
            //{
            if (User.IsRegist)
                LoadMdiForm(this, typeof(FormPatientEdit));
            //}

            if (string.IsNullOrEmpty(User.DBPath))
            {
                User.DBPath = "Patient";
            }

            InitSkinGallery();
            UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");

            if (User.Patients == null)
            {
                User.Patients = new List<Patient>();
            }

            ribbonControl.Enabled = User.IsRegist;

            if(User.Language == "cn")
            {
                barCheckItem1.Checked = true;
                barCheckItem2.Checked = false;
            }
            else
            {
                barCheckItem1.Checked = false;
                barCheckItem2.Checked = true;
            }
            barCheckItem1.CheckedChanged += barCheckItem1_CheckedChanged;
            barCheckItem2.CheckedChanged += barCheckItem2_CheckedChanged;
        }        

        /// <summary>
        /// 初始化用户相关的系统信息
        /// </summary>
        private void InitUserRelated()
        {
            #region 初始化系统名称
            try
            {
                string Manufacturer = "";
                string ApplicationName = this.Text; //GetText(ConfigurationManager.AppSettings["ApplicationName"]);
                if (!User.IsRegist)
                {
                    Manufacturer = GetText("软件未注册");
                }
                if(User.Date <= 30)
                {
                    Manufacturer += $"({GetText("试用期")} {User.Date}{GetText("天")})";
                }
                string AppWholeName = string.Format("{0}{1}", !string.IsNullOrEmpty(Manufacturer) ? Manufacturer+"-" : "", ApplicationName);
                Portal.gc.gAppUnit = Manufacturer;
                Portal.gc.gAppMsgboxTitle = AppWholeName;
                Portal.gc.gAppWholeName = AppWholeName;

                this.Text = AppWholeName;

                //UserStatus = string.Format("{1}：{0}", User.CurrentPatient.FName, GetText("当前用户"));
                //CommandStatus = string.Format("{1} {0}", Portal.gc.gAppWholeName, GetText("欢迎使用"));
            }
            catch { }

            #endregion
        }        

        void InitSkinGallery()
        {
            DevExpress.XtraBars.Helpers.SkinHelper.InitSkinGallery(rgbiSkins, true);
            this.ribbonControl.Toolbar.ItemLinks.Add(rgbiSkins);
        }

        #region 托盘菜单操作

        private void MainForm_MaximizedBoundsChanged(object sender, EventArgs e)
        {
            this.Hide();
        }        

        #endregion        

        private void MainForm_Load(object sender, EventArgs e)
        {
            Init();
        }        

        private void Init()
        {
            this.lblCalendar.Caption = System.DateTime.Now.ToString();//  cal.GetDateInfo(System.DateTime.Now).Fullinfo;
        }        

        private void tool_ItemDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {            
            foreach(var form in this.MdiChildren)
            {
                if(form.GetType() == typeof(FrmItemDetail))
                {
                    ((FrmItemDetail)form).Print();
                }
            }
        }

        private void tool_StockSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadMdiForm(this, typeof(FormHistory));
        }

        private void tool_Purchase_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadMdiForm(this, typeof(FormPatientEdit));
        }

        private void tool_TakeOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(User.CurrentPatient == null)
            {
                CommHelp.ShowWarning("请选择需要拍摄的患者");
            }
            else
            {
                var form = new FormPicture();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadMdiForm(this, typeof(FormPatientEdit));
                    if(form.imgList.Count > 0)
                    {
                        foreach (var f in this.MdiChildren)
                        {
                            if (f.GetType() == typeof(FormPatientEdit))
                            {
                                ((FormPatientEdit)f).SaveImgToHistory(form.imgList);
                            }
                        }
                    }
                }
            }
        }

        private void LoadMdiForm(Form mainForm, Type formType)
        {
            Form[] mdiChildren = mainForm.MdiChildren;
            Form form = null;

            foreach (var child in mdiChildren)
            {
                if (child.GetType() == formType)
                {
                    form = child;
                    break;
                }
            }

            if (form == null)
            {
                form = (Form)Activator.CreateInstance(formType);
                form.MdiParent = mainForm;
                form.Show();
            }

            form.BringToFront();
            form.Activate();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
            Portal.glExitApp = true;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(barCheckItem1.Checked)
            {
                barCheckItem2.Checked = !barCheckItem1.Checked;
                User.Language = barCheckItem1.Tag.ToString();
                LanguageHelp.ReLoadLanguage();

                LanguageHelp.InitControlLanguage(this);
                Form[] mdiChildren = this.MdiChildren;
                foreach (var child in mdiChildren)
                {                    
                    var form = child as FormPatientEdit;
                    if (form != null)
                    {
                        form.Close();
                        LoadMdiForm(this, typeof(FormPatientEdit));
                    }
                    else
                    {
                        LanguageHelp.InitControlLanguage(child);
                    }
                    child.Refresh();
                }
                SaveLanguage(User.Language);
            }            
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barCheckItem2.Checked)
            {
                barCheckItem1.Checked = !barCheckItem2.Checked;
                User.Language = barCheckItem2.Tag.ToString();
                LanguageHelp.ReLoadLanguage();

                LanguageHelp.InitControlLanguage(this);
                Form[] mdiChildren = this.MdiChildren;
                foreach (var child in mdiChildren)
                {
                    var form = child as FormPatientEdit;
                    if (form != null)
                    {
                        form.Close();
                        LoadMdiForm(this, typeof(FormPatientEdit));
                    }
                    else
                    {
                        LanguageHelp.InitControlLanguage(child);
                    }
                    child.Refresh();                    
                }
                SaveLanguage(User.Language);
            }
        }

        private string GetText(string text)
        {
            return LanguageHelp.GetTextLanguage(text);
        }

        private void SaveLanguage(string language)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["Language"].Value = language;
            configuration.Save();
        }

        private void tool_FilePath_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = GetText("请选择保存的目录");
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        CommHelp.ShowWarning("文件夹路径不能为空");
                        return;
                    }
                    if(dialog.SelectedPath != User.DBPath)
                    {
                        var yesno = CommHelp.ShowYesNoAndTips("选择了新的目录，是否将原目录下的文件移动到新目录，请确保原目录下的文件没有被占用，如移动失败请手动移动");
                        if(yesno == DialogResult.Yes)
                        {
                            MoveFolder(User.DBPath, dialog.SelectedPath);
                            SaveDbPath(dialog.SelectedPath);
                            CommHelp.ShowTips("设置成功");
                        }
                    }
                }
            }
        }

        public bool MoveFolder(string sourceFolder, string targetFolder)
        {
            // 检查目标目录是否以目录分割字符结束,如果不是则添加
            if (targetFolder[targetFolder.Length - 1] != Path.DirectorySeparatorChar)
            {
                targetFolder += Path.DirectorySeparatorChar;
            }
            if (sourceFolder[sourceFolder.Length - 1] != Path.DirectorySeparatorChar)
            {
                sourceFolder += Path.DirectorySeparatorChar;
            }

            List<string> fileNames = new List<string>();

            if (Directory.Exists(sourceFolder))
            {
                //得到所有文件路径
                fileNames.AddRange(Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories));
                if (fileNames.Count > 0)
                {
                    //同盘符
                    if (sourceFolder[0].Equals(targetFolder[0]))
                    {
                        if (Directory.Exists(targetFolder))
                        {
                            Directory.Delete(targetFolder, true);
                        }
                        Directory.Move(sourceFolder, targetFolder);
                        return true;
                    }
                    else
                    {
                        if (!Directory.Exists(targetFolder))
                        {
                            Directory.CreateDirectory(targetFolder);
                        }
                        DirectoryInfo dir = new DirectoryInfo(sourceFolder);
                        DirectoryInfo[] folders = dir.GetDirectories("*", SearchOption.AllDirectories);

                        for (int i = 0; i < folders.Length; i++)
                        {
                            string name = folders[i].FullName.Replace(sourceFolder, targetFolder);
                            if (!Directory.Exists(name))
                            {
                                Directory.CreateDirectory(name);
                            }
                        }

                        foreach (string filename in fileNames)
                        {
                            string fn = filename.Replace(sourceFolder, targetFolder);
                            if (File.Exists(fn))
                            {
                                File.Delete(fn);
                            }
                            File.Move(filename, filename.Replace(sourceFolder, targetFolder));
                        }
                        //删除空的目录结构
                        Directory.Delete(sourceFolder, true);
                        return true;
                    }
                }
            }
            return false;
        }

        private void SaveDbPath(string path)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["DBPath"].Value = path;
            configuration.Save();
        }
    }
}
