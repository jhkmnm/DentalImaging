using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NativeWifi;

namespace DentalImaging.Help
{
    public class WifiHelp
    {
        private WIFISSID ssid;
        private string key;                 //wifi密码
        public List<WIFISSID> ssids = new List<WIFISSID>();

        public WifiHelp()
        {
            ssids.Clear();
        }

        public void SetWifi(WIFISSID ssid, string key)
        {
            this.ssid = ssid;
            this.key = key;
        }

        /// <summary>
        /// 寻找当前连接的网络：
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentConnection()
        {
            WlanClient client = new WlanClient();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    if (wlanIface.InterfaceState == Wlan.WlanInterfaceState.Connected && wlanIface.CurrentConnection.isState == Wlan.WlanInterfaceState.Connected)
                    {
                        return wlanIface.CurrentConnection.profileName;
                    }
                }
            }

            return string.Empty;
        }

        static string GetStringForSSID(Wlan.Dot11Ssid ssid)
        {
            return Encoding.UTF8.GetString(ssid.SSID, 0, (int)ssid.SSIDLength);
        }

        /// <summary>
        /// 枚举所有无线设备接收到的SSID
        /// </summary>
        public void ScanSSID()
        {
            WlanClient client = new WlanClient();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                // Lists all networks with WEP security
                Wlan.WlanAvailableNetwork[] networks = wlanIface.GetAvailableNetworkList(0);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    WIFISSID targetSSID = new WIFISSID();

                    targetSSID.wlanInterface = wlanIface;
                    targetSSID.wlanSignalQuality = (int)network.wlanSignalQuality;
                    targetSSID.SSID = GetStringForSSID(network.dot11Ssid);
                    //targetSSID.SSID = Encoding.Default.GetString(network.dot11Ssid.SSID, 0, (int)network.dot11Ssid.SSIDLength);
                    targetSSID.dot11DefaultAuthAlgorithm = network.dot11DefaultAuthAlgorithm.ToString();
                    targetSSID.dot11DefaultCipherAlgorithm = network.dot11DefaultCipherAlgorithm.ToString();
                    ssids.Add(targetSSID);
                }
            }
        }

        /// <summary>
        /// 字符串转Hex
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToHex(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.Default.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString().ToUpper());
        }

        /// <summary>
        /// 连接到无线网络
        /// </summary>
        public string ConnectToSSID()
        {
            try
            {
                String auth = string.Empty;
                String cipher = string.Empty;
                bool isNoKey = false;
                String keytype = string.Empty;
                switch (ssid.dot11DefaultAuthAlgorithm)
                {
                    case "IEEE80211_Open":
                        auth = "open"; break;
                    case "RSNA":
                        auth = "WPA2PSK"; break;
                    case "RSNA_PSK":
                        auth = "WPA2PSK"; break;
                    case "WPA":
                        auth = "WPAPSK"; break;
                    case "WPA_None":
                        auth = "WPAPSK"; break;
                    case "WPA_PSK":
                        auth = "WPAPSK"; break;
                }
                switch (ssid.dot11DefaultCipherAlgorithm)
                {
                    case "CCMP":
                        cipher = "AES";
                        keytype = "passPhrase";
                        break;
                    case "TKIP":
                        cipher = "TKIP";
                        keytype = "passPhrase";
                        break;
                    case "None":
                        cipher = "none"; keytype = "";
                        isNoKey = true;
                        break;
                    case "WWEP":
                        cipher = "WEP";
                        keytype = "networkKey";
                        break;
                    case "WEP40":
                        cipher = "WEP";
                        keytype = "networkKey";
                        break;
                    case "WEP104":
                        cipher = "WEP";
                        keytype = "networkKey";
                        break;
                }

                if (isNoKey && !string.IsNullOrEmpty(key))
                {
                    return "无法连接网络";
                }
                else if (!isNoKey && string.IsNullOrEmpty(key))
                {
                    return "无法连接网络";
                }
                else
                {
                    string profileName = ssid.SSID;
                    string mac = StringToHex(profileName);
                    string profileXml = string.Empty;
                    if (!string.IsNullOrEmpty(key))
                    {
                        profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>auto</connectionMode><autoSwitch>false</autoSwitch><MSM><security><authEncryption><authentication>{2}</authentication><encryption>{3}</encryption><useOneX>false</useOneX></authEncryption><sharedKey><keyType>{4}</keyType><protected>false</protected><keyMaterial>{5}</keyMaterial></sharedKey><keyIndex>0</keyIndex></security></MSM></WLANProfile>",
                            profileName, mac, auth, cipher, keytype, key);
                    }
                    else
                    {
                        profileXml = string.Format("<?xml version=\"1.0\"?><WLANProfile xmlns=\"http://www.microsoft.com/networking/WLAN/profile/v1\"><name>{0}</name><SSIDConfig><SSID><hex>{1}</hex><name>{0}</name></SSID></SSIDConfig><connectionType>ESS</connectionType><connectionMode>auto</connectionMode><autoSwitch>false</autoSwitch><MSM><security><authEncryption><authentication>{2}</authentication><encryption>{3}</encryption><useOneX>false</useOneX></authEncryption></security></MSM></WLANProfile>",
                            profileName, mac, auth, cipher, keytype);
                    }

                    ssid.wlanInterface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);

                    bool success = ssid.wlanInterface.ConnectSynchronously(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName, 15000);
                    if (!success)
                    {
                        return "连接网络失败";
                    }
                    return "";
                }
            }
            catch (Exception e)
            {
                return "连接网络失败" + e.Message;
            }
        }

        /// <summary>
        /// 当连接的连接状态进行通知 面是简单的通知事件的实现，根据通知的内容在界面上显示提示信息：
        /// </summary>
        /// <param name="notifyData"></param>
        /// <param name="connNotifyData"></param>
        private void WlanInterface_WlanConnectionNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanConnectionNotificationData connNotifyData)
        {
            try
            {
                if (notifyData.notificationSource == Wlan.WlanNotificationSource.ACM)
                {
                    int notificationCode = (int)notifyData.NotificationCode;
                    switch (notificationCode)
                    {
                        case (int)Wlan.WlanNotificationCodeAcm.ConnectionStart:

                            Console.WriteLine("开始连接无线网络.......");
                            break;
                        case (int)Wlan.WlanNotificationCodeAcm.ConnectionComplete:

                            break;
                        case (int)Wlan.WlanNotificationCodeAcm.Disconnecting:

                            Console.WriteLine("正在断开无线网络连接.......");
                            break;
                        case (int)Wlan.WlanNotificationCodeAcm.Disconnected:
                            Console.WriteLine("已经断开无线网络连接.......");
                            break;
                    }
                }
                //}));
            }
            catch (Exception e)
            {
                //Loger.WriteLog(e.Message);
            }
        }
    }

    public class WIFISSID
    {
        public string SSID = "NONE";
        public string dot11DefaultAuthAlgorithm = "";
        public string dot11DefaultCipherAlgorithm = "";
        public bool networkConnectable = true;
        public string wlanNotConnectableReason = "";
        public int wlanSignalQuality = 0;
        public WlanClient.WlanInterface wlanInterface = null;
    }
}
