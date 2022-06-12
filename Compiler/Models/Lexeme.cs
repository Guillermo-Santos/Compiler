namespace Compiler.Models
{
    internal class Lexeme
    {
        /// <summary>
        /// Texto del lexema.
        /// </summary>
        public string Text;
        /// <summary>
        /// Estado del lexema.
        /// </summary>
        public State State;
        /// <summary>
        /// Linea en que se encuentra el lexema.
        /// </summary>
        public int Line;
        /// <summary>
        /// Columna en la que se encuentra el lexema.
        /// </summary>
        public int Column;
        /// <summary>
        /// Tamaño del lexema.
        /// </summary>
        public int Lenght => Text.Length;
    }
}
