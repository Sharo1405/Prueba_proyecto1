using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Otras
{
    class ClaveValor
    {
        public Object clave { get; set; }
        public tipoDato tipoClave { get; set; }
        public Object valor { get; set; }
        public tipoDato tipoValor { get; set; }

        public ClaveValor(Object clave, Object valor)
        {
            this.clave = clave;
            this.valor = valor;
        }

        public ClaveValor(Object clave, tipoDato tipoClave, Object valor, tipoDato tipoValor)
        {
            this.clave = clave;
            this.tipoClave = tipoClave;
            this.valor = valor;
            this.tipoValor = tipoValor;
        }
    }
}
