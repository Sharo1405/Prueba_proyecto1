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
    class Asignacion : Instruccion
    {
        public String id { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        int contador = 0;

        public Asignacion(String id, Expresion valor,
            int linea, int columna)
        {
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Simbolo variable = entorno.get(id, entorno, Simbolo.Rol.VARIABLE);
                if (variable != null)
                {
                    tipoDato tipoValor = valor.getType(entorno, listas);
                    object value = valor.getValue(entorno, listas);
                    if (valor is Corchetes)
                    {
                        if (variable.tipo == tipoDato.list || variable.tipo == tipoDato.set)
                        {
                            List<Object> lista = (List<Object>)variable.valor;
                            lista.Clear();
                            if (tipoValor == variable.tipoValor)
                            {
                                lista.Add(value);
                            }
                            else
                            {
                                LinkedList<Comas> listaComas = (LinkedList<Comas>)value;
                                foreach (Comas cv in listaComas)
                                {
                                    Comas cv2 = (Comas)cv;
                                    Object objeto = cv2.getValue(entorno, listas);
                                    if (objeto is LinkedList<Comas>)
                                    {
                                        LinkedList<Comas> listComas = (LinkedList<Comas>)objeto;
                                        tipoDato dato = paraComas(listComas, entorno, listas, variable.tipoValor, lista);
                                        if (dato == tipoDato.errorSemantico)
                                        {
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    else
                                    {
                                        if (cv2.getType(entorno, listas) == variable.tipoValor)
                                        {
                                            lista.Add(objeto);
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                                               NodoError.tipoError.Semantico, "Los valores del List no son del mismo tipo."));
                                            return tipoDato.errorSemantico;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else if (valor is Llaves) //set
                    {
                        
                    }
                    else if (valor is LLaveAsTypeUser) //user types
                    {
                        CreateType ty = (CreateType)value;
                        if (variable.idTipo == ty.idType)
                        {
                            variable.valor = ty;
                        }
                    }
                    else if (valor is Neww)
                    {
                        Neww clase = (Neww)valor;
                        Tipo tipo = clase.tipoNew;
                        if (tipo.tipo == tipoDato.map && tipoDato.map == variable.tipo)
                        {
                            variable.valor = new HashSet<ClaveValor>();
                            if (tipo.listaTipos.Count == 2)
                            {
                                int contador = 0;
                                foreach (Tipo tipoclavevalor in tipo.listaTipos)
                                {
                                    contador++;
                                    if (contador == 1)
                                    {
                                        variable.tipoClave = tipoclavevalor.tipo;
                                    }
                                    else
                                    {
                                        variable.tipoValor = tipoclavevalor.tipo;
                                    }
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "La cantidad de tipos no es valida para el map"));
                            }
                        }
                        else if (tipo.tipo == variable.tipo)
                        {
                            variable.tipoValor = tipo.tipoValor.tipo;
                            variable.valor = new List<Object>();
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "Los tipos no coinciden para la variable, No se puede hacer la instancia. Tipos en cuestion: " +
                                Convert.ToString(variable.tipo) + Convert.ToString(tipo.tipo)));
                        }
                    }
                    else if (variable.tipo == tipoValor)
                    {
                        variable.valor = value;
                    }
                    else
                    {
                        if (variable.tipo == tipoDato.entero && tipoValor == tipoDato.decimall)
                        {
                            //se redondea el valor
                            double nuevo = Math.Floor(Double.Parse(Convert.ToString(value)));
                            Int32 ainsertar = Int32.Parse(Convert.ToString(nuevo));
                            variable.valor = ainsertar;
                        }
                        else if (variable.tipo == tipoDato.decimall && tipoValor == tipoDato.entero)
                        {
                            variable.valor = Double.Parse(Convert.ToString(value));
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "Los tipos no coinciden para la variable. Tipos en cuestion: " + Convert.ToString(variable.tipo) + Convert.ToString(tipoValor)));
                        }
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "La variable no existe para asignar valor: " + id));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                "La asignacion no es valida: " + id));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }       


        public tipoDato paraComas(LinkedList<Comas> listComas, Entorno entorno, ErrorImpresion listas, tipoDato tipoValorLista, List<Object> list)
        {
            foreach (Comas cv in listComas)
            {
                Comas cv2 = (Comas)cv;
                Object valor = cv2.getValue(entorno, listas);
                if (valor is Comas)
                {
                    LinkedList<Comas> listaaComas = (LinkedList<Comas>)valor;
                    paraComas(listaaComas, entorno, listas, tipoValorLista, list);
                }
                else
                {
                    tipoDato tipoValor = cv2.getType(entorno, listas);
                    if (tipoValor == tipoDato.booleano ||
                            tipoValor == tipoDato.cadena ||
                            tipoValor == tipoDato.date ||
                            tipoValor == tipoDato.decimall ||
                            tipoValor == tipoDato.entero ||
                            tipoValor == tipoDato.id ||
                            tipoValor == tipoDato.list ||
                            tipoValor == tipoDato.map ||
                            tipoValor == tipoDato.set ||
                            tipoValor == tipoDato.time)
                    {
                        
                            if (tipoValor == tipoValorLista)
                            {
                                list.Add(valor);
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                                NodoError.tipoError.Semantico, "Los valores del List no son del mismo tipo."));
                                return tipoDato.errorSemantico;
                            }
                       
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                        NodoError.tipoError.Semantico, "Valor del List no es valido"));
                        return tipoDato.errorSemantico;
                    }
                }
            }
            return tipoDato.ok;
        }
    }
}
