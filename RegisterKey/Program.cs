using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RegisterKey
{
    class Program
    {
        static void Main(string[] args)
        {
            //RegistryKey key1 = Registry.CurrentUser.OpenSubKey("Software\\MyTest_ChildPlat\\ChildPlat");
            //if (key1 != null)
            //{
            //    object obj1 = key1.GetValue("ReKey", "");
            //    key1.Close();
            //    if (obj1 != null)
            //    {
            //        if (!(obj1 is string))
            //        {
            //            return "-1";
            //        }
            //        string obj2 = obj1.ToString();
            //        obj2 = DesDecrypt(obj2, GetCpuId());
            //        return obj2;
            //    }
            //    return "-1";
            //}
            WriteEndDate();
            Console.WriteLine("注册成功,按任意键退出！");
            Console.ReadKey();
        }

        private static void WriteEndDate()
        {
            string dirName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "//DentalImaging";
            string filePath = $"{dirName}//EndDate";
            string setting = DesEncrypt(DateTime.Now.AddYears(30).ToString("yyyy-MM-dd"), GetCpuId());

            using (var file = File.Open(filePath, FileMode.OpenOrCreate))
            {
                var bt = System.Text.Encoding.Default.GetBytes(setting);
                file.Write(bt, 0, bt.Count());
                file.Close();
            }
        }

        private static string CreatSerialNumber()
        {
            string SerialNumber = GetCpuId() + "-" + "20110915";
            return SerialNumber;
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        private static string DesDecrypt(string decryptString, string key)
        {
            if (string.IsNullOrEmpty(decryptString)) return "";
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        public static string DesEncrypt(string encryptString, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        private static string GetCpuId()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            string strCpuID = null;
            foreach (ManagementObject mo in moc)
            {
                strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return strCpuID;
        }
    }
}
