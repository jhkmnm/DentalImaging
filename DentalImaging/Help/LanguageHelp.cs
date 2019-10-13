using DentalImaging.Model;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DentalImaging.Help
{
    public class LanguageHelp
    {
        static string filePath = $"{Environment.CurrentDirectory}\\Language\\cn\\Basic.json";
        static Dictionary<string, string> resources = new Dictionary<string, string>();
        static Dictionary<string, string> abcList = new Dictionary<string, string>();

        static LanguageHelp()
        {
            LoadLanguage();
        }

        /// <summary>
        /// 重新加载语言
        /// </summary>
        /// <returns></returns>
        public static bool ReLoadLanguage()
        {
            resources.Clear();
            try
            {
                LoadLanguage();                
            }
            catch { }
            return resources.Count > 0;
        }

        private static void LoadLanguage()
        {
            filePath = $"{Environment.CurrentDirectory}\\Language\\{User.Language}\\Basic.json";

            if (File.Exists(filePath))
            {
                var content = File.ReadAllText(filePath, Encoding.UTF8);
                if (!string.IsNullOrEmpty(content))
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                    foreach (string key in dict.Keys)
                    {
                        //遍历集合如果语言资源键值不存在，则创建，否则更新
                        if (!resources.ContainsKey(key))
                        {
                            resources.Add(key, dict[key]);
                        }
                        else
                        {
                            resources[key] = dict[key];
                        }
                    }
                }
            }
        }

        public static void Save()
        {
            var text = string.Join(Environment.NewLine, abcList.Select(s => $"\"{s.Key}\": \"\""));
            File.WriteAllText($"{Environment.CurrentDirectory}\\Language\\en\\Basic.json", text);
        }

        /// <summary>
        /// 设置窗体上所有控件的语言
        /// </summary>
        /// <param name="control"></param>
        public static void InitControlLanguage(Control control)
        {
            if(control.CompanyName == "Developer Express Inc.")
            {
                switch(control.ProductName)
                {
                    case "DevExpress.XtraBars.Design":
                        var ribbon = control as DevExpress.XtraBars.Ribbon.RibbonControl;
                        if (ribbon != null)
                        {
                            foreach (BarItem item in ribbon.Items)
                            {
                                SetDevExpressLanguage(item);
                            }
                            foreach(RibbonPage item in ribbon.Pages)
                            {
                                var text = item.Tag ?? item.Text;
                                item.Text = GetTextLanguage(text.ToString());
                            }
                        }
                        break;
                    default:
                        SetControlLanguage(control);
                        break;
                }                
            }
            else
            {
                SetControlLanguage(control);
                foreach (Control ctrl in control.Controls)
                {
                    InitControlLanguage(ctrl);
                }
            }            
        }

        private static void SetControlLanguage(Control control)
        {
            //if(!abcList.ContainsKey(control.Text))
            //    abcList.Add(control.Text, "");
            var text = control.Tag ?? control.Text;
            control.Text = GetTextLanguage(text.ToString());
        }

        private static void SetDevExpressLanguage(BarItem item)
        {
            var text = item.Tag ?? item.Caption;
            item.Caption = GetTextLanguage(text.ToString());
        }

        /// <summary>
        /// 返回指定字符的当前语言
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetTextLanguage(string text)
        {
            if (resources.Keys.Contains(text))
                return resources[text];
            return text;
        }

        
    }
}
