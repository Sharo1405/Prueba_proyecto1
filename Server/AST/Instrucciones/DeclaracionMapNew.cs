using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class DeclaracionMapNew : Instruccion
    {
        public LinkedList<String> idMap { get; set; }
        public tipoDato tipoClave { get; set; }
        public tipoDato tipoValor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public DeclaracionMapNew(LinkedList<String> idMap, tipoDato tipoClave,
            tipoDato tipoValor, int linea, int columna)
        {
            this.idMap = idMap;
            this.tipoClave = tipoClave;
            this.tipoValor = tipoValor;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                foreach (String id in idMap)
                {
                    Simbolo buscado = entorno.getEnActual(id.ToLower(), Simbolo.Rol.VARIABLE);
                    if (buscado == null)
                    {
                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), new HashSet<ClaveValor>(), linea, columna,
                                tipoDato.map, tipoClave, tipoValor, Simbolo.Rol.VARIABLE));
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
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Map"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
