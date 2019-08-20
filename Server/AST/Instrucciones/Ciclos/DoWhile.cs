using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones.Ciclos
{
    class DoWhile : Instruccion
    {
        public Instruccion sentenciasEjecutar { get; set; }
        public Expresion condicion { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public DoWhile(Instruccion sentenciasEjecutar, Expresion condicion, int linea, int col)
        {
            this.sentenciasEjecutar = sentenciasEjecutar;
            this.condicion = condicion;
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                do
                {
                    Boolean reiniciar = false;
                    Object retorno = sentenciasEjecutar.ejecutar(entorno, listas);

                    if (retorno is Breakk)
                    {
                        break;
                    }
                    else if (retorno is Continuee)
                    {
                        continue;
                    }
                    else if (retorno is Retorno)
                    {
                        return retorno;
                    }

                    //-----------------------------------------------------------------------------------------------
                    tipoDato tipo = condicion.getType(entorno, listas);
                    if (tipo == tipoDato.booleano)
                    {
                        //todo cool
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, col, NodoError.tipoError.Semantico, "La condicion no es valida para el DoWhile: "
                            + Convert.ToString(tipo)));
                        return tipoDato.errorSemantico;
                    }
                    //-----------------------------------------------------------------------------------------------

                } while ((Boolean)condicion.getValue(entorno, listas));

                return tipoDato.ok;
            }
            catch (Exception e)
            {

            }
            listas.errores.AddLast(new NodoError(linea, col, NodoError.tipoError.Semantico, "La condicion no es valida para el DoWhile"));
            return tipoDato.errorSemantico;
        }
    }
}
