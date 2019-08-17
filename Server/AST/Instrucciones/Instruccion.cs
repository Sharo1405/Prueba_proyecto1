using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Instrucciones
{
    interface Instruccion: NodoAST
    {
        Object ejecutar(Entorno entorno, ErrorImpresion listas);
    }
}
