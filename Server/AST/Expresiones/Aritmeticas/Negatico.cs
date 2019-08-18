using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Aritmeticas
{
    class Negatico: Operacion, Expresion
    {
        public Negatico(int linea, int columna, Expresion expresion1)
            : base(linea, columna, expresion1)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            if (((tipoDato)expresion1.getType(entorno, listas) == tipoDato.decimall) || ((tipoDato)expresion1.getType(entorno, listas) == tipoDato.entero))
            {
                return (tipoDato)expresion1.getType(entorno, listas);
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            if ((tipoDato)expresion1.getType(entorno, listas) == tipoDato.decimall)
            {
                return -1.0 * Convert.ToDouble(expresion1.getValue(entorno, listas));

            }else if ((tipoDato)expresion1.getType(entorno, listas) == tipoDato.entero)
            {
                return -1 * Convert.ToInt32(expresion1.getValue(entorno, listas));
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Tipo de dato para operador \"-\" no valido Tipo: " +
                   Convert.ToString(expresion1.getType(entorno, listas)) + " y se esperaba Double o int"));
                return tipoDato.errorSemantico;
            }
        }
    }
}
