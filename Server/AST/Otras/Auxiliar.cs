using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Otras
{
    class Auxiliar
    {
        public LinkedList<Tipo> listaTipos = new LinkedList<Tipo>();
        Tipo tipo = new Tipo();
        public Object valorLlaves = new Object();
        
        public Auxiliar(LinkedList<Tipo> listaTipos, Object valorLlaves)
        {
            this.listaTipos = listaTipos;
            this.valorLlaves = valorLlaves;
        }

        public Auxiliar( Tipo tipo, Object valorLlaves)
        {
            this.tipo = tipo;
            this.valorLlaves = valorLlaves;
        }
    }
}
