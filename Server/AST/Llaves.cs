using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST
{
    class Llaves: Expresion
    {
        public Expresion expresion { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }        

        public Llaves(Expresion expresion, int linea, int columna)
        {
            this.expresion = expresion;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas)
        {
            if (expresion is ListaExpresiones)
            {
                return Operacion.tipoDato.set;
            }
            else
            {
                return expresion.getType(entorno, listas);
            }
        }


        int contador = 0;
        List<Object> listaList = new List<Object>();
        tipoDato tipoValor = tipoDato.errorSemantico;
        tipoDato tipoValorAnterior = tipoDato.errorSemantico;
        public Tipo tipoRetorno = new Tipo();


        public object getValue(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Object dev = expresion.getValue(entorno, listas);
                if (dev is LinkedList<Comas>)
                {
                    LinkedList<Comas> lista = (LinkedList<Comas>)dev;
                    foreach (Comas coma in lista)
                    {
                        contador++;
                        
                        Object resultado = coma.getValue(entorno, listas);

                        if (resultado is Auxiliar)
                        {
                            if (contador == 1)
                            {
                                tipoRetorno = new Tipo();
                            }
                            else
                            {

                            }
                        }
                        else if (resultado is LinkedList<Comas>)
                        {
                            LinkedList<Comas> listComas = (LinkedList<Comas>)resultado;
                            tipoDato dato = paraComas(listComas, entorno, listas);
                            if (dato == tipoDato.errorSemantico)
                            {
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            tipoValor = coma.getType(entorno, listas);
                            if (contador == 1)
                            {
                                tipoValorAnterior = tipoValor;
                                if (dev is Simbolo)
                                {
                                    Simbolo sim = (Simbolo)dev;
                                    listaList.Add(sim.valor);
                                }
                                else if (tipoValor == tipoDato.id)
                                {
                                    
                                }
                                else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                                {
                                   
                                }
                                else
                                {
                                    listaList.Add(resultado);
                                    tipoRetorno = new Tipo(tipoValor, linea, columna);
                                }
                            }
                            else
                            {
                                if (tipoValorAnterior == tipoValor)
                                {
                                    if (dev is Simbolo)
                                    {
                                        Simbolo sim = (Simbolo)dev;
                                        listaList.Add(sim.valor);
                                    }
                                    else if (tipoValor == tipoDato.id)
                                    {
                                        

                                    }
                                    else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                                    {
                                       
                                    }
                                    else
                                    {
                                        listaList.Add(resultado);
                                        //tipoRetorno = new Tipo(tipoValor, linea, columna);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    tipoValor = expresion.getType(entorno, listas);
                    if (contador == 1)
                    {
                        tipoValorAnterior = tipoValor;
                        if (dev is Simbolo)
                        {
                            Simbolo sim = (Simbolo)dev;
                            listaList.Add(sim.valor);
                            
                        }                        
                        else if (tipoValor == tipoDato.id)
                        {
                            

                        }
                        else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                        {
                            
                        }
                        else
                        {
                            listaList.Add(dev);
                            tipoRetorno = new Tipo(tipoValor, linea, columna);
                        }
                    }
                    else
                    {
                        if (tipoValorAnterior == tipoValor)
                        {
                            if (dev is Simbolo)
                            {
                                Simbolo sim = (Simbolo)dev;
                                listaList.Add(sim.valor);
                            }
                            else if (tipoValor == tipoDato.id)
                            {
                                
                            }
                            else if (tipoValor == tipoDato.list || tipoValor == tipoDato.set)
                            {
                                
                            }
                            else
                            {
                                listaList.Add(dev);
                            }
                        }
                    }                    
                }
                return new Auxiliar(new Tipo(tipoDato.set, tipoRetorno,linea, columna), listaList);//return listaList;
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Set"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }


        public tipoDato paraComas(LinkedList<Comas> listComas, Entorno entorno, ErrorImpresion listas)
        {
            foreach (Comas cv in listComas)
            {
                Comas cv2 = (Comas)cv;
                Object valor = cv2.getValue(entorno, listas);
                if (valor is List<object>)
                {
                    Tipo auxparanada = new Tipo();
                    auxparanada.tipo = tipoDato.ok;
                    tipoRetorno = new Tipo(tipoDato.set, auxparanada, linea, columna);
                    listaList.Add(valor);

                }
                if (valor is LinkedList<Comas>)
                {
                    LinkedList<Comas> listaaComas = (LinkedList<Comas>)valor;
                    paraComas(listaaComas, entorno, listas);
                }
                else
                {

                    if (valor is Simbolo)
                    {
                        Simbolo sim = (Simbolo)valor;
                        listaList.Add(sim.valor);
                    }
                    else
                    {
                        listaList.Add(valor);
                    }
                }                 
                }
            return tipoDato.ok;
        }            
    }
}
