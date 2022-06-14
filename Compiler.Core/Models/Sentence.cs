using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Core.Models
{
    public struct Secuence
    {
        public Token[] Tokens { get; set; }
    }
    public class Sentence
    {
        public Secuence[] Secuencias { get; set; }
    }
}
