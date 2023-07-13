using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace WS_POS_web
{
    public class MetodosComunes
    {
        public static string[] getCodigoFundasPlasticas()
        {
            string[] codigoFundas;
            DataTable dtFundasPlasticas = Bd.getFundasPlasticas();

            codigoFundas = new string[dtFundasPlasticas.Rows.Count];
            for (int i = 0; i < dtFundasPlasticas.Rows.Count; i++)
            {
                codigoFundas[i] = dtFundasPlasticas.Rows[i]["funPlaCodigoArticulo"].ToString();
            }

            return codigoFundas;
        }
    }
}