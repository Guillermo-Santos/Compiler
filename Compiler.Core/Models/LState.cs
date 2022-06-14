namespace Compiler.Core.Models
{
    /// <summary>
    /// Estados lexicos.
    /// </summary>
    public enum LState
    {
        /// <summary>
        /// Primer estado usado para iniciar el analizador.
        /// </summary>
        Default,
        /// <summary>
        /// Estado que representa que el lexema es una palabra, ya sea reservada, o el identificador de una variable.
        /// </summary>
        Word,
        /// <summary>
        /// Estado que representa un numero, ya sea un numero entero o un numero real.
        /// </summary>
        Number,
        /// <summary>
        /// Estado que representa una cadena de texto.
        /// </summary>
        String,
        /// <summary>
        /// Estado que representa un simbolo aceptado por el lenguaje.
        /// </summary>
        Symbol,
        /// <summary>
        /// Estado que representa a los comentarios.
        /// </summary>
        Comment,
        /// <summary>
        /// Delimitadores de tokens, casos como un espacio, final de linea, tabulaciones, etc. que seran ignorados.
        /// </summary>
        Ignorable,
        /// <summary>
        /// Estado que representa, de manera general, todo caracter no aceptado por el lenguaje.
        /// </summary>
        NotAcepted,
        /// <summary>
        /// Estado que simbolisa el final de una linea.
        /// </summary>
        Line,
    }
}
