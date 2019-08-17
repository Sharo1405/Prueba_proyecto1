using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Aritmeticas
{
    class DividirIgual: Operacion, Expresion
    {
        public DividirIgual(int linea, int columna, Expresion expresion1, Expresion expresion2, string operador, int cantExp)
            : base(linea, columna, expresion1, expresion2)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            throw new NotImplementedException();
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            throw new NotImplementedException();
        }
    }
}
