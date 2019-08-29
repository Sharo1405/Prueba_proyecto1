using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class LlamadaProcedimiento : Expresion
    {
        public String idProc { get; set; }
        public Expresion parametros { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public LlamadaProcedimiento(String idProc,
            Expresion parametros, int linea, int col)
        {
            this.idProc = idProc;
            this.parametros = parametros;
            this.linea = linea;
            this.col = col;
        }

        public LlamadaProcedimiento(String idProc,
            int linea, int col)
        {
            this.idProc = idProc;
            this.parametros = parametros;
            this.linea = linea;
            this.col = col;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return Operacion.tipoDato.list;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                //AQUI TODA LA EJECUCION
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "NO se puede ejecutar llamada a funcion: " + idFuncion));
                return tipoDato.errorSemantico;
            }
            return tipoDato.errorSemantico;
        }
    }
}
