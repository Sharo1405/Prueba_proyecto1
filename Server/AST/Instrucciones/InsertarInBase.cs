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
                                listaExpresiones.AddLast(new Comas(this.linea, this.col, expresion));
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
                                    if (cantidadValores >= 1)
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
                                                if (tipoComa == tipoDato.id)
                                                {
                                                    if (iterador.tipo == tipoComa)
                                                    {
                                                        if (iterador.idTipo != ((CreateType)valorComa).idType)
                                                        {
                                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                                "Los tipos de las columnas no son iguales: " + Convert.ToString(((CreateType)exp).idType) +
                                                                " en la tabla: " + idTabla));
                                                            return tipoDato.errorSemantico;
                                                        }
                                                    }
                                                }
                                                else if (tipoComa == tipoDato.list || tipoComa == tipoDato.set)
                                                {
                                                    if (iterador.tipoValor != ((Lista)valorComa).tipoValor)
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                                "Los tipos de los valores del set/list" +
                                                                "no son iguales: " + Convert.ToString(((Lista)exp).tipoValor) +
                                                                " en la tabla: " + idTabla));
                                                        return tipoDato.errorSemantico;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (tipoComa == tipoDato.id)
                                                {
                                                    if (iterador.tipo == tipoComa)
                                                    {
                                                        if (iterador.idTipo != ((CreateType)valorComa).idType)
                                                        {                                                                                                          
                                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                                "Los tipos de las columnas no son iguales: " + Convert.ToString(((CreateType)exp).idType) +
                                                                " en la tabla: " + idTabla));
                                                            return tipoDato.errorSemantico;
                                                        }
                                                    }
                                                }
                                                else if (tipoComa == tipoDato.list || tipoComa == tipoDato.set)
                                                {
                                                    if (iterador.tipoValor != ((Lista)valorComa).tipoValor)
                                                    {                                                    
                                                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                                "Los tipos de los valores del set/list" +
                                                                "no son iguales: " + Convert.ToString(((Lista)exp).tipoValor) +
                                                                " en la tabla: " + idTabla));
                                                        return tipoDato.errorSemantico;
                                                    }
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
                                    if (cantidadValores >= 1)
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
                                                if (tipoComa == tipoDato.id)
                                                {
                                                    if (iterador.tipo == tipoComa)
                                                    {
                                                        if (iterador.idTipo == ((CreateType)valorComa).idType)
                                                        {
                                                            iterador.valorColumna.AddLast(valorComa);
                                                        }
                                                        else
                                                        {
                                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                                "Los tipos de las columnas no son iguales: " + Convert.ToString(((CreateType)valorComa).idType) +
                                                                " en la tabla: " + idTabla));
                                                            return tipoDato.errorSemantico;
                                                        }
                                                    }
                                                }
                                                else if (tipoComa == tipoDato.list || tipoComa == tipoDato.set)
                                                {
                                                    if (iterador.tipoValor == ((Lista)valorComa).tipoValor)
                                                    {
                                                        iterador.valorColumna.AddLast(valorComa);
                                                    }
                                                    else
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                                "Los tipos de los valores del set/list" +
                                                                "no son iguales: " + Convert.ToString(((Lista)valorComa).tipoValor) +
                                                                " en la tabla: " + idTabla));
                                                        return tipoDato.errorSemantico;
                                                    }
                                                }
                                                else
                                                {
                                                    iterador.valorColumna.AddLast(valorComa);
                                                }
                                            }
                                            else
                                            {
                                                if (tipoComa == tipoDato.id)
                                                {
                                                    if (iterador.tipo == tipoComa)
                                                    {
                                                        if (iterador.idTipo == ((CreateType)valorComa).idType)
                                                        {
                                                            iterador.valorColumna.AddLast(valorComa);
                                                        }
                                                        else
                                                        {
                                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                                "Los tipos de las columnas no son iguales: " + Convert.ToString(((CreateType)valorComa).idType) +
                                                                " en la tabla: " + idTabla));
                                                            return tipoDato.errorSemantico;
                                                        }
                                                    }
                                                }
                                                else if (tipoComa == tipoDato.list || tipoComa == tipoDato.set)
                                                {
                                                    if (iterador.tipoValor == ((Lista)valorComa).tipoValor)
                                                    {
                                                        iterador.valorColumna.AddLast(valorComa);
                                                    }
                                                    else
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                                "Los tipos de los valores del set/list" +
                                                                "no son iguales: " + Convert.ToString(((Lista)valorComa).tipoValor) +
                                                                " en la tabla: " + idTabla));
                                                        return tipoDato.errorSemantico;
                                                    }
                                                }
                                                else
                                                {
                                                    iterador.valorColumna.AddLast(valorComa);
                                                }
                                            }
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
