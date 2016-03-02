using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    class Token
    {
        public String nombre;
        public int regex, linea, columna;
        Object valor;

        static public List<Token> todosTokens = new List<Token>();

        public Token(String nombre, int linea, int columna, int regex)
        {
            this.nombre = nombre;
            this.linea = linea;
            this.columna = columna;
            Token.todosTokens.Add(this);
            this.regex = regex;
        }
    }
}
