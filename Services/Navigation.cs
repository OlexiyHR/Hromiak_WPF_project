using Hromiak_WPF_project.Models;
using Hromiak_WPF_project.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;

namespace Hromiak_WPF_project.Tools
{
    public class Navigation : INavigation
    {
        public void NavigateToResults(Person person)
        {
            var resultsPage = new ResultsView(person);
            resultsPage.Show();

           //Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
           // Application.Current.MainWindow.Close();
        }

        public void NavigateToAllUsers()
        {
            var allUsersWindow = new AllUsersView();
            allUsersWindow.Show();
        }
    }
}
