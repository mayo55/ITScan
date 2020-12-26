using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TwainDotNet;
using TwainDotNet.Win32;
using TwainDotNet.WinFroms;

namespace ITScan
{
    public partial class Form1 : Form
    {
        private MyTwain twain = null;
        private ScanSettings settings = null;

        IntPtr _dibHandle;
        IntPtr _bitmapPointer;
        IntPtr _pixelInfoPointer;
        Rectangle _rectangle;
        BitmapInfoHeader _bitmapInfo;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            twain = new MyTwain(new WinFormsWindowMessageHook(this));
            twain.MyTransferImage += twain_MyTransferImage;
            twain.ScanningComplete += twain_ScanningComplete;

            settings = new ScanSettings();
            settings.ShowTwainUI = true;

            txtSaveFolder.Text = Properties.Settings.Default.SaveFolder;
            nudCounter.Value = Properties.Settings.Default.Counter;
            nudDigits.Value = Properties.Settings.Default.Digits;
            cmbFormat.SelectedIndex = cmbFormat.Items.IndexOf(Properties.Settings.Default.Format);

            Location = Properties.Settings.Default.Form1Location;

            SetOutputFilename();
            ActiveControl = btnScan;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Form1Location = Location;
            Properties.Settings.Default.Save();
        }

        private void btnReference_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "保存先フォルダを選択して下さい。";
            fbd.SelectedPath = txtSaveFolder.Text;
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                txtSaveFolder.Text = fbd.SelectedPath;
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtSaveFolder.Text))
            {
                DialogResult result = MessageBox.Show(
                    txtSaveFolder.Text + "が存在しません。作成しますか？",
                    "フォルダ作成確認",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No || result == DialogResult.Cancel)
                {
                    return;
                }

                try
                {
                    Directory.CreateDirectory(txtSaveFolder.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        txtSaveFolder.Text + "を作成できませんでした。",
                        "フォルダ作成失敗",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1);
                    return;
                }
            }

            Enabled = false;

            try
            {
                twain.StartScanning(settings);
            }
            catch (Exception ex)
            {
                Enabled = true;
            }
        }

        private void btnSelectSource_Click(object sender, EventArgs e)
        {
            twain.SelectSource();
        }

        private void txtSaveFolder_TextChanged(object sender, EventArgs e)
        {
            SetOutputFilename();
            Properties.Settings.Default.SaveFolder = txtSaveFolder.Text;
        }
        private void nudCounter_ValueChanged(object sender, EventArgs e)
        {
            SetOutputFilename();
            Properties.Settings.Default.Counter = nudCounter.Value;
        }

        private void nudDigits_ValueChanged(object sender, EventArgs e)
        {
            SetOutputFilename();
            Properties.Settings.Default.Digits = nudDigits.Value;
        }

        private void cmbFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetOutputFilename();
            Properties.Settings.Default.Format = cmbFormat.SelectedItem.ToString();
        }

        private void SetOutputFilename()
        {
            int digits = (int)nudDigits.Value;
            string strCounter = "00000000000000000000000000000" + nudCounter.Value.ToString();
            string strCounterPadding = strCounter.Substring(strCounter.Length - digits, digits);
            txtOutputFilename.Text = Path.Combine(txtSaveFolder.Text, strCounterPadding + "." + cmbFormat.Text.ToLower());
        }

        private void twain_MyTransferImage(object sender, MyTransferImageEventArgs e)
        {
            _dibHandle = e.HBitmap;
            _bitmapPointer = MyKernel32Native.GlobalLock(_dibHandle);
            try
            {
                _bitmapInfo = new BitmapInfoHeader();
                Marshal.PtrToStructure(_bitmapPointer, _bitmapInfo);
                //log.Debug(_bitmapInfo.ToString());

                _rectangle = new Rectangle();
                _rectangle.X = _rectangle.Y = 0;
                _rectangle.Width = _bitmapInfo.Width;
                _rectangle.Height = _bitmapInfo.Height;

                if (_bitmapInfo.SizeImage == 0)
                {
                    _bitmapInfo.SizeImage = ((((_bitmapInfo.Width * _bitmapInfo.BitCount) + 31) & ~31) >> 3) * _bitmapInfo.Height;
                }

                // The following code only works on x86
                System.Diagnostics.Debug.Assert(Marshal.SizeOf(typeof(IntPtr)) == 4);

                int pixelInfoPointer = _bitmapInfo.ClrUsed;
                if ((pixelInfoPointer == 0) && (_bitmapInfo.BitCount <= 8))
                {
                    pixelInfoPointer = 1 << _bitmapInfo.BitCount;
                }

                pixelInfoPointer = (pixelInfoPointer * 4) + _bitmapInfo.Size + _bitmapPointer.ToInt32();

                _pixelInfoPointer = new IntPtr(pixelInfoPointer);
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

                using (MemoryStream ms = new MemoryStream(buffer))
                using (Bitmap bitmap = new Bitmap(ms))
                {
                    switch (Config.Rotate)
                    {
                        case 90:
                            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;

                        case 180:
                            bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;

                        case 270:
                            bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;

                    }

                    string strSelectedItem = cmbFormat.SelectedItem.ToString();
                    string outputFilename = txtOutputFilename.Text;
                    nudCounter.Value++;

                    if (strSelectedItem == "BMP")
                    {
                        bitmap.Save(outputFilename, ImageFormat.Bmp);
                    }
                    else if (strSelectedItem == "PNG")
                    {
                        bitmap.Save(outputFilename, ImageFormat.Png);
                    }

                    SetOutputFilename();
                    Properties.Settings.Default.Save();
                }
            }
            finally
            {
                MyKernel32Native.GlobalUnlock(_dibHandle);
                MyKernel32Native.GlobalFree(_dibHandle);
            }
        }

        private void twain_ScanningComplete(object sender, ScanningCompleteEventArgs e)
        {
            Enabled = true;
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            ConfigForm configForm = new ConfigForm();
            configForm.ShowDialog();
        }
    }
}
