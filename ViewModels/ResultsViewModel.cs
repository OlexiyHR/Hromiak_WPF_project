using Hromiak_WPF_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hromiak_WPF_project.Tools;
using System.Windows.Navigation;
using System.Windows.Input;

namespace Hromiak_WPF_project.ViewModels 
{ 
    public class ResultsViewModel 
    {
        private readonly INavigation _navigationService;
        public Person _person { get; } 
        public string Greeting 
        { 
            get 
            { 
                return _person.IsBirthday ? $"Привіт, {_person.FirstName}! З Днем Народження!" : $"Привіт, {_person.FirstName}!"; 
            } 
        } 
        public string UserInfo => $"Ваше ім'я: {_person.FirstName}\n" + $"Ваше прізвище: {_person.LastName}\n" + $"Ваша адреса електронної пошти: {_person.Email}\n" + $"Ваша дата народження: {_person.BirthDate}\n" + $"Чи ви старше 18 років: {_person.IsAdult}\n" + $"Ваш західний знак: {_person.SunSign}\n" + $"Ваш китайський знак: {_person.ChineseSign}\n" + $"Чи сьогодні ваше день народження: {_person.IsBirthday}";
        public ResultsViewModel(Person user) 
        { 
            _person = user; 
        }

        private ICommand _showAllUsersCommand;
        public ICommand ShowAllUsersCommand
        {
            get
            {
                if (_showAllUsersCommand == null)
                    _showAllUsersCommand = new SimpleCommand(ExecuteShowAllUsers);
                return _showAllUsersCommand;
            }
        }

        private void ExecuteShowAllUsers()
        {
            // Виклик методу переходу через навігаційний сервіс
            _navigationService.NavigateToAllUsers();
        }

        public ResultsViewModel(Person person, INavigation navigationService)
        {
            _person = person;
            _navigationService = navigationService;
        }
    }

    public class SimpleCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public SimpleCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
