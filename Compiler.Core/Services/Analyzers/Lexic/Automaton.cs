using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using Compiler.Core.Services.Interfaces;
using Compiler.Core.Models;

namespace Compiler.Core.Services.Analyzers.Lexic
{
    /// <summary>
    /// Automata del analizador lexico.
    /// </summary>
    public class Automaton : IAutomaton<IEnumerable<Lexeme>, LState>
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
        LState[] Swap(LState[] items, LState item)
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
        public LState Evaluate(string lexeme, LState LastState)
        {
            //Orden estandar de evaluaciones de estado. Posiciones cambiables(2,3,4,...,n)
            LState[] order = {
                LState.Comment,
                LState.String,
                LState.Number,
                LState.Word,
                LState.Symbol,
                LState.Line,
                LState.Ignorable,
            };

            //Si el ultimo estado no es el default(inicializador) ni tampoco es las posiciones no cambiables
            //ni tampoco la primera posicion cambiable, entonces reordenamos.
            if (LastState != LState.Default && LastState != LState.NotAcepted && LastState != LState.Ignorable && LastState != LState.Line)
            {
                order = Swap(order, LastState);
            }
            //Visitamos cada estado en el orden establecido, si el lexema es igual al estado que se esta evaluando, entonces devuelve ese estado.
            foreach (LState state in order)
            {
                switch (state)
                {
                    case LState.Comment:
                        if (Regex.IsMatch(lexeme, "(^//.*$)"))
                        {
                            return state;
                        }
                        break;
                    case LState.Word:
                        if (Regex.IsMatch(lexeme, "^[a-zA-Z_][a-zA-z0-9_]*$"))
                        {
                            return state;
                        }
                        break;
                    case LState.Number:
                        if (Regex.IsMatch(lexeme, @"^[0-9]+\.{0,1}[0-9]*$"))
                        {
                            return state;
                        }
                        break;
                    case LState.String:
                        if (Regex.IsMatch(lexeme, "^((\"[^\"]*\")|(\"[^\"]*))$"))
                        {
                            return state;
                        }
                        break;
                    case LState.Symbol:
                        if (Regex.IsMatch(lexeme, @"^([!<>;,\.;+\-*/()={}]|(\+\+|--|<=|>=|\+=|-=|==))$"))
                        {
                            return state;
                        }
                        break;
                    case LState.Ignorable:
                        if (Regex.IsMatch(lexeme, @"[\s\t]$"))
                        {
                            return state;
                        }
                        break;
                    case LState.Line:
                        if (Regex.IsMatch(lexeme, "\r$"))
                        {
                            return state;
                        }
                        break;
                }
            }
            return LState.NotAcepted;
        }
        public IEnumerable<Lexeme> Recognize(string texto)
        {
            //Inicializar conjunto a devolver.
            List<Lexeme> Lexicons = new List<Lexeme>();
            //Inicializar variable temporal usada para llenar el conjunto a devolver en cada iteracion.
            Lexeme NewLexicon = new Lexeme() { Text = string.Empty };
            //Preparando el ultimo estado para la primera iteracion
            LState LastState = LState.Default;
            int L = 0;
            for (int i = 0; i < texto.Length; i++)
            {
                //Sacamos el caracter a evaluar de la linea.
                char _char = texto[i];
                //Evaluamos al caracter junto al lexema actual.
                LState NewState = Evaluate(NewLexicon.Text + _char, LastState);
                if (NewState != LastState && NewState != LState.Comment)
                {
                    if (LastState != LState.Default && LastState != LState.Ignorable && LastState != LState.Line && NewLexicon.Text.Length > 0)
                    {
                        //Le colocamos al tipo de estado al que pertenece el lexema.
                        NewLexicon.State = LastState;
                        //Y algunos datos extras.
                        NewLexicon.Line = L;
                        NewLexicon.Column = i - NewLexicon.Text.Length;
                        //Agregamos el nuevo lexema a la lista de lexemas.
                        Lexicons.Add(NewLexicon);
                    }
                    //Inicializamos el nuevo lexema con el caracter actual.
                    NewLexicon = new Lexeme() { Text = string.Empty };
                    //Si el nuevo estado no es un ignorable, entonces vuelvo a revisar el caracter.
                    if (NewState != LState.Ignorable)
                    {
                        i--;
                    }
                    else if (LastState == LState.Line)
                    {
                        L++;
                    }
                }
                else
                {
                    NewLexicon.Text += _char.ToString();
                }
                LastState = NewState;
            }

            return Lexicons;
        }
    }
}
