using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class AccesoIdsConASignacion : Instruccion
    {
        public String variable { get; set; }
        public LinkedList<String> accesos { get; set; }
        public Expresion valor { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public AccesoIdsConASignacion(String variable, LinkedList<String> accesos,
            Expresion valor, int linea, int columna)
        {
            this.variable = variable;
            this.accesos = accesos;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas)
        {
            try
            {
                Simbolo sim = entorno.get(variable.ToLower(), entorno, Simbolo.Rol.VARIABLE);

                int contador = 0;
                if (sim != null)
                {
                    if (sim.tipo == tipoDato.id)
                    {
                        CreateType type = (CreateType)sim.valor;                        
                        String id = accesos.ElementAt(contador).ToLower();
                        foreach (itemType itType in type.itemTypee)
                        {
                            if (itType.id.Equals(id))
                            {
                                if (itType.tipo.tipo == tipoDato.id)
                                {
                                    if ((valor is Neww) && (contador == (accesos.Count -1)))
                                    {
                                        Simbolo sim2 = entorno.get(itType.tipo.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                        if (sim2 != null)
                                        {
                                            if (sim2.valor is CreateType)
                                            {                                                
                                                CreateType ss = (CreateType)sim2.valor;
                                                LinkedList<itemType> itemTy2 = new LinkedList<itemType>();
                                                foreach (itemType zz in ss.itemTypee)
                                                {
                                                    itemTy2.AddLast((itemType)zz.Clone());
                                                }
                                                CreateType lista2 = (CreateType)ss.Clone(itemTy2);
                                                itType.valor = lista2;
                                            }
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "El tipo de la variable del User type NO EXISTE. Tipos en cuestion: " + Convert.ToString(itType.tipo.id)));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    else
                                    {
                                        CreateType otroitem = (CreateType)itType.valor;
                                        if (otroitem != null) {
                                            contador++;
                                            tipoDato tipoDevuelto = tipoType(entorno, listas, otroitem, contador);
                                            if (tipoDevuelto == tipoDato.errorSemantico)
                                            {
                                                return tipoDato.errorSemantico;
                                            }
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "La variable \"" + variable+ "." + id + "\" no esta Instanciada"));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    break;
                                }
                                else if (itType.tipo.tipo == tipoDato.list)
                                {

                                }
                                else if (itType.tipo.tipo == tipoDato.set)
                                {
                                    
                                }
                                else
                                {
                                    tipoDato tipoValue = valor.getType(entorno, listas);
                                    if (tipoValue == itType.tipo.tipo)
                                    {
                                        itType.valor = valor.getValue(entorno, listas);
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "Tipos no validos para asignar el valor en el Acesso de la variable: " + variable));
                                        return tipoDato.errorSemantico;
                                    }
                                    break;
                                }
                            }
                        }

                    }
                    else if (sim.tipo == tipoDato.list)
                    {

                    }
                    else if (sim.tipo == tipoDato.set)
                    {

                    }
                    else
                    {
                        //tipo primitivo normal
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                        "La variable \"" + variable + "\" NO EXISTE no se puede asignar valor"));
                    return tipoDato.errorSemantico;
                }                

            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                "Asignacion no valida"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.id;
        }


        public tipoDato tipoType(Entorno entorno, ErrorImpresion listas, CreateType claseType, int contador)
        {
            
            String id = accesos.ElementAt(contador).ToLower();
            foreach (itemType itType in claseType.itemTypee)
            {
                if (itType.id.Equals(id))
                {
                    if (itType.tipo.tipo == tipoDato.id)
                    {
                        if ((valor is Neww) && (contador == (accesos.Count - 1)))
                        {
                            Simbolo sim2 = entorno.get(itType.tipo.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                            if (sim2 != null)
                            {
                                if (sim2.valor is CreateType)
                                {
                                    CreateType ss = (CreateType)sim2.valor;
                                    LinkedList<itemType> itemTy2 = new LinkedList<itemType>();
                                    foreach (itemType zz in ss.itemTypee)
                                    {
                                        itemTy2.AddLast((itemType)zz.Clone());
                                    }
                                    CreateType lista2 = (CreateType)ss.Clone(itemTy2);
                                    itType.valor = lista2;
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO EXISTE. Tipos en cuestion: " + Convert.ToString(itType.tipo.id)));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            CreateType otroitem = (CreateType)itType.valor;
                            if (otroitem != null)
                            {
                                contador++;
                                tipoDato tipoDevuelto = tipoType(entorno, listas, otroitem, contador);
                                if (tipoDevuelto == tipoDato.errorSemantico)
                                {
                                    return tipoDato.errorSemantico;
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "La variable \"" + variable + "." + id + "\" no esta Instanciada"));
                                return tipoDato.errorSemantico;
                            }
                        }
                        break;
                    }
                    else if (itType.tipo.tipo == tipoDato.list)
                    {

                    }
                    else if (itType.tipo.tipo == tipoDato.set)
                    {

                    }
                    else
                    {
                        tipoDato tipoValue = valor.getType(entorno, listas);
                        if (tipoValue == itType.tipo.tipo)
                        {
                            itType.valor = valor.getValue(entorno, listas);
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "Tipos no validos para asignar el valor en el Acesso de la variable: " + variable));
                            return tipoDato.errorSemantico;
                        }
                        break;
                    }
                }
            }
            return tipoDato.ok;
        }
    }
}
