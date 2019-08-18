using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Aritmeticas
{
    class Modulo: Operacion, Expresion
    {
        public Modulo(int linea, int columna, Expresion expresion1, Expresion expresion2)
            : base(linea, columna, expresion1, expresion2)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipoResultante((tipoDato)expresion1.getType(entorno, listas), (tipoDato)expresion2.getType(entorno, listas), entorno, listas);
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            tipoDato nuevo = tipoResultante((tipoDato)expresion1.getType(entorno, listas), (tipoDato)expresion2.getType(entorno, listas), entorno, listas);
            if (nuevo == tipoDato.entero)
            {
                return Convert.ToInt32(expresion1.getValue(entorno, listas)) % Convert.ToInt32(expresion2.getValue(entorno, listas));
            }
            else if (nuevo == tipoDato.decimall)
            {
                return Convert.ToDouble(expresion1.getValue(entorno, listas)) % Convert.ToDouble(expresion2.getValue(entorno, listas));
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Tipo de dato para operador \"%\" no valido Tipos: " +
                    Convert.ToString(expresion1.getType(entorno, listas)) + " y " + Convert.ToString(expresion2.getType(entorno, listas)) + " y se esperaba Int o Double"));
                return tipoDato.errorSemantico;
            }
        }
    }
}
