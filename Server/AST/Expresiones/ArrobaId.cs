using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class ArrobaId : Entorno, Expresion
    {
        public String id { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public ArrobaId(String id, int linea, int columna)
        {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            Simbolo encontrado = get(id, entorno, Simbolo.Rol.VARIABLE);
            if (encontrado != null)
            {
                return encontrado.tipo;
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            Simbolo encontrado = get(id, entorno, Simbolo.Rol.VARIABLE);
            if (encontrado != null)
            {
                tipoDato ti = encontrado.tipo;
                return encontrado;
            }
            else
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico, "Variable no existe: " + id));
                return tipoDato.errorSemantico;
            }
        }
    }
}
