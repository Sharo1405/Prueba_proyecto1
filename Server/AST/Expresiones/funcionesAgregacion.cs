using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class funcionesAgregacion : Expresion
    {
        public tipoFuncionAgregacion idFuncion { get; set; }
        public Expresion Selectt { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public enum tipoFuncionAgregacion
        {
            COUNT,
            MIN,
            MAX,
            SUM,
            AVG,
            X
        }
           

        public funcionesAgregacion(tipoFuncionAgregacion idFuncion,
            Expresion Selectt, int linea, int columna)
        {
            this.idFuncion = idFuncion;
            this.Selectt = Selectt;
            this.linea = linea;
            this.columna = columna;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            if (tipoFuncionAgregacion.X == idFuncion)
            {
                return Operacion.tipoDato.errorSemantico;
            }
            else
            {
                return Operacion.tipoDato.entero;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            int numeroRetorno = 1234567891;
            try
            {
                if (idFuncion == tipoFuncionAgregacion.X)
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede realizar la funcion agregacion: " + Convert.ToString(idFuncion)));
                    return tipoDato.errorSemantico;
                }

                switch (idFuncion)
                {
                    case tipoFuncionAgregacion.COUNT:
                        Tabla elSelect = (Tabla)this.Selectt.getValue(entorno, listas, management);
                        foreach (KeyValuePair<string, Columna> kvp in elSelect.columnasTabla)
                        {
                            numeroRetorno = kvp.Value.valorColumna.Count;
                            break;
                        }
                        return numeroRetorno;



                    case tipoFuncionAgregacion.AVG:
                        Tabla elSelect2 = (Tabla)this.Selectt.getValue(entorno, listas, management);
                        foreach (KeyValuePair<string, Columna> kvp in elSelect2.columnasTabla)
                        {                            
                            try
                            {

                                foreach (object objs in kvp.Value.valorColumna)
                                {
                                    numeroRetorno += Convert.ToInt32(objs);
                                }

                                numeroRetorno = numeroRetorno / kvp.Value.valorColumna.Count;
                            }
                            catch (Exception e)
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                   "No se puede realizar el AVG porque no es tipo numerico la columna: " + Convert.ToString(idFuncion)));
                                return tipoDato.errorSemantico;
                            }
                            break;
                        }
                        return numeroRetorno;



                    case tipoFuncionAgregacion.MAX:
                        Tabla elSelect23 = (Tabla)this.Selectt.getValue(entorno, listas, management);
                        foreach (KeyValuePair<string, Columna> kvp in elSelect23.columnasTabla)
                        {
                            try
                            {
                                foreach (object objs in kvp.Value.valorColumna)
                                {
                                    if (Convert.ToInt32(objs) > numeroRetorno)
                                    {
                                        numeroRetorno = Convert.ToInt32(objs);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede realizar el MAX porque no es tipo numerico la columna: " + Convert.ToString(idFuncion)));
                                return tipoDato.errorSemantico;
                            }
                            break;
                        }
                        return numeroRetorno;



                    case tipoFuncionAgregacion.MIN:
                        Tabla elSelect231 = (Tabla)this.Selectt.getValue(entorno, listas, management);
                        foreach (KeyValuePair<string, Columna> kvp in elSelect231.columnasTabla)
                        {
                            try
                            {
                                foreach (object objs in kvp.Value.valorColumna)
                                {
                                    if (Convert.ToInt32(objs) < numeroRetorno)
                                    {
                                        numeroRetorno = Convert.ToInt32(objs);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede realizar el MIN porque no es tipo numerico la columna: " + Convert.ToString(idFuncion)));
                                return tipoDato.errorSemantico;
                            }
                            break;
                        }
                        return numeroRetorno;


                    case tipoFuncionAgregacion.SUM:
                        Tabla elSelect22 = (Tabla)this.Selectt.getValue(entorno, listas, management);
                        foreach (KeyValuePair<string, Columna> kvp in elSelect22.columnasTabla)
                        {
                            try
                            {
                                foreach (object objs in kvp.Value.valorColumna)
                                {
                                    numeroRetorno += Convert.ToInt32(objs);
                                }
                            }
                            catch (Exception e)
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede realizar el SUM porque no es tipo numerico la columna: " + Convert.ToString(idFuncion)));
                                return tipoDato.errorSemantico;
                            }
                            break;
                        }
                        return numeroRetorno;
                }

            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede realizar la cuncion agregacion: " + Convert.ToString(idFuncion)));
                return tipoDato.errorSemantico;
            }
            return numeroRetorno;
        }
    }
}
