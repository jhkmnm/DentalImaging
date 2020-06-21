﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentalImaging.Model
{
    public class UsbMessage
    {
        public static Dictionary<string, OrderType> Orders = new Dictionary<string, OrderType>();
        public static Dictionary<OrderType, string> OrdersB = new Dictionary<OrderType, string>();

        static UsbMessage()
        {
            Orders.Add("1 1 1 1 1 1 1 1", OrderType.Sleep);
            Orders.Add("0 0 1 0 0 1 0 0", OrderType.PowerOff);
            Orders.Add("1 1 0 1 1 4 0 0", OrderType.HeartBeat);

            Orders.Add("0 1 0 0 0 1 0 0", OrderType.MTP_Rule_View);     //微距预览
            Orders.Add("0 2 0 0 0 2 0 0", OrderType.MTP_Rule_See);      //拍照

            Orders.Add("1 4 0 0 0 5 0 0", OrderType.MTP_UV_View);
            Orders.Add("1 8 0 0 0 9 0 0", OrderType.MTP_UV_See);

            //"2 16 0 0 0 18 0 0"
            Orders.Add("2 16 0 0 0 18 0 0", OrderType.MTP_ICON_View);
            Orders.Add("2 32 0 0 0 34 0 0", OrderType.MTP_ICON_See);

            Orders.Add("0 1 0 1 1 3 0 0", OrderType.PReturn_Rule_View);     //微距预览返回
            Orders.Add("0 2 0 1 1 4 0 0", OrderType.PReturn_Rule_See);  //拍照回发

            Orders.Add("1 4 0 1 1 7 0 0", OrderType.PReturn_UV_View);
            Orders.Add("1 8 0 1 1 11 0 0", OrderType.PReturn_UV_See);

            Orders.Add("2 16 0 1 1 20 0 0", OrderType.PReturn_ICON_View);
            Orders.Add("2 32 0 1 1 36 0 0", OrderType.PReturn_ICON_See);

            Orders.Add("0 1 0 1 0 2 0 0", OrderType.PSend_Rule_View);
            Orders.Add("0 2 0 1 0 3 0 0", OrderType.PSend_Rule_See);

            Orders.Add("1 4 0 1 0 6 0 0", OrderType.PSend_UV_View);
            Orders.Add("1 8 0 1 0 10 0 0", OrderType.PSend_UV_See);

            Orders.Add("2 16 0 1 0 19 0 0", OrderType.PSend_ICON_View);
            Orders.Add("2 32 0 1 0 35 0 0", OrderType.PSend_ICON_See);

            //          5A A5  F0  A1  11 A5  5A
            Orders.Add("90 165 8 240 161 17 165 90", OrderType.MTP_Rule_See);     //拍照
            //          5A A5  8 F1  A1  11 A5  5A
            Orders.Add("90 165 8 241 161 17 165 90", OrderType.MTP_Open_SB);     //打开水泵
            //          5A A5  8 F1  A1  00 A5  5A
            Orders.Add("90 165 8 241 161 0 165 90", OrderType.MTP_Close_SB);     //关闭水泵
            //          5A A5  8 F2  A1  11 A5  5A
            Orders.Add("90 165 8 242 161 17 165 90", OrderType.MTP_Open_QB);     //打开气泵
            //          5A A5  8 F2  A1  00 A5 5A
            Orders.Add("90 165 8 242 161 0 165 90", OrderType.MTP_Close_QB);     //关闭气泵

            Orders.Add("90 165 8 255 161 17 165 90", OrderType.MTP_Dri_Test);     //设备连接
            Orders.Add("90 165 8 243 161 17 165 90", OrderType.MTP_Con_Ready);     //通讯readly


            OrdersB.Add(OrderType.Sleep, "1 1 1 1 1 1 1 1");

            OrdersB.Add(OrderType.PowerOff, "0 0 1 0 0 1 0 0");
            OrdersB.Add(OrderType.HeartBeat, "1 1 0 1 1 4 0 0");
                  
            OrdersB.Add(OrderType.MTP_Rule_View, "0 1 0 0 0 1 0 0");
            OrdersB.Add(OrderType.MTP_Rule_See, "0 2 0 0 0 2 0 0");
                  
            OrdersB.Add(OrderType.MTP_UV_View, "1 4 0 0 0 5 0 0");
            OrdersB.Add(OrderType.MTP_UV_See, "1 8 0 0 0 9 0 0");
                  
            OrdersB.Add(OrderType.MTP_ICON_View, "2 16 0 0 0 18 0 0");
            OrdersB.Add(OrderType.MTP_ICON_See, "2 32 0 0 0 34 0 0");
                  
            OrdersB.Add(OrderType.PReturn_Rule_View, "0 1 0 1 1 3 0 0");
            //OrdersB.Add(OrderType.PReturn_Rule_See, "0 2 0 1 1 4 0 0");
                  
            OrdersB.Add(OrderType.PReturn_UV_View, "1 4 0 1 1 7 0 0");
            OrdersB.Add(OrderType.PReturn_UV_See, "1 8 0 1 1 11 0 0");
                  
            OrdersB.Add(OrderType.PReturn_ICON_View, "2 16 0 1 1 20 0 0");
            OrdersB.Add(OrderType.PReturn_ICON_See, "2 32 0 1 1 36 0 0");
                  
            OrdersB.Add(OrderType.PSend_Rule_View, "0 1 0 1 0 2 0 0");
            OrdersB.Add(OrderType.PSend_Rule_See, "0 2 0 1 0 3 0 0");
                  
            OrdersB.Add(OrderType.PSend_UV_View, "1 4 0 1 0 6 0 0");
            OrdersB.Add(OrderType.PSend_UV_See, "1 8 0 1 0 10 0 0");
                  
            OrdersB.Add(OrderType.PSend_ICON_View, "2 16 0 1 0 19 0 0");
            OrdersB.Add(OrderType.PSend_ICON_See, "2 32 0 1 0 36 0 0");

            OrdersB.Add(OrderType.PSend_Close_Camenable, "{\"Camenable\":\"0\"}");
            OrdersB.Add(OrderType.PSend_Open_Camenable, "{\"Camenable\":\"1\"}");

                                        //          5A  A5  F0  A2  11 A5  5A
            OrdersB.Add(OrderType.PReturn_Rule_See, "90 165 8 240 162 17 165 90");     //拍照
                                        //          5A  A5 8 F1   A2 11 A5 5A
            OrdersB.Add(OrderType.PReturn_Open_SB, "90 165 8 241 162 17 165 90");     //打开水泵
                                        //          5A  A5  8 F1   A2 00 A5 5A
            OrdersB.Add(OrderType.PReturn_Close_SB, "90 165 8 241 162 0 165 90");     //关闭水泵
                                        //          5A A5   8 F2   A2 11 A5 5A
            OrdersB.Add(OrderType.PReturn_Open_QB, "90 165 8 242 162 17 165 90");     //打开气泵
                                        //          5A A5   8 F2  A2 00 A5 5A
            OrdersB.Add(OrderType.PReturn_Close_QB, "90 165 8 242 162 0 165 90");     //关闭气泵

            OrdersB.Add(OrderType.PSend_Dri_Test, "90 165 8 255 162 17 165 90");     //设备连接
            OrdersB.Add(OrderType.PSend_Con_Ready, "90 165 8 243 161 17 165 90");     //通讯readly
        }        
    }

    public enum OrderType
    {
        /// <summary>
        /// 关机
        /// </summary>
        PowerOff,
        /// <summary>
        /// 心跳
        /// </summary>
        HeartBeat,        

        #region MCU发送给PC
        /// <summary>
        /// MCU发送给PC, 常规预览
        /// </summary>
        MTP_Rule_View,
        /// <summary>
        /// MCU发送给PC, 常规查看
        /// </summary>
        MTP_Rule_See,
        /// <summary>
        /// MCU发送给PC, UV预览
        /// </summary>
        MTP_UV_View,
        /// <summary>
        /// MCU发送给PC, UV查看
        /// </summary>
        MTP_UV_See,
        /// <summary>
        /// MCU发送给PC, UV预览
        /// </summary>
        MTP_ICON_View,
        /// <summary>
        /// MCU发送给PC, UV查看
        /// </summary>
        MTP_ICON_See,
        /// <summary>
        /// 打开水泵
        /// </summary>
        MTP_Open_SB,
        /// <summary>
        /// 关闭水泵
        /// </summary>
        MTP_Close_SB,
        /// <summary>
        /// 打开气泵
        /// </summary>
        MTP_Open_QB,
        /// <summary>
        /// 关闭气泵
        /// </summary>
        MTP_Close_QB,
        /// <summary>
        /// 设备连接
        /// </summary>
        MTP_Dri_Test,
        /// <summary>
        /// 通讯连接
        /// </summary>
        MTP_Con_Ready,
        #endregion

        #region PC返回
        /// <summary>
        /// PC返回, 常规预览
        /// </summary>
        PReturn_Rule_View,
        /// <summary>
        /// PC返回, 常规查看
        /// </summary>
        PReturn_Rule_See,
        /// <summary>
        /// PC返回, UV预览
        /// </summary>
        PReturn_UV_View,
        /// <summary>
        /// PC返回, UV查看
        /// </summary>
        PReturn_UV_See,
        /// <summary>
        /// PC返回, UV预览
        /// </summary>
        PReturn_ICON_View,
        /// <summary>
        /// PC返回, UV查看
        /// </summary>
        PReturn_ICON_See,
        /// <summary>
        /// 打开水泵
        /// </summary>
        PReturn_Open_SB,
        /// <summary>
        /// 关闭水泵
        /// </summary>
        PReturn_Close_SB,
        /// <summary>
        /// 打开气泵
        /// </summary>
        PReturn_Open_QB,
        /// <summary>
        /// 关闭气泵
        /// </summary>
        PReturn_Close_QB,        
        #endregion

        #region PC发送
        /// <summary>
        /// PC发送, 常规预览
        /// </summary>
        PSend_Rule_View,
        /// <summary>
        /// PC发送, 常规查看
        /// </summary>
        PSend_Rule_See,
        /// <summary>
        /// PC发送, UV预览
        /// </summary>
        PSend_UV_View,
        /// <summary>
        /// PC发送, UV查看
        /// </summary>
        PSend_UV_See,
        /// <summary>
        /// PC发送, UV预览
        /// </summary>
        PSend_ICON_View,
        /// <summary>
        /// PC发送, UV查看
        /// </summary>
        PSend_ICON_See,
        /// <summary>
        /// 设备连接
        /// </summary>
        PSend_Dri_Test,
        /// <summary>
        /// 通讯连接
        /// </summary>
        PSend_Con_Ready,
        #endregion

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        PSend_Close_Camenable,
        /// <summary>
        /// 开启摄像头
        /// </summary>
        PSend_Open_Camenable,

        /// <summary>
        /// 休眠
        /// </summary>
        Sleep
    }
}
