using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;
using System.Windows.Forms;

namespace Compi2_Practica1_1s2016
{
    //hereda de la clase Accion
    class AccionesGramatica
    {
        public Nodo finCopia;
        public String cad;
        public List<ID_Cerradura> todosIDCerradura;
        public List<int> graphRelated;
        public List<Conjunto> conjuntosCerraduras;
        public List<Nodo> listaCopiaAux;
        public List<int> auxiliarCerradura;

        public Object hacerAccion(ParseTreeNode nodo)
        {
            return action(nodo, null);
        }

        public Object action(ParseTreeNode node, Respuesta root)
        {
            Object result = null;

            switch (node.Term.Name.ToString())
            {
                case "S":
                    TablaSimbolos.todos.Clear();
                    action(node.ChildNodes[0], root);
                    break;
                case "E":
                    action(node.ChildNodes[1], root);
                    break;
                case "B":
                    if (node.ChildNodes[0].Term.Name.ToString().Equals("B"))
                    {
                        action(node.ChildNodes[0], root);
                        action(node.ChildNodes[1], root);
                    }
                    else
                    {
                        action(node.ChildNodes[0], root);
                    }
                    break;
                case "C":
                    switch (node.ChildNodes.Count)
                    {
                        case 5:
                            Respuesta resTemp = (Respuesta)action(node.ChildNodes[4], root);
                            Respuesta id = (Respuesta)action(node.ChildNodes[2], root);
                            Simbolo simNuevo = new Simbolo(id.id, 0);
                            if (resTemp.inicio == null && resTemp.fin == null)
                            {
                                if (resTemp.min == null)
                                {
                                    simNuevo.agregarCadenaUnica(id.id);
                                }
                                else
                                {
                                    simNuevo.agregarMinMax(resTemp.min, resTemp.max);
                                }
                            }
                            else
                            {
                                graficar(resTemp, id.id);
                                simNuevo.agregarRoot(resTemp.inicio, resTemp.fin);
                            }
                            TablaSimbolos.insertarSimbolo(simNuevo);
                            break;

                        case 6:
                            Respuesta idAux = (Respuesta)action(node.ChildNodes[0], root);
                            Simbolo simNuevoAux = new Simbolo(idAux.id, 1);
                            Respuesta a = (Respuesta)action(node.ChildNodes[2], root);
                            Respuesta tipo = (Respuesta)action(node.ChildNodes[4], root);
                            if (a.inicio == null && a.fin == null)
                            {
                                if (a.min == null)
                                {
                                    simNuevoAux.agregarCadenaUnica(a.id);
                                }
                                else
                                {
                                    simNuevoAux.agregarMinMax(a.min, a.max);
                                }
                            }
                            else
                            {
                                graficar(a, idAux.id);
                                simNuevoAux.agregarRoot(a.inicio, a.fin);
                                metodoCerraduras(simNuevoAux);
                            }
                            simNuevoAux.agregarDato(tipo.min);
                            simNuevoAux.nombre2 = tipo.max;
                            TablaSimbolos.insertarSimbolo(simNuevoAux);
                            break;
                    }
                    break;
                case "G":
                    switch (node.ChildNodes.Count)
                    {
                        case 1:
                            Respuesta r = (Respuesta)action(node.ChildNodes[0], root);
                            return r;
                        case 3:
                            Respuesta rr;
                            Respuesta primera = (Respuesta)action(node.ChildNodes[0], root);
                            Respuesta segunda = (Respuesta)action(node.ChildNodes[2], root);
                            rr = (Respuesta)doOr(primera, segunda);
                            return rr;
                    }
                    break;
                case "REGEX":
                    return (Respuesta)action(node.ChildNodes[0], root);
                case "D":
                    switch (node.ChildNodes.Count)
                    {
                        case 1:
                            return action(node.ChildNodes[0], root);
                        case 2:
                            switch (node.ChildNodes[1].Term.ToString())
                            {
                                case "*":
                                    return do0oMas((Respuesta)action(node.ChildNodes[0], root));
                                case "+":
                                    return doSum((Respuesta)action(node.ChildNodes[0], root));

                                case "?":
                                    return doPuede((Respuesta)action(node.ChildNodes[0], root));
                            }
                            break;
                        case 3:
                            switch (node.ChildNodes[2].Term.ToString())
                            {
                                case "|":
                                    return doOr((Respuesta)action(node.ChildNodes[0], root), (Respuesta)action(node.ChildNodes[1], root));
                                case ".":
                                    return doConcat((Respuesta)action(node.ChildNodes[0], root), (Respuesta)action(node.ChildNodes[1], root));
                            }
                            break;
                    }
                    break;
                case "ID_CONJ":
                    if (node.ChildNodes.Count == 1)
                    {
                        return new Respuesta(null, null, node.ChildNodes[0].Token.ToString().Split('(')[0]);//(Respuesta)action(node.ChildNodes[0], root);
                    }
                    else
                    {
                        return new Respuesta(node.ChildNodes[0].Token.ToString().Split('(')[0], node.ChildNodes[2].Token.ToString().Split('(')[0]);
                    }
                case "RETORNO":
                    return new Respuesta((String)action(node.ChildNodes[4], root), node.ChildNodes[2].Token.ToString().Split('(')[0]);
                case "CUERPO_RETURN":
                    return action(node.ChildNodes[2], root);
                case "TIPO":
                    return node.ChildNodes[0].Term.ToString();
                case "ESPECIAL":
                    switch (node.ChildNodes[0].Term.ToString())
                    {
                        case "\\n":
                            return new Respuesta(null, null, "\\n");
                        case "\\t":
                            return new Respuesta(null, null, "\\t");
                        case "\\'":
                            return new Respuesta(null, null, "\\'");
                        case "\\\"":
                            return new Respuesta(null, null, "\"");
                        case "[:blanco:]":
                            return new Respuesta(null, null, " ");
                        case "[:todo:]":
                            return new Respuesta(null, null, "[:todo:]");
                    }
                    break;
                case "ID":

                    switch (node.ChildNodes.Count)
                    {
                        case 3:
                            return new Respuesta(null, null, node.ChildNodes[1].Token.ToString().Split('(')[0]);
                        case 5:
                            return action(node.ChildNodes[0], root);
                        case 1:
                            if (node.ChildNodes[0].Term.ToString().Equals("ESPECIAL"))
                            {
                                return action(node.ChildNodes[0], root);
                            }
                            else
                            {

                                Simbolo sim = TablaSimbolos.buscarSim(node.FindTokenAndGetText());
                                if (sim != null)
                                {
                                    if (sim.cadenaUnica == null)
                                    {
                                        if (sim.min == null)
                                        {
                                            Nodo nuevo = copiarSimbolo(sim.regexRoot);
                                            return new Respuesta(nuevo, finCopia, null);
                                        }
                                        else
                                        {
                                            return new Respuesta(sim.min, sim.max);
                                        }
                                    }
                                    else
                                    {
                                        return new Respuesta(null, null, sim.cadenaUnica);
                                    }
                                }
                                else
                                {
                                    return new Respuesta(null, null, node.FindTokenAndGetText());
                                }
                            }
                    }
                    break;
            }
            return result;
        }

