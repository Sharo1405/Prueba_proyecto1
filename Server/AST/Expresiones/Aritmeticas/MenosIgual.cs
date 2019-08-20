using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.Aritmeticas
{
    class MenosIgual: Expresion
    {
        public Expresion expresion1 { get; set; }
        public Expresion expresion2 { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public string id { get; set; }


        public MenosIgual(int linea, int columna, Expresion expresion2, String id)
        {
            this.id = id;
            this.expresion2 = expresion2;
            this.linea = linea;
            this.columna = columna;
        }

        public MenosIgual(int linea, int columna, Expresion expresion1, Expresion expresion2)
        {
            this.expresion1 = expresion1;
            this.expresion2 = expresion2;
            this.linea = linea;
            this.columna = columna;
        }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            throw new NotImplementedException();
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            throw new NotImplementedException();
        }
    }
}
