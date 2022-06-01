using Compiler.ViewModels;
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
        private void analyze_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => ViewModel?.Analyze();

        private void exit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => ViewModel?.Exit();

        private void clean_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) => ViewModel?.Clean();
    }
}
