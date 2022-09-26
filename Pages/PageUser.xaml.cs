﻿using System;
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
    /// Логика взаимодействия для PageUser.xaml
    /// </summary>
    public partial class PageUser : Page
    {
        public PageUser()
        {
            InitializeComponent();
           DGridUsers.ItemsSource = dbISP19AEntities.GetContext().User.ToList();

        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageAddEdit((sender as Button).DataContext as User));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageAddEdit(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
          //  динамическое отображение данных
            if (Visibility == Visibility.Visible)
            {
                dbISP19AEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridUsers.ItemsSource = dbISP19AEntities.GetContext().User.ToList();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
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
    }
}

