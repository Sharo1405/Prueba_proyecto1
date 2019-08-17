using Server.AST.Expresiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Instrucciones
{
    class Imprimir : Instruccion
    {
        public Expresion expImpre { get; set; }
        public int linea { get; set; }
        public  int col { get; set; }


        public Imprimir(Expresion expImpre, int linea, int col)
        {
            this.expImpre = expImpre;
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            string valor = Convert.ToString(expImpre.getValue(entorno, listas));
            listas.impresiones.AddLast(valor);
            return null;
        }
    }
}
