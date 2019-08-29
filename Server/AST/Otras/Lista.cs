using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Otras
{
    class Lista
    {
        public String idLista { get; set; }
        public List<Object> listaValores = new List<object>();
        public LinkedList<Tipo> tiposCascada = new LinkedList<Tipo>();
        public tipoDato tipoGeneral { get; set; }
        public tipoDato tipoValor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Lista() { }

        public Lista(String idLista, List<Object> listaValores,
            tipoDato tipoGeneral, LinkedList<Tipo> tiposCascada, int linea, int columna)
        {
            this.idLista = idLista;
            this.listaValores = listaValores;
            this.tipoGeneral = tipoGeneral;
            this.tiposCascada = tiposCascada;
            this.linea = linea;
            this.columna = columna;
        }

        public Lista(String idLista, List<Object> listaValores, 
            tipoDato tipoGeneral, tipoDato tipoValor, int linea, int columna)
        {
            this.idLista = idLista;
            this.listaValores = listaValores;
            this.tipoGeneral = tipoGeneral;
            this.tipoValor = tipoValor;
            this.linea = linea;
            this.columna = columna;
        }
    }
}
