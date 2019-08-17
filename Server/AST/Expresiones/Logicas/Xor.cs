using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Logicas
{
    class Xor: Operacion, Expresion
    {
        public Xor(int linea, int columna, Expresion expresion1, Expresion expresion2)
            : base(linea, columna, expresion1, expresion2)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            if (((tipoDato)expresion1.getType(entorno, listas) == tipoDato.booleano) &&
                ((tipoDato)expresion2.getType(entorno, listas) == tipoDato.booleano))
            {
                return tipoDato.booleano;
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            if (((tipoDato)expresion1.getType(entorno, listas) == tipoDato.booleano) &&
                ((tipoDato)expresion2.getType(entorno, listas) == tipoDato.booleano))
            {
                return Convert.ToBoolean(expresion1.getValue(entorno, listas)) ^ Convert.ToBoolean((Boolean)expresion2.getValue(entorno, listas));
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Tipo de dato para operador \"^\" no valido Tipos: " +
                    Convert.ToString(expresion1.getType(entorno, listas)) + " y " + Convert.ToString(expresion2.getType(entorno, listas)) + " y se esperaba boolean"));
                return tipoDato.errorSemantico;
            }
        }
    }
}
