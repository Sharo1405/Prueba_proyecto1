using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class Batchch : Instruccion
    {
        public LinkedList<NodoAST> listaConsultasETC = 
            new LinkedList<NodoAST>();
        public int linea { get; set; }
        public int columna { get; set; }

        public Batchch(LinkedList<NodoAST> listaConsultasETC,
            int linea, int columna)
        {
            this.listaConsultasETC = listaConsultasETC;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                foreach (NodoAST nodo in listaConsultasETC)
                {
                    object ob = new object();
                    if (nodo is Instruccion)
                    {
                        Instruccion ins = (Instruccion)nodo;
                        ob = ins.ejecutar(entorno, listas, management);
                    }
                    else if (nodo is Expresion)
                    {
                        Expresion exp = (Expresion)nodo;
                        ob = exp.getValue(entorno, listas, management);
                    }

                    if (ob.Equals(tipoDato.errorSemantico))
                    {
                        listas.impresiones.AddLast("WARNNING!! NO SE PUEDE SEGUIR EJECUTANDO EL BATCH");
                        return TipoExcepcion.excep.BatchException;
                    }
                }
            }
            catch (Exception)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                    "No se puede realizar el BATCH"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
