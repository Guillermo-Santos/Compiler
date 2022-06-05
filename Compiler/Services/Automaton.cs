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
    /// <summary>
    /// Pequeña estructura usada para evaluar los lexemas.
    /// </summary>
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

    internal class Automaton : IAutomaton<IEnumerable<Lexeme>,State>
    {
        State[] Arrange(State[] states, State first)
        {
            for(int i = 2; i<states.Length; i++)
            {
                if(states[i] == first)
                {
                    states[i] = states[2];
                    states[2] = first;
                    break;
                }
            }
            return states;
        }
        public State Evaluate(string lexeme, State LastState)
        {
            State[] order = { 
                State.String,
                State.NotAcepted, 
                State.Word, 
                State.Number, 
                State.Symbol
            };
            if(LastState != State.Default && LastState != State.NotAcepted && LastState != order[2] && LastState != State.String) {
                order = Arrange(order, LastState); 
            }
            foreach(State state in order)
            {
                switch (state)
                {
                    case State.NotAcepted:
                        if (Regex.IsMatch(lexeme, @"[\s\n\v\b\t\rŸ]"))
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
            //Inicializar conjunto a devolver.
            List<Lexeme> Lexicons = new List<Lexeme>();
            //Dividir el texto dado en lineas.
            string[] lines = texto.Split("\r");
            //Inicializar variable temporal usada para llenar el conjunto a devolver en cada iteracion.
            Lexeme NewLexicon = new Lexeme() { Text = string.Empty };
            //Preparando el ultimo estado para la primera iteracion
            State LastState = State.Default;
            for (int L = 0;L< lines.Length; L++){
                //Agregamos un caracter no implementado a la linea con la que vayamos a trabajar.
                lines[L] += "Ÿ";
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
                                    if (LastState == State.Symbol)
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
                                    //Agregamos el caracter actual al nuevo lexema.
                                    NewLexicon.Text += lines[L][C];
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
    }
}
