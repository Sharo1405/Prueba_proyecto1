using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.TipoDato
{
    class TypeBase : Expresion
    {
        public object valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public TypeBase(object valor, int linea, int columna)
        {
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return tipoDato.id;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return valor;
        }
    }
}
