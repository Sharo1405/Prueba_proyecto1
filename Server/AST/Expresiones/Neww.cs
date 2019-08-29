using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Otras;

namespace Server.AST.Expresiones
{
    class Neww : Expresion
    {
        public Tipo tipoNew { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Neww(Tipo tipoNew, int linea, int columna)
        {
            this.tipoNew = tipoNew;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipoNew.tipo;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            return this;
        }
    }
}
