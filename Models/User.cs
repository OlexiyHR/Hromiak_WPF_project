using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Hromiak_WPF_project.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _username;
        private DateTime _birthDate;
        public string Username 
        { 
            get => _username; 
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
                OnPropertyChanged(nameof(Age));
                OnPropertyChanged(nameof(WesternZodiac));
                OnPropertyChanged(nameof(ChineseZodiac));
            }
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                int age = today.Year - BirthDate.Year;
                if (BirthDate > today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        public string WesternZodiac
        {
            get
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
        }

        public string ChineseZodiac
        {
            get
            {
                string[] signs = { "Мавпа", "Півень", "Собака", "Свиня", "Щур", "Бик", "Тигр", "Кролик", "Дракон", "Змія", "Кінь", "Коза" };
                return signs[BirthDate.Year % 12];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged; 
        protected virtual void OnPropertyChanged(string propertyName) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }
    }
}
