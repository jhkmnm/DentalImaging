using DentalImaging.Help;
using DentalImaging.Model;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;

namespace DentalImaging.NewForm
{
    public partial class ReportViewerDialog : XtraForm
    {
        public ReportViewerDialog()
        {
            InitializeComponent();
        }

        public List<ReportSource> DataSourceDict { get; set; }
        public string ReportFilePath { get; set; }

        private void ReportViewerDialog_Load(object sender, EventArgs e)
        {
            this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            this.reportViewer1.ZoomMode = Microsoft.Reporting.WinForms.ZoomMode.Percent;
            this.reportViewer1.ZoomPercent = 100;

            reportSourceBindingSource1.DataSource = DataSourceDict;
            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReportViewerDialog_Shown(object sender, EventArgs e)
        {
            LanguageHelp.InitControlLanguage(this);
        }
    }
}
