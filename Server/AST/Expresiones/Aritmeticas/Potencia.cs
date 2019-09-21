using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Aritmeticas
{
    class Potencia:Operacion, Expresion
    {
        public Potencia(int linea, int columna, Expresion expresion1, Expresion expresion2)
            : base(linea, columna, expresion1, expresion2)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return tipoResultante((tipoDato)expresion1.getType(entorno, listas, management), 
                (tipoDato)expresion2.getType(entorno, listas, management), entorno, listas);
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {         
            object exp = expresion1.getValue(entorno, listas, management);
            object exp2 = expresion2.getValue(entorno, listas, management);

            tipoDato nuevo = tipoResultante((tipoDato)expresion1.getType(entorno, listas, management), 
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

            if (nuevo == tipoDato.entero)
            {
                return Math.Pow(Convert.ToInt32(exp) , Convert.ToInt32(exp2));
            }
            else if (nuevo == tipoDato.decimall)
            {
                return Math.Pow(Convert.ToDouble(exp) , Convert.ToDouble(exp2));
            }
            else
            {                
                listas.impresiones.AddLast("WARNINGGGGGGGGGGGGGGGGGGGG!!!!!!!!!!! " +
                               " Tipo de dato para operador \"**\" no valido Tipos: " +
                     Convert.ToString(expresion1.getType(entorno, listas, management)) + " y " +
                     Convert.ToString(expresion2.getType(entorno, listas, management)) + " y se esperaba Int o Double " + " Linea/Columna: "
                                    + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                return TipoExcepcion.excep.ArithmeticException;
            }
        }
    }
}
