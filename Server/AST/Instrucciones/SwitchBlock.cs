using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.Relacionales;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class SwitchBlock : Instruccion
    {
        public Expresion condicion { get; set; }
        public int linea { get; set; }
        public int col { get; set; }
        public LinkedList<NodoAST> listaGrupos { get; set; }
        public LinkedList<NodoAST> listaCaseDefault { get; set; }

        public SwitchBlock(Expresion condicion, int linea,
           int col, LinkedList<NodoAST> listaGrupos, LinkedList<NodoAST> listaCaseDefault)
        {
            this.condicion = condicion;
            this.linea = linea;
            this.col = col;
            this.listaGrupos = listaGrupos;
            this.listaCaseDefault = listaCaseDefault;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Object valor = condicion.getValue(entorno, listas, management);
                if (valor == null)
                {
                    listas.errores.AddLast(new NodoError(linea, col, NodoError.tipoError.Semantico,"Condicion del Swicht no es valida para evaluar"));
                    return tipoDato.errorSemantico;
                }

                foreach (NodoAST listaGrupo in listaGrupos)
                {
                        SwitchBlockStatement_Group casee = 
                            (SwitchBlockStatement_Group)listaGrupo;
                        NodoAST caseeDefault = casee.listaCasee.Last();
                        StatementBlock sentencias =
                            (StatementBlock)casee.listaSentencias;
                    if (caseeDefault is Instruccion)
                    {
                        Instruccion instru = (Instruccion)caseeDefault;
                        if (instru is Defaultt)
                        {

                            Object retorno = sentencias.ejecutar(entorno, listas, management);

                            if (retorno is Breakk)
                            {
                                return tipoDato.ok;
                            }
                            else if (retorno is Retorno)
                            {
                                return retorno;
                            }
                            else if (retorno is TipoExcepcion.excep)
                            {
                                return retorno;
                            }
                        }
                    }
                    else if (caseeDefault is Expresion)
                    {
                        Expresion instru = (Expresion)caseeDefault;
                        if (instru is Casee)
                        {
                            IgualIgual relacion = new IgualIgual(linea, col, condicion, ((Casee)instru).valorCase);
                            //Relacional relacion = new Relacional(condicion, ((Casee)instru).getValorCase(), Operacion.Operador.IGUAL, linea, col, null);
                            if ((Boolean)relacion.getValue(entorno, listas, management))
                            {
                                Object retorno = sentencias.ejecutar(entorno, listas, management);

                                if (retorno is Breakk)
                                {
                                    break;
                                }
                                else if (retorno is Retorno)
                                {
                                    return retorno;
                                }
                                else if (retorno is TipoExcepcion.excep)
                                {
                                    return retorno;
                                }

                            }

                        }
                    }
                }

            //la lista case default no se toma en cuenta porque siempre vienen vacios entonces no importa que vengan.

            } catch (Exception e) {
                listas.errores.AddLast(new NodoError(linea, col, NodoError.tipoError.Semantico,
                    "Asignacion no valida"));
                return tipoDato.errorSemantico;
            }
            return null;
        }
    }
}
