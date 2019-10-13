using System;
using System.Reflection;
using System.Configuration;
using DentalImaging.Help;

namespace WareHouseMis.UI
{
    partial class AboutBox : DevExpress.XtraEditors.XtraForm
    {
        public AboutBox()
        {
            InitializeComponent();            

            #region 初始化系统名称
            try
            {                
                string Manufacturer = ConfigurationManager.AppSettings["Manufacturer"];
                string ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
                this.Text = string.Format("{0}-{1}", Manufacturer, ApplicationName);
                this.labelProductName.Text = ApplicationName;
                this.labelVersion.Text = String.Format("{1} {0}", AssemblyVersion, LanguageHelp.GetTextLanguage("版本"));
                this.textBoxDescription.Text = "Dental Intraoral Camera,  Jun  2018 是Bangvo 注册的专利技术";
            }
            catch { }

            #endregion 
        }

        #region 程序集属性访问器
        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        #endregion

        private void AboutBox_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }
    }
}
