using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
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

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Object ob = condicion.getValue(entorno, listas, management);
                do
                {
                    Boolean reiniciar = false;
                    Object retorno = sentenciasEjecutar.ejecutar(entorno, listas, management);

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
                    else if (retorno is TipoExcepcion.excep)
                    {
                        return retorno;
                    }

                    //-----------------------------------------------------------------------------------------------
                    ob = condicion.getValue(entorno, listas, management);
                    if (ob == null)
                    {
                        listas.impresiones.AddLast("WARNINGGGGGGGGGGGGGGGGGGGG!!!!!!!!!!! " + " Linea/Columna " +
                                   Convert.ToString(this.linea) + " / " + Convert.ToString(this.col));
                        return TipoExcepcion.excep.NullPointerException;
                    }

                    tipoDato tipo = condicion.getType(entorno, listas, management);
                    if (tipo == tipoDato.booleano)
                    {
                        //todo cool
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, col, NodoError.tipoError.Semantico, 
                            "La condicion no es valida para el DoWhile: "
                            + Convert.ToString(tipo) + " Linea/Columna " 
                            + Convert.ToString(this.linea) + " " + Convert.ToString(this.col)));
                        return tipoDato.errorSemantico;
                    }
                    //-----------------------------------------------------------------------------------------------

                } while ((Boolean)ob);

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
