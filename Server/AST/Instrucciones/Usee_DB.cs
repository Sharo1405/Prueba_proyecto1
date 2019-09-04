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
    class Usee_DB: Instruccion
    {
        public String usada { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Usee_DB(String usada, int linea, int columna)
        {
            this.usada = usada;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            object basse = management.buscarBaseDatos(usada);
            if (basse != null)
            {
                BaseDeDatos DB = (BaseDeDatos)basse;
                DB.isUse = true;
                management.baseEnUso = usada;
                return usada;
            }
            else
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "La base de datos especificada para usar NO EXISTE: " + usada));
                return tipoDato.errorSemantico;
            }
        }
    }
}
