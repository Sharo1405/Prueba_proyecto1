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

        LinkedList<Tipo> tiposSet = new LinkedList<Tipo>();

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                foreach (String id in idList)
                {
                    Simbolo buscado = entorno.getEnActual(id.ToLower(), Simbolo.Rol.VARIABLE);
                    if (buscado == null)
                    {
                        tipoDato tt = comprobandoTipos(entorno, listas, tipoValor);
                        if (tt == tipoDato.ok) {
                            Tipo aux = new Tipo(tipoDato.set, tiposSet, linea, columna);
                            Lista listaGuardar = new Lista(id.ToLower(), new List<Object>(), tipoDato.set, tiposSet, linea, columna);
                            entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), listaGuardar, linea, columna,
                                        tipoDato.set,aux.tipo, aux, Simbolo.Rol.VARIABLE));
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "Tipos no valido en el valor del set: " + id));
                            return tipoDato.errorSemantico;
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

        public tipoDato comprobandoTipos(Entorno entorno, ErrorImpresion listas, Tipo tipoVal)
        {
            if (tipoVal.tipoValor is Tipo)
            {
                tiposSet.AddLast(new Tipo(tipoVal.tipo, tipoVal.tipoValor, linea, columna));
                tipoDato t = comprobandoTipos(entorno, listas, tipoVal.tipoValor);
                if (t == tipoDato.errorSemantico)
                {
                    return tipoDato.errorSemantico;
                }
            }
            else
            {
                if (tipoVal.tipo == tipoDato.id)
                {
                    Simbolo sim = entorno.get(tipoVal.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                    if (sim == null)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "El tipo de la variable del set no existe. El nombre es: " + tipoVal.id));
                        return tipoDato.errorSemantico;
                    }                    
                }
                else if(tipoVal.tipo == tipoDato.booleano ||
                        tipoVal.tipo == tipoDato.cadena ||
                        tipoVal.tipo == tipoDato.date ||
                        tipoVal.tipo == tipoDato.decimall ||
                        tipoVal.tipo == tipoDato.entero ||
                        tipoVal.tipo == tipoDato.time)
                {                    
                    return tipoDato.ok;
                }
            }

            return tipoDato.ok;
        }
    }
}
