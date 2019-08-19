using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;

namespace Server.AST.Instrucciones
{
    class Continuee : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }

        public Continuee(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            return this;
        }
    }
}
