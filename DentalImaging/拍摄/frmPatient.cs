using DentalImaging.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalImaging
{
    public partial class frmPatient : Form
    {
        private int childFormNumber = 0;

        public frmPatient()
        {
            InitializeComponent();

            this.Text = $"{Environment.UserName} - {User.CurrentPatient.FName}{User.CurrentPatient.Name}";

            FormToothMap map = new FormToothMap();
            map.MdiParent = this;
            map.TopMost = true;
            map.Show();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "窗口 " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            var form = new FormPicture();
            if(form.ShowDialog() == DialogResult.OK)
            {
                foreach(var img in form.imgList)
                {
                    if(img.InfoType == 0)
                    {
                        var imgForm = new FormImg(img);
                        imgForm.MdiParent = this;
                        imgForm.Show();
                    }
                }
                var conForm = FormEditControl.GetForm();
                conForm.MdiParent = MdiParent;
                conForm.Show();
            }
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog file = new SaveFileDialog())
            {
                file.Filter = "BMP Bitmap(*.bmp)|*.bmp|CUR Cursor(*.cur)|*.cur|GIF (*.gif)|*.gif|ICO Icon(*.ico)|*.ico|JPG JPEG Image(*.jpg;*.jpeg;*jpe)|*.jpg;*.jpeg;*jpe|PNG Portable NetworkGraphics(*.png)|*.png";
                file.FileName = User.CurrentImg.Img.FileName;
                file.RestoreDirectory = true;
                if(file.ShowDialog() == DialogResult.OK)
                {
                    var saveFile = file.FileName;
                    User.CurrentImg.Img.Img.Save(saveFile);
                }
            }
        }

        private void tsmParameter_Click(object sender, EventArgs e)
        {
            FormParameter form = new FormParameter();
            form.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog file = new OpenFileDialog())
            {
                file.Filter = "BMP Bitmap(*.bmp)|*.bmp|CUR Cursor(*.cur)|*.cur|GIF (*.gif)|*.gif|ICO Icon(*.ico)|*.ico|JPG JPEG Image(*.jpg;*.jpeg;*jpe)|*.jpg;*.jpeg;*jpe|PNG Portable NetworkGraphics(*.png)|*.png";
                if(file.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream f = new FileStream(file.FileName, FileMode.Open))
                    {
                        Image img = Image.FromStream(f);
                        FormImg form = new FormImg(new ImgInfo { Date = DateTime.Now, Img = (Bitmap)img });
                        form.Show();
                    }
                    var conForm = FormEditControl.GetForm();
                    conForm.MdiParent = MdiParent;
                    conForm.Show();
                }
            }
        }
    }
}
