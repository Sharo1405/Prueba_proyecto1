using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.TipoDato
{
    class Numero : Expresion
    {
        public Object valor { get; set; }
        public tipoDato tipo { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Numero()
        {
        }

        public Numero(Object valor, tipoDato tipo, int linea, int columna)
        {
            this.valor = valor;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
        }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            tipoDato tipo = EnteroDecimal(valor);
            return tipo;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            tipoDato tipo = EnteroDecimal(valor);
            if (tipo == tipoDato.entero) {
                string val = valor.ToString();
                return Int32.Parse(val);
            }
            else
            {
                string val = valor.ToString();
                return Double.Parse(val);
            }
        }

        public tipoDato EnteroDecimal(Object valor)
        {
            double nuevo = Math.Floor(Double.Parse(Convert.ToString(valor)));
            if ((Double.Parse(Convert.ToString(valor)) - Double.Parse(Convert.ToString(nuevo))) == 0.0)
            {
                return tipoDato.entero;
            }
            else
            {
                return tipoDato.decimall;
            }
        }
    }
}
