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
                            break;

                        case "procedure": //3
                            break;

                        default:
                            listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "Tipo de Objeto no permitido"));
                            return "ERRORSHAROLIN";
                    }
                    break;

                case "name":
                    switch (tipo)
                    {
                        case 1:
                            aux += " " + nodo.ChildNodes.ElementAt(4).Token.Text.ToLower().Replace("\"","") + " ";
                            break;

                        case 2:
                            break;

                        case 3:
                            break;
                    }
                    break;

                case "type":
                    break;

                case "pk":
                    break;

                case "attrs":
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
                            aux += "(" + devuelto + "); ";
                            break;                            

                        default:
                            break;
                    }
                    break;

                case "data":
                    break;

                case "parameters":
                    break;

                case "instr":
                    break;

                case "IMPORTAR":
                    break;

                case "erid":
                    break;
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
                return aux + "\n";
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String aux = "";
                aux += iteradorEscribeColumnas(nodo.ChildNodes.ElementAt(1));
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
            aux = aux.TrimEnd(',');
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
                        aux += " primaty key,\n";
                    }
                    else
                    {
                        aux += ",\n";
                    }
                    break;

                default:
                    listas.errores.AddLast(new NodoError(nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(4).Token.Location.Column, NodoError.tipoError.Semantico, "atributo de la columna no valido: " +
                                nodo.ChildNodes.ElementAt(1).Token.Text));
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