        Object do0oMas(Respuesta a)
        {
            Nodo n0 = new Nodo();
            if (a.tipo == 0)
            {
                if (a.id != null)
                {
                    Nodo n1 = new Nodo(),
                    n2 = new Nodo(), n3 = new Nodo();
                    n0.agregarHijo(n3);
                    n0.agregarSalto(new Salto(-1, "", "", "", n3));
                    n0.agregarHijo(n1);
                    n0.agregarSalto(new Salto(-1, "", "", "", n1));
                    n1.agregarHijo(n2);
                    n1.agregarSalto(new Salto(0, a.id, "", "", n2));
                    n2.agregarHijo(n1);
                    n2.agregarSalto(new Salto(-1, "", "", "", n1));
                    n2.agregarHijo(n3);
                    n2.agregarSalto(new Salto(-1, "", "", "", n3));
                    return (new Respuesta(n0, n3, null));
                }
                else
                {
                    Nodo n3 = new Nodo();
                    n0.agregarHijo(a.inicio);
                    n0.agregarSalto(new Salto(-1, "", "", "", a.inicio));
                    n0.agregarHijo(n3);
                    n0.agregarSalto(new Salto(-1, "", "", "", n3));
                    a.fin.agregarHijo(a.inicio);
                    a.fin.agregarSalto(new Salto(-1, "", "", "", a.inicio));
                    a.fin.agregarHijo(n3);
                    a.fin.agregarSalto(new Salto(-1, "", "", "", n3));
                    return (new Respuesta(n0, n3, null));
                }
            }
            else
            {
                Nodo n1 = new Nodo(),
                    n2 = new Nodo(),
                    n3 = new Nodo();
                n0.agregarHijo(n3);
                n0.agregarSalto(new Salto(-1, "", "", "", n3));
                n0.agregarHijo(n1);
                n0.agregarSalto(new Salto(-1, "", "", "", n1));
                n1.agregarHijo(n2);
                n1.agregarSalto(new Salto(0, "", a.min, a.max, n2));
                n2.agregarHijo(n1);
                n2.agregarSalto(new Salto(-1, "", "", "", n1));
                n2.agregarHijo(n3);
                n2.agregarSalto(new Salto(-1, "", "", "", n3));
                return (new Respuesta(n0, n3, null));
            }
        }

