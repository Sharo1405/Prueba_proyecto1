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
    class GrantUsu : Instruccion
    {
        public String idUsu { get; set; }
        public String idBase { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public GrantUsu(String idUsu, String idBase, int linea,
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
                management.usuarios.TryGetValue(idUsu.ToLower(),out encontrado);

                if (encontrado == null)
                {
                    listas.impresiones.AddLast("WARNNING!! EL USUARIO NO EXISTE: " + idUsu + " Linea/Columna "
                                   + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                    return TipoExcepcion.excep.UserDontExists;
                }

                if (encontrado.permisoBase.Contains(idBase.ToLower()))
                {
                    BaseDeDatos buscarBase = new BaseDeDatos();
                    if (management.basesExistentes.TryGetValue(idBase.ToLower(), out buscarBase))
                    {
                        encontrado.permisoBase.Add(idBase.ToLower());
                    }
                    else
                    {
                        /*listas.errores.AddLast(new NodoError(linea, columna,
                            NodoError.tipoError.Semantico, "La base de datos que se quiere asignar en el Grant no existe: " + idUsu));*/
                        listas.impresiones.AddLast("WARNNING!! La base de datos que se quiere asignar en el Grant no existe: " + idUsu + " Linea/Columna "
                                   + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                        return TipoExcepcion.excep.BDDontExists;
                    }                    
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna,
                    NodoError.tipoError.Semantico, "El usuario ya tiene un permiso asignado: " + idUsu + ": " + idBase.ToLower()));
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
