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
        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate = DateTime.Today;
        private Person _person;
        private bool _isCalculating;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); }
        }

        // Readonly Person (створюється після натискання кнопки)
        public Person Person
        {
            get => _person;
            private set { _person = value; OnPropertyChanged(nameof(Person)); }
        }

        // Змінна для блокування UI під час обчислень
        public bool IsCalculating
        {
            get => _isCalculating;
            set { _isCalculating = value; OnPropertyChanged(nameof(IsCalculating)); }
        }

        public ICommand CalculateCommand { get; }

        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(async () => await CalculateAsync(), () => !IsCalculating);
        }

        private async Task CalculateAsync()
        {
            IsCalculating = true;
            try
            {

                Person = new Person(FirstName, LastName, Email, BirthDate);

                bool validationPassed = await Task.Run(() =>
                {
                    // Перевірка: чи заповнені всі поля?
                    if (string.IsNullOrWhiteSpace(Person.FirstName) ||
                        string.IsNullOrWhiteSpace(Person.LastName) ||
                        string.IsNullOrWhiteSpace(Person.Email) ||
                        Person.BirthDate == DateTime.Today)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                            MessageBox.Show("Будь ласка, заповніть всі поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning)
                        );
                        return false;
                    }

                    // Перевірка віку
                    int age = Person.CalculateAge();
                    if (age < 0 || age > 135)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                            MessageBox.Show("Вік користувача некоректний. Перевірте дату народження.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error)
                        );
                        return false;
                    }

                    return true;
                });

                if (validationPassed)
                {
                    // Якщо перевірки пройшли, відкриваємо сторінку з результатами
                    var resultsPage = new ResultsView(Person);
                    resultsPage.Show();

                    Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
                    // Закриваємо головне вікно
                    Application.Current.MainWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsCalculating = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();
    }

}
