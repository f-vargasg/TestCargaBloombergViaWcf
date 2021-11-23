using CargaBloomberg.WinLoadPreciosBloomberg.WcfSrvCargaPrecBloomRef;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargaBloomberg.WinLoadPreciosBloomberg
{
    public partial class FrmPrincipal : Form
    {
        string initialPath;


        #region Constructores
        public FrmPrincipal()
        {
            InitializeComponent();
            InitMyComponents();
        }

        #endregion

        #region Miscelaneous
        private void InitMyComponents()
        {
            this.Text = ConfigurationManager.AppSettings["captionForm"];
            this.StartPosition = FormStartPosition.CenterScreen;
            txtPathName.Text = ConfigurationManager.AppSettings["fname"];
            txtDownloadFile.Text = ConfigurationManager.AppSettings["downloadPath"];
            initialPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private string GetFilePath(string currentFilePath)
        {
            string res = string.Empty;
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = initialPath;
                openFileDialog.Filter = "pdf files (*.pdf)|*.pdf|xlsx files (*.xlsx)|*.xlsx|docx files (*.docx)|*.docx|txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                res = currentFilePath;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    res = openFileDialog.FileName;
                }
            }
            return res;
        }

        #endregion

        #region Eventos
        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                CargarPreciosBloomSrvClient client = new CargarPreciosBloomSrvClient();
                string pathFname = txtPathName.Text;
                FileInfo fileInfo = new FileInfo(pathFname);
                RemoteFileInfo remoteFileInfo = new RemoteFileInfo();
                using (FileStream stream = new FileStream(pathFname, FileMode.Open, FileAccess.Read))
                {
                    remoteFileInfo.Filename = Path.GetFileName(pathFname);
                    remoteFileInfo.FileByteStream = stream;
                    remoteFileInfo.Length = fileInfo.Length;
                 //    client.UploadFile(remoteFileInfo.Filename, remoteFileInfo.Length, remoteFileInfo.FileByteStream);
                    client.CargarPreciosTmp(remoteFileInfo.Filename, remoteFileInfo.Length, remoteFileInfo.FileByteStream);
                }

                MessageBox.Show(string.Format("Archivo {0} Subido Correctamente ...", remoteFileInfo.Filename));

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                CargarPreciosBloomSrvClient cliente = new CargarPreciosBloomSrvClient();
                DownloadRequest requestData = new DownloadRequest();
                RemoteFileInfo fileInfo = new RemoteFileInfo();
                requestData.Filename = Path.GetFileName(txtPathName.Text);
                Stream stream;
                fileInfo.Length = cliente.DownloadFile(ref requestData.Filename, out stream);
                fileInfo.Filename = requestData.Filename;
                string downloadPath = ConfigurationManager.AppSettings["downloadPath"];
                var realFname = Path.Combine(downloadPath, fileInfo.Filename);
                using (FileStream outputFs = new FileStream(realFname, FileMode.Create))
                {
                    stream.CopyTo(outputFs);
                }
                MessageBox.Show(string.Format("Archivo {0} bajado correctamente", fileInfo.Filename));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void butBrowse_Click(object sender, EventArgs e)
        {
            string currentFilePath = txtPathName.Text;
            txtPathName.Text = GetFilePath(currentFilePath);
        }

        #endregion


    }
}
