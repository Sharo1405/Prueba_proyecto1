using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.Aritmeticas;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class ArrobaIDPuntoE : Expresion
    {
        public String id { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        public ArrobaIDPuntoE(String id, Expresion valor,
            int linea, int columna)
        {
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public ArrobaIDPuntoE()
        {
        }        

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipoDato.ok;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Simbolo sim = entorno.get(id.ToLower(), entorno, Simbolo.Rol.VARIABLE);

                if (sim != null)
                {
                    Simbolo si = entorno.get(id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                    if (sim != null)
                    {
                        if (valor is ListaPuntos)
                        {
                            ListaPuntos a = (ListaPuntos)valor;
                            ListaPuntos sett = new ListaPuntos(id, a.ExpSeparadasPuntos, this.linea, this.columna);
                            sett.getValue(entorno, listas);
                        }
                        else if (valor is MasIgual)
                        {
                            MasIgual m = (MasIgual)valor;
                            MasIgual masigual = new MasIgual(this.linea, this.columna, m.expresion1, m.expresion2, id);
                            masigual.getValue(entorno, listas);
                        }
                        else if (valor is MenosIgual)
                        {
                            MenosIgual m = (MenosIgual)valor;
                            MenosIgual masigual = new MenosIgual(this.linea, this.columna, m.expresion1, m.expresion2, id);
                            masigual.getValue(entorno, listas);
                        }
                        else if (valor is PorIgual)
                        {
                            PorIgual m = (PorIgual)valor;
                            PorIgual masigual = new PorIgual(this.linea, this.columna, m.expresion1, m.expresion2, id);
                            masigual.getValue(entorno, listas);
                        }
                        else if (valor is DividirIgual)
                        {
                            DividirIgual m = (DividirIgual)valor;
                            DividirIgual masigual = new DividirIgual(this.linea, this.columna, m.expresion1, m.expresion2, id);
                            masigual.getValue(entorno, listas);
                        }
                        else if (valor is Incremento)
                        {
                            Incremento i = (Incremento)valor;
                            Incremento ii = new Incremento(id, i.idExp, this.linea, this.columna);
                            ii.getValue(entorno, listas);
                        }
                        else if (valor is Decremento)
                        {
                            Decremento i = (Decremento)valor;
                            Decremento ii = new Decremento(id, i.idExp, this.linea, this.columna);
                            ii.getValue(entorno, listas);
                        }
                        else
                        {
                            ArrobaId a = new ArrobaId(id, this.linea, this.columna);
                            Expresion exp = (Expresion)a;
                            ListaPuntos b = new ListaPuntos(exp, valor, this.linea, this.columna);

                            ListaPuntos sett = new ListaPuntos(id, b.ExpSeparadasPuntos, this.linea, this.columna);
                            sett.getValue(entorno, listas);

                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "La variable \"" + id + "\" NO EXISTE no se puede asignar valor"));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                        "La variable \"" + id + "\" NO EXISTE no se puede asignar valor"));
                    return tipoDato.errorSemantico;
                }

            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                "Asignacion no valida"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }        
    }
}
