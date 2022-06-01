using Compiler.Models;
using Compiler.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compiler.Services
{
    internal class LexiconAnalyzer : ILexiconAnalyzer
    {
        const int TOKREC = 5;
        //const int MAXTOKENS = 500;
        int _i;
        int _iniToken;
        readonly Automaton Automaton = new Automaton();
        public IEnumerable<Lexicon> Analyze(string text)
        {
            _i = 0;
            _iniToken = 0;
            List<Lexicon> tokens = new List<Lexicon>();
            bool recAuto; 
            int noAuto;

            while (_i < text.Length)
            {

                recAuto = false; noAuto = 0;

                while( noAuto < TOKREC && !recAuto)
                {
                    if (Automaton.Recognize(text, _iniToken, ref _i, noAuto)) 
                        recAuto = true; 
                    else 
                        noAuto++;
                }
                if (recAuto)
                {
                    Lexicon lexicon = new Lexicon
                    {
                        Lexeme = text.Substring(_iniToken, _i - _iniToken)
                    };
                    switch (noAuto)
                    {
                        //--- Automata delim---
                        case 0:
                            break;
                        //--- Automata id---
                        case 1:
                            if (IsId(lexicon.Lexeme))
                                lexicon.Token = "id";
                            else
                                lexicon.Token = lexicon.Lexeme;
                            break;
                        //--- Automata num---

                        case 2:
                            lexicon.Token = "num";
                            break;

                        //--- Automata otros---

                        case 3:
                            lexicon.Token = lexicon.Lexeme;
                            break;

                        //--- Automata cad---

                        case 4:
                            lexicon.Token = "cad";
                            break;
                    }
                    if (noAuto != 0)
                        tokens.Add(lexicon);
                }
                else
                {
                    _i++;
                }
                _iniToken = _i;

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
