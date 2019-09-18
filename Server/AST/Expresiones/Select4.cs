using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class Select4 : Expresion
    {

        public object OpcionSelect { get; set; } //puede ser E o un *
        public String idTabla { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Select4(object OpcionSelect, String idTabla,
            int linea, int columna)
        {
            this.OpcionSelect = OpcionSelect;
            this.idTabla = idTabla;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return tipoDato.cursor;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
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
                            String asterisco = "";
                            Tabla paraRetorno = new Tabla();
                            if (OpcionSelect is Expresion)
                            {
                                if (OpcionSelect is ListaExpresiones)
                                {
                                    ListaExpresiones lexp = (ListaExpresiones)OpcionSelect;
                                    LinkedList<String> listacampos = new LinkedList<string>();
                                    LinkedList<Comas> lcomas = (LinkedList<Comas>) lexp.getValue(entorno, listas, management);

                                    foreach (Comas coma in lcomas)
                                    {

                                        if (coma.expresion1 is Identificador)
                                        {
                                            Identificador idd = (Identificador)coma.expresion1;
                                            //listacampos.AddLast(idd.id);

                                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                                            {
                                                Columna col = kvp.Value;
                                                if (col.idColumna.ToLower().Equals(idd.id.ToLower()))
                                                {
                                                    paraRetorno.columnasTabla.Add(idd.id.ToLower(), col);
                                                    break;
                                                }
                                            }

                                        }
                                        else if (coma.expresion1 is ListaPuntos)
                                        {
                                            ListaPuntos lpuntos = (ListaPuntos)coma.expresion1;
                                            LinkedList<Puntos> ExpSeparadasPuntos = lpuntos.ExpSeparadasPuntos;
                                            //posicion 0 es la columna y pos 1 es el id de elemento del usertype
                                            String idColumnaPunto = "";
                                            String idItemUserType = "";
                                            int count = 0;
                                            foreach (Puntos punto in ExpSeparadasPuntos)
                                            {
                                                if (count == 0)
                                                {
                                                    if (punto.expresion1 is Identificador)
                                                    {
                                                        idColumnaPunto = ((Identificador)punto.expresion1).id.ToLower();
                                                    }
                                                }
                                                else
                                                {
                                                    if (punto.expresion1 is Identificador)
                                                    {
                                                        idItemUserType = ((Identificador)punto.expresion1).id.ToLower();
                                                    }
                                                }
                                                count++;
                                            }


                                            //ya buscando en laas columnas
                                            Columna colNuevita = new Columna();
                                            List<object> listaItems = new List<object>();
                                            Columna colNuevita2 = new Columna();
                                            if (encontrado2.columnasTabla.TryGetValue(idColumnaPunto.ToLower(), out colNuevita))
                                            {
                                                CreateType extract = new CreateType();
                                                foreach (CreateType type in colNuevita.valorColumna)
                                                {
                                                    foreach (itemType t in type.itemTypee) {
                                                        if (t.id.ToLower().Equals(idItemUserType.ToLower())) {
                                                            //LinkedList<itemType> ite = new LinkedList<itemType>();
                                                            //ite.AddLast(t);
                                                            //CreateType n = new CreateType(idColumnaPunto.ToLower(),ite, type.linea, type.columna);
                                                            listaItems.Add(t);
                                                        }
                                                    }
                                                }
                                                colNuevita2.idColumna = colNuevita.idColumna + "." + idItemUserType;
                                                colNuevita2.idTipo = colNuevita.idTipo;
                                                colNuevita2.tipo = colNuevita.tipo;
                                                colNuevita2.valorColumna = listaItems;
                                                paraRetorno.columnasTabla.Add(colNuevita2.idColumna, colNuevita2);
                                            }
                                            else
                                            {
                                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                    "El id de la columna no existe en la tabla " + idTabla ));
                                                return tipoDato.errorSemantico;
                                            }

                                        }
                                    }

                                    //aqui deberia empezar el select
                                                                        
                                    paraRetorno.idTabla = encontrado2.idTabla;
                                    //paraRetorno.llavePrimaria = encontrado2.llavePrimaria;                                    

                                    return paraRetorno;
                                }
                                else if (OpcionSelect is ListaPuntos)
                                {
                                    ListaPuntos lpuntos = (ListaPuntos)OpcionSelect;
                                    LinkedList<Puntos> ExpSeparadasPuntos = lpuntos.ExpSeparadasPuntos;
                                    //posicion 0 es la columna y pos 1 es el id de elemento del usertype
                                    String idColumnaPunto = "";
                                    String idItemUserType = "";
                                    int count = 0;
                                    foreach (Puntos punto in ExpSeparadasPuntos)
                                    {
                                        if (count == 0)
                                        {
                                            if (punto.expresion1 is Identificador)
                                            {
                                                idColumnaPunto = ((Identificador)punto.expresion1).id.ToLower();
                                            }
                                        }
                                        else
                                        {
                                            if (punto.expresion1 is Identificador)
                                            {
                                                idItemUserType = ((Identificador)punto.expresion1).id.ToLower();
                                            }
                                        }
                                        count++;
                                    }


                                    //ya buscando en laas columnas
                                    Columna colNuevita = new Columna();
                                    List<object> listaItems = new List<object>();
                                    Columna colNuevita2 = new Columna();
                                    if (encontrado2.columnasTabla.TryGetValue(idColumnaPunto.ToLower(), out colNuevita))
                                    {
                                        CreateType extract = new CreateType();
                                        foreach (CreateType type in colNuevita.valorColumna)
                                        {
                                            foreach (itemType t in type.itemTypee)
                                            {
                                                if (t.id.ToLower().Equals(idItemUserType.ToLower()))
                                                {
                                                    listaItems.Add(t);
                                                }
                                            }
                                        }
                                        colNuevita2.idColumna = colNuevita.idColumna + "." + idItemUserType;
                                        colNuevita2.idTipo = colNuevita.idTipo;
                                        colNuevita2.tipo = colNuevita.tipo;
                                        colNuevita2.valorColumna = listaItems;
                                        paraRetorno.columnasTabla.Add(colNuevita2.idColumna, colNuevita2);
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                            "El id de la columna no existe en la tabla " + idTabla));
                                        return tipoDato.errorSemantico;
                                    }

                                    return paraRetorno;
                                }
                                else if(OpcionSelect is Identificador)
                                {
                                    Identificador ii = (Identificador)OpcionSelect;
                                    foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                                    {
                                        Columna col = kvp.Value;
                                        if (col.idColumna.ToLower().Equals(ii.id.ToLower()))
                                        {
                                            paraRetorno.columnasTabla.Add(col.idColumna.ToLower(), col);
                                            break;
                                        }
                                    }
                                    return paraRetorno;
                                }
                                return null;
                            }
                            else
                            {
                                asterisco = "*";
                                return encontrado2;
                            }

                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "La tabla con el id: " + idTabla + " No existe"));
                            return tipoDato.errorSemantico;
                        }
                    }
                    catch (ArgumentException e)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "NO se puede realizar el Select4 en la tabla: " + idTabla));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "La base de datos EN USO no fue encontrada"));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede realizar el Select4 de la tabla" + idTabla));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
