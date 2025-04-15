using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gameco
{
	public partial class Form1 : Form
	{
		private const int CELL_SIZE = 100;
		private const int BOARD_SIZE = 5;
		private int BOARD_PIXEL = CELL_SIZE * (BOARD_SIZE - 1);
		internal static Button[,] boardButtons = new Button[BOARD_SIZE, BOARD_SIZE]; // Change to internal
		private chess[,] boardPieces = new chess[BOARD_SIZE, BOARD_SIZE];
		private chess selectedChess = null;
		private bool isBlueTurn = true; // Biến theo dõi lượt hiện tại
		private int gameMode;
		private object bot;
		private const int OFFSET = 20;
		private int newCellSize = (int)(CELL_SIZE * SCALE_FACTOR);
		private int newButtonSize = (int)(20 * SCALE_FACTOR);
		private int newBoardPixel;
		private const float SCALE_FACTOR = 1.15f;
		public Form1(int difficultyLevel)
		{
			InitializeComponent();
			this.Width = BOARD_PIXEL + 200;
			this.Height = BOARD_PIXEL + 100;
			gameMode = difficultyLevel;
			InitializeBoard();
			this.Paint += new PaintEventHandler(this.Form1_Paint);
			if (gameMode == 1)
			{
				bot = new BotEasy(Color.Red);
			}
			else if (gameMode == 2)
			{
				bot = new BotNormal(Color.Red);
			}
			else if (gameMode == 3)
			{
				bot = new BotHard(Color.Red);
			}
		}

		private void InitializeBoard()
		{
			for (int i = 0; i < BOARD_SIZE; i++)
			{
				for (int j = 0; j < BOARD_SIZE; j++)
				{
					Button btn = new Button();
					btn.Size = new Size(newButtonSize, newButtonSize);
					btn.Location = new Point(OFFSET + i * newCellSize - newButtonSize / 2, OFFSET + j * newCellSize - newButtonSize / 2);
					btn.Tag = new Point(i, j);
					btn.BackColor = Color.White;
					btn.FlatStyle = FlatStyle.Flat;
					btn.Paint += new PaintEventHandler(this.Button_Paint);
					btn.Click += new EventHandler(Button_Click);

					// Đặt tất cả các nút ban đầu là không hiển thị
					btn.Visible = false;

					boardButtons[i, j] = btn;
					this.Controls.Add(btn);
				}
			}
		}



		private void Button_Paint(object sender, PaintEventArgs e)
		{
			Button btn = sender as Button;
			Graphics g = e.Graphics;
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			g.Clear(btn.BackColor);

			using (Brush brush = new SolidBrush(btn.BackColor))
			{
				g.FillEllipse(brush, 0, 0, btn.Width, btn.Height);
			}
			using (Pen pen = new Pen(btn.ForeColor))
			{
				g.DrawEllipse(pen, 0, 0, btn.Width - 1, btn.Height - 1);
			}
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Pen blackPen = new Pen(Color.Black, 2);

			// Tăng kích thước CELL_SIZE lên 15% để vẽ lại bàn cờ
			int newCellSize = (int)(CELL_SIZE * 1.15);
			int newBoardPixel = newCellSize * (BOARD_SIZE - 1);

			for (int i = 0; i < BOARD_SIZE; i++)
			{
				g.DrawLine(blackPen, OFFSET, OFFSET + i * newCellSize, OFFSET + newBoardPixel, OFFSET + i * newCellSize);
				g.DrawLine(blackPen, OFFSET + i * newCellSize, OFFSET, OFFSET + i * newCellSize, OFFSET + newBoardPixel);

			}

			g.DrawLine(blackPen, OFFSET, OFFSET, OFFSET + newBoardPixel, OFFSET + newBoardPixel);
			g.DrawLine(blackPen, OFFSET + newBoardPixel, OFFSET, OFFSET, OFFSET + newBoardPixel);

			g.DrawLine(blackPen, OFFSET, OFFSET + 2 * newCellSize, OFFSET + 2 * newCellSize, OFFSET);
			g.DrawLine(blackPen, OFFSET + newBoardPixel, OFFSET + 2 * newCellSize, OFFSET + 2 * newCellSize, OFFSET);
			g.DrawLine(blackPen, OFFSET, OFFSET + 2 * newCellSize, OFFSET + 2 * newCellSize, OFFSET + newBoardPixel);
			g.DrawLine(blackPen, OFFSET + newBoardPixel, OFFSET + 2 * newCellSize, OFFSET + 2 * newCellSize, OFFSET + newBoardPixel);

		}


		private void UpdateTurnButton()
		{
			if (isBlueTurn)
			{
				turn.BackColor = Color.Blue;
				timer1.Start();
				timer2.Stop();

			}
			else
			{
				turn.BackColor = Color.Red;
				timer1.Stop();
				timer2.Start();
			}
		}
		private async void Button_Click(object sender, EventArgs e)
		{
			Button clickedButton = sender as Button;
			Point position = (Point)clickedButton.Tag;
			int x = position.X;
			int y = position.Y;

			// Kiểm tra lượt hiện tại
			if ((isBlueTurn && clickedButton.BackColor == Color.Red) || (!isBlueTurn && clickedButton.BackColor == Color.Blue))
			{
				MessageBox.Show("Không phải lượt của bạn!");
				return;
			}

			if (clickedButton.BackColor == Color.Red || clickedButton.BackColor == Color.Blue)
			{
				selectedChess = new chess(x, y, clickedButton.BackColor);
				ClearHighlightedMoves();
				ShowValidMoves(x, y);
			}
			else if (clickedButton.BackColor == Color.LightGreen && selectedChess != null)
			{
				// Người chơi thực hiện nước đi
				MovePiece(selectedChess.X, selectedChess.Y, x, y);
				selectedChess = null;
				isBlueTurn = !isBlueTurn; // Chuyển lượt sau khi di chuyển quân cờ
				UpdateTurnButton(); // Cập nhật màu của button hiển thị lượt

				// Kiểm tra điều kiện thắng sau lượt của người chơi
				CheckWinCondition();

				// Nếu là lượt của bot, bot thực hiện nước đi
				if (!isBlueTurn && gameMode > 0) // gameMode > 0 nghĩa là có bot
				{
					await Task.Delay(500); // Thêm khoảng trễ 0.5 giây trước khi bot đi
					MakeBotMove(); // Bot thực hiện nước đi
					isBlueTurn = !isBlueTurn; // Chuyển lượt lại cho người chơi
					UpdateTurnButton(); // Cập nhật màu của button hiển thị lượt

					// Kiểm tra điều kiện thắng sau lượt của bot
					CheckWinCondition();
				}
			}
		}



		private void MakeBotMove()
		{
			if (bot is BotEasy botEasy)
			{
				botEasy.MakeMove(boardPieces, boardButtons); // Pass both boardPieces and boardButtons
			}
			else if (bot is BotNormal botNormal)
			{
				botNormal.MakeMove(boardPieces, boardButtons); // Pass both boardPieces and boardButtons
			}
			else if (bot is BotHard botHard)
			{
				botHard.MakeMove(boardPieces, boardButtons);
				; // Pass both boardButtons and boardPieces
			}
		}


		private void ShowValidMoves(int x, int y)
		{
			// Đặt tất cả các ô không có quân cờ về trạng thái không hiển thị
			for (int i = 0; i < BOARD_SIZE; i++)
			{
				for (int j = 0; j < BOARD_SIZE; j++)
				{
					if (boardPieces[i, j] == null) // Nếu không có quân cờ
					{
						boardButtons[i, j].Visible = false;
					}
					else
					{
						boardButtons[i, j].Visible = true; // Các ô có quân cờ luôn hiển thị
					}
				}
			}

			List<Point> restrictedPoints = new List<Point>
	{
		new Point(0, 1), new Point(0, 3),
		new Point(1, 0), new Point(1, 2), new Point(1, 4),
		new Point(2, 1), new Point(2, 3),
		new Point(3, 0), new Point(3, 2), new Point(3, 4),
		new Point(4, 1), new Point(4, 3)
	};

			int[,] directionsStraight = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
			int[,] directionsDiagonal = { { 1, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 } };
			bool isRestricted = restrictedPoints.Contains(new Point(x, y));

			int[,] directions;
			if (isRestricted)
			{
				directions = directionsStraight;
			}
			else
			{
				directions = new int[directionsStraight.GetLength(0) + directionsDiagonal.GetLength(0), 2];
				Array.Copy(directionsStraight, 0, directions, 0, directionsStraight.Length);
				Array.Copy(directionsDiagonal, 0, directions, directionsStraight.Length, directionsDiagonal.Length);
			}

			// Hiển thị các ô hợp lệ để di chuyển
			for (int i = 0; i < directions.GetLength(0); i++)
			{
				int newX = x + directions[i, 0];
				int newY = y + directions[i, 1];

				if (IsValidMove(newX, newY))
				{
					boardButtons[newX, newY].BackColor = Color.LightGreen;
					boardButtons[newX, newY].Visible = true; // Hiển thị ô hợp lệ
				}
			}

			// Đảm bảo quân cờ được chọn vẫn hiển thị
			boardButtons[x, y].Visible = true;
		}



		private void MovePiece(int oldX, int oldY, int newX, int newY)
		{
			boardButtons[newX, newY].BackColor = boardButtons[oldX, oldY].BackColor;
			boardButtons[newX, newY].Visible = true; // Hiển thị ô mới
			boardButtons[oldX, oldY].BackColor = Color.White;
			boardButtons[oldX, oldY].Visible = false; // Ẩn ô cũ nếu không có quân cờ

			if (boardPieces[oldX, oldY] != null)
			{
				boardPieces[newX, newY] = boardPieces[oldX, oldY];
				boardPieces[oldX, oldY] = null;
				boardPieces[newX, newY].Move(newX, newY, boardPieces);
				boardPieces[newX, newY].Capture(newX, newY, boardPieces); // Kiểm tra gánh
			}
			boardPieces[newX, newY].CaptureSurroundedPieces(boardPieces); // Kiểm tra vây quân
			ClearHighlightedMoves();
			CheckWinCondition();
		}




		private void CheckWinCondition()
		{
			bool RedExists = false;
			bool BlueExists = false;
			// Duyệt hết bàn cờ
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					if (boardPieces[i, j] != null)
					{
						if (boardPieces[i, j].Color == Color.Red)
							RedExists = true;
						else if (boardPieces[i, j].Color == Color.Blue)
							BlueExists = true;
					}
				}
			}
			// Nếu 1 màu không còn, thì thông báo thắng
			if (!RedExists)
			{
				DialogResult result = MessageBox.Show("Bên xanh thắng! Bạn có muốn chơi lại không?", "Kết thúc trò chơi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					ResetPieces();
					s1 = 0;
					m1 = 5;
					s2 = 0;
					m2 = 5;
					minute1.Text = m1.ToString("D2");
					second1.Text = s1.ToString("D2");
					minute2.Text = m2.ToString("D2");
					second2.Text = s2.ToString("D2");
					turn.BackColor = Color.Blue;
				}
				else
				{
					Form2 form2= new Form2();
					form2.Show();
					this.Close(); // Đóng form hiện tại
				}
			}
			else if (!BlueExists)
			{
				DialogResult result = MessageBox.Show("Bên đỏ thắng! Bạn có muốn chơi lại không?", "Kết thúc trò chơi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					ResetPieces();
					s1 = 0;
					m1 = 5;
					s2 = 0;
					m2 = 5;
					minute1.Text = m1.ToString("D2");
					second1.Text = s1.ToString("D2");
					minute2.Text = m2.ToString("D2");
					second2.Text = s2.ToString("D2");
					turn.BackColor = Color.Blue;
				}
				else
				{
					Form2 form2 = new Form2();
					form2.Show();
					this.Close(); // Đóng form hiện tại
				}
			}

		}

		private void ClearHighlightedMoves()
		{
			for (int i = 0; i < BOARD_SIZE; i++)
			{
				for (int j = 0; j < BOARD_SIZE; j++)
				{
					if (boardButtons[i, j].BackColor == Color.LightGreen)
					{
						boardButtons[i, j].BackColor = Color.White;
						boardButtons[i, j].Visible = false; // Ẩn các ô không cần thiết
					}

					// Hiển thị lại các ô có quân cờ
					if (boardPieces[i, j] != null)
					{
						boardButtons[i, j].Visible = true;
					}
				}
			}
		}



		private bool IsValidMove(int x, int y)
		{
			return x >= 0 && x < BOARD_SIZE && y >= 0 && y < BOARD_SIZE &&
				   boardButtons[x, y].BackColor == Color.White;
		}


		private void ResetPieces()
		{
			for (int i = 0; i < BOARD_SIZE; i++)
			{
				for (int j = 0; j < BOARD_SIZE; j++)
				{
					boardButtons[i, j].BackColor = Color.White;
					boardButtons[i, j].Visible = false; // Ẩn tất cả các ô
					boardPieces[i, j] = null;
				}
			}

			// Đặt lại các quân cờ và hiển thị chúng
			for (int i = 0; i < BOARD_SIZE; i++)
			{
				boardButtons[i, 4].BackColor = Color.Blue;
				boardButtons[i, 4].Visible = true;
				boardPieces[i, 4] = new chess(i, 4, Color.Blue);
			}
			boardButtons[0, 3].BackColor = Color.Blue;
			boardButtons[0, 3].Visible = true;
			boardPieces[0, 3] = new chess(0, 3, Color.Blue);
			boardButtons[4, 3].BackColor = Color.Blue;
			boardButtons[4, 3].Visible = true;
			boardPieces[4, 3] = new chess(4, 3, Color.Blue);
			boardButtons[4, 2].BackColor = Color.Red;
			boardButtons[4, 2].Visible = true;
			boardPieces[4, 2] = new chess(4, 2, Color.Red);

			for (int i = 0; i < BOARD_SIZE; i++)
			{
				boardButtons[i, 0].BackColor = Color.Red;
				boardButtons[i, 0].Visible = true;
				boardPieces[i, 0] = new chess(i, 0, Color.Red);
			}
			boardButtons[0, 1].BackColor = Color.Red;
			boardButtons[0, 1].Visible = true;
			boardPieces[0, 1] = new chess(0, 1, Color.Red);
			boardButtons[4, 1].BackColor = Color.Red;
			boardButtons[4, 1].Visible = true;
			boardPieces[4, 1] = new chess(4, 1, Color.Red);
			boardButtons[0, 2].BackColor = Color.Blue;
			boardButtons[0, 2].Visible = true;
			boardPieces[0, 2] = new chess(0, 2, Color.Blue);
		}



		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show("Bạn muốn chơi lại không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (result == DialogResult.Yes)
			{
				ResetPieces();
				s1 = 0;
				m1 = 5;
				s2 = 0;
				m2 = 5;
				minute1.Text = m1.ToString("D2");
				second1.Text = s1.ToString("D2");
				minute2.Text = m2.ToString("D2");
				second2.Text = s2.ToString("D2");
				isBlueTurn = true; // Đặt lại lượt đi đầu tiên là của bên xanh
			

				turn.BackColor = Color.Blue;
			}
			
		}


		private void button2_Click(object sender, EventArgs e)
		{
			// Hiển thị hộp thoại xác nhận
			DialogResult result = MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				// Hiển thị Form2
				Form2 form2 = new Form2();
				form2.Show();

				// Ẩn Form1 thay vì đóng
				this.Hide();
			}
		}


		private void Form1_Load(object sender, EventArgs e)
		{
			s1 = 0;
			m1 = 5;
			s2 = 0;
			m2 = 5;
			ResetPieces();
			turn.BackColor = Color.Blue;
			timer1.Start();
			if (gameMode == 0)
			{
				mode.Text = "Chế độ : PVP";
			}
			else if (gameMode == 1)
			{
				mode.Text = "Chế độ : Dễ";
			}
			else if (gameMode == 2)
			{
				mode.Text = "Chế độ : Thường";
			}
			else if (gameMode == 3)
			{
				mode.Text = "Chế độ : Khó";
			}
		}
		private void label1_Click(object sender, EventArgs e)
		{

		}
		int m1 = 5, m2 = 5;
		int s1 = 0, s2 = 0;
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (s1 == 0)
			{
				if (m1 == 0)
				{
					timer1.Stop();
					DialogResult result = MessageBox.Show("Bên xanh hết thởi gian Bên đỏ thắng! Bạn có muốn chơi lại không?", "Kết thúc trò chơi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (result == DialogResult.Yes)
					{
						ResetPieces();
						s1 = 0;
						m1 = 5;
						s2 = 0;
						m2 = 5;
						minute1.Text = m1.ToString("D2");
						second1.Text = s1.ToString("D2");
						minute2.Text = m2.ToString("D2");
						second2.Text = s2.ToString("D2");
						turn.BackColor = Color.Blue;
					}
					else
					{
						Application.Exit(); // Thoát ứng dụng nếu chọn "No"
					}
				}
				m1--;
				s1 = 59;
			}
			else
			{
				s1--;
			}

			minute1.Text = m1.ToString("D2");
			second1.Text = s1.ToString("D2");
		}

		private void label7_Click(object sender, EventArgs e)
		{

		}

		private void second_Click(object sender, EventArgs e)
		{

		}

		private void timer2_Tick(object sender, EventArgs e)
		{
			if (s2 == 0)
			{
				if (m2 == 0)
				{
					timer2.Stop();
					DialogResult result = MessageBox.Show("Bên đỏ hết thởi gian Bên xanh thắng! Bạn có muốn chơi lại không?", "Kết thúc trò chơi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (result == DialogResult.Yes)
					{
						ResetPieces();
						s1 = 0;
						m1 = 5;
						s2 = 0;
						m2 = 5;
						minute1.Text = m1.ToString("D2");
						second1.Text = s1.ToString("D2");
						minute2.Text = m2.ToString("D2");
						second2.Text = s2.ToString("D2");
						turn.BackColor = Color.Blue;
					}
					else
					{
						Application.Exit(); // Thoát ứng dụng nếu chọn "No"
					}
				}
				m2--;
				s2 = 59;
			}
			else
			{
				s2--;
			}

			minute2.Text = m2.ToString("D2");
			second2.Text = s2.ToString("D2");
		}


	}
}
