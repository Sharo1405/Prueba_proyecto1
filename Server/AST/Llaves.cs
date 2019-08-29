using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST
{
    class Llaves: Expresion
    {
        public Expresion expresion { get; set; } //debe ser lista expresiones
        public int linea { get; set; }
        public int columna { get; set; }

        int contador = 0;

        public Llaves(Expresion expresion, int linea, int columna)
        {
            this.expresion = expresion;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            if (expresion is ListaExpresiones)
            {
                /*listaRetorno = new List<object>();
                listaRetorno.Clear();
                getValue(entorno, listas);
                contador = 0;
                listaRetorno = new List<object>();*/
                return tipoValor;
            }
            else
            {
                return expresion.getType(entorno, listas);
            }
        }
        
        tipoDato tipoValor = tipoDato.errorSemantico;
        tipoDato tipoValorAnterior = tipoDato.errorSemantico;
        List<Object> listaRetorno = new List<object>();


        public CreateType CreaNuevoType(CreateType existente, Entorno entorno, ErrorImpresion listas)
        {
            CreateType nuevo = new CreateType();
            nuevo.idType = existente.idType;
            nuevo.ifnotexists = existente.ifnotexists;
            nuevo.linea = existente.linea;
            nuevo.columna = existente.columna;

            foreach (itemType var in existente.itemTypee)
            {
                itemType i = new itemType();
                i.id = var.id;
                i.tipo = var.tipo;
                i.valor = new Object();
                if (var.tipo.tipo == tipoDato.id)
                {
                    Simbolo buscado2 = entorno.get(var.tipo.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                    if (buscado2 != null)
                    {
                        //arreglar solo declarar sin el clonar
                        //CreateType typeComoTipo =(CreateType) buscado2.valor;
                        i.valor = null;//typeComoTipo.Clone();
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "El tipo de esa variable del Type User no existe: Tipo:" + var.tipo.id.ToLower()));
                    }
                }
                else if (var.tipo.tipo == tipoDato.entero)
                {
                    var.valor = 0;
                }
                else if (var.tipo.tipo == tipoDato.decimall)
                {
                    var.valor = 0.0;
                }
                else if (var.tipo.tipo == tipoDato.booleano)
                {
                    var.valor = false;
                }
                else
                {
                    var.valor = null;
                }

                nuevo.itemTypee.AddLast(i);
            }
            return nuevo;
        }


        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                if (expresion is ListaExpresiones)
                {
                    LinkedList<Comas> objeto = (LinkedList<Comas>)expresion.getValue(entorno, listas); //Lista comas

                    foreach (Comas coma in objeto)
                    {
                        Comas cv2 = (Comas)coma;
                        Object valo = cv2.getValue(entorno, listas);
                        tipoDato tipo = cv2.getType(entorno, listas);                        

                        if (cv2.expresion1 is Llaves)
                        {                            
                            if (valo is List<object>)
                            {
                                //listaRetorno = (List<object>)valo;
                                Lista listaGuardar = new Lista("item", (List<object>)valo, tipoDato.set, tipo, linea, columna);
                                listaRetorno.Add(listaGuardar);
                            }
                            else
                            {
                                //listaRetorno.Add(listaRetorno);
                                List<Object> solo1 = new List<object>();
                                solo1.Add(valo);
                                Lista listaGuardar = new Lista("item", solo1, tipoDato.set, tipo, linea, columna);
                                listaRetorno.Add(listaGuardar);
                            }
                            tipo = tipoDato.set;
                        }
                        else if (cv2.expresion1 is LLaveAsTypeUser)
                        {                            
                            listaRetorno.Add(valo); //guardando un Type User
                            tipo = tipoDato.id;
                        }
                        else if (cv2.expresion1 is Corchetes)
                        {                            
                            if (valo is List<object>)
                            {
                                //listaRetorno = (List<object>)valo;
                                Lista listaGuardar = new Lista("item",(List<object>) valo, tipoDato.list, tipo, linea, columna);
                                listaRetorno.Add(listaGuardar);
                            }
                            else
                            {
                                List<Object> solo1 = new List<object>();
                                solo1.Add(valo);
                                Lista listaGuardar = new Lista("item", solo1, tipoDato.list, tipo, linea, columna);
                                listaRetorno.Add(listaGuardar);
                            }
                            tipo = tipoDato.list;
                        }
                        else if (tipo == tipoDato.id)
                        {
                            if (valo is Neww)
                            {
                                Neww n = (Neww)valo;

                                Simbolo sim2 = entorno.get(n.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                if (sim2 != null)
                                {
                                    if (sim2.valor is CreateType)
                                    {
                                        CreateType ss = (CreateType)sim2.valor;
                                        CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                        listaRetorno.Add(lista2);
                                    }
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El tipo de la variable del User type NO EXISTE. Tipos en cuestion: " + Convert.ToString(tipo)));
                                    return tipoDato.errorSemantico;
                                }
                            }
                            else
                            {
                                listaRetorno.Add(valo);
                            }
                        }
                        else if (tipo == tipoDato.list)
                        {
                            if (valo is Neww)
                            {
                                Neww v = (Neww)valo;
                                tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                                if (tt == tipoDato.ok)
                                {
                                    //Tipo aux = new Tipo(tipoDato.set, tiposSet, linea, columna);
                                    Lista listaGuardar = new Lista("item", new List<Object>(), tipoDato.list, tiposSet, linea, columna);
                                    listaRetorno.Add(listaGuardar);
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "Tipos no valido en el valor de la lista"));
                                    return tipoDato.errorSemantico;
                                }
                            }
                            else
                            {
                                listaRetorno.Add(valo);
                            }
                        }
                        else if (tipo == tipoDato.set)
                        {
                            if (valo is Neww)
                            {
                                Neww v = (Neww)valo;
                                tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                                if (tt == tipoDato.ok)
                                {
                                    //Tipo aux = new Tipo(tipoDato.set, tiposSet, linea, columna);
                                    Lista listaGuardar = new Lista("item", new List<Object>(), tipoDato.set, tiposSet, linea, columna);
                                    listaRetorno.Add(listaGuardar);                                    
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "Tipos no valido en el valor del set"));
                                    return tipoDato.errorSemantico;
                                }
                            }
                            else
                            {
                                listaRetorno.Add(valo);
                            }
                        }
                        else
                        {                            
                            listaRetorno.Add(valo);                            
                        }

                        if (contador == 0)
                        {
                            tipoValorAnterior = tipo;
                            tipoValor = tipo;
                            contador++;
                        }
                        else
                        {
                            if (tipoValorAnterior != tipo)
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Set" +
                                    "no son del mismo tipo los elementos"));
                                tipoValor = tipoDato.errorSemantico;
                                return tipoDato.errorSemantico;
                            }
                        }
                        contador++;
                    }

                    return listaRetorno;

                }
                else
                {
                    return expresion.getValue(entorno, listas); //un solo valor
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Set"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }

        LinkedList<Tipo> tiposSet = new LinkedList<Tipo>();
        public tipoDato comprobandoTipos(Entorno entorno, ErrorImpresion listas, Tipo tipoVal)
        {
            if (tipoVal.tipoValor is Tipo)
            {
                tiposSet.AddLast(new Tipo(tipoVal.tipo, linea, columna));
                tipoDato t = comprobandoTipos(entorno, listas, tipoVal.tipoValor);
                if (t == tipoDato.errorSemantico)
                {
                    return tipoDato.errorSemantico;
                }
            }
            else
            {
                if (tipoVal.tipo == tipoDato.id)
                {
                    Simbolo sim = entorno.get(tipoVal.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                    if (sim == null)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "El tipo de la variable del set no existe. El nombre es: " + tipoVal.id));
                        return tipoDato.errorSemantico;
                    }
                }
                else if (tipoVal.tipo == tipoDato.booleano ||
                        tipoVal.tipo == tipoDato.cadena ||
                        tipoVal.tipo == tipoDato.date ||
                        tipoVal.tipo == tipoDato.decimall ||
                        tipoVal.tipo == tipoDato.entero ||
                        tipoVal.tipo == tipoDato.time)
                {
                    return tipoDato.ok;
                }
            }

            return tipoDato.ok;
        }
    }
}
