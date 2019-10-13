namespace DentalImaging
{
    partial class FormSaveImg
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
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnNew = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ddlTooth6 = new System.Windows.Forms.ComboBox();
            this.ddlTooth5 = new System.Windows.Forms.ComboBox();
            this.ddlTooth4 = new System.Windows.Forms.ComboBox();
            this.ddlTooth3 = new System.Windows.Forms.ComboBox();
            this.ddlTooth2 = new System.Windows.Forms.ComboBox();
            this.ddlTooth1 = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "日期";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "yyyy-MM-dd";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(14, 26);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(84, 21);
            this.dtpDate.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnNew);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(184, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 64);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "文件";
            // 
            // rbtnNew
            // 
            this.rbtnNew.AutoSize = true;
            this.rbtnNew.Checked = true;
            this.rbtnNew.Location = new System.Drawing.Point(150, 39);
            this.rbtnNew.Name = "rbtnNew";
            this.rbtnNew.Size = new System.Drawing.Size(59, 16);
            this.rbtnNew.TabIndex = 2;
            this.rbtnNew.TabStop = true;
            this.rbtnNew.Text = "新文件";
            this.rbtnNew.UseVisualStyleBackColor = true;
            this.rbtnNew.CheckedChanged += new System.EventHandler(this.rbtnNew_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Enabled = false;
            this.radioButton1.Location = new System.Drawing.Point(150, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(71, 16);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.Text = "同一文件";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(15, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "注释";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(14, 79);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(322, 147);
            this.textBox2.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ddlTooth6);
            this.groupBox2.Controls.Add(this.ddlTooth5);
            this.groupBox2.Controls.Add(this.ddlTooth4);
            this.groupBox2.Controls.Add(this.ddlTooth3);
            this.groupBox2.Controls.Add(this.ddlTooth2);
            this.groupBox2.Controls.Add(this.ddlTooth1);
            this.groupBox2.Location = new System.Drawing.Point(14, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(322, 58);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "多个牙";
            // 
            // ddlTooth6
            // 
            this.ddlTooth6.FormattingEnabled = true;
            this.ddlTooth6.Location = new System.Drawing.Point(251, 25);
            this.ddlTooth6.Name = "ddlTooth6";
            this.ddlTooth6.Size = new System.Drawing.Size(43, 20);
            this.ddlTooth6.TabIndex = 5;
            // 
            // ddlTooth5
            // 
            this.ddlTooth5.FormattingEnabled = true;
            this.ddlTooth5.Location = new System.Drawing.Point(202, 25);
            this.ddlTooth5.Name = "ddlTooth5";
            this.ddlTooth5.Size = new System.Drawing.Size(43, 20);
            this.ddlTooth5.TabIndex = 4;
            // 
            // ddlTooth4
            // 
            this.ddlTooth4.FormattingEnabled = true;
            this.ddlTooth4.Location = new System.Drawing.Point(153, 25);
            this.ddlTooth4.Name = "ddlTooth4";
            this.ddlTooth4.Size = new System.Drawing.Size(43, 20);
            this.ddlTooth4.TabIndex = 3;
            // 
            // ddlTooth3
            // 
            this.ddlTooth3.FormattingEnabled = true;
            this.ddlTooth3.Location = new System.Drawing.Point(104, 25);
            this.ddlTooth3.Name = "ddlTooth3";
            this.ddlTooth3.Size = new System.Drawing.Size(43, 20);
            this.ddlTooth3.TabIndex = 2;
            // 
            // ddlTooth2
            // 
            this.ddlTooth2.FormattingEnabled = true;
            this.ddlTooth2.Location = new System.Drawing.Point(55, 25);
            this.ddlTooth2.Name = "ddlTooth2";
            this.ddlTooth2.Size = new System.Drawing.Size(43, 20);
            this.ddlTooth2.TabIndex = 1;
            // 
            // ddlTooth1
            // 
            this.ddlTooth1.FormattingEnabled = true;
            this.ddlTooth1.Location = new System.Drawing.Point(6, 25);
            this.ddlTooth1.Name = "ddlTooth1";
            this.ddlTooth1.Size = new System.Drawing.Size(43, 20);
            this.ddlTooth1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(382, 232);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(382, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // FormSaveImg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 295);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSaveImg";
            this.Text = "保存图像";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnNew;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox ddlTooth6;
        private System.Windows.Forms.ComboBox ddlTooth5;
        private System.Windows.Forms.ComboBox ddlTooth4;
        private System.Windows.Forms.ComboBox ddlTooth3;
        private System.Windows.Forms.ComboBox ddlTooth2;
        private System.Windows.Forms.ComboBox ddlTooth1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button button2;
    }
}