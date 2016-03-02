using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compi2_Practica1_1s2016
{
    class Simbolo
    {
        public Conjunto afdInicio;
        public Nodo regexRoot, regexFin;
        public String nombre, cadenaUnica, nombre2;
        public String min, max, tipoDeDato;
        public int tipo; // 0 = CONJ, 1 = regex

        public Simbolo(String nombre, int tipo)
        {
            afdInicio = null;
            cadenaUnica = null;
            regexFin = null;
            regexRoot = null;
            this.nombre = nombre;
            nombre2 = nombre;
            this.tipo = tipo;
        }

        public void agregarRoot(Nodo regexRoot, Nodo regexFin)
        {
            this.regexRoot = regexRoot;
            this.regexFin = regexFin;
        }

        public void agregarCadenaUnica(String specialString)
        {
            cadenaUnica = specialString;
        }

        public void agregarMinMax(String min, String max)
        {
            this.min = min;
            this.max = max;
        }

        public void agregarDato(String tipoDato)
        {
            tipoDeDato = tipoDato;
        }
    }

}
