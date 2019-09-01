using Server.AST.Entornos;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones.Aritmeticas
{
    class Decremento: Expresion
    {
        public Expresion idExp { get; set; }

        public String id { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Decremento(Expresion id, int linea, int columna)
        {
            this.idExp = id;
            this.linea = linea;
            this.columna = columna;
        }

        public Decremento(String id, int linea, int columna)
        {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }
   

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            if (idExp != null)
            {
                return idExp.getType(entorno, listas);
            }
            else {
                ArrobaId ididid = new ArrobaId(id, linea, columna);
                return ididid.getType(entorno, listas);
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                if (idExp != null)
                {
                    if (idExp is ArrobaId)
                    {
                        ArrobaId id2 = (ArrobaId)idExp;
                        tipoDato tipovar = idExp.getType(entorno, listas);
                        Simbolo variable = entorno.get(id2.id, entorno, Simbolo.Rol.VARIABLE);
                        if (tipovar == tipoDato.entero)
                        {
                            int valorantiguo = Convert.ToInt32(variable.valor);
                            variable.valor = Convert.ToInt32(variable.valor) - 1;
                            return valorantiguo;
                        }
                        else if (tipovar == tipoDato.decimall)
                        {
                            Double valorantiguo = Double.Parse(Convert.ToString(variable.valor));
                            variable.valor = Double.Parse(Convert.ToString(variable.valor)) - 1.0;
                            return valorantiguo;
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna,
                                NodoError.tipoError.Semantico, "No se puede realizar el Decremento -- porque el tipo no lo admite: " + Convert.ToString(tipovar)));
                            return tipoDato.errorSemantico;
                        }
                    }
                    else if (idExp is ListaPuntos)
                    {
                        object valorExpid = idExp.getValue(entorno, listas);
                        tipoDato tipoExpId = idExp.getType(entorno, listas);
                        if (tipoExpId != tipoDato.entero && tipoExpId != tipoDato.decimall)
                        {
                            listas.errores.AddLast(new NodoError(linea, columna,
                                NodoError.tipoError.Semantico, "El tipo del incremento no es numerico sino: " + Convert.ToString(tipoExpId)));
                            return tipoDato.errorSemantico;
                        }

                        if (tipoExpId == tipoDato.entero)
                        {
                            valorExpid = Convert.ToInt32(valorExpid) - 1;
                        }
                        else if (tipoExpId == tipoDato.decimall)
                        {
                            valorExpid = Convert.ToDouble(valorExpid) - 1.0;
                        }
                        ListaPuntos l = (ListaPuntos)idExp;
                        SetearvaloresAccesos setear = new SetearvaloresAccesos(l, valorExpid, this.linea, this.columna);
                        setear.ejecutar(entorno, listas);
                        return valorExpid;
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna,
                            NodoError.tipoError.Semantico, "No se puede realizar el Decremento -- porque no es id el primer arguemnto"));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    ArrobaId ididid = new ArrobaId(id, linea, columna);
                    tipoDato tipovar = ididid.getType(entorno, listas);
                    Simbolo variable = entorno.get(id, entorno, Simbolo.Rol.VARIABLE);
                    if (tipovar == tipoDato.entero)
                    {
                        int valorantiguo = Convert.ToInt32(variable.valor);
                        variable.valor = Convert.ToInt32(variable.valor) - 1;
                        return valorantiguo;
                    }
                    else if (tipovar == tipoDato.decimall)
                    {
                        Double valorantiguo = Double.Parse(Convert.ToString(variable.valor));
                        variable.valor = Double.Parse(Convert.ToString(variable.valor)) - 1.0;
                        return valorantiguo;
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna,
                            NodoError.tipoError.Semantico, "No se puede realizar el Decremento -- porque el tipo no lo admite: " + Convert.ToString(tipovar)));
                        return tipoDato.errorSemantico;
                    }
                }
            }
            catch (Exception e)
            { }
            listas.errores.AddLast(new NodoError(linea, columna,
                NodoError.tipoError.Semantico, "No se puede realizar el Decremento --"));
            return tipoDato.errorSemantico;
        }
    }
}
