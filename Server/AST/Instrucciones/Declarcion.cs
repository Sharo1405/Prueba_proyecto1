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
    class Declarcion : Instruccion
    {
        public Tipo tipo { get; set; }
        public LinkedList<String> ids { get; set; }        

        public Declarcion(Tipo type, LinkedList<String> id)
        {
            this.tipo = type;
            this.ids = id;          
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {

            foreach (String id in ids) {
                Simbolo buscado = entorno.getEnActual(id, Simbolo.Rol.VARIABLE);
                if(buscado == null) {
                    if (tipo.tipo == tipoDato.entero)
                    {
                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), 0, tipo.linea, tipo.columna,
                            tipo.tipo, Simbolo.Rol.VARIABLE));
                    }
                    else if (tipo.tipo == tipoDato.decimall)
                    {
                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), 0.0, tipo.linea, tipo.columna, 
                            tipo.tipo, Simbolo.Rol.VARIABLE));
                    }
                    else if (tipo.tipo == tipoDato.booleano)
                    {
                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), false, tipo.linea, tipo.columna, 
                            tipo.tipo, Simbolo.Rol.VARIABLE));
                    }
                    else
                    {
                        if (tipo.tipo == tipoDato.id)
                        {
                            entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), null, tipo.linea, tipo.columna,
                                tipo.tipo, tipo.id,Simbolo.Rol.VARIABLE));
                        }
                        else {
                            entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), null, tipo.linea, tipo.columna,
                                tipo.tipo, Simbolo.Rol.VARIABLE));
                        }
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(tipo.linea,tipo.columna, NodoError.tipoError.Semantico, "Variable ya declara en el entorno actual. El nombre es: " + id));
                }
            }
            return "ok";
        }
    }
}
