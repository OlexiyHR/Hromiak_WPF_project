using Hromiak_WPF_project.Models;
using Hromiak_WPF_project.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace Hromiak_WPF_project.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private User _user;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public ICommand CalculateCommand { get; }

        public MainViewModel()
        {
            User = new User();
            CalculateCommand = new RelayCommand(Calculate);
        }

        private void Calculate(object parameter)
        {
            if (string.IsNullOrWhiteSpace(User.Username) || User.BirthDate == default)
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (User.Age < 0 || User.Age > 135)
            {
                MessageBox.Show("Вік користувача некоректний. Перевірте дату народження.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var resultsPage = new ResultsView(User);
            resultsPage.Show();
            Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
            Application.Current.MainWindow.Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

}
