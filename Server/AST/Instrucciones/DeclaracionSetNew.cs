using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class DeclaracionSetNew : Instruccion
    {
        public LinkedList<String> idList { get; set; }
        public Tipo tipoValor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public DeclaracionSetNew(LinkedList<String> idList, Tipo tipoValor,
            int linea, int columna)
        {
            this.idList = idList;
            this.tipoValor = tipoValor;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                foreach (String id in idList)
                {
                    Simbolo buscado = entorno.getEnActual(id.ToLower(), Simbolo.Rol.VARIABLE);
                    if (buscado == null)
                    {
                        if (tipoValor.tipo == tipoDato.id)
                        {
                            Simbolo sim = entorno.get(tipoValor.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                            if (sim == null)
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                        "El tipo de la variable del set no existe. El nombre es: " + tipoValor.id));
                                return tipoDato.errorSemantico;
                            }
                            else if (sim.valor is CreateType) {
                                CreateType simvalor = (CreateType)sim.valor;
                                entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), simvalor.Clone(), linea, columna,
                                       tipoDato.set, tipoValor.tipo, Simbolo.Rol.VARIABLE));
                            }
                            else
                            {
                                //sim es simbol osea lista o set
                                entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), sim.Clone(), linea, columna,
                                       tipoDato.set, sim.tipoValor, Simbolo.Rol.VARIABLE));
                            }
                        }                       
                        else
                        {
                            entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), new List<Object>(), linea, columna,
                                    tipoDato.set, tipoValor.tipo, Simbolo.Rol.VARIABLE));
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "Variable ya declara en el entorno actual. El nombre es: " + id));
                        return tipoDato.errorSemantico;
                    }
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Set"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
