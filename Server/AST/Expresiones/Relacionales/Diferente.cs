using Server.AST.BaseDatos;
using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Relacionales
{
    class Diferente: Operacion, Expresion
    {
        public Diferente(int linea, int columna, Expresion expresion1, Expresion expresion2)
            : base(linea, columna, expresion1, expresion2)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            tipoDato nuevo = tipoResultanteRELACIONALES((tipoDato)expresion1.getType(entorno, listas, management), 
                (tipoDato)expresion2.getType(entorno, listas, management), entorno, listas);
            if (nuevo == tipoDato.decimall)
            {
                return tipoDato.booleano;
            }
            else if (nuevo == tipoDato.date)
            {
                return tipoDato.booleano;
            }
            else if (nuevo == tipoDato.time)
            {
                return tipoDato.booleano;
            }
            else if (nuevo == tipoDato.cadena)
            {
                return tipoDato.booleano;

            }
            else if (nuevo == tipoDato.nulo)
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

            tipoDato nuevo = tipoResultanteRELACIONALES((tipoDato)expresion1.getType(entorno, listas, management), 
                (tipoDato)expresion2.getType(entorno, listas, management), entorno, listas);
            
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

            if (nuevo == tipoDato.nulo)
            {
                tipoDato izq = (tipoDato)expresion1.getType(entorno, listas, management);
                tipoDato der = (tipoDato)expresion2.getType(entorno, listas, management);

                if (izq == tipoDato.nulo)
                {
                    if (der == tipoDato.decimall || der == tipoDato.date || der == tipoDato.time || der == tipoDato.cadena
                        || der == tipoDato.booleano)
                    {
                        return false;
                    }
                    else if (der == tipoDato.nulo)
                    {
                        return true;
                    }
                    else
                    {
                        return tipoDato.errorSemantico;
                    }

                }
                else if (der == tipoDato.nulo)
                {
                    if (izq == tipoDato.decimall || izq == tipoDato.date || izq == tipoDato.time || izq == tipoDato.cadena
                        || izq == tipoDato.booleano)
                    {
                        return false;
                    }
                    else if (der == tipoDato.nulo)
                    {
                        return true;
                    }
                    else
                    {
                        return tipoDato.errorSemantico;
                    }
                }
            }
            if (nuevo == tipoDato.decimall)
            {
                return Convert.ToDouble(exp) != Convert.ToDouble(exp2);
            }
            else if (nuevo == tipoDato.date)
            {
                return Convert.ToDateTime(exp) !=
                       Convert.ToDateTime(exp2);
            }
            else if (nuevo == tipoDato.time)
            {
                return Convert.ToDateTime(exp) !=
                       Convert.ToDateTime(exp2);
            }
            else if (nuevo == tipoDato.cadena)
            {
                return !Convert.ToString(exp)
                    .Equals(Convert.ToString(exp2));
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Tipo de dato para operador \"==\" no valido Tipos: " +
                    Convert.ToString(expresion1.getType(entorno, listas, management)) + " y " + 
                    Convert.ToString(expresion2.getType(entorno, listas, management)) + " y se esperaba Int o Double, Strings, dates, times"));
                return tipoDato.errorSemantico;
            }
            
        }
    }
}
