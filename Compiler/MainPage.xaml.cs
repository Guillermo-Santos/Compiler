using Windows.UI.Xaml.Controls;
using Compiler.Views;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Compiler
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void LexicTest_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Visual.Navigate(typeof(LexicAnalyzerTestPage));
        }

        private void SyncTest_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Visual.Navigate(typeof(SyntactiAnalyzerTestPage));
        }
    }
}
