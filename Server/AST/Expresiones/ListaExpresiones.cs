using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;

namespace Server.AST.Expresiones
{
    class ListaExpresiones : Expresion
    {
        public LinkedList<Comas> ExpSeparadasComas = new LinkedList<Comas>();
        public int linea { get; set; }
        public int columna { get; set; }


        LinkedList<Comas> listaComas = new LinkedList<Comas>();
        int contador = 0;


        public ListaExpresiones()
        {

        }

        public ListaExpresiones(Expresion izq, Expresion der, int linea, int columna)
        {
            ExpSeparadasComas.AddLast(new Comas(linea, columna, izq));
            ExpSeparadasComas.AddLast(new Comas(linea, columna, der));
        }
        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return Operacion.tipoDato.list; //likedlist nada que ver con el proyecto
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            foreach (Comas exp in ExpSeparadasComas)
            {
                if (exp.expresion1 is ListaExpresiones)
                {
                    ListaExpresiones listanueva = (ListaExpresiones)exp.expresion1;
                    LinkedList<Comas> otraLista = (LinkedList<Comas>)listanueva.ExpSeparadasComas;
                    object result = masPuntos(entorno, listas, otraLista, contador);
                }
                else
                {
                    listaComas.AddLast(exp);
                }
            }

            return listaComas;
        }


        public object masPuntos(Entorno entorno, ErrorImpresion listas, LinkedList<Comas> listapuntos, int contador)
        {
            foreach (Comas exp in listapuntos)
            {
                if (exp.expresion1 is ListaExpresiones)
                {
                    ListaExpresiones listanueva = (ListaExpresiones)exp.expresion1;
                    LinkedList<Comas> otraLista = (LinkedList<Comas>)listanueva.ExpSeparadasComas;
                    object result = masPuntos(entorno, listas, otraLista, contador);
                }
                else
                {
                    listaComas.AddLast(exp);
                }
            }
            return null;
        }
    }
}
