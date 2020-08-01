using LibUsbDotNet;
using LibUsbDotNet.Main;
using log4net;
using System;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DentalImaging.Help
{
    public class UsbHelp : IMessageBase
    {
        public static UsbDevice MyUsbDevice;

        public UsbHelp(int vid, int pid)
        {
            UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(vid, pid);
            MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);
        }

        public byte[] UsbMessage()
        {
            byte[] readBuffer = new byte[8];
            try
            {
                if(MyUsbDevice.IsOpen)
                {
                    UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
                    int bytesRead;
                    reader.Read(readBuffer, 5000, out bytesRead);
                    return readBuffer;
                }
                return null;
            }
            catch {
                return null;
            }            
        }

        public void SendMessage(string message)
        {
            byte[] writeBuffer = new byte[8];
            var str = message.Split(' ');
            for (int i = 0; i < 8; i++)
            {
                if (i < str.Length)
                {
                    writeBuffer[i] = Convert.ToByte(str[i]);
                }
            }
            int transferLength;
            if(MyUsbDevice.IsOpen)
            {
                UsbEndpointWriter writer = MyUsbDevice.OpenEndpointWriter(WriteEndpointID.Ep01);
                writer.Write(writeBuffer, 5000, out transferLength);
            }
        }

        public void Close()
        {
            MyUsbDevice.Close();
        }
    }

    public class UDPHelp : IMessageBase
    {
        UdpClient udpClient;
        IPEndPoint locatePoint;

        public UDPHelp(string locateIP, int locatePort)
        {
            IPAddress locateIp = IPAddress.Parse(locateIP);
            locatePoint = new IPEndPoint(locateIp, Convert.ToInt32(locatePort));
            udpClient = new UdpClient(locatePoint);            
        }

        public byte[] UsbMessage()
        {
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Any, 0);
            try
            {   
                var recBuffer = udpClient.Receive(ref remotePoint);
                if (recBuffer != null)
                {
                    if (recBuffer.Length > 8)
                        return recBuffer.Skip(0).Take(8).ToArray();
                    return recBuffer;
                }
                return null;
            }
            catch(Exception ex) {
                return null;
            }
        }

        public void SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message.Trim()))
                return;

            byte[] writeBuffer = new byte[8];
            var str = message.Split(' ');            
            for (int i = 0; i < 8; i++)
            {
                if (i < str.Length)
                {
                    writeBuffer[i] = Convert.ToByte(str[i]);
                }
            }
            IPAddress remoteIp = IPAddress.Parse("10.10.10.254");
            IPEndPoint remotePoint = new IPEndPoint(remoteIp, 3333);
            udpClient.Send(writeBuffer, writeBuffer.Length, remotePoint);
        }

        public void Close()
        {
            udpClient.Close();
        }
    }

    public class TcpHelp : IMessageBase
    {
        ILog logger;
        Socket tcpcz = null;
        public TcpHelp(string locateIP, int locatePort)
        {
            tcpcz = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpcz.Connect(locateIP, locatePort);//根据服务器的IP地址和端口号 异步连接服务器
            logger = LogManager.GetLogger(typeof(TcpHelp));
        }

        public void Close()
        {
            tcpcz.Close();
        }
                
        public void SendMessage(string message)
        {
            if(!message.StartsWith("{"))
            {
                var chars = message.Split(' ');
                message = string.Empty;
                foreach (var cha in chars)
                {
                    message += string.Format("{0:X}", Convert.ToInt32(cha)).PadLeft(2, '0');
                }
                message = "{\"mcucmd\":\"" + message + "\"}";
            }
            logger.Fatal("Send:" + message);
            try
            {
                tcpcz.Send(Encoding.UTF8.GetBytes(message));
            }
            catch(SocketException se)
            {                
                CommHelp.ShowError("失去连接，发送指令失败");
                throw;
            }
        }

        public byte[] UsbMessage()
        {
            byte[] readBuffer = new byte[33];
            var message = string.Empty;
            try
            {
                tcpcz.Receive(readBuffer);                
                message = Encoding.UTF8.GetString(readBuffer).Trim();
                logger.Fatal("Receive:" + message);
                var byt = new byte[8];
                if (message.StartsWith("{"))
                {
                    message = message.Replace("\n", "").Replace("\t", "").Replace("{\"mcucmd\":\"", "").Replace("\"}", "");
                    for (int i = 0; i < 16; i += 2)
                    {
                        var str = string.Format("{0}{1}", message[i], message[i + 1]);
                        byt[i / 2] = Convert.ToByte(Convert.ToInt32(str, 16));
                    }
                    return byt;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class COMHelp : IMessageBase
    {
        /// <summary>
        /// 数据位
        /// </summary>
        private int intDataBits = 8;
        /// <summary>
        /// 波特率
        /// </summary>
        private int intBaudRate = 115200;
        /// <summary>
        /// 串口数据访问类
        /// </summary>
        private SerialPort comPort = new SerialPort();        

        public COMHelp(string portName)
        {
            comPort.PortName = portName;
            comPort.DataBits = intDataBits;
            comPort.BaudRate = intBaudRate;
            comPort.ReadTimeout = 3000;
            comPort.Parity = Parity.None;
            comPort.StopBits = StopBits.One;
            comPort.Open();
        }

        public void Close()
        {
            if (comPort.IsOpen)
                comPort.Close();
        }

        public void SendMessage(string message)
        {
            byte[] writeBuffer = new byte[8];
            var str = message.Split(' ');
            for (int i = 0; i < 8; i++)
            {
                if (i < str.Length)
                {
                    writeBuffer[i] = Convert.ToByte(str[i]);
                }
            }
            if (comPort.IsOpen)
            {
                comPort.Write(writeBuffer, 0, writeBuffer.Length);
            }
        }

        public byte[] UsbMessage()
        {
            if (!comPort.IsOpen)
                comPort.Open();

            var readLength = comPort.BytesToRead;
            byte[] reDatas = new byte[readLength];
            comPort.Read(reDatas, 0, readLength);
            //StringBuilder sb = new StringBuilder();
            //for (int i = 0; i < reDatas.Length; i++)
            //{
            //    sb.AppendFormat("{0:X2}" + " ", reDatas[i]);
            //}
            //return sb.ToString();
            if (reDatas.Length >= 8)
                return reDatas.Skip(0).Take(8).ToArray();


            return new byte[8];
        }
    }
}
