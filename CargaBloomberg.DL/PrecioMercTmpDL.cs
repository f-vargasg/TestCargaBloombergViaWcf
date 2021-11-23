using CargaBloomberg.BE;
using CommonUtils.DateTb;
using CommonUtils.Log4NetTb;
using CommonUtils.OraDbTb;
using CommonUtils.StringTb;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CargaBloomberg.DL
{
    public class PrecioMercTmpDL
    {

        OracleConnection conn;

        public PrecioMercTmpDL()
        {
            this.conn = ConnGl.Instance.Conn;
        }

        public bool ExisteRegistro (string pCodIsinV, 
                                    DateTime pFecTransaccion)
        {
            bool res = false;
            int scrap = 0;
            try
            {
                string sql = "select count(1) as res " + Environment.NewLine +
                               " from dual  " + Environment.NewLine +
                              "  where exists (select a.* " + Environment.NewLine +
                                              " from vo_ambpremercadotmp  a " + Environment.NewLine +
                                               " where a.cod_isin_v = " + MyStringUtils.entreComas(pCodIsinV) + Environment.NewLine +
                                               " and a.FEC_TRANSACCION = " + MyDateUtils.toDate(pFecTransaccion) + ")";

                OracleDataReader dr = MyOracleUtils.executeSqlStm(sql, this.conn);
                while (dr.Read())
                {
                   scrap = Convert.ToInt32(dr["res"]);
                }
                res = (scrap == 1);
                return res;
            }
            catch (Exception)
            {
                throw;
            }


        }

        public void Insertar(PrecioMercTmpBE precioMercTmpBE)
        {
            Log4NetManager.WriteDebugLog("Inicio [PrecioMercTmpDL.Insertar]");
            string wsql = "insert into VO_AMBPREMERCADOTMP(" + Environment.NewLine + 
                          " COD_EMISOR_V ,TAS_RENDNOMI,USU_REGISTRO_V,COD_SERIE_V, " + Environment.NewLine +
                          " USU_MODIFICO_V,DES_CLAMOODY,COD_INSFINA_V,COD_ISIN_V ," + Environment.NewLine + 
                          " DES_CLAFITCH,MTO_PRECIOCIE,IND_CARGA_N,FEC_REGISTRO, " + Environment.NewLine + 
                          " FEC_MODIFICO,COD_EMISION_V,DES_CLASP,FEC_TRANSACCION ) " + Environment.NewLine + 
                          "values ( " + Environment.NewLine +
                            MyStringUtils.entreComas(precioMercTmpBE.CodEmisorV) + ", " + 
                            string.Format( "{0:0.000}", precioMercTmpBE.TasRendnomi) + ", " +
                             MyStringUtils.entreComas(precioMercTmpBE.UsuRegistroV) + ", " + 
                            MyStringUtils.entreComas( precioMercTmpBE.CodSerieV) + ", " + 
                            MyStringUtils.entreComas(precioMercTmpBE.UsuModificoV) + ", " + 
                            MyStringUtils.entreComas(precioMercTmpBE.DesClamoody) + ", " + 
                            MyStringUtils.entreComas(precioMercTmpBE.CodInsfinaV)+ ", " + 
                            MyStringUtils.entreComas(precioMercTmpBE.CodIsinV) + ", " + 
                            MyStringUtils.entreComas(precioMercTmpBE.DesClafitch)  + ", " + 
                            string.Format("{0:##########0.00}", precioMercTmpBE.MtoPreciocie) + ", " + 
                            Convert.ToString(precioMercTmpBE.IndCargaN) + ", " + 
                            MyStringUtils.fmtDateForDb(precioMercTmpBE.FecRegistro)  + ", " + 
                            MyStringUtils.fmtDateForDb(precioMercTmpBE.FecModifico)+ ", " + 
                            MyStringUtils.entreComas(precioMercTmpBE.CodEmisionV) + ", " + 
                            MyStringUtils.entreComas(precioMercTmpBE.DesClasp) + ", " +
                            MyStringUtils.fmtDateForDb(precioMercTmpBE.FecTransaccion) + ")" ;
            Log4NetManager.WriteDebugLog("Fin [PrecioMercTmpDL.Insertar]");
                            
        }
    }
}
