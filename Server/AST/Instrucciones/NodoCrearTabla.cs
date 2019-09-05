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
                    int contador = 0;
                    Boolean esCounter = false;
                    Boolean entroAlCatch = false;
                    foreach (NodoColumnas nodoCol in Columnas)
                    {
                        Columna colParaGuardar = new Columna();
                        if (nodoCol.listaIdColumnasPK.Count == 0)
                        {
                            try
                            {
                                if (nodoCol.tipo.tipoValor != null)
                                {
                                    colParaGuardar = new Columna(nodoCol.idColumna, nodoCol.tipo.tipo, nodoCol.tipo.tipoValor.tipo, nodoCol.primaryKey);
                                }
                                else if (nodoCol.tipo.tipo == tipoDato.id)
                                {
                                    colParaGuardar = new Columna(nodoCol.idColumna, nodoCol.tipo.tipo, nodoCol.tipo.id, tipoDato.errorSemantico, nodoCol.primaryKey);
                                }
                                else
                                {
                                    colParaGuardar = new Columna(nodoCol.idColumna, nodoCol.tipo.tipo, tipoDato.errorSemantico, nodoCol.primaryKey);
                                }

                                if (nodoCol.primaryKey == true)
                                {
                                    llavePrimaria.AddLast(nodoCol.idColumna);


                                    if (nodoCol.tipo.tipo == tipoDato.counter && esCounter == false)
                                    {
                                        esCounter = true;
                                    }

                                    if (esCounter == true && nodoCol.tipo.tipo != tipoDato.counter)
                                    {
                                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                            "Una de las 2 llaves primarias es de tipoCounter y otra deberia serlo, pero es: " +
                                            Convert.ToString(nodoCol.tipo.tipo) + "NO SE DECLARO LA TABLA: " + this.idTabla));
                                        return tipoDato.errorSemantico;
                                    }                                    
                                }

                                contador++;
                                nuevaTabla.columnasTabla.Add(nodoCol.idColumna, colParaGuardar);
                            }
                            catch (ArgumentException e)
                            {
                                if (ifnotexists is false)
                                {
                                    entroAlCatch = true;
                                    listas.impresiones.AddLast("WARNNING!! ESA COLUMNA YA EXISTE: " + nodoCol.idColumna);
                                }
                            }
                        }                        
                    }

                    if (entroAlCatch == true)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                      "No se guardo la tabla por tener columnas repeditdas"));
                        return tipoDato.errorSemantico;
                    }

                    foreach (NodoColumnas nodoCol in Columnas)
                    {
                        if (nodoCol.listaIdColumnasPK.Count > 0)
                        {
                            if (llavePrimaria.Count == 0)
                            {

                                esCounter = false;
                                contador = 0;

                                foreach (String idCol in nodoCol.listaIdColumnasPK)
                                {
                                    Columna encontrado = new Columna();
                                    if (nuevaTabla.columnasTabla.TryGetValue(idCol, out encontrado))
                                    {
                                        encontrado.primaryKey = true;
                                        if (encontrado.tipo == tipoDato.counter && esCounter == false)
                                        {
                                            esCounter = true;
                                        }

                                        if (esCounter == true && encontrado.tipo != tipoDato.counter)
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                                "Una de las llaves primarias es de tipoCounter y otra deberia serlo, pero es: " +
                                                Convert.ToString(nodoCol.tipo.tipo) + "NO SE DECLARO LA TABLA: " + this.idTabla));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                            "La columna con el id: " + idCol + " No fue encontrada en la tabla " + this.idTabla));
                                        return tipoDato.errorSemantico;
                                    }
                                }
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

                    foreach (NodoColumnas nodoCol in Columnas)
                    {
                        Columna encontrado = new Columna();
                        if (nuevaTabla.columnasTabla.TryGetValue(nodoCol.idColumna.ToLower(), out encontrado))
                        {
                            if (encontrado.tipo == tipoDato.counter && encontrado.primaryKey == false)
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                    "No puede existir un campo tipo counter que no sea llave primaria: tabla: " + nuevaTabla.idTabla
                                    + " en la base de datos: " + basee.idbase));
                                return tipoDato.errorSemantico;
                            }                            
                        }
                    }


                    if (llavePrimaria.Count == 0)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                    "Esta tabla " + idTabla + " NO tiene Llave Primaria"));
                        return tipoDato.errorSemantico;
                    }

                    nuevaTabla.idTabla = this.idTabla;
                    nuevaTabla.llavePrimaria = llavePrimaria;
                    basee.Tabla.Add(nuevaTabla.idTabla, nuevaTabla);
                    
                }
                else
                {

                    listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico,
                                "La base de datos EN USO no fue encontrada"));
                    
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
