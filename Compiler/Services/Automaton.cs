using Compiler.Services.Interfaces;
using System.Text.RegularExpressions;

namespace Compiler.Services
{
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
        public bool Recognize(string texto, int iniToken, ref int i, int noAuto)
        {
            if (i >= texto.Length - 1)
                return false;
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
                c = NextChar(ref i);
                switch (_edoAct)
                {
                    //--- Automata delim---

                    case 0:

                        if ((@"\s\n\r\t").IndexOf(c) >= 0)
                            _edoAct = 1;
                        else
                        {
                            i = iniToken;
                            return false;
                        }
                        break;

                    case 1:

                        if ((@"\s\n\r\t").IndexOf(c) >= 0)
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
                        if (("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz").IndexOf(c) >= 0)
                            _edoAct = 4;
                        else
                        {
                            i = iniToken;

                            return false;
                        }
                        break;

                    case 4:
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

                        if (("0123456789").IndexOf(c) >= 0) _edoAct = 7;
                        else
                        {
                            i = iniToken;

                            return false;
                        }
                        break;

                    case 7:

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

                        if (("=").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if ((";").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if ((",").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if ((".").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if (("+").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if (("-").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if (("*").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if (("/").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if (("(").IndexOf(c) >= 0)
                            _edoAct = 10;
                        else if ((")").IndexOf(c) >= 0)
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
                        if (("\"").IndexOf(c) >= 0)
                            _edoAct = 12;
                        else
                        {
                            i = iniToken;
                            return false;
                        }
                        break;
                    case 12:
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
        }
    }
}
