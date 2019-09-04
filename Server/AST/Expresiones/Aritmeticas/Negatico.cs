using Server.AST.BaseDatos;
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

        public tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            if (((tipoDato)expresion1.getType(entorno, listas, management) == tipoDato.decimall) || 
                ((tipoDato)expresion1.getType(entorno, listas, management) == tipoDato.entero))
            {
                return (tipoDato)expresion1.getType(entorno, listas, management);
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            object exp = expresion1.getValue(entorno, listas, management);            
            if (expresion1 is ArrobaId)
            {
                Simbolo ar = (Simbolo)exp;
                exp = ar.valor;
            }            
            if ((tipoDato)expresion1.getType(entorno, listas, management) == tipoDato.decimall)
            {
                return -1.0 * Convert.ToDouble(exp);

            }else if ((tipoDato)expresion1.getType(entorno, listas, management) == tipoDato.entero)
            {
                return -1 * Convert.ToInt32(exp);
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Tipo de dato para operador \"-\" no valido Tipo: " +
                   Convert.ToString(expresion1.getType(entorno, listas, management)) + " y se esperaba Double o int"));
                return tipoDato.errorSemantico;
            }
        }
    }
}
