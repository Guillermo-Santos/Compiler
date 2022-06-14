using Compiler.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Services.Analyzers.Sintax
{
    public class Syntaxis
    {
        public static readonly List<SyState[]> Clase = new List<SyState[]>()
        {
            new SyState[]{
                SyState.Class,
                SyState.Id,
                SyState.ParentesisAbierto,
                SyState.ParentesisCerrado,
                SyState.LlaveAbierta,
                SyState.Sentencia,
                SyState.LlaveCerrada,
            },
        };
        public static readonly List<SyState[]> Sentencia = new List<SyState[]>()
        {
            new SyState[] { 
                SyState.Declaracion
            },
            new SyState[]
            {
                SyState.Sentencia, SyState.Declaracion
            },
            new SyState[] { 
                SyState.FOR
            },
            new SyState[] { 
                SyState.IF
            },
            new SyState[]
            {
                SyState.IF_ELSE
            },
        };
        public static readonly List<SyState[]> Declaracion = new List<SyState[]>()
        {
            new SyState[]
            {
                SyState.DType,
                SyState.Definicion
            },
            new SyState[]
            {
                SyState.DType,
                SyState.Id,
                SyState.PuntoComa
            },
            new SyState[]
            {
                SyState.DType,
                SyState.Id,
                SyState.Igual,
                SyState.Num,
                SyState.PuntoComa
            },
            new SyState[]
            {
                SyState.DType,
                SyState.Id,
                SyState.Igual,
                SyState.Cad,
                SyState.PuntoComa
            },
        };
        public static readonly List<SyState[]> Definicion = new List<SyState[]>()
        {
            // id = num;
            new SyState[]
            {
                SyState.Id,
                SyState.Igual,
                SyState.Num,
                SyState.PuntoComa
            },
            // id = cad;
            new SyState[]
            {
                SyState.Id,
                SyState.Igual,
                SyState.Cad,
                SyState.PuntoComa
            },
            // id = id;
            new SyState[]
            {
                SyState.Id,
                SyState.Igual,
                SyState.Id,
                SyState.PuntoComa
            }
        };
    }
}
