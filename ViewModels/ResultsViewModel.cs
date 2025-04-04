using Hromiak_WPF_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hromiak_WPF_project.ViewModels 
{ 
    public class ResultsViewModel 
    { 
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
    } 
}
