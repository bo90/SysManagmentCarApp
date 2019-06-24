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
using System.Data;
using System.Data.Entity;

namespace SysManagmentCarApp.Models
{
    /// <summary>
    /// Логика взаимодействия для NewEmp.xaml
    /// </summary>
    public partial class NewEmp : Window
    {
        public NewEmployee newEmp { get; set; }
        public Employess emp { get; set; }

        public NewEmp()
        {
            newEmp = new NewEmployee();
            emp = new Employess();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewRecord();
            MessageBox.Show("Запись создана!", "Результат");
        }

        

        private void NewRecord()
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                // добавление в таблицу Employess
                emp.NameEmp = nameTxtBox.Text;
                emp.profession = professTextBoxs.Text;
                emp.sNameEnp = secondNameTxtBox.Text;
                emp.yearBorn = Convert.ToDateTime(ageBox.Text);
                emp.Id = Convert.ToInt32(numberEmptxtBox.Text);
                db.Employess.Add(emp);
                db.SaveChanges();
                // Добавление в таблицу newEmployee                
                newEmp.nameEmp = nameTxtBox.Text;
                newEmp.sName = nameTxtBox.Text;
                newEmp.profession = professTextBoxs.Text;
                newEmp.age = Convert.ToDateTime(ageBox.Text);
                newEmp.originCity = cityTxtBox.Text;
                newEmp.telephone = phoneTxtBox.Text;
                //var id = db.Employess.Where(r => r.Id == newEmp.idEmp).FirstOrDefault();
                newEmp.idEmp = emp.Id;
                newEmp.id = Convert.ToInt32(numberEmptxtBox.Text);
                newEmp.email = mailTxtBox.Text;
                newEmp.enducation = enduTxtBox.Text;
                newEmp.enduPlace = enduPlaceTxtBox.Text;
                db.NewEmployee.Add(newEmp);
                db.SaveChanges();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScanLoad();            
        }

        private async void ScanLoad()
        {
            EmpForm ef = this.Owner as EmpForm;
            if (ef != null)
            {

                int ID = ef.ChangeRecord();
                await Task.Run(() => SelectInfo(ID));
                changeBtn.Visibility = Visibility.Visible;
                createBtn.Visibility = Visibility.Hidden;
            }        
            else
            {
                changeBtn.Visibility = Visibility.Hidden;
                createBtn.Visibility = Visibility.Visible;
            }
            
        }
           

        private async void SelectInfo(int ID)
        {
            using (GarageDBEntities db = new GarageDBEntities())
            {
                var info = await (from m in db.NewEmployee
                                  where m.id == ID
                                  select m).ToListAsync();
                await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (ThreadStart)delegate ()
               {
                   foreach (var i in info)
                   {
                       numberEmptxtBox.Text += string.Format("{0}\r\n", i.id);
                       secondNameTxtBox.Text += string.Format("{0}\r\n", i.sName);
                       nameTxtBox.Text += string.Format("{0}\r\n", i.nameEmp);
                       cityTxtBox.Text += string.Format("{0}\r\n", i.originCity);
                       ageBox.Text += string.Format("{0}\r\n", i.age);
                       phoneTxtBox.Text += string.Format("{0}\r\n", i.telephone);
                       mailTxtBox.Text += string.Format("{0}\r\n", i.email);
                       enduTxtBox.Text += string.Format("{0}\r\n", i.enducation);
                       enduPlaceTxtBox.Text += string.Format("{0}\r\n", i.enduPlace);
                       professTextBoxs.Text += string.Format("{0}\r\n", i.profession);
                   }

               });
            }
        }
    }
}
