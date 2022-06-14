using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Compiler.Core.Models;
using Compiler.Core.Services.Interfaces;

namespace Compiler.Core.Services.Analyzers.Sintax
{
    public class Automaton
    {
        Token NextToken(Token[] tokens)
        {
            index++;
            if(index < tokens.Length)
            {
                return tokens[index];
            }
            return Token.NotDefined;
        }
        public bool IsType(Token token)
        {
            switch (token)
            {
                case Token.Int:
                case Token.Float:
                case Token.String:
                case Token.Var:
                    return true;
            }
            return false;
        }

        public bool IsDeclaration(Token[] sentence)
        {
            if (
                // TipoDato Identificador PuntoComa
                (
                    IsType(sentence[index]) &&
                    sentence[index+1] == Token.Id &&
                    sentence[index+2] == Token.PuntoComa
                ) ||
                // TipoDato Identificador 
                (
                    IsType(sentence[index]) &&
                    sentence[index+1] == Token.Id &&
                    sentence[index+2] == Token.Igual&&
                    (IsType(sentence[index+3]) || sentence[index + 3] == Token.num) &&
                    sentence[index + 4] == Token.PuntoComa)
                )
            {
                return true;
            }
            return false;
        }
        public bool IsSentence(Token[] sentence)
        {
            if (
                (
                    IsSentence(sentence) && 
                    IsDeclaration(sentence)
                )||
                (
                    IsDeclaration(sentence)
                )
              )
            {
                return true;
            }
            return false;
        }
        int index = 0;
        public void Analyze(SyState[] rule)
        {
            index = 0;
        }
        public SyState[] Evaluate(Token sentence, SyState[] LastState)
        {
            List<SyState> states = new List<SyState>();
            SyState[] order =
            {
                SyState.Declaracion,
                SyState.Definicion,
                SyState.Asignacion,
                SyState.Sentencia,
                SyState.Sentencia_Booleana,
                SyState.Sentencia_For,
                SyState.IF,
                SyState.IF_ELSE,
                SyState.WHILE,
                SyState.DO_WHILE,
                SyState.FOR,
            };
            foreach (SyState state in order)
            {
                switch (state)
                {
                    case SyState.Declaracion:
                        states.Add(state);
                        break;
                    case SyState.Definicion:
                        states.Add(state);
                        break;
                    case SyState.Asignacion:
                         states.Add(state);
                        break;
                }
            }
            return states.ToArray();
        }

        public IEnumerable<Syntacticon> Recognize(Lexicon[] lexicons)
        {
            List<Syntacticon> syntacticons = new List<Syntacticon>();
            Syntacticon syntacticon = new Syntacticon();
            string sentence = "";
            SyState[] LastStates = {0};
            foreach(Lexicon lexicon in lexicons)
            {
                SyState[] NewStates = Evaluate(lexicon.Token,LastStates);
                if(NewStates.Length == 0)
                {

                }
                else
                {

                }
            }
            return syntacticons;
        }

    }
}
