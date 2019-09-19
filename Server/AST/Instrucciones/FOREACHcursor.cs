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
    class FOREACHcursor : Instruccion
    {
        public LinkedList<Parametros> Parametross { get; set; }
        public String idCursor { get; set; }
        public StatementBlock sentencias { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public FOREACHcursor(LinkedList<Parametros> Parametross, String idCursor,
            StatementBlock sentencias, int linea, int columna)
        {
            this.Parametross = Parametross;
            this.idCursor = idCursor;
            this.sentencias = sentencias;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Simbolo variable = entorno.get(idCursor.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                if (variable != null)
                {
                    if (variable.valor is Tabla)
                    {
                        if (variable.abierto == true)
                        {
                            int indice = 0;
                            if (Parametross.Count == ((Tabla)variable.valor).columnasTabla.Count)
                            {
                                List<String> listaIDParametros = new List<string>();
                                Entorno actual = new Entorno(entorno);
                                foreach (Parametros idPara in Parametross)
                                {
                                    //necesito nombre columna y tipo columna
                                    LinkedList<String> ids = new LinkedList<string>();
                                    ids.AddLast(idPara.id.ToLower());
                                    listaIDParametros.Add(idPara.id.ToLower());
                                    Tipo tipoids = new Tipo();
                                    if (idPara.tipo.tipo == tipoDato.list || idPara.tipo.tipo == tipoDato.set)
                                    {
                                        tipoids = new Tipo(idPara.tipo.tipo, idPara.tipo.tipoValor, this.linea, this.columna);
                                    }
                                    else
                                    {
                                        tipoids = new Tipo(idPara.tipo.tipo, this.linea, this.columna);
                                    }
                                    Declarcion decla = new Declarcion(tipoids, ids);
                                    decla.ejecutar(actual, listas, management);
                                }

                                int vecesRepetir = 1;
                                while (indice < vecesRepetir) {
                                    int posLista = 0;
                                    foreach (KeyValuePair<string, Columna> kvp in ((Tabla)variable.valor).columnasTabla)
                                    {
                                        object valor = kvp.Value.valorColumna.ElementAt(indice);
                                        Expresion exp = devueleExpresionTipo(kvp.Value.tipo, valor);
                                        Asignacion asignarValor = new Asignacion(listaIDParametros.ElementAt(posLista), exp, this.linea, this.columna);
                                        asignarValor.getValue(actual, listas, management);
                                        if (indice == 0)
                                        {
                                            vecesRepetir = kvp.Value.valorColumna.Count;
                                        }
                                        posLista++;
                                    }

                                    object devuelto= sentencias.ejecutar(actual, listas, management);
                                    if (devuelto is Retorno)
                                    {
                                        return devuelto;
                                    }
                                    else if (devuelto is Continuee)
                                    {
                                        continue;
                                    }
                                    else if (devuelto is Breakk)
                                    {
                                        return devuelto;
                                    }                                    

                                    indice++;
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                    "El cursor no coincide con la lista del foreach, id: " + idCursor));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                    "El cursor no esta abierto no se puede utilizar, id: " + idCursor));
                            return tipoDato.errorSemantico;
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                            "No se puede Realizar el foreach porque la variable no es un cursor, id: " + idCursor));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                         "No se puede Realiza el cursor ya que no existe el id: " + idCursor));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
               listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                "No se puede Guardar el cursor" + idCursor));
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
