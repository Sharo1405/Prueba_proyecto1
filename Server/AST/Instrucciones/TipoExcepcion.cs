using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;

namespace Server.AST.Instrucciones
{
    class TipoExcepcion : Instruccion
    {
        public excep tipoExcep { get; set; }

        public enum excep
        {
            TypeAlreadyExists,
            TypeDontExists,
            BDAlreadyExists,
            BDDontExists,
            UseBDException,
            TableAlreadyExists,
            TableDontExists,
            CounterTypeException,
            UserAlreadyExists,
            UserDontExists,
            ValuesException,
            ColumnException,
            BatchException,
            IndexOutException,
            ArithmeticException,
            NullPointerException,
            NumberReturnsException,
            FunctionAlreadyExists,
            ProcedureAlreadyExists,
            ObjectAlreadyExists,
            ErrorSemantico
        }

        public TipoExcepcion()
        {

        }

        public TipoExcepcion(excep tipoExcep)
        {
            this.tipoExcep = tipoExcep;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return this.tipoExcep;
        }
    }
}
