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
    class DividirIgual: Expresion
    {
        public Expresion expresion1 { get; set; }
        public Expresion expresion2 { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public string id { get; set; }


        public DividirIgual(int linea, int columna, Expresion expresion2, String id)
        {
            this.expresion1 = null;
            this.id = id;
            this.expresion2 = expresion2;
            this.linea = linea;
            this.columna = columna;
        }

        public DividirIgual(int linea, int columna, Expresion expresion1, Expresion expresion2)
        {
            this.id = "";
            this.expresion1 = expresion1;
            this.expresion2 = expresion2;
            this.linea = linea;
            this.columna = columna;
        }

        public DividirIgual(int linea, int columna, Expresion expresion1, Expresion expresion2, String id)
        {
            this.expresion1 = expresion1;
            this.expresion2 = expresion2;
            this.linea = linea;
            this.columna = columna;
            this.id = id;
        }

        tipoDato tipoOpcion1 = tipoDato.errorSemantico;
        tipoDato tipoOpcion2 = tipoDato.errorSemantico;
        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            if (id.Equals(""))
            {
                return expresion1.getType(entorno, listas);
            }
            else if (expresion1 is ListaPuntos)
            {
                return tipoOpcion1;
            }
            else
            {
                return tipoOpcion2;
            }

        }
        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Boolean arrobaid = false;
                Simbolo s = new Simbolo();

                object valorExp2 = expresion2.getValue(entorno, listas);
                tipoDato tipoExp2 = expresion2.getType(entorno, listas);


                if (tipoExp2 != tipoDato.entero && tipoExp2 != tipoDato.decimall)
                {
                    listas.errores.AddLast(new NodoError(linea, columna,
                        NodoError.tipoError.Semantico, "No se puede realizar la operacion /= por no son tipo numerico las expresiones"));
                    return tipoDato.errorSemantico;
                }


                if (valorExp2 is Simbolo)
                {
                    Simbolo ss = (Simbolo)valorExp2;
                    valorExp2 = ss.valor;
                }

                if (id.Equals(""))
                {

                    object valorExp1 = expresion1.getValue(entorno, listas);
                    tipoDato tipoExp1 = expresion1.getType(entorno, listas);

                    if (tipoExp1 != tipoDato.entero && tipoExp1 != tipoDato.decimall)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna,
                            NodoError.tipoError.Semantico, "No se puede realizar la operacion /= por no son tipo numerico las expresiones"));
                        return tipoDato.errorSemantico;
                    }

                    if (valorExp1 is Simbolo)
                    {
                        arrobaid = true;
                        s = (Simbolo)valorExp1;
                        valorExp1 = s.valor;
                    }

                    if (expresion1 is ListaPuntos && expresion2 is ListaPuntos)
                    {
                        if (tipoExp1 == tipoDato.entero)
                        {
                            valorExp1 = Convert.ToInt32(valorExp1) / Convert.ToInt32(valorExp2);
                        }
                        else if (tipoExp1 == tipoDato.decimall)
                        {
                            valorExp1 = Convert.ToDouble(valorExp1) / Convert.ToDouble(valorExp2);
                        }
                        ListaPuntos l = (ListaPuntos)expresion1;
                        SetearvaloresAccesos setear = new SetearvaloresAccesos(l, valorExp1, this.linea, this.columna, tipoExp1);
                        setear.ejecutar(entorno, listas);
                        return valorExp1;
                    }
                    else if (expresion1 is ListaPuntos)
                    {
                        if (tipoExp1 == tipoDato.entero)
                        {
                            valorExp1 = Convert.ToInt32(valorExp1) / Convert.ToInt32(valorExp2);
                        }
                        else if (tipoExp1 == tipoDato.decimall)
                        {
                            valorExp1 = Convert.ToDouble(valorExp1) / Convert.ToDouble(valorExp2);
                        }
                        ListaPuntos l = (ListaPuntos)expresion1;
                        SetearvaloresAccesos setear = new SetearvaloresAccesos(l, valorExp1, this.linea, this.columna, tipoExp1);
                        setear.ejecutar(entorno, listas);
                        return valorExp1;
                    }
                    else if (expresion2 is ListaPuntos)
                    {
                        if (s.tipo == tipoDato.entero)
                        {
                            s.valor = Convert.ToInt32(s.valor) / Convert.ToInt32(valorExp2);

                        }
                        else if (s.tipo == tipoDato.decimall)
                        {
                            s.valor = Convert.ToDouble(s.valor) / Convert.ToDouble(valorExp2);
                        }
                        return s.valor;
                    }
                    else
                    {
                        if (s.tipo == tipoDato.entero)
                        {
                            s.valor = Convert.ToInt32(s.valor) / Convert.ToInt32(valorExp2);

                        }
                        else if (s.tipo == tipoDato.decimall)
                        {
                            s.valor = Convert.ToDouble(s.valor) / Convert.ToDouble(valorExp2);
                        }
                        return s.valor;
                    }
                }
                else if (expresion1 == null)
                {
                    Simbolo jk = entorno.get(id, entorno, Simbolo.Rol.VARIABLE);
                    if (jk != null)
                    {                        

                        if (jk.tipo == tipoExp2)
                        {
                            if (jk.tipo == tipoDato.entero)
                            {
                                jk.valor = Convert.ToInt32(jk.valor) / Convert.ToInt32(valorExp2);

                            }
                            else if (jk.tipo == tipoDato.decimall)
                            {
                                jk.valor = Convert.ToDouble(jk.valor) / Convert.ToDouble(valorExp2);
                            }
                            return jk.valor;
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna,
                                NodoError.tipoError.Semantico, "No se puede realizar la operacion -= porque no existe la variable" + id));
                        return tipoDato.errorSemantico;
                    }

                }
                else
                {
                    if (expresion1 is ListaPuntos)
                    {
                        ListaPuntos a = (ListaPuntos)expresion1;
                        ListaPuntos sett = new ListaPuntos(id, a.ExpSeparadasPuntos, this.linea, this.columna);
                        object valorExp1 = sett.getValue(entorno, listas);
                        tipoDato tipoExp1 = sett.getType(entorno, listas);
                        tipoOpcion1 = tipoExp1;

                        if (tipoExp1 != tipoDato.entero && tipoExp1 != tipoDato.decimall)
                        {
                            listas.errores.AddLast(new NodoError(linea, columna,
                                NodoError.tipoError.Semantico, "No se puede realizar la operacion /= por no son tipo numerico las expresiones"));
                            return tipoDato.errorSemantico;
                        }

                        if (valorExp1 is Simbolo)
                        {
                            arrobaid = true;
                            s = (Simbolo)valorExp1;
                            valorExp1 = s.valor;
                        }

                        if (tipoExp1 == tipoDato.entero)
                        {
                            valorExp1 = Convert.ToInt32(valorExp1) / Convert.ToInt32(valorExp2);
                        }
                        else if (tipoExp1 == tipoDato.decimall)
                        {
                            valorExp1 = Convert.ToDouble(valorExp1) / Convert.ToDouble(valorExp2);
                        }

                        SetearvaloresAccesos setear = new SetearvaloresAccesos(id, a, valorExp1, this.linea, this.columna, tipoExp1);
                        setear.ejecutar(entorno, listas);
                        return valorExp1;
                    }
                    else
                    {
                        ArrobaId a = new ArrobaId(id, this.linea, this.columna);
                        Expresion exp = (Expresion)a;
                        ListaPuntos b = new ListaPuntos(exp, expresion1, this.linea, this.columna);

                        object valorExp1 = b.getValue(entorno, listas);
                        tipoDato tipoExp1 = b.getType(entorno, listas);
                        tipoOpcion2 = tipoExp1;

                        if (tipoExp1 != tipoDato.entero && tipoExp1 != tipoDato.decimall)
                        {
                            listas.errores.AddLast(new NodoError(linea, columna,
                                NodoError.tipoError.Semantico, "No se puede realizar la operacion /= por no son tipo numerico las expresiones"));
                            return tipoDato.errorSemantico;
                        }

                        if (valorExp1 is Simbolo)
                        {
                            arrobaid = true;
                            s = (Simbolo)valorExp1;
                            valorExp1 = s.valor;
                        }

                        if (tipoExp1 == tipoDato.entero)
                        {
                            valorExp1 = Convert.ToInt32(valorExp1) / Convert.ToInt32(valorExp2);
                        }
                        else if (tipoExp1 == tipoDato.decimall)
                        {
                            valorExp1 = Convert.ToDouble(valorExp1) / Convert.ToDouble(valorExp2);
                        }

                        SetearvaloresAccesos setear = new SetearvaloresAccesos(b, valorExp1, this.linea, this.columna, tipoExp1);
                        setear.ejecutar(entorno, listas);
                        return valorExp1;
                    }
                }
            }
            catch (Exception e)
            {
            }
            listas.errores.AddLast(new NodoError(linea, columna,
                NodoError.tipoError.Semantico, "No se puede realizar la operacion /= "));
            return tipoDato.errorSemantico;
        }
    }
}
