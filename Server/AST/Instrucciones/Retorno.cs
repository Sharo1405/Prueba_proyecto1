using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Expresiones;


namespace Server.AST.Instrucciones
{
    class Retorno : Expresion
    {
        public Expresion retorno { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public Retorno()
        {
        }

        public Retorno(Expresion retorno, int linea, int col)
        {
            this.retorno = retorno;
            this.linea = linea;
            this.col = col;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            if (retorno != null)
            {
                return Operacion.tipoDato.errorSemantico;
            }
            else {
                return retorno.getType(entorno, listas);
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            if (retorno != null)
            {
                return Operacion.tipoDato.errorSemantico;
            }
            else
            {
                return retorno.getValue(entorno, listas);
            }            
        }
    }
}
