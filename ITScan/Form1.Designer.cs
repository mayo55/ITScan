namespace ITScan
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.nudCounter = new System.Windows.Forms.NumericUpDown();
            this.txtSaveFolder = new System.Windows.Forms.TextBox();
            this.lblSaveFolder = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnSelectSource = new System.Windows.Forms.Button();
            this.lblCounter = new System.Windows.Forms.Label();
            this.cmbFormat = new System.Windows.Forms.ComboBox();
            this.lblFormat = new System.Windows.Forms.Label();
            this.txtOutputFilename = new System.Windows.Forms.TextBox();
            this.lblOutputFilename = new System.Windows.Forms.Label();
            this.btnReference = new System.Windows.Forms.Button();
            this.lblDigits = new System.Windows.Forms.Label();
            this.nudDigits = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudCounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDigits)).BeginInit();
            this.SuspendLayout();
            // 
            // nudCounter
            // 
            this.nudCounter.Location = new System.Drawing.Point(94, 38);
            this.nudCounter.Maximum = new decimal(new int[] {
            -1,
            -1,
            -1,
            0});
            this.nudCounter.Name = "nudCounter";
            this.nudCounter.Size = new System.Drawing.Size(120, 19);
            this.nudCounter.TabIndex = 4;
            this.nudCounter.ValueChanged += new System.EventHandler(this.nudCounter_ValueChanged);
            // 
            // txtSaveFolder
            // 
            this.txtSaveFolder.Location = new System.Drawing.Point(94, 13);
            this.txtSaveFolder.Name = "txtSaveFolder";
            this.txtSaveFolder.Size = new System.Drawing.Size(147, 19);
            this.txtSaveFolder.TabIndex = 1;
            this.txtSaveFolder.TextChanged += new System.EventHandler(this.txtSaveFolder_TextChanged);
            // 
            // lblSaveFolder
            // 
            this.lblSaveFolder.AutoSize = true;
            this.lblSaveFolder.Location = new System.Drawing.Point(12, 16);
            this.lblSaveFolder.Name = "lblSaveFolder";
            this.lblSaveFolder.Size = new System.Drawing.Size(76, 12);
            this.lblSaveFolder.TabIndex = 0;
            this.lblSaveFolder.Text = "保存先フォルダ";
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(11, 119);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(91, 23);
            this.btnScan.TabIndex = 11;
            this.btnScan.Text = "スキャン(&T)...";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnSelectSource
            // 
            this.btnSelectSource.Location = new System.Drawing.Point(108, 119);
            this.btnSelectSource.Name = "btnSelectSource";
            this.btnSelectSource.Size = new System.Drawing.Size(91, 23);
            this.btnSelectSource.TabIndex = 12;
            this.btnSelectSource.Text = "ソース選択(&S)...";
            this.btnSelectSource.UseVisualStyleBackColor = true;
            this.btnSelectSource.Click += new System.EventHandler(this.btnSelectSource_Click);
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Location = new System.Drawing.Point(12, 40);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(40, 12);
            this.lblCounter.TabIndex = 3;
            this.lblCounter.Text = "カウンタ";
            // 
            // cmbFormat
            // 
            this.cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.Items.AddRange(new object[] {
            "BMP",
            "PNG"});
            this.cmbFormat.Location = new System.Drawing.Point(94, 64);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.Size = new System.Drawing.Size(74, 20);
            this.cmbFormat.TabIndex = 8;
            this.cmbFormat.SelectedIndexChanged += new System.EventHandler(this.cmbFormat_SelectedIndexChanged);
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(12, 67);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(55, 12);
            this.lblFormat.TabIndex = 7;
            this.lblFormat.Text = "フォーマット";
            // 
            // txtOutputFilename
            // 
            this.txtOutputFilename.Location = new System.Drawing.Point(94, 91);
            this.txtOutputFilename.Name = "txtOutputFilename";
            this.txtOutputFilename.ReadOnly = true;
            this.txtOutputFilename.Size = new System.Drawing.Size(218, 19);
            this.txtOutputFilename.TabIndex = 10;
            // 
            // lblOutputFilename
            // 
            this.lblOutputFilename.AutoSize = true;
            this.lblOutputFilename.Location = new System.Drawing.Point(12, 94);
            this.lblOutputFilename.Name = "lblOutputFilename";
            this.lblOutputFilename.Size = new System.Drawing.Size(75, 12);
            this.lblOutputFilename.TabIndex = 9;
            this.lblOutputFilename.Text = "出力ファイル名";
            // 
            // btnReference
            // 
            this.btnReference.Location = new System.Drawing.Point(247, 11);
            this.btnReference.Name = "btnReference";
            this.btnReference.Size = new System.Drawing.Size(65, 23);
            this.btnReference.TabIndex = 2;
            this.btnReference.Text = "参照(&R)...";
            this.btnReference.UseVisualStyleBackColor = true;
            this.btnReference.Click += new System.EventHandler(this.btnReference_Click);
            // 
            // lblDigits
            // 
            this.lblDigits.AutoSize = true;
            this.lblDigits.Location = new System.Drawing.Point(237, 40);
            this.lblDigits.Name = "lblDigits";
            this.lblDigits.Size = new System.Drawing.Size(29, 12);
            this.lblDigits.TabIndex = 5;
            this.lblDigits.Text = "桁数";
            // 
            // nudDigits
            // 
            this.nudDigits.Location = new System.Drawing.Point(272, 38);
            this.nudDigits.Maximum = new decimal(new int[] {
            29,
            0,
            0,
            0});
            this.nudDigits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDigits.Name = "nudDigits";
            this.nudDigits.Size = new System.Drawing.Size(40, 19);
            this.nudDigits.TabIndex = 6;
            this.nudDigits.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDigits.ValueChanged += new System.EventHandler(this.nudDigits_ValueChanged);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnScan;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 151);
            this.Controls.Add(this.nudDigits);
            this.Controls.Add(this.lblDigits);
            this.Controls.Add(this.btnReference);
            this.Controls.Add(this.lblOutputFilename);
            this.Controls.Add(this.txtOutputFilename);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.cmbFormat);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.btnSelectSource);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.lblSaveFolder);
            this.Controls.Add(this.txtSaveFolder);
            this.Controls.Add(this.nudCounter);
            this.MaximumSize = new System.Drawing.Size(342, 190);
            this.MinimumSize = new System.Drawing.Size(342, 190);
            this.Name = "Form1";
            this.Text = "ITScan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudCounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDigits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudCounter;
        private System.Windows.Forms.TextBox txtSaveFolder;
        private System.Windows.Forms.Label lblSaveFolder;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnSelectSource;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.ComboBox cmbFormat;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.TextBox txtOutputFilename;
        private System.Windows.Forms.Label lblOutputFilename;
        private System.Windows.Forms.Button btnReference;
        private System.Windows.Forms.Label lblDigits;
        private System.Windows.Forms.NumericUpDown nudDigits;
    }
}

