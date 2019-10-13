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
    }
}
