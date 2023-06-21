using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class NotifyConData
    {
        public int cbSize { get; set; }
        public int hwnd { get; set; }
        public int uID { get; set; }
        public int uFlags { get; set; }
        public int uCallBackMessage { get; set; }
        public int hIcon { get; set; }
        public string szTip { get; set; }
        public int dwState { get; set; }
        public int dwStateMask { get; set; }
        public string szInfo { get; set; }
        public int uTimeOut { get; set; }
        public string szInfoTitle { get; set; }
        public int dwInfoFlags { get; set; }
    }
}
