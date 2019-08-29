using Server.AST.Entornos;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Instrucciones;
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
        int contadorFuncionNativa = 0;
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
            contadorFuncionNativa = 0;
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
            ///*****************************
            //////
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
                                else if (itType.tipo.tipo == tipoDato.list)
                                {

                                }
                                else if (itType.tipo.tipo == tipoDato.set)
                                {

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
                    else if(punto.expresion1 is FuncionesNativasCadenas)
                    {
                        FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)punto.expresion1.getValue(entorno, listas);
                        if (tipoFinal != tipoDato.cadena)
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "La variable a la que se quiere aplicar : " +  funcion.nativa + "no es de tipo String"));
                            return tipoDato.errorSemantico;
                        }

                        return devuelveFunconEjecutada(funcion, entorno, listas);
                    }                    
                }
                else if (s.tipo == tipoDato.list)
                {

                }
                else if (s.tipo == tipoDato.set)
                {

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
            }
            else
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "La variable no fue encontrada, no se puede hacer el acceso"));
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
                        else if (itType.tipo.tipo == tipoDato.list)
                        {

                        }
                        else if (itType.tipo.tipo == tipoDato.set)
                        {

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
            return tipoDato.ok;
        }


    }
}
