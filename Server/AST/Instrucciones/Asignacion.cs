using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class Asignacion : Expresion
    {
        public String id { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        int contador = 0;

        public Asignacion(String id, Expresion valor,
            int linea, int columna)
        {
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }


        public CreateType CreaNuevoType(CreateType existente, Entorno entorno, ErrorImpresion listas)
        {
            CreateType nuevo = new CreateType();
            nuevo.idType = existente.idType;
            nuevo.ifnotexists = existente.ifnotexists;
            nuevo.linea = existente.linea;
            nuevo.columna = existente.columna;

            foreach (itemType var in existente.itemTypee)
            {
                itemType i = new itemType();
                i.id = var.id;
                i.tipo = var.tipo;
                i.valor = new Object();
                if (var.tipo.tipo == tipoDato.id)
                {
                    Simbolo buscado2 = entorno.get(var.tipo.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                    if (buscado2 != null)
                    {
                        //arreglar solo declarar sin el clonar
                        //CreateType typeComoTipo =(CreateType) buscado2.valor;
                        i.valor = null;//typeComoTipo.Clone();
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                            "El tipo de esa variable del Type User no existe: Tipo:" + var.tipo.id.ToLower()));
                    }
                }
                else if (var.tipo.tipo == tipoDato.entero)
                {
                    var.valor = 0;
                }
                else if (var.tipo.tipo == tipoDato.decimall)
                {
                    var.valor = 0.0;
                }
                else if (var.tipo.tipo == tipoDato.booleano)
                {
                    var.valor = false;
                }
                else
                {
                    var.valor = null;
                }

                nuevo.itemTypee.AddLast(i);
            }
            return nuevo;
        }        


        LinkedList<Tipo> tiposSet = new LinkedList<Tipo>();
        List<Object> listaRetorno = new List<object>();
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

        public tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return tipoDato.ok;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Simbolo variable = entorno.get(id, entorno, Simbolo.Rol.VARIABLE);
                if (variable != null)
                {
                    object value = valor.getValue(entorno, listas, management);
                    tipoDato tipoValor = valor.getType(entorno, listas, management);
                    if (valor is Corchetes)
                    {
                        if (variable.tipo == tipoDato.list)
                        {
                            Lista listaGuardar = new Lista();
                            if (value is List<object>)
                            {
                                List<object> lista = (List<object>)value;
                                listaGuardar = new Lista(id.ToLower(), lista, tipoDato.list, tipoValor, linea, columna);
                                variable.valor = listaGuardar;
                            }
                            else
                            {
                                List<object> lista = new List<object>();
                                lista.Add(value);
                                listaGuardar = new Lista(id.ToLower(), lista, tipoDato.list, tipoValor, linea, columna);
                                variable.valor = listaGuardar;
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                   "La variable no es del mismo tipo que su asignacion, se esperaba: " +
                                   Convert.ToString(variable.tipo) + "Y se recibe un tipo: " + Convert.ToString(tipoDato.list)));
                            return tipoDato.errorSemantico;
                        }
                    }
                    else if (valor is Llaves) //set
                    {
                        if (variable.tipo == tipoDato.set)
                        {
                            Lista listaGuardar = new Lista();
                            if (value is List<object>)
                            {
                                List<object> lista = (List<object>)value;
                                listaGuardar = new Lista(id.ToLower(), lista, tipoDato.set, tipoValor, linea, columna);
                                variable.valor = listaGuardar;
                            }
                            else
                            {
                                List<object> lista = new List<object>();
                                lista.Add(value);
                                listaGuardar = new Lista(id.ToLower(), lista, tipoDato.set, tipoValor, linea, columna);
                                variable.valor = listaGuardar;
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                   "La variable no es del mismo tipo que su asignacion, se esperaba: " +
                                   Convert.ToString(variable.tipo) + "Y se recibe un tipo: " + Convert.ToString(tipoDato.set)));
                            return tipoDato.errorSemantico;
                        }
                    }
                    else if (valor is LLaveAsTypeUser) //user types
                    {
                        CreateType ty = (CreateType)value;
                        if (variable.idTipo == ty.idType)
                        {
                            variable.valor = ty;
                        }
                    }
                    else if (valor is Neww)
                    {
                        Neww clase = (Neww)valor;
                        Tipo tipo = clase.tipoNew;
                        if (tipo.tipo == tipoDato.list && variable.tipo == tipoDato.list)
                        {
                            Neww v = (Neww)value;
                            tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                            if (tt == tipoDato.ok)
                            {
                                Lista listaGuardar = new Lista(variable.id.ToLower(), new List<Object>(), tipoDato.list, tiposSet, linea, columna);
                                variable.valor = listaGuardar;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "Tipos no valido en el valor de la lista"));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else if (tipo.tipo == tipoDato.set && variable.tipo == tipoDato.set)
                        {
                            Neww v = (Neww)value;
                            tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                            if (tt == tipoDato.ok)
                            {
                                Lista listaGuardar = new Lista(variable.id.ToLower(), new List<Object>(), tipoDato.set, tiposSet, linea, columna);
                                variable.valor = listaGuardar;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "Tipos no valido en el valor del set"));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else if (tipo.tipo == tipoDato.id && variable.tipo == tipoDato.id)
                        {
                            Neww claseNeww = (Neww)value;
                            Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                            if (sim2 != null)
                            {
                                if (sim2.valor is CreateType)
                                {
                                    CreateType ss = (CreateType)sim2.valor;
                                    CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                    variable.valor = lista2;
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO EXISTE."));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "Los tipos no coinciden para la variable, No se puede hacer la instancia. Tipos en cuestion: " +
                                Convert.ToString(variable.tipo) + Convert.ToString(tipo.tipo)));
                        }
                    }
                    else if (valor is LlamadaProcedimiento)
                    {
                        Lista retornada = (Lista)value;
                        retornada.idLista = id.ToLower();
                        variable.valor = retornada;
                    }
                    else if (variable.tipo == tipoValor)
                    {
                        variable.valor = value;
                    }
                    else
                    {
                        if (variable.tipo == tipoDato.entero && tipoValor == tipoDato.decimall)
                        {
                            //se redondea el valor
                            double nuevo = Math.Floor(Double.Parse(Convert.ToString(value)));
                            Int32 ainsertar = Int32.Parse(Convert.ToString(nuevo));
                            variable.valor = ainsertar;
                        }
                        else if (variable.tipo == tipoDato.decimall && tipoValor == tipoDato.entero)
                        {
                            variable.valor = Double.Parse(Convert.ToString(value));
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "Los tipos no coinciden para la variable. Tipos en cuestion: " + Convert.ToString(variable.tipo) + Convert.ToString(tipoValor)));
                        }
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "La variable no existe para asignar valor: " + id));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                "La asignacion no es valida: " + id));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
