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
                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                            {
                                Columna iterador = (Columna)kvp.Value;
                                foreach (String idCol in listaIds)
                                {
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
                                            }
                                        }
                                        break;
                                    }                                    
                                }

                                if (existeColumna == false && iterador.tipo != tipoDato.counter)
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                        "La Columna con el id: " + iterador.idColumna + "No existe, en la tabla: " + idTabla));
                                    return tipoDato.errorSemantico;
                                }else if (iterador.tipo == tipoDato.counter)
                                {
                                    contador--;
                                }
                                existeColumna = false;
                                contador++;
                            }





                            contador = 0;
                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                            {
                                Columna iterador = (Columna)kvp.Value;
                                if (iterador.tipo == tipoDato.counter)
                                {
                                    LinkedList<object> sts = (LinkedList<object>)iterador.valorColumna;
                                    int x = sts.Count() - 1;
                                    
                                    if (iterador.valorColumna.Count == 0)
                                    {
                                        iterador.valorColumna.AddLast(0);
                                    }
                                    else
                                    {
                                        object incrementable = sts.ElementAt(x);
                                        iterador.valorColumna.AddLast(Convert.ToInt32(Convert.ToInt32(incrementable) + 1));
                                    }
                                }
                                else
                                {
                                    foreach (String idCol in listaIds)
                                    {
                                        
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
                                                }
                                                else
                                                {
                                                    if (iterador.tipo == tipoComa) {
                                                        iterador.valorColumna.AddLast(valorComa);
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }                                    
                                }

                                if (existeColumna == false && iterador.tipo != tipoDato.counter)
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                        "La Columna con el id: " + iterador.idColumna + "No existe, en la tabla: " + idTabla));
                                    return tipoDato.errorSemantico;
                                }
                                else if (iterador.tipo == tipoDato.counter)
                                {
                                    contador--;
                                }
                                existeColumna = false;
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
