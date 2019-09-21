using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones.TipoDato;
using static Server.AST.Expresiones.Operacion;
using static Server.AST.Instrucciones.TipoExcepcion;

namespace Server.AST.Instrucciones
{
    class TryCatchEjecutar : Instruccion
    {
        public StatementBlock sentenciasTRY { get; set; }
        public excep expcionCaturada { get; set; }
        public String idVarExcepcion { get; set; }
        public StatementBlock sentenciasCATCH { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        public TryCatchEjecutar(StatementBlock sentenciasTRY, excep expcionCaturada,
            String idVarExcepcion, StatementBlock sentenciasCATCH,
            int line, int col)
        {
            this.sentenciasTRY = sentenciasTRY;
            this.expcionCaturada = expcionCaturada;
            this.idVarExcepcion = idVarExcepcion;
            this.sentenciasCATCH = sentenciasCATCH;
            this.linea = line;
            this.columna = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                object devueltoTRY = sentenciasTRY.ejecutar(entorno, listas, management);
                if (devueltoTRY is Retorno)
                {
                    return devueltoTRY;
                }
                else if (devueltoTRY is Continuee)
                {
                    return devueltoTRY;
                }
                else if (devueltoTRY is Breakk)
                {
                    return devueltoTRY;
                }
                else if (devueltoTRY is TipoExcepcion.excep)
                {
                    tryCathMetodoEjecutar((TipoExcepcion.excep)devueltoTRY, entorno, listas, management);
                }


            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                    "No se puede Ejecutar el tryCatch"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }



        public object tryCathMetodoEjecutar(TipoExcepcion.excep excepcion, Entorno entorno, ErrorImpresion listas, Administrador management)
        {

            if (excepcion == expcionCaturada)
            {
                //este seria el catch
                LinkedList<String> listaidsvar = new LinkedList<string>();
                listaidsvar.AddLast(this.idVarExcepcion.ToLower());
                DeclaracionAsignacion declaraElExcepcion = new DeclaracionAsignacion(new Otras.Tipo(tipoDato.excepcion, this.linea, this.columna),listaidsvar,
                    new cadena("EXCEPXION..." + Convert.ToString(expcionCaturada), tipoDato.excepcion, this.linea, this.columna));

                declaraElExcepcion.ejecutar(entorno, listas, management);

                object devueltoCATCH = sentenciasCATCH.ejecutar(entorno, listas, management);
                if (devueltoCATCH is Retorno)
                {
                    return devueltoCATCH;
                }
                else if (devueltoCATCH is Continuee)
                {
                    return devueltoCATCH;
                }
                else if (devueltoCATCH is Breakk)
                {
                    return devueltoCATCH;
                }
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                    "La excepcion del trow del catch no es la misma, la del catch es:" + Convert.ToString(expcionCaturada)));
                return tipoDato.errorSemantico;
            }

            return tipoDato.ok;
        }
    }
}
