using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Core.Models;

namespace Compiler.Core.Services.Analyzers.Sintax
{
    public class SintaxAnalyzer
    {
        List<Sentence> sentenceList = new List<Sentence>();
        Automaton Automaton = new Automaton();
        int cont = 0;
        //bool IsClass(SyState[] secuence, ref int index)
        //{
        //    List<bool[]> bools = new List<bool[]>();
        //    foreach (SyState[] Dclase in Syntaxis.Clase)
        //    {
        //        bool[] isClass = new bool[Dclase.Length];
        //        for(int i = 0; i < Dclase.Length; i++)
        //        {
        //            switch (Dclase[i])
        //            {
        //                case SyState.Sentencia:
        //                    if(IsSentence(secuence, ref index))
        //                    {
        //                        isClass[i] = true;
        //                    }
        //                    else
        //                    {
        //                        isClass[i] = false;
        //                    }
        //                    break;
        //                default:
        //                    if(secuence[index] == Dclase[i])
        //                    {
        //                        isClass[i] = true;
        //                    }
        //                    else
        //                    {
        //                        isClass[i] = false;
        //                    }
        //                    index++;
        //                    break;
        //            }
        //        }
        //        bools.Add(isClass);
        //    }

        //    //{ false, false, false, false, false, false };
        //    return false;
        //}
        //bool IsSentence(SyState[] secuence, ref int index)
        //{
        //    return false;
        //}
        //bool IsDeclaration(SyState[] secuence, ref int index)
        //{
        //    return false;
        //}
        //bool IsDefinition(SyState[] secuence, ref int index) { 
        //    return false;
        //}
        /// <summary>
        /// return true or false depending on if all the members of the array are true or not.
        /// </summary>
        /// <param name="bools"></param>
        /// <returns></returns>
        bool IsAllTrue(bool[] bools)
        {
            foreach(bool s in bools)
            {
                if (!s)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Return the index and number of true of the array with more true on it
        /// </summary>
        /// <param name="bools"></param>
        /// <returns></returns>
        int[] GetMostTrue(List<bool[]> bools)
        {
            int TrueIndex = 0;
            int count = 0;
            for (int i=0; i<bools.Count; i++)
            {
                int cont = 0;
                foreach(bool b in bools[i])
                {
                    if (b)
                    {
                        cont++;
                    }   
                }
                if(cont > count)
                {
                    TrueIndex = i;
                    count = cont;
                }
            }
            return new int[]{ TrueIndex, count};
        }
        public bool Evaluate(SyState[] states,List<SyState[]> evaluator, ref int index, out Syntacticon Tree)
        {
            Tree = new Syntacticon();
            Syntacticon sentence;
            int begin = index;
            //List<Token> rule = new List<Token>();
            List<bool[]> results = new List<bool[]>();
            foreach (SyState[] evalua in evaluator)
            {
                if (index >= states.Length) { break; }
                bool[] result = new bool[evalua.Length];
                for(int i = 0; i < evalua.Length; i++)
                {
                    if (index >= states.Length) { break; }
                    switch (evalua[i])
                    {
                        case SyState.Clase:
                            if (Evaluate(states.ToArray(), Syntaxis.Clase, ref index, out sentence))
                            {
                                sentence.State = evalua[i];
                                Tree.SyntacticonList.Add(sentence);
                                result[i] = true;
                                index++;
                            }
                            else
                            {
                                result[i] = false;
                            }
                            break;
                        case SyState.Sentencia:
                            if (Evaluate(states.ToArray(), Syntaxis.Sentencia, ref index, out sentence))
                            {
                                sentence.State = evalua[i];
                                Tree.SyntacticonList.Add(sentence);
                                result[i] = true;
                                index++;
                            }
                            else
                            {
                                result[i] = false;
                            }
                            break;
                        case SyState.Declaracion:
                            if (Evaluate(states.ToArray(), Syntaxis.Declaracion, ref index, out sentence))
                            {
                                sentence.State = evalua[i];
                                Tree.SyntacticonList.Add(sentence);
                                result[i] = true;
                                index++;
                            }
                            else
                            {
                                result[i] = false;
                            }
                            break;
                        case SyState.Definicion:
                            if (Evaluate(states.ToArray(), Syntaxis.Definicion, ref index, out sentence))
                            {
                                sentence.State = evalua[i];
                                Tree.SyntacticonList.Add(sentence);
                                result[i] = true;
                                index++;
                            }
                            else
                            {
                                result[i] = false;
                            }
                            break;
                        default:
                            if (states[index] == evalua[i])
                            {
                                Tree.SyntacticonList.Add(new Syntacticon()
                                {
                                    Lexicon = myevaluator[index],
                                    State = states[index],
                                    Parent = Tree
                                });
                                result[i] = true;
                                index++;
                            }
                            else
                            {
                                result[i] = false;
                            }
                            break;
                    }
                }
                if (IsAllTrue(result))
                {
                    return true;
                }
                else
                {
                    results.Add(result);
                }
            }
            int[] ints = GetMostTrue(results);
            if (ints[1] == 0)
            {
                return false;
            }
            return true;
        }
        List<Lexicon> myevaluator = new List<Lexicon>();
        public IEnumerable<Syntacticon> Analyze(List<Lexicon> lexicons)
        {
            List<Syntacticon> Trees = new List<Syntacticon>();
            Syntacticon Tree;
            myevaluator = lexicons;   
            int index = 0;
            List<SyState> sentence = new List<SyState>();
            //List<Token> rule = new List<Token>();
            foreach (Lexicon lexicon in lexicons)
            {
                sentence.Add((SyState)((int)lexicon.Token));
            }
            sentence.Add(SyState.SATerminator);
            SyState[] evaluators = { SyState.Clase, SyState.Sentencia, SyState.Declaracion, SyState.Definicion };
            foreach (var evaluator in evaluators)
            {
                switch (evaluator)
                {

                    case SyState.Clase:
                        if (Evaluate(sentence.ToArray(), Syntaxis.Clase, ref index, out Tree))
                        {
                            Tree.State = evaluator;
                            Trees.Add(Tree);
                        }
                        else
                        {
                        }
                        break;
                    case SyState.Sentencia:
                        if (Evaluate(sentence.ToArray(), Syntaxis.Sentencia, ref index, out Tree))
                        {
                            Tree.State = evaluator;
                            Trees.Add(Tree);
                        }
                        else
                        {
                        }
                        break;
                    case SyState.Declaracion:
                        if (Evaluate(sentence.ToArray(), Syntaxis.Declaracion, ref index, out Tree))
                        {
                            Tree.State = evaluator;
                            Trees.Add(Tree);
                        }
                        else
                        {
                        }
                        break;
                    case SyState.Definicion:
                        if (Evaluate(sentence.ToArray(), Syntaxis.Definicion, ref index, out Tree))
                        {
                            Tree.State = evaluator;
                            Trees.Add(Tree);
                        }
                        else
                        {
                        }
                        break;
                }
                if (index >= sentence.Count) { break; }
            }
            myevaluator.Clear();
            return Trees;
        }
    }
}
