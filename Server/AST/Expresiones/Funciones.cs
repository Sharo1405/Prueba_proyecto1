using System;
using System.Collections.Generic;
using Server.AST.Entornos;
using Server.AST.Instrucciones;
using Server.AST.Otras;

namespace Server.AST.Expresiones
{
    class Funciones : Expresion
    {

        public Tipo tipo { get; set; }
        public String idFuncion { get; set; }
        public LinkedList<Parametros> parametros = new LinkedList<Parametros>();
        public StatementBlock sentencias { get; set; }

        public Funciones(Tipo tipo, String idFuncion, 
            LinkedList<Parametros> parametros, StatementBlock sentencias)
        {
            this.tipo = tipo;
            this.idFuncion = idFuncion;
            this.parametros = parametros;
            this.sentencias = sentencias;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipo.tipo;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            return "ok";
        }
    }
}
