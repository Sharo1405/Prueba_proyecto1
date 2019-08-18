using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones
{
    class Parametros
    {
        public Tipo tipo { get; set; }
        public String id { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        public Parametros(Tipo tipo, String id, int linea, int columna)
        {
            this.tipo = tipo;
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }

    }
}
