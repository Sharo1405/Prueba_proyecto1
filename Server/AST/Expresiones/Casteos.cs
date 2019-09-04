using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class Casteos : Expresion
    {
        public String tipoCasteo { get; set; }
        public Expresion expParaCastear { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        Boolean seCasteo = false;
        tipoDato nuevotipo;

        public Casteos(String tipoCasteo, Expresion expParaCastear,
            int linea, int columna)
        {
            this.tipoCasteo = tipoCasteo;
            this.expParaCastear = expParaCastear;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            if (seCasteo)
            {
                return nuevotipo;
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                object valor = expParaCastear.getValue(entorno, listas, management);
                tipoDato tipoValor = expParaCastear.getType(entorno, listas, management);

                if (tipoValor == tipoDato.counter || 
                    tipoValor == tipoDato.id ||
                    tipoValor == tipoDato.list ||
                    tipoValor == tipoDato.map ||
                    tipoValor == tipoDato.neww ||
                    tipoValor == tipoDato.nulo ||
                    tipoValor == tipoDato.set ||
                    tipoValor == tipoDato.ok)
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                        "El tipo no es primitivo por lo que no se puede Castear"));
                    return tipoDato.errorSemantico;
                }

                switch (tipoCasteo)
                {
                    case "string":
                        nuevotipo = tipoDato.cadena;
                        seCasteo = true;
                        return Convert.ToString(valor);

                    case "time":
                        try
                        {                            
                            DateTime.Parse(Convert.ToString(valor));
                            nuevotipo = tipoDato.time;
                            seCasteo = true;
                            return DateTime.Parse(Convert.ToString(valor));
                        }
                        catch (InvalidCastException e)
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                    " Error en el casteo de TIME NO se pudo realizar "));
                            return tipoDato.errorSemantico;
                        }
                        break;


                    case "date":
                        try
                        {
                            DateTime.Parse(Convert.ToString(valor));
                            nuevotipo = tipoDato.date;
                            seCasteo = true;
                            return DateTime.Parse(Convert.ToString(valor));
                        }
                        catch (InvalidCastException e)
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                    " Error en el casteo de DATE NO se pudo realizar "));
                            return tipoDato.errorSemantico;
                        }
                        break;

                    case "int":
                        try
                        {
                            Convert.ToInt32(valor);
                            nuevotipo = tipoDato.entero;
                            seCasteo = true;
                            return Convert.ToInt32(valor);
                        }
                        catch (InvalidCastException e)
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                    " Error en el casteo de INT NO se pudo realizar "));
                            return tipoDato.errorSemantico;
                        }
                        break;

                    case "double":
                        try
                        {
                            Convert.ToDouble(valor);
                            nuevotipo = tipoDato.decimall;
                            seCasteo = true;
                            return Convert.ToDouble(valor);
                        }
                        catch (InvalidCastException e)
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                    " Error en el casteo de INT NO se pudo realizar "));
                            return tipoDato.errorSemantico;
                        }
                        break;

                }

            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                " Error en el casteo NO se pudo realizar "));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }
    }
}
