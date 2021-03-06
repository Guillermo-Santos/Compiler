namespace Compiler.Core.Models
{
    public enum Token
    {
        /// <summary>
        /// Identificador de una variable.
        /// </summary>
        Id,
        /// <summary>
        /// Numero.
        /// </summary>
        num,
        Cad,
        /// <summary>
        /// Tipo de dato.
        /// </summary>
        DType,
        #region Palabras reservadas
        Const,
        Read,
        Show,
        //Partial,
        //NameSpace,
        //ReadOnly,
        #region Tipos
        Var,
        Int,
        Float,
        String,
        #endregion
        #endregion
        #region Simbolos
        /// <summary>
        /// !
        /// </summary>
        Negacion,
        /// <summary>
        /// .
        /// </summary>
        Punto,
        /// <summary>
        /// ;
        /// </summary>
        PuntoComa,
        /// <summary>
        /// :
        /// </summary>
        DosPuntos,
        /// <summary>
        /// ,
        /// </summary>
        Coma,
        /// <summary>
        /// (
        /// </summary>
        ParentesisAbierto,
        /// <summary>
        /// )
        /// </summary>
        ParentesisCerrado,
        /// <summary>
        /// {
        /// </summary>
        LlaveAbierta,
        /// <summary>
        /// }
        /// </summary>
        LlaveCerrada,
        #region Operadores
        Igual,
        Op_Aritmetico,
        Op_Atribucion,
        /// <summary>
        /// += | -=
        /// </summary>
        Op_Asignacion,
        Op_Boolean,
        Op_Relacional,
        Op_Logicos,
        #endregion
        #endregion
        /// <summary>
        /// Lexema no aceptado por el lenguaje.
        /// </summary>
        NotDefined,
        Line,
        Class,
    }
}
