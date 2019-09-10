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
    class update4 : Instruccion
    {
        public String idTabla { get; set; }
        public LinkedList<NodoAST> asignaciones = new LinkedList<NodoAST>();
        public int linea { get; set; }
        public int columna { get; set; }


        public update4(String idTabla, LinkedList<NodoAST> asignaciones,
            int linea, int columna)
        {
            this.idTabla = idTabla;
            this.asignaciones = asignaciones;
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

                            foreach (NodoAST asignar in asignaciones)
                            {
                                if (asignar is idigualEupdate)
                                {
                                    idigualEupdate asi = (idigualEupdate)asignar;
                                    Columna coll = new Columna();
                                    if (encontrado2.columnasTabla.TryGetValue(asi.idCol.ToLower(), out coll))
                                    {
                                        int cantidadDatos = coll.valorColumna.Count;
                                        object valor = asi.igual.getValue(entorno, listas, management);
                                        tipoDato tipoValor = asi.igual.getType(entorno, listas, management);
                                        if (tipoValor == coll.tipo) {
                                            coll.valorColumna.Clear();

                                            for (int i = 0; i < cantidadDatos; i++)
                                            {
                                                coll.valorColumna.AddLast(valor);
                                            }
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                "El tipo del valor no es igual al tipo de la columna para actualizar, col:" + asi.idCol + 
                                                "tipo valor: " + Convert.ToString(tipoValor)));
                                            return tipoDato.errorSemantico;
                                        }
                                    }

                                }
                                else if (asignar is idpuntoEigualEupdate)
                                {

                                    idpuntoEigualEupdate asi = (idpuntoEigualEupdate)asignar;
                                    Columna coll = new Columna();
                                    if (encontrado2.columnasTabla.TryGetValue(asi.idCol.ToLower(), out coll))
                                    {
                                        object valor = asi.igual.getValue(entorno, listas, management);
                                        tipoDato tipoValor = asi.igual.getType(entorno, listas, management);
                                        if (asi.acceso is ListaPuntos)
                                        {
                                            SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, (ListaPuntos)asi.acceso, valor, tipoValor, this.linea, this.columna);
                                            svCol.ejecutar(entorno, listas, management);
                                        }
                                        else
                                        {
                                            if (asi.acceso is Identificador)
                                            {
                                                Identificador id = (Identificador)asi.acceso;
                                                ListaPuntos lpp = new ListaPuntos(id, this.linea, this.columna);
                                                SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, valor, tipoValor, this.linea, this.columna);
                                                svCol.ejecutar(entorno, listas, management);
                                            } 
                                            else if (asi.acceso is listaAccesoTabla)
                                            {

                                            }
                                            else
                                            {
                                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                    "El acceso a la columna en la tabla con el id: " + idTabla + " No es permitido"));
                                                return tipoDato.errorSemantico;
                                            }
                                        }
                                        
                                    }

                                }
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
                                "NO se puede realizar el Update4 en la tabla: " + idTabla));
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
                                "No se puede realizar el Update4 de la tabla" + idTabla));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