        Object doPuede(Respuesta aPuede)
        {
            if (aPuede.tipo == 0)
            {
                if (aPuede.id != null)
                {
                    Nodo n0P = new Nodo(),
                        n1P = new Nodo(),
                        n2P = new Nodo(),
                        n3P = new Nodo();
                    n0P.agregarHijo(n1P);
                    n0P.agregarSalto(new Salto(-1, "", "", "", n1P));
                    n0P.agregarHijo(n3P);
                    n0P.agregarSalto(new Salto(-1, "", "", "", n3P));
                    n1P.agregarHijo(n2P);
                    n1P.agregarSalto(new Salto(0, aPuede.id, "", "", n2P));
                    n2P.agregarHijo(n3P);
                    n2P.agregarSalto(new Salto(-1, "", "", "", n3P));
                    return new Respuesta(n0P, n3P, null);
                }
                else
                {
                    Nodo n0P = new Nodo(),
                        n3P = new Nodo();
                    n0P.agregarHijo(aPuede.inicio);
                    n0P.agregarSalto(new Salto(-1, "", "", "", aPuede.inicio));
                    n0P.agregarHijo(n3P);
                    n0P.agregarSalto(new Salto(-1, "", "", "", n3P));
                    aPuede.fin.agregarHijo(n3P);
                    aPuede.fin.agregarSalto(new Salto(-1, "", "", "", n3P));
                    return new Respuesta(n0P, n3P, null);
                }
            }
            else
            {
                Nodo n0P = new Nodo(),
                        n1P = new Nodo(),
                        n2P = new Nodo(),
                        n3P = new Nodo();
                n0P.agregarHijo(n1P);
                n0P.agregarSalto(new Salto(-1, "", "", "", n1P));
                n0P.agregarHijo(n3P);
                n0P.agregarSalto(new Salto(-1, "", "", "", n3P));
                n1P.agregarHijo(n2P);
                n1P.agregarSalto(new Salto(0, "", aPuede.min, aPuede.max, n2P));
                n2P.agregarHijo(n3P);
                n2P.agregarSalto(new Salto(-1, "", "", "", n3P));
                return new Respuesta(n0P, n3P, null);
            }
        }

        Object doSum(Respuesta aSum)
        {
            if (aSum.tipo == 0)
            {
                if (aSum.id != null)
                {
                    Nodo n0s = new Nodo(),
                    n1s = new Nodo(),
                    n2s = new Nodo(),
                    n3s = new Nodo();
                    n0s.agregarHijo(n1s);
                    n0s.agregarSalto(new Salto(-1, "", "", "", n1s));
                    n1s.agregarHijo(n2s);
                    n1s.agregarSalto(new Salto(0, aSum.id, "", "", n2s));
                    n2s.agregarHijo(n1s);
                    n2s.agregarSalto(new Salto(-1, "", "", "", n1s));
                    n2s.agregarHijo(n3s);
                    n2s.agregarSalto(new Salto(-1, "", "", "", n3s));
                    return (new Respuesta(n0s, n3s, null));
                }
                else
                {
                    Nodo n0s = new Nodo(),
                    n3s = new Nodo();
                    n0s.agregarHijo(aSum.inicio);
                    n0s.agregarSalto(new Salto(-1, "", "", "", aSum.inicio));

                    aSum.fin.agregarHijo(aSum.inicio);
                    aSum.fin.agregarSalto(new Salto(-1, "", "", "", aSum.inicio));
                    aSum.fin.agregarHijo(n3s);
                    aSum.fin.agregarSalto(new Salto(-1, "", "", "", n3s));
                    return (new Respuesta(n0s, n3s, null));
                }
            }
            else
            {
                Nodo n0s = new Nodo(),
                    n1s = new Nodo(),
                    n2s = new Nodo(),
                    n3s = new Nodo();
                n0s.agregarHijo(n1s);
                n0s.agregarSalto(new Salto(-1, "", "", "", n1s));
                n1s.agregarHijo(n2s);
                n1s.agregarSalto(new Salto(0, "", aSum.min, aSum.max, n2s));
                n2s.agregarHijo(n1s);
                n2s.agregarSalto(new Salto(-1, "", "", "", n1s));
                n2s.agregarHijo(n3s);
                n2s.agregarSalto(new Salto(-1, "", "", "", n3s));
                return (new Respuesta(n0s, n3s, null));
            }
        }

