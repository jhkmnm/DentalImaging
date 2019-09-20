using AForge.Video.DirectShow;
using DentalImaging.Model;
using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DentalImaging.新界面
{
    public partial class VideoSettingWifi : Form
    {
        VideoParm CameraParm;
        VideoParm ParmA;
        VideoParm ParmB;
        VideoParm ParmC;
        string path;
        VideoParm _defParm;

        public VideoSettingWifi()
        {
            InitializeComponent();

            _defParm = new VideoParm
            {
                Baipingheng = tbBaipinghengA.Value,
                BaipinghengAuto = chkBaipinghengA.Checked,
                Baoguang = tbBaoguangA.Value,
                BaoguangAuto = chkBaoguangA.Checked,
                Baohedu = tbBaoheduA.Value,
                Duibidu = tbDuibiduA.Value,
                Duijiao = tbDuijiaoA.Value,
                DuijiaoAuto = chkDuijiaoA.Checked,
                Fenbian = ddlFenbianlvA.Text,
                Gama = tbGamaA.Value,
                Gonglv = tbGonglvA.Value,
                Liangdu = tbLiangduA.Value,
                Qingxidu = tbQinxiduA.Value,
                //Sediao = tbSediaoA.Value,
                Zengyi = tbZengyiA.Value
            };

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
            chkBaipinghengA.Checked = ParmA.BaipinghengAuto;
            chkBaipinghengB.Checked = ParmB.BaipinghengAuto;
            chkBaipinghengC.Checked = ParmC.BaipinghengAuto;

            tbBaoguangA.Value = ParmA.Baoguang;
            tbBaoguangB.Value = ParmB.Baoguang;
            tbBaoguangC.Value = ParmC.Baoguang;
            txtBaoguangA.Text = ParmA.Baoguang.ToString();
            txtBaoguangB.Text = ParmB.Baoguang.ToString();
            txtBaoguangC.Text = ParmC.Baoguang.ToString();
            chkBaoguangA.Checked = ParmA.BaoguangAuto;
            chkBaoguangB.Checked = ParmB.BaoguangAuto;
            chkBaoguangC.Checked = ParmC.BaoguangAuto;

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

            ddlFenbianlvA.Text = ParmA.Fenbian;
            ddlFenbianlvB.Text = ParmB.Fenbian;
            ddlFenbianlvC.Text = ParmC.Fenbian;

            tbGonglvA.Value = ParmA.Gonglv;
            tbGonglvB.Value = ParmB.Gonglv;
            tbGonglvC.Value = ParmC.Gonglv;
            txtGonglvA.Text = ParmA.Gonglv.ToString();
            txtGonglvB.Text = ParmB.Gonglv.ToString();
            txtGonglvC.Text = ParmC.Gonglv.ToString();

            tbDuijiaoA.Value = ParmA.Duijiao;
            tbDuijiaoB.Value = ParmB.Duijiao;
            tbDuijiaoC.Value = ParmC.Duijiao;
            txtDuijiaoA.Text = ParmA.Duijiao.ToString();
            txtDuijiaoB.Text = ParmB.Duijiao.ToString();
            txtDuijiaoC.Text = ParmC.Duijiao.ToString();
            chkDuijiaoA.Checked = ParmA.DuijiaoAuto;
            chkDuijiaoB.Checked = ParmB.DuijiaoAuto;
            chkDuijiaoC.Checked = ParmC.DuijiaoAuto;

            tbZengyiA.Value = ParmA.Zengyi;
            tbZengyiB.Value = ParmB.Zengyi;
            tbZengyiC.Value = ParmC.Zengyi;
            txtZengyiA.Text = ParmA.Zengyi.ToString();
            txtZengyiB.Text = ParmB.Zengyi.ToString();
            txtZengyiC.Text = ParmC.Zengyi.ToString();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ParmA == null) ParmA = new VideoParm();
            ParmA.Baipingheng = tbBaipinghengA.Value;
            ParmA.BaipinghengAuto = chkBaipinghengA.Checked;
            ParmA.Baoguang = tbBaoguangA.Value;
            ParmA.BaoguangAuto = chkBaoguangA.Checked;
            ParmA.Baohedu = tbBaoheduA.Value;
            ParmA.Duibidu = tbDuibiduA.Value;
            ParmA.Gama = tbGamaA.Value;
            ParmA.Liangdu = tbLiangduA.Value;
            ParmA.Qingxidu = tbQinxiduA.Value;
            //ParmA.Sediao = tbSediaoA.Value;
            ParmA.Fenbian = ddlFenbianlvA.Text;
            ParmA.Gonglv = tbGonglvA.Value;
            ParmA.Duijiao = tbDuijiaoA.Value;
            ParmA.DuijiaoAuto = chkDuijiaoA.Checked;
            ParmA.Zengyi = tbZengyiA.Value;
            ParmA.IsChange = ParmA.Equals(CameraParm);
            ParmA.Type = ParmType.A;

            if (ParmB == null) ParmB = new VideoParm();
            ParmB.Baipingheng = tbBaipinghengB.Value;
            ParmB.BaipinghengAuto = chkBaipinghengB.Checked;
            ParmB.Baoguang = tbBaoguangB.Value;
            ParmB.BaoguangAuto = chkBaoguangB.Checked;
            ParmB.Baohedu = tbBaoheduB.Value;
            ParmB.Duibidu = tbDuibiduB.Value;
            ParmB.Gama = tbGamaB.Value;
            ParmB.Liangdu = tbLiangduB.Value;
            ParmB.Qingxidu = tbQinxiduB.Value;
            //ParmB.Sediao = tbSediaoB.Value;
            ParmB.Fenbian = ddlFenbianlvB.Text;
            ParmB.Gonglv = tbGonglvB.Value;
            ParmB.Duijiao = tbDuijiaoB.Value;
            ParmB.DuijiaoAuto = chkDuijiaoB.Checked;
            ParmB.Zengyi = tbZengyiB.Value;
            ParmB.IsChange = ParmB.Equals(CameraParm);
            ParmB.Type = ParmType.B;

            if (ParmC == null) ParmC = new VideoParm();
            ParmC.Baipingheng = tbBaipinghengC.Value;
            ParmC.BaipinghengAuto = chkBaipinghengC.Checked;
            ParmC.Baoguang = tbBaoguangC.Value;
            ParmC.BaoguangAuto = chkBaoguangC.Checked;
            ParmC.Baohedu = tbBaoheduC.Value;
            ParmC.Duibidu = tbDuibiduC.Value;
            ParmC.Gama = tbGamaC.Value;
            ParmC.Liangdu = tbLiangduC.Value;
            ParmC.Qingxidu = tbQinxiduC.Value;
            //ParmC.Sediao = tbSediaoC.Value;
            ParmC.Fenbian = ddlFenbianlvC.Text;
            ParmC.Gonglv = tbGonglvC.Value;
            ParmC.Duijiao = tbDuijiaoC.Value;
            ParmC.DuijiaoAuto = chkDuijiaoC.Checked;
            ParmC.Zengyi = tbZengyiC.Value;
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

        private void chkBaipinghengC_CheckedChanged(object sender, EventArgs e)
        {
            tbBaipinghengC.Enabled = false;
            txtBaipinghengC.Enabled = false;
            txtBaipinghengC.ReadOnly = true;
        }

        private void chkBaoguangC_CheckedChanged(object sender, EventArgs e)
        {
            tbBaoguangC.Enabled = false;
            txtBaoguangC.Enabled = false;
            txtBaoguangC.ReadOnly = true;
        }

        private void chkDuijiaoC_CheckedChanged(object sender, EventArgs e)
        {
            tbDuijiaoC.Enabled = false;
            txtDuijiaoC.Enabled = false;
            txtDuijiaoC.ReadOnly = true;
        }

        private void chkBaipinghengB_CheckedChanged(object sender, EventArgs e)
        {
            tbBaipinghengB.Enabled = false;
            txtBaipinghengB.Enabled = false;
            txtBaipinghengB.ReadOnly = true;
        }

        private void chkBaoguangB_CheckedChanged(object sender, EventArgs e)
        {
            tbBaoguangB.Enabled = false;
            txtBaoguangB.Enabled = false;
            txtBaoguangB.ReadOnly = true;
        }

        private void chkDuijiaoB_CheckedChanged(object sender, EventArgs e)
        {
            tbDuijiaoB.Enabled = false;
            txtDuijiaoB.Enabled = false;
            txtDuijiaoB.ReadOnly = true;
        }

        private void chkBaipinghengA_CheckedChanged(object sender, EventArgs e)
        {
            tbBaipinghengA.Enabled = false;
            txtBaipinghengA.Enabled = false;
            txtBaipinghengA.ReadOnly = true;
        }

        private void chkBaoguangA_CheckedChanged(object sender, EventArgs e)
        {
            tbBaoguangA.Enabled = false;
            txtBaoguangA.Enabled = false;
            txtBaoguangA.ReadOnly = true;
        }

        private void chkDuijiaoA_CheckedChanged(object sender, EventArgs e)
        {
            tbDuijiaoA.Enabled = false;
            txtDuijiaoA.Enabled = false;
            txtDuijiaoA.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0)
            {
                txtBaipinghengA.Text = _defParm.Baipingheng.ToString();
                chkBaipinghengA.Checked = _defParm.BaipinghengAuto;
                txtBaoguangA.Text = _defParm.Baoguang.ToString();
                chkBaoguangA.Checked = _defParm.BaoguangAuto;
                txtBaoheduA.Text = _defParm.Baohedu.ToString();
                txtDuibiduA.Text = _defParm.Duibidu.ToString();
                txtDuijiaoA.Text = _defParm.Duijiao.ToString();
                chkDuijiaoA.Checked = _defParm.DuijiaoAuto;
                ddlFenbianlvA.Text = _defParm.Fenbian;
                txtGamaA.Text = _defParm.Gama.ToString();
                txtGonglvA.Text = _defParm.Gonglv.ToString();
                txtLiangduA.Text = _defParm.Liangdu.ToString();
                txtQinxiduA.Text = _defParm.Qingxidu.ToString();
                //txtSediaoA.Text = _defParm.Sediao.ToString();
                txtZengyiA.Text = _defParm.Zengyi.ToString();
            }
            else if(tabControl1.SelectedIndex == 1)
            {
                txtBaipinghengB.Text = _defParm.Baipingheng.ToString();
                chkBaipinghengB.Checked = _defParm.BaipinghengAuto;
                txtBaoguangB.Text = _defParm.Baoguang.ToString();
                chkBaoguangB.Checked = _defParm.BaoguangAuto;
                txtBaoheduB.Text = _defParm.Baohedu.ToString();
                txtDuibiduB.Text = _defParm.Duibidu.ToString();
                txtDuijiaoB.Text = _defParm.Duijiao.ToString();
                chkDuijiaoB.Checked = _defParm.DuijiaoAuto;
                ddlFenbianlvB.Text = _defParm.Fenbian;
                txtGamaB.Text = _defParm.Gama.ToString();
                txtGonglvB.Text = _defParm.Gonglv.ToString();
                txtLiangduB.Text = _defParm.Liangdu.ToString();
                txtQinxiduB.Text = _defParm.Qingxidu.ToString();
                //txtSediaoB.Text = _defParm.Sediao.ToString();
                txtZengyiB.Text = _defParm.Zengyi.ToString();
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                txtBaipinghengC.Text = _defParm.Baipingheng.ToString();
                chkBaipinghengC.Checked = _defParm.BaipinghengAuto;
                txtBaoguangC.Text = _defParm.Baoguang.ToString();
                chkBaoguangC.Checked = _defParm.BaoguangAuto;
                txtBaoheduC.Text = _defParm.Baohedu.ToString();
                txtDuibiduC.Text = _defParm.Duibidu.ToString();
                txtDuijiaoC.Text = _defParm.Duijiao.ToString();
                chkDuijiaoC.Checked = _defParm.DuijiaoAuto;
                ddlFenbianlvC.Text = _defParm.Fenbian;
                txtGamaC.Text = _defParm.Gama.ToString();
                txtGonglvC.Text = _defParm.Gonglv.ToString();
                txtLiangduC.Text = _defParm.Liangdu.ToString();
                txtQinxiduC.Text = _defParm.Qingxidu.ToString();
                //txtSediaoC.Text = _defParm.Sediao.ToString();
                txtZengyiC.Text = _defParm.Zengyi.ToString();
            }
        }
    }
}
