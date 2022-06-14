using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compiler.Core.Services.Analyzers.Lexic;
using Compiler.Core.Services.Analyzers.Sintax;
using Compiler.Core.Models;
using System.Collections.ObjectModel;
using Compiler.Models.Templates;

namespace Compiler.ViewModels
{
    internal class SyntaxAnalyzerTestViewModel:BaseViewModel
    {
        ObservableCollection<SyntaxTemplate> tree = new ObservableCollection<SyntaxTemplate>();
        public ObservableCollection<SyntaxTemplate> Tree
        {
            get => tree;
            set => SetProperty(ref tree, value);
        }
        SintaxAnalyzer SintaxAnalyzer = new SintaxAnalyzer();
        LexiconAnalyzer LexiconAnalyzer = new LexiconAnalyzer();
        public void Analyze(string Text)
        {
            Tree.Clear();
            var Sync = SintaxAnalyzer.Analyze((List<Lexicon>)LexiconAnalyzer.Analyze(Text));
            List<SyntaxTemplate> temp = PrepareData((List<Syntacticon>)Sync);
            foreach (var item in temp)
            {
                Tree.Add(item);
            }
        }
        List<SyntaxTemplate> PrepareData(List<Syntacticon> syntacticons)
        {
            if (syntacticons == null)
                return new List<SyntaxTemplate>();
            List<SyntaxTemplate> data = new List<SyntaxTemplate>();
            
            foreach(Syntacticon syntacticon in syntacticons)
            {
                SyntaxTemplate node = new SyntaxTemplate()
                {
                    Symbol = Enum.GetName(typeof(SyState), syntacticon.State),
                    Name = syntacticon.Lexicon == null ? string.Empty : syntacticon.Lexicon.Lexeme,
                };
                node.Type = (syntacticon.SyntacticonList.Count > 0) ? SyntaxTemplate.SyntaxItemType.Basic : SyntaxTemplate.SyntaxItemType.Child;
                var childs = PrepareData(syntacticon.SyntacticonList);
                foreach(var child in childs)
                {
                    node.Children.Add(child);
                }
                data.Add(node);
            }
            return data;
        }
        public void Clean()
        {
            Tree.Clear();
        }
    }
}
