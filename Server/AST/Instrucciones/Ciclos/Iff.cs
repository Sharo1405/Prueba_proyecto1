using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones.Ciclos
{
    class Iff : Instruccion
    {
        public Instruccion ejecutarELSE { get; set; }
        public LinkedList<IfLista> ejecutarIFS = new LinkedList<IfLista>();
        public int linea { get; set; }
        public int col { get; set; }

        protected Boolean entro = false;

        public Iff(Instruccion ejecutarELSE, LinkedList<IfLista> ejecutarIFS, int linea, int col)
        {
            this.ejecutarELSE = ejecutarELSE;
            this.ejecutarIFS = ejecutarIFS;
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                entro = false;
                foreach (IfLista ifLista in ejecutarIFS)
                {
                    Object ob = ifLista.condicion.getValue(entorno, listas, management);
                    tipoDato tipo = ifLista.condicion.getType(entorno, listas, management);
                    if (tipo == tipoDato.booleano)
                    {
                        Boolean prueba = (Boolean)ob;
                        if (prueba)
                        {
                            entro = true;
                            foreach (NodoAST nodo in ((LinkedList<NodoAST>)ifLista.listaParaEjecutar.listaIns))
                            {
                                if (nodo is Instruccion)
                                {
                                    Instruccion ins = (Instruccion)nodo;
                                    if (ins is Breakk)
                                    {
                                        return ins;
                                    }
                                    else if (ins is Continuee)
                                    {
                                        return ins;
                                    }
                                    else if (ins is Retorno)
                                    {
                                        return ins;
                                    }
                                    else
                                    {
                                        object aal = ins.ejecutar(entorno, listas, management);
                                        if (aal is TipoExcepcion.excep)
                                        {
                                            return aal;
                                        }
                                    }
                                }
                                else
                                {//funciones 
                                    Expresion exp = (Expresion)nodo;
                                    if (exp is Retorno)
                                    {
                                        return exp;//.getValue(entorno, listas);
                                    }
                                    else
                                    {
                                        object aal = exp.getValue(entorno, listas, management);
                                        if (aal is TipoExcepcion.excep)
                                        {
                                            return aal;
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, col, NodoError.tipoError.Semantico,
                        "Condicion de if no es tipo Boolean, NO ES VALIDA"));
                    }
                    //break;
                }

                if (entro == false && ejecutarELSE != null)
                {

                    Object reto = ejecutarELSE.ejecutar(entorno, listas, management);

                    if (reto is Breakk)
                    {
                        return reto;
                    }
                    else if (reto is Continuee)
                    {
                        return reto;
                    }
                    else if (reto is Retorno)
                    {
                        return reto;
                    }
                }
                return tipoDato.ok;
            }
            catch (Exception e)
            {

            }
            return tipoDato.errorSemantico;
        }
    }
}
