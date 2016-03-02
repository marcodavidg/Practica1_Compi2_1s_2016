using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    class Conjunto
    {
        static public int contador = 0;
        public String cadena;
        public int nombre;
        public List<Transicion> transiciones;
        public bool aceptacion;
        static public List<Conjunto> todosConjuntosCerraduras = new List<Conjunto>();

        public Conjunto(String cadena)
        {
            aceptacion = false;
            transiciones = new List<Transicion>();
            this.nombre = Conjunto.contador;
            Conjunto.contador++;
            this.cadena = cadena;
            Conjunto.todosConjuntosCerraduras.Add(this);
        }

        public void agregarTransicion(Conjunto conj, ID_Cerradura id_cerradura)
        {
            transiciones.Add(new Transicion(conj, id_cerradura));
        }

        public void hacerDeAceptacion()
        {
            aceptacion = true;
        }
    }
}
