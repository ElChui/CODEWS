using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;

using System.Threading;
using System.Globalization;
using System.IO;
using System.Web.Services.Description;
using System.Security.Principal;
using System.Runtime.Serialization;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using Ws_POS_web;


namespace WS_POS_web
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WS_POS_web : System.Web.Services.WebService
    {
        Boolean produccion = true;

        clCapaDatos confact = new clCapaDatos();
        String Conexion = ConfigurationManager.AppSettings["conexion"];
        String conexionPOS = ConfigurationManager.AppSettings["conexionPOS"];
        String conexionPOSWeb = ConfigurationManager.AppSettings["conexionPOSWeb"];
        String conexionPINPAD = ConfigurationManager.AppSettings["conexionPINPAD"];
        String conexionBIN = ConfigurationManager.AppSettings["conexionBIN"];
        String UsuarioSap = ConfigurationManager.AppSettings["UsuarioSap"];//"AJE_WS";
        String ContraSap = ConfigurationManager.AppSettings["ContreSap"];//"W19+25s.";
        String IVA = ConfigurationManager.AppSettings["IVA"];

        bool esCorrecto = false;
        int contE = 0;
        int contRepetir = 10;
        string solicitudCancelada = "Anulada la solicitud: La solicitud fue cancelada";

        static bool esProduccion = ConfigurationManager.AppSettings["esProduccion"] == "S" ? true : false;

        #region << LOGIN >>

        [WebMethod]
        public DataSet Login(String Usuario, String Pass)
        {
            DataSet ds = null;
            DataSet ds1 = new DataSet("usuarios");
            DataTable dt = new DataTable("usuario");

            dt.Columns.Add("info");
            dt.Columns.Add("idUsuario");
            dt.Columns.Add("usuario");
            dt.Columns.Add("identificacion");
            dt.Columns.Add("nombre");
            dt.Columns.Add("alias");
            dt.Columns.Add("codigoVendedor");

            String veridicador = "";
            if (Pass != null)
            {
                if (Usuario.Trim() != "" && Pass.Trim() != "")
                {
                    veridicador = "1";
                    List<SqlParameter> parameters = new List<SqlParameter>()
                            {
                                 new SqlParameter() {ParameterName = "@pUsua",Value= Usuario.Trim()},
                                 new SqlParameter() {ParameterName = "@pPass", Value = Pass.Trim()}
                             };
                    try
                    {
                        ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Login", parameters);

                        if (ds.Tables[0].Columns.Count > 2)
                        {

                            if (produccion) // SI ES PRODUCCION
                            {
                                Ws_obt_vend_PRD.ZSDWS_POS_CONSULTA_VENDEDORES vend = new Ws_obt_vend_PRD.ZSDWS_POS_CONSULTA_VENDEDORES();
                                vend.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                                //Consulta si existe código de vendedor con este número de cédula
                                String[] vendget = vend.ZsdrfcPosConsultaVendedores(Usuario).Split('|');

                                if (vendget[0] == "0")
                                {
                                    foreach (DataRow f in ds.Tables[0].Rows)
                                    {
                                        DataRow fila = dt.NewRow();
                                        fila["info"] = f["info"].ToString();
                                        fila["idUsuario"] = f["idUsuario"].ToString();
                                        fila["usuario"] = f["usuario"].ToString();
                                        fila["identificacion"] = f["identificacion"].ToString();
                                        fila["nombre"] = f["nombre"].ToString();
                                        fila["alias"] = f["alias"].ToString();
                                        fila["codigoVendedor"] = vendget[1];
                                        dt.Rows.Add(fila);
                                    }
                                    ds1.Tables.Add(dt);
                                    veridicador = "0";

                                }
                                else
                                {
                                    foreach (DataRow f in ds.Tables[0].Rows)
                                    {
                                        DataRow fila = dt.NewRow();
                                        fila["info"] = f["info"].ToString();
                                        fila["idUsuario"] = f["idUsuario"].ToString();
                                        fila["usuario"] = f["usuario"].ToString();
                                        fila["identificacion"] = f["identificacion"].ToString();
                                        fila["nombre"] = f["nombre"].ToString();
                                        fila["alias"] = f["alias"].ToString();
                                        fila["codigoVendedor"] = "SINCODIGO";
                                        dt.Rows.Add(fila);
                                    }
                                    ds1.Tables.Add(dt);
                                    veridicador = "0";

                                }
                            }
                            else // SI ES CALIDAD
                            {
                                Ws_obt_vend.ZSDWS_POS_CONSULTA_VENDEDORES vend = new Ws_obt_vend.ZSDWS_POS_CONSULTA_VENDEDORES();
                                vend.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                                //Consulta si existe código de vendedor con este número de cédula
                                String[] vendget = vend.ZsdrfcPosConsultaVendedores(Usuario).Split('|');

                                if (vendget[0] == "0")
                                {
                                    foreach (DataRow f in ds.Tables[0].Rows)
                                    {
                                        DataRow fila = dt.NewRow();
                                        fila["info"] = f["info"].ToString();
                                        fila["idUsuario"] = f["idUsuario"].ToString();
                                        fila["usuario"] = f["usuario"].ToString();
                                        fila["identificacion"] = f["identificacion"].ToString();
                                        fila["nombre"] = f["nombre"].ToString();
                                        fila["alias"] = f["alias"].ToString();
                                        fila["codigoVendedor"] = vendget[1];
                                        dt.Rows.Add(fila);
                                    }
                                    ds1.Tables.Add(dt);
                                    veridicador = "0";

                                }
                                else
                                {
                                    foreach (DataRow f in ds.Tables[0].Rows)
                                    {
                                        DataRow fila = dt.NewRow();
                                        fila["info"] = f["info"].ToString();
                                        fila["idUsuario"] = f["idUsuario"].ToString();
                                        fila["usuario"] = f["usuario"].ToString();
                                        fila["identificacion"] = f["identificacion"].ToString();
                                        fila["nombre"] = f["nombre"].ToString();
                                        fila["alias"] = f["alias"].ToString();
                                        fila["codigoVendedor"] = "SINCODIGO";
                                        dt.Rows.Add(fila);
                                    }
                                    ds1.Tables.Add(dt);
                                    veridicador = "0";

                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        veridicador = e.ToString();
                    }
                }
                else
                {
                    veridicador = "2";
                }
            }


            return ds1;
        }
        #endregion

        #region << CONDICIONES DE PAGO>>
        [WebMethod]
        public DataTable getCondicionesPago(String ofiVent)
        {
            DataSet ds = null;
            DataTable dt = null;
            if (ofiVent != "")
            {
                List<SqlParameter> fact = new List<SqlParameter>()
                            {
                                 new SqlParameter() {ParameterName = "@pOfiVenta",Value= ofiVent.Trim()},                                 
                            };
                try
                {
                    ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Get_PaymentConditions", fact);
                    ds.Tables[0].TableName = "Formas";
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
                catch (Exception e)
                {
                    dt = null;
                }

            }

            return dt;
        }
        #endregion

        #region << MODULES N TRANSACTIONS >>
        [WebMethod]
        public DataSet getTransactions(String idUsuario, String idModule)
        {
            DataSet ds = null;
            DataTable dt = new DataTable("transaction");
            String option = "getModules";
            String veridicador = "";
            if (idModule.Trim() != "")
            {
                option = "getTransaction";
            }

            if (idUsuario.Trim() != "")
            {
                veridicador = "1";
                List<SqlParameter> parameters = new List<SqlParameter>()
                            {
                                 new SqlParameter() {ParameterName = "@pIdUser",Value= idUsuario.Trim()},
                                 new SqlParameter() {ParameterName = "@pOption",Value= option.Trim()},
                                 new SqlParameter() {ParameterName = "@pIdModule", Value = idModule.Trim()}
                             };
                try
                {
                    ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Get_Transactions", parameters);
                }
                catch (Exception e)
                {
                    veridicador = e.ToString();
                }
            }
            else
            {
                veridicador = "2";
            }
            return ds;
        }
        #endregion

        #region << OBTENER CONSTANTES POS >>
        [WebMethod]
        public DataSet getConstants()
        {
            DataSet ds = null;
            DataTable dt = new DataTable("constantes");
            String veridicador = "";

            veridicador = "1";
            List<SqlParameter> parameters = new List<SqlParameter>()
                            {

                            };
            try
            {
                ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Get_Constants", parameters);
            }
            catch (Exception e)
            {
                veridicador = e.ToString();
            }
            return ds;
        }
        #endregion

        #region << OBTENER CAJAS >>
        [WebMethod]
        public DataSet getCashRegisters(String idUsuario)
        {
            DataSet ds = null;
            DataTable dt = new DataTable("cashRegisters");
            String veridicador = "";

            veridicador = "1";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@pIdUser",Value= idUsuario.Trim()},
            };
            try
            {
                ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Get_CashRegisters", parameters);
            }
            catch (Exception e)
            {
                veridicador = e.ToString();
            }
            return ds;
        }
        #endregion

        #region << TABLAS DE DESCUENTO >>

        [WebMethod]
        public String actValoresFactura(String cliente, String sector, String ofVent, String canal, String condicionPago, String sociedad, String items, String docuVentas)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ",";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";

            JArray itemsArray = (JArray)JsonConvert.DeserializeObject(items);
            String[] pData = new String[itemsArray.Count];
            String respuesta = "4";
            String cadenaRespuesta = "";

            String promoEmpleado = ""; //Mercointelg no necesita de promoción de empleado

            for (int i = 0; i < itemsArray.Count; i++)
            {
                pData[i] = itemsArray[i].ToString();

            }

            /*AUART CLASE DE DOCUMENTO DE VENTAS
             * VKORG   ORGANIZACION DE VENTAS
             * VTWEG  CANAL DE DISTRIBUCION
             * SPART SECTOR
             * VKBUR   OFINICA DE VENTAS
             * DZTERM  CLAVE DE CONDICIONES DE PAGO
             * BUKRS  SOCIEDAD
             * KUNNR  NUMERO DE CLIENTE SAP
             * ZIT_ITEMIN  MATERIAL PARA DETERMINAR PRECIO DESCUENTO
             * */

            if (produccion) // SI ES PRODUCCION
            {
                Ws_Financiamiento_PRD.ZWS_PRECIO_DESC_FINANCIAMIENTO finan = new Ws_Financiamiento_PRD.ZWS_PRECIO_DESC_FINANCIAMIENTO();
                finan.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Financiamiento_PRD.ZestItemin[] itemsIn = new Ws_Financiamiento_PRD.ZestItemin[itemsArray.Count];
                Ws_Financiamiento_PRD.ZestOutItemin[] tablaRespuesta;

                for (int i = 0; i < itemsIn.Length; i++)
                {
                    Ws_Financiamiento_PRD.ZestItemin item = new Ws_Financiamiento_PRD.ZestItemin();
                    String[] infoMaterial = pData[i].Split(';');

                    item.Almacen = infoMaterial[0];
                    item.Material = infoMaterial[1];
                    item.Cantidad = infoMaterial[2];
                    item.Centro = infoMaterial[3];
                    if (infoMaterial.Length > 4)
                    {
                        Decimal descuento = Decimal.Parse(infoMaterial[4]);
                        item.Desc_25Manual = descuento;
                    }

                    itemsIn[i] = item;
                }

                tablaRespuesta = finan.ZsdrfcPrecioDescFinancia(docuVentas, sociedad, promoEmpleado, itemsIn, cliente, sector, ofVent, sociedad, canal, condicionPago, out respuesta);

                cadenaRespuesta = respuesta;

                if (tablaRespuesta != null)
                {
                    for (int i = 0; i < tablaRespuesta.Length; i++)
                    {
                        String respTabla = tablaRespuesta[i].Posicion + ";" + tablaRespuesta[i].Material + ";" +
                                        tablaRespuesta[i].Cantidad + ";" + tablaRespuesta[i].PrecioBruto + ";" +
                                        tablaRespuesta[i].Descuento + ";" + tablaRespuesta[i].PorcDesc + ";" +
                                        tablaRespuesta[i].PrecioNeto + ";" + tablaRespuesta[i].Impuesto + ";" +
                                        tablaRespuesta[i].DescManual + ";" + tablaRespuesta[i].PrecioNetoFinal;


                        cadenaRespuesta = cadenaRespuesta + "|" + respTabla;
                    }
                }
            }
            else // SI ES CALIDAD
            {
                Ws_Financiamiento.ZWS_PRECIO_DESC_FINANCIAMIENTO finan = new Ws_Financiamiento.ZWS_PRECIO_DESC_FINANCIAMIENTO();
                finan.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Financiamiento.ZestItemin[] itemsIn = new Ws_Financiamiento.ZestItemin[itemsArray.Count];
                Ws_Financiamiento.ZestOutItemin[] tablaRespuesta;

                for (int i = 0; i < itemsIn.Length; i++)
                {
                    Ws_Financiamiento.ZestItemin item = new Ws_Financiamiento.ZestItemin();

                    String[] infoMaterial = pData[i].Split(';');


                    item.Almacen = infoMaterial[0];
                    item.Material = infoMaterial[1];
                    item.Cantidad = infoMaterial[2];
                    item.Centro = infoMaterial[3];
                    Decimal descuento = Decimal.Parse(infoMaterial[4]);
                    item.Desc_25Manual = descuento;

                    itemsIn[i] = item;
                }

                tablaRespuesta = finan.ZsdrfcPrecioDescFinancia(docuVentas, sociedad, promoEmpleado, itemsIn, cliente, sector, ofVent, sociedad, canal, condicionPago, out respuesta);

                cadenaRespuesta = respuesta;

                if (tablaRespuesta != null)
                {
                    for (int i = 0; i < tablaRespuesta.Length; i++)
                    {
                        String respTabla = tablaRespuesta[i].Posicion + ";" + tablaRespuesta[i].Material + ";" +
                                        tablaRespuesta[i].Cantidad + ";" + tablaRespuesta[i].PrecioBruto + ";" +
                                        tablaRespuesta[i].Descuento + ";" + tablaRespuesta[i].PorcDesc + ";" +
                                        tablaRespuesta[i].PrecioNeto + ";" + tablaRespuesta[i].Impuesto + ";" +
                                        tablaRespuesta[i].DescManual + ";" + tablaRespuesta[i].PrecioNetoFinal;


                        cadenaRespuesta = cadenaRespuesta + "|" + respTabla;
                    }
                }
            }

            return cadenaRespuesta;
        }

        [WebMethod]
        public string [] actualizarPreciosDescuentos(int opcion, string area, string esquema, string centro, string grupoArticulo, string codigoArticulo, string codigoSector, string tipoNegociacion, string ramoCliente, string idCliente, string jerarquia, bool tieneYmhPlus, string pagoConTarjeta)
        {
            DataSet ds = null;
            DataTable dt = new DataTable();
            string[] descuento = new String[4];

            string yamahaPlus = tieneYmhPlus ? "S" : "N";

            List<SqlParameter> pParams = new List<SqlParameter>()
                            {
            new SqlParameter() {ParameterName = "@Opcion", Value = opcion},
            new SqlParameter() {ParameterName = "@Area",Value =  area.Trim()},
            new SqlParameter() {ParameterName = "@AreaEsquema",Value =  esquema.Trim()},
            new SqlParameter() {ParameterName = "@CodCentro",Value =  centro.Trim()},
            new SqlParameter() {ParameterName = "@CodMarca",Value =  grupoArticulo.Trim()},
            new SqlParameter() {ParameterName = "@CodMaterial",Value =  codigoArticulo.Trim()},
            new SqlParameter() {ParameterName = "@CodSector",Value =  codigoSector.Trim()},
            new SqlParameter() {ParameterName = "@TipoNegociacion",Value =  tipoNegociacion.Trim()},
            new SqlParameter() {ParameterName = "@pTipoCliente", Value = ramoCliente.Trim()},
            new SqlParameter() {ParameterName = "@pClie", Value = idCliente.Trim()},
            new SqlParameter() {ParameterName = "@Jerarquia",Value =  jerarquia.Trim()},
            new SqlParameter() {ParameterName = "@TieneYmhPlus",Value =  yamahaPlus.Trim()},
            new SqlParameter() {ParameterName = "@pPagoConTarjeta", Value = pagoConTarjeta.Trim()}
                  };
            ds = confact.ejecutarSP(conexionPOSWeb, "sp_Web_Descuentos_PRD", pParams);
           

            descuento[0] = dt.Rows[0][0].ToString();
            descuento[1] = dt.Rows[0][1].ToString();
            descuento[2] = dt.Rows[0][2].ToString();
            descuento[3] = dt.Rows[0][3].ToString();

            return descuento;
        }

        #endregion

        #region <<MATERIALES>>

        [WebMethod]
        public DataTable getMaterial(string pAlmacen, string pCodigoMat, string pCanal)
        {
            string[] oReto = new string[20];

            DataTable dt = new DataTable("material");
            dt.Columns.Add("codigo");
            dt.Columns.Add("unidad");
            dt.Columns.Add("descripcion");
            dt.Columns.Add("numeroEuropeo");
            dt.Columns.Add("numeroFabricante");
            dt.Columns.Add("grupoArticulo");
            dt.Columns.Add("denominacionGrupoArticulo");
            dt.Columns.Add("sector");
            dt.Columns.Add("sujeto_a_Lote");
            dt.Columns.Add("perfilNumeroSerie");
            dt.Columns.Add("tipoMaterial");
            dt.Columns.Add("tipoMaterialFlag");
            dt.Columns.Add("precioNeto");
            dt.Columns.Add("precio");
            dt.Columns.Add("descuentoInicial");
            dt.Columns.Add("clasificacionFiscal");
            dt.Columns.Add("denominacionClasificacionFiscal");
            dt.Columns.Add("stockLibreUtilizacion");
            dt.Columns.Add("costo");
            dt.Columns.Add("centro");
            dt.Columns.Add("serie");
            dt.Columns.Add("lote");
            dt.Columns.Add("tipoPosicion");
            dt.Columns.Add("estado");


            if (produccion)
            { //Cuando es PRODUCCION

                Ws_Get_Material_PRD.ZSDWS_POS_CONSULTA_MATERIAL3 serv = new Ws_Get_Material_PRD.ZSDWS_POS_CONSULTA_MATERIAL3();
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                try
                {
                    esCorrecto = false;
                    contE = 0;
                    do
                    {
                        try
                        {
                            serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio 
                            oReto = serv.ZsdrfcPosConsultaMaterial3(pAlmacen, pCodigoMat, pCanal).Split('|');

                            if (oReto[0] == solicitudCancelada)
                            {
                                esCorrecto = false;
                            }
                            else
                            {
                                esCorrecto = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains(solicitudCancelada))
                            {
                                esCorrecto = false;
                            }
                        }
                        contE++;
                    } while (!esCorrecto && contE < contRepetir);

                }
                catch (Exception erra)
                {
                    oReto[0] = erra.Message + " | " + pCodigoMat;
                }
                if (oReto[0] == "0")
                {
                    String flagTypeMaterial = "N";
                    if (oReto[9] != "")
                    {
                        flagTypeMaterial = "L";
                        //fila["Perfil"] = materiales[i].Xchpf;

                    }
                    else if (oReto[10] != "")
                    {
                        flagTypeMaterial = "S";
                       // fila["Perfil"] = materiales[i].Sernp;

                    }
                    else
                    {
                        if (oReto[11].Equals("COMB") || oReto[11].Equals("MCOM") || oReto[11].Equals("ZCOM"))
                        {
                            flagTypeMaterial = "C";
                           // fila["Perfil"] = string.IsNullOrEmpty(materiales[i].Sernp) ? "" : materiales[i].Sernp;
                        }

                        else
                        {
                            flagTypeMaterial = "N";
                        //    fila["Perfil"] = "";
                        }

                    }

                    DataRow fila = dt.NewRow();
                    fila["codigo"] = int.Parse(oReto[1]).ToString();
                    fila["unidad"] = oReto[2];
                    fila["descripcion"] = oReto[3];
                    fila["numeroEuropeo"] = oReto[4];
                    fila["numeroFabricante"] = oReto[5];
                    fila["grupoArticulo"] = oReto[6];
                    fila["denominacionGrupoArticulo"] = oReto[7];
                    fila["sector"] = oReto[8];
                    fila["sujeto_a_Lote"] = oReto[9];
                    fila["perfilNumeroSerie"] = oReto[10];
                    fila["tipoMaterial"] = oReto[11];
                    fila["tipoMaterialFlag"] = flagTypeMaterial;
                    fila["precioNeto"] = oReto[12];
                    fila["precio"] = "0.00";
                    fila["descuentoInicial"] = "0.00";
                    fila["clasificacionFiscal"] = oReto[13];
                    fila["denominacionClasificacionFiscal"] = oReto[14];
                    fila["stockLibreUtilizacion"] = oReto[15];
                    fila["costo"] = oReto[16];
                    fila["centro"] = oReto[17];
                    fila["serie"] = oReto[18];
                    fila["lote"] = oReto[19];
                    fila["tipoPosicion"] = oReto[20];
                    fila["estado"] = oReto[21];
                    dt.Rows.Add(fila);
                }
                return dt;
            }
            else
            { //Cuando es CALIDAD

                Ws_Get_Material.ZSDWS_POS_CONSULTA_MATERIAL3 serv = new Ws_Get_Material.ZSDWS_POS_CONSULTA_MATERIAL3();
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                try
                {
                    esCorrecto = false;
                    contE = 0;
                    do
                    {
                        try
                        {
                            serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio 
                            oReto = serv.ZsdrfcPosConsultaMaterial3(pAlmacen, pCodigoMat, pCanal).Split('|');

                            if (oReto[0] == solicitudCancelada)
                            {
                                esCorrecto = false;
                            }
                            else
                            {
                                esCorrecto = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains(solicitudCancelada))
                            {
                                esCorrecto = false;
                            }
                        }
                        contE++;
                    } while (!esCorrecto && contE < contRepetir);

                }
                catch (Exception erra)
                {
                    oReto[0] = erra.Message + " | " + pCodigoMat;
                }
                if (oReto[0] == "0")
                {
                    String flagTypeMaterial = "N";
                    if (oReto[9] != "")
                    {
                        flagTypeMaterial = "L";
                        //fila["Perfil"] = materiales[i].Xchpf;

                    }
                    else if (oReto[10] != "")
                    {
                        flagTypeMaterial = "S";
                        // fila["Perfil"] = materiales[i].Sernp;

                    }
                    else
                    {
                        if (oReto[11].Equals("COMB") || oReto[11].Equals("MCOM") || oReto[11].Equals("ZCOM"))
                        {
                            flagTypeMaterial = "C";
                            // fila["Perfil"] = string.IsNullOrEmpty(materiales[i].Sernp) ? "" : materiales[i].Sernp;
                        }

                        else
                        {
                            flagTypeMaterial = "N";
                            //    fila["Perfil"] = "";
                        }

                    }
                    DataRow fila = dt.NewRow();
                    fila["codigo"] = int.Parse(oReto[1]).ToString();
                    fila["unidad"] = oReto[2];
                    fila["descripcion"] = oReto[3];
                    fila["numeroEuropeo"] = oReto[4];
                    fila["numeroFabricante"] = oReto[5];
                    fila["grupoArticulo"] = oReto[6];
                    fila["denominacionGrupoArticulo"] = oReto[7];
                    fila["sector"] = oReto[8];
                    fila["sujeto_a_Lote"] = oReto[9];
                    fila["perfilNumeroSerie"] = oReto[10];
                    fila["tipoMaterial"] = oReto[11];
                    fila["tipoMaterialFlag"] = flagTypeMaterial;
                    fila["precioNeto"] = oReto[12];
                    fila["precio"] = "0.00";
                    fila["descuentoInicial"] = "0.00";
                    fila["clasificacionFiscal"] = oReto[13];
                    fila["denominacionClasificacionFiscal"] = oReto[14];
                    fila["stockLibreUtilizacion"] = oReto[15];
                    fila["costo"] = oReto[16];
                    fila["centro"] = oReto[17];
                    fila["serie"] = oReto[18];
                    fila["lote"] = oReto[19];
                    fila["tipoPosicion"] = oReto[20];
                    fila["estado"] = oReto[21];
                    dt.Rows.Add(fila);
                }
                return dt;
            }
        }

        [WebMethod]
        public DataTable getSerieLote(String material, String almacen, String centro)
        {
            DataTable dt = new DataTable("SeriesLotes");
            dt.Columns.Add("SerieLotes");
            dt.Columns.Add("Stock");

            if (produccion) // SI ES PRODUCCION
            {
                Ws_material_lote_serie_alma_PRD.ZWS_MATERIAL_LOTE_SERIE_ALMA mat = new Ws_material_lote_serie_alma_PRD.ZWS_MATERIAL_LOTE_SERIE_ALMA();
                mat.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_material_lote_serie_alma_PRD.ZmmMaterialLote[] lotes;
                Ws_material_lote_serie_alma_PRD.ZmmMaterialSerie[] series;
                Ws_material_lote_serie_alma_PRD.Bapiret1[] error = mat.ZmmMaterialLoteSerieAlma(almacen, material, centro, out lotes, out series);

                if (lotes.Length > 0 || series.Length > 0)
                {
                    if (series.Length > 0) //series
                    {
                        for (int i = 0; i < series.Length; i++)
                        {
                            DataRow fila = dt.NewRow();
                            fila["SerieLotes"] = series[i].Sernr;
                            fila["Stock"] = "1.0";
                            dt.Rows.Add(fila);
                        }

                    }
                    else //lotes
                    {
                        for (int i = 0; i < lotes.Length; i++)
                        {
                            DataRow fila = dt.NewRow();
                            fila["SerieLotes"] = lotes[i].Charg;
                            fila["Stock"] = lotes[i].Labst;
                            dt.Rows.Add(fila);
                        }

                    }
                }
            }
            else // SI ES CALIDAD
            {
                Ws_material_lote_serie_alma.ZWS_MATERIAL_LOTE_SERIE_ALMA mat = new Ws_material_lote_serie_alma.ZWS_MATERIAL_LOTE_SERIE_ALMA();
                mat.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_material_lote_serie_alma.ZmmMaterialLote[] lotes;
                Ws_material_lote_serie_alma.ZmmMaterialSerie[] series;
                Ws_material_lote_serie_alma.Bapiret1[] error = mat.ZmmMaterialLoteSerieAlma(almacen, material, centro, out lotes, out series);

                if (lotes.Length > 0 || series.Length > 0)
                {
                    if (series.Length > 0) //series
                    {
                        for (int i = 0; i < series.Length; i++)
                        {
                            DataRow fila = dt.NewRow();
                            fila["SerieLotes"] = series[i].Sernr;
                            fila["Stock"] = "1.0";
                            dt.Rows.Add(fila);
                        }

                    }
                    else //lotes
                    {
                        for (int i = 0; i < lotes.Length; i++)
                        {
                            DataRow fila = dt.NewRow();
                            fila["SerieLotes"] = lotes[i].Charg;
                            fila["Stock"] = lotes[i].Labst;
                            dt.Rows.Add(fila);
                        }

                    }
                }
            }


            return dt;
        }

        [WebMethod]
        public String getCaracteristica(String codMaterial, String serie)
        {
            String caracteristica = "";
            String respuesta = "";

            if (produccion) // SI ES PRODUCCION
            {
                Ws_CaractMat_PRD.ZSD_WS_CARACTERISCITA_MAT caract = new Ws_CaractMat_PRD.ZSD_WS_CARACTERISCITA_MAT();
                caract.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                caracteristica = caract.ZsdRfcCaracteriscitaMat(codMaterial, serie, out respuesta);
            }
            else // SI ES CALIDAD
            {
                Ws_CaractMat.ZSD_WS_CARACTERISCITA_MAT caract = new Ws_CaractMat.ZSD_WS_CARACTERISCITA_MAT();
                caract.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                caracteristica = caract.ZsdRfcCaracteriscitaMat(codMaterial, serie, out respuesta);
            }


            return respuesta + "|" + caracteristica;

        }

        [WebMethod]
        public DataSet getCombo(String pAlmacen, String pCentro, String pMaterialCombo)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ",";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";

            DataSet ds = new DataSet("ds");

            DataTable dtRespuesta = new DataTable("respuesta");
            dtRespuesta.Columns.Add("codigoRespuesta");
            dtRespuesta.Columns.Add("mensajeRespuesta");

            String codigoRespuesta = "4";
            String mensajeRespuesta = "";

            DataTable dtCombos = new DataTable("combos");
            dtCombos.Columns.Add("codigoMaterial");
            dtCombos.Columns.Add("stock");
            dtCombos.Columns.Add("sujetoA"); //L = Lote, S = Serie, N = Ninguno
            dtCombos.Columns.Add("tipoCombo");
            dtCombos.Columns.Add("cantidadEnCombo");

            List<ItemCombo> itemsCombo = new List<ItemCombo>();

            int stockMenor = 9999;

            if (produccion) //SI ES PRODUCCIÓN
            {
                Ws_Combos_PRD.ZWS_CONSULTA_COMBOS_TABLET service = new Ws_Combos_PRD.ZWS_CONSULTA_COMBOS_TABLET();
                service.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Combos_PRD.ZestMaterialComboTablet[] combos = service.ZsdrfcConsultaCombosTablet(pAlmacen, pCentro, pMaterialCombo);

                if (combos == null || combos.Length == 0)
                {

                    codigoRespuesta = "4";
                    mensajeRespuesta = "No se recuperaron materiales del combo con código: " + pMaterialCombo;
                    goto FINAL;
                }

                codigoRespuesta = "0";
                mensajeRespuesta = "Se recuperaron materiales que componen el combo: " + pMaterialCombo;

                for (int i = 0; i < combos.Length; i++)
                {

                    Ws_Combos_PRD.ZestMaterialComboTablet combo = combos[i];

                    ItemCombo itemCombo = new ItemCombo();

                    itemCombo.CodigoMaterial = int.Parse( combo.Material).ToString();
                    itemCombo.Stock = combo.Stock;
                    if (combo.SujetoSerie == "X")
                    {
                        itemCombo.SujetoA = ItemCombo.SUJETO_A_SERIE;
                    }
                    else if (combo.SujetoLote == "X")
                    {
                        itemCombo.SujetoA = ItemCombo.SUJETO_A_LOTE;
                    }
                    else
                    {
                        itemCombo.SujetoA = ItemCombo.SUJETO_A_NADA;
                    }

                    itemCombo.TipoCombo = combo.TipoCombo;
                    itemCombo.CantidadEnCombo = combo.Cantidad;

                    if (itemCombo.TipoCombo != ItemCombo.COMPONENTE_COMBO)
                    {
                        if (combo.Stock == 0)
                        {
                            itemCombo.MaximaCantidad = 0;
                            stockMenor = 0;
                        }
                        else
                        {
                            itemCombo.MaximaCantidad = (int)(combo.Stock / itemCombo.CantidadEnCombo);
                            if (itemCombo.MaximaCantidad < stockMenor)
                            {
                                stockMenor = itemCombo.MaximaCantidad;
                            }
                        }
                    }


                    itemsCombo.Add(itemCombo);

                }
            }
            else // SI ES CALIDAD
            {
                Ws_Combos.ZWS_CONSULTA_COMBOS_TABLET service = new Ws_Combos.ZWS_CONSULTA_COMBOS_TABLET();
                service.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Combos.ZestMaterialComboTablet[] combos = service.ZsdrfcConsultaCombosTablet(pAlmacen, pCentro, pMaterialCombo);

                if (combos == null || combos.Length == 0)
                {

                    codigoRespuesta = "4";
                    mensajeRespuesta = "No se recuperaron materiales del combo con código: " + pMaterialCombo;
                    goto FINAL;
                }

                codigoRespuesta = "0";
                mensajeRespuesta = "Se recuperaron materiales que componen el combo: " + pMaterialCombo;

                for (int i = 0; i < combos.Length; i++)
                {

                    Ws_Combos.ZestMaterialComboTablet combo = combos[i];

                    ItemCombo itemCombo = new ItemCombo();

                    itemCombo.CodigoMaterial = int.Parse(combo.Material).ToString();
                    itemCombo.Stock = combo.Stock;
                    if (combo.SujetoSerie == "X")
                    {
                        itemCombo.SujetoA = ItemCombo.SUJETO_A_SERIE;
                    }
                    else if (combo.SujetoLote == "X")
                    {
                        itemCombo.SujetoA = ItemCombo.SUJETO_A_LOTE;
                    }
                    else
                    {
                        itemCombo.SujetoA = ItemCombo.SUJETO_A_NADA;
                    }

                    itemCombo.TipoCombo = combo.TipoCombo;
                    itemCombo.CantidadEnCombo = combo.Cantidad;

                    if (itemCombo.TipoCombo != ItemCombo.COMPONENTE_COMBO)
                    {
                        if (combo.Stock == 0)
                        {
                            itemCombo.MaximaCantidad = 0;
                            stockMenor = 0;
                        }
                        else
                        {
                            itemCombo.MaximaCantidad = (int)(combo.Stock / itemCombo.CantidadEnCombo);
                            if (itemCombo.MaximaCantidad < stockMenor)
                            {
                                stockMenor = itemCombo.MaximaCantidad;
                            }
                        }
                    }


                    itemsCombo.Add(itemCombo);

                }

            }

            foreach (ItemCombo itemCombo in itemsCombo)
            {

                DataRow row = dtCombos.NewRow();
                row["codigoMaterial"] = itemCombo.CodigoMaterial;
                row["sujetoA"] = itemCombo.SujetoA;
                row["tipoCombo"] = itemCombo.TipoCombo;
                row["cantidadEnCombo"] = itemCombo.CantidadEnCombo;

                if (itemCombo.TipoCombo.Equals(ItemCombo.COMPONENTE_COMBO))
                {
                    row["stock"] = stockMenor;
                }
                else
                {
                    row["stock"] = itemCombo.Stock;
                }


                dtCombos.Rows.Add(row);

            }

        FINAL:

            //Mensaje de respuesta
            DataRow rowRespuesta = dtRespuesta.NewRow();
            rowRespuesta["codigoRespuesta"] = codigoRespuesta;
            rowRespuesta["mensajeRespuesta"] = mensajeRespuesta;
            dtRespuesta.Rows.Add(rowRespuesta);

            ds.Tables.Add(dtRespuesta);
            ds.Tables.Add(dtCombos);

            return ds;
        }

        [WebMethod]
        public String getPrecioMaterial(String pCodigoMaterial, String pCodigoSAPCliente, String pAlmacen, String pSector,
            String pOficinaVenta, String pCanal, String pOrganizacion)
        {

            String respuesta = "";
            int codigoRespuesta;
            decimal cantidad = 1;

            if (produccion) // SI ES PRODUCCION
            {
                Ws_Precios_cep_PRD.ZWS_SDNETPR0_CEP service = new Ws_Precios_cep_PRD.ZWS_SDNETPR0_CEP();
                service.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                Ws_Precios_cep_PRD.ZststockpriceTablet precios = new Ws_Precios_cep_PRD.ZststockpriceTablet();

                Ws_Precios_cep_PRD.Bapiret1[] tablaRespuesta = service.ZRfcSdnetpr0Cep(pCodigoSAPCliente, cantidad, pAlmacen, pCodigoMaterial, pSector, pOficinaVenta,
                    pOrganizacion, pCanal, out precios, out codigoRespuesta);

                Decimal p = precios.Labst; // TODO: NO SE USA
                if (codigoRespuesta == 0)
                {
                    respuesta = codigoRespuesta + ";" + precios.Netwr.ToString() + ";"
                        + precios.Labst.ToString() + ";" + precios.Descu + ";" + precios.Pneto;
                }
                else
                {
                    respuesta = codigoRespuesta + ";no se determino el precio";
                }
            }
            else // SI ES CALIDAD
            {
                Ws_Precios_cep.ZWS_SDNETPR0_CEP servi2 = new Ws_Precios_cep.ZWS_SDNETPR0_CEP();
                servi2.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Precios_cep.ZststockpriceTablet precios = new Ws_Precios_cep.ZststockpriceTablet();

                String dataPrice = "ZRfcSdnetpr0Cep(IKunnr: " + pCodigoSAPCliente + ", IKwmeng: " + cantidad + ", ILgort: " + pAlmacen + ", IMatnr: " + pCodigoMaterial + ", ISpart: " + pSector + ", IVkbur: " + pOficinaVenta + ", IVkorg: " + pOrganizacion + ", IVtweg: " + pCanal + ")";



                Ws_Precios_cep.Bapiret1[] respue = servi2.ZRfcSdnetpr0Cep(pCodigoSAPCliente, cantidad, pAlmacen, pCodigoMaterial, pSector, pOficinaVenta,
                    pOrganizacion, pCanal, out precios, out codigoRespuesta);

                Decimal p = precios.Labst; // TODO: NO SE USA
                if (codigoRespuesta == 0)
                {
                    respuesta = codigoRespuesta + ";" + precios.Netwr.ToString() + ";"
                        + precios.Labst.ToString() + ";" + precios.Descu + ";" + precios.Pneto;
                }
                else
                {
                    respuesta = codigoRespuesta + ";no se determino el precio";
                }


            }


            return respuesta;
        }

        [WebMethod]
        public String getPrecioMAt(String Mat, String Cliente, String Almacen, String Sector, String ofvent, String canal, String Orge)
        {
            int res = 0;
            String m = "";

            if (produccion) // SI ES PRODUCCION
            {
                Ws_Get_Material_Price_PRD.ZWS_SDNETPR0 serv = new Ws_Get_Material_Price_PRD.ZWS_SDNETPR0();
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Get_Material_Price_PRD.Zststockprice precios = new Ws_Get_Material_Price_PRD.Zststockprice();
                Ws_Get_Material_Price_PRD.Bapiret1[] respue = serv.ZRfcSdnetpr0(Cliente, 1, Almacen, Mat, Sector, ofvent, Orge, canal, out precios, out res);
                Decimal p = precios.Labst; // TODO: NO SE USA
                if (res == 0)
                {
                    m = res + ";" + precios.Netwr.ToString() + ";" + precios.Labst.ToString();
                }
                else
                {
                    m = res + ";0.00";
                }
            }
            else // SI ES CALIDAD
            {
                /*
                Ws_Get_Material_Price.ZWS_SDNETPR0_CEP servi2 = new Ws_Get_Material_Price.ZWS_SDNETPR0_CEP();
                servi2.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Get_Material_Price.ZststockpriceTablet precios = new Ws_Get_Material_Price.ZststockpriceTablet();
                Ws_Get_Material_Price.Bapiret1[] respue = servi2.ZRfcSdnetpr0Cep(Cliente, 1, Almacen, Mat, Sector, ofvent, Orge, canal, out precios, out res);


                Decimal p = precios.Labst; // TODO: NO SE USA
                if (res == 0)
                {
                    m = res + ";" + precios.Netwr.ToString() + ";" + precios.Labst.ToString();
                }
                else
                {
                    m = res + ";no se determino el precio";
                }
                 */
            }

            return m;
        }

        [WebMethod]
        public string Get_Price_Retail(string precio, string esquema, String cliente,
            String codigo, String area, String grArticulo, String alma, String centro,
            String sector, String tipoNegociacion, String tipoCliente, String estadoMaterial,
            String pagoConTarjeta, String costo, String esCombo, String costoCombo, String dctoTodosLocales,
            String cantidadVender, String esWeb)
        {
            switch (centro)
            {
                case "1001":
                    //     Session["bandCentro"] = 10; //electro
                    esquema = "AJE-ELECTRO";
                    break;
                case "1002":
                    //     Session["bandCentro"] = 10; //Perfumería
                    esquema = "AJE-ELECTRO";
                    break;
                case "1004":
                    //     Session["bandCentro"] = 10; //Musical
                    esquema = "AJE-ELECTRO";
                    break;
                case "1006":
                    //     Session["bandCentro"] = 10; //Ferretería
                    esquema = "AJE-FERRETERIA";
                    break;
                case "1007":
                    //     Session["bandCentro"] = 10; //Bazar
                    esquema = "AJE-ELECTRO";
                    break;
                case "1008":
                    //     Session["bandCentro"] = 10; //Motos Chinas
                    esquema = "AJE-ELECTRO";
                    break;
                case "1003":
                    //     Session["bandCentro"] = 10; //Yamaha Motos
                    //if(cmbAlmacen.SelectedItem.ToString().Contains("YAMAHA"))
                    esquema = "AJE-YAMAHA";
                    //else
                    //    aje = "AJE-ELECTRO";
                    break;
                case "1075":
                    //     Session["bandCentro"] = 10; //Lubricantes
                    //if(cmbAlmacen.SelectedItem.ToString().Contains("YAMAHA"))
                    esquema = "AJE-YAMAHA";
                    //else
                    //    aje = "AJE-ELECTRO";
                    break;
                case "1005":
                    //     Session["bandCentro"] = 3; //Polaris
                    esquema = "AJE-YAMAHA";
                    break;
                case "1009":
                    //     Session["bandCentro"] = 7; //Licores
                    esquema = "AJE-TABERNAS";
                    break;
                case "MI01":
                    //     Session["bandCentro"] = 0; //MERCOINTEL
                    esquema = "MERCOINTEL";
                    break;
                case "CE01":
                    //    Session["bandCentro"] = 0; //COMERCIALIZADORA
                    esquema = "COMERCIALIZADORA";
                    break;
            }

            String price = "0.00";
            String resp = "0";
            String data = "new SqlParameter() {ParameterName = @pPrec,Value= " + precio.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pEmpr,Value= " + esquema.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pClie,Value= " + cliente.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pIden,Value= " + codigo.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pArea,Value= " + area.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pGrAr,Value=" + grArticulo.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pAlma,Value= " + alma.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pCentro,Value=" + centro.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pSector,Value=" + sector.Trim() + "},\x0A" +

                                "new SqlParameter() {ParameterName = @pTipoNegociacion,Value=" + tipoNegociacion.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pTipoCliente,Value=" + tipoCliente.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @estadoMaterial,Value=" + estadoMaterial.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pPagoConTarjeta,Value=" + pagoConTarjeta.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pCosto,Value= " + costo.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pEsCombo,Value=" + esCombo.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pCostoCombo,Value=" + costoCombo.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pDctoTodosLosLocales,Value=" + dctoTodosLocales.Trim() + "},\x0A" +

                                "new SqlParameter() {ParameterName = @pCantidadVender,Value=" + cantidadVender.Trim() + "},\x0A" +
                                "new SqlParameter() {ParameterName = @pEsWeb,Value=" + esWeb.Trim() + "},";
            List<SqlParameter> fact = new List<SqlParameter>()
                            {
                                
                                new SqlParameter() {ParameterName = "@pPrec",Value= precio.Trim()},
                                new SqlParameter() {ParameterName = "@pEmpr",Value= esquema.Trim()},
                                new SqlParameter() {ParameterName = "@pClie",Value= cliente.Trim()},
                                new SqlParameter() {ParameterName = "@pIden",Value= codigo.Trim()},
                                new SqlParameter() {ParameterName = "@pArea",Value= 2128},
                                new SqlParameter() {ParameterName = "@pGrAr",Value= grArticulo.Trim()},
                                new SqlParameter() {ParameterName = "@pAlma",Value= alma.Trim()},
                                new SqlParameter() {ParameterName = "@pCentro",Value= centro.Trim()},
                                new SqlParameter() {ParameterName = "@pSector",Value= sector.Trim()},

                                new SqlParameter() {ParameterName = "@pTipoNegociacion",Value= tipoNegociacion.Trim()},
                                new SqlParameter() {ParameterName = "@pTipoCliente",Value= tipoCliente.Trim()},
                                new SqlParameter() {ParameterName = "@estadoMaterial",Value= estadoMaterial.Trim()},
                                new SqlParameter() {ParameterName = "@pPagoConTarjeta",Value= pagoConTarjeta.Trim()},
                                new SqlParameter() {ParameterName = "@pCosto",Value= costo.Trim()},
                                new SqlParameter() {ParameterName = "@pEsCombo",Value= esCombo.Trim()},
                                new SqlParameter() {ParameterName = "@pCostoCombo",Value= costoCombo.Trim()},
                                new SqlParameter() {ParameterName = "@pDctoTodosLosLocales",Value= dctoTodosLocales.Trim()},
            
                                new SqlParameter() {ParameterName = "@pCantidadVender",Value= cantidadVender.Trim()},
                                new SqlParameter() {ParameterName = "@pEsWeb",Value= esWeb.Trim()},
                             };
            try
            {
                DataSet ds = confact.ejecutarSP(conexionPOS, "_PKG_SAPArtis_PRD", fact);

                foreach (DataRow f in ds.Tables[0].Rows)
                {
                    price = f["precio_nuevo"].ToString().Replace(",", ".");
                }
                
            }
            catch (Exception e)
            {
                string error = "ERROR";
                resp = "1";
            }
            return resp+";"+price;
        }

        #endregion

        #region << CLIENTE>>

        [WebMethod]
        public String setClientesap(String Distrito, String PaEmail, String estadocivil, String contribuyente, String grpcliente, String grpcuentas,
                                String genero, String percontacto, String poblacion, String nombre2, String nombre, String region, String cedula,
                                String tipcedula, String calle1, String calle2, String telefono1, String telefono2, String tratamiento,
                                String empleadoGrupo, String ofvent, String cajero)
        {

            //AGREGAR VALIDACION: 
            //SI LA IDENTIFICACION PASADA COMO PARAMETRO ES RUC
            //VERIFICAR DE NUEVO SI EL CLIENTE ESTÁ CREADO. 
            //AL SER RUC Y EL CLIENTE ESTÉ CREADO, NO PERMITIR UTILIZAR ESTE WEBSERVICE
            //CASO CONTRARIO SE HABILITA EL USO DE MODIFICAR RUC

            String resp = "";

            //Variable para convenio de cliente. Esta variable va vacía por el momento para que no se afecte la creación de cliente
            //a perfumería que está utilizando tablet
            String subRamo = "";

            if (produccion) // SI ES PRODUCCION
            {
                Ws_Creacion_Cliente_N_PRD.ZWS_CREATER_COSTUMER_2010 clie = new Ws_Creacion_Cliente_N_PRD.ZWS_CREATER_COSTUMER_2010();
                clie.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                String dataClient = " ZsdCreateCustomer2010(PaBarr: " + Distrito + ", PaBran1: " + subRamo + ", PaBrsch: " + empleadoGrupo + ", PaCelu: " + " " + ", PaEmail: " + PaEmail + ", PaExt1: " + " " + ", PaExt2: " + " " + ", PaFamst: " + estadocivil + ", PaFax: " + " " + ", PaFityp: " + contribuyente + ", PaKdgrp: " + grpcliente + ", PaKtokd: " + grpcuentas + ", PaParge: " + genero + ",  PaParh1: " + percontacto + ",  PaPobl: " + poblacion + ", PaPriap: " + nombre2 + ", PaPrino: " + nombre + ", PaRecco: " + "SI" + ",  PaRegi: " + region + ", PaStcd1: " + cedula + ",  PaStcdt: " + tipcedula + ", PaStr1: " + calle1 + ", PaStr2: " + calle2 + ", PaStr3: " + " " + ", PaTel1: " + telefono1 + ", PaTel2: " + telefono2 + ", PaTrata: " + tratamiento + ", PaUcaja: " + cajero + ", PaVkbur: " + ofvent + ")";
                try
                {
                    String[] cliente = clie.ZsdCreateCustomer2010(Distrito, subRamo, empleadoGrupo, "", PaEmail, "", "", estadocivil, "", contribuyente, grpcliente, grpcuentas, genero, percontacto, poblacion, nombre2, nombre, "SI", region, cedula, tipcedula, calle1, calle2, "", telefono1, telefono2, tratamiento, cajero, ofvent).Split('|');

                    for (int i = 0; i < cliente.Length; i++)
                    {
                        resp = resp + cliente[i] + ";";
                    }

                    String codSap = null;

                    try
                    {
                        if (cliente[0].Trim().Equals("0"))
                        {
                            codSap = cliente[1];
                        }
                    }
                    catch (Exception e)
                    {

                    }

                    if (codSap != null)
                    {
                        String pTrat = "00" + tratamiento.Trim();
                        List<SqlParameter> fact = new List<SqlParameter>()
                            {
                                new SqlParameter() {ParameterName = "@pUsua",Value= cajero},
                                new SqlParameter() {ParameterName = "@pIden",Value= cedula},
                                new SqlParameter() {ParameterName = "@pTiId",Value= Decimal.Parse(tipcedula)},
                                new SqlParameter() {ParameterName = "@pApe",Value= nombre2},
                                new SqlParameter() {ParameterName = "@pNom",Value= nombre},
                                new SqlParameter() {ParameterName = "@pActi",Value= percontacto},
                                new SqlParameter() {ParameterName = "@pDir1",Value= calle1},
                                new SqlParameter() {ParameterName = "@pTel1",Value= telefono1},
                                new SqlParameter() {ParameterName = "@pDir2",Value= calle2},
                                new SqlParameter() {ParameterName = "@pTel2",Value= telefono2},
                                new SqlParameter() {ParameterName = "@pProv",Value= region}, 
                                new SqlParameter() {ParameterName = "@pCiud",Value= poblacion},
                                new SqlParameter() {ParameterName = "@pFeNa",Value= ""},
                                new SqlParameter() {ParameterName = "@pMail",Value= PaEmail},
                                new SqlParameter() {ParameterName = "@pObse",Value= ""},
                                new SqlParameter() {ParameterName = "@pRepre",Value= ""},
                                new SqlParameter() {ParameterName = "@pSapCod",Value= codSap},
                                new SqlParameter() {ParameterName = "@pSAPGrCl",Value= grpcliente},
                                new SqlParameter() {ParameterName = "@pSAPGrCt",Value= grpcuentas},
                                new SqlParameter() {ParameterName = "@pSAPRamo",Value= ""},
                                new SqlParameter() {ParameterName = "@pSapTrat",Value= pTrat}
                             };
                        try
                        {
                            confact.ejecutarSP(conexionPOS, "_SP_SAP_ACTUALIZA_INTE", fact);

                        }
                        catch (Exception e)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            guardarLogs("Cliente", "wsPOSweb.asmx.cs", "setClientesap", resp, "SAP", dataClient, cajero, ofvent, "");
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
                catch (Exception e1)
                {
                    guardarLogs("Cliente", "wsPOSweb.asmx.cs", "setClientesap", resp, "SAP", dataClient, cajero, ofvent, "");
                }

            }
            else // SI ES CALIDAD
            {
                Ws_Creacion_Cliente_N.ZWS_CREATER_COSTUMER_2010 clie = new Ws_Creacion_Cliente_N.ZWS_CREATER_COSTUMER_2010();
                clie.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                String dataClient = " ZsdCreateCustomer2010(PaBarr: " + Distrito + ", PaBran1: " + subRamo + ", PaBrsch: " + empleadoGrupo + ", PaCelu: " + " " + ", PaEmail: " + PaEmail + ", PaExt1: " + " " + ", PaExt2: " + " " + ", PaFamst: " + estadocivil + ", PaFax: " + " " + ", PaFityp: " + contribuyente + ", PaKdgrp: " + grpcliente + ", PaKtokd: " + grpcuentas + ", PaParge: " + genero + ",  PaParh1: " + percontacto + ",  PaPobl: " + poblacion + ", PaPriap: " + nombre2 + ", PaPrino: " + nombre + ", PaRecco: " + "SI" + ",  PaRegi: " + region + ", PaStcd1: " + cedula + ",  PaStcdt: " + tipcedula + ", PaStr1: " + calle1 + ", PaStr2: " + calle2 + ", PaStr3: " + " " + ", PaTel1: " + telefono1 + ", PaTel2: " + telefono2 + ", PaTrata: " + tratamiento + ", PaUcaja: " + cajero + ", PaVkbur: " + ofvent + ")";
                try
                {
                    String[] cliente = clie.ZsdCreateCustomer2010(Distrito, subRamo, empleadoGrupo, "", PaEmail, "", "", estadocivil, "", contribuyente, grpcliente, grpcuentas, genero, percontacto, poblacion, nombre2, nombre, "SI", region, cedula, tipcedula, calle1, calle2, "", telefono1, telefono2, tratamiento, cajero, ofvent).Split('|');

                    for (int i = 0; i < cliente.Length; i++)
                    {
                        resp = resp + cliente[i] + ";";
                    }

                    String codSap = null;

                    try
                    {
                        if (cliente[0].Trim().Equals("0"))
                        {
                            codSap = cliente[1];
                        }
                    }
                    catch (Exception e)
                    {

                    }

                    if (codSap != null)
                    {
                        String pTrat = "00" + tratamiento.Trim();
                        List<SqlParameter> fact = new List<SqlParameter>()
                            {
                                new SqlParameter() {ParameterName = "@pUsua",Value= cajero},
                                new SqlParameter() {ParameterName = "@pIden",Value= cedula},
                                new SqlParameter() {ParameterName = "@pTiId",Value= Decimal.Parse(tipcedula)},
                                new SqlParameter() {ParameterName = "@pApe",Value= nombre2},
                                new SqlParameter() {ParameterName = "@pNom",Value= nombre},
                                new SqlParameter() {ParameterName = "@pActi",Value= percontacto},
                                new SqlParameter() {ParameterName = "@pDir1",Value= calle1},
                                new SqlParameter() {ParameterName = "@pTel1",Value= telefono1},
                                new SqlParameter() {ParameterName = "@pDir2",Value= calle2},
                                new SqlParameter() {ParameterName = "@pTel2",Value= telefono2},
                                new SqlParameter() {ParameterName = "@pProv",Value= region}, 
                                new SqlParameter() {ParameterName = "@pCiud",Value= poblacion},
                                new SqlParameter() {ParameterName = "@pFeNa",Value= ""},
                                new SqlParameter() {ParameterName = "@pMail",Value= PaEmail},
                                new SqlParameter() {ParameterName = "@pObse",Value= ""},
                                new SqlParameter() {ParameterName = "@pRepre",Value= ""},
                                new SqlParameter() {ParameterName = "@pSapCod",Value= codSap},
                                new SqlParameter() {ParameterName = "@pSAPGrCl",Value= grpcliente},
                                new SqlParameter() {ParameterName = "@pSAPGrCt",Value= grpcuentas},
                                new SqlParameter() {ParameterName = "@pSAPRamo",Value= ""},
                                new SqlParameter() {ParameterName = "@pSapTrat",Value= pTrat}
                             };
                        try
                        {
                            confact.ejecutarSP(conexionPOS, "_SP_SAP_ACTUALIZA_INTE", fact);

                        }
                        catch (Exception e)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            guardarLogs("Cliente", "wsPOSweb.asmx.cs", "setClientesap", resp, "SAP", dataClient, cajero, ofvent, "");
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
                catch (Exception e1)
                {
                    try
                    {
                        guardarLogs("Cliente", "wsPOSweb.asmx.cs", "setClientesap", e1.Message, "SAP", dataClient, cajero, ofvent, "");
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            return resp;
        }

        [WebMethod]
        public DataTable getCliente(String ide)
        {

            DataTable dt = new DataTable("cliente");

            dt.Columns.Add("SAPcod");
            dt.Columns.Add("tipo");                  //Tipo de Cliente (DEMP, FAM1, DRET...)
            dt.Columns.Add("tipoIdentificacion");    //Tipo identificacion (01 -RUC, 02 - Cédula, 03 - Pasaporte)
            dt.Columns.Add("ide");                   //Identificación del cliente
            dt.Columns.Add("nombre");
            dt.Columns.Add("provincia");
            dt.Columns.Add("ciudad");
            dt.Columns.Add("parroquia");
            dt.Columns.Add("direccion");
            dt.Columns.Add("genero");
            dt.Columns.Add("tratamiento");
            dt.Columns.Add("estadoCivil");
            dt.Columns.Add("actividadEconomica");
            dt.Columns.Add("mail");
            dt.Columns.Add("tel1");
            dt.Columns.Add("tel2");
            dt.Columns.Add("cupoCredito");           //Deuda (Si excede límite de crédito)
            dt.Columns.Add("ramo");
            dt.Columns.Add("subramo");
            dt.Columns.Add("bloqueadoVentaCredito"); //S -> BloqueadoN -> Desbloqueado
            dt.Columns.Add("fechaNacimiento");
            dt.Columns.Add("nombreProvincia");
            dt.Columns.Add("nombreCiudad");

            if (produccion) //SI ES PRODUCCIÓN
            {
                Ws_Get_Cliente_PRD.ZSDWS_POS_CONSULTA_CLIENTES clie = new Ws_Get_Cliente_PRD.ZSDWS_POS_CONSULTA_CLIENTES();
                clie.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                String[] cliente = clie.ZsdrfcPosConsultaCliente(ide).Split('|');
                if (cliente[0] == "0")
                {
                    DataRow fila = dt.NewRow();
                    fila["SAPcod"] = cliente[1];
                    fila["tipo"] = cliente[2];
                    fila["tipoIdentificacion"] = cliente[4];
                    fila["ide"] = cliente[5];
                    fila["nombre"] = cliente[7] + " " + cliente[6];
                    fila["provincia"] = cliente[8];
                    fila["ciudad"] = cliente[9];
                    fila["parroquia"] = cliente[10];
                    fila["direccion"] = cliente[11];
                    fila["genero"] = cliente[14];
                    fila["tratamiento"] = cliente[15];
                    fila["estadoCivil"] = cliente[16];
                    fila["actividadEconomica"] = cliente[17];
                    fila["mail"] = cliente[18];
                    fila["tel1"] = cliente[19];
                    fila["tel2"] = cliente[20];
                    fila["cupoCredito"] = cliente[21];
                    fila["ramo"] = cliente[22];
                    fila["subramo"] = cliente[23];
                    fila["bloqueadoVentaCredito"] = cliente[24];
                    //fila["fechaNacimiento"] = cliente[25];
                    fila["nombreProvincia"] = cliente[25];
                    fila["nombreCiudad"] = cliente[26];
                    dt.Rows.Add(fila);
                }
            }
            else //SI ES CALIDAD
            {
                try
                {
                    Ws_Get_Cliente.ZSDWS_POS_CONSULTA_CLIENTES clie = new Ws_Get_Cliente.ZSDWS_POS_CONSULTA_CLIENTES();
                    clie.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                    String[] cliente = clie.ZsdrfcPosConsultaCliente(ide).Split('|');
                    if (cliente[0] == "0")
                    {                     
                        DataRow fila = dt.NewRow();
                        fila["SAPcod"] = cliente[1];
                        fila["tipo"] = cliente[2];
                        fila["ide"] = cliente[5];
                        fila["nombre"] = cliente[7] + " " + cliente[6];
                        fila["direccion"] = cliente[11];
                        fila["mail"] = cliente[18];
                        fila["tel1"] = cliente[19];
                        fila["tel2"] = cliente[20];
                        fila["cupoCredito"] = cliente[21];
                        dt.Rows.Add(fila);
                    }
                }
                catch (Exception e)
                {
                    String error = "error";
                }
            }

            return dt;
        }

        [WebMethod]
        public String setAmpliacion(String pCanal, String pCodigoSAPCliente, String pSociedad, String jsonSectores)
        {
            String res = "";
            int veri = 0;

            JArray sectoresArray = (JArray)JsonConvert.DeserializeObject(jsonSectores);


            if (produccion) // SI ES PRODUCCION
            {
                Ws_Ampliacion_PRD.ZWS_AMPLIA_CLIENTE_TABLET service = new Ws_Ampliacion_PRD.ZWS_AMPLIA_CLIENTE_TABLET();
                service.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                Ws_Ampliacion_PRD.ZsdEstSector[] sectores = new Ws_Ampliacion_PRD.ZsdEstSector[sectoresArray.Count];

                for (int i = 0; i < sectoresArray.Count; i++)
                {
                    String sector = sectoresArray[i].ToString();

                    Ws_Ampliacion_PRD.ZsdEstSector sectorItem = new Ws_Ampliacion_PRD.ZsdEstSector();
                    sectorItem.Spart = sector;

                    sectores[i] = sectorItem;
                }

                Ws_Ampliacion_PRD.Bapiret1[] erro = service.ZmmAmpliacionClienteTablet(pCanal, pCodigoSAPCliente, sectores, pSociedad, out veri);

                if (veri == 0)
                {
                    res = veri + "";
                }
                else
                {
                    for (int i = 0; i < erro.Length; i++)
                    {
                        res = res + erro[i].Message;
                    }
                }
            }
            else // SI ES CALIDAD
            {
                /*
                Ws_Ampliacion.ZWS_AMPLIA_CLIENTE_TABLET service = new Ws_Ampliacion.ZWS_AMPLIA_CLIENTE_TABLET();
                service.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContreSap);

                Ws_Ampliacion.ZsdEstSector[] sectores = new Ws_Ampliacion.ZsdEstSector[sectoresArray.Count];

                for (int i = 0; i < sectoresArray.Count; i++)
                {
                    String sector = sectoresArray[i].ToString();

                    Ws_Ampliacion.ZsdEstSector sectorItem = new Ws_Ampliacion.ZsdEstSector();
                    sectorItem.Spart = sector;

                    sectores[i] = sectorItem;
                }

                Ws_Ampliacion.Bapiret1[] erro = service.ZmmAmpliacionClienteTablet(pCanal, pCodigoSAPCliente, sectores, pSociedad, out veri);

                if (veri == 0)
                {
                    res = veri + "";
                }
                else
                {
                    for (int i = 0; i < erro.Length; i++)
                    {
                        res = res + erro[i].Message;
                    }
                }*/
            }


            return res;
        }

        #endregion

        #region <<VENDEDORES>>
        [WebMethod]
        public DataTable getVendedores(String ofiVent)
        {
            DataSet ds = null;
            DataTable dt = null;
            if (ofiVent != "")
            {
                List<SqlParameter> fact = new List<SqlParameter>()
                            {
                                 new SqlParameter() {ParameterName = "@pOfiVenta",Value= ofiVent.Trim()},                                 
                            };
                try
                {
                    ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Get_Sellers", fact);
                    ds.Tables[0].TableName = "Vendedores";
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
                catch (Exception e)
                {
                    dt = null;
                }

            }

            return dt;
        }

        #endregion

        #region << LOGAPP >>

        public void guardarLogs(String log_errTransaccion, String log_errClase, String log_errMetodo, String log_errError,
            String log_errTipo, String log_errData, String log_usu, String log_idSapOfi, String idDocCab)
        {
            DataSet ds = null;
            String veridicador = "";

            veridicador = "1";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@plogErrTransaccion",Value= log_errTransaccion.Trim()},
                new SqlParameter() {ParameterName = "@plorErrClase",Value= log_errClase.Trim()},
                new SqlParameter() {ParameterName = "@plogErrMetodo",Value= log_errMetodo.Trim()},
                new SqlParameter() {ParameterName = "@plogErrError",Value= log_errError.Trim()},
                new SqlParameter() {ParameterName = "@plogErrTipo",Value= log_errTipo.Trim()},
                new SqlParameter() {ParameterName = "@plogErrData",Value= log_errData.Trim()},
                new SqlParameter() {ParameterName = "@pusuarioCr",Value= log_usu.Trim()},
                new SqlParameter() {ParameterName = "@psapOfi",Value= log_idSapOfi.Trim()},
                new SqlParameter() {ParameterName = "@pidDocumentosCabecera", Value = idDocCab.Trim()}
            };
            try
            {
                ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Set_Logs", parameters);
            }
            catch (Exception e)
            {
                veridicador = e.ToString();
            }

        }

        #endregion
    
        #region <<CHUI>>

        //bool esCorrecto = false;
        //int contE = 0;
        //int contRepetir = 10;
        //string solicitudCancelada = "Anulada la solicitud: La solicitud fue cancelada";        
       
        //Recupera provincias, ciudades y parroquias
        [WebMethod]
        public DataTable GetProvCiudParr(String esProduccion, String provincia, String ciudad, String parroquia)
        {
            string error = "";
            
            DataTable dtRespuesta = new DataTable("dtRespuesta");

            dtRespuesta.Columns.Add("codProv", typeof(string));
            dtRespuesta.Columns.Add("desProv", typeof(string));
            dtRespuesta.Columns.Add("codCiud", typeof(string));
            dtRespuesta.Columns.Add("desCiud", typeof(string));
            dtRespuesta.Columns.Add("codParr", typeof(string));
            dtRespuesta.Columns.Add("desParr", typeof(string));

            string prov = provincia.Length > 0 ? provincia.PadLeft(2, '0') : provincia;
            string ciud = ciudad.Length > 0 ? ciudad.PadLeft(2, '0') : ciudad;
            string parr = parroquia.Length > 0 ? parroquia.PadLeft(2, '0') : parroquia;

            if (esProduccion == "S"){ //Cuando es PRODUCCION

                wsGetProvCiudParr_PRD.ZWS_CONSULTAR_PROVINCIAS serv = new wsGetProvCiudParr_PRD.ZWS_CONSULTAR_PROVINCIAS();
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                wsGetProvCiudParr_PRD.ZstrCantParro[] cantParrResult = new wsGetProvCiudParr_PRD.ZstrCantParro[0];

                try {
                    esCorrecto = false; 
                    contE = 0;
                    do{
                        try{
                            serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                            cantParrResult = serv.ZrfcConsultarProvCantParro(ciud, parr, prov);

                            esCorrecto = true;                            
                        }catch (Exception ex){                       
                            if (ex.Message.Contains(solicitudCancelada)){
                                esCorrecto = false;
                            }                                                                
                        }
                        contE++;
                    } while (!esCorrecto && contE < contRepetir);

                    DataRow dr = dtRespuesta.NewRow();
                    dr["codProv"] = "0";
                    dr["desProv"] = "Seleccione";
                    dr["codCiud"] = "0";
                    dr["desCiud"] = "Seleccione";
                    dr["codParr"] = "0";
                    dr["desParr"] = "Seleccione";
                    dtRespuesta.Rows.Add(dr);
                    
                    for (int i = 0; i < cantParrResult.Length; i++)
                    {
                        DataRow drRespuesta = dtRespuesta.NewRow();

                        drRespuesta["codProv"] = cantParrResult[i].CodProv;
                        drRespuesta["desProv"] = cantParrResult[i].DescProv;
                        drRespuesta["codCiud"] = cantParrResult[i].CodCant;
                        drRespuesta["desCiud"] = cantParrResult[i].DescCant;
                        drRespuesta["codParr"] = cantParrResult[i].CodParr;
                        drRespuesta["desParr"] = cantParrResult[i].DescParr;

                        dtRespuesta.Rows.Add(drRespuesta);
                    }                  

                } catch (Exception erra) {
                    error = erra.Message;
                }

                return dtRespuesta;
      
            }else{ //Cuando es CALIDAD                

                //wsGetProvCiudParr.ZWS_CONSULTAR_PROVINCIAS serv = new wsGetProvCiudParr.ZWS_CONSULTAR_PROVINCIAS();
                //serv.Credentials = new System.Net.NetworkCredential(pUser, pPass);

                //wsGetProvCiudParr.ZstrCantParro[] cantParrResult = new wsGetProvCiudParr.ZstrCantParro[0];

                //try {
                //    esCorrecto = false; 
                //    contE = 0;
                //    do{
                //        try{
                //            serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                //            cantParrResult = serv.ZrfcConsultarProvCantParro(ciud, parr, prov);

                //            esCorrecto = true;                            
                //        }catch (Exception ex){                       
                //            if (ex.Message.Contains(solicitudCancelada)){
                //                esCorrecto = false;
                //            }                                                                
                //        }
                //        contE++;
                //    } while (!esCorrecto && contE < contRepetir);

                //    //Orden de Trabajo
                //    for (int i = 0; i < cantParrResult.Length; i++)
                //    {
                //        DataRow drRespuesta = dtRespuesta.NewRow();

                //        drRespuesta["codProv"] = cantParrResult[i].CodProv;
                //        drRespuesta["desProv"] = cantParrResult[i].DescProv;
                //        drRespuesta["codCiud"] = cantParrResult[i].CodCant;
                //        drRespuesta["desCiud"] = cantParrResult[i].DescCant;
                //        drRespuesta["codParr"] = cantParrResult[i].CodParr;
                //        drRespuesta["desParr"] = cantParrResult[i].DescParr;

                //        dtRespuesta.Rows.Add(drRespuesta);
                //    }                  

                //} catch (Exception erra) {
                //    error = erra.Message;
                //}

                return dtRespuesta;
            }            
        }

        //Recupera los Subramos        
        [WebMethod]
        public DataTable getSubramos(String esProduccion)
        {            
            DataTable dt = new DataTable("dtSubramos");

            if (esProduccion == "S"){ //Cuando es PRODUCCION
                wsGetSubramo_PRD.ZWS_LISTA_SUBRAMOS serv = new wsGetSubramo_PRD.ZWS_LISTA_SUBRAMOS();
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);                                                                

                try{
                    serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                    wsGetSubramo_PRD.ZsdEstListaSubramo[] listaSubramo = serv.ZsdrfcListaSubramos();                                  

                    dt.Columns.Add("id", typeof(string));
                    dt.Columns.Add("descripcion", typeof(string));                    

                    for (int i = 0; i < listaSubramo.Count(); i++)
                    {
                        DataRow fila = dt.NewRow();
                        fila["id"] = listaSubramo[i].Braco;
                        fila["descripcion"] = listaSubramo[i].Vtext;
                        dt.Rows.Add(fila);
                    }                  
                }catch (Exception err){
                   
                }               
            }else{ //Cuando es CALIDAD
                //wsGetSubramo.ZWS_LISTA_SUBRAMOS serv = new wsGetSubramo.ZWS_LISTA_SUBRAMOS();
                //serv.Credentials = new System.Net.NetworkCredential(pUser, pPass);                                                                

                //try{
                //    serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                //    wsGetSubramo.ZsdEstListaSubramo[] listaSubramo = serv.ZsdrfcListaSubramos();                                  

                //    dt.Columns.Add("id", typeof(string));
                //    dt.Columns.Add("descripcion", typeof(string));                    

                //    for (int i = 0; i < listaSubramo.Count(); i++)
                //    {
                //        DataRow fila = dt.NewRow();
                //        fila["id"] = listaSubramo[i].Braco;
                //        fila["descripcion"] = listaSubramo[i].Vtext;
                //        dt.Rows.Add(fila);
                //    }                  
                //}catch (Exception err){
                   
                //}                
            }
            return dt;
        }

        [WebMethod]
        public DataTable getSubGrupo(int opcion, string pIdGrupoSubgrupo, string pGruIdentificacion)
        {
            DataTable dt = new DataTable("dtSubgrupo");
            
            try{
                string idGrupoSubgrupo = (pIdGrupoSubgrupo == "" || pIdGrupoSubgrupo == "0") ? null : pIdGrupoSubgrupo;

                dt = Bd.getGrupoSubgrupo(opcion, idGrupoSubgrupo, pGruIdentificacion);              
            }catch (Exception e){
         
            }

            return dt;
        }

        //Graba o modifica persona en SAP y en la BD
        [WebMethod]
        public DataTable setPersona(                                 
                                    string pPerIdentificacion,
                                    string pPerNombres,
                                    string pPerApellidos,
                                    string pPerDireccion,
                                    string pPerDireccion2,
                                    string pPerTelefono,
                                    string pPerCelular,
                                    string pPerEmail,
                                    string pPerFechaNacimiento,
                                    string pPerProvincia,
                                    string pPerCanton,
                                    string pPerParroquia,
                                    string pPerCodigoSAP,
                                    string pPerObservacion,
                                    string pPerRamo,
                                    string pPerSubramo,
                                    string pUsuario,
                                    string pIdGruSubGenero,
                                    string pIdGruSubEstadoCivil,
                                    string pIdGruSubTratamiento,
                                    string pIdGruSubActividadEc,
                                    string pIdGruSubTipoIdentificacion,
                                    string pIdGruSubGrupoCliente,
                                    string pIdGruSubGrupoCuenta
            ){          

            string[] sReto = new string[2];
            bool puedeModificar = true;
            DataTable dtPersona = new DataTable("dtPersona");

            DataTable dtResultado = new DataTable("dtResultado");
            dtResultado.Columns.Add("codResultado", typeof(string));
            dtResultado.Columns.Add("mensaje", typeof(string));

            DataRow drResultado = dtResultado.NewRow();

            //----------------------------------------- GRABA EL CLIENTE EN LA BD ---------------------------------------------------
            string idGenero = (pIdGruSubGenero == "" || pIdGruSubGenero == "0") ? null : pIdGruSubGenero;
            string idEstadoCivil = (pIdGruSubEstadoCivil == "" || pIdGruSubEstadoCivil == "0") ? null : pIdGruSubEstadoCivil;
            string idTratamiento = (pIdGruSubTratamiento == "" || pIdGruSubTratamiento == "0") ? null : pIdGruSubTratamiento;
            string idActividadEc = (pIdGruSubActividadEc == "" || pIdGruSubActividadEc == "0") ? null : pIdGruSubActividadEc;
            string idTipoIdentificacion = (pIdGruSubTipoIdentificacion == "" || pIdGruSubTipoIdentificacion == "0") ? null : pIdGruSubTipoIdentificacion;
            string idGrupoCliente = (pIdGruSubGrupoCliente == "" || pIdGruSubGrupoCliente == "0") ? null : pIdGruSubGrupoCliente;
            string idGrupoCuenta = (pIdGruSubGrupoCuenta == "" || pIdGruSubGrupoCuenta == "0") ? null : pIdGruSubGrupoCuenta;
            //-----------------------------------------------------------------------------------------------------------------------
      
            wsAddUpdCliente.ZWS_ADD_UPD_CLIENTE serv = new wsAddUpdCliente.ZWS_ADD_UPD_CLIENTE();
            serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);                               

            if (pIdGruSubTipoIdentificacion == "26") { //RUC
                try{
                    dtPersona = getCliente(pPerIdentificacion);
                    if (dtPersona.Rows.Count > 0){                           
                        puedeModificar = false;                         
                    }
                }catch (Exception err){
                    puedeModificar = false;
                }                   
            }

            if (puedeModificar){
                try{

                    string parroquia = (pPerProvincia + pPerCanton + pPerParroquia).TrimStart('0');                  
                    string con2 = "";

                    if (pPerRamo == "CON2"){                    
                        con2 = "X";  
                    }else if (pPerRamo == "CON1") {                  
                        con2 = "Y";                          
                    }

                    if (pPerSubramo.Length > 0){
                        if (pPerSubramo == "OTRAEMP"){
                            con2 = "Y";
                        }else{
                            con2 = "X";
                        }
                    }                    

                    string estadoCivil = getSubGrupo(2, pIdGruSubEstadoCivil, "").Rows[0]["gruIdentificacion"].ToString();
                    string tipoContribuyente = pIdGruSubTipoIdentificacion == "26" ? "PJ" : "PN";
                    string grupoCliente = getSubGrupo(2, pIdGruSubGrupoCliente, "").Rows[0]["gruIdentificacion"].ToString();
                    string grupoCuenta = getSubGrupo(2, pIdGruSubGrupoCuenta, "").Rows[0]["gruIdentificacion"].ToString();
                    string genero = getSubGrupo(2, pIdGruSubGenero, "").Rows[0]["gruIdentificacion"].ToString();
                    string actividadEconomica = getSubGrupo(2, pIdGruSubActividadEc, "").Rows[0]["gruIdentificacion"].ToString();
                    string ciudad =  (pPerProvincia + pPerCanton).TrimStart('0');
                    string tipoIdentificacion = getSubGrupo(2, pIdGruSubTipoIdentificacion, "").Rows[0]["gruIdentificacion"].ToString();
                    string tratamiento = getSubGrupo(2, pIdGruSubTratamiento, "").Rows[0]["gruIdentificacion"].ToString();
                        
                    serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                    sReto = serv.ZsdBapiCreateCustomer(parroquia, pPerSubramo, con2, pPerCelular, pPerEmail, "", "", estadoCivil, "", tipoContribuyente, "", grupoCliente, grupoCuenta, "", genero, actividadEconomica, ciudad, pPerApellidos, pPerNombres, "SI", pPerProvincia, "", pPerIdentificacion, tipoIdentificacion, pPerDireccion, pPerDireccion2, pPerCelular, pPerTelefono, "", tratamiento, pUsuario, "0101").Split('|');
                    //sReto = serv.ZsdBapiCreateCustomer(parroquia, pPerSubramo, con2, pPerCelular, pPerEmail, "", "", estadoCivil, "", pPerFechaNacimiento, tipoContribuyente, "", grupoCliente, grupoCuenta, "", genero, actividadEconomica, ciudad, pPerApellidos, pPerNombres, "SI", pPerProvincia, "", pPerIdentificacion, tipoIdentificacion, pPerDireccion, pPerDireccion2, pPerCelular, pPerTelefono, "", tratamiento, pUsuario, "0101").Split('|');

                    if (sReto[0] == "0"){
                        Bd.spPersona(1, pPerIdentificacion, pPerNombres, pPerApellidos, pPerDireccion, pPerDireccion2, pPerTelefono, pPerCelular, pPerEmail, pPerFechaNacimiento, pPerProvincia, pPerCanton, pPerParroquia, sReto[1], pPerObservacion, pPerRamo, pPerSubramo, pUsuario, idGenero, idEstadoCivil, idTratamiento, idActividadEc, idTipoIdentificacion, idGrupoCliente, idGrupoCuenta);

                        drResultado["codResultado"] = "0";
                        drResultado["mensaje"] = sReto[1];
                    }else{
                        drResultado["codResultado"] = "";
                        drResultado["mensaje"] = sReto[0];
                    }

                }catch (Exception erra){
                    drResultado["codResultado"] = "";
                    drResultado["mensaje"] = erra.Message;                             
                }
            }else{
                drResultado["codResultado"] = "";
                drResultado["mensaje"] = "No se puede modificar cliente con RUC";               
            }

            dtResultado.Rows.Add(drResultado);

            return dtResultado;
        }        

        //Consulta persona en SAP y en la BD
        [WebMethod]
        public DataTable getPersona(string pPerIdentificacion){                      

            DataTable dtCliente = new DataTable("cliente");
            DataTable dt = new DataTable("dtPersona");

            dtCliente = getCliente(pPerIdentificacion);
            if (dtCliente.Rows.Count == 0){
                pPerIdentificacion = "0000000000";
            }

            dt = Bd.spPersona(2, pPerIdentificacion, "", "", "", "", "", "", "", "", "", "","","", "", "", "", "", null, null, null, null, null, null, null);
               
            if (dtCliente.Rows.Count > 0){           
                dt.Rows[0]["perRamo"] = dtCliente.Rows[0]["ramo"];
                dt.Rows[0]["perSubramo"] = dtCliente.Rows[0]["subramo"];
            }
                   
            return dt;
        }

        //Consulta la coordenada (LLave - Valor)
        [WebMethod]
        public DataTable getTarjetaCoordenadas()
        {
            DataTable dt = new DataTable("dtTarjetaCoordenadas");

            dt = Bd.spTarjetaCoordenadas();

            return dt;
        }


        //Recupera información del cliente TODAVIA NO SE UTILIZA
        [WebMethod]
        public DataSet GetInfoCliente(string idCliente)
        {
            int salida = 0;
            string error = "";

            DataSet dsCliente = new DataSet("dsCliente");

            DataTable dtInfoCliente = new DataTable("dtInfoCliente");
            dtInfoCliente.Columns.Add("actividadEconomica", typeof(string));
            dtInfoCliente.Columns.Add("apellidos", typeof(string));
            dtInfoCliente.Columns.Add("bloqCredito", typeof(string));
            dtInfoCliente.Columns.Add("calle", typeof(string));
            dtInfoCliente.Columns.Add("cedulaRuc", typeof(string));
            dtInfoCliente.Columns.Add("codEstadoCivil", typeof(string));
            dtInfoCliente.Columns.Add("codGenero", typeof(string));
            dtInfoCliente.Columns.Add("codigoSap", typeof(string));
            dtInfoCliente.Columns.Add("codProvincia", typeof(string));
            dtInfoCliente.Columns.Add("codRamo", typeof(string));
            dtInfoCliente.Columns.Add("codRamo1", typeof(string));
            dtInfoCliente.Columns.Add("codTratamiento", typeof(string));
            dtInfoCliente.Columns.Add("correo", typeof(string));
            dtInfoCliente.Columns.Add("deuda", typeof(string));
            dtInfoCliente.Columns.Add("distrito", typeof(string));
            dtInfoCliente.Columns.Add("fecNacimiento", typeof(string));
            dtInfoCliente.Columns.Add("grpCtaCliente", typeof(string));
            dtInfoCliente.Columns.Add("grupoCliente", typeof(string));
            dtInfoCliente.Columns.Add("limiteCredito", typeof(string));
            dtInfoCliente.Columns.Add("nombres", typeof(string));
            dtInfoCliente.Columns.Add("pais", typeof(string));
            dtInfoCliente.Columns.Add("poblacion", typeof(string));
            dtInfoCliente.Columns.Add("saldoCliente", typeof(string));
            dtInfoCliente.Columns.Add("telefono1", typeof(string));
            dtInfoCliente.Columns.Add("telefono2", typeof(string));
            dtInfoCliente.Columns.Add("textoActEconomica", typeof(string));
            dtInfoCliente.Columns.Add("textoDistrito", typeof(string));
            dtInfoCliente.Columns.Add("textoEstadoCivil", typeof(string));
            dtInfoCliente.Columns.Add("textoGenero", typeof(string));
            dtInfoCliente.Columns.Add("textoProvincia", typeof(string));
            dtInfoCliente.Columns.Add("textoRamo", typeof(string));
            dtInfoCliente.Columns.Add("textoRamo1", typeof(string));
            dtInfoCliente.Columns.Add("textoTratamiento", typeof(string));
            dtInfoCliente.Columns.Add("tipoIdentificacion", typeof(string));    
            
            dsCliente.Tables.Add(dtInfoCliente);

            if (esProduccion){ //Cuando es PRODUCCION

                                
            }else{ //Cuando es CALIDAD                

                wsGetInfoCliente.ZWS_DATOS_CLIENTE_POSWEBService serv = new wsGetInfoCliente.ZWS_DATOS_CLIENTE_POSWEBService();
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                wsGetInfoCliente.ZestNpCabCliente resultado = new wsGetInfoCliente.ZestNpCabCliente();

                try {
                    esCorrecto = false;
                    contE = 0;
                    do{
                        try{
                            serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                            resultado = serv.ZfiDatosClientePos2023(idCliente, out salida);                       

                            if (salida == 0){
                                esCorrecto = true;
                            }else{
                                esCorrecto = false;
                            }
                        }catch (Exception ex){                       
                            if (ex.Message.Contains(solicitudCancelada)){
                                esCorrecto = false;
                            }                                                                
                        }
                        contE++;
                    } while (!esCorrecto && contE < contRepetir);

                    DataRow drInfoCliente = dtInfoCliente.NewRow();

                    drInfoCliente["actividadEconomica"] = resultado.ActividadEconomica;
                    drInfoCliente["apellidos"] = resultado.Apellidos;
                    drInfoCliente["bloqCredito"] = resultado.BloqCredito;
                    drInfoCliente["calle"] = resultado.Calle;
                    drInfoCliente["cedulaRuc"] = resultado.CedulaRuc;
                    drInfoCliente["codEstadoCivil"] = resultado.CodEstadoCivil;
                    drInfoCliente["codGenero"] = resultado.CodGenero;
                    drInfoCliente["codigoSap"] = resultado.CodigoSap;
                    drInfoCliente["codProvincia"] = resultado.CodProvincia;
                    drInfoCliente["codRamo"] = resultado.CodRamo;
                    drInfoCliente["codRamo1"] = resultado.CodRamo1;
                    drInfoCliente["codTratamiento"] = resultado.CodTratamiento;
                    drInfoCliente["correo"] = resultado.Correo;
                    drInfoCliente["deuda"] = resultado.Deuda;
                    drInfoCliente["distrito"] = resultado.Distrito;
                    drInfoCliente["fecNacimiento"] = resultado.FecNacimiento;
                    drInfoCliente["grpCtaCliente"] = resultado.GrpCtaCliente;
                    drInfoCliente["grupoCliente"] = resultado.GrupoCliente;
                    drInfoCliente["limiteCredito"] = resultado.LimiteCredito;
                    drInfoCliente["nombres"] = resultado.Nombres;
                    drInfoCliente["pais"] = resultado.Pais;
                    drInfoCliente["poblacion"] = resultado.Poblacion;
                    drInfoCliente["saldoCliente"] = resultado.SaldoCliente;
                    drInfoCliente["telefono1"] = resultado.Telefono1;
                    drInfoCliente["telefono2"] = resultado.Telefono2;
                    drInfoCliente["textoActEconomica"] = resultado.TextoActEconomica;
                    drInfoCliente["textoDistrito"] = resultado.TextoDistrito;
                    drInfoCliente["textoEstadoCivil"] = resultado.TextoEstadoCivil;
                    drInfoCliente["textoGenero"] = resultado.TextoGenero;
                    drInfoCliente["textoProvincia"] = resultado.TextoProvincia;
                    drInfoCliente["textoRamo"] = resultado.TextoRamo;
                    drInfoCliente["textoRamo1"] = resultado.TextoRamo1;
                    drInfoCliente["textoTratamiento"] = resultado.TextoTratamiento;
                    drInfoCliente["tipoIdentificacion"] = resultado.TipoIdentificacion;

                    dtInfoCliente.Rows.Add(drInfoCliente);
                   
                } catch (Exception erra) {
                    error = erra.Message;
                }
            }
                          
            return dsCliente;
        }

        #endregion
    
    }
}