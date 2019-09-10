using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;

namespace Server.AST.Instrucciones
{
    class idigualEupdate : Instruccion
    {
        public String idCol { get; set; }
        public Expresion igual { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public idigualEupdate(String idCol, Expresion igual,
            int linea, int col)
        {
            this.idCol = idCol;
            this.igual = igual;
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return this;
        }
    }
}
