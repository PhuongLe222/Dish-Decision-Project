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
using Accord.Math.Optimization.Losses;
using Accord.MachineLearning.DecisionTrees.Learning;
using System.Data;
using System.Data.SqlClient;
using Accord.MachineLearning.DecisionTrees;
using Accord.Math;
using Accord.Statistics.Filters;


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
            trainModel();            
        }
        
        private void optSoLuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(optSoLuong.Text);
            string value = optSoLuong.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
            int soluong = int.Parse(value.Split(new string[] { " " }, StringSplitOptions.None).First());
            var ComboBoxNL = new List<ComboBox>();
            ComboBoxNL.Add(txtNguyenLieu1);
            ComboBoxNL.Add(txtNguyenLieu2);
            ComboBoxNL.Add(txtNguyenLieu3);
            ComboBoxNL.Add(txtNguyenLieu4);
            ComboBoxNL.Add(txtNguyenLieu5);

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
                    ComboBoxNL[i].Visibility = Visibility.Visible;
                    lableNL[i].Visibility = Visibility.Visible;
                }
                else
                {
                    ComboBoxNL[i].Visibility = Visibility.Hidden;
                    lableNL[i].Visibility = Visibility.Hidden;
                }
            }         
          
        }

        private void optNgLieu1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string value = optLoaiMonAn.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
           
            Boolean LoaiMonAn;
            if (value == "Món chay")                
                LoaiMonAn = true;
            else
                LoaiMonAn = false;
            
            List<string> ListMaNgLieu1 = new List<string>();
            string connetionString;
            string sql;

            SqlConnection cnn;
            SqlCommand cmd;

            connetionString = @"Data Source=LAPTOP-O7B4R4R5\PHUONGLE;Initial Catalog=DecisionTree;User ID=sa;Password=210222810";
            cnn = new SqlConnection(connetionString);

            cnn.Open();
            sql = "select distinct NgLieu1 from  dbo.ClassificationMaterial where LoaiMonAn = '" + LoaiMonAn + "';";
            cmd = new SqlCommand(sql, cnn);
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                // Đọc từng dòng tập kết quả
                while (reader.Read())
                {
                    var MaNgLieu = reader["NgLieu1"];
                    ListMaNgLieu1.Add((string)MaNgLieu);
                }
            }
            else
            {
                Console.WriteLine("Không có dữ liệu trả về");
            }
            cnn.Close();


            foreach (var MaNgLieu1 in ListMaNgLieu1)
            {
                var output = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.MaNguyenLieu == MaNgLieu1).FirstOrDefault();
                string TenNgLieu1 = output.TenNguyenLieu;
                txtNguyenLieu1.Items.Add(TenNgLieu1);
            }
            cnn.Close();
        }

        private void optNgLieu2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string value = optLoaiMonAn.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();

            Boolean LoaiMonAn;
            if (value == "Món chay")
                LoaiMonAn = true;
            else
                LoaiMonAn = false;
            string TenNgLieu1 = txtNguyenLieu1.Text;
            if (TenNgLieu1 == "")
                TenNgLieu1 = txtNguyenLieu1.SelectedItem.ToString();
            string MaNgLieu = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu1).Select(p => p.MaNguyenLieu).FirstOrDefault();            

            List<string> ListMaNgLieu2 = new List<string>();
            string connetionString;
            string sql;

            SqlConnection cnn;
            SqlCommand cmd;

            connetionString = @"Data Source=LAPTOP-O7B4R4R5\PHUONGLE;Initial Catalog=DecisionTree;User ID=sa;Password=210222810";
            cnn = new SqlConnection(connetionString);

            cnn.Open();
            sql = "select distinct NgLieu2 from  dbo.ClassificationMaterial where Nglieu1 = '" + MaNgLieu + "' and LoaiMonAn = '" + LoaiMonAn + "';";
            cmd = new SqlCommand(sql, cnn);
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                // Đọc từng dòng tập kết quả
                while (reader.Read())
                {
                    var MaNgLieu2 = reader["NgLieu2"];
                    ListMaNgLieu2.Add((string)MaNgLieu2);
                }
            }
            else
            {
                Console.WriteLine("Không có dữ liệu trả về");
            }
            cnn.Close();


            foreach (var MaNgLieu2 in ListMaNgLieu2)
            {

                var output = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.MaNguyenLieu == MaNgLieu2).FirstOrDefault();
                if (output != null)
                {
                    string TenNgLieu2 = output.TenNguyenLieu;
                    txtNguyenLieu2.Items.Add(TenNgLieu2);
                }
            }
            cnn.Close();
        }

        private void optNgLieu3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string value = optLoaiMonAn.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
            Boolean LoaiMonAn;
            if (value == "Món chay")
                LoaiMonAn = true;
            else
                LoaiMonAn = false;
            string TenNgLieu1 = txtNguyenLieu1.Text;
            if (TenNgLieu1 == "")
                TenNgLieu1 = txtNguyenLieu1.SelectedItem.ToString();
            string MaNgLieu1 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu1).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string TenNgLieu2 = txtNguyenLieu2.Text;
            if (TenNgLieu2 == "")
                TenNgLieu2 = txtNguyenLieu2.SelectedItem.ToString();
            string MaNgLieu2 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu2).Select(p => p.MaNguyenLieu).FirstOrDefault();

            List<string> ListMaNgLieu3 = new List<string>();
            string connetionString;
            string sql;

            SqlConnection cnn;
            SqlCommand cmd;

            connetionString = @"Data Source=LAPTOP-O7B4R4R5\PHUONGLE;Initial Catalog=DecisionTree;User ID=sa;Password=210222810";
            cnn = new SqlConnection(connetionString);

            cnn.Open();
            sql = "select distinct NgLieu3 from  dbo.ClassificationMaterial where Nglieu1 = '" + MaNgLieu1 + "' and Nglieu2 = '" + MaNgLieu2 + "' and LoaiMonAn = '" + LoaiMonAn + "';";
            cmd = new SqlCommand(sql, cnn);
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                // Đọc từng dòng tập kết quả
                while (reader.Read())
                {
                    var MaNgLieu3 = reader["NgLieu3"];
                    ListMaNgLieu3.Add((string)MaNgLieu3);
                }
            }
            else
            {
                Console.WriteLine("Không có dữ liệu trả về");
            }
            cnn.Close();


            foreach (var MaNgLieu3 in ListMaNgLieu3)
            {

                var output = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.MaNguyenLieu == MaNgLieu3).FirstOrDefault();
                if (output != null)
                {
                    string TenNgLieu3 = output.TenNguyenLieu;
                    txtNguyenLieu3.Items.Add(TenNgLieu3);
                }
            }
            cnn.Close();            
        }

        private void optNgLieu4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string TenNgLieu1 = txtNguyenLieu1.Text;
            if (TenNgLieu1 == "")
                TenNgLieu1 = txtNguyenLieu2.SelectedItem.ToString();
            string MaNgLieu1 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu1).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string TenNgLieu2 = txtNguyenLieu2.Text;
            if (TenNgLieu2 == "")
                TenNgLieu2 = txtNguyenLieu2.SelectedItem.ToString();
            string MaNgLieu2 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu2).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string TenNgLieu3 = txtNguyenLieu3.Text;
            if (TenNgLieu3 == "")
                TenNgLieu3 = txtNguyenLieu2.SelectedItem.ToString();
            string MaNgLieu3 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu3).Select(p => p.MaNguyenLieu).FirstOrDefault();

            List<string> ListMaNgLieu4 = new List<string>();
            string connetionString;
            string sql;

            SqlConnection cnn;
            SqlCommand cmd;

            connetionString = @"Data Source=LAPTOP-O7B4R4R5\PHUONGLE;Initial Catalog=DecisionTree;User ID=sa;Password=210222810";
            cnn = new SqlConnection(connetionString);

            cnn.Open();
            sql = "select distinct NgLieu4 from  dbo.ClassificationMaterial where Nglieu1 = '" + MaNgLieu1 + "' and Nglieu2 = '" + MaNgLieu2 + "' and Nglieu3 = '" + MaNgLieu3 + "';";
            cmd = new SqlCommand(sql, cnn);
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                // Đọc từng dòng tập kết quả
                while (reader.Read())
                {
                    var MaNgLieu4 = reader["NgLieu4"];
                    ListMaNgLieu4.Add((string)MaNgLieu4);
                }
            }
            else
            {
                Console.WriteLine("Không có dữ liệu trả về");
            }
            cnn.Close();


            foreach (var MaNgLieu4 in ListMaNgLieu4)
            {

                var output = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.MaNguyenLieu == MaNgLieu4).FirstOrDefault();
                if (output != null)
                {
                    string TenNgLieu4 = output.TenNguyenLieu;
                    txtNguyenLieu4.Items.Add(TenNgLieu4);
                }
            }
            cnn.Close();
        }

        private void optNgLieu5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string TenNgLieu1 = txtNguyenLieu1.Text;
            if (TenNgLieu1 == "")
                TenNgLieu1 = txtNguyenLieu2.SelectedItem.ToString();
            string MaNgLieu1 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu1).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string TenNgLieu2 = txtNguyenLieu2.Text;
            if (TenNgLieu2 == "")
                TenNgLieu2 = txtNguyenLieu2.SelectedItem.ToString();
            string MaNgLieu2 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu2).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string TenNgLieu3 = txtNguyenLieu3.Text;
            if (TenNgLieu3 == "")
                TenNgLieu3 = txtNguyenLieu2.SelectedItem.ToString();
            string MaNgLieu3 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu3).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string TenNgLieu4 = txtNguyenLieu4.Text;
            if (TenNgLieu4 == "")
                TenNgLieu4 = txtNguyenLieu2.SelectedItem.ToString();
            string MaNgLieu4 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == TenNgLieu4).Select(p => p.MaNguyenLieu).FirstOrDefault();

            List<string> ListMaNgLieu5 = new List<string>();
            string connetionString;
            string sql;

            SqlConnection cnn;
            SqlCommand cmd;

            connetionString = @"Data Source=LAPTOP-O7B4R4R5\PHUONGLE;Initial Catalog=DecisionTree;User ID=sa;Password=210222810";
            cnn = new SqlConnection(connetionString);

            cnn.Open();
            sql = "select distinct NgLieu5 from  dbo.ClassificationMaterial where Nglieu1 = '" + MaNgLieu1 + "' and Nglieu2 = '" + MaNgLieu2 + "' and Nglieu3 = '" + MaNgLieu3 + "' and Nglieu4 = '" + MaNgLieu4 + "';";
            cmd = new SqlCommand(sql, cnn);
            var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                // Đọc từng dòng tập kết quả
                while (reader.Read())
                {
                    var MaNgLieu5 = reader["NgLieu5"];
                    ListMaNgLieu5.Add((string)MaNgLieu5);
                }
            }
            else
            {
                Console.WriteLine("Không có dữ liệu trả về");
            }
            cnn.Close();


            foreach (var MaNgLieu5 in ListMaNgLieu5)
            {

                var output = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.MaNguyenLieu == MaNgLieu5).FirstOrDefault();
                if (output != null)
                {
                    string TenNgLieu5 = output.TenNguyenLieu;
                    txtNguyenLieu5.Items.Add(TenNgLieu5);
                }
            }
            cnn.Close();
        }

        private void btnSearchNL_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ front-end
            String num_nguyenlieu = optSoLuong.Text;
            String type_nguyenlieu = optLoaiMonAn.Text;

            string nguyenlieu1 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == txtNguyenLieu1.Text).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string nguyenlieu2 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == txtNguyenLieu2.Text).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string nguyenlieu3 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == txtNguyenLieu3.Text).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string nguyenlieu4 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == txtNguyenLieu4.Text).Select(p => p.MaNguyenLieu).FirstOrDefault();
            string nguyenlieu5 = DataProvider.Ins.DB.NGUYENLIEUx.Where(p => p.TenNguyenLieu == txtNguyenLieu5.Text).Select(p => p.MaNguyenLieu).FirstOrDefault();
            if (nguyenlieu2 == null)
                nguyenlieu2 = "";
            if (nguyenlieu3 == null)
                nguyenlieu3 = "";
            if (nguyenlieu4 == null)
                nguyenlieu4 = "";
            if (nguyenlieu5 == null)
                nguyenlieu5 = "";

            //MessageBox.Show(
            //    "Số lượng nguyên liệu: " + num_nguyenlieu + "\n" +
            //    "Loại nguyên liêu: " + type_nguyenlieu + "\n" +
            //    "Nguyên liêu 1: " + nguyenlieu1 + "\n"
            //    );
            // Truyền tham số và mở trang chi tiết món ăn
            String recommendMonAn;
            if (type_nguyenlieu.Equals("Món chay"))
                recommendMonAn = decide("True", nguyenlieu1, nguyenlieu2, nguyenlieu3, nguyenlieu4, nguyenlieu5);
            else
                recommendMonAn = decide("False", nguyenlieu1, nguyenlieu2, nguyenlieu3, nguyenlieu4, nguyenlieu5);

            // todo: cần một hàm để convert từ MaMonAn -> TenMonAn;
            if (recommendMonAn != "")
            {
                mon_an = new ChiTietMonAn(this, recommendMonAn.ToString());
                mon_an.Show();
            }            
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
        private DataTable data;
        private DecisionTree tree;
        private Codification codebook;

        private string decide(string loaiMonAn, string NgLieu1, string NgLieu2, string NgLieu3, string NgLieu4, string NgLieu5)
        {
            // Thử nghiệm
            string answer = "";
            try
            {
                int[] query = codebook.Transform(new[,]
                            {
                { "LoaiMonAn",    loaiMonAn  },
                { "NguyenLieu1",  NgLieu1},
                { "NguyenLieu2",  NgLieu2},
                { "NguyenLieu3",  NgLieu3},
                { "NguyenLieu4",  NgLieu4},
                { "NguyenLieu5",  NgLieu5},

            });
                int predicted = tree.Decide(query);
                answer = codebook.Revert("MaMonAn", predicted);
            }
            catch
            {
                MessageBox.Show("Không thể tìm thấy món ăn phù hợp với nguyên liệu này");
            }
            //MessageBox.Show(answer);
            return answer;
        }
        private void trainModel()
        {
            string connetionString;
            string sql;

            SqlConnection cnn;
            SqlCommand sqlCommand;
            SqlDataReader sqlDataReader;
            

            connetionString = @"Data Source=LAPTOP-O7B4R4R5\PHUONGLE;Initial Catalog=DecisionTree;User ID=sa;Password=210222810";
            cnn = new SqlConnection(connetionString);
            data = new DataTable("Food Recommend");

            // Định nghĩa các columns
            data.Columns.Add("LoaiMonAn");
            data.Columns.Add("NguyenLieu1");
            data.Columns.Add("NguyenLieu2");
            data.Columns.Add("NguyenLieu3");
            data.Columns.Add("NguyenLieu4");
            data.Columns.Add("NguyenLieu5");
            data.Columns.Add("MaMonAn");

            // Lấy dữ liệu từ database
            cnn.Open();
            sql = "SELECT *  FROM ClassificationMaterial";
            sqlCommand = new SqlCommand(sql, cnn);
            sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                data.Rows.Add(
                    sqlDataReader.GetValue(1),
                    sqlDataReader.GetValue(2),
                    sqlDataReader.GetValue(3),
                    sqlDataReader.GetValue(4),
                    sqlDataReader.GetValue(5),
                    sqlDataReader.GetValue(6),
                    sqlDataReader.GetValue(7)
                );
            }
            cnn.Close();

            // Tạo input và output
            codebook = new Codification(data);
            DataTable symbols = codebook.Apply(data);
            int[][] inputs = symbols.ToArray<int>("LoaiMonAn", "NguyenLieu1", "NguyenLieu2", "NguyenLieu3", "NguyenLieu4", "NguyenLieu5");
            int[] outputs = symbols.ToArray<int>("MaMonAn");

            // Định nghĩa mô hình
            var id3learning = new ID3Learning()
            {

                new DecisionVariable("LoaiMonAn",        3),    // 3 possible values 
                new DecisionVariable("NguyenLieu1", 130),  // 130 possible values  
                new DecisionVariable("NguyenLieu2", 150),  // 150 possible values   
                new DecisionVariable("NguyenLieu3",   111),  // 111 possible values 
                new DecisionVariable("NguyenLieu4",   108),  // 108 possible values   
                new DecisionVariable("NguyenLieu5",   90)    // 90 possible values 
            };

            // Huấn luyện
            tree = id3learning.Learn(inputs, outputs);

            // Đánh giá
            double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));
            
        }
    }
}
