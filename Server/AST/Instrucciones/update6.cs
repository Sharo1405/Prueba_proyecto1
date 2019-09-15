using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Expresiones;
using Server.AST.Expresiones.TipoDato;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Instrucciones
{
    class update6 : Instruccion
    {
        public String idTabla { get; set; }
        public LinkedList<NodoAST> asignaciones = new LinkedList<NodoAST>();
        public Expresion where { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        public update6(String idTabla, LinkedList<NodoAST> asignaciones,
            Expresion whersito, int linea, int columna)
        {
            this.idTabla = idTabla;
            this.asignaciones = asignaciones;
            this.where = whersito;
            this.linea = linea;
            this.columna = columna;
        }

        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Object inUse = management.getInUse();
                if (inUse != null)
                {
                    BaseDeDatos basee = (BaseDeDatos)inUse;
                    Tabla encontrado = new Tabla();
                    try
                    {
                        Tabla encontrado2 = new Tabla();
                        if (basee.Tabla.TryGetValue(idTabla.ToLower(), out encontrado2))
                        {
                            Entorno actual = new Entorno(entorno);

                            //declarar todas las variables 
                            foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                            {
                                //necesito nombre columna y tipo columna
                                LinkedList<String> ids = new LinkedList<string>();
                                ids.AddLast(kvp.Value.idColumna.ToLower());
                                Tipo tipoids = new Tipo();
                                tipoids = new Tipo(kvp.Value.tipo, this.linea, this.columna);
                                Declarcion decla = new Declarcion(tipoids, ids);
                                decla.ejecutar(actual, listas, management);
                            }


                            int contador = 0;
                            int cantValoresXcolumna = 1;
                            while (contador <= cantValoresXcolumna-1)
                            {
                                foreach (KeyValuePair<string, Columna> kvp in encontrado2.columnasTabla)
                                {
                                    String idsvar = kvp.Value.idColumna.ToLower();
                                    object valor = kvp.Value.valorColumna.ElementAt(contador);
                                    Expresion exp = devueleExpresionTipo(kvp.Value.tipo, valor);
                                    Asignacion asigna = new Asignacion(idsvar, exp, this.linea, this.columna);
                                    asigna.getValue(actual, listas, management);
                                    //hasta aqui ya estan los valores en la tabla de simbolos
                                    //ahora necesito ver el where si devuelve true debo guardar 
                                    if (contador == 0)
                                    {
                                        cantValoresXcolumna = kvp.Value.valorColumna.Count;
                                    }
                                }


                                object resultado = where.getValue(actual, listas, management);
                                tipoDato tipoResultado = where.getType(actual, listas, management);

                                if (tipoResultado == tipoDato.booleano)
                                {
                                    if ((Boolean)resultado)//si entra el where es el exxito
                                    {
                                        foreach (NodoAST asignar in asignaciones)
                                        {
                                            //---------------------------------------------------------------------------

                                            if (asignar is listaAccesoTabla)
                                            {
                                                //aqui lo de id[numero]
                                                listaAccesoTabla asi = (listaAccesoTabla)asignar;
                                                Columna coll = new Columna();
                                                if (encontrado2.columnasTabla.TryGetValue(asi.idLista.ToLower(), out coll))
                                                {
                                                    //--------------------------------------------------------------------------------
                                                    List<object> lista = new List<object>();
                                                    Lista listaGuardar = new Lista();
                                                    object index = asi.index.getValue(entorno, listas, management);
                                                    tipoDato tipoIndex = asi.index.getType(entorno, listas, management);

                                                    if (tipoIndex != tipoDato.entero)
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                            "El idice de la lista al que se desea acceder no es de tipo entero en la columna" + coll.idColumna));
                                                        return tipoDato.errorSemantico;
                                                    }

                                                    object valorComa = asi.igual.getValue(entorno, listas, management);
                                                    tipoDato tipoComa = asi.igual.getType(entorno, listas, management);
                                                    if (asi.igual is Corchetes)
                                                    {
                                                        if (valorComa is List<object>)
                                                        {
                                                            lista = (List<object>)valorComa;
                                                            listaGuardar = new Lista("", lista, tipoDato.list, tipoComa, this.linea, this.columna);
                                                        }
                                                        else
                                                        {
                                                            lista.Add(valorComa);
                                                            listaGuardar = new Lista("", lista, tipoDato.list, tipoComa, this.linea, this.columna);
                                                        }
                                                        tipoComa = tipoDato.list;
                                                        valorComa = listaGuardar;
                                                    }
                                                    else if (asi.igual is Llaves)
                                                    {
                                                        if (valorComa is List<object>)
                                                        {
                                                            lista = (List<object>)valorComa;
                                                            listaGuardar = new Lista("", lista, tipoDato.set, tipoComa, this.linea, this.columna);
                                                        }
                                                        else
                                                        {
                                                            lista.Add(valorComa);
                                                            listaGuardar = new Lista("", lista, tipoDato.set, tipoComa, this.linea, this.columna);
                                                        }
                                                        tipoComa = tipoDato.set;
                                                        valorComa = listaGuardar;
                                                    }
                                                    else if (valorComa is Neww)
                                                    {
                                                        Neww claseNeww = (Neww)valorComa;
                                                        if (claseNeww.tipoNew.tipo == tipoDato.id)
                                                        {
                                                            Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                                            if (sim2 != null)
                                                            {
                                                                if (sim2.valor is CreateType)
                                                                {
                                                                    CreateType ss = (CreateType)sim2.valor;
                                                                    CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                                                    valorComa = lista2;
                                                                }
                                                            }
                                                        }
                                                        else if (claseNeww.tipoNew.tipo == tipoDato.list || claseNeww.tipoNew.tipo == tipoDato.set)
                                                        {
                                                            listaGuardar = new Lista("", new List<Object>(), claseNeww.tipoNew.tipo, claseNeww.tipoNew.tipoValor.tipo, this.linea, this.columna);
                                                            valorComa = listaGuardar;
                                                        }
                                                    }

                                                    //---------------------------------------------------------------------------------

                                                    if (coll.primaryKey == true)
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                            "La columa a actualizar es llave primaria: " + coll.idColumna));
                                                        return tipoDato.errorSemantico;
                                                    }
                                                    int cantidadDatos = coll.valorColumna.Count;
                                                    if (tipoComa == coll.tipoValor)
                                                    {
                                                        Lista lis = (Lista)coll.valorColumna.ElementAt(contador);

                                                        lis.listaValores.RemoveAt(Convert.ToInt32(index));
                                                        lis.listaValores.Insert(Convert.ToInt32(index), valorComa);
                                                    }
                                                    else
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                            "El tipo del valor no es igual al tipo de la columna para actualizar, col:" + asi.idLista +
                                                            "tipo valor: " + Convert.ToString(tipoComa)));
                                                        return tipoDato.errorSemantico;
                                                    }
                                                }
                                            }
                                            else if (asignar is idigualEupdate)
                                            {
                                                idigualEupdate asi = (idigualEupdate)asignar;
                                                Columna coll = new Columna();
                                                if (encontrado2.columnasTabla.TryGetValue(asi.idCol.ToLower(), out coll))
                                                {
                                                    //--------------------------------------------------------------------------------
                                                    List<object> lista = new List<object>();
                                                    Lista listaGuardar = new Lista();
                                                    object valorComa = asi.igual.getValue(entorno, listas, management);
                                                    tipoDato tipoComa = asi.igual.getType(entorno, listas, management);
                                                    if (asi.igual is Corchetes)
                                                    {
                                                        if (valorComa is List<object>)
                                                        {
                                                            lista = (List<object>)valorComa;
                                                            listaGuardar = new Lista("", lista, tipoDato.list, tipoComa, this.linea, this.columna);
                                                        }
                                                        else
                                                        {
                                                            lista.Add(valorComa);
                                                            listaGuardar = new Lista("", lista, tipoDato.list, tipoComa, this.linea, this.columna);
                                                        }
                                                        tipoComa = tipoDato.list;
                                                        valorComa = listaGuardar;
                                                    }
                                                    else if (asi.igual is Llaves)
                                                    {
                                                        if (valorComa is List<object>)
                                                        {
                                                            lista = (List<object>)valorComa;
                                                            listaGuardar = new Lista("", lista, tipoDato.set, tipoComa, this.linea, this.columna);
                                                        }
                                                        else
                                                        {
                                                            lista.Add(valorComa);
                                                            listaGuardar = new Lista("", lista, tipoDato.set, tipoComa, this.linea, this.columna);
                                                        }
                                                        tipoComa = tipoDato.set;
                                                        valorComa = listaGuardar;
                                                    }
                                                    else if (valorComa is Neww)
                                                    {
                                                        Neww claseNeww = (Neww)valorComa;
                                                        if (claseNeww.tipoNew.tipo == tipoDato.id)
                                                        {
                                                            Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                                            if (sim2 != null)
                                                            {
                                                                if (sim2.valor is CreateType)
                                                                {
                                                                    CreateType ss = (CreateType)sim2.valor;
                                                                    CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                                                    valorComa = lista2;
                                                                }
                                                            }
                                                        }
                                                        else if (claseNeww.tipoNew.tipo == tipoDato.list || claseNeww.tipoNew.tipo == tipoDato.set)
                                                        {
                                                            listaGuardar = new Lista("", new List<Object>(), claseNeww.tipoNew.tipo, claseNeww.tipoNew.tipoValor.tipo, this.linea, this.columna);
                                                            valorComa = listaGuardar;
                                                        }
                                                    }

                                                    //---------------------------------------------------------------------------------

                                                    if (coll.primaryKey == true)
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                            "La columa a actualizar es llave primaria: " + coll.idColumna));
                                                        return tipoDato.errorSemantico;
                                                    }

                                                    int cantidadDatos = coll.valorColumna.Count;

                                                    if (asi.igual is Suma)
                                                    {
                                                        Suma plus = (Suma)asi.igual;
                                                        Identificador ident = (Identificador)plus.expresion1;
                                                        if (ident.id == coll.idColumna)
                                                        {
                                                            if (plus.expresion2 is Corchetes)
                                                            {
                                                                tipoComa = tipoDato.list;
                                                            }
                                                            else if (plus.expresion2 is Llaves)
                                                            {
                                                                tipoComa = tipoDato.set;
                                                            }
                                                            else
                                                            {
                                                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                                    "No se puede actualizar el campo porque no es el mismo tipo en la: " + coll.idColumna));
                                                                return tipoDato.errorSemantico;
                                                            }

                                                            object listaSetValores = plus.expresion2.getValue(entorno, listas, management);
                                                            tipoDato ltipo = plus.expresion2.getType(entorno, listas, management);

                                                            if (coll.tipoValor == ltipo)
                                                            {
                                                                if (coll.tipo == tipoComa)
                                                                {
                                                                    List<object> foreaachSET = (List<object>)listaSetValores;

                                                                    Lista ls = (Lista) coll.valorColumna.ElementAt(contador);
                                                                    foreach (object oo in foreaachSET)
                                                                    {
                                                                        ls.listaValores.Add(oo);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else if (asi.igual is Resta)
                                                    {
                                                        Resta plus = (Resta)asi.igual;
                                                        Identificador ident = (Identificador)plus.expresion1;
                                                        if (ident.id == coll.idColumna)
                                                        {
                                                            if (plus.expresion2 is Llaves)
                                                            {
                                                                tipoComa = tipoDato.set;
                                                            }
                                                            else
                                                            {
                                                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                                    "No se puede actualizar el campo porque no es el mismo tipo en la: " + coll.idColumna));
                                                                return tipoDato.errorSemantico;
                                                            }
                                                            object listaSetValores = plus.expresion2.getValue(entorno, listas, management);
                                                            tipoDato ltipo = plus.expresion2.getType(entorno, listas, management);

                                                            if (coll.tipoValor == ltipo)
                                                            {
                                                                if (coll.tipo == tipoComa)
                                                                {
                                                                    List<object> foreaach = (List<object>)coll.valorColumna; ;
                                                                    List<object> foreaachSET = (List<object>)listaSetValores;

                                                                    Lista ls = (Lista)coll.valorColumna.ElementAt(contador);
                                                                    foreach (object oo in foreaachSET)
                                                                    {
                                                                        foreach (object ooo in ls.listaValores)
                                                                        {
                                                                            if (ooo.Equals(oo))
                                                                            {
                                                                                ls.listaValores.Remove(oo);
                                                                                break;
                                                                            }
                                                                        }
                                                                    }                                                                    
                                                                }
                                                            }
                                                        }
                                                    }

                                                    else if (tipoComa == coll.tipo)
                                                    {                                                        
                                                        coll.valorColumna.RemoveAt(contador);
                                                        coll.valorColumna.Insert(contador, valorComa);
                                                    }
                                                    else
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                            "El tipo del valor no es igual al tipo de la columna para actualizar, col:" + asi.idCol +
                                                            "tipo valor: " + Convert.ToString(tipoComa)));
                                                        return tipoDato.errorSemantico;
                                                    }
                                                }

                                            }
                                            else if (asignar is idpuntoEigualEupdate)
                                            {

                                                idpuntoEigualEupdate asi = (idpuntoEigualEupdate)asignar;
                                                Columna coll = new Columna();
                                                if (encontrado2.columnasTabla.TryGetValue(asi.idCol.ToLower(), out coll))
                                                {
                                                    //object valor = asi.igual.getValue(entorno, listas, management);
                                                    //tipoDato tipoValor = asi.igual.getType(entorno, listas, management);

                                                    List<object> lista = new List<object>();
                                                    Lista listaGuardar = new Lista();
                                                    object valorComa = asi.igual.getValue(entorno, listas, management);
                                                    tipoDato tipoComa = asi.igual.getType(entorno, listas, management);
                                                    if (asi.igual is Corchetes)
                                                    {
                                                        if (valorComa is List<object>)
                                                        {
                                                            lista = (List<object>)valorComa;
                                                            listaGuardar = new Lista("", lista, tipoDato.list, tipoComa, this.linea, this.columna);
                                                        }
                                                        else
                                                        {
                                                            lista.Add(valorComa);
                                                            listaGuardar = new Lista("", lista, tipoDato.list, tipoComa, this.linea, this.columna);
                                                        }
                                                        tipoComa = tipoDato.list;
                                                        valorComa = listaGuardar;
                                                    }
                                                    else if (asi.igual is Llaves)
                                                    {
                                                        if (valorComa is List<object>)
                                                        {
                                                            lista = (List<object>)valorComa;
                                                            listaGuardar = new Lista("", lista, tipoDato.set, tipoComa, this.linea, this.columna);
                                                        }
                                                        else
                                                        {
                                                            lista.Add(valorComa);
                                                            listaGuardar = new Lista("", lista, tipoDato.set, tipoComa, this.linea, this.columna);
                                                        }
                                                        tipoComa = tipoDato.set;
                                                        valorComa = listaGuardar;
                                                    }
                                                    else if (valorComa is Neww)
                                                    {
                                                        Neww claseNeww = (Neww)valorComa;
                                                        if (claseNeww.tipoNew.tipo == tipoDato.id)
                                                        {
                                                            Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                                            if (sim2 != null)
                                                            {
                                                                if (sim2.valor is CreateType)
                                                                {
                                                                    CreateType ss = (CreateType)sim2.valor;
                                                                    CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                                                    valorComa = lista2;
                                                                }
                                                            }
                                                        }
                                                        else if (claseNeww.tipoNew.tipo == tipoDato.list || claseNeww.tipoNew.tipo == tipoDato.set)
                                                        {
                                                            listaGuardar = new Lista("", new List<Object>(), claseNeww.tipoNew.tipo, claseNeww.tipoNew.tipoValor.tipo, this.linea, this.columna);
                                                            valorComa = listaGuardar;
                                                        }
                                                    }
                                                    //-------------------------------------------------------------------------------------------------------------

                                                    if (asi.acceso is ListaPuntos)
                                                    {
                                                        if (asi.igual is Suma || asi.igual is Resta)
                                                        {
                                                            SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, (ListaPuntos)asi.acceso, this.linea, this.columna, asi.igual, contador);
                                                            svCol.ejecutar(entorno, listas, management);
                                                        }
                                                        else
                                                        {
                                                            SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, (ListaPuntos)asi.acceso, valorComa, tipoComa, this.linea, this.columna, contador);
                                                            svCol.ejecutar(entorno, listas, management);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (asi.acceso is Identificador)
                                                        {
                                                            Identificador id = (Identificador)asi.acceso;
                                                            ListaPuntos lpp = new ListaPuntos(id, this.linea, this.columna);
                                                            if (asi.igual is Suma || asi.igual is Resta)
                                                            {
                                                                SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, lpp, this.linea, this.columna, asi.igual, contador);
                                                                svCol.ejecutar(entorno, listas, management);
                                                            }
                                                            else
                                                            {
                                                                SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, lpp, valorComa, tipoComa, this.linea, this.columna, contador);
                                                                svCol.ejecutar(entorno, listas, management);
                                                            }
                                                        }
                                                        else if (asi.acceso is listaAccesoTabla)
                                                        {
                                                            listaAccesoTabla id = (listaAccesoTabla)asi.acceso;
                                                            ListaPuntos lpp = new ListaPuntos(id, this.linea, this.columna);
                                                            if (asi.igual is Suma || asi.igual is Resta)
                                                            {
                                                                SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, lpp, this.linea, this.columna, asi.igual, contador);
                                                                svCol.ejecutar(entorno, listas, management);
                                                            }
                                                            else
                                                            {
                                                                SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, lpp, valorComa, tipoComa, this.linea, this.columna, contador);
                                                                svCol.ejecutar(entorno, listas, management);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                                "El acceso a la columna en la tabla con el id: " + idTabla + " No es permitido"));
                                                            return tipoDato.errorSemantico;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (asignar is listaAccesoTablapuntoEigualE)
                                            {
                                                listaAccesoTablapuntoEigualE asi = (listaAccesoTablapuntoEigualE)asignar;
                                                Columna coll = new Columna();
                                                if (encontrado2.columnasTabla.TryGetValue(asi.idListaCol.ToLower(), out coll))
                                                {
                                                    List<object> lista = new List<object>();
                                                    Lista listaGuardar = new Lista();

                                                    object indice = asi.index.getValue(entorno, listas, management);
                                                    tipoDato tipoIndice = asi.index.getType(entorno, listas, management);

                                                    if (tipoIndice != tipoDato.entero)
                                                    {
                                                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                                "El acceso a la lista en la tabla con el id: " + idTabla + " En la columna" + asi.idListaCol + " No es permitido porque el tipo del indice no es valido"));
                                                        return tipoDato.errorSemantico;
                                                    }

                                                    object valorComa = asi.igual.getValue(entorno, listas, management);
                                                    tipoDato tipoComa = asi.igual.getType(entorno, listas, management);
                                                    if (asi.igual is Corchetes)
                                                    {
                                                        if (valorComa is List<object>)
                                                        {
                                                            lista = (List<object>)valorComa;
                                                            listaGuardar = new Lista("", lista, tipoDato.list, tipoComa, this.linea, this.columna);
                                                        }
                                                        else
                                                        {
                                                            lista.Add(valorComa);
                                                            listaGuardar = new Lista("", lista, tipoDato.list, tipoComa, this.linea, this.columna);
                                                        }
                                                        tipoComa = tipoDato.list;
                                                        valorComa = listaGuardar;
                                                    }
                                                    else if (asi.igual is Llaves)
                                                    {
                                                        if (valorComa is List<object>)
                                                        {
                                                            lista = (List<object>)valorComa;
                                                            listaGuardar = new Lista("", lista, tipoDato.set, tipoComa, this.linea, this.columna);
                                                        }
                                                        else
                                                        {
                                                            lista.Add(valorComa);
                                                            listaGuardar = new Lista("", lista, tipoDato.set, tipoComa, this.linea, this.columna);
                                                        }
                                                        tipoComa = tipoDato.set;
                                                        valorComa = listaGuardar;
                                                    }
                                                    else if (valorComa is Neww)
                                                    {
                                                        Neww claseNeww = (Neww)valorComa;
                                                        if (claseNeww.tipoNew.tipo == tipoDato.id)
                                                        {
                                                            Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                                            if (sim2 != null)
                                                            {
                                                                if (sim2.valor is CreateType)
                                                                {
                                                                    CreateType ss = (CreateType)sim2.valor;
                                                                    CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                                                    valorComa = lista2;
                                                                }
                                                            }
                                                        }
                                                        else if (claseNeww.tipoNew.tipo == tipoDato.list || claseNeww.tipoNew.tipo == tipoDato.set)
                                                        {
                                                            listaGuardar = new Lista("", new List<Object>(), claseNeww.tipoNew.tipo, claseNeww.tipoNew.tipoValor.tipo, this.linea, this.columna);
                                                            valorComa = listaGuardar;
                                                        }
                                                    }
                                                    //-------------------------------------------------------------------------------------------------------


                                                    if (asi.accesos is ListaPuntos)
                                                    {
                                                        if (asi.igual is Suma || asi.igual is Resta)
                                                        {
                                                            SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, (ListaPuntos)asi.accesos, this.linea, this.columna, Convert.ToInt32(indice), asi.igual, contador);
                                                            svCol.ejecutar(entorno, listas, management);
                                                        }
                                                        else
                                                        {
                                                            SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, (ListaPuntos)asi.accesos, valorComa, tipoComa, this.linea, this.columna, Convert.ToInt32(indice), contador);
                                                            svCol.ejecutar(entorno, listas, management);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (asi.accesos is Identificador)
                                                        {
                                                            Identificador id = (Identificador)asi.accesos;
                                                            ListaPuntos lpp = new ListaPuntos(id, this.linea, this.columna);
                                                            if (asi.igual is Suma || asi.igual is Resta)
                                                            {
                                                                SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, lpp, this.linea, this.columna, Convert.ToInt32(indice), asi.igual, contador);
                                                                svCol.ejecutar(entorno, listas, management);
                                                            }
                                                            else
                                                            {
                                                                SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, lpp, valorComa, tipoComa, this.linea, this.columna, Convert.ToInt32(indice), contador);
                                                                svCol.ejecutar(entorno, listas, management);
                                                            }
                                                        }
                                                        else if (asi.accesos is listaAccesoTabla)
                                                        {
                                                            listaAccesoTabla id = (listaAccesoTabla)asi.accesos;
                                                            ListaPuntos lpp = new ListaPuntos(id, this.linea, this.columna);
                                                            if (asi.igual is Suma || asi.igual is Resta)
                                                            {
                                                                SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, lpp, this.linea, this.columna, 
                                                                    Convert.ToInt32(indice), asi.igual, contador);
                                                                svCol.ejecutar(entorno, listas, management);
                                                            }
                                                            else
                                                            {
                                                                SetValorColumnaTablaWhere svCol = new SetValorColumnaTablaWhere(coll, lpp, valorComa, tipoComa, this.linea, 
                                                                    this.columna, Convert.ToInt32(indice), contador);
                                                                svCol.ejecutar(entorno, listas, management);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                                "El acceso a la columna en la tabla con el id: " + idTabla + " No es permitido"));
                                                            return tipoDato.errorSemantico;
                                                        }
                                                    }

                                                }
                                            }

                                            //---------------------------------------------------------------------------
                                        }
                                    }
                                }
                                contador++;
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "La tabla con el id: " + idTabla + " No existe"));
                            return tipoDato.errorSemantico;
                        }
                    }
                    catch (ArgumentException e)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "NO se puede realizar el Update6 en la tabla: " + idTabla));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "La base de datos EN USO no fue encontrada"));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede realizar el Update6 de la tabla" + idTabla));
                return tipoDato.errorSemantico;
            }
            return tipoDato.ok;
        }


        public CreateType CreaNuevoType(CreateType existente, Entorno entorno, ErrorImpresion listas)
        {
            CreateType nuevo = new CreateType();
            nuevo.idType = existente.idType;
            nuevo.ifnotexists = existente.ifnotexists;
            nuevo.linea = existente.linea;
            nuevo.columna = existente.columna;

            foreach (itemType var in existente.itemTypee)
            {
                itemType i = new itemType();
                i.id = var.id;
                i.tipo = var.tipo;
                i.valor = new Object();
                if (var.tipo.tipo == tipoDato.id)
                {
                    Simbolo buscado2 = entorno.get(var.tipo.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                    if (buscado2 != null)
                    {
                        //arreglar solo declarar sin el clonar
                        //CreateType typeComoTipo =(CreateType) buscado2.valor;
                        i.valor = null;//typeComoTipo.Clone();
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                            "El tipo de esa variable del Type User no existe: Tipo:" + var.tipo.id.ToLower()));
                    }
                }
                else if (var.tipo.tipo == tipoDato.entero)
                {
                    var.valor = 0;
                }
                else if (var.tipo.tipo == tipoDato.decimall)
                {
                    var.valor = 0.0;
                }
                else if (var.tipo.tipo == tipoDato.booleano)
                {
                    var.valor = false;
                }
                else
                {
                    var.valor = null;
                }

                nuevo.itemTypee.AddLast(i);
            }
            return nuevo;
        }


        public Expresion devueleExpresionTipo(tipoDato tipo, object valor)
        {
            switch (tipo)
            {
                case tipoDato.booleano:
                    return new Booleano(valor, tipo, this.linea, this.columna);

                case tipoDato.cadena:
                    return new cadena(valor, tipo, this.linea, this.columna);

                case tipoDato.date:
                    return new Date(valor, tipo, this.linea, this.columna);

                case tipoDato.decimall:
                    return new Numero(valor, tipo, this.linea, this.columna);

                case tipoDato.entero:
                    return new Numero(valor, tipo, this.linea, this.columna);

                case tipoDato.id:
                    //return new Identificador(Convert.ToString(valor),this.linea, this.columna);
                    return new TypeBase(valor, this.linea, this.columna);

                case tipoDato.nulo:
                    return new Nulo(valor, tipo, this.linea, this.columna);

                case tipoDato.time:
                    return new Time(valor, tipo, this.linea, this.columna);

                case tipoDato.set:
                    return new SetBase(valor, this.linea, this.columna);

                case tipoDato.list:
                    return new ListsBase(valor, this.linea, this.columna);
            }

            return null;
        }
    }
}
