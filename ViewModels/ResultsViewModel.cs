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
        public User User { get; } 
        public string Greeting 
        { 
            get 
            { 
                bool isBirthday = (User.BirthDate.Month == DateTime.Today.Month && User.BirthDate.Day == DateTime.Today.Day); 
                return isBirthday ? $"Привіт, {User.Username}! З Днем Народження!" : $"Привіт, {User.Username}!"; 
            } 
        } 
        public string UserInfo => $"Ваш вік: {User.Age}\n" + $"Західний знак: {User.WesternZodiac}\n" + $"Китайський знак: {User.ChineseZodiac}"; 
        public ResultsViewModel(User user) 
        { 
            User = user; 
        } 
    } 
}
