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

namespace Dish_Decision_Project
{
    /// <summary>
    /// Interaction logic for ChiTietMonAn.xaml
    /// </summary>
    public partial class ChiTietMonAn : Window
    {
        private String msg { get; set; }

        public ChiTietMonAn()
        {
            InitializeComponent();
        }
        public ChiTietMonAn(String _msg)
        {
            InitializeComponent();
            msg = _msg;
        }

    }
}
