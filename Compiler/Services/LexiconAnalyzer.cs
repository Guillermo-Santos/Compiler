using Compiler.Models;
using Compiler.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compiler.Services
{
    internal class LexiconAnalyzer : ILexiconAnalyzer<Automaton, IEnumerable<Lexicon>>
    {
        Automaton _automaton { get; set; }
        public Automaton Automaton { 
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
            foreach(Lexeme lexeme in lexemes)
            {
                Lexicon lexicon = new Lexicon()
                {
                    Lexeme = lexeme.Text
                };
                switch (lexeme.State)
                {
                    case State.Word:
                        if (IsId(lexicon.Lexeme))
                            lexicon.Token = "id";
                        else
                            lexicon.Token = lexicon.Lexeme;
                        break;
                    case State.Number:
                        lexicon.Token = "num";
                        break;
                    case State.String:
                        lexicon.Token = "cad";
                        break;
                    case State.Symbol:
                        lexicon.Token = lexicon.Lexeme;
                        break;
                }
                tokens.Add(lexicon);
            }
            return tokens;
        }

        public bool IsId(string Lexeme)
        {
            string[] reswords = { 
                "inicio", 
                "fin", 
                "const", 
                "var",
                "entero",
                "real",
                "cadena",
                "leer",
                "visua"
            };
            for (int i = 0; i < reswords.Length; i++)
                if (Lexeme == reswords[i])
                    return false;
            return true;
        }
    }
}
