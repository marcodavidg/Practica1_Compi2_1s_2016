using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Compi2_Practica1_1s2016
{
    class Gramatica : Grammar
    {
        public Gramatica()
            : base(false)
        {
            #region terminales / noTerminales

            NumberLiteral numero = new NumberLiteral("numero");
            IdentifierTerminal palabra = new IdentifierTerminal("palabra");
            RegexBasedTerminal cualquiera = new RegexBasedTerminal("cualquiera", "[^\"]");

            NonTerminal S = new NonTerminal("S");
            NonTerminal E = new NonTerminal("E");
            NonTerminal B = new NonTerminal("B");
            NonTerminal C = new NonTerminal("C");
            NonTerminal REGEX = new NonTerminal("REGEX");
            NonTerminal D = new NonTerminal("D");
            NonTerminal G = new NonTerminal("G");
            NonTerminal ID = new NonTerminal("ID");
            NonTerminal TIPO = new NonTerminal("TIPO");
            NonTerminal ID_CONJ = new NonTerminal("ID_CONJ");
            NonTerminal ESPECIAL = new NonTerminal("ESPECIAL");

            NonTerminal RETORNO = new NonTerminal("RETORNO");
            NonTerminal CUERPO_RETURN = new NonTerminal("CUERPO_RETURN");
            NonTerminal RESERV = new NonTerminal("RESERV");
            NonTerminal RESERV_CUERPO = new NonTerminal("RESERV_CUERPO");

            #endregion

            #region gramatica

            S.Rule = E;
            E.Rule = ToTerm("%%") + B + ToTerm("%%");
            B.Rule = B + C + ToTerm(";")
                | C + ToTerm(";");
            C.Rule = ID + ToTerm("->") + REGEX + ToTerm("->") + RETORNO + RESERV
                | ToTerm("CONJ") + ToTerm(":") + ID + ToTerm("->") + G;
            ID_CONJ.Rule = cualquiera
                | cualquiera + ToTerm("~") + cualquiera;
            G.Rule = G + ToTerm(",") + ID_CONJ
                | ID_CONJ;
            REGEX.Rule = D;
            D.Rule = D + D + ToTerm("|")
                | D + D + ToTerm(".")
                | D + ToTerm("*")
                | D + ToTerm("+")
                | D + ToTerm("?")
                | ID;
            ID.Rule = palabra
                | ToTerm("\"") + palabra + ToTerm("\"")
                | ToTerm("\"") + cualquiera + ToTerm("\"")
                | numero
                | ESPECIAL;
            ESPECIAL.Rule = ToTerm("\\n")
                | ToTerm("\\t")
                | ToTerm("\\'")
                | ToTerm("\\\"")
                | ToTerm("[:blanco:]")
                | ToTerm("[:todo:]");
            RETORNO.Rule = ToTerm("retorno") + ToTerm("(") + palabra + ToTerm(",") + CUERPO_RETURN;

            CUERPO_RETURN.Rule = ToTerm("yytext") + ToTerm(",") + TIPO + ToTerm(",") + ToTerm("yyline") + ToTerm(",") + ToTerm("yyrow") + ToTerm(")")
                                | ToTerm("yyline") + ToTerm(",") + ToTerm("yyrow") + ToTerm(")");
            TIPO.Rule = ToTerm("int")
                    | ToTerm("string")
                    | ToTerm("char")
                    | ToTerm("float")
                    | ToTerm("bool");
            RESERV.Rule = Empty
                    | ToTerm("->") + ToTerm("RESERV") + ToTerm("[") + RESERV_CUERPO + ToTerm("]");

            RESERV_CUERPO.Rule = RESERV_CUERPO + ToTerm("\"") + palabra + ToTerm("\"") + ToTerm("->") + ToTerm("retorno") + ToTerm("(") + palabra + ToTerm(",") + CUERPO_RETURN + ";"
                                | ToTerm("\"") + palabra + ToTerm("\"") + ToTerm("->") + ToTerm("retorno") + ToTerm("(") + palabra + ToTerm(",") + CUERPO_RETURN + ";";

            #endregion

            #region preferencias

            RegisterOperators(1, "|");
            RegisterOperators(2, ".");
            RegisterOperators(3, "*", "+", "?");

            MarkReservedWords("CONJ", "retorno", "error", "yyline", "yytext", "yyrow", "int", "string", "char", "float", "bool");

            this.Root = S;

            #endregion
        }

    }
}
