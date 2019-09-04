using Server.AST.BaseDatos;
using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Logicas
{
    class And:Operacion, Expresion
    {
        public And(int linea, int columna, Expresion expresion1, Expresion expresion2)
            : base(linea, columna, expresion1, expresion2)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {            
            if (((tipoDato)expresion1.getType(entorno, listas, management) == tipoDato.booleano) && 
                ((tipoDato)expresion2.getType(entorno, listas, management) == tipoDato.booleano))
            {
                return tipoDato.booleano;
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            object exp = expresion1.getValue(entorno, listas, management);
            object exp2 = expresion2.getValue(entorno, listas, management);

            if (((tipoDato)expresion1.getType(entorno, listas, management) == tipoDato.booleano) &&
                ((tipoDato)expresion2.getType(entorno, listas, management) == tipoDato.booleano))
            {
                
                if (expresion1 is ArrobaId)
                {
                    Simbolo ar = (Simbolo)exp;
                    exp = ar.valor;
                }
                if (expresion2 is ArrobaId)
                {
                    Simbolo ar = (Simbolo)exp2;
                    exp2 = ar.valor;
                }
                return (Boolean)exp && (Boolean)exp2;
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Tipo de dato para operador \"&&\" no valido Tipos: " +
                    Convert.ToString(expresion1.getType(entorno, listas, management)) + " y " + 
                    Convert.ToString(expresion2.getType(entorno, listas, management)) + " y se esperaba boolean"));
                return tipoDato.errorSemantico;
            }
        }
    }
}
