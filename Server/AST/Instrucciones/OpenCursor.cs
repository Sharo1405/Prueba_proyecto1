using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class OpenCursor : Instruccion
    {
        public String idCursor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public OpenCursor(String idCursor, int linea, int col)
        {
            this.idCursor = idCursor;
            this.linea = linea;
            this.columna = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Simbolo variable = entorno.get(idCursor.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                if (variable != null)
                {
                    variable.abierto = true;
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, 
                        "El id del cursor no existe para abrirlo: " + idCursor));
                    return tipoDato.errorSemantico;
                }                
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                 "No se puede Guardar el cursos" + idCursor));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
