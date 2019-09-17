using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.BaseDatos
{
    class Administrador
    {
        public Dictionary<String, BaseDeDatos> basesExistentes =
            new Dictionary<string, BaseDeDatos>();

        public Dictionary<String, userPass> usuarios =
            new Dictionary<string, userPass>();

        public String baseEnUso { get; set; }
        public String idUsarioEnUso { get; set; }

        public Administrador()
        {

        }

        public BaseDeDatos getInUse()
        {
            try
            {
                BaseDeDatos encontrado = new BaseDeDatos();
                if (basesExistentes.TryGetValue(baseEnUso, out encontrado))
                {
                    return encontrado;
                }
            }
            catch (Exception e)
            {
            }
            return null;
        }


        public BaseDeDatos buscarBaseDatos(String id)
        {
            try
            {
                BaseDeDatos encontrado = new BaseDeDatos();
                if (basesExistentes.TryGetValue(id, out encontrado))
                {
                    return encontrado;                    
                }
            }
            catch (Exception e)
            {                
            }
            return null;
        }

        public tipoDato borrarBaseDatos(String id)
        {
            try
            {
                if (basesExistentes.Remove(id))
                {
                    return tipoDato.ok ;
                }
            }
            catch (Exception e)
            {
            }
            return tipoDato.errorSemantico;
        }
    }
}
