using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentalImaging.Model
{
    public class VideoParm
    {
        public ParmType Type { get; set; }

        public bool IsChange { get; set; }
        /// <summary>
        /// 亮度
        /// </summary>
        public int Liangdu { get; set; }
        /// <summary>
        /// 对比度
        /// </summary>
        public int Duibidu { get; set; }
        ///// <summary>
        ///// 色调
        ///// </summary>
        //public int Sediao { get; set; }
        /// <summary>
        /// 饱和度
        /// </summary>
        public int Baohedu { get; set; }
        /// <summary>
        /// 清晰度
        /// </summary>
        public int Qingxidu { get; set; }
        /// <summary>
        /// 伽马
        /// </summary>
        public int Gama { get; set; }
        /// <summary>
        /// 白平衡
        /// </summary>
        public int Baipingheng { get; set; }
        /// <summary>
        /// 白平衡自动
        /// </summary>
        public bool BaipinghengAuto { get; set; }
        /// <summary>
        /// 逆光对比
        /// </summary>
        public int Niguangduibi { get; set; }
        /// <summary>
        /// 增益
        /// </summary>
        public int Zengyi { get; set; }
        /// <summary>
        /// 曝光
        /// </summary>
        public int Baoguang { get; set; }
        /// <summary>
        /// 曝光自动
        /// </summary>
        public bool BaoguangAuto { get; set; }
        /// <summary>
        /// 分辨率
        /// </summary>
        public string Fenbian { get; set; }
        /// <summary>
        /// 发射功率
        /// </summary>
        public int Gonglv { get; set; }
        /// <summary>
        /// 对焦
        /// </summary>
        public int Duijiao { get; set; }
        /// <summary>
        /// 曝光自动
        /// </summary>
        public bool DuijiaoAuto { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if ((obj.GetType().Equals(this.GetType())) == false)
            {
                return false;
            }
            VideoParm temp = (VideoParm)obj;

            return this.Baipingheng.Equals(temp.Baipingheng) && 
                (this.Baoguang.Equals(temp.Baoguang) || this.BaoguangAuto.Equals(temp.BaoguangAuto))&& 
                this.Baohedu.Equals(temp.Baohedu) && 
                this.Duibidu.Equals(temp.Duibidu) && 
                this.Gama.Equals(temp.Gama) && 
                this.Liangdu.Equals(temp.Liangdu) && 
                this.Niguangduibi.Equals(temp.Niguangduibi) && 
                this.Qingxidu.Equals(temp.Qingxidu) && 
                //this.Sediao.Equals(temp.Sediao) &&
                this.Fenbian.Equals(temp.Fenbian) &&
                this.Gonglv.Equals(temp.Gonglv) &&
                this.Duijiao.Equals(temp.Duijiao) &&
                (this.Baipingheng.Equals(temp.Baipingheng) || this.BaipinghengAuto.Equals(temp.BaipinghengAuto)) &&
                (this.Duijiao.Equals(temp.Duijiao) || this.DuijiaoAuto.Equals(temp.DuijiaoAuto));

        }
    }

    public enum ParmType
    {
        /// <summary>
        /// 微距
        /// </summary>
        A = 1,
        /// <summary>
        /// 全景
        /// </summary>
        B,
        /// <summary>
        /// 人像
        /// </summary>
        C
    }
}
