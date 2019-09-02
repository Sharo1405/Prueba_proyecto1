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
    class AccesoIdsConASignacion : Expresion
    {
        public String variable { get; set; }
        //public LinkedList<String> accesos { get; set; }
        public Expresion accesos { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public AccesoIdsConASignacion(String variable, Expresion accesos,
            Expresion valor, int linea, int columna)
        {
            this.variable = variable;
            this.accesos = accesos;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }        

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Simbolo sim = entorno.get(variable.ToLower(), entorno, Simbolo.Rol.VARIABLE);

                if (sim != null)
                {
                    if (accesos is ListaPuntos)
                    {
                        ListaPuntos a = (ListaPuntos)accesos;

                        object value = valor.getValue(entorno, listas);
                        tipoDato tipoValor = valor.getType(entorno, listas);

                        if (value is Simbolo)
                        {
                            value = ((Simbolo)value).valor;
                        }

                        Lista listaGuardar = new Lista();
                        if (valor is Corchetes)
                        {
                            if (value is List<object>)
                            {
                                List<object> lista = (List<object>)value;
                                listaGuardar = new Lista("", lista, tipoDato.list, tipoValor, linea, columna);
                            }
                            else
                            {
                                List<object> lista = new List<object>();
                                lista.Add(value);
                                listaGuardar = new Lista("", lista, tipoDato.list, tipoValor, linea, columna);
                            }

                            SetearvaloresAccesos sett = new SetearvaloresAccesos(variable, a, listaGuardar, this.linea, this.columna, tipoDato.list);
                            sett.ejecutar(entorno, listas);
                        }
                        else if (valor is Llaves) //set
                        {
                            if (value is List<object>)
                            {
                                List<object> lista = (List<object>)value;
                                listaGuardar = new Lista("", lista, tipoDato.set, tipoValor, linea, columna);
                            }
                            else
                            {
                                List<object> lista = new List<object>();
                                lista.Add(value);
                                listaGuardar = new Lista("", lista, tipoDato.set, tipoValor, linea, columna);
                            }
                            SetearvaloresAccesos sett = new SetearvaloresAccesos(variable, a, listaGuardar, this.linea, this.columna, tipoDato.set);
                            sett.ejecutar(entorno, listas);
                        }
                        else
                        {
                            SetearvaloresAccesos sett = new SetearvaloresAccesos(variable, a, value, this.linea, this.columna, tipoValor);
                            sett.ejecutar(entorno, listas);
                        }
                    }
                    else
                    {
                        ArrobaId a = new ArrobaId(variable, this.linea, this.columna);
                        Expresion exp = (Expresion)a;
                        ListaPuntos b = new ListaPuntos(exp, accesos, this.linea, this.columna);

                        object value = valor.getValue(entorno, listas);
                        tipoDato tipoValor = valor.getType(entorno, listas);


                        if (value is Simbolo)
                        {
                            value = ((Simbolo)value).valor;
                        }

                        Lista listaGuardar = new Lista();
                        if (valor is Corchetes)
                        {
                            if (value is List<object>)
                            {
                                List<object> lista = (List<object>)value;
                                listaGuardar = new Lista("", lista, tipoDato.list, tipoValor, linea, columna);
                            }
                            else
                            {
                                List<object> lista = new List<object>();
                                lista.Add(value);
                                listaGuardar = new Lista("", lista, tipoDato.list, tipoValor, linea, columna);
                            }

                            SetearvaloresAccesos sett = new SetearvaloresAccesos(b, listaGuardar, this.linea, this.columna, tipoValor);
                            sett.ejecutar(entorno, listas);
                        }
                        else if (valor is Llaves) //set
                        {
                            if (value is List<object>)
                            {
                                List<object> lista = (List<object>)value;
                                listaGuardar = new Lista("", lista, tipoDato.set, tipoValor, linea, columna);
                            }
                            else
                            {
                                List<object> lista = new List<object>();
                                lista.Add(value);
                                listaGuardar = new Lista("", lista, tipoDato.set, tipoValor, linea, columna);
                            }
                            SetearvaloresAccesos sett = new SetearvaloresAccesos(b, listaGuardar, this.linea, this.columna, tipoValor);
                            sett.ejecutar(entorno, listas);
                        }
                        else
                        {
                            SetearvaloresAccesos sett = new SetearvaloresAccesos(b, value, this.linea, this.columna, tipoValor);
                            sett.ejecutar(entorno, listas);
                        }
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                        "La variable \"" + variable + "\" NO EXISTE no se puede asignar valor"));
                    return tipoDato.errorSemantico;
                }

            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                "Asignacion no valida"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.id;
        }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipoDato.ok;
        }
    }
}
