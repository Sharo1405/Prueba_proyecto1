using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Otras
{
    class SetearvaloresAccesos : Instruccion
    {
        public String idVarInicio { get; set; }

        public ListaPuntos idExp { get; set; }
        public object valorParaAsignar { get; set; }
        public tipoDato tipoValorParaAsignar { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        tipoDato tipoFinal;
        LinkedList<Puntos> ListaExpresionesPuntos = new LinkedList<Puntos>();
        int contador = 0;
        object auxParaFunciones = new object();

        public SetearvaloresAccesos(String idVarInicio, ListaPuntos idExp, object valorParaAsignar, int linea, int columna, tipoDato tipovalorasignar)
        {
            this.idVarInicio = idVarInicio;
            this.idExp = idExp;
            this.valorParaAsignar = valorParaAsignar;
            this.tipoValorParaAsignar = tipovalorasignar;
            this.linea = linea;
            this.columna = columna;
        }

        public SetearvaloresAccesos(ListaPuntos idExp, object valorParaAsignar, int linea, int columna, tipoDato tipovalorasignar)
        {
            this.idVarInicio = "";
            this.idExp = idExp;
            this.valorParaAsignar = valorParaAsignar;
            this.tipoValorParaAsignar = tipovalorasignar;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            
            foreach (Puntos exp in idExp.ExpSeparadasPuntos)
            {
                Puntos exp2 = exp;
                if (exp.expresion1 is ListaPuntos)
                {
                    ListaPuntos x = (ListaPuntos)exp.expresion1;
                    LinkedList<Puntos> otraLista = (LinkedList<Puntos>)x.ExpSeparadasPuntos;
                    object result = masPuntos(entorno, listas, otraLista, contador);
                }
                else
                {
                    ListaExpresionesPuntos.AddLast(exp);
                }
            }


            //----------------------------------------------------------------------------------------
            Object ob = new object();
            if (idVarInicio.Equals("")) {
                Puntos puntos = ListaExpresionesPuntos.ElementAt(contador);
                ob = puntos.expresion1.getValue(entorno, listas);
            }
            else
            {
                contador--;
                ob = entorno.get(idVarInicio, entorno, Simbolo.Rol.VARIABLE);
            }

            if (ob is Simbolo)
            {
                Simbolo s = (Simbolo)ob;
                if (s.tipo == tipoDato.id)
                {
                    CreateType type = (CreateType)s.valor;
                    contador++;
                    Puntos punto = ListaExpresionesPuntos.ElementAt(contador);
                    if (punto.expresion1 is Identificador)
                    {
                        Identificador identi = (Identificador)punto.expresion1;
                        foreach (itemType itType in type.itemTypee)
                        {
                            if (itType.id.ToLower().Equals(identi.id.ToLower()))
                            {
                                if (itType.tipo.tipo == tipoDato.id)
                                {
                                    CreateType otroitem = (CreateType)itType.valor;

                                    if (valorParaAsignar is CreateType && contador >= ListaExpresionesPuntos.Count)
                                    {
                                        itType.valor = valorParaAsignar;
                                    }

                                    if (otroitem != null)
                                    {
                                        contador++;
                                        object tipoDevuelto = tipoType(entorno, listas, otroitem, contador);                                        
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "La variable \"" + identi.id + "\" no esta Instanciada para el acceso"));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                                else if (itType.tipo.tipo == tipoDato.list || itType.tipo.tipo == tipoDato.set)
                                {
                                    contador++;
                                    tipoFinal = itType.tipo.tipo;
                                    Lista l = (Lista)itType.valor;
                                    auxParaFunciones = null;
                                    auxParaFunciones = l.listaValores;
                                    tipoDato tvtvtv = l.tipoValor;


                                    if (valorParaAsignar is Lista && contador >= ListaExpresionesPuntos.Count)
                                    {
                                        Lista vv = (Lista)valorParaAsignar;
                                        if (vv.tipoGeneral == itType.tipo.tipo)
                                        {
                                            itType.valor = valorParaAsignar;
                                        }
                                    }

                                    while (contador < ListaExpresionesPuntos.Count)
                                    {
                                        punto = ListaExpresionesPuntos.ElementAt(contador);
                                        if (punto.expresion1 is FuncionesCollections)
                                        {
                                            Lista ll = new Lista();
                                            if (auxParaFunciones is Lista)
                                            {
                                                ll = (Lista)auxParaFunciones;
                                                auxParaFunciones = null;
                                                auxParaFunciones = ll.listaValores;
                                                tvtvtv = ll.tipoValor;
                                            }
                                            FuncionesCollections funcion = (FuncionesCollections)punto.expresion1.getValue(entorno, listas);
                                            if (tipoFinal == tipoDato.list || tipoFinal == tipoDato.set)
                                            {
                                                devuleveFuncionCollection(funcion, entorno, listas, tvtvtv);
                                            }
                                            else
                                            {
                                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                                    "La variable a la que se quiere aplicar : " + funcion.funcionCollections + "no es de tipo List"));
                                                return tipoDato.errorSemantico;
                                            }

                                        }                                        
                                        else if (punto.expresion1 is Identificador)
                                        {
                                            if (auxParaFunciones is CreateType)
                                            {
                                                CreateType type2 = (CreateType)auxParaFunciones;
                                                //contador++;
                                                auxParaFunciones = tipoType(entorno, listas, type2, contador);
                                                return auxParaFunciones;
                                            }
                                            else
                                            {
                                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                       "Acceso no valido para una lista/ Set"));
                                                return tipoDato.errorSemantico;
                                            }
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                            "Acceso no valido para una lista/ Set"));
                                            return tipoDato.errorSemantico;
                                        }

                                        contador++;
                                    }
                                }
                                else
                                {
                                    tipoFinal = itType.tipo.tipo;
                                    if (tipoValorParaAsignar == itType.tipo.tipo) {

                                        itType.valor = valorParaAsignar;
                                        
                                    }
                                    else if (tipoValorParaAsignar == tipoDato.entero && tipoDato.decimall == itType.tipo.tipo)
                                    {
                                        itType.valor = Convert.ToDouble(valorParaAsignar);
                                    }
                                    else if (tipoValorParaAsignar == tipoDato.decimall && tipoDato.entero == itType.tipo.tipo)
                                    {
                                        itType.valor = Convert.ToInt32(valorParaAsignar);
                                    }
                                    contador++;
                                    return tipoDato.ok ;

                                }
                            }
                        }
                    }                    
                }
                else if (s.tipo == tipoDato.list || s.tipo == tipoDato.set)
                {
                    tipoFinal = s.tipo;
                    contador++;
                    Lista l = (Lista)s.valor;
                    auxParaFunciones = null;
                    auxParaFunciones = l.listaValores;
                    tipoDato tvlista = s.tipoValor;

                    if (valorParaAsignar is Lista && contador >= ListaExpresionesPuntos.Count)
                    {
                        Lista vv = (Lista)valorParaAsignar;
                        if (vv.tipoGeneral == s.tipo)
                        {
                            s.valor = valorParaAsignar;
                        }
                    }

                    while (contador < ListaExpresionesPuntos.Count)
                    {
                        Lista ll = new Lista();
                        if (auxParaFunciones is Lista)
                        {
                            ll = (Lista)auxParaFunciones;
                            auxParaFunciones = null;
                            auxParaFunciones = ll.listaValores;
                            tvlista = ll.tipoValor;
                        }
                        Puntos puntos2 = ListaExpresionesPuntos.ElementAt(contador);
                        if (puntos2.expresion1 is FuncionesCollections)
                        {
                            FuncionesCollections funcion = (FuncionesCollections)puntos2.expresion1.getValue(entorno, listas);
                            if (tipoFinal == tipoDato.list || tipoFinal == tipoDato.set)
                            {
                                devuleveFuncionCollection(funcion, entorno, listas, tvlista);
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                    "La variable a la que se quiere aplicar : " + funcion.funcionCollections + "no es de tipo List"));
                                return tipoDato.errorSemantico;
                            }

                        }                        
                        else if (puntos2.expresion1 is Identificador)
                        {
                            if (auxParaFunciones is CreateType)
                            {
                                CreateType type = (CreateType)auxParaFunciones;
                                contador++;
                                auxParaFunciones = tipoType(entorno, listas, type, contador);
                                return auxParaFunciones;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                       "Acceso no valido para una lista/ Set"));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "Acceso no valido para una lista/ Set"));
                            return tipoDato.errorSemantico;
                        }

                        contador++;
                    }
                }
                else
                {
                    tipoFinal = s.tipo;
                    auxParaFunciones = s.valor; // aqui solo vendrian variables normales
                    contador++;                    
                }
            }
            return tipoDato.ok;
        }

        public object devuleveFuncionCollection(FuncionesCollections funcion, Entorno entorno,
            ErrorImpresion listas, tipoDato tipoValor)
        {
            try
            {
                if (tipoFinal == tipoDato.list || tipoFinal == tipoDato.set)
                {
                    List<Object> auxlista = (List<Object>)auxParaFunciones;
                    switch (funcion.funcionCollections)
                    {
                       case "get":
                            if (auxlista.Count == 0)
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "La lista esta vacia no se puede realizar ninguna operacion"));
                                auxParaFunciones = tipoDato.errorSemantico;
                                return tipoDato.errorSemantico;
                            }
                            int contaPos1 = -1;
                            foreach (Object item in auxlista)
                            {
                                contaPos1++;
                            }
                            object valorPos = funcion.exp1.getValue(entorno, listas);
                            tipoDato tipovalorPos = funcion.exp1.getType(entorno, listas);

                            if (tipovalorPos != tipoDato.entero)
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El indice no es de tipo entero"));
                                auxParaFunciones = tipoDato.errorSemantico;
                                return tipoDato.errorSemantico;
                            }
                            if (Int32.Parse(Convert.ToString(valorPos)) > contaPos1)
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "Posicion es mayor a la de la lists/set"));
                                auxParaFunciones = tipoDato.errorSemantico;
                                return tipoDato.errorSemantico;
                            }

                            auxParaFunciones = null;
                            tipoFinal = tipoValor;
                            auxParaFunciones = auxlista.ElementAt(Int32.Parse(Convert.ToString(valorPos)));
                            return auxParaFunciones;                        
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                       "Acceso no valido no es una lista/set a la que se le aplicara las funciones de listas"));
                    auxParaFunciones = tipoDato.errorSemantico;
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                        "Acceso no valido"));
                auxParaFunciones = tipoDato.errorSemantico;
                return tipoDato.errorSemantico;
            }
            return tipoDato.errorSemantico;
        }


        public object tipoType(Entorno entorno, ErrorImpresion listas, CreateType claseType, int contador)
        {

            Puntos punto = ListaExpresionesPuntos.ElementAt(contador);

            if (punto.expresion1 is Identificador)
            {
                Identificador identi = (Identificador)punto.expresion1;

                foreach (itemType itType in claseType.itemTypee)
                {
                    if (itType.id.ToLower().Equals(identi.id.ToLower()))
                    {
                        if (itType.tipo.tipo == tipoDato.id)
                        {
                            CreateType otroitem = (CreateType)itType.valor;
                            if (valorParaAsignar is CreateType && contador >= ListaExpresionesPuntos.Count)
                            {
                                itType.valor = valorParaAsignar;
                            }

                            if (otroitem != null)
                            {
                                contador++;
                                object tipoDevuelto = tipoType(entorno, listas, otroitem, contador);
                                auxParaFunciones = tipoDevuelto;
                                return tipoDevuelto;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "La variable \"" + identi.id + "\" no esta Instanciada para el acceso"));
                                if (otroitem == null)
                                {
                                    return tipoDato.nulo;
                                }
                                return tipoDato.errorSemantico;
                            }
                        }
                        else if (itType.tipo.tipo == tipoDato.list || itType.tipo.tipo == tipoDato.set)
                        {
                            contador++;
                            tipoFinal = itType.tipo.tipo;
                            Lista l = (Lista)itType.valor;
                            auxParaFunciones = null;
                            auxParaFunciones = l.listaValores;
                            tipoDato tvtvtv = l.tipoValor;

                            if (valorParaAsignar is Lista && contador >= ListaExpresionesPuntos.Count)
                            {
                                Lista vv = (Lista)valorParaAsignar;
                                if (vv.tipoGeneral == itType.tipo.tipo)
                                {
                                    itType.valor = valorParaAsignar;
                                }
                            }

                            if (valorParaAsignar is Lista && contador >= ListaExpresionesPuntos.Count)
                            {
                                Lista vv = (Lista)valorParaAsignar;
                                if (vv.tipoGeneral == itType.tipo.tipo)
                                {
                                    itType.valor = valorParaAsignar;
                                }
                            }

                            while (contador < ListaExpresionesPuntos.Count)
                            {
                                Lista ll = new Lista();
                                if (auxParaFunciones is Lista)
                                {
                                    ll = (Lista)auxParaFunciones;
                                    auxParaFunciones = null;
                                    auxParaFunciones = ll.listaValores;
                                    tvtvtv = ll.tipoValor;
                                }
                                punto = ListaExpresionesPuntos.ElementAt(contador);
                                if (punto.expresion1 is FuncionesCollections)
                                {
                                    FuncionesCollections funcion = (FuncionesCollections)punto.expresion1.getValue(entorno, listas);
                                    if (tipoFinal == tipoDato.list || tipoFinal == tipoDato.set)
                                    {
                                        devuleveFuncionCollection(funcion, entorno, listas, tvtvtv);
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                            "La variable a la que se quiere aplicar : " + funcion.funcionCollections + "no es de tipo List"));
                                        return tipoDato.errorSemantico;
                                    }

                                }                                
                                else if (punto.expresion1 is Identificador)
                                {
                                    if (auxParaFunciones is CreateType)
                                    {
                                        CreateType type = (CreateType)auxParaFunciones;
                                        //contador++;
                                        auxParaFunciones = tipoType(entorno, listas, type, contador);
                                        return auxParaFunciones;
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                               "Acceso no valido para una lista/ Set"));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                    "Acceso no valido para una lista/ Set"));
                                    return tipoDato.errorSemantico;
                                }

                                contador++;
                            }
                        }
                        else
                        {
                            tipoFinal = itType.tipo.tipo;
                            if (tipoValorParaAsignar == itType.tipo.tipo)
                            {
                                itType.valor = valorParaAsignar;
                            }
                            else if (tipoValorParaAsignar == tipoDato.entero && tipoDato.decimall == itType.tipo.tipo)
                            {
                                itType.valor = Convert.ToDouble(valorParaAsignar);
                            }
                            else if (tipoValorParaAsignar == tipoDato.decimall && tipoDato.entero == itType.tipo.tipo)
                            {
                                itType.valor = Convert.ToInt32(valorParaAsignar);
                            }
                            //itType.valor = valorParaAsignar;
                            contador++;                            
                            return tipoDato.ok;

                        }
                    }
                }
            }            
            else
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El acceso no es una Variable, por lo tanto no se puede acceder"));
                return tipoDato.errorSemantico;
            }
            return auxParaFunciones;
        }


        public object masPuntos(Entorno entorno, ErrorImpresion listas, LinkedList<Puntos> listapuntos, int contador)
        {
            foreach (Puntos exp in listapuntos)
            {
                if (exp.expresion1 is ListaPuntos)
                {
                    ListaPuntos x = (ListaPuntos)exp.expresion1;
                    LinkedList<Puntos> otraLista = (LinkedList<Puntos>)x.ExpSeparadasPuntos;
                    object result = masPuntos(entorno, listas, otraLista, contador);
                }
                else
                {
                    ListaExpresionesPuntos.AddLast(exp);
                }
            }
            return null;
        }
    }
}
