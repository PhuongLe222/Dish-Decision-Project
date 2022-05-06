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
using Dish_Decision_Project.Model;

namespace Dish_Decision_Project
{
    /// <summary>
    /// Interaction logic for ChiTietMonAn.xaml
    /// </summary>
    public partial class ChiTietMonAn : Window
    {

        private String msg { get; set; }

        private MainWindow mainScreen { get; set; }

        public ChiTietMonAn()
        {
            InitializeComponent();
        }
        public ChiTietMonAn(MainWindow _mainScreen, string _msg)
        {
            InitializeComponent();
            msg = _msg;
            mainScreen = _mainScreen;
            HienThiMonAN();
        }
        private void btnSearchMA_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ front-end
            String ten_monan = txtSearch.Text;

            // Truyền tham số và mở trang chi tiết món ăn
            ChiTietMonAn mon_an = new ChiTietMonAn(ten_monan);
            mon_an.Show();
            this.Close();

        private void HienThiMonAN()
        {
            var output = DataProvider.Ins.DB.MONANs.Where(p => p.TenMonAn == msg);
            var MonAn = output.FirstOrDefault();
            txtTenMonAn.Text = MonAn.TenMonAn;
            txtCachThucHien.Text = MonAn.CachThucHien;
            HinhAnh.Height = 400;
            HinhAnh.Source = new BitmapImage(new Uri(MonAn.HinhAnh));
            ListTenNL.Items.Clear();
            var num_nguyenlieu = MonAn.CTMAs.Count();
            foreach (var item in MonAn.CTMAs)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = item.NGUYENLIEU.TenNguyenLieu;
                ListTenNL.Items.Add(textBlock);
            }
            ListLieuLuong.Items.Clear();
            var num_lieuluong = MonAn.CTMAs.Count();
            foreach (var item in MonAn.CTMAs)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = item.LieuLuong;
                ListLieuLuong.Items.Add(textBlock);
            }

        }
        
        private void btnSearchMA_Click(object sender, RoutedEventArgs e)
        {
            String ten_monan = txtSearch.Text;
            var outputList = DataProvider.Ins.DB.MONANs.Where(p => p.TenMonAn == ten_monan);
            int CheckMA = int.Parse(outputList.Count().ToString());
            if (CheckMA == 0)
            {
                MessageBox.Show("Không thể tìm thấy tên món ăn này. Mời bạn nhập thử tên món ăn khác!");
            }
            else
            {                                  
                mainScreen.monAn = new ChiTietMonAn(mainScreen, ten_monan);
                mainScreen.monAn.Show();
                this.Close();
            }
        }
    }
}