        Object doConcat(Respuesta aPunto, Respuesta bPunto)
        {
            if (aPunto.tipo == 0)
            {
                if (bPunto.tipo == 0) // a = 0, b = 0
                {
                    if (aPunto.id != null)
                    {
                        if (bPunto.id != null)
                        {
                            Nodo n0Punto = new Nodo(),
                                n1Punto = new Nodo(),
                                n2Punto = new Nodo();
                            n0Punto.agregarHijo(n1Punto);
                            n0Punto.agregarSalto(new Salto(0, aPunto.id, "", "", n1Punto));
                            n1Punto.agregarHijo(n2Punto);
                            n1Punto.agregarSalto(new Salto(0, bPunto.id, "", "", n2Punto));
                            return new Respuesta(n0Punto, n2Punto, null);
                        }
                        else
                        {
                            Nodo n0Punto = new Nodo();
                            n0Punto.agregarHijo(bPunto.inicio);
                            n0Punto.agregarSalto(new Salto(0, aPunto.id, "", "", bPunto.inicio));
                            return new Respuesta(n0Punto, bPunto.fin, null);
                        }
                    }
                    else
                    {
                        if (bPunto.id != null)
                        {
                            Nodo n2Punto = new Nodo();
                            aPunto.fin.agregarHijo(n2Punto);
                            aPunto.fin.agregarSalto(new Salto(0, bPunto.id, "", "", n2Punto));
                            return new Respuesta(aPunto.inicio, n2Punto, null);
                        }
                        else
                        {
                            if (aPunto.inicio.numeroEstado == bPunto.inicio.numeroEstado)
                            {
                                Nodo nuevoAini = copiarSimbolo(aPunto.inicio);
                                Nodo nuevoAfin = finCopia;
                                Nodo nuevoBini = copiarSimbolo(bPunto.inicio);
                                Nodo nuevoBfin = finCopia;
                                nuevoAfin.agregarHijo(nuevoBini);
                                nuevoAfin.agregarSalto(new Salto(-1, "", "", "", nuevoBini));
                                return new Respuesta(nuevoAini, nuevoBfin, null);
                            }
                            else
                            {
                                aPunto.fin.agregarHijo(bPunto.inicio);
                                aPunto.fin.agregarSalto(new Salto(-1, "", "", "", bPunto.inicio));
                                return new Respuesta(aPunto.inicio, bPunto.fin, null);
                            }
                        }
                    }
                }
                else // a = 0, b = 1
                {
                    if (aPunto.id != null)
                    {
                        Nodo n0Punto = new Nodo(),
                            n1Punto = new Nodo(),
                            n2Punto = new Nodo();
                        n0Punto.agregarHijo(n1Punto);
                        n0Punto.agregarSalto(new Salto(0, aPunto.id, "", "", n1Punto));
                        n1Punto.agregarHijo(n2Punto);
                        n1Punto.agregarSalto(new Salto(0, "", bPunto.min, bPunto.max, n2Punto));
                        return new Respuesta(n0Punto, n2Punto, null);
                    }
                    else
                    {
                        Nodo n2Punto = new Nodo();
                        aPunto.fin.agregarHijo(n2Punto);
                        aPunto.fin.agregarSalto(new Salto(0, "", bPunto.min, bPunto.max, n2Punto));
                        return new Respuesta(aPunto.inicio, n2Punto, null);
                    }
                }
            }
            else // a != 0
            {
                if (bPunto.tipo == 0) // a != 0, b = 0
                {
                    if (bPunto.id != null)
                    {
                        Nodo n0Punto = new Nodo(),
                            n1Punto = new Nodo(),
                            n2Punto = new Nodo();
                        n0Punto.agregarHijo(n1Punto);
                        n0Punto.agregarSalto(new Salto(0, "", aPunto.min, aPunto.max, n1Punto));
                        n1Punto.agregarHijo(n2Punto);
                        n1Punto.agregarSalto(new Salto(0, bPunto.id, "", "", n2Punto));
                        return new Respuesta(n0Punto, n2Punto, null);
                    }
                    else
                    {
                        Nodo n0Punto = new Nodo();
                        n0Punto.agregarHijo(bPunto.inicio);
                        n0Punto.agregarSalto(new Salto(0, "", aPunto.min, aPunto.max, bPunto.inicio));
                        return new Respuesta(n0Punto, bPunto.fin, null);
                    }
                }
                else // a != 0, b != 0
                {
                    Nodo n0Punto = new Nodo(),
                        n1Punto = new Nodo(),
                        n2Punto = new Nodo();
                    n0Punto.agregarHijo(n1Punto);
                    n0Punto.agregarSalto(new Salto(0, "", aPunto.min, aPunto.max, n1Punto));
                    n1Punto.agregarHijo(n2Punto);
                    n1Punto.agregarSalto(new Salto(0, "", bPunto.min, bPunto.max, n2Punto));
                    return new Respuesta(n0Punto, n2Punto, null);
                }
            }
        }

