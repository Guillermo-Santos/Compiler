using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Models
{
    public class Syntacticon
    {
        public Syntacticon Parent { get; set; }
        public SyState State { get; set; }
        public Lexicon Lexicon { get; set; }
        public bool IsTerminal => Lexicon != null;
        public List<Syntacticon> SyntacticonList { get; set; } = new List<Syntacticon>();
        public bool IsError { get; set; } = false;
    }
}
