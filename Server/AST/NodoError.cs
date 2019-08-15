using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST
{
    class NodoError
    {
        public int fila;
        public int columna;
        public enum tipoError
        {
            lexico,
            Sintactico,
            Semantico
        }

        public tipoError tipo;
        public String descripcion;

        public NodoError(int fila, int columna, tipoError tipo, String descripcion)
        {
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.descripcion = descripcion;
        }
    }
}
