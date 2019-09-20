using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;
using DevExpress.LookAndFeel;
using DentalImaging;
using DentalImaging.新界面;
using DentalImaging.Model;
using System.Configuration;
using DentalImaging.Help;

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

            User.DBPath = Environment.CurrentDirectory + "\\Patient";// ConfigurationManager.AppSettings["DBPath"];
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

            //SplashScreen.Splasher.Close();

            ribbonControl.Enabled = User.IsRegist;
        }        

        /// <summary>
        /// 初始化用户相关的系统信息
        /// </summary>
        private void InitUserRelated()
        {
            #region 初始化系统名称
            try
            {
                string Manufacturer = ConfigurationManager.AppSettings["Manufacturer"];
                string ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
                if (!User.IsRegist)
                {
                    Manufacturer = "软件未注册";
                }
                if(User.Date <= 30)
                {
                    ApplicationName += $"(试用期 {User.Date}天)";
                }
                string AppWholeName = string.Format("{0}-{1}", Manufacturer, ApplicationName);
                Portal.gc.gAppUnit = Manufacturer;
                Portal.gc.gAppMsgboxTitle = AppWholeName;
                Portal.gc.gAppWholeName = AppWholeName;

                this.Text = AppWholeName;

                UserStatus = string.Format("当前用户：{0}", User.CurrentPatient.FName);
                CommandStatus = string.Format("欢迎使用 {0}", Portal.gc.gAppWholeName);
            }
            catch { }

            #endregion
        }        

        void InitSkinGallery()
        {
            DevExpress.XtraBars.Helpers.SkinHelper.InitSkinGallery(rgbiSkins, true);
            this.ribbonControl.Toolbar.ItemLinks.Add(rgbiSkins);
        }

        public void CloseAllDocuments()
        {
        }

        #region 托盘菜单操作

        private void notifyMenu_About_Click(object sender, EventArgs e)
        {
            Portal.gc.About();
        }

        private void notifyMenu_Show_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
                this.Show();
                this.BringToFront();
                this.Activate();
                this.Focus();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
        }

        private void notifyMenu_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                this.ShowInTaskbar = false;
                Portal.gc.Quit();
            }
            catch
            {
                // Nothing to do.
            }
        }

        private void MainForm_MaximizedBoundsChanged(object sender, EventArgs e)
        {
            this.Hide();
        }        

        #endregion

        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Exit(object sender, EventArgs args)
        {
            DialogResult dr = CommHelp.ShowYesNoAndTips("点击“Yes”退出系统，点击“No”返回");

            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
            else if (dr == DialogResult.No)
            {
                return;
            }
        }

        

        private void MainForm_Load(object sender, EventArgs e)
        {
            Init();
        }        

        private void Init()
        {
            this.lblCalendar.Caption = System.DateTime.Now.ToString();//  cal.GetDateInfo(System.DateTime.Now).Fullinfo;
        }


        #region 工具条操作
        private void barItemExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Exit(null, null);
        }

        private void btnHelp_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Portal.gc.Help();
        }

        private void btnAbout_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Portal.gc.About();
        }        

        #endregion        

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
    }
}
