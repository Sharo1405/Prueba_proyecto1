using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;

namespace Server.AST.Expresiones
{
    class Corchetes : Expresion
    {

        public Expresion expresion { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Corchetes(Expresion expresion, int linea, int columna)
        {
            this.expresion = expresion;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return expresion.getType(entorno, listas);
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            return expresion.getValue(entorno, listas);
        }
    }
}
