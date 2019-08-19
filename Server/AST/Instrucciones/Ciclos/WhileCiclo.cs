using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Ciclos
{
    class WhileCiclo : Instrucciones.Instruccion
    {

        public Expresion condicion { get; set; }
        public StatementBlock sentencias { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public WhileCiclo()
        {

        }

        public WhileCiclo(Expresion cond, StatementBlock sentencias,
            int linea, int columna)
        {
            this.condicion = cond;
            this.sentencias = sentencias;
        }

        public Object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            Object valor = condicion.getValue(entorno, listas);
            tipoDato tipo = condicion.getType(entorno, listas);
            if (tipoDato.booleano == tipo)
            {
                while ((Boolean) condicion.getValue(entorno, listas))
                {
                    Object retorno = sentencias.ejecutar(entorno, listas);

                    if (retorno is Breakk) {
                        break;
                    } else if (retorno is Continuee){
                        continue;
                    } else if (retorno is Retorno) {
                        return retorno;
                    }
                }
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna,
                    NodoError.tipoError.Semantico, "No se puede imprimir ese Argumento"));
                return tipoDato.errorSemantico;
            }
            return valor;
        }
    }
}
