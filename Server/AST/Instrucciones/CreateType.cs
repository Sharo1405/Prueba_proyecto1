using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class CreateType : Instruccion, ICloneable
    {
        public String idType { get; set; }
        public Boolean ifnotexists { get; set; }
        public LinkedList<itemType> itemTypee = new LinkedList<itemType>();
        public int linea { get; set; }
        public int columna { get; set; }

        public CreateType(String idType, LinkedList<itemType> itemType,
            int linea, int columna)
        {
            this.idType = idType;
            this.itemTypee = itemType;
            this.linea = linea;
            this.columna = columna;
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

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Simbolo buscado = entorno.getEnActual(idType.ToLower(), Simbolo.Rol.VARIABLE);
                if (buscado == null)
                {
                    foreach (itemType ty in itemTypee)
                    {
                        if (ty.tipo.tipo == tipoDato.id)
                        {
                            Simbolo buscado2 = entorno.getEnActual(ty.tipo.id.ToLower(), Simbolo.Rol.VARIABLE);
                            if (buscado2 != null)
                            {
                                //arreglar solo declarar sin el clonar
                                //CreateType typeComoTipo =(CreateType) buscado2.valor;
                                ty.valor = null;//typeComoTipo.Clone();
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de esa variable del Type User no existe: Tipo:" + ty.tipo.id.ToLower()));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else if (ty.tipo.tipo == tipoDato.entero)
                        {                            
                            ty.valor = 0;
                        }
                        else if (ty.tipo.tipo == tipoDato.decimall)
                        {                            
                            ty.valor = 0.0;
                        }
                        else if (ty.tipo.tipo == tipoDato.booleano)
                        {
                            ty.valor = false;
                        }
                        else
                        {
                            ty.valor = null;
                        }
                    }

                    entorno.setSimbolo(idType.ToLower(), new Simbolo(idType.ToLower(), this, linea, columna, tipoDato.id, Simbolo.Rol.VARIABLE));
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                        "Variable ya declara en el entorno actual. El nombre es: " + idType));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Type: " + idType));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
