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
using Microsoft.Reporting.WinForms;
using Microsoft.Reporting;
using Microsoft.ReportingServices;
using System.Threading;
using System.Data;
using System.Data.Entity;
using System.Windows.Forms;

namespace SysManagmentCarApp.Models
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        
        public Report()
        {
            InitializeComponent();
            _reportViwer.Load += _reportViwer_Load;
        }

        private bool _isReportViwerLoaded;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

       

        private void _reportViwer_Load(object sender, EventArgs e)
        {
            if(!_isReportViwerLoaded)
            {
                // Microsoft.Reporting.WinForms.ReportDataSource reportDataSource = new ReportDataSource();
                // GarageDBDataSet dataSet = new GarageDBDataSet();
                // dataSet.BeginInit();

                // reportDataSource.Name = "DataSet1";
                // reportDataSource.Value = dataSet.History;
                // this._reportViwer.LocalReport.DataSources.Add(reportDataSource);
                // this._reportViwer.LocalReport.ReportEmbeddedResource = "SysManagmentCarApp.Report1.rdlc";
                // this._reportViwer.LocalReport.Render("PDF", null);
                // dataSet.EndInit();

                // GarageDBDataSetTableAdapters.HistoryTableAdapter historyTableAdapter = new GarageDBDataSetTableAdapters.HistoryTableAdapter();
                // historyTableAdapter.ClearBeforeFill = true;

                //// string sqlQuery = "";

                // historyTableAdapter.Fill(dataSet.History);
                // _reportViwer.RefreshReport();
                _newReport();

                _isReportViwerLoaded = true;
            }
        }

        private void _newReport()
        {
            ReportDataSource ds = new ReportDataSource();
            GarageDBDataSet3 garageDB = new GarageDBDataSet3();
            garageDB.BeginInit();
            ds.Name = "DataSet1";
            ds.Value = garageDB.GetOrder;
            string val = "22";
            
            ReportParameter[] parameter = new ReportParameter[]
            {
                new ReportParameter("@ID_Order", val)
            };
            this._reportViwer.LocalReport.DataSources.Add(ds);
            this._reportViwer.LocalReport.ReportEmbeddedResource = "SysManagmentCarApp.GetID_STP.rdlc";
            //this._reportViwer.LocalReport.SetParameters(parameter);
            GarageDBDataSet3TableAdapters.GetOrderTableAdapter getOrder = new GarageDBDataSet3TableAdapters.GetOrderTableAdapter();
            Order order = this.Owner as Order;            
            getOrder.Fill(garageDB.GetOrder, order.IDORder, order.VinNumber);
            this._reportViwer.RefreshReport();
        }
    }
}
