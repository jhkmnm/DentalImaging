using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DentalImaging.Model
{
    public static class User
    {
        static User()
        {
            InitLog4Net();
        }

        private static void InitLog4Net()
        {
            var logCfg = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
        }

        public static bool IsRegist { get; set; }
        public static int Date { get; set; }

        public static List<Patient> Patients { get; set; }

        public static Patient CurrentPatient { get; set; }

        //public static FormImg CurrentImg { get; set; }

        public static string  DBPath { get; set; }

        //public static int PageCount { get; set; }
        //public static bool IsAll { get; set; }

        /// <summary>
        /// 获取图片保存的文件名称
        /// </summary>
        public static string GetFileName
        {
            get {
                //var dirName = $"{DBPath}\\{CurrentPatient.Number}";
                var files = Directory.GetFiles(PatientPath, "*.ini");
                List<int> fileIndex = new List<int>();
                foreach (var file in files)
                {
                    fileIndex.Add(Convert.ToInt32(file.Replace(PatientPath + "\\", "").Replace(".ini", "").Replace("S", "")));
                }
                return fileIndex.Count == 0 ? "S1" : $"S{(fileIndex.Max() + 1).ToString()}";
            }
        }

        public static string PatientPath => $"{DBPath}\\{CurrentPatient.FName + CurrentPatient.BirthDay}";

        /// <summary>
        /// 读取患者信息
        /// </summary>
        public static void ReadPatients()
        {
            try
            {
                if(File.Exists($"{DBPath}\\us.ini"))
                {
                    var str = File.ReadAllText($"{DBPath}\\us.ini");
                    Patients = str.ToObject<List<Patient>>();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 读取当前患者的病历
        /// </summary>
        public static void ReadCurrentPatientHistorys()
        {
            if (CurrentPatient.Historys == null)
            {
                CurrentPatient.Historys = new List<CaseHistory>();
            }
            if(Directory.Exists(PatientPath))
            {
                var dirs = Directory.GetDirectories(PatientPath);

                CurrentPatient.Historys.Clear();
                foreach (var dir in dirs)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(dir);
                    var str = File.ReadAllText(dir + $"\\{dirInfo.Name}.ini");
                    CaseHistory history = str.ToObject<CaseHistory>();

                    var files = Directory.GetFiles(dir, "*.ini");
                    //int index = 0;

                    foreach (var file in files)
                    {
                        FileInfo finfo = new FileInfo(file);

                        if (finfo.Name != $"{dirInfo.Name}.ini")
                        {
                            str = File.ReadAllText(file);
                            var imgInfo = str.ToObject<ImgInfo>();
                            //imgInfo.Img = ReadImageFile(imgInfo.ImgPath); //Help.Base64Util.GetImageFromBase64(imgInfo.Base64);
                            //imgInfo.ID = index++;
                            if (history.imgInfos == null)
                                history.imgInfos = new List<ImgInfo>();
                            history.imgInfos.Add(imgInfo);
                        }
                    }
                    CurrentPatient.Historys.Add(history);
                }
            }
        }

        /// <summary>
        /// 保存病历
        /// </summary>
        /// <param name="history"></param>
        public static void SaveHistorys(CaseHistory history)
        {
            var dirName = $"{PatientPath}\\{history.HistoryNo}";
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            if(history.imgInfos != null)
            {                
                history.imgInfos.Where(w => !w.IsSaveToFile).ToList().ForEach(f => {
                    if(f.InfoType == 1)
                    {
                        var file = new FileInfo(f.VideoPath);
                        string fileName = "General_A";
                        var newFilePath = $"{dirName}\\{fileName}{f.FileName}.avi";
                        File.Move(f.VideoPath, newFilePath);
                        f.VideoPath = newFilePath;
                    }
                    var imgPath = $"{PatientPath}\\{history.HistoryNo}\\{f.FileName}.bmp";
                    f.ImgPath = imgPath;
                    f.IsSaveToFile = true;
                    File.WriteAllText($"{PatientPath}\\{history.HistoryNo}\\{f.FileName}.ini", f.ToJson());
                    ((Image)f.Img).Save(imgPath);
                });
            }
            File.WriteAllText($"{dirName}\\{history.HistoryNo}.ini", history.ToJson());

            ReadCurrentPatientHistorys();
        }

        /// <summary>
        /// 通过FileStream 来打开文件，这样就可以实现不锁定Image文件，到时可以让多用户同时访问Image文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }
    }
}
