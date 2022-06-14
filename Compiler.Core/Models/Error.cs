using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Models
{
    enum ErrorTypes
    {
        Lexic,
        Syntactic,
        Semantic
    }
    internal class Error
    {
        public ErrorTypes Type { get; set; }
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
