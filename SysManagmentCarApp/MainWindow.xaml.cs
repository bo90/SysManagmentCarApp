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
using System.Data.Entity;
using System.Data.Sql;
using System.Threading;

namespace SysManagmentCarApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int _ID;

        //GarageDBEntities db;
        public MainWindow()
        {
            InitializeComponent();

            //db = new GarageDBEntities();
            
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource carsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("carsViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // carsViewSource.Source = [универсальный источник данных]
            await Task.Run(()=> Load());
        }

        private async void Load()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var q = await (from m in db.Cars
                        select m).ToListAsync();
                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   dGrid.ItemsSource = q;
               });
            }
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Открытие формирования новой записи
            Models.RecordCar recCar = new Models.RecordCar();
            recCar.Show();
        }

        private void openRecord_Click(object sender, RoutedEventArgs e)
        {
            SearchRecord();
        }

        protected void SearchRecord()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var quer = dGrid.SelectedItem as Cars;
                var search = db.Cars.Where(r => r.id == quer.id).FirstOrDefault();
                _ID = search.id;

                Models.RecordCar rec = new Models.RecordCar();
                rec.Owner = this;
                rec.Show();
            }
        }

        private void createOrder_Click(object sender, RoutedEventArgs e)
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var quer = dGrid.SelectedItem as Cars;
                var search = db.Cars.Where(r => r.id == quer.id).FirstOrDefault();
                _ID = search.id;

                Models.Order order = new Models.Order();
                order.Owner = this;
                order.ShowDialog();
            }
        }

        private void deleteRecord_Click(object sender, RoutedEventArgs e)
        {
            // удаление записи из базы
            Delete();
            Load();
            
        }

        private void Delete()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                try
                {
                    var quer = dGrid.SelectedItem as Cars;
                    var searchQuer = db.Cars.Where(r => r.id == quer.id).FirstOrDefault();
                    var searchFeatures = db.FeaturesCars.Where(r => r.VinNumber == quer.VinNumber).FirstOrDefault();
                    var seekClient = db.Clientes.Where(r => r.VinNumber == quer.VinNumber).FirstOrDefault();
                    var seekOrder = db.Orders.Where(r => r.IdClient == seekClient.Id).FirstOrDefault();
                    //db.Entry(searchQuer).State = EntityState.Deleted;                    
                    //db.FeaturesCars.Remove(searchFeatures);

                    //db.Cars.Remove(searchQuer);
                    if (seekOrder != null)
                    {
                        db.Entry(seekOrder).State = EntityState.Deleted;
                        db.SaveChanges();
                    }                    
                    db.Entry(seekClient).State = EntityState.Deleted;
                    db.SaveChanges();
                    db.Entry(searchFeatures).State = EntityState.Deleted;
                    db.SaveChanges();
                    db.Entry(searchQuer).State = EntityState.Deleted;
                    db.SaveChanges();
                    MessageBox.Show("Запись удалена!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Models.Order order = new Models.Order();
            order.Show();
        }

        private void TextBlock_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            Models.EmpForm empl = new Models.EmpForm();
            empl.Show();
        }

        private void TextBlock_MouseLeftButtonDown_3(object sender, MouseButtonEventArgs e)
        {
            Models.viewJobForm vj = new Models.viewJobForm();
            vj.Show();
        }

        private void TextBlock_MouseLeftButtonDown_4(object sender, MouseButtonEventArgs e)
        {
            Models.historyOrderForm hist = new Models.historyOrderForm();
            hist.Show();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => Load());
        }
    }
}
