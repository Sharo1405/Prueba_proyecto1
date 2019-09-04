using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class Procedimientos : Expresion
    {
        public String idProc { get; set; }
        public LinkedList<Parametros> parametros = new LinkedList<Parametros>();
        public LinkedList<Parametros> retornos = new LinkedList<Parametros>();
        public StatementBlock sentencias { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public Procedimientos(String idProc, LinkedList<Parametros> parametros,
            LinkedList<Parametros> retornos, StatementBlock sentencias,
            int linea, int col)
        {
            this.idProc = idProc;
            this.parametros = parametros;
            this.retornos = retornos;
            this.sentencias = sentencias;
            this.linea = linea;
            this.columna = col;
        }


        public Procedimientos(String idProc, LinkedList<Parametros> parametros,
            StatementBlock sentencias, int linea, int col)
        {
            this.idProc = idProc;
            this.parametros = parametros;
            this.sentencias = sentencias;
            this.linea = linea;
            this.columna = col;
        }


        public Procedimientos(String idProc, LinkedList<Parametros> retornos,
            StatementBlock sentencias, int linea, int col, int nada)
        {
            this.idProc = idProc;
            this.retornos = retornos;
            this.sentencias = sentencias;
            this.linea = linea;
            this.columna = col;
        }

        public Procedimientos(String idProc, StatementBlock sentencias, int linea, int col)
        {
            this.idProc = idProc;
            this.sentencias = sentencias;
            this.linea = linea;
            this.columna = col;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            return tipoDato.list; //porque siempre va a devolver una lista
        }

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            String firmaFuncion = crearFirma();
            Simbolo buscado = entorno.get(firmaFuncion, entorno, Simbolo.Rol.FUNCION);
            if (buscado == null)
            {

                Object inUse = management.getInUse();
                if (inUse != null)
                {
                    try {

                        BaseDeDatos basee = (BaseDeDatos)inUse;
                        basee.procedures.Add(firmaFuncion, new Simbolo(firmaFuncion, sentencias, linea, columna,
                        tipoDato.list, Simbolo.Rol.PROCEDIMIENTO, parametros, retornos));
                    }
                    catch (ArgumentException e)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                    "El procedimiento ya existe en la base de datos EN USO: " + firmaFuncion));
                    }
                }

                entorno.setSimbolo(firmaFuncion, new Simbolo(firmaFuncion, sentencias, linea, columna,
                tipoDato.list, Simbolo.Rol.PROCEDIMIENTO, parametros, retornos));
            }
            else
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                    "Funcion ya esta declara. El nombre es: " + idProc));
            }

            return tipoDato.ok;
        }

        public String crearFirma()
        {
            String firma = "";
            firma += idProc;
            foreach (Parametros p in parametros)
            {
                Tipo tparametro = p.tipo;
                firma += "_" + Convert.ToString(tparametro.tipo);
            }
            return firma; ;
        }
    }
}
