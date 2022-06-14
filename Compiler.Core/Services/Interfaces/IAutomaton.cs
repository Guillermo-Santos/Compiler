namespace Compiler.Core.Services.Interfaces
{
    /// <summary>
    /// Interfaz usada como base para un automata.
    /// </summary>
    /// <typeparam name="T1">Tipo que devuelve la funcion de reconocimiento.</typeparam>
    /// <typeparam name="T2">Tipo que devuelve la funcion de evaluacion.</typeparam>
    public interface IAutomaton<T1, T2>
    {
        /// <summary>
        /// Metodo utilizado para reconocer una cadena de texto.
        /// </summary>
        /// <param name="texto">Texto a reconocer.</param>
        /// <returns>
        ///     Un <see cref="T1"/> sacado del <paramref name="texto"/>.
        /// </returns>
        T1 Recognize(string texto);
        /// <summary>
        /// Metodo para evaluar una cadena de texto.
        /// </summary>
        /// <param name="lexeme">Lexema o cadena de texto a evauluar.</param>
        /// <param name="LastState">Ultimo <see cref="T2"/> evaluado, usado para reorganizar la evaluacion.</param>
        /// <returns>
        ///     Un <see cref="T2"/> evaluado del <paramref name="lexeme"/> que puede o no ser igual a <paramref name="LastState"/>.
        /// </returns>
        T2 Evaluate(string lexeme, T2 LastState);
    }
}
