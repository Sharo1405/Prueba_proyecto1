using Server.AST.BaseDatos;
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


        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
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
                        /*else if(ins is ){  //es para evitar que vengan user types, funciones 

                        }*/
                        else if (ins is Retorno)
                        {
                            return ins;
                        }                       
                        else
                        {
                            object ss = ins.ejecutar(entorno, listas, management);
                            if (ss is TipoExcepcion.excep)
                            {
                                return ss;
                            }
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
                            object sss = exp.getValue(entorno, listas, management);
                            if (sss is TipoExcepcion.excep)
                            {
                                return sss;
                            }
                        }
                    }
                }
                return Operacion.tipoDato.ok;
            }
            catch (Exception e)
            {

            }            
            return Operacion.tipoDato.errorSemantico;
        }
    }
}
