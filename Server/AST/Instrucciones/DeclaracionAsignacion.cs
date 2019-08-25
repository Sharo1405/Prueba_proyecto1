﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class DeclaracionAsignacion : Instruccion
    {

        public Tipo tipo { get; set; }
        public LinkedList<String> ids = new LinkedList<string>();
        public Expresion valor { get; set; }


        public DeclaracionAsignacion(Tipo tipo, LinkedList<String> ids, Expresion valor)
        {
            this.tipo = tipo;
            this.ids = ids;
            this.valor = valor;
        }


        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try {
                foreach (String id in ids)
                {
                    Simbolo buscado = entorno.getEnActual(id.ToLower(), Simbolo.Rol.VARIABLE);
                    if (buscado == null)
                    {
                        tipoDato tipoValor = valor.getType(entorno, listas);
                        object value = valor.getValue(entorno, listas);
                        if (tipo.tipo == tipoValor)
                        {
                            entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), value, tipo.linea, tipo.columna,
                                tipo.tipo, Simbolo.Rol.VARIABLE));
                        }
                        else
                        {
                            if (tipo.tipo == tipoDato.entero && tipoValor == tipoDato.decimall)
                            {
                                //se redondea el valor
                                double nuevo = Math.Floor(Double.Parse(Convert.ToString(value)));
                                Int32 ainsertar = Int32.Parse(Convert.ToString(nuevo));
                                entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), ainsertar, tipo.linea, tipo.columna,
                                tipo.tipo, Simbolo.Rol.VARIABLE));
                            }
                            else if (tipo.tipo == tipoDato.decimall && tipoValor == tipoDato.entero)
                            {
                                entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), Double.Parse(Convert.ToString(value)), tipo.linea, tipo.columna,
                                tipo.tipo, Simbolo.Rol.VARIABLE));
                            }
                            else if (tipo.tipo == tipoDato.id && valor is Neww)
                            {
                                Simbolo sim = entorno.get(tipo.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                if (sim != null) {
                                    if (sim.valor is CreateType)
                                    {
                                        CreateType s = (CreateType)sim.valor;
                                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), s.Clone(), tipo.linea, tipo.columna,
                                            tipo.tipo, Simbolo.Rol.VARIABLE));
                                    }
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(tipo.linea, tipo.columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO EXISTE. Tipos en cuestion: " + Convert.ToString(tipo.id)));
                                    return tipoDato.errorSemantico;
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(tipo.linea, tipo.columna, NodoError.tipoError.Semantico,
                                    "Los tipos no coinciden para la variable. Tipos en cuestion: " + Convert.ToString(tipo.tipo) + 
                                    Convert.ToString(tipoValor)));
                                return tipoDato.errorSemantico;
                            }
                        }

                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(tipo.linea, tipo.columna, NodoError.tipoError.Semantico,
                            "Variable ya declara en el entorno actual. El nombre es: " + id));
                        return tipoDato.errorSemantico;
                    }
                }
                return tipoDato.ok;
            } catch (Exception e) {
                listas.errores.AddLast(new NodoError(tipo.linea, tipo.columna, NodoError.tipoError.Semantico,
                "Asignacion no valida"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.id;
        }
    }
}
