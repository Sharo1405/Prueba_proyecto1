using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones.TipoDato;
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
                                            listacampos.AddLast(idd.id);
                                        }

                                    }

                                    //aqui deberia empezar el select
                                    Tabla paraRetorno = new Tabla();
                                    int contador = 0;
                                    paraRetorno.idTabla = encontrado2.idTabla;
                                    paraRetorno.llavePrimaria = encontrado2.llavePrimaria;
                                    foreach (String campo in listacampos) {
                                        foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                                        {
                                            Columna col = kvp.Value;
                                            if (col.idColumna.ToLower().Equals(campo.ToLower()))
                                            {
                                                paraRetorno.columnasTabla.Add(campo.ToLower(), col);
                                            }
                                        }
                                    }

                                    return paraRetorno;
                                }
                                else if (OpcionSelect is ListaPuntos)
                                {

                                }
                                else
                                {

                                }


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
