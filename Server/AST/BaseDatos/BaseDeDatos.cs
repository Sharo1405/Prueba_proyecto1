using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.BaseDatos
{
    class BaseDeDatos : Instruccion
    {
        public String idbase { get; set; }
        public Dictionary<String, Simbolo> procedures = new Dictionary<string, Simbolo>();
        public Dictionary<String, CreateType> usertypes = new Dictionary<string, CreateType>();
        public Dictionary<String, Tabla> Tabla = new Dictionary<string, Tabla>();
        public Boolean isUse = false;
        public Boolean ifnotexist = false;
        public int linea { get; set; }
        public int columna { get; set; }

        public BaseDeDatos(String idbase, Boolean ifnotexist,
            int linea, int columna)
        {
            this.idbase = idbase;
            this.ifnotexist = ifnotexist;
            this.linea = linea;
            this.columna = columna;
        }

        public BaseDeDatos(String idbase,
            int linea, int columna)
        {
            this.idbase = idbase;
            this.linea = linea;
            this.columna = columna;
        }

        public BaseDeDatos()
        {

        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                management.basesExistentes.Add(idbase, this);
            }
            catch (ArgumentException e)
            {                
                if (ifnotexist is false)
                {
                    listas.impresiones.AddLast("WARNNING!! BASE DE DATOS YA EXISTE: " + idbase);
                }                
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
