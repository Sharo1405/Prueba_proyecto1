using System;
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
    class DeclaraListValores : Instruccion
    {
        public LinkedList<String> idList { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public DeclaraListValores(LinkedList<String> idList, Expresion valor,
            int linea, int columna)
        {
            this.idList = idList;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        int contador = 0;
        List<Object> listaList = new List<Object>();
        tipoDato tipoValor = tipoDato.errorSemantico;
        tipoDato tipoValorAnterior = tipoDato.errorSemantico;

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Object dev = valor.getValue(entorno, listas);
                if (dev is LinkedList<Comas>)
                {
                    LinkedList<Comas> listaComas = (LinkedList<Comas>)dev;
                    foreach (Comas cv in listaComas)
                    {
                        Comas cv2 = (Comas)cv;
                        Object objeto = cv2.getValue(entorno, listas);
                        if (objeto is LinkedList<Comas>)
                        {
                            LinkedList<Comas> listComas = (LinkedList<Comas>)objeto;
                            tipoDato dato = paraComas(listComas, entorno, listas);
                            if (dato == tipoDato.errorSemantico)
                            {
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            contador++;
                            if (contador == 1)
                            {
                                tipoValorAnterior = tipoValor;
                                if (tipoValor == tipoDato.id)
                                {
                                    Object sim = cv2.getValue(entorno, listas);
                                    CreateType si = (CreateType)sim;
                                    listaList.Add(si.Clone());

                                }
                                else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                                {
                                    Object sim = cv2.getValue(entorno, listas);
                                    Simbolo s = (Simbolo)sim;
                                    listaList.Add(s.Clone());
                                }
                                else
                                {
                                    listaList.Add(objeto);
                                }
                            }
                            else
                            {
                                if (tipoValor == tipoValorAnterior)
                                {
                                    if (tipoValor == tipoDato.id)
                                    {
                                        Object sim = cv2.getValue(entorno, listas);
                                        CreateType si = (CreateType)sim;
                                        listaList.Add(si.Clone());

                                    }
                                    else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                                    {
                                        Object sim = cv2.getValue(entorno, listas);
                                        Simbolo s = (Simbolo)sim;
                                        listaList.Add(s.Clone());
                                    }
                                    else
                                    {
                                        listaList.Add(objeto);
                                    }
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                                    NodoError.tipoError.Semantico, "Los valores del List no son del mismo tipo."));
                                    return tipoDato.errorSemantico;
                                }
                            }
                        }
                    }

                    foreach (String id in idList)
                    {
                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), listaList, linea, columna,
                                    tipoDato.list, tipoValorAnterior, Simbolo.Rol.VARIABLE));
                    }
                }
                else
                {
                    //Expresion exp = (Expresion)dev;
                    tipoValor = valor.getType(entorno, listas);
                    if (tipoValor == tipoDato.booleano ||
                        tipoValor == tipoDato.cadena ||
                        tipoValor == tipoDato.date ||
                        tipoValor == tipoDato.decimall ||
                        tipoValor == tipoDato.entero ||
                        tipoValor == tipoDato.id ||
                        tipoValor == tipoDato.list ||
                        tipoValor == tipoDato.map ||
                        tipoValor == tipoDato.set ||
                        tipoValor == tipoDato.time)
                    {
                        if (tipoValor == tipoDato.id)
                        {
                            Object sim = valor.getValue(entorno, listas);
                            CreateType si = (CreateType)sim;
                            listaList.Add(si.Clone());

                        }
                        else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                        {
                            Object sim = valor.getValue(entorno, listas);
                            Simbolo s = (Simbolo)sim;
                            listaList.Add(s.Clone());
                        }
                        else
                        {
                            listaList.Add(dev);
                        }

                        foreach (String id in idList)
                        {
                            entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), listaList, linea, columna,
                                        tipoDato.list, tipoValor, Simbolo.Rol.VARIABLE));
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                        NodoError.tipoError.Semantico, "Valor del List no es valido"));
                        return tipoDato.errorSemantico;
                    }
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el List"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }


        public tipoDato paraComas(LinkedList<Comas> listComas, Entorno entorno, ErrorImpresion listas)
        {
            foreach (Comas cv in listComas)
            {
                Comas cv2 = (Comas)cv;
                Object valor = cv2.getValue(entorno, listas);
                if (valor is Comas)
                {
                    LinkedList<Comas> listaaComas = (LinkedList<Comas>)valor;
                    paraComas(listaaComas, entorno, listas);
                }
                else
                {
                    tipoValor = cv2.getType(entorno, listas);
                    if (tipoValor == tipoDato.booleano ||
                            tipoValor == tipoDato.cadena ||
                            tipoValor == tipoDato.date ||
                            tipoValor == tipoDato.decimall ||
                            tipoValor == tipoDato.entero ||
                            tipoValor == tipoDato.id ||
                            tipoValor == tipoDato.list ||
                            tipoValor == tipoDato.map ||
                            tipoValor == tipoDato.set ||
                            tipoValor == tipoDato.time)
                    {
                        contador++;

                        if (contador == 1)
                        {
                            tipoValorAnterior = tipoValor;
                            tipoValorAnterior = tipoValor;
                            if (tipoValor == tipoDato.id)
                            {
                                Object sim = cv2.getValue(entorno, listas);
                                CreateType si = (CreateType)sim;
                                listaList.Add(si.Clone());

                            }
                            else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                            {
                                Object sim = cv2.getValue(entorno, listas);
                                Simbolo s = (Simbolo)sim;
                                listaList.Add(s.Clone());
                            }
                            else
                            {
                                listaList.Add(valor);
                            }
                        }
                        else
                        {
                            if (tipoValor == tipoValorAnterior)
                            {
                                if (tipoValor == tipoDato.id)
                                {
                                    Object sim = cv2.getValue(entorno, listas);
                                    CreateType si = (CreateType)sim;
                                    listaList.Add(si.Clone());

                                }
                                else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                                {
                                    Object sim = cv2.getValue(entorno, listas);
                                    Simbolo s = (Simbolo)sim;
                                    listaList.Add(s.Clone());
                                }
                                else
                                {
                                    listaList.Add(valor);
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                                NodoError.tipoError.Semantico, "Los valores del List no son del mismo tipo."));
                                return tipoDato.errorSemantico;
                            }
                        }

                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                        NodoError.tipoError.Semantico, "Valor del List no es valido"));
                        return tipoDato.errorSemantico;
                    }
                }
            }
            return tipoDato.ok;
        }
    }
}

