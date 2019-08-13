using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Server.Analizador
{
    class Gramatica : Grammar
    {    

        //FALTA LOS CARACTERES DE ESCAPE REVISARLO DESPUES
        //EL ID DEBE LLEVAR @ VER COMO PONERSELA
        public Gramatica() : base(caseSensitive: false)
        {
            #region COMENTARIOS
            CommentTerminal comentariosimple = new CommentTerminal("comentario_simple", "//", "\n", "\r\n");
            CommentTerminal comentariodoble = new CommentTerminal("comentario_mult", "/*", "*/");
            NonGrammarTerminals.Add(comentariosimple);
            NonGrammarTerminals.Add(comentariodoble);
            #endregion

            #region PALABRAS_RESERVADAS
            MarkReservedWords("null");
            MarkReservedWords("int");
            MarkReservedWords("double");
            MarkReservedWords("boolean");
            MarkReservedWords("string");
            MarkReservedWords("date");
            MarkReservedWords("time");
            MarkReservedWords("true");
            MarkReservedWords("false");
            MarkReservedWords("create");
            MarkReservedWords("type");
            MarkReservedWords("if");
            MarkReservedWords("not");
            MarkReservedWords("exists");
            MarkReservedWords("new");
            MarkReservedWords("alter");
            MarkReservedWords("add");
            MarkReservedWords("delete");
            MarkReservedWords("database");
            MarkReservedWords("use");
            MarkReservedWords("drop");
            MarkReservedWords("counter");
            MarkReservedWords("primary");
            MarkReservedWords("key");
            MarkReservedWords("update");
            MarkReservedWords("truncate");
            MarkReservedWords("commit");
            MarkReservedWords("rollback");
            MarkReservedWords("with");
            MarkReservedWords("password");
            MarkReservedWords("user");
            MarkReservedWords("grant");
            MarkReservedWords("on");
            MarkReservedWords("revoke");
            MarkReservedWords("insert");
            MarkReservedWords("into");
            MarkReservedWords("values");
            MarkReservedWords("set");
            MarkReservedWords("where");
            MarkReservedWords("from");
            MarkReservedWords("select");
            MarkReservedWords("order");
            MarkReservedWords("by");
            MarkReservedWords("asc");
            MarkReservedWords("desc");
            MarkReservedWords("limit");
            MarkReservedWords("begin");
            MarkReservedWords("batch");
            MarkReservedWords("apply");
            MarkReservedWords("count");
            MarkReservedWords("min");
            MarkReservedWords("max");
            MarkReservedWords("sum");
            MarkReservedWords("avg");
            MarkReservedWords("in");
            //MarkReservedWords("or");
            MarkReservedWords("map");
            MarkReservedWords("table");
            MarkReservedWords("else");
            MarkReservedWords("switch");
            MarkReservedWords("case");
            MarkReservedWords("while");
            MarkReservedWords("do");
            MarkReservedWords("for");
            MarkReservedWords("get");
            MarkReservedWords("remove");
            MarkReservedWords("size");
            MarkReservedWords("clear");
            MarkReservedWords("contains");
            MarkReservedWords("List");
            MarkReservedWords("procedure");
            MarkReservedWords("call");
            MarkReservedWords("return");
            MarkReservedWords("lenght");
            MarkReservedWords("toUpperCase");
            MarkReservedWords("toLowerCase");
            MarkReservedWords("startsWith");
            MarkReservedWords("endsWith");
            MarkReservedWords("subString");
            MarkReservedWords("getYear");
            MarkReservedWords("getMonth");
            MarkReservedWords("getDay");
            MarkReservedWords("getHour");
            MarkReservedWords("getMinuts");
            MarkReservedWords("getSeconds");
            MarkReservedWords("Today");
            MarkReservedWords("Now");
            MarkReservedWords("break");
            MarkReservedWords("continue");
            MarkReservedWords("cursor");
            MarkReservedWords("is");
            MarkReservedWords("each");
            MarkReservedWords("open");
            MarkReservedWords("close");
            MarkReservedWords("log");
            MarkReservedWords("throw");
            MarkReservedWords("try");
            MarkReservedWords("catch");
            MarkReservedWords("TypeAlreadyExists");
            MarkReservedWords("TypeDontExists:");
            MarkReservedWords("BDAlreadyExists");
            MarkReservedWords("BDDontExists");
            MarkReservedWords("UseBDException");
            MarkReservedWords("TableAlreadyExists");
            MarkReservedWords("TableDontExists");
            MarkReservedWords("CounterTypeException");
            MarkReservedWords("UserAlreadyExists");
            MarkReservedWords("UserDontExists");
            MarkReservedWords("ValuesException");
            MarkReservedWords("ColumnException");
            MarkReservedWords("BatchException");
            MarkReservedWords("IndexOutException");
            MarkReservedWords("ArithmeticException");
            MarkReservedWords("NullPointerException");
            MarkReservedWords("NumberReturnsException");
            MarkReservedWords("FunctionAlreadyExists");
            MarkReservedWords("ProcedureAlreadyExists");
            MarkReservedWords("ObjectAlreadyExists");
            MarkReservedWords("default");
            #endregion

            #region VARIABLES_PRESERVADAS
            var nulo = ToTerm("null");
            var intt = ToTerm("int");
            var doublee = ToTerm("double");
            var boolean = ToTerm("boolean");
            var stringg = ToTerm("string");
            var date = ToTerm("date");
            var time = ToTerm("time");
            var verdadero = ToTerm("true");
            var falso = ToTerm("false");
            var create = ToTerm("create");
            var type = ToTerm("type");
            var iff = ToTerm("if");
            var nott = ToTerm("nott");
            var exists = ToTerm("exists");
            var neww = ToTerm("new");
            var alter = ToTerm("alter");
            var add = ToTerm("add");
            var delete = ToTerm("delete");
            var database = ToTerm("database");
            var use = ToTerm("use");
            var drop = ToTerm("drop");
            var counter = ToTerm("counter");
            var primary = ToTerm("primary");
            var key = ToTerm("key");
            var update = ToTerm("update");
            var truncate = ToTerm("truncate");
            var commit = ToTerm("commit");
            var rollback = ToTerm("rollback");
            var with = ToTerm("with");
            var password = ToTerm("password");
            var user = ToTerm("user");
            var grant = ToTerm("grant");
            var on = ToTerm("on");
            var revoke = ToTerm("revoke");
            var insert = ToTerm("insert");
            var into = ToTerm("into");
            var values = ToTerm("values");
            var set = ToTerm("set");
            var where = ToTerm("where");
            var from = ToTerm("from");
            var select = ToTerm("select");
            var order = ToTerm("order");
            var by = ToTerm("by");
            var asc = ToTerm("asc");
            var desc = ToTerm("desc");
            var limit = ToTerm("limit");
            var begin = ToTerm("begin");
            var batch = ToTerm("batch");
            var apply = ToTerm("apply");
            var count = ToTerm("count");
            var min = ToTerm("min");
            var max = ToTerm("max");
            var sum = ToTerm("sum");
            var avg = ToTerm("avg");
            var inn = ToTerm("in");
            //var or = ToTerm("or");
            var map = ToTerm("map");
            var table = ToTerm("table");
            var elsee = ToTerm("else");
            var switchh = ToTerm("switch");
            var casee = ToTerm("case");
            var whilee = ToTerm("while");
            var doo = ToTerm("do");
            var forr = ToTerm("for");
            var get = ToTerm("get");
            var remove = ToTerm("remove");
            var size = ToTerm("size");
            var clear = ToTerm("clear");
            var contains = ToTerm("contains");
            var list = ToTerm("list");
            var procedure = ToTerm("procedure");
            var call = ToTerm("call");
            var returnn = ToTerm("return");
            var length = ToTerm("length");
            var toUpperCase = ToTerm("toUpperCase");
            var toLowerCase = ToTerm("toLowerCase");
            var startsWith = ToTerm("startsWith");
            var endsWith = ToTerm("endsWith");
            var subString = ToTerm("subString");
            var getYear = ToTerm("getYear");
            var getMonth = ToTerm("getMonth");
            var getDay = ToTerm("getDay");
            var getHour = ToTerm("getHour");
            var getMinuts = ToTerm("getMinuts");
            var getSeconds = ToTerm("getSeconds");
            var today = ToTerm("Today");
            var now = ToTerm("Now");
            var breakk = ToTerm("break");
            var continuee = ToTerm("continue");
            var cursor = ToTerm("cursor");
            var iss = ToTerm("is");
            var each = ToTerm("each");
            var open = ToTerm("open");
            var close = ToTerm("close");
            var log = ToTerm("log");
            var throww = ToTerm("throw");
            var tryy = ToTerm("try");
            var catchh = ToTerm("catch");
            var TypeAlreadyExists = ToTerm("TypeAlreadyExists");
            var TypeDontExists =ToTerm("TypeDontExists");
            var BDAlreadyExists =ToTerm("BDAlreadyExists");
            var BDDontExists = ToTerm("BDDontExists");
            var UseBDException = ToTerm("UseBDException");
            var TableAlreadyExists = ToTerm("TableAlreadyExists");
            var TableDontExists = ToTerm("TableDontExists");
            var CounterTypeException = ToTerm("CounterTypeException");
            var UserAlreadyExists = ToTerm("UserAlreadyExists");
            var UserDontExists = ToTerm("UserDontExists");
            var ValuesException = ToTerm("ValuesException");
            var ColumnException = ToTerm("ColumnException");
            var BatchException = ToTerm("BatchException");
            var IndexOutException = ToTerm("IndexOutException");
            var ArithmeticException = ToTerm("ArithmeticException");
            var NullPointerException = ToTerm("NullPointerException");
            var NumberReturnsException = ToTerm("NumberReturnsException");
            var FunctionAlreadyExists = ToTerm("FunctionAlreadyExists");
            var ProcedureAlreadyExists = ToTerm("ProcedureAlreadyExists");
            var ObjectAlreadyExists = ToTerm("ObjectAlreadyExists");

            var defaultt = ToTerm("default");
            #endregion

            #region SIGNOS
            var allave = ToTerm("{");
            var cllave = ToTerm("}");
            var acorchete = ToTerm("[");
            var ccorchete = ToTerm("]");
            var aparentesis = ToTerm("(");
            var cparentesis = ToTerm(")");
            var dospuntos = ToTerm(":");
            var puntoycoma = ToTerm(";");
            var coma = ToTerm(",");
            var igual = ToTerm("=");
            var punto = ToTerm(".");
            var amperson = ToTerm("&");
            var menorq = ToTerm("<");
            var mayorq = ToTerm(">");
            var menorqigual = ToTerm("<=");
            var mayorqigual = ToTerm(">=");
            var igualigual = ToTerm("==");
            var diferente = ToTerm("!=");
            var or = ToTerm("||");
            var and = ToTerm("&&");
            var xor = ToTerm("^");
            var not = ToTerm("!");
            var por = ToTerm("*");
            var mas = ToTerm("+");
            var masUnario = ToTerm("+");
            var menos = ToTerm("-");
            var menosUnario = ToTerm("-");
            var potencia = ToTerm("**");
            var modulo = ToTerm("%");
            var division = ToTerm("/");
            var incremento = ToTerm("++");
            var decremento = ToTerm("--");
            var masigual = ToTerm("+=");
            var porigual = ToTerm("*=");
            var menosigual = ToTerm("-=");
            var dividirigual = ToTerm("/=");
            var interrogacion = ToTerm("?");

            #endregion

            #region NO_TERMINALES
            var INICIO = new NonTerminal("INICIO");
            var CUERPO = new NonTerminal("CUERPO");
            var SENTENCIA = new NonTerminal("SENTENCIA");
            var CREATETYPE = new NonTerminal("CREATETYPE");
            var TIPOS = new NonTerminal("TIPOS");
            var ITEMCREATETYPE = new NonTerminal("ITEMCREATETYPE");
            var OPCIONESITEMCREATETYPES = new NonTerminal("OPCIONESITEMCREATETYPES");
            var INSTANCIAOBJETO = new NonTerminal("INSTANCIAOBJETO");
            var LISTAVALORES = new NonTerminal("LISTAVALORES");
            var E = new NonTerminal("E");
            var ACCESOOBJETO = new NonTerminal("ACCESOOBJETO");
            var ACCESOASIGNACION = new NonTerminal("ACCESOASIGNACION");
            var ALTERTYPE = new NonTerminal("ALTERTYPE");
            var LISTAID = new NonTerminal("LISTAID");
            var ELIMINARUSERTYPE = new NonTerminal("ELIMINARUSERTYPE");

            var CREARDATABASE = new NonTerminal("CREARDATABASE");
            var USEE = new NonTerminal("USE");
            var DROPDATABASE = new NonTerminal("DROPDATABASE");
            var CREARTABLA = new NonTerminal("CREARTABLA");
            var DEFINICIONCOLUMNAS = new NonTerminal("DEFINICIONCOLUMNAS");
            var COLUMNAS = new NonTerminal("COLUMNAS");
            var ALTERTABLE = new NonTerminal("ALTERTABLE");
            var DROPTABLE = new NonTerminal("DROPTABLE");
            var TRUNCATEE = new NonTerminal("TRUNCATEE");

            var COMMITT = new NonTerminal("COMMITT");
            var ROLLBACKK = new NonTerminal("ROLLBACKK");

            var CREATEUSER = new NonTerminal("CREATEUSER");
            var GRANT = new NonTerminal("GRANT");
            var REVOKEE = new NonTerminal("REVOKEE");

            var INSERTARR = new NonTerminal("INSERTARR");
            var UPDATEE = new NonTerminal("UPDATEE");
            var LISTAASIGNACION = new NonTerminal("LISTAASIGNACION");
            var ITEMASIGNACION = new NonTerminal("ITEMASIGNACION");
            var DELETEE = new NonTerminal("DELETEE");
            var SELECTT = new NonTerminal("SELECTT");
            var ORDERBY = new NonTerminal("ORDERBY");
            var ORDEN = new NonTerminal("ORDEN");
            var OPCIONSELECT = new NonTerminal("OPCIONSELECT");
            var BATCHH = new NonTerminal("BATCHH");
            var CONTENIDOBATCHH = new NonTerminal("CONTENIDOBATCHH");
            var ITEMBATCH = new NonTerminal("ITEMBATCH");
            var FUNCIONAGREGACION = new NonTerminal("FUNCIONAGREGACION");
            var ITEMFAGREGACION = new NonTerminal("ITEMFAGREGACION");
            var LISTATIPOS = new NonTerminal("LISTATIPOS");


            var DECLARACION = new NonTerminal("DECLARACION");
            var LISTADECLARADA = new NonTerminal("LISTADECLARADA");
            var IFSTATEMENT = new NonTerminal("IFSTATEMENT");
            var IF_LISTA = new NonTerminal("IF_LISta");
            var STATEMENTBLOCK = new NonTerminal("STATEMENTBLOCK");
            var SWITCHSTATEMENT = new NonTerminal("SWITCHSTATEMENT");
            var SWITCHBLOCK = new NonTerminal("SWITCHBLOCK");
            var SWITCHBLOCKSTATEMENTGROUPS = new NonTerminal("SWITCHBLOCKSTATEMENTGROUPS");
            var SWITCHLABELS = new NonTerminal("SWITCHLABELS");
            var SWITCHBLOCKSTATEMENTGRO = new NonTerminal("SWITCHBLOCKSTATEMENTGRO");
            var SWITCHLABEL = new NonTerminal("SWITCHLABEL");
            var WHILEE = new NonTerminal("WHILEE");
            var DOWHILE = new NonTerminal("DOWHILE");
            var FORR = new NonTerminal("FORR");
            var INICIALIZACION = new NonTerminal("INICIALIZACION");
            var ACTUALIZACION = new NonTerminal("ACTUALIZACION");
            var MAPCOLLECTIONS = new NonTerminal("MAPCOLLECTIONS");
            var LISTCOLLECTIONS = new NonTerminal("LISTCOLLECTIONS");
            var SETCOLLECTIONS = new NonTerminal("SETCOLLECTIONS");
            var TIPOSPRIMITIVOS = new NonTerminal("TIPOSPRIMITIVOS");
            var FUNCIONESCOLLECTIONS = new NonTerminal("FUNCIONESCOLLECTIONS");
            var FUNCIONESMETODOS = new NonTerminal("FUNCIONESMETODOS");
            var PARAMETROS = new NonTerminal("PARAMETROS");
            var LLAMADASFUNCIONES = new NonTerminal("LLAMADASFUNCIONES");
            var PROCEDIMIENTOS = new NonTerminal("PROCEDIMIENTOS");
            var LISTAVARIABLES = new NonTerminal("LISTAVARIABLES");
            var FUNCIONESNATIVASCADENAS = new NonTerminal("FUNCIONESNATIVASCADENAS");
            var FUNCIONESNATIVASABSTRACCION = new NonTerminal("FUNCIONESNATIVASABSTRACCION");
            var SENTENCIATRANSFERENCIA = new NonTerminal("SENTENCIATRANSFERENCIA");
            var CURSORES = new NonTerminal("CURSORES");
            var TODOCONSULTAS = new NonTerminal("TODOCONSULTAS");
            var FOREACH_ACCESOCURSOR = new NonTerminal("FOREACH_ACCESOCURSOR");
            var LOG = new NonTerminal("LOG");
            var EXCEPCIONES = new NonTerminal("EXCEPCIONES");
            var TRYCATCHH = new NonTerminal("TRYCATCHH");
            var TIPOEXCEPCION = new NonTerminal("TIPOEXCEPCION");
            #endregion

            #region TERMINALES
            NumberLiteral numero = TerminalFactory.CreateCSharpNumber("numero");
            IdentifierTerminal id = TerminalFactory.CreateCSharpIdentifier("id");
            var tstring = new StringLiteral("tstring", "\"", StringOptions.AllowsAllEscapes);
            //BOOLEANO -----> verdadero, falso -----> YA ESTA
            //DATE TIME
            var tdatetime = new StringLiteral("tdatetime", "'", StringOptions.AllowsDoubledQuote);
            RegexBasedTerminal arroba = new RegexBasedTerminal("arroba", "@");
            #endregion

            #region GRAMATICA
            this.Root = INICIO;

            CUERPO.Rule = MakeStarRule(CUERPO, SENTENCIA);

            SENTENCIA.Rule = CREATETYPE + puntoycoma
                            | INSTANCIAOBJETO + puntoycoma
                            | ACCESOASIGNACION + puntoycoma
                            | ALTERTYPE + puntoycoma
                            | ELIMINARUSERTYPE + puntoycoma
                            | CREARDATABASE + puntoycoma
                            | USEE + puntoycoma
                            | DROPDATABASE + puntoycoma
                            | CREARTABLA + puntoycoma
                            | ALTERTABLE + puntoycoma
                            | DROPTABLE + puntoycoma
                            | TRUNCATEE + puntoycoma
                            | COMMITT + puntoycoma
                            | ROLLBACKK + puntoycoma
                            | CREATEUSER + puntoycoma
                            | GRANT + puntoycoma
                            | REVOKEE + puntoycoma

                            /*| INSERTARR + puntoycoma
                            | UPDATEE + puntoycoma
                            | DELETEE + puntoycoma
                            | SELECTT + puntoycoma*/
                            
                            | TODOCONSULTAS + puntoycoma

                            | BATCHH + puntoycoma
                            | FUNCIONAGREGACION + puntoycoma
                            | DECLARACION + puntoycoma 
                            | IFSTATEMENT
                            | SWITCHSTATEMENT
                            | WHILEE
                            | DOWHILE + puntoycoma
                            | FORR
                            | MAPCOLLECTIONS + puntoycoma
                            | LISTCOLLECTIONS + puntoycoma
                            | SETCOLLECTIONS + puntoycoma
                            | FUNCIONESMETODOS
                            | LLAMADASFUNCIONES
                            | PROCEDIMIENTOS
                            | SENTENCIATRANSFERENCIA + puntoycoma
                            | CURSORES + puntoycoma
                            | FOREACH_ACCESOCURSOR
                            | LOG + puntoycoma
                            | EXCEPCIONES + puntoycoma
                            | TRYCATCHH;

            TODOCONSULTAS.Rule = INSERTARR
                            | UPDATEE
                            | DELETEE
                            | SELECTT;

            STATEMENTBLOCK.Rule = allave + CUERPO + cllave;

            CREATETYPE.Rule = create + type + iff + not + exists + id + aparentesis + ITEMCREATETYPE  + cparentesis
                            | create + type + aparentesis + ITEMCREATETYPE + cparentesis;

            ITEMCREATETYPE.Rule = ITEMCREATETYPE + coma + OPCIONESITEMCREATETYPES
                                | OPCIONESITEMCREATETYPES;

            OPCIONESITEMCREATETYPES.Rule = id + TIPOS
                                         | TIPOS + id;

            INSTANCIAOBJETO.Rule = id + arroba + id //Estudiante @est;
                                 | arroba + id + igual + neww + id //@est = new Estudiante;
                                 | id + arroba + id +igual + neww + id //Estudiante @est2 = new Estudiante; 
                                 | id + arroba + id + igual + allave + LISTAVALORES + cllave //Estudiante @est3 = {201504481, “Julio Arango”}
                                 | arroba + id + igual + allave + LISTAVALORES + cllave; //@est3 = {201504481, “Julio Arango”}

            LISTAVALORES.Rule = LISTAVALORES + coma + E
                              | E;

            TIPOS.Rule = TIPOSPRIMITIVOS
                    | id
                    | counter
                    | map + menorq + LISTATIPOS + mayorq //para columnas de tablas
                    | map //para tipo normal variable
                    | set + menorq + LISTATIPOS + mayorq //para columnas de tablas
                    | list + menorq + TIPOS + mayorq //para columnas de tablas
                    | list;

            TIPOSPRIMITIVOS.Rule = intt
                    | stringg
                    | boolean
                    | doublee
                    | date
                    | time;

            LISTATIPOS.Rule = LISTATIPOS + coma + TIPOS
                            | TIPOS;

            ACCESOASIGNACION.Rule = ACCESOOBJETO
                                  | ACCESOOBJETO + igual + E;
       
            ACCESOOBJETO.Rule = arroba + id + punto + E
                              | arroba + id 
                              | LISTAVARIABLES
                              | id + punto + E;

            LISTAVARIABLES.Rule = LISTAVARIABLES + coma + arroba + id
                                | arroba + id;

            ALTERTYPE.Rule = alter + type + id + add + aparentesis + ITEMCREATETYPE + cparentesis
                           | alter + type + id + delete + aparentesis + LISTAID + cparentesis;

            LISTAID.Rule = LISTAID + coma + id
                         | id;

            ELIMINARUSERTYPE.Rule = delete + type + id;

            CREARDATABASE.Rule = create + database + iff + not + exists + id
                               | create + database + id;

            USEE.Rule = use + id;

            DROPDATABASE.Rule = drop + database + id;

            CREARTABLA.Rule = create + table + iff + not + exists + id + aparentesis + DEFINICIONCOLUMNAS + cparentesis
                            | create + table + id + aparentesis + DEFINICIONCOLUMNAS + cparentesis;

            DEFINICIONCOLUMNAS.Rule = DEFINICIONCOLUMNAS + coma + COLUMNAS
                                    | COLUMNAS;

            COLUMNAS.Rule = id + TIPOS + primary + key
                          | id + TIPOS
                          | primary + key + aparentesis + LISTAID + cparentesis;

            ALTERTABLE.Rule = alter + table + id + add + ITEMCREATETYPE
                            | alter + table + id + drop + ITEMCREATETYPE;

            DROPTABLE.Rule = drop + table + iff + exists + id
                           | drop + table + id;

            TRUNCATEE.Rule = truncate + table + id;

            COMMITT.Rule = commit;

            ROLLBACKK.Rule = rollback;

            CREATEUSER.Rule = create + user + id + with + password + E;

            GRANT.Rule = id + on + id;

            REVOKEE.Rule = revoke + id + on + id;


            INSERTARR.Rule = insert + into + id + values + aparentesis + LISTAVALORES + cparentesis
                           | insert + into + id + aparentesis + LISTAID + cparentesis + values + aparentesis + LISTAVALORES + cparentesis;

            UPDATEE.Rule = update + id + set + LISTAASIGNACION + where + E
                         | update + id + set + LISTAASIGNACION;

            LISTAASIGNACION.Rule = LISTAASIGNACION + coma + ITEMASIGNACION
                                 | ITEMASIGNACION;

            ITEMASIGNACION.Rule = ACCESOASIGNACION
                                 | id + igual + E;

            DELETEE.Rule = delete + from + id + where + E
                         | delete + from + id
                         | delete + id + acorchete + E + ccorchete+ from + id + where + E; //delete de un registro de un map

            SELECTT.Rule = select + aparentesis + OPCIONSELECT + cparentesis + from + id + where + E + ORDERBY + limit + E
                         | select + aparentesis + OPCIONSELECT + cparentesis + from + id + where + E + limit + E
                         | select + aparentesis + OPCIONSELECT + cparentesis + from + id + ORDERBY + limit + E
                         | select + aparentesis + OPCIONSELECT + cparentesis + from + id + where + E + ORDERBY
                         | select + aparentesis + OPCIONSELECT + cparentesis + from + id + ORDERBY
                         | select + aparentesis + OPCIONSELECT + cparentesis + from + id + where + E
                         | select + aparentesis + OPCIONSELECT + cparentesis + from + id + limit + E
                         | select + aparentesis + OPCIONSELECT + cparentesis + from + id;
            //WHERE userid IN (199, 200, 207); el IN es para las listas

            OPCIONSELECT.Rule = LISTAID
                              | por;

            ORDERBY.Rule = ORDERBY + coma + order + by + id + ORDEN
                         | order + by + id + ORDEN;

            ORDEN.Rule = asc
                       | desc;

            BATCHH.Rule = begin + batch + CONTENIDOBATCHH + apply + batch;

            CONTENIDOBATCHH.Rule = MakeStarRule(CONTENIDOBATCHH, ITEMBATCH);

            ITEMBATCH.Rule = INSERTARR
                           | UPDATEE
                           | DELETEE;

            FUNCIONAGREGACION.Rule = ITEMFAGREGACION + aparentesis + menorq + TODOCONSULTAS + mayorq + aparentesis;
            //van en E funciones de agragacion

            ITEMFAGREGACION.Rule = count
                                 | min
                                 | max
                                 | sum
                                 | avg;


            DECLARACION.Rule = TIPOS + LISTADECLARADA + igual + E
                             | TIPOS + LISTADECLARADA;

            LISTADECLARADA.Rule = LISTADECLARADA + coma + arroba + id
                                | arroba + id;

            IFSTATEMENT.Rule = IF_LISTA + elsee + STATEMENTBLOCK
                             | IF_LISTA;

            IF_LISTA.Rule = iff + aparentesis + E + cparentesis + STATEMENTBLOCK
                          | IF_LISTA + elsee + iff + aparentesis + E + cparentesis + STATEMENTBLOCK;


            SWITCHSTATEMENT.Rule = switchh + aparentesis + E + cparentesis + SWITCHBLOCK;

            SWITCHBLOCK.Rule = allave + SWITCHBLOCKSTATEMENTGROUPS + SWITCHLABELS + cllave
                             | allave + SWITCHBLOCKSTATEMENTGROUPS + cllave
                             | allave + SWITCHLABELS + cllave;

            SWITCHBLOCKSTATEMENTGROUPS.Rule = SWITCHBLOCKSTATEMENTGRO
                                            | SWITCHBLOCKSTATEMENTGROUPS + SWITCHBLOCKSTATEMENTGRO;

            SWITCHBLOCKSTATEMENTGRO.Rule = SWITCHLABELS + CUERPO;

            SWITCHLABELS.Rule = SWITCHLABEL
                              | SWITCHLABELS + SWITCHLABEL;

            SWITCHLABEL.Rule = casee + E + dospuntos
                             | defaultt + dospuntos;


            WHILEE.Rule = whilee + aparentesis + E + cparentesis + STATEMENTBLOCK;

            DOWHILE.Rule = doo + STATEMENTBLOCK + whilee + aparentesis + E + cparentesis;

            FORR.Rule = forr + aparentesis + INICIALIZACION + puntoycoma + E + ACTUALIZACION + cparentesis + STATEMENTBLOCK;

            INICIALIZACION.Rule = DECLARACION
                                | ACCESOASIGNACION;

            ACTUALIZACION.Rule = ACCESOASIGNACION
                                | arroba + id + incremento
                                | arroba + id + decremento;


            MAPCOLLECTIONS.Rule = map + arroba + id + igual + neww + map + menorq + TIPOSPRIMITIVOS + coma + TIPOS + mayorq
                                | map + arroba + id + igual + acorchete + E + ccorchete + puntoycoma;

            LISTCOLLECTIONS.Rule = list + arroba + id + igual + neww + list + menorq + TIPOS + mayorq
                                 | list + arroba + id + igual + acorchete + E + ccorchete;

            SETCOLLECTIONS.Rule = set + arroba + id + igual + neww + set + menorq + TIPOS + mayorq
                                | set + arroba + id + igual + acorchete + E + ccorchete;


            FUNCIONESCOLLECTIONS.Rule = insert + aparentesis + E + coma + E + cparentesis
                                      | insert + aparentesis + E + cparentesis
                                      | get + aparentesis + E + cparentesis
                                      | set + aparentesis + E + coma + E + cparentesis
                                      | remove + aparentesis + E + cparentesis
                                      | size + aparentesis + cparentesis
                                      | clear + aparentesis + cparentesis
                                      | contains + aparentesis + E + cparentesis;

            FUNCIONESMETODOS.Rule = TIPOS + id + aparentesis + PARAMETROS + cparentesis + STATEMENTBLOCK
                                  | TIPOS + id + aparentesis + cparentesis + STATEMENTBLOCK; ;

            PARAMETROS.Rule = PARAMETROS + coma + TIPOS + arroba + id
                           | TIPOS + arroba + id;

            LLAMADASFUNCIONES.Rule = id + aparentesis + E + cparentesis //llamadas funciones
                                   | call + id + aparentesis + LISTAVALORES + cparentesis; // llamadas a procedimientos

            PROCEDIMIENTOS.Rule = procedure + id + aparentesis + PARAMETROS + cparentesis + coma + aparentesis + PARAMETROS + cparentesis + STATEMENTBLOCK
                                | procedure + id + aparentesis + PARAMETROS + cparentesis + coma + aparentesis + cparentesis + STATEMENTBLOCK
                                | procedure + id + aparentesis + cparentesis + coma + aparentesis + PARAMETROS + cparentesis + STATEMENTBLOCK
                                | procedure + id + aparentesis + cparentesis + coma + aparentesis + cparentesis + STATEMENTBLOCK;


            FUNCIONESNATIVASCADENAS.Rule = length + aparentesis + cparentesis
                                         | toUpperCase + aparentesis + cparentesis
                                         | toLowerCase + aparentesis + cparentesis
                                         | startsWith + aparentesis + cparentesis
                                         | endsWith + aparentesis + cparentesis
                                         | subString + aparentesis + cparentesis;


            FUNCIONESNATIVASABSTRACCION.Rule = getYear + aparentesis + cparentesis
                                             | getMonth + aparentesis + cparentesis
                                             | getDay + aparentesis + cparentesis
                                             | getHour + aparentesis + cparentesis
                                             | getMinuts + aparentesis + cparentesis
                                             | getSeconds + aparentesis + cparentesis
                                             | today + aparentesis + cparentesis
                                             | now + aparentesis + cparentesis;


            SENTENCIATRANSFERENCIA.Rule = continuee
                                        | breakk
                                        | returnn
                                        | returnn + E;

            CURSORES.Rule = cursor + arroba + id + iss + TODOCONSULTAS
                          | open + arroba + id
                          | close + arroba + id;

            FOREACH_ACCESOCURSOR.Rule = forr + each + aparentesis + PARAMETROS + inn + id + STATEMENTBLOCK;

            LOG.Rule = log + aparentesis + E + cparentesis;

            EXCEPCIONES.Rule = throww + neww + TIPOEXCEPCION;

            TIPOEXCEPCION.Rule = TypeAlreadyExists
                               | TypeDontExists
                               | BDAlreadyExists
                               | BDDontExists
                               | UseBDException
                               | TableAlreadyExists
                               | TableDontExists
                               | CounterTypeException
                               | UserAlreadyExists
                               | UserDontExists
                               | ValuesException
                               | ColumnException
                               | BatchException
                               | IndexOutException
                               | ArithmeticException
                               | NullPointerException
                               | NumberReturnsException
                               | FunctionAlreadyExists
                               | ProcedureAlreadyExists
                               | ObjectAlreadyExists;

            TRYCATCHH.Rule = tryy + STATEMENTBLOCK + catchh + aparentesis + TIPOEXCEPCION + cparentesis + STATEMENTBLOCK;


            //falta meter todo lo de () {} []
            E.Rule = E + interrogacion + E + dospuntos + E
                | E + or + E
                | E + and + E
                | E + igualigual + E
                | E + diferente + E
                | E + mayorq + E
                | E + menorq + E
                | E + mayorqigual + E
                | E + menorqigual + E
                | E + mas + E
                | E + menos + E
                | E + por + E
                | E + division + E                
                | not + E
                | menosUnario + E
                | masUnario + E
                | E + coma + E
                | E + puntoycoma + E
                | E + incremento
                | E + decremento
                | aparentesis + E + cparentesis
                | acorchete + E + acorchete
                | allave + E + cllave
                | ACCESOOBJETO
                | FUNCIONESCOLLECTIONS
                | FUNCIONESNATIVASCADENAS
                | FUNCIONESNATIVASABSTRACCION
                | LLAMADASFUNCIONES
                | id
                | arroba + id
                | numero
                | tstring
                | tdatetime
                | falso
                | verdadero;
            #endregion


            #region PRECEDENCIA
            RegisterOperators(1, Associativity.Right, interrogacion, dospuntos);            
            RegisterOperators(2, Associativity.Left, or);                 //OR
            RegisterOperators(3, Associativity.Left, and);                 //AND
            RegisterOperators(4, Associativity.Left, xor);
            RegisterOperators(5, Associativity.Left, igualigual, diferente);           //IGUAL, DIFERENTE
            RegisterOperators(6, Associativity.Left, mayorq, menorq, menorqigual, mayorqigual); //MAYORQUES, MENORQUES
            RegisterOperators(7, Associativity.Left, mas, menos);             //MAS, MENOS
            RegisterOperators(8, Associativity.Left, por, division, modulo);             //POR, DIVIDIR
            RegisterOperators(9, Associativity.Left, masigual, menosigual);
            RegisterOperators(10, Associativity.Left, porigual, dividirigual);
            RegisterOperators(11, Associativity.Right, not);                 //NOT
            RegisterOperators(12, Associativity.Right, menosUnario);
            RegisterOperators(13, Associativity.Right, masUnario);
            RegisterOperators(14, Associativity.Left, incremento, decremento);
            
            #endregion
        }
    }
}
