using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class KhachHang : Form
    {
        private ServerConnection server = new ServerConnection();
        private List<MonAnViewModel> menuDisplayList;

        public KhachHang()
        {
            InitializeComponent();
            ConfigDataGridView(); // Cấu hình bảng ngay khi mở form
        }

        private void ConfigDataGridView()
        {
            dgvMenu.AutoGenerateColumns = false;
            dgvMenu.AllowUserToAddRows = false; // Không cho tự thêm dòng trống

            // Cột ID (Ẩn hoặc hiện tùy bạn)
            dgvMenu.Columns.Add(new DataGridViewTextBoxColumn()
            { DataPropertyName = "ID", HeaderText = "ID", ReadOnly = true, Width = 50 });

            // Cột Tên Món
            dgvMenu.Columns.Add(new DataGridViewTextBoxColumn()
            { DataPropertyName = "TenMon", HeaderText = "Tên Món", ReadOnly = true, Width = 200 });

            // Cột Đơn Giá
            dgvMenu.Columns.Add(new DataGridViewTextBoxColumn()
            { DataPropertyName = "DonGia", HeaderText = "Đơn Giá", ReadOnly = true, Width = 100 });

            // Cột Số Lượng (QUAN TRỌNG: Cho phép sửa - ReadOnly = false)
            dgvMenu.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "SoLuong",
                HeaderText = "Số Lượng (Nhập tại đây)",
                ReadOnly = false, // Cho phép người dùng nhập
                DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.LightYellow } // Đổi màu cho dễ nhìn
            });
        }

        private async Task LoadMenu()
        {
            try
            {
                // Lấy List<MonAn> từ server
                List<MonAn> rawMenu = await Task.Run(() => server.GetMenu());

                if (rawMenu != null)
                {
                    // Chuyển đổi sang ViewModel để có trường SoLuong = 0
                    menuDisplayList = rawMenu.Select(m => new MonAnViewModel
                    {
                        ID = m.ID,
                        TenMon = m.TenMon,
                        DonGia = m.DonGia,
                        SoLuong = 0
                    }).ToList();

                    // Đổ dữ liệu vào Grid
                    dgvMenu.DataSource = menuDisplayList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải menu: " + ex.Message);
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false; // Chống spam click
            this.Text = "Đang kết nối...";

            try
            {
                // Kết nối tới Server (Chạy task ngầm)
                bool connected = await Task.Run(() => server.Connect("127.0.0.1", 8888));

                if (connected)
                {
                    this.Text = "ĐÃ KẾT NỐI SERVER";
                    MessageBox.Show("Kết nối thành công! Đang tải menu...");

                    // Gọi hàm lấy Menu
                    await LoadMenu();
                }
                else
                {
                    this.Text = "KẾT NỐI THẤT BẠI";
                    MessageBox.Show("Không thể kết nối Server (Hãy chắc chắn Server đang chạy).");
                    btnConnect.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                btnConnect.Enabled = true;
            }
        }

        // Lưu ý: Phải thêm 'async' vào đây vì bên trong dùng 'await'
        private async void btnOrder_Click(object sender, EventArgs e)
        {
            if (!server.IsConnected)
            {
                MessageBox.Show("Chưa kết nối Server!");
                return;
            }

            // Lấy số bàn
            int tableID = (int)nudTable.Value;
            if (tableID <= 0)
            {
                MessageBox.Show("Vui lòng nhập số bàn hợp lệ!");
                return;
            }

            // Lọc ra những món khách đã chọn (Số lượng > 0)
            if (menuDisplayList == null) return;
            var itemsToOrder = menuDisplayList.Where(m => m.SoLuong > 0).ToList();

            if (itemsToOrder.Count == 0)
            {
                MessageBox.Show("Bạn chưa chọn món nào (Số lượng phải > 0).");
                return;
            }

            // Gửi Order từng món lên Server
            btnOrder.Enabled = false; // Khóa nút
            int successCount = 0;
            string errorLog = "";

            foreach (var item in itemsToOrder)
            {
                // Gọi hàm Order của ServerConnection (chạy async để không đơ)
                var result = await Task.Run(() => server.Order(tableID, item.ID, item.SoLuong));

                if (result.success)
                {
                    successCount++;
                    // Reset số lượng về 0 sau khi đặt thành công
                    item.SoLuong = 0;
                }
                else
                {
                    errorLog += $"- {item.TenMon}: {result.msg}\n";
                }
            }

            // Cập lại giao diện
            dgvMenu.Refresh(); // Refresh lại grid (để số lượng về 0 hiển thị lên UI)
            btnOrder.Enabled = true;

            // Thông báo kết quả
            string message = $"Đã đặt thành công {successCount} món.";
            if (!string.IsNullOrEmpty(errorLog))
            {
                message += $"\n\nCó lỗi xảy ra:\n{errorLog}";
            }
            MessageBox.Show(message); // Đã thêm dấu chấm phẩy
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Disconnect();
        }

        private void KhachHang_Load(object sender, EventArgs e)
        {

        }
    }

    public class MonAnViewModel
    {
        public int ID { get; set; }
        public string TenMon { get; set; }
        public decimal DonGia { get; set; }

        public int SoLuong { get; set; } = 0;
    }
}