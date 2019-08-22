using Server.AST.Expresiones;
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
        public tipoDato tipoClave { get; set; }
        public tipoDato tipoValor { get; set; }
        public Rol rol { get; set; }
        public LinkedList<Parametros> parametros = new LinkedList<Parametros>();

        public enum Rol
        {
            VARIABLE,
            METODO,
            FUNCION
        }


        public Simbolo() { }

        public Simbolo(String id, Object valor, int fila, int columna,
            tipoDato tipo, tipoDato tipoValor, Rol rol)
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.tipoValor = tipoValor;
            this.rol = rol;
        }

        public Simbolo(String id, Object valor, int fila, int columna,
            tipoDato tipo, tipoDato tipoClave, tipoDato tipoValor, Rol rol)
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.tipoClave = tipoClave;
            this.tipoValor = tipoValor;
            this.rol = rol;
        }

        public Simbolo(String id, Object valor, int fila, int columna, tipoDato tipo, Rol rol)
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.rol = rol;
        }

        public Simbolo(String id, Object valor, int fila, int columna, tipoDato tipo, Rol rol, LinkedList<Parametros> parametros)
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.rol = rol;
            this.parametros = parametros;
        }
    }
}
