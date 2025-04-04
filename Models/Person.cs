using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hromiak_WPF_project.Models
{
    public class Person : INotifyPropertyChanged
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime BirthDate { get; }

        public readonly bool IsAdult;
        public readonly string SunSign;
        public readonly string ChineseSign;
        public readonly bool IsBirthday;

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;

            IsAdult = CalculateAge() >= 18;
            SunSign = GetWesternZodiac(birthDate);
            ChineseSign = GetChineseZodiac(birthDate);
            IsBirthday = birthDate.Month == DateTime.Today.Month && birthDate.Day == DateTime.Today.Day;
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.MinValue) { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate) { }

        public int CalculateAge()
        {
            var today = DateTime.Today;
            int age = today.Year - BirthDate.Year;
            if (BirthDate > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        private string GetWesternZodiac(DateTime birthDate)
        {
            int day = BirthDate.Day, month = BirthDate.Month;
            return month switch
            {
                1 => day <= 19 ? "Козеріг" : "Водолій",
                2 => day <= 18 ? "Водолій" : "Риби",
                3 => day <= 20 ? "Риби" : "Овен",
                4 => day <= 19 ? "Овен" : "Телець",
                5 => day <= 20 ? "Телець" : "Близнюки",
                6 => day <= 20 ? "Близнюки" : "Рак",
                7 => day <= 22 ? "Рак" : "Лев",
                8 => day <= 22 ? "Лев" : "Діва",
                9 => day <= 22 ? "Діва" : "Терези",
                10 => day <= 22 ? "Терези" : "Скорпіон",
                11 => day <= 21 ? "Скорпіон" : "Стрілець",
                12 => day <= 21 ? "Стрілець" : "Козеріг",
                _ => "Невідомий знак"
            };
        }

        private string GetChineseZodiac(DateTime birthDate)
        {
            string[] signs = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
            return signs[BirthDate.Year % 12];
        }

        public event PropertyChangedEventHandler PropertyChanged; 
        protected virtual void OnPropertyChanged(string propertyName) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }
    }
}
