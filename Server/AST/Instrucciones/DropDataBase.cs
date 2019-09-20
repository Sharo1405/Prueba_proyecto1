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
    class DropDataBase : Instruccion
    {
        public String usada { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public DropDataBase(String usada, int linea, int columna)
        {
            this.usada = usada;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            tipoDato basse = management.borrarBaseDatos(usada);
            if (basse == tipoDato.ok)
            {
                return basse;
            }
            else
            {
                listas.impresiones.AddLast("WARNINGGGGGGGGGG!!!!!!!!!!  La base de datos especificada para eliminar NO EXISTE: " + usada);
                return TipoExcepcion.excep.BDDontExists;
            }
        }
    }
}
