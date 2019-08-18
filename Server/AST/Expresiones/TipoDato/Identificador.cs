using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.TipoDato
{
    class Identificador : Entorno, Expresion
    {

        public String id;
        public tipoDato tipo;
        public int linea;
        public int columna;

        public Identificador(String id, tipoDato tipo, int linea, int columna)
        {
            this.id = id;
            this.tipo = tipo;
            this.linea = linea;
            this.columna = columna;
        }

        public Identificador(String id, int linea, int columna)
        {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {

            Simbolo encontrado = get(id, entorno);
            if (encontrado != null)
            {
                return encontrado.tipo;
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            Simbolo encontrado = get(id, entorno);
            if (encontrado != null)
            {
                tipoDato ti = encontrado.tipo;
                return encontrado.valor;
            }
            else
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico, "Variable no existe: " + id));
                return tipoDato.errorSemantico;
            }
        }
    }
}
