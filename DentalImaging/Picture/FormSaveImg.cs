using DentalImaging.Help;
using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class FormSaveImg : Form
    {
        ImgInfo img;
        ImgInfo newImg;
        List<int> ToothIndex = new List<int>();
        string dirName = $"{User.DBPath}\\{User.CurrentPatient.Number}";
        bool isPrint;

        public FormSaveImg(ImgInfo img, bool print = false)
        {
            InitializeComponent();
            this.img = img;
            if(!string.IsNullOrWhiteSpace(img.FileName))
            {
                radioButton1.Enabled = true;
                radioButton1.Checked = true;
            }
            else
            {
                textBox1.Text = User.GetFileName;
            }            
            dtpDate.Value = img.Date;
            Init();
            isPrint = print;
        }

        //private string GetFileName()
        //{
        //    var files = Directory.GetFiles(dirName, "*.ini");
        //    List<int> fileIndex = new List<int>();            
        //    foreach (var file in files)
        //    {
        //        fileIndex.Add(Convert.ToInt32(file.Replace(dirName +"\\", "").Replace(".ini", "").Replace("S", "")));
        //    }
        //    return fileIndex.Count == 0 ? "S1" : $"S{(fileIndex.Max() + 1).ToString()}";
        //}

        private void Init()
        {
            for (int i = 0; i <= 32; i++)
            {
                if (i == 0)
                {
                    ddlTooth1.Items.Add("");
                    ddlTooth2.Items.Add("");
                    ddlTooth3.Items.Add("");
                    ddlTooth4.Items.Add("");
                    ddlTooth5.Items.Add("");
                    ddlTooth6.Items.Add("");
                }                    
                else
                {
                    ddlTooth1.Items.Add(i);
                    ddlTooth2.Items.Add(i);
                    ddlTooth3.Items.Add(i);
                    ddlTooth4.Items.Add(i);
                    ddlTooth5.Items.Add(i);
                    ddlTooth6.Items.Add(i);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            img.Date = dtpDate.Value;
            img.FileName = textBox1.Text;
            img.Note = textBox2.Text;
            img.ToothIndex = new List<int>();
            if(!string.IsNullOrEmpty(ddlTooth1.Text))
            {
                img.ToothIndex.Add(Convert.ToInt32(ddlTooth1.Text));
            }
            if (!string.IsNullOrEmpty(ddlTooth2.Text))
            {
                img.ToothIndex.Add(Convert.ToInt32(ddlTooth2.Text));
            }
            if (!string.IsNullOrEmpty(ddlTooth3.Text))
            {
                img.ToothIndex.Add(Convert.ToInt32(ddlTooth3.Text));
            }
            if (!string.IsNullOrEmpty(ddlTooth4.Text))
            {
                img.ToothIndex.Add(Convert.ToInt32(ddlTooth4.Text));
            }
            if (!string.IsNullOrEmpty(ddlTooth5.Text))
            {
                img.ToothIndex.Add(Convert.ToInt32(ddlTooth5.Text));
            }
            if (!string.IsNullOrEmpty(ddlTooth6.Text))
            {
                img.ToothIndex.Add(Convert.ToInt32(ddlTooth6.Text));
            }
            //img.Base64 = Base64Util.GetBase64FromImage(img.Img);
            ToothIndex = img.ToothIndex;
               
            if(!isPrint)
            {
                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }
                File.WriteAllText($"{dirName}/{img.FileName}.ini", img.ToJson());
                this.DialogResult = DialogResult.OK;
                var savePath = ConfigurationManager.AppSettings["ImgSavePath"];
                if (string.IsNullOrWhiteSpace(savePath))
                {
                    img.Img.Save($"{dirName}/{img.FileName}.png");
                }
                else
                {
                    img.Img.Save($"{savePath}/{img.FileName}.png");
                }
            }         
            else
            {
                var form = new FormPrint(img);
                form.ShowDialog();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                textBox1.Text = img.FileName;
            }
        }

        private void rbtnNew_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnNew.Checked)
            {
                textBox1.Text = User.GetFileName;
            }
        }
    }
}
