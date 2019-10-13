namespace DentalImaging
{
    partial class FormParameter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtImgSavePath = new System.Windows.Forms.TextBox();
            this.btnImgSavePath = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(702, 428);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnImgSavePath);
            this.tabPage1.Controls.Add(this.txtImgSavePath);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(694, 402);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "一般";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图片保存路径";
            // 
            // txtImgSavePath
            // 
            this.txtImgSavePath.Location = new System.Drawing.Point(23, 47);
            this.txtImgSavePath.Name = "txtImgSavePath";
            this.txtImgSavePath.ReadOnly = true;
            this.txtImgSavePath.Size = new System.Drawing.Size(531, 21);
            this.txtImgSavePath.TabIndex = 1;
            // 
            // btnImgSavePath
            // 
            this.btnImgSavePath.Location = new System.Drawing.Point(560, 47);
            this.btnImgSavePath.Name = "btnImgSavePath";
            this.btnImgSavePath.Size = new System.Drawing.Size(38, 23);
            this.btnImgSavePath.TabIndex = 2;
            this.btnImgSavePath.Text = "...";
            this.btnImgSavePath.UseVisualStyleBackColor = true;
            this.btnImgSavePath.Click += new System.EventHandler(this.btnImgSavePath_Click);
            // 
            // FormParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 428);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormParameter";
            this.Text = "参数选择";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnImgSavePath;
        private System.Windows.Forms.TextBox txtImgSavePath;
        private System.Windows.Forms.Label label1;
    }
}