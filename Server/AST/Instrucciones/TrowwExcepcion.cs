using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using static Server.AST.Instrucciones.TipoExcepcion;

namespace Server.AST.Instrucciones
{
    class TrowwExcepcion : Instruccion
    {
        public excep tipoExcep { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public TrowwExcepcion(excep tipoEx,
            int linea, int col)
        {
            this.tipoExcep = tipoEx;
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return this.tipoExcep;
        }
    }
}
