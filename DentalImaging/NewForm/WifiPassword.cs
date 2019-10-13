using DentalImaging.Help;
using SimpleWifi;
using System;
using System.Windows.Forms;

namespace DentalImaging.NewForm
{
    public partial class WifiPassword : DevExpress.XtraEditors.XtraForm
    {
        public WifiPassword(string pwd="")
        {
            InitializeComponent();
            textBox1.Text = pwd;
            if (!string.IsNullOrEmpty(pwd))
                chkRPwd.Checked = true;
        }

        public AuthRequest AuthRequest { get; set; }

        public string Password { get { return textBox1.Text; } }
        public bool IsRPwd { get { return chkRPwd.Checked; } }

        private void button1_Click(object sender, EventArgs e)
        {
            AuthRequest.Password = textBox1.Text;
            if(!AuthRequest.IsPasswordValid)
            {
                CommHelp.ShowWarning("密码不符合要求，请重新输入!");
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void WifiPassword_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }
    }
}
