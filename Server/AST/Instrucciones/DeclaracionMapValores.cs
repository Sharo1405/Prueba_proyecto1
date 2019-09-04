using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class DeclaracionMapValores : Instruccion
    {
        public LinkedList<String> idMap { get; set; }
        public Expresion ListaExpresiones { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        public DeclaracionMapValores(LinkedList<String> idMap, Expresion comas,
            int linea, int columna)
        {
            this.idMap = idMap;
            this.ListaExpresiones = comas;
            this.linea = linea;
            this.columna = columna;
        }

        tipoDato tipoClave = tipoDato.errorSemantico;
        tipoDato tipoValor = tipoDato.errorSemantico;
        tipoDato tipoClaveAnterior = tipoDato.errorSemantico;        
        tipoDato tipoValorAnterior = tipoDato.errorSemantico;
        HashSet<ClaveValor> hashParaSimbolo = new HashSet<ClaveValor>();
        int contador = 0;

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                                           
                Object dev = ListaExpresiones.getValue(entorno, listas, management);
                if (dev is LinkedList<Comas>)
                {
                    LinkedList<Comas> listaComas = (LinkedList<Comas>)dev;                    
                    foreach (Comas cv in listaComas)
                    {
                        Comas cv2 = (Comas)cv;
                        Object valor = cv2.getValue(entorno, listas, management);
                        if (valor is DosPuntos)
                        {
                            DosPuntos val = (DosPuntos)valor;
                            Object valorClave = val.getValue(entorno, listas, management);
                            if (valorClave is ClaveValor)
                            {
                                ClaveValor a = (ClaveValor)valorClave;
                                if (a.clave is Expresion && a.valor is Expresion)
                                {
                                    contador++;
                                    Expresion clave = (Expresion)a.clave;
                                    Expresion value = (Expresion)a.valor;
                                    tipoClave = clave.getType(entorno, listas, management);
                                    tipoValor = value.getType(entorno, listas, management);
                                    if (contador == 1)
                                    {
                                        tipoClaveAnterior = tipoClave;
                                        tipoValorAnterior = tipoValor;
                                        ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), 
                                            value.getValue(entorno, listas, management));
                                        hashParaSimbolo.Add(paraGuardar);
                                    }
                                    else
                                    {
                                        if ((tipoClave == tipoClaveAnterior) && (tipoValor == tipoValorAnterior))
                                        {
                                            ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), 
                                                value.getValue(entorno, listas, management));                                            
                                            hashParaSimbolo.Add(paraGuardar);
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.columna, 
                                                NodoError.tipoError.Semantico, "Las claves y valores del map no son del mismo tipo."));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                }                                
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "El valor del map no es valido"));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else if (valor is LinkedList<Comas>)
                        {
                            LinkedList<Comas> listComas = (LinkedList<Comas>)valor;
                            paraComas(listComas, entorno, listas, management);
                        }
                        else if (valor is ClaveValor)
                        {
                            ClaveValor a = (ClaveValor)valor;
                            if (a.clave is Expresion && a.valor is Expresion)
                            {
                                contador++;
                                Expresion clave = (Expresion)a.clave;
                                Expresion value = (Expresion)a.valor;
                                tipoClave = clave.getType(entorno, listas, management);
                                tipoValor = value.getType(entorno, listas, management);
                                if (contador == 1)
                                {
                                    tipoClaveAnterior = tipoClave;
                                    tipoValorAnterior = tipoValor;
                                    ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), value.getValue(entorno, listas, management));
                                    hashParaSimbolo.Add(paraGuardar);
                                }
                                else
                                {
                                    if ((tipoClave == tipoClaveAnterior) && (tipoValor == tipoValorAnterior))
                                    {
                                        ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), value.getValue(entorno, listas, management));
                                        hashParaSimbolo.Add(paraGuardar);
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                            NodoError.tipoError.Semantico, "Las claves y valores del map no son del mismo tipo."));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "El valor del map no es valido"));
                            return tipoDato.errorSemantico;
                        }
                    }

                    //si llega hasta aqui es porque todo cool entonces guardamos el map
                    foreach (String id in idMap) {
                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), hashParaSimbolo, linea, columna,
                                    tipoDato.map, tipoClaveAnterior, tipoValorAnterior, Simbolo.Rol.VARIABLE));
                    }
                }
                else if (dev is ClaveValor)
                {
                    ClaveValor a = (ClaveValor)dev;
                    if (a.clave is Expresion && a.valor is Expresion)
                    {
                        contador++;
                        Expresion clave = (Expresion)a.clave;
                        Expresion value = (Expresion)a.valor;
                        tipoClave = clave.getType(entorno, listas, management);
                        tipoValor = value.getType(entorno, listas, management);
                        if (contador == 1)
                        {
                            tipoClaveAnterior = tipoClave;
                            tipoValorAnterior = tipoValor;
                            ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), value.getValue(entorno, listas, management));
                            hashParaSimbolo.Add(paraGuardar);
                        }
                        else
                        {
                            if ((tipoClave == tipoClaveAnterior) && (tipoValor == tipoValorAnterior))
                            {
                                ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), value.getValue(entorno, listas, management));
                                hashParaSimbolo.Add(paraGuardar);
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                    NodoError.tipoError.Semantico, "Las claves y valores del map no son del mismo tipo."));
                                return tipoDato.errorSemantico;
                            }
                        }
                    }
                    foreach (String id in idMap)
                    {
                        entorno.setSimbolo(id.ToLower(), new Simbolo(id.ToLower(), hashParaSimbolo, linea, columna,
                                    tipoDato.map, tipoClaveAnterior, tipoValorAnterior, Simbolo.Rol.VARIABLE));
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "El valor del map no es valido"));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "No se puede declarar el Map con Valores"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }

        public void paraComas(LinkedList<Comas> listComas, Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            foreach (Comas cv in listComas)
            {
                Comas cv2 = (Comas)cv;
                Object valor = cv2.getValue(entorno, listas, management);
                if (valor is DosPuntos)
                {
                    DosPuntos val = (DosPuntos)valor;
                    Object valorClave = val.getValue(entorno, listas, management);
                    if (valorClave is ClaveValor)
                    {
                        ClaveValor a = (ClaveValor)valorClave;
                        if (a.clave is Expresion && a.valor is Expresion)
                        {
                            contador++;
                            Expresion clave = (Expresion)a.clave;
                            Expresion value = (Expresion)a.valor;
                            tipoClave = clave.getType(entorno, listas, management);
                            tipoValor = value.getType(entorno, listas, management);
                            if (contador == 1)
                            {
                                tipoClaveAnterior = tipoClave;
                                tipoValorAnterior = tipoValor;
                                ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), value.getValue(entorno, listas, management));
                                hashParaSimbolo.Add(paraGuardar);
                            }
                            else
                            {
                                if ((tipoClave == tipoClaveAnterior) && (tipoValor == tipoValorAnterior))
                                {
                                    ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), value.getValue(entorno, listas, management));
                                    hashParaSimbolo.Add(paraGuardar);
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                        NodoError.tipoError.Semantico, "Las claves y valores del map no son del mismo tipo."));
                                }
                            }
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "El valor del map no es valido"));
                    }
                }
                else if (valor is Comas)
                {
                    LinkedList<Comas> listaaComas = (LinkedList<Comas>)valor;
                    paraComas(listaaComas, entorno, listas, management);
                }
                else if (valor is ClaveValor)
                {
                    ClaveValor a = (ClaveValor)valor;
                    if (a.clave is Expresion && a.valor is Expresion)
                    {
                        contador++;
                        Expresion clave = (Expresion)a.clave;
                        Expresion value = (Expresion)a.valor;
                        tipoClave = clave.getType(entorno, listas, management);
                        tipoValor = value.getType(entorno, listas, management);
                        if (contador == 1)
                        {
                            tipoClaveAnterior = tipoClave;
                            tipoValorAnterior = tipoValor;
                            ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), value.getValue(entorno, listas, management));
                            hashParaSimbolo.Add(paraGuardar);
                        }
                        else
                        {
                            if ((tipoClave == tipoClaveAnterior) && (tipoValor == tipoValorAnterior))
                            {
                                ClaveValor paraGuardar = new ClaveValor(clave.getValue(entorno, listas, management), value.getValue(entorno, listas, management));
                                hashParaSimbolo.Add(paraGuardar);
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna,
                                    NodoError.tipoError.Semantico, "Las claves y valores del map no son del mismo tipo."));
                            }
                        }
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico, "El valor del map no es valido"));
                }
            }
        }
    }
}
