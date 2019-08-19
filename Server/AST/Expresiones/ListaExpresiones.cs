using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;

namespace Server.AST.Expresiones
{
    class ListaExpresiones : Expresion
    {
        public LinkedList<Comas> ExpSeparadasComas = new LinkedList<Comas>();
        public int linea { get; set; }
        public int columna { get; set; }

        public ListaExpresiones(Expresion izq, Expresion der, int linea, int columna)
        {
            ExpSeparadasComas.AddLast(new Comas(linea, columna, izq));
            ExpSeparadasComas.AddLast(new Comas(linea, columna, der));
        }
        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return Operacion.tipoDato.list; //likedlist nada que ver con el proyecto
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            return ExpSeparadasComas;
        }
    }
}
