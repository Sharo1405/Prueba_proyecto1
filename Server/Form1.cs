using System;
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
            Gramatica gram = new Gramatica();
            Parser parser = new Parser(gram);

            parser.Parse(this.textBox1.Text);
        }
    }
}
