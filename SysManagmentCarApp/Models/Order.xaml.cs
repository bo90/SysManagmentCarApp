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
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        public Clientes clientes { get; set; }
        public Orders order { get; set; }
        public Employess emp { get; set; }
        public Cars cars { get; set; }
        public History history { get; set; }

        public int IDORder;
        public string VinNumber;

        public Order()
        {
            order = new Orders();
            emp = new Employess();
            clientes = new Clientes();
            cars = new Cars();
            history = new History();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateOrder();
                MessageBox.Show("Заказ-наряд сформирован!");
                printBtn.Visibility = Visibility.Visible;
                createBtn.Visibility = Visibility.Hidden;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Data.ToString());
            }
        }

        private void NewOrderCreate()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                order.VinNumber = vintxtbox.Text;
                var seekVin = db.Cars.Where(r => r.VinNumber == order.VinNumber).FirstOrDefault();
                if (seekVin == null)
                {
                    MessageBox.Show("VIN-номер отсутствует в базе! Необходимо создать запись");
                    return;
                }
            }
        }

        protected async void OutVinNumber()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var q = await (from m in db.Cars
                               select m.VinNumber).ToListAsync();
                await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    vintxtbox.ItemsSource = q;
                });
            }
        }

        private void Vintxtbox_TextInput(object sender, TextCompositionEventArgs e)
        {
            
        }

        private async void SeekRec(string vin)
        {
            
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var q = await (from m in db.Cars
                               from d in db.FeaturesCars
                               from c in db.Clientes
                               from r in db.ViewJob
                               where m.VinNumber == vin &&
                               d.VinNumber == m.VinNumber &&
                               c.VinNumber == d.VinNumber
                               select new
                               { m.VinNumber,
                                   m.GosNumber,
                                   d.PTC_number,
                                   c.sName,
                                   c.fName,
                                   r.viewJob1
                               }).ToListAsync();
               await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
               foreach (var i in q)
               {
                       vintxtbox.Text = null;
                       gosNumTxtBox.Text = null;
                       ptcNumTxtBox.Text = null;
                       secondNameTxtBox.Text = null;
                       fNameTxtBox.Text = null;
                       viewJobTxtbox.Text = null;
                       //vintxtbox.Text += string.Format("{0}\r\n", i.VinNumber);
                       vintxtbox.Text = i.VinNumber;
                       gosNumTxtBox.Text += string.Format("{0}\r\n", i.GosNumber);
                       ptcNumTxtBox.Text += string.Format("{0}\r\n", i.PTC_number);
                       secondNameTxtBox.Text += string.Format("{0}\r\n", i.sName);
                       fNameTxtBox.Text += string.Format("{0}\r\n", i.fName);
                       //viewJobTxtbox.Text += string.Format("{0}\r\n", i.viewJob1);
                       viewJobTxtbox.Text = i.viewJob1.ToString();
                       //viewJobTxtbox.SelectedIndex = 0;
                       //vintxtbox.SelectedIndex = 0;
                       //gosNumTxtBox.SelectedIndex = 0;
                       
                   }
               });
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            System.Windows.Data.CollectionViewSource carsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("carsViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // carsViewSource.Source = [универсальный источник данных]
            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // clientesViewSource.Source = [универсальный источник данных]
            System.Windows.Data.CollectionViewSource employessViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("employessViewSource")));
            // Загрузите данные, установив свойство CollectionViewSource.Source:
            // employessViewSource.Source = [универсальный источник данных]
            MainWindow mw = this.Owner as MainWindow;
            if (mw != null)
            {                
                int getID = mw._ID;
                await Task.Run(() => LoadVinToID(getID));
                await Task.Run(() => LoadGosNumToID(getID));
                await Task.Run(() => LoadPeopleOnID(getID));                
            }
            else
            {
                await Task.Run(() => LoadVin());
                await Task.Run(() => LoadGosNum());
                await Task.Run(() => LoadPeople());
            }
        }

        protected async void LoadPeople()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var people = await (from m in db.Employess
                                    select m.sNameEnp).ToListAsync();
                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {                   
                   sNameTxtBox.ItemsSource = people;                      
               });
            }
        }

        protected async void LoadPeopleOnID(int id)
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var people = await (from m in db.Employess
                                   from p in db.History
                                   where p.id == id &&
                                   m.Id == p.idEmployees
                                    select m.sNameEnp).ToListAsync();
                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    sNameTxtBox.ItemsSource = people;
                    sNameTxtBox.SelectedIndex = 0;
                });
            }
        }

        // загрузка всех VIN ноеров в combobox VIN-номер
        protected async void LoadVin()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var lVin = await (from m in db.Cars
                                  select m.VinNumber).ToListAsync();
                await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   vintxtbox.ItemsSource = lVin;
               });
            }
        }

        protected async void LoadVinToID(int id)
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var lVin = await (from m in db.Cars
                                  where m.id == id
                                  select m.VinNumber).ToListAsync();
                await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    vintxtbox.ItemsSource = lVin;
                    vintxtbox.SelectedIndex = 0;
                });
            }
        }

        // Загрузка всех гос номеров в Gos NUm
        protected async void LoadGosNum()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var lGos = await (from m in db.Cars
                                  select m.GosNumber).ToListAsync();
                await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   gosNumTxtBox.ItemsSource = lGos;

               });
            }
        }

        protected async void LoadGosNumToID(int id)
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var lGos = await (from m in db.Cars
                                  where m.id == id
                                  select m.GosNumber).ToListAsync();
                await Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    gosNumTxtBox.ItemsSource = lGos;
                    gosNumTxtBox.SelectedIndex = 0;
                });
            }
        }

        private void Vintxtbox_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            
        }

        // инициализация поиска и вывода информации при выборе конкретного Vin-номера
        private async void Vintxtbox_DropDownClosed(object sender, EventArgs e)
        {           
            string vin = vintxtbox.Text;
            await Task.Run(() => SeekRec(vin));
        }

        private async void GosNumTxtBox_DropDownClosed(object sender, EventArgs e)
        {           
            string gos = gosNumTxtBox.Text;
            //vintxtbox.SelectedItem = null; 
            if (gos != null && gos != "")
            {
                await Task.Run(() => SeekGos(gos));
            }
            else
            {
                return;
            }
        }

        // поиск и вывод информации по гос номеру автомобиля
        private async void SeekGos(string gos)
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var vin = db.Cars.Where(r => r.GosNumber == gos).FirstOrDefault();
                var quer = await (from m in db.Cars
                                  from t in db.FeaturesCars
                                  from c in db.Clientes
                                  where m.VinNumber == vin.VinNumber &&
                                  t.VinNumber == m.VinNumber &&
                                  c.VinNumber == t.VinNumber
                                  select new
                                  {
                                      m.VinNumber,
                                      m.GosNumber,
                                      t.PTC_number,
                                      c.sName,
                                      c.fName
                                  }).ToListAsync();

                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    foreach(var q in quer)
                    {                        
                        gosNumTxtBox.Text = null;
                        ptcNumTxtBox.Text = null;
                        secondNameTxtBox.Text = null;
                        fNameTxtBox.Text = null;
                        //vintxtbox.Text += string.Format("{0}\r\n", q.VinNumber);                        
                        vintxtbox.Text = q.VinNumber;
                        gosNumTxtBox.Text += string.Format("{0}\r\n", q.GosNumber);
                        ptcNumTxtBox.Text += string.Format("{0}\r\n", q.PTC_number);
                        secondNameTxtBox.Text += string.Format("{0}\r\n", q.sName);
                        fNameTxtBox.Text += string.Format("{0}\r\n", q.fName);
                        //vintxtbox.SelectedIndex = 0;
                       // gosNumTxtBox.SelectedIndex = 0;
                    }
                });
            }
        }

        private async void SNameTxtBox_DropDownClosed(object sender, EventArgs e)
        {
            string sName = sNameTxtBox.Text;
            await Task.Run(() => LoadingName(sName));
        }

        private async void LoadingName(string sName)
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var names = await (from m in db.Employess
                                   where m.sNameEnp == sName
                                   select m).ToListAsync();
                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   foreach(var n in names)
                   {
                       nameTxtBox.Clear();
                       nameTxtBox.Text += string.Format("{0}\r\n", n.NameEmp);
                   }
               });
            }
        }


        // создание нового заказ-наряда
        private void CreateOrder()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                int getId = 0;
                var vN = vintxtbox.Text;
                order.VinNumber = vN.ToString();
                var countMax = from m in db.Orders
                               select m.id;  
                foreach(var c in countMax)
                 getId = countMax.Max();
                if (getId != 0)
                {
                    var getNum = db.Orders.Where(r => r.id == getId).FirstOrDefault();
                    if (getNum != null)
                    {
                        do
                        {
                            getId++;
                            order.id = getId;
                        } while (getNum.id == getId);
                    }                   
                }
                else
                {
                    getId = 1;
                }
                order.IdOrder = Convert.ToInt32(idOrderTxtBox.Text);
                var vinN = db.Clientes.Where(r => r.VinNumber == vN.ToString()).FirstOrDefault();
                if (vinN != null)
                {
                    order.IdClient = vinN.Id;
                }                

                order.Descript = DescriptJobTxtBox.Text;
                order.DateBegin = Convert.ToDateTime(beginDT.Text);
                order.DateEnd = Convert.ToDateTime(EndDT.Text);
                TimeSpan timeBegin = TimeSpan.Parse(TimeStart.Text);
                order.timeStart = timeBegin;
                TimeSpan timeEnd = TimeSpan.Parse(TimeEnd.Text);
                order.TimeEnd = timeEnd;                
                var emp = db.Employess.Where(r => r.sNameEnp == sNameTxtBox.Text).FirstOrDefault();
                if (emp != null)
                    order.IdEmp = emp.Id;
                //db.Orders.Add(order);
                //db.Entry(order).State = EntityState.Added;
                //db.SaveChanges();

                db.Orders.Add(order);
                db.SaveChanges();

                // добавление записей в историю 
                history.id = getId; //vinN.Id;
                history.idORDER = order.id;//Convert.ToInt32(idOrderTxtBox.Text);
                history.idVin = vN.ToString();
                history.idEmployees = emp.Id;
                var featur = db.FeaturesCars.Where(r => r.VinNumber == order.VinNumber).FirstOrDefault();
                if (featur != null)
                    history.idFeatures = featur.PTC_number;
                
                history.DateHistory = Convert.ToDateTime(beginDT.Text);
                string viewJob = viewJobTxtbox.Text.ToString();
                
                history.EventHistory = viewJob;
                history.Result = false;
                history.Descript = DescriptJobTxtBox.Text;
                db.History.Add(history);
                db.SaveChanges();
                IDORder = Convert.ToInt32(idOrderTxtBox.Text);
                VinNumber = vintxtbox.Text;
            }
        }

        private async void Vintxtbox_DropDownClosed_1(object sender, EventArgs e)
        {
            string vin = vintxtbox.Text;
            await Task.Run(() => SeekRec(vin));
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            Report rep = new Report();
            rep.Owner = this;
            rep.ShowDialog();
        }       
    }
}
