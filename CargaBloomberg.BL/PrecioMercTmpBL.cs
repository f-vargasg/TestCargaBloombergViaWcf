using CargaBloomberg.BE;
using CargaBloomberg.DL;
using CommonUtils.Log4NetTb;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargaBloomberg.BL
{
    public class PrecioMercTmpBL
    {
        PrecioMercTmpDL precioMercTmpDL;

        public PrecioMercTmpBL()
        {
            this.precioMercTmpDL = new PrecioMercTmpDL();
        }

        public bool ExisteRegistro(string pCodIsinV,
                                    DateTime pFecTransaccion)
        {
            bool res = false;
            try
            {
                res = this.precioMercTmpDL.ExisteRegistro(pCodIsinV, pFecTransaccion);
            }
            catch (Exception)
            {
                
                throw;
            }
            return res;
        }

        public void Insertar(PrecioMercTmpBE precioMercTmpBE)
        {
            try
            {
                this.precioMercTmpDL.Insertar(precioMercTmpBE);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public PreciosACargarBE ExcelFileToList(string pathFname)
        {
            try
            {
                Log4NetManager.WriteDebugLog("Inicio [PrecioMercTmpBL.ExcelFileToList]");
                List<PreciosACargarBE> lstRes = new List<PreciosACargarBE>();
                XSSFWorkbook hssfwb;
                PreciosACargarBE preciosACargar = new PreciosACargarBE();

                using (FileStream file = new FileStream(pathFname, FileMode.Open, FileAccess.Read))
                {
                    hssfwb = new XSSFWorkbook(file);
                }

                ISheet sheet = hssfwb.GetSheet("ArchivoFinal");
                preciosACargar.FechaCarga = sheet.GetRow(0).GetCell(0).DateCellValue;
                for (int row = 2; row <= sheet.LastRowNum; row++)
                {
                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                    {
                        PrecioMercTmpBE precioMercBE = new PrecioMercTmpBE();
                        precioMercBE.FecTransaccion = preciosACargar.FechaCarga;
                        precioMercBE.CodIsinV = sheet.GetRow(row).GetCell(0).StringCellValue;
                        precioMercBE.MtoPreciocie = sheet.GetRow(row).GetCell(1).NumericCellValue;
                        precioMercBE.DesClafitch = sheet.GetRow(row).GetCell(2).StringCellValue;
                        precioMercBE.DesClamoody = sheet.GetRow(row).GetCell(3).StringCellValue;
                        precioMercBE.DesClasp = sheet.GetRow(row).GetCell(4).StringCellValue;
                        precioMercBE.TasRendnomi = sheet.GetRow(row).GetCell(5).NumericCellValue;
                        precioMercBE.DesCatBloomberg = sheet.GetRow(row).GetCell(6).StringCellValue;
                        precioMercBE.FecRegistro = DateTime.Now;
                        preciosACargar.LstPrecios.Add(precioMercBE);
                        
                        // MessageBox.Show(string.Format("Row {0} = {1}", row, sheet.GetRow(row).GetCell(0).StringCellValue));
                    }
                }
                Log4NetManager.WriteDebugLog("Fin [PrecioMercTmpBL.ExcelFileToList]");
                return preciosACargar;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
