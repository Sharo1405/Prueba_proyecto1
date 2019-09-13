using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;

namespace Server.AST.Expresiones
{
    class listaAccesoTablapuntoEigualE : Expresion
    {
        public String idListaCol { get; set; }
        public Expresion index { get; set; }
        public Expresion accesos { get; set; }
        public Expresion igual { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public listaAccesoTablapuntoEigualE(String nombre,
            Expresion index, Expresion accesos, Expresion igual,
            int linea, int columna)
        {
            this.idListaCol = nombre;
            this.index = index;
            this.accesos = accesos;
            this.igual = igual;
            this.linea = linea;
            this.columna = columna;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return Operacion.tipoDato.list;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return this;
        }
    }
}
