using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Windows.Forms;

namespace Compi2_Practica1_1s2016
{
    class Analizador
    {
        public bool isValid(String texto, Gramatica grammar)
        {
            LanguageData lenguaje = new LanguageData(grammar);
            Parser parser = new Parser(lenguaje);
            ParseTree tree = parser.Parse(texto);
            if (tree.Root == null)
            {
                #region reportes de errores
                tree = parser.Context.CurrentParseTree;
                parser.RecoverFromError();

                if (tree.ParserMessages.Count == 0)
                {
                    MessageBox.Show("no error", "fd");
                }
                foreach (var err in tree.ParserMessages)
                {
                    MessageBox.Show("Linea: " + err.Location.Line.ToString() + " Columna: " + err.Location.Column.ToString() + " Mensaje: " + err.Message + "\n\nInforme completo: " + err.ToString());
                }
                #endregion

                return false;
            }
            else
            {
                return true;
            }
        }
        public ParseTreeNode getRoot(string sourceCode, Grammar grammar)
        {

            LanguageData language = new LanguageData(grammar);

            Parser parser = new Parser(language);

            ParseTree parseTree = parser.Parse(sourceCode);

            ParseTreeNode root = parseTree.Root;

            return root;

        }
        public Object evaluar(ParseTreeNode node)
        {
            Object resultado = null;
            switch (node.Term.Name.ToString())
            {
                case "S":
                    MessageBox.Show("S");
                    break;
            }
            return resultado;
        }
    }
}
