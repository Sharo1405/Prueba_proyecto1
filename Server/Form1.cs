﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Irony.Ast;
using Irony.Parsing;
using Server.Analizador;
using Server.Analizador_CHISON;
using Server.AST;
using Server.GenerarAST;

namespace Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Iniciar iniciar = new Iniciar();
            ErrorImpresion listas =  iniciar.analizar(this.richTextBox1.Text);

            if (listas != null) {
                foreach (string log in listas.impresiones)
                {
                    richTextBox2.Text += log + "\n";
                }

                foreach (NodoError log in listas.errores)
                {
                    richTextBox2.Text += log.descripcion + " " + Convert.ToString(log.fila) + ", " + Convert.ToString(log.columna) + "\n";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Gramatica gramatica = new Gramatica();
            Parser parser = new Parser(gramatica);

            ParseTree arbol = parser.Parse(richTextBox1.Text);
            ParseTreeNode raiz = arbol.Root;

            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {
                //---------------------> Hay Errores  
                MessageBox.Show("Hay Errores" + arbol.ParserMessages.ToString());

                return;
            }
            //---------------------> Todo Bien
            GraficadorTree g = new GraficadorTree();
            g.graficar(arbol);
            g.abrirArbol(g.desktop + "\\Files\\Arbol\\arbol.png");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Iniciar iniciar = new Iniciar();
            ErrorImpresion listas = iniciar.analizarCHISON(this.richTextBox3.Text);

            if (listas != null)
            {

                richTextBox2.Text += "----------CHISON------------------";
                foreach (string log in listas.impresiones)
                {
                    richTextBox2.Text += log + "\n";
                }

                foreach (NodoError log in listas.errores)
                {
                    richTextBox2.Text += log.descripcion + " " + Convert.ToString(log.fila) + ", " + Convert.ToString(log.columna) + "\n";
                }
            }
        }
    }
}
