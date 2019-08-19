﻿using System;
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
            this.idFuncion = idFuncion;
            this.parametros = parametros;
            this.linea = linea;
            this.columna = columna;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            Simbolo buscado = entorno.get(idFuncion, entorno);
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
                Simbolo buscado = entorno.get(firmaFun, entorno);
                if (buscado != null && buscado.rol == Simbolo.Rol.FUNCION)
                {
                    StatementBlock sentenciasEject = (StatementBlock)buscado.valor;
                    Retorno retorno = new Retorno();
                    Boolean retexiste = false;
                    foreach (var item in sentenciasEject.listaIns)
                    {
                        if(item is Retorno)
                        {
                            retexiste = true;
                            break;
                        }
                    }

                    Entorno actual = new Entorno(entorno);
                    if (parametros != null)
                    {
                        declararParametros(actual, listas, buscado.parametros, (LinkedList<Comas>)parametros.getValue(entorno, listas));
                    }
                    else
                    {
                        //no hace nada
                    }

                    if (retexiste == true) {
                        //retorno = (Retorno)sentenciasEject.ejecutar(entorno, listas);

                        foreach (NodoAST sentencia in sentenciasEject.listaIns)
                        {
                            if (sentencia is Instruccion)
                            {
                                Instruccion ins = (Instruccion)sentencia;
                                ins.ejecutar(entorno, listas);
                            }
                            else
                            {//funciones 
                                Expresion exp = (Expresion)sentencia;
                                exp.getValue(entorno, listas);
                                if (exp is Retorno)
                                {
                                    retorno = (Retorno) exp.getValue(entorno, listas);
                                }                                
                                else
                                {
                                    exp.getValue(entorno, listas);
                                }
                            }                            
                        }

                        if (buscado.tipo == retorno.getType(entorno, listas))
                        {
                            return retorno.getValue(entorno, listas);
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Retorno de funcion no es el mismo tipo de la funcion: Tipo funcion "
                                + Convert.ToString(buscado.tipo) + "Tipo retorno: " + Convert.ToString(retorno.getType(entorno, listas))));
                            return tipoDato.errorSemantico;
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No existe el retorno en la funcion: " + firmaFun));
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
                return tipoDato.errorSemantico;
            }
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

                if (valor is LlamadaFuncion)//llamada a funcion en el parametro
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

        public String crearFirmaParas(Entorno entorno, ErrorImpresion listas)
        {
            String firmaFuncion = "";
            firmaFuncion += idFuncion;
            if (parametros != null) {
                LinkedList<Comas> param = (LinkedList<Comas>)parametros.getValue(entorno, listas);
                foreach (Expresion parametro in param)
                {
                    tipoDato tipoPara = parametro.getType(entorno, listas);
                    firmaFuncion += "_" + Convert.ToString(tipoPara);
                }
            }
            return firmaFuncion;
        }
    }
}
