using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WindowsShutdownScheduler.Commands;

namespace WindowsShutdownScheduler.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        private int _minutes;

        public int Minutes
        {
            get => _minutes;
            set
            {
                _minutes = value; 
                OnPropertyChanged(nameof(Minutes));
            }
        }

        private SubmitCommand _submitcommand;

        public SubmitCommand SubmitCommand
        {
            get
            {
                return _submitcommand ??
                       (_submitcommand = new SubmitCommand(obj =>
                       {
                           MessageBox.Show(Minutes.ToString());
                       }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}