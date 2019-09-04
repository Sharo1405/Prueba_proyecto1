using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;

namespace Server.AST.Expresiones
{
    class FuncionesNativasCadenas : Expresion
    {
        public String nativa { get; set; }
        public Expresion exp1 { get; set; }
        public Expresion exp2 { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public FuncionesNativasCadenas(String nativa,
            int linea, int columna)
        {
            this.nativa = nativa;
            this.linea = linea;
            this.columna = columna;
        }

        public FuncionesNativasCadenas(String nativa,
            Expresion exp1, int linea, int columna)
        {
            this.nativa = nativa;
            this.exp1 = exp1;
            this.linea = linea;
            this.columna = columna;
        }

        public FuncionesNativasCadenas(String nativa,
            Expresion exp1, Expresion exp2, int linea, int columna)
        {
            this.nativa = nativa;
            this.exp1 = exp1;
            this.exp2 = exp2;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return Operacion.tipoDato.cadena;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return this;
        }
    }
}
