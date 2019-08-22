using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;

namespace Server.AST.Instrucciones
{
    class Defaultt: Instruccion
    {
        public int linea { get; set; }
        public int col { get; set; }

        public Defaultt(int linea, int col)
        {
            this.linea = linea;
            this.col = col;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
            }
            catch (Exception e)
            {                
            }
            return null;
        }
    }
}
