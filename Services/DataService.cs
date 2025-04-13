using Hromiak_WPF_project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hromiak_WPF_project.Services
{
    public static class DataService
    {
        private const string DataFilePath = "users.json";

        public static async Task<ObservableCollection<Person>> LoadUsersAsync()
        {
            if (File.Exists(DataFilePath))
            {
                string json = File.ReadAllText(DataFilePath);
                var users = JsonSerializer.Deserialize<ObservableCollection<Person>>(json);
                if (users != null)
                    return users;
            }
            // Якщо файл відсутній – створюємо 50 користувачів:
            var newUsers = new ObservableCollection<Person>();
            Random random = new Random();
            for (int i = 1; i <= 50; i++)
            {
                // Генеруємо вік від 10 до 70 років
                int age = random.Next(10, 71);
                int year = DateTime.Today.Year - age;
                int month = random.Next(1, 13);
                int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);

                DateTime birthDate = new DateTime(year, month, day);

                var person = new Person(
                    firstName: $"FirstName{i}",
                    lastName: $"LastName{i}",
                    email: $"user{i}@example.com",
                    birthDate: birthDate
                );
                await person.InitAsync();
                newUsers.Add(person);
            }
            SaveUsers(newUsers);
            return newUsers;
        }

        public static void SaveUsers(ObservableCollection<Person> users)
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(DataFilePath, json);
        }
    }
}
