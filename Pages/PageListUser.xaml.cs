using ConnectDBSQLServer.Classes;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
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
using Excel = Microsoft.Office.Interop.Excel;


namespace ConnectDBSQLServer.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageListUser.xaml
    /// </summary>
    public partial class PageListUser : System.Windows.Controls.Page
    {
        
        public PageListUser()
        {
            InitializeComponent();
            var currentUser = dbISP19AEntities.GetContext().User.ToList();
            LViewUser.ItemsSource = currentUser;
            DataContext = LViewUser;
            CmbFiltr.Items.Add("Все пользователи");
            foreach (var item in dbISP19AEntities.GetContext().User.
                Select(x => x.Adress).Distinct().ToList())
                CmbFiltr.Items.Add(item);
        }
        /// <summary>
        /// переход на форму редактирования
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageAddEdit((sender as System.Windows.Controls.Button).DataContext as User));
        }
        /// <summary>
        /// Фильтрация по адресу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbFiltr.SelectedValue.ToString() == "Все пользователи")
            {
                LViewUser.ItemsSource = dbISP19AEntities.GetContext().User.ToList();
                //TxbCountSearchItem.Text = dbISP19AEntities.GetContext().User.Count().ToString();
            }
            else
            {
                LViewUser.ItemsSource = dbISP19AEntities.GetContext().User.
                    Where(x => x.Adress == CmbFiltr.SelectedValue.ToString()).ToList();
                //TxbCountSearchItem.Text = dbISP19AEntities.GetContext().User.
                 //       Where(x => x.LastName == CmbFiltr.SelectedValue.ToString()).Count().ToString();
            }
        }
        /// <summary>
        /// сортировка в обратном порядке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbDown_Checked(object sender, RoutedEventArgs e)
        {
            LViewUser.ItemsSource = dbISP19AEntities.GetContext().User.
                OrderByDescending(x => x.LastName).ToList();
        }
        /// <summary>
        /// сортировка по алфавиту
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RbUp_Checked(object sender, RoutedEventArgs e)
        {
            LViewUser.ItemsSource = dbISP19AEntities.GetContext().User.
                OrderBy(x => x.LastName).ToList();
        }
        /// <summary>
        /// Поиск по всем полям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //поиск
            string search = TxtSearch.Text;
            if (TxtSearch.Text != null)
            {
                LViewUser.ItemsSource = dbISP19AEntities.GetContext().User.
                    Where(x => x.LastName.Contains(search)
                    || x.FirstName.Contains(search)
                    || x.Adress.Contains(search)
                    || x.Phone.ToString().Contains(search)).ToList();
            }
        }
        /// <summary>
        /// Вывод на печать Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSaveToExcel_Click(object sender, RoutedEventArgs e)
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

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = LViewUser.Items;
            //цикл по данным из списка для печати
            foreach (User item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.LastName;
                worksheet.Cells[3][indexRows + 1] = item.FirstName;
                worksheet.Cells[4][indexRows + 1] = item.Adress;
                worksheet.Cells[5][indexRows + 1].Value = item.Phone.ToString();

                indexRows++;
            }
            Excel.Range range = worksheet.Range[worksheet.Cells[2][indexRows + 1],
                    worksheet.Cells[5][indexRows + 1]];
            range.ColumnWidth = 30; //ширина столбцов
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;//выравнивание по левому краю
            
            //показать Excel
            app.Visible = true;
        }

        private void BtnSaveToExcelTemplate_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open($"{Directory.GetCurrentDirectory()}\\Шаблон.xlsx");
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Cells[4, 2] = DateTime.Now.ToString();
            ws.Cells[4, 5] = 7;
            int indexRows = 6;
            //ячейка
            ws.Cells[1][indexRows] = "Номер";
            ws.Cells[2][indexRows] = "Фамилия";
            ws.Cells[3][indexRows] = "Имя";
            ws.Cells[4][indexRows] = "Адрес";
            ws.Cells[5][indexRows] = "Телефон";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = LViewUser.Items;
            //цикл по данным из списка для печати
            foreach (User item in printItems)
            {
                ws.Cells[1][indexRows + 1] = indexRows;
                ws.Cells[2][indexRows + 1] = item.LastName;
                ws.Cells[3][indexRows + 1] = item.FirstName;
                ws.Cells[4][indexRows + 1] = item.Adress;
                ws.Cells[5][indexRows + 1].Value = item.Phone.ToString();

                indexRows++;
            }
            ws.Cells[indexRows + 2, 3] = "Подпись";
            ws.Cells[indexRows + 2, 5] = "Кузьмина Е.Е.";
            excelApp.Visible = true;
        }
    }
}
