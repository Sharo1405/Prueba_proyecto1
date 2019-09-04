using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class Ternario : Expresion
    {
        private Expresion condicion;
        private Expresion valorVerdadero;
        private Expresion valorFalso;
        private int linea;
        private int columna;

        public Ternario(Expresion condicion, Expresion valorVerdadero, Expresion valorFalso, int linea, int columna)
        {
            this.condicion = condicion;
            this.valorVerdadero = valorVerdadero;
            this.valorFalso = valorFalso;
            this.linea = linea;
            this.columna = columna;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {

            Object cndicion = condicion.getValue(entorno, listas, management);
            if (cndicion != null)
            {
                tipoDato tipoo = condicion.getType(entorno, listas, management);
                if (tipoo == tipoDato.booleano)
                {
                    if ((Boolean)cndicion)
                    {
                        valorVerdadero.getValue(entorno, listas, management);
                        return valorVerdadero.getType(entorno, listas, management);
                    }
                    else
                    {
                        valorFalso.getValue(entorno, listas, management);
                        return valorFalso.getType(entorno, listas, management);
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                        "El tipo de la condicion no es booleana sino: " + Convert.ToString(tipoo) +
                        " Por lo que no se puede realizar el ternario"));
                    return tipoDato.errorSemantico;
                }
            }
            return tipoDato.errorSemantico;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Object cndicion = condicion.getValue(entorno, listas, management);
                if (cndicion != null)
                {
                    tipoDato tipoo = condicion.getType(entorno, listas, management);
                    if (tipoo == tipoDato.booleano)
                    {
                        if ((Boolean)cndicion)
                        {
                            return valorVerdadero.getValue(entorno, listas, management);
                        }
                        else
                        {
                            return valorFalso.getValue(entorno, listas, management);
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "El tipo de la condicion no es booleana sino: " + Convert.ToString(tipoo) + 
                            " Por lo que no se puede realizar el ternario"));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "la condicion es nula no se puede realizar el ternario"));
                    return tipoDato.errorSemantico;
                }
                return null;
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                             "NO se puede realizar la operacion ternaria"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.errorSemantico; ;
        }
    }
}
