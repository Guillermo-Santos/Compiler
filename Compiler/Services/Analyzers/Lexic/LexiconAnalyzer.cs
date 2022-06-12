using Compiler.Models;
using Compiler.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Compiler.Services.Analyzers.Lexic
{
    partial class LexiconAnalyzer : IAnalyzer<Automaton, IEnumerable<Lexicon>>
    {
        private Automaton _automaton { get; set; }
        public Automaton Automaton
        {
            get => _automaton;
            set => _automaton = value;
        }

        public LexiconAnalyzer()
        {
            Automaton = new Automaton();
        }
        public IEnumerable<Lexicon> Analyze(string text)
        {
            List<Lexicon> tokens = new List<Lexicon>();
            List<Lexeme> lexemes = (List<Lexeme>)Automaton.Recognize(text);
            foreach (Lexeme lexeme in lexemes)
            {
                if(lexeme.State != State.Comment)
                {
                    Lexicon lexicon = new Lexicon()
                    {
                        Lexeme = lexeme.Text
                    };
                    switch (lexeme.State)
                    {
                        case State.Word:
                            lexicon.Token = GetWordToken(lexicon.Lexeme);
                            break;
                        case State.Number:
                            lexicon.Token = Token.num;
                            break;
                        case State.String:
                            lexicon.Token = Token.String;
                            break;
                        case State.Symbol:
                            lexicon.Token = GetSymbolToken(lexicon.Lexeme);
                            break;
                        case State.NotAcepted:
                            lexicon.Token = Token.NotDefined;
                            break;
                    }
                    tokens.Add(lexicon);
                }
            }
            return tokens;
        }

        /// <summary>
        /// Metodo que identifica un <paramref name="Lexeme"/> 
        /// dentro de las palabra reservadas del lenguaje.
        /// </summary>
        /// <param name="Lexeme">Lexema a identificar</param>
        /// <returns> 
        ///     Si el <paramref name="Lexeme"/> en cuestion pertenece o no a las palabras reservadas
        /// </returns>
        public Token GetWordToken(string Lexeme)
        {
            Token[] Tokens =
            {
                Token.Const,
                Token.Var,
                Token.Int,
                Token.Float,
                Token.String,
                Token.Read,
                Token.Show,
                Token.Public,
                Token.Private,
                Token.Protected,
            };
            foreach (Token token in Tokens)
                if (Lexeme.Equals(Enum.GetName(typeof(Token), token).ToLower()))
                    return token;
            return Token.Id;
        }

        public Token GetSymbolToken(string Lexeme)
        {

            switch (Lexeme)
            {
                case "!": return Token.Exclamacion;
                case "=": return Token.Igual;
                case "+": return Token.Mas;
                case "-": return Token.Menos;
                case "*": return Token.Aterisco;
                case "/": return Token.Barra;
                case ".": return Token.Punto;
                case ",": return Token.Coma;
                case ";": return Token.PuntoComa;
                case ":": return Token.DosPuntos;
                case "<": return Token.MenorQue;
                case ">": return Token.MayorQue;
                case "(": return Token.ParentesisAbierto;
                case ")": return Token.ParentesisCerrado;
                case "{": return Token.LlaveAbierta;
                case "}": return Token.LlaveCerrada;
                case "++": return Token.MasMas;
                case "--": return Token.MenosMenos;
                case "<=": return Token.MenorIgualQue;
                case ">=": return Token.MayorIgualQue;
                case "+=": return Token.MasIgual;
                case "-=": return Token.MenosIgual;
                case "==": return Token.IgualIgual;
            }
            return Token.NotDefined;
        }
    }
}
