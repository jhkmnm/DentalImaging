using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DentalImaging;
using DentalImaging.Help;

namespace WareHouseMis.UI
{
    public class GlobalControl
    {
        #region 系统全局变量
        public MainForm MainDialog;
        public List<CListItem> ManagedWareHouse = new List<CListItem>();

        public string Login_Name_Key = "WareHouseMis_LoginName";
        public string gAppMsgboxTitle = string.Empty;   //程序的对话框标题
        public string gAppUnit = string.Empty; //单位名称
        public string gAppWholeName = "";

        #endregion

        #region 基本操作函数        
        /// <summary>
        /// 退出系统
        /// </summary>
        public void Quit()
        {
            if (Portal.gc.MainDialog != null)
            {
                Portal.gc.MainDialog.Hide();
                Portal.gc.MainDialog.CloseAllDocuments();
            }

            Application.Exit();
        }

        /// <summary>
        /// 打开帮助文档
        /// </summary>
        public void Help()
        {
            try
            {
                const string helpfile = "Help.chm";
                Process.Start(helpfile);
            }
            catch (Exception)
            {
                CommHelp.ShowWarning("文件打开失败");
            }
        }

        /// <summary>
        /// 关于对话框信息
        /// </summary>
        public void About()
        {
            AboutBox dlg = new AboutBox();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.ShowDialog();
        }        

        #endregion

    }
}
