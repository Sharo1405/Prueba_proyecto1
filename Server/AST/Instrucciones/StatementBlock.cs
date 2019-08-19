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
               
                if (sentencia is Instruccion)
                {
                    Instruccion ins = (Instruccion)sentencia;                   
                    if (ins is Breakk)
                    {
                        return ins;
                    }
                    else if (ins is Continuee)
                    {
                        return ins;
                    }
                    else
                    {
                        ins.ejecutar(entorno, listas);
                    }
                }
                else
                {//funciones 
                    Expresion exp = (Expresion)sentencia;                   
                    if (exp is Retorno)
                    {
                        //return exp.getValue(entorno, listas);
                        return exp;
                    }
                    else
                    {
                        exp.getValue(entorno, listas);
                    }                    
                }
            }
            return Operacion.tipoDato.ok;
        }
    }
}