        Object doOr(Respuesta aOr, Respuesta bOr)
        {
            if (aOr.tipo == 0)
            {
                if (bOr.tipo == 0) // a = 0, b = 0
                {
                    if (aOr.id != null)
                    {
                        if (bOr.id != null)
                        {
                            Nodo n0O = new Nodo(),
                                n1O = new Nodo(),
                                n2O = new Nodo(),
                                n3O = new Nodo(),
                                n4O = new Nodo(),
                                n5O = new Nodo();
                            n0O.agregarHijo(n1O);
                            n0O.agregarHijo(n3O);
                            n0O.agregarSalto(new Salto(-1, "", "", "", n1O));
                            n0O.agregarSalto(new Salto(-1, "", "", "", n3O));
                            n1O.agregarHijo(n2O);
                            n1O.agregarSalto(new Salto(0, aOr.id, "", "", n2O));
                            n3O.agregarHijo(n4O);
                            n3O.agregarSalto(new Salto(0, bOr.id, "", "", n4O));
                            n2O.agregarHijo(n5O);
                            n2O.agregarSalto(new Salto(-1, "", "", "", n5O));
                            n4O.agregarHijo(n5O);
                            n4O.agregarSalto(new Salto(-1, "", "", "", n5O));
                            return new Respuesta(n0O, n5O, null);
                        }
                        else
                        {
                            Nodo n0O = new Nodo(),
                                n1O = new Nodo(),
                                n2O = new Nodo(),
                                n5O = new Nodo();
                            n0O.agregarHijo(n1O);
                            n0O.agregarHijo(bOr.inicio);
                            n0O.agregarSalto(new Salto(-1, "", "", "", n1O));
                            n0O.agregarSalto(new Salto(-1, "", "", "", bOr.inicio));
                            n1O.agregarHijo(n2O);
                            n1O.agregarSalto(new Salto(0, aOr.id, "", "", n2O));
                            n2O.agregarHijo(n5O);
                            n2O.agregarSalto(new Salto(-1, "", "", "", n5O));
                            bOr.fin.agregarHijo(n5O);
                            bOr.fin.agregarSalto(new Salto(-1, "", "", "", n5O));
                            return new Respuesta(n0O, n5O, null);
                        }
                    }
                    else
                    {
                        if (bOr.id != null)
                        {
                            Nodo n0O = new Nodo(),
                                n3O = new Nodo(),
                                n4O = new Nodo(),
                                n5O = new Nodo();
                            n0O.agregarHijo(aOr.inicio);
                            n0O.agregarHijo(n3O);
                            n0O.agregarSalto(new Salto(-1, "", "", "", aOr.inicio));
                            n0O.agregarSalto(new Salto(-1, "", "", "", n3O));
                            n3O.agregarHijo(n4O);
                            n3O.agregarSalto(new Salto(0, bOr.id, "", "", n4O));
                            n4O.agregarHijo(n5O);
                            n4O.agregarSalto(new Salto(-1, "", "", "", n5O));
                            aOr.fin.agregarHijo(n5O);
                            aOr.fin.agregarSalto(new Salto(-1, "", "", "", n5O));
                            return new Respuesta(n0O, n5O, null);
                        }
                        else
                        {
                            Nodo n0O = new Nodo(),
                                n5O = new Nodo();
                            if (aOr.inicio.numeroEstado == bOr.inicio.numeroEstado)
                            {
                                Nodo nuevoAini = copiarSimbolo(aOr.inicio);
                                Nodo nuevoAfin = finCopia;
                                Nodo nuevoBini = copiarSimbolo(bOr.inicio);
                                Nodo nuevoBfin = finCopia;
                                n0O.agregarHijo(nuevoAini);
                                n0O.agregarHijo(nuevoBini);
                                n0O.agregarSalto(new Salto(-1, "", "", "", nuevoAini));
                                n0O.agregarSalto(new Salto(-1, "", "", "", nuevoBini));
                                nuevoAfin.agregarHijo(n5O);
                                nuevoAfin.agregarSalto(new Salto(-1, "", "", "", n5O));
                                nuevoBfin.agregarHijo(n5O);
                                nuevoBfin.agregarSalto(new Salto(-1, "", "", "", n5O));
                            }
                            else
                            {
                                n0O.agregarHijo(aOr.inicio);
                                n0O.agregarHijo(bOr.inicio);
                                n0O.agregarSalto(new Salto(-1, "", "", "", aOr.inicio));
                                n0O.agregarSalto(new Salto(-1, "", "", "", bOr.inicio));
                                aOr.fin.agregarHijo(n5O);
                                aOr.fin.agregarSalto(new Salto(-1, "", "", "", n5O));
                                bOr.fin.agregarHijo(n5O);
                                bOr.fin.agregarSalto(new Salto(-1, "", "", "", n5O));
                            }
                            return new Respuesta(n0O, n5O, null);
                        }
                    }
                }
                else // a = 0, b != 0
                {
                    if (aOr.id != null)
                    {
                        Nodo n0O = new Nodo(),
                            n1O = new Nodo(),
                            n2O = new Nodo(),
                            n3O = new Nodo(),
                            n4O = new Nodo(),
                            n5O = new Nodo();
                        n0O.agregarHijo(n1O);
                        n0O.agregarHijo(n3O);
                        n0O.agregarSalto(new Salto(-1, "", "", "", n1O));
                        n0O.agregarSalto(new Salto(-1, "", "", "", n3O));
                        n1O.agregarHijo(n2O);
                        n1O.agregarSalto(new Salto(0, aOr.id, "", "", n2O));
                        n3O.agregarHijo(n4O);
                        n3O.agregarSalto(new Salto(0, "", bOr.min, bOr.max, n4O));
                        n2O.agregarHijo(n5O);
                        n2O.agregarSalto(new Salto(-1, "", "", "", n5O));
                        n4O.agregarHijo(n5O);
                        n4O.agregarSalto(new Salto(-1, "", "", "", n5O));
                        return new Respuesta(n0O, n5O, null);
                    }
                    else
                    {
                        Nodo n0O = new Nodo(),
                            n3O = new Nodo(),
                            n4O = new Nodo(),
                            n5O = new Nodo();
                        n0O.agregarHijo(aOr.inicio);
                        n0O.agregarHijo(n3O);
                        n0O.agregarSalto(new Salto(-1, "", "", "", aOr.inicio));
                        n0O.agregarSalto(new Salto(-1, "", "", "", n3O));
                        n3O.agregarHijo(n4O);
                        n3O.agregarSalto(new Salto(0, "", bOr.min, bOr.max, n4O));
                        n4O.agregarHijo(n5O);
                        n4O.agregarSalto(new Salto(-1, "", "", "", n5O));
                        aOr.fin.agregarHijo(n5O);
                        aOr.fin.agregarSalto(new Salto(-1, "", "", "", n5O));
                        return new Respuesta(n0O, n5O, null);
                    }
                }
            }
            else // a != 0
            {
                if (bOr.tipo == 0) // a != 0, b = 0
                {
                    if (bOr.id != null)
                    {
                        Nodo n0O = new Nodo(),
                            n1O = new Nodo(),
                            n2O = new Nodo(),
                            n3O = new Nodo(),
                            n4O = new Nodo(),
                            n5O = new Nodo();
                        n0O.agregarHijo(n1O);
                        n0O.agregarHijo(n3O);
                        n0O.agregarSalto(new Salto(-1, "", "", "", n1O));
                        n0O.agregarSalto(new Salto(-1, "", "", "", n3O));
                        n1O.agregarHijo(n2O);
                        n1O.agregarSalto(new Salto(0, "", aOr.min, aOr.max, n2O));
                        n3O.agregarHijo(n4O);
                        n3O.agregarSalto(new Salto(0, bOr.id, "", "", n4O));
                        n2O.agregarHijo(n5O);
                        n2O.agregarSalto(new Salto(-1, "", "", "", n5O));
                        n4O.agregarHijo(n5O);
                        n4O.agregarSalto(new Salto(-1, "", "", "", n5O));
                        return new Respuesta(n0O, n5O, null);
                    }
                    else
                    {
                        Nodo n0O = new Nodo(),
                            n1O = new Nodo(),
                            n2O = new Nodo(),
                            n5O = new Nodo();
                        n0O.agregarHijo(n1O);
                        n0O.agregarHijo(bOr.inicio);
                        n0O.agregarSalto(new Salto(-1, "", "", "", n1O));
                        n0O.agregarSalto(new Salto(-1, "", "", "", bOr.inicio));
                        n1O.agregarHijo(n2O);
                        n1O.agregarSalto(new Salto(0, "", aOr.min, aOr.max, n2O));
                        n2O.agregarHijo(n5O);
                        n2O.agregarSalto(new Salto(-1, "", "", "", n5O));
                        bOr.fin.agregarHijo(n5O);
                        bOr.fin.agregarSalto(new Salto(-1, "", "", "", n5O));
                        return new Respuesta(n0O, n5O, null);
                    }

                }
                else // a != 0, b != 0
                {
                    Nodo n0O = new Nodo(),
                        n1O = new Nodo(),
                        n2O = new Nodo(),
                        n3O = new Nodo(),
                        n4O = new Nodo(),
                        n5O = new Nodo();
                    n0O.agregarHijo(n1O);
                    n0O.agregarHijo(n3O);
                    n0O.agregarSalto(new Salto(-1, "", "", "", n1O));
                    n0O.agregarSalto(new Salto(-1, "", "", "", n3O));
                    n1O.agregarHijo(n2O);
                    n1O.agregarSalto(new Salto(0, "", aOr.min, aOr.max, n2O));
                    n3O.agregarHijo(n4O);
                    n3O.agregarSalto(new Salto(0, "", bOr.min, bOr.max, n4O));
                    n2O.agregarHijo(n5O);
                    n2O.agregarSalto(new Salto(-1, "", "", "", n5O));
                    n4O.agregarHijo(n5O);
                    n4O.agregarSalto(new Salto(-1, "", "", "", n5O));
                    return new Respuesta(n0O, n5O, null);

                }
            }
        }

