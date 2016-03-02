using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    public class Caracter
    {
        public int linea, columna;
        public String id;

        public Caracter(String id, int linea, int columna)
        {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }
    }
}
