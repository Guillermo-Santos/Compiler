using Compiler.ViewModels;
using Windows.System;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Compiler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LexicAnalyzerTestPage : Page
    {
        LexicAnalyzerTestViewModel ViewModel;
        public LexicAnalyzerTestPage()
        {
            this.InitializeComponent();
            ViewModel = new LexicAnalyzerTestViewModel();
            DataContext = ViewModel;
        }
        private void analyze_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string Text = null;
            code.Document.GetText(Windows.UI.Text.TextGetOptions.None, out Text);
            ViewModel?.Analyze(Text);
        }

        private void exit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => ViewModel?.Exit();

        private void clean_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            code.Document.SetText(Windows.UI.Text.TextSetOptions.None, string.Empty);
            ViewModel?.Clean();
        }

        private void code_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Tab)
            {
                RichEditBox richEditBox = sender as RichEditBox;
                if (richEditBox != null)
                {
                    richEditBox.Document.Selection.TypeText("\t");
                    e.Handled = true;
                }
            }
        }
    }
}
