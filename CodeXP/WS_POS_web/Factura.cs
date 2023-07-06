using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_POS_web
{
    public class Factura
    {

    }
    public class ItemFactura
    {

        public string Posicion;
        public string CodigoMaterial;
        public string Centro;
        public string Almacen;
        public decimal Cantidad;
        public string Unidad;
        public decimal Precio;
        public decimal Descuento;
        public bool esCombo;



        public string getFlagCombo
        {

            get { return esCombo ? "S" : ""; }

        }

    }

    public class ItemCombo
    {

        public static string COMPONENTE_COMBO = "P";
        public static string COMPONENTE_MATERIAL_COMBO = "C";

        public static string SUJETO_A_LOTE = "L";
        public static string SUJETO_A_SERIE = "S";
        public static string SUJETO_A_NADA = "N";

        public string CodigoMaterial;
        public string TipoCombo;
        public string SujetoA;
        public decimal CantidadEnCombo;
        public decimal Stock;
        public int MaximaCantidad;



    }
}