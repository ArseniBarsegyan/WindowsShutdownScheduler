using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using WindowsShutdownScheduler.Commands;
using WindowsShutdownScheduler.Helpers;

namespace WindowsShutdownScheduler.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("user32")]
        public static extern void LockWorkStation();

        private DispatcherTimer _timer;
        private TimeSpan _timeValue;
        private string _time;
        private bool _isShutdownApproved;

        private SubmitCommand _setTimerCommand;
        private SubmitCommand _showFullSizeWindowCommand;
        private SubmitCommand _quitProgramCommand;

        public ScheduleViewModel()
        {
            _timeValue = TimeSpan.FromHours(8);
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (sender, args) =>
            {
                Time = _timeValue.ToString("c");
                if (_timeValue == TimeSpan.Zero)
                {
                    _timer.Stop();
                    if (_isShutdownApproved)
                    {
                        ShutDownComputer();
                    }
                }
                _timeValue = _timeValue.Add(TimeSpan.FromSeconds(-1));
            };
            _timer.Start();
        }

        public string Time
        {
            get => ConstantsHelper.WindowsShutdownIn + _time;
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public int Minutes
        {
            get => _timeValue.Minutes;
            set
            {
                _timeValue = TimeSpan.FromMinutes(value);
                OnPropertyChanged(nameof(Minutes));
            }
        }

        public SubmitCommand SetTimerCommand
        {
            get
            {
                return _setTimerCommand ??
                       (_setTimerCommand = new SubmitCommand(obj =>
                       {
                           if (obj is string text)
                           {
                               int.TryParse(text, out var minutes);
                               _timeValue = TimeSpan.FromMinutes(minutes);

                               MessageBoxResult answer = MessageBox.Show($"Windows shutdown scheduled in {Minutes}?",
                                   ConstantsHelper.Warning, MessageBoxButton.YesNo, MessageBoxImage.Warning);

                               if (answer == MessageBoxResult.Yes)
                               {
                                   _isShutdownApproved = true;
                               }
                           }
                       }));
            }
        }

        public SubmitCommand ShowFullSizeWindowCommand
        {
            get { return _showFullSizeWindowCommand ?? (_showFullSizeWindowCommand = new SubmitCommand(obj =>
            {
                if (Application.Current.MainWindow is Window window)
                {
                    window.Show();
                    window.WindowState = WindowState.Normal;
                }
            }));}
        }

        public SubmitCommand QuitProgramCommand
        {
            get
            {
                return _quitProgramCommand ??
                       (_quitProgramCommand = new SubmitCommand(obj =>
                       {
                           Application.Current.Shutdown();
                       }));
            }
        }

        private void ShutDownComputer()
        {
            var psi = new ProcessStartInfo("shutdown", "/s /f /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);
            // LockWorkStation();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}