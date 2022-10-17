using ConnectDBSQLServer.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ConnectDBSQLServer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageListUser.xaml
    /// </summary>
    public partial class PageListUser : Page
    {
        public PageListUser()
        {
            InitializeComponent();
            var currentUser = dbISP19AEntities.GetContext().User.ToList();
            LViewUser.ItemsSource = currentUser;
        }
    }
}
