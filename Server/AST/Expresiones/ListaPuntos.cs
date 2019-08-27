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

        public ListaPuntos(Expresion izq, Expresion der, int linea, int columna)
        {
            ExpSeparadasPuntos.AddLast(new Puntos(linea, columna, izq));
            ExpSeparadasPuntos.AddLast(new Puntos(linea, columna, der));
        }
        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            getValue(entorno, listas);
            return tipoFinal;
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
                                    return itType.valor;
                                }
                            }                            
                        }
                    }
                    else
                    {

                    }                    
                }
                else if (s.tipo == tipoDato.list)
                {

                }
                else if (s.tipo == tipoDato.set)
                {

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
                            return itType.valor;
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
            return tipoDato.ok;
        }


    }
}
