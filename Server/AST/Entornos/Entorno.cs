﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.AST.Entornos
{
    class Entorno
    {
        public Entorno padreANTERIOR;
        public Dictionary<String, Simbolo> tablaS;

        public Entorno()
        {
            this.padreANTERIOR = null;
            this.tablaS = new Dictionary<string, Simbolo>();
        }


        public Entorno(Entorno antes)
        {
            this.padreANTERIOR = antes;
            this.tablaS = new Dictionary<string, Simbolo>();
        }



        public Simbolo getEnActual(String id)
        {
            try
            {
                Simbolo encontrado = new Simbolo();                
                if (tablaS.TryGetValue(id, out encontrado))
                {
                    return encontrado;
                }
            }
            catch (Exception e)
            {
                Console.Write("Error en la clase Entorno GetEnActual");
            }

            return null;
        }





        public Simbolo get(String id, Entorno actual)
        {
            try
            {

                for (Entorno e = actual; e != null; e = e.padreANTERIOR)
                {
                    Simbolo encontrado = new Simbolo();
                    if (e.tablaS.TryGetValue(id, out encontrado))                        
                    {
                        return encontrado;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Error en la clase Entorno get");
            }
            return null;
        }






        public void setSimbolo(String id, Simbolo nuevoSimbolo)
        {

            try
            {
                if (!this.tablaS.ContainsKey(id))
                {
                    this.tablaS.Add(id, nuevoSimbolo);
                }
            }
            catch (Exception e)
            {
            }
        }








        public void setValorSimbolo(String id, Object valorNuevo, Entorno lista)
        {
            try
            {
                for (Entorno e = lista; e != null; e = e.padreANTERIOR)
                {
                    Simbolo encontrado = new Simbolo();                    
                    if (e.tablaS.TryGetValue(id, out encontrado))
                    {
                        encontrado.valor = valorNuevo;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}
