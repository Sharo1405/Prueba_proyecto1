using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Instrucciones
{
    class idpuntoEigualEupdate: Instruccion
    {
        public String idCol { get; set; }
        public Expresion igual { get; set; }
        public Expresion acceso { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public idpuntoEigualEupdate(String idCol, Expresion igual,
            Expresion acceso, int linea, int col)
        {
            this.idCol = idCol;
            this.igual = igual;
            this.acceso = acceso;
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return this;
        }
    }
}
