using Irony.Parsing;
using Server.AST;
using Server.AST.Ciclos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.Aritmeticas;
using Server.AST.Expresiones.Logicas;
using Server.AST.Expresiones.Relacionales;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server.AST.Expresiones.Operacion;

namespace Server.GenerarAST
{
    class RecorridoCuerpo
    {

        //LinkedList<NodoAST> lista = new LinkedList<NodoAST>();

        public LinkedList<NodoAST> Inicio(ParseTreeNode nodo)
        {

            if (nodo.ChildNodes.Count == 2)
            {
                
                    LinkedList<NodoAST> nodoAST = Inicio(nodo.ChildNodes.ElementAt(0));
                    nodoAST.AddLast(Sentencia(nodo.ChildNodes.ElementAt(1)));
                    return nodoAST;
                
            }
            else if(nodo.ChildNodes.Count == 1)
            {
                LinkedList<NodoAST> nodoAST = new LinkedList<NodoAST>();
                nodoAST.AddLast(Sentencia(nodo.ChildNodes.ElementAt(0)));
                return nodoAST;
            }
            else
            {
                return new LinkedList<NodoAST>();
            }

        }

        public StatementBlock bloque(ParseTreeNode nodo)
        {
            //aqui debo meter el bloque { listasentencias } para que
            // se ejecute en la clase bloque
            LinkedList<NodoAST> nodoAST = new LinkedList<NodoAST>();
            nodoAST = Inicio(nodo.ChildNodes.ElementAt(1));
            if (nodoAST == null)
            {
                return new StatementBlock( new LinkedList<NodoAST>());
            }
            else
            {
                return new StatementBlock(nodoAST);
            }            
        }


            public NodoAST Sentencia(ParseTreeNode nodo)
        {
                switch (nodo.ChildNodes.ElementAt(0).ToString())
                {
                    /*case "SENTENCIA":
                        NodoAST nodoast = Sentencia(nodo.ChildNodes.ElementAt(0));
                        return nodoast;

                    case "STATEMENTBLOCK":
                        NodoAST nnn = Sentencia(nodo.ChildNodes.ElementAt(1));
                        return nnn;*/

                    case "ACCESOASIGNACION":
                        break;

                    case "CREATETYPE":
                        break;

                    case "DECLARATODO":
                        return new Declarcion(Tipos(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0)) , 
                        ListaVariables(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1)));

