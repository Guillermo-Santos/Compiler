namespace Compiler.Models
{
    enum Token
    {
        /// <summary>
        /// Identificador de una variable.
        /// </summary>
        Id,
        /// <summary>
        /// Numero.
        /// </summary>
        num,
    #region Palabras reservadas
        Const,
        Read,
        Show,
        Public,
        Private,
        Protected,
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
        Exclamacion,
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
        /// <summary>
        /// -
        /// </summary>
        Menos,
        /// <summary>
        /// +
        /// </summary>
        Mas,
        /// <summary>
        /// *
        /// </summary>
        Aterisco,
        /// <summary>
        /// /
        /// </summary>
        Barra,
        /// <summary>
        /// =
        /// </summary>
        Igual,
        /// <summary>
        /// >
        /// </summary>
        MayorQue,
        /// <summary>
        /// <![CDATA[<]]>
        /// </summary>
        MenorQue,
        /// <summary>
        /// >=
        /// </summary>
        MayorIgualQue,
        /// <summary>
        /// <![CDATA[<=]]>
        /// </summary>
        MenorIgualQue,
        /// <summary>
        /// ++
        /// </summary>
        MasMas,
        /// <summary>
        /// --
        /// </summary>
        MenosMenos,
        /// <summary>
        /// +=
        /// </summary>
        MasIgual,
        /// <summary>
        /// -=
        /// </summary>
        MenosIgual,
        /// <summary>
        /// ==
        /// </summary>
        IgualIgual,
    #endregion
    #endregion
        /// <summary>
        /// Lexema no aceptado por el lenguaje.
        /// </summary>
        NotDefined,
    }
}
