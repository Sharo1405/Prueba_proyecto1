using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class EliminarDeLista : Instruccion
    {
        public String idColumna { get; set; }
        public Expresion posicionColumna { get; set; }
        public String idTabla { get; set; }
        public Expresion where { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public EliminarDeLista(String idColumna, Expresion posicionColumna,
            String idTabla, Expresion where)
        {
            this.idColumna = idColumna;
            this.posicionColumna = posicionColumna;
            this.idTabla = idTabla;
            this.where = where;
            this.linea = linea;
            this.columna = columna;
        }


        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Object inUse = management.getInUse();
                if (inUse != null)
                {
                    BaseDeDatos basee = (BaseDeDatos)inUse;
                    Tabla encontrado = new Tabla();
                    try
                    {
                        Tabla encontrado2 = new Tabla();
                        if (basee.Tabla.TryGetValue(idTabla.ToLower(), out encontrado2))
                        {
                            Entorno actual = new Entorno(entorno);
                            //declarar todas las variables 
                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                            {
                                LinkedList<String> ids = new LinkedList<string>();
                                ids.AddLast(kvp.Value.idColumna.ToLower());
                                Tipo tipoids = new Tipo();
                                tipoids = new Tipo(kvp.Value.tipo, this.linea, this.columna);
                                Declarcion decla = new Declarcion(tipoids, ids);
                                decla.ejecutar(actual, listas, management);
                            }


                            int contador = 0;
                            int cantValoresXcolumna = 1;
                            while (contador <= cantValoresXcolumna - 1)
                            {
                                foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                                {
                                    String idsvar = kvp.Value.idColumna.ToLower();
                                    object valor = kvp.Value.valorColumna.ElementAt(contador);
                                    Expresion exp = devueleExpresionTipo(kvp.Value.tipo, valor);
                                    Asignacion asigna = new Asignacion(idsvar, exp, this.linea, this.columna);
                                    asigna.getValue(actual, listas, management);
                                    //hasta aqui ya estan los valores en la tabla de simbolos
                                    //ahora necesito ver el where si devuelve true debo guardar 
                                    if (contador == 0)
                                    {
                                        cantValoresXcolumna = kvp.Value.valorColumna.Count;
                                    }
                                }


                                object resultado = where.getValue(actual, listas, management);
                                tipoDato tipoResultado = where.getType(actual, listas, management);

                                if (tipoResultado == tipoDato.booleano)
                                {
                                    if ((Boolean)resultado)//si entra el where es el exxito
                                    {

                                        Columna coll = new Columna();
                                        if (encontrado2.columnasTabla.TryGetValue(idColumna.ToLower(), out coll))
                                        {
                                            List<object> lista = new List<object>();
                                            Lista listaGuardar = new Lista();
                                            if (posicionColumna is Corchetes)
                                            {
                                                object index = posicionColumna.getValue(entorno, listas, management);
                                                tipoDato tipoIndex = posicionColumna.getType(entorno, listas, management);

                                                //verifico que el indice de la lista sea entero
                                                if (tipoIndex != tipoDato.entero)
                                                {
                                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                        "El idice de la lista al que se desea acceder no es de tipo entero en la columna" + coll.idColumna));
                                                    return tipoDato.errorSemantico;
                                                }

                                                //verifico que la columna no sea counter o PK
                                                if (coll.tipo == tipoDato.counter)
                                                {
                                                    listas.impresiones.AddLast("WARNNING!! NO SE PUEDE ACTUALIZAR UN DATO DE TIPO COUNTER: " + idTabla + " Linea/Columna "
                                                            + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                                                    return TipoExcepcion.excep.CounterTypeException;
                                                }
                                                else if (coll.primaryKey == true)
                                                {
                                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                        "La columa a actualizar es llave primaria: " + coll.idColumna));
                                                    return tipoDato.errorSemantico;
                                                }

                                                //ahora a eliminar la posicion de la lista en la columna

                                                if (tipoDato.list == coll.tipo)
                                                {
                                                    Lista lis = (Lista)coll.valorColumna.ElementAt(contador);
                                                    try
                                                    {
                                                        lis.listaValores.RemoveAt(Convert.ToInt32(index));
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        listas.impresiones.AddLast("WARNNING!! NO SE PUEDE ACCEDER A LA LISTA/SET INDICE FUERA DE RANGO " + " Linea/Columna "
                                                            + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                                                        return TipoExcepcion.excep.IndexOutException;
                                                    }
                                                }
                                                else
                                                {
                                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                        "El tipo de la columna " + idColumna + " No es una lista"));
                                                    return tipoDato.errorSemantico;
                                                }                                                
                                            }
                                            else
                                            {
                                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                        "Acceso a la lista no permitido en la columna" + coll.idColumna));
                                                return tipoDato.errorSemantico;
                                            }
                                        }

                                    }
                                }
                                contador++;
                            }

                        }
                        else
                        {
                            listas.impresiones.AddLast("WARNNING!! ESA TABLA NO EXISTE: " + idTabla + " Linea/Columna "
                                                                + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                            return TipoExcepcion.excep.TableDontExists;
                        }
                    }
                    catch (ArgumentException e)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "NO se puede realizar el Delete en lista en la tabla: " + idTabla));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    listas.impresiones.AddLast("WARNINGGGGGGGGGGGGGGGGGGGG!!!!!!!!!!!  La base de datos EN USO no fue encontrada " + " Linea/Columna "
                               + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                    return TipoExcepcion.excep.UseBDException;
                }

            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                         "No se puede eliminar en la posicion de la lista especificada"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }


        public Expresion devueleExpresionTipo(tipoDato tipo, object valor)
        {
            switch (tipo)
            {
                case tipoDato.booleano:
                    return new Booleano(valor, tipo, this.linea, this.columna);

                case tipoDato.cadena:
                    return new cadena(valor, tipo, this.linea, this.columna);

                case tipoDato.date:
                    return new Date(valor, tipo, this.linea, this.columna);

                case tipoDato.decimall:
                    return new Numero(valor, tipo, this.linea, this.columna);

                case tipoDato.entero:
                    return new Numero(valor, tipo, this.linea, this.columna);

                case tipoDato.id:
                    //return new Identificador(Convert.ToString(valor),this.linea, this.columna);
                    return new TypeBase(valor, this.linea, this.columna);

                case tipoDato.nulo:
                    return new Nulo(valor, tipo, this.linea, this.columna);

                case tipoDato.time:
                    return new Time(valor, tipo, this.linea, this.columna);

                case tipoDato.set:
                    return new SetBase(valor, this.linea, this.columna);

                case tipoDato.list:
                    return new ListsBase(valor, this.linea, this.columna);
            }

            return null;
        }
    }
}
