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
    class DeclaraListNew : Instruccion
    {
        public LinkedList<String> idList { get; set; }
        public Tipo tipoValor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public DeclaraListNew(LinkedList<String> idList, Tipo tipoValor,
            int linea, int columna)
        {
            this.idList = idList;
            this.tipoValor = tipoValor;
            this.linea = linea;
            this.columna = columna;
        }


        public tipoDato comprobandoTipos(Entorno entorno, ErrorImpresion listas, Tipo tipoVal)
        {
            if (tipoVal.tipoValor is Tipo)
            {
                tiposList.AddLast(new Tipo(tipoVal.tipo, linea, columna));
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
                                "El tipo de la variable del list no existe. El nombre es: " + tipoVal.id));
                        return tipoDato.errorSemantico;
                    }
                }
                else if (tipoVal.tipo == tipoDato.booleano ||
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

        LinkedList<Tipo> tiposList = new LinkedList<Tipo>();

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
                        if (tt == tipoDato.ok)
                        {
                            Tipo aux = new Tipo(tipoDato.list, tiposList, linea, columna);
                            Lista listaGuardar = new Lista("item", new List<Object>(), tipoDato.list, tiposList, linea, columna);
                            entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), listaGuardar, linea, columna,
                                        tipoDato.list, aux.tipo, aux, Simbolo.Rol.VARIABLE));
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "Tipos no valido en el valor del list: " + id));
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
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el List"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
