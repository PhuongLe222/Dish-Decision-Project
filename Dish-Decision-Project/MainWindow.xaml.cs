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

            string nguyenlieu1 = txtNguyenLieu1.Text;
            string nguyenlieu2 = txtNguyenLieu2.Text;
            string nguyenlieu3 = txtNguyenLieu3.Text;
            string nguyenlieu4 = txtNguyenLieu4.Text;
            string nguyenlieu5 = txtNguyenLieu5.Text;


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

            mon_an = new ChiTietMonAn(this, recommendMonAn.ToString());           
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
        private DataTable data;
        private DecisionTree tree;
        private Codification codebook;

        private string decide(string loaiMonAn, string NgLieuChinh1, string NgLieuChinh2, string NgLieuPhu1, string NgLieuPhu2, string NgLieuPhu3)
        {
            // Thử nghiệm
            int[] query = codebook.Transform(new[,]
            {
                { "LoaiMonAn",         loaiMonAn  },
                { "NguyenLieuChinh1",  NgLieuChinh1},
                { "NguyenLieuChinh2",  NgLieuChinh2},
                { "NguyenLieuPhu1",    NgLieuPhu1},
                { "NguyenLieuPhu2",    NgLieuPhu2},
                { "NguyenLieuPhu3",    NgLieuPhu3},

            });

            int predicted = tree.Decide(query);
            string answer = codebook.Revert("MaMonAn", predicted);
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
            

            connetionString = @"Data Source=TRANTRAN;Initial Catalog=DecisionTree;User ID=user;Password=1234";
            cnn = new SqlConnection(connetionString);
            data = new DataTable("Food Recommend");

            // Định nghĩa các columns
            data.Columns.Add("LoaiMonAn");
            data.Columns.Add("NguyenLieuChinh1");
            data.Columns.Add("NguyenLieuChinh2");
            data.Columns.Add("NguyenLieuPhu1");
            data.Columns.Add("NguyenLieuPhu2");
            data.Columns.Add("NguyenLieuPhu3");
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
            int[][] inputs = symbols.ToArray<int>("LoaiMonAn", "NguyenLieuChinh1", "NguyenLieuChinh2", "NguyenLieuPhu1", "NguyenLieuPhu2", "NguyenLieuPhu3");
            int[] outputs = symbols.ToArray<int>("MaMonAn");

            // Định nghĩa mô hình
            var id3learning = new ID3Learning()
            {

                new DecisionVariable("LoaiMonAn",        3),    // 3 possible values 
                new DecisionVariable("NguyenLieuChinh1", 130),  // 130 possible values  
                new DecisionVariable("NguyenLieuChinh2", 140),  // 140 possible values   
                new DecisionVariable("NguyenLieuPhu1",   111),  // 111 possible values 
                new DecisionVariable("NguyenLieuPhu2",   108),  // 108 possible values   
                new DecisionVariable("NguyenLieuPhu3",   91)    // 91 possible values 
            };

            // Huấn luyện
            tree = id3learning.Learn(inputs, outputs);

            // Đánh giá
            double error = new ZeroOneLoss(outputs).Loss(tree.Decide(inputs));
            
        }
    }
}
