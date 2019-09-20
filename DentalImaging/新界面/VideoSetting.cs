using AForge.Video.DirectShow;
using DentalImaging.Model;
using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DentalImaging.新界面
{
    public partial class VideoSetting : Form
    {
        VideoCaptureDevice device;
        VideoParm CameraParm;
        VideoParm ParmA;
        VideoParm ParmB;
        VideoParm ParmC;
        string path;

        public VideoSetting(VideoCaptureDevice device)
        {
            this.device = device;
            InitializeComponent();
            InitParm();
        }

        private void InitParm()
        {
            path = Application.StartupPath;
            var aPath = Path.Combine(path, "Microspur.set");
            var bPath = Path.Combine(path, "Panorama.set");
            var cPath = Path.Combine(path, "Portrait.set");
            if (File.Exists(aPath))
            {
                var str = File.ReadAllText(aPath);
                ParmA = JsonConvert.DeserializeObject<VideoParm>(str);
            }
            if (File.Exists(bPath))
            {
                var str = File.ReadAllText(bPath);
                ParmB = JsonConvert.DeserializeObject<VideoParm>(str);
            }
            if (File.Exists(cPath))
            {
                var str = File.ReadAllText(cPath);
                ParmC = JsonConvert.DeserializeObject<VideoParm>(str);
            }

            CameraParm = GetCameraParm();

            if (ParmA == null)
                ParmA = CameraParm;

            if (ParmB == null)
                ParmB = CameraParm;

            if (ParmC == null)
                ParmC = CameraParm;

            if (ParmA == null)
                return;

            tbBaipinghengA.Value = ParmA.Baipingheng;            
            tbBaipinghengB.Value = ParmB.Baipingheng;
            tbBaipinghengC.Value = ParmC.Baipingheng;
            txtBaipinghengA.Text = ParmA.Baipingheng.ToString();
            txtBaipinghengB.Text = ParmB.Baipingheng.ToString();
            txtBaipinghengC.Text = ParmC.Baipingheng.ToString();

            tbBaoguangA.Value = ParmA.Baoguang;
            tbBaoguangB.Value = ParmB.Baoguang;
            tbBaoguangC.Value = ParmC.Baoguang;
            txtBaoguangA.Text = ParmA.Baoguang.ToString();
            txtBaoguangB.Text = ParmB.Baoguang.ToString();
            txtBaoguangC.Text = ParmC.Baoguang.ToString();

            tbBaoheduA.Value = ParmA.Baohedu;
            tbBaoheduB.Value = ParmB.Baohedu;
            tbBaoheduC.Value = ParmC.Baohedu;
            txtBaoheduA.Text = ParmA.Baohedu.ToString();
            txtBaoheduB.Text = ParmB.Baohedu.ToString();
            txtBaoheduC.Text = ParmC.Baohedu.ToString();

            tbDuibiduA.Value = ParmA.Duibidu;
            tbDuibiduB.Value = ParmB.Duibidu;
            tbDuibiduC.Value = ParmC.Duibidu;
            txtDuibiduA.Text = ParmA.Duibidu.ToString();
            txtDuibiduB.Text = ParmB.Duibidu.ToString();
            txtDuibiduC.Text = ParmC.Duibidu.ToString();

            tbGamaA.Value = ParmA.Gama;
            tbGamaB.Value = ParmB.Gama;
            tbGamaC.Value = ParmC.Gama;
            txtGamaA.Text = ParmA.Gama.ToString();
            txtGamaB.Text = ParmB.Gama.ToString();
            txtGamaC.Text = ParmC.Gama.ToString();

            tbLiangduA.Value = ParmA.Liangdu;
            tbLiangduB.Value = ParmB.Liangdu;
            tbLiangduC.Value = ParmC.Liangdu;
            txtLiangduA.Text = ParmA.Liangdu.ToString();
            txtLiangduB.Text = ParmB.Liangdu.ToString();
            txtLiangduC.Text = ParmC.Liangdu.ToString();

            tbNiguangduibiA.Value = ParmA.Niguangduibi;
            tbNiguangduibiB.Value = ParmB.Niguangduibi;
            tbNiguangduibiC.Value = ParmC.Niguangduibi;
            txtNiguangduibiA.Text = ParmA.Niguangduibi.ToString();
            txtNiguangduibiB.Text = ParmB.Niguangduibi.ToString();
            txtNiguangduibiC.Text = ParmC.Niguangduibi.ToString();

            tbQinxiduA.Value = ParmA.Qingxidu;
            tbQinxiduB.Value = ParmB.Qingxidu;
            tbQinxiduC.Value = ParmC.Qingxidu;
            txtQinxiduA.Text = ParmA.Qingxidu.ToString();
            txtQinxiduB.Text = ParmB.Qingxidu.ToString();
            txtQinxiduC.Text = ParmC.Qingxidu.ToString();

            //tbSediaoA.Value = ParmA.Sediao;
            //tbSediaoB.Value = ParmB.Sediao;
            //tbSediaoC.Value = ParmC.Sediao;
            //txtSediaoA.Text = ParmA.Sediao.ToString();
            //txtSediaoB.Text = ParmB.Sediao.ToString();
            //txtSediaoC.Text = ParmC.Sediao.ToString();
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键  
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数  
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符  
                }
            }
        }

        private void tb_Scroll(object sender, EventArgs e)
        {
            var tb = (TrackBar)sender;
            ((TextBox)tb.Tag).Text = tb.Value.ToString();
        }

        private void txt_TextChanged(object sender, EventArgs e)
        {
            int value = 0;
            var txt = (TextBox)sender;
            int.TryParse(txt.Text, out value);
            ((TrackBar)txt.Tag).Value = value;
        }

        private VideoParm GetCameraParm()
        {          
            var exposureValue = 0; //曝光-4 -10
            var exposureFlage = CameraControlFlags.Auto;
            device.GetCameraProperty(CameraControlProperty.Exposure, out exposureValue, out exposureFlage);            

            var brightnessValue = 0;//亮度 -64  64
            var brightnessFlage = VideoProcAmpFlags.Auto;
            device.GetVideoProperty(VideoProcAmpProperty.Brightness, out brightnessValue, out brightnessFlage);            

            var contrastValue = 0;//对比度 0 95
            var contrastFlage = VideoProcAmpFlags.Auto;
            device.GetVideoProperty(VideoProcAmpProperty.Contrast, out contrastValue, out contrastFlage);            

            var sharpnessValue = 0;//清晰度 1 7
            var sharpnessFlage = VideoProcAmpFlags.Auto;
            device.GetVideoProperty(VideoProcAmpProperty.Sharpness, out sharpnessValue, out sharpnessFlage);

            var whiteBalanceValue = 0;  //白平衡
            var whiteBalanceFlage = VideoProcAmpFlags.Auto;
            device.GetVideoProperty(VideoProcAmpProperty.WhiteBalance, out whiteBalanceValue, out whiteBalanceFlage);

            var saturationValue = 0;  //饱和度
            var saturationFlage = VideoProcAmpFlags.Auto;
            device.GetVideoProperty(VideoProcAmpProperty.Saturation, out saturationValue, out saturationFlage);

            var gammaValue = 0;  //伽马
            var gammaFlage = VideoProcAmpFlags.Auto;
            device.GetVideoProperty(VideoProcAmpProperty.Gamma, out gammaValue, out gammaFlage);

            var backlightCompensationValue = 0;  //逆光对比
            var backlightCompensationFlage = VideoProcAmpFlags.Auto;
            device.GetVideoProperty(VideoProcAmpProperty.BacklightCompensation, out backlightCompensationValue, out backlightCompensationFlage);

            //var hueValue = 0;  //色调
            //var hueFlage = VideoProcAmpFlags.Auto;
            //device.GetVideoProperty(VideoProcAmpProperty.Hue, out hueValue, out hueFlage);

            VideoParm parm = new VideoParm
            {
                Baoguang = exposureValue,
                Liangdu = brightnessValue,
                Duibidu = contrastValue,
                Qingxidu = sharpnessValue,
                Baipingheng = whiteBalanceValue,
                Baohedu = saturationValue,
                Gama = gammaValue,
                Niguangduibi = backlightCompensationValue,
                //Sediao = hueValue                
            };

            return parm;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //ParmA.Baipingheng = tbBaipinghengA.Value;
            //ParmA.Baoguang = tbBaoguangA.Value;
            ParmA.Baohedu = tbBaoheduA.Value;
            ParmA.Duibidu = tbDuibiduA.Value;
            ParmA.Gama = tbGamaA.Value;
            ParmA.Liangdu = tbLiangduA.Value;
            ParmA.Niguangduibi = tbNiguangduibiA.Value;
            ParmA.Qingxidu = tbQinxiduA.Value;
            //ParmA.Sediao = tbSediaoA.Value;
            ParmA.IsChange = ParmA.Equals(CameraParm);
            ParmA.Type = ParmType.A;

            //ParmB.Baipingheng = tbBaipinghengB.Value;
            //ParmB.Baoguang = tbBaoguangB.Value;
            ParmB.Baohedu = tbBaoheduB.Value;
            ParmB.Duibidu = tbDuibiduB.Value;
            ParmB.Gama = tbGamaB.Value;
            ParmB.Liangdu = tbLiangduB.Value;
            ParmB.Niguangduibi = tbNiguangduibiB.Value;
            ParmB.Qingxidu = tbQinxiduB.Value;
            //ParmB.Sediao = tbSediaoB.Value;
            ParmB.IsChange = ParmB.Equals(CameraParm);
            ParmB.Type = ParmType.B;

            //ParmC.Baipingheng = tbBaipinghengC.Value;
            //ParmC.Baoguang = tbBaoguangC.Value;
            ParmC.Baohedu = tbBaoheduC.Value;
            ParmC.Duibidu = tbDuibiduC.Value;
            ParmC.Gama = tbGamaC.Value;
            ParmC.Liangdu = tbLiangduC.Value;
            ParmC.Niguangduibi = tbNiguangduibiC.Value;
            ParmC.Qingxidu = tbQinxiduC.Value;
            //ParmC.Sediao = tbSediaoC.Value;
            ParmC.IsChange = ParmC.Equals(CameraParm);
            ParmC.Type = ParmType.C;

            var aPath = Path.Combine(path, "Microspur.set");
            var bPath = Path.Combine(path, "Panorama.set");
            var cPath = Path.Combine(path, "Portrait.set");

            File.WriteAllText(aPath, ParmA.ToJson());
            File.WriteAllText(bPath, ParmB.ToJson());
            File.WriteAllText(cPath, ParmC.ToJson());

            ShowTips("保存成功");
        }

        private void ShowTips(string message)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
