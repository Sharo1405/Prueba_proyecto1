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
    class NodoCrearTabla : Instruccion
    {
        public Boolean ifnotexists = false;
        public String idTabla { get; set; }
        public LinkedList<NodoColumnas> Columnas = new LinkedList<NodoColumnas>();
        public int linea { get; set; }
        public int col { get; set; }

        public NodoCrearTabla(String idTabla,
            LinkedList<NodoColumnas> Columnas, 
            int linea, int col)
        {
            this.idTabla = idTabla;
            this.Columnas = Columnas;
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
                    //primero crear tabla 
                    //hacer un for paa crear cada columna
                    //hacer cada nodo para cada columna
                    Tabla nuevaTabla = new Tabla();
                    LinkedList<String> llavePrimaria = new LinkedList<string>();
                    foreach (NodoColumnas nodoCol in Columnas)
                    {
                        Columna colParaGuardar = new Columna();
                        if (nodoCol.listaIdColumnasPK.Count == 0)
                        {
                            try
                            {
                                if (nodoCol.tipo.tipoValor != null)
                                {
                                    colParaGuardar = new Columna(nodoCol.idColumna, nodoCol.tipo.tipo, nodoCol.tipo.tipoValor.tipo, null, nodoCol.primaryKey);
                                }
                                else
                                {
                                    colParaGuardar = new Columna(nodoCol.idColumna, nodoCol.tipo.tipo, tipoDato.errorSemantico, null, nodoCol.primaryKey);
                                }

                                if (nodoCol.primaryKey == true)
                                {
                                    llavePrimaria.AddLast(nodoCol.idColumna);
                                }

                                nuevaTabla.columnasTabla.Add(nodoCol.idColumna, colParaGuardar);
                            }
                            catch (ArgumentException e)
                            {
                                if (ifnotexists is false)
                                {
                                    listas.impresiones.AddLast("WARNNING!! ESA COLUMNA YA EXISTE: " + nodoCol.idColumna);
                                }
                                return tipoDato.errorSemantico;
                            }
                        }                        
                    }

                    foreach (NodoColumnas nodoCol in Columnas)
                    {
                        if (nodoCol.listaIdColumnasPK.Count > 0)
                        {
                            if (llavePrimaria.Count == 0) {
                                llavePrimaria = nodoCol.listaIdColumnasPK;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                    "Las llaves primarias estan identificadas 2 veces y solo se espera o llaves compuestas o llave unitaria"));
                                return tipoDato.errorSemantico;
                            }
                        }
                    }

                    nuevaTabla.idTabla = this.idTabla;
                    nuevaTabla.llavePrimaria = llavePrimaria;
                    basee.Tabla.Add(nuevaTabla.idTabla, nuevaTabla);
                    
                }
                else
                {
                    if (!ifnotexists) {
                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                    "La base de datos EN USO no fue encontrada"));
                    }
                    return tipoDato.errorSemantico;
                }
            }
            catch (ArgumentException e)
            {
                if (ifnotexists is false)
                {
                    listas.impresiones.AddLast("WARNNING!! ESA TABLA YA EXISTE: " + idTabla);
                }
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
