using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.TipoDato
{
    class ElExcepcion : Entorno, Expresion
    {
        public String idExcepcion { get; set; }
        public int linea { get; set; }
        public int col { get; set; }


        public ElExcepcion(String idExcepcion, int linea, int col)
        {
            this.idExcepcion = idExcepcion;
            this.linea = linea;
            this.col = col;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return tipoDato.excepcion;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            Simbolo encontrado = get(idExcepcion, entorno, Simbolo.Rol.VARIABLE);
            if (encontrado != null)
            {
                if (encontrado.tipo == tipoDato.excepcion) {
                    return Convert.ToString(encontrado.valor);
                }
                else
                {
                    return tipoDato.errorSemantico;
                }
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }
    }
}
