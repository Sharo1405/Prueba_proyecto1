using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Otras
{
    class Tabla
    {
        public String idTabla { get; set; }
        public Dictionary<String,Columna> columnasTabla = new Dictionary<String, Columna>();
        public LinkedList<String> llavePrimaria = new LinkedList<String>();

        public Tabla()
        {

        }

        
        public Tabla(String idTabla, Dictionary<String, Columna> columnasTabla,
            LinkedList<String> llavePrimaria)
        {
            this.idTabla = idTabla;
            this.columnasTabla = columnasTabla;
            this.llavePrimaria = llavePrimaria;
        }
    }
}
