using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging.新界面
{
    public partial class WifiPassword : DevExpress.XtraEditors.XtraForm
    {
        public WifiPassword()
        {
            InitializeComponent();
        }

        public string Password { get { return textBox1.Text; } }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
