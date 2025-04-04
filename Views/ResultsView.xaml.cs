using Hromiak_WPF_project.Models;
using Hromiak_WPF_project.ViewModels;
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
    public partial class ResultsView : Window
    {
        public ResultsView(Person user)
        {
            InitializeComponent();
            DataContext = new ResultsViewModel(user);
        }
    }
}
