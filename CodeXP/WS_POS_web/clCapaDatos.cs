using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Ws_POS_web
{
    public class clCapaDatos
    {
        public DataSet ejecutarSP(String cadena,String nombreSP, List<SqlParameter> parametros)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection conn = new SqlConnection(cadena);
            try
            {
                // setear parametros del command
                SqlCommand cmd = new SqlCommand(nombreSP, conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                //asignar paramentros
                cmd.Parameters.AddRange(parametros.ToArray());

                //ejecutar el query
                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet ejecutarSPSinParametros(String cadena, String nombreSP)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection conn = new SqlConnection(cadena);
            try
            {
                // setear parametros del command
                SqlCommand cmd = new SqlCommand(nombreSP, conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                //ejecutar el query
                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet ejecutarSPConDataTable(String cadena,String nombreSP,String nombreParametro,DataTable tabla)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConnection conn = new SqlConnection(cadena);
            try
            {
                // setear parametros del command
                SqlCommand cmd = new SqlCommand(nombreSP, conn);
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                //asignar paramentro tabla
                SqlParameter tvparam = cmd.Parameters.AddWithValue(nombreParametro, tabla);

                //ejecutar el query
                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}