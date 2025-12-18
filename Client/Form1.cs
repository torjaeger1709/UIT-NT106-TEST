namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Green;


            KhachHang f2 = new KhachHang();


            f2.Show();
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            button2.BackColor = Color.Green;


            ThuNgan f3 = new ThuNgan();


            f3.Show();
        }
    }
}
