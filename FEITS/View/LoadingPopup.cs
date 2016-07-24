using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FEITS.Controller;
using FEITS.Model;

namespace FEITS.View
{
    public partial class LoadingPopup : Form
    {
        public LoadingPopup()
        {
            InitializeComponent();
        }

        public void BeginLoading()
        {
            assetLoader.RunWorkerAsync();
        }

        private void assetLoader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            PB_Progress.Value = e.ProgressPercentage;
        }

        private void assetLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                Dispose();
            }
        }

        private void assetLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            AssetGeneration.Initialize(worker, e);
        }
    }
}
