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

        public Retorno(int linea, int col)
        {
            this.linea = linea;
            this.col = col;
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
                return retorno.getType(entorno, listas);
            }
            else {
                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                    "EL RETURN NO RETORNA NADA, por eso retornara un error en el tipo"));
                return Operacion.tipoDato.errorSemantico;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            if (retorno != null)
            {
                return retorno.getValue(entorno, listas);
            }
            else
            {
                return "";
            }            
        }
    }
}
