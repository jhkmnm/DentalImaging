using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalImaging.Model
{
    public class Patient
    {
        /// <summary>
        /// 病人编号
        /// </summary>
        public int Number { get; set; }

        public string FName { get; set; }

        public string Name { get; set; }

        public string BirthDay { get; set; }

        /// <summary>
        /// 社保号
        /// </summary>
        public string SECU { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Tel1 { get; set; }
        public string Tel2 { get; set; }
        public string Photo { get; set; }
        public string Yijian { get; set; }

        public List<CaseHistory> Historys { get; set; }
    }

    public class CaseHistory
    {
        /// <summary>
        /// 病历编号  时间精确到秒
        /// </summary>
        public string HistoryNo { get; set; }
        /// <summary>
        /// 处理意见
        /// </summary>
        public string Yijian { get; set; }
        [JsonIgnore]
        public List<ImgInfo> imgInfos { get; set; }
    }
}
