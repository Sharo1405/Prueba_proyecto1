using Irony.Parsing;
using Server.Analizador;
using Server.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.GenerarAST
{
    class Iniciar
    {

        //metodo que inicia el analisis del texto
        public void analizar(String texto)
        {
            //LENGUAJE -------------->CQL
            Gramatica gram = new Gramatica();
            Parser parser = new Parser(gram);
            ParseTree arbol = parser.Parse(texto);
            ParseTreeNode raiz = arbol.Root;

            if (raiz == null || arbol.HasErrors() || arbol.ParserMessages.Count > 0)
            {
                //hay errores
                foreach (var item in arbol.ParserMessages)
                {
                    MessageBox.Show("Error: " + item.Message + " Linea: " + item.Location.Line + " Columna: " + item.Location.Column);
                }
            }
            else
            {
                //todo cool para analizar

                RecorridoCuerpo recorrido = new RecorridoCuerpo();
                LinkedList<NodoAST> ASTClases = recorrido.Rcuerpo(raiz.ChildNodes.ElementAt(0));


            }
        }
    }
}
