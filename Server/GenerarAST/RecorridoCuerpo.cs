using Irony.Parsing;
using Server.AST;
using Server.AST.Ciclos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.Aritmeticas;
using Server.AST.Expresiones.Logicas;
using Server.AST.Expresiones.Relacionales;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Instrucciones;
using Server.AST.Instrucciones.Ciclos;
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
                    case "ACCESOASIGNACION":
                        return accesoAsignacion(nodo.ChildNodes.ElementAt(0));

                    case "CREATETYPE":
                        return CrearType(nodo.ChildNodes.ElementAt(0));

                    case "DECLARATODO":
                        return new Declarcion(Tipos(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0)) , 
                        ListaVariables(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1)));

                    case "DECLAASGINACION":
                    return new DeclaracionAsignacion(Tipos(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0)), 
                        ListaVariables(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1)), 
                        expresiones(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(3)));

                    case "ALTERTYPE": //no aplica
                        break;

                    case "ELIMINARUSERTYPE": //no aplica
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
                        return ifStatement(nodo.ChildNodes.ElementAt(0));

                    case "SWITCHSTATEMENT":
                        return SwitchSatement(nodo.ChildNodes.ElementAt(0));

                    case "WHILEE":
                        return whilee(nodo.ChildNodes.ElementAt(0));

                    case "DOWHILE":
                        return new DoWhile(bloque(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1)),
                            expresiones(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(4)),
                            nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Location.Column);

                    case "AUMENTOSSOLOS":
                        return AumentosSolos(nodo.ChildNodes.ElementAt(0));

                    case "FORR":
                        return forstatement(nodo.ChildNodes.ElementAt(0));

                    case "MAPCOLLECTIONS":
                        return MapCollectios(nodo.ChildNodes.ElementAt(0));

                    case "LISTCOLLECTIONS":
                        return ListCollectios(nodo.ChildNodes.ElementAt(0));

                    case "SETCOLLECTIONS":
                        return SetCollections(nodo.ChildNodes.ElementAt(0));

                    case "FUNCIONESMETODOS":
                        return FuncionesMetodos(nodo.ChildNodes.ElementAt(0));

                    case "LLAMADASFUNCIONES":
                    return new LlamadaFuncion(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Text,
                        expresiones(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2)),
                        nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Location.Column);          

                    case "PROCEDIMIENTOS":
                        break;

                    case "SENTENCIATRANSFERENCIA":
                        return sentenciaTranferencia(nodo.ChildNodes.ElementAt(0));

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


        public NodoAST CrearType(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 9)
            {
                return new CreateType(nodo.ChildNodes.ElementAt(5).Token.Text.ToLower(),
                    itemTypes(nodo.ChildNodes.ElementAt(7)),
                    nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(1).Token.Location.Column,
                    true);
            }
            else
            {
                return new CreateType(nodo.ChildNodes.ElementAt(2).Token.Text.ToLower(),
                    itemTypes(nodo.ChildNodes.ElementAt(4)),
                    nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(1).Token.Location.Column);
            }
        }

        public LinkedList<itemType> itemTypes(ParseTreeNode nodo)
        {
            LinkedList<itemType> lista = new LinkedList<itemType>();
            foreach (ParseTreeNode ite in nodo.ChildNodes)
            {
                itemType i = item(ite);
                lista.AddLast(i);
            }
            return lista;
        }

        public itemType item(ParseTreeNode nodo)
        {
            String i = nodo.ChildNodes.ElementAt(0).ToString();
            if (i == "TIPOS")
            {
                return new itemType(Tipos(nodo.ChildNodes.ElementAt(0)),
                    nodo.ChildNodes.ElementAt(1).Token.Text);
            }
            else
            {
                return new itemType(nodo.ChildNodes.ElementAt(0).Token.Text,
                    Tipos(nodo.ChildNodes.ElementAt(1)));
            }
        }

        public NodoAST SwitchSatement(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 8)
            {
                return new SwitchBlock(expresiones(nodo.ChildNodes.ElementAt(2)),
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column,
                    sbsgroups(nodo.ChildNodes.ElementAt(5)),
                    lslabel(nodo.ChildNodes.ElementAt(6)));
            }
            else if (nodo.ChildNodes.Count == 7)
            {
                string s = nodo.ChildNodes.ElementAt(5).ToString();
                switch (s)
                {
                    case "SWITCHBLOCKSTATEMENTGROUPS":
                            return new SwitchBlock(expresiones(nodo.ChildNodes.ElementAt(2)),
                        nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column,
                        sbsgroups(nodo.ChildNodes.ElementAt(5)),
                        new LinkedList<NodoAST>());
                

                    case "SWITCHLABELS":
                            return new SwitchBlock(expresiones(nodo.ChildNodes.ElementAt(2)),
                        nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column,
                        new LinkedList<NodoAST>(),
                        lslabel(nodo.ChildNodes.ElementAt(5)));
                    }
            }
            else if(nodo.ChildNodes.Count == 6)
            {
                return new SwitchBlock(expresiones(nodo.ChildNodes.ElementAt(2)),
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column, 
                    new LinkedList<NodoAST>(), new LinkedList<NodoAST>());
            }
            return null;
        }

        public LinkedList<NodoAST> sbsgroups(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                LinkedList<NodoAST> nodoAST = sbsgroups(nodo.ChildNodes.ElementAt(0));
                nodoAST.AddLast(lodelGroup(nodo.ChildNodes.ElementAt(1)));
                return nodoAST;
            }
            else
            {
                LinkedList<NodoAST> nodoAST = new LinkedList<NodoAST>();
                nodoAST.AddLast(lodelGroup(nodo.ChildNodes.ElementAt(0)));
                return nodoAST;
            }
        }

        public NodoAST lodelGroup(ParseTreeNode nodo)
        {
            LinkedList<NodoAST> nodoAST = new LinkedList<NodoAST>();
            nodoAST = Inicio(nodo.ChildNodes.ElementAt(1));
            if (nodoAST == null)
            {
                return new SwitchBlockStatement_Group(lslabel(nodo.ChildNodes.ElementAt(0)), new StatementBlock(new LinkedList<NodoAST>()));
            }
            else
            {
                return new SwitchBlockStatement_Group(lslabel(nodo.ChildNodes.ElementAt(0)),
                new StatementBlock(nodoAST));
            }
            
        }

        public LinkedList<NodoAST> lslabel(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                LinkedList<NodoAST> nodoAST = lslabel(nodo.ChildNodes.ElementAt(0));
                nodoAST.AddLast(label(nodo.ChildNodes.ElementAt(1)));
                return nodoAST;
            }
            else
            {
                LinkedList<NodoAST> nodoAST = new LinkedList<NodoAST>();
                nodoAST.AddLast(label(nodo.ChildNodes.ElementAt(0)));
                return nodoAST;
            }
        }


        public NodoAST label(ParseTreeNode nodo)
        {
            String s = nodo.ChildNodes.ElementAt(0).Token.Text;
            switch (s)
            {
                case "case":
                    return new Casee(expresiones(nodo.ChildNodes.ElementAt(1)),
                        nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                case "default":
                    return new Defaultt(nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);

            }
            return null;
        }






        public NodoAST SetCollections(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 8)
            {
                return new DeclaracionSetNew(ListaVariables(nodo.ChildNodes.ElementAt(1)),
                    Tipos(nodo.ChildNodes.ElementAt(6)).tipo,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column);
            }
            else
            {
                return new DeclaracionSetValores(ListaVariables(nodo.ChildNodes.ElementAt(1)),
                    expresiones(nodo.ChildNodes.ElementAt(4)),
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column);
            }
            return null;
        }

        public NodoAST ListCollectios(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 8)
            {
                return new DeclaraListNew(ListaVariables(nodo.ChildNodes.ElementAt(1)),
                    Tipos(nodo.ChildNodes.ElementAt(6)).tipo,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column);
            }
            else
            {
                return new DeclaraListValores(ListaVariables(nodo.ChildNodes.ElementAt(1)),
                    expresiones(nodo.ChildNodes.ElementAt(4)),
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column);
            }
            return null;
        }


        public NodoAST MapCollectios(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 10)
            {
                return new DeclaracionMapNew(ListaVariables(nodo.ChildNodes.ElementAt(1)),
                        Primitivos(nodo.ChildNodes.ElementAt(6)).tipo,
                        Tipos(nodo.ChildNodes.ElementAt(8)).tipo,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).Token.Location.Column);

            }
            else
            {
                return new DeclaracionMapValores(ListaVariables(nodo.ChildNodes.ElementAt(1)),
                    expresiones(nodo.ChildNodes.ElementAt(4)),
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column);
            }
            return null;
        }

        public Expresion AumentosSolos(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 4)
            {
                ///-----------------------------------------------------------------------------FALTA
            }
            else if (nodo.ChildNodes.Count == 3)
            {
                String s = nodo.ChildNodes.ElementAt(1).Token.Text;
                switch (s)
                {
                    case "+=":
                        return new MasIgual(nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(1).Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(2)), 
                            variable(nodo.ChildNodes.ElementAt(0)));

                    case "-=":
                        return new MenosIgual(nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(1).Token.Location.Column,
                            expresiones(nodo.ChildNodes.ElementAt(2)),
                            variable(nodo.ChildNodes.ElementAt(0)));

                    case "LISTAID":
                        break;
                }
            }
            else if (nodo.ChildNodes.Count == 2)
            {
                String s = nodo.ChildNodes.ElementAt(1).Token.Text;
                switch (s)
                {
                    case "++":
                        return new Incremento(variable(nodo.ChildNodes.ElementAt(0)),
                            nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(1).Token.Location.Column);

                    case "--":
                        return new Decremento(variable(nodo.ChildNodes.ElementAt(0)),
                            nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(1).Token.Location.Column);
                }
            }
            return null;
        }


        public NodoAST forstatement(ParseTreeNode nodo)
        {
            return new Forr(Inicializacion(nodo.ChildNodes.ElementAt(2)),
                expresiones(nodo.ChildNodes.ElementAt(4)),
                Actualizacion(nodo.ChildNodes.ElementAt(6)),
                bloque(nodo.ChildNodes.ElementAt(8)),
                nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                nodo.ChildNodes.ElementAt(0).Token.Location.Column);
        }

        public NodoAST Inicializacion(ParseTreeNode nodo)
        {
            return Sentencia(nodo);
        }


        public Expresion Actualizacion(ParseTreeNode nodo)
        {
            switch (nodo.ChildNodes.ElementAt(0).ToString())
            {
                case "AUMENTOSSOLOS":
                    return AumentosSolos(nodo.ChildNodes.ElementAt(0));

                case "E":
                    break;
            }

            return null;
        }


        public NodoAST ifStatement(ParseTreeNode nodo)
        {/*IF_LISTA + elsee + STATEMENTBLOCK
                             | IF_LISTA*/
            if (nodo.ChildNodes.Count == 3)
            {
                return new Iff(bloque(nodo.ChildNodes.ElementAt(2)),
                    listaifs(nodo.ChildNodes.ElementAt(0)),
                    nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(1).Token.Location.Column);
            }
            else
            {
                return new Iff(null,
                    listaifs(nodo.ChildNodes.ElementAt(0)), -1, -1);
            }
        }

        public LinkedList<IfLista> listaifs(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 7)
            {
                LinkedList<IfLista> nodoAST = listaifs(nodo.ChildNodes.ElementAt(0));
                nodoAST.AddLast(new IfLista(expresiones(nodo.ChildNodes.ElementAt(4)),
                    bloque(nodo.ChildNodes.ElementAt(6)),
                    nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(1).Token.Location.Column));
                return nodoAST;
            }
            else
            {
                LinkedList<IfLista> nodoAST = new LinkedList<IfLista>();
                nodoAST.AddLast(new IfLista(expresiones(nodo.ChildNodes.ElementAt(2)),
                    bloque(nodo.ChildNodes.ElementAt(4)),
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column));
                return nodoAST;
            }
        }

        public NodoAST accesoAsignacion(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 3)
            {
                String s = nodo.ChildNodes.ElementAt(2).ToString();
                switch (s)
                {
                    case "E":
                        return new Asignacion(variable(nodo.ChildNodes.ElementAt(0)),
                            expresiones(nodo.ChildNodes.ElementAt(2)),
                            nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(1).Token.Location.Column);

                    case "FUNCIONESCOLLECTIONS":
                        break;

                    case "FUNCIONESNATIVASCADENAS":
                        break;

                    case "FUNCIONESNATIVASABSTRACCION":
                        break;
                }
            }
            else if (nodo.ChildNodes.Count == 4)
            {
                String s = nodo.ChildNodes.ElementAt(3).ToString();
                switch (s)
                {
                    case "E":
                        break;

                    case "FUNCIONESCOLLECTIONS":
                        break;

                    case "FUNCIONESNATIVASCADENAS":
                        break;

                    case "FUNCIONESNATIVASABSTRACCION":
                        break;
                }
            }
            return null;
        }


        public NodoAST sentenciaTranferencia(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                return new Retorno(expresiones(nodo.ChildNodes.ElementAt(1)), 
                    nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                    nodo.ChildNodes.ElementAt(0).Token.Location.Column);
            }
            else
            {
                String nombre = nodo.ChildNodes.ElementAt(0).Token.Text;
                switch (nombre)
                {
                    case "continue":
                        return new Continuee(nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "break":
                        return new Breakk(nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "return":
                        return new Retorno(nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);
                }
            }

            return null;
        }


        public Funciones FuncionesMetodos(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 6)
            {
                return new Funciones(Tipos(nodo.ChildNodes.ElementAt(0)),
                    nodo.ChildNodes.ElementAt(1).Token.Text.ToLower(),
                    Parametros(nodo.ChildNodes.ElementAt(3)),
                    bloque(nodo.ChildNodes.ElementAt(5)),
                    nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(1).Token.Location.Column);
            }
            else if (nodo.ChildNodes.Count == 5)
            {
                return new Funciones(Tipos(nodo.ChildNodes.ElementAt(0)),
                    nodo.ChildNodes.ElementAt(1).Token.Text.ToLower(),
                    new LinkedList<Parametros>(),
                    bloque(nodo.ChildNodes.ElementAt(4)),
                    nodo.ChildNodes.ElementAt(1).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(1).Token.Location.Column);
            }            
            return null;
        }

        public LinkedList<Parametros> Parametros(ParseTreeNode nodo)
        {
            if(nodo.ChildNodes.Count == 5)
            {

                LinkedList<Parametros> nodoAST = Parametros(nodo.ChildNodes.ElementAt(0));
                nodoAST.AddLast(new Parametros(Tipos(nodo.ChildNodes.ElementAt(2)),
                    "@" + nodo.ChildNodes.ElementAt(4).Token.Text.ToLower(),
                    nodo.ChildNodes.ElementAt(4).Token.Location.Line,
                    nodo.ChildNodes.ElementAt(4).Token.Location.Column));
                return nodoAST;

            }
            else
            {
                LinkedList<Parametros> nodoAST = new LinkedList<Parametros>();
                nodoAST.AddLast(new Parametros(Tipos(nodo.ChildNodes.ElementAt(0)), 
                    "@" + nodo.ChildNodes.ElementAt(2).Token.Text.ToLower(),
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
            return "@" + nodo.ChildNodes.ElementAt(1).Token.Text.ToLower();
        }

        public Tipo Tipos(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 1) {
                String t = nodo.ChildNodes.ElementAt(0).Term.Name;//.ToLower() ;//ToString().Replace("Keyboard", "").Trim();
                switch (t)
                {
                    case "TIPOSPRIMITIVOS":
                        return Primitivos(nodo.ChildNodes.ElementAt(0));

                    case "counter":
                        return new Tipo(tipoDato.counter, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "map":
                        return new Tipo(tipoDato.map, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "set":
                        return new Tipo(tipoDato.set, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "list":
                        return new Tipo(tipoDato.list, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "id":
                        return new Tipo(nodo.ChildNodes.ElementAt(0).Token.Text.ToString().ToLower(), tipoDato.id, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line, 
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                }
            }
            else
            {
                String s = nodo.ChildNodes.ElementAt(0).Token.Text.ToLower();
                switch (s)
                {
                    case "map":
                        return new Tipo(tipoDato.map,
                            ListaTipos(nodo.ChildNodes.ElementAt(2)),
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "set":
                        return new Tipo(tipoDato.set,
                            Tipos(nodo.ChildNodes.ElementAt(2)).tipo,
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                    case "list":
                        return new Tipo(tipoDato.list,
                            Tipos(nodo.ChildNodes.ElementAt(2)).tipo,
                            nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                            nodo.ChildNodes.ElementAt(0).Token.Location.Column);

                }
            }
            return new Tipo(tipoDato.errorSemantico, -1, -1);
        } 


        public LinkedList<Tipo> ListaTipos(ParseTreeNode nodo)
        {           
            LinkedList<Tipo> listatipos = new LinkedList<Tipo>();
            foreach (ParseTreeNode ItemTipo in nodo.ChildNodes)
            {
                Tipo tipoMap = Tipos(ItemTipo);
                listatipos.AddLast(tipoMap);
            }
            return listatipos;
        }


        public Tipo Primitivos(ParseTreeNode nodo)
        {
            String t = nodo.ChildNodes.ElementAt(0).Token.Text.ToLower();
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
            return new WhileCiclo(expresiones(nodo.ChildNodes.ElementAt(2)), 
                bloque(nodo.ChildNodes.ElementAt(4)),
                nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                nodo.ChildNodes.ElementAt(0).Token.Location.Column);
        }

        public Expresion expresiones(ParseTreeNode nodo)
        {
            if (nodo.ChildNodes.Count == 4)
            {
                string operador = nodo.ChildNodes.ElementAt(0).ToString();
                switch (operador)
                {                    
                    case "aparentesis": //CASTEOS
                        break;
                }
            }
            else if (nodo.ChildNodes.Count == 3)
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


                    //E coma E
                    case ",":
                        return new ListaExpresiones(expresiones(nodo.ChildNodes.ElementAt(0)), expresiones(nodo.ChildNodes.ElementAt(2)),
                            nodo.ChildNodes[1].Token.Location.Line, nodo.ChildNodes[1].Token.Location.Column);

                    //E dospuntos E
                    case ":":
                        ClaveValor claVa = new ClaveValor(expresiones(nodo.ChildNodes.ElementAt(0)), 
                            expresiones(nodo.ChildNodes.ElementAt(2)));
                        return new DosPuntos(claVa,
                            nodo.ChildNodes[1].Token.Location.Line, 
                            nodo.ChildNodes[1].Token.Location.Column);

                    case "E":
                        String sss = nodo.ChildNodes.ElementAt(0).Token.Text.ToLower();
                        switch (sss)
                        {
                            case "(":
                                return expresiones(nodo.ChildNodes.ElementAt(1));

                            case "[":
                                return new Corchetes(expresiones(nodo.ChildNodes.ElementAt(1)),
                                    nodo.ChildNodes[0].Token.Location.Line,
                                    nodo.ChildNodes[0].Token.Location.Column);

                            case "{":
                                break;

                        }
                        break;
                        

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
                            return new ArrobaId("@" + nodo.ChildNodes.ElementAt(1).Token.Text.ToLower(), nodo.ChildNodes[0].Token.Location.Line,
                                nodo.ChildNodes[0].Token.Location.Column);

                        case "new":
                            return new Neww(Tipos(nodo.ChildNodes.ElementAt(1)),
                                nodo.ChildNodes.ElementAt(0).Token.Location.Line,
                                nodo.ChildNodes.ElementAt(0).Token.Location.Column);
                    }
                }
                else
                {
                    operador = nodo.ChildNodes.ElementAt(1).Term.Name.ToString();
                    switch (operador)
                    {

                        //-----------------------------------------------------------------------------------------------------------
                        case "--":
                            return new Decremento(expresiones(nodo.ChildNodes.ElementAt(0)),
                                nodo.ChildNodes[1].Token.Location.Line, 
                                nodo.ChildNodes[1].Token.Location.Column);

                        case "++":
                            return new Incremento(expresiones(nodo.ChildNodes.ElementAt(0)),
                                nodo.ChildNodes[1].Token.Location.Line,
                                nodo.ChildNodes[1].Token.Location.Column);
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
                        return new Identificador(nodo.ChildNodes.ElementAt(0).Token.Text.ToLower(), nodo.ChildNodes[0].Token.Location.Line,
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

                    case "LLAMADASFUNCIONES":
                        return new LlamadaFuncion(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(0).Token.Text,
                        expresiones(nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(2)),
                        nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Location.Line,
                        nodo.ChildNodes.ElementAt(0).ChildNodes.ElementAt(1).Token.Location.Column);

                }
            }
            return null;
        }

    }
}
