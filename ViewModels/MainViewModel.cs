using Hromiak_WPF_project.Models;
using Hromiak_WPF_project.Views;
using Hromiak_WPF_project.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using Hromiak_WPF_project.Exceptions;

namespace Hromiak_WPF_project.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Navigation _navigationService;

        private string _firstName;
        private string _lastName;
        private string _email;
        private DateTime _birthDate;
        private Person _person;
        private bool _isCalculating;

        public string FirstName
        {
            get => _firstName;
            set 
            { 
                _firstName = value; 
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(CanProceed));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(CanProceed));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanProceed));
            }
        }

        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                OnPropertyChanged(nameof(CanProceed));
            }
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

        // Властивість, яка вказує, чи можна натиснути кнопку Proceed
        public bool CanProceed =>
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(Email) &&
            BirthDate != default;

        public ICommand ProceedCommand { get; }

        public MainViewModel(Navigation navigationService)
        {
            ProceedCommand = new RelayCommand(async () => await ProceedAsync(), () => CanProceed && !IsCalculating);
            _navigationService = navigationService;
        }

        private async Task ProceedAsync()
        {
            IsCalculating = true;
            try
            {

                var person = new Person(FirstName, LastName, Email, BirthDate);
                await person.InitAsync(); // обчислення async


                // Якщо валідації пройшли успішно, переходимо до наступних дій:
                _navigationService.NavigateToResults(person);
            }
            catch (FutureBirthDateException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ExcessivelyOldBirthDateException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidEmailException ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _executeAsync;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Func<Task> executeAsync, Func<bool> canExecute = null)
        {
            _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public async void Execute(object parameter) => await _executeAsync();
    }

}
