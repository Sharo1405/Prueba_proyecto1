using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class Asignacion : Instruccion
    {
        public String id { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        public Asignacion(String id, Expresion valor,
            int linea, int columna)
        {
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            Simbolo variable = entorno.get(id, entorno, Simbolo.Rol.VARIABLE);
            if (variable != null)
            {
                tipoDato tipoValor = valor.getType(entorno, listas);
                object value = valor.getValue(entorno, listas);
                if (variable.tipo == tipoValor)
                {
                    variable.valor = value;
                }
                else
                {
                    if (variable.tipo == tipoDato.entero && tipoValor == tipoDato.decimall)
                    {
                        //se redondea el valor
                        double nuevo = Math.Floor(Double.Parse(Convert.ToString(value)));
                        Int32 ainsertar = Int32.Parse(Convert.ToString(nuevo));                        
                        variable.valor = ainsertar;
                    }
                    else if (variable.tipo == tipoDato.decimall && tipoValor == tipoDato.entero)
                    {                        
                        variable.valor = Double.Parse(Convert.ToString(value));
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico, 
                            "Los tipos no coinciden para la variable. Tipos en cuestion: " + Convert.ToString(variable.tipo) + Convert.ToString(tipoValor)));
                    }
                }
            }
            else
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "La variable no existe para asignar valor: " + id));
                return tipoDato.errorSemantico;
            }
            return tipoDato.errorSemantico;
        }
    }
}
