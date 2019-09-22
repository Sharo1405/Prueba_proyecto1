using Server.AST.BaseDatos;
using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class RevokeUsu: Instruccion
    {
        public String idUsu { get; set; }
        public String idBase { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public RevokeUsu(String idUsu, String idBase, int linea,
            int columna)
        {
            this.idUsu = idUsu;
            this.idBase = idBase;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                userPass encontrado = null;
                management.usuarios.TryGetValue(idUsu.ToLower(), out encontrado);

                if (encontrado == null)
                {
                    listas.impresiones.AddLast("WARNNING!! EL USUARIO NO EXISTE: " + idUsu + " Linea/Columna "
                                                                + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                    return TipoExcepcion.excep.UserDontExists;
                }

                if (encontrado.permisoBase.Count != 0)
                {
                    if (encontrado.permisoBase.Contains(idBase.ToLower()))
                    {
                        encontrado.permisoBase.Remove(idBase.ToLower());
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna,
                            NodoError.tipoError.Semantico, "El permiso no coincide con el existente en el usuario: " + idUsu));
                        return tipoDato.errorSemantico;
                    }
                    
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna,
                    NodoError.tipoError.Semantico, "El usuario no tiene un permiso asignado: " + idUsu));
                    return tipoDato.errorSemantico;
                }
            }
            catch (ArgumentException e)
            {
                listas.errores.AddLast(new NodoError(linea, columna,
                    NodoError.tipoError.Semantico, "El usuario ya existe: " + idUsu));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
