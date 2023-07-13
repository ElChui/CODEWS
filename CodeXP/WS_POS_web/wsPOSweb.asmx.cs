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

        bool bReto = false;

        clCapaDatos confact = new clCapaDatos();
        String Conexion = ConfigurationManager.AppSettings["conexion"];
        String conexionPOS = ConfigurationManager.AppSettings["conexionPOS"];
        String conexionPOSWeb = ConfigurationManager.AppSettings["conexionPOSWeb"];
        String conexionPINPAD = ConfigurationManager.AppSettings["conexionPINPAD"];
        String conexionBIN = ConfigurationManager.AppSettings["conexionBIN"];
        String UsuarioSap = ConfigurationManager.AppSettings["UsuarioSap"];//"AJE_WS";
        String ContraSap = ConfigurationManager.AppSettings["ContreSap"];//"W19+25s.";
        String IVA = ConfigurationManager.AppSettings["IVA"];       

        String varChui = "";

        bool esCorrecto = false;
        int contE = 0;
        int contRepetir = 10;
        string solicitudCancelada = "Anulada la solicitud: La solicitud fue cancelada";

        static bool esProduccion = ConfigurationManager.AppSettings["esProduccion"] == "S" ? true : false;

        #region << SEBAS >>
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
                            String identificacion = ds.Tables[0].Rows[0]["identificacion"].ToString();
                            if (produccion) // SI ES PRODUCCION
                            {
                                Ws_obt_vend_PRD.ZSDWS_POS_CONSULTA_VENDEDORES vend = new Ws_obt_vend_PRD.ZSDWS_POS_CONSULTA_VENDEDORES();
                                vend.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                                //Consulta si existe código de vendedor con este número de cédula
                                String[] vendget = vend.ZsdrfcPosConsultaVendedores(identificacion).Split('|');

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
                            }
                            else // SI ES CALIDAD
                            {
                                Ws_obt_vend.ZSDWS_POS_CONSULTA_VENDEDORES vend = new Ws_obt_vend.ZSDWS_POS_CONSULTA_VENDEDORES();
                                vend.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                                //Consulta si existe código de vendedor con este número de cédula
                                String[] vendget = vend.ZsdrfcPosConsultaVendedores(identificacion).Split('|');

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

        [WebMethod]
        public DataTable getBancos()
        {

            DataTable dt = null;
            try
            {
                dt = getSubGroups("BANCO");
            }
            catch (Exception e)
            {
            }
            return dt;
        }

        [WebMethod]
        public DataTable getBancosTarjeta()
        {

            DataTable dt = null;
            try
            {
                dt = getSubGroups("TARJETA CREDITO");
            }
            catch (Exception e)
            {
            }
            return dt;
        }

        [WebMethod] // Devuelve los tipos diferido y plazos para el metodo de pago PINPAD
        public DataTable getParametrosPINPAD(String Tipo)
        {
            DataTable dt = new DataTable("ParametrosPINPAD");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("Valor");
            List<SqlParameter> par = new List<SqlParameter>() {
                new SqlParameter() {ParameterName = "@pCodigo",Value= Tipo}
            };
            DataSet ds = confact.ejecutarSP(conexionPINPAD, "SP_GET_PARAMETROS", par);
            dt = ds.Tables[0];
            dt.Columns["param_descripcion"].ColumnName = "Descrip";
            dt.Columns["param_valor1"].ColumnName = "Valor";
            return dt;
        }

        [WebMethod]
        public DataTable getBinTarjetas(String bancoTarjeta)
        {

            DataTable dt = null;
            try
            {
                dt = getSubGroups(bancoTarjeta);
            }
            catch (Exception e)
            {
            }
            return dt;
        }

        [WebMethod]
        public DataTable getSubGroups(String subgrupo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
                            {
                                 new SqlParameter() {ParameterName = "@pGruIdentificacion ",Value= subgrupo},
                             };

            DataSet ds = null;
            DataTable dt = null;
            try
            {
                ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Get_SubGrupo", parameters);
                dt = ds.Tables[0];
            }
            catch (Exception e)
            {
            }
            return dt;
        }

        [WebMethod]
        public String verificarCuentaCREA(String Cedula, String Valor, String Refernecia, String Dispositivo, String IpDispositivo, String CodigoUsuario, String Almacen)
        {
            Valor = Valor.Replace(".", ",");
            Decimal Valor2 = Convert.ToDecimal(Valor);

            if (produccion) // SI ES PRODUCCION
            {
                Ws_ValidateCreaAccount_PRD.MetodosBilletera validacion = new Ws_ValidateCreaAccount_PRD.MetodosBilletera();
                String[] datos = validacion.ValidaSocio(Cedula, Valor2, Refernecia, Dispositivo, "Tablet", IpDispositivo, CodigoUsuario, Almacen);
                if (datos[0].Equals("0"))
                {
                    return "0" + ";" + "Correcto";
                }
                else
                {
                    return "4" + ";" + "Incorrecto";
                }
            }
            else // SI ES CALIDAD
            {
                // Se agrega el WS de verificacion
                Ws_ValidateCreaAccount_PRD.MetodosBilletera validacion = new Ws_ValidateCreaAccount_PRD.MetodosBilletera();
                String[] datos = validacion.ValidaSocio(Cedula, Valor2, Refernecia, Dispositivo, "Tablet", IpDispositivo, CodigoUsuario, Almacen);
                if (datos[0].Equals("0"))
                {
                    return "0" + ";" + "Correcto";
                }
                else
                {
                    return "4" + ";" + "Incorrecto";
                }

            }

        }

        [WebMethod]
        public String validarPINCREA(String Cedula, String PIN, String Valor, String sReferencia, String sCodigoUsuario, String Almacen)
        {

            int NPIN = Convert.ToInt32(PIN);
            if (produccion) // SI ES PRODUCCION
            {
                // Se agrega el WS de verificacion
                Ws_ValidateCreaAccount_PRD.MetodosBilletera validacionPIN = new Ws_ValidateCreaAccount_PRD.MetodosBilletera();
                String[] datos = validacionPIN.ValidaPing(sReferencia, NPIN, "Tablet", sCodigoUsuario, Almacen);
                if (datos[0].Equals("0"))
                {
                    //EL WS de BotonCREA devuelve un string[], string[0]= 0 correcto, 4 error, string[1]= Error incorrecto, Número de Cuenta, Numero de Deposito, Fecha de Deposito, Código del banco separado por ";" 
                    return "0" + ";" + datos[1];
                }
                else
                {
                    return "4" + ";" + "Incorrecto";
                }
            }
            else // SI ES CALIDAD
            {
                // Se agrega el WS de verificacion
                Ws_ValidateCreaAccount_PRD.MetodosBilletera validacionPIN = new Ws_ValidateCreaAccount_PRD.MetodosBilletera();
                String[] datos = validacionPIN.ValidaPing(sReferencia, NPIN, "Tablet", sCodigoUsuario, Almacen);
                if (datos[0].Equals("0"))
                {
                    //EL WS de BotonCREA devuelve un string[], string[0]= 0 correcto, 4 error, string[1]= Error incorrecto, Número de Cuenta, Numero de Deposito, Fecha de Deposito, Código del banco separado por ";" 
                    return "0" + ";" + datos[1];
                }
                else
                {
                    return "4" + ";" + "Incorrecto";
                }

            }

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

        #region << ANTICIPOS >>

        [WebMethod]
        public DataTable getAnticipos(String CedCliente, String sociedad, out String respuesta)
        {
            DataTable dt = new DataTable();
            dt.TableName = "anticipos";
            dt.Columns.Add("cliente");
            dt.Columns.Add("documento");
            dt.Columns.Add("monto");


            if (produccion) // SI ES PRODUCCION
            {
                Ws_Anticipos_PRD.ZWS_GET_ANTICIPO_CLIENTE anticipo = new Ws_Anticipos_PRD.ZWS_GET_ANTICIPO_CLIENTE();
                anticipo.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Anticipos_PRD.ZfiDocAnticipoCliente[] antRecibidos;

                respuesta = anticipo.ZsdrfcGetAnticipoClientes(sociedad, CedCliente, out antRecibidos);

                for (int i = 0; i < antRecibidos.Count(); i++)
                {
                    DataRow fila = dt.NewRow();
                    fila["cliente"] = antRecibidos[i].Kunnr;
                    fila["documento"] = antRecibidos[i].Belnr;
                    fila["monto"] = antRecibidos[i].Monto;
                    dt.Rows.Add(fila);
                }
            }
            else // SI ES CALIDAD
            {
                Ws_Anticipos_PRD.ZWS_GET_ANTICIPO_CLIENTE anticipo = new Ws_Anticipos_PRD.ZWS_GET_ANTICIPO_CLIENTE();
                anticipo.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Anticipos_PRD.ZfiDocAnticipoCliente[] antRecibidos;

                respuesta = anticipo.ZsdrfcGetAnticipoClientes(sociedad, CedCliente, out antRecibidos);

                for (int i = 0; i < antRecibidos.Count(); i++)
                {
                    DataRow fila = dt.NewRow();
                    fila["cliente"] = antRecibidos[i].Kunnr;
                    fila["documento"] = antRecibidos[i].Belnr;
                    fila["monto"] = antRecibidos[i].Monto;
                    dt.Rows.Add(fila);
                }
            }

            return dt;
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
        public DataTable actValoresFactura(String cliente, String sector, String ofVent, String canal, String condicionPago, String sociedad, String items, String docuVentas)
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

            DataTable dt = new DataTable();

            dt.Columns.Add("Respuesta");
            dt.Columns.Add("Posicion");
            dt.Columns.Add("Material");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("PrecioBruto");
            dt.Columns.Add("Descuento");
            dt.Columns.Add("PorcDesc");
            dt.Columns.Add("PrecioNeto");
            dt.Columns.Add("Impuesto");
            dt.Columns.Add("DescManual");
            dt.Columns.Add("PrecioNetoFinal");

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
                        DataRow fila = dt.NewRow();

                        fila["Respuesta"] = cadenaRespuesta;
                        fila["Posicion"] = tablaRespuesta[i].Posicion;
                        fila["Material"] = tablaRespuesta[i].Material;
                        fila["Cantidad"] = tablaRespuesta[i].Cantidad;
                        fila["PrecioBruto"] = tablaRespuesta[i].PrecioBruto;
                        fila["Descuento"] = tablaRespuesta[i].Descuento;
                        fila["PorcDesc"] = tablaRespuesta[i].PorcDesc;
                        fila["PrecioNeto"] = tablaRespuesta[i].PrecioNeto;
                        fila["Impuesto"] = tablaRespuesta[i].Impuesto;
                        fila["DescManual"] = tablaRespuesta[i].DescManual;
                        fila["PrecioNetoFinal"] = tablaRespuesta[i].PrecioNetoFinal;

                        dt.Rows.Add(fila);
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

                String data = "ZsdrfcPrecioDescFinancia(IAuart: " + docuVentas + ", IBukrs: " + sociedad + ", IInco1: " + promoEmpleado + ", IItemin: " + JsonConvert.SerializeObject(itemsIn) + ", IKunnr: " + cliente + ", ISpart: " + sector + ", IVkbur: " + ofVent + ", IVkorg: " + sociedad + ", IVtweg: " + canal + ", IZterm: " + condicionPago + ", out string OReturn)";
                tablaRespuesta = finan.ZsdrfcPrecioDescFinancia(docuVentas, sociedad, promoEmpleado, itemsIn, cliente, sector, ofVent, sociedad, canal, condicionPago, out respuesta);

                cadenaRespuesta = respuesta;

                if (tablaRespuesta != null)
                {
                    for (int i = 0; i < tablaRespuesta.Length; i++)
                    {
                        DataRow fila = dt.NewRow();

                        fila["Respuesta"] = cadenaRespuesta;
                        fila["Posicion"] = tablaRespuesta[i].Posicion;
                        fila["Material"] = tablaRespuesta[i].Material;
                        fila["Cantidad"] = tablaRespuesta[i].Cantidad;
                        fila["PrecioBruto"] = tablaRespuesta[i].PrecioBruto;
                        fila["Descuento"] = tablaRespuesta[i].Descuento;
                        fila["PorcDesc"] = tablaRespuesta[i].PorcDesc;
                        fila["PrecioNeto"] = tablaRespuesta[i].PrecioNeto;
                        fila["Impuesto"] = tablaRespuesta[i].Impuesto;
                        fila["DescManual"] = tablaRespuesta[i].DescManual;
                        fila["PrecioNetoFinal"] = tablaRespuesta[i].PrecioNetoFinal;

                        dt.Rows.Add(fila);
                    }
                }
            }

            return dt;
        }

        [WebMethod]
        public string[] actualizarPreciosDescuentos(int opcion, string area, string esquema, string centro, string grupoArticulo, string codigoArticulo, string codigoSector, string tipoNegociacion, string ramoCliente, string idCliente, string jerarquia, bool tieneYmhPlus, string pagoConTarjeta)
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
            dt.Columns.Add("iva");
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
                    String iva = "0.00";
                    if (oReto[13] == "1" || oReto[13] == "3")
                    {
                        iva = IVA.ToString();
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
                    fila["iva"] = iva;
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

                    String iva = "0.00";
                    if (oReto[13] == "1" || oReto[13] == "3")
                    {
                        iva = IVA.ToString();
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


                try
                {
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
                catch (Exception e)
                {
                    respuesta = "4" + ";no se determino el precio";

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
            return resp + ";" + price;
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
                            guardarLogs("WS_POS_web", "Crear Cliente", "wsPOSweb.asmx.cs", "setClientesap", "", resp, "error SAP", dataClient, cajero, ofvent, "");
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
                catch (Exception e1)
                {
                    guardarLogs("WS_POS_web", "Crear Cliente", "wsPOSweb.asmx.cs", "setClientesap", "", resp, "error SAP", dataClient, cajero, ofvent, "");
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
                            guardarLogs("WS_POS_web", "Crear Cliente", "wsPOSweb.asmx.cs", "setClientesap", "", resp, "error SAP", dataClient, cajero, ofvent, "");
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
                        guardarLogs("WS_POS_web", "Crear Cliente", "wsPOSweb.asmx.cs", "setClientesap", "", resp, "error SAP", dataClient, cajero, ofvent, "");
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
            String[] cliente = new string[27];
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
        public DataSet SapClienteNombre(String ClienteNom) // TODO: MEJORAR MÉTODO COMO EN FACTURACION POS
        {
            if (produccion) // SI ES PRODUCCIÓN
            {
                DataSet ds = new DataSet("ds");
                DataTable dtTablaClientes = new DataTable("tablaClientes");
                dtTablaClientes.Columns.Add("cedula");
                dtTablaClientes.Columns.Add("codigoSAP");
                dtTablaClientes.Columns.Add("nombre1");
                dtTablaClientes.Columns.Add("nombre2");
                dtTablaClientes.Columns.Add("direccion");
                dtTablaClientes.Columns.Add("mail");
                dtTablaClientes.Columns.Add("porcDesc");
                dtTablaClientes.Columns.Add("telefono1");
                dtTablaClientes.Columns.Add("telefono2");

                Ws_Get_Clientes_Nombre_PRD.ZDS_BUSCAR_CLIE_NOM servi2 = new Ws_Get_Clientes_Nombre_PRD.ZDS_BUSCAR_CLIE_NOM();
                servi2.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Get_Clientes_Nombre_PRD.ZestResultadoClientes[] respuesta = servi2.ZsdrfcConClieNom(ClienteNom);


                for (int i = 0; i < respuesta.Length; i++)
                {
                    DataRow fila = dtTablaClientes.NewRow();
                    fila["cedula"] = respuesta[i].Cedula;
                    fila["codigoSAP"] = "";
                    fila["nombre1"] = respuesta[i].Nombre;
                    fila["nombre2"] = " ";
                    fila["direccion"] = "";
                    fila["mail"] = "";
                    fila["porcDesc"] = "";
                    fila["telefono1"] = "";
                    fila["telefono2"] = "";
                    dtTablaClientes.Rows.Add(fila);
                }
                ds.Tables.Add(dtTablaClientes);
                return ds;

            }
            else // SI ES CALIDAD
            {
                DataSet ds = new DataSet("ds");
                DataTable dtTablaClientes = new DataTable("tablaClientes");
                dtTablaClientes.Columns.Add("cedula");
                dtTablaClientes.Columns.Add("codigoSAP");
                dtTablaClientes.Columns.Add("nombre1");
                dtTablaClientes.Columns.Add("nombre2");
                dtTablaClientes.Columns.Add("direccion");
                dtTablaClientes.Columns.Add("mail");
                dtTablaClientes.Columns.Add("porcDesc");
                dtTablaClientes.Columns.Add("telefono1");
                dtTablaClientes.Columns.Add("telefono2");

                Ws_Get_Clientes_Nombre_PRD.ZDS_BUSCAR_CLIE_NOM servi2 = new Ws_Get_Clientes_Nombre_PRD.ZDS_BUSCAR_CLIE_NOM();
                servi2.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Get_Clientes_Nombre_PRD.ZestResultadoClientes[] respuesta = servi2.ZsdrfcConClieNom(ClienteNom);


                for (int i = 0; i < respuesta.Length; i++)
                {
                    DataRow fila = dtTablaClientes.NewRow();
                    fila["cedula"] = respuesta[i].Cedula;
                    fila["codigoSAP"] = "";
                    fila["nombre1"] = respuesta[i].Nombre;
                    fila["nombre2"] = " ";
                    fila["direccion"] = "";
                    fila["mail"] = "";
                    fila["porcDesc"] = "";
                    fila["telefono1"] = "";
                    fila["telefono2"] = "";
                    dtTablaClientes.Rows.Add(fila);
                }
                ds.Tables.Add(dtTablaClientes);
                return ds;
            }

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

        [WebMethod]
        public string[] GetVend(string esProduccion, string pIden)
        {
            string[] sReto = new string[2];

            if (esProduccion == "S")
            { //Cuando es PRODUCCION                
                if (pIden == "0190007510001")
                { //Ruc de almacenes JUAN ELJURI (Cuando no comisiona nadie)
                    sReto[0] = "0";
                    sReto[1] = "1000000";
                }
                else
                {
                    Ws_obt_vend_PRD.ZSDWS_POS_CONSULTA_VENDEDORES serv = new Ws_obt_vend_PRD.ZSDWS_POS_CONSULTA_VENDEDORES();
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
                                sReto = serv.ZsdrfcPosConsultaVendedores(pIden).Split('|');

                                if (sReto[0] == solicitudCancelada)
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
                        sReto[0] = erra.Message;
                        bReto = false;
                    }
                }

                return sReto;
            }
            else
            { //Cuando es CALIDAD
                if (pIden == "0190007510001")
                { //Ruc de almacenes JUAN ELJURI (Cuando no comisiona nadie)              
                    sReto[0] = "0";
                    sReto[1] = "1000000";
                }
                else
                {

                    Ws_obt_vend.ZSDWS_POS_CONSULTA_VENDEDORES serv = new Ws_obt_vend.ZSDWS_POS_CONSULTA_VENDEDORES();
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
                                sReto = serv.ZsdrfcPosConsultaVendedores(pIden).Split('|');

                                if (sReto[0] == solicitudCancelada)
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
                        sReto[0] = erra.Message;
                        bReto = false;
                    }
                }

                return sReto;
            }
        }

        #endregion

        #region << FACTURACION >>

        [WebMethod]
        public DataSet calculateTotals(String itemStr, String sociedad, String condicionPago, String entrada, String amortizacion)
        {
            DataTable dtT = new DataTable("totals");
            DataTable dtI = new DataTable("items");
            DataSet ds = new DataSet("dsTotals");
            String preciototalFac = "0.00";
            String iceTotalFac = "0.00";
            String ivaTotalFac = "0.00";
            String totalFac = "0.00";
            String subtotalFac = "0.00";
            String descuentoFac = "0.00";
            JArray itemsArray = (JArray)JsonConvert.DeserializeObject(itemStr);
            /*Ws_Totales_Factura.ZestfiItems[] updatedItems = null;
            if (produccion) // SI ES PRODUCCION
            {
                Ws_Totales_Factura.ZWS_ITEMS_VALORES_FACTURAService totalesFact = new Ws_Totales_Factura.ZWS_ITEMS_VALORES_FACTURAService();
                totalesFact.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Totales_Factura.ZestfiMaterialPrecio[] items = new Ws_Totales_Factura.ZestfiMaterialPrecio[itemsArray.Count];
                for (int cont = 0; cont < itemsArray.Count; cont++)
                {
                    Ws_Totales_Factura.ZestfiMaterialPrecio item = new Ws_Totales_Factura.ZestfiMaterialPrecio();

                    JObject itemsarr = (JObject)itemsArray[cont];

                    item.Material = itemsarr.SelectToken("codigo").Value<string>();
                    item.Cantidad = itemsarr.SelectToken("cantidad").Value<string>();
                    item.Valor = itemsarr.SelectToken("precioNeto").Value<string>();
                    item.Descuento = itemsarr.SelectToken("descuentoAplicado").Value<string>();

                    items[cont] = item;
                }

                String dataTotal = "totalesFact.ZfiDescuentoFinanciamiento(IAmortizacion: " + amortizacion + ", ICondPago: " + condicionPago + ", IEntrada: " + entrada + ", Iitems: " + JsonConvert.SerializeObject(items) + ", ISociedad: " + sociedad + ")";

                updatedItems = totalesFact.ZfiDescuentoFinanciamiento(amortizacion, condicionPago, entrada, items, sociedad, out iceTotalFac, out ivaTotalFac, out preciototalFac, out subtotalFac, out descuentoFac, out totalFac);

            }
            else   //ES CALIDAD
            {
                Ws_Totales_Factura.ZWS_ITEMS_VALORES_FACTURAService totalesFact = new Ws_Totales_Factura.ZWS_ITEMS_VALORES_FACTURAService();
                totalesFact.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Totales_Factura.ZestfiMaterialPrecio[] items = new Ws_Totales_Factura.ZestfiMaterialPrecio[itemsArray.Count];
                for (int cont = 0; cont < itemsArray.Count; cont++)
                {
                    Ws_Totales_Factura.ZestfiMaterialPrecio item = new Ws_Totales_Factura.ZestfiMaterialPrecio();

                    JObject itemsarr = (JObject)itemsArray[cont];

                    item.Material = itemsarr.SelectToken("codigo").Value<string>();
                    item.Cantidad = itemsarr.SelectToken("cantidad").Value<string>();
                    item.Valor = itemsarr.SelectToken("precioNeto").Value<string>();
                    item.Descuento = itemsarr.SelectToken("descuentoAplicado").Value<string>();

                    items[cont] = item;
                }

                String dataTotal = "totalesFact.ZfiDescuentoFinanciamiento(IAmortizacion: " + amortizacion + ", ICondPago: " + condicionPago + ", IEntrada: " + entrada + ", Iitems: " + JsonConvert.SerializeObject(items) + ", ISociedad: " + sociedad + ")";

                updatedItems = totalesFact.ZfiDescuentoFinanciamiento(amortizacion, condicionPago, entrada, items, sociedad, out iceTotalFac, out ivaTotalFac, out preciototalFac, out subtotalFac, out descuentoFac, out totalFac);
            }
            

            dtT.Columns.Add("totalFac");
            dtT.Columns.Add("precioFac");
            dtT.Columns.Add("subtotalFac");
            dtT.Columns.Add("ivaTotalFac");
            dtT.Columns.Add("iceTotalFac");
            dtT.Columns.Add("descuentoFac");

            DataRow fila = dtT.NewRow();

            fila["totalFac"] = totalFac != "" ? totalFac : "0.00";
            fila["precioFac"] = preciototalFac != "" ? preciototalFac : "0.00";
            fila["subtotalFac"] = subtotalFac != "" ? subtotalFac : "0.00";
            fila["ivaTotalFac"] = ivaTotalFac != "" ? ivaTotalFac : "0.00";
            fila["iceTotalFac"] = iceTotalFac != "" ? iceTotalFac : "0.00";
            fila["descuentoFac"] = descuentoFac != "" ? descuentoFac : "0.00";

            dtT.Rows.Add(fila);

            dtI.Columns.Add("material");
            dtI.Columns.Add("cantidad");
            dtI.Columns.Add("precio");
            dtI.Columns.Add("descuento");
            dtI.Columns.Add("valor_descuento");
            dtI.Columns.Add("subtotal");
            dtI.Columns.Add("iva");
            dtI.Columns.Add("ice");
            dtI.Columns.Add("tasa_financiamiento");
            dtI.Columns.Add("total");

            for (int cont = 0; cont < updatedItems.Count(); cont++)
            {
                DataRow filaI = dtI.NewRow();
                filaI["material"] = updatedItems[cont].Material;
                filaI["cantidad"] = updatedItems[cont].Cantidad;
                filaI["precio"] = updatedItems[cont].Precio;
                filaI["descuento"] = updatedItems[cont].Descuento;
                filaI["valor_descuento"] = updatedItems[cont].ValorDescuento;
                filaI["subtotal"] = updatedItems[cont].Subtotal;
                filaI["iva"] = updatedItems[cont].Iva;
                filaI["ice"] = updatedItems[cont].Ice;
                filaI["tasa_financiamiento"] = updatedItems[cont].TasaFinanciamiento;
                filaI["total"] = updatedItems[cont].Total;

                dtI.Rows.Add(filaI);
            }
            */
            ds.Tables.Add(dtT);
            ds.Tables.Add(dtI);

            return ds;
        }

        [WebMethod]
        public String setSecuencialCaja(String numEstablecimiento, String numPuntoEmision, String tipoDoc, String OfiVent)
        {
            int resp = 1;
            String respuesta = "";
            if (produccion) // SI ES PRODUCCION
            {
                /*
                Ws_Difinir_Secuencial.ZWS_CREA_SECUENCIAL_OFIC_VENTAService setNewSec = new Ws_Difinir_Secuencial.ZWS_CREA_SECUENCIAL_OFIC_VENTAService();
                setNewSec.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                respuesta = setNewSec.ZfiCrearSecOficinaVenta(numEstablecimiento, numPuntoEmision, tipoDoc, OfiVent, out resp);
                */
            }
            else   //ES CALIDAD
            {
                /*
                Ws_Definir_Secuencial.ZWS_CREA_SECUENCIAL_OFIC_VENTAService setNewSec = new Ws_Definir_Secuencial.ZWS_CREA_SECUENCIAL_OFIC_VENTAService();
                setNewSec.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                respuesta = setNewSec.ZfiCrearSecOficinaVenta(numEstablecimiento, numPuntoEmision, tipoDoc, OfiVent, out resp);
                */
            }
            return resp + ";" + respuesta;
        }

        [WebMethod]
        public String getSecuencial(String numEstablecimiento, String numPuntoEmision, String tipoDoc, String OfiVent)
        {
            int resp = 1;
            String respuesta = "";
            String secuencial = "";
            if (produccion) // SI ES PRODUCCION
            {
                //    Ws_Get_Secuencial.ZWS_GET_SECUENCIAL_OFIC_VENTAService getSecuencial = new Ws_Get_Secuencial.ZWS_GET_SECUENCIAL_OFIC_VENTAService();
                //    getSecuencial.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                //    respuesta = getSecuencial.ZfiGetsecVentasAjeweb(numEstablecimiento, numPuntoEmision, tipoDoc, OfiVent, out secuencial, out resp);
            }
            else   //ES CALIDAD
            {
                /*
                Ws_Get_Secuencial.ZWS_GET_SECUENCIAL_OFIC_VENTAService getSecuencial = new Ws_Get_Secuencial.ZWS_GET_SECUENCIAL_OFIC_VENTAService();
                getSecuencial.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                respuesta = getSecuencial.ZfiGetsecVentasAjeweb(numEstablecimiento, numPuntoEmision, tipoDoc, OfiVent, out secuencial, out resp);
                */
            }
            return resp + ";" + secuencial + ";" + respuesta;
        }

        [WebMethod]
        public DataSet setFactura(String cabecera, String detalles, String formasPago, String claseDoc, String usuario)
        {

            JObject facturaCabecera = (JObject)JsonConvert.DeserializeObject(cabecera);
            JArray facturaDetalles = (JArray)JsonConvert.DeserializeObject(detalles);
            JArray facturaFormaPago = (JArray)JsonConvert.DeserializeObject(formasPago);
            JObject facturaUsuario = (JObject)JsonConvert.DeserializeObject(usuario);


            String secuencialresp = getSecuencial(facturaCabecera.SelectToken("caja").SelectToken("ofiEstablecimientoSRI").Value<string>(), facturaCabecera.SelectToken("caja").SelectToken("cajPuntoEmisionSRI").Value<string>(), claseDoc, facturaCabecera.SelectToken("caja").SelectToken("idOficinaVenta_VKBUR").Value<string>());
            String clave = "";
            String secuencial = "";
            if (secuencial.Equals("0;;No existe secuencial para la Of.venta R119 Clase doc. M Estab. 2 Pto.emisión 2"))
            {
                secuencial = "";
            }
            else
            {
                secuencial = secuencialresp.Split(';')[1];
                String ambiente = "1";
                if (produccion)
                {
                    ambiente = "2";
                }
                else
                {
                    ambiente = "1";
                }
                List<SqlParameter> parametersClave = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@Opcion",Value= "1"},
                new SqlParameter() {ParameterName = "@TipoDoc",Value= claseDoc},
                new SqlParameter() {ParameterName = "@idCaja",Value= facturaCabecera.SelectToken("caja").SelectToken("idCaja").Value<string>()},
                new SqlParameter() {ParameterName = "@Ambiente",Value= ambiente},
                new SqlParameter() {ParameterName = "@secuencial",Value= secuencial}
            };
                DataSet dsClaveAccesso = null;
                String dataClave = "@Opcion: " + "1" + " @TipoDoc: " + claseDoc + " @idCaja: " + facturaCabecera.SelectToken("caja").SelectToken("idCaja").Value<string>() + " @Ambiente: " + ambiente + " @secuencial: " + secuencial;

                try
                {
                    dsClaveAccesso = confact.ejecutarSP(conexionPOSWeb, "sp_PW_CLAVE_ACCESO", parametersClave);
                    clave = dsClaveAccesso.Tables[0].Rows[0][0].ToString();
                }
                catch (Exception e)
                {
                    try
                    {

                        guardarLogs("WS_POS_web", "Crear Factura", "wsPOSweb.asmx.cs", "setFactura", "Calculo Clave Acceso", JsonConvert.SerializeObject(e), "error DB", dataClave, facturaUsuario.SelectToken("idUser").Value<string>(), facturaCabecera.SelectToken("caja").SelectToken("idCaja").Value<string>(), "");
                    }
                    catch (Exception e2)
                    {
                    }
                }

            }

            DataTable dtCab = new DataTable("cabecera");
            DataTable dtDet = new DataTable("detalles");
            DataTable dtFpago = new DataTable("fpagos");

            DataSet dsCab = new DataSet("dsCabecera");
            DataSet dsDet = new DataSet("dsDetalles");
            DataSet dsFPago = new DataSet("dsFormasPago");


            dtCab.Columns.Add("docClaseDocumento_VBTYP_BLART");
            dtCab.Columns.Add("docCabEstado");
            dtCab.Columns.Add("docCabObservacion");
            dtCab.Columns.Add("docCabObservacion2");
            dtCab.Columns.Add("docCabTipoNegociacion");
            dtCab.Columns.Add("docCabVendedor");
            dtCab.Columns.Add("docCabSecuencialSRI_XBLNR");
            dtCab.Columns.Add("docCabAutorizacionSRI");
            dtCab.Columns.Add("docCabAmbienteSRI");
            dtCab.Columns.Add("docCabFechaSRI");
            dtCab.Columns.Add("docCabMensajeSRI");
            dtCab.Columns.Add("docCabEstadoSRI");
            dtCab.Columns.Add("docCabXMLSRI");
            dtCab.Columns.Add("docCabPathRIDE");
            dtCab.Columns.Add("docCabCodEstadoSRI");
            dtCab.Columns.Add("pedidoSAP_VBELN_VA");
            dtCab.Columns.Add("entregaSAP__VBELN_VL");
            dtCab.Columns.Add("documentoSAP__VBELN_VF");
            dtCab.Columns.Add("documentoContableSAP_BELNR");
            dtCab.Columns.Add("recaudoSAP_AUGBL");
            dtCab.Columns.Add("numeroReciboSAP");
            dtCab.Columns.Add("docCabSubtotal12");
            dtCab.Columns.Add("docCabSubtotal0");
            dtCab.Columns.Add("docCabSubtotalExcentoIVA");
            dtCab.Columns.Add("docCabSubtotalSinImpuestos");
            dtCab.Columns.Add("docCabDescuento");
            dtCab.Columns.Add("docCabICE");
            dtCab.Columns.Add("docCabIVA");
            dtCab.Columns.Add("docCabTotal");
            dtCab.Columns.Add("fechaCr");
            dtCab.Columns.Add("fechaAct");
            dtCab.Columns.Add("usuarioCr");
            dtCab.Columns.Add("usuarioAct");
            dtCab.Columns.Add("idPersona");
            dtCab.Columns.Add("idDocumentosCabeceraReferencia");
            dtCab.Columns.Add("idCaja");
            dtCab.Columns.Add("docCabIP");

            dtDet.Columns.Add("docDetCodigo");
            dtDet.Columns.Add("docDetDescripcion");
            dtDet.Columns.Add("docDetPrecio");
            dtDet.Columns.Add("docDetCantidad");
            dtDet.Columns.Add("movDetPorcentajeDescuento");
            dtDet.Columns.Add("docDetPorcentajeIva");
            dtDet.Columns.Add("docDetExcentoIva");
            dtDet.Columns.Add("docDetValorIce");
            dtDet.Columns.Add("docDetSerieLote");
            dtDet.Columns.Add("docDetTipoSerieLote");
            dtDet.Columns.Add("docDetCentro");
            dtDet.Columns.Add("docDetPosicion");
            dtDet.Columns.Add("docDetValorIva");
            dtDet.Columns.Add("docDetValorDescuento");
            dtDet.Columns.Add("docDetSubtotal");
            dtDet.Columns.Add("docDetTotal");
            dtDet.Columns.Add("fechaCr");
            dtDet.Columns.Add("idDocumentosCabecera");

            dtFpago.Columns.Add("pagValor");
            dtFpago.Columns.Add("pagCodigoSAP");
            dtFpago.Columns.Add("pagNumeroDocumento");
            dtFpago.Columns.Add("pagNumeroCuentaBanco");
            dtFpago.Columns.Add("pagFechaDocumento");
            dtFpago.Columns.Add("pagEsGiftCard");
            dtFpago.Columns.Add("pagMaterialGiftCard");
            dtFpago.Columns.Add("pagSerieGiftCard");
            dtFpago.Columns.Add("pagCorrienteDiferido");
            dtFpago.Columns.Add("pagMesesDiferido");
            dtFpago.Columns.Add("pagLote");
            dtFpago.Columns.Add("pagAutorizacion");
            dtFpago.Columns.Add("pagPropietarioTarjeta");
            dtFpago.Columns.Add("pagTexto");
            dtFpago.Columns.Add("fechaCr");
            dtFpago.Columns.Add("idDocumentosCabecera");
            dtFpago.Columns.Add("idGruSubFormaPago");
            dtFpago.Columns.Add("idGruSubCodigoBin");
            dtFpago.Columns.Add("idGruSubBanco");
            dtFpago.Columns.Add("idGruSubTipoTarjeta");
            dtFpago.Columns.Add("pagEsCuotaInicial");

            DataRow filaCab = dtCab.NewRow();

            filaCab["docClaseDocumento_VBTYP_BLART"] = claseDoc;
            filaCab["docCabEstado"] = "0";
            filaCab["docCabObservacion"] = facturaCabecera.SelectToken("observacion").Value<string>();
            filaCab["docCabObservacion2"] = null;
            filaCab["docCabTipoNegociacion"] = facturaCabecera.SelectToken("condicionPago").Value<string>();
            filaCab["docCabVendedor"] = facturaCabecera.SelectToken("vendedor").SelectToken("codigo").Value<string>();

            filaCab["docCabSecuencialSRI_XBLNR"] = secuencial;
            filaCab["docCabAutorizacionSRI"] = clave;
            filaCab["docCabAmbienteSRI"] = null;
            filaCab["docCabFechaSRI"] = null;
            filaCab["docCabMensajeSRI"] = null;
            filaCab["docCabEstadoSRI"] = null;
            filaCab["docCabXMLSRI"] = null;

            filaCab["docCabPathRIDE"] = null;
            filaCab["docCabCodEstadoSRI"] = null;
            filaCab["pedidoSAP_VBELN_VA"] = null;
            filaCab["entregaSAP__VBELN_VL"] = null;
            filaCab["documentoSAP__VBELN_VF"] = null;
            filaCab["documentoContableSAP_BELNR"] = null;
            filaCab["recaudoSAP_AUGBL"] = null;

            filaCab["numeroReciboSAP"] = null;
            filaCab["docCabSubtotal12"] = null;
            filaCab["docCabSubtotal0"] = null;
            filaCab["docCabSubtotalExcentoIVA"] = null;
            filaCab["docCabSubtotalSinImpuestos"] = facturaCabecera.SelectToken("suma").Value<string>();
            filaCab["docCabDescuento"] = facturaCabecera.SelectToken("descuentoTotal").Value<string>();
            filaCab["docCabICE"] = facturaCabecera.SelectToken("ice").Value<string>();

            filaCab["docCabIVA"] = facturaCabecera.SelectToken("iva").Value<string>();
            filaCab["docCabTotal"] = facturaCabecera.SelectToken("total").Value<string>();
            filaCab["fechaCr"] = null;
            filaCab["fechaAct"] = null;
            filaCab["usuarioCr"] = facturaUsuario.SelectToken("ident").Value<string>(); ;
            filaCab["usuarioAct"] = null;
            filaCab["idPersona"] = null;
            filaCab["idDocumentosCabeceraReferencia"] = null;
            filaCab["idCaja"] = facturaCabecera.SelectToken("caja").SelectToken("idCaja").Value<string>();
            filaCab["docCabIP"] = "";
            dtCab.Rows.Add(filaCab);


            for (int cont = 0; cont < facturaDetalles.Count(); cont++)
            {
                DataRow filaDet = dtDet.NewRow();
                JObject detalle = (JObject)facturaDetalles[cont];
                filaDet["docDetCodigo"] = detalle.SelectToken("codigo").Value<string>();
                filaDet["docDetDescripcion"] = detalle.SelectToken("descripcion").Value<string>();
                filaDet["docDetPrecio"] = detalle.SelectToken("precio").Value<string>();
                filaDet["docDetCantidad"] = detalle.SelectToken("cantidad").Value<string>();
                filaDet["movDetPorcentajeDescuento"] = detalle.SelectToken("descuento_porcentaje").Value<string>();
                filaDet["docDetPorcentajeIva"] = IVA;
                filaDet["docDetExcentoIva"] = "0.00";
                filaDet["docDetValorIce"] = detalle.SelectToken("ice").Value<string>();

                if (detalle.SelectToken("serie").Value<string>().Equals("S"))
                {
                    filaDet["docDetSerieLote"] = detalle.SelectToken("serie").Value<string>();
                }
                else if (detalle.SelectToken("serie").Value<string>().Equals("L"))
                {
                    filaDet["docDetSerieLote"] = detalle.SelectToken("lote").Value<string>();
                }
                else if (detalle.SelectToken("serie").Value<string>().Equals("C"))
                {
                    filaDet["docDetSerieLote"] = detalle.SelectToken("combo").Value<string>();
                }
                else
                {
                    filaDet["docDetSerieLote"] = null;
                }
                filaDet["docDetTipoSerieLote"] = detalle.SelectToken("tipoMaterial").Value<string>();
                filaDet["docDetCentro"] = detalle.SelectToken("centro").Value<string>();
                filaDet["docDetPosicion"] = cont.ToString();
                filaDet["docDetValorIva"] = detalle.SelectToken("iva").Value<string>();
                filaDet["docDetValorDescuento"] = detalle.SelectToken("descuento_valor").Value<string>();
                filaDet["docDetSubtotal"] = detalle.SelectToken("subtotal").Value<string>();
                filaDet["docDetTotal"] = detalle.SelectToken("total").Value<string>();
                filaDet["fechaCr"] = null;
                filaDet["idDocumentosCabecera"] = null;

                dtDet.Rows.Add(filaDet);
            }


            for (int cont = 0; cont < facturaFormaPago.Count(); cont++)
            {
                JObject fpago = (JObject)facturaFormaPago[cont];
                DataRow filaFpago = dtFpago.NewRow();
                filaFpago["pagValor"] = fpago.SelectToken("pagValor").Value<string>();
                filaFpago["pagCodigoSAP"] = fpago.SelectToken("pagCodigoSAP").Value<string>();
                filaFpago["pagNumeroDocumento"] = fpago.SelectToken("pagNumeroDocumento").Value<string>();
                filaFpago["pagNumeroCuentaBanco"] = fpago.SelectToken("pagNumeroCuentaBanco").Value<string>();
                filaFpago["pagFechaDocumento"] = fpago.SelectToken("pagFechaDocumento").Value<string>();
                filaFpago["pagEsGiftCard"] = fpago.SelectToken("pagEsGiftCard").Value<string>();
                filaFpago["pagMaterialGiftCard"] = fpago.SelectToken("pagMaterialGiftCard").Value<string>();
                filaFpago["pagSerieGiftCard"] = fpago.SelectToken("pagSerieGiftCard").Value<string>();
                filaFpago["pagCorrienteDiferido"] = fpago.SelectToken("pagCorrienteDiferido").Value<string>();
                filaFpago["pagMesesDiferido"] = fpago.SelectToken("pagMesesDiferido").Value<string>();
                filaFpago["pagLote"] = fpago.SelectToken("pagLote").Value<string>();
                filaFpago["pagAutorizacion"] = fpago.SelectToken("pagAutorizacion").Value<string>();
                filaFpago["pagPropietarioTarjeta"] = fpago.SelectToken("pagPropietarioTarjeta").Value<string>();
                filaFpago["pagTexto"] = null;
                filaFpago["fechaCr"] = null;
                filaFpago["idDocumentosCabecera"] = null;
                filaFpago["idGruSubFormaPago"] = fpago.SelectToken("idGruSubFormaPago").Value<string>();
                filaFpago["idGruSubCodigoBin"] = null;
                filaFpago["idGruSubTipoTarjeta"] = fpago.SelectToken("idGruSubTipoTarjeta") != null ? fpago.SelectToken("idGruSubTipoTarjeta").Value<string>() : null;
                filaFpago["idGruSubBanco"] = fpago.SelectToken("idGruSubBanco") != null ? fpago.SelectToken("idGruSubBanco").Value<string>() : null;
                filaFpago["pagEsCuotaInicial"] = "";

                dtFpago.Rows.Add(filaFpago);
            }

            DataSet ds = null;
            String veridicador = "";

            veridicador = "1";
            String dataFact =
                    "@pCabecera: " + JsonConvert.SerializeObject(dtCab) + "\n" +
                    "@pDetalle: " + JsonConvert.SerializeObject(dtDet) + "\n" +
                    "@pPago: " + JsonConvert.SerializeObject(dtFpago) + "\n" +
                    "@pCliente: " + facturaCabecera.SelectToken("cliente").SelectToken("ide").Value<string>() + "\n" +
                    "@pIdCaja: " + facturaCabecera.SelectToken("caja").SelectToken("idCaja").Value<string>() + "\n" +
                    "@pusuario: " + facturaUsuario.SelectToken("idUser").Value<string>() + "\n";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@pCabecera",Value= dtCab},
                new SqlParameter() {ParameterName = "@pDetalle",Value= dtDet},
                new SqlParameter() {ParameterName = "@pPago",Value= dtFpago},
                new SqlParameter() {ParameterName = "@pCliente",Value= facturaCabecera.SelectToken("cliente").SelectToken("ide").Value<string>()},
                new SqlParameter() {ParameterName = "@pIdCaja",Value= facturaCabecera.SelectToken("caja").SelectToken("idCaja").Value<string>()},
                new SqlParameter() {ParameterName = "@pusuario",Value= facturaUsuario.SelectToken("idUser").Value<string>()},

                new SqlParameter() {ParameterName = "@valorEntrada",Value= facturaCabecera.SelectToken("entrada").Value<string>()},
                new SqlParameter() {ParameterName = "@diasGracia",Value= facturaCabecera.SelectToken("diasGracia").Value<string>()},
                new SqlParameter() {ParameterName = "@valorMatricula",Value= facturaCabecera.SelectToken("valorMatricula").Value<string>()},
                new SqlParameter() {ParameterName = "@seguroYamaha",Value= facturaCabecera.SelectToken("seguroYamaha").Value<string>()},
                new SqlParameter() {ParameterName = "@ordenTrabajo",Value= facturaCabecera.SelectToken("ordenTrabajo").Value<string>()},
                new SqlParameter() {ParameterName = "@tarjetaCoorporativa",Value= facturaCabecera.SelectToken("tarjetaCoorporativa").Value<string>()},
                new SqlParameter() {ParameterName = "@numeroTraslado",Value= facturaCabecera.SelectToken("numeroTraslado").Value<string>()},
                new SqlParameter() {ParameterName = "@generaMillas",Value= facturaCabecera.SelectToken("generaMillas").Value<string>()},
                new SqlParameter() {ParameterName = "@descuentoMillas",Value= facturaCabecera.SelectToken("descuentoMillas").Value<string>()}
            };

            try
            {
                ds = confact.ejecutarSP(conexionPOSWeb, "SP_PW_Set_Factura", parameters);
            }
            catch (Exception e)
            {
                try
                {

                    guardarLogs("WS_POS_web", "Crear Factura", "wsPOSweb.asmx.cs", "setFactura", "", JsonConvert.SerializeObject(e), "error DB", dataFact, facturaUsuario.SelectToken("idUser").Value<string>(), facturaCabecera.SelectToken("caja").SelectToken("idCaja").Value<string>(), "");
                }
                catch (Exception e2)
                {
                }
            }

            if (ds.Tables[0].Rows[0]["respuesta"].ToString().Equals("0"))
            {
                try
                {

                    Thread hilo = new Thread(() => setThreadFacturaSAP(ds.Tables[0].Rows[0]["idDocumentosCabecera"].ToString(), facturaCabecera.SelectToken("cliente").SelectToken("tipo").Value<string>()));
                    hilo.IsBackground = true;
                    hilo.Start();
                }
                catch (Exception e) { };
            }
            else
            {
                try
                {

                    guardarLogs("WS_POS_web", "Crear Factura", "wsPOSweb.asmx.cs", "setFactura", "", ds.Tables[0].Rows[0]["ErrorMessage"].ToString(), "error DB", dataFact, facturaUsuario.SelectToken("idUser").Value<string>(), facturaCabecera.SelectToken("caja").SelectToken("idCaja").Value<string>(), "");
                }
                catch (Exception e2)
                {
                }
            }
            return ds;
        }

        public void setThreadFacturaSAP(String id_factura, String tipoCliente)
        {
            DataSet ds = null;
            DataTable pData = null;
            DateTime pFeFa = new DateTime();
            String production = "";

            if (produccion) // SI ES PRODUCCION
            {
                production = "S";
            }
            else
            {
                production = "N";
            }

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@idDocumentosCabecera",Value= id_factura}
            };

            try
            {
                ds = confact.ejecutarSP(conexionPOSWeb, "sp_getInfoFacturaParaSAP_PRD", parameters);
                pData = ds.Tables[1];
            }
            catch (Exception e)
            {

            }

            String[] respuest = setFacturaSAP(production, id_factura, ds.Tables[0].Rows[0]["numFacturaSRI"].ToString(), pData, pFeFa,
                ds.Tables[0].Rows[0]["codigoSAPCliente"].ToString(), ds.Tables[0].Rows[0]["identificacionVendedor"].ToString(),
                ds.Tables[0].Rows[0]["sector"].ToString(), ds.Tables[0].Rows[0]["oficinaVenta"].ToString(),
                ds.Tables[0].Rows[0]["canal"].ToString(), ds.Tables[0].Rows[0]["tipoNegociacion"].ToString(), ds.Tables[0].Rows[0]["almacen"].ToString(), ds.Tables[0].Rows[0]["sociedad"].ToString(),
                ds.Tables[0].Rows[0]["identificacionUsuarioPOS"].ToString(), "", "",
                ds.Tables[0].Rows[0]["noGenerarMillas"].ToString(), ds.Tables[0].Rows[0]["descuentoMillas"].ToString(),
                tipoCliente, ds.Tables[0].Rows[0]["numeroOrdenTrabajo"].ToString(), ds.Tables[0].Rows[0]["tarjetaCorporativa"].ToString());
        }

        [WebMethod]
        public string[] setFacturaSAP(string esProduccion, string idFact, string pDSRI, DataTable pData, DateTime pFeFa, string pClie, string pIdVendedor, string pSect, string pOfVe, string pCana, string pFoPa, string pArea, string pOrga, string usuario, string pReproceso, string pCedulaReprocesa, string noGenerarMillas, string descuentoMillas, string tipoCliente, string numeroOrdenTrabajo, string tarjetaCorporativa)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ",";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";

            string vReto = "";
            string eArti = "";
            string[] sReto = new string[5];
            string sFech = "";
            string[] arrayVendedor = new string[2];

            double porcentajeDescuento = 0;

            String[] codigoFundas = MetodosComunes.getCodigoFundasPlasticas();
            tarjetaCorporativa = tarjetaCorporativa == "S" ? "X" : "";

            try
            {
                sFech = string.Format("{0:00}{1:00}{2:0000}", pFeFa.Day, pFeFa.Month, pFeFa.Year);
            }
            catch (Exception err)
            {
                sReto[0] = "Error al recuperar la fecha para enviar a SAP. --> " + err.Message;
                return sReto;
            }

            if (esProduccion == "S")
            { //Cuando es PRODUCCION
                SapSwQCom_PRD.ZSDWS_POS_CONSULTA_COMBOS zerv = new SapSwQCom_PRD.ZSDWS_POS_CONSULTA_COMBOS();
                zerv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                try
                {
                    if (tipoCliente == "FAM1")
                    {
                        pIdVendedor = "0190007510001"; //RUC de Almacenes Juan Eljuri
                    }

                    if (pArea == "2194")
                    {
                        pIdVendedor = "0104613880"; //Federico Cabezas
                    }

                    arrayVendedor = GetVend(esProduccion, pIdVendedor);

                    if (arrayVendedor[0].Trim() != "0")
                    {
                        sReto[0] = "Error al recuperar el vendedor. --> " + arrayVendedor[0];
                        return sReto;
                    }
                }
                catch (Exception err)
                {
                    if (pReproceso == "X")
                    {
                        sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar el vendedor. --> " + err.Message;
                    }
                    else
                    {
                        sReto[0] = "Error al recuperar el vendedor. --> " + err.Message;
                    }
                    return sReto;
                }

                int contComponentesCombo = 0;
                Dictionary<string, string> dicComponentesCombo = new Dictionary<string, string>();

                //Cambia de posición los items 
                DataTable dtFundas = pData.Clone();
                DataTable dtCombos = pData.Clone();
                DataTable dtOrdenado = pData.Clone();
                try
                {
                    //Determinar el tamaño de la matriz que se enviara a SAP                    
                    foreach (DataRow f in pData.Rows)
                    {
                        string item = f["item"].ToString();

                        esCorrecto = false;
                        contE = 0;
                        do
                        {
                            try
                            {
                                zerv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio                        
                                vReto = zerv.ZsdrfcPosConsultaCombos(f["cent"].ToString(), f["item"].ToString()).Trim();

                                if (vReto == solicitudCancelada)
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
                                else
                                {
                                    if (pReproceso == "X")
                                    {
                                        sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar el número de componentes del combo. --> " + ex.Message;
                                    }
                                    else
                                    {
                                        sReto[0] = "Error al recuperar el número de componentes del combo. --> " + ex.Message;
                                    }
                                    return sReto;
                                }
                            }
                            contE++;
                        } while (!esCorrecto && contE < contRepetir);

                        if (vReto.Length > 0)
                        { //Combo 
                            string[] sCoEl = vReto.Split('|');
                            contComponentesCombo += (sCoEl.Length / 2) - 1;
                            dicComponentesCombo.Add(f["item"].ToString(), vReto);
                            dtCombos.ImportRow(f);
                        }
                        else if (Array.Exists(codigoFundas, element => element == f["item"].ToString().TrimStart('0')))
                        {
                            dtFundas.ImportRow(f);
                        }
                        else
                        {
                            dtOrdenado.ImportRow(f);
                        }
                    }
                    dtOrdenado.Merge(dtCombos); //Coloca al final todos los combos
                    dtOrdenado.Merge(dtFundas); //Coloca al final todas las fundas
                }
                catch (Exception err)
                {
                    if (pReproceso == "X")
                    {
                        sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar el número de componentes del combo. --> " + err.Message;
                    }
                    else
                    {
                        sReto[0] = "Error al recuperar el número de componentes del combo. --> " + err.Message;
                    }
                    return sReto;
                }

                Ws_Set_Factura_Sap_PRD.ZWS_FACTURACION_POS serv = new Ws_Set_Factura_Sap_PRD.ZWS_FACTURACION_POS();
                Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos[] items = new Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos[pData.Rows.Count + contComponentesCombo];
                Ws_Set_Factura_Sap_PRD.ZestSerieMat[] objSeries;
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                List<Ws_Set_Factura_Sap_PRD.ZestSerieMat> listaSerie = new List<Ws_Set_Factura_Sap_PRD.ZestSerieMat>();

                int r = 0;
                foreach (DataRow f in dtOrdenado.Rows)
                //foreach (DataRow f in pData.Rows)
                {
                    Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos item = new Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos();

                    vReto = "";
                    if (dicComponentesCombo.ContainsKey(f["item"].ToString()))
                    {
                        vReto = dicComponentesCombo[f["item"].ToString()];
                    }

                    if (vReto.Length > 0)
                    { //Combo  
                        try
                        {
                            item.Flag = "1";
                            item.Combo = "S";

                            item.Almacen = pArea;
                            item.Centro = f["cent"].ToString();
                            item.Material = f["item"].ToString();
                            item.Cantidad = f["cant"].ToString();
                            item.Valor = f["prec"].ToString();

                            if (double.Parse(f["dctoMuchosDecimales"].ToString()) > 0)
                            {
                                porcentajeDescuento = double.Parse(f["dctoMuchosDecimales"].ToString().Trim()) * 0.01;
                            }
                            else
                            {
                                porcentajeDescuento = double.Parse(f["dcto"].ToString().Trim()) * 0.01;
                            }

                            //MILLAS
                            if (descuentoMillas.Trim() == "0" || descuentoMillas.Trim() == "" || descuentoMillas.Trim() == "0.00")
                            {
                                item.Pordscto = (Math.Round((Math.Round(double.Parse(item.Cantidad) * double.Parse(item.Valor), 2) * (porcentajeDescuento)), 2)).ToString();
                            }
                            else
                            {
                                item.Pordscto = "0";
                            }

                            item.Categoria = "";

                            if (f["flag"].ToString() == "3")
                            {
                                item.Lote = f["extr"].ToString();
                                item.Serie = "";
                            }
                            else if (f["flag"].ToString() == "2")
                            {
                                item.Serie = f["extr"].ToString();
                                item.Lote = "";
                            }
                            else
                            {
                                item.Serie = "";
                                item.Lote = "";
                            }

                            items[r] = item;
                        }
                        catch (Exception err)
                        {
                            if (pReproceso == "X")
                            {
                                sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al agregar la información principal del combo. --> " + err.Message;
                            }
                            else
                            {
                                sReto[0] = "Error al agregar la información principal del combo. --> " + err.Message;
                            }
                            return sReto;
                        }

                        Dictionary<string, string> dicSerieLote = new Dictionary<string, string>();
                        try
                        {
                            //--------------- Recupero las series y los lotes de los componentes del combo -----------                             
                            if (f["extr"].ToString().Length > 0)
                            {
                                string[] arraySerieLotes = f["extr"].ToString().Split('@');

                                for (int i = 0; i < arraySerieLotes.Length; i++)
                                {
                                    string[] arrayCodigoSerieLote = arraySerieLotes[i].Split(':');
                                    if (arrayCodigoSerieLote.Length > 1)
                                    {
                                        dicSerieLote.Add(arrayCodigoSerieLote[0], arrayCodigoSerieLote[1]);
                                    }
                                }
                            }
                            //---------------------------------------------------------------------------------------
                        }
                        catch (Exception err)
                        {
                            if (pReproceso == "X")
                            {
                                sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar series y lotes del combo. --> " + err.Message;
                            }
                            else
                            {
                                sReto[0] = "Error al recuperar series y lotes del combo. --> " + err.Message;
                            }
                            return sReto;
                        }

                        //Registrar los materiales de dicho combo desglose de items del combo
                        string[] sCoEl = vReto.Split('|');
                        int eNume = sCoEl.Length / 2;
                        double eCant;
                        byte eFlag;

                        //Recuperamos elementos del combo y los guardamos en matriz
                        for (int e = 1; e < eNume; e++)
                        {
                            Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos mComb = new Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos();

                            try
                            {
                                eArti = sCoEl[e * 2];
                                eCant = double.Parse(sCoEl[(e * 2) + 1]);

                                //Verificamos lote, serie o material

                                DataTable sMate = getMaterial(pArea, eArti, pCana);
                                eFlag = 1;

                                if (sMate.Rows[0]["sujeto_a_Lote"].ToString() == "X") eFlag = 3;
                                else if (sMate.Rows[0]["perfilNumeroSerie"].ToString().Trim().Length > 0) eFlag = 2;

                                r++;
                                mComb.Flag = eFlag.ToString();
                                mComb.Combo = "S";

                                mComb.Almacen = pArea;
                                mComb.Centro = sMate.Rows[0]["centro"].ToString();
                                mComb.Material = sMate.Rows[0]["codigo"].ToString();
                                mComb.Cantidad = f["cant"].ToString();
                                eCant = double.Parse(f["cant"].ToString()) * eCant;
                                mComb.Cantidad = eCant.ToString();
                                mComb.Valor = sMate.Rows[0]["precioNeto"].ToString().Trim();
                                mComb.Pordscto = "0";
                                mComb.Categoria = "";

                                mComb.Serie = "";
                                mComb.Lote = "";
                                if (dicSerieLote.ContainsKey(sMate.Rows[0]["codigo"].ToString()))
                                {
                                    if (eFlag == 3)
                                    { //Lote
                                        string[] arrayLote = dicSerieLote[sMate.Rows[0]["codigo"].ToString()].Split('|');
                                        mComb.Lote = arrayLote[1];
                                        //mComb.Lote = dicSerieLote[m["codigo"].ToString()]; //Cuando se empiece a controlar combo REVISAR                                                                        
                                    }
                                    else if (eFlag == 2)
                                    { //Serie
                                        string series = dicSerieLote[sMate.Rows[0]["codigo"].ToString()];
                                        if (series.Length > 0)
                                        {
                                            string[] arraySeries = series.Split(';');

                                            for (int i = 0; i < arraySeries.Length; i++)
                                            {
                                                Ws_Set_Factura_Sap_PRD.ZestSerieMat fila = new Ws_Set_Factura_Sap_PRD.ZestSerieMat();
                                                fila.Matnr = sMate.Rows[0]["codigo"].ToString();
                                                fila.Sernr = arraySeries[i];
                                                fila.Combo = "S";

                                                listaSerie.Add(fila);
                                                mComb.Serie = arraySeries[i];
                                            }
                                        }
                                    }
                                }

                                items[r] = mComb;
                            }
                            catch (Exception err)
                            {
                                if (pReproceso == "X")
                                {
                                    sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar la información del componente del combo. --> " + err.Message;
                                }
                                else
                                {
                                    sReto[0] = "Error al recuperar la información del componente del combo. --> " + eArti + " --> " + err.Message;
                                }
                                return sReto;
                            }
                        }

                    }
                    else
                    { //No es combo
                        try
                        {
                            item.Flag = f["flag"].ToString();
                            item.Combo = "N";

                            item.Almacen = pArea;
                            item.Centro = f["cent"].ToString();
                            item.Material = f["item"].ToString();
                            item.Cantidad = f["cant"].ToString();
                            item.Valor = f["prec"].ToString();

                            if (double.Parse(f["dctoMuchosDecimales"].ToString()) > 0)
                            {
                                porcentajeDescuento = double.Parse(f["dctoMuchosDecimales"].ToString().Trim()) * 0.01;
                            }
                            else
                            {
                                porcentajeDescuento = double.Parse(f["dcto"].ToString().Trim()) * 0.01;
                            }

                            //MILLAS
                            if (descuentoMillas.Trim() == "0" || descuentoMillas.Trim() == "" || descuentoMillas.Trim() == "0.00")
                            {
                                item.Pordscto = (Math.Round((Math.Round(double.Parse(item.Cantidad) * double.Parse(item.Valor), 2) * (porcentajeDescuento)), 2)).ToString();
                            }
                            else
                            {
                                item.Pordscto = "0";
                            }

                            item.Categoria = "";

                            item.Serie = "";
                            item.Lote = "";
                            if (f["flag"].ToString() == "3")
                            { //Lote                             
                                item.Lote = f["extr"].ToString();
                            }
                            else if (f["flag"].ToString() == "2")
                            { //Serie                                
                                if (f["extr"].ToString().Length > 0)
                                {
                                    string[] arraySeries = f["extr"].ToString().Split(';');

                                    for (int i = 0; i < arraySeries.Length; i++)
                                    {
                                        Ws_Set_Factura_Sap_PRD.ZestSerieMat fila = new Ws_Set_Factura_Sap_PRD.ZestSerieMat();
                                        fila.Matnr = f["item"].ToString();
                                        fila.Sernr = arraySeries[i];
                                        fila.Combo = "N";

                                        listaSerie.Add(fila);
                                        item.Serie = arraySeries[i];
                                    }
                                }
                            }

                            items[r] = item;
                        }
                        catch (Exception err)
                        {
                            if (pReproceso == "X")
                            {
                                sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar la información de cada item cuando NO es combo. --> " + err.Message;
                            }
                            else
                            {
                                sReto[0] = "Error al recuperar la información de cada item cuando NO es combo. --> " + err.Message;
                            }
                            return sReto;
                        }
                    }
                    r++;
                }
                objSeries = listaSerie.ToArray();

                try
                {
                    serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                    sReto = serv.ZsdrfcFacOtPos("", descuentoMillas, "",pDSRI, sFech, items, pClie, numeroOrdenTrabajo, arrayVendedor[1], pReproceso, objSeries, noGenerarMillas, pSect, tarjetaCorporativa, usuario, pCedulaReprocesa, pOfVe, pOrga, pCana, pFoPa).Split('|'); //11/03/2022
                }
                catch (Exception err)
                {
                    if (pReproceso == "X")
                    {
                        sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al llamar a WS ZsdrfcPedidoEntregaFacPos. --> " + err.Message;
                    }
                    else
                    {
                        sReto[0] = "Error al llamar a WS ZsdrfcPedidoEntregaFacPos. --> " + err.Message;
                    }
                    return sReto;
                }

                return sReto;
            }
            else
            { //Cuando es CALIDAD 

                SapSwQCom_PRD.ZSDWS_POS_CONSULTA_COMBOS zerv = new SapSwQCom_PRD.ZSDWS_POS_CONSULTA_COMBOS();
                zerv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                try
                {
                    if (tipoCliente == "FAM1")
                    {
                        pIdVendedor = "0190007510001"; //RUC de Almacenes Juan Eljuri
                    }

                    if (pArea == "2194")
                    {
                        pIdVendedor = "0104613880"; //Federico Cabezas
                    }

                    arrayVendedor = GetVend(esProduccion, pIdVendedor);

                    if (arrayVendedor[0].Trim() != "0")
                    {
                        sReto[0] = "Error al recuperar el vendedor. --> " + arrayVendedor[0];
                        return sReto;
                    }
                }
                catch (Exception err)
                {
                    if (pReproceso == "X")
                    {
                        sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar el vendedor. --> " + err.Message;
                    }
                    else
                    {
                        sReto[0] = "Error al recuperar el vendedor. --> " + err.Message;
                    }
                    return sReto;
                }

                int contComponentesCombo = 0;
                Dictionary<string, string> dicComponentesCombo = new Dictionary<string, string>();

                //Cambia de posición los items 
                DataTable dtUno = pData.Clone();
                DataTable dtDos = pData.Clone();
                DataTable dtTres = pData.Clone();

                try
                {
                    //Determinar el tamaño de la matriz que se enviara a SAP                    
                    foreach (DataRow f in pData.Rows)
                    {
                        string item = f["item"].ToString();

                        esCorrecto = false;
                        contE = 0;
                        do
                        {
                            try
                            {
                                zerv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                                vReto = zerv.ZsdrfcPosConsultaCombos(f["cent"].ToString(), f["item"].ToString()).Trim();

                                if (vReto == solicitudCancelada)
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
                                else
                                {
                                    if (pReproceso == "X")
                                    {
                                        sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar el número de componentes del combo. --> " + ex.Message;
                                    }
                                    else
                                    {
                                        sReto[0] = "Error al recuperar el número de componentes del combo. --> " + ex.Message;
                                    }
                                    return sReto;
                                }
                            }
                            contE++;
                        } while (!esCorrecto && contE < contRepetir);

                        if (vReto.Length > 0)
                        { //Combo 
                            string[] sCoEl = vReto.Split('|');
                            contComponentesCombo += (sCoEl.Length / 2) - 1;
                            dicComponentesCombo.Add(f["item"].ToString(), vReto);
                            dtUno.ImportRow(f);
                        }
                        else if (Array.Exists(codigoFundas, element => element == f["item"].ToString().TrimStart('0')))
                        {
                            dtTres.ImportRow(f);
                        }
                        else
                        {
                            dtDos.ImportRow(f);
                        }
                    }
                    dtDos.Merge(dtUno); //Coloca al final todos los combos
                    dtDos.Merge(dtTres); //Coloca al final todas las fundas
                }
                catch (Exception err)
                {

                }

                Ws_Set_Factura_Sap_PRD.ZWS_FACTURACION_POS serv = new Ws_Set_Factura_Sap_PRD.ZWS_FACTURACION_POS();
                Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos[] items = new Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos[pData.Rows.Count + contComponentesCombo];
                Ws_Set_Factura_Sap_PRD.ZestSerieMat[] objSeries;
                //Ws_Set_Factura_Sap.ZmmWsLote[] objLotes;
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                List<Ws_Set_Factura_Sap_PRD.ZestSerieMat> listaSerie = new List<Ws_Set_Factura_Sap_PRD.ZestSerieMat>();
                //List<Ws_Set_Factura_Sap.ZmmWsLote> listaLote = new List<Ws_Set_Factura_Sap.ZmmWsLote>();         

                int r = 0;
                foreach (DataRow f in dtDos.Rows)
                //foreach (DataRow f in pData.Rows)
                {
                    Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos item = new Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos();

                    vReto = "";
                    if (dicComponentesCombo.ContainsKey(f["item"].ToString()))
                    {
                        vReto = dicComponentesCombo[f["item"].ToString()];
                    }

                    if (vReto.Length > 0)
                    { //Combo  
                        try
                        {
                            item.Flag = "1";
                            item.Combo = "S";

                            item.Almacen = pArea;
                            item.Centro = f["cent"].ToString();
                            item.Material = f["item"].ToString();
                            item.Cantidad = f["cant"].ToString();
                            item.Valor = f["prec"].ToString();

                            if (double.Parse(f["dctoMuchosDecimales"].ToString()) > 0)
                            {
                                porcentajeDescuento = double.Parse(f["dctoMuchosDecimales"].ToString().Trim()) * 0.01;
                            }
                            else
                            {
                                porcentajeDescuento = double.Parse(f["dcto"].ToString().Trim()) * 0.01;
                            }

                            //MILLAS
                            if (descuentoMillas == null || descuentoMillas.Trim() == "0" || descuentoMillas.Trim() == "" || descuentoMillas.Trim() == "0.00")
                            {
                                item.Pordscto = (Math.Round((Math.Round(double.Parse(item.Cantidad) * double.Parse(item.Valor), 2) * (porcentajeDescuento)), 2)).ToString();
                            }
                            else
                            {
                                item.Pordscto = "0";
                            }

                            item.Categoria = "";

                            if (f["flag"].ToString() == "3")
                            {
                                item.Lote = f["extr"].ToString();
                                item.Serie = "";
                            }
                            else if (f["flag"].ToString() == "2")
                            {
                                item.Serie = f["extr"].ToString();
                                item.Lote = "";
                            }
                            else
                            {
                                item.Serie = "";
                                item.Lote = "";
                            }

                            items[r] = item;
                        }
                        catch (Exception err)
                        {
                            if (pReproceso == "X")
                            {
                                sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al agregar la información principal del combo. --> " + err.Message;
                            }
                            else
                            {
                                sReto[0] = "Error al agregar la información principal del combo. --> " + err.Message;
                            }
                            return sReto;
                        }

                        Dictionary<string, string> dicSerieLote = new Dictionary<string, string>();
                        try
                        {
                            //--------------- Recupero las series y los lotes de los componentes del combo -----------                         
                            if (f["extr"].ToString().Length > 0)
                            {
                                string[] arraySerieLotes = f["extr"].ToString().Split('@');

                                for (int i = 0; i < arraySerieLotes.Length; i++)
                                {
                                    string[] arrayCodigoSerieLote = arraySerieLotes[i].Split(':');
                                    if (arrayCodigoSerieLote.Length > 1)
                                    {
                                        dicSerieLote.Add(arrayCodigoSerieLote[0], arrayCodigoSerieLote[1]);
                                    }
                                }
                            }
                            //---------------------------------------------------------------------------------------
                        }
                        catch (Exception err)
                        {
                            if (pReproceso == "X")
                            {
                                sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar series y lotes del combo. --> " + err.Message;
                            }
                            else
                            {
                                sReto[0] = "Error al recuperar series y lotes del combo. --> " + err.Message;
                            }
                            return sReto;
                        }

                        //Registrar los materiales de dicho combo desglose de items del combo
                        string[] sCoEl = vReto.Split('|');
                        int eNume = sCoEl.Length / 2;
                        double eCant;
                        byte eFlag;

                        //Recuperamos elementos del combo y los guardamos en matriz
                        for (int e = 1; e < eNume; e++)
                        {
                            Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos mComb = new Ws_Set_Factura_Sap_PRD.ZsdesPosPedidos();

                            try
                            {
                                eArti = sCoEl[e * 2];
                                eCant = double.Parse(sCoEl[(e * 2) + 1]);

                                //Verificamos lote, serie o material
                                DataTable sMate = getMaterial(pArea, eArti, pCana);

                                foreach (DataRow m in sMate.Rows)
                                {
                                    eFlag = 1;

                                    if (m["sujeto_a_Lote"].ToString().Trim() == "X") eFlag = 3;
                                    else if (m["perfilNumeroSerie"].ToString().Trim().Length > 0) eFlag = 2;

                                    r++;
                                    mComb.Flag = eFlag.ToString();
                                    mComb.Combo = "S";

                                    mComb.Almacen = pArea;
                                    mComb.Centro = m["centro"].ToString();
                                    mComb.Material = m["codigo"].ToString();
                                    mComb.Cantidad = f["cant"].ToString();
                                    eCant = double.Parse(f["cant"].ToString()) * eCant;
                                    mComb.Cantidad = eCant.ToString();
                                    mComb.Valor = m["precioNeto"].ToString().Trim();
                                    mComb.Pordscto = "0";
                                    mComb.Categoria = "";

                                    mComb.Serie = "";
                                    mComb.Lote = "";
                                    if (dicSerieLote.ContainsKey(m["codigo"].ToString()))
                                    {
                                        if (eFlag == 3)
                                        { //Lote
                                            string[] arrayLote = dicSerieLote[m["codigo"].ToString()].Split('|');
                                            mComb.Lote = arrayLote[1];

                                            //string[] arrayLotes = dicSerieLote[m["codigo"].ToString()].Split(';');                                    

                                            //for (int i = 0; i < arrayLotes.Length; i++)
                                            //{
                                            //    string[] arrayCantidadLote = arrayLotes[i].Split('|');

                                            //    Ws_Set_Factura_Sap.ZmmWsLote fila = new Ws_Set_Factura_Sap.ZmmWsLote();
                                            //    fila.Matnp = item.Material;
                                            //    fila.Matnr = m["codigo"].ToString();
                                            //    fila.Lgort = pArea;
                                            //    fila.Lfimg = decimal.Parse(arrayCantidadLote[0]); //Cantidad
                                            //    fila.Charg = arrayCantidadLote[1]; //Lote                                        

                                            //    listaLote.Add(fila);                                            
                                            //}                                  

                                        }
                                        else if (eFlag == 2)
                                        { //Serie
                                            string series = dicSerieLote[m["codigo"].ToString()];
                                            if (series.Length > 0)
                                            {
                                                string[] arraySeries = series.Split(';');

                                                for (int i = 0; i < arraySeries.Length; i++)
                                                {
                                                    Ws_Set_Factura_Sap_PRD.ZestSerieMat fila = new Ws_Set_Factura_Sap_PRD.ZestSerieMat();
                                                    fila.Matnr = m["codigo"].ToString();
                                                    fila.Sernr = arraySeries[i];
                                                    fila.Combo = "S";

                                                    listaSerie.Add(fila);
                                                    mComb.Serie = arraySeries[i];
                                                }
                                            }
                                        }
                                    }

                                    items[r] = mComb;
                                }
                            }
                            catch (Exception err)
                            {
                                if (pReproceso == "X")
                                {
                                    sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar la información del componente del combo. --> " + err.Message;
                                }
                                else
                                {
                                    sReto[0] = "Error al recuperar la información del componente del combo. --> " + eArti + " --> " + err.Message;
                                }
                                return sReto;
                            }
                        }

                    }
                    else
                    { //No es combo
                        try
                        {
                            item.Flag = f["flag"].ToString();
                            item.Combo = "N";

                            item.Almacen = pArea;
                            item.Centro = f["cent"].ToString();
                            item.Material = f["item"].ToString();
                            item.Cantidad = f["cant"].ToString();
                            item.Valor = f["prec"].ToString();

                            if (double.Parse(f["dctoMuchosDecimales"].ToString()) > 0)
                            {
                                porcentajeDescuento = double.Parse(f["dctoMuchosDecimales"].ToString().Trim()) * 0.01;
                            }
                            else
                            {
                                porcentajeDescuento = double.Parse(f["dcto"].ToString().Trim()) * 0.01;
                            }

                            //MILLAS
                            if (descuentoMillas == null || descuentoMillas.Trim() == "0" || descuentoMillas.Trim() == "" || descuentoMillas.Trim() == "0.00")
                            {
                                item.Pordscto = (Math.Round((Math.Round(double.Parse(item.Cantidad) * double.Parse(item.Valor), 2) * (porcentajeDescuento)), 2)).ToString();
                            }
                            else
                            {
                                item.Pordscto = "0";
                            }

                            item.Categoria = "";

                            item.Serie = "";
                            item.Lote = "";
                            if (f["flag"].ToString() == "3")
                            { //Lote 
                                item.Lote = f["extr"].ToString();

                                //if (f["extr"].ToString().Length > 0){
                                //    string[] arrayLotes = f["extr"].ToString().Split(';');

                                //    for (int i = 0; i < arrayLotes.Length; i++)
                                //    {
                                //        string[] arrayCantidadLote = arrayLotes[i].Split('|');

                                //        Ws_Set_Factura_Sap.ZmmWsLote fila = new Ws_Set_Factura_Sap.ZmmWsLote();
                                //        fila.Matnp = f["item"].ToString();
                                //        fila.Matnr = f["item"].ToString();
                                //        fila.Lgort = pArea;
                                //        fila.Lfimg = decimal.Parse(arrayCantidadLote[0]); //Cantidad
                                //        fila.Charg = arrayCantidadLote[1]; //Lote                                        

                                //        listaLote.Add(fila);                                            
                                //    }                                                                 
                                //} 

                            }
                            else if (f["flag"].ToString() == "2")
                            { //Serie                                
                                if (f["extr"].ToString().Length > 0)
                                {
                                    string[] arraySeries = f["extr"].ToString().Split(';');

                                    for (int i = 0; i < arraySeries.Length; i++)
                                    {
                                        Ws_Set_Factura_Sap_PRD.ZestSerieMat fila = new Ws_Set_Factura_Sap_PRD.ZestSerieMat();
                                        fila.Matnr = f["item"].ToString();
                                        fila.Sernr = arraySeries[i];
                                        fila.Combo = "N";

                                        listaSerie.Add(fila);
                                        item.Serie = arraySeries[i];
                                    }
                                }
                            }

                            items[r] = item;
                        }
                        catch (Exception err)
                        {
                            if (pReproceso == "X")
                            {
                                sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al recuperar la información de cada item cuando NO es combo. --> " + err.Message;
                            }
                            else
                            {
                                sReto[0] = "Error al recuperar la información de cada item cuando NO es combo. --> " + err.Message;
                            }
                            return sReto;
                        }
                    }
                    r++;
                }
                objSeries = listaSerie.ToArray();
                //objLotes = listaLote.ToArray();
                string dataFacSAP = "";
                try
                {
                    serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio                         
                    dataFacSAP = "ZsdrfcFacOtPos(Wcheck: " + "" + ", WdesMillas: " + descuentoMillas + ", Wfacsri: " + pDSRI + ", Wfecha: " + sFech + ", Witems: " + JsonConvert.SerializeObject(items) + ", Wkunnr: " + pClie + ", WordenTrabajo: " + numeroOrdenTrabajo + ", Wpartner: " + arrayVendedor[1] + ", Wreproceso: " + pReproceso + ", Wseries: " + JsonConvert.SerializeObject(objSeries) + ", Wsinmillas: " + noGenerarMillas + ", Wspart: " + pSect + ", Wtcorp: " + tarjetaCorporativa + ", Wucaja: " + usuario + ", Wureproceso: " + pCedulaReprocesa + ", Wvkbur: " + pOfVe + ", Wvkorg: " + pOrga + ", Wvtweg: " + pCana + ", Wzterm: " + pFoPa + ")";

                    sReto = serv.ZsdrfcFacOtPos("", descuentoMillas, "",pDSRI, sFech, items, pClie, numeroOrdenTrabajo, arrayVendedor[1], pReproceso, objSeries, noGenerarMillas, pSect, tarjetaCorporativa, usuario, pCedulaReprocesa, pOfVe, pOrga, pCana, pFoPa).Split('|'); //11/03/2022

                }
                catch (Exception err)
                {
                    if (pReproceso == "X")
                    {
                        sReto[0] = "PROCESE NUEVAMENTE POR FAVOR. \nError al llamar a WS ZsdrfcPedidoEntregaFacPos. --> " + err.Message;
                    }
                    else
                    {
                        sReto[0] = "Error al llamar a WS ZsdrfcPedidoEntregaFacPos. --> " + err.Message;
                    }
                    return sReto;
                }
                if (sReto[0].Equals("Serie SRI no corresponde a la Of.Vta"))
                {
                    try
                    {
                        guardarLogs("WS_POS_web", "Crear Factura SAP", "wsPOSweb.asmx.cs", "setFacturaSAP", "", JsonConvert.SerializeObject(sReto), "error SAP", dataFacSAP, usuario, pOfVe, idFact);
                    }
                    catch (Exception elog) { }
                }
                return sReto;
            }
        }

        #endregion

        #region << NOTA CREDITO >>

        //Recupera los datos de una factura, utilizado para visualizar factura previo a nota de crédito
        [WebMethod]
        public DataSet recuperarDatosFactura(String pNumeroFacturaSAP)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator = ",";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator = ",";

            DataSet datosFactura = new DataSet("datosFactura");


            DataTable dtCabecera = new DataTable("facturaCabecera");

            dtCabecera.Columns.Add("nombre");
            dtCabecera.Columns.Add("cedula");
            dtCabecera.Columns.Add("codSap");
            dtCabecera.Columns.Add("sociedad");
            dtCabecera.Columns.Add("canal");
            dtCabecera.Columns.Add("condicionPago");
            dtCabecera.Columns.Add("oficinaVenta");
            dtCabecera.Columns.Add("fecha");
            dtCabecera.Columns.Add("hora");


            DataTable dtItems = new DataTable("facturaItems");

            dtItems.Columns.Add("descripcion");
            dtItems.Columns.Add("codigoMaterial");
            dtItems.Columns.Add("cantidad");
            dtItems.Columns.Add("valor");
            dtItems.Columns.Add("precioTotal");
            dtItems.Columns.Add("iva");
            dtItems.Columns.Add("tipo");
            dtItems.Columns.Add("centro");
            dtItems.Columns.Add("almacen");
            dtItems.Columns.Add("unidadMedida");
            dtItems.Columns.Add("codigoCombo");
            dtItems.Columns.Add("perteneceACombo");


            DataTable dtSeriesLotes = new DataTable("facturaSeriesLotes");

            dtSeriesLotes.Columns.Add("sujetoA");
            dtSeriesLotes.Columns.Add("serieLote");
            dtSeriesLotes.Columns.Add("codigoMaterial");
            dtSeriesLotes.Columns.Add("cantidad");
            dtSeriesLotes.Columns.Add("almacen");
            dtSeriesLotes.Columns.Add("codigoCombo");
            dtSeriesLotes.Columns.Add("flagCombo");


            if (produccion) // SI ES PRODUCCION
            {
                Ws_Recuperar_Factura_PRD.ZWS_RECUPERAR_FACTURA_RESUL1 service = new Ws_Recuperar_Factura_PRD.ZWS_RECUPERAR_FACTURA_RESUL1();
                service.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);
                Ws_Recuperar_Factura_PRD.ZwsItemsFactura[] items;
                Ws_Recuperar_Factura_PRD.ZwsLote[] lotes;
                Ws_Recuperar_Factura_PRD.ZwsSeries[] Serie;
                Ws_Recuperar_Factura_PRD.ZwsCabeceraFactura[] Cabecera = service.ZRfcRecuperarFacturaResul(pNumeroFacturaSAP, out items, out lotes, out Serie);

                //datos de cabecera
                if (Cabecera.Length > 0)
                {
                    DataRow fila = dtCabecera.NewRow();

                    fila["nombre"] = Cabecera[0].Name1 + " " + Cabecera[0].Name2;
                    fila["cedula"] = Cabecera[0].Stcd1;
                    fila["codSap"] = Cabecera[0].Kunnr;
                    fila["sociedad"] = Cabecera[0].Vkorg;
                    fila["canal"] = Cabecera[0].Vtweg;
                    fila["condicionPago"] = Cabecera[0].Zterm;
                    fila["oficinaVenta"] = Cabecera[0].Vkbur;
                    fila["fecha"] = Cabecera[0].Fkdat;
                    fila["hora"] = Cabecera[0].Erzet;


                    dtCabecera.Rows.Add(fila);
                }

                //datos de items
                if (items.Length > 0)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        DataRow fila = dtItems.NewRow();

                        try
                        {
                            Decimal cantidad = items[i].Fkimg;

                            if (cantidad > 0)
                            {

                                Decimal precioTotal = items[i].Netwr;
                                Decimal precioUnitario = precioTotal / cantidad;
                                precioUnitario = Decimal.Round(precioUnitario, 2);

                                fila["descripcion"] = items[i].Maktx;
                                fila["codigoMaterial"] = Decimal.Parse(items[i].Matnr);
                                fila["cantidad"] = cantidad;
                                fila["valor"] = precioUnitario;
                                fila["precioTotal"] = items[i].Netwr;
                                fila["iva"] = items[i].Mwsbp;
                                fila["centro"] = items[i].Werks;
                                fila["almacen"] = items[i].Lgort;
                                fila["unidadMedida"] = items[i].Meins;

                                if (items[i].Indls.Trim().Equals(""))
                                {
                                    fila["tipo"] = "N";
                                }
                                else
                                {
                                    fila["tipo"] = items[i].Indls;
                                }

                                fila["codigoCombo"] = items[i].Matnp;
                                if (string.IsNullOrEmpty(items[i].Matnp))
                                {
                                    fila["perteneceACombo"] = "false";
                                }
                                else
                                {
                                    fila["perteneceACombo"] = "true";
                                    fila["codigoCombo"] = Decimal.Parse(items[i].Matnp);
                                }

                                dtItems.Rows.Add(fila);
                            }
                        }
                        catch (Exception e)
                        {

                        }

                    }
                }

                String almacen = "";

                //datos de lotes
                if (lotes.Length > 0)
                {
                    if (items.Length > 0)
                    {
                        almacen = items[0].Lgort;
                    }

                    for (int i = 0; i < lotes.Length; i++)
                    {
                        DataRow fila = dtSeriesLotes.NewRow();
                        fila["sujetoA"] = "L";
                        fila["serieLote"] = lotes[i].Charg;
                        fila["codigoMaterial"] = Decimal.Parse(lotes[i].Matnr);
                        fila["cantidad"] = lotes[i].Fkimg;
                        fila["almacen"] = almacen;
                        if (string.IsNullOrEmpty(lotes[i].Patnr))
                        {
                            fila["codigoCombo"] = lotes[i].Patnr;
                        }
                        else
                        {
                            try
                            {
                                fila["codigoCombo"] = Decimal.Parse(lotes[i].Patnr);
                            }
                            catch (Exception e)
                            {
                                fila["codigoCombo"] = lotes[i].Patnr;
                            }
                        }

                        fila["flagCombo"] = lotes[i].Combo;

                        dtSeriesLotes.Rows.Add(fila);

                    }
                }


                //datos de serie
                if (Serie.Length > 0)
                {
                    for (int i = 0; i < Serie.Length; i++)
                    {
                        DataRow fila = dtSeriesLotes.NewRow();
                        fila["sujetoA"] = "S";
                        fila["serieLote"] = Serie[i].Sernr;
                        fila["codigoMaterial"] = Decimal.Parse(Serie[i].Matnr);
                        fila["cantidad"] = "1";
                        fila["almacen"] = almacen;
                        //fila["codigoCombo"] = lotes[i].Patnr;
                        //fila["flagCombo"] = lotes[i].Combo;

                        dtSeriesLotes.Rows.Add(fila);
                    }
                }
            }
            else // SI ES CALIDAD
            {
                /*
                Ws_Recuperar_Factura_Restante.ZWS_RECUPERAR_FACTURA_RESUL1 service = new Ws_Recuperar_Factura_Restante.ZWS_RECUPERAR_FACTURA_RESUL1();
                service.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContreSap);
                Ws_Recuperar_Factura_Restante.ZwsItemsFactura[] items;
                Ws_Recuperar_Factura_Restante.ZwsLote[] lotes;
                Ws_Recuperar_Factura_Restante.ZwsSeries[] Serie;
                Ws_Recuperar_Factura_Restante.ZwsCabeceraFactura[] Cabecera = service.ZRfcRecuperarFacturaResul(pNumeroFacturaSAP, out items, out lotes, out Serie);

                //datos de cabecera
                if (Cabecera.Length > 0)
                {
                    DataRow fila = dtCabecera.NewRow();

                    fila["nombre"] = Cabecera[0].Name1 + " " + Cabecera[0].Name2;
                    fila["cedula"] = Cabecera[0].Stcd1;
                    fila["codSap"] = Cabecera[0].Kunnr;
                    fila["sociedad"] = Cabecera[0].Vkorg;
                    fila["canal"] = Cabecera[0].Vtweg;
                    fila["condicionPago"] = Cabecera[0].Zterm;
                    fila["oficinaVenta"] = Cabecera[0].Vkbur;
                    fila["fecha"] = Cabecera[0].Fkdat;
                    fila["hora"] = Cabecera[0].Erzet;


                    dtCabecera.Rows.Add(fila);
                }

                //datos de items
                if (items.Length > 0)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        DataRow fila = dtItems.NewRow();

                        try
                        {
                            Decimal cantidad = items[i].Fkimg;

                            if (cantidad > 0)
                            {

                                Decimal precioTotal = items[i].Netwr;
                                Decimal precioUnitario = precioTotal / cantidad;
                                precioUnitario = Decimal.Round(precioUnitario, 2);

                                fila["descripcion"] = items[i].Maktx;
                                fila["codigoMaterial"] = Decimal.Parse(items[i].Matnr);
                                fila["cantidad"] = cantidad;
                                fila["valor"] = precioUnitario;
                                fila["precioTotal"] = items[i].Netwr;
                                fila["iva"] = items[i].Mwsbp;
                                fila["centro"] = items[i].Werks;
                                fila["almacen"] = items[i].Lgort;
                                fila["unidadMedida"] = items[i].Meins;

                                if (items[i].Indls.Trim().Equals(""))
                                {
                                    fila["tipo"] = "N";
                                }
                                else
                                {
                                    fila["tipo"] = items[i].Indls;
                                }

                                fila["codigoCombo"] = items[i].Matnp;
                                if (string.IsNullOrEmpty(items[i].Matnp))
                                {
                                    fila["perteneceACombo"] = "false";
                                }
                                else
                                {
                                    fila["perteneceACombo"] = "true";
                                    fila["codigoCombo"] = Decimal.Parse(items[i].Matnp);
                                }

                                dtItems.Rows.Add(fila);
                            }
                        }
                        catch (Exception e)
                        {

                        }

                    }
                }

                String almacen = "";

                //datos de lotes
                if (lotes.Length > 0)
                {
                    if (items.Length > 0)
                    {
                        almacen = items[0].Lgort;
                    }

                    for (int i = 0; i < lotes.Length; i++)
                    {
                        DataRow fila = dtSeriesLotes.NewRow();
                        fila["sujetoA"] = "L";
                        fila["serieLote"] = lotes[i].Charg;
                        fila["codigoMaterial"] = Decimal.Parse(lotes[i].Matnr);
                        fila["cantidad"] = lotes[i].Fkimg;
                        fila["almacen"] = almacen;
                        if (string.IsNullOrEmpty(lotes[i].Patnr))
                        {
                            fila["codigoCombo"] = lotes[i].Patnr;
                        }
                        else
                        {
                            try
                            {
                                fila["codigoCombo"] = Decimal.Parse(lotes[i].Patnr);
                            }
                            catch (Exception e)
                            {
                                fila["codigoCombo"] = lotes[i].Patnr;
                            }
                        }

                        fila["flagCombo"] = lotes[i].Combo;

                        dtSeriesLotes.Rows.Add(fila);

                    }
                }


                //datos de serie
                if (Serie.Length > 0)
                {
                    for (int i = 0; i < Serie.Length; i++)
                    {
                        DataRow fila = dtSeriesLotes.NewRow();
                        fila["sujetoA"] = "S";
                        fila["serieLote"] = Serie[i].Sernr;
                        fila["codigoMaterial"] = Decimal.Parse(Serie[i].Matnr);
                        fila["cantidad"] = "1";
                        fila["almacen"] = almacen;
                        fila["codigoCombo"] = lotes[i].Patnr;
                        fila["flagCombo"] = lotes[i].Combo;

                        dtSeriesLotes.Rows.Add(fila);
                    }
                }
                */
            }

            datosFactura.Tables.Add(dtCabecera);
            datosFactura.Tables.Add(dtItems);
            datosFactura.Tables.Add(dtSeriesLotes);

            return datosFactura;

        }

        #endregion

        #region << LOGAPP >>

        public void guardarLogs(String log_errProyecto, String log_errTransaccion, String log_errArchivo, String log_errMetodo, String log_errObservacion,
            String log_errError, String log_errTipo, String log_errData, String log_usu, String log_idSapOfi, String idDocCab)
        {
            DataSet ds = null;
            String veridicador = "";

            veridicador = "1";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@plogErrProyecto",Value= log_errProyecto.Trim()},
                new SqlParameter() {ParameterName = "@plogErrTransaccion",Value= log_errTransaccion.Trim()},
                new SqlParameter() {ParameterName = "@plogErrArchivo",Value= log_errArchivo.Trim()},
                new SqlParameter() {ParameterName = "@plogErrMetodo",Value= log_errMetodo.Trim()},
                new SqlParameter() {ParameterName = "@plogErrObservacion",Value= log_errObservacion.Trim()},
                new SqlParameter() {ParameterName = "@plogErrError",Value= log_errError.Trim()},
                new SqlParameter() {ParameterName = "@plogErrTipo",Value= log_errTipo.Trim()},
                new SqlParameter() {ParameterName = "@plogErrData",Value= log_errData.Trim()},
                new SqlParameter() {ParameterName = "@pusuarioCr",Value= log_usu.Trim()},
                new SqlParameter() {ParameterName = "@psapOfi",Value= log_idSapOfi.Trim()},
                new SqlParameter() {ParameterName = "@pidDocumentosCabecera", Value = idDocCab.Trim().Equals("")?null:idDocCab.Trim()}
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
       
        [WebMethod]
        public string[] getClienteSAP(String ide)
        {
            String[] cliente = new string[27];

            if (produccion){ //SI ES PRODUCCIÓN

                Ws_Get_Cliente_PRD.ZSDWS_POS_CONSULTA_CLIENTES clie = new Ws_Get_Cliente_PRD.ZSDWS_POS_CONSULTA_CLIENTES();
                clie.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                esCorrecto = false;
                contE = 0;
                do{
                    try{
                        clie.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                        cliente = clie.ZsdrfcPosConsultaCliente(ide).Split('|');

                        esCorrecto = true;
                    }catch (Exception ex){
                        if (ex.Message.Contains(solicitudCancelada)){
                            esCorrecto = false;
                        }
                    }
                    contE++;
                } while (!esCorrecto && contE < contRepetir);
                
            }else{ //SI ES CALIDAD
               
            }

            return cliente;
        }

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

        //Recupera los Subramos SAP         
        [WebMethod]
        public DataTable getSubramos(String esProduccion)
        {            
            DataTable dt = new DataTable("dtSubramos");

            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("descripcion", typeof(string));

            if (esProduccion == "S"){ //Cuando es PRODUCCION
                wsGetSubramo_PRD.ZWS_LISTA_SUBRAMOS serv = new wsGetSubramo_PRD.ZWS_LISTA_SUBRAMOS();
                serv.Credentials = new System.Net.NetworkCredential(UsuarioSap, ContraSap);

                wsGetSubramo_PRD.ZsdEstListaSubramo[] listaSubramo = new wsGetSubramo_PRD.ZsdEstListaSubramo[0];

                try{
                    esCorrecto = false; 
                    contE = 0;
                    do{
                        try{
                            serv.Timeout = -1; //CHUI Agregado para maximizar el tiempo de espera del servicio
                            listaSubramo = serv.ZsdrfcListaSubramos();

                            esCorrecto = true;                            
                        }catch (Exception ex){                       
                            if (ex.Message.Contains(solicitudCancelada)){
                                esCorrecto = false;
                            }                                                                
                        }
                        contE++;
                    } while (!esCorrecto && contE < contRepetir);
                                                                                              
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

        //Consulta persona en BD y SAP
        [WebMethod]
        public DataTable getPersona(string pPerIdentificacion) {

            string[] cliente = new string[27];            
            DataTable dt = new DataTable("dtPersona");                          

            cliente = getClienteSAP(pPerIdentificacion);

            if (cliente[0].ToString() != "0"){ //Cliente no existe en SAP
                pPerIdentificacion = "0000000000";
            }

            dt = Bd.spPersona(2, pPerIdentificacion, "", "", "", "", "", "", "", "", "", "","","", "", "", "", "", null, null, null, null, null, null, null);
               
            if (cliente[0].ToString() == "0"){

                DataRow fila = dt.NewRow();
                if (dt.Rows.Count == 0){
                    fila["perIdentificacion"] = cliente[5].ToString();
                    fila["perNombres"] = cliente[7].ToString();
                    fila["perApellidos"] = cliente[6].ToString();
                    fila["perDireccion"] = cliente[11].ToString();
                    fila["perTelefono"] = cliente[20].ToString();
                    fila["perCelular"] = cliente[19].ToString();
                    fila["perEmail"] = cliente[18].ToString();
                    fila["perFechaNacimiento"] = "01/01/1900"; //TODO: SAP aún no retorna fecha de nacimiento
                    fila["perProvincia"] = cliente[8].ToString();
                    fila["perCanton"] = cliente[9].Length >= 2 ? cliente[9].Substring(cliente[9].Length - 2, 2) : cliente[9];
                    fila["perParroquia"] = cliente[10].Length >= 2 ? cliente[10].Substring(cliente[10].Length - 2, 2) : cliente[10];
                    fila["perCodigoSAP"] = cliente[1].ToString();
                    fila["perObservacion"] = "";
                    fila["idGruSubGenero"] = Bd.getElementoSubgrupo("GENERO",cliente[14].ToString());
                    fila["idGruSubEstadoCivil"] = Bd.getElementoSubgrupo("ESTADO CIVIL", cliente[16].ToString());
                    fila["idGruSubTratamiento"] = Bd.getElementoSubgrupo("TRATAMIENTO", cliente[15].ToString());
                    fila["idGruSubActividadEc"] = Bd.getElementoSubgrupo("ACTIVIDAD ECONO", cliente[17].ToString());
                    fila["idGruSubTipoIdentificacion"] = Bd.getElementoSubgrupo("TIPO IDENTIFICA", cliente[4].ToString());
                    fila["idGruSubGrupoCliente"] = "29"; //RETAIL
                    fila["idGruSubGrupoCuenta"] = "32";  //CLIENTE RETAIL AJE
                    fila["perDireccion2"] = "";

                    dt.Rows.Add(fila);
                }

                dt.Rows[0]["perRamo"] = cliente[22].ToString();
                dt.Rows[0]["perSubramo"] = cliente[23].ToString();
            }
                   
            return dt;
        }

        //Consulta la coordenada (LLave - Valor) en BD
        [WebMethod]
        public DataTable getTarjetaCoordenadas()
        {
            DataTable dt = new DataTable("dtTarjetaCoordenadas");

            dt = Bd.spTarjetaCoordenadas();

            return dt;
        }      
        #endregion
        
    }
}