using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DentalImaging.Help
{
    public static class CommHelp
    {
        public static DialogResult ShowYesNoAndTips(string message)
        {
            return MessageBoxShow(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        public static void ShowWarning(string message)
        {
            MessageBoxShow(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowError(string message)
        {
            MessageBoxShow(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowTips(string message)
        {
            MessageBoxShow(message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult ShowYesNoAndWarning(string message)
        {
            return MessageBoxShow(message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        }        

        private static DialogResult MessageBoxShow(string msg, string title, MessageBoxButtons btn, MessageBoxIcon icon)
        {
            return XtraMessageBox.Show(LanguageHelp.GetTextLanguage(msg), LanguageHelp.GetTextLanguage(title), btn, icon);
        }
    }
}
