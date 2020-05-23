using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using TwainDotNet.Win32;
using log4net;

namespace ITScan
{
    public class MyBitmapRenderer : IDisposable
    {
        /// <summary>
        /// The logger for this class.
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(MyBitmapRenderer));

        IntPtr _dibHandle;
        IntPtr _bitmapPointer;
        IntPtr _pixelInfoPointer;
        Rectangle _rectangle;
        BitmapInfoHeader _bitmapInfo;

        public MyBitmapRenderer(IntPtr dibHandle)
        {
            _dibHandle = dibHandle;
            _bitmapPointer = MyKernel32Native.GlobalLock(dibHandle);

            _bitmapInfo = new BitmapInfoHeader();
            Marshal.PtrToStructure(_bitmapPointer, _bitmapInfo);
            log.Debug(_bitmapInfo.ToString());

            _rectangle = new Rectangle();
            _rectangle.X = _rectangle.Y = 0;
            _rectangle.Width = _bitmapInfo.Width;
            _rectangle.Height = _bitmapInfo.Height;

            if (_bitmapInfo.SizeImage == 0)
            {
                _bitmapInfo.SizeImage = ((((_bitmapInfo.Width * _bitmapInfo.BitCount) + 31) & ~31) >> 3) * _bitmapInfo.Height;
            }


            // The following code only works on x86
            Debug.Assert(Marshal.SizeOf(typeof(IntPtr)) == 4);

            int pixelInfoPointer = _bitmapInfo.ClrUsed;
            if ((pixelInfoPointer == 0) && (_bitmapInfo.BitCount <= 8))
            {
                pixelInfoPointer = 1 << _bitmapInfo.BitCount;
            }

            pixelInfoPointer = (pixelInfoPointer * 4) + _bitmapInfo.Size + _bitmapPointer.ToInt32();

            _pixelInfoPointer = new IntPtr(pixelInfoPointer);
        }

        ~MyBitmapRenderer()
        {
            Dispose(false);
        }

        public Bitmap RenderToBitmap()
        {
            if (_bitmapInfo.BitCount != 32)
            {
                int sizeBitmapFileHeader = Marshal.SizeOf(typeof(BitmapFileHeader));

                BitmapFileHeader bitmapFile = new BitmapFileHeader();
                bitmapFile.Type = 'M' * 256 + 'B';
                bitmapFile.Size = (_pixelInfoPointer.ToInt32() - _bitmapPointer.ToInt32()) + sizeBitmapFileHeader + _bitmapInfo.SizeImage;
                bitmapFile.Reserved1 = 0;
                bitmapFile.Reserved2 = 0;
                bitmapFile.OffBits = (_pixelInfoPointer.ToInt32() - _bitmapPointer.ToInt32()) + sizeBitmapFileHeader;

                IntPtr _bitmapFilePointer = Marshal.AllocHGlobal(sizeBitmapFileHeader);
                Marshal.StructureToPtr(bitmapFile, _bitmapFilePointer, true);

                byte[] buffer = new byte[bitmapFile.Size];
                Marshal.Copy(_bitmapFilePointer, buffer, 0, sizeBitmapFileHeader);
                Marshal.Copy(_bitmapPointer, buffer, sizeBitmapFileHeader, bitmapFile.Size - sizeBitmapFileHeader);

                Bitmap bitmap = null;
                using (MemoryStream ms = new MemoryStream(buffer))
                using (Bitmap bmp = new Bitmap(ms))
                {
                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                    try
                    {
                        bitmap = new Bitmap(bmp.Width, bmp.Height, bmp.PixelFormat);
                        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                        try
                        {
                            int bmpBufferSize = bmpData.Stride * bmp.Height;
                            byte[] bmpBuffer = new byte[bmpBufferSize];
                            Marshal.Copy(bmpData.Scan0, bmpBuffer, 0, bmpBufferSize);
                            Marshal.Copy(bmpBuffer, 0, bitmapData.Scan0, bmpBufferSize);
                        }
                        finally
                        {
                            bitmap.UnlockBits(bitmapData);
                        }
                    }
                    finally
                    {
                        bmp.UnlockBits(bmpData);
                    }

                    if (_bitmapInfo.BitCount < 24)
                    {
                        bitmap.Palette = bmp.Palette;
                    }
                }

                bitmap.SetResolution(PpmToDpi(_bitmapInfo.XPelsPerMeter), PpmToDpi(_bitmapInfo.YPelsPerMeter));

                return bitmap;
            }
            else
            {
                Bitmap bitmap = new Bitmap(_rectangle.Width, _rectangle.Height);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    IntPtr hdc = graphics.GetHdc();

                    try
                    {
                        Gdi32Native.SetDIBitsToDevice(hdc, 0, 0, _rectangle.Width, _rectangle.Height,
                            0, 0, 0, _rectangle.Height, _pixelInfoPointer, _bitmapPointer, 0);
                    }
                    finally
                    {
                        graphics.ReleaseHdc(hdc);
                    }
                }

                bitmap.SetResolution(PpmToDpi(_bitmapInfo.XPelsPerMeter), PpmToDpi(_bitmapInfo.YPelsPerMeter));

                return bitmap;
            }
        }

        private static float PpmToDpi(double pixelsPerMeter)
        {
            double pixelsPerMillimeter = (double)pixelsPerMeter / 1000.0;
            double dotsPerInch = pixelsPerMillimeter * 25.4;
            return (float)Math.Round(dotsPerInch, 2);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            MyKernel32Native.GlobalUnlock(_dibHandle);
            MyKernel32Native.GlobalFree(_dibHandle);
        }
    }
}
