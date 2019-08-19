﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;

namespace Server.AST.Expresiones
{
    class Comas : Operacion, Expresion
    {
        //solo expresion 1 funciona
        public Comas(int linea, int columna, Expresion expresion1)
            : base(linea, columna, expresion1)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return expresion1.getType(entorno, listas);
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            return expresion1.getValue(entorno, listas);
        }
    }
}