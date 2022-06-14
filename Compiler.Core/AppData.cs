using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Core.Models;

namespace Compiler.Core
{
    internal static class AppData
    {
        public static readonly Token[] ReservedWords =
        {
            Token.Class,
            Token.Const,
            //Token.Var,
            //Token.Int,
            //Token.Float,
            //Token.String,
            Token.Read,
            Token.Show,
        };
        public static readonly string[] DTypes =
        {
            "var",
            "int",
            "Float",
            "string",
        };
        //public static readonly Token[] Symbols;
    }
}
