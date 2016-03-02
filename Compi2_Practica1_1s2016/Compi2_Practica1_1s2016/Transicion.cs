using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    class Transicion
    {
        public Conjunto conjunto;
        public ID_Cerradura terminal;

        public Transicion(Conjunto conj, ID_Cerradura terminal)
        {
            this.conjunto = conj;
            this.terminal = terminal;
        }
    }
}
