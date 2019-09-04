using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Otras
{
    class NodoColumnas
    {
        public String idColumna{ get; set; }
        public Tipo tipo { get; set; }

        public Boolean primaryKey = false;

        public LinkedList<String> listaIdColumnasPK = new LinkedList<string>();

        public int col { get; set; }
        public int linea { get; set; }

        public NodoColumnas(String idColumna,
            Tipo tipo, int col, int linea)
        {
            this.idColumna = idColumna;
            this.tipo = tipo;
            this.col = col;
            this.linea = linea;
        }

        public NodoColumnas(String idColumna,
            Tipo tipo, Boolean pk, int col, int linea)
        {
            this.idColumna = idColumna;
            this.tipo = tipo;
            this.primaryKey = pk;
            this.col = col;
            this.linea = linea;
        }


        public NodoColumnas(LinkedList<String> listaIdColumnasPK,
           int col, int linea)
        {
            this.listaIdColumnasPK = listaIdColumnasPK;
            this.col = col;
            this.linea = linea;
        }

    }
}
