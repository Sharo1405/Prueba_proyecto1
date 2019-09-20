using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class crearUsuario : Instruccion
    {
        public String idUsuario { get; set; }
        public Expresion Contrasenia { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public crearUsuario(String idUsuario, Expresion Contrasenia,
            int linea, int columna)
        {
            this.idUsuario = idUsuario;
            this.Contrasenia = Contrasenia;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                management.idUsarioEnUso = idUsuario.ToLower();
                management.usuarios.Add(idUsuario.ToLower(), new userPass(idUsuario, Convert.ToString(Contrasenia.getValue(entorno, listas, management))));

            }
            catch (ArgumentException e)
            {
                listas.impresiones.AddLast("WARNNING!! ESE USUARIO YA EXISTE: " + idUsuario  );
                return TipoExcepcion.excep.UserAlreadyExists;
            }
            return tipoDato.ok;
        }
    }
}
