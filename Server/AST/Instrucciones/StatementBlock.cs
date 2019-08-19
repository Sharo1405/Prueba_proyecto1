using Server.AST.Entornos;
using Server.AST.Expresiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Instrucciones
{
    class StatementBlock : Instruccion
    {

        public LinkedList<NodoAST> listaIns;

        public StatementBlock(LinkedList<NodoAST> lista)
        {

            this.listaIns = lista;

        }


        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            foreach (NodoAST sentencia in listaIns)
            {
                Instruccion eject = (Instruccion)sentencia;
                if (eject is Retorno)
                {
                    return eject.ejecutar(entorno, listas);
                }
                else if (eject is Breakk)
                {
                    return eject.ejecutar(entorno, listas);
                }                
                else
                {
                    eject.ejecutar(entorno, listas);
                }
            }
            return Operacion.tipoDato.ok;
        }
    }
}
