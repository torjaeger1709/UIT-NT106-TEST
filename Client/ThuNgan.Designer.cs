namespace Client
{
    partial class ThuNgan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            button1 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(776, 307);
            dataGridView1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(661, 367);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "Charge";
            button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(119, 359);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(56, 362);
            label1.Name = "label1";
            label1.Size = new Size(55, 20);
            label1.TabIndex = 3;
            label1.Text = "Số bàn";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(410, 367);
            label2.Name = "label2";
            label2.Size = new Size(72, 20);
            label2.TabIndex = 4;
            label2.Text = "Tổng tiền";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(503, 367);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 5;
            label3.Text = "(tổng)";
            // 
            // ThuNgan
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Name = "ThuNgan";
            Text = "ThuNgan";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}