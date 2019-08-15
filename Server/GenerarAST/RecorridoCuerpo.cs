using Irony.Parsing;
using Server.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.GenerarAST
{
    class RecorridoCuerpo
    {

        public LinkedList<NodoAST> Rcuerpo(ParseTreeNode nodo) 
        {
            foreach (var item in nodo.ChildNodes)
            {
                switch (nodo.Term.Name)
                {
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
                        break;

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
                        break;

                    case "EXCEPCIONES":
                        break;

                    case "TRYCATCHH":
                        break;
                }
            }

            LinkedList<NodoAST> list = new LinkedList<NodoAST>();
            return list;
        }

    }
}
