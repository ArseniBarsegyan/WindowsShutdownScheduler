using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WindowsShutdownScheduler
{
    /// <summary>
    /// Interaction logic for SchedulerWindow.xaml
    /// </summary>
    public partial class SchedulerWindow : Window
    {
        public SchedulerWindow()
        {
            InitializeComponent();
        }

        private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var text = e.Text;
            e.Handled = !IsTextAllowed(text);
        }

        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
