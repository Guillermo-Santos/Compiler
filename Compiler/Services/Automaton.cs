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
        Delim,//Delimitadores de tokens, casos como un espacio, final de linea, tabulaciones, etc. que seran ignorados.
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
        /// <summary>
        /// Cambia de posicion el elemento <paramref name="item"/> con el elemento de la posicion <paramref name="pos"/> en <paramref name="items"/>.
        /// </summary>
        /// <param name="items">Array de elementos.</param>
        /// <param name="item">Item a cambiar de posicion.</param>
        /// <param name="pos">Posicion destino.</param>
        /// <returns>
        ///     <paramref name="items"/> ordenado.
        /// </returns>
        State[] Swap(State[] items, State item, int pos)
        {
            for(int i = 2; i<items.Length; i++)
            {
                //Si el estado i = igual al estado que tiene que estar primero, entonces lo colocamos de primero.
                if(items[i] == item)
                {
                    items[i] = items[pos];
                    items[pos] = item;
                    break;
                }
            }
            return items;
        }
        public State Evaluate(string lexeme, State LastState)
        {
            //Orden estandar de evaluaciones de estado. Posiciones cambiables(2,3,4,...,n)
            State[] order = { 
                State.String,
                State.NotAcepted,
                State.Number,
                State.Word,
                State.Symbol
            };
            //Si el ultimo estado no es el default(inicializador) ni tampoco es las posiciones no cambiables
            //ni tampoco la primera posicion cambiable, entonces reordenamos.
            if (LastState != State.Default && LastState != order[0] && LastState != order[1] && LastState != order[2])
            {
                order = Swap(order, LastState, 2);
            }
            //Visitamos cada estado en el orden establecido, si el lexema es igual al estado que se esta evaluando, entonces devuelve ese estado.
            foreach (State state in order)
            {
                switch (state)
                {
                    case State.NotAcepted:
                        if (Regex.IsMatch(lexeme, @"[\s\n\c\b\t\r]"))
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
                        if (Regex.IsMatch(lexeme, "\"[^\"]*\"|\"[^\"]*"))
                        {
                            return state;
                        }
                        break;
                    case State.Symbol:
                        if (Regex.IsMatch(lexeme, @"[!<>,\.;+\-*/()=]"))
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
                for (int C = 0; C < lines[L].Length; C++)
                {
                    //Sacamos el caracter a evaluar de la linea.
                    char c = lines[L][C];
                    //Evaluamos al caracter junto al lexema actual.
                    State NewState = Evaluate(NewLexicon.Text + c, LastState);
                    //Constrains propios de del nuevo estado
                    switch (NewState)
                    {
                        case State.Word:
                            {
                                if (Regex.IsMatch(c.ToString(), @"[a-zA-Z0-9_]") && LastState == NewState)
                                {
                                    //Agregamos el caracter actual al nuevo lexema.
                                    NewLexicon.Text += c;
                                }
                                else
                                {
                                    if(LastState != State.Default && LastState != State.NotAcepted && NewLexicon.Text.Length > 0)
                                    {
                                        //Le colocamos al tipo de estado al que pertenece el lexema.
                                        NewLexicon.State = LastState;
                                        //Agregamos el nuevo lexema a la lista de lexemas.
                                        Lexicons.Add(NewLexicon);
                                        //Inicializamos el nuevo lexema con el caracter actual.
                                        NewLexicon = new Lexeme() { Text = string.Empty };
                                    }
                                    C--;
                                }
                                break;
                            }
                        case State.Number:
                            {
                                if (Regex.IsMatch(c.ToString(), @"[0-9.]") && Regex.IsMatch(NewLexicon.Text + c, @"[0-9]+\.{0,1}[0-9]*"))
                                {
                                    //Agregamos el caracter actual al nuevo lexema.
                                    NewLexicon.Text += c;
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

                                    if (LastState == State.Symbol && Regex.IsMatch(NewLexicon.Text+c, "\"[^\"]*"))
                                    {
                                        if (NewLexicon.Text.Length > 0)
                                        {

                                            //Le colocamos al tipo de estado al que pertenece el lexema.
                                            NewLexicon.State = LastState;
                                            //Agregamos el nuevo lexema a la lista de lexemas.
                                            Lexicons.Add(NewLexicon);
                                            //Inicializamos el nuevo lexema con el caracter actual.
                                            NewLexicon = new Lexeme() { Text = c.ToString() };
                                        }
                                    }
                                    else
                                    {
                                        //Agregamos el caracter actual al nuevo lexema.
                                        NewLexicon.Text += c;
                                    }
                                }
                                break;
                            }
                        case State.Symbol:
                            {
                                if (Regex.IsMatch(c.ToString(), @"[!<>;,\.;+\-*/()=]"))
                                {
                                    //Si el simbolo sin el nuevo caracter ya es un simbolo compuesto y el simbolo + el nuevo caracter tipo simbolo no es un simbolo compuesto.
                                    if (Regex.IsMatch(NewLexicon.Text, @"\+\+|--|<=|>=|\+=|-=|!=|==") )
                                    {
                                        //Le colocamos al tipo de estado al que pertenece el lexema.
                                        NewLexicon.State = LastState;
                                        //Agregamos el nuevo lexema a la lista de lexemas.
                                        Lexicons.Add(NewLexicon);
                                        //Inicializamos el nuevo lexema con el caracter actual.
                                        NewLexicon = new Lexeme() { Text = string.Empty + c };
                                    }
                                    else
                                    {
                                        //Agregamos el caracter actual al nuevo lexema.
                                        NewLexicon.Text += c;
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
                                /*
                                 * MANEJAR ERRRORES
                                 * IGNORABLES
                                 * Terminador de lineas
                                 */

                                //Si el ultimo estado no es igual al estado actual(estado de "no aceptado")
                                //O el caracter actual no es aceptado
                                if (LastState != NewState || Regex.IsMatch(c.ToString(), @"[\s\n\c\b\t\r]"))
                                {
                                    //si el lexema tiene algun caracter, entonces agregamos el lexema.
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
                    //Asegurador para lexema final.
                    if (C == lines[L].Length - 1 && NewLexicon.Text.Length > 0)
                    {
                        //Le colocamos al tipo de estado al que pertenece el lexema.
                        NewLexicon.State = NewState;
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
