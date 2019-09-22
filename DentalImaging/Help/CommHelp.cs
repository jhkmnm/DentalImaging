using System.Windows.Forms;

namespace DentalImaging.Help
{
    public static class CommHelp
    {
        public static DialogResult ShowYesNoAndTips(string message)
        {
            return DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        public static void ShowWarning(string message)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(string message)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowTips(string message)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ShowYesNoAndWarning(string message)
        {
            return DevExpress.XtraEditors.XtraMessageBox.Show(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }
    }
}
