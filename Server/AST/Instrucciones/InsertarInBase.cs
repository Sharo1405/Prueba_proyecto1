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
    class InsertarInBase : Instruccion
    {
        public String idTabla { get; set; }
        public Expresion expresion { get; set; } //parentesis o sea lista expresiones
        public LinkedList<String> listaIds = new LinkedList<string>(); //lista de campo
        public int linea { get; set; }
        public int col { get; set; }


        public InsertarInBase(String idTabla, Expresion expresion,
            int linea, int col)
        {
            this.idTabla = idTabla;
            this.expresion = expresion;
            this.linea = linea;
            this.col = col;
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
                            int contador = 0;
                            int cantidadValores = 0;
                            object exp = expresion.getValue(entorno, listas, management);
                            tipoDato tipoexp = expresion.getType(entorno, listas, management);
                            LinkedList<Comas> listaExpresiones = new LinkedList<Comas>();
                            if (exp is LinkedList<Comas>)
                            {
                                cantidadValores = ((LinkedList<Comas>)exp).Count;
                                listaExpresiones = (LinkedList<Comas>)exp;
                            }
                            else
                            {
                                cantidadValores = 1;
                            }


                            if (encontrado2.columnasTabla.Count != cantidadValores)
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                        "La cantidad de valores a insertar no coincide con la cantidad de columnas de la tabla: "
                                        + idTabla));
                                return tipoDato.errorSemantico;
                            }

                            //validar que las llaves primarias no esten repetidas y no insertar por gusto
                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                            {
                                Columna iterador = (Columna)kvp.Value;
                                if (iterador.tipo == tipoDato.counter)
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                        "No se puede realizar el insert NORMAL ya que contiene counters la Tabla: " + idTabla + "en la columna: "
                                        + iterador.idColumna));
                                    return tipoDato.errorSemantico;
                                }
                                else
                                {
                                    if (cantidadValores > 1)
                                    {
                                        object valorComa = ((Comas)listaExpresiones.ElementAt(contador)).getValue(entorno, listas, management);
                                        tipoDato tipoComa = ((Comas)listaExpresiones.ElementAt(contador)).getType(entorno, listas, management);

                                        if (((Comas)listaExpresiones.ElementAt(contador)).expresion1 is Corchetes)
                                        {
                                            tipoComa = tipoDato.list;
                                        }
                                        else if (((Comas)listaExpresiones.ElementAt(contador)).expresion1 is Llaves)
                                        {
                                            tipoComa = tipoDato.set;
                                        }

                                        if (valorComa is Simbolo)
                                        {
                                            valorComa = ((Simbolo)valorComa).valor;
                                        }

                                        if (iterador.tipo == tipoComa)
                                        {
                                            if (iterador.primaryKey == true)
                                            {
                                                Boolean yaexiste = existe_llave_primaria(valorComa, iterador.valorColumna, tipoComa);
                                                if (yaexiste)
                                                {
                                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "La llave primaria a ingresar ya existe: " + Convert.ToString(valorComa) +
                                                        " en la tabla: " + idTabla));
                                                    return tipoDato.errorSemantico;
                                                }
                                            }
                                            
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "Los tipos de las columnas no son iguales: " + Convert.ToString(tipoComa) +
                                                        " en la tabla: " + idTabla));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    else
                                    {
                                        if (((Comas)listaExpresiones.ElementAt(contador)).expresion1 is Corchetes)
                                        {
                                            tipoexp = tipoDato.list;
                                        }
                                        else if (((Comas)listaExpresiones.ElementAt(contador)).expresion1 is Llaves)
                                        {
                                            tipoexp = tipoDato.set;
                                        }

                                        if (exp is Simbolo)
                                        {
                                            exp = ((Simbolo)exp).valor;
                                        }

                                        if (iterador.tipo == tipoexp)
                                        {
                                            if (iterador.primaryKey == true)
                                            {
                                                Boolean yaexiste = existe_llave_primaria(exp, iterador.valorColumna, tipoexp);
                                                if (yaexiste)
                                                {
                                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "La llave primaria a ingresar ya existe: " + Convert.ToString(exp) +
                                                        " en la tabla: " + idTabla));
                                                    return tipoDato.errorSemantico;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "Los tipos de las columnas no son iguales: " + Convert.ToString(tipoexp) +
                                                        " en la tabla: " + idTabla));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                }
                                contador++;
                            }

                            contador = 0;
                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                            {
                                Columna iterador = (Columna)kvp.Value;
                                if (iterador.tipo == tipoDato.counter)
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                        "No se puede realizar el insert NORMAL ya que contiene counters la Tabla: " + idTabla + "en la columna: "
                                        + iterador.idColumna));
                                    return tipoDato.errorSemantico;
                                }
                                else
                                {
                                    if (cantidadValores > 1)
                                    {
                                        List<object> lista = new List<object>();
                                        Lista listaGuardar = new Lista();
                                        object valorComa = ((Comas)listaExpresiones.ElementAt(contador)).getValue(entorno, listas, management);
                                        tipoDato tipoComa = ((Comas)listaExpresiones.ElementAt(contador)).getType(entorno, listas, management);

                                        if (((Comas)listaExpresiones.ElementAt(contador)).expresion1 is Corchetes)
                                        {
                                            if (valorComa is List<object>)
                                            {
                                                lista = (List<object>)valorComa;
                                                listaGuardar = new Lista(iterador.idColumna.ToLower(), lista, tipoDato.list, tipoComa, linea, col);
                                            }
                                            else
                                            {
                                                lista.Add(valorComa);
                                                listaGuardar = new Lista(iterador.idColumna.ToLower(), lista, tipoDato.list, tipoComa, linea, col);
                                            }
                                            tipoComa = tipoDato.list;
                                            valorComa = listaGuardar;
                                        }
                                        else if (((Comas)listaExpresiones.ElementAt(contador)).expresion1 is Llaves)
                                        {
                                            if (valorComa is List<object>)
                                            {
                                                lista = (List<object>)valorComa;
                                                listaGuardar = new Lista(iterador.idColumna.ToLower(), lista, tipoDato.set, tipoComa, linea, col);
                                            }
                                            else
                                            {
                                                lista.Add(valorComa);
                                                listaGuardar = new Lista(iterador.idColumna.ToLower(), lista, tipoDato.set, tipoComa, linea, col);
                                            }
                                            tipoComa = tipoDato.set;
                                            valorComa = listaGuardar;
                                        }

                                        if (valorComa is Simbolo)
                                        {
                                            valorComa = ((Simbolo)valorComa).valor;
                                        }

                                        if (iterador.tipo == tipoComa)
                                        {
                                            if (iterador.primaryKey == true)
                                            {
                                                Boolean yaexiste = existe_llave_primaria(valorComa, iterador.valorColumna, tipoComa);
                                                if (yaexiste)
                                                {
                                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "La llave primaria a ingresar ya existe: " + Convert.ToString(valorComa) +
                                                        " en la tabla: " + idTabla));
                                                    return tipoDato.errorSemantico;
                                                }
                                                else
                                                {
                                                    iterador.valorColumna.AddLast(valorComa);
                                                }
                                            }
                                            else
                                            {
                                                iterador.valorColumna.AddLast(valorComa);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        List<object> lista = new List<object>();
                                        Lista listaGuardar = new Lista();
                                        if (expresion is Corchetes)
                                        {
                                            if (exp is List<object>)
                                            {
                                                lista = (List<object>)exp;
                                                listaGuardar = new Lista(iterador.idColumna.ToLower(), lista, tipoDato.list, tipoexp, linea, col);
                                            }
                                            else
                                            {
                                                lista.Add(exp);
                                                listaGuardar = new Lista(iterador.idColumna.ToLower(), lista, tipoDato.list, tipoexp, linea, col);
                                            }
                                            tipoexp = tipoDato.list;
                                            exp = listaGuardar;
                                        }
                                        else if (expresion is Llaves)
                                        {
                                            if (exp is List<object>)
                                            {
                                                lista = (List<object>)exp;
                                                listaGuardar = new Lista(iterador.idColumna.ToLower(), lista, tipoDato.set, tipoexp, linea, col);
                                            }
                                            else
                                            {
                                                lista.Add(exp);
                                                listaGuardar = new Lista(iterador.idColumna.ToLower(), lista, tipoDato.set, tipoexp, linea, col);
                                            }
                                            tipoexp = tipoDato.set;
                                            exp = listaGuardar;
                                        }

                                        if (exp is Simbolo)
                                        {
                                            exp = ((Simbolo)exp).valor;
                                        }

                                        if (iterador.tipo == tipoexp)
                                        {
                                            if (iterador.primaryKey == true)
                                            {
                                                Boolean yaexiste = existe_llave_primaria(exp, iterador.valorColumna, tipoexp);
                                                if (yaexiste)
                                                {
                                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "La llave primaria a ingresar ya existe: " + Convert.ToString(exp) +
                                                        " en la tabla: " + idTabla));
                                                    return tipoDato.errorSemantico;
                                                }
                                                else
                                                {
                                                    iterador.valorColumna.AddLast(exp);
                                                }
                                            }
                                            else
                                            {
                                                iterador.valorColumna.AddLast(exp);
                                            }

                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "Los tipos de las columnas no son iguales: " + Convert.ToString(tipoexp) +
                                                        " en la tabla: " + idTabla));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                }
                                contador++;
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                "La tabla con el id: " + idTabla + "No existe"));
                            return tipoDato.errorSemantico;
                        }

                    }
                    catch (ArgumentException e)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                "La tabla con el id: " + idTabla + "No existe"));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {

                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                "La base de datos EN USO no fue encontrada"));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                "No se puede realizar el INSERT NORMAL de la tabla" + idTabla));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }



        public Boolean existe_llave_primaria(object valor, LinkedList<object> listaValores, tipoDato tipoComa)
        {

            foreach (object guardado in listaValores)
            {
                if (guardado.Equals(valor))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
