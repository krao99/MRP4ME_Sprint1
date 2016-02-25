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

namespace MRP4ME.UIElementsLib
{
    /// <summary>
    /// Interaction logic for MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        RelayCommand _openPOCommand;

        public MenuControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns a command that saves the customer.
        /// </summary>
        public ICommand OpenPOCommand
        {
            get
            {
                if (_openPOCommand == null)
                {
                    _openPOCommand = new RelayCommand(
                        param => this.OpenPO()
                        );
                }
                return _openPOCommand;
            }
        }

        private object OpenPO()
        {
            throw new NotImplementedException();
        }
    }
}
