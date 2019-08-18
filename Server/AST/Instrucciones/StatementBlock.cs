using Server.AST.Entornos;
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
            throw new NotImplementedException();
        }
    }
}
