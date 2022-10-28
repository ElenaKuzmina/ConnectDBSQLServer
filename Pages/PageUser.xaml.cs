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
using Excel = Microsoft.Office.Interop.Excel;

namespace ConnectDBSQLServer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageUser.xaml
    /// </summary>
    public partial class PageUser : Page
    {
       

        public PageUser()
        {
            InitializeComponent(); 
            // получение списка данных
            
           DGridUsers.ItemsSource = dbISP19AEntities.GetContext().User.ToList();

            CmbFiltrLogin.ItemsSource = dbISP19AEntities.GetContext().Account.ToList();
            CmbFiltrLogin.SelectedValuePath = "ID";
            CmbFiltrLogin.DisplayMemberPath = "Login";
       
           
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {//переход на редактирование
            ClassFrame.frmObj.Navigate(new PageAddEdit((sender as Button).DataContext as User));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {//переход на добавление
            ClassFrame.frmObj.Navigate(new PageAddEdit(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
          //  динамическое отображение добавленных или измененных данных
            if (Visibility == Visibility.Visible)
            {
               dbISP19AEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridUsers.ItemsSource = dbISP19AEntities.GetContext().User.ToList();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {// удаление нескольких пользователей
            var usersForRemoving = DGridUsers.SelectedItems.Cast<User>().ToList();
            if (MessageBox.Show($"Удалить {usersForRemoving.Count()} пользователей?", 
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                     
                try
                {
                    dbISP19AEntities.GetContext().User.RemoveRange(usersForRemoving);
                    dbISP19AEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены");
                    DGridUsers.ItemsSource = dbISP19AEntities.GetContext().User.ToList();
                }
               catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                    

        }

        private void CmbFiltrLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {//фильтр по логину
            int id = Convert.ToInt32(CmbFiltrLogin.SelectedValue);
            
            DGridUsers.ItemsSource = dbISP19AEntities.GetContext().User.Where(x=>x.Login_ID == id).ToList();
        }

        private void BtnResetAll_Click(object sender, RoutedEventArgs e)
        {//сброс фильтрации
            DGridUsers.ItemsSource = dbISP19AEntities.GetContext().User.ToList();
        }

        private void BtnToList_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageListUser());
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            
            
            //объект Excel
            var app = new Excel.Application();

            //книга 
            Excel.Workbook wb = app.Workbooks.Add();
            //лист
            Excel.Worksheet worksheet = app.Worksheets.Item[1];
            int indexRows = 1;
            //ячейка
            worksheet.Cells[1][indexRows] = "Номер";
            worksheet.Cells[2][indexRows] = "Фамилия";
            worksheet.Cells[3][indexRows] = "Имя";
            worksheet.Cells[4][indexRows] = "Адрес";
            worksheet.Cells[5][indexRows] = "Телефон";
            var printItems = dbISP19AEntities.GetContext().User.ToList();
            //цикл по данным из таблицы
            foreach (var item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.LastName;
                worksheet.Cells[3][indexRows + 1] = item.FirstName;
                worksheet.Cells[4][indexRows + 1] = item.Adress;
                worksheet.Cells[5][indexRows + 1] = item.Phone.ToString();
                indexRows++;
            }

            //показать Excel
            app.Visible = true;
            
        }
    }
}

