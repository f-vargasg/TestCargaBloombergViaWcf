using CargaBloomberg.BE;
using CargaBloomberg.BL;
using CommonUtils.Log4NetTb;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace CargaBloomberg.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class CargarPreciosBloomSrv : ICargarPreciosBloomSrv
    {

        public RemoteFileInfo DownloadFile(BE.DownloadRequest request)
        {
            RemoteFileInfo result = new RemoteFileInfo();
            try
            {
                string filePath = Path.Combine(ConfigurationManager.AppSettings["uploadPath"], request.Filename);
                FileInfo fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException("File not Found", request.Filename);
                }

                // open stream
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // return result
                result.Filename = request.Filename;
                result.Length = fileInfo.Length;
                result.FileByteStream = stream;
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public void UploadFile(RemoteFileInfo request)
        {
            Stream sourceStream = request.FileByteStream;
            string upLoadFolder = ConfigurationManager.AppSettings["uploadPath"];
            string filePath = Path.Combine(upLoadFolder, request.Filename);
            using (var fname = File.Create(filePath))
            {
                sourceStream.CopyTo(fname);
            }

        }

        public void CargarPreciosTmp(RemoteFileInfo request)
        {
            string pathFname;
            bool uploadOk = false;
            try
            {
                Log4NetManager.WriteDebugLog("Inicio de Carga");
                UploadFile(request);
                uploadOk = true;
                pathFname = Path.Combine(ConfigurationManager.AppSettings["uploadPath"], request.Filename);
                CargarArchivoExcel(pathFname);
                Log4NetManager.WriteDebugLog("Final de la carga");
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void CargarArchivoExcel(string pathFname)
        {
            try
            {
                PrecioMercTmpBL precioMercTmpBL = new PrecioMercTmpBL();
                Log4NetManager.WriteDebugLog("Inicio de Procesar archivo Excel");
                PreciosACargarBE preciosACargarBE = precioMercTmpBL.ExcelFileToList(pathFname);
                Log4NetManager.WriteDebugLog("Fin de Procesar archivo Excel");
                foreach (var item in preciosACargarBE.LstPrecios)
                {
                    Log4NetManager.WriteDebugLog("Valida si Registro Existe!!!");
                    if (!precioMercTmpBL.ExisteRegistro(item.CodIsinV, preciosACargarBE.FechaCarga))
                    {
                        precioMercTmpBL.Insertar(item);
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
