using Irony.Parsing;
using Server.Analizador;
using Server.Analizador_CHISON;
using Server.AST;
using Server.AST.BaseDatos;
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

                Entorno global = new Entorno();
                ErrorImpresion listas = new ErrorImpresion();
                Administrador management = new Administrador();
                management.usuarios.Add("admin",new userPass("admin","admin"));
                
                foreach (var item in ASTClases)
                {
                    if (item != null) {
                        if (item is Instruccion)
                        {
                            Instruccion ins = (Instruccion)item;
                            if (ins is CreateType) {
                                ins.ejecutar(global, listas, management);
                            } else if (ins is DeclaracionAsignacion || ins is Declarcion ||
                                      ins is DeclaraListNew || ins is DeclaraListValores ||
                                      ins is DeclaracionSetNew || ins is DeclaracionSetValores ||
                                      ins is DeclaracionMapNew || ins is DeclaracionMapValores || 
                                      ins is BaseDeDatos || ins is Usee_DB || ins is crearUsuario || 
                                      ins is GrantUsu || ins is RevokeUsu || ins is update4 ||
                                      ins is update6 || ins is InsertarInBase || ins is InsertarInBaseEspecial || ins is NodoCrearTabla)
                            {
                                ins.ejecutar(global, listas, management);
                            }
                        }
                        else
                        {//funciones 
                            Expresion exp = (Expresion)item;
                            if (exp is Funciones ||
                                exp is Procedimientos || exp is Select6 ||
                                      exp is Select4) {
                                exp.getValue(global, listas, management);
                            }
                            //aqui faltan los procedimientos
                        }
                    }
                }



                Entorno next = new Entorno(global);
                foreach (var item in ASTClases)
                { 
                    if (item != null)
                    {
                        if (item is Instruccion)
                        {
                            Instruccion ins = (Instruccion)item;
                            if (!(ins is CreateType) && !(ins is DeclaracionAsignacion || ins is Declarcion ||
                                     ins is DeclaraListNew || ins is DeclaraListValores ||
                                     ins is DeclaracionSetNew || ins is DeclaracionSetValores ||
                                     ins is DeclaracionMapNew || ins is DeclaracionMapValores ||
                                     ins is BaseDeDatos || ins is Usee_DB || ins is crearUsuario ||
                                     ins is GrantUsu || ins is RevokeUsu || ins is update4 ||
                                     ins is update6 || ins is InsertarInBase || ins is InsertarInBaseEspecial
                                     || ins is NodoCrearTabla))
                            {
                                ins.ejecutar(next, listas, management);
                            }
                        }
                        else
                        {//funciones 
                            Expresion exp = (Expresion)item;
                            if (!(exp is Funciones || exp is Procedimientos || exp is Select6 ||
                                      exp is Select4))
                            {
                                exp.getValue(next, listas, management);
                            }
                        }
                    }
                }
                //paraaaaaaaaaaaaaaaaaaaaaaa

                return listas;
            }
            return null;
        }



        public ErrorImpresion analizarCHISON(String texto)
        {
            //LENGUAJE -------------->CHISON
            GramaticaCHISON gram = new GramaticaCHISON();
            Parser p = new Parser(gram);
            ParseTree arbol = p.Parse(texto);
            ParseTreeNode raiz = arbol.Root;

            if (raiz == null || arbol.HasErrors() || arbol.ParserMessages.Count > 0)
            {
                //hay errores
                foreach (var item in arbol.ParserMessages)
                {
                    MessageBox.Show("Error: " + item.Message + " Linea: " + item.Location.Line + " Columna: " + item.Location.Column);
                }
            }

            return null;
        }
    }
}
