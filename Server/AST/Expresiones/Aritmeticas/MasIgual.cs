using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Aritmeticas
{
    class MasIgual:Operacion, Expresion
    {
        public MasIgual(int linea, int columna, Expresion expresion1, Expresion expresion2)
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
