using DentalImaging.Help;
using DentalImaging.Model;
using SimpleWifi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging.NewForm
{
    public partial class ChannelChoose : DevExpress.XtraEditors.XtraForm
    {
        List<AccessPoint> accessPoints;
        List<WifiInfo> wifiInfo;
        Wifi wifi;
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

                LoadWifiHotSpot();
            }
            else
            {
                CommHelp.ShowTips(label1.Text);
            }
        }

        private void LoadWifiHotSpot()
        {
            if(wifi == null)
                wifi = new Wifi();
            lbxWIFI.Items.Clear();
            accessPoints = wifi.GetAccessPoints();
            List<string> ssids = new List<string>();
            foreach (var item in accessPoints)
            {
                if (!ssids.Any(w => w == item.Name))// item.Name.Contains("Bangvo") && 
                {
                    ssids.Add(item.Name);
                    lbxWIFI.Items.Add(item.Name);
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
            var point = accessPoints.FirstOrDefault(w => w.Name == ssid);
            AuthRequest authRequest = new AuthRequest(point);

            WifiPassword form = new WifiPassword(password);
            form.AuthRequest = authRequest;
            if (form.ShowDialog() == DialogResult.OK)
            {
                password = form.Password;
                if (wifiInfo == null) wifiInfo = new List<WifiInfo>();
                if(!wifiInfo.Any(w => w.SSID == ssid))
                    wifiInfo.Add(new WifiInfo { SSID = ssid, Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password)) });

                if (form.IsRPwd)
                {
                    SaveWifiInfo();
                }

                if(point.Connect(form.AuthRequest, true))
                {
                    User.LinkType = LinkType.WIFI;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    CommHelp.ShowError("无法连接到网络");
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

        private void ChannelChoose_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }
    }
}