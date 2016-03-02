using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    public class Salto
    {
        public int Tipo;
        public String caracter, inicio, final;
        public Nodo destino;

        public Salto(int Tipo, String caracter, String inicio, String final, Nodo destino)
        {
            this.Tipo = Tipo;
            this.caracter = caracter;
            this.inicio = inicio;
            this.final = final;
            this.destino = destino;
        }

        public void agregarDestino(Nodo destino)
        {
            this.destino = destino;
        }
    }
}
