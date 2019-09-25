using Irony.Parsing;
using Server.AST.Expresiones;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Entornos
{
    class Simbolo: ICloneable
    {
        public Boolean abierto { get; set; }
        public String id { get; set; }
        public Object valor { get; set; }
        public int fila { get; set; }
        public int columna { get; set; }
        public String idTipo { get; set; }
        public tipoDato tipo { get; set; } 
        public tipoDato tipoClave { get; set; }
        public Tipo tipoValorLISTSET { get; set; }
        public tipoDato tipoValor { get; set; }
        public Rol rol { get; set; }
        public LinkedList<Parametros> parametros = new LinkedList<Parametros>();
        public LinkedList<Parametros> retornos = new LinkedList<Parametros>();

        public ParseTreeNode nodo { get; set; }

        public enum Rol
        {
            VARIABLE,
            METODO,
            FUNCION,
            PROCEDIMIENTO
        }

        public Simbolo() { }

        public Simbolo(String id, Object valor, int fila, int columna, tipoDato tipo, Rol rol, 
            LinkedList<Parametros> parametros, LinkedList<Parametros> retornos, ParseTreeNode nodo)
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.rol = rol;
            this.parametros = parametros;
            this.retornos = retornos;
            this.nodo = nodo;
        }

        public Simbolo(String id, Object valor, int fila, int columna, //list con typeUSER
            tipoDato tipo, tipoDato tipoValor, String idTipo, Rol rol)
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.tipoValor = tipoValor;
            this.idTipo = idTipo;
            this.rol = rol;
        }

        public Simbolo(String id, Object valor, int fila, int columna,
            tipoDato tipo, tipoDato tipoValor, Tipo tipovalorsetlist, Rol rol)
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.tipoValor = tipoValor;
            this.tipoValorLISTSET = tipovalorsetlist;
            this.rol = rol;
        }

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

        public Simbolo(String id, Object valor, int fila, int columna, tipoDato tipo, String idtipo, Rol rol) //usertype
        {
            this.id = id;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.idTipo = idtipo;
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
            this.abierto = false;
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
