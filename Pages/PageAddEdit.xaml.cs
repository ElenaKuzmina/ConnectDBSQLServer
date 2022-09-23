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
using ConnectDBSQLServer.Classes;

namespace ConnectDBSQLServer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAddEdit.xaml
    /// </summary>
    public partial class PageAddEdit : Page
    {
        //новое поле, которое будет хранить в себе экземпляр добавляемого пользователя

        private User _currentUser = new User();

        public PageAddEdit()
        {//создаем контекст
            DataContext = _currentUser;

            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.FirstName))
                error.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(_currentUser.LastName))
                error.AppendLine("Укажите фамилию");
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }
            dbISP19AEntities.GetContext().User.Add(_currentUser);
            try
            {
                dbISP19AEntities.GetContext().SaveChanges();
                MessageBox.Show("Новый пользователь добавлен");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
