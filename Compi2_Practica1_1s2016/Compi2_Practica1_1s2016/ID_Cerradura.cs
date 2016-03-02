using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    class ID_Cerradura
    {
        public String idUnico, min, max;

        public ID_Cerradura(String idUnico)
        {
            this.idUnico = idUnico;
            min = null;
            max = null;
        }

        public ID_Cerradura(String min, String max)
        {
            idUnico = null;
            this.min = min;
            this.max = max;
        }
    }
}
