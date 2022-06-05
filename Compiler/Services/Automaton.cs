using Compiler.Models;
using Compiler.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;

namespace Compiler.Services
{
    enum State
    {
        Default,//Primer estado usado para iniciar el analizador.
        Word,//Estado que representa que el lexema es una palabra, ya sea reservada, o el identificador de una variable.
        Number,//Estado que representa un numero, ya sea un numero entero o un numero real.
        String,//Estado que representa una cadena de texto.
        Symbol,//Estado que representa un simbolo aceptado por el lenguaje,
        Delim,//Delimitadores de tokens, casos como un espacio, final de linea, tabulaciones, etc.
        NotAcepted,//Estado que representa, de manera general, todo caracter no aceptado por el lenguaje. 
    }
    struct Lexeme
    {
        /// <summary>
        /// Texto del lexema.
        /// </summary>
        public string Text;
        /// <summary>
        /// Estado del lexema.
        /// </summary>
        public State State;
    }

    internal class Automaton : IAutomaton
    {
        string _textoIma;
        int _edoAct;
        
        public char NextChar(ref int i)
        {
            if (i == _textoIma.Length)
            {
                i++;
                return ' ';
            }
            return _textoIma[i++];
        }

        //Not implemented
        IEnumerable<string> LexemeToString(List<Lexeme> mylexemes)
        {
            List<string> lexemes = new List<string>();
            foreach(Lexeme lexeme in mylexemes)
            {
                lexemes.Add(lexeme.Text);
            }
            return lexemes;
        }
        State[] Arrange(State[] states, State first)
        {
            for(int i = 1; i<states.Length; i++)
            {
                if(states[i] == first)
                {
                    states[i] = states[1];
                    states[1] = first;
                    break;
                }
            }
            return states;
        }
        State Evaluate(string lexeme, State LastState)
        {
            State[] order = {State.String, State.NotAcepted, State.Word, State.Number, State.Symbol};
            if(LastState != State.Default && LastState != State.NotAcepted && LastState != order[1]) {
                order = Arrange(order, LastState); 
            }
            foreach(State state in order)
            {
                switch (state)
                {
                    case State.NotAcepted:
                        if (Regex.IsMatch(lexeme, @"[\s\n\v\b\t\r]"))
                        {
                            return state;
                        }
                        break;
                    case State.Word:
                        if (Regex.IsMatch(lexeme, "[a-zA-Z_][a-zA-z0-9_]*"))
                        {
                            return state;
                        }
                        break;
                    case State.Number:
                        if (Regex.IsMatch(lexeme, @"[0-9]+\.{0,1}[0-9]*"))
                        {
                            return state;
                        }
                        break;
                    case State.String:
                        if (Regex.IsMatch(lexeme, "\"[^\"]*\"|\"[^\"]*[\"]*"))
                        {
                            return state;
                        }
                        break;
                    case State.Symbol:
                        if (Regex.IsMatch(lexeme, @"[!<>;,\.+\-*/()=]"))
                        {
                            return state;
                        }
                        break;
                }
            }
            return State.NotAcepted;
        }
        public IEnumerable<Lexeme> Recognize(string texto)
        {

            List<Lexeme> Lexicons = new List<Lexeme>();
            string[] lines = texto.Split("\r");
            Lexeme NewLexicon = new Lexeme() { Text = string.Empty };
            State LastState = State.Default;//preparando el ultimo estado para la primera iteracion
            for (int L = 0;L< lines.Length; L++){
                lines[L] += " ";
                for (int C = 0; C < lines[L].Length; C++)
                {
                    State NewState = Evaluate(NewLexicon.Text + lines[L][C], LastState);
                    switch (NewState)
                    {
                        case State.Word:
                            {
                                if (Regex.IsMatch(lines[L][C].ToString(), @"[a-zA-Z_][a-zA-z0-9_]*"))
                                {
                                    //Agregamos el caracter actual al nuevo lexema.
                                    NewLexicon.Text += lines[L][C];
                                }
                                else
                                {
                                    //Le colocamos al tipo de estado al que pertenece el lexema.
                                    NewLexicon.State = LastState;
                                    //Agregamos el nuevo lexema a la lista de lexemas.
                                    Lexicons.Add(NewLexicon);
                                    //Inicializamos el nuevo lexema con el caracter actual.
                                    NewLexicon = new Lexeme() { Text = string.Empty };
                                    C--;
                                }
                                break;
                            }
                        case State.Number:
                            {
                                if (Regex.IsMatch(lines[L][C].ToString(), @"[0-9.]"))
                                {
                                    //Agregamos el caracter actual al nuevo lexema.
                                    NewLexicon.Text += lines[L][C];
                                }
                                else
                                {
                                    //Le colocamos al tipo de estado al que pertenece el lexema.
                                    NewLexicon.State = LastState;
                                    //Agregamos el nuevo lexema a la lista de lexemas.
                                    Lexicons.Add(NewLexicon);
                                    //Inicializamos el nuevo lexema con el caracter actual.
                                    NewLexicon = new Lexeme() { Text = string.Empty };
                                    C--;
                                }
                                break;
                            }
                        case State.String:
                            {
                                //Si el string sin el nuevo caracter ya es un string completo (un string rodeado por "").
                                if (Regex.IsMatch(NewLexicon.Text, "\"[^\"]*\""))
                                {
                                    //Le colocamos al tipo de estado al que pertenece el lexema.
                                    NewLexicon.State = LastState;
                                    //Agregamos el nuevo lexema a la lista de lexemas.
                                    Lexicons.Add(NewLexicon);
                                    //Inicializamos el nuevo lexema con el caracter actual.
                                    NewLexicon = new Lexeme() { Text = string.Empty };
                                    C--;
                                }
                                else
                                {
                                    //Agregamos el caracter actual al nuevo lexema.
                                    NewLexicon.Text += lines[L][C];
                                    if (Regex.IsMatch(NewLexicon.Text, "\"[^\"]*\""))
                                    {
                                        //Le colocamos al tipo de estado al que pertenece el lexema.
                                        NewLexicon.State = LastState;
                                        //Agregamos el nuevo lexema a la lista de lexemas.
                                        Lexicons.Add(NewLexicon);
                                        //Inicializamos el nuevo lexema con el caracter actual.
                                        NewLexicon = new Lexeme() { Text = string.Empty };
                                    }
                                }
                                break;
                            }
                        case State.Symbol:
                            {
                                if (Regex.IsMatch(lines[L][C].ToString(), @"[!<>;,\.+\-*/()=]"))
                                {
                                    //Si el simbolo sin el nuevo caracter ya es un simbolo compuesto y el simbolo + el nuevo caracter tipo simbolo no es un simbolo compuesto.
                                    if (Regex.IsMatch(NewLexicon.Text, @"\+\+|--|<=|>=|\+=|-=|!=|==") && !Regex.IsMatch(NewLexicon.Text + lines[L][C], @"\+\+|--|<=|>=|\+=|-=|!=|=="))
                                    {
                                        //Le colocamos al tipo de estado al que pertenece el lexema.
                                        NewLexicon.State = LastState;
                                        //Agregamos el nuevo lexema a la lista de lexemas.
                                        Lexicons.Add(NewLexicon);
                                        //Inicializamos el nuevo lexema con el caracter actual.
                                        NewLexicon = new Lexeme() { Text = string.Empty + lines[L][C] };
                                    }
                                    else
                                    {
                                        //Agregamos el caracter actual al nuevo lexema.
                                        NewLexicon.Text += lines[L][C];
                                    }
                                }
                                else
                                {
                                    //Le colocamos al tipo de estado al que pertenece el lexema.
                                    NewLexicon.State = LastState;
                                    //Agregamos el nuevo lexema a la lista de lexemas.
                                    Lexicons.Add(NewLexicon);
                                    //Inicializamos el nuevo lexema con el caracter actual.
                                    NewLexicon = new Lexeme() { Text = string.Empty };
                                    C--;
                                }
                                
                                break;
                            }
                        case State.NotAcepted:
                            {
                                if (LastState != NewState)
                                {
                                    if (NewLexicon.Text.Length > 0)
                                    {

                                        //Le colocamos al tipo de estado al que pertenece el lexema.
                                        NewLexicon.State = LastState;
                                        //Agregamos el nuevo lexema a la lista de lexemas.
                                        Lexicons.Add(NewLexicon);
                                        //Inicializamos el nuevo lexema con el caracter actual.
                                        NewLexicon = new Lexeme() { Text = string.Empty };
                                    }
                                }
                                break;
                            }       
                    }
                    LastState = NewState;
                }
            }
            return Lexicons;
        }
        public bool Recognize(string texto, int iniToken, ref int i, int noAuto)
        {
            char c;
            _textoIma = texto;
            switch (noAuto)
            {
                case 0: _edoAct = 0; break;
                case 1: _edoAct = 3; break;
                case 2: _edoAct = 6; break;
                case 3: _edoAct = 9; break;
                case 4: _edoAct = 11; break;
            }
            while (i <= _textoIma.Length)
            {
                switch (_edoAct)
                {
                    //--- Automata delim---

                    case 0:

                        c = NextChar(ref i);
                        if ((" \n\r\t").IndexOf(c) >= 0)
                            _edoAct = 1;
                        else
                        {
                            i = iniToken;
                            return false;
                        }
                        break;

                    case 1:

                        c = NextChar(ref i);
                        if ((" \n\r\t").IndexOf(c) >= 0)
                            _edoAct = 1;
                        else if (("!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_'abcdefghijklmnopqr stuvwxyz{|}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ''“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬-®¯°±²³´µ¶·¸¹º»¼½¾¿\f").IndexOf(c) >= 0)
                            _edoAct = 2;
                        else
                        {
                            i = iniToken;
                            return false;
                        }
                        break;
                    case 2:
                        i--;
                        return true;

                    //--- Automata id---
                    case 3:
                        c = NextChar(ref i);
                        if (("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz").IndexOf(c) >= 0)
                            _edoAct = 4;
                        else
                        {
                            i = iniToken;

                            return false;
                        }
                        break;

                    case 4:
                        c = NextChar(ref i);
                        if (("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz").IndexOf(c) >= 0)
                            _edoAct = 4;
                        else if (("0123456789").IndexOf(c) >= 0)
                            _edoAct = 4;
                        else if (("_").IndexOf(c) >= 0)
                            _edoAct = 4;
                        else if (("!\"#$%&\'()*+,-./:;<=>?@[\\]^`{|}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬-®¯°±²³´µ¶·¸¹º»¼½¾¿\n\t\r\f").IndexOf(c) >= 0)
                            _edoAct = 5;
                        else
                        {
                            i = iniToken;
                            return false;
                        }
                        break;
                    case 5:
                        i--;
                        return true;
                    //--- Automata num---

                    case 6:

                        c = NextChar(ref i);
                        if (("0123456789").IndexOf(c) >= 0) _edoAct = 7;
                        else
                        {
                            i = iniToken;

                            return false;
                        }
                        break;

                    case 7:

                        c = NextChar(ref i);
                        if (("0123456789").IndexOf(c) >= 0) _edoAct = 7;
                        else

                        if (("!\"#$%&\'()*+,-./:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{| }~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬-®¯°±²³´µ¶·¸¹º»¼½¾¿\n\t\r\f").IndexOf(c) >= 0)
                            _edoAct = 8;
                        else
                        {
                            i = iniToken;
                            return false;
                        }
                        break;
                    case 8:
                        i--;
                        return true;

                    //--- Automata otros---

                    case 9:

                        c = NextChar(ref i);
                        if (("=").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if ((";").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if ((",").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if ((".").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if (("+").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if (("-").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if (("*").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if (("/").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if (("(").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else if ((")").IndexOf(c) >= 0)
                            //return true;
                            _edoAct = 10;
                        else
                        {
                            i = iniToken;

                            return false;
                        }
                        break;
                    case 10:
                        return true;
                    //--- Automata cad---

                    case 11:
                        c = NextChar(ref i);
                        if (("\"").IndexOf(c) >= 0)
                            _edoAct = 12;
                        else
                        {
                            i = iniToken;
                            return false;
                        }
                        break;
                    case 12:
                        c = NextChar(ref i);
                        if (("\"").IndexOf(c) >= 0)
                            _edoAct = 13;
                        else
                        if (("!#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqr stuvwxyz{|}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ‘’“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬-®¯°±²³´µ¶·¸¹º»¼½¾¿\n\t\r\f").IndexOf(c) >= 0)
                            _edoAct = 12;
                        else
                        {
                            i = iniToken;

                            return false;
                        }
                        break;

                    case 13: return true;

                }

                switch (_edoAct)
                {
                    case 2: // Autómata delim
                    case 5: // Autómata id
                    case 8: // Autómata num
                        --i;
                        return true;
                }
            }
            return false;
            //return Lexicons;
        }
    }
}
