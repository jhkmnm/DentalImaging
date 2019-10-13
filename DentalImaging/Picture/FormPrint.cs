using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormPrint : Form
    {
        ImgInfo img;
        public FormPrint(ImgInfo img)
        {
            InitializeComponent();
            this.img = img;
        }
    }
}