        public void getIDsCerradura(Nodo respuesta)
        {
            graphRelated = new List<int>();
            todosIDCerradura = new List<ID_Cerradura>();
            getIDsCerraduraAux(respuesta);
        }

        public void getIDsCerraduraAux(Nodo root)
        {
            if (!graphRelated.Contains(root.numeroEstado))
            {
                graphRelated.Add(root.numeroEstado);
                int conteo = root.saltos.Count;
                for (int i = 0; i < conteo; i++)
                {
                    bool existe = false;
                    if (!root.saltos[i].caracter.Equals(""))
                    {
                        for (int j = 0; j < todosIDCerradura.Count; j++)
                        {
                            if (todosIDCerradura[j].idUnico != null)
                            {
                                if (todosIDCerradura[j].idUnico.Equals(root.saltos[i].caracter))
                                {
                                    existe = true;
                                }
                            }
                        }
                        if (!existe)
                        {
                            todosIDCerradura.Add(new ID_Cerradura(root.saltos[i].caracter));
                        }
                    }
                    else if (!root.saltos[i].inicio.Equals(""))
                    {
                        for (int j = 0; j < todosIDCerradura.Count; j++)
                        {
                            if (todosIDCerradura[j].min != null)
                            {
                                if (todosIDCerradura[j].min.Equals(root.saltos[i].inicio) && todosIDCerradura[j].max.Equals(root.saltos[i].final))
                                {
                                    existe = true;
                                }
                            }
                        }
                        if (!existe)
                        {
                            todosIDCerradura.Add(new ID_Cerradura(root.saltos[i].inicio, root.saltos[i].final));
                        }
                    }
                    else
                    {

                    }

                    getIDsCerraduraAux(root.saltos[i].destino);
                }
            }

        }

