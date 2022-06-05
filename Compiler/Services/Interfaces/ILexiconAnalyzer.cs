using Compiler.Models;
using System.Collections.Generic;

namespace Compiler.Services.Interfaces
{
    internal interface ILexiconAnalyzer
    {
        /// <summary>
        /// Realiza el analisis lexico una cadena de texto <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Cadena de texto a analizar.</param>
        /// <returns>
        ///     Un <see cref="IEnumerable{T}"/> de tipo <seealso cref="Lexicon"/>
        ///     obtenido  del <paramref name="text"/>.
        /// </returns>
        IEnumerable<Lexicon> Analyze(string text);
        /// <summary>
        /// Metodo que identifica un lexeme <paramref name="Lexeme"/> 
        /// dentro de las palabra reservadas del lenguaje.
        /// </summary>
        /// <param name="Lexeme">Lexema a identificar</param>
        /// <returns> 
        ///     Si el <paramref name="Lexeme"/> en cuestion pertenece o no a las palabras reservadas
        /// </returns>
        bool IsId(string Lexeme);
    }
}
