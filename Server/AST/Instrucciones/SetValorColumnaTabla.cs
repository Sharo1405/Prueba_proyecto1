using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class SetValorColumnaTabla : Instruccion
    {
        public Columna col { get; set; }
        public ListaPuntos idExp { get; set; }
        public object valorParaAsignar { get; set; }
        public tipoDato tipoValorParaAsignar { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        tipoDato tipoFinal;
        LinkedList<Puntos> ListaExpresionesPuntos = new LinkedList<Puntos>();
        int contador = 0;
        object auxParaFunciones = new object();

        public SetValorColumnaTabla(Columna col, ListaPuntos idExp, object valorParaAsignar, tipoDato tipoValorParaAsignar,
            int linea, int colllll)
        {
            this.col = col;
            this.idExp = idExp;
            this.valorParaAsignar = valorParaAsignar;
            this.tipoValorParaAsignar = tipoValorParaAsignar;
            this.linea = linea;
            this.columna = colllll;
        }


        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            ListaExpresionesPuntos = new LinkedList<Puntos>();
            contador = 0;
            auxParaFunciones = new object();

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


            Columna coll = new Columna();
            int contador2 = 0;

            if (col.tipo == tipoDato.id)
            {

            }
            else if (col.tipo == tipoDato.list || col.tipo == tipoDato.set)
            {
                
            }
            else
            {
                object o = ListaExpresionesPuntos.ElementAt(contador2);

                if (tipoDato.counter == col.tipo)
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                        "No se puede actualizar un campo de tipo counter en la columna: " + coll.idColumna));
                    return tipoDato.errorSemantico;
                }
                else
                {
                    if (col.tipo == tipoValorParaAsignar)
                    {
                        int cantValores = coll.valorColumna.Count;
                        coll.valorColumna.Clear();
                        for (int i = 0; i < cantValores; i++)
                        {
                            coll.valorColumna.AddLast(valorParaAsignar);
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                            "No se puede actualizar la columna porque no es del mismo tipo: " + coll.idColumna));
                        return tipoDato.errorSemantico;
                    }
                }                
            }

            return tipoDato.ok;
        }

        public object devuleveFuncionCollection(FuncionesCollections funcion, Entorno entorno,
            ErrorImpresion listas, tipoDato tipoValor, Administrador management)
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
                            object valorPos = funcion.exp1.getValue(entorno, listas, management);
                            tipoDato tipovalorPos = funcion.exp1.getType(entorno, listas, management);

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