                    case "DECLAASGINACION":
                    return new DeclaracionAsignacion(Tipos(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0)), 
                        ListaVariables(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1)), 
                        expresiones(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3)));

                    case "ALTERTYPE":
                        break;

                    case "ELIMINARUSERTYPE":
                        break;

                    case "CREARDATABASE":
                        break;

                    case "USEE":
                        break;

                    case " DROPDATABASE":
                        break;

                    case "CREARTABLA":
                        break;

                    case "ALTERTABLE":
                        break;

                    case "DROPTABLE":
                        break;

                    case "TRUNCATEE":
                        break;

                    case "COMMITT":
                        break;

                    case "ROLLBACKK":
                        break;

                    case "CREATEUSER":
                        break;

                    case "GRANT":
                        break;

                    case "REVOKEE":
                        break;

                    case "TODOCONSULTAS":
                        break;

                    case "BATCHH":
                        break;

                    case "FUNCIONAGREGACION":
                        break;

                    case "IFSTATEMENT":
                        break;

                    case "SWITCHSTATEMENT":
                        break;

                    case "WHILEE":
                        return whilee(nodo.ChildNodes.ElementAt(0));

                    case "DOWHILE":
                        break;

                    case "AUMENTOSSOLOS":
                        break;

                    case "FORR":
                        break;

                    case "MAPCOLLECTIONS":
                        break;

                    case "LISTCOLLECTIONS":
                        break;

                    case "SETCOLLECTIONS":
                        break;

                    case "FUNCIONESMETODOS":
                    return FuncionesMetodos(nodo.ChildNodes.ElementAt(0));

                    case "PROCEDIMIENTOS":
                        break;

                    case "SENTENCIATRANSFERENCIA":
                        break;

                    case "CURSORES":
                        break;

                    case "FOREACH_ACCESOCURSOR":
                        break;

                    case "LOG":
                        return Log(nodo.ChildNodes.ElementAt(0));

                case "EXCEPCIONES":
                        break;

                    case "TRYCATCHH":
                        break;
                }
            return null;
        }


        public Funciones FuncionesMetodos(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 6)
            {
                return new Funciones(Tipos(nodo.ChildNodes.ElementAt(0)),
                    nodo.ChildNodes.ElementAt(1).Token.Text,
                    Parametros(nodo.ChildNodes.ElementAt(3)),
                    bloque(nodo.ChildNodes.ElementAt(5)));
            }
            else if (nodo.ChildNodes.Count == 5)
            {
                return new Funciones(Tipos(nodo.ChildNodes.ElementAt(0)),
                    nodo.ChildNodes.ElementAt(1).Token.Text,
                    new LinkedList<Parametros>(),
                    bloque(nodo.ChildNodes.ElementAt(4)));
            }
            else if (nodo.ChildNodes.Count == 4) //llamada con parametros
            {

            }
            else if (nodo.ChildNodes.Count == 3) //llamada sin parametros
            {

            }
            return null;
        }

        public LinkedList<Parametros> Parametros(ParseTreeNode nodo)
        {
            if(nodo.ChildNodes.Count == 5)
            {

                LinkedList<Parametros> nodoAST = Parametros(nodo.ChildNodes.ElementAt(0));
                nodoAST.AddLast(new Parametros(Tipos(nodo.ChildNodes.ElementAt(2)),
                    nodo.ChildNodes.ElementAt(4).Token.Text,
                    nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(4).Token.Location.Column));
                return nodoAST;

            }
            else
            {
                LinkedList<Parametros> nodoAST = new LinkedList<Parametros>();
                nodoAST.AddLast(new Parametros(Tipos(nodo.ChildNodes.ElementAt(0)), 
                    nodo.ChildNodes.ElementAt(2).Token.Text,
                    nodo.ChildNodes.ElementAt(2).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(2).Token.Location.Column));
                return nodoAST;
            }
        }

        public LinkedList<String> ListaVariables(ParseTreeNode nodo)
        {
            LinkedList<String> variables = new LinkedList<string>();
            foreach (ParseTreeNode item in nodo.ChildNodes)
            {
                variables.AddLast(variable(item));
            }
            return variables;
        }

        public String variable(ParseTreeNode nodo)
        {
            return "@" + nodo.ChildNodes.ElementAt(1).Token.Text;
        }

        public Tipo Tipos(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 1) {
                String t = nodo.ChildNodes.ElementAt(0).ToString().Replace("Keyboard", "").Trim();

                switch (t)
                {
                    case "TIPOSPRIMITIVOS":
                        return Primitivos(nodo.ChildNodes.ElementAt(0));

                    case "counter":
                        return new Tipo(tipoDato.counter, nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "map":
                        return new Tipo(tipoDato.map, nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "set":
                        return new Tipo(tipoDato.set, nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "list":
                        return new Tipo(tipoDato.list, nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "id":
                        return new Tipo(nodo.ChildNodes.ElementAt(0).Token.ToString(), tipoDato.id, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line, nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                }
            }
            else
            {
                switch (nodo.ChildNodes.ElementAt(0).Token.Text)
                {
                    case "map":

                    case "set":
                        break;

                    case "list":
                        break;                       
                }
            }
            return new Tipo(tipoDato.errorSemantico, -1, -1);
        } 


        public LinkedList<Tipo> ListaTipos(ParseTreeNode nodo)
        {
            return new LinkedList<Tipo>();
        }


        public Tipo Primitivos(ParseTreeNode nodo)
        {
            String t = nodo.ChildNodes.ElementAt(0).Token.Text;
            switch (t)
            {
                case "int":
                    return new Tipo(tipoDato.entero, nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                case "string":
                    return new Tipo(tipoDato.cadena, nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                case "boolean":
                    return new Tipo(tipoDato.booleano, nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                case "double":
                    return new Tipo(tipoDato.decimall, nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                case "date":
                    return new Tipo(tipoDato.date, nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                case "time":
                    return new Tipo(tipoDato.time, nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);
            }

            return new Tipo(tipoDato.errorSemantico, -1, -1);
        }


        public Instruccion Log(ParseTreeNode nodo)
        {
            return new Imprimir(expresiones(nodo.ChildNodes.ElementAt(2)), 
                nodo.ChildNodes.ElementAt(0).Token.Location.Line, nodo.ChildNodes.ElementAt(0).Token.Location.Column); 
        }

        public Instruccion whilee(ParseTreeNode nodo)
        {
            return new WhileCiclo(expresiones(nodo.ChildNodes.ElementAt(2)), bloque(nodo.ChildNodes.ElementAt(4)));
        }

        public Expresion expresiones(ParseTreeNode nodo)
        {
            
            if (nodo.ChildNodes.Count == 3)
            {
                string operador = nodo.ChildNodes.ElementAt(1).Term.Name.ToString();
                switch (operador)
                {
                    //aritmeticas
                    case "+":
                        return new Suma(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column, 
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "-":
                        return new Resta(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "*":
                        return new Multiplicacion(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "/":
                        return new Division(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "%":
                        return new Modulo(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "**":
                        return new Potencia(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));





                    case "+=":
                        return new MasIgual(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "-=":
                        return new Resta(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "*=":
                        return new Resta(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "/=":
                        return new Resta(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    


                    //RELACIONALES
                    case "<":
                        return new MenorQ(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case ">":
                        return new MayorQ(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "<=":
                        return new MenorIgualQ(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case ">=":
                        return new MayorIgual(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "==":
                        return new IgualIgual(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "!=":
                        return new Diferente(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    //Logicas
                    case "||":
                        return new Or(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "&&":
                        return new And(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    case "^":
                        return new Xor(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)));

                    default:
                        return expresiones(nodo.ChildNodes.ElementAt(1));

                }

            }
            else if (nodo.ChildNodes.Count == 2)
            {
                string operador = nodo.ChildNodes.ElementAt(0).Term.Name.ToString();
                if (!operador.Equals("E"))
                {
                    switch (operador)
                    {
                        case "-":
                            return new Negatico(nodo.ChildNodes[0].Token.Location.Line, nodo.ChildNodes[0].Token.Location.Column,
                                expresiones(nodo.ChildNodes.ElementAt(1)));                        

                        case "!":
                            return new Nott(nodo.ChildNodes[0].Token.Location.Line, nodo.ChildNodes[0].Token.Location.Column,
                                expresiones(nodo.ChildNodes.ElementAt(1)));

                        case "@":
                            return new ArrobaId("@" + nodo.ChildNodes.ElementAt(1).Token.Text, nodo.ChildNodes[0].Token.Location.Line,
                                nodo.ChildNodes[0].Token.Location.Column);
                    }
                }
                else
                {
                    operador = nodo.ChildNodes.ElementAt(1).Term.Name.ToString();
                    switch (operador)
                    {

                        //-----------------------------------------------------------------------------------------------------------
                        case "--":
                            return new Decremento(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column, 
                                expresiones(nodo.ChildNodes.ElementAt(0)));

                        case "++":
                            return new Incremento(nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column, 
                                expresiones(nodo.ChildNodes.ElementAt(0)));
                        //-----------------------------------------------------------------------------------------------------------

                    }
                }

                }
            else if (nodo.ChildNodes.Count == 1)
            {
                string operador = nodo.ChildNodes.ElementAt(0).Term.Name.ToString();
                switch (operador)
                {
                    case "id":
                        return new Identificador(nodo.ChildNodes.ElementAt(0).Token.Text, nodo.ChildNodes[0].Token.Location.Line,
                            nodo.ChildNodes[0].Token.Location.Column);


                    //fecha y hora
                    case "tdatetime":
                        String valor = nodo.ChildNodes.ElementAt(0).Token.Text.Replace("'","");
                        if (valor.Contains("-"))
                        {
                            return new Date(valor, Operacion.tipoDato.date, nodo.ChildNodes[0].Token.Location.Line, 
                                nodo.ChildNodes[0].Token.Location.Column);
                        }
                        else
                        {
                            return new Time(valor, Operacion.tipoDato.time, nodo.ChildNodes[0].Token.Location.Line, 
                                nodo.ChildNodes[0].Token.Location.Column);
                        }
                        

                    case "numero":
                        //Object valor, tipoDato tipo, int linea, int columna
                        return new Numero(nodo.ChildNodes.ElementAt(0).Token.Text, Operacion.tipoDato.decimall, 
                            nodo.ChildNodes[0].Token.Location.Line, nodo.ChildNodes[0].Token.Location.Column);

                    case "tstring":
                        String valor2 = nodo.ChildNodes.ElementAt(0).Token.Text.Replace("\"", "");
                        return new cadena(valor2, Operacion.tipoDato.cadena, nodo.ChildNodes[0].Token.Location.Line, 
                            nodo.ChildNodes[0].Token.Location.Column);
                    
                    case "true":
                        return new Booleano(nodo.ChildNodes.ElementAt(0).Token.Text, Operacion.tipoDato.booleano,
                            nodo.ChildNodes[0].Token.Location.Line, nodo.ChildNodes[0].Token.Location.Column);

                    case "false":
                        return new Booleano(nodo.ChildNodes.ElementAt(0).Token.Text, Operacion.tipoDato.booleano, 
                            nodo.ChildNodes[0].Token.Location.Line, nodo.ChildNodes[0].Token.Location.Column);

                    case "null":
                        return new Nulo(nodo.ChildNodes.ElementAt(0).Token.Text, Operacion.tipoDato.booleano,
                            nodo.ChildNodes[0].Token.Location.Line, nodo.ChildNodes[0].Token.Location.Column);
                }
            }
            return null;
        }

    }
}
