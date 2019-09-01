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
            
            return expresion1.getType(entorno, listas);
            
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
                        NodoError.tipoError.Semantico, "No se puede realizar la operacion += por no son tipo numerico las expresiones"));
                    return tipoDato.errorSemantico;
                }

                if (tipoExp2 != tipoDato.entero && tipoExp2 != tipoDato.decimall)
                {
                    listas.errores.AddLast(new NodoError(linea, columna,
                        NodoError.tipoError.Semantico, "No se puede realizar la operacion += por no son tipo numerico las expresiones"));
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
                    if (tipoExp1 == tipoDato.entero)
                    {
                        valorExp1 = Convert.ToInt32(valorExp1) + Convert.ToInt32(valorExp2);
                    }
                    else if (tipoExp1 == tipoDato.decimall)
                    {
                        valorExp1 = Convert.ToDouble(valorExp1) + Convert.ToDouble(valorExp2);
                    }
                    ListaPuntos l = (ListaPuntos)expresion1;
                    SetearvaloresAccesos setear = new SetearvaloresAccesos(l, valorExp1, this.linea, this.columna);
                    setear.ejecutar(entorno, listas);
                    return valorExp1;
                }
                else if (expresion1 is ListaPuntos)
                {                                 
                    if (tipoExp1 == tipoDato.entero)
                    {
                        valorExp1 = Convert.ToInt32(valorExp1) + Convert.ToInt32(valorExp2);
                    }
                    else if (tipoExp1 == tipoDato.decimall)
                    {
                        valorExp1 = Convert.ToDouble(valorExp1) + Convert.ToDouble(valorExp2);
                    }
                    ListaPuntos l = (ListaPuntos)expresion1;
                    SetearvaloresAccesos setear = new SetearvaloresAccesos(l, valorExp1, this.linea, this.columna);
                    setear.ejecutar(entorno, listas);
                    return valorExp1;
                }
                else if (expresion2 is ListaPuntos)
                {
                    if (s.tipo == tipoDato.entero)
                    {
                        s.valor = Convert.ToInt32(s.valor) + Convert.ToInt32(valorExp2);

                    }
                    else if (s.tipo == tipoDato.decimall)
                    {
                        s.valor = Convert.ToDouble(s.valor) + Convert.ToDouble(valorExp2);
                    }
                    return s.valor;
                }
                else
                {
                    if (s.tipo == tipoDato.entero)
                    {
                        s.valor = Convert.ToInt32(s.valor) + Convert.ToInt32(valorExp2);

                    }else if (s.tipo == tipoDato.decimall)
                    {
                        s.valor = Convert.ToDouble(s.valor) + Convert.ToDouble(valorExp2);
                    }
                    return s.valor;
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
