using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gameco
{
    public partial class Form3: Form
    {
        public Form3()
        {
            InitializeComponent();
        }

		private void button3_Click(object sender, EventArgs e)
		{
			
			Form1 form1 = new Form1(3); // 1 là mức độ Dễ
			form1.Show();
			this.Close(); // Cuối cùng, đóng form hiện tại
		}

		private void easy_btn_Click(object sender, EventArgs e)
		{
			
			Form1 form1 = new Form1(1); // 1 là mức độ Dễ
			form1.Show();
			this.Close(); // Cuối cùng, đóng form hiện tại
		}


		private void normal_btn_Click(object sender, EventArgs e)
		{
			

			Form1 form1 = new Form1(2); // 1 là mức độ Dễ
			form1.Show();
			this.Close(); // Cuối cùng, đóng form hiện tại
			
		}

		private void exit_btn_Click(object sender, EventArgs e)
		{
			Form2 form2 = new Form2();
			form2.Show();
			this.Close();
		}
	}
}
