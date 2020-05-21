using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using TwainDotNet;
using TwainDotNet.WinFroms;

namespace ITScan
{
    public partial class Form1 : Form
    {
        private Twain twain = null;
        private ScanSettings settings = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            twain = new Twain(new WinFormsWindowMessageHook(this));
            twain.TransferImage += twain_TransferImage;
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
            }
            finally
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

        private void twain_TransferImage(object sender, TransferImageEventArgs e)
        {
            Bitmap resultImage = e.Image;
            string strSelectedItem = cmbFormat.SelectedItem.ToString();

            if (strSelectedItem == "BMP")
            {
                resultImage.Save(txtOutputFilename.Text, ImageFormat.Bmp);
            }
            else if (strSelectedItem == "PNG")
            {
                resultImage.Save(txtOutputFilename.Text, ImageFormat.Png);
            }

            nudCounter.Value++;

            SetOutputFilename();
            Properties.Settings.Default.Save();
        }

        private void twain_ScanningComplete(object sender, ScanningCompleteEventArgs e)
        {
            Enabled = true;
        }
    }
}
