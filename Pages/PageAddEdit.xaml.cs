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

        public PageAddEdit(User selectedUser) // в конструктор добавлен параметр типа User
        {
            InitializeComponent();

            CmbLogin.ItemsSource = dbISP19AEntities.GetContext().Account.ToList();
            CmbLogin.SelectedValuePath = "ID";
            CmbLogin.DisplayMemberPath = "Login";

            if (selectedUser != null)
                _currentUser = selectedUser;
            //создаем контекст
            DataContext = _currentUser;

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            

            StringBuilder error = new StringBuilder(); //объект для сообщения об ошибке

            //проверка полей объекта
            if (string.IsNullOrWhiteSpace(_currentUser.FirstName))
                error.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(_currentUser.LastName))
                error.AppendLine("Укажите фамилию");
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString());
                return;
            }
            //если пользователь новый
            if (_currentUser.ID == 0)
                dbISP19AEntities.GetContext().User.Add(_currentUser); //добавить в контекст
            try
            {
                dbISP19AEntities.GetContext().SaveChanges(); // сохранить изменения
               // dbISP19AEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                MessageBox.Show("Данные сохранены");
                ClassFrame.frmObj.Navigate(new PageUser());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
