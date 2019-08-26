using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class DeclaracionSetValores : Instruccion
    {
        public LinkedList<String> idSet { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public DeclaracionSetValores(LinkedList<String> idSet, Expresion valor,
            int linea, int columna)
        {
            this.idSet = idSet;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                foreach (String id in idSet) {
                    Simbolo s = entorno.getEnActual(id, Simbolo.Rol.VARIABLE);
                    if (s == null) {
                        Object setLista = valor.getValue(entorno, listas);
                        if (setLista is Auxiliar)
                        {
                            LinkedList<Tipo> tip = ((Auxiliar)setLista).listaTipos;
                            if (valor is Llaves) {
                                Tipo au = new Tipo(tipoDato.set, tip,linea, columna);
                                entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), ((Auxiliar)setLista).valorLlaves, linea, columna,
                                       tipoDato.set, au.tipo, au, Simbolo.Rol.VARIABLE));
                            }
                            else
                            {
                                //solo igualacion con una lista ya existente

                            }
                        }
                        else
                        {
                            if (setLista is tipoDato)
                            {
                                if ((tipoDato)setLista == tipoDato.errorSemantico)
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el List"));
                                    return tipoDato.errorSemantico;
                                }
                            }
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Variable ya existe"));
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
