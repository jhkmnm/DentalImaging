using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormToothImg : Form
    {
        string dirName = $"{User.DBPath}/{User.CurrentPatient.Number}";
        int toothIndex;        

        public FormToothImg(int toothIndex)
        {
            InitializeComponent();
            this.toothIndex = toothIndex;
            this.Text = $"牙编号{toothIndex}";
            Init();
        }

        public void Init()
        {
            var picBoxs = new[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5 };
            var labels = new[] { label1, label2, label3, label4, label5 };
            var files = Directory.GetFiles(dirName, "*.ini");
            List<int> fileIndex = new List<int>();
            int index = 0;
            foreach (var file in files)
            {
                if (index >= picBoxs.Length) return;
                var str = File.ReadAllText(file);
                var img = str.ToObject<ImgInfo>();
                if(img.ToothIndex.Contains(toothIndex))
                {
                    //picBoxs[index].Image = Help.Base64Util.GetImageFromBase64(img.Base64);
                    labels[index].Text = img.Date.ToString("yyyy-MM-dd");
                    index++;
                }
            }
        }
    }
}
