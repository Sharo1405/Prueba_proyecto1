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
    class accesosFuncionCadena3 : Instruccion
    {
        public String variable { get; set; }
        public LinkedList<FuncionesNativasCadenas> funciones = new LinkedList<FuncionesNativasCadenas>();
        public int linea { get; set; }
        public int columna { get; set; }

        public accesosFuncionCadena3(String variable,
            LinkedList<FuncionesNativasCadenas> funciones,
            int linea, int col)
        {
            this.variable = variable.ToLower();
            this.funciones = funciones;
            this.linea = linea;
            this.columna = col;
        }


        object auxFuncion = new object();
        int contador = 0;

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                auxFuncion = new object();
                contador = 0;
                Simbolo s = entorno.get(variable, entorno, Simbolo.Rol.VARIABLE);
                if (s != null)
                {
                    if (s.tipo == tipoDato.cadena)
                    {
                        auxFuncion = s.valor;
                        while (contador < funciones.Count)
                        {
                            FuncionesNativasCadenas puntos2 = funciones.ElementAt(contador);                            
                            FuncionesNativasCadenas funcion = (FuncionesNativasCadenas)puntos2.getValue(entorno, listas, management);
                            auxFuncion = devuelveFunconEjecutada(funcion, entorno, listas, management);                            
                            contador++;
                        }
                        return auxFuncion;
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                            "No se le puede aplicar funciones nativas a la variable, porque NO es de tipo cadena sino que de tipo: " + 
                            Convert.ToString(s.tipo)));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                    "No se le puede aplicar funciones nativas a la variable, porque NO EXISTE"));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, 
                    "No se le puede aplicar funciones nativas a la variable"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.errorSemantico;
        }


        public object devuelveFunconEjecutada(FuncionesNativasCadenas funcion, Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            switch (funcion.nativa)
            {
                case "length":
                    return Convert.ToString(auxFuncion).Length;

                case "touppercase":
                    return Convert.ToString(auxFuncion).ToUpper();

                case "tolowercase":
                    return Convert.ToString(auxFuncion).ToLower();

                case "startswith":
                    tipoDato es = funcion.exp1.getType(entorno, listas, management);
                    if (es != tipoDato.cadena)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El parametro de inicio para la funcion StartsWith no es de tipo cadena"));
                        return tipoDato.errorSemantico;
                    }
                    Object cadena = funcion.exp1.getValue(entorno, listas, management);
                    return Convert.ToString(auxFuncion).StartsWith(Convert.ToString(cadena));

                case "endswith":
                    tipoDato ess = funcion.exp1.getType(entorno, listas, management);
                    if (ess != tipoDato.cadena)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El parametro de inicio para la funcion endsWith no es de tipo cadena"));
                        return tipoDato.errorSemantico;
                    }
                    Object cadena2 = funcion.exp1.getValue(entorno, listas, management);
                    return Convert.ToString(auxFuncion).StartsWith(Convert.ToString(cadena2));

                case "substring":
                    tipoDato esss = funcion.exp1.getType(entorno, listas, management);
                    tipoDato essss = funcion.exp1.getType(entorno, listas, management);
                    if (esss != tipoDato.entero && essss != tipoDato.entero)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El parametro de inicio para la funcion endsWith no es de tipo cadena"));
                        return tipoDato.errorSemantico;
                    }
                    Object inicio = funcion.exp1.getValue(entorno, listas, management);
                    Object final = funcion.exp2.getValue(entorno, listas, management);
                    return Convert.ToString(auxFuncion).Substring(Int32.Parse(Convert.ToString(inicio)),
                        Int32.Parse(Convert.ToString(final)));

            }

            return tipoDato.errorSemantico;
        }
    }
}
