using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class truncateTabla : Instruccion
    {
        public String idTabla { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public truncateTabla(String idTabla, int linea, int col)
        {
            this.idTabla = idTabla;
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Object inUse = management.getInUse();
                if (inUse != null)
                {
                    BaseDeDatos basee = (BaseDeDatos)inUse;
                    Tabla encontrado = new Tabla();
                    try
                    {
                        Tabla encontrado2 = new Tabla();
                        if (basee.Tabla.TryGetValue(idTabla.ToLower(), out encontrado2))
                        {
                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                            {
                                kvp.Value.valorColumna.Clear();
                            }

                        }
                        else
                        {
                            listas.impresiones.AddLast("WARNNING!! ESA TABLA NO EXISTE: " + idTabla);
                            return TipoExcepcion.excep.TableDontExists;
                        }

                    }
                    catch (ArgumentException e)
                    {
                         listas.impresiones.AddLast("WARNNING!! ESA TABLA NO EXISTE: " + idTabla);
                        return TipoExcepcion.excep.TableDontExists;
                    }
                }
                else
                {

                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                "La base de datos EN USO no fue encontrada"));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                "No se puede realizar el TRUNCATE de la tabla" + idTabla));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
