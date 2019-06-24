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
    /// Логика взаимодействия для historyOrderForm.xaml
    /// </summary>
    public partial class historyOrderForm : Window
    {
        public historyOrderForm()
        {
            InitializeComponent();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            AcceptChange();
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // загрузка истории
        private async void GetData()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var q = await (from m in db.History
                               where m.Result == false
                               select m).ToListAsync();

                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   dataGistoryOrder.ItemsSource = q;
               });

            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => GetData());
        }

        private async void OpenOrder()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                //var getOrder = dataGistoryOrder.SelectedItem as History;
                //var seekOrder 
            }
        }

        private void AcceptChange()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var getOrder = dataGistoryOrder.SelectedItem as History;
                if (getOrder != null)
                {
                    var seekOrder = db.History.Where(r => r.id == getOrder.id).FirstOrDefault();                
                    seekOrder.Result = true;
                    db.Entry(seekOrder).State = EntityState.Modified;
                    db.SaveChanges();
                    GetData();
                }
                else
                {
                    MessageBox.Show("Выберите строку с заказ-нарядом!");
                    return;
                }
            }
        }

        private async void AllOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => AllOrder());
            closeOrderBtn.IsEnabled = false;
        }

        private async void AllOrder()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var result = await (from m in db.History
                                    select m).ToListAsync();
                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   dataGistoryOrder.ItemsSource = result;
               });
            }
        }

        private void AllClosedOrders_Click(object sender, RoutedEventArgs e)
        {
            AllClosingOrder();
            closeOrderBtn.IsEnabled = false;
        }

        private async void AllClosingOrder()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var result = await (from m in db.History
                                    where m.Result == true
                                    select m).ToListAsync();
                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    dataGistoryOrder.ItemsSource = result;
                });
            }
        }

        private void ActivOrders_Click(object sender, RoutedEventArgs e)
        {
            GetData();
            closeOrderBtn.IsEnabled = true;
        }
    }
}
