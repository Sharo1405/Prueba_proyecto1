using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.TipoDato
{
    class ListsBase: Expresion
    {
        public object valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public ListsBase(object valor, int linea, int columna)
        {
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            if (valor is Lista)
            {
                return ((Lista)valor).tipoGeneral;
            }
            return tipoDato.errorSemantico;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return valor;
        }
    }
}
