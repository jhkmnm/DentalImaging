using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace DentalImaging
{
    public class TimeClass
    {
        public static int InitRegedit()
        {
            var reDate = ReadDate();

            if (string.IsNullOrEmpty(reDate))
                return -1;

            var endDate = ReadEndDate();

            var date1 = Convert.ToDateTime(reDate +" 00:00:00");
            var date2 = Convert.ToDateTime(endDate + " 00:00:00");
            
            TimeSpan ts = date2 - date1;
            if (ts.Days > 0)
                return ts.Days;
            else
                return -1;
            //if (ts.Days > 30)
            //    return 1;   //已注册
            //else if (ts.Days > 0 && ts.Days <= 30)
            //    return 0;   //未注册试用
            //else
            //    return -1;  //不可用
        }

        public static void CreateRegedit()
        {
            if(string.IsNullOrEmpty(ReadDate()))
                WriteSetting();
        }

        /// <summary>
        /// CpuID
        /// </summary>
        /// <returns></returns>
        public static string GetCpuId()
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
        
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static string GetNowDate()
        {
            string NowDate = DateTime.Now.ToString("yyyy-MM-dd"); //.Year + DateTime.Now.Month + DateTime.Now.Day).ToString();
                                                                //     DateTime date = Convert.ToDateTime(NowDate, "yyyy/MM/dd");
            return NowDate;
        }

        public static string GetEndDate()
        {
            string NowDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"); //.Year + DateTime.Now.Month + DateTime.Now.Day).ToString();
                                                                //     DateTime date = Convert.ToDateTime(NowDate, "yyyy/MM/dd");
            return NowDate;
        }
        
        /// <summary>
        /// 生成序列号
        /// </summary>
        /// <returns></returns>
        public static string CreatSerialNumber()
        {
            string SerialNumber = GetCpuId() + "-" + "20110915";
            return SerialNumber;
        }
        /* 
         * i=1 得到 CUP 的id 
         * i=0 得到上次或者 开始时间 
         */
        public static string GetSoftEndDateAllCpuId(int i, string SerialNumber)
        {
            if (i == 1)
            {
                string cupId = SerialNumber.Substring(0, SerialNumber.LastIndexOf("-")); // .LastIndexOf("-"));
                return cupId;
            }
            if (i == 0)
            {
                string dateTime = SerialNumber.Substring(SerialNumber.LastIndexOf("-") + 1);
                //  dateTime = dateTime.Insert(4, "/").Insert(7, "/");
                //  DateTime date = Convert.ToDateTime(dateTime);
                return dateTime;
            }
            else
            {
                return string.Empty;
            }
        }
        
        /// <summary>
        /// 写入配置
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Setting"></param>
        private static void WriteSetting()  // name = key  value=setting  Section= path
        {
            string key1 = "ReKey";
            string value1 = Encryption.DesEncrypt(CreatSerialNumber(), GetCpuId());
            string key2 = "ReDate";
            string value2 = Encryption.DesEncrypt(GetNowDate(), GetCpuId());
            string key3 = "EndDate";
            string value3 = Encryption.DesEncrypt(GetEndDate(), GetCpuId());
            WriteRegistry(key1, value1);
                        
            WriteFile(key2, value2);
            WriteFile(key3, value3);
        }

        /// <summary>
        /// 读取注册码
        /// </summary>
        /// <returns></returns>
        private static string ReadSetting()
        {
            string key1 = "ReKey";
            return ReadRegistry(key1, "");
        }

        /// <summary>
        /// 读取注册日期
        /// </summary>
        /// <returns></returns>
        private static string ReadDate()
        {
            string key2 = "ReDate";
            return Encryption.DesDecrypt(ReadFile(key2), GetCpuId());
        }

        private static string ReadEndDate()
        {
            string key2 = "EndDate";
            return Encryption.DesDecrypt(ReadFile(key2), GetCpuId());
        }

        /// <summary>
        /// 写入注册表
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Setting"></param>
        private static void WriteRegistry(string Key, string Setting)
        {
            RegistryKey key1 = Registry.CurrentUser.CreateSubKey("Software\\MyTest_ChildPlat\\ChildPlat"); // .LocalMachine.CreateSubKey("Software\\mytest");
            if (key1 == null)
            {
                return;
            }
            try
            {
                key1.SetValue(Key, Setting);
            }
            catch (Exception exception1)
            {
                return;
            }
            finally
            {
                key1.Close();
            }
        }

        /// <summary>
        /// 读取注册表
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Setting"></param>
        /// <param name="Default"></param>
        /// <returns></returns>
        private static string ReadRegistry(string Key, string Default)
        {
            RegistryKey key1 = Registry.CurrentUser.OpenSubKey("Software\\MyTest_ChildPlat\\ChildPlat");
            if (key1 != null)
            {
                object obj1 = key1.GetValue(Key, Default);
                key1.Close();
                if (obj1 != null)
                {
                    if (!(obj1 is string))
                    {
                        return "-1";
                    }
                    string obj2 = obj1.ToString();
                    obj2 = Encryption.DesDecrypt(obj2, GetCpuId());
                    return obj2;
                }
                return "-1";
            }

            return Default;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Setting"></param>
        private static void WriteFile(string Key, string Setting)
        {
            string path = CreateFile(Key, Setting);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static string ReadFile(string Key)
        {
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}//DentalImaging//{Key}";
            if(File.Exists(path))
                return File.ReadAllText(path);
            return "";
        }

        private static string CreateFile(string key, string setting)
        {
            string dirName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+ "//DentalImaging";
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
                Directory.SetCreationTime(dirName, DateTime.Now.AddYears(-2));
                
            }                
            string filePath = $"{dirName}//{key}";
            using (var file = File.Create(filePath))
            {
                var bt = System.Text.Encoding.Default.GetBytes(setting);
                file.Write(bt, 0, bt.Count());
                file.Close();
                File.SetAttributes(filePath, FileAttributes.Hidden);//设置文件夹属性为隐藏
                File.SetCreationTime(filePath, DateTime.Now.AddYears(-2));
                File.SetLastWriteTime(filePath, DateTime.Now.AddYears(-2));
                File.SetLastAccessTime(filePath, DateTime.Now.AddYears(-2));
            }
            Directory.SetLastWriteTime(dirName, DateTime.Now.AddYears(-2));
            Directory.SetLastAccessTime(dirName, DateTime.Now.AddYears(-2));
            return filePath;
        }
    }
}
