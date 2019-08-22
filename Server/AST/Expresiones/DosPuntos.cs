using Server.AST.Entornos;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones
{
    class DosPuntos: Expresion
    {
        public ClaveValor claveValor { get; set; }
        public int linea;
        public int columna;

        public DosPuntos(ClaveValor claveValor, int linea, int columna)
        {
            this.claveValor = claveValor;
            this.linea = linea;
            this.columna = columna;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            return claveValor;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return Operacion.tipoDato.map;
        }
    }
}