        public Nodo copiarSimbolo(Nodo respuesta)
        {
            finCopia = null;
            graphRelated = new List<int>();
            listaCopiaAux = new List<Nodo>();
            Nodo nuevo = new Nodo();
            copiarSimboloAux(respuesta, nuevo, null, 0, null, null, null);
            return nuevo;
        }

        public void copiarSimboloAux(Nodo root, Nodo aModificar, Nodo padre, int tipo, String caracter, String inicio, String final)
        {
            if (padre != null)
            {
                padre.agregarHijo(aModificar);
                padre.agregarSalto(new Salto(tipo, caracter, inicio, final, aModificar));
            }
            if (!graphRelated.Contains(root.numeroEstado))
            {
                graphRelated.Add(root.numeroEstado);
                listaCopiaAux.Add(aModificar);
                int conteo = root.saltos.Count;
                if (conteo == 0)
                {
                    finCopia = aModificar;
                }
                for (int i = 0; i < conteo; i++)
                {
                    if (graphRelated.Contains(root.saltos[i].destino.numeroEstado))
                    {
                        int posicion = graphRelated.IndexOf(root.saltos[i].destino.numeroEstado);
                        copiarSimboloAux(root.saltos[i].destino, listaCopiaAux[posicion], aModificar, root.saltos[i].Tipo, root.saltos[i].caracter, root.saltos[i].inicio, root.saltos[i].final);
                    }
                    else
                    {
                        copiarSimboloAux(root.saltos[i].destino, new Nodo(), aModificar, root.saltos[i].Tipo, root.saltos[i].caracter, root.saltos[i].inicio, root.saltos[i].final);
                    }
                }
            }

        }

        public void metodoCerraduras(Simbolo main)
        {
            int ultimoEstado = main.regexFin.numeroEstado;
            conjuntosCerraduras = new List<Conjunto>();
            graphRelated = new List<int>();
            auxiliarCerradura = new List<int>();
            getIDsCerradura(main.regexRoot);
            String cerradura = Cerradura(main.regexRoot);
            conjuntosCerraduras.Add(new Conjunto(cerradura));
            int a = conjuntosCerraduras.Count;
            for (int i = 0; i < a; i++)
            {
                Conjunto x = conjuntosCerraduras[i];
                Mover(x, ultimoEstado);
                if (a != conjuntosCerraduras.Count)
                {
                    a++;
                }
            }
            graficarAFD(main.nombre);
            main.afdInicio = conjuntosCerraduras[0];
        }

        public void graficarAFD(String nombre)
        {
            String cod = "";
            String cod1 = "";
            for (int i = 0; i < conjuntosCerraduras.Count; i++)
            {
                if (conjuntosCerraduras[i].aceptacion)
                {
                    cod1 += "\"" + conjuntosCerraduras[i].nombre + "\";";
                }
                for (int j = 0; j < conjuntosCerraduras[i].transiciones.Count; j++)
                {
                    if (conjuntosCerraduras[i].transiciones[j].terminal.idUnico != null)
                    {
                        cod += "\"" + conjuntosCerraduras[i].nombre + "\" -> \"" + conjuntosCerraduras[i].transiciones[j].conjunto.nombre + "\" [label = \"" + conjuntosCerraduras[i].transiciones[j].terminal.idUnico + "\"]";
                    }
                    else
                    {
                        cod += "\"" + conjuntosCerraduras[i].nombre + "\" -> \"" + conjuntosCerraduras[i].transiciones[j].conjunto.nombre + "\" [label = \"" + conjuntosCerraduras[i].transiciones[j].terminal.min + "~" + conjuntosCerraduras[i].transiciones[j].terminal.max + "\"]";
                    }
                }
            }
            Graphviz g = new Graphviz();
            g.graph(nombre + "-AFD", cod1, cod);
        }

