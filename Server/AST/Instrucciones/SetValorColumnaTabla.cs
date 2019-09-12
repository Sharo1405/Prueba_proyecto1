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
    class SetValorColumnaTabla : Instruccion
    {
        public Columna col { get; set; }
        public ListaPuntos idExp { get; set; }
        public object valorParaAsignar { get; set; }
        public tipoDato tipoValorParaAsignar { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }


        tipoDato tipoFinal;
        LinkedList<Puntos> ListaExpresionesPuntos = new LinkedList<Puntos>();
        int contador = 0;
        object auxParaFunciones = new object();

        public SetValorColumnaTabla(Columna col, ListaPuntos idExp, object valorParaAsignar, tipoDato tipoValorParaAsignar,
            int linea, int colllll)
        {
            this.col = col;
            this.idExp = idExp;
            this.valorParaAsignar = valorParaAsignar;
            this.tipoValorParaAsignar = tipoValorParaAsignar;
            this.linea = linea;
            this.columna = colllll;
        }

        //int contador2 = 0;
        public object ejecutar(Entorno entorno, ErrorImpresion listas, Administrador management)
        {
            ListaExpresionesPuntos = new LinkedList<Puntos>();
            contador = 0;
            auxParaFunciones = new object();

            foreach (Puntos exp in idExp.ExpSeparadasPuntos)
            {
                Puntos exp2 = exp;
                if (exp.expresion1 is ListaPuntos)
                {
                    ListaPuntos x = (ListaPuntos)exp.expresion1;
                    LinkedList<Puntos> otraLista = (LinkedList<Puntos>)x.ExpSeparadasPuntos;
                    object result = masPuntos(entorno, listas, otraLista, contador);
                }
                else
                {
                    ListaExpresionesPuntos.AddLast(exp);
                }
            }
            //----------------------------------------------------------------------------------------


          
            int contador2 = 0;

            if (col.tipo == tipoDato.id)
            {
                String iditemType = "";
                Expresion posLista;
                Puntos idItem = ListaExpresionesPuntos.ElementAt(contador2);//0
                posLista = idItem;
                if (idItem.expresion1 is Identificador)
                {
                    Identificador ident = (Identificador)idItem.expresion1;
                    iditemType = ident.id;
                }
                else if (idItem.expresion1 is listaAccesoTabla)
                {
                    listaAccesoTabla lat = (listaAccesoTabla)idItem.expresion1;
                    iditemType = lat.idLista;
                    posLista = lat.index;
                }

                foreach (CreateType typeUser in col.valorColumna) {
                    contador2 = 0;
                    foreach (itemType item in typeUser.itemTypee)
                    {
                        if (item.id.ToLower().Equals(iditemType.ToLower()))
                        {
                            if (item.tipo.tipo == tipoDato.id)
                            {
                                //creattipe
                                if (contador2 == ListaExpresionesPuntos.Count - 1)
                                {
                                    if (valorParaAsignar is CreateType)
                                    {
                                        item.valor = valorParaAsignar;
                                    }
                                    else
                                    {
                                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                            "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                                        return tipoDato.errorSemantico;
                                    }
                                }
                                else
                                {
                                    contador2 = 1;
                                    mastypeusersss((CreateType) item.valor, entorno, listas, management, contador2);
                                }
                            }
                            else if (item.tipo.tipo == tipoDato.list || item.tipo.tipo == tipoDato.list)
                            {
                                if (contador2 == ListaExpresionesPuntos.Count - 1)
                                {
                                    if (idItem.expresion1 is Identificador)
                                    {
                                        if (valorParaAsignar is Lista)//tendria que ser de tipo list/set
                                        {
                                            if (((Lista)item.valor).tipoValor == ((Lista)valorParaAsignar).tipoValor)
                                            {
                                                item.valor = new object();
                                                item.valor = valorParaAsignar;
                                            }
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                    else if (idItem.expresion1 is listaAccesoTabla)
                                    {
                                        Lista valorExistente = (Lista)item.valor;
                                        List<object> lista = (List<object>)valorExistente.listaValores;
                                        object index = posLista.getValue(entorno, listas, management);
                                        tipoDato tipoIndex = posLista.getType(entorno, listas, management);
                                        if (tipoIndex == tipoDato.entero)
                                        {
                                            lista.RemoveAt(Convert.ToInt32(index));
                                            lista.Insert(Convert.ToInt32(index), valorParaAsignar);
                                        }
                                        else
                                        {
                                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                                "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                                            return tipoDato.errorSemantico;
                                        }
                                    }
                                }
                                else
                                {
                                    contador2++;
                                    Lista valorExistente = (Lista)item.valor;
                                    List<object> lista = (List<object>)valorExistente.listaValores;
                                    foreach (object dentroLista in lista)
                                    {
                                        if (dentroLista is CreateType)
                                        {
                                            mastypeusersss((CreateType)item.valor, entorno, listas, management, contador2);
                                        }
                                    }
                                }
                            }
                            else 
                            {
                                if (item.tipo.tipo == tipoValorParaAsignar)
                                {
                                    item.valor = valorParaAsignar;
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                        "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                                    return tipoDato.errorSemantico;
                                }
                            }
                        }
                    }
                }
            }
            else if (col.tipo == tipoDato.list || col.tipo == tipoDato.set)
            {
                //REVISAR

                if (contador2 == ListaExpresionesPuntos.Count - 1)
                {
                    if (idItem.expresion1 is Identificador)
                    {
                        if (valorParaAsignar is Lista)//tendria que ser de tipo list/set
                        {
                            if (((Lista)item.valor).tipoValor == ((Lista)valorParaAsignar).tipoValor)
                            {
                                item.valor = new object();
                                item.valor = valorParaAsignar;
                            }
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                            return tipoDato.errorSemantico;
                        }
                    }
                    else if (idItem.expresion1 is listaAccesoTabla)
                    {
                        Lista valorExistente = (Lista)item.valor;
                        List<object> lista = (List<object>)valorExistente.listaValores;
                        object index = posLista.getValue(entorno, listas, management);
                        tipoDato tipoIndex = posLista.getType(entorno, listas, management);
                        if (tipoIndex == tipoDato.entero)
                        {
                            lista.RemoveAt(Convert.ToInt32(index));
                            lista.Insert(Convert.ToInt32(index), valorParaAsignar);
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                            return tipoDato.errorSemantico;
                        }
                    }
                }
                else
                {
                    contador2++;
                    Lista valorExistente = (Lista)item.valor;
                    List<object> lista = (List<object>)valorExistente.listaValores;
                    foreach (object dentroLista in lista)
                    {
                        if (dentroLista is CreateType)
                        {
                            mastypeusersss((CreateType)item.valor, entorno, listas, management, contador2);
                        }
                    }
                }

                //REVISAR
            }
            else
            {
                object o = ListaExpresionesPuntos.ElementAt(contador2);

                if (tipoDato.counter == col.tipo)
                {
                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                        "No se puede actualizar un campo de tipo counter en la columna: " + col.idColumna));
                    return tipoDato.errorSemantico;
                }
                else
                {
                    if (col.tipo == tipoValorParaAsignar)
                    {
                        int cantValores = col.valorColumna.Count;
                        col.valorColumna.Clear();
                        for (int i = 0; i < cantValores; i++)
                        {
                            col.valorColumna.Add(valorParaAsignar);
                        }
                    }
                    else
                    {
                        listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                            "No se puede actualizar la columna porque no es del mismo tipo: " + col.idColumna));
                        return tipoDato.errorSemantico;
                    }
                }                
            }

            return tipoDato.ok;
        }


        
        public object mastypeusersss(CreateType masTypeUser, Entorno entorno, ErrorImpresion listas, Administrador management, int cuenta)
        {
            String iditemType = "";
            Expresion posLista;
            Puntos idItem = ListaExpresionesPuntos.ElementAt(cuenta);
            posLista = idItem;
            if (idItem.expresion1 is Identificador)
            {
                Identificador ident = (Identificador)idItem.expresion1;
                iditemType = ident.id;
            }
            else if (idItem.expresion1 is listaAccesoTabla)
            {
                listaAccesoTabla lat = (listaAccesoTabla)idItem.expresion1;
                iditemType = lat.idLista;
                posLista = lat.index;
            }

            foreach (itemType item in masTypeUser.itemTypee)
            {
                if (iditemType.ToLower().Equals(item.id.ToLower()))
                {
                    if (item.tipo.tipo == tipoDato.id)
                    {
                        //creattipe
                        if (cuenta == ListaExpresionesPuntos.Count - 1)
                        {
                            if (valorParaAsignar is CreateType)
                            {
                                item.valor = valorParaAsignar;
                            }
                            else
                            {
                                listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                    "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                                return tipoDato.errorSemantico;
                            }
                        }
                        else
                        {
                            cuenta++;
                            mastypeusersss((CreateType)item.valor, entorno, listas, management, cuenta);
                        }
                    }
                    else if (item.tipo.tipo == tipoDato.list || item.tipo.tipo == tipoDato.list)
                    {
                        if (cuenta == ListaExpresionesPuntos.Count - 1) {
                            if (idItem.expresion1 is Identificador)
                            {
                                if (valorParaAsignar is Lista)//tendria que ser de tipo list/set
                                {
                                    if (((Lista)item.valor).tipoValor== ((Lista)valorParaAsignar).tipoValor) {
                                        item.valor = new object();
                                        item.valor = valorParaAsignar;
                                    }
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                        "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                                    return tipoDato.errorSemantico;
                                }
                            }
                            else if (idItem.expresion1 is listaAccesoTabla)
                            {
                                Lista valorExistente = (Lista)item.valor;
                                List<object> lista = (List<object>)valorExistente.listaValores;
                                object index = posLista.getValue(entorno, listas, management);
                                tipoDato tipoIndex = posLista.getType(entorno, listas, management);
                                if (tipoIndex == tipoDato.entero) {
                                    lista.RemoveAt(Convert.ToInt32(index));
                                    lista.Insert(Convert.ToInt32(index), valorParaAsignar);
                                }
                                else
                                {
                                    listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                        "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                                    return tipoDato.errorSemantico;
                                }
                            }
                        }
                        else
                        {
                            cuenta++;
                            Lista valorExistente = (Lista)item.valor;
                            List<object> lista = (List<object>)valorExistente.listaValores;
                            foreach (object dentroLista in lista) {
                                if (dentroLista is CreateType) {
                                    mastypeusersss((CreateType)item.valor, entorno, listas, management, cuenta);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (item.tipo.tipo == tipoValorParaAsignar)
                        {
                            item.valor = valorParaAsignar;
                        }
                        else
                        {
                            listas.errores.AddLast(new NodoError(this.linea, this.columna, NodoError.tipoError.Semantico,
                                "No se puede actualizar el campo porque no es el mismo tipo en la: " + col.idColumna));
                            return tipoDato.errorSemantico;
                        }
                    }
                }
            }

            return null;
        }





        public object masPuntos(Entorno entorno, ErrorImpresion listas, LinkedList<Puntos> listapuntos, int contador)
        {
            foreach (Puntos exp in listapuntos)
            {
                if (exp.expresion1 is ListaPuntos)
                {
                    ListaPuntos x = (ListaPuntos)exp.expresion1;
                    LinkedList<Puntos> otraLista = (LinkedList<Puntos>)x.ExpSeparadasPuntos;
                    object result = masPuntos(entorno, listas, otraLista, contador);
                }
                else
                {
                    ListaExpresionesPuntos.AddLast(exp);
                }
            }
            return null;
        }
    }
}
