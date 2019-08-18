using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Otras
{
    class Tipo
    {
        public String id { get; set; }
        public tipoDato tipo { get; set; }
        public LinkedList<Tipo> listaTipos = new LinkedList<Tipo>();
        public int linea { get; set; }
        public int columna { get; set; }


        public Tipo(tipoDato tipo, int linea, int columna)
        {
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
        }

        public Tipo(String id, tipoDato tipo, int linea, int columna)
        {
            this.id = id;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
        }

        public Tipo(tipoDato tipo, LinkedList<Tipo> listaTipos, int linea, int columna)
        {
            this.tipo = tipo;
            this.listaTipos = listaTipos;
            this.linea = linea;
            this.columna = columna;
        }

    }
}
