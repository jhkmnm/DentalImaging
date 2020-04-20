using System;
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
            OrdersB.Add(OrderType.PReturn_Rule_See, "0 2 0 1 1 4 0 0");
                  
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
