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
    class InsertarInBaseEspecial : Instruccion
    {
        public String idTabla { get; set; }
        public Expresion expresion { get; set; } //parentesis o sea lista expresiones
        public LinkedList<String> listaIds = new LinkedList<string>(); //lista de campo
        public int linea { get; set; }
        public int col { get; set; }

        public InsertarInBaseEspecial(String idTabla, Expresion expresion
            , LinkedList<String> listaIds, int linea, int col)
        {
            this.idTabla = idTabla;
            this.expresion = expresion;
            this.listaIds = listaIds;
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
                            Boolean existeColumna = false;
                            object exp = expresion.getValue(entorno, listas, management);
                            tipoDato tipoexp = expresion.getType(entorno, listas, management);
                            LinkedList<Comas> listaExpresiones = new LinkedList<Comas>();
                            if (exp is LinkedList<Comas>)
                            {                                
                                listaExpresiones = (LinkedList<Comas>)exp; cantidadValores = ((LinkedList<Comas>)exp).Count;
                                listaExpresiones = (LinkedList<Comas>)exp;
                            }
                            else
                            {
                                cantidadValores = 1;
                                listaExpresiones.AddLast(new Comas(this.linea, this.col, expresion));
                            }

                            if (cantidadValores != listaIds.Count)
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                        "La cantidad de valores a insertar no coincide con la cantidad de valores a insertar en la tabla: "
                                        + idTabla));
                                return tipoDato.errorSemantico;
                            }
                            //validar que las llaves primarias no esten repetidas y no insertar por gusto                            

                            contador = 0;
                            foreach (String idCol in listaIds)
                            {                                
                                foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                                {
                                    Columna iterador = (Columna)kvp.Value;
                                    List<object> lista = new List<object>();
                                    Lista listaGuardar = new Lista();
                                    
                                    if (kvp.Value.idColumna.ToLower().Equals(idCol.ToLower()))
                                    {
                                        existeColumna = true;

                                        if (iterador.tipo == tipoDato.counter)
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                "No se puede realizar el insert ESPECIAL ya que es de tipo counter en la Tabla: " + idTabla + "en la columna: "
                                                + iterador.idColumna));
                                            return tipoDato.errorSemantico;
                                        }


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


                                        if (tipoComa == tipoDato.id)
                                        {
                                            if (iterador.tipo == tipoComa)
                                            {
                                                if (iterador.idTipo != ((CreateType)valorComa).idType)
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
                                            if (iterador.tipoValor != ((Lista)valorComa).tipoValor)
                                            {
                                                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "Los tipos de los valores del set/list" +
                                                        "no son iguales: " + Convert.ToString(((Lista)valorComa).tipoValor) +
                                                        " en la tabla: " + idTabla));
                                                return tipoDato.errorSemantico;
                                            }
                                        }
                                        else if (tipoComa != kvp.Value.tipo)
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "Los tipos de las columnas no son iguales: " + Convert.ToString(tipoComa) +
                                                        " en la tabla: " + idTabla));
                                            return tipoDato.errorSemantico;
                                        }
                                        else
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
                                        break;
                                    }                                    
                                }

                                if (existeColumna == false)// && iterador.tipo != tipoDato.counter)
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                        "La Columna con el id: " + idCol + "No existe, en la tabla: " + idTabla));
                                    return tipoDato.errorSemantico;
                                }/*else if (iterador.tipo == tipoDato.counter)
                                {
                                    contador--;
                                }*/
                                existeColumna = false;
                                contador++;
                            }





                            contador = 0;
                            foreach (String idCol in listaIds)
                            {
                                foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                                {
                                    Columna iterador = (Columna)kvp.Value;
                                   if (kvp.Value.idColumna.ToLower().Equals(idCol.ToLower()))
                                    {
                                        existeColumna = true;
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

                                        if (tipoComa != kvp.Value.tipo)
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                        "Los tipos de las columnas no son iguales: " + Convert.ToString(tipoComa) +
                                                        " en la tabla: " + idTabla));
                                            return tipoDato.errorSemantico;
                                        }
                                        else
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
                                                if (iterador.tipo == tipoComa)
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
                                                break;
                                            }
                                        }
                                    }
                                }                                    
                                
                                existeColumna = false;
                                contador++;
                            }

                            existeColumna = false;
                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                            {
                                existeColumna = false;
                                foreach (String idCol in listaIds)
                                {
                                    if (kvp.Value.idColumna.ToLower().Equals(idCol.ToLower()))
                                    {
                                        existeColumna = true;
                                        break;
                                    }
                                }

                                if (existeColumna == false )// kvp.Value.tipo != tipoDato.counter)
                                {
                                    kvp.Value.valorColumna.AddLast(llenadoautomatico(kvp.Value.tipo));
                                    if (kvp.Value.tipo == tipoDato.counter)
                                    {
                                        LinkedList<object> sts = (LinkedList<object>)kvp.Value.valorColumna;
                                        int x = sts.Count() - 1;

                                        if (kvp.Value.valorColumna.Count == 0)
                                        {
                                            kvp.Value.valorColumna.AddLast(0);
                                            kvp.Value.ultimovalorincrementable = 0;
                                        }
                                        else
                                        {
                                            object incrementable = sts.ElementAt(x);
                                            kvp.Value.valorColumna.AddLast(Convert.ToInt32(Convert.ToInt32(incrementable) + 1));
                                            kvp.Value.ultimovalorincrementable = Convert.ToInt32(Convert.ToInt32(incrementable) + 1);
                                        }                                        
                                    }
                                }
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
                                "NO se puede realizar el insert especial"));
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
                                "No se puede realizar el TRUNCATE de la tabla" + idTabla));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }


        public object llenadoautomatico(tipoDato tipoComa)
        {
            if (tipoComa == tipoDato.entero)
            {
                return 0;
            }
            else if (tipoComa == tipoDato.decimall)
            {
                return 0.0;
            }
            else if (tipoComa == tipoDato.booleano)
            {
                return false;
            }
            else //if (tipoComa == tipoDato.)
            {
                return null;
            }
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
