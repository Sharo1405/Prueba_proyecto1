using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class SetValorColumnaTablaWhere : Instruccion
    {
        public Columna col { get; set; }
        public ListaPuntos idExp { get; set; }
        public object valorParaAsignar { get; set; }
        public tipoDato tipoValorParaAsignar { get; set; }
        public int posicionColumna { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }
        public int iterINDICE { get; set; }

        public Expresion sumaResta { get; set; }


        tipoDato tipoFinal;
        LinkedList<Puntos> ListaExpresionesPuntos = new LinkedList<Puntos>();
        int contador = 0;
        object auxParaFunciones = new object();


        public SetValorColumnaTablaWhere(Columna col, ListaPuntos idExp,
            int linea, int colllll, Expresion sumaRes, int posCol)
        {
            this.col = col;
            this.idExp = idExp;
            this.valorParaAsignar = null;
            this.tipoValorParaAsignar = tipoDato.errorSemantico;
            this.linea = linea;
            this.columna = colllll;
            this.sumaResta = sumaRes;
            this.posicionColumna = posCol;
        }


        public SetValorColumnaTablaWhere(Columna col, ListaPuntos idExp,
            int linea, int colllll, int iterINDICE, Expresion sumaRes, int posCol)
        {
            this.col = col;
            this.idExp = idExp;
            this.valorParaAsignar = null;
            this.tipoValorParaAsignar = tipoDato.errorSemantico;
            this.linea = linea;
            this.columna = colllll;
            this.iterINDICE = iterINDICE;
            this.sumaResta = null;
            this.posicionColumna = posCol;
        }


        public SetValorColumnaTablaWhere(Columna col, ListaPuntos idExp, object valorParaAsignar, tipoDato tipoValorParaAsignar,
            int linea, int colllll, int iterINDICE, int posCol)
        {
            this.col = col;
            this.idExp = idExp;
            this.valorParaAsignar = valorParaAsignar;
            this.tipoValorParaAsignar = tipoValorParaAsignar;
            this.linea = linea;
            this.columna = colllll;
            this.iterINDICE = iterINDICE;
            this.sumaResta = null;
            this.posicionColumna = posCol;
        }

        public SetValorColumnaTablaWhere(Columna col, ListaPuntos idExp, object valorParaAsignar, tipoDato tipoValorParaAsignar,
            int linea, int colllll, int posCol)
        {
            this.col = col;
            this.idExp = idExp;
            this.valorParaAsignar = valorParaAsignar;
            this.tipoValorParaAsignar = tipoValorParaAsignar;
            this.linea = linea;
            this.columna = colllll;
            this.posicionColumna = posCol;
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



            int contador2 = 0;




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
