using Compiler.Models;
using Microsoft.Graphics.Canvas.Effects;
using System.Collections.Generic;
using Windows.AI.MachineLearning;

namespace Compiler.Services.Interfaces
{
    /// <summary>
    /// Interfaz base usada para el analizador lexico.
    /// </summary>
    /// <typeparam name="T1">Tipo del analizador lexico.</typeparam>
    /// <typeparam name="T2">Tipo que retorna el analisis.</typeparam>
    internal interface ILexiconAnalyzer<T1,T2> where T1 : class
    {
        /// <summary>
        /// Automata utilizado por el analizador lexico
        /// </summary>
        T1 Automaton { get; set; }
        /// <summary>
        /// Realiza el analisis lexico una cadena de texto <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Cadena de texto a analizar.</param>
        /// <returns>
        ///     Un <see cref="T2"/> obtenido  del <paramref name="text"/>.
        /// </returns>
        T2 Analyze(string text);
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
