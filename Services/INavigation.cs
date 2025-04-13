using Hromiak_WPF_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hromiak_WPF_project.Tools
{
    public interface INavigation
    {
        void NavigateToResults(Person person);
        void NavigateToAllUsers();
    }
}
