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
    class Casee : Expresion
    {
        public Expresion valorCase { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public Casee(Expresion valorCase, int linea, int col)
        {
            this.valorCase = valorCase;
            this.linea = linea;
            this.col = col;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                return valorCase.getType(entorno, listas, management);
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico, "No se puede ejecutar el Case"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                return valorCase.getValue(entorno, listas, management);
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.col, NodoError.tipoError.Semantico, "No se puede ejecutar el Case"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
