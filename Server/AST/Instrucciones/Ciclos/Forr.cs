﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones.Ciclos
{
    class Forr : Instruccion
    {
        public NodoAST declaAsig { get; set; }
        public Expresion condicion { get; set; }
        public Expresion aumento { get; set; } //ver que onda proque el a++ no esta en el E sino en PREPOSTFIJO
        public Instruccion sentencias { get; set; }
        public int linea { get; set; }
        public int col { get; set; }

        public Forr(NodoAST declaAsig, Expresion condicion, Expresion aumento, Instruccion sentencias, int linea, int col)
        {
            this.declaAsig = declaAsig;
            this.condicion = condicion;
            this.aumento = aumento;
            this.sentencias = sentencias;
            this.linea = linea;
            this.col = col;
        }


        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Entorno actual = new Entorno(entorno);
                if (declaAsig is Instruccion) {
                    Instruccion h = (Instruccion)declaAsig;
                    h.ejecutar(actual, listas, management);
                }
                else
                {
                    Expresion h = (Expresion)declaAsig;
                    h.getValue(actual, listas, management);
                }

                object ob = condicion.getValue(actual, listas, management);
                if (ob == null)
                {
                    listas.impresiones.AddLast("WARNINGGGGGGGGGGGGGGGGGGGG!!!!!!!!!!! " +
                               " El valor es nulo " + " Linea/Columna " +
                                   Convert.ToString(this.linea) + " " + Convert.ToString(this.col));
                    return TipoExcepcion.excep.NullPointerException;
                }

                tipoDato t = condicion.getType(actual, listas, management);
                if (t == tipoDato.booleano)
                {

                    //hace la validacion del for
                    while ((Boolean)ob)
                    {
                        Boolean reiniciar = false;
                        //SE LE DEBE COPIAR EL CONTENIDO DEL ENTORNO ACTUAL AL NUEVO PARA QUE LA VARIABLE DECLARADA DEL FOR(esta) NO SE OCULTE
                        Entorno actualactual = new Entorno(actual);

                        //ejecuta sentencias
                        Object retorno = sentencias.ejecutar(actualactual, listas, management);

                        if (retorno is Breakk) {
                            return retorno;
                        } else if (retorno is Continuee){
                            continue;
                        } else if (retorno is Retorno) {
                            return retorno; 
                        } else if (retorno is TipoExcepcion.excep)
                        {
                            return retorno;
                        }
                        

                        Object valor = aumento.getValue(actualactual, listas, management);

                        ob = condicion.getValue(actual, listas, management);
                        if (ob == null)
                        {
                            listas.impresiones.AddLast("WARNINGGGGGGGGGGGGGGGGGGGG!!!!!!!!!!! " 
                                       +" Linea/Columna " +
                                   " El valor es nulo " + Convert.ToString(this.linea) + " " + Convert.ToString(this.col));
                            return TipoExcepcion.excep.NullPointerException;
                        }
                    }

                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, col, 
                        NodoError.tipoError.Semantico, "Condicion del For no valida para ejecutarse"));
                }

            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, col,
                NodoError.tipoError.Semantico, "No se puede realizar el for"));
                return tipoDato.errorSemantico;
            }

            return tipoDato.ok;
        }
    }
}
