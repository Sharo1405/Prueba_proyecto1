using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.BaseDatos
{
    class userPass
    {
        public String idUsuario { get; set; }
        public String pass { get; set; }
        public List<String> permisoBase { get; set; }

        public userPass(String id, String pass)
        {
            this.idUsuario = id;
            this.pass = pass;
            this.permisoBase = new List<string>();
        }

        public userPass()
        {

        }

        public userPass(String permisoBase)
        {
            this.permisoBase.Add(permisoBase);
        }
    }
}
