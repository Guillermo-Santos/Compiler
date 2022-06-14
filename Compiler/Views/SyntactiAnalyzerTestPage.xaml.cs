using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Compiler.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Compiler.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SyntactiAnalyzerTestPage : Page
    {
        SyntaxAnalyzerTestViewModel ViewModel = new SyntaxAnalyzerTestViewModel();
        public SyntactiAnalyzerTestPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }
        private void analyze_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            code.Document.GetText(Windows.UI.Text.TextGetOptions.None, out string Text);
            ViewModel?.Analyze(Text);
        }

        private void exit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

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
