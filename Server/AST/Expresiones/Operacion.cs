using Server.AST.Entornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Expresiones
{
    class Operacion
    {
        public int linea { get; set; }
        public int columna { get; set; }
        public Expresion expresion1 { get; set; }
        public Expresion expresion2 { get; set; }
        public String operador { get; set; }
        public int cantExp { get; set; }

        public enum tipoDato
        {
            cadena,
            entero,
            decimall,
            booleano,
            date,
            time,
            nulo,
            errorSemantico,
            map,
            set,
            list,
            counter,
            id,
            ok //solo para validar que todo esa cool
        }

        public Operacion(int linea, int columna, Expresion expresion1, Expresion expresion2)
        {
            this.linea = linea;
            this.columna = columna;
            this.expresion1 = expresion1;
            this.expresion2 = expresion2;
        }

        public Operacion(int linea, int columna, Expresion expresion1)
        {
            //para las de 1
            this.linea = linea;
            this.columna = columna;
            this.expresion1 = expresion1;
            
        }

        //solo para aritmeticas
        public tipoDato tipoResultante(tipoDato izquierda, tipoDato derecha, Entorno lista, ErrorImpresion impresion)
        {
            if (izquierda == tipoDato.nulo || derecha == tipoDato.nulo)
            {
                return tipoDato.errorSemantico;
            }
            else if (izquierda == tipoDato.cadena || derecha == tipoDato.cadena)
            {
                return tipoDato.cadena;

            }
            else if ((izquierda == tipoDato.decimall && derecha == tipoDato.entero) || (izquierda == tipoDato.entero && derecha == tipoDato.decimall)
                || (izquierda == tipoDato.decimall && derecha == tipoDato.decimall))
            {
                return tipoDato.decimall;
            }
            else if (izquierda == tipoDato.entero && derecha == tipoDato.entero)
            {
                return tipoDato.entero;
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }

        //para relacionales 
        public tipoDato tipoResultanteRELACIONALES(tipoDato izquierda, tipoDato derecha, Entorno lista, ErrorImpresion impresion)
        {
            if (izquierda == tipoDato.nulo || derecha == tipoDato.nulo)
            {
                return tipoDato.nulo;
            }
            else if ((izquierda == tipoDato.decimall && derecha == tipoDato.entero) || (izquierda == tipoDato.entero && derecha == tipoDato.decimall)
                || (izquierda == tipoDato.decimall && derecha == tipoDato.decimall) || (izquierda == tipoDato.entero && derecha == tipoDato.entero))
            {
                return tipoDato.decimall;
            }
            else if (izquierda == tipoDato.cadena && derecha == tipoDato.cadena)
            {
                return tipoDato.cadena;
            }
            else if (izquierda == tipoDato.date && derecha == tipoDato.date)
            {
                return tipoDato.date;
            }
            else if (izquierda == tipoDato.time && derecha == tipoDato.time)
            {
                return tipoDato.time;
            }
            else
            {
                return tipoDato.errorSemantico;
            }
        }
    }
}
