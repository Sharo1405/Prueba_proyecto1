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
    class LLaveAsTypeUser : Expresion
    {
        public Expresion listaValores { get; set; } //debe ser una listaExpresiones
        public String tipoTypeUser { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        int contador = 0;

        public LLaveAsTypeUser(Expresion listaValores, String tipoTypeUser,
            int linea, int columna)
        {
            this.listaValores = listaValores;
            this.tipoTypeUser = tipoTypeUser;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipoDato.id;
        }


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
                Simbolo s = entorno.get(tipoTypeUser.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                if (s != null)
                {
                    CreateType ll = (CreateType)s.valor;
                    CreateType lista = CreaNuevoType(ll, entorno, listas);
                    
                    LinkedList<Comas> objeto = (LinkedList<Comas>) listaValores.getValue(entorno, listas); //Lista comas
                    foreach (Comas coma in objeto)
                    {
                        Comas cv2 = (Comas)coma;
                        Object valo = cv2.getValue(entorno, listas);
                        itemType itType = lista.itemTypee.ElementAt(contador);

                        if (itType.tipo.tipo == tipoDato.id)
                        {
                            if (valo is Neww)
                            {
                                Neww claseNeww = (Neww)valo;
                                Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                if (sim2 != null)
                                {
                                    if (sim2.valor is CreateType)
                                    {
                                        CreateType ss = (CreateType)sim2.valor;
                                        CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                        itType.valor = lista2;
                                    }
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El tipo de la variable del User type NO EXISTE. Tipos en cuestion: " + Convert.ToString(itType.tipo.id)));
                                    return tipoDato.errorSemantico;
                                }
                            }
                            else
                            {
                                tipoDato tipoValue = cv2.getType(entorno, listas);
                                if (tipoValue == itType.tipo.tipo)
                                {
                                    if (valo is Simbolo)
                                    {
                                        itType.valor = ((Simbolo)valo).valor;
                                    }
                                    else
                                    {
                                        itType.valor = valo;
                                    }
                                }
                                else if (tipoValue == tipoDato.nulo && itType.tipo.tipo == tipoDato.id)
                                {
                                    itType.valor = valo;
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El tipo de la variable del User type NO EXISTE."));
                                    return tipoDato.errorSemantico;
                                }
                            }
                        }
                        else if (itType.tipo.tipo == tipoDato.list)
                        {

                        }
                        else if (itType.tipo.tipo == tipoDato.set)
                        {

                        }
                        else
                        {
                            tipoDato tipoValue = cv2.getType(entorno, listas);
                            if (tipoValue == itType.tipo.tipo)
                            {
                                itType.valor = valo;
                            }
                            else if (tipoValue == tipoDato.nulo && (itType.tipo.tipo == tipoDato.cadena ||
                                itType.tipo.tipo == tipoDato.date ||
                                itType.tipo.tipo == tipoDato.time ||
                                itType.tipo.tipo == tipoDato.list ||
                                itType.tipo.tipo == tipoDato.set ||
                                itType.tipo.tipo == tipoDato.id ||
                                itType.tipo.tipo == tipoDato.map))
                            {
                                itType.valor = valo;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "El tipo de la variable del User type NO es el mismo."));
                                return tipoDato.errorSemantico;
                            }
                        }
                        contador++;
                    }

                    return lista;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                "TypeUser no valido para asignar"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.id;
        }


        public tipoDato paraComasTYPEUSER(LinkedList<Comas> listComas, Entorno entorno, ErrorImpresion listas, LinkedList<itemType> itTypes)
        {
            foreach (Comas cv in listComas)
            {
                Comas cv2 = (Comas)cv;
                Object valo = cv2.getValue(entorno, listas);
                if (valo is Comas)
                {
                    LinkedList<Comas> listaaComas = (LinkedList<Comas>)valo;
                    paraComasTYPEUSER(listaaComas, entorno, listas, itTypes);
                }
                else
                {
                    itemType itType = itTypes.ElementAt(contador);

                    if (itType.tipo.tipo == tipoDato.id)
                    {
                        if (valo is Neww)
                        {
                            Neww claseNeww = (Neww)valo;
                            Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                            if (sim2 != null)
                            {
                                if (sim2.valor is CreateType)
                                {
                                    CreateType ss = (CreateType)sim2.valor;
                                    CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                    itType.valor = lista2;
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO EXISTE."));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            tipoDato tipoValue = cv2.getType(entorno, listas);
                            if (tipoValue == itType.tipo.tipo)
                            {
                                itType.valor = valo;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO es el mismo."));
                                return tipoDato.errorSemantico;
                            }
                        }
                    }
                    else if (itType.tipo.tipo == tipoDato.list)
                    {

                    }
                    else if (itType.tipo.tipo == tipoDato.set)
                    {

                    }
                    else
                    {
                        tipoDato tipoValue = cv2.getType(entorno, listas);
                        if (tipoValue == itType.tipo.tipo)
                        {
                            itType.valor = cv2.getValue(entorno, listas);
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO EXISTE."));
                            return tipoDato.errorSemantico;
                        }
                    }

                }
                contador++;
            }

            return tipoDato.ok;
        }
    }
}
