using System.ComponentModel;

namespace DentalImaging.新界面
{
    partial class ChannelChoose
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbtWIFI = new System.Windows.Forms.RadioButton();
            this.rbtUSB = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbxWIFI = new System.Windows.Forms.ListBox();
            this.btnRef = new System.Windows.Forms.Button();
            this.btnLink = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(167, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "请选择连接方式";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbtWIFI);
            this.panel1.Controls.Add(this.rbtUSB);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 136);
            this.panel1.TabIndex = 1;
            // 
            // rbtWIFI
            // 
            this.rbtWIFI.AutoSize = true;
            this.rbtWIFI.Location = new System.Drawing.Point(312, 41);
            this.rbtWIFI.Name = "rbtWIFI";
            this.rbtWIFI.Size = new System.Drawing.Size(51, 18);
            this.rbtWIFI.TabIndex = 4;
            this.rbtWIFI.TabStop = true;
            this.rbtWIFI.Text = "WIFI";
            this.rbtWIFI.UseVisualStyleBackColor = true;
            // 
            // rbtUSB
            // 
            this.rbtUSB.AutoSize = true;
            this.rbtUSB.Location = new System.Drawing.Point(66, 41);
            this.rbtUSB.Name = "rbtUSB";
            this.rbtUSB.Size = new System.Drawing.Size(47, 18);
            this.rbtUSB.TabIndex = 3;
            this.rbtUSB.TabStop = true;
            this.rbtUSB.Text = "USB";
            this.rbtUSB.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(170, 96);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(87, 27);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbxWIFI);
            this.panel2.Controls.Add(this.btnRef);
            this.panel2.Controls.Add(this.btnLink);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(3, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(418, 262);
            this.panel2.TabIndex = 2;
            this.panel2.Visible = false;
            // 
            // lbxWIFI
            // 
            this.lbxWIFI.FormattingEnabled = true;
            this.lbxWIFI.ItemHeight = 14;
            this.lbxWIFI.Location = new System.Drawing.Point(42, 38);
            this.lbxWIFI.Name = "lbxWIFI";
            this.lbxWIFI.Size = new System.Drawing.Size(215, 214);
            this.lbxWIFI.TabIndex = 8;
            // 
            // btnRef
            // 
            this.btnRef.Location = new System.Drawing.Point(312, 38);
            this.btnRef.Name = "btnRef";
            this.btnRef.Size = new System.Drawing.Size(72, 27);
            this.btnRef.TabIndex = 7;
            this.btnRef.Text = "刷新";
            this.btnRef.UseVisualStyleBackColor = true;
            this.btnRef.Click += new System.EventHandler(this.btnRef_Click);
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(312, 74);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(72, 27);
            this.btnLink.TabIndex = 6;
            this.btnLink.Text = "连接";
            this.btnLink.UseVisualStyleBackColor = true;
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "请选择热点网络";
            // 
            // ChannelChoose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 281);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MaximizeBox = false;
            this.Name = "ChannelChoose";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChannelChoose";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.RadioButton rbtWIFI;
        private System.Windows.Forms.RadioButton rbtUSB;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lbxWIFI;
        private System.Windows.Forms.Button btnRef;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.Label label2;
    }
}