        public void Mover(Conjunto conjunto, int ultimoEstado)
        {
            String[] a = conjunto.cadena.Split(',');
            String cadena = "";
            for (int j = 0; j < todosIDCerradura.Count; j++)
            {
                cadena = "";
                auxiliarCerradura = new List<int>();
                for (int i = 1; i < a.Length; i++)
                {
                    cadena += MoverAux(Nodo.todosLosNodos[Int32.Parse(a[i])], todosIDCerradura[j]);
                }
                String[] b = cadena.Split(',');
                String cad2 = "";
                for (int i = 1; i < b.Length; i++)
                {
                    cad2 += Cerradura(Nodo.todosLosNodos[Int32.Parse(b[i])]);
                }

                Conjunto x = existeConjunto(cad2);
                if (x == null)
                {
                    x = new Conjunto(cad2);
                    conjuntosCerraduras.Add(x);
                }
                String[] aux = x.cadena.Split(',');
                if (aux.Contains(ultimoEstado + ""))
                {
                    x.hacerDeAceptacion();
                }
                conjunto.agregarTransicion(x, todosIDCerradura[j]);
            }
        }

        public String MoverAux(Nodo root, ID_Cerradura terminal)
        {
            String strReturn = "";
            for (int a = 0; a < root.saltos.Count; a++)
            {
                if (root.saltos[a].Tipo != -1)
                {
                    if (!root.saltos[a].caracter.Equals(""))
                    {
                        if (terminal.idUnico != null)
                        {
                            if (root.saltos[a].caracter.Equals(terminal.idUnico))
                            {
                                strReturn += "," + root.saltos[a].destino.numeroEstado;
                            }
                        }
                    }
                    else
                    {
                        if (terminal.min != null)
                        {
                            if (root.saltos[a].inicio.Equals(terminal.min) && root.saltos[a].final.Equals(terminal.max))
                            {
                                strReturn += "," + root.saltos[a].destino.numeroEstado;
                            }
                        }
                    }
                }
            }
            return strReturn;
        }

        public Conjunto existeConjunto(String cadena)
        {
            for (int i = 0; i < conjuntosCerraduras.Count; i++)
            {
                if (conjuntosCerraduras[i].cadena.Equals(cadena))
                {
                    return conjuntosCerraduras[i];
                }
            }
            return null;
            //conjuntosCerraduras.Add();
        }

        public String Cerradura(Nodo root)
        {
            String cad = "";
            CerraduraAux(root, ref cad);
            return cad;
        }

        public void CerraduraAux(Nodo root, ref String cadena)
        {
            if (!auxiliarCerradura.Contains(root.numeroEstado))
            {
                cadena += "," + root.numeroEstado;

                auxiliarCerradura.Add(root.numeroEstado);
                for (int i = 0; i < root.saltos.Count; i++)
                {
                    if (root.saltos[i].Tipo == -1)
                    {
                        CerraduraAux(root.saltos[i].destino, ref cadena);
                    }
                }
            }
        }

        public void graficar(Nodo respuesta, String filename)
        {
            cad = "";
            graphRelated = new List<int>();
            graficarAux(respuesta, ref cad);
            Graphviz g = new Graphviz();
            g.graph(filename, cad);
        }

        public void graficar(Respuesta respuesta, String filename)
        {
            cad = "";
            graphRelated = new List<int>();
            graficarAux(respuesta.inicio, ref cad);
            Graphviz g = new Graphviz();
            g.graph(filename, cad);
        }

        public void graficarAux(Nodo root, ref String cad)
        {
            if (!graphRelated.Contains(root.numeroEstado))
            {
                graphRelated.Add(root.numeroEstado);
                int conteo = root.saltos.Count;
                for (int i = 0; i < conteo; i++)
                {
                    if (!root.saltos[i].caracter.Equals(""))
                    {
                        cad = cad + "\"" + root.numeroEstado + "\" -> \"" + root.saltos[i].destino.numeroEstado + "\"[label = \"" + root.saltos[i].caracter + "\"]";
                    }
                    else
                    {
                        if (!root.saltos[i].inicio.Equals(""))
                        {
                            cad = cad + "\"" + root.numeroEstado + "\" -> \"" + root.saltos[i].destino.numeroEstado + "\"[label = \"" + root.saltos[i].inicio + "~" + root.saltos[i].final + "\"]";
                        }
                        else
                        {
                            cad = cad + "\"" + root.numeroEstado + "\" -> \"" + root.saltos[i].destino.numeroEstado + "\"[label = \"ε\"]";
                        }
                    }
                    graficarAux(root.saltos[i].destino, ref cad);
                }
            }

        }
    }
}
