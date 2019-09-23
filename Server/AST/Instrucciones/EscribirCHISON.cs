using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using System.IO;
using static Server.AST.Expresiones.Operacion;

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
                chison += "\"USERS\" = [ \n";
                //escribir usuarios---------------------------------
                int cantUsuario = management.usuarios.Count;
                int contador = 0;
                foreach (KeyValuePair<string, userPass> kvp in management.usuarios)
                {
                    if (contador < cantUsuario-1)
                    {
                        chison += "< \n";
                        chison += "\"NAME\" = \"" + kvp.Value.idUsuario + "\", \n";
                        chison += "\" PASSWORD\" = \"" + kvp.Value.pass + "\", \n";
                        chison += "\" PERMISSIONS\" = [";
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
                        chison += "\" PASSWORD \" = \"" + kvp.Value.pass + "\", \n";
                        chison += "\" PERMISSIONS\" = [ \n";
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
                chison += "] \n";
                //escribir usuarios---------------------------------

                chison += ">$";

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
