using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DentalImaging.Help
{
    public interface IMessageBase
    {
        byte[] UsbMessage();
        void SendMessage(string message);

        void Close();
    }
}
