﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace WS_POS_web
{
    class Bd {        
        static Core.SQL sql = new Core.SQL();       
     
        static bool esProduccion = ConfigurationManager.AppSettings["esProduccion"] == "S" ? true : false;
        static string conexionPOSWeb = esProduccion ? "conexionPOSWeb_PRD" : "conexionPOSWeb";
        
        //Retorna los Grupos o Subgrupos
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
    }
}
