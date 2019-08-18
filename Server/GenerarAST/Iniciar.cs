using Irony.Parsing;
using Server.Analizador;
using Server.AST;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Instrucciones;
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
        public ErrorImpresion analizar(String texto)
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
                LinkedList<NodoAST> ASTClases = recorrido.Inicio(raiz);

                Entorno entorno = new Entorno();
                ErrorImpresion listas = new ErrorImpresion();

                foreach (var item in ASTClases)
                {
                    if (item is Instruccion)
                    {
                        Instruccion ins = (Instruccion)item;
                        ins.ejecutar(entorno, listas);
                    }
                }
                //paraaaaaaaaaaaaaaaaaaaaaaa

                return listas;
            }
            return null;
        }
    }
}
