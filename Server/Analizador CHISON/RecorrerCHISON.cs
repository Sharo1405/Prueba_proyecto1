using Irony.Parsing;
using Server.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Analizador_CHISON
{
    class RecorrerCHISON
    {
        public ErrorImpresion listas = new ErrorImpresion();

        public String Sentencia(ParseTreeNode nodo)
        {
            String aux = "";
            String h = nodo.ChildNodes.ElementAt(0).Term.Name;
            switch (h)
            {
                case "$":
                    return RecorrerDesdeInicio(nodo.ChildNodes.ElementAt(2));
                case "USUS":
                    break;
                case "LISTAPERMISOS":
                    break;
                case "BASES":
                    break;
                case "LISTACQLTYPE":
                    break;
            }

            return aux;
        }


        public String RecorrerDesdeInicio(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                String aux = RecorrerDesdeInicio(nodo.ChildNodes.ElementAt(0));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }
                String devuelto = SentenciaInicio(nodo.ChildNodes.ElementAt(2));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
                return aux + "\n";
            }
            else 
            {
                String aux = "";
                String devuelto = SentenciaInicio(nodo.ChildNodes.ElementAt(0));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
                return aux;
            }
        }


        public String SentenciaInicio(ParseTreeNode nodo)
        {
            String aux = "";
            switch (nodo.ChildNodes.ElementAt(0).ToString())
            {
                case "BASESDEDATOS":
                    String devuelto = traducirBasesdeDatos(nodo.ChildNodes.ElementAt(0));
                    if (devuelto.Equals("ERRORSHAROLIN"))
                    {
                        return devuelto;
                    }
                    aux += devuelto;
                    break;

                case "USUARIOS":
                    aux += traducirUsuarios(nodo.ChildNodes.ElementAt(0));
                    break;
            }
            return aux;
        }


        public String traducirBasesdeDatos(ParseTreeNode nodo)
        {
            String aux = "";
            String devuelto = ListaBases(nodo.ChildNodes.ElementAt(5));
            if (devuelto.Equals("ERRORSHAROLIN"))
            {
                return devuelto;
            }
            aux += devuelto;
            return aux;
        }

        public String ListaBases(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 5)
            {
                String aux = ListaBases(nodo.ChildNodes.ElementAt(0));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }
                String devuelto = LISTABASESmetodo(nodo.ChildNodes.ElementAt(3));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto + "\n";
                return aux + "\n";
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                String devuelto = LISTABASESmetodo(nodo.ChildNodes.ElementAt(1));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto + "\n";
                return aux + "\n";
            }
            else
            {
                try
                {
                    if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("IMPORTAR"))
                    {
                        //EL IMPORTAR
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            return "";
        }

        String nombreBaseActual = "";
        public String LISTABASESmetodo(ParseTreeNode nodo)
        {
            String aux = "";
            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                String devuelto =  NoditoBases(nodito);
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto + "\n";

            }
            nombreBaseActual = "";
            return aux;
        }


        public String NoditoBases(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {
                case "name":
                    nombreBaseActual = nodo.ChildNodes.ElementAt(4).Token.Text;
                    return "create database " + nodo.ChildNodes.ElementAt(4).Token.Text + ";\n" + " use " + nodo.ChildNodes.ElementAt(4).Token.Text + ";\n";

                case "data":
                    String devuelto =  ITEMB(nodo.ChildNodes.ElementAt(5)) + "\n";
                    if (devuelto.Equals("ERRORSHAROLIN")) 
                    {
                        return devuelto;
                    }
                    return devuelto;
               
            }

            return aux;
        }


        public String ITEMB(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 5)
            {
                String aux = ITEMB(nodo.ChildNodes.ElementAt(0));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }

                String devuelto = ITEMCQLTYPE(nodo.ChildNodes.ElementAt(3));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
                return aux + "\n";
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                aux += ITEMCQLTYPE(nodo.ChildNodes.ElementAt(1));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }
                return aux + "\n";
            }
            else
            {
                try
                {
                    if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("IMPORTAR"))
                    {
                        //EL IMPORTAR
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            return "";
        }


        public String ITEMCQLTYPE(ParseTreeNode nodo)
        {
            String aux = "";
            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                String devuelto = ICQL(nodito);
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
            }
            return aux;
        }


        int tipo = -1;
        String nombreTablaActual ="";
        public String ICQL(ParseTreeNode nodo)
        {
            String aux = "";
            switch (nodo.ChildNodes.ElementAt(1).Term.Name)
            {
                case "CQL-TYPE":
                    String tipoCQL = nodo.ChildNodes.ElementAt(4).Token.Text.ToLower().Replace("\"","");
                    switch (tipoCQL)
                    {
                        case "table": //1
                            tipo = 1;
                            aux += " create table ";
                            break;

                        case "object": //2
                            tipo = 2;
                            aux += " create type ";
                            break;

                        case "procedure": //3
                            tipo = 3;
                            aux += " procedure ";
                            break;

                        default:
                            listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Tipo de Objeto no permitido " +
                                nodo.ChildNodes.ElementAt(4).Term.Name));
                            return "ERRORSHAROLIN";
                    }
                    break;

                case "name":
                    switch (tipo)
                    {
                        case 1:
                            aux += " " + nodo.ChildNodes.ElementAt(4).Token.Text.ToLower().Replace("\"","") + " ";
                            nombreTablaActual = nodo.ChildNodes.ElementAt(4).Token.Text.ToLower().Replace("\"", "");
                            break;

                        case 2:
                            aux += " " + nodo.ChildNodes.ElementAt(4).Token.Text.ToLower().Replace("\"", "") + " ";
                            break;

                        case 3:
                            aux += " " + nodo.ChildNodes.ElementAt(4).Token.Text.ToLower().Replace("\"", "") + " ";
                            break;
                    }
                    break;

                /*case "type":
                    break;

                case "pk":
                    break;*/

                case "attrs":
                    switch (tipo)
                    {                        
                        case 2:
                            String devuelto = EscribirObjetoTP(nodo.ChildNodes.ElementAt(5));
                            if (devuelto.Equals("ERRORSHAROLIN"))
                            {
                                return devuelto;
                            }
                            if (!devuelto.Equals(""))
                            {
                                devuelto = devuelto.Remove(devuelto.Length - 1, 1);
                            }
                            aux += "(" + devuelto + "); ";
                            break;

                        default:
                            listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Tipo de CQL-TYPE no permite lista " +
                                "de atributos "));
                            return "ERRORSHAROLIN";
                    }
                    break;

                case "columns":
                    switch (tipo)
                    {
                        case 1:
                            String devuelto = EscribirCOLUMNAS(nodo.ChildNodes.ElementAt(5));
                            if (devuelto.Equals("ERRORSHAROLIN"))
                            {
                                return devuelto;
                            }
                            if (!devuelto.Equals("")) {
                                devuelto = devuelto.Remove(devuelto.Length - 1, 1);
                            }
                            aux += "(" + devuelto + "); \n";
                            break;                            

                        default:
                            listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Tipo de CQL-TYPE no permite Columnas "));
                            return "ERRORSHAROLIN";
                    }
                    break;

                case "data":
                    switch (tipo)
                    {
                        case 1:
                            String devuelto = EscribirDataColumnas(nodo.ChildNodes.ElementAt(5));
                            if (devuelto.Equals("ERRORSHAROLIN"))
                            {
                                return devuelto;
                            }
                            if (!devuelto.Equals(""))
                            {
                                devuelto = devuelto.Remove(devuelto.Length - 1, 1);
                            }                            
                            aux +=  devuelto + "\n";
                            break;

                        default:
                            listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                               nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Tipo de CQL-TYPE no permite Columnas "));
                            return "ERRORSHAROLIN";
                    }
                    break;

                case "parameters":
                    switch (tipo)
                    {
                        case 3:
                            String devuelto = EscribirProcedimientoPARAMETROS(nodo.ChildNodes.ElementAt(5));
                            if (devuelto.Equals("ERRORSHAROLIN"))
                            {
                                return devuelto;
                            }
                            aux += CadenaParametros();
                            aux += devuelto + "\n";
                            parametrosIN.Clear();
                            parametrosIN = new List<string>();

                            parametrosOUT.Clear();
                            parametrosOUT = new List<string>();
                            break;

                        default:
                            listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Tipo de CQL-TYPE no permite Parametros "));
                            return "ERRORSHAROLIN";
                    }
                    break; 

                case "instr":
                    switch (tipo)
                    {
                        case 3:
                            String devuelto = EscribirProcedimientoINSTRUCCIONES(nodo);
                            if (devuelto.Equals("ERRORSHAROLIN"))
                            {
                                return devuelto;
                            }
                            if (!devuelto.Equals(""))
                            {
                                devuelto = devuelto.Remove(devuelto.Length - 1, 1);
                                devuelto = devuelto.Remove(0,1);
                            }
                            aux += "{ \n"+ devuelto + "} \n";
                            break;

                        default:
                            listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Tipo de CQL-TYPE no permite Parametros "));
                            return "ERRORSHAROLIN";
                    }
                    break;

                case "IMPORTAR":
                    break;

                /*case "erid":
                    break;*/
            }
            return aux;
        }

        public String EscribirProcedimientoINSTRUCCIONES(ParseTreeNode nodo)
        {
            String aux = "";

            String ss = nodo.ChildNodes.ElementAt(4).Term.Name;
            switch (ss)
            {
                case "cadenaProcedimiento":
                    aux += nodo.ChildNodes.ElementAt(4).Token.Text;
                    break;

                case "IMPORTAR":
                    //EL IMPORTAR
                    break;
            }

            return aux;
        }


        public String CadenaParametros()
        {
            String aux = "";
            int cant = parametrosIN.Count;
            aux += "( ";
            foreach (String cadena in parametrosIN)
            {
                aux += cadena + ",";
            }
            aux = aux.Remove( aux.Length - 1, 1);
            aux += " ) , ( ";
            int cantOut = parametrosOUT.Count;
            foreach (String cade in parametrosOUT)
            {
                aux += cade + ",";
            }
            aux = aux.Remove(aux.Length - 1, 1);
            aux += " )";
            return aux;
        }


        public String EscribirProcedimientoPARAMETROS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 5)
            {
                String aux = EscribirProcedimientoPARAMETROS(nodo.ChildNodes.ElementAt(0));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }
                String devuelto = IteradorParamatros(nodo.ChildNodes.ElementAt(3));                
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                //aux += CadenaParametros();
                aux += devuelto;
                return aux;
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                String devuelto = IteradorParamatros(nodo.ChildNodes.ElementAt(1));                
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                //aux += CadenaParametros();
                aux += devuelto ;
                return aux;
            }
            else
            {
                try
                {
                    if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("IMPORTAR"))
                    {
                        //EL IMPORTAR
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            return "";
        }



        List<String> parametrosIN = new List<string>();
        List<String> parametrosOUT = new List<string>();
        public String IteradorParamatros(ParseTreeNode nodo)
        {
            String aux = "";
            String nombre = "";
            String typo = "";
            String ass = "";
            int contador = 0;
            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                //TipoIDParametro p = new TipoIDParametro();

                if (nodito.ChildNodes.ElementAt(1).Term.Name.Equals("name"))
                {
                    nombre = ParametrosFinalNOMBRE(nodito);
                }
                else if (nodito.ChildNodes.ElementAt(1).Term.Name.Equals("type"))
                {
                    typo = ParametrosFinalTIPO(nodito);
                }
                else if (nodito.ChildNodes.ElementAt(1).Term.Name.Equals("as"))
                {
                    ass = ParametrosFinalINOUT(nodito);
                }
                else
                {
                    listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(contador).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, 
                                "Atributo no valido para un parametro de un procedimiento"));
                    return "ERRORSHAROLIN";
                }
                contador++;
            }

            if (ass.ToLower().Equals("in"))
            {
                parametrosIN.Add(typo+ " " + nombre);
            }
            else if (ass.ToLower().Equals("out"))
            {
                parametrosOUT.Add(typo + " " + nombre);
            }
            return aux;
        }


        public String ParametrosFinalINOUT(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {                

                case "as":
                    String sss = nodo.ChildNodes.ElementAt(4).Term.Name;
                    if (sss.Equals("in"))
                    {
                        return "in";
                    }
                    else if (sss.Equals("out"))
                    {
                        return "out";
                    }
                    break;
            }
            return aux;
        }

        public String ParametrosFinalTIPO(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {                            
                case "type":
                    aux += " " + nodo.ChildNodes.ElementAt(4).Token.Text.ToLower().Replace("\"", "") + " ";
                    break;               
            }
            return aux;
        }

        public String ParametrosFinalNOMBRE(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {
                case "name":
                    aux += " " + nodo.ChildNodes.ElementAt(4).Token.Text.ToLower().Replace("\"", "") + " ";
                    break;
            }
            return aux;
        }



        public String EscribirDataColumnas(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 5)
            {
                String aux = EscribirDataColumnas(nodo.ChildNodes.ElementAt(0));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }

                aux += " insert into " + nombreTablaActual + " values( ";
                String devuelto = IteradorDataColumnas(nodo.ChildNodes.ElementAt(3));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                if (!devuelto.Equals(""))
                {
                    devuelto = devuelto.Remove(devuelto.Length - 1, 1);
                }
                aux += devuelto + ");\n";
                return aux;
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                aux += "insert into " + nombreTablaActual + " values( ";
                String devuelto = IteradorDataColumnas(nodo.ChildNodes.ElementAt(1));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                if (!devuelto.Equals(""))
                {
                    devuelto = devuelto.Remove(devuelto.Length -1, 1);
                }
                aux += devuelto + ");\n";
                return aux;
            }
            else
            {
                try
                {
                    if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("IMPORTAR"))
                    {
                        //EL IMPORTAR
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            return "";
        }


        public String IteradorDataColumnas(ParseTreeNode nodo)
        {
            String aux = "";
            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                String devuelto = DataColumnFinal(nodito);
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
            }
            return aux;
        }


        public String DataColumnFinal(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {
                case "erid":
                    aux += Valor(nodo.ChildNodes.ElementAt(4)) + " ,";
                    break;

                default:
                    listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Atributo no valido para un la data de " +
                                "una columna: " +
                                nodo.ChildNodes.ElementAt(1).Token.Text));
                    return "ERRORSHAROLIN";
            }
            return aux;
        }


        public String Valor(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(0).Term.Name;
            switch (ss)
            {
                case "tstring":
                    aux += nodo.ChildNodes.ElementAt(0).Token.Text;
                    break;

                case "numero":
                    aux += nodo.ChildNodes.ElementAt(0).Token.Text;
                    break;

                case "datetime":
                    aux += nodo.ChildNodes.ElementAt(0).Token.Text;
                    break;

                case "falsee":
                    aux += nodo.ChildNodes.ElementAt(0).Token.Text;
                    break;

                case "true":
                    aux += nodo.ChildNodes.ElementAt(0).Token.Text;
                    break;

                case "[":
                    aux += Lista(nodo.ChildNodes.ElementAt(1));
                    break;

                case "{":
                    aux += Set(nodo.ChildNodes.ElementAt(1));
                    break;

                case "DATABASES":
                    aux += "{ " + OBJETOS(nodo.ChildNodes.ElementAt(1)) + " } as ";  //aqui falta el tipo del objeto ver que onda;
                    break;
            }

            return aux;
        }


        public String OBJETOS(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {
                case "ITEMCQLTYPE":
                    aux += ContenidoObjetoDJAKSD(nodo.ChildNodes.ElementAt(1));
                    break;

                default:
                    listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, 
                        "Atributo no valido para un objeto"));
                    return "ERRORSHAROLIN";

            }
            return aux;
        }


        public String ContenidoObjetoDJAKSD(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 5)
            {
                String aux = ContenidoObjetoDJAKSD(nodo.ChildNodes.ElementAt(0));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }

                String devuelto = iteradorDeObjeto(nodo.ChildNodes.ElementAt(3));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                /*if (!devuelto.Equals(""))
                {
                    devuelto = devuelto.Remove(devuelto.Length - 1, 1);
                }*/
                return aux;
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                String devuelto = iteradorDeObjeto(nodo.ChildNodes.ElementAt(1));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                if (!devuelto.Equals(""))
                {
                    devuelto = devuelto.Remove(devuelto.Length - 1, 1);
                }
                return aux;
            }
            else
            {
                try
                {
                    if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("IMPORTAR"))
                    {
                        //EL IMPORTAR
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            return "";
        }



        public String iteradorDeObjeto(ParseTreeNode nodo)
        {
            String aux = "";
            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                String devuelto = DataColumnFinal(nodito);
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
            }
            return aux;
        }


        public String Set(ParseTreeNode nodo)
        {
            String aux = "";
            aux += "{ ";
            foreach (ParseTreeNode nod in nodo.ChildNodes)
            {
                aux += Valor(nod) + " ,";
            }
            aux = aux.Remove(aux.Length - 1, 1);
            aux += " }";
            return aux;
        }


        public String Lista(ParseTreeNode nodo)
        {
            String aux = "";
            aux += "[ ";
            foreach (ParseTreeNode nod in nodo.ChildNodes)
            {
                aux += Valor(nod) + " ,";
            }
            aux = aux.Remove(aux.Length - 1, 1);
            aux += " ]";
            return aux;
        }



        public String EscribirObjetoTP(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 5)
            {
                String aux = EscribirObjetoTP(nodo.ChildNodes.ElementAt(0));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }

                String devuelto = iteradorEscribeAtributos(nodo.ChildNodes.ElementAt(3));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
                return aux;
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                aux += iteradorEscribeAtributos(nodo.ChildNodes.ElementAt(1));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }
                return aux;
            }
            else
            {
                try
                {
                    if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("IMPORTAR"))
                    {
                        //EL IMPORTAR
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            return "";
        }


        public String iteradorEscribeAtributos(ParseTreeNode nodo)
        {
            String aux = "";
            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                String devuelto = AtributosEncabezados(nodito);
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
            }
            //aux = aux.TrimEnd(',');
            return aux;
        }


        public String AtributosEncabezados(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {
                case "name":
                    aux += nodo.ChildNodes.ElementAt(4).Token.Text.Replace("\"", "") + " ";
                    break;

                case "type":
                    aux += nodo.ChildNodes.ElementAt(4).Token.Text.Replace("\"", "") + ",";
                    break;

                default:
                    listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Atributo no valido para un item de Objeto User Type: " +
                                nodo.ChildNodes.ElementAt(1).Token.Text));
                    return "ERRORSHAROLIN";
            }

            return aux;
        }


        public String EscribirCOLUMNAS(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 5)
            {
                String aux = EscribirCOLUMNAS(nodo.ChildNodes.ElementAt(0));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }

                String devuelto = iteradorEscribeColumnas(nodo.ChildNodes.ElementAt(3));
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
                return aux;
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                aux += iteradorEscribeColumnas(nodo.ChildNodes.ElementAt(1));
                if (aux.Equals("ERRORSHAROLIN"))
                {
                    return aux;
                }
                return aux;
            }
            else
            {
                try
                {
                    if (nodo.ChildNodes.ElementAt(0).Term.Name.Equals("IMPORTAR"))
                    {
                        //EL IMPORTAR
                    }
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            return "";
        }


        public String iteradorEscribeColumnas(ParseTreeNode nodo)
        {
            String aux = "";
            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                String devuelto = COLUMNSENCABEZADOS(nodito);
                if (devuelto.Equals("ERRORSHAROLIN"))
                {
                    return devuelto;
                }
                aux += devuelto;
            }
            //aux = aux.TrimEnd(',');
            return aux;
        }


        public String COLUMNSENCABEZADOS(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {
                case "name":
                    aux += nodo.ChildNodes.ElementAt(4).Token.Text.Replace("\"","") + " ";
                    break;

                case "type":
                    aux += nodo.ChildNodes.ElementAt(4).Token.Text.Replace("\"", "") + " ";
                    break;

                case "pk":
                    String bo = nodo.ChildNodes.ElementAt(4).Token.Text;
                    if (Convert.ToBoolean(bo))
                    {
                        aux += " primary key,";
                    }
                    else
                    {
                        aux += ",";
                    }
                    break;

                default:
                    listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "atributo de la columna no valido: " +
                                nodo.ChildNodes.ElementAt(1).Term.Name));
                    return "ERRORSHAROLIN";
            }

            return aux;
        }

      

        //USUARIO--------------------------------------------------------------------------------------------------------------------
        public String traducirUsuarios(ParseTreeNode nodo)
        {
            String aux = "";
            aux += ListaUsuarios(nodo.ChildNodes.ElementAt(5));
            return aux;
        }

        public String ListaUsuarios(ParseTreeNode nodo)
        {

            if (nodo.ChildNodes.Count == 5)
            {
                String aux = ListaUsuarios(nodo.ChildNodes.ElementAt(0));
                aux += LISTAITEMUSU(nodo.ChildNodes.ElementAt(3));
                return aux + "\n";
            }
            else if(nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                aux += LISTAITEMUSU(nodo.ChildNodes.ElementAt(1));
                return aux + "\n";
            }
            else
            {
                return "";
            }
        }

        String nombreUsuarioActual = "";
        public String LISTAITEMUSU(ParseTreeNode nodo)
        {
            String aux = "";
            
            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                aux += Nodito(nodito);
            }
            nombreUsuarioActual = "";
            return aux;
        }


        public String Nodito(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.ChildNodes.ElementAt(1).Term.Name;
            switch (ss)
            {
                case "name":
                    nombreUsuarioActual = nodo.ChildNodes.ElementAt(4).Token.Text;
                    return "CREATE USER " + nodo.ChildNodes.ElementAt(4).Token.Text + " ";

                case "password":
                    return "WITH PASSWORD \"" + nodo.ChildNodes.ElementAt(4).Token.Text + "\";\n";

                case "permissions":
                    return Permisos(nodo.ChildNodes.ElementAt(5));
            }

            return aux;
        }

        public String Permisos(ParseTreeNode nodo)
        {
            String aux = "";
            String ss = nodo.Term.Name;
            switch (ss)
            {
                case "LISTAPERMISOS":
                    aux += LISTAPERMISOSmetodo(nodo);
                    break;

                case "]":
                    break;
            }
            return aux;
        }


        public String LISTAPERMISOSmetodo(ParseTreeNode nodo)
        {
            String aux = "";

            foreach (ParseTreeNode nodito in nodo.ChildNodes)
            {
                aux += NoditoPermisos(nodito);
            }

            return aux;
        }

        public String NoditoPermisos(ParseTreeNode nodo)
        {
            return "GRANT " + nombreUsuarioActual + " on " + nodo.ChildNodes.ElementAt(5) + " ;";
        }
    }
}
