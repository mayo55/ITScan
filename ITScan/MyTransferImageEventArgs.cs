using System;
using System.Drawing;

namespace ITScan
{
    public class MyTransferImageEventArgs : EventArgs
    {
        public IntPtr HBitmap { get; private set; }
        public bool ContinueScanning { get; set; }

        public MyTransferImageEventArgs(IntPtr hBitmap, bool continueScanning)
        {
            this.HBitmap = hBitmap;
            this.ContinueScanning = continueScanning;
        }
    }
}
