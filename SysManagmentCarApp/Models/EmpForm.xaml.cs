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
using System.Threading;
using System.Data.Entity;
using System.Data;

namespace SysManagmentCarApp.Models
{
    /// <summary>
    /// Логика взаимодействия для EmpForm.xaml
    /// </summary>
    public partial class EmpForm : Window
    {
        
        public EmpForm()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource employessViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("employessViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // employessViewSource.Source = [универсальный источник данных]

            await Task.Run(() => LoadingInfo());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewEmp newEmp = new NewEmp();
            newEmp.Show();
        }

        protected async void LoadingInfo()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var q = await (from m in db.Employess
                        select m).ToListAsync();

                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   dg.ItemsSource = q;
               });
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            int ch = ChangeRecord();
            if (ch != 0)
            {
                NewEmp emp = new NewEmp();
                emp.Owner = this;
                emp.ShowDialog();
            }
            else
            {
                MessageBox.Show("Не выбрана запись!");
                return;
            }
        }

        // вызов изменения
        public int ChangeRecord()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {                
                var sel = dg.SelectedItem as Employess;
                if (sel == null)
                    return 0;
                var seekSel = db.NewEmployee.Where(r => r.idEmp == sel.Id).FirstOrDefault();
                int id = seekSel.id;
                return id;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Delete();
                LoadingInfo();
                MessageBox.Show("Запись удалена!");
            }
            else
            {
                return;
            }            
        }

        private void Delete()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var s = dg.SelectedItem as Employess;
                var emS = db.Employess.Where(r => r.Id == s.Id).FirstOrDefault();
                var newEmpl = db.NewEmployee.Where(t => t.id == emS.Id).FirstOrDefault();
                if(emS != null && newEmpl != null)
                {
                    db.Entry(newEmpl).State = EntityState.Deleted;
                    db.Entry(emS).State = EntityState.Deleted;
                    db.SaveChanges();
                }

            }
        }

        private void RefrasheBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadingInfo();
        }
    }
}
