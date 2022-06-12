using Compiler.Services.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Compiler.Models;

namespace Compiler.Services.Analyzers.Lexic
{
    /// <summary>
    /// Automata del analizador lexico.
    /// </summary>
    internal class Automaton : IAutomaton<IEnumerable<Lexeme>, State>
    {
        /// <summary>
        /// Cambia de posicion el elemento <paramref name="item"/> con el elemento de la posicion <paramref name="pos"/> en <paramref name="items"/>.
        /// </summary>
        /// <param name="items">Array de elementos.</param>
        /// <param name="item">Item a cambiar de posicion.</param>
        /// <param name="pos">Posicion destino.</param>
        /// <returns>
        ///     <paramref name="items"/> ordenado.
        /// </returns>
        State[] Swap(State[] items, State item)
        {
            for (int i = 2; i < items.Length; i++)
            {
                //Si el estado i = igual al estado que tiene que estar primero, entonces lo colocamos de primero.
                if (items[i] == item)
                {
                    items[i] = items[0];
                    items[0] = item;
                    break;
                }
            }
            return items;
        }
        //Orden estandar de evaluaciones de estado. Posiciones cambiables(2,3,4,...,n)
        State[] order = {
                State.Comment,
                State.String,
                State.Number,
                State.Word,
                State.Symbol,
                State.Ignorable,
            };
        public State Evaluate(string lexeme, State LastState)
        {
            
            //Si el ultimo estado no es el default(inicializador) ni tampoco es las posiciones no cambiables
            //ni tampoco la primera posicion cambiable, entonces reordenamos.
            if (LastState != State.Default && LastState != State.NotAcepted && LastState != State.Ignorable)
            {
                order = Swap(order, LastState);
            }
            //Visitamos cada estado en el orden establecido, si el lexema es igual al estado que se esta evaluando, entonces devuelve ese estado.
            foreach (State state in order)
            {
                switch (state)
                {
                    case State.Comment:
                        if (Regex.IsMatch(lexeme, "(^//.*$)"))
                        {
                            return state;
                        }
                        break;
                    case State.Word:
                        if (Regex.IsMatch(lexeme, "^[a-zA-Z_][a-zA-z0-9_]*$"))
                        {
                            return state;
                        }
                        break;
                    case State.Number:
                        if (Regex.IsMatch(lexeme, @"^[0-9]+\.{0,1}[0-9]*$"))
                        {
                            return state;
                        }
                        break;
                    case State.String:
                        if (Regex.IsMatch(lexeme, "^((\"[^\"]*\")|(\"[^\"]*))$"))
                        {
                            return state;
                        }
                        break;
                    case State.Symbol:
                        if (Regex.IsMatch(lexeme, @"^([!<>;,\.;+\-*/()={}]|(\+\+|--|<=|>=|\+=|-=|==))$"))
                        {
                            return state;
                        }
                        break;
                    case State.Ignorable:
                        if (Regex.IsMatch(lexeme, @"^[\s\t]|[\s\t]$"))
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
            for (int L = 0; L < lines.Length; L++)
            {
                for (int C = 0; C < lines[L].Length; C++)
                {
                    //Sacamos el caracter a evaluar de la linea.
                    char _char = lines[L][C];
                    //Evaluamos al caracter junto al lexema actual.
                    State NewState = Evaluate(NewLexicon.Text + _char, LastState);
                    if(NewState != LastState && NewState != State.Comment)
                    {
                        if(LastState != State.Default && LastState != State.Ignorable && NewLexicon.Text.Length > 0)
                        {
                            //Le colocamos al tipo de estado al que pertenece el lexema.
                            NewLexicon.State = LastState;
                            //Y algunos datos extras.
                            NewLexicon.Line = L;
                            NewLexicon.Column = C - NewLexicon.Text.Length;
                            //Agregamos el nuevo lexema a la lista de lexemas.
                            Lexicons.Add(NewLexicon);
                        }
                        //Inicializamos el nuevo lexema con el caracter actual.
                        NewLexicon = new Lexeme() { Text = string.Empty };
                        //Si el nuevo estado no es un ignorable, entonces vuelvo a revisar el caracter.
                        if (NewState != State.Ignorable)
                        {
                            C--;
                        }
                    }
                    else
                    {
                        NewLexicon.Text += _char.ToString();
                    }
                    //Asegurador para lexema final.
                    if (C == lines[L].Length - 1 && NewLexicon.Text.Length > 0)
                    {
                        //Le colocamos al tipo de estado al que pertenece el lexema.
                        NewLexicon.State = NewState;
                        //Y algunos datos extras.
                        NewLexicon.Line = L;
                        NewLexicon.Column = C - NewLexicon.Text.Length;
                        //Agregamos el nuevo lexema a la lista de lexemas.
                        Lexicons.Add(NewLexicon);
                        //Inicializamos el nuevo lexema con el caracter actual.
                        NewLexicon = new Lexeme() { Text = string.Empty };
                    }
                    LastState = NewState;
                }
            }
            return Lexicons;
        }
    }
}
