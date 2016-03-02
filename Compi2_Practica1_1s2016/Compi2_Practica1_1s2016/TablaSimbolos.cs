using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compi2_Practica1_1s2016
{
    
    static class TablaSimbolos
    {
        public static List<Simbolo> todos;

        static public void CrearTablaSimbolos()
        {
             todos = new List<Simbolo>();
        }

        static public void insertarSimbolo(Simbolo nuevo)
        {
            todos.Add(nuevo);
        }

        static public void mostrarTodos()
        {
            for (int i = 0; i < todos.Count; i++)
            {
                if (todos[i].cadenaUnica == null)
                {
                    if (todos[i].min == null)
                    {
                        Nodo a = todos[i].regexRoot;
                        Nodo b = todos[i].regexFin;
                        MessageBox.Show(todos[i].nombre + "->" + a.numeroEstado + "->" + b.numeroEstado);
                    }
                    else
                    {
                        MessageBox.Show(todos[i].nombre + "//min = " + todos[i].min + ". Max = " + todos[i].max);
                    }
                }
                else
                {
                    MessageBox.Show(todos[i].nombre + "//Unica ->" + todos[i].cadenaUnica);
                }
                if (todos[i].afdInicio != null)
                {
                    MessageBox.Show("afd inicio-" + todos[i].afdInicio.nombre);
                }
            }
        }

        static public Simbolo buscarSim(String str)
        {
            for (int i = 0; i < todos.Count; i++)
            {
                if(todos[i].nombre.Equals(str)){
                    return todos[i];
                }
            }
            return null;
        }
    }
}
