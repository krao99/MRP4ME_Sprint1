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

namespace MRP4ME.View
{
    /// <summary>
    /// Interaction logic for BOM.xaml
    /// </summary>
    public partial class BOMView : UserControl
    {
        public BOMView()
        {
            InitializeComponent();
        }

        private void dgBOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
