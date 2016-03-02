using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    public class Nodo
    {
        static int contador = 0;
        static public List<Nodo> todosLosNodos = new List<Nodo>();
        public List<Nodo> hijos;
        public List<Salto> saltos;
        public int numeroEstado;

        public Nodo()
        {
            numeroEstado = Nodo.contador;
            Nodo.contador++;
            hijos = new List<Nodo>();
            saltos = new List<Salto>();
            Nodo.todosLosNodos.Add(this);
        }

        public void agregarHijo(Nodo son)
        {
            hijos.Add(son);
        }

        public void agregarSalto(Salto jump)
        {
            saltos.Add(jump);
        }

        static public void resetearCounter()
        {
            Nodo.contador = 0;
        }

        static public int getCounterAndAdd()
        {
            Nodo.contador++;
            return Nodo.contador - 1;
        }

        static public Nodo getNodo(int i)
        {
            return Nodo.todosLosNodos[i];
        }
    }
}
