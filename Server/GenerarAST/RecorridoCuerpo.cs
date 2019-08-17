using Irony.Parsing;
using Server.AST;
using Server.AST.Ciclos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.Aritmeticas;
using Server.AST.Expresiones.Logicas;
using Server.AST.Expresiones.Relacionales;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Instrucciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return null;
        }

        public LinkedList<NodoAST> bloque(ParseTreeNode nodo)
        {
            LinkedList<NodoAST> nodoAST = new LinkedList<NodoAST>();
            nodoAST = Inicio(nodo.ChildNodes.ElementAt(1));
            if (nodoAST == null)
            {
                return new LinkedList<NodoAST>();
            }
            else
            {
                return nodoAST;
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
                        break;

                    case "DECLAASGINACION":
                        break;

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
                        break;

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
                if (!operador.Equals("EXP"))
                {
                    switch (operador)
                    {
                        case "-":
                            return new Negatico(nodo.ChildNodes[0].Token.Location.Line, nodo.ChildNodes[0].Token.Location.Column,
                                expresiones(nodo.ChildNodes.ElementAt(1)));                        

                        case "!":
                            return new Nott(nodo.ChildNodes[0].Token.Location.Line, nodo.ChildNodes[0].Token.Location.Column,
                                expresiones(nodo.ChildNodes.ElementAt(1)));
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
                        break;

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
