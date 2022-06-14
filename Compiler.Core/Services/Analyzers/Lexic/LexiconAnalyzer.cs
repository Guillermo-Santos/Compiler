using Compiler.Core;
using Compiler.Core.Models;
using Compiler.Core.Services.Analyzers.Lexic;
using Compiler.Core.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Compiler.Core.Services.Analyzers.Lexic
{
    public partial class LexiconAnalyzer : IAnalyzer<Automaton, IEnumerable<Lexicon>>
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
                if(lexeme.State != LState.Comment)
                {
                    Lexicon lexicon = new Lexicon()
                    {
                        Lexeme = lexeme.Text
                    };
                    switch (lexeme.State)
                    {
                        case LState.Word:
                            lexicon.Token = GetWordToken(lexicon.Lexeme);
                            break;
                        case LState.Number:
                            lexicon.Token = Token.num;
                            break;
                        case LState.String:
                            lexicon.Token = Token.String;
                            break;
                        case LState.Symbol:
                            lexicon.Token = GetSymbolToken(lexicon.Lexeme);
                            break;
                        case LState.Line:
                            lexicon.Lexeme = "";
                            lexicon.Token = Token.Line;
                            break;
                        case LState.NotAcepted:
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
            foreach (Token token in AppData.ReservedWords)
                if (Lexeme.Equals(Enum.GetName(typeof(Token), token).ToLower()))
                    return token;
            foreach (string Dtype in AppData.DTypes)
                if (Lexeme.Equals(Dtype))
                    return Token.DType;
            return Token.Id;
        }

        public Token GetSymbolToken(string Lexeme)
        {
            switch (Lexeme)
            {
                case "!":  return Token.Negacion;
                case "=":  return Token.Igual;
                case ".":  return Token.Punto;
                case ",":  return Token.Coma;
                case ";":  return Token.PuntoComa;
                case ":":  return Token.DosPuntos;
                case "(":  return Token.ParentesisAbierto;
                case ")":  return Token.ParentesisCerrado;
                case "{":  return Token.LlaveAbierta;
                case "}":  return Token.LlaveCerrada;
                case "<": 
                case ">": 
                case "<=":
                case ">=":
                case "==": return Token.Op_Relacional;
                case "+": 
                case "-": 
                case "*": 
                case "/": return Token.Op_Aritmetico;
                case "++":
                case "--": return Token.Op_Atribucion;
                case "+=":
                case "-=": return Token.Op_Asignacion;
            }
            return Token.NotDefined;
        }
    }
}
