using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class LlamadaProcedimiento : Expresion
    {
        public String idProc { get; set; }
        public Expresion parametros { get; set; }
        public int linea { get; set; }
        public int col { get; set; }


        LinkedList<Comas> parametrosLista = new LinkedList<Comas>();

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


        public String crearFirmaParas(Entorno entorno, ErrorImpresion listas)
        {
            String firmaFuncion = "";
            firmaFuncion += idProc;
            if (parametros != null)
            {
                object v = parametros.getValue(entorno, listas);
                if (v is LinkedList<Comas>)
                {
                    parametrosLista = (LinkedList<Comas>)v;
                    foreach (Expresion parametro in parametrosLista)
                    {
                        tipoDato tipoPara = parametro.getType(entorno, listas);
                        firmaFuncion += "_" + Convert.ToString(tipoPara);
                    }
                }
                else
                {
                    tipoDato tipoPara = parametros.getType(entorno, listas);
                    firmaFuncion += "_" + Convert.ToString(tipoPara);
                }
            }
            return firmaFuncion;
        }



        public void declararParametros(Entorno lista, ErrorImpresion impresion,
            LinkedList<Parametros> parametros, LinkedList<Comas> valoresParametros)
        {
            int cantidad = parametros.Count;
            for (int i = 0; i < cantidad; i++)
            {

                Parametros var = parametros.ElementAt(i); //tipo, nombre, linea, columna
                Comas valor = valoresParametros.ElementAt(i); //valor, linea, columna

                String nombreVar = var.id;
                Expresion value = valor.expresion1;

                if (value is LlamadaFuncion)//llamada a funcion en el parametro
                {
                    Object val = value.getValue(lista.padreANTERIOR, impresion);
                    lista.setSimbolo(nombreVar, new Simbolo(nombreVar, val, valor.linea, valor.columna,
                        var.tipo.tipo, Simbolo.Rol.VARIABLE));
                }
                else
                {
                    Object val = value.getValue(lista, impresion);
                    lista.setSimbolo(nombreVar, new Simbolo(nombreVar, val, valor.linea, valor.columna,
                        var.tipo.tipo, Simbolo.Rol.VARIABLE));
                }
            }            
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                //AQUI TODA LA EJECUCION
                String firmaFun = crearFirmaParas(entorno, listas);
                Simbolo buscado = entorno.get(firmaFun, entorno, Simbolo.Rol.PROCEDIMIENTO);
                if (buscado != null && buscado.rol == Simbolo.Rol.PROCEDIMIENTO)
                {
                    StatementBlock sentenciasEject = (StatementBlock)buscado.valor;
                    Boolean retexiste = false;

                    Entorno actual = new Entorno(entorno);
                    if (parametros != null)
                    {

                        //object v = parametros.getValue(actual, listas);
                        if (parametrosLista.Count > 0)
                        {
                            declararParametros(actual, listas, buscado.parametros, parametrosLista);
                        }
                        else
                        {
                            LinkedList<Comas> listaparas = new LinkedList<Comas>();
                            listaparas.AddLast(new Comas(linea, col, parametros));
                            declararParametros(actual, listas, buscado.parametros, listaparas);
                        }
                    }
                    else
                    {
                        //no hace nada
                    }

                    foreach (NodoAST sentencia in sentenciasEject.listaIns)
                    {
                        if (sentencia is Instruccion)
                        {
                            Instruccion ins = (Instruccion)sentencia;
                            Object reto = ins.ejecutar(actual, listas);
                            if (ins is Breakk)
                            {
                                return ins;
                            }
                            else if (ins is Continuee)
                            {
                                return ins;
                            }
                            else if (reto is Retorno)
                            {
                                retexiste = true;
                                Expresion r = (Expresion)reto;
                                Retorno rr = (Retorno)r;
                                
                                if (rr.retorno is ListaExpresiones)
                                {
                                    
                                    Corchetes haciendoLista = new Corchetes(rr.retorno, rr.linea, rr.col);
                                    List<Object> listRetornada = (List<Object>)haciendoLista.getValue(entorno, listas);
                                    tipoDato t = rr.getType(entorno, listas);
                                    Lista listaGuardar = new Lista("", listRetornada, tipoDato.list, t, linea, col);
                                    return listaGuardar;
                                }
                                else
                                {
                                    List<Object> listRetornada = new List<object>();
                                    Object ob = rr.getValue(entorno, listas);
                                    listRetornada.Add(ob);
                                    tipoDato t = rr.getType(entorno, listas);
                                    Lista listaGuardar = new Lista("", listRetornada, tipoDato.list, t, linea, col);
                                    return listaGuardar;
                                }                                                               
                            }
                        }
                        else
                        {//funciones 
                            Expresion exp = (Expresion)sentencia;
                            if (exp is Retorno)
                            {
                                retexiste = true;
                                Expresion r = (Expresion)exp;
                                Retorno rr = (Retorno)r;
                                if (rr.retorno is ListaExpresiones)
                                {

                                    Corchetes haciendoLista = new Corchetes(rr.retorno, rr.linea, rr.col);
                                    List<Object> listRetornada = (List<Object>)haciendoLista.getValue(entorno, listas);
                                    tipoDato t = rr.getType(entorno, listas);
                                    Lista listaGuardar = new Lista("", listRetornada, tipoDato.list, t, linea, col);
                                    return listaGuardar;
                                }
                                else
                                {
                                    List<Object> listRetornada = new List<object>();
                                    Object ob = rr.getValue(entorno, listas);
                                    listRetornada.Add(ob);
                                    tipoDato t = rr.getType(entorno, listas);
                                    Lista listaGuardar = new Lista("", listRetornada, tipoDato.list, t, linea, col);
                                    return listaGuardar;
                                }
                            }
                            else
                            {
                                exp.getValue(actual, listas);
                            }
                        }
                    }

                    if (retexiste == false)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                            "Retorno de funcion no EXISTE, funcion: " + firmaFun));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {

                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico, "Funcion no existe: " + firmaFun));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico, "NO se puede ejecutar llamada a funcion: " + idProc));
                return tipoDato.errorSemantico;
            }
            return tipoDato.errorSemantico;
        }
    }
}
