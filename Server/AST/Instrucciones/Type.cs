using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class itemType: Expresion
    {
        public String id { get; set; }
        //public tipoDato tipo { get; set; }
        public Tipo tipo { get; set; }
        public Object valor { get; set; }

        public itemType()
        {

        }

        public itemType(String id, Tipo tipo)
        {
            this.id = id;
            this.tipo = tipo;
        }

        public itemType(Tipo tipo, String id)
        {
            this.tipo = tipo;
            this.id = id;            
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            return valor;
        }

        public tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipo.tipo;
        }       

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
