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
    class DeclaraListValores : Instruccion
    {
        public LinkedList<String> idList { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public DeclaraListValores(LinkedList<String> idList, Expresion valor,
            int linea, int columna)
        {
            this.idList = idList;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        int contador = 0;
        List<Object> listaList = new List<Object>();
        tipoDato tipoValor = tipoDato.errorSemantico;
        tipoDato tipoValorAnterior = tipoDato.errorSemantico;

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                List<object> lista = new List<object>();
                Lista listaGuardar = new Lista();
                foreach (String id in idList)
                {
                    Simbolo s = entorno.getEnActual(id, Simbolo.Rol.VARIABLE);
                    if (s == null)
                    {
                        if (!(valor is Corchetes))
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "Los valores asignar a una LISTA no son validos"));
                            return tipoDato.errorSemantico;
                        }
                        Object setLista = valor.getValue(entorno, listas);
                        if (setLista is tipoDato)
                        {                            
                            return tipoDato.errorSemantico;
                        }
                        tipoDato tipo = valor.getType(entorno, listas);
                        if (setLista is List<object>)
                        {
                            lista = (List<object>)setLista;
                            listaGuardar = new Lista(id.ToLower(), lista, tipoDato.list, tipo, linea, columna);
                        }
                        else
                        {
                            lista.Add(setLista);
                            listaGuardar = new Lista(id.ToLower(), lista, tipoDato.list, tipo, linea, columna);
                        }

                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), listaGuardar, linea, columna,
                                tipoDato.list, tipo, Simbolo.Rol.VARIABLE));
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "Variable ya existe" + id));
                        return tipoDato.errorSemantico;
                    }
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el List"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }        
    }
}

