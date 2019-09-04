using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Ciclos
{
    class WhileCiclo : Instrucciones.Instruccion
    {

        public Expresion condicion { get; set; }
        public StatementBlock sentencias { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public WhileCiclo()
        {

        }

        public WhileCiclo(Expresion cond, StatementBlock sentencias,
            int linea, int columna)
        {
            this.condicion = cond;
            this.sentencias = sentencias;
        }

        public Object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Object valor = condicion.getValue(entorno, listas, management);
                tipoDato tipo = condicion.getType(entorno, listas, management);
                if (tipoDato.booleano == tipo)
                {
                    while ((Boolean)valor)
                    {
                        Object retorno = sentencias.ejecutar(entorno, listas, management);

                        if (retorno is Breakk)
                        {
                            break;
                        }
                        else if (retorno is Continuee)
                        {
                            continue;
                        }
                        else if (retorno is Retorno)
                        {
                            return retorno;
                        }

                        valor = condicion.getValue(entorno, listas, management);
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna,
                        NodoError.tipoError.Semantico, "Condicion no valida para el While"));
                    return tipoDato.errorSemantico;
                }
                return valor;
            }
            catch (Exception e)
            {

            }
            listas.errores.AddLast(new NodoError(this.linea, this.columna,
                        NodoError.tipoError.Semantico, "Condicion no valida para el while"));
            return tipoDato.errorSemantico;
        }
    }
}
