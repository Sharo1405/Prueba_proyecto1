using Server.AST.Expresiones;
using Server.AST.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Ciclos
{
    class WhileCiclo : Instrucciones.Instruccion
    {

        public Expresion condicion { get; set; }
        public LinkedList<NodoAST> sentencias { get; set; }

        public WhileCiclo()
        {

        }

        public WhileCiclo(Expresion cond, LinkedList<NodoAST> sentencias)
        {
            this.condicion = cond;
            this.sentencias = sentencias;
        }

        public Object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            Object valor = condicion.getValue(entorno, listas);
            Object tipo = condicion.getType(entorno, listas);
            foreach (var item in sentencias)
            {
                if (item is Instruccion)
                {
                    Instruccion ins = (Instruccion)item;
                    ins.ejecutar(entorno, listas);
                }
            }
            return valor;
        }
    }
}
