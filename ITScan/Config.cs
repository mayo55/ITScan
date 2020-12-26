using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace ITScan
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class Config
    {
        public static int Rotate { get; set;  }

        public static void LoadConfig()
        {
            Rotate = Properties.Settings.Default.Rotate;
        }
    }
}
