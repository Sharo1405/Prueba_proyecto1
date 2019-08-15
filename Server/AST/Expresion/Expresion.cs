using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresion
{
    interface Expresion : NodoAST
    {

        Object getValue(Entorno entorno, ErrorImpresion listas);
        Object getType(Entorno entorno, ErrorImpresion listas);

    }
}
