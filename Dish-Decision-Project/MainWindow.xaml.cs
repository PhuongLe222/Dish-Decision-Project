using Dish_Decision_Project.Model;
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


namespace Dish_Decision_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChiTietMonAn mon_an;

        public ChiTietMonAn monAn {
            get { return mon_an; }
            set { mon_an = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            mon_an = null;
        }

        private void optSoLuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(optSoLuong.SelectedItem.ToString());
            string value = optSoLuong.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
            int soluong = int.Parse(value.Split(new string[] { " " }, StringSplitOptions.None).First());
            var textboxNL = new List<TextBox>();
            textboxNL.Add(txtNguyenLieu1);
            textboxNL.Add(txtNguyenLieu2);
            textboxNL.Add(txtNguyenLieu3);
            textboxNL.Add(txtNguyenLieu4);
            textboxNL.Add(txtNguyenLieu5);

            var lableNL = new List<TextBlock>();
            lableNL.Add(txtNL1);
            lableNL.Add(txtNL2);
            lableNL.Add(txtNL3);
            lableNL.Add(txtNL4);
            lableNL.Add(txtNL5);

            for (int i = 0; i < 5; i++)
            {
                if(i < soluong)
                {
                    textboxNL[i].Visibility = Visibility.Visible;
                    lableNL[i].Visibility = Visibility.Visible;
                }
                else
                {
                    textboxNL[i].Visibility = Visibility.Hidden;
                    lableNL[i].Visibility = Visibility.Hidden;
                }
            }         
          
        }

        
        private void btnSearchNL_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ front-end
            String num_nguyenlieu = optSoLuong.Text;
            String type_nguyenlieu = optLoaiMonAn.Text;
            String nguyenlieu1 = txtNguyenLieu1.Text;

            //MessageBox.Show(
            //    "Số lượng nguyên liệu: " + num_nguyenlieu + "\n" +
            //    "Loại nguyên liêu: " + type_nguyenlieu + "\n" +
            //    "Nguyên liêu 1: " + nguyenlieu1 + "\n"
            //    );
            // Truyền tham số và mở trang chi tiết món ăn
            mon_an = new ChiTietMonAn(this, "MA0009");           
            mon_an.Show();
        }


        private void btnSearchMA_Click(object sender, RoutedEventArgs e)              
        {
            // Lấy dữ liệu từ front-end
            String ten_monan = txtSearch.Text;
            var outputList = DataProvider.Ins.DB.MONANs.Where(p => p.TenMonAn == ten_monan);         
            int CheckMA = int.Parse(outputList.Count().ToString());
            if (CheckMA == 0)
            {
                MessageBox.Show("Không thể tìm thấy tên món ăn này. Mời bạn nhập thử tên món ăn khác!");
            }
            else
            {
                if (mon_an != null)  mon_an.Close();

                mon_an = new ChiTietMonAn(this, ten_monan);
                mon_an.Show();
            }
        }
    }
}
