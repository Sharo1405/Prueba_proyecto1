using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class CursorLlenar : Instruccion
    {
        public String idCursor { get; set; }
        public Expresion selectt { get; set; }
        public int linea { get; set; }
        public int columana { get; set; }

        public CursorLlenar(String idCursor, Expresion selectt,
            int linea, int columna)
        {
            this.idCursor = idCursor;
            this.selectt = selectt;
            this.linea = linea;
            this.columana = columna;
        }


        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Simbolo variable = entorno.get(idCursor.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                if (variable == null)
                {
                    object obj = selectt.getValue(entorno, listas, management);

                    entorno.setSimbolo(idCursor.ToLower(), new Simbolo(idCursor.ToLower(), obj, this.linea, this.columana,
                                    tipoDato.cursor, Simbolo.Rol.VARIABLE));
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columana, NodoError.tipoError.Semantico,
                         "No se puede Guardar el cursor porque ya existe ese id: " + idCursor));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columana, NodoError.tipoError.Semantico,
                 "No se puede Guardar el cursor" + idCursor));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
