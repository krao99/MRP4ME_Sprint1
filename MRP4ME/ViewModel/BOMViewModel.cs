using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRP4ME.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using System.Windows;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Data.Entity;


namespace MRP4ME.ViewModel
{
    public class BOMViewModel : WorkspaceViewModel
    {
        sales_order _sales_order = new sales_order();
        RelayCommand _deleteSOCommand;
        private ObservableCollection<sales_order> _salesorders;

        private static BOMViewModel _instance = new BOMViewModel();
        public static BOMViewModel Instance { get { return _instance; } }

        public BOMViewModel()
        {
            
        }

        public ObservableCollection<sales_order> Salesorders
        {
            get
            {
                using (var dbContext = new MRP4MEEntities())
                {
                    _salesorders = new ObservableCollection<sales_order>(dbContext.sales_order.ToList());
                }
                return _salesorders;
            }
            set
            {
                NotifyPropertyChanged("Salesorders");
                return;
            }
        }
        /// <summary>
        /// Returns a command that delete the sales order.
        /// </summary>
        public ICommand DeleteSOCommand
        {
            get
            {
                if (_deleteSOCommand == null)
                {
                    _deleteSOCommand = new RelayCommand(
                        param => this.DeleteSO((string)param)
                         );
                }
                return _deleteSOCommand;
            }
        }

        /// <summary>
        /// delete the sales order from the database.  This method is invoked by the DeleteSOCommand.
        /// </summary>
        public void DeleteSO(string SONumber)
        {
            MessageBoxResult messageBoxResult = 
                System.Windows.MessageBox.Show("Are you sure to delete sales order # " + SONumber + " ?", "MRP4ME - Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                using (var dbContext = new MRP4MEEntities())
                {
                    sales_order item = Salesorders.FirstOrDefault(x => x.so_number == SONumber);
                    dbContext.sales_order.Attach(item);
                    dbContext.sales_order.Remove(item);
                    dbContext.SaveChanges();
                    dbContext.Entry(item).State = System.Data.Entity.EntityState.Detached;
                    Salesorders.Remove(item);
                    NotifyPropertyChanged("Salesorders");
                }
             }
           }
    }
    
}
