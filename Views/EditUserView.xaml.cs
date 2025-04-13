using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hromiak_WPF_project.Views
{
    /// <summary>
    /// Interaction logic for EditUserView.xaml
    /// </summary>
    public partial class EditUserView : Window
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }

        public EditUserView(string firstName, string lastName, string email, DateTime birthDate)
        {
            InitializeComponent();

            // Заповнюємо поля початковими даними
            FirstNameTextBox.Text = firstName;
            LastNameTextBox.Text = lastName;
            EmailTextBox.Text = email;
            BirthDatePicker.SelectedDate = birthDate;

            FirstNameTextBox.TextChanged += CheckInput;
            LastNameTextBox.TextChanged += CheckInput;
            EmailTextBox.TextChanged += CheckInput;
            BirthDatePicker.SelectedDateChanged += CheckInput;

            CheckInput(null, null);
        }

        private void CheckInput(object sender, EventArgs e)
        {
            // Дозволяємо натискання OK лише якщо всі поля заповнені (і дата обрана)
            OkButton.IsEnabled = !string.IsNullOrWhiteSpace(FirstNameTextBox.Text)
                                 && !string.IsNullOrWhiteSpace(LastNameTextBox.Text)
                                 && !string.IsNullOrWhiteSpace(EmailTextBox.Text)
                                 && BirthDatePicker.SelectedDate.HasValue;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Зчитуємо значення із полів введення
            FirstName = FirstNameTextBox.Text;
            LastName = LastNameTextBox.Text;
            Email = EmailTextBox.Text;
            BirthDate = BirthDatePicker.SelectedDate.HasValue ? BirthDatePicker.SelectedDate.Value : DateTime.Today;

            DialogResult = true;
        }
    }
}
