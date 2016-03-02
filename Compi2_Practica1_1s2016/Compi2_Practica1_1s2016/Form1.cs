using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Parsing;
using Irony.Ast;
using System.IO;
using System.Diagnostics;

namespace Compi2_Practica1_1s2016
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        


        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            EditorTexto a = new EditorTexto();
            a.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TablaSimbolos.CrearTablaSimbolos();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Copia_de_EditorTexto b = new Copia_de_EditorTexto();
            b.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Copia_de_Copia_de_EditorTexto c = new Copia_de_Copia_de_EditorTexto();
            c.Show();
        }
    }
}
