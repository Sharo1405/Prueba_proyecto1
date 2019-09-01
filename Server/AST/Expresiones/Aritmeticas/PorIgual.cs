using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones.Aritmeticas
{
    class PorIgual: Operacion, Expresion
    {
        public PorIgual(int linea, int columna, Expresion expresion1, Expresion expresion2, string operador, int cantExp)
            : base(linea, columna, expresion1, expresion2)
        { }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            throw new NotImplementedException();
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Boolean arrobaid = false;
                Simbolo s = new Simbolo();
                object valorExp1 = expresion1.getValue(entorno, listas);
                object valorExp2 = expresion2.getValue(entorno, listas);

                tipoDato tipoExp1 = expresion1.getType(entorno, listas);
                tipoDato tipoExp2 = expresion2.getType(entorno, listas);


                if (tipoExp1 != tipoDato.entero && tipoExp1 != tipoDato.decimall)
                {
                    listas.errores.AddLast(new NodoError(linea, columna,
                        NodoError.tipoError.Semantico, "No se puede realizar la operacion *= por no son tipo numerico las expresiones"));
                    return tipoDato.errorSemantico;
                }

                if (tipoExp2 != tipoDato.entero && tipoExp2 != tipoDato.decimall)
                {
                    listas.errores.AddLast(new NodoError(linea, columna,
                        NodoError.tipoError.Semantico, "No se puede realizar la operacion *= por no son tipo numerico las expresiones"));
                    return tipoDato.errorSemantico;
                }


                if (valorExp1 is Simbolo)
                {
                    arrobaid = true;
                    s = (Simbolo)valorExp1;

                }
                if (valorExp2 is Simbolo)
                {
                    Simbolo ss = (Simbolo)valorExp2;
                    valorExp2 = ss.valor;
                }

                if (expresion1 is ListaPuntos && expresion2 is ListaPuntos)
                {
                    if (arrobaid)
                    {
                        if (s.tipo == tipoDato.entero)
                        {
                            s.valor = Convert.ToInt32(s.valor) * Convert.ToInt32(valorExp2);

                        }
                        else if (s.tipo == tipoDato.decimall)
                        {
                            s.valor = Convert.ToDouble(s.valor) * Convert.ToDouble(valorExp2);
                        }
                        return s.valor;
                    }
                    else
                    {
                        if (tipoExp1 == tipoDato.entero)
                        {
                            return Convert.ToInt32(valorExp1) * Convert.ToInt32(valorExp2);
                        }
                        else if (tipoExp1 == tipoDato.decimall)
                        {
                            return Convert.ToDouble(valorExp1) * Convert.ToDouble(valorExp2);
                        }
                    }
                }
                else if (expresion1 is ListaPuntos)
                {
                    if (arrobaid)
                    {
                        if (s.tipo == tipoDato.entero)
                        {
                            s.valor = Convert.ToInt32(s.valor) * Convert.ToInt32(valorExp2);

                        }
                        else if (s.tipo == tipoDato.decimall)
                        {
                            s.valor = Convert.ToDouble(s.valor) * Convert.ToDouble(valorExp2);
                        }
                        return s.valor;
                    }
                    else
                    {
                        if (tipoExp1 == tipoDato.entero)
                        {
                            return Convert.ToInt32(valorExp1) * Convert.ToInt32(valorExp2);
                        }
                        else if (tipoExp1 == tipoDato.decimall)
                        {
                            return Convert.ToDouble(valorExp1) * Convert.ToDouble(valorExp2);
                        }
                    }
                }
                else if (expresion2 is ListaPuntos)
                {
                    if (s.tipo == tipoDato.entero)
                    {
                        s.valor = Convert.ToInt32(s.valor) * Convert.ToInt32(valorExp2);

                    }
                    else if (s.tipo == tipoDato.decimall)
                    {
                        s.valor = Convert.ToDouble(s.valor) * Convert.ToDouble(valorExp2);
                    }
                    return s.valor;
                }
                else
                {
                    if (s.tipo == tipoDato.entero)
                    {
                        s.valor = Convert.ToInt32(s.valor) * Convert.ToInt32(valorExp2);

                    }
                    else if (s.tipo == tipoDato.decimall)
                    {
                        s.valor = Convert.ToDouble(s.valor) * Convert.ToDouble(valorExp2);
                    }
                    return s.valor;
                }
            }
            catch (Exception e)
            {
            }
            listas.errores.AddLast(new NodoError(linea, columna,
                NodoError.tipoError.Semantico, "No se puede realizar la operacion *= "));
            return tipoDato.errorSemantico;
        }
    }
}
