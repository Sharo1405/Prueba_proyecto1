using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.Aritmeticas
{
    class MasIgual: Expresion
    {
        public Expresion expresion1 { get; set; }
        public Expresion expresion2 { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public string id { get; set; }


        public MasIgual(int linea, int columna, Expresion expresion2, String id)
        {
            this.id = id;
            this.expresion2 = expresion2;
            this.linea = linea;
            this.columna = columna;
        }

        public MasIgual(int linea, int columna, Expresion expresion1, Expresion expresion2)
        {
            this.expresion1 = expresion1;
            this.expresion2 = expresion2;
            this.linea = linea;
            this.columna = columna;
        }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            if (expresion1 != null)
            {
                return expresion1.getType(entorno, listas);
            }
            else
            {
                ArrobaId ididid = new ArrobaId(id, linea, columna);
                return ididid.getType(entorno, listas);
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                if (expresion1 != null)
                {
                    //accesos
                }
                else
                {
                    ArrobaId ididid = new ArrobaId(id, linea, columna);
                    tipoDato tipovar = ididid.getType(entorno, listas);
                    Object valor2 = expresion2.getValue(entorno, listas);
                    Simbolo variable = entorno.get(id, entorno, Simbolo.Rol.VARIABLE);
                    if (tipovar == tipoDato.entero)
                    {
                        int valorantiguo = Convert.ToInt32(variable.valor);
                        variable.valor = Convert.ToInt32(variable.valor) + Convert.ToInt32(valor2);
                        return valorantiguo;
                    }
                    else if (tipovar == tipoDato.decimall)
                    {
                        Double valorantiguo = Double.Parse(Convert.ToString(variable.valor));
                        variable.valor = Double.Parse(Convert.ToString(variable.valor)) + Double.Parse(Convert.ToString(valor2));
                        return valorantiguo;
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna,
                            NodoError.tipoError.Semantico, "No se puede realizar la operacion += porque el tipo no lo admite: " + Convert.ToString(tipovar)));
                        return tipoDato.errorSemantico;
                    }
                }
            }
            catch (Exception e)
            { 
            }
            listas.errores.AddLast(new NodoError(linea, columna,
                NodoError.tipoError.Semantico, "No se puede realizar la operacion += "));
            return tipoDato.errorSemantico;
        }
    }
}
