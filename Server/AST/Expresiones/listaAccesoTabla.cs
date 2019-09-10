using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;

namespace Server.AST.Expresiones
{
    class listaAccesoTabla : Expresion
    {
        public String idLista { get; set; }
        public Expresion index { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        public listaAccesoTabla(String idLista, Expresion index,
            int linea, int columna)
        {
            this.idLista = idLista;
            this.index = index;
            this.linea = linea;
            this.columna = columna;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            throw new NotImplementedException();
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            throw new NotImplementedException();
        }
    }
}
