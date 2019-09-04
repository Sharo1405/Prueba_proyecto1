using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.AST.BaseDatos;
using Server.AST.Entornos;
using Server.AST.Instrucciones;
using Server.AST.Otras;
using static Server.AST.Expresiones.Operacion;

namespace Server.AST.Expresiones
{
    class LLaveAsTypeUser : Expresion
    {
        public Expresion listaValores { get; set; } //debe ser una listaExpresiones
        public String tipoTypeUser { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }

        int contador = 0;

        public LLaveAsTypeUser(Expresion listaValores, String tipoTypeUser,
            int linea, int columna)
        {
            this.listaValores = listaValores;
            this.tipoTypeUser = tipoTypeUser;
            this.linea = linea;
            this.columna = columna;
        }

        public Operacion.tipoDato getType(Entorno entorno, ErrorImpresion listas,Administrador management)
        {
            return tipoDato.id;
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
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
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

        public object getValue(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            try
            {
                Simbolo s = entorno.get(tipoTypeUser.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                if (s != null)
                {
                    CreateType ll = (CreateType)s.valor;
                    CreateType lista = CreaNuevoType(ll, entorno, listas);
                    if (listaValores is ListaExpresiones)
                    {
                        LinkedList<Comas> objeto = (LinkedList<Comas>)listaValores.getValue(entorno, listas, management); //Lista comas
                        foreach (Comas coma in objeto)
                        {
                            Comas cv2 = (Comas)coma;
                            Object valo = cv2.getValue(entorno, listas, management);
                            tipoDato tipoVALO = cv2.getType(entorno, listas, management);
                            itemType itType = lista.itemTypee.ElementAt(contador);

                            if (itType.tipo.tipo == tipoDato.id)
                            {
                                Simbolo simi = new Simbolo();
                                if (valo is Simbolo)
                                {
                                    simi = (Simbolo)valo;
                                    valo = simi.valor;

                                    //se copia la misma lista porque ya viene asi
                                    if (tipoVALO == itType.tipo.tipo) {
                                        itType.valor = valo;
                                    }
                                }
                                else if (valo is Neww)
                                {
                                    Neww claseNeww = (Neww)valo;
                                    Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                    if (sim2 != null)
                                    {
                                        if (sim2.valor is CreateType)
                                        {
                                            CreateType ss = (CreateType)sim2.valor;
                                            CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                            itType.valor = lista2;
                                        }
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "El tipo de la variable del User type NO EXISTE. Tipos en cuestion: " + Convert.ToString(itType.tipo.id)));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                                else
                                {
                                    tipoDato tipoValue = cv2.getType(entorno, listas, management);
                                    if (tipoValue == itType.tipo.tipo)
                                    {
                                        if (valo is Simbolo)
                                        {
                                            itType.valor = ((Simbolo)valo).valor;
                                        }
                                        else
                                        {
                                            if (valo is List<object>)
                                            {
                                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                    "El tipo de la variable del no coincide con el experado, " + "esperado: " + Convert.ToString(tipoValue) +
                                                    "Viene: list/set"));
                                                return tipoDato.errorSemantico;
                                            }
                                            else
                                            {
                                                itType.valor = valo;
                                            }
                                            
                                        }
                                    }
                                    else if (tipoValue == tipoDato.nulo && itType.tipo.tipo == tipoDato.id)
                                    {
                                        itType.valor = valo;
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "El tipo de la variable del User type NO EXISTE."));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                            }
                            else if (itType.tipo.tipo == tipoDato.list)
                            {
                                Simbolo simi = new Simbolo();
                                if (valo is Simbolo)
                                {
                                    simi = (Simbolo)valo;
                                    valo = simi.valor;
                                    if (simi.tipo == tipoDato.list)
                                    {
                                        //se copia la misma lista porque ya viene asi
                                        itType.valor = valo;                                        
                                    }
                                    else
                                    {

                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "Los tipos para guardar no son iguales en UserType"));
                                        return tipoDato.errorSemantico;
                                        
                                    }
                                }
                                else if (valo is Neww)
                                {
                                    Neww v = (Neww)valo;
                                    tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                                    if (tt == tipoDato.ok)
                                    {
                                        //Tipo aux = new Tipo(tipoDato.set, tiposSet, linea, columna);

                                        Lista listaGuardar = new Lista("item", new List<Object>(), tipoDato.list, tiposSet, linea, columna);
                                        //listaRetorno.Add(listaGuardar);
                                        itType.valor = listaGuardar;
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "Tipos no valido en el valor de la lista"));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                                else if (cv2.expresion1 is Corchetes)
                                {
                                    if (valo is List<object>)
                                    {
                                        //listaRetorno = (List<object>)valo;
                                        Lista listaGuardar = new Lista("item", (List<object>)valo, tipoDato.list, cv2.getType(entorno, listas, management), linea, columna);
                                        //listaRetorno.Add(listaGuardar);
                                        itType.valor = listaGuardar;
                                    }
                                    else
                                    {
                                        List<Object> solo1 = new List<object>();
                                        solo1.Add(valo);
                                        Lista listaGuardar = new Lista("item", solo1, tipoDato.list, cv2.getType(entorno, listas, management), linea, columna);
                                        //listaRetorno.Add(listaGuardar);
                                        itType.valor = listaGuardar;
                                    }
                                }                                
                                else
                                {
                                    //listaRetorno.Add(valo);
                                    if (tipoVALO == itType.tipo.tipo) {
                                        itType.valor = valo;
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "Los tipos para guardar no son iguales en UserType"));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                            }
                            else if (itType.tipo.tipo == tipoDato.set)
                            {
                                Simbolo simi = new Simbolo();
                                if (valo is Simbolo)
                                {
                                    simi = (Simbolo)valo;
                                    valo = simi.valor;
                                    if (simi.tipo == tipoDato.set)
                                    {
                                        //se copia la misma lista porque ya viene asi
                                        itType.valor = valo;                                        
                                    }
                                    else
                                    {

                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "Los tipos para guardar no son iguales en UserType"));
                                        return tipoDato.errorSemantico;
                                        
                                    }
                                }
                                else if (valo is Neww)
                                {
                                    Neww v = (Neww)valo;
                                    tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                                    if (tt == tipoDato.ok)
                                    {
                                        //Tipo aux = new Tipo(tipoDato.set, tiposSet, linea, columna);
                                        Lista listaGuardar = new Lista("item", new List<Object>(), tipoDato.set, tiposSet, linea, columna);
                                        //listaRetorno.Add(listaGuardar);
                                        itType.valor = listaGuardar;
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "Tipos no valido en el valor del set"));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                                else if (cv2.expresion1 is Llaves)
                                {
                                    if (valo is List<object>)
                                    {
                                        //listaRetorno = (List<object>)valo;
                                        Lista listaGuardar = new Lista("item", (List<object>)valo, tipoDato.set, cv2.getType(entorno, listas, management), linea, columna);
                                        //listaRetorno.Add(listaGuardar);
                                        itType.valor = listaGuardar;
                                    }
                                    else
                                    {
                                        //listaRetorno.Add(listaRetorno);
                                        List<Object> solo1 = new List<object>();
                                        solo1.Add(valo);
                                        Lista listaGuardar = new Lista("item", solo1, tipoDato.set, cv2.getType(entorno, listas, management), linea, columna);
                                        //listaRetorno.Add(listaGuardar);
                                        itType.valor = listaGuardar;
                                    }
                                }
                                else
                                {
                                    //listaRetorno.Add(valo);
                                    itType.valor = valo;
                                }
                            }
                            else
                            {
                                tipoDato tipoValue = cv2.getType(entorno, listas, management);
                                Simbolo simi = new Simbolo();
                                if (valo is Simbolo)
                                {
                                    simi = (Simbolo)valo;
                                    valo = simi.valor;
                                    if (tipoValue == itType.tipo.tipo)
                                    {
                                        itType.valor = valo;
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "Los tipos para guardar no son iguales en UserType"));
                                        return tipoDato.errorSemantico;
                                    }
                                    
                                }
                                else if (tipoValue == itType.tipo.tipo)
                                {                                   
                                    if (valo is List<object>)
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "El tipo de la variable del no coincide con el experado, " + "esperado: " + Convert.ToString(tipoValue) +
                                            "Viene: list/set"));
                                        return tipoDato.errorSemantico;
                                    }
                                    else
                                    {
                                        itType.valor = valo;
                                    }
                                }
                                else if (tipoValue == tipoDato.nulo && (itType.tipo.tipo == tipoDato.cadena ||
                                    itType.tipo.tipo == tipoDato.date ||
                                    itType.tipo.tipo == tipoDato.time ||
                                    itType.tipo.tipo == tipoDato.list ||
                                    itType.tipo.tipo == tipoDato.set ||
                                    itType.tipo.tipo == tipoDato.id ||
                                    itType.tipo.tipo == tipoDato.map))
                                {
                                    itType.valor = valo;
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO es el mismo."));
                                    return tipoDato.errorSemantico;
                                }
                            }
                            contador++;
                        }
                    }
                    else
                    {
                        LinkedList<itemType> ty2 =  lista.itemTypee;
                        if (ty2.Count == 1)
                        {
                            foreach (itemType it in ty2)
                            {
                                object o = listaValores.getValue(entorno, listas, management);
                                tipoDato t = listaValores.getType(entorno, listas, management);
                                if (it.tipo.tipo == tipoDato.list || it.tipo.tipo == tipoDato.set)
                                {
                                    Simbolo simi = new Simbolo();
                                    if (o is Simbolo)
                                    {
                                        simi = (Simbolo)o;
                                        o = simi.valor;
                                        if (simi.tipo == tipoDato.list || simi.tipo == tipoDato.set)
                                        {
                                            //se copia la misma lista porque ya viene asi
                                            it.valor = o;                                            
                                        }
                                        else
                                        {

                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "Los tipos para guardar no son iguales en UserType"));
                                            return tipoDato.errorSemantico;
                                            
                                        }
                                    }
                                    else if (listaValores is Corchetes)
                                    {
                                        if (it.tipo.tipo != tipoDato.list)
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "Los tipos para guardar no son iguales en UserType"));
                                            return tipoDato.errorSemantico;
                                        }

                                        if (o is List<object>)
                                        {
                                            //listaRetorno = (List<object>)valo;
                                            Lista listaGuardar = new Lista("item", (List<object>)o, tipoDato.list, t, linea, columna);
                                            //listaRetorno.Add(listaGuardar);
                                            it.valor = listaGuardar;
                                        }
                                        else
                                        {
                                            List<Object> solo1 = new List<object>();
                                            solo1.Add(o);
                                            Lista listaGuardar = new Lista("item", solo1, tipoDato.list, t, linea, columna);
                                            //listaRetorno.Add(listaGuardar);
                                            it.valor = listaGuardar;
                                        }
                                    }
                                    else if (listaValores is Llaves)
                                    {
                                        if (it.tipo.tipo != tipoDato.set)
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "Los tipos para guardar no son iguales en UserType"));
                                            return tipoDato.errorSemantico;
                                        }
                                        if (o is List<object>)
                                        {
                                            Lista listaGuardar = new Lista("item", (List<object>)o, tipoDato.set, t, linea, columna);                                           
                                            it.valor = listaGuardar;
                                        }
                                        else
                                        {
                                            List<Object> solo1 = new List<object>();
                                            solo1.Add(o);
                                            Lista listaGuardar = new Lista("item", solo1, tipoDato.set, t, linea, columna);
                                            it.valor = listaGuardar;
                                        }
                                    }
                                    else if(listaValores is Neww)
                                    {
                                        Neww v = (Neww)o;
                                        tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                                        if (tt == tipoDato.ok)
                                        {
                                            if (v.tipoNew.tipo == it.tipo.tipo)
                                            {
                                                Lista listaGuardar = new Lista("item", new List<Object>(), v.tipoNew.tipo, tiposSet, linea, columna);
                                                it.valor = listaGuardar;
                                            }
                                            else
                                            {
                                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "Los tipos para guardar no son iguales en UserType"));
                                                return tipoDato.errorSemantico;
                                            }
                                            
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "Tipos no valido en el valor de la lista"));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    else
                                    {
                                        if (it.tipo.tipo == t)
                                        {
                                            it.valor = o;
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "Los tipos para guardar no son iguales en UserType"));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                }
                                else if (it.tipo.tipo == tipoDato.id)
                                {
                                    //it.valor = o;
                                    Simbolo simi = new Simbolo();
                                    if (o is Simbolo)
                                    {
                                        simi = (Simbolo)o;
                                        o = simi.valor;

                                        //se copia la misma lista porque ya viene asi
                                        if (it.id == simi.idTipo) {
                                            it.valor = o;
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "Los tipos no son iguales, no se pueden asignar"));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    else if (o is Neww)
                                    {
                                        Neww claseNeww = (Neww)o;
                                        Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                                        if (sim2 != null)
                                        {
                                            if (sim2.valor is CreateType)
                                            {
                                                CreateType ss = (CreateType)sim2.valor;
                                                CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                                it.valor = lista2;
                                            }
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "El tipo de la variable del User type NO EXISTE. Tipos en cuestion: " + Convert.ToString(it.tipo.id)));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    else
                                    {
                                        if (t == it.tipo.tipo)
                                        {
                                            if (o is Simbolo)
                                            {
                                                it.valor = ((Simbolo)o).valor;
                                            }
                                            else
                                            {
                                                if (o is List<object>)
                                                {
                                                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                        "El tipo de la variable del no coincide con el experado, " + "esperado: " + Convert.ToString(t)+
                                                        "Viene: list/set" ));
                                                    return tipoDato.errorSemantico;
                                                }
                                                else {
                                                    it.valor = o;
                                                }
                                            }
                                        }
                                        else if (t == tipoDato.nulo && it.tipo.tipo == tipoDato.id)
                                        {
                                            it.valor = o;
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                "El tipo de la variable del User type NO EXISTE."));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                }
                                else if (it.tipo.tipo == tipoDato.map)
                                {
                                    it.valor = null;
                                }
                                else
                                {
                                    if (it.tipo.tipo == t)
                                    {
                                        if (o is Simbolo)
                                        {
                                            it.valor = ((Simbolo)o).valor;
                                        }
                                        else
                                        {
                                            if (o is List<object>)
                                            {
                                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                                    "El tipo de la variable del no coincide con el experado, " + "esperado: " + Convert.ToString(t) +
                                                    "Viene: list/set"));
                                                return tipoDato.errorSemantico;
                                            }
                                            else
                                            {
                                                it.valor = o;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                            "Los tipos para guardar no son iguales en UserType"));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "Error hay mas argumentos para guardar y no vienen en la lista"));
                            return tipoDato.errorSemantico;
                        }
                        return lista;
                    }

                    return lista;
                }
                else
                {
                    listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                        "El typo no existe"));
                    return tipoDato.errorSemantico;
                }
            }
            catch (Exception e)
            {
                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                "TypeUser no valido para asignar"));
                return tipoDato.errorSemantico;
            }
            return tipoDato.id;
        }


        public tipoDato paraComasTYPEUSER(LinkedList<Comas> listComas, Entorno entorno, ErrorImpresion listas, LinkedList<itemType> itTypes,Administrador management)
        {
            foreach (Comas cv in listComas)
            {
                Comas cv2 = (Comas)cv;
                Object valo = cv2.getValue(entorno, listas, management);
                if (valo is Comas)
                {
                    LinkedList<Comas> listaaComas = (LinkedList<Comas>)valo;
                    paraComasTYPEUSER(listaaComas, entorno, listas, itTypes, management);
                }
                else
                {
                    itemType itType = itTypes.ElementAt(contador);

                    if (itType.tipo.tipo == tipoDato.id)
                    {
                        if (valo is Neww)
                        {
                            Neww claseNeww = (Neww)valo;
                            Simbolo sim2 = entorno.get(claseNeww.tipoNew.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                            if (sim2 != null)
                            {
                                if (sim2.valor is CreateType)
                                {
                                    CreateType ss = (CreateType)sim2.valor;
                                    CreateType lista2 = CreaNuevoType(ss, entorno, listas);
                                    itType.valor = lista2;
                                }
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO EXISTE."));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            tipoDato tipoValue = cv2.getType(entorno, listas, management);
                            if (tipoValue == itType.tipo.tipo)
                            {
                                itType.valor = valo;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO es el mismo."));
                                return tipoDato.errorSemantico;
                            }
                        }
                    }
                    else if (itType.tipo.tipo == tipoDato.list)
                    {
                        if (valo is Neww)
                        {
                            Neww v = (Neww)valo;
                            tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                            if (tt == tipoDato.ok)
                            {
                                //Tipo aux = new Tipo(tipoDato.set, tiposSet, linea, columna);
                                Lista listaGuardar = new Lista("item", new List<Object>(), tipoDato.list, tiposSet, linea, columna);
                                //listaRetorno.Add(listaGuardar);
                                itType.valor = listaGuardar;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "Tipos no valido en el valor de la lista"));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else if (cv2.expresion1 is Corchetes)
                        {
                            if (valo is List<object>)
                            {
                                //listaRetorno = (List<object>)valo;
                                Lista listaGuardar = new Lista("item", (List<object>)valo, tipoDato.list, cv2.getType(entorno, listas, management), linea, columna);
                                //listaRetorno.Add(listaGuardar);
                                itType.valor = listaGuardar;
                            }
                            else
                            {
                                List<Object> solo1 = new List<object>();
                                solo1.Add(valo);
                                Lista listaGuardar = new Lista("item", solo1, tipoDato.list, cv2.getType(entorno, listas, management), linea, columna);
                                //listaRetorno.Add(listaGuardar);
                                itType.valor = listaGuardar;
                            }
                        }
                        else
                        {
                            //listaRetorno.Add(valo);
                            itType.valor = valo;
                        }
                    }
                    else if (itType.tipo.tipo == tipoDato.set)
                    {
                        if (valo is Neww)
                        {
                            Neww v = (Neww)valo;
                            tipoDato tt = comprobandoTipos(entorno, listas, v.tipoNew);
                            if (tt == tipoDato.ok)
                            {
                                //Tipo aux = new Tipo(tipoDato.set, tiposSet, linea, columna);
                                Lista listaGuardar = new Lista("item", new List<Object>(), tipoDato.set, tiposSet, linea, columna);
                                //listaRetorno.Add(listaGuardar);
                                itType.valor = listaGuardar;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "Tipos no valido en el valor del set"));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else if (cv2.expresion1 is Llaves)
                        {
                            if (valo is List<object>)
                            {
                                //listaRetorno = (List<object>)valo;
                                Lista listaGuardar = new Lista("item", (List<object>)valo, tipoDato.set, cv2.getType(entorno, listas, management), linea, columna);
                                //listaRetorno.Add(listaGuardar);
                                itType.valor = listaGuardar;
                            }
                            else
                            {
                                //listaRetorno.Add(listaRetorno);
                                List<Object> solo1 = new List<object>();
                                solo1.Add(valo);
                                Lista listaGuardar = new Lista("item", solo1, tipoDato.set, cv2.getType(entorno, listas, management), linea, columna);
                                //listaRetorno.Add(listaGuardar);
                                itType.valor = listaGuardar;
                            }
                        }
                        else
                        {
                            //listaRetorno.Add(valo);
                            itType.valor = valo;
                        }
                    }
                    else
                    {
                        tipoDato tipoValue = cv2.getType(entorno, listas, management);
                        if (tipoValue == itType.tipo.tipo)
                        {
                            itType.valor = valo;
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                    "El tipo de la variable del User type NO EXISTE."));
                            return tipoDato.errorSemantico;
                        }
                    }

                }
                contador++;
            }

            return tipoDato.ok;
        }


        LinkedList<Tipo> tiposSet = new LinkedList<Tipo>();
        List<Object> listaRetorno = new List<object>();
        public tipoDato comprobandoTipos(Entorno entorno, ErrorImpresion listas, Tipo tipoVal)
        {
            if (tipoVal.tipoValor is Tipo)
            {
                tiposSet.AddLast(new Tipo(tipoVal.tipo, tipoVal.tipoValor, linea, columna));
                tipoDato t = comprobandoTipos(entorno, listas, tipoVal.tipoValor);
                if (t == tipoDato.errorSemantico)
                {
                    return tipoDato.errorSemantico;
                }
            }
            else
            {
                if (tipoVal.tipo == tipoDato.id)
                {
                    Simbolo sim = entorno.get(tipoVal.id.ToLower(), entorno, Simbolo.Rol.VARIABLE);
                    if (sim == null)
                    {
                        listas.errores.AddLast(new NodoError(linea, columna, NodoError.tipoError.Semantico,
                                "El tipo de la variable del set no existe. El nombre es: " + tipoVal.id));
                        return tipoDato.errorSemantico;
                    }
                }
                else if (tipoVal.tipo == tipoDato.booleano ||
                        tipoVal.tipo == tipoDato.cadena ||
                        tipoVal.tipo == tipoDato.date ||
                        tipoVal.tipo == tipoDato.decimall ||
                        tipoVal.tipo == tipoDato.entero ||
                        tipoVal.tipo == tipoDato.time)
                {
                    return tipoDato.ok;
                }
            }

            return tipoDato.ok;
        }

    }
}
