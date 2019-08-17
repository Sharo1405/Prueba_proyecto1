using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.TipoDato
{
    class Date: Expresion
    {
        public Object valor { get; set; }
        public tipoDato tipo { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Date()
        {}

        public Date(Object valor, tipoDato tipo, int linea, int columna)
        {
            this.valor = valor;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                return DateTime.Parse(Convert.ToString(valor));
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine("Error en cast Date");
            }
            return "";
        }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                DateTime.Parse(Convert.ToString(valor));
                return tipoDato.date;
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine("Error en cast Date");
            }
            return tipoDato.errorSemantico;
        }
    }
}
