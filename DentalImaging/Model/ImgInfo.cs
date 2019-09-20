using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using DentalImaging.Model;
using DentalImaging.Help;

namespace DentalImaging
{
    public class ImgInfo
    {
        public List<int> ToothIndex { get; set; }
        /// <summary>
        /// 0图片 1视频
        /// </summary>
        public int InfoType { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
        public string Note { get; set; }
        [JsonIgnore]
        public Bitmap Img { get; set; }
        public string ImgPath { get; set; }
        /// <summary>
        /// 牙齿索引
        /// </summary>
        public int Index { get; set; }
        public List<DrawControl> ImgEditInfo { get; set; }
        /// <summary>
        /// 图片索引
        /// </summary>
        //[JsonIgnore]        
        public int ID { get; set; }
        /// <summary>
        /// 是否存档
        /// </summary>
        public bool IsSaveToFile { get; set; }
        public string VideoPath { get; set; }
    }
}
