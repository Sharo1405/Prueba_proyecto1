using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;

namespace Server.AST.Instrucciones
{
    class SwitchBlockStatement_Group : Instruccion
    {
        public LinkedList<NodoAST> listaCasee { get; set; }
        public Instruccion listaSentencias { get; set; }

        public SwitchBlockStatement_Group(LinkedList<NodoAST> listaCasee, Instruccion listaSentencias)
        {
            this.listaCasee = listaCasee;
            this.listaSentencias = listaSentencias;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            throw new NotImplementedException();
        }
    }
}
