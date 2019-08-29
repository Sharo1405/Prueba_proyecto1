using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Instrucciones;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class LlamadaFuncion : Expresion
    {
        public String idFuncion { get; set; }
        public Expresion parametros { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        public LlamadaFuncion(String idFuncion, Expresion parametros, 
            int linea, int columna)
        {
            this.idFuncion = idFuncion.ToLower();
            this.parametros = parametros;
            this.linea = linea;
            this.columna = columna;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {

            parametrosLista.Clear();
            parametrosLista = new LinkedList<Comas>();
            String firmaFun = crearFirmaParas(entorno, listas);
            Simbolo buscado = entorno.get(firmaFun, entorno, Simbolo.Rol.FUNCION);
            if (buscado != null)
            {
                return buscado.tipo;
            }
            else
            {
                return tipoDato.errorSemantico;
            }            
        }


        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                String firmaFun = crearFirmaParas(entorno, listas);
                Simbolo buscado = entorno.get(firmaFun, entorno, Simbolo.Rol.FUNCION);
                if (buscado != null && buscado.rol == Simbolo.Rol.FUNCION)
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
                            listaparas.AddLast(new Comas(linea, columna, parametros));
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
                                if (buscado.tipo == r.getType(actual, listas))
                                {
                                    return r.getValue(actual, listas);
                                }
                            }
                        }
                        else
                        {//funciones 
                            Expresion exp = (Expresion)sentencia;
                            //exp.getValue(actual, listas);
                            if (exp is Retorno)
                            {
                                retexiste = true;
                                //exp.getValue(actual, listas);
                                if (buscado.tipo == exp.getType(actual, listas))
                                {
                                    return exp.getValue(actual, listas);
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                        "Retorno de funcion no es el mismo tipo de la funcion: Tipo funcion "
                                        + Convert.ToString(buscado.tipo) + "Tipo retorno: " + Convert.ToString(exp.getType(actual, listas))));
                                    return tipoDato.errorSemantico;
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
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                            "Retorno de funcion no EXISTE, funcion: " + firmaFun));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {

                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Funcion no existe: " + firmaFun));
                    return tipoDato.errorSemantico; 
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "NO se puede ejecutar llamada a funcion: " + idFuncion));
                return tipoDato.errorSemantico;
            }
            return tipoDato.errorSemantico;
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

        LinkedList<Comas> parametrosLista = new LinkedList<Comas>();

        public String crearFirmaParas(Entorno entorno, ErrorImpresion listas)
        {
            String firmaFuncion = "";
            firmaFuncion += idFuncion;
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
    }
}
