using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace WS_POS_web
{
    class Bd {        
        static Core.SQL sql = new Core.SQL();

        string varChui = "";
        string varChu2 = "";
        string varChui3 = "";

        static bool esProduccion = ConfigurationManager.AppSettings["esProduccion"] == "S" ? true : false;
        static string conexionPOS = "conexionPos";
        static string conexionPOSWeb = esProduccion ? "conexionPOSWeb" : "conexionPOSWeb_PRD";
        //static string conexionPOSWeb = esProduccion ? "conexionPOSWeb_PRD" : "conexionPOSWeb";

        #region << SEBAS >>

        //Guarda el log de la aplicación
        public static void guardarLogs(String log_errProyecto, String log_errTransaccion, String log_errArchivo, String log_errMetodo, String log_errObservacion, String log_errError, String log_errTipo, String log_errData, String log_usu, String log_idSapOfi, String idDocCab)
        {          
            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("plogErrProyecto", log_errProyecto));
            pParams.Add(new Core.SQL.Parametro("plogErrTransaccion", log_errTransaccion));
            pParams.Add(new Core.SQL.Parametro("plogErrArchivo", log_errArchivo));
            pParams.Add(new Core.SQL.Parametro("plogErrMetodo", log_errMetodo));
            pParams.Add(new Core.SQL.Parametro("plogErrObservacion", log_errObservacion));
            pParams.Add(new Core.SQL.Parametro("plogErrError", log_errError));
            pParams.Add(new Core.SQL.Parametro("plogErrTipo", log_errTipo));
            pParams.Add(new Core.SQL.Parametro("plogErrData", log_errData));
            pParams.Add(new Core.SQL.Parametro("pusuarioCr", log_usu));
            pParams.Add(new Core.SQL.Parametro("psapOfi", log_idSapOfi));
            pParams.Add(new Core.SQL.Parametro("pidDocumentosCabecera", idDocCab));

            sql.DT(conexionPOSWeb, "SP_PW_Set_Logs", pParams);   
        }

        //Retorna la información del usuario
        public static DataTable getLogin(string usuario, string pass)
        {
            DataTable dt = new DataTable("");

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("pUsua", usuario));
            pParams.Add(new Core.SQL.Parametro("pPass", pass));

            dt = sql.DT(conexionPOSWeb, "SP_PW_Login", pParams);

            return dt;
        }

        //Retorna las transacciones del usuario
        public static DataSet transaccion(string idUsuario, string option, string idModule)
        {
            DataSet ds = new DataSet();

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("pIdUser", idUsuario));
            pParams.Add(new Core.SQL.Parametro("pOption", option));
            pParams.Add(new Core.SQL.Parametro("pIdModule", idModule));            
        
            ds = sql.DS(conexionPOSWeb, "SP_PW_Get_Transactions", pParams);

            return ds;
        }

        //Recupera las constantes para el sistema
        public static DataSet getConstantes()
        {
            DataSet ds = new DataSet();

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();        

            ds = sql.DS(conexionPOSWeb, "SP_PW_Get_Constants", pParams);

            return ds;
        }

        //Recupera las cajas por usurio
        public static DataSet getCajas(string idUsuario)
        {
            DataSet ds = new DataSet();

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("pIdUser", idUsuario));

            ds = sql.DS(conexionPOSWeb, "SP_PW_Get_CashRegisters", pParams);

            return ds;
        }

        //Recupera Las condiciones de pago por oficina de venta
        public static DataTable getCondicionesPago(string ofiVent)
        {
            DataTable dt = new DataTable("");

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("pOfiVenta", ofiVent));

            dt = sql.DT(conexionPOSWeb, "SP_PW_Get_PaymentConditions", pParams);

            return dt;
        }

        //Recupera los códigos de las fundas plásticas
        public static DataTable getFundasPlasticas()
        {
            DataTable dt = new DataTable("dtFundasPlasticas");

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();

            dt = sql.DT(conexionPOSWeb, "_SP_FUNDAS_PLASTICAS", pParams);

            return dt;
        }

        //Recupera los códigos de las fundas plásticas
        public static DataTable getVendedores(string ofiVent)
        {
            DataTable dt = new DataTable("");

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("pOfiVenta", ofiVent));

            dt = sql.DT(conexionPOSWeb, "SP_PW_Get_Sellers", pParams);

            return dt;
        }

        //Recupera descuento tope del local para el artículo dado
        public static string[] getDescuento(int opcion, string area, string esquema, string centro, string grupoArticulo, string codigoArticulo, string codigoSector, string tipoNegociacion, string ramoCliente, string idCliente, string jerarquia, bool tieneYmhPlus, string pagoConTarjeta)
        {
            DataTable dt = new DataTable();
            string[] descuento = new String[4];

            string yamahaPlus = tieneYmhPlus ? "S" : "N";

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("Opcion", opcion));
            pParams.Add(new Core.SQL.Parametro("Area", area));
            pParams.Add(new Core.SQL.Parametro("AreaEsquema", esquema));
            pParams.Add(new Core.SQL.Parametro("CodCentro", centro));
            pParams.Add(new Core.SQL.Parametro("CodMarca", grupoArticulo));
            pParams.Add(new Core.SQL.Parametro("CodMaterial", codigoArticulo));
            pParams.Add(new Core.SQL.Parametro("CodSector", codigoSector));
            pParams.Add(new Core.SQL.Parametro("TipoNegociacion", tipoNegociacion));
            pParams.Add(new Core.SQL.Parametro("pTipoCliente", ramoCliente));
            pParams.Add(new Core.SQL.Parametro("pClie", idCliente));
            pParams.Add(new Core.SQL.Parametro("Jerarquia", jerarquia));
            pParams.Add(new Core.SQL.Parametro("TieneYmhPlus", yamahaPlus));
            pParams.Add(new Core.SQL.Parametro("pPagoConTarjeta", pagoConTarjeta));

            dt = sql.DT(conexionPOS, "sp_Web_Descuentos_PRD", pParams);            

            descuento[0] = dt.Rows[0][0].ToString();
            descuento[1] = dt.Rows[0][1].ToString();
            descuento[2] = dt.Rows[0][2].ToString();
            descuento[3] = dt.Rows[0][3].ToString();

            return descuento;
        }

        //Recupera precios para POS
        public static DataTable getPrecioSP(decimal precio, string esquema, string codigoArticulo, int area, string grupoArticulo, string centro, string sector, string idCliente, string pTipoNegociacion, string pTipoCliente, string estadoMaterial, decimal costo, string esCombo, decimal costoCombo, string dctoTodosLosLocales, string pagoConTarjeta, string cantidadVender)
        {
            DataTable dt = new DataTable();

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("pPrec", precio));
            pParams.Add(new Core.SQL.Parametro("pEmpr", esquema));
            pParams.Add(new Core.SQL.Parametro("pIden", codigoArticulo));
            pParams.Add(new Core.SQL.Parametro("pArea", area));
            pParams.Add(new Core.SQL.Parametro("pGrAr", grupoArticulo));
            pParams.Add(new Core.SQL.Parametro("pCentro", centro));
            pParams.Add(new Core.SQL.Parametro("pSector", sector));
            pParams.Add(new Core.SQL.Parametro("pClie", idCliente));
            pParams.Add(new Core.SQL.Parametro("pTipoNegociacion", pTipoNegociacion));
            pParams.Add(new Core.SQL.Parametro("pTipoCliente", pTipoCliente));
            pParams.Add(new Core.SQL.Parametro("estadoMaterial", estadoMaterial));
            pParams.Add(new Core.SQL.Parametro("pCosto", costo));
            pParams.Add(new Core.SQL.Parametro("pEsCombo", esCombo));
            pParams.Add(new Core.SQL.Parametro("pCostoCombo", costoCombo));
            pParams.Add(new Core.SQL.Parametro("pDctoTodosLosLocales", dctoTodosLosLocales));
            pParams.Add(new Core.SQL.Parametro("pPagoConTarjeta", pagoConTarjeta));
            pParams.Add(new Core.SQL.Parametro("pCantidadVender", cantidadVender));
            pParams.Add(new Core.SQL.Parametro("pEsWeb", "N"));
           
            dt = sql.DT(conexionPOS, "_PKG_SAPArtis_PRD", pParams);

            return dt;
        }

        //Recupera la informacion de la factura para enviar a SAP
        public static DataSet getInfoFacturaParaSAP(string id_factura)
        {
            DataSet ds = new DataSet();

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("idDocumentosCabecera", id_factura));

            ds = sql.DS(conexionPOSWeb, "sp_getInfoFacturaParaSAP_PRD", pParams);

            return ds;
        }

        #endregion

        #region <<CHUI>>
        //Retorna los elementos de un Subgrupos
        public static DataTable getGrupoSubgrupo(int opcion, string pIdGrupoSubgrupo, string pGruIdentificacion)
        {
            DataTable dt = new DataTable();        

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("opcion", opcion));
            pParams.Add(new Core.SQL.Parametro("pIdGrupoSubgrupo", pIdGrupoSubgrupo));
            pParams.Add(new Core.SQL.Parametro("pGruIdentificacion", pGruIdentificacion));      

            dt = sql.DT(conexionPOSWeb, "SP_PW_Get_SubGrupo_PRD", pParams);
           
            return dt;
        }

        //Retorna un elemento del Subgrupos dado el pIdGrupoSubgrupo del grupo y el pIdGrupoSubgrupo del subgrupo
        public static int getElementoSubgrupo(string pGruIdentificacion, string pGruIdentificacionSub)
        {
            int resultado = 0;

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("opcion", 3));   
            pParams.Add(new Core.SQL.Parametro("pGruIdentificacion", pGruIdentificacion));
            pParams.Add(new Core.SQL.Parametro("pGruIdentificacionSub", pGruIdentificacionSub));

            DataTable dt = sql.DT(conexionPOSWeb, "SP_PW_Get_SubGrupo_PRD", pParams);

            //Para que no de error, cuando no exista en la tabla de GRUPO_SUBGRUPO
            if (dt.Rows.Count > 0){
                resultado = int.Parse(dt.Rows[0]["idGrupoSubgrupo"].ToString());
            }

            return resultado;
        }

        //Consulta, graba o modifica Persona
        public static DataTable spPersona(                                          
                                            int opcion,	
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
                                        )
        {
            DataTable dt = new DataTable("dtPersona");

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();
            pParams.Add(new Core.SQL.Parametro("opcion", opcion));
            pParams.Add(new Core.SQL.Parametro("pPerIdentificacion", pPerIdentificacion));
            pParams.Add(new Core.SQL.Parametro("pPerNombres", pPerNombres));
            pParams.Add(new Core.SQL.Parametro("pPerApellidos", pPerApellidos));
            pParams.Add(new Core.SQL.Parametro("pPerDireccion", pPerDireccion));
            pParams.Add(new Core.SQL.Parametro("pPerDireccion2", pPerDireccion2));
            pParams.Add(new Core.SQL.Parametro("pPerTelefono", pPerTelefono));
            pParams.Add(new Core.SQL.Parametro("pPerCelular", pPerCelular));
            pParams.Add(new Core.SQL.Parametro("pPerEmail", pPerEmail));
            pParams.Add(new Core.SQL.Parametro("pPerFechaNacimiento", pPerFechaNacimiento));
            pParams.Add(new Core.SQL.Parametro("pPerProvincia", pPerProvincia));
            pParams.Add(new Core.SQL.Parametro("pPerCanton", pPerCanton));
            pParams.Add(new Core.SQL.Parametro("pPerParroquia", pPerParroquia));
            pParams.Add(new Core.SQL.Parametro("pPerCodigoSAP", pPerCodigoSAP));
            pParams.Add(new Core.SQL.Parametro("pPerObservacion", pPerObservacion));
            pParams.Add(new Core.SQL.Parametro("pPerRamo", pPerRamo));
            pParams.Add(new Core.SQL.Parametro("pPerSubramo", pPerSubramo));            
            pParams.Add(new Core.SQL.Parametro("pUsuario", pUsuario));
            pParams.Add(new Core.SQL.Parametro("pIdGruSubGenero", pIdGruSubGenero));
            pParams.Add(new Core.SQL.Parametro("pIdGruSubEstadoCivil", pIdGruSubEstadoCivil));
            pParams.Add(new Core.SQL.Parametro("pIdGruSubTratamiento", pIdGruSubTratamiento));
            pParams.Add(new Core.SQL.Parametro("pIdGruSubActividadEc", pIdGruSubActividadEc));
            pParams.Add(new Core.SQL.Parametro("pIdGruSubTipoIdentificacion", pIdGruSubTipoIdentificacion));
            pParams.Add(new Core.SQL.Parametro("pIdGruSubGrupoCliente", pIdGruSubGrupoCliente));
            pParams.Add(new Core.SQL.Parametro("pIdGruSubGrupoCuenta", pIdGruSubGrupoCuenta));

            DataSet ds = sql.DS(conexionPOSWeb, "SP_PW_PERSONA", pParams);
            
            if (ds.Tables.Count > 0){
                dt = ds.Tables[0];
            }
           
            return dt;
        }

        //Consulta, graba o modifica Persona
        public static DataTable spTarjetaCoordenadas(){
            DataTable dt = new DataTable("dtTarjetaCoordenadas");

            List<Core.SQL.Parametro> pParams = new List<Core.SQL.Parametro>();

            dt = sql.DT(conexionPOSWeb, "SP_PW_TARJETA_COORDENADAS", pParams);

            return dt;
        }
        
        #endregion
    }
}
