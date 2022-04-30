using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dish_Decision_Project.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        public MainViewModel()
           {
            
            MessageBox.Show("Đã vào trong MainViewModel -> DataContext của mainwindow.xaml");
        }
    }
}
