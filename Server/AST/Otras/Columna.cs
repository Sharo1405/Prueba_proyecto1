using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Otras
{
    class Columna
    {
        public String idColumna { get; set; }
        public tipoDato tipo { get; set; }
        public tipoDato tipoValor { get; set; } //por si hay listas o sets
        public String idTipo = "";
        public LinkedList<object> valorColumna = new LinkedList<object>();
        public Boolean primaryKey = false;
        public int ultimovalorincrementable = -1;

        public Columna()
        {

        }

        public Columna(String idCol, tipoDato tipo, tipoDato tipoValor, 
            Boolean pk)
        {
            this.idColumna = idCol;
            this.tipo = tipo;
            this.tipoValor = tipoValor;
            this.valorColumna = new LinkedList<object>();
            this.primaryKey = pk;
        }


        public Columna(String idCol, tipoDato tipo, String idTypeUser, tipoDato tipoValor, Boolean pk)
        {
            this.idColumna = idCol;
            this.tipo = tipo;
            this.idTipo = idTypeUser;
            this.tipoValor = tipoValor;
            this.valorColumna = new LinkedList<object>();
            this.primaryKey = pk;
        }


        public Columna(String idCol, tipoDato tipo, tipoDato tipoValor, LinkedList<object> valorColumna,
            Boolean pk)
        {
            this.idColumna = idCol;
            this.tipo = tipo;
            this.tipoValor = tipoValor;
            this.valorColumna = valorColumna;
            this.primaryKey = pk;
        }
    }
}
