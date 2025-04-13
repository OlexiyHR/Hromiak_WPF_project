using Hromiak_WPF_project.Exceptions;
using Hromiak_WPF_project.Models;
using Hromiak_WPF_project.Services;
using Hromiak_WPF_project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hromiak_WPF_project.ViewModels
{
    public class AllUsersViewModel : INotifyPropertyChanged
    {
        // Повний список користувачів
        private ObservableCollection<Person> _users;
        public ObservableCollection<Person> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
                ApplyFilteringAndSorting();
            }
        }

        // Колекція, відображувана в DataGrid (після фільтрації і сортування)
        private ObservableCollection<Person> _displayedUsers;
        public ObservableCollection<Person> DisplayedUsers
        {
            get => _displayedUsers;
            set { _displayedUsers = value; OnPropertyChanged(); }
        }

        // Властивості для фільтрування та сортування
        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set { _filterText = value; OnPropertyChanged(); ApplyFilteringAndSorting(); }
        }

        private string _sortProperty;
        public string SortProperty
        {
            get => _sortProperty;
            set { _sortProperty = value; OnPropertyChanged(); ApplyFilteringAndSorting(); }
        }

        private Person _selectedUser;
        public Person SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                ((AllUsersCommand)EditUserCommand).RaiseCanExecuteChanged();
                ((AllUsersCommand)DeleteUserCommand).RaiseCanExecuteChanged();
            }
        }

        // Команди для CRUD операцій
        public ICommand AddUserCommand { get; set; }
        public ICommand EditUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public AllUsersViewModel()
        {
            AddUserCommand = new AllUsersCommand((object param) => AddUser());
            EditUserCommand = new AllUsersCommand((object param) => EditUser(param as Person), (object param) => param is Person);
            DeleteUserCommand = new AllUsersCommand((object param) => DeleteUser(param as Person), (object param) => param is Person);
            SaveCommand = new AllUsersCommand((object param) => SaveUsers());

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            Users = await DataService.LoadUsersAsync();
            DisplayedUsers = new ObservableCollection<Person>(Users);
        }

        private void ApplyFilteringAndSorting()
        {
            var query = Users.AsQueryable();

            // Фільтрація – шукаємо збіги за всіма 8-ма властивостями.
            // Для BirthDate, IsAdult та IsBirthday виконуємо ToString(), щоб зробити порівняння.
            if (!string.IsNullOrWhiteSpace(FilterText))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    u.BirthDate.ToString("d").Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    u.IsAdult.ToString().Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    u.SunSign.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    u.ChineseSign.Contains(FilterText, StringComparison.OrdinalIgnoreCase) ||
                    u.IsBirthday.ToString().Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Сортування – додаємо варіанти для всіх 8-ма властивостей
            if (!string.IsNullOrWhiteSpace(SortProperty))
            {
                switch (SortProperty)
                {
                    case "FirstName":
                        query = query.OrderBy(u => u.FirstName);
                        break;
                    case "LastName":
                        query = query.OrderBy(u => u.LastName);
                        break;
                    case "Email":
                        query = query.OrderBy(u => u.Email);
                        break;
                    case "BirthDate":
                        query = query.OrderBy(u => u.BirthDate);
                        break;
                    case "IsAdult":
                        query = query.OrderBy(u => u.IsAdult);
                        break;
                    case "SunSign":
                        query = query.OrderBy(u => u.SunSign);
                        break;
                    case "ChineseSign":
                        query = query.OrderBy(u => u.ChineseSign);
                        break;
                    case "IsBirthday":
                        query = query.OrderBy(u => u.IsBirthday);
                        break;
                    default:
                        break;
                }
            }

            DisplayedUsers = new ObservableCollection<Person>(query.ToList());
        }

        private async void AddUser()
        {
            // Використовуємо EditUserDialog для додавання нового користувача
            var addDialog = new EditUserView("", "", "", DateTime.Today.AddYears(-25));
            if (addDialog.ShowDialog() == true)
            {
                try
                {
                    int newId = Users.Any() ? Users.Count + 1 : 1;

                    var newUser = new Person(
                        firstName: addDialog.FirstName,
                        lastName: addDialog.LastName,
                        email: addDialog.Email,
                        birthDate: addDialog.BirthDate
                    );

                    await newUser.InitAsync();

                    Users.Add(newUser);

                    ApplyFilteringAndSorting();
                }
                catch (FutureBirthDateException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка дати народження", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (ExcessivelyOldBirthDateException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка дати народження", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidEmailException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка email", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void EditUser(Person user)
        {
            if (user == null)
                return;

            // Передаємо поточні дані користувача в діалог редагування
            var editDialog = new EditUserView(user.FirstName, user.LastName, user.Email, user.BirthDate);

            if (editDialog.ShowDialog() == true)
            {

                try
                {
                    // Створюємо новий об’єкт Person із зміненими даними, які отримали з діалогу.
                    var editedUser = new Person(
                    firstName: editDialog.FirstName,
                    lastName: editDialog.LastName,
                    email: editDialog.Email,
                    birthDate: editDialog.BirthDate
                    );
                    await editedUser.InitAsync();

                    // Заміняємо старий об'єкт на новий у колекції
                    int index = Users.IndexOf(user);
                    if (index >= 0)
                    {
                        Users[index] = editedUser;
                        ApplyFilteringAndSorting();
                    }
                }
                catch (FutureBirthDateException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка дати народження", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (ExcessivelyOldBirthDateException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка дати народження", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidEmailException ex)
                {
                    MessageBox.Show(ex.Message, "Помилка email", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteUser(Person user)
        {
            if (user == null)
                return;
            Users.Remove(user);
            ApplyFilteringAndSorting();
        }

        private void SaveUsers()
        {
            DataService.SaveUsers(Users);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class AllUsersCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public AllUsersCommand(Action<object> execute, Func<object, bool> canExecute = null)
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
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
