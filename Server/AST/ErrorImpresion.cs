﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST
{
    class ErrorImpresion
    {
        public LinkedList<String> impresiones = new LinkedList<string>();
        public LinkedList<NodoError> errores = new LinkedList<NodoError>();
    }
}
