using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace Client
{
    public partial class ThuNgan : Form
    {
        // Sử dụng Class kết nối mới
        private ServerConnection _server = new ServerConnection();
        private const string SERVER_IP = "127.0.0.1";
        private const int SERVER_PORT = 8888;

        public ThuNgan()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            // Gán sự kiện
            this.Load += ThuNgan_Load;
            this.button1.Click += button1_Click; // Nút Charge
            this.FormClosing += ThuNgan_FormClosing;
        }

        private void ThuNgan_Load(object sender, EventArgs e)
        {
            SetupGridView();
            ConnectToServer();
        }

        private void SetupGridView()
        {
            // Cấu hình bảng
            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Bàn";
            dataGridView1.Columns[1].Name = "Tên Món";
            dataGridView1.Columns[2].Name = "SL";
            dataGridView1.Columns[3].Name = "Thành Tiền";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void ConnectToServer()
        {
            bool result = _server.Connect(SERVER_IP, SERVER_PORT);
            if (result)
            {
                this.Text = "Thu Ngân - Đã kết nối";
                // Lấy danh sách đơn hàng ngay khi kết 
                LoadOrders();

                
            }
            else
            {
                MessageBox.Show("Không thể kết nối Server!");
                this.Text = "Thu Ngân - Mất kết nối";
            }
        }

        private void LoadOrders()
        {
            if (!_server.IsConnected) return;

            // Dùng hàm GetOrders từ ServerConnection.cs
            var listOrders = _server.GetOrders();
            
            // Đổ dữ liệu vào Grid
            dataGridView1.Rows.Clear();
            foreach (var item in listOrders)
            {
                dataGridView1.Rows.Add(item.Ban, item.TenMon, item.SL, item.ThanhTien);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Nút Thanh Toán
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Nhập số bàn cần thanh toán!");
                return;
            }

            if (int.TryParse(textBox1.Text, out int banID))
            {
                // Gọi hàm Pay từ ServerConnection
                var result = _server.Pay(banID);

                if (result.success)
                {
                    label3.Text = result.total.ToString("N0") + " VNĐ";
                    MessageBox.Show(result.msg + "\nTổng tiền: " + result.total);
                    
                    // Xuất file bill
                    ExportBill(banID, result.total);

                    // Refresh lại danh sách
                    LoadOrders();
                }
                else
                {
                    MessageBox.Show("Lỗi: " + result.msg);
                }
            }
            else
            {
                MessageBox.Show("Số bàn phải là số nguyên!");
            }
        }

        private void ExportBill(int banID, decimal total)
        {
            try
            {
                string fileName = $"Bill_Ban{banID}_{DateTime.Now:HHmmss}.txt";
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine($"=== HÓA ĐƠN BÀN {banID} ===");
                    sw.WriteLine($"Thời gian: {DateTime.Now}");
                    sw.WriteLine("---------------------------");
                    // In các món của bàn này đang hiện trên Grid
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[0].Value?.ToString() == banID.ToString())
                        {
                            sw.WriteLine($"{row.Cells[1].Value} x {row.Cells[2].Value} = {row.Cells[3].Value}");
                        }
                    }
                    sw.WriteLine("---------------------------");
                    sw.WriteLine($"TỔNG CỘNG: {total:N0} VNĐ");
                }
            }
            catch { }
        }

        private void ThuNgan_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server.Disconnect();
        }
    }
}