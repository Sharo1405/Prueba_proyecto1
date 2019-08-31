using Server.AST.Entornos;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class ListaPuntos: Expresion
    {
        public LinkedList<Puntos> ExpSeparadasPuntos = new LinkedList<Puntos>();
        public int linea { get; set; }
        public int columna { get; set; }

        LinkedList<Puntos> ListaExpresionesPuntos = new LinkedList<Puntos>();
        int contador = 0;
        object auxParaFunciones = new object();

        public ListaPuntos(Expresion izq, Expresion der, int linea, int columna)
        {
            ExpSeparadasPuntos.AddLast(new Puntos(linea, columna, izq));
            ExpSeparadasPuntos.AddLast(new Puntos(linea, columna, der));
        }
        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            ListaExpresionesPuntos = new LinkedList<Puntos>();
            ListaExpresionesPuntos.Clear();
            auxParaFunciones = new object();
            getValue(entorno, listas);
            contador = 0;
            auxParaFunciones = new object();
            ListaExpresionesPuntos = new LinkedList<Puntos>();
            tipoDato tipoFinal2 = tipoFinal;
            return tipoFinal2;
        }

        tipoDato tipoFinal;

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            //int contador = 0;
            foreach (Puntos exp in ExpSeparadasPuntos)
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

            //aqui comienza el acceso para retornar el valor
            
            Puntos puntos = ListaExpresionesPuntos.ElementAt(contador);
            Object ob = puntos.expresion1.getValue(entorno, listas);
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

                                    while (contador < ListaExpresionesPuntos.Count)
                                    {
                                        punto = ListaExpresionesPuntos.ElementAt(contador);
                                        if (punto.expresion1 is FuncionesCollections)
                                        {
                                            FuncionesCollections funcion = (FuncionesCollections)punto.expresion1.getValue(entorno, listas);
                                            if (tipoFinal == tipoDato.list || tipoFinal == tipoDato.set)
                                            {
                                                devuleveFuncionCollection(funcion, entorno, listas, l.tipoValor);
                                            }
                                            else
                                            {
                                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                                    "La variable a la que se quiere aplicar : " + funcion.funcionCollections + "no es de tipo List"));
                                                return tipoDato.errorSemantico;
                                            }

                                        }
                                        else if (punto.expresion1 is FuncionesNativasCadenas)
                                        {
                                            if (tipoFinal != tipoDato.list && tipoFinal != tipoDato.set && tipoFinal != tipoDato.id)
                                            {
                                                FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)punto.expresion1.getValue(entorno, listas);
                                                if (tipoFinal != tipoDato.cadena)
                                                {
                                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                                    "La variable a la que se quiere aplicar : " + funcion.nativa + "no es de tipo String"));
                                                    return tipoDato.errorSemantico;
                                                }
                                                auxParaFunciones = devuelveFunconEjecutada(funcion, entorno, listas);
                                            }
                                            else
                                            {
                                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                            "La variable a la que se quiere aplicar una funcion nativa de cadenas no es de tipo String"));
                                                return tipoDato.errorSemantico;
                                            }
                                        }
                                        else if (punto.expresion1 is Identificador)
                                        {
                                            if (auxParaFunciones is CreateType)
                                            {
                                                CreateType type2 = (CreateType)auxParaFunciones;
                                                contador++;
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
                                    auxParaFunciones = itType.valor;
                                    contador++;
                                    while (contador < ListaExpresionesPuntos.Count)
                                    {
                                        Puntos puntos2 = ListaExpresionesPuntos.ElementAt(contador);
                                        if (puntos2.expresion1 is FuncionesNativasCadenas)
                                        {
                                            FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)puntos2.expresion1.getValue(entorno, listas);
                                            if (tipoFinal != tipoDato.cadena)
                                            {
                                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                                "La variable a la que se quiere aplicar : " + funcion.nativa + "no es de tipo String"));
                                                return tipoDato.errorSemantico;
                                            }
                                            auxParaFunciones = devuelveFunconEjecutada(funcion, entorno, listas);
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                                "El acceso no es valido porque es una variable normal"));
                                            return tipoDato.errorSemantico;
                                        }

                                        contador++;
                                    }
                                    return auxParaFunciones;

                                }
                            }
                        }
                    }
                    else if (punto.expresion1 is FuncionesNativasCadenas)
                    {
                        FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)punto.expresion1.getValue(entorno, listas);
                        if (tipoFinal != tipoDato.cadena)
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "La variable a la que se quiere aplicar : " + funcion.nativa + "no es de tipo String"));
                            return tipoDato.errorSemantico;
                        }

                        return devuelveFunconEjecutada(funcion, entorno, listas);
                    }
                }
                else if (s.tipo == tipoDato.list || s.tipo == tipoDato.set)
                {
                    tipoFinal = s.tipo;
                    contador++;
                    Lista l = (Lista)s.valor;
                    auxParaFunciones = null;
                    auxParaFunciones = l.listaValores;
                    

                    while (contador < ListaExpresionesPuntos.Count)
                    {
                        Puntos puntos2 = ListaExpresionesPuntos.ElementAt(contador);
                        if (puntos2.expresion1 is FuncionesCollections)
                        {
                            FuncionesCollections funcion = (FuncionesCollections)puntos2.expresion1.getValue(entorno, listas);
                            if (tipoFinal == tipoDato.list || tipoFinal == tipoDato.set)
                            {
                                devuleveFuncionCollection(funcion, entorno, listas, s.tipoValor);
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                    "La variable a la que se quiere aplicar : " + funcion.funcionCollections + "no es de tipo List"));
                                return tipoDato.errorSemantico;
                            }
                            
                        }
                        else if (puntos2.expresion1 is FuncionesNativasCadenas)
                        {
                            if (tipoFinal != tipoDato.list && tipoFinal != tipoDato.set && tipoFinal != tipoDato.id)
                            {
                                FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)puntos2.expresion1.getValue(entorno, listas);
                                if (tipoFinal != tipoDato.cadena)
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                    "La variable a la que se quiere aplicar : " + funcion.nativa + "no es de tipo String"));
                                    return tipoDato.errorSemantico;
                                }
                                auxParaFunciones = devuelveFunconEjecutada(funcion, entorno, listas);
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "La variable a la que se quiere aplicar una funcion nativa de cadenas no es de tipo String"));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else if (puntos2.expresion1 is Identificador)
                        {
                            if (auxParaFunciones is CreateType) {
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
                    while (contador < ListaExpresionesPuntos.Count)
                    {
                        Puntos puntos2 = ListaExpresionesPuntos.ElementAt(contador);
                        if (puntos2.expresion1 is FuncionesNativasCadenas)
                        {
                            FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)puntos2.expresion1.getValue(entorno, listas);
                            if (tipoFinal != tipoDato.cadena)
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "La variable a la que se quiere aplicar : " + funcion.nativa + "no es de tipo String"));
                                return tipoDato.errorSemantico;
                            }
                            auxParaFunciones = devuelveFunconEjecutada(funcion, entorno, listas);
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "El acceso no es valido porque es una variable normal"));
                            return tipoDato.errorSemantico;
                        }

                        contador++;
                    }
                    return auxParaFunciones;
                }
                return auxParaFunciones;
            }
            else
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "La variable no fue encontrada, no se puede hacer el acceso"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.errorSemantico;
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
                        case "insert":
                            if (funcion.exp2 == null)
                            {
                                Object itemAsignar = funcion.exp1.getValue(entorno, listas);
                                tipoDato tipoAsignar = funcion.exp1.getType(entorno, listas);
                                if (tipoValor == tipoAsignar)
                                {
                                    auxlista.Add(itemAsignar);
                                    auxParaFunciones = null;
                                    auxParaFunciones = auxlista;
                                    return auxParaFunciones;
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El valor a asignar no es del mismo tipo que los Ya existentes"));
                                    auxParaFunciones = tipoDato.errorSemantico;
                                    return tipoDato.errorSemantico;
                                }
                            }
                            auxParaFunciones = "";
                            break;

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

                        case "set":
                            if (auxlista.Count == 0)
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "La lista esta vacia no se puede realizar ninguna operacion"));
                                auxParaFunciones = tipoDato.errorSemantico;
                                return tipoDato.errorSemantico;
                            }
                            int contaPos = -1;
                            foreach (Object item in auxlista)
                            {
                                contaPos++;
                            }

                            Object pos = funcion.exp1.getValue(entorno, listas);
                            tipoDato tipoP = funcion.exp1.getType(entorno, listas);
                            if (tipoP == tipoDato.entero)
                            {
                                int p = Int32.Parse(Convert.ToString(pos));
                                if (p > contaPos)
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "Posicion es mayor a la de la lists/set"));
                                    auxParaFunciones = tipoDato.errorSemantico;
                                    return tipoDato.errorSemantico;
                                }

                                Object itemAsignar = funcion.exp2.getValue(entorno, listas);
                                tipoDato tipoAsignar = funcion.exp2.getType(entorno, listas);
                                if (tipoValor == tipoAsignar)
                                {
                                    auxlista.RemoveAt(p);
                                    auxlista.Insert(p, itemAsignar);
                                }
                                auxParaFunciones = null;
                                auxParaFunciones = auxlista;
                                return auxParaFunciones;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "Posicion no valida para setear el valor a la lista: " +
                                        Convert.ToString(tipoP)));
                                auxParaFunciones = tipoDato.errorSemantico;
                                return tipoDato.errorSemantico;
                            }

                        case "remove":
                            if (auxlista.Count == 0)
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "La lista esta vacia no se puede realizar ninguna operacion"));
                                auxParaFunciones = tipoDato.errorSemantico;
                                return tipoDato.errorSemantico;
                            }
                            object posicion = funcion.exp1.getValue(entorno, listas);
                            tipoDato tipoPos = funcion.exp1.getType(entorno, listas);
                            if (tipoPos == tipoDato.entero)
                            {
                                auxlista.RemoveAt(Int32.Parse(Convert.ToString(posicion)));
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El tipo para remover la posicion de la lista/set no es tipo entero sino: " +
                                        Convert.ToString(tipoPos)));
                                auxParaFunciones = tipoDato.errorSemantico;
                                return tipoDato.errorSemantico;
                            }
                            auxParaFunciones = null;
                            auxParaFunciones = auxlista;
                            return auxParaFunciones;

                        case "size":
                            auxParaFunciones = null;
                            auxParaFunciones = auxlista.Count;
                            tipoFinal = tipoDato.entero;
                            return auxParaFunciones;

                        case "clear":
                            auxlista.Clear();
                            auxParaFunciones = null;
                            auxParaFunciones = auxlista;
                            return auxParaFunciones;

                        case "contains":
                            object posicion2 = funcion.exp1.getValue(entorno, listas);
                            tipoDato tipoPos2 = funcion.exp1.getType(entorno, listas);
                            if (tipoPos2 == tipoValor) {
                                tipoFinal = tipoDato.booleano;
                                auxParaFunciones = auxlista.Contains(posicion2);
                            }
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


        public object devuelveFunconEjecutada(FuncionesNativasCadenas funcion, Entorno entorno, ErrorImpresion listas)
        {
            switch (funcion.nativa)
            {
                case "length":
                    return Convert.ToString(auxParaFunciones).Length;

                case "touppercase":
                    return Convert.ToString(auxParaFunciones).ToUpper();

                case "tolowercase":
                    return Convert.ToString(auxParaFunciones).ToLower();

                case "startswith":
                    tipoDato es = funcion.exp1.getType(entorno, listas);
                    if (es != tipoDato.cadena)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El parametro de inicio para la funcion StartsWith no es de tipo cadena"));
                        return tipoDato.errorSemantico;
                    }
                    Object cadena = funcion.exp1.getValue(entorno, listas);
                    return Convert.ToString(auxParaFunciones).StartsWith(Convert.ToString(cadena));

                case "endswith":
                    tipoDato ess = funcion.exp1.getType(entorno, listas);
                    if (ess != tipoDato.cadena)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El parametro de inicio para la funcion endsWith no es de tipo cadena"));
                        return tipoDato.errorSemantico;
                    }
                    Object cadena2 = funcion.exp1.getValue(entorno, listas);
                    return Convert.ToString(auxParaFunciones).StartsWith(Convert.ToString(cadena2));

                case "substring":
                    tipoDato esss = funcion.exp1.getType(entorno, listas);
                    tipoDato essss = funcion.exp1.getType(entorno, listas);
                    if (esss != tipoDato.entero && essss != tipoDato.entero)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El parametro de inicio para la funcion endsWith no es de tipo cadena"));
                        return tipoDato.errorSemantico;
                    }
                    Object inicio = funcion.exp1.getValue(entorno, listas);
                    Object final = funcion.exp2.getValue(entorno, listas);
                    return Convert.ToString(auxParaFunciones).Substring(Int32.Parse(Convert.ToString(inicio)),
                        Int32.Parse(Convert.ToString(final)));

            }

            return tipoDato.errorSemantico;
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
                                        "La variable \"" + identi.id +"\" no esta Instanciada para el acceso"));
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

                            while (contador < ListaExpresionesPuntos.Count)
                            {
                                punto = ListaExpresionesPuntos.ElementAt(contador);
                                if (punto.expresion1 is FuncionesCollections)
                                {
                                    FuncionesCollections funcion = (FuncionesCollections)punto.expresion1.getValue(entorno, listas);
                                    if (tipoFinal == tipoDato.list || tipoFinal == tipoDato.set)
                                    {
                                        devuleveFuncionCollection(funcion, entorno, listas, l.tipoValor);
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                            "La variable a la que se quiere aplicar : " + funcion.funcionCollections + "no es de tipo List"));
                                        return tipoDato.errorSemantico;
                                    }

                                }
                                else if (punto.expresion1 is FuncionesNativasCadenas)
                                {
                                    if (tipoFinal != tipoDato.list && tipoFinal != tipoDato.set && tipoFinal != tipoDato.id)
                                    {
                                        FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)punto.expresion1.getValue(entorno, listas);
                                        if (tipoFinal != tipoDato.cadena)
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                            "La variable a la que se quiere aplicar : " + funcion.nativa + "no es de tipo String"));
                                            return tipoDato.errorSemantico;
                                        }
                                        auxParaFunciones = devuelveFunconEjecutada(funcion, entorno, listas);
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                    "La variable a la que se quiere aplicar una funcion nativa de cadenas no es de tipo String"));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                                else if (punto.expresion1 is Identificador)
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
                            tipoFinal = itType.tipo.tipo;
                            auxParaFunciones = itType.valor;
                            contador++;
                            while (contador < ListaExpresionesPuntos.Count)
                            {
                                Puntos puntos2 = ListaExpresionesPuntos.ElementAt(contador);
                                if (puntos2.expresion1 is FuncionesNativasCadenas)
                                {
                                    FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)puntos2.expresion1.getValue(entorno, listas);
                                    if (tipoFinal != tipoDato.cadena)
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                        "La variable a la que se quiere aplicar : " + funcion.nativa + "no es de tipo String"));
                                        return tipoDato.errorSemantico;
                                    }
                                    auxParaFunciones = devuelveFunconEjecutada(funcion, entorno, listas);
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                        "El acceso no es valido porque es una variable normal"));
                                    return tipoDato.errorSemantico;
                                }

                                contador++;
                            }
                            return auxParaFunciones;

                        }
                    }
                }
            }
            else if (punto.expresion1 is FuncionesNativasCadenas)
            {
                
                while (contador < ListaExpresionesPuntos.Count)
                {
                    Puntos puntos2 = ListaExpresionesPuntos.ElementAt(contador);
                    if (puntos2.expresion1 is FuncionesNativasCadenas)
                    {
                        FuncionesNativasCadenas funcion2 = (FuncionesNativasCadenas)puntos2.expresion1.getValue(entorno, listas);
                        if (tipoFinal != tipoDato.cadena)
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "La variable a la que se quiere aplicar : " + funcion2.nativa + "no es de tipo String"));
                            return tipoDato.errorSemantico;
                        }
                        auxParaFunciones = devuelveFunconEjecutada(funcion2, entorno, listas);
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "El acceso no es valido porque es una variable normal"));
                        return tipoDato.errorSemantico;
                    }

                    contador++;
                }
                return auxParaFunciones;
                
            }
            else
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El acceso no es una Variable, por lo tanto no se puede acceder"));
                return tipoDato.errorSemantico;
            }
            return auxParaFunciones;
        }


    }
}
