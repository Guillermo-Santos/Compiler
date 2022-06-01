using Compiler.Services;
using Compiler.Models;
using System.Collections.ObjectModel;

namespace Compiler.ViewModels
{
    internal class LexicAnalyzerTestViewModel : BaseViewModel
    {
        readonly LexiconAnalyzer LexiconAnalyzer = new LexiconAnalyzer();
        
        ObservableCollection<Lexicon> lexicons = new ObservableCollection<Lexicon>();
        string text = string.Empty;
        public ObservableCollection<Lexicon> Lexicons
        {
            get => lexicons;
            set => SetProperty(ref lexicons, value);
        }
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public LexicAnalyzerTestViewModel()
        {
            Title = "Prueba de analizador lexico";
        }

        public void Analyze()
        {
            Lexicons.Clear();
            var lex = LexiconAnalyzer.Analyze(Text);
            foreach (var item in lex)
            {
                Lexicons.Add(item);
            }
        }
        public void Clean()
        {
            Text = string.Empty;
            Lexicons.Clear();
        }
        public void Exit()
        {
            Windows.UI.Xaml.Application.Current.Exit();
        }
    }
}
