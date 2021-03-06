﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;

namespace TwainDotNet.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class BitmapFileHeader
    {
        public short Type;
        public int Size;
        public short Reserved1;
        public short Reserved2;
        public int OffBits;

        public override string ToString()
        {
            return string.Format(
                "t:{0} s:{1} r1:{2} r2:{3} o:{4}",
                Type,
                Size,
                Reserved1,
                Reserved2,
                OffBits);
        }
    }
}
