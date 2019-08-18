using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.TipoDato
{
    class Decimal: Expresion
    {
        public Object valor { get; set; }
        public tipoDato tipo { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Decimal()
        {
        }

        public Decimal(Object valor, tipoDato tipo, int linea, int columna)
        {
            this.valor = valor;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            return Double.Parse(Convert.ToString(valor));
        }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipoDato.decimall;
        }
    }
}
