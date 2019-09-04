using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class AlterAddTable : Instruccion
    {

        public String idTabla { get; set; }
        public LinkedList<itemType> itemTypee = new LinkedList<itemType>();
        public int linea { get; set; }
        public int columna { get; set; }

        public AlterAddTable(String idTabla, LinkedList<itemType> itemTypee,
            int linea, int columna)
        {
            this.idTabla = idTabla;
            this.itemTypee = itemTypee;
            this.linea = linea;
            this.columna = columna;
        }


        public LinkedList<object> llenarListaValores(tipoDato tipo, int indice)
        {
            LinkedList<Object> listaRetorno = new LinkedList<object>();
            for (int i = 0; i < indice; i++)
            {
                if (tipo == tipoDato.entero)
                {
                    listaRetorno.AddLast(0);
                }
                else if (tipo == tipoDato.decimall)
                {
                    listaRetorno.AddLast(0.0);
                }
                else if (tipo == tipoDato.booleano)// || tipo == tipoDato.date || tipo == tipoDato.time)
                {
                    listaRetorno.AddLast(false);
                }
                else
                {
                    listaRetorno.AddLast(null);
                }
            }
            return listaRetorno;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Object inUse = management.getInUse();
                if (inUse != null)
                {
                    BaseDeDatos basee = (BaseDeDatos)inUse;
                    int countColumna = 0;
                    Tabla encontrado = new Tabla();
                    if (basee.Tabla.TryGetValue(idTabla.ToLower(), out encontrado))
                    {
                        Columna col = new Columna();
                        foreach (KeyValuePair<string, Columna> kvp in encontrado.columnasTabla)
                        {
                            countColumna = kvp.Value.valorColumna.Count();
                            break;
                        }

                        Columna nueva = new Columna();
                        foreach (itemType item in itemTypee) {
                            try
                            {
                                LinkedList<object> valColumnaGuardar = llenarListaValores(item.tipo.tipo, countColumna);
                                if (item.tipo.tipoValor != null)
                                {
                                    nueva = new Columna(item.id.ToLower(), item.tipo.tipo,
                                        item.tipo.tipoValor.tipo, valColumnaGuardar, false);
                                }
                                else
                                {
                                    nueva = new Columna(item.id.ToLower(), item.tipo.tipo,
                                        tipoDato.errorSemantico, valColumnaGuardar, false);
                                }

                                encontrado.columnasTabla.Add(item.id.ToLower(), nueva);
                            }
                            catch (ArgumentException e)
                            {
                                listas.impresiones.AddLast("WARNNING!! ESA COLUMNA YA EXISTE: " + item.id);
                            }
                        }
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
                                "No se puede realizar el ALTER add en la tabla" + idTabla));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
