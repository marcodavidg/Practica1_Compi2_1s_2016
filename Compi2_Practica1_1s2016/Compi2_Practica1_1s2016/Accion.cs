using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Compi2_Practica1_1s2016
{
    //clase utilizada para asociar acciones a una gramatica. simplemente se puede copiar
    // y pegar ya que solo se llama en la clase asociada a las acciones de nuestra gramatica.
    interface Accion
    {
        Object hacerAccion(ParseTreeNode nodo);
        Object action   (ParseTreeNode pt_node);
    }

}
