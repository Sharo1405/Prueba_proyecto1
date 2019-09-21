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
    class update4 : Instruccion
    {
        public String idTabla { get; set; }
        public LinkedList<NodoAST> asignaciones = new LinkedList<NodoAST>();
        public int linea { get; set; }
        public int columna { get; set; }


        public update4(String idTabla, LinkedList<NodoAST> asignaciones,
            int linea, int columna)
        {
            this.idTabla = idTabla;
            this.asignaciones = asignaciones;
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
                            foreach (NodoAST asignar in asignaciones)
                            {

                                if (asignar is listaAccesoTabla)
                                {
                                    //aqui
                                    //lo de id[numero]
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

                                        if (coll.tipo == tipoDato.counter)
                                        {
                                            listas.impresiones.AddLast("WARNNING!! NO SE PUEDE ACTUALIZAR UN DATO DE TIPO COUNTER: " + idTabla
                                                + " Linea/Columna "
                                                                + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                                            return TipoExcepcion.excep.CounterTypeException;
                                        }
                                        else if (coll.primaryKey == true)
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                "La columa a actualizar es llave primaria: " + coll.idColumna));
                                            return tipoDato.errorSemantico;
                                        }
                                        int cantidadDatos = coll.valorColumna.Count;
                                        if (tipoComa == coll.tipoValor)
                                        {
                                            List<object> l = (List<object>)coll.valorColumna;
                                            foreach (Lista lis in l)
                                            {
                                                lis.listaValores.RemoveAt(Convert.ToInt32(index));
                                                lis.listaValores.Insert(Convert.ToInt32(index), valorComa);
                                            }
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


                                        if (coll.tipo == tipoDato.counter)
                                        {
                                            listas.impresiones.AddLast("WARNNING!! NO SE PUEDE ACTUALIZAR UN DATO DE TIPO COUNTER: " + idTabla + " Linea/Columna "
                                                                + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                                            return TipoExcepcion.excep.CounterTypeException;
                                        }
                                        else if(coll.primaryKey == true)
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                "La columa a actualizar es llave primaria: "+ coll.idColumna));
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
                                                        List<object> foreaach = (List<object>)coll.valorColumna; 
                                                        List<object> foreaachSET = (List<object>)listaSetValores;

                                                        foreach (object o in foreaach)
                                                        {
                                                            Lista ls = (Lista)o;
                                                            foreach (object oo in foreaachSET)
                                                            {
                                                                ls.listaValores.Add(oo);                                                                     
                                                            }
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

                                                        foreach (object o in foreaach)
                                                        {
                                                            Lista ls = (Lista)o;
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

                                        }

                                        else if (tipoComa == coll.tipo) {



                                            coll.valorColumna.Clear();

                                            for (int i = 0; i < cantidadDatos; i++)
                                            {
                                                coll.valorColumna.Add(valorComa);
                                            }
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
                                                SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, (ListaPuntos)asi.acceso, this.linea, this.columna, asi.igual);
                                                svCol.ejecutar(entorno, listas, management);
                                            }
                                            else
                                            {
                                                SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, (ListaPuntos)asi.acceso, valorComa, tipoComa, this.linea, this.columna);
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
                                                    SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, this.linea, this.columna, asi.igual);
                                                    svCol.ejecutar(entorno, listas, management);
                                                }
                                                else
                                                {
                                                    SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, valorComa, tipoComa, this.linea, this.columna);
                                                    svCol.ejecutar(entorno, listas, management);
                                                }
                                            } 
                                            else if (asi.acceso is listaAccesoTabla)
                                            {
                                                listaAccesoTabla id = (listaAccesoTabla)asi.acceso;
                                                ListaPuntos lpp = new ListaPuntos(id, this.linea, this.columna);
                                                if (asi.igual is Suma || asi.igual is Resta)
                                                {
                                                    SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, this.linea, this.columna, asi.igual);
                                                    svCol.ejecutar(entorno, listas, management);
                                                }
                                                else
                                                {
                                                    SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, valorComa, tipoComa, this.linea, this.columna);
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
                                                SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, (ListaPuntos)asi.accesos, this.linea, this.columna, Convert.ToInt32(indice), asi.igual);
                                                svCol.ejecutar(entorno, listas, management);
                                            }
                                            else
                                            {
                                                SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, (ListaPuntos)asi.accesos, valorComa, tipoComa, this.linea, this.columna, Convert.ToInt32(indice));
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
                                                    SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, this.linea, this.columna, Convert.ToInt32(indice), asi.igual);
                                                    svCol.ejecutar(entorno, listas, management);
                                                }
                                                else
                                                {
                                                    SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, valorComa, tipoComa, this.linea, this.columna, Convert.ToInt32(indice));
                                                    svCol.ejecutar(entorno, listas, management);
                                                }
                                            }
                                            else if (asi.accesos is listaAccesoTabla)
                                            {
                                                listaAccesoTabla id = (listaAccesoTabla)asi.accesos;
                                                ListaPuntos lpp = new ListaPuntos(id, this.linea, this.columna);
                                                if (asi.igual is Suma || asi.igual is Resta)
                                                {
                                                    SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, this.linea, this.columna, Convert.ToInt32(indice), asi.igual);
                                                    svCol.ejecutar(entorno, listas, management);
                                                }
                                                else
                                                {
                                                    SetValorColumnaTabla svCol = new SetValorColumnaTabla(coll, lpp, valorComa, tipoComa, this.linea, this.columna, Convert.ToInt32(indice));
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
                            }
                        }
                        else
                        {
                            listas.impresiones.AddLast("WARNNING!! ESA TABLA NO EXISTE: " + idTabla + " Linea/Columna "
                                                                + Convert.ToString(this.linea) + " " + Convert.ToString(this.col));
                            return TipoExcepcion.excep.TableDontExists;
                        }
                    }
                    catch (ArgumentException e)
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "NO se puede realizar el Update4 en la tabla: " + idTabla));
                        return tipoDato.errorSemantico;
                    }
                }
                else
                {
                    listas.impresiones.AddLast("WARNINGGGGGGGGGGGGGGGGGGGG!!!!!!!!!!!  La base de datos EN USO no fue encontrada " + " Linea/Columna "
                               + Convert.ToString(this.linea) + " " + Convert.ToString(this.columna));
                    return TipoExcepcion.excep.UseBDException;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede realizar el Update4 de la tabla" + idTabla));
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
    }
}
