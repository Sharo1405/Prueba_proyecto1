using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using System.IO;
using static Server.AST.Expresiones.Operacion;
using Server.AST.Otras;
using Server.AST.Expresiones;
using Irony.Parsing;

namespace Server.AST.Instrucciones
{
    class EscribirCHISON : Instruccion
    {
        public int linea { get; set; }
        public int columna { get; set; }

        public EscribirCHISON(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                String chison = "$< \n";

                //escribir usuarios---------------------------------
                #region USUARIOS
                chison += "\"USERS\" = [ \n";
                int cantUsuario = management.usuarios.Count;
                int contador = 0;
                foreach (KeyValuePair<string, userPass> kvp in management.usuarios)
                {
                    if (contador < cantUsuario-1)
                    {
                        chison += "< \n";
                        chison += "\"NAME\" = \"" + kvp.Value.idUsuario + "\", \n";
                        chison += "\"PASSWORD\" = \"" + kvp.Value.pass + "\", \n";
                        chison += "\"PERMISSIONS\" = [";
                        int cantPermisos = kvp.Value.permisoBase.Count;
                        int contador2 = 0;
                        foreach (String dbpermiso in kvp.Value.permisoBase)
                        {
                            if (contador2 < cantPermisos - 1)
                            {
                                chison += "< \n\n \"NAME\"= \"" + dbpermiso + "\" \n\n >, \n\n";
                            }
                            else
                            {
                                chison += "< \n\n \"NAME\"= \"" + dbpermiso + "\" \n\n > \n\n";
                            }
                            contador2++;
                        }
                        chison += "] \n";
                        chison += ">, \n";
                    }
                    else
                    {
                        chison += "< \n";
                        chison += "\"NAME\" = \"" + kvp.Value.idUsuario + "\", \n";
                        chison += "\"PASSWORD\" = \"" + kvp.Value.pass + "\", \n";
                        chison += "\"PERMISSIONS\" = [ \n";
                        int cantPermisos = kvp.Value.permisoBase.Count;
                        int contador2 = 0;
                        foreach (String dbpermiso in kvp.Value.permisoBase)
                        {
                            if (contador2 < cantPermisos - 1)
                            {
                                chison += "< \n \"NAME\"= \"" + dbpermiso + "\" \n >, \n";
                            }
                            else
                            {
                                chison += "< \n \"NAME\"= \"" + dbpermiso + "\" \n > \n";
                            }
                            contador2++;
                        }
                        chison += "] \n";
                        chison += "> \n";
                    }
                    contador++;
                }
                chison += "], \n";
                #endregion
                //escribir usuarios-----------------------------------------


                //escribir bases de datos-----------------------------------
                chison += "\"DATABASES\" = [ \n";
                int cantBases = management.basesExistentes.Count;
                int contadorB = 0;
                foreach (KeyValuePair<string, BaseDeDatos> kvp in management.basesExistentes)
                {
                    if (contadorB < cantBases - 1)
                    {
                        chison += "< \n";
                        chison += "\"NAME\" = \"" + kvp.Value.idbase + "\", \n";
                        chison += "\"Data\" = [\n";
                        //OBJETOS----------------------------------------------------------------------------
                        #region OBJETOS
                        int contadorObjeto = 0;
                        int cantObjetos = kvp.Value.usertypes.Count;
                        foreach (KeyValuePair<string, Simbolo> sim in kvp.Value.usertypes)
                        {
                            CreateType objeto = (CreateType)sim.Value.valor;
                            if (contadorObjeto < cantObjetos - 1)
                            {                                
                                chison += "<\n";
                                chison += "\"CQL-TYPE\" = \"OBJECT\", \n";
                                chison += "\"NAME\" = \"" + objeto.idType + "\", \n";
                                chison += "\"ATTRS\" = [\n";
                                int contAtri = 0;
                                int cantAtri = objeto.itemTypee.Count;
                                foreach (itemType it in objeto.itemTypee)
                                {
                                    if (contAtri < cantAtri-1)
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + it.id + "\", \n";
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.id + "\" \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list || it.tipo.tipo ==tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.tipo + ">" + "\" \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\" \n";
                                        }
                                        chison += ">,\n";
                                    }
                                    else
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + it.id + "\", \n";
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.id + "\" \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list || it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.tipo + ">" + "\" \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\"\n";
                                        }
                                        chison += ">\n";
                                    }
                                    contAtri++;
                                }

                                chison += "] \n";
                                chison += ">,\n";
                            }
                            else
                            {
                                chison += "<\n";
                                chison += "\"CQL-TYPE\" = \"OBJECT\", \n";
                                chison += "\"NAME\" = \"" + objeto.idType + "\", \n";
                                chison += "\"ATTRS\" = [\n";
                                int contAtri = 0;
                                int cantAtri = objeto.itemTypee.Count;
                                foreach (itemType it in objeto.itemTypee)
                                {
                                    if (contAtri < cantAtri - 1)
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + it.id + "\", \n";
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.id + "\" \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list || it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.tipo + ">" + "\" \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\" \n";
                                        }
                                        chison += ">,\n";
                                    }
                                    else
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + it.id + "\", \n";
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.id + "\" \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list || it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.tipo + ">" + "\" \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\" \n";
                                        }
                                        chison += ">\n";
                                    }
                                    contAtri++;
                                }

                                chison += "] \n";
                                chison += ">\n";
                            }
                            contadorObjeto++;
                        }

                        #endregion
                        //OBJETOS----------------------------------------------------------------------------


                        //TABLAS-----------------------------------------------------------------------------
                        #region TABLAS
                        int contadorTablas = 0;
                        int cantTablas = kvp.Value.Tabla.Count;
                        if (cantTablas > 0)
                        {
                            chison += ", \n";
                        }

                        //COLUMNAS
                        foreach (KeyValuePair<string, Tabla> sim in kvp.Value.Tabla)
                        {
                            if (contadorTablas < cantTablas - 1)
                            {
                                chison += "<\n";
                                chison += "\"CQL-TYPE\" = \"TABLE\", \n";
                                chison += "\"NAME\" = \"" + sim.Value.idTabla + "\", \n";
                                chison += "\"COLUMNS\" = [\n";
                                int contadorColumnas = 0;
                                int cantColumnas = sim.Value.columnasTabla.Count;
                                foreach (KeyValuePair<string, Columna> col in sim.Value.columnasTabla)
                                {
                                    if (contadorColumnas < cantColumnas-1)
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + col.Value.idColumna + "\", \n";
                                        if (col.Value.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.idTipo + "\", \n";
                                        }
                                        else if (col.Value.tipo == tipoDato.list || col.Value.tipo == tipoDato.set)
                                        {
                                            if (col.Value.tipoValor == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.idTipo + ">" + "\", \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.tipoValor + ">" + "\", \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.tipo + "\", \n";
                                        }

                                        if (col.Value.primaryKey == true) {
                                            chison += "\"PK\" =  TRUE \n";
                                        }
                                        else
                                        {
                                            chison += "\"PK\" =  FALSE \n";
                                        }
                                        chison += ">,\n";
                                    }
                                    else
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + col.Value.idColumna + "\", \n";
                                        if (col.Value.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.idTipo + "\", \n";
                                        }
                                        else if (col.Value.tipo == tipoDato.list || col.Value.tipo == tipoDato.set)
                                        {
                                            if (col.Value.tipoValor == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.idTipo + ">" + "\", \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.tipoValor + ">" + "\", \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.tipo + "\", \n";
                                        }

                                        if (col.Value.primaryKey == true)
                                        {
                                            chison += "\"PK\" =  TRUE \n";
                                        }
                                        else
                                        {
                                            chison += "\"PK\" =  FALSE \n";
                                        }
                                        chison += ">\n";
                                    }
                                    contadorColumnas++;
                                }

                                chison += "], \n";

                                #region DATA
                                chison += ",";
                                chison += "\"DATA\" = [\n";

                                chison += EscribirDATAcolumna(sim.Value);

                                chison += "] \n";
                                #endregion

                                

                                chison += ">,\n";
                            }
                            else
                            {
                                chison += "<\n";
                                chison += "\"CQL-TYPE\" = \"TABLE\", \n";
                                chison += "\"NAME\" = \"" + sim.Value.idTabla + "\", \n";
                                chison += "\"COLUMNS\" = [\n";
                                int contadorColumnas = 0;
                                int cantColumnas = sim.Value.columnasTabla.Count;
                                foreach (KeyValuePair<string, Columna> col in sim.Value.columnasTabla)
                                {
                                    if (contadorColumnas < cantColumnas - 1)
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + col.Value.idColumna + "\", \n";
                                        if (col.Value.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.idTipo + "\", \n";
                                        }
                                        else if (col.Value.tipo == tipoDato.list || col.Value.tipo == tipoDato.set)
                                        {
                                            if (col.Value.tipoValor == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.idTipo + ">" + "\", \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.tipoValor + ">" + "\", \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.tipo + "\", \n";
                                        }

                                        if (col.Value.primaryKey == true)
                                        {
                                            chison += "\"PK\" =  TRUE \n";
                                        }
                                        else
                                        {
                                            chison += "\"PK\" =  FALSE \n";
                                        }
                                        chison += ">,\n";
                                    }
                                    else
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + col.Value.idColumna + "\", \n";
                                        if (col.Value.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.idTipo + "\", \n";
                                        }
                                        else if (col.Value.tipo == tipoDato.list || col.Value.tipo == tipoDato.set)
                                        {
                                            if (col.Value.tipoValor == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.idTipo + ">" + "\", \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.tipoValor + ">" + "\", \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.tipo + "\", \n";
                                        }

                                        if (col.Value.primaryKey == true)
                                        {
                                            chison += "\"PK\" =  TRUE \n";
                                        }
                                        else
                                        {
                                            chison += "\"PK\" =  FALSE \n";
                                        }
                                        chison += ">\n";
                                    }
                                    contadorColumnas++;
                                }

                                chison += "], \n";

                                #region DATA
                                chison += ",";
                                chison += "\"DATA\" = [\n";

                                chison += EscribirDATAcolumna(sim.Value);

                                chison += "] \n";
                                #endregion

                                chison += ">\n";
                            }
                            contadorTablas++;
                        }

                        #endregion
                        //TABLAS-----------------------------------------------------------------------------


                        #region PROCEDIMIENTO

                        int cantColumnasDATA = 0;
                        foreach (KeyValuePair<string, Tabla> sim in kvp.Value.Tabla) {

                            foreach (KeyValuePair<string, Columna> col in sim.Value.columnasTabla)
                            {
                                cantColumnasDATA = col.Value.valorColumna.Count;
                                break;
                            }
                            break;
                        }
                        if (kvp.Value.procedures.Count > 0 && cantColumnasDATA >0)
                        {
                            chison += ",\n" + EscribirProcedimiento(kvp.Value.procedures);
                        }
                        else
                        {
                            chison += EscribirProcedimiento(kvp.Value.procedures);
                        }

                        #endregion


                        chison += "] \n";
                        chison += ">, \n";
                    }
                    else
                    {

                        chison += "< \n";
                        chison += "\"NAME\" = \"" + kvp.Value.idbase + "\", \n";
                        chison += "\"Data\" = [\n";
                        //OBJETOS----------------------------------------------------------------------------
                        #region OBJETOS
                        int contadorObjeto = 0;
                        int cantObjetos = kvp.Value.usertypes.Count;
                        foreach (KeyValuePair<string, Simbolo> sim in kvp.Value.usertypes)
                        {
                            CreateType objeto = (CreateType)sim.Value.valor;
                            if (contadorObjeto < cantObjetos - 1)
                            {
                                chison += "<\n";
                                chison += "\"CQL-TYPE\" = \"OBJECT\", \n";
                                chison += "\"NAME\" = \"" + objeto.idType + "\", \n";
                                chison += "\"ATTRS\" = [\n";
                                int contAtri = 0;
                                int cantAtri = objeto.itemTypee.Count;
                                foreach (itemType it in objeto.itemTypee)
                                {
                                    if (contAtri < cantAtri - 1)
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + it.id + "\", \n";
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.id + "\" \n";
                                        }
                                        else
                                        if (it.tipo.tipo == tipoDato.list || it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.tipo + ">" + "\" \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\" \n";
                                        }
                                        chison += ">,\n";
                                    }
                                    else
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + it.id + "\", \n";
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.id + "\" \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list || it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.tipo + ">" + "\" \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\" \n";
                                        }
                                        chison += ">\n";
                                    }
                                    contAtri++;
                                }

                                chison += "] \n";
                                chison += ">,\n";
                            }
                            else
                            {
                                chison += "<\n";
                                chison += "\"CQL-TYPE\" = \"OBJECT\", \n";
                                chison += "\"NAME\" = \"" + objeto.idType + "\", \n";
                                chison += "\"ATTRS\" = [\n";
                                int contAtri = 0;
                                int cantAtri = objeto.itemTypee.Count;
                                foreach (itemType it in objeto.itemTypee)
                                {
                                    if (contAtri < cantAtri - 1)
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + it.id + "\", \n";
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.id + "\" \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list || it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.tipo + ">" + "\" \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\" \n";
                                        }
                                        chison += ">,\n";
                                    }
                                    else
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + it.id + "\", \n";
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.id + "\" \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list || it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.tipo + ">" + "\" \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\" \n";
                                        }
                                        chison += ">\n";
                                    }
                                    contAtri++;
                                }

                                chison += "] \n";
                                chison += ">\n";
                            }
                            contadorObjeto++;
                        }
                        #endregion
                        //OBJETOS----------------------------------------------------------------------------

                        //TABLAS-----------------------------------------------------------------------------
                        #region TABLAS
                        int contadorTablas = 0;
                        int cantTablas = kvp.Value.Tabla.Count;
                        if (cantTablas > 0)
                        {
                            chison += ", \n";
                        }
                        foreach (KeyValuePair<string, Tabla> sim in kvp.Value.Tabla)
                        {
                            if (contadorTablas < cantTablas - 1)
                            {
                                chison += "<\n";
                                chison += "\"CQL-TYPE\" = \"TABLE\", \n";
                                chison += "\"NAME\" = \"" + sim.Value.idTabla + "\", \n";
                                chison += "\"COLUMNS\" = [\n";

                                #region COLUMNAS

                                int contadorColumnas = 0;
                                int cantColumnas = sim.Value.columnasTabla.Count;
                                foreach (KeyValuePair<string, Columna> col in sim.Value.columnasTabla)
                                {
                                    if (contadorColumnas < cantColumnas - 1)
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + col.Value.idColumna + "\", \n";
                                        if (col.Value.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.idTipo + "\", \n";
                                        }
                                        else if (col.Value.tipo == tipoDato.list || col.Value.tipo == tipoDato.set)
                                        {
                                            if (col.Value.tipoValor == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.idTipo + ">" + "\", \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.tipoValor + ">" + "\", \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.tipo + "\", \n";
                                        }

                                        if (col.Value.primaryKey == true)
                                        {
                                            chison += "\"PK\" =  TRUE \n";
                                        }
                                        else
                                        {
                                            chison += "\"PK\" =  FALSE \n";
                                        }
                                        chison += ">,\n";
                                    }
                                    else
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + col.Value.idColumna + "\", \n";
                                        if (col.Value.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.idTipo + "\", \n";
                                        }
                                        else if (col.Value.tipo == tipoDato.list || col.Value.tipo == tipoDato.set)
                                        {
                                            if (col.Value.tipoValor == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.idTipo + ">" + "\", \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.tipoValor + ">" + "\", \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.tipo + "\", \n";
                                        }

                                        if (col.Value.primaryKey == true)
                                        {
                                            chison += "\"PK\" =  TRUE \n";
                                        }
                                        else
                                        {
                                            chison += "\"PK\" =  FALSE \n";
                                        }
                                        chison += ">\n";
                                    }
                                    contadorColumnas++;
                                }

                                chison += "] \n";
                                #endregion


                                #region DATA
                                chison += ",";
                                chison += "\"DATA\" = [\n";

                                chison += EscribirDATAcolumna(sim.Value);

                                chison += "] \n";
                                #endregion

                                chison += ">,\n";
                            }
                            else
                            {
                                chison += "<\n";
                                chison += "\"CQL-TYPE\" = \"TABLE\", \n";
                                chison += "\"NAME\" = \"" + sim.Value.idTabla + "\", \n";
                                chison += "\"COLUMNS\" = [\n";

                                #region COLUMNAS

                                int contadorColumnas = 0;
                                int cantColumnas = sim.Value.columnasTabla.Count;
                                foreach (KeyValuePair<string, Columna> col in sim.Value.columnasTabla)
                                {
                                    if (contadorColumnas < cantColumnas - 1)
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + col.Value.idColumna + "\", \n";
                                        if (col.Value.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.idTipo + "\", \n";
                                        }
                                        else if (col.Value.tipo == tipoDato.list || col.Value.tipo == tipoDato.set)
                                        {
                                            if (col.Value.tipoValor == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.idTipo + ">" + "\", \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.tipoValor + ">" + "\", \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.tipo + "\", \n";
                                        }

                                        if (col.Value.primaryKey == true)
                                        {
                                            chison += "\"PK\" =  TRUE \n";
                                        }
                                        else
                                        {
                                            chison += "\"PK\" =  FALSE \n";
                                        }
                                        chison += ">,\n";
                                    }
                                    else
                                    {
                                        chison += "<\n";
                                        chison += "\"NAME\" = \"" + col.Value.idColumna + "\", \n";
                                        if (col.Value.tipo == tipoDato.id)
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.idTipo + "\", \n";
                                        }
                                        else if (col.Value.tipo == tipoDato.list || col.Value.tipo == tipoDato.set)
                                        {
                                            if (col.Value.tipoValor == tipoDato.id)
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.idTipo + ">" + "\", \n";
                                            }
                                            else
                                            {
                                                chison += "\"TYPE\" = \"" + col.Value.tipo + "<" + col.Value.tipoValor + ">" + "\", \n";
                                            }
                                        }
                                        else
                                        {
                                            chison += "\"TYPE\" = \"" + col.Value.tipo + "\", \n";
                                        }

                                        if (col.Value.primaryKey == true)
                                        {
                                            chison += "\"PK\" =  TRUE \n";
                                        }
                                        else
                                        {
                                            chison += "\"PK\" =  FALSE \n";
                                        }
                                        chison += ">\n";
                                    }
                                    contadorColumnas++;
                                }

                                chison += "], \n";
                                #endregion

                                #region DATA
                                chison += "\"DATA\" = [\n";

                                chison += EscribirDATAcolumna(sim.Value);

                                chison += "] \n";
                                #endregion



                                chison += ">\n";
                            }
                            contadorTablas++;
                        }
                        #endregion
                        //TABLAS-----------------------------------------------------------------------------


                        #region PROCEDIMIENTO

                        int cantColumnasDATA = 0;
                        foreach (KeyValuePair<string, Tabla> sim in kvp.Value.Tabla)
                        {

                            foreach (KeyValuePair<string, Columna> col in sim.Value.columnasTabla)
                            {
                                cantColumnasDATA = col.Value.valorColumna.Count;
                                break;
                            }
                            break;
                        }
                        if (kvp.Value.procedures.Count > 0 && cantColumnasDATA > 0)
                        {
                            chison += ",\n" + EscribirProcedimiento(kvp.Value.procedures);
                        }
                        else
                        {
                            chison += EscribirProcedimiento(kvp.Value.procedures);
                        }

                        #endregion

                        chison += "] \n";
                        chison += "> \n";

                    }
                    contadorB++;
                }

                chison += "]\n";
                //escribir bases de datos-----------------------------------


                chison += ">$";
                //escribir y guardar el archivo------------------------------
                GuardarArchivo(chison);
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                               "No se puede realizar el committ"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }


        public String EscribirProcedimiento(Dictionary<String, Simbolo> procedures)
        {
            String aux = "";
            int contador = 0;
            int cantProcedimientos = procedures.Count;
            foreach (KeyValuePair<String, Simbolo> kvp in procedures)
            {
                Simbolo simi = kvp.Value;
                if (contador < cantProcedimientos -1)
                {
                    aux += "< \n";
                    aux += "\"CQL-TYPE\" = \"PROCEDURE\",\n";
                    aux += "\"NAME\" = \"" + simi.id + "\",\n";
                    aux += "\"PARAMETERS\" = [\n " + EscribirParametros(simi.parametros, simi.retornos) + "],\n";
                    aux += "\"INSTR\" = $ \n" + EscribirSentenciasProcedure(simi.nodo.ChildNodes.ElementAt(1)) +"$\n";
                    aux += ">,\n";
                }
                else
                {
                    aux += "< \n";
                    aux += "\"CQL-TYPE\" = \"PROCEDURE\",\n";
                    aux += "\"NAME\" = \"" + simi.id + "\",\n";
                    aux += "\"PARAMETERS\" = [\n " + EscribirParametros(simi.parametros, simi.retornos) + "],\n";
                    aux += "\"INSTR\" = $ \n" + EscribirSentenciasProcedure(simi.nodo.ChildNodes.ElementAt(1)) + "$\n";
                    aux += ">\n";
                }
                contador++;
            }                 
            return aux;
        }


        public String EscribirSentenciasProcedure(ParseTreeNode nodo)
        {
            String aux = "";
            aux += Inicio(nodo) + "\n";
            return aux;
        }


        public String Inicio(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                String aux2 = Inicio(nodo.ChildNodes.ElementAt(0));
                aux2 += Sentencia(nodo.ChildNodes.ElementAt(1));
                return aux2 + "\n";
            }
            else if (nodo.ChildNodes.Count == 1)
            {
                String aux = "";
                aux += Sentencia(nodo.ChildNodes.ElementAt(0));
                return aux + "\n";
            }
            else
            {
                return "\n";
            }
        }


        public String Sentencia(ParseTreeNode nodo)
        {
            String aux = "";
            if (!(nodo.Term is Terminal))
            {
                //si es no terminal
                for (int i = 0; i< nodo.ChildNodes.Count; i++)
                {
                    aux += Sentencia(nodo.ChildNodes.ElementAt(i));
                }
                return aux;
            }
            else
            {
                //si es terminal
                if (nodo.Token.Text.Equals("@"))
                {
                    return aux += nodo.Token.Text;
                }
                else if (nodo.Token.Text.Equals("}"))
                {
                    return aux += nodo.Token.Text + " \n";
                }
                else
                {
                    return aux += nodo.Token.Text + " ";
                }
            }

            return aux;
        }

        //revisar el tipado
        public String EscribirParametros(LinkedList<Parametros> parametros, LinkedList<Parametros> retornos)
        {
            String aux = "";
            int contador = 0;
            int cantParametros = parametros.Count;
            foreach (Parametros pa in parametros)
            {
                if (contador < cantParametros -1)
                {
                    aux += "<\n";
                    aux += "\"NAME\" = \"" + pa.id + "\",\n";
                    if (pa.tipo.tipo == tipoDato.id)
                    {
                        aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.id) + "\",\n";
                    }
                    else if (pa.tipo.tipo == tipoDato.set || pa.tipo.tipo == tipoDato.list)
                    {
                        if (pa.tipo.tipoValor.tipo == tipoDato.list || pa.tipo.tipoValor.tipo == tipoDato.set)
                        {
                            aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo)+ "< " + ConcatenarTipos(pa.tipo.tipoValor)+" >" + "\",\n";
                        }
                        else
                        {
                            aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "<" + Convert.ToString(pa.tipo.tipoValor.tipo) + ">" + "\",\n";
                        }
                    }
                    else
                    {
                        aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "\",\n";
                    }

                    aux += "\"AS\" = IN\n";
                    aux += ">,\n";
                }
                else
                {
                    aux += "<\n";
                    aux += "\"NAME\" = \"" + pa.id + "\",\n";
                    if (pa.tipo.tipo == tipoDato.id)
                    {
                        aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.id) + "\",\n";
                    }
                    else if (pa.tipo.tipo == tipoDato.set || pa.tipo.tipo == tipoDato.list)
                    {
                        if (pa.tipo.tipoValor.tipo == tipoDato.list || pa.tipo.tipoValor.tipo == tipoDato.set)
                        {
                            aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "< " + ConcatenarTipos(pa.tipo.tipoValor) + " >" + "\",\n";
                        }
                        else
                        {
                            aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "<" + Convert.ToString(pa.tipo.tipoValor.tipo) + ">" + "\",\n";
                        }
                    }
                    else
                    {
                        aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "\",\n";
                    }

                    aux += "\"AS\" = IN\n";
                    aux += ">\n";
                }
                contador++;
            }

            if (retornos.Count >0 && parametros.Count >0)
            {
                aux += ",";
            }
            contador = 0;
            cantParametros = retornos.Count;
            foreach (Parametros pa in retornos)
            {
                if (contador < cantParametros - 1)
                {
                    aux += "<\n";
                    aux += "\"NAME\" = \"" + pa.id + "\",\n";
                    if (pa.tipo.tipo == tipoDato.id)
                    {
                        aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.id) + "\",\n";
                    }
                    else if (pa.tipo.tipo == tipoDato.set || pa.tipo.tipo == tipoDato.list)
                    {
                        if (pa.tipo.tipoValor.tipo == tipoDato.list || pa.tipo.tipoValor.tipo == tipoDato.set)
                        {
                            aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "< " + ConcatenarTipos(pa.tipo.tipoValor) + " >" + "\",\n";
                        }
                        else
                        {
                            aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "<" + Convert.ToString(pa.tipo.tipoValor.tipo) + ">" + "\",\n";
                        }
                    }
                    else
                    {
                        aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "\",\n";
                    }

                    aux += "\"AS\" = OUT\n";
                    aux += ">,\n";
                }
                else
                {
                    aux += "<\n";
                    aux += "\"NAME\" = \"" + pa.id + "\",\n";
                    if (pa.tipo.tipo == tipoDato.id)
                    {
                        aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.id) + "\",\n";
                    }
                    else if (pa.tipo.tipo == tipoDato.set || pa.tipo.tipo == tipoDato.list)
                    {
                        if (pa.tipo.tipoValor.tipo == tipoDato.list || pa.tipo.tipoValor.tipo == tipoDato.set)
                        {
                            aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "< " + ConcatenarTipos(pa.tipo.tipoValor) + " >" + "\",\n";
                        }
                        else
                        {
                            aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "<" + Convert.ToString(pa.tipo.tipoValor.tipo) + ">" + "\",\n";
                        }
                    }
                    else
                    {
                        aux += "\"TYPE\" = \"" + Convert.ToString(pa.tipo.tipo) + "\",\n";
                    }

                    aux += "\"AS\" = OUT\n";
                    aux += ">\n";
                }
                contador++;
            }

            return aux;
        }


        public String ConcatenarTipos(Tipo tipo)
        {
            String aux = Convert.ToString(tipo.tipo)+ "< ";
            if (tipo.tipoValor != null) {
                if (tipo.tipoValor.tipo == tipoDato.list || tipo.tipoValor.tipo == tipoDato.set)
                {
                    aux += ConcatenarTipos(tipo.tipoValor);
                }
                else
                {
                    aux += tipo.tipoValor.tipo;
                }
            }
            else
            {
                aux += tipo.tipo;
            }
            aux += "> ";
            return aux;
        }

        public String EscribirDATAcolumna(Tabla tabla)
        {
            String aux = "";

            int contadorColDATA = 0;
            int cantColumnasDATA = 0;
            int columnasHAY =tabla.columnasTabla.Count;
            int sharolin = 0;
            foreach (KeyValuePair<string, Columna> col in tabla.columnasTabla)
            {
                cantColumnasDATA = col.Value.valorColumna.Count;
                break;
            }
            int posicion = 0;
            while (posicion < cantColumnasDATA)
            {
                sharolin = 0;
                if (posicion == cantColumnasDATA - 1)
                {
                    aux += "<\n";                   
                    foreach (KeyValuePair<string, Columna> col in tabla.columnasTabla)
                    {
                        Columna encontrada = col.Value;
                        if (sharolin < columnasHAY - 1)
                        {
                            if (encontrada.tipo == tipoDato.id)
                            {
                                aux += "\"" + encontrada.idColumna + "\" = ";
                                CreateType type = (CreateType)encontrada.valorColumna.ElementAt(posicion);
                                aux += "<\n";
                                int cantidadITEMS = type.itemTypee.Count;
                                int conIT = 0;
                                foreach (itemType it in type.itemTypee)
                                {
                                    if (conIT < cantidadITEMS - 1)
                                    {
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            CreateType tyy = (CreateType)it.valor;
                                            aux += "\"" + it.id + "\" = " + EscribirObjetos(tyy) + ", \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "],\n";
                                            }
                                            else
                                            {

                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "],\n";
                                            }
                                        }
                                        else if (it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ", \n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty);
                                                    }
                                                    cont++;
                                                }
                                                aux += "},\n";
                                            }
                                            else
                                            {
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "},\n";
                                            }
                                        }
                                        else
                                        {
                                            aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + ",\n";
                                        }
                                    }
                                    else
                                    {
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            CreateType tyy = (CreateType)it.valor;
                                            aux += "\"" + it.id + "\" = " + EscribirObjetos(tyy) + " \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "]\n";
                                            }
                                            else
                                            {

                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "]\n";
                                            }
                                        }
                                        else if (it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ", \n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty);
                                                    }
                                                    cont++;
                                                }
                                                aux += "}\n";
                                            }
                                            else
                                            {
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "}\n";
                                            }
                                        }
                                        else
                                        {
                                            aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + "\n";
                                        }
                                    }
                                    conIT++;
                                }
                                aux += ">,";

                            }
                            else if (encontrada.tipo == tipoDato.list)
                            {
                                if (encontrada.tipoValor == tipoDato.id)
                                {   //el del id
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "[\n";
                                    int cont = 0;
                                    foreach (CreateType ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirObjetos(ty) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirObjetos(ty) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "],\n";
                                }
                                else
                                {

                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "[\n";
                                    int cont = 0;
                                    foreach (object ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "],\n";
                                }
                            }
                            else if (encontrada.tipo == tipoDato.set)
                            {
                                if (encontrada.tipo == tipoDato.id)
                                {   //el del id
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "{\n";
                                    int cont = 0;
                                    foreach (CreateType ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirObjetos(ty) + ", \n";
                                        }
                                        else
                                        {
                                            aux += EscribirObjetos(ty);
                                        }
                                        cont++;
                                    }
                                    aux += "},\n";
                                }
                                else
                                {
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "{\n";
                                    int cont = 0;
                                    foreach (object ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "},\n";
                                }
                            }
                            else
                            {
                                aux += "\"" + encontrada.idColumna + "\" = " + EscribirPrimitivoObject(
                                    encontrada.valorColumna.ElementAt(posicion), encontrada.tipo) + ",\n";
                            }
                        }
                        else
                        {
                            if (encontrada.tipo == tipoDato.id)
                            {
                                aux += "\"" + encontrada.idColumna + "\" = ";
                                CreateType type = (CreateType)encontrada.valorColumna.ElementAt(posicion);
                                aux += "<\n";
                                int cantidadITEMS = type.itemTypee.Count;
                                int conIT = 0;
                                foreach (itemType it in type.itemTypee)
                                {
                                    if (conIT < cantidadITEMS - 1)
                                    {
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            CreateType tyy = (CreateType)it.valor;
                                            aux += EscribirObjetos(tyy) + ", \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "],\n";
                                            }
                                            else
                                            {

                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "],\n";
                                            }
                                        }
                                        else if (it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ", \n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty);
                                                    }
                                                    cont++;
                                                }
                                                aux += "},\n";
                                            }
                                            else
                                            {
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "},\n";
                                            }
                                        }
                                        else
                                        {
                                            aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + ",\n";
                                        }
                                    }
                                    else
                                    {
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            CreateType tyy = (CreateType)it.valor;
                                            aux += EscribirObjetos(tyy) + " \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "]\n";
                                            }
                                            else
                                            {

                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "]\n";
                                            }
                                        }
                                        else if (it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ", \n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty);
                                                    }
                                                    cont++;
                                                }
                                                aux += "}\n";
                                            }
                                            else
                                            {
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "}\n";
                                            }
                                        }
                                        else
                                        {
                                            aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + "\n";
                                        }
                                    }
                                    conIT++;
                                }
                                aux += ">";

                            }
                            else if (encontrada.tipo == tipoDato.list)
                            {
                                if (encontrada.tipoValor == tipoDato.id)
                                {   //el del id
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "[\n";
                                    int cont = 0;
                                    foreach (CreateType ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirObjetos(ty) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirObjetos(ty) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "]\n";
                                }
                                else
                                {

                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "[\n";
                                    int cont = 0;
                                    foreach (object ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "]\n";
                                }
                            }
                            else if (encontrada.tipo == tipoDato.set)
                            {
                                if (encontrada.tipo == tipoDato.id)
                                {   //el del id
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "{\n";
                                    int cont = 0;
                                    foreach (CreateType ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirObjetos(ty) + ", \n";
                                        }
                                        else
                                        {
                                            aux += EscribirObjetos(ty);
                                        }
                                        cont++;
                                    }
                                    aux += "}\n";
                                }
                                else
                                {
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "{\n";
                                    int cont = 0;
                                    foreach (object ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "}\n";
                                }
                            }
                            else
                            {
                                aux += "\"" + encontrada.idColumna + "\" = " + EscribirPrimitivoObject(
                                    encontrada.valorColumna.ElementAt(posicion), encontrada.tipo) + "\n";
                            }
                        }
                        sharolin++;
                    }
                    aux += ">\n";
                }
                else
                {
                    aux += "<\n";
                    foreach (KeyValuePair<string, Columna> col in tabla.columnasTabla)
                    {
                        Columna encontrada = col.Value;
                        if (sharolin < columnasHAY - 1)
                        {
                            if (encontrada.tipo == tipoDato.id)
                            {
                                aux += "\"" + encontrada.idColumna + "\" = ";
                                CreateType type = (CreateType)encontrada.valorColumna.ElementAt(posicion);
                                aux += "<\n";
                                int cantidadITEMS = type.itemTypee.Count;
                                int conIT = 0;
                                foreach (itemType it in type.itemTypee)
                                {
                                    if (conIT < cantidadITEMS - 1)
                                    {
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            CreateType tyy = (CreateType)it.valor;
                                            aux += "\"" + it.id + "\" = " + EscribirObjetos(tyy) + ", \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "],\n";
                                            }
                                            else
                                            {

                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "],\n";
                                            }
                                        }
                                        else if (it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ", \n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty);
                                                    }
                                                    cont++;
                                                }
                                                aux += "},\n";
                                            }
                                            else
                                            {
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "},\n";
                                            }
                                        }
                                        else
                                        {
                                            aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + ",\n";
                                        }
                                    }
                                    else
                                    {
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            CreateType tyy = (CreateType)it.valor;
                                            aux += "\"" + it.id + "\" = " + EscribirObjetos(tyy) + " \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "]\n";
                                            }
                                            else
                                            {

                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "]\n";
                                            }
                                        }
                                        else if (it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ", \n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty);
                                                    }
                                                    cont++;
                                                }
                                                aux += "}\n";
                                            }
                                            else
                                            {
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "}\n";
                                            }
                                        }
                                        else
                                        {
                                            aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + "\n";
                                        }
                                    }
                                    conIT++;
                                }
                                aux += ">,";

                            }
                            else if (encontrada.tipo == tipoDato.list)
                            {
                                if (encontrada.tipoValor == tipoDato.id)
                                {   //el del id
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "[\n";
                                    int cont = 0;
                                    foreach (CreateType ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirObjetos(ty) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirObjetos(ty) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "],\n";
                                }
                                else
                                {

                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "[\n";
                                    int cont = 0;
                                    foreach (object ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "],\n";
                                }
                            }
                            else if (encontrada.tipo == tipoDato.set)
                            {
                                if (encontrada.tipo == tipoDato.id)
                                {   //el del id
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "{\n";
                                    int cont = 0;
                                    foreach (CreateType ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirObjetos(ty) + ", \n";
                                        }
                                        else
                                        {
                                            aux += EscribirObjetos(ty);
                                        }
                                        cont++;
                                    }
                                    aux += "},\n";
                                }
                                else
                                {
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "{\n";
                                    int cont = 0;
                                    foreach (object ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "},\n";
                                }
                            }
                            else
                            {
                                aux += "\"" + encontrada.idColumna + "\" = " + EscribirPrimitivoObject(
                                    encontrada.valorColumna.ElementAt(posicion), encontrada.tipo) + ",\n";
                            }
                        }
                        else
                        {
                            if (encontrada.tipo == tipoDato.id)
                            {
                                aux += "\"" + encontrada.idColumna + "\" = ";
                                CreateType type = (CreateType)encontrada.valorColumna.ElementAt(posicion);
                                aux += "<\n";
                                int cantidadITEMS = type.itemTypee.Count;
                                int conIT = 0;
                                foreach (itemType it in type.itemTypee)
                                {
                                    if (conIT < cantidadITEMS - 1)
                                    {
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            CreateType tyy = (CreateType)it.valor;
                                            aux += "\"" + it.id + "\" = " +  EscribirObjetos(tyy) + ", \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "],\n";
                                            }
                                            else
                                            {

                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "],\n";
                                            }
                                        }
                                        else if (it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ", \n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty);
                                                    }
                                                    cont++;
                                                }
                                                aux += "},\n";
                                            }
                                            else
                                            {
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "},\n";
                                            }
                                        }
                                        else
                                        {
                                            aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + ",\n";
                                        }
                                    }
                                    else
                                    {
                                        if (it.tipo.tipo == tipoDato.id)
                                        {
                                            CreateType tyy = (CreateType)it.valor;
                                            aux += "\"" + it.id + "\" = " + EscribirObjetos(tyy) + " \n";
                                        }
                                        else if (it.tipo.tipo == tipoDato.list)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "]\n";
                                            }
                                            else
                                            {

                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "[\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "]\n";
                                            }
                                        }
                                        else if (it.tipo.tipo == tipoDato.set)
                                        {
                                            if (it.tipo.tipoValor.tipo == tipoDato.id)
                                            {   //el del id
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (CreateType ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += EscribirObjetos(ty) + ", \n";
                                                    }
                                                    else
                                                    {
                                                        aux += EscribirObjetos(ty);
                                                    }
                                                    cont++;
                                                }
                                                aux += "}\n";
                                            }
                                            else
                                            {
                                                Lista listatypes = (Lista)it.valor;
                                                aux += "\"" + it.id + "\" = ";
                                                aux += "{\n";
                                                int cont = 0;
                                                foreach (object ty in listatypes.listaValores)
                                                {
                                                    if (cont < listatypes.listaValores.Count - 1)
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                                    }
                                                    else
                                                    {
                                                        aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                                    }
                                                    cont++;
                                                }
                                                aux += "}\n";
                                            }
                                        }
                                        else
                                        {
                                            aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + "\n";
                                        }
                                    }
                                    conIT++;
                                }
                                aux += ">";

                            }
                            else if (encontrada.tipo == tipoDato.list)
                            {
                                if (encontrada.tipoValor == tipoDato.id)
                                {   //el del id
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "[\n";
                                    int cont = 0;
                                    foreach (CreateType ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirObjetos(ty) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirObjetos(ty) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "]\n";
                                }
                                else
                                {

                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "[\n";
                                    int cont = 0;
                                    foreach (object ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "]\n";
                                }
                            }
                            else if (encontrada.tipo == tipoDato.set)
                            {
                                if (encontrada.tipo == tipoDato.id)
                                {   //el del id
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "{\n";
                                    int cont = 0;
                                    foreach (CreateType ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirObjetos(ty) + ", \n";
                                        }
                                        else
                                        {
                                            aux += EscribirObjetos(ty);
                                        }
                                        cont++;
                                    }
                                    aux += "}\n";
                                }
                                else
                                {
                                    Lista listatypes = (Lista)encontrada.valorColumna.ElementAt(posicion);
                                    aux += "\"" + encontrada.idColumna + "\" = ";
                                    aux += "{\n";
                                    int cont = 0;
                                    foreach (object ty in listatypes.listaValores)
                                    {
                                        if (cont < listatypes.listaValores.Count - 1)
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + ",\n";
                                        }
                                        else
                                        {
                                            aux += EscribirPrimitivoObject(ty, encontrada.tipoValor) + "\n";
                                        }
                                        cont++;
                                    }
                                    aux += "}\n";
                                }
                            }
                            else
                            {
                                aux += "\"" + encontrada.idColumna + "\" = " + EscribirPrimitivoObject(
                                    encontrada.valorColumna.ElementAt(posicion), encontrada.tipo) + "\n";
                            }
                        }
                        sharolin++;
                    }
                    aux += ">,\n";
                }
                posicion++;
            }

            return aux;
        }


        public String EscribirPrimitivo(itemType itit)
        {
            if (itit.tipo.tipo == tipoDato.cadena)
            {
                return "\"" + Convert.ToString(itit.valor) + "\"";
            }
            else if (itit.tipo.tipo == tipoDato.date || itit.tipo.tipo == tipoDato.time)
            {
                return "'" + Convert.ToString(itit.valor) + "'";
            }
            else if (itit.tipo.tipo == tipoDato.counter || itit.tipo.tipo == tipoDato.decimall || itit.tipo.tipo == tipoDato.entero
                || itit.tipo.tipo == tipoDato.booleano)
            {
                return Convert.ToString(itit.valor);
            }
            else if (itit.tipo.tipo == tipoDato.nulo)
            {
                return "null";
            }

            return "";
        }



        public String EscribirPrimitivoObject(object itit, tipoDato tipo)
        {
            if (tipo == tipoDato.cadena)
            {
                return "\"" + Convert.ToString(itit) + "\"";
            }
            else if (tipo == tipoDato.date || tipo == tipoDato.time)
            {
                return "'" + Convert.ToString(itit) + "'";
            }
            else if (tipo == tipoDato.counter || tipo == tipoDato.decimall || tipo == tipoDato.entero
                || tipo == tipoDato.booleano)
            {
                return Convert.ToString(itit);
            }
            else if (tipo == tipoDato.nulo)
            {
                return "null";
            }

            return "";
        }


        public String EscribirObjetos(CreateType objeto)
        {
            String aux = "";

            aux += "<\n";
            int cantidadITEMS = objeto.itemTypee.Count;
            int conIT = 0;
            foreach (itemType it in objeto.itemTypee)
            {
                if (conIT < cantidadITEMS - 1)
                {
                    if (it.tipo.tipo == tipoDato.id)
                    {
                        CreateType tyy = (CreateType)it.valor;
                        aux += "\"" + it.id + "\" = " + EscribirObjetos(tyy) + ", \n"; 
                    }
                    else if (it.tipo.tipo == tipoDato.list)
                    {
                        if (it.tipo.tipoValor.tipo == tipoDato.id)
                        {   //el del id
                            Lista listatypes = (Lista)it.valor;
                            aux += "\"" + it.id + "\" = ";
                            aux += "[\n";
                            int cont = 0;
                            foreach (CreateType ty in listatypes.listaValores) {
                                if (cont < listatypes.listaValores.Count - 1)
                                {
                                    aux += EscribirObjetos(ty) + ",\n";
                                }
                                else {
                                    aux += EscribirObjetos(ty)+ "\n";
                                }
                                cont++;
                            }
                            aux += "],\n";
                        }
                        else
                        {

                            Lista listatypes = (Lista)it.valor;
                            aux += "\"" + it.id + "\" = ";
                            aux += "[\n";
                            int cont = 0;
                            foreach (object ty in listatypes.listaValores)
                            {
                                if (cont < listatypes.listaValores.Count - 1)
                                {
                                    aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                }
                                else
                                {
                                    aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                }
                                cont++;
                            }
                            aux += "]\n";
                        }
                    }
                    else if (it.tipo.tipo == tipoDato.set)
                    {
                        if (it.tipo.tipoValor.tipo == tipoDato.id)
                        {   //el del id
                            Lista listatypes = (Lista)it.valor;
                            aux += "\"" + it.id + "\" = ";
                            aux += "{\n";
                            int cont = 0;
                            foreach (CreateType ty in listatypes.listaValores)
                            {
                                if (cont < listatypes.listaValores.Count - 1)
                                {
                                    aux += EscribirObjetos(ty) + ", \n";
                                }
                                else
                                {
                                    aux += EscribirObjetos(ty);
                                }
                                cont++;
                            }
                            aux += "},\n";
                        }
                        else
                        {
                            Lista listatypes = (Lista)it.valor;
                            aux += "\"" + it.id + "\" = ";
                            aux += "{\n";
                            int cont = 0;
                            foreach (object ty in listatypes.listaValores)
                            {
                                if (cont < listatypes.listaValores.Count - 1)
                                {
                                    aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                }
                                else
                                {
                                    aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                }
                                cont++;
                            }
                            aux += "}\n";
                        }
                    }
                    else
                    {
                        aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + ",\n";
                    }
                }
                else
                {

                    if (it.tipo.tipo == tipoDato.id)
                    {
                        CreateType tyy = (CreateType)it.valor;
                        aux += "\"" + it.id + "\" = " + EscribirObjetos(tyy) + "\n";
                    }
                    else if (it.tipo.tipo == tipoDato.list)
                    {
                        if (it.tipo.tipoValor.tipo == tipoDato.id)
                        {   //el del id
                            Lista listatypes = (Lista)it.valor;
                            aux += "\"" + it.id + "\" = ";
                            aux += "[\n";
                            int cont = 0;
                            foreach (CreateType ty in listatypes.listaValores)
                            {
                                if (cont < listatypes.listaValores.Count - 1)
                                {
                                    aux += EscribirObjetos(ty) + ", \n";
                                }
                                else
                                {
                                    aux += EscribirObjetos(ty) + "\n";
                                }
                                cont++;
                            }
                            aux += "]\n";
                        }
                        else
                        {

                            Lista listatypes = (Lista)it.valor;
                            aux += "\"" + it.id + "\" = ";
                            aux += "[\n";
                            int cont = 0;
                            foreach (object ty in listatypes.listaValores)
                            {
                                if (cont < listatypes.listaValores.Count - 1)
                                {
                                    aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                }
                                else
                                {
                                    aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                }
                                cont++;
                            }
                            aux += "]\n";
                        }
                    }
                    else if (it.tipo.tipo == tipoDato.set)
                    {
                        if (it.tipo.tipoValor.tipo == tipoDato.id)
                        {   //el del id
                            Lista listatypes = (Lista)it.valor;
                            aux += "\"" + it.id + "\" = ";
                            aux += "{\n";
                            int cont = 0;
                            foreach (CreateType ty in listatypes.listaValores)
                            {
                                if (cont < listatypes.listaValores.Count - 1)
                                {
                                    aux += EscribirObjetos(ty) + ", \n";
                                }
                                else
                                {
                                    aux += EscribirObjetos(ty) + "\n";
                                }
                                cont++;
                            }
                            aux += "}\n";
                        }
                        else
                        {
                            Lista listatypes = (Lista)it.valor;
                            aux += "\"" + it.id + "\" = ";
                            aux += "{\n";
                            int cont = 0;
                            foreach (object ty in listatypes.listaValores)
                            {
                                if (cont < listatypes.listaValores.Count - 1)
                                {
                                    aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + ",\n";
                                }
                                else
                                {
                                    aux += "\"" + it.id + "\" =" + EscribirPrimitivoObject(ty, it.tipo.tipo) + "\n";
                                }
                                cont++;
                            }
                            aux += "}\n";
                        }
                    }
                    else
                    {
                        aux += "\"" + it.id + "\" =" + EscribirPrimitivo(it) + "\n";
                    }

                }
                conIT++;
            }
            aux += ">\n";

            return aux;
        }


        public void GuardarArchivo(String chison)
        {
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"CHISON"); ;
        
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(ruta, "PRINCIPALMIO.chison")))
            {                
                    outputFile.WriteLine(chison);
            }
        }

    }
}
