using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Entornos
{
    class Simbolo
    {
        public String id { get; set; }
        public Object valor { get; set; }
        public int fila { get; set; }
        public int columna { get; set; }
        public tipoDato tipo { get; set; }  
        public Rol rol;

        public enum Rol
        {
            VARIABLE,
            METODO,
            FUNCION
        }


        public Simbolo() { }


        public Simbolo(String id, Object valor, int fila, int columna, tipoDato tipo, Rol rol)
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.rol = rol;
        }
    }
}
