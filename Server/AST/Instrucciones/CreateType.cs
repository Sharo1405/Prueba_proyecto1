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
    class CreateType : Instruccion
    {
        public String idType { get; set; }
        public Boolean ifnotexists { get; set; }
        public LinkedList<itemType> itemTypee = new LinkedList<itemType>();
        public int linea { get; set; }
        public int columna { get; set; }
        
        public CreateType()
        {

        }

        public CreateType(String idType, LinkedList<itemType> itemType,
            int linea, int columna)
        {
            this.idType = idType;
            this.itemTypee = itemType;
            this.linea = linea;
            this.columna = columna;
            this.ifnotexists = false;
        }

        public CreateType(String idType, LinkedList<itemType> itemType,
            int linea, int columna, Boolean ifnotexists)
        {
            this.idType = idType;
            this.itemTypee = itemType;
            this.linea = linea;
            this.columna = columna;
            this.ifnotexists = ifnotexists;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Simbolo buscado = entorno.getEnActual(idType.ToLower(), Simbolo.Rol.VARIABLE);
                if (buscado == null)
                {                    

                    entorno.setSimbolo(idType.ToLower(), new Simbolo(idType.ToLower(), this, linea, columna, tipoDato.id, Simbolo.Rol.VARIABLE));
                }
                else
                {
                    if (ifnotexists == false)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "Variable ya declara en el entorno actual. El nombre es: " + idType));
                        return TipoExcepcion.excep.TypeAlreadyExists;
                    }
                    else
                    {
                        listas.impresiones.AddLast("WARNINGGGGGGGGGGGGGGGGGGGGGGGGGG!!!!!!!!!!!!!!!!!! Intento de crar un type user que ya existe " + idType);
                    }
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Type: " + idType));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }

        public object Clone(LinkedList<itemType> itemType)
        {
            this.itemTypee = new LinkedList<itemType>();
            this.itemTypee = itemType;
            return this.MemberwiseClone();
        }
    }
}
