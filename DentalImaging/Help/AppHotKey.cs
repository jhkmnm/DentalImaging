using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DentalImaging.Help
{
    public class AppHotKey
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")] //申明API函数
        public static extern bool RegisterHotKey(
        IntPtr hWnd, // handle to window
        int id, // hot key identifier
        uint fsModifiers, // key-modifier options
        Keys vk // virtual-key code
       );
        [System.Runtime.InteropServices.DllImport("user32.dll")] //申明API函数
        public static extern bool UnregisterHotKey(
         IntPtr hWnd, // handle to window
         int id // hot key identifier
        );
    }

    public enum KeyModifiers //组合键枚举
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }

}
