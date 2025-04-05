using Hromiak_WPF_project.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hromiak_WPF_project.Models
{
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime BirthDate { get; }

        public bool IsAdult { get; private set; }
        public string SunSign { get; private set; }
        public string ChineseSign { get; private set; }
        public bool IsBirthday { get; private set; }

        public Person(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;


            ValidateBirthDate();
            ValidateEmail();
        }

        public Person(string firstName, string lastName, string email)
            : this(firstName, lastName, email, DateTime.Today) { }

        public Person(string firstName, string lastName, DateTime birthDate)
            : this(firstName, lastName, string.Empty, birthDate) { }

        private void ValidateBirthDate()
        {
            if (BirthDate > DateTime.Now)
            {
                throw new FutureBirthDateException();
            }

            if ((DateTime.Now.Year - BirthDate.Year) > 135)
            {
                throw new ExcessivelyOldBirthDateException();
            }
        }

        private void ValidateEmail()
        {
            if (string.IsNullOrEmpty(Email) || !Email.Contains("@") || !Email.Contains("."))
            {
                throw new InvalidEmailException();
            }
        }

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

        // Асинхронна ініціалізація обчислюваних полів
        public async Task InitAsync()
        {
            var ageTask = Task.Run(() => CalculateIsAdult());
            var sunSignTask = Task.Run(() => GetWesternZodiac());
            var chineseSignTask = Task.Run(() => GetChineseZodiac());
            var birthdayTask = Task.Run(() => CheckIfBirthday());

            await Task.WhenAll(ageTask, sunSignTask, chineseSignTask, birthdayTask);

            IsAdult = ageTask.Result;
            SunSign = sunSignTask.Result;
            ChineseSign = chineseSignTask.Result;
            IsBirthday = birthdayTask.Result;
        }

        private bool CalculateIsAdult()
        {
            int age = CalculateAge();
            return age >= 18;
        }

        private string GetWesternZodiac()
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

        private string GetChineseZodiac()
        {
            string[] signs = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
            return signs[BirthDate.Year % 12];
        }

        private bool CheckIfBirthday()
        {
            var today = DateTime.Today;
            return BirthDate.Month == today.Month && BirthDate.Day == today.Day;
        }
    }
}
