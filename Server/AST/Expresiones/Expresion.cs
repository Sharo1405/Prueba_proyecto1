using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    interface Expresion: NodoAST
    {
        Object getValue(Entorno entorno, ErrorImpresion listas);
        tipoDato getType(Entorno entorno, ErrorImpresion listas);
    }
}
