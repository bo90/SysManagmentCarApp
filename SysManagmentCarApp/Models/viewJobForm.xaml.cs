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
using System.Data;
using System.Data.Entity;
using System.Threading;

namespace SysManagmentCarApp.Models
{
    /// <summary>
    /// Логика взаимодействия для viewJobForm.xaml
    /// </summary>
    public partial class viewJobForm : Window
    {
        public viewJobForm()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource viewJobViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("viewJobViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // viewJobViewSource.Source = [универсальный источник данных]
            await Task.Run(() => Load());
        }

        protected async void Load()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var quer = await (from m in db.ViewJob
                                  select m).ToListAsync();
                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   DataGrid.ItemsSource = quer;
               });
            }
        }

        private void Add()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var sel = DataGrid.SelectedItem as ViewJob;
                if (sel != null)
                {
                    var result = db.ViewJob.Where(r => r.id == sel.id).FirstOrDefault();
                    if (result == null)
                    {
                        db.ViewJob.Add(sel);
                    }
                    else
                    {
                        result.id = sel.id;
                        db.Entry(result).State = EntityState.Added;
                    }
                    db.SaveChanges();
                    MessageBox.Show("Запись добавлена");
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать строку в таблице!");
                    return;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             Add();
            
            Load();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Change();
            
        }

        private void Change()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var sel = DataGrid.SelectedItem as ViewJob;
                if (sel != null)
                {
                    var result = db.ViewJob.Where(r => r.id == sel.id).FirstOrDefault();
                    if (result != null)
                    {
                        result.id = sel.id;
                        db.Entry(result).State = EntityState.Modified;
                        db.SaveChanges();
                        MessageBox.Show("Запись измнена!");
                    }
                    else
                    {
                        MessageBox.Show("Нельзя изменть шфир оборудования!");
                        Load();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Необходимо выбрать строку в таблице!");
                    return;
                }
            }            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Delete();
            }
            else
            {
                return;
            }
        }

        private void Delete()
        {
            try
            {
                using (GarageDBEntities db = new GarageDBEntities())
                {
                    var sel = DataGrid.SelectedItem as ViewJob;
                    if (sel != null)
                    {
                        var result = db.ViewJob.Where(r => r.id == sel.id).FirstOrDefault();
                        if (result != null)
                        {
                            result.id = sel.id;
                            db.Entry(result).State = EntityState.Deleted;
                            db.SaveChanges();
                            Load();
                            MessageBox.Show("Запись удалена");
                        }
                        else
                        {
                            MessageBox.Show("Нельзя удалять пустую строку!");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Необходимо выбрать строку для удаления!");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
