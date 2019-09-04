using Server.AST.BaseDatos;
using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.TipoDato
{
    class Booleano : Expresion
    {
        public Object valor { get; set; }
        public tipoDato tipo { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Booleano()
        {
        }

        public Booleano(Object valor, tipoDato tipo, int linea, int columna)
        {
            this.valor = valor;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            
            return Convert.ToBoolean(valor);            
            
        }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return tipoDato.booleano;
        }
    }
}
