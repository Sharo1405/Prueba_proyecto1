using System;
using System.Collections.Generic;
using Server.AST.Entornos;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class Funciones : Expresion
    {

        public Tipo tipo { get; set; }
        public String idFuncion { get; set; }
        public LinkedList<Parametros> parametros = new LinkedList<Parametros>();
        public StatementBlock sentencias { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        public Funciones(Tipo tipo, String idFuncion, 
            LinkedList<Parametros> parametros, StatementBlock sentencias,
            int linea, int columna)
        {
            this.tipo = tipo;
            this.idFuncion = idFuncion.ToLower();
            this.parametros = parametros;
            this.sentencias = sentencias;
            this.linea = linea;
            this.columna = columna;
        }


        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            return tipo.tipo;
        }

        public object getValue(Entorno entorno, ErrorImpresion listas)
        {

            //guardar la funcion en TS
            String firmaFuncion = crearFirma();
            Simbolo buscado = entorno.get(firmaFuncion, entorno, Simbolo.Rol.FUNCION);
            if (buscado == null)
            {
                
                entorno.setSimbolo(firmaFuncion, new Simbolo(firmaFuncion, sentencias, linea, columna, 
                    this.getType(entorno, listas), Simbolo.Rol.FUNCION, parametros));
            }
            else
            {
                listas.errores.AddLast(new NodoError(tipo.linea, tipo.columna, NodoError.tipoError.Semantico,
                    "Funcion ya esta declara. El nombre es: " + idFuncion ));
            }

                return tipoDato.ok;
        }


        public String crearFirma()
        {
            String firma = "";
            firma += idFuncion;
            foreach (Parametros p in parametros)
            {
                Tipo tparametro = p.tipo;
                firma += "_"+Convert.ToString(tparametro.tipo);
            }
            return firma; ;
        }
    }
}
