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
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void button3_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.Yes)
			{
				Application.Exit();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			
		}


		private void button2_Click(object sender, EventArgs e)
		{

		}

		private void button4_Click(object sender, EventArgs e)
		{
			string gameRules = "Luật chơi:\n" +
							   "1. Quân cờ màu xanh đi trước, quân cờ màu đỏ đi sau.\n" +
							   "2. Quân cờ chỉ có thể di chuyển theo hướng ngang và dọc.\n" +
							   "3. Các quân cờ không thể di chuyển chéo.\n" +
							   "4. Mỗi lượt, người chơi chỉ được di chuyển một quân cờ.\n" +
							   "5. Trò chơi kết thúc khi một bên không còn quân cờ nào có thể di chuyển.";

			MessageBox.Show(gameRules, "Luật chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

	}
}
