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
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\", \n";
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
                                            chison += "\"TYPE\" = \"" + it.tipo.tipo + "\", \n";
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
                                        chison += "\"NAME\" = \"" + it.id + "\", \n"; if (it.tipo.tipo == tipoDato.id)
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
                                            if (it.tipo.tipoValor.tipo == tipoDato.id) {
                                                chison += "\"TYPE\" = \"" + it.tipo.tipo + "<" + it.tipo.tipoValor.id + ">" + "\" \n";
                                            }
                                            else {
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
