using System;
using System.Collections.Generic;
using TwainDotNet;

namespace ITScan
{
    public class MyTwain
    {
        MyDataSourceManager _dataSourceManager;

        public MyTwain(IWindowsMessageHook messageHook)
        {
            ScanningComplete += delegate { };
            // MyTransferImageに変更
            //TransferImage += delegate { };
            MyTransferImage += delegate { };

            _dataSourceManager = new MyDataSourceManager(DataSourceManager.DefaultApplicationId, messageHook);
            _dataSourceManager.ScanningComplete += delegate(object sender, ScanningCompleteEventArgs args)
            {
                ScanningComplete(this, args);
            };
            // MyTransferImageに変更
            //_dataSourceManager.TransferImage += delegate(object sender, TransferImageEventArgs args)
            _dataSourceManager.MyTransferImage += delegate (object sender, MyTransferImageEventArgs args)
            {
                //TransferImage(this, args);
                MyTransferImage(this, args);
            };
        }

        /// <summary>
        /// Notification that the scanning has completed.
        /// </summary>
        public event EventHandler<ScanningCompleteEventArgs> ScanningComplete;

        // MyTransferImageに変更
        //public event EventHandler<TransferImageEventArgs> TransferImage;
        public event EventHandler<MyTransferImageEventArgs> MyTransferImage;

        /// <summary>
        /// Starts scanning.
        /// </summary>
        public void StartScanning(ScanSettings settings)
        {
            _dataSourceManager.StartScan(settings);
        }

        /// <summary>
        /// Shows a dialog prompting the use to select the source to scan from.
        /// </summary>
        public void SelectSource()
        {
            _dataSourceManager.SelectSource();
        }

        /// <summary>
        /// Selects a source based on the product name string.
        /// </summary>
        /// <param name="sourceName">The source product name.</param>
        public void SelectSource(string sourceName)
        {
            var source = MyDataSource.GetSource(
                sourceName,
                _dataSourceManager.ApplicationId,
                _dataSourceManager.MessageHook);

            _dataSourceManager.SelectSource(source);
        }

        /// <summary>
        /// Gets the product name for the default source.
        /// </summary>
        public string DefaultSourceName
        {
            get
            {
                using (var source = MyDataSource.GetDefault(_dataSourceManager.ApplicationId, _dataSourceManager.MessageHook))
                {
                    return source.SourceId.ProductName;
                }
            }
        }

        /// <summary>
        /// Gets a list of source product names.
        /// </summary>
        public IList<string> SourceNames
        {
            get
            {
                var result = new List<string>();
                var sources = MyDataSource.GetAllSources(
                    _dataSourceManager.ApplicationId,
                    _dataSourceManager.MessageHook);

                foreach (var source in sources)
                {
                    result.Add(source.SourceId.ProductName);
                    source.Dispose();
                }

                return result;
            }
        }
    }
}
