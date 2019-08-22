using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class DeclaracionSetNew : Instruccion
    {
        public LinkedList<String> idList { get; set; }
        public tipoDato tipoValor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public DeclaracionSetNew(LinkedList<String> idList, tipoDato tipoValor,
            int linea, int columna)
        {
            this.idList = idList;
            this.tipoValor = tipoValor;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                foreach (String id in idList)
                {
                    Simbolo buscado = entorno.getEnActual(id.ToLower(), Simbolo.Rol.VARIABLE);
                    if (buscado == null)
                    {
                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), new List<Object>(), linea, columna,
                                tipoDato.set, tipoValor, Simbolo.Rol.VARIABLE));
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "Variable ya declara en el entorno actual. El nombre es: " + id));
                        return tipoDato.errorSemantico;
                    }
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Set"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
