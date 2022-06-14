using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Core.Models;

namespace Compiler.Core.Services.Error_Manager
{
    internal class ErrorHandler
    {
        private static readonly List<Error> errors = new List<Error>();
        public static void AddError(Error error)
        {
            errors.Add(error);
        }
        public static List<Error> Errors => errors;
        public static void Clean() { errors.Clear(); }
    }
}
