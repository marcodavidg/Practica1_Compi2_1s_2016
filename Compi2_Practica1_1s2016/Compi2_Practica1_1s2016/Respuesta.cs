using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    class Respuesta
    {
        public Nodo inicio, fin;
        public String id, min, max;
        public int tipo;
        public Respuesta(Nodo start, Nodo end, String id)
        {
            tipo = 0;
            inicio = start;
            fin = end;
            this.id = id;
        }
        public Respuesta(String min, String max)
        {
            tipo = 1;
            this.min = min;
            this.max = max;
            inicio = null;
            fin = null;
            id = null;
        }
    }
}
