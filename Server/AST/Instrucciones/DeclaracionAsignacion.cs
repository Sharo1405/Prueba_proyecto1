using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class DeclaracionAsignacion : Instruccion
    {

        public Tipo tipo { get; set; }
        public LinkedList<String> ids = new LinkedList<string>();
        public Expresion valor { get; set; }


        public DeclaracionAsignacion(Tipo tipo, LinkedList<String> ids, Expresion valor)
        {
            this.tipo = tipo;
            this.ids = ids;
            this.valor = valor;
        }


        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            foreach (String id in ids)
            {
                Simbolo buscado = entorno.getEnActual(id);
                if (buscado == null)
                {
                    tipoDato tipoValor = valor.getType(entorno, listas);
                    object value = valor.getValue(entorno, listas);
                    if (tipo.tipo == tipoValor)
                    {
                        entorno.setSimbolo(id, new Simbolo(id, value, tipo.linea, tipo.columna,
                            tipo.tipo, Simbolo.Rol.VARIABLE));
                    }
                    else
                    {
                        if (tipo.tipo == tipoDato.entero && tipoValor == tipoDato.decimall)
                        {
                            //se redondea el valor
                            double nuevo = Math.Floor(Double.Parse(Convert.ToString(value)));
                            Int32 ainsertar = Int32.Parse(Convert.ToString(nuevo));
                            entorno.setSimbolo(id, new Simbolo(id, ainsertar, tipo.linea, tipo.columna,
                            tipo.tipo, Simbolo.Rol.VARIABLE));
                        }
                        else if(tipo.tipo == tipoDato.decimall && tipoValor == tipoDato.entero)
                        {
                            entorno.setSimbolo(id, new Simbolo(id, Double.Parse(Convert.ToString(value)), tipo.linea, tipo.columna,
                            tipo.tipo, Simbolo.Rol.VARIABLE));
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(tipo.linea, tipo.columna, NodoError.tipoError.Semantico, "Los tipos no coinciden para la variable. Tipos en cuestion: " + Convert.ToString(tipo.tipo) + Convert.ToString(tipoValor)));
                        }
                    }
                    
                }
                else
                {
                    listas.errores.AddLast(new NodoError(tipo.linea, tipo.columna, NodoError.tipoError.Semantico, "Variable ya declara en el entorno actual. El nombre es: " + id));
                }
            }
            return "ok";
        }
    }
}
