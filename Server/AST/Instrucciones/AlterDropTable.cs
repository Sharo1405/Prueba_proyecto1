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
    class AlterDropTable : Instruccion
    {
        public String idTabla { get; set; }
        public LinkedList<String> itemTypee = new LinkedList<String>();
        public int linea { get; set; }
        public int columna { get; set; }

        public AlterDropTable(String idTabla, LinkedList<String> itemTypee,
            int linea, int columna)
        {
            this.idTabla = idTabla;
            this.itemTypee = itemTypee;
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
                    if (basee.Tabla.TryGetValue(idTabla.ToLower(), out encontrado))
                    {                      
                        Columna nueva = new Columna();
                        foreach (String item in itemTypee)
                        {

                            Columna encontrado2 = new Columna();
                            if (encontrado.columnasTabla.TryGetValue(item, out encontrado2))
                            {
                                if (encontrado2.primaryKey == true)
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                        "No se puede eliminar la columa por ser tipo couter y ser primary key: " + 
                                        item + " En la tabla: " + encontrado.idTabla+ " Y en la base de datos: " + basee.idbase));
                                    return tipoDato.errorSemantico;
                                }
                                else {
                                    if (encontrado.columnasTabla.Remove(item.ToLower()))
                                    {
                                        //todo cool
                                    }
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "La tabla " + this.idTabla + " de la base de datos " 
                                + basee.idbase + " No existe la columna que quere eliminar" +
                                item.ToLower()));
                                return tipoDato.errorSemantico;
                            }
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "La Tabla" + this.idTabla + "en la base de datos EN USO no fue encontrada"));
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
                                "No se puede realizar el ALTER drop en la tabla" + idTabla));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
