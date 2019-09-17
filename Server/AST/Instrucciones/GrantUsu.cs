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
                userPass encontrado = new userPass();
                management.usuarios.TryGetValue(idUsu.ToLower(),out encontrado);

                if (encontrado.permisoBase.Equals(""))
                {
                    BaseDeDatos buscarBase = new BaseDeDatos();
                    if (management.basesExistentes.TryGetValue(idBase.ToLower(), out buscarBase))
                    {
                        encontrado.permisoBase = idBase.ToLower();
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna,
                            NodoError.tipoError.Semantico, "La base de datos que se quiere asignar en el Grant no existe: " + idUsu));
                        return tipoDato.errorSemantico;
                    }                    
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna,
                    NodoError.tipoError.Semantico, "El usuario ya tiene un permiso asignado: " + idUsu));
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
