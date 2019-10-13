namespace DentalImaging
{
    partial class FormEditPicture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEditPicture));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnColor = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.ddlPenSize = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlText = new System.Windows.Forms.ComboBox();
            this.ddlRectangle = new System.Windows.Forms.ComboBox();
            this.ddlEllipse = new System.Windows.Forms.ComboBox();
            this.ddlLine = new System.Windows.Forms.ComboBox();
            this.btnRecover = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(2, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1111, 447);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(302, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(528, 441);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.imageBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnColor);
            this.panel2.Controls.Add(this.btnDel);
            this.panel2.Controls.Add(this.ddlPenSize);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.ddlText);
            this.panel2.Controls.Add(this.ddlRectangle);
            this.panel2.Controls.Add(this.ddlEllipse);
            this.panel2.Controls.Add(this.ddlLine);
            this.panel2.Controls.Add(this.btnRecover);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Location = new System.Drawing.Point(2, 451);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1021, 136);
            this.panel2.TabIndex = 1;
            // 
            // btnColor
            // 
            this.btnColor.BackColor = System.Drawing.Color.Black;
            this.btnColor.Location = new System.Drawing.Point(778, 44);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(42, 42);
            this.btnColor.TabIndex = 11;
            this.btnColor.Text = "删除";
            this.btnColor.UseVisualStyleBackColor = false;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(914, 47);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(42, 42);
            this.btnDel.TabIndex = 10;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // ddlPenSize
            // 
            this.ddlPenSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlPenSize.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddlPenSize.FormattingEnabled = true;
            this.ddlPenSize.ItemHeight = 29;
            this.ddlPenSize.Location = new System.Drawing.Point(829, 49);
            this.ddlPenSize.Name = "ddlPenSize";
            this.ddlPenSize.Size = new System.Drawing.Size(58, 37);
            this.ddlPenSize.TabIndex = 9;
            this.ddlPenSize.SelectedIndexChanged += new System.EventHandler(this.ddlPenSize_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.label1);
            this.panel4.Location = new System.Drawing.Point(572, 31);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(110, 84);
            this.panel4.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // ddlText
            // 
            this.ddlText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlText.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddlText.FormattingEnabled = true;
            this.ddlText.ItemHeight = 29;
            this.ddlText.Location = new System.Drawing.Point(466, 52);
            this.ddlText.Name = "ddlText";
            this.ddlText.Size = new System.Drawing.Size(58, 37);
            this.ddlText.TabIndex = 6;
            this.ddlText.SelectedIndexChanged += new System.EventHandler(this.DrawTypeSelected);
            // 
            // ddlRectangle
            // 
            this.ddlRectangle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlRectangle.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddlRectangle.FormattingEnabled = true;
            this.ddlRectangle.ItemHeight = 29;
            this.ddlRectangle.Location = new System.Drawing.Point(402, 52);
            this.ddlRectangle.Name = "ddlRectangle";
            this.ddlRectangle.Size = new System.Drawing.Size(58, 37);
            this.ddlRectangle.TabIndex = 5;
            this.ddlRectangle.SelectedIndexChanged += new System.EventHandler(this.DrawTypeSelected);
            // 
            // ddlEllipse
            // 
            this.ddlEllipse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlEllipse.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddlEllipse.FormattingEnabled = true;
            this.ddlEllipse.ItemHeight = 29;
            this.ddlEllipse.Location = new System.Drawing.Point(338, 52);
            this.ddlEllipse.Name = "ddlEllipse";
            this.ddlEllipse.Size = new System.Drawing.Size(58, 37);
            this.ddlEllipse.TabIndex = 4;
            this.ddlEllipse.SelectedIndexChanged += new System.EventHandler(this.DrawTypeSelected);
            // 
            // ddlLine
            // 
            this.ddlLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLine.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddlLine.FormattingEnabled = true;
            this.ddlLine.ItemHeight = 29;
            this.ddlLine.Location = new System.Drawing.Point(274, 52);
            this.ddlLine.Name = "ddlLine";
            this.ddlLine.Size = new System.Drawing.Size(58, 37);
            this.ddlLine.TabIndex = 3;
            this.ddlLine.SelectedIndexChanged += new System.EventHandler(this.DrawTypeSelected);
            // 
            // btnRecover
            // 
            this.btnRecover.Location = new System.Drawing.Point(185, 52);
            this.btnRecover.Name = "btnRecover";
            this.btnRecover.Size = new System.Drawing.Size(42, 42);
            this.btnRecover.TabIndex = 2;
            this.btnRecover.Text = "恢复";
            this.btnRecover.UseVisualStyleBackColor = true;
            this.btnRecover.Click += new System.EventHandler(this.btnRecover_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(117, 52);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(42, 42);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "撤销";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(22, 52);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(42, 42);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "选择";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnSaveAndClose);
            this.panel3.Controls.Add(this.btnClear);
            this.panel3.Location = new System.Drawing.Point(1029, 451);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(84, 136);
            this.panel3.TabIndex = 2;
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Location = new System.Drawing.Point(15, 76);
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.Size = new System.Drawing.Size(55, 42);
            this.btnSaveAndClose.TabIndex = 12;
            this.btnSaveAndClose.Text = "保存并关闭";
            this.btnSaveAndClose.UseVisualStyleBackColor = true;
            this.btnSaveAndClose.Click += new System.EventHandler(this.btnSaveAndClose_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(20, 22);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(42, 42);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "全部清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // FormEditPicture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 589);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEditPicture";
            this.Text = "FormEditPicture";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormEditPicture_KeyPress);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.ComboBox ddlPenSize;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox ddlText;
        private System.Windows.Forms.ComboBox ddlRectangle;
        private System.Windows.Forms.ComboBox ddlEllipse;
        private System.Windows.Forms.ComboBox ddlLine;
        private System.Windows.Forms.Button btnRecover;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSaveAndClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.Label label1;
    }
}