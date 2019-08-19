using Server.AST.Expresiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Instrucciones.Ciclos
{
    class IfLista
    {
        public Expresion condicion { get; set; }
        public StatementBlock listaParaEjecutar { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public IfLista(Expresion condicion, StatementBlock listaParaEjecutar, int linea, int col)
        {
            this.condicion = condicion;
            this.listaParaEjecutar = listaParaEjecutar;
            this.linea = linea;
            this.col = col;
        }
    }
}
