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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.IO.Packaging;
using System.IO;
using System.Printing.Interop;
using System.Data.Entity;
using System.Threading;

namespace SysManagmentCarApp.Models
{
    /// <summary>
    /// Логика взаимодействия для RecordCar.xaml
    /// </summary>
    public partial class RecordCar : Window
    {
        public Cars carClass { get; set; }
        public FeaturesCars features { get; set; }
        public Clientes clientes { get; set; }

        public RecordCar()
        {
            carClass = new Cars();
            features = new FeaturesCars();
            clientes = new Clientes();
            InitializeComponent();            
        }
                
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewRecord();
            MessageBox.Show("Новая запись создана!");
        }

        private void NewRecord()
        {
            try
            {
                using (GarageDBEntities db = new GarageDBEntities())
                {
                    // добавление в таблицу VIN-номера
                    carClass.VinNumber = vinTxtbox.Text;
                    // Добавление гос номера
                    carClass.GosNumber = goxNumberTxtBox.Text;
                    // Добавление модели
                    carClass.Model = ModeltxtBox.Text;
                    // Добавление марки автомобиля
                    carClass.Mark = MarkTxtBox.Text;

                    db.Cars.Add(carClass);
                    db.SaveChanges();
                    int getId = 0;
                    var countMax = from m in db.FeaturesCars
                                   select m.Id;
                    foreach(var cont in countMax)
                      getId = countMax.Max();
                    if (getId != 0)
                    {
                        var getNum = db.FeaturesCars.Where(r => r.Id == getId).FirstOrDefault();
                        if (getNum != null)
                        {
                            do
                            {
                                getId++;
                                features.Id = getId;
                            } while (getNum.Id == getId);
                        }
                    }
                    else
                    {
                        getId = 1;
                    }

                    // добавление VIN-номера в таблицу FeaturesCars
                    features.VinNumber = vinTxtbox.Text;
                    //Добавление ПТС номера
                    features.PTC_number = ptcTxtBox.Text;
                    //добавление цвета
                    features.Color = colorTxtBox.Text;
                    // добавление возраста автомобиля
                    //features.Age = Convert.ToDateTime(dataAge.Text);
                    // добавление типа двигателя
                    features.TypeEngien = TypeEngTxtBox.Text;
                    // Добавление объема двигателя
                    features.VolumeEngien = Convert.ToDouble(volumeEngine.Text);
                    // Добавление мощности двигателя
                    features.EngienPower = Convert.ToDouble(EngPowerTxtBox.Text);
                    // добавление типа привода
                    features.TypeDriveMachine = typeDrive.Text;
                    // добавление типа кузова
                    features.TypeFrame = frameTxtbox.Text;
                    //добавление возраста автомобиля
                    features.Age = Convert.ToInt32(agetxtbox.Text);
                    features.Category = categoryTxtBox.Text;

                    db.FeaturesCars.Add(features);
                    db.SaveChanges();

                    clientes.Id = carClass.id;
                    var cId = db.Clientes.Where(r => r.Id == clientes.Id).FirstOrDefault();
                    if (cId == null)
                        clientes.Id = carClass.id;
                    clientes.VinNumber = vinTxtbox.Text;
                    clientes.sName = sNameTTxtBox.Text;
                    clientes.fName = fNameTxtBox.Text;
                    clientes.city = cityTxtBox.Text;
                    clientes.telephonNumber = teltxtBox.Text;
                    clientes.mail = mailTxtBox.Text;                   
                    
                    db.Clientes.Add(clientes);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow mw = this.Owner as MainWindow;
            if(mw != null)
            {
                LoadOnID(mw._ID);
                createBtn.IsEnabled = false;
            }
        }

        private async void LoadOnID(int Id)
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var query = await (from m in db.Cars
                               from c in db.Clientes
                               from f in db.FeaturesCars
                               where m.id == Id &&
                               c.VinNumber == m.VinNumber &&
                               f.VinNumber == m.VinNumber
                               select new
                               {
                                   m.Mark,
                                   m.Model,
                                   m.GosNumber,
                                   m.VinNumber,
                                   c.sName,
                                   c.fName,
                                   c.city,
                                   c.telephonNumber,
                                   c.mail,
                                   f.PTC_number,
                                   f.TypeDriveMachine,
                                   f.TypeEngien,
                                   f.TypeFrame,
                                   f.VolumeEngien,
                                   f.Age,
                                   f.Category,
                                   f.Color,
                                   f.EngienPower
                               }).ToListAsync();

                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    foreach(var q in query)
                    {
                        vinTxtbox.Text = q.VinNumber;
                        vinTxtbox.IsEnabled = false;

                        goxNumberTxtBox.Text = q.GosNumber;
                        goxNumberTxtBox.IsEnabled = false;

                        ptcTxtBox.Text = q.PTC_number;
                        ptcTxtBox.IsEnabled = false;

                        ModeltxtBox.Text = q.Model;
                        ModeltxtBox.IsEnabled = false;

                        MarkTxtBox.Text = q.Mark;
                        MarkTxtBox.IsEnabled = false;

                        colorTxtBox.Text = q.Color;
                        colorTxtBox.IsEnabled = false;

                        TypeEngTxtBox.Text = q.TypeEngien;
                        TypeEngTxtBox.IsEnabled = false;

                        EngPowerTxtBox.Text = q.EngienPower.ToString();
                        EngPowerTxtBox.IsEnabled = false;

                        volumeEngine.Text = q.VolumeEngien.ToString();
                        volumeEngine.IsEnabled = false;

                        frameTxtbox.Text = q.TypeFrame;
                        frameTxtbox.IsEnabled = false;

                        typeDrive.Text = q.TypeDriveMachine;
                        typeDrive.IsEnabled = false;

                        categoryTxtBox.Text = q.Category;
                        categoryTxtBox.IsEnabled = false;

                        sNameTTxtBox.Text = q.sName;
                        sNameTTxtBox.IsEnabled = false;

                        fNameTxtBox.Text = q.fName;
                        fNameTxtBox.IsEnabled = false;

                        cityTxtBox.Text = q.city;
                        cityTxtBox.IsEnabled = false;

                        teltxtBox.Text = q.telephonNumber;
                        teltxtBox.IsEnabled = false;

                        mailTxtBox.Text = q.mail;
                        mailTxtBox.IsEnabled = false;

                        agetxtbox.Text = q.Age.ToString();
                        agetxtbox.IsEnabled = false;
                    }
                });
            }
        }

    }   
}
