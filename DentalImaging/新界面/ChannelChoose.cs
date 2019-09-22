using DentalImaging.Help;
using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging.新界面
{
    public partial class ChannelChoose : DevExpress.XtraEditors.XtraForm
    {
        WifiHelp wifiHelp;
        List<WifiInfo> wifiInfo;
        string filePath = Environment.CurrentDirectory + "\\wifi.ini";

        public ChannelChoose()
        {
            InitializeComponent();
            
            if(File.Exists(filePath))
            {
                var str = File.ReadAllText(Environment.CurrentDirectory + "\\wifi.ini");
                wifiInfo = str.ToObject<List<WifiInfo>>();
            }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if (rbtUSB.Checked)
            {
                User.LinkType = LinkType.USB;
                this.DialogResult = DialogResult.OK;
            }
            else if (rbtWIFI.Checked)
            {
                panel2.Location = panel1.Location;
                panel2.Visible = true;
                panel1.Visible = false;

                wifiHelp = new WifiHelp();                
                LoadWifiHotSpot();
            }
            else
            {
                CommHelp.ShowTips(label1.Text);
            }
        }

        private void LoadWifiHotSpot()
        {
            wifiHelp.ssids.Clear();
            wifiHelp.ScanSSID();
            var wifiList = wifiHelp.ssids;
            lbxWIFI.Items.Clear();
            List<string> ssids = new List<string>();
            foreach (var item in wifiList)
            {
                if (item.SSID.Contains("Bangvo") && !ssids.Any(w => w == item.SSID.ToString()))
                {
                    lbxWIFI.Items.Add(item.SSID);
                }
            }
        }

        private void btnRef_Click(object sender, System.EventArgs e)
        {
            LoadWifiHotSpot();
        }

        private void btnLink_Click(object sender, System.EventArgs e)
        {
            var ssid = lbxWIFI.SelectedItem.ToString();
            var password = "";
            if(wifiInfo != null && wifiInfo.Any(w => w.SSID == ssid))
            {
                password = Encoding.UTF8.GetString(Convert.FromBase64String(wifiInfo.FirstOrDefault(w => w.SSID == ssid).Password));                
            }
            bool inputPassword = false;
            if (string.IsNullOrEmpty(password))
            {
                WifiPassword form = new WifiPassword();
                if (form.ShowDialog() == DialogResult.OK)
                {
                    password = form.Password;
                    if (wifiInfo == null) wifiInfo = new List<WifiInfo>();
                    wifiInfo.Add(new WifiInfo { SSID = ssid, Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password))});
                    inputPassword = true;
                }
            }

            if (!string.IsNullOrEmpty(password))
            {
                WIFISSID wfssid = null;
                foreach (var item in wifiHelp.ssids)
                {
                    if (ssid == item.SSID)
                    {
                        wfssid = item;
                        break;
                    }
                }
                wifiHelp.SetWifi(wfssid, password);
                var result = wifiHelp.ConnectToSSID();
                if (string.IsNullOrEmpty(result))
                {
                    User.LinkType = LinkType.WIFI;
                    this.DialogResult = DialogResult.OK;
                    if(inputPassword)
                    {
                        SaveWifiInfo();
                    }
                }
                else
                {
                    CommHelp.ShowError(result);
                    this.DialogResult = DialogResult.No;
                }
            }
        }

        /// <summary>
        /// 保存病历
        /// </summary>
        /// <param name="history"></param>
        private void SaveWifiInfo()
        {
            File.WriteAllText(filePath, wifiInfo.ToJson());
        }
    }
}