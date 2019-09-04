using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class Imprimir : Instruccion
    {
        public Expresion expImpre { get; set; }
        public int linea { get; set; }
        public  int col { get; set; }


        public Imprimir(Expresion expImpre, int linea, int col)
        {
            this.expImpre = expImpre;
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                object valor = expImpre.getValue(entorno, listas, management);
                if (valor is Simbolo)
                {
                    Simbolo ss = (Simbolo)valor;
                    listas.impresiones.AddLast(Convert.ToString(ss.valor));
                }
                else
                {
                    listas.impresiones.AddLast(Convert.ToString(valor));
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico, "No se puede imprimir ese Argumento"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
