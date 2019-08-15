﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Instruccion
{
    interface Instruccion: NodoAST
    {
        Object ejecutar(Entorno entorno, ErrorImpresion listas);
    }
}
