using Hromiak_WPF_project.Tools;
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
    /// <summary>
    /// Interaction logic for AllUsersView.xaml
    /// </summary>
    public partial class AllUsersView : Window
    {
        public AllUsersView()
        {
            InitializeComponent();
            DataContext = new AllUsersViewModel();
        }
    }
}
