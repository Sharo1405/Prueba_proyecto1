using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Otras
{
    class ColAccesoSelect6
    {
        public String columna { get; set; }
        public String accesoCol { get; set; }

        public ColAccesoSelect6(String columna, String accesoCol)
        {
            this.columna = columna;
            this.accesoCol = accesoCol;
        }
    }
}
