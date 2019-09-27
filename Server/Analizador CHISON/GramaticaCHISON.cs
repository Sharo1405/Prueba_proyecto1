using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Server.Analizador_CHISON
{
    class GramaticaCHISON : Grammar
    {
        public GramaticaCHISON() : base(caseSensitive: false)
        {

            MarkReservedWords("CQL-TYPE");
            

            #region VARIABLES_PRESERVADAS
            var databases = ToTerm("databases");
            var users = ToTerm("users");
            var name = ToTerm("name");
            var data = ToTerm("data");
            var password = ToTerm("password");
            var permisos = ToTerm("permissions");
            var cqltype = ToTerm("CQL-TYPE");
            var columns = ToTerm("columns");
            var type = ToTerm("type");
            var pk = ToTerm("pk");
            var attrs = ToTerm("attrs");
            var parameters = ToTerm("parameters");
            var instr = ToTerm("instr");
            var ass = ToTerm("as");
            var truee = ToTerm("true");
            var falsee = ToTerm("false");
            var inn = ToTerm("in");
            var outt = ToTerm("out");
            var intt = ToTerm("int");
            var stringg = ToTerm("string");
            var booleann = ToTerm("boolean");
            var doublee = ToTerm("doublee");
            var datee = ToTerm("date");
            var timee = ToTerm("time");
            var list = ToTerm("list");
            var set = ToTerm("set");
            var counter = ToTerm("counter");
            #endregion


            #region SIGNOS
            var dolar = ToTerm("$");
            var allave = ToTerm("{");
            var cllave = ToTerm("}");
            var acorchete = ToTerm("[");
            var ccorchete = ToTerm("]");
            var menorq = ToTerm("<");
            var mayorq = ToTerm(">");
            var comillas = ToTerm("\"");
            var igual = ToTerm("=");
            var dospuntos = ToTerm(":");
            var coma = ToTerm(",");
            #endregion

            #region NO_TERMINALES
            var CUERPO = new NonTerminal("CUERPO");
            var S = new NonTerminal("S");
            var SENTENCIA = new NonTerminal("SENTENCIA");
            var BASESDEDATOS = new NonTerminal("BASESDEDATOS");
            var LISTABASES = new NonTerminal("LISTABASES");
            var BASES = new NonTerminal("BASES");
            var ITEMB = new NonTerminal("ITEMB");
            var USUARIOS = new NonTerminal("USUARIOS");
            var LISTABASEDEDATOS = new NonTerminal("LISTABASEDEDATOS");
            var LISTACQLTYPE = new NonTerminal("LISTACQLTYPE");
            var ITEMCQLTYPE = new NonTerminal("LISTACQLTYPE");
            var USUS = new NonTerminal("USUS");
            var LISTAITEMUSU = new NonTerminal("LISTAITEMUSU");
            var ITEMUSU = new NonTerminal("ITEMUSU");
            var LISTAPERMISOS = new NonTerminal("LISTAPERMISOS");
            var ITEMLISTAPERMISOS = new NonTerminal("ITEMLISTAPERMISOS");
            var LISTATRIBUTOS = new NonTerminal("LISTATRIBUTOS");
            var LISTACOLUMNAS = new NonTerminal("LISTACOLUMNAS");
            var DATABASES = new NonTerminal("DATABASES");
            var LISTAPARAMETROS = new NonTerminal("LISTAPARAMETROS");
            var ITEMCOLUMNAS = new NonTerminal("ITEMCOLUMNAS");
            var ITCOL = new NonTerminal("ITCOL");
            var ITEMATRIBUTOS = new NonTerminal("ITEMATRIBUTOS");
            var IATRI = new NonTerminal("IATRI");
            var ITEMPARAMETRO = new NonTerminal("ITEMPARAMETRO");
            var IPARA = new NonTerminal("IPARA");
            var IMPORTAR = new NonTerminal("IMPORTAR");
            var VALOR = new NonTerminal("VALOR");
            var DD = new NonTerminal("DD");
            var ICQL = new NonTerminal("ICQL");
            var LISTAA = new NonTerminal("LISTA");
            var SETT = new NonTerminal("SET");
            var OBJETOO = new NonTerminal("OBJETOO");
            var TIPOS = new NonTerminal("TIPOS");
            var TIPOSPRIMITIVOS = new NonTerminal("TIPOSPRIMITIVOS");
            var LISTATIPOS = new NonTerminal("LISTATIPOS");
            #endregion


            #region TERMINALES
            NumberLiteral numero = TerminalFactory.CreateCSharpNumber("numero");
            IdentifierTerminal erid = TerminalFactory.CreateCSharpIdentifier("erid");
            var tstring = new StringLiteral("tstring", "\"", StringOptions.AllowsAllEscapes);
            CommentTerminal cadenaDolar = new CommentTerminal("cadenaDolar", "${", "}$");
            var datetime = new StringLiteral("datetime", "'", StringOptions.AllowsAllEscapes);
            StringLiteral cadenaProcedimiento = new StringLiteral("cadenaProcedimiento", "$", StringOptions.AllowsLineBreak);
            #endregion


            #region GRAMATICA

            this.Root = S;

            S.Rule = dolar + menorq + CUERPO + mayorq + dolar
                    | USUS
                    | LISTAPERMISOS

                    | BASES
                    | LISTACQLTYPE
                    | Empty;

            CUERPO.Rule = CUERPO + coma + SENTENCIA
                        | SENTENCIA;
                        //| Empty;

            SENTENCIA.Rule = BASESDEDATOS
                           | USUARIOS;



            //USUARIOS
            USUARIOS.Rule = comillas + users + comillas + igual +acorchete + USUS + ccorchete;

            USUS.Rule = USUS + coma + menorq + LISTAITEMUSU + mayorq
                      | menorq + LISTAITEMUSU + mayorq
                      | IMPORTAR
                      | Empty;

            LISTAITEMUSU.Rule = MakeStarRule(LISTAITEMUSU, coma, ITEMUSU);

            ITEMUSU.Rule = comillas + name + comillas + igual + tstring
                         | comillas + password + comillas + igual + tstring
                         | comillas + permisos + comillas + igual + acorchete +  LISTAPERMISOS + ccorchete
                         | comillas + permisos + comillas + igual + acorchete + ccorchete;

            LISTAPERMISOS.Rule = MakeStarRule(LISTAPERMISOS , coma, ITEMLISTAPERMISOS);
                                /*LISTAPERMISOS + coma + ITEMLISTAPERMISOS
                               | ITEMLISTAPERMISOS;*/

            ITEMLISTAPERMISOS.Rule = menorq + comillas + name + comillas + igual + tstring + mayorq
                                   | IMPORTAR;


            //TIPOS TIPOS TIPOS
            TIPOS.Rule = TIPOSPRIMITIVOS
                   | counter
                   | set + menorq + TIPOS + mayorq //para columnas de tablas
                   | set
                   | list + menorq + TIPOS + mayorq //para columnas de tablas
                   | list
                   | erid;

            TIPOSPRIMITIVOS.Rule = intt
                    | stringg
                    | booleann
                    | doublee
                    | datee
                    | timee;

            LISTATIPOS.Rule = MakePlusRule(LISTATIPOS, coma, TIPOS);




            //BASES DE DATOS
            BASESDEDATOS.Rule = comillas + databases + comillas + igual + acorchete + BASES + ccorchete;

            BASES.Rule = BASES + coma + menorq + LISTABASES + mayorq
                       | menorq + LISTABASES + mayorq
                       | IMPORTAR
                       | Empty;

            LISTABASES.Rule = MakeStarRule(LISTABASES, coma, ITEMB);
            /*LISTABASES + coma + ITEMB
            | ITEMB;*/

            ITEMB.Rule = comillas + name + comillas + igual + tstring
                       | comillas + data + comillas + igual + acorchete + LISTACQLTYPE + ccorchete;
                       //| IMPORTAR;

            LISTACQLTYPE.Rule = LISTACQLTYPE + coma + menorq + ITEMCQLTYPE + mayorq
                             | menorq + ITEMCQLTYPE + mayorq
                             | IMPORTAR
                             | Empty;

            ITEMCQLTYPE.Rule = MakeStarRule(ITEMCQLTYPE, coma, ICQL);

            ICQL.Rule = comillas + cqltype + comillas + igual + tstring
                             | comillas + name + comillas + igual + tstring
                             | comillas + type + comillas + igual + tstring
                             | comillas + pk + comillas + igual + truee
                             | comillas + pk + comillas + igual + falsee
                             | comillas + attrs + comillas + igual + acorchete + LISTACQLTYPE + ccorchete
                             | comillas + columns + comillas + igual + acorchete + LISTACQLTYPE + ccorchete
                             | comillas + data + comillas + igual + acorchete + LISTACQLTYPE + ccorchete
                             //| comillas + data + comillas + igual + acorchete + IMPORTAR + ccorchete
                             | comillas + parameters + comillas + igual + acorchete + LISTACQLTYPE + ccorchete
                             | comillas + ass + comillas + igual + inn
                             | comillas + ass + comillas + igual + outt
                             | comillas + instr + comillas + igual + cadenaProcedimiento // SON SENTENCIAS DEL CQL
                             | comillas + instr + comillas + igual + IMPORTAR // SON SENTENCIAS DEL CQL                             
                             | IMPORTAR
                             | comillas + erid + comillas + igual + VALOR;                 

            IMPORTAR.Rule = cadenaDolar;


            VALOR.Rule = tstring
                      | numero
                      | datetime
                      | falsee
                      | truee
                      | acorchete + LISTAA + ccorchete
                      | allave + SETT + cllave
                      | DATABASES;
            //| LISTACQLTYPE;

            DATABASES.Rule = //DATABASES + coma + LISTATRIBUTOS
                            menorq + ITEMCQLTYPE + mayorq;
                            //| IMPORTAR
                            //| Empty;

            //LISTATRIBUTOS.Rule = menorq + ITEMCQLTYPE + mayorq;
                               //| ITEMCQLTYPE;

            LISTAA.Rule = MakeStarRule(LISTAA, coma, VALOR);

            SETT.Rule = MakeStarRule(SETT, coma, VALOR);

            /*OBJETOO.Rule = OBJETOO + coma + menorq + ITEMCQLTYPE + mayorq
                         | menorq + ITEMCQLTYPE + mayorq;*/

            //ITEMATRIBUTOS.Rule = comillas + erid + comillas + igual + VALOR;
            #endregion
        }


    }
}
